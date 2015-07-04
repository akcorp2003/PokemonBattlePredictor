Public Class test

    Private Sub Initiate_build_Click(sender As Object, e As EventArgs) Handles Initiate_build.Click

        Dim dic_enum As New Dictionary(Of String, String).Enumerator
        dic_enum = Form1.Get_ResourceURIDictionary.Get_ResourceURIDictionary.GetEnumerator()
        dic_enum.MoveNext()
        Dim i As Integer = 0

        While i < Form1.Get_ResourceURIDictionary.Get_ResourceURIDictionary.Count

            BattleSetup.Pokemon_Name.Text = dic_enum.Current.Key.Trim("""")
            BattleSetup.InsertPokemon.PerformClick()
            i += 1
            dic_enum.MoveNext()
        End While
    End Sub
End Class