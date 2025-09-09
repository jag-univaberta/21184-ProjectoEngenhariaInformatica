Namespace Dataframe

    Public Structure phAc��o

        Public Definida As Boolean
        Public Pedido As datPedido

        Public DbPreparada As Boolean
        Public DbSintaxe As String

        Public XmlPreparada As Boolean

        Public ResultadoEscalar As Object
        Public ResultadoAfectados As Integer

        Public Inscritos() As Integer           ' calculado: array de inteiros dos inscritos
        Public Filtros() As Integer             ' calculado: array de inteiros dos filtros
        Public Ordena��es() As Integer          ' calculado: array de inteiros da ordena��o
        Public Aceites() As Integer             ' calculado: array de inteiros dos aceites

        Private Shared ReadOnly _locker As Object()

        Public Sub AssimilaPedido()

            Dim m As Integer

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

            If Pedido.Ordena��es Is Nothing Then
                Ordena��es = Nothing
            Else
                ReDim Ordena��es(Pedido.Ordena��es.GetUpperBound(0))
                For m = 0 To Pedido.Ordena��es.GetUpperBound(0)
                    Ordena��es(m) = Pedido.Ordena��es(m).Membro.Ordem
                Next
            End If

            '                                                                               

        End Sub

        Public Function Clone() As phAc��o

            Dim NovaAc��o As phAc��o

            NovaAc��o.Definida = Me.Definida
            NovaAc��o.Pedido = Me.Pedido

            NovaAc��o.DbPreparada = Me.DbPreparada
            NovaAc��o.DbSintaxe = String.Copy(Me.DbSintaxe)

            NovaAc��o.XmlPreparada = Me.XmlPreparada

            NovaAc��o.ResultadoEscalar = Me.ResultadoEscalar
            NovaAc��o.ResultadoAfectados = Me.ResultadoAfectados

            If Not Me.Inscritos Is Nothing Then NovaAc��o.Inscritos = CType(Me.Inscritos.Clone, Integer())
            If Not Me.Filtros Is Nothing Then NovaAc��o.Filtros = CType(Me.Filtros.Clone, Integer())
            If Not Me.Ordena��es Is Nothing Then NovaAc��o.Ordena��es = CType(Me.Ordena��es.Clone, Integer())
            If Not Me.Aceites Is Nothing Then NovaAc��o.Aceites = CType(Me.Aceites.Clone, Integer())

            Return NovaAc��o

        End Function

    End Structure

    Public Class phDatlet

        Implements IDBLET
        Implements IXMLLET
        Implements IDATLET

        ' 01/10/2003    frank       cria��o
        ' 11/12/2003    frank       acr�scimo de mais uma ac��o "ac��o_select_primeiro" 
        ' 22/12/2003    frank       acr�scimo de mais uma ac��o "ac��o_select_pesquisa" 
        ' 22/12/2003    frank       o conteudo do parametro pode ser tirado de um array de atributos paralelo 
        '                           o objectivo � permitir indicar valores sem alterar os conteudos da classe 
        '                           cada posi��o deste array tem de conter o dado para o campo 
        '                               apontado pelo filtro para a mesma posi��o 
        '                           isto s� � valido para a��es de selec��o em que o filtro corrosponde 
        '                               aos campos condicionantes 
        '                           em ac��es de elimina��o, inser��o ou altera��o o filtro indica 
        '                               campos a trabalhar e nem sempre s�o iguais aos campos que 
        '                               condicionam o alcance da mesma ac��o 
        ' 29/12/2003    frank       cria��o de uma phAc��o que substituir� a SpecAc��oDB e a SpecAc��oXML
        ' 29/12/2003    frank       ac��o_parametros: esta propriedade for�a a parametriza��o de um comando 
        '                               a ser feita por estes conteudos e n�o pelo conteudo das propriedades da classe

        Protected l_info As DatInfo
        Protected l_atributos() As Object
        Protected l_separador As String

        Protected l_ac��es() As phAc��o
        Protected l_ac��o As phAc��o
        Protected l_filtros() As datInscri��o

        Protected b_conector As System.Data.IDbConnection
        Protected b_construtor As datConstrutorSQL
        Protected b_excep��o As System.Exception

        Protected x_ficheiro As String
        Protected x_nobase As System.Xml.XmlNode
        Protected x_excep��o As System.Exception
        Protected x_eliminado As Boolean

        Public Const ac��o_count As Integer = 12
        Public Const ac��o_especial As Integer = 0
        Public Const ac��o_select_ficha As Integer = 1
        Public Const ac��o_select_registo As Integer = 2
        Public Const ac��o_select_lista As Integer = 3
        Public Const ac��o_select_fichas As Integer = 4
        Public Const ac��o_select_procura As Integer = 5
        Public Const ac��o_select_proximo As Integer = 6
        Public Const ac��o_select_vista As Integer = 7
        Public Const ac��o_update_ficha As Integer = 8
        Public Const ac��o_insert_ficha As Integer = 9
        Public Const ac��o_delete_ficha As Integer = 10
        Public Const ac��o_select_primeiro As Integer = 11
        Public Const ac��o_select_pesquisa As Integer = 12

        Protected ac��o_canal As datNaturezas
        Protected ac��o_parametros() As Object

#Region "C�digo de suporte da pr�pria classe"

        ' cria��o:      frank   01/10/2003

        Public Erro As Boolean
        Public ErroAvisa As Boolean
        Public ErroResposta As MsgBoxResult



        Public Sub New()

            ' 01/10/2003    frank   Construtor de Servi�o
            '                       a ausencia de qualquer tipo de iniciailiza��o permite a cria��o 
            '                       de uma instancia da classe de forma silenciosa

        End Sub

        Public Sub New(ByVal Info As datInfo)

            ' 16/12/2003    frank   Construtor Parametrizado

            If Not Info Is Nothing Then Me.l_info = Info

            DatReset()

        End Sub

        Public Sub New(ByVal Conector As System.Data.IDbConnection, ByVal Construtor As datConstrutorSQL, ByVal Objecto As datObjecto)

            ' 01/10/2003    frank   Construtor Parametrizado
            '                       MUST IMPLEMENT 

            Me.l_config(Conector, Construtor, Objecto)

            DatReset()

        End Sub


        Protected Overridable Sub l_config(ByVal conector As System.Data.IDbConnection, ByVal construtor As datConstrutorSQL, ByVal objecto As datObjecto)

            ' cria��o:      frank   01/10/2003

            If Not conector Is Nothing Then Me.b_conector = conector
            If Not construtor Is Nothing Then Me.b_construtor = construtor
            If Not objecto Is Nothing Then Me.l_info.Objecto = objecto

        End Sub

        Protected Overridable Function l_objectoreset() As Boolean

            Dim c As Integer

            If l_info.Objecto Is Nothing Then

                Return False

            Else

                ReDim l_atributos(l_info.Objecto.Membros.GetUpperBound(0))

                For c = 0 To l_atributos.GetUpperBound(0)
                    DatAtributo(c) = Nothing
                Next

                Return True

            End If

        End Function

        Protected Overridable Sub l_clone(ByVal the_clone As phDatlet)

            ' cria��o:      frank   01/10/2003

            With the_clone

                ' ILET
                .l_info = Me.l_info
                If Not Me.l_atributos Is Nothing Then .l_atributos = CType(Me.l_atributos.Clone, Object())
                .l_separador = Me.l_separador

                If Not Me.l_ac��es Is Nothing Then .l_ac��es = CType(Me.l_ac��es.Clone, phAc��o())
                .l_ac��o = Me.l_ac��o.Clone

                .ac��o_canal = Me.ac��o_canal
                If Not Me.ac��o_parametros Is Nothing Then .ac��o_parametros = CType(Me.ac��o_parametros.Clone, Object())

                ' IDBLET
                .b_conector = Me.b_conector
                .b_construtor = Me.b_construtor
                .b_excep��o = Me.b_excep��o

                ' IXMLET
                .x_ficheiro = Me.x_ficheiro
                .x_nobase = Me.x_nobase
                .x_excep��o = Me.x_excep��o
                .x_eliminado = Me.x_eliminado


            End With

        End Sub

        Protected Overridable Sub l_defineac��o(ByVal ac��o As Integer)

            ' defini��o da ac��o 

            l_ac��es(ac��o).Pedido = New datPedido
            l_ac��es(ac��o).Pedido.Objecto = Me.l_info.Objecto

            Select Case ac��o

                Case ac��o_select_ficha
                    l_ac��es(ac��o).Pedido.Opera��o = datOpera��es.Consulta
                    l_ac��es(ac��o).Pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosTodos))
                    l_ac��es(ac��o).Pedido.MaisFiltros(datInscri��o.Novas(l_info.Objecto.MembrosChave))
                    l_ac��es(ac��o).Pedido.Ordena��es = Nothing

                Case ac��o_select_registo
                    l_ac��es(ac��o).Pedido.Opera��o = datOpera��es.Consulta
                    l_ac��es(ac��o).Pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosTodos))
                    l_ac��es(ac��o).Pedido.MaisFiltros(datInscri��o.Novas(l_info.Objecto.MembrosPonteiro))
                    l_ac��es(ac��o).Pedido.Ordena��es = Nothing

                Case ac��o_select_lista
                    l_ac��es(ac��o).Pedido.Opera��o = datOpera��es.Consulta
                    l_ac��es(ac��o).Pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosLista))
                    l_ac��es(ac��o).Pedido.Filtros = Nothing
                    l_ac��es(ac��o).Pedido.MaisOrdena��es(datInscri��o.Novas(l_info.Objecto.MembrosChave))

                Case ac��o_select_fichas
                    l_ac��es(ac��o).Pedido.Opera��o = datOpera��es.Consulta
                    l_ac��es(ac��o).Pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosTodos))
                    l_ac��es(ac��o).Pedido.Filtros = Nothing
                    l_ac��es(ac��o).Pedido.MaisOrdena��es(datInscri��o.Novas(l_info.Objecto.MembrosChave))

                Case ac��o_select_procura
                    l_ac��es(ac��o).Pedido.Opera��o = datOpera��es.Consulta
                    l_ac��es(ac��o).Pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosPonteiro))
                    l_ac��es(ac��o).Pedido.MaisFiltros(datInscri��o.Novas(l_info.Objecto.MembrosChave))
                    l_ac��es(ac��o).Pedido.Ordena��es = Nothing

                Case ac��o_select_vista
                    l_ac��es(ac��o).Pedido.Opera��o = datOpera��es.Consulta
                    l_ac��es(ac��o).Pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosVista))
                    l_ac��es(ac��o).Pedido.MaisFiltros(datInscri��o.Novas(l_info.Objecto.MembrosPonteiro))
                    l_ac��es(ac��o).Pedido.Ordena��es = Nothing

                Case ac��o_select_proximo
                    l_ac��es(ac��o).Pedido.Opera��o = datOpera��es.Consulta
                    l_ac��es(ac��o).Pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosPonteiro))
                    l_ac��es(ac��o).Pedido.Filtros = Nothing
                    l_ac��es(ac��o).Pedido.Ordena��es = Nothing

                Case ac��o_update_ficha
                    l_ac��es(ac��o).Pedido.Opera��o = datOpera��es.Altera��o
                    l_ac��es(ac��o).Pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosTodos))
                    l_ac��es(ac��o).Pedido.MaisFiltros(datInscri��o.Novas(l_info.Objecto.MembrosPonteiro))
                    l_ac��es(ac��o).Pedido.Ordena��es = Nothing

                Case ac��o_insert_ficha
                    l_ac��es(ac��o).Pedido.Opera��o = datOpera��es.Inser��o
                    l_ac��es(ac��o).Pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosTodos))
                    l_ac��es(ac��o).Pedido.MaisFiltros(datInscri��o.Novas(l_info.Objecto.MembrosTodos))
                    l_ac��es(ac��o).Pedido.Ordena��es = Nothing

                Case ac��o_delete_ficha
                    l_ac��es(ac��o).Pedido.Opera��o = datOpera��es.Elimina��o
                    l_ac��es(ac��o).Pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosTodos))
                    l_ac��es(ac��o).Pedido.MaisFiltros(datInscri��o.Novas(l_info.Objecto.MembrosPonteiro))
                    l_ac��es(ac��o).Pedido.Ordena��es = Nothing

                Case ac��o_select_primeiro
                    l_ac��es(ac��o).Pedido.Opera��o = datOpera��es.Consulta
                    l_ac��es(ac��o).Pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosTodos))
                    l_ac��es(ac��o).Pedido.Filtros = Nothing
                    l_ac��es(ac��o).Pedido.Ordena��es = Nothing

                Case ac��o_select_pesquisa
                    l_ac��es(ac��o).Pedido.Opera��o = datOpera��es.Consulta
                    l_ac��es(ac��o).Pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosPesquisa))
                    l_ac��es(ac��o).Pedido.MaisFiltros(datInscri��o.Novas(l_info.Objecto.MembrosDescri��o))
                    l_ac��es(ac��o).Pedido.MaisOrdena��es(datInscri��o.Novas(l_info.Objecto.MembrosChave))

            End Select

            If Not l_filtros Is Nothing Then l_ac��es(ac��o).Pedido.MaisFiltros(l_filtros)
            l_ac��es(ac��o).Pedido.AutoCompleta()

        End Sub


        Protected Overridable Function b_exec(ByVal comandoSQL As String) As Boolean

            ' cria��o:      frank   01/10/2003

            Dim Comando As System.Data.IDbCommand
            Dim Cursor As System.Data.IDataReader

            Comando = b_conector.CreateCommand
            Comando.CommandType = CommandType.Text
            Comando.CommandText = comandoSQL

            b_parametriza(Comando)

            Do

                ErroResposta = MsgBoxResult.Ok
                b_excep��o = Nothing
                Erro = False

                Try

                    l_ac��o.ResultadoAfectados = Comando.ExecuteNonQuery
                    Return True

                Catch execep��o_ao_comando As System.Exception

                    Erro = True
                    b_excep��o = execep��o_ao_comando
                    l_ac��o.ResultadoAfectados = 0

                    ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcep��o(b_excep��o, Comando.CommandText, ErroAvisa, True, False)

                End Try

            Loop While ErroResposta = MsgBoxResult.Retry

            Return False

        End Function

        Protected Overridable Function b_select(ByVal comandoSQL As String, ByVal Cursor As System.Data.IDataReader, ByVal Escalar As Boolean) As Boolean

            ' cria��o:      frank   01/10/2003

            Dim Comando As System.Data.IDbCommand

            Comando = b_conector.CreateCommand
            Comando.CommandType = CommandType.Text
            Comando.CommandText = comandoSQL

            b_parametriza(Comando)

            Do

                Erro = False
                ErroResposta = MsgBoxResult.Ok
                b_excep��o = Nothing

                Try

                    If Escalar Then
                        l_ac��o.ResultadoEscalar = Comando.ExecuteScalar
                    Else
                        Cursor = Comando.ExecuteReader
                    End If

                    If Cursor Is Nothing Then
                        Return False
                    Else
                        Return True
                    End If

                Catch execep��o_ao_comando As System.Exception

                    Erro = True
                    b_excep��o = execep��o_ao_comando

                    ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcep��o(b_excep��o, Comando.CommandText, ErroAvisa, True, False)

                End Try

            Loop While ErroResposta = MsgBoxResult.Retry

            Return False

        End Function

        Protected Overridable Sub b_parametriza(ByVal comando As System.Data.IDbCommand)

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   22/12/2003      o conteudo do parametro pode ser tirado de uma 
            '                                       array de atributos paralelo

            Dim parametros() As System.Data.IDataParameter
            Dim f As Integer
            Dim c As Integer

            comando.Parameters.Clear()

            If Not l_ac��o.Filtros Is Nothing Then

                ReDim parametros(l_ac��o.Filtros.GetUpperBound(0))

                For f = 0 To l_ac��o.Filtros.GetUpperBound(0)

                    c = l_ac��o.Filtros(f)

                    parametros(f) = comando.CreateParameter
                    parametros(f).ParameterName = b_construtor.SintaxeDeParametroValor(l_ac��o.Pedido.Objecto.Membros(c))
                    parametros(f).DbType = l_ac��o.Pedido.Objecto.Membros(c).Tipo

                    If ac��o_parametros Is Nothing Then
                        parametros(f).Value = DatAtributo(c)
                    Else
                        parametros(f).Value = ac��o_parametros(f)
                    End If

                    If parametros(f).Value Is Nothing Then parametros(f).Value = ""

                    comando.Parameters.Add(parametros(f))

                Next

            End If

            ac��o_parametros = Nothing

        End Sub


        Protected Overridable Sub x_defxmlno(ByVal no_registo As System.Xml.XmlNode)

            ' cria��o:      frank   01/10/2003

            Dim a As Integer
            Dim c As Integer

            For a = 0 To l_ac��o.Pedido.Aceites.GetUpperBound(0)

                c = l_ac��o.Inscritos(a)
                no_registo.AppendChild(no_registo.OwnerDocument.CreateElement(l_info.Objecto.Membros(c).Nome))

            Next

        End Sub

        Protected Overridable Function x_select(ByVal no_datlet As System.Xml.XmlNode, ByVal filtros() As Integer) As System.Xml.XmlNode

            Dim no_registo As System.Xml.XmlNode
            Dim f As Integer
            Dim a As Integer
            Dim coincide As Boolean

            ' m�todo parecido com x_selectvarios

            If no_datlet.HasChildNodes Then

                For Each no_registo In no_datlet.ChildNodes

                    coincide = False

                    If filtros Is Nothing Then
                        coincide = True
                    Else

                        For f = 0 To filtros.GetUpperBound(0)

                            a = filtros(f)

                            If ac��o_parametros Is Nothing Then

                                If no_registo.Item(l_info.Objecto.Membros(a).Nome).InnerText = CType(DatAtributo(a), String) Then
                                    coincide = coincide And True
                                End If

                            Else

                                If no_registo.Item(l_info.Objecto.Membros(a).Nome).InnerText = CType(ac��o_parametros(f), String) Then
                                    coincide = coincide And True
                                End If

                            End If

                        Next

                    End If

                    If coincide Then

                        ' a diferen�a com o m�todo x_select � aqui:
                        ' este m�todo quando encontra um n� retorna-o imediatamente sem continuara a procura

                        ac��o_parametros = Nothing
                        Return no_registo

                    End If

                Next

            End If

            Return Nothing

        End Function

        Protected Overridable Function x_selectvarios(ByVal no_datlet As System.Xml.XmlNode, ByVal filtros() As Integer) As System.Xml.XmlNode()

            Dim no_registo As System.Xml.XmlNode
            Dim no_lista() As System.Xml.XmlNode
            Dim f As Integer
            Dim a As Integer
            Dim t As Integer
            Dim coincide As Boolean

            ' m�todo parecido com x_select 

            If no_datlet.HasChildNodes Then

                For Each no_registo In no_datlet.ChildNodes

                    coincide = False

                    If filtros Is Nothing Then
                        coincide = True
                    Else

                        For f = 0 To filtros.GetUpperBound(0)

                            a = filtros(f)

                            If ac��o_parametros Is Nothing Then

                                If no_registo.Item(l_info.Objecto.Membros(a).Nome).InnerText = CType(DatAtributo(a), String) Then
                                    coincide = coincide And True
                                End If

                            Else

                                If no_registo.Item(l_info.Objecto.Membros(a).Nome).InnerText = CType(ac��o_parametros(f), String) Then
                                    coincide = coincide And True
                                End If

                            End If

                        Next

                    End If

                    If coincide Then

                        ' a diferen�a com o m�todo x_select � aqui:
                        ' este m�todo quando encontra um n� adiciona-o a uma lista que retornar�

                        ReDim Preserve no_lista(t)
                        no_lista(t) = no_registo
                        t = t + 1

                    End If

                Next

                ac��o_parametros = Nothing
                Return no_lista

            Else

                ac��o_parametros = Nothing
                Return Nothing

            End If

        End Function

        Protected Overrides Sub Finalize()

            Me.DatDispose()

            MyBase.Finalize()

        End Sub

