Imports phSETFRAME.g10phPDF

Namespace PlugIn

    Public Structure Evoca��oDEF

        ' ser� fun��o de um processador de funcionalidades
        ' determinar o id atrav�s da sintaxe se este n�o for passado

        Public ComID As Integer
        Public Parametros() As Object

        Public Executado As Boolean
        Public Sucesso As Boolean
        Public Resultado As String

    End Structure

    Public Interface IPROMPT

        ' entidades exteriores � consola podem evocar a realiza��o de comandos
        Function Realiza(ByVal Evoca��o As Evoca��oDEF) As Evoca��oDEF
        Function Realiza(ByVal Sintaxe As String, ByVal ParamArray Parametros() As Object) As Evoca��oDEF
        Function Realiza(ByVal ID As Integer, ByVal ParamArray Parametros() As Object) As Evoca��oDEF
        ' podemos ter uma consola que l� scripts e evoca no processador de funcionalidades 
        ' a execu��o de comandos 
        ' se alguma comando exigir perguntas ser� a consola a recebe-las :) e a tira-los do script
        Function Realiza(ByVal Script As String) As Evoca��oDEF

        ' alguns comandos podem exigir ao processador determinados parametros e se n�o forem fornecidos
        ' o processador pode pedir � consola a resposta para os mesmos
        Function Pergunta(ByVal Sintaxe As String) As String

        ' lista de processadores de funcionalidades a usar pela consola
        ' os processadores devem ser evocados um a um at� que o comando em quest�o seja executado
        Property Processadores() As IPROFUN()
        Sub ProcessadoresAdiciona(ByVal Processador As IPROFUN)
        Sub ProcessadoresRemove(ByVal Processador As IPROFUN)

        Sub Reporta(ByVal Relatorio As String, ByVal LinhasNovas As Integer)

        Property Mordomo() As Acompanhamento.IMORDOMO

        ' esta interface representa uma consola onde podemos digitar e seguir a execu��o de comandos
        '   representativos de funcionalidades da aplica��o
        ' regra geral uma consola usar� um ou mais processadores de fun��es para a execu��o dos comandos

    End Interface

    Public Interface IPROFUN

        Function Evoca(ByVal Prompter As IPROMPT, ByVal Evoca��o As Evoca��oDEF) As Evoca��oDEF
        Function Evoca(ByVal Prompter As IPROMPT, ByVal Sintaxe As String, ByVal ParamArray Parametros() As Object) As Evoca��oDEF

        Event Pr�Evoca��o(ByVal Processador As IPROFUN, ByVal Comando As Evoca��oDEF, ByRef getCancelamento As Boolean)
        Event Pr�Pergunta(ByVal Processador As IPROFUN, ByVal Comando As Evoca��oDEF, ByVal Pergunta As String, ByRef getCancelamento As Boolean)
        Event P�sPergunta(ByVal Processador As IPROFUN, ByVal Comando As Evoca��oDEF, ByVal Pergunta As String, ByVal Resposta As String)
        Event P�sEvoca��o(ByVal Processador As IPROFUN, ByVal Comando As Evoca��oDEF, ByVal Sucesso As Boolean)

        ' a implementa��o desta interface resultar� em processadores de funcionalidades
        ' que acrescentar�o m�todos pr�prios para representar/evocar todas as funcionalidades 
        ' que a respectiva aplica��o expoe

    End Interface

    Public Interface IREMOTO

        Property Distribuido() As Boolean

        Property ServidorURL() As String
        Property ServidorPorta() As String

        Function Evoca(ByVal Prompter As IPROMPT, ByVal Evoca��o As Evoca��oDEF) As Evoca��oDEF

    End Interface

    '                                                                                               

    <AttributeUsage(AttributeTargets.All)> _
    Public Class PlugInFlag
        Inherits System.Attribute
        ' atributo a usar para marcar as classes a lan�ar como plugins da aplica��o base
    End Class

    Public Interface IPLUGIN

        ' identifica��o do plugin
        ReadOnly Property ID() As Integer
        ReadOnly Property Titulo() As String
        ReadOnly Property Descri��o() As String

        ' logo e cor identificativas do plugin
        ReadOnly Property Logo() As System.Drawing.Image
        ReadOnly Property Cor() As System.Drawing.Color

        ' processador de comandos pr�prio do plugin
        ReadOnly Property Processador() As IPROFUN

        ' lista de rastreadores associados ao plugin
        ' no inicio o plugin carrega nesta lista os seus rastreadores
        ' normalmente esta propriedade ser� gerida pela aplica��o base
        Property ListaDeRastreadores() As Colec��oDeRastreadores

        ' 1� m�todo a executar pela aplica��o base
        ' o plugin s� deve iniciar a sua actividade ap�s evoca��o deste m�todo
        ' deve ser este m�todo a evocar o evento "EmAbertura"
        Sub Main(ByVal aplica��o As IAPPSUPORTE)

        ' ultimo m�todo a executar pela aplica��o base
        ' o plugin deve libertar todos os recursos usados pois a sua instancia ser� descartada
        ' deve ser este m�todo a evocar o evento "EmFecho"
        Sub MainEnd(ByVal aplica��o As IAPPSUPORTE)

        ' abertura e fecho do pr�prio plugin 
        Event EmAbertura(ByVal aplica��o As IAPPSUPORTE, ByVal plugin As IPLUGIN)
        Event EmFecho(ByVal aplica��o As IAPPSUPORTE, ByVal plugin As IPLUGIN)

    End Interface

    Public Interface IRASTREADOR

        ' identifica��o do rastreador
        ReadOnly Property ID() As Integer
        ReadOnly Property Titulo() As String
        ReadOnly Property Descri��o() As String

        ' plugin ao qual o rastreador est� associado
        ' � o plugin que cria o rastreador que se regista nesta propriedade
        ' normalmente esta propriedade ser� gerida pela aplica��o base
        Property Plugin() As IPLUGIN

        ' diz se o rastreador est� activo, isto �: a rastrear eventos
        ' a aplica��o base ir� olhar para esta propriedade antes de lhe comunicar um evento
        ' a aplica��o base poder� controlar a propriedade
        ' deve ser a mudan�a sobre este propriedade a evocar os eventos "EmActiva��o" e "EmDesactiva��o"
        Property Activo() As Boolean

        ' aquando da inser��o do rastreador na fila de restreio da aplica��o base
        ' o rastreador � inserido antes do 1� rastreador com prioridade igual ou maior
        ReadOnly Property Prioridade() As Integer

        ' diz se o rastreador rastreia determinado evento
        ReadOnly Property Restreia(ByVal EventoNome As String) As Boolean

        ' activa��o e desactiva��o do rastreador 
        Event EmActiva��o(ByVal aplica��o As IAPPSUPORTE, ByVal rastreador As IRASTREADOR)
        Event EmDesactiva��o(ByVal aplica��o As IAPPSUPORTE, ByVal rastreador As IRASTREADOR)

    End Interface

    Public Interface IAPPSUPORTE

        ' lista de plugins e rastreadores controlados pela aplica��o base
        Property ListaDePlugIns() As Colec��oDePlugIns
        Property ListaDeRastreadores() As Colec��oDeRastreadores

        ' cliente de dados usado pela aplica��o base
        Property ClienteDB() As Setframe.setClienteDb

        ' diz se a aplica��o se encontra em processo de auto-lan�amento
        ' � durante este processo que os plugins s�o lidos a a aplica��o configurada
        Property EmLan�amento() As Boolean

        ' eventos de restreio da aplica��o
        Event PluginAAbrir(ByVal aplica��o As IAPPSUPORTE, ByVal plugin As IPLUGIN)
        Event PluginAberto(ByVal aplica��o As IAPPSUPORTE, ByVal plugin As IPLUGIN)
        Event PluginAFechar(ByVal aplica��o As IAPPSUPORTE, ByVal plugin As IPLUGIN)
        Event PluginFechado(ByVal aplica��o As IAPPSUPORTE, ByVal plugin As IPLUGIN)
        Event Ap�sLan�amento(ByVal aplica��o As IAPPSUPORTE)

    End Interface

    Public Class Colec��oDePlugIns
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
            ' tratamento adicional � inser��o
        End Sub

        Protected Overrides Sub OnRemove(ByVal index As Integer, ByVal value As [Object])
            ' tratamento adicional � remo��o
        End Sub

        Protected Overrides Sub OnSet(ByVal index As Integer, ByVal oldValue As [Object], ByVal newValue As [Object])
            ' tratamento adicional � atribui��o
        End Sub

        Protected Overrides Sub OnValidate(ByVal value As Object)

            If Not TypeOf value Is IPLUGIN Then
                Throw New ArgumentException("Conteudo deve ser do tipo 'IPLUGIN'.", "value")
            End If

        End Sub

    End Class

    Public Class Colec��oDeRastreadores
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
            ' tratamento adicional � inser��o
        End Sub

        Protected Overrides Sub OnRemove(ByVal index As Integer, ByVal value As [Object])
            ' tratamento adicional � remo��o
        End Sub

        Protected Overrides Sub OnSet(ByVal index As Integer, ByVal oldValue As [Object], ByVal newValue As [Object])
            ' tratamento adicional � atribui��o
        End Sub

        Protected Overrides Sub OnValidate(ByVal value As [Object])

            If Not TypeOf value Is IRASTREADOR Then
                Throw New ArgumentException("Conteudo deve ser do tipo 'IRASTREADOR'.", "value")
            End If

        End Sub

    End Class

    '                                                                                               

    Public Class Aplica��oDeSuporte
        Implements IAPPSUPORTE

        Private l_clientedb As Setframe.setClienteDb
        Private l_emlan�amento As Boolean
        Private l_plugins As Colec��oDePlugIns
        Private l_rastreadores As Colec��oDeRastreadores

        Public Event Ap�sLan�amento(ByVal aplica��o As IAPPSUPORTE) Implements IAPPSUPORTE.Ap�sLan�amento
        Public Event PluginAAbrir(ByVal aplica��o As IAPPSUPORTE, ByVal plugin As IPLUGIN) Implements IAPPSUPORTE.PluginAAbrir
        Public Event PluginAberto(ByVal aplica��o As IAPPSUPORTE, ByVal plugin As IPLUGIN) Implements IAPPSUPORTE.PluginAberto
        Public Event PluginAFechar(ByVal aplica��o As IAPPSUPORTE, ByVal plugin As IPLUGIN) Implements IAPPSUPORTE.PluginAFechar
        Public Event PluginFechado(ByVal aplica��o As IAPPSUPORTE, ByVal plugin As IPLUGIN) Implements IAPPSUPORTE.PluginFechado


        Public Property ClienteDB() As Setframe.setClienteDb Implements IAPPSUPORTE.ClienteDB
            Get

                Return l_clientedb

            End Get
            Set(ByVal Value As Setframe.setClienteDb)

                l_clientedb = Value

            End Set
        End Property

        Public Property EmLan�amento() As Boolean Implements IAPPSUPORTE.EmLan�amento
            Get

                Return l_emlan�amento

            End Get
            Set(ByVal Value As Boolean)

                l_emlan�amento = Value

            End Set
        End Property

        Public Property ListaDePlugIns() As Colec��oDePlugIns Implements IAPPSUPORTE.ListaDePlugIns
            Get

                Return l_plugins

            End Get
            Set(ByVal Value As Colec��oDePlugIns)

                l_plugins = Value

            End Set
        End Property

        Public Property ListaDeRastreadores() As Colec��oDeRastreadores Implements IAPPSUPORTE.ListaDeRastreadores
            Get

                Return l_rastreadores

            End Get
            Set(ByVal Value As Colec��oDeRastreadores)

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