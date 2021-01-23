Option Explicit On
Imports System.Device.Location
Imports System.Net.Sockets
Imports System.Xml.XPath

<Serializable>
Public Class clsRamp
    Friend Property Gates As clsGate()
    Friend Property connectionPoints As New List(Of clsConnectionPoint)

    Friend Property taxiPaths As clsPathWay()
    Friend Property gatePaths As clsGatePath()
    Friend Property objectID As String

    Friend Property runwayTaxiPaths As clsRunwayTaxiPath()

    Friend allNavigationPoints As New List(Of clsNavigationPoint)           'list of all navigation points used in this ramp
    Friend allNavigationPaths As New List(Of clsNavigationPath)           'list of all navigation paths used in this ramp
    Friend allPathWays As New List(Of clsPathWay)           'list of all navigation paths used in this ramp

    Friend ReadOnly Property POIs As Dictionary(Of String, clsNavigationPoint)
        Get
            Dim result As New Dictionary(Of String, clsNavigationPoint)

            'search all waypoints first to make sure that runways show up on top
            For C1 As Long = 0 To Me.connectionPoints.Count - 1
                If Me.connectionPoints(C1).isPOIGround Then
                    result.Add(Me.connectionPoints(C1).name.ToLower, Me.connectionPoints(C1))
                End If
            Next
            'search all gates
            For C1 As Long = 0 To Me.Gates.GetUpperBound(0)
                If Me.Gates(C1).isPOIGround Then
                    result.Add(Me.Gates(C1).name.ToLower, Me.Gates(C1))
                End If
            Next



            Return result
        End Get
    End Property


    Friend ReadOnly Property mostTop As Double
        Get
            Dim tmpMostTop As Double = Double.PositiveInfinity

            For Each gate As clsGate In Me.Gates
                If gate.pos_Y < tmpMostTop Then tmpMostTop = gate.pos_Y
            Next

            For Each taxiWayPoint As clsConnectionPoint In Me.connectionPoints
                If taxiWayPoint.pos_Y < tmpMostTop Then tmpMostTop = taxiWayPoint.pos_Y
            Next

            Return tmpMostTop
        End Get
    End Property
    Friend ReadOnly Property mostLeft As Double
        Get
            Dim tmpMostLeft As Double = Double.PositiveInfinity

            For Each gate As clsGate In Me.Gates
                If gate.pos_X < tmpMostLeft Then tmpMostLeft = gate.pos_X
            Next

            For Each taxiWayPoint As clsConnectionPoint In Me.connectionPoints
                If taxiWayPoint.pos_X < tmpMostLeft Then tmpMostLeft = taxiWayPoint.pos_X
            Next

            Return tmpMostLeft
        End Get
    End Property
    Friend ReadOnly Property mostBottom As Double
        Get
            Dim tmpMostBottom As Double = Double.NegativeInfinity

            For Each gate As clsGate In Me.Gates
                If gate.pos_Y > tmpMostBottom Then tmpMostBottom = gate.pos_Y
            Next

            For Each taxiWayPoint As clsConnectionPoint In Me.connectionPoints
                If taxiWayPoint.pos_Y > tmpMostBottom Then tmpMostBottom = taxiWayPoint.pos_Y
            Next

            Return tmpMostBottom
        End Get
    End Property
    Friend ReadOnly Property mostRight As Double
        Get
            Dim tmpMostRight As Double = Double.NegativeInfinity

            For Each gate As clsGate In Me.Gates
                If gate.pos_X > tmpMostRight Then tmpMostRight = gate.pos_X
            Next

            For Each taxiWayPoint As clsConnectionPoint In Me.connectionPoints
                If taxiWayPoint.pos_X > tmpMostRight Then tmpMostRight = taxiWayPoint.pos_X
            Next

            Return tmpMostRight
        End Get
    End Property

    Public Sub New(ByRef xElement As XElement, ByRef reference As clsAirport.structGeoCoordinate)
        Dim referenceCoordinate As New GeoCoordinate(reference.lat, reference.lng)

        ' ReDim Me.taxiWays(xElement.<taxiway>.Count - 1)
        ReDim Me.Gates(xElement.<gate>.Count - 1)
        'ReDim Me.connectionPoints(xElement.<connectionpoint>.Count - 1)
        ReDim Me.gatePaths(xElement.<gatepath>.Count - 1)
        '  ReDim Me.runwayTaxiWays(xElement.<runwaytaxiway>.Count - 1)
        ReDim Me.taxiPaths(xElement.<taxipath>.Count - 1)
        ReDim Me.runwayTaxiPaths(xElement.<runwaytaxipath>.Count - 1)

        'gates
        For C1 As Long = 0 To xElement.<gate>.Count - 1
            Me.Gates(C1) = New clsGate(xElement.<gate>(C1), reference)
        Next

        ' taxiwaypoints
        For C1 As Long = 0 To xElement.<connectionpoint>.Count - 1
            Me.connectionPoints.Add(New clsConnectionPoint(xElement.<connectionpoint>(C1), reference))
        Next

        'taxipaths with multiple sections
        For C1 As Long = 0 To xElement.<taxipath>.Count - 1
            Me.taxiPaths(C1) = New clsPathWay(Me.getPointByID(xElement.<taxipath>(C1).@connectionpoint1),
                                              Me.getPointByID(xElement.<taxipath>(C1).@connectionpoint2),
                                              clsNavigationPath.enumPathWayType.taxiWay,
                                              xElement.<taxipath>(C1),
                                              reference)

            'add all points in the points to connectionpointslist
            For Each singlePoint As clsConnectionPoint In Me.taxiPaths(C1).wayPoints
                Me.connectionPoints.add(singlePoint)
            Next

            Me.allNavigationPaths.AddRange(Me.taxiPaths(C1).allNavigationPaths)
            Me.allNavigationPoints.AddRange(Me.taxiPaths(C1).allNavigationPoints)
        Next

        'gatepaths
        'for the time being, treat them like normal taxiways
        For C1 As Long = 0 To xElement.<gatepath>.Count - 1
            Me.gatePaths(C1) = New clsGatePath(Me.getPointByID(xElement.<gatepath>(C1).@connectionpoint),
                                            Me.getPointByID(xElement.<gatepath>(C1).@gate),
                                            xElement.<gatepath>(C1), reference)

            'add all points in the points to connectionpointslist
            For Each singlePoint As clsConnectionPoint In Me.gatePaths(C1).wayPoints
                Me.connectionPoints.Add(singlePoint)
            Next

            Me.allNavigationPaths.AddRange(Me.gatePaths(C1).allNavigationPaths)
            Me.allNavigationPoints.AddRange(Me.gatePaths(C1).allNavigationPoints)
        Next

        'runwaytaxipaths with multiple sections
        For C1 As Long = 0 To xElement.<runwaytaxipath>.Count - 1
            Me.runwayTaxiPaths(C1) = New clsRunwayTaxiPath(Me.getPointByID(xElement.<runwaytaxipath>(C1).@runwaypointing),
                                              Me.getPointByID(xElement.<runwaytaxipath>(C1).@ramppointing),
                                              xElement.<runwaytaxipath>(C1),
                                              reference)

            'add all points in the points to connectionpointslist
            For Each singlePoint As clsConnectionPoint In Me.runwayTaxiPaths(C1).wayPoints
                Me.connectionPoints.Add(singlePoint)
            Next

            Me.allNavigationPaths.AddRange(Me.runwayTaxiPaths(C1).allNavigationPaths)
            Me.allNavigationPoints.AddRange(Me.runwayTaxiPaths(C1).allNavigationPoints)
        Next

        Me.allNavigationPoints.AddRange(Me.connectionPoints)
        Me.allNavigationPoints.AddRange(Me.Gates)

        Me.allPathWays.AddRange(Me.taxiPaths)
        Me.allPathWays.AddRange(Me.gatePaths)
        Me.allPathWays.AddRange(Me.runwayTaxiPaths)

        Me.objectID = xElement.@id
    End Sub

    Friend Function getGateByID(ByVal id As String) As clsGate
        Dim result As clsGate = Nothing

        For C1 As Long = 0 To Me.Gates.GetUpperBound(0)
            If Me.Gates(C1).objectID = id Then
                result = Me.Gates(C1)
                Exit For
            End If
        Next

        Return result
    End Function

    Friend Function getConnectionPointByName(ByVal name As String) As clsConnectionPoint
        Dim result As clsConnectionPoint = Nothing

        For Each singleConnectionPoint As clsConnectionPoint In Me.connectionPoints
            If singleConnectionPoint.name.ToLower = name.ToLower Then
                result = singleConnectionPoint
                Exit For
            End If
        Next

        Return result
    End Function

    Friend Function getPointByID(ByVal ID As String) As clsConnectionPoint
        Dim result As clsConnectionPoint = Nothing

        For C1 As Long = 0 To Me.connectionPoints.Count - 1
            If Me.connectionPoints(C1).objectID = ID Then
                result = Me.connectionPoints(C1)
                Exit For
            End If
        Next

        For C1 As Long = 0 To Me.Gates.GetUpperBound(0)
            If Me.Gates(C1).objectID = ID Then
                result = Me.Gates(C1)
                Exit For
            End If
        Next

        Return result
    End Function


    Friend Function getPOIbyName(ByVal name As String) As clsConnectionPoint
        Dim result As clsConnectionPoint = Me.POIs(name)

        Return result
    End Function


End Class
