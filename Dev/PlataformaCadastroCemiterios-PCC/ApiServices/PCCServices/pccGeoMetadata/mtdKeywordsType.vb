''' <summary>
''' Implementa a Classe Tipo de Palavras Chave
''' </summary>
''' <remarks></remarks>
Public Class mtdKeywordsType
    Private _keywordTypeCode As String
    Private _Keywords As List(Of String) 'mtdKeywords
    Private _thesaurus As mtdCitation

    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Tipo do Grupo de Palavras Chave - Assunto utilizado para agrupar as palavras chave
    ''' </summary>
    ''' <value>novo Tipo do Grupo de Palavras Chave</value>
    ''' <returns>Tipo do Grupo de Palavras Chave</returns>
    ''' <remarks></remarks>
    Public Property KeywordTypeCode As String
        Get
            SyncLock _locker
                Return _keywordTypeCode
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _keywordTypeCode = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Palavra chave
    ''' </summary>
    ''' <value>nova Palavra Chave</value>
    ''' <returns>Palavra Chave</returns>
    ''' <remarks></remarks>
    Public Property Keywords As List(Of String)
        Get
            SyncLock _locker
                Return _Keywords
            End SyncLock
        End Get
        Set(ByVal value As List(Of String))
            SyncLock _locker
                _Keywords = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Citação ou Thesaurus
    ''' </summary>
    ''' <value>nova Citação</value>
    ''' <returns>Citação</returns>
    ''' <remarks></remarks>
    Public Property Thesaurus As mtdCitation
        Get
            SyncLock _locker
                Return _thesaurus
            End SyncLock
        End Get
        Set(ByVal value As mtdCitation)
            SyncLock _locker
                _thesaurus = value
            End SyncLock
        End Set
    End Property

End Class
