Imports localize
'A window for specific AddOn preferences. At the moment only language localization can be modified.
Public Class Form6
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Form6_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim addOnDirectory As String = "AddOn"
        Dim lvlDirectory As String = "Data\_lvl_pc"
        Dim addOnLocalization As Localization = New Localization
        addOnLocalization.LoadCoreFile(addOnDirectory & "\" & Form2.ListBox1.SelectedItem.ToString & "\" & lvlDirectory & "\core.lvl", 1)
        ComboBox1.Text = addOnLocalization.GetLanguage
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim addOnDirectory As String = "AddOn"
        Dim lvlDirectory As String = "Data\_lvl_pc"
        Dim addOnLocalization As Localization = New Localization
        addOnLocalization.LoadCoreFile(addOnDirectory & "\" & Form2.ListBox1.SelectedItem.ToString & "\" & lvlDirectory & "\core.lvl", 1)
        addOnLocalization.ChangeLocalization(ComboBox1.Text)
        Me.Close()
    End Sub
End Class