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
            Return Me.knots * 0.000514444
        End Get
        Set(value)
            Me.knots = value / 0.000514444
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
            Return Me.knots * 0.514444
        End Get
        Set(value As Double)
            Me.knots = value / 0.514444
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
            Return Me.knots * 0.000319661
        End Get
        Set(value As Double)
            Me.knots = value / 0.000319661
        End Set
    End Property

    Friend Property inFeetPerHour As Double
        Get
            Return Me.knots * 0.000468836
        End Get
        Set(value As Double)
            Me.knots = value / 0.000468836
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
