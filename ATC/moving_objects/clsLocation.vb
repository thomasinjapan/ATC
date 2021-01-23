Option Explicit On

<Serializable>
Public Class clsLocation
    Friend Property X As clsDistanceCollection
    Friend Property Y As clsDistanceCollection

    Public Sub New(ByVal X As clsDistanceCollection, ByVal Y As clsDistanceCollection)
        Me.X = X
        Me.Y = Y
    End Sub

    Public Sub New()
        Me.X = New clsDistanceCollection(0, clsDistanceCollection.enumDistanceUnits.meters)
        Me.Y = New clsDistanceCollection(0, clsDistanceCollection.enumDistanceUnits.meters)
    End Sub


End Class
