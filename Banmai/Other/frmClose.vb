Imports System.Data.OleDb
Imports System.Drawing ' ใช้สำหรับการตั้งค่าฟอนต์

Public Class frmClose
    Private formLoaded As Boolean = False

    Private Sub frmClose_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' ปิดการใช้งาน ComboBox จนกว่าจะเลือกวันที่
        ComboBox1.Enabled = False

        ' ตั้งค่า MaxDate เพื่อไม่ให้เลือกวันที่ในอนาคตได้
        DateTimePicker1.MaxDate = DateTime.Now

        ' ตั้งค่า MinDate ให้ DateTimePicker2 เท่ากับ DateTimePicker1
        DateTimePicker2.MinDate = DateTimePicker1.Value

        ' โหลดข้อมูลบัญชีทั้งหมดลงใน ComboBox
        LoadAccounts()

        ' บอกว่าฟอร์มได้ทำการโหลดแล้ว
        formLoaded = True
    End Sub

    ' Method to load account names into ComboBox
    Private Sub LoadAccounts()
        Dim dtAccounts As New DataTable

        ' Connect to database and retrieve account names and ids
        Using conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
            Try
                Dim query As String = "SELECT acc_id, acc_name FROM Account"
                Dim cmd As New OleDbCommand(query, conn)
                Dim adapter As New OleDbDataAdapter(cmd)
                adapter.Fill(dtAccounts)

                ' Bind account names to ComboBox
                ComboBox1.DataSource = dtAccounts
                ComboBox1.DisplayMember = "acc_name"
                ComboBox1.ValueMember = "acc_id" ' ใช้ acc_id เป็นค่าที่จะใช้ในการกรอง
            Catch ex As Exception
                MessageBox.Show("Error loading accounts: " & ex.Message)
            End Try
        End Using
    End Sub

    ' Method to load income data into Guna2DataGridView1 พร้อมกรองวันที่และชื่อเฉพาะ
    Private Sub LoadIncomeData(accId As String, startDate As DateTime, endDate As DateTime)
        Dim dtIncome As New DataTable

        ' Connect to database and retrieve income data filtered by acc_id, date, and specific names
        Using conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
            Try
                ' Filter by acc_id, date range, and specific income names
                Dim query As String = "SELECT ind_accname, SUM(ind_amount) AS TotalIncome " &
                                  "FROM Income_Details " &
                                  "WHERE acc_id = @accId AND ind_date BETWEEN @startDate AND @endDate " &
                                  "AND ind_accname IN ('ค่าธรรมเนียม', 'เงินบริจาค', 'ค่าสัญญา', 'ค่าประกัน', 'ดอกเบี้ยเงินฝากธนาคาร', 'ค่าธรรมเนียมแรกเข้า', 'อื่น ๆ', 'ดอกเบี้ย', 'ค่าปรับ') " &
                                  "GROUP BY ind_accname"
                Dim cmd As New OleDbCommand(query, conn)
                cmd.Parameters.AddWithValue("@accId", accId)
                cmd.Parameters.AddWithValue("@startDate", startDate)
                cmd.Parameters.AddWithValue("@endDate", endDate)
                Dim adapter As New OleDbDataAdapter(cmd)
                adapter.Fill(dtIncome)
            Catch ex As Exception
                MessageBox.Show("Error loading income data: " & ex.Message)
            End Try
        End Using

        ' Bind the data to the DataGridView
        Me.Guna2DataGridView1.DataSource = dtIncome
    End Sub


    ' Method to load expense data into Guna2DataGridView2 พร้อมกรองวันที่และชื่อเฉพาะ
    Private Sub LoadExpenseData(accId As String, startDate As DateTime, endDate As DateTime)
        Dim dtExpense As New DataTable

        ' Connect to database and retrieve expense data filtered by acc_id, date, and specific names
        Using conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
            Try
                ' Filter by acc_id, date range, and specific expense names
                Dim query As String = "SELECT exd_nameacc, SUM(exd_amount) AS TotalExpense " &
                                  "FROM Expense_Details " &
                                  "WHERE acc_id = @accId AND exd_date BETWEEN @startDate AND @endDate " &
                                  "AND exd_nameacc IN ('ค่าเช่าสำนักงาน', 'เงินสมทบ', 'เงินประกันความเสี่ยง', 'ค่าตอบแทน', 'ค่าจ้าง', 'ดอกเบี้ยสัจจะ', 'ดอกเบี้ยจ่าย', 'อื่นๆ') " &
                                  "GROUP BY exd_nameacc"
                Dim cmd As New OleDbCommand(query, conn)
                cmd.Parameters.AddWithValue("@accId", accId)
                cmd.Parameters.AddWithValue("@startDate", startDate)
                cmd.Parameters.AddWithValue("@endDate", endDate)
                Dim adapter As New OleDbDataAdapter(cmd)
                adapter.Fill(dtExpense)
            Catch ex As Exception
                MessageBox.Show("Error loading expense data: " & ex.Message)
            End Try
        End Using

        ' Bind the data to the DataGridView
        Me.Guna2DataGridView2.DataSource = dtExpense
    End Sub



    ' Method to customize the DataGridView
    Private Sub CustomizeDataGridView()
        ' ตั้งค่าใช้ฟอนต์ FC Minimal ขนาด 16 สำหรับเนื้อหาในตาราง (ตัวปกติ)
        Dim fcMinimalFont As New Font("FC Minimal", 16, FontStyle.Regular)
        ' ตั้งค่าใช้ฟอนต์ FC Minimal ขนาด 16 สำหรับหัวตาราง (ตัวหนา)
        Dim fcMinimalBoldFont As New Font("FC Minimal", 16, FontStyle.Bold)

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



    ' Method to calculate total income and expense, and the difference
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
    End Sub


    ' Method to handle btnShowData click event
    Private Sub btnShowData_Click(sender As Object, e As EventArgs) Handles btnShowData.Click
        Dim selectedAccId As String = ComboBox1.SelectedValue.ToString()

        ' ดึงวันที่เริ่มต้นและสิ้นสุดจาก DateTimePicker
        Dim startDate As DateTime = DateTimePicker1.Value
        Dim endDate As DateTime = DateTimePicker2.Value

        ' กรองข้อมูลตามบัญชีและวันที่ที่เลือก
        LoadIncomeData(selectedAccId, startDate, endDate)
        LoadExpenseData(selectedAccId, startDate, endDate)

        ' เรียกใช้เมธอด CustomizeDataGridView หลังจากโหลดข้อมูล
        CustomizeDataGridView()

        ' คำนวณยอดรวมรายได้และรายจ่าย
        CalculateTotals()
    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged
        ' ตรวจสอบว่าผู้ใช้เลือกวันที่เริ่มต้นล่วงหน้าหรือไม่
        If formLoaded AndAlso DateTimePicker1.Value > DateTime.Now Then
            ' แสดงข้อความแจ้งเตือนว่าไม่สามารถเลือกวันที่ในอนาคตได้
            MessageBox.Show("ไม่สามารถเลือกวันที่เริ่มต้นที่เป็นล่วงหน้าได้ กรุณาเลือกวันที่ใหม่", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ComboBox1.Enabled = False ' ปิดการใช้งาน ComboBox ถ้าเลือกวันที่ผิด
        Else
            ' ตั้งค่า MinDate ให้ DateTimePicker2 ตามวันที่เริ่มต้น
            DateTimePicker2.MinDate = DateTimePicker1.Value

            ' เปิดการใช้งาน ComboBox เมื่อเลือกวันที่ถูกต้อง
            ComboBox1.Enabled = True

            ' ตรวจสอบวันที่สิ้นสุดเมื่อวันที่เริ่มต้นถูกเปลี่ยน
            If formLoaded AndAlso DateTimePicker2.Value < DateTimePicker1.Value Then
                ' แสดงข้อความแจ้งเตือนว่ามีการเลือกวันที่สิ้นสุดก่อนวันที่เริ่มต้น
                MessageBox.Show("วันที่สิ้นสุดต้องไม่อยู่ก่อนวันที่เริ่มต้น กรุณาเลือกวันที่ใหม่", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        End If
    End Sub

    Private Sub DateTimePicker2_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker2.ValueChanged
        ' ตรวจสอบว่าผู้ใช้เลือกวันที่สิ้นสุดไม่ให้อยู่ก่อนวันที่เริ่มต้น
        If formLoaded AndAlso DateTimePicker2.Value < DateTimePicker1.Value Then
            ' แสดงข้อความแจ้งเตือนว่ามีการเลือกวันที่สิ้นสุดก่อนวันที่เริ่มต้น
            MessageBox.Show("วันที่สิ้นสุดต้องไม่อยู่ก่อนวันที่เริ่มต้น กรุณาเลือกวันที่ใหม่", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

End Class