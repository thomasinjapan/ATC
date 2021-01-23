Option Explicit On

<Serializable>
Public Class clsGateWaySection
    Inherits clsNavigationPath

    Public Sub New(ByRef taxiWayPoint1 As clsConnectionPoint, ByRef taxiWayPoint2 As clsConnectionPoint, ByVal comment As String, ByRef maxSpeed As clsSpeedCollection, Optional ByVal Name As String = Nothing, Optional taxipath As String = Nothing)
        MyBase.New(taxiWayPoint1, taxiWayPoint2, enumPathWayType.gateWay, comment, maxSpeed, Name, taxipath)
    End Sub
End Class
