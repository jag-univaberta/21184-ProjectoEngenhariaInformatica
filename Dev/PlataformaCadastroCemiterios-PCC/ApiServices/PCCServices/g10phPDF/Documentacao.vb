
Public Class Documentacao

    Public Sub New()

    End Sub

    Public Function CriaDocumento(ByRef Pedido As PedidoDocumento, ByVal TScript As String) As WebRespostaDoc

        Dim resposta As New WebRespostaDoc
        Dim dir As New Director()

        'dir.Id = 1
        'dir.Descricao = "Modelos WebEPL"
        'dir.Titulo = "Modelos WebEPL"
        'dir.TScript = "#orientacao PORTRAIT #papel A4 #modelo SBG_01" ' "#form FORM_LOC #pagina A4 #orientação PORTRAIT "
        'dir.Codigo = "DIR0000"


        Dim motor As New g10phPDF4.Access.MotorDePretensões

        dir.Motor = motor
        motor.Area = Pedido.Extensao.conv_2_gisFRAME_IEXTENSAO
        dir.Motor.PedidoId = Pedido.PedidoId

        Dim S As String = ""
        ' Está nothing porque o objecto ainda não está implementado
        motor.Pretensão = Nothing
        motor.Geometrias = ""

        If Not Pedido.FicheiroDestino Is Nothing Then

            Try
                System.IO.File.Delete(Pedido.FicheiroDestino)
            Catch ex As Exception

            End Try

            motor.FicheiroDestino = Pedido.FicheiroDestino
            AddHandler dir.PosModelo, AddressOf ApanhaPosModelo

            dir.TScript = TScript
            dir.ProcessaScript(Pedido)

            resposta.Sucesso = True

        Else

            resposta.Sucesso = False

        End If

        'End If

        Return resposta

    End Function

    Private Sub ApanhaPosModelo(ByVal Motor As g10phPDF4.Report.IMOTOR, ByVal Modelo As g10phPDF4.Report.IMODELO)

        Dim proc As New g10phPDF4.PDF.ProcLDD2PDF
        proc.Ficheiro = CType(Motor, g10phPDF4.Access.MotorDePretensões).FicheiroDestino
        proc.FicheiroAppend = True
        proc.Processo = New g10phPDF4.Report.LDDProcesso(Modelo.Documento)
        proc.RealizaDocumento()

    End Sub

End Class


