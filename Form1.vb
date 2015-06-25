﻿Imports System
Imports System.Net
Imports System.IO

Public Class Form1

    Dim dictionaryOf_ResourceURI As New Pokemon_ResourceURI_dictionary
    Dim dictionaryOf_Pokemon As New Pokemon_Dictionary
    Dim dictionaryOf_Moves As New Move_Dictionary
    Dim dictionaryOf_Abilities As New Ability_Dictionary

    Dim Team_Blue As New Pokemon_Team
    Dim Team_Red As New Pokemon_Team

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

    Public Function Get_TeamBlue() As Pokemon_Team
        Return Team_Blue
    End Function

    Public Function Get_TeamRed() As Pokemon_Team
        Return Team_Red
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
        my_stream = GETURL.GetResponse.GetResponseStream()

        Dim stream_reader As New StreamReader(my_stream)

        Dim line As String = ""
        line = stream_reader.ReadLine()

        Progress.AddingPokemon_Bar.PerformStep()
        Progress.Information.Text = "Got all text from database."

        Return line

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

        REM if the program successfully finds the file, then we don't need to worry!
        Dim dex_formatter As New Dex_formatter
        dex_formatter.Formatting_Filename = filename
        Worker_Pokedex_Formatter.RunWorkerAsync(dex_formatter)

        REM at this point, the pokedex has been successfully formatted
        REM we will now begin adding the URI Pokemon into the ResourceURI_dictionary
        REM this MUST wait until the Pokedex_Formatter worker finished its job
        Worker_InsertURI.RunWorkerAsync(dictionaryOf_ResourceURI)
    End Sub



    Private Sub BattleButton_Click(sender As Object, e As EventArgs) Handles BattleButton.Click
        BattleSetup.Show()

    End Sub

    'Prediction algorithm begins here
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Pass the Pokemon to true algorithm
    End Sub
End Class
