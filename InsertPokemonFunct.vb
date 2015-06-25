Public Class InsertPokemonFunct

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

    End Sub

    Private Sub AddBlue_Click(sender As Object, e As EventArgs) Handles AddBlue.Click
        Dim bluepokemon As New Pokemon

        Dim move1 As New Move_Info
        Dim move2 As New Move_Info
        Dim move3 As New Move_Info
        Dim move4 As New Move_Info

        bluepokemon = Form1.Get_PokemonDictionary().Get_Pokemon(Me.Pokemon_Name.Text)

        REM Update the values with the user's values
        bluepokemon.ATK = Me.ATK.Text
        bluepokemon.DEF = Me.DEF.Text
        bluepokemon.Sp_ATK = Me.SpATK.Text
        bluepokemon.Sp_DEF = Me.SpDEF.Text
        bluepokemon.SPD = Me.SPEED.Text

        If Not bluepokemon.Moves_For_Battle.Count = 0 Then
            REM we want the move list to be fresh
            bluepokemon.Moves_For_Battle.Clear()
        End If

        move1 = Form1.Get_MoveDictionary().Get_Move(Me.Move1.Text)
        move2 = Form1.Get_MoveDictionary().Get_Move(Me.Move2.Text)
        move3 = Form1.Get_MoveDictionary().Get_Move(Me.Move3.Text)
        move4 = Form1.Get_MoveDictionary().Get_Move(Me.Move4.Text)

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

        Form1.Get_TeamBlue.Addto_Team(bluepokemon, "blue")
        m_lastadded_pokemon = "blue"
        Me.Close()
    End Sub


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

        Form1.Get_TeamRed.Addto_Team(redpokemon, "red")
        m_lastadded_pokemon = "red"
        Me.Close()
    End Sub

End Class