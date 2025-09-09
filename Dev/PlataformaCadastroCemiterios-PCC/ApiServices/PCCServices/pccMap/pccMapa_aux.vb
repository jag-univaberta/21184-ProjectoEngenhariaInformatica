 

Public Class pccMapaLayerSet_aux
    Private _layers As List(Of pccMapaLayer_aux)
    Private Shared ReadOnly _locker As New Object()
    Public Sub New()
        SyncLock _locker
            _layers = New List(Of pccMapaLayer_aux)
        End SyncLock
    End Sub
    Public Property Layers As List(Of pccMapaLayer_aux)
        Get
            SyncLock _locker
                Return _layers
            End SyncLock
        End Get
        Set(ByVal value As List(Of pccMapaLayer_aux))
            SyncLock _locker
                _layers = value
            End SyncLock
        End Set
    End Property
End Class
Public Class pccMapaLayer_aux
    Private _nome As String
    Private _legendlabel As String
    Private _getvisibility As Boolean
    Private _tipo As Integer
    Private Shared ReadOnly _locker As New Object()
    Public Property Name As String
        Get
            SyncLock _locker
                Return _nome
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _nome = value
            End SyncLock
        End Set
    End Property
    Public Property LegendLabel As String
        Get
            SyncLock _locker
                Return _legendlabel
            End SyncLock 
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _legendlabel = value 
            End SyncLock
        End Set
    End Property
    Public Property GetVisibility As Boolean
        Get
            SyncLock _locker
                Return _getvisibility
            End SyncLock
        End Get
        Set(ByVal value As Boolean)
            SyncLock _locker
                _getvisibility = value
            End SyncLock
        End Set
    End Property
    Public Property Tipo As Integer
        Get
            SyncLock _locker
                Return _tipo
            End SyncLock
        End Get
        Set(ByVal value As Integer)
            SyncLock _locker
                _tipo = value
            End SyncLock
        End Set
    End Property
End Class