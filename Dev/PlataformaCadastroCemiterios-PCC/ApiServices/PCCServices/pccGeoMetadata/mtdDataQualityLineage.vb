''' <summary>
''' Classe que implementa o Histórico da Qualidade dos Dados
''' </summary>
''' <remarks></remarks>
Public Class mtdDataQualityLineage

    Private _statement_PT As String
    Private _statement_EN As String
    Private _processStep As mtdQualityProcessStep
    Private _source As mtdQualityProcessSource


    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Descrição geral ou declaração sobre a forma como o recurso foi produzido (em português).
    ''' </summary>
    ''' <value>nova Descrição</value>
    ''' <returns>Descrição</returns>
    ''' <remarks></remarks>
    Public Property Statement_PT As String
        Get
            SyncLock _locker
                Return _statement_PT
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _statement_PT = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Descrição geral ou declaração sobre a forma como o recurso foi produzido (em inglês).
    ''' </summary>
    ''' <value>nova Descrição</value>
    ''' <returns>Descrição</returns>
    ''' <remarks></remarks>
    Public Property Statement_EN As String
        Get
            SyncLock _locker
                Return _statement_EN
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _statement_EN = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Etapa do Processo
    ''' </summary>
    ''' <value>nova Etapa do processo</value>
    ''' <returns>Etapa do processo</returns>
    ''' <remarks></remarks>
    Public Property ProcessStep As mtdQualityProcessStep
        Get
            SyncLock _locker
                Return _processStep
            End SyncLock
        End Get
        Set(ByVal value As mtdQualityProcessStep)
            SyncLock _locker
                _processStep = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Fonte de Dados
    ''' </summary>
    ''' <value>nova Fonte de dados</value>
    ''' <returns>Fonte de dados</returns>
    ''' <remarks></remarks>
    Public Property Source As mtdQualityProcessSource
        Get
            SyncLock _locker
                Return _source
            End SyncLock
        End Get
        Set(ByVal value As mtdQualityProcessSource)
            SyncLock _locker
                _source = value
            End SyncLock
        End Set
    End Property

End Class
