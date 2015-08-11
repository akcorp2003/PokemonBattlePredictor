Namespace PBP
    Module Constants
        Public Const LEVEL As Integer = 100 REM for TPP levels
        Public Const CONFUSE_DAMAGE As Integer = 40
        Public Const ACCURACY_HIGH As Integer = 25

        Dim m_iterationdict As New Dictionary(Of Integer, Integer)

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

        Enum StatusCondition
            none = 0
            burn = 1
            freeze = 2
            paralyzed = 3
            poison = 4
            badly_poisoned = 5
            sleep = 6
            confused = 7
            attracted = 8
        End Enum

        Enum Funct_IDs
            IterateParalysis = 10
            IterateConfusion = 20
            EvaluateGreenCase = 30
            ApplyBattle = 40
            ApplyBattle_SuperEffectiveBranch = 50
            ApplyBattle_NormalMoveBranch = 60
        End Enum

        Enum Stat_Type
            Affect_Opponent = 1
            Affect_Self = 2
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

        Public Function Get_EffectString(ByVal effect_id As Integer, ByVal CSV_line As String()) As String
            If effect_id = 2 Then
                Return "SLPO"
            ElseIf effect_id = 3 Then
                Return "PSNchanceO," + CSV_line(11) REM poison target with chance
            ElseIf effect_id = 4 Then
                Return "HPdrainO,0.5"
            ElseIf effect_id = 5 Then
                Return "BRNchanceO," + CSV_line(11)
            ElseIf effect_id = 6 Then
                Return "FRZchanceO," + CSV_line(11)
            ElseIf effect_id = 7 Then
                Return "PRLYZchanceO," + CSV_line(11)
            ElseIf effect_id = 9 Then
                Return "HPdrainOsleep,0.5" REM drain's half of damage inflicted on sleeping opponent
            ElseIf effect_id = 11 Then
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
            ElseIf effect_id = 32 Then
                Return "FLNCHchanceO," + CSV_line(11)
            ElseIf effect_id = 33 Then
                Return "HPhalfU"
            ElseIf effect_id = 34 Then
                Return "PSNBO"
                REM explore 37
            ElseIf effect_id = 38 Then
                Return "SLPU,HPfullU"
            ElseIf effect_id = 39 Then
                Return "HITKO" REM one-hit KO
            ElseIf effect_id = 40 Then
                Return "2turn,n"
            ElseIf effect_id = 50 Then
                Return "CONFO"
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
            ElseIf effect_id = 67 Then
                Return "PSNO"
            ElseIf effect_id = 68 Then
                Return "PRLYZO"
            ElseIf effect_id = 76 Then
                Return "2turn,n,FLNCHchanceO," + CSV_line(11)
            ElseIf effect_id = 77 Then
                Return "CONFchanceO," + CSV_line(11)
            ElseIf effect_id = 78 Then
                Return "DAM2," + "PSNchanceO," + CSV_line(11)
            ElseIf effect_id = 85 Then
                Return "HPdrainO,0.0625" REM 0.0625 => 1/16
                REM explore 93 - flnch and double turn
                REM explore 101
            ElseIf effect_id = 109 Then
                Return "EVAU+2"
            ElseIf effect_id = 119 Then
                Return "ATKO+2,CONFO"
            ElseIf effect_id = 126 Then
                Return "BRNchanceO," + CSV_line(11) REM thaw opponent
            ElseIf effect_id = 133 Then
                Return "HPhalfU"
            ElseIf effect_id = 146 Then
                Return "2turn,n,DEFU+1" REM also user charges for one turn before attacking
            ElseIf effect_id = 147 Then
                Return "FLNCHchanceO," + CSV_line(11)
            ElseIf effect_id = 151 Then
                Return "FLNCHchanceO," + CSV_line(11)
            ElseIf effect_id = 152 Then
                Return "2turn,n"
            ElseIf effect_id = 153 Then
                Return "PRLYZchanceO," + CSV_line(11)
            ElseIf effect_id = 156 Then
                Return "2turn,air" REM means the move takes 2 turns and pokemon goes into the air
            ElseIf effect_id = 157 Then
                Return "DEFU+1"
                REM explore 159 Fake Out
            ElseIf effect_id = 167 Then
                Return "SPATKO+1,CONFO"
            ElseIf effect_id = 168 Then
                Return "BRNO"
            ElseIf effect_id = 169 Then
                Return "ATKO-2,SPATKO-2,FAINTU"
            ElseIf effect_id = 175 Then
                Return "SPDEFU+1" REM also electric power doubled next turn
            ElseIf effect_id = 183 Then
                Return "ATKU-1,DEFU-1" REM after inflicting damage
            ElseIf effect_id = 188 Then
                Return "SLPnextO" REM opponent sleeps on next turn
                REM Explore 190
            ElseIf effect_id = 200 Then
                Return "CONFO"
            ElseIf effect_id = 201 Then
                Return "BRNchanceO," + CSV_line(11) REM increase critical hit chance
            ElseIf effect_id = 203 Then
                Dim chance As String = "," + CSV_line(11)
                Return "PSNBchanceO," + chance REM chance to badly affect target
            ElseIf effect_id = 205 Then
                Return "SPATKU-2" REM afer inflicting damage
            ElseIf effect_id = 206 Then
                Return "ATKO-1,DEFO-1"
            ElseIf effect_id = 207 Then
                Return "DEFU+1,SPDEFU+1"
            ElseIf effect_id = 209 Then
                Return "ATKU+1,DEFU+1"
            ElseIf effect_id = 210 Then
                Dim chance As String = "," + CSV_line(11)
                Return "PSNchanceO," + chance
            ElseIf effect_id = 212 Then
                Return "SPATKU+1,SPDEFU+1"
            ElseIf effect_id = 213 Then
                Return "ATKU+1,SPDU+1"
            ElseIf effect_id = 215 Then
                Return "HPhalfU" REM restore by half of its max hp
            ElseIf effect_id = 219 Then
                Return "SPDU-1"
            ElseIf effect_id = 230 Then
                Return "DEFU-1,SPDEFU-1" REM after inflicting damage
            ElseIf effect_id = 254 Then
                Return "BRNchanceO," + CSV_line(11) + "RECL,1/3"
            ElseIf effect_id = 256 Then
                Return "2turn,water"
            ElseIf effect_id = 257 Then
                Return "2turn,ground"
            ElseIf effect_id = 259 Then
                Return "EVAO-1" REM removes field effects on other side
            ElseIf effect_id = 261 Then
                Return "FRZchanceO," + CSV_line(11)
            ElseIf effect_id = 263 Then
                Return "RECL,1/3," + "PRLYZchanceO," + CSV_line(11)
            ElseIf effect_id = 264 Then
                Return "2turn,air,PRLYZchanceO," + CSV_line(11)
                REM explore 266
            ElseIf effect_id = 273 Then
                Return "2turn,van"
            ElseIf effect_id = 274 Then
                Return "FLNCHchanceO," + CSV_line(11) + ",BRNchanceO," + CSV_line(11)
            ElseIf effect_id = 275 Then
                Return "FLNCHchanceO," + CSV_line(11) + ",FRZchanceO," + CSV_line(11)
            ElseIf effect_id = 276 Then
                Return "PRLYZchanceO," + CSV_line(11) + ",FLNCHchanceO," + CSV_line(11)
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
            ElseIf effect_id = 312 Then
                Return "2turn,air"
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
            ElseIf effect_id = 330 Then
                Return "SLPchanceO," + CSV_line(11)
            ElseIf effect_id = 331 Then
                Return "SPDO-1"
            ElseIf effect_id = 332 Then
                Return "2turn,n,PRLYZchanceO," + CSV_line(11) REM also requires one turn to charge
            ElseIf effect_id = 333 Then
                Return "2turn,n,BRNchanceO," + CSV_line(11) REM also requires one turn to charge
            ElseIf effect_id = 335 Then
                Return "DEFU-1,SPDEFU-1,SPDU-1"
            ElseIf effect_id = 338 Then
                Return "CONFchanceO," + CSV_line(11)
                REM explore 342, 344, 355later
            ElseIf effect_id = 346 Then
                Return "ATKO-1,SPATKO-1"
            ElseIf effect_id = 348 Then
                Return "HPdrainO,1/2" REM also, inflicts damage to all opponents on the field
                REM explore 350
            ElseIf effect_id = 351 Then
                Return "ATKO-1,SPATKO-1" REM user switches out
            ElseIf effect_id = 353 Then
                Return "HPdrainO,0.75"
                REM explore 360
            ElseIf effect_id = 361 Then
                Return "SPATKO-1"
            ElseIf effect_id = 364 Then
                Return "ATKO-1,SPATKO-1,SPDO-1" REM if O is poisoned
            ElseIf effect_id = 366 Then
                Return "2turn,n,SPATKU+2,SPDEFU+2,SPD+2"
            ElseIf effect_id = 269 Then
                Return "SPATKO-2"
            ElseIf effect_id = 10004 Then
                Return "EVAO-2"
            Else
                Return ""
            End If
        End Function

        Public Sub turnon_iterationflag(ByVal funct_id As Integer)
            If m_iterationdict.ContainsKey(funct_id) Then
                m_iterationdict.Item(funct_id) = 1
            Else
                m_iterationdict.Add(funct_id, 1)
            End If
        End Sub

        Public Sub turnoff_iterationflag(ByVal funct_id As Integer)
            If m_iterationdict.ContainsKey(funct_id) Then
                m_iterationdict.Item(funct_id) = 0
            Else
                m_iterationdict.Add(funct_id, 0)
            End If
        End Sub

        ''' <summary>
        ''' Returns the state of the iteration flag for a function. -100 means that the function does not exist in the dictionary.
        ''' 1 -> flag is on
        ''' 0 -> flag is off
        ''' </summary>
        ''' <param name="funct_id"></param>
        ''' <returns>An integer specifying the state of the iteration flag.</returns>
        ''' <remarks></remarks>
        Public Function stateof_iterationflag(ByVal funct_id As Integer) As Integer
            Dim value As Integer = -100
            If m_iterationdict.TryGetValue(funct_id, value) Then
                Return value
            Else
                Return value
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
            If ugly_string Is Nothing Then
                Return ""
            End If
            ugly_string = ugly_string.Trim()
            ugly_string = ugly_string.Trim("""")
            ugly_string = ugly_string.ToLower()

            Return ugly_string
        End Function

    End Module
End Namespace