Public Class Director

    Public Id As Integer
    Public Titulo As String
    Public Descricao As String
    Public Codigo As String
    Public Script As String
    Public TScript As String

    Public Motor As g10phPDF4.Access.MotorDePretensões

    Private script_forma As g10phPDF4.Report.PaginaForma
    Private script_orientação As g10phPDF4.Report.PaginaOrientação
    Private script_paginação As g10phPDF4.Report.NumeroPaginaAlinhamento



    Public Sub New()

    End Sub

    Public Sub Clone(ByVal classe_a_clonar As Director)
    End Sub


    Public Function ProcessaScript(ByRef Pedido As PedidoDocumento) As Boolean

        Dim directivas() As String = IIf(TScript.Length > 0, TScript, Me.Script).ToString.Split("#"c)
        Dim c As String
        Dim p As String
        Dim Mapa As String
        Dim Escala As Integer = 0

        For Each direct As String In directivas

            If direct.Trim.Length > 0 Then

                c = g10phPDF4.Basframe.Strings.Corta1ªPalavra(direct)
                p = g10phPDF4.Basframe.Strings.Sem1ªPalavra(direct)

                Select Case c.ToLower

                    Case "pagina", "papel" : script_forma = paginaDirect2phFRAME(p)
                    Case "orientação", "orientacao" : script_orientação = orientaçãoDirect2phFRAME(p)
                    Case "paginação", "paginacao" : script_paginação = alinhamentoDirect2phFRAME(p)
                    Case "mapa"
                        Mapa = p
                    Case "escala"
                        If IsNumeric(p) Then Escala = CInt(Val(p))

                    Case "modelo" : lançar_modelo(Pedido, p.Trim, Mapa, Escala)
                        Escala = 0
                        Mapa = ""

                End Select

            End If

        Next

    End Function

    Private Function paginaDirect2phFRAME(ByVal pagina As String) As g10phPDF4.Report.PaginaForma

        Select Case pagina.Trim.ToLower
            Case "a0" : Return g10phPDF4.Report.PaginaForma.A0
            Case "a1" : Return g10phPDF4.Report.PaginaForma.A1
            Case "a2" : Return g10phPDF4.Report.PaginaForma.A2
            Case "a3" : Return g10phPDF4.Report.PaginaForma.A3
            Case "a4" : Return g10phPDF4.Report.PaginaForma.A4
            Case "a5" : Return g10phPDF4.Report.PaginaForma.A5

        End Select

    End Function
    Private Function alinhamentoDirect2phFRAME(ByVal orientação As String) As g10phPDF4.Report.NumeroPaginaAlinhamento

        Select Case orientação.Trim.ToLower

            Case "bottomcenter" : Return g10phPDF4.Report.NumeroPaginaAlinhamento.BottomCenter
            Case "bottomleft" : Return g10phPDF4.Report.NumeroPaginaAlinhamento.BottomLeft
            Case "bottomright" : Return g10phPDF4.Report.NumeroPaginaAlinhamento.BottomRight
            Case "topcenter" : Return g10phPDF4.Report.NumeroPaginaAlinhamento.TopCenter
            Case "topleft" : Return g10phPDF4.Report.NumeroPaginaAlinhamento.TopLeft
            Case "topright" : Return g10phPDF4.Report.NumeroPaginaAlinhamento.TopRight


        End Select

    End Function
    Private Function orientaçãoDirect2phFRAME(ByVal orientação As String) As g10phPDF4.Report.PaginaOrientação

        Select Case orientação.Trim.ToLower

            Case "portrait" : Return g10phPDF4.Report.PaginaOrientação.Portrait
            Case "landscape" : Return g10phPDF4.Report.PaginaOrientação.Landscape

        End Select

    End Function

    Private Sub lançar_modelo(ByRef Pedido As PedidoDocumento, ByVal codigo As String, ByVal Mapa As String, ByVal Escala As Integer)

        Dim modelo As New Modelo()
        Dim clienteDB As g10phPDF4.Setframe.setClienteDb

        'If modelo.ConsultaPorCodigo(codigo) Then

        'iMODmod, sMODtit, sMODdesc, sMODcod, iMODtipo, mMODscript, sMODxml, sMODmwf, iMODescala, iMODarea, rMODpreco
        modelo.Id = 0
        modelo.Titulo = "Planta Teste"
        modelo.Descricao = "Planta Teste"
        modelo.Codigo = "SBG_01"
        modelo.Tipo = g10phPDF4.Report.ScriptTipo.Dll_Referencia
        modelo.Script = Pedido.ModeloScript '"C:\DesenvolvimentoG10\g10phPDF\g10phPDF\g10ModeloGeral\bin\Debug\g10ModeloGeral.dll"
        modelo.ModeloXML = Pedido.ModeloXML '"C:\DesenvolvimentoG10\g10phPDF\g10phPDF\TestarphPDF\Amarante_Cartografia.xml"
        modelo.ModeloMapa = ""
        modelo.ModeloEscala = Pedido.ModeloEscala
        modelo.AnalisaAreas = False
        modelo.Preco = 0
        modelo.ImagemMapa = Pedido.ImagemMapa '"C:\DesenvolvimentoG10\g10phPDF\g10phPDF\TestarphPDF\imagemmapa.png"
        modelo.imagemLegenda = Pedido.ImagemLegenda
        modelo.AreaMapa = Pedido.AreaMapa
        modelo.ModeloStartPageNumber = Pedido.NumeroPagina_Inicial
        modelo.Variaveis = Pedido.Variaveis
        modelo.AssinaturaDigital = Pedido.Certificado
        modelo.PasswordCert = Pedido.PasswordCert
        'modelo.Variaveis.Add("#id#|125")
        'modelo.Variaveis.Add("#mapaescala#|15000")
        'modelo.Variaveis.Add("#data#|31-12-2012")

        If modelo.CriaInstancia Then

            modelo.Instancia.Documento.Setup.Forma = Me.script_forma
            modelo.Instancia.Documento.Setup.Orientação = Me.script_orientação
            modelo.Instancia.Documento.NumeroPagina_Inicial = Pedido.NumeroPagina_Inicial
            modelo.Instancia.Documento.NumeroPagina_Alinhamento = Pedido.NumeroPagina_Alinhamento
            modelo.Instancia.Documento.NumeroPagina_FontSize = Pedido.NumeroPagina_FontSize
            modelo.Instancia.Documento.NumeroPagina_OffsetHorizontal = Pedido.NumeroPagina_OffsetHorizontal
            modelo.Instancia.Documento.NumeroPagina_OffsetVertical = Pedido.NumeroPagina_OffsetVertical
            modelo.Instancia.Documento.Mostra_NumeroPagina = Pedido.MostraNumeroPagina
            modelo.Instancia.Documento.Assina_Doc = Pedido.Assina_Doc
            modelo.Instancia.Documento.AssinaturaDigital = Pedido.Certificado
            modelo.Instancia.Documento.PasswordCert = Pedido.PasswordCert

            modelo.Instancia.Documento.NumeroPagina_Prefixo = Pedido.NumeroPagina_Prefixo
            modelo.Instancia.Documento.NumeroPagina_Total = Pedido.NumeroPagina_Total
            modelo.Instancia.Documento.NumeroPagina_TotalValor = Pedido.NumeroPagina_TotalValor

            RaiseEvent PreModelo(Me.Motor, modelo.Instancia)

            clienteDB = New g10phPDF4.Setframe.setClienteDb
            'clienteDB.Mania = Me.canal.Mania
            'clienteDB.Provider = Me.canal.Provider
            'clienteDB.ProviderDLL = Me.canal.ProviderDLL
            'clienteDB.ProviderModulo = Me.canal.ProviderModulo
            'clienteDB.ProviderTipo = Me.canal.ProviderTipo
            'clienteDB.Datasource = Me.canal.Datasource
            'clienteDB.Database = Me.canal.Database
            'clienteDB.Setup = Me.canal.Setup
            'clienteDB.DbLogin = Me.canal.DbLogin
            'ReDim clienteDB.Repositorios(0)
            'clienteDB.Repositorios(0) = New g10phPDF4.Setframe.setRepositorio
            'clienteDB.Repositorios(0).StringDeConexão = Me.canal.StringDeConexão
            'clienteDB.Repositorios(0).Conector = Me.canal.NovaConexão

            Me.Motor.ModeloXML = modelo.ModeloXML
            Me.Motor.ModeloEscala = modelo.ModeloEscala
            Me.Motor.ModeloMapa = modelo.ModeloMapa
            Me.Motor.ImagemMapa = modelo.ImagemMapa
            Me.Motor.ImagemLegenda = modelo.imagemLegenda
            Me.Motor.Variaveis = modelo.Variaveis
            Me.Motor.AreaMapa = modelo.AreaMapa

            If Len(Mapa) > 0 Then Me.Motor.ModeloMapa = Mapa
            If Escala > 0 Then Me.Motor.ModeloEscala = Escala

            Me.Motor.Executa(modelo.Instancia, Nothing, clienteDB)

            RaiseEvent PosModelo(Me.Motor, modelo.Instancia)
            Pedido.NumeroPagina_Final = modelo.Instancia.Documento.NumeroPagina_Inicial + modelo.Instancia.Documento.Paginas.Length - 1

        End If

        'End If

    End Sub

    Public Event PreModelo(ByVal Motor As g10phPDF4.Report.IMOTOR, ByVal Modelo As g10phPDF4.Report.IMODELO)

    Public Event PosModelo(ByVal Motor As g10phPDF4.Report.IMOTOR, ByVal Modelo As g10phPDF4.Report.IMODELO)

