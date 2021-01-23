Option Explicit On

<Serializable>
Public Class clsAStarCard
    Friend Property G As Double
    Friend ReadOnly Property H As Double
    Friend ReadOnly Property F As Double
        Get
            Return Me.H + Me.G
        End Get
    End Property
    Friend Property isClosed As Boolean = False
    Friend Property originNodeIndex As Long = -1
    Friend ReadOnly Property entryAngle As Double
    Friend ReadOnly Property wayToNode As clsNavigationPath
    Friend ReadOnly Property node As clsConnectionPoint

    Public Sub New(ByVal distanceFromStart As Double, ByVal distanceToGoal As Double, ByVal entryAngle As Double, ByVal originnodeindex As Long, ByRef node As clsConnectionPoint, ByRef wayToNode As clsNavigationPath)
        Me.H = distanceToGoal
        Me.G = distanceFromStart
        Me.entryAngle = entryAngle
        Me.originNodeIndex = originnodeindex
        Me.node = node
        Me.wayToNode = wayToNode
    End Sub



End Class
