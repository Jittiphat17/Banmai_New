Imports System.Data.OleDb
Imports System.IO


Public Class frmIncome
    ' เชื่อมต่อกับฐานข้อมูล Access
    Private Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")

    Private Sub frmIncome_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupDataGridView() ' สำหรับรายรับ
        SetupDataGridViewForPayments() ' สำหรับค่างวด
        LoadIncomeTypes()
        LoadPaymentTypes() ' โหลดประเภทของค่างวด
        LoadMemberData()
        LoadAccountData()
        GenerateNextIncomeId() ' เรียกฟังก์ชันนี้เมื่อฟอร์มโหลด
        SetupDateTimePicker()
    End Sub

    Private Sub SetupDateTimePicker()
        dtpBirth.Format = DateTimePickerFormat.Custom
        dtpBirth.CustomFormat = "dd/MM/yyyy"
        dtpBirth.Value = DateTime.Now
    End Sub

    Private Sub GenerateNextIncomeId()
        Try
            Using Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
                Conn.Open()
                Dim query As String = "SELECT MAX(inc_id) FROM Income"
                Dim cmd As New OleDbCommand(query, Conn)
                Dim result As Object = cmd.ExecuteScalar()

                If IsDBNull(result) Then
                    txtInid.Text = "1" ' ถ้าไม่มีข้อมูลในตาราง Income เลย ให้เริ่มต้นจาก 1
                Else
                    txtInid.Text = (Convert.ToInt32(result) + 1).ToString() ' ถ้ามีข้อมูลอยู่แล้ว เพิ่มจากค่าที่มากที่สุด
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการสร้างเลขที่รายรับใหม่: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ฟังก์ชัน SetupDataGridView
    Private Sub SetupDataGridView()
        ' เพิ่มคอลัมน์ ComboBox สำหรับประเภทของรายรับ
        Dim colIncomeType As New DataGridViewComboBoxColumn()
        colIncomeType.HeaderText = "ประเภทของรายรับ"
        colIncomeType.Name = "IncomeType"
        colIncomeType.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dgvIncomeDetails.Columns.Add(colIncomeType)

        ' ปิดการใช้งาน DataGridView ก่อนที่จะมีการเลือกสมาชิก
        dgvIncomeDetails.Enabled = False

        ' เพิ่มคอลัมน์สำหรับจำนวนเงิน
        Dim colAmount As New DataGridViewTextBoxColumn()
        colAmount.HeaderText = "จำนวนเงิน"
        colAmount.Name = "Amount"
        colAmount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        colAmount.DefaultCellStyle.Format = "N2"
        colAmount.ValueType = GetType(Decimal)
        dgvIncomeDetails.Columns.Add(colAmount)

        ' เพิ่มคอลัมน์ปุ่มลบรายการ
        Dim colDeleteButton As New DataGridViewButtonColumn()
        colDeleteButton.HeaderText = "ลบรายการ"
        colDeleteButton.Name = "DeleteButton"
        colDeleteButton.Text = "ลบ"
        colDeleteButton.UseColumnTextForButtonValue = True
        dgvIncomeDetails.Columns.Add(colDeleteButton)

        ' ตั้งค่าเพิ่มเติมให้ DataGridView
        dgvIncomeDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvIncomeDetails.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvIncomeDetails.RowTemplate.Height = 30
        dgvIncomeDetails.AllowUserToAddRows = True
    End Sub

    Private Sub SetupDataGridViewForPayments()
        ' เพิ่มคอลัมน์ ComboBox สำหรับประเภทของค่างวด
        Dim colPaymentType As New DataGridViewComboBoxColumn()
        colPaymentType.HeaderText = "ประเภทของค่างวด"
        colPaymentType.Name = "PaymentType"
        colPaymentType.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dgvPaymentDetails.Columns.Add(colPaymentType)

        ' เพิ่มคอลัมน์สำหรับจำนวนเงินค่างวด
        Dim colPaymentAmount As New DataGridViewTextBoxColumn()
        colPaymentAmount.HeaderText = "จำนวนเงินค่างวด"
        colPaymentAmount.Name = "PaymentAmount"
        colPaymentAmount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        colPaymentAmount.DefaultCellStyle.Format = "N2"
        colPaymentAmount.ValueType = GetType(Decimal)
        dgvPaymentDetails.Columns.Add(colPaymentAmount)

        ' เพิ่มคอลัมน์ ComboBox สำหรับเลขที่สัญญา
        Dim colPaymentContractNumber As New DataGridViewComboBoxColumn()
        colPaymentContractNumber.HeaderText = "เลขที่สัญญา"
        colPaymentContractNumber.Name = "PaymentContractNumber"
        colPaymentContractNumber.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        dgvPaymentDetails.Columns.Add(colPaymentContractNumber)

        ' เพิ่มคอลัมน์ ComboBox สำหรับงวดที่
        Dim colPaymentPeriod As New DataGridViewComboBoxColumn()
        colPaymentPeriod.HeaderText = "งวดที่"
        colPaymentPeriod.Name = "PaymentPeriod"
        colPaymentPeriod.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvPaymentDetails.Columns.Add(colPaymentPeriod)

        ' เพิ่มคอลัมน์ปุ่มลบรายการ
        Dim colDeletePaymentButton As New DataGridViewButtonColumn()
        colDeletePaymentButton.HeaderText = "ลบรายการ"
        colDeletePaymentButton.Name = "DeletePaymentButton"
        colDeletePaymentButton.Text = "ลบ"
        colDeletePaymentButton.UseColumnTextForButtonValue = True
        dgvPaymentDetails.Columns.Add(colDeletePaymentButton)

        ' ตั้งค่าเพิ่มเติมให้ DataGridView สำหรับค่างวด
        dgvPaymentDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvPaymentDetails.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvPaymentDetails.RowTemplate.Height = 30
        dgvPaymentDetails.AllowUserToAddRows = True
    End Sub
    Private Sub LoadPaymentTypes()
        Try
            Dim paymentTypeColumn As DataGridViewComboBoxColumn = CType(dgvPaymentDetails.Columns("PaymentType"), DataGridViewComboBoxColumn)
            paymentTypeColumn.Items.Clear() ' ล้างรายการเก่า

            ' เพิ่มรายการใหม่
            paymentTypeColumn.Items.AddRange(New String() {"เงินต้น", "ดอกเบี้ย", "ค่าปรับ"})
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการโหลดข้อมูลประเภทค่างวด: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadAccountData()
        Try
            Using Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
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
            End Using
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการโหลดข้อมูลบัญชี: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub LoadIncomeTypes()
        Try
            Using Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
                Conn.Open()
                Dim incomeTypeColumn As DataGridViewComboBoxColumn = CType(dgvIncomeDetails.Columns("IncomeType"), DataGridViewComboBoxColumn)
                incomeTypeColumn.Items.Clear() ' ล้างรายการเก่า

                ' เพิ่มรายการใหม่
                incomeTypeColumn.Items.Add("เงินฝากสัจจะ")
                incomeTypeColumn.Items.Add("เงินสำรอง")
                incomeTypeColumn.Items.Add("เงินหุ้น")
                incomeTypeColumn.Items.Add("ค่าธรรมเนียม") 'รายได้
                incomeTypeColumn.Items.Add("เงินบริจาค") ' รายได้
                incomeTypeColumn.Items.Add("เงินสนับสนุน")
                incomeTypeColumn.Items.Add("เงินกู้")
                incomeTypeColumn.Items.Add("ทุนบัญชี1")
                incomeTypeColumn.Items.Add("ทุนบัญชีประชารัฐ")
                incomeTypeColumn.Items.Add("กำไรสะสม")
                incomeTypeColumn.Items.Add("เงินสมทบ")
                incomeTypeColumn.Items.Add("ค่าสัญญา") 'รายได้
                incomeTypeColumn.Items.Add("ค่าประกัน") ' รายได้
                incomeTypeColumn.Items.Add("เงินประกันความเสี่ยง")
                incomeTypeColumn.Items.Add("ดอกเบี้ยเงินฝากธนาคาร") 'รายได้
                incomeTypeColumn.Items.Add("ค่าธรรมเนียมแรกเข้า") 'รายได้
                incomeTypeColumn.Items.Add("อื่น ๆ") 'รายได้
                incomeTypeColumn.Items.Add("ค่าเอกสาร")
                incomeTypeColumn.Items.Add("ค่าปรับ")
            End Using
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการโหลดข้อมูลประเภทเงินฝาก: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub LoadMemberData()
        Try
            Using Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
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
            End Using
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการโหลดข้อมูลสมาชิก: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub LoadContractNumbersForPayment(memberName As String)
        Try
            Using Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
                Conn.Open()

                ' ตรวจสอบว่ามีคอลัมน์ PaymentContractNumber ใน dgvPaymentDetails หรือไม่
                If dgvPaymentDetails.Columns.Contains("PaymentContractNumber") Then
                    ' โหลดเลขที่สัญญาสำหรับ dgvPaymentDetails
                    Dim contractNumberColumnPayment As DataGridViewComboBoxColumn = CType(dgvPaymentDetails.Columns("PaymentContractNumber"), DataGridViewComboBoxColumn)
                    contractNumberColumnPayment.Items.Clear()

                    If Not String.IsNullOrEmpty(memberName) Then
                        Dim query As String = "SELECT Contract.con_id FROM Contract INNER JOIN Member ON Contract.m_id = Member.m_id WHERE Member.m_name = @memberName"
                        Dim cmd As New OleDbCommand(query, Conn)
                        cmd.Parameters.AddWithValue("@memberName", memberName)
                        Dim reader As OleDbDataReader = cmd.ExecuteReader()

                        While reader.Read()
                            Dim contractId As String = reader("con_id").ToString()
                            contractNumberColumnPayment.Items.Add(contractId)
                        End While
                    End If
                Else
                    MessageBox.Show("ไม่พบคอลัมน์ PaymentContractNumber ใน DataGridView", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการโหลดเลขที่สัญญาของสมาชิก: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub DisplayMemberDetails(memberName As String)
        If String.IsNullOrEmpty(memberName) Then
            txtDetails.Clear()
            Return
        End If

        Try
            Using Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
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
            End Using
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการดึงข้อมูลสมาชิก: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub CalculateTotalAmount()
        Dim totalAmount As Decimal = 0

        ' Calculate the total amount from dgvIncomeDetails
        For Each row As DataGridViewRow In dgvIncomeDetails.Rows
            If Not row.IsNewRow Then
                Dim amount As Decimal
                If Decimal.TryParse(row.Cells("Amount").Value?.ToString(), amount) Then
                    totalAmount += amount
                End If
            End If
        Next

        ' Calculate the total amount from dgvPaymentDetails
        For Each row As DataGridViewRow In dgvPaymentDetails.Rows
            If Not row.IsNewRow Then
                Dim amount As Decimal
                If Decimal.TryParse(row.Cells("PaymentAmount").Value?.ToString(), amount) Then
                    totalAmount += amount
                End If
            End If
        Next

        ' Display the total amount in lblTotalAmount
        lblTotalAmount.Text = totalAmount.ToString("N2")
    End Sub
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            ' ตรวจสอบว่ามีการเลือกแถวใน DataGridView หรือไม่
            If dgvPaymentDetails.CurrentRow IsNot Nothing Then
                ' ดึงค่าจากคอลัมน์ PaymentContractNumber ใน DataGridView
                Dim contractNumber As String = dgvPaymentDetails.CurrentRow.Cells("PaymentContractNumber").Value.ToString()

            Else
                MessageBox.Show("ไม่มีข้อมูลให้เลือก", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการส่งข้อมูล: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Try
            ' ตรวจสอบว่า lblBalanceAmount.Text มีข้อมูลและเป็นตัวเลข
            Dim balanceAmount As Decimal
            If Not Decimal.TryParse(lblBalanceAmount.Text, balanceAmount) Then
                MessageBox.Show("ยอดคงเหลือต้องเป็นตัวเลข", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' ตรวจสอบว่าค่า balanceAmount เป็นค่าติดลบหรือไม่
            If balanceAmount < 0 Then
                MessageBox.Show("ยอดคงเหลือต้องมากกว่า 0", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' ตรวจสอบว่ามีการเลือกแถวใน DataGridView หรือไม่
            If dgvPaymentDetails.CurrentRow IsNot Nothing Then
                ' รับค่าจากคอลัมน์ใน DataGridView (dgvPaymentDetails)
                Dim conId As Integer = 0
                Dim paymentPeriod As Integer = 0

                ' ตรวจสอบว่าเซลล์มีค่า และแปลงค่าให้ถูกต้อง
                If Integer.TryParse(dgvPaymentDetails.CurrentRow.Cells("PaymentContractNumber").Value.ToString(), conId) AndAlso
               Integer.TryParse(dgvPaymentDetails.CurrentRow.Cells("PaymentPeriod").Value.ToString(), paymentPeriod) Then

                    ' ตรวจสอบว่าค่า conId และ paymentPeriod มีค่ามากกว่า 0
                    If conId > 0 AndAlso paymentPeriod > 0 Then
                        ' รับค่า DateTime จาก DateTimePicker โดยตรง
                        Dim paymentDate As DateTime = dtpBirth.Value.Date

                        ' สร้างการเชื่อมต่อกับฐานข้อมูล
                        Using Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
                            Conn.Open()

                            ' สร้าง SQL คำสั่งในการอัพเดทข้อมูล
                            Dim queryUpdate As String = "UPDATE Payment SET payment_balance = @balanceAmount, status_id = 2, payment_prin = 0, payment_interest = 0 WHERE con_id = @conId AND payment_period = @paymentPeriod"

                            ' สร้างคำสั่งสำหรับการอัพเดทข้อมูล
                            Using cmdUpdate As New OleDbCommand(queryUpdate, Conn)
                                ' กำหนดค่าให้กับพารามิเตอร์ @balanceAmount, @conId, @paymentPeriod
                                cmdUpdate.Parameters.AddWithValue("@balanceAmount", balanceAmount)
                                cmdUpdate.Parameters.AddWithValue("@conId", conId)
                                cmdUpdate.Parameters.AddWithValue("@paymentPeriod", paymentPeriod)

                                ' ดำเนินการอัพเดทข้อมูล
                                Dim rowsAffected As Integer = cmdUpdate.ExecuteNonQuery()

                                ' ตรวจสอบว่าอัพเดทสำเร็จหรือไม่
                                If rowsAffected > 0 Then
                                    Dim queryUpdateDatePayment As String = "UPDATE Payment SET date_payment = @date_payment WHERE con_id = @conId AND payment_period = @paymentPeriod"

                                    ' สร้างคำสั่งสำหรับการอัพเดทข้อมูล
                                    Using cmdUpdateDatePayment As New OleDbCommand(queryUpdateDatePayment, Conn)
                                        cmdUpdateDatePayment.Parameters.AddWithValue("@date_payment", paymentDate) ' ส่งวันที่ที่แปลงแล้ว
                                        cmdUpdateDatePayment.Parameters.AddWithValue("@conId", conId)
                                        cmdUpdateDatePayment.Parameters.AddWithValue("@paymentPeriod", paymentPeriod)
                                        cmdUpdateDatePayment.ExecuteNonQuery()

                                        ' เรียกฟังก์ชันอื่นๆ หลังจากอัพเดทข้อมูล
                                        SaveTransactionToDatabase()

                                        ' ล้างข้อมูลในฟอร์มหลังจากบันทึกเสร็จ
                                        cleartext()
                                    End Using
                                Else
                                    MessageBox.Show("ไม่พบข้อมูลที่ตรงกับเงื่อนไขที่กำหนด", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                End If
                            End Using
                        End Using
                    Else
                        MessageBox.Show("กรุณาตรวจสอบข้อมูล เลขที่สัญญาหรืองวดที่ไม่ถูกต้อง", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                Else
                    MessageBox.Show("ไม่สามารถแปลงข้อมูลเลขที่สัญญาหรืองวดที่ได้", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            Else
                ' กรณีที่ไม่มีการเลือกข้อมูลใน DataGridView จะบันทึกเฉพาะข้อมูลรายรับ (IncomeDetails)
                SaveIncomeDetailsToDatabase()
                cleartext() ' ล้างข้อมูลในฟอร์ม
            End If
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกข้อมูล: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Public Sub cleartext()
        ' ล้างแถวทั้งหมดใน DataGridView ก่อนที่จะตั้งค่าคอลัมน์ใหม่
        dgvPaymentDetails.Rows.Clear()

        dgvIncomeDetails.Rows.Clear()
        txtMemberID.Text = ""
        txtDetails.Text = ""
        txtDescrip.Text = ""
        cboDepositType.Text = ""
        txtAmount.Text = ""
        lblTotalAmount.Text = "0.00"
        lblBalanceAmount.Text = "0.00"
    End Sub

    Private Sub SaveData()
        Try
            If cboDepositType.SelectedValue Is Nothing Then
                MessageBox.Show("กรุณาเลือกบัญชีเงินฝาก", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Using Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
                Conn.Open()

                ' ดึง m_id จากตาราง Member โดยใช้ชื่อสมาชิก
                Dim queryGetMemberId As String = "SELECT m_id FROM Member WHERE m_name = @memberName"
                Dim cmdGetMemberId As New OleDbCommand(queryGetMemberId, Conn)
                cmdGetMemberId.Parameters.AddWithValue("@memberName", txtMemberID.Text)
                Dim memberId As Object = cmdGetMemberId.ExecuteScalar()

                If memberId Is Nothing OrElse DBNull.Value.Equals(memberId) Then
                    MessageBox.Show("ไม่พบข้อมูลสมาชิก", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If

                ' บันทึกข้อมูลลงในตาราง Income
                Dim queryIncome As String = "INSERT INTO Income (m_id, inc_detail, inc_description, inc_date, inc_amount, acc_id) VALUES (@m_id, @inc_detail, @inc_description, @inc_date, @inc_amount, @acc_id)"
                Using cmdIncome As New OleDbCommand(queryIncome, Conn)
                    cmdIncome.Parameters.AddWithValue("@m_id", CInt(memberId))
                    cmdIncome.Parameters.AddWithValue("@inc_detail", txtDetails.Text)
                    cmdIncome.Parameters.AddWithValue("@inc_description", txtDescrip.Text)
                    cmdIncome.Parameters.AddWithValue("@inc_date", dtpBirth.Value)
                    cmdIncome.Parameters.AddWithValue("@inc_amount", CDec(lblTotalAmount.Text))
                    cmdIncome.Parameters.AddWithValue("@acc_id", cboDepositType.SelectedValue.ToString())

                    cmdIncome.ExecuteNonQuery()

                    ' ดึง inc_id ล่าสุด
                    cmdIncome.CommandText = "SELECT @@IDENTITY"
                    Dim incId As Integer = CInt(cmdIncome.ExecuteScalar())

                    ' บันทึกข้อมูลจาก dgvIncomeDetails ลงในตาราง Income_Details พร้อม ind_date
                    For Each row As DataGridViewRow In dgvIncomeDetails.Rows
                        If Not row.IsNewRow Then
                            Dim incomeType As String = If(row.Cells("IncomeType").Value, "").ToString()
                            Dim amount As Decimal = CDec(If(row.Cells("Amount").Value, 0))
                            Dim indDate As DateTime = dtpBirth.Value ' Using the date from DateTimePicker
                            Dim accId As String = cboDepositType.SelectedValue.ToString() ' Retrieve acc_id

                            If Not String.IsNullOrEmpty(incomeType) Then
                                Dim queryDetails As String = "INSERT INTO Income_Details (ind_accname, con_id, ind_amount, inc_id, m_id, ind_date, acc_id) VALUES (@ind_accname, @con_id, @ind_amount, @inc_id, @m_id, @ind_date, @acc_id)"
                                Using cmdDetails As New OleDbCommand(queryDetails, Conn)
                                    cmdDetails.Parameters.AddWithValue("@ind_accname", incomeType)
                                    cmdDetails.Parameters.AddWithValue("@con_id", DBNull.Value) ' No con_id in this case
                                    cmdDetails.Parameters.AddWithValue("@ind_amount", amount)
                                    cmdDetails.Parameters.AddWithValue("@inc_id", incId)
                                    cmdDetails.Parameters.AddWithValue("@m_id", CInt(memberId))
                                    cmdDetails.Parameters.AddWithValue("@ind_date", indDate) ' Add ind_date
                                    cmdDetails.Parameters.AddWithValue("@acc_id", accId) ' Add acc_id to Income_Details

                                    cmdDetails.ExecuteNonQuery()
                                End Using
                            End If
                        End If
                    Next

                    ' บันทึกข้อมูลจาก dgvPaymentDetails ลงในตาราง Income_Details พร้อม ind_date
                    For Each row As DataGridViewRow In dgvPaymentDetails.Rows
                        If Not row.IsNewRow Then
                            Dim paymentType As String = If(row.Cells("PaymentType").Value, "").ToString()
                            Dim paymentContractNumber As String = If(row.Cells("PaymentContractNumber").Value, "").ToString()
                            Dim amount As Decimal = CDec(If(row.Cells("PaymentAmount").Value, 0))
                            Dim indDate As DateTime = dtpBirth.Value ' ใช้วันที่จาก DateTimePicker

                            If Not String.IsNullOrEmpty(paymentType) Then
                                If Not String.IsNullOrEmpty(paymentContractNumber) Then
                                    DeductBalance(paymentContractNumber, amount)
                                End If

                                Dim queryDetails As String = "INSERT INTO Income_Details (ind_accname, con_id, ind_amount, inc_id, m_id, ind_date) VALUES (@ind_accname, @con_id, @ind_amount, @inc_id, @m_id, @ind_date)"
                                Using cmdDetails As New OleDbCommand(queryDetails, Conn)
                                    cmdDetails.Parameters.AddWithValue("@ind_accname", paymentType)
                                    cmdDetails.Parameters.AddWithValue("@con_id", If(String.IsNullOrEmpty(paymentContractNumber), DBNull.Value, paymentContractNumber))
                                    cmdDetails.Parameters.AddWithValue("@ind_amount", amount)
                                    cmdDetails.Parameters.AddWithValue("@inc_id", incId)
                                    cmdDetails.Parameters.AddWithValue("@m_id", CInt(memberId))
                                    cmdDetails.Parameters.AddWithValue("@ind_date", indDate) ' เพิ่ม ind_date

                                    cmdDetails.ExecuteNonQuery()
                                End Using
                            End If
                        End If
                    Next
                End Using
            End Using

            MessageBox.Show("บันทึกข้อมูลสำเร็จ", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' รีเซ็ตฟอร์มเพื่อเตรียมทำรายการใหม่
            ClearAll()

        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกข้อมูล: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Function DeductBalance(contractNumber As String, amount As Decimal) As Boolean
        Try
            Using Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
                Conn.Open()

                ' ดึงจำนวนเงินที่มีอยู่ในสัญญาปัจจุบัน
                Dim queryAmount As String = "SELECT con_amount FROM Contract WHERE con_id = @contractNumber"
                Dim cmdAmount As New OleDbCommand(queryAmount, Conn)
                cmdAmount.Parameters.AddWithValue("@contractNumber", contractNumber)
                Dim currentAmount As Decimal = Convert.ToDecimal(cmdAmount.ExecuteScalar())

                Dim newAmount As Decimal = currentAmount - amount

                If newAmount < 0 Then
                    MessageBox.Show("ไม่สามารถหักยอดเงินได้เนื่องจากยอดเงินคงเหลือติดลบ", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return False
                End If

                ' อัปเดตจำนวนเงินในสัญญา
                Dim queryUpdateContract As String = "UPDATE Contract SET con_amount = @newAmount WHERE con_id = @contractNumber"
                Dim cmdUpdateContract As New OleDbCommand(queryUpdateContract, Conn)
                cmdUpdateContract.Parameters.AddWithValue("@newAmount", newAmount)
                cmdUpdateContract.Parameters.AddWithValue("@contractNumber", contractNumber)
                cmdUpdateContract.ExecuteNonQuery()

                ' อัปเดตฟิลด์ payment_balance ในตาราง Payment
                Dim queryUpdatePayment As String = "UPDATE Payment SET payment_balance = @newAmount WHERE con_id = @contractNumber"
                Dim cmdUpdatePayment As New OleDbCommand(queryUpdatePayment, Conn)
                cmdUpdatePayment.Parameters.AddWithValue("@newAmount", newAmount)
                cmdUpdatePayment.Parameters.AddWithValue("@contractNumber", contractNumber)
                cmdUpdatePayment.ExecuteNonQuery()

                Return True
            End Using
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการหักยอดเงิน: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function
    Private Sub UpdatePaymentStatus(contractNumber As String)
        Try
            Using Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
                Conn.Open()

                ' ดึงข้อมูลงวดที่ยังไม่ได้ชำระล่าสุด
                Dim querySelect As String = "SELECT TOP 1 payment_period, payment_prin, payment_interest FROM Payment WHERE con_id = @contractNumber AND status_id = 1 ORDER BY payment_period ASC"
                Dim cmdSelect As New OleDbCommand(querySelect, Conn)
                cmdSelect.Parameters.AddWithValue("@contractNumber", contractNumber)
                Dim reader As OleDbDataReader = cmdSelect.ExecuteReader()

                If reader.Read() Then
                    Dim period As Integer = Convert.ToInt32(reader("payment_period"))
                    Dim principal As Decimal = Convert.ToDecimal(reader("payment_prin"))
                    Dim interest As Decimal = Convert.ToDecimal(reader("payment_interest"))

                    ' อัปเดตสถานะของงวดที่พบเป็นชำระแล้ว
                    Dim queryUpdate As String = "UPDATE Payment SET status_id = 2 WHERE con_id = @contractNumber AND payment_period = @period"
                    Dim cmdUpdate As New OleDbCommand(queryUpdate, Conn)
                    cmdUpdate.Parameters.AddWithValue("@contractNumber", contractNumber)
                    cmdUpdate.Parameters.AddWithValue("@period", period)
                    cmdUpdate.ExecuteNonQuery()

                    MessageBox.Show($"ชำระงวดที่ {period} สำเร็จ เงินต้น: {principal:N2} ดอกเบี้ย: {interest:N2}", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("ไม่พบงวดที่ยังไม่ได้ชำระ", "ข้อมูล", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการอัพเดตสถานะการชำระเงิน: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub ClearAll()
        ' ปิดการใช้งาน DataGridView ชั่วคราวเพื่อป้องกันข้อผิดพลาด
        dgvIncomeDetails.Enabled = False
        dgvPaymentDetails.Enabled = False

        ' ล้างข้อมูลในฟอร์ม
        txtMemberID.Clear()
        txtDetails.Clear()
        txtAmount.Clear()
        cboDepositType.SelectedIndex = -1
        lblTotalAmount.Text = "0.00"

        ' ล้างข้อมูลใน DataGridView
        dgvIncomeDetails.Rows.Clear()
        dgvPaymentDetails.Rows.Clear()

        ' โหลดข้อมูลใหม่สำหรับ ComboBox ใน DataGridView
        LoadIncomeTypes()
        LoadContractNumbersForPayment(txtMemberID.Text)

        ' สร้างเลขที่รายรับใหม่
        GenerateNextIncomeId()

        ' เปิดการใช้งาน DataGridView อีกครั้งหลังจากการล้างข้อมูลเสร็จสิ้น
        dgvIncomeDetails.Enabled = True
        dgvPaymentDetails.Enabled = True
    End Sub

    ' เมื่อมีการเปลี่ยนแปลงรหัสสมาชิก
    Private Sub txtMemberID_TextChanged(sender As Object, e As EventArgs) Handles txtMemberID.TextChanged
        ' ตรวจสอบว่าผู้ใช้ได้ป้อนชื่อสมาชิกหรือไม่ ถ้าไม่มี ให้หยุดการทำงานและไม่แสดงข้อความแจ้งเตือน
        If String.IsNullOrEmpty(txtMemberID.Text.Trim()) Then
            Return ' ถ้าไม่มีข้อมูลก็หยุดการทำงานของฟังก์ชัน โดยไม่แจ้งเตือน
        End If

        ' เรียกฟังก์ชันเพื่อแสดงรายละเอียดของสมาชิกเมื่อผู้ใช้พิมพ์หรือเลือกชื่อสมาชิก
        DisplayMemberDetails(txtMemberID.Text)

        ' โหลดเลขที่สัญญาที่เกี่ยวข้องกับสมาชิกที่ถูกเลือก
        LoadContractNumbersForPayment(txtMemberID.Text) ' ใช้สำหรับทั้ง dgvIncomeDetails และ dgvPaymentDetails

        ' เปิดการใช้งาน DataGridView
        dgvIncomeDetails.Enabled = True
        dgvPaymentDetails.Enabled = True
    End Sub

    ' ฟังก์ชันสำหรับการคำนวณยอดรวมโดยไม่หักยอด
    Private Sub btnCalculate_Click(sender As Object, e As EventArgs) Handles btnCalculate.Click
        Try
            ' ตรวจสอบว่ามีข้อมูลใน dgvPaymentDetails หรือ dgvIncomeDetails หรือไม่
            If dgvPaymentDetails.Rows.Count <= 1 And dgvIncomeDetails.Rows.Count <= 1 Then
                MessageBox.Show("กรุณากรอกข้อมูลใน DataGridView อย่างน้อยหนึ่งรายการ", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return ' หากไม่มีข้อมูลในทั้งสอง DataGridView ให้หยุดการทำงาน
            End If

            ' คำนวณยอดรวมจาก dgvPaymentDetails
            Dim totalPaymentAmount As Decimal = 0
            If dgvPaymentDetails.Rows.Count > 1 Then ' ตรวจสอบว่ามีแถวใน dgvPaymentDetails
                For Each row As DataGridViewRow In dgvPaymentDetails.Rows
                    If Not row.IsNewRow Then
                        Dim paymentAmount As Decimal
                        If row.Cells("PaymentAmount").Value IsNot Nothing AndAlso
                       Decimal.TryParse(row.Cells("PaymentAmount").Value?.ToString(), paymentAmount) Then
                            totalPaymentAmount += paymentAmount
                        End If
                    End If
                Next
            End If

            ' คำนวณยอดรวมจาก dgvIncomeDetails
            Dim totalIncomeAmount As Decimal = 0
            If dgvIncomeDetails.Rows.Count > 1 Then ' ตรวจสอบว่ามีแถวใน dgvIncomeDetails
                For Each row As DataGridViewRow In dgvIncomeDetails.Rows
                    If Not row.IsNewRow Then
                        Dim incomeAmount As Decimal
                        If row.Cells("Amount").Value IsNot Nothing AndAlso
                       Decimal.TryParse(row.Cells("Amount").Value?.ToString(), incomeAmount) Then
                            totalIncomeAmount += incomeAmount
                        End If
                    End If
                Next
            End If

            ' รวมยอดจากทั้งสอง DataGridView
            Dim grandTotal As Decimal = totalPaymentAmount + totalIncomeAmount

            ' เรียกใช้ฟังก์ชัน CheckPaymentDate (ถ้ามีการเลือกแถว)
            If dgvPaymentDetails.CurrentRow IsNot Nothing Then
                CheckPaymentDate()
            End If

            ' แสดงยอดรวมทั้งหมดใน Label หรือที่อื่น ๆ ตามต้องการ
            lblTotalAmount.Text = grandTotal.ToString("N2")

        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการคำนวณ: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        ' เรียกใช้ฟังก์ชันคำนวณอื่นๆ ถ้าจำเป็น
        CalculateTotalAmount()

    End Sub



    Private Sub dgvIncomeDetails_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvIncomeDetails.DataError
        ' แสดงข้อความแจ้งข้อผิดพลาดที่เข้าใจง่ายขึ้น
        MessageBox.Show("มีข้อผิดพลาดในการกรอกข้อมูลในเซลล์ กรุณาตรวจสอบค่าที่คุณกรอกให้ตรงกับรายการที่มีใน ComboBox", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        e.ThrowException = False ' ป้องกันไม่ให้แอปพลิเคชันหยุดทำงาน
    End Sub
    Private Sub dgvIncomeDetails_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles dgvIncomeDetails.CellValidating
        If dgvIncomeDetails.Columns(e.ColumnIndex).Name = "IncomeType" Then
            ' ตรวจสอบว่าผู้ใช้ไม่ปล่อยค่าเป็นค่าว่างก่อน
            If String.IsNullOrWhiteSpace(e.FormattedValue.ToString()) Then
                ' หากเป็นค่าว่าง จะไม่ทำการแจ้งเตือน
                Return
            End If

            ' ตรวจสอบว่าค่าที่กรอกตรงกับรายการใน ComboBox หรือไม่
            Dim comboBoxColumn As DataGridViewComboBoxColumn = CType(dgvIncomeDetails.Columns("IncomeType"), DataGridViewComboBoxColumn)
            If Not comboBoxColumn.Items.Contains(e.FormattedValue.ToString()) Then
                ' แสดงการแจ้งเตือนหากไม่ใช่ค่าที่ถูกต้อง
                MessageBox.Show("กรุณาเลือกประเภทของรายรับที่ถูกต้อง", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                e.Cancel = True
            End If
        End If
    End Sub

    Private Sub dgvIncomeDetails_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvIncomeDetails.CellClick
        ' ตรวจสอบว่าเป็นการคลิกปุ่มลบ
        If e.ColumnIndex = dgvIncomeDetails.Columns("DeleteButton").Index AndAlso e.RowIndex >= 0 Then
            ' ยืนยันการลบ
            Dim result As DialogResult = MessageBox.Show("คุณต้องการลบรายการนี้หรือไม่?", "ยืนยันการลบ", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = DialogResult.Yes Then
                ' ลบรายการ
                dgvIncomeDetails.Rows.RemoveAt(e.RowIndex)
            End If
        End If
    End Sub
    Private Sub dgvIncomeDetails_RowsAdded(sender As Object, e As DataGridViewRowsAddedEventArgs) Handles dgvIncomeDetails.RowsAdded
        btnSave.Enabled = True ' เปิดการใช้งานปุ่ม "บันทึก" เมื่อมีการเพิ่มรายการใหม่
        btnCalculate.Enabled = True ' เปิดการใช้งานปุ่ม "คำนวณ" เมื่อมีการเพิ่มรายการใหม่
    End Sub
    Private Sub dgvPaymentDetails_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles dgvPaymentDetails.EditingControlShowing
        If dgvPaymentDetails.CurrentCell.ColumnIndex = dgvPaymentDetails.Columns("PaymentContractNumber").Index Then
            Dim cmb As ComboBox = TryCast(e.Control, ComboBox)
            If cmb IsNot Nothing Then
                RemoveHandler cmb.SelectedIndexChanged, AddressOf PaymentContractNumber_SelectedIndexChanged
                AddHandler cmb.SelectedIndexChanged, AddressOf PaymentContractNumber_SelectedIndexChanged
            End If
        End If

    End Sub
    ' ตัวแปร flag เพื่อควบคุมการทำงานของ CellValueChanged
    Private isAdding As Boolean = False
    Private Sub AddPaymentDetails(contractNumber As String)
        ' ป้องกันการเกิด StackOverflowException โดยหยุดการทำงานชั่วคราว
        If isAdding Then Exit Sub

        Try
            isAdding = True ' เริ่มต้นกระบวนการเพิ่มข้อมูล

            Dim paymentDataList = GetPaymentData(contractNumber)

            If paymentDataList.Count > 0 Then
                For Each paymentData In paymentDataList
                    ' ตรวจสอบสถานะงวดก่อนหน้าว่ามีการชำระเงินแล้วหรือไม่
                    If Not CheckPreviousPaymentStatus(contractNumber, paymentData.Period) Then
                        Exit Sub ' ถ้ายังคงค้างชำระงวดก่อนหน้า หยุดการทำงาน
                    End If

                    ' ตรวจสอบว่างวดปัจจุบันมีการชำระเงินแล้วหรือไม่
                    If Not CheckCurrentPaymentStatus(contractNumber, paymentData.Period) Then
                        Exit Sub ' ถ้างวดที่เลือกชำระเงินแล้ว หยุดการทำงาน
                    End If

                    ' เพิ่มเงินต้นและดอกเบี้ยสำหรับงวดที่ปัจจุบัน
                    If Not IsPaymentAlreadyAdded(contractNumber, "เงินต้น") Then
                        Dim rowIndex As Integer = dgvPaymentDetails.Rows.Add()
                        dgvPaymentDetails.Rows(rowIndex).Cells("PaymentContractNumber").Value = contractNumber
                        dgvPaymentDetails.Rows(rowIndex).Cells("PaymentType").Value = "เงินต้น"
                        dgvPaymentDetails.Rows(rowIndex).Cells("PaymentAmount").Value = paymentData.Principal
                    End If

                    If Not IsPaymentAlreadyAdded(contractNumber, "ดอกเบี้ย") Then
                        Dim rowIndex As Integer = dgvPaymentDetails.Rows.Add()
                        dgvPaymentDetails.Rows(rowIndex).Cells("PaymentContractNumber").Value = contractNumber
                        dgvPaymentDetails.Rows(rowIndex).Cells("PaymentType").Value = "ดอกเบี้ย"
                        dgvPaymentDetails.Rows(rowIndex).Cells("PaymentAmount").Value = paymentData.Interest
                    End If
                Next
            End If

            ' ลบแถวที่ไม่มีข้อมูลจริงออกไป
            RemoveEmptyRows()

        Finally
            isAdding = False ' รีเซ็ต flag หลังจากเพิ่มข้อมูลเสร็จสิ้น
        End Try
    End Sub
    Private Sub PaymentContractNumber_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim cmb As ComboBox = CType(sender, ComboBox)
        Dim contractNumber As String = cmb.Text

        ' เรียกฟังก์ชันเพื่อเพิ่มข้อมูลการชำระเงิน
        AddPaymentDetails(contractNumber)
    End Sub
    ' ฟังก์ชันตรวจสอบว่ามีการเพิ่ม PaymentType แบบเดียวกันแล้วหรือไม่
    Private Function IsPaymentAlreadyAdded(contractNumber As String, paymentType As String) As Boolean
        For Each row As DataGridViewRow In dgvPaymentDetails.Rows
            If Not row.IsNewRow AndAlso
           row.Cells("PaymentContractNumber").Value IsNot Nothing AndAlso
           row.Cells("PaymentType").Value IsNot Nothing AndAlso
           row.Cells("PaymentContractNumber").Value.ToString() = contractNumber AndAlso
           row.Cells("PaymentType").Value.ToString() = paymentType Then
                Return True
            End If
        Next
        Return False
    End Function

    ' ฟังก์ชันสำหรับดึงข้อมูลเงินต้นและดอกเบี้ยตาม PaymentContractNumber
    Private Function GetPaymentData(contractNumber As String) As List(Of (Period As Integer, Principal As Decimal, Interest As Decimal))
        Dim payments As New List(Of (Period As Integer, Principal As Decimal, Interest As Decimal))

        Using Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
            Conn.Open()

            ' ดึงข้อมูลงวดการชำระทั้งหมดที่ยังไม่ได้ชำระเรียงตามลำดับงวด
            Dim query As String = "SELECT payment_period, payment_prin, payment_interest FROM Payment WHERE con_id = @contractNumber AND status_id = 1 ORDER BY payment_period ASC"
            Dim cmd As New OleDbCommand(query, Conn)
            cmd.Parameters.AddWithValue("@contractNumber", contractNumber)
            Dim reader As OleDbDataReader = cmd.ExecuteReader()

            ' เก็บข้อมูลแต่ละงวดที่ดึงมาในรายการ
            While reader.Read()
                Dim period As Integer = Convert.ToInt32(reader("payment_period"))
                Dim principalAmount As Decimal = Convert.ToDecimal(reader("payment_prin"))
                Dim interestAmount As Decimal = Convert.ToDecimal(reader("payment_interest"))
                payments.Add((Period:=period, Principal:=principalAmount, Interest:=interestAmount))
            End While
        End Using
        Return payments
    End Function
    Private Sub dgvPaymentDetails_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles dgvPaymentDetails.DataError
        If e.Context = DataGridViewDataErrorContexts.Commit Then
            MessageBox.Show("มีข้อผิดพลาดในการกรอกข้อมูลในเซลล์ กรุณาตรวจสอบค่าที่คุณกรอกให้ตรงกับรายการที่มีใน ComboBox", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
        e.ThrowException = False ' ป้องกันไม่ให้แอปพลิเคชันหยุดทำงาน
    End Sub

    Private Sub dgvPaymentDetails_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPaymentDetails.CellClick
        ' ตรวจสอบว่าคลิกที่คอลัมน์ปุ่มลบและไม่ใช่แถวส่วนหัว
        If e.ColumnIndex = dgvPaymentDetails.Columns("DeletePaymentButton").Index AndAlso e.RowIndex >= 0 Then
            ' ตรวจสอบว่ามีมากกว่า 2 แถวหรือไม่
            If dgvPaymentDetails.Rows.Count > 2 Then
                ' ถามยืนยันก่อนลบ
                Dim result As DialogResult = MessageBox.Show("คุณแน่ใจหรือไม่ที่จะลบรายการนี้?", "ยืนยันการลบ", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If result = DialogResult.Yes Then
                    ' ลบแถว
                    dgvPaymentDetails.Rows.RemoveAt(e.RowIndex)

                    ' คำนวณยอดรวมใหม่หลังจากลบ
                    CalculateTotalAmount()
                End If
            Else
                MessageBox.Show("ไม่สามารถลบรายการนี้ได้ เนื่องจากต้องมีอย่างน้อย 2 รายการ (เงินต้นและดอกเบี้ย)", "ไม่สามารถลบได้", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub
    Private Sub dgvPaymentDetails_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles dgvPaymentDetails.CellValidating
        If dgvPaymentDetails.Columns(e.ColumnIndex).Name = "PaymentType" Then
            Dim comboBoxColumn As DataGridViewComboBoxColumn = CType(dgvPaymentDetails.Columns("PaymentType"), DataGridViewComboBoxColumn)

            ' ตรวจสอบว่าค่าที่ป้อนไม่เป็นค่าว่างหรือมีแค่ช่องว่าง
            If Not String.IsNullOrWhiteSpace(e.FormattedValue.ToString()) Then
                ' ตรวจสอบว่าค่าที่ป้อนอยู่ในรายการของ ComboBox หรือไม่
                If Not comboBoxColumn.Items.Contains(e.FormattedValue.ToString()) Then
                    e.Cancel = True
                    MessageBox.Show("กรุณาเลือกประเภทของค่างวดที่ถูกต้อง", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            End If
        End If
    End Sub

    '*********************************************************************************************************************************************************************************************
    Private Sub PopulatePaymentPeriodBasedOnContractNumber(rowIndex As Integer)
        Try
            ' ตรวจสอบว่ามีค่าในคอลัมน์ PaymentContractNumber หรือไม่
            Dim contractNumber As String = dgvPaymentDetails.Rows(rowIndex).Cells("PaymentContractNumber").Value?.ToString()
            If String.IsNullOrEmpty(contractNumber) Then
                MessageBox.Show("กรุณาเลือกเลขที่สัญญาก่อน", "ข้อมูลไม่พบ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' กำหนดการเชื่อมต่อฐานข้อมูล Access
            Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb"
            Using conn As New OleDbConnection(connectionString)
                ' เปิดการเชื่อมต่อฐานข้อมูล
                conn.Open()

                ' สร้าง SQL Query เพื่อดึงข้อมูลจากคอลัมน์ payment_period ในตาราง Payment ที่มี con_id ตรงกับ PaymentContractNumber
                Dim query As String = "SELECT payment_period FROM Payment WHERE con_id = @contractNumber"
                Using cmd As New OleDbCommand(query, conn)
                    ' ส่งค่า parameter con_id ไปกับ SQL Command
                    cmd.Parameters.AddWithValue("@contractNumber", contractNumber)

                    ' อ่านข้อมูลจากฐานข้อมูล
                    Using reader As OleDbDataReader = cmd.ExecuteReader()
                        ' ตรวจสอบว่ามีข้อมูลหรือไม่
                        If reader.HasRows Then
                            ' ล้างข้อมูลเดิมใน ComboBox ก่อน
                            Dim colPaymentPeriod As DataGridViewComboBoxCell = CType(dgvPaymentDetails.Rows(rowIndex).Cells("PaymentPeriod"), DataGridViewComboBoxCell)
                            colPaymentPeriod.Items.Clear()

                            ' วนรอบเพื่อเพิ่มข้อมูล payment_period ลงใน ComboBox ของ DataGridView
                            While reader.Read()
                                colPaymentPeriod.Items.Add(reader("payment_period").ToString())
                            End While
                        Else
                            '       MessageBox.Show("ไม่พบข้อมูล payment_period สำหรับเลขที่สัญญาที่เลือก", "ข้อมูลไม่พบ", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            ' MessageBox.Show("เกิดข้อผิดพลาดในการดึงข้อมูล payment_period: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub dgvPaymentDetails_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles dgvPaymentDetails.CellValueChanged
        ' ตรวจสอบว่าค่าที่เปลี่ยนแปลงเป็นคอลัมน์ PaymentContractNumber หรือไม่
        If e.ColumnIndex = dgvPaymentDetails.Columns("PaymentContractNumber").Index Then
            PopulatePaymentPeriodBasedOnContractNumber(e.RowIndex)
        End If
    End Sub

    Private Sub RemoveEmptyRows()
        ' วนลูปตรวจสอบแถวใน dgvPaymentDetails จากท้ายสุดไปต้นสุด (เพื่อป้องกันการเปลี่ยนแปลง index)
        For i As Integer = dgvPaymentDetails.Rows.Count - 1 To 0 Step -1
            Dim row As DataGridViewRow = dgvPaymentDetails.Rows(i)

            ' ข้ามการลบแถวใหม่ที่ยังไม่ได้รับการคอมมิต
            If row.IsNewRow Then
                Continue For
            End If

            ' ตรวจสอบว่าคอลัมน์ PaymentAmount ว่างเปล่าหรือไม่
            Dim paymentAmount As String = row.Cells("PaymentAmount").Value?.ToString()
            If String.IsNullOrEmpty(paymentAmount) OrElse paymentAmount = "0" Then
                ' ลบแถวที่ไม่มีข้อมูลจริง
                dgvPaymentDetails.Rows.RemoveAt(i)
            End If
        Next
    End Sub
    ' ฟังก์ชันตรวจสอบสถานะการชำระเงินของงวดก่อนหน้า
    Private Function CheckPreviousPaymentStatus(contractNumber As String, currentPeriod As Integer) As Boolean
        Try
            ' เปิดการเชื่อมต่อฐานข้อมูล
            Using Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
                Conn.Open()

                ' ตรวจสอบงวดก่อนหน้าว่ามีการชำระเงินหรือไม่ (status_id = 2 คือชำระเงินแล้ว)
                Dim query As String = "SELECT status_id FROM Payment WHERE con_id = @contractNumber AND payment_period = @previousPeriod"
                Using cmd As New OleDbCommand(query, Conn)
                    cmd.Parameters.AddWithValue("@contractNumber", contractNumber)
                    cmd.Parameters.AddWithValue("@previousPeriod", currentPeriod - 1) ' งวดก่อนหน้า

                    Dim statusId As Object = cmd.ExecuteScalar()

                    ' ตรวจสอบว่ามีข้อมูลและสถานะเป็น "ยังไม่ชำระเงิน"
                    If statusId IsNot Nothing AndAlso Convert.ToInt32(statusId) <> 2 Then
                        MessageBox.Show($"ยังคงค้างชำระงวดที่ {currentPeriod - 1}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return False ' งวดก่อนหน้ายังไม่ชำระเงิน
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการตรวจสอบสถานะการชำระเงิน: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return True ' งวดก่อนหน้าชำระเงินแล้ว
    End Function
    ' ฟังก์ชันตรวจสอบสถานะการชำระเงินของงวดปัจจุบัน
    Private Function CheckCurrentPaymentStatus(contractNumber As String, currentPeriod As Integer) As Boolean
        Try
            ' เปิดการเชื่อมต่อฐานข้อมูล
            Using Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
                Conn.Open()

                ' ตรวจสอบว่างวดที่เลือกมีการชำระเงินแล้วหรือไม่ (status_id = 2 คือชำระเงินแล้ว)
                Dim query As String = "SELECT status_id FROM Payment WHERE con_id = @contractNumber AND payment_period = @currentPeriod"
                Using cmd As New OleDbCommand(query, Conn)
                    cmd.Parameters.AddWithValue("@contractNumber", contractNumber)
                    cmd.Parameters.AddWithValue("@currentPeriod", currentPeriod) ' งวดปัจจุบัน

                    Dim statusId As Object = cmd.ExecuteScalar()

                    ' ถ้า status_id คือ 2 แสดงว่ามีการชำระเงินแล้ว
                    If statusId IsNot Nothing AndAlso Convert.ToInt32(statusId) = 2 Then
                        MessageBox.Show($"งวดที่ {currentPeriod} คุณเลือกมีการชำระเงินแล้ว", "ข้อมูล", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return False ' งวดปัจจุบันชำระเงินแล้ว
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการตรวจสอบสถานะการชำระเงิน: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return True ' งวดปัจจุบันยังไม่ได้ชำระเงิน
    End Function
    ' ฟังก์ชันสำหรับดึงข้อมูลจำนวนค่างวด (principal + interest) จากฐานข้อมูล
    Private Function GetPaymentAmount(contractNumber As String, paymentPeriod As Integer) As Decimal
        Dim paymentAmount As Decimal = 0
        Try
            Using Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
                Conn.Open()

                ' ดึงค่าเงินต้นและดอกเบี้ยจากฐานข้อมูลสำหรับงวดที่เลือก
                Dim query As String = "SELECT payment_prin, payment_interest FROM Payment WHERE con_id = @contractNumber AND payment_period = @paymentPeriod"
                Dim cmd As New OleDbCommand(query, Conn)
                cmd.Parameters.AddWithValue("@contractNumber", contractNumber)
                cmd.Parameters.AddWithValue("@paymentPeriod", paymentPeriod)

                Dim reader As OleDbDataReader = cmd.ExecuteReader()

                If reader.Read() Then
                    Dim principal As Decimal = Convert.ToDecimal(reader("payment_prin"))
                    Dim interest As Decimal = Convert.ToDecimal(reader("payment_interest"))

                    ' เก็บยอดรวมของเงินต้นและดอกเบี้ยในตัวแปร a
                    paymentAmount = principal + interest
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการดึงข้อมูลค่างวด: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return paymentAmount
    End Function

    Private Sub CheckPaymentDate()
        Try
            ' ตรวจสอบว่ามีแถวปัจจุบันที่ถูกเลือกใน DataGridView หรือไม่
            If dgvPaymentDetails.CurrentRow IsNot Nothing Then
                ' ดึงค่า con_id, payment_period และ payment_amount จาก DataGridView
                Dim contractNumber As Integer = Convert.ToInt32(dgvPaymentDetails.CurrentRow.Cells("PaymentContractNumber").Value)
                Dim paymentPeriod As Integer = Convert.ToInt32(dgvPaymentDetails.CurrentRow.Cells("PaymentPeriod").Value)
                Dim paymentAmount As Decimal = Convert.ToDecimal(dgvPaymentDetails.CurrentRow.Cells("PaymentAmount").Value)

                ' เชื่อมต่อกับฐานข้อมูล Access
                Using Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
                    Conn.Open()

                    ' คำสั่ง SQL เพื่อดึงวันที่และจำนวนเงินจากตาราง Payment
                    Dim query As String = "SELECT payment_date FROM Payment WHERE con_id = @contractNumber AND payment_period = @paymentPeriod"
                    Using cmd As New OleDbCommand(query, Conn)
                        ' กำหนดค่าให้กับพารามิเตอร์
                        cmd.Parameters.AddWithValue("@contractNumber", contractNumber)
                        cmd.Parameters.AddWithValue("@paymentPeriod", paymentPeriod)

                        ' แสดงข้อความเพื่อยืนยันการรันคำสั่ง SQL
                        Using reader As OleDbDataReader = cmd.ExecuteReader()
                            ' ตรวจสอบแต่ละแถวในฐานข้อมูล
                            If reader.HasRows Then
                                While reader.Read()
                                    ' ดึงค่า payment_date จากฐานข้อมูล
                                    Dim paymentDate As DateTime = Convert.ToDateTime(reader("payment_date"))

                                    ' คำนวณวันที่ปัจจุบัน
                                    Dim currentDate As DateTime = DateTime.Now.Date

                                    ' คำนวณจำนวนวันที่แตกต่างกัน
                                    Dim dateDifference As Integer = (currentDate - paymentDate).Days

                                    ' ถ้าความแตกต่างมากกว่า 7 วัน
                                    If dateDifference > 7 Then
                                        ' แสดง popup แจ้งเตือน
                                        MessageBox.Show("ท่านชำระบริการเกินกำหนดในงวดนี้ : " & (dateDifference - 7).ToString() & " วัน", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information)

                                        ' เพิ่มแถวใหม่ใน DataGridView สำหรับ "ค่าปรับ"
                                        Dim newRowIndex As Integer = dgvPaymentDetails.Rows.Add()
                                        dgvPaymentDetails.Rows(newRowIndex).Cells("PaymentType").Value = "ค่าปรับ"
                                        dgvPaymentDetails.Rows(newRowIndex).Cells("PaymentAmount").Value = (paymentAmount * 0.05D) * (dateDifference - 7)
                                    End If

                                End While
                            Else
                                ' กรณีไม่พบข้อมูล
                                MessageBox.Show("ไม่พบข้อมูลที่ตรงกับ contract number และ payment period", "ข้อมูลไม่พบ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            End If
                        End Using
                    End Using
                End Using
            Else
                MessageBox.Show("กรุณาเลือกแถวใน DataGridView", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            ' จัดการข้อผิดพลาดที่อาจเกิดขึ้น
            MessageBox.Show("เกิดข้อผิดพลาด: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SavePaymentDetailsToDatabase()
        Try
            ' ตรวจสอบว่ามีข้อมูลใน DataGridView หรือไม่
            If dgvPaymentDetails.Rows.Count > 0 Then
                ' เชื่อมต่อกับฐานข้อมูล
                Using Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
                    Conn.Open()

                    ' ดึง m_id จากตาราง Member โดยใช้ชื่อสมาชิก
                    Dim queryGetMemberId As String = "SELECT m_id FROM Member WHERE m_name = @memberName"
                    Dim cmdGetMemberId As New OleDbCommand(queryGetMemberId, Conn)
                    cmdGetMemberId.Parameters.AddWithValue("@memberName", txtMemberID.Text)
                    Dim memberId As Object = cmdGetMemberId.ExecuteScalar()

                    If memberId Is Nothing OrElse DBNull.Value.Equals(memberId) Then
                        MessageBox.Show("ไม่พบข้อมูลสมาชิก", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If

                    ' คำนวณจำนวนเงินรวมทั้งหมด
                    Dim totalAmount As Decimal = 0
                    For Each row As DataGridViewRow In dgvPaymentDetails.Rows
                        If Not row.IsNewRow Then
                            Dim paymentAmount As Decimal = Convert.ToDecimal(row.Cells("PaymentAmount").Value)
                            totalAmount += paymentAmount ' รวมจำนวนเงิน
                        End If
                    Next

                    ' บันทึกรายการรวมในตาราง Income
                    Dim indDate As DateTime = dtpBirth.Value ' ใช้วันที่จาก DateTimePicker
                    Dim accId As String = cboDepositType.SelectedValue.ToString() ' Retrieve acc_id
                    Dim queryIncome As String = "INSERT INTO Income (m_id, inc_detail, inc_description, inc_date, inc_amount, acc_id) VALUES (@m_id, @inc_detail, @inc_description, @inc_date, @inc_amount, @acc_id)"
                    Using cmdIncome As New OleDbCommand(queryIncome, Conn)
                        cmdIncome.Parameters.AddWithValue("@m_id", CInt(memberId))
                        cmdIncome.Parameters.AddWithValue("@inc_detail", "ชำระเงินรวม")
                        cmdIncome.Parameters.AddWithValue("@inc_description", "รวมการชำระเงินทั้งหมด")
                        cmdIncome.Parameters.AddWithValue("@inc_date", indDate)
                        cmdIncome.Parameters.AddWithValue("@inc_amount", totalAmount)
                        cmdIncome.Parameters.AddWithValue("@acc_id", accId)
                        cmdIncome.ExecuteNonQuery()

                        ' รับค่า inc_id ที่ถูกสร้างขึ้นในตาราง Income
                        Dim queryGetIncId As String = "SELECT @@IDENTITY"
                        Dim cmdGetIncId As New OleDbCommand(queryGetIncId, Conn)
                        Dim incId As Integer = Convert.ToInt32(cmdGetIncId.ExecuteScalar())

                        ' วนลูปบันทึกรายละเอียดใน Income_Details
                        For Each row As DataGridViewRow In dgvPaymentDetails.Rows
                            If row.IsNewRow Then Continue For

                            Dim paymentType As String = row.Cells("PaymentType").Value.ToString()
                            Dim paymentAmount As Decimal = Convert.ToDecimal(row.Cells("PaymentAmount").Value)
                            Dim contractNumber As Integer = Convert.ToInt32(row.Cells("PaymentContractNumber").Value)

                            Dim queryDetails As String = "INSERT INTO Income_Details (ind_accname, con_id, ind_amount, ind_date, m_id, acc_id, inc_id) VALUES (@paymentType, @contractNumber, @paymentAmount, @ind_date, @m_id, @acc_id, @inc_id)"
                            Using cmdDetails As New OleDbCommand(queryDetails, Conn)
                                cmdDetails.Parameters.AddWithValue("@paymentType", paymentType)
                                cmdDetails.Parameters.AddWithValue("@contractNumber", contractNumber)
                                cmdDetails.Parameters.AddWithValue("@paymentAmount", paymentAmount)
                                cmdDetails.Parameters.AddWithValue("@ind_date", indDate)
                                cmdDetails.Parameters.AddWithValue("@m_id", CInt(memberId))
                                cmdDetails.Parameters.AddWithValue("@acc_id", accId)
                                cmdDetails.Parameters.AddWithValue("@inc_id", incId) ' ใช้ค่า inc_id ที่เพิ่งบันทึก
                                cmdDetails.ExecuteNonQuery()
                            End Using
                        Next
                    End Using

                    ' แสดงข้อความว่าได้บันทึกข้อมูลสำเร็จ
                    MessageBox.Show("บันทึกข้อมูลสำเร็จ", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Using
            Else
                ' แจ้งเตือนหากไม่มีข้อมูลใน DataGridView
                MessageBox.Show("ไม่มีข้อมูลที่จะบันทึก", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            ' จัดการข้อผิดพลาด
            MessageBox.Show("เกิดข้อผิดพลาด: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SaveIncomeDetailsToDatabase()
        Try
            ' ตรวจสอบว่ามีข้อมูลใน DataGridView หรือไม่
            If dgvIncomeDetails.Rows.Count > 0 Then
                ' เชื่อมต่อกับฐานข้อมูล
                Using Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
                    Conn.Open()

                    ' ดึง m_id จากตาราง Member โดยใช้ชื่อสมาชิก
                    Dim queryGetMemberId As String = "SELECT m_id FROM Member WHERE m_name = @memberName"
                    Dim cmdGetMemberId As New OleDbCommand(queryGetMemberId, Conn)
                    cmdGetMemberId.Parameters.AddWithValue("@memberName", txtMemberID.Text)
                    Dim memberId As Object = cmdGetMemberId.ExecuteScalar()

                    If memberId Is Nothing OrElse DBNull.Value.Equals(memberId) Then
                        MessageBox.Show("ไม่พบข้อมูลสมาชิก", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If

                    ' คำนวณจำนวนเงินรวมทั้งหมด
                    Dim totalAmount As Decimal = 0
                    For Each row As DataGridViewRow In dgvIncomeDetails.Rows
                        If Not row.IsNewRow Then
                            Dim amount As Decimal = Convert.ToDecimal(row.Cells("Amount").Value)
                            totalAmount += amount ' รวมจำนวนเงิน
                        End If
                    Next

                    ' บันทึกรายการรวมในตาราง Income (ทำครั้งเดียว)
                    Dim indDate As DateTime = dtpBirth.Value ' ใช้วันที่จาก DateTimePicker
                    Dim accId As String = cboDepositType.SelectedValue.ToString() ' Retrieve acc_id
                    Dim queryIncome As String = "INSERT INTO Income (m_id, inc_detail, inc_description, inc_date, inc_amount, acc_id) VALUES (@m_id, @inc_detail, @inc_description, @inc_date, @inc_amount, @acc_id)"
                    Using cmdIncome As New OleDbCommand(queryIncome, Conn)
                        cmdIncome.Parameters.AddWithValue("@m_id", CInt(memberId))
                        cmdIncome.Parameters.AddWithValue("@inc_detail", "รายรับรวม")
                        cmdIncome.Parameters.AddWithValue("@inc_description", "รวมรายรับทั้งหมด")
                        cmdIncome.Parameters.AddWithValue("@inc_date", indDate)
                        cmdIncome.Parameters.AddWithValue("@inc_amount", totalAmount)
                        cmdIncome.Parameters.AddWithValue("@acc_id", accId)
                        cmdIncome.ExecuteNonQuery()

                        ' รับค่า inc_id ที่ถูกสร้างขึ้นในตาราง Income
                        Dim queryGetIncId As String = "SELECT @@IDENTITY"
                        Dim cmdGetIncId As New OleDbCommand(queryGetIncId, Conn)
                        Dim incId As Integer = Convert.ToInt32(cmdGetIncId.ExecuteScalar())

                        ' วนลูปบันทึกรายละเอียดใน Income_Details (เชื่อมกับ inc_id ที่เพิ่งบันทึก)
                        For Each row As DataGridViewRow In dgvIncomeDetails.Rows
                            If row.IsNewRow Then Continue For

                            Dim incomeType As String = row.Cells("IncomeType").Value.ToString()
                            Dim amount As Decimal = Convert.ToDecimal(row.Cells("Amount").Value)

                            Dim queryDetails As String = "INSERT INTO Income_Details (ind_accname, ind_amount, ind_date, m_id, acc_id, inc_id) VALUES (@incomeType, @amount, @ind_date, @m_id, @acc_id, @inc_id)"
                            Using cmdDetails As New OleDbCommand(queryDetails, Conn)
                                cmdDetails.Parameters.AddWithValue("@incomeType", incomeType)
                                cmdDetails.Parameters.AddWithValue("@amount", amount)
                                cmdDetails.Parameters.AddWithValue("@ind_date", indDate)
                                cmdDetails.Parameters.AddWithValue("@m_id", CInt(memberId))
                                cmdDetails.Parameters.AddWithValue("@acc_id", accId)
                                cmdDetails.Parameters.AddWithValue("@inc_id", incId) ' ใช้ค่า inc_id ที่เพิ่งบันทึก
                                cmdDetails.ExecuteNonQuery()
                            End Using
                        Next
                    End Using

                    ' แสดงข้อความว่าได้บันทึกข้อมูลสำเร็จ
                    MessageBox.Show("บันทึกข้อมูลสำเร็จ", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Using
            Else
                ' แจ้งเตือนหากไม่มีข้อมูลใน DataGridView
                MessageBox.Show("ไม่มีข้อมูลที่จะบันทึก", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            ' จัดการข้อผิดพลาด
            MessageBox.Show("เกิดข้อผิดพลาด: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub SaveTransactionToDatabase()
        Try
            ' ตรวจสอบว่ามีข้อมูลใน DataGridView หรือไม่
            If dgvPaymentDetails.Rows.Count > 0 OrElse dgvIncomeDetails.Rows.Count > 0 Then
                ' เชื่อมต่อกับฐานข้อมูล
                Using Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
                    Conn.Open()

                    ' ดึง m_id จากตาราง Member โดยใช้ชื่อสมาชิก
                    Dim queryGetMemberId As String = "SELECT m_id FROM Member WHERE m_name = @memberName"
                    Dim cmdGetMemberId As New OleDbCommand(queryGetMemberId, Conn)
                    cmdGetMemberId.Parameters.AddWithValue("@memberName", txtMemberID.Text)
                    Dim memberId As Object = cmdGetMemberId.ExecuteScalar()

                    If memberId Is Nothing OrElse DBNull.Value.Equals(memberId) Then
                        MessageBox.Show("ไม่พบข้อมูลสมาชิก", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If

                    ' คำนวณจำนวนเงินรวมทั้งหมดจากทั้งการชำระเงินและรายรับ
                    Dim totalAmount As Decimal = 0

                    ' คำนวณยอดรวมจากการชำระเงิน
                    For Each row As DataGridViewRow In dgvPaymentDetails.Rows
                        If Not row.IsNewRow Then
                            Dim paymentAmount As Decimal = Convert.ToDecimal(row.Cells("PaymentAmount").Value)
                            totalAmount += paymentAmount ' รวมยอดเงินชำระ
                        End If
                    Next

                    ' คำนวณยอดรวมจากรายรับ
                    For Each row As DataGridViewRow In dgvIncomeDetails.Rows
                        If Not row.IsNewRow Then
                            Dim incomeAmount As Decimal = Convert.ToDecimal(row.Cells("Amount").Value)
                            totalAmount += incomeAmount ' รวมยอดรายรับ
                        End If
                    Next

                    ' บันทึกรายการรวมในตาราง Income (ทำครั้งเดียว)
                    Dim indDate As DateTime = dtpBirth.Value ' ใช้วันที่จาก DateTimePicker
                    Dim accId As String = cboDepositType.SelectedValue.ToString() ' Retrieve acc_id
                    Dim queryIncome As String = "INSERT INTO Income (m_id, inc_detail, inc_description, inc_date, inc_amount, acc_id) VALUES (@m_id, @inc_detail, @inc_description, @inc_date, @inc_amount, @acc_id)"
                    Using cmdIncome As New OleDbCommand(queryIncome, Conn)
                        cmdIncome.Parameters.AddWithValue("@m_id", CInt(memberId))
                        cmdIncome.Parameters.AddWithValue("@inc_detail", "รายการรวมชำระเงินและรายรับ")
                        cmdIncome.Parameters.AddWithValue("@inc_description", "รวมการชำระเงินและรายรับทั้งหมด")
                        cmdIncome.Parameters.AddWithValue("@inc_date", indDate)
                        cmdIncome.Parameters.AddWithValue("@inc_amount", totalAmount)
                        cmdIncome.Parameters.AddWithValue("@acc_id", accId)
                        cmdIncome.ExecuteNonQuery()

                        ' รับค่า inc_id ที่ถูกสร้างขึ้นในตาราง Income
                        Dim queryGetIncId As String = "SELECT @@IDENTITY"
                        Dim cmdGetIncId As New OleDbCommand(queryGetIncId, Conn)
                        Dim incId As Integer = Convert.ToInt32(cmdGetIncId.ExecuteScalar())

                        ' วนลูปบันทึกรายละเอียดการชำระเงินใน Income_Details
                        For Each row As DataGridViewRow In dgvPaymentDetails.Rows
                            If row.IsNewRow Then Continue For

                            Dim paymentType As String = row.Cells("PaymentType").Value.ToString()
                            Dim paymentAmount As Decimal = Convert.ToDecimal(row.Cells("PaymentAmount").Value)
                            Dim contractNumber As Integer = Convert.ToInt32(row.Cells("PaymentContractNumber").Value)

                            Dim queryDetails As String = "INSERT INTO Income_Details (ind_accname, con_id, ind_amount, ind_date, m_id, acc_id, inc_id) VALUES (@paymentType, @contractNumber, @paymentAmount, @ind_date, @m_id, @acc_id, @inc_id)"
                            Using cmdDetails As New OleDbCommand(queryDetails, Conn)
                                cmdDetails.Parameters.AddWithValue("@paymentType", paymentType)
                                cmdDetails.Parameters.AddWithValue("@contractNumber", contractNumber)
                                cmdDetails.Parameters.AddWithValue("@paymentAmount", paymentAmount)
                                cmdDetails.Parameters.AddWithValue("@ind_date", indDate)
                                cmdDetails.Parameters.AddWithValue("@m_id", CInt(memberId))
                                cmdDetails.Parameters.AddWithValue("@acc_id", accId)
                                cmdDetails.Parameters.AddWithValue("@inc_id", incId) ' ใช้ค่า inc_id ที่เพิ่งบันทึก
                                cmdDetails.ExecuteNonQuery()
                            End Using
                        Next

                        ' วนลูปบันทึกรายละเอียดรายรับใน Income_Details โดยดึง con_id จาก dgvPaymentDetails
                        For Each rowIncome As DataGridViewRow In dgvIncomeDetails.Rows
                            If rowIncome.IsNewRow Then Continue For

                            Dim incomeType As String = rowIncome.Cells("IncomeType").Value.ToString()
                            Dim incomeAmount As Decimal = Convert.ToDecimal(rowIncome.Cells("Amount").Value)
                            Dim contractNumber As Integer = 0 ' ค่าเริ่มต้นของ con_id

                            ' ค้นหา con_id ที่สอดคล้องกันใน dgvPaymentDetails
                            For Each rowPayment As DataGridViewRow In dgvPaymentDetails.Rows
                                If rowPayment.IsNewRow Then Continue For

                                ' สมมติว่า PaymentContractNumber เป็นคอลัมน์ที่มี con_id ใน dgvPaymentDetails
                                Dim paymentContractNumber As Integer = Convert.ToInt32(rowPayment.Cells("PaymentContractNumber").Value)

                                ' ถ้าเจอ con_id ที่ต้องการจาก dgvPaymentDetails ให้ใช้งาน con_id นั้น
                                contractNumber = paymentContractNumber
                                Exit For ' ออกจากลูปเมื่อเจอ con_id ที่ตรงกัน
                            Next

                            ' บันทึกข้อมูลลงใน Income_Details
                            Dim queryDetails As String = "INSERT INTO Income_Details (ind_accname, con_id, ind_amount, ind_date, m_id, acc_id, inc_id) VALUES (@incomeType, @contractNumber, @incomeAmount, @ind_date, @m_id, @acc_id, @inc_id)"
                            Using cmdDetails As New OleDbCommand(queryDetails, Conn)
                                cmdDetails.Parameters.AddWithValue("@incomeType", incomeType)
                                cmdDetails.Parameters.AddWithValue("@contractNumber", contractNumber) ' ใช้ con_id ที่ได้จาก dgvPaymentDetails
                                cmdDetails.Parameters.AddWithValue("@incomeAmount", incomeAmount)
                                cmdDetails.Parameters.AddWithValue("@ind_date", indDate)
                                cmdDetails.Parameters.AddWithValue("@m_id", CInt(memberId))
                                cmdDetails.Parameters.AddWithValue("@acc_id", accId)
                                cmdDetails.Parameters.AddWithValue("@inc_id", incId) ' ใช้ค่า inc_id ที่เพิ่งบันทึก
                                cmdDetails.ExecuteNonQuery()
                            End Using
                        Next

                    End Using

                    ' แสดงข้อความว่าได้บันทึกข้อมูลสำเร็จ
                    MessageBox.Show("บันทึกข้อมูลสำเร็จ", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Using
            Else
                ' แจ้งเตือนหากไม่มีข้อมูลใน DataGridView
                MessageBox.Show("ไม่มีข้อมูลที่จะบันทึก", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            ' จัดการข้อผิดพลาด
            MessageBox.Show("เกิดข้อผิดพลาด: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtAmount_TextChanged(sender As Object, e As EventArgs) Handles txtAmount.TextChanged
        ' ตรวจสอบและจัดการกรณีที่ txtAmount หรือ lblTotalAmount ไม่มีค่า
        Dim enteredAmount As Decimal
        Dim totalAmount As Decimal

        ' ตรวจสอบว่า txtAmount เป็นตัวเลขหรือไม่ ถ้าไม่เป็น ให้กำหนดค่าเป็น 0
        If Not Decimal.TryParse(txtAmount.Text, enteredAmount) Then
            enteredAmount = 0
        End If

        ' ตรวจสอบว่า lblTotalAmount เป็นตัวเลขหรือไม่ ถ้าไม่เป็น ให้กำหนดค่าเป็น 0
        If Not Decimal.TryParse(lblTotalAmount.Text, totalAmount) Then
            totalAmount = 0
        End If

        ' คำนวณยอดคงเหลือ
        Dim balanceAmount As Decimal = enteredAmount - totalAmount
        lblBalanceAmount.Text = balanceAmount.ToString("N2")
    End Sub


End Class