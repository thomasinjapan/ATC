Option Explicit On
Imports System.Device.Location

<Serializable>
Public Class clsExitWay
    Inherits clsPathWay

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="towardsRunway">entrypoint for Exitway pointing towards runway</param>
    ''' <param name="towarsRamp">entrypoint for Exitway pointing towards ramp</param>
    ''' <param name="xElement"></param>
    Public Sub New(ByRef towardsRunway As clsTouchDownWayPoint, ByRef towarsRamp As clsConnectionPoint, ByRef xElement As XElement, ByRef reference As clsAirport.structGeoCoordinate)
        MyBase.New(towardsRunway, towarsRamp, clsWaySection.enumPathWayType.exitWay, xElement, reference)

    End Sub



End Class
