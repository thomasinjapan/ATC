Option Explicit On
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography.X509Certificates

Public Class frmGroundRadar

    Friend WithEvents Game As clsGame

    Dim devMode As Boolean = True
    Dim mouseLocationBeforeMove As Point
    Dim threadGraphics As Threading.Thread


    'dimensions to draw locations
    Dim offsetX As Double
    Dim offsetY As Double
    Dim multiplyerX As Double
    Dim multiplyerY As Double

    Friend Sub timerhandling()
        Dim sleeper As New Threading.ManualResetEvent(False)

        Do
            sleeper.WaitOne(1) 'timer interval in ms.
            'when timer expires
            'do updates
            Me.Tick()
        Loop
    End Sub


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Game = frmMenu.Game
        recalculateOffsets()
        Me.ctlWindRose.loadAirport(Me.Game.AirPort)
        Me.ctlWindRose.refreshRateInMs = 1000

        Me.threadGraphics = New Threading.Thread(AddressOf timerhandling)
        threadGraphics.Start()

        ' Me.trkTimerIterval.Value = Me.tmrTick.Interval
    End Sub

    Friend Function isVisible(ByRef X As Long, ByRef Y As Long) As Boolean
        Dim result As Boolean = False

        If X < (0 - Me.picGroundRadar.Left) Or X > (Me.pnlGround.Width - Me.picGroundRadar.Left) Or Y < (0 - Me.picGroundRadar.Top) Or Y > (Me.pnlGround.Height - Me.picGroundRadar.Top) Then
            result = False
        Else
            result = True
        End If

        Return result
    End Function


    Private Sub paintPlane(ByRef singlePlane As clsPlane, ByVal offsetX As Double, ByVal offsety As Double, ByVal multiplyerX As Double, ByVal multiplyerY As Double, ByRef graphics As Graphics)
        If singlePlane.isGroundRadarRelevant Then
            'as an open triangle pointing to degree direction
            'point1 is plane location
            'point 2 is plane location w/ distance of 5 and degree - (90+45)
            'Dim cockpitX As Double = singlePlane.pos_X.meters + singlePlane.collisionRadius.meters * Math.Sin(singlePlane.pos_direction * Math.PI / 180)
            'Dim cockpitY As Double = singlePlane.pos_Y.meters - singlePlane.collisionRadius.meters * Math.Cos(singlePlane.pos_direction * Math.PI / 180)

            Dim brushCrashedPlane = New SolidBrush(Color.Red)

            Dim cockpitPointX As Double = (singlePlane.cockpitLocation.X.meters - offsetX) * multiplyerX
            Dim cockpitPointY As Double = (singlePlane.cockpitLocation.Y.meters - offsety) * multiplyerY

            Dim centerPointX As Double = (singlePlane.pos_X.meters - offsetX) * multiplyerX
            Dim centerPointY As Double = (singlePlane.pos_Y.meters - offsety) * multiplyerY

            'if plane is visible
            If Me.isVisible(centerPointX, centerPointY) Then

                Dim beta1 As Double = (singlePlane.pos_direction - (90 + 45))
                Dim beta2 As Double = (singlePlane.pos_direction + (90 + 45))
                Dim planePoint2 As New Point(cockpitPointX + (10 * Math.Sin(beta1 * Math.PI / 180)), (cockpitPointY) - 10 * Math.Cos(beta1 * Math.PI / 180))
                'point 3 is plane location w/ distance of 5 and degree + (90+45)
                Dim planePoint3 As New Point(cockpitPointX + (10 * Math.Sin(beta2 * Math.PI / 180)), (cockpitPointY) - 10 * Math.Cos(beta2 * Math.PI / 180))

                'collisioncircle
                Dim widthheight As Double = singlePlane.modelInfo.length.meters * multiplyerX
                Dim collisionCircleTopLeftX As Double = centerPointX - (singlePlane.collisionRadius.meters) * multiplyerX
                Dim collisionCircleTopLefty As Double = centerPointY - (singlePlane.collisionRadius.meters) * multiplyerY

                'mark selected plane differently
                Dim penPlane As Pen
                Dim penCollisionCircle As Pen
                If singlePlane Is Me.Game.selectedPlane Then
                    penCollisionCircle = New Pen(Color.Red, 2)
                    penPlane = New Pen(Color.Red, 2)
                Else
                    penCollisionCircle = New Pen(Color.Green, 3)
                    penPlane = New Pen(Color.Orange, 2)
                End If

                graphics.DrawEllipse(penCollisionCircle, New Rectangle(New Point(collisionCircleTopLeftX, collisionCircleTopLefty), New Size(widthheight, widthheight)))

                'direction arrow
                graphics.DrawLine(penPlane, New Point(cockpitPointX, cockpitPointY), planePoint2)
                graphics.DrawLine(penPlane, New Point(cockpitPointX, cockpitPointY), planePoint3)


                'crashed plane
                If singlePlane.currentState = clsPlane.enumPlaneState.special_crashed Then graphics.FillEllipse(brushCrashedPlane, New Rectangle(New Point(collisionCircleTopLeftX, collisionCircleTopLefty), New Size(widthheight, widthheight)))


                'pointdetection
                Dim detectionPointScaleX As Double = (singlePlane.cockpitLocation.X.meters - offsetX) * multiplyerX
                Dim detectionPointScaleY As Double = (singlePlane.cockpitLocation.Y.meters - offsety) * multiplyerY
                Dim detectionPointScaleDiameter As Double = singlePlane.pointDetectionCircle.meters * multiplyerX

                Dim penDetectionCircle As Pen = New Pen(Color.Blue, 2)
                graphics.DrawEllipse(penDetectionCircle, New Rectangle(detectionPointScaleX - detectionPointScaleDiameter \ 2, detectionPointScaleY - detectionPointScaleDiameter \ 2, detectionPointScaleDiameter, detectionPointScaleDiameter))

                'Dim aftPointScaleX As Double = (singlePlane.aftLocation.X.meters - offsetX) * multiplyerX
                'Dim aftPointScaleY As Double = (singlePlane.aftLocation.Y.meters - offsety) * multiplyerY

                'penDetectionCircle = New Pen(Color.OrangeRed, 7)
                'e.Graphics.DrawEllipse(penDetectionCircle, New Rectangle(aftPointScaleX - detectionPointScaleDiameter \ 2, aftPointScaleY - detectionPointScaleDiameter \ 2, detectionPointScaleDiameter, detectionPointScaleDiameter))

                'write text
                'only if plane is not at gate
                If Not singlePlane.currentState = clsPlane.enumPlaneState.ground_atGate Then
                    Dim callsign As String = singlePlane.callsign

                    Dim textColor As New SolidBrush(Color.Gray)
                    Dim textFont As New Font("Courier New", 11)

                    If singlePlane.isArriving Then
                        textColor.Color = Color.Goldenrod
                    End If
                    If singlePlane.isDeparting Then
                        textColor.Color = Color.Blue
                    End If
                    If Not singlePlane.frequency = clsPlane.enumFrequency.ground Then
                        textColor.Color = Color.Gray
                    End If

                    If singlePlane Is Me.Game.selectedPlane Then textColor.Color = Color.Red
                    If singlePlane Is Me.Game.selectedPlane Then textFont = New Font(textFont, FontStyle.Bold)

                    Dim textPoint As New Point(((singlePlane.pos_X.meters + singlePlane.collisionRadius.meters) - offsetX) * multiplyerX, ((singlePlane.pos_Y.meters) - offsety) * multiplyerY)
                    graphics.DrawString(callsign, textFont, textColor, textPoint)

                    'write runwaway and if cleartoland
                    Dim runway As String = ""
                    If Not singlePlane.ground_goalWayPoint Is Nothing Then
                        runway = singlePlane.ground_goalWayPoint.stripename
                        If singlePlane.tower_LineUpApproved Then
                            runway &= "➕"
                        End If
                        If singlePlane.tower_takeOffApproved Then
                            runway &= "✔"
                        End If
                    End If
                    ' textFont = New Font("Courier New", 11)
                    textPoint = New Point(((singlePlane.pos_X.meters + singlePlane.collisionRadius.meters) - offsetX) * multiplyerX, ((singlePlane.pos_Y.meters) - offsety) * multiplyerY + 14)
                    graphics.DrawString(runway, textFont, textColor, textPoint)
                End If
            End If
        End If
    End Sub


    Friend Function paintGroundRadarImage() As Image

        Dim picturebox As New PictureBox
        picturebox.Image = New Bitmap(Me.picGroundRadar.Width, Me.picGroundRadar.Height)
        Dim graphics As Graphics = Graphics.FromImage(picturebox.Image)

        Dim backcolor As Color = Color.White
        Dim penTaxiWay As Pen = New Pen(Color.Blue, 2)
        Dim penActiveTaxiWay As Pen = New Pen(Color.Violet, 3)
        Dim penTaxiWayPoint As Pen = New Pen(Color.DarkBlue, 0)
        Dim penGateWay As Pen = New Pen(Color.LightBlue, 3)
        Dim penPlane As Pen = New Pen(Color.Orange, 2)
        Dim penEntryWay As Pen = New Pen(Color.Green, 1)
        Dim penEntryPoint As Pen = New Pen(Color.Green, 3)
        Dim penLandingWayPoint As Pen = New Pen(Color.Purple, 1)
        Dim penRunwayTaxiWay = New Pen(Color.Purple, 1)
        Dim brushCrashedPlane = New SolidBrush(Color.Red)
        Dim penConcrete = New Pen(Color.Gray, 7)
        Dim penWaymarking = New Pen(Color.Orange, 1)
        Dim penRunway = New Pen(Color.Gray, 15)


        picturebox.BackColor = backcolor

        'prepare dimensions
        'get most left point
        'get most top point
        'get most bottom point
        'get most right point

        ''anjust dimensions to draw
        'Dim offsetX As Double = Me.Game.AirPort.groundRadarMostLeft
        'Dim offsetY As Double = Me.Game.AirPort.groundRadarMostTop
        'Dim multiplyerX As Double = Me.picGroundRadar.Width / (Me.Game.AirPort.groundRadarWidth)
        'Dim multiplyerY As Double = Me.picGroundRadar.Height / (Me.Game.AirPort.groundRadarHeight)

        ''make sure that ratio is fixed based using the smaller mulitplier
        'If multiplyerX < multiplyerY Then
        '    multiplyerY = multiplyerX
        'Else
        '    multiplyerX = multiplyerY
        'End If

        'draw landscape
        If Me.chkRenderBackground.Checked Then
            Dim penLandscape As New Pen(Color.FromArgb(128, Color.Gray), 2)
            Dim brushLandscape As New SolidBrush(penLandscape.Color)
            For Each singleLineCollection As List(Of clsNavigationPoint) In Me.Game.AirPort.landscape.lines
                For C1 As Long = 0 To singleLineCollection.Count - 2
                    Dim point1 As New Point((singleLineCollection(C1).pos_X - offsetX) * multiplyerX, (singleLineCollection(C1).pos_Y - offsetY) * multiplyerY)
                    Dim point2 As New Point((singleLineCollection(C1 + 1).pos_X - offsetX) * multiplyerX, (singleLineCollection(C1 + 1).pos_Y - offsetY) * multiplyerY)
                    graphics.DrawLine(penLandscape, point1, point2)
                Next
            Next
            For Each singlePolygonsCollection As List(Of clsNavigationPoint) In Me.Game.AirPort.landscape.Polygons
                Dim points As New List(Of Point)
                For C1 As Long = 0 To singlePolygonsCollection.Count - 2
                    Dim point1 As New Point((singlePolygonsCollection(C1).pos_X - offsetX) * multiplyerX, (singlePolygonsCollection(C1).pos_Y - offsetY) * multiplyerY)
                    Dim point2 As New Point((singlePolygonsCollection(C1 + 1).pos_X - offsetX) * multiplyerX, (singlePolygonsCollection(C1 + 1).pos_Y - offsetY) * multiplyerY)
                    graphics.DrawLine(penLandscape, point1, point2)

                    points.Add(point1)
                Next
                graphics.FillPolygon(brushLandscape, points.ToArray)
            Next
        End If

        'paint runways first, since runwaytaxiways are part of ramp and need to be painted over runway concrete
        'but first do gateways
        For Each ramp As clsRamp In Me.Game.AirPort.Ramps

            'gateways
            For Each gateway As clsGatePath In ramp.gatePaths
                For Each singleGatewaySegment As clsWaySection In gateway.waySections
                    Dim point1 As New Point((singleGatewaySegment.pointAwayFromRamp.pos_X - offsetX) * multiplyerX, (singleGatewaySegment.pointAwayFromRamp.pos_Y - offsetY) * multiplyerY)
                    Dim point2 As New Point((singleGatewaySegment.pointTowardsRamp.pos_X - offsetX) * multiplyerX, (singleGatewaySegment.pointTowardsRamp.pos_Y - offsetY) * multiplyerY)

                    graphics.DrawLine(penGateWay, point1, point2)
                Next
            Next

            'gates
            For Each gate As clsGate In ramp.Gates
                Dim penGate As New Pen(Color.Blue, 1)
                Dim brushGate As New SolidBrush(Color.LightPink)

                Dim gateEndXMeters As Double = gate.pos_X - gate.maxWidth.meters / 2 * Math.Sin(gate.parkingDirection * Math.PI / 180)
                Dim gateEndYMeters As Double = gate.pos_Y + gate.maxWidth.meters / 2 * Math.Cos(gate.parkingDirection * Math.PI / 180)

                'Dim point As New Point((gate.pos_X - offsetX) * multiplyerX, (gate.pos_Y - offsetY) * multiplyerY)
                Dim point As New Point((gateEndXMeters - offsetX) * multiplyerX, (gateEndYMeters - offsetY) * multiplyerY)

                Dim gateRadius As Double = gate.maxWidth.meters * multiplyerX
                Dim gateCircle As New Rectangle(point.X - gateRadius \ 2, point.Y - gateRadius \ 2, gateRadius \ 1, gateRadius \ 1)

                If Not Me.Game.selectedPlane Is Nothing AndAlso Me.Game.selectedPlane.ground_terminal Is gate Then
                    graphics.FillEllipse(brushGate, gateCircle)
                Else
                    graphics.DrawEllipse(penGate, gateCircle)
                End If


                Dim gateNumber As String = gate.radarName
                Dim textPoint As New Point(point.X, point.Y)
                Dim textColor As New SolidBrush(Color.Blue)
                Dim textFont As New Font("Courier New", 10)

                textPoint.X -= (gateNumber.Length * textFont.Size) \ 2
                textPoint.Y -= textFont.Height \ 2

                graphics.DrawString(gateNumber, textFont, textColor, textPoint)

            Next

        Next

        'paint runways - concrete
        For Each runway As clsRunWay In Me.Game.AirPort.runWays
            'paint lineupways - concrete
            For Each lineupway As clsLineUpWay In runway.lineUpPaths
                For Each singleLineUpWaySegment As clsWaySection In lineupway.waySections
                    Dim point1 As New Point((singleLineUpWaySegment.pointAwayFromRamp.pos_X - offsetX) * multiplyerX, (singleLineUpWaySegment.pointAwayFromRamp.pos_Y - offsetY) * multiplyerY)
                    Dim point2 As New Point((singleLineUpWaySegment.pointTowardsRamp.pos_X - offsetX) * multiplyerX, (singleLineUpWaySegment.pointTowardsRamp.pos_Y - offsetY) * multiplyerY)
                    graphics.DrawLine(penRunway, point1, point2)
                Next
            Next

            'paint takeoffpaths - concrete
            For Each takeoffpath As clsTakeOffPath In runway.takeOffPaths
                Dim point1 As New Point((takeoffpath.waySections.First.taxiWayPoint1.pos_X - offsetX) * multiplyerX, (takeoffpath.waySections.First.taxiWayPoint1.pos_Y - offsetY) * multiplyerY)
                Dim point2 As New Point((takeoffpath.waySections.First.taxiWayPoint2.pos_X - offsetX) * multiplyerX, (takeoffpath.waySections.First.taxiWayPoint2.pos_Y - offsetY) * multiplyerY)
                graphics.DrawLine(penRunway, point1, point2)
            Next


            'paint landingways - concrete
            For Each touchdownway In runway.touchDownWays
                Dim point1 As New Point((touchdownway.taxiWayPoint1.pos_X - offsetX) * multiplyerX, (touchdownway.taxiWayPoint1.pos_Y - offsetY) * multiplyerY)
                Dim point2 As New Point((touchdownway.taxiWayPoint2.pos_X - offsetX) * multiplyerX, (touchdownway.taxiWayPoint2.pos_Y - offsetY) * multiplyerY)
                graphics.DrawLine(penRunway, point1, point2)
            Next

            'paint exitways - concrete
            For Each exitway As clsExitWay In runway.exitPaths
                For Each singleExitSegment As clsWaySection In exitway.waySections
                    Dim point1 As New Point((singleExitSegment.pointTowardsRamp.pos_X - offsetX) * multiplyerX, (singleExitSegment.pointTowardsRamp.pos_Y - offsetY) * multiplyerY)
                    Dim point2 As New Point((singleExitSegment.pointAwayFromRamp.pos_X - offsetX) * multiplyerX, (singleExitSegment.pointAwayFromRamp.pos_Y - offsetY) * multiplyerY)
                    graphics.DrawLine(penRunway, point1, point2)
                Next
            Next

            ''if runway is in use, paint it red
            'If runway.isInUse Then
            '    'paint landingways - concrete
            '    For Each touchdownway In runway.touchDownWays
            '        Dim point1 As New Point((touchdownway.taxiWayPoint1.pos_X - offsetX) * multiplyerX, (touchdownway.taxiWayPoint1.pos_Y - offsetY) * multiplyerY)
            '        Dim point2 As New Point((touchdownway.taxiWayPoint2.pos_X - offsetX) * multiplyerX, (touchdownway.taxiWayPoint2.pos_Y - offsetY) * multiplyerY)
            '        e.Graphics.DrawLine(New Pen(Color.Red, 20), point1, point2)
            '    Next
            'End If
        Next

        'paint the ramp - concrete
        For Each ramp As clsRamp In Me.Game.AirPort.Ramps

            'taxipaths - concrete
            For Each taxipath As clsPathWay In ramp.taxiPaths
                For Each singleGatewaySegment As clsWaySection In taxipath.waySections
                    Dim point1 As New Point((singleGatewaySegment.pointAwayFromRamp.pos_X - offsetX) * multiplyerX, (singleGatewaySegment.pointAwayFromRamp.pos_Y - offsetY) * multiplyerY)
                    Dim point2 As New Point((singleGatewaySegment.pointTowardsRamp.pos_X - offsetX) * multiplyerX, (singleGatewaySegment.pointTowardsRamp.pos_Y - offsetY) * multiplyerY)

                    graphics.DrawLine(penConcrete, point1, point2)
                Next
            Next

            'taxipaths on runways
            For Each runwaytaxiway As clsRunwayTaxiPath In ramp.runwayTaxiPaths
                For Each singleRunwayTaxiwaySegment As clsWaySection In runwaytaxiway.waySections
                    Dim point1 As New Point((singleRunwayTaxiwaySegment.taxiWayPoint1.pos_X - offsetX) * multiplyerX, (singleRunwayTaxiwaySegment.taxiWayPoint1.pos_Y - offsetY) * multiplyerY)
                    Dim point2 As New Point((singleRunwayTaxiwaySegment.taxiWayPoint2.pos_X - offsetX) * multiplyerX, (singleRunwayTaxiwaySegment.taxiWayPoint2.pos_Y - offsetY) * multiplyerY)

                    graphics.DrawLine(penConcrete, point1, point2)
                Next
            Next

        Next


        'paint runways - guidelines
        For Each runway As clsRunWay In Me.Game.AirPort.runWays
            'paint lineupways - guideleine
            For Each lineupway As clsLineUpWay In runway.lineUpPaths
                For Each singleLineUpWaySegment As clsWaySection In lineupway.waySections
                    Dim point1 As New Point((singleLineUpWaySegment.pointAwayFromRamp.pos_X - offsetX) * multiplyerX, (singleLineUpWaySegment.pointAwayFromRamp.pos_Y - offsetY) * multiplyerY)
                    Dim point2 As New Point((singleLineUpWaySegment.pointTowardsRamp.pos_X - offsetX) * multiplyerX, (singleLineUpWaySegment.pointTowardsRamp.pos_Y - offsetY) * multiplyerY)

                    graphics.DrawLine(penEntryWay, point1, point2)
                Next
            Next


            'paint takeoffpath - guideline
            For Each takeoffpath As clsTakeOffPath In runway.takeOffPaths
                Dim point1 As New Point((takeoffpath.waySections.First.taxiWayPoint1.pos_X - offsetX) * multiplyerX, (takeoffpath.waySections.First.taxiWayPoint1.pos_Y - offsetY) * multiplyerY)
                Dim point2 As New Point((takeoffpath.waySections.First.taxiWayPoint2.pos_X - offsetX) * multiplyerX, (takeoffpath.waySections.First.taxiWayPoint2.pos_Y - offsetY) * multiplyerY)
                graphics.DrawLine(penEntryWay, point1, point2)
            Next

            'paint landingways - guideline
            For Each touchdownway In runway.touchDownWays
                Dim point1 As New Point((touchdownway.taxiWayPoint1.pos_X - offsetX) * multiplyerX, (touchdownway.taxiWayPoint1.pos_Y - offsetY) * multiplyerY)
                Dim point2 As New Point((touchdownway.taxiWayPoint2.pos_X - offsetX) * multiplyerX, (touchdownway.taxiWayPoint2.pos_Y - offsetY) * multiplyerY)

                graphics.DrawEllipse(penLandingWayPoint, New Rectangle(point1, New Size(2, 2)))
                graphics.DrawEllipse(penLandingWayPoint, New Rectangle(point2, New Size(2, 2)))

                graphics.DrawLine(penLandingWayPoint, point1, point2)
            Next

            'paint exitways - guideline
            For Each exitway As clsExitWay In runway.exitPaths
                For Each singleExitSegment As clsWaySection In exitway.waySections
                    Dim point1 As New Point((singleExitSegment.pointTowardsRamp.pos_X - offsetX) * multiplyerX, (singleExitSegment.pointTowardsRamp.pos_Y - offsetY) * multiplyerY)
                    Dim point2 As New Point((singleExitSegment.pointAwayFromRamp.pos_X - offsetX) * multiplyerX, (singleExitSegment.pointAwayFromRamp.pos_Y - offsetY) * multiplyerY)

                    graphics.DrawLine(penLandingWayPoint, point1, point2)
                Next
            Next

            'paint attempted exitpoint
            If Not Me.Game.selectedPlane Is Nothing Then
                For Each exitPoint As clsTouchDownWayPoint In runway.touchDownWayPoints
                    If exitPoint.objectID = Me.Game.selectedPlane.tower_assignedExitPointID Then
                        Dim point As New Point((exitPoint.pos_X - offsetX) * multiplyerX, (exitPoint.pos_Y - offsetY) * multiplyerY)

                        Dim diameter As Long = 10
                        Dim brush As New SolidBrush(Color.FromArgb(223, Color.Red))

                        'make green if speed is low enough
                        If (Me.Game.selectedPlane.currentState = clsPlane.enumPlaneState.tower_inTouchDown Or
                            Me.Game.selectedPlane.currentState = clsPlane.enumPlaneState.ground_inTaxi) AndAlso
                            Me.Game.selectedPlane.mov_speed_absolute.knots <= Me.Game.selectedPlane.target_speed.knots Then
                            brush.Color = Color.FromArgb(223, Color.LightGreen)
                        End If

                        graphics.FillEllipse(brush, point.X - diameter \ 2, point.Y - diameter \ 2, diameter, diameter)
                    End If
                Next
            End If

            'runwaystring
            Dim textFont As New Font("Courier New", 10)
            Dim textColor As New SolidBrush(Color.Black)
            If runway.canHandleArrivals Then
                Dim runwayName As String = runway.arrivalPoint.radarName
                Dim textPoint As New Point((runway.arrivalPoint.pos_X - offsetX) * multiplyerX, (runway.arrivalPoint.pos_Y - offsetY) * multiplyerY)


                textPoint.X -= (runwayName.Length * textFont.Size) \ 2
                textPoint.Y -= textFont.Height \ 2

                'backgroundbox + string runway for landing
                Dim brushLandingName As SolidBrush
                If runway.isAvailableForArrival Then
                    If runway.isInUse Then
                        brushLandingName = New SolidBrush(Color.FromArgb(127, Color.Yellow))
                    Else
                        brushLandingName = New SolidBrush(Color.FromArgb(127, Color.Green))
                    End If
                Else
                    brushLandingName = New SolidBrush(Color.FromArgb(127, Color.Red))
                End If

                Dim textWidth As Long = graphics.MeasureString(runwayName, textFont).Width
                Dim textHeight As Long = graphics.MeasureString(runwayName, textFont).Height
                graphics.FillRectangle(brushLandingName, textPoint.X, textPoint.Y, textWidth, textHeight)

                graphics.DrawString(runwayName, textFont, textColor, textPoint)
            End If

            For Each singleLineUpWay As clsLineUpWay In runway.lineUpPaths
                Dim stringLineUpName As String = singleLineUpWay.Name
                Dim brushLineUpName As SolidBrush

                Dim lineUpPoint As Point = New Point(singleLineUpWay.entryPoint.pos_X, singleLineUpWay.entryPoint.pos_Y)
                Dim lineUpTextPoint As New Point((lineUpPoint.X - offsetX) * multiplyerX, (lineUpPoint.Y - offsetY) * multiplyerY)

                If runway.isAvailableForDeparture Then
                    brushLineUpName = New SolidBrush(Color.FromArgb(127, Color.Green))
                Else
                    brushLineUpName = New SolidBrush(Color.FromArgb(127, Color.Red))
                End If

                Dim textTakeOffWidth As Long = graphics.MeasureString(stringLineUpName, textFont).Width
                Dim textTakeOffHight As Long = graphics.MeasureString(stringLineUpName, textFont).Height
                graphics.FillRectangle(brushLineUpName, lineUpTextPoint.X, lineUpTextPoint.Y, textTakeOffWidth, textTakeOffHight)

                graphics.DrawString(stringLineUpName, textFont, textColor, lineUpTextPoint)
            Next

        Next

        'paint the ramp
        For Each ramp As clsRamp In Me.Game.AirPort.Ramps

            'taxipaths - guidelines
            For Each taxipath As clsPathWay In ramp.taxiPaths
                For Each singleGatewaySegment As clsWaySection In taxipath.waySections
                    Dim point1 As New Point((singleGatewaySegment.pointAwayFromRamp.pos_X - offsetX) * multiplyerX, (singleGatewaySegment.pointAwayFromRamp.pos_Y - offsetY) * multiplyerY)
                    Dim point2 As New Point((singleGatewaySegment.pointTowardsRamp.pos_X - offsetX) * multiplyerX, (singleGatewaySegment.pointTowardsRamp.pos_Y - offsetY) * multiplyerY)

                    graphics.DrawLine(penWaymarking, point1, point2)
                Next
            Next

            'taxipaths on runways
            For Each runwaytaxiway As clsRunwayTaxiPath In ramp.runwayTaxiPaths
                For Each singleRunwayTaxiwaySegment As clsWaySection In runwaytaxiway.waySections
                    Dim point1 As New Point((singleRunwayTaxiwaySegment.taxiWayPoint1.pos_X - offsetX) * multiplyerX, (singleRunwayTaxiwaySegment.taxiWayPoint1.pos_Y - offsetY) * multiplyerY)
                    Dim point2 As New Point((singleRunwayTaxiwaySegment.taxiWayPoint2.pos_X - offsetX) * multiplyerX, (singleRunwayTaxiwaySegment.taxiWayPoint2.pos_Y - offsetY) * multiplyerY)

                    graphics.DrawLine(penRunwayTaxiWay, point1, point2)

                Next
            Next

            'POIs
            If Me.chkShowLabels.Checked Then
                For Each singleNavPoint As clsConnectionPoint In ramp.connectionPoints
                    '!!!ugly implementation
                    'only draw non-gates or empty names
                    If Not (TypeOf (singleNavPoint) Is clsGate Or singleNavPoint.radarName & "" = "") Then
                        Dim POIText As String = singleNavPoint.radarName
                        Dim textPoint As New Point((singleNavPoint.pos_X - offsetX) * multiplyerX, (singleNavPoint.pos_Y - offsetY) * multiplyerY)
                        Dim textColor As New SolidBrush(Color.Maroon)
                        Dim textFont As New Font("Courier New", 11, FontStyle.Bold)
                        textPoint.X -= (POIText.Length * textFont.Size) \ 2
                        textPoint.Y -= textFont.Height \ 2

                        'backgroundbox
                        Dim brushPOI As New SolidBrush(Color.FromArgb(127, Color.White))
                        Dim textWidth As Long = graphics.MeasureString(POIText, textFont).Width
                        Dim textHeight As Long = graphics.MeasureString(POIText, textFont).Height
                        graphics.FillRectangle(brushPOI, textPoint.X, textPoint.Y, textWidth, textHeight)

                        'text
                        graphics.DrawString(POIText, textFont, textColor, textPoint)
                    End If
                Next
            End If
        Next

        'paint taxipath of selected plane
        If Not Game.selectedPlane Is Nothing AndAlso Not Me.Game.selectedPlane.ground_taxiPath Is Nothing Then
            For Each singlePathWay As clsAStarEngine.structPathStep In Me.Game.selectedPlane.ground_taxiPath

                Dim point1 As Point
                Dim point2 As Point

                If Not singlePathWay.taxiwayToWayPoint Is Nothing Then
                    point1 = New Point((singlePathWay.taxiwayToWayPoint.taxiWayPoint1.pos_X - offsetX) * multiplyerX, (singlePathWay.taxiwayToWayPoint.taxiWayPoint1.pos_Y - offsetY) * multiplyerY)
                    point2 = New Point((singlePathWay.taxiwayToWayPoint.taxiWayPoint2.pos_X - offsetX) * multiplyerX, (singlePathWay.taxiwayToWayPoint.taxiWayPoint2.pos_Y - offsetY) * multiplyerY)

                Else
                    point1 = New Point((Me.Game.selectedPlane.pos_X.meters - offsetX) * multiplyerX, (Me.Game.selectedPlane.pos_Y.meters - offsetY) * multiplyerY)
                    point2 = New Point((Me.Game.selectedPlane.ground_nextWayPoint.pos_X - offsetX) * multiplyerX, (Me.Game.selectedPlane.ground_nextWayPoint.pos_Y - offsetY) * multiplyerY)
                End If

                graphics.DrawLine(penActiveTaxiWay, point1, point2)

            Next
        End If

        ''paint wind arrow
        'Dim penWind As New Pen(Color.Blue, 2)
        'Dim windXscale As Double = 30
        'Dim windYscale As Double = 30

        'Dim beta1 As Double = (Me.Game.AirPort.windDirectionTo + (180))
        'Dim beta2 As Double = (Me.Game.AirPort.windDirectionTo - (90 + 45))
        'Dim beta3 As Double = (Me.Game.AirPort.windDirectionTo + (90 + 45))
        'Dim windPoint1 As New Point(windXscale + (15 * Math.Sin(beta1 * Math.PI / 180)), (windYscale) - 15 * Math.Cos(beta1 * Math.PI / 180))
        'Dim windPoint2 As New Point(windXscale + (5 * Math.Sin(beta2 * Math.PI / 180)), (windYscale) - 5 * Math.Cos(beta2 * Math.PI / 180))
        'Dim windPoint3 As New Point(windXscale + (5 * Math.Sin(beta3 * Math.PI / 180)), (windYscale) - 5 * Math.Cos(beta3 * Math.PI / 180))
        'e.Graphics.DrawLine(penWind, New Point(windXscale, windYscale), windPoint1)
        'e.Graphics.DrawLine(penWind, New Point(windXscale, windYscale), windPoint2)
        'e.Graphics.DrawLine(penWind, New Point(windXscale, windYscale), windPoint3)

        'paint planes
        For Each singlePlane As clsPlane In Me.Game.Planes
            Me.paintPlane(singlePlane, offsetX, offsetY, multiplyerX, multiplyerY, graphics)
        Next

        If Not Me.Game.selectedPlane Is Nothing Then Me.paintPlane(Me.Game.selectedPlane, offsetX, offsetY, multiplyerX, multiplyerY, graphics)


        GC.Collect()

        Return picturebox.Image
    End Function



    Friend Sub updateImage(ByRef image As Image)
        Me.picGroundRadar.Image = image
        If Me.chkShowFramerate.Checked Then
            Me.lblFPS.Visible = True
            Me.lblMillisecondsBetweenFrames.Visible = True
        Else
            Me.lblFPS.Visible = False
            Me.lblMillisecondsBetweenFrames.Visible = False
        End If

        Dim timeStamp As DateTime = Now
        Dim oldtime As DateTime = Me.lblMillisecondsBetweenFrames.Tag
        Dim milliseconds As Long = (timeStamp - oldtime).TotalMilliseconds
        Me.lblMillisecondsBetweenFrames.Text = milliseconds & " ms"
        Me.lblFPS.Text = (1000 / milliseconds) \ 1 & " FPS"
        Me.lblMillisecondsBetweenFrames.Tag = timeStamp

    End Sub

    Friend Sub Tick()
        'paint picture in separate this non-ui thread and then transfer it to the pic as one image
        Dim image As Image = paintGroundRadarImage()

        Try
            Me.Invoke(Sub() updateImage(image))
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try


    End Sub

    Private Sub picGroundRadar_MouseWheel(sender As PictureBox, e As MouseEventArgs) Handles picGroundRadar.MouseWheel
        Dim X As Long = e.X
        Dim Y As Long = e.Y

        X += sender.Left
        Y += sender.Top

        If e.Delta > 0 Then
            sender.Width *= 1.02
            sender.Height *= 1.02

            sender.Left = X - 1.02 * (X - sender.Left)
            sender.Top = Y - 1.02 * (Y - sender.Top)
        Else
            sender.Width *= 0.98
            sender.Height *= 0.98

            sender.Left = X - 0.98 * (X - sender.Left)
            sender.Top = Y - 0.98 * (Y - sender.Top)
        End If
    End Sub

    Private Sub picGroundRadar_MouseDown(sender As Object, e As MouseEventArgs) Handles picGroundRadar.MouseDown
        If e.Button = MouseButtons.Right Then
            '!!!make icon to move icon
            sender.Cursor = Cursors.SizeAll
        End If
        Me.mouseLocationBeforeMove = e.Location
    End Sub

    Private Sub picGroundRadar_MouseUp(sender As Object, e As MouseEventArgs) Handles picGroundRadar.MouseUp
        sender.Cursor = Cursors.Default
    End Sub

    Private Sub picGroundRadar_MouseMove(sender As Object, e As MouseEventArgs) Handles picGroundRadar.MouseMove
        If e.Button = MouseButtons.Right Then
            Dim X As Long = e.X
            Dim Y As Long = e.Y

            sender.Left += e.X - Me.mouseLocationBeforeMove.X
            sender.Top += e.Y - Me.mouseLocationBeforeMove.Y
        End If

    End Sub

    Private Sub Game_EventCardFound(ByRef card As clsAStarCard) Handles Game.EventCardFound

        'make sure that ratio is fixed based using the smaller mulitplier
        If Me.multiplyerX < Me.multiplyerY Then
            Me.multiplyerY = Me.multiplyerX
        Else
            Me.multiplyerX = Me.multiplyerY
        End If

        'paint locaiton of point to map
        Dim X As Double = (card.node.pos_X - Me.offsetX) * Me.multiplyerX
        Dim Y As Double = (card.node.pos_Y - Me.offsetY) * Me.multiplyerY

        'MsgBox(X & " | " & Y)
        Dim newBrush As New SolidBrush(Color.FromArgb(128, Color.LightGoldenrodYellow))

        Dim g As Graphics = picGroundRadar.CreateGraphics
        g.FillEllipse(newBrush, X \ 1 - 2, Y \ 1 - 2, CInt(5), CInt(5))

    End Sub

    Private Sub picGroundRadar_Resize(sender As PictureBox, e As EventArgs) Handles picGroundRadar.Resize
        Me.recalculateOffsets()
    End Sub

    Private Sub recalculateOffsets()
        If Not Me.Game Is Nothing Then
            'anjust dimensions to draw
            Me.offsetX = Me.Game.AirPort.groundRadarMostLeft
            Me.offsetY = Me.Game.AirPort.groundRadarMostTop
            Me.multiplyerX = Me.picGroundRadar.Width / (Me.Game.AirPort.groundRadarWidth)
            Me.multiplyerY = Me.picGroundRadar.Height / (Me.Game.AirPort.groundRadarHeight)

            'make sure that ratio is fixed based using the smaller mulitplier
            If Me.multiplyerX < Me.multiplyerY Then
                Me.multiplyerY = Me.multiplyerX
            Else
                Me.multiplyerX = Me.multiplyerY
            End If
        End If

    End Sub

    Private Sub frmGroundRadar_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.L Or e.KeyCode = Keys.Space Then
            Me.chkShowLabels.Checked = Not Me.chkShowLabels.Checked
        ElseIf e.KeyCode = Keys.E Or e.KeyCode = Keys.B Then
            Me.chkRenderBackground.Checked = Not Me.chkRenderBackground.Checked
        End If
    End Sub

    Private Sub chkShowLabels_GotFocus(sender As Object, e As EventArgs) Handles chkShowLabels.GotFocus
        Me.Focus()
    End Sub

    Private Sub frmGroundRadar_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        'close thread
        If Me.threadGraphics.IsAlive Then
            Me.threadGraphics.Abort()
        End If
    End Sub
End Class
