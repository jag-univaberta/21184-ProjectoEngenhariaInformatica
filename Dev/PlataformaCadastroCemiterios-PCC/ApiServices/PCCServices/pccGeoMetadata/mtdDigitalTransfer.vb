''' <summary>
''' Implementa a Classe Transferência Digital
''' </summary>
''' <remarks></remarks>
Public Class mtdDigitalTransfer
    Private _unitsOfDistribution As String
    Private _Size As String
    Private _Linkage As mtdOnlineResource


    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Unidade de Distribuição em que o recurso está disponível, por exemplo: folhas, temas ou áreas geográficas específicas
    ''' </summary>
    ''' <value>nova Unidade de Distribuição</value>
    ''' <returns>Unidade de Distribuição</returns>
    ''' <remarks></remarks>
    Public Property UnitsOfDistribution As String
        Get
            SyncLock _locker
                Return _unitsOfDistribution
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _unitsOfDistribution = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Tamanho estimado de uma unidade de recurso, no formato de transferência em Mb
    ''' </summary>
    ''' <value>novo Tamanho (Mb)</value>
    ''' <returns>Tamanho (Mb)</returns>
    ''' <remarks></remarks>
    Public Property Size As String
        Get
            SyncLock _locker
                Return _Size
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _Size = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Endereço electrónico (URL) para acesso
    ''' </summary>
    ''' <value>novo URL</value>
    ''' <returns>URL</returns>
    ''' <remarks></remarks>
    Public Property Linkage As mtdOnlineResource
        Get
            SyncLock _locker
                Return _Linkage
            End SyncLock
        End Get
        Set(ByVal value As mtdOnlineResource)
            SyncLock _locker
                _Linkage = value
            End SyncLock
        End Set
    End Property

End Class