<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTowerRadar
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
        Me.picTowerRadar = New System.Windows.Forms.PictureBox()
        Me.pnlTower = New System.Windows.Forms.Panel()
        Me.chkRenderBackground = New System.Windows.Forms.CheckBox()
        Me.ctlWindRose = New ATC.ctlWindRose()
        Me.tmrTick = New System.Windows.Forms.Timer(Me.components)
        Me.lblMillisecondsBetweenFrames = New System.Windows.Forms.Label()
        Me.lblFPS = New System.Windows.Forms.Label()
        Me.chkShowFramerate = New System.Windows.Forms.CheckBox()
        CType(Me.picTowerRadar, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlTower.SuspendLayout()
        Me.SuspendLayout()
        '
        'picTowerRadar
        '
        Me.picTowerRadar.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picTowerRadar.BackColor = System.Drawing.Color.White
        Me.picTowerRadar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picTowerRadar.Location = New System.Drawing.Point(0, 0)
        Me.picTowerRadar.Name = "picTowerRadar"
        Me.picTowerRadar.Size = New System.Drawing.Size(776, 426)
        Me.picTowerRadar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picTowerRadar.TabIndex = 0
        Me.picTowerRadar.TabStop = False
        '
        'pnlTower
        '
        Me.pnlTower.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlTower.BackColor = System.Drawing.Color.Transparent
        Me.pnlTower.Controls.Add(Me.chkShowFramerate)
        Me.pnlTower.Controls.Add(Me.lblFPS)
        Me.pnlTower.Controls.Add(Me.lblMillisecondsBetweenFrames)
        Me.pnlTower.Controls.Add(Me.chkRenderBackground)
        Me.pnlTower.Controls.Add(Me.ctlWindRose)
        Me.pnlTower.Controls.Add(Me.picTowerRadar)
        Me.pnlTower.Location = New System.Drawing.Point(12, 12)
        Me.pnlTower.Name = "pnlTower"
        Me.pnlTower.Size = New System.Drawing.Size(776, 426)
        Me.pnlTower.TabIndex = 1
        '
        'chkRenderBackground
        '
        Me.chkRenderBackground.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkRenderBackground.AutoSize = True
        Me.chkRenderBackground.BackColor = System.Drawing.Color.Transparent
        Me.chkRenderBackground.Checked = True
        Me.chkRenderBackground.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkRenderBackground.ForeColor = System.Drawing.Color.Transparent
        Me.chkRenderBackground.Location = New System.Drawing.Point(758, 390)
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
        'lblMillisecondsBetweenFrames
        '
        Me.lblMillisecondsBetweenFrames.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMillisecondsBetweenFrames.BackColor = System.Drawing.SystemColors.Control
        Me.lblMillisecondsBetweenFrames.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMillisecondsBetweenFrames.Location = New System.Drawing.Point(723, 409)
        Me.lblMillisecondsBetweenFrames.Name = "lblMillisecondsBetweenFrames"
        Me.lblMillisecondsBetweenFrames.Size = New System.Drawing.Size(29, 13)
        Me.lblMillisecondsBetweenFrames.TabIndex = 5
        Me.lblMillisecondsBetweenFrames.Text = "0 ms"
        Me.lblMillisecondsBetweenFrames.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblFPS
        '
        Me.lblFPS.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFPS.Location = New System.Drawing.Point(684, 410)
        Me.lblFPS.Name = "lblFPS"
        Me.lblFPS.Size = New System.Drawing.Size(33, 13)
        Me.lblFPS.TabIndex = 6
        Me.lblFPS.Text = "0FPS"
        Me.lblFPS.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'chkShowFramerate
        '
        Me.chkShowFramerate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkShowFramerate.AutoSize = True
        Me.chkShowFramerate.Location = New System.Drawing.Point(758, 410)
        Me.chkShowFramerate.Name = "chkShowFramerate"
        Me.chkShowFramerate.Size = New System.Drawing.Size(15, 14)
        Me.chkShowFramerate.TabIndex = 7
        Me.chkShowFramerate.UseVisualStyleBackColor = True
        '
        'frmTowerRadar
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.pnlTower)
        Me.Name = "frmTowerRadar"
        Me.Text = "Tower Radar"
        CType(Me.picTowerRadar, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlTower.ResumeLayout(False)
        Me.pnlTower.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents picTowerRadar As PictureBox
    Friend WithEvents pnlTower As Panel
    Friend WithEvents tmrTick As Timer
    Friend WithEvents ctlWindRose As ctlWindRose
    Friend WithEvents chkRenderBackground As CheckBox
    Friend WithEvents lblMillisecondsBetweenFrames As Label
    Friend WithEvents lblFPS As Label
    Friend WithEvents chkShowFramerate As CheckBox
End Class
