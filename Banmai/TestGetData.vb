Imports System.Data.OleDb

Public Class TestGetData
    Public Property ContractId As String ' Property สำหรับเก็บค่า conId ที่ส่งมาจากหน้าแรก
    Private Conn As New OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\Project-2022\Banmai\Banmai\db_banmai1.accdb")



    ' เรียกใช้ฟังก์ชันเมื่อกดปุ่มค้นหา
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        FetchIncomeDetails()
        FetchDataToDataGridView1()
        FetchDataToDataGridView2()
        FetchDataToDataGridView3()
        ContractConAmount()
        GetDataPrincipal()
        GetDataMPrincipal()
        Calculate()
    End Sub


    Private Sub FetchDataToDataGridView1()
        Dim ContractId As String = TextBoxConId.Text
        Try
            Conn.Open()
            Dim query As String = "SELECT * FROM Income_details WHERE con_id = @con_id"
            Dim cmd As New OleDbCommand(query, Conn)
            cmd.Parameters.AddWithValue("@con_id", ContractId)

            Dim adapter As New OleDbDataAdapter(cmd)
            Dim dt As New DataTable()
            adapter.Fill(dt)
            DataGridView1.DataSource = dt ' แสดงข้อมูลใน DataGridView
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาด: " & ex.Message)
        Finally
            Conn.Close()
        End Try
    End Sub
    Private Sub FetchDataToDataGridView2()
        Try
            Conn.Open()
            Dim query As String = "SELECT * FROM Payment  "
            Dim cmd As New OleDbCommand(query, Conn)
            Dim adapter As New OleDbDataAdapter(cmd)
            Dim dt As New DataTable()
            adapter.Fill(dt)
            DataGridView2.DataSource = dt
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาด: " & ex.Message)
        Finally
            Conn.Close()
        End Try
    End Sub

    Private Sub FetchDataToDataGridView3()
        Try
            Conn.Open()
            Dim query As String = "SELECT * FROM Contract  "
            Dim cmd As New OleDbCommand(query, Conn)
            Dim adapter As New OleDbDataAdapter(cmd)
            Dim dt As New DataTable()
            adapter.Fill(dt)
            DataGridView3.DataSource = dt
        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาด: " & ex.Message)
        Finally
            Conn.Close()
        End Try
    End Sub
    Private Sub FetchIncomeDetails()
        Try
            ' เปิดการเชื่อมต่อฐานข้อมูล
            Conn.Open()

            ' กำหนดค่าการค้นหาจาก TextBox
            Dim con_id As String = TextBoxConId.Text  ' ค่าจาก TextBoxConId
            Dim ind_accname As String = "เงินต้น"

            ' ดึงค่าวันที่จาก DateTimePicker
            Dim ind_date As DateTime = DateTimePicker1.Value.Date

            ' แปลงวันที่ให้เป็นรูปแบบที่ MS Access เข้าใจ (MM/dd/yyyy)

            Dim ind_date_formatted As String = ind_date.ToString("MM/dd/yyyy")

            ' สร้างคำสั่ง SQL สำหรับการค้นหา
            Dim query As String = "SELECT ind_amount,ind_date FROM Income_details WHERE con_id = @con_id AND ind_date = @ind_date AND ind_accname = @ind_accname"

            ' สร้างคำสั่ง SQL command
            Dim cmd As New OleDbCommand(query, Conn)
            cmd.Parameters.AddWithValue("@con_id", con_id)
            cmd.Parameters.AddWithValue("@ind_date", ind_date_formatted) ' ใช้วันที่ในรูปแบบ MM/dd/yyyy
            cmd.Parameters.AddWithValue("@ind_accname", ind_accname)

            ' ใช้ OleDbDataReader ในการดึงข้อมูลหลายคอลัมน์
            Dim reader As OleDbDataReader = cmd.ExecuteReader()

            ' ตรวจสอบว่ามีข้อมูลหรือไม่
            If reader.Read() Then
                ' แสดงผล ind_amount ใน TextBox3
                TextBox3.Text = reader("ind_amount").ToString()

                ' แสดงวันที่ ind_date ใน TextBox4
                Dim retrievedDate As DateTime = Convert.ToDateTime(reader("ind_date"))
                TextBox4.Text = retrievedDate.ToString("MM/dd/yyyy") ' แสดงวันที่ในรูปแบบที่เข้าใจง่าย
            Else
                MessageBox.Show("ไม่พบข้อมูล FetchIncomeDetails ที่ตรงกับเงื่อนไข")
                TextBox3.Clear()
                TextBox4.Clear()
            End If

            reader.Close() ' ปิด reader หลังใช้งานเสร็จ

        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาด: " & ex.Message)
        Finally
            ' ปิดการเชื่อมต่อฐานข้อมูล
            Conn.Close()
        End Try
    End Sub

    Private Sub ContractConAmount()
        Try
            ' เปิดการเชื่อมต่อฐานข้อมูล
            Conn.Open()

            ' กำหนดค่าการค้นหาจาก TextBox
            Dim con_id As String = TextBoxConId.Text ' ค่าจาก TextBoxConId

            ' ดึงค่าวันที่จาก DateTimePicker
            Dim con_date As DateTime = DateTimePicker1.Value
            Dim con_date_formatted As String = con_date.ToString("MM/dd/yyyy")

            ' ใช้ค่า DateTimePicker1.Value โดยตรงในพารามิเตอร์ SQL
            Dim query As String = "SELECT con_amount, con_date FROM Contract WHERE con_id = @con_id AND con_date = @con_date"

            ' สร้างคำสั่ง SQL command
            Dim cmd As New OleDbCommand(query, Conn)
            cmd.Parameters.AddWithValue("@con_id", con_id)
            cmd.Parameters.AddWithValue("@con_date", con_date_formatted) ' ใช้ค่า DateTime โดยตรง

            ' ใช้ OleDbDataReader ในการดึงข้อมูลหลายคอลัมน์
            Dim reader As OleDbDataReader = cmd.ExecuteReader()

            ' ตรวจสอบว่ามีข้อมูลหรือไม่
            If reader.Read() Then
                ' แสดงผล con_amount ใน TextBox2
                TextBox2.Text = reader("con_amount").ToString()

                ' แสดงวันที่ con_date ใน TextBox5
                Dim retrievedDate As DateTime = Convert.ToDateTime(reader("con_date"))
                TextBox5.Text = retrievedDate.ToShortDateString() ' แสดงวันที่ในรูปแบบที่เข้าใจง่าย
            Else
                MessageBox.Show("ไม่พบข้อมูล ContractConAmount ที่ตรงกับเงื่อนไข")
            End If

            reader.Close() ' ปิด reader หลังใช้งานเสร็จ

        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาด: " & ex.Message)
        Finally
            ' ปิดการเชื่อมต่อฐานข้อมูล
            Conn.Close()
        End Try
    End Sub


    Private Sub GetDataPrincipal()
        Try
            ' เปิดการเชื่อมต่อฐานข้อมูล
            Conn.Open()

            ' กำหนดค่าการค้นหาจาก TextBox
            Dim con_id As String = TextBoxConId.Text ' ค่าจาก TextBoxConId

            ' ดึงค่าวันที่จาก DateTimePicker และลดลง 1 เดือน
            Dim payment_date As DateTime = DateTimePicker1.Value.AddMonths(-1)
            Dim payment_date_formatted As String = payment_date.ToString("MM/dd/yyyy") ' ใช้รูปแบบ MM/dd/yyyy เพื่อให้สอดคล้องกับ MS Access

            ' ใช้ค่า DateTimePicker1.Value โดยตรงในพารามิเตอร์ SQL
            Dim query As String = "SELECT payment_Mprincipal, payment_date FROM Payment WHERE con_id = @con_id AND payment_date = @payment_date"

            ' สร้างคำสั่ง SQL command
            Dim cmd As New OleDbCommand(query, Conn)
            cmd.Parameters.AddWithValue("@con_id", con_id)
            cmd.Parameters.AddWithValue("@payment_date", payment_date_formatted) ' ใช้ค่า DateTime ในรูปแบบที่ MS Access รองรับ

            ' ใช้ OleDbDataReader ในการดึงข้อมูลหลายคอลัมน์
            Dim reader As OleDbDataReader = cmd.ExecuteReader()

            ' ตรวจสอบว่ามีข้อมูลหรือไม่
            If reader.Read() Then
                ' แสดงผล payment_Principal ใน TextBox8
                TextBox8.Text = reader("payment_Mprincipal").ToString()

                ' แสดงวันที่ payment_date ใน TextBox7
                Dim retrievedDate As DateTime = Convert.ToDateTime(reader("payment_date"))
                TextBox7.Text = retrievedDate.ToShortDateString() ' แสดงวันที่ในรูปแบบที่เข้าใจง่าย
            Else
                MessageBox.Show("ไม่พบข้อมูล GetDataPrincipal ที่ตรงกับเงื่อนไข")
            End If

            reader.Close() ' ปิด reader หลังใช้งานเสร็จ

        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาด: " & ex.Message)
        Finally
            ' ปิดการเชื่อมต่อฐานข้อมูล
            Conn.Close()
        End Try
    End Sub



    Private Sub GetDataMPrincipal()
        Try
            ' เปิดการเชื่อมต่อฐานข้อมูล
            Conn.Open()

            ' กำหนดค่าการค้นหาจาก TextBox
            Dim con_id As String = TextBoxConId.Text ' ค่าจาก TextBoxConId

            ' ดึงค่าวันที่จาก DateTimePicker
            Dim payment_date As DateTime = DateTimePicker1.Value
            Dim payment_date_formatted As String = payment_date.ToString("MM/dd/yyyy") ' ใช้รูปแบบ MM/dd/yyyy เพื่อให้สอดคล้องกับ MS Access

            ' สร้างคำสั่ง SQL สำหรับการค้นหา
            Dim query As String = "SELECT payment_Mprincipal, payment_date FROM Payment WHERE con_id = @con_id AND payment_date = @payment_date"

            ' สร้างคำสั่ง SQL command
            Dim cmd As New OleDbCommand(query, Conn)
            cmd.Parameters.AddWithValue("@con_id", con_id)
            cmd.Parameters.AddWithValue("@payment_date", payment_date_formatted) ' ใช้ค่า DateTime ในรูปแบบที่ MS Access รองรับ

            ' ใช้ OleDbDataReader ในการดึงข้อมูลหลายคอลัมน์
            Dim reader As OleDbDataReader = cmd.ExecuteReader()

            ' ตรวจสอบว่ามีข้อมูลหรือไม่
            If reader.Read() Then
                ' แสดงผล payment_Mprincipal ใน TextBox1
                TextBox1.Text = reader("payment_Mprincipal").ToString()

                ' แสดงวันที่ payment_date ใน TextBox6
                Dim retrievedDate As DateTime = Convert.ToDateTime(reader("payment_date"))
                TextBox6.Text = retrievedDate.ToShortDateString() ' แสดงวันที่ในรูปแบบที่เข้าใจง่าย
            Else
                MessageBox.Show("ไม่พบข้อมูล GetDataMPrincipal ที่ตรงกับเงื่อนไข")
            End If

            reader.Close() ' ปิด reader หลังใช้งานเสร็จ

        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาด: " & ex.Message)
        Finally
            ' ปิดการเชื่อมต่อฐานข้อมูล
            Conn.Close()
        End Try
    End Sub

    Private Sub Calculate()
        Dim a As Decimal
        Dim b As Decimal
        Dim c As Decimal
        Dim total As Decimal

        ' Validate TextBox1
        If Not Decimal.TryParse(TextBox8.Text, a) Then
            MessageBox.Show("Please enter a valid number in TextBox1.")
            Return
        End If

        ' Validate TextBox2
        If Not Decimal.TryParse(TextBox2.Text, b) Then
            MessageBox.Show("Please enter a valid number in TextBox2.")
            Return
        End If

        ' Validate TextBox3
        If Not Decimal.TryParse(TextBox3.Text, c) Then
            MessageBox.Show("Please enter a valid number in TextBox3.")
            Return
        End If

        ' Calculate the total
        total = (a + b) - c

        ' Display the result in TextBox9
        TextBox9.Text = total.ToString()
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            ' เปิดการเชื่อมต่อฐานข้อมูล
            Conn.Open()

            ' แปลงค่าจาก TextBox9 เป็น Decimal เพื่อบันทึกลงในฐานข้อมูล (ถ้าข้อมูลเป็นตัวเลข)
            Dim paymentMPrincipal As Decimal
            If Not Decimal.TryParse(TextBox9.Text, paymentMPrincipal) Then
                MessageBox.Show("กรุณากรอกจำนวนเงินที่ถูกต้อง", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            ' บันทึกค่าใน TextBox9 ไปยัง payment_Mprincipal ในเดือนที่เลือกใน DateTimePicker1
            Dim query1 As String = "UPDATE Payment SET payment_Mprincipal = @payment_Mprincipal WHERE con_id = @con_id AND payment_date = @payment_date"
            Dim cmd1 As New OleDbCommand(query1, Conn)
            cmd1.Parameters.AddWithValue("@payment_Mprincipal", paymentMPrincipal)
            cmd1.Parameters.AddWithValue("@con_id", TextBoxConId.Text)
            cmd1.Parameters.AddWithValue("@payment_date", DateTimePicker1.Value.ToString("MM/dd/yyyy"))
            cmd1.ExecuteNonQuery()

            ' บันทึกค่าใน TextBox9 ไปยัง payment_principal ในเดือนถัดไป
            Dim nextMonthDate As DateTime = DateTimePicker1.Value.AddMonths(1) ' เพิ่ม 1 เดือน
            Dim query2 As String = "UPDATE Payment SET payment_Principal = @payment_Principal WHERE con_id = @con_id AND payment_date = @next_payment_date"
            Dim cmd2 As New OleDbCommand(query2, Conn)
            cmd2.Parameters.AddWithValue("@payment_Principal", paymentMPrincipal)
            cmd2.Parameters.AddWithValue("@con_id", TextBoxConId.Text)
            cmd2.Parameters.AddWithValue("@next_payment_date", nextMonthDate.ToString("MM/dd/yyyy"))
            cmd2.ExecuteNonQuery()

            ' แสดงข้อความสำเร็จเมื่อบันทึกข้อมูลเสร็จ
            ' MessageBox.Show("บันทึกข้อมูลเรียบร้อยแล้ว", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("เกิดข้อผิดพลาด: " & ex.Message)
        Finally
            ' ปิดการเชื่อมต่อฐานข้อมูล
            Conn.Close()
        End Try
    End Sub


    Private Async Sub TestGetData_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' เมื่อหน้าโหลด ให้ใช้ค่า ContractId ในการดึงข้อมูล
        If Not String.IsNullOrEmpty(ContractId) Then
            FetchDataToDataGridView1() ' ดึงข้อมูลตามค่า conId ที่ถูกส่งมา
        End If
        Button1.PerformClick()
        Await Task.Delay(2000) ' รอ 2 วินาที (2000 milliseconds)
        Button2.PerformClick()
        Me.Close()
    End Sub


End Class
