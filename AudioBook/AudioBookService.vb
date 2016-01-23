Imports System.Net
Imports System.Web

Namespace NuevaLuz.AudioBooks
    Public Class AudioBookService
        Implements IAudioBooks
        Private audioBooksDataService As New AudioBookDataService()
        Private authService As New AuthService()
        Private denyAccess As String = "Acceso denegado"

        Private Function IAudioBooks_Login(Username As String, Password As String) As LoginResult Implements IAudioBooks.Login
            Dim Result As New LoginResult()

            Try
                If audioBooksDataService.GetPassword(Username).Equals(Password) Then
                    Result.Success = True
                    Result.Session = authService.GetNewSession(Username)
                Else
                    Throw New System.ServiceModel.Web.WebFaultException(HttpStatusCode.MethodNotAllowed)
                End If
            Catch ex As Exception
                Result.Success = False
                Result.Message = ex.Message
            End Try

            Return Result
        End Function

        Private Function IAudioBooks_GetTitles(Session As String, Index As Integer, Count As Integer) As TitlesResult Implements IAudioBooks.GetTitles
            If Not authService.ValidateSession(Session) Then
                Throw New System.ServiceModel.Web.WebFaultException(HttpStatusCode.MethodNotAllowed)
            End If

            Return audioBooksDataService.GetTitles(Index, Count)
        End Function

        Private Function IAudioBooks_GetBooks(Session As String, Index As Integer, Count As Integer) As AudioBooksResult Implements IAudioBooks.GetBooks
            If Not authService.ValidateSession(Session) Then
                Throw New System.ServiceModel.Web.WebFaultException(HttpStatusCode.MethodNotAllowed)
            End If

            Return audioBooksDataService.GetAudioBooks(Index, Count)
        End Function

        Private Function IAudioBooks_GetAuthors(Session As String, Index As Integer, Count As Integer) As AuthorsResult Implements IAudioBooks.GetAuthors
            If Not authService.ValidateSession(Session) Then
                Throw New System.ServiceModel.Web.WebFaultException(HttpStatusCode.MethodNotAllowed)
            End If

            Return audioBooksDataService.GetAuthors(Index, Count)
        End Function

        Private Function IAudioBooks_GetTitlesByAuthor(Session As String, Id As Integer, Index As Integer, Count As Integer) As TitlesResult Implements IAudioBooks.GetTitlesByAuthor
            If Not authService.ValidateSession(Session) Then
                Throw New System.ServiceModel.Web.WebFaultException(HttpStatusCode.MethodNotAllowed)
            End If

            Return audioBooksDataService.GetTitlesByAuthor(Id, Index, Count)
        End Function

        Private Function IAudioBooks_GetAudioBookDetail(Session As String, Id As Integer) As AudioBook Implements IAudioBooks.GetAudioBookDetail
            If Not authService.ValidateSession(Session) Then
                Throw New System.ServiceModel.Web.WebFaultException(HttpStatusCode.MethodNotAllowed)
            End If

            Return audioBooksDataService.GetAudioBookDetails(Id)
        End Function

        Public Function SearchTitles(Session As String, Text As String, Index As Integer, Count As Integer) As TitlesResult Implements IAudioBooks.SearchTitles
            If Not authService.ValidateSession(Session) Then
                Throw New System.ServiceModel.Web.WebFaultException(HttpStatusCode.MethodNotAllowed)
            End If

            Return audioBooksDataService.SearchTitles(Text, Index, Count)

        End Function

        Public Function SearchAuthors(Session As String, Text As String, Index As Integer, Count As Integer) As AuthorsResult Implements IAudioBooks.SearchAuthors
            If Not authService.ValidateSession(Session) Then
                Throw New System.ServiceModel.Web.WebFaultException(HttpStatusCode.MethodNotAllowed)
            End If

            Return audioBooksDataService.SearchAuthors(Text, Index, Count)
        End Function

        Public Function RegisterDownload(Session As String, IdAudio As String) As Object Implements IAudioBooks.RegisterDownload
            If Not authService.ValidateSession(Session) Then
                Throw New System.ServiceModel.Web.WebFaultException(HttpStatusCode.MethodNotAllowed)
            End If

            Return audioBooksDataService.RegisterDownload(CInt(authService.GetUsername(Session)), IdAudio)
        End Function
    End Class
End Namespace