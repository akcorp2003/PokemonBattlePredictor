Public Class Poke_Calculator

    ''' <summary>
    ''' Calculates damage done by attack_move. Uses the Formula provided by bulbapedia.
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
                                    ByVal EFF As Double, ByVal MAXMIN As Integer) As Integer
        Dim damage As Integer = 0
        Dim MODIFIER As Double
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

    ''' <summary>
    ''' Applies the damage that attack_move inflicts on defender. The function can accept non-damaging moves.
    ''' The function also applies any effects attack_move may have.
    ''' </summary>
    ''' <param name="attacker"></param>
    ''' <param name="defender"></param>
    ''' <param name="attack_move">The move used by attacker.</param>
    ''' <param name="poke_calc"></param>
    ''' <param name="max_or_min">Max(1) damage, Min(-1) damage, or Norm(0) damage.</param>
    ''' <remarks></remarks>
    Public Sub apply_damage(ByRef attacker As Pokemon, ByRef defender As Pokemon, ByVal attack_move As Move_Info,
                            ByVal poke_calc As Poke_Calculator, ByVal max_or_min As Integer)
        'REM first check if attacking_move is a damaging move. If not, this function cannot accept it.
        'If attack_move.Power = 0 Then
        '    Return
        'End If

        Dim eff_table As New EffectivenessTable
        Dim damagevalue As Integer = 0
        Dim EFF As Double

        EFF = eff_table.Effective_Type(attack_move.Type, defender.Types)

        Me.apply_moveeffect(attacker, defender, attack_move)
        damagevalue = Me.CalculateDamage(attacker, defender, attack_move, EFF, max_or_min)

        REM damage value differs depending on the status condition of the attacker
        If attacker.Status_Condition = Constants.StatusCondition.burn Then
            If attack_move.Is_Special = False Then
                damagevalue = damagevalue / 2
            End If

        End If

        REM apply the damage to the defending pokemon
        REM in the future we will apply the special damages such as burn, poison type
        defender.HP = defender.HP - damagevalue

    End Sub

    ''' <summary>
    ''' Applies the damage when the confused_pokemon is confused.
    ''' </summary>
    ''' <param name="confused_pokemon">The confused pokemon</param>
    ''' <param name="poke_calc"></param>
    ''' <returns>A boolean value indicating whether the confused_pokemon hit itself or not.</returns>
    ''' <remarks></remarks>
    Public Function apply_confusion(ByVal confused_pokemon As Pokemon, ByVal poke_calc As Poke_Calculator) As Boolean
        Dim confuse_success As Boolean = False
        If confused_pokemon.Other_Status_Condition = Constants.StatusCondition.confused Then
            Dim chance As Integer = Poke_Calculator.GenerateRandomNumber()
            If chance <= 50 Then
                REM 50% chance that the pokemon will hit itself
                confuse_success = True
                Dim confusemove As New Move_Info
                confusemove.Is_Special = False
                confusemove.Power = Constants.CONFUSE_DAMAGE
                confusemove.Name = "confused"
                Dim damage As Integer = poke_calc.CalculateDamage(confused_pokemon, confused_pokemon, confusemove, 1, 1)
                confused_pokemon.HP -= damage
            End If
        End If
        Return confuse_success
    End Function

    ''' <summary>
    ''' Applies the effect for all status moves. The move can be damaging or non-damaging.
    ''' </summary>
    ''' <param name="my_pokemon">The Pokemon using the move</param>
    ''' <param name="opponent_pokemon">The target pokemon (if the status applies to the opponent)</param>
    ''' <param name="move">The move used. Can be damaging or non-damaging.</param>
    ''' <remarks></remarks>
    Public Sub apply_moveeffect(ByVal my_pokemon As Pokemon, ByVal opponent_pokemon As Pokemon, ByVal move As Move_Info)
        Dim effectlist As String() = move.Effect.Split(",")
        If effectlist.Length = 0 Then
            Return
        End If

        Dim chance As Integer
        Dim chance_success As Boolean = False
        For i As Integer = 0 To effectlist.Length - 1 Step 1
            If effectlist(i).Contains("chance") Then
                chance = Convert.ToInt32(effectlist(i + 1)) REM since the next array element contains the chance value, guaranteed. CAN LEAD TO REFERENCE ISSUE!
                REM compute the chance
                Dim value As Integer = Poke_Calculator.GenerateRandomNumber()
                If value <= chance Then
                    chance_success = True
                Else
                    Continue For REM don't even bother with the next steps since we are not going to apply the effect anyway
                End If
            Else
                chance_success = True
            End If


            REM now test to see what kind of effect to apply
            If effectlist(i).Contains("SLP") And chance_success = True Then
                REM confirm it is targeting opponent
                If effectlist(i).Contains("O") Then
                    If opponent_pokemon.Status_Condition = Constants.StatusCondition.none Then
                        opponent_pokemon.Status_Condition = Constants.StatusCondition.sleep
                    End If
                    REM else we do nothing since every pokemon can only have one status condition at a time
                End If
            ElseIf effectlist(i).Contains("PSNB") And chance_success = True Then
                If effectlist(i).Contains("O") Then
                    If opponent_pokemon.Status_Condition = Constants.StatusCondition.none Then
                        opponent_pokemon.Status_Condition = Constants.StatusCondition.badly_poisoned
                    End If
                End If
            ElseIf effectlist(i).Contains("PSN") And chance_success = True Then
                If effectlist(i).Contains("O") Then
                    If opponent_pokemon.Status_Condition = Constants.StatusCondition.none Then
                        opponent_pokemon.Status_Condition = Constants.StatusCondition.poison
                    End If
                End If
            ElseIf effectlist(i).Contains("BRN") And chance_success = True Then
                If effectlist(i).Contains("O") Then
                    If opponent_pokemon.Status_Condition = Constants.StatusCondition.none Then
                        opponent_pokemon.Status_Condition = Constants.StatusCondition.burn
                    End If
                End If
            ElseIf effectlist(i).Contains("FRZ") And chance_success = True Then
                If effectlist(i).Contains("O") Then
                    If opponent_pokemon.Status_Condition = Constants.StatusCondition.none Then
                        opponent_pokemon.Status_Condition = Constants.StatusCondition.freeze
                    End If
                End If
            ElseIf effectlist(i).Contains("PRLYZ") And chance_success = True Then
                If effectlist(i).Contains("O") Then
                    If opponent_pokemon.Status_Condition = Constants.StatusCondition.none Then
                        opponent_pokemon.Status_Condition = Constants.StatusCondition.paralyzed
                    End If
                End If
            ElseIf effectlist(i).Contains("CONF") And chance_success = True Then
                If effectlist(i).Contains("CONFO") Or effectlist(i).Contains("CONFchanceO") Then
                    opponent_pokemon.Other_Status_Condition = Constants.StatusCondition.confused
                End If
            End If
        Next
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

    ''' <summary>
    ''' A wrapper that applies the functions apply_statustopokemon_before or apply_statustopokemon_after depending on the flag "before_or_after."
    ''' Call this when you do not want to manage the functions yourself.
    ''' </summary>
    ''' <param name="first_pokemon">First pokemon to check for and apply status conditions.</param>
    ''' <param name="second_pokemon">Second pokemon to check for and apply status conditions.</param>
    ''' <param name="arena"></param>
    ''' <param name="before_or_after">Before (-1) or After (1)</param>
    ''' <remarks></remarks>
    Public Sub apply_statustopokemon_wrapper(ByVal first_pokemon As Pokemon, ByVal second_pokemon As Pokemon, ByVal arena As Pokemon_Arena, ByVal before_or_after As Integer)
        If before_or_after = -1 Then
            Me.apply_statustopokemon_before(first_pokemon, arena)
            Me.apply_statustopokemon_before(second_pokemon, arena)
        ElseIf before_or_after = 1 Then
            Me.apply_statustopokemon_after(first_pokemon, arena)
            Me.apply_statustopokemon_after(second_pokemon, arena)
        Else
            Return
        End If
        Return
    End Sub

    ''' <summary>
    ''' Applies any status damage that the_pokemon may have. Only deals with burn, poison, and badly poisoned (toxic). 
    ''' </summary>
    ''' <param name="the_pokemon"></param>
    ''' <param name="arena"></param>
    ''' <remarks>Only computes damaging status.</remarks>
    Public Sub apply_statustopokemon_after(ByVal the_pokemon As Pokemon, ByVal arena As Pokemon_Arena)
        If the_pokemon.Status_Condition = Constants.StatusCondition.none Then
            Return
        End If

        If the_pokemon.Status_Condition = Constants.StatusCondition.burn Then
            the_pokemon.HP = the_pokemon.HP * (1 / 8)
        ElseIf the_pokemon.Status_Condition = Constants.StatusCondition.poison Then
            REM query the database for the original HP of the pokemon
            Dim original_hp As Integer = Form1.Get_PokemonDictionary.Get_Pokemon(Constants.Get_FormattedString(the_pokemon.Name)).HP
            Dim damage As Integer = original_hp * 1 / 8
            the_pokemon.HP = the_pokemon.HP - damage
        ElseIf the_pokemon.Status_Condition = Constants.StatusCondition.badly_poisoned Then
            Dim total_damage As Double = 1 / 16
            If the_pokemon.Team = "blue" Then
                For i As Integer = 1 To arena.Blue_NumBadPoison Step 1
                    total_damage = total_damage * i
                Next
                the_pokemon.HP -= total_damage
            ElseIf the_pokemon.Team = "red" Then
                For i As Integer = 1 To arena.Red_NumBadPoison Step 1
                    total_damage = total_damage * i
                Next
                the_pokemon.HP -= total_damage
            End If

        Else
            Return
        End If
        Return
    End Sub

    ''' <summary>
    ''' Determines if the_pokemon's status should be changed. This includes sleep, freeze.
    ''' </summary>
    ''' <param name="the_pokemon"></param>
    ''' <param name="arena"></param>
    ''' <remarks></remarks>
    Public Sub apply_statustopokemon_before(ByVal the_pokemon As Pokemon, ByVal arena As Pokemon_Arena)

        If the_pokemon.Status_Condition = Constants.StatusCondition.sleep Then
            REM first check if the sleep counters should expire
            If the_pokemon.Team = "red" Then
                If arena.Red_NumSleep > 3 Then
                    the_pokemon.Status_Condition = Constants.StatusCondition.none
                    Return
                End If
            ElseIf the_pokemon.Team = "blue" Then
                If arena.Blue_NumSleep > 3 Then
                    the_pokemon.Status_Condition = Constants.StatusCondition.none
                    Return
                End If
            End If

            Dim random As Integer = Poke_Calculator.GenerateRandomNumber()
            If random <= 33 AndAlso random >= 0 Then
                REM pokemon has a 33% chance of waking up each turn
                the_pokemon.Status_Condition = Constants.StatusCondition.none
            End If
        ElseIf the_pokemon.Status_Condition = Constants.StatusCondition.freeze Then
            Dim random As Integer = Poke_Calculator.GenerateRandomNumber()
            If random <= 20 AndAlso random >= 0 Then
                REM the pokemon has a 20% chance of thawing
                the_pokemon.Status_Condition = Constants.StatusCondition.none
            End If
        End If
    End Sub

    ''' <summary>
    ''' Determines if the pokemon is paralyzed and if so, if paralysis prohibits pokemon from moving
    ''' </summary>
    ''' <param name="pokemon"></param>
    ''' <returns>If paralysis prevents pokemon from moving</returns>
    ''' <remarks></remarks>
    Public Function apply_turnparalysis(ByVal pokemon As Pokemon) As Boolean
        If Not pokemon.Status_Condition = Constants.StatusCondition.paralyzed Then
            Return False
        End If
        Dim random As Integer
        random = Poke_Calculator.GenerateRandomNumber()
        If random <= 25 AndAlso random >= 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Shared Function GenerateRandomNumber(ByVal minimum As Double, ByVal maximum As Double) As Double
        Dim random As New Random()
        Return random.NextDouble() * (maximum - minimum) + minimum
    End Function

    ''' <summary>
    ''' Generates a number between 0 and 100
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
