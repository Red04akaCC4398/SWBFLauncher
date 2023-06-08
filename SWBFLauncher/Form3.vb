Imports winini
Public Class Form3
    'Formulary which adds all the Mods from the AddOn folder to the ComboBox.
    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim modsDirectory As String = "Mods"
        Dim modInstalled As Boolean = False
        If System.IO.Directory.Exists(modsDirectory) Then
            For Each modID As String In System.IO.Directory.GetDirectories(modsDirectory)
                If System.IO.Directory.Exists(modID & "\Data\_LVL_PC") Then
                    ComboBox1.Items.Add(System.IO.Path.GetFileName(modID))
                End If
                'If a mod is already installed, we select and load its information.
                If System.IO.File.Exists(modID & "\modinfo.path") Then
                    modInstalled = True
                    ComboBox1.Text = System.IO.Path.GetFileName(modID)
                End If
            Next
            'If any mod is installed, it shows nothing.
            If Not modInstalled Then
                ComboBox1.Text = "None"
                TextBox1.Text = "No mods installed"
                TextBox1.BackColor = Color.Gray
            End If
        End If
    End Sub
    'Button which deletes the selected Mod directory, and removes from the ComboBox.
    'This only will work for not installed mods.
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim modsDirectory As String = "Mods"
        Dim modID As Object = ComboBox1.SelectedItem
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox4.Clear()
        TextBox1.BackColor = Nothing
        System.IO.Directory.Delete(modsDirectory & "\" & modID.ToString, True)
        ComboBox1.Items.Remove(modID)
        ComboBox1.Text = "None"
        TextBox1.Text = "No mods installed"
        TextBox1.BackColor = Color.Gray
        Button1.Enabled = False
        Button2.Enabled = False
        Button3.Enabled = False
    End Sub
    'Mod install/uninstall process. Only works if a mod is selected.
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim modID As Object = ComboBox1.SelectedItem
        Dim lvlDirectory As String = "Data\_LVL_PC"
        Dim backUpDirectory As String = "Data\_BACKUP"
        Dim modIDDirectory As String = "Mods\" & modID.ToString
        Dim lvlRead As String() = {}
        Dim modReadAux As String = ""
        Dim lvlReadAux As String = ""
        Dim found, deleted As Boolean
        Dim i As Integer
        deleted = False
        'Check if the mod is installed or not.
        If System.IO.Directory.GetFiles(backUpDirectory).Count > 0 Or System.IO.Directory.GetDirectories(backUpDirectory).Count > 0 Then
            Dim auxDirectory As String = ""
            'Here we check if we want to uninstall a mod which is already installed to be replaced with a new one or not.
            If Not System.IO.File.Exists(modIDDirectory & "\modinfo.path") Then
                For Each auxID As String In System.IO.Directory.GetDirectories("Mods")
                    If System.IO.File.Exists(auxID & "\modinfo.path") Then
                        auxDirectory = auxID
                    End If
                Next
            Else
                auxDirectory = modIDDirectory
                deleted = True 'Boolean value to make sure we want to uninstall an installed mod, not to install it again.
            End If
            If System.IO.File.Exists("Data\battlefront.exe") Then
                System.IO.File.Move("battlefront.exe", auxDirectory & "\battlefront.exe")
                System.IO.File.Move("Data\battlefront.exe", "battlefront.exe")
            End If
            'Reading modinfo.path file, necessary to know where all the mod files are placed into _LVL_PC directory.
            Dim modRead As New System.IO.StreamReader(auxDirectory & "\modinfo.path")
            'Starting uninstall move process: Data\_LVL_PC -> Mods\<ModID>\Data\_LVL_PC, Data\_BACKUP -> Data\_LVL_PC.
            Do
                modReadAux = modRead.ReadLine
                If Not System.IO.Directory.Exists(auxDirectory & "\" & lvlDirectory & "\" & System.IO.Path.GetDirectoryName(modReadAux)) And System.IO.Path.GetDirectoryName(modReadAux) <> "" Then
                    System.IO.Directory.CreateDirectory(auxDirectory & "\" & lvlDirectory & "\" & System.IO.Path.GetDirectoryName(modReadAux))
                End If
                System.IO.File.Move(lvlDirectory & "\" & modReadAux, auxDirectory & "\" & lvlDirectory & "\" & modReadAux)
                If System.IO.File.Exists(backUpDirectory & "\" & modReadAux) Then
                    System.IO.File.Move(backUpDirectory & "\" & modReadAux, lvlDirectory & "\" & modReadAux)
                ElseIf System.IO.Directory.GetFiles(lvlDirectory & "\" & System.IO.Path.GetDirectoryName(modReadAux)).Count = 0 Then
                    System.IO.Directory.Delete(lvlDirectory & "\" & System.IO.Path.GetDirectoryName(modReadAux))
                End If
            Loop Until modRead.EndOfStream
            modRead.Close()
            System.IO.Directory.Delete(backUpDirectory, True)
            System.IO.Directory.CreateDirectory(backUpDirectory)
            System.IO.File.Delete(auxDirectory & "\modinfo.path")
            'Deleting empty directories which aren't linked to original files (we don't need these anymore).
            For Each path As String In System.IO.Directory.GetDirectories(lvlDirectory)
                If System.IO.Directory.GetFiles(path).Count = 0 And System.IO.Directory.GetDirectories(path).Count = 0 Then
                    System.IO.Directory.Delete(path)
                End If
            Next
            Button2.Enabled = True
            Button1.Text = "Install"
            TextBox1.Text = "Not installed"
            TextBox1.BackColor = Color.Red
        End If
        If Not System.IO.File.Exists(modIDDirectory & "\modinfo.path") And Not deleted Then
            'Creating modinfo.path file to save all file paths, necessary to recover mod files later.
            Dim modWrite As New System.IO.StreamWriter(modIDDirectory & "\modinfo.path", False)
            For Each path As String In System.IO.Directory.GetFiles(modIDDirectory & "\" & lvlDirectory, "*.lvl", System.IO.SearchOption.AllDirectories)
                modWrite.WriteLine(path.Replace(modIDDirectory & "\" & lvlDirectory & "\", ""))
            Next
            modWrite.Close()
            If System.IO.File.Exists(modIDDirectory & "\battlefront.exe") Then
                System.IO.File.Move("battlefront.exe", "Data\battlefront.exe")
                System.IO.File.Move(modIDDirectory & "\battlefront.exe", "battlefront.exe")
            End If
            Dim modRead As New System.IO.StreamReader(modIDDirectory & "\modinfo.path")
            lvlRead = System.IO.Directory.GetFiles(lvlDirectory, "*.lvl", System.IO.SearchOption.AllDirectories)
            'Starting install move process: Data\_LVL_PC -> Data\_BACKUP, Mods\<ModID>\Data\_LVL_PC -> Data\_LVL_PC.
            Do
                modReadAux = modRead.ReadLine()
                found = False
                i = 0
                'We must check if any of the mod filenames matches with any stock filenames. The original ones will be moved to Data\_BACKUP directory.
                While i < lvlRead.Length And Not found
                    lvlReadAux = lvlRead(i)
                    If modReadAux = lvlReadAux.Replace(lvlDirectory & "\", "") Then
                        found = True
                        If Not System.IO.Directory.Exists(backUpDirectory & "\" & System.IO.Path.GetDirectoryName(modReadAux)) And System.IO.Path.GetDirectoryName(modReadAux) <> "" Then
                            System.IO.Directory.CreateDirectory(backUpDirectory & "\" & System.IO.Path.GetDirectoryName(modReadAux))
                        End If
                        System.IO.File.Move(lvlDirectory & "\" & modReadAux, backUpDirectory & "\" & modReadAux)
                        System.IO.File.Move(modIDDirectory & "\" & lvlDirectory & "\" & modReadAux, lvlDirectory & "\" & modReadAux)
                    Else
                        i += 1
                    End If
                End While
                'If is a new directory/file, we don't have to make a back-up, just move the file to Data\_LVL_PC directory.
                If Not found Then
                    If Not System.IO.Directory.Exists(lvlDirectory & "\" & System.IO.Path.GetDirectoryName(modReadAux)) And System.IO.Path.GetDirectoryName(modReadAux) <> "" Then
                        System.IO.Directory.CreateDirectory(lvlDirectory & "\" & System.IO.Path.GetDirectoryName(modReadAux))
                    End If
                    System.IO.File.Move(modIDDirectory & "\" & lvlDirectory & "\" & modReadAux, lvlDirectory & "\" & modReadAux)
                End If
            Loop Until modRead.EndOfStream
            modRead.Close()
            System.IO.Directory.Delete(modIDDirectory & "\" & lvlDirectory, True)
            System.IO.Directory.CreateDirectory(modIDDirectory & "\" & lvlDirectory)
            Button2.Enabled = False
            Button1.Text = "Uninstall"
            TextBox1.Text = "Installed"
            TextBox1.BackColor = Color.Green
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Me.Close()
    End Sub
    'Each time we select an item, the related info will be displayed.
    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim modsDirectory As String = "Mods"
        Dim readMeFile As String = "readme.txt"
        Dim modID As Object = ComboBox1.SelectedItem
        If Not modID Is Nothing Then
            TextBox2.Text = modID.ToString
            If System.IO.File.Exists(modsDirectory & "\" & modID.ToString & "\" & readMeFile) Then
                Dim readMe As New System.IO.StreamReader(modsDirectory & "\" & modID.ToString & "\" & readMeFile)
                TextBox4.Clear()
                TextBox4.Text = readMe.ReadToEnd()
                'Do
                'TextBox4.Text += readMe.ReadLine
                'TextBox4.Text += vbCrLf
                'Loop Until readMe.EndOfStream
                readMe.Close()
            Else
                TextBox4.Text = "Not available."
            End If
            If ComboBox1.SelectedIndex >= 0 Then
                If System.IO.File.Exists(modsDirectory & "\" & modID.ToString & "\modinfo.path") Then
                    Button1.Text = "Uninstall"
                    TextBox1.Text = "Installed"
                    TextBox1.BackColor = Color.Green
                    Button2.Enabled = False
                ElseIf Not System.IO.File.Exists(modsDirectory & "\" & modID.ToString & "\modinfo.path") Then
                    Button1.Text = "Install"
                    TextBox1.Text = "Not installed"
                    TextBox1.BackColor = Color.Red
                    Button2.Enabled = True
                End If
            End If
        End If
        Button1.Enabled = True
        Button3.Enabled = True
    End Sub
    'Button which displays a new window dedicated to the selected Mod settings.
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Form7.Show()
        Form7.Text = ComboBox1.SelectedItem.ToString & " settings"
    End Sub
End Class