Option Explicit On
Imports System.IO

<Serializable>
Public Class clsAStarEngine

    Dim noOriginId As String = Guid.NewGuid.ToString                'for the nodes that have no origin

    Friend Event cardFound(ByRef card As clsAStarCard)

    <Serializable> Public Structure structPathStep
        Friend nextWayPoint As clsConnectionPoint
        Friend taxiwayToWayPoint As clsNavigationPath
    End Structure

    Friend Function Solution(ByRef startPoint As clsConnectionPoint, ByRef endPoint As clsConnectionPoint, ByVal startAngle As Double, ByVal maxAngle As Double, Optional ByVal priorities As List(Of clsConnectionPoint) = Nothing) As List(Of structPathStep)
        Dim result As New List(Of structPathStep)

        If priorities Is Nothing Then priorities = New List(Of clsConnectionPoint)

        Dim foundDuplicate As Boolean = True
        While foundDuplicate
            foundDuplicate = False
            For C1 As Long = 0 To priorities.Count - 2
                If priorities(C1).objectID = priorities(C1 + 1).objectID Then
                    priorities.RemoveAt(C1)
                    foundDuplicate = True
                    Exit For
                End If
            Next
        End While


        Dim allPathParts As New List(Of List(Of structPathStep))
        Dim lastEndPoint As clsConnectionPoint = startPoint
        Dim lastAngle As Double = startAngle

        '--- find path to next goal by following the currentgoal
        ' as long as there is at least one preference, search for closest point with preference
        While Not priorities.Count = 0
            'search from last endpoint to first point with preference name  
            Dim nextPath As List(Of structPathStep) = Me.findPath(lastEndPoint, priorities.First, lastAngle, maxAngle)
            'add path only if something was found
            If nextPath.Count > 0 Then

                allPathParts.Add(nextPath)
                priorities.Remove(priorities.First)

                lastEndPoint = allPathParts.Last.Last.nextWayPoint
                lastAngle = allPathParts.Last.Last.taxiwayToWayPoint.directionTo(lastEndPoint)
            Else
                'else just remove this part
                priorities.Remove(priorities.First)
            End If
        End While

        ' finally search for path from last point to goal
        Dim finalPath As List(Of structPathStep) = Me.findPath(lastEndPoint, endPoint, lastAngle, maxAngle)
        'add only if a pathstep found
        If finalPath.Count > 0 Then
            allPathParts.Add(finalPath)
        End If

        'combinepaths
        For Each singlePathSet As List(Of structPathStep) In allPathParts
            'remove first item in case this is not the first path part
            'otherwise, the last waypoint of the former part and the first waypoint of the later part will be the same
            'but with one without way to get there
            If allPathParts.IndexOf(singlePathSet) > 0 Then singlePathSet.Remove(singlePathSet.First)
            result.AddRange(singlePathSet)
        Next


        Return result
    End Function

    Private Function findPath(ByRef startPoint As clsConnectionPoint, ByRef endPoint As clsConnectionPoint, ByVal startAngle As Double, ByVal maxAngle As Double) As List(Of structPathStep)
        Dim result As New List(Of structPathStep)

        Dim nodeList As New List(Of clsAStarCard)
        Dim visitedComboList As New clsVisitedComboCollection

        Dim firstNode As New clsAStarCard(0, mdlHelpers.diffBetweenPoints2D(startPoint.pos_X, startPoint.pos_Y, endPoint.pos_X, endPoint.pos_Y), startAngle.ToString, -1, startPoint, Nothing)
        nodeList.Add(firstNode)

        Dim endFound As Boolean = False
        Dim nextNodeIndex As Long = 0
        Dim endNodeIndex As Long = -1

        While (Not endFound) And nextNodeIndex >= 0
            'get next index to work with
            Dim smallestH As Double = Double.PositiveInfinity
            Dim smallestF As Double = Double.PositiveInfinity
            nextNodeIndex = -1
            For C1 As Long = 0 To nodeList.Count - 1
                Dim currentNode As clsAStarCard = nodeList(C1)
                If Not currentNode.isClosed Then
                    If currentNode.F < smallestF Then
                        'If currentNode.H < smallestH Then
                        smallestF = currentNode.F
                        'smallestH = currentNode.H
                        nextNodeIndex = C1
                        'Else
                        '    'this node's total way+estimated way to goal is longer than the last found node
                        'End If
                    Else
                        'this node is further away from the last found node
                    End If
                Else
                    'node is closed and not considered for evaluation
                End If
            Next

            'if node is not goal, 
            If Not (nextNodeIndex = -1) AndAlso Not nodeList(nextNodeIndex).node Is endPoint Then
                'continue search
                'we know the node now and can continue if we found a node
                If nextNodeIndex >= 0 Then
                    Dim currentNode As clsAStarCard = nodeList(nextNodeIndex)
                    Dim originNode As clsAStarCard = Nothing
                    If currentNode.originNodeIndex >= 0 Then originNode = nodeList(currentNode.originNodeIndex)

                    'RaiseEvent cardFound(currentNode)
                    'Threading.Thread.Sleep(10)


                    'close this node
                    currentNode.isClosed = True

                    'calculate new cards for all valid paths and add them to list of nodes
                    For Each singlePath As clsNavigationPath In currentNode.node.taxiWays
                        Dim nextNode As clsConnectionPoint = singlePath.oppositeTaxiWayPoint(currentNode.node)

                        Dim oldAngle As Double = currentNode.entryAngle
                        Dim newAngle As Double = singlePath.directionFrom(currentNode.node)

                        Dim angleDelta As Double = mdlHelpers.diffBetweenAnglesAbs(newAngle, oldAngle)

                        If Not angleDelta >= maxAngle Then

                            'in case of first node, there is not ID - we give it the pointer to the no-ID
                            Dim originNodeId As String = Me.noOriginId
                            If Not originNode Is Nothing Then originNodeId = originNode.node.objectID
                            Dim newProposedCombo As New clsVisitedComboCollection.structVisitedCombo With {.fromNodeID = originNodeId, .selfNodeID = currentNode.node.objectID, .toNodeID = nextNode.objectID}

                            If Not visitedComboList.containsCombo(newProposedCombo) Then
                                'add this combo first and then move on
                                visitedComboList.addCombo(newProposedCombo)

                                'calculate distance

                                'punishment for routes on runways
                                Dim malus As Double = 0
                                'in case taxiway is on runway, make it twice as long
                                If singlePath.type = clsWaySection.enumPathWayType.touchDownWay Or
                                    singlePath.type = clsWaySection.enumPathWayType.runwayTaxiWay Or
                                    singlePath.type = clsNavigationPath.enumPathWayType.exitWay Or
                                    singlePath.type = clsNavigationPath.enumPathWayType.lineUpWay Or
                                    singlePath.type = clsNavigationPath.enumPathWayType.takeOffWay Or
                                    singlePath.type = clsNavigationPath.enumPathWayType.touchDownWay Then
                                    malus = (singlePath.length)
                                End If

                                'new distance = length of path + malus + distance so far
                                Dim distanceFromStart As Double = singlePath.length + malus + currentNode.G
                                Dim distanceToEnd As Double = mdlHelpers.diffBetweenPoints2D(nextNode.pos_X, nextNode.pos_Y, endPoint.pos_X, endPoint.pos_Y)

                                Dim newCard As New clsAStarCard(distanceFromStart, distanceToEnd, singlePath.directionFrom(currentNode.node), nextNodeIndex, nextNode, singlePath)
                                nodeList.Add(newCard)


                            Else
                                'this combination of travel already exists and we dont look there again
                            End If
                        Else
                            'new path is more than max angle and will be ignored
                        End If
                    Next
                Else
                    'no way found
                End If
            Else
                'end node found
                endFound = True
                endNodeIndex = nextNodeIndex
            End If

        End While

        'decide what to do
        'if node found, determine wayu
        'if no node found, return empty list
        If endFound Then
            'move backwards from endnode to start
            nextNodeIndex = endNodeIndex

            While nextNodeIndex >= 0
                Dim currentCard As clsAStarCard = nodeList(nextNodeIndex)

                'find new step and add to top of list

                Dim newStep As New structPathStep With {.nextWayPoint = currentCard.node, .taxiwayToWayPoint = currentCard.wayToNode}
                result.Insert(0, newStep)
                nextNodeIndex = currentCard.originNodeIndex
            End While

        Else
            'way to end not found, so do nothing
        End If

        Return result
    End Function


End Class
