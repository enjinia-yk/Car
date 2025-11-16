Imports System.Windows.Media.Media3D

Public Class FlyingCar
    Inherits Car

    Private _speedY As Double = 0


    Public Sub New(visual As ModelVisual3D)
        MyBase.New(visual)

        ' ★ ここで翼やプロペラを追加しても OK
        ' group.Children.Add(CreateWing(...))
    End Sub

    ' ===== 飛行操作 =====
    Public Sub Ascend()

        If SpeedZ >= 100 Then
            If _speedY < 3 Then
                _speedY += 1
            Else
                _speedY = 3
            End If
        End If

    End Sub

    Public Sub Descend()
        If _speedY > -2 Then
            _speedY -= 1
        Else
            _speedY = -2
        End If
    End Sub


    Public Overrides Sub MoveForward()

        _speedY = Math.Floor(_speedY * (10 ^ 1)) / (10 ^ 1)

        ' 追加の Y 移動（飛行）
        posY += _speedY * 0.05
        If posY < 0 Then posY = 0  ' 地面より下に行かないように制限

        If posY > 0 Then
            ' 浮いているときは100より遅くならない
            If SpeedZ < 100 Then
                SpeedZ = 100
            End If
        End If
        MyBase.MoveForward()


        '_posX -= _speedX * _speedZ * 0.0002
        '_posZ -= _speedZ * 0.02
        'UpdateTransform()
    End Sub

    ' 速度の慣性・自然減速
    Public Overrides Sub ApplyFriction()
        If _speedY > 0 Then
            _speedY -= 0.1  ' ★減速の強さ（調整可）
            If _speedY < 0 Then _speedY = 0
        ElseIf _speedY < 0 Then
            _speedY += 0.1  ' 後退している場合も減速
            If _speedY > 0 Then _speedY = 0
        End If
        MyBase.ApplyFriction()
    End Sub

    ' _speedY の値を取得するプロパティ
    Public Property speedY() As String
        Get
            Return _speedY
        End Get
        Set(ByVal value As String)
            _speedY = value
        End Set
    End Property

    '' ===== 移動処理の上書き =====
    'Protected Overrides Sub UpdateTransform()
    '    ' 通常の X-Z 移動は Car の挙動を利用
    '    MyBase.UpdateTransform()

    '    ' 追加の Y 移動（飛行）
    '    _posY += _speedY * 0.05

    '    ' Transform 再適用
    '    'ApplyTransform()
    'End Sub

    'Protected Overrides Sub ApplyTransform()
    '    Dim group As New Transform3DGroup()
    '    group.Children.Add(New TranslateTransform3D(_posX, _posY, _posZ))
    '    _visual.Transform = group
    'End Sub
End Class
