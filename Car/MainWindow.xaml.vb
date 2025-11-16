Imports System.Windows.Media.Media3D
Imports System.Windows.Media
Imports System.Windows.Threading
Imports System.Text.RegularExpressions

Class MainWindow

    'Private carManager As New CarManager()
    Private carManager As New FlyingCarManager()

    'Private selectCar As Car
    'Private myCar As Car
    Private ground As Ground
    Private timer As DispatcherTimer

    ' クラス内のフィールドとして追加
    Private moveTimer As DispatcherTimer
    Private currentAction As Action = Nothing

    ' --- 複数キー同時押し管理用 ---
    Private pressedKeys As New HashSet(Of Key)


    Public Sub New()
        InitializeComponent()
        AddHandler Me.Loaded, AddressOf Window_Loaded
    End Sub

    Private Sub Window_Loaded(sender As Object, e As RoutedEventArgs) ' Handles Me.Loaded
        ' 車の3Dモデルを作成（立方体で簡易表現）
        'Dim mesh As New MeshGeometry3D()
        'mesh.Positions = New Point3DCollection From {
        '    New Point3D(-1, -0.5, 1), New Point3D(1, -0.5, 1), New Point3D(1, 0.5, 1), New Point3D(-1, 0.5, 1),
        '    New Point3D(-1, -0.5, -1), New Point3D(1, -0.5, -1), New Point3D(1, 0.5, -1), New Point3D(-1, 0.5, -1)
        '}
        'mesh.TriangleIndices = New Int32Collection From {
        '    0, 1, 2, 0, 2, 3,  ' 前面
        '    1, 5, 6, 1, 6, 2,  ' 右面
        '    5, 4, 7, 5, 7, 6,  ' 背面
        '    4, 0, 3, 4, 3, 7,  ' 左面
        '    3, 2, 6, 3, 6, 7,  ' 上面
        '    4, 5, 1, 4, 1, 0   ' 底面
        '}

        'Dim material As New DiffuseMaterial(New SolidColorBrush(Colors.Red))
        'Dim geometry As New GeometryModel3D(mesh, material)

        ''Dim transform As New TranslateTransform3D(0, 0, -20)
        ''geometry.Transform = transform


        '' Viewport3Dに追加
        'carModel.Content = geometry

        '' Carクラスのインスタンス作成
        'myCar = New Car(Geometry)

        ' 車モデルの作成
        'carManager.SelectedCar = New Car(carModel)
        ' 地面モデルの作成
        ground = New Ground(groundModel)
        'ground.Update(myCar._posZ)

        ' ボタン操作
        ''''AddHandler btnLeft.Click, Sub() myCar.MoveLeft()
        ''''AddHandler btnRight.Click, Sub() myCar.MoveRight()
        ''''AddHandler btnAccel.Click, Sub() myCar.Accel()
        ''''AddHandler btnBrake.Click, Sub() myCar.Brake()
        'AddHandler btnAccel.Click, Sub() MoveForwardCamera()
        'AddHandler btnBrake.Click, Sub() MoveForwardCamera()


        ' タイマー初期化（例：0.01秒ごとに実行）
        moveTimer = New DispatcherTimer()
        moveTimer.Interval = TimeSpan.FromMilliseconds(10)
        AddHandler moveTimer.Tick, Sub()
                                       If currentAction IsNot Nothing Then
                                           currentAction.Invoke()
                                       End If
                                   End Sub



        ' *****************************************
        ' 「空飛ぶ車を出す」ボタン
        AddHandler btnSpawnFlyingCar.Click, Sub()
                                                Dim newCarModel As New ModelVisual3D()
                                                viewport.Children.Add(newCarModel)

                                                Dim newCar As New FlyingCar(newCarModel)
                                                carManager.AddCar(newCar)

                                                ' 車選択用ボタンを作成
                                                Dim carButton As New Button With {
                                              .Content = "空飛ぶ車 " & carManager.Cars.Count,
                                              .Width = 60,
                                              .Margin = New Thickness(2)
                                          }
                                                AddHandler carButton.Click, Sub()
                                                                                carManager.SelectedCar = newCar
                                                                            End Sub
                                                carManager.SelectedCar = newCar
                                                spCarButtons.Children.Add(carButton)
                                            End Sub

        AddHandler btnAscent.PreviewMouseLeftButtonDown, Sub()
                                                               If carManager.SelectedCar IsNot Nothing Then
                                                                 currentAction = Sub() carManager.SelectedCar.Ascend()
                                                                 moveTimer.Start()
                                                               End If
                                                           End Sub
        AddHandler btnAscent.PreviewMouseLeftButtonUp, Sub()
                                                           If carManager.SelectedCar IsNot Nothing Then
                                                               moveTimer.Stop()
                                                               currentAction = Nothing
                                                               carManager.SelectedCar.speedY = 0
                                                           End If
                                                       End Sub

        AddHandler btnDescent.PreviewMouseLeftButtonDown, Sub()
                                                              If carManager.SelectedCar IsNot Nothing Then
                                                                  currentAction = Sub() carManager.SelectedCar.Descend()
                                                                  moveTimer.Start()
                                                              End If
                                                          End Sub
        AddHandler btnDescent.PreviewMouseLeftButtonUp, Sub()
                                                            If carManager.SelectedCar IsNot Nothing Then
                                                                moveTimer.Stop()
                                                                currentAction = Nothing
                                                                carManager.SelectedCar.speedY = 0
                                                            End If
                                                        End Sub
        ' *****************************************


        ' 「車を出す」ボタン
        'AddHandler btnSpawnCar.Click, Sub()
        '                                  Dim newCarModel As New ModelVisual3D()
        '                                  viewport.Children.Add(newCarModel)

        '                                  Dim newCar As New Car(newCarModel)
        '                                  carManager.AddCar(newCar)

        '                                  ' 車選択用ボタンを作成
        '                                  Dim carButton As New Button With {
        '                                      .Content = "車 " & carManager.Cars.Count,
        '                                      .Width = 60,
        '                                      .Margin = New Thickness(2)
        '                                  }
        '                                  AddHandler carButton.Click, Sub()
        '                                                                  carManager.SelectedCar = newCar
        '                                                              End Sub
        '                                  carManager.SelectedCar = newCar
        '                                  spCarButtons.Children.Add(carButton)
        '                              End Sub


        ' 各ボタンにイベント登録
        AddHandler btnLeft.PreviewMouseLeftButtonDown, Sub()
                                                           If carManager.SelectedCar IsNot Nothing Then
                                                               currentAction = Sub() carManager.SelectedCar.MoveLeft()
                                                               moveTimer.Start()
                                                           End If
                                                       End Sub
        AddHandler btnLeft.PreviewMouseLeftButtonUp, Sub()
                                                         If carManager.SelectedCar IsNot Nothing Then
                                                             moveTimer.Stop()
                                                             currentAction = Nothing
                                                         End If
                                                     End Sub

        AddHandler btnRight.PreviewMouseLeftButtonDown, Sub()
                                                            If carManager.SelectedCar IsNot Nothing Then
                                                                currentAction = Sub() carManager.SelectedCar.MoveRight()
                                                                moveTimer.Start()
                                                            End If
                                                        End Sub
        AddHandler btnRight.PreviewMouseLeftButtonUp, Sub()
                                                          If carManager.SelectedCar IsNot Nothing Then
                                                              moveTimer.Stop()
                                                              currentAction = Nothing
                                                          End If
                                                      End Sub

        AddHandler btnAccel.PreviewMouseLeftButtonDown, Sub()
                                                            If carManager.SelectedCar IsNot Nothing Then
                                                                currentAction = Sub() carManager.SelectedCar.Accel()
                                                                moveTimer.Start()
                                                            End If
                                                        End Sub
        AddHandler btnAccel.PreviewMouseLeftButtonUp, Sub()
                                                          If carManager.SelectedCar IsNot Nothing Then
                                                              moveTimer.Stop()
                                                              currentAction = Nothing
                                                          End If
                                                      End Sub

        AddHandler btnBrake.PreviewMouseLeftButtonDown, Sub()
                                                            If carManager.SelectedCar IsNot Nothing Then
                                                                currentAction = Sub() carManager.SelectedCar.Brake()
                                                                moveTimer.Start()
                                                            End If
                                                        End Sub
        AddHandler btnBrake.PreviewMouseLeftButtonUp, Sub()
                                                          If carManager.SelectedCar IsNot Nothing Then
                                                              moveTimer.Stop()
                                                              currentAction = Nothing
                                                          End If
                                                      End Sub

        AddHandler Me.KeyDown, AddressOf OnKeyDownHandler
        AddHandler Me.KeyUp, AddressOf OnKeyUpHandler


        ' タイマー設定（0.01秒ごと）
        timer = New DispatcherTimer()
        timer.Interval = TimeSpan.FromMilliseconds(10)
        AddHandler timer.Tick, AddressOf OnTick
        timer.Start()
    End Sub

    Private Sub OnTick(sender As Object, e As EventArgs)
        'carManager.SelectedCar.MoveForward()
        If carManager.SelectedCar IsNot Nothing Then
            ' --- 同時押し対応: キーの組み合わせで処理 ---
            If pressedKeys.Contains(Key.W) Then
                carManager.SelectedCar.Accel()
            End If

            If pressedKeys.Contains(Key.S) Then
                carManager.SelectedCar.Brake()
            End If

            If pressedKeys.Contains(Key.A) Then
                carManager.SelectedCar.MoveLeft()
            End If

            If pressedKeys.Contains(Key.D) Then
                carManager.SelectedCar.MoveRight()
            End If

        End If



        '  すべての車を動かす
        For Each c In carManager.Cars
            If Not pressedKeys.Contains(Key.W) AndAlso Not pressedKeys.Contains(Key.S) Then
                c.ApplyFriction()
            End If
            c.MoveForward()
        Next


        If carManager.SelectedCar IsNot Nothing Then
            ' 車を前後移動させてカメラ追従(カメラ追従は選択中の車だけでOK)
            Dim carPos = carManager.SelectedCar.Position
            Dim camOffset As New Vector3D(0, 10, 20) ' 車の背後上方
            camera.Position = New Point3D(carPos.X + camOffset.X, carPos.Y + camOffset.Y, carPos.Z + camOffset.Z)
            camera.LookDirection = New Vector3D(0, -5, -20)

            'lblSpeed.Content = $"Speed: {carManager.SelectedCar.SpeedZ:F2}"
            lblSpeed.Content = $"Speed: {carManager.SelectedCar.SpeedZ:F2}"
            lblPosY.Content = $"PosY  : {carManager.SelectedCar.posY:F2}"
        End If


        'camera.LookDirection = New Vector3D(carPos.X - camera.Position.X, carPos.Y - camera.Position.Y, carPos.Z - camera.Position.Z)
    End Sub

    ' キーボード対応
    Private Sub OnKeyDownHandler(sender As Object, e As KeyEventArgs)
        'If currentAction IsNot Nothing Then Return ' 既に押されているキーがある場合は無視
        If carManager.SelectedCar IsNot Nothing Then
            '    Select Case e.Key
            '        Case Key.A
            '            currentAction = Sub() carManager.SelectedCar.MoveLeft()
            '            moveTimer.Start()

            '        Case Key.D
            '            currentAction = Sub() carManager.SelectedCar.MoveRight()
            '            moveTimer.Start()

            '        Case Key.W
            '            currentAction = Sub() carManager.SelectedCar.Accel()
            '            moveTimer.Start()

            '        Case Key.S
            '            currentAction = Sub() carManager.SelectedCar.Brake()
            '            moveTimer.Start()
            '    End Select

            pressedKeys.Add(e.Key)
        End If
        'pressedKeys.Add(e.Key)
    End Sub

    Private Sub OnKeyUpHandler(sender As Object, e As KeyEventArgs)
        If carManager.SelectedCar IsNot Nothing Then
            'Select Case e.Key
            '    Case Key.A, Key.D, Key.W, Key.S
            '        moveTimer.Stop()
            '        currentAction = Nothing
            'End Select

            If pressedKeys.Contains(e.Key) Then
                pressedKeys.Remove(e.Key)
            End If
        End If
    End Sub

    ' 車を前後移動させてカメラ追従
    'Private Sub MoveForwardCamera()
    '    myCar.MoveForward() ' 前進
    '    ' カメラ追従
    '    Dim carPos = myCar.Position
    '    Dim camOffset As New Vector3D(0, 3, 10) ' 車の背後上方
    '    camera.Position = New Point3D(carPos.X + camOffset.X, carPos.Y + camOffset.Y, carPos.Z + camOffset.Z)
    '    camera.LookDirection = New Vector3D(carPos.X - camera.Position.X, carPos.Y - camera.Position.Y, carPos.Z - camera.Position.Z)
    'End Sub

    'Private Sub btnLeft_Click(sender As Object, e As RoutedEventArgs) Handles btnLeft.Click
    '    myCar.MoveLeft()
    'End Sub

    'Private Sub btnRight_Click(sender As Object, e As RoutedEventArgs) Handles btnRight.Click
    '    myCar.MoveRight()
    'End Sub

    'Private Sub btnForward_Click(sender As Object, e As RoutedEventArgs) Handles btnRight.Click
    '    myCar.MoveForward()
    'End Sub
End Class
