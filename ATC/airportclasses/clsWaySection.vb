Option Explicit On

<Serializable>
Public Class clsWaySection
    Inherits clsNavigationPath

    Friend ReadOnly Property pointTowardsRamp As clsNavigationPoint
        Get
            Return Me.taxiWayPoint1
        End Get
    End Property
    Friend ReadOnly Property pointAwayFromRamp As clsNavigationPoint
        Get
            Return Me.taxiWayPoint2
        End Get
    End Property

    Public Sub New(ByRef taxiWayPoint1 As clsConnectionPoint, ByRef taxiWayPoint2 As clsConnectionPoint, ByVal type As clsNavigationPath.enumPathWayType, ByVal comment As String, ByRef maxSpeed As clsSpeedCollection, ByVal Name As String, ByVal ObjectID As String, Optional ByVal taxipath As String = Nothing)
        MyBase.New(taxiWayPoint1, taxiWayPoint2, type, comment, maxSpeed, Name, ObjectID, taxipath)

    End Sub
End Class
