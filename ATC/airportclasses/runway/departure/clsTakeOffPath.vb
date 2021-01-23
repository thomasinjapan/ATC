Option Explicit On
Imports System.Device.Location

<Serializable>
Public Class clsTakeOffPath
    Inherits clsPathWay

    Public Sub New(ByRef frontPoint As clsConnectionPoint, ByRef endPoint As clsConnectionPoint, ByRef xElement As XElement, ByRef reference As clsAirport.structGeoCoordinate)
        MyBase.New(frontPoint, endPoint, clsWaySection.enumPathWayType.takeOffWay, xElement, reference)
    End Sub
End Class
