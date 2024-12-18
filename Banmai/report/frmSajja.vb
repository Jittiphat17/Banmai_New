﻿Imports Microsoft.Reporting.WinForms
Imports System.Data.OleDb
Imports System.Globalization ' ใช้สำหรับ CultureInfo ภาษาไทย
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.IO
Public Class frmSajja

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
    Private Sub frmSajja_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

        ' เพิ่มรายการเดือนใน ComboBox1 โดยใช้ชื่อเดือนภาษาไทย
        Dim thaiCulture As New CultureInfo("th-TH")
        For i As Integer = 1 To 12
            ComboBox1.Items.Add(thaiCulture.DateTimeFormat.GetMonthName(i))
        Next
        ComboBox1.Items.Add("ทั้งหมด") ' เพิ่มตัวเลือกสำหรับยอดรวมทั้งปี

        ' เพิ่มรายการปีใน ComboBox2 (แสดงข้อมูล 5 ปีย้อนหลังในรูปแบบ ค.ศ.)
        Dim currentYear As Integer = DateTime.Now.Year ' ค.ศ. ไม่ต้องแปลง

        ' เติม ComboBox2 ด้วยปี ค.ศ.
        For i As Integer = 0 To 4
            ComboBox2.Items.Add(currentYear - i) ' ใช้ปี ค.ศ.
        Next

        ' ตั้งค่าเริ่มต้น
        ComboBox1.SelectedIndex = DateTime.Now.Month - 1
        ComboBox2.SelectedIndex = 0

        ' โหลดข้อมูลรายงานสำหรับเดือนและปีที่เลือก
        LoadReportData(ComboBox1.SelectedIndex + 1, CInt(ComboBox2.SelectedItem))
    End Sub


    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        LoadReportData(ComboBox1.SelectedIndex + 1, CInt(ComboBox2.SelectedItem))
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        LoadReportData(ComboBox1.SelectedIndex + 1, CInt(ComboBox2.SelectedItem))
    End Sub

    Private Sub LoadReportData(selectedMonth As Integer, selectedYear As Integer)
        Try
            ' Convert selectedYear to ค.ศ. (CE)
            Dim selectedCEYear As Integer = selectedYear

            If Conn.State = ConnectionState.Closed Then Conn.Open()


            ' สร้าง DataSet แรกสำหรับข้อมูลสมาชิก
            Dim ds1 As New DataSet()
                Using adapter1 As New OleDbDataAdapter("SELECT m_id, m_name, m_address, m_tel FROM Member", conn)
                    adapter1.Fill(ds1, "MemberDataSet")
                End Using

                ' สร้าง DataSet ที่สองสำหรับข้อมูลเงินฝากสัจจะเฉพาะเดือนและปีที่เลือก หรือยอดรวมทั้งปี
                Dim ds2 As New DataSet()
                Dim query As String
                Using adapter2 As New OleDbDataAdapter()

                    If ComboBox1.SelectedItem.ToString() = "ทั้งหมด" Then
                        ' Query สำหรับยอดรวมทั้งปี
                        query = "SELECT d.m_id, SUM(d.ind_amount) AS ind_amount " &
                            "FROM Income_Details d " &
                            "WHERE d.ind_accname = 'เงินฝากสัจจะ' AND YEAR(d.ind_date) = @Year " &
                            "GROUP BY d.m_id"
                        adapter2.SelectCommand = New OleDbCommand(query, conn)
                        adapter2.SelectCommand.Parameters.AddWithValue("@Year", selectedCEYear) ' ใช้ ค.ศ.
                    Else
                        ' Query สำหรับเดือนและปีที่เลือก
                        query = "SELECT d.m_id, SUM(d.ind_amount) AS ind_amount " &
                            "FROM Income_Details d " &
                            "WHERE d.ind_accname = 'เงินฝากสัจจะ' AND MONTH(d.ind_date) = @Month AND YEAR(d.ind_date) = @Year " &
                            "GROUP BY d.m_id"
                        adapter2.SelectCommand = New OleDbCommand(query, conn)
                        adapter2.SelectCommand.Parameters.AddWithValue("@Month", selectedMonth)
                        adapter2.SelectCommand.Parameters.AddWithValue("@Year", selectedCEYear) ' ใช้ ค.ศ.
                    End If

                    adapter2.Fill(ds2, "IncomeDataSet")

                    ' Debug: แสดงข้อมูลที่ถูกดึงมา
                    Debug.WriteLine($"จำนวนรายการที่พบ: {ds2.Tables("IncomeDataSet").Rows.Count}")
                    For Each row As DataRow In ds2.Tables("IncomeDataSet").Rows
                        Debug.WriteLine($"m_id: {row("m_id")}, จำนวนเงินฝากสัจจะ: {row("ind_amount")}")
                    Next
                End Using

                ' เพิ่มข้อมูลสำหรับสมาชิกที่ไม่มีรายการฝากในเดือนที่เลือก หรือทั้งปี
                For Each memberRow As DataRow In ds1.Tables("MemberDataSet").Rows
                    Dim memberID As String = memberRow("m_id").ToString()
                    Dim incomeRows As DataRow() = ds2.Tables("IncomeDataSet").Select($"m_id = '{memberID}'")

                    If incomeRows.Length = 0 Then
                        Dim newRow As DataRow = ds2.Tables("IncomeDataSet").NewRow()
                        newRow("m_id") = memberID
                        newRow("ind_amount") = 0
                        ds2.Tables("IncomeDataSet").Rows.Add(newRow)
                        Debug.WriteLine($"เพิ่มข้อมูลสำหรับสมาชิก m_id: {memberID} ที่ไม่มีรายการฝาก")
                    End If
                Next

            ' ตั้งค่าเส้นทางรายงาน
            Dim reportPath As String = GetReportPath("Sajja")

            If Not IO.File.Exists(reportPath) Then
                    MessageBox.Show($"ไม่พบไฟล์รายงาน: {reportPath}")
                    Return
                End If

                Me.ReportViewer1.LocalReport.ReportPath = reportPath

                ' ผูกข้อมูลกับ ReportViewer
                Me.ReportViewer1.LocalReport.DataSources.Clear()
                Me.ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("MemberDataSet", ds1.Tables("MemberDataSet")))
                Me.ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("IncomeDataSet", ds2.Tables("IncomeDataSet")))

                ' เพิ่มพารามิเตอร์เพื่อแสดงวัน เดือน ปี ค.ศ. ในรายงาน
                Dim thaiCulture As New CultureInfo("th-TH")
                Dim monthName As String = If(ComboBox1.SelectedItem.ToString() = "ทั้งหมด", "ทั้งปี", thaiCulture.DateTimeFormat.GetMonthName(selectedMonth))
                Dim reportDate As String

                ' ถ้าเลือก "ทั้งหมด"
                If ComboBox1.SelectedItem.ToString() = "ทั้งหมด" Then
                    reportDate = $"ทั้งปี {selectedCEYear}" ' ใช้ปี ค.ศ.
                Else
                    reportDate = $"{DateTime.Now.Day} {monthName} {selectedCEYear}" ' ใช้วัน, เดือน, และปี ค.ศ.
                End If

                ' ส่งพารามิเตอร์ไปยังรายงาน
                Dim reportParams As New List(Of ReportParameter) From {
                New ReportParameter("ReportDate", reportDate)
            }

                Me.ReportViewer1.LocalReport.SetParameters(reportParams)

                ' Refresh รายงาน
                Me.ReportViewer1.RefreshReport()

        Catch ex As Exception
            MessageBox.Show($"เกิดข้อผิดพลาด: {ex.Message}")
            Debug.WriteLine($"ข้อผิดพลาด: {ex.ToString()}")
        End Try
    End Sub




End Class