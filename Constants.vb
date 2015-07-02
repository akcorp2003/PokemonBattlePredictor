Module Constants
    Public Const LEVEL As Integer = 100 REM for TPP levels


    Enum Types
        normal = 0
        fighting = 1
        flying = 2
        poison = 3
        ground = 4
        rock = 5
        bug = 6
        ghost = 7
        steel = 8
        fire = 9
        water = 10
        grass = 11
        electric = 12
        psychic = 13
        ice = 14
        dragon = 15
        dark = 16
        fairy = 17
    End Enum

    Public Function Get_CriticalStageValue(ByVal stage As Integer) As Double
        If stage = 0 Then
            Return 1 / 16
        ElseIf stage = 1 Then
            Return 1 / 8
        ElseIf stage = 2 Then
            Return 1 / 4
        ElseIf stage = 3 Then
            Return 1 / 3
        ElseIf stage >= 4 Then
            Return 1 / 2
        Else
            Return -100
        End If
    End Function

    ''' <summary>
    ''' Takes in a string and formats the string by:
    ''' 1. Removing any leading and ending whitespace
    ''' 2. Removes any quotations
    ''' 3. Lowercases all letters
    ''' </summary>
    ''' <param name="ugly_string"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Get_FormattedString(ByRef ugly_string As String) As String
        ugly_string = ugly_string.Trim()
        ugly_string = ugly_string.Trim("""")
        ugly_string = ugly_string.ToLower()

        Return ugly_string
    End Function
End Module
