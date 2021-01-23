Option Explicit On
Imports System.Device.Location

<Serializable>
Public Class clsConnectionPoint
    Inherits clsNavigationPoint

    Friend Property taxiWays As clsNavigationPath()
    Friend Property isHoldPoint As Boolean

    Public Sub New(ByRef xElement As XElement, ByRef reference As clsAirport.structGeoCoordinate)
        MyBase.New(xElement, reference)
        ReDim taxiWays(-1)
        Me.isHoldPoint = (xElement.@isholdpoint = "true")
    End Sub

    Friend Sub addTaxiWay(ByRef taxiWay As clsNavigationPath)
        ReDim Preserve Me.taxiWays(Me.taxiWays.GetUpperBound(0) + 1)
        Me.taxiWays(Me.taxiWays.GetUpperBound(0)) = taxiWay
    End Sub

    Friend ReadOnly Property isRunwayPoint As Boolean
        Get
            'if there is one lineupway connected, it is a runwaypoint
            Dim isFound As Boolean = False
            For Each singleTaxiWay As clsNavigationPath In Me.taxiWays
                If singleTaxiWay.type = clsWaySection.enumPathWayType.lineUpWay Then
                    isFound = True
                    Exit For
                End If
            Next
            Return isFound
        End Get
    End Property

    Friend ReadOnly Property isLineUpPoint As Boolean
        Get
            'if there is one lineupway connected and one takeoffway
            Dim foundTakeOffWay As Boolean = False
            Dim foundLineUpWay As Boolean = False

            For Each singleConnectedWay As clsNavigationPath In Me.taxiWays
                If singleConnectedWay.type = clsNavigationPath.enumPathWayType.lineUpWay Then
                    foundLineUpWay = True
                End If

                If singleConnectedWay.type = clsNavigationPath.enumPathWayType.takeOffWay Then
                    foundTakeOffWay = True
                End If
            Next

            Dim result As Boolean = (foundTakeOffWay And foundLineUpWay)
            Return result
        End Get
    End Property

    Friend ReadOnly Property isGate As Boolean
        Get
            Return TypeOf (Me) Is clsGate
        End Get
    End Property



    ''' <summary>
    ''' find the path for takeoff from entrypoint to runway to last point before entering freeflight
    ''' </summary>
    ''' <returns>path to follow for takeoff</returns>
    Friend Function getTakeOffPath() As List(Of clsAStarEngine.structPathStep)
        Dim result As New List(Of clsAStarEngine.structPathStep)

        Dim takeOffPoint As clsConnectionPoint = Me

        'go through the path until there is no landingwaypoint on the poopsite left
        Dim currentPoint As clsConnectionPoint = takeOffPoint

        Dim newWayPart As New clsAStarEngine.structPathStep With {.nextWayPoint = currentPoint, .taxiwayToWayPoint = Nothing}
        result.Add(newWayPart)

        Dim currentPath As clsNavigationPath = Nothing
        '!!! I assume that only one lineuppath is connected
        For C1 As Long = 0 To Me.taxiWays.GetUpperBound(0)
            If Me.taxiWays(C1).type = clsWaySection.enumPathWayType.lineUpWay Then
                currentPath = Me.taxiWays(C1)
            End If
        Next

        currentPoint = currentPath.oppositeTaxiWayPoint(currentPoint)

        newWayPart = New clsAStarEngine.structPathStep With {.nextWayPoint = currentPoint, .taxiwayToWayPoint = currentPath}

        result.Add(newWayPart)

        Dim endFound As Boolean = False
        While Not endFound
            Dim foundCounterPart As Boolean = False
            'go thorugh all paths that are connected and choose the one that is a landingway and not the last one

            'if 1 way is connected, we found the end
            'if 2 ways are connected, we follow the way 
            'if more than 2 ways are connected, we need to find the takeoffway
            Select Case currentPoint.taxiWays.Count
                Case 1
                    'end reached - add final point
                    currentPath = currentPoint.taxiWays(0)
                    currentPoint = currentPath.oppositeTaxiWayPoint(currentPoint)
                    endFound = True
                Case 2
                    For C1 = 0 To currentPoint.taxiWays.GetUpperBound(0)
                        'first check if found way is self
                        If Not currentPath Is currentPoint.taxiWays(C1) Then
                            'we found the next point
                            currentPath = currentPoint.taxiWays(C1)
                            currentPoint = currentPath.oppositeTaxiWayPoint(currentPoint)

                            newWayPart = New clsAStarEngine.structPathStep With {.nextWayPoint = currentPoint, .taxiwayToWayPoint = currentPath}

                            result.Add(newWayPart)
                            Exit For
                        End If
                    Next
                Case Else
                    For C1 = 0 To currentPoint.taxiWays.GetUpperBound(0)
                        'make sure that the found way is not self and that it is a takeoffway
                        If Not (currentPath Is currentPoint.taxiWays(C1)) And currentPoint.taxiWays(C1).type = clsWaySection.enumPathWayType.takeOffWay Then
                            'we found the next point
                            currentPath = currentPoint.taxiWays(C1)
                            currentPoint = currentPath.oppositeTaxiWayPoint(currentPoint)

                            newWayPart = New clsAStarEngine.structPathStep With {.nextWayPoint = currentPoint, .taxiwayToWayPoint = currentPath}

                            result.Add(newWayPart)
                            Exit For
                        End If
                    Next
            End Select



        End While


        Return result
    End Function

End Class
