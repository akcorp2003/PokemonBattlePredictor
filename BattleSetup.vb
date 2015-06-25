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
            
        End If

        REM to freeze the progress of this form
        InsertPokemonFunct.ShowDialog()

        If InsertPokemonFunct.LastAdded_Pokemon = "blue" Then
            Dim pokeinfo As ListViewItem

            REM display the information of the last pokemon added
            pokeinfo = TeamBlue_List.Items.Add(Form1.Get_TeamBlue.Get_Team("blue").Last.Name)
            pokeinfo.SubItems.Add(Convert.ToString(Form1.Get_TeamBlue.Get_Team("blue").Last.HP))
            pokeinfo.SubItems.Add(Convert.ToString(Form1.Get_TeamBlue.Get_Team("blue").Last.ATK))
            pokeinfo.SubItems.Add(Convert.ToString(Form1.Get_TeamBlue.Get_Team("blue").Last.DEF))
            pokeinfo.SubItems.Add(Convert.ToString(Form1.Get_TeamBlue.Get_Team("blue").Last.Sp_ATK))
            pokeinfo.SubItems.Add(Convert.ToString(Form1.Get_TeamBlue.Get_Team("blue").Last.Sp_DEF))
            pokeinfo.SubItems.Add(Convert.ToString(Form1.Get_TeamBlue.Get_Team("blue").Last.SPD))
            pokeinfo.SubItems.Add(Form1.Get_TeamBlue.Get_Team("blue").Last.Ability.First.Name)
            TeamBlue_List.Update()
            TeamBlue_List.EndUpdate()
        ElseIf InsertPokemonFunct.LastAdded_Pokemon = "red" Then
            Dim pokeinfo As ListViewItem

            REM display the information of the last pokemon added
            pokeinfo = TeamRed_List.Items.Add(Form1.Get_TeamRed.Get_Team("red").Last.Name)
            pokeinfo.SubItems.Add(Convert.ToString(Form1.Get_TeamRed.Get_Team("red").Last.HP))
            pokeinfo.SubItems.Add(Convert.ToString(Form1.Get_TeamRed.Get_Team("red").Last.ATK))
            pokeinfo.SubItems.Add(Convert.ToString(Form1.Get_TeamRed.Get_Team("red").Last.DEF))
            pokeinfo.SubItems.Add(Convert.ToString(Form1.Get_TeamRed.Get_Team("red").Last.Sp_ATK))
            pokeinfo.SubItems.Add(Convert.ToString(Form1.Get_TeamRed.Get_Team("red").Last.Sp_DEF))
            pokeinfo.SubItems.Add(Convert.ToString(Form1.Get_TeamRed.Get_Team("red").Last.SPD))
            pokeinfo.SubItems.Add(Form1.Get_TeamRed.Get_Team("red").Last.Ability.First.Name)
            TeamRed_List.Update()
            TeamRed_List.EndUpdate()
        Else
            MessageBox.Show("Something unexpected happen when adding a pokemon. The pokemon is probably not displaying on the List.", "Check Up!", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End If

        'If InsertPokemonFunct.ShowDialog() = Windows.Forms.DialogResult.OK Then
        '    If InsertPokemonFunct.LastAdded_Pokemon = "blue" Then
        '        Dim pokeinfo As ListViewItem
        '        REM display the name of the last pokemon added
        '        pokeinfo = TeamBlue_List.Items.Add(Form1.Get_TeamBlue.Get_Team("blue").Last.Name)
        '    End If
        'End If



    End Sub
End Class