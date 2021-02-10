Imports winini
Public Class Form4
    Private Sub Form4_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim section As String() = {"Game", "Video", "Audio"}
        Dim settingsFile As String = "settings.ini"
        Dim lvlDirectory As String = "Data\_LVL_PC"
        Dim gameSettings As INI = New INI
        Dim videoSettings As INI = New INI
        Dim audioSettings As INI = New INI
        gameSettings.LoadINIFile(section(0), settingsFile)
        videoSettings.LoadINIFile(section(1), settingsFile)
        audioSettings.LoadINIFile(section(2), settingsFile)
        ComboBox1.Text = gameSettings.GetINIValue("Language")
        ComboBox2.Text = videoSettings.GetINIValue("Width") & "x" & videoSettings.GetINIValue("Height")
        If videoSettings.GetINIValue("WindowedMode") = 1 Then
            CheckBox1.Checked = True
        End If
        If videoSettings.GetINIValue("NoIntro") = 1 Then
            CheckBox2.Checked = True
        End If
        If videoSettings.GetINIValue("NoMovies") = 1 Then
            CheckBox3.Checked = True
        End If
        If audioSettings.GetINIValue("NoStartupMusic") = 1 Then
            CheckBox4.Checked = True
        End If
        TextBox1.Text = audioSettings.GetINIValue("AudioSampleRate")
        If audioSettings.GetINIValue("SetCustomAudioSampleRate") = 1 Then
            CheckBox5.Checked = True
            TextBox1.Enabled = True
        End If
        TextBox2.Text = audioSettings.GetINIValue("AudioMixBuffer")
        If audioSettings.GetINIValue("SetCustomAudioMixBuffer") = 1 Then
            CheckBox6.Checked = True
            TextBox2.Enabled = True
        End If
        If System.IO.File.Exists(lvlDirectory & "\vidmode.ini") Then
            Using resolution As New System.IO.StreamReader(lvlDirectory & "\vidmode.ini")
                Dim endOfResolution As Boolean = False
                Dim aux As String = ""
                Dim auxResolution As String = ""
                'Dim auxSize As Integer = 100
                Dim count As Integer
                Dim num As Integer = 0
                Dim auxNum As Integer = 0
                resolution.ReadLine()
                Do
                    aux = resolution.ReadLine()
                    count = 0
                    auxResolution = ""
                    For Each letter In aux
                        If letter = " " Then
                            count += 1
                            If count < 2 Then
                                auxResolution += "x"
                            End If
                        Else
                            If count = 0 Then
                                auxResolution += letter
                            End If
                            If count = 1 Then
                                auxResolution += letter
                            End If
                            If count = 2 Then
                                num += letter.ToString
                            End If
                        End If
                    Next
                    If auxNum < num Then
                        endOfResolution = True
                    Else
                        ComboBox2.Items.Add(auxResolution)
                    End If
                    auxNum = num
                Loop Until endOfResolution Or resolution.EndOfStream
                resolution.Close()
            End Using
        End If
    End Sub

    Private Sub CheckBox5_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox5.CheckedChanged
        If CheckBox5.Checked = True Then
            TextBox1.Enabled = True
        Else
            TextBox1.Enabled = False
        End If
    End Sub

    Private Sub CheckBox6_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox6.CheckedChanged
        If CheckBox6.Checked = True Then
            TextBox2.Enabled = True
        Else
            TextBox2.Enabled = False
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form5.Show()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim section As String() = {"Game", "Video", "Audio", "Mods"}
        Dim settingsFile As String = "settings.ini"
        Dim gameSettings As INI = New INI
        Dim videoSettings As INI = New INI
        Dim audioSettings As INI = New INI
        Dim modsSettings As INI = New INI
        Dim width As String = ""
        Dim height As String = ""
        Dim splitted As Boolean = False
        gameSettings.LoadINIFile(section(0), settingsFile)
        videoSettings.LoadINIFile(section(1), settingsFile)
        audioSettings.LoadINIFile(section(2), settingsFile)
        modsSettings.LoadINIFile(section(3), settingsFile)
        gameSettings.SetINIValue("Language", ComboBox1.Text)
        For Each symbol In ComboBox2.Text
            If symbol = "x" Then
                splitted = True
            Else
                If Not splitted Then
                    width += symbol
                Else
                    height += symbol
                End If
            End If
        Next
        videoSettings.SetINIValue("Width", width)
        videoSettings.SetINIValue("Height", height)
        If CheckBox1.Checked = False Then
            videoSettings.SetINIValue("WindowedMode", 0)
        Else
            videoSettings.SetINIValue("WindowedMode", 1)
        End If
        If CheckBox2.Checked = False Then
            videoSettings.SetINIValue("NoIntro", 0)
        Else
            videoSettings.SetINIValue("NoIntro", 1)
        End If
        If CheckBox3.Checked = False Then
            videoSettings.SetINIValue("NoMovies", 0)
        Else
            videoSettings.SetINIValue("NoMovies", 1)
        End If
        If CheckBox4.Checked = False Then
            audioSettings.SetINIValue("NoStartupMusic", 0)
        Else
            audioSettings.SetINIValue("NoStartupMusic", 1)
        End If
        If CheckBox5.Checked = False Then
            audioSettings.SetINIValue("SetCustomAudioSampleRate", 0)
        Else
            audioSettings.SetINIValue("SetCustomAudioSampleRate", 1)
            audioSettings.SetINIValue("AudioSampleRate", TextBox1.Text)
        End If
        If CheckBox6.Checked = False Then
            audioSettings.SetINIValue("SetCustomAudioMixBuffer", 0)
        Else
            audioSettings.SetINIValue("SetCustomAudioMixBuffer", 1)
            audioSettings.SetINIValue("AudioMixBuffer", TextBox2.Text)
        End If
        gameSettings.SaveINIFile(True)
        videoSettings.SaveINIFile(False)
        audioSettings.SaveINIFile(False)
        modsSettings.SaveINIFile(False)
        Me.Close()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
    End Sub
End Class