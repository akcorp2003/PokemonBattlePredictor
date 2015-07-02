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

    Private Function predict_battle(ByVal battle_arena As Pokemon_Arena) As String

        If battle_arena.Get_TurnNumber = 0 Then
            REM load the first pokemon of each team as the current battling
            battle_arena.AddTo_CurrentBattling_Red(battle_arena.Get_TeamRed.Get_Team("red").First)
            battle_arena.AddTo_CurrentBattling_Blue(battle_arena.Get_TeamBlue.Get_Team("blue").First)
        End If

        Dim poke_calc As New Poke_Calculator
        Dim first_pokemon As New Pokemon REM the first pokemon to move during this turn cycle
        Dim turn_queue As New Queue(Of Pokemon)


        'loop until one team is dead
        While Not battle_arena.IsBlueFainted And Not battle_arena.IsRedFainted
            REM begin actual battle logic

            If turn_queue.Count = 0 Then
                REM populate the queue since we are on a fresh cycle
                REM check speed, the higher speed stat pokemon moves first
                If battle_arena.CurrentBattlingBlue.First.SPD > battle_arena.CurrentBattlingRed.First.SPD Then
                    turn_queue.Enqueue(battle_arena.CurrentBattlingBlue.First)
                    turn_queue.Enqueue(battle_arena.CurrentBattlingRed.First)

                ElseIf battle_arena.CurrentBattlingBlue.First.SPD < battle_arena.CurrentBattlingRed.First.SPD Then
                    REM red goes first
                    turn_queue.Enqueue(battle_arena.CurrentBattlingRed.First)
                    turn_queue.Enqueue(battle_arena.CurrentBattlingBlue.First)

                Else
                    REM same speed, in this case, it will be random
                End If
            End If

            If turn_queue.Count = 2 Then

                first_pokemon = turn_queue.Dequeue()
                Me.apply_battle(first_pokemon, turn_queue.Peek(), poke_calc)

            ElseIf turn_queue.Count = 1 Then

                Me.apply_battle(turn_queue.Peek(), first_pokemon, poke_calc)
                turn_queue.Dequeue()

            Else
                MessageBox.Show("Something went wrong in predict_battle(), specifically with the queue. Returning...", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            REM pop first thing off stack, that will be the first pokemon to move


            

        End While


        If battle_arena.IsBlueFainted = True Then
            Return "blue"
        ElseIf battle_arena.IsRedFainted = True Then
            Return "red"
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

                If effect_value = 1 Then
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
    ''' <remarks>Non-damaging moves are not accepted</remarks>
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
    ''' Applys the battle logic for pokemon battles.
    ''' The function looks 
    ''' </summary>
    ''' <param name="first_pokemon">The attacking Pokemon</param>
    ''' <param name="second_pokemon">The defending Pokemon</param>
    ''' <param name="poke_calc"></param>
    ''' <remarks></remarks>
    Private Sub apply_battle(ByRef first_pokemon As Pokemon, ByRef second_pokemon As Pokemon, ByVal poke_calc As Poke_Calculator)
        Dim turn_poke_move As New Move_Info
        Dim isthere_SEmove As List(Of Move_Info)
        isthere_SEmove = Me.IsThereSuperEffectiveMove(first_pokemon, second_pokemon, effectiveness_table)

        If Not isthere_SEmove.Count = 0 Then
            REM there exists a supereffective move we can use!
            Dim SE_turnstofaint As Integer = Integer.MaxValue
            Dim attacker As New Pokemon
            Dim defender As New Pokemon
            attacker = first_pokemon.Clone()
            defender = second_pokemon.Clone() REM this is guaranteed to be the defending pokemon

            Dim i As Integer = 0
            Dim new_SE_turnstofaint As Integer = 0
            Dim SEmove_enum As New List(Of Move_Info).Enumerator
            SEmove_enum = isthere_SEmove.GetEnumerator()
            SEmove_enum.MoveNext()
            While i < isthere_SEmove.Count
                REM apply each available super effective move to the defender, take the move that has the fewest turns until faint
                REM TODO: for the 1, we are going to implement a "Derp" factor based on what the user think is the stupidity of the players
                new_SE_turnstofaint = Me.Project_Battle(first_pokemon.Clone(), second_pokemon.Clone(), SEmove_enum.Current, poke_calc, 1)

                If new_SE_turnstofaint < SE_turnstofaint Then
                    REM we have a new move in town
                    SE_turnstofaint = new_SE_turnstofaint
                    turn_poke_move = SEmove_enum.Current
                ElseIf new_SE_turnstofaint = SE_turnstofaint Then
                    REM choose the higher power
                    If SEmove_enum.Current.Power > turn_poke_move.Power Then
                        turn_poke_move = SEmove_enum.Current
                    End If
                End If

                SEmove_enum.MoveNext()
                i += 1
            End While

            REM once we have selected the move to use, apply the damage
            poke_calc.apply_damage(first_pokemon, second_pokemon, turn_poke_move, poke_calc, 1)

        Else
            REM TODO: add check for status moves and stat lowering moves

            REM we don't have any SE moves, check for normal damaging moves
            Dim isthere_normmove As New List(Of Move_Info)
            isthere_normmove = Me.IsThereNormalDamageMove(first_pokemon, second_pokemon, effectiveness_table)

            If Not isthere_normmove.Count = 0 Then
                REM we can apply a normal damaging move!
                Dim turnstofaint As Integer = Integer.MaxValue
                Dim attacker As New Pokemon
                Dim defender As New Pokemon
                attacker = first_pokemon.Clone()
                defender = second_pokemon.Clone()

                REM find normal move with max damage
                Dim i As Integer = 0
                Dim new_turnstofaint As Integer = 0
                Dim normmove_enum As New List(Of Move_Info).Enumerator
                normmove_enum = isthere_normmove.GetEnumerator()
                normmove_enum.MoveNext()
                While i < isthere_normmove.Count
                    new_turnstofaint = Me.Project_Battle(first_pokemon.Clone(), second_pokemon.Clone(), normmove_enum.Current(), poke_calc, 1)

                    If new_turnstofaint < turnstofaint Then
                        turnstofaint = new_turnstofaint
                        turn_poke_move = normmove_enum.Current
                    ElseIf new_turnstofaint = turnstofaint Then
                        REM choose the higher power
                        If normmove_enum.Current.Power > turn_poke_move.Power Then
                            turn_poke_move = normmove_enum.Current
                        End If

                    End If
                    normmove_enum.MoveNext()
                    i += 1

                End While
                poke_calc.apply_damage(first_pokemon, second_pokemon, turn_poke_move, poke_calc, 1)
            End If
        End If
    End Sub

End Class
