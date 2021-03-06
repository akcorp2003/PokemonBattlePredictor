﻿Imports System.Collections
Imports System.Reflection
Imports PokemonBattlePredictor
Imports PokemonBattlePredictor.PBP
Imports PokemonBattlePredictor.PBP.InfoBlocks

Namespace PBP
    Public Class Pokemon
        Implements System.ICloneable

        Private m_name As String
        Private m_ability As New List(Of Ability_Info)
        Private m_ATK As Integer
        Private m_DEF As Integer
        Private m_Sp_ATK As Integer
        Private m_Sp_DEF As Integer
        Private m_HP As Integer
        Private m_SPD As Integer
        Private m_Moves As New List(Of String)
        Private m_Moves_for_Battle As New List(Of Move_Info)
        Private m_Type As New List(Of String)
        Private m_NextMove As New Move_Info
        Private m_stage As Integer = 0
        Private m_statuscondition As Integer
        Private m_otherstatus As Integer
        Private m_team As String
        Private m_ATKboost As Integer
        Private m_DEFboost As Integer
        Private m_SPATKboost As Integer
        Private m_SPDEFboost As Integer
        Private m_SPDboost As Integer

        Public Property Name() As String
            Get
                Return m_name
            End Get
            Set(value As String)
                m_name = value
            End Set
        End Property

        Public Property ATK() As Integer
            Get
                Return m_ATK
            End Get
            Set(value As Integer)
                m_ATK = value
            End Set
        End Property

        Public Property DEF() As Integer
            Get
                Return m_DEF
            End Get
            Set(value As Integer)
                m_DEF = value
            End Set
        End Property

        Public Property Sp_ATK() As Integer
            Get
                Return m_Sp_ATK
            End Get
            Set(value As Integer)
                m_Sp_ATK = value
            End Set
        End Property

        Public Property Sp_DEF() As Integer
            Get
                Return m_Sp_DEF
            End Get
            Set(value As Integer)
                m_Sp_DEF = value
            End Set
        End Property

        Public Property HP() As Integer
            Get
                Return m_HP
            End Get
            Set(value As Integer)
                m_HP = value
            End Set
        End Property

        Public Property SPD() As Integer
            Get
                Return m_SPD
            End Get
            Set(value As Integer)
                m_SPD = value
            End Set
        End Property

        Public Property ATK_Boost() As Integer
            Get
                Return m_ATKboost
            End Get
            Set(value As Integer)
                m_ATKboost = value
            End Set
        End Property

        Public Property DEF_Boost() As Integer
            Get
                Return m_DEFboost
            End Get
            Set(value As Integer)
                m_DEFboost = value
            End Set
        End Property

        Public Property SP_ATK_Boost() As Integer
            Get
                Return m_SPATKboost
            End Get
            Set(value As Integer)
                m_SPATKboost = value
            End Set
        End Property

        Public Property SP_DEF_Boost() As Integer
            Get
                Return m_SPDEFboost
            End Get
            Set(value As Integer)
                m_SPDEFboost = value
            End Set
        End Property

        Public Property SPEED_Boost() As Integer
            Get
                Return m_SPDboost
            End Get
            Set(value As Integer)
                m_SPDboost = value
            End Set
        End Property

        ''' <summary>
        ''' Stage for determining critical hits
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Stage() As Integer
            Get
                Return m_stage
            End Get
            Set(value As Integer)
                m_stage = value
            End Set
        End Property

        Public Property Status_Condition() As Integer
            Get
                Return m_statuscondition
            End Get
            Set(value As Integer)
                m_statuscondition = value
            End Set
        End Property

        ''' <summary>
        ''' Other status refers to Confusion, attraction
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Other_Status_Condition() As Integer
            Get
                Return m_otherstatus
            End Get
            Set(value As Integer)
                m_otherstatus = value
            End Set
        End Property

        Public Property Next_Move() As Move_Info
            Get
                Return m_NextMove
            End Get
            Set(value As Move_Info)
                m_NextMove = value
            End Set
        End Property

        Public Property Team() As String
            Get
                Return m_team
            End Get
            Set(value As String)
                m_team = value
            End Set
        End Property

        REM mimic a copy constructor
        Public Property Moves() As List(Of String)
            Get
                Return m_Moves
            End Get
            Set(value As List(Of String))
                For Each Move As String In value
                    m_Moves.Add(Move)
                Next
            End Set
        End Property

        REM Mimic a copy constructor
        Public Property Types() As List(Of String)
            Get
                Return m_Type
            End Get
            Set(value As List(Of String))
                For Each Type As String In value
                    m_Type.Add(Type)
                Next
            End Set
        End Property

        Public Property Ability() As List(Of Ability_Info)
            Get
                Return m_ability
            End Get
            Set(value As List(Of Ability_Info))
                For Each ability As Ability_Info In value
                    m_ability.Add(ability)
                Next
            End Set
        End Property

        Public Property Moves_For_Battle() As List(Of Move_Info)
            Get
                Return m_Moves_for_Battle
            End Get
            Set(value As List(Of Move_Info))
                For Each move As Move_Info In value
                    m_Moves_for_Battle.Add(move)
                Next
            End Set
        End Property

        ''' <summary>
        ''' Locates the move of the pokemon and returns a reference to it. Nothing if none found.
        ''' This function searches the move by name.
        ''' </summary>
        ''' <param name="name">The name of the move to look for.</param>
        ''' <value></value>
        ''' <returns>A reference to the move or nothing.</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Moves_For_Battle(ByVal name As String) As Move_Info
            Get
                For i As Integer = 0 To m_Moves_for_Battle.Count - 1 Step 1
                    If Constants.Get_FormattedString(m_Moves_for_Battle(i).Name) = Constants.Get_FormattedString(name) Then
                        Return m_Moves_for_Battle.Item(i)
                    End If
                Next
                Return Nothing
            End Get
        End Property

        Public Function is_nomovesleft() As Boolean
            Dim no_moves As Boolean = False

            Dim nomoves_counter As Integer = 0
            For i As Integer = 0 To Me.Moves_For_Battle.Count - 1 Step 1
                If Me.Moves_For_Battle.Item(i).PP = 0 Then
                    nomoves_counter += 1
                End If
            Next

            If nomoves_counter = Me.Moves_For_Battle.Count Then
                no_moves = True
            End If

            Return no_moves
        End Function

        ''' <summary>
        ''' Returns the number of special moves that the Pokemon has in Moves_For_Battle
        ''' </summary>
        ''' <returns>The number of special battling moves</returns>
        ''' <remarks></remarks>
        Public Function num_Special() As Integer
            Dim move_enum As New List(Of Move_Info).Enumerator
            move_enum = m_Moves_for_Battle.GetEnumerator()
            move_enum.MoveNext()
            Dim num_moves As Integer = 0
            While Not move_enum.Current Is Nothing
                If move_enum.Current.Is_Special = True Then
                    num_moves += 1
                End If
                move_enum.MoveNext()
            End While
            Return num_moves
        End Function

        ''' <summary>
        ''' Returns a list of special moves. Make sure to clone these moves if you are going to modify them!
        ''' </summary>
        ''' <returns>A list of special moves. Make sure to clone!</returns>
        ''' <remarks></remarks>
        Public Function get_Special() As List(Of Move_Info)
            Dim s_list As New List(Of Move_Info)
            Dim move_enum As New List(Of Move_Info).Enumerator
            move_enum = m_Moves_for_Battle.GetEnumerator()
            move_enum.MoveNext()
            While Not move_enum.Current Is Nothing
                If move_enum.Current.Is_Special = True Then
                    s_list.Add(move_enum.Current)
                End If
            End While
            Return s_list
        End Function

        Public Function num_Normal() As Integer
            Dim move_enum As New List(Of Move_Info).Enumerator
            move_enum = m_Moves_for_Battle.GetEnumerator()
            move_enum.MoveNext()
            Dim num_moves As Integer = 0
            While Not move_enum.Current Is Nothing
                REM try not to confuse normal damage with stat moves
                If move_enum.Current.Is_Special = False And move_enum.Current.Power > 0 Then
                    num_moves += 1
                End If
                move_enum.MoveNext()
            End While
            Return num_moves
        End Function

        Public Function get_Normal() As List(Of Move_Info)
            Dim n_list As New List(Of Move_Info)
            Dim move_enum As New List(Of Move_Info).Enumerator
            move_enum = m_Moves_for_Battle.GetEnumerator()
            move_enum.MoveNext()

            While Not move_enum.Current Is Nothing
                REM try not to confuse normal damage with stat moves
                If move_enum.Current.Is_Special = False And move_enum.Current.Power > 0 Then
                    n_list.Add(move_enum.Current)
                End If
                move_enum.MoveNext()
            End While
            Return n_list
        End Function

        Public Function num_StatusMoves() As Integer
            Dim move_enum As New List(Of Move_Info).Enumerator
            move_enum = m_Moves_for_Battle.GetEnumerator()
            move_enum.MoveNext()
            Dim num_moves As Integer = 0
            While Not move_enum.Current Is Nothing
                REM try not to confuse normal damage with stat moves
                If move_enum.Current.Power = 0 Then
                    num_moves += 1
                End If
                move_enum.MoveNext()
            End While
            Return num_moves
        End Function

        Public Function get_StatusMoves() As List(Of Move_Info)
            Dim st_list As New List(Of Move_Info)
            Dim move_enum As New List(Of Move_Info).Enumerator
            move_enum = m_Moves_for_Battle.GetEnumerator()
            move_enum.MoveNext()
            Dim num_moves As Integer = 0
            While Not move_enum.Current Is Nothing
                REM try not to confuse normal damage with stat moves
                If move_enum.Current.Power = 0 Then
                    st_list.Add(move_enum.Current)
                End If
                move_enum.MoveNext()
            End While
            Return st_list
        End Function

        Public Function get_StatusCondMoves() As List(Of Move_Info)
            Dim st_list As New List(Of Move_Info)
            Dim move_enum As New List(Of Move_Info).Enumerator
            move_enum = m_Moves_for_Battle.GetEnumerator()
            move_enum.MoveNext()
            While Not move_enum.Current Is Nothing
                Dim stats_indic As String() = {"BRN", "FRZ", "PRLYZ", "SLP", "PSN"}
                For i As Integer = 0 To stats_indic.Length - 1 Step 1
                    If move_enum.Current.Effect.Contains(stats_indic(i)) Then
                        st_list.Add(move_enum.Current)
                    End If
                Next
                move_enum.MoveNext()
            End While
            Return st_list
        End Function

        Public Function get_OtherStatusCondMoves() As List(Of Move_Info)
            Dim otherst_list As New List(Of Move_Info)
            Dim move_enum As New List(Of Move_Info).Enumerator
            move_enum = m_Moves_for_Battle.GetEnumerator()
            move_enum.MoveNext()
            While Not move_enum.Current Is Nothing
                Dim other_stats As String() = {"CONF", "ATTR"}
                For i As Integer = 0 To other_stats.Length - 1 Step 1
                    If move_enum.Current.Effect.Contains(other_stats(i)) Then
                        otherst_list.Add(move_enum.Current)
                    End If
                Next
                move_enum.MoveNext()
            End While
            Return otherst_list
        End Function

        Public Function get_HealingMoves() As List(Of Move_Info)
            Dim heal_list As New List(Of Move_Info)
            Dim move_enum As New List(Of Move_Info).Enumerator
            move_enum = m_Moves_for_Battle.GetEnumerator()
            move_enum.MoveNext()
            While Not move_enum.Current Is Nothing
                If move_enum.Current.Effect.Contains("HPdrainO") OrElse move_enum.Current.Effect.Contains("HPhalfU") OrElse _
                    move_enum.Current.Effect.Contains("HPfullU") Then
                    heal_list.Add(move_enum.Current)
                End If
                move_enum.MoveNext()
            End While
            Return heal_list
        End Function

        Public Function get_BRNMoves() As List(Of Move_Info)
            Dim brn_list As New List(Of Move_Info)
            Dim move_enum As New List(Of Move_Info).Enumerator
            move_enum = m_Moves_for_Battle.GetEnumerator()
            move_enum.MoveNext()
            While Not move_enum.Current Is Nothing
                If move_enum.Current.Effect.Contains("BRN") Then
                    brn_list.Add(move_enum.Current)
                End If
                move_enum.MoveNext()
            End While
            Return brn_list
        End Function

        Public Function get_FRZMoves() As List(Of Move_Info)
            Dim frz_list As New List(Of Move_Info)
            Dim move_enum As New List(Of Move_Info).Enumerator
            move_enum = m_Moves_for_Battle.GetEnumerator()
            move_enum.MoveNext()
            While Not move_enum.Current Is Nothing
                If move_enum.Current.Effect.Contains("FRZ") Then
                    frz_list.Add(move_enum.Current)
                End If
                move_enum.MoveNext()
            End While
            Return frz_list
        End Function

        Public Function get_PRLYZMoves() As List(Of Move_Info)
            Dim prlyz_list As New List(Of Move_Info)
            Dim move_enum As New List(Of Move_Info).Enumerator
            move_enum = m_Moves_for_Battle.GetEnumerator()
            move_enum.MoveNext()
            While Not move_enum.Current Is Nothing
                If move_enum.Current.Effect.Contains("PRLYZ") Then
                    prlyz_list.Add(move_enum.Current)
                End If
                move_enum.MoveNext()
            End While
            Return prlyz_list
        End Function

        Public Function get_SLPMoves() As List(Of Move_Info)
            Dim SLP_list As New List(Of Move_Info)
            Dim move_enum As New List(Of Move_Info).Enumerator
            move_enum = m_Moves_for_Battle.GetEnumerator()
            move_enum.MoveNext()
            While Not move_enum.Current Is Nothing
                If move_enum.Current.Effect.Contains("SLP") Then
                    SLP_list.Add(move_enum.Current)
                End If
                move_enum.MoveNext()
            End While
            Return SLP_list
        End Function

        Public Function get_PSNMoves() As List(Of Move_Info)
            Dim PSN_list As New List(Of Move_Info)
            Dim move_enum As New List(Of Move_Info).Enumerator
            move_enum = m_Moves_for_Battle.GetEnumerator()
            move_enum.MoveNext()
            While Not move_enum.Current Is Nothing
                If move_enum.Current.Effect.Contains("PSN") Then
                    PSN_list.Add(move_enum.Current)
                End If
                move_enum.MoveNext()
            End While
            Return PSN_list
        End Function

        Public Function get_CONFMoves() As List(Of Move_Info)
            Dim CONF_list As New List(Of Move_Info)
            Dim move_enum As New List(Of Move_Info).Enumerator
            move_enum = m_Moves_for_Battle.GetEnumerator()
            move_enum.MoveNext()
            While Not move_enum.Current Is Nothing
                If move_enum.Current.Effect.Contains("CONF") Then
                    CONF_list.Add(move_enum.Current)
                End If
                move_enum.MoveNext()
            End While
            Return CONF_list
        End Function

        Public Function get_StrongestMove() As Move_Info
            Dim strongmove As New Move_Info
            Dim move_enum As New List(Of Move_Info).Enumerator
            move_enum = m_Moves_for_Battle.GetEnumerator()
            move_enum.MoveNext()
            While Not move_enum.Current Is Nothing
                If move_enum.Current.Power > strongmove.Power Then
                    strongmove = move_enum.Current
                ElseIf move_enum.Current.Power = strongmove.Power Then
                    REM choose higher accuracy
                    If move_enum.Current.Accuracy > strongmove.Accuracy Then
                        strongmove = move_enum.Current
                    ElseIf move_enum.Current.Accuracy = strongmove.Accuracy Then
                        REM choose higher PP
                        If move_enum.Current.PP > strongmove.PP Then
                            strongmove = move_enum.Current
                            REM otherwise, we'll just stick with the current one (well, there is the type bonus but not
                            REM going to implement it yet TODO)
                        End If
                    End If
                End If
                move_enum.MoveNext()
            End While
            Return strongmove
        End Function

        Public Function Clone() As Object Implements ICloneable.Clone
            Dim freshpokemon As New Pokemon
            freshpokemon.ATK = Me.ATK
            freshpokemon.DEF = Me.DEF
            freshpokemon.Sp_ATK = Me.Sp_ATK
            freshpokemon.Sp_DEF = Me.Sp_DEF
            freshpokemon.SPD = Me.SPD
            freshpokemon.Name = Me.Name
            freshpokemon.HP = Me.HP
            freshpokemon.ATK_Boost = Me.ATK_Boost
            freshpokemon.DEF_Boost = Me.DEF_Boost
            freshpokemon.SP_ATK_Boost = Me.SP_ATK_Boost
            freshpokemon.SP_DEF_Boost = Me.SP_DEF_Boost
            freshpokemon.SPEED_Boost = Me.SPEED_Boost
            freshpokemon.Status_Condition = Me.Status_Condition
            freshpokemon.Other_Status_Condition = Me.Other_Status_Condition
            freshpokemon.Team = Me.Team
            freshpokemon.Moves = Me.Moves.Select(Function(x) x.Clone()).Cast(Of String).ToList()
            freshpokemon.Types = Me.Types.Select(Function(x) x.Clone()).Cast(Of String).ToList()
            freshpokemon.Ability = Me.Ability.Select(Function(x) x.Clone()).Cast(Of Ability_Info).ToList()
            freshpokemon.Moves_For_Battle = Me.Moves_For_Battle.Select(Function(x) x.Clone()).Cast(Of Move_Info).ToList()

            Return freshpokemon
        End Function
    End Class
End Namespace
