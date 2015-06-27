﻿Public Class InsertPokemonFunct

    Private m_lastadded_pokemon As String
    Private m_finishadding As Boolean = False

    Public ReadOnly Property LastAdded_Pokemon() As String
        Get
            Return m_lastadded_pokemon
        End Get
    End Property

    Public ReadOnly Property Finish_Adding() As Boolean
        Get
            Return m_finishadding
        End Get
    End Property
    
    Private Sub InsertPokemonFunct_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim pokemon_name As String = BattleSetup.Pokemon_Name.Text
        pokemon_name = pokemon_name.Trim()
        pokemon_name = pokemon_name.ToLower
        Dim addingpokemon As Pokemon = Form1.Get_PokemonDictionary().Get_Pokemon(pokemon_name)

        If addingpokemon Is Nothing Then
            Return
        End If

        Me.Pokemon_Name.Text = addingpokemon.Name
        Me.HP.Text = Convert.ToString(addingpokemon.HP)
        Me.ATK.Text = Convert.ToString(addingpokemon.ATK)
        Me.DEF.Text = Convert.ToString(addingpokemon.DEF)
        Me.SpATK.Text = Convert.ToString(addingpokemon.Sp_ATK)
        Me.SpDEF.Text = Convert.ToString(addingpokemon.Sp_DEF)
        Me.SPEED.Text = Convert.ToString(addingpokemon.SPD)
        Me.ABILITY.Text = addingpokemon.Ability.First().Name

        Me.Move1.Text = ""
        Me.Move2.Text = ""
        Me.Move3.Text = ""
        Me.Move4.Text = ""

    End Sub

    Private Sub AddBlue_Click(sender As Object, e As EventArgs) Handles AddBlue.Click
        Dim bluepokemon As New Pokemon

        Dim move1 As New Move_Info
        Dim move2 As New Move_Info
        Dim move3 As New Move_Info
        Dim move4 As New Move_Info

        bluepokemon = Form1.Get_PokemonDictionary().Get_Pokemon(Me.Pokemon_Name.Text).Clone()

        REM Update the values with the user's values
        bluepokemon.ATK = Me.ATK.Text
        bluepokemon.DEF = Me.DEF.Text
        bluepokemon.Sp_ATK = Me.SpATK.Text
        bluepokemon.Sp_DEF = Me.SpDEF.Text
        bluepokemon.SPD = Me.SPEED.Text
        bluepokemon.HP = Me.HP.Text

        If Not bluepokemon.Moves_For_Battle.Count = 0 Then
            REM we want the move list to be fresh
            bluepokemon.Moves_For_Battle.Clear()
        End If

        Dim proper_move1 As String = brushupstring(Me.Move1.Text)
        Dim proper_move2 As String = brushupstring(Me.Move2.Text)
        Dim proper_move3 As String = brushupstring(Me.Move3.Text)
        Dim proper_move4 As String = brushupstring(Me.Move4.Text)

        move1 = Form1.Get_MoveDictionary().Get_Move(proper_move1)
        move2 = Form1.Get_MoveDictionary().Get_Move(proper_move2)
        move3 = Form1.Get_MoveDictionary().Get_Move(proper_move3)
        move4 = Form1.Get_MoveDictionary().Get_Move(proper_move4)

        If move1 Is Nothing Then
            MessageBox.Show("Couldn't find the move " & Me.Move1.Text & ".", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            bluepokemon.Moves_For_Battle.Add(move1)
        End If
        If move2 Is Nothing Then
            MessageBox.Show("Couldn't find the move " & Me.Move2.Text & ".", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            bluepokemon.Moves_For_Battle.Add(move2)
        End If

        If move3 Is Nothing Then
            MessageBox.Show("Couldn't find the move " & Me.Move3.Text & ".", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            bluepokemon.Moves_For_Battle.Add(move3)
        End If

        If move4 Is Nothing Then
            MessageBox.Show("Couldn't find the move " & Me.Move4.Text & ".", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            bluepokemon.Moves_For_Battle.Add(move4)
        End If

        Form1.Get_PokemonArena.Get_TeamBlue.Addto_Team(bluepokemon, "blue")
        m_lastadded_pokemon = "blue"
        Me.Close()
    End Sub

    ''' <summary>
    ''' Replaces all upper case characters after the first uppercase with a lowercase. Also adds a hyphen for every space.
    ''' </summary>
    ''' <param name="tomodify_string"></param>
    ''' <returns>A string with a hyphen and all lowercase except for the first one.</returns>
    ''' <remarks></remarks>
    Private Function brushupstring(ByVal tomodify_string As String) As String
        Dim returnstring As String
        REM first add the hyphen
        returnstring = tomodify_string.Replace(" ", "-")
        Dim seenUpper As Boolean = False
        Dim secondUpperseen As Boolean = False REM for extra security
        For Each c As Char In returnstring
            If Char.IsUpper(c) Then
                If seenUpper = False Then
                    seenUpper = True
                Else
                    returnstring = returnstring.Replace(c, c.ToString.ToLower())
                End If
            End If
        Next

        Return returnstring

    End Function


    Private Sub AddRed_Click(sender As Object, e As EventArgs) Handles AddRed.Click
        Dim redpokemon As New Pokemon

        Dim move1 As New Move_Info
        Dim move2 As New Move_Info
        Dim move3 As New Move_Info
        Dim move4 As New Move_Info

        redpokemon = Form1.Get_PokemonDictionary().Get_Pokemon(Me.Pokemon_Name.Text)

        REM Update the values with the user's values
        redpokemon.ATK = Me.ATK.Text
        redpokemon.DEF = Me.DEF.Text
        redpokemon.Sp_ATK = Me.SpATK.Text
        redpokemon.Sp_DEF = Me.SpDEF.Text
        redpokemon.SPD = Me.SPEED.Text
        redpokemon.HP = Me.HP.Text

        If Not redpokemon.Moves_For_Battle.Count = 0 Then
            REM we want the move list to be fresh
            redpokemon.Moves_For_Battle.Clear()
        End If

        move1 = Form1.Get_MoveDictionary().Get_Move(Me.Move1.Text)
        move2 = Form1.Get_MoveDictionary().Get_Move(Me.Move2.Text)
        move3 = Form1.Get_MoveDictionary().Get_Move(Me.Move3.Text)
        move4 = Form1.Get_MoveDictionary().Get_Move(Me.Move4.Text)

        If move1 Is Nothing Then
            MessageBox.Show("Couldn't find the move " & Me.Move1.Text & ".", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            redpokemon.Moves_For_Battle.Add(move1)
        End If
        If move2 Is Nothing Then
            MessageBox.Show("Couldn't find the move " & Me.Move2.Text & ".", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            redpokemon.Moves_For_Battle.Add(move2)
        End If

        If move3 Is Nothing Then
            MessageBox.Show("Couldn't find the move " & Me.Move3.Text & ".", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            redpokemon.Moves_For_Battle.Add(move3)
        End If

        If move4 Is Nothing Then
            MessageBox.Show("Couldn't find the move " & Me.Move4.Text & ".", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            redpokemon.Moves_For_Battle.Add(move4)
        End If

        Form1.Get_PokemonArena.Get_TeamRed.Addto_Team(redpokemon, "red")
        m_lastadded_pokemon = "red"
        Me.Close()
    End Sub

End Class