''' <summary>
''' Implementa a Classe das Data devido aos Tipos de data (Publicação/Criação/Revisão)
''' </summary>
''' <remarks></remarks>
Public Class mtdDate
    Private _dateType As String
    Private _date As String


    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Tipo de data (Publicação/Criação/Revisão)
    ''' </summary>
    ''' <value>novo Tipo de data</value>
    ''' <returns>Tipo de data</returns>
    ''' <remarks></remarks>
    Public Property DateType As String
        Get
            SyncLock _locker
                Return _dateType
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _dateType = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Data
    ''' </summary>
    ''' <value>nova Data</value>
    ''' <returns>Data</returns>
    ''' <remarks></remarks>
    Public Property [Date] As String
        Get
            SyncLock _locker
                Return _date
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _date = value
            End SyncLock
        End Set
    End Property
End Class
