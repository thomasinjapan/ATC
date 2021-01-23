Option Explicit On

<Serializable>
Public Class clsNavigationPath

    Public Enum enumPathWayType As Long
        undefined = 0
        exitWay
        lineUpWay
        gateWay
        AirWay
        touchDownWay
        takeOffWay
        runwayTaxiWay
        taxiWay
    End Enum

    Friend Property name As String
    Friend Property maxSpeed As clsSpeedCollection
    Friend Property comment As String
    Friend ReadOnly Property type As enumPathWayType
    Friend ReadOnly Property taxipath As String
    Friend ReadOnly Property ObjectID As String

    Friend ReadOnly Property taxiWayPoint1 As clsConnectionPoint
    Friend ReadOnly Property taxiWayPoint2 As clsConnectionPoint

    Public Sub New(ByRef taxiWayPoint1 As clsConnectionPoint, ByRef taxiWayPoint2 As clsConnectionPoint, ByVal type As clsNavigationPath.enumPathWayType, ByVal comment As String, ByRef maxSpeed As clsSpeedCollection, ByVal Name As String, ByVal ObjectID As String, Optional ByVal taxipath As String = Nothing)
        Me.taxiWayPoint1 = taxiWayPoint1
        Me.taxiWayPoint1.addTaxiWay(Me)

        Me.taxiWayPoint2 = taxiWayPoint2
        Me.taxiWayPoint2.addTaxiWay(Me)

        Me.maxSpeed = maxSpeed
        Me.comment = comment
        Me.name = Name
        Me.type = type
        Me.taxipath = taxipath

        Me.ObjectID = ObjectID
    End Sub


    ''' <summary>
    ''' returns the opposite TaxiWayPoint byref
    ''' </summary>
    ''' <param name="taxiwaypoint">the waypoint we search the oposite for</param>
    ''' <returns></returns>
    Friend Function oppositeTaxiWayPoint(ByRef taxiwaypoint As clsConnectionPoint) As clsConnectionPoint
        If taxiwaypoint Is taxiWayPoint1 Then
            Return taxiWayPoint2
        Else
            Return taxiWayPoint1
        End If
    End Function

    Friend Function directionFrom(ByRef startTaxiWayPoint As clsConnectionPoint) As Double
        Dim result As Double = Nothing

        Dim beginTaxiWayPoint As clsConnectionPoint = Nothing
        Dim endTaxiWayPoint As clsConnectionPoint = Nothing

        If startTaxiWayPoint Is taxiWayPoint1 Then
            beginTaxiWayPoint = Me.taxiWayPoint1
            endTaxiWayPoint = Me.taxiWayPoint2
        ElseIf startTaxiWayPoint Is taxiWayPoint2 Then
            beginTaxiWayPoint = taxiWayPoint2
            endTaxiWayPoint = taxiWayPoint1
        Else
            'do nothing and return nothing
        End If

        result = directionbetweenpoints(beginTaxiWayPoint, endTaxiWayPoint)

        Return result
    End Function

    Friend Function directionTo(ByRef goalTaxiWayPoint As clsConnectionPoint) As Double
        Dim result As Double = Nothing

        Dim beginTaxiWayPoint As clsConnectionPoint = Nothing
        Dim endTaxiWayPoint As clsConnectionPoint = Nothing

        If goalTaxiWayPoint Is Me.taxiWayPoint1 Then
            beginTaxiWayPoint = Me.taxiWayPoint1
            endTaxiWayPoint = Me.taxiWayPoint2
        ElseIf goalTaxiWayPoint Is taxiWayPoint2 Then
            beginTaxiWayPoint = taxiWayPoint2
            endTaxiWayPoint = taxiWayPoint1
        Else
            'do nothing and return nothing
        End If

        result = directionbetweenpoints(endTaxiWayPoint, beginTaxiWayPoint)

        Return result
    End Function

    Friend Shared Function directionbetweenpoints(ByRef point1 As clsConnectionPoint, ByRef point2 As clsConnectionPoint) As Double
        Dim result As Double = directionbetweenpoints(point1.pos_X, point1.pos_Y, point2.pos_X, point2.pos_Y)
        Return result
    End Function

    Public Shared Function directionbetweenpoints(ByVal x1 As Double, ByVal y1 As Double, ByVal x2 As Double, ByVal y2 As Double) As Double

        'stolen from here: https://math.stackexchange.com/questions/707673/find-angle-in-degrees-from-one-point-to-another-in-2d-space/2587852

        '!!!changed x and y for reasons I don't understand and changed y2 and y1 because directions are flipped
        'Dim result As Double = Math.Atan2(y2 - y1, x2 - x1)
        Dim result As Double = Math.Atan2(x2 - x1, y1 - y2)

        'convert from rad to degree
        result = result * 180 / Math.PI

        'make value something between 0 and 359.999
        result = (result + 360) Mod 360

        Return result
    End Function

    Friend ReadOnly Property length() As Double
        Get
            Return (Math.Sqrt(((Me.taxiWayPoint1.pos_X - Me.taxiWayPoint2.pos_X) ^ 2) + ((Me.taxiWayPoint1.pos_Y - Me.taxiWayPoint2.pos_Y) ^ 2)))
        End Get
    End Property

End Class
