<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAppDepRadar
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
        Me.picAppDep = New System.Windows.Forms.PictureBox()
        Me.pnlAppDep = New System.Windows.Forms.Panel()
        Me.trkTimerIterval = New System.Windows.Forms.TrackBar()
        Me.chkShowFramerate = New System.Windows.Forms.CheckBox()
        Me.lblFPS = New System.Windows.Forms.Label()
        Me.lblMillisecondsBetweenFrames = New System.Windows.Forms.Label()
        Me.chkRenderBackground = New System.Windows.Forms.CheckBox()
        Me.ctlWindRose = New ATC.ctlWindRose()
        Me.tmrTick = New System.Windows.Forms.Timer(Me.components)
        CType(Me.picAppDep, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlAppDep.SuspendLayout()
        CType(Me.trkTimerIterval, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'picAppDep
        '
        Me.picAppDep.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picAppDep.BackColor = System.Drawing.Color.White
        Me.picAppDep.InitialImage = Nothing
        Me.picAppDep.Location = New System.Drawing.Point(0, 0)
        Me.picAppDep.Name = "picAppDep"
        Me.picAppDep.Size = New System.Drawing.Size(775, 425)
        Me.picAppDep.TabIndex = 0
        Me.picAppDep.TabStop = False
        '
        'pnlAppDep
        '
        Me.pnlAppDep.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlAppDep.AutoScroll = True
        Me.pnlAppDep.BackColor = System.Drawing.Color.Transparent
        Me.pnlAppDep.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlAppDep.Controls.Add(Me.trkTimerIterval)
        Me.pnlAppDep.Controls.Add(Me.chkShowFramerate)
        Me.pnlAppDep.Controls.Add(Me.lblFPS)
        Me.pnlAppDep.Controls.Add(Me.lblMillisecondsBetweenFrames)
        Me.pnlAppDep.Controls.Add(Me.chkRenderBackground)
        Me.pnlAppDep.Controls.Add(Me.ctlWindRose)
        Me.pnlAppDep.Controls.Add(Me.picAppDep)
        Me.pnlAppDep.Location = New System.Drawing.Point(12, 12)
        Me.pnlAppDep.Name = "pnlAppDep"
        Me.pnlAppDep.Size = New System.Drawing.Size(776, 426)
        Me.pnlAppDep.TabIndex = 1
        '
        'trkTimerIterval
        '
        Me.trkTimerIterval.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.trkTimerIterval.AutoSize = False
        Me.trkTimerIterval.LargeChange = 1000
        Me.trkTimerIterval.Location = New System.Drawing.Point(3, 405)
        Me.trkTimerIterval.Maximum = 5000
        Me.trkTimerIterval.Minimum = 30
        Me.trkTimerIterval.Name = "trkTimerIterval"
        Me.trkTimerIterval.Size = New System.Drawing.Size(633, 15)
        Me.trkTimerIterval.TabIndex = 11
        Me.trkTimerIterval.Value = 30
        '
        'chkShowFramerate
        '
        Me.chkShowFramerate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkShowFramerate.AutoSize = True
        Me.chkShowFramerate.Location = New System.Drawing.Point(756, 406)
        Me.chkShowFramerate.Name = "chkShowFramerate"
        Me.chkShowFramerate.Size = New System.Drawing.Size(15, 14)
        Me.chkShowFramerate.TabIndex = 8
        Me.chkShowFramerate.UseVisualStyleBackColor = True
        '
        'lblFPS
        '
        Me.lblFPS.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFPS.Location = New System.Drawing.Point(642, 407)
        Me.lblFPS.Name = "lblFPS"
        Me.lblFPS.Size = New System.Drawing.Size(55, 13)
        Me.lblFPS.TabIndex = 5
        Me.lblFPS.Text = "0FPS"
        Me.lblFPS.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblMillisecondsBetweenFrames
        '
        Me.lblMillisecondsBetweenFrames.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMillisecondsBetweenFrames.BackColor = System.Drawing.SystemColors.Control
        Me.lblMillisecondsBetweenFrames.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMillisecondsBetweenFrames.Location = New System.Drawing.Point(703, 407)
        Me.lblMillisecondsBetweenFrames.Name = "lblMillisecondsBetweenFrames"
        Me.lblMillisecondsBetweenFrames.Size = New System.Drawing.Size(47, 13)
        Me.lblMillisecondsBetweenFrames.TabIndex = 4
        Me.lblMillisecondsBetweenFrames.Text = "0 ms"
        Me.lblMillisecondsBetweenFrames.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'chkRenderBackground
        '
        Me.chkRenderBackground.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkRenderBackground.AutoSize = True
        Me.chkRenderBackground.BackColor = System.Drawing.Color.Transparent
        Me.chkRenderBackground.Checked = True
        Me.chkRenderBackground.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkRenderBackground.ForeColor = System.Drawing.Color.Transparent
        Me.chkRenderBackground.Location = New System.Drawing.Point(756, 385)
        Me.chkRenderBackground.Name = "chkRenderBackground"
        Me.chkRenderBackground.Size = New System.Drawing.Size(15, 14)
        Me.chkRenderBackground.TabIndex = 3
        Me.chkRenderBackground.UseVisualStyleBackColor = False
        '
        'ctlWindRose
        '
        Me.ctlWindRose.BackColor = System.Drawing.Color.White
        Me.ctlWindRose.Location = New System.Drawing.Point(3, 3)
        Me.ctlWindRose.Name = "ctlWindRose"
        Me.ctlWindRose.Size = New System.Drawing.Size(64, 64)
        Me.ctlWindRose.TabIndex = 1
        '
        'tmrTick
        '
        Me.tmrTick.Enabled = True
        '
        'frmAppDepRadar
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.pnlAppDep)
        Me.Name = "frmAppDepRadar"
        Me.Text = "Arrival/Departure Control Radar"
        CType(Me.picAppDep, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlAppDep.ResumeLayout(False)
        Me.pnlAppDep.PerformLayout()
        CType(Me.trkTimerIterval, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents picAppDep As PictureBox
    Friend WithEvents pnlAppDep As Panel
    Friend WithEvents tmrTick As Timer
    Friend WithEvents ctlWindRose As ctlWindRose
    Friend WithEvents chkRenderBackground As CheckBox
    Friend WithEvents lblMillisecondsBetweenFrames As Label
    Friend WithEvents lblFPS As Label
    Friend WithEvents chkShowFramerate As CheckBox
    Friend WithEvents trkTimerIterval As TrackBar
End Class