#End Region

#Region "Implementa��o da interface ILET"

        Public Overridable Property DatInfo() As datInfo Implements ILET.DatInfo
            Get

                Return l_info

            End Get
            Set(ByVal Value As datInfo)

                l_info = Value

            End Set
        End Property

        Public Overridable Property DatAtributo(ByVal Index As Integer) As Object Implements ILET.DatAtributo

            ' cria��o:      frank   01/10/2003

            Get

                Return l_atributos(Index)

            End Get
            Set(ByVal Value As Object)

                l_atributos(Index) = Value

            End Set
        End Property

        Public Overridable Function DatClone() As ILET Implements ILET.DatClone


            ' 01/10/2003    frank   cria uma c�pia desta classe
            '                       MUST OVERRIDE

            Dim TheClone As New phDatlet    ' phDatlet = tipo

            l_clone(TheClone)

            Return TheClone

        End Function

        Public Overridable Function DatNew() As ILET Implements ILET.DatNew


            ' 01/10/2003    frank   cria uma nova instanica de uma classe deste tipo
            '                       MUST OVERRIDE

            Return New phDatlet ' phDatlet = tipo

        End Function

        Public Overridable Sub DatCopy(ByVal datalet As ILET) Implements ILET.DatCopy

            Dim a As Integer

            For a = 0 To l_atributos.GetUpperBound(0)

                Me.l_atributos(a) = datalet.DatAtributo(a)

            Next

        End Sub

        Public Overrides Function ToString() As String Implements ILET.ToString

            ' cria��o:      frank   12/11/2003

            Dim c As Integer
            Dim t As String

            For c = 0 To Me.l_info.Objecto.Membros.GetUpperBound(0)

                If Me.l_info.Objecto.Membros(c).Descritivo Then

                    If Not t Is Nothing AndAlso t.Length > 0 Then t = t & " - "
                    t = t & CType(Me.DatAtributo(c), String)

                End If

            Next

            Return t

        End Function

#End Region

#Region "Implementa��o da interface IDBLET"

        ' frank   01/10/2003:   cria��o

        Public Event DbJoin(ByVal Agregador As datMembro, ByVal Cursor As System.Data.IDataReader, ByVal Inscri��esAceites() As datAceita��o, ByRef JoinIndex As Integer) Implements IDBLET.DbJoin

        Public Overridable Sub DbReset() Implements IDBLET.DbReset

            ' cria��o:      frank   01/10/2003

            b_excep��o = Nothing

            l_ac��es = Nothing
            ReDim l_ac��es(ac��o_count)
            l_ac��o = Nothing

        End Sub

        Public Overridable Property DbConector() As System.Data.IDbConnection Implements IDBLET.DbConector

            ' cria��o:      frank   01/10/2003

            Get

                DbConector = b_conector

            End Get
            Set(ByVal Value As System.Data.IDbConnection)

                b_conector = Value

            End Set
        End Property

        Public Overridable Property DbConstrutor() As datConstrutorSQL Implements IDBLET.DbConstrutor

            ' cria��o:      frank   01/10/2003

            Get

                DbConstrutor = b_construtor

            End Get
            Set(ByVal Value As datConstrutorSQL)

                b_construtor = Value

            End Set
        End Property

        Public Overridable Property DbExcep��o() As System.Exception Implements IDBLET.DbExcep��o
            Get

                DbExcep��o = b_excep��o

            End Get
            Set(ByVal Value As System.Exception)

                b_excep��o = Value

            End Set
        End Property


        Public Overridable Function DbPrepara(ByVal Ac��o As Integer) As Boolean Implements IDBLET.DbPrepara

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   11/12/2003          adi��o de novo alcance "ac��o_select_primeiro"
            ' altera��o:    frank   22/12/2003          adi��o de novo alcance "ac��o_select_pesquisa"

            If Not l_ac��es(Ac��o).Definida Then

                ' defini��o da ac��o (objecto, inscritos, filtros, ordena��o)

                l_defineac��o(Ac��o)
                l_ac��es(Ac��o).Definida = True

            End If

            If Not l_ac��es(Ac��o).DbPreparada Then

                ' constru��o do sintaxe (comando sql) 
                ' e obten��o da lista de aceites (campos presentes no comando)

                l_ac��es(Ac��o).DbSintaxe = b_construtor.ConstroiSintaxe(l_ac��es(Ac��o).Pedido)
                l_ac��es(Ac��o).AssimilaPedido()
                l_ac��es(Ac��o).DbPreparada = True

            End If

            l_ac��o = l_ac��es(Ac��o)

            Return True

        End Function

        Public Overridable Function DbExecuta() As Boolean Implements IDBLET.DbExecuta

            ' cria��o:      frank   01/10/2003

            Return b_exec(l_ac��o.DbSintaxe)

        End Function

        Public Overridable Overloads Function DbConsulta() As Boolean Implements IDBLET.DbConsulta

            ' cria��o:      frank   01/10/2003

            Dim Cursor As System.Data.IDataReader

            DbConsulta = b_select(l_ac��o.DbSintaxe, Cursor, False)

            If DbConsulta Then

                If DbSaca(Cursor, True, l_ac��o.Pedido.Aceites, 0) Then l_ac��o.ResultadoAfectados = 1

                Cursor.Close()

            Else

                l_ac��o.ResultadoAfectados = 0

            End If

        End Function

        Public Overridable Function DbSelecciona() As IDBLET() Implements IDBLET.DbSelecciona

            ' cria��o:      frank   01/10/2003

            Dim cursor As System.Data.IDataReader
            Dim lista() As phDatlet
            Dim l As Integer
            Dim sucesso As Boolean

            l_ac��o.ResultadoAfectados = 0
            sucesso = b_select(l_ac��o.DbSintaxe, cursor, False)

            If sucesso Then

                l = -1
                While cursor.Read

                    l += 1
                    ReDim Preserve lista(l)

                    lista(l) = CType(Me.DatClone, phDatlet)
                    lista(l).l_ac��o.Pedido.Objecto = l_info.Objecto
                    lista(l).DbSaca(cursor, False, l_ac��o.Pedido.Aceites, 0)

                End While

                cursor.Close()

                l_ac��o.ResultadoAfectados = l + 1
                Return lista

            Else

                Return Nothing

            End If

        End Function

        Public Overridable ReadOnly Property DbAfectados() As Integer Implements IDBLET.DbAfectados

            ' cria��o:      frank   01/10/2003

            Get

                DbAfectados = l_ac��o.ResultadoAfectados

            End Get
        End Property

        Public Overridable Function DbSaca(ByVal Cursor As System.Data.IDataReader, ByVal CursorFetch As Boolean) As Boolean Implements IDBLET.DbSaca

            ' cria��o:      frank   01/10/2003

            Return DbSaca(Cursor, CursorFetch, l_ac��o.Pedido.Aceites, 0)

        End Function

        Public Overridable Function DbSaca(ByVal Cursor As System.Data.IDataReader, ByVal CursorFetch As Boolean, ByVal Inscri��esAceites() As datAceita��o, ByRef JoinIndex As Integer) As Boolean Implements IDBLET.DbSaca

            ' cria��o:      frank   01/10/2003

            Dim i As Integer
            Dim m As Integer
            Dim ji As Integer

            If CursorFetch Then
                DbSaca = Cursor.Read
            Else
                DbSaca = True
            End If

            If DbSaca Then

                If Inscri��esAceites Is Nothing Then

                    '                                                                                       

                    For m = 0 To Cursor.FieldCount - 1

                        If Cursor.IsDBNull(m) Then
                            DatAtributo(m) = Nothing
                        Else
                            Select Case l_ac��o.Pedido.Objecto.Membros(m).Tipo

                                Case DbType.Int32 : DatAtributo(m) = Cursor.GetInt32(m)
                                Case DbType.Decimal : DatAtributo(m) = Cursor.GetDecimal(m)
                                Case DbType.Date : DatAtributo(m) = Cursor.GetDateTime(m)
                                Case DbType.Time : DatAtributo(m) = Cursor.GetDateTime(m)
                                Case DbType.Boolean : DatAtributo(m) = Cursor.GetBoolean(m)
                                Case Else : DatAtributo(m) = Cursor.GetString(m)

                            End Select
                        End If

                    Next

                    '                                                                                       

                Else

                    '                                                                                       

                    ji = JoinIndex

                    For i = 0 To Inscri��esAceites.GetUpperBound(0)

                        If Inscri��esAceites(i).JoinNivel = ji Then

                            m = Inscri��esAceites(i).Membro.Ordem

                            If Cursor.IsDBNull(i) Then
                                DatAtributo(m) = Nothing
                            Else
                                Select Case l_ac��o.Pedido.Objecto.Membros(m).Tipo

                                    Case DbType.Int32 : DatAtributo(m) = Cursor.GetInt32(i)
                                    Case DbType.Decimal : DatAtributo(m) = Cursor.GetDecimal(i)
                                    Case DbType.Date : DatAtributo(m) = Cursor.GetDateTime(i)
                                    Case DbType.Time : DatAtributo(m) = Cursor.GetDateTime(i)
                                    Case DbType.Boolean : DatAtributo(m) = Cursor.GetBoolean(i)
                                    Case Else : DatAtributo(m) = Cursor.GetString(i)

                                End Select
                            End If

                            If Not l_ac��o.Pedido.Objecto.Membros(m).Referencia Is Nothing Then

                                JoinIndex += 1
                                RaiseEvent DbJoin(l_ac��o.Pedido.Objecto.Membros(m), Cursor, l_ac��o.Pedido.Aceites, JoinIndex)

                            End If
                        End If

                    Next

                    '                                                                                       

                End If

            End If

        End Function

        Public Overridable Function DbLink(ByVal Objecto As datObjecto) As Integer() Implements IDBLET.DbLink

            ' cria��o:      frank   01/10/2003

            Dim c As Integer
            Dim l As Integer
            Dim lista() As Integer

            l = -1

            For c = 0 To l_info.Objecto.Membros.GetUpperBound(0)

                If l_info.Objecto.Membros(c).ReferenciaObj.Nome = Objecto.Nome Then

                    l += 1
                    ReDim Preserve lista(l)
                    lista(l) = c

                End If

            Next

        End Function

        Public Overridable Sub DbDispose() Implements IDBLET.DbDispose

            ' cria��o:      frank   12/11/2003

            b_conector = Nothing
            b_construtor = Nothing

        End Sub

        Public Overridable Property DbAc��o() As datAc��o Implements IDBLET.DbAc��o
            Get

            End Get
            Set(ByVal Value As datAc��o)

            End Set
        End Property

        Public Overridable Overloads Function DbConsulta(ByVal Escalar As Boolean) As Boolean Implements IDBLET.DbConsulta

        End Function

        Public Overridable Property DbParametros() As Object() Implements IDBLET.DbParametros
            Get

            End Get
            Set(ByVal Value() As Object)

            End Set
        End Property


