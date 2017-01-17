Public Class Mainmenu

    Public songlocation As String 'location of song
    Public notelocation As String 'location of notechart

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        OpenFileDialog1.Title = "Select Song"
        OpenFileDialog1.DefaultExt = "wav"
        OpenFileDialog1.Filter = "Waveform Audio File|*.wav|All Files|*.*"
        OpenFileDialog1.FilterIndex = 1
        OpenFileDialog1.ShowDialog()

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        OpenFileDialog2.Title = "Select Notechart"
        OpenFileDialog2.DefaultExt = "txt"
        OpenFileDialog2.Filter = "Text File|*.txt|All Files|*.*"
        OpenFileDialog2.FilterIndex = 1
        OpenFileDialog2.ShowDialog()

    End Sub

    Private Sub OpenFileDialog1_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk

        songlocation = OpenFileDialog1.FileName
        TextBox1.Text = songlocation

    End Sub

    Private Sub OpenFileDialog2_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog2.FileOk

        notelocation = OpenFileDialog2.FileName
        TextBox2.Text = notelocation

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click

        If songlocation <> "" And notelocation <> "" Then
            Game.Show()
            Me.Hide()
        Else
            MsgBox("Please select your song and notechart!")
        End If

    End Sub
End Class