Imports Novacode
Imports System
Imports System.Net
Imports System.IO
Imports System.Drawing
Imports Microsoft.VisualBasic.FileIO.TextFieldParser

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

    Public Sub PrintPokemonInfo(ByVal calling_form As Form1)
        Dim pokemonfile As String = "pokemondictionary_formatted.txt"

        If My.Computer.FileSystem.FileExists(pokemonfile) = True Then
            Try
                My.Computer.FileSystem.DeleteFile(pokemonfile)
            Catch ex As Exception
                MessageBox.Show("Mysteriously unable to delete pokemondictionary_formatted.txt. Try checking if you have it open", _
                                "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End Try
        End If


        Dim filewriter As StreamWriter = Nothing
        Try
            filewriter = My.Computer.FileSystem.OpenTextFileWriter(pokemonfile, False)
        Catch ex As Exception
            MessageBox.Show("Could not open pokemondictionary_formatted.txt for writing...", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        REM begin taking information and writing it all out
        Dim poke_dictionary As Dictionary(Of String, Pokemon) = calling_form.Get_PokemonDictionary.Get_Dictionary
        Dim poke_enum As New Dictionary(Of String, Pokemon).Enumerator
        poke_enum = poke_dictionary.GetEnumerator()
        poke_enum.MoveNext()

        Dim i As Integer = 0
        While i < poke_dictionary.Count
            filewriter.WriteLine("POKEMON")

            filewriter.WriteLine("NAME")
            filewriter.WriteLine(poke_enum.Current.Value.Name)
            filewriter.WriteLine("ATK")
            filewriter.WriteLine(Convert.ToString(poke_enum.Current.Value.ATK))
            filewriter.WriteLine("DEF")
            filewriter.WriteLine(Convert.ToString(poke_enum.Current.Value.DEF))
            filewriter.WriteLine("SP_ATK")
            filewriter.WriteLine(Convert.ToString(poke_enum.Current.Value.Sp_ATK))
            filewriter.WriteLine("SP_DEF")
            filewriter.WriteLine(Convert.ToString(poke_enum.Current.Value.Sp_DEF))
            filewriter.WriteLine("HP")
            filewriter.WriteLine(Convert.ToString(poke_enum.Current.Value.HP))
            filewriter.WriteLine("SPD")
            filewriter.WriteLine(Convert.ToString(poke_enum.Current.Value.SPD))

            filewriter.WriteLine("BEGIN ABILITY")
            Dim num_abilities As Integer = poke_enum.Current.Value.Ability.Count
            Dim ability_enum As New List(Of Ability_Info).Enumerator
            ability_enum = poke_enum.Current.Value.Ability.GetEnumerator()
            ability_enum.MoveNext()

            Dim j As Integer = 0
            While j < num_abilities
                filewriter.WriteLine("NAME")
                filewriter.WriteLine(ability_enum.Current.Name)
                filewriter.WriteLine("URI")
                filewriter.WriteLine(ability_enum.Current.URI)
                ability_enum.MoveNext()
                j += 1
            End While
            ability_enum.Dispose()
            filewriter.WriteLine("END ABILITY")

            filewriter.WriteLine("BEGIN MOVES")
            Dim num_moves As Integer = poke_enum.Current.Value.Moves.Count
            Dim move_enum As New List(Of String).Enumerator
            Dim movelist() As String = poke_enum.Current.Value.Moves.ToArray()
            'move_enum.MoveNext()
            Dim k As Integer = 0
            While k < num_moves
                REM we are going to skip over the details. They will be detailed in another file
                filewriter.WriteLine("NAME")
                filewriter.WriteLine(movelist(k))
                'move_enum.MoveNext()
                k += 1
            End While
            move_enum.Dispose()
            filewriter.WriteLine("END MOVES")

            filewriter.WriteLine("BEGIN TYPES")
            Dim num_types As Integer = poke_enum.Current.Value.Types.Count
            Dim num_enum As New List(Of String).Enumerator
            Dim typelist() As String = poke_enum.Current.Value.Types.ToArray()
            'num_enum.MoveNext()
            Dim l As Integer = 0
            While l < num_types
                filewriter.WriteLine(typelist(l))
                'num_enum.MoveNext()
                l += 1
            End While
            num_enum.Dispose()
            filewriter.WriteLine("END TYPES")

            filewriter.WriteLine("END POKEMON")

            poke_enum.MoveNext()

            i += 1
        End While

        filewriter.Close()


    End Sub

    Public Sub PrintAbilityInfo(ByVal calling_form As Form1)
        Dim pokemonfile As String = "ability_formatted.txt"

        If My.Computer.FileSystem.FileExists(pokemonfile) = True Then
            Try
                My.Computer.FileSystem.DeleteFile(pokemonfile)
            Catch ex As Exception
                MessageBox.Show("Mysteriously unable to delete ability_formatted.txt. Try checking if you have it open", _
                                "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End Try
        End If



        Dim filewriter As StreamWriter
        Try
            filewriter = My.Computer.FileSystem.OpenTextFileWriter(pokemonfile, False)
        Catch ex As Exception
            MessageBox.Show("Could not open ability_formatted.txt for writing...", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Dim abilitydictionary As Dictionary(Of String, Ability_Info) = calling_form.Get_AbilityDictionary.Get_AbilityDictionary()
        Dim ability_enum As New Dictionary(Of String, Ability_Info).Enumerator
        ability_enum = abilitydictionary.GetEnumerator()
        ability_enum.MoveNext()
        Dim ability_count As Integer = abilitydictionary.Count

        Dim i As Integer = 0
        While i < ability_count
            filewriter.WriteLine("ABILITY")
            filewriter.WriteLine("NAME")
            filewriter.WriteLine(ability_enum.Current.Value.Name)
            filewriter.WriteLine("URI")
            filewriter.WriteLine(ability_enum.Current.Value.URI)
            filewriter.WriteLine("END ABILITY")

            ability_enum.MoveNext()

            i += 1
        End While

        filewriter.Close()

    End Sub

    Public Sub PrintMoveInfo(ByVal calling_form As Form1)
        Dim pokemonfile As String = "moves_formatted.txt"

        If My.Computer.FileSystem.FileExists(pokemonfile) = True Then
            Try
                My.Computer.FileSystem.DeleteFile(pokemonfile)
            Catch ex As Exception
                MessageBox.Show("Mysteriously unable to delete moves_formatted.txt. Try checking if you have it open", _
                                "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End Try
        End If



        Dim filewriter As StreamWriter
        Try
            filewriter = My.Computer.FileSystem.OpenTextFileWriter(pokemonfile, False)
        Catch ex As Exception
            MessageBox.Show("Could not open moves_formatted.txt for writing...", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Dim movecount As Integer = calling_form.Get_MoveDictionary.Get_MoveDictionary.Count
        Dim move_enum As Dictionary(Of String, Move_Info).Enumerator
        move_enum = calling_form.Get_MoveDictionary.Get_MoveDictionary.GetEnumerator()
        move_enum.MoveNext()

        Dim i As Integer = 0
        While i < movecount
            filewriter.WriteLine("MOVE")
            filewriter.WriteLine("NAME")
            filewriter.WriteLine(move_enum.Current.Value.Name)
            filewriter.WriteLine("ACCURACY")
            filewriter.WriteLine(Convert.ToString(move_enum.Current.Value.Accuracy))
            filewriter.WriteLine("POWER")
            filewriter.WriteLine(Convert.ToString(move_enum.Current.Value.Power))
            filewriter.WriteLine("PP")
            filewriter.WriteLine(Convert.ToString(move_enum.Current.Value.PP))
            filewriter.WriteLine("URI")
            filewriter.WriteLine(move_enum.Current.Value.URI)
            filewriter.WriteLine("SPECIAL")
            filewriter.WriteLine(Convert.ToString(move_enum.Current.Value.Is_Special))

            filewriter.WriteLine("EFFECT")
            If move_enum.Current.Value.Effect Is Nothing Then
                filewriter.WriteLine() REM write an empty string, we can fill it in in the file
            Else
                filewriter.WriteLine(move_enum.Current.Value.Effect)
            End If

            filewriter.WriteLine("TYPE")
            If move_enum.Current.Value.Type Is Nothing Then
                filewriter.WriteLine() REM write an empty string, we can fill it in in the file
            Else
                filewriter.WriteLine(move_enum.Current.Value.Type)
            End If
            filewriter.WriteLine("END MOVE")

            move_enum.MoveNext()
            i += 1
        End While

        filewriter.Close()
    End Sub

    Public Sub PrintLogger(ByVal arena As Pokemon_Arena)
        Dim filename As String = "battlelog.docx"
        Dim newfile As DocX = DocX.Create(filename)

        Dim title_format As New Formatting

        title_format.Bold = True
        title_format.UnderlineColor = Color.Black
        title_format.FontColor = Color.Black
        title_format.FontFamily = New FontFamily("Calibri")
        title_format.Size = 16.0
        Dim titlepara As Paragraph = newfile.InsertParagraph("Complete Battle Log", False, title_format)
        titlepara.Alignment = Alignment.center

        Dim bluetableformat As Formatting = Get_TableHeadingFormat()
        newfile.InsertParagraph("Team Blue", False, bluetableformat)

        REM first print Pokemon information
        REM Team Blue:
        Dim blue_table As Table = newfile.AddTable(8, 4)
        blue_table.Design = TableDesign.ColorfulGridAccent5
        blue_table.Rows(0).Cells(0).Paragraphs.First().Append("Name")
        blue_table.Rows(1).Cells(0).Paragraphs.First().Append("Type(s)")
        blue_table.Rows(2).Cells(0).Paragraphs.First().Append("HP")
        blue_table.Rows(3).Cells(0).Paragraphs.First().Append("ATK")
        blue_table.Rows(4).Cells(0).Paragraphs.First().Append("DEF")
        blue_table.Rows(5).Cells(0).Paragraphs.First().Append("SP.ATK")
        blue_table.Rows(6).Cells(0).Paragraphs.First().Append("SP.DEF")
        blue_table.Rows(7).Cells(0).Paragraphs.First().Append("SPEED")

        For i As Integer = 0 To arena.Get_TeamBlue.Get_Team("blue").Count - 1 Step 1
            Dim pokemon As Pokemon = arena.Get_TeamBlue.Get_Team("blue").Item(i)

            REM perform steps to add image to file
            Dim poke_img As Novacode.Image = newfile.AddImage("sprites\" + Constants.Get_FormattedString(pokemon.Name) + ".bmp")
            Dim poke_pic As Novacode.Picture = poke_img.CreatePicture()
            blue_table.Rows(0).Cells(i + 1).Paragraphs.First().InsertPicture(poke_pic)
            blue_table.Rows(0).Cells(i + 1).InsertParagraph(pokemon.Name)

            For j As Integer = 0 To pokemon.Types.Count - 1 Step 1
                blue_table.Rows(1).Cells(i + 1).Paragraphs.First().Append(pokemon.Types(j))
                If j < pokemon.Types.Count - 1 Then
                    blue_table.Rows(1).Cells(i + 1).Paragraphs.First().Append(",")
                End If
            Next
            blue_table.Rows(2).Cells(i + 1).Paragraphs.First().Append(Convert.ToString(pokemon.HP))
            blue_table.Rows(3).Cells(i + 1).Paragraphs.First().Append(Convert.ToString(pokemon.ATK))
            blue_table.Rows(4).Cells(i + 1).Paragraphs.First().Append(Convert.ToString(pokemon.DEF))
            blue_table.Rows(5).Cells(i + 1).Paragraphs.First().Append(Convert.ToString(pokemon.Sp_ATK))
            blue_table.Rows(6).Cells(i + 1).Paragraphs.First().Append(Convert.ToString(pokemon.Sp_DEF))
            blue_table.Rows(7).Cells(i + 1).Paragraphs.First().Append(Convert.ToString(pokemon.SPD))
        Next
        newfile.InsertTable(blue_table)

        Me.PrintMoveInformation(newfile, arena, "blue")

        Dim redtableformat As Formatting = Get_TableHeadingFormat()
        newfile.InsertParagraph("Team Red", False, redtableformat)
        REM Team Red:
        Dim red_table As Table = newfile.AddTable(8, 4)
        red_table.Design = TableDesign.ColorfulGridAccent5
        red_table.Rows(0).Cells(0).Paragraphs.First().Append("Name")
        red_table.Rows(1).Cells(0).Paragraphs.First().Append("Type(s)")
        red_table.Rows(2).Cells(0).Paragraphs.First().Append("HP")
        red_table.Rows(3).Cells(0).Paragraphs.First().Append("ATK")
        red_table.Rows(4).Cells(0).Paragraphs.First().Append("DEF")
        red_table.Rows(5).Cells(0).Paragraphs.First().Append("SP.ATK")
        red_table.Rows(6).Cells(0).Paragraphs.First().Append("SP.DEF")
        red_table.Rows(7).Cells(0).Paragraphs.First().Append("SPEED")

        For i As Integer = 0 To arena.Get_TeamRed.Get_Team("red").Count - 1 Step 1
            Dim pokemon As Pokemon = arena.Get_TeamRed.Get_Team("red").Item(i)

            REM perform steps to create picture
            Dim poke_img As Novacode.Image = newfile.AddImage("sprites\" + Constants.Get_FormattedString(pokemon.Name) + ".bmp")
            Dim poke_pic As Novacode.Picture = poke_img.CreatePicture()
            red_table.Rows(0).Cells(i + 1).Paragraphs.First().InsertPicture(poke_pic)
            red_table.Rows(0).Cells(i + 1).InsertParagraph(pokemon.Name)

            For j As Integer = 0 To pokemon.Types.Count - 1 Step 1
                red_table.Rows(1).Cells(i + 1).Paragraphs.First.Append(pokemon.Types(j))
                If j < pokemon.Types.Count - 1 Then
                    red_table.Rows(1).Cells(i + 1).Paragraphs.First.Append(",")
                End If
            Next
            red_table.Rows(2).Cells(i + 1).Paragraphs.First().Append(Convert.ToString(pokemon.HP))
            red_table.Rows(3).Cells(i + 1).Paragraphs.First().Append(Convert.ToString(pokemon.ATK))
            red_table.Rows(4).Cells(i + 1).Paragraphs.First().Append(Convert.ToString(pokemon.DEF))
            red_table.Rows(5).Cells(i + 1).Paragraphs.First().Append(Convert.ToString(pokemon.Sp_ATK))
            red_table.Rows(6).Cells(i + 1).Paragraphs.First().Append(Convert.ToString(pokemon.Sp_DEF))
            red_table.Rows(7).Cells(i + 1).Paragraphs.First().Append(Convert.ToString(pokemon.SPD))
        Next
        newfile.InsertTable(red_table)

        Me.PrintMoveInformation(newfile, arena, "red")


        Dim my_format As Formatting = Get_NormalTextFormat()

        newfile.InsertParagraph(Environment.NewLine, False, my_format)
        For i As Integer = 0 To Logger.get_Log1().Count - 1 Step 1
            If Logger.get_Log1.Item(i) = "BEGIN BATTLE" Then
                my_format.UnderlineColor = Color.Black
                Dim para As Paragraph = newfile.InsertParagraph("The battle begins!!", False, my_format)
                para.Alignment = Alignment.center
                Reset_Formatting(my_format)
                Continue For
            End If

            If Logger.get_Log1.Item(i) = "END BATTLE" Then
                newfile.InsertParagraph(Environment.NewLine, False, my_format)
                Continue For
            End If

            If Logger.get_Log1.Item(i) = "BEGIN TEMP INFO" Then
                newfile.InsertParagraph("-------Arena Status-------", False, my_format)
                i = PrintArenaCondition(i + 1, newfile, arena)
                i -= 1 REM off by one, PrintArenaCondition() returns the index to END TEMP INFO, now we need to pull it back by one
                Continue For
            End If

            If Logger.get_Log1.Item(i) = "END TEMP INFO" Then
                newfile.InsertParagraph("-------Arena Status Completed-------", False, my_format)
                newfile.InsertParagraph(Environment.NewLine, False, my_format)
                Continue For
            End If

            If Logger.get_Log1.Item(i) = "FAINT" Then
                newfile.InsertParagraph(Environment.NewLine, False, my_format)
                Continue For
            End If

            If Logger.get_Log1().Item(i).Contains(";") Or Logger.get_Log1().Item(i).Length = 1 Then
                Get_Formatting(Logger.get_Log1().Item(i), my_format)
                newfile.InsertParagraph(Logger.get_Log1().Item(i + 1), False, my_format)
                i += 1
                Reset_Formatting(my_format)
                Continue For
            End If

            If Logger.get_Log1().Item(i).Contains("END CYCLE") Then
                newfile.InsertParagraph(Environment.NewLine, False, my_format)
                Continue For
            End If

            If Logger.get_Log1().Item(i).Contains("winning") Then
                my_format.Position = 36
                If Logger.get_Log1().Item(i).Contains("red") Then
                    my_format.FontColor = Color.Red
                ElseIf Logger.get_Log1().Item(i).Contains("blue") Then
                    my_format.FontColor = Color.Blue
                End If
                newfile.InsertParagraph(Logger.get_Log1().Item(i), False, my_format)
                REM this should be the last line
                Reset_Formatting(my_format)
                Continue For
            End If

            REM write the line normally
            newfile.InsertParagraph(Logger.get_Log1().Item(i), False, my_format)

            Reset_Formatting(my_format)

        Next

        newfile.Save()

    End Sub

    Private Sub PrintMoveInformation(ByRef newfile As DocX, ByVal arena As Pokemon_Arena, ByVal team As String)
        If team = "blue" Then
            Dim bluemoves As Formatting = Get_TableHeadingFormat()
            newfile.InsertParagraph("Blue Team Move Information", False, bluemoves)

            For i As Integer = 0 To arena.Team_Blue.Get_Team("blue").Count - 1 Step 1
                Dim bluetable As Table = newfile.AddTable(7, 5)
                bluetable.Design = TableDesign.LightGridAccent1
                Dim blueteam As List(Of Pokemon) = arena.Team_Blue.Get_Team("blue")
                bluetable.Rows(0).Cells(0).Paragraphs.First().Append(blueteam(i).Name)
                Dim namerow As Row = bluetable.Rows(0)
                namerow.MergeCells(0, 4)
                Dim paragraphs As List(Of Paragraph) = bluetable.Rows(0).Paragraphs
                For p As Integer = 1 To paragraphs.Count - 1 Step 1
                    paragraphs(p).Remove(False)
                Next
                bluetable.Rows(1).Cells(0).Paragraphs.First().Append("Name:")
                bluetable.Rows(2).Cells(0).Paragraphs.First().Append("Power:")
                bluetable.Rows(3).Cells(0).Paragraphs.First().Append("PP:")
                bluetable.Rows(4).Cells(0).Paragraphs.First().Append("Accuracy:")
                bluetable.Rows(5).Cells(0).Paragraphs.First().Append("Type:")
                bluetable.Rows(6).Cells(0).Paragraphs.First().Append("Category:")

                REM now fill in move information
                Dim move_enum As New List(Of Move_Info).Enumerator
                move_enum = blueteam(i).Moves_For_Battle.GetEnumerator()
                move_enum.MoveNext()
                For j As Integer = 0 To blueteam(i).Moves_For_Battle.Count - 1 Step 1
                    bluetable.Rows(1).Cells(j + 1).Paragraphs.First.Append(move_enum.Current.Name)
                    bluetable.Rows(2).Cells(j + 1).Paragraphs.First.Append(Convert.ToString(move_enum.Current.Power))
                    bluetable.Rows(3).Cells(j + 1).Paragraphs.First.Append(Convert.ToString(move_enum.Current.PP))
                    bluetable.Rows(4).Cells(j + 1).Paragraphs.First.Append(Convert.ToString(move_enum.Current.Accuracy))
                    bluetable.Rows(5).Cells(j + 1).Paragraphs.First.Append(move_enum.Current.Type)
                    If move_enum.Current.Is_Special() Then
                        bluetable.Rows(6).Cells(j + 1).Paragraphs.First.Append("Special")
                    ElseIf move_enum.Current.Power = 0 Then
                        bluetable.Rows(6).Cells(j + 1).Paragraphs.First.Append("Status")
                    Else
                        bluetable.Rows(6).Cells(j + 1).Paragraphs.First.Append("Physical")
                    End If
                    move_enum.MoveNext()
                Next
                newfile.InsertTable(bluetable)
                newfile.InsertParagraph(Environment.NewLine, False)
            Next



        Else
            Dim redtableformat As Formatting = Get_TableHeadingFormat()
            newfile.InsertParagraph("Red Team Move Information", False, redtableformat)

            For i As Integer = 0 To arena.Team_Red.Get_Team("red").Count - 1 Step 1
                Dim redtable As Table = newfile.AddTable(7, 5)
                redtable.Design = TableDesign.LightGridAccent5
                Dim redteam As List(Of Pokemon) = arena.Team_Red.Get_Team("red")
                redtable.Rows(0).Cells(0).Paragraphs.First().Append(redteam(i).Name)
                Dim namerow As Row = redtable.Rows(0)
                namerow.MergeCells(0, 4)
                REM mergecells creates extraneous paragraphs that we don't need
                Dim paragraphs As List(Of Paragraph) = redtable.Rows(0).Paragraphs
                For p As Integer = 1 To paragraphs.Count - 1 Step 1
                    paragraphs(p).Remove(False)
                Next
                redtable.Rows(1).Cells(0).Paragraphs.First().Append("Name:")
                redtable.Rows(2).Cells(0).Paragraphs.First().Append("Power:")
                redtable.Rows(3).Cells(0).Paragraphs.First().Append("PP:")
                redtable.Rows(4).Cells(0).Paragraphs.First().Append("Accuracy:")
                redtable.Rows(5).Cells(0).Paragraphs.First().Append("Type:")
                redtable.Rows(6).Cells(0).Paragraphs.First().Append("Category:")

                REM now fill in move information
                Dim move_enum As New List(Of Move_Info).Enumerator
                move_enum = redteam(i).Moves_For_Battle.GetEnumerator()
                move_enum.MoveNext()
                For j As Integer = 0 To redteam(i).Moves_For_Battle.Count - 1 Step 1
                    redtable.Rows(1).Cells(j + 1).Paragraphs.First.Append(move_enum.Current.Name)
                    redtable.Rows(2).Cells(j + 1).Paragraphs.First.Append(Convert.ToString(move_enum.Current.Power))
                    redtable.Rows(3).Cells(j + 1).Paragraphs.First.Append(Convert.ToString(move_enum.Current.PP))
                    redtable.Rows(4).Cells(j + 1).Paragraphs.First.Append(Convert.ToString(move_enum.Current.Accuracy))
                    redtable.Rows(5).Cells(j + 1).Paragraphs.First.Append(move_enum.Current.Type)
                    If move_enum.Current.Is_Special() Then
                        redtable.Rows(6).Cells(j + 1).Paragraphs.First.Append("Special")
                    ElseIf move_enum.Current.Power = 0 Then
                        redtable.Rows(6).Cells(j + 1).Paragraphs.First.Append("Status")
                    Else
                        redtable.Rows(6).Cells(j + 1).Paragraphs.First.Append("Physical")
                    End If
                    move_enum.MoveNext()
                Next
                newfile.InsertTable(redtable)
                newfile.InsertParagraph(Environment.NewLine, False)
            Next
        End If
    End Sub

    Private Function PrintArenaCondition(ByVal start_index As Integer, ByRef newfile As DocX, ByVal arena As Pokemon_Arena) As Integer
        Dim ending_index As Integer = start_index
        Dim my_format As Formatting = Get_NormalTextFormat()
        Dim workingon_red As Boolean = False
        Dim workingon_blue As Boolean = False

        For i As Integer = start_index To Logger.get_Log1.Count - 1 Step 1
            ending_index = i
            If Logger.get_Log1.Item(i) = "BEGIN TEAM BLUE" Then
                workingon_blue = True
                workingon_red = False
                Continue For
            End If

            If Logger.get_Log1.Item(i) = "END TEAM BLUE" Then
                workingon_blue = False
                Continue For
            End If

            If Logger.get_Log1.Item(i) = "BEGIN TEAM RED" Then
                workingon_blue = False
                workingon_red = True
                Continue For
            End If

            If Logger.get_Log1.Item(i) = "END TEAM RED" Then
                REM by convention, this is the last one
                ending_index = i
                Exit For
            End If

            If Logger.get_Log1().Item(i).Contains(";") Or Logger.get_Log1().Item(i).Length = 1 Then
                Get_Formatting(Logger.get_Log1().Item(i), my_format)
                newfile.InsertParagraph(Logger.get_Log1().Item(i + 1), False, my_format)
                i += 1
                Reset_Formatting(my_format)
                Continue For
            End If

            If Logger.get_Log1().Item(i).Contains("green") OrElse Logger.get_Log1().Item(i).Contains("yellow") OrElse Logger.get_Log1().Item(i).Contains("red") Then
                If Logger.get_Log1().Item(i).Contains("green") Then
                    my_format.FontColor = Color.Green
                ElseIf Logger.get_Log1().Item(i).Contains("yellow") Then
                    my_format.FontColor = Color.Yellow
                ElseIf Logger.get_Log1().Item(i).Contains("red") Then
                    my_format.FontColor = Color.Red
                Else
                    my_format.FontColor = Color.Green
                End If
                newfile.InsertParagraph(Logger.get_Log1().Item(i), False, my_format)
                Reset_Formatting(my_format)
                Continue For
            End If

            If Logger.get_Log1().Item(i).Contains("ONE POKEMON") Then
                i += 1 REM move to the next item which begins the important things
                Dim n_columns As Integer
                If workingon_blue Then
                    n_columns = arena.Get_TeamBlue.Get_Team("blue").Count - 1
                Else
                    n_columns = arena.Get_TeamRed.Get_Team("red").Count - 1
                End If

                Dim infotable As Table = newfile.AddTable(4, n_columns)
                infotable.Design = TableDesign.ColorfulListAccent5
                For j As Integer = 0 To n_columns - 1 Step 1
                    REM only be true for the iteration where j >=1 
                    If Logger.get_Log1().Item(i).Contains("ONE POKEMON") Then
                        i += 1
                    End If

                    infotable.Rows(0).Cells(j).Paragraphs.First.Append(Logger.get_Log1.Item(i)) 'Blue/Red Team #
                    infotable.Rows(1).Cells(j).Paragraphs.First.Append(Logger.get_Log1.Item(i + 1)) 'HP:
                    infotable.Rows(2).Cells(j).Paragraphs.First.Append(Logger.get_Log1.Item(i + 2)) 'HP color:
                    REM apply a color to the paragraph
                    If Logger.get_Log1.Item(i + 2) = "red" Then
                        infotable.Rows(2).Cells(j).Paragraphs.First.Color(Color.Red)
                    ElseIf Logger.get_Log1.Item(i + 2) = "yellow" Then
                        infotable.Rows(2).Cells(j).Paragraphs.First.Color(Color.Yellow)
                    Else
                        infotable.Rows(2).Cells(j).Paragraphs.First.Color(Color.Green)
                    End If
                    infotable.Rows(3).Cells(j).Paragraphs.First.Append(Logger.get_Log1.Item(i + 3)) 'Status:

                    i += 4
                    If Logger.get_Log1.Item(i) = "END ONE POKEMON" Then
                        i += 1
                        Continue For
                    End If
                Next
                newfile.InsertTable(infotable)
                newfile.InsertParagraph(Environment.NewLine, False)
                Continue For
            End If

            REM just insert the paragraph normally
            newfile.InsertParagraph(Logger.get_Log1().Item(i), False, my_format)
        Next

        Return ending_index
    End Function

    Private Sub Get_Formatting(ByVal properties As String, ByVal format As Formatting)
        Dim formatting As String() = properties.Split(";")
        For i As Integer = 0 To formatting.Length - 1 Step 1
            If formatting(i) = "B" Then
                format.Bold = True
            End If
            If formatting(i) = "I" Then
                format.Italic = True
            End If
        Next
    End Sub

    Private Sub Reset_Formatting(ByVal format As Formatting)
        format.Bold = False
        format.Italic = False
        format.UnderlineColor = Color.White
        format.FontColor = Color.Black
    End Sub

    Private Function Get_TableHeadingFormat() As Formatting
        Dim table_headerformat As New Formatting
        table_headerformat.UnderlineColor = Color.Black
        table_headerformat.Size = 12.0
        table_headerformat.FontFamily = New FontFamily("Times New Roman")
        table_headerformat.Position = 12
        Return table_headerformat
    End Function

    Private Function Get_NormalTextFormat() As Formatting
        Dim normaltext As New Formatting
        normaltext.FontFamily = New FontFamily("Times New Roman")
        normaltext.Size = 12.0
        Return normaltext
    End Function


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

Public Class Dex_reader

    Private m_reading_file As String = ""
    Private m_parent_form As Form1

    Public Property Read_File As String
        Get
            Return m_reading_file
        End Get
        Set(value As String)
            m_reading_file = value
        End Set
    End Property

    Public Property Parent_Form As Form1
        Get
            Return m_parent_form
        End Get
        Set(value As Form1)
            m_parent_form = value
        End Set
    End Property

    ''' <summary>
    ''' Begins reading files. The object will determine which function to run based on the filename it holds.
    ''' </summary>
    ''' <param name="worker"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub Execute(ByVal worker As System.ComponentModel.BackgroundWorker,
                                ByVal e As System.ComponentModel.DoWorkEventArgs)
        If Me.Read_File = "pokemondictionary_formatted.txt" Then
            Me.read_pokemon(Me.Parent_Form)
            e.Result = "pokemondictionary_formatted.txt"
        ElseIf Me.Read_File = "moves_formatted.txt" Then
            Me.Read_Moves(Me.Parent_Form)
            e.Result = "moves_formatted.txt"
        ElseIf Me.Read_File = "ability_formatted.txt" Then
            Me.Read_Ability(Me.Parent_Form)
            e.Result = "ability_formatted.txt"
        Else
            MessageBox.Show("A thread couldn't load a file...", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Public Sub read_pokemon(ByVal calling_form As Form1)

        Dim filename As String = "pokemondictionary_formatted.txt"
        Dim filereader As StreamReader
        Try
            filereader = My.Computer.FileSystem.OpenTextFileReader(filename)
        Catch ex As Exception
            MessageBox.Show("Could not load pokemondictionary_formatted.txt. Check to see if it's not being opened somewhere else.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Dim dummypokemon As New Pokemon
        Dim toadd_pokemon As New Pokemon

        Dim moveflag As Boolean = False
        Dim abilityflag As Boolean = False
        Dim typeflag As Boolean = False

        Dim readingline As String = filereader.ReadLine()
        While Not filereader.EndOfStream()
            If readingline = "POKEMON" Then
                REM rip a pokemon out of the dummypokemon
                toadd_pokemon = dummypokemon.Clone()
            ElseIf readingline = "NAME" And moveflag = False And abilityflag = False And typeflag = False Then
                REM we have read a pokemon name
                readingline = filereader.ReadLine()
                toadd_pokemon.Name = readingline
            ElseIf readingline = "ATK" Then
                readingline = filereader.ReadLine()
                toadd_pokemon.ATK = Convert.ToInt32(readingline)
            ElseIf readingline = "DEF" Then
                readingline = filereader.ReadLine()
                toadd_pokemon.DEF = Convert.ToInt32(readingline)
            ElseIf readingline = "SP_ATK" Then
                readingline = filereader.ReadLine()
                toadd_pokemon.Sp_ATK = Convert.ToInt32(readingline)
            ElseIf readingline = "SP_DEF" Then
                readingline = filereader.ReadLine()
                toadd_pokemon.Sp_DEF = Convert.ToInt32(readingline)
            ElseIf readingline = "HP" Then
                readingline = filereader.ReadLine()
                toadd_pokemon.HP = Convert.ToInt32(readingline)
            ElseIf readingline = "SPD" Then
                readingline = filereader.ReadLine()
                toadd_pokemon.SPD = Convert.ToInt32(readingline)

            ElseIf readingline = "BEGIN ABILITY" Then
                REM read the following line just to make sure this pokemon has an ability
                readingline = filereader.ReadLine()
                While Not readingline = "END ABILITY"
                    If readingline = "NAME" Then
                        Dim ability As New Ability_Info
                        ability.Name = filereader.ReadLine() REM this line contains the name
                        readingline = filereader.ReadLine() REM this line contains the word "URI"
                        If Not readingline = "URI" Then
                            MessageBox.Show("There is an error in the pokemondictionary_formatted.txt file. An ability is messed up. We did not complete the reading process", "Whoops!", _
                                            MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return
                        End If
                        readingline = filereader.ReadLine() REM this line should contain the URI address
                        ability.URI = readingline

                        toadd_pokemon.Ability.Add(ability.Clone())
                    Else
                        MessageBox.Show("There is an error in the pokemondictionary_formatted.txt file. An ability is messed up. We did not complete the reading process", "Whoops!", _
                                            MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If

                    readingline = filereader.ReadLine()
                End While

            ElseIf readingline = "BEGIN MOVES" Then
                REM check to make sure the pokemon has some moves
                readingline = filereader.ReadLine()
                While Not readingline = "END MOVES"
                    If readingline = "NAME" Then
                        toadd_pokemon.Moves.Add(filereader.ReadLine()) REM read the next line, since it's the move name
                    Else
                        MessageBox.Show("There is an error in the pokemondictionary_formatted.txt file. A move is messed up. We did not complete the reading process", "Whoops!", _
                                            MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return
                    End If
                    readingline = filereader.ReadLine()
                End While
            ElseIf readingline = "BEGIN TYPES" Then
                readingline = filereader.ReadLine()
                While Not readingline = "END TYPES"
                    REM keep on reading all the lines, since they are all type names
                    toadd_pokemon.Types.Add(readingline)

                    readingline = filereader.ReadLine()
                End While
            ElseIf readingline = "END POKEMON" Then
                REM package the pokemon and add it to the dictionary
                REM this is the important line
                calling_form.Get_PokemonDictionary.Add_Pokemon(toadd_pokemon.Name, toadd_pokemon)
            Else
                MessageBox.Show("Unknown value read in the file... Aborting to be safe. No pokemon have been added to the dictionary.", _
                                "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If

            readingline = filereader.ReadLine()
        End While

        REM our method of implementation will miss out the last pokemon of the stream
        calling_form.Get_PokemonDictionary.Add_Pokemon(toadd_pokemon.Name, toadd_pokemon)

        filereader.Close()
    End Sub

    Public Sub Read_Moves(ByVal calling_form As Form1)

        Dim filename As String = "moves_formatted.txt"
        Dim filereader As StreamReader

        Try
            filereader = My.Computer.FileSystem.OpenTextFileReader(filename)
        Catch ex As Exception
            MessageBox.Show("Could not load moves_formatted.txt. Make sure the file exists or it is not being used by someone else." & Environment.NewLine & _
                            "Don't worry about this because after you close the program moves should be written to the file.", "Whoops!", _
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Dim dummymoveinfo As New Move_Info
        Dim toadd_move As New Move_Info

        Dim readingline As String
        readingline = filereader.ReadLine()
        While Not filereader.EndOfStream

            If readingline = "MOVE" Then
                REM rip a move
                toadd_move = dummymoveinfo.Clone()

            ElseIf readingline = "NAME" Then
                REM we can read everything in one go
                readingline = filereader.ReadLine() REM this line contains the name of the move
                toadd_move.Name = readingline

                readingline = filereader.ReadLine() REM this line contains ACCURACY
                If Not readingline = "ACCURACY" Then
                    MessageBox.Show("There is unknown text in moves_formatted.txt. Aborting to be safe.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                readingline = filereader.ReadLine()
                toadd_move.Accuracy = Convert.ToInt32(readingline)

                readingline = filereader.ReadLine() REM this line contains POWER
                If Not readingline = "POWER" Then
                    MessageBox.Show("There is unknown text in moves_formatted.txt. Aborting to be safe.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                readingline = filereader.ReadLine()
                toadd_move.Power = Convert.ToInt32(readingline)

                readingline = filereader.ReadLine() REM this line should contain PP
                If Not readingline = "PP" Then
                    MessageBox.Show("There is unknown text in moves_formatted.txt. Aborting to be safe.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                readingline = filereader.ReadLine()
                toadd_move.PP = Convert.ToInt32(readingline)

                readingline = filereader.ReadLine() REM this line should contain URI
                If Not readingline = "URI" Then
                    MessageBox.Show("There is unknown text in moves_formatted.txt. Aborting to be safe.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                readingline = filereader.ReadLine()
                toadd_move.URI = readingline

                readingline = filereader.ReadLine() REM this line should contain SPECIAL
                If Not readingline = "SPECIAL" Then
                    MessageBox.Show("There is unknown text in moves_formatted.txt. Aborting to be safe.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                readingline = filereader.ReadLine()
                toadd_move.Is_Special = Convert.ToBoolean(readingline)

                readingline = filereader.ReadLine()
                If Not readingline = "EFFECT" Then
                    MessageBox.Show("There is unknown text in moves_formatted.txt. Aborting to be safe.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                readingline = filereader.ReadLine()
                toadd_move.Effect = readingline

                readingline = filereader.ReadLine() REM this line should contain TYPE
                If Not readingline = "TYPE" Then
                    MessageBox.Show("There is unknown text in moves_formatted.txt. Aborting to be safe.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                toadd_move.Type = filereader.ReadLine()

                readingline = filereader.ReadLine()
                If Not readingline = "END MOVE" Then
                    MessageBox.Show("There is unknown text in moves_formatted.txt. Aborting to be safe.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                Dim movename_key As String = toadd_move.Name.Trim("""")
                calling_form.Get_MoveDictionary.Add_Move(movename_key, toadd_move)
            Else
                MessageBox.Show("There is unknown text in moves_formatted.txt. Aborting to be safe.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            readingline = filereader.ReadLine()
        End While

        filereader.Close()

    End Sub

    Public Sub Read_Ability(ByVal calling_form As Form1)

        Dim filename As String = "ability_formatted.txt"
        Dim filereader As StreamReader
        Try
            filereader = My.Computer.FileSystem.OpenTextFileReader(filename)
        Catch ex As Exception
            MessageBox.Show("Could not load ability_formatted.txt. Make sure the file exists or it is not being used by someone else.", "Whoops!", _
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Dim dummyabilityinfo As New Ability_Info
        Dim toadd_ability As New Ability_Info

        Dim readingline As String = ""
        readingline = filereader.ReadLine()
        While Not filereader.EndOfStream
            If readingline = "ABILITY" Then
                toadd_ability = dummyabilityinfo.Clone()
            ElseIf readingline = "NAME" Then
                readingline = filereader.ReadLine()
                toadd_ability.Name = readingline

                readingline = filereader.ReadLine() REM this line should contain URI
                If Not readingline = "URI" Then
                    MessageBox.Show("There is unknown text in ability_formatted.txt. Aborting to be safe.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                readingline = filereader.ReadLine()
                toadd_ability.URI = readingline

                readingline = filereader.ReadLine()
                If Not readingline = "END ABILITY" Then
                    MessageBox.Show("There is unknown text in moves_formatted.txt. Aborting to be safe.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return
                End If
                REM now we can add the ability
                calling_form.Get_AbilityDictionary.Add_Ability(toadd_ability.Name, toadd_ability)

            Else
                MessageBox.Show("There is unknown text in moves_formatted.txt. Aborting to be safe.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            readingline = filereader.ReadLine()
        End While

        filereader.Close()
    End Sub

    Public Sub Read_MovesCSV()
        Dim move_csv As FileIO.TextFieldParser = New FileIO.TextFieldParser("moves.csv")
        Dim currentLine As String()
        move_csv.Delimiters = New String() {","}
        Dim eff_table As New EffectivenessTable

        REM this should be the line where it's all the headings
        currentLine = move_csv.ReadFields()
        While Not move_csv.EndOfData
            currentLine = move_csv.ReadFields()

            Dim movename As String = currentLine(1)
            Dim type As String = currentLine(3)
            Dim damage_type As String = currentLine(9)
            Dim effect As String = currentLine(10)
            Dim norm_or_special As Integer = Convert.ToInt32(damage_type)
            Dim typename As String = eff_table.GetTypeName(Convert.ToInt32(type) - 1)

            Dim effectclass As Integer = Convert.ToInt32(effect)
            Dim effect_string As String = Constants.Get_EffectString(effectclass, currentLine)

            movename = Capitalizefirstletter(movename)

            Dim modify_move As Move_Info
            modify_move = Form1.Get_MoveDictionary.Get_Move(movename)
            If Not modify_move Is Nothing Then
                modify_move.Type = typename
                modify_move.Effect = effect_string
                If norm_or_special = 3 Then
                    modify_move.Is_Special = True
                Else
                    modify_move.Is_Special = False
                End If
            End If


        End While
        move_csv.Close()
    End Sub

    Public Sub Build_Sprites()
        'Dim base_url As String = "http://pokeapi.co/"
        'Dim full_url As String = base_url + "media/img/1383395659.12.png"
        If Not My.Computer.FileSystem.DirectoryExists("sprites") Then
            Try
                My.Computer.FileSystem.CreateDirectory("sprites")
            Catch ex As Exception
                MessageBox.Show("Could not create directory for sprites!", "Oh no!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End Try
        End If

        Dim base_url_bw As String = "http://img.pokemondb.net/sprites/black-white/normal/"
        Dim base_url_xy As String = "http://img.pokemondb.net/sprites/x-y/normal/"
        Dim base_url_bw2 As String = "http://img.pokemondb.net/sprites/black-white-2/normal/"

        Dim sprite As Bitmap
        Dim poke_dict As Dictionary(Of String, Pokemon) = Form1.Get_PokemonDictionary.Get_Dictionary()
        Dim dict_enum As New Dictionary(Of String, Pokemon).Enumerator
        dict_enum = poke_dict.GetEnumerator()
        dict_enum.MoveNext()

        Dim i As Integer = 0
        While i < poke_dict.Count

            Dim pokemonname As String = Constants.Get_FormattedString(dict_enum.Current.Key)

            REM the weird case of meowstic
            If pokemonname = "meowstic-male" Or pokemonname = "meowstic-female" Then
                If pokemonname = "meowstic-male" Then
                    pokemonname = "meowstic"
                Else
                    pokemonname = "meowstic-f"
                End If
            End If

            Dim full_url As String = base_url_bw + pokemonname + ".png"
            If Not My.Computer.FileSystem.FileExists("sprites\" + pokemonname + ".bmp") Then
                sprite = Form1.RequestImage(full_url)
                If Not sprite Is Nothing Then
                    sprite.Save("sprites/" + pokemonname + ".bmp", Imaging.ImageFormat.Bmp)
                Else
                    REM try black-white2 database
                    Dim full_url3 As String = base_url_bw2 + pokemonname + ".png"
                    sprite = Form1.RequestImage(full_url3)
                    If Not sprite Is Nothing Then
                        sprite.Save("sprites/" + pokemonname + ".bmp", Imaging.ImageFormat.Bmp)
                    Else
                        REM try x-y database as last resort
                        Dim full_url2 As String = base_url_xy + pokemonname + ".png"
                        sprite = Form1.RequestImage(full_url2)
                        If Not sprite Is Nothing Then
                            sprite.Save("sprites/" + pokemonname + ".bmp", Imaging.ImageFormat.Bmp)
                        End If
                    End If

                End If
            End If


            i += 1
            dict_enum.MoveNext()
        End While


    End Sub

    Private Function Capitalizefirstletter(ByVal ugly_string As String) As String
        For Each c As Char In ugly_string
            If Char.IsLower(c) Then
                Dim letter As Char = c
                Dim Sletter As String = letter.ToString()
                Sletter = Sletter.ToUpper()
                ugly_string = ugly_string.Remove(0, 1)
                ugly_string = ugly_string.Insert(0, Sletter)
                Exit For
            End If
        Next
        Return ugly_string
    End Function
End Class
