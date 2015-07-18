Imports System
Imports System.Collections
Imports System.Collections.Generic

Public Class Arena
    Dim m_Team_Blue As New Pokemon_Team
    Dim m_Team_Red As New Pokemon_Team
    Dim m_turn_number As Integer = 0
<<<<<<< HEAD
    Dim m_curr_attacker As String
=======
>>>>>>> master

    Dim currentbattlingpokemon_blue As New List(Of Pokemon)
    Dim currentbattlingpokemon_red As New List(Of Pokemon)

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

<<<<<<< HEAD
    Public Property Current_Attacker() As String
        Get
            Return m_curr_attacker
        End Get
        Set(value As String)
            m_curr_attacker = value
        End Set
    End Property

=======
>>>>>>> master
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

End Class

Public Class Pokemon_Arena
    Inherits Arena
    Implements System.ICloneable

<<<<<<< HEAD
    Dim m_last_fainted As String
    Dim m_sleepturns_blue As Integer
    Dim m_sleepturns_red As Integer
    Dim m_bpoisonturns_blue As Integer
    Dim m_bpoisonturns_red As Integer

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

    Public Property Last_Fainted() As String
        Get
            Return m_last_fainted
        End Get
        Set(value As String)
            m_last_fainted = value
        End Set
    End Property

=======
>>>>>>> master
    Public Function IsBlueFainted() As Boolean
        Dim hasallbluefainted As Boolean = False
        Dim my_enumerator As List(Of Pokemon).Enumerator = Me.Get_TeamBlue().Get_Team("blue").GetEnumerator
        my_enumerator.MoveNext() REM initialize the enumerator
<<<<<<< HEAD
        For i As Integer = 1 To Me.Get_TeamBlue().Get_Team("blue").Count Step 1
            If Not my_enumerator.Current.HP <= 0 Then
=======
        For i As Integer = 0 To Me.Get_TeamBlue().Get_Team("blue").Count Step 1
            If Not my_enumerator.Current.HP = 0 Then
>>>>>>> master
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
<<<<<<< HEAD
        For i As Integer = 1 To Me.Get_TeamRed().Get_Team("red").Count Step 1
            If Not my_enumerator.Current.HP <= 0 Then
=======
        For i As Integer = 0 To Me.Get_TeamRed().Get_Team("red").Count Step 1
            If Not my_enumerator.Current.HP = 0 Then
>>>>>>> master
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
<<<<<<< HEAD
            If my_enumerator.Current.HP <= 0 Then
=======
            If my_enumerator.Current.HP = 0 Then
>>>>>>> master
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
<<<<<<< HEAD
            If my_enumerator.Current.HP <= 0 Then
=======
            If my_enumerator.Current.HP = 0 Then
>>>>>>> master
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

<<<<<<< HEAD
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
    ''' Increases the count for toxic and sleep counters
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ManageTurns()
        If Me.CurrentBattlingBlue.First.Status_Condition = Constants.StatusCondition.badly_poisoned Then
            Me.IncreaseNumBadPoison_Blue()
        End If
        If Me.CurrentBattlingRed.First.Status_Condition = Constants.StatusCondition.badly_poisoned Then
            Me.IncreaseNumBadPoison_Red()
        End If

        If Me.CurrentBattlingBlue.First.Status_Condition = Constants.StatusCondition.sleep Then
            Me.IncreaseNumSleep_Blue()
        End If
        If Me.CurrentBattlingRed.First.Status_Condition = Constants.StatusCondition.sleep Then
            Me.IncreaseNumSleep_Red()
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

=======
>>>>>>> master

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim freshbattlearena As New Pokemon_Arena
        freshbattlearena.Team_Blue = Me.Team_Blue.Clone()
        freshbattlearena.Team_Red = Me.Team_Red.Clone()
        freshbattlearena.Turn_Number = Me.Turn_Number
<<<<<<< HEAD
        freshbattlearena.Red_NumSleep = Me.Red_NumSleep
        freshbattlearena.Blue_NumSleep = Me.Blue_NumSleep
=======

>>>>>>> master
        Return freshbattlearena
    End Function
End Class
