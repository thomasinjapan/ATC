Option Explicit On
Imports System.Device.Location

<Serializable>
Public Class clsEarth
    'values and formula taken from https://en.wikipedia.org/wiki/Geographic_coordinate_system#Length_of_a_degree
    Const lng_a As Double = 111418.85
    Const lng_b As Double = 93.5
    Const lng_c As Double = 0.118

    Const lat_a As Double = 111132.954
    Const lat_b As Double = 559.822
    Const lat_c As Double = 1.175

    Private reference As GeoCoordinate

    Public Sub New(ByVal reference As GeoCoordinate)
        Me.reference = reference
    End Sub

    ''' <summary>
    ''' returns the amount of meters for given longitude degrees ("x-axis") at a certain latitude
    ''' </summary>
    ''' <param name="lat">latitude in degree ("y-axis")</param>
    ''' <param name="lng">longitude in degree ("x-axis") </param>
    ''' <returns></returns>
    Function lng2m(ByVal lat As Double, lng As Double) As Double
        'Dim result As Double = Me.mPerLngDegreeAtLat(lat) * lng
        Dim sCoordinate As New GeoCoordinate(lat, Me.reference.Longitude)
        Dim eCoordinate As New GeoCoordinate(lat, lng)

        Dim result As Double = sCoordinate.GetDistanceTo(eCoordinate)

        Return result
    End Function


    Friend Function lat2m(ByVal lat As Double) As Double
        'Dim result As Double = Me.mPerLatDegreeAtLat(lat) * lat
        Dim sCoordinate As New GeoCoordinate(Me.reference.Latitude, Me.reference.Longitude)
        Dim eCoordinate As New GeoCoordinate(lat, Me.reference.Longitude)

        Dim result As Double = eCoordinate.GetDistanceTo(sCoordinate)

        Return result
    End Function


End Class
