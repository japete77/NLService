﻿Imports System.ServiceModel.Web
Imports AudioBook.NuevaLuz.AudioBooks

<ServiceContract>
Public Interface IAudioBooks
    <OperationContract>
    <WebInvoke(Method:="GET", ResponseFormat:=WebMessageFormat.Json, BodyStyle:=WebMessageBodyStyle.Wrapped, UriTemplate:="Login?Username={Username}&Password={Password}")>
    Function Login(Username As String, Password As String) As LoginResult

    <OperationContract>
    <WebInvoke(Method:="GET", ResponseFormat:=WebMessageFormat.Json, BodyStyle:=WebMessageBodyStyle.Wrapped, UriTemplate:="GetTitles?Session={Session}&Index={Index}&Count={Count}")>
    Function GetTitles(Session As String, Index As Integer, Count As Integer) As TitlesResult

    <OperationContract>
    <WebInvoke(Method:="GET", ResponseFormat:=WebMessageFormat.Json, BodyStyle:=WebMessageBodyStyle.Wrapped, UriTemplate:="GetAudioBooks?Session={Session}&Index={Index}&Count={Count}")>
    Function GetBooks(Session As String, Index As Integer, Count As Integer) As AudioBooksResult

    <OperationContract>
    <WebInvoke(Method:="GET", ResponseFormat:=WebMessageFormat.Json, BodyStyle:=WebMessageBodyStyle.Wrapped, UriTemplate:="GetAuthors?Session={Session}&Index={Index}&Count={Count}")>
    Function GetAuthors(Session As String, Index As Integer, Count As Integer) As AuthorsResult

    <OperationContract>
    <WebInvoke(Method:="GET", ResponseFormat:=WebMessageFormat.Json, BodyStyle:=WebMessageBodyStyle.Wrapped, UriTemplate:="GetTitlesByAuthor?Session={Session}&Id={Id}&Index={Index}&Count={Count}")>
    Function GetTitlesByAuthor(Session As String, Id As Integer, Index As Integer, Count As Integer) As TitlesResult

    <OperationContract>
    <WebInvoke(Method:="GET", ResponseFormat:=WebMessageFormat.Json, BodyStyle:=WebMessageBodyStyle.Wrapped, UriTemplate:="GetAudioBookDetail?Session={Session}&Id={Id}")>
    Function GetAudioBookDetail(Session As String, Id As Integer) As AudioBook.NuevaLuz.AudioBooks.AudioBook

    '' Trabajo mio
    '<OperationContract()>
    '<WebInvoke(Method:="GET", ResponseFormat:=WebMessageFormat.Json, BodyStyle:=WebMessageBodyStyle.Wrapped, UriTemplate:="GetAuthors?Session={Session}&Index={Index}&Count={Count}")>
    'Function Autores(Session As String, Index As Integer, Count As Integer) As AuthorsResult


End Interface

