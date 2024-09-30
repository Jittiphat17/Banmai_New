Imports System.Data.OleDb
Imports Microsoft.Reporting.WinForms

Public Class frmDebtor
    Private ReadOnly connString As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb"

    ' เพิ่ม DataSet เพื่อเก็บข้อมูล
    Private incomeDataSet As New DataSet()
    Private memberDataSet As New DataSet()

    Private Sub frmDebtor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' โหลดข้อมูลสำหรับรายงาน
        LoadReportData()
    End Sub

    Private Sub LoadReportData()
        Try
            Using conn As New OleDbConnection(connString)
                conn.Open()

                ' Query สำหรับการรวมข้อมูลจากตาราง Income และ Income_Details
                Dim incomeDetailsQuery As String = "
                    SELECT 
                        i.m_id, i.inc_date, i.inc_amount, 
                        d.ind_accname, d.ind_amount
                    FROM 
                        Income i 
                    INNER JOIN 
                        Income_Details d ON i.inc_id = d.inc_id
                "
                Using incomeAdapter As New OleDbDataAdapter(incomeDetailsQuery, conn)
                    incomeAdapter.Fill(incomeDataSet, "IncomeDataSet")
                End Using

                ' DataSet สำหรับข้อมูลจากตาราง Member
                Dim memberQuery As String = "SELECT m_id, m_name FROM Member"
                Using memberAdapter As New OleDbDataAdapter(memberQuery, conn)
                    memberAdapter.Fill(memberDataSet, "MemberDataSet")
                End Using

                ' ตรวจสอบว่าไฟล์รายงานมีอยู่หรือไม่
                Dim reportPath As String = $"D:\Project-2022\Banmai\Banmai\report\Individualdebtor.rdlc"
                If Not IO.File.Exists(reportPath) Then
                    MessageBox.Show($"ไม่พบไฟล์รายงานที่: {reportPath}")
                    Return
                End If

                ' ตั้งค่าเส้นทางของรายงาน
                Me.ReportViewer1.LocalReport.ReportPath = reportPath

                ' ผูกข้อมูล DataSet เข้ากับ ReportViewer
                Me.ReportViewer1.LocalReport.DataSources.Clear()
                Me.ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("IncomeDataSet", incomeDataSet.Tables("IncomeDataSet")))
                Me.ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("MemberDataSet", memberDataSet.Tables("MemberDataSet")))

                ' ทำการ Refresh รายงาน
                Me.ReportViewer1.RefreshReport()
            End Using
        Catch ex As Exception
            MessageBox.Show($"เกิดข้อผิดพลาด: {ex.Message}")
        End Try
    End Sub
End Class
