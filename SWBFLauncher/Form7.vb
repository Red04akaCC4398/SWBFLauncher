Imports localize
'A window for specific Mod preferences. At the moment only language localization can be modified.
Public Class Form7
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Form7_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim modsDirectory As String = "Mods"
        Dim lvlDirectory As String = "Data\_LVL_PC"
        Dim backUpDirectory As String = "Data\_BACKUP"
        Dim modLocalization As Localization = New Localization
        If System.IO.File.Exists(modsDirectory & "\" & Form3.ComboBox1.SelectedItem.ToString & "\" & lvlDirectory & "\core.lvl") Then
            modLocalization.LoadCoreFile(modsDirectory & "\" & Form3.ComboBox1.SelectedItem.ToString & "\" & lvlDirectory & "\core.lvl", 2)
            ComboBox1.Text = modLocalization.GetLanguage
        ElseIf System.IO.File.Exists(backUpDirectory & "\core.lvl") Then
            modLocalization.LoadCoreFile(lvlDirectory & "\core.lvl", 2)
            ComboBox1.Text = modLocalization.GetLanguage
        Else
            ComboBox1.Enabled = False
            Button2.Enabled = False
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim modsDirectory As String = "Mods"
        Dim lvlDirectory As String = "Data\_LVL_PC"
        Dim backUpDirectory As String = "Data\_BACKUP"
        Dim modLocalization As Localization = New Localization
        If System.IO.File.Exists(modsDirectory & "\" & Form3.ComboBox1.SelectedItem.ToString & "\" & lvlDirectory & "\core.lvl") Then
            modLocalization.LoadCoreFile(modsDirectory & "\" & Form3.ComboBox1.SelectedItem.ToString & "\" & lvlDirectory & "\core.lvl", 2)
        ElseIf System.IO.File.Exists(backUpDirectory & "\core.lvl") Then
            modLocalization.LoadCoreFile(lvlDirectory & "\core.lvl", 2)
        End If
        modLocalization.ChangeLocalization(ComboBox1.Text)
        Me.Close()
    End Sub
End Class