Imports System
Imports System.IO
Imports System.Text

Public Class Form2

    Dim highscores(500) As Integer

    Private Sub Form2_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If System.IO.File.Exists("E:\highscores.txt") Then
        Else
            Dim filestream As FileStream = File.Create("E:\highscores.txt")
            filestream.Close()
        End If

        Dim streamwriter As New System.IO.StreamWriter("E:\highscores.txt", True)
        streamwriter.Write(Chr(13) & Chr(10) & Game.score.ToString)
        streamwriter.Close()

        Dim streamreader As New System.IO.StreamReader("E:\highscores.txt")
        Dim line_num As Integer

        Do Until streamreader.EndOfStream
            highscores(line_num) = streamreader.ReadLine
            line_num += 1
        Loop

        Array.Sort(highscores)

    End Sub

    Private Sub Form2_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint

        For i = 0 To 4
            e.Graphics.DrawString(highscores(i), SystemFonts.CaptionFont, Brushes.Red, 20, 40 + (20 * i))
        Next

    End Sub
End Class