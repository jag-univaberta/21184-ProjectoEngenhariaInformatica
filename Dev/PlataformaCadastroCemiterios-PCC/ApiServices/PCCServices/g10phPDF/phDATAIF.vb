Namespace Dataframe

    Public Enum datManias

        NãoDefinido = 0

        SqlServer = 1
        Access = 2
        Informix = 3
        Oracle7 = 4
        Oracle8 = 5
        Postgre = 6
    End Enum

    Public Enum datNaturezas

        NãoDefinido = -1

        Db = 0
        Xml = 1

    End Enum

    Public Enum datOperações

        NãoDefinido = 0

        Consulta = 1
        Alteração = 2
        Inserção = 3
        Eliminação = 4

    End Enum

    Public Enum datAlcances

        count = 15

        especial = 0

        select_ficha = 1
        select_registo = 2
        select_lista = 3
        select_fichas = 4
        select_procura = 5
        select_maximo = 6
        select_vista = 7

        update_ficha = 8
        insert_ficha = 9
        delete_ficha = 10

        select_primeiro = 11
        select_pesquisa = 12

        select_verifica = 13

        insert_registo = 14

    End Enum

    Public Enum datDirecções

        Ascendente = 0
        Descendente = 1

    End Enum

    Public Enum datLigações

        NãoDefinido = 0

        LeftJoin = 1
        InnerJoin = 2
        RightJoin = 3
        FullJoin = 4
        CrossJoin = 5

    End Enum

    Public Enum datOperadores

        Igual = 0
        Diferente = 1
        Maior = 2
        MaiorIgual = 3
        Menor = 4
        MenorIgual = 5
        Semelhante = 6

    End Enum

    Public Enum datPontes

        NãoDefinido = 0

        E = 1
        Ou = 2
        XE = 3
        XOu = 4

    End Enum

    Public Enum datFAgregadas

        NãoDefinida = 0

        Maximo = 1
        Minimo = 2
        Somatorio = 3
        Conta = 4
        Media = 5
        Converte = 6
        Maiusc = 7
        Minusc = 8
        Top = 9

    End Enum

    Public Enum datLocks

        Nenhum = 0
        Generico = 1
        Exclusivo = 2

    End Enum

    '                                                                                                       

    Public Class datObjecto

        Public Nome As String
        Public Titulo As String
        Public Descrição As String
        Public NTab As Integer

        Public DbAlias As String
        Public DbGrupo As String

        Public Membros() As datMembro
        Public Ponteiro As Integer
        Public MembroPonteiro As datMembro

        Public ArrayPonteiro() As Integer
        Public ArrayChave() As Integer
        Public ArrayDescrição() As Integer
        Public ArrayTodos() As Integer
        Public ArrayLista() As Integer
        Public ArrayVista() As Integer
        Public ArrayPesquisa() As Integer
        Public ArrayLinkados() As Integer

        Public MembrosPonteiro() As datMembro
        Public MembrosChave() As datMembro
        Public MembrosDescrição() As datMembro
        Public MembrosTodos() As datMembro
        Public MembrosLista() As datMembro
        Public MembrosVista() As datMembro
        Public MembrosPesquisa() As datMembro
        Public MembrosLinkados() As datMembro



        Public Sub New()

        End Sub

        Public Sub New(ByVal nome As String, ByVal ntab As Integer)

            Me.Nome = nome
            Me.Titulo = nome
            Me.Descrição = nome
            Me.DbAlias = nome
            Me.DbGrupo = "dataset_" & nome
            Me.NTab = ntab

            ReDim Me.Membros(0)
            Me.Membros = Nothing

            Me.Ponteiro = 0
            Me.ArrayPonteiro = Nothing
            Me.ArrayChave = Nothing
            Me.ArrayDescrição = Nothing
            Me.ArrayLista = Nothing
            Me.ArrayVista = Nothing
            Me.ArrayPesquisa = Nothing

        End Sub

        Public Sub MembroDef(ByVal campo As datMembro)

            If Me.Membros Is Nothing Then ReDim Me.Membros(0) Else ReDim Preserve Me.Membros(Me.Membros.GetUpperBound(0) + 1)
            Me.Membros(Me.Membros.GetUpperBound(0)) = campo

            Me.Membros(Me.Membros.GetUpperBound(0)).Objecto = Me
            Me.Membros(Me.Membros.GetUpperBound(0)).Ordem = Me.Membros.GetUpperBound(0)

            Me.MaisUmTodos()

        End Sub

        Public Sub MembroDef(ByVal nome As String, ByVal tipo As System.Data.DbType)

            Dim m As New datMembro(nome, tipo, 0, 0, 0, Nothing, Nothing)

            Me.MembroDef(m)

        End Sub

        Public Sub MembroDef(ByVal nome As String, ByVal tipo As System.Data.DbType, ByVal referencia As IDATLET, ByVal join As datLigações)

            Dim m As New datMembro(nome, tipo, 0, 0, 0, referencia, join)

            Me.MembroDef(m)

        End Sub

        Public Sub MembroDef(ByVal nome As String, ByVal tipo As System.Data.DbType, ByVal referencia As IDATLET, ByVal join As datLigações, ByVal refindex As Integer)

            Dim m As New datMembro(nome, tipo, 0, 0, 0, referencia, join)

            m.ReferenciaIndex = refindex

            Me.MembroDef(m)

        End Sub

        Public Sub MembroDef(ByVal nome As String, ByVal tipo As System.Data.DbType, ByVal tamanho As Integer, ByVal precisao As Integer, ByVal escala As Integer, ByVal referencia As IDATLET, ByVal join As datLigações)

            Dim m As New datMembro(nome, tipo, tamanho, precisao, escala, referencia, join)

            Me.MembroDef(m)

        End Sub

        Public Sub MembroDef(ByVal ParamArray MembrosArray() As String)

            Dim t As datMembro
            Dim m As Integer

            For m = MembrosArray.GetLowerBound(0) To MembrosArray.GetUpperBound(0)

                t = New datMembro(MembrosArray(m), Nothing, 0, 0, 0, Nothing, Nothing)

                Me.MembroDef(t)

            Next

        End Sub

        Public Sub MembroSetPonteiro()

            MaisUmPonteiro()
            MaisUmLista()
            MaisUmVista()
            MaisUmPesquisa()

            Ponteiro = Membros.GetUpperBound(0)
            Membros(Membros.GetUpperBound(0)).Ponteiro = True

            MembroPonteiro = Membros(Membros.GetUpperBound(0))

        End Sub

        Public Sub MembroSetChave()

            MaisUmChave()
            MaisUmLista()
            MaisUmPesquisa()

            Membros(Membros.GetUpperBound(0)).Chave = True
            Membros(Membros.GetUpperBound(0)).ChaveIndex = ArrayChave.GetUpperBound(0)

        End Sub

        Public Sub MembroSetDescrição()

            MaisUmDescrição()
            MaisUmLista()
            MaisUmVista()
            MaisUmLinkado()

            Membros(Membros.GetUpperBound(0)).Descritivo = True
            Membros(Membros.GetUpperBound(0)).DescritivoIndex = ArrayDescrição.GetUpperBound(0)

        End Sub

        Public Sub MembroSetLinkado()

            MaisUmLinkado()

            Membros(Membros.GetUpperBound(0)).Presente = True
            Membros(Membros.GetUpperBound(0)).PresenteIndex = ArrayLinkados.GetUpperBound(0)

        End Sub

        Public Sub MembroSetTudo()

            MembroSetPonteiro()
            MembroSetChave()
            MembroSetDescrição()

        End Sub

        Public Function MembroIsChave(ByVal Index As Integer) As Boolean

            Dim c As Integer

            MembroIsChave = False

            For c = 0 To ArrayChave.GetUpperBound(0)

                If ArrayChave(c) = Index Then
                    MembroIsChave = True
                    Exit For
                End If
            Next

        End Function

        Public Function MembroIsDescrição(ByVal Index As Integer) As Boolean

            Dim c As Integer

            MembroIsDescrição = False

            For c = 0 To ArrayDescrição.GetUpperBound(0)

                If ArrayDescrição(c) = Index Then
                    MembroIsDescrição = True
                    Exit For
                End If
            Next

        End Function

        Private Sub MaisUmPonteiro()

            ReDim ArrayPonteiro(0)
            ArrayPonteiro(0) = Me.Ponteiro

            ReDim MembrosPonteiro(0)
            MembrosPonteiro(0) = Me.Membros(Me.Ponteiro)

        End Sub

        Private Sub MaisUmChave()

            If ArrayChave Is Nothing Then ReDim ArrayChave(0) Else ReDim Preserve ArrayChave(ArrayChave.GetUpperBound(0) + 1)
            ArrayChave(ArrayChave.GetUpperBound(0)) = Membros.GetUpperBound(0)

            If MembrosChave Is Nothing Then ReDim MembrosChave(0) Else ReDim Preserve MembrosChave(MembrosChave.GetUpperBound(0) + 1)
            MembrosChave(MembrosChave.GetUpperBound(0)) = Membros(Membros.GetUpperBound(0))

        End Sub

        Private Sub MaisUmDescrição()

            If ArrayDescrição Is Nothing Then ReDim ArrayDescrição(0) Else ReDim Preserve ArrayDescrição(ArrayDescrição.GetUpperBound(0) + 1)
            ArrayDescrição(ArrayDescrição.GetUpperBound(0)) = Membros.GetUpperBound(0)

            If MembrosDescrição Is Nothing Then ReDim MembrosDescrição(0) Else ReDim Preserve MembrosDescrição(MembrosDescrição.GetUpperBound(0) + 1)
            MembrosDescrição(MembrosDescrição.GetUpperBound(0)) = Membros(Membros.GetUpperBound(0))

        End Sub

        Private Sub MaisUmTodos()

            ReDim Preserve ArrayTodos(Membros.GetUpperBound(0))
            ArrayTodos(Membros.GetUpperBound(0)) = Membros.GetUpperBound(0)

            ReDim Preserve MembrosTodos(Membros.GetUpperBound(0))
            MembrosTodos(Membros.GetUpperBound(0)) = Membros(Membros.GetUpperBound(0))

        End Sub

        Private Sub MaisUmLista()

            If ArrayLista Is Nothing Then ReDim ArrayLista(0) Else ReDim Preserve ArrayLista(ArrayLista.GetUpperBound(0) + 1)
            ArrayLista(ArrayLista.GetUpperBound(0)) = Membros.GetUpperBound(0)

            If MembrosLista Is Nothing Then ReDim MembrosLista(0) Else ReDim Preserve MembrosLista(MembrosLista.GetUpperBound(0) + 1)
            MembrosLista(MembrosLista.GetUpperBound(0)) = Membros(Membros.GetUpperBound(0))

        End Sub

        Private Sub MaisUmVista()

            If ArrayVista Is Nothing Then ReDim ArrayVista(0) Else ReDim Preserve ArrayVista(ArrayVista.GetUpperBound(0) + 1)
            ArrayVista(ArrayVista.GetUpperBound(0)) = Membros.GetUpperBound(0)

            If MembrosVista Is Nothing Then ReDim MembrosVista(0) Else ReDim Preserve MembrosVista(MembrosVista.GetUpperBound(0) + 1)
            MembrosVista(MembrosVista.GetUpperBound(0)) = Membros(Membros.GetUpperBound(0))

        End Sub

        Private Sub MaisUmPesquisa()

            If ArrayPesquisa Is Nothing Then ReDim ArrayPesquisa(0) Else ReDim Preserve ArrayPesquisa(ArrayPesquisa.GetUpperBound(0) + 1)
            ArrayPesquisa(ArrayPesquisa.GetUpperBound(0)) = Membros.GetUpperBound(0)

            If MembrosPesquisa Is Nothing Then ReDim MembrosPesquisa(0) Else ReDim Preserve MembrosPesquisa(MembrosPesquisa.GetUpperBound(0) + 1)
            MembrosPesquisa(MembrosPesquisa.GetUpperBound(0)) = Membros(Membros.GetUpperBound(0))

        End Sub

        Private Sub MaisUmLinkado()

            If ArrayLinkados Is Nothing Then ReDim ArrayLinkados(0) Else ReDim Preserve ArrayLinkados(ArrayLinkados.GetUpperBound(0) + 1)
            ArrayLinkados(ArrayLinkados.GetUpperBound(0)) = Membros.GetUpperBound(0)

            If MembrosLinkados Is Nothing Then ReDim MembrosLinkados(0) Else ReDim Preserve MembrosLinkados(MembrosLinkados.GetUpperBound(0) + 1)
            MembrosLinkados(MembrosLinkados.GetUpperBound(0)) = Membros(Membros.GetUpperBound(0))

        End Sub

        Public Function ColecçãoMembros(ByVal ParamArray MembrosIndex() As Integer) As datMembro()

            Dim i As Integer
            Dim colecção() As datMembro

            ReDim colecção(MembrosIndex.GetUpperBound(0))

            For i = 0 To MembrosIndex.GetUpperBound(0)

                colecção(i) = Me.Membros(MembrosIndex(i))

            Next

            Return colecção

        End Function

        Public ReadOnly Property Membro(ByVal MembroNome As String) As datMembro
            Get

                Dim i As Integer

                If Not Membros Is Nothing Then

                    For i = 0 To Membros.GetUpperBound(0)

                        If Membros(i).Nome = MembroNome Then Return Membros(i)

                    Next

                End If

                Return Nothing

            End Get
        End Property

        Public Function InscriçõesTodas() As datInscrição()

            Dim i As Integer
            Dim insc() As datInscrição

            ReDim insc(Membros.GetUpperBound(0))

            For i = 0 To Membros.GetUpperBound(0)

                insc(i).Membro = Membros(i)

            Next

            Return insc

        End Function

        Public Function Inscrições(ByVal ParamArray MembrosLista() As Integer) As datInscrição()

            Dim i As Integer
            Dim m As Integer
            Dim insc() As datInscrição

            ReDim insc(MembrosLista.GetUpperBound(0))

            For i = 0 To MembrosLista.GetUpperBound(0)

                m = MembrosLista(i)
                insc(i).Membro = Membros(m)

            Next

            Return insc

        End Function

    End Class

    Public Structure datMembro

        Public Nome As String
        Public Titulo As String
        Public Descrição As String

        Public DbAlias As String

        Public Tipo As System.Data.DbType
        Public Tamanho As Integer
        Public Precisao As Integer
        Public Escala As Integer

        Public Referencia As IDATLET
        Public ReferenciaObj As datObjecto
        Public ReferenciaTipo As datLigações
        Public ReferenciaIndex As Integer

        Public Ponteiro As Boolean
        Public Chave As Boolean
        Public ChaveIndex As Integer
        Public Descritivo As Boolean
        Public DescritivoIndex As Integer
        Public Presente As Boolean
        Public PresenteIndex As Integer

        Public Objecto As datObjecto
        Public Ordem As Integer
        Private Shared ReadOnly _locker As Object()

        Public Sub New(ByVal nome As String, ByVal tipo As System.Data.DbType, ByVal tamanho As Integer, ByVal precisao As Integer, ByVal escala As Integer, ByVal referencia As IDATLET, ByVal join As datLigações)

            Me.Nome = nome
            Me.Titulo = nome
            Me.Descrição = nome

            Me.DbAlias = nome
            Me.Tipo = tipo
            Me.Tamanho = tamanho
            Me.Precisao = precisao
            Me.Escala = escala

            If Not referencia Is Nothing Then
                Me.Referencia = referencia
                Me.ReferenciaObj = referencia.DatInfo.Objecto
                Me.ReferenciaTipo = join
            End If

            Me.Ponteiro = False
            Me.Chave = False
            Me.ChaveIndex = 0
            Me.Descritivo = False
            Me.DescritivoIndex = 0

            Me.Objecto = Nothing
            Me.Ordem = -1

        End Sub

    End Structure

    Public Structure datAcção

        Public Definida As Boolean
        Public Pedido As datPedido

        Public Preparada As Boolean
        Public Sintaxe As String

        Public Inscritos() As Integer
        Public Filtros() As Integer
        Public Ordenações() As Integer
        Public Aceites() As Integer

        Public ResultadoEscalar As Object
        Public ResultadoAfectados As Integer

        Public ParametrosValor() As Object
        Public ParametrosFiltro() As Object

        Public Construtor As ICONSTRUTOR
        Private Shared ReadOnly _locker As Object()

        Public Sub PreparaAcção(ByVal p_construtor As ICONSTRUTOR)

            Me.Construtor = p_construtor

            Me.Sintaxe = p_construtor.ConstroiSintaxe(Me.Pedido)

            Me.AssimilaPedido(Me.Pedido)

            Me.Preparada = True

        End Sub

        Public Sub PreparaAcção(ByVal p_construtor As ICONSTRUTOR, ByVal p_pedido As datPedido)

            Me.Pedido = p_pedido
            Me.PreparaAcção(p_construtor)

        End Sub

        Public Sub AssimilaPedido(ByVal p_pedido As datPedido)

            Dim m As Integer

            Me.Pedido = p_pedido

            '                                                                               

            If Pedido.Inscritos Is Nothing Then
                Inscritos = Nothing
            Else
                ReDim Inscritos(Pedido.Inscritos.GetUpperBound(0))
                For m = 0 To Pedido.Inscritos.GetUpperBound(0)
                    Inscritos(m) = Pedido.Inscritos(m).Membro.Ordem
                Next
            End If

            '                                                                               

            If Pedido.Filtros Is Nothing Then
                Filtros = Nothing
            Else
                ReDim Filtros(Pedido.Filtros.GetUpperBound(0))
                For m = 0 To Pedido.Filtros.GetUpperBound(0)
                    Filtros(m) = Pedido.Filtros(m).Membro.Ordem
                Next
            End If

            '                                                                               

            If Pedido.Aceites Is Nothing Then
                Aceites = Nothing
            Else
                ReDim Aceites(Pedido.Aceites.GetUpperBound(0))
                For m = 0 To Pedido.Aceites.GetUpperBound(0)
                    Aceites(m) = Pedido.Aceites(m).Membro.Ordem
                Next
            End If

            '                                                                               

            If Pedido.Ordenações Is Nothing Then
                Ordenações = Nothing
            Else
                ReDim Ordenações(Pedido.Ordenações.GetUpperBound(0))
                For m = 0 To Pedido.Ordenações.GetUpperBound(0)
                    Ordenações(m) = Pedido.Ordenações(m).Membro.Ordem
                Next
            End If

            '                                                                               

        End Sub

        Public Sub Reset()

            ResultadoAfectados = 0
            ResultadoEscalar = Nothing

        End Sub

    End Structure

    Public Class datPedido

        Public Operação As datOperações

        Public Objecto As datObjecto
        Public ComAlias As String

        Public Inscritos() As datInscrição
        Public Filtros() As datInscrição
        Public Ordenações() As datInscrição
        Public Aceites() As datAceitação

        Public Ligação As datLigações
        Public Index As Integer
        Public Nivel As Integer

        Public SubPedidos() As datPedido
        Public SubCondições()() As datInscrição

        Public Lock As datLocks


        Public Sub New()

        End Sub

        Public Sub New(ByVal objecto As datObjecto)

            Me.Objecto = objecto

        End Sub

        Public Sub New(ByVal objecto As datObjecto, ByVal operação As datOperações)

            Me.Objecto = objecto
            Me.Operação = operação

        End Sub

        Public Sub AutoCompleta()

            AutoCompleta(True)

        End Sub

        Public Sub AutoCompleta(ByVal TopoHierarquico As Boolean)

            Dim i As Integer
            Dim sp As Integer

            For i = 0 To Me.Inscritos.GetUpperBound(0)

                If Not Me.Inscritos(i).Membro.Referencia Is Nothing Then

                    sp = sp + 1

                    ReDim Preserve Me.SubPedidos(sp - 1)

                    Me.SubPedidos(sp - 1) = New datPedido(Me.Inscritos(i).Membro.ReferenciaObj)
                    Me.SubPedidos(sp - 1).Ligação = Me.Inscritos(i).Membro.ReferenciaTipo

                    Me.SubPedidos(sp - 1).MaisInscritos(datInscrição.Novas(Me.Inscritos(i).Membro.ReferenciaObj.MembrosLinkados))

                    ReDim Preserve Me.SubCondições(sp - 1)
                    ReDim Preserve Me.SubCondições(sp - 1)(0)

                    Me.SubCondições(sp - 1)(0).Pedido = Me.SubPedidos(sp - 1)
                    If Me.Inscritos(i).Membro.ReferenciaIndex = 0 Then
                        Me.SubCondições(sp - 1)(0).Membro = Me.Inscritos(i).Membro.ReferenciaObj.MembrosPonteiro(0)
                    Else
                        Me.SubCondições(sp - 1)(0).Membro = Me.Inscritos(i).Membro.ReferenciaObj.ColecçãoMembros(Me.Inscritos(i).Membro.ReferenciaIndex)(0)
                    End If
                    Me.SubCondições(sp - 1)(0).Operador = datOperadores.Igual
                    Me.SubCondições(sp - 1)(0).Parametro = Me.Inscritos(i)

                    Me.SubPedidos(sp - 1).AutoCompleta(False)

                End If

            Next

        End Sub

        Public Sub ResolveComAlias(ByVal TopoHierarquico As Boolean)

            Dim i As Integer
            Dim sp As Integer
            Dim tsp As Integer

            If TopoHierarquico Then Me.ComAlias = Me.Objecto.DbAlias

            If Not Me.SubPedidos Is Nothing Then

                sp = 0
                tsp = Me.Index

                For i = 0 To Me.SubPedidos.GetUpperBound(0)

                    sp = sp + 1
                    tsp = tsp + 1

                    Me.SubPedidos(i).Index = tsp
                    Me.SubPedidos(i).Nivel = Me.Nivel + 1

                    Me.SubPedidos(i).ComAlias = "J" & tsp

                    Me.SubPedidos(i).ResolveComAlias(False)

                Next

            End If

        End Sub

        Public Sub SupPedidoAdiciona(ByVal Tipo As datLigações, ByVal NovoPedido As datPedido, ByVal CondiçõesDeLigação() As datInscrição)

            Dim limite As Integer

            If SubPedidos Is Nothing Then
                ReDim SubPedidos(0)
                ReDim SubCondições(0)
            Else
                ReDim Preserve SubPedidos(SubPedidos.GetUpperBound(0) + 1)
                ReDim Preserve SubCondições(SubCondições.GetUpperBound(0) + 1)
            End If

            NovoPedido.Ligação = Tipo
            NovoPedido.Paternize(CondiçõesDeLigação)

            SubPedidos(SubPedidos.GetUpperBound(0)) = NovoPedido
            SubCondições(SubCondições.GetUpperBound(0)) = CondiçõesDeLigação

        End Sub

        Public Sub Paternize()

            Me.Paternize(Inscritos)

            Me.Paternize(Filtros)

            Me.Paternize(Ordenações)

            Me.Paternize(Aceites)

        End Sub

        Public Sub Paternize(ByRef Filhos() As datInscrição)

            Dim i As Integer

            If Not Filhos Is Nothing Then
                For i = 0 To Filhos.GetUpperBound(0)
                    Filhos(i).Pedido = Me

                    If Not Filhos(i).Parametro Is Nothing Then
                        If TypeOf Filhos(i).Parametro Is datInscrição Then
                            Dim m2 As datInscrição
                            m2 = CType(Filhos(i).Parametro, datInscrição)
                            If m2.Pedido Is Nothing Then m2.Pedido = Me
                        End If
                    End If

                Next
            End If

        End Sub

        Public Sub Paternize(ByRef Filhos() As datAceitação)

            Dim i As Integer

            If Not Filhos Is Nothing Then
                For i = 0 To Filhos.GetUpperBound(0)
                    Filhos(i).Pedido = Me
                Next
            End If

        End Sub

        Public Sub MaisInscritos(ByVal ParamArray NovasInscrições() As datInscrição)

            Dim limite As Integer

            If Not NovasInscrições Is Nothing Then

                Me.Paternize(NovasInscrições)

                If Me.Inscritos Is Nothing Then
                    limite = -1
                Else
                    limite = Me.Inscritos.GetUpperBound(0)
                End If

                ReDim Preserve Me.Inscritos(limite + NovasInscrições.GetUpperBound(0) + 1)
                NovasInscrições.CopyTo(Me.Inscritos, limite + 1)

            End If

        End Sub

        Public Sub MaisFiltros(ByVal ParamArray NovosFiltros() As datInscrição)

            Dim limite As Integer

            If Not NovosFiltros Is Nothing Then

                Me.Paternize(NovosFiltros)

                If Me.Filtros Is Nothing Then
                    limite = -1
                Else
                    limite = Me.Filtros.GetUpperBound(0)
                End If

                ReDim Preserve Me.Filtros(limite + NovosFiltros.GetUpperBound(0) + 1)
                NovosFiltros.CopyTo(Me.Filtros, limite + 1)

            End If

        End Sub

        Public Sub MaisOrdenações(ByVal ParamArray NovasOrdenações() As datInscrição)

            Dim limite As Integer

            If Not NovasOrdenações Is Nothing Then

                Me.Paternize(NovasOrdenações)

                If Ordenações Is Nothing Then
                    limite = -1
                Else
                    limite = Me.Ordenações.GetUpperBound(0)
                End If

                ReDim Preserve Me.Ordenações(limite + NovasOrdenações.GetUpperBound(0) + 1)
                NovasOrdenações.CopyTo(Me.Ordenações, limite + 1)

            End If

        End Sub

        Public Sub MaisInscritos(ByVal ParamArray MembrosIndices() As Integer)

            Dim novas_inscrições() As datInscrição

            ReDim novas_inscrições(MembrosIndices.GetUpperBound(0))

            For m As Integer = 0 To MembrosIndices.GetUpperBound(0)

                novas_inscrições(m) = New datInscrição(Me.Objecto.Membros(MembrosIndices(m)))

            Next

            Me.Paternize(novas_inscrições)

            Me.MaisInscritos(novas_inscrições)

        End Sub

        Public Sub MaisFiltros(ByVal ponte As datPontes, ByVal membro As Integer, ByVal operador As datOperadores, ByVal parametro As String)

            Dim nova_inscrição As New datInscrição

            nova_inscrição.Ponte = ponte
            nova_inscrição.Membro = Me.Objecto.Membros(membro)
            nova_inscrição.Operador = operador
            nova_inscrição.Parametro = parametro

            Me.MaisFiltros(nova_inscrição)

        End Sub

        Public Sub MaisFiltros(ByVal ponte As datPontes, ByVal membro As Integer, ByVal operador As datOperadores, ByVal parametro As String, ByVal PriorSobe As Integer, ByVal PriorDesce As Integer)

            Dim nova_inscrição As New datInscrição

            nova_inscrição.Ponte = ponte
            nova_inscrição.Membro = Me.Objecto.Membros(membro)
            nova_inscrição.Operador = operador
            nova_inscrição.Parametro = parametro
            nova_inscrição.PriorSobe = PriorSobe
            nova_inscrição.PriorDesce = PriorDesce

            Me.MaisFiltros(nova_inscrição)

        End Sub

    End Class

    Public Structure datInscrição

        Public Membro As datMembro              ' membro que a inscrição representa

        Public Direcção As datDirecções         ' direcção que a inscrição toma (ascendente, descendente)
        Public FAgregada As datFunção           ' função agregado à inscrição

        Public Operador As datOperadores        ' operador de condição (se a inscrição servir uma condição)
        Public Ponte As datPontes               ' ponte para a condição anterior (»»)
        Public Parametro As Object              ' parametro para 2º membro da condição (»»)
        Public PriorSobe As Integer             ' niveis de prioridade acima da condição anterior (»»)
        Public PriorDesce As Integer            ' niveis de prioridade abaixo da condição anterior (»»)
        Public NegaAntes As Boolean
        Public NegaDepois As Boolean

        Public Pedido As datPedido              ' pedido ao qual esta inscrição está associada

        Private Shared ReadOnly _locker As Object()

        Public Sub New(ByVal ponte As datPontes, ByVal membro As datMembro, ByVal operador As datOperadores, ByVal parametro As String)

            Me.Ponte = ponte
            Me.Membro = membro
            Me.Operador = operador
            Me.Parametro = parametro

        End Sub

        Public Sub New(ByVal membro As datMembro)

            Me.Membro = membro

        End Sub

        Shared Function Novas(ByVal ParamArray inscritos() As datMembro) As datInscrição()

            Dim i As Integer
            Dim maisum() As datInscrição

            ReDim maisum(inscritos.GetUpperBound(0) - inscritos.GetLowerBound(0))

            For i = inscritos.GetLowerBound(0) To inscritos.GetUpperBound(0)

                maisum(i).Membro = inscritos(i)

            Next

            Return maisum

        End Function

        Shared Function Novas(ByVal membro As datMembro, ByVal operador As datOperadores, ByVal parametro As Object) As datInscrição()

            Dim maisum(0) As datInscrição

            maisum(0).Membro = membro
            maisum(0).Operador = operador
            maisum(0).Parametro = parametro

            Return maisum

        End Function

        Shared Function Novas(ByVal membro() As datMembro, ByVal operador As datOperadores, ByVal parametro As Object) As datInscrição()

            Dim i As Integer
            Dim maisumas() As datInscrição

            ReDim maisumas(membro.GetUpperBound(0) - membro.GetLowerBound(0))

            For i = membro.GetLowerBound(0) To membro.GetUpperBound(0)

                maisumas(i).Membro = membro(i)
                If i > membro.GetLowerBound(0) Then maisumas(i).Ponte = datPontes.Ou
                maisumas(i).Operador = operador
                maisumas(i).Parametro = parametro

            Next


        End Function

        Shared Function Novas(ByVal ponte As datPontes, ByVal membro As datMembro, ByVal operador As datOperadores, ByVal parametro As String) As datInscrição()

            Dim maisum(0) As datInscrição

            maisum(0).Ponte = ponte
            maisum(0).Membro = membro
            maisum(0).Operador = operador
            maisum(0).Parametro = parametro

            Return maisum

        End Function

        Shared Function Novas(ByVal membro As datMembro, ByVal Agregada As datFAgregadas) As datInscrição()

            Dim maisum(0) As datInscrição

            maisum(0).Membro = membro
            maisum(0).FAgregada = New datFunção(Agregada)

            Return maisum

        End Function

        Shared Function Novas(ByVal membro As datMembro, ByVal direcção As datDirecções) As datInscrição()

            Dim maisum(0) As datInscrição

            maisum(0).Membro = membro
            maisum(0).Direcção = direcção

            Return maisum

        End Function

        Shared Sub Adiciona(ByRef Array() As datInscrição, ByVal ParamArray ArrayAAdicionar() As datInscrição)

            Dim limite As Integer

            If Not ArrayAAdicionar Is Nothing Then

                If Array Is Nothing Then
                    limite = -1
                Else
                    limite = Array.GetUpperBound(0)
                End If

                ReDim Preserve Array(limite + ArrayAAdicionar.GetUpperBound(0) + 1)
                ArrayAAdicionar.CopyTo(Array, limite + 1)

            End If

        End Sub

        Shared Sub Adiciona(ByRef Array() As datInscrição, ByVal ParamArray MembrosAAdicionar() As datMembro)

            Dim m As Integer
            Dim limite As Integer
            Dim novas() As datInscrição

            If Not MembrosAAdicionar Is Nothing Then

                ReDim novas(MembrosAAdicionar.GetUpperBound(0))

                For m = 0 To MembrosAAdicionar.GetUpperBound(0)

                    novas(m).Membro = MembrosAAdicionar(m)

                Next

                datInscrição.Adiciona(Array, novas)

            End If

        End Sub

        Shared Sub Paternize(ByVal Pedido As datPedido, ByVal ParamArray Inscritos() As datInscrição)

            Dim i As Integer

            If Not Inscritos Is Nothing Then
                For i = 0 To Inscritos.GetUpperBound(0)
                    Inscritos(i).Pedido = Pedido
                Next
            End If

        End Sub

    End Structure

    Public Structure datAceitação

        Public Membro As datMembro
        Public JoinIndex As Integer
        Public JoinNivel As Integer
        Public Pedido As datPedido

        Private Shared ReadOnly _locker As Object()

        Shared Function Novas(ByVal ParamArray Inscrições() As datMembro) As datAceitação()

            Dim i As Integer
            Dim maisum() As datAceitação

            ReDim maisum(Inscrições.GetUpperBound(0) - Inscrições.GetLowerBound(0))

            For i = Inscrições.GetLowerBound(0) To Inscrições.GetUpperBound(0)

                maisum(i).Membro = Inscrições(i)

            Next

            Return maisum

        End Function

        Shared Function Novas(ByVal Agregador As datMembro, ByVal JoinIndex As Integer, ByVal JoinNivel As Integer, ByVal ParamArray Inscrições() As datMembro) As datAceitação()

            Dim i As Integer
            Dim maisum() As datAceitação

            ReDim maisum(Inscrições.GetUpperBound(0) - Inscrições.GetLowerBound(0))

            For i = Inscrições.GetLowerBound(0) To Inscrições.GetUpperBound(0)

                maisum(i).Membro = Inscrições(i)
                maisum(i).JoinIndex = JoinIndex
                maisum(i).JoinNivel = JoinNivel

            Next

            Return maisum

        End Function

        Shared Function Novas(ByVal Inscritos() As datInscrição) As datAceitação()

            Dim i As Integer
            Dim maisum() As datAceitação

            ReDim maisum(Inscritos.GetUpperBound(0) - Inscritos.GetLowerBound(0))

            For i = Inscritos.GetLowerBound(0) To Inscritos.GetUpperBound(0)

                maisum(i).Membro = Inscritos(i).Membro

            Next

            Return maisum

        End Function

        Shared Function Novas(ByVal Pedido As datPedido) As datAceitação()

            Dim i As Integer
            Dim j As Integer
            Dim aceites() As datAceitação

            If Not Pedido.Inscritos Is Nothing Then

                ReDim aceites(Pedido.Inscritos.GetUpperBound(0))

                For i = 0 To Pedido.Inscritos.GetUpperBound(0)

                    aceites(i).Membro = Pedido.Inscritos(i).Membro
                    aceites(i).JoinIndex = Pedido.Index
                    aceites(i).JoinNivel = Pedido.Nivel

                Next

            End If

            If Not Pedido.SubPedidos Is Nothing Then

                For j = 0 To Pedido.SubPedidos.GetUpperBound(0)

                    datAceitação.Adiciona(aceites, datAceitação.Novas(Pedido.SubPedidos(j)))

                Next

            End If

            Return aceites

        End Function

        Shared Sub Adiciona(ByRef Array() As datAceitação, ByVal ParamArray ArrayAAdicionar() As datAceitação)

            Dim limite As Integer

            If Not ArrayAAdicionar Is Nothing Then

                If Array Is Nothing Then
                    limite = -1
                Else
                    limite = Array.GetUpperBound(0)
                End If

                ReDim Preserve Array(limite + ArrayAAdicionar.GetUpperBound(0) + 1)
                ArrayAAdicionar.CopyTo(Array, limite + 1)

            End If

        End Sub

        Shared Sub Config(ByVal Inscritos() As datAceitação, ByVal JoinIndex As Integer, ByVal JoinNivel As Integer)

            Dim i As Integer

            For i = Inscritos.GetLowerBound(0) To Inscritos.GetUpperBound(0)

                Inscritos(i).JoinIndex = JoinIndex
                Inscritos(i).JoinNivel = JoinNivel

            Next

        End Sub

        Shared Sub Paternize(ByVal Pedido As datPedido, ByVal ParamArray Inscritos() As datAceitação)

            Dim i As Integer

            If Not Inscritos Is Nothing Then
                For i = 0 To Inscritos.GetUpperBound(0)
                    Inscritos(i).Pedido = Pedido
                Next
            End If

        End Sub

    End Structure

    Public Structure datFunção

        Public Função As datFAgregadas
        Public Parametros() As Object

        Private Shared ReadOnly _locker As Object()

        Public Sub New(ByVal Fun As datFAgregadas)

            Função = Fun

        End Sub

        Public Sub New(ByVal Fun As datFAgregadas, ByVal ParamArray Pars() As Object)

            Função = Fun

            Dim i As Integer

            ReDim Parametros(Pars.GetUpperBound(0) - Pars.GetLowerBound(0))

            For i = Pars.GetLowerBound(0) To Pars.GetUpperBound(0)

                Parametros(i) = Pars(i)

            Next

        End Sub

    End Structure

    '                                                                                                       

    Public Structure datAtributo

        Public Nome As String
        Public Titulo As String
        Public Descrição As String

        ' tipo de dado que o atributo suporta 
        Public Tipo As System.Data.DbType

        ' se o membro for "-1" quer dizer que não representa um membro original do objecto de dados
        ' mas sim um atributo computado a partir de outros
        Public Membro As Integer

        ' o atributo pode apresentar-se sobre forma individual, array ou em coleccção
        Public IsArray As Boolean
        Public IsCollection As Boolean

        ' o atributo (original ou computado) pode representar outra datlet
        Public IsDatlet As Boolean

        ' o atributo é apenas de leitura?
        Public IsReadonly As Boolean

        ' o conteudo do atributo pode depender do conteudo de um outro atributo (Frank 14/05/2004)
        Public IsDependente As Boolean
        Public AtributosPai As Integer()

        Private Shared ReadOnly _locker As Object()

        Public Sub Def(ByVal Membro As datMembro)

            Me.Nome = Membro.Nome
            Me.Titulo = Membro.Titulo
            Me.Descrição = Membro.Descrição
            Me.Tipo = Membro.Tipo
            Me.Membro = Membro.Ordem
            Me.IsArray = False
            Me.IsCollection = False
            Me.IsDatlet = False

        End Sub

        Shared Sub Adiciona(ByRef ArrayDeDescrições As datAtributo(), ByVal Descrição As datAtributo)

            If ArrayDeDescrições Is Nothing Then
                ReDim ArrayDeDescrições(0)
            Else
                ReDim Preserve ArrayDeDescrições(ArrayDeDescrições.GetUpperBound(0) + 1)
            End If

            ArrayDeDescrições(ArrayDeDescrições.GetUpperBound(0)) = Descrição

        End Sub

        Shared Function Nova(ByVal Nome As String, ByVal Tipo As System.Data.DbType, ByVal IsArray As Boolean, ByVal IsCollection As Boolean, ByVal IsDatlet As Boolean) As datAtributo

            Dim novo_attr As datAtributo

            novo_attr.Nome = Nome
            novo_attr.Titulo = Nome
            novo_attr.Tipo = Tipo
            novo_attr.Membro = -1
            novo_attr.IsArray = IsArray
            novo_attr.IsCollection = IsCollection
            novo_attr.IsDatlet = IsDatlet

            Return novo_attr

        End Function

    End Structure

    Public Structure datLigação

        ' tipo da ligação aqui descrita 
        Public Tipo As datLigações

        ' Namespace (módulo) e nome da datlet à qual a ligação se refere
        Public CatNamespace As String
        Public CatAlias As String

        ' atributo na ligação origem 
        Public AtributoLiga As Integer

        ' atributo na ligação destino
        Public AtributoLigado As Integer

    End Structure

    Public Class datInfo

        ' identificação da datlet
        Public Titulo As String
        Public Descrição As String

        ' descrição do recipiente de dados que a datlet representa
        Public Objecto As datObjecto

        ' atributos que expoe 
        Public Atributos() As datAtributo

        ' ligações nas quais participa
        Public Ligações() As datLigação



        Public Sub AtributoDef(ByVal Objecto As datObjecto)

            Dim i As Integer
            Dim a As Integer

            If Atributos Is Nothing Then
                i = 0
                ReDim Atributos(Objecto.Membros.GetUpperBound(0))
            Else
                i = Atributos.GetUpperBound(0) + 1
                ReDim Preserve Atributos(Atributos.GetUpperBound(0) + Objecto.Membros.GetUpperBound(0) + 1)
            End If

            For a = 0 To Objecto.Membros.GetUpperBound(0)

                Atributos(i + a).Def(Objecto.Membros(a))

            Next

        End Sub

        Public Sub AtributoDef(ByVal Nome As String, ByVal Tipo As System.Data.DbType, ByVal IsArray As Boolean, ByVal IsCollection As Boolean, ByVal IsDatlet As Boolean, ByVal IsReadOnly As Boolean)

            Dim a As Integer

            If Atributos Is Nothing Then
                a = 0
                ReDim Atributos(0)
            Else
                a = Atributos.GetUpperBound(0) + 1
                ReDim Preserve Atributos(a)
            End If

            Atributos(a).Nome = Nome
            Atributos(a).Titulo = Nome
            Atributos(a).Tipo = Tipo
            Atributos(a).IsArray = IsArray
            Atributos(a).IsCollection = IsCollection
            Atributos(a).IsDatlet = IsDatlet
            Atributos(a).Membro = -1

        End Sub

        Public Sub AtributoDef(ByVal Nome As String, ByVal Tipo As System.Data.DbType, ByVal IsArray As Boolean, ByVal IsCollection As Boolean, ByVal IsDatlet As Boolean, ByVal IsReadOnly As Boolean, ByVal ParamArray AtributosPai() As Integer)

            Dim a As Integer

            If Atributos Is Nothing Then
                a = 0
                ReDim Atributos(0)
            Else
                a = Atributos.GetUpperBound(0) + 1
                ReDim Preserve Atributos(a)
            End If

            Atributos(a).Nome = Nome
            Atributos(a).Titulo = Nome
            Atributos(a).Tipo = Tipo
            Atributos(a).IsArray = IsArray
            Atributos(a).IsCollection = IsCollection
            Atributos(a).IsDatlet = IsDatlet
            Atributos(a).IsDependente = True
            Atributos(a).AtributosPai = AtributosPai
            Atributos(a).Membro = -1

        End Sub

        Public Sub LigaçãoDef(ByVal AtributoLiga As Integer, ByVal CatNamespace As String, ByVal CatAlias As String, ByVal AtributoLigado As Integer, ByVal Tipo As datLigações)

            Dim l As Integer

            If Ligações Is Nothing Then
                l = 0
                ReDim Ligações(0)
            Else
                l = Ligações.GetUpperBound(0) + 1
                ReDim Preserve Ligações(l)
            End If

            Ligações(l).AtributoLiga = AtributoLiga
            Ligações(l).AtributoLigado = AtributoLigado
            Ligações(l).CatAlias = CatAlias
            Ligações(l).CatNamespace = CatNamespace
            Ligações(l).Tipo = Tipo

        End Sub

    End Class

    Public Interface ILET

        Property DatInfo() As datInfo

        Property DatAtributo(ByVal Index As Integer) As Object

        Function DatClone() As ILET

        Function DatNew() As ILET

        Function DatCast(ByVal Lista() As ILET) As ILET()

        Sub DatCopy(ByVal datalet As ILET)

        Function ToString() As String

    End Interface

    Public Interface IDBLET
        Inherits ILET

        Property DbConector() As System.Data.IDbConnection

        Property DbConstrutor() As datConstrutorSQL

        Property DbExcepção() As System.Exception

        Function DbPrepara(ByVal Alcance As Integer) As Boolean

        Function DbExecuta() As Boolean

        Function DbConsulta() As Boolean
        Function DbConsulta(ByVal Escalar As Boolean) As Boolean

        Function DbSelecciona() As IDBLET()

        ReadOnly Property DbAfectados() As Integer

        Function DbSaca(ByVal Cursor As System.Data.IDataReader, ByVal CursorFetch As Boolean) As Boolean
        Function DbSaca(ByVal Cursor As System.Data.IDataReader, ByVal CursorFetch As Boolean, ByVal InscriçõesAceites() As datAceitação, ByRef JoinIndex As Integer) As Boolean

        Event DbJoin(ByVal Agregador As datMembro, ByVal Cursor As System.Data.IDataReader, ByVal InscriçõesAceites() As datAceitação, ByRef JoinIndex As Integer)

        Function DbLink(ByVal Objecto As datObjecto) As Integer()

        Sub DbReset()

        Sub DbDispose()

        Property DbParametros() As Object()

        Property DbAcção() As datAcção

    End Interface

    Public Interface IXMLLET
        Inherits ILET

        ' nome do ficheiro a usar para entrada e saida de dados
        Property XmlFicheiro() As String

        ' nó base numa estrutura XML a usar para suporte dos dados
        Property XmlNoBase() As System.Xml.XmlNode

        ' ultimo eerro causado por uma operação XML
        Property XmlExcepção() As System.Exception

        ' preparação da próxima operação XML
        ' a preparação consiste na definição do tipo de comando e do alcance
        ' o alcance consiste na definição das colunas seleccionadas e dos filtros
        Function XmlPrepara(ByVal Acção As Integer) As Boolean

        ' entrada de dados para a própria classe
        ' este método retorna o sucesso da execução e não qq tipo de resultados
        Function XmlConsulta() As Boolean

        ' entrada de dados para cada classe de um array a retornar
        ' este método retorna os resultados sob a forma de um array do tipo da classe em questão
        Function XmlSelecciona() As IXMLLET()

        ' escreve os dados da própria classe na estrutura XML
        ' este método retorna o sucesso da execução e não qq tipo de resultados
        Function XmlEscreve() As Boolean

        ' escreve os dados oriundos de cada classe de um array recebido na estrutura XML
        ' este método retorna o sucesso da execução e não qq tipo de resultados
        Function XmlEscreve(ByVal Lista() As IXMLLET) As Boolean

        ' elimina os dados apontados pela própria classe na estrutura XML
        ' este método retorna o sucesso da execução e não qq tipo de resultados
        Function XmlElimina() As Boolean

        ' retorna o numero de registos afectados pela acção anterior 
        ReadOnly Property XmlAfectados() As Integer

        ' carrega dados dos atributos da própria classe no nó XML indicado
        Function XmlSaca(ByVal No As System.Xml.XmlNode, ByVal InscriçõesAceites() As Integer) As Boolean

        ' carrega o nó XML com os dados da própria classe
        Function XmlEnche(ByVal No As System.Xml.XmlNode, ByVal InscriçõesAceites() As Integer) As Boolean

        ' reinicia os componentes da classe
        Sub XmlReset()

        ' liberta todos os recursos usados pela classe
        Sub XmlDispose()

        ' array dos parametros a usar na próxima acção
        Property XmlParametros() As Object()

        ' acção presentemente preparada
        Property XmlAcção() As datAcção

    End Interface

    Public Interface ICONSTRUTOR

        Function ConstroiSintaxe(ByVal Pedido As datPedido) As String

        Function SintaxeDeConsulta(ByVal Pedido As datPedido) As String
        Function SintaxeDeAlteração(ByVal Pedido As datPedido) As String
        Function SintaxeDeInserção(ByVal Pedido As datPedido) As String
        Function SintaxeDeEliminação(ByVal Pedido As datPedido) As String

        Function SintaxeDeFiltro(ByVal Pedido As datPedido) As String
        Function SintaxeDeOrdenação(ByVal Pedido As datPedido) As String

        Function SintaxeDeInscrição(ByVal Inscrição As datInscrição) As String
        Function SintaxeDeCondição(ByVal Condições() As datInscrição, ByVal ForçaPonte As datPontes) As String
        Function SintaxeDeFAgregada(ByVal Inscrito As datInscrição) As String

        Function SintaxeDeMembro(ByVal Membro As datInscrição) As String
        Function SintaxeDeParametroValor(ByVal Membro As datMembro) As String
        Function SintaxeDeParametroFiltro(ByVal Membro As datMembro) As String

        Function SintaxeDeOperador(ByVal Operador As datOperadores) As String
        Function SintaxeDePonte(ByVal Pontes As datPontes) As String

    End Interface

    Public Interface IADAPTADOR

        Function Consulta(ByRef Acção As datAcção, ByVal DatletACarregar As IDATLET) As Boolean
        Function ConsultaEscalar(ByRef Acção As datAcção) As Boolean

        Function Selecciona(ByRef Acção As datAcção, ByVal DatletAClonar As IDATLET) As IDATLET()

        Function Executa(ByRef Acção As datAcção) As Boolean

        ReadOnly Property Natureza() As datNaturezas

        Function TransaçãoAbre() As Boolean
        Function TransaçãoCommit() As Boolean
        Function TransaçãoRollback() As Boolean

        Property Excepção() As System.Exception

    End Interface

    Public Class datCanal

        Public Construtor As ICONSTRUTOR
        Public Adaptador As IADAPTADOR

    End Class

    Public Structure datFetcher

        ' Esta classe permite rolar sobre um cursor mantendo a possibilidade de no ciclo seguinte
        ' (se encessário) rastrear a mesma linha, isto consegue-se suspendendo o fetch seguinte 
        ' mas retornando na mesma "TRUE"

        ' pensar: datFetcher não é lá uma coisa muito elegante, muito processamento! 
        ' por agora não está a ser usada!!!

        Public FixaEstaLinha As Boolean
        Public TinhaLinha As Boolean

        Private Shared ReadOnly _locker As Object()

        Public Function TemLinhas(ByVal Cursor As System.Data.IDataReader) As Boolean

            ' Este procedimento simula o fecth sobre o cursor. Na verdade
            ' em procedimento normal o fecth ao cursor é realmente executado,
            ' porém se a propriedade "FixaEstaLinha" for activada a próxima
            ' evocação salta o fecth e deixa o cursor no mesmo ponto.

            If FixaEstaLinha Then
                FixaEstaLinha = False
                Return TinhaLinha
            Else
                TinhaLinha = Cursor.Read
                Return TinhaLinha
            End If

        End Function

    End Structure

    '                                                                                                       

    Public Interface IDATLET
        Inherits ILET

        ' canal de acesso á origem de dados
        Property DatCanal() As datCanal

        ' acção presentemente preparada
        Property DatAcção() As datAcção

        ' array de parametros para usos particulares
        ' o conteudo de cada posição deve reflectir o filtro da acção preparada
        Property DatParametros() As Object()

        ' preparação de uma acção
        Sub DatPrepara(ByVal Alcance As Integer)

        ' selecciona todos os campos do registo condicionado pela chave
        Function DatConsultaFicha() As Boolean
        Function DatConsultaFicha(ByVal ParamArray Chave() As Object) As Boolean

        ' selecciona todos os campos do registo condicionado pelo ponteiro
        Function DatConsultaRegisto() As Boolean
        Function DatConsultaRegisto(ByVal Ponteiro As Integer) As Boolean

        ' seleciona ponteiro e descrição do registo condicionado pelo ponteiro
        Function DatConsultaVista() As Boolean
        Function DatConsultaVista(ByVal Ponteiro As Integer) As Boolean

        ' selecciona ponteiro e chave dos registos condicionado pelo descritivo
        Function DatPesquisa() As IDATLET()
        Function DatPesquisa(ByVal ParamArray Descrição() As Object) As IDATLET()

        ' selecciona todos os campos do primeiro registo 
        Function DatConsultaPrimeiro() As Boolean

        ' selecciona todos os campos de todos os registos ordenados pela chave
        Function DatSeleccionaFichas() As IDATLET()

        ' selecciona ponteiro, chave e descrição de todas os registos ordenadas pela chave
        Function DatSeleccionaLista() As IDATLET()

        ' verifica se o registo condicionado pela chave existe 
        Function DatExiste() As Boolean
        Function DatExiste(ByVal ParamArray Chave() As Object) As Boolean

        ' verifica se o registo condicionado pelo ponteiro existe 
        Function DatVerifica() As Boolean
        Function DatVerifica(ByVal Ponteiro As Object) As Boolean

        ' define o conteudo do ponteiro para um novo registo
        Function DatAutoId() As Boolean

        ' actualiza todos os campos no registos condicionado pelo ponteiro
        Function DatAltera() As Boolean

        ' insere todos os campos num novo registo
        Function DatInsere() As Boolean

        ' elimina o registo condicionado pelo ponteiro
        Function DatElimina() As Boolean

        ' actualiza na origem dos dados o registo
        ' para isso tem que verificar se o registo já existe ou não
        ' para evocar ou o DatInsere(+DatAutoID) ou o DatAltera
        Function DatActualiza() As Boolean

        ' filtro a aplicar sobre qualquer acção 
        Property DatFiltros() As datInscrição()

        ' erro provocado pelo ultimo acesso à origem de dados por uma acção da classe
        Property DatExcepção() As System.Exception

        ' função a evocar quando for necessário que a classe redireccione dados para outras classes
        ' a função retorna o adaptador a usar e a classe destino
        'Function DatReferencia(ByVal Referenciador As phFRAME.Dataframe.datMembro, ByRef getDatlet As IDATLET, ByRef getAgregados As Boolean) As Boolean
        ' uma vez que os membros conteem uma referencia à datlet que referenciam podemos usa-la para recolher os dados

        ' reinicia os componentes da classe
        ' este método não é um método de inicialização da classe para existem os contrutores,
        ' este método serve para durante o seu uso levar a classe a um estado neutro
        Sub DatReset()

        ' liberta todos os recursos usados pela classe
        Sub DatDispose()

    End Interface

    Public Class datColecçãoDeDatlets

        Public Lista() As IDATLET



        Public Sub New()

        End Sub

        Public Sub New(ByVal lista_inicial() As IDATLET)

            Me.Lista = lista_inicial

        End Sub

        Public Function DatActualiza() As Boolean

            Dim ok As Boolean

            ok = True

            For Each datlet As IDATLET In Lista

                ok = ok And datlet.DatActualiza

            Next

        End Function

    End Class

    '                                                                                                       

    Public Class ExcepçãoConstruçãoSQLTipoDestinoNãoImplementado
        Inherits System.Exception



        Public Overrides ReadOnly Property Message() As String
            Get

                Return "O tipo de dados destino da conversão não está implementado!" & vbCrLf & vbCrLf & "Esta implementação deve ser feita no método StringFAgregadas da classe datConstrutorSQL ou suas derivadas."

            End Get
        End Property

    End Class

    Public Class ExcepçãoConstruçãoSQLFAgregadoNãoImplementado
        Inherits System.Exception



        Public Overrides ReadOnly Property Message() As String
            Get

                Return "A função agregada indicada não está implementado!" & vbCrLf & vbCrLf & "Esta implementação deve ser feita no método StringFAgregadas da classe datConstrutorSQL ou suas derivadas."

            End Get
        End Property

    End Class

    Public Class ExcepçãoTypeCodeNãoImplementado
        Inherits System.Exception



        Public Overrides ReadOnly Property Message() As String
            Get

                Return "O Type corrospondente ao TypeCode pedido ainda não foi implementado!" & vbCrLf & vbCrLf & "Esta implementação deve ser feita na função SystemTypeCode do módulo Util."

            End Get
        End Property

    End Class

    Public Class ExcepçãoDbTypeNãoImplementado
        Inherits System.Exception



        Public Overrides ReadOnly Property Message() As String
            Get

                Return "O Type corrospondente ao DBType pedido ainda não foi implementado!" & vbCrLf & vbCrLf & "Esta implementação deve ser feita na função SystemType do módulo Util."

            End Get
        End Property

    End Class

    Public Module Dataframe
        Private ReadOnly _locker As New Object()

        Public Function SystemType(ByVal DbType As System.Data.DbType) As System.Type

            Select Case DbType

                Case System.Data.DbType.Int32 : Return System.Type.GetType("System.Int32")
                Case System.Data.DbType.String : Return System.Type.GetType("System.String")
                Case System.Data.DbType.Boolean : Return System.Type.GetType("System.Boolean")

                Case Else

                    Stop
                    'Basframe.ErroLogger.DeuExcepção(New ExcepçãoDbTypeNãoImplementado, DbType.ToString, True, False, False)

            End Select

        End Function

        Public Function SystemTypeCode(ByVal TypeCode As System.TypeCode) As System.Type

            Select Case TypeCode

                Case System.TypeCode.Int32 : Return System.Type.GetType("System.Int32")
                Case System.TypeCode.String : Return System.Type.GetType("System.String")
                Case System.TypeCode.Boolean : Return System.Type.GetType("System.Boolean")

                Case Else

                    Stop
                    'Basframe.ErroLogger.DeuExcepção(New ExcepçãoTypeCodeNãoImplementado, TypeCode.ToString, True, False, False)

            End Select

        End Function

        Public Function SystemTypeCodeAlfa(ByVal Tipo As System.TypeCode) As Boolean

            Select Case Tipo

                Case TypeCode.Char, TypeCode.DateTime, TypeCode.String

                    Return True

                Case Else

                    Return False

            End Select

        End Function

        Public Function SystemDbTypeAlfa(ByVal Tipo As System.Data.DbType) As Boolean

            Select Case Tipo
                Case DbType.AnsiString, _
                        DbType.AnsiStringFixedLength, _
                        DbType.String, _
                        DbType.StringFixedLength
                    Return True
                Case Else
                    Return False
            End Select

        End Function

        Public Function SystemDbTypeNumerico(ByVal Tipo As System.Data.DbType) As Boolean

            Select Case Tipo
                Case DbType.Byte, _
                        DbType.Currency, _
                        DbType.Decimal, _
                        DbType.Double, _
                        DbType.Int16, _
                        DbType.Int32, _
                        DbType.Int64, _
                        DbType.[SByte], _
                        DbType.Single, _
                        DbType.UInt16, _
                        DbType.UInt32, _
                        DbType.UInt64, _
                        DbType.VarNumeric
                    Return True
                Case Else
                    Return False
            End Select

        End Function

        Public Function SystemDbTypeDataHora(ByVal Tipo As System.Data.DbType) As Boolean

            Select Case Tipo
                Case DbType.Date, DbType.DateTime, DbType.Time
                    Return True
                Case Else
                    Return False
            End Select

        End Function


        Public Function XMLGetNo(ByVal NoBase As System.Xml.XmlNode, ByVal NoPedidoNome As String) As System.Xml.XmlNode

            Dim NoPedido As System.Xml.XmlNode

            If NoBase.Name = NoPedidoNome Then

                Return NoBase

            Else

                If NoBase.HasChildNodes Then

                    For Each NoPedido In NoBase.ChildNodes
                        If NoPedido.Name = NoPedidoNome Then Return NoPedido
                    Next

                    For Each NoPedido In NoBase.ChildNodes
                        XMLGetNo = XMLGetNo(NoPedido, NoPedidoNome)
                        If Not XMLGetNo Is Nothing Then Return XMLGetNo
                    Next

                    Return Nothing

                Else

                    Return Nothing

                End If

            End If

        End Function

        Public Function XMLGetNo(ByVal NoBase As System.Xml.XmlNode, ByVal XmlFicheiro As String, ByVal NoPedidoNome As String, ByVal XmlFicheiroCria As Boolean, ByRef getXmlDoc As System.Xml.XmlDocument, ByRef getXmlExcepção As System.Exception) As System.Xml.XmlNode

            ' procura um determinado nó dentro de um nó indicado ou então dentro do um ficheiro xml
            ' o ficheiro xml pode ser criado caso não exista

            ' se o NoBase não foi indicado quer dizer que temos que abrir o ficheiro XML
            ' caso o ficheiro XML pedido não exista será criado (se tem a autorização, claro!)
            ' no caso de abertura de ficheiro XML o NoBase passará então a sair a raiz do ficheiro XML
            ' caso não existe um extrutura xml no ficheiro XML será criado uma

            ' se o nobase não foi indicado um xmldoc é retornado para poder ser feita a gravação do xml
            ' caso contrário fica à responsabilidade de quem indicou o nobase a gravação do xml

            Dim NoPedido As System.Xml.XmlNode

            getXmlExcepção = Nothing

            If NoBase Is Nothing Then

                If Not System.IO.File.Exists(XmlFicheiro) Then
                    If XmlFicheiroCria Then
                        If g10phPDF4.Basframe.File.Escreve(XmlFicheiro, "<" & NoPedidoNome & ">" & vbCrLf & "</" & NoPedidoNome & ">", IO.FileMode.Open, True, getXmlExcepção) Then
                        Else
                            Return Nothing
                        End If
                    Else
                        getXmlExcepção = New System.IO.FileNotFoundException
                        Return Nothing
                    End If
                End If

                getXmlDoc = New System.Xml.XmlDocument

                Try
                    getXmlDoc.Load(XmlFicheiro)
                Catch exp As System.Exception
                    getXmlExcepção = exp
                    Return Nothing
                End Try

                NoBase = getXmlDoc.DocumentElement

                If NoBase Is Nothing Then
                    NoPedido = getXmlDoc.AppendChild(getXmlDoc.CreateElement(NoPedidoNome))
                    Return NoPedido
                Else
                    NoPedido = XMLGetNo(NoBase, NoPedidoNome)
                    If NoPedido Is Nothing Then NoPedido = NoBase.AppendChild(NoBase.OwnerDocument.CreateElement(NoPedidoNome))
                    Return NoPedido
                End If

            Else

                NoPedido = XMLGetNo(NoBase, NoPedidoNome)
                If NoPedido Is Nothing Then NoPedido = NoBase.AppendChild(NoBase.OwnerDocument.CreateElement(NoPedidoNome))
                Return NoPedido

            End If

        End Function

        Public Function XMLGetItem(ByVal NoBase As System.Xml.XmlNode, ByVal ItemPedidoNome As String, ByVal ItemPedidoInnerText As String) As System.Xml.XmlNode

            Dim NoPedido As System.Xml.XmlNode

            If NoBase.HasChildNodes Then

                For Each NoPedido In NoBase.ChildNodes

                    If NoPedido.Item(ItemPedidoNome).InnerText = ItemPedidoInnerText Then

                        Return NoPedido

                    End If

                Next

                Return Nothing

            Else

                Return Nothing

            End If

        End Function

    End Module

End Namespace
