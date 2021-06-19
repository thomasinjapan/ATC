Option Explicit On
Imports System.Device.Location
Imports System.Runtime.Remoting.Messaging
Imports Microsoft.VisualBasic.CompilerServices

<Serializable>
Public Class clsRunWay

    Friend ReadOnly Property objectID As String
    Friend ReadOnly Property name As String

    Friend Property takeOffPoints As clsTakeOffPoint()
    Friend Property lineUpPaths As clsLineUpWay()
    Friend Property takeOffPaths As clsTakeOffPath()

    Friend Property FINALs As New List(Of clsAirPath)
    Friend Property STARs As clsAirPath()
    Friend Property SIDs As clsAirPath()
    Friend Property touchDownWayPoints As clsTouchDownWayPoint()
    Friend Property touchDownWays As clsTouchDownWay()
    Friend Property arrivalPoint As clsTouchDownWayPoint
    Friend Property goAroundPoint As clsNavigationPoint

    Friend Property exitPaths As clsExitWay()

    Friend allNavigationPoints As New List(Of clsNavigationPoint)           'list of all navigation points used in this runway
    Friend allNavigationPaths As New List(Of clsNavigationPath)             'list of all navigation paths used in this runway
    Friend allPathWays As New List(Of clsPathWay)                           'list of all navigation paths used in this runway

    Friend Property isAvailableForDeparture As Boolean
    Friend Property isAvailableForArrival As Boolean
        Set(value As Boolean)
            Me.arrivalPoint.isAvailableForArrival = value
        End Set
        Get
            Return Me.arrivalPoint.isAvailableForArrival
        End Get
    End Property
    Friend ReadOnly Property canHandleArrivals As Boolean
        Get
            Return Not (Me.arrivalPoint Is Nothing)
        End Get
    End Property
    Friend ReadOnly Property canHandleDepartures As Boolean
        Get
            Return (Me.takeOffPoints.Count > 0)
        End Get
    End Property

    Friend ReadOnly Property landingAngle As Double
        Get
            Dim result As Double = Me.arrivalPoint.landingAngle
            Return result
        End Get
    End Property

    Friend ReadOnly Property takeoffAngle As Double
        Get
            Dim result As Double = clsNavigationPath.directionbetweenpoints(Me.takeOffPaths(0).entryPoint, Me.takeOffPaths(0).exitPoint)
            Return result
        End Get
    End Property

    Friend ReadOnly Property POIs As Dictionary(Of String, clsNavigationPoint)
        Get
            Dim result As New Dictionary(Of String, clsNavigationPoint)

            'search all LandingPoints
            For C1 As Long = 0 To Me.touchDownWayPoints.GetUpperBound(0)
                If Me.touchDownWayPoints(C1).isPOITower Then
                    result.Add(Me.touchDownWayPoints(C1).name.ToLower, Me.touchDownWayPoints(C1))
                End If
            Next

            Return result
        End Get
    End Property

    Friend ReadOnly Property mostTop As Double
        Get
            Dim tmpMostTop As Double = Double.PositiveInfinity

            'go throgh all points in this runway and return lowest value
            'departurepoints
            For Each singleDeparturePoint As clsTakeOffPoint In Me.takeOffPoints
                If singleDeparturePoint.pos_Y < tmpMostTop Then tmpMostTop = singleDeparturePoint.pos_Y
            Next

            'LandingPoints
            For Each singleLandingPoint As clsTouchDownWayPoint In Me.touchDownWayPoints
                If singleLandingPoint.pos_Y < tmpMostTop Then tmpMostTop = singleLandingPoint.pos_Y
            Next

            'interfaces between runway and ramp
            For Each singleLineUpWay As clsLineUpWay In Me.lineUpPaths
                If singleLineUpWay.mostTop < tmpMostTop Then tmpMostTop = singleLineUpWay.mostTop
            Next

            For Each singleExitWay As clsExitWay In Me.exitPaths
                If singleExitWay.mostTop < tmpMostTop Then tmpMostTop = singleExitWay.mostTop
            Next


            Return tmpMostTop
        End Get
    End Property
    Friend ReadOnly Property mostLeft As Double
        Get
            Dim tmpMostLeft As Double = Double.PositiveInfinity
            'go throgh all points in this runway and return lowest value
            'first departurepoints
            For Each singleDeparturePoint As clsTakeOffPoint In Me.takeOffPoints
                If singleDeparturePoint.pos_X < tmpMostLeft Then tmpMostLeft = singleDeparturePoint.pos_X
            Next

            'LandingPoints
            For Each singleLandingPoint As clsTouchDownWayPoint In Me.touchDownWayPoints
                If singleLandingPoint.pos_X < tmpMostLeft Then tmpMostLeft = singleLandingPoint.pos_X
            Next

            'interfaces between runway and ramp
            If Not Me.lineUpPaths Is Nothing Then
                For Each singleLineUpWay As clsLineUpWay In Me.lineUpPaths
                    If singleLineUpWay.mostLeft < tmpMostLeft Then tmpMostLeft = singleLineUpWay.mostLeft
                Next
            End If

            If Not Me.exitPaths Is Nothing Then
                For Each singleExitWay As clsExitWay In Me.exitPaths
                    If singleExitWay.mostLeft < tmpMostLeft Then tmpMostLeft = singleExitWay.mostLeft
                Next
            End If

            Return tmpMostLeft
        End Get
    End Property
    Friend ReadOnly Property mostBottom As Double
        Get
            Dim tmpMostBottom As Double = Double.NegativeInfinity

            'go throgh all points in this runway and return highest value
            'first departurepoints
            For Each singleDeparturePoint As clsTakeOffPoint In Me.takeOffPoints
                If singleDeparturePoint.pos_Y > tmpMostBottom Then tmpMostBottom = singleDeparturePoint.pos_Y
            Next

            'LandingPoints
            For Each singleLandingPoint As clsTouchDownWayPoint In Me.touchDownWayPoints
                If singleLandingPoint.pos_Y > tmpMostBottom Then tmpMostBottom = singleLandingPoint.pos_Y
            Next


            'interfaces between runway and ramp
            For Each singleLineUpWay As clsLineUpWay In Me.lineUpPaths
                If singleLineUpWay.mostBottom > tmpMostBottom Then tmpMostBottom = singleLineUpWay.mostBottom
            Next
            For Each singleExitWay As clsExitWay In Me.exitPaths
                If singleExitWay.mostBottom > tmpMostBottom Then tmpMostBottom = singleExitWay.mostBottom
            Next

            Return tmpMostBottom
        End Get
    End Property
    Friend ReadOnly Property mostRight As Double
        Get
            Dim tmpMostRight As Double = Double.NegativeInfinity

            'go throgh all points in this runway and return highest value
            'first departurepoints
            For Each singleDeparturePoint As clsTakeOffPoint In Me.takeOffPoints
                If singleDeparturePoint.pos_X > tmpMostRight Then tmpMostRight = singleDeparturePoint.pos_X
            Next

            'LandingPoints
            For Each singleLandingPoint As clsTouchDownWayPoint In Me.touchDownWayPoints
                If singleLandingPoint.pos_X > tmpMostRight Then tmpMostRight = singleLandingPoint.pos_X
            Next


            'interfaces between runway and ramp
            For Each singleLineUpWay As clsLineUpWay In Me.lineUpPaths
                If singleLineUpWay.mostRight > tmpMostRight Then tmpMostRight = singleLineUpWay.mostRight
            Next
            For Each singleExitWay As clsExitWay In Me.exitPaths
                If singleExitWay.mostRight > tmpMostRight Then tmpMostRight = singleExitWay.mostRight
            Next

            Return tmpMostRight
        End Get
    End Property

    'weather
    Friend Property windDirectionTo As Double
    Friend Property maxCrossWind As Long

    'gameplay
    Friend Property isInUse As Boolean = False


    Friend ReadOnly Property windDirectionFrom As Double
        Get
            Return (Me.windDirectionTo + 180) Mod 360
        End Get
    End Property

    Public Sub New(ByRef xElement As XElement, ByRef reference As clsAirport.structGeoCoordinate, ByRef ramps() As clsRamp, ByRef stars As clsAirPath(), ByRef sids As clsAirPath(), ByRef navigationPoints As List(Of clsConnectionPoint))
        Me.name = xElement.@name
        Me.objectID = xElement.@id

        Dim referenceCoordinate As New GeoCoordinate(reference.lat, reference.lng)

        ReDim Me.takeOffPoints(xElement.<takeoffpoint>.Count - 1)
        '   ReDim Me.takeOffWays(xElement.<takeoffway>.Count - 1)
        ReDim Me.takeOffPaths(xElement.<takeoffpath>.Count - 1)
        ReDim Me.lineUpPaths(xElement.<lineuppath>.Count - 1)

        ReDim Me.touchDownWayPoints(xElement.<touchdownwaypoint>.Count - 1)
        ReDim Me.touchDownWays(xElement.<touchdownway>.Count - 1)
        ReDim Me.exitPaths(xElement.<exitpath>.Count - 1)

        'departurepoints
        For C1 As Long = 0 To xElement.<takeoffpoint>.Count - 1
            Me.takeOffPoints(C1) = New clsTakeOffPoint(xElement.<takeoffpoint>(C1), reference)
        Next
        Me.allNavigationPoints.AddRange(Me.takeOffPoints)

        'takeoffpaths
        For C1 As Long = 0 To xElement.<takeoffpath>.Count - 1

            Me.takeOffPaths(C1) = New clsTakeOffPath(Me.getTakeOffPointByID(xElement.<takeoffpath>(C1).@startpoint),
                                                     Me.getTakeOffPointByID(xElement.<takeoffpath>(C1).@endpoint),
                                                     xElement.<takeoffpath>(C1), reference)
            Me.allNavigationPoints.AddRange(Me.takeOffPaths(C1).allNavigationPoints)
            Me.allNavigationPaths.AddRange(Me.takeOffPaths(C1).allNavigationPaths)
        Next
        Me.allPathWays.AddRange(Me.takeOffPaths)

        'lineuppaths
        For C1 As Long = 0 To xElement.<lineuppath>.Count - 1
            'go through all ramps
            For C2 = 0 To ramps.GetUpperBound(0)
                If ramps(C2).objectID = xElement.<lineuppath>(C1).@ramp Then

                    Me.lineUpPaths(C1) = New clsLineUpWay(ramps(C2).getPointByID(xElement.<lineuppath>(C1).@towardsramp),
                                                     Me.getTakeOffPointByID(xElement.<lineuppath>(C1).@towardsrunway),
                                                     xElement.<lineuppath>(C1), reference)
                    Me.allNavigationPoints.AddRange(Me.lineUpPaths(C1).allNavigationPoints)
                    Me.allNavigationPaths.AddRange(Me.lineUpPaths(C1).allNavigationPaths)
                    Exit For
                End If
            Next
        Next
        Me.allPathWays.AddRange(Me.lineUpPaths)

        'FINALS
        If xElement.<finals>.Count > 0 Then
            For Each singleFinal As XElement In xElement.<finals>(0).<final>
                Dim finalPath As New List(Of clsAStarEngine.structPathStep)
                Dim previousFinalPoint As clsConnectionPoint = Nothing
                Dim nextFinalPoint As clsConnectionPoint = Nothing
                Dim FinalName As String = singleFinal.@name
                For Each singleNavPoint As XElement In singleFinal.<finalpoint>
                    nextFinalPoint = navigationPoints.Find(Function(p As clsConnectionPoint) p.objectID = singleNavPoint.@name)

                    'error message if point not found
                    If nextFinalPoint Is Nothing Then MsgBox("runway: " & Me.name & vbNewLine & "final: " & FinalName & vbNewLine & "missing point: " & singleNavPoint.@name & vbNewLine & vbNewLine & "Program will crash.", MsgBoxStyle.Critical, "FinalPathPoint not found!")

                    '!!! add dynamic target speed for each section
                    Dim targetspeed As clsSpeedCollection = Nothing
                    Dim name As String = singleNavPoint.@name
                    Dim newPath As clsNavigationPath = Nothing
                    If Not previousFinalPoint Is Nothing Then newPath = New clsNavigationPath(previousFinalPoint, nextFinalPoint, clsNavigationPath.enumPathWayType.AirWay, name, targetspeed, Guid.NewGuid.ToString, name)
                    Dim newStructPathStep As New clsAStarEngine.structPathStep With {
                        .nextWayPoint = nextFinalPoint,
                        .taxiwayToWayPoint = newPath
                    }
                    finalPath.Add(newStructPathStep)
                    previousFinalPoint = nextFinalPoint

                    'Me.allNavigationPaths.AddRange(newStructPathStep.taxiwayToWayPoint)
                Next
                Me.FINALs.Add(New clsAirPath(finalPath, FinalName))
            Next
        End If

        'STARs
        ReDim Me.STARs(xElement.<star>.Count - 1)
        For C1 As Long = 0 To xElement.<star>.Count - 1
            Dim singleStarName As String = xElement.<star>(C1).@name
            For Each singleStar As clsAirPath In stars
                If singleStar.object_ID = singleStarName Then
                    Me.STARs(C1) = singleStar
                    Exit For
                End If
            Next
        Next

        'SIDs
        ReDim Me.SIDs(xElement.<sid>.Count - 1)
        For C1 As Long = 0 To xElement.<sid>.Count - 1
            Dim singleSidName As String = xElement.<sid>(C1).@name
            For Each singleSid As clsAirPath In sids
                If singleSid.object_ID = singleSidName Then
                    Me.SIDs(C1) = singleSid
                End If
            Next
        Next

        'touchdownwaypoints
        For C1 As Long = 0 To xElement.<touchdownwaypoint>.Count - 1
            Me.touchDownWayPoints(C1) = New clsTouchDownWayPoint(xElement.<touchdownwaypoint>(C1), reference)
        Next

        'arrivalpoint for this runway
        For C1 As Long = 0 To Me.touchDownWayPoints.GetUpperBound(0)
            If Me.touchDownWayPoints(C1).objectID = xElement.@arrivalpoint Then
                Me.arrivalPoint = Me.touchDownWayPoints(C1)
                'this has a final path
                Me.arrivalPoint.FINALs = Me.FINALs
                Exit For
            End If
        Next

        'touchdownways
        For C1 As Long = 0 To xElement.<touchdownway>.Count - 1
            Me.touchDownWays(C1) = New clsTouchDownWay(Me.getTouchDownWayPointByID(xElement.<touchdownway>(C1).@startpoint),
                                                       Me.getTouchDownWayPointByID(xElement.<touchdownway>(C1).@endpoint),
                                                       xElement.<touchdownway>(C1).@comment,
                                                       New clsSpeedCollection(xElement.<touchdownway>(C1).@maxspeed), Guid.NewGuid.ToString, xElement.<touchdownway>(C1).@name, xElement.<touchdownway>(C1).@taxipath)
            Me.allNavigationPaths.Add(Me.touchDownWays(C1))
        Next

        'exitpaths
        For C1 As Long = 0 To xElement.<exitpath>.Count - 1
            'go through all ramps
            For C2 = 0 To ramps.GetUpperBound(0)
                If ramps(C2).objectID = xElement.<exitpath>(C1).@ramp Then
                    Me.exitPaths(C1) = New clsExitWay(Me.getTouchDownWayPointByID(xElement.<exitpath>(C1).@towardsrunway),
                                                     ramps(C2).getPointByID(xElement.<exitpath>(C1).@towardsramp),
                                                     xElement.<exitpath>(C1), reference)
                    Me.allNavigationPaths.AddRange(Me.exitPaths(C1).allNavigationPaths)
                    Me.allNavigationPoints.AddRange(Me.exitPaths(C1).allNavigationPoints)
                    Exit For
                End If
            Next
        Next

        'goaroundpoint
        'but only if there is one
        If Not xElement.Element("goaroundpoint") Is Nothing Then
            Dim goaroundname As String = xElement.<goaroundpoint>(0).@name
            For Each singlePoint As clsNavigationPoint In navigationPoints
                If singlePoint.objectID = goaroundname Then
                    Me.goAroundPoint = singlePoint
                End If
            Next
        Else
            Me.goAroundPoint = Nothing
        End If

        'store all navigation points
        Me.allNavigationPoints.AddRange(Me.touchDownWayPoints)


        'set arrivalpoint if there is one for this runway
        If Me.canHandleArrivals Then Me.isAvailableForArrival = True
        If Me.canHandleDepartures Then Me.isAvailableForDeparture = True
    End Sub


    Friend Function getTakeOffPointByID(ByVal id As String) As clsTakeOffPoint
        Dim result As clsTakeOffPoint = Nothing
        For C1 = 0 To Me.takeOffPoints.GetUpperBound(0)
            If Me.takeOffPoints(C1).objectID = id Then
                result = Me.takeOffPoints(C1)
                Exit For
            End If
        Next
        Return result
    End Function

    Friend Function getTouchDownWayPointByID(ByVal id As String) As clsTouchDownWayPoint
        Dim result As clsTouchDownWayPoint = Nothing
        For C1 = 0 To Me.touchDownWayPoints.GetUpperBound(0)
            If Me.touchDownWayPoints(C1).objectID = id Then
                result = Me.touchDownWayPoints(C1)
                Exit For
            End If
        Next
        Return result
    End Function

    Friend Function getTouchDownWayPointByName(ByVal Name As String) As clsTouchDownWayPoint
        Dim result As clsTouchDownWayPoint = Nothing

        For C1 As Long = 0 To Me.touchDownWayPoints.GetUpperBound(0)
            If Me.touchDownWayPoints(C1).name = Name Then
                result = Me.touchDownWayPoints(C1)
                Exit For
            End If
        Next

        Return result
    End Function

    ''' <summary>
    ''' sets wind direction w/ angle to where the wind is going
    ''' </summary>
    ''' <param name="direction">direction where whe wind goes</param>
    Friend Sub setWindDirection(ByVal direction As Double)
        Me.windDirectionTo = direction

        'set winddirection for all touchdownwaypoints
        For Each singleTouchDownWayPoint As clsTouchDownWayPoint In Me.touchDownWayPoints
            singleTouchDownWayPoint.setWindDirection(direction)
        Next
    End Sub

    Friend Sub setMaxCrossWind(ByVal _maxCrossWindAngle As Long)
        Me.maxCrossWind = _maxCrossWindAngle

        'set winddirection for all touchdownwaypoints
        For Each singleTouchDownWayPoint As clsTouchDownWayPoint In Me.touchDownWayPoints
            singleTouchDownWayPoint.setMaxCrossWind(_maxCrossWindAngle)
        Next
    End Sub

    Friend Sub updateIsInUse(ByRef planes As List(Of clsPlane))
        Dim result_isInUse As Boolean = False


        'runway end points
        '!!! take the longest takeofpath!
        Dim minDistance As Double = Double.PositiveInfinity
        Dim x1 As Double = 0
        Dim y1 As Double = 0
        Dim x2 As Double = 0
        Dim y2 As Double = 0

        For Each singlePath As clsTakeOffPath In Me.takeOffPaths
            Dim tmpDistance = mdlHelpers.diffBetweenPoints2D(singlePath.entryPoint.pos_X, singlePath.entryPoint.pos_Y, singlePath.exitPoint.pos_X, singlePath.exitPoint.pos_Y)

            If tmpDistance < minDistance Then
                x1 = singlePath.entryPoint.pos_X
                y1 = singlePath.entryPoint.pos_Y
                x2 = singlePath.exitPoint.pos_X
                y2 = singlePath.exitPoint.pos_Y
            End If
        Next

        If Me.touchDownWays.Count > 0 Then
            Dim tmpDistance = mdlHelpers.diffBetweenPoints2D(Me.touchDownWays.First.taxiWayPoint1.pos_X,
                                                             Me.touchDownWays.First.taxiWayPoint1.pos_Y,
                                                             Me.touchDownWays.Last.taxiWayPoint2.pos_X,
                                                             Me.touchDownWays.Last.taxiWayPoint2.pos_Y)
            If tmpDistance < minDistance Then
                x1 = Me.touchDownWays.First.taxiWayPoint1.pos_X
                y1 = Me.touchDownWays.First.taxiWayPoint1.pos_Y
                x2 = Me.touchDownWays.Last.taxiWayPoint1.pos_X
                y2 = Me.touchDownWays.Last.taxiWayPoint1.pos_Y
            End If

        End If

        Dim mostLeft As Double = Me.mostLeft
        Dim mostRight As Double = Me.mostRight
        Dim mostTop As Double = Me.mostTop
        Dim mostBottom As Double = Me.mostBottom

        'check if at least one plane intersects w/ longes takeoffline
        For Each singlePlane As clsPlane In planes

            'look only at planes that are below 50 feet and don't gate
            'also, filter planes that have the same altitude as the touchdownwaypoint to ignore the incoming plane itself
            If Not singlePlane.currentState = clsPlane.enumPlaneState.ground_atGate AndAlso
                singlePlane.pos_Altitude.feet < 50 AndAlso
                singlePlane.pos_Altitude.feet <> Me.touchDownWayPoints.First.altitude.feet Then


                'circle data
                Dim cx As Double = singlePlane.pos_X.meters
                Dim cy As Double = singlePlane.pos_Y.meters
                Dim r As Double = singlePlane.collisionRadius.meters

                'first check if plane is in bounds
                '!!! check if cprrect
                'If mostLeft < (cx - r) And mostRight > (cx + r) And mostTop < (cy - r) And mostBottom > (cy + r) Then
                If (Not (
                        ((cx - r) < mostLeft And (cx - r) < mostRight And (cx - r) < mostLeft And (cx + r) < mostRight) Or
                        ((cx - r) > mostLeft And (cx - r) > mostRight And (cx + r) > mostLeft And (cx + r) > mostRight)
                    ) And Not (
                        ((cy - r) < mostTop And (cy - r) < mostBottom And (cy - r) < mostTop And (cy + r) < mostBottom) Or
                        ((cy - r) > mostTop And (cy - r) > mostBottom And (cy + r) > mostTop And (cy + r) > mostBottom)
                    )) Then

                    'get closest point on runway
                    Dim distX As Double = x1 - x2
                    Dim distY As Double = y1 - y2
                    Dim len As Double = Math.Sqrt((distX * distX) + (distY * distY))

                    'get dot product
                    Dim dot As Double = (((cx - x1) * (x2 - x1)) + ((cy - y1) * (y2 - y1))) / Math.Pow(len, 2)

                    'get the actual point
                    Dim closestX As Double = x1 + (dot * (x2 - x1))
                    Dim closestY As Double = y1 + (dot * (y2 - y1))

                    '!!!missing: make sure that the closest point is between P1 and P2

                    'calculate distance between C and the closest point
                    Dim newDistX As Double = closestX - cx
                    Dim newDistY As Double = closestY - cy
                    Dim distance As Double = Math.Sqrt((newDistX * newDistX) + (newDistY * newDistY))

                    If distance <= r Then
                        result_isInUse = True

                        Exit For
                    End If
                End If
            End If
        Next
        If Not Me.arrivalPoint Is Nothing Then
            Me.arrivalPoint.isInUse = result_isInUse

        End If
        Me.isInUse = result_isInUse

    End Sub


End Class
