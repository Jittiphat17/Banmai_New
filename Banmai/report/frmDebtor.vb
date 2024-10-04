Imports System.Data.OleDb
Imports Banmai.individualDSTableAdapters
Imports Microsoft.Reporting.WinForms

Public Class frmDebtor
    Private ReadOnly connString As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb"

    ' เพิ่ม DataSet เพื่อเก็บข้อมูล
    Private incomeDataSet As New DataSet()
    Private memberDataSet As New DataSet()

    Private Sub frmDebtor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'Db_banmai1DataSet.Account' table. You can move, or remove it, as needed.
        Me.AccountTableAdapter.Fill(Me.Db_banmai1DataSet.Account)
        ' โหลดข้อมูลสำหรับรายงาน
        initialOnload()
        LoadReportData()
    End Sub
    Private Sub LoadReportData()
        Dim month As Integer = Integer.Parse(drpMonth.SelectedValue.ToString())
        Dim year As Integer = Integer.Parse(drpYear.SelectedItem.ToString())

        ' Get the first and last date of the month
        Dim firstDate As New DateTime(year, month, 1)
        Dim lastDate As New DateTime(year, month, DateTime.DaysInMonth(year, month))

        Try
            Using conn As New OleDbConnection(connString)
                conn.Open()



                'month -1
                Dim beforeDS As New individualDS()
                Dim beforeAdapter As New DataTable1TableAdapter()
                beforeAdapter.filterdate(beforeDS.DataTable1, firstDate.AddMonths(-1), lastDate.AddMonths(-1), ComboBox1.SelectedValue)
                Dim reportDataSourceBefore As New ReportDataSource("before1MonthDS", beforeDS.Tables(0))


                'Contract
                Dim contDS As New individualDS()
                Dim contAdapter As New dtContractTableAdapter()
                contAdapter.filterContract(contDS.dtContract, firstDate, lastDate, ComboBox1.SelectedValue)
                Dim reportDataSourceCont As New ReportDataSource("contDS", contDS.Tables(0))


                'Search
                Dim ds As New individualDS()
                Dim adapter As New DataTable1TableAdapter()
                'adapter.Fill(ds.DataTable1) 

                adapter.filterdate(ds.DataTable1, firstDate, lastDate, ComboBox1.SelectedValue)
                Dim reportDataSource As New ReportDataSource("individualDS", ds.Tables(0))

                Dim dtcont As DataTable = contDS.Tables("dtContract")
                Dim dt As DataTable = ds.Tables("DataTable1")

                'รวม Object กู้ใหม่กับ Payment
                dt.Merge(dtcont)

                ReportViewer1.LocalReport.DataSources.Clear()
                ReportViewer1.LocalReport.DataSources.Add(reportDataSourceBefore)
                ReportViewer1.LocalReport.DataSources.Add(reportDataSource)



                ' ตรวจสอบว่าไฟล์รายงานมีอยู่หรือไม่
                Dim reportPath As String = $"D:\Project-2022\Banmai\Banmai\report\Individualdebtor.rdlc"
                ReportViewer1.ProcessingMode = ProcessingMode.Local

                If Not IO.File.Exists(reportPath) Then
                    MessageBox.Show($"ไม่พบไฟล์รายงานที่: {reportPath}")
                    Return
                End If

                '' ตั้งค่าเส้นทางของรายงาน
                Me.ReportViewer1.LocalReport.ReportPath = reportPath


                ' Create the parameters to pass to the report
                Dim parameters As New List(Of ReportParameter)()
                parameters.Add(New ReportParameter("SelectedMonth", month))
                parameters.Add(New ReportParameter("SelectedYear", year))

                ' Set the parameters for the report
                ReportViewer1.LocalReport.SetParameters(parameters)

                ' ทำการ Refresh รายงาน
                Me.ReportViewer1.RefreshReport()
            End Using
        Catch ex As Exception
            MessageBox.Show($"เกิดข้อผิดพลาด: {ex.Message}")
        End Try
    End Sub

    Private Sub initialOnload()
        ComboBox1.SelectedValue = "ACC002"

        drpMonth.DataSource = New List(Of KeyValuePair(Of Integer, String)) From {
            New KeyValuePair(Of Integer, String)(1, "มกราคม"),
            New KeyValuePair(Of Integer, String)(2, "กุมภาพันธ์"),
            New KeyValuePair(Of Integer, String)(3, "มีนาคม"),
            New KeyValuePair(Of Integer, String)(4, "เมษายน"),
            New KeyValuePair(Of Integer, String)(5, "พฤษภาคม"),
            New KeyValuePair(Of Integer, String)(6, "มิถุนายน"),
            New KeyValuePair(Of Integer, String)(7, "กรกฎาคม"),
            New KeyValuePair(Of Integer, String)(8, "สิงหาคม"),
            New KeyValuePair(Of Integer, String)(9, "กันยายน"),
            New KeyValuePair(Of Integer, String)(10, "ตุลาคม"),
            New KeyValuePair(Of Integer, String)(11, "พฤศจิกายน"),
            New KeyValuePair(Of Integer, String)(12, "ธันวาคม")
        }
        drpMonth.DisplayMember = "Value"
        drpMonth.ValueMember = "Key"
        drpMonth.SelectedValue = 10

        drpYear.Items.Clear()

        ' Get the current year
        Dim currentYear As Integer = DateTime.Now.Year

        ' Loop to add the current year and the next 10 years
        For i As Integer = 0 To 10
            drpYear.Items.Add(currentYear + i)
        Next

        ' Optionally set the selected value to the current year
        drpYear.SelectedItem = currentYear
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        LoadReportData()
    End Sub

End Class
