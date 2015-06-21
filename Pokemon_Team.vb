Imports System
Imports System.Collections

Public Class Pokemon_Team
    Dim Team_Blue As List(Of Pokemon)
    Dim Team_Red As List(Of Pokemon)

    ''' <summary>
    ''' Add the pokemon to the corresponding team
    ''' </summary>
    ''' <param name="pokemon"></param>
    ''' <param name="team">Must be either "blue" or "red"</param>
    ''' <remarks></remarks>
    Public Sub Addto_Team(ByVal pokemon As Pokemon, ByVal team As String)
        Dim team_name As String = team.ToLower()
        If team_name = "blue" Then
            Team_Blue.Add(pokemon)
        ElseIf team_name = "red" Then
            Team_Red.Add(pokemon)
        Else
            MessageBox.Show("Unable to add pokemon to a team. Please make sure you spelled Blue or Red properly.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
    End Sub

    Public Function Get_Team(ByVal team As String) As List(Of Pokemon)
        Dim team_name As String = team.ToLower()
        If team_name = "blue" Then
            Return Team_Blue
        ElseIf team_name = "red" Then
            Return Team_Red
        Else
            MessageBox.Show("Unable to get pokemon team. Please make sure you spelled Blue or Red properly.", "Whoops!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        End If
    End Function
End Class
