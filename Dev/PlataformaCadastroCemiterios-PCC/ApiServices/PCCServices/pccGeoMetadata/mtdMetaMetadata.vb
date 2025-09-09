''' <summary>
''' Implenta a Classe Meta-Metadados
''' </summary>
''' <remarks></remarks>
Public Class mtdMetaMetadata

    Private _fileIdentifier As String
    Private _language As String
    Private _hierarchyLevel As String
    Private _date As String

    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Identificador dos dados
    ''' </summary>
    ''' <value>novo Identificador</value>
    ''' <returns>Identificador</returns>
    ''' <remarks></remarks>
    Public Property FileIdentifier As String
        Get
            SyncLock _locker
                Return _fileIdentifier
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _fileIdentifier = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Idioma dos dados
    ''' </summary>
    ''' <value>novo Idioma</value>
    ''' <returns>Idioma</returns>
    ''' <remarks></remarks>
    Public Property Language As String
        Get
            SyncLock _locker
                Return _language
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _language = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Nível Hierarquico
    ''' </summary>
    ''' <value>novo Nível Hierarquico</value>
    ''' <returns>Nível Hierarquico</returns>
    ''' <remarks></remarks>
    Public Property HierarchyLevel As String
        Get
            SyncLock _locker
                Return _hierarchyLevel
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _hierarchyLevel = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Data
    ''' </summary>
    ''' <value>nova Data</value>
    ''' <returns>Data</returns>
    ''' <remarks></remarks>
    Public Property [Date] As String
        Get
            SyncLock _locker
                Return _date
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _date = value
            End SyncLock
        End Set
    End Property

End Class
