''' <summary>
''' Implementa a Classe de Citações ou Thesaurus
''' </summary>
''' <remarks></remarks>
Public Class mtdCitation
    
    Private _title_PT As String
    Private _title_EN As String
    Private _alternateTitle As String
    Private _edition As String
    Private _editionDate As String
    Private _identifier As String
    Private _series_name As String
    Private _citedResponsibleParty As mtdContact
    Private _date As mtdDate

    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Nome (título) pelo qual o recurso citado é conhecido em Português
    ''' </summary>
    ''' <value>novo Nome</value>
    ''' <returns>Nome</returns>
    ''' <remarks></remarks>
    Public Property Title_PT As String
        Get
            SyncLock _locker
                Return _title_PT
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _title_PT = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Nome (título) pelo qual o recurso citado é conhecido em Inglês
    ''' </summary>
    ''' <value>novo Nome</value>
    ''' <returns>Nome</returns>
    ''' <remarks></remarks>
    Public Property Title_EN As String
        Get
            SyncLock _locker
                Return _title_EN
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _title_EN = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Nome abreviado ou nome alternativo do recurso
    ''' </summary>
    ''' <value>novo Nome abreviado</value>
    ''' <returns>Nome abreviado</returns>
    ''' <remarks></remarks>
    Public Property AlternateTitle As String
        Get
            SyncLock _locker
                Return _alternateTitle
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _alternateTitle = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Versão do recurso citado
    ''' </summary>
    ''' <value>nova Versão</value>
    ''' <returns>Versão do recurso citado</returns>
    ''' <remarks></remarks>
    Public Property Edition As String
        Get
            SyncLock _locker
                Return _edition
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _edition = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Data da edição
    ''' </summary>
    ''' <value>nova Data da edição</value>
    ''' <returns>Data da edição</returns>
    ''' <remarks></remarks>
    Public Property EditionDate As String
        Get
            SyncLock _locker
                Return _editionDate
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _editionDate = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Código de identificação do recurso
    ''' </summary>
    ''' <value>novo Código de identificação do recurso</value>
    ''' <returns>Código de identificação do recurso</returns>
    ''' <remarks></remarks>
    Public Property Identifier As String
        Get
            SyncLock _locker
                Return _identifier
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _identifier = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Nome da série a que o recurso faz parte
    ''' </summary>
    ''' <value>novo Nome</value>
    ''' <returns>Nome da série</returns>
    ''' <remarks></remarks>
    Public Property Series_name As String
        Get
            SyncLock _locker
                Return _series_name
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _series_name = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Responsável Citado
    ''' </summary>
    ''' <value>novo Responsável</value>
    ''' <returns>Responsável Citado</returns>
    ''' <remarks></remarks>
    Public Property CitedResponsibleParty As mtdContact
        Get
            SyncLock _locker
                Return _citedResponsibleParty
            End SyncLock
        End Get
        Set(ByVal value As mtdContact)
            SyncLock _locker
                _citedResponsibleParty = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Data de Citação
    ''' </summary>
    ''' <value>nova Data</value>
    ''' <returns>Data de Citação</returns>
    ''' <remarks></remarks>
    Public Property [Date] As mtdDate
        Get
            SyncLock _locker
                Return _date
            End SyncLock
        End Get
        Set(ByVal value As mtdDate)
            SyncLock _locker
                _date = value
            End SyncLock
        End Set
    End Property

End Class
