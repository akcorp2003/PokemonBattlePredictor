Imports Novacode
Imports System
Imports System.Net
Imports System.IO
Imports System.Drawing
Imports PokemonBattlePredictor
Imports PokemonBattlePredictor.PBP
Imports PokemonBattlePredictor.PBP.Dictionary
Imports PokemonBattlePredictor.PBP.Constants
Imports PokemonBattlePredictor.PBP.InfoBlocks
Imports PokemonBattlePredictor.PBP.Dex_IO

Public Class Form1

    Dim dictionaryOf_ResourceURI As New Pokemon_ResourceURI_dictionary
    Dim dictionaryOf_Pokemon As New Pokemon_Dictionary
    Dim dictionaryOf_Moves As New Move_Dictionary
    Dim dictionaryOf_Abilities As New Ability_Dictionary
    Dim listOf_PokemonArena As New List(Of Pokemon_Arena) REM this list will only house 2 arenas at the moment

    Dim battle_arena As New Pokemon_Arena

    Public Function Get_ResourceURIDictionary() As Pokemon_ResourceURI_dictionary
        Return dictionaryOf_ResourceURI
    End Function

    Public Function Get_PokemonDictionary() As Pokemon_Dictionary
        Return dictionaryOf_Pokemon
    End Function

    Public Function Get_MoveDictionary() As Move_Dictionary
        Return dictionaryOf_Moves
    End Function

    Public Function Get_AbilityDictionary() As Ability_Dictionary
        Return dictionaryOf_Abilities
    End Function

    Public Function Get_PokemonArena() As Pokemon_Arena
        Return listOf_PokemonArena(0) REM we will assume the first arena is the working arena
    End Function

    '////////////////////////////////////////////////////////////////
    '// BackgroundWorker Code                                      //
    '////////////////////////////////////////////////////////////////

    Dim Pokedex_Formatter_Finished As Boolean = False

    Private Sub Pokedex_Writer_DoWork(ByVal sender As Object,
                                      ByVal e As System.ComponentModel.DoWorkEventArgs
                                      ) Handles Worker_Pokedex_Writer.DoWork
        REM The real work begins here

        Dim worker As System.ComponentModel.BackgroundWorker
        worker = CType(sender, System.ComponentModel.BackgroundWorker)

        Dim dex_writer As Dex_Writer = CType(e.Argument, Dex_Writer)
        dex_writer.Write_to_Pokedex(worker, e)

    End Sub

    Private Sub Pokedex_Writer_WriteCompleted(ByVal sender As Object,
                                              ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs
                                              ) Handles Worker_Pokedex_Writer.RunWorkerCompleted
        REM outputs any error of sorts
        If e.Error IsNot Nothing Then
            MessageBox.Show("Whoops! Error: " & e.Error.Message)
        ElseIf e.Cancelled Then
            MessageBox.Show("Cancelled")
        Else
            MessageBox.Show("Finished Baby.")
        End If
    End Sub

    Private Sub Pokedex_Formatter_DoWork(ByVal sender As Object,
                                         ByVal e As System.ComponentModel.DoWorkEventArgs
                                         ) Handles Worker_Pokedex_Formatter.DoWork
        REM The real work begins here
        Dim worker As System.ComponentModel.BackgroundWorker
        worker = CType(sender, System.ComponentModel.BackgroundWorker)


        Dim dex_formatter As Dex_formatter = CType(e.Argument, Dex_formatter)
        dex_formatter.Format_Dex(dex_formatter.Formatting_Filename)
    End Sub

    Private Sub Pokedex_Formatter_WriteCompleted(ByVal sender As Object,
                                                 ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs
                                                 ) Handles Worker_Pokedex_Formatter.RunWorkerCompleted
        REM Set the flag to alert
        If e.Error IsNot Nothing Then
            MessageBox.Show("Whoops! Error: " & e.Error.Message)
        ElseIf e.Cancelled Then
            MessageBox.Show("Cancelled")
        Else
            MessageBox.Show("Finished Formatting Baby.")
            Pokedex_Formatter_Finished = True
        End If
    End Sub

    Private Sub Worker_InsertURI_DoWork(ByVal sender As Object,
                                      ByVal e As System.ComponentModel.DoWorkEventArgs
                                      ) Handles Worker_InsertURI.DoWork
        REM infinite For loop until the Pokedex_Formatter_Finished flag is finished
        For i As Integer = 0 To 1 Step 0
            If Pokedex_Formatter_Finished = True Then
                Exit For
            End If
        Next

        Dim worker As System.ComponentModel.BackgroundWorker
        worker = CType(sender, System.ComponentModel.BackgroundWorker)

        dictionaryOf_ResourceURI = CType(e.Argument, Pokemon_ResourceURI_dictionary)
        dictionaryOf_ResourceURI.Process_Pokedex(worker, e)
    End Sub

    Private Sub ResourceURIDictionary_WriteCompleted(ByVal sender As Object,
                                                    ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs
                                                    ) Handles Worker_InsertURI.RunWorkerCompleted
        If e.Error IsNot Nothing Then
            MessageBox.Show("Whoops! Error: " & e.Error.Message)
        ElseIf e.Cancelled Then
            MessageBox.Show("Cancelled")
        Else
            MessageBox.Show("Finished Creating URI Dictionary Baby. You like that don't you?")
        End If

    End Sub

    Private Sub PokemonReader_DoWork(ByVal sender As Object,
                                      ByVal e As System.ComponentModel.DoWorkEventArgs
                                      ) Handles Worker_Pokemonreader.DoWork
        Dim worker As System.ComponentModel.BackgroundWorker
        worker = CType(sender, System.ComponentModel.BackgroundWorker)

        Dim poke_reader As New Dex_reader
        poke_reader = CType(e.Argument, Dex_reader)
        poke_reader.Execute(worker, e)

    End Sub

    Private Sub PokemonReader_WriteCompleted(ByVal sender As Object,
                                             ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs
                                             ) Handles Worker_Pokemonreader.RunWorkerCompleted
        If e.Error IsNot Nothing Then
            MessageBox.Show("Whoops! Error: " & e.Error.Message)
        ElseIf e.Cancelled Then
            MessageBox.Show("Cancelled")
        Else
            MessageBox.Show("Finished processing file" & e.Result & ". You like that don't you?")
        End If

    End Sub

    Private Sub MoveReader_DoWork(ByVal sender As Object,
                                      ByVal e As System.ComponentModel.DoWorkEventArgs
                                      ) Handles Worker_Movereader.DoWork
        Dim worker As System.ComponentModel.BackgroundWorker
        worker = CType(sender, System.ComponentModel.BackgroundWorker)

        Dim move_reader As New Dex_reader
        move_reader = CType(e.Argument, Dex_reader)
        move_reader.Execute(worker, e)

    End Sub

    Private Sub MoveReader_WriteCompleted(ByVal sender As Object,
                                             ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs
                                             ) Handles Worker_Movereader.RunWorkerCompleted
        If e.Error IsNot Nothing Then
            MessageBox.Show("Whoops! Error: " & e.Error.Message)
        ElseIf e.Cancelled Then
            MessageBox.Show("Cancelled")
        Else
            MessageBox.Show("Finished processing file" & e.Result & ". You like that don't you?")
        End If
    End Sub

    Private Sub AbilityReader_DoWork(ByVal sender As Object,
                                      ByVal e As System.ComponentModel.DoWorkEventArgs
                                      ) Handles Worker_Abilityreader.DoWork
        Dim worker As System.ComponentModel.BackgroundWorker
        worker = CType(sender, System.ComponentModel.BackgroundWorker)

        Dim ability_reader As New Dex_reader
        ability_reader = CType(e.Argument, Dex_reader)
        ability_reader.Execute(worker, e)

    End Sub

    Private Sub AbilityReader_WriteCompleted(ByVal sender As Object,
                                             ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs
                                             ) Handles Worker_Abilityreader.RunWorkerCompleted
        If e.Error IsNot Nothing Then
            MessageBox.Show("Whoops! Error: " & e.Error.Message)
        ElseIf e.Cancelled Then
            MessageBox.Show("Cancelled")
        Else
            MessageBox.Show("Finished processing file" & e.Result & ". You like that don't you?")
        End If
    End Sub



    '/////////////////////////////////////////////////////////////
    '// END BackgroundWorker Code                               //
    '/////////////////////////////////////////////////////////////

    '/////////////////////////////////////////////////////////////
    '// Grunt work functions                                    //
    '/////////////////////////////////////////////////////////////

    Public Function RequestLine(ByVal uri As String) As String
        Dim GETURL As WebRequest
        GETURL = WebRequest.Create(uri)


        REM check for internet connection
        If Not My.Computer.Network.IsAvailable Then
            MessageBox.Show("There is no internet connection. The battle predictor cannot work without the interwebs. Please check your connection", _
                            "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If

        Dim my_stream As Stream
        Try
            my_stream = GETURL.GetResponse.GetResponseStream()
        Catch ex As Exception
            MessageBox.Show("There is probably something wrong with your internet connection. Try again maybe?", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ""
        End Try


        Dim stream_reader As New StreamReader(my_stream)

        Dim line As String = ""
        line = stream_reader.ReadLine()

        Progress.AddingPokemon_Bar.PerformStep()
        Progress.Information.Text = "Got all text from database."

        Return line

    End Function

    Public Function RequestImage(ByVal uri As String) As Bitmap
        Dim my_client As New WebClient
        Dim poke_sprite As Bitmap
        Try
            poke_sprite = Bitmap.FromStream(New MemoryStream(my_client.DownloadData(uri)))
        Catch ex As Exception
            If uri.Contains("x-y") And Not uri.Contains("mega") Then REM currently, there are no mega sprites available... :(
                MessageBox.Show("Something went wrong when trying to download the image from this uri: !!" + uri, "Oh no!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            Return Nothing
        End Try

        Return poke_sprite
    End Function

    ''' <summary>
    ''' This function will properly remove all unnecessary characters according to the design of this program.
    ''' </summary>
    ''' <param name="content"></param>
    ''' <param name="inputfilename"></param>
    ''' <param name="outputfilename"></param>
    ''' <returns>The original filename or "" to indicate an error</returns>
    ''' <remarks></remarks>
    Public Function FormatFile(ByVal content As String, ByVal inputfilename As String, ByVal outputfilename As String) As String
        Dim master_filereader As StreamReader = Nothing
        Dim master_filewriter1 As StreamWriter = Nothing
        Dim master_filewriter2 As StreamWriter = Nothing

        If My.Computer.FileSystem.FileExists(inputfilename) = True Then
            Try
                REM this isn't the file we're looking for ;)
                My.Computer.FileSystem.DeleteFile(inputfilename)
            Catch ex As Exception
                MessageBox.Show("Something went wrong when trying to delete the file " & inputfilename & "... Aborting", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return ""
            End Try
        End If

        Try
            master_filewriter1 = My.Computer.FileSystem.OpenTextFileWriter(inputfilename, False)
        Catch ex As Exception
            MessageBox.Show("Something went wrong when trying to open the file " & inputfilename & " for writing... Aborting", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ""
        End Try

        master_filewriter1.WriteLine(content)
        master_filewriter1.Close()

        Try
            master_filereader = My.Computer.FileSystem.OpenTextFileReader(inputfilename)
        Catch ex As Exception
            MessageBox.Show("Something went wrong when trying to open the file " & inputfilename & " for reading... Aborting", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ""
        End Try

        If My.Computer.FileSystem.FileExists(outputfilename) Then
            Try
                My.Computer.FileSystem.DeleteFile(outputfilename)
            Catch ex As Exception
                MessageBox.Show("Something went wrong when trying to delete the file " & outputfilename & "... Aborting", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return ""
            End Try
        End If

        Try
            master_filewriter2 = My.Computer.FileSystem.OpenTextFileWriter(outputfilename, False)
        Catch ex As Exception
            MessageBox.Show("Something went wrong when trying to open the file " & outputfilename & " for writing... Aborting", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ""
        End Try

        While master_filereader.EndOfStream() = False
            Dim characterinString(0) As Char
            master_filereader.Read(characterinString, 0, 1)
            Dim to_write_character As String = New String(characterinString)
            If to_write_character = " " Then
                REM write the character to file
                master_filewriter2.Write(Environment.NewLine)
            ElseIf Not to_write_character = "{" And Not to_write_character = ":" And Not to_write_character = "}" And
                Not to_write_character = "," And Not to_write_character = "[" And Not to_write_character = "]" Then
                master_filewriter2.Write(to_write_character)
            End If
        End While

        master_filereader.Close()
        master_filewriter2.Close()

        Return outputfilename

    End Function

    '/////////////////////////////////////////////////////////////
    '// End Grunt work Functions                                //
    '/////////////////////////////////////////////////////////////

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        REM First check if an existing Pokedex file is there
        Dim filename As String = "pokedex.txt"

        Dim filereader As System.IO.StreamReader
        Try
            filereader = My.Computer.FileSystem.OpenTextFileReader(filename)
        Catch ex As Exception
            REM if we get here, then that means no file exists so we have to query the
            REM master database server
            Dim dex_writer As New Dex_Writer
            Worker_Pokedex_Writer.RunWorkerAsync(dex_writer)
            REM we do not need to invoke the RunWorkerAsync for Dex_formatter because dex_writer calls
            REM it for us. We can safely finish the loading process
            Return
        End Try

        filereader.Close()

        REM if the program successfully finds the file, then we don't need to worry!
        Dim dex_formatter As New Dex_formatter
        dex_formatter.Formatting_Filename = filename
        Worker_Pokedex_Formatter.RunWorkerAsync(dex_formatter)

        REM at this point, the pokedex has been successfully formatted
        REM we will now begin adding the URI Pokemon into the ResourceURI_dictionary
        REM this MUST wait until the Pokedex_Formatter worker finished its job
        Worker_InsertURI.RunWorkerAsync(dictionaryOf_ResourceURI)

        REM thread for reading pokemon
        Dim dex_reader1 As New Dex_reader
        dex_reader1.Read_File = "pokemondictionary_formatted.txt"
        dex_reader1.Parent_Form = Me
        Worker_Pokemonreader.RunWorkerAsync(dex_reader1)

        REM thread for reading moves
        Dim dex_reader2 As New Dex_reader
        dex_reader2.Read_File = "moves_formatted.txt"
        dex_reader2.Parent_Form = Me
        Worker_Movereader.RunWorkerAsync(dex_reader2)

        REM thread for reading abilities
        Dim dex_reader3 As New Dex_reader
        dex_reader3.Read_File = "ability_formatted.txt"
        dex_reader3.Parent_Form = Me
        Worker_Abilityreader.RunWorkerAsync(dex_reader3)

        REM do other initializations
        Dim first_arena As New Pokemon_Arena
        listOf_PokemonArena.Add(first_arena)
        Constants.Accuracy_Level = Accuracy.HIGH
        Constants.Damage = Damage_Level.MAX

    End Sub

    Private Sub Form1_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        REM output the data into files
        Dim writer As New Dex_Writer

        REM first readout the Pokemon files
        writer.PrintPokemonInfo(Me)

        REM second readout the Abilities
        writer.PrintAbilityInfo(Me)

        REM finally readout the Moves
        writer.PrintMoveInfo(Me)

        MessageBox.Show("Thank you for using PBP. The data is now finished writing.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub


    Private Sub BattleButton_Click(sender As Object, e As EventArgs) Handles BattleButton.Click
        BattleSetup.Show()
    End Sub

    Public Sub LoadImages()
        Dim base_path As String = "sprites/"
        Dim filetype As String = ".bmp"

        REM load the Blue Team photos
        For i As Integer = 0 To listOf_PokemonArena(0).Get_TeamBlue.Get_Team("blue").Count - 1 Step 1
            Dim name As String = listOf_PokemonArena(0).Get_TeamBlue.Get_Team("blue").Item(i).Name
            Dim full_path As String = base_path + Constants.Get_FormattedString(name) + filetype
            Dim sprite As New Bitmap(full_path)
            If sprite IsNot Nothing Then
                REM only display the first three pokemon's sprites
                If i = 0 Then
                    TeamBlue_1.Image = sprite
                    TeamBlue_1.SizeMode = PictureBoxSizeMode.AutoSize
                ElseIf i = 1 Then
                    TeamBlue_2.Image = sprite
                    TeamBlue_2.SizeMode = PictureBoxSizeMode.AutoSize
                ElseIf i = 2 Then
                    TeamBlue_3.Image = sprite
                    TeamBlue_3.SizeMode = PictureBoxSizeMode.AutoSize
                End If
            End If
        Next

        REM load the Red Team photos
        For i As Integer = 0 To listOf_PokemonArena(0).Get_TeamRed.Get_Team("red").Count - 1 Step 1
            Dim name As String = listOf_PokemonArena(0).Get_TeamRed.Get_Team("red").Item(i).Name
            Dim full_path As String = base_path + Constants.Get_FormattedString(name) + filetype
            Dim sprite As New Bitmap(full_path)
            If sprite IsNot Nothing Then
                REM only display the first three pokemon's sprites
                If i = 0 Then
                    TeamRed_1.Image = sprite
                    TeamRed_1.SizeMode = PictureBoxSizeMode.AutoSize
                ElseIf i = 1 Then
                    TeamRed_2.Image = sprite
                    TeamRed_2.SizeMode = PictureBoxSizeMode.AutoSize
                ElseIf i = 2 Then
                    TeamRed_3.Image = sprite
                    TeamRed_3.SizeMode = PictureBoxSizeMode.AutoSize
                End If
            End If
        Next
    End Sub

    'Prediction algorithm begins here
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Pass the Pokemon to true algorithm
        Dim predictor As New Battle_Prediction
        Dim winner As String = ""
        If listOf_PokemonArena.Count = 0 Then
            MessageBox.Show("There are no arenas in the system! Something has gone wrong...try restarting the program. Returning.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        Dim temp_battlearena As New Pokemon_Arena REM for log printing
        temp_battlearena = listOf_PokemonArena(0).Clone()

        Logger.InitializeRecord()
        winner = predictor.predict_outcome(temp_battlearena)
        MessageBox.Show("The winning party is: " & winner & ". ", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Logger.Record(winner + " is the winning team of this match!")
        Dim toprint As Integer = MessageBox.Show("Would you like to see the battle record?", "Print?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If toprint = DialogResult.Yes Then
            Dim logprinter As New Dex_Writer
            logprinter.PrintLogger(listOf_PokemonArena(0), 1, Funct_IDs.Button2_Form1)
            MessageBox.Show("The log is finished writing!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        REM Archive current arena and prepare the next arena
        Dim next_arena As New Pokemon_Arena
        listOf_PokemonArena.Insert(0, next_arena)
    End Sub

    '///////////////////////////////////////////////////
    '//// ToolBar Functions                           //
    '///////////////////////////////////////////////////

    Private Sub RecentBattleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RecentBattleToolStripMenuItem.Click
        If listOf_PokemonArena.Count = 1 Then REM after the first prediction, there will be 2 arenas. However, if only one arena is in the system, that arena is empty by convention
            MessageBox.Show("There has been no battles! Try predicting a battle and I'll print out the battle for you then. :)", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        Dim recent_logprinter As New Dex_Writer
        recent_logprinter.PrintLogger(listOf_PokemonArena(1), 1)
        MessageBox.Show("The recent log is finished printing! Find the filename battlelog_recent.docx.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub LastBattleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LastBattleToolStripMenuItem.Click
        If listOf_PokemonArena.Count = 1 Then
            MessageBox.Show("There has been no battles! Try predicting a battle and I'll print out the battle for you then. :)", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        Dim last_logprinter As New Dex_Writer
        last_logprinter.PrintLogger(listOf_PokemonArena(2), 2)
        MessageBox.Show("The last log is finished printing! Find the filename battlelog_last.docx.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub HighAccuracyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HighAccuracyToolStripMenuItem.Click
        Constants.Accuracy_Level = Accuracy.HIGH
        MessageBox.Show("The accuracy has been set to HIGH.", "Settings Changed", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub


    Private Sub MediumAccuracyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MediumAccuracyToolStripMenuItem.Click
        Constants.Accuracy_Level = Accuracy.MEDIUM
        MessageBox.Show("The accuracy has been set to MEDIUM.", "Settings Changed", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub LowAccuracyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LowAccuracyToolStripMenuItem.Click
        Constants.Accuracy_Level = Accuracy.LOW
        MessageBox.Show("The accuracy has been set to LOW.", "Settings Changed", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub


    Private Sub MaximumToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MaximumToolStripMenuItem.Click
        Constants.Damage = Damage_Level.MAX
        MessageBox.Show("The damage has been set to MAXIMUM.", "Settings Changed", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub


    Private Sub NormalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NormalToolStripMenuItem.Click
        Constants.Damage = Damage_Level.NORM
        MessageBox.Show("The damage has been set to NORMAL.", "Settings Changed", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub MinimumToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MinimumToolStripMenuItem.Click
        Constants.Damage = Damage_Level.MIN
        MessageBox.Show("The damage has been set to MINIMUM.", "Settings Changed", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
End Class
