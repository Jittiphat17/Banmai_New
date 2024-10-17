Imports System.Data.OleDb
Imports Microsoft.Reporting.WinForms
Imports System.Globalization


Public Class frmReceipts

    ' สร้าง Connection String เพื่อเชื่อมต่อกับฐานข้อมูล
    Private Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")

    Private incomeTable As New DataTable() ' เก็บข้อมูลตาราง Income
    Private detailsTable As New DataTable() ' เก็บข้อมูลตาราง Income_Details
    Private selectedIncId As Integer ' เก็บค่า inc_id ที่ถูกเลือกจากเซลล์

    Private Sub frmIncomeReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' เรียกฟังก์ชันดึงข้อมูล
        LoadIncomeData()
    End Sub

    Private Sub LoadIncomeData()
        Try
            Using conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
                conn.Open()

                ' ดึงข้อมูลจากตาราง Income
                Dim query As String = "SELECT i.[inc_id], i.[m_id], i.[inc_detail], i.[inc_description], i.[inc_amount], i.[inc_date], i.[acc_id] " &
                          "FROM [Income] AS i"
                Dim cmd As New OleDbCommand(query, conn)
                Dim adapter As New OleDbDataAdapter(cmd)

                ' ดึงข้อมูลจากตาราง Income
                adapter.Fill(incomeTable)

                ' ดึงข้อมูลจากตาราง Income_Details
                Dim detailsQuery As String = "SELECT d.[inc_id], d.[ind_accname], d.[ind_amount], d.[ind_date] " &
                                 "FROM [Income_Details] AS d"
                Dim detailsCmd As New OleDbCommand(detailsQuery, conn)
                Dim detailsAdapter As New OleDbDataAdapter(detailsCmd)

                ' ดึงข้อมูลจากตาราง Income_Details
                detailsAdapter.Fill(detailsTable)

                ' แสดงข้อมูลใน DataGridView
                dgvResults.DataSource = incomeTable
                dgvResults.Refresh()

                ' กำหนดฟอนต์สำหรับเซลล์ (ขนาด 20) และหัวตาราง (ขนาด 22) โดยใช้ฟอนต์ Fc Minimal
                Dim cellFont As New Font("Fc Minimal", 20)
                Dim headerFont As New Font("Fc Minimal", 22)

                ' ตั้งชื่อหัวตารางใน DataGridView เป็นภาษาไทย
                With dgvResults
                    .Columns("inc_id").HeaderText = "เลขที่รายรับ"
                    .Columns("m_id").HeaderText = "รหัสสมาชิก"
                    .Columns("inc_detail").HeaderText = "รายละเอียดรายรับ"
                    .Columns("inc_description").HeaderText = "คำอธิบายรายรับ"
                    .Columns("inc_amount").HeaderText = "ยอดเงิน"
                    .Columns("inc_date").HeaderText = "วันที่"
                    .Columns("acc_id").HeaderText = "รหัสบัญชี"

                    ' จัดรูปแบบคอลัมน์ยอดเงินให้ชิดขวาและมีเครื่องหมายคอมมา
                    .Columns("inc_amount").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    .Columns("inc_amount").DefaultCellStyle.Format = "N2" ' รูปแบบตัวเลข: มีเครื่องหมายคอมมาและทศนิยม 2 ตำแหน่ง

                    ' กำหนดฟอนต์สำหรับเซลล์และหัวตาราง
                    .DefaultCellStyle.Font = cellFont
                    .ColumnHeadersDefaultCellStyle.Font = headerFont

                    ' ตั้งให้หัวตารางจัดอยู่ตรงกลาง
                    .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                End With

            End Using
        Catch ex As Exception
            ' แสดงข้อความหากเกิดข้อผิดพลาด
            MessageBox.Show("เกิดข้อผิดพลาดในการดึงข้อมูล: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    ' เหตุการณ์เมื่อคลิกเซลล์ใน DataGridView
    Private Sub dgvResults_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvResults.CellClick
        ' ตรวจสอบว่ามีการคลิกในเซลล์ที่มีข้อมูล
        If e.RowIndex >= 0 AndAlso e.ColumnIndex >= 0 Then
            ' ดึงค่า inc_id จากแถวที่คลิก (สามารถคลิกจากเซลล์ใดก็ได้ในแถว)
            selectedIncId = Convert.ToInt32(dgvResults.Rows(e.RowIndex).Cells("inc_id").Value)
        End If
    End Sub


    ' ปรับฟังก์ชัน btnGenerateReport_Click ให้รวมการค้นหา inc_id
    Private Sub btnGenerateReport_Click(sender As Object, e As EventArgs) Handles btnGenerateReport.Click
        ' ดึงค่า inc_id จาก TextBox
        Dim searchIncId As Integer
        If Integer.TryParse(txtSearchIncId.Text, searchIncId) Then
            ' กรองข้อมูลใน DataTable ตาม inc_id ที่ระบุ
            Dim filteredRows As DataRow() = incomeTable.Select("inc_id = " & searchIncId)

            If filteredRows.Length > 0 Then
                ' ถ้าพบข้อมูลให้ทำการเลือกแถวที่ค้นหาได้
                selectedIncId = searchIncId

                ' สร้าง DataTable ใหม่เพื่อเก็บข้อมูลที่เลือก
                Dim selectedTable As New DataTable()
                selectedTable.Columns.Add("inc_id", GetType(Integer))
                selectedTable.Columns.Add("m_id", GetType(Integer))
                selectedTable.Columns.Add("m_name", GetType(String)) ' เพิ่มคอลัมน์ m_name สำหรับชื่อสมาชิก
                selectedTable.Columns.Add("inc_detail", GetType(String))
                selectedTable.Columns.Add("inc_description", GetType(String))
                selectedTable.Columns.Add("ind_accname", GetType(String)) ' เพิ่มคอลัมน์ ind_accname สำหรับชื่อบัญชี
                selectedTable.Columns.Add("ind_amount", GetType(Decimal)) ' เพิ่มคอลัมน์ ind_amount สำหรับยอดเงินแต่ละรายการ
                selectedTable.Columns.Add("inc_date", GetType(String)) ' ใช้ String แทน Date เพื่อแสดงผลรูปแบบที่ต้องการ
                selectedTable.Columns.Add("acc_id", GetType(String))

                ' ตัวแปรสำหรับเก็บยอดรวม
                Dim totalAmount As Decimal = 0

                ' ค้นหาข้อมูลที่ตรงกับ inc_id ที่เลือก
                Dim selectedRow = filteredRows(0)

                ' ค้นหาชื่อสมาชิกจากตาราง Member โดยใช้ m_id
                Dim memberName As String = GetMemberName(Convert.ToInt32(selectedRow("m_id")))

                ' ค้นหารายการที่ตรงกับ inc_id ใน detailsTable
                Dim relatedDetails = detailsTable.Select("inc_id = " & searchIncId)

                ' แปลงวันที่เป็นพ.ศ. และเดือนภาษาไทย
                Dim incDate As Date = Convert.ToDateTime(selectedRow("inc_date"))
                Dim thaiCulture As CultureInfo = New CultureInfo("th-TH") ' กำหนดวัฒนธรรมภาษาไทย
                Dim thaiDate As String = incDate.ToString("dd MMMM yyyy", thaiCulture) ' แสดงเป็นวันที่ เดือน ปี (พ.ศ.)

                ' เพิ่มข้อมูลใน selectedTable ทีละรายการ (แยกรายการแต่ละแถว)
                For Each detailRow As DataRow In relatedDetails
                    Dim amount As Decimal = GetDecimalValue(detailRow("ind_amount")) ' ใช้ฟังก์ชันตรวจสอบค่า
                    totalAmount += amount ' คำนวณยอดรวมเงิน

                    selectedTable.Rows.Add(selectedRow("inc_id"), selectedRow("m_id"), memberName,
                           selectedRow("inc_detail"), selectedRow("inc_description"),
                           detailRow("ind_accname"), amount.ToString("N2"), thaiDate, selectedRow("acc_id"))
                Next


                ' แสดงข้อมูลใน ReportViewer
                ShowReport(selectedTable, totalAmount)
            Else
                ' ถ้าไม่พบข้อมูลแสดงข้อความแจ้งเตือน
                MessageBox.Show("ไม่พบข้อมูลที่มี inc_id = " & searchIncId, "ผลการค้นหา", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            MessageBox.Show("กรุณาป้อน inc_id ที่ถูกต้อง", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
    ' ฟังก์ชันสำหรับตรวจสอบและแปลงค่าเป็น Decimal
    Private Function GetDecimalValue(ByVal value As Object) As Decimal
        If IsDBNull(value) OrElse value Is Nothing Then
            Return 0
        Else
            Return Convert.ToDecimal(value)
        End If
    End Function


    ' ฟังก์ชันสำหรับดึงชื่อสมาชิกจากตาราง Member โดยใช้ m_id
    Private Function GetMemberName(ByVal mId As Integer) As String
        Dim memberName As String = ""

        ' คำสั่ง SQL สำหรับดึงชื่อสมาชิกจากตาราง Member
        Dim query As String = "SELECT [m_name] FROM [Member] WHERE [m_id] = @m_id"
        Using conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
            conn.Open()

            Using cmd As New OleDbCommand(query, conn)
                cmd.Parameters.AddWithValue("@m_id", mId)

                Dim reader As OleDbDataReader = cmd.ExecuteReader()
                If reader.Read() Then
                    ' เก็บชื่อสมาชิก
                    memberName = reader("m_name").ToString()
                End If
            End Using
        End Using

        Return memberName
    End Function

    ' ฟังก์ชันสำหรับแสดงรายงานใน ReportViewer พร้อมกับยอดรวม
    Private Sub ShowReport(ByVal selectedTable As DataTable, ByVal totalAmount As Decimal)
        Me.ReportViewer1.LocalReport.ReportPath = "D:\Project-2022\Banmai\Banmai\Page2\ReceiptReport.rdlc"

        ' กำหนด ReportDataSource โดยใช้ selectedTable
        Dim rds As New ReportDataSource("IncomeDataSet", selectedTable)

        ' ล้าง DataSources ก่อน แล้วเพิ่ม DataSource ใหม่
        Me.ReportViewer1.LocalReport.DataSources.Clear()
        Me.ReportViewer1.LocalReport.DataSources.Add(rds)

        ' ส่งค่าพารามิเตอร์ยอดรวมไปที่ ReportViewer
        Dim reportParameters As New List(Of ReportParameter)
        reportParameters.Add(New ReportParameter("TotalAmount", totalAmount.ToString("N2")))

        ' กำหนดพารามิเตอร์ให้กับรายงาน
        Me.ReportViewer1.LocalReport.SetParameters(reportParameters)

        ' รีเฟรช ReportViewer เพื่อแสดงผล
        Me.ReportViewer1.RefreshReport()
    End Sub


End Class