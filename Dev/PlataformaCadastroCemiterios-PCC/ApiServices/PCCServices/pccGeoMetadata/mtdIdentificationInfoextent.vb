''' <summary>
''' Implementa a Classe Extensão
''' </summary>
''' <remarks></remarks>
Public Class mtdIdentificationInfoextent

    Private _extentdescription As String
    Private _geographicElement As mtdExtentGeographic ' Extensão geográfica
    Private _temporalElement As mtdExtentTemporal ' Extensão temporal
    Private _verticalElement As mtdExtentVertical ' Extensão Altimétrica

    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Descrição textual da extensão
    ''' </summary>
    ''' <value>nova Descrição textual da extensão</value>
    ''' <returns>Descrição textual da extensão</returns>
    ''' <remarks></remarks>
    Public Property Extentdescription As String
        Get
            SyncLock _locker
                Return _extentdescription
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _extentdescription = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Extensão Geográfica
    ''' </summary>
    ''' <value>nova Extensão Geográfica</value>
    ''' <returns>Extensão Geográfica</returns>
    ''' <remarks></remarks>
    Public Property GeographicElement As mtdExtentGeographic
        Get
            SyncLock _locker
                Return _geographicElement
            End SyncLock
        End Get
        Set(ByVal value As mtdExtentGeographic)
            SyncLock _locker
                _geographicElement = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Extensão Temporal
    ''' </summary>
    ''' <value>nova Extensão Temporal</value>
    ''' <returns>Extensão Temporal</returns>
    ''' <remarks></remarks>
    Public Property TemporalElement As mtdExtentTemporal
        Get
            SyncLock _locker
                Return _temporalElement
            End SyncLock
        End Get
        Set(ByVal value As mtdExtentTemporal)
            SyncLock _locker
                _temporalElement = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Extensão Altimétrica
    ''' </summary>
    ''' <value>nova Extensão Altimétrica</value>
    ''' <returns>Extensão Altimétrica</returns>
    ''' <remarks></remarks>
    Public Property VerticalElement As mtdExtentVertical
        Get
            SyncLock _locker
                Return _verticalElement
            End SyncLock
        End Get
        Set(ByVal value As mtdExtentVertical)
            SyncLock _locker
                _verticalElement = value
            End SyncLock
        End Set
    End Property

End Class
