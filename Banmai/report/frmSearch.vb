Imports System.Data.OleDb
Imports System.Text
Imports Microsoft.Reporting.WinForms
Imports System.IO

Public Class frmSearch
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

    Private Sub frmSearch_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

        ' โหลดข้อมูลทั้งหมดทันทีเมื่อเปิดฟอร์ม
        LoadAllContracts()
        LoadDirectors()

        Try
            ' ดึง path ของรายงานสัญญากู้ยืมจาก config.ini
            Dim loanReportPath As String = GetReportPath("Loan")
            Me.ReportViewer1.LocalReport.ReportPath = loanReportPath

            ' ดึง path ของรายงานผู้ค้ำประกันจาก config.ini
            Dim guarantorReportPath As String = GetReportPath("Guarantor")
            Me.ReportViewer2.LocalReport.ReportPath = guarantorReportPath

            ' รีเฟรชรายงาน
            Me.ReportViewer1.RefreshReport()
            Me.ReportViewer2.RefreshReport()

        Catch ex As Exception
            ' แสดงข้อความข้อผิดพลาดหากไม่พบไฟล์รายงาน
            MessageBox.Show($"เกิดข้อผิดพลาดในการโหลดรายงาน: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadDirectors()
        Try
            ' ดึง path ของฐานข้อมูลจาก config.ini
            Dim dbPath As String = GetDatabasePath()
            Dim connStr As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath}"

            Using conn As New OleDbConnection(connStr)
                conn.Open()

                ' ดึงข้อมูลกรรมการพร้อมชื่อและคำนำหน้าจากตาราง Member โดย JOIN กับ Director
                Dim query As String = "
            SELECT d.fu_id, m.m_name, m.m_gender, m.m_id 
            FROM Director d 
            INNER JOIN Member m ON d.m_id = m.m_id"

                Dim cmd As New OleDbCommand(query, conn)
                Dim adapter As New OleDbDataAdapter(cmd)
                Dim table As New DataTable()
                adapter.Fill(table)

                ' เพิ่มคอลัมน์ใหม่เพื่อรวมคำนำหน้าและชื่อ
                table.Columns.Add("FullName", GetType(String), "m_gender + ' ' + m_name") ' สร้างคอลัมน์ FullName

                ' ใส่ข้อมูลกรรมการ (ชื่อพร้อมคำนำหน้า) ลงใน ComboBox ทั้งสอง
                ComboBoxDirector1.DataSource = table.Copy() ' สำเนาตารางสำหรับ ComboBox ตัวแรก
                ComboBoxDirector2.DataSource = table.Copy() ' สำเนาตารางสำหรับ ComboBox ตัวที่สอง

                ' แสดงชื่อกรรมการใน ComboBox
                ComboBoxDirector1.DisplayMember = "FullName" ' แสดงชื่อกรรมการรวมคำนำหน้า
                ComboBoxDirector2.DisplayMember = "FullName"

                ' ค่าเบื้องหลังยังคงเป็น m_id เพื่อใช้ในการอ้างอิง
                ComboBoxDirector1.ValueMember = "m_id"
                ComboBoxDirector2.ValueMember = "m_id"
            End Using
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการดึงข้อมูลกรรมการ: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnReport.Click
        If String.IsNullOrWhiteSpace(txtSearch.Text) Then
            LoadAllContracts()
        Else
            SearchContracts(txtSearch.Text)
        End If
    End Sub
    Private Sub FormatDataGridView()
        ' สร้างฟอนต์ FC Minimal Bold ขนาด 12pt
        Dim fcMinimalBoldFont As New Font("FC Minimal", 16)

        ' ตั้งค่าฟอนต์ให้กับ DataGridView
        dgvResults.Font = fcMinimalBoldFont
        dgvResults.RowTemplate.Height = 40


        ' ตั้งค่าสีสำหรับ Header
        dgvResults.EnableHeadersVisualStyles = False
        dgvResults.ColumnHeadersDefaultCellStyle.BackColor = Color.MediumSlateBlue
        dgvResults.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvResults.ColumnHeadersDefaultCellStyle.Font = fcMinimalBoldFont

        ' ตั้งค่าการจัดข้อความสำหรับคอลัมน์ตัวเลข
        dgvResults.Columns("con_amount").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvResults.Columns("con_interest").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        dgvResults.Columns("con_permonth").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

        ' เพิ่มการจัดรูปแบบตัวเลขให้มีเครื่องหมายคอมมา
        dgvResults.Columns("con_amount").DefaultCellStyle.Format = "N2" ' แสดงจำนวนเงินด้วยทศนิยม 2 ตำแหน่ง และเครื่องหมายคอมมา
        dgvResults.Columns("con_interest").DefaultCellStyle.Format = "N2"
        dgvResults.Columns("con_permonth").DefaultCellStyle.Format = "N2"

        ' ตั้งค่าให้ข้อความในเซลล์ตัดบรรทัด (Wrap Text)
        dgvResults.DefaultCellStyle.WrapMode = DataGridViewTriState.True

        ' ปรับขนาดคอลัมน์ที่ต้องการขนาดเฉพาะ
        dgvResults.Columns("con_details").Width = 200
        dgvResults.Columns("con_amount").Width = 100
        dgvResults.Columns("con_interest").Width = 80
        dgvResults.Columns("con_permonth").Width = 80

        ' ตั้งค่าการเลื่อน (Scrollbar)
        dgvResults.ScrollBars = ScrollBars.Both ' เปิด scrollbar ทั้งแนวตั้งและแนวนอน

        ' ตั้งค่าให้คอลัมน์ขยายอัตโนมัติและเติมเต็มพื้นที่ DataGridView
        dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        ' กำหนดชื่อคอลัมน์ให้กับ DataGridView
        dgvResults.Columns("con_id").HeaderText = "รหัสสัญญา"
        dgvResults.Columns("m_id").HeaderText = "รหัสสมาชิก"
        dgvResults.Columns("con_details").HeaderText = "รายละเอียดสัญญา"
        dgvResults.Columns("con_amount").HeaderText = "จำนวนเงิน"
        dgvResults.Columns("con_interest").HeaderText = "ดอกเบี้ย"
        dgvResults.Columns("con_permonth").HeaderText = "จำนวนงวดต่อเดือน"
        dgvResults.Columns("con_date").HeaderText = "วันที่ทำสัญญา"
        dgvResults.Columns("acc_id").HeaderText = "ชื่อบัญชี"
        dgvResults.Columns("guarantor_names").HeaderText = "ชื่อผู้ค้ำประกัน"
        dgvResults.Columns("con_GuaranteeType").HeaderText = "ประเภทการค้ำประกัน"


    End Sub




    Private Sub LoadAllContracts()
        Try
            ' ดึง path ของฐานข้อมูลจาก config.ini
            Dim dbPath As String = GetDatabasePath()
            Dim connStr As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath}"

            Using conn As New OleDbConnection(connStr)
                conn.Open()
                Dim query As String = "SELECT con_id, m_id, con_details, con_amount, con_interest, con_permonth, con_date, acc_id, con_GuaranteeType FROM Contract"
                Dim cmd As New OleDbCommand(query, conn)
                Dim adapter As New OleDbDataAdapter(cmd)
                Dim table As New DataTable()
                adapter.Fill(table)

                ' Add the guarantor names as before
                table.Columns.Add("guarantor_names", GetType(String)) ' Add new column for guarantor names

                For Each row As DataRow In table.Rows
                    Dim con_id As Integer = row("con_id")
                    Dim guarantorQuery As String = "SELECT m.m_name FROM Guarantor g INNER JOIN Member m ON g.m_id = m.m_id WHERE g.con_id = @con_id"
                    Dim guarantorCmd As New OleDbCommand(guarantorQuery, conn)
                    guarantorCmd.Parameters.AddWithValue("@con_id", con_id)
                    Dim guarantorAdapter As New OleDbDataAdapter(guarantorCmd)
                    Dim guarantorTable As New DataTable()
                    guarantorAdapter.Fill(guarantorTable)

                    ' Collect guarantor names
                    Dim guarantorNames As String = String.Join(", ", guarantorTable.AsEnumerable().[Select](Function(r) r.Field(Of String)("m_name")).ToArray())
                    row("guarantor_names") = guarantorNames
                Next

                ' Update DataGridView DataSource
                dgvResults.DataSource = table

                ' Refresh DataGridView to show new data
                dgvResults.Refresh()

                ' Call the format function to setup DataGridView
                FormatDataGridView()
            End Using
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการเชื่อมต่อฐานข้อมูล: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    Private Sub SearchContracts(keyword As String)
        Try
            ' ดึง path ของฐานข้อมูลจาก config.ini
            Dim dbPath As String = GetDatabasePath()
            Dim connStr As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath}"

            Using conn As New OleDbConnection(connStr)
                conn.Open()
                Dim query As String = "SELECT con_id, m_id, con_details, con_amount, con_interest, con_permonth, con_date, acc_id, con_GuaranteeType FROM Contract WHERE con_id LIKE @keyword OR m_id IN (SELECT m_id FROM Member WHERE m_name LIKE @keyword)"
                Dim cmd As New OleDbCommand(query, conn)
                cmd.Parameters.AddWithValue("@keyword", "%" & keyword & "%")
                Dim adapter As New OleDbDataAdapter(cmd)
                Dim table As New DataTable()
                adapter.Fill(table)

                ' Add the guarantor names as before
                table.Columns.Add("guarantor_names", GetType(String)) ' Add new column for guarantor names

                For Each row As DataRow In table.Rows
                    Dim con_id As Integer = row("con_id")
                    Dim guarantorQuery As String = "SELECT m.m_name FROM Guarantor g INNER JOIN Member m ON g.m_id = m.m_id WHERE g.con_id = @con_id"
                    Dim guarantorCmd As New OleDbCommand(guarantorQuery, conn)
                    guarantorCmd.Parameters.AddWithValue("@con_id", con_id)
                    Dim guarantorAdapter As New OleDbDataAdapter(guarantorCmd)
                    Dim guarantorTable As New DataTable()
                    guarantorAdapter.Fill(guarantorTable)

                    ' Collect guarantor names
                    Dim guarantorNames As String = String.Join(", ", guarantorTable.AsEnumerable().[Select](Function(r) r.Field(Of String)("m_name")).ToArray())
                    row("guarantor_names") = guarantorNames
                Next

                dgvResults.DataSource = table
            End Using
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการเชื่อมต่อฐานข้อมูล: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Public Function ConvertNumberToText(ByVal num As Decimal) As String
        Dim bahtText As String = ""
        Dim numParts() As String = Split(num.ToString("F2"), ".")
        Dim bahtPart As Long = CLng(numParts(0))
        Dim satangPart As Integer = CInt(numParts(1))

        Dim bahtWords As String = ConvertPartToText(bahtPart)
        Dim satangWords As String = ConvertPartToText(satangPart)

        If bahtWords <> "" Then
            bahtText &= bahtWords & "บาท"
        End If

        If satangWords <> "" Then
            bahtText &= satangWords & "สตางค์"
        Else
            bahtText &= "ถ้วน"
        End If

        Return bahtText
    End Function

    Private Function ConvertPartToText(ByVal num As Long) As String
        Dim text As String = ""
        Dim units() As String = {"", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน"}
        Dim nums() As String = {"", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า"}

        Dim digit As Integer
        Dim pos As Integer = 0

        While num > 0
            digit = num Mod 10
            If digit > 0 Then
                text = nums(digit) & units(pos) & text
            End If
            num = num \ 10
            pos += 1
        End While

        text = text.Replace("หนึ่งสิบ", "สิบ")
        text = text.Replace("สองสิบ", "ยี่สิบ")

        ' แก้ไขการแปลงตัวเลข "หนึ่ง" ในหลักสิบหน่วย
        If text.Length > 1 AndAlso text.Substring(1, 1) = "สิบ" Then
            text = text.Substring(0, 1) & "เอ็ด" & text.Substring(2)
        End If

        Return text
    End Function

    Private Function ConvertToThaiDateString(ByVal dateValue As Date) As String
        Dim thaiMonths As String() = {"มกราคม", "กุมภาพันธ์", "มีนาคม", "เมษายน", "พฤษภาคม", "มิถุนายน", "กรกฎาคม", "สิงหาคม", "กันยายน", "ตุลาคม", "พฤศจิกายน", "ธันวาคม"}
        Dim month As Integer = dateValue.Month
        Dim year As Integer = dateValue.Year + 543 ' เพิ่ม 543 เพื่อแปลงเป็นปีพุทธศักราช (พ.ศ.)

        Return String.Format("{0} พ.ศ. {1}", thaiMonths(month - 1), year)
    End Function
    Private Function ConvertToMonthYear(ByVal dateValue As DateTime) As String
        Dim month As Integer = dateValue.Month
        Dim year As Integer = dateValue.Year + 543 ' แปลงเป็นปีพุทธศักราช (พ.ศ.)

        ' ส่งคืนค่าที่เป็น /เดือน/ปี เช่น /08/2566
        Return String.Format("/{0:D2}/{1}", month, year)
    End Function
    Private Function ConvertToThaiMonthYear(ByVal dateValue As DateTime) As String
        Dim thaiMonths As String() = {"มกราคม", "กุมภาพันธ์", "มีนาคม", "เมษายน", "พฤษภาคม", "มิถุนายน", "กรกฎาคม", "สิงหาคม", "กันยายน", "ตุลาคม", "พฤศจิกายน", "ธันวาคม"}
        Dim month As Integer = dateValue.Month
        Dim year As Integer = dateValue.Year + 543 ' แปลงเป็นปีพุทธศักราช (พ.ศ.)

        ' ส่งคืนค่าที่เป็น เดือน พ.ศ. เช่น เมษายน พ.ศ. 2567
        Return String.Format("{0} พ.ศ. {1}", thaiMonths(month - 1), year)
    End Function


    Private Sub btnReport_Click(sender As Object, e As EventArgs) Handles btnReport.Click
        If dgvResults.SelectedRows.Count = 0 Then
            MessageBox.Show("กรุณาเลือกสัญญาที่ต้องการสร้างรายงาน", "คำเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' ตรวจสอบการเลือกกรรมการจาก ComboBox
        If ComboBoxDirector1.SelectedIndex = -1 Or ComboBoxDirector2.SelectedIndex = -1 Then
            MessageBox.Show("กรุณาเลือกกรรมการ 2 คน", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' ดึง m_id ของกรรมการจาก ComboBox
        Dim director1ID As String = ComboBoxDirector1.SelectedValue.ToString() ' m_id ของกรรมการคนแรก
        Dim director2ID As String = ComboBoxDirector2.SelectedValue.ToString() ' m_id ของกรรมการคนที่สอง

        ' ตรวจสอบว่า m_id ของกรรมการทั้งสองคนไม่เหมือนกัน
        If director1ID = director2ID Then
            MessageBox.Show("กรุณาเลือกกรรมการที่มี m_id ไม่ซ้ำกัน", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' ดึงชื่อกรรมการจาก ComboBox
        Dim director1 As String = ComboBoxDirector1.Text
        Dim director2 As String = ComboBoxDirector2.Text

        Dim selectedConId As String = dgvResults.SelectedRows(0).Cells("con_id").Value.ToString()
        Dim selectedMemberId As String = dgvResults.SelectedRows(0).Cells("m_id").Value.ToString()
        Dim selectedAccId As String = dgvResults.SelectedRows(0).Cells("acc_id").Value.ToString()

        Try
            ' ดึง path ของฐานข้อมูลจาก config.ini
            Dim dbPath As String = GetDatabasePath()
            Dim connStr As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath}"

            Using conn As New OleDbConnection(connStr)
                conn.Open()

                ' ดึงข้อมูลสำหรับ DataSet1 จากตาราง Contract
                Dim query1 As String = "SELECT con_id, m_id, con_details, con_amount, con_interest, con_permonth, con_date, acc_id, con_GuaranteeType FROM Contract WHERE con_id = @con_id"
                Dim cmd1 As New OleDbCommand(query1, conn)
                cmd1.Parameters.AddWithValue("@con_id", selectedConId)
                Dim adapter1 As New OleDbDataAdapter(cmd1)
                Dim table1 As New DataTable()
                adapter1.Fill(table1)

                ' ดึงข้อมูลสำหรับ DataSet2 จากตาราง Member
                Dim query2 As String = "SELECT m_id, m_gender, m_national, m_thaiid, m_name, m_address, m_tel FROM Member WHERE m_id = @m_id"
                Dim cmd2 As New OleDbCommand(query2, conn)
                cmd2.Parameters.AddWithValue("@m_id", selectedMemberId)
                Dim adapter2 As New OleDbDataAdapter(cmd2)
                Dim table2 As New DataTable()
                adapter2.Fill(table2)

                ' ดึงข้อมูลสำหรับ DataSet3 จากตาราง Account
                Dim query3 As String = "SELECT acc_id, acc_name FROM Account WHERE acc_id = @acc_id"
                Dim cmd3 As New OleDbCommand(query3, conn)
                cmd3.Parameters.AddWithValue("@acc_id", selectedAccId)
                Dim adapter3 As New OleDbDataAdapter(cmd3)
                Dim table3 As New DataTable()
                adapter3.Fill(table3)

                ' ดึงข้อมูลสำหรับ DataSet4 จากตาราง Payment
                Dim query4 As String = "SELECT payment_id, con_id, payment_date, payment_amount FROM Payment WHERE con_id = @con_id ORDER BY payment_date ASC"
                Dim cmd4 As New OleDbCommand(query4, conn)
                cmd4.Parameters.AddWithValue("@con_id", selectedConId)
                Dim adapter4 As New OleDbDataAdapter(cmd4)
                Dim table4 As New DataTable()
                adapter4.Fill(table4)

                ' ดึงข้อมูลผู้ค้ำประกันจากตาราง Guarantor และ Member
                Dim query5 As String = "SELECT m.m_gender, m.m_name, m.m_national, m.m_address, m.m_tel, m.m_thaiid FROM Guarantor g INNER JOIN Member m ON g.m_id = m.m_id WHERE g.con_id = @con_id"
                Dim cmd5 As New OleDbCommand(query5, conn)
                cmd5.Parameters.AddWithValue("@con_id", selectedConId)
                Dim adapter5 As New OleDbDataAdapter(cmd5)
                Dim table5 As New DataTable()
                adapter5.Fill(table5)

                ' ชื่อ DataSource ต้องตรงกับชื่อในรายงาน
                Dim rds1 As New ReportDataSource("DataSet1", table1)
                Dim rds2 As New ReportDataSource("DataSet2", table2)
                Dim rds3 As New ReportDataSource("DataSet3", table3)
                Dim rds4 As New ReportDataSource("DataSet4", table4)

                ' กำหนดข้อมูลสำหรับ ReportViewer1 (รายงานสัญญากู้ยืม)
                Me.ReportViewer1.LocalReport.DataSources.Clear()
                Me.ReportViewer1.LocalReport.DataSources.Add(rds1)
                Me.ReportViewer1.LocalReport.DataSources.Add(rds2)
                Me.ReportViewer1.LocalReport.DataSources.Add(rds3)
                Me.ReportViewer1.LocalReport.DataSources.Add(rds4)

                ' ส่งข้อมูลจำนวนเงินเป็นคำอ่านไปยังรายงาน
                Dim conAmount As Decimal = 0
                If Not IsDBNull(table1.Rows(0)("con_amount")) Then
                    conAmount = Convert.ToDecimal(table1.Rows(0)("con_amount"))
                End If

                Dim amountText As String = ConvertNumberToText(conAmount)
                Dim amountWithCommas As String = conAmount.ToString("N0")

                Me.ReportViewer1.LocalReport.SetParameters(New ReportParameter("AmountText", amountText))
                Me.ReportViewer1.LocalReport.SetParameters(New ReportParameter("AmountWithCommas", amountWithCommas))
                Me.ReportViewer2.LocalReport.SetParameters(New ReportParameter("AmountText", amountText))
                Me.ReportViewer2.LocalReport.SetParameters(New ReportParameter("AmountWithCommas", amountWithCommas))

                ' ส่งข้อมูลกรรมการเป็นพารามิเตอร์ไปยังรายงาน
                Me.ReportViewer1.LocalReport.SetParameters(New ReportParameter("DirectorName1", director1))
                Me.ReportViewer1.LocalReport.SetParameters(New ReportParameter("DirectorName2", director2))
                Me.ReportViewer2.LocalReport.SetParameters(New ReportParameter("DirectorName1", director1))
                Me.ReportViewer2.LocalReport.SetParameters(New ReportParameter("DirectorName2", director2))
                ' ส่งข้อมูลวันที่ชำระเงินแรกและสุดท้ายไปยังรายงาน
                Dim firstPaymentDate As Date = Date.MinValue
                Dim lastPaymentDate As Date = Date.MinValue

                If table4.Rows.Count > 0 Then
                    firstPaymentDate = Convert.ToDateTime(table4.Rows(0)("payment_date"))
                    lastPaymentDate = Convert.ToDateTime(table4.Rows(table4.Rows.Count - 1)("payment_date"))
                End If

                Dim firstPaymentDateThai As String = ConvertToThaiDateString(firstPaymentDate)
                Dim lastPaymentDateThai As String = ConvertToThaiDateString(lastPaymentDate)
                Me.ReportViewer1.LocalReport.SetParameters(New ReportParameter("FirstPaymentDate", firstPaymentDateThai))
                Me.ReportViewer1.LocalReport.SetParameters(New ReportParameter("LastPaymentDate", lastPaymentDateThai))

                ' ส่งข้อมูลวันที่ทำรายการไปยังรายงาน
                Dim conDate As DateTime = Convert.ToDateTime(table1.Rows(0)("con_date"))
                Dim conDateMonthYear As String = ConvertToMonthYear(conDate)
                Me.ReportViewer1.LocalReport.SetParameters(New ReportParameter("ConDate", conDateMonthYear))
                Me.ReportViewer2.LocalReport.SetParameters(New ReportParameter("ConDate", conDateMonthYear))

                ' ส่งข้อมูลเดือนและปีไปยังรายงานโดยใช้พารามิเตอร์ conDate1
                Dim conDate1 As DateTime = Convert.ToDateTime(table1.Rows(0)("con_date"))
                Dim conDateMonthYear1 As String = ConvertToThaiMonthYear(conDate)
                Me.ReportViewer1.LocalReport.SetParameters(New ReportParameter("conDate1", conDateMonthYear1))
                Me.ReportViewer2.LocalReport.SetParameters(New ReportParameter("conDate1", conDateMonthYear1))

                ' ตรวจสอบว่ามีผู้ค้ำประกันหรือไม่
                If table5.Rows.Count > 0 Then
                    ' ใช้ StringBuilder เพื่อรวมข้อมูลผู้ค้ำประกัน
                    Dim sb As New StringBuilder()

                    ' สร้างลำดับข้อมูลผู้ค้ำประกัน
                    For i As Integer = 0 To table5.Rows.Count - 1
                        Dim row = table5.Rows(i)

                        ' ตรวจสอบและเพิ่มข้อมูลใน StringBuilder
                        sb.Append((i + 1).ToString() & ".) " &
                  If(Not IsDBNull(row("m_gender")), row.Field(Of String)("m_gender"), "") & " " &
                  If(Not IsDBNull(row("m_name")), row.Field(Of String)("m_name"), "") & " สัญชาติ: " &
                  If(Not IsDBNull(row("m_national")), row.Field(Of String)("m_national"), "") & " ที่อยู่: " &
                  If(Not IsDBNull(row("m_address")), row.Field(Of String)("m_address"), "") & " โทรศัพท์: " &
                  If(Not IsDBNull(row("m_tel")), row.Field(Of String)("m_tel"), "") & " เลขบัตรประชาชน: " &
                  If(Not IsDBNull(row("m_thaiid")), row.Field(Of String)("m_thaiid"), ""))

                        ' เพิ่มการเว้นบรรทัดเล็กน้อยหลังจากข้อมูลของผู้ค้ำแต่ละคน ยกเว้นคนสุดท้าย
                        If i < table5.Rows.Count - 1 Then
                            sb.Append(Environment.NewLine) ' เว้นบรรทัดหนึ่งบรรทัด
                        End If
                    Next

                    ' ตั้งค่า Parameter สำหรับข้อมูลผู้ค้ำใน ReportViewer1
                    Me.ReportViewer1.LocalReport.SetParameters(New ReportParameter("GuarantorDetails", sb.ToString()))
                    Me.ReportViewer2.LocalReport.SetParameters(New ReportParameter("GuarantorDetails", sb.ToString()))

                    ' ตั้งค่าชื่อผู้ค้ำประกันแยกกัน
                    Me.ReportViewer2.LocalReport.SetParameters(New ReportParameter("GuarantorName1", If(table5.Rows.Count > 0, table5.Rows(0)("m_name").ToString(), String.Empty)))
                    Me.ReportViewer2.LocalReport.SetParameters(New ReportParameter("GuarantorName2", If(table5.Rows.Count > 1, table5.Rows(1)("m_name").ToString(), String.Empty)))
                Else
                    ' ไม่ส่ง 'GuarantorDetails' parameter ถ้าไม่มีผู้ค้ำประกัน
                    Me.ReportViewer1.LocalReport.SetParameters(New ReportParameter("GuarantorDetails", String.Empty))
                    Me.ReportViewer2.LocalReport.SetParameters(New ReportParameter("GuarantorDetails", String.Empty))
                    Me.ReportViewer2.LocalReport.SetParameters(New ReportParameter("GuarantorName1", String.Empty))
                    Me.ReportViewer2.LocalReport.SetParameters(New ReportParameter("GuarantorName2", String.Empty))
                End If

                ' รีเฟรชรายงานหลังจากตั้งค่าพารามิเตอร์และ DataSource สำหรับ ReportViewer1
                Me.ReportViewer1.RefreshReport()

                ' กำหนดข้อมูลสำหรับ ReportViewer2 (รายงานผู้ค้ำประกัน)
                If table5.Rows.Count > 0 Then
                    Dim rds5 As New ReportDataSource("DataSet5", table5)
                    Me.ReportViewer2.LocalReport.DataSources.Clear()
                    Me.ReportViewer2.LocalReport.DataSources.Add(rds1)
                    Me.ReportViewer2.LocalReport.DataSources.Add(rds2)
                    Me.ReportViewer2.LocalReport.DataSources.Add(rds3)
                    Me.ReportViewer2.LocalReport.DataSources.Add(rds4)
                    Me.ReportViewer2.LocalReport.DataSources.Add(rds5)
                    Me.ReportViewer2.RefreshReport()
                Else
                    ' ซ่อน ReportViewer2 ถ้าไม่มีผู้ค้ำประกัน
                    Me.ReportViewer2.Visible = False
                End If

            End Using
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการสร้างรายงาน: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


End Class