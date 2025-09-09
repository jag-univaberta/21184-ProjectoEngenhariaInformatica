''' <summary>
''' Classe de Operação dos Serviços
''' </summary>
''' <remarks></remarks>
Public Class mtdServiceOperations
    Private _operationName As String ' Nome da Operação
    Private _DCP As List(Of String) ' Plataforma Computacional
    Private _connectPoint As mtdOnlineResource ' Recurso Online

    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Nome da Operação
    ''' </summary>
    ''' <value>novo Nome da Operação</value>
    ''' <returns>Nome da Operação</returns>
    ''' <remarks></remarks>
    Public Property OperationName As String
        Get
            SyncLock _locker
                Return _operationName
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _operationName = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' lista de Plataforma Computacional
    ''' </summary>
    ''' <value>nova lista Plataforma Computacional</value>
    ''' <returns>Plataforma Computacional</returns>
    ''' <remarks>Web Services, XML, Corba, Java, COM, SQL</remarks>
    Public Property DCP As List(Of String)
        Get
            SyncLock _locker
                Return _DCP
            End SyncLock
        End Get
        Set(ByVal value As List(Of String))
            SyncLock _locker
                _DCP = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Hiperligação
    ''' </summary>
    ''' <value>nova Hiperligação</value>
    ''' <returns>Hiperligação</returns>
    ''' <remarks></remarks>
    Public Property ConnectPoint As mtdOnlineResource
        Get
            SyncLock _locker
                Return _connectPoint
            End SyncLock
        End Get
        Set(ByVal value As mtdOnlineResource)
            SyncLock _locker
                _connectPoint = value
            End SyncLock
        End Set
    End Property
End Class
