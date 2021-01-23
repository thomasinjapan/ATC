Option Explicit On
Imports System.Device.Location

<Serializable>
Public Class clsNavigationPoint

    Friend Property pos_X As Double             'meters
    Friend Property pos_Y As Double             'meters
    Friend Property altitude As clsDistanceCollection
    Friend Property comment As String
    Friend Property name As String
    Friend Property isPOIGround As Boolean = False
    Friend Property isPOITower As Boolean = False
    Friend Property radarName As String
    Friend Property UIName As String
    Friend Property stripename As String

    Friend ReadOnly Property objectID As String

    Public Sub New(ByRef xelement As XElement, ByRef referencePoint As clsAirport.structGeoCoordinate)
        Dim referenceCoordinate As New GeoCoordinate(referencePoint.lat, referencePoint.lng)
        'use the reference coordinates to calculate the location x and y
        'decide meters based on what value is bigger
        Dim lng As Double = Nothing
        Dim lat As Double = Nothing
        Dim alt As Double = Nothing

        'if lnglat does not exist..
        If xelement.@lnglat Is Nothing Then
            '..assume that the values are explicitly stated
            lng = xelement.@lng
            lat = xelement.@lat
            alt = xelement.@alt
        Else
            'else asume that the data is in the right format
            lng = xelement.@lnglat.Split(",")(0)
            lat = xelement.@lnglat.Split(",")(1)
            alt = xelement.@lnglat.Split(",")(2)
        End If


        'calculate meters to reference based on reference coordinate
        If lng - referenceCoordinate.Longitude > 0 Then
            Me.pos_X = (New GeoCoordinate(lat, referenceCoordinate.Longitude)).GetDistanceTo(New GeoCoordinate(lat, lng))
        Else
            Me.pos_X = -(New GeoCoordinate(lat, referenceCoordinate.Longitude)).GetDistanceTo(New GeoCoordinate(lat, lng))
        End If

        If lat - referenceCoordinate.Latitude < 0 Then
            Me.pos_Y = (New GeoCoordinate(lat, referenceCoordinate.Longitude)).GetDistanceTo(New GeoCoordinate(referenceCoordinate.Latitude, referenceCoordinate.Longitude))
        Else
            Me.pos_Y = -(New GeoCoordinate(lat, referenceCoordinate.Longitude)).GetDistanceTo(New GeoCoordinate(referenceCoordinate.Latitude, referenceCoordinate.Longitude))

        End If

        Me.altitude = New clsDistanceCollection(alt, clsDistanceCollection.enumDistanceUnits.meters)

        'get basic values
        Me.name = xelement.@name
        Me.comment = xelement.@comment
        Me.objectID = xelement.@id
        Me.isPOIGround = xelement.@poiground = "true"
        Me.isPOITower = xelement.@poitower = "true"
        Me.radarName = xelement.@radarname
        Me.uiname = xelement.@uiname
        Me.stripename = xelement.@stripename


    End Sub


End Class
