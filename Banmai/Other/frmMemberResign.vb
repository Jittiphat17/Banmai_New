﻿Imports System.Data.OleDb
Imports Guna.UI2.WinForms
Imports System.IO
Public Class frmMemberResign
    Private Conn As New OleDbConnection

    Private Sub frmMemberResign_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
        ' ตั้งค่า TextAlign ให้กับ Guna2TextBox ที่ต้องการ
        txtBeforeTotalSaving.TextAlign = HorizontalAlignment.Right
        txtTotalSaving.TextAlign = HorizontalAlignment.Right
        txtLoanAccount1.TextAlign = HorizontalAlignment.Right
        txtLoanSaving.TextAlign = HorizontalAlignment.Right
        txtLoanPublic.TextAlign = HorizontalAlignment.Right
        txtTotalLoan.TextAlign = HorizontalAlignment.Right

        LoadMemberData() ' โหลดข้อมูลสมาชิกเพื่อทำ AutoComplete
    End Sub
    Private Function GetDatabasePath() As String
        Dim iniPath As String = Path.Combine(Application.StartupPath, "config.ini")
        If Not File.Exists(iniPath) Then
            Throw New Exception("ไม่พบไฟล์ config.ini ที่ตำแหน่ง: " & iniPath)
        End If

        ' อ่านบรรทัดทั้งหมดใน config.ini
        Dim lines = File.ReadAllLines(iniPath)

        ' ค้นหาบรรทัดที่มี Path
        Dim dbPathLine = lines.FirstOrDefault(Function(line) line.StartsWith("Path="))
        If String.IsNullOrEmpty(dbPathLine) Then
            Throw New Exception("ไม่พบ 'Path' ในไฟล์ config.ini")
        End If

        ' ดึง path จากบรรทัดนั้นและตัดส่วน 'Path=' ออก
        Dim dbPath = dbPathLine.Replace("Path=", "").Trim()

        ' แปลง path เป็น path แบบเต็ม (Absolute Path)
        If dbPath.StartsWith(".\") Then
            dbPath = Path.Combine(Application.StartupPath, dbPath.Substring(2))
        End If

        If Not File.Exists(dbPath) Then
            Throw New Exception($"ไม่พบไฟล์ฐานข้อมูลที่ตำแหน่ง: {dbPath}")
        End If

        Return dbPath
    End Function

    ' Method to load member data for AutoComplete in Guna2TextBox
    Private Sub LoadMemberData()

        Try
            Conn.Open()
            Dim query As String = "SELECT m_name FROM Member"
            Dim cmd As New OleDbCommand(query, Conn)
            Dim reader As OleDbDataReader = cmd.ExecuteReader()

            Dim autoComplete As New AutoCompleteStringCollection()

            While reader.Read()
                autoComplete.Add(reader("m_name").ToString())
            End While

            txtSearchMember.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            txtSearchMember.AutoCompleteSource = AutoCompleteSource.CustomSource
            txtSearchMember.AutoCompleteCustomSource = autoComplete
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการโหลดข้อมูลสมาชิก: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Conn.Close()
        End Try
    End Sub

    Private Sub txtSearchMember_TextChanged(sender As Object, e As EventArgs) Handles txtSearchMember.TextChanged
        If txtSearchMember.Text.Length > 0 Then
            SearchMemberDetails(txtSearchMember.Text)
        End If
    End Sub

    Private Sub SearchMemberDetails(memberName As String)
        Try
            Conn.Open()

            Dim queryIncomeDetails As String = "SELECT SUM(ind_amount) FROM Income_Details " &
                                   "WHERE m_id = (SELECT m_id FROM Member WHERE m_name = @memberName) " &
                                   "AND ind_accname = 'เงินฝากสัจจะ' AND acc_id = 'ACC002'"
            Dim cmdIncome As New OleDbCommand(queryIncomeDetails, Conn)
            cmdIncome.Parameters.AddWithValue("@memberName", memberName)
            Dim incomeResult As Object = cmdIncome.ExecuteScalar()

            If IsDBNull(incomeResult) OrElse incomeResult Is Nothing Then
                txtBeforeTotalSaving.Text = "0.00" ' แสดงยอดเงินก่อนรวม
                txtTotalSaving.Text = "0.00" ' แสดงยอดเงินรวม (ในกรณีนี้คือค่าเดียวกัน)
            Else
                txtBeforeTotalSaving.Text = Convert.ToDecimal(incomeResult).ToString("N2") ' แสดงยอดเงินก่อนรวม
                txtTotalSaving.Text = txtBeforeTotalSaving.Text ' แสดงยอดเงินรวม
            End If

            ' ตรวจสอบว่าชื่อสมาชิกที่ค้นหาได้ไปเป็นผู้ค้ำประกันให้กับใครในสัญญาใดบ้าง
            Dim queryGuarantorForOthers As String = "SELECT c.con_id, c.con_amount, m.m_name FROM (Guarantor g " &
                                        "INNER JOIN Contract c ON g.con_id = c.con_id) " &
                                        "INNER JOIN Member m ON c.m_id = m.m_id " &
                                        "WHERE g.m_id = (SELECT m_id FROM Member WHERE m_name = @memberName)"

            Dim cmdGuarantorForOthers As New OleDbCommand(queryGuarantorForOthers, Conn)
            cmdGuarantorForOthers.Parameters.AddWithValue("@memberName", memberName)
            Dim guarantorReader As OleDbDataReader = cmdGuarantorForOthers.ExecuteReader()

            Dim guarantorForOthersList As New List(Of String)

            While guarantorReader.Read()
                ' ดึงชื่อและรายละเอียดของสมาชิกที่ผู้ค้นหาได้ไปค้ำให้
                Dim otherMemberName As String = guarantorReader("m_name").ToString()
                Dim contractAmount As Decimal = Convert.ToDecimal(guarantorReader("con_amount"))
                guarantorForOthersList.Add($"{otherMemberName} ยอดเงิน {contractAmount.ToString("N2")} บาท")
            End While
            guarantorReader.Close()

            ' แสดงภาระการค้ำประกันใน TextBox (txtGuarantor)
            If guarantorForOthersList.Count > 0 Then
                txtGuarantor.Text = String.Join(Environment.NewLine, guarantorForOthersList)
            Else
                txtGuarantor.Text = "ไม่มีภาระการค้ำประกัน"
            End If

            ' ดึงข้อมูลสัญญาของสมาชิก
            Dim queryContracts As String = "SELECT con_id, acc_id, con_amount FROM Contract WHERE m_id = (SELECT m_id FROM Member WHERE m_name = @memberName)"
            Dim cmdContracts As New OleDbCommand(queryContracts, Conn)
            cmdContracts.Parameters.AddWithValue("@memberName", memberName)
            Dim contractReader As OleDbDataReader = cmdContracts.ExecuteReader()

            ' กำหนดค่าเริ่มต้นสำหรับยอดเงินกู้
            Dim totalLoanAccount1 As Decimal = 0
            Dim totalLoanSaving As Decimal = 0
            Dim totalLoanPublic As Decimal = 0

            While contractReader.Read()
                Dim contractId As Integer = contractReader("con_id")
                Dim accId As String = contractReader("acc_id").ToString()

                ' คำนวณยอดเงินกู้แยกตามประเภทบัญชี
                If accId = "ACC001" Then
                    totalLoanAccount1 += Convert.ToDecimal(contractReader("con_amount"))
                ElseIf accId = "ACC002" Then
                    totalLoanSaving += Convert.ToDecimal(contractReader("con_amount"))
                ElseIf accId = "ACC003" Then
                    totalLoanPublic += Convert.ToDecimal(contractReader("con_amount"))
                End If
            End While
            contractReader.Close()

            ' แสดงยอดเงินกู้ใน Guna2TextBox ที่เกี่ยวข้อง
            txtLoanAccount1.Text = totalLoanAccount1.ToString("N2")
            txtLoanSaving.Text = totalLoanSaving.ToString("N2")
            txtLoanPublic.Text = totalLoanPublic.ToString("N2")

            ' คำนวณยอดรวมของเงินกู้
            Dim totalLoan As Decimal = totalLoanAccount1 + totalLoanSaving + totalLoanPublic
            txtTotalLoan.Text = totalLoan.ToString("N2")

            ' คำนวณยอดคงเหลือแยกตามบัญชี
            Dim totalRemainingBalance As Decimal = 0 ' ตัวแปรสำหรับเก็บยอดรวมของยอดคงเหลือทั้ง 3 บัญชี

            Dim queryRemainingBalance As String = "SELECT c.acc_id, SUM(p.payment_prin) AS total_principal " &
                                                  "FROM Payment p, Contract c, Member m " &
                                                  "WHERE p.con_id = c.con_id AND c.m_id = m.m_id AND m.m_name = @memberName " &
                                                  "GROUP BY c.acc_id"
            Dim cmdRemainingBalance As New OleDbCommand(queryRemainingBalance, Conn)
            cmdRemainingBalance.Parameters.AddWithValue("@memberName", memberName)
            Dim reader As OleDbDataReader = cmdRemainingBalance.ExecuteReader()

            While reader.Read()
                Dim accId As String = reader("acc_id").ToString()
                Dim totalPrincipal As Decimal = Convert.ToDecimal(reader("total_principal"))

                ' แสดงยอดคงเหลือตามบัญชี และเพิ่มเข้าไปในยอดรวมทั้งหมด
                If accId = "ACC001" Then
                    txtRemainingBalanceAcc1.Text = totalPrincipal.ToString("N2") ' ยอดคงเหลือสำหรับบัญชี ACC001
                ElseIf accId = "ACC002" Then
                    txtRemainingBalanceAcc2.Text = totalPrincipal.ToString("N2") ' ยอดคงเหลือสำหรับบัญชี ACC002
                ElseIf accId = "ACC003" Then
                    txtRemainingBalanceAcc3.Text = totalPrincipal.ToString("N2") ' ยอดคงเหลือสำหรับบัญชี ACC003
                End If

                ' เพิ่มยอดคงเหลือของบัญชีปัจจุบันไปยังยอดรวมทั้งหมด
                totalRemainingBalance += totalPrincipal
            End While

            reader.Close()

            ' แสดงยอดรวมของยอดคงเหลือทั้ง 3 บัญชี
            txtTotalRemainingBalance.Text = totalRemainingBalance.ToString("N2")

        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการดึงข้อมูล: " & ex.Message, "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Conn.Close()
        End Try
    End Sub



End Class