#End Region

#Region "Implementa��o da interface IXMLLET"

        Public Overridable Sub XmlReset() Implements IXMLLET.XmlReset

            ' cria��o:      frank   01/10/2003

            x_excep��o = Nothing
            x_eliminado = False
            l_ac��o = Nothing

        End Sub

        Public Overridable Property XmlFicheiro() As String Implements IXMLLET.XmlFicheiro

            ' cria��o:      frank   01/10/2003

            Get

                XmlFicheiro = x_ficheiro

            End Get
            Set(ByVal Value As String)

                x_ficheiro = Value

            End Set
        End Property

        Public Overridable Property XmlNoBase() As System.Xml.XmlNode Implements IXMLLET.XmlNoBase

            ' cria��o:      frank   01/10/2003

            Get

                XmlNoBase = Me.x_nobase

            End Get
            Set(ByVal Value As System.Xml.XmlNode)

                Me.x_nobase = Value

            End Set
        End Property

        Public Overridable Property XmlExcep��o() As System.Exception Implements IXMLLET.XmlExcep��o
            Get

                XmlExcep��o = x_excep��o

            End Get
            Set(ByVal Value As System.Exception)

                x_excep��o = Value

            End Set
        End Property

        Public Overridable Function XmlPrepara(ByVal Ac��o As Integer) As Boolean Implements IXMLLET.XmlPrepara

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   11/12/2003      as ac��es xml passam a ser filtradas, 
            '                                       os registos s�o procurados tendo em considera��o o conteudo
            '                                       de determinados atributos
            ' altera��o:    frank   11/12/2003      novo alcance "ac��o_select_primeiro"
            ' 18/12/2003    frank   ao contr�rio das ac��es DB as ac��es XML j� preparadas n�o s�o armazenadas
            '                       cada vez que se prepara uma ac��o XML todos os passos de prepara��o s�o efectuados

            If Not l_ac��es(Ac��o).Definida Then

                ' defini��o da ac��o (objecto, inscritros e filtros)

                l_defineac��o(Ac��o)
                l_ac��es(Ac��o).Definida = True

            End If

            If Not l_ac��es(Ac��o).XmlPreparada Then

                l_ac��es(Ac��o).AssimilaPedido()
                l_ac��es(Ac��o).XmlPreparada = True

            End If

            l_ac��o = l_ac��es(Ac��o)

            Return True

        End Function

        Public Overridable Function XmlConsulta() As Boolean Implements IXMLLET.XmlConsulta

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   11/12/2003          dos n�s existentes procura aquele cujo ponteiro seja igual
            '                                           ao conteudo do atributo ponteiro da classe
            '                                           se este valor for nulo "0" l� o primeiro n� dispon�vel
            ' altera��o:    frank   11/12/2003          dos n�s existentes procura aquele cujo conteudos dos Membros
            '                                           filtrados seja igual aos dos atributos da classe

            Dim no_datlet As System.Xml.XmlNode
            Dim no_registo As System.Xml.XmlNode

            Do

                Erro = False
                x_excep��o = Nothing
                no_datlet = XMLGetNo(x_nobase, x_ficheiro, l_info.Objecto.DbGrupo, False, Nothing, x_excep��o)

                If Not x_excep��o Is Nothing Then

                    Erro = True

                    ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcep��o(x_excep��o, x_ficheiro, ErroAvisa, True, False)

                End If

            Loop While ErroResposta = MsgBoxResult.Retry

            If no_datlet Is Nothing Then

                l_ac��o.ResultadoAfectados = 0
                Return False

            Else

                If no_datlet.FirstChild Is Nothing Then

                    l_ac��o.ResultadoAfectados = 0

                Else

                    If l_ac��o.Filtros Is Nothing Then
                        no_registo = no_datlet.FirstChild
                    Else
                        no_registo = x_select(no_datlet, l_ac��o.Filtros)
                    End If

                    If Not no_registo Is Nothing Then

                        XmlSaca(no_registo, l_ac��o.Inscritos)
                        l_ac��o.ResultadoAfectados = 1

                    Else

                        l_ac��o.ResultadoAfectados = 0

                    End If

                End If

                Return True

            End If

        End Function

        Public Overridable Function XmlSelecciona() As IXMLLET() Implements IXMLLET.XmlSelecciona

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   11/12/2003          dos n�s existentes procura aqueles cujo conteudos dos Membros
            '                                           filtrados seja igual aos dos atributos da classe

            Dim no_datlet As System.Xml.XmlNode
            Dim no_registos() As System.Xml.XmlNode
            Dim lista() As IXMLLET
            Dim l As Integer

            Do

                Erro = False
                x_excep��o = Nothing
                no_datlet = XMLGetNo(x_nobase, x_ficheiro, l_info.Objecto.DbGrupo, False, Nothing, x_excep��o)

                If Not x_excep��o Is Nothing Then

                    Erro = True

                    ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcep��o(x_excep��o, x_ficheiro, ErroAvisa, True, False)

                End If

            Loop While ErroResposta = MsgBoxResult.Retry

            If no_datlet Is Nothing Then

                l_ac��o.ResultadoAfectados = 0
                Return Nothing

            Else

                no_registos = x_selectvarios(no_datlet, l_ac��o.Filtros)

                ReDim lista(no_registos.GetUpperBound(0))

                For l = 0 To lista.GetUpperBound(0)

                    lista(l) = CType(Me.DatClone, phDatlet)
                    lista(l).XmlSaca(no_registos(l), l_ac��o.Inscritos)

                Next

                l_ac��o.ResultadoAfectados = l + 1
                Return lista

            End If

        End Function

        Public Overridable Overloads Function XmlEscreve() As Boolean Implements IXMLLET.XmlEscreve

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   11/12/2003      antes de gravar procura o n� corrospondente e modifica-o
            ' altera��o:    frank   11/12/2003      verifica se o n� foi marcado para elimina��o

            Dim xmldoc As System.Xml.XmlDocument
            Dim no_datlet As System.Xml.XmlNode
            Dim no_registo As System.Xml.XmlNode

            Dim n As Integer

            Do

                Erro = False
                x_excep��o = Nothing
                no_datlet = XMLGetNo(x_nobase, x_ficheiro, l_info.Objecto.DbGrupo, False, xmldoc, x_excep��o)

                If Not x_excep��o Is Nothing Then

                    Erro = True

                    ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcep��o(x_excep��o, x_ficheiro, ErroAvisa, True, False)

                End If

            Loop While ErroResposta = MsgBoxResult.Retry

            If no_datlet Is Nothing Then

                l_ac��o.ResultadoAfectados = 0
                Return False

            Else

                no_registo = x_select(no_datlet, l_info.Objecto.ArrayPonteiro)

                If no_registo Is Nothing And Not x_eliminado Then

                    no_registo = no_datlet.OwnerDocument.CreateElement(l_info.Objecto.Nome)
                    x_defxmlno(no_registo)
                    no_datlet.AppendChild(no_registo)
                    no_registo = no_datlet.FirstChild

                End If

                If x_eliminado Then
                    If Not no_registo Is Nothing Then no_datlet.RemoveChild(no_registo)
                Else
                    XmlEnche(no_registo, l_ac��o.Inscritos)
                End If

                If Not xmldoc Is Nothing Then xmldoc.Save(XmlFicheiro)

                l_ac��o.ResultadoAfectados = 1
                Return True

            End If

        End Function

        Public Overridable Overloads Function XmlEscreve(ByVal Lista() As IXMLLET) As Boolean Implements IXMLLET.XmlEscreve

            ' cria��o:      frank   01/10/2003
            ' altera��o.    frank   11/12/2003      uma a uma evoca o evento XMLEscreve de cada classe da lista

            Dim xmldoc As System.Xml.XmlDocument
            Dim no_datlet As System.Xml.XmlNode

            Dim l As Integer

            Do

                Erro = False
                x_excep��o = Nothing
                no_datlet = XMLGetNo(x_nobase, x_ficheiro, l_info.Objecto.DbGrupo, False, xmldoc, x_excep��o)

                If Not x_excep��o Is Nothing Then

                    Erro = True

                    ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcep��o(x_excep��o, x_ficheiro, ErroAvisa, True, False)

                End If

            Loop While ErroResposta = MsgBoxResult.Retry

            If no_datlet Is Nothing Then

                l_ac��o.ResultadoAfectados = 0
                Return False

            Else

                For l = 0 To Lista.GetUpperBound(0)

                    Lista(l).XmlNoBase = no_datlet
                    Lista(l).XmlEscreve()
                    l_ac��o.ResultadoAfectados = l + Lista(l).XmlAfectados

                Next

                If Not xmldoc Is Nothing Then xmldoc.Save(XmlFicheiro)

                Return True

            End If

        End Function

        Public Overridable Function XmlElimina() As Boolean Implements IXMLLET.XmlElimina

            ' cria��o:      frank   11/12/2003

            Dim xmldoc As System.Xml.XmlDocument
            Dim no_datlet As System.Xml.XmlNode
            Dim no_registo As System.Xml.XmlNode

            Dim n As Integer

            x_eliminado = True
            x_excep��o = Nothing

            If Not x_nobase Is Nothing Or x_ficheiro.Length > 0 Then

                Do

                    Erro = False
                    x_excep��o = Nothing
                    no_datlet = XMLGetNo(x_nobase, x_ficheiro, l_info.Objecto.DbGrupo, False, xmldoc, x_excep��o)

                    If Not x_excep��o Is Nothing Then

                        Erro = True

                        ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcep��o(x_excep��o, x_ficheiro, ErroAvisa, True, False)

                    End If

                Loop While ErroResposta = MsgBoxResult.Retry

                If no_datlet Is Nothing Then

                    l_ac��o.ResultadoAfectados = 0
                    Return False

                Else

                    no_registo = x_select(no_datlet, l_info.Objecto.ArrayPonteiro)

                    If no_registo Is Nothing Then

                        l_ac��o.ResultadoAfectados = 0

                    Else

                        no_datlet.RemoveChild(no_registo)

                        If Not xmldoc Is Nothing Then xmldoc.Save(XmlFicheiro)

                        l_ac��o.ResultadoAfectados = 1

                    End If

                    Return True

                End If

            Else

                Return True

            End If

        End Function

        Public Overridable ReadOnly Property XmlAfectados() As Integer Implements IXMLLET.XmlAfectados
            Get

                XmlAfectados = l_ac��o.ResultadoAfectados

            End Get
        End Property

        Public Overridable Function XmlSaca(ByVal No As System.Xml.XmlNode, ByVal Inscri��esAceites() As Integer) As Boolean Implements IXMLLET.XmlSaca

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   10/11/2003      passamos a aceder ao n� pelo seu nome
            '                                       a vantagem de aceder ao n� pelo seu nome � de que a ordem 
            '                                       destes no XML pode ser alterada sem prejuizo para as fun��es, 
            '                                       mais: sem uma ordem fixa os n�s podem at� n�o estar presentes no XML

            Dim a As Integer
            Dim c As Integer

            For a = 0 To Inscri��esAceites.GetUpperBound(0)

                c = Inscri��esAceites(a)
                'DatAtributo(c) = No.ChildNodes(a).InnerText
                DatAtributo(c) = No.Item(l_ac��o.Pedido.Objecto.Membros(c).Nome).InnerText

            Next

            x_eliminado = False

            Return True

        End Function

        Public Overridable Function XmlEnche(ByVal No As System.Xml.XmlNode, ByVal Inscri��esAceites() As Integer) As Boolean Implements IXMLLET.XmlEnche

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   10/11/2003      passamos a aceder ao n� pelo seu nome
            '                                       a vantagem de aceder ao n� pelo seu nome � de que a ordem 
            '                                       destes no XML pode ser alterada sem prejuizo para as fun��es, 
            '                                       mais, sem uma ordem fixa os n�s podem at� n�o estar presentes no XML

            Dim a As Integer
            Dim c As Integer

            For a = 0 To Inscri��esAceites.GetUpperBound(0)

                c = Inscri��esAceites(a)
                'No.ChildNodes(a).InnerText = CType(DatAtributo(c), String)
                No.Item(l_ac��o.Pedido.Objecto.Membros(c).Nome).InnerText = CType(DatAtributo(c), String)

            Next

        End Function

        Public Overridable Sub XmlDispose() Implements IXMLLET.XmlDispose

            ' cria��o:      frank   12/11/2003

            x_ficheiro = Nothing
            x_nobase = Nothing

        End Sub

        Public Overridable Property XmlAc��o() As datAc��o Implements IXMLLET.XmlAc��o
            Get

            End Get
            Set(ByVal Value As datAc��o)

            End Set
        End Property

        Public Overridable Property XmlParametros() As Object() Implements IXMLLET.XmlParametros
            Get

            End Get
            Set(ByVal Value() As Object)

            End Set
        End Property

