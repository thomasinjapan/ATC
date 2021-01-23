<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ctlStripe
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.picCallsign = New System.Windows.Forms.PictureBox()
        Me.picType = New System.Windows.Forms.PictureBox()
        Me.picPathInfo = New System.Windows.Forms.PictureBox()
        Me.picTargetHeight = New System.Windows.Forms.PictureBox()
        Me.picTargetSpeed = New System.Windows.Forms.PictureBox()
        Me.picRwy = New System.Windows.Forms.PictureBox()
        CType(Me.picCallsign, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picType, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picPathInfo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picTargetHeight, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picTargetSpeed, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.picRwy, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'picCallsign
        '
        Me.picCallsign.BackColor = System.Drawing.Color.White
        Me.picCallsign.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picCallsign.Location = New System.Drawing.Point(0, 0)
        Me.picCallsign.Margin = New System.Windows.Forms.Padding(0)
        Me.picCallsign.Name = "picCallsign"
        Me.picCallsign.Size = New System.Drawing.Size(80, 33)
        Me.picCallsign.TabIndex = 0
        Me.picCallsign.TabStop = False
        '
        'picType
        '
        Me.picType.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.picType.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.picType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picType.Location = New System.Drawing.Point(0, 32)
        Me.picType.Margin = New System.Windows.Forms.Padding(0)
        Me.picType.Name = "picType"
        Me.picType.Size = New System.Drawing.Size(80, 32)
        Me.picType.TabIndex = 1
        Me.picType.TabStop = False
        '
        'picPathInfo
        '
        Me.picPathInfo.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picPathInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.picPathInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picPathInfo.Location = New System.Drawing.Point(79, 0)
        Me.picPathInfo.Margin = New System.Windows.Forms.Padding(0)
        Me.picPathInfo.Name = "picPathInfo"
        Me.picPathInfo.Size = New System.Drawing.Size(142, 64)
        Me.picPathInfo.TabIndex = 2
        Me.picPathInfo.TabStop = False
        '
        'picTargetHeight
        '
        Me.picTargetHeight.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picTargetHeight.BackColor = System.Drawing.Color.White
        Me.picTargetHeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picTargetHeight.Location = New System.Drawing.Point(220, 0)
        Me.picTargetHeight.Margin = New System.Windows.Forms.Padding(0)
        Me.picTargetHeight.Name = "picTargetHeight"
        Me.picTargetHeight.Size = New System.Drawing.Size(80, 22)
        Me.picTargetHeight.TabIndex = 3
        Me.picTargetHeight.TabStop = False
        '
        'picTargetSpeed
        '
        Me.picTargetSpeed.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picTargetSpeed.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.picTargetSpeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picTargetSpeed.Location = New System.Drawing.Point(220, 21)
        Me.picTargetSpeed.Name = "picTargetSpeed"
        Me.picTargetSpeed.Size = New System.Drawing.Size(80, 22)
        Me.picTargetSpeed.TabIndex = 4
        Me.picTargetSpeed.TabStop = False
        '
        'picRwy
        '
        Me.picRwy.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picRwy.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.picRwy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picRwy.Location = New System.Drawing.Point(220, 42)
        Me.picRwy.Margin = New System.Windows.Forms.Padding(0)
        Me.picRwy.Name = "picRwy"
        Me.picRwy.Size = New System.Drawing.Size(80, 22)
        Me.picRwy.TabIndex = 5
        Me.picRwy.TabStop = False
        '
        'ctlStripe
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.picRwy)
        Me.Controls.Add(Me.picTargetSpeed)
        Me.Controls.Add(Me.picTargetHeight)
        Me.Controls.Add(Me.picPathInfo)
        Me.Controls.Add(Me.picType)
        Me.Controls.Add(Me.picCallsign)
        Me.Margin = New System.Windows.Forms.Padding(0)
        Me.Name = "ctlStripe"
        Me.Size = New System.Drawing.Size(300, 64)
        CType(Me.picCallsign, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picType, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picPathInfo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picTargetHeight, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picTargetSpeed, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.picRwy, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents picCallsign As PictureBox
    Friend WithEvents picType As PictureBox
    Friend WithEvents picPathInfo As PictureBox
    Friend WithEvents picTargetHeight As PictureBox
    Friend WithEvents picTargetSpeed As PictureBox
    Friend WithEvents picRwy As PictureBox
End Class
