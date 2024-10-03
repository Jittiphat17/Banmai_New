Imports System.Data.OleDb
Imports System.Windows.Controls
Imports Guna.UI2.WinForms

Public Class frmEditIncome
    Dim conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
    Private incomeID As String = "" ' เก็บค่า inc_id ของรายรับที่ถูกเลือก

    Private Sub frmEditIncome_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadAccounts() ' โหลดข้อมูลบัญชี
        LoadIncome() ' โหลดรายการรายรับ
        ConfigureDataGridView() ' ตั้งค่าการแสดงผลของ DataGridView
        ConfigureDetailsDataGridView() ' ตั้งค่าการแสดงผลของ DataGridView สำหรับรายละเอียดรายรับ
    End Sub

    ' โหลดบัญชีลงใน ComboBox
    Private Sub LoadAccounts()
        Try
            If conn.State = ConnectionState.Closed Then conn.Open()
            Dim query As String = "SELECT acc_id, acc_name FROM Account"
            Dim adapter As New OleDbDataAdapter(query, conn)
            Dim table As New DataTable()
            adapter.Fill(table)

            If table.Rows.Count > 0 Then
                cmbAccount.DataSource = table
                cmbAccount.DisplayMember = "acc_name" ' แสดงชื่อบัญชี
                cmbAccount.ValueMember = "acc_id" ' ใช้ acc_id เป็นค่า
                cmbAccount.SelectedIndex = -1 ' ไม่มีการเลือกค่าใด ๆ เริ่มต้น
            Else
                MessageBox.Show("ไม่พบข้อมูลบัญชีในตาราง Account")
            End If
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการโหลดบัญชี: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub

    ' ตั้งค่าการแสดงผล DataGridView สำหรับรายรับ
    Private Sub ConfigureDataGridView()
        dgvIncome.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.Dark
        dgvIncome.DefaultCellStyle.Font = New Font("Fc Minimal", 12)
        dgvIncome.DefaultCellStyle.BackColor = Color.White
        dgvIncome.DefaultCellStyle.ForeColor = Color.Black
        dgvIncome.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray
        dgvIncome.ColumnHeadersDefaultCellStyle.Font = New Font("Fc Minimal", 10, FontStyle.Bold)
        dgvIncome.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy
        dgvIncome.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvIncome.EnableHeadersVisualStyles = False
    End Sub

    ' ตั้งค่าการแสดงผล DataGridView สำหรับรายละเอียดรายรับ
    Private Sub ConfigureDetailsDataGridView()
        dgvIncomeDetails.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.Dark
        dgvIncomeDetails.DefaultCellStyle.Font = New Font("Fc Minimal", 12)
        dgvIncomeDetails.DefaultCellStyle.BackColor = Color.White
        dgvIncomeDetails.DefaultCellStyle.ForeColor = Color.Black
        dgvIncomeDetails.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray
        dgvIncomeDetails.ColumnHeadersDefaultCellStyle.Font = New Font("Fc Minimal", 10, FontStyle.Bold)
        dgvIncomeDetails.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy
        dgvIncomeDetails.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvIncomeDetails.EnableHeadersVisualStyles = False
    End Sub

    ' โหลดข้อมูลรายรับลงใน DataGridView
    Private Sub LoadIncome()
        Try
            If conn.State = ConnectionState.Closed Then conn.Open()

            ' ทำการ Join ตาราง Income กับ Account เพื่อดึงชื่อบัญชีและรหัสบัญชี
            Dim query As String = "SELECT i.inc_id, i.m_id, i.inc_detail, i.inc_description, i.inc_date, i.inc_amount, a.acc_id, a.acc_name " &
                              "FROM Income i " &
                              "INNER JOIN Account a ON i.acc_id = a.acc_id"

            Dim adapter As New OleDbDataAdapter(query, conn)
            Dim table As New DataTable()
            adapter.Fill(table)

            dgvIncome.DataSource = table

            ' เปลี่ยนหัวข้อคอลัมน์เป็นภาษาไทย
            dgvIncome.Columns("inc_id").HeaderText = "รหัสรายรับ"
            dgvIncome.Columns("m_id").HeaderText = "รหัสสมาชิก"
            dgvIncome.Columns("inc_detail").HeaderText = "รายละเอียดรายรับ"
            dgvIncome.Columns("inc_description").HeaderText = "คำอธิบาย"
            dgvIncome.Columns("inc_date").HeaderText = "วันที่"
            dgvIncome.Columns("inc_amount").HeaderText = "จำนวนเงิน"
            dgvIncome.Columns("acc_name").HeaderText = "ชื่อบัญชี" ' แสดงชื่อบัญชีแทนรหัสบัญชี
            dgvIncome.Columns("acc_id").HeaderText = "รหัสบัญชี" ' เพิ่มการแสดงรหัสบัญชีด้วย
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการโหลดข้อมูลรายรับ: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub


    ' โหลดข้อมูลรายละเอียดรายรับลงใน DataGridView
    Private Sub LoadIncomeDetails()
        Try
            If conn.State = ConnectionState.Closed Then conn.Open()

            ' ทำการ Join ตาราง Income_Details กับ Account เพื่อดึงชื่อบัญชีและรหัสบัญชี
            Dim query As String = "SELECT d.ind_id, d.ind_accname, d.con_id, d.ind_amount, d.ind_date, d.inc_id, d.m_id, a.acc_id, a.acc_name " &
                              "FROM Income_Details d " &
                              "INNER JOIN Account a ON d.acc_id = a.acc_id " &
                              "WHERE d.inc_id = @inc_id"

            Dim cmd As New OleDbCommand(query, conn)
            cmd.Parameters.AddWithValue("@inc_id", Convert.ToInt32(incomeID))
            Dim adapter As New OleDbDataAdapter(cmd)
            Dim table As New DataTable()
            adapter.Fill(table)

            dgvIncomeDetails.DataSource = table

            ' เปลี่ยนหัวข้อคอลัมน์เป็นภาษาไทย
            dgvIncomeDetails.Columns("ind_id").HeaderText = "รหัสรายละเอียด"
            dgvIncomeDetails.Columns("ind_accname").HeaderText = "ชื่อบัญชี"
            dgvIncomeDetails.Columns("con_id").HeaderText = "รหัสสัญญา"
            dgvIncomeDetails.Columns("ind_amount").HeaderText = "จำนวนเงิน"
            dgvIncomeDetails.Columns("ind_date").HeaderText = "วันที่"
            dgvIncomeDetails.Columns("acc_name").HeaderText = "ชื่อบัญชี" ' แสดงชื่อบัญชีแทนรหัสบัญชี
            dgvIncomeDetails.Columns("acc_id").HeaderText = "รหัสบัญชี" ' เพิ่มการแสดงรหัสบัญชีด้วย
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการโหลดรายละเอียดรายรับ: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub


    ' เมื่อคลิกเลือกข้อมูลใน DataGridView รายรับ
    Private Sub dgvIncome_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvIncome.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvIncome.Rows(e.RowIndex)
            Try
                txtIncomeID.Text = row.Cells("inc_id").Value?.ToString()
                txtIncomeName.Text = row.Cells("inc_description").Value?.ToString()
                txtIncomeAmount.Text = row.Cells("inc_amount").Value?.ToString()

                ' แปลงวันที่
                If DateTime.TryParse(row.Cells("inc_date").Value?.ToString(), Nothing) Then
                    dtpIncomeDate.Value = Convert.ToDateTime(row.Cells("inc_date").Value)
                End If

                ' ตั้งค่า ComboBox สำหรับบัญชี
                If row.Cells("acc_id").Value IsNot Nothing Then
                    cmbAccount.SelectedValue = row.Cells("acc_id").Value.ToString() ' ตรวจสอบค่า acc_id
                End If

                ' เก็บค่า incomeID และโหลดรายละเอียดรายรับ
                incomeID = row.Cells("inc_id").Value?.ToString()
                LoadIncomeDetails() ' โหลดรายละเอียดรายรับตาม inc_id ที่ถูกเลือก
            Catch ex As Exception
                MessageBox.Show("เกิดข้อผิดพลาดในการโหลดข้อมูล: " & ex.Message)
            End Try
        End If
    End Sub

    ' เมื่อคลิกเลือกข้อมูลใน DataGridView รายละเอียดรายรับ
    Private Sub dgvIncomeDetails_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvIncomeDetails.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvIncomeDetails.Rows(e.RowIndex)
            txtDetailID.Text = row.Cells("ind_id").Value.ToString()
            txtDetailName.Text = row.Cells("ind_accname").Value.ToString()
            txtDetailAmount.Text = row.Cells("ind_amount").Value.ToString()
        End If
    End Sub

    ' ฟังก์ชันบันทึกข้อมูลรายรับและรายละเอียดรายรับ
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Try
            If conn.State = ConnectionState.Closed Then conn.Open()

            ' บันทึกการแก้ไขข้อมูลรายรับ
            Dim query As String = "UPDATE Income SET inc_description = @description, inc_date = @date, inc_amount = @amount, acc_id = @acc_id WHERE inc_id = @inc_id"
            Dim cmd As New OleDbCommand(query, conn)
            cmd.Parameters.AddWithValue("@description", txtIncomeName.Text)
            cmd.Parameters.AddWithValue("@date", dtpIncomeDate.Value)
            cmd.Parameters.AddWithValue("@amount", Convert.ToDouble(txtIncomeAmount.Text))
            cmd.Parameters.AddWithValue("@acc_id", Convert.ToString(cmbAccount.SelectedValue))
            cmd.Parameters.AddWithValue("@inc_id", Convert.ToInt32(txtIncomeID.Text))
            cmd.ExecuteNonQuery()

            ' บันทึกการแก้ไขข้อมูลรายละเอียดรายรับ
            UpdateIncomeDetails()

            MessageBox.Show("แก้ไขข้อมูลสำเร็จ")
            LoadIncome()
            LoadIncomeDetails()

        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการบันทึกข้อมูล: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub

    ' ฟังก์ชันบันทึกการแก้ไขรายละเอียดรายรับ
    Private Sub UpdateIncomeDetails()
        Try
            For Each row As DataGridViewRow In dgvIncomeDetails.Rows
                If Not row.IsNewRow Then
                    Dim query As String = "UPDATE Income_Details SET ind_accname = @accname, ind_amount = @amount WHERE ind_id = @ind_id"
                    Dim cmd As New OleDbCommand(query, conn)
                    cmd.Parameters.AddWithValue("@accname", row.Cells("ind_accname").Value)
                    cmd.Parameters.AddWithValue("@amount", Convert.ToDouble(row.Cells("ind_amount").Value))
                    cmd.Parameters.AddWithValue("@ind_id", Convert.ToInt32(row.Cells("ind_id").Value))
                    cmd.ExecuteNonQuery()
                End If
            Next
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการอัปเดตรายละเอียดรายรับ: " & ex.Message)
        End Try
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        ' ตรวจสอบว่ามีการเลือกข้อมูลใน DataGridView หรือไม่
        If dgvIncome.SelectedRows.Count > 0 Then
            Dim result As DialogResult = MessageBox.Show("คุณต้องการลบข้อมูลรายรับนี้รวมถึงรายละเอียดรายรับด้วยหรือไม่?", "ยืนยันการลบ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If result = DialogResult.Yes Then
                Try
                    ' ดึงค่า inc_id ของรายการที่ถูกเลือก
                    Dim selectedRow As DataGridViewRow = dgvIncome.SelectedRows(0)
                    Dim incomeID As String = selectedRow.Cells("inc_id").Value.ToString()

                    ' เปิดการเชื่อมต่อฐานข้อมูล
                    If conn.State = ConnectionState.Closed Then conn.Open()

                    ' ลบข้อมูลจากตาราง Income_Details ที่เกี่ยวข้องกับ inc_id นี้ก่อน
                    Dim deleteDetailsQuery As String = "DELETE FROM Income_Details WHERE inc_id = @inc_id"
                    Dim cmdDeleteDetails As New OleDbCommand(deleteDetailsQuery, conn)
                    cmdDeleteDetails.Parameters.AddWithValue("@inc_id", incomeID)
                    cmdDeleteDetails.ExecuteNonQuery()

                    ' ลบข้อมูลจากตาราง Income หลังจากลบใน Income_Details แล้ว
                    Dim deleteIncomeQuery As String = "DELETE FROM Income WHERE inc_id = @inc_id"
                    Dim cmdDeleteIncome As New OleDbCommand(deleteIncomeQuery, conn)
                    cmdDeleteIncome.Parameters.AddWithValue("@inc_id", incomeID)
                    cmdDeleteIncome.ExecuteNonQuery()

                    ' ลบข้อมูลออกจาก DataGridView
                    dgvIncome.Rows.Remove(selectedRow)

                    MessageBox.Show("ลบข้อมูลรายรับและรายละเอียดรายรับสำเร็จ", "ลบข้อมูล", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Catch ex As Exception
                    MessageBox.Show("เกิดข้อผิดพลาดในการลบข้อมูล: " & ex.Message)
                Finally
                    ' ปิดการเชื่อมต่อฐานข้อมูล
                    conn.Close()
                End Try
            End If
        Else
            MessageBox.Show("กรุณาเลือกข้อมูลที่ต้องการลบ")
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        ' เคลียร์ TextBox
        txtIncomeID.Text = ""  ' เคลียร์รหัสรายรับ
        txtIncomeName.Text = ""  ' เคลียร์ชื่อหรือคำอธิบายรายรับ
        txtIncomeAmount.Text = ""  ' เคลียร์จำนวนเงิน

        ' ตั้งค่า DateTimePicker ให้เป็นวันที่ปัจจุบันหรือค่าเริ่มต้น
        dtpIncomeDate.Value = DateTime.Now

        ' ตั้งค่า ComboBox ให้กลับไปเป็นค่าเริ่มต้น (ไม่เลือก)
        cmbAccount.SelectedIndex = -1

        ' เคลียร์ TextBox สำหรับรายละเอียดรายรับ (ถ้ามี)
        txtDetailID.Text = ""  ' เคลียร์รหัสรายละเอียด
        txtDetailName.Text = ""  ' เคลียร์ชื่อบัญชีในรายละเอียดรายรับ
        txtDetailAmount.Text = ""  ' เคลียร์จำนวนเงินในรายละเอียดรายรับ

        ' หากมีการตั้งค่าอื่น ๆ เพิ่มเติมในฟอร์มก็ให้เคลียร์ข้อมูลในส่วนเหล่านั้นด้วย
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Try
            If conn.State = ConnectionState.Closed Then conn.Open()

            ' รับค่าจาก TextBox ที่ผู้ใช้กรอก
            Dim searchText As String = txtSearch.Text.Trim()

            ' คำสั่ง SQL ที่มีเงื่อนไขการค้นหาในตาราง Income
            Dim query As String = "SELECT i.inc_id, i.m_id, i.inc_detail, i.inc_description, i.inc_date, i.inc_amount, a.acc_id, a.acc_name " &
                                  "FROM Income i " &
                                  "INNER JOIN Account a ON i.acc_id = a.acc_id " &
                                  "WHERE i.inc_description LIKE @searchText OR i.inc_detail LIKE @searchText OR a.acc_name LIKE @searchText"

            ' สร้างคำสั่ง OleDbCommand
            Dim cmd As New OleDbCommand(query, conn)
            cmd.Parameters.AddWithValue("@searchText", "%" & searchText & "%") ' ใช้ LIKE เพื่อค้นหาข้อมูลที่เกี่ยวข้องกับคำที่กรอก

            ' ดึงข้อมูลและแสดงใน DataGridView
            Dim adapter As New OleDbDataAdapter(cmd)
            Dim table As New DataTable()
            adapter.Fill(table)
            dgvIncome.DataSource = table

        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการค้นหา: " & ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub


End Class
