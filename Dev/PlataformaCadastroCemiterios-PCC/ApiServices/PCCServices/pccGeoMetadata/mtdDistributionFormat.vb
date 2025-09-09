''' <summary>
''' Implementa a Classe Formato dos ficheiros 
''' </summary>
''' <remarks></remarks>
Public Class mtdDistributionFormat
    Private _name As String
    Private _version As String


    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Nome do Formato
    ''' </summary>
    ''' <value>novo Nome do Formato</value>
    ''' <returns>Nome do Formato</returns>
    ''' <remarks></remarks>
    Public Property Name As String
        Get
            SyncLock _locker
                Return _name
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _name = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Versão do Formato
    ''' </summary>
    ''' <value>nova Versão do Formato</value>
    ''' <returns>Versão do Formato</returns>
    ''' <remarks></remarks>
    Public Property Version As String
        Get
            SyncLock _locker
                Return _version
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _version = value
            End SyncLock
        End Set
    End Property

End Class
