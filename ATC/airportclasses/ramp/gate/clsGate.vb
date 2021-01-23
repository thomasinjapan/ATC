Option Explicit On
Imports System.Device.Location

<Serializable>
Public Class clsGate
    Inherits clsConnectionPoint

    '   Friend Property name As String
    Friend Property parkingDirection As Double
    Friend Property minWidth As clsDistanceCollection
    Friend Property maxWidth As clsDistanceCollection
    Friend Property IATAs As New List(Of String)

    Public Sub New(ByRef xElement As XElement, ByRef reference As clsAirport.structGeoCoordinate)
        MyBase.New(xElement, reference)
        Me.parkingDirection = xElement.@parkingdirection
        Me.minWidth = New clsDistanceCollection(xElement.@minwidth, clsDistanceCollection.enumDistanceUnits.meters)
        Me.maxWidth = New clsDistanceCollection(xElement.@maxwidth, clsDistanceCollection.enumDistanceUnits.meters)

        If Not xElement.@iatas Is Nothing Then
            For Each singleIATA As String In xElement.@iatas.Split(",")
                Me.IATAs.Add(singleIATA)
            Next
        End If
        '      Me.name = xElement.@name
    End Sub

End Class
