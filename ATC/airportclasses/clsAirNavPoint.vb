Option Explicit On

<Serializable>
Public Class clsAirNavPoint
    Inherits clsConnectionPoint

    Friend Enum enumAirNavPointType
        undefined = 0
        STAR = 1
        SID = 2
        VORTAC = 4
        pilot = 8
        ILS = 16
        LOC = 32
    End Enum

    Friend Property NavPointType As enumAirNavPointType

    Public Sub New(ByRef xElement As XElement, ByRef reference As clsAirport.structGeoCoordinate)
        MyBase.New(xElement, reference)

        'append an empty string to make sure that the value is not nothing
        xElement.@type = xElement.@type & ""
        Select Case xElement.@type.ToString.ToLower
            Case "star"
                Me.NavPointType = enumAirNavPointType.STAR
            Case "sid"
                Me.NavPointType = enumAirNavPointType.SID
            Case "vortac"
                Me.NavPointType = enumAirNavPointType.VORTAC
            Case "pilot"
                Me.NavPointType = enumAirNavPointType.pilot
            Case "ils"
                Me.NavPointType = enumAirNavPointType.ILS
            Case "loc"
                Me.NavPointType = enumAirNavPointType.LOC
            Case Else
                Me.NavPointType = enumAirNavPointType.undefined
        End Select
    End Sub
End Class
