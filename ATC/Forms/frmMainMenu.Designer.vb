<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMainMenu
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.cmdLoadGame = New System.Windows.Forms.Button()
        Me.trkSpawnUntil = New System.Windows.Forms.TrackBar()
        Me.trkEndGateUntil = New System.Windows.Forms.TrackBar()
        Me.lblSpawnUntil = New System.Windows.Forms.Label()
        Me.lblEndGateUntil = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtClientMessages = New System.Windows.Forms.TextBox()
        Me.txtMessageFromServer = New System.Windows.Forms.TextBox()
        Me.cmdSendToClients = New System.Windows.Forms.Button()
        Me.txtMessagesServer = New System.Windows.Forms.TextBox()
        Me.txtMesssageFromClient = New System.Windows.Forms.TextBox()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.tmrServer = New System.Windows.Forms.Timer(Me.components)
        Me.txtIP = New System.Windows.Forms.TextBox()
        Me.cmdConnectToServer = New System.Windows.Forms.Button()
        Me.tmrClient = New System.Windows.Forms.Timer(Me.components)
        Me.cmdJoinGame = New System.Windows.Forms.Button()
        Me.txtJoinIP = New System.Windows.Forms.TextBox()
        Me.txtPort = New System.Windows.Forms.TextBox()
        Me.cboAirport = New System.Windows.Forms.ComboBox()
        Me.label3 = New System.Windows.Forms.Label()
        Me.trkMaxPlanesGround = New System.Windows.Forms.TrackBar()
        Me.lblMaxPlanesGround = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.trkMaxPlanesTotal = New System.Windows.Forms.TrackBar()
        Me.lblMaxPlanes = New System.Windows.Forms.Label()
        CType(Me.trkSpawnUntil, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trkEndGateUntil, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trkMaxPlanesGround, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trkMaxPlanesTotal, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdLoadGame
        '
        Me.cmdLoadGame.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdLoadGame.Location = New System.Drawing.Point(11, 308)
        Me.cmdLoadGame.Name = "cmdLoadGame"
        Me.cmdLoadGame.Size = New System.Drawing.Size(395, 56)
        Me.cmdLoadGame.TabIndex = 0
        Me.cmdLoadGame.Text = "Start Own Game"
        Me.cmdLoadGame.UseVisualStyleBackColor = True
        '
        'trkSpawnUntil
        '
        Me.trkSpawnUntil.LargeChange = 30
        Me.trkSpawnUntil.Location = New System.Drawing.Point(154, 14)
        Me.trkSpawnUntil.Maximum = 180
        Me.trkSpawnUntil.Minimum = 15
        Me.trkSpawnUntil.Name = "trkSpawnUntil"
        Me.trkSpawnUntil.Size = New System.Drawing.Size(200, 45)
        Me.trkSpawnUntil.SmallChange = 15
        Me.trkSpawnUntil.TabIndex = 1
        Me.trkSpawnUntil.TickFrequency = 15
        Me.trkSpawnUntil.Value = 60
        '
        'trkEndGateUntil
        '
        Me.trkEndGateUntil.LargeChange = 30
        Me.trkEndGateUntil.Location = New System.Drawing.Point(154, 72)
        Me.trkEndGateUntil.Maximum = 180
        Me.trkEndGateUntil.Minimum = 15
        Me.trkEndGateUntil.Name = "trkEndGateUntil"
        Me.trkEndGateUntil.Size = New System.Drawing.Size(200, 45)
        Me.trkEndGateUntil.SmallChange = 15
        Me.trkEndGateUntil.TabIndex = 2
        Me.trkEndGateUntil.TickFrequency = 15
        Me.trkEndGateUntil.Value = 60
        '
        'lblSpawnUntil
        '
        Me.lblSpawnUntil.AutoSize = True
        Me.lblSpawnUntil.Location = New System.Drawing.Point(360, 14)
        Me.lblSpawnUntil.Name = "lblSpawnUntil"
        Me.lblSpawnUntil.Size = New System.Drawing.Size(23, 13)
        Me.lblSpawnUntil.TabIndex = 3
        Me.lblSpawnUntil.Text = "min"
        '
        'lblEndGateUntil
        '
        Me.lblEndGateUntil.AutoSize = True
        Me.lblEndGateUntil.Location = New System.Drawing.Point(360, 72)
        Me.lblEndGateUntil.Name = "lblEndGateUntil"
        Me.lblEndGateUntil.Size = New System.Drawing.Size(23, 13)
        Me.lblEndGateUntil.TabIndex = 3
        Me.lblEndGateUntil.Text = "min"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(69, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "spawn for:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(58, 72)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(67, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "end gate for:"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'txtClientMessages
        '
        Me.txtClientMessages.Location = New System.Drawing.Point(12, 551)
        Me.txtClientMessages.Multiline = True
        Me.txtClientMessages.Name = "txtClientMessages"
        Me.txtClientMessages.Size = New System.Drawing.Size(186, 53)
        Me.txtClientMessages.TabIndex = 8
        '
        'txtMessageFromServer
        '
        Me.txtMessageFromServer.Location = New System.Drawing.Point(6, 414)
        Me.txtMessageFromServer.Name = "txtMessageFromServer"
        Me.txtMessageFromServer.Size = New System.Drawing.Size(186, 20)
        Me.txtMessageFromServer.TabIndex = 9
        '
        'cmdSendToClients
        '
        Me.cmdSendToClients.Location = New System.Drawing.Point(203, 411)
        Me.cmdSendToClients.Name = "cmdSendToClients"
        Me.cmdSendToClients.Size = New System.Drawing.Size(75, 23)
        Me.cmdSendToClients.TabIndex = 10
        Me.cmdSendToClients.Text = "Button2"
        Me.cmdSendToClients.UseVisualStyleBackColor = True
        '
        'txtMessagesServer
        '
        Me.txtMessagesServer.Location = New System.Drawing.Point(6, 440)
        Me.txtMessagesServer.Multiline = True
        Me.txtMessagesServer.Name = "txtMessagesServer"
        Me.txtMessagesServer.Size = New System.Drawing.Size(186, 79)
        Me.txtMessagesServer.TabIndex = 11
        '
        'txtMesssageFromClient
        '
        Me.txtMesssageFromClient.Location = New System.Drawing.Point(6, 610)
        Me.txtMesssageFromClient.Name = "txtMesssageFromClient"
        Me.txtMesssageFromClient.Size = New System.Drawing.Size(186, 20)
        Me.txtMesssageFromClient.TabIndex = 13
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(204, 607)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(75, 23)
        Me.Button3.TabIndex = 14
        Me.Button3.Text = "Button3"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'tmrServer
        '
        Me.tmrServer.Interval = 1000
        '
        'txtIP
        '
        Me.txtIP.Location = New System.Drawing.Point(6, 525)
        Me.txtIP.Name = "txtIP"
        Me.txtIP.Size = New System.Drawing.Size(100, 20)
        Me.txtIP.TabIndex = 15
        Me.txtIP.Text = "127.0.0.1"
        '
        'cmdConnectToServer
        '
        Me.cmdConnectToServer.Location = New System.Drawing.Point(203, 522)
        Me.cmdConnectToServer.Name = "cmdConnectToServer"
        Me.cmdConnectToServer.Size = New System.Drawing.Size(75, 23)
        Me.cmdConnectToServer.TabIndex = 16
        Me.cmdConnectToServer.Text = "Connect"
        Me.cmdConnectToServer.UseVisualStyleBackColor = True
        '
        'tmrClient
        '
        Me.tmrClient.Interval = 1000
        '
        'cmdJoinGame
        '
        Me.cmdJoinGame.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdJoinGame.Location = New System.Drawing.Point(261, 370)
        Me.cmdJoinGame.Name = "cmdJoinGame"
        Me.cmdJoinGame.Size = New System.Drawing.Size(145, 23)
        Me.cmdJoinGame.TabIndex = 17
        Me.cmdJoinGame.Text = "Join Game"
        Me.cmdJoinGame.UseVisualStyleBackColor = True
        '
        'txtJoinIP
        '
        Me.txtJoinIP.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.txtJoinIP.Location = New System.Drawing.Point(11, 373)
        Me.txtJoinIP.Name = "txtJoinIP"
        Me.txtJoinIP.Size = New System.Drawing.Size(100, 20)
        Me.txtJoinIP.TabIndex = 18
        Me.txtJoinIP.Text = "127.0.0.1"
        '
        'txtPort
        '
        Me.txtPort.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.txtPort.Location = New System.Drawing.Point(154, 372)
        Me.txtPort.Name = "txtPort"
        Me.txtPort.Size = New System.Drawing.Size(89, 20)
        Me.txtPort.TabIndex = 19
        Me.txtPort.Text = "4616"
        '
        'cboAirport
        '
        Me.cboAirport.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cboAirport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAirport.FormattingEnabled = True
        Me.cboAirport.Location = New System.Drawing.Point(12, 281)
        Me.cboAirport.Name = "cboAirport"
        Me.cboAirport.Size = New System.Drawing.Size(394, 21)
        Me.cboAirport.TabIndex = 20
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(12, 144)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(143, 13)
        Me.label3.TabIndex = 21
        Me.label3.Text = "max number of gated planes:"
        Me.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'trkMaxPlanesGround
        '
        Me.trkMaxPlanesGround.Location = New System.Drawing.Point(154, 144)
        Me.trkMaxPlanesGround.Maximum = 200
        Me.trkMaxPlanesGround.Name = "trkMaxPlanesGround"
        Me.trkMaxPlanesGround.Size = New System.Drawing.Size(200, 45)
        Me.trkMaxPlanesGround.TabIndex = 22
        Me.trkMaxPlanesGround.TickFrequency = 5
        Me.trkMaxPlanesGround.Value = 100
        '
        'lblMaxPlanesGround
        '
        Me.lblMaxPlanesGround.AutoSize = True
        Me.lblMaxPlanesGround.Location = New System.Drawing.Point(360, 144)
        Me.lblMaxPlanesGround.Name = "lblMaxPlanesGround"
        Me.lblMaxPlanesGround.Size = New System.Drawing.Size(38, 13)
        Me.lblMaxPlanesGround.TabIndex = 23
        Me.lblMaxPlanesGround.Text = "planes"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(42, 176)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(113, 13)
        Me.Label4.TabIndex = 24
        Me.Label4.Text = "max number of planes:"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'trkMaxPlanesTotal
        '
        Me.trkMaxPlanesTotal.Location = New System.Drawing.Point(154, 165)
        Me.trkMaxPlanesTotal.Maximum = 200
        Me.trkMaxPlanesTotal.Minimum = 1
        Me.trkMaxPlanesTotal.Name = "trkMaxPlanesTotal"
        Me.trkMaxPlanesTotal.Size = New System.Drawing.Size(200, 45)
        Me.trkMaxPlanesTotal.TabIndex = 25
        Me.trkMaxPlanesTotal.TickFrequency = 5
        Me.trkMaxPlanesTotal.TickStyle = System.Windows.Forms.TickStyle.TopLeft
        Me.trkMaxPlanesTotal.Value = 100
        '
        'lblMaxPlanes
        '
        Me.lblMaxPlanes.AutoSize = True
        Me.lblMaxPlanes.Location = New System.Drawing.Point(360, 176)
        Me.lblMaxPlanes.Name = "lblMaxPlanes"
        Me.lblMaxPlanes.Size = New System.Drawing.Size(38, 13)
        Me.lblMaxPlanes.TabIndex = 26
        Me.lblMaxPlanes.Text = "planes"
        '
        'frmMainMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(418, 403)
        Me.Controls.Add(Me.lblMaxPlanes)
        Me.Controls.Add(Me.trkMaxPlanesTotal)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lblMaxPlanesGround)
        Me.Controls.Add(Me.trkMaxPlanesGround)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.cboAirport)
        Me.Controls.Add(Me.txtPort)
        Me.Controls.Add(Me.txtJoinIP)
        Me.Controls.Add(Me.cmdJoinGame)
        Me.Controls.Add(Me.cmdConnectToServer)
        Me.Controls.Add(Me.txtIP)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.txtMesssageFromClient)
        Me.Controls.Add(Me.txtMessagesServer)
        Me.Controls.Add(Me.cmdSendToClients)
        Me.Controls.Add(Me.txtMessageFromServer)
        Me.Controls.Add(Me.txtClientMessages)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblEndGateUntil)
        Me.Controls.Add(Me.lblSpawnUntil)
        Me.Controls.Add(Me.trkEndGateUntil)
        Me.Controls.Add(Me.trkSpawnUntil)
        Me.Controls.Add(Me.cmdLoadGame)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmMainMenu"
        Me.Text = "ATC - Main Menu"
        CType(Me.trkSpawnUntil, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trkEndGateUntil, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trkMaxPlanesGround, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trkMaxPlanesTotal, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents cmdLoadGame As Button
    Friend WithEvents trkSpawnUntil As TrackBar
    Friend WithEvents trkEndGateUntil As TrackBar
    Friend WithEvents lblSpawnUntil As Label
    Friend WithEvents lblEndGateUntil As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtClientMessages As TextBox
    Friend WithEvents txtMessageFromServer As TextBox
    Friend WithEvents cmdSendToClients As Button
    Friend WithEvents txtMessagesServer As TextBox
    Friend WithEvents txtMesssageFromClient As TextBox
    Friend WithEvents Button3 As Button
    Friend WithEvents tmrServer As Timer
    Friend WithEvents txtIP As TextBox
    Friend WithEvents cmdConnectToServer As Button
    Friend WithEvents tmrClient As Timer
    Friend WithEvents cmdJoinGame As Button
    Friend WithEvents txtJoinIP As TextBox
    Friend WithEvents txtPort As TextBox
    Friend WithEvents cboAirport As ComboBox
    Friend WithEvents label3 As Label
    Friend WithEvents trkMaxPlanesGround As TrackBar
    Friend WithEvents lblMaxPlanesGround As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents trkMaxPlanesTotal As TrackBar
    Friend WithEvents lblMaxPlanes As Label
End Class
