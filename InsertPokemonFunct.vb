Imports System
Imports System.Drawing
Imports PokemonBattlePredictor
Imports PokemonBattlePredictor.PBP
Imports PokemonBattlePredictor.PBP.InfoBlocks

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

        BattleSetup.Hide()
        Form1.Hide()

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

        REM load the image
        Dim base_path As String = "sprites\"
        Dim full_path As String = base_path + Constants.Get_FormattedString(addingpokemon.Name) + ".bmp"
        Dim sprite As New Bitmap(full_path)
        SpriteBox.Image = sprite
        SpriteBox.SizeMode = PictureBoxSizeMode.AutoSize

    End Sub

    Private Sub AddBlue_Click(sender As Object, e As EventArgs) Handles AddBlue.Click
        Dim bluepokemon As New Pokemon
        Dim dummypokemon As Pokemon

        Dim move1 As New Move_Info
        Dim move2 As New Move_Info
        Dim move3 As New Move_Info
        Dim move4 As New Move_Info

        Dim continue_flag As Boolean = True
        Dim empty_flag As Boolean = False

        bluepokemon = Form1.Get_PokemonDictionary().Get_Pokemon(Me.Pokemon_Name.Text).Clone()

        REM Update the values with the user's values
        bluepokemon.ATK = Me.ATK.Text
        bluepokemon.DEF = Me.DEF.Text
        bluepokemon.Sp_ATK = Me.SpATK.Text
        bluepokemon.Sp_DEF = Me.SpDEF.Text
        bluepokemon.SPD = Me.SPEED.Text
        bluepokemon.HP = Me.HP.Text

        REM secretly update the original pokemon values so the user doesn't need to change them next time.
        dummypokemon = Form1.Get_PokemonDictionary.Get_Pokemon(Me.Pokemon_Name.Text)
        dummypokemon.ATK = Convert.ToInt32(Me.ATK.Text)
        dummypokemon.DEF = Convert.ToInt32(Me.DEF.Text)
        dummypokemon.Sp_ATK = Convert.ToInt32(Me.SpATK.Text)
        dummypokemon.Sp_DEF = Convert.ToInt32(Me.SpDEF.Text)
        dummypokemon.SPD = Convert.ToInt32(Me.SPEED.Text)
        dummypokemon.HP = Convert.ToInt32(Me.HP.Text)

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
            continue_flag = False
        Else
            If Not move1.Name = "" Then
                bluepokemon.Moves_For_Battle.Add(move1)
            End If
        End If
        If move2 Is Nothing Then
            MessageBox.Show("Couldn't find the move " & Me.Move2.Text & ".", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            continue_flag = False
        Else
            If Not move2.Name = "" Then
                bluepokemon.Moves_For_Battle.Add(move2)
            End If
        End If

        If move3 Is Nothing Then
            MessageBox.Show("Couldn't find the move " & Me.Move3.Text & ".", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            continue_flag = False
        Else
            If Not move3.Name = "" Then
                bluepokemon.Moves_For_Battle.Add(move3)
            End If
        End If

        If move4 Is Nothing Then
            MessageBox.Show("Couldn't find the move " & Me.Move4.Text & ".", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            continue_flag = False
        Else
            If Not move4.Name = "" Then
                bluepokemon.Moves_For_Battle.Add(move4)
            End If
        End If

        If ((Not move1 Is Nothing) AndAlso move1.Name = "") OrElse ((Not move2 Is Nothing) AndAlso move2.Name = "") _
            OrElse ((Not move3 Is Nothing) AndAlso move3.Name = "") OrElse ((Not move4 Is Nothing) AndAlso move4.Name = "") Then

            empty_flag = True
            Dim response As DialogResult = MessageBox.Show("You have an empty move. Do you wish to add the pokemon to the team or go back and add some moves?",
                                                           "Missing", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If response = Windows.Forms.DialogResult.No Then
                Form1.Get_PokemonArena.Get_TeamBlue.Addto_Team(bluepokemon.Clone(), "blue")
                m_lastadded_pokemon = "blue"
                Me.Close()

                Form1.Show()
                Return
            End If
        End If

        If Not empty_flag Then
            If Not continue_flag Then
                Dim response As DialogResult = MessageBox.Show("There were some errors in your Pokemon's moves. Would you like to go back and edit them? If not, we will not include those moves in the prediction.",
                                                               "Could not find moves", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If response = Windows.Forms.DialogResult.No Then
                    Form1.Get_PokemonArena.Get_TeamBlue.Addto_Team(bluepokemon.Clone(), "blue")
                    m_lastadded_pokemon = "blue"
                    Me.Close()

                    Form1.Show()
                    Return
                End If
            Else
                Form1.Get_PokemonArena.Get_TeamBlue.Addto_Team(bluepokemon.Clone(), "blue")
                m_lastadded_pokemon = "blue"
                Me.Close()

                Form1.Show()
                Return
            End If
        End If
        
        
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

        returnstring = Change_FirstLetterToUppercase(returnstring)

        Return returnstring

    End Function

    Private Function Change_FirstLetterToUppercase(ByVal my_string As String) As String
        If String.IsNullOrEmpty(my_string) Then
            Return my_string
        End If

        Dim str_array() As Char = my_string.ToArray
        str_array(0) = Char.ToUpper(str_array(0))

        Return New String(str_array)
    End Function


    Private Sub AddRed_Click(sender As Object, e As EventArgs) Handles AddRed.Click
        Dim redpokemon As New Pokemon
        Dim dummypokemon As Pokemon

        Dim move1 As New Move_Info
        Dim move2 As New Move_Info
        Dim move3 As New Move_Info
        Dim move4 As New Move_Info

        Dim continue_flag As Boolean = True
        Dim empty_flag As Boolean = False

        redpokemon = Form1.Get_PokemonDictionary().Get_Pokemon(Me.Pokemon_Name.Text)

        REM Update the values with the user's values
        redpokemon.ATK = Me.ATK.Text
        redpokemon.DEF = Me.DEF.Text
        redpokemon.Sp_ATK = Me.SpATK.Text
        redpokemon.Sp_DEF = Me.SpDEF.Text
        redpokemon.SPD = Me.SPEED.Text
        redpokemon.HP = Me.HP.Text

        REM secretly update the original pokemon values so the user doesn't need to change them next time.
        dummypokemon = Form1.Get_PokemonDictionary.Get_Pokemon(Me.Pokemon_Name.Text)
        dummypokemon.ATK = Convert.ToInt32(Me.ATK.Text)
        dummypokemon.DEF = Convert.ToInt32(Me.DEF.Text)
        dummypokemon.Sp_ATK = Convert.ToInt32(Me.SpATK.Text)
        dummypokemon.Sp_DEF = Convert.ToInt32(Me.SpDEF.Text)
        dummypokemon.SPD = Convert.ToInt32(Me.SPEED.Text)
        dummypokemon.HP = Convert.ToInt32(Me.HP.Text)

        If Not redpokemon.Moves_For_Battle.Count = 0 Then
            REM we want the move list to be fresh
            redpokemon.Moves_For_Battle.Clear()
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
            continue_flag = False
        Else
            If Not move1.Name = "" Then
                redpokemon.Moves_For_Battle.Add(move1)
            End If
        End If
        If move2 Is Nothing Then
            MessageBox.Show("Couldn't find the move " & Me.Move2.Text & ".", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            continue_flag = False
        Else
            If Not move2.Name = "" Then
                redpokemon.Moves_For_Battle.Add(move2)
            End If
        End If

        If move3 Is Nothing Then
            MessageBox.Show("Couldn't find the move " & Me.Move3.Text & ".", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            continue_flag = False
        Else
            If Not move3.Name = "" Then
                redpokemon.Moves_For_Battle.Add(move3)
            End If
        End If

        If move4 Is Nothing Then
            MessageBox.Show("Couldn't find the move " & Me.Move4.Text & ".", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            continue_flag = False
        Else
            If Not move4.Name = "" Then
                redpokemon.Moves_For_Battle.Add(move4)
            End If
        End If

        If ((Not move1 Is Nothing) AndAlso move1.Name = "") OrElse ((Not move2 Is Nothing) AndAlso move2.Name = "") _
            OrElse ((Not move3 Is Nothing) AndAlso move3.Name = "") OrElse ((Not move4 Is Nothing) AndAlso move4.Name = "") Then

            empty_flag = False
            Dim response As DialogResult = MessageBox.Show("You have an empty move. Do you wish to add the pokemon to the team or go back and add some moves?",
                                                           "Missing", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If response = Windows.Forms.DialogResult.No Then
                Form1.Get_PokemonArena.Get_TeamRed.Addto_Team(redpokemon.Clone(), "red")
                m_lastadded_pokemon = "red"
                Me.Close()

                Form1.Show()
                Return
            End If
        End If

        If Not empty_flag Then
            If Not continue_flag Then
                Dim response As DialogResult = MessageBox.Show("There were some errors in your Pokemon's moves. Would you like to go back and edit them? If not, we will not include those moves in the prediction.",
                                                               "Could not find moves", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If response = Windows.Forms.DialogResult.No Then
                    Form1.Get_PokemonArena.Get_TeamRed.Addto_Team(redpokemon.Clone(), "red")
                    m_lastadded_pokemon = "red"
                    Me.Close()

                    Form1.Show()
                    Return
                End If
            Else
                Form1.Get_PokemonArena.Get_TeamRed.Addto_Team(redpokemon.Clone(), "red")
                m_lastadded_pokemon = "red"
                Me.Close()

                Form1.Show()
            End If
        End If



    End Sub

    Private Sub InsertPokemonFunct_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        BattleSetup.UpdateListView()
        BattleSetup.Show()
        BattleSetup.BringToFront()
    End Sub

End Class