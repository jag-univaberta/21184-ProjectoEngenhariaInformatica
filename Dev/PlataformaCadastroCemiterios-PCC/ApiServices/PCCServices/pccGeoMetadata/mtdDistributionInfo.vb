''' <summary>
''' Classe com Informação de Distribuição dos dados
''' </summary>
''' <remarks></remarks>
Public Class mtdDistributionInfo
    Private _format As mtdDistributionFormat
    Private _distributorContact As mtdContact
    Private _digitalTransfer As mtdDigitalTransfer


    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

    End Sub


    ''' <summary>
    ''' Formato de Distribuição
    ''' </summary>
    ''' <value>novo Formato de Distribuição</value>
    ''' <returns>Formato de Distribuição</returns>
    ''' <remarks></remarks>
    Public Property Format As mtdDistributionFormat
        Get
            SyncLock _locker
                Return _format
            End SyncLock
        End Get
        Set(ByVal value As mtdDistributionFormat)
            SyncLock _locker
                _format = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Contacto do Distribuidor
    ''' </summary>
    ''' <value>novo Contacto do Distribuidor</value>
    ''' <returns>Contacto do Distribuidor</returns>
    ''' <remarks></remarks>
    Public Property DistributorContact As mtdContact
        Get
            SyncLock _locker
                Return _distributorContact
            End SyncLock
        End Get
        Set(ByVal value As mtdContact)
            SyncLock _locker
                _distributorContact = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Transferência Digital
    ''' </summary>
    ''' <value>nova Transferência Digital</value>
    ''' <returns>Transferência Digital</returns>
    ''' <remarks></remarks>
    Public Property DigitalTransfer As mtdDigitalTransfer
        Get
            SyncLock _locker
                Return _digitalTransfer
            End SyncLock
        End Get
        Set(ByVal value As mtdDigitalTransfer)
            SyncLock _locker
                _digitalTransfer = value
            End SyncLock
        End Set
    End Property


End Class