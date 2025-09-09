''' <summary>
''' Implementa a Classe Extensão Temporal
''' </summary>
''' <remarks></remarks>
Public Class mtdExtentTemporal

    ' Extensão Temporal
    Private _beginPosition As String 'Desde
    Private _endPosition As String 'Até

    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Extensão Temporal - Desde (data início)
    ''' </summary>
    ''' <value>nova data início</value>
    ''' <returns>data início</returns>
    ''' <remarks></remarks>
    Public Property BeginPosition As String
        Get
            SyncLock _locker
                Return _beginPosition
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _beginPosition = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Extensão Temporal - Até (data fim)
    ''' </summary>
    ''' <value>nova data fim</value>
    ''' <returns>data fim</returns>
    ''' <remarks></remarks>
    Public Property EndPosition As String
        Get
            SyncLock _locker
                Return _endPosition
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _endPosition = value
            End SyncLock
        End Set
    End Property
End Class
