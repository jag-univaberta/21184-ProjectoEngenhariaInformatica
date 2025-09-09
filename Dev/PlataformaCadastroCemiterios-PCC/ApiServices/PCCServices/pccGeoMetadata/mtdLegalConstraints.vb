''' <summary>
''' Implenta a Classe Restrições Legais
''' </summary>
''' <remarks></remarks>
Public Class mtdLegalConstraints
    'TODO: Ver esta Classe de Restrições Legais devido às listas do tipo string
    Private _useLimitation As String
    Private _accessConstraints As List(Of String)
    Private _useConstraints As List(Of String)

    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Limitações para o acesso e uso do recurso
    ''' </summary>
    ''' <value>nova Limitação de uso</value>
    ''' <returns>Limitação de Uso</returns>
    ''' <remarks></remarks>
    Public Property UseLimitation As String
        Get
            SyncLock _locker
                Return _useLimitation
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _useLimitation = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Lista de Restrições de Acesso
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AccessConstraints As List(Of String)
        Get
            SyncLock _locker
                Return _accessConstraints
            End SyncLock
        End Get
        Set(ByVal value As List(Of String))
            SyncLock _locker
                _accessConstraints = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Lista de Restrições de Uso
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UseConstraints As List(Of String)
        Get
            SyncLock _locker
                Return _useConstraints
            End SyncLock
        End Get
        Set(ByVal value As List(Of String))
            SyncLock _locker
                _useConstraints = value
            End SyncLock
        End Set
    End Property

End Class
