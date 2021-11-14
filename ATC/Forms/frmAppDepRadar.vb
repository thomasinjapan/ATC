Option Explicit On
Imports System.ComponentModel

Public Class frmAppDepRadar
    Friend Game As clsGame
    Dim threadGraphics As Threading.Thread

    Dim mouseLocationBeforeMove As Point

    Private Sub frmTraconRadar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Game = frmMenu.Game
        Me.ctlWindRose.loadAirport(Me.Game.AirPort)
        Me.ctlWindRose.refreshRateInMs = 1000

        Dim dimension As Integer
        If Me.picAppDep.Height < Me.picAppDep.Width Then
            dimension = Me.picAppDep.Height
        Else
            dimension = Me.picAppDep.Width
        End If

        Me.threadGraphics = New Threading.Thread(AddressOf timerhandling)
        threadGraphics.Start()

    End Sub

    Friend Sub timerhandling()
        Dim sleeper As New Threading.ManualResetEvent(False)

        Do
            sleeper.WaitOne(1) 'wait one MS

            Me.Tick()
        Loop
    End Sub

    Friend Sub Tick()
        'paint picture in separate this non-ui thread and then transfer it to the pic as one image
        Dim image As Image = paintAppDepImage()

        Try
            Me.Invoke(Sub() updateImage(image))
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try

    End Sub

    Friend Sub updateImage(ByRef image As Image)
        Me.picAppDep.Image = image
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

    Private Function paintAppDepImage() As Image
        Dim picturebox As New PictureBox
        picturebox.Image = New Bitmap(Me.picAppDep.Width, Me.picAppDep.Height)
        Dim graphics As Graphics = Graphics.FromImage(picturebox.Image)

        'draw background
        Dim backcolor As Color = Color.MidnightBlue
        Dim brushBack As Brush = New SolidBrush(backcolor)
        Dim penActiveTaxiWay As Pen = New Pen(Color.Violet, 3)
        Dim penTaxiWayPoint As Pen = New Pen(Color.DarkBlue, 0)
        Dim penGate As Pen = New Pen(Color.LightBlue, 3)
        Dim penGateWay As Pen = New Pen(Color.LightBlue, 3)
        Dim penPlane As Pen = New Pen(Color.Orange, 1)
        ' Dim penEntryWay As Pen = New Pen(Color.Green, 1)
        Dim penEntryPoint As Pen = New Pen(Color.Green, 3)
        Dim penLandingWayPoint As Pen = New Pen(Color.Purple, 1)
        Dim penRunwayTaxiWay = New Pen(Color.YellowGreen, 2)
        Dim penConcrete = New Pen(Color.Gray, 2)
        Dim penWaymarking = New Pen(Color.Orange, 1)
        Dim penRunway = New Pen(Color.LimeGreen, 5)
        graphics.FillRectangle(brushBack, 0, 0, picturebox.Image.Width, picturebox.Image.Height)

        'anjust dimensions to draw
        Dim offsetX As Double = Me.Game.AirPort.appDepRadarMostLeft
        Dim offsetY As Double = Me.Game.AirPort.appDepRadarMostTop
        Dim multiplyerX As Double = Me.picAppDep.Width / (Me.Game.AirPort.appDepRadarWidth)
        Dim multiplyerY As Double = Me.picAppDep.Height / (Me.Game.AirPort.appDepRadarHeight)

        'make sure that ratio is fixed based using the smaller mulitplier
        If multiplyerX < multiplyerY Then
            multiplyerY = multiplyerX
        Else
            multiplyerX = multiplyerY
        End If


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

        'airNavPoints
        For Each NavPoint As clsAirNavPoint In Me.Game.AirPort.airSpaceNavPoints
            Dim point As New Point((NavPoint.pos_X - offsetX) * multiplyerX, (NavPoint.pos_Y - offsetY) * multiplyerY)
            Dim penNavPoint As New Pen(Color.Salmon, 2)
            Dim penDebugPoint As New Pen(Color.FromArgb(127, Color.Red), 1)
            Dim brushILS As New SolidBrush(Color.LimeGreen)

            Select Case NavPoint.NavPointType
                Case clsAirNavPoint.enumAirNavPointType.SID
                    graphics.DrawLine(penNavPoint, point.X - 5, point.Y, point.X + 5, point.Y)
                    graphics.DrawLine(penNavPoint, point.X, point.Y - 5, point.X, point.Y + 5)
                    graphics.DrawEllipse(penNavPoint, New Rectangle(point.X - 3, point.Y - 3, 5, 5))

                Case clsAirNavPoint.enumAirNavPointType.pilot
                    graphics.DrawEllipse(penDebugPoint, New Rectangle(point.X, point.Y, 2, 2))
                Case clsAirNavPoint.enumAirNavPointType.ILS
                    graphics.FillEllipse(brushILS, New Rectangle(point.X - 1, point.Y - 1, 3, 3))

                Case clsAirNavPoint.enumAirNavPointType.LOC
                    graphics.DrawEllipse(penDebugPoint, New Rectangle(point.X, point.Y, 2, 2))
                Case Else
                    graphics.DrawEllipse(penNavPoint, New Rectangle(point.X - 3, point.Y - 3, 6, 6))
            End Select

            'navpointstring
            'only if not pilot
            If Not (NavPoint.NavPointType = clsAirNavPoint.enumAirNavPointType.pilot Or
                    NavPoint.NavPointType = clsAirNavPoint.enumAirNavPointType.ILS Or
                    NavPoint.NavPointType = clsAirNavPoint.enumAirNavPointType.LOC) Then
                Dim navPointName As String = NavPoint.radarName
                Dim navPointPoint As New Point(point.X, point.Y)
                navPointPoint.X += 4
                navPointPoint.Y += 4
                Dim textColor As New SolidBrush(Color.LightGray)
                Dim textFont As New Font("Courier New", 10)

                graphics.DrawString(navPointName, textFont, textColor, navPointPoint)
            End If
        Next

        'STARs
        Dim penSTAR As New Pen(Color.FromArgb(127, Color.DarkGoldenrod), 1)
        For Each singleSTAR As clsAirPath In Me.Game.AirPort.STARs
            For Each singlePath As clsAStarEngine.structPathStep In singleSTAR.path
                If Not singlePath.taxiwayToWayPoint Is Nothing Then
                    Dim point1 As New Point((singlePath.taxiwayToWayPoint.taxiWayPoint1.pos_X - offsetX) * multiplyerX, (singlePath.taxiwayToWayPoint.taxiWayPoint1.pos_Y - offsetY) * multiplyerY)
                    Dim point2 As New Point((singlePath.taxiwayToWayPoint.taxiWayPoint2.pos_X - offsetX) * multiplyerX, (singlePath.taxiwayToWayPoint.taxiWayPoint2.pos_Y - offsetY) * multiplyerY)

                    graphics.DrawLine(penSTAR, point1, point2)
                End If
            Next
        Next


        'SIDs
        Dim penSID As New Pen(Color.FromArgb(127, Color.SkyBlue), 1)
        For Each singleSID As clsAirPath In Me.Game.AirPort.SIDs
            For Each singlePath As clsAStarEngine.structPathStep In singleSID.path
                If Not singlePath.taxiwayToWayPoint Is Nothing Then
                    Dim point1 As New Point((singlePath.taxiwayToWayPoint.taxiWayPoint1.pos_X - offsetX) * multiplyerX, (singlePath.taxiwayToWayPoint.taxiWayPoint1.pos_Y - offsetY) * multiplyerY)
                    Dim point2 As New Point((singlePath.taxiwayToWayPoint.taxiWayPoint2.pos_X - offsetX) * multiplyerX, (singlePath.taxiwayToWayPoint.taxiWayPoint2.pos_Y - offsetY) * multiplyerY)

                    graphics.DrawLine(penSID, point1, point2)
                End If
            Next
        Next

        'paint runways
        For Each runway As clsRunWay In Me.Game.AirPort.runWays
            'pait takeoffpaths
            For Each takeoffpath As clsTakeOffPath In runway.takeOffPaths
                Dim point1 As New Point((takeoffpath.waySections.First.taxiWayPoint1.pos_X - offsetX) * multiplyerX, (takeoffpath.waySections.First.taxiWayPoint1.pos_Y - offsetY) * multiplyerY)
                Dim point2 As New Point((takeoffpath.waySections.First.taxiWayPoint2.pos_X - offsetX) * multiplyerX, (takeoffpath.waySections.First.taxiWayPoint2.pos_Y - offsetY) * multiplyerY)

                graphics.DrawLine(penRunway, point1, point2)
                '  Graphics.DrawLine(penEntryWay, point1, point2)
            Next


            ''paint landingways
            'For Each touchdownway In runway.touchDownWays
            '    Dim point1 As New Point((touchdownway.taxiWayPoint1.pos_X - offsetX) * multiplyerX, (touchdownway.taxiWayPoint1.pos_Y - offsetY) * multiplyerY)
            '    Dim point2 As New Point((touchdownway.taxiWayPoint2.pos_X - offsetX) * multiplyerX, (touchdownway.taxiWayPoint2.pos_Y - offsetY) * multiplyerY)

            '   Graphics.DrawEllipse(penLandingWayPoint, New Rectangle(point1, New Size(2, 2)))
            '   Graphics.DrawEllipse(penLandingWayPoint, New Rectangle(point2, New Size(2, 2)))

            '   Graphics.DrawLine(penRunway, point1, point2)
            '   Graphics.DrawLine(penLandingWayPoint, point1, point2)
            'Next

            'runwaystring
            If runway.canHandleArrivals Then
                Dim runwayName As String = runway.arrivalPoint.radarName
                Dim textPoint As New Point((runway.arrivalPoint.pos_X - offsetX) * multiplyerX, (runway.arrivalPoint.pos_Y - offsetY) * multiplyerY)
                Dim textColor As New SolidBrush(Color.LightGray)
                Dim textFont As New Font("Courier New", 10)

                textPoint.X -= (runwayName.Length * textFont.Size) \ 2
                textPoint.Y -= textFont.Height \ 2

                graphics.DrawString(runwayName, textFont, textColor, textPoint)
            End If
        Next

        'circles
        Dim penCircle As New Pen(Color.FromArgb(127, Color.Gray), 1)
        For C1 As Long = 1 To 10
            Dim radius As Long = New clsDistanceCollection(C1 * 10, clsDistanceCollection.enumDistanceUnits.nauticalMiles).meters                'radius in 10 miles steps
            Dim drawRadius As Long = (radius) * multiplyerX
            Dim point1 As New Point((Me.Game.AirPort.ARP.pos_X - offsetX) * multiplyerX, (Me.Game.AirPort.ARP.pos_Y - offsetY) * multiplyerY)

            point1.X -= drawRadius
            point1.Y -= drawRadius

            graphics.DrawEllipse(penCircle, New Rectangle(point1, New Size(drawRadius * 2, drawRadius * 2)))
        Next


        'paint flightpath of selected plane
        If Not Game.selectedPlane Is Nothing AndAlso Not Me.Game.selectedPlane.air_flightPath Is Nothing Then
            For Each singlePathWay As clsAStarEngine.structPathStep In Me.Game.selectedPlane.air_flightPath

                Dim point1 As Point
                Dim point2 As Point

                If Not singlePathWay.taxiwayToWayPoint Is Nothing Then
                    point1 = New Point((singlePathWay.taxiwayToWayPoint.taxiWayPoint1.pos_X - offsetX) * multiplyerX, (singlePathWay.taxiwayToWayPoint.taxiWayPoint1.pos_Y - offsetY) * multiplyerY)
                    point2 = New Point((singlePathWay.taxiwayToWayPoint.taxiWayPoint2.pos_X - offsetX) * multiplyerX, (singlePathWay.taxiwayToWayPoint.taxiWayPoint2.pos_Y - offsetY) * multiplyerY)

                Else
                    point1 = New Point((Me.Game.selectedPlane.pos_X.meters - offsetX) * multiplyerX, (Me.Game.selectedPlane.pos_Y.meters - offsetY) * multiplyerY)
                    point2 = New Point((Me.Game.selectedPlane.air_nextWayPoint.pos_X - offsetX) * multiplyerX, (Me.Game.selectedPlane.air_nextWayPoint.pos_Y - offsetY) * multiplyerY)
                End If

                graphics.DrawLine(penActiveTaxiWay, point1, point2)

            Next
        End If

        'paint planes
        'make copy to avoid collision of threads (one updating the list, the other painting
        Dim allplanes(Me.Game.Planes.Count - 1) As clsPlane
        Me.Game.Planes.CopyTo(allplanes)

        For Each singlePlane As clsPlane In allplanes
            If Not singlePlane Is Me.Game.selectedPlane Then Me.paintPlane(singlePlane, offsetX, offsetY, multiplyerX, multiplyerY, graphics)
        Next

        If Not Me.Game.selectedPlane Is Nothing Then Me.paintPlane(Me.Game.selectedPlane, offsetX, offsetY, multiplyerX, multiplyerY, graphics)

        GC.Collect()

        Return picturebox.Image
    End Function

    Private Sub paintPlane(ByRef singlePlane As clsPlane, ByVal offsetX As Double, ByVal offsetY As Double, ByVal multiplyerX As Double, ByVal multiplyerY As Double, ByRef graphics As Graphics)
        If singlePlane.isArrDepRadarRelevant Then
            Dim brushCrashedPlane = New SolidBrush(Color.Red)
            '    Dim penTaxiWay As Pen = New Pen(Color.Blue, 2)
            Dim brushProximity = New SolidBrush(Color.LightYellow)

            Dim planeColor As New SolidBrush(Color.Gray)
            If singlePlane.isArriving Then
                planeColor.Color = Color.Goldenrod
            End If
            If singlePlane.isDeparting Then
                planeColor.Color = Color.DeepSkyBlue
            End If
            If Not singlePlane.frequency = clsPlane.enumFrequency.appdep Or
                singlePlane.frequency = clsPlane.enumFrequency.arrival Or
                singlePlane.frequency = clsPlane.enumFrequency.departure Then
                planeColor.Color = Color.Gray
            End If
            If singlePlane Is Me.Game.selectedPlane Then
                planeColor.Color = Color.Red
            End If

            Dim planeXscale As Double = (singlePlane.cockpitLocation.X.meters - offsetX) * multiplyerX
            Dim planeYscale As Double = (singlePlane.cockpitLocation.Y.meters - offsetY) * multiplyerY

            'circle preparation
            Dim widthheight As Double = singlePlane.modelInfo.length.meters * multiplyerX

            Dim pointXCollisionCircle As Double = ((singlePlane.pos_X.meters - singlePlane.collisionRadius.meters) - offsetX) * multiplyerX
            Dim pointYCollisionCircle As Double = ((singlePlane.pos_Y.meters - singlePlane.collisionRadius.meters) - offsetY) * multiplyerY

            'paint history
            For C1 = 0 To singlePlane.air_FlightPathHistory.Count - 1
                Dim rectangleWidth As Long = 5
                Dim rectangleAlpha As Integer = 255 * (1 - (C1 / singlePlane.air_FlightPathHistory.Count))
                Dim brushHistoryAlpha As New SolidBrush(Color.FromArgb(rectangleAlpha, planeColor.Color))
                Dim index As Long = singlePlane.air_FlightPathHistory.Count - C1 - 1
                Dim historyXscale As Double = (singlePlane.air_FlightPathHistory(index).Item1.meters - offsetX) * multiplyerX
                Dim historyYscale As Double = (singlePlane.air_FlightPathHistory(index).Item2.meters - offsetY) * multiplyerY

                Dim rectangleHistory As New Rectangle(historyXscale - rectangleWidth \ 2, historyYscale - rectangleWidth \ 2, rectangleWidth, rectangleWidth)
                graphics.FillRectangle(brushHistoryAlpha, rectangleHistory)
            Next


            'mark selected plane differently
            Dim penPlane As Pen
            Dim penCollisionCircle As Pen
            If singlePlane Is Me.Game.selectedPlane Then
                penCollisionCircle = New Pen(Color.Red, 2)
                penPlane = New Pen(Color.Red, 1)
            Else
                penCollisionCircle = New Pen(Color.Green, 3)
                penPlane = New Pen(planeColor, 1)
            End If

            graphics.DrawEllipse(penCollisionCircle, New Rectangle(New Point(pointXCollisionCircle, pointYCollisionCircle), New Size(widthheight, widthheight)))

            'crashed plane
            If singlePlane.currentState = clsPlane.enumPlaneState.special_crashed Then graphics.FillEllipse(brushCrashedPlane, New Rectangle(New Point(pointXCollisionCircle, pointYCollisionCircle), New Size(widthheight, widthheight)))

            ''pointdetection
            'Dim detectionPointScaleX As Double = (singlePlane.cockpitLocation.X.meters - offsetX) * multiplyerX
            'Dim detectionpointScaleY As Double = (singlePlane.cockpitLocation.Y.meters - offsetY) * multiplyerY
            'Dim detectionPointScaleDiameter As Double = singlePlane.pointDetectionCircle.meters * multiplyerX

            'e.Graphics.DrawEllipse(penTaxiWay, New Rectangle(New Point(detectionPointScaleX, detectionpointScaleY), New Size(detectionPointScaleDiameter, detectionPointScaleDiameter)))

            'write label
            Dim callSign As String = singlePlane.callsign
            Dim planeType As String = singlePlane.modelInfo.modelShort
            Dim altitude As String = singlePlane.pos_Altitude.feet \ 1
            Dim altitudeTarget As String = singlePlane.target_altitude.feet \ 1
            Dim speed As String = singlePlane.mov_speed_absolute.knots \ 1
            Dim speedTarget As String = singlePlane.target_speed.knots \ 1

            Dim rootX As Integer = 10 + ((singlePlane.pos_X.meters + singlePlane.collisionRadius.meters) - offsetX) * multiplyerX
            Dim rootY As Integer = 10 + ((singlePlane.pos_Y.meters + singlePlane.collisionRadius.meters) - offsetY) * multiplyerY

            Dim pointCallSign As New Point(rootX, rootY)
            Dim pointPlaneType As New Point(pointCallSign.X + 60, pointCallSign.Y)
            Dim pointAltitude As New Point(pointCallSign.X, pointCallSign.Y + 15)

            Dim textFont As New Font("Courier New", 10)

            If singlePlane Is Me.Game.selectedPlane Then textFont = New Font(textFont, FontStyle.Bold)
            If singlePlane Is Me.Game.selectedPlane Then planeColor.Color = Color.Red

            'name and type
            graphics.DrawString(callSign, textFont, planeColor, pointCallSign)
            graphics.DrawString(planeType, textFont, planeColor, pointPlaneType)

            'write altitude and target altitude
            If singlePlane.pos_Altitude.feet > singlePlane.target_altitude.feet Then
                altitudeTarget = "↓" & altitudeTarget
            ElseIf singlePlane.pos_Altitude.feet < singlePlane.target_altitude.feet Then
                altitudeTarget = "↑" & altitudeTarget
            Else
                altitudeTarget = "|" & altitudeTarget
            End If

            'write speed and target speed
            If singlePlane.mov_speed_absolute.knots > singlePlane.target_speed.knots Then
                speedTarget = "↓" & speedTarget
            ElseIf singlePlane.mov_speed_absolute.knots < singlePlane.target_speed.knots Then
                speedTarget = "↑" & speedTarget
            Else
                speedTarget = "|" & speedTarget
            End If

            'line between plane location and label
            Dim linePoint1 As New Point(planeXscale + 3, planeYscale + 3)
            graphics.DrawLine(penPlane, linePoint1, pointCallSign)

            'write text
            graphics.DrawString(altitude & altitudeTarget & " " & speed & speedTarget, textFont, planeColor, pointAltitude)


        End If
    End Sub

    Private Sub picTracon_MouseWheel(sender As PictureBox, e As MouseEventArgs) Handles picAppDep.MouseWheel
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

    Private Sub picTracon_MouseMove(sender As PictureBox, e As MouseEventArgs) Handles picAppDep.MouseMove
        If e.Button = MouseButtons.Right Then
            Dim X As Long = e.X
            Dim Y As Long = e.Y

            sender.Left += e.X - Me.mouseLocationBeforeMove.X
            sender.Top += e.Y - Me.mouseLocationBeforeMove.Y
        End If
    End Sub

    Private Sub picTracon_MouseDown(sender As Object, e As MouseEventArgs) Handles picAppDep.MouseDown
        If e.Button = MouseButtons.Right Then
            '!!!make icon to move icon
            sender.Cursor = Cursors.SizeAll

        End If
        Me.mouseLocationBeforeMove = e.Location
    End Sub

    Private Sub picTracon_MouseUp(sender As Object, e As MouseEventArgs) Handles picAppDep.MouseUp
        sender.Cursor = Cursors.Default
    End Sub

    Private Sub frmAppDepRadar_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If Me.threadGraphics.IsAlive Then
            Me.threadGraphics.Abort()
        End If
    End Sub
End Class