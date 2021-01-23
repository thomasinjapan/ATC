Option Explicit On
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Xml.Serialization

Public Class frmMainMenu
    Private Sub cmdLoadGame_Click(sender As Object, e As EventArgs) Handles cmdLoadGame.Click
        Dim filePath As String = Me.cboAirport.Tag(Me.cboAirport.SelectedIndex)

        frmMenu.Game = New clsGame(filePath, Me.trkMaxPlanesGround.Value, Me.trkMaxPlanesTotal.Value)
        frmMenu.Game.allowSpawnUntil = Now.AddMinutes(Me.trkSpawnUntil.Value)
        frmMenu.Game.allowEndGateUntil = Now.AddMinutes(Me.trkEndGateUntil.Value)

        '!!! ↑problem is that the time is not delayed in case game is paused...
        Me.Hide()
        frmMenu.Show()
    End Sub

    Private Sub cmdJoinGame_Click(sender As Object, e As EventArgs) Handles cmdJoinGame.Click
        Try

            'join the game at IP
            'make connection
            frmMenu.Game = New clsGame()

            Dim game As clsGame = frmMenu.Game

            game.TCPClient = New TcpClient(Me.txtJoinIP.Text, Me.txtPort.Text)
            game.TCPClient.NoDelay = True
            ' game.TCPClient.ReceiveBufferSize = UInt16.MaxValue * 16
            game.TCPClientStream = game.TCPClient.GetStream()

            game.tmrClientListen = New Timer With {.Interval = 100, .Enabled = False}
            game.serverConnected = True

            'wait until server sends airport information
            While Not game.TCPClientStream.DataAvailable
                'no data here yet
            End While

            'get header knowing that first int32 has
            Dim arraySizeArray(4) As Byte
            game.TCPClientStream.Read(arraySizeArray, 0, CInt(4))
            Dim arraySize As Int32 = BitConverter.ToInt32(arraySizeArray, 0)
            'Dim airportInBytes(arraySize) As Byte
            Dim airportInBytes(arraySize) As Byte
            Dim reader As New BinaryReader(game.TCPClientStream)
            'start reading from stream starting after the size integer for the expected length 
            '!!!assuming the buffer contains all the data 
            'game.TCPClientStream.Read(airportInBytes, 0, arraySize)

            Dim bytesread As Long = 0
            '!!! possibly change necessary in case airportinbytes will be overwritten or arraysize should be receivedbuffersize

            While bytesread < arraySize
                airportInBytes(bytesread) = reader.ReadByte
                bytesread += 1
            End While

            'receive airport data and load the game accordingly
            'Dim airportInBytes(game.TCPClient.ReceiveBufferSize) As Byte
            'game.TCPClientStream.Read(airportInBytes, 0, CInt(game.TCPClient.ReceiveBufferSize))

            Dim formatter As New BinaryFormatter
            Dim streamTarget As New MemoryStream(airportInBytes)

            Dim airport As clsAirport
            airport = formatter.Deserialize(streamTarget)

            game.AirPort = airport

            'start receiving planes
            game.tmrClientListen.Enabled = True

            'Load frmMenu but disable all options since client can't do anything
            Me.Hide()
            frmMenu.Show()
            frmMenu.cmdStartPause.Enabled = False
            frmMenu.cmdWaitForPlayer.Enabled = False
            frmMenu.trkEndGateMax.Enabled = False
            frmMenu.trkEndGateMin.Enabled = False
            frmMenu.trkSpawnMax.Enabled = False
            frmMenu.trkSpawnMin.Enabled = False
            frmMenu.txtPort.Enabled = False
            frmMenu.trkInitialWindDirection.Enabled = False
            frmMenu.trkChangeWindMaxAngle.Enabled = False
            frmMenu.trkChangeWindMaxDelay.Enabled = False
            frmMenu.trkChangeWindMinAngle.Enabled = False
            frmMenu.trkChangeWindMinDelay.Enabled = False
            frmMenu.trkMaxCrossWind.Enabled = False
            frmMenu.txtPort.Enabled = False
            frmMenu.trkClientUpdate.Enabled = False


            frmMenu.Text &= " - client"
            frmAllControl.Text &= " - client"
            frmAppDepRadar.Text &= " - client"
            frmGroundRadar.Text &= " - client"
            frmTowerRadar.Text &= " - client"

            ''start timer
            'game.tmrClientSend = New Timer With {.Interval = 100, .Enabled = False}
            'game.tmrClientSend.Enabled = True
            game.timerHistory.Enabled = True
        Catch ex As Exception

        End Try

    End Sub

    Private Sub trkSpawnUntil_Scroll(sender As TrackBar, e As EventArgs) Handles trkSpawnUntil.Scroll
        Me.lblSpawnUntil.Text = sender.Value & " min"
    End Sub

    Private Sub trkEndGateUntil_Scroll(sender As TrackBar, e As EventArgs) Handles trkEndGateUntil.Scroll
        Me.lblEndGateUntil.Text = sender.Value & " min"
    End Sub

    Private Sub trkMaxPlanesGround_Scroll(sender As TrackBar, e As EventArgs) Handles trkMaxPlanesGround.Scroll
        If sender.Value > Me.trkMaxPlanesTotal.Value Then Me.trkMaxPlanesTotal.Value = sender.Value
        Me.lblMaxPlanesGround.Text = sender.Value & " planes"
        Me.lblMaxPlanes.Text = Me.trkMaxPlanesTotal.Value & " planes"
    End Sub

    Private Sub trkMaxPlanedTotal_Scroll(sender As TrackBar, e As EventArgs) Handles trkMaxPlanesTotal.Scroll
        If sender.Value < Me.trkMaxPlanesGround.Value Then Me.trkMaxPlanesGround.Value = sender.Value
        Me.lblMaxPlanes.Text = sender.Value & " planes"
        Me.lblMaxPlanesGround.Text = Me.trkMaxPlanesGround.Value & " planes"
    End Sub

    Private Sub frmMainMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = Me.Text & " (" & My.Application.Info.Version.ToString() & ")"

        Me.lblSpawnUntil.Text = Me.trkSpawnUntil.Value & " min"
        Me.lblEndGateUntil.Text = Me.trkEndGateUntil.Value & " min"
        Me.lblMaxPlanesGround.Text = Me.trkMaxPlanesGround.Value & " planes"
        Me.lblMaxPlanes.Text = Me.trkMaxPlanesTotal.Value & " planes"

        Me.cboAirport.Items.Clear()
        Me.cboAirport.Tag = New List(Of String)

        'search own location folder for airport files
        For Each singleFile As String In IO.Directory.GetFiles(Application.StartupPath & "\data\")
            Dim info As New FileInfo(singleFile)
            If info.Extension = ".atc" Then
                'check if file is airport file

                Dim index As Integer = Me.cboAirport.Items.Add(info.Name)
                'add complete path to list in tag
                Me.cboAirport.Tag.add(singleFile)
            End If
        Next

        Me.cboAirport.SelectedIndex = 0

        'get own IP address
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
        Me.txtJoinIP.Text = IPList.First
    End Sub

End Class
