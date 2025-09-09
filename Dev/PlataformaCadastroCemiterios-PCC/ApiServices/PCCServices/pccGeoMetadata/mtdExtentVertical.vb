''' <summary>
''' Implementa a Classe Extensão Altimétrica
''' </summary>
''' <remarks></remarks>
Public Class mtdExtentVertical
    'Extensão Altimétrica
    Private _minimumValue As String ' Valor mínimo
    Private _maximimValue As String ' Valor máximo

    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Extensão Altimétrica
    ''' </summary>
    ''' <value>novo valor mínimo</value>
    ''' <returns>valor mínimo</returns>
    ''' <remarks></remarks>
    Public Property MinimumValue As String
        Get
            SyncLock _locker
                Return _minimumValue
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _minimumValue = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Extensão Altimétrica
    ''' </summary>
    ''' <value>novo valor máximo</value>
    ''' <returns>valor máximo</returns>
    ''' <remarks></remarks>
    Public Property MaximimValue As String
        Get
            SyncLock _locker
                Return _maximimValue
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _maximimValue = value
            End SyncLock
        End Set
    End Property
End Class
