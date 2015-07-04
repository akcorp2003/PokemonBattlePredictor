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
                Me.apply_battle(first_pokemon, turn_queue.Peek(), poke_calc, battle_arena)

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

                Me.apply_battle(turn_queue.Peek(), first_pokemon, poke_calc, battle_arena)
                turn_queue.Dequeue()

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

            Else
                MessageBox.Show("Something went wrong in predict_battle(), specifically with the queue. Returning...", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            REM pop first thing off stack, that will be the first pokemon to move




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
        Dim isthere_SEmove As List(Of Move_Info)
        isthere_SEmove = Me.IsThereSuperEffectiveMove(first_pokemon, second_pokemon, effectiveness_table)

        If Not isthere_SEmove.Count = 0 Then
            REM there exists a supereffective move we can use!

            turn_poke_move = Me.FindBestMove(first_pokemon, second_pokemon, poke_calc, isthere_SEmove, 1)
            turn_poke_move2 = Me.FindBestStatMove(first_pokemon, second_pokemon, poke_calc, turn_poke_move, 1)



            REM once we have selected the move to use, apply the damage
            poke_calc.apply_damage(first_pokemon, second_pokemon, turn_poke_move, poke_calc, 1)

        Else
            REM TODO: add check for status moves and stat lowering moves

            REM we don't have any SE moves, check for normal damaging moves
            Dim isthere_normmove As New List(Of Move_Info)
            isthere_normmove = Me.IsThereNormalDamageMove(first_pokemon, second_pokemon, effectiveness_table)

            If Not isthere_normmove.Count = 0 Then
                REM we can apply a normal damaging move!
                'Dim turnstofaint As Integer = Integer.MaxValue
                'Dim attacker As New Pokemon
                'Dim defender As New Pokemon
                'attacker = first_pokemon.Clone()
                'defender = second_pokemon.Clone()

                'REM find normal move with max damage
                'Dim i As Integer = 0
                'Dim new_turnstofaint As Integer = 0
                'Dim normmove_enum As New List(Of Move_Info).Enumerator
                'normmove_enum = isthere_normmove.GetEnumerator()
                'normmove_enum.MoveNext()
                'While i < isthere_normmove.Count
                '    new_turnstofaint = Me.Project_Battle(first_pokemon.Clone(), second_pokemon.Clone(), normmove_enum.Current(), poke_calc, 1)

                '    If new_turnstofaint < turnstofaint Then
                '        turnstofaint = new_turnstofaint
                '        turn_poke_move = normmove_enum.Current
                '    ElseIf new_turnstofaint = turnstofaint Then
                '        REM choose the higher power
                '        If normmove_enum.Current.Power > turn_poke_move.Power Then
                '            turn_poke_move = normmove_enum.Current
                '        End If

                '    End If
                '    normmove_enum.MoveNext()
                '    i += 1

                'End While
                turn_poke_move = Me.FindBestMove(first_pokemon, second_pokemon, poke_calc, isthere_normmove, 1)
                turn_poke_move2 = Me.FindBestStatMove(first_pokemon, second_pokemon, poke_calc, turn_poke_move, 1)

                poke_calc.apply_damage(first_pokemon, second_pokemon, turn_poke_move, poke_calc, 1)
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
                    turn_poke_move2 = Me.FindBestStatMove(first_pokemon, second_pokemon, poke_calc, Nothing, 1)
                Else
                    REM choose the best damaging move
                    Dim isthere_noneffectivemove As New List(Of Move_Info)
                    isthere_noneffectivemove = Me.IsThereNotEffectiveMoves(first_pokemon, second_pokemon, effectiveness_table)
                    If Not isthere_noneffectivemove.Count = 0 Then

                        turn_poke_move = Me.FindBestMove(first_pokemon, second_pokemon, poke_calc, isthere_noneffectivemove, 1)
                        poke_calc.apply_damage(first_pokemon, second_pokemon, turn_poke_move, poke_calc, 1)

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
                                 ByVal availmoves As List(Of Move_Info), ByVal max_or_min As Integer) As Move_Info
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

            If new_turnstofaint < turnstofaint Then
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

        Return turn_poke_move
    End Function

    Public Function FindBestStatMove(ByVal first_pokemon As Pokemon, ByVal second_pokemon As Pokemon, ByVal poke_calc As Poke_Calculator,
                                     ByVal movetouse As Move_Info, ByVal max_or_min As Integer) As Move_Info
        Dim statmove_touse As New Move_Info
        Dim attack_move As New Prediction_Move_Package
        Dim defense_move As New Prediction_Move_Package

        REM first checkout offensive moves
        attack_move = Me.Get_BestRaiseAttackStatMove(first_pokemon.Clone(), second_pokemon.Clone(), poke_calc, movetouse.Clone(), max_or_min)
        defense_move = Me.Get_BestLowerDefenseStatMove(first_pokemon.Clone(), second_pokemon.Clone(), poke_calc, movetouse.Clone(), max_or_min)


        REM for testing purposes:

        If attack_move.Turns <= defense_move.Turns Then
            statmove_touse = attack_move.Move
        Else
            statmove_touse = defense_move.Move
        End If




        Return statmove_touse

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
        Dim oppo_move As Move_Info = Me.FindBestMove(second_pokemon, first_pokemon, poke_calc, second_pokemon.Moves_For_Battle, max_or_min)
        Dim me_turnstofaint As Integer = Me.Project_Battle(second_pokemon.Clone(), first_pokemon.Clone(), oppo_move, poke_calc, max_or_min)


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

        move_package.Move = statmove_touse
        move_package.Turns = newmovestofaint

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
            End While
            move_enum.MoveNext()
            i += 1
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
        Dim oppo_move As Move_Info = Me.FindBestMove(second_pokemon, first_pokemon, poke_calc, second_pokemon.Moves_For_Battle, max_or_min)
        Dim me_turnstofaint As Integer = Me.Project_Battle(second_pokemon.Clone(), first_pokemon.Clone(), oppo_move, poke_calc, max_or_min)

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

        move_package.Move = statmove_touse
        move_package.Turns = newmovestofaint

        Return move_package


        Return move_package
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
    Dim m_turns As Integer

    Public Property Move() As Move_Info
        Get
            Return m_move
        End Get
        Set(value As Move_Info)
            m_move = value
        End Set
    End Property

    Public Property Turns As Integer
        Get
            Return m_turns
        End Get
        Set(value As Integer)
            m_turns = value
        End Set
    End Property
End Class
