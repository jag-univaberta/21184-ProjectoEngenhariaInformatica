Imports phAPPFRAME.g10phPDF4
Imports OSGeo.MapGuide
Imports System.Drawing
Imports System.IO
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging

Namespace Access

    Public Class MotorDePretensões
        Inherits Report.MotorBase

        Public Pretensão As Mapframe.IOBJECTO
        Public Area As Mapframe.IEXTENSÃO_
        Public FicheiroDestino As String
        Public PedidoId As Integer
        Public ModeloXML As String
        Public ModeloMapa As String
        Public ModeloEscala As Integer
        Public Geometrias As String
        Public ImagemMapa As String
        Public ImagemLegenda As String
        Public Variaveis As New List(Of String)
        Public AreaMapa As g10Map4.g10Map4View

        Private tracer_pretensão As ExtensãoPretensão
        Private tracer_requerente As Report.ExtensãoEntidade

        Private Shared ReadOnly _locker As New Object()

        Public Overrides ReadOnly Property Titulo() As String
            Get
                SyncLock _locker
                    Return "300"
                End SyncLock
            End Get
        End Property

        Public Overrides ReadOnly Property Descrição() As String
            Get
                SyncLock _locker
                    Return "301"
                End SyncLock
            End Get
        End Property

        Public Overrides ReadOnly Property ModeloRequisito() As System.Type
            Get
                SyncLock _locker
                    Return GetType(Report.ModeloBase)
                End SyncLock
            End Get
        End Property

        Public Overrides ReadOnly Property ExtensõesRequisito() As System.Type()
            Get
                SyncLock _locker
                    Dim arr() As System.Type = {GetType(ExtensãoPretensão)}
                    Return arr
                End SyncLock
            End Get
        End Property

        Private Sub MotorDePretensões_MaisUmaExtensão(ByVal extensão As Report.IEXTENSÃO) Handles MyBase.MaisUmaExtensão
            SyncLock _locker
                If TypeOf extensão Is ExtensãoPretensão Then
                    tracer_pretensão = CType(extensão, ExtensãoPretensão)
                    Report.ProcuraSubExtensão(tracer_pretensão.Extensões, GetType(Report.ExtensãoEntidade), tracer_requerente)
                End If
            End SyncLock
        End Sub

        Private Sub MotorDePretensões_MomentoReport() Handles MyBase.MomentoReport
            SyncLock _locker
                ' todo: ************ aqui
                'tracer_pretensão.Pretensão = Me.Pretensão
                'tracer_pretensão.Area = Me.Area
                tracer_pretensão.PedidoId = Me.PedidoId
                tracer_pretensão.ModeloXML = Me.ModeloXML
                tracer_pretensão.ModeloMapa = Me.ModeloMapa
                tracer_pretensão.ModeloEscala = Me.ModeloEscala
                tracer_pretensão.Geometrias = Me.Geometrias
                tracer_pretensão.ImagemMapa = Me.ImagemMapa
                tracer_pretensão.ImagemLegenda = Me.ImagemLegenda
                tracer_pretensão.Area = Me.Area
                tracer_pretensão.AreaMapa = Me.AreaMapa
                tracer_pretensão.Variaveis = Me.Variaveis
                If tracer_requerente Is Nothing Then
                    Me.TestaQuebras(tracer_pretensão)
                Else
                    Me.TestaQuebras(tracer_pretensão, tracer_requerente)
                End If
            End SyncLock
        End Sub

    End Class

    Public Class ExtensãoPretensão
        Implements Report.IEXTENSÃO

        Private Shared ReadOnly _locker As New Object()

        Public Overridable ReadOnly Property Extensões() As Report.IEXTENSÃO() Implements Report.IEXTENSÃO.Extensões
            Get
                SyncLock _locker
                    Return Nothing
                End SyncLock
            End Get
        End Property

        Public Pretensão As Mapframe.IOBJECTO
        Public Area As Mapframe.IEXTENSÃO_
        Public PedidoId As Integer
        Public ModeloXML As String
        Public ModeloMapa As String
        Public ModeloEscala As Integer
        Public Geometrias As String
        Public ImagemMapa As String
        Public ImagemLegenda As String
        Public Variaveis As New List(Of String)
        Public AreaMapa As g10Map4.g10Map4View

        Public Overridable Sub TestaQuebra(ByVal quebra As Integer, ByVal Paginador As Report.LDDPaginador) Implements Report.IEXTENSÃO.TestaQuebra
            SyncLock _locker
                Paginador.PaginateQuebra()
                RaiseEvent QuebraPretensão()
            End SyncLock
        End Sub

        Public Overridable Sub FimDeRegisto() Implements Report.IEXTENSÃO.FimDeRegisto

        End Sub

        Public Event QuebraPretensão()

        Public Overridable Function Type() As System.Type Implements Report.IEXTENSÃO.Type
            SyncLock _locker
                Return Me.GetType
            End SyncLock
        End Function

    End Class

    Public Class ParserLDD2LiteView
        Implements Report.ILDDPARSER

        Private Shared ReadOnly _locker As New Object()

        Private l_documento As Report.LDDDocumento
        Private l_paginador As Report.LDDPaginador

        Public Property Documento() As Report.LDDDocumento Implements Report.ILDDPARSER.Documento
            Get
                SyncLock _locker
                    Return l_documento
                End SyncLock
            End Get
            Set(ByVal Value As Report.LDDDocumento)
                SyncLock _locker
                    l_documento = Value
                    AddHandler l_documento.PaginaPrepara, AddressOf ApanhaNovaPagina
                End SyncLock
            End Set
        End Property

        Public Property Paginador() As Report.LDDPaginador Implements Report.ILDDPARSER.Paginador
            Get
                SyncLock _locker
                    Return l_paginador
                End SyncLock
            End Get
            Set(ByVal Value As Report.LDDPaginador)
                SyncLock _locker
                    l_paginador = Value
                End SyncLock
            End Set
        End Property

        Public Sub New()


        End Sub

        Public Sub New(ByVal p_documento As Report.LDDDocumento, ByVal p_paginador As Report.LDDPaginador)

            Me.New()
            SyncLock _locker
                l_documento = p_documento
                l_paginador = p_paginador

                AddHandler l_documento.PaginaPrepara, AddressOf ApanhaNovaPagina
            End SyncLock
        End Sub

        Private Sub ApanhaNovaPagina(ByVal Documento As Report.LDDDocumento)


        End Sub

        Public Sub NovaPagina()
            SyncLock _locker
                l_documento.PaginaAdd()
            End SyncLock
        End Sub

        Public Servidor As String
        Public Mwf As String
        Public CortaLogo As Boolean
        Public Factor As Integer = 1

        Public Function GetMapa(ByVal BBox As Mapframe.IEXTENSÃO_, ByVal Tamanho As System.Drawing.Size) As System.Drawing.Image
            SyncLock _locker
                Dim K As Integer = 0

                If CortaLogo Then K = 40

                Dim url As String = Servidor & "?REQUEST=MAP" & _
                                                "&RELOAD=TRUE" & _
                                                "&WIDTH=" & (Tamanho.Width * Factor) & _
                                                "&HEIGHT=" & (Tamanho.Height * Factor) + K & _
                                                "&FORMAT=PNG&LAYERS=" & Mwf & _
                                                "&BBOX=" & Basframe.NSF(BBox.VerticeSW.Lon) & "," & _
                                                Basframe.NSF(BBox.VerticeSW.Lat) & "," & _
                                                Basframe.NSF(BBox.VerticeNE.Lon) & "," & _
                                                Basframe.NSF(BBox.VerticeNE.Lat)

                Dim imagem_ori As System.Drawing.Image
                Dim imagem_dst As System.Drawing.Bitmap

                imagem_ori = SacaImagem(url)

                ' desta forma o parser pode ser parametrizado de forma a ignorar o "corte" do logotipo
                ' (util quando o liteview não fornece a imagem com o logotipo)
                If Not imagem_ori Is Nothing And CortaLogo Then
                    imagem_dst = SacaAutodeskLogo(imagem_ori)
                Else
                    imagem_dst = imagem_ori
                End If

                Return imagem_dst

            End SyncLock
        End Function

        Public Function PrintMapa(ByVal BBox As Mapframe.IEXTENSÃO_, ByVal AreaDestino As System.Drawing.Rectangle) As Boolean

            SyncLock _locker
                Dim imagem As System.Drawing.Image

                imagem = Me.GetMapa(BBox, AreaDestino.Size)

                If Not imagem Is Nothing Then

                    Dim objid As Integer = l_documento.StackObjAdd(Nothing, imagem)

                    l_documento.InstruçãoAdd(Report.LDDStandard.setx1, AreaDestino.X.ToString)
                    l_documento.InstruçãoAdd(Report.LDDStandard.sety1, AreaDestino.Y.ToString)
                    l_documento.InstruçãoAdd(Report.LDDStandard.setx2, AreaDestino.Width.ToString)
                    l_documento.InstruçãoAdd(Report.LDDStandard.sety2, AreaDestino.Height.ToString)
                    l_documento.InstruçãoAdd(Report.LDDStandard.obj, objid.ToString)
                    l_documento.InstruçãoAdd(Report.LDDStandard.pic, Nothing)

                    Return True

                Else

                    Return False

                End If
            End SyncLock
        End Function

        Public Sub DesenhaObjecto(ByVal Mapa As System.Drawing.Image, ByVal BBox As Mapframe.IEXTENSÃO_, ByVal Objecto As Mapframe.IOBJECTO)

            ' todo: ParserLDD2LiteView: desenha a geometria coorrespondente ao objecto sobre o mapa tendo em considerações as coordenadas de ambos

        End Sub

        Private Function SacaImagem(ByVal url As String) As System.Drawing.Image
            SyncLock _locker
                Dim wreq As System.Net.WebRequest = System.Net.WebRequest.Create(url)
                Dim hresp As System.Net.HttpWebResponse = CType(wreq.GetResponse, System.Net.HttpWebResponse)
                Dim saida As System.IO.Stream = hresp.GetResponseStream
                Return System.Drawing.Image.FromStream(saida)
            End SyncLock
        End Function

        Private Function SacaAutodeskLogo(ByVal Imagem As System.Drawing.Image) As System.Drawing.Image
            SyncLock _locker
                Dim retorno As System.Drawing.Bitmap

                retorno = New System.Drawing.Bitmap(Imagem.Width, Imagem.Height - 40, Imagem.PixelFormat)
                Dim graficos As System.Drawing.Graphics
                graficos = System.Drawing.Graphics.FromImage(retorno)
                Dim area_a_copiar As System.Drawing.Rectangle
                area_a_copiar = System.Drawing.Rectangle.FromLTRB(0, 20, Imagem.Width, Imagem.Height - 20)
                graficos.DrawImage(Imagem, 0, 0, area_a_copiar, Drawing.GraphicsUnit.Pixel)
            End SyncLock
        End Function

    End Class


    Public Class ParserLDD2MapguideOS
        Implements Report.ILDDPARSER

        Private Shared ReadOnly _locker As New Object()

        Private l_documento As Report.LDDDocumento
        Private l_paginador As Report.LDDPaginador

        Public Property Documento() As Report.LDDDocumento Implements Report.ILDDPARSER.Documento
            Get
                SyncLock _locker
                    Return l_documento
                End SyncLock
            End Get
            Set(ByVal Value As Report.LDDDocumento)
                SyncLock _locker
                    l_documento = Value
                    AddHandler l_documento.PaginaPrepara, AddressOf ApanhaNovaPagina
                End SyncLock
            End Set
        End Property

        Public Property Paginador() As Report.LDDPaginador Implements Report.ILDDPARSER.Paginador
            Get
                SyncLock _locker
                    Return l_paginador
                End SyncLock
            End Get
            Set(ByVal Value As Report.LDDPaginador)
                SyncLock _locker
                    l_paginador = Value
                End SyncLock
            End Set
        End Property

        Public Sub New()


        End Sub

        Public Sub New(ByVal p_documento As Report.LDDDocumento, ByVal p_paginador As Report.LDDPaginador)

            Me.New()
            SyncLock _locker
                l_documento = p_documento
                l_paginador = p_paginador

                AddHandler l_documento.PaginaPrepara, AddressOf ApanhaNovaPagina
            End SyncLock
        End Sub

        Private Sub ApanhaNovaPagina(ByVal Documento As Report.LDDDocumento)


        End Sub

        Public Sub NovaPagina()
            SyncLock _locker
                l_documento.PaginaAdd()
            End SyncLock
        End Sub

        Public webConfig As String
        Public userName As String
        Public userPassword As String
        Public Servidor As String
        Public DPI As Long

        Public Mapa As String
        Public MapaNome As String
        Public LayerTempXML As String
        Public LayerTempFeatureSourceName As String
        Public Escala As Integer
        Public Factor As Integer = 1 '3.125
        Public PedidoId As Integer
        Public Pretensao As Boolean
        Public Function GetMapafromURL(ByVal url As String, ByVal Tamanho As System.Drawing.Size) As System.Drawing.Image
            Dim retVal As System.Drawing.Image = Nothing
            Try


                Dim codeBase As String = Reflection.Assembly.GetExecutingAssembly().CodeBase
                Dim uri1 As UriBuilder = New UriBuilder(codeBase)
                Dim path1 As String = Uri.UnescapeDataString(uri1.Path)
                Dim logPath As String = IO.Path.GetDirectoryName(path1)

                Try

                    Dim logFile As String = logPath & "\g10phPDF.log"
                    Dim prefix As String = Date.Now.ToString("yyyy/MM/dd HH:mm:ss - ")
                    Using writer As New IO.StreamWriter(logFile, True)
                        writer.WriteLine(prefix & url)
                    End Using
                Catch ex1 As Exception

                End Try


                Dim p_imagewidth As Long = Tamanho.Width '(Tamanho.Width * Factor)
                Dim p_imageheight As Long = Tamanho.Height '(Tamanho.Height * Factor)

                Dim response As Net.WebResponse
                Dim request As Net.HttpWebRequest = DirectCast(Net.HttpWebRequest.Create(url), Net.HttpWebRequest)

                response = DirectCast(request.GetResponse(), Net.HttpWebResponse)
                retVal = System.Drawing.Image.FromStream(response.GetResponseStream())
                Dim mems As IO.MemoryStream

                Try

                    Dim ms As MemoryStream = New MemoryStream()
                    If p_imagewidth > 4095 Or p_imagewidth > 4095 Then
                        retVal.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
                    Else
                        retVal.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
                    End If

                    Dim aux As Byte() = ms.ToArray

                    Dim bb() As Byte

                    bb = aux
                
                    mems = New IO.MemoryStream(bb, 0, bb.Length)
                    mems.Write(bb, 0, bb.Length)


                Catch ex As Exception
                    Dim totalex As String = ex.Message.ToString
                    If ex.InnerException IsNot Nothing Then
                        totalex = totalex & vbCrLf & " inner: " & ex.InnerException.Message.ToString
                    End If
                    If ex.InnerException.InnerException IsNot Nothing Then
                        totalex = totalex & vbCrLf & " inner2: " & ex.InnerException.InnerException.Message.ToString
                    End If

                    Try

                        Dim logFile As String = logPath & "\g10phPDF.log"
                        Dim prefix As String = Date.Now.ToString("yyyy/MM/dd HH:mm:ss - ")
                        Using writer As New IO.StreamWriter(logFile, True)
                            writer.WriteLine(prefix & totalex.ToString)
                        End Using
                    Catch ex1 As Exception

                    End Try
                End Try
                Dim aux_bmp As New Bitmap(CInt(p_imagewidth), CInt(p_imageheight))


                aux_bmp.SetResolution(300, 300)
                Dim g As Graphics = Graphics.FromImage(aux_bmp)

                Dim layer As New Bitmap(mems)

                g.DrawImage(layer, 0, 0, CInt(p_imagewidth), CInt(p_imageheight))

                Dim sFN As String
                Dim tF As String = Path.GetTempPath()
                If Right(tF, 1) <> "\" Then tF &= "\"
                If CInt(Tamanho.Width) > 4095 Or CInt(Tamanho.Height) > 4095 Then
                    sFN = tF & Guid.NewGuid.ToString & ".jpg"
                Else
                    sFN = tF & Guid.NewGuid.ToString & ".png"
                End If
                Dim fx As String = ""

                If p_imagewidth > 4095 Or p_imageheight > 4095 Then

                    Dim myImageCodecInfo As ImageCodecInfo
                    Dim myEncoder As Encoder
                    Dim myEncoderParameter As EncoderParameter
                    Dim myEncoderParameters As EncoderParameters
                    myImageCodecInfo = GetEncoderInfo(ImageFormat.Jpeg)
                    myEncoder = Encoder.Quality
                    myEncoderParameters = New EncoderParameters(1)

                    ' Save the bitmap as a JPEG file with quality level 25.
                    myEncoderParameter = New EncoderParameter(myEncoder, CType(100L, Int32))
                    myEncoderParameters.Param(0) = myEncoderParameter
                    aux_bmp.Save(sFN, myImageCodecInfo, myEncoderParameters)
                    ' aux_bmp.Save(sFN, Imaging.ImageFormat.Jpeg)

                    System.Drawing.Image.FromStream(mems, True).Save(sFN & "2.jpg", Imaging.ImageFormat.Jpeg)
                    fx = sFN & "2.jpg"
                Else

                    Dim myImageCodecInfo As ImageCodecInfo
                    Dim myEncoder As Encoder
                    Dim myEncoderParameter As EncoderParameter
                    Dim myEncoderParameters As EncoderParameters
                    myImageCodecInfo = GetEncoderInfo(ImageFormat.Png)
                    myEncoder = Encoder.Quality
                    myEncoderParameters = New EncoderParameters(1)

                    ' Save the bitmap as a JPEG file with quality level 25.
                    myEncoderParameter = New EncoderParameter(myEncoder, CType(100L, Int32))

                    myEncoderParameters.Param(0) = myEncoderParameter
                    aux_bmp.Save(sFN, myImageCodecInfo, myEncoderParameters)
                    'aux_bmp.Save(sFN, Imaging.ImageFormat.Png)

                    System.Drawing.Image.FromStream(mems, True).Save(sFN & "2.png", Imaging.ImageFormat.Png)
                    fx = sFN & "2.png"
                End If

                Try

                    Dim logFile As String = logPath & "\g10phPDF.log"
                    Dim prefix As String = Date.Now.ToString("yyyy/MM/dd HH:mm:ss - ")
                    Using writer As New IO.StreamWriter(logFile, True)
                        writer.WriteLine(prefix & fx)
                    End Using
                Catch ex1 As Exception

                End Try


                Dim image2 As Image = Image.FromFile(fx)
                Return image2 'System.Drawing.Image.FromStream(mems, True) 'aux_bmp
            Catch generatedExceptionName As Exception
                retVal = Nothing
                Return Nothing
            End Try

        End Function
        Private Shared Function GetEncoderInfo(ByVal format As ImageFormat) As ImageCodecInfo
            Dim j As Integer
            Dim encoders() As ImageCodecInfo
            encoders = ImageCodecInfo.GetImageEncoders()

            j = 0
            While j < encoders.Length
                If encoders(j).FormatID = format.Guid Then
                    Return encoders(j)
                End If
                j += 1
            End While
            Return Nothing

        End Function 'GetEncoderInfo
        Public Function GetMapaURL(ByVal BBox As Mapframe.IEXTENSÃO_, ByVal Tamanho As System.Drawing.Size) As String 'System.Drawing.Image
            SyncLock _locker
                Dim K As Integer = 0
                Dim mapid As MgResourceIdentifier

                Try
                    Dim sMetricCSWKT As String = "PROJCS[&quot;Datum73a.ModPortgGrd&quot;,GEOGCS[&quot;Datum73a.LL&quot;,DATUM[&quot;Datum73-Mod/a&quot;,SPHEROID[&quot;INTNL&quot;,6378388.000,297.00000000],TOWGS84[-239.7490,88.1810,30.4880,0.263000,0.082000,1.211000,2.22900000]],PRIMEM[&quot;Greenwich&quot;,0],UNIT[&quot;Degree&quot;,0.017453292519943295]],PROJECTION[&quot;Transverse_Mercator&quot;],PARAMETER[&quot;false_easting&quot;,180.598],PARAMETER[&quot;false_northing&quot;,-86.990],PARAMETER[&quot;scale_factor&quot;,1.000000000000],PARAMETER[&quot;central_meridian&quot;,-8.13190611111111],PARAMETER[&quot;latitude_of_origin&quot;,39.66666666666666],UNIT[&quot;Meter&quot;,1.00000000000000]]"

                    Dim ms_aux As g10Map4.g10Mapguide4Server
                    Dim m_aux As g10Map4.g10Mapguide4Map
                    ms_aux = New g10Map4.g10Mapguide4Server(webConfig, userName, userPassword, Servidor)
                    m_aux = New g10Map4.g10Mapguide4Map(ms_aux, Mapa, MapaNome, sMetricCSWKT)

                    Dim resourceService As MgResourceService = DirectCast(ms_aux.SiteConnection.CreateService(MgServiceType.ResourceService), MgResourceService)
                    Dim mgSessionId As String = ms_aux.SessionID

                    'MapGuideApi.MgInitializeWebTier(webConfig)

                    'Dim url As String

                    'Dim userInfo As MgUserInformation = New MgUserInformation(userName, userPassword)
                    'Dim site As MgSite = New MgSite()

                    'site.Open(userInfo)
                    'Dim mgSessionId As String = site.CreateSession()

                    'Dim userInfoSession As MgUserInformation = New MgUserInformation(mgSessionId)
                    'Dim siteConnection As MgSiteConnection = New MgSiteConnection()
                    'siteConnection.Open(userInfo)

                    'Dim resourceId As MgResourceIdentifier = New MgResourceIdentifier(Mapa)

                    'Dim resourceSrvc As MgResourceService
                    'resourceSrvc = siteConnection.CreateService(MgServiceType.ResourceService)

                    ''Dim map As MgMap = New MgMap(siteConnection)
                    ''map.Create(resourceSrvc, resourceId, MapaNome)
                    ''mapid = New MgResourceIdentifier("Session:" & mgSessionId & "//" & MapaNome & "." & MgResourceType.Map)
                    ''map.Save(resourceSrvc, mapid)
                    'Dim resourceService As MgResourceService = DirectCast(siteConnection.CreateService(MgServiceType.ResourceService), MgResourceService)
                    'Dim featureService As MgFeatureService = DirectCast(siteConnection.CreateService(MgServiceType.FeatureService), MgFeatureService)

                    'Dim map As New MgMap(siteConnection)
                    'map.Open(MapaNome)

                    ''_resourceservice = siteConnection.CreateService(MgServiceType.ResourceService)
                    ''_featureservice = siteConnection.CreateService(MgServiceType.FeatureService)

                    'map = New MgMap(siteConnection)
                    'map.Open(MapaNome)
                    'Dim _mapid As MgResourceIdentifier
                    '_mapid = New MgResourceIdentifier("Session:" & mgSessionId & "//" & MapaNome & "." & MgResourceType.Map)
                    'map.Save()


                    'Dim userInfo1 As New MgUserInformation(mgSessionId)
                    'Dim siteConnection1 As New MgSiteConnection()
                    'siteConnection1.Open(userInfo1)

                    'Dim resourceService As MgResourceService = DirectCast(siteConnection.CreateService(MgServiceType.ResourceService), MgResourceService)
                    'Dim featureService As MgFeatureService = DirectCast(siteConnection.CreateService(MgServiceType.FeatureService), MgFeatureService)

                    'Dim map1 As New MgMap(siteConnection1)
                    'map1.Open(MapaNome)
                    If Pretensao Then

                        Dim xmlDoc As System.Xml.XmlDocument = New System.Xml.XmlDocument()
                        xmlDoc.Load(LayerTempXML)

                        Dim sw As IO.StringWriter = New IO.StringWriter()
                        Dim xw As System.Xml.XmlTextWriter = New System.Xml.XmlTextWriter(sw)

                        xmlDoc.WriteTo(xw)

                        Dim layerdefinition As String = sw.ToString()
                        layerdefinition = layerdefinition.Replace("%featuresource%", LayerTempFeatureSourceName)
                        layerdefinition = layerdefinition.Replace("%filter%", "pedidoid=" & PedidoId & "")

                        Dim encoding As New System.Text.UTF8Encoding()
                        Dim barray As Byte() = encoding.GetBytes(layerdefinition)

                        Dim byteSource1 As New MgByteSource(barray, Len(layerdefinition))
                        byteSource1.SetMimeType(MgMimeType.Xml)
                        Dim layerResourceID As New MgResourceIdentifier("Session:" & mgSessionId & "//" & "temp" & "." & MgResourceType.LayerDefinition)
                        Dim nullreader As MgByteReader
                        resourceService.SetResource(layerResourceID, byteSource1.GetReader, nullreader)

                        Dim mglayernovo As New MgLayer(layerResourceID, resourceService)
                        mglayernovo.SetLegendLabel("Temp")
                        mglayernovo.SetName("Temp")
                        mglayernovo.SetDisplayInLegend(True)
                        mglayernovo.SetSelectable(True)
                        mglayernovo.SetVisible(True)
                        mglayernovo.ForceRefresh()

                        Dim layerCollection As MgLayerCollection = m_aux.MapObject.GetLayers()
                        For Each layer As MgLayer In layerCollection
                            If layer.Name = "Temp" Then
                                layerCollection.Remove(layer)
                            End If
                        Next

                        layerCollection.Insert(0, mglayernovo)
                    End If
                    m_aux.MapObject.Save()

                    Dim DistX As Double
                    Dim DistY As Double
                    DistX = Math.Abs((BBox.VerticeSW.Lon - BBox.VerticeNE.Lon) / 2)
                    DistY = Math.Abs((BBox.VerticeSW.Lat - BBox.VerticeNE.Lat) / 2)
                    Dim centroX As Double = BBox.VerticeNE.Lon - DistX
                    Dim centroY As Double = BBox.VerticeNE.Lat - DistY
                    Dim p_imagewidth As Long = Tamanho.Width '(Tamanho.Width * Factor)
                    Dim p_imageheight As Long = Tamanho.Height '(Tamanho.Height * Factor)


                    ' ================================================================================================================
                    Dim byteReader As MgByteReader
                    Dim renderingService As MgRenderingService = ms_aux.SiteConnection.CreateService(MgServiceType.RenderingService)

                    Dim _actual_selection As MgSelection
                    _actual_selection = New MgSelection(m_aux.MapObject)
                    Dim geoFact As MgGeometryFactory = New MgGeometryFactory()
                    Dim center As MgCoordinate = geoFact.CreateCoordinateXY(centroX, centroY)

                    'Dim p_imagewidth As Long = Tamanho.Width '(Tamanho.Width * Factor)
                    'Dim p_imageheight As Long = Tamanho.Height '(Tamanho.Height * Factor)

                    p_imagewidth = p_imagewidth * 1.33 '0.66 '* Factor
                    p_imageheight = p_imageheight * 1.33 '* Factor

                    Dim csf As New MgCoordinateSystemFactory

                    Dim srs As String = m_aux.MapObject.GetMapSRS()
                    Dim code As String = csf.ConvertWktToCoordinateSystemCode(srs)
                    Dim epsgcode As String = csf.ConvertWktToEpsgCode(srs)
                    Dim rs As New g10GeoMetadata4.mtdReferenceSystemInfo(srs, csf.CreateFromCode(code))
                    rs.Code = epsgcode
                    rs.CodeSpace = "EPSG"

                    Dim geogCS As MgCoordinateSystem = csf.CreateFromCode(code)
                    Dim meters As Double = geogCS.ConvertCoordinateSystemUnitsToMeters(1)

                    Dim metersPerUnit As Double = meters
                    'Dim DPI As Long = DPI
                    If DPI >= 600 Then DPI = 600
                    If DPI = 0 Then DPI = 600
                    'DPI = CInt(DPI)
                    'Dim metersPerPixel As Double = 0.0254 / DPI
                    'Dim mappingWidth As Double = p_imagewidth * metersPerPixel * Escala / metersPerUnit
                    'Dim mappingHeight As Double = p_imageheight * metersPerPixel * Escala / metersPerUnit
                    Dim imageWidth As Double = CInt(p_imagewidth * (DPI / 96))
                    Dim imageHeight As Double = CInt(p_imageheight * (DPI / 96))
                    p_imagewidth = imageWidth
                    p_imageheight = imageHeight

                    Dim url As String
                    url = ""
                    url += Servidor & "?OPERATION=GETMAPIMAGE"
                    url += "&VERSION=1.0.0"
                    url += "&CLIENTAGENT=G10"
                    url += "&LOCALE=en"
                    url += "&SESSION=" & mgSessionId
                    url += "&MAPNAME=" & MapaNome
                    If p_imagewidth > 4095 Or p_imagewidth > 4095 Then
                        url += "&FORMAT=JPG"
                    Else
                        url += "&FORMAT=PNG"
                    End If
                    url += "&SETDISPLAYWIDTH=" & p_imagewidth
                    url += "&SETDISPLAYHEIGHT=" & p_imageheight
                    url += "&SETDISPLAYDPI=" & DPI
                    url += "&SETVIEWSCALE=" & Escala
                    url += "&SETVIEWCENTERX=" & centroX
                    url += "&SETVIEWCENTERY=" & centroY
                    'url += "&SEQ=0.6"
                    url += "&KEEPSELECTION=1"


                    Dim codeBase As String = Reflection.Assembly.GetExecutingAssembly().CodeBase
                    Dim uri1 As UriBuilder = New UriBuilder(codeBase)
                    Dim path1 As String = Uri.UnescapeDataString(uri1.Path)
                    Dim logPath As String = IO.Path.GetDirectoryName(path1)

                    Try

                        Dim logFile As String = logPath & "\g10phPDF.log"
                        Dim prefix As String = Date.Now.ToString("yyyy/MM/dd HH:mm:ss - ")
                        Using writer As New IO.StreamWriter(logFile, True)
                            writer.WriteLine(prefix & url)
                        End Using
                    Catch ex1 As Exception

                    End Try

                    Return url

                    'Dim retVal As System.Drawing.Image = Nothing
                    'Try
                    '    Dim response As Net.WebResponse
                    '    Dim request As Net.HttpWebRequest = DirectCast(Net.HttpWebRequest.Create(url), Net.HttpWebRequest)

                    '    response = DirectCast(request.GetResponse(), Net.HttpWebResponse)
                    '    retVal = System.Drawing.Image.FromStream(response.GetResponseStream())

                    '    Dim ms As MemoryStream = New MemoryStream()
                    '    If p_imagewidth > 4095 Or p_imagewidth > 4095 Then
                    '        retVal.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
                    '    Else
                    '        retVal.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
                    '    End If

                    '    Dim aux As Byte() = ms.ToArray

                    '    Dim bb() As Byte

                    '    bb = aux
                    '    Dim mems As IO.MemoryStream

                    '    mems = New IO.MemoryStream(bb, 0, bb.Length)
                    '    mems.Write(bb, 0, bb.Length)
                    '    Dim aux_bmp As New Bitmap(CInt(p_imagewidth), CInt(p_imageheight))
                    '    aux_bmp.SetResolution(300, 300)
                    '    Dim g As Graphics = Graphics.FromImage(aux_bmp)

                    '    Dim layer As New Bitmap(mems)

                    '    g.DrawImage(layer, 0, 0, CInt(p_imagewidth), CInt(p_imageheight))

                    '    Dim sFN As String
                    '    Dim tF As String = Path.GetTempPath()
                    '    If Right(tF, 1) <> "\" Then tF &= "\"
                    '    If CInt(Tamanho.Width) > 4095 Or CInt(Tamanho.Height) > 4095 Then
                    '        sFN = tF & Guid.NewGuid.ToString & ".jpg"
                    '    Else
                    '        sFN = tF & Guid.NewGuid.ToString & ".png"
                    '    End If
                    '    If p_imagewidth > 4095 Or p_imageheight > 4095 Then
                    '        aux_bmp.Save(sFN, Imaging.ImageFormat.Jpeg)

                    '        System.Drawing.Image.FromStream(mems, True).Save(sFN & "2.jpg", Imaging.ImageFormat.Jpeg)
                    '    Else
                    '        aux_bmp.Save(sFN, Imaging.ImageFormat.Png)

                    '        System.Drawing.Image.FromStream(mems, True).Save(sFN & "2.png", Imaging.ImageFormat.Png)
                    '    End If
                    '    Return aux_bmp
                    'Catch generatedExceptionName As Exception
                    '    retVal = Nothing
                    '    Return Nothing
                    'End Try

                Catch ex As Exception

                    Dim a As New MapGuideApi

                    Dim fullPath As String = System.Reflection.Assembly.GetAssembly(a.GetType()).Location
                    Dim theDirectory As String = IO.Path.GetDirectoryName(fullPath)
                    Dim mapguideapi As String = "MapGuideApi : " & theDirectory


                    Dim totalex As String = mapguideapi & vbCrLf & webConfig & vbCrLf & ex.Message.ToString
                    If ex.InnerException IsNot Nothing Then
                        totalex = totalex & vbCrLf & " inner: " & ex.InnerException.Message.ToString
                    End If
                    If ex.InnerException.InnerException IsNot Nothing Then
                        totalex = totalex & vbCrLf & " inner2: " & ex.InnerException.InnerException.Message.ToString
                    End If
                    Dim codeBase As String = Reflection.Assembly.GetExecutingAssembly().CodeBase
                    Dim uri1 As UriBuilder = New UriBuilder(codeBase)
                    Dim path1 As String = Uri.UnescapeDataString(uri1.Path)
                    Dim logPath As String = IO.Path.GetDirectoryName(path1)

                    Try

                        Dim logFile As String = logPath & "\g10phPDF.log"
                        Dim prefix As String = Date.Now.ToString("yyyy/MM/dd HH:mm:ss - ")
                        Using writer As New IO.StreamWriter(logFile, True)
                            writer.WriteLine(prefix & totalex.ToString)
                        End Using
                    Catch ex1 As Exception

                    End Try
                End Try
            End SyncLock
        End Function

        Private Function Pvt_ResizeImage(ByVal image As Image, ByVal size As Size, Optional ByVal preserveAspectRatio As Boolean = True) As Image
            Dim newWidth As Integer
            Dim newHeight As Integer
            If preserveAspectRatio Then
                Dim originalWidth As Integer = image.Width
                Dim originalHeight As Integer = image.Height
                Dim percentWidth As Single = CSng(size.Width) / CSng(originalWidth)
                Dim percentHeight As Single = CSng(size.Height) / CSng(originalHeight)
                Dim percent As Single = If(percentHeight < percentWidth, percentHeight, percentWidth)
                newWidth = CInt(originalWidth * percent)
                newHeight = CInt(originalHeight * percent)
            Else
                newWidth = size.Width
                newHeight = size.Height
            End If
            Dim newImage As Image = New Bitmap(newWidth, newHeight)
            Using graphicsHandle As Graphics = Graphics.FromImage(newImage)
                graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic
                graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight)
            End Using
            Return newImage
        End Function
        Private Function Pvt_JoinImages(Image1 As Bitmap, Image2 As Bitmap, Location As String) As Bitmap
            Dim methodName As String = "Pvt_JoinImages"
            Dim methodNameLog As String = methodName

            Try
                Select Case UCase(Location)
                    Case "TOP"
                        Dim Result As New Bitmap(Math.Max(Image1.Width, Image2.Width), Image1.Height + Image2.Height)
                        Dim gResult As Graphics = Graphics.FromImage(Result)
                        gResult.DrawImage(Image1, New Point(0, 0))
                        gResult.DrawImage(Image2, New Point(0, Image1.Height))
                        gResult.Dispose()
                        gResult = Nothing
                        Return Result
                    Case "BOTTOM"
                        Dim Result As New Bitmap(Math.Max(Image1.Width, Image2.Width), Image1.Height + Image2.Height)
                        Dim gResult As Graphics = Graphics.FromImage(Result)
                        gResult.DrawImage(Image2, New Point(0, 0))
                        gResult.DrawImage(Image1, New Point(0, Image2.Height))
                        gResult.Dispose()
                        gResult = Nothing
                        Return Result

                    Case "LEFT"

                        Dim Result As New Bitmap(Image1.Width + Image2.Width, Math.Max(Image1.Height, Image2.Height))
                        Dim gResult As Graphics = Graphics.FromImage(Result)
                        gResult.DrawImage(Image2, New Point(0, 0))
                        gResult.DrawImage(Image1, New Point(Image2.Width, 0))
                        gResult.Dispose()
                        gResult = Nothing
                        Return Result

                    Case "RIGHT"
                        Dim Result As New Bitmap(Image1.Width + Image2.Width, Math.Max(Image1.Height, Image2.Height))
                        Dim gResult As Graphics = Graphics.FromImage(Result)
                        gResult.DrawImage(Image1, New Point(0, 0))
                        gResult.DrawImage(Image2, New Point(Image1.Width, 0))
                        gResult.Dispose()
                        gResult = Nothing
                        Return Result
                End Select

            Catch ex As Exception
                Dim totalex As String = ex.Message.ToString
                If ex.InnerException IsNot Nothing Then
                    totalex = totalex & " inner: " & ex.InnerException.Message.ToString
                End If

                Return Nothing
            End Try
            Return Nothing
        End Function

        Public Function GetMapa(ByVal BBox As Mapframe.IEXTENSÃO_, ByVal Tamanho As System.Drawing.Size) As System.Drawing.Image
            SyncLock _locker
                Dim K As Integer = 0
                Dim mapid As MgResourceIdentifier

                Try
                    MapGuideApi.MgInitializeWebTier(webConfig)

                    Dim url As String

                    Dim userInfo As MgUserInformation = New MgUserInformation(userName, userPassword)
                    Dim site As MgSite = New MgSite()

                    site.Open(userInfo)
                    Dim mgSessionId As String = site.CreateSession()

                    Dim userInfoSession As MgUserInformation = New MgUserInformation(mgSessionId)
                    Dim siteConnection As MgSiteConnection = New MgSiteConnection()
                    siteConnection.Open(userInfo)

                    Dim resourceId As MgResourceIdentifier = New MgResourceIdentifier(Mapa)

                    Dim resourceSrvc As MgResourceService
                    resourceSrvc = siteConnection.CreateService(MgServiceType.ResourceService)

                    Dim map As MgMap = New MgMap(siteConnection)
                    map.Create(resourceSrvc, resourceId, MapaNome)
                    mapid = New MgResourceIdentifier("Session:" & mgSessionId & "//" & MapaNome & "." & MgResourceType.Map)
                    map.Save(resourceSrvc, mapid)

                    'Dim userInfo1 As New MgUserInformation(mgSessionId)
                    'Dim siteConnection1 As New MgSiteConnection()
                    'siteConnection1.Open(userInfo1)

                    Dim resourceService As MgResourceService = DirectCast(siteConnection.CreateService(MgServiceType.ResourceService), MgResourceService)
                    Dim featureService As MgFeatureService = DirectCast(siteConnection.CreateService(MgServiceType.FeatureService), MgFeatureService)

                    'Dim map1 As New MgMap(siteConnection1)
                    'map1.Open(MapaNome)
                    If Pretensao Then

                        Dim xmlDoc As System.Xml.XmlDocument = New System.Xml.XmlDocument()
                        xmlDoc.Load(LayerTempXML)

                        Dim sw As IO.StringWriter = New IO.StringWriter()
                        Dim xw As System.Xml.XmlTextWriter = New System.Xml.XmlTextWriter(sw)

                        xmlDoc.WriteTo(xw)

                        Dim layerdefinition As String = sw.ToString()
                        layerdefinition = layerdefinition.Replace("%featuresource%", LayerTempFeatureSourceName)
                        layerdefinition = layerdefinition.Replace("%filter%", "pedidoid=" & PedidoId & "")

                        Dim encoding As New System.Text.UTF8Encoding()
                        Dim barray As Byte() = encoding.GetBytes(layerdefinition)

                        Dim byteSource1 As New MgByteSource(barray, Len(layerdefinition))
                        byteSource1.SetMimeType(MgMimeType.Xml)
                        Dim layerResourceID As New MgResourceIdentifier("Session:" & mgSessionId & "//" & "temp" & "." & MgResourceType.LayerDefinition)
                        Dim nullreader As MgByteReader
                        resourceService.SetResource(layerResourceID, byteSource1.GetReader, nullreader)

                        Dim mglayernovo As New MgLayer(layerResourceID, resourceService)
                        mglayernovo.SetLegendLabel("Temp")
                        mglayernovo.SetName("Temp")
                        mglayernovo.SetDisplayInLegend(True)
                        mglayernovo.SetSelectable(True)
                        mglayernovo.SetVisible(True)
                        mglayernovo.ForceRefresh()

                        Dim layerCollection As MgLayerCollection = map.GetLayers()
                        For Each layer As MgLayer In layerCollection
                            If layer.Name = "Temp" Then
                                layerCollection.Remove(layer)
                            End If
                        Next

                        layerCollection.Insert(0, mglayernovo)
                    End If
                    map.Save()

                    Dim DistX As Double
                    Dim DistY As Double
                    DistX = Math.Abs((BBox.VerticeSW.Lon - BBox.VerticeNE.Lon) / 2)
                    DistY = Math.Abs((BBox.VerticeSW.Lat - BBox.VerticeNE.Lat) / 2)
                    Dim centroX As Double = BBox.VerticeNE.Lon - DistX
                    Dim centroY As Double = BBox.VerticeNE.Lat - DistY
                    Dim p_imagewidth As Long = Tamanho.Width '(Tamanho.Width * Factor)
                    Dim p_imageheight As Long = Tamanho.Height '(Tamanho.Height * Factor)
                    ' ================================================================================================================
                    Dim byteReader As MgByteReader
                    Dim renderingService As MgRenderingService = siteConnection.CreateService(MgServiceType.RenderingService)

                    Dim _actual_selection As MgSelection
                    _actual_selection = New MgSelection(map)
                    Dim geoFact As MgGeometryFactory = New MgGeometryFactory()
                    Dim center As MgCoordinate = geoFact.CreateCoordinateXY(centroX, centroY)

                    'Dim p_imagewidth As Long = Tamanho.Width '(Tamanho.Width * Factor)
                    'Dim p_imageheight As Long = Tamanho.Height '(Tamanho.Height * Factor)

                    p_imagewidth = p_imagewidth * 0.66 * Factor
                    p_imageheight = p_imageheight * 0.66 * Factor
                    'Dim csf As New MgCoordinateSystemFactory

                    'Dim srs As String = map.GetMapSRS()
                    'Dim code As String = csf.ConvertWktToCoordinateSystemCode(srs)
                    'Dim epsgcode As String = csf.ConvertWktToEpsgCode(srs)
                    'Dim rs As New g10GeoMetadata.mtdReferenceSystemInfo(srs, csf.CreateFromCode(code))
                    'rs.Code = epsgcode
                    'rs.CodeSpace = "EPSG"

                    'Dim geogCS As MgCoordinateSystem = csf.CreateFromCode(code)
                    'Dim meters As Double = geogCS.ConvertCoordinateSystemUnitsToMeters(1)

                    'Dim metersPerUnit As Double = meters
                    'Dim DPI As Long = 300
                    ''If DPI >= 600 Then DPI = 600
                    ''DPI = CInt(DPI)
                    ''Dim metersPerPixel As Double = 0.0254 / DPI
                    ''Dim mappingWidth As Double = p_imagewidth * metersPerPixel * Escala / metersPerUnit
                    ''Dim mappingHeight As Double = p_imageheight * metersPerPixel * Escala / metersPerUnit
                    'Dim imageWidth As Double = CInt(p_imagewidth * (DPI / 96))
                    'Dim imageHeight As Double = CInt(p_imageheight * (DPI / 96))
                    'p_imagewidth = imageWidth
                    'p_imageheight = imageHeight


                    Dim color As MgColor = New MgColor(255, 255, 255)

                    If (p_imagewidth * Factor) > 4095 Or (p_imageheight * Factor) > 4095 Then

                        ' TODO: tipo de imagem PNG ou JPG deve ser passado como parâmetro, vindo talvez de um ficheiro de configuração web.config ou alike. ya??
                        byteReader = renderingService.RenderMap(map, _actual_selection, center, Escala / Factor, p_imagewidth * Factor, p_imageheight * Factor, color, "JPG", True)

                    Else

                        ' TODO: tipo de imagem PNG ou JPG deve ser passado como parâmetro, vindo talvez de um ficheiro de configuração web.config ou alike. ya??
                        byteReader = renderingService.RenderMap(map, _actual_selection, center, Escala / Factor, p_imagewidth * Factor, p_imageheight * Factor, color, "PNG", True)

                    End If


                    Dim b(byteReader.GetLength) As Byte
                    Dim contador As Integer = 1
                    Dim tmp(2048) As Byte

                    Dim st As Integer = 0

                    While contador > 0
                        contador = byteReader.Read(tmp, 2048)
                        If contador > 0 Then
                            Buffer.BlockCopy(tmp, 0, b, st, contador)
                            st += contador
                        End If
                    End While

                    Dim imagem_ori As System.Drawing.Image
                    Dim imagem_dst As System.Drawing.Bitmap



                    Dim mems As IO.MemoryStream
                    mems = New IO.MemoryStream(b, 0, b.Length)
                    mems.Write(b, 0, b.Length)
                    imagem_ori = System.Drawing.Image.FromStream(mems, True)


                    imagem_dst = imagem_ori
                    ' =======================================================================
                    Dim tF As String = Path.GetTempPath()

                    If Right(tF, 1) <> "\" Then tF &= "\"
                    Dim sFN As String
                    If CInt(Tamanho.Width) > 4095 Or CInt(Tamanho.Height) > 4095 Then
                        sFN = tF & Guid.NewGuid.ToString & ".jpg"
                    Else
                        sFN = tF & Guid.NewGuid.ToString & ".png"
                    End If

                    Dim aux_bmp As New Drawing.Bitmap(CInt(Tamanho.Width), CInt(Tamanho.Height))
                    'Dim imgFormat = aux_bmp.RawFormat
                    aux_bmp.SetResolution(300, 300)
                    Dim g As Graphics = Graphics.FromImage(aux_bmp)

                    Dim layerimg As New Bitmap(mems)

                    g.DrawImage(layerimg, 0, 0, CInt(Tamanho.Width), CInt(Tamanho.Height))

                    If CInt(Tamanho.Width) > 4095 Or CInt(Tamanho.Height) > 4095 Then
                        aux_bmp.Save(sFN, Imaging.ImageFormat.Jpeg)
                        System.Drawing.Image.FromStream(mems, True).Save(sFN & "2.jpg", Imaging.ImageFormat.Jpeg)

                    Else
                        aux_bmp.Save(sFN, Imaging.ImageFormat.Png)
                        System.Drawing.Image.FromStream(mems, True).Save(sFN & "2.png", Imaging.ImageFormat.Png)

                    End If
                    DistX = Math.Abs((BBox.VerticeSW.Lon - BBox.VerticeNE.Lon) / 4)
                    DistY = Math.Abs((BBox.VerticeSW.Lat - BBox.VerticeNE.Lat) / 4)

                    Dim centroX1 As Double = BBox.VerticeNE.Lon - DistX
                    Dim centroY1 As Double = BBox.VerticeNE.Lat - DistY
                    Dim center1 As MgCoordinate = geoFact.CreateCoordinateXY(centroX1, centroY1)

                    Dim centroX2 As Double = BBox.VerticeNE.Lon - DistX
                    Dim centroY2 As Double = BBox.VerticeNE.Lat - DistY * 3
                    Dim center2 As MgCoordinate = geoFact.CreateCoordinateXY(centroX2, centroY)

                    Dim centroX3 As Double = BBox.VerticeNE.Lon - DistX * 3
                    Dim centroY3 As Double = BBox.VerticeNE.Lat - DistY
                    Dim center3 As MgCoordinate = geoFact.CreateCoordinateXY(centroX3, centroY3)

                    Dim centroX4 As Double = BBox.VerticeNE.Lon - DistX * 3
                    Dim centroY4 As Double = BBox.VerticeNE.Lat - DistY * 3
                    Dim center4 As MgCoordinate = geoFact.CreateCoordinateXY(centroX, centroY)

                    Return aux_bmp
                    'imagem_dst = aux_bmp
                    ' ================================================================================================================

                    'Dim url As String = Servidor & "?REQUEST=MAP" & _
                    '                                "&RELOAD=TRUE" & _
                    '                                "&WIDTH=" & (Tamanho.Width * Factor) & _
                    '                                "&HEIGHT=" & (Tamanho.Height * Factor) + K & _
                    '                                "&FORMAT=PNG&LAYERS=" & Mwf & _
                    '                                "&BBOX=" & Basframe.NSF(BBox.VerticeSW.Lon) & "," & _
                    '                                Basframe.NSF(BBox.VerticeSW.Lat) & "," & _
                    '                                Basframe.NSF(BBox.VerticeNE.Lon) & "," & _
                    '                                Basframe.NSF(BBox.VerticeNE.Lat)



                    'url = Servidor & "?OPERATION=GETMAPIMAGE"
                    'url += "&VERSION=1.0.0"
                    'url += "&CLIENTAGENT=G10phPDF"
                    'url += "&LOCALE=en"
                    'url += "&SESSION="
                    'url += "&MAPNAME=" & map1.Name
                    'If (Tamanho.Width * Factor) > 4095 Or (Tamanho.Width * Factor) > 4095 Then
                    '    url += "&FORMAT=JPG"
                    'Else
                    '    url += "&FORMAT=PNG"
                    'End If
                    'url += "&MAPDEFINITION=" & mapid.ToString
                    'url += "&SELECTION="
                    'url += "&SETDISPLAYWIDTH=" & (Tamanho.Width * Factor)
                    'url += "&SETDISPLAYHEIGHT=" & (Tamanho.Height * Factor)
                    'url += "&SETDISPLAYDPI=75" ' & DPI
                    'url += "&SETVIEWSCALE=" & Escala
                    'url += "&SETVIEWCENTERX=" & (Basframe.NSF(centroX))
                    'url += "&SETVIEWCENTERY=" & (Basframe.NSF(centroY))
                    'url += "&KEEPSELECTION=1"


                    'url = Servidor & "?OPERATION=GETMAPIMAGE"
                    'url += "&VERSION=1.0.0"
                    'url += "&CLIENTAGENT=G10phPDF"
                    'url += "&LOCALE=en"
                    'url += "&SESSION=" & map1.SessionId
                    'url += "&MAPNAME=" & map1.Name
                    'If (Tamanho.Width * Factor) > 4095 Or (Tamanho.Width * Factor) > 4095 Then
                    '    url += "&FORMAT=JPG"
                    'Else
                    '    url += "&FORMAT=PNG"
                    'End If
                    'url += "&SETDISPLAYWIDTH=" & (Tamanho.Width * Factor)
                    'url += "&SETDISPLAYHEIGHT=" & (Tamanho.Height * Factor)
                    'url += "&SETDISPLAYDPI=75" ' & DPI
                    'url += "&SETVIEWSCALE=" & Escala
                    'url += "&SETVIEWCENTERX=" & (Basframe.NSF(centroX))
                    'url += "&SETVIEWCENTERY=" & (Basframe.NSF(centroY))
                    'url += "&KEEPSELECTION=1"


                    'url = ""
                    'url += Servidor & "?OPERATION=GETMAPIMAGE"
                    'url += "&VERSION=1.0.0"
                    'url += "&CLIENTAGENT=G10"
                    'url += "&LOCALE=en"
                    'url += "&SESSION=" & mgSessionId
                    'url += "&MAPNAME=" & map.Name
                    'If p_imagewidth > 4095 Or p_imagewidth > 4095 Then
                    '    url += "&FORMAT=JPG"
                    'Else
                    '    url += "&FORMAT=PNG"
                    'End If
                    'url += "&SETDISPLAYWIDTH=" & p_imagewidth
                    'url += "&SETDISPLAYHEIGHT=" & p_imageheight
                    'url += "&SETDISPLAYDPI=" & 300
                    'url += "&SETVIEWSCALE=" & Escala
                    'url += "&SETVIEWCENTERX=" & centroX
                    'url += "&SETVIEWCENTERY=" & centroY
                    ''url += "&SEQ=0.6"
                    'url += "&KEEPSELECTION=1"

                    'Dim retVal As System.Drawing.Image = Nothing
                    'Try
                    '    Dim response As Net.WebResponse
                    '    Dim request As Net.HttpWebRequest = DirectCast(Net.HttpWebRequest.Create(url), Net.HttpWebRequest)

                    '    response = DirectCast(request.GetResponse(), Net.HttpWebResponse)
                    '    retVal = System.Drawing.Image.FromStream(response.GetResponseStream())

                    '    Dim ms As MemoryStream = New MemoryStream()
                    '    If p_imagewidth > 4095 Or p_imagewidth > 4095 Then
                    '        retVal.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
                    '    Else
                    '        retVal.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
                    '    End If
                    '    'retVal.Save(ms, System.Drawing.Imaging.ImageFormat.Png)

                    '    Dim geoFact As MgGeometryFactory = New MgGeometryFactory()
                    '    Dim center As MgCoordinate = geoFact.CreateCoordinateXY(centroX, centroY)

                    '    Dim aux As Byte() = ms.ToArray

                    '    Dim bb() As Byte

                    '    bb = aux
                    '    Dim mems As IO.MemoryStream

                    '    mems = New IO.MemoryStream(bb, 0, bb.Length)
                    '    mems.Write(bb, 0, bb.Length)
                    '    Dim aux_bmp As New Bitmap(CInt(p_imagewidth), CInt(p_imageheight))
                    '    aux_bmp.SetResolution(300, 300)
                    '    Dim g As Graphics = Graphics.FromImage(aux_bmp)

                    '    Dim layer As New Bitmap(mems)

                    '    g.DrawImage(layer, 0, 0, CInt(p_imagewidth), CInt(p_imageheight))

                    '    Dim sFN As String
                    '    Dim tF As String = Path.GetTempPath()
                    '    If Right(tF, 1) <> "\" Then tF &= "\"
                    '    If CInt(Tamanho.Width) > 4095 Or CInt(Tamanho.Height) > 4095 Then
                    '        sFN = tF & Guid.NewGuid.ToString & ".jpg"
                    '    Else
                    '        sFN = tF & Guid.NewGuid.ToString & ".png"
                    '    End If
                    '    If p_imagewidth > 4095 Or p_imageheight > 4095 Then
                    '        aux_bmp.Save(sFN, Imaging.ImageFormat.Jpeg)

                    '        System.Drawing.Image.FromStream(mems, True).Save(sFN & "2.jpg", Imaging.ImageFormat.Jpeg)
                    '    Else
                    '        aux_bmp.Save(sFN, Imaging.ImageFormat.Png)

                    '        System.Drawing.Image.FromStream(mems, True).Save(sFN & "2.png", Imaging.ImageFormat.Png)
                    '    End If
                    'Return aux_bmp
                    'Catch generatedExceptionName As Exception
                    '    retVal = Nothing
                    '    Return Nothing
                    'End Try

                Catch ex As Exception

                    Dim a As New MapGuideApi

                    Dim fullPath As String = System.Reflection.Assembly.GetAssembly(a.GetType()).Location
                    Dim theDirectory As String = IO.Path.GetDirectoryName(fullPath)
                    Dim mapguideapi As String = "MapGuideApi : " & theDirectory


                    Dim totalex As String = mapguideapi & vbCrLf & webConfig & vbCrLf & ex.Message.ToString
                    If ex.InnerException IsNot Nothing Then
                        totalex = totalex & vbCrLf & " inner: " & ex.InnerException.Message.ToString
                    End If
                    If ex.InnerException.InnerException IsNot Nothing Then
                        totalex = totalex & vbCrLf & " inner2: " & ex.InnerException.InnerException.Message.ToString
                    End If
                    Dim codeBase As String = Reflection.Assembly.GetExecutingAssembly().CodeBase
                    Dim uri1 As UriBuilder = New UriBuilder(codeBase)
                    Dim path1 As String = Uri.UnescapeDataString(uri1.Path)
                    Dim logPath As String = IO.Path.GetDirectoryName(path1)

                    Try

                        Dim logFile As String = logPath & "\g10phPDF.log"
                        Dim prefix As String = Date.Now.ToString("yyyy/MM/dd HH:mm:ss - ")
                        Using writer As New IO.StreamWriter(logFile, True)
                            writer.WriteLine(prefix & totalex.ToString)
                        End Using
                    Catch ex1 As Exception

                    End Try
                End Try
            End SyncLock
        End Function

        Public Function PrintMapa(ByVal BBox As Mapframe.IEXTENSÃO_, ByVal AreaDestino As System.Drawing.Rectangle) As Boolean

            SyncLock _locker
                Dim imagem As System.Drawing.Image

                imagem = Me.GetMapa(BBox, AreaDestino.Size)

                If Not imagem Is Nothing Then

                    Dim objid As Integer = l_documento.StackObjAdd(Nothing, imagem)

                    l_documento.InstruçãoAdd(Report.LDDStandard.setx1, AreaDestino.X.ToString)
                    l_documento.InstruçãoAdd(Report.LDDStandard.sety1, AreaDestino.Y.ToString)
                    l_documento.InstruçãoAdd(Report.LDDStandard.setx2, AreaDestino.Width.ToString)
                    l_documento.InstruçãoAdd(Report.LDDStandard.sety2, AreaDestino.Height.ToString)
                    l_documento.InstruçãoAdd(Report.LDDStandard.obj, objid.ToString)
                    l_documento.InstruçãoAdd(Report.LDDStandard.pic, Nothing)

                    Return True

                Else

                    Return False

                End If
            End SyncLock
        End Function

        Public Sub DesenhaObjecto(ByVal Mapa As System.Drawing.Image, ByVal BBox As Mapframe.IEXTENSÃO_, ByVal Objecto As Mapframe.IOBJECTO)

            ' todo: ParserLDD2LiteView: desenha a geometria coorrespondente ao objecto sobre o mapa tendo em considerações as coordenadas de ambos

        End Sub

        Private Function SacaImagem(ByVal url As String) As System.Drawing.Image
            SyncLock _locker
                Dim wreq As System.Net.WebRequest = System.Net.WebRequest.Create(url)
                Dim hresp As System.Net.HttpWebResponse = CType(wreq.GetResponse, System.Net.HttpWebResponse)
                Dim saida As System.IO.Stream = hresp.GetResponseStream
                Return System.Drawing.Image.FromStream(saida)
            End SyncLock
        End Function

    End Class

End Namespace
