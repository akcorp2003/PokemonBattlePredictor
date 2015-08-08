Imports System
Imports System.Collections
Imports System.Net
Imports System.IO

Public Class Pokemon_Dictionary
    Private pokemon_dictionary As New Dictionary(Of String, Pokemon)

    Public Function Get_Dictionary() As Dictionary(Of String, Pokemon)
        Return pokemon_dictionary
    End Function

    Public Function IsPokemonInDictionary(ByVal poke_name As String) As Boolean
        If pokemon_dictionary.ContainsKey(poke_name.ToLower) = True Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Fetches the Pokemon from the dictionary. poke_name should all be in lowercase and spaces trimmed.
    ''' No quotations.
    ''' </summary>
    ''' <param name="poke_name"></param>
    ''' <returns>A Pokemon object</returns>
    ''' <remarks>poke_name should be in lowercase and spaces removed. No quotations</remarks>
    Public Function Get_Pokemon(ByVal poke_name As String) As Pokemon
        Dim toreturn_pokemon As New Pokemon 'Hopefully this will be alright
        poke_name = Constants.Get_FormattedString(poke_name)
        If pokemon_dictionary.TryGetValue(poke_name, toreturn_pokemon) = True Then
            Return toreturn_pokemon
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Inserts Pokemon into the pokemon_dictionary. No need to format poke_name. The function will handle it.
    ''' </summary>
    ''' <param name="poke_name"></param>
    ''' <param name="the_pokemon"></param>
    ''' <remarks>The function does not query the database. The querying is left to the user. 
    ''' No need to format poke_name as the function will clean it up.</remarks>
    Public Sub Add_Pokemon(ByVal poke_name As String, ByVal the_pokemon As Pokemon)
        poke_name = poke_name.Trim()
        poke_name = poke_name.Trim("""")
        poke_name = poke_name.ToLower()
        If pokemon_dictionary.ContainsKey(poke_name) = True Then
            Return
        Else
            pokemon_dictionary.Add(poke_name, the_pokemon)
        End If
    End Sub

    ''' <summary>
    ''' Queries the master database using URL. No need to format poke_name. The function will format it properly for you.
    ''' </summary>
    ''' <param name="poke_name"></param>
    ''' <remarks>This function will probably be used more than its other overloaded counterpart. 
    ''' This function handles all the URL stuff and parsing.</remarks>
    Public Function Add_Pokemon(ByVal poke_name As String) As Integer
        Dim base_url As String = "http://pokeapi.co/"

        REM Because of our formatting conventions, we need to have quotations around pokemon name
        Dim proper_poke_name As String = poke_name.Trim()
        Dim proper_poke_name1 As String = poke_name.Trim()
        proper_poke_name1 = proper_poke_name1.ToLower()

        proper_poke_name = """" + proper_poke_name + """"
        Dim api_url As String = Form1.Get_ResourceURIDictionary.Get_PokemonURI(proper_poke_name)
        Dim fullapi_url As String = base_url + api_url

        Dim pokemonline As String = Form1.RequestLine(fullapi_url)
        Dim new_pokemon As Pokemon = Process_PokemonLine(pokemonline, poke_name)
        If Not new_pokemon Is Nothing Then
            pokemon_dictionary.Add(proper_poke_name1, new_pokemon)
            Return 0
        Else
            Return -1
        End If


    End Function

    Public Sub Process_PokemonInfo(ByVal worker As System.ComponentModel.BackgroundWorker,
                                ByVal e As System.ComponentModel.DoWorkEventArgs)

    End Sub

    Public Function Process_PokemonLine(ByVal line As String, ByVal pokemon_name As String) As Pokemon
        REM we will write the line out to a file according to space
        REM and read in the file and build the Pokemon that way

        'Dim master_filewriter As System.IO.StreamWriter = Nothing
        'Dim master_filewriter2 As System.IO.StreamWriter = Nothing
        'Dim temp_filename As String = "pokemon.txt"

        'If My.Computer.FileSystem.FileExists(temp_filename) Then
        '    Try
        '        My.Computer.FileSystem.DeleteFile(temp_filename)
        '    Catch ex As Exception
        '        MessageBox.Show("Something went awry when trying to delete the temporary pokemon.txt. Aborting this operation.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    End Try
        'End If

        'Try
        '    master_filewriter = My.Computer.FileSystem.OpenTextFileWriter(temp_filename, False)
        'Catch ex As Exception
        '    MessageBox.Show("Something went awry when trying to open a writer for pokemon.txt. Aborting this operation.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        'End Try

        'Dim temp_filename2 As String = "pokemon_formatted.txt"
        'If Not master_filewriter Is Nothing Then
        '    REM My.Computer.FileSystem.WriteAllText(temp_filename, line, False)
        '    master_filewriter.WriteLine(line)
        '    master_filewriter.Close()

        '    'BEGIN SECOND PHASE OF PARSING


        '    Try
        '        master_filewriter2 = My.Computer.FileSystem.OpenTextFileWriter(temp_filename2, False)
        '    Catch ex As Exception
        '        MessageBox.Show("Something went awry when trying to open a writer2 for pokemon_formatted.txt. Aborting this operation.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '    End Try
        '    Try
        '        master_filereader = My.Computer.FileSystem.OpenTextFileReader(temp_filename)
        '    Catch ex As Exception
        '        MessageBox.Show("Something went awry when trying to open a reader for pokemon.txt. Aborting this operation.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '        Return
        '    End Try

        '    While master_filereader.EndOfStream() = False
        '        Dim characterinString(0) As Char
        '        master_filereader.Read(characterinString, 0, 1)
        '        Dim to_write_character As String = New String(characterinString)
        '        If to_write_character = " " Then
        '            REM write the character to file
        '            master_filewriter2.Write(Environment.NewLine)
        '        ElseIf Not to_write_character = "{" And Not to_write_character = ":" And Not to_write_character = "}" And
        '            Not to_write_character = "," And Not to_write_character = "[" And Not to_write_character = "]" Then
        '            master_filewriter2.Write(to_write_character)
        '        End If
        '    End While

        'End If

        'master_filereader.Close()
        'master_filewriter2.Close()
        Dim temp_filename2 As String = "pokemon_formatted.txt"

        Form1.FormatFile(line, "pokemon.txt", "pokemon_formatted.txt")

        Dim master_filereader As System.IO.StreamReader = Nothing


        master_filereader = My.Computer.FileSystem.OpenTextFileReader(temp_filename2)
        Dim new_pokemon As New Pokemon

        'Set up flags
        Dim egg_flag As Boolean = False
        Dim quotelevel_flag As Boolean = False


        Dim abilities_flag As Boolean = False
        Dim atk_flag As Boolean = False
        Dim def_flag As Boolean = False
        Dim spatk_flag As Boolean = False
        Dim spdef_flag As Boolean = False
        Dim hp_flag As Boolean = False
        Dim spd_flag As Boolean = False
        Dim move_flag As Boolean = False
        Dim type_flag As Boolean = False

        REM because there is no peek function, (and choosing not to implement a class that peeks for me)
        REM for the moves, abilities, and types having multiple lines
        REM flags the program to hold onto the read character
        Dim dont_read_next_line As Boolean = False

        Dim listofkeywords As New List(Of String) From {"""abilities""", """attack""", """defense""",
                                                        """hp""", """sp_atk""", """sp_def""", """speed""",
                                                        """types""", """moves""", """national_id"""}


        new_pokemon.Name = pokemon_name.Trim()

        Dim poke_line_copy As String = ""
        Dim poke_line As String = ""
        While master_filereader.EndOfStream() = False


            REM check if a while loop has a line consumed and ready to be processed
            If Not dont_read_next_line Then
                poke_line = master_filereader.ReadLine()
            Else
                dont_read_next_line = False
            End If

            poke_line_copy = poke_line

            If poke_line = """abilities""" Then
                abilities_flag = True

            ElseIf abilities_flag = True And Not listofkeywords.Contains(poke_line) Then
                REM read in the line
                If poke_line = """name""" Then


                    While poke_line = """name""" Or poke_line = """resource_uri"""
                        Dim ability As New Ability_Info
                        REM read in the next 3 lines since we know what information they contain
                        poke_line = master_filereader.ReadLine() REM this will be the line of the actual ability name
                        ability.Name = poke_line
                        poke_line = master_filereader.ReadLine() REM this will be the line resource_uri
                        If Not poke_line = """resource_uri""" Then
                            MessageBox.Show("The formatting of resource_uri in pokemon_formatted.txt is messed up!! Aborting", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return Nothing
                        End If
                        poke_line = master_filereader.ReadLine() REM this will be the actual resource_uri
                        ability.URI = poke_line

                        'Dim abilitylist As New List(Of Ability_Info)
                        'If new_pokemon.Ability Is Nothing Then
                        '    new_pokemon.Ability = abilitylist
                        'End If
                        new_pokemon.Ability.Add(ability)

                        poke_line = master_filereader.ReadLine() REM fetch next line
                    End While

                    dont_read_next_line = True
                    abilities_flag = False

                Else
                    MessageBox.Show("The formatting of resource_uri in pokemon_formatted.txt is messed up!! Aborting", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return Nothing
                End If

            ElseIf poke_line = """attack""" Then
                atk_flag = True
                abilities_flag = False
            ElseIf atk_flag = True And Not listofkeywords.Contains(poke_line) Then
                REM we are definitely reading a value
                new_pokemon.ATK = Convert.ToInt32(poke_line)
                atk_flag = False

            ElseIf poke_line = """defense""" Then
                def_flag = True
            ElseIf def_flag = True And Not listofkeywords.Contains(poke_line) Then
                def_flag = False
                new_pokemon.DEF = Convert.ToInt32(poke_line)

            ElseIf poke_line = """hp""" Then
                hp_flag = True
            ElseIf hp_flag = True And Not listofkeywords.Contains(poke_line) Then
                hp_flag = False
                new_pokemon.HP = Convert.ToInt32(poke_line)

            ElseIf poke_line = """moves""" Then
                move_flag = True
            ElseIf move_flag = True And Not listofkeywords.Contains(poke_line) Then

                While Not listofkeywords.Contains(poke_line)
                    Dim package As New Move_Package
                    If poke_line = """name""" Then
                        REM we can consume the next 3 lines for the move name and URI
                        poke_line = master_filereader.ReadLine() REM this line will contain the name of the move
                        package.Name = poke_line
                        poke_line = master_filereader.ReadLine() REM this line will contain the word "resource_uri"

                        REM this is the next section after the abilities
                        REM as long as their format does not change, this should be valid
                        If poke_line = """national_id""" Then
                            Exit While
                        End If

                        If Not poke_line = """resource_uri""" Then
                            MessageBox.Show("The formatting of resource_uri in pokemon_formatted.txt is messed up!! Aborting", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return Nothing
                        End If
                        poke_line = master_filereader.ReadLine() REM this line will contain the actual resource URI
                        package.URI = poke_line

                        REM send it off to the function to insert
                        package.InsertMove(new_pokemon, package)
                    End If
                    poke_line = master_filereader.ReadLine()
                End While
                move_flag = False

            ElseIf poke_line = """sp_atk""" Then
                spatk_flag = True
            ElseIf spatk_flag = True And Not listofkeywords.Contains(poke_line) Then
                spatk_flag = False
                new_pokemon.Sp_ATK = Convert.ToInt32(poke_line)

            ElseIf poke_line = """sp_def""" Then
                spdef_flag = True
            ElseIf spdef_flag = True And Not listofkeywords.Contains(poke_line) Then
                spdef_flag = False
                new_pokemon.Sp_DEF = Convert.ToInt32(poke_line)

            ElseIf poke_line = """speed""" Then
                spd_flag = True
            ElseIf spd_flag = True And Not listofkeywords.Contains(poke_line) Then
                spd_flag = False
                new_pokemon.SPD = Convert.ToInt32(poke_line)

            ElseIf poke_line = """types""" Then
                type_flag = True
            ElseIf type_flag = True And Not listofkeywords.Contains(poke_line) Then
                While Not master_filereader.EndOfStream()
                    If poke_line = """name""" Then
                        REM consume the next line, knowing it's the name of the type
                        poke_line = master_filereader.ReadLine()
                        new_pokemon.Types.Add(poke_line)

                        poke_line = master_filereader.ReadLine()
                    ElseIf poke_line = """weight""" Then
                        Exit While
                    Else REM continue reading, we will encounter the resource_uri but for the types, we will ignore them
                        poke_line = master_filereader.ReadLine()
                    End If
                End While
                type_flag = False

            Else
                Continue While
            End If
        End While
        master_filereader.Close()

        Return new_pokemon
    End Function
End Class
