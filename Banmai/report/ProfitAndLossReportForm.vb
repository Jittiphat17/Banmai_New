Imports System.Data.OleDb
Imports Microsoft.Reporting.WinForms

Public Class ProfitAndLossReportForm

    ' การเชื่อมต่อกับฐานข้อมูล
    Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb"

    Private Sub IncomeExpenseReportForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ' สร้าง DataTable สำหรับรายได้
        Dim dtIncome As New DataTable()
        dtIncome.Columns.Add("ind_accname", GetType(String))
        dtIncome.Columns.Add("ind_amount", GetType(Decimal))

        ' สร้าง DataTable สำหรับค่าใช้จ่าย
        Dim dtExpense As New DataTable()
        dtExpense.Columns.Add("exd_nameacc", GetType(String))
        dtExpense.Columns.Add("exd_amount", GetType(Decimal))

        Using connection As New OleDbConnection(connectionString)
            connection.Open()

            ' ดึงข้อมูลรายได้ทั้งหมด โดยรวมยอดรายการที่ซ้ำกัน
            Using incomeCmd As New OleDbCommand("SELECT ind_accname, SUM(ind_amount) AS total_amount FROM Income_Details GROUP BY ind_accname", connection)
                Dim reader As OleDbDataReader = incomeCmd.ExecuteReader()
                While reader.Read()
                    dtIncome.Rows.Add(reader("ind_accname").ToString(), Convert.ToDecimal(reader("total_amount")))
                End While
            End Using

            ' ดึงข้อมูลค่าใช้จ่ายทั้งหมด โดยรวมยอดรายการที่ซ้ำกัน
            Using expenseCmd As New OleDbCommand("SELECT exd_nameacc, SUM(exd_amount) AS exd_amount FROM Expense_Details GROUP BY exd_nameacc", connection)
                Dim reader As OleDbDataReader = expenseCmd.ExecuteReader()
                While reader.Read()
                    dtExpense.Rows.Add(reader("exd_nameacc").ToString(), Convert.ToDecimal(reader("exd_amount")))
                End While
            End Using

        End Using

        ' สร้าง ReportDataSource สำหรับรายได้
        Dim incomeDataSource As New ReportDataSource("IncomeDataSet", dtIncome)

        ' สร้าง ReportDataSource สำหรับค่าใช้จ่าย
        Dim expenseDataSource As New ReportDataSource("ExpenseDataSet", dtExpense)

        ' กำหนด ReportViewer
        ReportViewer1.LocalReport.DataSources.Clear()
        ReportViewer1.LocalReport.DataSources.Add(incomeDataSource)
        ReportViewer1.LocalReport.DataSources.Add(expenseDataSource)
        ReportViewer1.LocalReport.ReportPath = "D:\Project-2022\Banmai\Banmai\report\test.rdlc" ' ระบุที่อยู่ของไฟล์ .rdlc

        ' แสดงรายงาน
        ReportViewer1.RefreshReport()
    End Sub
End Class
