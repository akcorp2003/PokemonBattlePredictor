﻿Imports System
Imports System.Collections
Imports PokemonBattlePredictor
Imports PokemonBattlePredictor.PBP

Namespace PBP
    Public Class Pokemon_Team
        Implements System.ICloneable

        Dim Team_Blue As New List(Of Pokemon)
        Dim Team_Red As New List(Of Pokemon)

        ''' <summary>
        ''' Add the pokemon to the corresponding team
        ''' </summary>
        ''' <param name="pokemon"></param>
        ''' <param name="team">Must be either "blue" or "red"</param>
        ''' <remarks></remarks>
        Public Sub Addto_Team(ByVal pokemon As Pokemon, ByVal team As String)
            Dim team_name As String = team.ToLower()
            If team_name = "blue" Then
                pokemon.Team = "blue"
                Team_Blue.Add(pokemon)
            ElseIf team_name = "red" Then
                pokemon.Team = "red"
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

        Public Function Clone() As Object Implements ICloneable.Clone
            Dim freshpokemonteam As New Pokemon_Team
            freshpokemonteam.Team_Blue = Me.Team_Blue.Select(Function(x) x.Clone).Cast(Of Pokemon).ToList()
            freshpokemonteam.Team_Red = Me.Team_Red.Select(Function(x) x.Clone).Cast(Of Pokemon).ToList()

            Return freshpokemonteam
        End Function
    End Class
End Namespace
