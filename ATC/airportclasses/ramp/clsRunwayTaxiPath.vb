Option Explicit On
Imports System.Device.Location

<Serializable>
Public Class clsRunwayTaxiPath
    Inherits clsPathWay

    Public Sub New(ByRef runwayFacing As clsConnectionPoint, ByRef rampFacing As clsConnectionPoint, ByRef xElement As XElement, ByRef reference As clsAirport.structGeoCoordinate)
        MyBase.New(runwayFacing, rampFacing, clsWaySection.enumPathWayType.runwayTaxiWay, xElement, reference)
    End Sub
End Class
