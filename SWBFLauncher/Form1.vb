Imports winini
Imports localize
Public Class Form1
    'ReadProcessMemory and WriteProcessMemory functions are used to read and write to battlefront.exe process memory specific address.
    Private Declare Function ReadProcessMemory Lib "kernel32" Alias "ReadProcessMemory" (ByVal hprocess As Integer, ByVal address As Integer, ByRef value As Integer, ByVal size As Integer, ByRef bytesToRead As Integer) As Boolean
    Private Declare Function WriteProcessMemory Lib "kernel32" Alias "WriteProcessMemory" (ByVal hprocess As Integer, ByVal address As Integer, ByRef value As Integer, ByVal size As Integer, ByRef bytesToWrite As Integer) As Boolean
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Form2.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Form3.Show()
        'MsgBox("This feature is not available yet.", 48, "Mods")
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Form4.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim title As String = "Red04's Star Wars Battlefront Launcher"
        Dim version As String = "Version 0.3"
        Dim copyright As String = "Copyright 2021-2023 by Red04 (CC-4398)"
        Dim license As String = "This software is not developed, supported or distributed by Pandemic Studios, LucasArts or Disney. LucasArts and the LucasArts logo are registered trademarks of Lucasfilm Ltd. Star Wars Battlefront is a trademark of Lucasfilm Entertainment Company Ltd. Copyright 2004-2005 Lucasfilm Entertainment Company Ltd. or Lucasfilm Ltd. & (R) or TM as indicated. All rights reserved."
        MsgBox(title + vbLf + version + vbLf + copyright + vbLf + vbLf + license, 0, "About")
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        End
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim name As String = "battlefront"
        Dim swbf As New Process()
        'Loading INI sections from settings.ini file.
        Dim gameSettings As INI = New INI
        Dim videoSettings As INI = New INI
        Dim audioSettings As INI = New INI
        Dim addOnsSettings As INI = New INI
        Dim modsSettings As INI = New INI
        'Loading language localization for each core.lvl file.
        Dim gameLocalization As Localization = New Localization
        Dim addOnLocalization As Localization = New Localization
        Dim modLocalization As Localization = New Localization
        Dim count As Integer = 0
        'Check if battlefront.exe exists, if not application will close.
        If System.IO.File.Exists(name & ".exe") Then
            Dim section As String() = {"Game", "Video", "Audio", "AddOns", "Mods"}
            Dim addOnDirectory As String = "AddOn"
            Dim settingsFile As String = "settings.ini"
            'Storing INI keys and values for each section temporarily.
            gameSettings.LoadINIFile(section(0), settingsFile)
            videoSettings.LoadINIFile(section(1), settingsFile)
            audioSettings.LoadINIFile(section(2), settingsFile)
            addOnsSettings.LoadINIFile(section(3), settingsFile)
            modsSettings.LoadINIFile(section(4), settingsFile)
            Dim lvlDirectory As String = "Data\_LVL_PC"
            Dim backUpDirectory As String = "Data\_BACKUP"
            'In case any mod is installed. it loads the stock core.lvl in the corresponding directory.
            If System.IO.File.Exists(backUpDirectory & "\core.lvl") Then
                gameLocalization.LoadCoreFile(backUpDirectory & "\core.lvl", 0)
            Else
                gameLocalization.LoadCoreFile(lvlDirectory & "\core.lvl", 0)
            End If
            'Setting the selected language to the game.
            gameLocalization.ChangeLocalization(gameSettings.GetINIValue("Language"))
            swbf.StartInfo.FileName = name + ".exe"
            swbf.StartInfo.Arguments = ""
            If videoSettings.GetINIValue("WindowedMode") = 1 Then
                swbf.StartInfo.Arguments += "/win"
                count += 1
            End If
            If videoSettings.GetINIValue("NoIntro") = 1 And count = 1 Then
                swbf.StartInfo.Arguments += ",/nointro"
            ElseIf videoSettings.GetINIValue("NoIntro") = 1 Then
                swbf.StartInfo.Arguments += "/nointro"
                count += 1
            End If
            If videoSettings.GetINIValue("NoMovies") = 1 And count = 1 Then
                swbf.StartInfo.Arguments += ",/nomovies"
            ElseIf videoSettings.GetINIValue("NoMovies") = 1 Then
                swbf.StartInfo.Arguments += "/nomovies"
                count += 1
            End If
            If audioSettings.GetINIValue("NoStartupMusic") = 1 And count = 1 Then
                swbf.StartInfo.Arguments += ",/nostartupmusic"
            ElseIf audioSettings.GetINIValue("NoStartupMusic") = 1 Then
                swbf.StartInfo.Arguments += "/nostartupmusic"
                count += 1
            End If
            If audioSettings.GetINIValue("SetCustomAudioSampleRate") = 1 And count = 1 Then
                swbf.StartInfo.Arguments += ",/audiosamplerate " & audioSettings.GetINIValue("AudioSampleRate")
            ElseIf audioSettings.GetINIValue("SetCustomAudioSampleRate") = 1 Then
                swbf.StartInfo.Arguments += "/audiosamplerate " & audioSettings.GetINIValue("AudioSampleRate")
                count += 1
            End If
            If audioSettings.GetINIValue("SetCustomAudioMixBuffer") = 1 And count = 1 Then
                swbf.StartInfo.Arguments += ",/audiomixbuffer " & audioSettings.GetINIValue("AudioMixBuffer")
            ElseIf audioSettings.GetINIValue("SetCustomAudioMixBuffer") = 1 Then
                swbf.StartInfo.Arguments += "/audiomixbuffer " & audioSettings.GetINIValue("AudioMixBuffer")
                count += 1
            End If
            If addOnsSettings.GetINIValue("AddOnLocalization") = 1 Then
                For Each addOn As String In System.IO.Directory.GetDirectories(addOnDirectory)
                    If System.IO.File.Exists(addOn & "\addme.script") Or System.IO.File.Exists(addOn & "\_addme.script") Then
                        addOnLocalization.LoadCoreFile(addOn & "\Data\_lvl_pc\core.lvl", 1)
                        addOnLocalization.ChangeLocalization(gameSettings.GetINIValue("Language"))
                    End If
                Next
            End If
            If modsSettings.GetINIValue("ModLocalization") = 1 Then
                For Each modID As String In System.IO.Directory.GetDirectories("Mods")
                    If System.IO.File.Exists(modID & "\Data\_lvl_pc\core.lvl") Then
                        modLocalization.LoadCoreFile(modID & "\Data\_lvl_pc\core.lvl", 2)
                        modLocalization.ChangeLocalization(gameSettings.GetINIValue("Language"))
                    End If
                Next
                If System.IO.File.Exists(backUpDirectory & "\core.lvl") Then
                    modLocalization.LoadCoreFile(lvlDirectory & "\core.lvl", 2)
                    modLocalization.ChangeLocalization(gameSettings.GetINIValue("Language"))
                End If
            End If
            'Once we have collected all the arguments, the game can start.
            swbf.Start()
            'Starting to load battlefront.exe process memory.
            Dim swbfmemory As Process() = Process.GetProcessesByName(name)
            Dim resXAddress As Integer = &H7FCA4A 'Resolution width memory address value.
            Dim resYAddress As Integer = &H7FCA4C 'Resolution height memory address value.
            Dim resXDefaultValue As Integer = 0
            Dim resYDefaultValue As Integer = 0
            Dim resXValue As Integer = videoSettings.GetINIValue("Width")
            Dim resYValue As Integer = videoSettings.GetINIValue("Height")
            ReadProcessMemory(swbfmemory(0).Handle, resXAddress, resXDefaultValue, 2, 0)
            ReadProcessMemory(swbfmemory(0).Handle, resYAddress, resYDefaultValue, 2, 0)
            'Read process memory again until we modified resolution values twice, or the game closes.
            Dim counter As Integer = 0
            Do
                System.Threading.Thread.Sleep(1000)
                If resXValue <> resXDefaultValue Or resYValue <> resYDefaultValue Then
                    WriteProcessMemory(swbfmemory(0).Handle, resXAddress, resXValue, 2, 0)
                    WriteProcessMemory(swbfmemory(0).Handle, resYAddress, resYValue, 2, 0)
                    counter += 1
                End If
                ReadProcessMemory(swbfmemory(0).Handle, resXAddress, resXDefaultValue, 2, 0)
                ReadProcessMemory(swbfmemory(0).Handle, resYAddress, resYDefaultValue, 2, 0)
            Loop Until counter = 2 Or swbf.HasExited
        End If
        End
    End Sub

    'When the application launches for its first time, it will make sure there are the necessary files/directories.
    'If not, these will be automatically created.
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim modsDirectory As String = "Mods"
        Dim backUpDirectory As String = "Data\_BACKUP"
        Dim settingsFile As String = "settings.ini"
        If Not System.IO.Directory.Exists(modsDirectory) Then
            System.IO.Directory.CreateDirectory(modsDirectory)
        End If
        If Not System.IO.Directory.Exists(backUpDirectory) Then
            System.IO.Directory.CreateDirectory(backUpDirectory)
        End If
        If Not System.IO.File.Exists(settingsFile) Then
            Dim settings As New System.IO.StreamWriter(settingsFile, False)
            settings.WriteLine("[Game]")
            settings.WriteLine("Language=English")
            settings.WriteLine("[Video]")
            settings.WriteLine("Width=640")
            settings.WriteLine("Height=480")
            settings.WriteLine("WindowedMode=0")
            settings.WriteLine("NoIntro=0")
            settings.WriteLine("NoMovies=0")
            settings.WriteLine("[Audio]")
            settings.WriteLine("NoStartupMusic=0")
            settings.WriteLine("SetCustomAudioSampleRate=0")
            settings.WriteLine("AudioSampleRate=44100")
            settings.WriteLine("SetCustomAudioMixBuffer=0")
            settings.WriteLine("AudioMixBuffer=")
            settings.WriteLine("[AddOns]")
            settings.WriteLine("SortBy=Enabled")
            settings.WriteLine("AddOnLocalization=0")
            settings.WriteLine("[Mods]")
            settings.WriteLine("ModLocalization=0")
            'For i As Integer = 0 To 9 Step 1
            'settings.WriteLine("Slot" & i & "=")
            'Next
            settings.Close()
        End If
    End Sub
End Class
