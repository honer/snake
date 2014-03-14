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
    Dim Score As Integer = 0 '����
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
    Sub CreatFlayHead() '���ͭ��X�h���Y

        If FlyHeadX >= 0 And FlyHeadX <= 660 And FlyHeadY >= 0 And FlyHeadY <= 340 Then '���p�W�@�ӭ��Y�����������ͷs��
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
        If CheckWhetherEatedFood() Then '�O�_�Y�쭹��
            ExrtendBody()
            CreatFood()
            DrawFood()
            Score += 100
            Label1.Text = "���� : " + Score.ToString
            Beep()
        End If
        If CheckWhetherEatedBody() Then '�O�_�Y��ۤv����
            Score -= 200
            Label1.Text = "���� : " + Score.ToString
            Beep()
        End If
        If CheckWhetherEatedFoodOfFlyHead() Then '���Y�O�_�Y�쭹��
            ExrtendBody()
            CreatFood()
            DrawFood()
            Score += 300
            Label1.Text = "���� : " + Score.ToString
            Beep()
        End If
        If CheckWhetherFlyHeadGoOutSide() Then
            FlyHeadX = -1000
            FlyHeadY = -1000
            Score -= 400
            Label1.Text = "���� : " + Score.ToString
            Beep()
        End If

        DrawFlyHead()      '�e�X���Y

        CreatNewXY(BodyX, BodyY, BodyBlankX, BodyBlankY)                    '���ͷs���骺�y��
        DrawBlank(BodyBlankX, BodyBlankY) '�¨���̫�@�ӵe�W�ť�
        DrawDraw(BodyX, BodyY)            '�e�X����

        DrawFood()                        '�e�X����
    End Sub
    Function CheckWhetherFlyHeadGoOutSide() As Boolean '�P�_���Y�O�_�o�g�o�S�Y�쭹��

        If FlyHeadX < 0 And FlyHeadX <> -1000 Or FlyHeadX >= 680 Or FlyHeadY < 0 And FlyHeadY <> -1000 Or FlyHeadY >= 360 Then
            Return True
        End If
        Return False
    End Function
    Function CheckWhetherEatedFoodOfFlyHead() As Boolean '�P�_���Y�O�_�Y�쭹��
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
        Array.Copy(BodyX, Xtemp, BodyX.Length) '�N��Ӫ�BodyX�s�_��
        Array.Copy(BodyY, Ytemp, BodyY.Length) '�N��Ӫ�BodyY�s�_��
        ReDim BodyX(BodyX.Length) '�N������}�C+1
        ReDim BodyY(BodyY.Length) '�N������}�C+1
        Array.Copy(Xtemp, BodyX, Xtemp.Length) '�N��Ū�^��
        Array.Copy(Ytemp, BodyY, Ytemp.Length) '�N��Ū�^��

        If BodyX.Length = 2 Then '���p����2�N�ھګe�i��V�Ӳ��ͷs������
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
        ElseIf BodyX.Length > 2 Then '���p���׶W�L2,�N�ھ��¨��骺�˼ƲĤG�өM�˼ƲĤ@�ӨӲ��ͷs��
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
    Function CheckWhetherEatedBody() As Boolean '�P�_�O�_�Y��ۤv������
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

    Function CheckWhetherEatedFood() As Boolean '�P�_�O�_�Y�쭹��
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
