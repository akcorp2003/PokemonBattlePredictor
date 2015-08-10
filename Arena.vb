Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports PokemonBattlePredictor
Imports PokemonBattlePredictor.PBP

'Arena_Constants is not thread-safe!
Namespace PBP
    Module Arena_Constants
        Dim m_blueflinch As Boolean = False
        Dim m_redflinch As Boolean = False

        Public Property Blue_Flinch As Boolean
            Get
                Return m_blueflinch
            End Get
            Set(value As Boolean)
                m_blueflinch = value
            End Set
        End Property

        Public Property Red_Flinch As Boolean
            Get
                Return m_redflinch
            End Get
            Set(value As Boolean)
                m_redflinch = value
            End Set
        End Property
    End Module
End Namespace

Namespace PBP
    Public Class Arena
        Implements ICloneable

        Dim m_Team_Blue As New Pokemon_Team
        Dim m_Team_Red As New Pokemon_Team
        Dim m_turn_number As Integer = 0
        Dim m_curr_attacker As String

        Dim currentbattlingpokemon_blue As New List(Of Pokemon)
        Dim currentbattlingpokemon_red As New List(Of Pokemon)

        Public Sub New()

        End Sub

        Public Sub New(ByVal arena As Arena)
            m_Team_Blue = arena.Team_Blue.Clone()
            m_Team_Red = arena.Team_Red.Clone()
            m_turn_number = arena.Turn_Number
            m_curr_attacker = arena.Current_Attacker

            currentbattlingpokemon_blue = arena.currentbattlingpokemon_blue.Select(Function(x) x.Clone()).Cast(Of Pokemon).ToList()
            currentbattlingpokemon_red = arena.currentbattlingpokemon_red.Select(Function(x) x.Clone()).Cast(Of Pokemon).ToList()
        End Sub

        Public Property Team_Blue() As Pokemon_Team
            Get
                Return m_Team_Blue
            End Get
            Set(value As Pokemon_Team)
                m_Team_Blue = value.Clone()
            End Set
        End Property

        Public Property Team_Red() As Pokemon_Team
            Get
                Return m_Team_Red
            End Get
            Set(value As Pokemon_Team)
                m_Team_Red = value.Clone()
            End Set
        End Property

        Public Property Turn_Number() As Integer
            Get
                Return m_turn_number
            End Get
            Set(value As Integer)
                m_turn_number = value
            End Set
        End Property

        Public Property Current_Attacker() As String
            Get
                Return m_curr_attacker
            End Get
            Set(value As String)
                m_curr_attacker = value
            End Set
        End Property

        Public Function Get_TeamBlue() As Pokemon_Team
            Return m_Team_Blue
        End Function

        Public Function Get_TeamRed() As Pokemon_Team
            Return m_Team_Red
        End Function

        Public Function CurrentBattlingRed() As List(Of Pokemon)
            Return currentbattlingpokemon_red
        End Function

        Public Function CurrentBattlingBlue() As List(Of Pokemon)
            Return currentbattlingpokemon_blue
        End Function

        Public Function Get_TurnNumber() As Integer
            Return m_turn_number
        End Function

        Public Sub Increment_Turn()
            m_turn_number += 1
        End Sub

        Public Overridable Function Clone() As Object Implements ICloneable.Clone
            Dim fresharena As New Arena
            fresharena.Turn_Number = Me.Turn_Number
            fresharena.Team_Blue = Me.Team_Blue.Clone()
            fresharena.Team_Red = Me.Team_Red.Clone()
            fresharena.Current_Attacker = Me.Current_Attacker
            fresharena.currentbattlingpokemon_blue = Me.currentbattlingpokemon_blue.Select(Function(x) x.Clone()).Cast(Of Pokemon).ToList()
            fresharena.currentbattlingpokemon_red = Me.currentbattlingpokemon_red.Select(Function(x) x.Clone()).Cast(Of Pokemon).ToList()
            Return fresharena
        End Function
    End Class

    Public Class Pokemon_Arena
        Inherits Arena

        Dim m_last_fainted As String
        Dim m_sleepturns_blue As Integer
        Dim m_sleepturns_red As Integer
        Dim m_bpoisonturns_blue As Integer
        Dim m_bpoisonturns_red As Integer
        Dim m_confuseturns_red As Integer
        Dim m_confuseturns_blue As Integer

        Public Sub New()

        End Sub

        Public Sub New(ByVal p_arena As Pokemon_Arena)
            MyBase.New(p_arena)
            m_last_fainted = p_arena.Last_Fainted
            m_sleepturns_blue = p_arena.Blue_NumSleep
            m_sleepturns_red = p_arena.Red_NumSleep
            m_bpoisonturns_blue = p_arena.Blue_NumBadPoison
            m_bpoisonturns_red = p_arena.Red_NumBadPoison
        End Sub

        Public Property Blue_NumSleep As Integer
            Get
                Return m_sleepturns_blue
            End Get
            Set(value As Integer)
                m_sleepturns_blue = value
            End Set
        End Property

        Public Sub IncreaseNumSleep_Blue()
            m_sleepturns_blue += 1
        End Sub

        Public Property Red_NumSleep As Integer
            Get
                Return m_sleepturns_red
            End Get
            Set(value As Integer)
                m_sleepturns_red = value
            End Set
        End Property

        Public Sub IncreaseNumSleep_Red()
            m_sleepturns_red += 1
        End Sub

        Public Property Blue_NumBadPoison As Integer
            Get
                Return m_bpoisonturns_blue
            End Get
            Set(value As Integer)
                m_bpoisonturns_blue = value
            End Set
        End Property

        Public Sub IncreaseNumBadPoison_Blue()
            m_bpoisonturns_blue += 1
        End Sub

        Public Property Red_NumBadPoison As Integer
            Get
                Return m_bpoisonturns_red
            End Get
            Set(value As Integer)
                m_bpoisonturns_red = value
            End Set
        End Property

        Public Sub IncreaseNumBadPoison_Red()
            m_bpoisonturns_red += 1
        End Sub

        Public Property Blue_NumConfused As Integer
            Get
                Return m_confuseturns_blue
            End Get
            Set(value As Integer)
                m_confuseturns_blue = value
            End Set
        End Property

        Public Sub IncreaseConfuseTurns_Blue()
            m_confuseturns_blue += 1
        End Sub

        Public Property Red_NumConfused As Integer
            Get
                Return m_confuseturns_red
            End Get
            Set(value As Integer)
                m_confuseturns_red = value
            End Set
        End Property

        Public Sub IncreaseConfuseTurns_Red()
            m_confuseturns_red += 1
        End Sub

        Public Property Last_Fainted() As String
            Get
                Return m_last_fainted
            End Get
            Set(value As String)
                m_last_fainted = value
            End Set
        End Property

        Public Function IsBlueFainted() As Boolean
            Dim hasallbluefainted As Boolean = False
            Dim my_enumerator As List(Of Pokemon).Enumerator = Me.Get_TeamBlue().Get_Team("blue").GetEnumerator
            my_enumerator.MoveNext() REM initialize the enumerator
            For i As Integer = 1 To Me.Get_TeamBlue().Get_Team("blue").Count Step 1
                If Not my_enumerator.Current.HP <= 0 Then
                    Return False
                End If
                my_enumerator.MoveNext()
            Next
            hasallbluefainted = True
            my_enumerator.Dispose()
            Return hasallbluefainted
        End Function

        Public Function IsRedFainted() As Boolean
            Dim hasallredfainted As Boolean = False
            Dim my_enumerator As List(Of Pokemon).Enumerator = Me.Get_TeamRed().Get_Team("red").GetEnumerator
            my_enumerator.MoveNext() REM initialize the enumerator
            For i As Integer = 1 To Me.Get_TeamRed().Get_Team("red").Count Step 1
                If Not my_enumerator.Current.HP <= 0 Then
                    Return False
                End If
                my_enumerator.MoveNext()
            Next
            hasallredfainted = True
            my_enumerator.Dispose()
            Return hasallredfainted
        End Function

        Public Function NumBlueFainted() As Integer
            Dim num_fainted As Integer = 0
            Dim my_enumerator As List(Of Pokemon).Enumerator = Me.Get_TeamBlue().Get_Team("blue").GetEnumerator
            my_enumerator.MoveNext()
            For i As Integer = 0 To Me.Get_TeamBlue().Get_Team("blue").Count Step 1
                If my_enumerator.Current.HP <= 0 Then
                    num_fainted += 1
                End If
                my_enumerator.MoveNext()
            Next
            my_enumerator.Dispose()
            Return num_fainted
        End Function

        Public Function NumRedFainted() As Integer
            Dim num_fainted As Integer = 0
            Dim my_enumerator As List(Of Pokemon).Enumerator = Me.Get_TeamRed().Get_Team("red").GetEnumerator
            my_enumerator.MoveNext()
            For i As Integer = 0 To Me.Get_TeamRed().Get_Team("red").Count Step 1
                If my_enumerator.Current.HP <= 0 Then
                    num_fainted += 1
                End If
                my_enumerator.MoveNext()
            Next
            my_enumerator.Dispose()
            Return num_fainted
        End Function

        Public Sub AddTo_CurrentBattling_Blue(ByVal battle_pokemon As Pokemon)
            Me.CurrentBattlingBlue.Add(battle_pokemon)
        End Sub

        Public Sub AddTo_CurrentBattling_Red(ByVal battle_pokemon As Pokemon)
            Me.CurrentBattlingRed.Add(battle_pokemon)
        End Sub

        ''' <summary>
        ''' Gets the health status in colour of the current battling Pokemon of Blue.
        ''' red -> less than 20%
        ''' yellow -> between 20 and 50%
        ''' green -> greater than 50%
        ''' </summary>
        ''' <returns>A string stating, "red," "yellow," and "green"</returns>
        ''' <remarks></remarks>
        Public Function Get_HealthStatusofBlue() As String
            Dim currentbattling_hp As Integer = Me.CurrentBattlingBlue.First.HP
            Dim original_hp As Integer = Form1.Get_PokemonDictionary.Get_Pokemon(Me.CurrentBattlingBlue.First.Name).HP

            Dim percent As Double = currentbattling_hp / original_hp
            If percent <= 0.5 And percent >= 0.2 Then
                Return "yellow"
            ElseIf percent < 0.2 Then
                Return "red"
            Else
                Return "green"
            End If
        End Function

        ''' <summary>
        ''' Gets the health status in colour of the current battling Pokemon of Red.
        ''' red -> less than 20%
        ''' yellow -> between 20 and 50%
        ''' green -> greater than 50%
        ''' </summary>
        ''' <returns>A string stating, "red," "yellow," and "green"</returns>
        ''' <remarks></remarks>
        Public Function Get_HealthStatusofRed() As String
            Dim currentbattling_hp As Integer = Me.CurrentBattlingRed.First.HP
            Dim original_hp As Integer = Form1.Get_PokemonDictionary.Get_Pokemon(Me.CurrentBattlingRed.First.Name).HP

            Dim percent As Double = currentbattling_hp / original_hp
            If percent <= 0.5 And percent >= 0.2 Then
                Return "yellow"
            ElseIf percent < 0.2 Then
                Return "red"
            Else
                Return "green"
            End If
        End Function

        ''' <summary>
        ''' Returns the health status, in colour, of the pokemon
        ''' red -> less than 20%
        ''' yellow -> between 20 and 50%
        ''' green -> greater than 50%
        ''' </summary>
        ''' <param name="pokemon">The pokemon to evaluate</param>
        ''' <returns>A string stating, "red," "yellow," and "green"</returns>
        ''' <remarks></remarks>
        Public Function Get_HealthStatusofPokemon(ByVal pokemon As Pokemon) As String
            Dim pokemon_HP As Integer = pokemon.HP
            Dim original_HP As Integer = Form1.Get_PokemonDictionary.Get_Pokemon(pokemon.Name).HP

            Dim percent As Double = pokemon_HP / original_HP
            If percent <= 0.5 And percent >= 0.2 Then
                Return "yellow"
            ElseIf percent < 0.2 Then
                Return "red"
            Else
                Return "green"
            End If
        End Function

        ''' <summary>
        ''' Increases the count for toxic and sleep counters
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ManageTurns()
            If Not Me.CurrentBattlingBlue.Count = 0 Then
                If Me.CurrentBattlingBlue.First.Status_Condition = Constants.StatusCondition.badly_poisoned Then
                    Me.IncreaseNumBadPoison_Blue()
                End If
                If Me.CurrentBattlingBlue.First.Status_Condition = PBP.Constants.StatusCondition.sleep Then
                    Me.IncreaseNumSleep_Blue()
                End If
                If Me.CurrentBattlingBlue.First.Other_Status_Condition = Constants.StatusCondition.confused Then
                    Me.IncreaseConfuseTurns_Blue()
                End If
                If Me.CurrentBattlingBlue.First.Status_Condition = Constants.StatusCondition.none Then
                    REM reset some counters
                    If Me.Blue_NumSleep > 0 Then
                        Me.Blue_NumSleep = 0
                    End If
                    If Me.Blue_NumBadPoison > 0 Then
                        Me.Blue_NumBadPoison = 0
                    End If
                    If Me.Blue_NumConfused > 0 Then
                        Me.Blue_NumConfused = 0
                    End If
                End If
            End If
            If Not Me.CurrentBattlingRed.Count = 0 Then
                If Me.CurrentBattlingRed.First.Status_Condition = Constants.StatusCondition.badly_poisoned Then
                    Me.IncreaseNumBadPoison_Red()
                End If
                If Me.CurrentBattlingRed.First.Status_Condition = Constants.StatusCondition.sleep Then
                    Me.IncreaseNumSleep_Red()
                End If
                If Me.CurrentBattlingRed.First.Other_Status_Condition = Constants.StatusCondition.confused Then
                    Me.IncreaseConfuseTurns_Red()
                End If
                If Me.CurrentBattlingRed.First.Status_Condition = Constants.StatusCondition.none Then
                    REM reset some counters
                    If Me.Red_NumSleep > 0 Then
                        Me.Red_NumSleep = 0
                    End If
                    If Me.Red_NumBadPoison > 0 Then
                        Me.Red_NumBadPoison = 0
                    End If
                    If Me.Red_NumConfused > 0 Then
                        Me.Red_NumConfused = 0
                    End If
                End If
            End If

        End Sub

        Public Sub Clear()
            REM clear out the current battling pokemon
            CurrentBattlingBlue.Clear()
            CurrentBattlingRed.Clear()

            REM clear out the teams
            Me.Get_TeamBlue().Get_Team("blue").Clear()
            Me.Get_TeamRed().Get_Team("red").Clear()

            REM clear out the stats
            Me.Turn_Number = 0
            Me.Last_Fainted = ""
            Me.Current_Attacker = ""
        End Sub


        Public Overrides Function Clone() As Object
            'Dim freshbattlearena = DirectCast(MyBase.Clone(), Pokemon_Arena)
            'freshbattlearena.Last_Fainted = Me.Last_Fainted
            'freshbattlearena.Red_NumSleep = Me.Red_NumSleep
            'freshbattlearena.Blue_NumSleep = Me.Blue_NumSleep
            'freshbattlearena.Blue_NumBadPoison = Me.Blue_NumBadPoison
            'freshbattlearena.Red_NumBadPoison = Me.Red_NumBadPoison
            'Return freshbattlearena
            Dim freshbattlearena As New Pokemon_Arena(Me)
            Return freshbattlearena
        End Function
    End Class
End Namespace
