Imports System
Imports System.Collections

Public Class Move_Dictionary
    Private move_dictionary As New Dictionary(Of String, Move_Info)

    Public Function IsMoveInDictionary(ByVal move As String) As Boolean
        If move_dictionary.ContainsKey(move) = True Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function Get_Move(ByVal move As String) As Move_Info
        Dim toreturn_move As Move_Info = Nothing
        If move_dictionary.TryGetValue(move, toreturn_move) = True Then
            Return toreturn_move
        Else
            Return Nothing
        End If
    End Function

    Public Sub Add_Move(ByVal move As String, ByVal move_info As Move_Info)
        If move_dictionary.ContainsKey(move) = True Then
            Return
        Else
            move_dictionary.Add(move, move_info)
            Return
        End If
    End Sub
End Class

Public Class Move_Info
    Private m_name As String
    Private m_accuracy As Integer
    Private m_type As String
    Private m_power As Integer
    Private m_pp As Integer

    Public Property Name As String
        Get
            Return m_name
        End Get
        Set(value As String)
            m_name = value
        End Set
    End Property

    Public Property Accuracy As Integer
        Get
            Return m_accuracy
        End Get
        Set(value As Integer)
            m_accuracy = value
        End Set
    End Property

    Public Property Type As String
        Get
            Return m_type
        End Get
        Set(value As String)
            m_type = value
        End Set
    End Property

    Public Property Power As Integer
        Get
            Return m_power
        End Get
        Set(value As Integer)
            m_power = value
        End Set
    End Property

    Public Property PP As Integer
        Get
            Return m_pp
        End Get
        Set(value As Integer)
            m_pp = value
        End Set
    End Property
End Class
