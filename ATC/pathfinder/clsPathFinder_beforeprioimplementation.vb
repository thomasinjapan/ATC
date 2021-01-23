Option Explicit On

Public Class clsPathFinder
    Friend Structure structInfoCard
        Friend name As String
        Friend distanceWithPath() As Double
        Friend node As clsConnectionPoint
        Friend knownShortestDistance As Double
        Friend previousNode As clsConnectionPoint
        Friend previousTaxiWay As clsNavigationPath
        Friend previousDirection As Double
        Friend cameFromIndex As Long
    End Structure

    Friend Structure structPathStep
        Friend nextWayPoint As clsConnectionPoint
        Friend taxiwayToWayPoint As clsNavigationPath
    End Structure

    Friend queueWaiting As New List(Of structInfoCard)
    Private pathExists As Boolean = False
    Private maxAngle As Double


    Friend Property solutions As New List(Of List(Of structInfoCard))

    Friend ReadOnly Property shortestPath As List(Of structPathStep)
        Get
            'take card from queuedone top and check for parent node down to start node card
            Dim result As New List(Of structPathStep)

            'check all solutions and find the shortest one
            Dim shortestPathSoFar As List(Of structInfoCard) = solutions(0)
            For Each singleSolution As List(Of structInfoCard) In Me.solutions
                If singleSolution.Last.knownShortestDistance < shortestPathSoFar.Last.knownShortestDistance Then
                    shortestPathSoFar = singleSolution
                End If
            Next

            'convert the path to a solution
            For C1 As Long = 0 To shortestPathSoFar.Count - 1
                result.Add(New structPathStep() With {.nextWayPoint = shortestPathSoFar(C1).node, .taxiwayToWayPoint = shortestPathSoFar(C1).previousTaxiWay})
            Next

            Return result
        End Get
    End Property

    Public Sub New(ByRef startNode As clsConnectionPoint, ByRef endNode As clsConnectionPoint, Optional startAngle As Double = 0, Optional ByVal maximalAngle As Double = 360, Optional ByVal runwayPunsihment As Double = 0, Optional ByVal preferences As List(Of String) = Nothing)
        Me.maxAngle = maximalAngle

        If preferences Is Nothing Then
            preferences = New List(Of String)
        End If

        'create the startnode
        Dim startCard As New structInfoCard
        startCard.knownShortestDistance = 0
        startCard.previousNode = Nothing
        startCard.node = startNode
        startCard.previousTaxiWay = Nothing
        startCard.previousDirection = startAngle
        startCard.cameFromIndex = -1
        startCard.name = startNode.objectID

        'we need to register what paths we have looked at to allow to re-visit points that have not visited paths due to wrong angle
        'first assume no path has been visited yet
        Dim startpathvisited(startCard.node.taxiWays.GetUpperBound(0)) As Double
        For C1 As Long = 0 To startpathvisited.GetUpperBound(0)
            startpathvisited(C1) = Double.PositiveInfinity
        Next
        startCard.distanceWithPath = startpathvisited

        queueWaiting.Add(startCard)

        'as long as we have not checked all nodes
        Dim goalFound As Boolean = False
        Dim nextCardPointer As Long = 0

        While nextCardPointer <= Me.queueWaiting.Count - 1
            'get the next card on the stack
            Dim nextCard As structInfoCard = queueWaiting(nextCardPointer)

            '-preference-
            'check if one of the next connected ways is the one after the curren one in the preference queue
            'if so, delete the current one
            If preferences.Count > 1 Then
                For Each singlePath As clsNavigationPath In nextCard.node.taxiWays
                    If singlePath.name = preferences(1) Then
                        preferences.RemoveAt(0)
                        Exit For
                    End If
                Next
            End If


            'next card to analyse is found
            'if it is the one we searched for, cancel here, else continue
            If Not nextCard.node.objectID = endNode.objectID Then

                For C1 As Long = 0 To nextCard.node.taxiWays.GetUpperBound(0)
                    'if the new path would be longer than the know one, ignore it
                    Dim newDistance As Double = nextCard.knownShortestDistance + nextCard.node.taxiWays(C1).length
                    Dim formerDistance As Double = nextCard.distanceWithPath(C1)
                    If newDistance < formerDistance Then
                        'so, this is a path we should look at again

                        'check if angle is within expectation
                        Dim oldAngle As Double = nextCard.previousDirection
                        Dim newAngle As Double = nextCard.node.taxiWays(C1).directionFrom(nextCard.node)

                        Dim angleDelta As Double = mdlHelpers.diffBetweenAnglesAbs(newAngle, oldAngle)

                        'if angle <= targetangle, allow finding path
                        If angleDelta <= maxAngle Then

                            'calculate new distance

                            Dim benefit As Double = 0

                            '-preference-
                            If preferences.Count > 0 Then
                                If nextCard.node.taxiWays(C1).name = preferences.First Then
                                    benefit = -(nextCard.node.taxiWays(C1).length)
                                End If
                            Else
                                benefit = 0
                            End If

                            'punishment for routes on runways
                            'If TypeOf (nextCard.node.taxiWays(C1)) Is clsTouchDownWay Then newDistance += runwayPunsihment
                            'If TypeOf (nextCard.node.taxiWays(C1)) Is clsRunwayTaxiWay Then newDistance += runwayPunsihment
                            If nextCard.node.taxiWays(C1).type = clsWaySection.enumPathWayType.touchDownWay Then newDistance += runwayPunsihment
                            If nextCard.node.taxiWays(C1).type = clsWaySection.enumPathWayType.runwayTaxiWay Then newDistance += runwayPunsihment

                            '- preference: add benefit
                            newDistance += benefit

                            'check if we had the node already and adjust the pathsvisited
                            'else make all ifinitely long
                            Dim oppositeNode As clsConnectionPoint = nextCard.node.taxiWays(C1).oppositeTaxiWayPoint(nextCard.node)
                            Dim propsedpathsvisited(oppositeNode.taxiWays.GetUpperBound(0)) As Double
                            Dim oppositeKnown As Boolean = False

                            'search backwards to receive most recent info
                            For C2 As Long = nextCardPointer - 1 To 0 Step -1
                                If Me.queueWaiting(C2).node.objectID = oppositeNode.objectID Then
                                    propsedpathsvisited = Me.queueWaiting(C2).distanceWithPath
                                    oppositeKnown = True
                                    Exit For
                                End If
                            Next

                            If Not oppositeKnown Then
                                'for the proposed card, assume that it is new and none of its paths have been visited yet
                                For C2 As Long = 0 To propsedpathsvisited.GetUpperBound(0)
                                    propsedpathsvisited(C2) = Double.PositiveInfinity
                                Next

                            End If


                            Dim proposedNewCard As New structInfoCard With {.node = oppositeNode, .knownShortestDistance = newDistance, .previousNode = nextCard.node, .previousTaxiWay = nextCard.node.taxiWays(C1), .previousDirection = newAngle, .distanceWithPath = propsedpathsvisited, .cameFromIndex = nextCardPointer, .name = oppositeNode.objectID}

                            'add the new card on the stack
                            Me.queueWaiting.Add(proposedNewCard)

                            nextCard.distanceWithPath(C1) = newDistance
                        Else
                            'angle is bigger than allowed
                            'so do nothing
                        End If
                    Else
                        'this path was already visited 
                        'still, it may be valid to update at least the node data if it is shorter to get there

                    End If

                Next



            Else
                'okay, we are at the end.

                'add the current solution to the set of solutions
                Me.solutions.Add(reconstructPath(nextCardPointer))

                goalFound = True
            End If

            nextCardPointer += 1
        End While

        Me.pathExists = goalFound

    End Sub

    Friend Function reconstructPath(ByVal goalCardIndex As Long) As List(Of structInfoCard)

        'take card from queuedone top and check for parent node down to start node card
        Dim result As New List(Of structInfoCard)

        Dim currentCard As structInfoCard = queueWaiting(goalCardIndex)
        result.Insert(0, currentCard)

        'continue as long as there are cards to be found
        While currentCard.cameFromIndex >= 0
            currentCard = Me.queueWaiting(currentCard.cameFromIndex)
            result.Insert(0, currentCard)

        End While

        Return result
    End Function


    Private Function findCardInQueue(ByRef searchCard As structInfoCard, ByRef queue As List(Of structInfoCard)) As Long
        Dim result As Long = -1

        For C1 As Long = 0 To queue.Count - 1
            If queue(C1).node.objectID = searchCard.node.objectID Then
                result = C1
                Exit For
            End If
        Next

        Return result
    End Function


    Private Function findNodeInQueue(ByRef searchNode As clsConnectionPoint, ByRef queue As List(Of structInfoCard)) As Long
        Dim result As Long = -1

        For C1 As Long = 0 To queue.Count - 1
            If queue(C1).node.objectID = searchNode.objectID Then
                result = C1
                Exit For
            End If
        Next

        Return result
    End Function

    Private Function findNodesInQueue(ByRef searchNode As clsConnectionPoint, ByRef queue As List(Of structInfoCard)) As List(Of Long)
        Dim result As New List(Of Long)

        For C1 As Long = 0 To queue.Count - 1
            If queue(C1).node.objectID = searchNode.objectID Then
                result.Add(C1)
            End If
        Next

        Return result
    End Function
End Class
