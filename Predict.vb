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

        'loop until one team is dead
        While Not battle_arena.IsBlueFainted And Not battle_arena.IsRedFainted
            REM begin actual battle logic

            REM check speed, the higher speed stat pokemon moves first
            If battle_arena.CurrentBattlingBlue.First.SPD > battle_arena.CurrentBattlingRed.First.SPD Then
                REM blue goes first
                Dim isthere_SEmove As String = ""
                isthere_SEmove = Me.IsThereSuperEffectiveMove(battle_arena.CurrentBattlingBlue.First, battle_arena.CurrentBattlingRed.First, effectiveness_table)


            ElseIf battle_arena.CurrentBattlingBlue.First.SPD < battle_arena.CurrentBattlingRed.First.SPD Then
                REM red goes first
            Else
                REM same speed, in this case, it will be random
            End If
        End While


        If battle_arena.IsBlueFainted = True Then
            Return "blue"
        ElseIf battle_arena.IsRedFainted = True Then
            Return "red"
        Else
            Return ""
        End If

    End Function

    Public Function IsThereSuperEffectiveMove(ByVal attacking_pokemon As Pokemon, ByVal defending_pokemon As Pokemon, ByVal effectiveness_table As EffectivenessTable) As String
        Dim movename As String = ""

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
                End If
            Next

            my_attackenum.MoveNext()
        Next

        Return movename
    End Function
End Class
