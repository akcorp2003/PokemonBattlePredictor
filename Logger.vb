REM Logger is NOT thread safe!!!
Module Logger
    Dim m_battlelist1 As New List(Of String)
    Dim m_battlelist2 As New List(Of String)
    Dim m_mute As Boolean
    Dim curr_recording As String


    Public Sub InitializeRecord()
        curr_recording = "1"
        m_mute = False

        If m_battlelist1.Count > 0 Then
            m_battlelist1.Clear()
        End If
        If m_battlelist2.Count > 0 Then
            m_battlelist2.Clear()
        End If
    End Sub

    Public Sub Record(ByVal data As String)
        m_battlelist1.Add(data)
    End Sub

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
