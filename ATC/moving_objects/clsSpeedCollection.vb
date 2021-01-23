Option Explicit On

<Serializable>
Public Class clsSpeedCollection
    Public Sub New(Optional ByVal knots As Double = 0)
        Me.knots = knots
    End Sub

    Friend Property knots As Double

    Friend Property inKmPerHour As Double
        Get
            Return Me.knots * 1.852
        End Get
        Set(value As Double)
            Me.knots = value / 1.852
        End Set
    End Property
    Friend Property inKmPerSeconds
        Get
            Return Me.inKmPerHour / 3600
        End Get
        Set(value)
            Me.inKmPerHour = value * 3600
        End Set
    End Property

    Friend Property inMetersPerHour As Double
        Get
            Return Me.knots * 1852
        End Get
        Set(value As Double)
            Me.knots = value / 1852
        End Set
    End Property

    Friend Property inMetersPerSecond As Double
        Get
            Return Me.inMetersPerHour / 3600
        End Get
        Set(value As Double)
            Me.inMetersPerHour = value * 3600
        End Set
    End Property

    Friend Property inMilesPerHour As Double
        Get
            Return knots * 1.15078
        End Get
        Set(value As Double)
            Me.knots = value / 1.15078
        End Set
    End Property
    Friend Property inMilesPerSecond As Double
        Get
            Return Me.inMilesPerHour / 3600
        End Get
        Set(value As Double)
            Me.inMilesPerHour = value * 3600
        End Set
    End Property

    Friend Property inFeetPerHour As Double
        Get
            Return Me.inFeetPerSecond / 3600
        End Get
        Set(value As Double)
            Me.inFeetPerSecond = value * 3600
        End Set
    End Property
    Friend Property inFeetPerSecond As Double
        Get
            Return Me.knots * 1.68781
        End Get
        Set(value As Double)
            Me.knots = value / 1.68781
        End Set
    End Property

End Class
