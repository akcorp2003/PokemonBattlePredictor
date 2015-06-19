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
        REM for testing purposes, we'll output it to the console
        Dim Line As String = ""
        Dim i As Integer = 0

        REM set up the filename and things
        Dim filename As String = "pokedex_real.txt"
        Try
            My.Computer.FileSystem.DeleteFile(filename)
        Catch ex As System.IO.FileNotFoundException
            REM yup, don't do anything
        End Try

        Do While Not Line Is Nothing
            i += 1
            Line = stream_Reader.ReadLine
            If Not Line Is Nothing Then
                REM Write the line to file
                My.Computer.FileSystem.WriteAllText(filename, Line, True)
            End If
        Loop

        My.Computer.FileSystem.WriteAllText(filename, "END", True)



    End Sub


End Class
