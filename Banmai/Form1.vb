Imports System.IO
Imports System.Data.OleDb

Public Class Form1
    ' ประกาศตัวแปร Conn สำหรับเชื่อมต่อฐานข้อมูล
    Dim Conn As OleDbConnection
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dt As DataTable
    Dim SQL As String

    ' ฟังก์ชันเพื่อดึง path ของฐานข้อมูลจาก config.ini
    Private Function GetDatabasePath() As String
        Dim iniPath As String = Path.Combine(Application.StartupPath, "config.ini")

        ' อ่านไฟล์ config.ini และค้นหาบรรทัดที่มี "Path="
        Dim dbPath As String = File.ReadAllLines(iniPath).
                                FirstOrDefault(Function(line) line.StartsWith("Path="))

        If Not String.IsNullOrEmpty(dbPath) Then
            Return dbPath.Replace("Path=", "").Trim() ' ตัดคำว่า "Path=" ออก
        Else
            Throw New Exception("ไม่พบ Path ของฐานข้อมูลใน config.ini")
        End If
    End Function

    ' ฟังก์ชัน Load เมื่อฟอร์มถูกเปิด
    Private Sub frmLogin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        txtPass.UseSystemPasswordChar = True ' แสดงรหัสผ่านเป็น ●
        txtUser.Focus() ' โฟกัสที่ TextBox ชื่อผู้ใช้

        Try
            ' อ่านค่า path จาก config.ini และสร้างการเชื่อมต่อ
            Dim dbPath As String = GetDatabasePath()
            Dim connStr As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath}"
            Conn = New OleDbConnection(connStr)

        Catch ex As Exception
            ' แสดงข้อความหากเกิดข้อผิดพลาดในการอ่าน config หรือเชื่อมต่อฐานข้อมูล
            MessageBox.Show($"เกิดข้อผิดพลาด: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Exit() ' ปิดโปรแกรมหากไม่สามารถเชื่อมต่อได้
        End Try
    End Sub

    ' ฟังก์ชัน Login เมื่อกดปุ่ม "เข้าสู่ระบบ"
    Private Sub btn_login_Click(sender As Object, e As EventArgs) Handles btn_login.Click
        btn_login.Enabled = False ' ปิดปุ่ม Login ชั่วคราว

        SQL = "SELECT * FROM Users WHERE user_name= @userName AND user_pass= @userPass"

        Try
            If Conn.State = ConnectionState.Closed Then Conn.Open() ' เปิดการเชื่อมต่อ

            ' ตรวจสอบชื่อผู้ใช้และรหัสผ่าน
            Using cmd As New OleDbCommand(SQL, Conn)
                cmd.Parameters.AddWithValue("@userName", txtUser.Text.Trim())
                cmd.Parameters.AddWithValue("@userPass", txtPass.Text.Trim())

                da = New OleDbDataAdapter(cmd)
                dt = New DataTable
                da.Fill(dt)

                If dt.Rows.Count > 0 Then
                    ' หากเข้าสู่ระบบสำเร็จ
                    User_name = dt.Rows(0)("user_name").ToString()
                    User_type = dt.Rows(0)("user_type").ToString()

                    ' เปิดฟอร์มหลักและซ่อนฟอร์ม Login
                    Dim frm As New frmMain
                    frm.Show()
                    Me.Hide()
                Else
                    ' หากชื่อผู้ใช้หรือรหัสผ่านผิด
                    MessageBox.Show("ชื่อผู้ใช้หรือรหัสผ่านไม่ถูกต้อง", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txtUser.Focus()
                End If
            End Using

        Catch ex As Exception
            ' แสดงข้อความหากเกิดข้อผิดพลาด
            MessageBox.Show($"เกิดข้อผิดพลาด: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            If Conn.State = ConnectionState.Open Then Conn.Close() ' ปิดการเชื่อมต่อ
            btn_login.Enabled = True ' เปิดการใช้งานปุ่มอีกครั้ง
        End Try
    End Sub

    ' ฟังก์ชันแสดง/ซ่อนรหัสผ่านเมื่อสลับสวิตช์
    Private Sub Guna2ToggleSwitch1_CheckedChanged(sender As Object, e As EventArgs) Handles Guna2ToggleSwitch1.CheckedChanged
        txtPass.UseSystemPasswordChar = Not Guna2ToggleSwitch1.Checked
    End Sub

    ' ฟังก์ชันปิดโปรแกรมเมื่อคลิกปุ่มปิด
    Private Sub Guna2ControlBox1_Click(sender As Object, e As EventArgs) Handles Guna2ControlBox1.Click
        Application.Exit()
    End Sub

    ' จำกัดการใส่ข้อมูลใน TextBox ให้เฉพาะตัวอักษรภาษาอังกฤษและตัวเลข
    Private Sub txtUser_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtUser.KeyPress
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub
End Class
