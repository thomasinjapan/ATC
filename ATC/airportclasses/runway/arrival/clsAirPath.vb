Option Explicit On

<Serializable>
Public Class clsAirPath
    Friend ReadOnly Property path As New List(Of clsastarengine.structPathStep)
    Friend ReadOnly Property name As String
    Friend ReadOnly Property object_ID As String

    Public Sub New(ByRef path As List(Of clsastarengine.structPathStep), ByVal name As String)
        Me.name = name
        Me.object_ID = name
        Me.path = path
    End Sub

End Class
