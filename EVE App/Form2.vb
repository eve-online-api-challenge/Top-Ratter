
Imports System
Imports System.IO
Imports System.Text
Public Class Form2
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ' Creates new entry of corporation 
        Dim CorpName As String
        Dim i As Integer

        Dim APIcode As String

        CorpName = TextBox2.Text
        keyId = TextBox1.Text
        vCode = TextBox3.Text

        If ToolStripComboBox1.SelectedItem = "" Or keyId = "" Or vCode = "" Or CorpName = "" Then
            MessageBox.Show("Something is empty, please fill in the fields")
            Me.Show()


        Else
            APIcode = "corpname:" & CorpName & "; keyid:" & keyId & "; vcode:" & vCode & "; walletdiv:" & ToolStripComboBox1.SelectedItem & ";"
            If Not File.Exists("Data/APIcodes.txt") Then
                Using sw As StreamWriter = File.CreateText("Data/APIcodes.txt")
                    sw.WriteLine(APIcode)
                End Using
            Else
                Using outputfile As New StreamWriter("Data/APIcodes.txt", True)
                    outputfile.WriteLine(APIcode)
                End Using
                Form1.Button1.Text = "Get Data"
                MsgBox("Entry added to database")
                Form3.Button2.Enabled = True
                Form3.Button3.Enabled = True
                Form3.TextBox1.Enabled = True
                Form3.TextBox2.Enabled = True
                Form3.TextBox3.Enabled = True
                Form3.ComboBox1.Enabled = True
                Form3.ComboBox2.Enabled = True
            End If

        End If
        Form1.PopulateToolStripCbox1()
        Form3.PopulateCombobox1(Form3.ComboBox1)
        Me.Close()

    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs) Handles Label4.Click
        Try
            System.Diagnostics.Process.Start("https://community.eveonline.com/support/api-key/CreatePredefined?accessMask=1048576")
        Catch
        End Try
    End Sub


    Private Sub ToolStripComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ToolStripComboBox1.SelectedIndexChanged
        If ToolStripComboBox1.SelectedItem = " " Then
            accountKey = "1000"

        End If

        Select Case ToolStripComboBox1.SelectedItem
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

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class