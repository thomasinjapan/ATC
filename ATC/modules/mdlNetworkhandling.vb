Option Explicit On

Imports System.IO
Imports System.Net.Sockets
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Xml.Serialization

Module mdlNetworkhandling
    <Serializable> Public Enum enumNetworkMessageType As Byte
        keyframe
        radioMessage
    End Enum

    <Serializable> Friend Structure structNetworkMessageFromServer
        Friend nwmMessageType As enumNetworkMessageType
        Friend nwmMessage As Object
    End Structure

    <Serializable> Friend Structure structRadioMessageNetwork
        Friend frequency As clsPlane.enumFrequency
        Friend message As String
    End Structure

    <Serializable> Friend Structure structNetworkKeyframeMessagefromServer
        Friend planeSkeletons As List(Of clsPlane.structPlaneSkeleton)
        Friend windDirectionTo As Double
        Friend openArrivalRunwayIDs As List(Of String)
        Friend openDepartureRunwayIDs As List(Of String)
        Friend usedRunwayIDs As List(Of String)
        Friend radioMessage As structRadioMessageNetwork
    End Structure


    Friend Sub serverSendUpdateToClients(ByRef game As clsGame, ByVal messageType As enumNetworkMessageType, Optional ByRef messageParameter As Object = Nothing)
        If game.TCPServerClientPlayers.Count > 0 Then

            'define what to send based on message type
            Dim messageContent As Object = Nothing
            Select Case messageType
                Case enumNetworkMessageType.keyframe
                    Dim planeSkeletons As New List(Of clsPlane.structPlaneSkeleton)

                    For Each singlePlane As clsPlane In game.Planes
                        Dim newplane As clsPlane.structPlaneSkeleton = singlePlane.skeleton

                        planeSkeletons.Add(newplane)
                    Next


                    messageContent = New structNetworkKeyframeMessagefromServer With {
                    .openArrivalRunwayIDs = game.AirPort.openArrivalRunwayIDsAsListOfStrings,
                    .openDepartureRunwayIDs = game.AirPort.openDepartureRunwayIDsAsListOfStrings,
                    .usedRunwayIDs = game.AirPort.usedRunwayIDsAsListOfStrings,
                    .windDirectionTo = game.AirPort.windDirectionTo,
                    .planeSkeletons = planeSkeletons
                    }
                Case enumNetworkMessageType.radioMessage
                    'need to cast to make sure that all info is correct
                    Dim radioMessage = DirectCast(messageParameter, structRadioMessageNetwork)
                    messageContent = New structRadioMessageNetwork With {
                        .frequency = radioMessage.frequency,
                        .message = radioMessage.message
                    }
            End Select

            'prepare the message
            Dim message As New mdlNetworkhandling.structNetworkMessageFromServer With {
                .nwmMessageType = messageType,
                .nwmMessage = messageContent
            }

            Try
                Dim formatter As New BinaryFormatter
                Dim streamTarget As New MemoryStream()
                Dim streamTargetMessage As New MemoryStream()

                formatter.Serialize(streamTargetMessage, message)
                'formatter.Serialize(streamTargetMessage, planeSkeletons(5))

                Dim byteArrayMessage() As Byte = streamTargetMessage.ToArray

                Dim arraySize As Int32 = byteArrayMessage.Length
                Dim arraySizeArray() As Byte = BitConverter.GetBytes(arraySize)
                Dim byteArray() As Byte = arraySizeArray.Concat(byteArrayMessage).ToArray

                ' Me.TCPServerClient.Send(byteArray)
                For Each singleClient As TcpClient In game.TCPServerClientPlayers
                    Dim stampTickStart As DateTime = Now
                    Console.WriteLine("end reading and handling package|" & Format(stampTickStart, "HH:mm:ss ffff"))
                    Console.WriteLine("package size sent to " & singleClient.Client.RemoteEndPoint.ToString & "|" & Format(stampTickStart, "HH:mm:ss ffff") & "|" & byteArray.Length)

                    singleClient.Client.Send(byteArray)

                    Dim stampTickEnd As DateTime = Now

                    Console.WriteLine("finished sending package|" & Format(stampTickEnd, "HH:mm:ss ffff"))
                    Console.WriteLine("duration sending package |" & (stampTickEnd - stampTickStart).TotalMilliseconds & "|" & (stampTickEnd - stampTickStart).Ticks)


                Next
            Catch ex As Exception

            End Try
        End If
    End Sub


    Friend Sub clientReceiveUpdateFromServer(ByRef game As clsGame)
        If game.serverConnected AndAlso game.TCPClientStream.DataAvailable Then
            Try
                Dim stampTickStart As DateTime = Now
                Console.WriteLine("reading and handling package|" & Format(stampTickStart, "HH:mm:ss ffff"))
                Dim arraySizeArray(4) As Byte
                game.TCPClientStream.Read(arraySizeArray, 0, CInt(4))
                Dim arraySize As Int32 = BitConverter.ToInt32(arraySizeArray, 0)

                Dim messageAsByteArray(arraySize) As Byte
                Dim reader As New BinaryReader(game.TCPClientStream)

                'Dim bytesread As Long = 0
                'While bytesread < arraySize
                messageAsByteArray = reader.ReadBytes(arraySize)
                '    bytesread += 1
                'End While

                'Dim message As structNetworkKeyframeMessagefromServer
                Dim message As structNetworkMessageFromServer
                Dim formatter As New BinaryFormatter

                Dim streamTarget As New MemoryStream(messageAsByteArray)
                message = formatter.Deserialize(streamTarget)

                Console.WriteLine("package size|" & streamTarget.Length)

                Select Case message.nwmMessageType
                    Case enumNetworkMessageType.keyframe
                        ClientReceiveKeyFrame(game, message.nwmMessage)
                    Case enumNetworkMessageType.radioMessage
                        ClientReceiveRadioMessage(game, message.nwmMessage)
                End Select



                Dim stampTickEnd As DateTime = Now
                Console.WriteLine("end reading and handling package|" & Format(stampTickEnd, "HH:mm:ss ffff"))
                Console.WriteLine("reading and handling package duration|" & (stampTickEnd - stampTickStart).TotalMilliseconds & "|" & (stampTickEnd - stampTickStart).Ticks)

                'RaiseEvent ticked((stampTickEnd - stampTickStart).TotalMilliseconds)
                game.raiseEventManuallyTicked((stampTickEnd - stampTickStart).TotalMilliseconds)
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.Critical)
            End Try
        End If
    End Sub

    Friend Sub ClientReceiveKeyFrame(ByRef game As clsGame, ByRef message As structNetworkKeyframeMessagefromServer)

        'check if new planes spawned
        For Each singlePlaneSkeleton As clsPlane.structPlaneSkeleton In message.planeSkeletons
            If game.getPlaneByCallsign(singlePlaneSkeleton.callsign) Is Nothing Then
                'we have a new plane and need to add it to the list
                Dim newPlane As New clsPlane(singlePlaneSkeleton, game.AirPort)

                game.Planes.Add(newPlane)
                'RaiseEvent game.spawnedPlane(newPlane)
                game.raiseEventManuallyPlaneSpawned(newPlane)
            End If
        Next

        'check if planes despawned
        Dim planesToRemove As New List(Of clsPlane)
        For Each singlePlane As clsPlane In game.Planes
            Dim planeFound As Boolean = False
            For Each singleSkeleton As clsPlane.structPlaneSkeleton In message.planeSkeletons
                If singleSkeleton.callsign.ToLower = singlePlane.callsign.ToLower Then
                    planeFound = True
                    Exit For
                End If
            Next

            If Not planeFound Then
                'plane is too much - we need to remove it
                planesToRemove.Add(singlePlane)
            End If
        Next
        For Each singlePlane As clsPlane In planesToRemove
            game.Planes.Remove(singlePlane)
            'RaiseEvent despawnedPlane(singlePlane)
            game.raiseEventManuallyPlaneDeSpawned(singlePlane)
        Next


        'check if values have changes

        'For Each singleplane As clsPlane In message.planes
        For Each singleplane As clsPlane.structPlaneSkeleton In message.planeSkeletons
            Dim forceUpdateState As Boolean = False              'do we manually raise the event of changed status
            Dim forceUpdateFrequency As Boolean = False              'do we manually raise the event of changed frequency
            Dim planeToChange As clsPlane = game.Planes.Find(Function(p As clsPlane) p.callsign = singleplane.callsign)

            '!!!fu*** is this ugly!
            'model info does not change, so we can leave it as it is
            If Not planeToChange.air_currentAirPathName = singleplane.air_currentAirPathName Then planeToChange.air_currentAirPathName = singleplane.air_currentAirPathName
            If Not planeToChange.callsign = singleplane.callsign Then planeToChange.callsign = singleplane.callsign
            If Not planeToChange.currentState = singleplane.currentState Then
                planeToChange.currentState = singleplane.currentState
                forceUpdateState = True
            End If
            If Not planeToChange.frequency = singleplane.Frequency Then
                planeToChange.frequency = singleplane.Frequency
                forceUpdateState = True
                forceUpdateFrequency = True
            End If
            If Not planeToChange.mov_speed_absolute.knots = singleplane.currentSpeedKnots Then planeToChange.mov_speed_absolute.knots = singleplane.currentSpeedKnots
            If Not planeToChange.mov_speed_rotation = singleplane.currentSpeedRotation Then planeToChange.mov_speed_rotation = singleplane.currentSpeedRotation
            If Not planeToChange.pointDetectionCircle.meters = singleplane.pointDetectionCircleMeters Then planeToChange.pointDetectionCircle.meters = singleplane.pointDetectionCircleMeters

            If Not planeToChange.pos_Altitude.feet = singleplane.currentAltitudeFeet Then planeToChange.pos_Altitude.feet = singleplane.currentAltitudeFeet
            If Not planeToChange.pos_direction = singleplane.currentDirection Then planeToChange.pos_direction = singleplane.currentDirection
            If Not planeToChange.pos_X.feet = singleplane.posXFeet Then planeToChange.pos_X.feet = singleplane.posXFeet
            If Not planeToChange.pos_Y.feet = singleplane.posYFeet Then planeToChange.pos_Y.feet = singleplane.posYFeet

            If Not planeToChange.target_altitude.feet = singleplane.targetAltitudeFeet Then planeToChange.target_altitude.feet = singleplane.targetAltitudeFeet

            If Not planeToChange.target_direction = singleplane.targetDirection Then planeToChange.target_direction = singleplane.targetDirection

            If Not planeToChange.target_speed.knots = singleplane.targetSpeedKnots Then planeToChange.target_speed.knots = singleplane.targetSpeedKnots


            If Not planeToChange.tower_LineUpApproved = singleplane.tower_LineUpApproved Then planeToChange.tower_LineUpApproved = singleplane.tower_LineUpApproved


            '!!! check if "IS" operator works since the info comes from remote; maybe check w/ names is better
            'for all items w/ ID, change only value if (only either one is NULL or plane to change is not null and their value is different
            'template to copy:
            '' point:   If (planeToChange.PC Is Nothing Xor singleplane.SP Is Nothing) Or (Not planeToChange.PC Is Nothing AndAlso Not (planeToChange.PC.objectID = singleplane.SP)) Then planeToChange.PC = AirPort.getNavigationPointById(singleplane.SP)
            '' path:   If (planeToChange.PC Is Nothing Xor singleplane.SP Is Nothing) Or (Not planeToChange.PC Is Nothing AndAlso Not (planeToChange.PC.objectID = singleplane.SP)) Then planeToChange.PC = AirPort.getNavigationPathById(singleplane.SP)

            If (planeToChange.air_currentAirWay Is Nothing Xor singleplane.air_currentAirWayID Is Nothing) Or (Not planeToChange.air_currentAirWay Is Nothing AndAlso Not (planeToChange.air_currentAirWay.ObjectID = singleplane.air_currentAirWayID)) Then planeToChange.air_currentAirWay = game.AirPort.getNavigationPathById(singleplane.air_currentAirWayID)

            'If Not planeToChange.air_flightPath Is singleplane.air_flightPath Then planeToChange.air_flightPath = singleplane.air_flightPath
            If Not singleplane.air_flightPathIDs Is Nothing Then
                planeToChange.air_flightPath = New List(Of clsAStarEngine.structPathStep)
                For Each singlePathStep As clsPlane.structPathStepSkeleton In singleplane.air_flightPathIDs
                    Dim newPathStep As clsAStarEngine.structPathStep
                    newPathStep = New clsAStarEngine.structPathStep With {
                                      .nextWayPoint = game.AirPort.getNavigationPointById(singlePathStep.navigationPointID),
                                      .taxiwayToWayPoint = game.AirPort.getNavigationPathById(singlePathStep.navigationPathID)
                                      }
                    planeToChange.air_flightPath.Add(newPathStep)
                Next
            End If


            If (planeToChange.air_goalWayPoint Is Nothing Xor singleplane.air_goalWayPointID Is Nothing) Or (Not planeToChange.air_goalWayPoint Is Nothing AndAlso Not (planeToChange.air_goalWayPoint.objectID = singleplane.air_goalWayPointID)) Then planeToChange.air_goalWayPoint = game.AirPort.getNavigationPointById(singleplane.air_goalWayPointID)
            If (planeToChange.air_nextWayPoint Is Nothing Xor singleplane.air_nextWayPointID Is Nothing) Or (Not planeToChange.air_nextWayPoint Is Nothing AndAlso Not (planeToChange.air_nextWayPoint.objectID = singleplane.air_nextWayPointID)) Then planeToChange.air_nextWayPoint = game.AirPort.getNavigationPointById(singleplane.air_nextWayPointID)
            If (planeToChange.air_terminal Is Nothing Xor singleplane.air_terminalID Is Nothing) Or (Not planeToChange.air_terminal Is Nothing AndAlso Not (planeToChange.air_terminal.objectID = singleplane.air_terminalID)) Then planeToChange.air_terminal = game.AirPort.getNavigationPointById(singleplane.air_terminalID)
            If Not planeToChange.air_altitudeOverrideByATC = singleplane.air_altitudeOverrideByATC Then planeToChange.air_altitudeOverrideByATC = singleplane.air_altitudeOverrideByATC

            If (planeToChange.ground_currentTaxiWay Is Nothing Xor singleplane.ground_CurrentTaxiWayID Is Nothing) Or (Not planeToChange.ground_currentTaxiWay Is Nothing AndAlso Not (planeToChange.ground_currentTaxiWay.ObjectID = singleplane.ground_CurrentTaxiWayID)) Then planeToChange.ground_currentTaxiWay = game.AirPort.getNavigationPathById(singleplane.ground_CurrentTaxiWayID)

            If (planeToChange.ground_goalWayPoint Is Nothing Xor singleplane.ground_goalWayPointID Is Nothing) Or (Not planeToChange.ground_goalWayPoint Is Nothing AndAlso Not (planeToChange.ground_goalWayPoint.objectID = singleplane.ground_goalWayPointID)) Then planeToChange.ground_goalWayPoint = game.AirPort.getNavigationPointById(singleplane.ground_goalWayPointID)
            If (planeToChange.ground_nextWayPoint Is Nothing Xor singleplane.ground_nextWayPointID Is Nothing) Or (Not planeToChange.ground_nextWayPoint Is Nothing AndAlso Not (planeToChange.ground_nextWayPoint.objectID = singleplane.ground_nextWayPointID)) Then planeToChange.ground_nextWayPoint = game.AirPort.getNavigationPointById(singleplane.ground_nextWayPointID)


            'If Not planeToChange.ground_taxiPath Is singleplane.ground_taxiPath Then planeToChange.ground_taxiPath = singleplane.ground_taxiPath
            If Not singleplane.ground_taxiPathIDs Is Nothing Then
                planeToChange.ground_taxiPath = New List(Of clsAStarEngine.structPathStep)
                For Each singlePathStep As clsPlane.structPathStepSkeleton In singleplane.ground_taxiPathIDs
                    Dim newPathStep As clsAStarEngine.structPathStep
                    newPathStep = New clsAStarEngine.structPathStep With {
                                  .nextWayPoint = game.AirPort.getNavigationPointById(singlePathStep.navigationPointID),
                                  .taxiwayToWayPoint = game.AirPort.getNavigationPathById(singlePathStep.navigationPathID)
                                  }
                    planeToChange.ground_taxiPath.Add(newPathStep)
                Next
            End If

            If (planeToChange.ground_terminal Is Nothing Xor singleplane.ground_terminalID Is Nothing) Or (Not planeToChange.ground_terminal Is Nothing AndAlso Not (planeToChange.ground_terminal.objectID = singleplane.ground_terminalID)) Then planeToChange.ground_terminal = game.AirPort.getNavigationPointById(singleplane.ground_terminalID)
            If (planeToChange.tower_assignedLandingPoint Is Nothing Xor singleplane.tower_assignedLandingPointID Is Nothing) Or (Not planeToChange.tower_assignedLandingPoint Is Nothing AndAlso Not (planeToChange.tower_assignedLandingPoint.objectID = singleplane.tower_assignedLandingPointID)) Then planeToChange.tower_assignedLandingPoint = game.AirPort.getNavigationPointById(singleplane.tower_assignedLandingPointID)
            If Not planeToChange.tower_cleardToLand = singleplane.tower_cleardToLand Then planeToChange.tower_cleardToLand = singleplane.tower_cleardToLand

            If (planeToChange.tower_currentTakeOffWay Is Nothing Xor singleplane.tower_currentTakeOffWayID Is Nothing) Or (Not planeToChange.tower_currentTakeOffWay Is Nothing AndAlso Not (planeToChange.tower_currentTakeOffWay.ObjectID = singleplane.tower_currentTakeOffWayID)) Then planeToChange.tower_currentTakeOffWay = game.AirPort.getNavigationPathById(singleplane.tower_currentTakeOffWayID)

            If (planeToChange.tower_currentTouchDownWay Is Nothing Xor singleplane.tower_currentTouchDownWayID Is Nothing) Or (Not planeToChange.tower_currentTouchDownWay Is Nothing AndAlso Not (planeToChange.tower_currentTouchDownWay.ObjectID = singleplane.tower_currentTouchDownWayID)) Then planeToChange.tower_currentTouchDownWay = game.AirPort.getNavigationPathById(singleplane.tower_currentTouchDownWayID)

            If (planeToChange.tower_goalTakeOffWayPoint Is Nothing Xor singleplane.tower_goalTakeOffWayPointID Is Nothing) Or (Not planeToChange.tower_goalTakeOffWayPoint Is Nothing AndAlso Not (planeToChange.tower_goalTakeOffWayPoint.objectID = singleplane.tower_goalTakeOffWayPointID)) Then planeToChange.tower_goalTakeOffWayPoint = game.AirPort.getNavigationPointById(singleplane.tower_goalTakeOffWayPointID)
            If (planeToChange.tower_goAroundPoint Is Nothing Xor singleplane.tower_goAroundPointID Is Nothing) Or (Not planeToChange.tower_goAroundPoint Is Nothing AndAlso Not (planeToChange.tower_goAroundPoint.objectID = singleplane.tower_goAroundPointID)) Then planeToChange.tower_goAroundPoint = game.AirPort.getNavigationPointById(singleplane.tower_goAroundPointID)
            If (planeToChange.tower_nextTakeOffWayPoint Is Nothing Xor singleplane.tower_nextTakeOffWayPointID Is Nothing) Or (Not planeToChange.tower_nextTakeOffWayPoint Is Nothing AndAlso Not (planeToChange.tower_nextTakeOffWayPoint.objectID = singleplane.tower_nextTakeOffWayPointID)) Then planeToChange.tower_nextTakeOffWayPoint = game.AirPort.getNavigationPointById(singleplane.tower_nextTakeOffWayPointID)
            If (planeToChange.tower_nextTouchDownPoint Is Nothing Xor singleplane.tower_nextTouchDownPointID Is Nothing) Or (Not planeToChange.tower_nextTouchDownPoint Is Nothing AndAlso Not (planeToChange.tower_nextTouchDownPoint.objectID = singleplane.tower_nextTouchDownPointID)) Then planeToChange.tower_nextTouchDownPoint = game.AirPort.getNavigationPointById(singleplane.tower_nextTouchDownPointID)
            If Not planeToChange.tower_takeOffApproved = singleplane.tower_takeOffApproved Then planeToChange.tower_takeOffApproved = singleplane.tower_takeOffApproved

            'If Not planeToChange.tower_takeOffPath Is singleplane.tower_takeOffPath Then planeToChange.tower_takeOffPath = singleplane.tower_takeOffPath
            If Not singleplane.tower_takeOffPathIDs Is Nothing Then
                planeToChange.tower_takeOffPath = New List(Of clsAStarEngine.structPathStep)
                For Each singlePathStep As clsPlane.structPathStepSkeleton In singleplane.tower_takeOffPathIDs
                    Dim newPathStep As clsAStarEngine.structPathStep
                    newPathStep = New clsAStarEngine.structPathStep With {
                              .nextWayPoint = game.AirPort.getNavigationPointById(singlePathStep.navigationPointID),
                              .taxiwayToWayPoint = game.AirPort.getNavigationPathById(singlePathStep.navigationPathID)
                              }
                    planeToChange.tower_takeOffPath.Add(newPathStep)
                Next
            End If

            'If Not planeToChange.tower_touchDownPath Is singleplane.tower_touchDownPath Then planeToChange.tower_touchDownPath = singleplane.tower_touchDownPath
            If Not singleplane.tower_touchDownPathIDs Is Nothing Then
                planeToChange.tower_touchDownPath = New List(Of clsAStarEngine.structPathStep)
                For Each singlePathStep As clsPlane.structPathStepSkeleton In singleplane.tower_touchDownPathIDs
                    Dim newPathStep As clsAStarEngine.structPathStep
                    newPathStep = New clsAStarEngine.structPathStep With {
                              .nextWayPoint = game.AirPort.getNavigationPointById(singlePathStep.navigationPointID),
                              .taxiwayToWayPoint = game.AirPort.getNavigationPathById(singlePathStep.navigationPathID)
                              }
                    planeToChange.tower_touchDownPath.Add(newPathStep)
                Next
            End If

            'find plane that changed and decide if we raise event to change status
            'If forceUpdateState Then RaiseEvent selectedPlaneStatusChanged(planeToChange)
            If forceUpdateState Then game.raiseEventManuallySelectedPlaneStatusChanged(planeToChange)
            'If forceUpdateFrequency Then RaiseEvent planeFrequencyChanged(planeToChange)
            If forceUpdateFrequency Then game.raiseEventManuallyPlaneFrequencyChanged(planeToChange)
        Next

        'wind
        game.AirPort.setWindDirection(message.windDirectionTo)

        'check if open for arrival runways changed
        ''check if the listed open runways are exactly the same as in the message
        ''if not, change the values and the UI
        Dim tmpOpenArrivalRunways As List(Of String) = message.openArrivalRunwayIDs

        'finds differences in currently open runways and messages
        Dim isListEqualArrival As Boolean = tmpOpenArrivalRunways.SequenceEqual(game.AirPort.openArrivalRunwayIDsAsListOfStrings)

        'if at least one difference, make local changes and update UI
        If Not isListEqualArrival Then
            For Each singleRunway As clsRunWay In game.AirPort.runWays
                If singleRunway.canHandleArrivals Then
                    If tmpOpenArrivalRunways.Contains(singleRunway.objectID) Then
                        singleRunway.isAvailableForArrival = True
                    Else
                        singleRunway.isAvailableForArrival = False
                    End If
                End If
            Next
            'RaiseEvent availableRunwaysArrivalChanged()
            game.raiseEventManuallyAvailableRunwaysArrivalChanged()
        End If

        'check if open for departure runways changed
        ''check if the listed open runways are exactly the same as in the message
        ''if not, change the values and the UI
        Dim tmpOpenDepartureRunways As List(Of String) = message.openDepartureRunwayIDs

        'finds differences in currently open runways and messages
        Dim isListEqualDeparture As Boolean = tmpOpenDepartureRunways.SequenceEqual(game.AirPort.openDepartureRunwayIDsAsListOfStrings)

        'if at least one difference, make local changes and update UI
        If Not isListEqualDeparture Then
            For Each singleRunway As clsRunWay In game.AirPort.runWays
                If tmpOpenDepartureRunways.Contains(singleRunway.objectID) Then
                    singleRunway.isAvailableForDeparture = True
                Else
                    singleRunway.isAvailableForDeparture = False
                End If
            Next
            'RaiseEvent availableRunwaysDepartureChanged()
            game.raiseEventManuallyAvailableRunwaysDepartureChanged()
        End If

        'check if blocked runway has changed
        ''check if the listed blocked runways are exactly the same as in the message
        ''if not, change the values and the UI
        Dim tmpBlockedDepartureRunways As List(Of String) = message.usedRunwayIDs

        'finds differences in currently open runways and messages
        Dim isListEqualBlocked As Boolean = tmpBlockedDepartureRunways.SequenceEqual(game.AirPort.usedRunwayIDsAsListOfStrings)

        'if at least one difference, make local changes and update UI
        If Not isListEqualBlocked Then
            For Each singleRunway As clsRunWay In game.AirPort.runWays
                If tmpBlockedDepartureRunways.Contains(singleRunway.objectID) Then
                    singleRunway.isInUse = True
                    If Not singleRunway.arrivalPoint Is Nothing Then singleRunway.arrivalPoint.isInUse = True
                Else
                    singleRunway.isInUse = False
                    If Not singleRunway.arrivalPoint Is Nothing Then singleRunway.arrivalPoint.isInUse = False
                End If
            Next
            'RaiseEvent usedRunwaysChanged()
            game.raiseEventManuallyUsedRunwaysChanged()
        End If

    End Sub

    Sub ClientReceiveRadioMessage(ByRef game As clsGame, ByRef message As structRadioMessageNetwork)
        game.raiseEventManuallyRadioMessage(message.frequency, message.message)
    End Sub

End Module
