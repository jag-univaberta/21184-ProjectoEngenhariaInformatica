Namespace Mapdefault

    Public Class Objecto
        Implements mapFRAME.IOBJECTO

        Private l_Geometrias() As mapFRAME.IGEOMETRIA
        Private l_Tamanho As Double = 0



        Public Property Link() As String Implements Mapframe.IOBJECTO.Link
            Get

            End Get
            Set(ByVal Value As String)

            End Set
        End Property
        Public Property Nome() As String Implements Mapframe.IOBJECTO.Nome
            Get

            End Get
            Set(ByVal Value As String)

            End Set
        End Property
        Public ReadOnly Property Tamanho() As Double Implements Mapframe.IOBJECTO.Tamanho
            Get

                Return l_Tamanho

            End Get
        End Property
        Public Property Tipo() As Mapframe.mapObjectoTipos Implements Mapframe.IOBJECTO.Tipo
            Get

            End Get
            Set(ByVal Value As Mapframe.mapObjectoTipos)

            End Set
        End Property
        Public Property Chave() As String Implements Mapframe.IOBJECTO.Chave
            Get

            End Get
            Set(ByVal Value As String)

            End Set
        End Property
        Public Property Extensão() As Mapframe.IEXTENSÃO_ Implements Mapframe.IOBJECTO.Extensão
            Get

            End Get
            Set(ByVal Value As Mapframe.IEXTENSÃO_)

            End Set
        End Property
        Public Property Layer() As Mapframe.ILAYER Implements Mapframe.IOBJECTO.Layer
            Get

            End Get
            Set(ByVal Value As Mapframe.ILAYER)

            End Set
        End Property

        Public Property Geometrias() As Mapframe.IGEOMETRIA() Implements Mapframe.IOBJECTO.Geometrias
            Get

                Return l_Geometrias

            End Get
            Set(ByVal Value As Mapframe.IGEOMETRIA())

                l_Geometrias = Value

            End Set
        End Property

        Public ReadOnly Property Vertices() As Mapframe.ICOORDENADA_() Implements Mapframe.IOBJECTO.Vertices
            Get

            End Get
        End Property

        Public Function AdicionaCirculo(ByVal Centro As Mapframe.ICOORDENADA_, ByVal Raio As Double, ByVal Unidade As Mapframe.mapUnidades, ByVal QuantosVertices As Integer) As Boolean Implements Mapframe.IOBJECTO.AdicionaCirculo

        End Function
        'todo: **** Falta implementar esta função
        Public Function AdicionaPoligono(ByVal Vertices() As Mapframe.ICOORDENADA_) As Boolean Implements Mapframe.IOBJECTO.AdicionaPoligono

            ReDim Preserve l_Geometrias(l_Tamanho)

            l_Tamanho = l_Tamanho + 1

            Return True

        End Function
        Public Function AdicionaPolilinha(ByVal Vertices() As Mapframe.ICOORDENADA_) As Boolean Implements Mapframe.IOBJECTO.AdicionaPolilinha

        End Function
        Public Function AdicionaSimbolo(ByVal Ponto As Mapframe.ICOORDENADA_) As Boolean Implements Mapframe.IOBJECTO.AdicionaSimbolo

        End Function
        Public Function AdicionaTexto(ByVal Ponto As Mapframe.ICOORDENADA_, ByVal Texto As String) As Boolean Implements Mapframe.IOBJECTO.AdicionaTexto

        End Function
        Public Function AdicionaVertice(ByVal Vertice As Mapframe.IGEOMETRIA) As Boolean Implements Mapframe.IOBJECTO.AdicionaVertice

        End Function

    End Class

    Public Structure Coordenada
        Implements Mapframe.ICOORDENADA_



        Private l_alt As Double
        Private l_lat As Double
        Private l_lon As Double
        Private l_sis As mapFRAME.ISISTEMACOORDENADAS

        Public Sub New(ByVal p_lon As Double, ByVal p_lat As Double)

            l_lon = p_lon
            l_lat = p_lat

        End Sub

        Public Sub New(ByVal p_lon As Double, ByVal p_lat As Double, ByVal p_alt As Double)

            l_lon = p_lon
            l_lat = p_lat
            l_alt = p_alt

        End Sub

        Public Property Alt() As Double Implements mapFRAME.ICOORDENADA_.Alt
            Get

                Return l_alt

            End Get
            Set(ByVal Value As Double)

                l_alt = Value

            End Set
        End Property
        Public Property Lat() As Double Implements mapFRAME.ICOORDENADA_.Lat
            Get

                Return l_lat

            End Get
            Set(ByVal Value As Double)

                l_lat = Value

            End Set
        End Property
        Public Property Lon() As Double Implements mapFRAME.ICOORDENADA_.Lon
            Get

                Return l_lon

            End Get
            Set(ByVal Value As Double)

                l_lon = Value

            End Set
        End Property

        Public Function SemelhanteA(ByVal OutroPonto As mapFRAME.ICOORDENADA_) As Boolean Implements mapFRAME.ICOORDENADA_.SemelhanteA

            Return (Me.l_lon = OutroPonto.Lon) And (Me.l_lat = OutroPonto.Lat) And (Me.l_alt = OutroPonto.Alt) And (Me.l_sis Is OutroPonto.Sistema)

        End Function

        Public Property Sistema() As mapFRAME.ISISTEMACOORDENADAS Implements mapFRAME.ICOORDENADA_.Sistema
            Get

                Return l_sis

            End Get
            Set(ByVal Value As Mapframe.ISISTEMACOORDENADAS)

                l_sis = Value

            End Set
        End Property

    End Structure

    Public Structure Vector
        Implements Mapframe.IVECTOR_

        Private l_Alt As Double
        Private l_Lat As Double
        Private l_Lon As Double
        Private l_Sis As mapFRAME.ISISTEMACOORDENADAS
        Private l_Azi As Double
        Private l_Com As Double
        Private l_Dec As Double



        Public Property Alt() As Double Implements Mapframe.ICOORDENADA_.Alt
            Get

                Return l_Alt

            End Get
            Set(ByVal Value As Double)

                l_Alt = Value

            End Set
        End Property
        Public Property Lat() As Double Implements mapFRAME.ICOORDENADA_.Lat
            Get

                Return l_Lat

            End Get
            Set(ByVal Value As Double)

                l_Lat = Value

            End Set
        End Property
        Public Property Lon() As Double Implements mapFRAME.ICOORDENADA_.Lon
            Get

                Return l_Lon

            End Get
            Set(ByVal Value As Double)

                l_Lon = Value

            End Set
        End Property
        Public Property Sistema() As mapFRAME.ISISTEMACOORDENADAS Implements mapFRAME.ICOORDENADA_.Sistema
            Get

                Return l_Sis

            End Get
            Set(ByVal Value As Mapframe.ISISTEMACOORDENADAS)

                l_Sis = Value

            End Set
        End Property
        Public Property Azimute() As Double Implements mapFRAME.IVECTOR_.Azimute
            Get

                Return l_Azi

            End Get
            Set(ByVal Value As Double)

                l_Azi = Value

            End Set
        End Property
        Public Property Comprimento() As Double Implements mapFRAME.IVECTOR_.Comprimento
            Get

                Return l_Com

            End Get
            Set(ByVal Value As Double)

                l_Com = Value

            End Set
        End Property
        Public Property Declinação() As Double Implements mapFRAME.IVECTOR_.Declinação
            Get

                Return l_Dec

            End Get
            Set(ByVal Value As Double)

                l_Dec = Value

            End Set
        End Property

        Public Overloads Function SemelhanteA(ByVal OutroPonto As mapFRAME.ICOORDENADA_) As Boolean Implements mapFRAME.ICOORDENADA_.SemelhanteA

            ' sou semelhante a outro ponto quando a lon, lat, alt e sistema forem os mesmos

            Return (Me.l_Lon = OutroPonto.Lon) And (Me.l_Lat = OutroPonto.Lat) And (Me.l_Alt = OutroPonto.Alt) And (Me.l_Sis Is OutroPonto.Sistema)

        End Function

        Public Overloads Function SemelhanteA(ByVal OutroVector As mapFRAME.IVECTOR_) As Boolean Implements mapFRAME.IVECTOR_.SemelhanteA

            ' sou semelhante a outro vector quando formos semlhantes como pontos e tivermos o mesmo azimute,
            '   a mesma declinação e o mesmo comprimento

            Return Me.SemelhanteA(CType(Me, Mapframe.ICOORDENADA_)) And Me.l_Azi = OutroVector.Azimute And Me.l_Dec = OutroVector.Declinação And Me.l_Com = OutroVector.Comprimento

        End Function

    End Structure

    Public Structure Extensão
        Implements Mapframe.IEXTENSÃO_

        Private l_verticeSW As mapFRAME.ICOORDENADA_
        Private l_verticeNE As mapFRAME.ICOORDENADA_
        Private l_unidade As mapFRAME.mapUnidades



        Public Sub New(ByVal SWLon As Double, ByVal SWLat As Double, ByVal NELon As Double, ByVal NELat As Double)

            Me.l_verticeSW = New Coordenada(SWLon, SWLat)
            Me.l_verticeNE = New Coordenada(NELon, NELat)

        End Sub

        Public Property VerticeSW() As mapFRAME.ICOORDENADA_ Implements mapFRAME.IEXTENSÃO_.VerticeSW
            Get

                If l_verticeSW Is Nothing Then l_verticeSW = New Coordenada
                Return l_verticeSW

            End Get
            Set(ByVal Value As mapFRAME.ICOORDENADA_)

                l_verticeSW = Value

            End Set
        End Property
        Public Property VerticeNE() As mapFRAME.ICOORDENADA_ Implements mapFRAME.IEXTENSÃO_.VerticeNE
            Get

                If l_verticeNE Is Nothing Then l_verticeNE = New Coordenada
                Return l_verticeNE

            End Get
            Set(ByVal Value As mapFRAME.ICOORDENADA_)

                l_verticeNE = Value

            End Set
        End Property
        Public Property Unidade() As mapFRAME.mapUnidades Implements mapFRAME.IEXTENSÃO_.Unidade
            Get

                Return l_unidade

            End Get
            Set(ByVal Value As mapFRAME.mapUnidades)

                l_unidade = Value

            End Set
        End Property

        Public ReadOnly Property VerticeCentral() As mapFRAME.ICOORDENADA_ Implements mapFRAME.IEXTENSÃO_.VerticeCentral
            Get

                Dim vc As Coordenada
                vc.Lon = VerticeSW.Lon + ((VerticeNE.Lon - VerticeSW.Lon) / 2)
                vc.Lat = VerticeSW.Lat + ((VerticeNE.Lat - VerticeSW.Lat) / 2)
                Return vc

            End Get
        End Property
        Public ReadOnly Property Area() As Double Implements mapFRAME.IEXTENSÃO_.Area
            Get

                Return Me.Largura * Me.Altura

            End Get
        End Property
        Public ReadOnly Property Largura() As Double Implements mapFRAME.IEXTENSÃO_.Largura
            Get

                Return VerticeNE.Lon - VerticeSW.Lon

            End Get
        End Property
        Public ReadOnly Property Altura() As Double Implements mapFRAME.IEXTENSÃO_.Altura
            Get

                Return VerticeNE.Lat - VerticeSW.Lat

            End Get
        End Property

        Public Sub DefExtensão(ByVal Lon As Double, ByVal Lat As Double, ByVal Largura As Double, ByVal Altura As Double) Implements mapFRAME.IEXTENSÃO_.DefExtensão

            VerticeSW.Lon = Lon - (Largura / 2)
            VerticeSW.Lat = Lat - (Altura / 2)
            VerticeNE.Lon = Lon + (Largura / 2)
            VerticeNE.Lat = Lat + (Altura / 2)

        End Sub
        Public Function SemelhanteA(ByVal OutraArea As mapFRAME.IEXTENSÃO_) As Boolean Implements mapFRAME.IEXTENSÃO_.SemelhanteA

            Return (Me.l_verticeSW.SemelhanteA(OutraArea.VerticeSW) And Me.l_verticeNE.SemelhanteA(OutraArea.VerticeNE) And l_unidade = OutraArea.Unidade)

        End Function

    End Structure

End Namespace

