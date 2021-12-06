Option Explicit On
Imports Microsoft.VisualBasic.ApplicationServices

Public Class frmAllControl
    Friend WithEvents Game As clsGame

    Private supressAvailableRunwayCheckListEvaluation As Boolean = True

    Private Sub frmCommands_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Game = frmMenu.Game

        'load planes into respective lists
        For Each singlePlane In Me.Game.Planes
            Me.lstGame.addStripe(singlePlane)
            Select Case singlePlane.frequency
                Case clsPlane.enumFrequency.ground
                    Me.lstGround.addStripe(singlePlane)
                Case clsPlane.enumFrequency.tower
                    Me.lstTower.addStripe(singlePlane)
                Case clsPlane.enumFrequency.departure, clsPlane.enumFrequency.arrival, clsPlane.enumFrequency.appdep, clsPlane.enumFrequency.tracon
                    Me.lstAppDep.addStripe(singlePlane)
            End Select
        Next

        'POI list
        Dim RwyItems As ToolStripMenuItem = Me.cmsAppDepNavPoints.Items.Add("Runways")
        Dim GateItems As ToolStripMenuItem = Me.cmsAppDepNavPoints.Items.Add("Gates")
        Dim OtherItems As ToolStripMenuItem = Me.cmsAppDepNavPoints.Items.Add("Other")

        'first TERMINAL
        Dim newTaxiToItem As ToolStripItem = cmsGroundTaxiTo.Items.Add("TERMINAL")
        newTaxiToItem.Tag = "TERMINAL"
        AddHandler newTaxiToItem.Click, AddressOf mnuGroundTaxiToClicked
        For Each singlePOI As clsConnectionPoint In Me.Game.AirPort.POIs.Values
            If singlePOI.isRunwayPoint Then
                Dim newItem As ToolStripItem = RwyItems.DropDownItems.Add(singlePOI.UIName)
                newItem.Tag = singlePOI.name
                AddHandler newItem.Click, AddressOf mnuGroundTaxiToClicked
            ElseIf singlePOI.isGate Then
                Dim newItem As ToolStripItem = GateItems.DropDownItems.Add(singlePOI.UIName)
                newItem.Tag = singlePOI.name
                AddHandler newItem.Click, AddressOf mnuGroundTaxiToClicked
            Else
                Dim newItem As ToolStripItem = OtherItems.DropDownItems.Add(singlePOI.UIName)
                newItem.Tag = singlePOI.name
                AddHandler newItem.Click, AddressOf mnuGroundTaxiToClicked
            End If
        Next

        cmsGroundTaxiTo.Items.Add(RwyItems)
        cmsGroundTaxiTo.Items.Add(GateItems)
        cmsGroundTaxiTo.Items.Add(OtherItems)

        'navpoints
        Dim STARitem As ToolStripMenuItem = Me.cmsAppDepNavPoints.Items.Add("STARs")
        Dim SIDitem As ToolStripMenuItem = Me.cmsAppDepNavPoints.Items.Add("SIDs")
        Dim VORTACitem As ToolStripMenuItem = Me.cmsAppDepNavPoints.Items.Add("VORTACs")
        Dim Otheritem As ToolStripMenuItem = Me.cmsAppDepNavPoints.Items.Add("Others")

        For Each singleNavPoint As clsAirNavPoint In Me.Game.AirPort.airSpaceNavPoints
            Select Case singleNavPoint.NavPointType
                Case clsAirNavPoint.enumAirNavPointType.STAR
                    Dim newItem As ToolStripItem = STARitem.DropDownItems.Add(singleNavPoint.UIName)
                    newItem.Tag = singleNavPoint.objectID
                    AddHandler newItem.Click, AddressOf mnuNavPointclicked
                Case clsAirNavPoint.enumAirNavPointType.SID
                    Dim newItem As ToolStripItem = SIDitem.DropDownItems.Add(singleNavPoint.UIName)
                    newItem.Tag = singleNavPoint.objectID
                    AddHandler newItem.Click, AddressOf mnuNavPointclicked
                Case clsAirNavPoint.enumAirNavPointType.VORTAC
                    Dim newItem As ToolStripItem = VORTACitem.DropDownItems.Add(singleNavPoint.UIName)
                    newItem.Tag = singleNavPoint.objectID
                    AddHandler newItem.Click, AddressOf mnuNavPointclicked
                Case Else
                    Dim newItem As ToolStripItem = Otheritem.DropDownItems.Add(singleNavPoint.UIName)
                    newItem.Tag = singleNavPoint.objectID
                    AddHandler newItem.Click, AddressOf mnuNavPointclicked
            End Select
        Next

        'STARs
        For Each singleRunway As clsRunWay In Me.Game.AirPort.runWays
            If singleRunway.STARs.Count > 0 Then
                Dim runwayItem As ToolStripMenuItem = Me.cmsAppDepSTAR.Items.Add(singleRunway.name)
                For Each singleStar As clsAirPath In singleRunway.STARs
                    Dim newItem As ToolStripItem = runwayItem.DropDownItems.Add(singleStar.name)
                    AddHandler newItem.Click, AddressOf mnuSTARclicked
                Next
            End If
        Next

        'enter STAR via
        For Each singleRunway As clsRunWay In Me.Game.AirPort.runWays
            If singleRunway.STARs.Count > 0 Then
                Dim runwayItem As ToolStripMenuItem = Me.cmsAppDepSTARvia.Items.Add(singleRunway.name)
                For Each singleStar As clsAirPath In singleRunway.STARs
                    Dim subStarItem As ToolStripMenuItem = runwayItem.DropDownItems.Add(singleStar.name)
                    'find and list all points
                    For Each singleNavStep As clsAStarEngine.structPathStep In singleStar.path
                        Dim singleNavPoint As clsAirNavPoint = singleNavStep.nextWayPoint
                        Dim newItem As ToolStripItem = subStarItem.DropDownItems.Add(singleNavPoint.UIName)
                        Dim newTuple As Tuple(Of String, String)
                        newTuple = Tuple.Create(singleStar.name, singleNavPoint.objectID)
                        newItem.Tag = newTuple
                        AddHandler newItem.Click, AddressOf mnuStarViaClicked
                    Next
                Next
            End If
        Next

        'SIDs
        For Each singleRunway As clsRunWay In Me.Game.AirPort.runWays
            If singleRunway.SIDs.Count > 0 Then
                Dim runwayItem As ToolStripMenuItem = Me.cmsAppDepSID.Items.Add(singleRunway.name)
                For Each singleSID As clsAirPath In singleRunway.SIDs
                    Dim newItem As ToolStripItem = runwayItem.DropDownItems.Add(singleSID.name)
                    AddHandler newItem.Click, AddressOf mnuSIDclicked
                Next
            End If
        Next

        'runways 
        Dim tagRunwayAvailableForLanding As New List(Of clsRunWay)
        For Each singleRunway As clsRunWay In Me.Game.AirPort.runWays
            If singleRunway.canHandleArrivals Then
                'for AppDep
                Dim newAppDepItem As ToolStripItem = Me.cmsAppDepRunways.Items.Add(singleRunway.arrivalPoint.UIName)
                newAppDepItem.Tag = singleRunway.arrivalPoint.name
                AddHandler newAppDepItem.Click, AddressOf mnuArrDepTowerRunwayclicked

                'for Tower
                Dim newTowerItem As ToolStripItem = Me.cmsTowerRunways.Items.Add(singleRunway.arrivalPoint.UIName)
                newTowerItem.Tag = singleRunway.arrivalPoint.name
                AddHandler newTowerItem.Click, AddressOf mnuArrDepTowerRunwayclicked

                'register runway as possible ruway for landing
                tagRunwayAvailableForLanding.Add(singleRunway)              '!!! needed?

            End If


        Next

        Dim tagRunwayAvailableForTakeOff As New List(Of clsRunWay)
        For Each singleRunway As clsRunWay In Me.Game.AirPort.runWays
            If singleRunway.canHandleArrivals Then
                Me.lstTowerOpenedRunwaysArrival.Items.Add(singleRunway.arrivalPoint.UIName)
            End If
            If singleRunway.canHandleDepartures Then
                Me.lstTowerOpenedRunwaysDeparture.Items.Add(singleRunway.name)
                tagRunwayAvailableForTakeOff.Add(singleRunway)
            End If
        Next
        Me.lstTowerOpenedRunwaysArrival.Tag = tagRunwayAvailableForLanding
        Me.lstTowerOpenedRunwaysDeparture.Tag = tagRunwayAvailableForTakeOff

        Me.listenToRunwayArrivalUpdates()                                                      'update lists of opened runways
        Me.listenToRunwayDepartureUpdates()                                                      'update lists of opened runways

        Me.lstGame.pnlStripes.Width += SystemInformation.VerticalScrollBarWidth
        Me.lstGround.pnlStripes.Width += SystemInformation.VerticalScrollBarWidth
        Me.lstTower.pnlStripes.Width += SystemInformation.VerticalScrollBarWidth
        Me.lstAppDep.pnlStripes.Width += SystemInformation.VerticalScrollBarWidth

        Me.updateButtonsEnabledGround()
        Me.updateButtonsEnabledTower()
        Me.updateButtonsEnabledAppDep()
        Me.updateButtonsEnabledSpecial()

        If Me.Game.isServer Then
            Me.trkMaxPlanes.Value = Me.Game.maxPlanes
            Me.lblMaxPlanes.Text = "max planes: " & Me.Game.maxPlanes

            Me.dtpSpawnUntil.Value = Me.Game.allowSpawnUntil
            Me.dtpEndGateUntil.Value = Me.Game.allowEndGateUntil
        End If

        'windrose
        Me.cltWindRose.loadAirport(Me.Game.AirPort)

        Me.pagGame.Show()
    End Sub

    Friend Sub Tick(ByVal timespan As TimeSpan)
        Me.lstGame.rePaint()
        Me.lstGround.rePaint()
        Me.lstTower.rePaint()
        Me.lstAppDep.rePaint()
        Me.updateButtonsText()
    End Sub

    Private Sub updateButtonsText()
        'updates the button availability based on the selected plane's state
        If Not Me.Game.selectedPlane Is Nothing Then

            Dim selectedPlane As clsPlane = Me.Game.selectedPlane

            'label for takeoff and lineup
            If Not selectedPlane.tower_LineUpApproved Then
                Me.cmdTowerLineUpAndWait.Text = "line up and wait | ➡"
            Else
                Me.cmdTowerLineUpAndWait.Text = "cancel line up and wait | ➡🚫"
            End If
            If Not selectedPlane.tower_takeOffApproved Then
                Me.cmdTowerTakeOff.Text = "take off | ↗"
            Else
                Me.cmdTowerTakeOff.Text = "cancel take off | ↗🚫"

            End If

            If Not (selectedPlane.tower_LineUpApproved And selectedPlane.tower_takeOffApproved) Then
                Me.cmdTowerLineUpandTakeOff.Text = "line up and take off | ➡↗"
            Else
                Me.cmdTowerLineUpandTakeOff.Text = "cancel line up and take off | ➡↗🚫"
            End If

            'select labeling for selected runway
            If Not selectedPlane.tower_assignedLandingPoint Is Nothing Then
                If selectedPlane.tower_cleardToLand Then Me.cmdTowerCleardToLand.Text = "Go Around | 🔂" Else Me.cmdTowerCleardToLand.Text = "Cleared To Land on " & vbNewLine & selectedPlane.tower_assignedLandingPoint.name & " | ♈✔"
            Else
                Me.cmdTowerCleardToLand.Text = " - "
            End If

            'game success data
            Me.lblCrashedPlanes.Text = "crashed planes: " & Me.Game.crashedPlanes
            Me.lblLandedPlanes.Text = "successful landings: " & Me.Game.successfulLandings
            Me.lblGatedPlanes.Text = "successful arrivals: " & Me.Game.successfulGated
            Me.lblGatedAtTerminal.Text = "successful arrivals at right gate: " & Me.Game.successfulArrival
            Me.lblTakenOffPlanes.Text = "successful take-offs: " & Me.Game.successfulTakeOffs
            Me.lblDepartedPlanes.Text = "successful departures: " & Me.Game.successfulDeparted

            'STAR


        End If

    End Sub

    Private Sub updateButtonsEnabledGround()
        'updates the button availability based on the selected plane's state
        Me.cmdGroundTaxiTo.Enabled = False
        Me.cmdGroundContactTower.Enabled = False
        Me.cmdGroundContinueTaxi.Enabled = False
        Me.cmdGroundHold.Enabled = False
        Me.cmdGroundPushbackApproved.Enabled = False

        If Not Me.Game.selectedPlane Is Nothing AndAlso Me.Game.selectedPlane.frequency = clsPlane.enumFrequency.ground Then

            Dim selectedPlane As clsPlane = Me.Game.selectedPlane

            If Not selectedPlane.ground_goalWayPoint Is Nothing Then
                Me.lblGroundTaxiTo.Text = "current: " & selectedPlane.ground_goalWayPoint.UIName
            Else
                Me.lblGroundTaxiTo.Text = "current: -"
            End If

            're-enable based on plane state
            Me.cmdGroundTaxiTo.Enabled =
                selectedPlane.currentState = clsPlane.enumPlaneState.ground_holdingPosition Or
                selectedPlane.currentState = clsPlane.enumPlaneState.ground_inTaxi Or
                selectedPlane.currentState = clsPlane.enumPlaneState.ground_breaking Or
                selectedPlane.currentState = clsPlane.enumPlaneState.tower_inTouchDown Or
                selectedPlane.currentState = clsPlane.enumPlaneState.ground_awaitingPushback Or
                selectedPlane.currentState = clsPlane.enumPlaneState.ground_inPushback

            Me.cmdGroundContactTower.Enabled = True

            Me.cmdGroundContinueTaxi.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.ground_holdingPosition
            Me.cmdGroundHold.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.ground_inTaxi
            Me.cmdGroundPushbackApproved.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.ground_awaitingPushback
        End If
    End Sub

    Private Sub updateButtonsEnabledTower()
        'updates the button availability based on the selected plane's state

        Me.cmdTowerExpectRunway.Enabled = False
        Me.cmdTowerAdjustSpeed.Enabled = False
        Me.cmdTowerCleardToLand.Enabled = False
        Me.cmdTowerContinueTaxi.Enabled = False
        Me.cmdTowerHeading.Enabled = False
        Me.cmdTowerHold.Enabled = False
        Me.cmdTowerLineUpandTakeOff.Enabled = False
        Me.cmdTowerLineUpAndWait.Enabled = False
        Me.cmdTowerEnterFinal.Enabled = False
        Me.cmdTowerMakeShortApproach.Enabled = False
        Me.cmdTowerTakeOff.Enabled = False
        Me.trkTowerHeading.Enabled = False
        Me.trkTowerSpeed.Enabled = False
        Me.txtTowerHeading.Enabled = False
        Me.trkTowerAltitude.Enabled = False
        Me.nudTowerAltitude.Enabled = False
        Me.cmdTowerAltitude.Enabled = False
        Me.cmdTowerContactApproach.Enabled = False
        Me.cmdTowerContactDeparture.Enabled = False
        Me.cmdTowerContactGround.Enabled = False
        Me.cmdTowerExitVia.Enabled = False
        Me.lstTowerOpenedRunwaysArrival.Enabled = True
        Me.lstTowerOpenedRunwaysDeparture.Enabled = True

        If Not Me.Game.selectedPlane Is Nothing AndAlso Me.Game.selectedPlane.frequency = clsPlane.enumFrequency.tower Then

            Dim selectedPlane As clsPlane = Me.Game.selectedPlane

            'text for runway
            If Not selectedPlane.tower_assignedLandingPoint Is Nothing Then
                Me.lblTowerCurrentRunway.Text = "current: " & selectedPlane.tower_assignedLandingPoint.name
            Else
                Me.lblTowerCurrentRunway.Text = "current: - "
            End If

            'fill exitList and select right exitnode
            Me.cmsTowerExitVia.Items.Clear()
            'add option to exit next
            Dim newNEXTItem As ToolStripItem = Me.cmsTowerExitVia.Items.Add("NEXT")
            newNEXTItem.Tag = Nothing
            AddHandler newNEXTItem.Click, AddressOf mnuTowerExitViaClicked

            'add all points that have a UI name
            If Not selectedPlane.tower_assignedLandingPoint Is Nothing Then
                If Not selectedPlane.tower_assignedLandingPoint Is Nothing Then
                    For Each singleExitSection As clsAStarEngine.structPathStep In selectedPlane.tower_assignedLandingPoint.getLandingPath
                        If Not singleExitSection.nextWayPoint Is selectedPlane.tower_assignedLandingPoint Then
                            If singleExitSection.nextWayPoint.UIName <> "" Then
                                Dim newItem As ToolStripItem = Me.cmsTowerExitVia.Items.Add(singleExitSection.nextWayPoint.UIName)
                                newItem.Tag = singleExitSection.nextWayPoint
                                AddHandler newItem.Click, AddressOf mnuTowerExitViaClicked
                            End If
                        End If
                    Next
                End If

                'write current exit node to label
                If Not selectedPlane.tower_assignedExitPointID = "" Then
                    'Me.lblTowerExitVia.Text = "exiting via " & selectedPlane.tower_assignedExitPointID
                    Dim exitPointName As String = selectedPlane.tower_assignedLandingPoint.getLandingPath.Find(Function(p As clsAStarEngine.structPathStep) p.nextWayPoint.objectID = selectedPlane.tower_assignedExitPointID).nextWayPoint.UIName
                    Me.lblTowerExitVia.Text = "exiting via " & exitPointName
                Else
                    Me.lblTowerExitVia.Text = "exiting via NEXT"
                End If
            Else
                Me.lblTowerExitVia.Text = "exiting via NEXT"
            End If

            'select heading in heading trackbar
            Dim targetDirection As Integer = CInt(selectedPlane.target_direction)
            If targetDirection = 0 Then targetDirection = 360

            Me.trkTowerHeading.Value = targetDirection
            Me.txtTowerHeading.Text = targetDirection

            'select altitude
            Me.trkTowerAltitude.Minimum = 0
            Me.trkTowerAltitude.Maximum = 100000
            Me.trkTowerAltitude.Value = selectedPlane.target_altitude.feet
            Me.trkTowerAltitude.Minimum = 0
            Me.trkTowerAltitude.Maximum = selectedPlane.modelInfo.air_AltMax.feet

            Me.nudTowerAltitude.Minimum = 0
            Me.nudTowerAltitude.Maximum = 100000
            Me.nudTowerAltitude.Value = selectedPlane.target_altitude.feet
            Me.nudTowerAltitude.Minimum = 0
            Me.nudTowerAltitude.Maximum = selectedPlane.modelInfo.air_AltMax.feet

            Me.nudTowerAltitude.Value = Me.trkTowerAltitude.Value
            Me.cmdTowerAltitude.Text = "Confirm Altitude | ⏫⏬"


            'select speed
            Me.trkTowerSpeed.Minimum = 0
            Me.trkTowerSpeed.Maximum = 1000
            Me.trkTowerSpeed.Value = selectedPlane.target_speed.knots
            Me.trkTowerSpeed.Minimum = selectedPlane.modelInfo.air_Vstall.knots
            Me.trkTowerSpeed.Maximum = selectedPlane.modelInfo.air_Vmo.knots

            '   If selectedPlane.target_speed.knots >= Me.trkTowerSpeed.Minimum And selectedPlane.target_speed.knots <= Me.trkTowerSpeed.Maximum Then
            Me.cmdTowerAdjustSpeed.Text = "Confirm Speed: " & selectedPlane.target_speed.knots & " knots."
            '  End If

            Me.cmdTowerExpectRunway.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.tower_freeFlight
            Me.cmdTowerAdjustSpeed.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.tower_freeFlight Or selectedPlane.currentState = clsPlane.enumPlaneState.tower_FinalApproach
            Me.cmdTowerCleardToLand.Enabled =
                (selectedPlane.currentState = clsPlane.enumPlaneState.tower_freeFlight Or
                selectedPlane.currentState = clsPlane.enumPlaneState.tower_FinalApproach) And
                (Not selectedPlane.tower_assignedLandingPoint Is Nothing)
            Me.cmdTowerContinueTaxi.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.ground_holdingPosition
            Me.cmdTowerHeading.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.tower_freeFlight Or selectedPlane.currentState = clsPlane.enumPlaneState.tower_FinalApproach
            Me.cmdTowerHold.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.ground_inTaxi
            Me.cmdTowerLineUpandTakeOff.Enabled =
                selectedPlane.currentState = clsPlane.enumPlaneState.ground_breaking Or
                selectedPlane.currentState = clsPlane.enumPlaneState.ground_holdingPosition Or
                selectedPlane.currentState = clsPlane.enumPlaneState.ground_inTaxi Or
                selectedPlane.currentState = clsPlane.enumPlaneState.tower_inLineUp

            Me.cmdTowerLineUpAndWait.Enabled =
                selectedPlane.currentState = clsPlane.enumPlaneState.ground_breaking Or
                selectedPlane.currentState = clsPlane.enumPlaneState.ground_holdingPosition Or
                selectedPlane.currentState = clsPlane.enumPlaneState.ground_inTaxi Or
                selectedPlane.currentState = clsPlane.enumPlaneState.tower_inLineUp

            Me.cmdTowerEnterFinal.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.tower_freeFlight
            Me.cmdTowerMakeShortApproach.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.tower_freeFlight Or selectedPlane.currentState = clsPlane.enumPlaneState.tower_FinalApproach
            Me.cmdTowerTakeOff.Enabled =
                selectedPlane.currentState = clsPlane.enumPlaneState.tower_inLineUp Or
                 selectedPlane.currentState = clsPlane.enumPlaneState.tower_linedupAndWaiting

            Me.trkTowerHeading.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.tower_freeFlight Or selectedPlane.currentState = clsPlane.enumPlaneState.tower_FinalApproach
            Me.trkTowerSpeed.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.tower_freeFlight Or selectedPlane.currentState = clsPlane.enumPlaneState.tower_FinalApproach
            Me.txtTowerHeading.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.tower_freeFlight Or selectedPlane.currentState = clsPlane.enumPlaneState.tower_FinalApproach

            Me.trkTowerAltitude.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.tower_freeFlight Or selectedPlane.currentState = clsPlane.enumPlaneState.tower_FinalApproach
            Me.nudTowerAltitude.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.tower_freeFlight Or selectedPlane.currentState = clsPlane.enumPlaneState.tower_FinalApproach
            Me.cmdTowerAltitude.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.tower_freeFlight Or selectedPlane.currentState = clsPlane.enumPlaneState.tower_FinalApproach

            Me.cmdTowerContactApproach.Enabled = True
            Me.cmdTowerContactDeparture.Enabled = True
            Me.cmdTowerContactGround.Enabled = True

            Me.cmdTowerExitVia.Enabled =
                selectedPlane.currentState = clsPlane.enumPlaneState.tower_enteringTouchDown Or
                selectedPlane.currentState = clsPlane.enumPlaneState.tower_inTouchDown Or
                (selectedPlane.isArriving And Not (selectedPlane.tower_assignedLandingPoint Is Nothing))

        End If


    End Sub

    Private Sub updateButtonsEnabledAppDep()
        'updates the button availability based on the selected plane's state
        Me.cmdAppDepEnterSID.Enabled = False
        Me.cmdAppDepEnterSTAR.Enabled = False
        Me.cmdAppDepEnterSTARVia.Enabled = False
        Me.cmdAppDepExpectRunway.Enabled = False
        Me.cmdAppDepHeadTo.Enabled = False
        Me.cmdAppDepAdjustSpeed.Enabled = False
        Me.cmdAppDepHeading.Enabled = False
        Me.cmdAppDepEnterFinal.Enabled = False
        Me.cmdAppDepMakeShortApproach.Enabled = False
        Me.lblAppDepHeading.Enabled = False
        Me.trkAppDepHeading.Enabled = False
        Me.trkAppDepSpeed.Enabled = False
        Me.txtAppDepHeading.Enabled = False
        Me.trkAppDepAltitude.Enabled = False
        Me.nudAppDepAltitude.Enabled = False
        Me.cmdAppDepAltitude.Enabled = False
        Me.cmdAppDepContactTower.Enabled = False

        If Not Me.Game.selectedPlane Is Nothing AndAlso
            (Me.Game.selectedPlane.frequency = clsPlane.enumFrequency.arrival Or
            Me.Game.selectedPlane.frequency = clsPlane.enumFrequency.departure Or
            Me.Game.selectedPlane.frequency = clsPlane.enumFrequency.appdep Or
            Me.Game.selectedPlane.frequency = clsPlane.enumFrequency.tracon) Then


            Dim selectedPlane As clsPlane = Me.Game.selectedPlane

            'text for waypoint
            If Not selectedPlane.air_nextWayPoint Is Nothing Then
                Me.lblArrDepCurrentNavPoint.Text = "next: " & selectedPlane.air_nextWayPoint.name
            Else
                Me.lblArrDepCurrentNavPoint.Text = "next: - "
            End If

            'text for STAR
            If selectedPlane.isArriving And Not selectedPlane.air_currentAirPathName Is Nothing Then
                Me.lblArrDepCurrentStar.Text = "current: " & selectedPlane.air_currentAirPathName
            Else
                Me.lblArrDepCurrentStar.Text = "current: - "
            End If

            'text for SID
            If selectedPlane.isDeparting And Not selectedPlane.air_currentAirPathName Is Nothing Then
                Me.lblArrDepCurrentSID.Text = "current: " & selectedPlane.air_currentAirPathName
            Else
                Me.lblArrDepCurrentSID.Text = "current: - "
            End If

            'text for runway
            If Not selectedPlane.tower_assignedLandingPoint Is Nothing Then
                Me.lblArrDepCurrentRunway.Text = "current: " & selectedPlane.tower_assignedLandingPoint.name
            Else
                Me.lblArrDepCurrentRunway.Text = "current: - "
            End If

            'select heading in heading trackbar
            Dim targetDirection As Integer = CInt(selectedPlane.target_direction)
            If targetDirection = 0 Then targetDirection = 360

            Me.trkAppDepHeading.Value = targetDirection
            Me.txtAppDepHeading.Text = targetDirection

            'handle altitude
            Me.trkAppDepAltitude.Minimum = 0
            Me.trkAppDepAltitude.Maximum = 100000
            Me.trkAppDepAltitude.Value = selectedPlane.target_altitude.feet
            Me.trkAppDepAltitude.Minimum = 0
            Me.trkAppDepAltitude.Maximum = selectedPlane.modelInfo.air_AltMax.feet

            Me.nudAppDepAltitude.Minimum = 0
            Me.nudAppDepAltitude.Maximum = 100000
            Me.nudAppDepAltitude.Value = selectedPlane.target_altitude.feet
            Me.nudAppDepAltitude.Minimum = 0
            Me.nudAppDepAltitude.Maximum = selectedPlane.modelInfo.air_AltMax.feet

            Me.nudAppDepAltitude.Value = Me.trkAppDepAltitude.Value
            Me.cmdAppDepAltitude.Text = "Confirm Altitude"

            'select speed
            Me.trkAppDepSpeed.Minimum = 0
            Me.trkAppDepSpeed.Maximum = 1000
            Me.trkAppDepSpeed.Value = selectedPlane.target_speed.knots
            Me.trkAppDepSpeed.Minimum = selectedPlane.modelInfo.air_Vstall.knots
            Me.trkAppDepSpeed.Maximum = selectedPlane.modelInfo.air_Vmo.knots

            If selectedPlane.target_speed.knots >= Me.trkAppDepSpeed.Minimum And selectedPlane.target_speed.knots <= Me.trkAppDepSpeed.Maximum Then
                Me.cmdAppDepAdjustSpeed.Text = "Confirm Speed: " & selectedPlane.target_speed.knots & " knots."
            End If

            Me.cmdAppDepEnterSID.Enabled =
                (selectedPlane.currentState = clsPlane.enumPlaneState.tower_freeFlight Or selectedPlane.currentState = clsPlane.enumPlaneState.tower_FinalApproach) And
                selectedPlane.isDeparting
            Me.cmdAppDepEnterSTAR.Enabled = selectedPlane.currentState =
                (clsPlane.enumPlaneState.tower_freeFlight Or selectedPlane.currentState = clsPlane.enumPlaneState.tower_FinalApproach) And
                selectedPlane.isArriving
            Me.cmdAppDepEnterSTARVia.Enabled = selectedPlane.currentState =
                (clsPlane.enumPlaneState.tower_freeFlight Or selectedPlane.currentState = clsPlane.enumPlaneState.tower_FinalApproach) And
                selectedPlane.isArriving
            Me.cmdAppDepExpectRunway.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.tower_freeFlight
            Me.cmdAppDepHeadTo.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.tower_freeFlight Or selectedPlane.currentState = clsPlane.enumPlaneState.tower_FinalApproach
            Me.cmdAppDepAdjustSpeed.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.tower_freeFlight Or selectedPlane.currentState = clsPlane.enumPlaneState.tower_FinalApproach
            Me.cmdAppDepHeading.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.tower_freeFlight Or selectedPlane.currentState = clsPlane.enumPlaneState.tower_FinalApproach
            Me.cmdAppDepEnterFinal.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.tower_freeFlight 'Or selectedPlane.currentState = clsPlane.enumPlaneState.tower_FinalApproach
            Me.cmdAppDepMakeShortApproach.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.tower_freeFlight Or selectedPlane.currentState = clsPlane.enumPlaneState.tower_FinalApproach
            Me.lblAppDepHeading.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.tower_freeFlight Or selectedPlane.currentState = clsPlane.enumPlaneState.tower_FinalApproach
            Me.trkAppDepHeading.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.tower_freeFlight Or selectedPlane.currentState = clsPlane.enumPlaneState.tower_FinalApproach
            Me.trkAppDepSpeed.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.tower_freeFlight Or selectedPlane.currentState = clsPlane.enumPlaneState.tower_FinalApproach
            Me.txtAppDepHeading.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.tower_freeFlight Or selectedPlane.currentState = clsPlane.enumPlaneState.tower_FinalApproach

            Me.trkAppDepAltitude.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.tower_freeFlight Or selectedPlane.currentState = clsPlane.enumPlaneState.tower_FinalApproach
            Me.nudAppDepAltitude.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.tower_freeFlight Or selectedPlane.currentState = clsPlane.enumPlaneState.tower_FinalApproach
            Me.cmdAppDepAltitude.Enabled = selectedPlane.currentState = clsPlane.enumPlaneState.tower_freeFlight Or selectedPlane.currentState = clsPlane.enumPlaneState.tower_FinalApproach

            Me.cmdAppDepContactTower.Enabled = True
        End If
    End Sub

    Private Sub updateButtonsEnabledSpecial()
        Me.cmdSpecialDespawn.Enabled = False
        Me.cmdSpecialSpawn.Enabled = False
        Me.trkMaxPlanes.Enabled = False
        Me.dtpEndGateUntil.Enabled = False
        Me.dtpSpawnUntil.Enabled = False
        If Not Me.Game.isclient Then
            Me.cmdSpecialDespawn.Enabled = (Not (Me.Game.selectedPlane Is Nothing))
            Me.cmdSpecialSpawn.Enabled = True
            Me.trkMaxPlanes.Enabled = True
            Me.dtpEndGateUntil.Enabled = True
            Me.dtpSpawnUntil.Enabled = True
        End If
    End Sub

    Private Sub despawnPlane(ByRef plane As clsPlane) Handles Game.despawnedPlane
        Me.lstGame.removeStripe(plane)
        Me.lstGround.removeStripe(plane)
        Me.lstTower.removeStripe(plane)
        Me.lstAppDep.removeStripe(plane)
    End Sub
    Private Sub cmdContinueTaxi_Click(sender As Object, e As EventArgs) Handles cmdGroundContinueTaxi.Click
        Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
            .plane = Me.Game.selectedPlane.callsign,
            .command = clsPlane.enumCommands.continueTaxi
        }
        Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)
    End Sub

    Private Sub cmdContinueTaxi2_Click(sender As Object, e As EventArgs) Handles cmdTowerContinueTaxi.Click
        Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
            .plane = Me.Game.selectedPlane.callsign,
            .command = clsPlane.enumCommands.continueTaxi
        }
        Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)
    End Sub

    Private Sub cmdHold_Click(sender As Object, e As EventArgs) Handles cmdGroundHold.Click
        Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
            .plane = Me.Game.selectedPlane.callsign,
            .command = clsPlane.enumCommands.holdPosition
        }
        Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)
    End Sub

    Private Sub cmdHold2_Click(sender As Object, e As EventArgs) Handles cmdTowerHold.Click
        Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
            .plane = Me.Game.selectedPlane.callsign,
            .command = clsPlane.enumCommands.holdPosition
        }
        Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)

    End Sub

    Private Sub cmdPushbackApproved_Click(sender As Object, e As EventArgs) Handles cmdGroundPushbackApproved.Click
        Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
            .plane = Me.Game.selectedPlane.callsign,
            .command = clsPlane.enumCommands.pushbackApproved
        }
        Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)

    End Sub

    Private Sub cmdLineUpAndWait_Click(sender As Object, e As EventArgs) Handles cmdTowerLineUpAndWait.Click
        'if take off not yet approved,...
        If Not Me.Game.selectedPlane.tower_LineUpApproved Then
            '...approve it
            Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
            .plane = Me.Game.selectedPlane.callsign,
            .command = clsPlane.enumCommands.lineUpandWait
            }
        Else
            '...else cancel it
            Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
           .plane = Me.Game.selectedPlane.callsign,
           .command = clsPlane.enumCommands.cancelLineupAndTakeoff
           }
        End If

        Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)

    End Sub

    Private Sub cmdTakeOff_Click(sender As Object, e As EventArgs) Handles cmdTowerTakeOff.Click
        'if take off not yet approved,...
        If Not Me.Game.selectedPlane.tower_takeOffApproved Then
            '... approve it
            Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
                .plane = Me.Game.selectedPlane.callsign,
                .command = clsPlane.enumCommands.clearForTakeOff
            }
        Else
            '...else cancel takeoff
            Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
                .plane = Me.Game.selectedPlane.callsign,
                .command = clsPlane.enumCommands.cancelTakeOff
            }
        End If

        Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)

    End Sub

    Private Sub cmdLineUpandTakeOff_Click(sender As Object, e As EventArgs) Handles cmdTowerLineUpandTakeOff.Click
        'if not both approved,...
        If Not (Me.Game.selectedPlane.tower_LineUpApproved And Me.Game.selectedPlane.tower_takeOffApproved) Then
            '...allow bothh
            Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
            .plane = Me.Game.selectedPlane.callsign,
            .command = clsPlane.enumCommands.lineUpandTakeOff
            }
        Else
            '...else, stop both
            Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
            .plane = Me.Game.selectedPlane.callsign,
            .command = clsPlane.enumCommands.cancelLineupAndTakeoff
            }
        End If

        Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)
    End Sub

    Private Sub cmdDespawn_Click(sender As Object, e As EventArgs) Handles cmdSpecialDespawn.Click
        If Not Me.lstGame.selectedStripe Is Nothing Then Me.Game.despawn(Me.lstGame.selectedStripe.plane)
    End Sub

    Private Sub cmdSpawn_Click(sender As Object, e As EventArgs) Handles cmdSpecialSpawn.Click
        Me.Game.triggerSpawn()
    End Sub

    Friend Sub listenPlaneSpawned(ByRef plane As clsPlane) Handles Game.spawnedPlane
        Me.lstGame.addStripe(plane)
        Select Case plane.frequency
            Case clsPlane.enumFrequency.ground
                Me.lstGround.addStripe(plane)
                Me.updateButtonsEnabledGround()
            Case clsPlane.enumFrequency.tower
                Me.lstTower.addStripe(plane)
                Me.updateButtonsEnabledTower()
            Case clsPlane.enumFrequency.arrival, clsPlane.enumFrequency.departure, clsPlane.enumFrequency.appdep, clsPlane.enumFrequency.tracon
                Me.lstAppDep.addStripe(plane)
                Me.updateButtonsEnabledAppDep()
        End Select
        Me.updateButtonsText()
    End Sub

    ''' <summary>
    ''' check if the status of the selected plane has changed and if so update the GUI
    ''' </summary>
    Friend Sub listenPlaneStatusChanged(ByRef plane As clsPlane) Handles Game.selectedPlaneStatusChanged
        Me.updateButtonsEnabledGround()
        Me.updateButtonsEnabledTower()
        Me.updateButtonsEnabledAppDep()
        Me.updateButtonsText()
        'If Not Me.lstGround.selectedStripe Is Nothing AndAlso plane Is Me.lstGround.selectedStripe.plane Then
        '    Me.updateButtonsEnabledGround()
        '    Me.updateButtonsText()
        'End If
        'If Not Me.lstTower.selectedStripe Is Nothing AndAlso plane Is Me.lstTower.selectedStripe.plane Then
        '    Me.updateButtonsEnabledTower()
        '    Me.updateButtonsText()
        'End If
        'If Not Me.lstTracon.selectedStripe Is Nothing AndAlso plane Is Me.lstTracon.selectedStripe.plane Then
        '    Me.updateButtonsEnabledTracon()
        '    Me.updateButtonsText()
        'End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="plane"></param>
    Friend Sub listenPlaneFrequencyChanged(ByRef plane As clsPlane) Handles Game.planeFrequencyChanged
        'remove plane from all lists
        Me.lstGround.removeStripe(plane)
        Me.lstTower.removeStripe(plane)
        Me.lstAppDep.removeStripe(plane)

        'add plane to correct list
        Select Case plane.frequency
            Case clsPlane.enumFrequency.ground
                Me.lstGround.addStripe(plane)
            Case clsPlane.enumFrequency.tower
                Me.lstTower.addStripe(plane)
            Case clsPlane.enumFrequency.arrival, clsPlane.enumFrequency.departure, clsPlane.enumFrequency.appdep, clsPlane.enumFrequency.tracon
                Me.lstAppDep.addStripe(plane)
        End Select
    End Sub

    Friend Sub messageReceived(ByRef frequency As clsPlane.enumFrequency, ByRef message As String) Handles Game.radioMessage
        Select Case frequency
            Case clsPlane.enumFrequency.appdep, clsPlane.enumFrequency.arrival, clsPlane.enumFrequency.departure
                Me.txtAppDepMessages.Text = DateTime.Now.ToString("hh:MM:ss") & " - " & message & vbNewLine & Me.txtAppDepMessages.Text
            Case clsPlane.enumFrequency.ground
                Me.txtGroundMessages.Text = DateTime.Now.ToString("hh:MM:ss") & " - " & message & vbNewLine & Me.txtGroundMessages.Text
            Case clsPlane.enumFrequency.tower
                Me.txtTowerMessages.Text = DateTime.Now.ToString("hh:MM:ss") & " - " & message & vbNewLine & Me.txtTowerMessages.Text
            Case clsPlane.enumFrequency.radioOff
                'do nothing
            Case clsPlane.enumFrequency.undefined, clsPlane.enumFrequency.tracon
                Me.txtGroundMessages.Text = DateTime.Now.ToString(">>hh:MM:ss") & " - " & message & vbNewLine & Me.txtGroundMessages.Text & "<<"
                Me.txtTowerMessages.Text = DateTime.Now.ToString(">>hh:MM:ss") & " - " & message & vbNewLine & Me.txtGroundMessages.Text & "<<"
                Me.txtAppDepMessages.Text = DateTime.Now.ToString(">>hh:MM:ss") & " - " & message & vbNewLine & Me.txtGroundMessages.Text & "<<"
            Case Else

        End Select

    End Sub

    Private Sub txtDirection_TextChanged(sender As TextBox, e As EventArgs) Handles txtAppDepHeading.TextChanged

        Dim deg As Integer = CInt(0 & sender.Text)
        If deg > 0 And deg <= 360 Then
            Me.trkAppDepHeading.Value = deg
            Me.trkTowerHeading.Value = deg
        End If

    End Sub

    Private Sub txtHeading2_TextChanged(sender As Object, e As EventArgs) Handles txtTowerHeading.TextChanged
        Dim deg As Integer = CInt(0 & sender.Text)
        If deg > 0 And deg <= 360 Then
            Me.trkTowerHeading.Value = deg
        End If

    End Sub

    Private Sub trkDirection_Scroll(sender As Object, e As EventArgs) Handles trkAppDepHeading.Scroll
        Me.txtAppDepHeading.Text = sender.Value
        Me.txtTowerHeading.Text = sender.Value
    End Sub

    Private Sub cmdHeading_Click(sender As Object, e As EventArgs) Handles cmdAppDepHeading.Click
        Dim deg As Integer = CInt(trkAppDepHeading.Value)
        If deg > 0 And deg <= 360 Then
            Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
                .plane = Me.Game.selectedPlane.callsign,
                .command = clsPlane.enumCommands.airHeadToDirection,
                .airDirectionCommandParameter = deg
            }
            Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)

            Me.trkAppDepHeading.Value = deg
            Me.txtAppDepHeading.Text = deg Mod 360
        End If
    End Sub

    Private Sub cmdHeading2_Click(sender As Object, e As EventArgs) Handles cmdTowerHeading.Click
        Dim deg As Integer = CInt(trkTowerHeading.Value)
        If deg > 0 And deg <= 360 Then
            Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
                .plane = Me.Game.selectedPlane.callsign,
                .command = clsPlane.enumCommands.airHeadToDirection,
                .airDirectionCommandParameter = deg
            }
            Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)

            Me.trkTowerHeading.Value = deg
            Me.txtTowerHeading.Text = deg Mod 360
        End If
    End Sub

    Private Sub cmdAdjustSpeed_Click(sender As Object, e As EventArgs) Handles cmdAppDepAdjustSpeed.Click
        Dim speed As Double = Me.trkAppDepSpeed.Value
        Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
            .plane = Me.Game.selectedPlane.callsign,
            .command = clsPlane.enumCommands.airAdjustSpeed,
            .airSpeedCommandParameter = Me.trkAppDepSpeed.Value
        }
        Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)

    End Sub

    Private Sub cmdAdjustSpeed2_Click(sender As Object, e As EventArgs) Handles cmdTowerAdjustSpeed.Click
        Dim speed As Double = Me.trkAppDepSpeed.Value
        Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
            .plane = Me.Game.selectedPlane.callsign,
            .command = clsPlane.enumCommands.airAdjustSpeed,
           .airSpeedCommandParameter = Me.trkTowerSpeed.Value
        }
        Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)

    End Sub

    Private Sub trkSpeed_Scroll(sender As Object, e As EventArgs) Handles trkAppDepSpeed.Scroll
        Me.cmdAppDepAdjustSpeed.Text = "Adjust Speed to " & Me.trkAppDepSpeed.Value & " knots"
    End Sub

    Private Sub trkSpeed2_Scroll(sender As Object, e As EventArgs) Handles trkTowerSpeed.Scroll
        Me.cmdTowerAdjustSpeed.Text = "Adjust Speed to " & Me.trkTowerSpeed.Value & " knots"
    End Sub

    Private Sub cmdCleardToLand_Click(sender As Object, e As EventArgs) Handles cmdTowerCleardToLand.Click
        If Me.Game.selectedPlane.tower_cleardToLand Then
            Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
                .plane = Me.Game.selectedPlane.callsign,
                .command = clsPlane.enumCommands.airGoAround
            }
            Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)

        Else
            Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
            .plane = Me.Game.selectedPlane.callsign,
            .command = clsPlane.enumCommands.airClearedForLanding
            }
            Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)

        End If

    End Sub

    Private Sub cmdMakeShortApproach_Click(sender As Object, e As EventArgs) Handles cmdAppDepMakeShortApproach.Click
        Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
            .plane = Me.Game.selectedPlane.callsign,
            .command = clsPlane.enumCommands.airMakeShortApproach
        }
        Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)

        Me.updateButtonsText()
    End Sub

    Private Sub cmdMakeShortApproach2_Click(sender As Object, e As EventArgs) Handles cmdTowerMakeShortApproach.Click
        Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
        .plane = Me.Game.selectedPlane.callsign,
        .command = clsPlane.enumCommands.airMakeShortApproach
        }
        Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)

        Me.updateButtonsText()
    End Sub

    Private Sub cmdContactGround_Click(sender As Object, e As EventArgs) Handles cmdTowerContactGround.Click
        Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
        .plane = Me.Game.selectedPlane.callsign,
        .command = clsPlane.enumCommands.contactGround
        }
        Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)

    End Sub

    Private Sub cmdContactTower_Click(sender As Object, e As EventArgs) Handles cmdGroundContactTower.Click
        Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
            .plane = Me.Game.selectedPlane.callsign,
            .command = clsPlane.enumCommands.contactTower
        }
        Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)

    End Sub

    Private Sub cmdContactDeparture_Click(sender As Object, e As EventArgs) Handles cmdTowerContactDeparture.Click
        Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
        .plane = Me.Game.selectedPlane.callsign,
        .command = clsPlane.enumCommands.contactArrDep
        }
        Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)

    End Sub

    Private Sub cmdContactArrival_Click(sender As Object, e As EventArgs) Handles cmdTowerContactApproach.Click
        Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
            .plane = Me.Game.selectedPlane.callsign,
            .command = clsPlane.enumCommands.contactArrDep
        }
        Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)

    End Sub

    Private Sub cmdAppDepContactTower_Click(sender As Object, e As EventArgs) Handles cmdAppDepContactTower.Click
        Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
            .plane = Me.Game.selectedPlane.callsign,
            .command = clsPlane.enumCommands.contactTower
        }
        Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)

    End Sub

    Private Sub trkHeading2_Scroll(sender As Object, e As EventArgs) Handles trkTowerHeading.Scroll
        Me.txtTowerHeading.Text = sender.Value
    End Sub

    Private Sub tmrTick_Tick(sender As Timer, e As EventArgs) Handles tmrTick.Tick
        Me.Tick(New TimeSpan(sender.Interval))
    End Sub

    Private Sub tmrUpdateButtons_Tick(sender As Object, e As EventArgs)
        Me.updateButtonsText()
    End Sub

    Private Sub lstGround_stripeSelected(ByRef stripe As ctlStripe) Handles lstGround.stripeSelected
        Me.Game.selectedPlane = stripe.plane
        Me.lstGame.selectStripe(stripe.plane)
        Me.updateButtonsEnabledGround()
        Me.updateButtonsText()
    End Sub

    Private Sub lstTower_stripeSelected(ByRef stripe As ctlStripe) Handles lstTower.stripeSelected
        Me.Game.selectedPlane = stripe.plane
        Me.lstGame.selectStripe(stripe.plane)
        Me.updateButtonsEnabledTower()
        Me.updateButtonsText()
    End Sub

    Private Sub lstTracon_stripeSelected(ByRef stripe As ctlStripe) Handles lstAppDep.stripeSelected
        Me.Game.selectedPlane = stripe.plane
        Me.lstGame.selectStripe(stripe.plane)
        Me.updateButtonsEnabledAppDep()
        Me.updateButtonsText()
    End Sub

    Private Sub tabControls_SelectedIndexChanged(sender As TabControl, e As EventArgs) Handles tabControls.SelectedIndexChanged
        If sender.SelectedTab Is pagGame Then
            If Not Me.lstGame.selectedStripe Is Nothing Then
                Me.Game.selectedPlane = Me.lstGame.selectedStripe.plane
            Else
                Me.Game.selectedPlane = Nothing
            End If
            Me.updateButtonsEnabledSpecial()
        ElseIf sender.SelectedTab Is pagGround Then
            If Not Me.lstGround.selectedStripe Is Nothing Then
                Game.selectedPlane = Me.lstGround.selectedStripe.plane
                Me.lstGame.selectStripe(Me.lstGround.selectedStripe.plane)
            Else
                Me.Game.selectedPlane = Nothing
            End If
            Me.updateButtonsEnabledGround()
        ElseIf sender.SelectedTab Is pagTower Then
            If Not Me.lstTower.selectedStripe Is Nothing Then
                Game.selectedPlane = Me.lstTower.selectedStripe.plane
                Me.lstGame.selectStripe(Me.lstTower.selectedStripe.plane)
            Else
                Me.Game.selectedPlane = Nothing
            End If
            Me.updateButtonsEnabledTower()
        ElseIf sender.SelectedTab Is pagAppDep Then
            If Not Me.lstAppDep.selectedStripe Is Nothing Then
                Game.selectedPlane = Me.lstAppDep.selectedStripe.plane
                Me.lstGame.selectStripe(Me.lstAppDep.selectedStripe.plane)
            Else
                Me.Game.selectedPlane = Nothing
            End If
            Me.updateButtonsEnabledAppDep()
        End If
    End Sub

    Private Sub cmdTowerAltitude_Click(sender As Object, e As EventArgs) Handles cmdTowerAltitude.Click
        Dim altitude As Double = Me.trkTowerAltitude.Value

        Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
            .plane = Me.Game.selectedPlane.callsign,
            .command = clsPlane.enumCommands.airAdjustAltitude,
           .airAltitudeCommandParameter = Me.trkTowerAltitude.Value
        }
        Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)

    End Sub

    Private Sub cmdTraconAltitude_Click(sender As Object, e As EventArgs) Handles cmdAppDepAltitude.Click
        Dim altitude As Double = Me.trkAppDepAltitude.Value

        Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
            .plane = Me.Game.selectedPlane.callsign,
            .command = clsPlane.enumCommands.airAdjustAltitude,
           .airAltitudeCommandParameter = Me.trkAppDepAltitude.Value
        }
        Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)

    End Sub

    Private Sub nudTowerAltitude_ValueChanged(sender As NumericUpDown, e As EventArgs) Handles nudTowerAltitude.ValueChanged

        Me.nudTowerAltitude.Minimum = 0
        Me.nudTowerAltitude.Maximum = 100000
        Me.nudTowerAltitude.Value = sender.Value
        Me.nudTowerAltitude.Minimum = 0
        Me.nudTowerAltitude.Maximum = Me.Game.selectedPlane.modelInfo.air_AltMax.feet

        Me.cmdTowerAltitude.Text = "Confirm Altitude | ⏫⏬"

        Me.trkTowerAltitude.Value = sender.Value
        If sender.Value > Me.Game.selectedPlane.pos_Altitude.feet Then
            Me.cmdTowerAltitude.Text = "Climb " & Me.trkTowerAltitude.Value
        ElseIf sender.Value <> Me.Game.selectedPlane.pos_Altitude.feet Then
            Me.cmdTowerAltitude.Text = "Descend " & Me.trkTowerAltitude.Value
        Else
            Me.cmdTowerAltitude.Text = "Confirm " & Me.trkTowerAltitude.Value
        End If
    End Sub

    Private Sub nudTraconAltitude_ValueChanged(sender As NumericUpDown, e As EventArgs) Handles nudAppDepAltitude.ValueChanged

        Me.nudAppDepAltitude.Minimum = 0
        Me.nudAppDepAltitude.Maximum = 100000
        Me.nudAppDepAltitude.Value = sender.Value
        Me.nudAppDepAltitude.Minimum = 0
        Me.nudAppDepAltitude.Maximum = Me.Game.selectedPlane.modelInfo.air_AltMax.feet

        Me.cmdAppDepAltitude.Text = "Confirm Altitude | ⏫⏬"

        Me.trkAppDepAltitude.Value = sender.Value
        If sender.Value > Me.Game.selectedPlane.pos_Altitude.feet Then
            Me.cmdAppDepAltitude.Text = "Climb " & Me.trkAppDepAltitude.Value & " | ⏫"
        ElseIf sender.Value <> Me.Game.selectedPlane.pos_Altitude.feet Then
            Me.cmdAppDepAltitude.Text = "Descend " & Me.trkAppDepAltitude.Value & "  | ⏬"
        Else
            Me.cmdAppDepAltitude.Text = "Confirm " & Me.trkAppDepAltitude.Value & " | ⏫⏬"
        End If
    End Sub

    Private Sub lstGame_stripeSelected(ByRef stripe As ctlStripe) Handles lstGame.stripeSelected
        Me.Game.selectedPlane = stripe.plane
        '!!!select respective plane in the other tabs

        Me.lstGame.selectStripe(stripe.plane)
        Me.updateButtonsEnabledSpecial()
        Me.updateButtonsText()
    End Sub

    Private Sub trkTowerAltitude_Scroll(sender As Object, e As EventArgs) Handles trkTowerAltitude.Scroll
        Me.nudTowerAltitude.Value = sender.Value
        If sender.Value > Me.Game.selectedPlane.pos_Altitude.feet Then
            Me.cmdTowerAltitude.Text = "Climb " & Me.trkTowerAltitude.Value & " | ⏫"
        ElseIf sender.Value <> Me.Game.selectedPlane.pos_Altitude.feet Then
            Me.cmdTowerAltitude.Text = "Descend " & Me.trkTowerAltitude.Value & "  | ⏬"
        Else
            Me.cmdTowerAltitude.Text = "Confirm " & Me.trkTowerAltitude.Value & " | ⏫⏬"
        End If
    End Sub

    Private Sub trkTraconAltitude_Scroll(sender As Object, e As EventArgs) Handles trkAppDepAltitude.Scroll
        Me.nudAppDepAltitude.Value = sender.Value
        If sender.Value > Me.Game.selectedPlane.pos_Altitude.feet Then
            Me.cmdAppDepAltitude.Text = "Climb " & Me.trkAppDepAltitude.Value & " | ⏫"
        ElseIf sender.Value <> Me.Game.selectedPlane.pos_Altitude.feet Then
            Me.cmdAppDepAltitude.Text = "Descend " & Me.trkAppDepAltitude.Value & "  | ⏬"
        Else
            Me.cmdAppDepAltitude.Text = "Confirm " & Me.trkAppDepAltitude.Value & " | ⏫⏬"
        End If
    End Sub

    'listens if a runway has been opened or closed for arrival (usually by the server)
    Friend Sub listenToRunwayArrivalUpdates() Handles Game.availableRunwaysArrivalChanged
        Me.supressAvailableRunwayCheckListEvaluation = True
        Dim arrivalPointList As List(Of clsRunWay) = Me.lstTowerOpenedRunwaysArrival.Tag
        For C1 As Long = 0 To arrivalPointList.Count - 1
            'check checkboxes based on availability
            'color the "expect runways" menu items
            If arrivalPointList(C1).isAvailableForArrival Then
                'set checkbox of runway based on if it is available
                Me.lstTowerOpenedRunwaysArrival.SetItemCheckState(C1, CheckState.Checked)
                Me.cmsAppDepRunways.Items(CInt(C1)).BackColor = Color.LightGreen
                Me.cmsTowerRunways.Items(CInt(C1)).BackColor = Color.LightGreen
            Else
                'set checkbox of runway based on if it is available
                Me.lstTowerOpenedRunwaysArrival.SetItemCheckState(C1, CheckState.Unchecked)
                Me.cmsAppDepRunways.Items(CInt(C1)).BackColor = Color.Salmon
                Me.cmsTowerRunways.Items(CInt(C1)).BackColor = Color.Salmon
            End If

        Next
        Me.supressAvailableRunwayCheckListEvaluation = False
    End Sub

    'listens if a runway has been opened or closed for departure (usually by the server)
    Friend Sub listenToRunwayDepartureUpdates() Handles Game.availableRunwaysDepartureChanged
        Me.supressAvailableRunwayCheckListEvaluation = True

        Dim arrivalPointList As List(Of clsRunWay) = Me.lstTowerOpenedRunwaysDeparture.Tag
        For C1 As Long = 0 To arrivalPointList.Count - 1
            'check checkboxes based on availability
            If arrivalPointList(C1).isAvailableForDeparture Then
                Me.lstTowerOpenedRunwaysDeparture.SetItemCheckState(C1, CheckState.Checked)
            Else
                Me.lstTowerOpenedRunwaysDeparture.SetItemCheckState(C1, CheckState.Unchecked)
            End If

            'make taxito options colored based on availability
            '!!! inefficient
            For Each singleRunwayMenuItem As System.Windows.Forms.ToolStripDropDownItem In DirectCast(Me.cmsGroundTaxiTo.Items(1), System.Windows.Forms.ToolStripDropDownItem).DropDownItems
                Dim relatedRunway As clsRunWay = Me.Game.AirPort.getRunwayByConnectionPoint(Me.Game.AirPort.getConnectionPointByName(singleRunwayMenuItem.Tag))
                If relatedRunway.isAvailableForDeparture Then
                    singleRunwayMenuItem.BackColor = Color.LightGreen
                Else
                    singleRunwayMenuItem.BackColor = Color.Salmon
                End If

            Next

        Next

            Me.supressAvailableRunwayCheckListEvaluation = False
    End Sub

    Private Sub lstTowerOpenedRunways_ItemCheck(sender As CheckedListBox, e As ItemCheckEventArgs) Handles lstTowerOpenedRunwaysArrival.ItemCheck
        If Not Me.supressAvailableRunwayCheckListEvaluation Then
            Dim arrivalPointList As List(Of clsRunWay) = Me.lstTowerOpenedRunwaysArrival.Tag
            'Dim departurePointList As List(Of clsRunWay) = Me.lstTowerOpenedRunwaysDeparture.Tag

            arrivalPointList(e.Index).isAvailableForArrival = e.NewValue

            Dim isAvailableForLanding As Boolean = (arrivalPointList(e.Index).canHandleArrivals AndAlso arrivalPointList(e.Index).isAvailableForArrival)
            Dim isAvailableForTakeoff As Boolean = arrivalPointList(e.Index).isAvailableForDeparture

            listenToRunwayArrivalUpdates()

            Dim runwayCommand As New clsPlane.structCommandInfo With {.towerRunwayID = arrivalPointList(e.Index).objectID, .towerRunwayIsNewActiveForArrival = isAvailableForLanding, .towerRunwayIsNewActiveForDeparture = isAvailableForTakeoff}
            Me.Game.sendCommandsToServer(runwayCommand)
        End If
    End Sub


    Private Sub lstTowerOpenedRunwaysDeparture_ItemCheck(sender As CheckedListBox, e As ItemCheckEventArgs) Handles lstTowerOpenedRunwaysDeparture.ItemCheck
        If Not Me.supressAvailableRunwayCheckListEvaluation Then
            'Dim arrivalPointList As List(Of clsRunWay) = Me.lstTowerOpenedRunwaysArrival.Tag
            Dim departurePointList As List(Of clsRunWay) = Me.lstTowerOpenedRunwaysDeparture.Tag

            departurePointList(e.Index).isAvailableForDeparture = e.NewValue

            Dim isAvailableForLanding As Boolean = (departurePointList(e.Index).canHandleArrivals AndAlso departurePointList(e.Index).isAvailableForArrival)
            Dim isAvailableForTakeoff As Boolean = departurePointList(e.Index).isAvailableForDeparture

            listenToRunwayDepartureUpdates()

            Dim runwayCommand As New clsPlane.structCommandInfo With {.towerRunwayID = departurePointList(e.Index).objectID, .towerRunwayIsNewActiveForDeparture = isAvailableForTakeoff, .towerRunwayIsNewActiveForArrival = isAvailableForLanding}
            Me.Game.sendCommandsToServer(runwayCommand)
        End If
    End Sub

    'Friend Sub showGroundRadar()
    '    Dim newWindow As New frmGroundRadar
    '    newWindow.Game = Me.Game
    '    newWindow.Show()
    'End Sub

    Private Sub cmdShowGroundRadar_Click(sender As Object, e As EventArgs) Handles cmdShowGroundRadar.Click
        'Dim Thread1 = New System.Threading.Thread(AddressOf showGroundRadar)
        'Thread1.Start()

        Dim newWindow As New frmGroundRadar
        'newWindow.Game = Me.Game
        newWindow.Show()
    End Sub

    Private Sub cmdShowTowerRadar_Click(sender As Object, e As EventArgs) Handles cmdShowTowerRadar.Click
        Dim newWindow As New frmTowerRadar
        'newWindow.Game = Me.Game
        newWindow.Show()
    End Sub

    Private Sub cmdShowAppDepRadar_Click(sender As Object, e As EventArgs) Handles cmdShowAppDepRadar.Click
        Dim newWindow As New frmAppDepRadar
        'newWindow.Game = Me.Game
        newWindow.Show()
    End Sub

    Private Sub trkMaxPlanes_Scroll(sender As TrackBar, e As EventArgs) Handles trkMaxPlanes.Scroll
        Me.Game.maxPlanes = sender.Value
        Me.lblMaxPlanes.Text = "max planes: " & sender.Value
    End Sub

    Private Sub dtpSpawnUntil_ValueChanged(sender As DateTimePicker, e As EventArgs) Handles dtpSpawnUntil.ValueChanged
        Me.Game.allowSpawnUntil = sender.Value
    End Sub

    Private Sub dtpEndGateUntil_ValueChanged(sender As DateTimePicker, e As EventArgs) Handles dtpEndGateUntil.ValueChanged
        Me.Game.allowEndGateUntil = sender.Value
    End Sub

    Private Sub cmdTowerEnterFinal_Click(sender As Object, e As EventArgs) Handles cmdTowerEnterFinal.Click
        Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
            .plane = Me.Game.selectedPlane.callsign,
            .command = clsPlane.enumCommands.airEnterFinal
        }
        Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)

        Me.updateButtonsText()
    End Sub

    Private Sub cmdAppDepEnterFinal_Click(sender As Object, e As EventArgs) Handles cmdAppDepEnterFinal.Click
        Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
           .plane = Me.Game.selectedPlane.callsign,
           .command = clsPlane.enumCommands.airEnterFinal
       }
        Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)

        Me.updateButtonsText()
    End Sub


    Private Sub cmdAppDepSTAR_Click(sender As Button, e As EventArgs) Handles cmdAppDepEnterSTAR.Click
        Dim pointMenuLocation As Point = sender.PointToScreen(Nothing)
        pointMenuLocation.Y += sender.Height

        cmsAppDepSTAR.Show(pointMenuLocation)
    End Sub

    Private Sub mnuSTARclicked(sender As Object, e As EventArgs)
        Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
            .plane = Me.Game.selectedPlane.callsign,
            .command = clsPlane.enumCommands.airEnterSTAR,
            .airAirPathCommandParameter = Me.Game.AirPort.getSTARbyName(sender.text.ToString)
        }
        Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)
    End Sub

    Private Sub mnuStarViaClicked(sender As Object, e As EventArgs)
        Dim newTuple As Tuple(Of String, String)

        newTuple = sender.tag
        Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
            .plane = Me.Game.selectedPlane.callsign,
            .command = clsPlane.enumCommands.airEnterSTARviaNavPoint,
            .airAirPathCommandParameter = Me.Game.AirPort.getSTARbyName(newTuple.Item1),
            .airNavPointCommandParameter = Me.Game.AirPort.airSpaceNavPoints.Find(Function(p As clsNavigationPoint) p.objectID = newTuple.Item2)
            }
        Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)
    End Sub

    Private Sub mnuSIDclicked(sender As Object, e As EventArgs)
        Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
            .plane = Me.Game.selectedPlane.callsign,
            .command = clsPlane.enumCommands.airEnterSID,
            .airAirPathCommandParameter = Me.Game.AirPort.getSIDbyName(sender.text.ToString)
        }
        Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)
    End Sub

    Private Sub mnuNavPointclicked(sender As Object, e As EventArgs)
        Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
                .plane = Me.Game.selectedPlane.callsign,
                .command = clsPlane.enumCommands.airHeadToNavPoint,
                .airNavPointCommandParameter = Me.Game.AirPort.airSpaceNavPoints.Find(Function(p As clsNavigationPoint) p.objectID = sender.Tag)
            }
        Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)
    End Sub

    Private Sub mnuArrDepTowerRunwayclicked(sender As Object, e As EventArgs)
        Dim ParameterTuple As Tuple(Of clsTouchDownWayPoint, clsNavigationPoint) = Me.Game.AirPort.getArrivalPointAndGoaroundPointByName(sender.Tag)

        Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
                .plane = Me.Game.selectedPlane.callsign,
                .command = clsPlane.enumCommands.airExpectRunway,
                .airTouchDownCommandParameter = ParameterTuple.Item1,
                .airGoAroundPointCommandParameter = ParameterTuple.Item2
            }
        Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)

    End Sub

    Private Sub mnuTowerExitViaClicked(sender As Object, e As EventArgs)

        'get exitname from lst
        'if name is "NEXT", make it empty
        Dim selectedSectionPointID As String = Nothing
        Dim selectedSectionPoint As clsTouchDownWayPoint = sender.Tag
        If Not selectedSectionPoint Is Nothing Then selectedSectionPointID = selectedSectionPoint.objectID

        Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
            .plane = Me.Game.selectedPlane.callsign,
            .command = clsPlane.enumCommands.towerExitVia,
            .towerExitPointCommandParameter = selectedSectionPointID
        }

        Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)

    End Sub

    Private Function splitViaInfo(ByVal viaString As String) As List(Of String)
        '  Dim result As New List(Of String)

        Dim splitter As Char() = {" "c}
        Dim result As List(Of String) = Me.txtVia.Text.Split(splitter, StringSplitOptions.RemoveEmptyEntries).ToList

        'remove empty ways 
        For Each singleWayName As String In result
            If singleWayName = "" Then viaString.Remove(singleWayName)
        Next

        Return result
    End Function

    Private Sub cmdGroundChangeTaxi_Click(sender As Object, e As EventArgs) Handles cmdGroundChangeTaxi.Click
        'do only if plane has a goal
        If Not Me.Game.selectedPlane Is Nothing AndAlso Not Me.Game.selectedPlane.ground_goalWayPoint Is Nothing Then
            'prepare preferences
            Dim preference As List(Of String) = Me.splitViaInfo(Me.txtVia.Text)


            'convert preferences from string to points
            Dim preferencePoints As New List(Of clsConnectionPoint)
            For Each singlePreference As String In preference
                Dim point As clsConnectionPoint = Me.Game.AirPort.getConnectionPointByName(singlePreference.ToLower)
                If Not point Is Nothing Then preferencePoints.Add(point)

            Next

            'required as parameter in case runway changes to determine new SID
            'if it is same as old, SID won't change.
            'in this case we keep it
            Dim targetRunway As clsRunWay = Me.Game.AirPort.getRunwayByConnectionPoint(Me.Game.selectedPlane.ground_goalWayPoint)

            Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
                .plane = Me.Game.selectedPlane.callsign,
                .command = clsPlane.enumCommands.taxiTo,
                .groundTaxiRunwayCommandParameter = targetRunway,
                .groundTaxiGoalPointCommandParameter = Me.Game.selectedPlane.ground_goalWayPoint,
                .groundTaxiViaCommandParameter = preferencePoints
            }
            Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)
        End If


    End Sub

    Private Sub mnuGroundTaxiToClicked(sender As Object, e As EventArgs)
        'prepare preferences
        Dim preference As List(Of String) = Me.splitViaInfo(Me.txtVia.Text)


        'in case terminal is selected assign terminal
        Dim targetWayPoint As clsConnectionPoint = Nothing
        If sender.tag = "TERMINAL" Then
            'only assign terminal if there is one
            If Not Me.Game.selectedPlane.ground_terminal Is Nothing Then targetWayPoint = Me.Game.selectedPlane.ground_terminal
        Else
            'if not TERMINAL was selected
            targetWayPoint = Me.Game.AirPort.POIs(sender.Tag.tolower)
        End If

        'only do something if targetwaypoint is selected (opposit can happen if e.g. terminal was selected but there is no terminal defined)
        If Not targetWayPoint Is Nothing Then

            'if plane is departing, check if the seleted point points at runway
            'If Me.Game.selectedPlane.isDeparting Then targetRunway = Me.Game.AirPort.getRunwayByConnectionPoint(targetWayPoint)
            Dim targetRunway As clsRunWay = Me.Game.AirPort.getRunwayByConnectionPoint(targetWayPoint)

            'convert preferences from string to points
            Dim preferencePoints As New List(Of clsConnectionPoint)
            For Each singlePreference As String In preference
                Dim point As clsConnectionPoint = Me.Game.AirPort.getConnectionPointByName(singlePreference.ToLower)
                If Not point Is Nothing Then preferencePoints.Add(point)

            Next


            Me.Game.selectedPlane.commandInfo = New clsPlane.structCommandInfo With {
                .plane = Me.Game.selectedPlane.callsign,
                .command = clsPlane.enumCommands.taxiTo,
                .groundTaxiRunwayCommandParameter = targetRunway,
                .groundTaxiGoalPointCommandParameter = targetWayPoint,            '.currentGroundTaxiCommandParameter = Me.Game.AirPort.POIs(sender.SelectedItem.ToString),
                .groundTaxiViaCommandParameter = preferencePoints
            }
            Me.Game.sendCommandsToServer(Me.Game.selectedPlane.commandInfo)
        End If

    End Sub

    Private Sub cmdAppDepEnterSID_Click(sender As Object, e As EventArgs) Handles cmdAppDepEnterSID.Click
        Dim pointMenuLocation As Point = sender.PointToScreen(Nothing)
        pointMenuLocation.Y += sender.Height

        cmsAppDepSID.Show(pointMenuLocation)
    End Sub

    Private Sub cmdAppDepHeadTo_Click(sender As Object, e As EventArgs) Handles cmdAppDepHeadTo.Click
        Dim pointMenuLocation As Point = sender.PointToScreen(Nothing)
        pointMenuLocation.Y += sender.Height

        cmsAppDepNavPoints.Show(pointMenuLocation)
    End Sub

    Private Sub cmdAppDepExpectRunway_Click(sender As Object, e As EventArgs) Handles cmdAppDepExpectRunway.Click
        Dim pointMenuLocation As Point = sender.PointToScreen(Nothing)
        pointMenuLocation.Y += sender.Height

        cmsAppDepRunways.Show(pointMenuLocation)
    End Sub

    Private Sub cmdTowerExpectRunway_Click(sender As Object, e As EventArgs) Handles cmdTowerExpectRunway.Click
        Dim pointMenuLocation As Point = sender.PointToScreen(Nothing)
        pointMenuLocation.Y += sender.Height

        cmsTowerRunways.Show(pointMenuLocation)
    End Sub

    Private Sub cmdTowerExitVia_Click(sender As Object, e As EventArgs) Handles cmdTowerExitVia.Click
        Dim pointMenuLocation As Point = sender.PointToScreen(Nothing)
        pointMenuLocation.Y += sender.Height

        cmsTowerExitVia.Show(pointMenuLocation)
    End Sub

    Private Sub cmdGroundTaxiTo_Click(sender As Object, e As EventArgs) Handles cmdGroundTaxiTo.Click
        Dim pointMenuLocation As Point = sender.PointToScreen(Nothing)
        pointMenuLocation.Y += sender.Height

        cmsGroundTaxiTo.Show(pointMenuLocation)
    End Sub

    Private Sub cmdAppDepEnterSTARVia_Click(sender As Object, e As EventArgs) Handles cmdAppDepEnterSTARVia.Click
        Dim pointMenuLocation As Point = sender.PointToScreen(Nothing)
        pointMenuLocation.Y += sender.Height

        cmsAppDepSTARvia.Show(pointMenuLocation)
    End Sub

    Private Sub cmdClearVia_Click(sender As Object, e As EventArgs) Handles cmdGroundClearVia.Click
        Me.txtVia.Text = ""
    End Sub

    Private Sub Game_ticked(ByVal _milliseconds As Long) Handles Game.ticked
        Dim timeStamp As DateTime = Now
        Dim oldtime As DateTime = Me.lblMillisecondsBetweenTicks.Tag
        Dim milliseconds As Long = (timeStamp - oldtime).TotalMilliseconds
        If milliseconds > 0 Then
            Me.lblMillisecondsBetweenTicks.Text = milliseconds \ 1 & " ms"
            Me.lblMillisecondsBetweenTicks.Tag = timeStamp
            Me.lblFPS.Text = (1000 / milliseconds) \ 1 & " FPS"
            'Me.lblMillisecondsBetweenTicks.Text = milliseconds & " ms"
        End If
    End Sub

End Class