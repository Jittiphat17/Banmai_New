Imports Microsoft.Reporting.WinForms
Imports System.Data.OleDb

Public Class frmStatement

    ' สร้างการเชื่อมต่อกับฐานข้อมูล
    Private Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")

    Private Sub frmProfitLossReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' ดึงข้อมูลบัญชีเพื่อลงใน ComboBox
        LoadAccountNames()
    End Sub

    ' ฟังก์ชันสำหรับโหลดข้อมูลบัญชีลงใน ComboBox
    Private Sub LoadAccountNames()
        Try
            Conn.Open()

            ' ดึงชื่อบัญชีจากตาราง Account
            Dim query As String = "SELECT acc_name FROM Account"
            Dim cmd As New OleDbCommand(query, Conn)

            Dim reader As OleDbDataReader = cmd.ExecuteReader()

            ' เติมข้อมูลใน ComboBox
            cmbAccountName.Items.Clear()
            While reader.Read()
                cmbAccountName.Items.Add(reader("acc_name").ToString())
            End While

            reader.Close()
            Conn.Close()
        Catch ex As Exception
            MessageBox.Show("Error loading account names: " & ex.Message)
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub


    Private Sub btnGenerateReport_Click(sender As Object, e As EventArgs) Handles btnGenerateReport.Click
        ' ตรวจสอบว่าผู้ใช้ได้เลือกบัญชีแล้วหรือยัง
        If cmbAccountName.SelectedIndex <> -1 Then
            LoadProfitLossReport() ' ดึงข้อมูลและแสดงรายงาน
        Else
            MessageBox.Show("กรุณาเลือกบัญชีก่อน", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub


    ' ฟังก์ชันสำหรับโหลดรายงานและส่งค่า Parameter ชื่อบัญชีและช่วงวันที่ 1/10/2024
    Private Sub LoadProfitLossReport()
        Try
            ' เปิดการเชื่อมต่อฐานข้อมูล
            Conn.Open()

            ' กำหนดไฟล์ RDLC ที่จะใช้ โดยใช้เส้นทางแบบสัมบูรณ์
            Me.ReportViewer1.LocalReport.ReportPath = "D:\Project-2022\Banmai\Banmai\report\StatementReport.rdlc"

            ' ดึงชื่อบัญชีที่เลือกจาก ComboBox
            Dim selectedAccountName As String = cmbAccountName.SelectedItem.ToString()

            ' ดึงวันที่เริ่มต้นและวันที่สิ้นสุดจาก DateTimePicker
            Dim startDate As Date = dtpStartDate.Value
            Dim endDate As Date = dtpEndDate.Value

            ' สร้าง DataSet สำหรับรายงาน
            Dim ds As New DataSet("dsProfitLoss")

            ' ดึงข้อมูลรายได้จาก Income_Details โดยกรองตาม ind_accname และช่วงวันที่
            Dim dtIncome As New DataTable("Income")
            dtIncome.Columns.Add("description", GetType(String))
            dtIncome.Columns.Add("amount", GetType(Decimal))

            Dim queryIncome As String = "SELECT ind_accname, ind_amount FROM Income_Details WHERE ind_accname = @accountName AND ind_date BETWEEN @startDate AND @endDate"
            Dim cmdIncome As New OleDbCommand(queryIncome, Conn)
            cmdIncome.Parameters.AddWithValue("@accountName", selectedAccountName)
            cmdIncome.Parameters.AddWithValue("@startDate", startDate)
            cmdIncome.Parameters.AddWithValue("@endDate", endDate)

            Dim readerIncome As OleDbDataReader = cmdIncome.ExecuteReader()
            While readerIncome.Read()
                dtIncome.Rows.Add(readerIncome("ind_accname"), readerIncome("ind_amount"))
            End While
            readerIncome.Close()

            ' ดึงข้อมูลค่าใช้จ่ายจาก Expense_Details โดยกรองตาม exd_nameacc และช่วงวันที่
            Dim dtExpense As New DataTable("Expense")
            dtExpense.Columns.Add("description", GetType(String))
            dtExpense.Columns.Add("amount", GetType(Decimal))

            Dim queryExpense As String = "SELECT exd_nameacc, exd_amount FROM Expense_Details WHERE exd_nameacc = @accountName AND exd_date BETWEEN @startDate AND @endDate"
            Dim cmdExpense As New OleDbCommand(queryExpense, Conn)
            cmdExpense.Parameters.AddWithValue("@accountName", selectedAccountName)
            cmdExpense.Parameters.AddWithValue("@startDate", startDate)
            cmdExpense.Parameters.AddWithValue("@endDate", endDate)

            Dim readerExpense As OleDbDataReader = cmdExpense.ExecuteReader()
            While readerExpense.Read()
                dtExpense.Rows.Add(readerExpense("exd_nameacc"), readerExpense("exd_amount"))
            End While
            readerExpense.Close()

            ' เพิ่ม DataTable ลงใน DataSet
            ds.Tables.Add(dtIncome)
            ds.Tables.Add(dtExpense)

            ' ตั้งค่ารายงาน RDLC
            ReportViewer1.LocalReport.DataSources.Clear()

            ' ผูก DataSet กับรายงาน
            ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("IncomeDataSet", dtIncome))
            ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("ExpenseDataSet", dtExpense))

            ' ส่งค่า Parameter ชื่อบัญชีและช่วงวันที่ไปยังรายงาน RDLC
            Dim accountNameParam As New ReportParameter("AccountName", selectedAccountName)
            Dim startDateParam As New ReportParameter("StartDate", startDate.ToString("yyyy-MM-dd"))
            Dim endDateParam As New ReportParameter("EndDate", endDate.ToString("yyyy-MM-dd"))
            ReportViewer1.LocalReport.SetParameters(New ReportParameter() {accountNameParam, startDateParam, endDateParam})

            ' Refresh รายงาน
            ReportViewer1.RefreshReport()

            ' ปิดการเชื่อมต่อฐานข้อมูล
            Conn.Close()
        Catch ex As Exception
            ' แสดงข้อความข้อผิดพลาดถ้าเกิดปัญหา
            MessageBox.Show("Error loading profit-loss report: " & ex.Message)
        Finally
            ' ตรวจสอบว่าฐานข้อมูลถูกปิดอย่างถูกต้องหรือไม่
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

End Class