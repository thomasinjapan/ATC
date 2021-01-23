Option Explicit On
Imports System.Drawing.Text

Public Class clsDijkstra
    Private Structure structInfoCard
        Friend node As clsConnectionPoint
        Friend knownShortestDistance As Double
        Friend previousNode As clsConnectionPoint
        Friend previousTaxiWay As clsNavigationPath
        Friend previousDirection As Double
    End Structure

    Friend Structure structPathStep
        Friend nextWayPoint As clsConnectionPoint
        Friend taxiwayToWayPoint As clsNavigationPath
    End Structure

    Private queueWaiting As New List(Of structInfoCard)
    Private queueDone As New List(Of structInfoCard)
    Private pathExists As Boolean = False

    Friend ReadOnly Property shortestPath As List(Of structPathStep)
        Get
            'take card from queuedone top and check for parent node down to start node card
            Dim result As List(Of structPathStep) = Nothing

            If Me.pathExists Then
                result = New List(Of structPathStep)

                Dim currentCard As structInfoCard = queueDone(queueDone.Count - 1)
                result.Insert(0, New structPathStep With {.nextWayPoint = currentCard.node, .taxiwayToWayPoint = currentCard.previousTaxiWay})

                While findCardInQueue(currentCard, queueDone) > 0
                    currentCard = queueDone(findNodeInQueue(currentCard.previousNode, queueDone))
                    result.Insert(0, New structPathStep With {.nextWayPoint = currentCard.node, .taxiwayToWayPoint = currentCard.previousTaxiWay})
                End While
            Else
                'do nothing
            End If
            Return result
        End Get
    End Property

    Public Sub New(ByRef startNode As clsConnectionPoint, ByRef endNode As clsConnectionPoint, Optional startAngle As Double = 0, Optional ByVal maxAngle As Double = 360, Optional ByVal runwayPunsihment As Double = 0, Optional ByVal preferences As List(Of String) = Nothing)

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

        queueWaiting.Add(startCard)

        'as long as we did not find the goal and the queue still has cards
        Dim goalFound As Boolean = False
        While (Not goalFound) And queueWaiting.Count > 0
            'take the node with the smallest knownShortesDistance and check for neighbors
            'if it is the end, skip the rest and inform parent
            'get card we want to look at by searching the one with the shortest distance

            Dim nextCard As structInfoCard = queueWaiting(0)
            Dim cardIndex As Long = 0
            For C1 As Long = 1 To queueWaiting.Count - 1
                If queueWaiting(C1).knownShortestDistance < nextCard.knownShortestDistance Then
                    nextCard = queueWaiting(C1)
                    cardIndex = C1
                End If
            Next

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
                'get all connected nodes
                Dim bonusused As Boolean = False

                For C1 As Long = 0 To nextCard.node.taxiWays.GetUpperBound(0)

                    'check if angle is within expectation
                    Dim oldAngle As Double = nextCard.previousDirection
                    Dim newAngle As Double = nextCard.node.taxiWays(C1).directionFrom(nextCard.node)

                    Dim angleDelta As Double = mdlHelpers.diffBetweenAnglesAbs(newAngle, oldAngle)

                    'if angle <= targetangle, allow finding path
                    If angleDelta <= maxAngle Then
                        'calculate new distance

                        Dim newDistance As Double = nextCard.knownShortestDistance + nextCard.node.taxiWays(C1).length
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

                        '- preference: in case we are on a road with name of 

                        '- preference: add benefit
                        newDistance += benefit
                        Dim proposedNewCard As New structInfoCard With {.node = nextCard.node.taxiWays(C1).oppositeTaxiWayPoint(nextCard.node), .knownShortestDistance = newDistance, .previousNode = nextCard.node, .previousTaxiWay = nextCard.node.taxiWays(C1), .previousDirection = newAngle}

                        'if we already put the card on the done pile, ignore it entirely
                        If findCardInQueue(proposedNewCard, queueDone) < 0 Then
                            'if we find the card in the waiting pile, check if the distances and the pass through have changed
                            'otherwise, just add the card
                            If findCardInQueue(proposedNewCard, queueWaiting) >= 0 Then
                                Dim existingCardIndex As Long = findCardInQueue(proposedNewCard, queueWaiting)
                                'check if the new route is shorter
                                'if yes, remove the old card and replace it by the new one
                                'if not, ignore the card

                                'If proposedNewCard.knownShortestDistance + punishment < queueWaiting(existingCardIndex).knownShortestDistance Then
                                If proposedNewCard.knownShortestDistance < queueWaiting(existingCardIndex).knownShortestDistance Then
                                    queueWaiting.RemoveAt(existingCardIndex)
                                    queueWaiting.Add(proposedNewCard)

                                Else
                                    'do nothing
                                End If

                            Else
                                'put the new card on the waiting pile
                                queueWaiting.Add(proposedNewCard)

                            End If

                        Else
                            'Do nothing
                        End If
                    Else
                        'angle is bigger than allowed
                        'so do nothing
                    End If
                Next

                'we are done with this card, lets put it on the finished-stack and delete it from the waiting-stack
                queueDone.Add(nextCard)
                queueWaiting.RemoveAt(Me.findCardInQueue(nextCard, queueWaiting))

            Else
                'okay, we are at the end.
                'put the card on the finished-stack, delete it from the waiting and set that goal was found
                queueDone.Add(nextCard)
                '                queueWaiting.RemoveAt(cardIndex)
                queueWaiting.RemoveAt(Me.findCardInQueue(nextCard, queueWaiting))
                goalFound = True
            End If

            'now create result


        End While

        Me.pathExists = goalFound

    End Sub

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
End Class
