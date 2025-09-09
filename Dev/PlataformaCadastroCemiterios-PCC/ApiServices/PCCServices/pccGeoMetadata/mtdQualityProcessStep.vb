''' <summary>
''' Classe que implementa Etapas do Processo do Histórico da Qualidade dos Dados
''' </summary>
''' <remarks></remarks>
Public Class mtdQualityProcessStep

    Private _date As String ' Data
    Private _description As String ' Descrição
    Private _rationale As String ' Justificação

    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Data da Etapa efectuada
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

    ''' <summary>
    ''' Descrição do processamento efectuado, incluindo parãmetros e tolerancias
    ''' </summary>
    ''' <value>nova Descrição</value>
    ''' <returns>Descrição</returns>
    ''' <remarks></remarks>
    Public Property Description As String
        Get
            SyncLock _locker
                Return _description
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _description = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Justificação para o processamento efectuado
    ''' </summary>
    ''' <value>nova Justificação</value>
    ''' <returns>Justificação</returns>
    ''' <remarks></remarks>
    Public Property Rationale As String
        Get
            SyncLock _locker
                Return _rationale
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _rationale = value
            End SyncLock
        End Set
    End Property

End Class
