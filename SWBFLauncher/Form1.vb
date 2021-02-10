Imports winini
Public Class Form1
    Private Declare Function ReadProcessMemory Lib "kernel32" Alias "ReadProcessMemory" (ByVal hprocess As Integer, ByVal address As Integer, ByRef value As Integer, ByVal size As Integer, ByRef bytesToRead As Integer) As Boolean
    Private Declare Function WriteProcessMemory Lib "kernel32" Alias "WriteProcessMemory" (ByVal hprocess As Integer, ByVal address As Integer, ByRef value As Integer, ByVal size As Integer, ByRef bytesToWrite As Integer) As Boolean
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Form2.Show()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        'Form3.Show()
        MsgBox("This feature is not available yet.", 48, "Mods")
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Form4.Show()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim title As String = "Red04's Star Wars Battlefront Launcher"
        Dim version As String = "Version 0.1"
        Dim copyright As String = "Copyright 2021 by Red04 (CC-4398)"
        Dim license As String = "This software is not developed, supported or distributed by Pandemic Studios, LucasArts or Disney. LucasArts and the LucasArts logo are registered trademarks of Lucasfilm Ltd. Star Wars Battlefront is a trademark of Lucasfilm Entertainment Company Ltd. Copyright 2004-2005 Lucasfilm Entertainment Company Ltd. or Lucasfilm Ltd. & (R) or TM as indicated. All rights reserved."
        MsgBox(title + vbLf + version + vbLf + copyright + vbLf + vbLf + license, 0, "About")
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        End
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim name As String = "battlefront"
        Dim swbf As New Process()
        Dim gameSettings As INI = New INI
        Dim videoSettings As INI = New INI
        Dim audioSettings As INI = New INI
        Dim count As Integer = 0
        If System.IO.File.Exists(name & ".exe") Then
            Dim section As String() = {"Game", "Video", "Audio"}
            Dim settingsFile As String = "settings.ini"
            gameSettings.LoadINIFile(section(0), settingsFile)
            videoSettings.LoadINIFile(section(1), settingsFile)
            audioSettings.LoadINIFile(section(2), settingsFile)
            Dim lvlDirectory As String = "Data\_LVL_PC"
            Dim backUpDirectory As String = "Data\_BACKUP"
            Dim language As String = gameSettings.GetINIValue("Language")
            Dim engAddress As Integer = &HCABB0
            Dim spaAddress As Integer = &HF2190
            Dim itaAddress As Integer = &H11D120
            Dim freAddress As Integer = &H14602C
            Dim gerAddress As Integer = &H170374
            Dim ukAddress As Integer = &H19A178
            Dim englishLoc As Byte() = {&H65, &H6E, &H67, &H6C, &H69, &H73, &H68}
            'Language modification will go here
            If Not System.IO.File.Exists(backUpDirectory & "\core.lvl") Then
                System.IO.File.Copy(lvlDirectory & "\core.lvl", backUpDirectory & "\core.lvl")
            End If
            Dim localization As New System.IO.FileStream(lvlDirectory & "\core.lvl", System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite)
            localization.Position = engAddress
            If localization.ReadByte <> &H0 Then
                For i As Integer = 0 To 6 Step 1
                    localization.WriteByte(&H0)
                Next
            End If
            localization.Position = spaAddress
            If localization.ReadByte <> &H0 Then
                For i As Integer = 0 To 6 Step 1
                    localization.WriteByte(&H0)
                Next
            End If
            localization.Position = itaAddress
            If localization.ReadByte <> &H0 Then
                For i As Integer = 0 To 6 Step 1
                    localization.WriteByte(&H0)
                Next
            End If
            localization.Position = freAddress
            If localization.ReadByte <> &H0 Then
                For i As Integer = 0 To 5 Step 1
                    localization.WriteByte(&H0)
                Next
            End If
            localization.Position = gerAddress
            If localization.ReadByte <> &H0 Then
                For i As Integer = 0 To 5 Step 1
                    localization.WriteByte(&H0)
                Next
            End If
            localization.Position = ukAddress
            If localization.ReadByte <> &H0 Then
                For i As Integer = 0 To 9 Step 1
                    localization.WriteByte(&H0)
                Next
            End If
            If language = "English" Then
                localization.Position = engAddress
                For i As Integer = 0 To (englishLoc.Length - 1) Step 1
                    localization.WriteByte(englishLoc(i))
                Next
            ElseIf language = "English (UK)" Then
                localization.Position = ukAddress
                For i As Integer = 0 To (englishLoc.Length - 1) Step 1
                    localization.WriteByte(englishLoc(i))
                Next
                For i As Integer = 0 To 2 Step 1
                    localization.WriteByte(&H0)
                Next
            ElseIf language = "French" Then
                localization.Position = freAddress
                For i As Integer = 0 To (englishLoc.Length - 1) Step 1
                    localization.WriteByte(englishLoc(i))
                Next
            ElseIf language = "German" Then
                localization.Position = gerAddress
                For i As Integer = 0 To (englishLoc.Length - 1) Step 1
                    localization.WriteByte(englishLoc(i))
                Next
            ElseIf language = "Italian" Then
                localization.Position = itaAddress
                For i As Integer = 0 To (englishLoc.Length - 1) Step 1
                    localization.WriteByte(englishLoc(i))
                Next
            ElseIf language = "Spanish" Then
                localization.Position = spaAddress
                For i As Integer = 0 To (englishLoc.Length - 1) Step 1
                    localization.WriteByte(englishLoc(i))
                Next
            End If
            localization.Close()
            'End of language part
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
            swbf.Start()
            Dim swbfmemory As Process() = Process.GetProcessesByName(name)
            Dim resXAddress As Integer = &H7FCA4A
            Dim resYAddress As Integer = &H7FCA4C
            Dim resXDefaultValue As Integer = 0
            Dim resYDefaultValue As Integer = 0
            Dim resXValue As Integer = videoSettings.GetINIValue("Width")
            Dim resYValue As Integer = videoSettings.GetINIValue("Height")
            ReadProcessMemory(swbfmemory(0).Handle, resXAddress, resXDefaultValue, 2, 0)
            ReadProcessMemory(swbfmemory(0).Handle, resYAddress, resYDefaultValue, 2, 0)
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
            Loop Until counter = 2
        End If
        End
    End Sub

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
            Using settings As New System.IO.StreamWriter(settingsFile, False)
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
                settings.WriteLine("[Mods]")
                For i As Integer = 0 To 9 Step 1
                    settings.WriteLine("InstalledMod" & i & "=")
                Next
                settings.Close()
            End Using
        End If
    End Sub
End Class
