Imports System
Imports System.Collections
Imports PokemonBattlePredictor
Imports PokemonBattlePredictor.PBP
Imports PokemonBattlePredictor.PBP.InfoBlocks

Namespace PBP.Dictionary
    Public Class Ability_Dictionary
        Private m_ability_dictionary As New Dictionary(Of String, Ability_Info)

        Public Function Get_AbilityDictionary() As Dictionary(Of String, Ability_Info)
            Return m_ability_dictionary
        End Function

        Public Function IsAbilityInDictionary(ByVal ability As String) As Boolean
            If m_ability_dictionary.ContainsKey(ability.ToLower) = True Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function Get_Ability(ByVal ability As String) As Ability_Info
            Dim toreturn_ability As Ability_Info = Nothing
            If m_ability_dictionary.TryGetValue(ability, toreturn_ability) = True Then
                Return toreturn_ability
            Else
                Return Nothing
            End If
        End Function

        Public Sub Add_Ability(ByVal ability As String, ByVal ability_info As Ability_Info)
            If m_ability_dictionary.ContainsKey(ability) = True Then
                Return
            Else
                m_ability_dictionary.Add(ability, ability_info)
            End If
        End Sub
    End Class
End Namespace

REM this class will be developed in the future. We will add more info about the moves
Namespace PBP.InfoBlocks
    Public Class Ability_Info
        Implements ICloneable

        Private m_name As String
        Private m_uri As String

        Public Property Name As String
            Get
                Return m_name
            End Get
            Set(value As String)
                m_name = value
            End Set
        End Property

        Public Property URI As String
            Get
                Return m_uri
            End Get
            Set(value As String)
                m_uri = value
            End Set
        End Property

        Public Function Clone() As Object Implements ICloneable.Clone
            Dim freshabilityinfo As New Ability_Info
            freshabilityinfo.Name = Me.Name
            freshabilityinfo.URI = Me.URI
            Return freshabilityinfo
        End Function
    End Class
End Namespace
