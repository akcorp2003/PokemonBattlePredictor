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
    Dim master_table(,) As Double = {{1, 1, 1, 1, 1, 0.5, 1, 0, 0.5, 1, 1, 1, 1, 1, 1, 1, 1, 1}, _
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
    ''' <param name="Attacking_Type">No need to format it. Function will format it accordingly.</param>
    ''' <param name="Defending_Type">No need to format it. Function will format it accordingly.</param>
    ''' <returns>A double. 0 is no effect, 0.5 is not very effective, 1 is normal, 
    ''' 2 is super effective, 100 is couldn't locate</returns>
    ''' <remarks></remarks>
    Public Function Effective_Type(ByVal Attacking_Type As String, ByVal Defending_Type As String) As Double
        REM first format the strings
        Attacking_Type = Constants.Get_FormattedString(Attacking_Type)

        Defending_Type = Constants.Get_FormattedString(Defending_Type)

        Dim attack_index As Integer = GetTypeIndexInChart(Attacking_Type)
        Dim defend_index As Integer = GetTypeIndexInChart(Defending_Type)
        If attack_index = -1 Or defend_index = -1 Then
            Return 100
        End If

        Return master_table(attack_index, defend_index)

    End Function

    ''' <summary>
    ''' Overloaded version. Takes the attacking type and calculates the effectiveness given a list of types for the defending pokemon
    ''' </summary>
    ''' <param name="Attacking_Type"></param>
    ''' <param name="Defending_Types">List of Strings of the types of the pokemon</param>
    ''' <returns>A Double that indicates the effective type of the battle</returns>
    ''' <remarks></remarks>
    Public Function Effective_Type(ByVal Attacking_Type As String, ByVal Defending_Types As List(Of String)) As Double
        Dim EFF As Double = 1.0
        Dim def_types As String() = Defending_Types.ToArray()
        Attacking_Type = Constants.Get_FormattedString(Attacking_Type)
        Dim attack_index As Integer = GetTypeIndexInChart(Attacking_Type)

        Dim i As Integer = 0
        While i < def_types.Length
            Dim defend_index As Integer = GetTypeIndexInChart(def_types(i))
            If attack_index = -1 Or defend_index = -1 Then
                EFF = 0 REM immediately throw this move under the bus
            Else
                EFF = master_table(attack_index, defend_index) * EFF
            End If
            i += 1
        End While

        Return EFF
    End Function

    Public Function GetTypeIndexInChart(ByVal type As String) As Integer
        type = Constants.Get_FormattedString(type)
        If type = "normal" Then
            Return Types.normal
        ElseIf type = "fighting" Then
            Return Types.fighting
        ElseIf type = "flying" Then
            Return Types.flying
        ElseIf type = "poison" Then
            Return Types.poison
        ElseIf type = "ground" Then
            Return Types.ground
        ElseIf type = "rock" Then
            Return Types.rock
        ElseIf type = "bug" Then
            Return Types.bug
        ElseIf type = "ghost" Then
            Return Types.ghost
        ElseIf type = "steel" Then
            Return Types.steel
        ElseIf type = "fire" Then
            Return Types.fire
        ElseIf type = "water" Then
            Return Types.water
        ElseIf type = "grass" Then
            Return Types.grass
        ElseIf type = "electric" Then
            Return Types.electric
        ElseIf type = "psychic" Then
            Return Types.psychic
        ElseIf type = "ice" Then
            Return Types.ice
        ElseIf type = "dragon" Then
            Return Types.dragon
        ElseIf type = "dark" Then
            Return Types.dark
        ElseIf type = "fairy" Then
            Return Types.fairy
        Else
            Return -1
        End If
    End Function

    Public Function GetTypeName(ByVal type_num As Integer) As String
        If type_num = Types.normal Then
            Return "normal"
        ElseIf type_num = Types.fighting Then
            Return "fighting"
        ElseIf type_num = Types.flying Then
            Return "flying"
        ElseIf type_num = Types.poison Then
            Return "poison"
        ElseIf type_num = Types.ground Then
            Return "ground"
        ElseIf type_num = Types.rock Then
            Return "rock"
        ElseIf type_num = Types.bug Then
            Return "bug"
        ElseIf type_num = Types.ghost Then
            Return "ghost"
        ElseIf type_num = Types.steel Then
            Return "steel"
        ElseIf type_num = Types.fire Then
            Return "fire"
        ElseIf type_num = Types.water Then
            Return "water"
        ElseIf type_num = Types.grass Then
            Return "grass"
        ElseIf type_num = Types.electric Then
            Return "electric"
        ElseIf type_num = Types.psychic Then
            Return "psychic"
        ElseIf type_num = Types.ice Then
            Return "ice"
        ElseIf type_num = Types.dragon Then
            Return "dragon"
        ElseIf type_num = Types.dark Then
            Return "dark"
        ElseIf type_num = Types.fairy Then
            Return "fairy"
        Else
            Return -1
        End If
    End Function




End Class

