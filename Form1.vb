Imports System
Imports System.Net
Imports System.IO

Public Class Form1

    Dim dictionaryOf_ResourceURI As New Pokemon_ResourceURI_dictionary

    '////////////////////////////////////////////////////////////////
    '// BackgroundWorker Code                                      //
    '////////////////////////////////////////////////////////////////

    Dim Pokedex_Formatter_Finished As Boolean = False

    Private Sub Pokedex_Writer_DoWork(ByVal sender As Object,
                                      ByVal e As System.ComponentModel.DoWorkEventArgs
                                      ) Handles Pokedex_Writer.DoWork
        REM The real work begins here

        Dim worker As System.ComponentModel.BackgroundWorker
        worker = CType(sender, System.ComponentModel.BackgroundWorker)

        Dim dex_writer As Dex_Writer = CType(e.Argument, Dex_Writer)
        dex_writer.Write_to_Pokedex(worker, e)

    End Sub

    Private Sub Pokedex_Writer_WriteCompleted(ByVal sender As Object,
                                              ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs
                                              ) Handles Pokedex_Writer.RunWorkerCompleted
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
                                         ) Handles Pokedex_Formatter.DoWork
        REM The real work begins here
        Dim worker As System.ComponentModel.BackgroundWorker
        worker = CType(sender, System.ComponentModel.BackgroundWorker)


        Dim dex_formatter As Dex_formatter = CType(e.Argument, Dex_formatter)
        dex_formatter.Format_Dex(dex_formatter.Formatting_Filename)
    End Sub

    Private Sub Pokedex_Formatter_WriteCompleted(ByVal sender As Object,
                                                 ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs
                                                 ) Handles Pokedex_Formatter.RunWorkerCompleted
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

        Dim josh As Integer = 9
    End Sub



    '/////////////////////////////////////////////////////////////
    '// END BackgroundWorker Code                               //
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
            Pokedex_Writer.RunWorkerAsync(dex_writer)
            REM we do not need to invoke the RunWorkerAsync for Dex_formatter because dex_writer calls
            REM it for us. We can safely finish the loading process
            Return
        End Try

        REM if the program successfully finds the file, then we don't need to worry!
        Dim dex_formatter As New Dex_formatter
        dex_formatter.Formatting_Filename = filename
        Pokedex_Formatter.RunWorkerAsync(dex_formatter)

        REM at this point, the pokedex has been successfully formatted
        REM we will now begin adding the URI Pokemon into the ResourceURI_dictionary
        REM this MUST wait until the Pokedex_Formatter worker finished its job
        Worker_InsertURI.RunWorkerAsync(dictionaryOf_ResourceURI)
        Dim justhere As Integer = 9
        justhere += 1
    End Sub



    Private Sub BattleButton_Click(sender As Object, e As EventArgs) Handles BattleButton.Click
        BattleSetup.Show()

    End Sub

    'Prediction algorithm begins here
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Pass the Pokemon to true algorithm
    End Sub
End Class
