Option Explicit On
Imports System.Device.Location

<Serializable>
Public Class clsPathWay
    Friend ReadOnly Property entryPoint As clsConnectionPoint
    Friend ReadOnly Property wayPoints As clsConnectionPoint()
    Friend ReadOnly Property exitPoint As clsConnectionPoint
    Friend ReadOnly Property waySections As New List(Of clsWaySection)

    Friend ReadOnly Property comment As String
    Friend ReadOnly Property Name As String
    Friend ReadOnly Property maxSpeed As clsSpeedCollection
    Friend ReadOnly Property ObjectID As String
    Friend ReadOnly Property type As clsWaySection.enumPathWayType
    Friend ReadOnly Property taxiPath As String

    Friend allNavigationPoints As New List(Of clsNavigationPoint)           'list of all navigation points used in this pathway
    Friend allNavigationPaths As New List(Of clsWaySection)           'list of all navigation points used in this pathway



    Friend ReadOnly Property mostTop As Double
        Get
            Dim tmpMostTop As Double = Double.PositiveInfinity

            'check the startpoint
            If Me.entryPoint.pos_Y < tmpMostTop Then tmpMostTop = Me.entryPoint.pos_Y

            'check the endpoint
            If Me.exitPoint.pos_Y < tmpMostTop Then tmpMostTop = Me.exitPoint.pos_Y

            'check all waypoints

            'departurepoints
            For Each singleWayPoint As clsConnectionPoint In Me.wayPoints
                If singleWayPoint.pos_Y < tmpMostTop Then tmpMostTop = singleWayPoint.pos_Y
            Next

            Return tmpMostTop
        End Get
    End Property
    Friend ReadOnly Property mostLeft As Double
        Get
            Dim tmpMostLeft As Double = Double.PositiveInfinity

            'check the startpoint
            If Me.entryPoint.pos_X < tmpMostLeft Then tmpMostLeft = Me.entryPoint.pos_X

            'check the endpoint
            If Me.exitPoint.pos_X < tmpMostLeft Then tmpMostLeft = Me.exitPoint.pos_X

            'check all waypoints

            'departurepoints
            For Each singleWayPoint As clsConnectionPoint In Me.wayPoints
                If singleWayPoint.pos_X < tmpMostLeft Then tmpMostLeft = singleWayPoint.pos_X
            Next

            Return tmpMostLeft
        End Get
    End Property
    Friend ReadOnly Property mostBottom As Double
        Get
            Dim tmpMostBottom As Double = Double.NegativeInfinity

            'check the startpoint
            If Me.entryPoint.pos_Y > tmpMostBottom Then tmpMostBottom = Me.entryPoint.pos_Y

            'check the endpoint
            If Me.exitPoint.pos_Y > tmpMostBottom Then tmpMostBottom = Me.exitPoint.pos_Y

            'check all waypoints

            'departurepoints
            For Each singleWayPoint As clsConnectionPoint In Me.wayPoints
                If singleWayPoint.pos_Y > tmpMostBottom Then tmpMostBottom = singleWayPoint.pos_Y
            Next

            Return tmpMostBottom
        End Get
    End Property
    Friend ReadOnly Property mostRight As Double
        Get
            Dim tmpMostRight As Double = Double.NegativeInfinity

            'check the startpoint
            If Me.entryPoint.pos_X > tmpMostRight Then tmpMostRight = Me.entryPoint.pos_X

            'check the endpoint
            If Me.exitPoint.pos_X > tmpMostRight Then tmpMostRight = Me.exitPoint.pos_X

            'check all waypoints

            'departurepoints
            For Each singleWayPoint As clsConnectionPoint In Me.wayPoints
                If singleWayPoint.pos_X > tmpMostRight Then tmpMostRight = singleWayPoint.pos_X
            Next

            Return tmpMostRight
        End Get
    End Property


    Public Sub New(ByRef entryPoint As clsConnectionPoint, ByRef exitPoint As clsConnectionPoint, ByVal type As clsWaySection.enumPathWayType, ByRef xElement As XElement, ByRef referencecoordinate As clsAirport.structGeoCoordinate)
        Me.comment = xElement.@comment
        Me.Name = xElement.@name
        Me.taxipath = xElement.@taxipath
        Me.maxSpeed = New clsSpeedCollection(xElement.@maxspeed)
        'Me.ObjectID = xElement.@id
        Me.ObjectID = Guid.NewGuid.ToString
        Me.type = type

        'get first point
        Me.entryPoint = entryPoint

        'get sections in between
        ReDim Me.wayPoints(xElement.<waypoint>.Count - 1)
        For C1 As Long = 0 To xElement.<waypoint>.Count - 1
            Dim newWayPoint As New clsConnectionPoint(xElement.<waypoint>(C1), referencecoordinate)
            Me.wayPoints(C1) = newWayPoint
        Next

        Me.allNavigationPoints.Add(entryPoint)
        Me.allNavigationPoints.AddRange(Me.wayPoints)
        Me.allNavigationPoints.Add(exitPoint)

        'get last point
        Me.exitPoint = exitPoint

        'create sections
        'add startpoint to first point in list
        Dim wayPointPointer As Long = 0
        Dim currentStartPoint As clsConnectionPoint = Me.entryPoint
        Dim currentEndPoint As clsConnectionPoint = Nothing

        'add next point in list with point after that and move forward
        While (Me.wayPoints.Count > 0) And (wayPointPointer < Me.wayPoints.Count)
            currentEndPoint = Me.wayPoints(wayPointPointer)

            'need temporary varialble to prevent copyuing only the pointer
            Dim tmpWayPointStart As clsConnectionPoint = currentStartPoint
            Dim tmpWayPointEnd As clsConnectionPoint = currentEndPoint

            Dim newSection As New clsWaySection(tmpWayPointStart, tmpWayPointEnd, type, Me.comment, Me.maxSpeed, Me.Name, Guid.NewGuid.ToString, Me.taxiPath)
            Me.waySections.Add(newSection)

            currentStartPoint = currentEndPoint
            wayPointPointer += 1
        End While

        'add last point to exit point
        Dim finalSection As New clsWaySection(currentStartPoint, Me.exitPoint, type, Me.comment, Me.maxSpeed, Me.Name, Guid.NewGuid.ToString, Me.taxiPath)
        Me.waySections.Add(finalSection)

        Me.allNavigationPaths.AddRange(Me.waySections)

    End Sub
End Class
