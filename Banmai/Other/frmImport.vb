Imports System.IO
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class frmImport
    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        Dim OFD As New OpenFileDialog With {
        .Filter = "Access Files|*.accdb",
        .Title = "เลือกไฟล์ฐานข้อมูล"
    }

        If OFD.ShowDialog() = DialogResult.OK Then
            Dim PickedFile As String = OFD.FileName

            ' ให้ผู้ใช้เลือกตำแหน่งปลายทาง
            Dim SFD As New SaveFileDialog With {
            .Filter = "Access Files|*.accdb",
            .Title = "เลือกตำแหน่งที่จะบันทึกฐานข้อมูล",
            .FileName = "db_banmai1.accdb"
        }

            If SFD.ShowDialog() = DialogResult.OK Then
                Dim RestoreLocation As String = SFD.FileName

                Try
                    ' ตรวจสอบว่าปลายทางไม่ถูกใช้งาน
                    If IsFileInUse(RestoreLocation) Then
                        MessageBox.Show("ไฟล์ฐานข้อมูลปลายทางกำลังถูกใช้งานอยู่ กรุณาปิดไฟล์แล้วลองใหม่อีกครั้ง")
                        Return
                    End If

                    ' สำรองไฟล์เดิม (ถ้ามี)
                    If File.Exists(RestoreLocation) Then
                        File.Copy(RestoreLocation, RestoreLocation & ".bak", True)
                    End If

                    ' เริ่มต้น ProgressBar
                    ProgressBar1.Value = 0
                    ProgressBar1.Maximum = 100
                    ProgressBar1.Visible = True

                    ' คัดลอกไฟล์พร้อมอัปเดต ProgressBar
                    CopyFileWithProgress(PickedFile, RestoreLocation)

                    MessageBox.Show("เรียกคืนข้อมูลเรียบร้อยแล้ว!")
                Catch ex As Exception
                    MessageBox.Show("เกิดข้อผิดพลาดในการเรียกคืนข้อมูล: " & ex.Message)
                Finally
                    ProgressBar1.Visible = False ' ซ่อน ProgressBar หลังจากทำเสร็จ
                End Try
            End If
        End If
    End Sub



    ' ฟังก์ชันตรวจสอบว่าไฟล์กำลังถูกใช้งานอยู่หรือไม่
    Private Function IsFileInUse(filePath As String) As Boolean
        Try
            Using fs As New FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None)
                fs.Close()
            End Using
            Return False
        Catch ex As IOException
            Return True
        End Try
    End Function

    ' ฟังก์ชันคัดลอกไฟล์พร้อมแสดง progress
    Private Sub CopyFileWithProgress(sourceFile As String, destinationFile As String)
        Dim bufferSize As Integer = 1024 * 1024 ' 1MB buffer
        Dim buffer(bufferSize - 1) As Byte
        Dim bytesRead As Integer

        Using sourceStream As New FileStream(sourceFile, FileMode.Open, FileAccess.Read)
            Using destinationStream As New FileStream(destinationFile, FileMode.Create, FileAccess.Write)
                Dim totalBytes As Long = sourceStream.Length
                Dim totalBytesRead As Long = 0

                While (bytesRead = sourceStream.Read(buffer, 0, bufferSize)) > 0
                    destinationStream.Write(buffer, 0, bytesRead)
                    totalBytesRead += bytesRead

                    ' อัปเดต progress bar
                    Dim progressPercentage As Integer = CInt((totalBytesRead * 100) / totalBytes)
                    ProgressBar1.Value = progressPercentage
                End While
            End Using
        End Using
    End Sub
End Class
