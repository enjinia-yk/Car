Public Class FlyingCarManager
    Private _cars As New List(Of FlyingCar)
    Public Property SelectedCar As FlyingCar = Nothing

    Public ReadOnly Property Cars As List(Of FlyingCar)
        Get
            Return _cars
        End Get
    End Property

    Public Sub AddCar(car As FlyingCar)
        _cars.Add(car)
        ' 最初の車なら自動選択
        If SelectedCar Is Nothing Then SelectedCar = car
    End Sub
End Class
