﻿Imports System.Data.OleDb
Imports System.Windows.Controls

Public Class frmManageUser
    ' ประกาศ Database connection string เพียงครั้งเดียวที่ระดับคลาส
    Private Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dt As DataTable
    Dim SQL As String

    ' Form load event to initialize any necessary data
    Private Sub frmManage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadUserTypes()
        LoadUsers()
        CustomizeDataGridView() ' เรียกฟังก์ชันปรับแต่ง DataGridView
    End Sub

    ' Method to load user types into the Guna2ComboBox
    Private Sub LoadUserTypes()
        cmbUsertype.Items.Add("Admin")
        cmbUsertype.Items.Add("ประธาน")
        cmbUsertype.Items.Add("เหรัญญิก")
        cmbUsertype.Items.Add("กรรมการ")
        cmbUsertype.SelectedIndex = 0 ' Default selection
    End Sub

    Private Sub CustomizeDataGridView()
        ' ตั้งค่าฟอนต์สำหรับ Guna2DataGridView
        gunaDataGridView1.DefaultCellStyle.Font = New Font("FC Minimal", 20, FontStyle.Regular)
        gunaDataGridView1.ColumnHeadersDefaultCellStyle.Font = New Font("FC Minimal", 22)
        gunaDataGridView1.RowTemplate.Height = 40


    End Sub

    ' Method to load users into the Guna2DataGridView
    Private Sub LoadUsers()
        Try
            ' ใช้ Conn ที่ประกาศไว้ระดับคลาส ไม่ต้องประกาศใหม่
            If Conn.State = ConnectionState.Closed Then Conn.Open()
            SQL = "SELECT user_id, user_name, user_pass, user_type, user_fname, user_tel FROM Users"
            da = New OleDbDataAdapter(SQL, Conn)
            dt = New DataTable()
            da.Fill(dt)

            ' ตั้งค่าชื่อหัวคอลัมน์เป็นภาษาไทย
            gunaDataGridView1.DataSource = dt
            gunaDataGridView1.Columns(0).HeaderText = "รหัสผู้ใช้"
            gunaDataGridView1.Columns(1).HeaderText = "ชื่อผู้ใช้"
            gunaDataGridView1.Columns(2).HeaderText = "รหัสผ่าน"
            gunaDataGridView1.Columns(3).HeaderText = "ประเภทผู้ใช้"
            gunaDataGridView1.Columns(4).HeaderText = "ชื่อ-นามสกุล"
            gunaDataGridView1.Columns(5).HeaderText = "เบอร์โทรศัพท์" ' ตรวจสอบให้แสดงเพียงคอลัมน์เดียว
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' ปิดการเชื่อมต่อหลังจากใช้งานเสร็จ
            If Conn.State = ConnectionState.Open Then Conn.Close()
        End Try
    End Sub


    Private Sub gunaDataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles gunaDataGridView1.CellClick
        ' ตรวจสอบว่ามีการเลือกแถวหรือไม่
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = gunaDataGridView1.Rows(e.RowIndex)

            ' โหลดข้อมูลจาก DataGridView ลงในฟิลด์ต่าง ๆ
            txtUsername.Text = row.Cells("user_name").Value.ToString()
            txtPassword.Text = row.Cells("user_pass").Value.ToString()
            cmbUsertype.SelectedItem = row.Cells("user_type").Value.ToString()
            txtFname.Text = row.Cells("user_fname").Value.ToString()
            txtTel.Text = row.Cells("user_tel").Value.ToString()

            ' เปิดการใช้งานปุ่มแก้ไข และปิดปุ่มบันทึก
            btnSave.Enabled = False   ' ปิดปุ่มบันทึกเพื่อไม่ให้บันทึกข้อมูลใหม่
            btnEdit.Enabled = True    ' เปิดการใช้งานปุ่มแก้ไข
        End If
    End Sub


    ' Button click event for saving user data (Guna2Button)
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        ' ตรวจสอบว่าทุกฟิลด์ได้รับการกรอกข้อมูลครบถ้วน
        If String.IsNullOrEmpty(txtUsername.Text) Or String.IsNullOrEmpty(txtPassword.Text) Or String.IsNullOrEmpty(txtFname.Text) Or String.IsNullOrEmpty(txtTel.Text) Then
            MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' บันทึกข้อมูลใหม่
        Try
            If Conn.State = ConnectionState.Closed Then Conn.Open()

            ' ตรวจสอบว่าชื่อผู้ใช้งานซ้ำหรือไม่
            SQL = "SELECT COUNT(*) FROM Users WHERE user_name = @Username"
            cmd = New OleDbCommand(SQL, Conn)
            cmd.Parameters.AddWithValue("@Username", txtUsername.Text)
            Dim userCount As Integer = Convert.ToInt32(cmd.ExecuteScalar())

            If userCount > 0 Then
                MessageBox.Show("Username already exists. Please choose a different username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            ' เพิ่มผู้ใช้ใหม่
            SQL = "INSERT INTO Users (user_name, user_pass, user_type, user_fname, user_tel) VALUES (@Username, @Password, @UserType, @FullName, @PhoneNumber)"
            cmd = New OleDbCommand(SQL, Conn)
            cmd.Parameters.AddWithValue("@Username", txtUsername.Text)
            cmd.Parameters.AddWithValue("@Password", txtPassword.Text)
            cmd.Parameters.AddWithValue("@UserType", cmbUsertype.SelectedItem.ToString())
            cmd.Parameters.AddWithValue("@FullName", txtFname.Text)
            cmd.Parameters.AddWithValue("@PhoneNumber", txtTel.Text)
            cmd.ExecuteNonQuery()

            MessageBox.Show("User added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' ล้างข้อมูลในฟิลด์
            ClearFields()

            ' โหลดข้อมูลผู้ใช้ใหม่
            LoadUsers()
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If Conn.State = ConnectionState.Open Then Conn.Close()
        End Try
    End Sub

    Private Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        ' ตรวจสอบว่าทุกฟิลด์ได้รับการกรอกข้อมูลครบถ้วน
        If txtUsername.Text = "" Or txtPassword.Text = "" Or txtFname.Text = "" Or txtTel.Text = "" Then
            MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Try
            ' อัปเดตข้อมูลผู้ใช้ในฐานข้อมูล
            If Conn.State = ConnectionState.Closed Then Conn.Open()

            ' SQL สำหรับอัปเดตข้อมูล
            SQL = "UPDATE Users SET user_pass = @Password, user_type = @UserType, user_fname = @FullName, user_tel = @PhoneNumber WHERE user_name = @Username"
            cmd = New OleDbCommand(SQL, Conn)
            cmd.Parameters.AddWithValue("@Password", txtPassword.Text)
            cmd.Parameters.AddWithValue("@UserType", cmbUsertype.SelectedItem.ToString())
            cmd.Parameters.AddWithValue("@FullName", txtFname.Text)
            cmd.Parameters.AddWithValue("@PhoneNumber", txtTel.Text)
            cmd.Parameters.AddWithValue("@Username", txtUsername.Text)
            cmd.ExecuteNonQuery()

            MessageBox.Show("User information updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            ' โหลดข้อมูลผู้ใช้ใหม่
            LoadUsers()

            ' ล้างข้อมูลในฟิลด์
            ClearFields()

            ' กลับไปเปิดการใช้งานปุ่มบันทึก
            btnSave.Enabled = True
            btnEdit.Enabled = False ' ปิดการใช้งานปุ่มแก้ไข
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If Conn.State = ConnectionState.Open Then Conn.Close()
        End Try
    End Sub

    ' Button click event for deleting user data (Guna2Button)
    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        ' ตรวจสอบว่ามีการเลือกแถวหรือไม่
        If gunaDataGridView1.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a user to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim username As String = gunaDataGridView1.SelectedRows(0).Cells("user_name").Value.ToString()
        DeleteUser(username)

        ' โหลดข้อมูลผู้ใช้ใหม่
        LoadUsers()
    End Sub

    ' Method to delete user data
    Private Sub DeleteUser(username As String)
        Try
            If Conn.State = ConnectionState.Closed Then Conn.Open()

            SQL = "DELETE FROM Users WHERE user_name = @Username"
            cmd = New OleDbCommand(SQL, Conn)
            cmd.Parameters.AddWithValue("@Username", username)
            cmd.ExecuteNonQuery()

            MessageBox.Show("User deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If Conn.State = ConnectionState.Open Then Conn.Close()
        End Try
    End Sub

    ' Button click event for canceling (Guna2Button)
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        ' ล้างข้อมูลในฟิลด์ทั้งหมด
        ClearFields()

        ' กำหนดปุ่มบันทึกให้พร้อมใช้งานสำหรับการเพิ่มข้อมูลใหม่
        btnSave.Enabled = True  ' เปิดใช้งานปุ่มบันทึกสำหรับเพิ่มข้อมูลใหม่
        btnEdit.Enabled = False ' ปิดการใช้งานปุ่มแก้ไขข้อมูล
    End Sub

    Private Sub ClearFields()
        txtUsername.Clear()
        txtPassword.Clear()
        txtFname.Clear()
        txtTel.Clear()
        cmbUsertype.SelectedIndex = 0 ' Reset to default selection
    End Sub

    ' ฟังก์ชันค้นหาเมื่อมีการพิมพ์ใน TextBox
    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Try
            If Conn.State = ConnectionState.Closed Then Conn.Open()

            ' รับค่าจาก TextBox สำหรับการค้นหา
            Dim searchText As String = txtSearch.Text.Trim()

            ' SQL Query สำหรับค้นหาข้อมูลในตาราง Users โดยใช้ LIKE
            SQL = "SELECT user_id, user_name, user_pass, user_type, user_fname, user_tel FROM Users " &
                  "WHERE user_name LIKE @search OR user_type LIKE @search OR user_fname LIKE @search OR user_tel LIKE @search"

            ' ใช้ @search สำหรับค่าที่จะค้นหา
            cmd = New OleDbCommand(SQL, Conn)
            cmd.Parameters.AddWithValue("@search", "%" & searchText & "%")

            ' ดึงข้อมูลและแสดงใน DataGridView
            da = New OleDbDataAdapter(cmd)
            dt = New DataTable()
            da.Fill(dt)

            ' อัปเดตข้อมูลใน DataGridView
            gunaDataGridView1.DataSource = dt

        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการค้นหา: " & ex.Message)
        Finally
            If Conn.State = ConnectionState.Open Then Conn.Close()
        End Try
    End Sub


End Class
