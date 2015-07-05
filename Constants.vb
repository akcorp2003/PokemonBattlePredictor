﻿Module Constants
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

    Public Function Get_StageBoostValue(ByVal stage As Integer) As Double
        If stage = 0 Then
            Return 1.0 REM no effect
        ElseIf stage = 1 Then
            Return 3 / 2
        ElseIf stage = 2 Then
            Return 4 / 2
        ElseIf stage = 3 Then
            Return 5 / 2
        ElseIf stage = 4 Then
            Return 6 / 2
        ElseIf stage = 5 Then
            Return 7 / 2
        ElseIf stage = 6 Then
            Return 8 / 2
        ElseIf stage = -1 Then
            Return 2 / 3
        ElseIf stage = -2 Then
            Return 2 / 4
        ElseIf stage = -3 Then
            Return 2 / 5
        ElseIf stage = -4 Then
            Return 2 / 6
        ElseIf stage = -5 Then
            Return 2 / 7
        ElseIf stage = -6 Then
            Return 2 / 8
        Else
            Return 0
        End If
    End Function

    Public Function Get_EffectString(ByVal effect_id As Integer) As String
        If effect_id = 11 Then
            Return "ATKU+1"
        ElseIf effect_id = 12 Then
            Return "DEFU+1"
        ElseIf effect_id = 14 Then
            Return "SPATKU+1"
        ElseIf effect_id = 17 Then
            Return "EVAU+1"
        ElseIf effect_id = 19 Then
            Return "ATKO-1"
        ElseIf effect_id = 20 Then
            Return "DEFO-1"
        ElseIf effect_id = 21 Then
            Return "SPDO-1"
        ElseIf effect_id = 24 Then
            Return "ACCUO-1"
        ElseIf effect_id = 25 Then
            Return "EVAO-1"
        ElseIf effect_id = 51 Then
            Return "ATKU+2"
        ElseIf effect_id = 52 Then
            Return "DEFU+2"
        ElseIf effect_id = 53 Then
            Return "SPDU+2"
        ElseIf effect_id = 54 Then
            Return "SPATKU+2"
        ElseIf effect_id = 55 Then
            Return "SPDEFU+2"
        ElseIf effect_id = 59 Then
            Return "ATKO-2"
        ElseIf effect_id = 60 Then
            Return "DEFO-2"
        ElseIf effect_id = 61 Then
            Return "SPDO-2"
        ElseIf effect_id = 63 Then
            Return "SPDEFO-2"
            REM explore 101
        ElseIf effect_id = 109 Then
            Return "EVAU+2"
        ElseIf effect_id = 119 Then
            Return "ATKO+2,CONFO"
        ElseIf effect_id = 146 Then
            Return "DEFU+1" REM also user charges for one turn before attacking
        ElseIf effect_id = 157 Then
            Return "DEFU+1"
        ElseIf effect_id = 167 Then
            Return "SPATKO+1,CONFO"
        ElseIf effect_id = 169 Then
            Return "ATKO-2,SPATKO-2,FAINTU"
        ElseIf effect_id = 175 Then
            Return "SPDEFU+1" REM also electric power doubled next turn
        ElseIf effect_id = 183 Then
            Return "ATKU-1,DEFU-1" REM after inflicting damage
            REM Explore 190
        ElseIf effect_id = 205 Then
            Return "SPATKU-2" REM afer inflicting damage
        ElseIf effect_id = 206 Then
            Return "ATKO-1,DEFO-1"
        ElseIf effect_id = 207 Then
            Return "DEFU+1,SPDEFU+1"
        ElseIf effect_id = 209 Then
            Return "ATKU+1,DEFU+1"
        ElseIf effect_id = 212 Then
            Return "SPATKU+1,SPDEFU+1"
        ElseIf effect_id = 213 Then
            Return "ATKU+1,SPDU+1"
        ElseIf effect_id = 219 Then
            Return "SPDU-1"
        ElseIf effect_id = 230 Then
            Return "DEFU-1,SPDEFU-1" REM after inflicting damage
        ElseIf effect_id = 259 Then
            Return "EVAO-1" REM removes field effects on other side
            REM explore 266
        ElseIf effect_id = 278 Then
            Return "ATKU+1,ACCUU+1"
        ElseIf effect_id = 285 Then
            Return "SPDU+2" REM user's weight is halved
        ElseIf effect_id = 291 Then
            Return "SPATKU+1,SPDEFU+1,SPDU+1"
        ElseIf effect_id = 296 Then
            Return "SPDU+1"
        ElseIf effect_id = 297 Then
            Return "SPDEFO-2"
        ElseIf effect_id = 309 Then
            Return "ATKU+2,SPATKU+2,SPDU+2,DEFU-1,SPDEFU-1"
        ElseIf effect_id = 313 Then
            Return "ATKU+1,SPDU+2"
        ElseIf effect_id = 317 Then
            Return "ATKU+1,SPATKU+1"
        ElseIf effect_id = 322 Then
            Return "SPATKU+3"
        ElseIf effect_id = 323 Then
            Return "ATKU+1,DEFU+1,ACCUU+1"
        ElseIf effect_id = 328 Then
            Return "ATKU+1,SPATKU+1"
        ElseIf effect_id = 329 Then
            Return "DEFU+3"
        ElseIf effect_id = 331 Then
            Return "SPDO-1"
        ElseIf effect_id = 335 Then
            Return "DEFU-1,SPDEFU-1,SPDU-1"
            REM explore 342, 344, 355later
        ElseIf effect_id = 346 Then
            Return "ATKO-1,SPATKO-1"
        ElseIf effect_id = 351 Then
            Return "ATKO-1,SPATKO-1" REM user switches out
            REM explore 360
        ElseIf effect_id = 361 Then
            Return "SPATKO-1"
        ElseIf effect_id = 364 Then
            Return "ATKO-1,SPATKO-1,SPDO-1" REM if O is poisoned
        ElseIf effect_id = 366 Then
            Return "SPATKU+2,SPDEFU+2,SPD+2"
        ElseIf effect_id = 269 Then
            Return "SPATKO-2"
        ElseIf effect_id = 10004 Then
            Return "EVAO-2"
        Else
            Return ""
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