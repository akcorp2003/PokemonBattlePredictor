Imports System
Imports System.Collections
Imports System.Net
Imports System.IO

Public Class BattleSetup

    Private Sub InsertPokemon_Click(sender As Object, e As EventArgs) Handles InsertPokemon.Click
        REM first query the Pokemon Dictionary if we have the pokemon already stored!
        REM if not, we will query the master database
        Dim proper_pokename As String = ""
        proper_pokename = Pokemon_Name.Text.Trim()
        proper_pokename = proper_pokename.ToLower()
        If Form1.Get_PokemonDictionary.IsPokemonInDictionary(proper_pokename) = False Then
            REM query the database
            Dim returnvalue As Integer

            returnvalue = Form1.Get_PokemonDictionary().Add_Pokemon(Pokemon_Name.Text)
            If returnvalue = 0 Then
                MessageBox.Show("Yay! We added " & Pokemon_Name.Text.Trim & " into our database!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                MessageBox.Show("Whoops! There was some trouble adding a pokemon. Please check the previous error messages.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If

        REM to freeze the progress of this form
        InsertPokemonFunct.ShowDialog()

        If InsertPokemonFunct.LastAdded_Pokemon = "blue" Then
            Dim pokeinfo As ListViewItem

            REM display the information of the last pokemon added
            pokeinfo = TeamBlue_List.Items.Add(Form1.Get_PokemonArena.Get_TeamBlue.Get_Team("blue").Last.Name)
            pokeinfo.SubItems.Add(Convert.ToString(Form1.Get_PokemonArena.Get_TeamBlue.Get_Team("blue").Last.HP))
            pokeinfo.SubItems.Add(Convert.ToString(Form1.Get_PokemonArena.Get_TeamBlue.Get_Team("blue").Last.ATK))
            pokeinfo.SubItems.Add(Convert.ToString(Form1.Get_PokemonArena.Get_TeamBlue.Get_Team("blue").Last.DEF))
            pokeinfo.SubItems.Add(Convert.ToString(Form1.Get_PokemonArena.Get_TeamBlue.Get_Team("blue").Last.Sp_ATK))
            pokeinfo.SubItems.Add(Convert.ToString(Form1.Get_PokemonArena.Get_TeamBlue.Get_Team("blue").Last.Sp_DEF))
            pokeinfo.SubItems.Add(Convert.ToString(Form1.Get_PokemonArena.Get_TeamBlue.Get_Team("blue").Last.SPD))
            pokeinfo.SubItems.Add(Form1.Get_PokemonArena.Get_TeamBlue.Get_Team("blue").Last.Ability.First.Name)
            TeamBlue_List.Update()
            TeamBlue_List.EndUpdate()
        ElseIf InsertPokemonFunct.LastAdded_Pokemon = "red" Then
            Dim pokeinfo As ListViewItem

            REM display the information of the last pokemon added
            pokeinfo = TeamRed_List.Items.Add(Form1.Get_PokemonArena.Get_TeamRed.Get_Team("red").Last.Name)
            pokeinfo.SubItems.Add(Convert.ToString(Form1.Get_PokemonArena.Get_TeamRed.Get_Team("red").Last.HP))
            pokeinfo.SubItems.Add(Convert.ToString(Form1.Get_PokemonArena.Get_TeamRed.Get_Team("red").Last.ATK))
            pokeinfo.SubItems.Add(Convert.ToString(Form1.Get_PokemonArena.Get_TeamRed.Get_Team("red").Last.DEF))
            pokeinfo.SubItems.Add(Convert.ToString(Form1.Get_PokemonArena.Get_TeamRed.Get_Team("red").Last.Sp_ATK))
            pokeinfo.SubItems.Add(Convert.ToString(Form1.Get_PokemonArena.Get_TeamRed.Get_Team("red").Last.Sp_DEF))
            pokeinfo.SubItems.Add(Convert.ToString(Form1.Get_PokemonArena.Get_TeamRed.Get_Team("red").Last.SPD))
            pokeinfo.SubItems.Add(Form1.Get_PokemonArena.Get_TeamRed.Get_Team("red").Last.Ability.First.Name)
            TeamRed_List.Update()
            TeamRed_List.EndUpdate()
        Else
            MessageBox.Show("Something unexpected happen when adding a pokemon. The pokemon is probably not displaying on the List.", "Check Up!", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End If
        Pokemon_Name.Text = ""

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        REM everything was already added. All we need to do is to close this form.
        Me.Close()
    End Sub

    Private Sub Initiate_Build_Click(sender As Object, e As EventArgs) Handles Initiate_Build.Click
        Dim dic_enum As New Dictionary(Of String, String).Enumerator
        dic_enum = Form1.Get_ResourceURIDictionary.Get_ResourceURIDictionary.GetEnumerator()
        dic_enum.MoveNext()
        Dim i As Integer = 0

        While i < Form1.Get_ResourceURIDictionary.Get_ResourceURIDictionary.Count

            Me.Pokemon_Name.Text = dic_enum.Current.Key.Trim("""")

            Me.InsertPokemon.PerformClick()

            i += 1
            dic_enum.MoveNext()
        End While

        MessageBox.Show("All done building!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub Initiate_movebuild_Click(sender As Object, e As EventArgs) Handles Initiate_movebuild.Click
        Dim csv_reader As New Dex_reader
        csv_reader.Read_MovesCSV()
        MessageBox.Show("All done building moves!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
End Class