Imports System.Data.OleDb
Imports System.Drawing ' ใช้สำหรับการตั้งค่าฟอนต์
Imports System.IO
Imports Guna.UI2.WinForms

Public Class frmClose
    Private formLoaded As Boolean = False
    ' สร้างการเชื่อมต่อฐานข้อมูล
    Private Conn As OleDbConnection
    ' ฟังก์ชันดึงเส้นทางฐานข้อมูลจาก config.ini
    Private Function GetDatabasePath() As String
        Dim iniPath As String = Path.Combine(Application.StartupPath, "config.ini")

        ' ตรวจสอบว่าไฟล์ config.ini มีอยู่หรือไม่
        If Not File.Exists(iniPath) Then
            Throw New Exception($"ไม่พบไฟล์ config.ini ที่: {iniPath}")
        End If

        ' อ่านบรรทัดทั้งหมดใน config.ini
        Dim lines = File.ReadAllLines(iniPath)

        ' ค้นหาบรรทัดที่มี Path=
        Dim dbPathLine = lines.FirstOrDefault(Function(line) line.StartsWith("Path="))
        If String.IsNullOrEmpty(dbPathLine) Then
            Throw New Exception("ไม่พบ 'Path' ของฐานข้อมูลใน config.ini")
        End If

        ' แปลง path เป็น Absolute Path ถ้าจำเป็น
        Dim dbPath = dbPathLine.Replace("Path=", "").Trim()
        If dbPath.StartsWith(".\") Then
            dbPath = Path.Combine(Application.StartupPath, dbPath.Substring(2))
        End If

        ' ตรวจสอบว่าไฟล์ฐานข้อมูลมีอยู่จริง
        If Not File.Exists(dbPath) Then
            Throw New Exception($"ไม่พบไฟล์ฐานข้อมูลที่: {dbPath}")
        End If

        Return dbPath
    End Function

    Private Sub frmClose_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

        ' โหลดข้อมูลบัญชีทั้งหมดลงใน ComboBox
        LoadAccounts()

        ' บอกว่าฟอร์มได้ทำการโหลดแล้ว
        formLoaded = True

        ' ตั้งค่า ComboBox สำหรับรายได้
        ComboBoxIncomeName.Items.AddRange(New String() {"กำไรสะสมบัญชี1", "กำไรสะสมบัญชีเงินสัจจะ", "กำไรสะสมบัญชีประชารัฐ",
                                                        "เงินประกันความเสี่ยงบัญชี1", "เงินประกันความเสี่ยงบัญชีเงินสัจจะ", "เงินประกันความเสี่ยงบัญชีประชารัฐ",
                                                        "เงินสมทบบัญชี1", "เงินสมทบบัญชีเงินสัจจะ", "เงินสมทบบัญชีประชารัฐ"})

        ' ตั้งค่า ComboBox สำหรับรายจ่าย
        ComboBoxExpenseName.Items.AddRange(New String() {"กำไรสะสมบัญชี1", "กำไรสะสมบัญชีเงินสัจจะ", "กำไรสะสมบัญชีประชารัฐ",
                                                         "เงินประกันความเสี่ยงบัญชี1", "เงินประกันความเสี่ยงบัญชีเงินสัจจะ", "เงินประกันความเสี่ยงบัญชีประชารัฐ",
                                                         "เงินสมทบบัญชี1", "เงินสมทบบัญชีเงินสัจจะ", "เงินสมทบบัญชีประชารัฐ"})

        ' ตั้งค่าเริ่มต้นเป็นไม่มีการเลือก
        ComboBoxIncomeName.SelectedIndex = -1
        ComboBoxExpenseName.SelectedIndex = -1
    End Sub

    ' Method to load account names into ComboBox
    Private Sub LoadAccounts()
        Dim dtAccounts As New DataTable

        Try
            ' ดึง path ของฐานข้อมูลจาก config.ini
            Dim dbPath As String = GetDatabasePath()
            Dim connStr As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;"

            ' เชื่อมต่อฐานข้อมูล
            Using conn As New OleDbConnection(connStr)
                conn.Open()

                ' ดึงข้อมูลชื่อและรหัสบัญชี
                Dim query As String = "SELECT acc_id, acc_name FROM Account"
                Dim cmd As New OleDbCommand(query, conn)
                Dim adapter As New OleDbDataAdapter(cmd)
                adapter.Fill(dtAccounts)
            End Using

            ' ผูกข้อมูลกับ ComboBox
            ComboBox1.DataSource = dtAccounts
            ComboBox1.DisplayMember = "acc_name"
            ComboBox1.ValueMember = "acc_id" ' ใช้ acc_id เป็นค่าที่จะใช้ในการกรอง

        Catch ex As Exception
            MessageBox.Show("Error loading accounts: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' Method to load income data into Guna2DataGridView1 พร้อมกรองวันที่เฉพาะ
    Private Sub LoadIncomeData(accId As String, selectedDate As DateTime)
        Dim dtIncome As New DataTable
        Dim formattedDate As String = selectedDate.ToString("yyyy-MM-dd")

        Try
            ' ดึง path ของฐานข้อมูลจาก config.ini
            Dim dbPath As String = GetDatabasePath()
            Dim connStr As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;"

            ' เชื่อมต่อกับฐานข้อมูล
            Using conn As New OleDbConnection(connStr)
                conn.Open()

                ' Filter by acc_id and date <= selectedDate
                Dim query As String = "SELECT ind_accname, SUM(ind_amount) AS TotalIncome " &
                                  "FROM Income_Details " &
                                  "WHERE acc_id = @accId AND FORMAT(ind_date, 'yyyy-MM-dd') <= @selectedDate " &
                                  "AND ind_accname IN ('ค่าธรรมเนียม', 'เงินบริจาค', 'ค่าสัญญา', 'ค่าประกัน', 'ดอกเบี้ยเงินฝากธนาคาร', 'ค่าธรรมเนียมแรกเข้า', 'อื่น ๆ', 'ดอกเบี้ย', 'ค่าปรับ') " &
                                  "GROUP BY ind_accname"
                Dim cmd As New OleDbCommand(query, conn)
                cmd.Parameters.AddWithValue("@accId", accId.ToString()) ' Ensure acc_id is treated as string
                cmd.Parameters.AddWithValue("@selectedDate", formattedDate)
                Dim adapter As New OleDbDataAdapter(cmd)
                adapter.Fill(dtIncome)
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading income data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        ' ผูกข้อมูลกับ DataGridView
        Me.Guna2DataGridView1.DataSource = dtIncome
    End Sub



    ' Method to load expense data into Guna2DataGridView2 พร้อมกรองวันที่เฉพาะ
    Private Sub LoadExpenseData(accId As String, selectedDate As DateTime)
        Dim dtExpense As New DataTable
        Dim formattedDate As String = selectedDate.ToString("yyyy-MM-dd")

        Try
            ' ดึง path ของฐานข้อมูลจาก config.ini
            Dim dbPath As String = GetDatabasePath()
            Dim connStr As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;"

            ' เชื่อมต่อกับฐานข้อมูล
            Using conn As New OleDbConnection(connStr)
                conn.Open()

                ' Filter by acc_id and date <= selectedDate
                Dim query As String = "SELECT exd_nameacc, SUM(exd_amount) AS TotalExpense " &
                                  "FROM Expense_Details " &
                                  "WHERE acc_id = @accId AND FORMAT(exd_date, 'yyyy-MM-dd') <= @selectedDate " &
                                  "AND exd_nameacc IN ('ค่าเช่าสำนักงาน', 'เงินสมทบ', 'เงินประกันความเสี่ยง', 'ค่าตอบแทน', 'ค่าจ้าง', 'ดอกเบี้ยสัจจะ', 'ดอกเบี้ยจ่าย', 'อื่นๆ') " &
                                  "GROUP BY exd_nameacc"

                Dim cmd As New OleDbCommand(query, conn)
                cmd.Parameters.AddWithValue("@accId", accId.ToString()) ' Ensure acc_id is treated as string
                cmd.Parameters.AddWithValue("@selectedDate", formattedDate)

                Dim adapter As New OleDbDataAdapter(cmd)
                adapter.Fill(dtExpense)
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading expense data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        ' ผูกข้อมูลกับ DataGridView
        Me.Guna2DataGridView2.DataSource = dtExpense
    End Sub



    ' Method to customize the DataGridView
    Private Sub CustomizeDataGridView()
        ' ตั้งค่าใช้ฟอนต์ FC Minimal ขนาด 16 สำหรับเนื้อหาในตาราง (ตัวปกติ)
        Dim fcMinimalFont As New Font("FC Minimal", 20, FontStyle.Regular)
        ' ตั้งค่าใช้ฟอนต์ FC Minimal ขนาด 16 สำหรับหัวตาราง (ตัวหนา)
        Dim fcMinimalBoldFont As New Font("FC Minimal", 22, FontStyle.Bold)

        ' ตั้งฟอนต์สำหรับ Guna2DataGridView1 (รายได้)
        Me.Guna2DataGridView1.DefaultCellStyle.Font = fcMinimalFont ' ตัวปกติสำหรับเนื้อหา
        Me.Guna2DataGridView1.ColumnHeadersDefaultCellStyle.Font = fcMinimalBoldFont ' ตัวหนาสำหรับหัวตาราง

        ' กำหนดความสูงของแถวใน Guna2DataGridView1
        Me.Guna2DataGridView1.RowTemplate.Height = 30 ' ตัวอย่าง: ตั้งค่าความสูง 30 พิกเซล

        ' กำหนดสีพื้นหลังและสีตัวอักษรสำหรับแถวสลับ
        Me.Guna2DataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240)
        Me.Guna2DataGridView1.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black

        ' ตั้งค่าสีพื้นหลังและตัวอักษรเมื่อเลือกแถว
        Me.Guna2DataGridView1.DefaultCellStyle.SelectionBackColor = Color.LightBlue
        Me.Guna2DataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black

        ' จัดจำนวนเงินให้ชิดขวาใน Guna2DataGridView1
        Me.Guna2DataGridView1.Columns("TotalIncome").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        ' กำหนดรูปแบบตัวเลขในคอลัมน์ TotalIncome ให้มีเครื่องหมายจุลภาคและทศนิยม 2 ตำแหน่ง
        Me.Guna2DataGridView1.Columns("TotalIncome").DefaultCellStyle.Format = "N2"

        ' กำหนดหัวตารางเป็นภาษาไทยสำหรับ Guna2DataGridView1
        Me.Guna2DataGridView1.Columns("ind_accname").HeaderText = "ชื่อบัญชีรายได้"
        Me.Guna2DataGridView1.Columns("TotalIncome").HeaderText = "รวมรายได้"

        ' ตั้งฟอนต์สำหรับ Guna2DataGridView2 (รายจ่าย)
        Me.Guna2DataGridView2.DefaultCellStyle.Font = fcMinimalFont ' ตัวปกติสำหรับเนื้อหา
        Me.Guna2DataGridView2.ColumnHeadersDefaultCellStyle.Font = fcMinimalBoldFont ' ตัวหนาสำหรับหัวตาราง

        ' กำหนดความสูงของแถวใน Guna2DataGridView2
        Me.Guna2DataGridView2.RowTemplate.Height = 30 ' ตัวอย่าง: ตั้งค่าความสูง 30 พิกเซล

        ' กำหนดสีพื้นหลังและสีตัวอักษรสำหรับแถวสลับใน Guna2DataGridView2
        Me.Guna2DataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240)
        Me.Guna2DataGridView2.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black

        ' ตั้งค่าสีพื้นหลังและตัวอักษรเมื่อเลือกแถว
        Me.Guna2DataGridView2.DefaultCellStyle.SelectionBackColor = Color.LightBlue
        Me.Guna2DataGridView2.DefaultCellStyle.SelectionForeColor = Color.Black

        ' จัดจำนวนเงินให้ชิดขวาใน Guna2DataGridView2
        Me.Guna2DataGridView2.Columns("TotalExpense").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        ' กำหนดรูปแบบตัวเลขในคอลัมน์ TotalExpense ให้มีเครื่องหมายจุลภาคและทศนิยม 2 ตำแหน่ง
        Me.Guna2DataGridView2.Columns("TotalExpense").DefaultCellStyle.Format = "N2"

        ' กำหนดหัวตารางเป็นภาษาไทยสำหรับ Guna2DataGridView2
        Me.Guna2DataGridView2.Columns("exd_nameacc").HeaderText = "ชื่อบัญชีค่าใช้จ่าย"
        Me.Guna2DataGridView2.Columns("TotalExpense").HeaderText = "รวมค่าใช้จ่าย"
    End Sub

    ' ฟังก์ชันคำนวณรายได้ รายจ่าย และส่วนต่าง
    Private Sub CalculateTotals()
        Dim totalIncome As Decimal = 0
        Dim totalExpense As Decimal = 0

        ' คำนวณยอดรวมรายได้จาก Guna2DataGridView1
        For Each row As DataGridViewRow In Guna2DataGridView1.Rows
            totalIncome += Convert.ToDecimal(row.Cells("TotalIncome").Value)
        Next

        ' คำนวณยอดรวมรายจ่ายจาก Guna2DataGridView2
        For Each row As DataGridViewRow In Guna2DataGridView2.Rows
            totalExpense += Convert.ToDecimal(row.Cells("TotalExpense").Value)
        Next

        ' คำนวณผลต่างระหว่างรายได้และรายจ่าย
        Dim difference As Decimal = totalIncome - totalExpense

        ' แสดงยอดรวมรายได้และรายจ่ายใน TextBox
        txtTotalIncome.Text = totalIncome.ToString("N2")
        txtTotalExpense.Text = totalExpense.ToString("N2")

        ' ตรวจสอบถ้าผลต่างเป็นลบ ให้แสดงเป็นวงเล็บ
        If difference < 0 Then
            txtDifference.Text = "(" & Math.Abs(difference).ToString("N2") & ")"
        Else
            txtDifference.Text = difference.ToString("N2")
        End If

        ' ตรวจสอบค่า txtDifference
        CheckDifference() ' เรียกใช้ฟังก์ชันตรวจสอบค่า txtDifference
    End Sub


    ' Button click event to show income and expense data
    Private Sub btnShowData_Click(sender As Object, e As EventArgs) Handles btnShowData.Click
        Dim selectedAccId As String = ComboBox1.SelectedValue.ToString()

        ' ดึงวันที่ที่เลือกจาก DateTimePicker1
        Dim selectedDate As DateTime = DateTimePicker1.Value

        ' กรองข้อมูลตามบัญชีและวันที่ที่เลือก
        LoadIncomeData(selectedAccId, selectedDate)
        LoadExpenseData(selectedAccId, selectedDate)

        ' เรียกใช้เมธอด CustomizeDataGridView หลังจากโหลดข้อมูล
        CustomizeDataGridView()

        ' คำนวณยอดรวมรายได้และรายจ่าย
        CalculateTotals()
    End Sub



    Private Sub AddIncomeData()
        ' ตรวจสอบว่ามีการกรอกจำนวนเงินที่ถูกต้องหรือไม่
        Dim incomeAmount As Decimal
        If Not Decimal.TryParse(txtIncomeAmount.Text, incomeAmount) OrElse incomeAmount <= 0 Then
            MessageBox.Show("กรุณากรอกจำนวนเงินสำหรับรายได้ที่ถูกต้อง", "ข้อมูลไม่ครบ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' ถ้าผู้ใช้ไม่ได้เลือกจาก ComboBox ให้ใส่เป็นค่าเริ่มต้นหรือปล่อยว่าง
        Dim incomeName As String = If(ComboBoxIncomeName.SelectedIndex = -1, "ไม่ระบุ", ComboBoxIncomeName.SelectedItem.ToString())

        ' เพิ่มข้อมูลเข้า DataGridView
        If TypeOf Guna2DataGridView1.DataSource Is DataTable Then
            Dim dt As DataTable = CType(Guna2DataGridView1.DataSource, DataTable)
            Dim newRow As DataRow = dt.NewRow()
            newRow("ind_accname") = incomeName
            newRow("TotalIncome") = incomeAmount.ToString("N2")
            dt.Rows.Add(newRow)
        Else
            Guna2DataGridView1.Rows.Add(incomeName, incomeAmount.ToString("N2"))
        End If

        ' รีเซ็ต ComboBox และ TextBox หลังจากเพิ่มข้อมูลแล้ว
        ComboBoxIncomeName.SelectedIndex = -1
        txtIncomeAmount.Clear()

        ' อัปเดตการคำนวณยอดรวม
        CalculateTotals()
    End Sub

    Private Sub AddExpenseData()
        ' ตรวจสอบว่ามีการกรอกจำนวนเงินที่ถูกต้องหรือไม่
        Dim expenseAmount As Decimal
        If Not Decimal.TryParse(txtExpenseAmount.Text, expenseAmount) OrElse expenseAmount <= 0 Then
            MessageBox.Show("กรุณากรอกจำนวนเงินสำหรับรายจ่ายที่ถูกต้อง", "ข้อมูลไม่ครบ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' ถ้าผู้ใช้ไม่ได้เลือกจาก ComboBox ให้ใส่เป็นค่าเริ่มต้นหรือปล่อยว่าง
        Dim expenseName As String = If(ComboBoxExpenseName.SelectedIndex = -1, "ไม่ระบุ", ComboBoxExpenseName.SelectedItem.ToString())

        ' เพิ่มข้อมูลเข้า DataGridView
        If TypeOf Guna2DataGridView2.DataSource Is DataTable Then
            Dim dt As DataTable = CType(Guna2DataGridView2.DataSource, DataTable)
            Dim newRow As DataRow = dt.NewRow()
            newRow("exd_nameacc") = expenseName
            newRow("TotalExpense") = expenseAmount.ToString("N2")
            dt.Rows.Add(newRow)
        Else
            Guna2DataGridView2.Rows.Add(expenseName, expenseAmount.ToString("N2"))
        End If

        ' รีเซ็ต ComboBox และ TextBox หลังจากเพิ่มข้อมูลแล้ว
        ComboBoxExpenseName.SelectedIndex = -1
        txtExpenseAmount.Clear()

        ' อัปเดตการคำนวณยอดรวม
        CalculateTotals()
    End Sub

    ' Button click event to add new income data
    Private Sub btnAddIncome_Click(sender As Object, e As EventArgs) Handles btnAddIncome.Click
        AddIncomeData() ' เรียกใช้ฟังก์ชันเพิ่มข้อมูลรายได้
    End Sub

    ' Button click event to add new expense data
    Private Sub btnAddExpense_Click(sender As Object, e As EventArgs) Handles btnAddExpense.Click
        AddExpenseData() ' เรียกใช้ฟังก์ชันเพิ่มข้อมูลรายจ่าย
    End Sub

    ' ฟังก์ชันหลักสำหรับจัดรูปแบบจำนวนเงินใน Guna2TextBox
    Private Sub FormatCurrencyInput(textBox As Guna.UI2.WinForms.Guna2TextBox)
        ' ตรวจสอบว่า TextBox ไม่ว่างเปล่า
        If Not String.IsNullOrWhiteSpace(textBox.Text) Then
            Dim cursorPosition As Integer = textBox.SelectionStart ' เก็บตำแหน่งเคอร์เซอร์เดิม
            Dim value As Decimal

            ' ลองแปลงค่าเป็น Decimal (ข้ามกรณีที่เป็น string ว่างหรือ error)
            If Decimal.TryParse(textBox.Text.Replace(",", ""), value) Then
                ' ปรับฟอร์แมตใหม่ แต่ไม่กระพริบเคอร์เซอร์
                textBox.Text = String.Format("{0:N2}", value)

                ' ตรวจสอบไม่ให้เคอร์เซอร์เลื่อนเกินความยาว Text ใหม่
                If cursorPosition > textBox.Text.Length Then
                    cursorPosition = textBox.Text.Length
                End If

                ' คืนค่าเคอร์เซอร์ไปตำแหน่งเดิมอย่างราบรื่น
                textBox.SelectionStart = cursorPosition
                textBox.SelectionLength = 0 ' ป้องกันการเลือกข้อความโดยไม่ได้ตั้งใจ
            End If
        End If
    End Sub

    ' เรียกใช้ฟังก์ชันจัดรูปแบบเมื่อมีการเปลี่ยนแปลงใน TextBox
    Private Sub txtIncomeAmount_TextChanged(sender As Object, e As EventArgs) Handles txtIncomeAmount.TextChanged
        FormatCurrencyInput(txtIncomeAmount)
    End Sub

    Private Sub txtExpenseAmount_TextChanged(sender As Object, e As EventArgs) Handles txtExpenseAmount.TextChanged
        FormatCurrencyInput(txtExpenseAmount)
    End Sub



    ' ปุ่มบันทึกข้อมูล
    Private Sub btnSaveData_Click(sender As Object, e As EventArgs) Handles btnSaveData.Click
        SaveData() ' เรียกใช้ฟังก์ชันบันทึกข้อมูล
    End Sub


    Private Sub SaveData()
        ' ดึง path ของฐานข้อมูลจาก config.ini
        Dim dbPath As String = GetDatabasePath()
        Dim connStr As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;"

        ' เชื่อมต่อกับฐานข้อมูล
        Using conn As New OleDbConnection(connStr)
            Dim transaction As OleDbTransaction = Nothing ' ประกาศตัวแปร transaction ไว้ภายนอก Try...Catch

            Try
                conn.Open()

                ' เริ่มการทำธุรกรรม
                transaction = conn.BeginTransaction()

                ' วันที่จาก DateTimePicker1
                Dim selectedDate As DateTime = DateTimePicker1.Value

                ' เตรียมคำสั่ง SQL สำหรับบันทึกรายการใน Income_Details
                Dim incomeCmd As New OleDbCommand("INSERT INTO Income_Details (ind_accname, ind_amount, acc_id, ind_date) VALUES (@ind_accname, @ind_amount, @acc_id, @ind_date)", conn, transaction)

                ' เตรียมคำสั่ง SQL สำหรับบันทึกรายการใน Expense_Details
                Dim expenseCmd As New OleDbCommand("INSERT INTO Expense_Details (exd_nameacc, exd_amount, acc_id, exd_date) VALUES (@exd_nameacc, @exd_amount, @acc_id, @exd_date)", conn, transaction)

                ' ลิสต์รายการที่ต้องตรวจสอบ (ต้องบันทึกใน Income_Details เท่านั้น)
                Dim specificAccounts As String() = {"กำไรสะสมบัญชี1", "กำไรสะสมบัญชีเงินสัจจะ", "กำไรสะสมบัญชีประชารัฐ",
                                                "เงินประกันความเสี่ยงบัญชี1", "เงินประกันความเสี่ยงบัญชีเงินสัจจะ", "เงินประกันความเสี่ยงบัญชีประชารัฐ",
                                                "เงินสมทบบัญชี1", "เงินสมทบบัญชีเงินสัจจะ", "เงินสมทบบัญชีประชารัฐ"}

                ' วนลูปบันทึกข้อมูลจาก Guna2DataGridView1 (รายได้)
                For Each row As DataGridViewRow In Guna2DataGridView1.Rows
                    If Not row.IsNewRow Then
                        Dim accountName As String = row.Cells("ind_accname").Value.ToString()
                        Dim amount As Decimal = Convert.ToDecimal(row.Cells("TotalIncome").Value)

                        ' ตรวจสอบว่าชื่อบัญชีอยู่ใน specificAccounts หรือไม่
                        If specificAccounts.Contains(accountName) Then
                            ' บันทึกใน Income_Details โดยบันทึกเป็นลบ (เพราะมาจากฝั่งรายได้)
                            incomeCmd.Parameters.Clear()
                            incomeCmd.Parameters.AddWithValue("@ind_accname", accountName)
                            incomeCmd.Parameters.AddWithValue("@ind_amount", -amount) ' บันทึกเป็นค่าลบ
                            incomeCmd.Parameters.AddWithValue("@acc_id", ComboBox1.SelectedValue.ToString())
                            incomeCmd.Parameters.AddWithValue("@ind_date", selectedDate.ToString("yyyy-MM-dd"))
                            incomeCmd.ExecuteNonQuery()
                        Else
                            ' ถ้าไม่ใช่ specificAccounts บันทึกลงตาราง Income_Details ตามปกติ (ค่าลบเสมอ)
                            incomeCmd.Parameters.Clear()
                            incomeCmd.Parameters.AddWithValue("@ind_accname", accountName)
                            incomeCmd.Parameters.AddWithValue("@ind_amount", -amount) ' บันทึกเป็นค่าลบเสมอ
                            incomeCmd.Parameters.AddWithValue("@acc_id", ComboBox1.SelectedValue.ToString())
                            incomeCmd.Parameters.AddWithValue("@ind_date", selectedDate.ToString("yyyy-MM-dd"))
                            incomeCmd.ExecuteNonQuery()
                        End If
                    End If
                Next

                ' วนลูปบันทึกข้อมูลจาก Guna2DataGridView2 (ค่าใช้จ่าย)
                For Each row As DataGridViewRow In Guna2DataGridView2.Rows
                    If Not row.IsNewRow Then
                        Dim accountName As String = row.Cells("exd_nameacc").Value.ToString()
                        Dim amount As Decimal = Convert.ToDecimal(row.Cells("TotalExpense").Value)

                        ' ตรวจสอบว่าชื่อบัญชีอยู่ใน specificAccounts หรือไม่
                        If specificAccounts.Contains(accountName) Then
                            ' บันทึกใน Income_Details โดยบันทึกเป็นบวก (เพราะมาจากฝั่งค่าใช้จ่าย)
                            incomeCmd.Parameters.Clear()
                            incomeCmd.Parameters.AddWithValue("@ind_accname", accountName)
                            incomeCmd.Parameters.AddWithValue("@ind_amount", amount) ' บันทึกเป็นค่าบวก
                            incomeCmd.Parameters.AddWithValue("@acc_id", ComboBox1.SelectedValue.ToString())
                            incomeCmd.Parameters.AddWithValue("@ind_date", selectedDate.ToString("yyyy-MM-dd"))
                            incomeCmd.ExecuteNonQuery()
                        Else
                            ' ถ้าไม่ใช่ specificAccounts บันทึกใน Expense_Details แต่เป็นลบเสมอ
                            expenseCmd.Parameters.Clear()
                            expenseCmd.Parameters.AddWithValue("@exd_nameacc", accountName)
                            expenseCmd.Parameters.AddWithValue("@exd_amount", -amount) ' บันทึกเป็นค่าลบเสมอ
                            expenseCmd.Parameters.AddWithValue("@acc_id", ComboBox1.SelectedValue.ToString())
                            expenseCmd.Parameters.AddWithValue("@exd_date", selectedDate.ToString("yyyy-MM-dd"))
                            expenseCmd.ExecuteNonQuery()
                        End If
                    End If
                Next

                ' ยืนยันการทำธุรกรรม
                transaction.Commit()

                ' แสดงข้อความความสำเร็จ
                MessageBox.Show("ข้อมูลได้บันทึกในตาราง Income_Details และ Expense_Details สำเร็จ", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                ' รีเฟรชข้อมูลใน DataGridView
                LoadIncomeData(ComboBox1.SelectedValue.ToString(), selectedDate) ' รีโหลดข้อมูลรายได้
                LoadExpenseData(ComboBox1.SelectedValue.ToString(), selectedDate) ' รีโหลดข้อมูลรายจ่าย

                ' ตั้งค่า txtTotalIncome และ txtTotalExpense ให้เป็น 0 หลังจากปิดยอด
                txtTotalIncome.Text = "0.00"
                txtTotalExpense.Text = "0.00"

            Catch ex As Exception
                ' ตรวจสอบว่ามีการเริ่มต้น transaction หรือไม่ก่อนทำการ rollback
                If transaction IsNot Nothing Then
                    transaction.Rollback() ' ยกเลิกการทำธุรกรรม
                End If
                MessageBox.Show("Error saving data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    ' ฟังก์ชันสำหรับตรวจสอบค่า txtDifference และควบคุมการใช้งานของปุ่มปิดงบ
    Private Sub CheckDifference()
        Dim difference As Decimal

        ' ตรวจสอบว่าค่าใน txtDifference เป็นตัวเลขหรือไม่
        If Decimal.TryParse(txtDifference.Text, difference) Then
            ' ถ้าค่า difference ไม่เท่ากับ 0 ให้ปิดการใช้งานปุ่ม
            If difference <> 0 Then
                btnSaveData.Enabled = False
            Else
                btnSaveData.Enabled = True
            End If
        Else
            ' ถ้าไม่สามารถแปลงเป็นตัวเลขได้ ให้ปิดการใช้งานปุ่ม
            btnSaveData.Enabled = False
        End If
    End Sub

End Class