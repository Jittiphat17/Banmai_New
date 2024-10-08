Imports System.Data.OleDb
Imports System.Globalization

Public Class frmManageMembers
    Dim conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
    Dim cmd As OleDbCommand
    Dim dr As OleDbDataReader
    Dim strSQL As String ' SQL Query String
    Dim isEditing As Boolean = False ' State for add/edit mode

    Private Sub frmManageMembers_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConfigureDataGridView() ' เรียกฟังก์ชันเพื่อกำหนดคอลัมน์ก่อน
        ClearAllData()
        Loadinfo() ' โหลดข้อมูลหลังจากกำหนดคอลัมน์
        Auto_id()
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        ' ตรวจสอบว่าช่องค้นหาว่างหรือไม่
        If String.IsNullOrWhiteSpace(txtSearch.Text) Then
            ' ถ้าช่องค้นหาว่าง ให้โหลดข้อมูลทั้งหมด
            Loadinfo()
        Else
            ' ถ้าช่องค้นหาไม่ว่าง ให้ทำการค้นหา
            SearchMember(txtSearch.Text.Trim())
        End If
    End Sub


    Private Sub SearchMember(searchTerm As String)
        ' Define a query that will search the Member table by name, ID, or phone number
        strSQL = "SELECT * FROM Member WHERE m_name LIKE @search OR m_id LIKE @search OR m_tel LIKE @search"

        Using conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
            Using cmd As New OleDbCommand(strSQL, conn)
                cmd.Parameters.AddWithValue("@search", "%" & searchTerm & "%")

                Try
                    conn.Open()
                    dr = cmd.ExecuteReader
                    dgvMembers.Rows.Clear()

                    If Not dr.HasRows Then
                        MessageBox.Show("ไม่พบข้อมูลที่ค้นหา", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If

                    While dr.Read
                        Dim birthDate As String = DateTime.Parse(dr("m_birth").ToString()).ToString("dd/MM/yyyy")
                        Dim age As Integer = dr("m_age")

                        ' แปลงค่า s_id เป็นสถานะสมาชิก
                        Dim memberStatus As String = If(dr("s_id") = 0, "สมาชิกลาออก", "สมาชิกคงอยู่")

                        dgvMembers.Rows.Add(dr("m_id").ToString, dr("m_gender").ToString, dr("m_name").ToString,
                                        dr("m_nick").ToString, birthDate, age, dr("m_thaiid").ToString, dr("m_job").ToString,
                                        dr("m_address").ToString, dr("m_post").ToString, dr("m_tel").ToString,
                                        dr("m_accountName").ToString, dr("m_accountNum").ToString,
                                        dr("m_beginning").ToString, dr("m_outstanding").ToString, dr("m_national").ToString,
                                        memberStatus)
                    End While
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End Using
    End Sub

    Sub ConfigureDataGridView()
        dgvMembers.Columns.Clear()
        dgvMembers.Columns.Add("m_id", "รหัสสมาชิก")
        dgvMembers.Columns.Add("m_gender", "คำนำหน้า")
        dgvMembers.Columns.Add("m_name", "ชื่อ-นามสกุล")
        dgvMembers.Columns.Add("m_nick", "ชื่อเล่น")
        dgvMembers.Columns.Add("m_birth", "วัน/เดือน/ปีเกิด")
        dgvMembers.Columns.Add("m_age", "อายุ")
        dgvMembers.Columns.Add("m_thaiid", "รหัสประชาชน")
        dgvMembers.Columns.Add("m_job", "อาชีพ")
        dgvMembers.Columns.Add("m_address", "ที่อยู่")
        dgvMembers.Columns.Add("m_post", "รหัสไปรษณีย์")
        dgvMembers.Columns.Add("m_tel", "เบอร์โทรติดต่อ")
        dgvMembers.Columns.Add("m_accountName", "ชื่อบัญชี")
        dgvMembers.Columns.Add("m_accountNum", "เลขบัญชี")
        dgvMembers.Columns.Add("m_national", "สัญชาติ")
        dgvMembers.Columns.Add("s_id", "สถานะสมาชิก")

        ' Set other DataGridView properties
        dgvMembers.DefaultCellStyle.Font = New Font("Tahoma", 10)
        dgvMembers.DefaultCellStyle.BackColor = Color.White
        dgvMembers.DefaultCellStyle.ForeColor = Color.Black
        dgvMembers.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray
        dgvMembers.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 10, FontStyle.Bold)
        dgvMembers.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy
        dgvMembers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgvMembers.EnableHeadersVisualStyles = False
    End Sub


    Sub Loadinfo()
        strSQL = "SELECT * FROM Member"

        Using conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
            Using cmd As New OleDbCommand(strSQL, conn)
                Try
                    conn.Open()
                    dr = cmd.ExecuteReader
                    dgvMembers.Rows.Clear()

                    While dr.Read
                        Dim birthDate As String = DateTime.Parse(dr("m_birth").ToString()).ToString("dd/MM/yyyy")
                        Dim age As Integer = dr("m_age") ' ดึงข้อมูลอายุจากฐานข้อมูล

                        ' แปลงค่า s_id เป็นสถานะสมาชิก
                        Dim memberStatus As String = If(dr("s_id") = 0, "สมาชิกลาออก", "สมาชิกคงอยู่")

                        ' เพิ่มข้อมูลลงใน DataGridView โดยไม่ดึง m_beginning และ m_outstanding
                        dgvMembers.Rows.Add(dr("m_id").ToString, dr("m_gender").ToString, dr("m_name").ToString,
                                        dr("m_nick").ToString, birthDate, age, dr("m_thaiid").ToString, dr("m_job").ToString,
                                        dr("m_address").ToString, dr("m_post").ToString, dr("m_tel").ToString,
                                        dr("m_accountName").ToString, dr("m_accountNum").ToString,
                                        dr("m_national").ToString, memberStatus) ' แสดงสถานะสมาชิกแทนค่า s_id
                    End While
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End Using
    End Sub


    Sub ClearAllData()
        txtID.Clear()
        txtName.Clear()
        txtnick.Clear()
        dtpBirth.Value = DateTime.Now
        txtThaiid.Clear()
        txtJob.Clear()
        txtAddress.Clear()
        txtPost.Clear()
        txtTel.Clear()
        txtAccountname.Clear()
        txtAccountnum.Clear()

        ' Clear and reset the gender ComboBox
        cmbGender.Items.Clear()
        cmbGender.Items.Add("เลือกคำนำหน้า")
        cmbGender.Items.Add("ดช.")
        cmbGender.Items.Add("ดญ.")
        cmbGender.Items.Add("นาย")
        cmbGender.Items.Add("นาง")
        cmbGender.Items.Add("นางสาว")
        cmbGender.SelectedIndex = 0

        ' Clear and reset the nationality ComboBox
        cmbNational.Items.Clear()
        cmbNational.Items.Add("เลือกสัญชาติ")
        cmbNational.Items.Add("ไทย")
        cmbNational.SelectedIndex = 0

        ' Clear and reset the status ComboBox (s_id)
        cmbStatus.Items.Clear()
        cmbStatus.Items.Add("เลือกสถานะ")
        cmbStatus.Items.Add("สมาชิกคงอยู่")
        cmbStatus.Items.Add("สมาชิกลาออก")
        cmbStatus.SelectedIndex = 0 ' Default selection for status

        isEditing = False ' Reset editing state
    End Sub

    Sub Auto_id()
        Dim m_id As Integer
        Try
            strSQL = "SELECT m_id FROM Member ORDER BY m_id DESC"

            Using conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
                Using cmd As New OleDbCommand(strSQL, conn)
                    conn.Open()
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection)
                    If dr.Read = True Then
                        m_id = Val(dr(0)) + 1
                    Else
                        m_id = 1
                    End If
                    txtID.Text = m_id.ToString("0000")
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Not AllFieldsFilled() Then
            MessageBox.Show("โปรดกรอกข้อมูลให้ครบถ้วน", "Incomplete Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            Using conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
                conn.Open()

                If isEditing Then
                    strSQL = "UPDATE Member SET m_gender = @m_gender, m_name = @m_name, m_nick = @m_nick, m_birth = @m_birth, m_national = @m_national, " &
                         "m_thaiid = @m_thaiid, m_job = @m_job, m_address = @m_address, m_post = @m_post, m_tel = @m_tel, " &
                         "m_accountName = @m_accountName, m_accountNum = @m_accountNum, m_age = @m_age, s_id = @s_id WHERE m_id = @m_id"
                Else
                    strSQL = "INSERT INTO Member (m_id, m_gender, m_name, m_nick, m_birth, m_national, m_thaiid, m_job, m_address, m_post, m_tel, " &
                         "m_accountName, m_accountNum, m_age, s_id) VALUES (@m_id, @m_gender, @m_name, @m_nick, @m_birth, @m_national, " &
                         "@m_thaiid, @m_job, @m_address, @m_post, @m_tel, @m_accountName, @m_accountNum, @m_age, @s_id)"
                End If

                Using cmd As New OleDbCommand(strSQL, conn)
                    cmd.Parameters.AddWithValue("@m_id", txtID.Text.Trim())
                    cmd.Parameters.AddWithValue("@m_gender", cmbGender.SelectedItem.ToString())
                    cmd.Parameters.AddWithValue("@m_name", txtName.Text.Trim())
                    cmd.Parameters.AddWithValue("@m_nick", txtnick.Text.Trim())
                    cmd.Parameters.AddWithValue("@m_birth", dtpBirth.Value.ToString("yyyy-MM-dd"))
                    cmd.Parameters.AddWithValue("@m_national", cmbNational.SelectedItem.ToString())
                    cmd.Parameters.AddWithValue("@m_thaiid", txtThaiid.Text.Trim())
                    cmd.Parameters.AddWithValue("@m_job", txtJob.Text.Trim())
                    cmd.Parameters.AddWithValue("@m_address", txtAddress.Text.Trim())
                    cmd.Parameters.AddWithValue("@m_post", txtPost.Text.Trim())
                    cmd.Parameters.AddWithValue("@m_tel", txtTel.Text.Trim())
                    cmd.Parameters.AddWithValue("@m_accountName", txtAccountname.Text.Trim())
                    cmd.Parameters.AddWithValue("@m_accountNum", txtAccountnum.Text.Trim())
                    cmd.Parameters.AddWithValue("@m_age", CalculateAge(dtpBirth.Value.ToString("dd/MM/yyyy")))
                    cmd.Parameters.AddWithValue("@s_id", If(cmbStatus.SelectedItem.ToString() = "สมาชิกคงอยู่", 1, 0))

                    Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                    If rowsAffected > 0 Then
                        MessageBox.Show("บันทึกข้อมูลสำเร็จ", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Loadinfo()
                        If Not isEditing Then
                            ClearAllData()
                            Auto_id()
                        End If
                    Else
                        MessageBox.Show("ไม่มีการเปลี่ยนแปลงข้อมูล", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาด: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Function AllFieldsFilled() As Boolean

        ' ตรวจสอบว่าคำนำหน้าได้รับการเลือกแล้ว
        If cmbGender.SelectedIndex = 0 Then
            MessageBox.Show("โปรดเลือกคำนำหน้า", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        ' ตรวจสอบว่าฟิลด์สัญชาติได้รับการเลือกแล้ว
        If cmbNational.SelectedIndex = 0 Then
            MessageBox.Show("โปรดเลือกสัญชาติ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        ' ตรวจสอบว่าฟิลด์สถานะสมาชิกได้รับการเลือกแล้ว
        If cmbStatus.SelectedIndex = 0 Then
            MessageBox.Show("โปรดเลือกสถานะสมาชิก", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        ' หากข้อมูลทั้งหมดถูกต้อง
        Return True
    End Function

    Private Sub dgvMembers_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvMembers.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = dgvMembers.Rows(e.RowIndex)

            ' ตรวจสอบค่าที่ดึงจาก DataGridView
            MessageBox.Show("ID ที่เลือก: " & row.Cells("m_id").Value.ToString())

            ' Populate your textboxes with the selected row data
            txtID.Text = row.Cells("m_id").Value.ToString()
            cmbGender.SelectedItem = row.Cells("m_gender").Value.ToString()
            txtName.Text = row.Cells("m_name").Value.ToString()
            txtnick.Text = row.Cells("m_nick").Value.ToString()

            ' ตรวจสอบรูปแบบวัน/เดือน/ปีเกิด
            If row.Cells("m_birth").Value IsNot Nothing Then
                dtpBirth.Value = DateTime.ParseExact(row.Cells("m_birth").Value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture)
            Else
                dtpBirth.Value = DateTime.Now
            End If

            ' Continue filling other fields
            txtThaiid.Text = row.Cells("m_thaiid").Value.ToString()
            txtJob.Text = row.Cells("m_job").Value.ToString()
            txtAddress.Text = row.Cells("m_address").Value.ToString()
            txtPost.Text = row.Cells("m_post").Value.ToString()
            txtTel.Text = row.Cells("m_tel").Value.ToString()
            txtAccountname.Text = row.Cells("m_accountName").Value.ToString()
            txtAccountnum.Text = row.Cells("m_accountNum").Value.ToString()
            cmbNational.SelectedItem = row.Cells("m_national").Value.ToString()

            ' แปลงค่า s_id เป็นสถานะสมาชิก
            cmbStatus.SelectedItem = If(row.Cells("s_id").Value.ToString() = "0", "สมาชิกลาออก", "สมาชิกคงอยู่")

            ' ตั้งค่าสถานะว่าเป็นการแก้ไข
            isEditing = True
        End If
    End Sub


    Private Function CalculateAge(birthDate As String) As Integer
        Dim birthDateObj As Date
        ' ตรวจสอบการแปลงวันที่โดยเพิ่มรูปแบบหลายแบบ
        Dim formats() As String = {"dd/MM/yyyy", "MM/dd/yyyy", "yyyy-MM-dd", "d/M/yyyy"}

        If DateTime.TryParseExact(birthDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, birthDateObj) Then
            Dim age As Integer = DateTime.Now.Year - birthDateObj.Year
            If DateTime.Now < birthDateObj.AddYears(age) Then age -= 1
            Return age
        Else
            MessageBox.Show("รูปแบบวันเกิดไม่ถูกต้อง", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return 0
        End If
    End Function

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearAllData()
        Auto_id() ' Set new ID for new entry
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If String.IsNullOrEmpty(txtID.Text) Then
            MessageBox.Show("Please select a member to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this member?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If result = DialogResult.Yes Then
            Try
                Using conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
                    conn.Open()

                    Dim deleteQuery As String = "DELETE FROM Member WHERE m_id = @m_id"
                    Using cmdDelete As New OleDbCommand(deleteQuery, conn)
                        cmdDelete.Parameters.AddWithValue("@m_id", txtID.Text)
                        cmdDelete.ExecuteNonQuery()
                    End Using
                End Using

                MessageBox.Show("Member deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ClearAllData()
                Loadinfo()
            Catch ex As Exception
                MessageBox.Show("Error deleting member: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If String.IsNullOrWhiteSpace(txtID.Text) Then
            MessageBox.Show("กรุณาเลือกสมาชิกก่อนทำการอัปเดต", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If Not AllFieldsFilled() Then
            MessageBox.Show("โปรดกรอกข้อมูลให้ครบถ้วน", "Incomplete Data", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Try
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            ' ค้นหา s_id จากตาราง Memberstatus
            Dim s_id As Integer = -1
            Dim statusQuery As String = "SELECT s_id FROM Memberstatus WHERE s_namestatus = @status"
            Using statusCmd As New OleDbCommand(statusQuery, conn)
                statusCmd.Parameters.AddWithValue("@status", cmbStatus.SelectedItem.ToString())
                Dim reader As OleDbDataReader = statusCmd.ExecuteReader()

                If reader.Read() Then
                    s_id = Convert.ToInt32(reader("s_id"))
                    MessageBox.Show("s_id ที่ดึงได้: " & s_id.ToString())  ' แสดง s_id ที่ถูกดึงมา
                End If
                reader.Close()
            End Using

            If s_id = -1 Then
                MessageBox.Show("ไม่พบสถานะที่เลือก", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            ' คำสั่ง SQL สำหรับอัปเดตข้อมูลสมาชิก
            strSQL = "UPDATE Member SET m_gender = @m_gender, m_name = @m_name, m_nick = @m_nick, m_birth = @m_birth, " &
                 "m_national = @m_national, m_thaiid = @m_thaiid, m_job = @m_job, m_address = @m_address, " &
                 "m_post = @m_post, m_tel = @m_tel, m_accountName = @m_accountName, m_accountNum = @m_accountNum, " &
                 "m_age = @m_age, s_id = @s_id WHERE m_id = @m_id"

            Using cmd As New OleDbCommand(strSQL, conn)
                cmd.Parameters.AddWithValue("@m_gender", cmbGender.SelectedItem.ToString())
                cmd.Parameters.AddWithValue("@m_name", txtName.Text.Trim())
                cmd.Parameters.AddWithValue("@m_nick", txtnick.Text.Trim())
                cmd.Parameters.AddWithValue("@m_birth", dtpBirth.Value.ToString("yyyy-MM-dd"))
                cmd.Parameters.AddWithValue("@m_national", cmbNational.SelectedItem.ToString())
                cmd.Parameters.AddWithValue("@m_thaiid", txtThaiid.Text.Trim())
                cmd.Parameters.AddWithValue("@m_job", txtJob.Text.Trim())
                cmd.Parameters.AddWithValue("@m_address", txtAddress.Text.Trim())
                cmd.Parameters.AddWithValue("@m_post", txtPost.Text.Trim())
                cmd.Parameters.AddWithValue("@m_tel", txtTel.Text.Trim())
                cmd.Parameters.AddWithValue("@m_accountName", txtAccountname.Text.Trim())
                cmd.Parameters.AddWithValue("@m_accountNum", txtAccountnum.Text.Trim())
                cmd.Parameters.AddWithValue("@m_age", CalculateAge(dtpBirth.Value.ToString("dd/MM/yyyy")))
                cmd.Parameters.AddWithValue("@s_id", s_id)
                cmd.Parameters.AddWithValue("@m_id", txtID.Text.Trim())

                Dim rowsAffected As Integer = cmd.ExecuteNonQuery()

                If rowsAffected > 0 Then
                    MessageBox.Show("อัปเดตข้อมูลสำเร็จ", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Loadinfo()
                    ClearAllData()
                    Auto_id()
                Else
                    MessageBox.Show("ไม่มีการเปลี่ยนแปลงข้อมูล", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาด: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub



    Private Function IsLettersOnly(input As String) As Boolean
        ' วนลูปผ่านทุกตัวอักษรในสตริงเพื่อตรวจสอบว่าทุกตัวเป็นตัวอักษร
        For Each c As Char In input
            If Not Char.IsLetter(c) And Not Char.IsWhiteSpace(c) Then
                Return False ' ถ้ามีตัวใดตัวหนึ่งไม่ใช่ตัวอักษร ให้คืนค่าเป็น False
            End If
        Next
        Return True ' ถ้าเป็นตัวอักษรทั้งหมด ให้คืนค่าเป็น True
    End Function

    Private Sub txtName_TextChanged(sender As Object, e As EventArgs) Handles txtName.TextChanged
        Dim currentText As String = txtName.Text
        Dim filteredText As String = ""

        ' วนลูปตรวจสอบแต่ละตัวอักษรใน TextBox
        For Each c As Char In currentText
            ' ตรวจสอบว่าเป็นตัวอักษร (รวมทั้งภาษาไทย) หรือช่องว่างเท่านั้น
            If Char.IsLetter(c) Or Char.IsWhiteSpace(c) Or Char.GetUnicodeCategory(c) = Globalization.UnicodeCategory.NonSpacingMark Then
                filteredText &= c ' ถ้าเป็นตัวอักษรหรือช่องว่างให้นำมาใช้ได้
            End If
        Next

        ' กำหนดค่าใหม่ให้กับ TextBox โดยกรองตัวเลขออกไป
        If filteredText <> currentText Then
            txtName.Text = filteredText
            txtName.SelectionStart = txtName.Text.Length ' เลื่อนเคอร์เซอร์ไปที่ตำแหน่งท้ายสุดของข้อความ
        End If
    End Sub

    Private Sub txtJob_TextChanged(sender As Object, e As EventArgs) Handles txtJob.TextChanged
        Dim currentText As String = txtJob.Text
        Dim filteredText As String = ""

        ' วนลูปตรวจสอบแต่ละตัวอักษรใน TextBox
        For Each c As Char In currentText
            ' ตรวจสอบว่าเป็นตัวอักษร (รวมทั้งภาษาไทย) หรือช่องว่างเท่านั้น
            If Char.IsLetter(c) Or Char.IsWhiteSpace(c) Or Char.GetUnicodeCategory(c) = Globalization.UnicodeCategory.NonSpacingMark Then
                filteredText &= c ' ถ้าเป็นตัวอักษรหรือช่องว่างให้นำมาใช้ได้
            End If
        Next

        ' กำหนดค่าใหม่ให้กับ TextBox โดยกรองตัวเลขออกไป
        If filteredText <> currentText Then
            txtJob.Text = filteredText
            txtJob.SelectionStart = txtJob.Text.Length ' เลื่อนเคอร์เซอร์ไปที่ตำแหน่งท้ายสุดของข้อความ
        End If
    End Sub

    Private Sub txtAccountname_TextChanged(sender As Object, e As EventArgs) Handles txtAccountname.TextChanged
        Dim currentText As String = txtAccountname.Text
        Dim filteredText As String = ""

        ' วนลูปตรวจสอบแต่ละตัวอักษรใน TextBox
        For Each c As Char In currentText
            ' ตรวจสอบว่าเป็นตัวอักษร (รวมทั้งภาษาไทย) หรือช่องว่างเท่านั้น
            If Char.IsLetter(c) Or Char.IsWhiteSpace(c) Or Char.GetUnicodeCategory(c) = Globalization.UnicodeCategory.NonSpacingMark Then
                filteredText &= c ' ถ้าเป็นตัวอักษรหรือช่องว่างให้นำมาใช้ได้
            End If
        Next

        ' กำหนดค่าใหม่ให้กับ TextBox โดยกรองตัวเลขออกไป
        If filteredText <> currentText Then
            txtAccountname.Text = filteredText
            txtAccountname.SelectionStart = txtAccountname.Text.Length ' เลื่อนเคอร์เซอร์ไปที่ตำแหน่งท้ายสุดของข้อความ
        End If
    End Sub

    Private Sub txtnick_TextChanged(sender As Object, e As EventArgs) Handles txtnick.TextChanged
        Dim currentText As String = txtnick.Text
        Dim filteredText As String = ""

        ' วนลูปตรวจสอบแต่ละตัวอักษรใน TextBox
        For Each c As Char In currentText
            ' ตรวจสอบว่าเป็นตัวอักษร (รวมทั้งภาษาไทย) หรือช่องว่างเท่านั้น
            If Char.IsLetter(c) Or Char.IsWhiteSpace(c) Or Char.GetUnicodeCategory(c) = Globalization.UnicodeCategory.NonSpacingMark Then
                filteredText &= c ' ถ้าเป็นตัวอักษรหรือช่องว่างให้นำมาใช้ได้
            End If
        Next

        ' กำหนดค่าใหม่ให้กับ TextBox โดยกรองตัวเลขออกไป
        If filteredText <> currentText Then
            txtnick.Text = filteredText
            txtnick.SelectionStart = txtnick.Text.Length ' เลื่อนเคอร์เซอร์ไปที่ตำแหน่งท้ายสุดของข้อความ
        End If
    End Sub


End Class