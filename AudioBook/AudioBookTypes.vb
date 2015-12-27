
Namespace NuevaLuz.AudioBooks
    <DataContract>
    Public Class LoginResult
        <DataMember>
        Public Success As Boolean

        <DataMember>
        Public Message As String

        <DataMember>
        Public Session As String
    End Class

    <DataContract>
    Public Class TitlesResult
        <DataMember>
        Public Property Total() As Integer
            Get
                Return m_Total
            End Get
            Set
                m_Total = Value
            End Set
        End Property
        Private m_Total As Integer

        <DataMember>
        Public Property Titles() As List(Of Title)
            Get
                Return m_Titles
            End Get
            Set
                m_Titles = Value
            End Set
        End Property
        Private m_Titles As List(Of Title)
    End Class

    <DataContract>
    Public Class AudioBooksResult
        <DataMember>
        Public Property Total() As Integer
            Get
                Return m_Total
            End Get
            Set
                m_Total = Value
            End Set
        End Property
        Private m_Total As Integer

        <DataMember>
        Public Property AudioBooks() As List(Of AudioBook)
            Get
                Return m_AudioBooks
            End Get
            Set
                m_AudioBooks = Value
            End Set
        End Property
        Private m_AudioBooks As List(Of AudioBook)
    End Class

    <DataContract>
    Public Class AuthorsResult
        <DataMember>
        Public Property Total() As Integer
            Get
                Return m_Total
            End Get
            Set
                m_Total = Value
            End Set
        End Property
        Private m_Total As Integer

        <DataMember>
        Public Property Authors() As List(Of Author)
            Get
                Return m_Authors
            End Get
            Set
                m_Authors = Value
            End Set
        End Property
        Private m_Authors As List(Of Author)
    End Class

    <DataContract>
    Public Class Title
        <DataMember>
        Public Property Id() As String
            Get
                Return m_Id
            End Get
            Set
                m_Id = Value
            End Set
        End Property
        Private m_Id As String

        <DataMember>
        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set
                m_Name = Value
            End Set
        End Property
        Private m_Name As String

        Public Shared Function FromDataRow(Row As DataRow) As Title
            Dim Item As New Title()

            Item.Id = Row("id").ToString().Trim()
            Item.Name = If(Row("titulo") IsNot Nothing, Row("titulo").ToString().Trim(), Nothing)

            Return Item
        End Function
    End Class

    <DataContract>
    Public Class AudioBook
        <DataMember>
        Public Property Id() As String
            Get
                Return m_Id
            End Get
            Set
                m_Id = Value
            End Set
        End Property
        Private m_Id As String

        <DataMember>
        Public Property Title() As String
            Get
                Return m_Title
            End Get
            Set
                m_Title = Value
            End Set
        End Property
        Private m_Title As String

        <DataMember>
        Public Property Number() As String
            Get
                Return m_Number
            End Get
            Set
                m_Number = Value
            End Set
        End Property
        Private m_Number As String

        <DataMember>
        Public Property Author() As Author
            Get
                Return m_Author
            End Get
            Set
                m_Author = Value
            End Set
        End Property
        Private m_Author As Author

        <DataMember>
        Public Property Editorial() As String
            Get
                Return m_Editorial
            End Get
            Set
                m_Editorial = Value
            End Set
        End Property
        Private m_Editorial As String

        <DataMember>
        Public Property Comments() As String
            Get
                Return m_Comments
            End Get
            Set
                m_Comments = Value
            End Set
        End Property
        Private m_Comments As String

        <DataMember>
        Public Property LengthHours() As Integer
            Get
                Return m_LengthHours
            End Get
            Set
                m_LengthHours = Value
            End Set
        End Property
        Private m_LengthHours As Integer

        <DataMember>
        Public Property LengthMins() As Integer
            Get
                Return m_LengthMins
            End Get
            Set
                m_LengthMins = Value
            End Set
        End Property
        Private m_LengthMins As Integer

        Public Shared Function FromDataRow(Row As DataRow) As AudioBook
            Dim Item As New AudioBook()

            Item.Id = Row("id").ToString().Trim()
            Item.Title = If(Row("titulo") IsNot Nothing, Row("titulo").ToString().Trim(), Nothing)
            Item.Number = Row("numero").ToString().Trim().PadLeft(4, "0")
            Item.Comments = If(Row("comentario") IsNot Nothing, Row("comentario").ToString().Trim(), Nothing)
            Item.Author = New Author()
            Item.Author.Id = Row("id_autor").ToString().Trim()
            Item.Author.Name = Row("autor").ToString().Trim()


            Item.Editorial = Row("editorial").ToString().Trim()
            Dim hours As Integer
            Integer.TryParse(Row("horas").ToString().Trim(), hours)
            Item.LengthHours = hours
            Dim mins As Integer
            Integer.TryParse(Row("minutos").ToString().Trim(), mins)
            Item.LengthMins = mins

            Return Item
        End Function
    End Class

    <DataContract>
    Public Class Author
        <DataMember>
        Public Property Id() As String
            Get
                Return m_Id
            End Get
            Set
                m_Id = Value
            End Set
        End Property
        Private m_Id As String

        <DataMember>
        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set
                m_Name = Value
            End Set
        End Property
        Private m_Name As String

        Public Shared Function FromDataRow(Row As DataRow) As Author
            Dim Result As New Author()

            Result.Id = Row("id").ToString().Trim()
            Result.Name = Row("nombre").ToString().Trim()

            Return Result
        End Function
    End Class

    'Trabajo mio
    <DataContract()>
    Public Class AutorResultado
        <DataMember()>
        Public Property Total() As Integer
            Get
                Return m_Total
            End Get
            Set(value As Integer)
                m_Total = value
            End Set
        End Property
        Private m_Total As Integer

        <DataMember()>
        Public Property Authors() As List(Of Author)
            Get
                Return m_Authors
            End Get
            Set(value As List(Of Author))
                m_Authors = value
            End Set
        End Property
        Private m_Authors As List(Of Author)
    End Class






End Namespace