Imports System.Xml
Imports System.IO

Imports System.Text.RegularExpressions
Imports System.Threading

Public Module GlobalVariables
    Public startDateDTBox1 As String
    Public finishDateDTBox2 As String
    Public d1 As String
    Public characterToSearch As Char = Convert.ToChar(34)
    Public ownername As String = "ownerName2="
    Public amount As String = "amount="
    Public keyId As String
    Public vCode As String
    Public corpname As String
    Public accountKey As String
    Public byPlayerDictionary As New Dictionary(Of String, Decimal)
    Public byDayDictionary As New Dictionary(Of String, Decimal)
    Public resultDate As String
    Public dateStatusS As Integer
    Public dateStatusF As Integer
    Public playerName As String
    Public playerIsk As Decimal
    Public refTypeId As Integer

End Module
Public Class Form1
    Private Function ReadXmlFromFileAndFindData2()
        'Reads the data from xml file with help of other subfunctions and displays it on the screen
        '
        Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
        Dim target As String = "date="
        ClearScreen()
        GetTimePeriodFromDateTimeBoxes()
        If keyId = "" Or vCode = "" Or amount = "" Then
            MsgBox("Missing keyID or Vcode please very it is correct in API Management")
            Button1.Text = "Import API"
            Progresbar1.Value = 10
            ToolStripStatusLabel1.Text = "Ready"
            ClearScreen()

        Else
            If System.IO.File.Exists(Application.StartupPath & "\Data\Corp_API_Data.xml") = True Then
                For Each line As String In IO.File.ReadAllLines(Application.StartupPath & "\Data\Corp_API_Data.xml")
                    If line.StartsWith("      <row") Then
                        resultDate = SearchStringMod1(line, target)
                        resultDate = Microsoft.VisualBasic.Left(resultDate, 10)
                        dateStatusS = CompareDate(startDateDTBox1, resultDate)
                        dateStatusF = CompareDate(finishDateDTBox2, resultDate)
                        If ((dateStatusS = 0 Or dateStatusS = 1) And (dateStatusF = 0 Or dateStatusF = -1)) Then
                            refTypeId = SearchStringMod1(line, "refTypeID=")
                            If (refTypeId = 85) Or (refTypeId = 17) Or (refTypeId = 34) Or (refTypeId = 33) Or (refTypeId = 99) Then
                                playerName = SearchStringMod1(line, ownername)
                                playerIsk = SearchStringMod1(line, amount)
                                AddItemsDictionary(playerName, playerIsk, byPlayerDictionary)
                                AddItemsDictionary(resultDate, playerIsk, byDayDictionary)
                            End If
                        End If
                    Else
                    End If
                Next
                Progresbar1.Value = 4
                ShowDataInList(byPlayerDictionary)
            Else
                MsgBox("Problem getting data from EVE servers OR invalid keyID/vCode")
                Progresbar1.Value = 10
                '  MsgBox("Can't read data from file!")
            End If
        End If
    End Function
    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click

        Dim rowkey As String
        Dim index As Integer
        Dim index2 As Integer
        If DateTimePicker1.Value > DateTime.Now Then
            MsgBox("Set FROM date to today or lower")
            DateTimePicker1.Value = DateTime.Now
            Exit Sub
        End If
        If Button1.Text = "Import API" Then
            Form3.Show()
        Else
            Progresbar1.Value = 1
            ToolStripStatusLabel1.Text = "Processing"
            Try
                For Each line As String In IO.File.ReadAllLines(Application.StartupPath & "\Data\APIcodes.txt")
                    If line.StartsWith("corpname:" & ComboBox1.SelectedItem & ";") Then ' gets the line where the corp name matches with the combobox item
                        rowkey = line
                        index = InStr(1, rowkey, ";", CompareMethod.Text)
                        index = index + 1
                        index = InStr(index, rowkey, ":", CompareMethod.Text)
                        index2 = InStr(index, rowkey, ";", CompareMethod.Text)
                        keyId = Mid(rowkey, index + 1, index2 - 1 - index) 'gets the key id from file

                        index = index + 1
                        index = InStr(index, rowkey, ":", CompareMethod.Text)
                        index2 = InStr(index2 + 1, rowkey, ";", CompareMethod.Text)
                        vCode = Mid(rowkey, index + 1, index2 - 1 - index) 'gets the vcode from file

                        index = index + 1
                        index = InStr(index, rowkey, ":", CompareMethod.Text)
                        index2 = InStr(index2 + 1, rowkey, ";", CompareMethod.Text)
                        accountKey = Mid(rowkey, index + 1, index2 - 1 - index) 'gets the accountkey from file
                    End If

                Next
            Catch ex As Exception
                MsgBox("Something went wrong :( ")
            End Try
            CreateXmlApi(keyId, vCode, accountKey)
            Progresbar1.Value = 3
            ReadXmlFromFileAndFindData2()
            If Progresbar1.Value = 10 Then
                ToolStripStatusLabel1.Text = "Ready"
            End If
        End If
    End Sub
    Public Sub GetTimePeriodFromDateTimeBoxes()
        'Acquires the date from both date time pickers.
        '
        Dim d1 As DateTime = Format(DateTimePicker1.Value, "yyyy-MM-dd")
        Dim d2 As DateTime = Format(DateTimePicker2.Value, "yyyy-MM-dd")
        Dim dateChek As Int16 = DateTime.Compare(d1, d2)


        If DateTimePicker1.Value <= DateTime.Now Then
            If (dateChek <= 0) Then
                startDateDTBox1 = d1.ToString("yyyy-MM-dd")
                finishDateDTBox2 = d2.ToString("yyyy-MM-dd")
            Else
                MsgBox("Date 'FROM' must be earlier than date 'TO' ")
                Exit Sub
            End If
        Else
            MsgBox("Set FROM date to today or lower")
            Exit Sub
        End If

        ''x<y  : x is less than y
        'If (dateChek < 0) Then
        '    startDateDTBox1 = d1.ToString("yyyy-MM-dd")
        '    finishDateDTBox2 = d2.ToString("yyyy-MM-dd")
        'Else(If datechek=0)


        '    MsgBox("Date 'FROM' must be earlier than date 'TO' ")
        '    Exit Sub
        'End If
        ' if date 1 is less than date 2 then return value is  < 0 
        ' id date 1 is  bigger than date 2 then return value is 
    End Sub
    Public Function Createdb()
        'Creates the file system for storing data
        '
        CreateFile("\License.txt")
        Dim streamWr As New IO.StreamWriter(Application.StartupPath & "\License.txt")
        streamWr.WriteLine("Copyright © 2016 <Uģis Varslavans , Matthew de Beer>")
        streamWr.WriteLine(" ")
        streamWr.WriteLine("Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the " & characterToSearch & "Software" & characterToSearch & "), to deal in the software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:")
        streamWr.WriteLine(" ")
        streamWr.WriteLine("The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.")
        streamWr.WriteLine(" ")
        streamWr.WriteLine("THE SOFTWARE IS PROVIDED " & characterToSearch & "As Is" & characterToSearch & ", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.")
        streamWr.WriteLine(" ")
        streamWr.Close()
        streamWr.Dispose()

        If My.Computer.FileSystem.DirectoryExists(Application.StartupPath & "\Data") Then
            'cheks if the files exist.
            If My.Computer.FileSystem.FileExists(Application.StartupPath & "\Data\APIcodes.txt") Then
            Else
                CreateFile("\Data\APIcodes.txt")
            End If
            If My.Computer.FileSystem.FileExists(Application.StartupPath & "\ Data \ Corp_API_Data.xml") Then
            Else
                CreateFile("\Data\Corp_API_Data.xml")
            End If
        Else
                My.Computer.FileSystem.CreateDirectory(Application.StartupPath & "\Data")
            CreateFile("\Data\APIcodes.txt")
            CreateFile("\Data\Corp_API_Data.xml")
        End If
        If My.Computer.FileSystem.DirectoryExists(Application.StartupPath & "\Logs") Then
        Else
            My.Computer.FileSystem.CreateDirectory(Application.StartupPath & "\Logs")
        End If

    End Function
    Public Function CreateFile(fileName As String)
        'Creates the file in the directory as specified as in parameter
        '
        If My.Computer.FileSystem.FileExists(Application.StartupPath & fileName) Then
        Else
            If Not File.Exists(Application.StartupPath & fileName) Then
                Dim theFile As FileStream = File.Create(Application.StartupPath & fileName)
                theFile.Close()
            End If
        End If
    End Function
    Public Function SearchStringMod1(stringreader As String, target As String)
        'Finds the value of the searched opbject from the line as follows -> Look_for_this="get_this_as_answer"
        '
        Dim retVal As String
        Dim nameStartPosition As Integer
        Dim nameFinishPosition As Integer
        Dim characterToSearch As Char = Convert.ToChar(34)
        Dim x As Integer = Findstring1(1, target, stringreader)
        If x = 0 Then
            retVal = 0
            Return retVal
        Else
            nameStartPosition = Findstring1(x, characterToSearch, stringreader)
            nameFinishPosition = Findstring1(nameStartPosition + 1, characterToSearch, stringreader)
            retVal = CutStringBetween(nameStartPosition, nameFinishPosition, stringreader)
            Return retVal
        End If
    End Function
    Public Function Findstring1(startposition As Integer, searchChar As String, stringData As String)
        'Finds the start position of searched string
        Dim retVal As Integer
        retVal = InStr(startposition, stringData, searchChar, CompareMethod.Text)
        Return retVal
    End Function
    Public Function CutStringBetween(start As Integer, finish As Integer, stringData As String)
        'Cuts the string from-to given values
        Dim lengt As Integer = (finish - start) - 1
        Dim retVal As String = stringData.Substring(start, lengt)
        Return retVal
    End Function
    Private Function CreateXmlApi(keyId As String, vCode As String, accountKey As String)
        'generate XML file from server API and saves it.
        Dim doc As XmlDocument
        doc = New XmlDocument()

        Try
            Dim api As String = "https://api.eveonline.com//corp/WalletJournal.xml.aspx?keyID=" & keyId & "&vCode=" & vCode & "&accountKey=" & accountKey & "&rowCount=2560"
            doc.Load(api)

            doc.Save(Application.StartupPath & "\Data\Corp_API_Data.xml")

        Catch ex As System.Exception
            If File.Exists(Application.StartupPath & "\Data\Corp_API_Data.xml") Then
                My.Computer.FileSystem.DeleteFile(Application.StartupPath & "\Data\Corp_API_Data.xml",
Microsoft.VisualBasic.FileIO.UIOption.AllDialogs,
Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin)
            End If

        End Try
    End Function
    Private Sub AddItemsDictionary(str As String, isk As Decimal, dict As Dictionary(Of String, Decimal))
        'Adds items to the the summ of value to the key in dictionary
        '
        Dim value As Decimal
        If (dict.TryGetValue(str, value)) Then
            dict(str) = value + isk
        Else
            dict.Add(str, isk)
        End If
    End Sub
    Private Function ShowDataInList(dict As Dictionary(Of String, Decimal))
        'Generates the data from dictionary 
        '
        Dim sortedDict = (From entry In dict Order By entry.Value Descending).ToDictionary(Function(pair) pair.Key, Function(pair) pair.Value)
        Dim counter As Integer = 0
        Dim summa As Decimal
        Dim topRAt As Decimal = 0
        Dim topname As String
        Progresbar1.Value = 6
        For Each entry As KeyValuePair(Of String, Decimal) In sortedDict
            counter = counter + 1
            Dim justName As String = String.Format(counter & ". {0}", entry.Key)
            Dim justIsk As String = String.Format("{0}", Format(entry.Value, "##,###,###,###.00"))
            summa += entry.Value
            If entry.Value > topRAt Then
                topRAt = entry.Value
                topname = entry.Key
            End If
            ListBox1.Items.Add(justName)
            ListBox2.Items.Add(justIsk)
        Next
        Progresbar1.Value = 8
        Label3.Text = String.Format("{0}", Format(summa, "##,###,###,###.00"))

        'valuecannot be null 
        'by wrong load file
        Label4.Text = String.Format(topname)
        Label8.Text = String.Format("{0}", Format(topRAt, "##,###,###,###.00"))
        PopulateChart(dict)
        Progresbar1.Value = 10
    End Function
    Private Sub APIKeySettingsToolStripMenuItem_Click(sender As Object, e As EventArgs)
        Form2.Show()
    End Sub
    Private Function PopulateChart(dict As Dictionary(Of String, Decimal))
        ' Populates the chart1 and chart2
        '
        Chart1.ChartAreas("ChartArea1").AxisX.MinorTickMark.Enabled = True
        Chart1.ChartAreas("ChartArea1").AxisX.Interval = 1
        Chart1.Series("ISK").IsVisibleInLegend = False
        Chart1.ChartAreas("ChartArea1").AxisY.Title = "ISK"
        Chart2.ChartAreas("ChartArea1").AxisX.MinorTickMark.Enabled = True
        Chart2.ChartAreas("ChartArea1").AxisX.Interval = 1
        Chart2.Series("Series1").IsVisibleInLegend = False
        Chart2.ChartAreas("ChartArea1").AxisY.Title = "ISK"
        'populates chart1
        Dim list As List(Of KeyValuePair(Of String, Decimal)) = dict.ToList
        For Each pair As KeyValuePair(Of String, Decimal) In list
            Chart1.Series("ISK").Points.AddXY(pair.Key, pair.Value)
        Next
        'populates chart2
        Dim list2 As List(Of KeyValuePair(Of String, Decimal)) = byDayDictionary.ToList
        For Each pair As KeyValuePair(Of String, Decimal) In list2
            Chart2.Series("Series1").Points.AddXY(pair.Key, pair.Value)
        Next
    End Function
    Private Function CompareDate(xmlDate As String, boxDate As String)
        'Compares the time and returns a value
        '

        'x<y  : x is less than y
        Dim ifDateIsBiggerOrLess As Int16 = DateTime.Compare(boxDate, xmlDate)
        If (ifDateIsBiggerOrLess < 0) Then
            'Return 0
            'if the first  value is smaller (eaarlier) than second value it returns " -1"
            Return -1
        ElseIf (ifDateIsBiggerOrLess = 0) Then
            Return 0
        Else
            'when t1 is later than t2 it gives "1"
            Return 1
        End If
    End Function
    Private Sub SaveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToolStripMenuItem.Click
        'Saves the file with data
        '
        Dim saveString As String = "\Logs\" & startDateDTBox1 & "_" & finishDateDTBox2 & ".txt"
        SaveListToFile(saveString)
    End Sub
    Private Function SaveListToFile(filename As String)
        'Writes the data in file
        '
        CreateFile(filename)
        Dim streamWr As New IO.StreamWriter(Application.StartupPath & filename)
        If byPlayerDictionary.Count > 0 Then
            Dim list As List(Of KeyValuePair(Of String, Decimal)) = byPlayerDictionary.ToList
            For Each pair As KeyValuePair(Of String, Decimal) In list
                streamWr.WriteLine("p>" & pair.Key & " , " & pair.Value)
            Next
            Dim list2 As List(Of KeyValuePair(Of String, Decimal)) = byDayDictionary.ToList
            For Each pair As KeyValuePair(Of String, Decimal) In list2
                streamWr.WriteLine("d>" & pair.Key & " , " & pair.Value)
            Next
            streamWr.Close()
            streamWr.Dispose()
            MsgBox("Saved -> " & filename)
        End If

    End Function
    Private Sub LoadToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoadToolStripMenuItem.Click
        'Loads the file from directory
        '
        Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
        Dim fd As OpenFileDialog = New OpenFileDialog()
        Dim strFileName As String
        ToolStripStatusLabel1.Text = "Processing"
        Progresbar1.Value = 2
        fd.Title = "Open File Dialog"
        fd.InitialDirectory = Application.StartupPath & "\Logs"
        fd.Filter = "All files (*.*)|*.*|All files (*.*)|*.*"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True
        If fd.ShowDialog() = DialogResult.OK Then
            strFileName = fd.FileName
            LoadFile(strFileName)
        End If
    End Sub
    Public Function ClearScreen()
        'Clears the Charts, listbox and dictironaries
        '
        byPlayerDictionary.Clear()
        byDayDictionary.Clear()
        ListBox1.Items.Clear()
        ListBox2.Items.Clear()
        Chart1.Series(0).Points.Clear()
        Chart2.Series(0).Points.Clear()
        Label3.Text = ""
        Label8.Text = ""
        Label4.Text = ""
        Chart1.Series("ISK").IsVisibleInLegend = False
        Chart2.Series("Series1").IsVisibleInLegend = False
    End Function
    Public Function LoadFile(fileName As String)
        'Reads the loaded file and populates the charts
        '
        Dim sortedDict = (From entry In byPlayerDictionary Order By entry.Value Descending).ToDictionary(Function(pair) pair.Key, Function(pair) pair.Value)
        ClearScreen()
        Dim part1 As String
        Dim part2 As Decimal
        Dim posX As Integer
        Progresbar1.Value = 4
        For Each line As String In IO.File.ReadAllLines(fileName)
            If line.StartsWith("p>") Then
                posX = Findstring1(1, ",", line)
                part1 = line.Substring(2, posX - 3)
                part2 = line.Substring(posX, line.Length - posX)
                AddItemsDictionary(part1, part2, byPlayerDictionary)
            ElseIf line.StartsWith("d>")
                posX = Findstring1(1, ",", line)
                part1 = line.Substring(2, posX - 3)
                part2 = line.Substring(posX, line.Length - posX)
                AddItemsDictionary(part1, part2, byDayDictionary)
            Else

                AddItemsDictionary("Spai awoxes your wallet", 6984548643, byPlayerDictionary)
                AddItemsDictionary("It's very effective", 9, byPlayerDictionary)
                AddItemsDictionary(" > >       ;..;      < < <", 8, byPlayerDictionary)
                AddItemsDictionary(" > > Btw, wrong file < < <", 7, byPlayerDictionary)
                AddItemsDictionary(" > >                 < < <", 6, byPlayerDictionary)
                AddItemsDictionary(" > > try File->Save  < < <", 5, byPlayerDictionary)
                AddItemsDictionary(" > > Then File->Load < < <", 4, byPlayerDictionary)
                AddItemsDictionary(" > > the saved data. < < <", 3, byPlayerDictionary)




                AddItemsDictionary("Yesterday", 78963854, byDayDictionary)
                AddItemsDictionary("Today", 463156485, byDayDictionary)
                AddItemsDictionary("Tomorow", 99321657656, byDayDictionary)
                Progresbar1.Value = 10
                ToolStripStatusLabel1.Text = "Ready"
                ShowDataInList(byPlayerDictionary)

                MsgBox("Wrong format for file, please select correct file located in " & Application.StartupPath & "\logs")
                Exit Function
            End If
        Next
        ShowDataInList(byPlayerDictionary)
        If Progresbar1.Value = 10 Then
            ToolStripStatusLabel1.Text = "Ready"
        End If
    End Function

    Function PopulateToolStripCbox1()
        ComboBox1.Items.Clear()

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
                i = ComboBox1.Items.Add(corpname) ' adds a corp name to combobox
            Else
                'do nothing

            End If

        Next
        If ComboBox1.Items.Count >= 1 Then
            ComboBox1.SelectedIndex = 0
            Button1.Text = "Get Data"
        Else
            'load add new api
        End If
    End Function
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Createdb()
        ClearScreen()

        DateTimePicker1.Value = DateTime.Now.AddDays(-7)
        DateTimePicker2.Value = DateTime.Now

        If My.Computer.FileSystem.FileExists(Application.StartupPath & "\Data\APIcodes.txt") Then

            PopulateToolStripCbox1()
            If ComboBox1.Items.Count >= 1 Then
                Button1.Text = "Get data"
                ' run normaly
            Else

                'run add new api

            End If

        End If


    End Sub
    Private Sub InfoToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InfoToolStripMenuItem.Click
        Form4.Show()
    End Sub

    Private Sub APIManageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles APIManageToolStripMenuItem.Click
        Form3.Show()

    End Sub

    Private Sub ComboBox1_Click(sender As Object, e As EventArgs) Handles ComboBox1.Click
        PopulateToolStripCbox1()
    End Sub
End Class
