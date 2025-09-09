''' <summary>
''' Classe Identificação
''' </summary>
''' <remarks></remarks>
Public Class mtdIdentificationInfo

    Private _description_PT As String ' Resumo
    Private _description_EN As String
    Private _purpose_PT As String ' Objectivo
    Private _purpose_EN As String
    Private _language As String
    Private _topicCategory As List(Of String) ' Categoria Temática
    Private _citation As mtdCitation ' Citação
    Private _credit As List(Of String) ' Créditos
    Private _pointOfContact As mtdContact ' Contactos
    Private _resourceConstraints As mtdLegalConstraints ' Restrições
    Private _spatialRepresentationType As List(Of String) ' Tipos de Representação Espacial
    Private _spatialResolution As mtdSpatialResolution ' Resolução Espacial
    Private _descriptiveKeywords As mtdKeywordsType ' Palavras Chave
    Private _extent As mtdIdentificationInfoextent ' extensões 
    Private _resourceMaintenance As List(Of String) ' Códigos de Frequencia de manutenção

    Private _serviceType As String ' Tipo de Serviço
    Private _couplingType As String ' Acoplamento
    Private _serviceoperatesOn As List(Of String) ' Recursos Acoplados
    Private _servicecontainsOperations As mtdServiceOperations ' Operações do Serviço


    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

    End Sub

    ''' <summary>
    ''' Resumo do recurso em Portugês
    ''' </summary>
    ''' <value>novo Resumo</value>
    ''' <returns>Resumo</returns>
    ''' <remarks></remarks>
    Public Property Description_PT As String
        Get
            SyncLock _locker
                Return _description_PT
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _description_PT = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Resumo do recurso em Inglês
    ''' </summary>
    ''' <value>novo Resumo</value>
    ''' <returns>Resumo</returns>
    ''' <remarks></remarks>
    Public Property Description_EN As String
        Get
            SyncLock _locker
                Return _description_EN
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _description_EN = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Objectivo do recurso em Português
    ''' </summary>
    ''' <value>novo Objectivo</value>
    ''' <returns>Objectivo</returns>
    ''' <remarks></remarks>
    Public Property Purpose_PT As String
        Get
            SyncLock _locker
                Return _purpose_PT
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _purpose_PT = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Objectivo do recurso em Inglês
    ''' </summary>
    ''' <value>novo Objectivo</value>
    ''' <returns>Objectivo</returns>
    ''' <remarks></remarks>
    Public Property Purpose_EN As String
        Get
            SyncLock _locker
                Return _purpose_EN
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _purpose_EN = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Idioma do Recurso - Linguagem utilizada no texto do recurso associado
    ''' </summary>
    ''' <value>Novo Idioma</value>
    ''' <returns>Idioma</returns>
    ''' <remarks>Não é usado em Serviços</remarks>
    Public Property Language As String
        Get
            SyncLock _locker
                Return _language
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _language = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Categoria Temática - Lista de Categorias onde se insere o recurso
    ''' </summary>
    ''' <value>nova Lista</value>
    ''' <returns>Lista de Categorias Temática</returns>
    ''' <remarks>Não é usado em Serviços
    ''' Agricultura Pesca Pecuária, Biótopos, Localização, Limites Administrativos, Saúde, ....</remarks>
    Public Property TopicCategory As List(Of String)
        Get
            SyncLock _locker
                Return _topicCategory
            End SyncLock
        End Get
        Set(ByVal value As List(Of String))
            SyncLock _locker
                _topicCategory = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Citação
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

    ''' <summary>
    ''' Créditos - Identificação dos indivíduos ou entidades que contribuiram para a produção do recurso
    ''' </summary>
    ''' <value>nova lista de Créditos</value>
    ''' <returns>lista de créditos</returns>
    ''' <remarks></remarks>
    Public Property Credit As List(Of String)
        Get
            SyncLock _locker
                Return _credit
            End SyncLock
        End Get
        Set(ByVal value As List(Of String))
            SyncLock _locker
                _credit = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Pontos de Contactos: Contactos
    ''' </summary>
    ''' <value>novo Ponto de Contacto</value>
    ''' <returns>Ponto de Contacto</returns>
    ''' <remarks></remarks>
    Public Property PointOfContact As mtdContact
        Get
            SyncLock _locker
                Return _pointOfContact
            End SyncLock
        End Get
        Set(ByVal value As mtdContact)
            SyncLock _locker
                _pointOfContact = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Restrições: Restrições Legais
    ''' </summary>
    ''' <value>nova Restrição</value>
    ''' <returns>Restrição</returns>
    ''' <remarks>Não é usado em Serviços</remarks>
    Public Property ResourceConstraints As mtdLegalConstraints
        Get
            SyncLock _locker
                Return _resourceConstraints
            End SyncLock
        End Get
        Set(ByVal value As mtdLegalConstraints)
            SyncLock _locker
                _resourceConstraints = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Tipo de Representação Espacial
    ''' </summary>
    ''' <value>nova lista de Tipos de Representação Espacial</value>
    ''' <returns>Lista de Tipos de Representação Espacial</returns>
    ''' <remarks>Não é usado em Serviços
    ''' Vectorial, Matricial, Texto Tabela, ...</remarks>
    Public Property SpatialRepresentationType As List(Of String)
        Get
            SyncLock _locker
                Return _spatialRepresentationType
            End SyncLock
        End Get
        Set(ByVal value As List(Of String))
            SyncLock _locker
                _spatialRepresentationType = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Resolução Espacial
    ''' </summary>
    ''' <value>nova Resolução Espacial</value>
    ''' <returns>Resolução Espacial</returns>
    ''' <remarks>Não é usado em Serviços</remarks>
    Public Property SpatialResolution As mtdSpatialResolution
        Get
            SyncLock _locker
                Return _spatialResolution
            End SyncLock
        End Get
        Set(ByVal value As mtdSpatialResolution)
            SyncLock _locker
                _spatialResolution = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Palavras Chave
    ''' </summary>
    ''' <value>nova Palavra Chave</value>
    ''' <returns>Palavra Chave</returns>
    ''' <remarks></remarks>
    Public Property DescriptiveKeywords As mtdKeywordsType
        Get
            SyncLock _locker
                Return _descriptiveKeywords
            End SyncLock
        End Get
        Set(ByVal value As mtdKeywordsType)
            SyncLock _locker
                _descriptiveKeywords = value
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

    ''' <summary>
    ''' Manutenção - Lista de Códigos de Frequência de Manutenção
    ''' </summary>
    ''' <value>nova Lista de Códigos de Frequência de Manutenção</value>
    ''' <returns>Códigos de Frequência de Manutenção</returns>
    ''' <remarks>Não é usado em Serviços
    ''' Contínua, diária, Semanal, ...</remarks>
    Public Property ResourceMaintenance As List(Of String)
        Get
            SyncLock _locker
                Return _resourceMaintenance
            End SyncLock
        End Get
        Set(ByVal value As List(Of String))
            SyncLock _locker
                _resourceMaintenance = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Tipo de Serviço - Nome para o tipo de Serviço (lista INSPIRE)
    ''' </summary>
    ''' <value>novo Tipo de Serviço</value>
    ''' <returns>Tipo de Serviço</returns>
    ''' <remarks>Usado só em Serviços 
    ''' Visualização, Pesquisa, Descarregamento, Transformação, Invocação de Dados Geográficos, Outro Serviço </remarks>
    Public Property ServiceType As String
        Get
            SyncLock _locker
                Return _serviceType
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _serviceType = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Acoplamento - Tipo de acoplamento dos conjuntos de dados geográficos disponibilizados pelo serviço
    ''' </summary>
    ''' <value>novo Acoplamemto</value>
    ''' <returns>Acoplamemto</returns>
    ''' <remarks>Usado só em Serviços</remarks>
    Public Property couplingType As String
        Get
            SyncLock _locker
                Return _couplingType
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _couplingType = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Identificador dos Recursos Acoplados
    ''' </summary>
    ''' <value>novo Recurso Acoplado</value>
    ''' <returns>Recurso Acoplado</returns>
    ''' <remarks>Usado só em Serviços</remarks>
    Public Property ServiceoperatesOn As List(Of String)
        Get
            SyncLock _locker
                Return _serviceoperatesOn
            End SyncLock
        End Get
        Set(ByVal value As List(Of String))
            SyncLock _locker
                _serviceoperatesOn = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Operações do Serviço
    ''' </summary>
    ''' <value>novas Operações do Serviço</value>
    ''' <returns>Operações do Serviço</returns>
    ''' <remarks>Usado só em Serviços</remarks>
    Public Property ServicecontainsOperations As mtdServiceOperations
        Get
            SyncLock _locker
                Return _servicecontainsOperations
            End SyncLock
        End Get
        Set(ByVal value As mtdServiceOperations)
            SyncLock _locker
                _servicecontainsOperations = value
            End SyncLock
        End Set
    End Property

End Class
