Option Explicit On
Imports System.Device.Location
Imports System.Runtime.CompilerServices
Imports System.Xml.XPath
Imports System.Globalization

<Serializable>
Public Class clsAirport
    <Serializable> Public Structure structAirPortMeta
        Friend Name As String
        Friend IATA As String
        Friend ICAO As String
        Friend asOf As Date
    End Structure

    <Serializable> Public Structure structGeoCoordinate
        Friend lng As Double
        Friend lat As Double
    End Structure

    Friend ReadOnly Property meta As structAirPortMeta

    Friend ReadOnly landscape As clsLandscape

    Friend ReadOnly Property Ramps As clsRamp()
    Friend ReadOnly Property airSpaceNavPoints As New List(Of clsConnectionPoint)
    Friend ReadOnly Property runWays As clsRunWay()
    Friend ReadOnly Property referenceCoordinate As structGeoCoordinate
    Friend ReadOnly Property ARP As clsNavigationPoint
    Friend ReadOnly Property STARs As clsAirPath()
    Friend ReadOnly Property SIDs As clsAirPath()

    Friend ReadOnly Property POIs As New Dictionary(Of String, clsNavigationPoint)
    Friend ReadOnly Property gates As List(Of clsGate)
        Get
            Dim result As New List(Of clsGate)
            'go thourgh all ramps
            For C1 As Long = 0 To Me.Ramps.GetUpperBound(0)
                'go through all gates
                For C2 As Long = 0 To Me.Ramps(C1).Gates.GetUpperBound(0)
                    'add all gates
                    result.Add(Me.Ramps(C1).Gates(C2))
                Next
            Next
            Return result
        End Get
    End Property

    Friend ReadOnly Property groundRadarBoundaryPoints As New List(Of clsNavigationPoint)
    Friend ReadOnly Property towerRadarBoundaryPoints As New List(Of clsNavigationPoint)
    Friend ReadOnly Property appDepRadarBoundaryPoints As New List(Of clsNavigationPoint)
    Friend ReadOnly Property traconRadarBoundaryPoints As New List(Of clsNavigationPoint)
    Friend allNavigationPoints As New List(Of clsNavigationPoint)           'list of all navigation points used in this airport
    Friend allNavigationPaths As New List(Of clsNavigationPath)               'list of all navigation paths used in this airport
    Friend allPathways As New List(Of clsPathWay)                            'list of all paths ways used in this airport

    Friend ReadOnly Property groundRadarMostTop As Double
        Get
            Dim tmpMostTop As Double = Double.PositiveInfinity
            For Each singleCoordinate As clsNavigationPoint In Me.groundRadarBoundaryPoints
                If singleCoordinate.pos_Y < tmpMostTop Then tmpMostTop = singleCoordinate.pos_Y
            Next

            Return tmpMostTop

        End Get
    End Property
    Friend ReadOnly Property groundRadarMostBottom As Double
        Get
            Dim tmpMostBottom As Double = Double.NegativeInfinity
            For Each singleCoordinate As clsNavigationPoint In Me.groundRadarBoundaryPoints
                If singleCoordinate.pos_Y > tmpMostBottom Then tmpMostBottom = singleCoordinate.pos_Y
            Next

            Return tmpMostBottom

        End Get
    End Property
    Friend ReadOnly Property groundRadarMostLeft As Double
        Get
            Dim tmpMostLeft As Double = Double.PositiveInfinity
            For Each singleCoordinate As clsNavigationPoint In Me.groundRadarBoundaryPoints
                If singleCoordinate.pos_X < tmpMostLeft Then tmpMostLeft = singleCoordinate.pos_X
            Next

            Return tmpMostLeft

        End Get
    End Property
    Friend ReadOnly Property groundRadarMostRight As Double
        Get
            Dim tmpMostRight As Double = Double.NegativeInfinity
            For Each singleCoordinate As clsNavigationPoint In Me.groundRadarBoundaryPoints
                If singleCoordinate.pos_X > tmpMostRight Then tmpMostRight = singleCoordinate.pos_X
            Next

            Return tmpMostRight

        End Get
    End Property
    Friend ReadOnly Property groundRadarWidth As Double
        Get
            Return Me.groundRadarMostRight - groundRadarMostLeft
        End Get
    End Property
    Friend ReadOnly Property groundRadarHeight As Double
        Get
            Return Me.groundRadarMostBottom - Me.groundRadarMostTop
        End Get
    End Property

    Friend ReadOnly Property towerRadarMostTop As Double
        Get
            Dim tmpMostTop As Double = Double.PositiveInfinity
            For Each singleCoordinate As clsNavigationPoint In Me.towerRadarBoundaryPoints
                If singleCoordinate.pos_Y < tmpMostTop Then tmpMostTop = singleCoordinate.pos_Y
            Next

            Return tmpMostTop

        End Get
    End Property
    Friend ReadOnly Property towerRadarMostBottom As Double
        Get
            Dim tmpMostBottom As Double = Double.NegativeInfinity
            For Each singleCoordinate As clsNavigationPoint In Me.towerRadarBoundaryPoints
                If singleCoordinate.pos_Y > tmpMostBottom Then tmpMostBottom = singleCoordinate.pos_Y
            Next

            Return tmpMostBottom

        End Get
    End Property
    Friend ReadOnly Property towerRadarMostLeft As Double
        Get
            Dim tmpMostLeft As Double = Double.PositiveInfinity
            For Each singleCoordinate As clsNavigationPoint In Me.towerRadarBoundaryPoints
                If singleCoordinate.pos_X < tmpMostLeft Then tmpMostLeft = singleCoordinate.pos_X
            Next

            Return tmpMostLeft

        End Get
    End Property
    Friend ReadOnly Property towerRadarMostRight As Double
        Get
            Dim tmpMostRight As Double = Double.NegativeInfinity
            For Each singleCoordinate As clsNavigationPoint In Me.towerRadarBoundaryPoints
                If singleCoordinate.pos_X > tmpMostRight Then tmpMostRight = singleCoordinate.pos_X
            Next

            Return tmpMostRight

        End Get
    End Property
    Friend ReadOnly Property towerRadarWidth As Double
        Get
            Return Me.towerRadarMostRight - towerRadarMostLeft
        End Get
    End Property
    Friend ReadOnly Property towerRadarHeight As Double
        Get
            Return Me.towerRadarMostBottom - Me.towerRadarMostTop
        End Get
    End Property


    Friend ReadOnly Property appDepRadarMostTop As Double
        Get
            Dim tmpMostTop As Double = Double.PositiveInfinity
            For Each singleCoordinate As clsNavigationPoint In Me.appDepRadarBoundaryPoints
                If singleCoordinate.pos_Y < tmpMostTop Then tmpMostTop = singleCoordinate.pos_Y
            Next

            Return tmpMostTop

        End Get
    End Property
    Friend ReadOnly Property appDepRadarMostBottom As Double
        Get
            Dim tmpMostBottom As Double = Double.NegativeInfinity
            For Each singleCoordinate As clsNavigationPoint In Me.appDepRadarBoundaryPoints
                If singleCoordinate.pos_Y > tmpMostBottom Then tmpMostBottom = singleCoordinate.pos_Y
            Next

            Return tmpMostBottom

        End Get
    End Property
    Friend ReadOnly Property appDepRadarMostLeft As Double
        Get
            Dim tmpMostLeft As Double = Double.PositiveInfinity
            For Each singleCoordinate As clsNavigationPoint In Me.appDepRadarBoundaryPoints
                If singleCoordinate.pos_X < tmpMostLeft Then tmpMostLeft = singleCoordinate.pos_X
            Next

            Return tmpMostLeft

        End Get
    End Property
    Friend ReadOnly Property appDepRadarMostRight As Double
        Get
            Dim tmpMostRight As Double = Double.NegativeInfinity
            For Each singleCoordinate As clsNavigationPoint In Me.appDepRadarBoundaryPoints
                If singleCoordinate.pos_X > tmpMostRight Then tmpMostRight = singleCoordinate.pos_X
            Next

            Return tmpMostRight

        End Get
    End Property
    Friend ReadOnly Property appDepRadarWidth As Double
        Get
            Return Me.appDepRadarMostRight - appDepRadarMostLeft
        End Get
    End Property
    Friend ReadOnly Property appDepRadarHeight As Double
        Get
            Return Me.appDepRadarMostBottom - Me.appDepRadarMostTop
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

    ''' <summary>
    ''' list of all open runways for arrival as list of clsRunway
    ''' </summary>
    ''' <returns></returns>
    Friend ReadOnly Property openArrivalRunwaysAsListOfRunways As List(Of clsRunWay)
        Get
            Dim result As New List(Of clsRunWay)
            For Each singleRunway As clsRunWay In Me.runWays
                If singleRunway.canHandleArrivals AndAlso singleRunway.isAvailableForArrival Then
                    result.Add(singleRunway)
                End If
            Next
            Return result
        End Get
    End Property

    ''' <summary>
    ''' list of all open runways for departure as list of clsRunway
    ''' </summary>
    ''' <returns></returns>
    Friend ReadOnly Property openDepartureRunwaysAsListOfRunways As List(Of clsRunWay)
        Get
            Dim result As New List(Of clsRunWay)
            For Each singleRunway As clsRunWay In Me.runWays
                If singleRunway.canHandleDepartures AndAlso singleRunway.isAvailableForDeparture Then
                    result.Add(singleRunway)
                End If
            Next
            Return result
        End Get
    End Property

    ''' <summary>
    ''' list of all runways open for arrival as list of IDs
    ''' </summary>
    ''' <returns></returns>
    Friend ReadOnly Property openArrivalRunwayIDsAsListOfStrings As List(Of String)
        Get
            Dim result As New List(Of String)
            For Each singleRunway As clsRunWay In Me.runWays
                If singleRunway.canHandleArrivals AndAlso singleRunway.isAvailableForArrival Then
                    result.Add(singleRunway.objectID)
                End If
            Next
            Return result
        End Get
    End Property

    ''' <summary>
    ''' list of all  runways open for departures as list of IDs
    ''' </summary>
    ''' <returns></returns>
    Friend ReadOnly Property openDepartureRunwayIDsAsListOfStrings As List(Of String)
        Get
            Dim result As New List(Of String)
            For Each singleRunway As clsRunWay In Me.runWays
                If singleRunway.canHandleDepartures AndAlso singleRunway.isAvailableForDeparture Then
                    result.Add(singleRunway.objectID)
                End If
            Next
            Return result
        End Get
    End Property

    Friend ReadOnly Property usedRunwayIDsAsListOfStrings As List(Of String)
        Get
            Dim result As New List(Of String)
            For Each singleRunway As clsRunWay In Me.runWays
                If singleRunway.isInUse Then
                    result.Add(singleRunway.objectID)
                End If
            Next
            Return result
        End Get
    End Property

    Public Sub New(ByRef xElement As XElement)
        'get reference coodinate to determine coorindates in meters
        Me.referenceCoordinate = New clsAirport.structGeoCoordinate With {.lat = xElement.@ref_lat, .lng = xElement.@ref_lng}
        Dim xARP As New XElement("ARP")
        xARP.@lat = xElement.@ref_lat
        xARP.@lng = xElement.@ref_lng
        Me.ARP = New clsNavigationPoint(xARP, Me.referenceCoordinate)

        'get groundcontrol coordinates
        For Each singleCoordinate As XElement In xElement.<groundradar>(0).<coordinate>
            Dim newCoorindate As New clsNavigationPoint(singleCoordinate, Me.referenceCoordinate)
            Me.groundRadarBoundaryPoints.Add(newCoorindate)
        Next

        'get towercontrol coordinates
        For Each singleCoordinate As XElement In xElement.<tower>(0).<coordinate>
            Dim newCoorindate As New clsNavigationPoint(singleCoordinate, Me.referenceCoordinate)
            Me.towerRadarBoundaryPoints.Add(newCoorindate)
        Next

        'get APPDEP coordinates
        For Each singleCoordinate As XElement In xElement.<appdep>(0).<coordinate>
            Dim newCoorindate As New clsNavigationPoint(singleCoordinate, Me.referenceCoordinate)

            Me.appDepRadarBoundaryPoints.Add(newCoorindate)
        Next

        'get TRACON coordinates
        For Each singleCoordinate As XElement In xElement.<tracon>(0).<coordinate>
            Dim newCoorindate As New clsNavigationPoint(singleCoordinate, Me.referenceCoordinate)

            Me.traconRadarBoundaryPoints.Add(newCoorindate)
        Next

        'prepare landscape information
        Me.landscape = New clsLandscape(xElement.<landscape>(0), Me.referenceCoordinate)

        'prepare ramps
        ReDim Me.Ramps(xElement.<ramp>.Count - 1)
        For C1 As Long = 0 To xElement.<ramp>.Count - 1
            Me.Ramps(C1) = New clsRamp(xElement.<ramp>(C1), Me.referenceCoordinate)
            Me.allNavigationPoints.AddRange(Me.Ramps(C1).allNavigationPoints)
            Me.allNavigationPaths.AddRange(Me.Ramps(C1).allNavigationPaths)
            Me.allPathways.AddRange(Me.Ramps(C1).allPathWays)
        Next


        'prepare airnavpoints
        For C1 As Long = 0 To xElement.<airnavpoints>(0).<airnavpoint>.Count - 1
            'Me.airSpaceNavPoints.Add(New clsConnectionPoint(xElement.<airnavpoints>(0).<airnavpoint>(C1), Me.referenceCoordinate))
            Me.airSpaceNavPoints.Add(New clsAirNavPoint(xElement.<airnavpoints>(0).<airnavpoint>(C1), Me.referenceCoordinate))
        Next
        Me.allNavigationPoints.AddRange(Me.airSpaceNavPoints)

        'search and prepare all stars
        'STARs
        ReDim Me.STARs(xElement.<stars>(0).<star>.Count - 1)
        For C1 As Long = 0 To xElement.<stars>(0).<star>.Count - 1
            Dim newStarPath As New List(Of clsAStarEngine.structPathStep)
            Dim previousPoint As clsConnectionPoint = Nothing
            Dim nextPoint As clsConnectionPoint = Nothing
            For Each SingleStar As XElement In xElement.<stars>(0).<star>(C1).<starpoint>
                nextPoint = Me.airSpaceNavPoints.Find(Function(p As clsConnectionPoint) p.objectID = SingleStar.@name)
                'error message if point not found
                If nextPoint Is Nothing Then MsgBox("STAR: " & xElement.<stars>(0).<star>(C1).@name & vbNewLine & "missing point: " & SingleStar.@name & vbNewLine & vbNewLine & "Program will crash.", MsgBoxStyle.Critical, "STAR point not found!")


                '!!! add dynamic target speed for each section
                Dim targetspeed As clsSpeedCollection = Nothing
                Dim name As String = xElement.<stars>(0).<star>(C1).@name
                Dim newPath As clsNavigationPath = Nothing
                If Not previousPoint Is Nothing Then newPath = New clsNavigationPath(previousPoint, nextPoint, clsNavigationPath.enumPathWayType.AirWay, name, targetspeed, name, Guid.NewGuid.ToString)
                Dim newStructPathStep As New clsAStarEngine.structPathStep With {
                    .nextWayPoint = nextPoint,
                    .taxiwayToWayPoint = newPath}
                newStarPath.Add(newStructPathStep)

                If Not newPath Is Nothing Then Me.allNavigationPaths.Add(newPath)

                previousPoint = nextPoint
            Next

            Me.STARs(C1) = New clsAirPath(newStarPath, xElement.<stars>(0).<star>(C1).@name)
        Next

        'search and prepare all SIDs
        'SIDs
        ReDim Me.SIDs(xElement.<sids>(0).<sid>.Count - 1)
        For C1 As Long = 0 To xElement.<sids>(0).<sid>.Count - 1
            Dim newSidPath As New List(Of clsAStarEngine.structPathStep)
            Dim previousPoint As clsConnectionPoint = Nothing
            Dim nextPoint As clsConnectionPoint = Nothing
            For Each SingleSid As XElement In xElement.<sids>(0).<sid>(C1).<sidpoint>
                nextPoint = Me.airSpaceNavPoints.Find(Function(p As clsConnectionPoint) p.objectID = SingleSid.@name)
                If nextPoint Is Nothing Then MsgBox("SID: " & xElement.<sids>(0).<sid>(C1).@name & vbNewLine & "missing point: " & SingleSid.@name & vbNewLine & vbNewLine & "Program will crash.", MsgBoxStyle.Critical, "SID point not found!")

                '!!! add dynamic target speed for each section
                Dim targetspeed As clsSpeedCollection = Nothing
                Dim name As String = xElement.<sids>(0).<sid>(C1).@name
                Dim newPath As clsNavigationPath = Nothing
                If Not previousPoint Is Nothing Then newPath = New clsNavigationPath(previousPoint, nextPoint, clsNavigationPath.enumPathWayType.AirWay, name, targetspeed, name, Guid.NewGuid.ToString)
                Dim newStructPathStep As New clsAStarEngine.structPathStep With {
                    .nextWayPoint = nextPoint,
                    .taxiwayToWayPoint = newPath}
                newSidPath.Add(newStructPathStep)
                previousPoint = nextPoint

                If Not newPath Is Nothing Then Me.allNavigationPaths.Add(newPath)
            Next

            Me.SIDs(C1) = New clsAirPath(newSidPath, xElement.<sids>(0).<sid>(C1).@name)
        Next

        'prepare runways
        ReDim Me.runWays(xElement.<runway>.Count - 1)
        For C1 As Long = 0 To xElement.<runway>.Count - 1
            Me.runWays(C1) = New clsRunWay(xElement.<runway>(C1), Me.referenceCoordinate, Me.Ramps, Me.STARs, Me.SIDs, Me.airSpaceNavPoints)
            Me.allNavigationPoints.AddRange(Me.runWays(C1).allNavigationPoints)
            Me.allNavigationPaths.AddRange(Me.runWays(C1).allNavigationPaths)
            Me.allPathways.AddRange(Me.runWays(C1).allPathWays)
        Next


        '= collect points of interest =

        'search all ramps for POIs
        For C1 As Long = 0 To Me.Ramps.GetUpperBound(0)
            For C2 As Long = 0 To Me.Ramps(C1).POIs.Count - 1
                Me.POIs.Add(Me.Ramps(C1).POIs.Keys(C2).ToLower, Me.Ramps(C1).POIs(Me.Ramps(C1).POIs.Keys(C2).ToLower))
            Next
        Next

        'search all runways for POIs
        For C1 As Long = 0 To Me.runWays.GetUpperBound(0)
            For C2 As Long = 0 To Me.runWays(C1).POIs.Count - 1
                Me.POIs.Add(Me.runWays(C1).POIs.Keys(C2).ToLower, Me.runWays(C1).POIs(Me.runWays(C1).POIs.Keys(C2).ToLower))

            Next
        Next


        'Dim result As clsConnectionPoint = Nothing
        'Dim isFound As Boolean = False


        Dim provider As CultureInfo = CultureInfo.InvariantCulture
        Dim meta As structAirPortMeta = New structAirPortMeta With {.Name = xElement.@name, .IATA = xElement.@IATA, .ICAO = xElement.@ICAO, .asOf = Date.ParseExact(xElement.@date, "yyyy/MM/dd", provider)}
        Me.meta = meta

        'crosswind
        Me.setMaxCrossWind(xElement.@maxcrosswind)

        'weather
        Dim randomizer As New Random(DateTime.Now.Millisecond)
        Me.setWindDirection(randomizer.Next(1, 361))



        'close runways that are opposite to wind direction
        For Each singleRunway As clsRunWay In Me.runWays
            'arrivals
            If singleRunway.canHandleArrivals Then
                If mdlHelpers.diffBetweenAnglesAbs(singleRunway.windDirectionFrom,
                                               singleRunway.landingAngle) > Me.maxCrossWind Then
                    singleRunway.isAvailableForArrival = False
                End If
            End If

            'departures
            If singleRunway.canHandleDepartures Then
                If mdlHelpers.diffBetweenAnglesAbs(Me.windDirectionFrom,
                                               singleRunway.takeoffAngle) > Me.maxCrossWind Then
                    singleRunway.isAvailableForDeparture = False
                End If
            End If
        Next


        'check for empty elements
        For Each singleElement As clsNavigationPath In Me.allNavigationPaths
            If singleElement.ObjectID Is Nothing Then MsgBox(singleElement.name)
            If singleElement.ObjectID = "" Then MsgBox(singleElement.name)
        Next

        For Each singleElement As clsPathWay In Me.allPathways
            If singleElement.ObjectID Is Nothing Then MsgBox(singleElement.Name)
            If singleElement.ObjectID = "" Then MsgBox(singleElement.Name)

        Next

        For Each singleElement As clsNavigationPoint In Me.allNavigationPoints
            If singleElement.objectID Is Nothing Then MsgBox(singleElement.name)
            If singleElement.objectID = "" Then MsgBox(singleElement.name)

        Next


    End Sub

    Friend Function getConnectionPointByName(ByVal name As String) As clsConnectionPoint
        Dim result As clsConnectionPoint = Nothing

        For Each singleRamp In Me.Ramps
            result = singleRamp.getConnectionPointByName(name)
            If Not result Is Nothing Then Exit For
        Next

        Return result
    End Function

    Friend Function getNavigationPointById(ByVal ID As String) As clsNavigationPoint
        Dim result As clsNavigationPoint = Nothing

        result = Me.allNavigationPoints.Find(Function(p As clsNavigationPoint) p.objectID = ID)

        Return result
    End Function

    Friend Function getNavigationPathById(ByVal ID As String) As clsNavigationPath
        Dim result As clsNavigationPath = Nothing

        result = Me.allNavigationPaths.Find(Function(p As clsNavigationPath) p.ObjectID = ID)

        Return result
    End Function


    Friend Function getPathWayById(ByVal ID As String) As clsPathWay
        Dim result As clsPathWay = Nothing

        result = Me.allPathways.Find(Function(p As clsPathWay) p.ObjectID = ID)

        Return result
    End Function

    Friend Function getRunwayByConnectionPoint(ByRef connectionPoint As clsConnectionPoint) As clsRunWay
        Dim result As clsRunWay = Nothing

        For Each singleRunway In Me.runWays
            For Each singleLineUpWay As clsLineUpWay In singleRunway.lineUpPaths
                If singleLineUpWay.entryPoint Is connectionPoint Or singleLineUpWay.exitPoint Is connectionPoint Then
                    result = singleRunway
                    Exit For
                End If
            Next
        Next

        Return result
    End Function

    Friend Function getRunWayByID(ByVal id As String) As clsRunWay
        Dim result As clsRunWay = Nothing

        For C1 As Long = 0 To Me.runWays.GetUpperBound(0)
            If Me.runWays(C1).objectID = id Then
                Return Me.runWays(C1)
                Exit For
            End If
        Next

        Return Nothing
    End Function

    Friend Function getRampByID(ByVal id As String) As clsRamp
        Dim result As clsRamp = Nothing

        For C1 As Long = 0 To Me.Ramps.GetUpperBound(0)
            If Me.Ramps(C1).objectID = id Then
                Return Me.Ramps(C1)
                Exit For
            End If
        Next

        Return Nothing
    End Function

    Friend Function getSTARbyName(ByVal name As String) As clsAirPath
        Dim result As clsAirPath = Nothing

        For Each singleRunway As clsRunWay In Me.runWays
            For Each singleStar As clsAirPath In singleRunway.STARs
                If singleStar.name = name Then
                    result = singleStar
                End If
            Next
        Next

        Return result
    End Function

    Friend Function getSIDbyName(ByVal name As String) As clsAirPath
        Dim result As clsAirPath = Nothing

        For Each singleRunway As clsRunWay In Me.runWays
            For Each singleSid As clsAirPath In singleRunway.SIDs
                If singleSid.name = name Then
                    result = singleSid
                End If
            Next
        Next

        Return result
    End Function

    Friend Function getArrivalPointAndGoaroundPointByName(ByVal arrivalPointName As String) As Tuple(Of clsTouchDownWayPoint, clsNavigationPoint)
        Dim result As Tuple(Of clsTouchDownWayPoint, clsNavigationPoint)

        Dim arrivalpoint As clsTouchDownWayPoint = Nothing
        Dim goaroundPoint As clsNavigationPoint = Nothing
        For Each singleRunway As clsRunWay In Me.runWays
            If (Not singleRunway.arrivalPoint Is Nothing) AndAlso singleRunway.arrivalPoint.name = arrivalPointName Then
                arrivalpoint = singleRunway.arrivalPoint
                goaroundPoint = singleRunway.goAroundPoint
            End If
        Next

        result = Tuple.Create(arrivalpoint, goaroundPoint)
        Return result
    End Function

    ''' <summary>
    ''' sets wind direction w/ angle to where the wind is going
    ''' </summary>
    ''' <param name="direction">direction where whe wind goes</param>
    Friend Sub setWindDirection(ByVal direction As Double)
        Me.windDirectionTo = direction

        'set winddirection for all touchdownwaypoints
        For Each singleRunWay As clsRunWay In Me.runWays
            singleRunWay.setWindDirection(direction)
        Next
    End Sub

    Friend Sub setMaxCrossWind(ByVal _maxcrosswindangle As Long)
        Me.maxCrossWind = _maxcrosswindangle

        'set maxcrosswind for all touchdownwaypoints
        For Each singleRunWay As clsRunWay In Me.runWays
            singleRunWay.setMaxCrossWind(_maxcrosswindangle)
        Next
    End Sub

    Friend Sub tick(ByVal timespan As TimeSpan)

    End Sub

End Class
