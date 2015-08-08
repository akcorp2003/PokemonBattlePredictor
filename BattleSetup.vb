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

        InsertPokemonFunct.Show()
        Me.Hide()
        Form1.Hide()

        

    End Sub

    Private Sub LoadPokemon_Click(sender As Object, e As EventArgs) Handles LoadPokemon.Click
        REM update labels on Form1
        Dim poke_enum1 As List(Of Pokemon).Enumerator
        poke_enum1 = Form1.Get_PokemonArena.Get_TeamBlue().Get_Team("blue").GetEnumerator
        poke_enum1.MoveNext()
        For i = 0 To Form1.Get_PokemonArena.Get_TeamBlue().Get_Team("blue").Count - 1 Step 1
            If i = 0 Then
                Form1.BluePoke_One.Text = poke_enum1.Current.Name
            ElseIf i = 1 Then
                Form1.BluePoke_Two.Text = poke_enum1.Current.Name
            ElseIf i = 2 Then
                Form1.BluePoke_Three.Text = poke_enum1.Current.Name
            Else
                Continue For
            End If
            poke_enum1.MoveNext()
        Next

        Dim poke_enum2 As List(Of Pokemon).Enumerator
        poke_enum2 = Form1.Get_PokemonArena.Get_TeamRed().Get_Team("red").GetEnumerator
        poke_enum2.MoveNext()
        For i = 0 To Form1.Get_PokemonArena.Get_TeamRed().Get_Team("red").Count - 1 Step 1
            If i = 0 Then
                Form1.RedPoke_One.Text = poke_enum2.Current.Name
            ElseIf i = 1 Then
                Form1.RedPoke_Two.Text = poke_enum2.Current.Name
            ElseIf i = 2 Then
                Form1.RedPoke_Three.Text = poke_enum2.Current.Name
            Else
                Continue For
            End If
            poke_enum2.MoveNext()
        Next

        poke_enum1.Dispose()
        poke_enum2.Dispose()

        REM everything was already added. All we need to do is to close this form.
        Dim listItem As ListViewItem
        For i = TeamBlue_List.Items.Count - 1 To 0 Step -1
            listItem = TeamBlue_List.Items(i)
            TeamBlue_List.Items.Remove(listItem)
        Next
        For j = TeamRed_List.Items.Count - 1 To 0 Step -1
            listItem = TeamRed_List.Items(j)
            TeamRed_List.Items.Remove(listItem)
        Next
        Me.Close()

        Form1.LoadImages()
    End Sub

    Private Sub BuildPokemonDictionaryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BuildPokemonDictionaryToolStripMenuItem.Click
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

    Public Sub UpdateListView()
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


    Private Sub BuildMoveDictionaryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BuildMoveDictionaryToolStripMenuItem.Click
        Dim csv_reader As New Dex_reader
        csv_reader.Read_MovesCSV()
        MessageBox.Show("All done building moves!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub BuildImageLibraryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BuildImageLibraryToolStripMenuItem.Click
        Dim image_reader As New Dex_reader
        image_reader.Build_Sprites()
        MessageBox.Show("All done building images!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
End Class