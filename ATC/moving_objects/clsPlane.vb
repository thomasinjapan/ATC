Option Explicit On
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Security.Cryptography

<Serializable>
Public Class clsPlane
    Inherits clsMovingObject

    Friend Event cardFound(ByRef card As clsAStarCard)
    '    Friend WithEvents eventPathFinder As clsPathFinder
    Friend WithEvents eventPathFinder As clsAStarEngine

    <Serializable> Public Structure structPlaneTypeInfo
        Friend model As String
        Friend maker As String
        Friend modelShort As String
        Friend length As clsDistanceCollection
        Friend wingSpan As clsDistanceCollection
        Friend air_DescentSpeed As clsSpeedCollection
        Friend air_ClimbSpeed As clsSpeedCollection
        Friend air_AccelerationRate As clsSpeedCollection
        Friend air_DescelerationRate As clsSpeedCollection
        Friend air_Vstall As clsSpeedCollection       'stall speed, takeoffspeed
        Friend ReadOnly Property air_VRef As clsSpeedCollection             'default landing speed when reaching 50ft over runway
            Get
                Return New clsSpeedCollection(air_Vstall.knots * 1.3)
            End Get
        End Property
        Friend air_Vcruise As clsSpeedCollection                            'cruise speed
        Friend air_Vmo As clsSpeedCollection                                'max speed
        Friend air_Vfto_V2 As clsSpeedCollection                               'speed at takeoff
        Friend air_V2_CLIMB5000 As clsSpeedCollection                                 'speed until 5000 ft
        Friend air_V2_CLIMBFL150 As clsSpeedCollection                           'speed until FL150
        Friend air_V2_DESCENTABOVELTFL100 As clsSpeedCollection                    'descent speed between FL2350 and FL100
        Friend air_V2_DESCENTBELOWFL100 As clsSpeedCollection                    'descent speed below FL100
        Friend air_AltMax As clsDistanceCollection
        Friend air_AltCruise As clsDistanceCollection
        Friend air_AngleSpeed As Double
        Friend ground_AccelerationRate As clsSpeedCollection
        Friend ground_DescelerationRate As clsSpeedCollection
        Friend ground_AngleSpeed As Double
    End Structure

    <Serializable> Friend Structure structCommandInfo
        Friend plane As String
        Friend command As clsPlane.enumCommands
        Friend groundTaxiRunwayCommandParameter As clsRunWay
        Friend groundTaxiGoalPointCommandParameter As clsConnectionPoint
        Friend groundTaxiViaCommandParameter As List(Of clsConnectionPoint)
        Friend airDirectionCommandParameter As Integer
        Friend airNavPointCommandParameter As clsNavigationPoint
        Friend airAirPathCommandParameter As clsAirPath
        Friend airSpeedCommandParameter As Double
        Friend airAltitudeCommandParameter As Double
        Friend airTouchDownCommandParameter As clsTouchDownWayPoint
        Friend airGoAroundPointCommandParameter As clsNavigationPoint
        Friend towerExitPointCommandParameter As String
        Friend towerRunwayID As String
        Friend towerRunwayIsNewActiveForArrival As Boolean
        Friend towerRunwayIsNewActiveForDeparture As Boolean
        Friend radioMessage As clsGame.structRadioMessageNetwork
    End Structure

    <Serializable> Friend Structure structPathStepSkeleton
        Friend navigationPointID As String
        Friend navigationPathID As String
    End Structure


    <Serializable> Friend Structure structPlaneSkeleton
        Friend air_currentAirPathName As String
        Friend callsign As String
        Friend currentState As clsPlane.enumPlaneState
        Friend Frequency As clsPlane.enumFrequency
        Friend currentSpeedKnots As Double
        Friend currentSpeedRotation As Double                               '!!!do we need that?
        Friend pointDetectionCircleMeters As Double
        Friend currentAltitudeFeet As Double
        Friend currentDirection As Double
        Friend posXFeet As Double
        Friend posYFeet As Double
        Friend targetAltitudeFeet As Double
        Friend targetDirection As Double
        Friend targetSpeedKnots As Double
        Friend tower_LineUpApproved As Boolean
        Friend air_currentAirWayID As String
        Friend air_flightPathIDs As List(Of structPathStepSkeleton)         'structpathstep
        Friend air_goalWayPointID As String
        Friend air_nextWayPointID As String
        Friend air_terminalID As String
        Friend air_altitudeOverrideByATC As Boolean
        Friend ground_CurrentTaxiWayID As String
        Friend ground_goalWayPointID As String
        Friend ground_nextWayPointID As String
        Friend ground_taxiPathIDs As List(Of structPathStepSkeleton)        'structpathstep
        Friend ground_terminalID As String
        Friend tower_assignedLandingPointID As String
        Friend tower_cleardToLand As Boolean
        Friend tower_currentTakeOffWayID As String
        Friend tower_currentTouchDownWayID As String
        Friend tower_goalTakeOffWayPointID As String
        Friend tower_goAroundPointID As String
        Friend tower_nextTakeOffWayPointID As String
        Friend tower_nextTouchDownPointID As String
        Friend tower_takeOffApproved As Boolean
        Friend tower_takeOffPathIDs As List(Of structPathStepSkeleton)                    'structpathstep
        Friend tower_touchDownPathIDs As List(Of structPathStepSkeleton)                  'structpathstep
        Friend modelInfo As structPlaneTypeInfo                                                            '!!! too much data?
    End Structure


    'exact state of plane
    Public Enum enumPlaneState As Long
        undefined = 0                   'not defined, plane should never be in this state
        ground_atGate                   'plane is at gate and does nothgin
        ground_awaitingPushback         'plane is ready for Pushback and awaits pushback order
        ground_inPushback               'plane is in procedure of pushback
        ground_breaking                 'plane breaks when reaching the last navigation point or a halt point before going to holding position
        ground_holdingPosition          'plane is holding position
        ground_inTaxi                   'plane is taxiing
        ground_inParking                'plane is moving towards gate on gateway
        ground_preparingGate            'plane makes last meters before docking at gate (adjust angle and come to full stop)
        tower_inLineUp                  'plane is lining up on runway
        tower_linedupAndWaiting         'plane is lined up and awaits permission for takeoff
        tower_takingOff                 'plane Is In progress Of takeoff
        tower_Departed                  'plane departed
        tower_freeFlight                'plane is appraoching the runway in intend to touchdown
        tower_FinalApproach             'plane does final approach and follows FINAL path
        tower_enteringTouchDown         'plane reached point for touchdown and searches for way to gate and right landway
        tower_inTouchDown               'plane is touching down on runway and should be decreasting
        special_crashed                 'plane is crashed but still there
    End Enum

    'possible commands that can be given to a plane
    Public Enum enumCommands As Long
        noCommand = 0
        continueTaxi
        holdPosition
        pushbackApproved
        askForPushback
        groundExpectRunway
        taxiTo
        lineUpandWait
        lineUpandTakeOff
        clearForTakeOff
        cancelLineUp
        cancelTakeOff
        cancelLineupAndTakeoff
        airHeadToDirection
        airHeadToNavPoint
        airExpectRunway
        airMakeShortApproach
        airEnterFinal
        airClearedForLanding
        towerExitVia
        airGoAround
        airEnterSTAR
        airEnterSTARviaNavPoint
        airEnterSID
        airAdjustSpeed
        airAdjustAltitude
        contactTower
        contactArrival
        contactDeparture
        contactArrDep
        contactTracon
        contactGround
    End Enum

    'frequencies the plane can be connected to
    Public Enum enumFrequency As Long
        undefined = 0
        ground = 1
        tower = 2
        departure = 4
        arrival = 8
        appdep = 12
        tracon = 16
        radioOff = 32
    End Enum

    Private _currentstate As enumPlaneState
    Private _frequency As enumFrequency
    Friend Property callsign As String

    Friend ReadOnly Property isGroundRadarRelevant As Boolean
        Get
            Dim result As Boolean = False
            If Me.currentState = enumPlaneState.ground_atGate Or
               Me.currentState = enumPlaneState.ground_awaitingPushback Or
               Me.currentState = enumPlaneState.ground_breaking Or
               Me.currentState = enumPlaneState.ground_holdingPosition Or
               Me.currentState = enumPlaneState.ground_inParking Or
               Me.currentState = enumPlaneState.ground_inPushback Or
               Me.currentState = enumPlaneState.ground_inTaxi Or
               Me.currentState = enumPlaneState.ground_preparingGate Or
               Me.currentState = enumPlaneState.special_crashed Or
               Me.currentState = enumPlaneState.tower_inLineUp Or
               Me.currentState = enumPlaneState.tower_inTouchDown Or
               Me.currentState = enumPlaneState.tower_linedupAndWaiting Or
               Me.currentState = enumPlaneState.tower_takingOff Then
                result = True

            End If
            Return result
        End Get
    End Property
    Friend ReadOnly Property isTowerRadarRelevant As Boolean
        Get
            Dim result As Boolean = False
            If Me.currentState = clsPlane.enumPlaneState.tower_enteringTouchDown Or
                    Me.currentState = clsPlane.enumPlaneState.tower_freeFlight Or
                    Me.currentState = enumPlaneState.tower_FinalApproach Or
                    Me.currentState = clsPlane.enumPlaneState.tower_inLineUp Or
                    Me.currentState = clsPlane.enumPlaneState.tower_inTouchDown Or
                    Me.currentState = clsPlane.enumPlaneState.tower_linedupAndWaiting Or
                    Me.currentState = clsPlane.enumPlaneState.tower_takingOff Or
                    Me.currentState = enumPlaneState.tower_freeFlight Then
                result = True
            ElseIf Not Me.ground_currentTaxiWay Is Nothing AndAlso (
                Me.ground_currentTaxiWay.type = clsNavigationPath.enumPathWayType.exitWay Or
                Me.ground_currentTaxiWay.type = clsNavigationPath.enumPathWayType.lineUpWay Or
                Me.ground_currentTaxiWay.type = clsNavigationPath.enumPathWayType.runwayTaxiWay Or
                Me.ground_currentTaxiWay.type = clsNavigationPath.enumPathWayType.takeOffWay Or
                Me.ground_currentTaxiWay.type = clsNavigationPath.enumPathWayType.touchDownWay) Then

                'if on runway or exitway
                result = True

            End If

            Return result
        End Get
    End Property
    Friend ReadOnly Property isArrDepRadarRelevant As Boolean
        Get
            Dim result As Boolean = False
            If Me.currentState = clsPlane.enumPlaneState.tower_freeFlight Or
                    Me.currentState = enumPlaneState.tower_FinalApproach Or
                     Me.pos_Altitude.feet >= 100 Then
                result = True

            End If

            Return result
        End Get
    End Property

    'ground_taxi
    Friend Property ground_nextWayPoint As clsConnectionPoint
    Friend Property ground_lastReachedWayPoint As clsConnectionPoint
    Friend Property ground_goalWayPoint As clsConnectionPoint
    Friend Property ground_currentTaxiWay As clsNavigationPath
    Friend ReadOnly Property ground_breakingDistance As clsDistanceCollection
        Get
            Return New clsDistanceCollection((Me.mov_speed_absolute.inMetersPerSecond ^ 2) / (2 * Me.modelInfo.ground_DescelerationRate.inMetersPerSecond), clsDistanceCollection.enumDistanceUnits.meters)
        End Get
    End Property

    Friend Property ground_terminal As clsGate              'goal assigned by game

    Friend Property ground_taxiPath As List(Of clsAStarEngine.structPathStep)

    'tower_approach
    Friend Property final_nextWayPoint As clsConnectionPoint
    Friend Property final_goalWayPoint As clsConnectionPoint
    Friend Property final_currentAirWay As clsNavigationPath
    Friend Property final_flightPath As List(Of clsAStarEngine.structPathStep)
    Friend Property final_currentAirPathName As String

    'tower_touchdown
    Friend Property tower_assignedLandingPoint As clsTouchDownWayPoint
    Friend Property tower_touchDownPath As List(Of clsAStarEngine.structPathStep)
    Friend Property tower_nextTouchDownPoint As clsTouchDownWayPoint
    Friend Property tower_currentTouchDownWay As clsTouchDownWay
    Friend Property tower_goAroundPoint As clsNavigationPoint
    Friend Property tower_cleardToLand As Boolean
    Friend Property tower_assignedExitPointID As String               'waypoint plane is asked to exit if possible

    'tower_takeoff
    Friend Property tower_takeOffApproved As Boolean = False
    Friend Property tower_LineUpApproved As Boolean = False
    Friend Property tower_takeOffPath As List(Of clsAStarEngine.structPathStep)
    Friend Property tower_nextTakeOffWayPoint As clsConnectionPoint
    Friend Property tower_goalTakeOffWayPoint As clsConnectionPoint
    Friend Property tower_currentTakeOffWay As clsNavigationPath

    'air
    Friend Property air_nextWayPoint As clsConnectionPoint
    Friend Property air_goalWayPoint As clsConnectionPoint
    Friend Property air_currentAirWay As clsNavigationPath
    Friend Property air_flightPath As List(Of clsAStarEngine.structPathStep)
    Friend Property air_currentAirPathName As String
    Friend Property air_terminal As clsNavigationPoint
    Friend Property air_altitudeOverrideByATC As Boolean                    'true if altitude is defined by ATC and not default value (e.g. next point)
    Friend ReadOnly Property isArriving As Boolean
        Get
            Return (Me.air_terminal Is Nothing)
        End Get
    End Property
    Friend ReadOnly Property isDeparting As Boolean
        Get
            Return Not (Me.air_terminal Is Nothing)
        End Get
    End Property
    Friend ReadOnly Property awaitingOrders As Boolean
        Get
            Return Me.currentState = enumPlaneState.ground_awaitingPushback Or
                   Me.currentState = enumPlaneState.ground_holdingPosition Or
                   Me.currentState = enumPlaneState.tower_linedupAndWaiting Or
                   Me.currentState = enumPlaneState.undefined
        End Get
    End Property
    Friend Property air_FlightPathHistory As List(Of Tuple(Of clsDistanceCollection, clsDistanceCollection))

    'plane information
    Friend Property modelInfo As structPlaneTypeInfo

    Friend ReadOnly Property collisionRadius As clsDistanceCollection
        Get
            Return New clsDistanceCollection(Me.modelInfo.length.meters / 2, clsDistanceCollection.enumDistanceUnits.meters)
        End Get
    End Property

    Friend ReadOnly Property cockpitLocation As clsLocation
        Get
            Dim result As New clsLocation()
            'be sure not to hand over the pointer but the vaulues
            result.X.meters = Me.pos_X.meters + Me.collisionRadius.meters * Math.Sin(Me.pos_direction * Math.PI / 180)
            result.Y.meters = Me.pos_Y.meters - Me.collisionRadius.meters * Math.Cos(Me.pos_direction * Math.PI / 180)

            Return result
        End Get
    End Property

    Friend ReadOnly Property aftLocation As clsLocation
        Get
            Dim result As New clsLocation()
            'be sure not to hand over the pointer but the vaulues
            result.X.meters = Me.pos_X.meters - Me.collisionRadius.meters * Math.Sin(Me.pos_direction * Math.PI / 180)
            result.Y.meters = Me.pos_Y.meters + Me.collisionRadius.meters * Math.Cos(Me.pos_direction * Math.PI / 180)

            Return result
        End Get
    End Property

    Friend Property currentState As enumPlaneState
        Set(value As enumPlaneState)
            Me._currentstate = value
            RaiseEvent statusChanged(Me)
        End Set
        Get
            Return Me._currentstate
        End Get
    End Property

    Friend Property commandInfo As structCommandInfo

    Friend Property frequency As enumFrequency
        Set(value As enumFrequency)
            If Me._frequency <> value Then
                RaiseEvent radioMessage(Me, "Thanks, Have a nice day!")
                Me._frequency = value
                RaiseEvent frequencyChanged(Me)
                RaiseEvent radioMessage(Me, "On your frequency.")
            End If
        End Set
        Get
            Return Me._frequency
        End Get
    End Property

    Friend Event statusChanged(ByRef plane As clsPlane)
    Friend Event Departed(ByRef plane As clsPlane)
    Friend Event frequencyChanged(ByRef plane As clsPlane)

    '
    Friend ReadOnly Property skeleton As structPlaneSkeleton
        Get
            Dim result As New structPlaneSkeleton
            result.air_altitudeOverrideByATC = Me.air_altitudeOverrideByATC
            result.air_currentAirPathName = Me.air_currentAirPathName
            If Not Me.air_currentAirWay Is Nothing Then result.air_currentAirWayID = Me.air_currentAirWay.ObjectID

            If Not Me.air_flightPath Is Nothing Then
                result.air_flightPathIDs = New List(Of structPathStepSkeleton)
                For Each singleStep In Me.air_flightPath
                    Dim newPointID As String = singleStep.nextWayPoint.objectID
                    Dim newPathID As String = Nothing
                    If Not singleStep.taxiwayToWayPoint Is Nothing Then newPathID = singleStep.taxiwayToWayPoint.ObjectID
                    result.air_flightPathIDs.Add(New structPathStepSkeleton With {.navigationPointID = newPointID, .navigationPathID = newPathID})
                Next
            End If

            If Not Me.air_goalWayPoint Is Nothing Then result.air_goalWayPointID = Me.air_goalWayPoint.objectID
            If Not Me.air_nextWayPoint Is Nothing Then result.air_nextWayPointID = Me.air_nextWayPoint.objectID
            If Not Me.air_terminal Is Nothing Then result.air_terminalID = Me.air_terminal.objectID
            result.callsign = Me.callsign
            result.currentAltitudeFeet = Me.pos_Altitude.feet
            result.currentDirection = Me.pos_direction
            result.currentSpeedKnots = Me.mov_speed_absolute.knots
            result.currentSpeedRotation = Me.mov_speed_rotation
            result.currentState = Me.currentState
            result.Frequency = Me.frequency

            If Not Me.ground_currentTaxiWay Is Nothing Then result.ground_CurrentTaxiWayID = Me.ground_currentTaxiWay.ObjectID
            If Not Me.ground_goalWayPoint Is Nothing Then result.ground_goalWayPointID = Me.ground_goalWayPoint.objectID
            If Not Me.ground_nextWayPoint Is Nothing Then result.ground_nextWayPointID = Me.ground_nextWayPoint.objectID


            If Not Me.ground_taxiPath Is Nothing Then
                result.ground_taxiPathIDs = New List(Of structPathStepSkeleton)
                For Each singleStep In Me.ground_taxiPath
                    Dim newPointID As String = singleStep.nextWayPoint.objectID
                    Dim newPathID As String = Nothing
                    If Not singleStep.taxiwayToWayPoint Is Nothing Then newPathID = singleStep.taxiwayToWayPoint.ObjectID
                    result.ground_taxiPathIDs.Add(New structPathStepSkeleton With {.navigationPointID = newPointID, .navigationPathID = newPathID})
                Next
            End If


            If Not Me.ground_terminal Is Nothing Then result.ground_terminalID = Me.ground_terminal.objectID
            result.pointDetectionCircleMeters = Me.pointDetectionCircle.meters
            result.posXFeet = Me.pos_X.feet
            result.posYFeet = Me.pos_Y.feet
            result.targetAltitudeFeet = Me.target_altitude.feet
            result.targetDirection = Me.target_direction
            result.targetSpeedKnots = Me.target_speed.knots
            If Not Me.tower_assignedLandingPoint Is Nothing Then result.tower_assignedLandingPointID = Me.tower_assignedLandingPoint.objectID
            result.tower_cleardToLand = Me.tower_cleardToLand
            'result.tower_currentTakeOffWay = Me.tower_currentTakeOffWay
            If Not Me.tower_currentTakeOffWay Is Nothing Then result.tower_currentTakeOffWayID = Me.tower_currentTakeOffWay.ObjectID
            'result.tower_currentTouchDownWay = Me.tower_currentTouchDownWay
            If Not Me.tower_currentTouchDownWay Is Nothing Then result.tower_currentTouchDownWayID = Me.tower_currentTouchDownWay.ObjectID
            If Not Me.tower_goalTakeOffWayPoint Is Nothing Then result.tower_goalTakeOffWayPointID = Me.tower_goalTakeOffWayPoint.objectID
            If Not Me.tower_goAroundPoint Is Nothing Then result.tower_goAroundPointID = Me.tower_goAroundPoint.objectID
            result.tower_LineUpApproved = Me.tower_LineUpApproved
            If Not Me.tower_nextTakeOffWayPoint Is Nothing Then result.tower_nextTakeOffWayPointID = Me.tower_nextTakeOffWayPoint.objectID
            If Not Me.tower_nextTouchDownPoint Is Nothing Then result.tower_nextTouchDownPointID = Me.tower_nextTouchDownPoint.objectID
            result.tower_takeOffApproved = Me.tower_takeOffApproved



            If Not Me.tower_takeOffPath Is Nothing Then
                result.tower_takeOffPathIDs = New List(Of structPathStepSkeleton)
                For Each singleStep In Me.tower_takeOffPath
                    Dim newPointID As String = singleStep.nextWayPoint.objectID
                    Dim newPathID As String = Nothing
                    If Not singleStep.taxiwayToWayPoint Is Nothing Then newPathID = singleStep.taxiwayToWayPoint.ObjectID
                    result.tower_takeOffPathIDs.Add(New structPathStepSkeleton With {.navigationPointID = newPointID, .navigationPathID = newPathID})
                Next
            End If


            If Not Me.tower_touchDownPath Is Nothing Then
                result.tower_touchDownPathIDs = New List(Of structPathStepSkeleton)
                For Each singleStep In Me.tower_touchDownPath
                    Dim newPointID As String = singleStep.nextWayPoint.objectID
                    Dim newPathID As String = Nothing
                    If Not singleStep.taxiwayToWayPoint Is Nothing Then newPathID = singleStep.taxiwayToWayPoint.ObjectID
                    result.tower_touchDownPathIDs.Add(New structPathStepSkeleton With {.navigationPointID = newPointID, .navigationPathID = newPathID})
                Next
            End If

            result.modelInfo = Me.modelInfo

            Return result
        End Get
    End Property

    'for game meta info
    Friend Event landed(ByRef plane As clsPlane)
    Friend Event crashed(ByRef plane As clsPlane)
    Friend Event takenOff(ByRef plane As clsPlane)
    Friend Event gated(ByRef plane As clsPlane)
    Friend Event arrived(ByRef plane As clsPlane)
    Friend Event radioMessage(ByRef plane As clsPlane, ByVal message As String)

    Public Sub New()
        MyBase.New()
        Me.air_FlightPathHistory = New List(Of Tuple(Of clsDistanceCollection, clsDistanceCollection))
    End Sub

    Public Sub New(ByVal positiondata As structPosition, ByVal movementData As structMovement, ByVal accelerationProperties As structAcceleration, ByVal planetypeinfo As structPlaneTypeInfo, ByVal currentState As enumPlaneState, ByRef nextWayPoint As clsConnectionPoint, ByRef terminal As clsConnectionPoint, ByVal callsign As String, ByVal frequency As enumFrequency)
        MyBase.New(positiondata, movementData, accelerationProperties)
        Me.ground_nextWayPoint = nextWayPoint
        Me.modelInfo = planetypeinfo

        Me.ground_terminal = terminal

        Me.currentState = currentState
        Me.callsign = callsign
        Me.frequency = frequency
        Me.air_altitudeOverrideByATC = False

        Me.pointDetectionCircle = New clsDistanceCollection(2, clsDistanceCollection.enumDistanceUnits.meters)

        Me.air_FlightPathHistory = New List(Of Tuple(Of clsDistanceCollection, clsDistanceCollection))
    End Sub

    Friend Sub New(ByRef skeleton As structPlaneSkeleton, ByRef airport As clsAirport)
        MyBase.New()

        Me.air_FlightPathHistory = New List(Of Tuple(Of clsDistanceCollection, clsDistanceCollection))

        Me.modelInfo = skeleton.modelInfo
        Me.air_altitudeOverrideByATC = skeleton.air_altitudeOverrideByATC
        Me.air_currentAirPathName = skeleton.air_currentAirPathName
        'Me.air_currentAirWay = skeleton.air_currentAirWay
        Me.air_currentAirWay = airport.getNavigationPathById(skeleton.air_currentAirWayID)

        If Not skeleton.air_flightPathIDs Is Nothing Then
            Me.air_flightPath = New List(Of clsAStarEngine.structPathStep)

            For Each singlePathStep As structPathStepSkeleton In skeleton.air_flightPathIDs
                Dim newPathStep As clsAStarEngine.structPathStep
                newPathStep = New clsAStarEngine.structPathStep With {
                                      .nextWayPoint = airport.getNavigationPointById(singlePathStep.navigationPointID),
                                      .taxiwayToWayPoint = airport.getNavigationPathById(singlePathStep.navigationPathID)
                                      }
                Me.air_flightPath.Add(newPathStep)
            Next
        End If

        Me.air_goalWayPoint = airport.getNavigationPointById(skeleton.air_goalWayPointID)
        Me.air_nextWayPoint = airport.getNavigationPointById(skeleton.air_nextWayPointID)
        Me.air_terminal = airport.getNavigationPointById(skeleton.air_terminalID)
        Me.callsign = skeleton.callsign
        Me.pos_Altitude = New clsDistanceCollection(skeleton.currentAltitudeFeet, clsDistanceCollection.enumDistanceUnits.feet)
        Me.pos_direction = skeleton.currentDirection
        Me.mov_speed_absolute = New clsSpeedCollection(skeleton.currentSpeedKnots)
        Me.mov_speed_rotation = skeleton.currentSpeedRotation
        Me.currentState = skeleton.currentState
        Me.frequency = skeleton.Frequency
        'Me.ground_currentTaxiWay = skeleton.ground_CurrentTaxiWay
        Me.ground_currentTaxiWay = airport.getNavigationPathById(skeleton.ground_CurrentTaxiWayID)
        Me.ground_goalWayPoint = airport.getNavigationPointById(skeleton.ground_goalWayPointID)
        Me.ground_nextWayPoint = airport.getNavigationPointById(skeleton.ground_nextWayPointID)



        If Not skeleton.ground_taxiPathIDs Is Nothing Then
            Me.ground_taxiPath = New List(Of clsAStarEngine.structPathStep)

            For Each singlePathStep As structPathStepSkeleton In skeleton.ground_taxiPathIDs
                Dim newPathStep As clsAStarEngine.structPathStep
                newPathStep = New clsAStarEngine.structPathStep With {
                                      .nextWayPoint = airport.getNavigationPointById(singlePathStep.navigationPointID),
                                      .taxiwayToWayPoint = airport.getNavigationPathById(singlePathStep.navigationPathID)
                                      }
                Me.ground_taxiPath.Add(newPathStep)
            Next
        End If



        Me.ground_terminal = airport.getNavigationPointById(skeleton.ground_terminalID)
        Me.pointDetectionCircle = New clsDistanceCollection(skeleton.pointDetectionCircleMeters, clsDistanceCollection.enumDistanceUnits.meters)
        Me.pos_X = New clsDistanceCollection(skeleton.posXFeet, clsDistanceCollection.enumDistanceUnits.feet)
        Me.pos_Y = New clsDistanceCollection(skeleton.posYFeet, clsDistanceCollection.enumDistanceUnits.feet)
        Me.target_altitude = New clsDistanceCollection(skeleton.targetAltitudeFeet, clsDistanceCollection.enumDistanceUnits.feet)
        Me.target_direction = skeleton.targetDirection
        Me.target_speed = New clsSpeedCollection(skeleton.targetSpeedKnots)
        Me.tower_assignedLandingPoint = airport.getNavigationPointById(skeleton.tower_assignedLandingPointID)
        Me.tower_cleardToLand = skeleton.tower_cleardToLand
        'Me.tower_currentTakeOffWay = skeleton.tower_currentTakeOffWay
        Me.tower_currentTakeOffWay = airport.getNavigationPathById(skeleton.tower_currentTakeOffWayID)
        'Me.tower_currentTouchDownWay = skeleton.tower_currentTouchDownWay
        Me.tower_currentTouchDownWay = airport.getNavigationPathById(skeleton.tower_currentTouchDownWayID)
        Me.tower_goalTakeOffWayPoint = airport.getNavigationPointById(skeleton.tower_goalTakeOffWayPointID)
        Me.tower_goAroundPoint = airport.getNavigationPointById(skeleton.tower_goAroundPointID)
        Me.tower_LineUpApproved = skeleton.tower_LineUpApproved
        Me.tower_nextTakeOffWayPoint = airport.getNavigationPointById(skeleton.tower_nextTakeOffWayPointID)
        Me.tower_nextTouchDownPoint = airport.getNavigationPointById(skeleton.tower_nextTouchDownPointID)
        Me.tower_takeOffApproved = skeleton.tower_takeOffApproved


        If Not skeleton.tower_takeOffPathIDs Is Nothing Then
            Me.tower_takeOffPath = New List(Of clsAStarEngine.structPathStep)

            For Each singlePathStep As structPathStepSkeleton In skeleton.tower_takeOffPathIDs
                Dim newPathStep As clsAStarEngine.structPathStep
                newPathStep = New clsAStarEngine.structPathStep With {
                                      .nextWayPoint = airport.getNavigationPointById(singlePathStep.navigationPointID),
                                      .taxiwayToWayPoint = airport.getNavigationPathById(singlePathStep.navigationPathID)
                                      }
                Me.tower_takeOffPath.Add(newPathStep)
            Next
        End If

        If Not skeleton.tower_touchDownPathIDs Is Nothing Then
            Me.tower_touchDownPath = New List(Of clsAStarEngine.structPathStep)

            For Each singlePathStep As structPathStepSkeleton In skeleton.tower_touchDownPathIDs
                Dim newPathStep As clsAStarEngine.structPathStep
                newPathStep = New clsAStarEngine.structPathStep With {
                                      .nextWayPoint = airport.getNavigationPointById(singlePathStep.navigationPointID),
                                      .taxiwayToWayPoint = airport.getNavigationPathById(singlePathStep.navigationPathID)
                                      }
                Me.tower_touchDownPath.Add(newPathStep)
            Next
        End If


    End Sub


    'Friend Sub loadFromSkeleton(ByRef skeleton As structPlaneSkeleton, ByRef airport As clsAirport)
    '    Me.air_altitudeOverrideByATC = skeleton.air_altitudeOverrideByATC
    '    Me.air_currentAirPathName = skeleton.air_currentAirPathName
    '    Me.air_currentAirWay = skeleton.air_currentAirWay
    '    Me.air_flightPath = skeleton.air_flightPath
    '    Me.air_goalWayPoint = skeleton.air_goalWayPoint
    '    Me.air_nextWayPoint = skeleton.air_nextWayPoint
    '    Me.air_terminal = skeleton.air_terminal
    '    Me.callsign = skeleton.callsign
    '    Me.pos_Altitude.feet = skeleton.currentAltitudeFeet
    '    Me.pos_direction = skeleton.currentDirection
    '    Me.mov_speed_absolute.knots = skeleton.currentSpeedKnots
    '    Me.mov_speed_rotation = skeleton.currentSpeedRotation
    '    Me.currentState = skeleton.currentState
    '    Me.frequency = skeleton.Frequency
    '    Me.ground_currentTaxiWay = skeleton.ground_CurrentTaxiWay
    '    Me.ground_goalWayPoint = skeleton.ground_goalWayPoint
    '    Me.ground_nextWayPoint = airport.getConnectionPointById(skeleton.ground_nextWayPointID)
    '    Me.ground_taxiPath = skeleton.ground_taxiPath
    '    Me.ground_terminal = skeleton.ground_terminal
    '    Me.pointDetectionCircle.meters = skeleton.pointDetectionCircleMeters
    '    Me.pos_X.feet = skeleton.posXFeet
    '    Me.pos_Y.feet = skeleton.posYFeet
    '    Me.target_altitude.feet = skeleton.targetAltitudeFeet
    '    Me.target_direction = skeleton.targetDirection
    '    Me.target_speed.knots = skeleton.targetSpeedKnots
    '    Me.tower_assignedLandingPoint = skeleton.tower_assignedLandingPoint
    '    Me.tower_cleardToLand = skeleton.tower_cleardToLand
    '    Me.tower_currentTakeOffWay = skeleton.tower_currentTakeOffWay
    '    Me.tower_currentTouchDownWay = skeleton.tower_currentTouchDownWay
    '    Me.tower_goalTakeOffWayPoint = skeleton.tower_goalTakeOffWayPoint
    '    Me.tower_goAroundPoint = skeleton.tower_goAroundPoint
    '    Me.tower_LineUpApproved = skeleton.tower_LineUpApproved
    '    Me.tower_nextTakeOffWayPoint = skeleton.tower_nextTakeOffWayPoint
    '    Me.tower_nextTouchDownPoint = skeleton.tower_nextTouchDownPoint
    '    Me.tower_takeOffApproved = skeleton.tower_takeOffApproved
    '    Me.tower_takeOffPath = skeleton.tower_takeOffPath
    '    Me.tower_touchDownPath = skeleton.tower_touchDownPath
    'End Sub

    ''' <summary>
    ''' gets the shortest path to an end point
    ''' </summary>
    ''' <param name="endPoint">end point to be reached</param>
    ''' <param name="maxAngle">max angle that is allowed between two paths</param>
    Private Sub getShortestTaxiPath(ByRef endPoint As clsConnectionPoint, Optional ByVal maxAngle As Double = 360, Optional ByVal preferences As List(Of clsConnectionPoint) = Nothing)
        Dim angle As Double = Nothing

        'if plane is on a path, use the angle of the path
        'if plane is not on a path, use the angle of the plane
        If Me.ground_currentTaxiWay Is Nothing Then
            angle = Me.pos_direction
        Else
            'we need to use direction*to* since the next waypoint is upcoming and we are still on the path before
            angle = Me.ground_currentTaxiWay.directionTo(Me.ground_nextWayPoint)
        End If


        '!!!Dim pathfinder As New clsPathFinder(Me.ground_nextWayPoint, endPoint, angle, maxAngle, preferences)
        Dim pathfinder As New clsAStarEngine()
        Me.eventPathFinder = pathfinder
        Me.ground_taxiPath = pathfinder.Solution(Me.ground_nextWayPoint, endPoint, angle, maxAngle, preferences)


        '!!!Me.ground_taxiPath = pathfinder.absoluteShortestPath
        'Me.ground_taxiPath = pathfinder.findShortestPath

        GC.Collect()
    End Sub

    ''' <summary>
    ''' finds a path to a goal and prepare taxiing
    ''' </summary>
    ''' <param name="endPoint">end point to be reached</param>
    ''' <param name="maxAngle">max angle that is allowed between two paths</param>
    Friend Sub taxiTo(ByRef endPoint As clsConnectionPoint, Optional ByVal maxAngle As Double = 360, Optional ByVal preferences As List(Of clsConnectionPoint) = Nothing)

        'in case we find no path, save the old path
        Dim oldpath As New List(Of clsAStarEngine.structPathStep)
        oldpath = Me.ground_taxiPath

        'in case the plane is at a gate, briefly turn it by 180 degree to find a path
        If Me.ground_nextWayPoint.isGate Then
            Me.pos_direction += 180
        End If

        Dim stampTickStart As DateTime = Now
        Console.WriteLine("searchpath start|" & Format(stampTickStart, "HH:mm:ss ffff"))
        Me.getShortestTaxiPath(endPoint, maxAngle, preferences)
        Dim stampTickEnd As DateTime = Now
        Console.WriteLine("searchpath end|" & Format(stampTickEnd, "HH:mm:ss ffff"))
        Console.WriteLine("searchpath duration|" & (stampTickEnd - stampTickStart).TotalMilliseconds & "|" & (stampTickEnd - stampTickStart).Ticks)

        'in case the plane is at a gate, turn it back
        If Me.ground_nextWayPoint.isGate Then
            Me.pos_direction -= 180
        End If

        'in case no path was foudd, keep the old one
        If Me.ground_taxiPath Is Nothing Then
            'get old info back
            Me.ground_taxiPath = oldpath
        End If

        If Not Me.ground_taxiPath Is Nothing AndAlso Me.ground_taxiPath.Count > 0 Then
            Me.ground_lastReachedWayPoint = Nothing
            Me.ground_nextWayPoint = Me.ground_taxiPath.First.nextWayPoint
            If Not Me.ground_taxiPath.First.taxiwayToWayPoint Is Nothing Then Me.ground_currentTaxiWay = Me.ground_taxiPath.First.taxiwayToWayPoint
            Me.ground_goalWayPoint = Me.ground_taxiPath.Last.nextWayPoint
        Else
            '
        End If

        Me.cancelTakeOff()

    End Sub

    Friend Sub FlyToNavPoint(ByRef navPoint As clsNavigationPoint)
        Me.clearAirCommands(True, True)
        If Not Me.air_currentAirPathName Is Nothing Then Me.air_currentAirPathName = Nothing
        Me.air_flightPath.Add(New clsAStarEngine.structPathStep With {.nextWayPoint = navPoint, .taxiwayToWayPoint = Nothing})
        Me.air_currentAirWay = Me.air_flightPath.First.taxiwayToWayPoint
        Me.air_nextWayPoint = Me.air_flightPath.First.nextWayPoint
        Me.air_goalWayPoint = Me.air_flightPath.Last.nextWayPoint

        'adjust target altitude if not overridden by ATC and if a height exists
        If Not Me.air_altitudeOverrideByATC AndAlso Not Me.air_nextWayPoint.altitude Is Nothing Then
            Me.target_altitude.feet = Me.air_nextWayPoint.altitude.feet
            If Me.target_altitude.feet < 0 Then Me.target_altitude.feet = Me.modelInfo.air_AltCruise.feet
        End If


        Me.target_direction = clsNavigationPath.directionbetweenpoints(Me.pos_X.meters, Me.pos_Y.meters, navPoint.pos_X, navPoint.pos_Y)

    End Sub

    ''' <summary>
    ''' fetches the landing path for touchdown
    ''' </summary>
    ''' <param name="landingWayPoint">first point of the path</param>
    Friend Sub landFrom(ByVal landingWayPoint As clsTouchDownWayPoint)

        '!!!better would be if the info came from the runway
        Me.tower_touchDownPath = landingWayPoint.getLandingPath
        Me.ground_currentTaxiWay = Me.tower_touchDownPath.First.taxiwayToWayPoint
        Me.ground_nextWayPoint = Me.tower_touchDownPath.First.nextWayPoint
        ' Me.ground_goalWayPoint = Me.ground_goalWayPoint


        '!!!
        Me.target_speed.knots = 0
    End Sub

    ''' <summary>
    ''' clears command
    ''' </summary>
    Friend Sub clearCommand()
        Me.commandInfo = Nothing
    End Sub

    ''' <summary>
    ''' all actions that the plane does in a phase
    ''' </summary>
    ''' <param name="timespan">time that has passed since the last time tick was triggered</param>
    Friend Sub tick(ByVal timespan As TimeSpan)
        'define pointDetectionRadius based on timespan
        'Me.pointDetectionCircle = 1 + Me.mov_speed_absolute.inMetersPerSecond * timespan.TotalSeconds
        Me.pointDetectionCircle = New clsDistanceCollection(2 + Me.mov_speed_absolute.inMetersPerSecond * timespan.TotalSeconds, clsDistanceCollection.enumDistanceUnits.meters) '2 meters + movement speed

        Select Case Me.currentState

            Case enumPlaneState.ground_atGate
                Me.tickGroundAtGate(timespan)
            Case enumPlaneState.ground_awaitingPushback
                Me.tickGroundAwaitingPushback(timespan)
            Case enumPlaneState.ground_inPushback
                Me.tickGroundInPushback(timespan)
            Case enumPlaneState.ground_breaking
                Me.tickGroundBreaking(timespan)
            Case enumPlaneState.ground_holdingPosition
                tickHoldingPosition(timespan)
            Case enumPlaneState.ground_inTaxi
                tickGroundInTaxi(timespan)
            Case enumPlaneState.ground_inParking
                tickGroundInParking(timespan)
            Case enumPlaneState.ground_preparingGate
                tickGroundPreparingGate(timespan)
            Case enumPlaneState.tower_inLineUp
                tickTowerLiningUp(timespan)
            Case enumPlaneState.tower_linedupAndWaiting
                tickTowerLinedUpAndWaiting(timespan)
            Case enumPlaneState.tower_takingOff
                tickTowerTakingOff(timespan)
            Case enumPlaneState.tower_Departed
                RaiseEvent Departed(Me)
            Case enumPlaneState.tower_freeFlight
                tickTowerFreeFlight(timespan)
            Case enumPlaneState.tower_FinalApproach
                tickTowerFinalFlight(timespan)
            Case enumPlaneState.tower_enteringTouchDown
                tickTowerEnteringTouchDown(timespan)
            Case enumPlaneState.tower_inTouchDown
                tickTowerInTouchDown(timespan)
            Case enumPlaneState.special_crashed
                'do nothing

        End Select

        Me.handleCommands()

    End Sub

    Friend Sub handleCommands()

        Select Case Me.commandInfo.command

            Case enumCommands.askForPushback
                If Me.currentState = enumPlaneState.ground_atGate Then
                    Me.ground_taxiPath = New List(Of clsAStarEngine.structPathStep)
                    Me.currentState = enumPlaneState.ground_awaitingPushback
                    Me.frequency = enumFrequency.ground
                    RaiseEvent statusChanged(Me)
                    RaiseEvent radioMessage(Me, "Request Pushback for flight to " & Me.air_goalWayPoint.UIName & " via " & Me.air_currentAirPathName)
                End If
            Case enumCommands.pushbackApproved
                If Me.currentState = enumPlaneState.ground_awaitingPushback Then
                    'allow pushback only if a taxiway is plottet
                    If Me.ground_taxiPath.Count > 1 Then
                        '!!! turn around 180 deg
                        Me.pos_direction += 180

                        Me.warpTo(Me.ground_nextWayPoint, True)

                        'consume all navigationpoints inside of collisioncycle so the taxiing can proceed correctly
                        While (mdlHelpers.diffBetweenPoints2D(
                            Me.ground_nextWayPoint.pos_X, Me.ground_nextWayPoint.pos_Y,
                            Me.pos_X.meters, Me.pos_Y.meters) \ 1) <= (Me.collisionRadius.meters \ 1) + 1

                            Me.proceedOnTaxiPath()

                        End While


                        Me.currentState = enumPlaneState.ground_inPushback
                    End If
                End If
            Case enumCommands.taxiTo
                If Me.currentState = enumPlaneState.ground_awaitingPushback Or
                    Me.currentState = enumPlaneState.ground_holdingPosition Or
                    Me.currentState = enumPlaneState.ground_inTaxi Or
                    Me.currentState = enumPlaneState.tower_inTouchDown Or
                    Me.currentState = enumPlaneState.ground_inPushback Then

                    Me.taxiTo(Me.commandInfo.groundTaxiGoalPointCommandParameter, 95, Me.commandInfo.groundTaxiViaCommandParameter)

                    'if we target a runway, make sure that a flight is possible
                    If Not Me.commandInfo.groundTaxiRunwayCommandParameter Is Nothing Then

                        If Me.isDeparting Then
                            'if departing, give new SID if runway no SID assigned or newly assigned runway does not contain this SID
                            Dim foundSID As Boolean = False
                            For Each singleSID As clsAirPath In Me.commandInfo.groundTaxiRunwayCommandParameter.SIDs
                                If singleSID.name = Me.air_currentAirPathName Then
                                    foundSID = True
                                    Exit For
                                End If
                            Next

                            If Not foundSID Then
                                Me.prepareSID(Me.commandInfo.groundTaxiRunwayCommandParameter)
                            End If

                        Else
                            'else, let into free flight
                            Me.clearAirCommands(True, False)
                        End If
                    End If
                    RaiseEvent statusChanged(Me)
                End If

            Case enumCommands.holdPosition
                If currentState = enumPlaneState.ground_inTaxi Then
                    Me.currentState = enumPlaneState.ground_holdingPosition
                End If
            Case enumCommands.continueTaxi
                If Me.currentState = enumPlaneState.ground_holdingPosition Then
                    'only if goal is not already reached
                    If Me.ground_lastReachedWayPoint Is Me.ground_goalWayPoint Then
                        Me.currentState = enumPlaneState.ground_holdingPosition
                    Else
                        Me.target_speed.knots = Me.ground_currentTaxiWay.maxSpeed.knots
                        Me.currentState = enumPlaneState.ground_inTaxi
                    End If
                End If

            Case enumCommands.lineUpandWait
                If Me.currentState = enumPlaneState.ground_holdingPosition Or
                    Me.currentState = enumPlaneState.ground_inTaxi Then
                    'allow lineup, only if plane wants to go to runway (or is at a runway, but thats the same)
                    If Me.ground_goalWayPoint.isRunwayPoint Then
                        Me.prepareTakeOff(Me.ground_goalWayPoint)
                        Me.tower_LineUpApproved = True
                    End If
                End If

            Case enumCommands.clearForTakeOff
                If Me.currentState = enumPlaneState.tower_inLineUp Or
                    Me.currentState = enumPlaneState.tower_linedupAndWaiting Then
                    'allow takeoff only if in lineup or lineup is finished
                    '!!! do I need this?
                    'If Me.tower_nextTakeOffWayPoint.isLineUpPoint Then
                    Me.tower_takeOffApproved = True
                    'End If
                End If

            Case enumCommands.cancelTakeOff
                Me.tower_takeOffApproved = False

            Case enumCommands.cancelLineUp
                Me.tower_LineUpApproved = False

            Case enumCommands.cancelLineupAndTakeoff
                Me.tower_takeOffApproved = False
                Me.tower_LineUpApproved = False

            Case enumCommands.lineUpandTakeOff
                If Me.currentState = enumPlaneState.ground_holdingPosition Or
                    Me.currentState = enumPlaneState.ground_inTaxi Then
                    'allow takeoff, only if plane wants to go to runway (or is at a runway, bit thats the same)
                    If Me.ground_goalWayPoint.isRunwayPoint Then
                        Me.prepareTakeOff(Me.ground_goalWayPoint)
                        Me.tower_LineUpApproved = True
                        Me.tower_takeOffApproved = True
                    End If
                End If
            Case enumCommands.airHeadToDirection
                If Me.currentState = enumPlaneState.tower_freeFlight Or Me.currentState = enumPlaneState.tower_FinalApproach Then
                    Me.target_direction = Me.commandInfo.airDirectionCommandParameter
                    If Not Me.air_currentAirPathName Is Nothing Then Me.leaveAirPath()
                    Me.clearAirCommands(True, True)
                    Me.currentState = enumPlaneState.tower_freeFlight
                    RaiseEvent statusChanged(Me)
                End If
            Case enumCommands.airAdjustAltitude
                If Me.currentState = enumPlaneState.tower_freeFlight Or Me.currentState = enumPlaneState.tower_FinalApproach Then
                    Me.target_altitude.feet = Me.commandInfo.airAltitudeCommandParameter
                    Me.air_altitudeOverrideByATC = True
                    Me.currentState = enumPlaneState.tower_freeFlight
                    RaiseEvent statusChanged(Me)
                End If
            Case enumCommands.airAdjustSpeed
                If Me.currentState = enumPlaneState.tower_freeFlight Or Me.currentState = enumPlaneState.tower_FinalApproach Then
                    Me.target_speed.knots = Me.commandInfo.airSpeedCommandParameter
                    RaiseEvent statusChanged(Me)
                End If
            Case enumCommands.airHeadToNavPoint
                If Me.currentState = enumPlaneState.tower_freeFlight Or Me.currentState = enumPlaneState.tower_FinalApproach Then
                    Me.FlyToNavPoint(Me.commandInfo.airNavPointCommandParameter)

                    Me.currentState = enumPlaneState.tower_freeFlight
                    RaiseEvent statusChanged(Me)
                End If
            Case enumCommands.airEnterSTAR
                If Me.currentState = enumPlaneState.tower_freeFlight Or Me.currentState = enumPlaneState.tower_FinalApproach Then
                    'enter STAR only if arriving
                    If Me.isArriving Then Me.enterAirPath(Me.commandInfo.airAirPathCommandParameter)

                    Me.currentState = enumPlaneState.tower_freeFlight
                    RaiseEvent statusChanged(Me)
                End If
            Case enumCommands.airEnterSTARviaNavPoint
                If Me.currentState = enumPlaneState.tower_freeFlight Or Me.currentState = enumPlaneState.tower_FinalApproach Then
                    'only if plane is arriving
                    If Me.isArriving Then
                        Me.FlyToNavPoint(Me.commandInfo.airNavPointCommandParameter)
                        Me.enterAirPath(Me.commandInfo.airAirPathCommandParameter)

                        Me.currentState = enumPlaneState.tower_freeFlight
                        RaiseEvent statusChanged(Me)
                    End If
                End If

            Case enumCommands.airEnterSID
                If Me.currentState = enumPlaneState.tower_freeFlight Or Me.currentState = enumPlaneState.tower_FinalApproach Then
                    'enter SID only if departing
                    If Me.isDeparting Then
                        Me.enterAirPath(Me.commandInfo.airAirPathCommandParameter)
                        'set terminal to end of new path
                        '!!! do we need this? if so, give player penalty
                        Me.air_terminal = Me.air_flightPath.Last.nextWayPoint

                        Me.currentState = enumPlaneState.tower_freeFlight
                        RaiseEvent statusChanged(Me)
                    End If
                End If
            Case enumCommands.airExpectRunway
                If Me.currentState = enumPlaneState.tower_freeFlight Or Me.currentState = enumPlaneState.tower_FinalApproach Then
                    'only if arriving
                    If Me.isArriving Then
                        Me.tower_assignedLandingPoint = Me.commandInfo.airTouchDownCommandParameter
                        Me.tower_goAroundPoint = Me.commandInfo.airGoAroundPointCommandParameter

                        'in case the flightpath is empty but a next waypoint is set, plane is in already in the last meters
                        'in this case correct the next waypoint to runway
                        If Me.air_nextWayPoint Is Nothing Then
                            Me.air_nextWayPoint = tower_assignedLandingPoint
                        Else
                            If Me.air_flightPath.Count = 0 Then
                                Me.air_nextWayPoint = Me.commandInfo.airTouchDownCommandParameter
                            End If
                        End If

                        'remove landing relevant information
                        Me.tower_assignedExitPointID = Nothing
                        Me.tower_cleardToLand = False
                        RaiseEvent statusChanged(Me)
                    End If
                End If
            Case enumCommands.airClearedForLanding
                If Me.currentState = enumPlaneState.tower_freeFlight Or Me.currentState = enumPlaneState.tower_FinalApproach Then
                    'only clear if runway is defined
                    If Not Me.tower_assignedLandingPoint Is Nothing Then
                        Me.tower_cleardToLand = True
                        RaiseEvent statusChanged(Me)
                    End If
                End If
            Case enumCommands.airGoAround
                If Me.currentState = enumPlaneState.tower_freeFlight Or Me.currentState = enumPlaneState.tower_FinalApproach Then
                    Me.tower_cleardToLand = False
                    RaiseEvent statusChanged(Me)
                End If
            Case enumCommands.airEnterFinal
                If Me.currentState = enumPlaneState.tower_freeFlight Or Me.currentState = enumPlaneState.tower_FinalApproach Then
                    'only accept this command in case plane is arriving and has a runway assigned
                    If Not Me.tower_assignedLandingPoint Is Nothing And Me.isArriving Then
                        Me.enterFinalPath()
                        Me.leaveAirPath()
                        Me.currentState = enumPlaneState.tower_FinalApproach
                        RaiseEvent statusChanged(Me)
                    End If
                End If
            Case enumCommands.airMakeShortApproach
                If Me.currentState = enumPlaneState.tower_freeFlight Or Me.currentState = enumPlaneState.tower_FinalApproach Then
                    'only accept this command in case plane is arriving and has a runway assigned
                    If Not Me.tower_assignedLandingPoint Is Nothing And Me.isArriving Then
                        Me.leaveAirPath()
                        Me.leaveFinalPath()
                        Me.enterShortApproach()
                        Me.currentState = enumPlaneState.tower_freeFlight
                        RaiseEvent statusChanged(Me)
                    End If
                End If
            Case enumCommands.towerExitVia
                If Not Me.tower_assignedLandingPoint Is Nothing Then
                    Me.tower_assignedExitPointID = Me.commandInfo.towerExitPointCommandParameter

                    RaiseEvent statusChanged(Me)
                End If

        End Select


        'handle radio frequency change requests
        Select Case Me.commandInfo.command
            Case enumCommands.contactArrival
                Me.frequency = enumFrequency.arrival
                RaiseEvent statusChanged(Me)
            Case enumCommands.contactDeparture
                Me.frequency = enumFrequency.departure
                RaiseEvent statusChanged(Me)
            Case enumCommands.contactGround
                Me.frequency = enumFrequency.ground
                RaiseEvent statusChanged(Me)
            Case enumCommands.contactTower
                Me.frequency = enumFrequency.tower
                RaiseEvent statusChanged(Me)
            Case enumCommands.contactArrDep
                Me.frequency = enumFrequency.appdep
                RaiseEvent statusChanged(Me)
            Case enumCommands.contactTracon
                Me.frequency = enumFrequency.tracon
                RaiseEvent statusChanged(Me)
        End Select
        Me.clearCommand()
    End Sub

    Friend Sub prepareSID(ByRef runway As clsRunWay)
        Dim randomizer As New Random(DateTime.Now.Millisecond)
        '!!! do not decide SID yet, since it depends on runway which SID to pick
        Dim selectedSID As clsAirPath = runway.SIDs(randomizer.Next(0, runway.SIDs.GetUpperBound(0) + 1))

        'define terminal so we can prepare despawn
        Me.air_terminal = selectedSID.path.Last.nextWayPoint

        'enter SID even if not started
        Me.enterAirPath(selectedSID)
    End Sub

    Friend Sub tickGroundAtGate(ByVal timespan As TimeSpan)
        'wait for speed being 0, direction parking direction and trigger to request pushback
        'handle commands from ATC
        Dim gate As clsGate = Me.ground_nextWayPoint
    End Sub

    Friend Sub tickGroundAwaitingPushback(ByVal timespan As TimeSpan)
        'do nothing until we know what runway to take


        'wait for pushback approval
    End Sub

    Friend Sub tickGroundInPushback(ByVal timespan As TimeSpan)
        '!!!for the moment, treat like taxi

        Me.taxiing(timespan)

        'check if next taxiwaypoint reached, get next taxipoint and hold position
        If (mdlHelpers.diffBetweenPoints2D(Me.cockpitLocation.X.meters, Me.cockpitLocation.Y.meters, Me.ground_nextWayPoint.pos_X, Me.ground_nextWayPoint.pos_Y) < Me.pointDetectionCircle.meters) Then 'And Me.currentState = enumPlaneState.ground_inPushback

            'if next point is hold point, last waypoint brake the plane
            '!!!
            Me.ground_lastReachedWayPoint = Me.ground_nextWayPoint
            Dim nextPoint As clsConnectionPoint = Me.ground_nextWayPoint
            'Dim nextTaxiWay As clsNavigationPath = Me.ground_currentTaxiWay
            Dim nextTaxiWay As clsNavigationPath = Me.ground_taxiPath(1).taxiwayToWayPoint
            If nextPoint.isHoldPoint Or (Not nextTaxiWay.type = clsNavigationPath.enumPathWayType.gateWay And Not nextTaxiWay Is Nothing) Then
                Me.currentState = enumPlaneState.ground_breaking
            Else
                Me.proceedOnTaxiPath()
            End If
        End If

    End Sub

    Friend Sub tickGroundBreaking(ByVal timespan As TimeSpan)
        'this status shall make sure that the plane is at a valid position at the end of this phase to allow taxi from there
        '!!! - better is a slow breaking based on difference to point and break way
        'Me.taxiing(timespan)

        Me.mov_speed_absolute.knots = 0
        Me.warpTo(Me.ground_nextWayPoint)

        'check if next taxiwaypoint reached, get nexttaxipoint
        While (mdlHelpers.diffBetweenPoints2D(Me.cockpitLocation.X.meters, Me.cockpitLocation.Y.meters, Me.ground_nextWayPoint.pos_X, Me.ground_nextWayPoint.pos_Y) < Me.pointDetectionCircle.meters) And (Me.currentState = enumPlaneState.ground_breaking)
            Me.ground_lastReachedWayPoint = ground_nextWayPoint
            Me.currentState = enumPlaneState.ground_holdingPosition
        End While


    End Sub

    Friend Sub tickHoldingPosition(ByVal timespan As TimeSpan)
        '!!! still dirty due to mix up of command and state

        Me.target_speed.knots = 0
        'only for rotation
        '!!!
        Me.taxiing(timespan)

        'check if next taxiwaypoint reached, get nexttaxipoint
        If (mdlHelpers.diffBetweenPoints2D(Me.cockpitLocation.X.meters, Me.cockpitLocation.Y.meters, Me.ground_nextWayPoint.pos_X, Me.ground_nextWayPoint.pos_Y) < Me.pointDetectionCircle.meters) Then
            Me.ground_lastReachedWayPoint = Me.ground_nextWayPoint
            Dim nextPoint As clsConnectionPoint = Me.ground_nextWayPoint
            If nextPoint.isHoldPoint Then
                'have check to avoid flickering of a plane that already holds
                If Not Me.currentState = enumPlaneState.ground_holdingPosition Then
                    'hold the plane
                    Me.currentState = enumPlaneState.ground_holdingPosition
                End If
            End If

            If Me.ground_nextWayPoint Is Me.ground_goalWayPoint And Me.ground_goalWayPoint.isRunwayPoint Then
                If Me.tower_LineUpApproved Then
                    Me.currentState = enumPlaneState.tower_inLineUp

                    'delete groundcommands, except plane is still inarrival and accidently sent to restart
                    If Not Me.isArriving Then Me.clearGroundCommands()
                    '!!!risk: user has command that is taxi to --> leads to crash
                Else
                    'do nothing and wait
                End If
            ElseIf Me.ground_nextWayPoint Is Me.ground_goalWayPoint Then
                'if end point is reached, do nothing
            Else
                Me.proceedOnTaxiPath()
            End If
        End If


    End Sub

    Friend Sub tickGroundInTaxi(ByVal timespan As TimeSpan)
        '!!! still dirty due to mix of status and command

        Me.taxiing(timespan)

        'check if next taxiwaypoint reached, get nexttaxipoint
        While (mdlHelpers.diffBetweenPoints2D(Me.cockpitLocation.X.meters, Me.cockpitLocation.Y.meters, Me.ground_nextWayPoint.pos_X, Me.ground_nextWayPoint.pos_Y) < Me.pointDetectionCircle.meters) And (Me.currentState = enumPlaneState.ground_inTaxi)
            'memorized this point as reached
            Me.ground_lastReachedWayPoint = Me.ground_nextWayPoint
            'if next point is hold point, last waypoint brake the plance
            Dim nextPoint As clsConnectionPoint = Me.ground_nextWayPoint
            If nextPoint.isHoldPoint Or nextPoint Is Me.ground_goalWayPoint Then
                Me.currentState = enumPlaneState.ground_breaking
            ElseIf Me.ground_nextWayPoint Is Me.ground_goalWayPoint And Me.ground_goalWayPoint.isRunwayPoint And Me.tower_LineUpApproved Then
                'check if next point is takeoff point, if so transition to takeoff if lineup is approved
                Me.currentState = enumPlaneState.tower_inLineUp
                'Me.ground_taxiPath = Nothing
                'delete groundcommands, except plane is still inarrival and accidently sent to restart
                If Not Me.isArriving Then Me.clearGroundCommands()

                '!!!risk: user has command that is taxi to --> leads to crash
            Else
                Me.proceedOnTaxiPath()
            End If

            'check if new path is gateway
            If Me.ground_currentTaxiWay.type = clsNavigationPath.enumPathWayType.gateWay Then
                Me.currentState = enumPlaneState.ground_inParking
            End If

        End While


    End Sub

    Friend Sub tickGroundInParking(ByVal timespan As TimeSpan)
        Me.taxiing(timespan)

        'check if next taxiwaypoint reached, get nexttaxipoint
        While (mdlHelpers.diffBetweenPoints2D(Me.cockpitLocation.X.meters, Me.cockpitLocation.Y.meters, Me.ground_nextWayPoint.pos_X, Me.ground_nextWayPoint.pos_Y) < Me.pointDetectionCircle.meters) And (Me.currentState = enumPlaneState.ground_inParking)
            Me.ground_lastReachedWayPoint = Me.ground_nextWayPoint
            'if next point is hold point, set status to hold
            Dim nextPoint As clsConnectionPoint = Me.ground_nextWayPoint
            If nextPoint.isHoldPoint Then
                Me.currentState = enumPlaneState.ground_holdingPosition
            End If

            'if next point is gate, change to gating
            If Me.ground_nextWayPoint.isGate Then
                'stop immediately not great but the best I can come up with now to avoid crashing into the terminal
                Me.mov_speed_absolute.knots = 0

                Me.target_speed.knots = 0
                Dim gate As clsGate = Me.ground_nextWayPoint
                Me.target_direction = gate.parkingDirection
                Me.currentState = enumPlaneState.ground_preparingGate
            Else
                'if reached, change to next waypoint
                Me.proceedOnTaxiPath()
            End If
        End While

    End Sub

    Friend Sub tickGroundPreparingGate(ByVal timespan As TimeSpan)
        Dim gate As clsGate = Me.ground_nextWayPoint
        Me.taxiing(timespan)

        If Me.target_direction = gate.parkingDirection And Me.mov_speed_absolute.knots = 0 Then
            Me.warpTo(gate)

            Me.currentState = enumPlaneState.ground_atGate
            Me.frequency = enumFrequency.radioOff

            'game meta
            RaiseEvent gated(Me)
            If gate Is Me.ground_terminal Then RaiseEvent arrived(Me)

            'delete all taxi information except for the current waypoint
            Me.ground_taxiPath = Nothing
            Me.ground_terminal = Nothing
            Me.ground_currentTaxiWay = Nothing
            Me.ground_goalWayPoint = Nothing

        End If

    End Sub

    Friend Sub tickTowerLiningUp(ByVal timespan As TimeSpan)
        '!!! still dirty due to mix up of command and status
        Me.takingoff(timespan)

        'check if next taxiwaypoint reached, get nexttaxipoint
        While (mdlHelpers.diffBetweenPoints2D(Me.cockpitLocation.X.meters, Me.cockpitLocation.Y.meters, Me.tower_nextTakeOffWayPoint.pos_X, Me.tower_nextTakeOffWayPoint.pos_Y) < Me.pointDetectionCircle.meters) And (Me.currentState = enumPlaneState.tower_inLineUp)
            Me.ground_lastReachedWayPoint = Me.ground_nextWayPoint
            'if next point is hold point, last waypoint brake the plance
            Dim nextPoint As clsConnectionPoint = Me.tower_nextTakeOffWayPoint
            If nextPoint.isLineUpPoint Then
                If Not Me.tower_takeOffApproved Then
                    Me.currentState = enumPlaneState.tower_linedupAndWaiting
                Else
                    Me.currentState = enumPlaneState.tower_takingOff
                End If
            Else
                Me.proceedOnTakeOffPath()
            End If

        End While
    End Sub

    Friend Sub tickTowerLinedUpAndWaiting(ByVal timespan As TimeSpan)
        'this status shall make sure that the plane Is at a valid position at the end of this phase to allow taxi from there
        '!!! - better is a slow breaking based on difference to point and break way
        Me.mov_speed_absolute.knots = 0
        Me.warpTo(Me.tower_nextTakeOffWayPoint)


        'check if next taxiwaypoint reached, get nexttaxipoint
        If (mdlHelpers.diffBetweenPoints2D(Me.cockpitLocation.X.meters, Me.cockpitLocation.Y.meters, Me.tower_nextTakeOffWayPoint.pos_X, Me.tower_nextTakeOffWayPoint.pos_Y) < Me.pointDetectionCircle.meters) Then
            Me.ground_lastReachedWayPoint = Me.ground_nextWayPoint
            'if takeoff approved, do takeoff
            If Me.tower_takeOffApproved Then
                Me.currentState = enumPlaneState.tower_takingOff
            Else
                'do nothing
            End If
        End If

    End Sub

    Friend Sub tickTowerTakingOff(ByVal timespan As TimeSpan)
        Me.takingoff(timespan)

        While (mdlHelpers.diffBetweenPoints2D(Me.cockpitLocation.X.meters, Me.cockpitLocation.Y.meters, Me.tower_nextTakeOffWayPoint.pos_X, Me.tower_nextTakeOffWayPoint.pos_Y) < Me.pointDetectionCircle.meters) And (Me.currentState = enumPlaneState.tower_takingOff)
            Me.ground_lastReachedWayPoint = Me.ground_nextWayPoint
            'if endpoint, release
            If tower_nextTakeOffWayPoint Is tower_goalTakeOffWayPoint Then
                Me.target_speed.knots = Me.modelInfo.air_V2_CLIMB5000.knots
                If Not Me.air_nextWayPoint Is Nothing AndAlso Not Me.air_nextWayPoint.altitude Is Nothing Then
                    'only update altitude if there is no override by ATC and a default altitude is set
                    If Not Me.air_altitudeOverrideByATC AndAlso Not Me.air_nextWayPoint.altitude Is Nothing Then
                        Me.target_altitude.feet = Me.air_nextWayPoint.altitude.feet
                        'in case height is negative, change to cruisespeed
                        If Me.target_altitude.feet < 0 Then Me.target_altitude.feet = Me.modelInfo.air_AltCruise.feet
                    End If
                End If
                Me.currentState = enumPlaneState.tower_freeFlight

                'game meta
                RaiseEvent takenOff(Me)
            Else
                Me.proceedOnTakeOffPath()
            End If
        End While

        '!!! add height here
    End Sub

    Friend Sub tickTowerEnteringTouchDown(ByVal timespan As TimeSpan)
        'plane reached an entrypoint of a runway for touchdown
        'now it checks if it is allowed to land
        'if no, goaround
        'else search paths on runway and taxiwat to gatey

        'first check if plane has goal attached
        'if not, add one


        'check if all conditions for landing are met
        Dim allowedToLand As Boolean = True
        Dim goaroundreason As String = ""

        'cleardtoland not given
        If Not Me.tower_cleardToLand Then
            allowedToLand = False
            goaroundreason = "no clearance"
        End If

        'too fast aka breaking way too short compared to rest of runay
        If Me.ground_breakingDistance.meters > Me.tower_assignedLandingPoint.runwayLength.meters Then
            allowedToLand = False
            goaroundreason = "too fast"
        End If
        'angle too high
        If mdlHelpers.diffBetweenAnglesAbs(Me.pos_direction, Me.tower_assignedLandingPoint.landingAngle) > 45 Then
            allowedToLand = False
            goaroundreason = "angle too steep to runway"
        End If

        'max height
        If Me.pos_Altitude.feet > Me.tower_assignedLandingPoint.altitude.feet Then
            allowedToLand = False
            goaroundreason = "too high"
        End If

        'wind direction
        If mdlHelpers.diffBetweenAnglesAbs(Me.pos_direction, Me.tower_assignedLandingPoint.windDirectionFrom) > Me.tower_assignedLandingPoint.maxCrossWind Then
            allowedToLand = False
            goaroundreason = "angle too steep to wind"
        End If

        'if runway is free to land
        If Not Me.tower_assignedLandingPoint.isAvailableForArrival Then
            allowedToLand = False
            goaroundreason = "runway not available"
        End If

        'if runway is not still in use by other plane
        If Me.tower_assignedLandingPoint.isInUse Then
            allowedToLand = False
            goaroundreason = "runway in use"
        End If

        '!!! if runway is active missing


        If allowedToLand Then
            'delete groundcommands but keep terminal so we can reset the way
            Me.clearGroundCommands(True)
            Me.tower_nextTouchDownPoint = Me.tower_assignedLandingPoint
            Me.ground_nextWayPoint = Me.tower_assignedLandingPoint
            Me.ground_goalWayPoint = Me.ground_terminal
            '!!!Me.taxiTo(Me.ground_terminal)
            Me.landFrom(Me.tower_assignedLandingPoint)
            Me.currentState = enumPlaneState.tower_inTouchDown
            Me.warpTo(Me.tower_assignedLandingPoint)
            If Not Me.ground_nextWayPoint.altitude Is Nothing Then Me.target_altitude.feet = Me.ground_nextWayPoint.altitude.feet Else Me.target_altitude.feet = 0

            'delete all airborne related information
            Me.clearAirCommands(False, False)

        Else
            'Me.currentState = enumPlaneState.tower_goingAround
            Me.FlyToNavPoint(Me.tower_goAroundPoint)
            Me.target_speed.knots = Me.modelInfo.air_Vcruise.knots
            Me.tower_goAroundPoint = Nothing
            Me.tower_assignedLandingPoint = Nothing
            Me.tower_cleardToLand = False
            Me.currentState = enumPlaneState.tower_freeFlight
            RaiseEvent radioMessage(Me, "going around - " & goaroundreason)

        End If
    End Sub

    Friend Sub tickTowerInTouchDown(ByVal timespan As TimeSpan)
        'plane runs down the runway until it reaches a point
        Me.touchingdown(timespan)

        'if a point is reached
        While (mdlHelpers.diffBetweenPoints2D(Me.cockpitLocation.X.meters, Me.cockpitLocation.Y.meters, Me.tower_nextTouchDownPoint.pos_X, Me.tower_nextTouchDownPoint.pos_Y) < Me.pointDetectionCircle.meters) And (Me.currentState = enumPlaneState.tower_inTouchDown)
            Me.ground_lastReachedWayPoint = Me.ground_nextWayPoint
            ' check if plane is slow enough
            If Me.mov_speed_absolute.knots <= Me.target_speed.knots Then
                ' check if the point is either the intended exitpoint, the last point or NULL (="")
                If Me.tower_assignedExitPointID = "" Or
                    Me.tower_nextTouchDownPoint.objectID = Me.tower_assignedExitPointID Or
                    Me.tower_nextTouchDownPoint Is Me.tower_touchDownPath.Last.nextWayPoint Then

                    'identify point To exit via
                    'find exit path and first point on it that is not on the runway
                    Dim preferences As New List(Of clsConnectionPoint)
                    For Each singlePath As clsNavigationPath In Me.tower_nextTouchDownPoint.taxiWays
                        If Not TypeOf (singlePath) Is clsTouchDownWay Then
                            'found a path that is not touchdownway
                            'add the first point on this way to the preference and exit loop
                            preferences.Add(singlePath.oppositeTaxiWayPoint(tower_nextTouchDownPoint))
                            Exit For
                        End If
                    Next

                    'taxi via preference
                    Me.taxiTo(Me.ground_goalWayPoint, 95, preferences)
                    'Me.taxiTo(Me.ground_goalWayPoint, 95)
                    Me.currentState = enumPlaneState.ground_inTaxi
                    If Me.ground_taxiPath Is Nothing Then
                        Me.currentState = enumPlaneState.ground_holdingPosition
                    End If
                    Me.tower_touchDownPath = Nothing
                    Me.tower_assignedLandingPoint = Nothing
                    Me.tower_assignedExitPointID = ""
                    Me.cancelTakeOff()

                    '!!!
                    Me.clearAirCommands(True, False)
                    Me.clearTowerLandingCommands()

                    'game meta
                    RaiseEvent landed(Me)
                Else

                    'proceed to next point since we are not at the right exit yet
                    Me.proceedOnLandWay()
                End If
            Else
                'proceed to next point since we are still too fast
                Me.proceedOnLandWay()
            End If
        End While

    End Sub

    Friend Sub tickTowerFreeFlight(ByVal timespan As TimeSpan)
        Me.flying(timespan)

        'if we have a waypoint to reach
        'check if next taxiwaypoint reached, get nexttaxipoint
        While (Not Me.air_nextWayPoint Is Nothing) AndAlso (mdlHelpers.diffBetweenPoints2D(Me.pos_X.meters, Me.pos_Y.meters, Me.air_nextWayPoint.pos_X, Me.air_nextWayPoint.pos_Y) < Me.pointDetectionCircle.meters) AndAlso (Me.currentState = enumPlaneState.tower_freeFlight)
            If Me.air_nextWayPoint Is Me.tower_assignedLandingPoint Then
                'we arrived at the landing point and we transition to touchdown
                Me.currentState = enumPlaneState.tower_enteringTouchDown
            ElseIf Me.air_nextWayPoint Is Me.air_terminal Then
                'we arrived at the intended terminal so we can despawn the plane
                Me.currentState = enumPlaneState.tower_Departed
            Else
                'proceed on flightpath or if last, return to free flight
                Me.proceedOnFlightPath()
            End If
        End While

    End Sub

    Friend Sub tickTowerFinalFlight(ByVal timespan As TimeSpan)
        Me.finaling(timespan)

        'if we have a waypoint to reach
        'check if next taxiwaypoint reached, get nexttaxipoint
        While (Not Me.final_nextWayPoint Is Nothing) AndAlso (mdlHelpers.diffBetweenPoints2D(Me.pos_X.meters, Me.pos_Y.meters, Me.final_nextWayPoint.pos_X, Me.final_nextWayPoint.pos_Y) < Me.pointDetectionCircle.meters) AndAlso (Me.currentState = enumPlaneState.tower_FinalApproach)
            If Me.final_nextWayPoint Is Me.tower_assignedLandingPoint Then
                'we arrived at the landing point and we transition to touchdown
                Me.currentState = enumPlaneState.tower_enteringTouchDown
            Else
                'proceed on flightpath or if last, return to free flight
                Me.proceedOnFinalPath()
            End If
        End While

    End Sub

    ''' <summary>
    ''' proceed to taxi based on currently defined path
    ''' </summary>
    ''' <param name="timespan">time that has passed since last tick and shall be represented here</param>
    Friend Sub taxiing(ByVal timespan As TimeSpan)
        'adjust speed
        Me.adjustGroundSpeed(timespan)

        'adjust angle
        ''first adjust targetangle
        ''If status is not prepare gate, calculate best angle
        If Not Me.currentState = enumPlaneState.ground_preparingGate Then
            'if coordinates are the same, the result is 0 although the current direction may be correct
            'so rotate only if points are different
            If Not (Me.pos_X.meters = Me.ground_nextWayPoint.pos_X And Me.pos_Y.meters = Me.ground_nextWayPoint.pos_Y) Then
                Me.target_direction = clsNavigationPath.directionbetweenpoints(Me.pos_X.meters, Me.pos_Y.meters, Me.ground_nextWayPoint.pos_X, Me.ground_nextWayPoint.pos_Y)
            End If

        Else
            Dim gate As clsGate = Me.ground_nextWayPoint
            Me.target_direction = gate.parkingDirection
        End If

        '' then adjust actgual angle to move
        Me.adjustGroundAngle(timespan)

        'move
        MyBase.move(timespan)

    End Sub

    Friend Sub flying(ByVal timespan As TimeSpan)
        'adjust airspeed
        Me.adjustAirSpeed(timespan)

        'adjust airdirection
        '' only if we have a defined direction through way
        If Not Me.air_nextWayPoint Is Nothing Then        ''first calculate targetangle
            ''If coordinates Then are the same, the result Is 0 although the current direction may be correct
            ''so rotate only if points are different
            If Not (Me.pos_X.meters = Me.air_nextWayPoint.pos_X And Me.pos_Y.meters = Me.air_nextWayPoint.pos_Y) Then
                Me.target_direction = clsNavigationPath.directionbetweenpoints(Me.pos_X.meters, Me.pos_Y.meters, Me.air_nextWayPoint.pos_X, Me.air_nextWayPoint.pos_Y)
            End If
        End If

        'then adjust actual angle to move
        Me.adjustAirAngle(timespan)

        'move
        'save old height in case the change of height makes a difference
        Dim oldheight As New clsDistanceCollection(Me.pos_Altitude.feet, clsDistanceCollection.enumDistanceUnits.feet)
        MyBase.move(timespan)
        Me.adjustAltitude(timespan)

        'increase speed in case plane is departing and crosses thresholds
        If Me.isDeparting Then
            If oldheight.feet < 5000 And Me.pos_Altitude.feet >= 5000 Then
                Me.target_speed.knots = Me.modelInfo.air_V2_CLIMBFL150.knots
            ElseIf oldheight.feet < 15000 And Me.pos_Altitude.feet >= 15000 Then
                Me.target_speed.knots = Me.modelInfo.air_V2_CLIMBFL150.knots
            End If
        End If

        'decrease speed in case plane is arriving and crosses thresholds
        If Me.isArriving Then
            If oldheight.feet > 24000 And Me.pos_Altitude.feet <= 24000 Then
                Me.target_speed.knots = Me.modelInfo.air_V2_DESCENTABOVELTFL100.knots
            ElseIf oldheight.feet > 10000 And Me.pos_Altitude.feet <= 10000 Then
                Me.target_speed.knots = Me.modelInfo.air_V2_DESCENTBELOWFL100.knots
            End If
        End If



    End Sub

    Friend Sub finaling(ByVal timespan As TimeSpan)
        'adjust airspeed
        Me.adjustAirSpeed(timespan)

        'adjust airdirection
        '' only if we have a defined direction through way
        If Not Me.final_nextWayPoint Is Nothing Then        ''first calculate targetangle
            ''If coordinates Then are the same, the result Is 0 although the current direction may be correct
            ''so rotate only if points are different
            If Not (Me.pos_X.meters = Me.final_nextWayPoint.pos_X And Me.pos_Y.meters = Me.final_nextWayPoint.pos_Y) Then
                Me.target_direction = clsNavigationPath.directionbetweenpoints(Me.pos_X.meters, Me.pos_Y.meters, Me.final_nextWayPoint.pos_X, Me.final_nextWayPoint.pos_Y)
            End If
        End If

        'then adjust actual angle to move
        Me.adjustAirAngle(timespan)

        'move
        MyBase.move(timespan)
        Me.adjustAltitude(timespan)

    End Sub

    ''' <summary>
    ''' adjusts height of plan based on targetheight
    ''' </summary>
    ''' <param name="timespan">time that has passed since last time</param>
    Friend Sub adjustAltitude(ByVal timespan As TimeSpan)
        If Me.pos_Altitude.feet = Me.target_altitude.feet Then
            'do nothing
        ElseIf Me.pos_Altitude.feet < Me.target_altitude.feet Then
            'if current height is smaller than expected, climb until maxheight is reached
            Me.pos_Altitude.feet += Me.modelInfo.air_ClimbSpeed.inFeetPerSecond * timespan.TotalSeconds
            If Me.pos_Altitude.feet > Me.target_altitude.feet Then Me.pos_Altitude.feet = Me.target_altitude.feet
            If Me.pos_Altitude.feet > Me.modelInfo.air_AltMax.feet Then Me.pos_Altitude.feet = Me.modelInfo.air_AltMax.feet
        ElseIf Me.pos_Altitude.feet > Me.target_altitude.feet Then
            'if current height is heigher than expected, descent until minheight is reached
            Me.pos_Altitude.feet -= Me.modelInfo.air_DescentSpeed.inFeetPerSecond * timespan.TotalSeconds
            If Me.pos_Altitude.feet < Me.target_altitude.feet Then Me.pos_Altitude.feet = Me.target_altitude.feet
        End If
    End Sub

    ''' <summary>
    ''' adjusts the airspeed based on targetspeed and maxspeed
    ''' </summary>
    ''' <param name="timespan">time that has passed since last time</param>
    Friend Sub adjustAirSpeed(ByVal timespan As TimeSpan)
        If Me.mov_speed_absolute.knots = Me.target_speed.knots Then
            'do nothgin
        ElseIf Me.mov_speed_absolute.knots < Me.target_speed.knots Then
            'accelerate and if targetspeed crossed trim down to target speed
            '!!! plane maxspeed is not covered yet!
            '!!! change speed to class and allow adjustment in knots
            Me.mov_speed_absolute.inMetersPerSecond += Me.modelInfo.air_AccelerationRate.inMetersPerSecond * timespan.TotalSeconds
            If Me.mov_speed_absolute.knots > Me.target_speed.knots Then Me.mov_speed_absolute.knots = Me.target_speed.knots
        ElseIf Me.mov_speed_absolute.knots > Me.target_speed.knots Then
            'decelerate and if targetspeed crossed trim to target speed
            '!!! change speed to class and allow adjustment in knots
            Me.mov_speed_absolute.inMetersPerSecond -= Me.modelInfo.air_AccelerationRate.inMetersPerSecond * timespan.TotalSeconds
            If Me.mov_speed_absolute.knots < Me.target_speed.knots Then Me.mov_speed_absolute.knots = Me.target_speed.knots
        End If

        If Me.mov_speed_absolute.knots > Me.modelInfo.air_Vmo.knots Then Me.mov_speed_absolute.knots = Me.modelInfo.air_Vmo.knots

    End Sub

    ''' <summary>
    ''' adjusts the groundspeed based on targetspeed and maxspeed
    ''' </summary>
    ''' <param name="timespan">time that has passed since last time</param>
    Friend Sub adjustGroundSpeed(ByVal timespan As TimeSpan)
        If Me.mov_speed_absolute.knots = Me.target_speed.knots Then
            'do nothing
        ElseIf Me.mov_speed_absolute.knots < Me.target_speed.knots Then
            'accelerate and if targetspeed crossed, trim down to target speed
            Me.mov_speed_absolute.inMetersPerSecond += Me.modelInfo.ground_AccelerationRate.inMetersPerSecond * timespan.TotalSeconds
            If Me.mov_speed_absolute.knots > Me.target_speed.knots Then Me.mov_speed_absolute.knots = Me.target_speed.knots

        ElseIf Me.mov_speed_absolute.knots > Me.target_speed.knots Then
            'decelerate and if target speed crossed, trim to target speed
            Me.mov_speed_absolute.inMetersPerSecond -= Me.modelInfo.ground_DescelerationRate.inMetersPerSecond * timespan.TotalSeconds
            If Me.mov_speed_absolute.knots < Me.target_speed.knots Then Me.mov_speed_absolute.knots = Me.target_speed.knots
        End If

        If Me.mov_speed_absolute.knots > Me.modelInfo.air_Vcruise.knots Then Me.mov_speed_absolute.knots = Me.modelInfo.air_Vcruise.knots

    End Sub


    Friend Sub crash()
        'check before to raise event only once
        If Not Me.currentState = enumPlaneState.special_crashed Then
            Me.currentState = enumPlaneState.special_crashed
            Me.pos_Altitude.feet = 0
            RaiseEvent crashed(Me)
        End If
    End Sub

    ''' <summary>
    ''' proceed touchdown movemement on currently defined path
    ''' </summary>
    ''' <param name="timespan">time that has passed since last tick and shall be represented here</param>
    Friend Sub touchingdown(ByVal timespan As TimeSpan)
        'adjust speed
        Me.adjustGroundSpeed(timespan)

        'adjust angle
        ''first adjust targetangle
        If Not (Me.pos_X.meters = Me.tower_nextTouchDownPoint.pos_X And Me.pos_Y.meters = Me.tower_nextTouchDownPoint.pos_Y) Then
            Me.target_direction = clsNavigationPath.directionbetweenpoints(Me.pos_X.meters, Me.pos_Y.meters, Me.tower_nextTouchDownPoint.pos_X, Me.tower_nextTouchDownPoint.pos_Y)
        End If

        '' then adjust actgual angle to move
        Me.adjustGroundAngle(timespan)

        'move
        MyBase.move(timespan)
        Me.adjustAltitude(timespan)

    End Sub

    ''' <summary>
    ''' proceed to take off based on the takeoff path
    ''' </summary>
    ''' <param name="timespan">time that has passed since last tick and shall be represented here</param>
    Friend Sub takingoff(ByVal timespan As TimeSpan)
        'adjust speed
        Me.adjustGroundSpeed(timespan)

        'adjust angle
        ''first adjust targetangle
        Me.target_direction = clsNavigationPath.directionbetweenpoints(Me.pos_X.meters, Me.pos_Y.meters, Me.tower_nextTakeOffWayPoint.pos_X, Me.tower_nextTakeOffWayPoint.pos_Y)

        '' then adjust actgual angle to move
        Me.adjustGroundAngle(timespan)

        'move
        MyBase.move(timespan)
        If Me.mov_speed_absolute.knots >= Me.modelInfo.air_Vfto_V2.knots Then Me.adjustAltitude(timespan)        'increase height only if speed is heigh enough

    End Sub

    ''' <summary>
    ''' deletes the current taxiwaypoint and sets the next in the list while updating the taxipath
    ''' </summary>
    Friend Sub proceedOnTaxiPath()

        If Not Me.ground_taxiPath Is Nothing AndAlso Me.ground_taxiPath.Count > 0 Then
            Me.ground_taxiPath.RemoveAt(0)

            'check if there is still a following path, else stop
            If Me.ground_taxiPath.Count > 0 Then
                Me.ground_nextWayPoint = Me.ground_taxiPath(0).nextWayPoint
                Me.ground_currentTaxiWay = Me.ground_taxiPath(0).taxiwayToWayPoint

                Me.target_speed.knots = Me.ground_currentTaxiWay.maxSpeed.knots
                Me.target_direction = clsNavigationPath.directionbetweenpoints(Me.pos_X.meters, Me.pos_Y.meters, Me.ground_nextWayPoint.pos_X, Me.ground_nextWayPoint.pos_Y)
            Else
                Me.commandInfo = New structCommandInfo With {.command = enumCommands.holdPosition, .plane = Me.callsign}
            End If
        Else
            Me.commandInfo = New structCommandInfo With {.command = enumCommands.holdPosition, .plane = Me.callsign}
        End If
    End Sub


    ''' <summary>
    ''' deletes the current waypoint and set up a new waypoint if there is one
    ''' </summary>
    Friend Sub proceedOnFinalPath()
        Me.final_flightPath.RemoveAt(0)

        If Me.final_flightPath.Count > 0 Then
            Me.final_nextWayPoint = Me.final_flightPath.First.nextWayPoint
            Me.final_currentAirWay = Me.final_flightPath.First.taxiwayToWayPoint

            'only update speed if a default speed is set
            If Not Me.final_currentAirWay.maxSpeed Is Nothing Then Me.target_speed.knots = Me.final_currentAirWay.maxSpeed.knots

            'only update altitude if there is no override by ATC and a default altitude is set
            If Not Me.air_altitudeOverrideByATC AndAlso Not Me.final_nextWayPoint.altitude Is Nothing Then
                Me.target_altitude.feet = Me.final_nextWayPoint.altitude.feet
                'in case height is negative, change to cruisespeed
                If Me.target_altitude.feet < 0 Then Me.target_altitude.feet = Me.modelInfo.air_AltCruise.feet
            End If
            Me.target_direction = clsNavigationPath.directionbetweenpoints(Me.pos_X.meters, Me.pos_Y.meters, Me.final_nextWayPoint.pos_X, Me.final_nextWayPoint.pos_Y)
        Else

            '!!! I feel this should be outside in the tick event
            'if plane is on STAR and we passed the latest point then we enter final approach
            If Not Me.final_currentAirPathName Is Nothing Then

                Me.leaveFinalPath()
                Me.enterShortApproach()
                Me.currentState = enumPlaneState.tower_freeFlight
                RaiseEvent statusChanged(Me)

            Else
                'clear all so plane can continue in free flight
                Me.clearAirCommands(True, True)
            End If
        End If

    End Sub

    ''' <summary>
    ''' deletes the current waypoint and set up a new waypoint if there is one
    ''' </summary>
    Friend Sub proceedOnFlightPath()
        Me.air_flightPath.RemoveAt(0)

        If Me.air_flightPath.Count > 0 Then
            Me.air_nextWayPoint = Me.air_flightPath.First.nextWayPoint
            Me.air_currentAirWay = Me.air_flightPath.First.taxiwayToWayPoint

            'only update speed if a default speed is set
            If Not Me.air_currentAirWay.maxSpeed Is Nothing Then Me.target_speed.knots = Me.air_currentAirWay.maxSpeed.knots

            'only update altitude if there is no override by ATC and a default altitude is set
            If Not Me.air_altitudeOverrideByATC AndAlso Not Me.air_nextWayPoint.altitude Is Nothing Then
                Me.target_altitude.feet = Me.air_nextWayPoint.altitude.feet
                'in case height is negative, change to cruisespeed
                If Me.target_altitude.feet < 0 Then Me.target_altitude.feet = Me.modelInfo.air_AltCruise.feet
            End If
            Me.target_direction = clsNavigationPath.directionbetweenpoints(Me.pos_X.meters, Me.pos_Y.meters, Me.air_nextWayPoint.pos_X, Me.air_nextWayPoint.pos_Y)
        Else

            '!!! I feel this should be outside in the tick event
            'if plane is on STAR and we passed the latest point then we enter final approach
            If Not Me.air_currentAirPathName Is Nothing Then

                Me.enterFinalPath()
                Me.leaveAirPath()
                Me.currentState = enumPlaneState.tower_FinalApproach
                RaiseEvent statusChanged(Me)

            Else
                'clear all so plane can continue in free flight
                Me.clearAirCommands(True, True)
            End If
        End If

    End Sub

    ''' <summary>
    '''  deletes the current takeoffwaypoint and sets the next in the list while updating the landingpath
    ''' </summary>
    Friend Sub proceedOnTakeOffPath()
        If Not Me.tower_takeOffPath Is Nothing Then
            Me.tower_takeOffPath.RemoveAt(0)

            Me.tower_nextTakeOffWayPoint = Me.tower_takeOffPath(0).nextWayPoint
            Me.tower_currentTakeOffWay = Me.tower_takeOffPath(0).taxiwayToWayPoint

            'if defined speed is given, use thie one (e.g. during line up)
            'else, go Vfto speed (takeoff speed)
            If Me.tower_currentTakeOffWay.maxSpeed.knots >= 0 Then
                Me.target_speed.knots = Me.tower_currentTakeOffWay.maxSpeed.knots
            Else
                Me.target_speed.knots = Me.modelInfo.air_V2_CLIMB5000.knots
            End If
            Me.target_altitude.feet = Me.tower_nextTakeOffWayPoint.altitude.feet
            Me.target_direction = clsNavigationPath.directionbetweenpoints(Me.pos_X.meters, Me.pos_Y.meters, Me.tower_nextTakeOffWayPoint.pos_X, Me.tower_nextTakeOffWayPoint.pos_Y)
        End If
    End Sub

    ''' <summary>
    '''  deletes the current landingwaypoint and sets the next in the list while updating the landingpath
    ''' </summary>
    Friend Sub proceedOnLandWay()
        Me.tower_touchDownPath.RemoveAt(0)

        'check if there is a way left to go
        If tower_touchDownPath.Count > 0 Then
            'yes, so proceed
            'next waypoint is the connected landway
            Me.ground_nextWayPoint = Me.tower_touchDownPath.First.nextWayPoint
            Me.ground_currentTaxiWay = Me.tower_touchDownPath.First.taxiwayToWayPoint

            Me.tower_nextTouchDownPoint = Me.tower_touchDownPath.First.nextWayPoint
            Me.tower_currentTouchDownWay = Me.tower_touchDownPath.First.taxiwayToWayPoint

            'correct taxiway
            Me.taxiTo(Me.ground_goalWayPoint, 95)

            If Me.ground_taxiPath Is Nothing Then
                '!!!            MsgBox("no path found")
            Else
                Me.target_speed.knots = Me.tower_currentTouchDownWay.maxSpeed.knots
                Me.target_altitude.feet = Me.ground_nextWayPoint.altitude.feet
                Me.target_direction = clsNavigationPath.directionbetweenpoints(Me.pos_X.meters, Me.pos_Y.meters, Me.tower_nextTouchDownPoint.pos_X, Me.tower_nextTouchDownPoint.pos_Y)

            End If
        Else
            'end of runway reached, therefore crash
            Me.crash()
        End If

    End Sub

    ''' <summary>
    ''' moves the plane to a location 
    ''' </summary>
    ''' <param name="goal">location as navigation point</param>
    ''' <param name="toBack">defines if the back of the plane shall be moved to the point while the direction remains</param>
    Private Sub warpTo(ByRef goal As clsConnectionPoint, Optional ByVal toBack As Boolean = False)

        If Not toBack Then
            Me.pos_X.meters = goal.pos_X - Me.collisionRadius.meters * Math.Sin(Me.pos_direction * Math.PI / 180)
            Me.pos_Y.meters = goal.pos_Y + Me.collisionRadius.meters * Math.Cos(Me.pos_direction * Math.PI / 180)
        Else
            Me.pos_X.meters = goal.pos_X + Me.collisionRadius.meters * Math.Sin(Me.pos_direction * Math.PI / 180)
            Me.pos_Y.meters = goal.pos_Y - Me.collisionRadius.meters * Math.Cos(Me.pos_direction * Math.PI / 180)
        End If
    End Sub

    ''' <summary>
    ''' moves the plane to a gate and directs the angle to the parking direction
    ''' </summary>
    ''' <param name="goal">gate</param>
    Friend Sub warpTo(ByRef goal As clsGate)
        Me.pos_X.meters = goal.pos_X - Me.collisionRadius.meters * Math.Sin(Me.pos_direction * Math.PI / 180)
        Me.pos_Y.meters = goal.pos_Y + Me.collisionRadius.meters * Math.Cos(Me.pos_direction * Math.PI / 180)
        Me.pos_direction = goal.parkingDirection
        'Me.pos_X.meters = goal.pos_X
        'Me.pos_Y.meters = goal.pos_Y
    End Sub

    ''' <summary>
    ''' prepare plane for takeoff
    ''' </summary>
    ''' <param name="takeOffPoint">taxiwaypoint where plane leaves ramp and enters runway</param>
    Private Sub prepareTakeOff(ByRef takeOffPoint As clsConnectionPoint)
        'find takeoffway
        Me.tower_takeOffPath = takeOffPoint.getTakeOffPath
        Me.tower_nextTakeOffWayPoint = Me.tower_takeOffPath.First.nextWayPoint
        Me.tower_currentTakeOffWay = Me.tower_takeOffPath.First.taxiwayToWayPoint
        Me.tower_goalTakeOffWayPoint = Me.tower_takeOffPath.Last.nextWayPoint
        Me.target_altitude.feet = Me.tower_takeOffPath.First.nextWayPoint.altitude.feet


        '!!!why do we have this here?
        'Me.air_currentAirPathName=Nothing 

        '  Me.air_flightPath = New List(Of clsastarengine.structPathStep)

    End Sub

    ''' <summary>
    ''' cancel all take-off related permissions and data
    ''' </summary>
    Private Sub cancelTakeOff()
        Me.tower_takeOffApproved = False
        Me.tower_LineUpApproved = False
        Me.tower_takeOffPath = Nothing
        Me.target_altitude.feet = 0
        GC.Collect()
    End Sub

    ''' <summary>
    ''' clears all commands and data for ground
    ''' </summary>
    Private Sub clearGroundCommands(Optional ByVal keepTerminal As Boolean = False)
        Me.ground_currentTaxiWay = Nothing
        Me.ground_goalWayPoint = Nothing
        Me.ground_nextWayPoint = Nothing
        Me.ground_taxiPath = Nothing
        If Not keepTerminal Then Me.ground_terminal = Nothing
        GC.Collect()
    End Sub

    ''' <summary>
    ''' clears all commands and data for air
    ''' </summary>
    ''' <param name="keepEmptyFlightPath">if true, flightpath will be cleaered but kept, if false, flightpath will be set to nothing</param>
    Private Sub clearAirCommands(ByVal keepEmptyFlightPath As Boolean, ByVal keepATCAltideOverride As Boolean)
        Me.air_flightPath = Nothing
        If keepEmptyFlightPath Then Me.air_flightPath = New List(Of clsAStarEngine.structPathStep)
        Me.air_currentAirWay = Nothing
        Me.air_goalWayPoint = Nothing
        Me.air_nextWayPoint = Nothing
        Me.air_currentAirPathName = Nothing

        Me.final_currentAirWay = Nothing
        Me.final_goalWayPoint = Nothing
        Me.final_nextWayPoint = Nothing
        Me.final_currentAirPathName = Nothing


        If Not keepATCAltideOverride Then Me.air_altitudeOverrideByATC = Nothing

        GC.Collect()
    End Sub

    Private Sub clearTowerLandingCommands()
        Me.tower_goAroundPoint = Nothing
        Me.tower_assignedLandingPoint = Nothing
        Me.tower_cleardToLand = False
        Me.tower_assignedExitPointID = Nothing
        Me.tower_currentTouchDownWay = Nothing
    End Sub

    Friend Sub enterFinalPath()
        If Not Me.tower_assignedLandingPoint Is Nothing Then
            'find final that has the closest point

            Dim searchPointX As Double
            Dim searchPointY As Double

            'if plane is on an airway, take the last point of the airway as reference,
            'else take plane position
            If Not Me.air_goalWayPoint Is Nothing Then
                searchPointX = Me.air_goalWayPoint.pos_X
                searchPointY = Me.air_goalWayPoint.pos_Y
            Else
                searchPointX = Me.pos_X.meters
                searchPointY = Me.pos_Y.meters
            End If

            Dim pathPointCombination As Tuple(Of clsAirPath, clsConnectionPoint) = Me.tower_assignedLandingPoint.findClosestFinal(searchPointX, searchPointY, Me.pos_direction)
            Dim finalPath As clsAirPath = pathPointCombination.Item1
            '!!! if current NextWayPoint is part of a STAR, enter STAR at this point
            'i.e. delete all points before the current one
            Dim firstPoint As clsNavigationPoint = pathPointCombination.Item2

            Me.final_flightPath = New List(Of clsAStarEngine.structPathStep)(finalPath.path)         'new list copying the other list w/o using it as reference
            Me.final_currentAirWay = Me.final_flightPath.First.taxiwayToWayPoint
            Me.final_nextWayPoint = Me.final_flightPath.First.nextWayPoint
            Me.final_goalWayPoint = Me.final_flightPath.Last.nextWayPoint
            Me.target_altitude.feet = Me.final_nextWayPoint.altitude.feet
            Me.air_altitudeOverrideByATC = False                                                'upon entering path, the default altitudes shall count (again)

            'take max altitude in case target altitude < 0
            If Me.target_altitude.feet < 0 Then Me.target_altitude.feet = Me.modelInfo.air_AltCruise.feet

            'check new path if former waypoint was inside
            Dim waypointFound As Boolean = False
            For Each singleWayPoint As clsAStarEngine.structPathStep In Me.final_flightPath
                If singleWayPoint.nextWayPoint Is firstPoint Then
                    waypointFound = True
                    Exit For
                End If
            Next

            'if found, delete all waypoints before the one we need
            If waypointFound Then
                While Not Me.final_nextWayPoint Is firstPoint
                    'deletes the first one and sets all variables
                    Me.proceedOnFinalPath()
                End While
            End If

            'also delete the point if it is reached because first point is last point
            While (mdlHelpers.diffBetweenPoints2D(Me.pos_X.meters, Me.pos_Y.meters, Me.final_nextWayPoint.pos_X, Me.final_nextWayPoint.pos_Y) < Me.pointDetectionCircle.meters)
                Me.proceedOnFinalPath()
            End While


            Me.final_currentAirPathName = finalPath.name
            Me.target_speed.knots = Me.modelInfo.air_VRef.knots \ 1


        End If
    End Sub

    Friend Sub enterAirPath(ByRef airPath As clsAirPath)
        '!!! if current NextWayPoint is part of a STAR, enter STAR at this point
        'i.e. delete all points before the current one
        Dim tmpWayPoint As clsNavigationPoint = Me.air_nextWayPoint

        Me.air_flightPath = New List(Of clsAStarEngine.structPathStep)(airPath.path)         'new list copying the other list w/o using it as reference
        Me.air_currentAirWay = Me.air_flightPath.First.taxiwayToWayPoint
        Me.air_nextWayPoint = Me.air_flightPath.First.nextWayPoint
        Me.air_goalWayPoint = Me.air_flightPath.Last.nextWayPoint
        Me.target_altitude.feet = Me.air_nextWayPoint.altitude.feet
        Me.air_altitudeOverrideByATC = False                                                'upon entering path, the default altitudes shall count (again)

        'take max altitude in case target altitude < 0
        If Me.target_altitude.feet < 0 Then Me.target_altitude.feet = Me.modelInfo.air_AltCruise.feet

        'check new path if former waypoint was inside
        Dim waypointFound As Boolean = False
        For Each singleWayPoint As clsAStarEngine.structPathStep In Me.air_flightPath
            If singleWayPoint.nextWayPoint Is tmpWayPoint Then
                waypointFound = True
                Exit For
            End If
        Next

        'if found, delete all waypoints before the one we need
        If waypointFound Then
            While Not Me.air_nextWayPoint Is tmpWayPoint
                'deletes the first one and sets all variables
                Me.proceedOnFlightPath()
            End While
        End If

        Me.air_currentAirPathName = airPath.name

    End Sub

    Friend Sub leaveAirPath()
        If Not Me.air_flightPath Is Nothing Then
            Me.air_flightPath.Clear()
        End If
        Me.air_nextWayPoint = Nothing
        Me.air_currentAirWay = Nothing
        Me.air_goalWayPoint = Nothing
        Me.air_currentAirPathName = Nothing
        GC.Collect()
    End Sub

    Friend Sub leaveFinalPath()
        If Not Me.final_flightPath Is Nothing Then
            Me.final_flightPath.Clear()
        End If
        Me.final_nextWayPoint = Nothing
        Me.final_currentAirWay = Nothing
        Me.final_goalWayPoint = Nothing
        Me.final_currentAirPathName = Nothing
        GC.Collect()
    End Sub

    ''' <summary>
    ''' enters final meters to runway if a runway is assigned and leaves all air- and finalpaths
    ''' </summary>
    Friend Sub enterShortApproach()
        If Not Me.tower_assignedLandingPoint Is Nothing Then
            Me.air_altitudeOverrideByATC = False                        'when entering final, ATC height is cancelled in favor of default
            Me.air_nextWayPoint = Me.tower_assignedLandingPoint
            Me.air_currentAirWay = Nothing
            Me.final_currentAirWay = Nothing
            Me.target_speed.knots = Me.modelInfo.air_VRef.knots \ 1
            Me.target_altitude.feet = Me.air_nextWayPoint.altitude.feet
            Me.target_direction = clsNavigationPath.directionbetweenpoints(Me.pos_X.meters, Me.pos_Y.meters, Me.air_nextWayPoint.pos_X, Me.air_nextWayPoint.pos_Y)


        End If
    End Sub

    Friend Sub adjustGroundAngle(ByVal timespan As TimeSpan)
        'convert angle distance to a value between -180 and 180
        Dim deltaAngle As Double = mdlHelpers.diffBetweenAngles(Me.pos_direction, Me.target_direction)

        'check if angle points at goal
        If deltaAngle = 0 Then
            'do nothing
        ElseIf deltaAngle < 0 Then
            'this means the plane has to move to the left
            'increase angle and if crossed, trim angle to target angle
            Me.pos_direction -= Me.modelInfo.ground_AngleSpeed * timespan.TotalSeconds
            deltaAngle = mdlHelpers.diffBetweenAngles(Me.pos_direction, Me.target_direction)
            If deltaAngle > 0 Then Me.pos_direction = Me.target_direction
        ElseIf deltaAngle > 0 Then
            'this means the plane has to move to the right
            'decrease angle and if crossed, trim angle to targetangle
            Me.pos_direction += Me.modelInfo.ground_AngleSpeed * timespan.TotalSeconds
            deltaAngle = mdlHelpers.diffBetweenAngles(Me.pos_direction, Me.target_direction)
            If deltaAngle < 0 Then Me.pos_direction = Me.target_direction

        End If
        If Me.pos_direction = 0 Then Me.pos_direction = 360

    End Sub



    Friend Sub adjustAirAngle(ByVal timespan As TimeSpan)
        'convert angle distance to a value between -180 and 180
        Dim deltaAngle As Double = mdlHelpers.diffBetweenAngles(Me.pos_direction, Me.target_direction)

        'check if angle points at goal
        If deltaAngle = 0 Then
            'do nothing
        ElseIf deltaAngle < 0 Then
            'this means the plane has to move to the left
            'increase angle and if crossed, trim angle to target angle
            Me.pos_direction -= Me.modelInfo.air_AngleSpeed * timespan.TotalSeconds
            deltaAngle = mdlHelpers.diffBetweenAngles(Me.pos_direction, Me.target_direction)
            If deltaAngle > 0 Then Me.pos_direction = Me.target_direction
        ElseIf deltaAngle > 0 Then
            'this means the plane has to move to the right
            'decrease angle and if crossed, trim angle to targetangle
            Me.pos_direction += Me.modelInfo.air_AngleSpeed * timespan.TotalSeconds
            deltaAngle = mdlHelpers.diffBetweenAngles(Me.pos_direction, Me.target_direction)
            If deltaAngle < 0 Then Me.pos_direction = Me.target_direction

        End If
    End Sub

    Private Sub eventPathFinder_cardFound(ByRef card As clsAStarCard) Handles eventPathFinder.cardFound
        RaiseEvent cardFound(card)
    End Sub
End Class
