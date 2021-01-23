<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ctlStripeList
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.pnlStripes = New System.Windows.Forms.Panel()
        Me.SuspendLayout()
        '
        'pnlStripes
        '
        Me.pnlStripes.AutoScroll = True
        Me.pnlStripes.BackColor = System.Drawing.SystemColors.Control
        Me.pnlStripes.Dock = System.Windows.Forms.DockStyle.Left
        Me.pnlStripes.Location = New System.Drawing.Point(0, 0)
        Me.pnlStripes.Name = "pnlStripes"
        Me.pnlStripes.Size = New System.Drawing.Size(300, 264)
        Me.pnlStripes.TabIndex = 0
        '
        'ctlStripeList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Controls.Add(Me.pnlStripes)
        Me.Name = "ctlStripeList"
        Me.Size = New System.Drawing.Size(321, 264)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlStripes As Panel
End Class
