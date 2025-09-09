Public Class mtdSpatialResolution

    Private _typeSpactialResolution As String ' Tipo: Distância ou Denominador
    Private _distance As Double ' Distância
    Private _denominator As Double ' Denominador

    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Tipo de Resolução Espacial
    ''' Distância ou Denominador
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TypeSpactialResolution As String
        Get
            SyncLock _locker
                Return _typeSpactialResolution
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _typeSpactialResolution = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Distãncia
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Distance As Double
        Get
            SyncLock _locker
                Return _distance
            End SyncLock
        End Get
        Set(ByVal value As Double)
            SyncLock _locker
                _distance = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Denominador
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Denominator As Double
        Get
            SyncLock _locker
                Return _denominator
            End SyncLock
        End Get
        Set(ByVal value As Double)
            SyncLock _locker
                _denominator = value
            End SyncLock
        End Set
    End Property

End Class
