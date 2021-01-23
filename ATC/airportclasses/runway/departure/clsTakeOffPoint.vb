Option Explicit On
Imports System.Device.Location

<Serializable>
Public Class clsTakeOffPoint
    Inherits clsConnectionPoint

    Public Sub New(ByRef xElement As XElement, ByRef referencecoordinate As clsAirport.structGeoCoordinate)
        MyBase.New(xElement, referencecoordinate)
    End Sub
End Class
