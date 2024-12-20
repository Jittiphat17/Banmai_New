﻿Imports System.Data.OleDb
Imports System.Globalization
Imports Guna.UI2.WinForms
Imports Microsoft.Reporting.WinForms
Imports System.IO

Public Class frmBrrow
    ' ประกาศ Conn ไว้ที่เดียวสำหรับใช้ในฟอร์มนี้
    Private Conn As New OleDbConnection
    Private cmd As OleDbCommand
    Private strSQL As String

    ' ฟังก์ชันดึง path ฐานข้อมูลจาก config.ini
    Private Function GetDatabasePath() As String
        Dim iniPath As String = Path.Combine(Application.StartupPath, "config.ini")
        Dim dbPath As String = File.ReadAllLines(iniPath).FirstOrDefault(Function(line) line.StartsWith("Path="))

        If Not String.IsNullOrEmpty(dbPath) Then
            Return dbPath.Replace("Path=", "").Trim()
        Else
            Throw New Exception("ไม่พบ Path ของฐานข้อมูลใน config.ini")
        End If
    End Function

    Private Sub frmBrrow_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try
            ' ดึงค่า path จาก config.ini และสร้างการเชื่อมต่อฐานข้อมูล
            Dim dbPath As String = GetDatabasePath()
            Dim connStr As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath}"
            Conn = New OleDbConnection(connStr)

        Catch ex As Exception
            ' แสดงข้อความข้อผิดพลาดเมื่อไม่พบหรือเชื่อมต่อกับฐานข้อมูลไม่ได้
            MessageBox.Show($"เกิดข้อผิดพลาด: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Exit() ' ปิดโปรแกรมหากไม่สามารถเชื่อมต่อได้
        End Try
        SetupEventHandlers()
        Auto_id()
        ClearAllData()
        SetupGuna2DataGridView()
        SetupDateTimePicker()
        LoadAutoCompleteData()
        LoadAccountData()
        LoadPerMData()
        LoadGuaranteeTypes()

        ' ปิดการใช้งานปุ่ม Save ในตอนเริ่มต้น
        btnSave.Enabled = False

    End Sub


    Private Sub txtMoney_Leave(sender As Object, e As EventArgs) Handles txtMoney.Leave
        ' จัดการฟังก์ชันเมื่อผู้ใช้พิมพ์เสร็จ (เมื่อ TextBox เสียโฟกัส)
        If txtMoney.Text.Length > 0 Then
            ' ลบคอมม่าออกก่อนการจัดรูปแบบใหม่
            Dim valueWithoutComma As String = txtMoney.Text.Replace(",", "")

            ' ตรวจสอบว่าเป็นจำนวนเงินที่ถูกต้องหรือไม่
            Dim value As Decimal
            If Decimal.TryParse(valueWithoutComma, value) Then
                ' จัดรูปแบบและใส่คอมม่าพร้อมกับทศนิยม (2 ตำแหน่ง)
                txtMoney.Text = String.Format("{0:N2}", value)
            End If
        End If
    End Sub



    Private Sub LoadGuaranteeTypes()
        cbGuaranteeType.Items.Clear()
        cbGuaranteeType.Items.Add("เลือกการค้ำประกัน")
        cbGuaranteeType.Items.Add("ผู้ค้ำประกัน")
        cbGuaranteeType.Items.Add("เงินในบัญชี")
        cbGuaranteeType.Items.Add("อื่น ๆ")
        cbGuaranteeType.SelectedIndex = 0 ' ตั้งค่าเป็นค่าเริ่มต้น
    End Sub

    Private Sub SetupGuna2DataGridView()
        ' ตั้งค่าฟอนต์ของ DataGridView
        guna2DataGridView1.Font = New Font("FC Minimal", 20, FontStyle.Bold)
        guna2DataGridView1.Columns.Clear()

        ' ชื่อคอลัมน์ที่ต้องการแสดง
        Dim columnNames As String() = {"เลขที่สัญญา", "ผู้กู้", "รายละเอียดผู้กู้", "จำนวนเงินกู้", "แหล่งจ่าย",
                                "จำนวนเดือน", "ดอกเบี้ย", "วันที่ทำรายการ", "ผู้ค้ำที่ 1", "ผู้ค้ำที่ 2",
                                "ผู้ค้ำที่ 3", "ผ่อนชำระต่อเดือน"}

        ' เพิ่มคอลัมน์ให้กับ DataGridView
        For Each colName As String In columnNames
            Dim column As New DataGridViewTextBoxColumn()
            column.HeaderText = colName
            column.Name = colName

            ' จัดรูปแบบคอลัมน์ที่เกี่ยวข้องกับตัวเลขหรือจำนวนเงิน
            If colName = "จำนวนเงินกู้" OrElse colName = "ผ่อนชำระต่อเดือน" Then
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                column.DefaultCellStyle.Format = "N2" ' แสดงจำนวนเงินด้วยคอมม่า
            End If

            ' เพิ่มคอลัมน์เข้าไปใน DataGridView
            guna2DataGridView1.Columns.Add(column)
        Next

        ' ตั้งค่าเพิ่มเติมให้ DataGridView
        guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill ' ให้คอลัมน์ขยายตามขนาดของหน้าจอ
        guna2DataGridView1.RowTemplate.Height = 40 ' กำหนดความสูงของแถว

        ' กำหนดรูปแบบตัวอักษรในส่วนหัวตาราง
        guna2DataGridView1.ColumnHeadersDefaultCellStyle.Font = New Font("FC Minimal", 22, FontStyle.Bold) ' กำหนดฟอนต์ FC Minimal ขนาด 18 และเป็นตัวหนา
        guna2DataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' จัดหัวตารางให้อยู่ตรงกลาง
    End Sub



    Private Sub SetupEventHandlers()
        AddHandler txtSearch.TextChanged, AddressOf txtSearch_TextChanged
        AddHandler txtSearch1.TextChanged, AddressOf txtSearch_TextChanged
        AddHandler txtSearch2.TextChanged, AddressOf txtSearch_TextChanged
        AddHandler txtSearch3.TextChanged, AddressOf txtSearch_TextChanged
        AddHandler chkGuarantor.CheckedChanged, AddressOf chkGuarantor_CheckedChanged
    End Sub

    Private Sub LoadAccountData()
        Try
            If Conn.State = ConnectionState.Closed Then Conn.Open() ' ตรวจสอบการเปิดเชื่อมต่อ
            Using cmd As New OleDbCommand("SELECT acc_id, acc_name FROM Account", Conn)
                Using reader As OleDbDataReader = cmd.ExecuteReader()
                    Dim accountDict As New Dictionary(Of String, String)()
                    If cbAccount.Items.Count = 0 Then
                        cbAccount.Items.Add("เลือกบัญชี")
                    End If
                    While reader.Read()
                        Dim accId As String = reader("acc_id").ToString()
                        Dim accName As String = reader("acc_name").ToString()
                        If Not accountDict.ContainsKey(accId) Then
                            accountDict.Add(accId, accName)
                            cbAccount.Items.Add(accName)
                        End If
                    End While
                    cbAccount.Tag = accountDict
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If Conn.State = ConnectionState.Open Then Conn.Close() ' ปิดการเชื่อมต่อเมื่อใช้งานเสร็จสิ้น
        End Try
    End Sub

    Private Sub LoadPerMData()
        cbPerM.Items.Clear()
        cbPerM.Items.AddRange(New String() {"เลือกจำนวนเดือน", "6เดือน", "12เดือน", "18เดือน", "24เดือน", "36เดือน", "48เดือน", "60เดือน"})
        cbPerM.SelectedIndex = 0
    End Sub

    Private Sub LoadAutoCompleteData()
        Try
            If Conn.State = ConnectionState.Closed Then Conn.Open() ' ตรวจสอบการเปิดเชื่อมต่อ
            Using cmd As New OleDbCommand("SELECT m_name FROM Member", Conn)
                Using reader As OleDbDataReader = cmd.ExecuteReader()
                    Dim autoComplete As New AutoCompleteStringCollection()
                    While reader.Read()
                        autoComplete.Add(reader("m_name").ToString())
                    End While
                    Dim textBoxes As Guna2TextBox() = {txtSearch, txtSearch1, txtSearch2, txtSearch3}
                    For Each tb As Guna2TextBox In textBoxes
                        tb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
                        tb.AutoCompleteSource = AutoCompleteSource.CustomSource
                        tb.AutoCompleteCustomSource = autoComplete
                    Next
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If Conn.State = ConnectionState.Open Then Conn.Close() ' ปิดการเชื่อมต่อเมื่อใช้งานเสร็จสิ้น
        End Try
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs)
        Dim tb As Guna2TextBox = CType(sender, Guna2TextBox)
        If String.IsNullOrWhiteSpace(tb.Text) Then
            Dim detailTextBox As Guna2TextBox = FindDetailTextBoxForSearchBox(tb)
            If detailTextBox IsNot Nothing Then detailTextBox.Text = String.Empty
            UpdateGuarantorStatus()
            Exit Sub
        End If

        Try
            If Conn.State = ConnectionState.Closed Then Conn.Open()
            Using cmd As New OleDbCommand("SELECT m_id, m_name, m_address, m_tel FROM Member WHERE m_name LIKE @m_name", Conn)
                cmd.Parameters.AddWithValue("@m_name", "%" & tb.Text & "%")
                Using reader As OleDbDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        Dim detail As String = String.Format("รหัสสมาชิก: {0}, ชื่อ-นามสกุล: {1}, ที่อยู่: {2}, เบอร์โทรติดต่อ: {3}",
                                                         reader("m_id"), reader("m_name"), reader("m_address"), reader("m_tel"))
                        Dim detailTextBox As Guna2TextBox = FindDetailTextBoxForSearchBox(tb)
                        If detailTextBox IsNot Nothing Then detailTextBox.Text = detail
                    Else
                        Dim detailTextBox As Guna2TextBox = FindDetailTextBoxForSearchBox(tb)
                        If detailTextBox IsNot Nothing Then detailTextBox.Text = String.Empty
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If Conn.State = ConnectionState.Open Then Conn.Close()
        End Try

        UpdateGuarantorStatus()
    End Sub

    Private Function FindDetailTextBoxForSearchBox(searchBox As Guna2TextBox) As Guna2TextBox
        Select Case searchBox.Name
            Case "txtSearch"
                Return txtDetail
            Case "txtSearch1"
                Return txtDetail1
            Case "txtSearch2"
                Return txtDetail2
            Case "txtSearch3"
                Return txtDetail3
            Case Else
                Return Nothing
        End Select
    End Function

    Private Sub Auto_id()
        Try
            If Conn.State = ConnectionState.Closed Then Conn.Open()
            Using cmd As New OleDbCommand("SELECT MAX(con_id) FROM Contract", Conn)
                Dim result As Object = cmd.ExecuteScalar()
                If IsDBNull(result) OrElse result Is Nothing Then
                    txtCid.Text = "0001" ' ถ้ายังไม่มีข้อมูล ให้เริ่มจาก 0001
                Else
                    ' ตรวจสอบและเพิ่มค่า ID
                    Dim newId As Integer = Convert.ToInt32(result) + 1
                    txtCid.Text = newId.ToString("D4") ' แปลงเป็นรูปแบบ D4 เพื่อให้ได้ 4 หลัก
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If Conn.State = ConnectionState.Open Then Conn.Close()
        End Try
    End Sub

    Private Sub ClearAllData()
        ' กำหนด TextBox ที่จะถูกล้างข้อมูล
        Dim textBoxesToClear As Guna2TextBox() = {txtSearch, txtSearch1, txtSearch2, txtSearch3, txtDetail, txtDetail1, txtDetail2, txtDetail3, txtMoney, txtPercen}
        For Each tb As Guna2TextBox In textBoxesToClear
            tb.Clear()
        Next

        ' รีเซ็ต ComboBox และ DateTimePicker
        If cbAccount.Items.Count > 0 Then cbAccount.SelectedIndex = 0
        LoadPerMData()
        dtpBirth.Value = DateTime.Today

        ' รีเซ็ตการเลือกผู้ค้ำประกัน
        Dim hasGuarantor As Boolean = False
        chkGuarantor.Checked = hasGuarantor

        ' ปิดการใช้งานช่องผู้ค้ำประกัน
        Dim guarantorFields As Guna2TextBox() = {txtSearch1, txtSearch2, txtSearch3, txtDetail1, txtDetail2, txtDetail3}
        For Each field As Guna2TextBox In guarantorFields
            field.Enabled = hasGuarantor
        Next

        ' ไม่ล้าง txtCid เพื่อให้คงค่าปัจจุบันหรือให้ Auto_id กำหนดใหม่หลังจากนี้
    End Sub


    Private Sub SetupDateTimePicker()
        dtpBirth.Format = DateTimePickerFormat.Custom
        dtpBirth.CustomFormat = "dd/MM/yyyy"
        dtpBirth.Value = DateTime.Now
    End Sub

    Private Function GetSavingsBalance(memberId As Integer) As Decimal
        Dim savingsBalance As Decimal = 0
        Try
            If Conn.State = ConnectionState.Closed Then Conn.Open()

            Dim strSQL As String = "SELECT SUM(ind_amount) AS TotalSavings FROM Income_Details WHERE m_id = @memberId AND ind_accname = 'เงินฝากสัจจะ'"
            Using cmd As New OleDbCommand(strSQL, Conn)
                cmd.Parameters.AddWithValue("@memberId", memberId)
                Dim result As Object = cmd.ExecuteScalar()
                If result IsNot DBNull.Value Then
                    savingsBalance = Convert.ToDecimal(result)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If Conn.State = ConnectionState.Open Then Conn.Close()
        End Try
        Return savingsBalance
    End Function

    Private Function CheckGuaranteeWithSavings() As Boolean
        If cbGuaranteeType.SelectedItem.ToString() = "เงินในบัญชี" Then
            Try
                Dim loanAmount As Decimal
                If Decimal.TryParse(txtMoney.Text, loanAmount) Then
                    Dim borrowerId As Integer = GetMemberIdByName(txtSearch.Text)
                    If borrowerId = -1 Then
                        MessageBox.Show("ไม่พบข้อมูลผู้กู้", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return False
                    End If

                    Dim savingsBalance As Decimal = GetSavingsBalance(borrowerId)

                    If savingsBalance >= loanAmount Then
                        MessageBox.Show("สามารถใช้เงินในบัญชีค้ำประกันได้", "การค้ำประกัน", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return True
                    Else
                        MessageBox.Show("เงินในบัญชีไม่เพียงพอสำหรับการค้ำประกัน", "การค้ำประกัน", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return False
                    End If
                Else
                    MessageBox.Show("กรุณากรอกจำนวนเงินที่ต้องการกู้ให้ถูกต้อง", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If
            Catch ex As Exception
                MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End Try
        End If

        ' If the guarantee type is not "เงินในบัญชี", the check is not applicable
        Return True
    End Function


    Private Sub cbGuaranteeType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbGuaranteeType.SelectedIndexChanged
        If cbGuaranteeType.SelectedItem.ToString() = "เงินในบัญชี" Then
            cbAccount.SelectedItem = "บัญชีเงินสัจจะ"
            cbAccount.Enabled = False
            txtSearch1.Enabled = False
            txtSearch2.Enabled = False
            txtSearch3.Enabled = False

            txtSearch1.Clear()
            txtSearch2.Clear()
            txtSearch3.Clear()

            chkGuarantor.Checked = False

        ElseIf cbGuaranteeType.SelectedItem.ToString() = "ผู้ค้ำประกัน" Then
            txtSearch1.Enabled = True
            txtSearch2.Enabled = True
            txtSearch3.Enabled = True
            cbAccount.Enabled = True

            chkGuarantor.Checked = True

            If txtSearch1.Text = txtSearch.Text OrElse txtSearch2.Text = txtSearch.Text OrElse txtSearch3.Text = txtSearch.Text Then
                MessageBox.Show("ผู้ค้ำประกันไม่สามารถเป็นคนเดียวกับคนกู้ได้", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                If txtSearch1.Text = txtSearch.Text Then txtSearch1.Clear()
                If txtSearch2.Text = txtSearch.Text Then txtSearch2.Clear()
                If txtSearch3.Text = txtSearch.Text Then txtSearch3.Clear()
            End If

        Else
            cbAccount.Enabled = True
            txtSearch1.Enabled = False
            txtSearch2.Enabled = False
            txtSearch3.Enabled = False

            txtSearch1.Clear()
            txtSearch2.Clear()
            txtSearch3.Clear()

            chkGuarantor.Checked = False
        End If
    End Sub
    ' ฟังก์ชันที่ใช้สำหรับเคลียร์ข้อมูลในฟอร์ม
    Private Sub ResetForm()
        ' เคลียร์ TextBox
        txtSearch.Clear()
        txtDetail.Clear()
        txtMoney.Clear()
        txtPercen.Clear()
        txtSearch1.Clear()
        txtSearch2.Clear()
        txtSearch3.Clear()

        ' รีเซ็ต ComboBox
        cbAccount.SelectedIndex = 0
        cbGuaranteeType.SelectedIndex = 0
        cbPerM.SelectedIndex = 0

        ' รีเซ็ต DateTimePicker
        dtpBirth.Value = DateTime.Today

        ' เคลียร์ DataGridView
        guna2DataGridView1.Rows.Clear()

        ' รีเซ็ต txtCid โดยเรียกใช้ Auto_id เพื่อสร้าง ID ใหม่
        Auto_id()

        ' ปิดการใช้งานปุ่มบันทึก
        btnSave.Enabled = False
    End Sub

    Private Function IsDataComplete() As Boolean
        ' ตรวจสอบชื่อผู้กู้
        If String.IsNullOrWhiteSpace(txtSearch.Text) Then
            MessageBox.Show("กรุณากรอกชื่อผู้กู้", "ข้อมูลไม่ครบ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtSearch.Focus()
            Return False
        End If

        ' ตรวจสอบจำนวนเงินกู้
        Dim loanAmount As Decimal
        If Not Decimal.TryParse(txtMoney.Text.Replace(",", ""), loanAmount) OrElse loanAmount <= 0 Then
            MessageBox.Show("กรุณากรอกจำนวนเงินกู้ที่ถูกต้อง", "ข้อมูลไม่ครบ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtMoney.Focus()
            Return False
        End If

        ' ตรวจสอบการค้ำประกัน
        If cbGuaranteeType.SelectedIndex = 0 Then
            MessageBox.Show("กรุณาเลือกการค้ำประกัน", "ข้อมูลไม่ครบ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cbGuaranteeType.Focus()
            Return False
        End If

        ' ตรวจสอบจำนวนเดือน
        If cbPerM.SelectedIndex = 0 Then
            MessageBox.Show("กรุณาเลือกจำนวนเดือน", "ข้อมูลไม่ครบ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            cbPerM.Focus()
            Return False
        End If

        ' ตรวจสอบอัตราดอกเบี้ย
        Dim interestRate As Decimal
        If Not Decimal.TryParse(txtPercen.Text, interestRate) OrElse interestRate <= 0 Then
            MessageBox.Show("กรุณากรอกอัตราดอกเบี้ยที่ถูกต้อง", "ข้อมูลไม่ครบ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtPercen.Focus()
            Return False
        End If

        ' ตรวจสอบประเภทการค้ำประกันว่าเป็น "เงินในบัญชี" หรือไม่
        Dim guaranteeType As String = cbGuaranteeType.SelectedItem.ToString()
        If guaranteeType = "เงินในบัญชี" Then
            ' ถ้าเลือกการค้ำประกันเป็น "เงินในบัญชี" ข้ามการตรวจสอบผู้ค้ำประกัน
            Return True
        End If

        ' ตรวจสอบจำนวนผู้ค้ำตามจำนวนเงินกู้
        Dim guarantorCount As Integer = 0
        If Not String.IsNullOrWhiteSpace(txtSearch1.Text) Then guarantorCount += 1
        If Not String.IsNullOrWhiteSpace(txtSearch2.Text) Then guarantorCount += 1
        If Not String.IsNullOrWhiteSpace(txtSearch3.Text) Then guarantorCount += 1

        ' ตรวจสอบเงื่อนไขจำนวนผู้ค้ำ
        If loanAmount <= 10000 AndAlso guarantorCount < 1 Then
            MessageBox.Show("จำนวนเงินกู้ไม่เกิน 10,000 บาท ต้องมีผู้ค้ำประกันอย่างน้อย 1 คน", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        ElseIf loanAmount > 10000 AndAlso loanAmount < 100000 AndAlso guarantorCount < 2 Then
            MessageBox.Show("จำนวนเงินกู้เกิน 10,000 บาท แต่ไม่ถึง 100,000 บาท ต้องมีผู้ค้ำประกันอย่างน้อย 2 คน", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        ElseIf loanAmount >= 100000 AndAlso guarantorCount < 3 Then
            MessageBox.Show("จำนวนเงินกู้เกิน 100,000 บาท ต้องมีผู้ค้ำประกันอย่างน้อย 3 คน", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        ' ถ้าทุกอย่างถูกต้อง
        Return True
    End Function



    Private Sub UpdateGuarantorStatus()
        Dim hasGuarantor As Boolean = Not String.IsNullOrWhiteSpace(txtSearch1.Text) OrElse
                                   Not String.IsNullOrWhiteSpace(txtSearch2.Text) OrElse
                                   Not String.IsNullOrWhiteSpace(txtSearch3.Text)

        chkGuarantor.Checked = hasGuarantor

        txtSearch1.Enabled = hasGuarantor
        txtSearch2.Enabled = hasGuarantor
        txtSearch3.Enabled = hasGuarantor
        txtDetail1.Enabled = hasGuarantor
        txtDetail2.Enabled = hasGuarantor
        txtDetail3.Enabled = hasGuarantor
    End Sub

    Private Sub chkGuarantor_CheckedChanged(sender As Object, e As EventArgs)
        txtSearch1.Enabled = chkGuarantor.Checked
        txtSearch2.Enabled = chkGuarantor.Checked
        txtSearch3.Enabled = chkGuarantor.Checked
        txtDetail1.Enabled = chkGuarantor.Checked
        txtDetail2.Enabled = chkGuarantor.Checked
        txtDetail3.Enabled = chkGuarantor.Checked

        If Not chkGuarantor.Checked Then
            txtSearch1.Clear()
            txtSearch2.Clear()
            txtSearch3.Clear()
            txtDetail1.Clear()
            txtDetail2.Clear()
            txtDetail3.Clear()
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Try
            ' ตรวจสอบว่าข้อมูลครบหรือไม่
            If Not IsDataComplete() Then
                MessageBox.Show("กรุณากรอกข้อมูลให้ครบถ้วน", "ข้อมูลไม่ครบ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim borrowerName As String = txtSearch.Text ' ชื่อผู้กู้จาก TextBox
            Dim isDuplicateBorrower As Boolean = False
            Dim isDuplicateWithGuarantor As Boolean = False

            ' ตรวจสอบชื่อผู้กู้ซ้ำกันใน DataGridView
            For Each row As DataGridViewRow In guna2DataGridView1.Rows
                If Not row.IsNewRow Then
                    If row.Cells("ผู้กู้").Value.ToString() = borrowerName Then
                        isDuplicateBorrower = True
                        Exit For
                    End If
                End If
            Next

            ' ถ้าพบชื่อผู้กู้ซ้ำ ให้แสดงข้อความแจ้งเตือนและไม่เพิ่มข้อมูล
            If isDuplicateBorrower Then
                MessageBox.Show("มีชื่อผู้กู้นี้อยู่ในรายการแล้ว", "ชื่อซ้ำ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' ตรวจสอบว่าชื่อผู้กู้ซ้ำกับชื่อผู้ค้ำหรือไม่
            If borrowerName = txtSearch1.Text OrElse borrowerName = txtSearch2.Text OrElse borrowerName = txtSearch3.Text Then
                isDuplicateWithGuarantor = True
            End If

            ' ถ้าพบชื่อผู้กู้ซ้ำกับชื่อผู้ค้ำ ให้แสดงข้อความแจ้งเตือนและไม่เพิ่มข้อมูล
            If isDuplicateWithGuarantor Then
                MessageBox.Show("ชื่อผู้กู้ไม่สามารถเป็นผู้ค้ำประกันได้", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' เช็คการค้ำประกันด้วยเงินในบัญชีก่อนเพิ่มข้อมูล
            If Not CheckGuaranteeWithSavings() Then
                ' ถ้าเงินในบัญชีไม่พอสำหรับการค้ำประกัน ให้หยุดการดำเนินการ
                Return
            End If

            ' ดำเนินการเพิ่มข้อมูลใหม่ใน DataGridView
            Dim principal As Decimal = Decimal.Parse(txtMoney.Text.Replace(",", ""))
            Dim loanDate As DateTime = dtpBirth.Value
            Dim interestRate As Decimal = Decimal.Parse(txtPercen.Text)
            Dim totalPayments As Integer = Integer.Parse(cbPerM.SelectedItem.ToString().Replace("เดือน", "").Trim())

            Dim monthlyPayment As Decimal = CalculateMonthlyPayment(principal, interestRate, totalPayments)

            Dim accountDict As Dictionary(Of String, String) = CType(cbAccount.Tag, Dictionary(Of String, String))
            Dim acc_id As String = accountDict.FirstOrDefault(Function(x) x.Value = cbAccount.SelectedItem.ToString()).Key

            Dim rowData As Object() = New Object() {
            txtCid.Text,
            txtSearch.Text,
            txtDetail.Text,
            principal.ToString("N2"),
            cbAccount.SelectedItem.ToString(),
            cbPerM.SelectedItem.ToString(),
            txtPercen.Text,
            dtpBirth.Value.ToString("dd/MM/yyyy"),
            If(chkGuarantor.Checked, txtSearch1.Text, ""),
            If(chkGuarantor.Checked, txtSearch2.Text, ""),
            If(chkGuarantor.Checked, txtSearch3.Text, ""),
            monthlyPayment.ToString("N2")
        }

            ' เพิ่มข้อมูลใหม่ลงใน DataGridView
            guna2DataGridView1.Rows.Add(rowData)

            ' ตรวจสอบว่ามีข้อมูลใน DataGridView หรือไม่ หากมีให้เปิดการใช้งานปุ่มบันทึก
            If guna2DataGridView1.Rows.Count > 0 Then
                btnSave.Enabled = True
            End If

            ' ล้างข้อมูลในฟอร์ม
            ClearAllData()
            Auto_id()

        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาด: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    ' ฟังก์ชันเพื่อตรวจสอบว่าผู้ค้ำประกันมีสัญญาที่ค้างชำระอยู่หรือไม่
    Private Function GetActiveGuaranteeCount(guarantorId As Integer) As Integer
        Dim activeContracts As Integer = 0
        Try
            If Conn.State = ConnectionState.Closed Then Conn.Open()

            ' คำสั่ง SQL เพื่อตรวจสอบจำนวนสัญญาที่ยังค้างอยู่
            Dim strSQL As String = "SELECT COUNT(*) FROM Guarantor g " &
                               "INNER JOIN Contract c ON g.con_id = c.con_id " &
                               "WHERE g.m_id = @guarantorId AND c.con_amount > 0"
            Using cmd As New OleDbCommand(strSQL, Conn)
                cmd.Parameters.AddWithValue("@guarantorId", guarantorId)
                activeContracts = Convert.ToInt32(cmd.ExecuteScalar())
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If Conn.State = ConnectionState.Open Then Conn.Close()
        End Try

        Return activeContracts
    End Function

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            ' ดึง path ของฐานข้อมูลจาก config.ini
            Dim dbPath As String = GetDatabasePath()
            Dim connStr As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath}"

            Using conn As New OleDbConnection(connStr)
                conn.Open() ' เปิดการเชื่อมต่อฐานข้อมูล

                ' วนลูปเพื่อบันทึกข้อมูลจาก DataGridView
                For Each row As DataGridViewRow In guna2DataGridView1.Rows
                    If Not row.IsNewRow Then
                        ' ดึงข้อมูลจากแต่ละแถวใน DataGridView
                        Dim borrowerName As String = row.Cells("ผู้กู้").Value.ToString()
                        Dim loanAmount As Decimal = Decimal.Parse(row.Cells("จำนวนเงินกู้").Value.ToString().Replace(" บาท", "").Replace(",", ""))
                        Dim accountDict As Dictionary(Of String, String) = CType(cbAccount.Tag, Dictionary(Of String, String))
                        Dim acc_id As String = accountDict.FirstOrDefault(Function(x) x.Value = row.Cells("แหล่งจ่าย").Value.ToString()).Key
                        Dim periodText As String = row.Cells("จำนวนเดือน").Value.ToString()
                        Dim interest As Decimal = Decimal.Parse(row.Cells("ดอกเบี้ย").Value.ToString())
                        Dim transactionDateStr As String = row.Cells("วันที่ทำรายการ").Value.ToString()

                        ' แปลงจำนวนงวดและวันที่ทำรายการ
                        Dim period As Integer = Integer.Parse(periodText.Replace("เดือน", ""))
                        Dim transactionDate As DateTime
                        If Not DateTime.TryParseExact(transactionDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, transactionDate) Then
                            MessageBox.Show("รูปแบบวันที่ไม่ถูกต้อง: " & transactionDateStr, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return
                        End If

                        ' ดึงข้อมูลผู้กู้
                        Dim borrowerId As Integer = GetMemberIdByName(borrowerName)
                        If borrowerId = -1 Then
                            MessageBox.Show("ไม่พบข้อมูลผู้กู้", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return
                        End If

                        ' บันทึกข้อมูลสัญญาเงินกู้
                        Dim strSQL As String = "INSERT INTO Contract (m_id, con_details, con_amount, con_interest, con_permonth, con_date, acc_id, con_GuaranteeType) VALUES (@m_id, @con_details, @con_amount, @con_interest, @con_permonth, @con_date, @acc_id, @con_GuaranteeType)"
                        Using cmd As New OleDbCommand(strSQL, conn)
                            cmd.Parameters.AddWithValue("@m_id", borrowerId)
                            cmd.Parameters.AddWithValue("@con_details", row.Cells("รายละเอียดผู้กู้").Value.ToString())
                            cmd.Parameters.AddWithValue("@con_amount", loanAmount)
                            cmd.Parameters.AddWithValue("@con_interest", interest)
                            cmd.Parameters.AddWithValue("@con_permonth", period)
                            cmd.Parameters.AddWithValue("@con_date", transactionDate)
                            cmd.Parameters.AddWithValue("@acc_id", acc_id)
                            cmd.Parameters.AddWithValue("@con_GuaranteeType", cbGuaranteeType.SelectedItem.ToString())
                            cmd.ExecuteNonQuery()

                            ' ดึงค่า con_id ที่ถูกสร้างขึ้นมา
                            cmd.CommandText = "SELECT @@IDENTITY"
                            Dim con_id As Integer = Convert.ToInt32(cmd.ExecuteScalar())

                            ' บันทึกข้อมูลผู้ค้ำ
                            Dim guarantorNames As String() = {row.Cells("ผู้ค้ำที่ 1").Value.ToString(), row.Cells("ผู้ค้ำที่ 2").Value.ToString(), row.Cells("ผู้ค้ำที่ 3").Value.ToString()}
                            For Each guarantorName As String In guarantorNames
                                If Not String.IsNullOrEmpty(guarantorName) Then
                                    Dim guarantorId As Integer = GetMemberIdByName(guarantorName)
                                    If guarantorId <> -1 Then
                                        strSQL = "INSERT INTO Guarantor (con_id, m_id) VALUES (@con_id, @m_id)"
                                        Using guarantorCmd As New OleDbCommand(strSQL, conn)
                                            guarantorCmd.Parameters.AddWithValue("@con_id", con_id)
                                            guarantorCmd.Parameters.AddWithValue("@m_id", guarantorId)
                                            guarantorCmd.ExecuteNonQuery()
                                        End Using
                                    Else
                                        MessageBox.Show("ไม่พบข้อมูลผู้ค้ำ: " & guarantorName, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                    End If
                                End If
                            Next

                            ' คำนวณและบันทึกข้อมูลการชำระเงิน
                            Dim monthlyPayment As Decimal = CalculateMonthlyPayment(loanAmount, interest, period)
                            Dim totalInterest As Decimal = loanAmount * (interest / 100) * period
                            Dim monthlyPrincipal As Decimal = Math.Round(loanAmount / period, 2)
                            Dim monthlyInterest As Decimal = Math.Round(totalInterest / period, 2)

                            For i As Integer = 1 To period
                                Dim paymentDate As DateTime = transactionDate.AddMonths(i)

                                If i = period Then
                                    monthlyPrincipal = loanAmount - (monthlyPrincipal * (period - 1))
                                    monthlyInterest = totalInterest - (monthlyInterest * (period - 1))
                                    monthlyPayment = monthlyPrincipal + monthlyInterest
                                End If

                                ' บันทึกยอดเงินกู้ทั้งหมดในฟิลด์ payment_Principal
                                strSQL = "INSERT INTO Payment (con_id, payment_date, payment_amount, payment_prin, payment_interest, status_id, payment_period, payment_Mprincipal) VALUES (@con_id, @payment_date, @payment_amount, @payment_prin, @payment_interest, @status_id, @payment_period, @payment_Mprincipal)"
                                Using paymentCmd As New OleDbCommand(strSQL, conn)
                                    paymentCmd.Parameters.AddWithValue("@con_id", con_id)
                                    paymentCmd.Parameters.AddWithValue("@payment_date", paymentDate)
                                    paymentCmd.Parameters.AddWithValue("@payment_amount", monthlyPayment)
                                    paymentCmd.Parameters.AddWithValue("@payment_prin", monthlyPrincipal)
                                    paymentCmd.Parameters.AddWithValue("@payment_interest", monthlyInterest)
                                    paymentCmd.Parameters.AddWithValue("@status_id", 1)
                                    paymentCmd.Parameters.AddWithValue("@payment_period", i)
                                    paymentCmd.Parameters.AddWithValue("@payment_Mprincipal", loanAmount)
                                    paymentCmd.ExecuteNonQuery()
                                End Using
                            Next

                            ' บันทึกข้อมูลในตาราง Account_Details
                            strSQL = "INSERT INTO Account_Details (acc_id, m_id) VALUES (@acc_id, @m_id)"
                            Using accountDetailsCmd As New OleDbCommand(strSQL, conn)
                                accountDetailsCmd.Parameters.AddWithValue("@acc_id", acc_id)
                                accountDetailsCmd.Parameters.AddWithValue("@m_id", borrowerId)
                                accountDetailsCmd.ExecuteNonQuery()
                            End Using

                            ' บันทึกข้อมูลในตาราง Expense และ Expense_Details
                            strSQL = "INSERT INTO Expense (ex_name, ex_detail, ex_description, ex_date, ex_amount, acc_id) VALUES (@ex_name, @ex_detail, @ex_description, @ex_date, @ex_amount, @acc_id)"
                            Using cmdExpense As New OleDbCommand(strSQL, conn)
                                cmdExpense.Parameters.AddWithValue("@ex_name", borrowerName)
                                cmdExpense.Parameters.AddWithValue("@ex_detail", row.Cells("รายละเอียดผู้กู้").Value.ToString())
                                cmdExpense.Parameters.AddWithValue("@ex_description", "เงินกู้ยืมสำหรับ " & borrowerName)
                                cmdExpense.Parameters.AddWithValue("@ex_date", transactionDate)
                                cmdExpense.Parameters.AddWithValue("@ex_amount", loanAmount)
                                cmdExpense.Parameters.AddWithValue("@acc_id", acc_id)

                                cmdExpense.ExecuteNonQuery()

                                ' ดึงค่า ex_id ที่ถูกสร้างขึ้นมา
                                cmdExpense.CommandText = "SELECT @@IDENTITY"
                                Dim ex_id As Integer = Convert.ToInt32(cmdExpense.ExecuteScalar())

                                ' บันทึกข้อมูลในตาราง Expense_Details
                                Dim accountNameFromGrid As String = "กู้เงิน " & row.Cells("แหล่งจ่าย").Value.ToString()
                                strSQL = "INSERT INTO Expense_Details (exd_nameacc, exd_amount, ex_id, m_id, exd_date, acc_id) VALUES (@exd_nameacc, @exd_amount, @ex_id, @m_id, @exd_date, @acc_id)"
                                Using cmdExpenseDetails As New OleDbCommand(strSQL, conn)
                                    cmdExpenseDetails.Parameters.AddWithValue("@exd_nameacc", accountNameFromGrid)
                                    cmdExpenseDetails.Parameters.AddWithValue("@exd_amount", loanAmount)
                                    cmdExpenseDetails.Parameters.AddWithValue("@ex_id", ex_id)
                                    cmdExpenseDetails.Parameters.AddWithValue("@m_id", borrowerId)
                                    cmdExpenseDetails.Parameters.AddWithValue("@exd_date", transactionDate)
                                    cmdExpenseDetails.Parameters.AddWithValue("@acc_id", acc_id)
                                    cmdExpenseDetails.ExecuteNonQuery()
                                End Using
                            End Using
                        End Using
                    End If
                Next

                ' แสดงข้อความเมื่อบันทึกข้อมูลเสร็จสิ้น
                MessageBox.Show("บันทึกข้อมูลสำเร็จ", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information)

            End Using ' การปิดการเชื่อมต่อจะเกิดขึ้นที่นี่ โดยอัตโนมัติเมื่อจบ Using

            ' รีเซ็ตฟอร์มหลังจากบันทึกเสร็จ
            ResetForm()

        Catch ex As Exception
            ' แสดงข้อความข้อผิดพลาด
            MessageBox.Show("ข้อผิดพลาด: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Function GetMemberIdByName(name As String) As Integer
        strSQL = "SELECT m_id FROM Member WHERE m_name = @m_name"
        Using cmd As New OleDbCommand(strSQL, Conn)
            cmd.Parameters.AddWithValue("@m_name", name)

            Try
                If Conn.State = ConnectionState.Closed Then Conn.Open()
                Using reader As OleDbDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        Return Integer.Parse(reader("m_id").ToString())
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show(ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                If Conn.State = ConnectionState.Open Then Conn.Close()
            End Try
        End Using

        Return -1
    End Function

    Private Function CalculateMonthlyPayment(principal As Decimal, monthlyInterestRate As Decimal, totalPayments As Integer) As Decimal
        Dim totalInterest As Decimal = principal * (monthlyInterestRate / 100) * totalPayments
        Dim totalAmount As Decimal = principal + totalInterest
        Dim monthlyPayment As Decimal = totalAmount / totalPayments
        Return monthlyPayment
    End Function

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ' เรียกใช้ฟังก์ชัน ResetForm เพื่อเคลียร์ข้อมูลทั้งหมดในฟอร์ม
        ResetForm()
    End Sub

    Private Function ValidateGuarantorCount(loanAmount As Decimal) As Boolean
        Dim guarantorCount As Integer = 0

        ' นับจำนวนผู้ค้ำที่ถูกกรอกข้อมูล
        If Not String.IsNullOrWhiteSpace(txtSearch1.Text) Then guarantorCount += 1
        If Not String.IsNullOrWhiteSpace(txtSearch2.Text) Then guarantorCount += 1
        If Not String.IsNullOrWhiteSpace(txtSearch3.Text) Then guarantorCount += 1

        ' ตรวจสอบจำนวนผู้ค้ำตามจำนวนเงินกู้
        If loanAmount <= 10000 AndAlso guarantorCount < 1 Then
            MessageBox.Show("จำนวนเงินกู้ไม่เกิน 10,000 บาท ต้องมีผู้ค้ำประกันอย่างน้อย 1 คน", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        ElseIf loanAmount > 10000 AndAlso loanAmount < 100000 AndAlso guarantorCount < 2 Then
            MessageBox.Show("จำนวนเงินกู้เกิน 10,000 บาท แต่ไม่ถึง 100,000 บาท ต้องมีผู้ค้ำประกันอย่างน้อย 2 คน", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        ElseIf loanAmount >= 100000 AndAlso guarantorCount < 3 Then
            MessageBox.Show("จำนวนเงินกู้เกิน 100,000 บาท ต้องมีผู้ค้ำประกันอย่างน้อย 3 คน", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Return True
    End Function


End Class
