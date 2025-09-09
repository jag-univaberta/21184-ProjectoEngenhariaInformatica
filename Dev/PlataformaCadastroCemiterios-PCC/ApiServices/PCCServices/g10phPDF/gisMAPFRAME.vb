Namespace Mapframe

    Public Module mapFrame

        Private ReadOnly _locker As New Object()

        Public Function DirecçãoFromSentido(ByVal Sentido As mapSentidos) As mapDirecções

            Dim hor As Integer
            Dim ver As Integer

            If Sentido And mapSentidos.Direita = mapSentidos.Direita Then hor = hor + 1
            If Sentido And mapSentidos.Esquerda = mapSentidos.Esquerda Then hor = hor + 1
            If Sentido And mapSentidos.Superior = mapSentidos.Superior Then ver = ver + 1
            If Sentido And mapSentidos.Inferior = mapSentidos.Inferior Then ver = ver + 1

            If hor > 0 And ver = 0 Then Return mapDirecções.Horizontal
            If hor = 0 And ver > 0 Then Return mapDirecções.Vertical
            If hor > 0 And ver > 0 Then Return mapDirecções.Obliqua

            Return mapDirecções.Desconhecida

        End Function

    End Module

    '                                                                                           

    Public Enum mapAlinhamentosHorizontais

        Centro = 1
        Direita = 2
        Esquerda = 3

    End Enum

    Public Enum mapAlinhamentosVerticais

        Superior = 1
        MeiaAltura = 2
        Central = 3
        Base = 4
        Inferior = 5

    End Enum

    Public Structure mapAlinhamento

        Public Horizontal As mapAlinhamentosHorizontais
        Public Vertical As mapAlinhamentosVerticais

    End Structure

    Public Enum mapWrappers

        Desconhecido = 0

        MapGuideViewer = 1

    End Enum

    Public Enum mapVisSelecções

        ' modos de selcção possíveis no visualizador

        Null = -1
        Desconhecida = 0

        ' Centroi e Intersection são para usar com SelecçãoModo do Visualizador
        Centroid = 1
        Intersection = 2

        ' Poligono, Raio e Area são para usar com SeleccionaUI de entidades Navegáveis
        Poligono = 3
        Raio = 4
        Area = 5

    End Enum

    Public Enum mapVisEstados

        ' modos de operação do visualizador

        Null = -1
        Desconhecido = 0

        Ocupado = 1
        DigCirculo = 2
        DigLinha = 4
        DigPonto = 8
        DigPoliLinha = 16
        DigPoligono = 32
        DigRectangulo = 64
        Sel = 128
        SelPoligono = 256
        SelRaio = 512
        VendoDistancia = 1024
        PanUp = 2048
        PanDown = 4096
        PanRight = 8192
        PanLeft = 16384
        ZoomingIn = 32768
        ZoomingOut = 65536

        Digitalizando = 2 + 4 + 8 + 16 + 32
        Seleccionando = 128 + 256 + 512
        Vendo = 1024
        Panning = 2048 + 4096 + 8192 + 16384 + 32768 + 65536
        Zooming = 32768 + 65536
        Navegando = Zooming + Panning

    End Enum

    Public Enum mapNavModos

        Null = -1
        Desconhecido = 0

        ZoomIn = 1
        ZoomOut = 2
        ZoomArea = 3
        Pan = 4

    End Enum

    Public Enum mapObjectoTipos

        ' tipos que um objecto pode assumir

        Null = -1
        Desconhecido = 0

        Texto = 1
        Ponto = 2
        Linha = 3
        Polilinha = 4
        Poligono = 5
        Circulo = 6
        Rectangulo = 7

    End Enum

    Public Enum mapUnidades

        ' unidades de medida

        Null = -1
        Desconhecida = 0

        Graus = 1
        Metros = 2
        Centimentros = 3
        Kilometros = 4
        Pés = 5
        Milhas = 6

    End Enum

    Public Enum mapDirecções

        Null = -1
        Desconhecida = 0

        Horizontal = 1
        Vertical = 2
        Obliqua = 3

    End Enum

    Public Enum mapSentidos

        ' sentidos base de navegação
        ' os valores atribuidos são escalados em potencia de 2 de forma a permitir
        ' a conjugação de vários sentidos base

        Null = -1
        Desconhecido = 0

        Esquerda = 1
        Direita = 2
        Superior = 4
        Inferior = 8

    End Enum

    Public Enum mapMapaProcessamentos

        Null = -1
        Desconhecido = 0

        EmProcessamento = 1
        EmEspera = 2

    End Enum

    Public Enum mapLayerTipos

        ' tipos que um layer pode assumir

        Null = -1
        Desconhecido = 0

        Pontos = 1
        Texto = 2
        PoliLinha = 3
        Poligono = 4
        Raster = 5
        Buffer = 7
        Redline = 8

    End Enum

    Public Enum mapLayerOrigens

        Null = -1
        Desconhecida = 0

        OleDb = 1
        SpatialDataProvider = 2
        FicheiroSDF = 4
        FicheiroDWG = 8
        Raster = 16
        Tema = 32

        Cartografica = 2 + 4 + 8
        BaseDeDados = 1

    End Enum

    Public Enum mapLinhaEstilos

        Null = -1
        Desconhecido = 0

        Solid = 1
        Dash = 2
        Dot = 3
        DashDot = 4
        DashDotDot = 5
        Rail = 6

        ' todo: mapFrame: adicionar restantes estilos de linha

    End Enum

    Public Enum mapEncheModos

        Null = -1
        Desconhecido = 0

        Transparente = 1
        Opaco = 2

    End Enum

    Public Enum mapEncheEstilos

        Null = -1
        Desconhecido = 0

        Horizontal = 1
        Vertical = 2

        ' todo: mapFrame: adicionar restantes estilos de preenchimento

    End Enum

    Public Enum mapTiposSistema

        Null = -1

        Naturais = 0
        Elipsoidais = 1
        Rectangulares = 2

        NãoGeodesicos = 0
        Geodisicos = 1 + 2

    End Enum

    '                                                                                           

    Public Interface ICARTOGRAFIA

        ' frase de conexão ao servidor da cartografia e frase de configuração da conexão
        Property Conexão() As String
        Property Setup() As String

        ' conteudo cartográfico da cartografia
        ReadOnly Property Conteudo() As ICARTA

        ' sistema de coordenadas usado pela cartografia
        Property SistemaCoordenadas() As ISISTEMACOORDENADAS

        ' estado actual de processamento da cartografia
        ReadOnly Property Estado() As mapMapaProcessamentos

        ' indica se a cartografia está disponível para uso
        ' a cartografia ficará indisponível sempre que actualizar toda ou parte dos dados que representa
        ReadOnly Property Ocupado() As Boolean

        ' nome da cartografia
        ReadOnly Property Nome() As String

        ' eventos:
        ' Pre...Em...PosConexão: antes, durante e após conexão
        Event PreConexão(ByVal Cartografia As ICARTOGRAFIA, ByRef Splasher As System.Windows.Forms.Control, ByRef Parametros() As Object)
        Event EmConexão(ByVal Cartografia As ICARTOGRAFIA, ByRef Splasher As System.Windows.Forms.Control, ByRef Parametros() As Object, ByVal Passo As Integer)
        Event PosConexão(ByVal Cartografia As ICARTOGRAFIA, ByRef Splasher As System.Windows.Forms.Control, ByRef Parametros() As Object)

        ' EmTarefa: durante uma tarefa não especificada
        Event EmTarefa(ByVal Cartografia As ICARTOGRAFIA, ByVal Tarefa As Integer, ByVal Parametros() As Object)

        Property Domino() As IDOMINIO

    End Interface

    Public Interface ICARTA

        ' cartografia do qual esta carta representa uma area
        Property Cartografia() As ICARTOGRAFIA

        ' indica a extensão da area abrangida pelos presentes dados cartográficos
        Property AreaAbordada() As IEXTENSÃO_

        ' lista de grupos de layers presentes na extensão abordada pela carta
        Property Grupos() As IGRUPO()

        ' lista de layers presentes na extensão abordada pela carta
        Property Layers() As ILAYER()

        ' lista de todos os objectos presntes na extensão abordada pela carta
        Property Objectos() As IOBJECTO()

        ' propriedades de consulta a grupos, layers e objectos pelos seus identificadores
        ReadOnly Property Grupo(ByVal Nome As String) As IGRUPO
        ReadOnly Property Layer(ByVal Nome As String) As ILAYER
        ReadOnly Property Objecto(ByVal Chave As String) As IOBJECTO

        ReadOnly Property Grupo(ByVal Index As Integer) As IGRUPO
        ReadOnly Property Layer(ByVal Index As Integer) As ILAYER
        ReadOnly Property Objecto(ByVal Index As Integer) As IOBJECTO

        ' metodos para consulta dos grupos, layers e objectos a partir de um array
        ReadOnly Property GruposEmArray() As String()
        ReadOnly Property LayersEmArray() As String()
        ReadOnly Property ObjectosEmArray() As String()

        ' métodos de manutenção da estrutura de conteudos da carta
        Function AdicionaGrupo(ByVal Grupo As IGRUPO) As Boolean
        Function RemoveGrupo(ByVal Grupo As IGRUPO) As Boolean

        ' cria buffer sobre os objectos presentes
        ' se LayerDestino for nulo a execução deve ser reencainhada para o outro Bufferize
        Function Bufferize(ByVal BufferEstilo As IBUFFERSTYLE_, ByVal LayerDestino As ILAYER) As Boolean
        Function Bufferize(ByVal BufferEstilo As IBUFFERSTYLE_) As Boolean

    End Interface

    Public Interface IGRUPO

        ' Cartografia do qual este grupo descende
        Property Cartografia() As ICARTOGRAFIA

        ' lista de layers presntes na extensão abordada pela carta
        Property Layers() As ILAYER()

        ' lista de todos os objectos presntes na extensão abordada pela carta
        Property Objectos() As IOBJECTO()

        ' métodos de consulta a grupos, layers e objectos pelos seus identificadores
        ReadOnly Property Layer(ByVal Nome As String) As ILAYER
        ReadOnly Property Objecto(ByVal Chave As String) As IOBJECTO

        ReadOnly Property Layer(ByVal Index As Integer) As ILAYER
        ReadOnly Property Objecto(ByVal Index As Integer) As IOBJECTO

        ' propriedades para consulta dos layers e objectos a partir de um array
        ReadOnly Property LayersEmArray() As String()
        ReadOnly Property ObjectosEmArray() As String()

        ' indica se o grupo se encontra colapsado e portanto expandido ou não.
        ' "Colapsado" e "Expandido" representam exactamente o contrário uma da outra.
        Property Colapsado() As Boolean
        Property Expandido() As Boolean

        ' nome associado ao grupo (suporte get/set)
        Property Nome() As String

        ' estado de visibilidade na legenda (suporte get/set)
        Property Legendado() As Boolean

        ' legenda associada ao grupo (suporte get/set)
        Property Legenda() As String

        ' estado eventual de visibilidade (suporte get/set)
        Property Visibilidade() As Boolean

        Function AdicionaLayer(ByVal Layer As ILAYER) As Boolean
        Function RemoveLayer(ByVal Layer As ILAYER) As Boolean

    End Interface

    Public Interface ILAYER

        ' grupo ao qual o layer está associado
        Property Grupo() As IGRUPO

        ' lista de todos os objectos pertencentes ao layer
        Property Objectos() As IOBJECTO()

        ' métodos de consulta a grupos, layers e objectos pelos seus identificadores
        ReadOnly Property Objecto(ByVal Chave As String) As IOBJECTO
        ReadOnly Property Objecto(ByVal Index As Integer) As IOBJECTO

        ' lista dos estilos de visualização do layer
        Property Estilos() As ILAYERESTILO()

        ' lista das origens de dados do layer
        Property Origens() As ILAYERORIGEM

        Property Nome() As String
        Property Tipo() As mapLayerTipos
        Property Legendado() As Boolean
        Property Legenda() As String
        Property Prioridade() As Double
        Property Sincronia() As Boolean ' MGV: Rebuild 
        Property Seleccionavel() As Boolean
        Property Estatico() As Boolean
        Property Visibilidade() As Boolean
        Property Visivel() As Boolean

        ' métodos para gestão da colecção de objectos do layer
        Function AdicionaObjecto(ByVal Objecto As IOBJECTO) As Boolean
        Function AdicionaObjecto(ByVal Objecto As IOBJECTO, ByVal Estilo As IOBJSTYLE_) As Boolean
        Function RemoveObjecto(ByVal Objecto As IOBJECTO) As Boolean
        Function RemoveObjecto(ByVal Objecto() As IOBJECTO) As Boolean

        ' propriede para consulta dos objectos a partir de um array
        ReadOnly Property ObjectosEmArray() As String()

        ' propriedades de definição da origem dos dados deste layer

        Property SetupMapServer() As String ' MGV: ServerUrl

    End Interface

    Public Interface IOBJECTO

        ' layer ao qual o objecto pertence
        Property Layer() As ILAYER

        ' extensão abarcada pelo objecto
        Property Extensão() As IEXTENSÃO_

        ' tamanho (comprimento, perimetro) ocupado pelo objecto
        ReadOnly Property Tamanho() As Double

        ' chave associado ao objecto
        Property Chave() As String

        ' link associado ao objecto (suporte get/set)
        Property Link() As String

        ' nome associado ao objecto (suporte get/set)
        Property Nome() As String

        ' tipo que o objecto incorpora
        Property Tipo() As mapObjectoTipos

        ' lista de geometria que compoem o objetco
        Property Geometrias() As IGEOMETRIA()

        ' lista de todos os vertices de todas as geometrias
        ReadOnly Property Vertices() As ICOORDENADA_()

        Function AdicionaVertice(ByVal Vertice As IGEOMETRIA) As Boolean
        Function AdicionaCirculo(ByVal Centro As ICOORDENADA_, ByVal Raio As Double, ByVal Unidade As mapUnidades, ByVal QuantosVertices As Integer) As Boolean
        Function AdicionaSimbolo(ByVal Ponto As ICOORDENADA_) As Boolean
        Function AdicionaTexto(ByVal Ponto As ICOORDENADA_, ByVal Texto As String) As Boolean
        Function AdicionaPolilinha(ByVal Vertices() As ICOORDENADA_) As Boolean
        Function AdicionaPoligono(ByVal Vertices() As ICOORDENADA_) As Boolean

    End Interface

    Public Interface IGEOMETRIA

        ' objecto ao qual a geometria pertence
        Property Objecto() As IOBJECTO

        ' extensão abarcada pelo objecto
        ReadOnly Property Extensão() As IEXTENSÃO_

        ' tamanho (comprimento, perimetro) da geometria
        ReadOnly Property Tamanho() As Double

        ' texto associado à geometria
        Property Texto() As String

        ' lista de todos os vertice da geometria
        ReadOnly Property Vertices() As ICOORDENADA_()

    End Interface

    Public Interface ILAYERESTILO

        ReadOnly Property Layer() As ILAYER

        Property EscalaMinima() As Double
        Property EscalaMaxima() As Double

    End Interface

    Public Interface ILAYERORIGEM

        Property Tipo() As mapLayerOrigens  ' MGV: SourceType
        Property DataSource() As String

        Property ColunaTabela() As String

        Property ColunaChave() As String
        Property ColunaChaveTipo() As System.Data.DbType
        Property ColunaTexto() As String
        Property ColunaLongitude() As String
        Property ColunaLatitude() As String
        Property ColunaAltitude() As String

        Property ColunaSimboloAngulo() As String
        Property ColunaSimboloAltura() As String
        Property ColunaSimboloLargura() As String

        Property ColunaTextoAngulo() As String
        Property ColunaTextoAltura() As String
        Property ColunaTextoAlinhaHor() As mapAlinhamentosHorizontais
        Property ColunaTextoAlinhaVert() As mapAlinhamentosVerticais

        Property Filtro() As String     ' MGV: WhereClause

    End Interface

    '                                                                                           

    Public Interface INAVEGAVEL

        ' métodos de controlo da area visualizada
        Property MapAutoRefresh() As Boolean
        Sub MapRefresh()
        Sub MapStop()

        ' métodos genericos de navegação
        Function ZoomGoto(ByVal Ponto As ICOORDENADA_, ByVal Escala As Double) As Boolean
        Function ZoomGoto(ByVal Area As IEXTENSÃO_) As Boolean
        Function ZoomGoto(ByVal Localização As String, ByVal Parametro As String, ByVal Escala As Double) As Boolean
        Function ZoomMais() As Boolean
        Function ZoomMais(ByVal Factor As Double) As Boolean
        Function ZoomMenos() As Boolean
        Function ZoomMenos(ByVal Factor As Double) As Boolean
        Function ZoomLargura(ByVal Largura As Double) As Boolean
        Function ZoomAltura(ByVal Altura As Double) As Boolean
        Function ZoomEscala(ByVal Escala As Double) As Boolean
        Function Pan(ByVal Sentido As mapSentidos, ByVal Factor As Double) As Boolean
        Function Pan(ByVal Sentido1 As mapSentidos, ByVal Factor1 As Double, ByVal Sentido2 As mapSentidos, ByVal Factor2 As Double) As Boolean
        Function Pan(ByVal Sentido As mapSentidos, ByVal Distancia As Double, ByVal Unidade As mapUnidades) As Boolean
        Function Pan(ByVal Sentido1 As mapSentidos, ByVal Distancia1 As Double, ByVal Sentido2 As mapSentidos, ByVal Distancia2 As Double, ByVal Unidade As mapUnidades) As Boolean

        ' zoom para a vista anterior
        Function ZoomAnterior() As Boolean

        ' realiza o zoom para a area total visivel
        Function ZoomRefresh() As Boolean

        ' zoom para a area que cubra a selecção actual
        Function ZoomSelecção() As Boolean

        ' zoom para a total extensão da cart
        Function ZoomMain() As Boolean

        ' inicia um processo especifico de navegação para o utilizador
        Function UINavega(ByVal Modo As mapNavModos) As Boolean

        ' lança a digitalização de uma determinada forma para o utilizador
        ' o parametro estilo pode ser nulo este serve apenas para ser reencaminhado para o PosDigitalização
        Function UIDigitaliza(ByVal Forma As mapObjectoTipos, ByVal Estilo As IOBJSTYLE_) As Boolean

        ' inicia um processo especifico de selecção para o utilizador
        Function UISelecciona() As Boolean
        Function UISelecciona(ByVal Forma As mapVisSelecções) As Boolean

        ' inicia um processo de medição no visualizador para o utilizador
        Function UIMede(ByVal Unidade As mapUnidades) As Boolean
        Function UIMede(ByVal Unidade As mapUnidades, ByVal Ponto As ICOORDENADA_) As Boolean

        ' selecciona todos os objectos abrangidos por determinado método
        ' "SeleccionaLayers" activa quais os layers que serão alvo do método de selecção
        ' "SeleccionaEmRaio" selecciona todos os objectos dentro de determinado raio de determinado ponto
        ' "SeleccionaEmPoligono" selecciona todos os objectos dentro dos limites de determinado poligono
        Function SeleccionaLayers(ByVal Layers() As ILAYER) As Boolean
        Function SeleccionaEmRaio(ByVal Ponto As ICOORDENADA_, ByVal Raio As Double, ByVal Unidade As mapUnidades) As IOBJECTO()
        Function SeleccionaEmPoligono(ByVal Poligono As IGEOMETRIA) As IOBJECTO()

        ' indica se o navegador está ocupado com operações
        ReadOnly Property Ocupado() As Boolean

        ' estado actual de operação do visualizador
        ReadOnly Property Estado() As mapVisEstados

        ' eventos:
        Event PreNavegação(ByVal Navegado As INAVEGAVEL, ByVal Area As IEXTENSÃO_, ByVal Cancelar As Boolean)
        Event PosNavegação(ByVal Navegado As INAVEGAVEL, ByVal Area As IEXTENSÃO_)
        Event PosSelecção(ByVal Navegado As INAVEGAVEL, ByVal Objectos() As IOBJECTO)
        Event MudouEstado(ByVal Navegado As INAVEGAVEL, ByVal Estado As mapVisEstados)
        Event MudouOcupado(ByVal Navegado As INAVEGAVEL, ByVal Ocupado As Boolean)

    End Interface

    Public Interface IVISUALIZADOR
        Inherits INAVEGAVEL

        Property Cartografia() As ICARTOGRAFIA

        ' indica a extensão da area visivel na area de amostragem
        Property MapArea() As IEXTENSÃO_
        Property Escala() As Double

        ' conteudo cartográfico do visualizador
        ReadOnly Property Conteudo() As ICARTA

        ' conteudo cartográfico presente na area de amostragem
        ReadOnly Property AreaConteudo() As ICARTA

        ' conteudo cartográfico visivel na area de amostragem
        ReadOnly Property AreaVisivel() As ICARTA

        ' conteudo cartográfico seleccionado na area de amostragem
        ReadOnly Property AreaSeleccionada() As ICARTA

        ' modo de selecção a usar
        Property SelecçãoModo() As mapVisSelecções

        ' espaço onde o objecto visualizador irá residir 
        Property Suporte() As System.Windows.Forms.Control

        ' localização e dimensões do objectos visualizador
        Property Localização() As System.Drawing.Point
        Property Dimensão() As System.Drawing.Size
        Property Visivel() As Boolean
        Property Ancoragem() As System.Windows.Forms.AnchorStyles
        Property Acostamento() As System.Windows.Forms.DockStyle

        ' propriedades de controlo do aspecto do visualizador
        Property StatusBarVisivel() As Boolean
        Property LegendaVisivel() As Boolean
        Property LegendaLargura() As Integer
        Property ToolbarVisivel() As Boolean

        ' métodos e propriedades de gestão dos navegadores que controlam este visualizador
        ' o parametro "RegVisual" diz se o visualizador que implementa o método se regista no navegador
        ' que está a registar
        Property Navegadores() As INAVEGADOR()
        Function AdicionaNavegador(ByVal Navegador As INAVEGADOR, ByVal RegVisual As Boolean) As Boolean
        Function RemoveNavegador(ByVal Navegador As INAVEGADOR, ByVal RegVisual As Boolean) As Boolean

        ' eventos:
        Event PreAbertura(ByVal Area As IVISUALIZADOR, ByRef Parametros() As Object)
        Event EmAbertura(ByVal Area As IVISUALIZADOR, ByRef Parametros() As Object)
        Event PosAbertura(ByVal Area As IVISUALIZADOR, ByRef Parametros() As Object)
        Event PosDigitalização(ByVal Area As IVISUALIZADOR, ByVal Forma As mapObjectoTipos, ByVal Objecto As IOBJECTO, ByVal Estilo As IOBJSTYLE_)
        Event EmMapaClick(ByVal Area As IVISUALIZADOR, ByVal Ponto As ICOORDENADA_, ByVal X As Integer, ByVal Y As Integer)
        Event EmObjectoSelecção(ByVal Area As IVISUALIZADOR, ByVal Objecto As IOBJECTO)
        Event EmObjectoEscolha(ByVal Area As IVISUALIZADOR, ByVal Objecto As IOBJECTO)
        Event PosMedição(ByVal Area As IVISUALIZADOR, ByVal Distancia As Double, ByVal Unidade As mapUnidades, ByVal Medidas As IOBJECTO)
        Event PosLegenda(ByVal Navegado As IVISUALIZADOR, ByVal Layer As ILAYER, ByVal Grupo As IGRUPO, ByVal Activação As Boolean)

    End Interface

    Public Interface INAVEGADOR
        Inherits INAVEGAVEL

        ' métodos e propriedades de gestão dos visualizadores utilizados por este navegador
        ' o parametro "RegNaveg" diz se o navegador que implementa o método se regista no visualizador
        ' que está a registar
        Property Visualizadores() As IVISUALIZADOR()
        Function AdicionaVisualizador(ByVal Visualizador As IVISUALIZADOR, ByVal RegNaveg As Boolean) As Boolean
        Function RemoveVisualizador(ByVal Visualizador As IVISUALIZADOR, ByVal RegNaveg As Boolean) As Boolean

    End Interface

    '                                                                                           

    Public Interface IIMPRESSOR

        Property Carta() As ICARTA
        Property Setup() As IIMPRESSORSETUP
        Property Monitor() As IIMPRESSORMONITOR

    End Interface

    Public Interface IIMPRESSORSETUP

        Property Elementos() As IIMPRESSORELEMENTO()

    End Interface

    Public Interface IIMPRESSORELEMENTO

    End Interface

    Public Interface IIMPRESSORMONITOR

    End Interface

    '                                                                                           

    Public Interface ICOORDENADA_
        ' ICOORDENADA_ tem de ser implementada como estrutura!!!

        Property Lat() As Double
        Property Lon() As Double
        Property Alt() As Double
        Property Sistema() As ISISTEMACOORDENADAS

        Overloads Function SemelhanteA(ByVal OutroPonto As ICOORDENADA_) As Boolean

    End Interface

    Public Interface IVECTOR_
        Inherits ICOORDENADA_
        ' IVECTOR_ tem de ser implementada como estrutura!!!

        Property Azimute() As Double
        Property Declinação() As Double
        Property Comprimento() As Double

        Overloads Function SemelhanteA(ByVal OutroVector As IVECTOR_) As Boolean

    End Interface

    Public Interface IEXTENSÃO_
        ' IEXTENSÃO_ tem de ser implementada como estrutura!!!

        Property VerticeSW() As ICOORDENADA_
        Property VerticeNE() As ICOORDENADA_
        ReadOnly Property VerticeCentral() As ICOORDENADA_

        ReadOnly Property Largura() As Double
        ReadOnly Property Altura() As Double
        ReadOnly Property Area() As Double

        ' unidade de medida do sistema de coordenadas usado pela extensão
        Property Unidade() As mapUnidades

        Sub DefExtensão(ByVal Lon As Double, ByVal Lat As Double, ByVal Largura As Double, ByVal Altura As Double)

        Function SemelhanteA(ByVal OutraExtensão As IEXTENSÃO_) As Boolean

    End Interface

    Public Interface IOBJSTYLE_

        Property Margem() As IESTILOMARGEM_
        Property Preenchimento() As IESTILOENCHE_
        Property Linha() As IESTILOLINHA_
        Property Simbolo() As IESTILOSIMBOLO_
        Property Texto() As IESTILOTEXTO_

    End Interface

    Public Interface IBUFFERSTYLE_

        Property Margens() As IESTILOMARGEM_
        Property Preenchimento() As IESTILOENCHE_
        Property Distancia() As Double
        Property Unidade() As mapUnidades
        Property FlagNumObjecto() As Boolean

    End Interface

    Public Interface IESTILOMARGEM_
        ' IMARGEM tem de ser implementada como estrutura!!!

        Property Linha() As IESTILOLINHA_
        Property Visibilidade() As Boolean

    End Interface

    Public Interface IESTILOENCHE_
        ' IPREENCHIMENTO tem de ser implementada como estrutura!!!

        Property Cor() As Integer
        Property CorFundo() As Integer
        Property Modo() As mapEncheModos
        Property Estilo() As mapEncheEstilos

    End Interface

    Public Interface IESTILOLINHA_
        ' ILINHA tem de ser implementada como estrutura!!!

        Property Cor() As Integer
        Property Estilo() As mapLinhaEstilos
        Property Grossura() As Integer

    End Interface

    Public Interface IESTILOSIMBOLO_
        ' ISIMBOLO tem de ser implementada como estrutura!!!

        Property Altura() As Double
        Property Largura() As Double
        Property Rotação() As Double
        Property Localização() As String

    End Interface

    Public Interface IESTILOTEXTO_
        ' ITEXTOFONTE tem de ser implementada como estrutura!!!

        Property Estilo() As IESTILOENCHE_
        Property Nome() As String
        Property Bold() As Boolean
        Property Italic() As Boolean
        Property Underline() As Boolean
        Property StrikeThrough() As Boolean
        Property Alinhamento() As mapAlinhamento
        Property Rotation() As Double

    End Interface

    '                                                                                           

    Public Interface ISISTEMACOORDENADAS

        Sub Define(ByVal p_nome As String, ByVal p_titulo As String, ByVal p_unidade As mapUnidades, ByVal p_projecção As IPROJECÇÃO, ByVal p_tipo As mapTiposSistema, ByVal p_datum As IDATUM, ByVal p_elipsoide As IELIPSOIDE)

        ReadOnly Property Nome() As String
        ReadOnly Property Titulo() As String
        ReadOnly Property Unidade() As mapUnidades
        ReadOnly Property Projecção() As IPROJECÇÃO
        ReadOnly Property Tipo() As mapTiposSistema
        ReadOnly Property Datum() As IDATUM
        ReadOnly Property Elipsoide() As IELIPSOIDE

        ' relação entre a unidade usada pelo sistema e a unidade pedida (numa dada coordenada)
        Function FactorMCS2Un(ByVal Unidade As mapUnidades, ByVal Ponto As IVECTOR_) As Double

        ' converte um dado valor na unidade pedida para a unidade usada pelo sistema (numa dada coordenada)
        Function ConverteUn2MCS(ByVal Valor As Double, ByVal Unidade As mapUnidades, ByVal Ponto As IVECTOR_) As Double

        ' converte um dado valor na unidade usada pelo sistema para a unidade pedida (numa dada coordenada)
        Function ConverteMCS2Un(ByVal Valor As Double, ByVal Unidade As mapUnidades, ByVal Ponto As IVECTOR_) As Double

    End Interface

    Public Interface IDATUM

        Sub Define(ByVal p_nome As String, ByVal p_titulo As String, ByVal p_tipo As mapTiposSistema, ByVal p_elipsoide As IELIPSOIDE)

        ReadOnly Property Nome() As String
        ReadOnly Property Titulo() As String
        ReadOnly Property Tipo() As mapTiposSistema
        ReadOnly Property Elipsoide() As IELIPSOIDE

        Function TransNaturais(ByVal Ponto As ICOORDENADA_)
        Function TransGeodesicas(ByVal Ponto As ICOORDENADA_)
        Function TransBursaWolfe(ByVal Ponto As ICOORDENADA_)
        Function TransMolodensky(ByVal Ponto As ICOORDENADA_, ByVal Simplificado As Boolean)

        ' na evocação de "TransNaturais" 
        '   se o Datum for não geodesico é Geodesicas->Naturais
        '   se o Datum for geodesico é Naturais->Geodesicas
        ' na evocação de "TransGeodesicas"
        '   se o Datum for rectangular é Elipsoidais->Rectangulares
        '   se o Datum for elipsoidal é Rectangulares->Elipsoidais
        ' "TransBursaWolfe" deve ser evocado quando os datum são rectangulares
        ' "TransMolodensky" deve ser evocado quando os datum são elipsoidais

    End Interface

    Public Interface IELIPSOIDE

        ReadOnly Property Nome() As String
        ReadOnly Property Titulo() As String

        Property RaioEquatorial() As Double
        Property RaioPolar() As Double
        Property Achatamento() As Double
        Property Excentricidade1() As Double
        ReadOnly Property Excentricidade2() As Double

        Function CurvaturaMeridiano(ByVal Latitude As Double) As Double
        Function SecçãoNormal(ByVal Latitude As Double) As Double
        Function Paralelo(ByVal Latitude As Double) As Double
        Function CurvaturaTotal(ByVal Latitude As Double) As Double
        Function CurvaturaMédia(ByVal Latitude As Double) As Double

        Function ArcoDeParalelo(ByVal DeLon As Double, ByVal AteLon As Double, ByVal Latitude As Double) As Double
        Function ArcoDeMeridiano(ByVal DeLat As Double, ByVal AteLat As Double) As Double
        Function ReprParametrica(ByVal Ponto As ICOORDENADA_) As Double
        Function CoefGaussianoLat(ByVal Ponto As ICOORDENADA_) As Double
        Function CoefGaussianoLatLon(ByVal Ponto As ICOORDENADA_) As Double
        Function CoefGaussianoLon(ByVal Ponto As ICOORDENADA_) As Double
        Function ElementoLinear(ByVal Paralelo As Double, ByVal Meridiano As Double) As Double
        Function AzimuteGeodesico(ByVal Ponto As ICOORDENADA_) As Double
        Function AreaDoParelelogramo(ByVal Ponto As ICOORDENADA_) As Double

    End Interface

    Public Interface IPROJECÇÃO

        Property Nome() As String
        Property Titulo() As String

        Property DistanciaAMeridiana() As Double
        Property DistanciaAPerpendicular() As Double

        ' parametros de projecção

        Property OrigemLatitude() As Double
        Property OrigemLongitude() As Double

        Property EscalaRedução() As Double
        Property EscalaNorte() As Double
        Property EscalaSul() As Double
        Property EscalaCentral() As Double
        Property EscalaEste() As Double
        Property EscalaOeste() As Double
        Property EscalaStadard() As Double
        Property EscalaNormal() As Double

        Property LongitudePolar() As Double()
        Property LatitudePolar() As Double()
        Property LongitudeObliquaPolar() As Double
        Property LatitudeObliquaPolar() As Double

        Property DistanciaAngularAoParalelo() As Double()
        Property ParaleloObliquoConico() As Double

        Property AzimuteEixoY() As Double
        Property AzimuteEixoX() As Double

        Property ElevaçãoMédia() As Double
        Property ElevaçãoMédiaGeoid() As Double

        Property ComplexoA() As Double()
        Property ComplexoB() As Double()

        Property CirculoLatitude() As Double
        Property CirculoLongitude() As Double
        Property CirculoMaiorLongitude() As Double()
        Property CirculoMaiorLatitude() As Double()
        Property CirculoMaiorAzimute() As Double()

        Property MeridianoCentral() As Double

        Property CoeficienteAfinidadeA() As Double()
        Property CoeficienteAfinidadeB() As Double()

        Property LimiteMargemOeste() As Double
        Property LimiteMargemEste() As Double

        Property ZonaUTM() As Double

        Property Hemisferio() As Double

    End Interface

    '                                                                                           

    ' atributo a usar para marcar a classe referencia de wrappers que implementam esta classe 
    <AttributeUsage(AttributeTargets.All)> Public Class DominioFlag
        Inherits System.Attribute
    End Class

    Public Interface IDOMINIO

        ReadOnly Property Familia() As mapWrappers
        ReadOnly Property Nome() As String
        ReadOnly Property Tecnologia() As String
        ReadOnly Property Copyright() As String
        ReadOnly Property Logo() As System.Drawing.Image

        Function NovaCartografia() As ICARTOGRAFIA
        Function NovaCarta() As ICARTA
        Function NovoGrupo() As IGRUPO
        Function NovoLayer() As ILAYER
        Function NovoObjecto() As IOBJECTO
        Function NovaGeometria() As IGEOMETRIA
        Function NovoLayerEstilo() As ILAYERESTILO
        Function NovoLayerOrigem() As ILAYERORIGEM

        Function NovoVisualizador() As IVISUALIZADOR
        Function NovoNavegador() As INAVEGADOR

        Function NovoImpressor() As IIMPRESSOR
        Function NovoImpressorSetup() As IIMPRESSORSETUP
        Function NovoImpressorElemento() As IIMPRESSORELEMENTO
        Function NovoImpressorMonitor() As IIMPRESSORMONITOR

        Function NovaCoordenada() As ICOORDENADA_
        Function NovaExtensão() As IEXTENSÃO_
        Function NovoEstiloDeObjecto() As IOBJSTYLE_
        Function NovoEstiloDeBuffer() As IBUFFERSTYLE_
        Function NovoEstiloDeMargem() As IESTILOMARGEM_
        Function NovoEstiloDeEnche() As IESTILOENCHE_
        Function NovoEstiloDeLinha() As IESTILOLINHA_
        Function NovoEstiloDeSimbolo() As IESTILOSIMBOLO_
        Function NovoEstiloDeTexto() As IESTILOTEXTO_

        Property Aplicação() As IAPLICAÇÃO

    End Interface

    Public Interface IAPLICAÇÃO

        Sub DoEvents()

    End Interface

End Namespace