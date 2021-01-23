Option Explicit On

<Serializable>
Public Class clsLandscape
    Friend ReadOnly Property Polygons As New List(Of List(Of clsNavigationPoint))
    Friend ReadOnly Property lines As New List(Of List(Of clsNavigationPoint))

    Public Sub New(ByRef xelement As XElement, ByRef referencePoint As clsAirport.structGeoCoordinate)
        For Each SinglePolygon As XElement In xelement.<polygoncollection>
            Dim newPolygon As New List(Of clsNavigationPoint)
            For Each singlePoint As XElement In SinglePolygon.<coordinate>
                singlePoint.@name = SinglePolygon.@name & "-" & Guid.NewGuid.ToString
                newPolygon.Add(New clsNavigationPoint(singlePoint, referencePoint))
            Next
            Me.Polygons.Add(newPolygon)
        Next
        For Each singleLineCollection As XElement In xelement.<linecollection>
            Dim newLineCollection As New List(Of clsNavigationPoint)
            For Each singlePoint As XElement In singleLineCollection.<coordinate>
                singlePoint.@name = singleLineCollection.@name & "-" & Guid.NewGuid.ToString
                newLineCollection.Add(New clsNavigationPoint(singlePoint, referencePoint))
            Next
            Me.lines.Add(newLineCollection)
        Next
    End Sub
End Class
