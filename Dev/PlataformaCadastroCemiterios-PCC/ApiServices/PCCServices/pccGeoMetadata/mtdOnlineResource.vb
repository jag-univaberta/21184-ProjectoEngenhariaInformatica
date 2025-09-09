
''' <summary>
''' Classe Hiperligação
''' </summary>
''' <remarks></remarks>
Public Class mtdOnlineResource

    Private _URL As String
    Private _OnlineFunctionCode As String

    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Hiperligação (URL)
    ''' </summary>
    ''' <value>nova Hiperligação (URL)</value>
    ''' <returns>Hiperligação (URL)</returns>
    ''' <remarks></remarks>
    Public Property URL As String
        Get
            SyncLock _locker
                Return _URL
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _URL = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Função Online (Importação/Download, Pesquisa, Encomenda,...)
    ''' </summary>
    ''' <value>nova Função</value>
    ''' <returns>Função</returns>
    ''' <remarks></remarks>
    Public Property OnlineFunctionCode As String
        Get
            SyncLock _locker
                Return _OnlineFunctionCode
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _OnlineFunctionCode = value
            End SyncLock
        End Set
    End Property

End Class
