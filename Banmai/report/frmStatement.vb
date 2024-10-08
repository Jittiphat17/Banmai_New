Imports Microsoft.Reporting.WinForms
Imports System.Data.OleDb
Imports System.Runtime.InteropServices.ComTypes
Imports System.Text

Public Class frmStatement

    ' สร้างการเชื่อมต่อกับฐานข้อมูล
    Private Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")

    Private Sub frmProfitLossReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' ดึงข้อมูลบัญชีเพื่อลงใน ComboBox
        LoadAccountNames()
    End Sub

    ' ฟังก์ชันสำหรับโหลดข้อมูลบัญชีลงใน ComboBox
    Private Sub LoadAccountNames()
        Try
            Conn.Open()

            ' ดึงชื่อบัญชีจากตาราง Account
            Dim query As String = "SELECT acc_id, acc_name FROM Account"
            Dim cmd As New OleDbCommand(query, Conn)

            Dim reader As OleDbDataReader = cmd.ExecuteReader()

            ' เติมข้อมูลใน ComboBox
            cmbAccountName.Items.Clear()
            While reader.Read()
                cmbAccountName.Items.Add(New KeyValuePair(Of String, String)(reader("acc_id").ToString(), reader("acc_name").ToString()))
            End While

            reader.Close()
            Conn.Close()
        Catch ex As Exception
            MessageBox.Show("Error loading account names: " & ex.Message)
        Finally
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try
    End Sub

    Private Sub btnGenerateReport_Click(sender As Object, e As EventArgs) Handles btnGenerateReport.Click
        ' ตรวจสอบว่าผู้ใช้ได้เลือกบัญชีแล้วหรือยัง
        If cmbAccountName.SelectedIndex <> -1 Then
            ' ตรวจสอบว่าช่วงวันที่ถูกต้องหรือไม่
            If dtpEndDate.Value >= dtpStartDate.Value Then
                btnGenerateReport.Enabled = False ' Disable the button to avoid multiple clicks
                ' LoadProfitLossReport() ' ดึงข้อมูลและแสดงรายงาน
                LoadProfitLossReportV2() ' ดึงข้อมูลและแสดงรายงาน
                btnGenerateReport.Enabled = True ' Re-enable the button when done
            Else
                MessageBox.Show("กรุณาเลือกช่วงวันที่ให้ถูกต้อง", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Else
            MessageBox.Show("กรุณาเลือกบัญชีก่อน", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub LoadProfitLossReportV2()

        Try
            Dim CmdIncExp As New OleDbCommand
            Dim drIncExp As OleDbDataReader

            ' เปิดการเชื่อมต่อฐานข้อมูล
            Conn.Open()
            ' กำหนดไฟล์ RDLC ที่จะใช้ โดยใช้เส้นทางแบบสัมบูรณ์
            Me.ReportViewer1.LocalReport.ReportPath = "D:\Project-2022\Banmai\Banmai\report\StatementReport.rdlc"

            Dim para_accid As String = CType(cmbAccountName.SelectedItem, KeyValuePair(Of String, String)).Key
            Dim para_dat1 As String = ConvertDateMsAccess(dtpStartDate.Value, False, "yyyy-MM-dd")
            Dim para_dat2 As String = ConvertDateMsAccess(dtpEndDate.Value, False, "yyyy-MM-dd")

            ' สร้าง DataSet สำหรับรายงาน
            Dim ds As New DataSet("dsIncExpSummary")

            ' Query Inc + Exp
            Dim dt As New DataTable("dtincexpsummary")

            With dt.Columns
                .Add("acc_id", GetType(String))
                .Add("acc_name", GetType(String))
                .Add("IncAmt1", GetType(Decimal))
                .Add("IncAmt2", GetType(Decimal))
                .Add("IncAmt3", GetType(Decimal))
                .Add("IncAmt4", GetType(Decimal))
                .Add("IncAmt5", GetType(Decimal))
                .Add("IncAmt6", GetType(Decimal))
                .Add("IncAmt7", GetType(Decimal))
                .Add("IncAmt8", GetType(Decimal))
                .Add("ExpAmt1", GetType(Decimal))
                .Add("ExpAmt2", GetType(Decimal))
                .Add("ExpAmt3", GetType(Decimal))
                .Add("ExpAmt4", GetType(Decimal))
                .Add("ExpAmt5", GetType(Decimal))
                .Add("ExpAmt6", GetType(Decimal))
                .Add("ExpAmt7", GetType(Decimal))
                .Add("ExpAmt8", GetType(Decimal))
                .Add("ExpAmt9", GetType(Decimal))
            End With

            Dim sb As New StringBuilder
            Dim stquery As String
            With sb
                .Remove(0, .Length)
                .Append("select ac.acc_id, ac.acc_name ")
                .Append(",IIf(IsNull(inc.incamt1),0,inc.incamt1) AS IncAmt1 ")
                .Append(",IIf(IsNull(inc.incamt2),0,inc.incamt2) AS IncAmt2 ")
                .Append(",IIf(IsNull(inc.incamt3),0,inc.incamt3) AS IncAmt3 ")
                .Append(",IIf(IsNull(inc.incamt4),0,inc.incamt4) AS IncAmt4 ")
                .Append(",IIf(IsNull(inc.incamt5),0,inc.incamt5) AS IncAmt5 ")
                .Append(",IIf(IsNull(inc.incamt6),0,inc.incamt6) AS IncAmt6 ")
                .Append(",IIf(IsNull(inc.incamt7),0,inc.incamt7) AS IncAmt7 ")
                .Append(",IIf(IsNull(inc.incamt8),0,inc.incamt8) AS IncAmt8 ")
                .Append(",IIf(IsNull(exp.exdamt1),0,exp.exdamt1) AS ExpAmt1 ")
                .Append(",IIf(IsNull(exp.exdamt2),0,exp.exdamt2) AS ExpAmt2 ")
                .Append(",IIf(IsNull(exp.exdamt3),0,exp.exdamt3) AS ExpAmt3 ")
                .Append(",IIf(IsNull(exp.exdamt4),0,exp.exdamt4) AS ExpAmt4 ")
                .Append(",IIf(IsNull(exp.exdamt5),0,exp.exdamt5) AS ExpAmt5 ")

                .Append(",IIf(IsNull(exp.exdamt7),0,exp.exdamt7) AS ExpAmt7 ")
                .Append(",IIf(IsNull(exp.exdamt8),0,exp.exdamt8) AS ExpAmt8 ")
                .Append(",IIf(IsNull(exp.exdamt9),0,exp.exdamt9) AS ExpAmt9 ")
                .Append("from (account as ac left join (select acc_id ")
                .Append(",sum(iif(ind_accname = 'ดอกเบี้ย',ind_amount,0.00)) as incamt1 ")
                .Append(",sum(iif(ind_accname = 'ดอกเบี้ยเงินฝากธนาคาร',ind_amount,0.00)) as incamt2 ")
                .Append(",sum(iif(ind_accname = 'ค่าธรรมเนียมแรกเข้า',ind_amount,0.00)) as incamt3 ")
                .Append(",sum(iif(ind_accname = 'ค่าปรับ',ind_amount,0.00)) as incamt4 ")
                .Append(",sum(iif(ind_accname = 'ค่าเอกสาร',ind_amount,0.00)) as incamt5 ")
                .Append(",sum(iif(ind_accname = 'เงินบริจาค',ind_amount,0.00)) as incamt6 ")
                .Append(",sum(iif(ind_accname = 'ค่าธรรมเนียม',ind_amount,0.00)) as incamt7 ")
                .Append(",sum(iif(ind_accname = 'อื่น ๆ',ind_amount,0.00)) as incamt8 ")
                .Append("from income_details ")
                .Append("where acc_id = @para_accid and ind_date between @para_dat1 and @para_dat2 ")
                .Append("group by acc_id)  as inc on ac.acc_id = inc.acc_id) LEFT JOIN (select acc_id ")
                .Append(",sum(iif(exd_nameacc = 'ค่าเช่าสำนักงาน',exd_amount,0.00)) as exdamt1 ")
                .Append(",sum(iif(exd_nameacc = 'เงินสมทบ',exd_amount,0.00)) as exdamt2 ")
                .Append(",sum(iif(exd_nameacc = 'เงินประกันความเสี่ยง',exd_amount,0.00)) as exdamt3 ")
                .Append(",sum(iif(exd_nameacc = 'ค่าตอบแทน',exd_amount,0.00)) as exdamt4 ")
                .Append(",sum(iif(exd_nameacc = 'ค่าจ้าง',exd_amount,0.00)) as exdamt5 ")

                .Append(",sum(iif(exd_nameacc = 'ดอกเบี้ยสัจจะ',exd_amount,0.00)) as exdamt7 ")
                .Append(",sum(iif(exd_nameacc = 'ดอกเบี้ยจ่าย',exd_amount,0.00)) as exdamt8 ")
                .Append(",sum(iif(exd_nameacc = 'อื่นๆ',exd_amount,0.00)) as exdamt9 ")
                .Append("from expense_details ")
                .Append("where acc_id = @para_accid and exd_date between @para_dat1 and @para_dat2 ")
                .Append("group by acc_id)  AS exp on ac.acc_id = exp.acc_id ")
                .Append("where (((ac.acc_id)= @para_accid)); ")
                stquery = .ToString()
            End With

            With CmdIncExp
                .Parameters.Clear()
                .Parameters.AddWithValue("@para_accid", para_accid)
                .Parameters.AddWithValue("@para_dat1", para_dat1 & " 00:00:00")
                .Parameters.AddWithValue("@para_dat2", para_dat2 & " 23:59:59")
                .CommandText = stquery
                .CommandType = CommandType.Text
                .Connection = Conn
                drIncExp = .ExecuteReader()
            End With

            If drIncExp.HasRows Then
                dt.Load(drIncExp)
            End If

            drIncExp.Close()

            ' ตั้งค่ารายงาน RDLC
            ReportViewer1.LocalReport.DataSources.Clear()

            ' ผูก DataSet กับรายงาน
            ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("IncExpSumDataSet", dt))

            ' ส่งค่า Parameter ชื่อบัญชีและช่วงวันที่ไปยังรายงาน RDLC
            Dim txt_titledate As String = "ณ วันที่ " & ConvertStringDateThai(para_dat1) & " ถึงวันที่ " & ConvertStringDateThai(para_dat2)
            Dim p_titledate As New ReportParameter("p_titledate", txt_titledate)
            ReportViewer1.LocalReport.SetParameters(New ReportParameter() {p_titledate})

            ' Refresh รายงาน
            ReportViewer1.RefreshReport()

            ' ปิดการเชื่อมต่อฐานข้อมูล
            Conn.Close()

        Catch ex As Exception
            ' แสดงข้อความข้อผิดพลาดถ้าเกิดปัญหา
            MessageBox.Show("Error loading profit-loss report: " & ex.Message)
        Finally
            ' ตรวจสอบว่าฐานข้อมูลถูกปิดอย่างถูกต้องหรือไม่
            If Conn.State = ConnectionState.Open Then
                Conn.Close()
            End If
        End Try

    End Sub

End Class