#End Region

#Region "Implementa��o da interface IDATLET"

        ' cria��o:      frank   01/10/2003

        Public Overridable Property DatCanal() As datCanal Implements IDATLET.DatCanal
            Get
                ' n�o expoe esta propriedade porque a phDatlet orienta-se pela Natureza
            End Get
            Set(ByVal Value As datCanal)

            End Set
        End Property

        Public Overridable Property DatNatureza() As datNaturezas
            Get

                Return ac��o_canal

            End Get
            Set(ByVal Value As datNaturezas)

                ac��o_canal = Value

            End Set
        End Property

        Public Overridable Property DatAc��o() As datAc��o Implements IDATLET.DatAc��o
            Get
                ' a phDatlet usa um modelo de ac��o ligeiramente diferente por isso n�o pode expo-la
            End Get
            Set(ByVal Value As datAc��o)

            End Set
        End Property

        Public Overridable Property DatParametros() As Object() Implements IDATLET.DatParametros
            Get

                Return ac��o_parametros

            End Get
            Set(ByVal Value() As Object)

                ac��o_parametros = Value

            End Set
        End Property



        Public Overridable Sub DatPrepara(ByVal Alcance As Integer) Implements IDATLET.DatPrepara

            Select Case ac��o_canal

                Case datNaturezas.Db

                    DbPrepara(ac��o_select_primeiro)

                Case datNaturezas.Xml

                    XmlPrepara(ac��o_select_primeiro)

            End Select

        End Sub

        Public Overridable Function DatConsultaPrimeiro() As Boolean Implements IDATLET.DatConsultaPrimeiro

            ' cria��o:      frank   01/10/2003

            Select Case ac��o_canal

                Case datNaturezas.Db

                    DatPrepara(ac��o_select_primeiro)
                    If DbConsulta() Then
                        Return (l_ac��o.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

                Case datNaturezas.Xml

                    DatPrepara(ac��o_select_primeiro)
                    If XmlConsulta() Then
                        Return (l_ac��o.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

            End Select

        End Function

        Public Overridable Overloads Function DatConsultaFicha() As Boolean Implements IDATLET.DatConsultaFicha

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case ac��o_canal

                Case datNaturezas.Db

                    DatPrepara(ac��o_select_ficha)
                    If DbConsulta() Then
                        Return (l_ac��o.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

                Case datNaturezas.Xml

                    DatPrepara(ac��o_select_ficha)
                    If XmlConsulta() Then
                        Return (l_ac��o.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

            End Select

        End Function

        Public Overridable Overloads Function DatConsultaRegisto() As Boolean Implements IDATLET.DatConsultaRegisto

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case ac��o_canal

                Case datNaturezas.Db

                    DatPrepara(ac��o_select_registo)
                    If DbConsulta() Then
                        Return (l_ac��o.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

                Case datNaturezas.Xml

                    DatPrepara(ac��o_select_registo)
                    If XmlConsulta() Then
                        Return (l_ac��o.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

            End Select

        End Function

        Public Overridable Overloads Function DatExiste() As Boolean Implements IDATLET.DatExiste

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case ac��o_canal

                Case datNaturezas.Db

                    DatPrepara(ac��o_select_procura)
                    b_select(l_ac��o.DbSintaxe, Nothing, True)
                    DatAtributo(l_ac��o.Pedido.Objecto.Ponteiro) = l_ac��o.ResultadoEscalar
                    Return (Not l_ac��o.ResultadoEscalar Is Nothing)

                Case datNaturezas.Xml

                    DatPrepara(ac��o_select_procura)
                    If XmlConsulta() Then
                        Return (l_ac��o.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

            End Select

        End Function

        Public Overridable Overloads Function DatSeleccionaLista() As IDATLET() Implements IDATLET.DatSeleccionaLista

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case ac��o_canal

                Case datNaturezas.Db

                    DatPrepara(ac��o_select_lista)
                    Return CType(DbSelecciona(), IDATLET())

                Case datNaturezas.Xml

                    DatPrepara(ac��o_select_lista)
                    Return CType(XmlSelecciona(), IDATLET())

            End Select

        End Function

        Public Overridable Overloads Function DatConsultaVista() As Boolean Implements IDATLET.DatConsultaVista

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case ac��o_canal

                Case datNaturezas.Db

                    DatPrepara(ac��o_select_vista)
                    If DbConsulta() Then
                        Return (l_ac��o.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

                Case datNaturezas.Xml

                    DatPrepara(ac��o_select_vista)
                    If XmlConsulta() Then
                        Return (l_ac��o.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

            End Select

        End Function

        Public Overridable Overloads Function DatPesquisa() As IDATLET() Implements IDATLET.DatPesquisa

            Select Case ac��o_canal

                Case datNaturezas.Db

                    DatPrepara(ac��o_select_pesquisa)
                    Return CType(DbSelecciona(), IDATLET())

                Case datNaturezas.Xml

                    DatPrepara(ac��o_select_pesquisa)
                    Return CType(XmlSelecciona(), IDATLET())

            End Select

        End Function

        Public Overridable Function DatSeleccionaFichas() As IDATLET() Implements IDATLET.DatSeleccionaFichas

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case ac��o_canal

                Case datNaturezas.Db

                    DatPrepara(ac��o_select_fichas)
                    Return CType(DbSelecciona(), IDATLET())

                Case datNaturezas.Xml

                    DatPrepara(ac��o_select_fichas)
                    Return CType(XmlSelecciona(), IDATLET())

            End Select

        End Function

        Public Overridable Function DatAutoId() As Boolean Implements IDATLET.DatAutoId

            ' 01/10/2003    frank       cria��o
            ' 11/12/2003    frank       de canal unico (DB) para multicanal (DB, XML)

            Select Case ac��o_canal

                Case datNaturezas.Db

                    DatPrepara(ac��o_select_proximo)
                    b_select(l_ac��o.DbSintaxe, Nothing, True)
                    If l_ac��o.ResultadoEscalar Is Nothing Then
                        If b_excep��o Is Nothing Then
                            DatAtributo(l_ac��o.Pedido.Objecto.Ponteiro) = 10
                            DatAutoId = True
                        Else
                            DatAutoId = False
                        End If
                    Else
                        DatAtributo(l_ac��o.Pedido.Objecto.Ponteiro) = CInt(l_ac��o.ResultadoEscalar) + 10
                        DatAutoId = True
                    End If

                Case datNaturezas.Xml

                    Dim Lista() As IXMLLET

                    DatPrepara(ac��o_select_proximo)
                    Lista = XmlSelecciona()
                    If Lista Is Nothing Then
                        If x_excep��o Is Nothing Then
                            DatAtributo(l_ac��o.Pedido.Objecto.Ponteiro) = 10
                            DatAutoId = True
                        Else
                            DatAutoId = False
                        End If
                    Else
                        DatAtributo(l_ac��o.Pedido.Objecto.Ponteiro) = CInt(Lista(Lista.GetUpperBound(0)).DatAtributo(l_info.Objecto.Ponteiro)) + 10
                        DatAutoId = False
                    End If

            End Select

        End Function

        Public Overridable Function DatAltera() As Boolean Implements IDATLET.DatAltera

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case ac��o_canal

                Case datNaturezas.Db

                    DatPrepara(ac��o_update_ficha)
                    If DbExecuta() Then
                        Return (l_ac��o.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

                Case datNaturezas.Xml

                    DatPrepara(ac��o_update_ficha)
                    If XmlEscreve() Then
                        Return (l_ac��o.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

            End Select

        End Function

        Public Overridable Function DatInsere() As Boolean Implements IDATLET.DatInsere

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case ac��o_canal

                Case datNaturezas.Db

                    DatPrepara(ac��o_insert_ficha)
                    If DbExecuta() Then
                        Return (l_ac��o.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

                Case datNaturezas.Xml

                    DatPrepara(ac��o_insert_ficha)
                    If XmlEscreve() Then
                        Return (l_ac��o.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

            End Select

        End Function

        Public Overridable Function DatElimina() As Boolean Implements IDATLET.DatElimina

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case ac��o_canal

                Case datNaturezas.Db

                    DatPrepara(ac��o_delete_ficha)
                    If DbExecuta() Then
                        Return (l_ac��o.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

                Case datNaturezas.Xml

                    DatPrepara(ac��o_delete_ficha)
                    If XmlElimina() Then
                        Return (l_ac��o.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

            End Select

        End Function

        Public Overridable Overloads Function DatConsultaFicha(ByVal ParamArray Chave() As Object) As Boolean Implements IDATLET.DatConsultaFicha

            ' cria��o:      frank   01/10/2003

            ac��o_parametros = Chave

            Return DatConsultaFicha()

        End Function

        Public Overridable Overloads Function DatConsultaRegisto(ByVal Ponteiro As Integer) As Boolean Implements IDATLET.DatConsultaRegisto

            ' cria��o:      frank   01/10/2003

            ReDim ac��o_parametros(0)
            ac��o_parametros(0) = Ponteiro

            Return DatConsultaRegisto()

        End Function

        Public Overridable Overloads Function DatExiste(ByVal ParamArray Chave() As Object) As Boolean Implements IDATLET.DatExiste

            ' cria��o:      frank   01/10/2003

            ac��o_parametros = Chave

            Return DatExiste()

        End Function

        Public Overridable Overloads Function DatConsultaVista(ByVal Ponteiro As Integer) As Boolean Implements IDATLET.DatConsultaVista

            ' cria��o:      frank   01/10/2003

            ReDim ac��o_parametros(0)
            ac��o_parametros(0) = Ponteiro

            Return DatConsultaVista()

        End Function

        Public Overridable Overloads Function DatPesquisa(ByVal ParamArray Descri��o() As Object) As IDATLET() Implements IDATLET.DatPesquisa

            ' 22/12/2003    frank       cria��o:

            ac��o_parametros = Descri��o

            Return DatPesquisa

        End Function

        Public Overridable Function DatActualiza() As Boolean Implements IDATLET.DatActualiza

            If CInt(DatAtributo(Me.l_info.Objecto.Ponteiro)) = 0 Then

                ' trata-se de um registo novo sem um ponteiro identificador

                If Not Me.DatAutoId Then

                    ' n�o foi poss�vel a autonumera��o
                    Return False

                End If

            End If

            If DatExiste() Then

                ' o registo j� existe
                Return DatAltera()

            Else

                ' o registo n�o existe
                Return DatInsere()

            End If

        End Function

        Public Overridable Property DatFiltros() As datInscri��o() Implements IDATLET.DatFiltros
            Get

                Return l_filtros

            End Get
            Set(ByVal Value() As datInscri��o)

                l_filtros = Value
                ReDim l_ac��es(ac��o_count)

            End Set
        End Property


        Public Overridable Property DatExcep��o() As System.Exception Implements IDATLET.DatExcep��o
            Get

                Select Case ac��o_canal

                    Case datNaturezas.Db : Return b_excep��o
                    Case datNaturezas.Xml : Return x_excep��o

                End Select

            End Get
            Set(ByVal Value As System.Exception)

                Select Case ac��o_canal

                    Case datNaturezas.Db : b_excep��o = Value
                    Case datNaturezas.Xml : x_excep��o = Value

                End Select

            End Set
        End Property

        Public Overridable Function DatCast(ByVal Lista() As ILET) As ILET() Implements IDATLET.DatCast

            ' 01/10/2003    frank   efectua a convers�o de uma lista de IDATLET para o tipo especifico da classe
            '                       MUST OVERRIDE

            Dim r() As IDATLET  ' IDATLET = tipo
            Dim a As Integer

            ReDim r(Lista.GetUpperBound(0))

            For a = 0 To Lista.GetUpperBound(0)

                r(a) = DirectCast(Lista(a), IDATLET) ' IDATLET = tipo

            Next

            Return r

        End Function

        Public Overridable Sub DatReset() Implements IDATLET.DatReset

            ' cria��o:      frank   01/10/2003

            DbReset()
            XmlReset()

            ' reinicializa��o da DATLET

            Me.DatInfo = l_info

            l_separador = " - "

        End Sub

        Public Overridable Sub DatDispose() Implements IDATLET.DatDispose

            ' cria��o:      frank   12/11/2003

            XmlDispose()
            DbDispose()

        End Sub

#End Region

        Shared Function CastTabela(ByVal Lista() As IDATLET, ByVal MinimoUmaLinha As Boolean, ByVal ParamArray Inscritos() As Integer) As System.Data.DataTable

            ' cria��o:      frank   12/11/2003

            Dim dt As New System.Data.DataTable
            Dim l As Integer
            Dim i As Integer
            Dim linha() As Object

            ' resumo:
            ' transforma uma lista de datlets numa tabela System.Data.DataTable compativel com ADO.NET
            ' caso a lista seja nula mas seja pedido pelo menos uma linha, uma instancia � adicionada � lista
            '   desta forma na tabela ser� colocada pelo menos uma linha

            If Lista Is Nothing And MinimoUmaLinha Then
                ReDim Lista(0)
                Lista(0) = New phDatlet
            End If

            If Not Lista Is Nothing Then

                ReDim linha(Inscritos.GetUpperBound(0))

                For i = 0 To Inscritos.GetUpperBound(0)
                    dt.Columns.Add(Lista(0).DatInfo.Objecto.Membros(Inscritos(i)).Titulo)
                Next

                For l = 0 To Lista.GetUpperBound(0)

                    For i = 0 To Inscritos.GetUpperBound(0)

                        linha(i) = Lista(0).DatAtributo(Inscritos(i))

                    Next

                    dt.Rows.Add(linha)

                Next

                Return dt

            End If

        End Function

        Public Overloads Function DatVerifica() As Boolean Implements IDATLET.DatVerifica

        End Function

        Public Overloads Function DatVerifica(ByVal Ponteiro As Object) As Boolean Implements IDATLET.DatVerifica

        End Function

    End Class

    '                                                                                                       

    Public Class SpecLet
        Implements ILET

        Protected l_info As datInfo
        Protected l_atributos() As Object



        Protected Overridable Function l_objectoreset() As Boolean

            Dim c As Integer

            If l_info.Objecto Is Nothing Then

                Return False

            Else

                ReDim l_atributos(l_info.Objecto.Membros.GetUpperBound(0))

                For c = 0 To l_atributos.GetUpperBound(0)
                    DatAtributo(c) = Nothing
                Next

                Return True

            End If

        End Function

        Public Overridable Property DatAtributo(ByVal Index As Integer) As Object Implements ILET.DatAtributo

            ' cria��o:      frank   01/10/2003

            Get

                Return l_atributos(Index)

            End Get
            Set(ByVal Value As Object)

                l_atributos(Index) = Value

            End Set
        End Property

        Public Property DatInfo() As datInfo Implements ILET.DatInfo
            Get

                Return l_info

            End Get
            Set(ByVal Value As datInfo)

                l_info = Value

            End Set
        End Property

        Public Overridable Function DatClone() As ILET Implements ILET.DatClone

            ' 01/10/2003    frank       cria uma c�pia desta classe
            '                           MUST OVERRIDE

            Dim TheClone As New SpecLet ' tipo derivado em vez de "SpecLet"

            TheClone.l_info = Me.l_info
            If Not Me.l_atributos Is Nothing Then TheClone.l_atributos = CType(Me.l_atributos.Clone, Object())

            Return TheClone

        End Function

        Public Overridable Function DatNew() As ILET Implements ILET.DatNew

            ' 01/10/2003    frank       cria uma nova instancia de uma classe deste tipo
            '                           MUST OVERRIDE

            Return New SpecLet ' tipo derivado em vez de "SpecLet"

        End Function

        Public Overridable Function DatCast(ByVal Lista() As ILET) As ILET() Implements ILET.DatCast

            ' 01/10/2003    frank   efectua a convers�o de uma lista de IDATLET para o tipo especifico da classe
            '                       MUST OVERRIDE

            Dim r() As IDATLET  ' IDATLET = tipo
            Dim a As Integer

            ReDim r(Lista.GetUpperBound(0))

            For a = 0 To Lista.GetUpperBound(0)

                r(a) = DirectCast(Lista(a), IDATLET) ' IDATLET = tipo

            Next

            Return r

        End Function

        Public Overridable Sub DatCopy(ByVal datalet As ILET) Implements ILET.DatCopy

            Dim a As Integer

            For a = 0 To l_atributos.GetUpperBound(0)

                Me.l_atributos(a) = datalet.DatAtributo(a)

            Next

        End Sub

        Public Overrides Function ToString() As String Implements ILET.ToString

            ' cria��o:      frank   12/11/2003

            Dim c As Integer
            Dim t As String

            For c = 0 To Me.l_info.Objecto.Membros.GetUpperBound(0)

                If Me.l_info.Objecto.Membros(c).Descritivo Then

                    If Not t Is Nothing AndAlso t.Length > 0 Then t = t & " - "
                    t = t & CType(Me.DatAtributo(c), String)

                End If

            Next

            Return t

        End Function

    End Class

    Public Class SpecDbLet
        Inherits SpecLet
        Implements IDBLET

        ' 01/10/2003    frank       cria��o

        Protected b_excep��o As System.Exception
        Protected b_conector As System.Data.IDbConnection
        Protected b_construtor As datConstrutorSQL
        Protected b_parametros() As Object

        Protected b_ac��es() As datAc��o
        Protected b_ac��o As datAc��o

        Public Erro As Boolean
        Public ErroAvisa As Boolean
        Public ErroResposta As MsgBoxResult




        Protected Overridable Sub b_defineac��o(ByVal ac��o As Integer)

            ' defini��o da ac��o 

            b_ac��es(ac��o).Pedido = New datPedido
            b_ac��es(ac��o).Pedido.Objecto = Me.l_info.Objecto

            Select Case ac��o

                Case datAlcances.select_ficha
                    b_ac��es(ac��o).Pedido.Opera��o = datOpera��es.Consulta
                    b_ac��es(ac��o).Pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosTodos))
                    b_ac��es(ac��o).Pedido.MaisFiltros(datInscri��o.Novas(l_info.Objecto.MembrosChave))
                    b_ac��es(ac��o).Pedido.Ordena��es = Nothing

                Case datAlcances.select_registo
                    b_ac��es(ac��o).Pedido.Opera��o = datOpera��es.Consulta
                    b_ac��es(ac��o).Pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosTodos))
                    b_ac��es(ac��o).Pedido.MaisFiltros(datInscri��o.Novas(l_info.Objecto.MembrosPonteiro))
                    b_ac��es(ac��o).Pedido.Ordena��es = Nothing

                Case datAlcances.select_lista
                    b_ac��es(ac��o).Pedido.Opera��o = datOpera��es.Consulta
                    b_ac��es(ac��o).Pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosLista))
                    b_ac��es(ac��o).Pedido.Filtros = Nothing
                    b_ac��es(ac��o).Pedido.MaisOrdena��es(datInscri��o.Novas(l_info.Objecto.MembrosChave))

                Case datAlcances.select_fichas
                    b_ac��es(ac��o).Pedido.Opera��o = datOpera��es.Consulta
                    b_ac��es(ac��o).Pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosTodos))
                    b_ac��es(ac��o).Pedido.Filtros = Nothing
                    b_ac��es(ac��o).Pedido.MaisOrdena��es(datInscri��o.Novas(l_info.Objecto.MembrosChave))

                Case datAlcances.select_procura
                    b_ac��es(ac��o).Pedido.Opera��o = datOpera��es.Consulta
                    b_ac��es(ac��o).Pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosPonteiro))
                    b_ac��es(ac��o).Pedido.MaisFiltros(datInscri��o.Novas(l_info.Objecto.MembrosChave))
                    b_ac��es(ac��o).Pedido.Ordena��es = Nothing

                Case datAlcances.select_vista
                    b_ac��es(ac��o).Pedido.Opera��o = datOpera��es.Consulta
                    b_ac��es(ac��o).Pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosVista))
                    b_ac��es(ac��o).Pedido.MaisFiltros(datInscri��o.Novas(l_info.Objecto.MembrosPonteiro))
                    b_ac��es(ac��o).Pedido.Ordena��es = Nothing

                Case datAlcances.select_maximo
                    b_ac��es(ac��o).Pedido.Opera��o = datOpera��es.Consulta
                    b_ac��es(ac��o).Pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosPonteiro))
                    b_ac��es(ac��o).Pedido.Filtros = Nothing
                    b_ac��es(ac��o).Pedido.Ordena��es = Nothing

                Case datAlcances.update_ficha
                    b_ac��es(ac��o).Pedido.Opera��o = datOpera��es.Altera��o
                    b_ac��es(ac��o).Pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosTodos))
                    b_ac��es(ac��o).Pedido.MaisFiltros(datInscri��o.Novas(l_info.Objecto.MembrosPonteiro))
                    b_ac��es(ac��o).Pedido.Ordena��es = Nothing

                Case datAlcances.insert_ficha
                    b_ac��es(ac��o).Pedido.Opera��o = datOpera��es.Inser��o
                    b_ac��es(ac��o).Pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosTodos))
                    b_ac��es(ac��o).Pedido.MaisFiltros(datInscri��o.Novas(l_info.Objecto.MembrosTodos))
                    b_ac��es(ac��o).Pedido.Ordena��es = Nothing

                Case datAlcances.delete_ficha
                    b_ac��es(ac��o).Pedido.Opera��o = datOpera��es.Elimina��o
                    b_ac��es(ac��o).Pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosTodos))
                    b_ac��es(ac��o).Pedido.MaisFiltros(datInscri��o.Novas(l_info.Objecto.MembrosPonteiro))
                    b_ac��es(ac��o).Pedido.Ordena��es = Nothing

                Case datAlcances.select_primeiro
                    b_ac��es(ac��o).Pedido.Opera��o = datOpera��es.Consulta
                    b_ac��es(ac��o).Pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosTodos))
                    b_ac��es(ac��o).Pedido.Filtros = Nothing
                    b_ac��es(ac��o).Pedido.Ordena��es = Nothing

                Case datAlcances.select_pesquisa
                    b_ac��es(ac��o).Pedido.Opera��o = datOpera��es.Consulta
                    b_ac��es(ac��o).Pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosPesquisa))
                    b_ac��es(ac��o).Pedido.MaisFiltros(datInscri��o.Novas(l_info.Objecto.MembrosDescri��o))
                    b_ac��es(ac��o).Pedido.MaisOrdena��es(datInscri��o.Novas(l_info.Objecto.MembrosChave))

            End Select

            b_ac��es(ac��o).Pedido.AutoCompleta()

        End Sub

        Protected Overridable Function b_exec(ByVal comandoSQL As String) As Boolean

            ' cria��o:      frank   01/10/2003

            Dim Comando As System.Data.IDbCommand
            Dim Cursor As System.Data.IDataReader

            Comando = b_conector.CreateCommand
            Comando.CommandType = CommandType.Text
            Comando.CommandText = comandoSQL

            b_parametriza(Comando)

            Do

                ErroResposta = MsgBoxResult.Ok
                b_excep��o = Nothing
                Erro = False

                Try

                    b_ac��o.ResultadoAfectados = Comando.ExecuteNonQuery
                    Return True

                Catch execep��o_ao_comando As System.Exception

                    Erro = True
                    b_excep��o = execep��o_ao_comando
                    b_ac��o.ResultadoAfectados = 0

                    ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcep��o(b_excep��o, Comando.CommandText, ErroAvisa, True, False)

                End Try

            Loop While ErroResposta = MsgBoxResult.Retry

            Return False

        End Function

        Protected Overridable Function b_select(ByVal comandoSQL As String, ByVal Cursor As System.Data.IDataReader, ByVal Escalar As Boolean) As Boolean

            ' cria��o:      frank   01/10/2003

            Dim Comando As System.Data.IDbCommand

            Comando = b_conector.CreateCommand
            Comando.CommandType = CommandType.Text
            Comando.CommandText = comandoSQL

            b_parametriza(Comando)

            Do

                Erro = False
                ErroResposta = MsgBoxResult.Ok
                b_excep��o = Nothing

                Try

                    If Escalar Then
                        b_ac��o.ResultadoEscalar = Comando.ExecuteScalar
                    Else
                        Cursor = Comando.ExecuteReader
                    End If

                    If Cursor Is Nothing Then
                        Return False
                    Else
                        Return True
                    End If

                Catch execep��o_ao_comando As System.Exception

                    Erro = True
                    b_excep��o = execep��o_ao_comando

                    ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcep��o(b_excep��o, Comando.CommandText, ErroAvisa, True, False)

                End Try

            Loop While ErroResposta = MsgBoxResult.Retry

            Return False

        End Function

        Protected Overridable Sub b_parametriza(ByVal comando As System.Data.IDbCommand)

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   22/12/2003      o conteudo do parametro pode ser tirado de uma 
            '                                       array de atributos paralelo

            Dim parametros() As System.Data.IDataParameter
            Dim f As Integer
            Dim c As Integer

            comando.Parameters.Clear()

            If Not b_ac��o.Filtros Is Nothing Then

                ReDim parametros(b_ac��o.Filtros.GetUpperBound(0))

                For f = 0 To b_ac��o.Filtros.GetUpperBound(0)

                    c = b_ac��o.Filtros(f)

                    parametros(f) = comando.CreateParameter
                    parametros(f).ParameterName = b_construtor.SintaxeDeParametroValor(b_ac��o.Pedido.Objecto.Membros(c))
                    parametros(f).DbType = b_ac��o.Pedido.Objecto.Membros(c).Tipo

                    If b_parametros Is Nothing Then
                        parametros(f).Value = DatAtributo(c)
                    Else
                        parametros(f).Value = b_parametros(f)
                    End If

                    If parametros(f).Value Is Nothing Then parametros(f).Value = ""

                    comando.Parameters.Add(parametros(f))

                Next

            End If

            b_parametros = Nothing

        End Sub


        Public Event DbJoin(ByVal Agregador As datMembro, ByVal Cursor As System.Data.IDataReader, ByVal Inscri��esAceites() As datAceita��o, ByRef JoinIndex As Integer) Implements IDBLET.DbJoin


        Public Overridable Sub DbReset() Implements IDBLET.DbReset

            ' cria��o:      frank       01/10/2003

            l_objectoreset()

            ReDim b_ac��es(datAlcances.count - 1)

        End Sub

        Public Overridable Property DbConector() As System.Data.IDbConnection Implements IDBLET.DbConector

            ' cria��o:      frank   01/10/2003

            Get

                DbConector = b_conector

            End Get
            Set(ByVal Value As System.Data.IDbConnection)

                b_conector = Value

            End Set
        End Property

        Public Overridable Property DbConstrutor() As datConstrutorSQL Implements IDBLET.DbConstrutor

            ' cria��o:      frank   01/10/2003

            Get

                DbConstrutor = b_construtor

            End Get
            Set(ByVal Value As datConstrutorSQL)

                b_construtor = Value

            End Set
        End Property

        Public Overridable Property DbExcep��o() As System.Exception Implements IDBLET.DbExcep��o
            Get

                DbExcep��o = b_excep��o

            End Get
            Set(ByVal Value As System.Exception)

                b_excep��o = Value

            End Set
        End Property


        Public Overridable Function DbPrepara(ByVal Ac��o As Integer) As Boolean Implements IDBLET.DbPrepara

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   11/12/2003          adi��o de novo alcance "datAlcances.Select_primeiro"
            ' altera��o:    frank   22/12/2003          adi��o de novo alcance "datAlcances.Select_pesquisa"

            If Not b_ac��es(Ac��o).Definida Then

                ' defini��o da ac��o (objecto, inscritos, filtros, ordena��o)

                b_defineac��o(Ac��o)
                b_ac��es(Ac��o).Definida = True

            End If

            If Not b_ac��es(Ac��o).Preparada Then

                ' constru��o do sintaxe (comando sql) 
                ' e obten��o da lista de aceites (campos presentes no comando)

                b_ac��es(Ac��o).Sintaxe = b_construtor.ConstroiSintaxe(b_ac��es(Ac��o).Pedido)
                b_ac��es(Ac��o).AssimilaPedido(b_ac��es(Ac��o).Pedido)
                b_ac��es(Ac��o).Preparada = True

            End If

            b_ac��o = b_ac��es(Ac��o)

            Return True

        End Function

        Public Overridable Function DbExecuta() As Boolean Implements IDBLET.DbExecuta

            ' cria��o:      frank   01/10/2003

            Return b_exec(b_ac��o.Sintaxe)

        End Function

        Public Overridable Overloads Function DbConsulta() As Boolean Implements IDBLET.DbConsulta

            Return DbConsulta(False)

        End Function

        Public Overridable Overloads Function DbConsulta(ByVal Escalar As Boolean) As Boolean Implements IDBLET.DbConsulta

            ' cria��o:      frank   01/10/2003

            Dim Cursor As System.Data.IDataReader

            DbConsulta = b_select(b_ac��o.Sintaxe, Cursor, Escalar)

            If DbConsulta Then

                If DbSaca(Cursor, True, b_ac��o.Pedido.Aceites, 0) Then b_ac��o.ResultadoAfectados = 1

                Cursor.Close()

            Else

                b_ac��o.ResultadoAfectados = 0

            End If

        End Function

        Public Overridable Function DbSelecciona() As IDBLET() Implements IDBLET.DbSelecciona

            ' cria��o:      frank   01/10/2003

            Dim cursor As System.Data.IDataReader
            Dim lista() As SpecDbLet
            Dim l As Integer
            Dim sucesso As Boolean

            b_ac��o.ResultadoAfectados = 0
            sucesso = b_select(b_ac��o.Sintaxe, cursor, False)

            If sucesso Then

                l = -1
                While cursor.Read

                    l += 1
                    ReDim Preserve lista(l)

                    lista(l) = CType(Me.DatClone, SpecDbLet)
                    lista(l).b_ac��o.Pedido.Objecto = l_info.Objecto
                    lista(l).DbSaca(cursor, False, b_ac��o.Pedido.Aceites, 0)

                End While

                cursor.Close()

                b_ac��o.ResultadoAfectados = l + 1
                Return lista

            Else

                Return Nothing

            End If

        End Function

        Public Overridable ReadOnly Property DbAfectados() As Integer Implements IDBLET.DbAfectados

            ' cria��o:      frank   01/10/2003

            Get

                DbAfectados = b_ac��o.ResultadoAfectados

            End Get
        End Property

        Public Overridable Function DbSaca(ByVal Cursor As System.Data.IDataReader, ByVal CursorFetch As Boolean) As Boolean Implements IDBLET.DbSaca

            ' cria��o:      frank   01/10/2003

            Return DbSaca(Cursor, CursorFetch, b_ac��o.Pedido.Aceites, 0)

        End Function

        Public Overridable Function DbSaca(ByVal Cursor As System.Data.IDataReader, ByVal CursorFetch As Boolean, ByVal Inscri��esAceites() As datAceita��o, ByRef JoinIndex As Integer) As Boolean Implements IDBLET.DbSaca

            ' cria��o:      frank   01/10/2003

            Dim i As Integer
            Dim m As Integer
            Dim ji As Integer

            If CursorFetch Then
                DbSaca = Cursor.Read
            Else
                DbSaca = True
            End If

            If DbSaca Then

                If Inscri��esAceites Is Nothing Then

                    '                                                                                       

                    For m = 0 To Cursor.FieldCount - 1

                        If Cursor.IsDBNull(m) Then
                            MyBase.DatAtributo(m) = Nothing
                        Else
                            Select Case b_ac��o.Pedido.Objecto.Membros(m).Tipo

                                Case DbType.Int32 : DatAtributo(m) = Cursor.GetInt32(m)
                                Case DbType.Decimal : DatAtributo(m) = Cursor.GetDecimal(m)
                                Case DbType.Date : DatAtributo(m) = Cursor.GetDateTime(m)
                                Case DbType.Time : DatAtributo(m) = Cursor.GetDateTime(m)
                                Case DbType.Boolean : DatAtributo(m) = Cursor.GetBoolean(m)
                                Case Else : DatAtributo(m) = Cursor.GetString(m)

                            End Select
                        End If

                    Next

                    '                                                                                       

                Else

                    '                                                                                       

                    ji = JoinIndex

                    For i = 0 To Inscri��esAceites.GetUpperBound(0)

                        If Inscri��esAceites(i).JoinNivel = ji Then

                            m = Inscri��esAceites(i).Membro.Ordem

                            If Cursor.IsDBNull(i) Then
                                DatAtributo(m) = Nothing
                            Else
                                Select Case b_ac��o.Pedido.Objecto.Membros(m).Tipo

                                    Case DbType.Int32 : DatAtributo(m) = Cursor.GetInt32(i)
                                    Case DbType.Decimal : DatAtributo(m) = Cursor.GetDecimal(i)
                                    Case DbType.Date : DatAtributo(m) = Cursor.GetDateTime(i)
                                    Case DbType.Time : DatAtributo(m) = Cursor.GetDateTime(i)
                                    Case DbType.Boolean : DatAtributo(m) = Cursor.GetBoolean(i)
                                    Case Else : DatAtributo(m) = Cursor.GetString(i)

                                End Select
                            End If

                            If Not b_ac��o.Pedido.Objecto.Membros(m).Referencia Is Nothing Then

                                JoinIndex += 1
                                RaiseEvent DbJoin(b_ac��o.Pedido.Objecto.Membros(m), Cursor, b_ac��o.Pedido.Aceites, JoinIndex)

                            End If
                        End If

                    Next

                    '                                                                                       

                End If

            End If

        End Function

        Public Overridable Function DbLink(ByVal Objecto As datObjecto) As Integer() Implements IDBLET.DbLink

            ' cria��o:      frank   01/10/2003

            Dim c As Integer
            Dim l As Integer
            Dim lista() As Integer

            l = -1

            For c = 0 To l_info.Objecto.Membros.GetUpperBound(0)

                If l_info.Objecto.Membros(c).ReferenciaObj.Nome = Objecto.Nome Then

                    l += 1
                    ReDim Preserve lista(l)
                    lista(l) = c

                End If

            Next

        End Function

        Public Overridable Sub DbDispose() Implements IDBLET.DbDispose

            ' cria��o:      frank       12/11/2003

            l_info = Nothing
            l_atributos = Nothing

            b_excep��o = Nothing
            b_conector = Nothing
            b_construtor = Nothing

            b_ac��o = Nothing
            b_ac��es = Nothing

        End Sub

        Public Overridable Property DbParametros() As Object() Implements IDBLET.DbParametros
            Get

                Return b_parametros

            End Get
            Set(ByVal Value() As Object)

                b_parametros = Value

            End Set
        End Property

        Public Property DbAc��o() As datAc��o Implements IDBLET.DbAc��o
            Get

                Return b_ac��o

            End Get
            Set(ByVal Value As datAc��o)

                b_ac��o = Value

            End Set
        End Property

    End Class

    Public Class SpecXmlLet
        Inherits SpecLet
        Implements IXMLLET

        ' todo: SpecXmlLet: vai usar o AdaptadorXML para aceder aos ficheiros XML)

        Protected x_ficheiro As String
        Protected x_nobase As System.Xml.XmlNode
        Protected x_excep��o As System.Exception
        Protected x_eliminado As Boolean
        Protected x_parametros() As Object

        Protected x_ac��es() As datAc��o
        Protected x_ac��o As datAc��o

        Public Erro As Boolean
        Public ErroAvisa As Boolean
        Public ErroResposta As MsgBoxResult




        Protected Overridable Sub x_defxmlno(ByVal no_registo As System.Xml.XmlNode)

            ' cria��o:      frank   01/10/2003

            Dim a As Integer
            Dim c As Integer

            For a = 0 To x_ac��o.Pedido.Aceites.GetUpperBound(0)

                c = x_ac��o.Inscritos(a)
                no_registo.AppendChild(no_registo.OwnerDocument.CreateElement(l_info.Objecto.Membros(c).Nome))

            Next

        End Sub

        Protected Overridable Function x_select(ByVal no_datlet As System.Xml.XmlNode, ByVal filtros() As Integer) As System.Xml.XmlNode

            Dim no_registo As System.Xml.XmlNode
            Dim f As Integer
            Dim a As Integer
            Dim coincide As Boolean

            ' m�todo parecido com x_selectvarios

            If x_parametros Is Nothing Then x_parametros = l_atributos

            If no_datlet.HasChildNodes Then

                For Each no_registo In no_datlet.ChildNodes

                    coincide = False

                    If filtros Is Nothing Then
                        coincide = True
                    Else

                        For f = 0 To filtros.GetUpperBound(0)

                            a = filtros(f)

                            'If no_registo.Item(l_objecto.Membros(a).Nome).InnerText = CType(datAtributo(a), String) Then
                            '    coincide = coincide And True
                            'End If

                            If no_registo.Item(l_info.Objecto.Membros(a).Nome).InnerText = CType(x_parametros(f), String) Then
                                coincide = coincide And True
                            End If

                        Next

                    End If

                    If coincide Then

                        ' a diferen�a com o m�todo x_select � aqui:
                        ' este m�todo quando encontra um n� retorna-o imediatamente sem continuara a procura

                        x_parametros = Nothing
                        Return no_registo

                    End If

                Next

            End If

            x_parametros = Nothing
            Return Nothing

        End Function

        Protected Overridable Function x_selectvarios(ByVal no_datlet As System.Xml.XmlNode, ByVal filtros() As Integer) As System.Xml.XmlNode()

            Dim no_registo As System.Xml.XmlNode
            Dim no_lista() As System.Xml.XmlNode
            Dim f As Integer
            Dim a As Integer
            Dim t As Integer
            Dim coincide As Boolean

            ' m�todo parecido com x_select 

            If x_parametros Is Nothing Then x_parametros = l_atributos

            If no_datlet.HasChildNodes Then

                For Each no_registo In no_datlet.ChildNodes

                    coincide = False

                    If filtros Is Nothing Then
                        coincide = True
                    Else

                        For f = 0 To filtros.GetUpperBound(0)

                            a = filtros(f)

                            'If no_registo.Item(l_objecto.Membros(a).Nome).InnerText = CType(datAtributo(a), String) Then
                            '    coincide = coincide And True
                            'End If

                            If no_registo.Item(l_info.Objecto.Membros(a).Nome).InnerText = CType(x_parametros(f), String) Then
                                coincide = coincide And True
                            End If

                        Next

                    End If

                    If coincide Then

                        ' a diferen�a com o m�todo x_select � aqui:
                        ' este m�todo quando encontra um n� adiciona-o a uma lista que retornar�

                        ReDim Preserve no_lista(t)
                        no_lista(t) = no_registo
                        t = t + 1

                    End If

                Next

                x_parametros = Nothing
                Return no_lista

            Else

                x_parametros = Nothing
                Return Nothing

            End If

        End Function


        Public Overridable Sub XmlReset() Implements IXMLLET.XmlReset

            ' cria��o:      frank   01/10/2003

            l_objectoreset()

            ReDim x_ac��es(datAlcances.count - 1)

        End Sub

        Public Overridable Property XmlFicheiro() As String Implements IXMLLET.XmlFicheiro

            ' cria��o:      frank   01/10/2003

            Get

                Return x_ficheiro

            End Get
            Set(ByVal Value As String)

                x_ficheiro = Value

            End Set
        End Property

        Public Overridable Property XmlNoBase() As System.Xml.XmlNode Implements IXMLLET.XmlNoBase

            ' cria��o:      frank   01/10/2003

            Get

                Return Me.x_nobase

            End Get
            Set(ByVal Value As System.Xml.XmlNode)

                Me.x_nobase = Value

            End Set
        End Property

        Public Overridable Property XmlExcep��o() As System.Exception Implements IXMLLET.XmlExcep��o
            Get

                Return x_excep��o

            End Get
            Set(ByVal Value As System.Exception)

                x_excep��o = Value

            End Set
        End Property

        Public Overridable Function XmlPrepara(ByVal Ac��o As Integer) As Boolean Implements IXMLLET.XmlPrepara

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   11/12/2003      as ac��es xml passam a ser filtradas, 
            '                                       os registos s�o procurados tendo em considera��o o conteudo
            '                                       de determinados atributos
            ' altera��o:    frank   11/12/2003      novo alcance "datAlcances.Select_primeiro"
            ' 18/12/2003    frank   ao contr�rio das ac��es DB as ac��es XML j� preparadas n�o s�o armazenadas
            '                       cada vez que se prepara uma ac��o XML todos os passos de prepara��o s�o efectuados

            If Not x_ac��es(Ac��o).Definida Then

                x_ac��es(Ac��o).Definida = True

            End If

            If Not x_ac��es(Ac��o).Preparada Then

                x_ac��es(Ac��o).AssimilaPedido(x_ac��es(Ac��o).Pedido)
                x_ac��es(Ac��o).Preparada = True

            End If

            x_ac��o = x_ac��es(Ac��o)

            Return True

        End Function

        Public Overridable Function XmlConsulta() As Boolean Implements IXMLLET.XmlConsulta

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   11/12/2003          dos n�s existentes procura aquele cujo ponteiro seja igual
            '                                           ao conteudo do atributo ponteiro da classe
            '                                           se este valor for nulo "0" l� o primeiro n� dispon�vel
            ' altera��o:    frank   11/12/2003          dos n�s existentes procura aquele cujo conteudos dos Membros
            '                                           filtrados seja igual aos dos atributos da classe

            Dim no_datlet As System.Xml.XmlNode
            Dim no_registo As System.Xml.XmlNode

            Do

                Erro = False
                x_excep��o = Nothing
                no_datlet = XMLGetNo(x_nobase, x_ficheiro, l_info.Objecto.DbGrupo, False, Nothing, x_excep��o)

                If Not x_excep��o Is Nothing Then

                    Erro = True

                    ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcep��o(x_excep��o, x_ficheiro, ErroAvisa, True, False)

                End If

            Loop While ErroResposta = MsgBoxResult.Retry

            If no_datlet Is Nothing Then

                x_ac��o.ResultadoAfectados = 0
                Return False

            Else

                If no_datlet.FirstChild Is Nothing Then

                    x_ac��o.ResultadoAfectados = 0

                Else

                    If x_ac��o.Filtros Is Nothing Then
                        no_registo = no_datlet.FirstChild
                    Else
                        no_registo = x_select(no_datlet, x_ac��o.Filtros)
                    End If

                    If Not no_registo Is Nothing Then

                        XmlSaca(no_registo, x_ac��o.Inscritos)
                        x_ac��o.ResultadoAfectados = 1

                    Else

                        x_ac��o.ResultadoAfectados = 0

                    End If

                End If

                Return True

            End If

        End Function

        Public Overridable Function XmlSelecciona() As IXMLLET() Implements IXMLLET.XmlSelecciona

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   11/12/2003          dos n�s existentes procura aqueles cujo conteudos dos Membros
            '                                           filtrados seja igual aos dos atributos da classe

            Dim no_datlet As System.Xml.XmlNode
            Dim no_registos() As System.Xml.XmlNode
            Dim lista() As SpecXmlLet
            Dim l As Integer

            Do

                Erro = False
                x_excep��o = Nothing
                no_datlet = XMLGetNo(x_nobase, x_ficheiro, l_info.Objecto.DbGrupo, False, Nothing, x_excep��o)

                If Not x_excep��o Is Nothing Then

                    Erro = True

                    ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcep��o(x_excep��o, x_ficheiro, ErroAvisa, True, False)

                End If

            Loop While ErroResposta = MsgBoxResult.Retry

            If no_datlet Is Nothing Then

                x_ac��o.ResultadoAfectados = 0
                Return Nothing

            Else

                no_registos = x_selectvarios(no_datlet, x_ac��o.Filtros)

                ReDim lista(no_registos.GetUpperBound(0))

                For l = 0 To lista.GetUpperBound(0)

                    lista(l) = CType(Me.DatClone, SpecXmlLet)
                    lista(l).XmlSaca(no_registos(l), x_ac��o.Inscritos)

                Next

                x_ac��o.ResultadoAfectados = l + 1
                Return lista

            End If

        End Function

        Public Overridable Overloads Function XmlEscreve() As Boolean Implements IXMLLET.XmlEscreve

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   11/12/2003      antes de gravar procura o n� corrospondente e modifica-o
            ' altera��o:    frank   11/12/2003      verifica se o n� foi marcado para elimina��o

            Dim xmldoc As System.Xml.XmlDocument
            Dim no_datlet As System.Xml.XmlNode
            Dim no_registo As System.Xml.XmlNode

            Dim n As Integer

            Do

                Erro = False
                x_excep��o = Nothing
                no_datlet = XMLGetNo(x_nobase, x_ficheiro, l_info.Objecto.DbGrupo, False, xmldoc, x_excep��o)

                If Not x_excep��o Is Nothing Then

                    Erro = True

                    ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcep��o(x_excep��o, x_ficheiro, ErroAvisa, True, False)

                End If

            Loop While ErroResposta = MsgBoxResult.Retry

            If no_datlet Is Nothing Then

                x_ac��o.ResultadoAfectados = 0
                Return False

            Else

                no_registo = x_select(no_datlet, l_info.Objecto.ArrayPonteiro)

                If no_registo Is Nothing And Not x_eliminado Then

                    no_registo = no_datlet.OwnerDocument.CreateElement(l_info.Objecto.Nome)
                    x_defxmlno(no_registo)
                    no_datlet.AppendChild(no_registo)
                    no_registo = no_datlet.FirstChild

                End If

                If x_eliminado Then
                    If Not no_registo Is Nothing Then no_datlet.RemoveChild(no_registo)
                Else
                    XmlEnche(no_registo, x_ac��o.Inscritos)
                End If

                If Not xmldoc Is Nothing Then xmldoc.Save(XmlFicheiro)

                x_ac��o.ResultadoAfectados = 1
                Return True

            End If

        End Function

        Public Overridable Overloads Function XmlEscreve(ByVal Lista() As IXMLLET) As Boolean Implements IXMLLET.XmlEscreve

            ' cria��o:      frank   01/10/2003
            ' altera��o.    frank   11/12/2003      uma a uma evoca o evento XMLEscreve de cada classe da lista

            Dim xmldoc As System.Xml.XmlDocument
            Dim no_datlet As System.Xml.XmlNode

            Dim l As Integer

            Do

                Erro = False
                x_excep��o = Nothing
                no_datlet = XMLGetNo(x_nobase, x_ficheiro, l_info.Objecto.DbGrupo, False, xmldoc, x_excep��o)

                If Not x_excep��o Is Nothing Then

                    Erro = True

                    ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcep��o(x_excep��o, x_ficheiro, ErroAvisa, True, False)

                End If

            Loop While ErroResposta = MsgBoxResult.Retry

            If no_datlet Is Nothing Then

                x_ac��o.ResultadoAfectados = 0
                Return False

            Else

                For l = 0 To Lista.GetUpperBound(0)

                    Lista(l).XmlNoBase = no_datlet
                    Lista(l).XmlEscreve()
                    x_ac��o.ResultadoAfectados = l + Lista(l).XmlAfectados

                Next

                If Not xmldoc Is Nothing Then xmldoc.Save(XmlFicheiro)

                Return True

            End If

        End Function

        Public Overridable Function XmlElimina() As Boolean Implements IXMLLET.XmlElimina

            ' cria��o:      frank   11/12/2003

            Dim xmldoc As System.Xml.XmlDocument
            Dim no_datlet As System.Xml.XmlNode
            Dim no_registo As System.Xml.XmlNode

            Dim n As Integer

            x_eliminado = True
            x_excep��o = Nothing

            If Not x_nobase Is Nothing Or x_ficheiro.Length > 0 Then

                Do

                    Erro = False
                    x_excep��o = Nothing
                    no_datlet = XMLGetNo(x_nobase, x_ficheiro, l_info.Objecto.DbGrupo, False, xmldoc, x_excep��o)

                    If Not x_excep��o Is Nothing Then

                        Erro = True

                        ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcep��o(x_excep��o, x_ficheiro, ErroAvisa, True, False)

                    End If

                Loop While ErroResposta = MsgBoxResult.Retry

                If no_datlet Is Nothing Then

                    x_ac��o.ResultadoAfectados = 0
                    Return False

                Else

                    no_registo = x_select(no_datlet, l_info.Objecto.ArrayPonteiro)

                    If no_registo Is Nothing Then

                        x_ac��o.ResultadoAfectados = 0

                    Else

                        no_datlet.RemoveChild(no_registo)

                        If Not xmldoc Is Nothing Then xmldoc.Save(XmlFicheiro)

                        x_ac��o.ResultadoAfectados = 1

                    End If

                    Return True

                End If

            Else

                Return True

            End If

        End Function

        Public Overridable ReadOnly Property XmlAfectados() As Integer Implements IXMLLET.XmlAfectados
            Get

                Return x_ac��o.ResultadoAfectados

            End Get
        End Property

        Public Overridable Function XmlSaca(ByVal No As System.Xml.XmlNode, ByVal Inscri��esAceites() As Integer) As Boolean Implements IXMLLET.XMLSaca

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   10/11/2003      passamos a aceder ao n� pelo seu nome
            '                                       a vantagem de aceder ao n� pelo seu nome � de que a ordem 
            '                                       destes no XML pode ser alterada sem prejuizo para as fun��es, 
            '                                       mais: sem uma ordem fixa os n�s podem at� n�o estar presentes no XML

            Dim a As Integer
            Dim c As Integer

            For a = 0 To Inscri��esAceites.GetUpperBound(0)

                c = Inscri��esAceites(a)
                'DatAtributo(c) = No.ChildNodes(a).InnerText
                DatAtributo(c) = No.Item(x_ac��o.Pedido.Objecto.Membros(c).Nome).InnerText

            Next

            x_eliminado = False

            Return True

        End Function

        Public Overridable Function XmlEnche(ByVal No As System.Xml.XmlNode, ByVal Inscri��esAceites() As Integer) As Boolean Implements IXMLLET.XMLEnche

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   10/11/2003      passamos a aceder ao n� pelo seu nome
            '                                       a vantagem de aceder ao n� pelo seu nome � de que a ordem 
            '                                       destes no XML pode ser alterada sem prejuizo para as fun��es, 
            '                                       mais, sem uma ordem fixa os n�s podem at� n�o estar presentes no XML

            Dim a As Integer
            Dim c As Integer

            For a = 0 To Inscri��esAceites.GetUpperBound(0)

                c = Inscri��esAceites(a)
                'No.ChildNodes(a).InnerText = CType(DatAtributo(c), String)
                No.Item(x_ac��o.Pedido.Objecto.Membros(c).Nome).InnerText = CType(DatAtributo(c), String)

            Next

        End Function

        Public Overridable Sub XmlDispose() Implements IXMLLET.XmlDispose

            ' cria��o:      frank   12/11/2003

            l_info = Nothing
            l_atributos = Nothing

            x_excep��o = Nothing
            x_eliminado = False
            x_ac��es = Nothing
            x_ac��o = Nothing
            x_ficheiro = Nothing
            x_nobase = Nothing

        End Sub

        Public Overridable Property XmlParametros() As Object() Implements IXMLLET.XmlParametros
            Get

                Return x_parametros

            End Get
            Set(ByVal Value() As Object)

                x_parametros = Value

            End Set
        End Property

        Public Property XmlAc��o() As datAc��o Implements IXMLLET.XmlAc��o
            Get

                Return x_ac��o

            End Get
            Set(ByVal Value As datAc��o)

                x_ac��o = Value

            End Set
        End Property

    End Class

    Public Class SpecDatlet
        Inherits SpecLet
        Implements IDATLET

        Protected d_ac��o_canal As datNaturezas

        Protected d_canalDB As IDBLET
        Protected d_canalXML As IXMLLET



        Public Overridable Property DatCanal() As DatCanal Implements IDATLET.DatCanal
            Get
                ' n�o expoe esta propriedade porque a SpecDatlet orienta-se pela Natureza
            End Get
            Set(ByVal Value As DatCanal)

            End Set
        End Property

        Public Overridable Property DatNatureza() As datNaturezas
            Get

                Return d_ac��o_canal

            End Get
            Set(ByVal Value As datNaturezas)

                d_ac��o_canal = Value

            End Set
        End Property

        Public Overridable Property DatParametros() As Object() Implements IDATLET.DatParametros
            Get

                Select Case d_ac��o_canal
                    Case datNaturezas.Db : Return d_canalDB.DbParametros
                    Case datNaturezas.Xml : Return d_canalXML.XmlParametros
                End Select

            End Get
            Set(ByVal Value() As Object)

                Select Case d_ac��o_canal
                    Case datNaturezas.Db : d_canalDB.DbParametros = Value
                    Case datNaturezas.Xml : d_canalXML.XmlParametros = Value
                End Select

            End Set
        End Property

        Public Overridable Property DatAc��o() As DatAc��o Implements IDATLET.DatAc��o
            Get

                Select Case d_ac��o_canal
                    Case datNaturezas.Db : Return d_canalDB.DbAc��o
                    Case datNaturezas.Xml : Return d_canalXML.XmlAc��o
                End Select

            End Get
            Set(ByVal Value As datAc��o)

                Select Case d_ac��o_canal
                    Case datNaturezas.Db : d_canalDB.DbAc��o = Value
                    Case datNaturezas.Xml : d_canalXML.XmlAc��o = Value
                End Select

            End Set
        End Property


        Public Overridable Sub DatPrepara(ByVal Alcance As Integer) Implements IDATLET.DatPrepara

            Select Case d_ac��o_canal

                Case datNaturezas.Db
                    d_canalDB.DbPrepara(datAlcances.select_primeiro)

                Case datNaturezas.Xml
                    d_canalXML.XmlPrepara(datAlcances.select_primeiro)

            End Select

        End Sub

        Public Overridable Function DatConsultaPrimeiro() As Boolean Implements IDATLET.DatConsultaPrimeiro

            ' cria��o:      frank   01/10/2003

            Select Case d_ac��o_canal

                Case datNaturezas.Db

                    DatPrepara(datAlcances.select_primeiro)
                    If d_canalDB.DbConsulta() Then
                        Return (d_canalDB.DbAfectados > 0)
                    Else
                        Return False
                    End If

                Case datNaturezas.Xml

                    DatPrepara(datAlcances.select_primeiro)
                    If d_canalXML.XmlConsulta() Then
                        Return (d_canalXML.XmlAfectados > 0)
                    Else
                        Return False
                    End If

            End Select

        End Function

        Public Overridable Overloads Function DatConsultaFicha() As Boolean Implements IDATLET.DatConsultaFicha

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case d_ac��o_canal

                Case datNaturezas.Db

                    DatPrepara(datAlcances.select_ficha)
                    If d_canalDB.DbConsulta() Then
                        Return (d_canalDB.DbAfectados > 0)
                    Else
                        Return False
                    End If

                Case datNaturezas.Xml

                    DatPrepara(datAlcances.select_ficha)
                    If d_canalXML.XmlConsulta() Then
                        Return (d_canalXML.XmlAfectados > 0)
                    Else
                        Return False
                    End If

            End Select

        End Function

        Public Overridable Overloads Function DatConsultaRegisto() As Boolean Implements IDATLET.DatConsultaRegisto

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case d_ac��o_canal

                Case datNaturezas.Db

                    DatPrepara(datAlcances.select_registo)
                    If d_canalDB.DbConsulta() Then
                        Return (d_canalDB.DbAfectados > 0)
                    Else
                        Return False
                    End If

                Case datNaturezas.Xml

                    DatPrepara(datAlcances.select_registo)
                    If d_canalXML.XmlConsulta() Then
                        Return (d_canalXML.XmlAfectados > 0)
                    Else
                        Return False
                    End If

            End Select

        End Function

        Public Overridable Overloads Function DatExiste() As Boolean Implements IDATLET.DatExiste

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case d_ac��o_canal

                Case datNaturezas.Db

                    DatPrepara(datAlcances.select_procura)
                    d_canalDB.DbConsulta(True)
                    DatAtributo(d_canalDB.DbAc��o.Pedido.Objecto.Ponteiro) = d_canalDB.DbAc��o.ResultadoEscalar
                    Return (Not d_canalDB.DbAc��o.ResultadoEscalar Is Nothing)

                Case datNaturezas.Xml

                    DatPrepara(datAlcances.select_procura)
                    If d_canalXML.XmlConsulta() Then
                        Return (d_canalXML.XmlAc��o.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

            End Select

        End Function

        Public Overridable Overloads Function DatSeleccionaLista() As IDATLET() Implements IDATLET.DatSeleccionaLista

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case d_ac��o_canal

                Case datNaturezas.Db

                    DatPrepara(datAlcances.select_lista)
                    Return CType(d_canalDB.DbSelecciona(), IDATLET())

                Case datNaturezas.Xml

                    DatPrepara(datAlcances.select_lista)
                    Return CType(d_canalXML.XmlSelecciona(), IDATLET())

            End Select

        End Function

        Public Overridable Overloads Function DatConsultaVista() As Boolean Implements IDATLET.DatConsultaVista

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case d_ac��o_canal

                Case datNaturezas.Db

                    DatPrepara(datAlcances.select_vista)
                    If d_canalDB.DbConsulta() Then
                        Return (d_canalDB.DbAfectados > 0)
                    Else
                        Return False
                    End If

                Case datNaturezas.Xml

                    DatPrepara(datAlcances.select_vista)
                    If d_canalXML.XmlConsulta() Then
                        Return (d_canalXML.XmlAfectados > 0)
                    Else
                        Return False
                    End If

            End Select

        End Function

        Public Overridable Overloads Function DatPesquisa() As IDATLET() Implements IDATLET.DatPesquisa

            Select Case d_ac��o_canal

                Case datNaturezas.Db

                    DatPrepara(datAlcances.select_pesquisa)
                    Return CType(d_canalDB.DbSelecciona(), IDATLET())

                Case datNaturezas.Xml

                    DatPrepara(datAlcances.select_pesquisa)
                    Return CType(d_canalXML.XmlSelecciona(), IDATLET())

            End Select

        End Function

        Public Overridable Function DatSeleccionaFichas() As IDATLET() Implements IDATLET.DatSeleccionaFichas

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case d_ac��o_canal

                Case datNaturezas.Db

                    DatPrepara(datAlcances.select_fichas)
                    Return CType(d_canalDB.DbSelecciona(), IDATLET())

                Case datNaturezas.Xml

                    DatPrepara(datAlcances.select_fichas)
                    Return CType(d_canalXML.XmlSelecciona(), IDATLET())

            End Select

        End Function

        Public Overridable Function DatAltera() As Boolean Implements IDATLET.DatAltera

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case d_ac��o_canal

                Case datNaturezas.Db

                    DatPrepara(datAlcances.update_ficha)
                    If d_canalDB.DbExecuta() Then
                        Return (d_canalDB.DbAfectados > 0)
                    Else
                        Return False
                    End If

                Case datNaturezas.Xml

                    DatPrepara(datAlcances.update_ficha)
                    If d_canalXML.XmlEscreve() Then
                        Return (d_canalXML.XmlAfectados > 0)
                    Else
                        Return False
                    End If

            End Select

        End Function

        Public Overridable Function DatInsere() As Boolean Implements IDATLET.DatInsere

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case d_ac��o_canal

                Case datNaturezas.Db

                    DatPrepara(datAlcances.insert_ficha)
                    If d_canalDB.DbExecuta() Then
                        Return (d_canalDB.DbAfectados > 0)
                    Else
                        Return False
                    End If

                Case datNaturezas.Xml

                    DatPrepara(datAlcances.insert_ficha)
                    If d_canalXML.XmlEscreve() Then
                        Return (d_canalXML.XmlAfectados > 0)
                    Else
                        Return False
                    End If

            End Select

        End Function

        Public Overridable Function DatElimina() As Boolean Implements IDATLET.DatElimina

            ' cria��o:      frank   01/10/2003
            ' altera��o:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case d_ac��o_canal

                Case datNaturezas.Db

                    DatPrepara(datAlcances.delete_ficha)
                    If d_canalDB.DbExecuta() Then
                        Return (d_canalDB.DbAfectados > 0)
                    Else
                        Return False
                    End If

                Case datNaturezas.Xml

                    DatPrepara(datAlcances.delete_ficha)
                    If d_canalXML.XmlElimina() Then
                        Return (d_canalXML.XmlAfectados > 0)
                    Else
                        Return False
                    End If

            End Select

        End Function

        Public Overridable Overloads Function DatConsultaFicha(ByVal ParamArray Chave() As Object) As Boolean Implements IDATLET.DatConsultaFicha

            ' cria��o:      frank   01/10/2003

            Select Case d_ac��o_canal
                Case datNaturezas.Db : d_canalDB.DbParametros = Chave
                Case datNaturezas.Xml : d_canalXML.XmlParametros = Chave
            End Select

            Return DatConsultaFicha()

        End Function

        Public Overridable Overloads Function DatConsultaRegisto(ByVal Ponteiro As Integer) As Boolean Implements IDATLET.DatConsultaRegisto

            ' cria��o:      frank   01/10/2003

            Dim pars(0) As Object
            pars(0) = Ponteiro

            Select Case d_ac��o_canal
                Case datNaturezas.Db : d_canalDB.DbParametros = pars
                Case datNaturezas.Xml : d_canalXML.XmlParametros = pars
            End Select

            Return DatConsultaRegisto()

        End Function

        Public Overridable Overloads Function DatExiste(ByVal ParamArray Chave() As Object) As Boolean Implements IDATLET.DatExiste

            ' cria��o:      frank   01/10/2003

            Select Case d_ac��o_canal
                Case datNaturezas.Db : d_canalDB.DbParametros = Chave
                Case datNaturezas.Xml : d_canalXML.XmlParametros = Chave
            End Select

            Return DatExiste()

        End Function

        Public Overridable Overloads Function DatConsultaVista(ByVal Ponteiro As Integer) As Boolean Implements IDATLET.DatConsultaVista

            ' cria��o:      frank   01/10/2003

            Dim pars(0) As Object
            pars(0) = Ponteiro

            Select Case d_ac��o_canal
                Case datNaturezas.Db : d_canalDB.DbParametros = pars
                Case datNaturezas.Xml : d_canalXML.XmlParametros = pars
            End Select

            Return DatConsultaVista()

        End Function

        Public Overridable Overloads Function DatPesquisa(ByVal ParamArray Descri��o() As Object) As IDATLET() Implements IDATLET.DatPesquisa

            ' 22/12/2003    frank       cria��o:

            Select Case d_ac��o_canal
                Case datNaturezas.Db : d_canalDB.DbParametros = Descri��o
                Case datNaturezas.Xml : d_canalXML.XmlParametros = Descri��o
            End Select

            Return DatPesquisa

        End Function

        Public Overridable Function DatActualiza() As Boolean Implements IDATLET.DatActualiza

            If CInt(DatAtributo(Me.l_info.Objecto.Ponteiro)) = 0 Then

                ' trata-se de um registo novo sem um ponteiro identificador

                If Not Me.DatAutoId Then

                    ' n�o foi poss�vel a autonumera��o
                    Return False

                End If

            End If

            If DatExiste() Then

                ' o registo j� existe
                Return DatAltera()

            Else

                ' o registo n�o existe
                Return DatInsere()

            End If

        End Function

        Public Overridable Property DatFiltros() As datInscri��o() Implements IDATLET.DatFiltros
            Get
            End Get
            Set(ByVal Value() As datInscri��o)
            End Set
        End Property


        Public Overridable Function DatAutoId() As Boolean Implements IDATLET.DatAutoId

            ' 01/10/2003    frank       cria��o
            ' 11/12/2003    frank       de canal unico (DB) para multicanal (DB, XML)

            Select Case d_ac��o_canal
                Case datNaturezas.Db

                    DatPrepara(datAlcances.select_maximo)
                    d_canalDB.DbConsulta(True)
                    If d_canalDB.DbAc��o.ResultadoEscalar Is Nothing Then
                        If d_canalDB.DbExcep��o Is Nothing Then
                            DatAtributo(d_canalDB.DbAc��o.Pedido.Objecto.Ponteiro) = 10
                            DatAutoId = True
                        Else
                            DatAutoId = False
                        End If
                    Else
                        DatAtributo(d_canalDB.DbAc��o.Pedido.Objecto.Ponteiro) = CInt(d_canalDB.DbAc��o.ResultadoEscalar) + 10
                        DatAutoId = True
                    End If

                Case datNaturezas.Xml

                    Dim Lista() As IXMLLET

                    DatPrepara(datAlcances.select_maximo)
                    Lista = d_canalXML.XmlSelecciona()
                    If Lista Is Nothing Then
                        If d_canalXML.XmlExcep��o Is Nothing Then
                            DatAtributo(d_canalDB.DbAc��o.Pedido.Objecto.Ponteiro) = 10
                            DatAutoId = True
                        Else
                            DatAutoId = False
                        End If
                    Else
                        DatAtributo(d_canalDB.DbAc��o.Pedido.Objecto.Ponteiro) = CInt(Lista(Lista.GetUpperBound(0)).DatAtributo(d_canalDB.DbAc��o.Pedido.Objecto.Ponteiro)) + 10
                        DatAutoId = False
                    End If

            End Select

        End Function

        Public Overridable Property DatExcep��o() As System.Exception Implements IDATLET.DatExcep��o
            Get

                Select Case d_ac��o_canal

                    Case datNaturezas.Db : Return d_canalDB.DbExcep��o
                    Case datNaturezas.Xml : Return d_canalXML.XmlExcep��o

                End Select

            End Get
            Set(ByVal Value As System.Exception)

                Select Case d_ac��o_canal

                    Case datNaturezas.Db : d_canalDB.DbExcep��o = Value
                    Case datNaturezas.Xml : d_canalXML.XmlExcep��o = Value

                End Select

            End Set
        End Property

        Public Overridable Sub DatReset() Implements IDATLET.DatReset

            ' cria��o:      frank   01/10/2003

            Me.DatInfo = l_info
            d_canalDB.DbReset()
            d_canalXML.XmlReset()

        End Sub

        Public Overridable Sub DatDispose() Implements IDATLET.DatDispose

                ' cria��o:      frank   12/11/2003

                d_canalXML.XmlDispose()
                d_canalDB.DbDispose()

        End Sub

        Public Overloads Function DatVerifica() As Boolean Implements IDATLET.DatVerifica

        End Function

        Public Overloads Function DatVerifica1(ByVal Ponteiro As Object) As Boolean Implements IDATLET.DatVerifica

        End Function
    End Class

End Namespace

