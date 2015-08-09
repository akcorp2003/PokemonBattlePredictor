REM Logger is NOT thread safe!!!
Module Logger
    Dim m_battlelist1 As New List(Of String)
    Dim m_battlelist2 As New List(Of String)
    Dim m_mute As Boolean
    Dim curr_recording As String = "first"


    Public Sub InitializeRecord()

        curr_recording = "1"
        m_mute = False

        If m_battlelist1.Count > 0 Then
            m_battlelist1.Clear()
        End If

        If curr_recording = "first" Then
            If m_battlelist2.Count > 0 Then
                m_battlelist2.Clear()
            End If
        Else
            REM for those folks who are lazy to call Prepare_NextRecord()
            Prepare_NextRecord()
        End If


    End Sub

    Public Sub Record(ByVal data As String)
        m_battlelist1.Add(data)
    End Sub

    ''' <summary>
    ''' Records a basic description of the current battling pokemon and their HP's
    ''' </summary>
    ''' <param name="arena"></param>
    ''' <remarks></remarks>
    Public Sub Record_CurrentArenaInfo(ByVal arena As Pokemon_Arena)
        Record("BEGIN TEAM BLUE")
        Record("B")
        Record("Current Battling Blue: " + arena.CurrentBattlingBlue.First.Name)
        Record("HP: " + Convert.ToString(arena.CurrentBattlingBlue.First.HP))
        Record("HP color: " + arena.Get_HealthStatusofPokemon(arena.CurrentBattlingBlue.First))
        Dim statusstring_blue As String = Get_StatusString(arena.CurrentBattlingBlue.First)
        Record("Status: " + statusstring_blue)

        Dim blueteam As Pokemon_Team = arena.Team_Blue
        For i As Integer = 0 To blueteam.Get_Team("blue").Count - 1 Step 1
            If Not blueteam.Get_Team("blue").Item(i).Name = arena.CurrentBattlingBlue.First.Name Then
                Record("ONE POKEMON")
                Record("Blue team #" + Convert.ToString(i + 1) + ": " + blueteam.Get_Team("blue").Item(i).Name)
                Record("HP: " + Convert.ToString(blueteam.Get_Team("blue").Item(i).HP))
                Record("HP color: " + arena.Get_HealthStatusofPokemon(blueteam.Get_Team("blue").Item(i)))
                Dim statusstring As String = Get_StatusString(blueteam.Get_Team("blue").Item(i))
                Record("Status: " + statusstring)
                Record("END ONE POKEMON")
            End If
        Next
        Record("END TEAM BLUE")

        Record("BEGIN TEAM RED")
        Record("B")
        Record("Current Battling Red: " + arena.CurrentBattlingRed.First.Name)
        Record("HP: " + Convert.ToString(arena.CurrentBattlingRed.First.HP))
        Record("HP color: " + arena.Get_HealthStatusofPokemon(arena.CurrentBattlingRed.First))
        Dim statusstring_red As String = Get_StatusString(arena.CurrentBattlingRed.First)
        Record("Status: " + statusstring_red)

        Dim redteam As Pokemon_Team = arena.Team_Red
        For i As Integer = 0 To redteam.Get_Team("red").Count - 1 Step 1
            If Not redteam.Get_Team("red").Item(i).Name = arena.CurrentBattlingRed.First.Name Then
                Record("ONE POKEMON")
                Record("Red team #" + Convert.ToString(i + 1) + ": " + redteam.Get_Team("red").Item(i).Name)
                Record("HP: " + Convert.ToString(redteam.Get_Team("red").Item(i).HP))
                Record("HP color: " + arena.Get_HealthStatusofPokemon(redteam.Get_Team("red").Item(i)))
                Dim statusstring As String = Get_StatusString(redteam.Get_Team("red").Item(i))
                Record("Status: " + statusstring)
                Record("END ONE POKEMON")
            End If
        Next
        Record("END TEAM RED")

    End Sub

    Private Function Get_StatusString(ByVal pokemon As Pokemon) As String
        Dim statusstring As String
        If pokemon.Status_Condition = Constants.StatusCondition.none Then
            statusstring = "none"
        ElseIf pokemon.Status_Condition = Constants.StatusCondition.attracted Then
            statusstring = "attracted"
        ElseIf pokemon.Status_Condition = Constants.StatusCondition.badly_poisoned Then
            statusstring = "badly poisoned"
        ElseIf pokemon.Status_Condition = StatusCondition.burn Then
            statusstring = "burned"
        ElseIf pokemon.Status_Condition = StatusCondition.freeze Then
            statusstring = "frozen"
        ElseIf pokemon.Status_Condition = StatusCondition.paralyzed Then
            statusstring = "paralyzed"
        ElseIf pokemon.Status_Condition = StatusCondition.poison Then
            statusstring = "poisoned"
        ElseIf pokemon.Status_Condition = StatusCondition.sleep Then
            statusstring = "sleeping"
        Else
            statusstring = "none"
        End If
        If pokemon.Other_Status_Condition = StatusCondition.confused Then
            statusstring += ", confused"
        End If
        Return statusstring
    End Function

    ''' <summary>
    ''' Return the last recording list information and 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Prepare_NextRecord() As List(Of String)
        If curr_recording = "1" Then
            For i As Integer = 0 To m_battlelist1.Count - 1 Step 1
                m_battlelist2(i) = m_battlelist1(i)
            Next
        End If
        m_battlelist1.Clear()
        Return m_battlelist2
    End Function

    Public Sub Set_Mute()
        m_mute = True
    End Sub

    Public Sub Set_Recording()
        m_mute = False
    End Sub

    Public Function isMute() As Boolean
        Return m_mute
    End Function

    Public Function get_CurrentRecording() As String
        Return curr_recording
    End Function

    Public Function get_Log1() As List(Of String)
        Return m_battlelist1
    End Function

    Public Function get_Log2() As List(Of String)
        Return m_battlelist2
    End Function
End Module
