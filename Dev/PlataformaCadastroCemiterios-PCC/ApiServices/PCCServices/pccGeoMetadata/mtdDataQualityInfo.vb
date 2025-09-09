''' <summary>
''' Implenta a Classe da Qualidade dos Dados
''' </summary>
''' <remarks></remarks>
Public Class mtdDataQualityInfo

    Private _hierarchyLevel As String
    Private _hierarchyLevelDescription As String
    Private _lineage As mtdDataQualityLineage ' Histórico

    Private _report As mtdDataQualityReport ' Relatório
    Private _extent As mtdIdentificationInfoextent ' Extensão


    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Nível Hierarquico dos dados que define o âmbito a que se aplica a qualidade
    ''' </summary>
    ''' <value>novo Nível hierarquico</value>
    ''' <returns>Nível hierarquico</returns>
    ''' <remarks></remarks>
    Public Property HierarchyLevel As String
        Get
            SyncLock _locker
                Return _hierarchyLevel
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _hierarchyLevel = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Descrição do Nível Hierarquico dos dados que define o âmbito a que se aplica a qualidade
    ''' </summary>
    ''' <value>nova Descrição do nível hierarquico</value>
    ''' <returns>Descrição do nível hierarquico</returns>
    ''' <remarks>A preeencher no caso da qualidade se reportar a um sub-conjunto do conjunto de dados geográficos descritos pelos metadados.</remarks>
    Public Property HierarchyLevelDescription As String
        Get
            SyncLock _locker
                Return _hierarchyLevelDescription
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _hierarchyLevelDescription = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Histórico da produção do recurso
    ''' </summary>
    ''' <value>novo Histórico</value>
    ''' <returns>Histórico</returns>
    ''' <remarks></remarks>
    Public Property LineAge As mtdDataQualityLineage
        Get
            SyncLock _locker
                Return _lineage
            End SyncLock
        End Get
        Set(ByVal value As mtdDataQualityLineage)
            SyncLock _locker
                _lineage = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Extensão
    ''' </summary>
    ''' <value>nova Extensão</value>
    ''' <returns>Extensão</returns>
    ''' <remarks></remarks>
    Public Property Extent As mtdIdentificationInfoextent
        Get
            SyncLock _locker
                Return _extent
            End SyncLock
        End Get
        Set(ByVal value As mtdIdentificationInfoextent)
            SyncLock _locker
                _extent = value
            End SyncLock
        End Set
    End Property


End Class
