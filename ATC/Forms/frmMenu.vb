Option Explicit On
Imports System.ComponentModel
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Runtime.Serialization.Formatters.Binary

Public Class frmMenu
    Private isFirstStart As Boolean = True
    Friend Game As clsGame

    Private Sub frmMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'change wind tracker delays based on selected playtime
        Dim totalSeconds As Long = (New TimeSpan(Me.Game.allowEndGateUntil.Ticks - Now.Ticks)).TotalSeconds

        If Me.Game.isclient Then totalSeconds = 3600
        Me.trkChangeWindMaxDelay.Minimum = 1
        Me.trkChangeWindMaxDelay.Maximum = totalSeconds
        Me.trkChangeWindMinDelay.Minimum = 1
        Me.trkChangeWindMinDelay.Maximum = totalSeconds
        Me.trkChangeWindMinDelay.Value = 25
        Me.trkChangeWindMaxDelay.Value = 45


        'Dim Thread1 = New System.Threading.Thread(AddressOf showGroundRadar)
        'Thread1.Start()
        frmGroundRadar.Show()

        frmGroundRadar.Left = 1
        frmGroundRadar.Top = 1

        frmAllControl.Show()
        frmAllControl.Left = frmGroundRadar.Left + frmGroundRadar.Width
        frmAllControl.Top = 1


        frmTowerRadar.Show()
        frmTowerRadar.Left = 1
        frmTowerRadar.Top = frmGroundRadar.Top + frmGroundRadar.Height

        frmAppDepRadar.Show()
        frmAppDepRadar.Left = frmTowerRadar.Left + frmTowerRadar.Width
        frmAppDepRadar.Top = frmAllControl.Top * frmAllControl.Height

        Me.Game.minWindChangeAngle = Me.trkChangeWindMinAngle.Value
        Me.Game.maxWindChangeAngle = Me.trkChangeWindMaxAngle.Value
        Me.Game.minEndgateDelay = Me.trkEndGateMin.Value * 1000
        Me.Game.maxEndgateDelay = Me.trkEndGateMax.Value * 1000
        Me.Game.minSpawnDelay = Me.trkSpawnMin.Value * 1000
        Me.Game.maxSpawnDelay = Me.trkSpawnMax.Value * 1000
        Me.Game.minWindChangeDelay = Me.trkChangeWindMinDelay.Value = 1000
        Me.Game.maxWindChangeDelay = Me.trkChangeWindMaxDelay.Value * 1000

        If Me.Game.isServer Then
            Dim hostname As String = System.Net.Dns.GetHostName
            Dim IPList As New List(Of String)

            For Each singleIP As IPAddress In System.Net.Dns.GetHostEntry(hostname).AddressList()
                If singleIP.AddressFamily = AddressFamily.InterNetwork Then
                    'If Not (singleIP.IsIPv4MappedToIPv6 Or
                    '    singleIP.IsIPv6LinkLocal Or
                    '    singleIP.IsIPv6Multicast Or
                    '    singleIP.IsIPv6SiteLocal Or
                    '    singleIP.IsIPv6Teredo) Then
                    IPList.Add(singleIP.ToString)
                End If
            Next
            Dim IPs As String = String.Join(", ", IPList)
            Me.cmdWaitForPlayer.Text = "Let Players join at " & hostname & " or " & IPs

            Me.lblSpawnMin.Text = Me.trkSpawnMin.Value & " sec."
            Me.lblSpawnMax.Text = Me.trkSpawnMax.Value & " sec."

            Me.lblEndGateMin.Text = Me.trkEndGateMin.Value & " sec."
            Me.lblEndGateMax.Text = Me.trkEndGateMax.Value & " sec."

            Me.lblWindDelayMin.Text = Me.trkChangeWindMinDelay.Value & " sec."
            Me.lblWindDelayMax.Text = Me.trkChangeWindMaxDelay.Value & " sec."

            Me.lblWindDirectionMin.Text = Me.trkChangeWindMinAngle.Value & " deg."
            Me.lblWindDirectionMax.Text = Me.trkChangeWindMaxAngle.Value & " deg."

            Me.chkEnableAppDep.Checked = Me.Game.playApproachDeparture


            Dim randomizer As New Random(DateTime.Now.Millisecond)


            Me.Game.minSpawnDelay = Me.trkSpawnMin.Value * 1000
            Me.Game.maxSpawnDelay = Me.trkSpawnMax.Value * 1000
            Me.Game.minEndgateDelay = Me.trkEndGateMin.Value * 1000
            Me.Game.maxEndgateDelay = Me.trkEndGateMax.Value * 1000

            '(only) in Case of first stat make sure tat somthing happens
            If Me.isFirstStart Then
                Me.isFirstStart = False

                Me.Game.timerSpawn.Interval = 1000
                Me.Game.timerEndGate.Interval = 1000
            Else
                Me.Game.timerSpawn.Interval = randomizer.Next(Me.Game.minSpawnDelay, Me.Game.maxSpawnDelay + 1)
                Me.Game.timerEndGate.Interval = randomizer.Next(Me.Game.minEndgateDelay, Me.Game.maxEndgateDelay + 1)

            End If

            'winddirection tracker
            Me.trkInitialWindDirection.Value = Me.Game.AirPort.windDirectionTo
            Me.lblWindDirectionfrom.Text = "from: " & Me.Game.AirPort.windDirectionFrom & "°"
            Me.lblWindDirectionTo.Text = "to: " & Me.Game.AirPort.windDirectionTo & "°"

            'maxcrosswind
            Me.trkMaxCrossWind.Value = Me.Game.AirPort.maxCrossWind
            Me.lblMaxCrossWind.Text = Me.Game.AirPort.maxCrossWind & "°"


        End If
    End Sub

    Private Sub cmdStartPause_Click(sender As Button, e As EventArgs) Handles cmdStartPause.Click

        Me.Game.togglePause()

        If Game.isPaused Then
            sender.Text = "Start | ▶"
        Else
            sender.Text = "Pause | ⏸"
            Me.WindowState = FormWindowState.Minimized
        End If
    End Sub

    Private Sub frmMenu_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        '!!! close and unload all windows that were created during game

        If Me.Game.isServer Then
            Me.Game.Universe.Stop()
            Me.Game.Universe.Enabled = False
        End If

        'close all windows before closing self to avoid crash because game in unloaded while other window still uses the game instance
        frmGroundRadar.Close()
        frmGroundRadar.Dispose()

        frmTowerRadar.Close()
        frmTowerRadar.Dispose()


        frmAppDepRadar.Close()
        frmAppDepRadar.Dispose()

        frmAllControl.Close()
        frmAllControl.Dispose()

        'show stats
        MsgBox("successful landings: " & Game.successfulLandings & vbNewLine &
               "successful arrivals: " & Game.successfulGated & vbNewLine &
               "..of which at right gate: " & Game.successfulArrival & vbNewLine &
               "successful take-offs: " & Game.successfulTakeOffs & vbNewLine &
               "successful departures: " & Game.successfulDeparted & vbNewLine &
               "crashed planes: " & Game.crashedPlanes & vbNewLine)

        Me.Game = Nothing

        frmMainMenu.Show()

        GC.Collect()
    End Sub

    Private Sub trkSpawnMax_Scroll(sender As TrackBar, e As EventArgs) Handles trkSpawnMax.Scroll
        If sender.Value < Me.trkSpawnMin.Value Then Me.trkSpawnMin.Value = sender.Value
        Me.lblSpawnMax.Text = Me.trkSpawnMax.Value & " sec."
        Me.lblSpawnMin.Text = Me.trkSpawnMin.Value & " sec."

        Me.Game.minSpawnDelay = Me.trkSpawnMin.Value * 1000
        Me.Game.maxSpawnDelay = Me.trkSpawnMax.Value * 1000

        Dim randomizer As New Random(DateTime.Now.Millisecond)
        Me.Game.timerSpawn.Interval = randomizer.Next(Me.Game.minSpawnDelay, Me.Game.maxSpawnDelay + 1)

    End Sub

    Private Sub trkSpawnMin_Scroll(sender As TrackBar, e As EventArgs) Handles trkSpawnMin.Scroll
        If sender.Value > Me.trkSpawnMax.Value Then Me.trkSpawnMax.Value = sender.Value
        Me.lblSpawnMax.Text = Me.trkSpawnMax.Value & " sec."
        Me.lblSpawnMin.Text = Me.trkSpawnMin.Value & " sec."

        Me.Game.minSpawnDelay = Me.trkSpawnMin.Value * 1000
        Me.Game.maxSpawnDelay = Me.trkSpawnMax.Value * 1000

        Dim randomizer As New Random(DateTime.Now.Millisecond)
        Me.Game.timerSpawn.Interval = randomizer.Next(Me.Game.minSpawnDelay, Me.Game.maxSpawnDelay + 1)

    End Sub

    Private Sub trkEndGateMin_Scroll(sender As TrackBar, e As EventArgs) Handles trkEndGateMin.Scroll
        If sender.Value > Me.trkEndGateMax.Value Then Me.trkEndGateMax.Value = sender.Value
        Me.lblEndGateMin.Text = Me.trkEndGateMin.Value & " sec."
        Me.lblEndGateMax.Text = Me.trkEndGateMax.Value & " sec."

        Me.Game.minEndgateDelay = Me.trkEndGateMin.Value * 1000
        Me.Game.maxEndgateDelay = Me.trkEndGateMax.Value * 1000

        Dim randomizer As New Random(DateTime.Now.Millisecond)
        Me.Game.timerEndGate.Interval = randomizer.Next(Me.Game.minEndgateDelay, Me.Game.maxEndgateDelay + 1)

    End Sub

    Private Sub trkEndGateMax_Scroll(sender As TrackBar, e As EventArgs) Handles trkEndGateMax.Scroll
        If sender.Value < Me.trkEndGateMin.Value Then Me.trkEndGateMin.Value = sender.Value
        Me.lblEndGateMin.Text = Me.trkEndGateMin.Value & " sec."
        Me.lblEndGateMax.Text = Me.trkEndGateMax.Value & " sec."

        Me.Game.minEndgateDelay = Me.trkEndGateMin.Value * 1000
        Me.Game.maxEndgateDelay = Me.trkEndGateMax.Value * 1000

        Dim randomizer As New Random(DateTime.Now.Millisecond)
        Me.Game.timerEndGate.Interval = randomizer.Next(Me.Game.minEndgateDelay, Me.Game.maxEndgateDelay + 1)

    End Sub

    Private Sub waitForPlayers()

        'send airport to client
        Dim airport As clsAirport = Me.Game.AirPort

        Dim formatter As New BinaryFormatter
        Dim streamTarget As New MemoryStream()
        formatter.Serialize(streamTarget, airport)

        Dim planeAsByteArray() As Byte = streamTarget.ToArray()
        Dim arraySize As Int32 = planeAsByteArray.Length
        Dim arraySizeArray() As Byte = BitConverter.GetBytes(arraySize)
        Dim byteArray() As Byte = arraySizeArray.Concat(planeAsByteArray).ToArray

        While True
            Me.Game.TCPListener.Start()
            'Me.Game.TCPServer = Me.Game.TCPListener.AcceptSocket
            Dim tmpClient As TcpClient = Me.Game.TCPListener.AcceptTcpClient
            tmpClient.NoDelay = True
            tmpClient.Client.Send(byteArray)

            'add to group of clients
            Me.Game.TCPServerClientPlayers.Add(tmpClient)
        End While

    End Sub

    Private Sub cmdWaitForPlayer_Click(sender As Button, e As EventArgs) Handles cmdWaitForPlayer.Click
        sender.Enabled = False
        Me.Game.TCPListener = New TcpListener(IPAddress.Any, Me.txtPort.Text)

        Dim threadScanForPlayers As New Threading.Thread(AddressOf Me.waitForPlayers)
        threadScanForPlayers.IsBackground = True
        threadScanForPlayers.Priority = Threading.ThreadPriority.Lowest
        threadScanForPlayers.Start()

        Me.Game.tmrServerListen = New Timer With {.Interval = 100, .Enabled = False}
        Me.Game.tmrServerListen.Enabled = True

        Me.Game.tmrServerSendKeyFrame = New Timer With {.Interval = 200, .Enabled = False}
        Me.Game.tmrServerSendKeyFrame.Enabled = True

        Me.Text &= " - server"
        frmAllControl.Text &= " - server"
        frmAppDepRadar.Text &= " - server"
        frmGroundRadar.Text &= " - server"
        frmTowerRadar.Text &= " - server"

        'network
        Me.trkClientUpdate.Enabled = True
        Me.trkClientUpdate.Value = Me.Game.tmrServerSendKeyFrame.Interval
        Me.lblClientUpdate.Text = Me.trkClientUpdate.Value & " ms"

        'adjust interval to send based on size of airport

        Dim formatter As New BinaryFormatter
        'Dim streamTarget As New MemoryStream()
        Dim streamTargetMessage As New MemoryStream()

        formatter.Serialize(streamTargetMessage, Me.Game.AirPort)
        'MsgBox(streamTargetMessage.Length)

        Me.trkClientUpdate.Value = streamTargetMessage.Length \ 4000
        Me.Game.tmrServerSendKeyFrame.Interval = Me.trkClientUpdate.Value
        Me.lblClientUpdate.Text = Me.trkClientUpdate.Value & " ms"

    End Sub

    Private Sub trkChangeWindMinDelay_Scroll(sender As TrackBar, e As EventArgs) Handles trkChangeWindMinDelay.Scroll
        If Me.trkChangeWindMinDelay.Value > Me.trkChangeWindMaxDelay.Value Then Me.trkChangeWindMaxDelay.Value = trkChangeWindMinDelay.Value
        Me.lblWindDelayMin.Text = Me.trkChangeWindMinDelay.Value & " sec."
        Me.lblWindDelayMax.Text = Me.trkChangeWindMaxDelay.Value & " sec."

        Me.Game.minWindChangeDelay = Me.trkChangeWindMinDelay.Value * 1000
        Me.Game.maxWindChangeDelay = Me.trkChangeWindMaxDelay.Value * 1000

        Dim randomizer As New Random(DateTime.Now.Millisecond)
        Me.Game.timerWindChange.Interval = randomizer.Next(Me.Game.minWindChangeDelay, Me.Game.maxWindChangeDelay + 1)
    End Sub

    Private Sub trkChangeWindMaxDelay_Scroll(sender As TrackBar, e As EventArgs) Handles trkChangeWindMaxDelay.Scroll
        If Me.trkChangeWindMaxDelay.Value < Me.trkChangeWindMinDelay.Value Then trkChangeWindMinDelay.Value = Me.trkChangeWindMaxDelay.Value
        Me.lblWindDelayMin.Text = Me.trkChangeWindMinDelay.Value & " sec."
        Me.lblWindDelayMax.Text = Me.trkChangeWindMaxDelay.Value & " sec."

        Me.Game.minWindChangeDelay = Me.trkChangeWindMinDelay.Value * 1000
        Me.Game.maxWindChangeDelay = Me.trkChangeWindMaxDelay.Value * 1000

        Dim randomizer As New Random(DateTime.Now.Millisecond)
        Me.Game.timerWindChange.Interval = randomizer.Next(Me.Game.minWindChangeDelay, Me.Game.maxWindChangeDelay + 1)
    End Sub

    Private Sub trkChangeWindMinAngle_Scroll(sender As TrackBar, e As EventArgs) Handles trkChangeWindMinAngle.Scroll
        If Me.trkChangeWindMinAngle.Value > Me.trkChangeWindMaxAngle.Value Then Me.trkChangeWindMaxAngle.Value = trkChangeWindMinAngle.Value
        Me.lblWindDirectionMin.Text = Me.trkChangeWindMinAngle.Value & " deg."
        Me.lblWindDirectionMax.Text = Me.trkChangeWindMaxAngle.Value & " deg."

        Me.Game.minWindChangeAngle = Me.trkChangeWindMinAngle.Value
        Me.Game.maxWindChangeAngle = Me.trkChangeWindMaxAngle.Value

    End Sub

    Private Sub trkChangeWindMaxAngle_Scroll(sender As TrackBar, e As EventArgs) Handles trkChangeWindMaxAngle.Scroll
        If Me.trkChangeWindMaxAngle.Value < Me.trkChangeWindMinAngle.Value Then trkChangeWindMinAngle.Value = Me.trkChangeWindMaxAngle.Value
        Me.lblWindDirectionMin.Text = Me.trkChangeWindMinAngle.Value & " deg."
        Me.lblWindDirectionMax.Text = Me.trkChangeWindMaxAngle.Value & " deg."

        Me.Game.minWindChangeAngle = Me.trkChangeWindMinAngle.Value
        Me.Game.maxWindChangeAngle = Me.trkChangeWindMaxAngle.Value
    End Sub

    Private Sub chkEnableAppDep_CheckedChanged(sender As CheckBox, e As EventArgs) Handles chkEnableAppDep.CheckedChanged
        'avoid crash on start of menu
        If Not Me.Game Is Nothing Then
            Me.Game.playApproachDeparture = sender.Checked
        End If
    End Sub

    Private Sub trkInitialWindDirection_Scroll(sender As Object, e As EventArgs) Handles trkInitialWindDirection.Scroll
        If Not Me.Game Is Nothing Then
            Me.Game.AirPort.setWindDirection(Me.trkInitialWindDirection.Value)
            Me.lblWindDirectionfrom.Text = "from: " & Me.Game.AirPort.windDirectionFrom & "°"
            Me.lblWindDirectionTo.Text = "to: " & Me.Game.AirPort.windDirectionTo & "°"
        End If

    End Sub

    Private Sub trkMaxCrossWind_Scroll(sender As Object, e As EventArgs) Handles trkMaxCrossWind.Scroll
        If Not Me.Game Is Nothing Then
            Me.Game.AirPort.setMaxCrossWind(Me.trkMaxCrossWind.Value)
            Me.lblMaxCrossWind.Text = Me.Game.AirPort.maxCrossWind & "°"
        End If
    End Sub

    Private Sub trkClientUpdate_Scroll(sender As Object, e As EventArgs) Handles trkClientUpdate.Scroll
        If Not Me.Game Is Nothing Then
            Me.Game.tmrServerSendKeyFrame.Interval = Me.trkClientUpdate.Value
            Me.lblClientUpdate.Text = Me.trkClientUpdate.Value & " ms"
        End If
    End Sub
End Class