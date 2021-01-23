Option Explicit On

<Serializable>
Public Class clsVisitedComboCollection
    Friend Structure structVisitedCombo
        Friend fromNodeID As String
        Friend selfNodeID As String
        Friend toNodeID As String
    End Structure

    Friend items As New List(Of structVisitedCombo)

    Friend Function containsCombo(ByVal proposedCombo As structVisitedCombo) As Boolean
        Dim result As Boolean = False
        For Each singleCombo As structVisitedCombo In Me.items
            If singleCombo.fromNodeID = proposedCombo.fromNodeID AndAlso
                    singleCombo.selfNodeID = proposedCombo.selfNodeID AndAlso
                    singleCombo.toNodeID = proposedCombo.toNodeID Then
                result = True
                Exit For
            End If
        Next
        Return result
    End Function

    Friend Sub addCombo(ByVal newCombo As structVisitedCombo)
        Me.items.Add(newCombo)
    End Sub

End Class
