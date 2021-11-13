Option Explicit On

Imports System.IO
Imports System.Net.Sockets
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Xml.Serialization

Module mdlNetworkhandling
    <Serializable> Public Enum enumNetworkMessageType As Byte
        keyframe = 1
    End Enum

    <Serializable> Friend Structure structNetworkMessageFromServer
        Friend nwmMessageType As enumNetworkMessageType
        Friend nwmMessage As Object
    End Structure

    <Serializable> Friend Structure structRadioMessageNetwork
        Friend frequency As clsPlane.enumFrequency
        Friend message As String
    End Structure

    <Serializable> Friend Structure structNetworkKeyframeMessagefromServer
        Friend planeSkeletons As List(Of clsPlane.structPlaneSkeleton)
        Friend windDirectionTo As Double
        Friend openArrivalRunwayIDs As List(Of String)
        Friend openDepartureRunwayIDs As List(Of String)
        Friend usedRunwayIDs As List(Of String)
        Friend radioMessage As structRadioMessageNetwork
    End Structure


    Friend Sub sendUpdateToClients(ByRef game As clsGame)
        If game.TCPServerClientPlayers.Count > 0 Then
            Dim planeSkeletons As New List(Of clsPlane.structPlaneSkeleton)

            For Each singlePlane As clsPlane In game.Planes
                Dim newplane As clsPlane.structPlaneSkeleton = singlePlane.skeleton

                planeSkeletons.Add(newplane)
            Next

            Dim radioMessage As structRadioMessageNetwork = Nothing
            If Not game.NetworkRadioMessageBuffer.Count = 0 Then
                radioMessage = game.NetworkRadioMessageBuffer.First
                game.NetworkRadioMessageBuffer.Remove(game.NetworkRadioMessageBuffer.First)
            End If

            Dim message As New structNetworkKeyframeMessagefromServer With {
                .openArrivalRunwayIDs = game.AirPort.openArrivalRunwayIDsAsListOfStrings,
                .openDepartureRunwayIDs = game.AirPort.openDepartureRunwayIDsAsListOfStrings,
                .usedRunwayIDs = game.AirPort.usedRunwayIDsAsListOfStrings,
                .windDirectionTo = game.AirPort.windDirectionTo,
                .planeSkeletons = planeSkeletons,
                .radioMessage = radioMessage
            }



            Try
                Dim formatter As New BinaryFormatter
                Dim streamTarget As New MemoryStream()
                Dim streamTargetMessage As New MemoryStream()

                formatter.Serialize(streamTargetMessage, message)
                'formatter.Serialize(streamTargetMessage, planeSkeletons(5))

                Dim byteArrayMessage() As Byte = streamTargetMessage.ToArray

                Dim arraySize As Int32 = byteArrayMessage.Length
                Dim arraySizeArray() As Byte = BitConverter.GetBytes(arraySize)
                Dim byteArray() As Byte = arraySizeArray.Concat(byteArrayMessage).ToArray

                ' Me.TCPServerClient.Send(byteArray)
                For Each singleClient As TcpClient In game.TCPServerClientPlayers
                    Dim stampTickStart As DateTime = Now
                    Console.WriteLine("end reading and handling package|" & Format(stampTickStart, "HH:mm:ss ffff"))
                    Console.WriteLine("package size sent to " & singleClient.Client.RemoteEndPoint.ToString & "|" & Format(stampTickStart, "HH:mm:ss ffff") & "|" & byteArray.Length)

                    singleClient.Client.Send(byteArray)

                    Dim stampTickEnd As DateTime = Now

                    Console.WriteLine("finished sending package|" & Format(stampTickEnd, "HH:mm:ss ffff"))
                    Console.WriteLine("duration sending package |" & (stampTickEnd - stampTickStart).TotalMilliseconds & "|" & (stampTickEnd - stampTickStart).Ticks)


                Next
            Catch ex As Exception

            End Try
        End If
    End Sub

End Module
