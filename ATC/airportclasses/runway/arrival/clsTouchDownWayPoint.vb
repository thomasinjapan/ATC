Option Explicit On
Imports System.Device.Location

<Serializable>
Public Class clsTouchDownWayPoint
    Inherits clsConnectionPoint

    Friend ReadOnly Property isArrivalPoint As Boolean
    Friend Property FINALs As List(Of clsAirPath)
    Friend Property isInUse As Boolean = False

    Friend ReadOnly Property landingAngle As Double
        Get
            Dim result As Double = Me.getLandingPath(1).taxiwayToWayPoint.directionTo(Me.getLandingPath(1).nextWayPoint)
            Return result
        End Get
    End Property

    'define if opened
    Friend Property isAvailableForArrival As Boolean

    'lenghts of remaining runway
    Friend ReadOnly Property runwayLength As clsDistanceCollection
        Get
            'take all remaining segments of the landingpath and calculate the sum of the lengths
            Dim result As New clsDistanceCollection(0, clsDistanceCollection.enumDistanceUnits.meters)
            Dim landingpath As List(Of clsastarengine.structPathStep) = Me.getLandingPath

            For Each singleSegment As clsastarengine.structPathStep In landingpath
                If Not singleSegment.taxiwayToWayPoint Is Nothing Then result.meters += singleSegment.taxiwayToWayPoint.length
                'End If
            Next

            Return result
        End Get
    End Property

    'weather
    Friend Property windDirectionTo As Double
    Friend ReadOnly Property windDirectionFrom As Double
        Get
            Return (Me.windDirectionTo + 180) Mod 360
        End Get
    End Property

    Friend Property maxCrossWind As Long

    Public Sub New(ByRef xElement As XElement, ByRef referencecoordinate As clsAirport.structGeoCoordinate)
        MyBase.New(xElement, referencecoordinate)
        Me.isArrivalPoint = xElement.@isarrivalpoint = "true"

    End Sub

    Friend Function getLandingPath() As List(Of clsastarengine.structPathStep)
        Dim result As New List(Of clsastarengine.structPathStep)

        Dim landingPoint As clsTouchDownWayPoint = Me


        'go through the path until there is no landingwaypoint on the poopsite left
        Dim currentPoint As clsTouchDownWayPoint = landingPoint

        Dim newWayPart As New clsastarengine.structPathStep With {.nextWayPoint = currentPoint, .taxiwayToWayPoint = Nothing}
        result.Add(newWayPart)

        '!!! I assume that the landingpoint has only one path connected
        Dim currentPath As clsTouchDownWay = landingPoint.taxiWays(0)
        currentPoint = currentPath.oppositeTaxiWayPoint(currentPoint)

        newWayPart = New clsastarengine.structPathStep With {.nextWayPoint = currentPoint, .taxiwayToWayPoint = currentPath}

        result.Add(newWayPart)

        Dim endFound As Boolean = False
        While Not endFound
            Dim foundCounterPart As Boolean = False
            'go thorugh all paths that are connected and choose the one that is a landingway and not the last one
            For C1 = 0 To currentPoint.taxiWays.GetUpperBound(0)
                If currentPoint.taxiWays(C1).type = clsWaySection.enumPathWayType.touchDownWay Then
                    'we found a valid way
                    'lets check if it is the one we visited before
                    If Not currentPath Is currentPoint.taxiWays(C1) Then
                        'we found the next point
                        currentPath = currentPoint.taxiWays(C1)
                        currentPoint = currentPath.oppositeTaxiWayPoint(currentPoint)

                        newWayPart = New clsastarengine.structPathStep With {.nextWayPoint = currentPoint, .taxiwayToWayPoint = currentPath}

                        result.Add(newWayPart)

                        foundCounterPart = True
                        Exit For
                    Else
                        'do nothing
                    End If

                Else
                    'do nothing

                End If
            Next

            endFound = Not foundCounterPart

        End While

        Return result
    End Function

    ''' <summary>
    ''' sets wind direction w/ angle to where the wind is going
    ''' </summary>
    ''' <param name="direction">direction where whe wind goes</param>
    Friend Sub setWindDirection(ByVal direction As Double)
        Me.windDirectionTo = direction
    End Sub

    Friend Sub setMaxCrossWind(ByVal _maxCrossWindAngle As Long)
        Me.maxCrossWind = _maxCrossWindAngle
    End Sub

    ''' <summary>
    ''' finds the closest final to a given point
    ''' </summary>
    ''' <param name="x">x position to measure from in meters</param>
    ''' <param name="y">y position to measure from in meters</param>
    ''' <returns></returns>
    Friend Function findClosestFinal(ByVal x As Double, ByVal y As Double, ByVal direction As Double) As Tuple(Of clsAirPath, clsConnectionPoint)
        Dim result As Tuple(Of clsAirPath, clsConnectionPoint) = Nothing

        'search in all finals
        Dim minDistanceBetweenPlaneAndPoint As Double = Double.PositiveInfinity
        For Each singleFinal As clsAirPath In Me.FINALs
            'search in all points
            For Each singleNavPoint As clsAStarEngine.structPathStep In singleFinal.path
                'if the point locations are identical, we can end the search here
                If Not mdlHelpers.diffBetweenPoints2D(x, y, singleNavPoint.nextWayPoint.pos_X, singleNavPoint.nextWayPoint.pos_Y) = 0 Then

                    'check distance to first point in final path
                    Dim distance As Double = mdlHelpers.diffBetweenPoints2D(singleNavPoint.nextWayPoint.pos_X, singleNavPoint.nextWayPoint.pos_Y, x, y)
                    Dim angle As Double = clsNavigationPath.directionbetweenpoints(x, y, singleNavPoint.nextWayPoint.pos_X, singleNavPoint.nextWayPoint.pos_Y)
                    Dim deltaangle As Double = mdlHelpers.diffBetweenAnglesAbs(direction, angle)

                    'only consider points within +-45 deg
                    If deltaangle <= 45 Then
                        If distance < minDistanceBetweenPlaneAndPoint Then
                            minDistanceBetweenPlaneAndPoint = distance
                            result = Tuple.Create(singleFinal, singleNavPoint.nextWayPoint)
                        End If
                    End If
                Else
                    'point found since it is identical w/ location
                    result = Tuple.Create(singleFinal, singleNavPoint.nextWayPoint)
                    minDistanceBetweenPlaneAndPoint = 0
                    Exit For
                End If
            Next
        Next

        'if no result found, use the default
        If result Is Nothing Then
            Dim defaultpath As clsAirPath = Me.FINALs.Find(Function(p As clsAirPath) p.object_ID.ToLower = "default")
            result = Tuple.Create(defaultpath, defaultpath.path.First.nextWayPoint)
        End If

        Return result
    End Function

End Class
