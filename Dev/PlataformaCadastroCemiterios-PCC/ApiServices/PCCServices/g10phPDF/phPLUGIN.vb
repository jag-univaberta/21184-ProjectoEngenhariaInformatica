Imports phSETFRAME.g10phPDF

Namespace PlugIn

    Public Structure EvocaçãoDEF

        ' será função de um processador de funcionalidades
        ' determinar o id através da sintaxe se este não for passado

        Public ComID As Integer
        Public Parametros() As Object

        Public Executado As Boolean
        Public Sucesso As Boolean
        Public Resultado As String

    End Structure

    Public Interface IPROMPT

        ' entidades exteriores à consola podem evocar a realização de comandos
        Function Realiza(ByVal Evocação As EvocaçãoDEF) As EvocaçãoDEF
        Function Realiza(ByVal Sintaxe As String, ByVal ParamArray Parametros() As Object) As EvocaçãoDEF
        Function Realiza(ByVal ID As Integer, ByVal ParamArray Parametros() As Object) As EvocaçãoDEF
        ' podemos ter uma consola que lê scripts e evoca no processador de funcionalidades 
        ' a execução de comandos 
        ' se alguma comando exigir perguntas será a consola a recebe-las :) e a tira-los do script
        Function Realiza(ByVal Script As String) As EvocaçãoDEF

        ' alguns comandos podem exigir ao processador determinados parametros e se não forem fornecidos
        ' o processador pode pedir à consola a resposta para os mesmos
        Function Pergunta(ByVal Sintaxe As String) As String

        ' lista de processadores de funcionalidades a usar pela consola
        ' os processadores devem ser evocados um a um até que o comando em questão seja executado
        Property Processadores() As IPROFUN()
        Sub ProcessadoresAdiciona(ByVal Processador As IPROFUN)
        Sub ProcessadoresRemove(ByVal Processador As IPROFUN)

        Sub Reporta(ByVal Relatorio As String, ByVal LinhasNovas As Integer)

        Property Mordomo() As Acompanhamento.IMORDOMO

        ' esta interface representa uma consola onde podemos digitar e seguir a execução de comandos
        '   representativos de funcionalidades da aplicação
        ' regra geral uma consola usará um ou mais processadores de funções para a execução dos comandos

    End Interface

    Public Interface IPROFUN

        Function Evoca(ByVal Prompter As IPROMPT, ByVal Evocação As EvocaçãoDEF) As EvocaçãoDEF
        Function Evoca(ByVal Prompter As IPROMPT, ByVal Sintaxe As String, ByVal ParamArray Parametros() As Object) As EvocaçãoDEF

        Event PréEvocação(ByVal Processador As IPROFUN, ByVal Comando As EvocaçãoDEF, ByRef getCancelamento As Boolean)
        Event PréPergunta(ByVal Processador As IPROFUN, ByVal Comando As EvocaçãoDEF, ByVal Pergunta As String, ByRef getCancelamento As Boolean)
        Event PósPergunta(ByVal Processador As IPROFUN, ByVal Comando As EvocaçãoDEF, ByVal Pergunta As String, ByVal Resposta As String)
        Event PósEvocação(ByVal Processador As IPROFUN, ByVal Comando As EvocaçãoDEF, ByVal Sucesso As Boolean)

        ' a implementação desta interface resultará em processadores de funcionalidades
        ' que acrescentarão métodos próprios para representar/evocar todas as funcionalidades 
        ' que a respectiva aplicação expoe

    End Interface

    Public Interface IREMOTO

        Property Distribuido() As Boolean

        Property ServidorURL() As String
        Property ServidorPorta() As String

        Function Evoca(ByVal Prompter As IPROMPT, ByVal Evocação As EvocaçãoDEF) As EvocaçãoDEF

    End Interface

    '                                                                                               

    <AttributeUsage(AttributeTargets.All)> _
    Public Class PlugInFlag
        Inherits System.Attribute
        ' atributo a usar para marcar as classes a lançar como plugins da aplicação base
    End Class

    Public Interface IPLUGIN

        ' identificação do plugin
        ReadOnly Property ID() As Integer
        ReadOnly Property Titulo() As String
        ReadOnly Property Descrição() As String

        ' logo e cor identificativas do plugin
        ReadOnly Property Logo() As System.Drawing.Image
        ReadOnly Property Cor() As System.Drawing.Color

        ' processador de comandos próprio do plugin
        ReadOnly Property Processador() As IPROFUN

        ' lista de rastreadores associados ao plugin
        ' no inicio o plugin carrega nesta lista os seus rastreadores
        ' normalmente esta propriedade será gerida pela aplicação base
        Property ListaDeRastreadores() As ColecçãoDeRastreadores

        ' 1º método a executar pela aplicação base
        ' o plugin só deve iniciar a sua actividade após evocação deste método
        ' deve ser este método a evocar o evento "EmAbertura"
        Sub Main(ByVal aplicação As IAPPSUPORTE)

        ' ultimo método a executar pela aplicação base
        ' o plugin deve libertar todos os recursos usados pois a sua instancia será descartada
        ' deve ser este método a evocar o evento "EmFecho"
        Sub MainEnd(ByVal aplicação As IAPPSUPORTE)

        ' abertura e fecho do próprio plugin 
        Event EmAbertura(ByVal aplicação As IAPPSUPORTE, ByVal plugin As IPLUGIN)
        Event EmFecho(ByVal aplicação As IAPPSUPORTE, ByVal plugin As IPLUGIN)

    End Interface

    Public Interface IRASTREADOR

        ' identificação do rastreador
        ReadOnly Property ID() As Integer
        ReadOnly Property Titulo() As String
        ReadOnly Property Descrição() As String

        ' plugin ao qual o rastreador está associado
        ' é o plugin que cria o rastreador que se regista nesta propriedade
        ' normalmente esta propriedade será gerida pela aplicação base
        Property Plugin() As IPLUGIN

        ' diz se o rastreador está activo, isto é: a rastrear eventos
        ' a aplicação base irá olhar para esta propriedade antes de lhe comunicar um evento
        ' a aplicação base poderá controlar a propriedade
        ' deve ser a mudança sobre este propriedade a evocar os eventos "EmActivação" e "EmDesactivação"
        Property Activo() As Boolean

        ' aquando da inserção do rastreador na fila de restreio da aplicação base
        ' o rastreador é inserido antes do 1º rastreador com prioridade igual ou maior
        ReadOnly Property Prioridade() As Integer

        ' diz se o rastreador rastreia determinado evento
        ReadOnly Property Restreia(ByVal EventoNome As String) As Boolean

        ' activação e desactivação do rastreador 
        Event EmActivação(ByVal aplicação As IAPPSUPORTE, ByVal rastreador As IRASTREADOR)
        Event EmDesactivação(ByVal aplicação As IAPPSUPORTE, ByVal rastreador As IRASTREADOR)

    End Interface

    Public Interface IAPPSUPORTE

        ' lista de plugins e rastreadores controlados pela aplicação base
        Property ListaDePlugIns() As ColecçãoDePlugIns
        Property ListaDeRastreadores() As ColecçãoDeRastreadores

        ' cliente de dados usado pela aplicação base
        Property ClienteDB() As Setframe.setClienteDb

        ' diz se a aplicação se encontra em processo de auto-lançamento
        ' é durante este processo que os plugins são lidos a a aplicação configurada
        Property EmLançamento() As Boolean

        ' eventos de restreio da aplicação
        Event PluginAAbrir(ByVal aplicação As IAPPSUPORTE, ByVal plugin As IPLUGIN)
        Event PluginAberto(ByVal aplicação As IAPPSUPORTE, ByVal plugin As IPLUGIN)
        Event PluginAFechar(ByVal aplicação As IAPPSUPORTE, ByVal plugin As IPLUGIN)
        Event PluginFechado(ByVal aplicação As IAPPSUPORTE, ByVal plugin As IPLUGIN)
        Event ApósLançamento(ByVal aplicação As IAPPSUPORTE)

    End Interface

    Public Class ColecçãoDePlugIns
        Inherits CollectionBase



        Default Public Property Item(ByVal index As Integer) As IPLUGIN
            Get

                Return CType(List(index), IPLUGIN)

            End Get
            Set(ByVal Value As IPLUGIN)

                List(index) = Value

            End Set
        End Property

        Public Function IndexOf(ByVal plugin As IPLUGIN) As Integer

            Return List.IndexOf(plugin)

        End Function

        Public Function Add(ByVal plugin As IPLUGIN) As Integer

            Return List.Add(plugin)

        End Function

        Public Sub Insert(ByVal index As Integer, ByVal plugin As IPLUGIN)

            List.Insert(index, plugin)

        End Sub

        Public Sub Remove(ByVal plugin As IPLUGIN)

            List.Remove(plugin)

        End Sub

        Public Function Contains(ByVal plugin As IPLUGIN) As Boolean

            Return List.Contains(plugin)

        End Function

        Protected Overrides Sub OnInsert(ByVal index As Integer, ByVal value As [Object])
            ' tratamento adicional à inserção
        End Sub

        Protected Overrides Sub OnRemove(ByVal index As Integer, ByVal value As [Object])
            ' tratamento adicional à remoção
        End Sub

        Protected Overrides Sub OnSet(ByVal index As Integer, ByVal oldValue As [Object], ByVal newValue As [Object])
            ' tratamento adicional à atribuição
        End Sub

        Protected Overrides Sub OnValidate(ByVal value As Object)

            If Not TypeOf value Is IPLUGIN Then
                Throw New ArgumentException("Conteudo deve ser do tipo 'IPLUGIN'.", "value")
            End If

        End Sub

    End Class

    Public Class ColecçãoDeRastreadores
        Inherits CollectionBase


        Default Public Property Item(ByVal index As Integer) As IRASTREADOR
            Get

                Return CType(List(index), IRASTREADOR)

            End Get
            Set(ByVal Value As IRASTREADOR)

                List(index) = Value

            End Set
        End Property

        Public Function Add(ByVal rastreador As IRASTREADOR) As Integer

            Return List.Add(rastreador)

        End Function

        Public Function IndexOf(ByVal rastreador As IRASTREADOR) As Integer

            Return List.IndexOf(rastreador)

        End Function

        Public Sub Insert(ByVal index As Integer, ByVal rastreador As IRASTREADOR)

            List.Insert(index, rastreador)

        End Sub

        Public Sub Remove(ByVal rastreador As IRASTREADOR)

            List.Remove(rastreador)

        End Sub

        Public Function Contains(ByVal rastreador As IRASTREADOR) As Boolean

            Return List.Contains(rastreador)

        End Function

        Protected Overrides Sub OnInsert(ByVal index As Integer, ByVal value As [Object])
            ' tratamento adicional à inserção
        End Sub

        Protected Overrides Sub OnRemove(ByVal index As Integer, ByVal value As [Object])
            ' tratamento adicional à remoção
        End Sub

        Protected Overrides Sub OnSet(ByVal index As Integer, ByVal oldValue As [Object], ByVal newValue As [Object])
            ' tratamento adicional à atribuição
        End Sub

        Protected Overrides Sub OnValidate(ByVal value As [Object])

            If Not TypeOf value Is IRASTREADOR Then
                Throw New ArgumentException("Conteudo deve ser do tipo 'IRASTREADOR'.", "value")
            End If

        End Sub

    End Class

    '                                                                                               

    Public Class AplicaçãoDeSuporte
        Implements IAPPSUPORTE

        Private l_clientedb As Setframe.setClienteDb
        Private l_emlançamento As Boolean
        Private l_plugins As ColecçãoDePlugIns
        Private l_rastreadores As ColecçãoDeRastreadores

        Public Event ApósLançamento(ByVal aplicação As IAPPSUPORTE) Implements IAPPSUPORTE.ApósLançamento
        Public Event PluginAAbrir(ByVal aplicação As IAPPSUPORTE, ByVal plugin As IPLUGIN) Implements IAPPSUPORTE.PluginAAbrir
        Public Event PluginAberto(ByVal aplicação As IAPPSUPORTE, ByVal plugin As IPLUGIN) Implements IAPPSUPORTE.PluginAberto
        Public Event PluginAFechar(ByVal aplicação As IAPPSUPORTE, ByVal plugin As IPLUGIN) Implements IAPPSUPORTE.PluginAFechar
        Public Event PluginFechado(ByVal aplicação As IAPPSUPORTE, ByVal plugin As IPLUGIN) Implements IAPPSUPORTE.PluginFechado


        Public Property ClienteDB() As Setframe.setClienteDb Implements IAPPSUPORTE.ClienteDB
            Get

                Return l_clientedb

            End Get
            Set(ByVal Value As Setframe.setClienteDb)

                l_clientedb = Value

            End Set
        End Property

        Public Property EmLançamento() As Boolean Implements IAPPSUPORTE.EmLançamento
            Get

                Return l_emlançamento

            End Get
            Set(ByVal Value As Boolean)

                l_emlançamento = Value

            End Set
        End Property

        Public Property ListaDePlugIns() As ColecçãoDePlugIns Implements IAPPSUPORTE.ListaDePlugIns
            Get

                Return l_plugins

            End Get
            Set(ByVal Value As ColecçãoDePlugIns)

                l_plugins = Value

            End Set
        End Property

        Public Property ListaDeRastreadores() As ColecçãoDeRastreadores Implements IAPPSUPORTE.ListaDeRastreadores
            Get

                Return l_rastreadores

            End Get
            Set(ByVal Value As ColecçãoDeRastreadores)

                l_rastreadores = Value

            End Set
        End Property

        '                                                                           

        Public Function CarregaPlugIn(ByVal FicheiroDLL As String) As Boolean

            Dim tiposlidos() As System.Type
            Dim t As Integer
            Dim plugin As IPLUGIN

            If Setframe.AssemblagemLoadTipo(FicheiroDLL, "", New PlugInFlag, tiposlidos) Then

                For t = 0 To tiposlidos.GetUpperBound(0)

                    plugin = CType(Setframe.AssemblagemConstroiTipo(FicheiroDLL, "", tiposlidos(t).FullName), IPLUGIN)

                    If Not plugin Is Nothing Then

                        Me.ListaDePlugIns.Add(plugin)

                        RaiseEvent PluginAAbrir(Me, plugin)
                        plugin.Main(Me)
                        RaiseEvent PluginAberto(Me, plugin)

                    End If

                Next

            End If

        End Function

        Public Function DescarregaPlugIn(ByVal plugin As IPLUGIN) As Boolean

            RaiseEvent PluginAFechar(Me, plugin)
            plugin.MainEnd(Me)
            RaiseEvent PluginFechado(Me, plugin)

            Me.ListaDePlugIns.Remove(plugin)

        End Function

    End Class

End Namespace