Option Explicit On

<Serializable>
Public Class clsMovingObject

    Private _direction As Double = 0

    Public Structure structPosition
        Friend pos_X As clsDistanceCollection
        Friend pos_Y As clsDistanceCollection
        Friend pos_Altitude As clsDistanceCollection
        Friend pos_direction As Double
    End Structure

    Public Structure structMovement
        Friend speed_absolute As clsSpeedCollection
        Friend speed_rotation As Double
    End Structure

    Public Structure structAcceleration
        Dim ground_accelleration_speed As Double
        Dim ground_accelleration_angle As Double
        Dim air_accelleration_speed As Double
        Dim air_accelleration_angle As Double
    End Structure


    Friend Property pos_X As clsDistanceCollection
    Friend Property pos_Y As clsDistanceCollection
    Friend Property pos_direction As Double
        Get
            Return Me._direction
        End Get
        Set(value As Double)
            If value <= 0 Then value += 360
            If value > 360 Then value -= 360
            Me._direction = value
        End Set
    End Property
    Friend Property pos_Altitude As clsDistanceCollection

    Friend Property mov_speed_absolute As clsSpeedCollection
    Friend Property mov_speed_rotation As Double

    Friend Property target_speed As clsSpeedCollection
    Friend Property target_direction As Double
    Friend Property target_altitude As clsDistanceCollection

    Friend Property pointDetectionCircle As clsDistanceCollection           'is the size how far the plane is allowed to be separated from the targetpoint

    Friend ReadOnly Property speed_X As Double
        Get
            Return (Me.mov_speed_absolute.inMetersPerSecond * Math.Sin(Math.PI * Me.pos_direction / 180))
        End Get
    End Property

    Friend ReadOnly Property speed_Y As Double
        Get
            Return (Me.mov_speed_absolute.inMetersPerSecond * Math.Cos(Math.PI * Me.pos_direction / 180))
        End Get
    End Property

    Public Sub New()

    End Sub
    Public Sub New(ByVal positiondata As structPosition, ByVal movementData As structMovement, ByVal accelerationProperties As structAcceleration)
        Me.pos_X = New clsDistanceCollection(positiondata.pos_X.meters, clsDistanceCollection.enumDistanceUnits.meters)
        Me.pos_Y = New clsDistanceCollection(positiondata.pos_Y.meters, clsDistanceCollection.enumDistanceUnits.meters)
        Me.pos_Altitude = New clsDistanceCollection(positiondata.pos_Altitude.feet, clsDistanceCollection.enumDistanceUnits.feet)
        Me.pos_direction = positiondata.pos_direction

        Me.mov_speed_absolute = movementData.speed_absolute
        Me.mov_speed_rotation = movementData.speed_rotation

        Me.target_speed = New clsSpeedCollection(Me.mov_speed_absolute.knots)
        Me.target_altitude = New clsDistanceCollection(Me.pos_Altitude.feet, clsDistanceCollection.enumDistanceUnits.feet)
        Me.target_direction = Me.pos_direction

    End Sub

    Friend Sub move(ByVal timespan As TimeSpan)
        Me.pos_X.meters += Me.speed_X * timespan.TotalSeconds
        Me.pos_Y.meters -= Me.speed_Y * timespan.TotalSeconds
    End Sub


End Class
