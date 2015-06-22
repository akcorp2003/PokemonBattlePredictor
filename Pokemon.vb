Imports System.Collections

Public Class Pokemon
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
End Class
