Imports System.Windows.Forms
Imports System.Globalization
Imports System.Text

Module Module1
    Public User_name As String
    Public User_type As String

    Public Function ConvertDateMsAccess(ByVal dat As Date, Optional ByVal bDateThai As Boolean = True, Optional ByVal FormatDateTime As String = "dd/MM/yyyy") As String
        Dim culinfo As CultureInfo
        Dim dtf As DateTimeFormatInfo

        If bDateThai Then
            'culinfo = New CultureInfo("th-TH")
            culinfo = New CultureInfo("TH")
            dtf = culinfo.DateTimeFormat
        Else
            'culinfo = New CultureInfo("en-US")
            culinfo = New CultureInfo("US")
            dtf = culinfo.DateTimeFormat
        End If

        Return dat.ToString(FormatDateTime, dtf)
    End Function

    Public Function ConvertStringDateThai(ByVal para_dat As String)
        Dim rt As String = ""
        Dim arr_dat() As String = para_dat.Split("-")
        rt = CInt(arr_dat(2)).ToString & " " & ConvertMountThai(arr_dat(1)) & " " & arr_dat(0)
        Return rt
    End Function

    Public Function ConvertMountThai(ByVal para_txt As String)
        Dim rt As String = ""
        Select Case para_txt
            Case "01"
                rt = "มกราคม"
            Case "02"
                rt = "กุมภาพันธ์"
            Case "03"
                rt = "มีนาคม"
            Case "04"
                rt = "เมษายน"
            Case "05"
                rt = "พฤษภาคม"
            Case "06"
                rt = "มิถุนายน"
            Case "07"
                rt = "กรกฏาคม"
            Case "08"
                rt = "สิงหาคม"
            Case "09"
                rt = "กันยายน"
            Case "10"
                rt = "ตุลาคม"
            Case "11"
                rt = "พฤศจิกายน"
            Case "12"
                rt = "ธันวาคม"
        End Select

        Return rt
    End Function


End Module
