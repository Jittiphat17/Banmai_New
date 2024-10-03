Imports Microsoft.Reporting.WinForms
Imports System.Data.OleDb

Public Class frmShare
    Private Sub frmShare_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' เพิ่มรายการเดือนลงใน ComboBox
        cmbMonth.Items.Add("มกราคม")
        cmbMonth.Items.Add("กุมภาพันธ์")
        cmbMonth.Items.Add("มีนาคม")
        cmbMonth.Items.Add("เมษายน")
        cmbMonth.Items.Add("พฤษภาคม")
        cmbMonth.Items.Add("มิถุนายน")
        cmbMonth.Items.Add("กรกฎาคม")
        cmbMonth.Items.Add("สิงหาคม")
        cmbMonth.Items.Add("กันยายน")
        cmbMonth.Items.Add("ตุลาคม")
        cmbMonth.Items.Add("พฤศจิกายน")
        cmbMonth.Items.Add("ธันวาคม")

        ' เลือกค่าเริ่มต้นเป็นเดือนปัจจุบัน
        cmbMonth.SelectedIndex = DateTime.Now.Month - 1
    End Sub

    Private Sub btnGenerateReport_Click(sender As Object, e As EventArgs) Handles btnGenerateReport.Click
        ' การเชื่อมต่อกับฐานข้อมูล
        Dim connString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb"
        Dim conn As New OleDbConnection(connString)

        ' ดึงเดือนที่เลือกจาก ComboBox
        Dim selectedMonth As Integer = cmbMonth.SelectedIndex + 1 ' เดือนที่เลือกจาก ComboBox
        Dim selectedYear As Integer = DateTime.Now.Year ' สามารถเพิ่ม ComboBox สำหรับปีได้

        ' สร้าง DataSet แรกสำหรับข้อมูลสมาชิก
        Dim query1 As String = "SELECT m_id, m_name, m_address, m_tel FROM Member"
        Dim adapter1 As New OleDbDataAdapter(query1, conn)
        Dim ds1 As New DataSet()

        ' สร้าง DataSet ที่สองสำหรับข้อมูลเงินหุ้นที่กรองตามเดือน
        Dim query2 As String = "SELECT m_id, ind_amount FROM Income_Details WHERE ind_accname = 'เงินหุ้น' " &
                               "AND MONTH(ind_date) = @month AND YEAR(ind_date) = @year"
        Dim adapter2 As New OleDbDataAdapter(query2, conn)
        adapter2.SelectCommand.Parameters.AddWithValue("@month", selectedMonth)
        adapter2.SelectCommand.Parameters.AddWithValue("@year", selectedYear)
        Dim ds2 As New DataSet()

        Try
            conn.Open()
            ' เติมข้อมูลให้กับ DataSets ทั้งสอง
            adapter1.Fill(ds1, "MemberDataSet")
            adapter2.Fill(ds2, "IncomeDataSet")
            conn.Close()

            ' ตั้งค่าเส้นทางรายงาน
            Me.ReportViewer1.LocalReport.ReportPath = "D:\Project-2022\Banmai\Banmai\report\shareReport.rdlc"
            ' ตรวจสอบว่าไฟล์รายงานมีอยู่จริงหรือไม่
            If Not IO.File.Exists(Me.ReportViewer1.LocalReport.ReportPath) Then
                MessageBox.Show("Report file not found: " & Me.ReportViewer1.LocalReport.ReportPath)
                Return
            End If

            ' ผูกข้อมูลกับ ReportViewer
            Me.ReportViewer1.LocalReport.DataSources.Clear()
            Me.ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("MemberDataSet", ds1.Tables("MemberDataSet")))
            Me.ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("IncomeDataSet", ds2.Tables("IncomeDataSet")))

            ' Refresh รายงาน
            Me.ReportViewer1.RefreshReport()

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub
End Class
