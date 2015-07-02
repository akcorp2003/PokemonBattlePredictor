Imports System
Imports System.Net
Imports System.IO

Public Class Dex_Writer

    Public Sub Write_to_Pokedex(ByVal worker As System.ComponentModel.BackgroundWorker,
                                ByVal e As System.ComponentModel.DoWorkEventArgs)
        REM Query the Master Server
        Dim URL As String
        URL = "http://pokeapi.co/api/v1/pokedex/1/"

        Dim GETURL As WebRequest
        GETURL = WebRequest.Create(URL)

        Dim my_stream As Stream
        my_stream = GETURL.GetResponse.GetResponseStream()

        Dim stream_Reader As New StreamReader(my_stream)

        Dim Line As String = ""

        REM set up the filename and things
        Dim filename As String = "pokedex.txt"
        Try
            My.Computer.FileSystem.DeleteFile(filename)
        Catch ex As System.IO.FileNotFoundException
            REM yup, don't do anything
        End Try

        Do While Not Line Is Nothing
            Line = stream_Reader.ReadLine
            If Not Line Is Nothing Then
                REM Write the line to file
                My.Computer.FileSystem.WriteAllText(filename, Line, True)
            End If
        Loop

        stream_Reader.Close()

        REM prepare to format the Dex we just received
        Dim dex_formatter As New Dex_formatter
        dex_formatter.Formatting_Filename = filename
        dex_formatter.Format_Dex(dex_formatter.Formatting_Filename)

    End Sub

    Public Sub PrintPokemonInfo(ByVal calling_form As Form1)
        Dim pokemonfile As String = "pokemondictionary_formatted.txt"

        If My.Computer.FileSystem.FileExists(pokemonfile) = True Then
            Try
                My.Computer.FileSystem.DeleteFile(pokemonfile)
            Catch ex As Exception
                MessageBox.Show("Mysteriously unable to delete pokemondictionary_formatted.txt. Try checking if you have it open", _
                                "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End Try
        End If


        Dim filewriter As StreamWriter = Nothing
        Try
            filewriter = My.Computer.FileSystem.OpenTextFileWriter(pokemonfile, False)
        Catch ex As Exception
            MessageBox.Show("Could not open pokemondictionary_formatted.txt for writing...", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        REM begin taking information and writing it all out
        Dim poke_dictionary As Dictionary(Of String, Pokemon) = calling_form.Get_PokemonDictionary.Get_Dictionary
        Dim poke_enum As New Dictionary(Of String, Pokemon).Enumerator
        poke_enum = poke_dictionary.GetEnumerator()
        poke_enum.MoveNext()

        Dim i As Integer = 0
        While i < poke_dictionary.Count
            filewriter.WriteLine("POKEMON")

            filewriter.WriteLine("NAME")
            filewriter.WriteLine(poke_enum.Current.Value.Name)
            filewriter.WriteLine("ATK")
            filewriter.WriteLine(Convert.ToString(poke_enum.Current.Value.ATK))
            filewriter.WriteLine("DEF")
            filewriter.WriteLine(Convert.ToString(poke_enum.Current.Value.DEF))
            filewriter.WriteLine("SP_ATK")
            filewriter.WriteLine(Convert.ToString(poke_enum.Current.Value.Sp_ATK))
            filewriter.WriteLine("SP_DEF")
            filewriter.WriteLine(Convert.ToString(poke_enum.Current.Value.Sp_DEF))
            filewriter.WriteLine("HP")
            filewriter.WriteLine(Convert.ToString(poke_enum.Current.Value.HP))
            filewriter.WriteLine("SPD")
            filewriter.WriteLine(Convert.ToString(poke_enum.Current.Value.SPD))

            filewriter.WriteLine("BEGIN ABILITY")
            Dim num_abilities As Integer = poke_enum.Current.Value.Ability.Count
            Dim ability_enum As New List(Of Ability_Info).Enumerator
            ability_enum = poke_enum.Current.Value.Ability.GetEnumerator()
            ability_enum.MoveNext()

            Dim j As Integer = 0
            While j < num_abilities
                filewriter.WriteLine("NAME")
                filewriter.WriteLine(ability_enum.Current.Name)
                filewriter.WriteLine("URI")
                filewriter.WriteLine(ability_enum.Current.URI)
                ability_enum.MoveNext()
                j += 1
            End While
            ability_enum.Dispose()
            filewriter.WriteLine("END ABILITY")

            filewriter.WriteLine("BEGIN MOVES")
            Dim num_moves As Integer = poke_enum.Current.Value.Moves.Count
            Dim move_enum As New List(Of String).Enumerator
            Dim movelist() As String = poke_enum.Current.Value.Moves.ToArray()
            'move_enum.MoveNext()
            Dim k As Integer = 0
            While k < num_moves
                REM we are going to skip over the details. They will be detailed in another file
                filewriter.WriteLine("NAME")
                filewriter.WriteLine(movelist(k))
                'move_enum.MoveNext()
                k += 1
            End While
            move_enum.Dispose()
            filewriter.WriteLine("END MOVES")

            filewriter.WriteLine("BEGIN TYPES")
            Dim num_types As Integer = poke_enum.Current.Value.Types.Count
            Dim num_enum As New List(Of String).Enumerator
            Dim typelist() As String = poke_enum.Current.Value.Types.ToArray()
            'num_enum.MoveNext()
            Dim l As Integer = 0
            While l < num_types
                filewriter.WriteLine(typelist(l))
                'num_enum.MoveNext()
                l += 1
            End While
            num_enum.Dispose()
            filewriter.WriteLine("END TYPES")

            filewriter.WriteLine("END POKEMON")

            poke_enum.MoveNext()

            i += 1
        End While

        filewriter.Close()


    End Sub

    Public Sub PrintAbilityInfo(ByVal calling_form As Form1)
        Dim pokemonfile As String = "ability_formatted.txt"

        If My.Computer.FileSystem.FileExists(pokemonfile) = True Then
            Try
                My.Computer.FileSystem.DeleteFile(pokemonfile)
            Catch ex As Exception
                MessageBox.Show("Mysteriously unable to delete ability_formatted.txt. Try checking if you have it open", _
                                "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End Try
        End If



        Dim filewriter As StreamWriter
        Try
            filewriter = My.Computer.FileSystem.OpenTextFileWriter(pokemonfile, False)
        Catch ex As Exception
            MessageBox.Show("Could not open ability_formatted.txt for writing...", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Dim abilitydictionary As Dictionary(Of String, Ability_Info) = calling_form.Get_AbilityDictionary.Get_AbilityDictionary()
        Dim ability_enum As New Dictionary(Of String, Ability_Info).Enumerator
        ability_enum = abilitydictionary.GetEnumerator()
        ability_enum.MoveNext()
        Dim ability_count As Integer = abilitydictionary.Count

        Dim i As Integer = 0
        While i < ability_count
            filewriter.WriteLine("ABILITY")
            filewriter.WriteLine("NAME")
            filewriter.WriteLine(ability_enum.Current.Value.Name)
            filewriter.WriteLine("URI")
            filewriter.WriteLine(ability_enum.Current.Value.URI)
            filewriter.WriteLine("END ABILITY")

            ability_enum.MoveNext()

            i += 1
        End While

        filewriter.Close()

    End Sub

    Public Sub PrintMoveInfo(ByVal calling_form As Form1)
        Dim pokemonfile As String = "moves_formatted.txt"

        If My.Computer.FileSystem.FileExists(pokemonfile) = True Then
            Try
                My.Computer.FileSystem.DeleteFile(pokemonfile)
            Catch ex As Exception
                MessageBox.Show("Mysteriously unable to delete moves_formatted.txt. Try checking if you have it open", _
                                "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End Try
        End If



        Dim filewriter As StreamWriter
        Try
            filewriter = My.Computer.FileSystem.OpenTextFileWriter(pokemonfile, False)
        Catch ex As Exception
            MessageBox.Show("Could not open moves_formatted.txt for writing...", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Dim movecount As Integer = calling_form.Get_MoveDictionary.Get_MoveDictionary.Count
        Dim move_enum As Dictionary(Of String, Move_Info).Enumerator
        move_enum = calling_form.Get_MoveDictionary.Get_MoveDictionary.GetEnumerator()
        move_enum.MoveNext()

        Dim i As Integer = 0
        While i < movecount
            filewriter.WriteLine("MOVE")
            filewriter.WriteLine("NAME")
            filewriter.WriteLine(move_enum.Current.Value.Name)
            filewriter.WriteLine("ACCURACY")
            filewriter.WriteLine(Convert.ToString(move_enum.Current.Value.Accuracy))
            filewriter.WriteLine("POWER")
            filewriter.WriteLine(Convert.ToString(move_enum.Current.Value.Power))
            filewriter.WriteLine("PP")
            filewriter.WriteLine(Convert.ToString(move_enum.Current.Value.PP))
            filewriter.WriteLine("URI")
            filewriter.WriteLine(move_enum.Current.Value.URI)
            filewriter.WriteLine("SPECIAL")
            filewriter.WriteLine(Convert.ToString(move_enum.Current.Value.Is_Special))

            filewriter.WriteLine("EFFECT")
            If move_enum.Current.Value.Effect Is Nothing Then
                filewriter.WriteLine() REM write an empty string, we can fill it in in the file
            Else
                filewriter.WriteLine(move_enum.Current.Value.Effect)
            End If

            filewriter.WriteLine("TYPE")
            If move_enum.Current.Value.Type Is Nothing Then
                filewriter.WriteLine() REM write an empty string, we can fill it in in the file
            Else
                filewriter.WriteLine(move_enum.Current.Value.Type)
            End If
            filewriter.WriteLine("END MOVE")

            move_enum.MoveNext()
            i += 1
        End While

        filewriter.Close()
    End Sub


End Class

'Given a Pokedex file, it will modify/create a new file that has the file properly formatted

Public Class Dex_formatter

    Private filename_to_format As String

    Public Property Formatting_Filename() As String
        Get
            Return filename_to_format
        End Get
        Set(ByVal value As String)
            filename_to_format = value
        End Set
    End Property
   

    Public Sub Format_Dex(ByVal filename As String)

        Dim master_filereader As System.IO.StreamReader = Nothing
        Dim master_filewriter As System.IO.StreamWriter = Nothing
        Dim new_filename As String = "pokedex_formatted.txt"

        If My.Computer.FileSystem.FileExists(new_filename) Then
            Try
                REM we are going to assume the existing file is bad so delete it
                My.Computer.FileSystem.DeleteFile(new_filename)
            Catch ex As Exception
                MessageBox.Show("Something went awry when trying to delete the formatted Pokedex file. Aborting this operation.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

        End If

        Try
            master_filereader = My.Computer.FileSystem.OpenTextFileReader(filename)
        Catch ex As Exception
            MessageBox.Show("Couldn't locate the Pokedex file. Try restarting the program.")
            Return
        End Try


        Try
            master_filewriter = My.Computer.FileSystem.OpenTextFileWriter(new_filename, True)
        Catch ex As Exception
            REM do nothing for now...
            MessageBox.Show("Something really bad happened. Try making sure no other programs are using the file.")
            Return
        End Try

        If Not master_filereader Is Nothing Then
            REM we will now begin our formatting process
            REM The general format we will follow is provided on the PokeAPI page
            'ignore punctuation except for "
            'new line when seeing a space
            While master_filereader.EndOfStream() = False
                Dim characterinString(0) As Char
                master_filereader.Read(characterinString, 0, 1)
                Dim to_write_character As String = New String(characterinString) REM convert the character to a string
                REM filter out which characters to not write
                If to_write_character = " " Then
                    REM write the character to file
                    master_filewriter.Write(Environment.NewLine)
                ElseIf Not to_write_character = "{" And Not to_write_character = ":" And Not to_write_character = "}" And
                    Not to_write_character = "," And Not to_write_character = "[" And Not to_write_character = "]" Then
                    master_filewriter.Write(to_write_character)
                End If
            End While


        Else 'Try to catch again if there is any error
            MessageBox.Show("Couldn't locate the Pokedex file. Try restarting the program.")
            Return
        End If

        master_filereader.Close()
        master_filewriter.Close()

        REM since we called this function, it means that we are starting the battle predictor off pretty cold
        REM so we need to cache things as we need it.

    End Sub
End Class

Public Class Dex_reader

    Private m_reading_file As String = ""
    Private m_parent_form As Form1

    Public Property Read_File As String
        Get
            Return m_reading_file
        End Get
        Set(value As String)
            m_reading_file = value
        End Set
    End Property

    Public Property Parent_Form As Form1
        Get
            Return m_parent_form
        End Get
        Set(value As Form1)
            m_parent_form = value
        End Set
    End Property

    ''' <summary>
    ''' Begins reading files. The object will determine which function to run based on the filename it holds.
    ''' </summary>
    ''' <param name="worker"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub Execute(ByVal worker As System.ComponentModel.BackgroundWorker,
                                ByVal e As System.ComponentModel.DoWorkEventArgs)
        If Me.Read_File = "pokemondictionary_formatted.txt" Then
            Me.read_pokemon(Me.Parent_Form)
            e.Result = "pokemondictionary_formatted.txt"
        ElseIf Me.Read_File = "moves_formatted.txt" Then
            Me.Read_Moves(Me.Parent_Form)
            e.Result = "moves_formatted.txt"
        ElseIf Me.Read_File = "ability_formatted.txt" Then
            Me.Read_Ability(Me.Parent_Form)
            e.Result = "ability_formatted.txt"
        Else
            MessageBox.Show("A thread couldn't load a file...", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Public Sub read_pokemon(ByVal calling_form As Form1)

        Dim filename As String = "pokemondictionary_formatted.txt"
        Dim filereader As StreamReader
        Try
            filereader = My.Computer.FileSystem.OpenTextFileReader(filename)
        Catch ex As Exception
            MessageBox.Show("Could not load pokemondictionary_formatted.txt. Check to see if it's not being opened somewhere else.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Dim dummypokemon As New Pokemon
        Dim toadd_pokemon As New Pokemon

        Dim moveflag As Boolean = False
        Dim abilityflag As Boolean = False
        Dim typeflag As Boolean = False

        Dim readingline As String = filereader.ReadLine()
        While Not filereader.EndOfStream()
            If readingline = "POKEMON" Then
                REM rip a pokemon out of the dummypokemon
                toadd_pokemon = dummypokemon.Clone()
            ElseIf readingline = "NAME" And moveflag = False And abilityflag = False And typeflag = False Then
                REM we have read a pokemon name
                readingline = filereader.ReadLine()
                toadd_pokemon.Name = readingline
            ElseIf readingline = "ATK" Then
                readingline = filereader.ReadLine()
                toadd_pokemon.ATK = Convert.ToInt32(readingline)
            ElseIf readingline = "DEF" Then
                readingline = filereader.ReadLine()
                toadd_pokemon.DEF = Convert.ToInt32(readingline)
            ElseIf readingline = "SP_ATK" Then
                readingline = filereader.ReadLine()
                toadd_pokemon.Sp_ATK = Convert.ToInt32(readingline)
            ElseIf readingline = "SP_DEF" Then
                readingline = filereader.ReadLine()
                toadd_pokemon.Sp_DEF = Convert.ToInt32(readingline)
            ElseIf readingline = "HP" Then
                readingline = filereader.ReadLine()
                toadd_pokemon.HP = Convert.ToInt32(readingline)
            ElseIf readingline = "SPD" Then
                readingline = filereader.ReadLine()
                toadd_pokemon.SPD = Convert.ToInt32(readingline)

            ElseIf readingline = "BEGIN ABILITY" Then
                REM read the following line just to make sure this pokemon has an ability
                readingline = filereader.ReadLine()
                While Not readingline = "END ABILITY"
                    If readingline = "NAME" Then
                        Dim ability As New Ability_Info
                        ability.Name = filereader.ReadLine() REM this line contains the name
                        readingline = filereader.ReadLine() REM this line contains the word "URI"
                        If Not readingline = "URI" Then
                            MessageBox.Show("There is an error in the pokemondictionary_formatted.txt file. An ability is messed up. We did not complete the reading process", "Whoops!", _
                                            MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return
                        End If
                        readingline = filereader.ReadLine() REM this line should contain the URI address
                        ability.URI = readingline

                        toadd_pokemon.Ability.Add(ability.Clone())
                    Else
                        MessageBox.Show("There is an error in the pokemondictionary_formatted.txt file. An ability is messed up. We did not complete the reading process", "Whoops!", _
                                            MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If

                    readingline = filereader.ReadLine()
                End While

            ElseIf readingline = "BEGIN MOVES" Then
                REM check to make sure the pokemon has some moves
                readingline = filereader.ReadLine()
                While Not readingline = "END MOVES"
                    If readingline = "NAME" Then
                        toadd_pokemon.Moves.Add(filereader.ReadLine()) REM read the next line, since it's the move name
                    Else
                        MessageBox.Show("There is an error in the pokemondictionary_formatted.txt file. A move is messed up. We did not complete the reading process", "Whoops!", _
                                            MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If
                    readingline = filereader.ReadLine()
                End While
            ElseIf readingline = "BEGIN TYPES" Then
                readingline = filereader.ReadLine()
                While Not readingline = "END TYPES"
                    REM keep on reading all the lines, since they are all type names
                    toadd_pokemon.Types.Add(readingline)

                    readingline = filereader.ReadLine()
                End While
            ElseIf readingline = "END POKEMON" Then
                REM package the pokemon and add it to the dictionary
                REM this is the important line
                calling_form.Get_PokemonDictionary.Add_Pokemon(toadd_pokemon.Name, toadd_pokemon)
            Else
                MessageBox.Show("Unknown value read in the file... Aborting to be safe. No pokemon have been added to the dictionary.", _
                                "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            readingline = filereader.ReadLine()
        End While

        REM our method of implementation will miss out the last pokemon of the stream
        calling_form.Get_PokemonDictionary.Add_Pokemon(toadd_pokemon.Name, toadd_pokemon)

        filereader.Close()
    End Sub

    Public Sub Read_Moves(ByVal calling_form As Form1)

        Dim filename As String = "moves_formatted.txt"
        Dim filereader As StreamReader

        Try
            filereader = My.Computer.FileSystem.OpenTextFileReader(filename)
        Catch ex As Exception
            MessageBox.Show("Could not load moves_formatted.txt. Make sure the file exists or it is not being used by someone else." & Environment.NewLine & _
                            "Don't worry about this because after you close the program moves should be written to the file.", "Whoops!", _
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Dim dummymoveinfo As New Move_Info
        Dim toadd_move As New Move_Info

        Dim readingline As String
        readingline = filereader.ReadLine()
        While Not filereader.EndOfStream

            If readingline = "MOVE" Then
                REM rip a move
                toadd_move = dummymoveinfo.Clone()

            ElseIf readingline = "NAME" Then
                REM we can read everything in one go
                readingline = filereader.ReadLine() REM this line contains the name of the move
                toadd_move.Name = readingline

                readingline = filereader.ReadLine() REM this line contains ACCURACY
                If Not readingline = "ACCURACY" Then
                    MessageBox.Show("There is unknown text in moves_formatted.txt. Aborting to be safe.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                readingline = filereader.ReadLine()
                toadd_move.Accuracy = Convert.ToInt32(readingline)

                readingline = filereader.ReadLine() REM this line contains POWER
                If Not readingline = "POWER" Then
                    MessageBox.Show("There is unknown text in moves_formatted.txt. Aborting to be safe.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                readingline = filereader.ReadLine()
                toadd_move.Power = Convert.ToInt32(readingline)

                readingline = filereader.ReadLine() REM this line should contain PP
                If Not readingline = "PP" Then
                    MessageBox.Show("There is unknown text in moves_formatted.txt. Aborting to be safe.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                readingline = filereader.ReadLine()
                toadd_move.PP = Convert.ToInt32(readingline)

                readingline = filereader.ReadLine() REM this line should contain URI
                If Not readingline = "URI" Then
                    MessageBox.Show("There is unknown text in moves_formatted.txt. Aborting to be safe.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                readingline = filereader.ReadLine()
                toadd_move.URI = readingline

                readingline = filereader.ReadLine() REM this line should contain SPECIAL
                If Not readingline = "SPECIAL" Then
                    MessageBox.Show("There is unknown text in moves_formatted.txt. Aborting to be safe.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                readingline = filereader.ReadLine()
                toadd_move.Is_Special = Convert.ToBoolean(readingline)

                readingline = filereader.ReadLine()
                If Not readingline = "EFFECT" Then
                    MessageBox.Show("There is unknown text in moves_formatted.txt. Aborting to be safe.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                readingline = filereader.ReadLine()
                toadd_move.Effect = readingline

                readingline = filereader.ReadLine() REM this line should contain TYPE
                If Not readingline = "TYPE" Then
                    MessageBox.Show("There is unknown text in moves_formatted.txt. Aborting to be safe.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                toadd_move.Type = filereader.ReadLine()

                readingline = filereader.ReadLine()
                If Not readingline = "END MOVE" Then
                    MessageBox.Show("There is unknown text in moves_formatted.txt. Aborting to be safe.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                Dim movename_key As String = toadd_move.Name.Trim("""")
                calling_form.Get_MoveDictionary.Add_Move(movename_key, toadd_move)
            Else
                MessageBox.Show("There is unknown text in moves_formatted.txt. Aborting to be safe.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            readingline = filereader.ReadLine()
        End While

        filereader.Close()

    End Sub

    Public Sub Read_Ability(ByVal calling_form As Form1)

        Dim filename As String = "ability_formatted.txt"
        Dim filereader As StreamReader
        Try
            filereader = My.Computer.FileSystem.OpenTextFileReader(filename)
        Catch ex As Exception
            MessageBox.Show("Could not load ability_formatted.txt. Make sure the file exists or it is not being used by someone else.", "Whoops!", _
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Dim dummyabilityinfo As New Ability_Info
        Dim toadd_ability As New Ability_Info

        Dim readingline As String = ""
        readingline = filereader.ReadLine()
        While Not filereader.EndOfStream
            If readingline = "ABILITY" Then
                toadd_ability = dummyabilityinfo.Clone()
            ElseIf readingline = "NAME" Then
                readingline = filereader.ReadLine()
                toadd_ability.Name = readingline

                readingline = filereader.ReadLine() REM this line should contain URI
                If Not readingline = "URI" Then
                    MessageBox.Show("There is unknown text in ability_formatted.txt. Aborting to be safe.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                readingline = filereader.ReadLine()
                toadd_ability.URI = readingline

                readingline = filereader.ReadLine()
                If Not readingline = "END ABILITY" Then
                    MessageBox.Show("There is unknown text in moves_formatted.txt. Aborting to be safe.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                REM now we can add the ability
                calling_form.Get_AbilityDictionary.Add_Ability(toadd_ability.Name, toadd_ability)

            Else
                MessageBox.Show("There is unknown text in moves_formatted.txt. Aborting to be safe.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            readingline = filereader.ReadLine()
        End While

        filereader.Close()
    End Sub
End Class
