Option Explicit On

Module mdlHelpers

    ''' <summary>
    ''' returns the difference between two angles as a value between 0 and 180
    ''' </summary>
    ''' <param name="angle1">angle 1 in degree </param>
    ''' <param name="angle2">angle 2 in degree</param>
    ''' <returns>difference in degree w/ a value between 0 and 180</returns>
    Friend Function diffBetweenAnglesAbs(ByVal angle1 As Double, ByVal angle2 As Double) As Double
        Dim result As Double = (360 + Math.Abs(angle2 - angle1)) Mod 360
        If result > 180 Then result -= 360

        result = Math.Abs(result)
        Return result
    End Function

    ''' <summary>
    ''' returns the difference between angles as a value between -180 and 180
    ''' </summary>
    ''' <param name="angleFrom">angle to measure from</param>
    ''' <param name="angleTo">angle to measure to</param>
    ''' <returns></returns>
    Friend Function diffBetweenAngles(ByVal angleFrom As Double, ByVal angleTo As Double) As Double
        Dim result As Double = (360 + (angleTo - angleFrom)) Mod 360

        If result > 180 Then result -= 360
        If result < -180 Then result += 360

        Return result
    End Function

    ''' <summary>
    ''' returns the distance between two points as a positive value
    ''' </summary>
    ''' <param name="x1">x coordinate of point 1</param>
    ''' <param name="y1">y coordinate of point 1</param>
    ''' <param name="x2">x coordinate of point 2</param>
    ''' <param name="y2">y coordinate of point 2</param>
    ''' <returns>distance between as a positive value</returns>
    Friend Function diffBetweenPoints2D(ByVal x1 As Double, ByVal y1 As Double, ByVal x2 As Double, ByVal y2 As Double) As Double
        Dim result As Double = Math.Sqrt((x2 - x1) ^ 2 + (y2 - y1) ^ 2)

        Return result
    End Function

    Friend Function diffBetweenPoints3D(ByVal x1 As Double, ByVal y1 As Double, ByVal z1 As Double, ByVal x2 As Double, ByVal y2 As Double, ByVal z2 As Double) As Double
        Dim result As Double = Math.Sqrt((x2 - x1) ^ 2 + (y2 - y1) ^ 2 + (z2 - z1) ^ 2)

        Return result
    End Function
End Module
