Imports System
Imports System.Collections

Public Class Ability_Dictionary
    Private ability_dictionary As New Dictionary(Of String, Ability_Info)

    Public Function IsAbilityInDictionary(ByVal ability As String) As Boolean
        If ability_dictionary.ContainsKey(ability) = True Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function Get_Ability(ByVal ability As String) As Ability_Info
        Dim toreturn_ability As Ability_Info = Nothing
        If ability_dictionary.TryGetValue(ability, toreturn_ability) = True Then
            Return toreturn_ability
        Else
            Return Nothing
        End If
    End Function

    Public Sub Add_Ability(ByVal ability As String, ByVal ability_info As Ability_Info)
        If ability_dictionary.ContainsKey(ability) = True Then
            Return
        Else
            ability_dictionary.Add(ability, ability_info)
        End If
    End Sub
End Class

Public Class Ability_Info

End Class
