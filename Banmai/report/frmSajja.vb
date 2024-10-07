Imports Microsoft.Reporting.WinForms
Imports System.Data.OleDb
Imports System.Globalization ' ใช้สำหรับ CultureInfo ภาษาไทย
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class frmSajja
    Private ReadOnly connString As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb"

    Private Sub frmSajja_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' เพิ่มรายการเดือนใน ComboBox1 โดยใช้ชื่อเดือนภาษาไทย
        Dim thaiCulture As New CultureInfo("th-TH")
        For i As Integer = 1 To 12
            ComboBox1.Items.Add(thaiCulture.DateTimeFormat.GetMonthName(i))
        Next
        ComboBox1.Items.Add("ทั้งหมด") ' เพิ่มตัวเลือกสำหรับยอดรวมทั้งปี

        ' เพิ่มรายการปีใน ComboBox2 (แสดงข้อมูล 5 ปีย้อนหลังในรูปแบบ พ.ศ.)
        Dim currentYear As Integer = DateTime.Now.Year + 543 ' แปลงเป็น พ.ศ.
        For i As Integer = 0 To 4
            ComboBox2.Items.Add(currentYear - i)
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
            Using conn As New OleDbConnection(connString)
                conn.Open()

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
                        adapter2.SelectCommand.Parameters.AddWithValue("@Year", selectedYear - 543) ' แปลงเป็น ค.ศ.
                    Else
                        ' Query สำหรับเดือนและปีที่เลือก
                        query = "SELECT d.m_id, SUM(d.ind_amount) AS ind_amount " &
                                "FROM Income_Details d " &
                                "WHERE d.ind_accname = 'เงินฝากสัจจะ' AND MONTH(d.ind_date) = @Month AND YEAR(d.ind_date) = @Year " &
                                "GROUP BY d.m_id"
                        adapter2.SelectCommand = New OleDbCommand(query, conn)
                        adapter2.SelectCommand.Parameters.AddWithValue("@Month", selectedMonth)
                        adapter2.SelectCommand.Parameters.AddWithValue("@Year", selectedYear - 543) ' แปลงเป็น ค.ศ.
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
                Dim reportPath As String = "D:\Project-2022\Banmai\Banmai\report\SajjaReport.rdlc"

                If Not IO.File.Exists(reportPath) Then
                    MessageBox.Show($"ไม่พบไฟล์รายงาน: {reportPath}")
                    Return
                End If

                Me.ReportViewer1.LocalReport.ReportPath = reportPath

                ' ผูกข้อมูลกับ ReportViewer
                Me.ReportViewer1.LocalReport.DataSources.Clear()
                Me.ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("MemberDataSet", ds1.Tables("MemberDataSet")))
                Me.ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("IncomeDataSet", ds2.Tables("IncomeDataSet")))

                ' เพิ่มพารามิเตอร์เพื่อแสดงวัน เดือน ปี พ.ศ. ในรายงาน
                Dim thaiCulture As New CultureInfo("th-TH")
                Dim monthName As String = If(ComboBox1.SelectedItem.ToString() = "ทั้งหมด", "ทั้งหมด", thaiCulture.DateTimeFormat.GetMonthName(selectedMonth))
                Dim reportDate As String = $"{DateTime.Now.Day} {monthName} {selectedYear}" ' วันที่ เดือน ปี พ.ศ.

                Dim reportParams As New List(Of ReportParameter) From {
                    New ReportParameter("ReportDate", reportDate)
                }

                Me.ReportViewer1.LocalReport.SetParameters(reportParams)

                ' Refresh รายงาน
                Me.ReportViewer1.RefreshReport()
            End Using
        Catch ex As Exception
            MessageBox.Show($"เกิดข้อผิดพลาด: {ex.Message}")
            Debug.WriteLine($"ข้อผิดพลาด: {ex.ToString()}")
        End Try
    End Sub

    Private Sub btnExportExcel_Click(sender As Object, e As EventArgs) Handles btnExportExcel.Click
        Try
            ' ดึงข้อมูลจาก ReportViewer ในรูปแบบ Excel (รูปแบบ Excel แบบเก่า .xls)
            Dim warnings As Warning() = Nothing
            Dim streamids As String() = Nothing
            Dim mimeType As String = String.Empty
            Dim encoding As String = String.Empty
            Dim extension As String = "xls" ' ใช้ .xls แทน .xlsx
            Dim deviceInfo As String = "<DeviceInfo>" &
                                       "  <OutputFormat>Excel</OutputFormat>" &
                                       "  <PageWidth>8.5in</PageWidth>" &
                                       "  <PageHeight>11in</PageHeight>" &
                                       "  <MarginTop>0.5in</MarginTop>" &
                                       "  <MarginLeft>0.5in</MarginLeft>" &
                                       "  <MarginRight>0.5in</MarginRight>" &
                                       "  <MarginBottom>0.5in</MarginBottom>" &
                                       "</DeviceInfo>"

            ' Render รายงานเป็น Excel (.xls)
            Dim renderedBytes As Byte() = ReportViewer1.LocalReport.Render("EXCEL", deviceInfo, mimeType, encoding, extension, streamids, warnings)

            ' แสดง Dialog ให้ผู้ใช้เลือกตำแหน่งที่จะบันทึกไฟล์
            Dim saveFileDialog As New SaveFileDialog()
            saveFileDialog.Filter = "Excel Files|*.xls"
            saveFileDialog.Title = "Save Report as Excel"
            saveFileDialog.FileName = "Sajja_Report.xls" ' ใช้ .xls แทน .xlsx

            If saveFileDialog.ShowDialog() = DialogResult.OK Then
                ' บันทึกไฟล์ Excel
                Using fs As New IO.FileStream(saveFileDialog.FileName, IO.FileMode.Create)
                    fs.Write(renderedBytes, 0, renderedBytes.Length)
                End Using

                MessageBox.Show("บันทึกไฟล์สำเร็จ!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show($"เกิดข้อผิดพลาดในการบันทึกไฟล์: {ex.Message}")
        End Try
    End Sub
    Private Sub btnExportPDF_Click(sender As Object, e As EventArgs) Handles btnExportPDF.Click
        Try
            ' ดึงข้อมูลจาก ReportViewer ในรูปแบบ PDF
            Dim warnings As Warning() = Nothing
            Dim streamids As String() = Nothing
            Dim mimeType As String = String.Empty
            Dim encoding As String = String.Empty
            Dim extension As String = "pdf"
            Dim deviceInfo As String = "<DeviceInfo>" &
                                       "  <OutputFormat>PDF</OutputFormat>" &
                                       "  <PageWidth>8.5in</PageWidth>" &
                                       "  <PageHeight>11in</PageHeight>" &
                                       "  <MarginTop>0.5in</MarginTop>" &
                                       "  <MarginLeft>0.5in</MarginLeft>" &
                                       "  <MarginRight>0.5in</MarginRight>" &
                                       "  <MarginBottom>0.5in</MarginBottom>" &
                                       "</DeviceInfo>"

            ' Render รายงานเป็น PDF
            Dim renderedBytes As Byte() = ReportViewer1.LocalReport.Render("PDF", deviceInfo, mimeType, encoding, extension, streamids, warnings)

            ' แสดง Dialog ให้ผู้ใช้เลือกตำแหน่งที่จะบันทึกไฟล์
            Dim saveFileDialog As New SaveFileDialog()
            saveFileDialog.Filter = "PDF Files|*.pdf"
            saveFileDialog.Title = "Save Report as PDF"
            saveFileDialog.FileName = "Sajja_Report.pdf"

            If saveFileDialog.ShowDialog() = DialogResult.OK Then
                ' บันทึกไฟล์ PDF
                Using fs As New IO.FileStream(saveFileDialog.FileName, IO.FileMode.Create)
                    fs.Write(renderedBytes, 0, renderedBytes.Length)
                End Using

                MessageBox.Show("บันทึกไฟล์สำเร็จ!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Catch ex As Exception
            MessageBox.Show($"เกิดข้อผิดพลาดในการบันทึกไฟล์: {ex.Message}")
        End Try
    End Sub

End Class