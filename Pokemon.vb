Imports System.Collections
Imports System.Reflection


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
    Private m_stage As Integer = 0
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
        freshpokemon.Moves = Me.Moves.Select(Function(x) x.Clone()).Cast(Of String).ToList()
        freshpokemon.Types = Me.Types.Select(Function(x) x.Clone()).Cast(Of String).ToList()
        freshpokemon.Ability = Me.Ability.Select(Function(x) x.Clone()).Cast(Of Ability_Info).ToList()
        freshpokemon.Moves_For_Battle = Me.Moves_For_Battle.Select(Function(x) x.Clone()).Cast(Of Move_Info).ToList()

        Return freshpokemon
    End Function
End Class
