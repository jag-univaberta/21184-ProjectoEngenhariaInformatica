''' <summary>
''' Classe que implementa as Fontes do Histórico da Qualidade dos Dados
''' </summary>
''' <remarks></remarks>
Public Class mtdQualityProcessSource
    Private _description As String ' Descrição da Fonte de dados
    Private _scaleDenominator As String ' Denominador da Escala

    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Descrição da Fonte de dados
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
    ''' Denominador da Escala da Fonte
    ''' </summary>
    ''' <value>novo Denominador</value>
    ''' <returns>Denominador</returns>
    ''' <remarks></remarks>
    Public Property ScaleDenominator As String
        Get
            SyncLock _locker
                Return _scaleDenominator
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _scaleDenominator = value
            End SyncLock
        End Set
    End Property
End Class
