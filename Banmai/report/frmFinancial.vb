Imports System.Data
Imports System.Data.OleDb
Imports Microsoft.Reporting.WinForms

Public Class frmFinancial

    Private Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")

    Private Sub frmFinancial_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' ระบุเส้นทางไฟล์รายงาน .rdlc
        Me.ReportViewer1.LocalReport.ReportPath = "D:\Project-2022\Banmai\Banmai\report\Financial.rdlc"

        ' โหลดข้อมูลบัญชีใน ComboBox
        LoadAccountNames()
    End Sub

    Private Sub LoadAccountNames()
        Dim accountQuery As String = "SELECT acc_id, acc_name FROM Account" ' ดึงข้อมูลจากตาราง Account
        Dim accountAdapter As New OleDbDataAdapter(accountQuery, Conn)
        Dim accountTable As New DataTable()

        Try
            Conn.Open()
            accountAdapter.Fill(accountTable)

            cmbAccountName.DataSource = accountTable
            cmbAccountName.DisplayMember = "acc_name" ' แสดงชื่อบัญชี
            cmbAccountName.ValueMember = "acc_id" ' ใช้ acc_id เป็นค่าที่แทน
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการโหลดชื่อบัญชี: " & ex.Message)
        Finally
            Conn.Close()
        End Try
    End Sub

    Private Sub LoadReport()
        Dim ds As New DataSet()

        ' ไม่ใช้เงื่อนไขกรอง
        Dim incomeQuery As String = "SELECT * FROM income"
        Dim incomeDetailsQuery As String = "SELECT * FROM income_details"
        Dim expenseQuery As String = "SELECT * FROM expense"
        Dim expenseDetailsQuery As String = "SELECT * FROM expense_details"

        Dim incomeAdapter As New OleDbDataAdapter(incomeQuery, Conn)
        Dim incomeDetailsAdapter As New OleDbDataAdapter(incomeDetailsQuery, Conn)
        Dim expenseAdapter As New OleDbDataAdapter(expenseQuery, Conn)
        Dim expenseDetailsAdapter As New OleDbDataAdapter(expenseDetailsQuery, Conn)

        Try
            Conn.Open()
            incomeAdapter.Fill(ds, "Income")
            incomeDetailsAdapter.Fill(ds, "IncomeDetails")
            expenseAdapter.Fill(ds, "Expense")
            expenseDetailsAdapter.Fill(ds, "ExpenseDetails")

            ' สร้าง DataSources สำหรับ ReportViewer
            Dim incomeDataSource As New ReportDataSource("IncomeDataSet", ds.Tables("Income"))
            Dim incomeDetailsDataSource As New ReportDataSource("IncomeDetailsDataSet", ds.Tables("IncomeDetails"))
            Dim expenseDataSource As New ReportDataSource("ExpenseDataSet", ds.Tables("Expense"))
            Dim expenseDetailsDataSource As New ReportDataSource("ExpenseDetailsDataSet", ds.Tables("ExpenseDetails"))

            ' เพิ่ม DataSources ลงใน ReportViewer
            Me.ReportViewer1.LocalReport.DataSources.Clear()
            Me.ReportViewer1.LocalReport.DataSources.Add(incomeDataSource)
            Me.ReportViewer1.LocalReport.DataSources.Add(incomeDetailsDataSource)
            Me.ReportViewer1.LocalReport.DataSources.Add(expenseDataSource)
            Me.ReportViewer1.LocalReport.DataSources.Add(expenseDetailsDataSource)

            ' โหลดและแสดงผลรายงาน
            Me.ReportViewer1.RefreshReport()
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการโหลดรายงาน: " & ex.Message)
        Finally
            Conn.Close()
        End Try
    End Sub


    ' Event handler สำหรับปุ่ม "สร้างรายงาน"
    Private Sub btnGenerateReport_Click(sender As Object, e As EventArgs) Handles btnGenerateReport.Click
        ' เรียกใช้ฟังก์ชันเพื่อสร้างรายงาน
        LoadReport()
    End Sub
End Class
