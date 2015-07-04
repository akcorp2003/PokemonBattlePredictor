Imports System
Imports System.Collections
Imports System.Net
Imports System.IO

Public Class Move_Dictionary
    Private m_move_dictionary As New Dictionary(Of String, Move_Info)

    Public Function Get_MoveDictionary() As Dictionary(Of String, Move_Info)
        Return m_move_dictionary
    End Function

    Public Function IsMoveInDictionary(ByVal move As String) As Boolean
        If m_move_dictionary.ContainsKey(move) = True Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function Get_Move(ByVal move As String) As Move_Info
        Dim toreturn_move As Move_Info = Nothing
        If m_move_dictionary.TryGetValue(move, toreturn_move) = True Then
            Return toreturn_move
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Adds a move to the dictionary
    ''' </summary>
    ''' <param name="move">Does not need to be specifically formatted. The function will handle
    ''' the formattting. FYI: the formatting is "Cut" (no quotes)</param>
    ''' <param name="move_info"></param>
    ''' <remarks>No need to specially format the arguments.</remarks>
    Public Sub Add_Move(ByVal move As String, ByVal move_info As Move_Info)
        move = move.Trim()
        move = move.Trim("""")
        If m_move_dictionary.ContainsKey(move) = True Then
            Return
        Else
            m_move_dictionary.Add(move, move_info)
            Return
        End If
    End Sub
End Class

Public Class Move_Info
    Implements ICloneable

    Private m_name As String
    Private m_accuracy As Integer
    Private m_type As String
    Private m_power As Integer
    Private m_pp As Integer
    Private m_uri As String
    Private m_boost As Integer
    Private m_isSpecial As Boolean
    Private m_effect As String

    Public Property Name As String
        Get
            Return m_name
        End Get
        Set(value As String)
            m_name = value
        End Set
    End Property

    Public Property Accuracy As Integer
        Get
            Return m_accuracy
        End Get
        Set(value As Integer)
            m_accuracy = value
        End Set
    End Property

    Public Property Type As String
        Get
            Return m_type
        End Get
        Set(value As String)
            m_type = value
        End Set
    End Property

    Public Property Power As Integer
        Get
            Return m_power
        End Get
        Set(value As Integer)
            m_power = value
        End Set
    End Property

    Public Property PP As Integer
        Get
            Return m_pp
        End Get
        Set(value As Integer)
            m_pp = value
        End Set
    End Property

    Public Property URI As String
        Get
            Return m_uri
        End Get
        Set(value As String)
            m_uri = value
        End Set
    End Property

    Public Property Boost As Integer
        Get
            Return m_boost
        End Get
        Set(value As Integer)
            m_boost = value
        End Set
    End Property

    ''' <summary>
    ''' Holds a list of string of effects. Generally for moves that raises or lowers stats.
    ''' For instance, a list can be ATK+1,SPD+1 means ATK raised by 1 stage and SPD raised by 1 stage
    ''' </summary>
    ''' <value></value>
    ''' <returns>A string of effect application</returns>
    ''' <remarks></remarks>
    Public Property Effect As String
        Get
            Return m_effect
        End Get
        Set(value As String)
            m_effect = value
        End Set
    End Property

    Public Property Is_Special As Boolean
        Get
            Return m_isSpecial
        End Get
        Set(value As Boolean)
            m_isSpecial = value
        End Set
    End Property

    Public Function Clone() As Object Implements ICloneable.Clone
        Dim freshmoveinfo As New Move_Info
        freshmoveinfo.Accuracy = Me.Accuracy
        freshmoveinfo.Type = Me.Type
        freshmoveinfo.Name = Me.Name
        freshmoveinfo.Power = Me.Power
        freshmoveinfo.PP = Me.PP
        'freshmoveinfo.Type = Me.Type.Clone() for a future release
        freshmoveinfo.URI = Me.URI
        freshmoveinfo.Boost = Me.Boost
        freshmoveinfo.Is_Special = Me.Is_Special
        freshmoveinfo.Effect = Me.Effect

        Return freshmoveinfo
    End Function
End Class

Public Class Move_Package

    Private m_name As String
    Private m_uri As String

    Public Property Name As String
        Get
            Return m_name
        End Get
        Set(value As String)
            m_name = value
        End Set
    End Property

    Public Property URI As String
        Get
            Return m_uri
        End Get
        Set(value As String)
            m_uri = value
        End Set
    End Property

    Public Sub InsertMove(ByRef pokemon As Pokemon, ByVal package As Move_Package)

        Dim movename As String = package.Name.Trim("""")
        movename.Trim()

        REM first check if the move is already in our dictionary, we can save a lot of time
        If Form1.Get_MoveDictionary.IsMoveInDictionary(movename) Then
            pokemon.Moves.Add(movename)
            Return
        End If

        Dim base_url As String = "http://pokeapi.co/"
        Dim uri As String = package.URI
        uri = uri.Trim("""")
        Dim fulluri As String = base_url + uri

        Dim movelines As String = Form1.RequestLine(fulluri)
        Dim filename As String = Me.ProcessLine(movelines)
        If filename = "" Then
            MessageBox.Show("Something went wrong with processing the line when trying to insert a move. Check previous error messages.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim master_filereader As StreamReader = Nothing
        Try
            master_filereader = My.Computer.FileSystem.OpenTextFileReader(filename)
        Catch ex As Exception
            MessageBox.Show("Something went wrong when trying to open " & filename & " for reading. Aborting...", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Dim poke_moveinfo As New Move_Info
        poke_moveinfo.Name = package.Name


        Dim line As String = ""
        While Not master_filereader.EndOfStream()
            line = master_filereader.ReadLine()
            If line = """accuracy""" Then
                REM directly consume the next line since we know it's the value of the accuracy
                line = master_filereader.ReadLine()
                Dim accuracy As Integer = Convert.ToInt32(line)
                poke_moveinfo.Accuracy = accuracy
            ElseIf line = """power""" Then
                line = master_filereader.ReadLine()
                Dim power As Integer = Convert.ToInt32(line)
                poke_moveinfo.Power = power
            ElseIf line = """pp""" Then
                line = master_filereader.ReadLine()
                Dim pp As Integer = Convert.ToInt32(line)
                poke_moveinfo.PP = pp
            ElseIf line = """resource_uri""" Then
                line = master_filereader.ReadLine()
                poke_moveinfo.URI = line
            Else
                Continue While
            End If
        End While
        master_filereader.Close()

        Form1.Get_MoveDictionary.Add_Move(movename, poke_moveinfo)
        pokemon.Moves.Add(movename)

    End Sub

    ''' <summary>
    ''' Given an unorganized moves text string, this function will parse it into one lines into
    ''' a new text file
    ''' </summary>
    ''' <param name="lines"></param>
    ''' <returns>Name of the formatted textfile</returns>
    ''' <remarks></remarks>
    Public Function ProcessLine(ByVal lines As String) As String

        Dim returnvalue As String = Form1.FormatFile(lines, "move.txt", "move_formatted.txt")
        If returnvalue = "" Then
            MessageBox.Show("Something went wrong with formatting the file. Check previous error messages.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ""
        End If

        Return "move_formatted.txt"

    End Function

End Class
