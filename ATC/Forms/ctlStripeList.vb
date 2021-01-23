Option Explicit On

Class ctlStripeList
    Friend WithEvents stripeList As New List(Of ctlStripe)
    Friend Property selectedStripe As ctlStripe

    Friend Event stripeSelected(ByRef stripe As ctlStripe)

    Friend Sub addStripe(ByRef plane As clsPlane)
        'load stripe
        Dim newStripe As New ctlStripe

        newStripe.loadPlane(plane)

        'position stripe
        newStripe.Top = stripeList.Count * newStripe.Height
        If Not stripeList.Count = 0 Then newStripe.Top += Me.stripeList.First.Top
        newStripe.Left = 0

        'prepare stripelist and show stripe with event
        Me.stripeList.Add(newStripe)
        'Me.pnlStripes.Height += newStripe.Height
        Me.pnlStripes.Controls.Add(newStripe)

        AddHandler newStripe.stripeClicked, AddressOf stripeClicked
    End Sub

    Friend Sub removeStripe(ByRef plane As clsPlane)
        'go through all planes
        Dim isFound As Boolean = False
        Dim numberOfStripes As Long = Me.stripeList.Count - 1
        Dim C1 As Long = 0
        If Not Me.selectedStripe Is Nothing AndAlso Me.selectedStripe.plane Is plane Then
            Me.selectedStripe.Dispose()
            Me.selectedStripe = Nothing
        End If
        While C1 <= numberOfStripes
            If Me.stripeList(C1).plane Is plane Then
                'if found, delete it from list, register that info and make panel smaller
                'Me.pnlStripes.Height -= Me.stripeList(C1).Height
                Me.pnlStripes.Controls.Remove(Me.stripeList(C1))
                Me.stripeList(C1).Dispose()
                Me.stripeList.RemoveAt(C1)
                isFound = True

                'set counter one back so we do not overrun
                C1 -= 1
                numberOfStripes -= 1

            ElseIf isFound Then
                'if found in past, move self one item up
                Me.stripeList(C1).Top -= Me.stripeList(C1).Height
            End If
                C1 += 1
        End While
        Me.pnlStripes.Refresh()
        GC.Collect()
    End Sub

    ''' <summary>
    ''' handles selection of stripe
    ''' </summary>
    ''' <param name="stripe">selected stripe</param>
    Friend Sub stripeClicked(ByRef stripe As ctlStripe)
        'unselect all other stripes and then select this stripe
        For Each singleStripe As ctlStripe In Me.stripeList
            singleStripe.isSelected = False
        Next
        Me.selectedStripe = stripe
        stripe.isSelected = True
        RaiseEvent stripeSelected(stripe)
        'MsgBox(stripe.plane.callsign & " - " & stripe.Top & " | " & stripe.Top Mod 64)
    End Sub

    Friend Sub rePaint()
        For Each singlestripe As ctlStripe In Me.stripeList
            singlestripe.paintAllLabels()
        Next
    End Sub

    Friend Sub selectStripe(ByRef plane As clsPlane)
        For Each singleStripe As ctlStripe In Me.stripeList
            singleStripe.isSelected = False
            If singleStripe.plane Is plane Then
                Me.selectedStripe = singleStripe
                singleStripe.isSelected = True
            End If
        Next

    End Sub


End Class
