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
