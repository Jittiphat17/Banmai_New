Imports System.Data.OleDb
Imports System.Globalization
Imports System.Text
Imports System.Windows.Controls
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports Banmai.individualDSTableAdapters
Imports Microsoft.Reporting.WinForms

Public Class frmDebtorV2

    Private Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")

    Private Sub frmDebtorV2_Load(sender As Object, e As EventArgs) Handles Me.Load
        LoadAccountNames()
        SettingCbxMonth()
        SettingCbxYear()
    End Sub

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
    Private Sub SettingCbxMonth()
        ' เพิ่มรายการเดือนใน ComboBox1 โดยใช้ชื่อเดือนภาษาไทย
        Dim thaiCulture As New CultureInfo("th-TH")
        For i As Integer = 1 To 12
            drpMonth.Items.Add(thaiCulture.DateTimeFormat.GetMonthName(i))
        Next
        'drpMonth.Items.Add("ทั้งหมด") ' เพิ่มตัวเลือกสำหรับยอดรวมทั้งปี

        ' ตั้งค่าเริ่มต้น
        drpMonth.SelectedIndex = DateTime.Now.Month - 1
    End Sub

    Private Sub SettingCbxYear()
        ' เพิ่มรายการปีใน ComboBox2 (แสดงข้อมูล 5 ปีย้อนหลังในรูปแบบ พ.ศ.)
        'Dim currentYear As Integer = DateTime.Now.Year + 543 ' แปลงเป็น พ.ศ.

        Dim currentYear As Integer = DateTime.Now.Year ' แปลงเป็น ค.ศ.
        For i As Integer = 0 To 4
            drpYear.Items.Add(currentYear - i)
        Next

        ' ตั้งค่าเริ่มต้น
        drpYear.SelectedIndex = 0
    End Sub

    Private Sub btnGenerateReport_Click(sender As Object, e As EventArgs) Handles btnGenerateReport.Click
        ' เรียกใช้ฟังก์ชันเพื่อสร้างรายงาน
        LoadDebtorReport()
    End Sub

    Private Sub LoadDebtorReport()

        Try
            Dim CmdIncExp As New OleDbCommand
            Dim drIncExp As OleDbDataReader

            ' เปิดการเชื่อมต่อฐานข้อมูล
            Conn.Open()
            ' กำหนดไฟล์ RDLC ที่จะใช้ โดยใช้เส้นทางแบบสัมบูรณ์
            Me.ReportViewer1.LocalReport.ReportPath = "D:\Project-2022\Banmai\Banmai\report\IndividualdebtorV2.rdlc"

            Dim para_accid As String = CType(cmbAccountName.SelectedItem, KeyValuePair(Of String, String)).Key
            Dim para_mt As String = (drpMonth.SelectedIndex) + 1
            Dim para_yr As String = drpYear.Text
            Dim para_lastdate As String = para_yr & "-" & CInt(para_mt).ToString("00") & "-01"

            ' สร้าง DataSet สำหรับรายงาน
            Dim ds As New DataSet("dsDebtor")

            ' Query Inc + Exp
            Dim dt As New DataTable("dtDebtor")

            With dt.Columns
                .Add("con_id", GetType(Integer))
                .Add("m_id", GetType(Integer))
                .Add("member_name", GetType(String))
                .Add("BalAmt", GetType(Decimal))
                .Add("NetAmt", GetType(Decimal))
                .Add("PrinAmt", GetType(Decimal))
                .Add("InterAmt", GetType(Decimal))
                .Add("FineAmt", GetType(Decimal))
                .Add("NewAmt", GetType(Decimal))
                .Add("ConfAmt", GetType(Decimal))
                .Add("InsuAmt", GetType(Decimal))
                .Add("EndAmt", GetType(Decimal))
            End With

            Dim sb As New StringBuilder
            Dim stquery As String
            With sb
                .Remove(0, .Length)
                .Append("select ct.con_id,ct.m_id,iif(IsNull(m.m_gender),'',m.m_gender) +  m.m_name as member_name ")
                .Append(",iif(ct.FirstAmt > 0.00 and (ct.PrinAmt + ct.InterAmt + ct.FineAmt + ct.ConfAmt + ct.InsuAmt) = 0.00,0.00,ct.SecAmt) as BalAmt ")
                .Append(",(ct.PrinAmt + ct.InterAmt + ct.FineAmt) as NetAmt,ct.PrinAmt,ct.InterAmt,ct.FineAmt ")
                .Append(",iif(ct.FirstAmt > 0.00 and (ct.PrinAmt + ct.InterAmt + ct.FineAmt + ct.ConfAmt + ct.InsuAmt) = 0.00,ct.FirstAmt,0.00) as NewAmt ")
                .Append(",ct.ConfAmt,ct.InsuAmt ")
                .Append(",((iif(ct.FirstAmt > 0.00 and (ct.PrinAmt + ct.InterAmt + ct.FineAmt + ct.ConfAmt + ct.InsuAmt) = 0.00,0.00,ct.SecAmt) + iif(ct.FirstAmt > 0.00 and (ct.PrinAmt + ct.InterAmt + ct.FineAmt + ct.ConfAmt + ct.InsuAmt) = 0.00,ct.FirstAmt,0.00)) - ct.PrinAmt) as EndAmt ")
                .Append("from ")
                .Append("(select ")
                .Append("c.con_id,c.m_id,c.acc_id ")
                .Append(",iif(IsNull(f.FirstAmt),0.00,f.FirstAmt) as FirstAmt ")
                .Append(",iif(IsNull(s.SecAmt),0.00,s.SecAmt) as SecAmt ")
                .Append(",iif(IsNull(n.PrinAmt),0.00,n.PrinAmt) as PrinAmt ")
                .Append(",iif(IsNull(n.InterAmt),0.00,n.InterAmt) as InterAmt ")
                .Append(",iif(IsNull(n.FineAmt),0.00,n.FineAmt) as FineAmt ")
                .Append(",iif(IsNull(n.ConfAmt),0.00,n.ConfAmt) as ConfAmt ")
                .Append(",iif(IsNull(n.InsuAmt),0.00,n.InsuAmt) as InsuAmt ")
                .Append("from ((contract c ")
                .Append("left join ( ")
                .Append("select con_id,m_id,acc_id,sum(con_amount) as FirstAmt ")
                .Append("from contract ")
                .Append("where acc_id = @para_accid and month(con_date) = @para_mt and year(con_date) = @para_yr ")
                .Append("group by con_id,m_id,acc_id ")
                .Append(")as f on f.con_id = c.con_id and f.m_id = c.m_id and f.acc_id = c.acc_id) ")
                .Append("left join ( ")
                .Append("select con_id,m_id,acc_id ")
                .Append(",sum(iif(ind_accname = 'เงินต้น',ind_amount,0.00)) as PrinAmt ")
                .Append(",sum(iif(ind_accname = 'ดอกเบี้ย',ind_amount,0.00)) as InterAmt ")
                .Append(",sum(iif(ind_accname = 'ค่าปรับ',ind_amount,0.00)) as FineAmt ")
                .Append(",sum(iif(ind_accname = 'ค่าสัญญา',ind_amount,0.00)) as ConfAmt ")
                .Append(",sum(iif(ind_accname = 'ค่าประกัน',ind_amount,0.00)) as InsuAmt ")
                .Append("from income_details ")
                .Append("where acc_id = @para_accid and month(ind_date) = @para_mt and year(ind_date) = @para_yr ")
                .Append("group by con_id,m_id,acc_id ")
                .Append(") as n on n.con_id = c.con_id and n.m_id = c.m_id and n.acc_id = c.acc_id) ")
                .Append("left join ( ")
                .Append("select x.con_id,x.m_id,x.acc_id,sum(x.amount) as SecAmt ")
                .Append("from ")
                .Append("(select c.con_id,c.m_id,c.acc_id,c.con_amount as amount ")
                .Append("from contract c ")
                .Append("where c.acc_id = @para_accid and c.con_date < @para_lastdate ")
                .Append("union all ")
                .Append("select s.con_id,s.m_id,s.acc_id,(s.ind_amount * -1) as amount ")
                .Append("from income_details s ")
                .Append("where s.acc_id = @para_accid and s.ind_date < @para_lastdate ")
                .Append("and s.ind_accname ='เงินต้น') as x ")
                .Append("group by x.con_id,x.m_id,x.acc_id ")
                .Append(") as s on s.con_id = c.con_id and s.m_id = c.m_id and s.acc_id = c.acc_id ")
                .Append("where c.acc_id = @para_accid ")
                .Append(") as ct ")
                .Append("inner join member m on m.m_id = ct.m_id ")
                .Append("order by ct.m_id ")
                stquery = .ToString()
            End With

            With CmdIncExp
                .Parameters.Clear()
                .Parameters.AddWithValue("@para_accid", para_accid)
                .Parameters.AddWithValue("@para_mt", para_mt)
                .Parameters.AddWithValue("@para_yr", para_yr)
                .Parameters.AddWithValue("@para_lastdate", para_lastdate)
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
            ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("DebtorDataSet", dt))

            ' ส่งค่า Parameter ชื่อบัญชีและช่วงวันที่ไปยังรายงาน RDLC
            Dim txt_titledate As String = "ประจำงวดที่ " & CInt(para_mt).ToString("00") & "/" & para_yr
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