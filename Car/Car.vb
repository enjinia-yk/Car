Imports System.Windows.Media.Media3D
Imports System.Windows.Media

Public Class Car
    ' 車の3Dモデル
    'Private _model As GeometryModel3D
    Private _visual As ModelVisual3D
    Private _posX As Double = 0
    Private _posY As Double = 0
    Private _posZ As Double = 0
    Private _speedX As Double = 0
    Private _speedZ As Double = 0

    Private group As Model3DGroup

    Public Sub New(visual As ModelVisual3D)
        '''_visual = visual
        '''' 車のモデルを作る（立方体で簡易表現）
        '''Dim mesh As New MeshGeometry3D()
        '''mesh.Positions = New Point3DCollection From {
        '''    New Point3D(-1, 0, 1), New Point3D(1, 0, 1), New Point3D(1, 1, 1), New Point3D(-1, 1, 1),
        '''    New Point3D(-1, 0, -1), New Point3D(1, 0, -1), New Point3D(1, 1, -1), New Point3D(-1, 1, -1)
        '''}
        '''mesh.TriangleIndices = New Int32Collection From {
        '''    0, 1, 2, 0, 2, 3,
        '''    1, 5, 6, 1, 6, 2,
        '''    5, 4, 7, 5, 7, 6,
        '''    4, 0, 3, 4, 3, 7,
        '''    3, 2, 6, 3, 6, 7,
        '''    4, 5, 1, 4, 1, 0
        '''}

        '''Dim material As New DiffuseMaterial(New SolidColorBrush(Colors.Red))
        '''Dim geometry As New GeometryModel3D(mesh, material)
        '''_visual.Content = geometry

        '''' 初期位置更新
        '''UpdateTransform()
        '''

        _visual = visual

        ' 3Dモデル全体を格納するGroup
        group = New Model3DGroup()

        ' ===== 車体部分（ボディ） =====
        group.Children.Add(CreateBox(New Point3D(0, 0, 0), 1.2, 0.5, 2.5, Colors.Red))

        ' ===== キャビン部分（上部） =====
        group.Children.Add(CreateBox(New Point3D(0, 0.5, 0), 1.0, 0.5, 1.5, Colors.DarkRed))

        ' ===== タイヤ =====
        Dim tireColor = Colors.Black
        Dim tireY = -0.25
        group.Children.Add(CreateCylinder(New Point3D(-1.1, tireY, 0.6), 0.25, 0.2, tireColor))
        group.Children.Add(CreateCylinder(New Point3D(1.1, tireY, 0.6), 0.25, 0.2, tireColor))
        group.Children.Add(CreateCylinder(New Point3D(-1.1, tireY, -0.6), 0.25, 0.2, tireColor))
        group.Children.Add(CreateCylinder(New Point3D(1.1, tireY, -0.6), 0.25, 0.2, tireColor))

        ' 光源
        group.Children.Add(New AmbientLight(Colors.Gray))
        group.Children.Add(New DirectionalLight(Colors.White, New Vector3D(-1, -1, -1)))

        ' Geometryを設定
        Dim model As New Model3DGroup()
        model.Children.Add(group)
        _visual.Content = model

        '_visual.Transform = New RotateTransform3D(New AxisAngleRotation3D(New Vector3D(0, 1, 0), 90))
    End Sub



    ' ====== 立方体を作る ======
    Private Function CreateBox(center As Point3D, width As Double, height As Double, depth As Double, color As Color) As GeometryModel3D
        Dim hw = width / 2
        Dim hh = height / 2
        Dim hd = depth / 2

        Dim mesh As New MeshGeometry3D()
        mesh.Positions = New Point3DCollection From {
            New Point3D(center.X - hw, center.Y - hh, center.Z + hd),
            New Point3D(center.X + hw, center.Y - hh, center.Z + hd),
            New Point3D(center.X + hw, center.Y + hh, center.Z + hd),
            New Point3D(center.X - hw, center.Y + hh, center.Z + hd),
            New Point3D(center.X - hw, center.Y - hh, center.Z - hd),
            New Point3D(center.X + hw, center.Y - hh, center.Z - hd),
            New Point3D(center.X + hw, center.Y + hh, center.Z - hd),
            New Point3D(center.X - hw, center.Y + hh, center.Z - hd)
        }

        mesh.TriangleIndices = New Int32Collection From {
            0, 1, 2, 0, 2, 3,
            1, 5, 6, 1, 6, 2,
            5, 4, 7, 5, 7, 6,
            4, 0, 3, 4, 3, 7,
            3, 2, 6, 3, 6, 7,
            4, 5, 1, 4, 1, 0
        }

        Dim material As New DiffuseMaterial(New SolidColorBrush(color))
        Return New GeometryModel3D(mesh, material)
    End Function

    ' ====== 簡易円柱（タイヤ）を作る ======
    'Private Function CreateCylinder(center As Point3D, radius As Double, height As Double, color As Color) As GeometryModel3D
    '    Const segs As Integer = 16
    '    Dim mesh As New MeshGeometry3D()
    '    Dim halfH = height / 2

    '    For i = 0 To segs
    '        Dim theta = 2 * Math.PI * i / segs
    '        Dim x = radius * Math.Cos(theta)
    '        Dim z = radius * Math.Sin(theta)

    '        mesh.Positions.Add(New Point3D(center.X + x, center.Y - halfH, center.Z + z))
    '        mesh.Positions.Add(New Point3D(center.X + x, center.Y + halfH, center.Z + z))
    '    Next

    '    For i = 0 To segs - 1
    '        Dim i0 = i * 2
    '        Dim i1 = i0 + 1
    '        Dim i2 = (i0 + 2) Mod (2 * segs)
    '        Dim i3 = (i0 + 3) Mod (2 * segs)
    '        mesh.TriangleIndices.Add(i0)
    '        mesh.TriangleIndices.Add(i2)
    '        mesh.TriangleIndices.Add(i1)
    '        mesh.TriangleIndices.Add(i1)
    '        mesh.TriangleIndices.Add(i2)
    '        mesh.TriangleIndices.Add(i3)
    '    Next

    '    Dim material As New DiffuseMaterial(New SolidColorBrush(color))
    '    Return New GeometryModel3D(mesh, material)
    'End Function

    Private Function CreateCylinder(center As Point3D, radius As Double, height As Double, color As Color) As GeometryModel3D
        Const segs As Integer = 16
        Dim mesh As New MeshGeometry3D()
        Dim halfH = height / 2



        For i = 0 To segs
            Dim theta = 2 * Math.PI * i / segs
            Dim x = radius * Math.Cos(theta)
            Dim z = radius * Math.Sin(theta)

            'mesh.Positions.Add(New Point3D(center.Z + z, center.Y - halfH, -(center.X + x)))
            'mesh.Positions.Add(New Point3D(center.Z + z, center.Y + halfH, -(center.X + x)))

            mesh.Positions.Add(New Point3D(center.Z - halfH, center.Y + z, -(center.X + x)))
            mesh.Positions.Add(New Point3D(center.Z + halfH, center.Y + z, -(center.X + x)))
        Next

        For i = 0 To segs - 1
            Dim i0 = i * 2
            Dim i1 = i0 + 1
            Dim i2 = (i0 + 2) Mod (2 * segs)
            Dim i3 = (i0 + 3) Mod (2 * segs)
            mesh.TriangleIndices.Add(i0)
            mesh.TriangleIndices.Add(i2)
            mesh.TriangleIndices.Add(i1)
            mesh.TriangleIndices.Add(i1)
            mesh.TriangleIndices.Add(i2)
            mesh.TriangleIndices.Add(i3)
        Next

        Dim material As New DiffuseMaterial(New SolidColorBrush(color))
        Return New GeometryModel3D(mesh, material)
    End Function







    Public Sub MoveLeft()
        '_posX -= 0.2
        If _speedZ <> 0 Then
            If _speedX < 0 Then
                _speedX += 0.3
            End If


            If _speedX < 20 Then
                _speedX += 0.1
            Else
                _speedX = 20
            End If

        Else
            _speedX = 0
        End If


        'UpdateTransform()
    End Sub

    Public Sub MoveRight()
        '_posX += 0.2
        If _speedZ <> 0 Then
            If _speedX > 0 Then
                _speedX -= 0.3
            End If

            If _speedX > -20 Then
                _speedX -= 0.1
            Else
                _speedX = -20
            End If
        Else
            _speedX = 0

        End If

        _speedX = Math.Floor(_speedX * (10 ^ 1)) / (10 ^ 1)

        'UpdateTransform()
    End Sub

    Public Sub Accel()
        If _speedZ < 200 Then
            _speedZ += 1
        Else
            _speedZ = 200
        End If
        _speedX = Math.Floor(_speedX * (10 ^ 1)) / (10 ^ 1)
        'UpdateTransform()
    End Sub

    Public Sub Brake()
        If _speedZ > -100 Then
            _speedZ -= 1
        Else
            _speedZ = -100
        End If
        _speedX = Math.Floor(_speedX * (10 ^ 1)) / (10 ^ 1)
        'UpdateTransform()
    End Sub

    Public Overridable Sub MoveForward()
        _speedZ = Math.Floor(_speedZ * (10 ^ 1)) / (10 ^ 1)
        _posX -= _speedX * Math.Abs(_speedZ) * 0.0002
        'If _speedX < 0 Then
        '    _posX *= -1
        'End If
        '_posX -= _speedX * _speedZ * 0.0002
        _posZ -= _speedZ * 0.02
        UpdateTransform()
    End Sub


    ' 速度の慣性・自然減速
    Public Overridable Sub ApplyFriction()
        If _speedZ > 0 Then
            _speedZ -= 0.1  ' ★減速の強さ（調整可）
            If _speedZ < 0 Then _speedZ = 0
        ElseIf _speedZ < 0 Then
            _speedZ += 0.1  ' 後退している場合も減速
            If _speedZ > 0 Then _speedZ = 0
        End If
    End Sub




    Protected Sub UpdateTransform()
        Dim group As New Transform3DGroup()
        'group.Children.Add(New RotateTransform3D(New AxisAngleRotation3D(New Vector3D(0, 1, 0), _rotationY)))
        group.Children.Add(New TranslateTransform3D(_posX, _posY, _posZ))
        _visual.Transform = group

        ' Geometryを設定
        'Dim model As New Model3DGroup()
        'model.Children.Add(group)
        '_visual.Content = model
    End Sub

    ' _posX の値を取得するプロパティ
    Public ReadOnly Property posX As Double
        Get
            Return _posX
        End Get
    End Property

    ' _posY の値を取得するプロパティ
    Public Property posY() As String
        Get
            Return _posY
        End Get
        Set(ByVal value As String)
            _posY = value
        End Set
    End Property

    ' _posZ の値を取得するプロパティ
    Public ReadOnly Property posZ As Double
        Get
            Return _posZ
        End Get
    End Property

    ' _speedZ の値を取得するプロパティ
    Public Property SpeedZ() As String
        Get
            Return _speedZ
        End Get
        Set(ByVal value As String)
            _speedZ = value
        End Set
    End Property


    Public ReadOnly Property Position As Point3D
        Get
            Return New Point3D(_posX, _posY, _posZ)
        End Get
    End Property
    'Public ReadOnly Property Position As Point3D
    '    Get
    '        Return _position
    '    End Get
    'End Property



    '' X方向の位置
    'Public Property PositionX As Double

    '' 移動速度
    'Public Property Speed As Double

    'Public Sub New(model As GeometryModel3D)
    '    _model = model
    '    'PositionX = 0
    '    'Speed = 0.5
    '    UpdatePosition()
    'End Sub

    '' 右へ移動
    'Public Overridable Sub MoveRight()
    '    PositionX += Speed
    '    UpdatePosition()
    'End Sub

    '' 左へ移動
    'Public Overridable Sub MoveLeft()
    '    PositionX -= Speed
    '    UpdatePosition()
    'End Sub

    'Public Sub MoveForward()
    '    _posZ -= _speed
    '    UpdateTransform()
    'End Sub

    '' モデルの位置更新
    'Private Sub UpdatePosition()
    '    Dim transform As New TranslateTransform3D(PositionX, 0, 0)
    '    _model.Transform = transform
    'End Sub
End Class
