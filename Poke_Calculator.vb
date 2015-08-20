Imports PokemonBattlePredictor
Imports PokemonBattlePredictor.PBP
Imports PokemonBattlePredictor.PBP.InfoBlocks
Imports PokemonBattlePredictor.PBP.Tables


Namespace PBP.Calculator
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

            If MAXMIN = Constants.Damage_Level.NORM Then
                REM apply damage according to formula normally
                MODIFIER = STAB * EFF * CRITICAL * OTHER * GenerateRandomNumber(0.85, 1)
            ElseIf MAXMIN = Constants.Damage_Level.MIN Then
                REM apply mi, no critical hit
                MODIFIER = STAB * EFF * OTHER * 0.85
            ElseIf MAXMIN = Constants.Damage_Level.MAX Then
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
                                ByVal poke_calc As Poke_Calculator, ByVal max_or_min As Integer, ByVal poke_arena As Pokemon_Arena)

            Dim eff_table As New EffectivenessTable
            Dim damagevalue As Integer = 0
            Dim EFF As Double

            EFF = eff_table.Effective_Type(attack_move.Type, defender.Types)

            If Not Logger.isMute() Then
                Logger.Record(attacker.Name + " uses " + attack_move.Name + " on " + defender.Name)
            End If

            REM check to see if move is a 2-turn
            If attacker.Team = "blue" Then
                If poke_arena.is_BlueMoveQueueEmpty() Then
                    If Me.apply_twoturnmove(attacker, defender, attack_move, poke_arena) Then
                        Return
                    End If
                Else
                    poke_arena.Dequeue_BlueMove()
                    poke_arena.Blue_Location = ""
                End If

            ElseIf attacker.Team = "red" Then
                If poke_arena.is_RedMoveQueueEmpty() Then
                    If Me.apply_twoturnmove(attacker, defender, attack_move, poke_arena) Then
                        Return
                    End If
                Else
                    poke_arena.Dequeue_RedMove()
                    poke_arena.Red_Location = ""
                End If
                
            End If

            If attack_move.Accuracy > 0 And attack_move.Accuracy < 100 Then
                Dim chance As Integer = Poke_Calculator.GenerateRandomNumber()
                If Not chance <= attack_move.Accuracy Then
                    REM the pokemon missed!!
                    If Not Logger.isMute() Then
                        Logger.Record("I")
                        Logger.Record(attacker.Name + " misses!")
                    End If
                    Return
                End If
            End If

            If attacker.Team = "blue" AndAlso Not poke_arena.Red_Location = "" Then
                If Not Logger.isMute() Then
                    Logger.Record("I")
                    Logger.Record(attacker.Name + " misses!")
                End If
                Return
            ElseIf attacker.Team = "red" AndAlso Not poke_arena.Blue_Location = "" Then
                If Not Logger.isMute() Then
                    Logger.Record("I")
                    Logger.Record(attacker.Name + " misses!")
                End If
                Return
            End If

            If Not Logger.isMute() Then
                If EFF = 2 Then
                    Logger.Record("I")
                    Logger.Record("It's super effective!")
                ElseIf EFF = 0.5 Then
                    Logger.Record("I")
                    Logger.Record("It's not very effective...")
                ElseIf EFF = 0 Then
                    Logger.Record("I")
                    Logger.Record("It has no effect...")
                End If
            End If

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

            Me.apply_recoil(attacker, defender, attack_move, damagevalue)
            Me.apply_drain(attacker, defender, attack_move, damagevalue)

            Dim is_pressure As Boolean = False
            For i As Integer = 0 To defender.Ability.Count - 1 Step 1
                If Constants.Get_FormattedString(defender.Ability.Item(i).Name) = "pressure" Then
                    attack_move.PP -= 2
                    is_pressure = True
                    Exit For
                End If
            Next
            If Not is_pressure Then
                attack_move.PP -= 1
            End If

            REM reason to place this check here is that we need to update the PP
            If attack_move.Effect = "KO" Then
                REM if we get here, it means that the KO move hits!
                defender.HP = 0
                If Not Logger.isMute() Then
                    Logger.Record("I")
                    Logger.Record("It's a one-hit KO!!")
                End If
                Return
            End If

            If Not Logger.isMute() Then
                Logger.Record("B")
                Logger.Record(defender.Name + " loses " + Convert.ToString(damagevalue) + " in HP!")
            End If

        End Sub

        ''' <summary>
        ''' Applies the recoil effect of recoiling_move. You don't need to worry if the recoil move is recoil or not. 
        ''' The function will handle that for you.
        ''' </summary>
        ''' <param name="first_pokemon"></param>
        ''' <param name="second_pokemon"></param>
        ''' <param name="recoiling_move"></param>
        ''' <param name="damage"></param>
        ''' <remarks></remarks>
        Public Sub apply_recoil(ByVal first_pokemon As Pokemon, ByVal second_pokemon As Pokemon, ByVal recoiling_move As Move_Info, ByVal damage As Integer)
            If Constants.Get_FormattedString(recoiling_move.Name) = "struggle" Then
                Dim original_hp As Integer = Form1.Get_PokemonDictionary().Get_Dictionary().Item(first_pokemon.Name).HP
                Dim totaldamage As Integer = Convert.ToInt32(Math.Round((1 / 4) * Convert.ToDouble(original_hp)))
                first_pokemon.HP -= totaldamage
                If Not Logger.isMute() Then
                    Logger.Record("I")
                    Logger.Record(first_pokemon.Name + " suffers damage from recoil!")
                End If
                Return
            End If

            Dim effect_list As String() = recoiling_move.Effect.Split(",")
            For i As Integer = 0 To effect_list.Length - 1 Step 1
                If effect_list(i).Contains("RECL") Then
                    Dim recoil_amount As Double = Convert.ToDouble(effect_list(i + 1))
                    Dim totaldamage As Integer = Convert.ToInt32(Math.Round(Convert.ToDouble(damage) * recoil_amount))
                    first_pokemon.HP -= totaldamage
                    If Not Logger.isMute() Then
                        Logger.Record("I")
                        Logger.Record(first_pokemon.Name + " suffers damage from recoil!")
                    End If
                End If
            Next
        End Sub

        Public Sub apply_drain(ByVal first_pokemon As Pokemon, ByVal second_pokemon As Pokemon, ByVal draining_move As Move_Info, ByVal damage As Integer)
            If draining_move Is Nothing Then
                Return
            End If

            Dim effect_list As String() = draining_move.Effect.Split(",")
            For i As Integer = 0 To effect_list.Length - 1 Step 1
                If effect_list(i).Contains("HPdrainO") Then
                    Dim drain_amount As Double = Convert.ToDouble(damage) * (Convert.ToDouble(effect_list(i + 1)))
                    first_pokemon.HP += drain_amount
                    If Not Logger.isMute() Then
                        Logger.Record("I")
                        Logger.Record(second_pokemon.Name + " has its health sapped by " + draining_move.Name + "!")
                        Logger.Record("I")
                        Logger.Record(first_pokemon.Name + " gains " + Convert.ToString(drain_amount) + " in HP!")
                    End If
                End If
            Next
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
                If Not Logger.isMute() Then
                    Logger.Record("I")
                    Logger.Record(confused_pokemon.Name + " is confused!")
                End If
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
                    If Logger.isMute() = False Then
                        Logger.Record(confused_pokemon.Name + " hit itself in confusion!!")
                    End If
                End If
            End If
            Return confuse_success
        End Function

        ''' <summary>
        ''' The function analyzes if twoturnmove is a move that requires two turns to execute and prepares the move to be used for next turn.
        ''' This function DOES NOT apply any damage to second_ or first_pokemon.
        ''' Returns true if the move is a 2 turn move. Otherwise, false.
        ''' </summary>
        ''' <param name="first_pokemon">The first pokemon.</param>
        ''' <param name="second_pokemon">The pokemon that will be hit by the move.</param>
        ''' <param name="twoturnmove">The move in question.</param>
        ''' <param name="arena"></param>
        ''' <returns>Returns true if the move is a 2 turn move. False if not.</returns>
        ''' <remarks></remarks>
        Public Function apply_twoturnmove(ByVal first_pokemon As Pokemon, ByVal second_pokemon As Pokemon, ByVal twoturnmove As Move_Info,
                                          ByVal arena As Pokemon_Arena) As Boolean
            Dim effectlist As String() = twoturnmove.Effect.Split(",")
            If effectlist.Length = 0 OrElse effectlist(0) = "" Then
                Return False
            End If

            Dim app_success As Boolean = False
            For i As Integer = 0 To effectlist.Length() - 1 Step 1
                If effectlist(i) = "2turn" Then
                    app_success = True
                    Dim pokemon_location As String = effectlist(i + 1)
                    If pokemon_location = "air" Then
                        If Not Constants.Get_FormattedString(twoturnmove.Name) = "bounce" Then
                            If Not Logger.isMute() Then
                                Logger.Record("I")
                                Logger.Record(first_pokemon.Name + " flies up high!")
                            End If
                        Else
                            If Not Logger.isMute() Then
                                Logger.Record("I")
                                Logger.Record(first_pokemon.Name + " bounces high up in the air!")
                            End If
                        End If
                        If first_pokemon.Team = "blue" Then
                            arena.Blue_Location = "air"
                        Else
                            arena.Red_Location = "air"
                        End If
                    ElseIf pokemon_location = "ground" Then
                        If Not Logger.isMute() Then
                            Logger.Record("I")
                            Logger.Record(first_pokemon.Name + " goes underground!")
                        End If
                        If first_pokemon.Team = "blue" Then
                            arena.Blue_Location = "ground"
                        Else
                            arena.Red_Location = "ground"
                        End If
                    ElseIf pokemon_location = "water" Then
                        If Not Logger.isMute() Then
                            Logger.Record("I")
                            Logger.Record(first_pokemon.Name + " goes underwater!")
                        End If
                        If first_pokemon.Team = "blue" Then
                            arena.Blue_Location = "water"
                        Else
                            arena.Red_Location = "water"
                        End If
                    ElseIf pokemon_location = "van" Then
                        If Not Logger.isMute() Then
                            Logger.Record("I")
                            Logger.Record(first_pokemon.Name + " vanishes!")
                        End If
                        If first_pokemon.Team = "blue" Then
                            arena.Blue_Location = "van"
                        Else
                            arena.Red_Location = "van"
                        End If
                    Else
                        If Not Logger.isMute() Then
                            Logger.Record("I")
                            Logger.Record(first_pokemon.Name + " loads up!")
                        End If
                    End If

                    If first_pokemon.Team = "blue" Then
                        arena.Enqueue_BlueMove(twoturnmove)
                    Else
                        arena.Enqueue_RedMove(twoturnmove)
                    End If
                    app_success = True
                    Return app_success
                End If
            Next

            Return app_success
        End Function

        ''' <summary>
        ''' Applies the effect for all status moves. The move can be damaging or non-damaging.
        ''' </summary>
        ''' <param name="my_pokemon">The Pokemon using the move</param>
        ''' <param name="opponent_pokemon">The target pokemon (if the status applies to the opponent)</param>
        ''' <param name="move">The move used. Can be damaging or non-damaging.</param>
        ''' <remarks></remarks>
        Public Sub apply_moveeffect(ByVal my_pokemon As Pokemon, ByVal opponent_pokemon As Pokemon, ByVal move As Move_Info,
                                    Optional ByVal funct_id As Integer = -1000,
                                    Optional ByVal it_funct_id As Integer = -1000)
            Dim effectlist As String() = move.Effect.Split(",")
            If effectlist.Length = 0 Then
                Return
            End If

            If funct_id = Constants.Funct_IDs.EvaluateGreenCase OrElse funct_id = Constants.Funct_IDs.ApplyBattle_SuperEffectiveBranch _
                OrElse funct_id = Constants.Funct_IDs.ApplyBattle_NormalMoveBranch Then
                If it_funct_id = -1000 Then
                    If Not Logger.isMute() Then
                        Logger.Record(my_pokemon.Name + " uses " + move.Name + " on " + opponent_pokemon.Name + "!")
                    End If

                    REM if this function is calling it, we would need to check for the accuracy hit
                    If move.Accuracy > 0 AndAlso move.Accuracy < 100 Then
                        Dim chancehit As Integer = Poke_Calculator.GenerateRandomNumber()
                        If Not chancehit <= move.Accuracy Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + " misses!")
                            Return
                        End If
                    Else
                        move.PP -= 1
                    End If
                End If
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
                        REM else we do nothing since every pokemon can only have one status condition at a time
                        If opponent_pokemon.Status_Condition = Constants.StatusCondition.none Then
                            opponent_pokemon.Status_Condition = Constants.StatusCondition.sleep
                            If Logger.isMute() = False Then
                                Logger.Record("I")
                                Logger.Record(opponent_pokemon.Name + " falls asleep!")
                            End If
                        End If

                    ElseIf effectlist(i).Contains("U") Then
                        REM if we arrive here, then the move is probably "rest." Check for it
                        If Constants.Get_FormattedString(move.Name) = "rest" Then
                            my_pokemon.Status_Condition = Constants.StatusCondition.sleep
                            my_pokemon.Other_Status_Condition = Constants.StatusCondition.none
                        Else
                            If my_pokemon.Status_Condition = Constants.StatusCondition.none Then
                                my_pokemon.Status_Condition = Constants.StatusCondition.sleep
                            End If
                        End If
                        If Not Logger.isMute() Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + " falls asleep!")
                        End If

                    End If
                ElseIf effectlist(i).Contains("PSNB") And chance_success = True Then
                    If effectlist(i).Contains("O") Then
                        If opponent_pokemon.Status_Condition = Constants.StatusCondition.none Then
                            opponent_pokemon.Status_Condition = Constants.StatusCondition.badly_poisoned
                            If Logger.isMute() = False Then
                                Logger.Record("I")
                                Logger.Record(opponent_pokemon.Name + " becomes badly poisoned!")
                            End If
                        End If
                    End If
                ElseIf effectlist(i).Contains("PSN") And chance_success = True Then
                    If effectlist(i).Contains("O") Then
                        If opponent_pokemon.Status_Condition = Constants.StatusCondition.none Then
                            opponent_pokemon.Status_Condition = Constants.StatusCondition.poison
                            If Logger.isMute() = False Then
                                Logger.Record("I")
                                Logger.Record(opponent_pokemon.Name + " becomes poisoned!")
                            End If
                        End If
                    End If
                ElseIf effectlist(i).Contains("BRN") And chance_success = True Then
                    If effectlist(i).Contains("O") Then
                        If opponent_pokemon.Status_Condition = Constants.StatusCondition.none Then
                            opponent_pokemon.Status_Condition = Constants.StatusCondition.burn
                            If Logger.isMute() = False Then
                                Logger.Record("I")
                                Logger.Record(opponent_pokemon.Name + " becomes burned!")
                            End If
                        End If
                    End If
                ElseIf effectlist(i).Contains("FRZ") And chance_success = True Then
                    If effectlist(i).Contains("O") Then
                        If opponent_pokemon.Status_Condition = Constants.StatusCondition.none Then
                            opponent_pokemon.Status_Condition = Constants.StatusCondition.freeze
                            If Logger.isMute() = False Then
                                Logger.Record("I")
                                Logger.Record(opponent_pokemon.Name + " becomes frozen!")
                            End If
                        End If
                    End If
                ElseIf effectlist(i).Contains("PRLYZ") And chance_success = True Then
                    If effectlist(i).Contains("O") Then
                        If opponent_pokemon.Status_Condition = Constants.StatusCondition.none Then
                            opponent_pokemon.Status_Condition = Constants.StatusCondition.paralyzed
                            If Logger.isMute() = False Then
                                Logger.Record("I")
                                Logger.Record(opponent_pokemon.Name + " becomes paralyzed!")
                            End If
                        End If
                    End If
                ElseIf effectlist(i).Contains("CONF") And chance_success = True Then
                    If effectlist(i).Contains("CONFO") Or effectlist(i).Contains("CONFchanceO") Then
                        opponent_pokemon.Other_Status_Condition = Constants.StatusCondition.confused
                        If Logger.isMute() = False Then
                            Logger.Record("I")
                            Logger.Record(opponent_pokemon.Name + " becomes confused!")
                        End If
                    End If
                ElseIf effectlist(i).Contains("HPfull") And chance_success = True Then
                    If effectlist(i).Contains("HPfullU") Then
                        REM restore health completely!
                        Dim t_pokemon As Pokemon = Form1.Get_PokemonDictionary().Get_Dictionary().Item(my_pokemon.Name)
                        my_pokemon.HP = t_pokemon.HP
                        If Not Logger.isMute() Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + " restores its health!")
                        End If
                    End If
                ElseIf effectlist(i).Contains("HPhalf") And chance_success = True Then
                    If effectlist(i).Contains("HPhalfU") Then
                        REM restore half of its max hp!
                        Dim t_pokemon As Pokemon = Form1.Get_PokemonDictionary().Get_Dictionary().Item(my_pokemon.Name)
                        my_pokemon.HP = my_pokemon.HP + (t_pokemon.HP / 2)
                        If Not Logger.isMute() Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + " restores some health!")
                        End If
                    End If
                ElseIf effectlist(i).Contains("FLNCH") And chance_success = True Then
                    If effectlist(i).Contains("O") Then
                        If opponent_pokemon.Team = "blue" Then
                            Arena_Constants.Blue_Flinch = True
                        Else
                            Arena_Constants.Red_Flinch = True
                        End If
                    End If
                End If
            Next
        End Sub

        ''' <summary>
        ''' Apples statchanging_move to my_pokemon. This could be raising damage_move by 1 stage, 2 stages, or lower 1 stage, etc...
        ''' This function cannot distinguish between an opponent or user move.
        ''' The function returns a boolean indicating if the application was successful or not.
        ''' </summary>
        ''' <param name="my_pokemon">The move that applies damage</param>
        ''' <param name="statchanging_move">The non-damaging move</param>
        ''' <returns>A Boolean saying if the application was successful.</returns>
        ''' <remarks>This function does not know who the damage_move belongs to.</remarks>
        Public Function apply_stattopokemon(ByVal my_pokemon As Pokemon, ByVal statchanging_move As Move_Info, ByVal calling_pokename As String) As Boolean
            Dim success As Boolean = True
            Dim effects As String() = statchanging_move.Effect.Split(",")
            If Not Logger.isMute() Then
                Logger.Record(calling_pokename + " uses " + statchanging_move.Name + "!")
            End If
            Dim i As Integer = 0
            While i < effects.Length
                If effects(i).Contains("SPATK") Then
                    If effects(i).Contains("SPATKU+1") Or effects(i).Contains("SPATKO+1") Then
                        my_pokemon.SP_ATK_Boost += 1
                        If Not Logger.isMute() And my_pokemon.SP_ATK_Boost <= 6 Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Special Attack rose!")
                        End If
                    ElseIf effects(i).Contains("SPATKU+2") Or effects(i).Contains("SPATKO+2") Then
                        my_pokemon.SP_ATK_Boost += 2
                        If Not Logger.isMute() And my_pokemon.SP_ATK_Boost <= 6 Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Special Attack sharply rose!")
                        End If
                    ElseIf effects(i).Contains("SPATKU-1") Or effects(i).Contains("SPATKO-1") Then
                        my_pokemon.SP_ATK_Boost -= 1
                        If Not Logger.isMute() And my_pokemon.SP_ATK_Boost >= -6 Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Special Attack fell!")
                        End If
                    ElseIf effects(i).Contains("SPATKU-2") Or effects(i).Contains("SPATKO-2") Then
                        my_pokemon.SP_ATK_Boost -= 2
                        If Not Logger.isMute() And my_pokemon.SP_ATK_Boost >= -6 Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Special Attack fell!")
                        End If
                    Else
                        Return False REM don't do anything

                    End If
                    If my_pokemon.SP_ATK_Boost <= 6 And my_pokemon.SP_ATK_Boost >= -6 Then
                        Dim boostvalue As Double = Constants.Get_StageBoostValue(my_pokemon.SP_ATK_Boost)
                        my_pokemon.Sp_ATK = my_pokemon.Sp_ATK * boostvalue
                    ElseIf my_pokemon.SP_ATK_Boost > 6 Then
                        If Not Logger.isMute() Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Special Attack won't go any higher!")
                        End If
                        my_pokemon.SP_ATK_Boost = 6
                        success = False
                    ElseIf my_pokemon.SP_ATK_Boost < -6 Then
                        If Not Logger.isMute() Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Special Attack won't go any lower!")
                        End If
                        my_pokemon.SP_ATK_Boost = -6
                        success = False
                    End If
                ElseIf effects(i).Contains("SPDEF") Then
                    If effects(i).Contains("SPDEFU+1") Or effects(i).Contains("SPDEFO+1") Then
                        my_pokemon.SP_DEF_Boost += 1
                        If Not Logger.isMute() And my_pokemon.SP_DEF_Boost <= 6 Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Special Defense rose!")
                        End If
                    ElseIf effects(i).Contains("SPDEFU+2") Or effects(i).Contains("SPDEFO+2") Then
                        my_pokemon.SP_DEF_Boost += 2
                        If Not Logger.isMute() And my_pokemon.SP_DEF_Boost <= 6 Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Special Defense sharply rose!")
                        End If
                    ElseIf effects(i).Contains("SPDEFU-1") Or effects(i).Contains("SPDEFO-1") Then
                        my_pokemon.SP_DEF_Boost -= 1
                        If Not Logger.isMute() And my_pokemon.SP_DEF_Boost >= -6 Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Special Defense fell!")
                        End If
                    ElseIf effects(i).Contains("SPDEFU-2") Or effects(i).Contains("SPDEFO-2") Then
                        my_pokemon.SP_DEF_Boost -= 2
                        If Not Logger.isMute() And my_pokemon.SP_DEF_Boost >= -6 Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Special Defense harshly fell!")
                        End If
                    Else
                        Return False REM don't do anything

                    End If
                    If my_pokemon.SP_DEF_Boost <= 6 And my_pokemon.SP_DEF_Boost >= -6 Then
                        Dim boostvalue As Double = Constants.Get_StageBoostValue(my_pokemon.SP_DEF_Boost)
                        my_pokemon.Sp_DEF = my_pokemon.Sp_DEF * boostvalue
                    ElseIf my_pokemon.SP_DEF_Boost > 6 Then
                        If Not Logger.isMute() Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Special Defense won't go any higher!")
                        End If
                        my_pokemon.SP_DEF_Boost = 6
                        success = False
                    ElseIf my_pokemon.SP_DEF_Boost < -6 Then
                        If Not Logger.isMute() Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Special Defense won't go any lower!")
                        End If
                        my_pokemon.SP_DEF_Boost = -6
                        success = False
                    End If

                ElseIf effects(i).Contains("ATK") Then
                    If effects(i).Contains("ATKU+1") Or effects(i).Contains("ATKO+1") Then
                        my_pokemon.ATK_Boost += 1
                        If Not Logger.isMute() And my_pokemon.ATK_Boost <= 6 Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Attack rose!")
                        End If
                    ElseIf effects(i).Contains("ATKU+2") Or effects(i).Contains("ATKO+2") Then
                        my_pokemon.ATK_Boost += 2
                        If Not Logger.isMute() And my_pokemon.ATK_Boost <= 6 Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Attack rose!")
                        End If
                    ElseIf effects(i).Contains("ATKU-1") Or effects(i).Contains("ATKO-1") Then
                        my_pokemon.ATK_Boost -= 1
                        If Not Logger.isMute() And my_pokemon.ATK_Boost >= -6 Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Attack fell!")
                        End If
                    ElseIf effects(i).Contains("ATKU-2") Or effects(i).Contains("ATKO-2") Then
                        my_pokemon.ATK_Boost -= 2
                        If Not Logger.isMute() And my_pokemon.Name >= -6 Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Attack harshly fell!")
                        End If
                    Else
                        Return False REM don't do anything

                    End If
                    If my_pokemon.ATK_Boost <= 6 And my_pokemon.ATK_Boost >= -6 Then
                        Dim boostvalue As Double = Constants.Get_StageBoostValue(my_pokemon.ATK_Boost)
                        my_pokemon.ATK = my_pokemon.ATK * boostvalue
                    ElseIf my_pokemon.ATK_Boost > 6 Then
                        If Not Logger.isMute() Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Attack won't go any higher!")
                        End If
                        my_pokemon.ATK_Boost = 6
                        success = False
                    ElseIf my_pokemon.ATK_Boost <= -6 Then
                        If Not Logger.isMute() Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Attack won't go any lower!")
                        End If
                        my_pokemon.ATK_Boost = -6
                        success = False
                    End If
                ElseIf effects(i).Contains("DEF") Then
                    If effects(i).Contains("DEFU+1") Or effects(i).Contains("DEFO+1") Then
                        my_pokemon.DEF_Boost += 1
                        If Not Logger.isMute() And my_pokemon.DEF_Boost <= 6 Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Defense rose!")
                        End If
                    ElseIf effects(i).Contains("DEFU+2") Or effects(i).Contains("DEFO+2") Then
                        my_pokemon.DEF_Boost += 2
                        If Not Logger.isMute() And my_pokemon.DEF_Boost <= 6 Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Defense sharply rose!")
                        End If
                    ElseIf effects(i).Contains("DEFU-1") Or effects(i).Contains("DEFO-1") Then
                        my_pokemon.DEF_Boost -= 1
                        If Not Logger.isMute() And my_pokemon.DEF_Boost >= -6 Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Defense fell!")
                        End If
                    ElseIf effects(i).Contains("DEFU-2") Or effects(i).Contains("DEFO-2") Then
                        my_pokemon.DEF_Boost -= 2
                        If Not Logger.isMute() And my_pokemon.DEF_Boost >= -6 Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Defense harshly fell!")
                        End If
                    Else
                        Return False REM don't do anything
                    End If
                    If my_pokemon.DEF_Boost <= 6 And my_pokemon.DEF_Boost >= -6 Then
                        Dim boostvalue As Double = Constants.Get_StageBoostValue(my_pokemon.DEF_Boost)
                        my_pokemon.DEF = my_pokemon.DEF * boostvalue
                    ElseIf my_pokemon.DEF_Boost > 6 Then
                        If Not Logger.isMute() Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Defense won't go any higher!")
                        End If
                        my_pokemon.DEF_Boost = 6
                        success = False
                    ElseIf my_pokemon.DEF_Boost < -6 Then
                        If Not Logger.isMute() Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Defense won't go any lower!")
                        End If
                        my_pokemon.SP_DEF_Boost = -6
                        success = False
                    End If


                ElseIf effects(i).Contains("SPD") Then
                    If effects(i).Contains("SPDU+1") Or effects(i).Contains("SPDO+1") Then
                        my_pokemon.SPEED_Boost += 1
                        If Not Logger.isMute() And my_pokemon.SPEED_Boost <= 6 Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Speed rose!")
                        End If
                    ElseIf effects(i).Contains("SPDU+2") Or effects(i).Contains("SPDO+2") Then
                        my_pokemon.SPEED_Boost += 2
                        If Not Logger.isMute() And my_pokemon.SPEED_Boost <= 6 Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Speed sharply rose!")
                        End If
                    ElseIf effects(i).Contains("SPDU-1") Or effects(i).Contains("SPDO-1") Then
                        my_pokemon.SPEED_Boost -= 1
                        If Not Logger.isMute() And my_pokemon.SPEED_Boost >= -6 Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Speed fell!")
                        End If
                    ElseIf effects(i).Contains("SPDU-2") Or effects(i).Contains("SPDO-2") Then
                        my_pokemon.SPEED_Boost -= 2
                        If Not Logger.isMute() And my_pokemon.SPEED_Boost >= -6 Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Speed harshly fell!")
                        End If
                    Else
                        Return False REM don't do anything

                    End If
                    If my_pokemon.SPEED_Boost <= 6 And my_pokemon.SPEED_Boost >= -6 Then
                        Dim boostvalue As Double = Constants.Get_StageBoostValue(my_pokemon.SPEED_Boost)
                        my_pokemon.SPD = my_pokemon.SPD * boostvalue
                    ElseIf my_pokemon.SPEED_Boost > 6 Then
                        If Not Logger.isMute() Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Speed won't go any higher!")
                        End If
                        my_pokemon.SPEED_Boost = 6
                        success = False
                    ElseIf my_pokemon.SPEED_Boost < -6 Then
                        If Not Logger.isMute() Then
                            Logger.Record("I")
                            Logger.Record(my_pokemon.Name + "'s Speed won't go any lower!")
                        End If
                        my_pokemon.SPEED_Boost = -6
                        success = False
                    End If

                Else
                    Return False
                End If

                i += 1
            End While

            statchanging_move.PP -= 1 REM TODO: consider the ability Pressure
            Return success
        End Function

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
                the_pokemon.HP -= (the_pokemon.HP * (1 / 8))
                If Not Logger.isMute() Then
                    Logger.Record("I")
                    Logger.Record(the_pokemon.Name + " loses health due to burn!")
                End If
            ElseIf the_pokemon.Status_Condition = Constants.StatusCondition.poison Then
                REM query the database for the original HP of the pokemon
                Dim original_hp As Integer = Form1.Get_PokemonDictionary.Get_Pokemon(Constants.Get_FormattedString(the_pokemon.Name)).HP
                Dim damage As Integer = original_hp * 1 / 8
                the_pokemon.HP = the_pokemon.HP - damage
                If Not Logger.isMute() Then
                    Logger.Record("I")
                    Logger.Record(the_pokemon.Name + " loses health due to its poisoning!")
                End If
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
                If Not Logger.isMute() Then
                    Logger.Record("I")
                    Logger.Record(the_pokemon.Name + " is losing more health due to its poisoning!!")
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
                        If Not Logger.isMute() Then
                            Logger.Record("I")
                            Logger.Record(the_pokemon.Name + " woke up!")
                        End If
                        arena.Red_NumSleep = 0
                        Return
                    End If
                ElseIf the_pokemon.Team = "blue" Then
                    If arena.Blue_NumSleep > 3 Then
                        the_pokemon.Status_Condition = Constants.StatusCondition.none
                        If Not Logger.isMute() Then
                            Logger.Record("I")
                            Logger.Record(the_pokemon.Name + " woke up!")
                        End If
                        arena.Blue_NumSleep = 0
                        Return
                    End If
                End If

                Dim random As Integer = Poke_Calculator.GenerateRandomNumber()
                If random <= 33 AndAlso random >= 0 Then
                    REM pokemon has a 33% chance of waking up each turn
                    the_pokemon.Status_Condition = Constants.StatusCondition.none
                    If Not Logger.isMute() Then
                        Logger.Record("I")
                        Logger.Record(the_pokemon.Name + " woke up!")
                    End If
                End If
            ElseIf the_pokemon.Status_Condition = Constants.StatusCondition.freeze Then
                Dim random As Integer = Poke_Calculator.GenerateRandomNumber()
                If random <= 20 AndAlso random >= 0 Then
                    REM the pokemon has a 20% chance of thawing
                    the_pokemon.Status_Condition = Constants.StatusCondition.none
                    If Not Logger.isMute() Then
                        Logger.Record("I")
                        Logger.Record(the_pokemon.Name + " defrosted! It's no longer frozen!")
                    End If
                End If
            End If
            If the_pokemon.Other_Status_Condition = Constants.StatusCondition.confused Then
                If the_pokemon.Team = "red" Then
                    If arena.Red_NumConfused > 4 Then
                        the_pokemon.Other_Status_Condition = Constants.StatusCondition.none
                        If Not Logger.isMute() Then
                            Logger.Record("I")
                            Logger.Record(the_pokemon.Name + " snapped out of confusion!")
                        End If
                        arena.Red_NumConfused = 0
                        Return
                    End If
                ElseIf the_pokemon.Team = "blue" Then
                    If arena.Blue_NumConfused > 4 Then
                        the_pokemon.Other_Status_Condition = Constants.StatusCondition.none
                        If Not Logger.isMute() Then
                            Logger.Record("I")
                            Logger.Record(the_pokemon.Name + " snapped out of confusion!")
                        End If
                        arena.Blue_NumConfused = 0
                        Return
                    End If
                End If

                Dim random As Integer = Poke_Calculator.GenerateRandomNumber()
                REM apply a 50% snapping out of confusion, could not find more information about this...
                If random <= 50 Then
                    the_pokemon.Other_Status_Condition = Constants.StatusCondition.none
                    If Not Logger.isMute() Then
                        Logger.Record("I")
                        Logger.Record(the_pokemon.Name + " snapped out of confusion!")
                    End If
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
                If Logger.isMute() = False Then
                    Logger.Record("I")
                    Logger.Record(pokemon.Name + " is paralyzed! It can't move!")
                End If
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
End Namespace