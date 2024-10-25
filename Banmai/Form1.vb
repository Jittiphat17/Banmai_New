Imports System.Data.OleDb
Imports System.IO

Public Class Form1
    ' การเชื่อมต่อฐานข้อมูล Access แบบใช้ Relative Path
    Dim Conn As OleDbConnection
    Dim cmd As OleDbCommand
    Dim da As OleDbDataAdapter
    Dim dt As DataTable
    Dim SQL As String

    ' ฟังก์ชัน Load เมื่อฟอร์มถูกเปิด
    Private Sub frmLogin_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' ใช้ Relative Path เพื่อเชื่อมต่อกับฐานข้อมูลในโฟลเดอร์ Database
        Dim dbPath As String = Path.Combine(Application.StartupPath, "Database", "db_banmai1.accdb")
        Dim ConnStr As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath}"

        ' สร้างการเชื่อมต่อกับฐานข้อมูล
        Conn = New OleDbConnection(ConnStr)
    End Sub

    ' จำกัดการป้อนข้อมูลเฉพาะตัวอักษรและตัวเลข
    Private Sub txtUser_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtUser.KeyPress
        If Not Char.IsLetterOrDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    ' ฟังก์ชันสำหรับการเข้าสู่ระบบ
    Private Sub btn_login_Click(sender As Object, e As EventArgs) Handles btn_login.Click
        btn_login.Enabled = False  ' ปิดปุ่มเพื่อป้องกันการคลิกซ้ำ

        ' SQL สำหรับการตรวจสอบผู้ใช้
        SQL = "SELECT * FROM Users WHERE user_name= @userName AND user_pass= @userPass"

        Try
            If Conn.State = ConnectionState.Open Then Conn.Close()
            Conn.Open()

            Using cmd As New OleDbCommand(SQL, Conn)
                cmd.Parameters.AddWithValue("@userName", txtUser.Text.Trim())
                cmd.Parameters.AddWithValue("@userPass", txtPass.Text.Trim())

                da = New OleDbDataAdapter(cmd)
                dt = New DataTable()
                da.Fill(dt)

                If dt.Rows.Count > 0 Then
                    User_name = dt.Rows(0)("user_name").ToString()
                    User_type = dt.Rows(0)("user_type").ToString()

                    ' เปิดฟอร์มหลักและซ่อนฟอร์ม Login
                    Dim frm As New frmMain
                    frm.Show()
                    Me.Hide()
                Else
                    MessageBox.Show("ชื่อผู้ใช้งาน หรือ รหัสผ่าน ผิด", "ข้อความจากระบบ", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    txtUser.Focus()
                End If
            End Using

        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาด: " & ex.Message, "ข้อความจากระบบ", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            If Conn.State = ConnectionState.Open Then Conn.Close()
            btn_login.Enabled = True  ' เปิดใช้งานปุ่มอีกครั้ง
        End Try
    End Sub

    ' การจัดการการแสดงหรือซ่อนรหัสผ่าน
    Private Sub Guna2ToggleSwitch1_CheckedChanged(sender As Object, e As EventArgs) Handles Guna2ToggleSwitch1.CheckedChanged
        txtPass.UseSystemPasswordChar = Not Guna2ToggleSwitch1.Checked
    End Sub

    ' ตรวจสอบการออกจากโปรแกรม
    Private Sub Guna2ControlBox1_Click(sender As Object, e As EventArgs) Handles Guna2ControlBox1.Click
        Dim result As DialogResult = MessageBox.Show("คุณต้องการปิดโปรแกรมหรือไม่?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            If Conn IsNot Nothing AndAlso Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
            Application.Exit()
        End If
    End Sub
End Class
