''' <summary>
''' Classe que implementa os Relatórios da Qualidade dos Dados
''' </summary>
''' <remarks></remarks>
Public Class mtdDataQualityReport
    Private _nameOfMeasure As String
    Private _measureDescription As String
    Private _date As String
    Private _evaluationMethodType As String
    Private _evaluationMethodDescription As String
    Private _conformanceresult_pass As Boolean ' Resultado - Aprovado sim/Não
    Private _conformanceResult_explanation As String ' Resultado - Justificação
    Private _citation As mtdCitation


    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Nome do Teste aplicado aos dados
    ''' </summary>
    ''' <value>novo Nome</value>
    ''' <returns>Nome do Teste aplicado aos dados</returns>
    ''' <remarks></remarks>
    Public Property NameOfMeasure As String
        Get
            SyncLock _locker
                Return _nameOfMeasure
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _nameOfMeasure = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Descrição da Medida
    ''' </summary>
    ''' <value>nova Descrição</value>
    ''' <returns>Descrição da Medida</returns>
    ''' <remarks></remarks>
    Public Property MeasureDescription As String
        Get
            SyncLock _locker
                Return _measureDescription
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _measureDescription = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Data em que a medida de qualidade foi aplicada
    ''' </summary>
    ''' <value>nova Data</value>
    ''' <returns>Data em que a medida de qualidade foi aplicada</returns>
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
    ''' Tipo de Método usado para avaliar a qualidade dos dados
    ''' </summary>
    ''' <value>novo Tipo de Método</value>
    ''' <returns>Tipo de Método usado</returns>
    ''' <remarks></remarks>
    Public Property EvaluationMethodType As String
        Get
            SyncLock _locker
                Return _evaluationMethodType
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _evaluationMethodType = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Descrição do Método de Avaliação da Qualidade
    ''' </summary>
    ''' <value>nova Descrição</value>
    ''' <returns>Descrição</returns>
    ''' <remarks></remarks>
    Public Property EvaluationMethodDescription As String
        Get
            SyncLock _locker
                Return _evaluationMethodDescription
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _evaluationMethodDescription = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Indicação da conformidade (Aprovado - Sim/Não)
    ''' </summary>
    ''' <value>nova Conformidade</value>
    ''' <returns>Conformidade</returns>
    ''' <remarks></remarks>
    Public Property ConformanceResult_pass As Boolean
        Get
            SyncLock _locker
                Return _conformanceresult_pass
            End SyncLock
        End Get
        Set(ByVal value As Boolean)
            SyncLock _locker
                _conformanceresult_pass = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Explicação do significado da conformidade para este resultado
    ''' </summary>
    ''' <value>nova Justificação</value>
    ''' <returns>Justificação</returns>
    ''' <remarks></remarks>
    Public Property ConformanceResult_explanation As String
        Get
            SyncLock _locker
                Return _conformanceResult_explanation
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _conformanceResult_explanation = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Citação ou Especificação
    ''' </summary>
    ''' <value>nova Citação</value>
    ''' <returns>Citação</returns>
    ''' <remarks></remarks>
    Public Property Citation As mtdCitation
        Get 
            SyncLock _locker
                Return _citation
            End SyncLock
        End Get
        Set(ByVal value As mtdCitation)
            SyncLock _locker
                _citation = value
            End SyncLock
        End Set
    End Property

End Class
