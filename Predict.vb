Imports System
Imports System.Collections

Public Interface Predict

    Function predict_outcome(ByVal battle_arena As Arena) As String
End Interface

Public Class Battle_Prediction : Implements Predict

    Dim effectiveness_table As New EffectivenessTable

    Private m_currentturn_team As String

    Public Property CurrentTurn_Team As String
        Get
            Return m_currentturn_team
        End Get
        Set(value As String)
            m_currentturn_team = value
        End Set
    End Property

    Public Function predict_outcome(ByVal battle_arena As Arena) As String Implements Predict.predict_outcome
        REM first, check to make sure we are receiving a Pokemon_Arena
        Dim my_pokemonarena As Pokemon_Arena = TryCast(battle_arena, Pokemon_Arena)
        If my_pokemonarena Is Nothing Then
            MessageBox.Show("Something went wrong when trying to cast the arena to a Pokemon_Arena.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ""
        End If

        Dim winningparty As String = ""

        winningparty = Me.predict_battle(my_pokemonarena)

        Return winningparty
    End Function

    ''' <summary>
    ''' Handles the turn-base mechanics of Pokemon
    ''' </summary>
    ''' <param name="battle_arena">An arena filled with two teams of Pokemon</param>
    ''' <returns>The winning party</returns>
    ''' <remarks>Only handles the turn mechanics. This function does not know how to calculate damage or which
    ''' Pokemon is better</remarks>
    Private Function predict_battle(ByVal battle_arena As Pokemon_Arena) As String

        If battle_arena.Get_TurnNumber = 0 Then
            REM load the first pokemon of each team as the current battling
            battle_arena.AddTo_CurrentBattling_Red(battle_arena.Get_TeamRed.Get_Team("red").First)
            battle_arena.AddTo_CurrentBattling_Blue(battle_arena.Get_TeamBlue.Get_Team("blue").First)
        End If

        Dim poke_calc As New Poke_Calculator
        Dim first_pokemon As New Pokemon REM the first pokemon to move during this turn cycle
        Dim second_pokemon As New Pokemon REM the second pokemon to move during this turn cycle
        Dim turn_queue As New Queue(Of Pokemon)
        Dim redteam_enum As New List(Of Pokemon).Enumerator
        Dim blueteam_enum As New List(Of Pokemon).Enumerator

        REM these will always be pointing at the current battling pokemon
        redteam_enum = battle_arena.Get_TeamRed().Get_Team("red").GetEnumerator()
        blueteam_enum = battle_arena.Get_TeamBlue.Get_Team("blue").GetEnumerator()

        redteam_enum.MoveNext()
        blueteam_enum.MoveNext()

        'loop until one team is dead
        While Not battle_arena.IsBlueFainted And Not battle_arena.IsRedFainted
            REM begin actual battle logic

            If turn_queue.Count = 0 Then

                REM check to make sure no Pokemon has fainted
                If Not battle_arena.Last_Fainted = "" Then
                    REM someone has fainted!!
                    If battle_arena.Last_Fainted = "red" Then
                        REM take the next pokemon on the red team
                        redteam_enum.MoveNext()
                        If redteam_enum.Current Is Nothing Then
                            REM red has ran out of pokemon! Blue is winner!
                            Return "blue"
                        Else
                            battle_arena.AddTo_CurrentBattling_Red(redteam_enum.Current)
                        End If
                    ElseIf battle_arena.Last_Fainted = "blue" Then
                        blueteam_enum.MoveNext()
                        If blueteam_enum.Current Is Nothing Then
                            Return "red"
                        Else
                            battle_arena.AddTo_CurrentBattling_Blue(blueteam_enum.Current)
                        End If
                    Else
                        MessageBox.Show("predict_battle has ran into an error. We cannot process this battle. Sorry!", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return ""

                    End If
                    battle_arena.Last_Fainted = ""
                End If

                REM populate the queue since we are on a fresh cycle
                REM check speed, the higher speed stat pokemon moves first
                If battle_arena.CurrentBattlingBlue.First.SPD > battle_arena.CurrentBattlingRed.First.SPD Then
                    turn_queue.Enqueue(battle_arena.CurrentBattlingBlue.First)
                    turn_queue.Enqueue(battle_arena.CurrentBattlingRed.First)
                    battle_arena.Current_Attacker = "blue"

                ElseIf battle_arena.CurrentBattlingBlue.First.SPD < battle_arena.CurrentBattlingRed.First.SPD Then
                    REM red goes first
                    turn_queue.Enqueue(battle_arena.CurrentBattlingRed.First)
                    turn_queue.Enqueue(battle_arena.CurrentBattlingBlue.First)
                    battle_arena.Current_Attacker = "red"

                Else
                    REM same speed, in this case, it will be random
                End If
            End If

            If turn_queue.Count = 2 Then

                first_pokemon = turn_queue.Dequeue()
                If poke_calc.apply_turnparalysis(first_pokemon) = False Then
                    poke_calc.apply_statustopokemon_before(first_pokemon, battle_arena)

                    If Not first_pokemon.Status_Condition = Constants.StatusCondition.freeze And Not first_pokemon.Status_Condition = Constants.StatusCondition.sleep Then
                        Me.apply_battle(first_pokemon, turn_queue.Peek(), poke_calc, battle_arena) 'THE IMPORTANT FUNCTION!!!
                    End If

                End If


                If battle_arena.Current_Attacker = "blue" Then
                    REM check if red has fainted
                    If battle_arena.CurrentBattlingRed.First.HP <= 0 Then
                        battle_arena.CurrentBattlingRed.Clear() REM remove red
                        battle_arena.Last_Fainted = "red"
                        turn_queue.Clear() REM empty and start fresh again since a new pokemon will be coming in
                    End If
                    battle_arena.Current_Attacker = "red"
                Else
                    If battle_arena.CurrentBattlingBlue.First.HP <= 0 Then
                        battle_arena.CurrentBattlingBlue.Clear()
                        battle_arena.Last_Fainted = "blue"
                        turn_queue.Clear()
                    End If
                    battle_arena.Current_Attacker = "blue"
                End If

            ElseIf turn_queue.Count = 1 Then

                If poke_calc.apply_turnparalysis(turn_queue.Peek()) = False Then
                    poke_calc.apply_statustopokemon_before(turn_queue.Peek(), battle_arena)

                    If Not turn_queue.Peek().Status_Condition = Constants.StatusCondition.freeze And Not turn_queue.Peek().Status_Condition = Constants.StatusCondition.sleep Then
                        Me.apply_battle(turn_queue.Peek(), first_pokemon, poke_calc, battle_arena) 'THE IMPORTANT FUNCTION!!
                    End If

                End If

                second_pokemon = turn_queue.Dequeue()

                If battle_arena.Current_Attacker = "blue" Then
                    REM check if red has fainted
                    If battle_arena.CurrentBattlingRed.First.HP <= 0 Then
                        battle_arena.CurrentBattlingRed.Clear() REM remove red
                        battle_arena.Last_Fainted = "red"
                    End If
                    battle_arena.Current_Attacker = "red"
                Else
                    If battle_arena.CurrentBattlingBlue.First.HP <= 0 Then
                        battle_arena.CurrentBattlingBlue.Clear()
                        battle_arena.Last_Fainted = "blue"
                    End If
                    battle_arena.Current_Attacker = "blue"
                End If

                battle_arena.ManageTurns()
                REM now statuses are applied after all pokemon have gone
                poke_calc.apply_statustopokemon_after(first_pokemon, battle_arena)
                poke_calc.apply_statustopokemon_after(second_pokemon, battle_arena)

            Else
                MessageBox.Show("Something went wrong in predict_battle(), specifically with the queue. Returning...", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If




        End While


        If battle_arena.IsBlueFainted = True Then
            Return "red"
        ElseIf battle_arena.IsRedFainted = True Then
            Return "blue"
        Else
            Return ""
        End If

    End Function

    ''' <summary>
    ''' Finds all supereffective moves that attacking_pokemon can use
    ''' </summary>
    ''' <param name="attacking_pokemon"></param>
    ''' <param name="defending_pokemon"></param>
    ''' <param name="effectiveness_table"></param>
    ''' <returns>A list of SuperEffective moves</returns>
    ''' <remarks></remarks>
    Public Function IsThereSuperEffectiveMove(ByVal attacking_pokemon As Pokemon, ByVal defending_pokemon As Pokemon, ByVal effectiveness_table As EffectivenessTable) As List(Of Move_Info)
        Dim movename As String = ""
        Dim SEmoves As New List(Of Move_Info)

        Dim nummoves_attacking As Integer = attacking_pokemon.Moves_For_Battle.Count
        'Dim nummoves_defending As Integer = defending_pokemon.Moves_For_Battle.Count

        Dim my_attackenum As New List(Of Move_Info).Enumerator
        my_attackenum = attacking_pokemon.Moves_For_Battle.GetEnumerator()
        my_attackenum.MoveNext() REM initialize enumerator
        Dim listoftypes As String() = defending_pokemon.Types.ToArray()
        Dim defend_numtypes As Integer = listoftypes.Length

        For i As Integer = 0 To nummoves_attacking - 1 Step 1

            For j As Integer = 1 To defend_numtypes Step 1
                Dim effect_value As ULong
                effect_value = effectiveness_table.Effective_Type(my_attackenum.Current.Type, listoftypes(j - 1))

                If effect_value = 2 Then
                    REM we found a super effective move
                    movename = my_attackenum.Current.Name REM set the name of the move
                    SEmoves.Add(my_attackenum.Current.Clone()) REM get a copy of that move

                End If
            Next

            my_attackenum.MoveNext()
        Next

        Return SEmoves
    End Function

    Public Function IsThereNormalDamageMove(ByVal attacking_pokemon As Pokemon, ByVal defending_pokemon As Pokemon, ByVal effectiveness_table As EffectivenessTable) As List(Of Move_Info)
        Dim normMoves As New List(Of Move_Info)

        Dim nummoves_attacking As Integer = attacking_pokemon.Moves_For_Battle.Count
        Dim my_attackenum As New List(Of Move_Info).Enumerator
        my_attackenum = attacking_pokemon.Moves_For_Battle.GetEnumerator()
        my_attackenum.MoveNext() REM initialize enumerator
        Dim listoftypes As String() = defending_pokemon.Types.ToArray()
        Dim defend_numtypes As Integer = listoftypes.Length

        For i As Integer = 0 To nummoves_attacking - 1 Step 1

            For j As Integer = 1 To defend_numtypes Step 1
                Dim effect_value As ULong
                effect_value = effectiveness_table.Effective_Type(my_attackenum.Current.Type, listoftypes(j - 1))

                REM we don't want any status moves hopping on board
                If effect_value = 1 And my_attackenum.Current.Power > 0 Then
                    REM we found a normal effective move
                    'movename = my_attackenum.Current.Name REM set the name of the move
                    normMoves.Add(my_attackenum.Current.Clone()) REM get a copy of that move

                End If
            Next

            my_attackenum.MoveNext()
        Next
        Return normMoves
    End Function

    Public Function IsThereNotEffectiveMoves(ByVal attacking_pokemon As Pokemon, ByVal defending_pokemon As Pokemon, ByVal effectiveness_table As EffectivenessTable) As List(Of Move_Info)
        Dim noteffective_Moves As New List(Of Move_Info)

        Dim nummoves_attacking As Integer = attacking_pokemon.Moves_For_Battle.Count
        Dim my_attackenum As New List(Of Move_Info).Enumerator
        my_attackenum = attacking_pokemon.Moves_For_Battle.GetEnumerator()
        my_attackenum.MoveNext() REM initialize enumerator
        Dim listoftypes As String() = defending_pokemon.Types.ToArray()
        Dim defend_numtypes As Integer = listoftypes.Length

        For i As Integer = 0 To nummoves_attacking - 1 Step 1

            For j As Integer = 1 To defend_numtypes Step 1
                Dim effect_value As ULong
                effect_value = effectiveness_table.Effective_Type(my_attackenum.Current.Type, listoftypes(j - 1))

                If effect_value = 1 Then
                    REM we found a normal effective move
                    'movename = my_attackenum.Current.Name REM set the name of the move
                    noteffective_Moves.Add(my_attackenum.Current.Clone()) REM get a copy of that move

                End If
            Next

            my_attackenum.MoveNext()
        Next
        Return noteffective_Moves
    End Function

    Public Function IsThereStatMoves(ByVal my_pokemon As Pokemon) As List(Of Move_Info)
        Dim statmoves As New List(Of Move_Info)
        Dim move_enum As New List(Of Move_Info).Enumerator
        move_enum = my_pokemon.Moves_For_Battle.GetEnumerator()
        move_enum.MoveNext()

        While Not move_enum.Current Is Nothing

            If move_enum.Current.Power = 0 Then
                If move_enum.Current.Effect.Contains("ATK") Or move_enum.Current.Effect.Contains("DEF") Or move_enum.Current.Effect.Contains("SPD") Or move_enum.Current.Effect.Contains("EVA") _
                    Or move_enum.Current.Effect.Contains("ACCU") Then
                    statmoves.Add(move_enum.Current)
                End If
            End If
            move_enum.MoveNext()
        End While

        Return statmoves
    End Function

    ''' <summary>
    ''' project_battle figures out the number of turns it takes for first_pokemon to destroy the second_pokemon
    ''' using attacking_move. Passing a non-damaging move does not work. 
    ''' </summary>
    ''' <param name="first_pokemon">It is recommended to pass a clone.</param>
    ''' <param name="second_pokemon">It is recommended to pass a clone.</param>
    ''' <param name="attacking_move"></param>
    ''' <param name="poke_calculator"></param>
    ''' <param name="max_or_min">An integer to indicate if the user wants to project the battle using max(1), min(-1), or regular(0) damage. </param>
    ''' <returns>The number of turns until second_pokemon faints</returns>
    ''' <remarks>Non-damaging moves are not accepted. It is strongly encouraged to pass a clone into this function.</remarks>
    Public Function Project_Battle(ByVal first_pokemon As Pokemon, ByVal second_pokemon As Pokemon, ByVal attacking_move As Move_Info,
                                   ByVal poke_calculator As Poke_Calculator, ByVal max_or_min As Integer) As Integer

        REM first check if attacking_move is a damaging move. If not, this function cannot accept it.
        If attacking_move.Power = 0 Then
            Return -1
        End If

        Dim turns_to_faint As Integer = 0
        Dim eff_table As New EffectivenessTable
        Dim damagevalue As Integer = -1
        Dim EFF As ULong

        EFF = eff_table.Effective_Type(attacking_move.Type, second_pokemon.Types)

        While Not second_pokemon.HP <= 0
            damagevalue = poke_calculator.CalculateDamage(first_pokemon, second_pokemon, attacking_move, EFF, max_or_min)

            REM apply the damage to the defending pokemon
            REM in the future we will apply the special damages such as burn, poison type
            second_pokemon.HP = second_pokemon.HP - damagevalue

            turns_to_faint += 1
        End While

        Return turns_to_faint
    End Function

    ''' <summary>
    ''' Applys the battle logic for pokemon battles. This function does not simulate. It actually applies the damage
    ''' </summary>
    ''' <param name="first_pokemon">The attacking Pokemon</param>
    ''' <param name="second_pokemon">The defending Pokemon</param>
    ''' <param name="poke_calc"></param>
    ''' <param name="poke_arena">The arena with Pokemon</param>
    ''' <remarks>The pokemon will be damaged! Do not use this function for simulation purposes unless if you 
    ''' pass in clones of first_pokemon and second_pokemon</remarks>
    Private Sub apply_battle(ByRef first_pokemon As Pokemon, ByRef second_pokemon As Pokemon, ByVal poke_calc As Poke_Calculator, ByVal poke_arena As Pokemon_Arena)
        Dim turn_poke_move As New Move_Info
        Dim turn_poke_move2 As New Move_Info
        Dim turn_poke_move_pack As New Prediction_Move_Package
        Dim turn_poke_move2_pack As New Prediction_Move_Package

        REM check for any status conditions that prohibit movement
        If first_pokemon.Status_Condition = Constants.StatusCondition.freeze Then
            Return
        ElseIf first_pokemon.Status_Condition = Constants.StatusCondition.sleep Then
            Return
        ElseIf first_pokemon.Status_Condition = Constants.StatusCondition.paralyzed Then
            Dim random As Double = Poke_Calculator.GenerateRandomNumber(0.0, 100.0)
            REM paralysis has a 25% chance of hitting so if it hits, then first_pokemon cannot move
            If random >= 75 Or random <= 100 Then
                Return
            End If
        End If

        Dim isthere_SEmove As List(Of Move_Info)
        isthere_SEmove = Me.IsThereSuperEffectiveMove(first_pokemon, second_pokemon, effectiveness_table)

        If Not isthere_SEmove.Count = 0 Then
            REM there exists a supereffective move we can use!

            turn_poke_move_pack = Me.FindBestMove(first_pokemon, second_pokemon, poke_calc, isthere_SEmove, 1)
            REM check out an available offensive stat move
            turn_poke_move2_pack = Me.FindBestStatMove(first_pokemon, second_pokemon, poke_calc, turn_poke_move_pack.Move, 1, 1)

            If turn_poke_move2_pack Is Nothing Then
                poke_calc.apply_damage(first_pokemon, second_pokemon, turn_poke_move_pack.Move, poke_calc, 1)
            Else
                If turn_poke_move2_pack.Opponent_Turns < turn_poke_move2_pack.My_Turns Then
                    REM then the opponenet kills us faster than we can kill it... No good. Just apply damage man...
                    'TODO: in a future edition, analyze these numbers carefully, as in look at the difference
                    'between these 2 values and if it is too great, then go for offensive but if it is not too great
                    'go into greater detail
                    poke_calc.apply_damage(first_pokemon, second_pokemon, turn_poke_move_pack.Move, poke_calc, 1)
                ElseIf turn_poke_move2_pack.Opponent_Turns > turn_poke_move2_pack.My_Turns Then
                    REM we can apply the stat move
                    poke_calc.apply_stattopokemon(first_pokemon, turn_poke_move2_pack.Move)
                    'poke_calc.apply_damage(first_pokemon, second_pokemon, turn_poke_move_pack.Move, poke_calc, 1)
                Else
                    REM we have a tie...just apply damage
                    REM I like to always inflict damage, so even if it's equal
                    poke_calc.apply_damage(first_pokemon, second_pokemon, turn_poke_move_pack.Move, poke_calc, 1)
                End If
            End If


        Else
            REM TODO: add check for status moves and stat lowering moves

            REM we don't have any SE moves, check for normal damaging moves
            Dim isthere_normmove As New List(Of Move_Info)
            isthere_normmove = Me.IsThereNormalDamageMove(first_pokemon, second_pokemon, effectiveness_table)

            If Not isthere_normmove.Count = 0 Then
                REM we can apply a normal damaging move!

                turn_poke_move_pack = Me.FindBestMove(first_pokemon, second_pokemon, poke_calc, isthere_normmove, 1)
                turn_poke_move2_pack = Me.FindBestStatMove(first_pokemon, second_pokemon, poke_calc, turn_poke_move_pack.Move, 1, 1)

                If turn_poke_move2_pack Is Nothing Then
                    poke_calc.apply_damage(first_pokemon, second_pokemon, turn_poke_move_pack.Move, poke_calc, 1)
                Else
                    If turn_poke_move2_pack.Opponent_Turns < turn_poke_move2_pack.My_Turns Then
                        REM then the opponenet kills us faster than we can kill it... No good. Just apply damage man...
                        'TODO: in a future edition, analyze these numbers carefully, as in look at the difference
                        'between these 2 values and if it is too great, then go for offensive but if it is not too great
                        'go into greater detail
                        poke_calc.apply_damage(first_pokemon, second_pokemon, turn_poke_move_pack.Move, poke_calc, 1)
                    ElseIf turn_poke_move2_pack.Opponent_Turns > turn_poke_move2_pack.My_Turns Then
                        REM we can apply the stat move
                        poke_calc.apply_stattopokemon(first_pokemon, turn_poke_move2_pack.Move)
                        'poke_calc.apply_damage(first_pokemon, second_pokemon, turn_poke_move_pack.Move, poke_calc, 1)
                    Else
                        REM we have a tie...just apply damage
                        REM For normal moves, it is better to go and rip apart the second_pokemon
                        poke_calc.apply_damage(first_pokemon, second_pokemon, turn_poke_move_pack.Move, poke_calc, 1)
                    End If
                End If

                'poke_calc.apply_damage(first_pokemon, second_pokemon, turn_poke_move_pack.Move, poke_calc, 1)
            Else
                REM all we have are noneffective or status moves
                Dim oppo_health As String
                If poke_arena.Current_Attacker = "blue" Then
                    oppo_health = poke_arena.Get_HealthStatusofRed()
                Else
                    oppo_health = poke_arena.Get_HealthStatusofBlue()
                End If

                If oppo_health = "green" Then
                    REM choose a status/lower stat move
                    turn_poke_move2_pack = Me.FindBestStatMove(first_pokemon, second_pokemon, poke_calc, 1, 1)
                    poke_calc.apply_stattopokemon(first_pokemon, turn_poke_move2_pack.Move)
                Else
                    REM choose the best damaging move
                    Dim isthere_noneffectivemove As New List(Of Move_Info)
                    isthere_noneffectivemove = Me.IsThereNotEffectiveMoves(first_pokemon, second_pokemon, effectiveness_table)
                    If Not isthere_noneffectivemove.Count = 0 Then

                        turn_poke_move_pack = Me.FindBestMove(first_pokemon, second_pokemon, poke_calc, isthere_noneffectivemove, 1)
                        poke_calc.apply_damage(first_pokemon, second_pokemon, turn_poke_move_pack.Move, poke_calc, 1)

                    End If
                End If

            End If

        End If
    End Sub

    ''' <summary>
    ''' Finds the best (damaging) move for the pokemon to use given a list of moves to choose from
    ''' </summary>
    ''' <param name="first_pokemon"></param>
    ''' <param name="second_pokemon"></param>
    ''' <param name="poke_calc"></param>
    ''' <param name="availmoves">A list of moves to choose from</param>
    ''' <param name="max_or_min">Max(1) choose max damage, Min(-1) choose min damage, Norm(0) choose normal damage</param>
    ''' <returns>The best move for the pokemon to use</returns>
    ''' <remarks></remarks>
    Public Function FindBestMove(ByVal first_pokemon As Pokemon, ByVal second_pokemon As Pokemon, ByVal poke_calc As Poke_Calculator,
                                 ByVal availmoves As List(Of Move_Info), ByVal max_or_min As Integer) As Prediction_Move_Package
        Dim move_pack As New Prediction_Move_Package
        Dim turn_poke_move As New Move_Info

        Dim turnstofaint As Integer = Integer.MaxValue
        Dim attacker As New Pokemon
        Dim defender As New Pokemon
        attacker = first_pokemon.Clone()
        defender = second_pokemon.Clone() REM this is guaranteed to be the defending pokemon

        Dim i As Integer = 0
        Dim new_turnstofaint As Integer = 0
        Dim move_enum As New List(Of Move_Info).Enumerator
        move_enum = availmoves.GetEnumerator()
        move_enum.MoveNext()
        While i < availmoves.Count
            REM apply each available super effective move to the defender, take the move that has the fewest turns until faint
            REM TODO: for the 1, we are going to implement a "Derp" factor based on what the user think is the stupidity of the players
            new_turnstofaint = Me.Project_Battle(first_pokemon.Clone(), second_pokemon.Clone(), move_enum.Current, poke_calc, 1)

            REM check to make sure the move is not a status move
            If new_turnstofaint < turnstofaint And Not new_turnstofaint = -1 Then
                REM we have a new move in town
                turnstofaint = new_turnstofaint
                turn_poke_move = move_enum.Current
            ElseIf new_turnstofaint = turnstofaint Then
                REM choose the higher power
                If move_enum.Current.Power > turn_poke_move.Power Then
                    turn_poke_move = move_enum.Current
                End If
            End If

            move_enum.MoveNext()
            i += 1
        End While

        move_pack.Move = turn_poke_move
        move_pack.My_Turns = turnstofaint

        Return move_pack
    End Function

    ''' <summary>
    ''' Finds the best non-damaging move when using a damaging move (movetouse). Returns a package containing the best
    ''' stat move to use and the number of turns using it would take using both the stat AND movetouse to take down second_pokemon
    ''' </summary>
    ''' <param name="first_pokemon">The attacking pokemon or the pokemon using the move</param>
    ''' <param name="second_pokemon">The defending pokemon</param>
    ''' <param name="poke_calc"></param>
    ''' <param name="movetouse">The damaging move first_pokemon will be using</param>
    ''' <param name="max_or_min">Max(1) damage, min damage(-1), norm(0)</param>
    ''' <param name="off_or_def">Choose a more offensive stat move (1) or choose a more defensive move (-1)</param>
    ''' <returns>A Prediction_Move_Package that contains the stat move to use and the number of turns it takes to kill second_pokemon.</returns>
    ''' <remarks></remarks>
    Public Function FindBestStatMove(ByVal first_pokemon As Pokemon, ByVal second_pokemon As Pokemon, ByVal poke_calc As Poke_Calculator,
                                     ByVal movetouse As Move_Info, ByVal max_or_min As Integer, ByVal off_or_def As Integer) As Prediction_Move_Package
        Dim statmove_touse As New Move_Info
        Dim statmove_touse_pack As New Prediction_Move_Package
        Dim attack_move As New Prediction_Move_Package
        Dim defense_move As New Prediction_Move_Package

        If off_or_def = 1 Then
            attack_move = Me.Get_BestRaiseAttackStatMove(first_pokemon.Clone(), second_pokemon.Clone(), poke_calc, movetouse.Clone(), max_or_min)
            defense_move = Me.Get_BestLowerDefenseStatMove(first_pokemon.Clone(), second_pokemon.Clone(), poke_calc, movetouse.Clone(), max_or_min)

            If attack_move Is Nothing Or defense_move Is Nothing Then
                If attack_move Is Nothing And Not defense_move Is Nothing Then
                    statmove_touse_pack.Move = defense_move.Move
                    statmove_touse_pack.My_Turns = defense_move.My_Turns
                    statmove_touse_pack.Opponent_Turns = defense_move.Opponent_Turns
                ElseIf defense_move Is Nothing And Not attack_move Is Nothing Then
                    statmove_touse_pack.Move = attack_move.Move
                    statmove_touse_pack.My_Turns = attack_move.My_Turns
                    statmove_touse_pack.Opponent_Turns = attack_move.Opponent_Turns
                Else
                    Return Nothing REM we absolutely got nothing out of it
                End If
               
            Else
                REM both are valid
                If attack_move.My_Turns < defense_move.My_Turns Then
                    REM the attack move is better
                    statmove_touse_pack.Move = attack_move.Move
                    statmove_touse_pack.My_Turns = attack_move.My_Turns
                    statmove_touse_pack.Opponent_Turns = attack_move.Opponent_Turns
                ElseIf attack_move.My_Turns > defense_move.My_Turns Then
                    REM defense move is better
                    statmove_touse_pack.Move = defense_move.Move
                    statmove_touse_pack.My_Turns = defense_move.My_Turns
                    statmove_touse_pack.Opponent_Turns = defense_move.Opponent_Turns
                Else
                    REM just choose one, I prefer increasing my attack
                    REM TODO: for a future release, we can project it to the next few pokemon that the other team has
                    statmove_touse_pack.Move = attack_move.Move
                    statmove_touse_pack.My_Turns = attack_move.My_Turns
                    statmove_touse_pack.Opponent_Turns = attack_move.Opponent_Turns
                End If


            End If
        ElseIf off_or_def = -1 Then
            attack_move = Me.Get_BestLowerAttackStatMove(first_pokemon.Clone(), second_pokemon.Clone(), poke_calc, movetouse.Clone(), max_or_min)
            defense_move = Me.Get_BestRaiseDefenseStatMove(first_pokemon.Clone(), second_pokemon.Clone(), poke_calc, movetouse.Clone(), max_or_min)

            If attack_move Is Nothing Or defense_move Is Nothing Then
                If attack_move Is Nothing And Not defense_move Is Nothing Then
                    statmove_touse_pack.Move = defense_move.Move
                    statmove_touse_pack.My_Turns = defense_move.My_Turns
                    statmove_touse_pack.Opponent_Turns = defense_move.Opponent_Turns
                ElseIf defense_move Is Nothing And Not attack_move Is Nothing Then
                    statmove_touse_pack.Move = attack_move.Move
                    statmove_touse_pack.My_Turns = attack_move.My_Turns
                    statmove_touse_pack.Opponent_Turns = attack_move.Opponent_Turns
                Else
                    Return Nothing REM we absolutely got nothing out of it
                End If

            Else
                REM both are valid
                REM for defensive situations, it's better to take the longer turn
                If attack_move.My_Turns > defense_move.My_Turns Then
                    REM the attack move is better
                    statmove_touse_pack.Move = attack_move.Move
                    statmove_touse_pack.My_Turns = attack_move.My_Turns
                    statmove_touse_pack.Opponent_Turns = attack_move.Opponent_Turns
                ElseIf attack_move.My_Turns < defense_move.My_Turns Then
                    REM defense move is better
                    statmove_touse_pack.Move = defense_move.Move
                    statmove_touse_pack.My_Turns = defense_move.My_Turns
                    statmove_touse_pack.Opponent_Turns = defense_move.Opponent_Turns
                Else
                    REM just choose one, I prefer increasing my defense
                    REM TODO: for a future release, we can project it to the next few pokemon that the other team has
                    statmove_touse_pack.Move = defense_move.Move
                    statmove_touse_pack.My_Turns = defense_move.My_Turns
                    statmove_touse_pack.Opponent_Turns = defense_move.Opponent_Turns
                End If


            End If
        Else
            Return Nothing

        End If



        Return statmove_touse_pack

    End Function

    ''' <summary>
    ''' An overloaded function. Returns the best stat move for first_pokemon to use given no damaging move to use
    ''' </summary>
    ''' <param name="first_pokemon"></param>
    ''' <param name="second_pokemon"></param>
    ''' <param name="poke_calc"></param>
    ''' <param name="max_or_min">Max(1) for max damage, Min(-1) for min damage, and Norm(0) to allow function to go all out (not yet implemented)</param>
    ''' <param name="off_or_def">Choose offensive-related stat move (1), choose defensive-related stat move(-1)</param>
    ''' <returns>
    ''' A Prediction_Move_Package containing the best move to use. Does not include information on how fast first_pokemon
    ''' can kill second_pokemon.
    ''' </returns>
    ''' <remarks></remarks>
    Public Function FindBestStatMove(ByVal first_pokemon As Pokemon, ByVal second_pokemon As Pokemon, ByVal poke_calc As Poke_Calculator,
                                     ByVal max_or_min As Integer, ByVal off_or_def As Integer) As Prediction_Move_Package

        Dim listofstatmoves As List(Of Move_Info) = Me.IsThereStatMoves(first_pokemon)
        Dim move_pack As New Prediction_Move_Package
        Dim move_enum As New List(Of Move_Info).Enumerator
        move_enum.MoveNext()

        If off_or_def = 1 Then
            REM look at offensive based stat moves
            If second_pokemon.num_Normal() > second_pokemon.num_Special() Then
                REM choose increasing ATK and lower DEF types
                While Not move_enum.Current Is Nothing
                    If move_enum.Current.Effect.Contains("ATKU+") Or move_enum.Current.Effect.Contains("DEFO-") Then
                        listofstatmoves.Add(move_enum.Current)
                    End If
                    move_enum.MoveNext()
                End While
            ElseIf second_pokemon.num_Normal() < second_pokemon.num_Special() Then
                While Not move_enum.Current Is Nothing
                    If move_enum.Current.Effect.Contains("SPATKU+") Or move_enum.Current.Effect.Contains("SPDEFO-") Then
                        listofstatmoves.Add(move_enum.Current)
                    End If
                    move_enum.MoveNext()
                End While
            Else
                REM the opponent got an equal number of moves (perhaps even only status moves...)
                If first_pokemon.num_Normal() > first_pokemon.num_Special() Then
                    REM choose increasing ATK and lower DEF types
                    While Not move_enum.Current Is Nothing
                        If move_enum.Current.Effect.Contains("ATKU+") Or move_enum.Current.Effect.Contains("DEFO-") Then
                            listofstatmoves.Add(move_enum.Current)
                        End If
                        move_enum.MoveNext()
                    End While
                ElseIf first_pokemon.num_Normal() < first_pokemon.num_Special() Then
                    While Not move_enum.Current Is Nothing
                        If move_enum.Current.Effect.Contains("SPATKU+") Or move_enum.Current.Effect.Contains("SPDEFO-") Then
                            listofstatmoves.Add(move_enum.Current)
                        End If
                        move_enum.MoveNext()
                    End While
                Else
                    REM we will take a look at the hard stats of the pokemon and depending on the result we will choose
                    REM the appropriate offensive move
                    If first_pokemon.ATK > first_pokemon.Sp_ATK Then
                        While Not move_enum.Current Is Nothing
                            If move_enum.Current.Effect.Contains("ATKU+") Or move_enum.Current.Effect.Contains("DEFO-") Then
                                listofstatmoves.Add(move_enum.Current)
                            End If
                            move_enum.MoveNext()
                        End While
                    ElseIf first_pokemon.Sp_ATK > first_pokemon.ATK Then
                        While Not move_enum.Current Is Nothing
                            If move_enum.Current.Effect.Contains("SPATKU+") Or move_enum.Current.Effect.Contains("SPDEFO-") Then
                                listofstatmoves.Add(move_enum.Current)
                            End If
                            move_enum.MoveNext()
                        End While
                    Else
                        REM we have an equal ATK and SP_ATK...
                        If first_pokemon.get_StrongestMove().Is_Special Then
                            REM find a stat move that can boost it!
                            While Not move_enum.Current Is Nothing
                                If move_enum.Current.Effect.Contains("SPATKU+") Or move_enum.Current.Effect.Contains("SPDEFO-") Then
                                    listofstatmoves.Add(move_enum.Current)
                                End If
                                move_enum.MoveNext()
                            End While
                        Else
                            While Not move_enum.Current Is Nothing
                                If move_enum.Current.Effect.Contains("ATKU+") Or move_enum.Current.Effect.Contains("DEFO-") Then
                                    listofstatmoves.Add(move_enum.Current)
                                End If
                                move_enum.MoveNext()
                            End While
                        End If
                    End If
                End If
            End If
        ElseIf off_or_def = -1 Then
            REM look at defensive moves
            If second_pokemon.num_Normal() > second_pokemon.num_Special() Then
                REM choose raise my defense or opposing ATK
                While Not move_enum.Current Is Nothing
                    If move_enum.Current.Effect.Contains("ATKO-") Or move_enum.Current.Effect.Contains("DEFU+") Then
                        listofstatmoves.Add(move_enum.Current)
                    End If
                    move_enum.MoveNext()
                End While
            ElseIf second_pokemon.num_Normal() < second_pokemon.num_Special() Then
                REM likewise as above
                While Not move_enum.Current Is Nothing
                    If move_enum.Current.Effect.Contains("SPATKO-") Or move_enum.Current.Effect.Contains("SPDEFU+") Then
                        listofstatmoves.Add(move_enum.Current)
                    End If
                    move_enum.MoveNext()
                End While
            Else
                REM boost our best defense stat
                If first_pokemon.DEF > first_pokemon.Sp_DEF Then
                    While Not move_enum.Current Is Nothing
                        If move_enum.Current.Effect.Contains("DEFU+") Then
                            listofstatmoves.Add(move_enum.Current)
                        End If
                        move_enum.MoveNext()
                    End While
                ElseIf first_pokemon.DEF < first_pokemon.Sp_DEF Then
                    While Not move_enum.Current Is Nothing
                        If move_enum.Current.Effect.Contains("SPDEFU+") Then
                            listofstatmoves.Add(move_enum.Current)
                        End If
                        move_enum.MoveNext()
                    End While
                Else
                    REM literally go all random... 
                    REM TODO: Implement a better system, possibly on the projecting the other team's pokemon situation
                    Dim random As New Random()
                    Dim norm_or_special As Integer = random.Next(0, 100)
                    If norm_or_special <= 5 Then
                        While Not move_enum.Current Is Nothing
                            If move_enum.Current.Effect.Contains("DEFU+") Then
                                listofstatmoves.Add(move_enum.Current)
                            End If
                            move_enum.MoveNext()
                        End While
                    Else
                        While Not move_enum.Current Is Nothing
                            If move_enum.Current.Effect.Contains("SPDEFU+") Then
                                listofstatmoves.Add(move_enum.Current)
                            End If
                            move_enum.MoveNext()
                        End While
                    End If
                End If
            End If
        Else
            REM we'll do something else later...
        End If

        move_enum.Dispose()

        If listofstatmoves.Count = 1 Then
            move_pack.Move = listofstatmoves.First
        Else
            Dim move_enum2 As New List(Of Move_Info).Enumerator
            move_enum2 = listofstatmoves.GetEnumerator()
            REM just go with PP, not the best idea. TODO: find a better method of doing this
            move_enum2.MoveNext()
            While Not move_enum2.Current Is Nothing
                If move_enum2.Current.PP > move_pack.Move.PP Then
                    move_pack.Move = move_enum2.Current
                End If
                move_enum2.MoveNext()
            End While
        End If

        Return move_pack
    End Function

    ''' <summary>
    ''' Returns a package containing the best raising Attack/SP. Attack stat move for first_pokemon to apply
    '''  and the number of turns using that move for the pokemon to knock out the second_pokemon.
    ''' </summary>
    ''' <param name="first_pokemon">It is advised to pass in a clone.</param>
    ''' <param name="second_pokemon">It is advised to pass in a clone.</param>
    ''' <param name="poke_calc"></param>
    ''' <param name="movetouse">The move to apply the stat move</param>
    ''' <param name="max_or_min">Max damage (1), min damage (-1), normal damage(0)</param>
    ''' <returns>A package containing the best stat move for the first_pokemon to apply if the pokemon uses movetouse. Also
    ''' returns the number of moves using that stat move will take to kill second_pokemon.</returns>
    ''' <remarks>Function should generally be called for offensive moves</remarks>
    Public Function Get_BestRaiseAttackStatMove(ByVal first_pokemon As Pokemon, ByVal second_pokemon As Pokemon, ByVal poke_calc As Poke_Calculator,
                                                ByVal movetouse As Move_Info, ByVal max_or_min As Integer) As Prediction_Move_Package

        Dim statmove_touse As New Move_Info
        Dim move_package As New Prediction_Move_Package
        Dim is_special As Boolean = False

        If movetouse.Is_Special = True Then
            is_special = True
        End If

        REM first look at raising our move stats, select the one that raises our attack
        Dim listof_statmoves As List(Of Move_Info) = first_pokemon.get_StatusMoves()
        If listof_statmoves.Count = 0 Then
            REM well...
            Return Nothing
        End If
        Dim final_statmoves As New List(Of Move_Info)
        Dim move_enum As New List(Of Move_Info).Enumerator
        move_enum = listof_statmoves.GetEnumerator()
        move_enum.MoveNext()
        While Not move_enum.Current Is Nothing
            'Dim effects As String() = movetouse.Effect.Split(New Char() {","c})
            REM search for an effect that has ATKU+...
            Dim effects As String() = move_enum.Current.Effect.Split(",")
            Dim i As Integer = 0
            While i < effects.Length
                If is_special = True Then
                    If effects(i).Contains("SPATKU+") Then
                        final_statmoves.Add(move_enum.Current)
                        Exit While
                    End If
                Else
                    If effects(i).Contains("ATKU+") Then
                        REM we can add this move to the final list!
                        final_statmoves.Add(move_enum.Current)
                        Exit While
                    End If
                End If
                i += 1
            End While
            move_enum.MoveNext()

        End While

        REM now we have established our final list of stat moves that can boost our attack, we can simulate
        REM the battle with the boost!
        Dim attacker As New Pokemon
        Dim defender As New Pokemon
        attacker = first_pokemon.Clone()
        defender = second_pokemon.Clone()

        REM first test to see how many turns it takes for first_pokemon to kill second_pokemon normally
        Dim turnstofaint_before As Integer = Me.Project_Battle(attacker.Clone(), defender.Clone(), movetouse, poke_calc, max_or_min)

        REM test how long it takes for the second_pokemon to kill first_pokemon
        Dim oppo_move As Prediction_Move_Package = Me.FindBestMove(second_pokemon, first_pokemon, poke_calc, second_pokemon.Moves_For_Battle, max_or_min)
        Dim me_turnstofaint As Integer = Me.Project_Battle(second_pokemon.Clone(), first_pokemon.Clone(), oppo_move.Move, poke_calc, max_or_min)


        Dim j As Integer = 0
        Dim finalmove_enum As New List(Of Move_Info).Enumerator
        finalmove_enum = final_statmoves.GetEnumerator()
        finalmove_enum.MoveNext()
        Dim newmovestofaint As Integer = Integer.MaxValue
        While Not finalmove_enum.Current Is Nothing

            poke_calc.apply_stattopokemon(attacker, finalmove_enum.Current)
            Dim turnstofaint As Integer = Me.Project_Battle(attacker.Clone(), defender.Clone(), movetouse, poke_calc, max_or_min)
            REM first check to make sure that the stat move doesn't take longer to kill the opponent than
            REM the opponent to kill me. Otherwise, don't think about using this move
            If turnstofaint + 1 < me_turnstofaint Then
                If turnstofaint + 1 < newmovestofaint Then
                    newmovestofaint = turnstofaint
                    statmove_touse = finalmove_enum.Current
                End If
            End If


            finalmove_enum.MoveNext()
            j += 1
        End While

        If newmovestofaint = Integer.MaxValue Then
            REM no suitable move was found 
            Return Nothing
        Else
            move_package.Move = statmove_touse
            move_package.My_Turns = newmovestofaint + 1 REM to compensate for the 1 stage boost turn
            move_package.Opponent_Turns = me_turnstofaint
        End If

        

        Return move_package
    End Function

    Public Function Get_BestLowerDefenseStatMove(ByVal first_pokemon As Pokemon, ByVal second_pokemon As Pokemon, ByVal poke_calc As Poke_Calculator,
                                                ByVal movetouse As Move_Info, ByVal max_or_min As Integer) As Prediction_Move_Package
        Dim move_package As New Prediction_Move_Package
        Dim statmove_touse As New Move_Info
        Dim is_special As Boolean = False

        If movetouse.Is_Special = True Then
            is_special = True
        End If

        REM first look at lowering opponent's move stats, select the one that lowers their defense
        Dim listof_statmoves As List(Of Move_Info) = first_pokemon.get_StatusMoves()
        If listof_statmoves.Count = 0 Then
            Return Nothing
        End If
        Dim final_statmoves As New List(Of Move_Info)
        Dim move_enum As New List(Of Move_Info).Enumerator
        move_enum = listof_statmoves.GetEnumerator()
        move_enum.MoveNext()
        While Not move_enum.Current Is Nothing
            'Dim effects As String() = movetouse.Effect.Split(New Char() {","c})
            REM search for an effect that has DEFO-...
            Dim effects As String() = move_enum.Current.Effect.Split(",")
            Dim i As Integer = 0
            While i < effects.Length
                If is_special = True Then
                    If effects(i).Contains("SPDEFO-") Then
                        final_statmoves.Add(move_enum.Current)
                        Exit While
                    End If
                Else
                    If effects(i).Contains("DEFO-") Then
                        REM we can add this move to the final list!
                        final_statmoves.Add(move_enum.Current)
                        Exit While
                    End If
                End If
                i += 1
            End While
            move_enum.MoveNext()
        End While

        REM now we have established our final list of stat moves that can lower their attack, we can simulate
        REM the battle with the boost!
        Dim attacker As New Pokemon
        Dim defender As New Pokemon
        attacker = first_pokemon.Clone()
        defender = second_pokemon.Clone()

        REM first test to see how many turns it takes for first_pokemon to kill second_pokemon normally
        Dim turnstofaint_before As Integer = Me.Project_Battle(attacker, defender, movetouse, poke_calc, max_or_min)

        REM test how long it takes for the second_pokemon to kill first_pokemon
        Dim oppo_move As Prediction_Move_Package = Me.FindBestMove(second_pokemon, first_pokemon, poke_calc, second_pokemon.Moves_For_Battle, max_or_min)
        Dim me_turnstofaint As Integer = Me.Project_Battle(second_pokemon.Clone(), first_pokemon.Clone(), oppo_move.Move, poke_calc, max_or_min)

        Dim finalmove_enum As New List(Of Move_Info).Enumerator
        finalmove_enum = final_statmoves.GetEnumerator()
        finalmove_enum.MoveNext()
        Dim newmovestofaint As Integer = Integer.MaxValue
        While Not finalmove_enum.Current Is Nothing

            poke_calc.apply_stattopokemon(defender, finalmove_enum.Current)
            Dim turnstofaint As Integer = Me.Project_Battle(attacker.Clone(), defender.Clone(), movetouse, poke_calc, max_or_min)
            REM first check to make sure that the stat move doesn't take longer to kill the opponent than
            REM the opponent to kill me. Otherwise, don't think about using this move
            If turnstofaint + 1 < me_turnstofaint Then
                If turnstofaint + 1 < newmovestofaint Then
                    newmovestofaint = turnstofaint
                    statmove_touse = finalmove_enum.Current
                End If
            End If


            finalmove_enum.MoveNext()
        End While

        If newmovestofaint = Integer.MaxValue Then
            Return Nothing
        Else
            move_package.Move = statmove_touse
            move_package.My_Turns = newmovestofaint
            move_package.Opponent_Turns = me_turnstofaint
        End If

        

        Return move_package
    End Function

    Public Function Get_BestLowerAttackStatMove(ByVal first_pokemon As Pokemon, ByVal second_pokemon As Pokemon, ByVal poke_calc As Poke_Calculator,
                                                ByVal movetouse As Move_Info, ByVal max_or_min As Integer) As Prediction_Move_Package
        Dim move_package As New Prediction_Move_Package
        Dim statmove_touse As New Move_Info
        Dim is_special As Boolean = False

        If movetouse.Is_Special = True Then
            is_special = True
        End If

        REM first look at lowering opponent's move stats, select the one that lowers their attack
        Dim listof_statmoves As List(Of Move_Info) = first_pokemon.get_StatusMoves()
        If listof_statmoves.Count = 0 Then
            Return Nothing
        End If
        Dim final_statmoves As New List(Of Move_Info)
        Dim move_enum As New List(Of Move_Info).Enumerator
        move_enum = listof_statmoves.GetEnumerator()
        move_enum.MoveNext()
        While Not move_enum.Current Is Nothing
            REM search for an effect that has ATKO-...
            Dim effects As String() = move_enum.Current.Effect.Split(",")
            Dim i As Integer = 0
            While i < effects.Length
                If second_pokemon.num_Special() > second_pokemon.num_Normal() Then
                    If effects(i).Contains("SPATKO-") Then
                        final_statmoves.Add(move_enum.Current)
                        Exit While
                    End If
                Else
                    If effects(i).Contains("ATKO-") Then
                        REM we can add this move to the final list!
                        final_statmoves.Add(move_enum.Current)
                        Exit While
                    End If
                End If
            End While
            move_enum.MoveNext()
            i += 1
        End While

        If final_statmoves.Count = 0 Then
            Return Nothing REM the pokemon has no available lowering attack moves!
        End If

        REM this is more of a defensive function so we are going to see how long we can "prolong" the battle
        REM first figure out the best move the opponent can use and how long it will take for it to kill us
        Dim oppo_move As Prediction_Move_Package = Me.FindBestMove(second_pokemon, first_pokemon, poke_calc, second_pokemon.Moves_For_Battle, max_or_min)
        Dim me_turnstofaint As Integer = Me.Project_Battle(second_pokemon.Clone(), first_pokemon.Clone(), oppo_move.Move, poke_calc, max_or_min)

        REM project how long I need to take to kill opponent
        Dim oppo_turnstofaint As Integer = Me.Project_Battle(first_pokemon.Clone(), second_pokemon.Clone(), movetouse, poke_calc, max_or_min)

        Dim attacker As Pokemon = first_pokemon.Clone()
        Dim defender As Pokemon = second_pokemon.Clone()
        Dim finalmove_enum As New List(Of Move_Info).Enumerator
        finalmove_enum = final_statmoves.GetEnumerator()
        finalmove_enum.MoveNext()
        Dim newmovestofaint As Integer = Integer.MinValue
        While Not finalmove_enum.Current Is Nothing

            poke_calc.apply_stattopokemon(defender, finalmove_enum.Current)
            REM see how long it takes for opponent to kill us with the boost!
            Dim turnstofaint As Integer = Me.Project_Battle(defender.Clone(), first_pokemon.Clone(), movetouse, poke_calc, max_or_min)
            REM see if the 
            If oppo_turnstofaint + 1 < turnstofaint Then
                REM check if the new result is better than the old one
                REM we want to select the one that takes the longest to kill us
                If turnstofaint > newmovestofaint Then
                    newmovestofaint = turnstofaint
                    statmove_touse = finalmove_enum.Current
                End If
            End If


            finalmove_enum.MoveNext()
        End While

        If newmovestofaint = Integer.MinValue Then
            Return Nothing
        Else
            move_package.Move = statmove_touse
            move_package.My_Turns = newmovestofaint
            move_package.Opponent_Turns = me_turnstofaint
        End If

        Return move_package
    End Function

    Public Function Get_BestRaiseDefenseStatMove(ByVal first_pokemon As Pokemon, ByVal second_pokemon As Pokemon, ByVal poke_calc As Poke_Calculator,
                                                ByVal movetouse As Move_Info, ByVal max_or_min As Integer) As Prediction_Move_Package
        Dim move_package As New Prediction_Move_Package
        Dim statmove_touse As New Move_Info
        Dim is_special As Boolean = False

        If movetouse.Is_Special = True Then
            is_special = True
        End If

        REM first look at lowering opponent's move stats, select the one that raises our defense
        Dim listof_statmoves As List(Of Move_Info) = first_pokemon.get_StatusMoves()
        If listof_statmoves.Count = 0 Then
            Return Nothing
        End If
        Dim final_statmoves As New List(Of Move_Info)
        Dim move_enum As New List(Of Move_Info).Enumerator
        move_enum = listof_statmoves.GetEnumerator()
        move_enum.MoveNext()
        While Not move_enum.Current Is Nothing
            REM search for an effect that has ATKO-...
            Dim effects As String() = move_enum.Current.Effect.Split(",")
            Dim i As Integer = 0
            While i < effects.Length
                If second_pokemon.num_Special() > second_pokemon.num_Normal() Then
                    REM it's more likely the pokemon will choose a special move
                    If effects(i).Contains("SPDEFU+") Then
                        final_statmoves.Add(move_enum.Current)
                        Exit While
                    End If
                Else
                    If effects(i).Contains("DEFU+") Then
                        REM we can add this move to the final list!
                        final_statmoves.Add(move_enum.Current)
                        Exit While
                    End If
                End If
            End While
            move_enum.MoveNext()
            i += 1
        End While

        If final_statmoves.Count = 0 Then
            Return Nothing REM the pokemon has no available lowering attack moves!
        End If

        REM this is more of a defensive function so we are going to see how long we can "prolong" the battle
        REM first figure out the best move the opponent can use and how long it will take for it to kill us
        Dim oppo_move As Prediction_Move_Package = Me.FindBestMove(second_pokemon, first_pokemon, poke_calc, second_pokemon.Moves_For_Battle, max_or_min)
        Dim me_turnstofaint As Integer = Me.Project_Battle(second_pokemon.Clone(), first_pokemon.Clone(), oppo_move.Move, poke_calc, max_or_min)

        REM project how long I need to take to kill opponent
        Dim oppo_turnstofaint As Integer = Me.Project_Battle(first_pokemon.Clone(), second_pokemon.Clone(), movetouse, poke_calc, max_or_min)

        Dim attacker As Pokemon = first_pokemon.Clone()
        Dim defender As Pokemon = second_pokemon.Clone()
        Dim finalmove_enum As New List(Of Move_Info).Enumerator
        finalmove_enum = final_statmoves.GetEnumerator()
        finalmove_enum.MoveNext()
        Dim newmovestofaint As Integer = Integer.MinValue
        While Not finalmove_enum.Current Is Nothing

            poke_calc.apply_stattopokemon(attacker, finalmove_enum.Current)
            REM see how long it takes for opponent to kill us with the boost!
            Dim turnstofaint As Integer = Me.Project_Battle(defender.Clone(), attacker.Clone(), movetouse, poke_calc, max_or_min)
            REM see if the 
            If oppo_turnstofaint + 1 < turnstofaint Then
                REM check if the new result is better than the old one
                REM we want to select the one that takes the longest to kill us
                If turnstofaint > newmovestofaint Then
                    newmovestofaint = turnstofaint
                    statmove_touse = finalmove_enum.Current
                End If
            End If


            finalmove_enum.MoveNext()
        End While

        If newmovestofaint = Integer.MinValue Then
            Return Nothing
        Else
            move_package.Move = statmove_touse
            move_package.My_Turns = newmovestofaint
            move_package.Opponent_Turns = me_turnstofaint
        End If

        

        Return move_package
    End Function

    Public Function FindBestStatusMove(ByVal first_pokemon As Pokemon, ByVal second_pokemon As Pokemon, ByVal poke_calc As Poke_Calculator,
                                     ByVal movetouse As Move_Info, ByVal max_or_min As Integer, ByVal off_or_def As Integer) As Prediction_Move_Package

        Dim listofstatus As List(Of Move_Info) = first_pokemon.get_StatusCondMoves()
        Return Nothing
    End Function

End Class

''' <summary>
''' A Package containing the winning team, the number of turns it took for the winner to win,
''' and a battle log (not implemented yet)
''' </summary>
''' <remarks></remarks>
Public Class Prediction_Package
    Dim m_winning_team As String
    Dim m_num_turnstowin As Integer
    Dim m_battle_log As List(Of String)

    Public Property Winning_Team() As String
        Get
            Return m_winning_team
        End Get
        Set(value As String)
            m_winning_team = value
        End Set
    End Property

    Public Property Turns_To_Win() As Integer
        Get
            Return m_num_turnstowin
        End Get
        Set(value As Integer)
            m_num_turnstowin = value
        End Set
    End Property
End Class

Public Class Prediction_Move_Package
    Dim m_move As Move_Info
    Dim m_myturns As Integer
    Dim m_oppturns As Integer

    Public Property Move() As Move_Info
        Get
            Return m_move
        End Get
        Set(value As Move_Info)
            m_move = value
        End Set
    End Property

    Public Property My_Turns As Integer
        Get
            Return m_myturns
        End Get
        Set(value As Integer)
            m_myturns = value
        End Set
    End Property

    Public Property Opponent_Turns As Integer
        Get
            Return m_oppturns
        End Get
        Set(value As Integer)
            m_oppturns = value
        End Set
    End Property
End Class
