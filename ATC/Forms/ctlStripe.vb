Option Explicit On

Public Class ctlStripe
    Friend WithEvents plane As clsPlane
    Friend Property isSelected As Boolean = False

    Friend Event stripeClicked(ByRef stripe As ctlStripe)

    Friend Sub loadPlane(ByRef plane As clsPlane)
        Me.plane = plane

        Me.paintAllLabels()
    End Sub

    Friend Sub paintAllLabels() Handles plane.statusChanged

        Me.picCallsign.Refresh()
        Me.picType.Refresh()
        Me.picTargetHeight.Refresh()
        Me.picTargetSpeed.Refresh()
        Me.picRwy.Refresh()
        Me.picPathInfo.Refresh()
        Me.picPathInfo.Refresh()

    End Sub

    Private Sub picCallsign_Paint(sender As PictureBox, e As PaintEventArgs) Handles picCallsign.Paint
        Dim callsign As String = ""
        If Not Me.plane Is Nothing Then
            callsign = Me.plane.callsign
        End If

        Dim textColor As New SolidBrush(Color.Black)
        Dim textFont As New Font("Arial", 11)
        Dim textPoint As New Point(0, 0)

        'if plane is waiting, make background red and text white
        If Me.plane.awaitingOrders Then
            Me.picCallsign.BackColor = Color.Red
            textColor = New SolidBrush(Color.White)
        Else
            sender.BackColor = Color.White
        End If

        e.Graphics.DrawString(callsign, textFont, textColor, textPoint)
        textColor.Dispose()
        textFont.Dispose()
    End Sub

    Private Sub picType_Paint(sender As PictureBox, e As PaintEventArgs) Handles picType.Paint
        Dim planeType As String = ""
        If Not Me.plane Is Nothing Then
            planeType = Me.plane.modelInfo.modelShort
        End If
        Dim textColor As New SolidBrush(Color.Black)
        Dim textFont As New Font("Arial", 11)
        Dim textPoint As New Point(0, 0)
        e.Graphics.DrawString(planetype, textFont, textColor, textPoint)
    End Sub

    Private Sub picTargetHeight_Paint(sender As PictureBox, e As PaintEventArgs) Handles picTargetHeight.Paint
        'make box red if default overridden by ATC else white
        If Me.plane.air_altitudeOverrideByATC Then
            sender.BackColor = Color.Red
        Else
            sender.BackColor = Color.White
        End If

        'prepare text
        Dim planeTargetHeight As String = ""
        If Not Me.plane Is Nothing Then
            planeTargetHeight = Me.plane.target_altitude.feet \ 1
        End If
        Dim textColor As New SolidBrush(Color.Black)
        Dim textFont As New Font("Arial", 11)
        Dim textPoint As New Point(0, 0)
        e.Graphics.DrawString(planeTargetHeight, textFont, textColor, textPoint)
    End Sub

    Private Sub picTargetSpeed_Paint(sender As PictureBox, e As PaintEventArgs) Handles picTargetSpeed.Paint
        Dim planeTargetSpeed As String = ""
        If Not Me.plane Is Nothing Then
            planeTargetSpeed = Me.plane.target_speed.knots \ 1
        End If
        Dim textColor As New SolidBrush(Color.Black)
        Dim textFont As New Font("Arial", 11)
        Dim textPoint As New Point(0, 0)
        e.Graphics.DrawString(planeTargetSpeed, textFont, textColor, textPoint)
    End Sub

    Private Sub picRwy_Paint(sender As PictureBox, e As PaintEventArgs) Handles picRwy.Paint
        Dim runway As String = ""

        If Not Me.plane Is Nothing Then
            'check for ground runway
            If Not Me.plane.ground_goalWayPoint Is Nothing Then
                runway = Me.plane.ground_goalWayPoint.stripename
                If Me.plane.tower_LineUpApproved And Not Me.plane.tower_takeOffApproved Then
                    runway &= "➕"
                End If
                If Me.plane.tower_takeOffApproved Then
                    runway &= "✔"
                End If
            End If

            'check for air runway
            If Not Me.plane.tower_assignedLandingPoint Is Nothing Then
                runway = Me.plane.tower_assignedLandingPoint.stripename
                If Me.plane.tower_cleardToLand Then
                    runway &= "✔"
                Else
                    runway &= "❌"
                End If
            End If
        End If
        Dim textColor As New SolidBrush(Color.Black)
        Dim textFont As New Font("Arial", 11)
        Dim textPoint As New Point(0, 0)
        e.Graphics.DrawString(runway, textFont, textColor, textPoint)
    End Sub

    Private Sub picPathInfo_Paint(sender As PictureBox, e As PaintEventArgs) Handles picPathInfo.Paint
        Dim nextPoint As String = ""
        Dim flightPath As String = ""
        Dim lastPoint As String = ""
        Dim status As String = ""
        Dim frequency As String = ""

        If Not Me.plane Is Nothing Then
            'color plane based on intend          
            If Me.plane.currentState = clsPlane.enumPlaneState.special_crashed Then
                'make all red
                sender.BackColor = Color.DarkRed
            ElseIf Me.plane.isArriving Then
                If Me.isSelected Then
                    sender.BackColor = Color.DarkSalmon
                Else
                    sender.BackColor = Color.Khaki
                End If
            ElseIf Me.plane.isDeparting Then
                If isSelected Then
                    sender.BackColor = Color.DarkSalmon
                Else
                    sender.BackColor = Color.LightBlue
                End If
            Else
                sender.BackColor = Color.White
            End If

            'text
            'flightpath
            If Not Me.plane.air_currentAirPathName Is Nothing Then
                flightPath = Me.plane.air_currentAirPathName
            End If

            'next point
            If Not Me.plane.air_nextWayPoint Is Nothing Then
                'show via only if there is a path
                If Not Me.plane.air_currentAirPathName Is Nothing Then nextPoint = "via "
                nextPoint &= Me.plane.air_nextWayPoint.stripename
            ElseIf Me.plane.currentState = clsPlane.enumPlaneState.tower_FinalApproach AndAlso Not Me.plane.tower_assignedLandingPoint Is Nothing Then
                nextPoint &= Me.plane.tower_assignedLandingPoint.name
            Else
                nextPoint = Me.plane.target_direction \ 1 & "°"
            End If

            'final point
            If Not Me.plane.air_goalWayPoint Is Nothing Then
                lastPoint = "TO: " & Me.plane.air_goalWayPoint.stripename
            End If

            'status and frequency
            Select Case Me.plane.currentState
                Case clsPlane.enumPlaneState.ground_atGate : status = "at gate"
                Case clsPlane.enumPlaneState.ground_awaitingPushback : status = "awaiting pushback"
                Case clsPlane.enumPlaneState.ground_breaking : status = "breaking"
                Case clsPlane.enumPlaneState.ground_holdingPosition : status = "holding"
                Case clsPlane.enumPlaneState.ground_inParking : status = "parking"
                Case clsPlane.enumPlaneState.ground_inPushback : status = "pushback"
                Case clsPlane.enumPlaneState.ground_inTaxi : status = "taxi"
                Case clsPlane.enumPlaneState.ground_preparingGate : status = "prepare gate"
                Case clsPlane.enumPlaneState.special_crashed : status = "CRASHED"
                Case clsPlane.enumPlaneState.tower_Departed : status = "departed"
                Case clsPlane.enumPlaneState.tower_enteringTouchDown : status = "entering tochdown"
                Case clsPlane.enumPlaneState.tower_freeFlight : status = "flight"
                Case clsPlane.enumPlaneState.tower_FinalApproach : status = "final"
                Case clsPlane.enumPlaneState.tower_inLineUp : status = "in lineup"
                Case clsPlane.enumPlaneState.tower_inTouchDown : status = "touchdown"
                Case clsPlane.enumPlaneState.tower_linedupAndWaiting : status = "lined up"
                Case clsPlane.enumPlaneState.tower_takingOff : status = "takeoff"
                Case clsPlane.enumPlaneState.undefined : status = "UNDEFINED"
                Case Else : status = "UNDEFINED"

            End Select

            frequency = Me.plane.frequency.ToString


        End If


        Dim textColor As New SolidBrush(Color.Black)
        Dim textFont As New Font("Segoe Script", 11)
        Dim textPointNext As New Point(0, 0)
        Dim textPointPath As New Point(0, 15)
        Dim textPointLast As New Point(0, 30)
        e.Graphics.DrawString(flightPath, textFont, textColor, textPointNext)
        e.Graphics.DrawString(nextPoint, textFont, textColor, textPointPath)
        e.Graphics.DrawString(lastPoint, textFont, textColor, textPointLast)

        '!!! add status And frequency for debug purposes
        Dim statusFrequencyFont As New Font("Courier New", 7)
        Dim statusFrequencyPoint As New Point(0, 50)


        e.Graphics.DrawString(status & " | " & frequency, statusFrequencyFont, textColor, statusFrequencyPoint)

    End Sub

    Private Sub picPathInfo_Click(sender As PictureBox, e As EventArgs) Handles picPathInfo.Click
        RaiseEvent stripeClicked(Me)
    End Sub

End Class
