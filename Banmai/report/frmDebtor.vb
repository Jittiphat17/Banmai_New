Imports System.Data.OleDb
Imports System.Globalization
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports Banmai.individualDSTableAdapters
Imports Microsoft.Reporting.WinForms

Public Class frmDebtor
    Private ReadOnly connString As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb"

    ' เพิ่ม DataSet เพื่อเก็บข้อมูล
    Private incomeDataSet As New DataSet()
    Private memberDataSet As New DataSet()

    Private Sub frmDebtor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'Db_banmai1DataSet.Account' table. You can move, or remove it, as needed.
        Me.AccountTableAdapter.Fill(Me.Db_banmai1DataSet.Account)
        ' โหลดข้อมูลสำหรับรายงาน
        initialOnload()
        ' LoadReportData()
        LoadReportData2()
    End Sub
    Private Sub LoadReportData()
        Dim month As Integer = Integer.Parse(drpMonth.SelectedValue.ToString())
        Dim year As Integer = Integer.Parse(drpYear.SelectedItem.ToString())

        ' Get the first and last date of the month
        Dim firstDate As New DateTime()
        Dim lastDate As New DateTime()

        If drpMonth.SelectedValue = 0 Then
            Dim firstDateYear As New DateTime(year, 1, 1)
            Dim lastDateYear As New DateTime(year, 12, DateTime.DaysInMonth(year, 12))
            firstDate = firstDateYear
            lastDate = lastDateYear
        Else
            firstDate = New DateTime(year, month, 1)
            lastDate = New DateTime(year, month, DateTime.DaysInMonth(year, month))
        End If



        Try
            Using conn As New OleDbConnection(connString)
                conn.Open()


                'month -1
                Dim beforeDS As New individualDS()
                Dim beforeAdapter As New DataTable1TableAdapter()
                beforeAdapter.filterdate(beforeDS.DataTable1, firstDate.AddMonths(-1), lastDate.AddMonths(-1), ComboBox1.SelectedValue)
                Dim reportDataSourceBefore As New ReportDataSource("before1MonthDS", beforeDS.Tables(0))


                'Contract
                Dim contDS As New individualDS()
                Dim contAdapter As New dtContractTableAdapter()
                contAdapter.filterContract(contDS.dtContract, firstDate, lastDate, ComboBox1.SelectedValue)
                Dim reportDataSourceCont As New ReportDataSource("contDS", contDS.Tables(0))


                'Search
                Dim ds As New individualDS()
                Dim adapter As New DataTable1TableAdapter()
                adapter.Fill(ds.DataTable1)

                'adapter.filterdate(ds.DataTable1, firstDate, lastDate, ComboBox1.SelectedValue)
                Dim reportDataSource As New ReportDataSource("individualDS", ds.Tables(0))

                Dim dtcont As DataTable = contDS.Tables("dtContract")
                Dim dt As DataTable = ds.Tables("DataTable1")

                'รวม Object กู้ใหม่กับ Payment
                dt.Merge(dtcont)

                ReportViewer1.LocalReport.DataSources.Clear()
                ReportViewer1.LocalReport.DataSources.Add(reportDataSourceBefore)
                ReportViewer1.LocalReport.DataSources.Add(reportDataSource)



                ' ตรวจสอบว่าไฟล์รายงานมีอยู่หรือไม่
                Dim reportPath As String = $"D:\Project-2022\Banmai\Banmai\report\Individualdebtor.rdlc"
                ReportViewer1.ProcessingMode = ProcessingMode.Local

                If Not IO.File.Exists(reportPath) Then
                    MessageBox.Show($"ไม่พบไฟล์รายงานที่: {reportPath}")
                    Return
                End If

                '' ตั้งค่าเส้นทางของรายงาน
                Me.ReportViewer1.LocalReport.ReportPath = reportPath


                ' Create the parameters to pass to the report
                Dim parameters As New List(Of ReportParameter)()
                parameters.Add(New ReportParameter("SelectedMonth", month))
                parameters.Add(New ReportParameter("SelectedYear", year))

                ' Set the parameters for the report
                ReportViewer1.LocalReport.SetParameters(parameters)

                ' ทำการ Refresh รายงาน
                Me.ReportViewer1.RefreshReport()
            End Using
        Catch ex As Exception
            MessageBox.Show($"เกิดข้อผิดพลาด: {ex.Message}")
        End Try
    End Sub

    Private Sub initialOnload()
        ComboBox1.SelectedValue = "ACC002"


        ' เพิ่มรายการเดือนใน ComboBox1 โดยใช้ชื่อเดือนภาษาไทย
        Dim thaiCulture As New CultureInfo("th-TH")
        For i As Integer = 1 To 12
            drpMonth.Items.Add(thaiCulture.DateTimeFormat.GetMonthName(i))
        Next
        drpMonth.Items.Add("ทั้งหมด") ' เพิ่มตัวเลือกสำหรับยอดรวมทั้งปี

        ' เพิ่มรายการปีใน ComboBox2 (แสดงข้อมูล 5 ปีย้อนหลังในรูปแบบ พ.ศ.)
        Dim currentYear As Integer = DateTime.Now.Year + 543 ' แปลงเป็น พ.ศ.
        For i As Integer = 0 To 4
            drpYear.Items.Add(currentYear - i)
        Next

        ' ตั้งค่าเริ่มต้น
        drpMonth.SelectedIndex = DateTime.Now.Month - 1
        drpYear.SelectedIndex = 0


    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        LoadReportData2()
    End Sub


    Private Sub LoadReportData2()
        Dim selectedMonth As Integer = drpMonth.SelectedIndex + 1 ' Convert.ToInt32(drpMonth.SelectedValue)
        Dim selectedYear As Integer = 2567
        Dim all As String = drpMonth.SelectedItem.ToString() '"ทั้งหมด"
        Try
            Using conn As New OleDbConnection(connString)
                conn.Open()


                ' สร้าง DataSet แรกสำหรับข้อมูลสมาชิก
                Dim ds1 As New DataSet()
                Using adapter1 As New OleDbDataAdapter("SELECT m_id, m_name, m_address, m_tel FROM Member", conn)
                    adapter1.Fill(ds1, "MemberDataSet")
                End Using

                ' สร้าง DataSet ที่สองสำหรับข้อมูลเงินฝากสัจจะเฉพาะเดือนและปีที่เลือก หรือยอดรวมทั้งปี
                Dim ds2 As New DataSet()
                Dim query As String
                Using adapter2 As New OleDbDataAdapter()

                    'If ComboBox1.SelectedItem.ToString() = "ทั้งหมด" Then
                    If all = "ทั้งหมด" Then
                        ' Query สำหรับยอดรวมทั้งปี


                        query = "SELECT d.m_id, SUM(d.ind_amount) AS ind_amount, d.ind_accname , d.con_id, d.acc_id " &
                            ", 0 as ind_amount_interest , 0 as ind_amount_fine , 0 as ind_con_amount , 0 as ind_insure_amount , 0 as first_amount, 0 as total_amount, 0 as pay_amount , 0 as sum_con_amount " &
                                "FROM Income_Details d " &
                                "WHERE YEAR(d.ind_date) = @Year " &
                                "GROUP BY d.m_id, d.ind_accname, d.con_id, d.acc_id"

                        ' "WHERE d.ind_accname = 'เงินต้น' AND YEAR(d.ind_date) = @Year " &
                        adapter2.SelectCommand = New OleDbCommand(query, conn)
                        adapter2.SelectCommand.Parameters.AddWithValue("@Year", selectedYear - 543) ' แปลงเป็น ค.ศ.
                    Else
                        ' Query สำหรับเดือนและปีที่เลือก
                        query = "SELECT d.m_id, SUM(d.ind_amount) AS ind_amount, d.ind_accname , d.con_id, d.acc_id " &
                            ", 0 as ind_amount_interest , 0 as ind_amount_fine , 0 as ind_con_amount , 0 as ind_insure_amount , 0 as first_amount, 0 as total_amount, 0 as pay_amount, 0 as sum_con_amount " &
                                "FROM Income_Details d " &
                                "WHERE MONTH(d.ind_date) = @Month AND YEAR(d.ind_date) = @Year " &
                                "GROUP BY d.m_id, d.ind_accname, d.con_id, d.acc_id"
                        adapter2.SelectCommand = New OleDbCommand(query, conn)
                        adapter2.SelectCommand.Parameters.AddWithValue("@Month", selectedMonth)
                        adapter2.SelectCommand.Parameters.AddWithValue("@Year", selectedYear - 543) ' แปลงเป็น ค.ศ.
                    End If


                    'Dim allTranaction As DataRow = ds2.Tables("IncomeDataSet").datar

                    adapter2.Fill(ds2, "IncomeDataSet")

                    ' Debug: แสดงข้อมูลที่ถูกดึงมา
                    'Debug.WriteLine($"จำนวนรายการที่พบ: {ds2.Tables("IncomeDataSet").Rows.Count}")
                    'For Each row As DataRow In ds2.Tables("IncomeDataSet").Rows
                    '    Debug.WriteLine($"m_id: {row("m_id")}, จำนวนเงินฝากสัจจะ: {row("ind_amount")}")
                    'Next
                End Using

                ' สร้าง DataSet สำหรับข้อมูลสมาชิก Contract
                Dim ds3 As New DataSet()
                Using adapter3 As New OleDbDataAdapter()
                    Dim conQuery = "SELECT con_id, m_id, acc_id, con_amount FROM Contract WHERE YEAR(con_date) = @Year " ' AND m_id not in (select m_id  FROM Income_Details WHERE ind_accname not in ('เงินต้น') ) "
                    If all <> "ทั้งหมด" Then
                        '   conQuery += "AND MONTH(con_date) = @Month "
                    End If


                    '("SELECT con_id, m_id, acc_id, con_amount FROM Contract WHERE YEAR(con_date) = @Year AND m_id not in (select m_id  FROM Income_Details WHERE ind_accname not in ('ค่าสัญญา','ค่าประกัน') ) ", conn)

                    adapter3.SelectCommand = New OleDbCommand(conQuery, conn)
                    ' adapter3.SelectCommand.Parameters.AddWithValue("@Month", selectedMonth)
                    adapter3.SelectCommand.Parameters.AddWithValue("@Year", selectedYear - 543) ' แปลงเป็น ค.ศ. 
                    adapter3.Fill(ds3, "ContractDataSet")

                End Using



                ' สร้างเงินต้น DataSet เฉพาะเดือนและปีที่เลือก หรือยอดรวมทั้งปี
                Dim bfDs As New DataSet()
                Dim sumContract As Decimal = 0
                Using bfAdapter2 As New OleDbDataAdapter()
                    Dim bfQuery As String

                    'If ComboBox1.SelectedItem.ToString() = "ทั้งหมด" Then
                    If all = "ทั้งหมด" Then
                        ' Query สำหรับยอดรวมทั้งปี


                        bfQuery = "SELECT d.m_id, SUM(d.ind_amount) AS ind_amount, d.ind_accname , d.con_id, d.acc_id " &
                                "FROM Income_Details d " &
                                "WHERE YEAR(d.ind_date) = @Year AND d.ind_accname = 'เงินต้น' " &
                                "GROUP BY d.m_id, d.ind_accname, d.con_id, d.acc_id"

                        ' "WHERE d.ind_accname = 'เงินต้น' AND YEAR(d.ind_date) = @Year " &
                        bfAdapter2.SelectCommand = New OleDbCommand(bfQuery, conn)
                        bfAdapter2.SelectCommand.Parameters.AddWithValue("@Year", selectedYear - 544) ' แปลงเป็น ค.ศ.
                    Else
                        ' Query สำหรับเดือนและปีที่เลือก
                        bfQuery = "SELECT d.m_id, SUM(d.ind_amount) AS ind_amount, d.ind_accname, d.con_id, d.acc_id " &
                                "FROM Income_Details d " &
                                "WHERE d.ind_accname = 'เงินต้น' AND MONTH(d.ind_date) = @Month AND YEAR(d.ind_date) = @Year " &
                                "GROUP BY d.m_id, d.ind_accname, d.con_id, d.acc_id"
                        bfAdapter2.SelectCommand = New OleDbCommand(bfQuery, conn)
                        bfAdapter2.SelectCommand.Parameters.AddWithValue("@Month", selectedMonth - 1)
                        bfAdapter2.SelectCommand.Parameters.AddWithValue("@Year", selectedYear - 543) ' แปลงเป็น ค.ศ.
                    End If

                    bfAdapter2.Fill(bfDs, "IncomeDataSet")
                End Using

                ' เพิ่มข้อมูลสำหรับสมาชิกที่ไม่มีรายการฝากในเดือนที่เลือก หรือทั้งปี
                For Each memberRow As DataRow In ds1.Tables("MemberDataSet").Rows
                    Dim memberID As String = memberRow("m_id").ToString()
                    '#####################3
                    Dim incomeRows As DataRow() = ds2.Tables("IncomeDataSet").Select($"m_id = '{memberID}'")
                    Dim incomeBfRows As DataRow() = bfDs.Tables("IncomeDataSet").Select($"m_id = '{memberID}'")
                    Dim contractRows As DataRow() = ds3.Tables("ContractDataSet").Select($"m_id = '{memberID}'")
                    '################

                    'Dim incomeRowsPrincipal As DataRow() = incomeRows.Where(t) $"ind_accname = '{"เงินต้น"}'")
                    Dim incomeRowsPrincipal As DataRow() = incomeRows.
        Where(Function(t) t("ind_accname").ToString() = "เงินต้น").ToArray()
                    Dim incomeRowInterestl As DataRow() = incomeRows.
        Where(Function(t) t("ind_accname").ToString() = "ดอกเบี้ย").ToArray()
                    Dim incomeRowFine As DataRow() = incomeRows.
        Where(Function(t) t("ind_accname").ToString() = "ค่าปรับ").ToArray()
                    Dim incomeRowCont As DataRow() = incomeRows.
        Where(Function(t) t("ind_accname").ToString() = "ค่าสัญญา").ToArray()
                    Dim incomeRowInsure As DataRow() = incomeRows.
        Where(Function(t) t("ind_accname").ToString() = "ค่าประกัน").ToArray()

                    Dim principalAmount As Decimal = If(incomeRowsPrincipal.FirstOrDefault() IsNot Nothing, incomeRowsPrincipal.FirstOrDefault().Field(Of Decimal)("ind_amount"), 0)
                    Dim interestAmount As Decimal = If(incomeRowInterestl.FirstOrDefault() IsNot Nothing, incomeRowInterestl.FirstOrDefault().Field(Of Decimal)("ind_amount"), 0)
                    Dim fineAmount As Decimal = If(incomeRowFine.FirstOrDefault() IsNot Nothing, incomeRowFine.FirstOrDefault().Field(Of Decimal)("ind_amount"), 0)
                    Dim contAmount As Decimal = If(incomeRowCont.FirstOrDefault() IsNot Nothing, incomeRowCont.FirstOrDefault().Field(Of Decimal)("ind_amount"), 0)
                    Dim insureAmount As Decimal = If(incomeRowInsure.FirstOrDefault() IsNot Nothing, incomeRowInsure.FirstOrDefault().Field(Of Decimal)("ind_amount"), 0)


                    Dim conAmount As Decimal = If(contractRows.FirstOrDefault() IsNot Nothing, contractRows.FirstOrDefault().Field(Of Decimal)("con_amount"), 0)

                    If principalAmount = 0 Then
                        sumContract += conAmount 'sum
                    End If


                    If incomeRows.Length > 0 Then
                        For Each incomeRow As DataRow In incomeRows


                            'Todo change logic
                            Dim firstAmount As Decimal = 10000 'contAmount - SUM (ind_amount)
                            'Dim totalAmount As Decimal = 10000 'contAmount - SUM (ind_amount)

                            ' Update the respective columns for the current incomeRow
                            incomeRow("ind_amount") = principalAmount
                            incomeRow("ind_amount_interest") = interestAmount
                            incomeRow("ind_amount_fine") = fineAmount

                            incomeRow("ind_con_amount") = contAmount
                            incomeRow("ind_insure_amount") = insureAmount

                            'incomeRow("first_amount") = test

                            incomeRow("first_amount") = firstAmount
                            incomeRow("total_amount") = firstAmount - principalAmount

                            incomeRow("sum_con_amount") = sumContract

                            '  Before Not working
                            'If(incomeBfRows.FirstOrDefault() IsNot Nothing, incomeBfRows.FirstOrDefault().Field(Of Decimal)("ind_amount"), firstAmount)


                            Debug.WriteLine($"แก้ไขข้อมูลสำหรับสมาชิก m_id: {memberID} จำนวนใหม่คือ {incomeRow("ind_amount")}")
                        Next

                    End If



                    If incomeRows.Length = 0 Then
                        Dim newRow As DataRow = ds2.Tables("IncomeDataSet").NewRow()
                        newRow("m_id") = memberID
                        newRow("ind_amount") = 0
                        '  newRow("ind_accname") = ""
                        ds2.Tables("IncomeDataSet").Rows.Add(newRow)
                        Debug.WriteLine($"เพิ่มข้อมูลสำหรับสมาชิก m_id: {memberID} ที่ไม่มีรายการฝาก")
                    End If
                Next





                ' ตั้งค่าเส้นทางรายงาน
                Dim reportPath As String = $"D:\Test\Banmai\Banmai\report\Individualdebtor.rdlc" ' "D:\Project-2022\Banmai\Banmai\report\SajjaReport.rdlc"

                If Not IO.File.Exists(reportPath) Then
                    MessageBox.Show($"ไม่พบไฟล์รายงาน: {reportPath}")
                    Return
                End If

                Me.ReportViewer1.LocalReport.ReportPath = reportPath

                ' ผูกข้อมูลกับ ReportViewer
                Me.ReportViewer1.LocalReport.DataSources.Clear()
                Me.ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("MemberDataSet", ds1.Tables("MemberDataSet")))
                Me.ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("IncomeDataSet", ds2.Tables("IncomeDataSet")))
                Me.ReportViewer1.LocalReport.DataSources.Add(New ReportDataSource("ContractDataSet", ds3.Tables("ContractDataSet")))

                ' เพิ่มพารามิเตอร์เพื่อแสดงวัน เดือน ปี พ.ศ. ในรายงาน
                Dim thaiCulture As New CultureInfo("th-TH")
                Dim monthName As String = If(ComboBox1.SelectedItem.ToString() = "ทั้งหมด", "ทั้งหมด", thaiCulture.DateTimeFormat.GetMonthName(selectedMonth))
                Dim reportDate As String = $"{DateTime.Now.Day} {monthName} {selectedYear}" ' วันที่ เดือน ปี พ.ศ.

                Dim reportParams As New List(Of ReportParameter) From {
                    New ReportParameter("ReportDate", reportDate)
                }

                Me.ReportViewer1.LocalReport.SetParameters(reportParams)

                ' Refresh รายงาน
                Me.ReportViewer1.RefreshReport()
            End Using
        Catch ex As Exception
            MessageBox.Show($"เกิดข้อผิดพลาด: {ex.Message}")
            Debug.WriteLine($"ข้อผิดพลาด: {ex.ToString()}")
        End Try
    End Sub
End Class
