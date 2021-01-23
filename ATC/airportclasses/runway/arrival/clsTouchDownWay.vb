Option Explicit On

<Serializable>
Public Class clsTouchDownWay
    Inherits clsWaySection

    Friend ReadOnly Property approachDirection As Double
        Get
            Return Me.directionFrom(taxiWayPoint1)
        End Get
    End Property

    Public Sub New(ByRef arrivalStartPoint As clsTouchDownWayPoint, ByRef arrivalEndPoint As clsTouchDownWayPoint, ByVal comment As String, ByRef maxSpeed As clsSpeedCollection, ByVal ObjectID As String, Optional ByVal Name As String = Nothing, Optional ByVal taxipath As String = Nothing)
        MyBase.New(arrivalStartPoint, arrivalEndPoint, enumPathWayType.touchDownWay, comment, maxSpeed, Name, ObjectID, taxipath)
    End Sub
End Class
