Imports System.Configuration
Imports System.Data.SqlClient


Namespace NuevaLuz.AudioBooks

    Class AudioBookDataService

        Private queryTitles As String = "SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY titulo) AS idx, LH_audioteca.id, LH_audioteca.titulo, LH_audioteca.numero " & _
            "FROM LH_audioteca, LH_formatosdisponibles " & _
            "WHERE LH_audioteca.id = LH_formatosdisponibles.id_audioteca AND LH_formatosdisponibles.id_formato = 4 " & _
            "AND LH_formatosdisponibles.activo = 'True' AND LH_audioteca.activo = 'True' " & _
            ") AS tbl WHERE idx BETWEEN @start AND @end "


        Private queryBooks As String = "SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY titulo) AS idx, " &
            "LHA.id, LHA.titulo, LHA.comentario, LHA.id_autor, LHA.horas, LHA.minutos, SIA.nombre 'autor', SIE.nombre 'editorial' " &
            "FROM LH_audioteca LHA " &
            "INNER JOIN SI_autores SIA ON SIA.id = LHA.id_autor " &
            "INNER JOIN SI_editoriales SIE ON SIE.id = LHA.id_editorial " &
            "INNER JOIN LH_formatosdisponibles LHF on LHF.id_audioteca = LHA.id " &
            "WHERE LHF.id_formato=4 AND LHF.activo='True' AND LHA.activo='True') AS tbl " &
            "WHERE idx BETWEEN @start AND @end "

        Private queryTitlesByAuthor As String = "SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY titulo) AS idx, LHA.id, LHA.titulo " &
            "FROM LH_audioteca LHA " &
            "INNER JOIN LH_formatosdisponibles LHF on LHF.id_audioteca = LHA.id " &
            "WHERE LHF.id_formato=4 AND LHF.activo='True' AND LHA.activo='True' AND and LHA.id_autor=@id) AS tbl " &
            "WHERE idx BETWEEN @start AND @end "

        Private queryBooksById As String = "SELECT LHA.id, LHA.titulo, LHA.comentario, LHA.id_autor, LHA.horas, LHA.minutos, " &
            "SIA.nombre 'autor', SIE.nombre 'editorial' " &
            "FROM LH_audioteca " &
            "LHA INNER JOIN SI_autores SIA ON SIA.id = LHA.id_autor " &
            "INNER JOIN SI_editoriales SIE ON SIE.id = LHA.id_editorial " &
            "WHERE LHA.activo=1 AND LHA.id=@id "

        Private queryAuthors As String = "SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY nombre) AS idx, id, nombre " &
            "FROM SI_autores SIA" &
            "INNER JOIN LH_audioteca LHA ON LHA.id_autor=SIA.id " &
            "INNER JOIN LH_formatosdisponibles LHF on LHF.id_audioteca = LHA.id " &
            "WHERE LHF.id_formato=4 AND LHF.activo='True' AND LHA.activo='True') AS tbl " &
            "WHERE idx BETWEEN @start AND @end "

        Private queryBooksCount As String = "SELECT count(*) FROM LH_audioteca, LH_formatosdisponibles " &
            "WHERE LH_audioteca.id = LH_formatosdisponibles.id_audioteca AND LH_formatosdisponibles.id_formato = 4 AND LH_formatosdisponibles.activo = 'True' AND LH_audioteca.activo = 'True'"

        Private queryBooksCountByAuthor As String = "SELECT count(*) FROM LH_audioteca, LH_formatosdisponibles " &
            "WHERE LH_audioteca.id = LH_formatosdisponibles.id_audioteca AND LH_formatosdisponibles.id_formato = 4 AND LH_formatosdisponibles.activo = 'True' AND LH_audioteca.activo = 'True' AND id_autor=@Id "

        Private queryAuthorsCount As String = "SELECT count(*) " &
            "FROM SI_autores SIA" &
            "INNER JOIN LH_audioteca LHA On LHA.id_autor=SIA.id " &
            "INNER JOIN LH_formatosdisponibles LHF On LHF.id_audioteca = LHA.id " &
            "WHERE LHF.id_formato=4 And LHF.activo='True' AND LHA.activo='True' "

        Private queryAuthenticate As String = "SELECT contrasena FROM US_usuarios WHERE id=@id "

        Private sqlConnection As SqlConnection = Nothing

        Public Sub New()
            sqlConnection = New SqlConnection(ConfigurationManager.ConnectionStrings("AudioBooksDataConnectionString").ConnectionString)
        End Sub

        Public Function GetTitles(Index As Integer, Count__1 As Integer) As TitlesResult
            Dim Result As New TitlesResult()
            Result.Titles = New List(Of Title)()

            ' Total authors
            Dim count__2 As DataTable = ExecuteSelectCommand(queryBooksCount, CommandType.Text)
            If count__2.Rows.Count > 0 Then
                Result.Total = Int32.Parse(count__2.Rows(0)(0).ToString())
            End If

            Dim titles As DataTable = ExecuteParamerizedSelectCommand(queryTitles, CommandType.Text, New SqlParameter() {New SqlParameter("start", Index), New SqlParameter("end", Index + Count__1 - 1)})

            For Each row As DataRow In titles.Rows
                Result.Titles.Add(Title.FromDataRow(row))
            Next

            Return Result
        End Function

        Public Function GetAudioBooks(Index As Integer, Count__1 As Integer) As AudioBooksResult
            Dim Result As New AudioBooksResult()
            Result.AudioBooks = New List(Of AudioBook)()

            ' Total books
            Dim count__2 As DataTable = ExecuteSelectCommand(queryBooksCount, CommandType.Text)
            If count__2.Rows.Count > 0 Then
                Result.Total = Int32.Parse(count__2.Rows(0)(0).ToString())
            End If

            Dim audioteca As DataTable = ExecuteParamerizedSelectCommand(queryBooks, CommandType.Text, New SqlParameter() {New SqlParameter("start", Index), New SqlParameter("end", Index + Count__1 - 1)})

            For Each row As DataRow In audioteca.Rows
                Result.AudioBooks.Add(AudioBook.FromDataRow(row))
            Next

            Return Result
        End Function

        Public Function GetTitlesByAuthor(Id As Integer, Index As Integer, Count__1 As Integer) As TitlesResult
            Dim Result As New TitlesResult()
            Result.Titles = New List(Of Title)()

            ' Total books
            Dim count__2 As DataTable = ExecuteParamerizedSelectCommand(queryBooksCountByAuthor, CommandType.Text, New SqlParameter() {New SqlParameter("id", Id)})
            If count__2.Rows.Count > 0 Then
                Result.Total = Int32.Parse(count__2.Rows(0)(0).ToString())
            End If

            Dim titles As DataTable = ExecuteParamerizedSelectCommand(queryTitlesByAuthor, CommandType.Text, New SqlParameter() {New SqlParameter("id", Id), New SqlParameter("start", Index), New SqlParameter("end", Index + Count__1 - 1)})

            For Each row As DataRow In titles.Rows
                Result.Titles.Add(Title.FromDataRow(row))
            Next

            Return Result
        End Function

        Public Function GetAudioBookDetails(Id As Integer) As AudioBook
            Dim book As DataTable = ExecuteParamerizedSelectCommand(queryBooksById, CommandType.Text, New SqlParameter() {New SqlParameter("Id", Id)})

            If book.Rows.Count > 0 Then
                Return AudioBook.FromDataRow(book.Rows(0))
            Else
                Return Nothing
            End If
        End Function

        Public Function GetAuthors(Index As Integer, Count__1 As Integer) As AuthorsResult
            Dim Result As New AuthorsResult()
            Result.Authors = New List(Of Author)()

            ' Total authors
            Dim count__2 As DataTable = ExecuteSelectCommand(queryAuthorsCount, CommandType.Text)
            If count__2.Rows.Count > 0 Then
                Result.Total = Int32.Parse(count__2.Rows(0)(0).ToString())
            End If

            ' Get Authors
            Dim authors As DataTable = ExecuteParamerizedSelectCommand(queryAuthors, CommandType.Text, New SqlParameter() {New SqlParameter("start", Index), New SqlParameter("end", Index + Count__1 - 1)})

            For Each row As DataRow In authors.Rows
                Result.Authors.Add(Author.FromDataRow(row))
            Next

            Return Result
        End Function

        Public Function GetPassword(Username As String) As String
            Dim user As DataTable = ExecuteParamerizedSelectCommand(queryAuthenticate, CommandType.Text, New SqlParameter() {New SqlParameter("id", Username)})

            If user.Rows.Count > 0 Then
                Return user.Rows(0)("contrasena").ToString().Trim()
            Else
                Throw New Exception("Usuario no válido")
            End If
        End Function

        Private Function ExecuteSelectCommand(CommandName As String, cmdType As CommandType) As DataTable
            Dim cmd As SqlCommand = Nothing
            Dim table As New DataTable()

            cmd = sqlConnection.CreateCommand()

            cmd.CommandType = cmdType
            cmd.CommandText = CommandName

            Try
                sqlConnection.Open()

                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(table)

            Catch ex As Exception
                Throw ex
            Finally
                cmd.Dispose()
                cmd = Nothing
                sqlConnection.Close()
            End Try

            Return table
        End Function

        Private Function ExecuteParamerizedSelectCommand(CommandName As String, cmdType As CommandType, param As SqlParameter()) As DataTable
            Dim cmd As SqlCommand = Nothing
            Dim table As New DataTable()

            cmd = sqlConnection.CreateCommand()

            cmd.CommandType = cmdType
            cmd.CommandText = CommandName
            cmd.Parameters.AddRange(param)

            Try
                sqlConnection.Open()

                Dim da As SqlDataAdapter = New SqlDataAdapter(cmd)
                da.Fill(table)

            Catch ex As Exception
                Throw ex
            Finally
                cmd.Dispose()
                cmd = Nothing
                sqlConnection.Close()
            End Try

            Return table
        End Function


        ''TRABAJO MIO
        'Public Function Autores(Index As Integer, Count__1 As Integer) As AutorResultado
        '    Dim Result As New AuthorsResult()
        '    Result.Authors = New List(Of Author)()

        '    ' Total authors
        '    Dim count__2 As DataTable = ExecuteSelectCommand(queryAuthorsCount, CommandType.Text)
        '    If count__2.Rows.Count > 0 Then
        '        Result.Total = Int32.Parse(count__2.Rows(0)(0).ToString())
        '    End If

        '    ' Get Authors
        '    Dim authors As DataTable = ExecuteParamerizedSelectCommand(queryAuthors, CommandType.Text, New SqlParameter() {New SqlParameter("start", Index), New SqlParameter("end", Index + Count__1 - 1)})

        '    For Each row As DataRow In authors.Rows
        '        Result.Authors.Add(Author.FromDataRow(row))
        '    Next

        '    Return Result
        'End Function

    End Class
End Namespace