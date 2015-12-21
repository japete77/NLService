Imports System.IO
Imports System.Reflection

Public Class Log
    Dim logWriter As TextWriterTraceListener

    Public Sub New()
        logWriter = New TextWriterTraceListener(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\NLService.log")
    End Sub

    Public Sub WriteLine(Message As String)
        logWriter.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss - ") + Message)
        logWriter.Flush()
    End Sub
End Class
