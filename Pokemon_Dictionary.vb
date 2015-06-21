Imports System
Imports System.Collections

Public Class Pokemon_Dictionary
    Private pokemon_dictionary As New Dictionary(Of String, Pokemon)

    Public Function IsPokemonInDictionary(ByVal poke_name As String) As Boolean
        If pokemon_dictionary.ContainsKey(poke_name) = True Then
            Return True
        Else
            Return False
        End If
    End Function


    Public Function Get_Pokemon(ByVal poke_name As String) As Pokemon
        Dim toreturn_pokemon As Pokemon = Nothing 'Hopefully this will be alright
        If pokemon_dictionary.TryGetValue(poke_name, toreturn_pokemon) = True Then
            Return toreturn_pokemon
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Inserts Pokemon into the pokemon_dictionary
    ''' </summary>
    ''' <param name="poke_name"></param>
    ''' <param name="the_pokemon"></param>
    ''' <remarks>The function does not query the database. The querying is left to the user.</remarks>
    Public Sub Add_Pokemon(ByVal poke_name As String, ByVal the_pokemon As Pokemon)
        If pokemon_dictionary.ContainsKey(poke_name) = True Then
            Return
        Else
            pokemon_dictionary.Add(poke_name, the_pokemon)
        End If
    End Sub

    Public Sub Process_PokemonInfo(ByVal worker As System.ComponentModel.BackgroundWorker,
                                ByVal e As System.ComponentModel.DoWorkEventArgs)

    End Sub
End Class
