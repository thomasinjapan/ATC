<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ctlWindRose
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
        Me.components = New System.ComponentModel.Container()
        Me.picCompas = New System.Windows.Forms.PictureBox()
        Me.tmrRefresh = New System.Windows.Forms.Timer(Me.components)
        CType(Me.picCompas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'picCompas
        '
        Me.picCompas.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picCompas.BackColor = System.Drawing.Color.Transparent
        Me.picCompas.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.picCompas.Location = New System.Drawing.Point(3, 3)
        Me.picCompas.Name = "picCompas"
        Me.picCompas.Size = New System.Drawing.Size(158, 158)
        Me.picCompas.TabIndex = 0
        Me.picCompas.TabStop = False
        '
        'tmrRefresh
        '
        Me.tmrRefresh.Enabled = True
        Me.tmrRefresh.Interval = 5000
        '
        'ctlWindRose
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me.picCompas)
        Me.Name = "ctlWindRose"
        Me.Size = New System.Drawing.Size(164, 164)
        CType(Me.picCompas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents picCompas As PictureBox
    Friend WithEvents tmrRefresh As Timer
End Class
