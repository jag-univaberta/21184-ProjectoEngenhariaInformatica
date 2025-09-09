Imports O2S.Components
Imports O2S.Components.PDF4NET
Imports O2S.Components.PDF4NET.Graphics
Imports O2S.Components.PDF4NET.Graphics.Shapes
Imports O2S.Components.PDF4NET.Graphics.Fonts
Imports O2S.Components.PDF4NET.Forms
Imports O2S.Components.PDF4NET.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.IO

Namespace PDF

    Public Class ProcLDD2PDF
        Inherits g10phPDF4.Report.ProcLDDBase

        Public Ficheiro As String
        Public FicheiroAppend As Boolean

        Public pdfDOC As PDF4NET.PDFDocument
        Public pdfPAGE As PDF4NET.PDFPage

        Private Shared ReadOnly _locker As New Object()
        Public Function StreamToByteArray(inputStream As Stream) As Byte()
            Dim bytes = New Byte(16383) {}
            Using memoryStream = New MemoryStream()
                Dim count As Integer
                While ((count = inputStream.Read(bytes, 0, bytes.Length)) > 0)
                    memoryStream.Write(bytes, 0, count)
                End While
                Return memoryStream.ToArray()
            End Using
        End Function

        Public Shared Function ExtractResource(ByVal filename As String) As Byte()
            Dim a As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
            Using resFilestream As Stream = a.GetManifestResourceStream(filename)
                If resFilestream Is Nothing Then Return Nothing
                Dim ba As Byte() = New Byte(resFilestream.Length - 1) {}
                resFilestream.Read(ba, 0, ba.Length)
                Return ba
            End Using
        End Function
        Public Overloads Overrides Function RealizaDocumento() As Boolean
            SyncLock _locker
                If System.IO.File.Exists(Ficheiro) And FicheiroAppend Then
                    Me.pdfDOC = New PDF4NET.PDFDocument(Ficheiro)
                Else
                    Me.pdfDOC = New PDF4NET.PDFDocument
                End If
                pdfDOC.SerialNumber = "PDF4NET-F20MI-0WB88-XP65J-5XRV7-VKDQ4"
                Dim a As New PDFDocumentInformation
                pdfDOC.DocumentInformation = a
                If Not l_processo.Documento.Autor Is Nothing Then pdfDOC.DocumentInformation.Author = l_processo.Documento.Autor
                If Not l_processo.Documento.Titulo Is Nothing Then pdfDOC.DocumentInformation.Title = l_processo.Documento.Titulo
                If Not l_processo.Documento.Assunto Is Nothing Then pdfDOC.DocumentInformation.Subject = l_processo.Documento.Assunto
                If Not l_processo.Documento.Chaves Is Nothing Then pdfDOC.DocumentInformation.Keywords = l_processo.Documento.Chaves
                pdfDOC.DocumentInformation.Creator = ""
                Dim b As New PDFViewerPreferences
                pdfDOC.ViewerPreferences = b
                pdfDOC.ViewerPreferences.HideMenubar = False
                pdfDOC.ViewerPreferences.HideToolbar = False
                pdfDOC.ViewerPreferences.HideWindowUI = False
                pdfDOC.ViewerPreferences.DisplayDocTitle = True
                pdfDOC.Metadata = New PDFXMPMetadata()
                pdfDOC.PageOrientation = l_orientaçãoLDD2O2S(l_processo.Documento.Setup.Orientação)
                If l_processo.Documento.Setup.Forma = g10phPDF4.Report.PaginaForma.NãoDefinido Then
                    pdfDOC.PageWidth = l_processo.Documento.Setup.Dimensão.Width
                    pdfDOC.PageHeight = l_processo.Documento.Setup.Dimensão.Height
                Else
                    pdfDOC.PageSize = l_formaLDD2O2S(l_processo.Documento.Setup.Forma)
                End If
                If l_processo.Documento.Mostra_NumeroPagina Then
                    Dim pnr1 As PDFPageNumberingRange = New PDFPageNumberingRange()
                    pnr1.StartPageIndex = 0
                    pnr1.StartPageNumber = l_processo.Documento.NumeroPagina_Inicial
                    pnr1.NumberingStyle = PDF4NET.PDFNumberingStyle.Arabic
                    pnr1.PageNumberPrefix = l_processo.Documento.NumeroPagina_Prefixo
                    If l_processo.Documento.NumeroPagina_Total Then
                        pnr1.PageNumberSuffix = "/" & l_processo.Documento.NumeroPagina_TotalValor
                    End If
                    pdfDOC.PageNumberingRanges.Add(pnr1)

                    Dim fontNumber As New PDFFont(PDFFontFace.Helvetica, l_processo.Documento.NumeroPagina_FontSize)
                    Dim blackBrush As New PDFBrush(New PDFRgbColor())
                    'Dim pen As PDFPen = New PDFPen(New PDFRgbColor(), 1)
                    Dim pn As New PDFPageNumber()
                    pn.Font = fontNumber
                    pn.Brush = blackBrush
                    ' Print the page number centered at the bottom of the page
                    pn.Align = PDFTextAlign.BottomCenter
                    ' The page number will be printed 10 points above the bottom margin.

                    pn.Align = l_processo.Documento.NumeroPagina_Alinhamento
                    pn.VerticalOffset = l_processo.Documento.NumeroPagina_OffsetVertical
                    pn.HorizontalOffset = l_processo.Documento.NumeroPagina_OffsetHorizontal

                    ' pn.Pen = pen
                    ' Set the page number visual aspect for all the pages in the document
                    ' by attaching it to the document.
                    pdfDOC.PageNumber = pn
                End If

                MyBase.RealizaDocumento()

                'pdfDOC.SaveToFile(Ficheiro)
                'Reflection.Assembly.GetExecutingAssembly().Location
                Try

                Catch ex As Exception

                End Try
                'Dim _imageStream As Stream
                'Dim _textStreamReader As StreamReader
                'Dim _assembly As Reflection.[Assembly]
                '_assembly = Reflection.[Assembly].GetExecutingAssembly()

                '_textStreamReader = New StreamReader(_assembly.GetManifestResourceStream("g10phPDF4.rgb.icc"))

                Dim profileData As Byte() = ExtractResource("g10phPDF4.rgb.icc")


                ' Read the ICC profile from disk.
                'Dim fs As New FileStream("C:\inetpub\wwwroot\EPL2017_2\ModeloGeral\rgb.icc", FileMode.Open, FileAccess.Read)
                'Dim profileData As Byte() = New Byte(fs.Length) {}
                'fs.Read(profileData, 0, profileData.Length)
                'fs.Close()

                ' Create a RGB output intent and attach it to the document.
                Dim icc As New ColorSpaces.PDFICCColorSpace()
                icc.ProfileData = profileData
                Dim oi As New PDFOutputIntent()
                oi.Type = PDFOutputIntentType.PDFA1
                oi.Info = "sRGB IEC61966-2.1"
                oi.OutputConditionIdentifier = "Custom"
                oi.DestinationOutputProfile = icc

                pdfDOC.OutputIntents = New PDFOutputIntentCollection()
                pdfDOC.OutputIntents.Add(oi)

                ' Save the document in PDF/A-1b format.
                Dim dso As New PDFDocumentSaveOptions()
                dso.SaveFormat = PDFDocumentSaveFormat.PDFA1b
                Try
                    pdfDOC.Save(Ficheiro, dso)
                Catch ex As Exception
                    pdfDOC.Save(Ficheiro)
                End Try
                'pdfDOC.Save(Ficheiro, dso)
                'pdfDOC.Save(Ficheiro)
            End SyncLock
        End Function

        Public Overloads Overrides Function RealizaPagina() As Boolean
            SyncLock _locker
                pdfPAGE = pdfDOC.AddPage
                pdfPAGE.Canvas.SetLogicalUnit(New PDFPointLogicalUnit())

                MyBase.RealizaPagina()
            End SyncLock
        End Function

        Public Overloads Overrides Function RealizaInstrução() As Boolean
            SyncLock _locker
                Dim instrução As g10phPDF4.Report.LDDInstrução

                If Not MyBase.RealizaInstrução() Then

                    instrução = l_processo.Documento.Paginas(l_processo.PaginaActual).Instruções(l_processo.InstruçãoActual)

                    Select Case instrução.Codigo

                        Case g10phPDF4.Report.LDDStandard.print

                            'Dim f As New PDF4NET.Graphics.PDFFont(PDF4NET.Graphics.FontFace.Courier, 10)

                            ' Dim fonte As PDF4NET.Graphics.PDFFont
                            Dim fonte As PDF4NET.Graphics.Fonts.PDFFontBase
                            fonte = l_fontNET2OS(l_processo.fonte_nome, l_processo.fonte_tamanho)
                            fonte.Size = l_processo.fonte_tamanho
                            fonte.Bold = l_processo.fonte_bold
                            fonte.Italic = l_processo.fonte_italic
                            fonte.Underline = l_processo.fonte_underline
                            fonte.Strikethrough = l_processo.fonte_strike
                            fonte.Overline = l_processo.fonte_overline
                            Dim pen As New PDF4NET.Graphics.PDFPen(l_corNET2O2S(l_processo.fonte_cor))
                            Dim brush As New PDF4NET.Graphics.PDFBrush(l_corNET2O2S(l_processo.fonte_cor))

                            pdfPAGE.Canvas.DrawText(l_processo.str, fonte, Nothing, brush, l_processo.posx, l_processo.posy, l_processo.fonte_angulo, l_alinhaLDD2OS(l_processo.fonte_alinhamento))

                        Case g10phPDF4.Report.LDDStandard.printbox

                            'Dim fonte As PDF4NET.Graphics.PDFFont
                            Dim fonte As PDF4NET.Graphics.Fonts.PDFFontBase
                            fonte = l_fontNET2OS(l_processo.fonte_nome, l_processo.fonte_tamanho)
                            fonte.Size = l_processo.fonte_tamanho
                            fonte.Bold = l_processo.fonte_bold
                            fonte.Italic = l_processo.fonte_italic
                            fonte.Underline = l_processo.fonte_underline
                            fonte.Strikethrough = l_processo.fonte_strike
                            fonte.Overline = l_processo.fonte_overline
                            Dim pen As New PDF4NET.Graphics.PDFPen(l_corNET2O2S(l_processo.fonte_cor))
                            Dim brush As New PDF4NET.Graphics.PDFBrush(l_corNET2O2S(l_processo.fonte_cor))

                            pdfPAGE.Canvas.DrawTextBox(l_processo.str, fonte, Nothing, brush, l_processo.x1, l_processo.y1, l_processo.x2 - l_processo.x1, l_processo.y2 - l_processo.y1, l_processo.fonte_angulo, l_alinhaLDD2OS(l_processo.fonte_alinhamento), True)

                        Case g10phPDF4.Report.LDDStandard.pic

                            Dim imagem As System.Drawing.Image

                            If l_processo.objecto Is Nothing Then
                                Dim pen As New PDF4NET.Graphics.PDFPen(l_corNET2O2S(l_processo.tinta))
                                pdfPAGE.Canvas.DrawRectangle(pen, l_processo.y1, l_processo.x1, l_processo.x2 - l_processo.x1, l_processo.y2 - l_processo.y1)
                                pdfPAGE.Canvas.DrawLine(pen, l_processo.y1, l_processo.x1, l_processo.y2, l_processo.x2)
                                pdfPAGE.Canvas.DrawLine(pen, l_processo.y1, l_processo.x2, l_processo.y2, l_processo.x1)
                            Else

                                imagem = CType(l_processo.objecto, System.Drawing.Image)
                                pdfPAGE.Canvas.DrawImage(imagem, l_processo.x1, l_processo.y1, l_processo.x2 - l_processo.x1, l_processo.y2 - l_processo.y1, 0, PDFKeepAspectRatio.KeepBoth)
                            End If

                        Case g10phPDF4.Report.LDDStandard.line

                            Dim pen As New PDF4NET.Graphics.PDFPen(l_corNET2O2S(l_processo.tinta))
                            pen.Width = l_processo.grossura

                            pdfPAGE.Canvas.DrawLine(pen, l_processo.x1, l_processo.y1, l_processo.x2, l_processo.y2)

                        Case g10phPDF4.Report.LDDStandard.box

                            Dim pen As New PDF4NET.Graphics.PDFPen(l_corNET2O2S(l_processo.tinta))
                            pen.Width = l_processo.grossura

                            If instrução.Parametro.ToString = System.Drawing.Color.Transparent.ToArgb.ToString Then

                                pdfPAGE.Canvas.DrawRectangle(pen, l_processo.x1, l_processo.y1, l_processo.x2 - l_processo.x1, l_processo.y2 - l_processo.y1)

                            Else

                                Dim brush As New PDF4NET.Graphics.PDFBrush(l_corNET2O2S(CInt(instrução.Parametro)))
                                pdfPAGE.Canvas.DrawRectangle(pen, brush, l_processo.x1, l_processo.y1, l_processo.x2 - l_processo.x1, l_processo.y2 - l_processo.y1)

                            End If

                        Case g10phPDF4.Report.LDDStandard.poldraw

                            Dim pen As New PDF4NET.Graphics.PDFPen(l_corNET2O2S(l_processo.tinta))
                            pen.Width = l_processo.grossura

                            Dim pontos(l_processo.pontos.GetUpperBound(0)) As System.Drawing.PointF

                            For i As Integer = 0 To l_processo.pontos.GetUpperBound(0)
                                pontos(i).X = l_processo.pontos(i).X
                                pontos(i).Y = l_processo.pontos(i).Y
                            Next

                            If instrução.Parametro.ToString = System.Drawing.Color.Transparent.ToArgb.ToString Then
                                pdfPAGE.Canvas.DrawPolygon(pen, pontos)
                            Else
                                Dim brush As New PDF4NET.Graphics.PDFBrush(l_corNET2O2S(CInt(instrução.Parametro)))
                                pdfPAGE.Canvas.DrawPolygon(pen, brush, pontos)
                            End If

                        Case g10phPDF4.Report.LDDStandard.digitalsign
                            If l_processo.Documento.Assina_Doc Then
                                Dim sign As New PDFSignatureField("Signature")
                                sign = l_processo.Documento.StackObj(CInt(instrução.Parametro))
                                If sign.Signature.Certificate Is Nothing Then sign.Signature.Certificate = New X509Certificate2(l_processo.Documento.AssinaturaDigital, l_processo.Documento.PasswordCert)

                                pdfPAGE.Fields.Add(sign)
                            End If

                            ' todo: LDD2Screen: interpretação das restantes instruções LDD 
                            ' não tratadas na classe base (aquelas que resultam em algo visivel)

                    End Select

                End If
            End SyncLock
        End Function


        Private Function l_orientaçãoLDD2O2S(ByVal ori As g10phPDF4.Report.PaginaOrientação) As PDF4NET.PDFPageOrientation ' PDF4NET.PageOrientation
            SyncLock _locker
                Select Case ori

                    Case g10phPDF4.Report.PaginaOrientação.Portrait : Return PDF4NET.PDFPageOrientation.Portrait ' PDF4NET.PageOrientation.Portrait
                    Case g10phPDF4.Report.PaginaOrientação.Landscape : Return PDF4NET.PDFPageOrientation.Landscape ' PDF4NET.PageOrientation.Landscape

                    Case Else
                        Return PDF4NET.PDFPageOrientation.Portrait 'PDF4NET.PageOrientation.Portrait

                End Select
            End SyncLock
        End Function

        Private Function l_formaLDD2O2S(ByVal forma As g10phPDF4.Report.PaginaForma) As PDF4NET.PDFPageSize 'PDF4NET.PageSize
            SyncLock _locker
                Select Case forma
                    Case g10phPDF4.Report.PaginaForma.A0 : Return PDF4NET.PDFPageSize.A0
                    Case g10phPDF4.Report.PaginaForma.A1 : Return PDF4NET.PDFPageSize.A1
                    Case g10phPDF4.Report.PaginaForma.A2 : Return PDF4NET.PDFPageSize.A2
                    Case g10phPDF4.Report.PaginaForma.A3 : Return PDF4NET.PDFPageSize.A3 ' PDF4NET.PageSize.A3
                    Case g10phPDF4.Report.PaginaForma.A4 : Return PDF4NET.PDFPageSize.A4 ' PDF4NET.PageSize.A4
                    Case g10phPDF4.Report.PaginaForma.A5 : Return PDF4NET.PDFPageSize.A5 'PDF4NET.PageSize.A5

                    Case Else
                        Return PDF4NET.PDFPageSize.A4 'PDF4NET.PageSize.A4

                End Select
            End SyncLock
        End Function

        Private Function l_corNET2O2S(ByVal cor As System.Drawing.Color) As PDF4NET.Graphics.PDFRgbColor ' PDF4NET.Graphics.PDFColor
            SyncLock _locker

                Return New PDF4NET.Graphics.PDFRgbColor(cor.R, cor.G, cor.B) 'New PDF4NET.Graphics.PDFColor(cor.R, cor.G, cor.B)
            End SyncLock
        End Function

        Private Function l_corNET2O2S(ByVal rgb As Integer) As PDF4NET.Graphics.PDFRgbColor ' PDF4NET.Graphics.PDFColor
            SyncLock _locker
                Dim cor As New System.Drawing.Color

                cor = Drawing.Color.FromArgb(rgb)

                Return New PDF4NET.Graphics.PDFRgbColor(cor.R, cor.G, cor.B)
            End SyncLock
        End Function

        Private Function l_fontNET2OS(ByVal fonte_nome As String, ByVal fonte_size As Single) As PDF4NET.Graphics.Fonts.PDFFontBase 'PDF4NET.Graphics.PDFFont
            SyncLock _locker
                Select Case fonte_nome.ToLower

                    Case "helvetica" : Return New PDF4NET.Graphics.Fonts.PDFFont(Fonts.PDFFontFace.Helvetica, 0)
                    Case "times new roman" : Return New PDF4NET.Graphics.Fonts.PDFFont(Fonts.PDFFontFace.TimesRoman, 0)
                    Case "courier" : Return New PDF4NET.Graphics.Fonts.PDFFont(Fonts.PDFFontFace.Courier, 0)
                    Case "symbol" : Return New PDF4NET.Graphics.Fonts.PDFFont(Fonts.PDFFontFace.Symbol, 0)
                    Case "zap" : Return New PDF4NET.Graphics.Fonts.PDFFont(Fonts.PDFFontFace.ZapfDingbats, 0)

                    Case Else

                        Try
                            Dim existe As PDF4NET.Graphics.Fonts.TrueTypeFont = New PDF4NET.Graphics.Fonts.TrueTypeFont(fonte_nome, fonte_size, True)

                            Return existe
                        Catch ex As Exception
                            Dim naoexiste As PDF4NET.Graphics.Fonts.PDFFont = New PDF4NET.Graphics.Fonts.PDFFont(Fonts.PDFFontFace.TimesRoman, 0)
                            Return naoexiste
                        End Try
                        'Return New PDF4NET.Graphics.Fonts.TrueTypeFont(fonte_nome, fonte_size, True)
                        'Return New PDF4NET.Graphics.PDFFont(FontFace.TimesRoman, 0)

                End Select
            End SyncLock
        End Function

        Private Function l_alinhaLDD2OS(ByVal alinhamento As Integer) As PDF4NET.Graphics.PDFTextAlign ' PDF4NET.Graphics.Shapes.TextAlign
            SyncLock _locker
                Select Case alinhamento

                    Case Windows.Forms.HorizontalAlignment.Right : Return PDF4NET.Graphics.PDFTextAlign.MiddleRight ' PDF4NET.Graphics.Shapes.TextAlign.MiddleRight
                    Case Windows.Forms.HorizontalAlignment.Left : Return PDF4NET.Graphics.PDFTextAlign.MiddleLeft 'PDF4NET.Graphics.Shapes.TextAlign.MiddleLeft
                    Case Windows.Forms.HorizontalAlignment.Center : Return PDF4NET.Graphics.PDFTextAlign.MiddleCenter 'PDF4NET.Graphics.Shapes.TextAlign.MiddleCenter

                    Case Else
                        Return PDF4NET.Graphics.PDFTextAlign.MiddleLeft ' PDF4NET.Graphics.Shapes.TextAlign.MiddleLeft

                End Select
            End SyncLock
        End Function

        Private Function GetTextoByLines(ByVal Texto As String, ByVal Largura As Single, ByVal Fonte As O2S.Components.PDF4NET.Graphics.Fonts.PDFFont) As String()

            SyncLock _locker
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

                Lin = ""
                L = 0

                C = 0
                LC = 1
                ReDim Linha(L)

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

                    ' testa se a linha já existente mais a palavra agora analisada cabem na largura
                    Excede = GetLarguraTexto(Lin & Pal & Car, Fonte) >= Largura - 300
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

                        ' linha para o próximo ciclo
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
            End SyncLock
        End Function

        Private Function GetLarguraTexto(ByVal Texto As String, ByVal Fonte As O2S.Components.PDF4NET.Graphics.Fonts.PDFFont) As Single
            SyncLock _locker
                Dim A As New PDF4NET.Graphics.Fonts.PDFFont(Fonte)

                Dim W As Single = 0

                If Texto Is Nothing Then Return 0

                For i As Integer = 0 To Texto.Length - 1
                    W += A.GetCharWidth(CChar(Texto.Substring(i, 1)))
                Next

                Return Fonte.Size * (W / 1000)
            End SyncLock
        End Function

    End Class

End Namespace