''' <summary>
''' Implementa a Classe Extensão Geográfica
''' </summary>
''' <remarks></remarks>
Public Class mtdExtentGeographic
    Private _extentTypeCode As Boolean ' Código de Extensão
    'geographicElement do tipo GeographicBoundingBox ou GeographicDescription
    Private _tipo As String
    Private _geographicIdentifier As String
    Private _westBoundLongitude As String
    Private _eastBoundLongitude As String

    Private _southBoundLatitude As String
    Private _northBoundLatitude As String

    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Código de Extensão - Indica se a área definida cobre a extensão do conjunto de dados geográfico ou
    ''' se o conjunto de dados geográfico não está presente neste área
    ''' </summary>
    ''' <value>novo Código de Extensão</value>
    ''' <returns>Código de Extensão</returns>
    ''' <remarks></remarks>
    Public Property ExtentTypeCode As String
        Get
            SyncLock _locker
                Return _extentTypeCode
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _extentTypeCode = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Tipo de Extensão Geográfica, por Coordenadas ou por Identificador - Identificador/Caixa Envolvente
    ''' </summary>
    ''' <value>novo Tipo de Extensão Geográfica</value>
    ''' <returns>Tipo de Extensão Geográfica</returns>
    ''' <remarks></remarks>
    Public Property Tipo As String
        Get
            SyncLock _locker
                Return _tipo
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _tipo = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Identificação da extensão, normalmente um código conhecido, uma NUTS ou limite administrativo
    ''' </summary>
    ''' <value>nova Identificação da extensão</value>
    ''' <returns>Identificação da extensaõ</returns>
    ''' <remarks></remarks>
    Public Property GeographicIdentifier As String
        Get
            SyncLock _locker
                Return _geographicIdentifier
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _geographicIdentifier = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Longitude Oeste - BoundingBox
    ''' </summary>
    ''' <value>novo Longitude Oeste</value>
    ''' <returns>Longitude Oeste</returns>
    ''' <remarks></remarks>
    Public Property WestBoundLongitude As String
        Get
            SyncLock _locker
                Return _westBoundLongitude
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _westBoundLongitude = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Longitude Este - BoundingBox
    ''' </summary>
    ''' <value>novo Longitude Este</value>
    ''' <returns>Longitude Este</returns>
    ''' <remarks></remarks>
    Public Property EastBoundLongitude As String
        Get
            SyncLock _locker
                Return _eastBoundLongitude
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _eastBoundLongitude = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Latitude Sul - BoundingBox
    ''' </summary>
    ''' <value>novo Latitude Sul</value>
    ''' <returns>Latitude Sul</returns>
    ''' <remarks></remarks>
    Public Property SouthBoundLatitude As String
        Get
            SyncLock _locker
                Return _southBoundLatitude
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _southBoundLatitude = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Latitude Norte - BoundingBox
    ''' </summary>
    ''' <value>novo Latitude Norte</value>
    ''' <returns>Latitude Norte</returns>
    ''' <remarks></remarks>
    Public Property NorthBoundLatitude As String
        Get
            SyncLock _locker
                Return _northBoundLatitude
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _northBoundLatitude = value
            End SyncLock
        End Set
    End Property

End Class
