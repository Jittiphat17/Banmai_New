Imports System.Data.OleDb
Imports System.Globalization
Imports System.Windows.Forms.DataVisualization.Charting
Imports Microsoft.Reporting

Public Class frmMain
    ' เชื่อมต่อกับฐานข้อมูล
    Dim Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")

    Public Sub Loadinfo()
        ' อัปเดตข้อมูลการใช้งานสำหรับ Admin
        If User_type = "Admin" Then
            tsm_exp.Enabled = True
            tsm_inc.Enabled = True
            tsm_report.Enabled = True
            tsm_other.Enabled = True
        Else
            tsm_other.Enabled = False
        End If
    End Sub

    Private Sub UpdateUserInfo()
        ' แสดงผู้ใช้งานปัจจุบันใน Label
        lblUserInfo.Text = $"ผู้ใช้ขณะนี้ : {User_type}"
    End Sub

    Private Sub UpdateDateTime()
        ' แสดงวันที่และเวลา
        lblDateTime.Text = DateTime.Now.ToString("d MMMM yyyy เวลา HH:mm:ss น.", CultureInfo.CreateSpecificCulture("th-TH"))
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        ' อัปเดตวันที่และเวลาในทุกๆ 1 วินาที
        UpdateDateTime()
    End Sub

    Private Sub frmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Loadinfo()

        ' อัปเดตข้อมูลสมาชิกและสัญญา
        UpdateMemberCount()
        UpdateContractCount()

        ' อัปเดตยอดเงินในบัญชี
        UpdateAccountBalances()


        ' อัปเดตข้อมูลผู้ใช้และเวลา
        UpdateUserInfo()
        UpdateDateTime()

        ' เริ่มการทำงานของ Timer
        Timer1.Start()
    End Sub

    ' ฟังก์ชันดึงข้อมูลจำนวนสมาชิก
    Private Sub UpdateMemberCount()
        Try
            ' SQL Query เพื่อดึงจำนวนสมาชิก
            Dim strSQL As String = "SELECT COUNT(*) FROM Member"
            Dim cmd As New OleDbCommand(strSQL, Conn)

            ' เปิดการเชื่อมต่อฐานข้อมูล
            If Conn.State = ConnectionState.Open Then Conn.Close()
            Conn.Open()

            ' ดึงจำนวนสมาชิกและแสดงใน Label
            Dim memberCount As Integer = Convert.ToInt32(cmd.ExecuteScalar())
            lbCount.Text = "จำนวนสมาชิก: " & memberCount.ToString() & " ราย"
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' ปิดการเชื่อมต่อฐานข้อมูล
            Conn.Close()
        End Try
    End Sub

    ' ฟังก์ชันดึงข้อมูลจำนวนสัญญา
    Private Sub UpdateContractCount()
        Try
            ' SQL Query เพื่อดึงจำนวนสัญญา
            Dim strSQL As String = "SELECT COUNT(*) FROM Contract"
            Dim cmd As New OleDbCommand(strSQL, Conn)

            ' เปิดการเชื่อมต่อฐานข้อมูล
            If Conn.State = ConnectionState.Open Then Conn.Close()
            Conn.Open()

            ' ดึงจำนวนสัญญาและแสดงใน Label
            Dim contractCount As Integer = Convert.ToInt32(cmd.ExecuteScalar())
            lbContractCount.Text = "จำนวนสัญญา: " & contractCount.ToString() & " ราย"
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' ปิดการเชื่อมต่อฐานข้อมูล
            Conn.Close()
        End Try
    End Sub

    ' ฟังก์ชันดึงยอดเงินจากบัญชี
    Private Sub UpdateAccountBalances()
        Try
            ' เปิดการเชื่อมต่อฐานข้อมูล
            If Conn.State = ConnectionState.Open Then Conn.Close()
            Conn.Open()

            ' ดึงยอดเงินจากบัญชีเงินฝาก
            Dim querySaving As String = "SELECT SUM(inc_amount) FROM Income WHERE acc_id = 'ACC002'"
            Dim cmdSaving As New OleDbCommand(querySaving, Conn)
            Dim totalSaving As Object = cmdSaving.ExecuteScalar()
            If IsDBNull(totalSaving) OrElse totalSaving Is Nothing Then
                totalSaving = 0
            End If

            ' ดึงยอดเงินจากบัญชีกู้เงิน
            Dim queryLoan As String = "SELECT SUM(con_amount) FROM Contract WHERE acc_id = 'ACC001'"
            Dim cmdLoan As New OleDbCommand(queryLoan, Conn)
            Dim totalLoan As Object = cmdLoan.ExecuteScalar()
            If IsDBNull(totalLoan) OrElse totalLoan Is Nothing Then
                totalLoan = 0
            End If

            ' ดึงยอดเงินจากบัญชีกู้เงินสาธารณะ
            Dim queryPublicLoan As String = "SELECT SUM(con_amount) FROM Contract WHERE acc_id = 'ACC003'"
            Dim cmdPublicLoan As New OleDbCommand(queryPublicLoan, Conn)
            Dim totalPublicLoan As Object = cmdPublicLoan.ExecuteScalar()
            If IsDBNull(totalPublicLoan) OrElse totalPublicLoan Is Nothing Then
                totalPublicLoan = 0
            End If

        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการดึงข้อมูลยอดเงิน: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ' ปิดการเชื่อมต่อฐานข้อมูล
            Conn.Close()
        End Try
    End Sub

    ' ฟังก์ชันเพื่อรีเฟรชข้อมูลในหน้าหลักเมื่อปิดฟอร์มอื่นๆ
    Private Sub RefreshMainForm(ByVal sender As Object, ByVal e As FormClosedEventArgs)
        ' เรียกใช้ฟังก์ชันเพื่อรีเฟรชข้อมูลในหน้าหลัก
        UpdateMemberCount()
        UpdateContractCount()
        UpdateDateTime() ' อัปเดตวันที่และเวลาหากต้องการ
    End Sub

    Private Sub ทำสญญาToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ทำสญญาToolStripMenuItem.Click
        Dim frm As New frmBrrow
        AddHandler frm.FormClosed, AddressOf RefreshMainForm
        frm.ShowDialog()
    End Sub

    Private Sub แกไขสญญาToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles แกไขสญญาToolStripMenuItem.Click
        Dim frm As New frmEditContract
        AddHandler frm.FormClosed, AddressOf RefreshMainForm
        frm.ShowDialog()
    End Sub


    Private Sub แกToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles แกToolStripMenuItem.Click
        Dim frm As New frmEditExpense

        AddHandler frm.FormClosed, AddressOf RefreshMainForm
        frm.ShowDialog()
    End Sub

    Private Sub ตารางเงนกToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ตารางเงนกToolStripMenuItem.Click
        Dim frm As New frmCon
        AddHandler frm.FormClosed, AddressOf RefreshMainForm
        frm.ShowDialog()
    End Sub

    Private Sub รายงานสญญาเงนกToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles รายงานสญญาเงนกToolStripMenuItem.Click
        Dim frm As New frmSearch
        AddHandler frm.FormClosed, AddressOf RefreshMainForm
        frm.ShowDialog()
    End Sub

    Private Sub จดการสมาชกToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles จดการสมาชกToolStripMenuItem.Click
        Dim frm As New frmManageMembers
        AddHandler frm.FormClosed, AddressOf RefreshMainForm
        frm.ShowDialog()
    End Sub

    Private Sub ดสมาชกToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ดสมาชกToolStripMenuItem.Click
        Dim frm As New frmViewMember()
        AddHandler frm.FormClosed, AddressOf RefreshMainForm
        frm.ShowDialog()
    End Sub

    Private Sub สมาชกลาออกToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles สมาชกลาออกToolStripMenuItem.Click
        Dim frm As New frmMemberResign
        AddHandler frm.FormClosed, AddressOf RefreshMainForm
        frm.ShowDialog()
    End Sub

    Private Sub อานบตรToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles อานบตรToolStripMenuItem.Click
        Dim frm As New frmCard
        AddHandler frm.FormClosed, AddressOf RefreshMainForm
        frm.ShowDialog()
    End Sub

    Private Sub สำรองขอมลToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles สำรองขอมลToolStripMenuItem.Click
        Dim frm As New frmBackup
        AddHandler frm.FormClosed, AddressOf RefreshMainForm
        frm.ShowDialog()
    End Sub

    Private Sub นำเขาขอมลToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles นำเขาขอมลToolStripMenuItem.Click
        Dim frm As New frmImport
        AddHandler frm.FormClosed, AddressOf RefreshMainForm
        frm.ShowDialog()
    End Sub

    Private Sub จดการสทธToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles จดการสทธToolStripMenuItem.Click
        Dim frm As New frmManageUser
        AddHandler frm.FormClosed, AddressOf RefreshMainForm
        frm.ShowDialog()
    End Sub


    Private Sub แกไขรายรบToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles แกไขรายรบToolStripMenuItem.Click
        Dim frm As New frmEditIncome
        AddHandler frm.FormClosed, AddressOf RefreshMainForm
        frm.ShowDialog()
    End Sub



    Private Sub ออกจากระบบToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles ออกจากระบบToolStripMenuItem1.Click
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        ' ตรวจสอบว่าผู้ใช้คลิก Yes หรือไม่
        If result = DialogResult.Yes Then
            ' ซ่อนฟอร์มปัจจุบัน
            Me.Hide()

            ' แสดง Form1 (หน้าล็อกอินหรือหน้าแรก)
            Dim form1 As New Form1()
            form1.Show()
        End If
    End Sub

    Private Sub รายรบToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles รายรบToolStripMenuItem.Click
        Dim frm As New frmIncome
        AddHandler frm.FormClosed, AddressOf RefreshMainForm
        frm.ShowDialog()
    End Sub

    Private Sub บนทกคาใชจายToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles บนทกคาใชจายToolStripMenuItem.Click
        Dim frm As New frmExpense
        AddHandler frm.FormClosed, AddressOf RefreshMainForm
        frm.ShowDialog()
    End Sub

    Private Sub รายงานดอกเบยสจจะToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles รายงานดอกเบยสจจะToolStripMenuItem.Click
        Dim frm As New frmSajja
        AddHandler frm.FormClosed, AddressOf RefreshMainForm
        frm.ShowDialog()
    End Sub

    Private Sub จดการกองทนToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles จดการกองทนToolStripMenuItem.Click
        Dim frm As New frmFundManagement
        AddHandler frm.FormClosed, AddressOf RefreshMainForm
        frm.ShowDialog()
    End Sub

    Private Sub หนToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles หนToolStripMenuItem.Click
        Dim frm As New frmShare
        AddHandler frm.FormClosed, AddressOf RefreshMainForm
        frm.ShowDialog()
    End Sub

    Private Sub รายงานลกหนรายตวToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles รายงานลกหนรายตวToolStripMenuItem.Click
        Dim frm As New frmDebtor
        AddHandler frm.FormClosed, AddressOf RefreshMainForm
        frm.ShowDialog()
    End Sub

    Private Sub รายงานงบกำไรขาดทนToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles รายงานงบกำไรขาดทนToolStripMenuItem.Click
        Dim frm As New frmStatement
        AddHandler frm.FormClosed, AddressOf RefreshMainForm
        frm.ShowDialog()
    End Sub

    Private Sub ทดลองรายงานToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ทดลองรายงานToolStripMenuItem.Click
        Dim frm As New ProfitAndLossReportForm
        AddHandler frm.FormClosed, AddressOf RefreshMainForm
        frm.ShowDialog()
    End Sub

    Private Sub การปดงบToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles การปดงบToolStripMenuItem.Click
        Dim frm As New frmClose
        AddHandler frm.FormClosed, AddressOf RefreshMainForm
        frm.ShowDialog()
    End Sub

End Class
