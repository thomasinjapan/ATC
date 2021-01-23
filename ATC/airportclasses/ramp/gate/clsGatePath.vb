Option Explicit On
Imports System.Device.Location

<Serializable>
Public Class clsGatePath
    Inherits clsPathWay

    Public Sub New(ByRef entryPoint As clsConnectionPoint, ByRef Gate As clsGate, ByRef xElement As XElement, ByRef reference As clsAirport.structGeoCoordinate)
        MyBase.New(entryPoint, Gate, clsWaySection.enumPathWayType.gateWay, xElement, reference)

    End Sub

    Public Sub New(ByRef entryPoint As clsConnectionPoint, ByRef exitPoint As clsConnectionPoint, ByRef xElement As XElement, ByRef reference As clsAirport.structGeoCoordinate)
        MyBase.New(entryPoint, exitPoint, clsWaySection.enumPathWayType.gateWay, xElement, reference)

    End Sub
End Class
