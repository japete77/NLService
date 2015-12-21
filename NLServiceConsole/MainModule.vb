Imports System.ServiceModel
Imports System.ServiceModel.Description
Imports AudioBook.NuevaLuz.AudioBooks

Module MainModule

    Sub Main()

        Dim serviceHost As ServiceHost = Nothing

        Try
            serviceHost = New ServiceHost(GetType(AudioBookService))

            ' Enable CORS
            For Each EP As ServiceEndpoint In serviceHost.Description.Endpoints
                EP.Behaviors.Add(New BehaviorAttribute())
            Next

            ' Activate Service
            serviceHost.Open()
            Console.WriteLine("<Nueva Luz> Audio Books Service is live now at:" & vbLf & " {0}" & vbLf, serviceHost.BaseAddresses.First().AbsoluteUri)
            Console.WriteLine("Press Ctrl + C to exit.")

            While True
                Console.ReadKey()
            End While
        Catch ex As Exception
            Console.WriteLine([String].Format("Error starting the service: {0}" & vbLf, ex.Message))
        End Try

        Console.ReadKey()
    End Sub

End Module
