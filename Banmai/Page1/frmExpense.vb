Imports System.Data.OleDb
Imports System.Drawing.Printing
Imports System.IO

Public Class frmExpense
    ' เชื่อมต่อกับฐานข้อมูล Access
    Private Conn As New OleDbConnection

    ' กำหนด PrintDocument สำหรับการพิมพ์ใบเสร็จ
    Private WithEvents printDoc As New PrintDocument
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

    Private Sub frmExpense_Load(sender As Object, e As EventArgs) Handles MyBase.Load

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

        SetupDataGridView()
        LoadExpenseTypes()
        LoadMemberData()
        LoadAccountData()
        GenerateNextExpenseId() ' เรียกฟังก์ชันนี้เมื่อฟอร์มโหลด
        SetupDateTimePicker()
    End Sub

    Private Sub SetupDateTimePicker()
        dtpBirth.Format = DateTimePickerFormat.Custom
        dtpBirth.CustomFormat = "dd/MM/yyyy"
        dtpBirth.Value = DateTime.Now
    End Sub

    Private Sub GenerateNextExpenseId()
        Try
            Conn.Open()
            Dim query As String = "SELECT MAX(ex_id) FROM Expense"
            Dim cmd As New OleDbCommand(query, Conn)
            Dim result As Object = cmd.ExecuteScalar()

            If IsDBNull(result) Then
                txtExpId.Text = "1" ' ถ้าไม่มีข้อมูลในตาราง Expense เลย ให้เริ่มต้นจาก 1
            Else
                txtExpId.Text = (Convert.ToInt32(result) + 1).ToString() ' ถ้ามีข้อมูลอยู่แล้ว เพิ่มจากค่าที่มากที่สุด
            End If
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการสร้างเลขที่รายจ่ายใหม่: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Conn.Close()
        End Try
    End Sub

    Private Sub SetupDataGridView()
        ' ตั้งค่า DataGridView
        ' ใช้ Guna2DataGridView สำหรับรายละเอียดรายจ่าย
        dgvExpenseDetails.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.Dark
        ' ตั้งค่าฟอนต์ FC Minimal และสีของ DataGridView สำหรับรายละเอียดรายจ่าย
        dgvExpenseDetails.DefaultCellStyle.Font = New Font("FC Minimal", 16) ' ใช้ฟอนต์ FC Minimal
        dgvExpenseDetails.DefaultCellStyle.BackColor = Color.White
        dgvExpenseDetails.DefaultCellStyle.ForeColor = Color.Black
        dgvExpenseDetails.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray
        dgvExpenseDetails.ColumnHeadersDefaultCellStyle.Font = New Font("FC Minimal", 20, FontStyle.Bold) ' ใช้ฟอนต์ FC Minimal สำหรับหัวตาราง
        dgvExpenseDetails.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy
        dgvExpenseDetails.ColumnHeadersDefaultCellStyle.ForeColor = Color.White

        dgvExpenseDetails.DefaultCellStyle.Font = New Font("FC Minimal", 20)
        dgvExpenseDetails.ColumnHeadersDefaultCellStyle.Font = New Font("FC Minimal", 22, FontStyle.Bold)
        dgvExpenseDetails.RowTemplate.Height = 40 ' ตัวอย่างการตั้งค่าความสูงของเซลล์เป็น 40


        ' เพิ่มคอลัมน์ ComboBox สำหรับประเภทของรายจ่าย
        Dim colExpenseType As New DataGridViewComboBoxColumn()
        colExpenseType.HeaderText = "ประเภทของรายจ่าย"
        colExpenseType.Name = "ExpenseType"
        colExpenseType.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dgvExpenseDetails.Columns.Add(colExpenseType)

        ' เพิ่มคอลัมน์สำหรับจำนวนเงิน
        Dim colAmount As New DataGridViewTextBoxColumn()
        colAmount.HeaderText = "จำนวนเงิน"
        colAmount.Name = "Amount"
        colAmount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        colAmount.DefaultCellStyle.Format = "N2"
        colAmount.ValueType = GetType(Decimal)
        dgvExpenseDetails.Columns.Add(colAmount)

        ' เพิ่มคอลัมน์ปุ่มลบ
        Dim colDelete As New DataGridViewButtonColumn()
        colDelete.HeaderText = "ลบ"
        colDelete.Name = "DeleteButton"
        colDelete.Text = "ลบ"
        colDelete.UseColumnTextForButtonValue = True ' ใช้ค่าใน Text เป็นปุ่ม
        dgvExpenseDetails.Columns.Add(colDelete)

        ' ตั้งค่าเพิ่มเติมให้ DataGridView
        dgvExpenseDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvExpenseDetails.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvExpenseDetails.RowTemplate.Height = 30
        dgvExpenseDetails.AllowUserToAddRows = True
    End Sub

    Private Sub dgvExpenseDetails_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvExpenseDetails.CellClick
        If e.ColumnIndex = dgvExpenseDetails.Columns("DeleteButton").Index Then
            ' ยืนยันการลบ
            Dim result As DialogResult = MessageBox.Show("คุณต้องการลบรายการนี้หรือไม่?", "ยืนยันการลบ", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = DialogResult.Yes Then
                ' ลบแถวที่ถูกเลือก
                dgvExpenseDetails.Rows.RemoveAt(e.RowIndex)
            End If
        End If
    End Sub

    Private Sub LoadAccountData()
        Try
            Conn.Open()
            Dim query As String = "SELECT acc_id, acc_name FROM Account"
            Dim cmd As New OleDbCommand(query, Conn)
            Dim reader As OleDbDataReader = cmd.ExecuteReader()

            cboDepositType.Items.Clear()
            Dim dt As New DataTable()
            dt.Load(reader)

            cboDepositType.DataSource = dt
            cboDepositType.DisplayMember = "acc_name"   ' ชื่อคอลัมน์ที่ต้องการแสดงใน ComboBox
            cboDepositType.ValueMember = "acc_id"       ' คอลัมน์ที่ต้องการใช้เป็นค่า (id)
            cboDepositType.SelectedIndex = -1           ' ทำให้ ComboBox ว่างเปล่าในตอนเริ่มต้น
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการโหลดข้อมูลบัญชี: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Conn.Close()
        End Try
    End Sub

    Private Sub LoadExpenseTypes()
        Try
            Conn.Open()
            Dim expenseTypeColumn As DataGridViewComboBoxColumn = CType(dgvExpenseDetails.Columns("ExpenseType"), DataGridViewComboBoxColumn)
            expenseTypeColumn.Items.Clear() ' ล้างรายการเก่า

            ' เพิ่มรายการใหม่
            expenseTypeColumn.Items.Add("ค่าเช่าสำนักงาน")
            expenseTypeColumn.Items.Add("ค่าตอบแทน")
            expenseTypeColumn.Items.Add("ค่าจ้าง")
            expenseTypeColumn.Items.Add("เงินกู้") ' ไม่เอามาคิด
            expenseTypeColumn.Items.Add("สมาชิกลาออก") ' ไม่เอามาคิด
            expenseTypeColumn.Items.Add("ดอกเบี้ยสัจจะ")
            expenseTypeColumn.Items.Add("ดอกเบี้ยจ่าย")
            expenseTypeColumn.Items.Add("อื่นๆ")
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการโหลดข้อมูลประเภทรายจ่าย: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Conn.Close()
        End Try
    End Sub

    Private Sub LoadMemberData()
        Try
            Conn.Open()
            Dim query As String = "SELECT m_id, m_name FROM Member"
            Dim cmd As New OleDbCommand(query, Conn)
            Dim reader As OleDbDataReader = cmd.ExecuteReader()

            Dim autoComplete As New AutoCompleteStringCollection()

            While reader.Read()
                ' เพิ่มชื่อสมาชิกลงใน AutoCompleteStringCollection
                autoComplete.Add(reader("m_name").ToString())
            End While

            ' ตั้งค่า AutoComplete ให้กับ TextBox txtMemberID
            txtMemberID.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            txtMemberID.AutoCompleteSource = AutoCompleteSource.CustomSource
            txtMemberID.AutoCompleteCustomSource = autoComplete
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการโหลดข้อมูลสมาชิก: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Conn.Close()
        End Try
    End Sub

    Private Sub DisplayMemberDetails(memberName As String)
        If String.IsNullOrEmpty(memberName) Then
            txtDetails.Clear()
            Return
        End If

        Try
            Conn.Open()
            Dim query As String = "SELECT * FROM Member WHERE m_name = @memberName"
            Dim cmd As New OleDbCommand(query, Conn)
            cmd.Parameters.AddWithValue("@memberName", memberName)
            Dim reader As OleDbDataReader = cmd.ExecuteReader()

            If reader.Read() Then
                ' แสดงรายละเอียดใน txtDetails
                txtDetails.Text = "รหัสสมาชิก: " & reader("m_id").ToString() & vbCrLf &
                                  "ชื่อ: " & reader("m_name").ToString() & vbCrLf &
                                  "ที่อยู่: " & reader("m_address").ToString() & vbCrLf &
                                  "เบอร์โทร: " & reader("m_tel").ToString()
            Else
                txtDetails.Clear()
            End If
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการดึงข้อมูลสมาชิก: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Conn.Close()
        End Try
    End Sub

    Private Sub CalculateTotalAmount()
        Dim totalAmount As Decimal = 0

        ' คำนวณผลรวมของจำนวนเงินใน DataGridView
        For Each row As DataGridViewRow In dgvExpenseDetails.Rows
            If Not row.IsNewRow Then
                Dim amount As Decimal
                If Decimal.TryParse(row.Cells("Amount").Value?.ToString(), amount) Then
                    totalAmount += amount
                Else
                    MessageBox.Show("พบข้อมูลจำนวนเงินที่ไม่ถูกต้องในแถวที่ " & row.Index + 1, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
            End If
        Next

        ' แสดงผลรวมใน Label
        lblTotalAmount.Text = totalAmount.ToString("N2")
    End Sub

    Private Sub SaveData()
        Try
            If cboDepositType.SelectedValue Is Nothing Then
                MessageBox.Show("กรุณาเลือกบัญชี", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' ดึง m_id ของสมาชิกจากชื่อ
            Dim memberId As Integer = GetMemberIdByName(txtMemberID.Text)
            If memberId = 0 Then
                MessageBox.Show("ไม่พบข้อมูลสมาชิก", "ข้อสังเกต", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim dbPath As String = GetDatabasePath()
            Dim connStr As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath}"

            Using conn As New OleDbConnection(connStr)
                conn.Open()

                ' บันทึกข้อมูลลงตาราง Expense
                Dim queryExpense As String = "INSERT INTO Expense (ex_id, ex_name, ex_detail, ex_description, ex_date, ex_amount, acc_id) " &
                                      "VALUES (@ex_id, @ex_name, @ex_detail, @ex_description, @ex_date, @ex_amount, @acc_id)"
                Using cmdExpense As New OleDbCommand(queryExpense, conn)
                    cmdExpense.Parameters.AddWithValue("@ex_id", Convert.ToInt32(txtExpId.Text))
                    cmdExpense.Parameters.AddWithValue("@ex_name", txtMemberID.Text)
                    cmdExpense.Parameters.AddWithValue("@ex_detail", txtDetails.Text)
                    cmdExpense.Parameters.AddWithValue("@ex_description", txtDescrip.Text)
                    cmdExpense.Parameters.AddWithValue("@ex_date", dtpBirth.Value)
                    cmdExpense.Parameters.AddWithValue("@ex_amount", Decimal.Parse(lblTotalAmount.Text))
                    cmdExpense.Parameters.AddWithValue("@acc_id", cboDepositType.SelectedValue)

                    cmdExpense.ExecuteNonQuery()
                    ' ดึง ex_id ล่าสุดที่ถูกเพิ่มลงในตาราง Expense
                    Dim exId As Integer = Convert.ToInt32(txtExpId.Text)

                    ' บันทึกข้อมูลลงในตาราง Expense_Details
                    For Each row As DataGridViewRow In dgvExpenseDetails.Rows
                        If Not row.IsNewRow Then
                            Dim expenseType As String = If(row.Cells("ExpenseType").Value, "").ToString()
                            Dim amount As Decimal
                            If Not Decimal.TryParse(row.Cells("Amount").Value?.ToString(), amount) Then
                                MessageBox.Show("พบข้อมูลจำนวนเงินที่ไม่ถูกต้องในแถวที่ " & row.Index + 1, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                Return
                            End If

                            Dim expenseDate As DateTime = dtpBirth.Value

                            If Not String.IsNullOrEmpty(expenseType) Then
                                ' Insert into the Expense_Details table
                                Dim queryDetails As String = "INSERT INTO Expense_Details (exd_nameacc, exd_amount, exd_date, ex_id, m_id, acc_id) VALUES (@exd_nameacc, @exd_amount, @exd_date, @ex_id, @m_id, @acc_id)"
                                Using cmdDetails As New OleDbCommand(queryDetails, conn)
                                    cmdDetails.Parameters.AddWithValue("@exd_nameacc", expenseType)
                                    cmdDetails.Parameters.AddWithValue("@exd_amount", amount)
                                    cmdDetails.Parameters.AddWithValue("@exd_date", expenseDate)
                                    cmdDetails.Parameters.AddWithValue("@ex_id", exId)
                                    cmdDetails.Parameters.AddWithValue("@m_id", memberId) ' บันทึกค่า m_id (ถ้าไม่พบสมาชิกจะเป็น 0)
                                    cmdDetails.Parameters.AddWithValue("@acc_id", cboDepositType.SelectedValue) ' บันทึกค่า acc_id จาก ComboBox

                                    cmdDetails.ExecuteNonQuery()
                                End Using
                            End If
                        End If
                    Next
                End Using


                ' ตรวจสอบและหักเงินเฉพาะสมาชิกที่ลาออก
                For Each row As DataGridViewRow In dgvExpenseDetails.Rows
                    If Not row.IsNewRow Then
                        Dim expenseType As String = row.Cells("ExpenseType").Value?.ToString()
                        Dim amount As Decimal = Decimal.Parse(row.Cells("Amount").Value?.ToString())

                        If expenseType = "สมาชิกลาออก" Then
                            ' หักเงินจาก "เงินฝากสัจจะ" ในตาราง Income_Details
                            Dim queryUpdateBalance As String = "UPDATE Income_Details " &
                                                           "SET ind_amount = ind_amount - @amount " &
                                                           "WHERE ind_accname = 'เงินฝากสัจจะ' AND m_id = @m_id"
                            Using cmdUpdate As New OleDbCommand(queryUpdateBalance, conn)
                                cmdUpdate.Parameters.AddWithValue("@amount", amount)
                                cmdUpdate.Parameters.AddWithValue("@m_id", memberId)
                                cmdUpdate.ExecuteNonQuery()
                            End Using
                        End If
                    End If
                Next
            End Using

            MessageBox.Show("บันทึกข้อมูลสำเร็จ", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ClearAll()

        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกข้อมูล: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub






    Private Sub ClearAll()
        ' ล้างข้อมูลในฟอร์ม
        txtMemberID.Clear()
        txtDetails.Clear()
        txtAmount.Clear()
        txtDescrip.Clear()  ' ล้างช่องคำอธิบาย
        cboDepositType.SelectedIndex = -1  ' รีเซ็ตค่า ComboBox บัญชี
        lblTotalAmount.Text = "0.00"

        ' ล้างข้อมูลใน DataGridView
        dgvExpenseDetails.Rows.Clear()

        ' โหลดข้อมูลใหม่สำหรับ ComboBox ใน DataGridView
        LoadExpenseTypes()

        ' สร้างเลขที่รายจ่ายใหม่
        GenerateNextExpenseId()
    End Sub


    Private Sub txtMemberID_TextChanged(sender As Object, e As EventArgs) Handles txtMemberID.TextChanged
        DisplayMemberDetails(txtMemberID.Text)
    End Sub

    Private Sub dgvExpenseDetails_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvExpenseDetails.DataError
        MessageBox.Show("มีข้อผิดพลาดในการกรอกข้อมูลในเซลล์ กรุณาตรวจสอบค่าที่คุณกรอกให้ตรงกับรายการที่มีใน ComboBox", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        e.ThrowException = False
    End Sub

    Private Sub dgvExpenseDetails_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles dgvExpenseDetails.CellValidating
        If dgvExpenseDetails.Columns(e.ColumnIndex).Name = "ExpenseType" Then
            ' ตรวจสอบว่าผู้ใช้ไม่ปล่อยค่าเป็นค่าว่างก่อน
            If String.IsNullOrWhiteSpace(e.FormattedValue.ToString()) Then
                ' หากเป็นค่าว่าง จะไม่ทำการแจ้งเตือน
                Return
            End If

            ' ตรวจสอบว่าค่าที่กรอกตรงกับรายการใน ComboBox หรือไม่
            Dim comboBoxColumn As DataGridViewComboBoxColumn = CType(dgvExpenseDetails.Columns("ExpenseType"), DataGridViewComboBoxColumn)
            If Not comboBoxColumn.Items.Contains(e.FormattedValue.ToString()) Then
                ' แสดงการแจ้งเตือนหากไม่ใช่ค่าที่ถูกต้อง
                MessageBox.Show("กรุณาเลือกประเภทของรายจ่ายที่ถูกต้อง", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                e.Cancel = True
            End If
        End If
    End Sub

    Private Function GetMemberIdByName(memberName As String) As Integer
        Try
            Conn.Open()
            Dim query As String = "SELECT m_id FROM Member WHERE m_name = @memberName"
            Dim cmd As New OleDbCommand(query, Conn)
            cmd.Parameters.AddWithValue("@memberName", memberName)

            Dim result As Object = cmd.ExecuteScalar()
            If IsDBNull(result) Then
                Return 0 ' ถ้าไม่พบข้อมูล ให้คืนค่า 0
            Else
                Return Convert.ToInt32(result) ' คืนค่า m_id ที่ค้นพบ
            End If
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการค้นหา m_id: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return 0
        Finally
            Conn.Close()
        End Try
    End Function


    Private Sub dgvExpenseDetails_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles dgvExpenseDetails.RowsAdded
        btnSave.Enabled = True
        btnCalculate.Enabled = True
    End Sub

    Private Sub btnSave_Click_1(sender As Object, e As EventArgs) Handles btnSave.Click
        ' แปลงค่าใน lblTotalAmount และ txtAmount ให้เป็น Decimal ก่อนเปรียบเทียบ
        Dim totalAmount As Decimal
        Dim inputAmount As Decimal

        ' ลองแปลงค่าจาก lblTotalAmount และ txtAmount
        If Decimal.TryParse(lblTotalAmount.Text, totalAmount) AndAlso Decimal.TryParse(txtAmount.Text, inputAmount) Then
            ' ตรวจสอบว่าค่าทั้งสองเท่ากันหรือไม่
            If totalAmount = inputAmount Then
                ' บันทึกข้อมูล
                SaveData()

                ' ปิดการใช้งานปุ่ม "บันทึก" จนกว่าจะมีการเพิ่มรายการใหม่
                btnSave.Enabled = False
            Else
                MessageBox.Show("จำนวนเงินรวมไม่ตรงกับจำนวนเงินที่ระบุ", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Else
            MessageBox.Show("กรุณากรอกจำนวนเงินที่ถูกต้อง", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub btnCalculate_Click_1(sender As Object, e As EventArgs) Handles btnCalculate.Click
        CalculateTotalAmount()
    End Sub

    Private Sub btnPrintReceipt_Click_1(sender As Object, e As EventArgs) Handles btnPrintReceipt.Click
        ' ตรวจสอบว่าจำนวนเงินรวมตรงกับจำนวนเงินที่ระบุหรือไม่
        Dim totalAmount As Decimal
        Dim inputAmount As Decimal

        ' ลองแปลงค่าจาก lblTotalAmount และ txtAmount
        If Decimal.TryParse(lblTotalAmount.Text, totalAmount) AndAlso Decimal.TryParse(txtAmount.Text, inputAmount) Then
            ' ตรวจสอบว่าค่าทั้งสองเท่ากันหรือไม่
            If totalAmount = inputAmount Then
                ' เปิดการพิมพ์ถ้าจำนวนเงินตรงกัน
                Dim printPreview As New PrintPreviewDialog()
                printPreview.Document = printDoc
                printPreview.ShowDialog()
            Else
                ' แสดงข้อความแจ้งเตือนถ้าจำนวนเงินไม่ตรงกัน
                MessageBox.Show("จำนวนเงินรวมไม่ตรงกับจำนวนเงินที่ระบุ", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Else
            MessageBox.Show("กรุณากรอกจำนวนเงินที่ถูกต้อง", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub


    Private Sub txtAmount_LostFocus(sender As Object, e As EventArgs) Handles txtAmount.LostFocus
        ' ตรวจสอบว่าไม่ใช่การลบข้อมูลทั้งหมด
        If txtAmount.Text.Length > 0 Then
            ' ลบคอมม่าออกก่อนการจัดรูปแบบใหม่
            Dim valueWithoutComma As String = txtAmount.Text.Replace(",", "")

            ' ตรวจสอบว่าเป็นจำนวนเงินที่ถูกต้องหรือไม่
            Dim value As Decimal
            If Decimal.TryParse(valueWithoutComma, value) Then
                ' จัดรูปแบบและใส่คอมม่า
                txtAmount.Text = String.Format("{0:N2}", value)
            End If
        End If
    End Sub





    Private Sub printDoc_PrintPage(sender As Object, e As PrintPageEventArgs) Handles printDoc.PrintPage
        Dim font As New Font("Arial", 10)
        Dim boldFont As New Font("Arial", 12, FontStyle.Bold)
        Dim headerFont As New Font("Arial", 14, FontStyle.Bold)
        Dim startX As Integer
        Dim startY As Integer = 10
        Dim offset As Integer = 40
        Dim lineHeight As Integer = 25 ' Standard line height for rows

        ' Get the page width
        Dim pageWidth As Integer = e.PageBounds.Width

        ' ส่วนหัว (Header Section)
        Dim headerText As String = "กองทุนหมู่บ้าน บ้านใหม่หลังมอ"
        startX = (pageWidth - e.Graphics.MeasureString(headerText, headerFont).Width) / 2 ' Center the header text
        e.Graphics.DrawString(headerText, headerFont, Brushes.Black, startX, startY)
        offset += 30

        Dim addressLine1 As String = "หมู่ที่ 14 ตำบลสุเทพ อำเภอเมืองเชียงใหม่ จังหวัดเชียงใหม่"
        startX = (pageWidth - e.Graphics.MeasureString(addressLine1, font).Width) / 2 ' Center the address line
        e.Graphics.DrawString(addressLine1, font, Brushes.Black, startX, startY + offset)
        offset += 20

        Dim phoneText As String = "โทรศัพท์: 053-219535"
        startX = (pageWidth - e.Graphics.MeasureString(phoneText, font).Width) / 2 ' Center the phone number
        e.Graphics.DrawString(phoneText, font, Brushes.Black, startX, startY + offset)
        offset += 20

        Dim addressLine2 As String = "รหัสกองทุน 5001000"
        startX = (pageWidth - e.Graphics.MeasureString(addressLine2, font).Width) / 2 ' Center the second address line
        e.Graphics.DrawString(addressLine2, font, Brushes.Black, startX, startY + offset)

        ' เว้นระยะระหว่างส่วนหัวและเนื้อหา
        offset += 40

        ' รายละเอียดการจ่าย (Details Section)
        Dim detailsTitle As String = "รายละเอียด:" & txtDescrip.Text
        startX = (pageWidth - e.Graphics.MeasureString(detailsTitle, boldFont).Width) / 2 ' Center the details title
        e.Graphics.DrawString(detailsTitle, boldFont, Brushes.Black, startX, startY + offset)
        offset += 20

        Dim memberIdText As String = "รหัสสมาชิก: " & txtMemberID.Text
        startX = (pageWidth - e.Graphics.MeasureString(memberIdText, font).Width) / 2 ' Center the member ID
        e.Graphics.DrawString(memberIdText, font, Brushes.Black, startX, startY + offset)
        offset += 20

        ' รายการในตาราง (Table Header)
        Dim noText As String = "No."
        Dim itemText As String = "รายการจ่าย"
        Dim amountText As String = "จำนวนเงิน"

        startX = (pageWidth - 500) / 2 ' Start at the middle of the page minus half the table width (assuming table is 500 units wide)
        e.Graphics.DrawString(noText, boldFont, Brushes.Black, startX, startY + offset)
        e.Graphics.DrawString(itemText, boldFont, Brushes.Black, startX + 100, startY + offset)
        e.Graphics.DrawString(amountText, boldFont, Brushes.Black, startX + 400, startY + offset)

        ' เส้นใต้หัวข้อ (Underline Table Header)
        offset += lineHeight
        e.Graphics.DrawLine(Pens.Black, startX, startY + offset, startX + 500, startY + offset)

        ' Loop through DataGridView rows and print them
        Dim rowIndex As Integer = 1
        For Each row As DataGridViewRow In dgvExpenseDetails.Rows
            If Not row.IsNewRow Then
                offset += 5
                e.Graphics.DrawString(rowIndex.ToString(), font, Brushes.Black, startX, startY + offset)
                e.Graphics.DrawString(row.Cells("ExpenseType").Value.ToString(), font, Brushes.Black, startX + 100, startY + offset)
                e.Graphics.DrawString(Decimal.Parse(row.Cells("Amount").Value.ToString()).ToString("N2") & " บาท", font, Brushes.Black, startX + 400, startY + offset)

                ' Move to next row
                rowIndex += 1
                offset += lineHeight
                e.Graphics.DrawLine(Pens.Black, startX, startY + offset, startX + 500, startY + offset) ' Line under the row
            End If
        Next

        ' รวมยอดเงิน (Total Amount Section)
        offset += 40
        Dim totalText As String = "รวมเป็นเงิน: " & lblTotalAmount.Text & " บาท"
        startX = (pageWidth - e.Graphics.MeasureString(totalText, boldFont).Width) / 2 ' Center the total amount text
        e.Graphics.DrawString(totalText, boldFont, Brushes.Black, startX, startY + offset)

        ' เส้นแบ่งลายเซ็น (Line for signature section)
        offset += 60
        Dim signatureText As String = "ผู้รับเงิน"
        startX = (pageWidth - e.Graphics.MeasureString(signatureText, font).Width) / 2 ' Center the signature line
        e.Graphics.DrawString(signatureText, font, Brushes.Black, startX, startY + offset)
        e.Graphics.DrawString("..........................................", font, Brushes.Black, startX + 100, startY + offset)
    End Sub

End Class
