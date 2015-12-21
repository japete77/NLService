Imports System.Runtime.Caching

Public Class AuthService
    Private sessions As ObjectCache = MemoryCache.[Default]
    Private ExpiryHours As Integer = 24

    Public Function GetNewSession(Username As String) As String
        Dim Session As String = System.Guid.NewGuid().ToString()

        ' setup session expiry date
        sessions.Add(Session, Username, DateTimeOffset.Now.AddHours(ExpiryHours))

        Return Session
    End Function

    Public Function ValidateSession(Session As String) As Boolean
        Dim User = sessions.[Get](Session)
        If User Is Nothing Then
            Return False
        Else
            ' Update expiry date
            sessions.[Set](Session, User, DateTimeOffset.Now.AddHours(ExpiryHours))

            Return True
        End If
    End Function
End Class
