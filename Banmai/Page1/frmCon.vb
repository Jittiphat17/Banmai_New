Imports System.Data.OleDb

Public Class frmCon
    ' เพิ่ม Property สำหรับเก็บค่าเลขที่สัญญาที่ถูกส่งมาจาก frmIncome
    Public Property SelectedContractId As String

    ' เพิ่มตัวเชื่อมต่อฐานข้อมูล
    Private Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
    ' Dictionary สำหรับเก็บการเปลี่ยนแปลงสถานะการชำระ
    Private statusChanges As New Dictionary(Of Integer, Integer)

    ' เมื่อฟอร์มโหลด
    Private Sub frmCon_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Not String.IsNullOrEmpty(SelectedContractId) Then
            SearchContract(SelectedContractId)
            SearchPayments(SelectedContractId)
        End If

        txtContractNumber.AutoSize = True
    End Sub

    ' เมื่อคลิกปุ่มค้นหา
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        If String.IsNullOrWhiteSpace(txtSearchContractNumber.Text) Then
            MessageBox.Show("กรุณากรอกเลขที่สัญญา", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        SearchContract(txtSearchContractNumber.Text.Trim())
        SearchPayments(txtSearchContractNumber.Text.Trim())
    End Sub

    ' ฟังก์ชันสำหรับค้นหาข้อมูลสัญญา
    Private Sub SearchContract(contractNumber As String)
        Try
            Dim strSQL As String = "SELECT c.*, m.m_name FROM Contract c INNER JOIN Member m ON c.m_id = m.m_id WHERE c.con_id = @contractNumber"
            Dim cmd As New OleDbCommand(strSQL, Conn)
            cmd.Parameters.AddWithValue("@contractNumber", contractNumber)

            If Conn.State = ConnectionState.Open Then Conn.Close()
            Conn.Open()

            Dim reader As OleDbDataReader = cmd.ExecuteReader()
            If reader IsNot Nothing AndAlso reader.Read() Then
                txtContractNumber.Text = reader("con_id").ToString()
                txtBorrowerName.Text = reader("m_name").ToString()
                txtDetails.Text = reader("con_details").ToString()
                txtAmount.Text = Decimal.Parse(reader("con_amount").ToString()).ToString("N2")
                txtMonths.Text = reader("con_permonth").ToString()
                txtTransactionDate.Text = DateTime.Parse(reader("con_date").ToString()).ToString("dd/MM/yyyy")
            Else
                MessageBox.Show("ไม่พบเลขที่สัญญานี้", "ไม่พบข้อมูล", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            reader?.Close()
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาด: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Conn.Close()
        End Try
    End Sub

    Private Sub SearchPayments(contractNumber As String)
        Try
            ' ตรวจสอบการเชื่อมต่อฐานข้อมูล
            If Conn Is Nothing Then
                MessageBox.Show("Database connection is not initialized.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            ' SQL query to retrieve payment data
            Dim strSQL As String = "SELECT p.payment_id, p.con_id, p.payment_date, p.payment_amount, p.payment_prin AS Principal, p.payment_interest AS Interest, p.payment_period, s.status_name " &
       "FROM Payment p " &
       "INNER JOIN Status s ON p.status_id = s.status_id " &
       "WHERE p.con_id = @contractNumber"

            Dim cmd As New OleDbCommand(strSQL, Conn)
            cmd.Parameters.AddWithValue("@contractNumber", contractNumber)

            ' เปิดการเชื่อมต่อ
            If Conn.State = ConnectionState.Closed Then Conn.Open()

            ' Create DataTable to hold the payment data
            Dim dt As New DataTable()
            Dim adapter As New OleDbDataAdapter(cmd)
            adapter.Fill(dt)

            ' แสดงผลใน DataGridView
            dgvPayments.DataSource = dt

            ' ตรวจสอบว่ามีแถวใน DataGridView ก่อนการจัดการคอลัมน์
            If dt.Rows.Count = 0 Then
                MessageBox.Show("No payment records found for the provided contract number.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            ' Set header text for columns
            dgvPayments.Columns("payment_id").HeaderText = "รหัสการชำระ"
            dgvPayments.Columns("con_id").HeaderText = "รหัสสัญญา"
            dgvPayments.Columns("payment_date").HeaderText = "วันที่ต้องชำระ"
            dgvPayments.Columns("payment_amount").HeaderText = "จำนวนเงิน"
            dgvPayments.Columns("Principal").HeaderText = "เงินต้น"
            dgvPayments.Columns("Interest").HeaderText = "ดอกเบี้ย (บาท)"
            dgvPayments.Columns("payment_period").HeaderText = "งวด"
            dgvPayments.Columns("status_name").HeaderText = "สถานะการชำระ"

            ' Format to show two decimal places and align text to the right
            dgvPayments.Columns("payment_amount").DefaultCellStyle.Format = "N2"
            dgvPayments.Columns("Principal").DefaultCellStyle.Format = "N2"
            dgvPayments.Columns("Interest").DefaultCellStyle.Format = "N2"

            ' Align the amount columns to the right
            dgvPayments.Columns("payment_amount").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvPayments.Columns("Principal").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgvPayments.Columns("Interest").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            ' Set the font for DataGridView cells to FC Mini Mal
            Dim fcMiniMalFont As New Font("FC Minimal", 16) ' Adjust font size as needed
            dgvPayments.DefaultCellStyle.Font = fcMiniMalFont
            dgvPayments.ColumnHeadersDefaultCellStyle.Font = fcMiniMalFont

            ' คำนวณยอดเงินต้นคงเหลือทั้งหมด
            Dim totalPrincipalBalance As Decimal = 0
            Dim totalInterestBalance As Decimal = 0
            Dim oneMonthInterest As Decimal = 0

            For Each row As DataRow In dt.Rows
                Dim principal As Decimal = Convert.ToDecimal(row("Principal"))
                Dim interest As Decimal = Convert.ToDecimal(row("Interest"))

                totalPrincipalBalance += principal
                totalInterestBalance += interest

                ' บันทึกดอกเบี้ยสำหรับ 1 เดือน (จากแถวแรกที่เจอเท่านั้น)
                If oneMonthInterest = 0 Then
                    oneMonthInterest = interest
                End If
            Next

            ' บวกดอกเบี้ยแค่ 1 เดือนให้กับยอดเงินต้น
            Dim totalWithOneMonthInterest As Decimal = totalPrincipalBalance + oneMonthInterest

            ' แสดงยอดเงินต้นรวมกับดอกเบี้ย 1 เดือนใน TextBox
            Guna2TextBox1.Text = totalWithOneMonthInterest.ToString("N2")

            ' แสดงยอดรวมของเงินต้น + ดอกเบี้ยทั้งหมด
            txtTotalBalance.Text = (totalPrincipalBalance + totalInterestBalance).ToString("N2")

        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาด: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' ปิดการเชื่อมต่อ
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub




    ' ฟังก์ชันสำหรับดึง status_id จาก status_name
    Private Function GetStatusId(statusName As String) As Integer
        Dim statusId As Integer = -1
        Try
            Dim strSQL As String = "SELECT status_id FROM Status WHERE status_name = @status_name"
            Dim cmd As New OleDbCommand(strSQL, Conn)
            cmd.Parameters.AddWithValue("@status_name", statusName)

            If Conn.State = ConnectionState.Open Then Conn.Close()
            Conn.Open()

            Dim reader As OleDbDataReader = cmd.ExecuteReader()
            If reader IsNot Nothing AndAlso reader.Read() Then
                statusId = Convert.ToInt32(reader("status_id"))
            End If
            reader?.Close()
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาด: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Conn.Close()
        End Try
        Return statusId
    End Function


End Class
