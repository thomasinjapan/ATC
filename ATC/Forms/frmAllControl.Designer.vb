<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmAllControl
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.cmdGroundPushbackApproved = New System.Windows.Forms.Button()
        Me.cmdTowerLineUpAndWait = New System.Windows.Forms.Button()
        Me.cmdTowerTakeOff = New System.Windows.Forms.Button()
        Me.cmdTowerLineUpandTakeOff = New System.Windows.Forms.Button()
        Me.cmdSpecialDespawn = New System.Windows.Forms.Button()
        Me.cmdSpecialSpawn = New System.Windows.Forms.Button()
        Me.cmdAppDepMakeShortApproach = New System.Windows.Forms.Button()
        Me.cmdTowerCleardToLand = New System.Windows.Forms.Button()
        Me.cmdAppDepAdjustSpeed = New System.Windows.Forms.Button()
        Me.trkAppDepSpeed = New System.Windows.Forms.TrackBar()
        Me.txtAppDepHeading = New System.Windows.Forms.TextBox()
        Me.cmdAppDepHeading = New System.Windows.Forms.Button()
        Me.lblAppDepHeading = New System.Windows.Forms.Label()
        Me.trkAppDepHeading = New System.Windows.Forms.TrackBar()
        Me.cmdTowerContactDeparture = New System.Windows.Forms.Button()
        Me.cmdGroundContactTower = New System.Windows.Forms.Button()
        Me.cmdTowerContactGround = New System.Windows.Forms.Button()
        Me.tabControls = New System.Windows.Forms.TabControl()
        Me.pagGround = New System.Windows.Forms.TabPage()
        Me.cmdGroundChangeTaxi = New System.Windows.Forms.Button()
        Me.cmdGroundClearVia = New System.Windows.Forms.Button()
        Me.lblGroundTaxiTo = New System.Windows.Forms.Label()
        Me.lblVia = New System.Windows.Forms.Label()
        Me.txtVia = New System.Windows.Forms.TextBox()
        Me.lstGround = New ATC.ctlStripeList()
        Me.pagTower = New System.Windows.Forms.TabPage()
        Me.cltWindRose = New ATC.ctlWindRose()
        Me.lblTowerExitVia = New System.Windows.Forms.Label()
        Me.cmdTowerExitVia = New System.Windows.Forms.Button()
        Me.lblTowerCurrentRunway = New System.Windows.Forms.Label()
        Me.cmdTowerExpectRunway = New System.Windows.Forms.Button()
        Me.cmdTowerEnterFinal = New System.Windows.Forms.Button()
        Me.lstTowerOpenedRunwaysDeparture = New System.Windows.Forms.CheckedListBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblAvailabeRunways = New System.Windows.Forms.Label()
        Me.lstTowerOpenedRunwaysArrival = New System.Windows.Forms.CheckedListBox()
        Me.nudTowerAltitude = New System.Windows.Forms.NumericUpDown()
        Me.cmdTowerAltitude = New System.Windows.Forms.Button()
        Me.trkTowerAltitude = New System.Windows.Forms.TrackBar()
        Me.cmdTowerAdjustSpeed = New System.Windows.Forms.Button()
        Me.trkTowerSpeed = New System.Windows.Forms.TrackBar()
        Me.cmdTowerHeading = New System.Windows.Forms.Button()
        Me.txtTowerHeading = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.trkTowerHeading = New System.Windows.Forms.TrackBar()
        Me.cmdTowerMakeShortApproach = New System.Windows.Forms.Button()
        Me.cmdTowerHold = New System.Windows.Forms.Button()
        Me.cmdTowerContinueTaxi = New System.Windows.Forms.Button()
        Me.cmdTowerContactApproach = New System.Windows.Forms.Button()
        Me.lstTower = New ATC.ctlStripeList()
        Me.pagAppDep = New System.Windows.Forms.TabPage()
        Me.cmdAppDepEnterSTARVia = New System.Windows.Forms.Button()
        Me.lblArrDepCurrentRunway = New System.Windows.Forms.Label()
        Me.cmdAppDepExpectRunway = New System.Windows.Forms.Button()
        Me.lblArrDepCurrentNavPoint = New System.Windows.Forms.Label()
        Me.cmdAppDepHeadTo = New System.Windows.Forms.Button()
        Me.cmdAppDepEnterSID = New System.Windows.Forms.Button()
        Me.lblArrDepCurrentSID = New System.Windows.Forms.Label()
        Me.lblArrDepCurrentStar = New System.Windows.Forms.Label()
        Me.cmdAppDepEnterSTAR = New System.Windows.Forms.Button()
        Me.cmdAppDepEnterFinal = New System.Windows.Forms.Button()
        Me.nudAppDepAltitude = New System.Windows.Forms.NumericUpDown()
        Me.cmdAppDepAltitude = New System.Windows.Forms.Button()
        Me.trkAppDepAltitude = New System.Windows.Forms.TrackBar()
        Me.cmdAppDepContactTower = New System.Windows.Forms.Button()
        Me.lstAppDep = New ATC.ctlStripeList()
        Me.pagGame = New System.Windows.Forms.TabPage()
        Me.lblMillisecondsBetweenTicks = New System.Windows.Forms.Label()
        Me.lblGatedAtTerminal = New System.Windows.Forms.Label()
        Me.dtpEndGateUntil = New System.Windows.Forms.DateTimePicker()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.dtpSpawnUntil = New System.Windows.Forms.DateTimePicker()
        Me.lblMaxPlanes = New System.Windows.Forms.Label()
        Me.trkMaxPlanes = New System.Windows.Forms.TrackBar()
        Me.lblDepartedPlanes = New System.Windows.Forms.Label()
        Me.lblGatedPlanes = New System.Windows.Forms.Label()
        Me.lblTakenOffPlanes = New System.Windows.Forms.Label()
        Me.lblLandedPlanes = New System.Windows.Forms.Label()
        Me.lblCrashedPlanes = New System.Windows.Forms.Label()
        Me.cmdShowTRACONRadar = New System.Windows.Forms.Button()
        Me.cmdShowAppDepRadar = New System.Windows.Forms.Button()
        Me.cmdShowTowerRadar = New System.Windows.Forms.Button()
        Me.cmdShowGroundRadar = New System.Windows.Forms.Button()
        Me.lstGame = New ATC.ctlStripeList()
        Me.tmrTick = New System.Windows.Forms.Timer(Me.components)
        Me.cmsAppDepSTAR = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmsAppDepSID = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmsAppDepNavPoints = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmsAppDepRunways = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmsTowerRunways = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmsTowerExitVia = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmsGroundTaxiTo = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmsAppDepSTARvia = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmdGroundTaxiTo = New System.Windows.Forms.Button()
        Me.cmdGroundContinueTaxi = New System.Windows.Forms.Button()
        Me.cmdGroundHold = New System.Windows.Forms.Button()
        CType(Me.trkAppDepSpeed, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trkAppDepHeading, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabControls.SuspendLayout()
        Me.pagGround.SuspendLayout()
        Me.pagTower.SuspendLayout()
        CType(Me.nudTowerAltitude, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trkTowerAltitude, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trkTowerSpeed, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trkTowerHeading, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pagAppDep.SuspendLayout()
        CType(Me.nudAppDepAltitude, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trkAppDepAltitude, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pagGame.SuspendLayout()
        CType(Me.trkMaxPlanes, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdGroundPushbackApproved
        '
        Me.cmdGroundPushbackApproved.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdGroundPushbackApproved.Location = New System.Drawing.Point(512, 6)
        Me.cmdGroundPushbackApproved.Name = "cmdGroundPushbackApproved"
        Me.cmdGroundPushbackApproved.Size = New System.Drawing.Size(144, 23)
        Me.cmdGroundPushbackApproved.TabIndex = 2
        Me.cmdGroundPushbackApproved.Text = "pushback approved | ⬇"
        Me.cmdGroundPushbackApproved.UseVisualStyleBackColor = True
        '
        'cmdTowerLineUpAndWait
        '
        Me.cmdTowerLineUpAndWait.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdTowerLineUpAndWait.Location = New System.Drawing.Point(333, 93)
        Me.cmdTowerLineUpAndWait.Name = "cmdTowerLineUpAndWait"
        Me.cmdTowerLineUpAndWait.Size = New System.Drawing.Size(158, 22)
        Me.cmdTowerLineUpAndWait.TabIndex = 8
        Me.cmdTowerLineUpAndWait.Text = "line up and wait | ➡"
        Me.cmdTowerLineUpAndWait.UseVisualStyleBackColor = True
        '
        'cmdTowerTakeOff
        '
        Me.cmdTowerTakeOff.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdTowerTakeOff.Location = New System.Drawing.Point(333, 121)
        Me.cmdTowerTakeOff.Name = "cmdTowerTakeOff"
        Me.cmdTowerTakeOff.Size = New System.Drawing.Size(158, 23)
        Me.cmdTowerTakeOff.TabIndex = 9
        Me.cmdTowerTakeOff.Text = "take off | ↗"
        Me.cmdTowerTakeOff.UseVisualStyleBackColor = True
        '
        'cmdTowerLineUpandTakeOff
        '
        Me.cmdTowerLineUpandTakeOff.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdTowerLineUpandTakeOff.Location = New System.Drawing.Point(333, 64)
        Me.cmdTowerLineUpandTakeOff.Name = "cmdTowerLineUpandTakeOff"
        Me.cmdTowerLineUpandTakeOff.Size = New System.Drawing.Size(158, 23)
        Me.cmdTowerLineUpandTakeOff.TabIndex = 10
        Me.cmdTowerLineUpandTakeOff.Text = "line up and take off | ➡↗"
        Me.cmdTowerLineUpandTakeOff.UseVisualStyleBackColor = True
        '
        'cmdSpecialDespawn
        '
        Me.cmdSpecialDespawn.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSpecialDespawn.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSpecialDespawn.Location = New System.Drawing.Point(432, 6)
        Me.cmdSpecialDespawn.Name = "cmdSpecialDespawn"
        Me.cmdSpecialDespawn.Size = New System.Drawing.Size(362, 23)
        Me.cmdSpecialDespawn.TabIndex = 11
        Me.cmdSpecialDespawn.Text = "despawn selected | ✂"
        Me.cmdSpecialDespawn.UseVisualStyleBackColor = True
        '
        'cmdSpecialSpawn
        '
        Me.cmdSpecialSpawn.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSpecialSpawn.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSpecialSpawn.Location = New System.Drawing.Point(432, 35)
        Me.cmdSpecialSpawn.Name = "cmdSpecialSpawn"
        Me.cmdSpecialSpawn.Size = New System.Drawing.Size(362, 23)
        Me.cmdSpecialSpawn.TabIndex = 12
        Me.cmdSpecialSpawn.Text = "Spawn new | ✨"
        Me.cmdSpecialSpawn.UseVisualStyleBackColor = True
        '
        'cmdAppDepMakeShortApproach
        '
        Me.cmdAppDepMakeShortApproach.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdAppDepMakeShortApproach.Location = New System.Drawing.Point(333, 328)
        Me.cmdAppDepMakeShortApproach.Name = "cmdAppDepMakeShortApproach"
        Me.cmdAppDepMakeShortApproach.Size = New System.Drawing.Size(182, 26)
        Me.cmdAppDepMakeShortApproach.TabIndex = 23
        Me.cmdAppDepMakeShortApproach.Text = "Make Short Approach | 🛬💨"
        Me.cmdAppDepMakeShortApproach.UseVisualStyleBackColor = True
        '
        'cmdTowerCleardToLand
        '
        Me.cmdTowerCleardToLand.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdTowerCleardToLand.Location = New System.Drawing.Point(514, 306)
        Me.cmdTowerCleardToLand.Name = "cmdTowerCleardToLand"
        Me.cmdTowerCleardToLand.Size = New System.Drawing.Size(175, 48)
        Me.cmdTowerCleardToLand.TabIndex = 22
        Me.cmdTowerCleardToLand.Text = "Cleared To Land | ♈✔"
        Me.cmdTowerCleardToLand.UseVisualStyleBackColor = True
        '
        'cmdAppDepAdjustSpeed
        '
        Me.cmdAppDepAdjustSpeed.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAppDepAdjustSpeed.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdAppDepAdjustSpeed.Location = New System.Drawing.Point(529, 163)
        Me.cmdAppDepAdjustSpeed.Name = "cmdAppDepAdjustSpeed"
        Me.cmdAppDepAdjustSpeed.Size = New System.Drawing.Size(231, 23)
        Me.cmdAppDepAdjustSpeed.TabIndex = 21
        Me.cmdAppDepAdjustSpeed.Text = "Adjust Speed | ⏩⏪"
        Me.cmdAppDepAdjustSpeed.UseVisualStyleBackColor = True
        '
        'trkAppDepSpeed
        '
        Me.trkAppDepSpeed.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.trkAppDepSpeed.Location = New System.Drawing.Point(529, 112)
        Me.trkAppDepSpeed.Maximum = 490
        Me.trkAppDepSpeed.Minimum = 140
        Me.trkAppDepSpeed.Name = "trkAppDepSpeed"
        Me.trkAppDepSpeed.Size = New System.Drawing.Size(231, 45)
        Me.trkAppDepSpeed.TabIndex = 20
        Me.trkAppDepSpeed.Value = 140
        '
        'txtAppDepHeading
        '
        Me.txtAppDepHeading.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtAppDepHeading.Location = New System.Drawing.Point(692, 54)
        Me.txtAppDepHeading.Name = "txtAppDepHeading"
        Me.txtAppDepHeading.Size = New System.Drawing.Size(68, 20)
        Me.txtAppDepHeading.TabIndex = 19
        '
        'cmdAppDepHeading
        '
        Me.cmdAppDepHeading.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAppDepHeading.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdAppDepHeading.Location = New System.Drawing.Point(529, 80)
        Me.cmdAppDepHeading.Name = "cmdAppDepHeading"
        Me.cmdAppDepHeading.Size = New System.Drawing.Size(231, 23)
        Me.cmdAppDepHeading.TabIndex = 18
        Me.cmdAppDepHeading.Text = "Change Heading | 🎈⤴"
        Me.cmdAppDepHeading.UseVisualStyleBackColor = True
        '
        'lblAppDepHeading
        '
        Me.lblAppDepHeading.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblAppDepHeading.Location = New System.Drawing.Point(526, 54)
        Me.lblAppDepHeading.Name = "lblAppDepHeading"
        Me.lblAppDepHeading.Size = New System.Drawing.Size(154, 18)
        Me.lblAppDepHeading.TabIndex = 16
        Me.lblAppDepHeading.Text = "Targetheading:"
        Me.lblAppDepHeading.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'trkAppDepHeading
        '
        Me.trkAppDepHeading.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.trkAppDepHeading.LargeChange = 10
        Me.trkAppDepHeading.Location = New System.Drawing.Point(526, 6)
        Me.trkAppDepHeading.Maximum = 360
        Me.trkAppDepHeading.Minimum = 1
        Me.trkAppDepHeading.Name = "trkAppDepHeading"
        Me.trkAppDepHeading.Size = New System.Drawing.Size(234, 45)
        Me.trkAppDepHeading.TabIndex = 15
        Me.trkAppDepHeading.TickStyle = System.Windows.Forms.TickStyle.Both
        Me.trkAppDepHeading.Value = 360
        '
        'cmdTowerContactDeparture
        '
        Me.cmdTowerContactDeparture.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdTowerContactDeparture.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdTowerContactDeparture.Location = New System.Drawing.Point(497, 418)
        Me.cmdTowerContactDeparture.Name = "cmdTowerContactDeparture"
        Me.cmdTowerContactDeparture.Size = New System.Drawing.Size(152, 23)
        Me.cmdTowerContactDeparture.TabIndex = 2
        Me.cmdTowerContactDeparture.Text = "Contact Departure | 💬🛫"
        Me.cmdTowerContactDeparture.UseVisualStyleBackColor = True
        '
        'cmdGroundContactTower
        '
        Me.cmdGroundContactTower.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdGroundContactTower.Location = New System.Drawing.Point(512, 173)
        Me.cmdGroundContactTower.Name = "cmdGroundContactTower"
        Me.cmdGroundContactTower.Size = New System.Drawing.Size(144, 23)
        Me.cmdGroundContactTower.TabIndex = 1
        Me.cmdGroundContactTower.Text = "Contact Tower | 💬🗼"
        Me.cmdGroundContactTower.UseVisualStyleBackColor = True
        '
        'cmdTowerContactGround
        '
        Me.cmdTowerContactGround.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdTowerContactGround.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdTowerContactGround.Location = New System.Drawing.Point(336, 418)
        Me.cmdTowerContactGround.Name = "cmdTowerContactGround"
        Me.cmdTowerContactGround.Size = New System.Drawing.Size(155, 23)
        Me.cmdTowerContactGround.TabIndex = 0
        Me.cmdTowerContactGround.Text = "Contact Ground  | 💬🚗"
        Me.cmdTowerContactGround.UseVisualStyleBackColor = True
        '
        'tabControls
        '
        Me.tabControls.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabControls.Controls.Add(Me.pagGround)
        Me.tabControls.Controls.Add(Me.pagTower)
        Me.tabControls.Controls.Add(Me.pagAppDep)
        Me.tabControls.Controls.Add(Me.pagGame)
        Me.tabControls.Location = New System.Drawing.Point(12, 12)
        Me.tabControls.Name = "tabControls"
        Me.tabControls.SelectedIndex = 0
        Me.tabControls.Size = New System.Drawing.Size(872, 473)
        Me.tabControls.TabIndex = 19
        '
        'pagGround
        '
        Me.pagGround.Controls.Add(Me.cmdGroundChangeTaxi)
        Me.pagGround.Controls.Add(Me.cmdGroundClearVia)
        Me.pagGround.Controls.Add(Me.lblGroundTaxiTo)
        Me.pagGround.Controls.Add(Me.cmdGroundTaxiTo)
        Me.pagGround.Controls.Add(Me.lblVia)
        Me.pagGround.Controls.Add(Me.txtVia)
        Me.pagGround.Controls.Add(Me.lstGround)
        Me.pagGround.Controls.Add(Me.cmdGroundContinueTaxi)
        Me.pagGround.Controls.Add(Me.cmdGroundPushbackApproved)
        Me.pagGround.Controls.Add(Me.cmdGroundHold)
        Me.pagGround.Controls.Add(Me.cmdGroundContactTower)
        Me.pagGround.Location = New System.Drawing.Point(4, 22)
        Me.pagGround.Name = "pagGround"
        Me.pagGround.Padding = New System.Windows.Forms.Padding(3)
        Me.pagGround.Size = New System.Drawing.Size(864, 447)
        Me.pagGround.TabIndex = 0
        Me.pagGround.Text = "Ground"
        Me.pagGround.UseVisualStyleBackColor = True
        '
        'cmdGroundChangeTaxi
        '
        Me.cmdGroundChangeTaxi.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdGroundChangeTaxi.Location = New System.Drawing.Point(334, 104)
        Me.cmdGroundChangeTaxi.Name = "cmdGroundChangeTaxi"
        Me.cmdGroundChangeTaxi.Size = New System.Drawing.Size(172, 23)
        Me.cmdGroundChangeTaxi.TabIndex = 51
        Me.cmdGroundChangeTaxi.Text = "change taxi path | ➰"
        Me.cmdGroundChangeTaxi.UseVisualStyleBackColor = True
        '
        'cmdGroundClearVia
        '
        Me.cmdGroundClearVia.Location = New System.Drawing.Point(452, 62)
        Me.cmdGroundClearVia.Name = "cmdGroundClearVia"
        Me.cmdGroundClearVia.Size = New System.Drawing.Size(22, 23)
        Me.cmdGroundClearVia.TabIndex = 50
        Me.cmdGroundClearVia.Text = "C"
        Me.cmdGroundClearVia.UseVisualStyleBackColor = True
        '
        'lblGroundTaxiTo
        '
        Me.lblGroundTaxiTo.Location = New System.Drawing.Point(331, 35)
        Me.lblGroundTaxiTo.Name = "lblGroundTaxiTo"
        Me.lblGroundTaxiTo.Size = New System.Drawing.Size(145, 24)
        Me.lblGroundTaxiTo.TabIndex = 49
        Me.lblGroundTaxiTo.Text = "current: -"
        '
        'lblVia
        '
        Me.lblVia.AutoSize = True
        Me.lblVia.Location = New System.Drawing.Point(331, 65)
        Me.lblVia.Name = "lblVia"
        Me.lblVia.Size = New System.Drawing.Size(24, 13)
        Me.lblVia.TabIndex = 10
        Me.lblVia.Text = "via:"
        '
        'txtVia
        '
        Me.txtVia.Location = New System.Drawing.Point(361, 62)
        Me.txtVia.Name = "txtVia"
        Me.txtVia.Size = New System.Drawing.Size(85, 20)
        Me.txtVia.TabIndex = 9
        '
        'lstGround
        '
        Me.lstGround.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstGround.BackColor = System.Drawing.SystemColors.Control
        Me.lstGround.Location = New System.Drawing.Point(3, 6)
        Me.lstGround.Name = "lstGround"
        Me.lstGround.Size = New System.Drawing.Size(321, 435)
        Me.lstGround.TabIndex = 8
        '
        'pagTower
        '
        Me.pagTower.Controls.Add(Me.cltWindRose)
        Me.pagTower.Controls.Add(Me.lblTowerExitVia)
        Me.pagTower.Controls.Add(Me.cmdTowerExitVia)
        Me.pagTower.Controls.Add(Me.lblTowerCurrentRunway)
        Me.pagTower.Controls.Add(Me.cmdTowerExpectRunway)
        Me.pagTower.Controls.Add(Me.cmdTowerEnterFinal)
        Me.pagTower.Controls.Add(Me.lstTowerOpenedRunwaysDeparture)
        Me.pagTower.Controls.Add(Me.Label4)
        Me.pagTower.Controls.Add(Me.lblAvailabeRunways)
        Me.pagTower.Controls.Add(Me.lstTowerOpenedRunwaysArrival)
        Me.pagTower.Controls.Add(Me.nudTowerAltitude)
        Me.pagTower.Controls.Add(Me.cmdTowerAltitude)
        Me.pagTower.Controls.Add(Me.trkTowerAltitude)
        Me.pagTower.Controls.Add(Me.cmdTowerAdjustSpeed)
        Me.pagTower.Controls.Add(Me.trkTowerSpeed)
        Me.pagTower.Controls.Add(Me.cmdTowerHeading)
        Me.pagTower.Controls.Add(Me.txtTowerHeading)
        Me.pagTower.Controls.Add(Me.Label1)
        Me.pagTower.Controls.Add(Me.trkTowerHeading)
        Me.pagTower.Controls.Add(Me.cmdTowerMakeShortApproach)
        Me.pagTower.Controls.Add(Me.cmdTowerHold)
        Me.pagTower.Controls.Add(Me.cmdTowerContinueTaxi)
        Me.pagTower.Controls.Add(Me.cmdTowerContactApproach)
        Me.pagTower.Controls.Add(Me.cmdTowerContactDeparture)
        Me.pagTower.Controls.Add(Me.cmdTowerLineUpAndWait)
        Me.pagTower.Controls.Add(Me.cmdTowerCleardToLand)
        Me.pagTower.Controls.Add(Me.cmdTowerContactGround)
        Me.pagTower.Controls.Add(Me.cmdTowerTakeOff)
        Me.pagTower.Controls.Add(Me.cmdTowerLineUpandTakeOff)
        Me.pagTower.Controls.Add(Me.lstTower)
        Me.pagTower.Location = New System.Drawing.Point(4, 22)
        Me.pagTower.Name = "pagTower"
        Me.pagTower.Padding = New System.Windows.Forms.Padding(3)
        Me.pagTower.Size = New System.Drawing.Size(864, 447)
        Me.pagTower.TabIndex = 1
        Me.pagTower.Text = "Tower"
        Me.pagTower.UseVisualStyleBackColor = True
        '
        'cltWindRose
        '
        Me.cltWindRose.BackColor = System.Drawing.Color.Transparent
        Me.cltWindRose.Location = New System.Drawing.Point(695, 192)
        Me.cltWindRose.Name = "cltWindRose"
        Me.cltWindRose.Size = New System.Drawing.Size(135, 135)
        Me.cltWindRose.TabIndex = 51
        '
        'lblTowerExitVia
        '
        Me.lblTowerExitVia.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTowerExitVia.Location = New System.Drawing.Point(511, 386)
        Me.lblTowerExitVia.Name = "lblTowerExitVia"
        Me.lblTowerExitVia.Size = New System.Drawing.Size(175, 29)
        Me.lblTowerExitVia.TabIndex = 50
        Me.lblTowerExitVia.Text = "exiting via NEXT"
        '
        'cmdTowerExitVia
        '
        Me.cmdTowerExitVia.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdTowerExitVia.Location = New System.Drawing.Point(514, 360)
        Me.cmdTowerExitVia.Name = "cmdTowerExitVia"
        Me.cmdTowerExitVia.Size = New System.Drawing.Size(175, 23)
        Me.cmdTowerExitVia.TabIndex = 49
        Me.cmdTowerExitVia.Text = "exit via | ↰↱"
        Me.cmdTowerExitVia.UseVisualStyleBackColor = True
        '
        'lblTowerCurrentRunway
        '
        Me.lblTowerCurrentRunway.Location = New System.Drawing.Point(514, 222)
        Me.lblTowerCurrentRunway.Name = "lblTowerCurrentRunway"
        Me.lblTowerCurrentRunway.Size = New System.Drawing.Size(175, 24)
        Me.lblTowerCurrentRunway.TabIndex = 48
        Me.lblTowerCurrentRunway.Text = "expected: -"
        '
        'cmdTowerExpectRunway
        '
        Me.cmdTowerExpectRunway.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdTowerExpectRunway.Location = New System.Drawing.Point(514, 196)
        Me.cmdTowerExpectRunway.Name = "cmdTowerExpectRunway"
        Me.cmdTowerExpectRunway.Size = New System.Drawing.Size(175, 23)
        Me.cmdTowerExpectRunway.TabIndex = 47
        Me.cmdTowerExpectRunway.Text = "Expect Runway | 🔛"
        Me.cmdTowerExpectRunway.UseVisualStyleBackColor = True
        '
        'cmdTowerEnterFinal
        '
        Me.cmdTowerEnterFinal.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdTowerEnterFinal.Location = New System.Drawing.Point(514, 249)
        Me.cmdTowerEnterFinal.Name = "cmdTowerEnterFinal"
        Me.cmdTowerEnterFinal.Size = New System.Drawing.Size(175, 23)
        Me.cmdTowerEnterFinal.TabIndex = 46
        Me.cmdTowerEnterFinal.Text = "Enter Final | 🛬"
        Me.cmdTowerEnterFinal.UseVisualStyleBackColor = True
        '
        'lstTowerOpenedRunwaysDeparture
        '
        Me.lstTowerOpenedRunwaysDeparture.CheckOnClick = True
        Me.lstTowerOpenedRunwaysDeparture.FormattingEnabled = True
        Me.lstTowerOpenedRunwaysDeparture.Location = New System.Drawing.Point(333, 179)
        Me.lstTowerOpenedRunwaysDeparture.Name = "lstTowerOpenedRunwaysDeparture"
        Me.lstTowerOpenedRunwaysDeparture.Size = New System.Drawing.Size(158, 94)
        Me.lstTowerOpenedRunwaysDeparture.TabIndex = 45
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(333, 163)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(99, 13)
        Me.Label4.TabIndex = 44
        Me.Label4.Text = "available Departure"
        '
        'lblAvailabeRunways
        '
        Me.lblAvailabeRunways.AutoSize = True
        Me.lblAvailabeRunways.Location = New System.Drawing.Point(333, 283)
        Me.lblAvailabeRunways.Name = "lblAvailabeRunways"
        Me.lblAvailabeRunways.Size = New System.Drawing.Size(81, 13)
        Me.lblAvailabeRunways.TabIndex = 43
        Me.lblAvailabeRunways.Text = "available Arrival"
        '
        'lstTowerOpenedRunwaysArrival
        '
        Me.lstTowerOpenedRunwaysArrival.CheckOnClick = True
        Me.lstTowerOpenedRunwaysArrival.FormattingEnabled = True
        Me.lstTowerOpenedRunwaysArrival.Location = New System.Drawing.Point(333, 299)
        Me.lstTowerOpenedRunwaysArrival.Name = "lstTowerOpenedRunwaysArrival"
        Me.lstTowerOpenedRunwaysArrival.Size = New System.Drawing.Size(158, 94)
        Me.lstTowerOpenedRunwaysArrival.TabIndex = 42
        '
        'nudTowerAltitude
        '
        Me.nudTowerAltitude.Increment = New Decimal(New Integer() {100, 0, 0, 0})
        Me.nudTowerAltitude.Location = New System.Drawing.Point(695, 137)
        Me.nudTowerAltitude.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudTowerAltitude.Name = "nudTowerAltitude"
        Me.nudTowerAltitude.Size = New System.Drawing.Size(155, 20)
        Me.nudTowerAltitude.TabIndex = 39
        Me.nudTowerAltitude.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudTowerAltitude.ThousandsSeparator = True
        '
        'cmdTowerAltitude
        '
        Me.cmdTowerAltitude.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdTowerAltitude.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdTowerAltitude.Location = New System.Drawing.Point(695, 163)
        Me.cmdTowerAltitude.Name = "cmdTowerAltitude"
        Me.cmdTowerAltitude.Size = New System.Drawing.Size(155, 23)
        Me.cmdTowerAltitude.TabIndex = 38
        Me.cmdTowerAltitude.Text = "Confirm Altitude | ⏫⏬"
        Me.cmdTowerAltitude.UseVisualStyleBackColor = True
        '
        'trkTowerAltitude
        '
        Me.trkTowerAltitude.LargeChange = 500
        Me.trkTowerAltitude.Location = New System.Drawing.Point(805, 6)
        Me.trkTowerAltitude.Maximum = 10000
        Me.trkTowerAltitude.Name = "trkTowerAltitude"
        Me.trkTowerAltitude.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.trkTowerAltitude.Size = New System.Drawing.Size(45, 125)
        Me.trkTowerAltitude.SmallChange = 50
        Me.trkTowerAltitude.TabIndex = 36
        Me.trkTowerAltitude.TickFrequency = 100
        Me.trkTowerAltitude.TickStyle = System.Windows.Forms.TickStyle.TopLeft
        '
        'cmdTowerAdjustSpeed
        '
        Me.cmdTowerAdjustSpeed.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdTowerAdjustSpeed.Location = New System.Drawing.Point(514, 163)
        Me.cmdTowerAdjustSpeed.Name = "cmdTowerAdjustSpeed"
        Me.cmdTowerAdjustSpeed.Size = New System.Drawing.Size(175, 23)
        Me.cmdTowerAdjustSpeed.TabIndex = 34
        Me.cmdTowerAdjustSpeed.Text = "Adjust Speed | ⏩⏪"
        Me.cmdTowerAdjustSpeed.UseVisualStyleBackColor = True
        '
        'trkTowerSpeed
        '
        Me.trkTowerSpeed.Location = New System.Drawing.Point(514, 112)
        Me.trkTowerSpeed.Maximum = 490
        Me.trkTowerSpeed.Minimum = 140
        Me.trkTowerSpeed.Name = "trkTowerSpeed"
        Me.trkTowerSpeed.Size = New System.Drawing.Size(175, 45)
        Me.trkTowerSpeed.TabIndex = 33
        Me.trkTowerSpeed.Value = 140
        '
        'cmdTowerHeading
        '
        Me.cmdTowerHeading.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdTowerHeading.Location = New System.Drawing.Point(514, 83)
        Me.cmdTowerHeading.Name = "cmdTowerHeading"
        Me.cmdTowerHeading.Size = New System.Drawing.Size(175, 23)
        Me.cmdTowerHeading.TabIndex = 32
        Me.cmdTowerHeading.Text = "Change Heading | 🎈⤴"
        Me.cmdTowerHeading.UseVisualStyleBackColor = True
        '
        'txtTowerHeading
        '
        Me.txtTowerHeading.Location = New System.Drawing.Point(623, 57)
        Me.txtTowerHeading.Name = "txtTowerHeading"
        Me.txtTowerHeading.Size = New System.Drawing.Size(66, 20)
        Me.txtTowerHeading.TabIndex = 31
        '
        'Label1
        '
        Me.Label1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label1.Location = New System.Drawing.Point(514, 54)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(153, 18)
        Me.Label1.TabIndex = 30
        Me.Label1.Text = "Targetheading:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'trkTowerHeading
        '
        Me.trkTowerHeading.LargeChange = 10
        Me.trkTowerHeading.Location = New System.Drawing.Point(514, 6)
        Me.trkTowerHeading.Maximum = 360
        Me.trkTowerHeading.Minimum = 1
        Me.trkTowerHeading.Name = "trkTowerHeading"
        Me.trkTowerHeading.Size = New System.Drawing.Size(175, 45)
        Me.trkTowerHeading.TabIndex = 29
        Me.trkTowerHeading.TickStyle = System.Windows.Forms.TickStyle.Both
        Me.trkTowerHeading.Value = 360
        '
        'cmdTowerMakeShortApproach
        '
        Me.cmdTowerMakeShortApproach.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdTowerMakeShortApproach.Location = New System.Drawing.Point(514, 278)
        Me.cmdTowerMakeShortApproach.Name = "cmdTowerMakeShortApproach"
        Me.cmdTowerMakeShortApproach.Size = New System.Drawing.Size(175, 23)
        Me.cmdTowerMakeShortApproach.TabIndex = 28
        Me.cmdTowerMakeShortApproach.Text = "Make Short Approach | 🛬💨"
        Me.cmdTowerMakeShortApproach.UseVisualStyleBackColor = True
        '
        'cmdTowerHold
        '
        Me.cmdTowerHold.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdTowerHold.Location = New System.Drawing.Point(333, 35)
        Me.cmdTowerHold.Name = "cmdTowerHold"
        Me.cmdTowerHold.Size = New System.Drawing.Size(158, 23)
        Me.cmdTowerHold.TabIndex = 27
        Me.cmdTowerHold.Text = "hold position | ⛔"
        Me.cmdTowerHold.UseVisualStyleBackColor = True
        '
        'cmdTowerContinueTaxi
        '
        Me.cmdTowerContinueTaxi.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdTowerContinueTaxi.Location = New System.Drawing.Point(333, 6)
        Me.cmdTowerContinueTaxi.Name = "cmdTowerContinueTaxi"
        Me.cmdTowerContinueTaxi.Size = New System.Drawing.Size(158, 23)
        Me.cmdTowerContinueTaxi.TabIndex = 26
        Me.cmdTowerContinueTaxi.Text = "continue taxi | ✔"
        Me.cmdTowerContinueTaxi.UseVisualStyleBackColor = True
        '
        'cmdTowerContactApproach
        '
        Me.cmdTowerContactApproach.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdTowerContactApproach.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdTowerContactApproach.Location = New System.Drawing.Point(655, 418)
        Me.cmdTowerContactApproach.Name = "cmdTowerContactApproach"
        Me.cmdTowerContactApproach.Size = New System.Drawing.Size(175, 23)
        Me.cmdTowerContactApproach.TabIndex = 23
        Me.cmdTowerContactApproach.Text = "Contact Approach  | 💬🛬"
        Me.cmdTowerContactApproach.UseVisualStyleBackColor = True
        '
        'lstTower
        '
        Me.lstTower.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstTower.BackColor = System.Drawing.SystemColors.Control
        Me.lstTower.Location = New System.Drawing.Point(6, 6)
        Me.lstTower.Name = "lstTower"
        Me.lstTower.Size = New System.Drawing.Size(321, 435)
        Me.lstTower.TabIndex = 35
        '
        'pagAppDep
        '
        Me.pagAppDep.Controls.Add(Me.cmdAppDepEnterSTARVia)
        Me.pagAppDep.Controls.Add(Me.lblArrDepCurrentRunway)
        Me.pagAppDep.Controls.Add(Me.cmdAppDepExpectRunway)
        Me.pagAppDep.Controls.Add(Me.lblArrDepCurrentNavPoint)
        Me.pagAppDep.Controls.Add(Me.cmdAppDepHeadTo)
        Me.pagAppDep.Controls.Add(Me.cmdAppDepEnterSID)
        Me.pagAppDep.Controls.Add(Me.lblArrDepCurrentSID)
        Me.pagAppDep.Controls.Add(Me.lblArrDepCurrentStar)
        Me.pagAppDep.Controls.Add(Me.cmdAppDepEnterSTAR)
        Me.pagAppDep.Controls.Add(Me.cmdAppDepEnterFinal)
        Me.pagAppDep.Controls.Add(Me.nudAppDepAltitude)
        Me.pagAppDep.Controls.Add(Me.cmdAppDepAltitude)
        Me.pagAppDep.Controls.Add(Me.trkAppDepAltitude)
        Me.pagAppDep.Controls.Add(Me.cmdAppDepContactTower)
        Me.pagAppDep.Controls.Add(Me.cmdAppDepMakeShortApproach)
        Me.pagAppDep.Controls.Add(Me.cmdAppDepAdjustSpeed)
        Me.pagAppDep.Controls.Add(Me.trkAppDepSpeed)
        Me.pagAppDep.Controls.Add(Me.cmdAppDepHeading)
        Me.pagAppDep.Controls.Add(Me.txtAppDepHeading)
        Me.pagAppDep.Controls.Add(Me.lblAppDepHeading)
        Me.pagAppDep.Controls.Add(Me.trkAppDepHeading)
        Me.pagAppDep.Controls.Add(Me.lstAppDep)
        Me.pagAppDep.Location = New System.Drawing.Point(4, 22)
        Me.pagAppDep.Name = "pagAppDep"
        Me.pagAppDep.Padding = New System.Windows.Forms.Padding(3)
        Me.pagAppDep.Size = New System.Drawing.Size(864, 447)
        Me.pagAppDep.TabIndex = 2
        Me.pagAppDep.Text = "Approach/Departure"
        Me.pagAppDep.UseVisualStyleBackColor = True
        '
        'cmdAppDepEnterSTARVia
        '
        Me.cmdAppDepEnterSTARVia.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdAppDepEnterSTARVia.Location = New System.Drawing.Point(333, 140)
        Me.cmdAppDepEnterSTARVia.Name = "cmdAppDepEnterSTARVia"
        Me.cmdAppDepEnterSTARVia.Size = New System.Drawing.Size(186, 23)
        Me.cmdAppDepEnterSTARVia.TabIndex = 37
        Me.cmdAppDepEnterSTARVia.Text = "enter STAR via"
        Me.cmdAppDepEnterSTARVia.UseVisualStyleBackColor = True
        '
        'lblArrDepCurrentRunway
        '
        Me.lblArrDepCurrentRunway.Location = New System.Drawing.Point(333, 272)
        Me.lblArrDepCurrentRunway.Name = "lblArrDepCurrentRunway"
        Me.lblArrDepCurrentRunway.Size = New System.Drawing.Size(186, 27)
        Me.lblArrDepCurrentRunway.TabIndex = 36
        Me.lblArrDepCurrentRunway.Text = "expected: -"
        '
        'cmdAppDepExpectRunway
        '
        Me.cmdAppDepExpectRunway.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdAppDepExpectRunway.Location = New System.Drawing.Point(333, 246)
        Me.cmdAppDepExpectRunway.Name = "cmdAppDepExpectRunway"
        Me.cmdAppDepExpectRunway.Size = New System.Drawing.Size(182, 26)
        Me.cmdAppDepExpectRunway.TabIndex = 35
        Me.cmdAppDepExpectRunway.Text = "Expect Runway | 🔛"
        Me.cmdAppDepExpectRunway.UseVisualStyleBackColor = True
        '
        'lblArrDepCurrentNavPoint
        '
        Me.lblArrDepCurrentNavPoint.Location = New System.Drawing.Point(336, 32)
        Me.lblArrDepCurrentNavPoint.Name = "lblArrDepCurrentNavPoint"
        Me.lblArrDepCurrentNavPoint.Size = New System.Drawing.Size(183, 27)
        Me.lblArrDepCurrentNavPoint.TabIndex = 34
        Me.lblArrDepCurrentNavPoint.Text = "next NavPoint: -"
        '
        'cmdAppDepHeadTo
        '
        Me.cmdAppDepHeadTo.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdAppDepHeadTo.Location = New System.Drawing.Point(336, 6)
        Me.cmdAppDepHeadTo.Name = "cmdAppDepHeadTo"
        Me.cmdAppDepHeadTo.Size = New System.Drawing.Size(183, 23)
        Me.cmdAppDepHeadTo.TabIndex = 33
        Me.cmdAppDepHeadTo.Text = "head to NavPoint"
        Me.cmdAppDepHeadTo.UseVisualStyleBackColor = True
        '
        'cmdAppDepEnterSID
        '
        Me.cmdAppDepEnterSID.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdAppDepEnterSID.Location = New System.Drawing.Point(333, 193)
        Me.cmdAppDepEnterSID.Name = "cmdAppDepEnterSID"
        Me.cmdAppDepEnterSID.Size = New System.Drawing.Size(182, 26)
        Me.cmdAppDepEnterSID.TabIndex = 32
        Me.cmdAppDepEnterSID.Text = "enter SID"
        Me.cmdAppDepEnterSID.UseVisualStyleBackColor = True
        '
        'lblArrDepCurrentSID
        '
        Me.lblArrDepCurrentSID.Location = New System.Drawing.Point(333, 219)
        Me.lblArrDepCurrentSID.Name = "lblArrDepCurrentSID"
        Me.lblArrDepCurrentSID.Size = New System.Drawing.Size(186, 27)
        Me.lblArrDepCurrentSID.TabIndex = 31
        Me.lblArrDepCurrentSID.Text = "current SID: -"
        '
        'lblArrDepCurrentStar
        '
        Me.lblArrDepCurrentStar.Location = New System.Drawing.Point(332, 166)
        Me.lblArrDepCurrentStar.Name = "lblArrDepCurrentStar"
        Me.lblArrDepCurrentStar.Size = New System.Drawing.Size(187, 27)
        Me.lblArrDepCurrentStar.TabIndex = 31
        Me.lblArrDepCurrentStar.Text = "current STAR: -"
        '
        'cmdAppDepEnterSTAR
        '
        Me.cmdAppDepEnterSTAR.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdAppDepEnterSTAR.Location = New System.Drawing.Point(333, 112)
        Me.cmdAppDepEnterSTAR.Name = "cmdAppDepEnterSTAR"
        Me.cmdAppDepEnterSTAR.Size = New System.Drawing.Size(186, 23)
        Me.cmdAppDepEnterSTAR.TabIndex = 30
        Me.cmdAppDepEnterSTAR.Text = "enter STAR"
        Me.cmdAppDepEnterSTAR.UseVisualStyleBackColor = True
        '
        'cmdAppDepEnterFinal
        '
        Me.cmdAppDepEnterFinal.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdAppDepEnterFinal.Location = New System.Drawing.Point(332, 299)
        Me.cmdAppDepEnterFinal.Name = "cmdAppDepEnterFinal"
        Me.cmdAppDepEnterFinal.Size = New System.Drawing.Size(184, 26)
        Me.cmdAppDepEnterFinal.TabIndex = 29
        Me.cmdAppDepEnterFinal.Text = "Enter Final | 🛬"
        Me.cmdAppDepEnterFinal.UseVisualStyleBackColor = True
        '
        'nudAppDepAltitude
        '
        Me.nudAppDepAltitude.Increment = New Decimal(New Integer() {100, 0, 0, 0})
        Me.nudAppDepAltitude.Location = New System.Drawing.Point(619, 350)
        Me.nudAppDepAltitude.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudAppDepAltitude.Name = "nudAppDepAltitude"
        Me.nudAppDepAltitude.Size = New System.Drawing.Size(141, 20)
        Me.nudAppDepAltitude.TabIndex = 28
        Me.nudAppDepAltitude.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudAppDepAltitude.ThousandsSeparator = True
        '
        'cmdAppDepAltitude
        '
        Me.cmdAppDepAltitude.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdAppDepAltitude.Location = New System.Drawing.Point(619, 376)
        Me.cmdAppDepAltitude.Name = "cmdAppDepAltitude"
        Me.cmdAppDepAltitude.Size = New System.Drawing.Size(141, 23)
        Me.cmdAppDepAltitude.TabIndex = 27
        Me.cmdAppDepAltitude.Text = "Confirm Altitude | ⏫⏬"
        Me.cmdAppDepAltitude.UseVisualStyleBackColor = True
        '
        'trkAppDepAltitude
        '
        Me.trkAppDepAltitude.LargeChange = 500
        Me.trkAppDepAltitude.Location = New System.Drawing.Point(704, 192)
        Me.trkAppDepAltitude.Maximum = 10000
        Me.trkAppDepAltitude.Name = "trkAppDepAltitude"
        Me.trkAppDepAltitude.Orientation = System.Windows.Forms.Orientation.Vertical
        Me.trkAppDepAltitude.Size = New System.Drawing.Size(45, 149)
        Me.trkAppDepAltitude.SmallChange = 50
        Me.trkAppDepAltitude.TabIndex = 26
        Me.trkAppDepAltitude.TickFrequency = 100
        Me.trkAppDepAltitude.TickStyle = System.Windows.Forms.TickStyle.TopLeft
        '
        'cmdAppDepContactTower
        '
        Me.cmdAppDepContactTower.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdAppDepContactTower.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdAppDepContactTower.Location = New System.Drawing.Point(529, 418)
        Me.cmdAppDepContactTower.Name = "cmdAppDepContactTower"
        Me.cmdAppDepContactTower.Size = New System.Drawing.Size(231, 23)
        Me.cmdAppDepContactTower.TabIndex = 24
        Me.cmdAppDepContactTower.Text = "Contact Tower | 💬🗼"
        Me.cmdAppDepContactTower.UseVisualStyleBackColor = True
        '
        'lstAppDep
        '
        Me.lstAppDep.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstAppDep.BackColor = System.Drawing.SystemColors.Control
        Me.lstAppDep.Location = New System.Drawing.Point(6, 6)
        Me.lstAppDep.Name = "lstAppDep"
        Me.lstAppDep.Size = New System.Drawing.Size(321, 435)
        Me.lstAppDep.TabIndex = 25
        '
        'pagGame
        '
        Me.pagGame.Controls.Add(Me.lblMillisecondsBetweenTicks)
        Me.pagGame.Controls.Add(Me.lblGatedAtTerminal)
        Me.pagGame.Controls.Add(Me.dtpEndGateUntil)
        Me.pagGame.Controls.Add(Me.Label3)
        Me.pagGame.Controls.Add(Me.Label2)
        Me.pagGame.Controls.Add(Me.dtpSpawnUntil)
        Me.pagGame.Controls.Add(Me.lblMaxPlanes)
        Me.pagGame.Controls.Add(Me.trkMaxPlanes)
        Me.pagGame.Controls.Add(Me.lblDepartedPlanes)
        Me.pagGame.Controls.Add(Me.lblGatedPlanes)
        Me.pagGame.Controls.Add(Me.lblTakenOffPlanes)
        Me.pagGame.Controls.Add(Me.lblLandedPlanes)
        Me.pagGame.Controls.Add(Me.lblCrashedPlanes)
        Me.pagGame.Controls.Add(Me.cmdShowTRACONRadar)
        Me.pagGame.Controls.Add(Me.cmdShowAppDepRadar)
        Me.pagGame.Controls.Add(Me.cmdShowTowerRadar)
        Me.pagGame.Controls.Add(Me.cmdShowGroundRadar)
        Me.pagGame.Controls.Add(Me.cmdSpecialSpawn)
        Me.pagGame.Controls.Add(Me.cmdSpecialDespawn)
        Me.pagGame.Controls.Add(Me.lstGame)
        Me.pagGame.Location = New System.Drawing.Point(4, 22)
        Me.pagGame.Name = "pagGame"
        Me.pagGame.Padding = New System.Windows.Forms.Padding(3)
        Me.pagGame.Size = New System.Drawing.Size(864, 447)
        Me.pagGame.TabIndex = 3
        Me.pagGame.Text = "Game"
        Me.pagGame.UseVisualStyleBackColor = True
        '
        'lblMillisecondsBetweenTicks
        '
        Me.lblMillisecondsBetweenTicks.AutoSize = True
        Me.lblMillisecondsBetweenTicks.Location = New System.Drawing.Point(429, 346)
        Me.lblMillisecondsBetweenTicks.Name = "lblMillisecondsBetweenTicks"
        Me.lblMillisecondsBetweenTicks.Size = New System.Drawing.Size(29, 13)
        Me.lblMillisecondsBetweenTicks.TabIndex = 29
        Me.lblMillisecondsBetweenTicks.Text = "0 ms"
        '
        'lblGatedAtTerminal
        '
        Me.lblGatedAtTerminal.AutoSize = True
        Me.lblGatedAtTerminal.Location = New System.Drawing.Point(429, 146)
        Me.lblGatedAtTerminal.Name = "lblGatedAtTerminal"
        Me.lblGatedAtTerminal.Size = New System.Drawing.Size(164, 13)
        Me.lblGatedAtTerminal.TabIndex = 28
        Me.lblGatedAtTerminal.Text = "successful arrivals at right gate: 0"
        '
        'dtpEndGateUntil
        '
        Me.dtpEndGateUntil.Format = System.Windows.Forms.DateTimePickerFormat.Time
        Me.dtpEndGateUntil.Location = New System.Drawing.Point(586, 281)
        Me.dtpEndGateUntil.Name = "dtpEndGateUntil"
        Me.dtpEndGateUntil.ShowUpDown = True
        Me.dtpEndGateUntil.Size = New System.Drawing.Size(200, 20)
        Me.dtpEndGateUntil.TabIndex = 27
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(429, 289)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(74, 13)
        Me.Label3.TabIndex = 26
        Me.Label3.Text = "end gate until:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(429, 256)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(63, 13)
        Me.Label2.TabIndex = 26
        Me.Label2.Text = "spawn until:"
        '
        'dtpSpawnUntil
        '
        Me.dtpSpawnUntil.Format = System.Windows.Forms.DateTimePickerFormat.Time
        Me.dtpSpawnUntil.Location = New System.Drawing.Point(586, 249)
        Me.dtpSpawnUntil.Name = "dtpSpawnUntil"
        Me.dtpSpawnUntil.ShowUpDown = True
        Me.dtpSpawnUntil.Size = New System.Drawing.Size(200, 20)
        Me.dtpSpawnUntil.TabIndex = 25
        '
        'lblMaxPlanes
        '
        Me.lblMaxPlanes.AutoSize = True
        Me.lblMaxPlanes.Location = New System.Drawing.Point(429, 216)
        Me.lblMaxPlanes.Name = "lblMaxPlanes"
        Me.lblMaxPlanes.Size = New System.Drawing.Size(58, 13)
        Me.lblMaxPlanes.TabIndex = 24
        Me.lblMaxPlanes.Text = "maxPlanes"
        Me.lblMaxPlanes.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'trkMaxPlanes
        '
        Me.trkMaxPlanes.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.trkMaxPlanes.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.trkMaxPlanes.Location = New System.Drawing.Point(432, 184)
        Me.trkMaxPlanes.Maximum = 200
        Me.trkMaxPlanes.Minimum = 1
        Me.trkMaxPlanes.Name = "trkMaxPlanes"
        Me.trkMaxPlanes.Size = New System.Drawing.Size(362, 45)
        Me.trkMaxPlanes.TabIndex = 23
        Me.trkMaxPlanes.TickFrequency = 5
        Me.trkMaxPlanes.Value = 1
        '
        'lblDepartedPlanes
        '
        Me.lblDepartedPlanes.AutoSize = True
        Me.lblDepartedPlanes.Location = New System.Drawing.Point(429, 159)
        Me.lblDepartedPlanes.Name = "lblDepartedPlanes"
        Me.lblDepartedPlanes.Size = New System.Drawing.Size(122, 13)
        Me.lblDepartedPlanes.TabIndex = 22
        Me.lblDepartedPlanes.Text = "successful departures: 0"
        '
        'lblGatedPlanes
        '
        Me.lblGatedPlanes.AutoSize = True
        Me.lblGatedPlanes.Location = New System.Drawing.Point(429, 129)
        Me.lblGatedPlanes.Name = "lblGatedPlanes"
        Me.lblGatedPlanes.Size = New System.Drawing.Size(105, 13)
        Me.lblGatedPlanes.TabIndex = 21
        Me.lblGatedPlanes.Text = "successful arrivals: 0"
        '
        'lblTakenOffPlanes
        '
        Me.lblTakenOffPlanes.AutoSize = True
        Me.lblTakenOffPlanes.Location = New System.Drawing.Point(429, 116)
        Me.lblTakenOffPlanes.Name = "lblTakenOffPlanes"
        Me.lblTakenOffPlanes.Size = New System.Drawing.Size(113, 13)
        Me.lblTakenOffPlanes.TabIndex = 20
        Me.lblTakenOffPlanes.Text = "successful take-offs: 0"
        '
        'lblLandedPlanes
        '
        Me.lblLandedPlanes.AutoSize = True
        Me.lblLandedPlanes.Location = New System.Drawing.Point(429, 103)
        Me.lblLandedPlanes.Name = "lblLandedPlanes"
        Me.lblLandedPlanes.Size = New System.Drawing.Size(114, 13)
        Me.lblLandedPlanes.TabIndex = 19
        Me.lblLandedPlanes.Text = "successful landings: 0 "
        '
        'lblCrashedPlanes
        '
        Me.lblCrashedPlanes.AutoSize = True
        Me.lblCrashedPlanes.Location = New System.Drawing.Point(429, 90)
        Me.lblCrashedPlanes.Name = "lblCrashedPlanes"
        Me.lblCrashedPlanes.Size = New System.Drawing.Size(91, 13)
        Me.lblCrashedPlanes.TabIndex = 18
        Me.lblCrashedPlanes.Text = "crashed planes: 0"
        '
        'cmdShowTRACONRadar
        '
        Me.cmdShowTRACONRadar.Enabled = False
        Me.cmdShowTRACONRadar.Location = New System.Drawing.Point(526, 418)
        Me.cmdShowTRACONRadar.Name = "cmdShowTRACONRadar"
        Me.cmdShowTRACONRadar.Size = New System.Drawing.Size(187, 23)
        Me.cmdShowTRACONRadar.TabIndex = 17
        Me.cmdShowTRACONRadar.Text = "new TRACON Window"
        Me.cmdShowTRACONRadar.UseVisualStyleBackColor = True
        '
        'cmdShowAppDepRadar
        '
        Me.cmdShowAppDepRadar.Location = New System.Drawing.Point(333, 418)
        Me.cmdShowAppDepRadar.Name = "cmdShowAppDepRadar"
        Me.cmdShowAppDepRadar.Size = New System.Drawing.Size(187, 23)
        Me.cmdShowAppDepRadar.TabIndex = 16
        Me.cmdShowAppDepRadar.Text = "New Approach/Departure Window"
        Me.cmdShowAppDepRadar.UseVisualStyleBackColor = True
        '
        'cmdShowTowerRadar
        '
        Me.cmdShowTowerRadar.Location = New System.Drawing.Point(526, 389)
        Me.cmdShowTowerRadar.Name = "cmdShowTowerRadar"
        Me.cmdShowTowerRadar.Size = New System.Drawing.Size(187, 23)
        Me.cmdShowTowerRadar.TabIndex = 15
        Me.cmdShowTowerRadar.Text = "New Tower Radar Window"
        Me.cmdShowTowerRadar.UseVisualStyleBackColor = True
        '
        'cmdShowGroundRadar
        '
        Me.cmdShowGroundRadar.Location = New System.Drawing.Point(333, 389)
        Me.cmdShowGroundRadar.Name = "cmdShowGroundRadar"
        Me.cmdShowGroundRadar.Size = New System.Drawing.Size(187, 23)
        Me.cmdShowGroundRadar.TabIndex = 14
        Me.cmdShowGroundRadar.Text = "New Ground Radar Window"
        Me.cmdShowGroundRadar.UseVisualStyleBackColor = True
        '
        'lstGame
        '
        Me.lstGame.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lstGame.BackColor = System.Drawing.SystemColors.Control
        Me.lstGame.Location = New System.Drawing.Point(6, 6)
        Me.lstGame.Name = "lstGame"
        Me.lstGame.Size = New System.Drawing.Size(321, 435)
        Me.lstGame.TabIndex = 13
        '
        'tmrTick
        '
        Me.tmrTick.Enabled = True
        '
        'cmsAppDepSTAR
        '
        Me.cmsAppDepSTAR.Name = "cmsAppDepSTAR"
        Me.cmsAppDepSTAR.Size = New System.Drawing.Size(61, 4)
        '
        'cmsAppDepSID
        '
        Me.cmsAppDepSID.Name = "cmsAppDepSID"
        Me.cmsAppDepSID.Size = New System.Drawing.Size(61, 4)
        '
        'cmsAppDepNavPoints
        '
        Me.cmsAppDepNavPoints.Name = "cmsAppDepNavPoints"
        Me.cmsAppDepNavPoints.Size = New System.Drawing.Size(61, 4)
        '
        'cmsAppDepRunways
        '
        Me.cmsAppDepRunways.Name = "cmsAppDepRunways"
        Me.cmsAppDepRunways.Size = New System.Drawing.Size(61, 4)
        '
        'cmsTowerRunways
        '
        Me.cmsTowerRunways.Name = "cmsTowerRunways"
        Me.cmsTowerRunways.Size = New System.Drawing.Size(61, 4)
        '
        'cmsTowerExitVia
        '
        Me.cmsTowerExitVia.Name = "cmsTowerExitVia"
        Me.cmsTowerExitVia.Size = New System.Drawing.Size(61, 4)
        '
        'cmsGroundTaxiTo
        '
        Me.cmsGroundTaxiTo.Name = "cmsGroundTaxiTo"
        Me.cmsGroundTaxiTo.Size = New System.Drawing.Size(61, 4)
        '
        'cmsAppDepSTARvia
        '
        Me.cmsAppDepSTARvia.Name = "cmsAppDepSTARvia"
        Me.cmsAppDepSTARvia.Size = New System.Drawing.Size(61, 4)
        '
        'cmdGroundTaxiTo
        '
        Me.cmdGroundTaxiTo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.cmdGroundTaxiTo.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!)
        Me.cmdGroundTaxiTo.Image = Global.ATC.My.Resources.Resources.imgRunway
        Me.cmdGroundTaxiTo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdGroundTaxiTo.Location = New System.Drawing.Point(334, 6)
        Me.cmdGroundTaxiTo.Name = "cmdGroundTaxiTo"
        Me.cmdGroundTaxiTo.Size = New System.Drawing.Size(172, 23)
        Me.cmdGroundTaxiTo.TabIndex = 11
        Me.cmdGroundTaxiTo.Text = "taxi to | 🔀"
        Me.cmdGroundTaxiTo.UseVisualStyleBackColor = True
        '
        'cmdGroundContinueTaxi
        '
        Me.cmdGroundContinueTaxi.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdGroundContinueTaxi.Image = Global.ATC.My.Resources.Resources.imgContinueTaxi
        Me.cmdGroundContinueTaxi.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdGroundContinueTaxi.Location = New System.Drawing.Point(512, 35)
        Me.cmdGroundContinueTaxi.Name = "cmdGroundContinueTaxi"
        Me.cmdGroundContinueTaxi.Size = New System.Drawing.Size(144, 63)
        Me.cmdGroundContinueTaxi.TabIndex = 3
        Me.cmdGroundContinueTaxi.Text = "           continue taxi | ✔"
        Me.cmdGroundContinueTaxi.UseVisualStyleBackColor = True
        '
        'cmdGroundHold
        '
        Me.cmdGroundHold.Font = New System.Drawing.Font("Segoe UI Emoji", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdGroundHold.Image = Global.ATC.My.Resources.Resources.imgHoldTaxi
        Me.cmdGroundHold.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdGroundHold.Location = New System.Drawing.Point(512, 104)
        Me.cmdGroundHold.Name = "cmdGroundHold"
        Me.cmdGroundHold.Size = New System.Drawing.Size(144, 63)
        Me.cmdGroundHold.TabIndex = 4
        Me.cmdGroundHold.Text = "          hold position | ⛔"
        Me.cmdGroundHold.UseVisualStyleBackColor = True
        '
        'frmAllControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(896, 497)
        Me.Controls.Add(Me.tabControls)
        Me.Name = "frmAllControl"
        Me.Text = "Command Center"
        CType(Me.trkAppDepSpeed, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trkAppDepHeading, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabControls.ResumeLayout(False)
        Me.pagGround.ResumeLayout(False)
        Me.pagGround.PerformLayout()
        Me.pagTower.ResumeLayout(False)
        Me.pagTower.PerformLayout()
        CType(Me.nudTowerAltitude, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trkTowerAltitude, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trkTowerSpeed, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trkTowerHeading, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pagAppDep.ResumeLayout(False)
        Me.pagAppDep.PerformLayout()
        CType(Me.nudAppDepAltitude, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trkAppDepAltitude, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pagGame.ResumeLayout(False)
        Me.pagGame.PerformLayout()
        CType(Me.trkMaxPlanes, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cmdGroundPushbackApproved As Button
    Friend WithEvents cmdGroundContinueTaxi As Button
    Friend WithEvents cmdGroundHold As Button
    Friend WithEvents cmdTowerLineUpAndWait As Button
    Friend WithEvents cmdTowerTakeOff As Button
    Friend WithEvents cmdTowerLineUpandTakeOff As Button
    Friend WithEvents cmdSpecialDespawn As Button
    Friend WithEvents cmdSpecialSpawn As Button
    Friend WithEvents trkAppDepHeading As TrackBar
    Friend WithEvents lblAppDepHeading As Label
    Friend WithEvents cmdAppDepHeading As Button
    Friend WithEvents txtAppDepHeading As TextBox
    Friend WithEvents cmdAppDepAdjustSpeed As Button
    Friend WithEvents trkAppDepSpeed As TrackBar
    Friend WithEvents cmdTowerCleardToLand As Button
    Friend WithEvents cmdAppDepMakeShortApproach As Button
    Friend WithEvents cmdTowerContactGround As Button
    Friend WithEvents cmdGroundContactTower As Button
    Friend WithEvents cmdTowerContactDeparture As Button
    Friend WithEvents tabControls As TabControl
    Friend WithEvents pagGround As TabPage
    Friend WithEvents pagTower As TabPage
    Friend WithEvents pagAppDep As TabPage
    Friend WithEvents pagGame As TabPage
    Friend WithEvents cmdTowerContactApproach As Button
    Friend WithEvents cmdAppDepContactTower As Button
    Friend WithEvents cmdTowerContinueTaxi As Button
    Friend WithEvents cmdTowerHold As Button
    Friend WithEvents cmdTowerMakeShortApproach As Button
    Friend WithEvents trkTowerHeading As TrackBar
    Friend WithEvents cmdTowerHeading As Button
    Friend WithEvents txtTowerHeading As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents trkTowerSpeed As TrackBar
    Friend WithEvents cmdTowerAdjustSpeed As Button
    Friend WithEvents tmrTick As Timer
    Friend WithEvents lstGround As ctlStripeList
    Friend WithEvents lstTower As ctlStripeList
    Friend WithEvents lstAppDep As ctlStripeList
    Friend WithEvents lstGame As ctlStripeList
    Friend WithEvents cmdTowerAltitude As Button
    Friend WithEvents trkTowerAltitude As TrackBar
    Friend WithEvents trkAppDepAltitude As TrackBar
    Friend WithEvents cmdAppDepAltitude As Button
    Friend WithEvents nudTowerAltitude As NumericUpDown
    Friend WithEvents nudAppDepAltitude As NumericUpDown
    Friend WithEvents lblVia As Label
    Friend WithEvents txtVia As TextBox
    Friend WithEvents lstTowerOpenedRunwaysArrival As CheckedListBox
    Friend WithEvents lblAvailabeRunways As Label
    Friend WithEvents cmdShowGroundRadar As Button
    Friend WithEvents cmdShowTowerRadar As Button
    Friend WithEvents cmdShowTRACONRadar As Button
    Friend WithEvents cmdShowAppDepRadar As Button
    Friend WithEvents lblCrashedPlanes As Label
    Friend WithEvents lblGatedPlanes As Label
    Friend WithEvents lblTakenOffPlanes As Label
    Friend WithEvents lblLandedPlanes As Label
    Friend WithEvents lblDepartedPlanes As Label
    Friend WithEvents trkMaxPlanes As TrackBar
    Friend WithEvents lblMaxPlanes As Label
    Friend WithEvents dtpSpawnUntil As DateTimePicker
    Friend WithEvents Label2 As Label
    Friend WithEvents dtpEndGateUntil As DateTimePicker
    Friend WithEvents Label3 As Label
    Friend WithEvents lblGatedAtTerminal As Label
    Friend WithEvents lstTowerOpenedRunwaysDeparture As CheckedListBox
    Friend WithEvents Label4 As Label
    Friend WithEvents cmdTowerEnterFinal As Button
    Friend WithEvents cmdAppDepEnterFinal As Button
    Friend WithEvents cmsAppDepSTAR As ContextMenuStrip
    Friend WithEvents lblArrDepCurrentStar As Label
    Friend WithEvents cmdAppDepEnterSTAR As Button
    Friend WithEvents cmdAppDepEnterSID As Button
    Friend WithEvents lblArrDepCurrentSID As Label
    Friend WithEvents cmsAppDepSID As ContextMenuStrip
    Friend WithEvents cmdAppDepHeadTo As Button
    Friend WithEvents cmsAppDepNavPoints As ContextMenuStrip
    Friend WithEvents lblArrDepCurrentNavPoint As Label
    Friend WithEvents lblArrDepCurrentRunway As Label
    Friend WithEvents cmdAppDepExpectRunway As Button
    Friend WithEvents cmsAppDepRunways As ContextMenuStrip
    Friend WithEvents cmdTowerExpectRunway As Button
    Friend WithEvents cmsTowerRunways As ContextMenuStrip
    Friend WithEvents lblTowerCurrentRunway As Label
    Friend WithEvents lblTowerExitVia As Label
    Friend WithEvents cmdTowerExitVia As Button
    Friend WithEvents cmsTowerExitVia As ContextMenuStrip
    Friend WithEvents lblGroundTaxiTo As Label
    Friend WithEvents cmdGroundTaxiTo As Button
    Friend WithEvents cmsGroundTaxiTo As ContextMenuStrip
    Friend WithEvents cmdAppDepEnterSTARVia As Button
    Friend WithEvents cmsAppDepSTARvia As ContextMenuStrip
    Friend WithEvents cltWindRose As ctlWindRose
    Friend WithEvents cmdGroundClearVia As Button
    Friend WithEvents cmdGroundChangeTaxi As Button
    Friend WithEvents lblMillisecondsBetweenTicks As Label
End Class