End Class


Public Class Modelo

    Public Id As Integer
    Public Titulo As String
    Public Descricao As String
    Public Codigo As String
    Public Tipo As g10phPDF4.Report.ScriptTipo
    Public Script As String
    Public ModeloXML As String
    Public ModeloMapa As String
    Public ModeloEscala As Integer
    Public ModeloStartPageNumber As Integer
    Public ModeloNrPaginas As Integer
    Public AnalisaAreas As Boolean
    Public Preco As Double
    Public ImagemMapa As String
    Public imagemLegenda As String
    Public AssinaturaDigital As String
    Public PasswordCert As String

    Public Variaveis As New List(Of String)

    Public AreaMapa As g10Map4.g10Map4View

    Public Instancia As g10phPDF4.Report.IMODELO

    Public Sub New()

    End Sub


    Public Function CriaInstancia() As Boolean

        Dim assemblagem As System.Reflection.Assembly
        Dim tipo As System.Type
        Dim objecto As Object

        Select Case Me.Tipo

            Case g10phPDF4.Report.ScriptTipo.Dll_Referencia

                assemblagem = g10phPDF4.Setframe.AssemblagemLoad(Me.Script)

        End Select

        tipo = g10phPDF4.Setframe.AssemblagemLoadTipo(assemblagem, "", New g10phPDF4.Report.ModeloFlag)

        If Not tipo Is Nothing Then

            Dim a As String = g10phPDF4.Basframe.File.GetPastaAssemblyActual

            objecto = g10phPDF4.Setframe.AssemblagemConstroiTipoDirecto(tipo)

            Me.Instancia = CType(objecto, g10phPDF4.Report.IMODELO)

        End If

        Return (Not Me.Instancia Is Nothing)

    End Function

End Class


Public Class WebRespostaDoc
    Inherits WebResposta

End Class

Public Class WebResposta

    Public Sucesso As Boolean
    Public Respostas As Integer

End Class
