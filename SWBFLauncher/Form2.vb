Imports winini
Public Class Form2
    'This function gets the name of a selected addon via addme.script file, and returns into a string.
    'Note it could happen that if folder name starts with sh, there's a potential risk to get a runtime error (needs to be replaced by a more accurate algorithm).
    Private Function GetAddOnName(ByVal addOn As Object, ByVal directory As String, ByVal file As String) As String
        Dim name As String = ""
        Dim addMe As New System.IO.StreamReader(directory & "\" & addOn.ToString & "\" & file)
        Dim i, j As Char
        Dim gotName As Boolean
        j = ""
        gotName = False
        For k As Integer = 0 To 101 Step 1
            addMe.Read()
        Next
        Do
            i = Chr(addMe.Read())
            If j.ToString + i.ToString = "sh" Then
                For k As Integer = 0 To 9 Step 1
                    addMe.Read()
                Next
                Do
                    i = Chr(addMe.Read())
                    If i <> Chr(0) Then
                        name += i.ToString
                    Else
                        gotName = True
                    End If
                Loop Until gotName
            End If
            j = i
        Loop Until gotName
        addMe.Close()
        Return name
    End Function
    'Formulary which adds all the AddOns from the AddOn directory to the ListBox.
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim addOnDirectory As String = "AddOn"
        Dim settingsFile As String = "settings.ini"
        Dim addOnsSettings As INI = New INI
        addOnsSettings.LoadINIFile("AddOns", settingsFile)
        'Before we add all items, check Sort By preferences. We add these either if are enabled or not.
        If System.IO.Directory.Exists(addOnDirectory) And addOnsSettings.GetINIValue("SortBy") = "Enabled" Then
            For Each addOn As String In System.IO.Directory.GetDirectories(addOnDirectory)
                If System.IO.File.Exists(addOn & "\addme.script") Then
                    ListBox1.Items.Add(System.IO.Path.GetFileName(addOn))
                End If
            Next
            For Each addOn As String In System.IO.Directory.GetDirectories(addOnDirectory)
                If System.IO.File.Exists(addOn & "\_addme.script") Then
                    ListBox1.Items.Add(System.IO.Path.GetFileName(addOn))
                End If
            Next
        ElseIf System.IO.Directory.Exists(addOnDirectory) And addOnsSettings.GetINIValue("SortBy") = "Disabled" Then
            For Each addOn As String In System.IO.Directory.GetDirectories(addOnDirectory)
                If System.IO.File.Exists(addOn & "\_addme.script") Then
                    ListBox1.Items.Add(System.IO.Path.GetFileName(addOn))
                End If
            Next
            For Each addOn As String In System.IO.Directory.GetDirectories(addOnDirectory)
                If System.IO.File.Exists(addOn & "\addme.script") Then
                    ListBox1.Items.Add(System.IO.Path.GetFileName(addOn))
                End If
            Next
        End If
        TextBox4.Text = "Not selected"
        TextBox4.BackColor = Color.Gray
    End Sub
    'Each time we select an item, the related info will be displayed.
    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Dim addOnDirectory As String = "AddOn"
        Dim mapInfoImage As String = "mapinfo.png"
        Dim readMeFile As String = "readme.txt"
        Dim addOn As Object = ListBox1.SelectedItem
        'Checking if we didn't select an item which is does not have any data.
        If Not addOn Is Nothing Then
            'Map preview. Only shows if a mapinfo.png image exists.
            If System.IO.File.Exists(addOnDirectory & "\" & addOn.ToString & "\" & mapInfoImage) Then
                Dim mapPreview As New System.IO.FileStream(addOnDirectory & "\" & addOn.ToString & "\" & mapInfoImage, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                PictureBox1.Image = Image.FromStream(mapPreview)
                mapPreview.Close()
            Else
                PictureBox1.Image = Nothing
            End If
            'Name of the AddOn map. Doesn't matter if the AddOn is enabled or disabled, we must load the name.
            If System.IO.File.Exists(addOnDirectory & "\" & addOn.ToString & "\addme.script") Then
                TextBox1.Text = GetAddOnName(addOn, addOnDirectory, "addme.script")
            ElseIf System.IO.File.Exists(addOnDirectory & "\" & addOn.ToString & "\_addme.script") Then
                TextBox1.Text = GetAddOnName(addOn, addOnDirectory, "_addme.script")
            End If
            'If ReadMe file exist, we load all it's content.
            If System.IO.File.Exists(addOnDirectory & "\" & addOn.ToString & "\" & readMeFile) Then
                Dim readMe As New System.IO.StreamReader(addOnDirectory & "\" & addOn.ToString & "\" & readMeFile)
                TextBox3.Clear()
                TextBox3.Text = readMe.ReadToEnd()
                'Do
                'TextBox3.Text += readMe.ReadLine()
                'TextBox3.Text += vbCrLf
                'Loop Until readMe.EndOfStream
                readMe.Close()
            Else
                TextBox3.Text = "Not available."
            End If
            'Monitoring if the AddOn is enabled or disabled. It will show current status in a TextBox.
            If ListBox1.SelectedIndex >= 0 Then
                If System.IO.File.Exists(addOnDirectory & "\" & addOn.ToString & "\addme.script") Then
                    Button1.Text = "Disable"
                    TextBox4.Text = "Enabled"
                    TextBox4.BackColor = Color.Green
                ElseIf System.IO.File.Exists(addOnDirectory & "\" & addOn.ToString & "\_addme.script") Then
                    Button1.Text = "Enable"
                    TextBox4.Text = "Disabled"
                    TextBox4.BackColor = Color.Red
                End If
            End If
        End If
        Button1.Enabled = True
        Button2.Enabled = True
        Button3.Enabled = True
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Me.Close()
    End Sub

    'Button which enables/disables an AddOn to allow/prevent to show ingame.
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim addOnDirectory As String = "AddOn"
        Dim addOn As Object = ListBox1.SelectedItem
        If System.IO.File.Exists(addOnDirectory & "\" & addOn.ToString & "\addme.script") Then
            My.Computer.FileSystem.RenameFile(addOnDirectory & "\" & addOn.ToString & "\addme.script", "_addme.script")
            Button1.Text = "Enable"
            TextBox4.Text = "Disabled"
            TextBox4.BackColor = Color.Red
        ElseIf System.IO.File.Exists(addOnDirectory & "\" & addOn.ToString & "\_addme.script") Then
            My.Computer.FileSystem.RenameFile(addOnDirectory & "\" & addOn.ToString & "\_addme.script", "addme.script")
            Button1.Text = "Disable"
            TextBox4.Text = "Enabled"
            TextBox4.BackColor = Color.Green
        End If
    End Sub

    'Button which deletes the selected AddOn directory, and removes from the ListBox.
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim addOnDirectory As String = "AddOn"
        Dim addOn As Object = ListBox1.SelectedItem
        PictureBox1.Image = Nothing
        TextBox1.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox4.BackColor = Nothing
        System.IO.Directory.Delete(addOnDirectory & "\" & addOn.ToString, True)
        ListBox1.Items.Remove(addOn)
        Button1.Enabled = False
        Button2.Enabled = False
        Button3.Enabled = False
    End Sub

    'Button which displays a new window dedicated to the selected AddOn settings.
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Form6.Show()
        Form6.Text = ListBox1.SelectedItem.ToString & " settings"
    End Sub
End Class