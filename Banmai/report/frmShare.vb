Imports Microsoft.Reporting.WinForms
Imports System.Data.OleDb
Imports System.IO

Public Class frmShare
    Private Conn As New OleDbConnection

    ' ฟังก์ชันสำหรับดึงค่า path ของฐานข้อมูลจาก config.ini
    Private Function GetDatabasePath() As String
        Dim iniPath As String = Path.Combine(Application.StartupPath, "config.ini")
        If Not File.Exists(iniPath) Then
            Throw New Exception("ไม่พบไฟล์ config.ini ที่ตำแหน่ง: " & iniPath)
        End If

        ' อ่านบรรทัดทั้งหมดใน config.ini
        Dim lines = File.ReadAllLines(iniPath)

        ' ค้นหาบรรทัดที่มี Path
        Dim dbPathLine = lines.FirstOrDefault(Function(line) line.StartsWith("Path="))
        If String.IsNullOrEmpty(dbPathLine) Then
            Throw New Exception("ไม่พบ 'Path' ในไฟล์ config.ini")
        End If

        ' ดึง path จากบรรทัดนั้นและตัดส่วน 'Path=' ออก
        Dim dbPath = dbPathLine.Replace("Path=", "").Trim()

        ' แปลง path เป็น path แบบเต็ม (Absolute Path)
        If dbPath.StartsWith(".\") Then
            dbPath = Path.Combine(Application.StartupPath, dbPath.Substring(2))
        End If

        If Not File.Exists(dbPath) Then
            Throw New Exception($"ไม่พบไฟล์ฐานข้อมูลที่ตำแหน่ง: {dbPath}")
        End If

        Return dbPath
    End Function
    Private Function GetReportPath(reportName As String) As String
        Dim iniPath As String = Path.Combine(Application.StartupPath, "config.ini")
        If Not File.Exists(iniPath) Then
            Throw New Exception("ไม่พบไฟล์ config.ini ที่: " & iniPath)
        End If

        ' อ่านบรรทัดทั้งหมดใน config.ini
        Dim lines = File.ReadAllLines(iniPath)

        ' ค้นหาบรรทัดที่มีชื่อรายงานตรงกับ key ที่ส่งมา
        Dim reportPathLine = lines.FirstOrDefault(Function(line) line.StartsWith(reportName & "="))
        If String.IsNullOrEmpty(reportPathLine) Then
            Throw New Exception($"ไม่พบรายงาน '{reportName}' ใน config.ini")
        End If

        ' ดึง path ของรายงานและแปลงเป็น Absolute Path ถ้าจำเป็น
        Dim reportPath = reportPathLine.Replace(reportName & "=", "").Trim()
        reportPath = Path.Combine(Application.StartupPath, reportPath)

        ' ตรวจสอบว่าไฟล์รายงานมีอยู่จริง
        If Not File.Exists(reportPath) Then
            Throw New Exception($"ไม่พบไฟล์รายงานที่: {reportPath}")
        End If

        Return reportPath
    End Function
    Private Sub frmShare_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' ดึง path ของฐานข้อมูลจาก config.ini
            Dim dbPath As String = GetDatabasePath()
            Dim connStr As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;"
            Conn = New OleDbConnection(connStr)

        Catch ex As Exception
            ' แสดงข้อความข้อผิดพลาดเมื่อไม่สามารถเชื่อมต่อกับฐานข้อมูลได้
            MessageBox.Show($"เกิดข้อผิดพลาด: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Exit() ' ปิดโปรแกรมหากไม่สามารถเชื่อมต่อได้
        End Try

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
        Try
            ' ดึงค่า path ของฐานข้อมูลจาก config.ini
            Dim dbPath As String = GetDatabasePath()
            Dim connString As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};Persist Security Info=False;"
            Dim conn As New OleDbConnection(connString)

            ' ดึงเดือนและปีที่เลือกจาก ComboBox
            Dim selectedMonth As Integer = cmbMonth.SelectedIndex + 1
            Dim selectedYear As Integer = DateTime.Now.Year ' สามารถปรับให้เลือกปีได้ตามความต้องการ

            ' สร้าง DataSet แรกสำหรับข้อมูลสมาชิก
            Dim query1 As String = "SELECT m_id, m_name, m_address, m_tel FROM Member"
            Dim adapter1 As New OleDbDataAdapter(query1, conn)
            Dim ds1 As New DataSet()

            ' สร้าง DataSet ที่สองสำหรับข้อมูลเงินหุ้นตามเดือนและปีที่เลือก
            Dim query2 As String = "SELECT m_id, ind_amount FROM Income_Details WHERE ind_accname = 'เงินหุ้น' " &
                                   "AND MONTH(ind_date) = @month AND YEAR(ind_date) = @year"
            Dim adapter2 As New OleDbDataAdapter(query2, conn)
            adapter2.SelectCommand.Parameters.AddWithValue("@month", selectedMonth)
            adapter2.SelectCommand.Parameters.AddWithValue("@year", selectedYear)
            Dim ds2 As New DataSet()

            ' เปิดการเชื่อมต่อและดึงข้อมูลใส่ DataSet
            conn.Open()
            adapter1.Fill(ds1, "MemberDataSet")
            adapter2.Fill(ds2, "IncomeDataSet")
            conn.Close()

            ' ดึง path ของรายงานจาก config.ini
            Dim reportPath As String = GetReportPath("Share")
            Me.ReportViewer1.LocalReport.ReportPath = reportPath

            ' ตรวจสอบว่าไฟล์รายงานมีอยู่จริง
            If Not IO.File.Exists(reportPath) Then
                MessageBox.Show("Report file not found: " & reportPath)
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
