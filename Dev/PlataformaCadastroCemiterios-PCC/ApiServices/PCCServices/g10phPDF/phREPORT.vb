Imports O2S.Components.PDF4NET.Forms

Namespace Report

    Public Module Report

        Private ReadOnly _locker As New Object()

        Public Function MeasureDisplayStringWidth(ByVal Frase As String, ByVal Estirador As System.Drawing.Graphics, ByVal Fonte As System.Drawing.Font) As Integer

            ' todo: phUTIL: MeasureDisplayStringWidth
            ' verificar se funciona!

            Dim format As System.Drawing.StringFormat
            Dim rect As New System.Drawing.RectangleF(0, 0, 1000, 1000)
            Dim ranges(0) As System.Drawing.CharacterRange
            Dim regions(0) As System.Drawing.Region

            ranges(0) = New System.Drawing.CharacterRange(0, Frase.Length)
            regions(0) = New System.Drawing.Region

            format.SetMeasurableCharacterRanges(ranges)

            regions = Estirador.MeasureCharacterRanges(Frase, Fonte, rect, format)
            rect = regions(0).GetBounds(Estirador)

            Return CInt(rect.Right + 1.0F)

        End Function

        Public Function Directiva2LDD_Forma(ByVal pagina As String) As PaginaForma

            Select Case pagina.Trim.ToLower
                Case "a0" : Return PaginaForma.A0
                Case "a1" : Return PaginaForma.A1
                Case "a2" : Return PaginaForma.A2
                Case "a3" : Return PaginaForma.A3
                Case "a4" : Return PaginaForma.A4
                Case "a5" : Return PaginaForma.A5

                Case Else : Return PaginaForma.N�oDefinido

            End Select

        End Function

        Public Function Directiva2LDD_Orienta��o(ByVal orienta��o As String) As PaginaOrienta��o

            Select Case orienta��o.Trim.ToLower

                Case "portrait" : Return PaginaOrienta��o.Portrait
                Case "landscape" : Return PaginaOrienta��o.Landscape

                Case Else : Return PaginaOrienta��o.N�oDefinido

            End Select

        End Function

        Public Function ProcuraSubExtens�o(ByVal lista() As IEXTENS�O, ByVal tipo As System.Type, ByRef destino As IEXTENS�O) As Boolean

            destino = Nothing

            If lista Is Nothing Then

                Return False

            Else

                For Each extens�o As IEXTENS�O In lista

                    If extens�o.Type Is tipo Then

                        destino = extens�o
                        Return True

                    End If

                Next

                Return False

            End If

        End Function

    End Module

    Public Enum LDDStandard

        str = 1         '   processada de base [done]
        obj = 2         '   processada de base [done]
        int = 3         '   processada de base [done]

        posx = 11       '   processada de base [done]
        posy = 12       '   processada de base [done]
        tpc = 13        '   processada de base [done]
        pag = 14        '   processada de base [done]
        conc = 15       '   processada de base [done]
        serie = 16      '   processada de base [done]
        print = 17      ' <especifica>
        font = 18       '   processada de base [done]
        size = 19       '   processada de base [done]
        bold = 20       '   processada de base [done]
        italic = 21     '   processada de base [done]
        under = 22      '   processada de base [done]
        strike = 23     '   processada de base [done]
        pen = 24        '   processada de base [done]
        angle = 25      '   processada de base [done]
        align = 26      '   processada de base [done]
        over = 27       '   processada de base [done]
        printbox = 28
        printlines = 29
        poswidth = 30
        posheight = 31

        brush = 41      '   processada de base [done]
        border = 42     '   processada de base [done]
        setx1 = 43      '   processada de base [done]
        sety1 = 44      '   processada de base [done]
        setx2 = 45      '   processada de base [done]
        sety2 = 46      '   processada de base [done]
        line = 47       ' <especifica>
        box = 48        ' <especifica>
        arc = 49        ' <especifica>
        circ = 50       ' <especifica>

        foto = 61       ' <especifica>
        pic = 62        ' <especifica>

        polini = 63     '   processada de base
        polvert = 64    '   processada de base
        polend = 65     '   processada de base
        poldraw = 66    ' <especifica>
        digitalsign = 67     ' <especifica>

    End Enum

    Public Enum PaginaOrienta��o

        N�oDefinido = 0
        Landscape = 1
        Portrait = 2

    End Enum

    Public Enum NumeroPaginaAlinhamento

        TopLeft = 9 'The text is top left aligned  
        TopCenter = 17 ' The text is top center aligned  
        TopRight = 33 ' The text is top right aligned  
        BottomLeft = 12 ' The text is bottom left aligned  
        BottomCenter = 20 ' The text is bottom center aligned  
        BottomRight = 36 ' The text is bottom right aligned  


    End Enum
    Public Enum PaginaForma

        N�oDefinido = 99
        A0 = 0
        A1 = 1
        A2 = 2

        A3 = 3
        A4 = 4
        A5 = 5

    End Enum

    Public Enum ScriptTipo

        Vb_NET = 1
        C_Sharp_NET = 2
        C_Plus_Plus_NET = 3
        Dll_Referencia = 4
        phScript = 5

    End Enum

    Public Enum Quebras

        N�oDefinida = 0

        EntidadeId = 10110
        EntidadeAbreviatura = 10120
        EntidadeNome = 10130
        EntidadeTituloSocial = 10140
        EntidadeTituloProf = 10150
        EntidadeGenero = 10160
        EntidadeNacionalidade = 10170
        EntidadeMoradaPostal = 10180
        EntidadeMoradaPais = 10190

        EntidadeFun��o = 10210
        EntidadeDepartamento = 10220

        EntidadeRelac��o = 10230

    End Enum

    '                                                                                               

    Public Structure LDDInstru��o

        Public Codigo As Integer
        Public Parametro As Object

        Private Shared ReadOnly _locker As Object()

        Public Sub New(ByVal cod As Integer, ByVal par As Object)

            Me.Codigo = cod
            Me.Parametro = par

        End Sub

    End Structure

    Public Structure LDDPagina

        Public Instru��es() As LDDInstru��o
        Public QuantasInstru��es As Integer
        Private Shared ReadOnly _locker As Object()

        Public Function Instru��oAdd(ByVal instru��o As LDDInstru��o) As Integer

            Me.QuantasInstru��es = Me.QuantasInstru��es + 1
            ReDim Preserve Me.Instru��es(Me.QuantasInstru��es - 1)
            Me.Instru��es(Me.QuantasInstru��es - 1) = instru��o

            Return Me.QuantasInstru��es - 1

        End Function

        Public Function Instru��oAdd(ByVal codigo As Integer, ByVal parametro As String) As Integer

            Dim i As New LDDInstru��o(codigo, parametro)

            Return Me.Instru��oAdd(i)

        End Function

    End Structure

    Public Class LDDDocumento

        Public Autor As String
        Public Titulo As String
        Public Assunto As String
        Public Chaves As String

        Public Setup As LDDSetup

        Public Paginas() As LDDPagina
        Public QuantasPaginas As Integer
        Public Mostra_NumeroPagina As Boolean
        Public NumeroPagina_Inicial As Integer
        Public NumeroPagina_Alinhamento As NumeroPaginaAlinhamento
        Public NumeroPagina_OffsetVertical As Integer
        Public NumeroPagina_OffsetHorizontal As Integer
        Public NumeroPagina_FontSize As Integer
        Public NumeroPagina_Prefixo As String
        Public NumeroPagina_Total As Boolean
        Public NumeroPagina_TotalValor As Integer
        Public StackObj As New Collection
        Public StackSeries As New Collection

        Public QuantasSeriesPartes As Integer
        Public Assina_Doc As Boolean
        Public AssinaturaDigital As String
        Public PasswordCert As String



        Public Sub New()

            Me.QuantasSeriesPartes = 1
            Setup.Forma = 99
            NumeroPagina_OffsetVertical = 0
            NumeroPagina_OffsetHorizontal = 0
            NumeroPagina_Inicial = 1
            NumeroPagina_Alinhamento = NumeroPaginaAlinhamento.BottomCenter
            Mostra_NumeroPagina = False
            Assina_Doc = False
        End Sub

        Public Sub New(ByVal p_setup As LDDSetup)

            Me.New()

            Me.Setup = p_setup

        End Sub

        Public Function StackObjAdd(ByVal Chave As String, ByVal ObjectoPonteiro As Object) As Integer

            If Chave Is Nothing OrElse Chave.Length = 0 Then Chave = (Me.StackObj.Count + 1).ToString

            Me.StackObj.Add(ObjectoPonteiro, Chave)
            Return Me.StackObj.Count

        End Function

        Public Function StackSeriesAdd(ByVal Chave As String, ByVal TextoSerie As String) As Integer

            Dim sc As Integer = Chave.Split("~"c).Length + 1
            If sc > Me.QuantasSeriesPartes Then Me.QuantasSeriesPartes = sc

            If Chave Is Nothing OrElse Chave.Length = 0 Then Chave = (Me.StackSeries.Count + 1).ToString

            Me.StackSeries.Add(TextoSerie, Chave)
            Return Me.StackSeries.Count

        End Function

        Public Function Instru��oAdd(ByVal instru��o As LDDInstru��o) As Integer

            Return Me.Paginas(Me.QuantasPaginas - 1).Instru��oAdd(instru��o)

        End Function

        Public Function Instru��oAdd(ByVal Codigo As Integer, ByVal Parametro As String) As Integer

            Dim i As LDDInstru��o
            i.Codigo = Codigo
            i.Parametro = Parametro

            Return Me.Instru��oAdd(i)

        End Function

        Public Function PaginaAdd() As Integer

            If Me.QuantasPaginas > 0 Then RaiseEvent PaginaEnds(Me)

            Me.QuantasPaginas = Me.QuantasPaginas + 1
            ReDim Preserve Me.Paginas(Me.QuantasPaginas - 1)

            If Me.QuantasPaginas = 1 Then
                RaiseEvent DocumentoPrepara(Me)
                RaiseEvent DocumentoAbre(Me)
            End If

            RaiseEvent PaginaPrepara(Me)
            RaiseEvent PaginaBegins(Me)

            Return Me.QuantasPaginas - 1

        End Function

        Public Function PaginaAdd(ByVal pagina As LDDPagina) As Integer

            Dim pi As Integer = Me.PaginaAdd
            Me.Paginas(pi) = pagina

        End Function

        Public Sub EncerraDocumento()

            If Me.QuantasPaginas > 0 Then RaiseEvent PaginaEnds(Me)
            RaiseEvent DocumentoEncerra(Me)

        End Sub

        Public Event DocumentoPrepara(ByVal Documento As LDDDocumento)
        Public Event DocumentoAbre(ByVal Documento As LDDDocumento)
        Public Event DocumentoEncerra(ByVal Documento As LDDDocumento)
        Public Event PaginaPrepara(ByVal Documento As LDDDocumento)
        Public Event PaginaBegins(ByVal Documento As LDDDocumento)
        Public Event PaginaEnds(ByVal Documento As LDDDocumento)

    End Class

    Public Structure LDDSetup

        Public Orienta��o As PaginaOrienta��o
        Public Forma As PaginaForma
        Public Dimens�o As System.Drawing.Size
        Public Margem As System.Drawing.Rectangle

    End Structure

    Public Interface ILDDPARSER

        Property Documento() As LDDDocumento

        Property Paginador() As LDDPaginador

    End Interface

    Public Class LDDPaginador

        ' documento sobre o qual est� a ser realizada a pagina��o
        Public Documento As LDDDocumento

        ' a direc��o determina a orienta��o de coloca��o das quebras
        Public Direc��o As PaginaOrienta��o

        ' informa��es especiais por quebra (identifica��o, dimens�o) determinantes para a pagina��o
        Public Quebras() As LDDQuebra

        ' dimens�es do cabe�alho e do rodap� de p�gina
        Public Cabe�alhoDim As System.Drawing.Size
        Public RodapeDim As System.Drawing.Size

        ' informa��es sobre a ultima quebra lan�ada
        ' esta informa��o deve ser usada pelos parser para determinar as coordenadas referencia
        Public QuebraActual As System.Drawing.Point
        Public QuebraActualDim As System.Drawing.Size



        ' construtor da pagina��o
        Public Sub New(ByVal p_documento As LDDDocumento)

            Me.Documento = p_documento

        End Sub

        ' adiciona uma quebra 
        Public Sub QuebraAdd(ByVal Id As Integer)

            Dim quebra As LDDQuebra = Me.QuebraGet(Id)

            If quebra.Id = 0 Then

                If Me.Quebras Is Nothing Then
                    ReDim Me.Quebras(0)
                Else
                    ReDim Preserve Me.Quebras(Me.Quebras.GetUpperBound(0) + 1)
                End If

                Me.Quebras(Me.Quebras.GetUpperBound(0)).Id = Id

            End If

        End Sub

        Public Sub QuebraAdd(ByVal Id As Integer, ByVal Dimens�o As System.Drawing.Size)

            Dim quebra As LDDQuebra = Me.QuebraGet(Id)

            If quebra.Id = 0 Then

                If Me.Quebras Is Nothing Then
                    ReDim Me.Quebras(0)
                Else
                    ReDim Preserve Me.Quebras(Me.Quebras.GetUpperBound(0) + 1)
                End If

                Me.Quebras(Me.Quebras.GetUpperBound(0)).Id = Id
                Me.Quebras(Me.Quebras.GetUpperBound(0)).Dimens�o = Dimens�o

            End If

        End Sub

        Public Sub QuebraAdd(ByVal Id As Integer, ByVal Largura As Integer, ByVal Altura As Integer)

            Dim quebra As LDDQuebra = Me.QuebraGet(Id)

            If quebra.Id = 0 Then

                If Me.Quebras Is Nothing Then
                    ReDim Me.Quebras(0)
                Else
                    ReDim Preserve Me.Quebras(Me.Quebras.GetUpperBound(0) + 1)
                End If

                Me.Quebras(Me.Quebras.GetUpperBound(0)).Id = Id
                Me.Quebras(Me.Quebras.GetUpperBound(0)).Dimens�o = New System.Drawing.Size(Largura, Altura)

            End If

        End Sub

        Public Sub QuebraAdd(ByVal Id As Integer, ByVal Altura As Integer)

            Dim quebra As LDDQuebra = Me.QuebraGet(Id)

            If quebra.Id = 0 Then

                If Me.Quebras Is Nothing Then
                    ReDim Me.Quebras(0)
                Else
                    ReDim Preserve Me.Quebras(Me.Quebras.GetUpperBound(0) + 1)
                End If

                Me.Quebras(Me.Quebras.GetUpperBound(0)).Id = Id
                Me.Quebras(Me.Quebras.GetUpperBound(0)).Dimens�o = New System.Drawing.Size(0, Altura)

            End If

        End Sub

        ' retorna o indice da quebra com o id indicado
        Public Function QuebraGet(ByVal Id As Integer) As LDDQuebra

            If Me.Quebras Is Nothing Then
                Return New LDDQuebra
            Else
                For Each quebra As LDDQuebra In Me.Quebras
                    If quebra.Id = Id Then Return quebra
                Next
                Return New LDDQuebra
            End If

        End Function

        ' defini��o das dimens�es de uma quebra
        Public Sub QuebraDef(ByVal Id As Integer, ByVal Largura As Integer, ByVal Altura As Integer)

            Me.QuebraDef(Id, New System.Drawing.Size(Largura, Altura))

        End Sub

        Public Sub QuebraDef(ByVal Id As Integer, ByVal Altura As Integer)

            Me.QuebraDef(Id, New System.Drawing.Size(0, Altura))

        End Sub

        Public Sub QuebraDef(ByVal Id As Integer, ByVal Dimens�o As System.Drawing.Size)

            Dim quebra As LDDQuebra = Me.QuebraGet(Id)

            If quebra.Id = Id Then quebra.Dimens�o = Dimens�o

        End Sub

        ' prepara coordenadas para lan�amento de uma quebra
        Public Sub PaginateQuebra(ByVal Id As Integer)

            ' todo: paginador: entrar com as margens e a orienta��o de pagina��o

            Dim novo_y As Integer
            Dim pag_height As Integer
            Dim nova_quebra As LDDQuebra = Me.QuebraGet(Id)

            pag_height = Me.Documento.Setup.Dimens�o.Height - Me.RodapeDim.Height

            novo_y = Me.QuebraActual.Y + Me.QuebraActualDim.Height

            If novo_y + nova_quebra.Dimens�o.Height > pag_height Then
                Me.Documento.PaginaAdd()
                novo_y = 0
            End If

            Me.QuebraActual.X = 0
            Me.QuebraActual.Y = novo_y
            Me.QuebraActualDim.Width = nova_quebra.Dimens�o.Width
            Me.QuebraActualDim.Height = nova_quebra.Dimens�o.Height

        End Sub

        Public Sub PaginateQuebra()

            Dim novo_y As Integer

            Me.QuebraActual.X = 0
            Me.QuebraActual.Y = Me.QuebraActual.Y + Me.QuebraActualDim.Height
            Me.QuebraActualDim.Width = Me.Documento.Setup.Dimens�o.Width
            Me.QuebraActualDim.Height = Me.Documento.Setup.Dimens�o.Height - (Me.Cabe�alhoDim.Height + Me.RodapeDim.Height)

        End Sub

        ' prepara coordenadas para lan�amento do cabe�alho da p�gina
        Public Sub PaginateCab()

            ' todo: paginador: entrar com as margens de pagina��o

            QuebraActual.X = 0
            QuebraActual.Y = 0
            QuebraActualDim = Cabe�alhoDim

        End Sub

        ' prepara coordenadas para lan�amento do rodap� da p�gina
        Public Sub PaginateRod()

            ' todo: paginador: entrar com as margens de pagina��o

            QuebraActual.X = 0
            QuebraActual.Y = Me.Documento.Setup.Dimens�o.Height - RodapeDim.Height
            QuebraActualDim = RodapeDim

        End Sub

    End Class

    Public Structure LDDQuebra

        Public Id As Integer
        Public Dimens�o As System.Drawing.Size

    End Structure

    Public Interface ILDDPROC

        Sub Reset()

        Property Processo() As LDDProcesso

        Function RealizaDocumento() As Boolean
        Function RealizaDocumento(ByVal p_processo As LDDProcesso) As Boolean

        Function RealizaSerie() As Boolean
        Function RealizaSerie(ByVal SerieIndex As Integer) As Boolean
        Function RealizaSerie(ByVal p_processo As LDDProcesso) As Boolean

        Function RealizaPagina() As Boolean
        Function RealizaPagina(ByVal SerieIndex As Integer, ByVal PaginaIndex As Integer) As Boolean
        Function RealizaPagina(ByVal p_processo As LDDProcesso) As Boolean
        Function RealizaPagina(ByVal p_pagina As LDDPagina) As Boolean

        Function RealizaInstru��o() As Boolean
        Function RealizaInstru��o(ByVal SerieIndex As Integer, ByVal PaginaIndex As Integer, ByVal Instru��oIndex As Integer) As Boolean
        Function RealizaInstru��o(ByVal p_processo As LDDProcesso) As Boolean
        Function RealizaInstru��o(ByVal p_pagina As LDDPagina) As Boolean
        Function RealizaInstru��o(ByVal p_instru��o As LDDInstru��o) As Boolean

    End Interface

    Public Class LDDProcesso

        Public Documento As LDDDocumento
        Public SerieParteActual As Integer
        Public PaginaActual As Integer
        Public Instru��oActual As Integer

        Public objecto As Object
        Public str As String
        Public inteiro As Integer
        Public posx As Integer
        Public posy As Integer
        Public poswidth As Integer
        Public posheight As Integer
        Public x1 As Integer
        Public y1 As Integer
        Public x2 As Integer
        Public y2 As Integer
        Public fonte_nome As String
        Public fonte_tamanho As Integer
        Public fonte_bold As Boolean
        Public fonte_italic As Boolean
        Public fonte_underline As Boolean
        Public fonte_strike As Boolean
        Public fonte_overline As Boolean
        Public fonte_cor As Integer
        Public fonte_angulo As Integer
        Public fonte_alinhamento As Integer
        Public tinta As Integer
        Public grossura As Integer
        Public pontos() As System.Drawing.Point



        Public Sub New(ByVal doc As LDDDocumento)

            Me.Documento = doc

        End Sub

    End Class

    '                                                                                               

    Public Interface IDIRECTOR

        Property Dicionario() As String()
        Property ClienteDB() As g10phPDF4.Setframe.setClienteDb

        Property Motor() As IMOTOR

        Function Realiza(ByVal Motor As IMOTOR, ByVal Modelo As IMODELO) As Boolean
        Function Realiza(ByVal Script As String) As Boolean

        Function GetModelo(ByVal ModeloCodigo As String) As IMODELO
        Function GetMotor(ByVal MotorCodigo As String) As IMOTOR

        Event PreModelo(ByVal Director As IDIRECTOR, ByVal Motor As IMOTOR, ByVal Modelo As IMODELO)
        Event PosModelo(ByVal Director As IDIRECTOR, ByVal Motor As IMOTOR, ByVal Modelo As IMODELO)

        Event PreForm(ByVal Director As IDIRECTOR, ByVal Motor As IMOTOR, ByVal Formulario As Object)
        Event PosForm(ByVal Director As IDIRECTOR, ByVal Motor As IMOTOR, ByVal Formulario As Object)

    End Interface

    Public Interface IMOTOR

        ' titulo e descri��o do motor
        ReadOnly Property Titulo() As String
        ReadOnly Property Descri��o() As String

        ' modelo pedido pelo motor
        ReadOnly Property ModeloRequisito() As System.Type

        ' tipo das extens�es pedidas pelo motor
        ReadOnly Property Extens�esRequisito() As System.Type()

        ' painel de configura��o do motor de dados
        ReadOnly Property ConfigPanel() As Object

        ' lan�a a execu��o do motor de dados sobre um dado modelo de apresenta��o
        Function Executa(ByVal Modelo As IMODELO, ByVal Dicionario() As String, ByVal ClienteDB As g10phPDF4.Setframe.setClienteDb) As Boolean

    End Interface

    <AttributeUsage(AttributeTargets.All)> Public Class ModeloFlag
        Inherits System.Attribute
        ' atributo a usar para marcar as classes a lan�ar como modelos de relat�rio
        Public titulo As String
    End Class

    <AttributeUsage(AttributeTargets.All)> Public Class Extens�oFlag
        Inherits System.Attribute
        ' atributo a usar para marcar as classes a lan�ar como modelos de relat�rio
    End Class

    Public Interface IMODELO

        ' titulo e descri��o do motor
        ReadOnly Property Titulo() As String
        ReadOnly Property Descri��o() As String

        ' extens�es de relat�rio disponibilzadas pelo modelo
        ReadOnly Property Extens�es() As IEXTENS�O()

        ' esquema de quebras exigido pelo modelo
        ReadOnly Property Sequencia() As Integer()

        ' configura��o pr�via ao processo de relat�rio
        Function PreConfig(ByVal Motor As IMOTOR, ByVal Dicionario As String(), ByVal ClienteDB As g10phPDF4.Setframe.setClienteDb) As Boolean

        ' mudan�a de p�gina no documento
        Function PaginaAdd() As Boolean

        ' tipo pr�prio do modelo quando acedido por vari�vel do tipo interface
        Function Type() As System.Type

        ' controla o processo de pagina��o do documento
        Property Paginador() As LDDPaginador

        ' representa o documento a construir pelo modelo
        Property Documento() As LDDDocumento

    End Interface

    Public Interface IEXTENS�O

        ReadOnly Property Extens�es() As IEXTENS�O()    ' overridable

        Sub TestaQuebra(ByVal quebra As Integer, ByVal paginador As LDDPaginador)         ' overridable

        Sub FimDeRegisto()                              ' overridable

        Function Type() As System.Type                  ' overridable

    End Interface

    '                                                                                               

    Public MustInherit Class DirectorBase
        Implements IDIRECTOR

        Protected l_clientedb As g10phPDF4.Setframe.setClienteDb
        Protected l_dicionario() As String
        Protected l_motor As IMOTOR



        Public Property ClienteDB() As g10phPDF4.Setframe.setClienteDb Implements IDIRECTOR.ClienteDB
            Get

                Return l_clientedb

            End Get
            Set(ByVal Value As g10phPDF4.Setframe.setClienteDb)

                l_clientedb = Value

            End Set
        End Property

        Public Property Dicionario() As String() Implements IDIRECTOR.Dicionario
            Get

                Return l_dicionario

            End Get
            Set(ByVal Value() As String)

                l_dicionario = Value

            End Set
        End Property

        Public Property Motor() As IMOTOR Implements IDIRECTOR.Motor
            Get

                Return l_motor

            End Get
            Set(ByVal Value As IMOTOR)

                l_motor = Value

            End Set
        End Property

        Public Overloads Function Realiza(ByVal Motor As IMOTOR, ByVal Modelo As IMODELO) As Boolean Implements IDIRECTOR.Realiza

            RaiseEvent PreModelo(Me, Motor, Modelo)

            Realiza = Motor.Executa(Modelo, Dicionario, ClienteDB)

            RaiseEvent PosModelo(Me, Motor, Modelo)

        End Function

        Public Overloads Function Realiza(ByVal Script As String) As Boolean Implements IDIRECTOR.Realiza

            Dim lista_de_directivas() As String = Script.Split("#"c)
            Dim com As String
            Dim par As String

            Dim setup As LDDSetup
            Dim p_motor As IMOTOR

            For Each directiva As String In lista_de_directivas

                If directiva.Trim.Length > 0 Then

                    com = g10phPDF4.Basframe.Strings.Corta1�Palavra(directiva)
                    par = g10phPDF4.Basframe.Strings.Sem1�Palavra(directiva)

                    Select Case com.ToLower

                        Case "pagina" : setup.Forma = Directiva2LDD_Forma(par)
                        Case "orienta��o" : setup.Orienta��o = Directiva2LDD_Orienta��o(par)

                        Case "motor"
                            p_motor = GetMotor(par.Trim)

                        Case "modelo"
                            If p_motor Is Nothing Then p_motor = l_motor
                            Dim modelo As IMODELO = GetModelo(par.Trim)
                            modelo.Documento = New LDDDocumento(setup)
                            modelo.Paginador = New LDDPaginador(modelo.Documento)
                            Me.Realiza(p_motor, modelo)

                    End Select

                End If

            Next

        End Function

        Public MustOverride Function GetModelo(ByVal ModeloCodigo As String) As IMODELO Implements IDIRECTOR.GetModelo

        Public MustOverride Function GetMotor(ByVal MotorCodigo As String) As IMOTOR Implements IDIRECTOR.GetMotor

        Public Event PreModelo(ByVal Director As IDIRECTOR, ByVal Motor As IMOTOR, ByVal Modelo As IMODELO) Implements IDIRECTOR.PreModelo
        Public Event PosModelo(ByVal Director As IDIRECTOR, ByVal Motor As IMOTOR, ByVal Modelo As IMODELO) Implements IDIRECTOR.PosModelo

        Public Event PreForm(ByVal Director As IDIRECTOR, ByVal Motor As IMOTOR, ByVal Formulario As Object) Implements IDIRECTOR.PreForm
        Public Event PosForm(ByVal Director As IDIRECTOR, ByVal Motor As IMOTOR, ByVal Formulario As Object) Implements IDIRECTOR.PosForm

    End Class

    Public MustInherit Class MotorBase
        Implements IMOTOR



        Protected l_modelo As IMODELO

        Public MustOverride ReadOnly Property Titulo() As String Implements IMOTOR.Titulo

        Public MustOverride ReadOnly Property Descri��o() As String Implements IMOTOR.Descri��o

        Public MustOverride ReadOnly Property ModeloRequisito() As System.Type Implements IMOTOR.ModeloRequisito

        Public MustOverride ReadOnly Property Extens�esRequisito() As System.Type() Implements IMOTOR.Extens�esRequisito

        Public Overridable ReadOnly Property ConfigPanel() As Object Implements IMOTOR.ConfigPanel
            Get

                Return Nothing

            End Get
        End Property

        Public Function Executa(ByVal Modelo As IMODELO, ByVal Dicionario() As String, ByVal ClienteDB As g10phPDF4.Setframe.setClienteDb) As Boolean Implements IMOTOR.Executa

            If Me.ModeloRequisito Is Modelo.Type Then

                Me.l_modelo = Modelo

                For Each extens�o As IEXTENS�O In Modelo.Extens�es

                    RaiseEvent MaisUmaExtens�o(extens�o)

                Next

                l_modelo.PreConfig(Me, Dicionario, ClienteDB)

                l_modelo.PaginaAdd()

                RaiseEvent MomentoReport()

                l_modelo.Documento.EncerraDocumento()

            End If

        End Function

        Public Event MaisUmaExtens�o(ByVal extens�o As IEXTENS�O)

        Public Event MomentoReport()

        Public Event FimDeRegisto()

        Protected Sub TestaQuebras(ByVal ParamArray extens�es() As IEXTENS�O)

            TestaQuebraPr�(extens�es)
            TestaQuebraP�s(extens�es)

            RaiseEvent FimDeRegisto()

        End Sub

        Protected Sub TestaQuebraPr�(ByVal ParamArray extens�es() As IEXTENS�O)

            For Each extens�o As IEXTENS�O In extens�es

                If Me.l_modelo.Sequencia Is Nothing Then

                    extens�o.TestaQuebra(Quebras.N�oDefinida, l_modelo.Paginador)
                    If Not extens�o.Extens�es Is Nothing Then Me.TestaQuebraPr�(extens�o.Extens�es)

                Else

                    For Each quebra As Integer In Me.l_modelo.Sequencia

                        extens�o.TestaQuebra(quebra, l_modelo.Paginador)
                        If Not extens�o.Extens�es Is Nothing Then Me.TestaQuebraPr�(extens�o.Extens�es)

                    Next

                End If

            Next

        End Sub

        Protected Sub TestaQuebraP�s(ByVal ParamArray extens�es() As IEXTENS�O)

            For Each extens�o As IEXTENS�O In extens�es

                extens�o.FimDeRegisto()
                If Not extens�o.Extens�es Is Nothing Then Me.TestaQuebraP�s(extens�o.Extens�es)

            Next

        End Sub

    End Class

    Public MustInherit Class ModeloBase
        Implements IMODELO

        Protected l_documento As LDDDocumento
        Protected l_paginador As LDDPaginador



        Public Sub New()

            Me.Documento = New LDDDocumento
            Me.Paginador = New LDDPaginador(Me.Documento)

            Me.Documento.Autor = ""
            Me.Documento.Titulo = Me.Titulo
            Me.Documento.Assunto = Me.Descri��o
            Me.Documento.Chaves = ""

        End Sub

        Public MustOverride ReadOnly Property Titulo() As String Implements IMODELO.Titulo

        Public MustOverride ReadOnly Property Descri��o() As String Implements IMODELO.Descri��o

        Public Overridable ReadOnly Property Sequencia() As Integer() Implements IMODELO.Sequencia
            Get

                Return Nothing

            End Get
        End Property

        Public MustOverride ReadOnly Property Extens�es() As IEXTENS�O() Implements IMODELO.Extens�es

        Public Property Documento() As LDDDocumento Implements IMODELO.Documento
            Get

                Return l_documento

            End Get
            Set(ByVal Value As LDDDocumento)

                l_documento = Value
                AddHandler l_documento.PaginaBegins, AddressOf ApanhaInicioDePagina
                AddHandler l_documento.PaginaEnds, AddressOf ApanhaFimDePagina
                AddHandler l_documento.DocumentoAbre, AddressOf ApanhaInicioDeDocumento
                AddHandler l_documento.DocumentoEncerra, AddressOf ApanhaFimDeDocumento

            End Set
        End Property

        Public Property Paginador() As LDDPaginador Implements IMODELO.Paginador
            Get

                Return l_paginador

            End Get
            Set(ByVal Value As LDDPaginador)

                l_paginador = Value

            End Set
        End Property

        Public Overridable Function PreConfig(ByVal Motor As IMOTOR, ByVal Dicionario() As String, ByVal ClienteDB As g10phPDF4.Setframe.setClienteDb) As Boolean Implements IMODELO.PreConfig

        End Function

        Public Overridable Function PaginaAdd() As Boolean Implements IMODELO.PaginaAdd

            Me.Documento.PaginaAdd()

        End Function

        Public Overridable Function Type() As System.Type Implements IMODELO.Type

            Return Me.GetType

        End Function

        Private Sub ApanhaInicioDePagina(ByVal Documento As LDDDocumento)

            If l_paginador.Cabe�alhoDim.Height > 0 Then

                l_paginador.PaginateCab()
                RaiseEvent PaginaCab()

            End If

        End Sub

        Private Sub ApanhaFimDePagina(ByVal Documento As LDDDocumento)

            If Me.Documento.QuantasPaginas > 0 Then

                If l_paginador.RodapeDim.Height > 0 Then

                    l_paginador.PaginateRod()
                    RaiseEvent PaginaRod()

                End If

            End If

        End Sub

        Private Sub ApanhaInicioDeDocumento(ByVal Documento As LDDDocumento)

            RaiseEvent DocumentoCab()

        End Sub

        Private Sub ApanhaFimDeDocumento(ByVal Documento As LDDDocumento)

            RaiseEvent DocumentoRod()

        End Sub

        Public Event DocumentoCab()
        Public Event DocumentoRod()
        Public Event PaginaCab()
        Public Event PaginaRod()

    End Class

    Public MustInherit Class Extens�oEntidade
        Implements IEXTENS�O



        Public Overridable ReadOnly Property Extens�es() As IEXTENS�O() Implements IEXTENS�O.Extens�es
            Get

                Return Nothing

            End Get
        End Property

        Public Structure EntidadeDef

            Public Id As Integer
            Public Abreviatura As String
            Public Nome As String

            Public TituloSocial As String
            Public TituloProf As String

            Public DataNasc As Date
            Public Genero As String
            Public Nacionalidade As String

            Public Morada As String
            Public MoradaPostal As String
            Public MoradaPais As String

            Public Imagem As System.Drawing.Image

            Public Telefone As String
            Public Fax As String
            Public TM�vel As String
            Public Pager As String
            Public EMail As String
            Public ISite As String

        End Structure

        Public __Entidade As EntidadeDef
        Public Entidade As EntidadeDef

        Public Overridable Sub TestaQuebra(ByVal quebra As Integer, ByVal Paginador As LDDPaginador) Implements IEXTENS�O.TestaQuebra

            Select Case quebra

                Case Quebras.EntidadeId
                    If __Entidade.Id <> Entidade.Id Then
                        Paginador.PaginateQuebra(quebra)
                        RaiseEvent QuebraEntidade()
                    End If

                Case Quebras.EntidadeTituloProf
                    If __Entidade.TituloProf <> Entidade.TituloProf Then
                        Paginador.PaginateQuebra(quebra)
                        RaiseEvent QuebraTituloProf()
                    End If

                Case Quebras.EntidadeTituloSocial
                    If __Entidade.TituloSocial <> Entidade.TituloSocial Then
                        Paginador.PaginateQuebra(quebra)
                        RaiseEvent QuebraTituloSocial()
                    End If

                Case Quebras.EntidadeGenero
                    If __Entidade.Genero <> Entidade.Genero Then
                        Paginador.PaginateQuebra(quebra)
                        RaiseEvent QuebraGenero()
                    End If

                Case Quebras.EntidadeNacionalidade
                    If __Entidade.Nacionalidade <> Entidade.Nacionalidade Then
                        Paginador.PaginateQuebra(quebra)
                        RaiseEvent QuebraNacionalidade()
                    End If

                Case Quebras.EntidadeMoradaPostal
                    If __Entidade.MoradaPostal <> Entidade.MoradaPostal Then
                        Paginador.PaginateQuebra(quebra)
                        RaiseEvent QuebraMoradaPostal()
                    End If

                Case Quebras.EntidadeMoradaPais
                    If __Entidade.MoradaPais <> Entidade.MoradaPais Then
                        Paginador.PaginateQuebra(quebra)
                        RaiseEvent QuebraMoradaPais()
                    End If

            End Select

        End Sub

        Public Overridable Sub FimDeRegisto() Implements IEXTENS�O.FimDeRegisto

            __Entidade = Entidade

        End Sub

        Public Overridable Function Type() As System.Type Implements IEXTENS�O.Type

            Return Me.Type

        End Function

        Public Event QuebraEntidade()
        Public Event QuebraTituloProf()
        Public Event QuebraTituloSocial()
        Public Event QuebraGenero()
        Public Event QuebraNacionalidade()
        Public Event QuebraMoradaPostal()
        Public Event QuebraMoradaPais()

    End Class

    Public MustInherit Class Extens�oEntidadeFuncional
        Inherits Extens�oEntidade



        Public Structure Fun��oDef

            Public Titulo As String
            Public Departamento As String

        End Structure

        Public __Fun��o As Fun��oDef
        Public Fun��o As Fun��oDef

        Public Overrides Sub TestaQuebra(ByVal quebra As Integer, ByVal Paginador As LDDPaginador)

            MyBase.TestaQuebra(quebra, Paginador)

            Select Case quebra

                Case Quebras.EntidadeFun��o
                    If __Fun��o.Titulo <> Fun��o.Titulo Then
                        Paginador.PaginateQuebra(quebra)
                        RaiseEvent QuebraFun��o()
                    End If

                Case Quebras.EntidadeDepartamento
                    If __Fun��o.Departamento <> Fun��o.Departamento Then
                        Paginador.PaginateQuebra(quebra)
                        RaiseEvent QuebraDepartamento()
                    End If

            End Select

        End Sub

        Public Event QuebraFun��o()
        Public Event QuebraDepartamento()

        Public Overrides Function Type() As System.Type

            Return Me.Type

        End Function

    End Class

    Public MustInherit Class Extens�oEntidadeRelacionada
        Inherits Extens�oEntidade



        Public Structure Relac��oDef

            Public Relac��oDirecta As String
            Public Relac��oInversa As String

        End Structure

        Public __Relac��o As Relac��oDef
        Public Relac��o As Relac��oDef

        Public Overrides Sub TestaQuebra(ByVal quebra As Integer, ByVal Paginador As LDDPaginador)

            MyBase.TestaQuebra(quebra, Paginador)

            Select Case quebra

                Case Quebras.EntidadeFun��o
                    If __Relac��o.Relac��oDirecta <> Relac��o.Relac��oDirecta Then
                        Paginador.PaginateQuebra(quebra)
                        RaiseEvent QuebraRelac��o()
                    End If

            End Select

        End Sub

        Public Event QuebraRelac��o()

        Public Overrides Function Type() As System.Type

            Return Me.Type

        End Function

    End Class

    '                                                                                               

    Public Class ParserLDD2Doc
        Implements ILDDPARSER



        Private l_documento As LDDDocumento
        Private l_paginador As LDDPaginador

        Public Property Documento() As LDDDocumento Implements ILDDPARSER.Documento
            Get

                Return l_documento

            End Get
            Set(ByVal Value As LDDDocumento)

                l_documento = Value
                AddHandler l_documento.DocumentoPrepara, AddressOf ApanhaNovaPagina
                AddHandler l_documento.PaginaPrepara, AddressOf ApanhaNovaPagina

            End Set
        End Property

        Public Property Paginador() As LDDPaginador Implements ILDDPARSER.Paginador
            Get

                Return l_paginador

            End Get
            Set(ByVal Value As LDDPaginador)

                l_paginador = Value


            End Set
        End Property

        Public Sub New(ByVal p_documento As LDDDocumento, ByVal p_paginador As LDDPaginador)

            W = New Windows.Forms.Form
            A = W.CreateGraphics

            l_FonteNome = "times new roman"
            l_FonteTamanho = 8
            l_FonteCor = System.Drawing.Color.FromKnownColor(Drawing.KnownColor.Black)
            l_FonteAlinhamento = Windows.Forms.HorizontalAlignment.Right
            l_Tinta = System.Drawing.Color.FromKnownColor(Drawing.KnownColor.Black)
            l_Grossura = 1

            Me.Documento = p_documento
            Me.Paginador = p_paginador

        End Sub

        Private Sub ApanhaNovaPagina(ByVal Documento As LDDDocumento)

            Me.FonteNome = l_FonteNome
            Me.FonteTamanho = l_FonteTamanho
            Me.FonteCor = l_FonteCor
            Me.FonteBold = l_FonteBold
            Me.FonteItalic = l_FonteItalic
            Me.FonteStrike = l_FonteStrike
            Me.FonteUnderline = l_FonteUnderline
            Me.FonteOverline = l_FonteOverline
            Me.FonteAlinhamento = l_FonteAlinhamento
            Me.FonteAngulo = l_FonteAngulo
            Me.Tinta = l_Tinta
            Me.Grossura = l_Grossura

        End Sub

        Private l_FonteNome As String
        Private l_FonteTamanho As Integer
        Private l_FonteBold As Boolean
        Private l_FonteItalic As Boolean
        Private l_FonteUnderline As Boolean
        Private l_FonteStrike As Boolean
        Private l_FonteOverline As Boolean
        Private l_FonteCor As System.Drawing.Color
        Private l_FonteAlinhamento As System.Windows.Forms.HorizontalAlignment
        Private l_FonteAngulo As Integer
        Private l_Tinta As System.Drawing.Color
        Private l_Grossura As Integer

        Private W As Windows.Forms.Form
        Private A As System.Drawing.Graphics


        Public Sub NovaPagina()

            l_documento.PaginaAdd()


        End Sub

        Public Property FonteNome() As String
            Get

                Return l_FonteNome

            End Get
            Set(ByVal Value As String)

                l_FonteNome = Value
                l_documento.Instru��oAdd(LDDStandard.font, l_FonteNome)

            End Set
        End Property

        Public Property FonteTamanho() As Integer
            Get

                Return l_FonteTamanho

            End Get
            Set(ByVal Value As Integer)

                l_FonteTamanho = Value
                l_documento.Instru��oAdd(LDDStandard.size, l_FonteTamanho.ToString)

            End Set
        End Property

        Public Property FonteCor() As System.Drawing.Color
            Get

                Return l_FonteCor

            End Get
            Set(ByVal Value As System.Drawing.Color)

                l_FonteCor = Value
                l_documento.Instru��oAdd(LDDStandard.pen, l_FonteCor.ToArgb.ToString)

            End Set
        End Property

        Public Property FonteBold() As Boolean
            Get

                Return l_FonteBold

            End Get
            Set(ByVal Value As Boolean)

                l_FonteBold = Value
                l_documento.Instru��oAdd(LDDStandard.bold, l_FonteBold.ToString)

            End Set
        End Property

        Public Property FonteItalic() As Boolean
            Get

                Return l_FonteItalic

            End Get
            Set(ByVal Value As Boolean)

                l_FonteItalic = Value
                l_documento.Instru��oAdd(LDDStandard.italic, l_FonteItalic.ToString)

            End Set
        End Property

        Public Property FonteStrike() As Boolean
            Get

                Return l_FonteStrike

            End Get
            Set(ByVal Value As Boolean)

                l_FonteStrike = Value
                l_documento.Instru��oAdd(LDDStandard.strike, l_FonteStrike.ToString)

            End Set
        End Property

        Public Property FonteUnderline() As Boolean
            Get

                Return l_FonteUnderline

            End Get
            Set(ByVal Value As Boolean)

                l_FonteUnderline = Value
                l_documento.Instru��oAdd(LDDStandard.under, l_FonteUnderline.ToString)

            End Set
        End Property

        Public Property FonteOverline() As Boolean
            Get

                Return l_FonteOverline

            End Get
            Set(ByVal Value As Boolean)

                l_FonteOverline = Value
                l_documento.Instru��oAdd(LDDStandard.over, l_FonteOverline.ToString)

            End Set
        End Property

        Public Property FonteAlinhamento() As System.Windows.Forms.HorizontalAlignment
            Get

                Return l_FonteAlinhamento

            End Get
            Set(ByVal Value As System.Windows.Forms.HorizontalAlignment)

                l_FonteAlinhamento = Value
                l_documento.Instru��oAdd(LDDStandard.align, CInt(l_FonteAlinhamento).ToString)

            End Set
        End Property

        Public Property FonteAngulo() As Integer
            Get

                Return l_FonteAngulo

            End Get
            Set(ByVal Value As Integer)

                l_FonteAngulo = Value
                l_documento.Instru��oAdd(LDDStandard.angle, l_FonteAngulo.ToString)

            End Set
        End Property

        Public Property Tinta() As System.Drawing.Color
            Get

                Return l_Tinta

            End Get
            Set(ByVal Value As System.Drawing.Color)

                l_Tinta = Value
                l_documento.Instru��oAdd(LDDStandard.brush, l_Tinta.ToArgb.ToString)

            End Set
        End Property

        Public Property Grossura() As Integer
            Get

                Return l_Grossura

            End Get
            Set(ByVal Value As Integer)

                l_Grossura = Value
                l_documento.Instru��oAdd(LDDStandard.border, l_Grossura.ToString)

            End Set
        End Property

        Public ReadOnly Property DocFrame() As System.Drawing.Rectangle
            Get

                Return l_paginador.Documento.Setup.Margem

            End Get
        End Property

        Public ReadOnly Property QuebraFrame() As System.Drawing.Rectangle
            Get

                Dim f As New System.Drawing.Rectangle(l_paginador.QuebraActual, l_paginador.QuebraActualDim)
                Return f

            End Get
        End Property

        Public Sub PrintTextBox(ByVal Texto As String, ByVal Caixa As System.Drawing.Rectangle)

            If Texto Is Nothing Then Texto = ""

            l_documento.Instru��oAdd(LDDStandard.setx1, (Me.Paginador.QuebraActual.X + Caixa.Left).ToString)
            l_documento.Instru��oAdd(LDDStandard.sety1, (Me.Paginador.QuebraActual.Y + Caixa.Top).ToString)
            l_documento.Instru��oAdd(LDDStandard.setx2, (Me.Paginador.QuebraActual.X + Caixa.Right).ToString)
            l_documento.Instru��oAdd(LDDStandard.sety2, (Me.Paginador.QuebraActual.Y + Caixa.Bottom).ToString)

            l_documento.Instru��oAdd(LDDStandard.printbox, Texto)


        End Sub

        Public Function PrintTextLines(ByVal Texto As String, ByVal x1 As Integer, ByVal y1 As Integer, ByVal Height As Integer, ByVal Width As Integer, ByVal SaltoY As Integer, ByRef TextoSobra() As String) As Integer

            If Not Texto Is Nothing Then

                Dim F As System.Drawing.Font
                Dim FonteNome As String = l_FonteNome

                If FonteNome.ToLower.EndsWith(".ttf") Then
                    FonteNome = FonteNome.Substring(0, FonteNome.Length - 4)
                End If

                If l_FonteBold Then
                    F = New System.Drawing.Font(FonteNome, l_FonteTamanho, Drawing.FontStyle.Bold)
                Else
                    F = New System.Drawing.Font(FonteNome, l_FonteTamanho)
                End If

                Height = y1 + Height

                l_documento.Instru��oAdd(LDDStandard.posx, (Me.Paginador.QuebraActual.X + x1).ToString)

                Dim res() As String = GetTextoByLines(Texto, Width, F)

                For i As Integer = 1 To res.GetUpperBound(0)
                    If y1 >= Height Then
                        ReDim TextoSobra(res.GetUpperBound(0) - i)
                        Dim C As Integer = 0
                        For j As Integer = i To res.GetUpperBound(0)
                            TextoSobra(C) = res(j)
                            C += 1
                        Next
                        y1 -= SaltoY
                        Return y1
                    Else
                        l_documento.Instru��oAdd(LDDStandard.posy, (Me.Paginador.QuebraActual.Y + y1).ToString)
                        l_documento.Instru��oAdd(LDDStandard.printlines, res(i))
                        y1 += SaltoY
                    End If
                Next
                F = Nothing
                Return y1
            Else
                TextoSobra = Nothing
                Return y1
            End If


        End Function

        Public Function GetTextLines(ByVal Texto As String, ByVal x1 As Integer, ByVal x2 As Integer, ByVal Factor As Integer) As String()

            If Factor <= 0 Then Factor = 1

            If Not Texto Is Nothing Then

                Dim F As System.Drawing.Font
                Dim FonteNome As String = l_FonteNome

                If FonteNome.ToLower.EndsWith(".ttf") Then
                    FonteNome = FonteNome.Substring(0, FonteNome.Length - 4)
                End If

                If l_FonteBold Then
                    F = New System.Drawing.Font(FonteNome, l_FonteTamanho, Drawing.FontStyle.Bold)
                Else
                    F = New System.Drawing.Font(FonteNome, l_FonteTamanho)
                End If

                Dim Res() As String = GetTextoByLines(Texto, (x2 - x1) * Factor, F)

                F = Nothing
                FonteNome = Nothing

                Return Res

            Else
                Return Nothing

            End If


        End Function

        '---------------------
        Private Function GetLarguraTexto(ByVal Texto As String, ByVal Fonte As System.Drawing.Font, ByVal ValorDefErro As Single) As Single

            Dim T As System.Drawing.SizeF

            Try

                T = A.MeasureString(Texto, Fonte)

                Return T.Width

            Catch ex As Exception

                Try
                    Dim E As Windows.Forms.Panel
                    Dim B As System.Drawing.Graphics

                    E = New Windows.Forms.Panel
                    B = E.CreateGraphics

                    T = B.MeasureString(Texto, Fonte)

                    A = Nothing
                    B = Nothing

                    Return T.Width

                Catch ex2 As Exception
                    Return ValorDefErro + 30
                End Try

            End Try

        End Function

        Private Function GetTextoByLines(ByVal Texto As String, ByVal Largura As Single, ByVal Fonte As System.Drawing.Font) As String()


            Dim Lin As String
            Dim L As Integer
            Dim C As Integer
            Dim LC As Integer
            Dim Esp As Integer
            Dim Enter As Integer
            Dim Linha() As String
            Dim Car As String
            Dim Pal As String
            Dim Excede As Boolean
            Dim Valor As Single

            Lin = ""
            L = 0

            C = 0
            LC = 1
            ReDim Linha(L)

            Texto = Texto.Replace(vbCrLf, vbCr)
            Texto = Texto.Replace(vbLf, vbCr)
            Texto = Texto.Replace(vbCr, vbCrLf)

            Do While LC <= Len(Texto)

                ' procura pontos onde o texto pode ser dividido em linhas
                Enter = InStr(LC, Texto, vbCrLf)
                If Enter = 0 Then Enter = InStr(LC, Texto, vbCr)
                Esp = InStr(LC, Texto, " ")

                If Enter = 0 Then Enter = Len(Texto) + 1
                If Esp = 0 Then Esp = Len(Texto) + 1

                ' determinad qual o ponto mais anterior
                If Enter <= Esp Then
                    C = Enter
                    Car = vbCrLf
                Else
                    C = Esp
                    Car = " "
                End If

                ' corta a ultima palavra antes do ponto
                If C <> 0 Then
                    Pal = Mid(Texto, LC, C - LC)
                Else
                    Pal = Mid(Texto, LC)
                End If

                If Car = vbCrLf Then C = C + 1

                ' testa se a linha j� existente mais a palavra agora analisada cabem na largura
                Valor = GetLarguraTexto(Lin & Pal & Car, Fonte, Valor)
                Excede = (Valor >= Largura - 10)
                If Excede Or Car = vbCrLf Or LC > Len(Texto) Then

                    L = L + 1
                    ' registar linha
                    ReDim Preserve Linha(L)
                    If Excede Then
                        If Len(Texto) <= C Then
                            Linha(L) = Lin & Pal
                        Else
                            Linha(L) = Lin
                        End If
                    Else
                        Linha(L) = Lin & Pal
                        Pal = ""
                    End If

                    ' linha para o pr�ximo ciclo
                    If Car <> vbCrLf Then
                        Lin = Pal & Car
                    Else
                        Lin = Pal
                    End If

                Else
                    Lin = Lin & Pal & Car
                End If

                LC = C + 1

            Loop

            Return Linha

        End Function
        '---------------------
        Public Sub Print()

            l_documento.Instru��oAdd(LDDStandard.str, "")

        End Sub

        Public Sub Print(ByVal Texto As String)

            If Texto Is Nothing Then Texto = ""
            l_documento.Instru��oAdd(LDDStandard.conc, Texto)

        End Sub

        Public Sub PrintNTP()

            l_documento.Instru��oAdd(LDDStandard.tpc, Nothing)

        End Sub

        Public Sub PrintPAG()

            l_documento.Instru��oAdd(LDDStandard.pag, Nothing)

        End Sub

        Public Sub Print(ByVal X As Integer, ByVal Y As Integer, ByVal Texto As String)

            l_documento.Instru��oAdd(LDDStandard.posx, (Me.Paginador.QuebraActual.X + X).ToString)
            l_documento.Instru��oAdd(LDDStandard.posy, (Me.Paginador.QuebraActual.Y + Y).ToString)
            If Texto Is Nothing Then Texto = ""
            l_documento.Instru��oAdd(LDDStandard.str, Texto)
            l_documento.Instru��oAdd(LDDStandard.print, Nothing)

        End Sub

        Public Sub Print(ByVal X As Integer, ByVal Y As Integer)

            l_documento.Instru��oAdd(LDDStandard.posx, (Me.Paginador.QuebraActual.X + X).ToString)
            l_documento.Instru��oAdd(LDDStandard.posy, (Me.Paginador.QuebraActual.Y + Y).ToString)
            l_documento.Instru��oAdd(LDDStandard.print, Nothing)

        End Sub

        Public Sub Desenha(ByVal imagem As System.Drawing.Image, ByVal destino As System.Drawing.Rectangle)

            Dim obj_id As Integer = l_documento.StackObjAdd("", imagem)

            l_documento.Instru��oAdd(LDDStandard.setx1, (Me.Paginador.QuebraActual.X + destino.Left).ToString)
            l_documento.Instru��oAdd(LDDStandard.sety1, (Me.Paginador.QuebraActual.Y + destino.Top).ToString)
            l_documento.Instru��oAdd(LDDStandard.setx2, (Me.Paginador.QuebraActual.X + destino.Right).ToString)
            l_documento.Instru��oAdd(LDDStandard.sety2, (Me.Paginador.QuebraActual.Y + destino.Bottom).ToString)
            l_documento.Instru��oAdd(LDDStandard.obj, obj_id.ToString)
            l_documento.Instru��oAdd(LDDStandard.pic, Nothing)

        End Sub

        Public Sub CriaAssinatura(ByVal sign As PDFSignatureField)

            Dim obj_id As Integer = l_documento.StackObjAdd("", sign)

            l_documento.Instru��oAdd(LDDStandard.setx1, (Me.Paginador.QuebraActual.X + sign.Widgets(0).DisplayRectangle.Left).ToString)
            l_documento.Instru��oAdd(LDDStandard.sety1, (Me.Paginador.QuebraActual.Y + sign.Widgets(0).DisplayRectangle.Top).ToString)
            l_documento.Instru��oAdd(LDDStandard.setx2, (Me.Paginador.QuebraActual.X + sign.Widgets(0).DisplayRectangle.Width).ToString)
            l_documento.Instru��oAdd(LDDStandard.sety2, (Me.Paginador.QuebraActual.Y + sign.Widgets(0).DisplayRectangle.Height).ToString)
            l_documento.Instru��oAdd(LDDStandard.digitalsign, obj_id.ToString)
            l_documento.Assina_Doc = True


        End Sub


        Public Sub Desenha(ByVal caminho As String, ByVal destino As System.Drawing.Rectangle)

            Dim imagem As System.Drawing.Image

            imagem = System.Drawing.Image.FromFile(caminho)

            If Not imagem Is Nothing Then Me.Desenha(imagem, destino)

        End Sub

        Public Sub DesenhaRef(ByVal caminho As String, ByVal destino As System.Drawing.Rectangle)

            l_documento.Instru��oAdd(LDDStandard.setx1, (Me.Paginador.QuebraActual.X + destino.Left).ToString)
            l_documento.Instru��oAdd(LDDStandard.sety1, (Me.Paginador.QuebraActual.Y + destino.Top).ToString)
            l_documento.Instru��oAdd(LDDStandard.setx2, (Me.Paginador.QuebraActual.X + destino.Right).ToString)
            l_documento.Instru��oAdd(LDDStandard.sety2, (Me.Paginador.QuebraActual.Y + destino.Bottom).ToString)
            l_documento.Instru��oAdd(LDDStandard.str, caminho)
            l_documento.Instru��oAdd(LDDStandard.foto, Nothing)

        End Sub

        Public Sub Linha(ByVal x1 As Integer, ByVal y1 As Integer, ByVal x2 As Integer, ByVal y2 As Integer)

            l_documento.Instru��oAdd(LDDStandard.setx1, x1.ToString)
            l_documento.Instru��oAdd(LDDStandard.sety1, y1.ToString)
            l_documento.Instru��oAdd(LDDStandard.setx2, x2.ToString)
            l_documento.Instru��oAdd(LDDStandard.sety2, y2.ToString)

            l_documento.Instru��oAdd(LDDStandard.line, Nothing)

        End Sub

        Public Sub Caixa(ByVal x As Integer, ByVal y As Integer, ByVal largura As Integer, ByVal altura As Integer)

            l_documento.Instru��oAdd(LDDStandard.setx1, x.ToString)
            l_documento.Instru��oAdd(LDDStandard.sety1, y.ToString)
            l_documento.Instru��oAdd(LDDStandard.setx2, (x + largura).ToString)
            l_documento.Instru��oAdd(LDDStandard.sety2, (y + altura).ToString)

            l_documento.Instru��oAdd(LDDStandard.box, System.Drawing.Color.Transparent.ToArgb.ToString)

        End Sub

        Public Sub Caixa(ByVal x As Integer, ByVal y As Integer, ByVal largura As Integer, ByVal altura As Integer, ByVal CorInterior As System.Drawing.Color)

            l_documento.Instru��oAdd(LDDStandard.setx1, x.ToString)
            l_documento.Instru��oAdd(LDDStandard.sety1, y.ToString)
            l_documento.Instru��oAdd(LDDStandard.setx2, (x + largura).ToString)
            l_documento.Instru��oAdd(LDDStandard.sety2, (y + altura).ToString)

            l_documento.Instru��oAdd(LDDStandard.brush, l_Tinta.ToArgb.ToString)
            l_documento.Instru��oAdd(LDDStandard.box, CorInterior.ToArgb.ToString)

        End Sub

        Public Sub Poligono(ByVal Pontos() As System.Drawing.Point, ByVal CorLinha As System.Drawing.Color, ByVal CorInterior As System.Drawing.Color)

            l_documento.Instru��oAdd(LDDStandard.polini, Nothing)

            For i As Integer = 0 To Pontos.GetUpperBound(0)

                l_documento.Instru��oAdd(LDDStandard.setx1, Pontos(i).X.ToString)
                l_documento.Instru��oAdd(LDDStandard.sety1, Pontos(i).Y.ToString)
                l_documento.Instru��oAdd(LDDStandard.polvert, Nothing)

            Next

            l_documento.Instru��oAdd(LDDStandard.brush, CorLinha.ToArgb.ToString)
            l_documento.Instru��oAdd(LDDStandard.polend, Nothing)
            l_documento.Instru��oAdd(LDDStandard.poldraw, CorInterior.ToArgb.ToString)

        End Sub

        Public Sub Poligono(ByVal Pontos() As System.Drawing.Point, ByVal CorLinha As System.Drawing.Color)

            Poligono(Pontos, CorLinha, System.Drawing.Color.Transparent)

        End Sub

        Public Sub QuebraBorder()

            Dim x As Integer = Me.Paginador.QuebraActual.X
            Dim y As Integer = Me.Paginador.QuebraActual.Y
            Dim w As Integer = Me.Paginador.QuebraActualDim.Width
            Dim h As Integer = Me.Paginador.QuebraActualDim.Height

            Me.Caixa(x, y, w, h)

        End Sub

    End Class

    Public Class ProcLDDBase
        Implements ILDDPROC



        ' este processador trata das instru��es LDD que, independentemente do destino, tem o mesmo
        ' resultado ou comportamento

        Protected l_processo As LDDProcesso

        Public Property Processo() As LDDProcesso Implements ILDDPROC.Processo
            Get

                Return l_processo

            End Get
            Set(ByVal Value As LDDProcesso)

                l_processo = Value

            End Set
        End Property

        Public Overridable Sub Reset() Implements ILDDPROC.Reset

            l_processo.SerieParteActual = 0
            l_processo.PaginaActual = 0
            l_processo.Instru��oActual = 0

            l_processo.objecto = Nothing
            l_processo.str = Nothing
            l_processo.inteiro = Nothing
            l_processo.posx = Nothing
            l_processo.posy = Nothing
            l_processo.poswidth = Nothing
            l_processo.posheight = Nothing
            l_processo.x1 = Nothing
            l_processo.y1 = Nothing
            l_processo.x2 = Nothing
            l_processo.y2 = Nothing
            l_processo.fonte_nome = Nothing
            l_processo.fonte_tamanho = Nothing
            l_processo.fonte_bold = Nothing
            l_processo.fonte_italic = Nothing
            l_processo.fonte_underline = Nothing
            l_processo.fonte_strike = Nothing
            l_processo.fonte_overline = Nothing
            l_processo.fonte_cor = Nothing
            l_processo.fonte_angulo = Nothing
            l_processo.fonte_alinhamento = Nothing

        End Sub

        Public Overridable Overloads Function RealizaDocumento() As Boolean Implements ILDDPROC.RealizaDocumento

            Dim ok As Boolean

            Me.Reset()

            ok = True

            For l_processo.SerieParteActual = 1 To l_processo.Documento.QuantasSeriesPartes

                ok = ok And Me.RealizaSerie()

            Next

            Return ok

        End Function

        Public Overridable Overloads Function RealizaDocumento(ByVal p_processo As LDDProcesso) As Boolean Implements ILDDPROC.RealizaDocumento

            Dim proc As LDDProcesso

            proc = l_processo
            l_processo = p_processo

            Me.RealizaDocumento()

            l_processo = proc

        End Function

        Public Overridable Overloads Function RealizaSerie() As Boolean Implements ILDDPROC.RealizaSerie

            Dim ok As Boolean

            ok = True

            For l_processo.PaginaActual = 0 To l_processo.Documento.QuantasPaginas - 1

                ok = ok And Me.RealizaPagina()

            Next

            Return ok

        End Function

        Public Overridable Overloads Function RealizaSerie(ByVal SerieParteIndex As Integer) As Boolean Implements ILDDPROC.RealizaSerie

            Dim proc_ori As LDDProcesso
            Dim proc_aux As LDDProcesso

            proc_ori = l_processo

            proc_aux = l_processo
            proc_aux.SerieParteActual = SerieParteIndex
            RealizaSerie = Me.RealizaSerie

            l_processo = proc_ori

        End Function

        Public Overridable Overloads Function RealizaSerie(ByVal p_processo As LDDProcesso) As Boolean Implements ILDDPROC.RealizaSerie

            Dim proc As LDDProcesso

            proc = l_processo

            l_processo = p_processo
            RealizaSerie = Me.RealizaSerie()

            l_processo = proc

        End Function

        Public Overridable Overloads Function RealizaPagina() As Boolean Implements ILDDPROC.RealizaPagina

            Dim ok As Boolean

            ok = True

            For l_processo.Instru��oActual = 0 To l_processo.Documento.Paginas(l_processo.PaginaActual).QuantasInstru��es - 1

                ok = ok And Me.RealizaInstru��o()

            Next

            Return ok

            ' em qualquer heran�a que use este m�todo ele deve ser chamado no fim
            ' de forma permitir qualquer tipo de prepara��o antes de processe as respectivas Paginas

        End Function

        Public Overridable Overloads Function RealizaPagina(ByVal SerieParteIndex As Integer, ByVal PaginaIndex As Integer) As Boolean Implements ILDDPROC.RealizaPagina

            Dim proc_ori As LDDProcesso
            Dim proc_aux As LDDProcesso

            proc_ori = l_processo

            proc_aux = l_processo
            proc_aux.SerieParteActual = SerieParteIndex
            proc_aux.PaginaActual = PaginaIndex
            RealizaPagina = Me.RealizaPagina

            l_processo = proc_ori

        End Function

        Public Overridable Overloads Function RealizaPagina(ByVal p_processo As LDDProcesso) As Boolean Implements ILDDPROC.RealizaPagina

            Dim proc As LDDProcesso

            proc = l_processo

            l_processo = p_processo
            RealizaPagina = Me.RealizaPagina()

            l_processo = proc

        End Function

        Public Overridable Overloads Function RealizaPagina(ByVal p_pagina As LDDPagina) As Boolean Implements ILDDPROC.RealizaPagina

            Dim proc_ori As LDDProcesso
            Dim proc_aux As LDDProcesso

            proc_ori = l_processo

            proc_aux.Documento.QuantasPaginas = 1
            ReDim proc_aux.Documento.Paginas(0)
            proc_aux.Documento.Paginas(0) = p_pagina
            l_processo = proc_aux
            RealizaPagina = Me.RealizaPagina()

            l_processo = proc_ori

        End Function

        Public Overridable Overloads Function RealizaInstru��o() As Boolean Implements ILDDPROC.RealizaInstru��o

            Dim instru��o As LDDInstru��o

            instru��o = l_processo.Documento.Paginas(l_processo.PaginaActual).Instru��es(l_processo.Instru��oActual)

            Select Case instru��o.Codigo

                Case LDDStandard.obj : l_processo.objecto = l_processo.Documento.StackObj(CInt(instru��o.Parametro))
                Case LDDStandard.str : l_processo.str = instru��o.Parametro.ToString
                Case LDDStandard.int : l_processo.inteiro = CInt(instru��o.Parametro)

                Case LDDStandard.posx : l_processo.posx = CInt(instru��o.Parametro)
                Case LDDStandard.posy : l_processo.posy = CInt(instru��o.Parametro)
                Case LDDStandard.poswidth : l_processo.poswidth = CInt(instru��o.Parametro)
                Case LDDStandard.posheight : l_processo.posheight = CInt(instru��o.Parametro)
                Case LDDStandard.tpc : l_processo.str = l_processo.str & l_processo.Documento.QuantasPaginas.ToString
                Case LDDStandard.pag : l_processo.str = l_processo.str & (l_processo.PaginaActual + 1).ToString
                Case LDDStandard.conc : l_processo.str = l_processo.str & instru��o.Parametro.ToString
                Case LDDStandard.serie

                    Dim partes() As String = l_processo.Documento.StackSeries(CInt(instru��o.Parametro)).ToString.Split("~"c)
                    If l_processo.SerieParteActual <= partes.Length Then l_processo.str = l_processo.str & partes(l_processo.SerieParteActual)

                Case LDDStandard.setx1 : l_processo.x1 = CInt(instru��o.Parametro)
                Case LDDStandard.sety1 : l_processo.y1 = CInt(instru��o.Parametro)
                Case LDDStandard.setx2 : l_processo.x2 = CInt(instru��o.Parametro)
                Case LDDStandard.sety2 : l_processo.y2 = CInt(instru��o.Parametro)

                Case LDDStandard.font : l_processo.fonte_nome = instru��o.Parametro.ToString
                Case LDDStandard.size : l_processo.fonte_tamanho = CInt(instru��o.Parametro)
                Case LDDStandard.bold : l_processo.fonte_bold = CBool(instru��o.Parametro)
                Case LDDStandard.italic : l_processo.fonte_italic = CBool(instru��o.Parametro)
                Case LDDStandard.under : l_processo.fonte_underline = CBool(instru��o.Parametro)
                Case LDDStandard.strike : l_processo.fonte_strike = CBool(instru��o.Parametro)
                Case LDDStandard.over : l_processo.fonte_overline = CBool(instru��o.Parametro)
                Case LDDStandard.pen : l_processo.fonte_cor = CInt(instru��o.Parametro)

                Case LDDStandard.align : l_processo.fonte_alinhamento = CInt(instru��o.Parametro)
                Case LDDStandard.angle : l_processo.fonte_angulo = CInt(instru��o.Parametro)

                Case LDDStandard.border : l_processo.grossura = CInt(instru��o.Parametro)
                Case LDDStandard.brush : l_processo.tinta = CInt(instru��o.Parametro)

                Case LDDStandard.polini : l_processo.pontos = Nothing
                Case LDDStandard.polvert
                    If l_processo.pontos Is Nothing Then
                        ReDim l_processo.pontos(0)
                        l_processo.pontos(0).X = l_processo.x1
                        l_processo.pontos(0).Y = l_processo.y1
                    Else
                        ReDim Preserve l_processo.pontos(l_processo.pontos.GetUpperBound(0) + 1)
                        l_processo.pontos(l_processo.pontos.GetUpperBound(0)).X = l_processo.x1
                        l_processo.pontos(l_processo.pontos.GetUpperBound(0)).Y = l_processo.y1
                    End If
                Case LDDStandard.polend ' o futuro dos poligonos poder� depender disto

                Case Else

                    Return False

            End Select

            Return True

            ' em qualquer heran�a que use este m�todo ele deve ser chamado primeiro
            ' de forma a processar os comandos LDD b�sicos

        End Function

        Public Overridable Overloads Function RealizaInstru��o(ByVal SerieParteIndex As Integer, ByVal PaginaIndex As Integer, ByVal Instru��oIndex As Integer) As Boolean Implements ILDDPROC.RealizaInstru��o

            Dim proc_ori As LDDProcesso
            Dim proc_aux As LDDProcesso

            proc_ori = l_processo

            proc_aux = l_processo
            proc_aux.SerieParteActual = SerieParteIndex
            proc_aux.PaginaActual = PaginaIndex
            proc_aux.Instru��oActual = Instru��oIndex
            RealizaInstru��o = Me.RealizaInstru��o

            l_processo = proc_ori

        End Function

        Public Overridable Overloads Function RealizaInstru��o(ByVal p_processo As LDDProcesso) As Boolean Implements ILDDPROC.RealizaInstru��o

            Dim proc As LDDProcesso

            proc = l_processo

            l_processo = p_processo
            RealizaInstru��o = Me.RealizaInstru��o()

            l_processo = proc

        End Function

        Public Overridable Overloads Function RealizaInstru��o(ByVal p_pagina As LDDPagina) As Boolean Implements ILDDPROC.RealizaInstru��o

            Dim proc_ori As LDDProcesso
            Dim proc_aux As LDDProcesso

            proc_ori = l_processo

            proc_aux.Documento.QuantasPaginas = 1
            ReDim proc_aux.Documento.Paginas(0)
            proc_aux.Documento.Paginas(0) = p_pagina
            l_processo = proc_aux
            RealizaInstru��o = Me.RealizaInstru��o()

            l_processo = proc_ori

        End Function

        Public Overridable Overloads Function RealizaInstru��o(ByVal p_instru��o As LDDInstru��o) As Boolean Implements ILDDPROC.RealizaInstru��o

            Dim proc_ori As LDDProcesso
            Dim proc_aux As LDDProcesso

            proc_ori = l_processo

            proc_aux.Documento.QuantasPaginas = 1
            ReDim proc_aux.Documento.Paginas(0)
            proc_aux.Documento.Paginas(0).QuantasInstru��es = 1
            ReDim proc_aux.Documento.Paginas(0).Instru��es(0)
            proc_aux.Documento.Paginas(0).Instru��es(0) = p_instru��o
            l_processo = proc_aux
            RealizaInstru��o = Me.RealizaInstru��o()

            l_processo = proc_ori

        End Function

    End Class

    Public Class ProcLDD2Screen
        Inherits ProcLDDBase



        Public Folhas() As System.Drawing.Graphics

        Public Overloads Overrides Function RealizaPagina() As Boolean

            Folhas(l_processo.PaginaActual).Clear((New System.Drawing.Color).Transparent)

            MyBase.RealizaPagina()

        End Function

        Public Overloads Overrides Function RealizaInstru��o() As Boolean

            Dim instru��o As LDDInstru��o

            If Not MyBase.RealizaInstru��o() Then

                instru��o = l_processo.Documento.Paginas(l_processo.PaginaActual).Instru��es(l_processo.Instru��oActual)

                ' todo: LDD2Screen: interpreta��o das instru��es LDD que resultam em ecran

                Select Case instru��o.Codigo

                    Case LDDStandard.print

                        'Folhas(Me.Processo.PaginaActual).DrawString(Me.Processo.string, )

                End Select

            End If

        End Function

    End Class

    Public Class ProcLDD2Printer
        Inherits ProcLDDBase

        ' todo: LDD2Printer
        ' este processador LDD gera saida do documento para impress�o em papel numa determinada impressora

    End Class

    '                                                                                               

    Module exemplo_de_implementa��o_real_num_script

        Private ReadOnly _locker As New Object()

        ' exemplo de implementa��o real num script

        ' MotorDeEmpresas.MotorDeEmpresas_ReportTime
        ' o comando tem que ser criado a partir de uma conex�o que n�o est� indicada :)
        ' � necess�rio verificar se a sequencia � Nothing e adicionar uma ordena��o default :)
        ' � necess�rio retirar a ultima "," da ultima ordena��o :)
        ' � necess�rio verificar se os tracer n�o est�o a null antes de os usar :)

        Private Doc As ParserLDD2Doc

        Public Class MotorDeEmpresas
            Inherits MotorBase

            Private tracer_empresa As Extens�oEntidade
            Private tracer_empresa_contacto As Extens�oEntidadeFuncional

            Public Overrides ReadOnly Property Titulo() As String
                Get

                    Return "Motor de Empresas"

                End Get
            End Property

            Public Overrides ReadOnly Property Descri��o() As String
                Get

                    Return "Selec��o dos dados relativos �s empresas"

                End Get
            End Property

            Public Overrides ReadOnly Property ModeloRequisito() As System.Type
                Get

                    Return GetType(ModeloBase)

                End Get
            End Property

            Public Overrides ReadOnly Property Extens�esRequisito() As System.Type()
                Get

                    ' indicamos quais s�o as extens�es de 1� n�vel exigidas por este motor
                    Dim arr() As System.Type = {GetType(Empresa)}
                    Return arr

                End Get
            End Property

            Private Sub MotorDeEmpresas_MaisUmaExtens�o(ByVal extens�o As IEXTENS�O) Handles MyBase.MaisUmaExtens�o

                ' verificar cada extens�o de 1� n�vel do modelo escolhido pelo utilizador
                ' se for algumas das extens�es que este motor exige registamo-la no respectivo tracer

                If TypeOf extens�o Is Extens�oEntidade Then

                    tracer_empresa = CType(extens�o, Extens�oEntidade)

                    Dim sub_extens�o As IEXTENS�O

                    ' no caso da extens�o para a empresa (Entidade) 
                    ' n�s gostariamos que ela estivesse extendida para os contactos (EntidadeFuncional)

                    If ProcuraSubExtens�o(tracer_empresa.Extens�es, GetType(Extens�oEntidadeFuncional), sub_extens�o) Then

                        ' a extens�o "Entidade" est� extendida para a fun��o "EntidadeFuncional"
                        ' por isso registamos essa extens�o no tracer adequado
                        tracer_empresa_contacto = CType(sub_extens�o, Extens�oEntidadeFuncional)

                    End If

                End If

            End Sub

            Private Sub MotorDeEmpresas_MomentoReport() Handles MyBase.MomentoReport

                Dim comm As System.Data.IDbCommand
                Dim cursor As System.Data.IDataReader

                comm.CommandType = CommandType.Text
                comm.CommandText = "SELECT emp.*, cont.* FROM tab_empresas emp LEFT JOIN tab_contactos cont ON cont.empresa=emp.id"
                comm.CommandText = comm.CommandText & " ORDER BY "

                For Each quebra As Integer In l_modelo.Sequencia
                    Select Case quebra
                        Case Quebras.EntidadeId : comm.CommandText = "emp.id,"
                        Case Quebras.EntidadeAbreviatura : comm.CommandText = "emp.sigla,"
                        Case Quebras.EntidadeNome : comm.CommandText = "emp.desig,"
                        Case Quebras.EntidadeTituloProf : comm.CommandText = "emp.titprof,"
                        Case Quebras.EntidadeTituloSocial : comm.CommandText = "emp.titsoc,"
                        Case Quebras.EntidadeNacionalidade : comm.CommandText = "emp.nacion,"
                        Case Quebras.EntidadeGenero
                        Case Quebras.EntidadeMoradaPostal : comm.CommandText = "emp.morpst,"
                        Case Quebras.EntidadeMoradaPais : comm.CommandText = "emp.morpais,"
                        Case Quebras.EntidadeFun��o : comm.CommandText = "cont.fun,"
                        Case Quebras.EntidadeDepartamento : comm.CommandText = "cont.dep,"
                    End Select
                Next

                cursor = comm.ExecuteReader

                While cursor.Read

                    tracer_empresa.Entidade.Id = cursor.GetInt32(0)
                    tracer_empresa.Entidade.Abreviatura = cursor.GetString(0)
                    tracer_empresa.Entidade.Nome = cursor.GetString(0)
                    tracer_empresa.Entidade.TituloProf = cursor.GetString(0)
                    tracer_empresa.Entidade.TituloSocial = cursor.GetString(0)
                    tracer_empresa.Entidade.Nacionalidade = cursor.GetString(0)
                    tracer_empresa.Entidade.Genero = Nothing
                    tracer_empresa.Entidade.Morada = cursor.GetString(0)
                    tracer_empresa.Entidade.MoradaPostal = cursor.GetString(0)
                    tracer_empresa.Entidade.MoradaPais = cursor.GetString(0)

                    tracer_empresa_contacto.Entidade.Id = cursor.GetInt32(0)
                    tracer_empresa_contacto.Entidade.Abreviatura = cursor.GetString(0)
                    tracer_empresa_contacto.Entidade.Nome = cursor.GetString(0)
                    tracer_empresa_contacto.Entidade.TituloProf = cursor.GetString(0)
                    tracer_empresa_contacto.Entidade.TituloSocial = cursor.GetString(0)
                    tracer_empresa_contacto.Entidade.Nacionalidade = cursor.GetString(0)
                    tracer_empresa_contacto.Entidade.Genero = Nothing
                    tracer_empresa_contacto.Entidade.Morada = cursor.GetString(0)
                    tracer_empresa_contacto.Entidade.MoradaPostal = cursor.GetString(0)
                    tracer_empresa_contacto.Entidade.MoradaPais = cursor.GetString(0)

                    tracer_empresa_contacto.Fun��o.Titulo = cursor.GetString(0)
                    tracer_empresa_contacto.Fun��o.Departamento = cursor.GetString(0)

                    Me.TestaQuebras(tracer_empresa, tracer_empresa_contacto)

                End While

            End Sub

        End Class

        <ModeloFlag()> Public Class ListaDeEmpresas
            Inherits ModeloBase

            Friend l_ext_empresa As Empresa

            Public Sub New()

                l_ext_empresa = New Empresa

            End Sub

            Public Overrides ReadOnly Property Titulo() As String
                Get

                    Return "Lista de empresas"

                End Get
            End Property

            Public Overrides ReadOnly Property Descri��o() As String
                Get

                    Return "Lista de empresas com detalhe de contactos."

                End Get
            End Property

            Public Overrides ReadOnly Property Sequencia() As Integer()
                Get

                    Dim arr() As Integer = {Quebras.EntidadeId, Quebras.EntidadeFun��o}
                    Return arr

                End Get
            End Property

            Public Overrides ReadOnly Property Extens�es() As IEXTENS�O()
                Get

                    Dim arr() As IEXTENS�O = {l_ext_empresa}
                    Return arr

                End Get
            End Property

            Public Overrides Function PreConfig(ByVal Motor As IMOTOR, ByVal Dicionario() As String, ByVal ClienteDB As g10phPDF4.Setframe.setClienteDb) As Boolean

                Doc = New ParserLDD2Doc(Me.Documento, Me.Paginador)

                Me.Paginador.Cabe�alhoDim.Width = Me.Documento.Setup.Dimens�o.Width
                Me.Paginador.Cabe�alhoDim.Height = 750

                Me.Paginador.RodapeDim.Width = Me.Documento.Setup.Dimens�o.Width
                Me.Paginador.RodapeDim.Height = 750

                Me.Paginador.QuebraAdd(1, Me.Documento.Setup.Dimens�o.Width, 1000)
                Me.Paginador.QuebraAdd(2, Me.Documento.Setup.Dimens�o.Width, 500)

            End Function

            ' codifica��o nos eventos de todo o design

        End Class

        <Extens�oFlag()> Public Class Empresa
            Inherits Extens�oEntidade

            Private l_ext_contacto As EmpresaContacto

            Public Sub New()

                l_ext_contacto = New EmpresaContacto

            End Sub

            Public Overrides ReadOnly Property Extens�es() As IEXTENS�O()
                Get

                    Dim arr() As IEXTENS�O = {l_ext_contacto}
                    Return arr

                End Get
            End Property

            ' codifica��o nos eventos de todo o design


            Private Sub Empresa_QuebraEntidade() Handles MyBase.QuebraEntidade

            End Sub

            Private Sub Empresa_QuebraMoradaPais() Handles MyBase.QuebraMoradaPais

            End Sub
        End Class

        <Extens�oFlag()> Public Class EmpresaContacto
            Inherits Extens�oEntidadeFuncional

            ' codifica��o nos eventos de todo o design

            Private Sub EmpresaContacto_QuebraDepartamento() Handles MyBase.QuebraDepartamento

                Doc.FonteNome = "Helvetica"
                Doc.FonteCor = (New System.Drawing.Color).FromKnownColor(System.Drawing.KnownColor.DarkBlue)

                ' escala vertical

                For y As Integer = 0 To 900 Step 100
                    Doc.Print(0, y, "__")
                Next
                For y As Integer = 0 To 900 Step 10
                    Doc.Print(5, y, "__")
                Next
                For y As Integer = 0 To 900 Step 50
                    Doc.Print(10, y, "__")
                Next
                For y As Integer = 0 To 900 Step 100
                    Doc.Print(25, y + 4, y.ToString)
                Next

                ' representa��o em diferentes tamanhos

                For s As Integer = 20 To 5 Step -5

                    Doc.FonteTamanho = s

                    For y As Integer = 100 To 900 Step s
                        Doc.Print(600 - (s * 20), y, y.ToString)
                    Next

                Next

                Doc.FonteTamanho = 10

                For y As Integer = 100 To 900 Step 40
                    Doc.Print(150, y, "__________________________________________________________________")
                Next

                ' escala horizontal

                Doc.FonteTamanho = 10

                For x As Integer = 100 To 500 Step 100
                    Doc.Print(x, 0, "|")
                Next
                For x As Integer = 100 To 500 Step 10
                    Doc.Print(x, 8, "|")
                Next
                For x As Integer = 100 To 500 Step 50
                    Doc.Print(x, 16, "|")
                Next
                Doc.FonteAlinhamento = System.Windows.Forms.HorizontalAlignment.Center
                For x As Integer = 100 To 500 Step 100
                    Doc.Print(x, 35, (x - 100).ToString)
                Next

                Doc.NovaPagina()

                Doc.FonteTamanho = 15
                Doc.FonteBold = True
                Doc.FonteItalic = True
                Doc.FonteStrike = True
                Doc.FonteUnderline = True
                Doc.FonteOverline = True

                Doc.FonteAlinhamento = System.Windows.Forms.HorizontalAlignment.Right
                Doc.FonteNome = "times new roman"
                Doc.Print(100, 100, "Times New Roman")

                Doc.FonteAlinhamento = System.Windows.Forms.HorizontalAlignment.Center
                Doc.FonteNome = "helvetica"
                Doc.Print(100, 150, "Helvetica")

                Doc.FonteAlinhamento = System.Windows.Forms.HorizontalAlignment.Left
                Doc.FonteAngulo = 45

                Doc.FonteNome = "courier"
                Doc.Print(100, 200, "Courier")

                Doc.FonteAngulo = -45

                Doc.FonteNome = "Symbol"
                Doc.Print(100, 250, "Symbol")

                Doc.FonteAngulo = 0

                Doc.FonteAlinhamento = System.Windows.Forms.HorizontalAlignment.Right
                Doc.FonteNome = "zap"
                Doc.Print(100, 3000, "Zap")


            End Sub

        End Class

    End Module

End Namespace