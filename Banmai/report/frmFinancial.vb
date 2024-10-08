Imports System.Data
Imports System.Data.OleDb
Imports System.Text
Imports Microsoft.Reporting.WinForms

Public Class frmFinancial

    Private Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")

    Private Sub frmFinancial_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' ระบุเส้นทางไฟล์รายงาน .rdlc
        ' Me.ReportViewer1.LocalReport.ReportPath = "D:\Project-2022\Banmai\Banmai\report\Financial.rdlc"

        ' โหลดข้อมูลบัญชีใน ComboBox
        LoadAccountNames()
    End Sub

    Private Sub LoadAccountNames()
        'Dim accountQuery As String = "SELECT acc_id, acc_name FROM Account" ' ดึงข้อมูลจากตาราง Account
        'Dim accountAdapter As New OleDbDataAdapter(accountQuery, Conn)
        'Dim accountTable As New DataTable()

        'Try
        '    Conn.Open()
        '    accountAdapter.Fill(accountTable)

        '    cmbAccountName.DataSource = accountTable
        '    cmbAccountName.DisplayMember = "acc_name" ' แสดงชื่อบัญชี
        '    cmbAccountName.ValueMember = "acc_id" ' ใช้ acc_id เป็นค่าที่แทน
        'Catch ex As Exception
        '    MessageBox.Show("เกิดข้อผิดพลาดในการโหลดชื่อบัญชี: " & ex.Message)
        'Finally
        '    Conn.Close()
        'End Try

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

    Private Sub LoadReport()
        Dim ds As New DataSet()

        ' ไม่ใช้เงื่อนไขกรอง
        Dim incomeQuery As String = "SELECT * FROM income"
        Dim incomeDetailsQuery As String = "SELECT * FROM income_details"
        Dim expenseQuery As String = "SELECT * FROM expense"
        Dim expenseDetailsQuery As String = "SELECT * FROM expense_details"

        Dim incomeAdapter As New OleDbDataAdapter(incomeQuery, Conn)
        Dim incomeDetailsAdapter As New OleDbDataAdapter(incomeDetailsQuery, Conn)
        Dim expenseAdapter As New OleDbDataAdapter(expenseQuery, Conn)
        Dim expenseDetailsAdapter As New OleDbDataAdapter(expenseDetailsQuery, Conn)

        Try
            Conn.Open()
            incomeAdapter.Fill(ds, "Income")
            incomeDetailsAdapter.Fill(ds, "IncomeDetails")
            expenseAdapter.Fill(ds, "Expense")
            expenseDetailsAdapter.Fill(ds, "ExpenseDetails")

            ' สร้าง DataSources สำหรับ ReportViewer
            Dim incomeDataSource As New ReportDataSource("IncomeDataSet", ds.Tables("Income"))
            Dim incomeDetailsDataSource As New ReportDataSource("IncomeDetailsDataSet", ds.Tables("IncomeDetails"))
            Dim expenseDataSource As New ReportDataSource("ExpenseDataSet", ds.Tables("Expense"))
            Dim expenseDetailsDataSource As New ReportDataSource("ExpenseDetailsDataSet", ds.Tables("ExpenseDetails"))

            ' เพิ่ม DataSources ลงใน ReportViewer
            Me.ReportViewer1.LocalReport.DataSources.Clear()
            Me.ReportViewer1.LocalReport.DataSources.Add(incomeDataSource)
            Me.ReportViewer1.LocalReport.DataSources.Add(incomeDetailsDataSource)
            Me.ReportViewer1.LocalReport.DataSources.Add(expenseDataSource)
            Me.ReportViewer1.LocalReport.DataSources.Add(expenseDetailsDataSource)

            ' โหลดและแสดงผลรายงาน
            Me.ReportViewer1.RefreshReport()
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาดในการโหลดรายงาน: " & ex.Message)
        Finally
            Conn.Close()
        End Try
    End Sub

    Private Sub LoadFinancialReport()

        Try
            Dim CmdIncExp As New OleDbCommand
            Dim drIncExp As OleDbDataReader

            ' เปิดการเชื่อมต่อฐานข้อมูล
            Conn.Open()
            ' กำหนดไฟล์ RDLC ที่จะใช้ โดยใช้เส้นทางแบบสัมบูรณ์
            Me.ReportViewer1.LocalReport.ReportPath = "D:\Project-2022\Banmai\Banmai\report\Financial.rdlc"

            Dim para_accid As String = CType(cmbAccountName.SelectedItem, KeyValuePair(Of String, String)).Key
            'Dim para_accid As String = "ACC002"
            Dim para_dat1 As String = ConvertDateMsAccess(dtpStartDate.Value, False, "yyyy-MM-dd")
            Dim para_dat2 As String = ConvertDateMsAccess(dtpEndDate.Value, False, "yyyy-MM-dd")

            ' สร้าง DataSet สำหรับรายงาน
            Dim ds As New DataSet("dsFinancialSummary")

            ' Query Inc + Exp
            Dim dt As New DataTable("dtfinancialsummary")

            With dt.Columns
                .Add("acc_id", GetType(String))
                .Add("acc_name", GetType(String))
                .Add("CshAmt", GetType(Decimal))
                .Add("BnkAmt", GetType(Decimal))
                .Add("PayAmt", GetType(Decimal))
                .Add("TagAmt", GetType(Decimal))
                .Add("OtherAmt", GetType(Decimal))
                .Add("FinTotal1", GetType(Decimal))
                .Add("LoanAmt", GetType(Decimal))
                .Add("TruSavAmt", GetType(Decimal))
                .Add("CostAmt", GetType(Decimal))
                .Add("RetAmt", GetType(Decimal))
                .Add("HedAmt", GetType(Decimal))
                .Add("CtbAmt", GetType(Decimal))
                .Add("IncExpAmt", GetType(Decimal))
                .Add("FinTotal2", GetType(Decimal))
            End With

            Dim sb As New StringBuilder
            Dim stquery As String
            With sb
                .Remove(0, .Length)
                .Append("select ")
                .Append("f.acc_id,f.acc_name,f.CshAmt,(f.IncAmt - ExpAmt) as BnkAmt,f.PayAmt,f.TagAmt,f.OtherAmt ")
                .Append(",(f.CshAmt + (f.IncAmt - ExpAmt) + f.PayAmt + f.TagAmt + f.OtherAmt) as FinTotal1 ")
                .Append(",(f.IncLoanAmt - f.ExdLoanAmt) as LoanAmt,f.TruSavAmt,f.CostAmt,f.RetAmt,f.HedAmt,f.CtbAmt,(f.IncStmAmt - f.ExpStmAmt) as IncExpAmt ")
                .Append(",((f.IncLoanAmt - f.ExdLoanAmt) + f.TruSavAmt + f.CostAmt + f.RetAmt + f.HedAmt + f.CtbAmt + (f.IncStmAmt - f.ExpStmAmt)) as FinTotal2 ")
                .Append("from ")
                .Append("(select ac.acc_id,ac.acc_name,0.00 as CshAmt,iif(IsNull(ic.IncAmt),0.00,ic.IncAmt) as IncAmt,iif(IsNull(ex.ExpAmt),0.00,ex.ExpAmt) as ExpAmt ")
                '.Append(",(select iif(IsNull(sum(payment_Principal)),0.00,sum(payment_Principal)) as paytotal from payment where date_payment between @para_dat1 and @para_dat2) as PayAmt ")
                .Append(",(select iif(IsNull(sum(payment_Prin)),0.00,sum(payment_Prin)) as paytotal from payment pp inner join contract cc on cc.con_id = pp.con_id where cc.acc_id = @para_accid and cc.con_date between @para_dat1 and @para_dat2) as PayAmt ")
                .Append(",0.00 as TagAmt,0.00 as OtherAmt,iif(IsNull(fu.IncLoanAmt),0.00,fu.IncLoanAmt) as IncLoanAmt,iif(IsNull(eln.ExdLoanAmt),0.00,eln.ExdLoanAmt) as ExdLoanAmt ")
                '.Append(",iif(IsNull(fu.TruSavAmt),0.00,fu.TruSavAmt) as TruSavAmt,iif(IsNull(fu.CostAmt),0.00,fu.CostAmt) as CostAmt,iif(IsNull(fu.RetEarnAmt),0.00,fu.RetEarnAmt) as RetAmt ")
                .Append(",((iif(IsNull(fu.TruSavAmt),0.00,fu.TruSavAmt)) - (select iif(IsNull(sum(exd_amount)),0.00,sum(exd_amount)) from expense_details where acc_id = @para_accid and exd_nameacc = 'สมาชิกลาออก' and exd_date between @para_dat1 and @para_dat2)) as TruSavAmt ")
                .Append(",iif(IsNull(fu.CostAmt),0.00,fu.CostAmt) as CostAmt,iif(IsNull(fu.RetEarnAmt),0.00,fu.RetEarnAmt) as RetAmt ")
                .Append(",iif(IsNull(fu.HedgingAmt),0.00,fu.HedgingAmt) as HedAmt,iif(IsNull(fu.ContribAmt),0.00,fu.ContribAmt) as CtbAmt ")
                .Append(",iif(IsNull(istn.IncStmAmt),0.00,istn.IncStmAmt) as IncStmAmt,iif(IsNull(estn.ExpStmAmt),0.00,estn.ExpStmAmt) as ExpStmAmt ")
                .Append("from (((((account ac ")
                .Append("left join ")
                .Append("(select acc_id,sum(inc_amount) as IncAmt ")
                .Append("from income ")
                .Append("where acc_id = @para_accid and inc_date between @para_dat1 and @para_dat2 ")
                .Append("group by acc_id) as ic on ic.acc_id = ac.acc_id) ")
                .Append("left join ")
                .Append("(select acc_id,sum(ex_amount) as ExpAmt ")
                .Append("from expense ")
                .Append("where acc_id = @para_accid and ex_date between @para_dat1 and @para_dat2 ")
                .Append("group by acc_id) as ex on ex.acc_id = ac.acc_id) ")
                .Append("left join ")
                .Append("(select acc_id ")
                .Append(",sum(iif(ind_accname = 'เงินกู้',ind_amount,0.00)) as IncLoanAmt ")
                .Append(",sum(iif(ind_accname = 'เงินฝากสัจจะ',ind_amount,0.00)) as TruSavAmt ")
                .Append(",sum(iif(acc_id = 'ACC001' and ind_accname = 'ทุนบัญชี1',ind_amount,iif(acc_id = 'ACC003' and ind_accname = 'ทุนบัญชีประชารัฐ',ind_amount,0.00))) as CostAmt ")
                .Append(",sum(iif(ind_accname = 'กาไรสะสม',ind_amount,0.00)) as RetEarnAmt ")
                .Append(",sum(iif(ind_accname = 'เงินประกันความเสี่ยง',ind_amount,0.00)) as HedgingAmt ")
                .Append(",sum(iif(ind_accname = 'เงินสบทบ',ind_amount,0.00)) as ContribAmt ")
                .Append("from income_details ")
                .Append("where acc_id = @para_accid and ind_date between @para_dat1 and @para_dat2 ")
                .Append("group by acc_id ")
                .Append(") as fu on fu.acc_id = ac.acc_id) ")
                .Append("left join ")
                .Append("( ")
                .Append("select acc_id,sum(iif(exd_nameacc = 'เงินกู้',exd_amount,0.00)) as ExdLoanAmt ")
                .Append("from expense_details ")
                .Append("where acc_id = @para_accid and exd_date  between @para_dat1 and @para_dat2 ")
                .Append("group by acc_id ")
                .Append(") as eln on eln.acc_id = ac.acc_id) ")
                .Append("left join ")
                .Append("( ")
                .Append("select acc_id,sum(ind_amount) as IncStmAmt ")
                .Append("from income_details ")
                .Append("where acc_id = @para_accid and ind_date between @para_dat1 and @para_dat2 ")
                .Append("and ind_accname in('เงินกู้','ดอกเบี้ยเงินฝากธนาคาร','ค่าธรรมเนียมแรกเข้า','ค่าปรับ','ค่าเอกสาร','เงินบริจาค','ค่าธรรมเนียม','อื่น ๆ') ")
                .Append("group by acc_id ")
                .Append(") as istn on istn.acc_id = ac.acc_id) ")
                .Append("left join ")
                .Append("( ")
                .Append("select acc_id,sum(exd_amount) as ExpStmAmt ")
                .Append("from expense_details ")
                .Append("where acc_id = @para_accid and exd_date between @para_dat1 and @para_dat2 ")
                .Append("and exd_nameacc in('ค่าเช่าสำนักงาน','เงินสมทบ','เงินประกันความเสี่ยง','ค่าตอบแทน','ค่าจ้าง','เงินกู้','ดอกเบี้ยสัจจะ','ดอกเบี้ยจ่าย','อื่นๆ') ")
                .Append("group by acc_id ")
                .Append(") as estn on estn.acc_id = ac.acc_id ")
                .Append("where ac.acc_id = @para_accid) as f ")
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
            ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("IncFinancialSumDataSet", dt))

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


    ' Event handler สำหรับปุ่ม "สร้างรายงาน"
    Private Sub btnGenerateReport_Click(sender As Object, e As EventArgs) Handles btnGenerateReport.Click
        ' เรียกใช้ฟังก์ชันเพื่อสร้างรายงาน
        'LoadReport()
        LoadFinancialReport()
    End Sub
End Class
