Option Explicit On
Imports System.Device.Location

<Serializable>
Public Class clsLineUpWay
    Inherits clsPathWay

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="towardsRamp">entrypoint of path which is towards ramp</param>
    ''' <param name="towardsRunway">entrypoint of path which is towards runway</param>
    ''' <param name="xElement"></param>
    Public Sub New(ByRef towardsRamp As clsConnectionPoint, ByRef towardsRunway As clsTakeOffPoint, ByRef xElement As XElement, ByRef reference As clsAirport.structGeoCoordinate)
        MyBase.New(towardsRamp, towardsRunway, clsWaySection.enumPathWayType.lineUpWay, xElement, reference)
    End Sub

End Class
