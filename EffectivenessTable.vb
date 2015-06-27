Public Class EffectivenessTable

    'Since there is only a fixed number of types, we're going to hardcode the for loop
    '0 - normal
    '1 - fighting
    '2 - flying
    '3 - poison
    '4 - ground
    '5 - rock
    '6 - bug
    '7 - ghost
    '8 - steel
    '9 - fire
    '10 - water
    '11 - grass
    '12 - electric
    '13 - psychic
    '14 - ice
    '15 - dragon
    '16 - dark
    '17 - fairy
    Dim master_table(,) As ULong = {{1, 1, 1, 1, 1, 0.5, 1, 0, 0.5, 1, 1, 1, 1, 1, 1, 1, 1, 1}, _
                                    {2, 1, 0.5, 0.5, 1, 2, 0.5, 0, 2, 1, 1, 1, 1, 0.5, 2, 1, 2, 0.5}, _
                                    {1, 2, 1, 1, 1, 0.5, 2, 1, 0.5, 1, 1, 2, 0.5, 1, 1, 1, 1, 1}, _
                                    {1, 1, 1, 0.5, 0.5, 0.5, 1, 0.5, 0, 1, 1, 2, 1, 1, 1, 1, 1, 2}, _
                                    {1, 1, 0, 2, 1, 2, 0.5, 1, 2, 2, 1, 0.5, 2, 1, 1, 1, 1, 1}, _
                                    {1, 0.5, 2, 1, 0.5, 1, 2, 1, 0.5, 2, 1, 1, 1, 1, 2, 1, 1, 1}, _
                                    {1, 0.5, 0.5, 0.5, 1, 1, 1, 0.5, 0.5, 0.5, 1, 2, 1, 2, 1, 1, 2, 0.5}, _
                                    {0, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 0.5, 1}, _
                                    {1, 1, 1, 1, 1, 2, 1, 1, 0.5, 0.5, 0.5, 1, 0.5, 1, 2, 1, 1, 2}, _
                                    {1, 1, 1, 1, 1, 0.5, 2, 1, 2, 0.5, 0.5, 2, 1, 1, 2, 0.5, 1, 1}, _
                                    {1, 1, 1, 1, 2, 2, 1, 1, 1, 2, 0.5, 0.5, 1, 1, 1, 0.5, 1, 1}, _
                                    {1, 1, 0.5, 0.5, 2, 2, 0.5, 1, 0.5, 0.5, 2, 0.5, 1, 1, 1, 0.5, 1, 1}, _
                                    {1, 1, 2, 1, 0, 1, 1, 1, 1, 1, 2, 0.5, 0.5, 1, 1, 0.5, 1, 1}, _
                                    {1, 2, 1, 2, 1, 1, 1, 1, 0.5, 1, 1, 1, 1, 0.5, 1, 1, 0, 1}, _
                                    {1, 1, 2, 1, 2, 1, 1, 1, 0.5, 0.5, 0.5, 2, 1, 1, 0.5, 2, 1, 1}, _
                                    {1, 1, 1, 1, 1, 1, 1, 1, 0.5, 1, 1, 1, 1, 1, 1, 2, 1, 0}, _
                                    {1, 0.5, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1, 1, 2, 1, 1, 0.5, 0.5}, _
                                    {1, 2, 1, 0.5, 1, 1, 1, 1, 0.5, 0.5, 1, 1, 1, 1, 1, 2, 2, 1}}

    ''' <summary>
    ''' Returns the effectiveness of the pokemon as an unsigned long.
    ''' </summary>
    ''' <param name="Attacking_Type"></param>
    ''' <param name="Defending_Type"></param>
    ''' <returns>An unsigned long. 0 is no effect, 0.5 is not very effective, 1 is normal, 
    ''' 2 is super effective, 100 is couldn't locate</returns>
    ''' <remarks></remarks>
    Public Function Effective_Type(ByVal Attacking_Type As String, ByVal Defending_Type As String) As ULong
        Dim attack_index As Integer = GetTypeIndexInChart(Attacking_Type)
        Dim defend_index As Integer = GetTypeIndexInChart(Defending_Type)
        If attack_index = -1 Or defend_index = -1 Then
            Return 100
        End If

        Return master_table(attack_index, defend_index)

    End Function

    Public Function GetTypeIndexInChart(ByVal type As String) As Integer
        If type = "normal" Then
            Return 0
        ElseIf type = "fighting" Then
            Return 1
        ElseIf type = "flying" Then
            Return 2
        ElseIf type = "poison" Then
            Return 3
        ElseIf type = "ground" Then
            Return 4
        ElseIf type = "rock" Then
            Return 5
        ElseIf type = "bug" Then
            Return 6
        ElseIf type = "ghost" Then
            Return 7
        ElseIf type = "steel" Then
            Return 8
        ElseIf type = "fire" Then
            Return 9
        ElseIf type = "water" Then
            Return 10
        ElseIf type = "grass" Then
            Return 11
        ElseIf type = "electric" Then
            Return 12
        ElseIf type = "psychic" Then
            Return 13
        ElseIf type = "ice" Then
            Return 14
        ElseIf type = "dragon" Then
            Return 15
        ElseIf type = "dark" Then
            Return 16
        ElseIf type = "fairy" Then
            Return 17
        Else
            Return -1
        End If
    End Function




End Class

