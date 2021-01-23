<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMenu
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
        Me.cmdStartPause = New System.Windows.Forms.Button()
        Me.trkSpawnMin = New System.Windows.Forms.TrackBar()
        Me.lblSpawn = New System.Windows.Forms.Label()
        Me.trkSpawnMax = New System.Windows.Forms.TrackBar()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblSpawnMin = New System.Windows.Forms.Label()
        Me.lblSpawnMax = New System.Windows.Forms.Label()
        Me.trkEndGateMin = New System.Windows.Forms.TrackBar()
        Me.trkEndGateMax = New System.Windows.Forms.TrackBar()
        Me.lblEndGate = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblEndGateMin = New System.Windows.Forms.Label()
        Me.lblEndGateMax = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cmdWaitForPlayer = New System.Windows.Forms.Button()
        Me.trkChangeWindMinDelay = New System.Windows.Forms.TrackBar()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblWindDelayMin = New System.Windows.Forms.Label()
        Me.trkChangeWindMaxDelay = New System.Windows.Forms.TrackBar()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.lblWindDelayMax = New System.Windows.Forms.Label()
        Me.lblWindD = New System.Windows.Forms.Label()
        Me.trkChangeWindMinAngle = New System.Windows.Forms.TrackBar()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.trkChangeWindMaxAngle = New System.Windows.Forms.TrackBar()
        Me.lblWindDirectionMin = New System.Windows.Forms.Label()
        Me.lblWindDirectionMax = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtPort = New System.Windows.Forms.TextBox()
        Me.chkEnableAppDep = New System.Windows.Forms.CheckBox()
        Me.trkInitialWindDirection = New System.Windows.Forms.TrackBar()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.lblWindDirectionfrom = New System.Windows.Forms.Label()
        Me.lblWindDirectionTo = New System.Windows.Forms.Label()
        Me.grpSpawn = New System.Windows.Forms.GroupBox()
        Me.grpWind = New System.Windows.Forms.GroupBox()
        Me.lblMaxCrossWind = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.lblmaxCross = New System.Windows.Forms.Label()
        Me.trkMaxCrossWind = New System.Windows.Forms.TrackBar()
        Me.grpNetwork = New System.Windows.Forms.GroupBox()
        Me.trkClientUpdate = New System.Windows.Forms.TrackBar()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.lblClientUpdate = New System.Windows.Forms.Label()
        CType(Me.trkSpawnMin, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trkSpawnMax, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trkEndGateMin, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trkEndGateMax, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trkChangeWindMinDelay, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trkChangeWindMaxDelay, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trkChangeWindMinAngle, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trkChangeWindMaxAngle, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.trkInitialWindDirection, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpSpawn.SuspendLayout()
        Me.grpWind.SuspendLayout()
        CType(Me.trkMaxCrossWind, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpNetwork.SuspendLayout()
        CType(Me.trkClientUpdate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdStartPause
        '
        Me.cmdStartPause.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdStartPause.Font = New System.Drawing.Font("Segoe UI Emoji", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdStartPause.Location = New System.Drawing.Point(12, 12)
        Me.cmdStartPause.Name = "cmdStartPause"
        Me.cmdStartPause.Size = New System.Drawing.Size(755, 33)
        Me.cmdStartPause.TabIndex = 0
        Me.cmdStartPause.Text = "Start | ▶ "
        Me.cmdStartPause.UseVisualStyleBackColor = True
        '
        'trkSpawnMin
        '
        Me.trkSpawnMin.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.trkSpawnMin.AutoSize = False
        Me.trkSpawnMin.LargeChange = 30
        Me.trkSpawnMin.Location = New System.Drawing.Point(157, 16)
        Me.trkSpawnMin.Maximum = 300
        Me.trkSpawnMin.Minimum = 1
        Me.trkSpawnMin.Name = "trkSpawnMin"
        Me.trkSpawnMin.Size = New System.Drawing.Size(513, 28)
        Me.trkSpawnMin.SmallChange = 10
        Me.trkSpawnMin.TabIndex = 1
        Me.trkSpawnMin.TickFrequency = 10
        Me.trkSpawnMin.Value = 90
        '
        'lblSpawn
        '
        Me.lblSpawn.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSpawn.Location = New System.Drawing.Point(6, 16)
        Me.lblSpawn.Name = "lblSpawn"
        Me.lblSpawn.Size = New System.Drawing.Size(109, 48)
        Me.lblSpawn.TabIndex = 2
        Me.lblSpawn.Text = "spwan frequency"
        Me.lblSpawn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'trkSpawnMax
        '
        Me.trkSpawnMax.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.trkSpawnMax.AutoSize = False
        Me.trkSpawnMax.LargeChange = 30
        Me.trkSpawnMax.Location = New System.Drawing.Point(157, 36)
        Me.trkSpawnMax.Maximum = 300
        Me.trkSpawnMax.Minimum = 1
        Me.trkSpawnMax.Name = "trkSpawnMax"
        Me.trkSpawnMax.Size = New System.Drawing.Size(513, 28)
        Me.trkSpawnMax.SmallChange = 10
        Me.trkSpawnMax.TabIndex = 1
        Me.trkSpawnMax.TickFrequency = 10
        Me.trkSpawnMax.TickStyle = System.Windows.Forms.TickStyle.TopLeft
        Me.trkSpawnMax.Value = 150
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(121, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(30, 28)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "min"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(121, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(30, 28)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "max"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'lblSpawnMin
        '
        Me.lblSpawnMin.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSpawnMin.Location = New System.Drawing.Point(690, 16)
        Me.lblSpawnMin.Name = "lblSpawnMin"
        Me.lblSpawnMin.Size = New System.Drawing.Size(59, 28)
        Me.lblSpawnMin.TabIndex = 2
        Me.lblSpawnMin.Text = "min"
        '
        'lblSpawnMax
        '
        Me.lblSpawnMax.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSpawnMax.Location = New System.Drawing.Point(690, 36)
        Me.lblSpawnMax.Name = "lblSpawnMax"
        Me.lblSpawnMax.Size = New System.Drawing.Size(59, 28)
        Me.lblSpawnMax.TabIndex = 2
        Me.lblSpawnMax.Text = "max"
        Me.lblSpawnMax.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'trkEndGateMin
        '
        Me.trkEndGateMin.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.trkEndGateMin.AutoSize = False
        Me.trkEndGateMin.LargeChange = 30
        Me.trkEndGateMin.Location = New System.Drawing.Point(157, 81)
        Me.trkEndGateMin.Maximum = 300
        Me.trkEndGateMin.Minimum = 1
        Me.trkEndGateMin.Name = "trkEndGateMin"
        Me.trkEndGateMin.Size = New System.Drawing.Size(513, 28)
        Me.trkEndGateMin.SmallChange = 10
        Me.trkEndGateMin.TabIndex = 1
        Me.trkEndGateMin.TickFrequency = 10
        Me.trkEndGateMin.Value = 120
        '
        'trkEndGateMax
        '
        Me.trkEndGateMax.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.trkEndGateMax.AutoSize = False
        Me.trkEndGateMax.LargeChange = 30
        Me.trkEndGateMax.Location = New System.Drawing.Point(157, 101)
        Me.trkEndGateMax.Maximum = 300
        Me.trkEndGateMax.Minimum = 1
        Me.trkEndGateMax.Name = "trkEndGateMax"
        Me.trkEndGateMax.Size = New System.Drawing.Size(513, 28)
        Me.trkEndGateMax.SmallChange = 10
        Me.trkEndGateMax.TabIndex = 1
        Me.trkEndGateMax.TickFrequency = 10
        Me.trkEndGateMax.TickStyle = System.Windows.Forms.TickStyle.TopLeft
        Me.trkEndGateMax.Value = 180
        '
        'lblEndGate
        '
        Me.lblEndGate.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblEndGate.Location = New System.Drawing.Point(6, 81)
        Me.lblEndGate.Name = "lblEndGate"
        Me.lblEndGate.Size = New System.Drawing.Size(109, 48)
        Me.lblEndGate.TabIndex = 2
        Me.lblEndGate.Text = "end gate frequency"
        Me.lblEndGate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(121, 81)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(30, 28)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "min"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblEndGateMin
        '
        Me.lblEndGateMin.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblEndGateMin.Location = New System.Drawing.Point(690, 81)
        Me.lblEndGateMin.Name = "lblEndGateMin"
        Me.lblEndGateMin.Size = New System.Drawing.Size(59, 28)
        Me.lblEndGateMin.TabIndex = 2
        Me.lblEndGateMin.Text = "min"
        '
        'lblEndGateMax
        '
        Me.lblEndGateMax.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblEndGateMax.Location = New System.Drawing.Point(690, 101)
        Me.lblEndGateMax.Name = "lblEndGateMax"
        Me.lblEndGateMax.Size = New System.Drawing.Size(59, 28)
        Me.lblEndGateMax.TabIndex = 2
        Me.lblEndGateMax.Text = "max"
        Me.lblEndGateMax.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(121, 101)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(30, 28)
        Me.Label7.TabIndex = 2
        Me.Label7.Text = "max"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'cmdWaitForPlayer
        '
        Me.cmdWaitForPlayer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdWaitForPlayer.Location = New System.Drawing.Point(10, 64)
        Me.cmdWaitForPlayer.Name = "cmdWaitForPlayer"
        Me.cmdWaitForPlayer.Size = New System.Drawing.Size(649, 29)
        Me.cmdWaitForPlayer.TabIndex = 3
        Me.cmdWaitForPlayer.Text = "Let players join"
        Me.cmdWaitForPlayer.UseVisualStyleBackColor = True
        '
        'trkChangeWindMinDelay
        '
        Me.trkChangeWindMinDelay.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.trkChangeWindMinDelay.LargeChange = 30
        Me.trkChangeWindMinDelay.Location = New System.Drawing.Point(162, 77)
        Me.trkChangeWindMinDelay.Maximum = 300
        Me.trkChangeWindMinDelay.Minimum = 1
        Me.trkChangeWindMinDelay.Name = "trkChangeWindMinDelay"
        Me.trkChangeWindMinDelay.Size = New System.Drawing.Size(502, 45)
        Me.trkChangeWindMinDelay.SmallChange = 10
        Me.trkChangeWindMinDelay.TabIndex = 4
        Me.trkChangeWindMinDelay.TickFrequency = 10
        Me.trkChangeWindMinDelay.Value = 30
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(126, 77)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(30, 28)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "min"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblWindDelayMin
        '
        Me.lblWindDelayMin.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblWindDelayMin.Location = New System.Drawing.Point(687, 77)
        Me.lblWindDelayMin.Name = "lblWindDelayMin"
        Me.lblWindDelayMin.Size = New System.Drawing.Size(56, 28)
        Me.lblWindDelayMin.TabIndex = 2
        Me.lblWindDelayMin.Text = "min"
        '
        'trkChangeWindMaxDelay
        '
        Me.trkChangeWindMaxDelay.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.trkChangeWindMaxDelay.LargeChange = 30
        Me.trkChangeWindMaxDelay.Location = New System.Drawing.Point(162, 96)
        Me.trkChangeWindMaxDelay.Maximum = 300
        Me.trkChangeWindMaxDelay.Minimum = 1
        Me.trkChangeWindMaxDelay.Name = "trkChangeWindMaxDelay"
        Me.trkChangeWindMaxDelay.Size = New System.Drawing.Size(502, 45)
        Me.trkChangeWindMaxDelay.SmallChange = 10
        Me.trkChangeWindMaxDelay.TabIndex = 5
        Me.trkChangeWindMaxDelay.TickFrequency = 10
        Me.trkChangeWindMaxDelay.TickStyle = System.Windows.Forms.TickStyle.TopLeft
        Me.trkChangeWindMaxDelay.Value = 45
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(126, 94)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(30, 28)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "max"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'lblWindDelayMax
        '
        Me.lblWindDelayMax.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblWindDelayMax.Location = New System.Drawing.Point(687, 94)
        Me.lblWindDelayMax.Name = "lblWindDelayMax"
        Me.lblWindDelayMax.Size = New System.Drawing.Size(56, 28)
        Me.lblWindDelayMax.TabIndex = 2
        Me.lblWindDelayMax.Text = "max"
        Me.lblWindDelayMax.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'lblWindD
        '
        Me.lblWindD.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.lblWindD.Location = New System.Drawing.Point(6, 77)
        Me.lblWindD.Name = "lblWindD"
        Me.lblWindD.Size = New System.Drawing.Size(116, 48)
        Me.lblWindD.TabIndex = 2
        Me.lblWindD.Text = "change frequency"
        Me.lblWindD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'trkChangeWindMinAngle
        '
        Me.trkChangeWindMinAngle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.trkChangeWindMinAngle.LargeChange = 45
        Me.trkChangeWindMinAngle.Location = New System.Drawing.Point(162, 147)
        Me.trkChangeWindMinAngle.Maximum = 179
        Me.trkChangeWindMinAngle.Name = "trkChangeWindMinAngle"
        Me.trkChangeWindMinAngle.Size = New System.Drawing.Size(502, 45)
        Me.trkChangeWindMinAngle.SmallChange = 5
        Me.trkChangeWindMinAngle.TabIndex = 6
        Me.trkChangeWindMinAngle.TickFrequency = 15
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.Label5.Location = New System.Drawing.Point(6, 144)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(110, 48)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "Angle change"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'trkChangeWindMaxAngle
        '
        Me.trkChangeWindMaxAngle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.trkChangeWindMaxAngle.LargeChange = 45
        Me.trkChangeWindMaxAngle.Location = New System.Drawing.Point(162, 167)
        Me.trkChangeWindMaxAngle.Maximum = 179
        Me.trkChangeWindMaxAngle.Name = "trkChangeWindMaxAngle"
        Me.trkChangeWindMaxAngle.Size = New System.Drawing.Size(502, 45)
        Me.trkChangeWindMaxAngle.SmallChange = 5
        Me.trkChangeWindMaxAngle.TabIndex = 8
        Me.trkChangeWindMaxAngle.TickFrequency = 5
        Me.trkChangeWindMaxAngle.TickStyle = System.Windows.Forms.TickStyle.TopLeft
        Me.trkChangeWindMaxAngle.Value = 10
        '
        'lblWindDirectionMin
        '
        Me.lblWindDirectionMin.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblWindDirectionMin.Location = New System.Drawing.Point(687, 147)
        Me.lblWindDirectionMin.Name = "lblWindDirectionMin"
        Me.lblWindDirectionMin.Size = New System.Drawing.Size(56, 28)
        Me.lblWindDirectionMin.TabIndex = 2
        Me.lblWindDirectionMin.Text = "min"
        '
        'lblWindDirectionMax
        '
        Me.lblWindDirectionMax.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblWindDirectionMax.Location = New System.Drawing.Point(687, 164)
        Me.lblWindDirectionMax.Name = "lblWindDirectionMax"
        Me.lblWindDirectionMax.Size = New System.Drawing.Size(56, 28)
        Me.lblWindDirectionMax.TabIndex = 2
        Me.lblWindDirectionMax.Text = "max"
        Me.lblWindDirectionMax.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(126, 147)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(30, 28)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "min"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(126, 164)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(30, 28)
        Me.Label9.TabIndex = 2
        Me.Label9.Text = "max"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.BottomRight
        '
        'txtPort
        '
        Me.txtPort.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPort.Location = New System.Drawing.Point(665, 67)
        Me.txtPort.Name = "txtPort"
        Me.txtPort.Size = New System.Drawing.Size(84, 20)
        Me.txtPort.TabIndex = 9
        Me.txtPort.Text = "4616"
        '
        'chkEnableAppDep
        '
        Me.chkEnableAppDep.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkEnableAppDep.AutoSize = True
        Me.chkEnableAppDep.Checked = True
        Me.chkEnableAppDep.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkEnableAppDep.Location = New System.Drawing.Point(12, 603)
        Me.chkEnableAppDep.Name = "chkEnableAppDep"
        Me.chkEnableAppDep.Size = New System.Drawing.Size(158, 17)
        Me.chkEnableAppDep.TabIndex = 10
        Me.chkEnableAppDep.Text = "Enable Approach Departure"
        Me.chkEnableAppDep.UseVisualStyleBackColor = True
        '
        'trkInitialWindDirection
        '
        Me.trkInitialWindDirection.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.trkInitialWindDirection.LargeChange = 45
        Me.trkInitialWindDirection.Location = New System.Drawing.Point(162, 19)
        Me.trkInitialWindDirection.Maximum = 360
        Me.trkInitialWindDirection.Minimum = 1
        Me.trkInitialWindDirection.Name = "trkInitialWindDirection"
        Me.trkInitialWindDirection.Size = New System.Drawing.Size(502, 45)
        Me.trkInitialWindDirection.SmallChange = 45
        Me.trkInitialWindDirection.TabIndex = 11
        Me.trkInitialWindDirection.TickFrequency = 5
        Me.trkInitialWindDirection.TickStyle = System.Windows.Forms.TickStyle.Both
        Me.trkInitialWindDirection.Value = 1
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.Label10.Location = New System.Drawing.Point(6, 19)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(116, 48)
        Me.Label10.TabIndex = 12
        Me.Label10.Text = "initial direction"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblWindDirectionfrom
        '
        Me.lblWindDirectionfrom.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblWindDirectionfrom.Location = New System.Drawing.Point(687, 19)
        Me.lblWindDirectionfrom.Name = "lblWindDirectionfrom"
        Me.lblWindDirectionfrom.Size = New System.Drawing.Size(56, 19)
        Me.lblWindDirectionfrom.TabIndex = 13
        Me.lblWindDirectionfrom.Text = "from xxx deg"
        Me.lblWindDirectionfrom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblWindDirectionTo
        '
        Me.lblWindDirectionTo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblWindDirectionTo.Location = New System.Drawing.Point(687, 45)
        Me.lblWindDirectionTo.Name = "lblWindDirectionTo"
        Me.lblWindDirectionTo.Size = New System.Drawing.Size(56, 19)
        Me.lblWindDirectionTo.TabIndex = 13
        Me.lblWindDirectionTo.Text = "to xxx deg"
        Me.lblWindDirectionTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'grpSpawn
        '
        Me.grpSpawn.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpSpawn.Controls.Add(Me.trkEndGateMax)
        Me.grpSpawn.Controls.Add(Me.trkSpawnMin)
        Me.grpSpawn.Controls.Add(Me.trkSpawnMax)
        Me.grpSpawn.Controls.Add(Me.trkEndGateMin)
        Me.grpSpawn.Controls.Add(Me.lblSpawn)
        Me.grpSpawn.Controls.Add(Me.lblEndGate)
        Me.grpSpawn.Controls.Add(Me.Label1)
        Me.grpSpawn.Controls.Add(Me.Label4)
        Me.grpSpawn.Controls.Add(Me.lblSpawnMin)
        Me.grpSpawn.Controls.Add(Me.lblEndGateMin)
        Me.grpSpawn.Controls.Add(Me.lblSpawnMax)
        Me.grpSpawn.Controls.Add(Me.lblEndGateMax)
        Me.grpSpawn.Controls.Add(Me.Label2)
        Me.grpSpawn.Controls.Add(Me.Label7)
        Me.grpSpawn.Location = New System.Drawing.Point(12, 51)
        Me.grpSpawn.Name = "grpSpawn"
        Me.grpSpawn.Size = New System.Drawing.Size(755, 297)
        Me.grpSpawn.TabIndex = 14
        Me.grpSpawn.TabStop = False
        Me.grpSpawn.Text = "plane behavior"
        '
        'grpWind
        '
        Me.grpWind.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpWind.Controls.Add(Me.lblMaxCrossWind)
        Me.grpWind.Controls.Add(Me.Label11)
        Me.grpWind.Controls.Add(Me.lblmaxCross)
        Me.grpWind.Controls.Add(Me.trkMaxCrossWind)
        Me.grpWind.Controls.Add(Me.trkChangeWindMaxDelay)
        Me.grpWind.Controls.Add(Me.trkInitialWindDirection)
        Me.grpWind.Controls.Add(Me.lblWindD)
        Me.grpWind.Controls.Add(Me.lblWindDirectionTo)
        Me.grpWind.Controls.Add(Me.Label3)
        Me.grpWind.Controls.Add(Me.lblWindDirectionfrom)
        Me.grpWind.Controls.Add(Me.Label8)
        Me.grpWind.Controls.Add(Me.Label10)
        Me.grpWind.Controls.Add(Me.lblWindDelayMin)
        Me.grpWind.Controls.Add(Me.lblWindDirectionMin)
        Me.grpWind.Controls.Add(Me.lblWindDelayMax)
        Me.grpWind.Controls.Add(Me.lblWindDirectionMax)
        Me.grpWind.Controls.Add(Me.trkChangeWindMaxAngle)
        Me.grpWind.Controls.Add(Me.Label6)
        Me.grpWind.Controls.Add(Me.Label5)
        Me.grpWind.Controls.Add(Me.Label9)
        Me.grpWind.Controls.Add(Me.trkChangeWindMinAngle)
        Me.grpWind.Controls.Add(Me.trkChangeWindMinDelay)
        Me.grpWind.Location = New System.Drawing.Point(12, 211)
        Me.grpWind.Name = "grpWind"
        Me.grpWind.Size = New System.Drawing.Size(755, 271)
        Me.grpWind.TabIndex = 15
        Me.grpWind.TabStop = False
        Me.grpWind.Text = "Wind"
        '
        'lblMaxCrossWind
        '
        Me.lblMaxCrossWind.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMaxCrossWind.Location = New System.Drawing.Point(687, 210)
        Me.lblMaxCrossWind.Name = "lblMaxCrossWind"
        Me.lblMaxCrossWind.Size = New System.Drawing.Size(56, 28)
        Me.lblMaxCrossWind.TabIndex = 17
        Me.lblMaxCrossWind.Text = "max"
        Me.lblMaxCrossWind.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(128, 218)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(30, 28)
        Me.Label11.TabIndex = 16
        Me.Label11.Text = "min"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'lblmaxCross
        '
        Me.lblmaxCross.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.lblmaxCross.Location = New System.Drawing.Point(6, 205)
        Me.lblmaxCross.Name = "lblmaxCross"
        Me.lblmaxCross.Size = New System.Drawing.Size(116, 48)
        Me.lblmaxCross.TabIndex = 15
        Me.lblmaxCross.Text = "max crosswind"
        Me.lblmaxCross.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'trkMaxCrossWind
        '
        Me.trkMaxCrossWind.LargeChange = 45
        Me.trkMaxCrossWind.Location = New System.Drawing.Point(162, 218)
        Me.trkMaxCrossWind.Maximum = 180
        Me.trkMaxCrossWind.Name = "trkMaxCrossWind"
        Me.trkMaxCrossWind.Size = New System.Drawing.Size(502, 45)
        Me.trkMaxCrossWind.SmallChange = 5
        Me.trkMaxCrossWind.TabIndex = 14
        Me.trkMaxCrossWind.TickFrequency = 5
        Me.trkMaxCrossWind.Value = 90
        '
        'grpNetwork
        '
        Me.grpNetwork.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.grpNetwork.Controls.Add(Me.lblClientUpdate)
        Me.grpNetwork.Controls.Add(Me.Label12)
        Me.grpNetwork.Controls.Add(Me.trkClientUpdate)
        Me.grpNetwork.Controls.Add(Me.cmdWaitForPlayer)
        Me.grpNetwork.Controls.Add(Me.txtPort)
        Me.grpNetwork.Location = New System.Drawing.Point(12, 488)
        Me.grpNetwork.Name = "grpNetwork"
        Me.grpNetwork.Size = New System.Drawing.Size(755, 109)
        Me.grpNetwork.TabIndex = 16
        Me.grpNetwork.TabStop = False
        Me.grpNetwork.Text = "Network"
        '
        'trkClientUpdate
        '
        Me.trkClientUpdate.Enabled = False
        Me.trkClientUpdate.LargeChange = 500
        Me.trkClientUpdate.Location = New System.Drawing.Point(162, 19)
        Me.trkClientUpdate.Maximum = 10000
        Me.trkClientUpdate.Minimum = 100
        Me.trkClientUpdate.Name = "trkClientUpdate"
        Me.trkClientUpdate.Size = New System.Drawing.Size(502, 45)
        Me.trkClientUpdate.SmallChange = 100
        Me.trkClientUpdate.TabIndex = 0
        Me.trkClientUpdate.TickFrequency = 100
        Me.trkClientUpdate.Value = 200
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!)
        Me.Label12.Location = New System.Drawing.Point(6, 16)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(116, 48)
        Me.Label12.TabIndex = 16
        Me.Label12.Text = "update frequency"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lblClientUpdate
        '
        Me.lblClientUpdate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblClientUpdate.Location = New System.Drawing.Point(670, 16)
        Me.lblClientUpdate.Name = "lblClientUpdate"
        Me.lblClientUpdate.Size = New System.Drawing.Size(56, 28)
        Me.lblClientUpdate.TabIndex = 18
        Me.lblClientUpdate.Text = "ms"
        Me.lblClientUpdate.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'frmMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(779, 632)
        Me.Controls.Add(Me.grpNetwork)
        Me.Controls.Add(Me.grpWind)
        Me.Controls.Add(Me.grpSpawn)
        Me.Controls.Add(Me.chkEnableAppDep)
        Me.Controls.Add(Me.cmdStartPause)
        Me.Name = "frmMenu"
        Me.Text = "ATC - Menu"
        CType(Me.trkSpawnMin, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trkSpawnMax, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trkEndGateMin, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trkEndGateMax, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trkChangeWindMinDelay, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trkChangeWindMaxDelay, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trkChangeWindMinAngle, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trkChangeWindMaxAngle, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.trkInitialWindDirection, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpSpawn.ResumeLayout(False)
        Me.grpWind.ResumeLayout(False)
        Me.grpWind.PerformLayout()
        CType(Me.trkMaxCrossWind, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpNetwork.ResumeLayout(False)
        Me.grpNetwork.PerformLayout()
        CType(Me.trkClientUpdate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents cmdStartPause As Button
    Friend WithEvents trkSpawnMin As TrackBar
    Friend WithEvents lblSpawn As Label
    Friend WithEvents trkSpawnMax As TrackBar
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents lblSpawnMin As Label
    Friend WithEvents lblSpawnMax As Label
    Friend WithEvents trkEndGateMin As TrackBar
    Friend WithEvents trkEndGateMax As TrackBar
    Friend WithEvents lblEndGate As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents lblEndGateMin As Label
    Friend WithEvents lblEndGateMax As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents cmdWaitForPlayer As Button
    Friend WithEvents trkChangeWindMinDelay As TrackBar
    Friend WithEvents Label3 As Label
    Friend WithEvents lblWindDelayMin As Label
    Friend WithEvents trkChangeWindMaxDelay As TrackBar
    Friend WithEvents Label6 As Label
    Friend WithEvents lblWindDelayMax As Label
    Friend WithEvents lblWindD As Label
    Friend WithEvents trkChangeWindMinAngle As TrackBar
    Friend WithEvents Label5 As Label
    Friend WithEvents trkChangeWindMaxAngle As TrackBar
    Friend WithEvents lblWindDirectionMin As Label
    Friend WithEvents lblWindDirectionMax As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents txtPort As TextBox
    Friend WithEvents chkEnableAppDep As CheckBox
    Friend WithEvents trkInitialWindDirection As TrackBar
    Friend WithEvents Label10 As Label
    Friend WithEvents lblWindDirectionfrom As Label
    Friend WithEvents lblWindDirectionTo As Label
    Friend WithEvents grpSpawn As GroupBox
    Friend WithEvents grpWind As GroupBox
    Friend WithEvents lblMaxCrossWind As Label
    Friend WithEvents Label11 As Label
    Friend WithEvents lblmaxCross As Label
    Friend WithEvents trkMaxCrossWind As TrackBar
    Friend WithEvents grpNetwork As GroupBox
    Friend WithEvents trkClientUpdate As TrackBar
    Friend WithEvents lblClientUpdate As Label
    Friend WithEvents Label12 As Label
End Class
