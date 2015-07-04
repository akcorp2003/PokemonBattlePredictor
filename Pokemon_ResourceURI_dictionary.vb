Imports System
Imports System.Collections

Public Class Pokemon_ResourceURI_dictionary
    Private pokemon_resourceURI_dictionary As New Dictionary(Of String, String)

    Public Function Get_ResourceURIDictionary() As Dictionary(Of String, String)
        Return pokemon_resourceURI_dictionary
    End Function

    ''' <summary>
    ''' Checks if Pokemon and its URI is in the Dictionary
    ''' </summary>
    ''' <param name="pokemon_name"></param>
    ''' <returns>A boolean saying yes or no if the pokemon exists in the dictionary</returns>
    ''' <remarks>Kind of a useless function because if Pokemon is not in dictionary, it means
    ''' that the master database doesn't have such pokemon. So it could mean that the user typed
    ''' a non-existant pokemon, spelling error, or a brand new pokemon</remarks>
    Public Function IsPokemonInDictionary(ByVal pokemon_name As String) As Boolean
        If pokemon_resourceURI_dictionary.ContainsKey(pokemon_name.ToLower) = True Then
            Return True
        Else
            Return False
        End If
        Return False
    End Function

    ''' <summary>
    ''' Fetches the URI of the pokemon in question
    ''' </summary>
    ''' <param name="pokemon_name"></param>
    ''' <returns>String of the URI</returns>
    ''' <remarks>the user will use the URI to request the actual pokemon object from the database</remarks>
    Public Function Get_PokemonURI(ByVal pokemon_name As String) As String
        Dim uri_string As String = ""
        If pokemon_resourceURI_dictionary.TryGetValue(pokemon_name.ToLower, uri_string) = True Then
            uri_string = uri_string.Trim("""")
            Return uri_string
        Else
            Return ""
        End If
    End Function

    ''' <summary>
    ''' Adds Pokemon and its URI
    ''' </summary>
    ''' <param name="pokemon_name"></param>
    ''' <param name="pokemon_URI"></param>
    ''' <remarks></remarks>
    Public Sub Add_PokemonandURI(ByVal pokemon_name As String, ByVal pokemon_URI As String)
        If pokemon_resourceURI_dictionary.ContainsKey(pokemon_name) = True Then
            Return REM it's already in the system
        Else
            pokemon_resourceURI_dictionary.Add(pokemon_name.ToLower, pokemon_URI)
        End If
    End Sub

    Public Sub Process_Pokedex(ByVal worker As System.ComponentModel.BackgroundWorker,
                                ByVal e As System.ComponentModel.DoWorkEventArgs)
        Dim master_filereader As System.IO.StreamReader = Nothing

        If My.Computer.FileSystem.FileExists("pokedex_formatted.txt") Then
            Try
                master_filereader = My.Computer.FileSystem.OpenTextFileReader("pokedex_formatted.txt")
            Catch ex As Exception
                MessageBox.Show("Something went wrong in the URI Dictionary. Mysteriously cannot open the pokedex_formatted.txt file", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            REM Because of our formatting, we are not interested in the first 6 lines of the file
            REM Move the read head beyond these 6 lines
            For index As Integer = 0 To 5
                If master_filereader.EndOfStream = False Then
                    master_filereader.ReadLine()
                Else
                    MessageBox.Show("Uh oh! URI Dictionary has detected a messed up pokedex_formatted file. Aborting! Please make sure the file is properly formatted.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
            Next

            'Packaging variables:
            Dim poke_name As String = ""
            Dim URI As String = ""
            'Reading Flags
            Dim lastline_name As Boolean = False
            Dim lastline_URI As Boolean = False

            While master_filereader.EndOfStream() = False
                Dim line As String = ""
                line = master_filereader.ReadLine()
                If line = """pokemon""" Then REM same as if the line equals to "pokemon" (including quotes)
                    Continue While
                ElseIf line = """name""" Then
                    lastline_name = True
                ElseIf lastline_name = True Then
                    lastline_name = False
                    poke_name = line
                ElseIf line = """resource_uri""" Then
                    lastline_URI = True
                ElseIf line = """/api/v1/pokedex/1/""" Then REM we have reached the URI for accessing the Pokedex. No need for it.
                    lastline_URI = False
                    Exit While
                ElseIf lastline_URI = True Then
                    lastline_URI = False
                    URI = line
                    REM everything is ready so add to dictionary!!
                    Me.Add_PokemonandURI(poke_name, URI)
                Else
                    Continue While
                End If

            End While

            REM check to make sure all flags are set to false. If not, ask the user to see if Pokemon were added properly.
            If Not lastline_name = False Or Not lastline_URI = False Then
                MessageBox.Show("Uh oh! The Process_Dictionary function may not have read all the Pokemon and their URI correctly. Make sure that you can query Pyroar", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Else
            MessageBox.Show("The ResourceURI Dictionary cannot locate the formatted pokedex. Aborting this operation. Please make sure the file is there.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    REM NOTE: there is no delete from the dictionary option. It's not recommended so we won't provide it
    REM NOTE: we do not provide the Dictionary object to the user. We don't want the user to meddle with it
End Class
