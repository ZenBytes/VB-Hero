Imports System.IO

Public Class Game

    Public WithEvents mainTimer As New System.Windows.Forms.Timer()
    Public WithEvents noteTimer As New System.Windows.Forms.Timer()
    Public WithEvents beatTimer As New System.Windows.Forms.Timer()
    Public bgColors(4) As Brush
    Public scorebar As Rectangle = New System.Drawing.Rectangle(0, 520, 300, 60)
    Public notes(1000) As Rectangle
    Public notecleared(1000) As Boolean
    Public doublenotes(1000) As Rectangle
    Public noteCount As Integer
    Public reader As StreamReader = New StreamReader(Mainmenu.notelocation)
    Public line_num As Integer = 1
    Public lane As Integer
    Public score As Integer = 100
    Public notechart(1000) As String
    Public popup As String
    Public rndbrush As Brush
    Public rndtext As Brush
    Public rndbrushval As Integer
    Public song As String

    Const beatinterval As Integer = 44 'the interval between beats in the song, 40 = very slow, 20 = very fast, adjust to match song
    Const penalty As Integer = 40 'the number of points to deduct for missed notes
    Const good As Integer = 50 'the number of points to add for a "good" note hit
    Const great As Integer = 100 'the number of points to add for a "great" note hit

    Declare Function GetAsyncKeyState Lib "user32" _
    (ByVal vKey As Long) As Integer

    Function random_int(ByVal small As Integer, ByVal big As Integer) As Integer
        Return Int(Rnd() * (big - small + 1)) + small
    End Function

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.DoubleBuffered = True
        Me.Size = New System.Drawing.Size(455, 628)
        Me.Text = "VB Hero"
        Randomize()

        mainTimer.Interval = 25
        mainTimer.Start()
        beatTimer.Interval = beatinterval
        beatTimer.Start()
        noteTimer.Interval = beatinterval * 16
        noteTimer.Start()

        bgColors(1) = Brushes.LightBlue
        bgColors(2) = Brushes.LightGreen
        bgColors(3) = Brushes.Pink
        bgColors(4) = Brushes.LightYellow

        My.Computer.Audio.Play(Mainmenu.songlocation, AudioPlayMode.Background)

        Do Until reader.EndOfStream
            notechart(line_num) = reader.ReadLine
            line_num = line_num + 1
        Loop
        reader.Close()

        Dim fileFilter As System.IO.FileInfo = My.Computer.FileSystem.GetFileInfo(Mainmenu.songlocation)
        song = fileFilter.Name

    End Sub
    Private Sub mainTimerLoop(ByVal myObject As Object, ByVal myEventArgs As EventArgs) Handles mainTimer.Tick
        bgColors(1) = Brushes.LightBlue
        bgColors(2) = Brushes.LightGreen
        bgColors(3) = Brushes.Pink
        bgColors(4) = Brushes.LightYellow

        'Handling control input
        If GetAsyncKeyState(Keys.Q) Then bgColors(1) = Brushes.Blue
        If GetAsyncKeyState(Keys.W) Then bgColors(2) = Brushes.Green
        If GetAsyncKeyState(Keys.E) Then bgColors(3) = Brushes.Violet
        If GetAsyncKeyState(Keys.R) Then bgColors(4) = Brushes.Yellow

        If noteCount >= 3 Then
            If notes(noteCount - 3).Y >= 600 And notecleared(noteCount - 3) = False And notechart(noteCount - 3) <> "" Then
                score -= penalty
                notecleared(noteCount - 3) = True
                popup = "miss"
            End If
        End If

        If score <= 0 Then score = 0

        Invalidate()
    End Sub
    Private Sub noteTimerLoop(ByVal myObject As Object, ByVal myEventArgs As EventArgs) Handles noteTimer.Tick
        noteCount += 1

        If notechart(noteCount) = "Q" Then notes(noteCount) = New System.Drawing.Rectangle(5, 0, 50, 35)
        If notechart(noteCount) = "W" Then notes(noteCount) = New System.Drawing.Rectangle(80, 0, 50, 35)
        If notechart(noteCount) = "E" Then notes(noteCount) = New System.Drawing.Rectangle(155, 0, 50, 35)
        If notechart(noteCount) = "R" Then notes(noteCount) = New System.Drawing.Rectangle(230, 0, 50, 35)
        If notechart(noteCount) = "QW" Then
            notes(noteCount) = New System.Drawing.Rectangle(5, 0, 50, 35)
            doublenotes(noteCount) = New System.Drawing.Rectangle(80, 0, 50, 35)
        ElseIf notechart(noteCount) = "QE" Then
            notes(noteCount) = New System.Drawing.Rectangle(5, 0, 50, 35)
            doublenotes(noteCount) = New System.Drawing.Rectangle(155, 0, 50, 35)
        ElseIf notechart(noteCount) = "QR" Then
            notes(noteCount) = New System.Drawing.Rectangle(5, 0, 50, 35)
            doublenotes(noteCount) = New System.Drawing.Rectangle(230, 0, 50, 35)
        ElseIf notechart(noteCount) = "WE" Then
            notes(noteCount) = New System.Drawing.Rectangle(80, 0, 50, 35)
            doublenotes(noteCount) = New System.Drawing.Rectangle(155, 0, 50, 35)
        ElseIf notechart(noteCount) = "WR" Then
            notes(noteCount) = New System.Drawing.Rectangle(80, 0, 50, 35)
            doublenotes(noteCount) = New System.Drawing.Rectangle(230, 0, 50, 35)
        ElseIf notechart(noteCount) = "ER" Then
            notes(noteCount) = New System.Drawing.Rectangle(155, 0, 50, 35)
            doublenotes(noteCount) = New System.Drawing.Rectangle(230, 0, 50, 35)
        End If

        If noteCount = line_num + 6 Then Form2.Show()

    End Sub
    Private Sub beatTimerLoop(ByVal myObject As Object, ByVal myEventArgs As EventArgs) Handles beatTimer.Tick
        For i = 1 To noteCount
            notes(i).Y += 16
            doublenotes(i).Y += 16
        Next
    End Sub
    Private Sub Form1_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseClick
        popup = "miss"

        If GetAsyncKeyState(Keys.Q) Then
            For i = noteCount - (noteCount / 1.2) To noteCount

                If notes(i).Y >= scorebar.Top And notes(i).Y <= scorebar.Bottom And notes(i).X = 5 Then
                    score += great + penalty
                    popup = "great"
                    Call RandomBrush(rndbrush, rndtext)
                    notecleared(i) = True
                ElseIf notes(i).IntersectsWith(scorebar) Then
                    score += good + penalty
                    popup = "good"
                    Call RandomBrush(rndbrush, rndtext)
                    notecleared(i) = True
                End If

            Next
        End If

        If GetAsyncKeyState(Keys.W) Then
            For i = noteCount - (noteCount / 1.2) To noteCount

                If notes(i).Y >= scorebar.Top And notes(i).Y <= scorebar.Bottom And notes(i).X = 80 Then
                    score += great + penalty
                    popup = "great"
                    Call RandomBrush(rndbrush, rndtext)
                    notecleared(i) = True
                ElseIf notes(i).IntersectsWith(scorebar) Then
                    score += good + penalty
                    popup = "good"
                    Call RandomBrush(rndbrush, rndtext)
                    notecleared(i) = True
                End If

            Next
        End If

        If GetAsyncKeyState(Keys.E) Then
            For i = noteCount - (noteCount / 1.2) To noteCount

                If notes(i).Y >= scorebar.Top And notes(i).Y <= scorebar.Bottom And notes(i).X = 155 Then
                    score += great + penalty
                    popup = "great"
                    Call RandomBrush(rndbrush, rndtext)
                    notecleared(i) = True
                ElseIf notes(i).IntersectsWith(scorebar) Then
                    score += good + penalty
                    popup = "good"
                    Call RandomBrush(rndbrush, rndtext)
                    notecleared(i) = True
                End If

            Next
        End If

        If GetAsyncKeyState(Keys.R) Then

            For i = noteCount - (noteCount / 1.2) To noteCount
                If notes(i).Y >= scorebar.Top And notes(i).Y <= scorebar.Bottom And notes(i).X = 230 Then
                    score += great + penalty
                    popup = "great"
                    Call RandomBrush(rndbrush, rndtext)
                    notecleared(i) = True
                ElseIf notes(i).IntersectsWith(scorebar) Then
                    score += good + penalty
                    popup = "good"
                    Call RandomBrush(rndbrush, rndtext)
                    notecleared(i) = True
                End If

            Next
        End If

        score -= penalty
        If score <= 0 Then score = 0

    End Sub
    Private Sub Form1_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        'Painting the background
        e.Graphics.FillRectangle(bgColors(1), 0, 0, 75, 600)
        e.Graphics.FillRectangle(bgColors(2), 75, 0, 75, 600)
        e.Graphics.FillRectangle(bgColors(3), 150, 0, 75, 600)
        e.Graphics.FillRectangle(bgColors(4), 225, 0, 75, 600)
        e.Graphics.FillRectangle(Brushes.Red, scorebar)

        'Painting notes
        For i = 1 To noteCount
            e.Graphics.FillEllipse(Brushes.DarkGoldenrod, notes(i))
            e.Graphics.FillEllipse(Brushes.Goldenrod, doublenotes(i))
        Next

        'Painting text
        Dim rect As New System.Drawing.Rectangle(300, 0, 150, 100)
        Dim font1 As New System.Drawing.Font("Times New Roman", 12, FontStyle.Bold)
        Dim font2 As New System.Drawing.Font("Arial", 16, FontStyle.Bold)
        e.Graphics.DrawString("Score: " & score, font2, Brushes.Red, 300, 560)
        e.Graphics.DrawString("Song: " & song, font1, Brushes.Blue, rect)

        'Painting popups
        If popup = "good" Then
            e.Graphics.FillEllipse(rndbrush, 310, 350, 120, 60)
            e.Graphics.DrawString("Good!", New System.Drawing.Font("Bauhaus 93 Regular", 14, FontStyle.Bold), rndtext, 336, 370)
        ElseIf popup = "great" Then
            e.Graphics.FillEllipse(rndbrush, 310, 350, 120, 60)
            e.Graphics.DrawString("Great!", New System.Drawing.Font("Bauhaus 93 Regular", 16, FontStyle.Bold), rndtext, 332, 369)
        ElseIf popup = "miss" Then
            e.Graphics.FillEllipse(Brushes.Black, 310, 350, 120, 60)
            e.Graphics.DrawString("Miss!", New System.Drawing.Font("Bauhaus 93 Regular", 14, FontStyle.Bold), Brushes.White, 340, 370)
        End If

    End Sub

    Sub RandomBrush(ByRef brush As Brush, ByRef text As Brush)

        rndbrushval = random_int(1, 4)

        If rndbrushval = 1 Then
            brush = Brushes.Red
            text = Brushes.Blue
        ElseIf rndbrushval = 2 Then
            brush = Brushes.Blue
            text = Brushes.Red
        ElseIf rndbrushval = 3 Then
            brush = Brushes.Yellow
            text = Brushes.Green
        ElseIf rndbrushval = 4 Then
            brush = Brushes.Yellow
            text = Brushes.Green
        End If

    End Sub

    Sub onClose() Handles Me.FormClosed

        Mainmenu.Show()

    End Sub

End Class
