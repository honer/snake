Imports System.Drawing.Drawing2D
Public Class Form1
    Dim GoX As Integer = 0
    Dim GoY As Integer = 0
    Const StartX As Integer = 200
    Const StartY As Integer = 200
    Dim FoodX As Integer
    Dim FoodY As Integer
    Dim BodyX() As Integer
    Dim BodyY() As Integer
    Dim BodyBlankX As Integer
    Dim BodyBlankY As Integer
    Dim FlyHeadX As Integer
    Dim FlyHeadY As Integer
    Dim Score As Integer = 0 '分數
    Dim FlyHeadFlag As Boolean

    Private Sub Form1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case 37
                GoX = -20
                GoY = 0
            Case 38
                GoX = 0
                GoY = -20
            Case 39
                GoX = 20
                GoY = 0
            Case 40
                GoX = 0
                GoY = 20
            Case 32
                CreatFlayHead()
            Case Else
        End Select
    End Sub
    Sub CreatFlayHead() '產生飛出去的頭

        If FlyHeadX >= 0 And FlyHeadX <= 660 And FlyHeadY >= 0 And FlyHeadY <= 340 Then '假如上一個飛頭未消失不產生新的
            Exit Sub
        End If

        FlyHeadFlag = True
        If BodyX(0) > BodyX(1) Then
            FlyHeadX = BodyX(0) + 40
            FlyHeadY = BodyY(0)
        ElseIf BodyX(0) < BodyX(1) Then
            FlyHeadX = BodyX(0) - 40
            FlyHeadY = BodyY(0)
        ElseIf BodyY(0) > BodyY(1) Then
            FlyHeadX = BodyX(0)
            FlyHeadY = BodyY(0) + 40
        ElseIf BodyY(0) < BodyY(1) Then
            FlyHeadX = BodyX(0)
            FlyHeadY = BodyY(0) - 40
        End If
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ReDim BodyX(1)
        ReDim BodyY(1)
        BodyX(0) = StartX
        BodyY(0) = StartY
        BodyX(1) = StartX - 20
        BodyY(1) = StartY - 20
        FlyHeadX = -1000
        FlyHeadY = -1000
        CreatFood()
    End Sub
    Sub Draw(ByVal X As Integer, ByVal Y As Integer)
        Dim g As Graphics
        g = Me.PictureBox1.CreateGraphics
        Dim rc As New Rectangle(X, Y, 20, 20)
        Dim hb As New LinearGradientBrush(rc, Color.Chartreuse, Color.Azure, LinearGradientMode.Horizontal)
        g.FillRectangle(hb, rc)
    End Sub
    Sub DrawFood()
        Dim g As Graphics
        g = Me.PictureBox1.CreateGraphics
        Dim rc As New Rectangle(FoodX, FoodY, 20, 20)
        Dim hb As New LinearGradientBrush(rc, Color.Red, Color.Azure, LinearGradientMode.Horizontal)
        g.FillRectangle(hb, rc)
    End Sub
    Sub DrawDraw(ByVal X() As Integer, ByVal Y() As Integer)
        Dim i As Integer
        For i = 0 To X.Length - 1
            Draw(X(i), Y(i))
        Next
    End Sub
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If CheckWhetherEatedFood() Then '是否吃到食物
            ExrtendBody()
            CreatFood()
            DrawFood()
            Score += 100
            Label1.Text = "分數 : " + Score.ToString
            Beep()
        End If
        If CheckWhetherEatedBody() Then '是否吃到自己身體
            Score -= 200
            Label1.Text = "分數 : " + Score.ToString
            Beep()
        End If
        If CheckWhetherEatedFoodOfFlyHead() Then '飛頭是否吃到食物
            ExrtendBody()
            CreatFood()
            DrawFood()
            Score += 300
            Label1.Text = "分數 : " + Score.ToString
            Beep()
        End If
        If CheckWhetherFlyHeadGoOutSide() Then
            FlyHeadX = -1000
            FlyHeadY = -1000
            Score -= 400
            Label1.Text = "分數 : " + Score.ToString
            Beep()
        End If

        DrawFlyHead()      '畫出飛頭

        CreatNewXY(BodyX, BodyY, BodyBlankX, BodyBlankY)                    '產生新身體的座標
        DrawBlank(BodyBlankX, BodyBlankY) '舊身體最後一個畫上空白
        DrawDraw(BodyX, BodyY)            '畫出身體

        DrawFood()                        '畫出食物
    End Sub
    Function CheckWhetherFlyHeadGoOutSide() As Boolean '判斷飛頭是否發射卻沒吃到食物

        If FlyHeadX < 0 And FlyHeadX <> -1000 Or FlyHeadX >= 680 Or FlyHeadY < 0 And FlyHeadY <> -1000 Or FlyHeadY >= 360 Then
            Return True
        End If
        Return False
    End Function
    Function CheckWhetherEatedFoodOfFlyHead() As Boolean '判斷飛頭是否吃到食物
        If FlyHeadX = FoodX And FlyHeadY = FoodY Then
            DrawBlank(FlyHeadX, FlyHeadY)
            FlyHeadX = -1000
            FlyHeadY = -1000
            Return True
        End If
        Return False
    End Function
    Sub DrawFlyHead()
        Static FlyHeadGoX As Integer
        Static FlyHeadGoY As Integer

        If FlyHeadFlag Then
            FlyHeadGoX = GoX
            FlyHeadGoY = GoY
            FlyHeadFlag = False
        End If
        DrawBlank(FlyHeadX, FlyHeadY)
        If FlyHeadX <> -1000 Then FlyHeadX += FlyHeadGoX
        If FlyHeadY <> -1000 Then FlyHeadY += FlyHeadGoY
        Draw(FlyHeadX, FlyHeadY)
    End Sub
    Sub DrawBlank(ByVal X As Integer, ByVal Y As Integer)
        Dim g As Graphics
        g = Me.PictureBox1.CreateGraphics
        Dim rc As New Rectangle(X, Y, 20, 20)
        Dim hb As New LinearGradientBrush(rc, Color.White, Color.White, LinearGradientMode.Horizontal)
        g.FillRectangle(hb, rc)
    End Sub
    Sub ExrtendBody()
        Dim Xtemp(BodyX.Length - 1) As Integer
        Dim Ytemp(BodyY.Length - 1) As Integer
        Array.Copy(BodyX, Xtemp, BodyX.Length) '將原來的BodyX存起來
        Array.Copy(BodyY, Ytemp, BodyY.Length) '將原來的BodyY存起來
        ReDim BodyX(BodyX.Length) '將原先的陣列+1
        ReDim BodyY(BodyY.Length) '將原先的陣列+1
        Array.Copy(Xtemp, BodyX, Xtemp.Length) '將值讀回來
        Array.Copy(Ytemp, BodyY, Ytemp.Length) '將值讀回來

        If BodyX.Length = 2 Then '假如長度2就根據前進方向來產生新的身體
            If GoX > 0 Then
                BodyX(BodyX.Length - 1) = BodyX(BodyX.Length - 2) - 20
                BodyY(BodyY.Length - 1) = BodyY(BodyY.Length - 2)
            ElseIf GoX < 0 Then
                BodyX(BodyX.Length - 1) = BodyX(BodyX.Length - 2) + 20
                BodyY(BodyY.Length - 1) = BodyY(BodyY.Length - 2)
            ElseIf GoY > 0 Then
                BodyX(BodyX.Length - 1) = BodyX(BodyX.Length - 2)
                BodyY(BodyY.Length - 1) = BodyY(BodyY.Length - 2) - 20
            ElseIf GoY < 0 Then
                BodyX(BodyX.Length - 1) = BodyX(BodyX.Length - 2)
                BodyY(BodyY.Length - 1) = BodyY(BodyY.Length - 2) + 20
            End If
        ElseIf BodyX.Length > 2 Then '假如長度超過2,就根據舊身體的倒數第二個和倒數第一個來產生新的
            If BodyX(BodyX.Length - 3) > BodyX(BodyX.Length - 2) Then
                BodyX(BodyX.Length - 1) = BodyX(BodyX.Length - 2) - 20
                BodyY(BodyY.Length - 1) = BodyY(BodyY.Length - 2)
            ElseIf BodyX(BodyX.Length - 3) < BodyX(BodyX.Length - 2) Then
                BodyX(BodyX.Length - 1) = BodyX(BodyX.Length - 2) + 20
                BodyY(BodyY.Length - 1) = BodyY(BodyY.Length - 2)
            ElseIf BodyY(BodyY.Length - 3) > BodyY(BodyY.Length - 2) Then
                BodyX(BodyX.Length - 1) = BodyX(BodyX.Length - 2)
                BodyY(BodyY.Length - 1) = BodyY(BodyY.Length - 2) - 20
            ElseIf BodyY(BodyY.Length - 3) < BodyY(BodyY.Length - 2) Then
                BodyX(BodyX.Length - 1) = BodyX(BodyX.Length - 2)
                BodyY(BodyY.Length - 1) = BodyY(BodyY.Length - 2) + 20
            End If
        End If
    End Sub

    Sub CreatNewXY(ByVal X() As Integer, ByVal Y() As Integer, ByRef BlankX As Integer, ByRef BlankY As Integer)
        Dim i As Integer

        If X.Length >= 2 Then
            BlankX = X(X.Length - 1)
            BlankY = Y(Y.Length - 1)
            For i = X.Length - 1 To 1 Step -1
                X(i) = X(i - 1)
                Y(i) = Y(i - 1)
            Next
        End If

        X(0) += GoX
        Y(0) += GoY
    End Sub
    Function CheckWhetherEatedBody() As Boolean '判斷是否吃到自己的身體
        Dim i As Integer
        If BodyX.Length > 2 Then
            For i = 1 To BodyX.Length - 1
                If BodyX(i) = BodyX(0) And BodyY(i) = BodyY(0) Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

    Function CheckWhetherEatedFood() As Boolean '判斷是否吃到食物
        Dim i As Integer
        For i = 0 To BodyX.Length - 1
            If BodyX(i) = FoodX And BodyY(i) = FoodY Then
                Return True
            End If
        Next
        Return False
    End Function

    Sub ClearDraw()
        Dim g As Graphics
        g = Me.PictureBox1.CreateGraphics
        g.Clear(Color.White)
    End Sub

    Sub CreatFood()
        Dim rnd As New Random
        Do
            FoodX = rnd.NextDouble * 660
            FoodY = rnd.NextDouble * 340
        Loop While FoodX Mod 20 <> 0 Or FoodY Mod 20 <> 0
    End Sub
End Class
