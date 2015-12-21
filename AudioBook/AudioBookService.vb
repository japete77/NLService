Imports System.Net
Imports System.Web

Namespace NuevaLuz.AudioBooks
    Public Class AudioBookService
        Implements IAudioBooks
        Private audioBooksDataService As New AudioBookDataService()
        Private authService As New AuthService()

        Private Function IAudioBooks_Login(Username As String, Password As String) As LoginResult Implements IAudioBooks.Login
            Dim Result As New LoginResult()

            Try
                If audioBooksDataService.GetPassword(Username).Equals(Password) Then
                    Result.Success = True
                    Result.Session = authService.GetNewSession(Username)
                Else
                    Throw New HttpException(CInt(HttpStatusCode.Unauthorized), "Acceso denegado")
                End If
            Catch ex As Exception
                Result.Success = False
                Result.Message = ex.Message
            End Try

            Return Result
        End Function

        Private Function IAudioBooks_GetTitles(Session As String, Index As Integer, Count As Integer) As TitlesResult Implements IAudioBooks.GetTitles
            If Not authService.ValidateSession(Session) Then
                Throw New HttpException(CInt(HttpStatusCode.Unauthorized), "Acceso denegado")
            End If

            Return audioBooksDataService.GetTitles(Index, Count)
        End Function

        Private Function IAudioBooks_GetBooks(Session As String, Index As Integer, Count As Integer) As AudioBooksResult Implements IAudioBooks.GetBooks
            If Not authService.ValidateSession(Session) Then
                Throw New HttpException(CInt(HttpStatusCode.Unauthorized), "Acceso denegado")
            End If

            Return audioBooksDataService.GetAudioBooks(Index, Count)
        End Function

        Private Function IAudioBooks_GetAuthors(Session As String, Index As Integer, Count As Integer) As AuthorsResult Implements IAudioBooks.GetAuthors
            If Not authService.ValidateSession(Session) Then
                Throw New HttpException(CInt(HttpStatusCode.Unauthorized), "Acceso denegado")
            End If

            Return audioBooksDataService.GetAuthors(Index, Count)
        End Function

        Private Function IAudioBooks_GetTitlesByAuthor(Session As String, Id As Integer, Index As Integer, Count As Integer) As TitlesResult Implements IAudioBooks.GetTitlesByAuthor
            If Not authService.ValidateSession(Session) Then
                Throw New HttpException(CInt(HttpStatusCode.Unauthorized), "Acceso denegado")
            End If

            Return audioBooksDataService.GetTitlesByAuthor(Id, Index, Count)
        End Function

        Private Function IAudioBooks_GetAudioBookDetail(Session As String, Id As Integer) As AudioBook Implements IAudioBooks.GetAudioBookDetail
            If Not authService.ValidateSession(Session) Then
                Throw New HttpException(CInt(HttpStatusCode.Unauthorized), "Acceso denegado")
            End If

            Return audioBooksDataService.GetAudioBookDetails(Id)
        End Function




        ''Trabajo mio
        'Private Function IAudioBooks_Autores(Session As String, Index As Integer, Count As Integer) As AutorResultado Implements IAudioBooks.Autores
        '    If Not authService.ValidateSession(Session) Then
        '        Throw New HttpException(CInt(HttpStatusCode.Unauthorized), "Acceso denegado")
        '    End If

        '    Return audioBooksDataService.Autores(Index, Count)
        'End Function





    End Class
End Namespace