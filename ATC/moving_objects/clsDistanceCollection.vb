Option Explicit On

<Serializable>
Public Class clsDistanceCollection
    Public Enum enumDistanceUnits
        meters
        feet
        nauticalMiles
    End Enum

    Public Sub New(ByVal distance As Double, ByVal unit As enumDistanceUnits)
        Select Case unit
            Case enumDistanceUnits.feet
                Me.feet = distance
            Case enumDistanceUnits.meters
                Me.meters = distance
            Case enumDistanceUnits.nauticalMiles
                Me.nauticalMiles = distance
            Case Else
                MsgBox("no valid distance")
        End Select

    End Sub

    Friend Property feet As Double

    Friend Property meters As Double
        Get
            Return Me.feet * 0.3048
        End Get
        Set(value As Double)
            Me.feet = value * 3.28084
        End Set
    End Property


    Friend Property nauticalMiles As Double
        Get
            Return Me.feet / 6076.12
        End Get
        Set(value As Double)
            Me.feet = value * 6076.12
        End Set
    End Property
End Class
