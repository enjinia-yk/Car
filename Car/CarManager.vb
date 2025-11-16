Public Class CarManager
    Private _cars As New List(Of Car)
    Public Property SelectedCar As Car = Nothing

    Public ReadOnly Property Cars As List(Of Car)
        Get
            Return _cars
        End Get
    End Property

    Public Sub AddCar(car As Car)
        _cars.Add(car)
        ' 最初の車なら自動選択
        If SelectedCar Is Nothing Then SelectedCar = car
    End Sub
End Class
