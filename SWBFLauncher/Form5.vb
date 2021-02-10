Public Class Form5
    Private Sub Form5_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim profilesDirectory As String = "SaveGames"
        If System.IO.Directory.Exists(profilesDirectory) Then
            For Each profile As String In System.IO.Directory.GetFiles(profilesDirectory, "*.profile")
                ListBox1.Items.Add(System.IO.Path.GetFileNameWithoutExtension(profile))
            Next
        End If
        Button1.Enabled = False
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim profilesDirectory As String = "SaveGames"
        Dim profile As Object = ListBox1.SelectedItem
        System.IO.File.Delete(profilesDirectory & "\" & profile.ToString & ".profile")
        ListBox1.Items.Remove(profile)
        Button1.Enabled = False
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Button1.Enabled = True
    End Sub
End Class