
Imports pccBase4
Public Class pccShapeObject
    Private _propriedades As List(Of pccShapeObjectProperties)
    Private _geometria As pccGeoGeometry

    Private Shared ReadOnly _locker As New Object()

    Public Property Propriedades As List(Of pccShapeObjectProperties)
        Get
            SyncLock _locker
                Return _propriedades
            End SyncLock
        End Get
        Set(ByVal value As List(Of pccShapeObjectProperties))
            SyncLock _locker
                _propriedades = value
            End SyncLock
        End Set
    End Property

    Public Property Geometria() As pccGeoGeometry
        Get
            SyncLock _locker
                Try
                    Return _geometria
                Catch ex As Exception
                    Return Nothing
                End Try
            End SyncLock
        End Get
        Set(ByVal value As pccGeoGeometry)
            SyncLock _locker
                _geometria = value
            End SyncLock
        End Set
    End Property

    Public Sub New()
        SyncLock _locker
            _propriedades = New List(Of pccShapeObjectProperties)
        End SyncLock
    End Sub

End Class

Public Class pccShapeObjectProperties
    Private _nomecampo As String
    Private _valorcampo As String

    Private Shared ReadOnly _locker As New Object()

    Public Property NomeCampo As String
        Get
            SyncLock _locker
                Return _nomecampo
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _nomecampo = value
            End SyncLock
        End Set
    End Property

    Public Property ValorCampo As String
        Get
            SyncLock _locker
                Return _valorcampo
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _valorcampo = value
            End SyncLock
        End Set
    End Property
    Public Sub New()
        SyncLock _locker
            _nomecampo = ""
            _valorcampo = ""
        End SyncLock
    End Sub
End Class
