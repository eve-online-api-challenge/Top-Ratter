Imports System.Xml
Imports System.IO

Imports System.Text.RegularExpressions
Imports System.Threading
Public Class Form3
    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles Me.Load
        ComboBox2.Items.Add("1000")
        ComboBox2.Items.Add("1001")
        ComboBox2.Items.Add("1002")
        ComboBox2.Items.Add("1003")
        ComboBox2.Items.Add("1004")
        ComboBox2.Items.Add("1005")
        ComboBox2.Items.Add("1006")


        PopulateCombobox1(ComboBox1)


        If ComboBox1.Items.Count <= 0 Then
            Button2.Enabled = False
            Button3.Enabled = False

            TextBox1.Enabled = False
            TextBox2.Enabled = False
            TextBox3.Enabled = False
            ComboBox1.Enabled = False
            ComboBox2.Enabled = False

        Else
            Button2.Enabled = True
            Button3.Enabled = True
            TextBox1.Enabled = True
            TextBox2.Enabled = True
            TextBox3.Enabled = True
            ComboBox1.Enabled = True
            ComboBox2.Enabled = True
        End If


    End Sub
    Public Function PopulateCombobox1(cb As ComboBox)
        ' Sets the contents of combobox1
        '
        cb.Items.Clear()

        Dim i As Integer
        Dim rowkey As String
        Dim index As Integer
        Dim index2 As Integer
        For Each APIkeys As String In IO.File.ReadAllLines(Application.StartupPath & "\Data\APIcodes.txt")
            If APIkeys.StartsWith("corpname:") Then
                rowkey = APIkeys
                index = InStr(1, rowkey, ":", CompareMethod.Text) ' finds corp name start
                index2 = InStr(index, rowkey, ";", CompareMethod.Text) 'corpname end
                corpname = Mid(rowkey, index + 1, index2 - 1 - index) ' crops corpname
                i = cb.Items.Add(corpname) ' adds a corp name to combobox
            End If
        Next
        If cb.Items.Count >= 1 Then
            cb.SelectedIndex = 0
        End If


    End Function
    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs)
        Select Case ComboBox2.SelectedItem
            Case "1000"
                accountKey = "1000"
            Case "1001"
                accountKey = "1001"
            Case "1002"
                accountKey = "1002"
            Case "1003"
                accountKey = "1003"
            Case "1004"
                accountKey = "1004"
            Case "1005"
                accountKey = "1005"
            Case "1006"
                accountKey = "1006"
        End Select

    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        ReadAllKeys()
    End Sub
    Private Sub ReadAllKeys()
        'Reads the data form savefile
        '

        Dim rowkey As String
        Dim index As Integer
        Dim index2 As Integer
        Dim keyid As String
        Dim vcode As String
        Dim accountkey As String
        Dim corpname As String
        corpname = ComboBox1.SelectedItem


        For Each APIkeys As String In IO.File.ReadAllLines(Application.StartupPath & "\Data\APIcodes.txt")
            If APIkeys.StartsWith("corpname:" & ComboBox1.SelectedItem & ";") Then
                rowkey = APIkeys
            End If
        Next
        index = InStr(1, rowkey, ";", CompareMethod.Text)
        index = index + 1
        index = InStr(index, rowkey, ":", CompareMethod.Text)
        index2 = InStr(index, rowkey, ";", CompareMethod.Text)
        keyid = Mid(rowkey, index + 1, index2 - 1 - index)

        index = index + 1
        index = InStr(index, rowkey, ":", CompareMethod.Text)
        index2 = InStr(index2 + 1, rowkey, ";", CompareMethod.Text)
        vcode = Mid(rowkey, index + 1, index2 - 1 - index)

        index = index + 1
        index = InStr(index, rowkey, ":", CompareMethod.Text)
        index2 = InStr(index2 + 1, rowkey, ";", CompareMethod.Text)
        accountkey = Mid(rowkey, index + 1, index2 - 1 - index)

        TextBox1.Text = corpname
        TextBox2.Text = keyid
        TextBox3.Text = vcode
        ComboBox2.SelectedItem = accountkey

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        'Deletes the record from saved corporations
        '
        Dim rowkey As String
        Dim corpname As String
        Dim count As Integer
        count = 0
        corpname = ComboBox1.SelectedItem
        For Each APIkeys As String In IO.File.ReadAllLines(Application.StartupPath & "\Data\APIcodes.txt")
            If Not APIkeys.StartsWith("corpname:" & ComboBox1.SelectedItem & ";") Then
                rowkey = APIkeys
                count = count + 1

                If Not File.Exists("Data/newAPIcodes.txt") Then
                    Using sw As StreamWriter = File.CreateText("Data/newAPIcodes.txt")
                        sw.WriteLine(rowkey)
                    End Using
                Else
                    Using outputfile As New StreamWriter("Data/newAPIcodes.txt", True)
                        outputfile.WriteLine(rowkey)
                    End Using
                End If
            ElseIf count = 0 Then
                System.IO.File.WriteAllText("Data/APIcodes.txt", "")
                If Not File.Exists("Data/newAPIcodes.txt") Then
                    Using sw As StreamWriter = File.CreateText("Data/newAPIcodes.txt")
                        sw.WriteLine("")
                    End Using
                End If
            End If
        Next
        If Not File.Exists("Data/newAPIcodes.txt") Then
            Using sw As StreamWriter = File.CreateText("Data/newAPIcodes.txt")
                sw.WriteLine("")
            End Using
        End If
        File.Copy("Data/newAPIcodes.txt", "Data/APIcodes.txt", True)
        File.Delete("Data/newAPIcodes.txt")
        MsgBox("Corporation profile * " & ComboBox1.SelectedItem & " * removed")
        ComboBox1.Items.Clear()
        ComboBox1.Text = String.Empty
        ComboBox2.Text = String.Empty
        Form1.ComboBox1.Text = String.Empty
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        'added now
        amount = ""
        keyId = ""
        vCode = ""
        '--------
        Form1.ClearScreen()

        PopulateCombobox1(ComboBox1)
        Form1.PopulateToolStripCbox1()

        For Each APIkeys As String In IO.File.ReadAllLines(Application.StartupPath & "\Data\APIcodes.txt")
            If APIkeys.StartsWith("corpname:") Then


            Else
                Me.ComboBox1.Enabled = False
                Me.TextBox1.Enabled = False
                Me.TextBox2.Enabled = False
                Me.TextBox3.Enabled = False
                Me.ComboBox2.Enabled = False
                Me.Button3.Enabled = False
                Me.Button2.Enabled = False
            End If

        Next



        Me.Refresh()
        ' Application.Restart()

        ' Me.Controls.Clear() 'removes all the controls on the form
        ' InitializeComponent() 'load all the controls again
        ' formHome_Load(e, e) 'Load everything in your form load event again

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        'Modiefies already existing entries
        Dim keyId As String
        Dim vCode As String
        Dim accountKey As String
        Dim corpname As String
        Dim rowkey As String
        corpname = TextBox1.Text
        keyId = TextBox2.Text
        vCode = TextBox3.Text
        accountKey = ComboBox2.SelectedItem
        If corpname = "" Or keyId = "" Or vCode = "" Or accountKey = "" Then
            MessageBox.Show("Something is empty, please fill in the fields")
            Exit Sub
        End If
        For Each APIkeys As String In IO.File.ReadAllLines(Application.StartupPath & "\Data\APIcodes.txt")
            If Not APIkeys.StartsWith("corpname:" & ComboBox1.SelectedItem & ";") Then
                rowkey = APIkeys
                If Not File.Exists("Data/newAPIcodes.txt") Then
                    Using sw As StreamWriter = File.CreateText("Data/newAPIcodes.txt")
                        sw.WriteLine(rowkey)
                    End Using
                Else
                    Using outputfile As New StreamWriter("Data/newAPIcodes.txt", True)
                        outputfile.WriteLine(rowkey)
                    End Using
                End If
            ElseIf APIkeys.StartsWith("corpname:" & ComboBox1.SelectedItem & ";") Then
                rowkey = "corpname:" & corpname & "; keyid:" & keyId & "; vcode:" & vCode & "; walletdiv:" & ComboBox2.SelectedItem & ";"
                If Not File.Exists("Data/newAPIcodes.txt") Then
                    Using sw As StreamWriter = File.CreateText("Data/newAPIcodes.txt")
                        sw.WriteLine(rowkey)
                    End Using
                Else
                    Using outputfile As New StreamWriter("Data/newAPIcodes.txt", True)
                        outputfile.WriteLine(rowkey)
                    End Using
                End If
            End If
        Next
        File.Copy("Data/newAPIcodes.txt", "Data/APIcodes.txt", True)
        File.Delete("Data/newAPIcodes.txt")
        ComboBox1.Items.Clear()
        ComboBox1.Text = String.Empty
        ComboBox2.Text = String.Empty
        Form1.ComboBox1.Text = String.Empty
        Form1.ComboBox1.Items.Clear()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        PopulateCombobox1(ComboBox1)
        Form1.PopulateToolStripCbox1()
    End Sub
    Private Sub ComboBox1_Click(sender As Object, e As EventArgs) Handles ComboBox1.Click
        PopulateCombobox1(ComboBox1)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form2.Show()
    End Sub

    Private Sub Form3_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        'cheks if there is records and then changes button1 text accordingly

        If Form1.ComboBox1.Items.Count <= 1 Then
            '  cb.SelectedIndex = 0
            Form1.Button1.Text = "Import API"
        End If
        '   MsgBox(Form1.ComboBox1.Items.Count)
    End Sub

    Private Sub Form3_MouseClick(sender As Object, e As MouseEventArgs) Handles Me.MouseClick
        '    MsgBox("mouseclick")
    End Sub
End Class