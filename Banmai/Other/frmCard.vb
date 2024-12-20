﻿Imports ThaiNationalIDCard
Imports System.ComponentModel
Imports System.Globalization
Imports System.Data.OleDb
Imports System.IO

Public Class frmCard

    Dim bgWorker As New BackgroundWorker

    ' ประกาศ connectionString ที่ระดับคลาส เพื่อให้สามารถใช้งานได้ในทุกฟังก์ชัน
    Private Conn As OleDbConnection
    ' ฟังก์ชันสำหรับดึงเส้นทางฐานข้อมูลจาก config.ini
    Private Function GetDatabasePath() As String
        Dim iniPath As String = Path.Combine(Application.StartupPath, "config.ini")

        ' ตรวจสอบว่าไฟล์ config.ini มีอยู่หรือไม่
        If Not File.Exists(iniPath) Then
            Throw New Exception($"ไม่พบไฟล์ config.ini ที่: {iniPath}")
        End If

        ' อ่านบรรทัดทั้งหมดใน config.ini
        Dim lines = File.ReadAllLines(iniPath)

        ' ค้นหาบรรทัดที่มี Path=
        Dim dbPathLine = lines.FirstOrDefault(Function(line) line.StartsWith("Path="))
        If String.IsNullOrEmpty(dbPathLine) Then
            Throw New Exception("ไม่พบ 'Path' ของฐานข้อมูลใน config.ini")
        End If

        ' แปลง path ให้เป็น Absolute Path
        Dim dbPath = dbPathLine.Replace("Path=", "").Trim()
        If dbPath.StartsWith(".\") Then
            dbPath = Path.Combine(Application.StartupPath, dbPath.Substring(2))
        End If

        ' ตรวจสอบว่าไฟล์ฐานข้อมูลมีอยู่จริง
        If Not File.Exists(dbPath) Then
            Throw New Exception($"ไม่พบไฟล์ฐานข้อมูลที่: {dbPath}")
        End If

        Return dbPath
    End Function


    ' ฟังก์ชันที่รันเมื่อเปิดฟอร์ม
    Private Sub frmSmartCard_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
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
        LoadMemberStatus()

        AddHandler txtFirstNameThai.KeyPress, AddressOf ThaiOnlyTextBox_KeyPress
        AddHandler txtLastNameThai.KeyPress, AddressOf ThaiOnlyTextBox_KeyPress
        AddHandler txtIDCard.KeyPress, AddressOf NumberOnlyTextBox_KeyPress
        AddHandler txtHouseNo.KeyPress, AddressOf NumberOnlyTextBox_KeyPress

        Call SetupScreen()
        If Not GetReader() Then Return
        ProgressBar1.Visible = False

        ' Initialized BackgroundWorker
        With bgWorker
            .WorkerReportsProgress = True
            .WorkerSupportsCancellation = True
        End With

        ' Add Event Handler สำหรับ BackgroundWorker
        AddHandler bgWorker.DoWork, AddressOf bgWorker_DoWork
        AddHandler bgWorker.RunWorkerCompleted, AddressOf bgWorker_RunWorkerCompleted
    End Sub

    Private Sub ThaiOnlyTextBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtFirstNameThai.KeyPress, txtLastNameThai.KeyPress
        ' ยอมรับเฉพาะตัวอักษรภาษาไทยและการควบคุม (เช่น Backspace)
        If Not Char.IsControl(e.KeyChar) AndAlso Not IsThaiChar(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    Private Function IsThaiChar(c As Char) As Boolean
        ' ช่วงรหัส Unicode สำหรับภาษาไทย
        Return (c >= ChrW(&HE01) AndAlso c <= ChrW(&HE5B))
    End Function

    Private Sub NumberOnlyTextBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtHouseNo.KeyPress
        ' ยอมรับเฉพาะตัวเลขและการควบคุม (เช่น Backspace)
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    ' ฟังก์ชันที่จะถูกเรียกเมื่อ BackgroundWorker ทำงานเสร็จ
    Private Sub bgWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs)
        ' ซ่อน ProgressBar
        ProgressBar1.Visible = False

        ' ตรวจสอบว่าการทำงานเสร็จสมบูรณ์หรือไม่
        If e.Error IsNot Nothing Then
            MessageBox.Show("An error occurred: " & e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            LogError(e.Error)
        ElseIf e.Cancelled Then
            MessageBox.Show("Operation was cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Card read successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        btnRead.Enabled = True
    End Sub

    ' ฟังก์ชันการทำงานของ BackgroundWorker สำหรับการอ่านข้อมูลบัตรประชาชน
    Private Sub bgWorker_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs)
        Try
            ' ตรวจสอบการยกเลิก
            If bgWorker.CancellationPending Then
                e.Cancel = True
                Return
            End If

            ' อ่านข้อมูลบัตรประชาชน
            Dim ID As New ThaiIDCard
            Dim Personal As Personal = ID.readAllPhoto
            If Not IsNothing(Personal) Then
                ' ใช้ Invoke เพื่ออัปเดต UI controls บน UI Thread
                Me.Invoke(New MethodInvoker(Sub()
                                                With Personal
                                                    ' แสดงข้อมูลบัตรประชาชน
                                                    txtIDCard.Text = .Citizenid
                                                    txtPreFixThai.Text = .Th_Prefix
                                                    txtPrefixEng.Text = .En_Prefix
                                                    txtFirstNameThai.Text = .Th_Firstname
                                                    txtLastNameThai.Text = .Th_Lastname
                                                    txtFirstNameEng.Text = .En_Firstname
                                                    txtLastNameEng.Text = .En_Lastname
                                                    txtSex.Text = CheckSex(.Sex, txtSex)

                                                    ' สำหรับวันเกิด
                                                    If Not String.IsNullOrWhiteSpace(.Birthday) Then
                                                        Dim birthDateString As String = .Birthday
                                                        Dim birthDate As DateTime
                                                        Dim formats() As String = {"M/d/yyyy", "dd/MM/yyyy", "yyyyMMdd"} ' เพิ่มรูปแบบที่เป็นไปได้
                                                        If DateTime.TryParseExact(birthDateString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, birthDate) Then
                                                            ' แปลงจากปีพุทธศักราชเป็นคริสตศักราช (ถ้าจำเป็น)
                                                            If birthDate.Year > 2500 Then
                                                                birthDate = birthDate.AddYears(-543)
                                                            End If
                                                            txtBirthDate.Text = birthDate.ToString("dd/MM/yyyy")
                                                            ' คำนวณอายุ
                                                            Dim age As Integer = DateTime.Now.Year - birthDate.Year
                                                            If DateTime.Now < birthDate.AddYears(age) Then age -= 1
                                                            lblAge.Text = age.ToString()
                                                        Else
                                                            MessageBox.Show($"Unable to parse birth date: {birthDateString}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                                        End If
                                                    Else
                                                        MessageBox.Show("Birth date is empty or null", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                                                    End If

                                                    ' สำหรับวันออกบัตรและวันหมดอายุ
                                                    Try
                                                        Dim dateFormats() As String = {"yyyyMMdd", "dd/MM/yyyy", "M/d/yyyy"}

                                                        ' ตรวจสอบวันออกบัตรและแปลงรูปแบบ
                                                        If Not String.IsNullOrWhiteSpace(.Issue) Then
                                                            Dim issueDate As DateTime
                                                            If DateTime.TryParseExact(.Issue, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, issueDate) Then
                                                                ' แปลงจากปีพุทธศักราช (ถ้ามี)
                                                                If issueDate.Year > 2500 Then
                                                                    issueDate = issueDate.AddYears(-543)
                                                                End If
                                                                txtIssueDate.Text = issueDate.ToString("dd/MM/yyyy")
                                                            Else
                                                                txtIssueDate.Text = "Invalid Date"
                                                            End If
                                                        Else
                                                            txtIssueDate.Text = "N/A"
                                                        End If

                                                        ' ตรวจสอบวันหมดอายุและแปลงรูปแบบ
                                                        If Not String.IsNullOrWhiteSpace(.Expire) Then
                                                            Dim expireDate As DateTime
                                                            If DateTime.TryParseExact(.Expire, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, expireDate) Then
                                                                ' แปลงจากปีพุทธศักราช (ถ้ามี)
                                                                If expireDate.Year > 2500 Then
                                                                    expireDate = expireDate.AddYears(-543)
                                                                End If
                                                                txtExpireDate.Text = expireDate.ToString("dd/MM/yyyy")
                                                            Else
                                                                txtExpireDate.Text = "Invalid Date"
                                                            End If
                                                        Else
                                                            txtExpireDate.Text = "N/A"
                                                        End If
                                                    Catch ex As Exception
                                                        MessageBox.Show("Error processing dates: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                                        LogError(ex)
                                                    End Try

                                                    ' แสดงที่อยู่
                                                    txtHouseNo.Text = .addrHouseNo
                                                    txtVillageNo.Text = .addrVillageNo
                                                    txtLane.Text = .addrLane
                                                    txtRoad.Text = .addrRoad
                                                    txtTambol.Text = .addrTambol
                                                    txtAmphur.Text = .addrAmphur
                                                    txtProvince.Text = .addrProvince
                                                    ' แสดงรูปภาพ
                                                    picData.Image = .PhotoBitmap
                                                End With
                                            End Sub))
            Else
                Me.Invoke(New MethodInvoker(Sub()
                                                MessageBox.Show("Error reading card. No data retrieved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                            End Sub))
            End If
        Catch ex As Exception
            Me.Invoke(New MethodInvoker(Sub()
                                            MessageBox.Show("Error reading card: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                            LogError(ex)
                                        End Sub))
        End Try
    End Sub

    ' ฟังก์ชันสำหรับการตั้งค่าเริ่มต้นฟอร์ม
    Sub SetupScreen()
        For Each control As Control In Me.Controls
            If TypeOf control Is TextBox Then
                DirectCast(control, TextBox).Clear()
            ElseIf TypeOf control Is ComboBox Then
                DirectCast(control, ComboBox).SelectedIndex = -1
            End If
        Next
        lblAge.Text = ""
        picData.Image = Nothing
    End Sub

    ' ฟังก์ชันคำนวณอายุ
    Public Function CalcDate(sDate As Date, eDate As Date) As String
        Dim vYears As Integer = eDate.Year - sDate.Year
        If eDate < sDate.AddYears(vYears) Then
            vYears -= 1
        End If
        Return vYears.ToString() & " ปี"
    End Function

    ' ฟังก์ชันตรวจสอบเพศ
    Function CheckSex(ByVal sex As Byte, ByRef txt As TextBox) As String
        txt.Tag = sex
        If sex = 1 Then
            Return "ชาย"
        Else
            Return "หญิง"
        End If
    End Function

    ' ตรวจสอบเครื่องอ่านบัตรว่ามีหรือไม่
    Function GetReader() As Boolean
        Try
            Dim ID As New ThaiIDCard
            Dim readers = ID.GetReaders
            If readers Is Nothing OrElse readers.Length = 0 Then
                MessageBox.Show("No smart card readers found.", "Report Status", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            MessageBox.Show("Error detecting smart card reader: " & ex.Message, "Report Status", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            LogError(ex)
            Return False
        End Try
    End Function

    ' ฟังก์ชันบันทึก Log ข้อผิดพลาด
    Private Sub LogError(ex As Exception)
        Try
            Dim logPath As String = Path.Combine(Application.StartupPath, "ErrorLog.txt")
            Using writer As New StreamWriter(logPath, True)
                writer.WriteLine($"[{DateTime.Now}] Error: {ex.Message}")
                writer.WriteLine($"StackTrace: {ex.StackTrace}")
                writer.WriteLine(New String("-"c, 50))
            End Using
        Catch logEx As Exception
            MessageBox.Show($"Error writing to log file: {logEx.Message}", "Log Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ฟังก์ชันตรวจสอบความถูกต้องของข้อมูลก่อนบันทึก
    Private Function ValidateData() As Boolean
        ' ตรวจสอบว่าได้กรอกเลขบัตรประชาชนหรือไม่
        If String.IsNullOrWhiteSpace(txtIDCard.Text) Then
            MessageBox.Show("กรุณากรอกเลขบัตรประชาชน", "ข้อมูลไม่ครบถ้วน", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        ' ตรวจสอบว่ากรอกชื่อและนามสกุลหรือไม่
        If String.IsNullOrWhiteSpace(txtFirstNameThai.Text) OrElse String.IsNullOrWhiteSpace(txtLastNameThai.Text) Then
            MessageBox.Show("กรุณากรอกชื่อและนามสกุล", "ข้อมูลไม่ครบถ้วน", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        ' ตรวจสอบว่ากรอกวันเกิดหรือไม่
        If String.IsNullOrWhiteSpace(txtBirthDate.Text) Then
            MessageBox.Show("กรุณากรอกวันเกิด", "ข้อมูลไม่ครบถ้วน", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        ' ตรวจสอบว่ามีการเลือกสถานะใน ComboBox หรือไม่
        If cbStaus.SelectedItem Is Nothing Then
            MessageBox.Show("กรุณาเลือกสถานะ", "ข้อมูลไม่ครบถ้วน", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        ' เพิ่มการตรวจสอบข้อมูลอื่นๆ ตามต้องการ

        Return True
    End Function


    ' ฟังก์ชันล้างข้อมูลทั้งหมดในฟอร์ม
    Private Sub ClearAllFields()
        For Each control As Control In Me.Controls
            If TypeOf control Is TextBox Then
                DirectCast(control, TextBox).Clear()
            ElseIf TypeOf control Is ComboBox Then
                DirectCast(control, ComboBox).SelectedIndex = -1
            End If
        Next
        picData.Image = Nothing
        lblAge.Text = ""
    End Sub

    ' ฟังก์ชันอ่านข้อมูลจากบัตรประชาชน
    Private Sub btnRead_Click_1(sender As Object, e As EventArgs) Handles btnRead.Click
        Call SetupScreen()
        If Not GetReader() Then Return
        btnRead.Enabled = False

        If Not bgWorker.IsBusy Then
            ProgressBar1.Style = ProgressBarStyle.Marquee
            ProgressBar1.Visible = True
            bgWorker.RunWorkerAsync()
        Else
            MessageBox.Show("Process is already running. Please wait for it to complete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    ' ฟังก์ชันบันทึกข้อมูลลงฐานข้อมูล
    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        ' ตรวจสอบข้อมูลเบื้องต้นก่อนทำการบันทึก
        If Not ValidateData() Then Return

        ' ตรวจสอบว่าเลขบัตรประชาชนมีอยู่แล้วหรือไม่
        If IsThaiIDExist(txtIDCard.Text) Then
            MessageBox.Show("เลขบัตรประชาชนนี้มีอยู่ในระบบแล้ว ไม่สามารถบันทึกซ้ำได้", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Try
            ' ตรวจสอบว่า ComboBox สถานะถูกเลือกหรือไม่
            If cbStaus.SelectedItem Is Nothing Then
                MessageBox.Show("กรุณาเลือกสถานะ", "ข้อมูลไม่ครบถ้วน", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' ดึงค่า s_id จาก ComboBox
            Dim selectedStatus As KeyValuePair(Of Integer, String) = DirectCast(cbStaus.SelectedItem, KeyValuePair(Of Integer, String))
            Dim statusID As Integer = selectedStatus.Key ' ดึงค่า s_id จาก ComboBox

            ' ดึง path ของฐานข้อมูลจาก config.ini
            Dim dbPath As String = GetDatabasePath()
            Dim connStr As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath}"

            Using conn As New OleDbConnection(connStr)
                conn.Open()

                ' รวมข้อมูลที่อยู่จากหลายฟิลด์
                Dim fullAddress As String = $" {txtHouseNo.Text}  {txtVillageNo.Text} ตรอก/ซอย {txtLane.Text} ถนน {txtRoad.Text}  {txtTambol.Text}  {txtAmphur.Text}  {txtProvince.Text}"

                ' คำสั่ง SQL สำหรับบันทึกข้อมูลลงในตาราง Member รวม s_id
                Dim query As String = "INSERT INTO Member (m_gender, m_name, m_nick, m_job, m_tel, m_post, m_birth, m_national, m_thaiid, m_address, m_age, s_id) " &
              "VALUES (@Gender, @Name, @Nick, @Jobs, @Tel, @Post, @Birth, @National, @ThaiID, @Address, @Age, @StatusID)"

                Using command As New OleDbCommand(query, conn)
                    ' เพิ่มพารามิเตอร์ข้อมูล
                    command.Parameters.AddWithValue("@Gender", txtPreFixThai.Text)
                    command.Parameters.AddWithValue("@Name", $"{txtFirstNameThai.Text} {txtLastNameThai.Text}")
                    command.Parameters.AddWithValue("@Nick", txtNick.Text)
                    command.Parameters.AddWithValue("@Jobs", txtJob.Text)
                    command.Parameters.AddWithValue("@Tel", txtTel.Text)
                    command.Parameters.AddWithValue("@Post", txtPost.Text)

                    ' แปลงและตรวจสอบวันเกิด
                    Dim birthDate As DateTime
                    If DateTime.TryParseExact(txtBirthDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, birthDate) Then
                        command.Parameters.AddWithValue("@Birth", birthDate)
                    Else
                        Throw New FormatException("Invalid birth date format")
                    End If

                    command.Parameters.AddWithValue("@National", txtNationality.Text)
                    command.Parameters.AddWithValue("@ThaiID", txtIDCard.Text)
                    command.Parameters.AddWithValue("@Address", fullAddress)

                    ' แปลงอายุเป็นตัวเลข
                    Dim age As Integer
                    If Integer.TryParse(lblAge.Text.Replace(" ปี", ""), age) Then
                        command.Parameters.AddWithValue("@Age", age)
                    Else
                        Throw New FormatException("Invalid age format")
                    End If

                    ' เพิ่ม s_id ลงในพารามิเตอร์
                    command.Parameters.AddWithValue("@StatusID", statusID)

                    ' บันทึกข้อมูลลงฐานข้อมูล
                    command.ExecuteNonQuery()

                    MessageBox.Show("Data saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    ' เรียกใช้ฟังก์ชันรีเฟรชหน้าจอ
                    ClearAndRefreshForm()
                End Using
            End Using

        Catch ex As FormatException
            MessageBox.Show("Error saving data: " & ex.Message, "Format Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("Error saving data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            LogError(ex)
        End Try
    End Sub



    ' ฟังก์ชันล้างข้อมูลในฟอร์ม
    Private Sub ClearAndRefreshForm()
        ' ล้างข้อมูลใน TextBox ทั้งหมดในฟอร์ม
        For Each ctrl As Control In Me.Controls
            If TypeOf ctrl Is TextBox Then
                DirectCast(ctrl, TextBox).Clear()
            End If
        Next

        ' ล้างข้อมูลใน GroupBox ถ้ามี
        For Each gb As GroupBox In Me.Controls.OfType(Of GroupBox)()
            For Each ctrl As Control In gb.Controls
                If TypeOf ctrl Is TextBox Then
                    DirectCast(ctrl, TextBox).Clear()
                ElseIf TypeOf ctrl Is ComboBox Then
                    DirectCast(ctrl, ComboBox).SelectedIndex = -1
                End If
            Next
        Next

        ' ล้างข้อมูลใน txtIDCard โดยตรง
        txtIDCard.Clear()

        ' รีเซ็ต ProgressBar ถ้ามี
        If ProgressBar1 IsNot Nothing Then
            ProgressBar1.Value = 0
            ProgressBar1.Visible = False
        End If

        ' เปิดใช้งานปุ่มอ่านบัตรอีกครั้ง
        btnRead.Enabled = True

        ' ตั้งค่าหน้าจอใหม่ (ถ้ามีฟังก์ชัน SetupScreen)
        SetupScreen()
    End Sub


    ' ฟังก์ชันตรวจสอบเลขบัตรประชาชน
    Private Function IsThaiIDExist(thaiID As String) As Boolean
        ' ดึง path ของฐานข้อมูลจาก config.ini
        Dim dbPath As String = GetDatabasePath()
        Dim connStr As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath}"

        Using conn As New OleDbConnection(connStr)
            conn.Open()
            Dim query As String = "SELECT COUNT(*) FROM Member WHERE m_thaiid = @ThaiID"
            Using command As New OleDbCommand(query, conn)
                command.Parameters.AddWithValue("@ThaiID", thaiID)
                Dim count As Integer = CInt(command.ExecuteScalar())
                Return count > 0
            End Using
        End Using
    End Function

    Private Sub LoadMemberStatus()
        Try
            ' ดึง path ของฐานข้อมูลจาก config.ini
            Dim dbPath As String = GetDatabasePath()
            Dim connStr As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath}"

            Using conn As New OleDbConnection(connStr)
                conn.Open()

                ' ดึงข้อมูลจากตาราง Memberstatus
                Dim query As String = "SELECT s_id, s_namestatus FROM Memberstatus"
                Using command As New OleDbCommand(query, conn)
                    Using reader As OleDbDataReader = command.ExecuteReader()
                        ' เคลียร์ข้อมูลเดิมใน ComboBox
                        cbStaus.Items.Clear()

                        ' เพิ่มข้อมูลใหม่ใน ComboBox โดยใช้ KeyValuePair
                        While reader.Read()
                            Dim statusName As String = reader("s_namestatus").ToString()
                            Dim statusID As Integer = CInt(reader("s_id"))
                            ' เพิ่ม KeyValuePair ที่เก็บ s_id เป็น Value และ s_namestatus เป็น Text
                            cbStaus.Items.Add(New KeyValuePair(Of Integer, String)(statusID, statusName))
                        End While

                        ' ตั้งค่าให้ ComboBox แสดงค่า s_namestatus
                        cbStaus.DisplayMember = "Value"
                        cbStaus.ValueMember = "Key"
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading Memberstatus: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            LogError(ex) ' บันทึกข้อผิดพลาดลงใน log ถ้ามีฟังก์ชันนี้
        End Try
    End Sub



    Private Sub cbStaus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbStaus.SelectedIndexChanged
        If cbStaus.SelectedItem IsNot Nothing Then
            ' ตรวจสอบว่า cbStaus.SelectedItem เป็น KeyValuePair ที่ถูกต้อง
            Dim selectedStatus As KeyValuePair(Of Integer, String) = DirectCast(cbStaus.SelectedItem, KeyValuePair(Of Integer, String))
            Dim statusName As String = selectedStatus.Value ' ชื่อสถานะ (s_namestatus)
            Dim statusID As Integer = selectedStatus.Key ' รหัสสถานะ (s_id)

            ' คุณสามารถใช้ statusID และ statusName ในการดำเนินการเพิ่มเติมได้ที่นี่
        End If
    End Sub

End Class
