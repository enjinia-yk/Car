Imports System.Windows.Media.Media3D

Public Class Ground
    Private _visual As ModelVisual3D
    Private _offsetZ As Double = 0

    Public Sub New(visual As ModelVisual3D)
        _visual = visual
        CreateGround()
    End Sub

    Private Sub CreateGround()
        ' MeshGeometry3D で平面を作成（長方形 20x0.1x50）
        Dim mesh As New MeshGeometry3D()
        mesh.Positions = New Point3DCollection From {
            New Point3D(-10, -1.2, -5000), New Point3D(10, -1.2, -5000),
            New Point3D(10, -1.2, 50), New Point3D(-10, -1.2, 50)
        }
        mesh.TriangleIndices = New Int32Collection From {
           0, 2, 1, 0, 3, 2
        }

        Dim material As New DiffuseMaterial(New SolidColorBrush(Colors.Gray))
        Dim geometry As New GeometryModel3D(mesh, material)


        ' ==== 中央の白い点線 ====
        Dim lineGroup As New Model3DGroup()
        Dim segmentLength As Double = 5.0     ' 点線1つの長さ
        Dim gap As Double = 3.0               ' 点線の間隔
        Dim z As Double = -5000

        While z < 500
            ' 1つの白線を作成（幅0.3m、高さわずかに浮かせる）
            Dim lineMesh As New MeshGeometry3D()
            lineMesh.Positions = New Point3DCollection From {
                New Point3D(-0.15, -1.19, z),
                New Point3D(0.15, -1.19, z),
                New Point3D(0.15, -1.19, z + segmentLength),
                New Point3D(-0.15, -1.19, z + segmentLength)
            }
            lineMesh.TriangleIndices = New Int32Collection From {0, 2, 1, 0, 3, 2}
            Dim lineMaterial As New DiffuseMaterial(New SolidColorBrush(Colors.White))
            lineGroup.Children.Add(New GeometryModel3D(lineMesh, lineMaterial))

            z += segmentLength + gap
        End While

        ' ==== まとめて設定 ====
        Dim group As New Model3DGroup()
        group.Children.Add(geometry) ' 地面
        group.Children.Add(lineGroup)      ' 点線群

        _visual.Content = group

        ' ModelVisual3D に設定
        '_visual.Content = geometry

    End Sub

    'Public Sub Update(carZ As Double)
    '    ' 車に追従して地面を動かす
    '    _offsetZ = -carZ
    '    _visual.Transform = New TranslateTransform3D(0, 0, _offsetZ)
    'End Sub
End Class
