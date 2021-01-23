Option Explicit On

Public Class ctlWindRose
    Private airport As clsAirport

    Friend Sub loadAirport(ByRef _airport As clsAirport)
        Me.airport = _airport
    End Sub

    Friend Property refreshRateInMs As Long
        Set(value As Long)
            Me.tmrRefresh.Interval = value
        End Set
        Get
            Return Me.tmrRefresh.Interval
        End Get
    End Property

    Private Sub picCompas_Paint(sender As Object, e As PaintEventArgs) Handles picCompas.Paint
        Dim pointMiddle As Point = New Point(Me.picCompas.Width \ 2, Me.picCompas.Height \ 2)

        'paint all degree
        For C1 = 1 To 360

            If C1 Mod 45 = 0 Then
                Dim beta1 As Double = C1

                Dim innerRadius As Long = Me.picCompas.Width \ 3
                Dim lineLength As Long = Me.picCompas.Width \ 6


                Dim pointToTheMiddle As Point = New Point(pointMiddle.X + (innerRadius * Math.Sin(beta1 * Math.PI / 180)), pointMiddle.Y + (innerRadius * Math.Cos(beta1 * Math.PI / 180)))
                Dim pointToTheOuter As Point = New Point(pointToTheMiddle.X + (lineLength * Math.Sin(beta1 * Math.PI / 180)), pointToTheMiddle.Y + (lineLength * Math.Cos(beta1 * Math.PI / 180)))

                e.Graphics.DrawLine(New Pen(Color.Black, 2), pointToTheMiddle, pointToTheOuter)
            End If

            'if game has winddirection, paint the winddirection
            If Not Me.airport Is Nothing Then
                'paint wind arrow
                Dim penWind As New Pen(Color.Blue, 2)

                Dim beta1 As Double = (Me.airport.windDirectionTo + (180))
                Dim beta2 As Double = (Me.airport.windDirectionTo - (90 + 45))
                Dim beta3 As Double = (Me.airport.windDirectionTo + (90 + 45))
                Dim windPoint1 As New Point(pointMiddle.X + (15 * Math.Sin(beta1 * Math.PI / 180)), (pointMiddle.Y) - 15 * Math.Cos(beta1 * Math.PI / 180))
                Dim windPoint2 As New Point(pointMiddle.X + (5 * Math.Sin(beta2 * Math.PI / 180)), (pointMiddle.Y) - 5 * Math.Cos(beta2 * Math.PI / 180))
                Dim windPoint3 As New Point(pointMiddle.X + (5 * Math.Sin(beta3 * Math.PI / 180)), (pointMiddle.Y) - 5 * Math.Cos(beta3 * Math.PI / 180))
                e.Graphics.DrawLine(penWind, pointMiddle, windPoint1)
                e.Graphics.DrawLine(penWind, pointMiddle, windPoint2)
                e.Graphics.DrawLine(penWind, pointMiddle, windPoint3)
            End If

        Next
    End Sub

    Private Sub tmrRefresh_Tick(sender As Object, e As EventArgs) Handles tmrRefresh.Tick
        Me.picCompas.Refresh()
    End Sub
End Class
