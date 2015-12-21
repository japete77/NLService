Imports System.ServiceModel
Imports System.ServiceModel.Description
Imports AudioBook.NuevaLuz.AudioBooks
Imports System.ServiceProcess
Imports System.ComponentModel
Imports System.Configuration.Install

Public Class NLService
    Inherits ServiceBase

    Dim serviceHost As ServiceHost = Nothing
    Dim logWritter As Log

    Public Shared Sub Main()
        ServiceBase.Run(New NLService())
    End Sub

    Protected Overrides Sub OnStart(ByVal args() As String)
        Try
            logWritter = New Log()

            logWritter.WriteLine("Starting Nueva Luz Service...")

            If serviceHost IsNot Nothing Then
                serviceHost.Close()
            End If

            serviceHost = New ServiceHost(GetType(AudioBookService))

            ' Enable CORS
            For Each EP As ServiceEndpoint In serviceHost.Description.Endpoints
                EP.Behaviors.Add(New BehaviorAttribute())
            Next

            serviceHost.Open()

            logWritter.WriteLine("<Nueva Luz> Audio Books Service is live now at: " + serviceHost.BaseAddresses.First().AbsoluteUri)

        Catch ex As Exception
            logWritter.WriteLine(ex.Message)
        End Try
    End Sub

    Protected Overrides Sub OnStop()
        Try
            logWritter.WriteLine("Shutting down Nueva Luz Service...")

            serviceHost.Close()
            serviceHost = Nothing

            logWritter.WriteLine("Service closed")

        Catch ex As Exception
            logWritter.WriteLine(ex.Message)
        End Try
    End Sub

End Class

<RunInstaller(True)>
Public Class ProjectInstaller
    Inherits Installer
    Private process As ServiceProcessInstaller
    Private service As ServiceInstaller

    Public Sub New()
        process = New ServiceProcessInstaller()
        process.Account = ServiceAccount.LocalSystem
        service = New ServiceInstaller()
        service.ServiceName = "Nueva Luz Service"
        Installers.Add(process)
        Installers.Add(service)
    End Sub
End Class
