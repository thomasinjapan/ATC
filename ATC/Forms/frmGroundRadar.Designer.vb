<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmGroundRadar
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
        Me.picGroundRadar = New System.Windows.Forms.PictureBox()
        Me.pnlGround = New System.Windows.Forms.Panel()
        Me.chkShowLabels = New System.Windows.Forms.CheckBox()
        Me.chkShowFramerate = New System.Windows.Forms.CheckBox()
        Me.lblFPS = New System.Windows.Forms.Label()
        Me.lblMillisecondsBetweenFrames = New System.Windows.Forms.Label()
        Me.chkRenderBackground = New System.Windows.Forms.CheckBox()
        Me.ctlWindRose = New ATC.ctlWindRose()
        CType(Me.picGroundRadar, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlGround.SuspendLayout()
        Me.SuspendLayout()
        '
        'picGroundRadar
        '
        Me.picGroundRadar.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picGroundRadar.BackColor = System.Drawing.Color.White
        Me.picGroundRadar.Location = New System.Drawing.Point(-1, -1)
        Me.picGroundRadar.Name = "picGroundRadar"
        Me.picGroundRadar.Size = New System.Drawing.Size(776, 426)
        Me.picGroundRadar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picGroundRadar.TabIndex = 0
        Me.picGroundRadar.TabStop = False
        '
        'pnlGround
        '
        Me.pnlGround.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlGround.BackColor = System.Drawing.Color.Transparent
        Me.pnlGround.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlGround.Controls.Add(Me.chkShowLabels)
        Me.pnlGround.Controls.Add(Me.chkShowFramerate)
        Me.pnlGround.Controls.Add(Me.lblFPS)
        Me.pnlGround.Controls.Add(Me.lblMillisecondsBetweenFrames)
        Me.pnlGround.Controls.Add(Me.chkRenderBackground)
        Me.pnlGround.Controls.Add(Me.ctlWindRose)
        Me.pnlGround.Controls.Add(Me.picGroundRadar)
        Me.pnlGround.Location = New System.Drawing.Point(12, 12)
        Me.pnlGround.Name = "pnlGround"
        Me.pnlGround.Size = New System.Drawing.Size(776, 426)
        Me.pnlGround.TabIndex = 9
        '
        'chkShowLabels
        '
        Me.chkShowLabels.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkShowLabels.AutoSize = True
        Me.chkShowLabels.Checked = True
        Me.chkShowLabels.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowLabels.Location = New System.Drawing.Point(735, 388)
        Me.chkShowLabels.Name = "chkShowLabels"
        Me.chkShowLabels.Size = New System.Drawing.Size(15, 14)
        Me.chkShowLabels.TabIndex = 9
        Me.chkShowLabels.UseVisualStyleBackColor = True
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
        Me.lblFPS.Size = New System.Drawing.Size(54, 13)
        Me.lblFPS.TabIndex = 4
        Me.lblFPS.Text = "0FPS"
        Me.lblFPS.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblMillisecondsBetweenFrames
        '
        Me.lblMillisecondsBetweenFrames.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblMillisecondsBetweenFrames.BackColor = System.Drawing.SystemColors.Control
        Me.lblMillisecondsBetweenFrames.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMillisecondsBetweenFrames.Location = New System.Drawing.Point(702, 407)
        Me.lblMillisecondsBetweenFrames.Name = "lblMillisecondsBetweenFrames"
        Me.lblMillisecondsBetweenFrames.Size = New System.Drawing.Size(48, 13)
        Me.lblMillisecondsBetweenFrames.TabIndex = 3
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
        Me.chkRenderBackground.Location = New System.Drawing.Point(756, 388)
        Me.chkRenderBackground.Name = "chkRenderBackground"
        Me.chkRenderBackground.Size = New System.Drawing.Size(15, 14)
        Me.chkRenderBackground.TabIndex = 2
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
        'frmGroundRadar
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.pnlGround)
        Me.KeyPreview = True
        Me.Name = "frmGroundRadar"
        Me.Text = "Ground Radar"
        CType(Me.picGroundRadar, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlGround.ResumeLayout(False)
        Me.pnlGround.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents picGroundRadar As PictureBox
    Friend WithEvents pnlGround As Panel
    Friend WithEvents ctlWindRose As ctlWindRose
    Friend WithEvents chkRenderBackground As CheckBox
    Friend WithEvents lblMillisecondsBetweenFrames As Label
    Friend WithEvents lblFPS As Label
    Friend WithEvents chkShowFramerate As CheckBox
    Friend WithEvents chkShowLabels As CheckBox
End Class
