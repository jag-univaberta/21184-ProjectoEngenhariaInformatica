Namespace Sistemas

    Public Enum SistemasDeDatums

        NãoUsado = 0
        Portugal_73 = 1
        Portugal_Lisboa = 2

    End Enum

    Public Enum SistemasDeCoordenadas

        Natural = 1
        Portugal_IGC1 = 2
        Portugal_IGC3 = 3
        Portugal_UTM_29N = 4
        Portugal_SCE = 5

    End Enum

    '                                                                                           

    Public Class SistemaDeCoordenadasBase
        Implements Mapframe.ISISTEMACOORDENADAS

        Protected l_nome As String
        Protected l_titulo As String
        Protected l_unidade As Mapframe.mapUnidades
        Protected l_tipo As Mapframe.mapTiposSistema
        Protected l_datum As Mapframe.IDATUM
        Protected l_projecção As Mapframe.IPROJECÇÃO
        Protected l_elipsoide As Mapframe.IELIPSOIDE

        Private Shared ReadOnly _locker As New Object()

        Public Sub New()

        End Sub

        Public Sub New(ByVal p_nome As String, ByVal p_titulo As String, ByVal p_unidade As Mapframe.mapUnidades, ByVal p_tipo As Mapframe.mapTiposSistema, ByVal p_datum As Mapframe.IDATUM, ByVal p_projecção As Mapframe.IPROJECÇÃO, ByVal p_elipsoide As Mapframe.IELIPSOIDE)
            SyncLock _locker
                Me.Define(p_nome, p_titulo, p_unidade, p_projecção, p_tipo, p_datum, p_elipsoide)
            End SyncLock
        End Sub

        Public Sub Define(ByVal p_nome As String, ByVal p_titulo As String, ByVal p_unidade As Mapframe.mapUnidades, ByVal p_projecção As Mapframe.IPROJECÇÃO, ByVal p_tipo As Mapframe.mapTiposSistema, ByVal p_datum As Mapframe.IDATUM, ByVal p_elipsoide As Mapframe.IELIPSOIDE) Implements Mapframe.ISISTEMACOORDENADAS.Define
            SyncLock _locker
                Me.l_nome = p_nome
                Me.l_titulo = p_titulo
                Me.l_unidade = p_unidade
                Me.l_tipo = p_tipo
                Me.l_datum = p_datum
                Me.l_projecção = p_projecção
                Me.l_elipsoide = p_elipsoide
            End SyncLock
        End Sub


        Public ReadOnly Property Nome() As String Implements Mapframe.ISISTEMACOORDENADAS.Nome
            Get
                SyncLock _locker
                    Return l_nome
                End SyncLock
            End Get
        End Property

        Public ReadOnly Property Titulo() As String Implements Mapframe.ISISTEMACOORDENADAS.Titulo
            Get
                SyncLock _locker
                    Return l_titulo
                End SyncLock
            End Get
        End Property

        Public ReadOnly Property Projecção() As Mapframe.IPROJECÇÃO Implements Mapframe.ISISTEMACOORDENADAS.Projecção
            Get
                SyncLock _locker
                    Return l_projecção
                End SyncLock
            End Get
        End Property

        Public ReadOnly Property Unidade() As Mapframe.mapUnidades Implements Mapframe.ISISTEMACOORDENADAS.Unidade
            Get
                SyncLock _locker
                    Return l_unidade
                End SyncLock
            End Get
        End Property

        Public ReadOnly Property Tipo() As Mapframe.mapTiposSistema Implements Mapframe.ISISTEMACOORDENADAS.Tipo
            Get
                SyncLock _locker
                    Return l_tipo
                End SyncLock
            End Get
        End Property

        Public ReadOnly Property Datum() As Mapframe.IDATUM Implements Mapframe.ISISTEMACOORDENADAS.Datum
            Get
                SyncLock _locker
                    Return l_datum
                End SyncLock
            End Get
        End Property

        Public ReadOnly Property Elipsoide() As Mapframe.IELIPSOIDE Implements Mapframe.ISISTEMACOORDENADAS.Elipsoide
            Get
                SyncLock _locker
                    Return l_elipsoide
                End SyncLock
            End Get
        End Property


        Public Function ConverteMCS2Un(ByVal Valor As Double, ByVal Unidade As Mapframe.mapUnidades, ByVal Ponto As Mapframe.IVECTOR_) As Double Implements Mapframe.ISISTEMACOORDENADAS.ConverteMCS2Un

        End Function

        Public Function ConverteUn2MCS(ByVal Valor As Double, ByVal Unidade As Mapframe.mapUnidades, ByVal Ponto As Mapframe.IVECTOR_) As Double Implements Mapframe.ISISTEMACOORDENADAS.ConverteUn2MCS

        End Function

        Public Function FactorMCS2Un(ByVal Unidade As Mapframe.mapUnidades, ByVal Ponto As Mapframe.IVECTOR_) As Double Implements Mapframe.ISISTEMACOORDENADAS.FactorMCS2Un

        End Function

    End Class

    Public Class DatumBase
        Implements Mapframe.IDATUM

        Protected l_nome As String
        Protected l_titulo As String
        Protected l_tipo As Mapframe.mapTiposSistema
        Protected l_elipsoide As Mapframe.IELIPSOIDE

        Private Shared ReadOnly _locker As New Object()

        Public Sub New()

        End Sub

        Public Sub New(ByVal p_nome As String, ByVal p_titulo As String, ByVal p_tipo As Mapframe.mapTiposSistema, ByVal p_elipsoide As Mapframe.IELIPSOIDE)
            SyncLock _locker
                Me.Define(p_nome, p_titulo, p_tipo, p_elipsoide)
            End SyncLock
        End Sub

        Public Sub Define(ByVal p_nome As String, ByVal p_titulo As String, ByVal p_tipo As Mapframe.mapTiposSistema, ByVal p_elipsoide As Mapframe.IELIPSOIDE) Implements Mapframe.IDATUM.Define
            SyncLock _locker
                If Not p_nome Is Nothing Then l_nome = p_nome
                If Not p_titulo Is Nothing Then l_titulo = p_titulo
                If p_tipo <> Mapframe.mapTiposSistema.Null Then l_tipo = p_tipo
                If Not p_elipsoide Is Nothing Then l_elipsoide = p_elipsoide
            End SyncLock
        End Sub

        Public ReadOnly Property Elipsoide() As Mapframe.IELIPSOIDE Implements Mapframe.IDATUM.Elipsoide
            Get

            End Get
        End Property

        Public ReadOnly Property Nome() As String Implements Mapframe.IDATUM.Nome
            Get

            End Get
        End Property

        Public ReadOnly Property Tipo() As Mapframe.mapTiposSistema Implements Mapframe.IDATUM.Tipo
            Get

            End Get
        End Property

        Public ReadOnly Property Titulo() As String Implements Mapframe.IDATUM.Titulo
            Get

            End Get
        End Property


        Public Function TransBursaWolfe(ByVal Ponto As Mapframe.ICOORDENADA_) As Object Implements Mapframe.IDATUM.TransBursaWolfe

        End Function

        Public Function TransGeodesicas(ByVal Ponto As Mapframe.ICOORDENADA_) As Object Implements Mapframe.IDATUM.TransGeodesicas

        End Function

        Public Function TransMolodensky(ByVal Ponto As Mapframe.ICOORDENADA_, ByVal Simplificado As Boolean) As Object Implements Mapframe.IDATUM.TransMolodensky

        End Function

        Public Function TransNaturais(ByVal Ponto As Mapframe.ICOORDENADA_) As Object Implements Mapframe.IDATUM.TransNaturais

        End Function

    End Class

    Public Class ProjecçãoBase
        Implements Mapframe.IPROJECÇÃO

        Public Property AzimuteEixoX() As Double Implements Mapframe.IPROJECÇÃO.AzimuteEixoX
            Get

            End Get
            Set(ByVal Value As Double)

            End Set
        End Property

        Public Property AzimuteEixoY() As Double Implements Mapframe.IPROJECÇÃO.AzimuteEixoY
            Get

            End Get
            Set(ByVal Value As Double)

            End Set
        End Property

        Public Property CirculoLatitude() As Double Implements Mapframe.IPROJECÇÃO.CirculoLatitude
            Get

            End Get
            Set(ByVal Value As Double)

            End Set
        End Property

        Public Property CirculoLongitude() As Double Implements Mapframe.IPROJECÇÃO.CirculoLongitude
            Get

            End Get
            Set(ByVal Value As Double)

            End Set
        End Property

        Public Property CirculoMaiorAzimute() As Double() Implements Mapframe.IPROJECÇÃO.CirculoMaiorAzimute
            Get

            End Get
            Set(ByVal Value() As Double)

            End Set
        End Property

        Public Property CirculoMaiorLatitude() As Double() Implements Mapframe.IPROJECÇÃO.CirculoMaiorLatitude
            Get

            End Get
            Set(ByVal Value() As Double)

            End Set
        End Property

        Public Property CirculoMaiorLongitude() As Double() Implements Mapframe.IPROJECÇÃO.CirculoMaiorLongitude
            Get

            End Get
            Set(ByVal Value() As Double)

            End Set
        End Property

        Public Property CoeficienteAfinidadeA() As Double() Implements Mapframe.IPROJECÇÃO.CoeficienteAfinidadeA
            Get

            End Get
            Set(ByVal Value() As Double)

            End Set
        End Property

        Public Property CoeficienteAfinidadeB() As Double() Implements Mapframe.IPROJECÇÃO.CoeficienteAfinidadeB
            Get

            End Get
            Set(ByVal Value() As Double)

            End Set
        End Property

        Public Property ComplexoA() As Double() Implements Mapframe.IPROJECÇÃO.ComplexoA
            Get

            End Get
            Set(ByVal Value() As Double)

            End Set
        End Property

        Public Property ComplexoB() As Double() Implements Mapframe.IPROJECÇÃO.ComplexoB
            Get

            End Get
            Set(ByVal Value() As Double)

            End Set
        End Property

        Public Property DistanciaAMeridiana() As Double Implements Mapframe.IPROJECÇÃO.DistanciaAMeridiana
            Get

            End Get
            Set(ByVal Value As Double)

            End Set
        End Property

        Public Property DistanciaAngularAoParalelo() As Double() Implements Mapframe.IPROJECÇÃO.DistanciaAngularAoParalelo
            Get

            End Get
            Set(ByVal Value() As Double)

            End Set
        End Property

        Public Property DistanciaAPerpendicular() As Double Implements Mapframe.IPROJECÇÃO.DistanciaAPerpendicular
            Get

            End Get
            Set(ByVal Value As Double)

            End Set
        End Property

        Public Property ElevaçãoMédia() As Double Implements Mapframe.IPROJECÇÃO.ElevaçãoMédia
            Get

            End Get
            Set(ByVal Value As Double)

            End Set
        End Property

        Public Property ElevaçãoMédiaGeoid() As Double Implements Mapframe.IPROJECÇÃO.ElevaçãoMédiaGeoid
            Get

            End Get
            Set(ByVal Value As Double)

            End Set
        End Property

        Public Property EscalaCentral() As Double Implements Mapframe.IPROJECÇÃO.EscalaCentral
            Get

            End Get
            Set(ByVal Value As Double)

            End Set
        End Property

        Public Property EscalaEste() As Double Implements Mapframe.IPROJECÇÃO.EscalaEste
            Get

            End Get
            Set(ByVal Value As Double)

            End Set
        End Property

        Public Property EscalaNormal() As Double Implements Mapframe.IPROJECÇÃO.EscalaNormal
            Get

            End Get
            Set(ByVal Value As Double)

            End Set
        End Property

        Public Property EscalaNorte() As Double Implements Mapframe.IPROJECÇÃO.EscalaNorte
            Get

            End Get
            Set(ByVal Value As Double)

            End Set
        End Property

        Public Property EscalaOeste() As Double Implements Mapframe.IPROJECÇÃO.EscalaOeste
            Get

            End Get
            Set(ByVal Value As Double)

            End Set
        End Property

        Public Property EscalaRedução() As Double Implements Mapframe.IPROJECÇÃO.EscalaRedução
            Get

            End Get
            Set(ByVal Value As Double)

            End Set
        End Property

        Public Property EscalaStadard() As Double Implements Mapframe.IPROJECÇÃO.EscalaStadard
            Get

            End Get
            Set(ByVal Value As Double)

            End Set
        End Property

        Public Property EscalaSul() As Double Implements Mapframe.IPROJECÇÃO.EscalaSul
            Get

            End Get
            Set(ByVal Value As Double)

            End Set
        End Property

        Public Property Hemisferio() As Double Implements Mapframe.IPROJECÇÃO.Hemisferio
            Get

            End Get
            Set(ByVal Value As Double)

            End Set
        End Property

        Public Property LatitudeObliquaPolar() As Double Implements Mapframe.IPROJECÇÃO.LatitudeObliquaPolar
            Get

            End Get
            Set(ByVal Value As Double)

            End Set
        End Property

        Public Property LatitudePolar() As Double() Implements Mapframe.IPROJECÇÃO.LatitudePolar
            Get

            End Get
            Set(ByVal Value() As Double)

            End Set
        End Property

        Public Property LimiteMargemEste() As Double Implements Mapframe.IPROJECÇÃO.LimiteMargemEste
            Get

            End Get
            Set(ByVal Value As Double)

            End Set
        End Property

        Public Property LimiteMargemOeste() As Double Implements Mapframe.IPROJECÇÃO.LimiteMargemOeste
            Get

            End Get
            Set(ByVal Value As Double)

            End Set
        End Property

        Public Property LongitudeObliquaPolar() As Double Implements Mapframe.IPROJECÇÃO.LongitudeObliquaPolar
            Get

            End Get
            Set(ByVal Value As Double)

            End Set
        End Property

        Public Property LongitudePolar() As Double() Implements Mapframe.IPROJECÇÃO.LongitudePolar
            Get

            End Get
            Set(ByVal Value() As Double)

            End Set
        End Property

        Public Property MeridianoCentral() As Double Implements Mapframe.IPROJECÇÃO.MeridianoCentral
            Get

            End Get
            Set(ByVal Value As Double)

            End Set
        End Property

        Public Property Nome() As String Implements Mapframe.IPROJECÇÃO.Nome
            Get

            End Get
            Set(ByVal Value As String)

            End Set
        End Property

        Public Property OrigemLatitude() As Double Implements Mapframe.IPROJECÇÃO.OrigemLatitude
            Get

            End Get
            Set(ByVal Value As Double)

            End Set
        End Property

        Public Property OrigemLongitude() As Double Implements Mapframe.IPROJECÇÃO.OrigemLongitude
            Get

            End Get
            Set(ByVal Value As Double)

            End Set
        End Property

        Public Property ParaleloObliquoConico() As Double Implements Mapframe.IPROJECÇÃO.ParaleloObliquoConico
            Get

            End Get
            Set(ByVal Value As Double)

            End Set
        End Property

        Public Property Titulo() As String Implements Mapframe.IPROJECÇÃO.Titulo
            Get

            End Get
            Set(ByVal Value As String)

            End Set
        End Property

        Public Property ZonaUTM() As Double Implements Mapframe.IPROJECÇÃO.ZonaUTM
            Get

            End Get
            Set(ByVal Value As Double)

            End Set
        End Property
    End Class

    Public Class ElipsoideBase

    End Class

    '                                                                                           

    Public Class ProjecçãoTransverseMercator
        Inherits ProjecçãoBase

    End Class

    Public Class DatumPortugal_73
        Inherits DatumBase

    End Class

    Public Class DatumPortugal_Lisboa
        Inherits DatumBase

    End Class

    Public Class SistemaNatural
        Inherits SistemaDeCoordenadasBase

    End Class

    Public Class SistemaPortugal_IGC1
        Inherits SistemaDeCoordenadasBase

        Private Shared ReadOnly _locker As New Object()

        Public Sub New()
            SyncLock _locker
                Me.l_nome = "PRT-IGC1"
                Me.l_titulo = "Portugal, IGC1 System"
                Me.l_unidade = Mapframe.mapUnidades.Graus
                Me.l_tipo = Mapframe.mapTiposSistema.Elipsoidais
                Me.l_datum = New DatumPortugal_73
                Me.l_projecção = New ProjecçãoTransverseMercator
            End SyncLock
        End Sub

    End Class

    '                                                                                           

    Module Sistemas
        Private ReadOnly _locker As New Object()

        ' como converter uma coordenada entre um sistema A para um sistema B 
        '
        ' Dim A as ICOORDENADA_ 
        ' A.Sistema = NovoSistema(SistemaDeCoordenadas.Natural) 
        ' A.Lat = 039.413733 
        ' A.Lon = 008.075343 
        ' A.Alt = 600.51 
        ' A.Sistema = NovoSistema(SistemaDeCoordenadas.Europeu) 
        ' ? A.Lat ' 039.412994
        ' ? A.Lon ' 008.074455
        ' ? A.Alt ' 570.40

        Public Function NovoDatum(ByVal Datum As SistemasDeDatums) As Mapframe.IDATUM
            SyncLock _locker
                Select Case Datum

                    Case SistemasDeDatums.NãoUsado : Return Nothing
                    Case SistemasDeDatums.Portugal_73 : Return New DatumPortugal_73
                    Case SistemasDeDatums.Portugal_Lisboa : Return New DatumPortugal_Lisboa

                End Select
            End SyncLock
        End Function

        Public Function NovoSistema(ByVal Sistema As SistemasDeCoordenadas) As Mapframe.ISISTEMACOORDENADAS
            SyncLock _locker
                Select Case Sistema

                    Case SistemasDeCoordenadas.Natural : Return New SistemaNatural

                    Case SistemasDeCoordenadas.Portugal_IGC1 : Return New SistemaPortugal_IGC1

                    Case SistemasDeCoordenadas.Portugal_IGC3 : Return New SistemaDeCoordenadasBase( _
                                                                "PRT-IGC3", _
                                                                "Portugal IGC3 System", _
                                                                Mapframe.mapUnidades.Graus, _
                                                                Mapframe.mapTiposSistema.Elipsoidais, _
                                                                New DatumPortugal_Lisboa, _
                                                                New ProjecçãoTransverseMercator, Nothing)

                    Case SistemasDeCoordenadas.Portugal_UTM_29N : Return New SistemaDeCoordenadasBase( _
                                                                "PRT-U29", _
                                                                "Portugal using UTM, Zone 29 North, Meter", _
                                                                Mapframe.mapUnidades.Metros, _
                                                                Mapframe.mapTiposSistema.Elipsoidais, _
                                                                Nothing, _
                                                                New ProjecçãoTransverseMercator, Nothing)

                    Case SistemasDeCoordenadas.Portugal_SCE : Return New SistemaDeCoordenadasBase( _
                                                                "PRT-SCE", _
                                                                "Portugal, National Grid, SCE System", _
                                                                Mapframe.mapUnidades.Graus, _
                                                                Mapframe.mapTiposSistema.Elipsoidais, _
                                                                New DatumPortugal_Lisboa, _
                                                                New ProjecçãoTransverseMercator, Nothing)

                End Select
            End SyncLock
        End Function

        ' talvez o melhor não seja implementar uma classe para cada datum ou sistema mas antes
        '   procedimento publico que as deine a partir de classes base

    End Module

End Namespace