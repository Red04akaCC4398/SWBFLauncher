Public Class Form2
    Private Function GetAddOnName(ByVal addOn As Object, ByVal directory As String) As String
        Dim name As String = ""
        Using addMe As New System.IO.StreamReader(directory & "\" & addOn.ToString & "\addme.script")
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
        End Using
        Return name
    End Function
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim modsDirectory As String = "Mods"
        Dim addOnDirectory As String = "AddOn"
        If System.IO.Directory.Exists(modsDirectory) Then
            For Each addOn As String In System.IO.Directory.GetDirectories(modsDirectory)
                If System.IO.File.Exists(addOn & "\addme.script") Then
                    ListBox1.Items.Add(System.IO.Path.GetFileName(addOn))
                End If
            Next
        End If
        If System.IO.Directory.Exists(addOnDirectory) Then
            For Each addOn As String In System.IO.Directory.GetDirectories(addOnDirectory)
                If System.IO.File.Exists(addOn & "\addme.script") Then
                    ListBox2.Items.Add(System.IO.Path.GetFileName(addOn))
                End If
            Next
        End If
        Button1.Enabled = False
        Button2.Enabled = False
        Button6.Enabled = False
        If ListBox1.Items.Count = 0 Then
            Button3.Enabled = False
        End If
        If ListBox2.Items.Count = 0 Then
            Button5.Enabled = False
        End If
    End Sub
    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Dim modsDirectory As String = "Mods"
        Dim addInfoImage As String = "addinfo.png"
        Dim addInfoFile As String = "addinfo.txt"
        Dim addOn As Object = ListBox1.SelectedItem
        Button1.Enabled = False
        Button2.Enabled = True
        Button6.Enabled = True
        If Not addOn Is Nothing Then
            If System.IO.File.Exists(modsDirectory & "\" & addOn.ToString & "\" & addInfoImage) Then
                Dim mapPreview As New System.IO.FileStream(modsDirectory & "\" & addOn.ToString & "\" & addInfoImage, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                PictureBox1.Image = Image.FromStream(mapPreview)
                mapPreview.Close()
            Else
                PictureBox1.Image = Nothing
            End If
            TextBox1.Text = GetAddOnName(addOn, modsDirectory)
            If System.IO.File.Exists(modsDirectory & "\" & addOn.ToString & "\" & addInfoFile) Then
                Using addInfo As New System.IO.StreamReader(modsDirectory & "\" & addOn.ToString & "\" & addInfoFile)
                    TextBox2.Text = addInfo.ReadLine
                    TextBox3.Clear()
                    Do
                        TextBox3.Text += addInfo.ReadLine
                        TextBox3.Text += vbCrLf
                    Loop Until addInfo.EndOfStream
                    addInfo.Close()
                End Using
            Else
                TextBox2.Text = "Unknown"
                TextBox3.Text = "Not available."
            End If
            If ListBox1.SelectedIndex >= 0 Then
                TextBox4.Text = "Not added"
                TextBox4.BackColor = Color.Red
            End If
        End If
    End Sub

    Private Sub ListBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox2.SelectedIndexChanged
        Dim addOnDirectory As String = "AddOn"
        Dim addInfoImage As String = "addinfo.png"
        Dim addInfoFile As String = "addinfo.txt"
        Dim addOn As Object = ListBox2.SelectedItem
        Button1.Enabled = True
        Button2.Enabled = False
        Button6.Enabled = False
        If Not addOn Is Nothing Then
            If System.IO.File.Exists(addOnDirectory & "\" & addOn.ToString & "\" & addInfoImage) Then
                Dim mapPreview As New System.IO.FileStream(addOnDirectory & "\" & addOn.ToString & "\" & addInfoImage, System.IO.FileMode.Open, System.IO.FileAccess.Read)
                PictureBox1.Image = Image.FromStream(mapPreview)
                mapPreview.Close()
            Else
                PictureBox1.Image = Nothing
            End If
            TextBox1.Text = GetAddOnName(addOn, addOnDirectory)
            If System.IO.File.Exists(addOnDirectory & "\" & addOn.ToString & "\" & addInfoFile) Then
                Using addInfo As New System.IO.StreamReader(addOnDirectory & "\" & addOn.ToString & "\" & addInfoFile)
                    TextBox2.Text = addInfo.ReadLine
                    TextBox3.Clear()
                    Do
                        TextBox3.Text += addInfo.ReadLine
                        TextBox3.Text += vbCrLf
                    Loop Until addInfo.EndOfStream
                    addInfo.Close()
                End Using
            Else
                TextBox2.Text = "Unknown"
                TextBox3.Text = "Not available."
            End If
            If ListBox2.SelectedIndex >= 0 Then
                TextBox4.Text = "Added"
                TextBox4.BackColor = Color.Green
            End If
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Me.Close()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim modsDirectory As String = "Mods"
        Dim addOnDirectory As String = "AddOn"
        Dim addOn As Object = ListBox2.SelectedItem
        PictureBox1.Image = Nothing
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox4.BackColor = Nothing
        System.IO.Directory.Move(addOnDirectory & "\" & addOn.ToString, modsDirectory & "\" & addOn.ToString)
        ListBox1.Items.Add(addOn)
        ListBox2.Items.Remove(addOn)
        Button1.Enabled = False
        Button2.Enabled = False
        Button6.Enabled = False
        If ListBox1.Items.Count = 0 Then
            Button3.Enabled = False
        Else
            Button3.Enabled = True
        End If
        If ListBox2.Items.Count = 0 Then
            Button5.Enabled = False
        Else
            Button5.Enabled = True
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim modsDirectory As String = "Mods"
        Dim addOnDirectory As String = "AddOn"
        Dim addOn As Object = ListBox1.SelectedItem
        PictureBox1.Image = Nothing
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox4.BackColor = Nothing
        System.IO.Directory.Move(modsDirectory & "\" & addOn.ToString, addOnDirectory & "\" & addOn.ToString)
        ListBox2.Items.Add(addOn)
        ListBox1.Items.Remove(addOn)
        Button1.Enabled = False
        Button2.Enabled = False
        Button6.Enabled = False
        If ListBox1.Items.Count = 0 Then
            Button3.Enabled = False
        Else
            Button3.Enabled = True
        End If
        If ListBox2.Items.Count = 0 Then
            Button5.Enabled = False
        Else
            Button5.Enabled = True
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim modsDirectory As String = "Mods"
        Dim addOnDirectory As String = "AddOn"
        Dim addOn As Object
        PictureBox1.Image = Nothing
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox4.BackColor = Nothing
        Do
            addOn = ListBox1.Items.Item(0)
            System.IO.Directory.Move(modsDirectory & "\" & addOn.ToString, addOnDirectory & "\" & addOn.ToString)
            ListBox2.Items.Add(addOn)
            ListBox1.Items.Remove(addOn)
        Loop Until ListBox1.Items.Count = 0
        Button1.Enabled = False
        Button2.Enabled = False
        Button3.Enabled = False
        Button5.Enabled = True
        Button6.Enabled = False
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim modsDirectory As String = "Mods"
        Dim addOnDirectory As String = "AddOn"
        Dim addOn As Object
        PictureBox1.Image = Nothing
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox4.BackColor = Nothing
        Do
            addOn = ListBox2.Items.Item(0)
            System.IO.Directory.Move(addOnDirectory & "\" & addOn.ToString, modsDirectory & "\" & addOn.ToString)
            ListBox1.Items.Add(addOn)
            ListBox2.Items.Remove(addOn)
        Loop Until ListBox2.Items.Count = 0
        Button1.Enabled = False
        Button2.Enabled = False
        Button5.Enabled = False
        Button3.Enabled = True
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim modsDirectory As String = "Mods"
        Dim addOn As Object = ListBox1.SelectedItem
        PictureBox1.Image = Nothing
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox4.Clear()
        TextBox4.BackColor = Nothing
        System.IO.Directory.Delete(modsDirectory & "\" & addOn.ToString, True)
        ListBox1.Items.Remove(addOn)
    End Sub
End Class