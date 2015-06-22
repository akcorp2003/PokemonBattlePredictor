Imports System
Imports System.Collections
Imports System.Net
Imports System.IO

Public Class BattleSetup

    Private Sub InsertPokemon_Click(sender As Object, e As EventArgs) Handles InsertPokemon.Click
        REM first query the Pokemon Dictionary if we have the pokemon already stored!
        REM if not, we will query the master database
        If Form1.Get_PokemonDictionary.IsPokemonInDictionary(Pokemon_Name.Text) = False Then
            REM query the database
            Dim returnvalue As Integer
            returnvalue = Form1.Get_PokemonDictionary().Add_Pokemon(Pokemon_Name.Text)
            If returnvalue = 0 Then
                MessageBox.Show("Yay! We added " & Pokemon_Name.Text & " into our database!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Whoops! There was some trouble adding a pokemon. Please check the previous error messages.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
            
            Dim Josh As Integer = 9
        End If



        InsertPokemonFunct.Show()


    End Sub
End Class