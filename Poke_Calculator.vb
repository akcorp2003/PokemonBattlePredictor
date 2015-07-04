Public Class Poke_Calculator

    ''' <summary>
    ''' Uses the Formula provided by bulbapedia
    ''' </summary>
    ''' <param name="attacking_pokemon"></param>
    ''' <param name="defending_pokemon"></param>
    ''' <param name="attack_move"></param>
    ''' <param name="EFF">Indicates how effective is the attacking pokemon move on the defending pokemon</param>
    ''' <param name="MAXMIN">Indicates if user wants to apply max(1) or min (-1) possible damage. 0 is
    ''' regular damage. </param>
    ''' <returns>The damage value</returns>
    ''' <remarks></remarks>
    Public Function CalculateDamage(ByVal attacking_pokemon As Pokemon, ByVal defending_pokemon As Pokemon, ByVal attack_move As Move_Info, _
                                    ByVal EFF As ULong, ByVal MAXMIN As Integer) As Integer
        Dim damage As Integer = 0
        Dim MODIFIER As ULong
        Dim CRITICAL As Integer
        Dim STAB As Integer
        Dim OTHER As Integer = 1
        Dim ATTACKING_ATK As Integer REM could be ATK or Sp_ATK
        Dim DEFENDING_DEF As Integer REM coudl be DEF or Sp_DEF
        Dim random As New Random

        If attacking_pokemon.Types.Contains(attack_move.Type) = True Then
            STAB = 1.5
        Else
            STAB = 1
        End If

        REM these values will have the stage value applied
        REM that means, for instance, if a pokemon has its attack raised by 2 stages
        REM then its ATK value will reflect that increase
        If attack_move.Is_Special = True Then
            ATTACKING_ATK = attacking_pokemon.Sp_ATK
            DEFENDING_DEF = defending_pokemon.Sp_DEF
        Else
            ATTACKING_ATK = attacking_pokemon.ATK
            DEFENDING_DEF = defending_pokemon.DEF
        End If

        If IsCritical(GetCriticalStage(attacking_pokemon)) = True Then
            CRITICAL = 2
        Else
            CRITICAL = 1
        End If

        If MAXMIN = 0 Then
            REM apply damage according to formula normally
            MODIFIER = STAB * EFF * CRITICAL * OTHER * GenerateRandomNumber(0.85, 1)
        ElseIf MAXMIN = -1 Then
            REM apply min possible, no critical hit
            MODIFIER = STAB * EFF * OTHER * 0.85
        ElseIf MAXMIN = 1 Then
            REM apply max possible, with critical
            MODIFIER = STAB * EFF * 2 * OTHER * 1
        Else
            REM just apply regular damage
            MODIFIER = STAB * EFF * CRITICAL * OTHER * GenerateRandomNumber(0.85, 1)
        End If

        'NOTE: DEBUG AND CHECK THE RESULT OF THE DAMAGE, MAY NEED TO DO SOME ROUNDING
        damage = (((2 * LEVEL + 10) / 250) * (ATTACKING_ATK / DEFENDING_DEF) * attack_move.Power + 2) * MODIFIER

        Return damage
    End Function

    Public Sub apply_damage(ByRef attacker As Pokemon, ByRef defender As Pokemon, ByVal attack_move As Move_Info,
                            ByVal poke_calc As Poke_Calculator, ByVal max_or_min As Integer)
        REM first check if attacking_move is a damaging move. If not, this function cannot accept it.
        If attack_move.Power = 0 Then
            Return
        End If

        Dim eff_table As New EffectivenessTable
        Dim damagevalue As Integer = 0
        Dim EFF As ULong

        EFF = eff_table.Effective_Type(attack_move.Type, defender.Types)

        damagevalue = Me.CalculateDamage(attacker, defender, attack_move, EFF, max_or_min)

        REM apply the damage to the defending pokemon
        REM in the future we will apply the special damages such as burn, poison type
        defender.HP = defender.HP - damagevalue

    End Sub

    ''' <summary>
    ''' Apples statchanging_move to my_pokemon. This could be raising damage_move by 1 stage, 2 stages, or lower 1 stage, etc...
    ''' This function cannot distinguish between an opponent or user move.
    ''' </summary>
    ''' <param name="my_pokemon">The move that applies damage</param>
    ''' <param name="statchanging_move">The non-damaging move</param>
    ''' <remarks>This function does not know who the damage_move belongs to.</remarks>
    Public Sub apply_stattopokemon(ByVal my_pokemon As Pokemon, ByVal statchanging_move As Move_Info)
        Dim effects As String() = statchanging_move.Effect.Split(",")
        Dim i As Integer = 0
        While i < effects.Length
            If effects(i).Contains("SPATK") Then
                If effects(i).Contains("SPATKU+1") Or effects(i).Contains("SPATKO+1") Then
                    my_pokemon.SP_ATK_Boost += 1
                ElseIf effects(i).Contains("SPATKU+2") Or effects(i).Contains("SPATKO+2") Then
                    my_pokemon.SP_ATK_Boost += 2
                ElseIf effects(i).Contains("SPATKU-1") Or effects(i).Contains("SPATKO-1") Then
                    my_pokemon.SP_ATK_Boost -= 1
                ElseIf effects(i).Contains("SPATKU-2") Or effects(i).Contains("SPATKO-2") Then
                    my_pokemon.SP_ATK_Boost -= 2
                Else
                    Return REM don't do anything

                End If
                Dim boostvalue As Double = Constants.Get_StageBoostValue(my_pokemon.SP_ATK_Boost)
                my_pokemon.Sp_ATK = my_pokemon.Sp_ATK * boostvalue
            ElseIf effects(i).Contains("SPDEF") Then
                If effects(i).Contains("SPDEFU+1") Or effects(i).Contains("SPDEFO+1") Then
                    my_pokemon.SP_DEF_Boost += 1
                ElseIf effects(i).Contains("SPDEFU+2") Or effects(i).Contains("SPDEFO+2") Then
                    my_pokemon.SP_DEF_Boost += 2
                ElseIf effects(i).Contains("SPDEFU-1") Or effects(i).Contains("SPDEFO-1") Then
                    my_pokemon.SP_DEF_Boost -= 1
                ElseIf effects(i).Contains("SPDEFU-2") Or effects(i).Contains("SPDEFO-2") Then
                    my_pokemon.SP_DEF_Boost -= 2
                Else
                    Return REM don't do anything

                End If
                Dim boostvalue As Double = Constants.Get_StageBoostValue(my_pokemon.SP_DEF_Boost)
                my_pokemon.Sp_DEF = my_pokemon.Sp_DEF * boostvalue
            ElseIf effects(i).Contains("ATK") Then
                If effects(i).Contains("ATKU+1") Or effects(i).Contains("ATKO+1") Then
                    my_pokemon.ATK_Boost += 1
                ElseIf effects(i).Contains("ATKU+2") Or effects(i).Contains("ATKO+2") Then
                    my_pokemon.ATK_Boost += 2
                ElseIf effects(i).Contains("ATKU-1") Or effects(i).Contains("ATKO-1") Then
                    my_pokemon.ATK_Boost -= 1
                ElseIf effects(i).Contains("ATKU-2") Or effects(i).Contains("ATKO-2") Then
                    my_pokemon.ATK_Boost -= 2
                Else
                    Return REM don't do anything

                End If
                Dim boostvalue As Double = Constants.Get_StageBoostValue(my_pokemon.ATK_Boost)
                my_pokemon.ATK = my_pokemon.ATK * boostvalue
            ElseIf effects(i).Contains("DEF") Then
                If effects(i).Contains("DEFU+1") Or effects(i).Contains("DEFO+1") Then
                    my_pokemon.DEF_Boost += 1
                ElseIf effects(i).Contains("DEFU+2") Or effects(i).Contains("DEFO+2") Then
                    my_pokemon.DEF_Boost += 2
                ElseIf effects(i).Contains("DEFU-1") Or effects(i).Contains("DEFO-1") Then
                    my_pokemon.DEF_Boost -= 1
                ElseIf effects(i).Contains("DEFU-2") Or effects(i).Contains("DEFO-2") Then
                    my_pokemon.DEF_Boost -= 2
                Else
                    Return REM don't do anything
                End If
                Dim boostvalue As Double = Constants.Get_StageBoostValue(my_pokemon.DEF_Boost)
                my_pokemon.DEF = my_pokemon.DEF * boostvalue

            ElseIf effects(i).Contains("SPD") Then
                If effects(i).Contains("SPDU+1") Or effects(i).Contains("SPDO+1") Then
                    my_pokemon.SPEED_Boost += 1
                ElseIf effects(i).Contains("SPDU+2") Or effects(i).Contains("SPDO+2") Then
                    my_pokemon.SPEED_Boost += 2
                ElseIf effects(i).Contains("SPDU-1") Or effects(i).Contains("SPDO-1") Then
                    my_pokemon.SPEED_Boost -= 1
                ElseIf effects(i).Contains("SPDU-2") Or effects(i).Contains("SPDO-2") Then
                    my_pokemon.SPEED_Boost -= 2
                Else
                    Return REM don't do anything

                End If
                Dim boostvalue As Double = Constants.Get_StageBoostValue(my_pokemon.SPEED_Boost)
                my_pokemon.SPD = my_pokemon.SPD * boostvalue
            Else
                Return
            End If

            i += 1
        End While
    End Sub

    Public Shared Function GenerateRandomNumber(ByVal minimum As Double, ByVal maximum As Double) As Double
        Dim random As New Random()
        Return random.NextDouble() * (maximum - minimum) + minimum
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns>A number between 0 and 100</returns>
    ''' <remarks></remarks>
    Public Shared Function GenerateRandomNumber() As Integer
        Dim random As New Random()
        Dim value As Integer = random.Next(0, 100)

        Return value
    End Function

    Public Function IsCritical(ByVal chance As Double) As Boolean
        Dim value As Integer = GenerateRandomNumber()
        Dim range As Integer = chance * 100 REM sets up the range of values that indicate a critical hit
        If value <= range Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Returns the percentage value that a critical hit can occur for the pokemon
    ''' </summary>
    ''' <param name="pokemon"></param>
    ''' <returns>The percentage value that the stage the pokemon is currently in.</returns>
    ''' <remarks></remarks>
    Public Function GetCriticalStage(ByVal pokemon As Pokemon) As Double
        Return Constants.Get_CriticalStageValue(pokemon.Stage)
    End Function
End Class
