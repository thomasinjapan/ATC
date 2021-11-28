Option Explicit On
Imports System.IO
Imports System.Net.Sockets
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Xml.Serialization

<Serializable>
Public Class clsGame
    Friend Const PLANE_HISTORY As Long = 20

    'networking info
    Friend ReadOnly isServer As Boolean
    Friend ReadOnly isclient As Boolean
    Friend serverConnected As Boolean
    Friend TCPListener As TcpListener           'listening for info from client(s)
    Friend TCPServerClientPlayers As List(Of TcpClient)
    Friend TCPClient As TcpClient               'client
    Friend TCPClientStream As NetworkStream     'stream where client is constantly listening
    Friend WithEvents tmrServerListen As Timer          'timer that listens To information from client
    Friend WithEvents tmrServerSendKeyFrame As Timer            'timer that sends information from server
    Friend WithEvents tmrClientListen As Timer          'timer that listens to information from server
    ' Friend WithEvents tmrClientSend As Timer            'timer that sends information to server

    Friend Event EventCardFound(ByRef card As clsAStarCard)
    Friend Sub cardFound(ByRef card As clsAStarCard)
        RaiseEvent EventCardFound(card)
    End Sub

    'universe
    Friend WithEvents Universe As Timer
    Friend oldTimeTicks As Long = -1
    Friend isPaused As Boolean = True
    Friend WithEvents timerSpawn As Timer
    Friend Property minSpawnDelay As Long
    Friend Property maxSpawnDelay As Long
    Friend WithEvents timerEndGate As Timer
    Friend Property minEndgateDelay As Long
    Friend Property maxEndgateDelay As Long
    Friend WithEvents timerWindChange As Timer      'timer that changes wind
    Friend WithEvents timerHistory As Timer         'timer that records history of all planes
    Friend Property minWindChangeDelay As Long
    Friend Property maxWindChangeDelay As Long
    Friend Property minWindChangeAngle As Double
    Friend Property maxWindChangeAngle As Double

    'info about airport
    Friend AirPort As clsAirport
    Friend WithEvents Planes As New List(Of clsPlane)

    'game info
    Friend WithEvents selectedPlane As clsPlane

    Friend Property reservedGates As New List(Of clsGate)                   'gates that are reserved and should not be used when spawning or giving new targets
    Friend Property allowSpawnUntil As DateTime
    Friend Property allowEndGateUntil As DateTime
    Friend Property playApproachDeparture As Boolean = True

    'plane infos
    '!!! should be part of airport
    Friend Property planeTypes As List(Of clsPlane.structPlaneTypeInfo)


    'meta
    Friend Property maxPlanes As Long                                   'maximal number of planes
    Friend crashedPlanes As Long
    Friend successfulLandings As Long
    Friend successfulGated As Long                                      'number of planes that arrived at any gate
    Friend successfulArrival As Long                                    'number of planes that arrived at intended gate
    Friend successfulTakeOffs As Long
    Friend successfulDeparted As Long

    'events
    Friend Event selectedPlaneStatusChanged(ByRef plane As clsPlane)    'supposed to be raused if a plane changed the status
    Friend Event planeFrequencyChanged(ByRef plane As clsPlane)         'supposed to be raised if a plane changes frequency
    Friend Event spawnedPlane(ByRef plane As clsPlane)                  'supposed to be raised if a plane spawns
    Friend Event despawnedPlane(ByRef plane As clsPlane)                'supposed to be raused if a plane despawns
    Friend Event availableRunwaysArrivalChanged()                              'supposed to be raised if the opened runway has changed remotely from the server
    Friend Event availableRunwaysDepartureChanged()                              'supposed to be raised if the opened runway has changed remotely from the server
    Friend Event usedRunwaysChanged()                              'supposed to be raised if the runway availability has changed
    Friend Event ticked(ByVal milliseconds As Long)
    Friend Event radioMessage(ByRef frequency As clsPlane.enumFrequency, ByRef message As String)


    'load instance as server
    Public Sub New(ByVal airportFilePath As String, Optional ByVal maxGroundPlanes As Long = Long.MaxValue, Optional ByVal maxPlanes As Long = Long.MaxValue)
        Me.maxPlanes = maxPlanes

        'prepare metainfo
        Me.isServer = True
        Me.isclient = False
        Me.serverConnected = False
        Me.TCPServerClientPlayers = New List(Of TcpClient)

        'prepare airport
        Me.prepareAirPort(airportFilePath)

        'prepare planetypes
        '!!! should be part of airport
        Me.preparePlaneTypes()

        '!!! prepare initial state
        '!!!prepare planes
        Me.preparePlanesAtGates(maxGroundPlanes)

        'create universeTicker
        Me.Universe = New Timer With {.Enabled = False, .Interval = 10}
        Me.oldTimeTicks = Nothing

        'prepare timerSpawner
        Me.timerSpawn = New Timer With {.Enabled = False, .Interval = 120000}

        'prepare timerEndGate
        Me.timerEndGate = New Timer With {.Enabled = False, .Interval = 120000}

        'prepare timerWindChange
        Me.timerWindChange = New Timer With {.Enabled = False, .Interval = 120000}

        'prepare timer for recording of plane histories
        Me.timerHistory = New Timer With {.Enabled = False, .Interval = 1000}

        'meta
        Me.crashedPlanes = 0
        Me.successfulLandings = 0
        Me.successfulTakeOffs = 0
        Me.successfulDeparted = 0
        Me.successfulGated = 0
        Me.successfulArrival = 0
    End Sub

    ''' <summary>
    ''' load game as client
    ''' </summary>
    Public Sub New()
        Me.isclient = True
        Me.isServer = False
        Me.serverConnected = False
        Me.TCPServerClientPlayers = New List(Of TcpClient)

        'prepare timer for recording of plane histories
        Me.timerHistory = New Timer With {.Enabled = False, .Interval = 1000}

        'meta
        Me.crashedPlanes = 0
        Me.successfulLandings = 0
        Me.successfulTakeOffs = 0

    End Sub

    ''' <summary>
    ''' listents if plane status of selected plane has changed and raises event as game
    ''' </summary>
    Friend Sub ListenSelectedPlaneStatus(ByRef plane As clsPlane) Handles selectedPlane.statusChanged
        RaiseEvent selectedPlaneStatusChanged(plane)
    End Sub

    ''' <summary>
    ''' listens if plane frequency of selected plane has changed and raises event as game
    ''' </summary>
    ''' <param name="plane"></param>
    Friend Sub listenSelectedPlaneFrequency(ByRef plane As clsPlane) Handles selectedPlane.frequencyChanged
        RaiseEvent planeFrequencyChanged(plane)
    End Sub

    Friend Sub prepareAirPort(ByVal filename As String)
        Dim xDoc As XDocument = XDocument.Load(filename)
        Dim xAirport As XElement = xDoc.Root

        Dim airport As New clsAirport(xAirport)

        Me.AirPort = airport
    End Sub

    ''' <summary>
    ''' defines all plane types that we use in this game
    ''' !!!should be based on airport and part of airport data?
    ''' </summary>
    Friend Sub preparePlaneTypes()
        'define all plane types we offer
        'check this page for refecence: https://www.skybrary.aero/index.php/B762 where approach is stall speed

        Dim planeTypeInfoCollection As New List(Of clsPlane.structPlaneTypeInfo)
        planeTypeInfoCollection.Add(New clsPlane.structPlaneTypeInfo With {
            .maker = "Boeing",
            .modelShort = "B787-8",
            .model = "787-8",
            .length = New clsDistanceCollection(56.72, clsDistanceCollection.enumDistanceUnits.meters),
            .wingSpan = New clsDistanceCollection(60.12, clsDistanceCollection.enumDistanceUnits.meters),
            .air_AltMax = New clsDistanceCollection(43000, clsDistanceCollection.enumDistanceUnits.feet),
            .air_AltCruise = New clsDistanceCollection(35000, clsDistanceCollection.enumDistanceUnits.feet),
            .air_Vcruise = New clsSpeedCollection(488),
            .air_Vstall = New clsSpeedCollection(140),
            .air_Vmo = New clsSpeedCollection(515),
            .air_Vfto_V2 = New clsSpeedCollection(165),
            .air_V2_CLIMBTO5000 = New clsSpeedCollection(190),
            .air_V2_CLIMBTOFL150 = New clsSpeedCollection(290),
            .air_V2_CLIMBTOFL240 = New clsSpeedCollection(290),
            .air_V2_DESCENTABOVELTFL100 = New clsSpeedCollection(300),
            .air_V2_DESCENTBELOWFL100 = New clsSpeedCollection(220),
            .ground_AccelerationRate = New clsSpeedCollection(3.88769),
            .ground_DescelerationRate = New clsSpeedCollection(5.83153),
            .ground_AngleSpeed = 20,
            .air_AccelerationRate = New clsSpeedCollection(30),
            .air_DescelerationRate = New clsSpeedCollection(30),
            .air_AngleSpeed = 3,
            .air_ClimbSpeed = New clsSpeedCollection(30),
            .air_DescentSpeed = New clsSpeedCollection(20)
        })

        planeTypeInfoCollection.Add(New clsPlane.structPlaneTypeInfo With {
            .maker = "Cessna",
            .modelShort = "C172R",
            .model = "Cessna 172R",
            .length = New clsDistanceCollection(8.82, clsDistanceCollection.enumDistanceUnits.meters),
            .wingSpan = New clsDistanceCollection(11, clsDistanceCollection.enumDistanceUnits.meters),
            .air_AltMax = New clsDistanceCollection(13500, clsDistanceCollection.enumDistanceUnits.feet),
            .air_AltCruise = New clsDistanceCollection(13500, clsDistanceCollection.enumDistanceUnits.feet),
            .air_Vcruise = New clsSpeedCollection(122),
            .air_Vstall = New clsSpeedCollection(47),
            .air_Vmo = New clsSpeedCollection(163),                        'ktas according to cessna.txtav.com
            .air_Vfto_V2 = New clsSpeedCollection(47),
            .air_V2_CLIMBTO5000 = New clsSpeedCollection(60),                      '!!!estimated
            .air_V2_CLIMBTOFL150 = New clsSpeedCollection(122),                    '!!!estimated
                    .air_V2_CLIMBTOFL240 = New clsSpeedCollection(122),         '!!!estimated
            .air_V2_DESCENTABOVELTFL100 = New clsSpeedCollection(122),              '!!!estimated
            .air_V2_DESCENTBELOWFL100 = New clsSpeedCollection(122),            '!!!estimated
            .ground_AccelerationRate = New clsSpeedCollection(3.88769),
            .ground_DescelerationRate = New clsSpeedCollection(5.83153),
            .ground_AngleSpeed = 20,
            .air_AccelerationRate = New clsSpeedCollection(30),
            .air_DescelerationRate = New clsSpeedCollection(30),
            .air_AngleSpeed = 3,
            .air_ClimbSpeed = New clsSpeedCollection(7),
            .air_DescentSpeed = New clsSpeedCollection(7)
        })

        planeTypeInfoCollection.Add(New clsPlane.structPlaneTypeInfo With {
            .maker = "Airbus",
            .modelShort = "A380-800",
            .model = "A380-800",
            .length = New clsDistanceCollection(72.72, clsDistanceCollection.enumDistanceUnits.meters),
            .wingSpan = New clsDistanceCollection(79.75, clsDistanceCollection.enumDistanceUnits.meters),
            .air_AltMax = New clsDistanceCollection(43000, clsDistanceCollection.enumDistanceUnits.feet),
            .air_AltCruise = New clsDistanceCollection(35000, clsDistanceCollection.enumDistanceUnits.feet),
            .air_Vfto_V2 = New clsSpeedCollection(170),                     '!!! source missing
            .air_V2_CLIMBTO5000 = New clsSpeedCollection(190),
            .air_V2_CLIMBTOFL150 = New clsSpeedCollection(240),
            .air_V2_CLIMBTOFL240 = New clsSpeedCollection(240), '!!! source missing
            .air_V2_DESCENTABOVELTFL100 = New clsSpeedCollection(300),
            .air_V2_DESCENTBELOWFL100 = New clsSpeedCollection(250),
            .air_Vmo = New clsSpeedCollection(593),
            .air_Vcruise = New clsSpeedCollection(488),
            .air_Vstall = New clsSpeedCollection(192),
            .ground_AccelerationRate = New clsSpeedCollection(3.88769),
            .ground_DescelerationRate = New clsSpeedCollection(5.83153),
            .ground_AngleSpeed = 20,
            .air_AccelerationRate = New clsSpeedCollection(30),
            .air_DescelerationRate = New clsSpeedCollection(30),
            .air_AngleSpeed = 3,
            .air_ClimbSpeed = New clsSpeedCollection(30),
            .air_DescentSpeed = New clsSpeedCollection(20)
        })

        planeTypeInfoCollection.Add(New clsPlane.structPlaneTypeInfo With {
            .maker = "Boeing",
            .modelShort = "B767-200",
            .model = "B767-200",
            .length = New clsDistanceCollection(48.51, clsDistanceCollection.enumDistanceUnits.meters),
            .wingSpan = New clsDistanceCollection(47.57, clsDistanceCollection.enumDistanceUnits.meters),
            .air_AltMax = New clsDistanceCollection(43100, clsDistanceCollection.enumDistanceUnits.feet),
            .air_AltCruise = New clsDistanceCollection(39000, clsDistanceCollection.enumDistanceUnits.feet),
            .air_Vcruise = New clsSpeedCollection(473),
            .air_Vstall = New clsSpeedCollection(134),
            .air_Vmo = New clsSpeedCollection(484),
            .air_Vfto_V2 = New clsSpeedCollection(160),
            .air_V2_CLIMBTO5000 = New clsSpeedCollection(190),
            .air_V2_CLIMBTOFL150 = New clsSpeedCollection(290),
            .air_V2_CLIMBTOFL240 = New clsSpeedCollection(290),         '!!! source missing
            .air_V2_DESCENTABOVELTFL100 = New clsSpeedCollection(290),
            .air_V2_DESCENTBELOWFL100 = New clsSpeedCollection(220),
            .ground_AccelerationRate = New clsSpeedCollection(3.88769),
            .ground_DescelerationRate = New clsSpeedCollection(5.83153),
            .ground_AngleSpeed = 20,
            .air_AccelerationRate = New clsSpeedCollection(30),
            .air_DescelerationRate = New clsSpeedCollection(30),
            .air_AngleSpeed = 3,
            .air_ClimbSpeed = New clsSpeedCollection(30),
            .air_DescentSpeed = New clsSpeedCollection(20)
             })

        planeTypeInfoCollection.Add(New clsPlane.structPlaneTypeInfo With {
            .maker = "Boeing",
            .modelShort = "B777-200",
            .model = "B777-200",
            .length = New clsDistanceCollection(63.7, clsDistanceCollection.enumDistanceUnits.meters),
            .wingSpan = New clsDistanceCollection(60.9, clsDistanceCollection.enumDistanceUnits.meters),
            .air_AltMax = New clsDistanceCollection(43100, clsDistanceCollection.enumDistanceUnits.feet),
            .air_AltCruise = New clsDistanceCollection(35000, clsDistanceCollection.enumDistanceUnits.feet),
            .air_Vcruise = New clsSpeedCollection(490),
            .air_Vstall = New clsSpeedCollection(136),
            .air_Vfto_V2 = New clsSpeedCollection(170),
            .air_V2_CLIMBTO5000 = New clsSpeedCollection(200),
            .air_V2_CLIMBTOFL150 = New clsSpeedCollection(300),
            .air_V2_CLIMBTOFL240 = New clsSpeedCollection(300),
            .air_V2_DESCENTABOVELTFL100 = New clsSpeedCollection(300),
            .air_V2_DESCENTBELOWFL100 = New clsSpeedCollection(250),
            .air_Vmo = New clsSpeedCollection(512),
            .ground_AccelerationRate = New clsSpeedCollection(3.88769),
            .ground_DescelerationRate = New clsSpeedCollection(5.83153),
            .ground_AngleSpeed = 20,
            .air_AccelerationRate = New clsSpeedCollection(30),
            .air_DescelerationRate = New clsSpeedCollection(30),
            .air_AngleSpeed = 3,
            .air_ClimbSpeed = New clsSpeedCollection(30),
            .air_DescentSpeed = New clsSpeedCollection(20)
        })

        planeTypeInfoCollection.Add(New clsPlane.structPlaneTypeInfo With {
            .maker = "Bombardier",
            .modelShort = "Global 5000",
            .model = "B Global 5000",
            .length = New clsDistanceCollection(29.5, clsDistanceCollection.enumDistanceUnits.meters),
            .wingSpan = New clsDistanceCollection(28.7, clsDistanceCollection.enumDistanceUnits.meters),
            .air_AltMax = New clsDistanceCollection(51000, clsDistanceCollection.enumDistanceUnits.feet),
            .air_AltCruise = New clsDistanceCollection(41000, clsDistanceCollection.enumDistanceUnits.feet),
            .air_Vcruise = New clsSpeedCollection(487),
            .air_Vstall = New clsSpeedCollection(154),                          'not confirmed
            .air_Vfto_V2 = New clsSpeedCollection(154),                            'not confirmed
            .air_V2_CLIMBTO5000 = New clsSpeedCollection(190),                          'not confirmed
            .air_V2_CLIMBTOFL150 = New clsSpeedCollection(290),                        'not confirmed
            .air_V2_CLIMBTOFL240 = New clsSpeedCollection(290),                         'not confirmed
            .air_V2_DESCENTABOVELTFL100 = New clsSpeedCollection(300),              'not confirmed
            .air_V2_DESCENTBELOWFL100 = New clsSpeedCollection(220),                'not confirmed
            .air_Vmo = New clsSpeedCollection(504),                             'max high speed cruise; may be higher
            .ground_AccelerationRate = New clsSpeedCollection(3.88769),
            .ground_DescelerationRate = New clsSpeedCollection(5.83153),
            .ground_AngleSpeed = 20,
            .air_AccelerationRate = New clsSpeedCollection(30),
            .air_DescelerationRate = New clsSpeedCollection(30),
            .air_AngleSpeed = 3,
            .air_ClimbSpeed = New clsSpeedCollection(30),
            .air_DescentSpeed = New clsSpeedCollection(20)
        })

        planeTypeInfoCollection.Add(New clsPlane.structPlaneTypeInfo With {
            .maker = "Bombardier",                                      '!!! numbers to be corrected
            .modelShort = "Global 7500",
            .model = "B Global 7500",
            .length = New clsDistanceCollection(33.88, clsDistanceCollection.enumDistanceUnits.meters),
            .wingSpan = New clsDistanceCollection(31.7, clsDistanceCollection.enumDistanceUnits.meters),
            .air_AltMax = New clsDistanceCollection(15545, clsDistanceCollection.enumDistanceUnits.feet),
            .air_AltCruise = New clsDistanceCollection(43000, clsDistanceCollection.enumDistanceUnits.feet),
            .air_Vcruise = New clsSpeedCollection(487),
            .air_Vstall = New clsSpeedCollection(109),                          'not confirmed
            .air_Vfto_V2 = New clsSpeedCollection(154),                            'not confirmed
            .air_V2_CLIMBTO5000 = New clsSpeedCollection(190),                          'not confirmed
            .air_V2_CLIMBTOFL150 = New clsSpeedCollection(290),                        'not confirmed
            .air_V2_CLIMBTOFL240 = New clsSpeedCollection(290),             'not confirmed
            .air_V2_DESCENTABOVELTFL100 = New clsSpeedCollection(300),              'not confirmed
            .air_V2_DESCENTBELOWFL100 = New clsSpeedCollection(220),                'not confirmed
            .air_Vmo = New clsSpeedCollection(504),                             'max high speed cruise; may be higher
            .ground_AccelerationRate = New clsSpeedCollection(3.88769),
            .ground_DescelerationRate = New clsSpeedCollection(5.83153),
            .ground_AngleSpeed = 20,
            .air_AccelerationRate = New clsSpeedCollection(30),
            .air_DescelerationRate = New clsSpeedCollection(30),
            .air_AngleSpeed = 3,
            .air_ClimbSpeed = New clsSpeedCollection(30),
            .air_DescentSpeed = New clsSpeedCollection(20)
        })

        planeTypeInfoCollection.Add(New clsPlane.structPlaneTypeInfo With {
            .maker = "Airbus",
            .modelShort = "A320",
            .model = "Airbus 320",
            .length = New clsDistanceCollection(37.57, clsDistanceCollection.enumDistanceUnits.meters),
            .wingSpan = New clsDistanceCollection(35.8, clsDistanceCollection.enumDistanceUnits.meters),
            .air_AltMax = New clsDistanceCollection(41000, clsDistanceCollection.enumDistanceUnits.feet),
            .air_AltCruise = New clsDistanceCollection(39100, clsDistanceCollection.enumDistanceUnits.feet),
            .air_Vcruise = New clsSpeedCollection(447),
            .air_Vstall = New clsSpeedCollection(137),                          'not confirmed
            .air_Vfto_V2 = New clsSpeedCollection(145),                           'V2 IAS
            .air_V2_CLIMBTO5000 = New clsSpeedCollection(175),                             'V2 <5000  FT
            .air_V2_CLIMBTOFL150 = New clsSpeedCollection(290),                        'V2 < FL150
            .air_V2_CLIMBTOFL240 = New clsSpeedCollection(290),
            .air_V2_DESCENTABOVELTFL100 = New clsSpeedCollection(290),
            .air_V2_DESCENTBELOWFL100 = New clsSpeedCollection(210),
            .air_Vmo = New clsSpeedCollection(470),                             'max high speed cruise; may be higher
            .ground_AccelerationRate = New clsSpeedCollection(3.88769),
            .ground_DescelerationRate = New clsSpeedCollection(5.83153),
            .ground_AngleSpeed = 20,
            .air_AccelerationRate = New clsSpeedCollection(30),
            .air_DescelerationRate = New clsSpeedCollection(30),
            .air_AngleSpeed = 3,
            .air_ClimbSpeed = New clsSpeedCollection(30),
            .air_DescentSpeed = New clsSpeedCollection(20)
        })

        Me.planeTypes = planeTypeInfoCollection
    End Sub

    ''' <summary>
    ''' returns the biggest planetype that fits into a gate out of all planetypes in the game
    ''' </summary>
    ''' <param name="gate">gate that the plane shall fit in</param>
    ''' <returns>largets planetype that we can use</returns>
    Friend Function getLargestPlaneTypeForGate(ByRef gate As clsGate) As clsPlane.structPlaneTypeInfo
        Dim result As New clsPlane.structPlaneTypeInfo

        'find largest plant that fits into gate
        For Each singlePlaneType As clsPlane.structPlaneTypeInfo In Me.planeTypes
            If singlePlaneType.wingSpan.meters <= gate.maxWidth.meters Then
                If result.wingSpan Is Nothing Then
                    'if no plane found yet, add this onw
                    result = singlePlaneType
                Else
                    'at least plane is already found
                    If singlePlaneType.wingSpan.meters > result.wingSpan.meters Then
                        'new plane is bigger but still fits, take this one
                        result = singlePlaneType
                    End If
                End If
            End If
        Next

        Return result
    End Function

    ''' <summary>
    ''' returns a random planetypte that fits into a gate out of all planetypoes in the game
    ''' </summary>
    ''' <param name="gate">gate that the plane shall fit in</param>
    ''' <returns>random planetype that can be used at the gate</returns>
    Friend Function getPlaneTypeThatFitsForGate(ByRef gate As clsGate) As clsPlane.structPlaneTypeInfo
        Dim result As clsPlane.structPlaneTypeInfo

        Dim candidateModels As New List(Of clsPlane.structPlaneTypeInfo)

        'get all possible types that fit
        For Each singlePlaneType As clsPlane.structPlaneTypeInfo In Me.planeTypes
            If singlePlaneType.wingSpan.meters <= gate.maxWidth.meters And
                singlePlaneType.wingSpan.meters >= gate.minWidth.meters Then
                candidateModels.Add(singlePlaneType)
            End If
        Next

        'randomly chose a type
        Dim randomizer As New Random(DateTime.Now.Millisecond)

        'chose random gate
        result = candidateModels(randomizer.Next(0, candidateModels.Count))

        Return result
    End Function

    ''' <summary>
    ''' spawns new plane and adds relevant event handlers
    ''' </summary>
    ''' <param name="plane">plane to spawn</param>
    Private Sub spawn(ByRef plane As clsPlane)
        'add event handler to release gate reservation
        AddHandler plane.Departed, AddressOf Me.despawn
        AddHandler plane.frequencyChanged, AddressOf Me.frequencyChanged
        AddHandler plane.crashed, AddressOf Me.addCrash
        AddHandler plane.landed, AddressOf Me.addLanding
        AddHandler plane.takenOff, AddressOf Me.addTakeOff
        AddHandler plane.gated, AddressOf Me.addGated
        AddHandler plane.arrived, AddressOf Me.addArrived
        AddHandler plane.radioMessage, AddressOf Me.MessageSent


        AddHandler plane.cardFound, AddressOf Me.cardFound

        Me.Planes.Add(plane)
    End Sub

    ''' <summary>
    ''' releases reservation of gate so it can be used for new purposes
    ''' </summary>
    ''' <param name="gate">gate that can be released</param>
    Friend Sub releaseGateReservation(ByRef gate As clsGate)
        'release reservation only if there is a reservation
        If Me.reservedGates.Contains(gate) Then
            '!!!actually deletion should only be possible if the plane triggering the event had a reservation
            Me.reservedGates.RemoveAt(Me.reservedGates.IndexOf(gate))
        End If
    End Sub

    ''' <summary>
    ''' reserves a gate so it cannot be occupied twice
    ''' </summary>
    ''' <param name="gate">gate that shall be reserved</param>
    Friend Sub reserveGate(ByRef gate As clsGate)
        Me.reservedGates.Add(gate)
    End Sub

    ''' <summary>
    ''' depspawns plane based on callsign
    ''' </summary>
    ''' <param name="callsign">callsign</param>
    Friend Sub despawn(ByVal callsign As String)
        Dim plane As clsPlane = Me.getPlaneByCallsign(callsign)
        Me.despawn(plane)
    End Sub

    ''' <summary>
    ''' removes plane from game
    ''' </summary>
    ''' <param name="plane">plane that is to be removed</param>
    Friend Sub despawn(ByRef plane As clsPlane)
        '!!!remove reservation from gate if plane holds reservation
        If TypeOf (plane.ground_nextWayPoint) Is clsGate Then Me.releaseGateReservation(plane.ground_nextWayPoint)
        If TypeOf (plane.ground_terminal) Is clsGate Then Me.releaseGateReservation(plane.ground_terminal)

        If Me.selectedPlane Is plane Then
            Me.selectedPlane = Nothing
        End If
        Me.Planes.Remove(plane)
        RaiseEvent despawnedPlane(plane)


        GC.Collect()
    End Sub

    Friend Sub frequencyChanged(ByRef Plane As clsPlane)
        RaiseEvent planeFrequencyChanged(Plane)
    End Sub

    Friend Sub MessageSent(ByRef plane As clsPlane, ByVal message As String)
        RaiseEvent radioMessage(plane.frequency, plane.callsign & ": " & message)
        mdlNetworkhandling.serverSendUpdateToClients(Me, enumNetworkMessageType.radioMessage, New structRadioMessageNetwork With {.frequency = plane.frequency, .message = plane.callsign & ": " & message})
        'Me.NetworkRadioMessageBuffer.Add(New structRadioMessageNetwork With {.frequency = plane.frequency, .message = plane.callsign & ": " & message})
    End Sub

    ''' <summary>
    ''' returns a gate that is not reserved by a plane so far
    ''' </summary>
    ''' <returns></returns>
    Friend Function getUnreservedGate() As clsGate
        Dim result As clsGate
        Dim randomizer As New Random(DateTime.Now.Millisecond)

        'chose random gate
        Dim gate As clsGate = Me.AirPort.gates(randomizer.Next(0, Me.AirPort.gates.Count))

        'as long as the selected gate is already reserved, chose antoher one
        While Me.reservedGates.Contains(gate)
            gate = Me.AirPort.gates(randomizer.Next(0, Me.AirPort.gates.Count))
        End While

        result = gate

        Return result
    End Function

    Friend Sub preparePlanesAtGates(Optional maxPlanes As Long = Long.MaxValue)

        Dim randomizer As New Random(DateTime.Now.Millisecond)
        'needs to be +1 since the max value is not included
        Dim initialgatesused As Long = randomizer.Next(0, Me.AirPort.gates.Count + 1)

        'make sure that we dont spawn more planes than maximal allowed
        If initialgatesused > Me.maxPlanes Then initialgatesused = maxPlanes

        For C1 As Long = 1 To initialgatesused
            'determine startgate and reserve it since we will use it
            Dim currentGate As clsGate = Me.getUnreservedGate
            Me.reservedGates.Add(currentGate)

            'get the position so we can spawn the plane ther
            Dim position As New clsMovingObject.structPosition With {
                .pos_X = New clsDistanceCollection(currentGate.pos_X, clsDistanceCollection.enumDistanceUnits.meters),
                .pos_Y = New clsDistanceCollection(currentGate.pos_Y, clsDistanceCollection.enumDistanceUnits.meters),
                .pos_Altitude = New clsDistanceCollection(0, clsDistanceCollection.enumDistanceUnits.feet),
                .pos_direction = currentGate.parkingDirection
                }

            'get movement information
            Dim movement As New clsMovingObject.structMovement With {
               .speed_absolute = New clsSpeedCollection(0),
               .speed_rotation = 0
            }

            'get accellerationdata for the plane
            '!!! maybe change this to part of planetype info
            Dim accelerationData As New clsMovingObject.structAcceleration With {
                .ground_accelleration_angle = 20,
                .ground_accelleration_speed = 2,
                .air_accelleration_angle = 3,
                .air_accelleration_speed = 30
            }

            'Dim planeTypeInfo As clsPlane.structPlaneTypeInfo = Me.getLargestPlaneTypeForGate(currentGate)
            Dim planeTypeInfo As clsPlane.structPlaneTypeInfo = Me.getPlaneTypeThatFitsForGate(currentGate)


            'get random callsign
            Dim callsign As String = ""

            'if designated gate has IATA code restrictions, use these,
            'else use random letters
            If currentGate.IATAs.Count > 0 Then
                'get random IATA from list
                callsign &= currentGate.IATAs(randomizer.Next(0, currentGate.IATAs.Count))
            Else
                callsign &= Chr(randomizer.Next(65, 91))
                callsign &= Chr(randomizer.Next(65, 91))
                callsign &= Chr(randomizer.Next(65, 91))
            End If

            callsign &= randomizer.Next(0, 10).ToString
            callsign &= randomizer.Next(0, 10).ToString
            callsign &= randomizer.Next(0, 10).ToString

            'prepare plane 
            Dim newPlane As clsPlane = New clsPlane(position, movement, accelerationData, planeTypeInfo, clsPlane.enumPlaneState.ground_atGate, currentGate, Nothing, callsign, clsPlane.enumFrequency.radioOff)
            'give plane a purpose
            '!!! for the moments, all planes want to leave
            newPlane.ground_taxiPath = New List(Of clsAStarEngine.structPathStep)
            newPlane.warpTo(currentGate)

            'spawn plane
            AddHandler newPlane.Departed, AddressOf Me.despawn
            AddHandler newPlane.frequencyChanged, AddressOf Me.frequencyChanged

            AddHandler newPlane.crashed, AddressOf Me.addCrash
            AddHandler newPlane.landed, AddressOf Me.addLanding
            AddHandler newPlane.takenOff, AddressOf Me.addTakeOff
            AddHandler newPlane.gated, AddressOf Me.addGated
            AddHandler newPlane.arrived, AddressOf Me.addArrived

            AddHandler newPlane.radioMessage, AddressOf Me.MessageSent

            AddHandler newPlane.cardFound, AddressOf Me.cardFound

            Me.Planes.Add(newPlane)

        Next


    End Sub

    Private Sub Universe_Tick(sender As Timer, e As EventArgs) Handles Universe.Tick
        'Console.WriteLine("universe since last tick|" & (Now.Ticks - Me.oldTimeTicks).ToString)
        Dim stampTickStart As DateTime = Now
        'Console.WriteLine("universe tick start|" & Format(stampTickStart, "HH:mm:ss ffff"))
        'briefly stop the ticker to make sure that we have no parallel streams
        sender.Enabled = False
        'me.oldtimestamp = Nothing
        Dim currentTimeStamp As DateTime = New DateTime(DateTime.Now.Ticks)
        Dim oldTimeTicks As Long = Me.oldTimeTicks
        Dim oldTimeStamp As DateTime

        If Me.oldTimeTicks = -1 Then
            oldTimeTicks = currentTimeStamp.Ticks
        End If
        oldTimeStamp = New DateTime(oldTimeTicks)

        Dim timeDifference As TimeSpan = currentTimeStamp - oldTimeStamp

        'if tme difference is larger than 500 ms, reset timedifference to 0
        If timeDifference.TotalMilliseconds > 500 Then
            timeDifference = New TimeSpan(0)
        End If

        Me.AirPort.tick(timeDifference)

        Dim stampPlaneStart As DateTime = Now
        'Console.WriteLine("planes tick start|" & Format(stampPlaneStart, "HH:mm:ss ffff"))
        For C1 As Long = 0 To Me.Planes.Count - 1
            Me.Planes(C1).tick(timeDifference)
        Next
        Dim stampPlaneEnd As DateTime = Now
        'Console.WriteLine("planes tick end|" & Format(stampPlaneEnd, "HH:mm:ss ffff"))
        'Console.WriteLine("planes tick duration|" & (stampPlaneEnd - stampPlaneStart).TotalMilliseconds & "|" & (stampPlaneEnd - stampPlaneStart).Ticks)


        '!!!-- check for planes that should be despawned
        '!!! ugly workaround since departed event does not trigger for planes starting at gate

        Dim removestack As New List(Of clsPlane)
        For Each singlePlane As clsPlane In Me.Planes
            '!!!also ugly: check if a plane is on SID and we play w/o arrival / departure
            '!!!in this case label as departed and it will be cleaned up in the nextg step
            'despawn only if plane is in freeflight and we play a mode that does not include approach and departure
            If Not Me.playApproachDeparture AndAlso
                singlePlane.currentState = clsPlane.enumPlaneState.tower_freeFlight And
               singlePlane.isDeparting And
               singlePlane.frequency = clsPlane.enumFrequency.appdep Then
                singlePlane.currentState = clsPlane.enumPlaneState.tower_Departed

            End If

            If singlePlane.currentState = clsPlane.enumPlaneState.tower_Departed Then
                removestack.Add(singlePlane)
                Me.addDeparted(singlePlane)
            End If
        Next
        For Each singlePlane As clsPlane In removestack
            Me.despawn(singlePlane)
        Next

        '-- check for collision
        For C1 As Long = 0 To Me.Planes.Count - 1
            For C2 As Long = C1 + 1 To Me.Planes.Count - 1
                'if the position plane 1 and plane 2 is less than the sum of both collision radii, we have a collision
                Dim plane1 As clsPlane = Me.Planes(C1)
                Dim plane2 As clsPlane = Me.Planes(C2)

                'first, check collision
                Dim separation = mdlHelpers.diffBetweenPoints3D(plane1.pos_X.meters, plane1.pos_Y.meters, plane1.pos_Altitude.meters, plane2.pos_X.meters, plane2.pos_Y.meters, plane2.pos_Altitude.meters)

                'if sepearation is too small and at least one plane is not crashed yet, register the crash
                If (separation < plane1.collisionRadius.meters + plane2.collisionRadius.meters) And (plane1.currentState <> clsPlane.enumPlaneState.special_crashed Or plane2.currentState <> clsPlane.enumPlaneState.special_crashed) Then
                    'boom
                    plane1.crash()
                    plane2.crash()
                    'MsgBox("These planes have crashed into each other: " & plane1.callsign & " and " & plane2.callsign)
                End If
            Next
        Next

        'check for crashes w/ ground
        For Each singlePlane As clsPlane In Me.Planes
            If singlePlane.currentState = clsPlane.enumPlaneState.tower_freeFlight And
                singlePlane.pos_Altitude.feet <= 0 Then
                singlePlane.crash()
            End If
        Next

        'check for each runway, if it is in use
        For Each singleRunway As clsRunWay In Me.AirPort.runWays
            singleRunway.updateIsInUse(Me.Planes)
        Next

        Me.oldTimeTicks = currentTimeStamp.Ticks
        're-eneable the timer
        sender.Enabled = True

        Dim stampTickEnd As DateTime = Now
        'Console.WriteLine("universe tick end|" & Format(stampTickEnd, "HH:mm:ss ffff"))
        'Console.WriteLine("universe tick duration|" & (stampTickEnd - stampTickStart).TotalMilliseconds & "|" & (stampTickEnd - stampTickStart).Ticks)
        RaiseEvent ticked((stampTickEnd - stampTickStart).TotalMilliseconds)
    End Sub

    Friend Function getPlaneByCallsign(ByVal callsign As String) As clsPlane
        Dim result As clsPlane = Nothing

        For C1 As Long = 0 To Me.Planes.Count - 1
            If Me.Planes(C1).callsign = callsign Then
                result = Me.Planes(C1)
                Exit For
            End If
        Next

        Return result
    End Function

    ''' <summary>
    ''' pauses or unpauses the game
    ''' </summary>
    Friend Sub togglePause()
        If Me.isPaused Then
            Me.isPaused = False
            Me.Universe.Enabled = True

            Me.timerSpawn.Enabled = True
            Me.timerEndGate.Enabled = True
            Me.timerWindChange.Enabled = True
            Me.timerHistory.Enabled = True

            'make the last time -1 so the game detects the pause and does not calculate events during the pause
            Me.oldTimeTicks = -1
        Else
            Me.isPaused = True
            Me.Universe.Enabled = False

            Me.timerSpawn.Enabled = False
            Me.timerEndGate.Enabled = False
            Me.timerWindChange.Enabled = False
            Me.timerHistory.Enabled = False

            'make the last time -1 so the game detects the pause and does not calculate events during the pause
            Me.oldTimeTicks = -1
        End If
    End Sub

    Friend Sub triggerEndGate() Handles timerEndGate.Tick
        'run only if we are still before end of spawn
        If Now <= Me.allowEndGateUntil Then
            Dim randomizer As New Random(DateTime.Now.Millisecond)

            'disable timer for the time being
            Me.timerEndGate.Enabled = False

            'if at least one gate has a plane, search for a random one and release it
            'end reservation

            'check all planes that are currently gated
            Dim gatedPlanes As New List(Of clsPlane)
            For Each singlePlane As clsPlane In Me.Planes
                If singlePlane.currentState = clsPlane.enumPlaneState.ground_atGate Then
                    gatedPlanes.Add(singlePlane)
                End If
            Next

            'if at least one plane is gated, chose one randomly to degate

            If gatedPlanes.Count >= 1 Then
                'chose randomly gate that is currently 
                Dim chosenPlane As clsPlane = gatedPlanes(randomizer.Next(0, gatedPlanes.Count))

                'release from gate
                Me.releaseGateReservation(chosenPlane.ground_nextWayPoint)

                'identify SID we would like to leave on
                'assign random terminal to mark plane as leaving
                Dim selectedSID As clsAirPath
                If Me.AirPort.openDepartureRunwaysAsListOfRunways.Count > 0 Then
                    'randomly chose runway
                    Dim listOfRunways As List(Of clsRunWay) = Me.AirPort.openDepartureRunwaysAsListOfRunways
                    Dim selectedRunway As clsRunWay = listOfRunways(randomizer.Next(0, listOfRunways.Count))
                    selectedSID = selectedRunway.SIDs(randomizer.Next(0, selectedRunway.SIDs.Count))
                Else
                    'take any SID
                    selectedSID = Me.AirPort.SIDs(randomizer.Next(0, Me.AirPort.SIDs.GetUpperBound(0) + 1))
                End If


                'define terminal so we can prepare despawn
                '!!! missing:
                'if we play w/ arr/dep, use end of path
                'else use beginning (problem is, that this is overwritten by taxiTo procedure in plane and plane doesnt know if the game allows arr/dep
                chosenPlane.air_terminal = selectedSID.path.Last.nextWayPoint

                'enter SID even if not started
                chosenPlane.enterAirPath(selectedSID)


                chosenPlane.commandInfo = New clsPlane.structCommandInfo With {
                    .plane = chosenPlane.callsign,
                    .command = clsPlane.enumCommands.askForPushback
                }

            End If

            'prepare next time to end a gate
            'Me.timerEndGate.Interval = 1000
            Me.timerEndGate.Interval = randomizer.Next(Me.minEndgateDelay, Me.maxEndgateDelay + 1)

            Me.timerEndGate.Enabled = True

            GC.Collect()
        End If
    End Sub

    Friend Sub triggerSpawn() Handles timerSpawn.Tick
        'do only if we are still before end gate
        If Now <= Me.allowSpawnUntil Then

            'stop ticking until finished
            Me.timerSpawn.Enabled = False


            Dim randomizer As New Random(DateTime.Now.Millisecond)

            'spawn only new plane if at least one gate is free and if not max number of planes is reached

            If (Me.AirPort.gates.Count > Me.reservedGates.Count) And (Me.Planes.Count < Me.maxPlanes) Then

                'get free gate and reserve itx 
                Dim terminal As clsGate = Me.getUnreservedGate
                Me.reserveGate(terminal)

                'decide what landingppoint to take
                'randomly select runway from opened runways
                'if no runway is open, chose a random runway

                Dim selectedRunway As clsRunWay
                If Me.AirPort.openArrivalRunwaysAsListOfRunways.Count > 0 Then
                    selectedRunway = Me.AirPort.openArrivalRunwaysAsListOfRunways(randomizer.Next(0, Me.AirPort.openArrivalRunwaysAsListOfRunways.Count))
                Else
                    'make sure that we chose from runway hat actually has a landingpoint
                    Dim runwaysWithArrivalPoint As New List(Of clsRunWay)
                    For Each singleRunway As clsRunWay In Me.AirPort.runWays
                        If singleRunway.canHandleArrivals Then runwaysWithArrivalPoint.Add(singleRunway)
                    Next

                    selectedRunway = runwaysWithArrivalPoint(randomizer.Next(0, runwaysWithArrivalPoint.Count))
                End If
                'on the runway decide what touchdownway we use
                'get random touchdownwaypoint
                Dim selectedArrivalPoint As clsTouchDownWayPoint = selectedRunway.arrivalPoint
                Dim selectedGoaroundPoint As clsNavigationPoint = selectedRunway.goAroundPoint

                'search for a star that has the correct arrivalpoint
                'count stars w/ arrivalpoint inside
                Dim foundSTARs As New List(Of clsAirPath)
                For Each singleSTAR As clsAirPath In selectedRunway.STARs
                    foundSTARs.Add(singleSTAR)
                Next
                'randomly select one
                Dim selectedSTAR As clsAirPath = foundSTARs(randomizer.Next(0, foundSTARs.Count))
                '!!!possibly we have to copy all info not to break the path once the fist plane follows the path

                'get full landingpath
                Dim selectedLandingPath As List(Of clsAStarEngine.structPathStep) = selectedArrivalPoint.getLandingPath

                'get first segment on landingpath
                Dim selectedTouchdownWay As clsTouchDownWay = selectedLandingPath(1).taxiwayToWayPoint


                'find largest plant that fits into gate
                'Dim planeTypeInfo As clsPlane.structPlaneTypeInfo = Me.getLargestPlaneTypeForGate(terminal)
                Dim planeTypeInfo As clsPlane.structPlaneTypeInfo = Me.getPlaneTypeThatFitsForGate(terminal)

                'calculate location for spawning
                Dim spawnPoint As clsNavigationPoint
                Dim arrivalAngle As Double
                Dim frequency As clsPlane.enumFrequency

                'spawn at beginning of AppDep if we play w/ it
                'else spawn at end and directly enter final approach
                If Me.playApproachDeparture Then
                    'get angle for approach
                    '!!!Dim arrivalAngle As Double = selectedTouchdownWay.approachDirection
                    arrivalAngle = selectedSTAR.path(1).taxiwayToWayPoint.directionFrom(selectedSTAR.path.First.nextWayPoint)
                    spawnPoint = selectedSTAR.path.First.nextWayPoint
                    frequency = clsPlane.enumFrequency.appdep
                Else
                    arrivalAngle = selectedSTAR.path.Last.taxiwayToWayPoint.directionTo(selectedSTAR.path.Last.nextWayPoint)
                    spawnPoint = selectedSTAR.path.Last.nextWayPoint
                    frequency = clsPlane.enumFrequency.tower
                End If


                Dim spawnLocationX As Double = spawnPoint.pos_X
                Dim spawnLocationY As Double = spawnPoint.pos_Y
                Dim spawndirection As Double = arrivalAngle

                If spawnPoint.altitude.feet < 0 Then spawnPoint.altitude.feet = planeTypeInfo.air_AltMax.feet
                'Dim spawnAltitude As New clsDistanceCollection(planeTypeInfo.maxAltitude.feet, clsDistanceCollection.enumDistanceUnits.feet)
                Dim spawnAltitude As New clsDistanceCollection(spawnPoint.altitude.feet, clsDistanceCollection.enumDistanceUnits.feet)

                Dim positionInfo As New clsMovingObject.structPosition With {
                    .pos_X = New clsDistanceCollection(spawnLocationX, clsDistanceCollection.enumDistanceUnits.meters),
                    .pos_Y = New clsDistanceCollection(spawnLocationY, clsDistanceCollection.enumDistanceUnits.meters),
                    .pos_Altitude = spawnAltitude,
                    .pos_direction = spawndirection
                }

                Dim initialSpeed As New clsSpeedCollection(0)
                If positionInfo.pos_Altitude.feet >= 10000 Then
                    initialSpeed.knots = planeTypeInfo.air_V2_DESCENTABOVELTFL100.knots
                ElseIf positionInfo.pos_Altitude.feet < 10000 Then
                    initialSpeed.knots = planeTypeInfo.air_V2_DESCENTBELOWFL100.knots
                End If

                Dim movementInfo As New clsMovingObject.structMovement With {
                    .speed_absolute = initialSpeed,
                    .speed_rotation = 0
                }

                Dim accelerationData As New clsMovingObject.structAcceleration With {
                    .ground_accelleration_angle = 20,
                    .ground_accelleration_speed = 2,
                    .air_accelleration_angle = 3,
                    .air_accelleration_speed = 30
                }

                'prepare callsign
                Dim callsign As String = ""

                'if designated gate has IATA code restrictions, use these,
                'else use random letters
                If terminal.IATAs.Count > 0 Then
                    'get random IATA from list
                    callsign &= terminal.IATAs(randomizer.Next(0, terminal.IATAs.Count))
                Else
                    callsign &= Chr(randomizer.Next(65, 91))
                    callsign &= Chr(randomizer.Next(65, 91))
                    callsign &= Chr(randomizer.Next(65, 91))
                End If


                callsign &= randomizer.Next(0, 10).ToString
                callsign &= randomizer.Next(0, 10).ToString
                callsign &= randomizer.Next(0, 10).ToString

                Dim plane As clsPlane = New clsPlane(positionInfo, movementInfo, accelerationData, planeTypeInfo, clsPlane.enumPlaneState.tower_freeFlight, spawnPoint, terminal, callsign, frequency)
                plane.air_nextWayPoint = spawnPoint
                plane.tower_assignedLandingPoint = selectedArrivalPoint
                plane.tower_goAroundPoint = selectedGoaroundPoint
                plane.enterAirPath(selectedSTAR)



                Me.spawn(plane)
                RaiseEvent spawnedPlane(plane)
                RaiseEvent radioMessage(plane.frequency, plane.callsign & ": On your frequency!")
                mdlNetworkhandling.serverSendUpdateToClients(Me, enumNetworkMessageType.radioMessage, New structRadioMessageNetwork With {.frequency = plane.frequency, .message = plane.callsign & ": On your frequency!"})

            End If

            'restart timer
            'Me.timerSpawn.Interval = 1000
            Me.timerSpawn.Interval = randomizer.Next(Me.minSpawnDelay, Me.maxSpawnDelay + 1)

            Me.timerSpawn.Enabled = True


            GC.Collect()

        End If
    End Sub

    Private Sub tmrServerListen_Tick(sender As Object, e As EventArgs) Handles tmrServerListen.Tick
        'server is on and we should reveive data once in a while
        'the data, the server reveices can only be commands to a plane

        Try

            For Each singleClient In Me.TCPServerClientPlayers
                Dim stream As NetworkStream = singleClient.GetStream

                If stream.DataAvailable Then
                    Dim arraySizeArray(4) As Byte
                    stream.Read(arraySizeArray, 0, CInt(4))
                    Dim arraysize As Int32 = BitConverter.ToInt32(arraySizeArray, 0)
                    Dim messageInBytes(arraysize) As Byte
                    Dim reader As New BinaryReader(stream)
                    Dim bytesRead As Long = 0

                    messageInBytes = reader.ReadBytes(arraysize)

                    'we received something - let's extract the commands out of it
                    Dim formatter As New BinaryFormatter
                    Dim streamTarget As New MemoryStream(messageInBytes)
                    Dim command As New clsPlane.structCommandInfo
                    command = formatter.Deserialize(streamTarget)

                    'if plane is not nothing, the command is for a plane
                    If Not command.plane Is Nothing Then
                        'hand over the commands to the plane so that the plane can react
                        'find plane based on callsign to makse sure that it gets the command asasigned
                        Dim relevantPlane As clsPlane = Me.Planes.Find(Function(p As clsPlane) p.callsign = command.plane)
                        'command.plane = relevantPlane.callsign
                        'hand over all commands and parameters and hope nothing crashes

                        'the "is" parameter in Dijkstra and other pathfunders will not work
                        'we need to find the right nodes in the server game class and assign then
                        If Not command.groundTaxiGoalPointCommandParameter Is Nothing Then
                            command.groundTaxiGoalPointCommandParameter = Me.AirPort.POIs(command.groundTaxiGoalPointCommandParameter.name.ToLower)
                        End If
                        If Not command.groundTaxiViaCommandParameter Is Nothing Then
                            Dim tmpViaConnectionPopints As New List(Of clsConnectionPoint)
                            For Each singleViaPoint As clsConnectionPoint In command.groundTaxiViaCommandParameter
                                tmpViaConnectionPopints.Add(Me.AirPort.allNavigationPoints.Find(Function(p As clsConnectionPoint) p.name.ToLower = singleViaPoint.name.ToLower))
                            Next
                            command.groundTaxiViaCommandParameter = tmpViaConnectionPopints
                        End If
                        If Not command.groundTaxiRunwayCommandParameter Is Nothing Then
                            command.groundTaxiRunwayCommandParameter = Me.AirPort.getRunWayByID(command.groundTaxiRunwayCommandParameter.objectID)
                        End If

                        If Not command.airNavPointCommandParameter Is Nothing Then
                            command.airNavPointCommandParameter = Me.AirPort.airSpaceNavPoints.Find(Function(p As clsNavigationPoint) p.name.ToLower = command.airNavPointCommandParameter.name.ToLower)
                        End If
                        If Not command.airAirPathCommandParameter Is Nothing Then
                            Dim pathname As String = command.airAirPathCommandParameter.name
                            command.airAirPathCommandParameter = Me.AirPort.getSTARbyName(pathname)
                            If command.airAirPathCommandParameter Is Nothing Then
                                command.airAirPathCommandParameter = Me.AirPort.getSIDbyName(pathname)
                            End If
                        End If
                        If Not command.airTouchDownCommandParameter Is Nothing Then
                            Dim ParameterTuple As Tuple(Of clsTouchDownWayPoint, clsNavigationPoint) = Me.AirPort.getArrivalPointAndGoaroundPointByName(command.airTouchDownCommandParameter.name)

                            command.airTouchDownCommandParameter = ParameterTuple.Item1
                            command.airGoAroundPointCommandParameter = ParameterTuple.Item2
                        End If

                        If Not command.radioMessage.message Is Nothing Then
                            RaiseEvent radioMessage(command.radioMessage.frequency, command.radioMessage.message)
                        End If


                        relevantPlane.commandInfo = command
                    ElseIf Not command.towerRunwayID Is Nothing Then
                        'update new runwaystate of given runway
                        If Me.AirPort.getRunWayByID(command.towerRunwayID).canHandleArrivals Then
                            Me.AirPort.getRunWayByID(command.towerRunwayID).isAvailableForArrival = command.towerRunwayIsNewActiveForArrival
                            RaiseEvent availableRunwaysArrivalChanged()
                        End If

                        Me.AirPort.getRunWayByID(command.towerRunwayID).isAvailableForDeparture = command.towerRunwayIsNewActiveForDeparture
                        RaiseEvent availableRunwaysDepartureChanged()
                    ElseIf Not command.radioMessage.message Is Nothing Then
                        RaiseEvent radioMessage(command.radioMessage.frequency, command.radioMessage.message)
                    End If

                End If
            Next

        Catch ex As Exception
            'nothing received
        End Try
    End Sub

    Private Sub tmrServerSend_Tick(sender As Object, e As EventArgs) Handles tmrServerSendKeyFrame.Tick
        mdlNetworkhandling.serverSendUpdateToClients(Me, enumNetworkMessageType.keyframe)
    End Sub

    Private Sub tmrClientListen_Tick(sender As Object, e As EventArgs) Handles tmrClientListen.Tick
        mdlNetworkhandling.clientReceiveUpdateFromServer(Me)
    End Sub

    Friend Sub sendCommandsToServer(ByRef commandInfo As clsPlane.structCommandInfo)
        If Me.isclient Then
            Try
                'necessary to copy plane because otheriwise the containing form would also try to serialize
                'Dim tmpPlane As clsPlane = commandInfo.plane
                'commandInfo.plane = tmpPlane.callsign
                'convert to bytearray
                Dim formatter As New BinaryFormatter
                Dim streamTarget As New MemoryStream()

                formatter.Serialize(streamTarget, commandInfo)

                Dim messageAsByteArray() As Byte = streamTarget.ToArray
                Dim arraySize As Int32 = messageAsByteArray.Length
                Dim arraySizeArray() As Byte = BitConverter.GetBytes(arraySize)
                Dim byteArray() As Byte = arraySizeArray.Concat(messageAsByteArray).ToArray

                'send to server
                Me.TCPClient.Client.Send(byteArray)

                'delete original plane's command IF it was a plane command 
                'If Not tmpPlane Is Nothing Then tmpPlane.clearCommand()
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub timerWindChange_Tick(sender As Object, e As EventArgs) Handles timerWindChange.Tick
        Me.timerWindChange.Enabled = False

        Dim randomizer As New Random(DateTime.Now.Millisecond)

        'randomly decide angle
        Dim deltawind As Integer = randomizer.Next(Me.minWindChangeAngle, maxWindChangeAngle + 1)

        'randomly decide sign
        Dim sign As Int16 = randomizer.Next(0, 2)
        If sign = 0 Then sign = -1

        'set wind direction
        Me.AirPort.setWindDirection(Me.AirPort.windDirectionTo + sign * deltawind)

        'decide new time and restart timer
        Me.timerWindChange.Interval = randomizer.Next(Me.minWindChangeDelay, Me.maxWindChangeDelay + 1)
        Me.timerWindChange.Enabled = True

    End Sub

    Private Sub addCrash(ByRef plane As clsPlane)
        Me.crashedPlanes += 1
    End Sub

    Private Sub addLanding(ByRef plane As clsPlane)
        Me.successfulLandings += 1
    End Sub

    Private Sub addTakeOff(ByRef plane As clsPlane)
        Me.successfulTakeOffs += 1
    End Sub

    Private Sub addGated(ByRef plane As clsPlane)
        Me.successfulGated += 1
    End Sub

    Private Sub addArrived(ByRef plane As clsPlane)
        Me.successfulArrival += 1
    End Sub

    Private Sub addDeparted(ByRef plane As clsPlane)
        Me.successfulDeparted += 1
    End Sub


    Private Sub timerHistory_Tick(sender As Object, e As EventArgs) Handles timerHistory.Tick
        'add current position to history
        For Each singlePlane As clsPlane In Me.Planes
            Dim positionInfo As Tuple(Of clsDistanceCollection, clsDistanceCollection) = Tuple.Create(New clsDistanceCollection(singlePlane.pos_X.feet, clsDistanceCollection.enumDistanceUnits.feet), New clsDistanceCollection(singlePlane.pos_Y.feet, clsDistanceCollection.enumDistanceUnits.feet))
            singlePlane.air_FlightPathHistory.Add(positionInfo)

            'remove all positions > 10
            While singlePlane.air_FlightPathHistory.Count > PLANE_HISTORY
                singlePlane.air_FlightPathHistory.Remove(singlePlane.air_FlightPathHistory.First)
            End While
        Next
    End Sub

    Friend Sub raiseEventManuallyPlaneSpawned(ByRef plane As clsPlane)
        RaiseEvent spawnedPlane(plane)
    End Sub

    Friend Sub raiseEventManuallyPlaneDeSpawned(ByRef plane As clsPlane)
        RaiseEvent despawnedPlane(plane)
    End Sub

    Friend Sub raiseEventManuallySelectedPlaneStatusChanged(ByRef plane As clsPlane)
        RaiseEvent selectedPlaneStatusChanged(plane)
    End Sub

    Friend Sub raiseEventManuallyPlaneFrequencyChanged(ByRef plane As clsPlane)
        RaiseEvent planeFrequencyChanged(plane)
    End Sub

    Friend Sub raiseEventManuallyAvailableRunwaysArrivalChanged()
        RaiseEvent availableRunwaysArrivalChanged()
    End Sub

    Friend Sub raiseEventManuallyAvailableRunwaysDepartureChanged()
        RaiseEvent availableRunwaysDepartureChanged()
    End Sub

    Friend Sub raiseEventManuallyUsedRunwaysChanged()
        RaiseEvent usedRunwaysChanged()
    End Sub

    Friend Sub raiseEventManuallyRadioMessage(ByRef frequency As clsPlane.enumFrequency, ByRef message As String)
        RaiseEvent radioMessage(frequency, message)
    End Sub

    Friend Sub raiseEventManuallyTicked(ByRef milliseconds As Long)
        RaiseEvent ticked(milliseconds)
    End Sub

End Class
