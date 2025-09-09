
Public Class PedidoDocumento

    Public ProcId As Integer

    Public Extensao As Area

    Public Geometrias() As Geometria

    Public FicheiroDestino As String

    Public PedidoId As Integer

    Public MostraNumeroPagina As Boolean
    Public NumeroPagina_Inicial As Integer
    Public NumeroPagina_Final As Integer
    Public NumeroPagina_Alinhamento As Integer
    Public NumeroPagina_OffsetVertical As Integer
    Public NumeroPagina_OffsetHorizontal As Integer
    Public NumeroPagina_FontSize As Integer
    Public NumeroPagina_Prefixo As String
    Public NumeroPagina_Total As Boolean
    Public NumeroPagina_TotalValor As Integer
    Public ModeloXML As String
    Public Assina_Doc As Boolean
    Public Certificado As String
    Public PasswordCert As String


    Public ModeloScript As String

    Public ModeloMapa As String

    Public ModeloEscala As Integer

    Public ImagemMapa As String

    Public ImagemLegenda As String

    Public AreaMapa As g10Map4.g10Map4View

    Public Variaveis As New List(Of String)




End Class

Public Structure Area

    Public CoordenadaSW As Coordenada
    Public CoordenadaNE As Coordenada

    Public Function conv_2_gisFRAME_IEXTENSAO() As g10phPDF4.Mapframe.IEXTENSÃO_

        Dim e As New g10phPDF4.Mapdefault.Extensão

        e.VerticeSW = Me.CoordenadaSW.gisFRAME_Mapframe_ICOORDENADA
        e.VerticeNE = Me.CoordenadaNE.gisFRAME_Mapframe_ICOORDENADA

        Return e

    End Function

End Structure
Public Structure Geometria

    Public Vertices() As Coordenada

    Private Shared ReadOnly _locker As Long = 0

    Public Function conv_2_gisFRAME_ICOORDENADA() As g10phPDF4.Mapframe.ICOORDENADA_()

        Dim v() As g10phPDF4.Mapframe.ICOORDENADA_

        ReDim v(Vertices.GetUpperBound(0))

        For i As Integer = 0 To Vertices.GetUpperBound(0)

            v(i) = Vertices(i).gisFRAME_Mapframe_ICOORDENADA

        Next

        Return v

    End Function

End Structure

Public Structure Coordenada

    Public Lon As Double
    Public Lat As Double
    Public Alt As Double

    Private Shared ReadOnly _locker As Long = 0

    Public Function gisFRAME_Mapframe_ICOORDENADA() As g10phPDF4.Mapframe.ICOORDENADA_


        Dim c As New g10phPDF4.Mapdefault.Coordenada(Lon, Lat, Alt)

        Return c

    End Function

End Structure