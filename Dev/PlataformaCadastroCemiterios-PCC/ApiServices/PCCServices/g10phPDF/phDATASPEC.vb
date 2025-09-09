Namespace Dataframe

    Public Structure phAcção

        Public Definida As Boolean
        Public Pedido As datPedido

        Public DbPreparada As Boolean
        Public DbSintaxe As String

        Public XmlPreparada As Boolean

        Public ResultadoEscalar As Object
        Public ResultadoAfectados As Integer

        Public Inscritos() As Integer           ' calculado: array de inteiros dos inscritos
        Public Filtros() As Integer             ' calculado: array de inteiros dos filtros
        Public Ordenações() As Integer          ' calculado: array de inteiros da ordenação
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

        Public Function Clone() As phAcção

            Dim NovaAcção As phAcção

            NovaAcção.Definida = Me.Definida
            NovaAcção.Pedido = Me.Pedido

            NovaAcção.DbPreparada = Me.DbPreparada
            NovaAcção.DbSintaxe = String.Copy(Me.DbSintaxe)

            NovaAcção.XmlPreparada = Me.XmlPreparada

            NovaAcção.ResultadoEscalar = Me.ResultadoEscalar
            NovaAcção.ResultadoAfectados = Me.ResultadoAfectados

            If Not Me.Inscritos Is Nothing Then NovaAcção.Inscritos = CType(Me.Inscritos.Clone, Integer())
            If Not Me.Filtros Is Nothing Then NovaAcção.Filtros = CType(Me.Filtros.Clone, Integer())
            If Not Me.Ordenações Is Nothing Then NovaAcção.Ordenações = CType(Me.Ordenações.Clone, Integer())
            If Not Me.Aceites Is Nothing Then NovaAcção.Aceites = CType(Me.Aceites.Clone, Integer())

            Return NovaAcção

        End Function

    End Structure

    Public Class phDatlet

        Implements IDBLET
        Implements IXMLLET
        Implements IDATLET

        ' 01/10/2003    frank       criação
        ' 11/12/2003    frank       acréscimo de mais uma acção "acção_select_primeiro" 
        ' 22/12/2003    frank       acréscimo de mais uma acção "acção_select_pesquisa" 
        ' 22/12/2003    frank       o conteudo do parametro pode ser tirado de um array de atributos paralelo 
        '                           o objectivo é permitir indicar valores sem alterar os conteudos da classe 
        '                           cada posição deste array tem de conter o dado para o campo 
        '                               apontado pelo filtro para a mesma posição 
        '                           isto só é valido para ações de selecção em que o filtro corrosponde 
        '                               aos campos condicionantes 
        '                           em acções de eliminação, inserção ou alteração o filtro indica 
        '                               campos a trabalhar e nem sempre são iguais aos campos que 
        '                               condicionam o alcance da mesma acção 
        ' 29/12/2003    frank       criação de uma phAcção que substituirá a SpecAcçãoDB e a SpecAcçãoXML
        ' 29/12/2003    frank       acção_parametros: esta propriedade força a parametrização de um comando 
        '                               a ser feita por estes conteudos e não pelo conteudo das propriedades da classe

        Protected l_info As DatInfo
        Protected l_atributos() As Object
        Protected l_separador As String

        Protected l_acções() As phAcção
        Protected l_acção As phAcção
        Protected l_filtros() As datInscrição

        Protected b_conector As System.Data.IDbConnection
        Protected b_construtor As datConstrutorSQL
        Protected b_excepção As System.Exception

        Protected x_ficheiro As String
        Protected x_nobase As System.Xml.XmlNode
        Protected x_excepção As System.Exception
        Protected x_eliminado As Boolean

        Public Const acção_count As Integer = 12
        Public Const acção_especial As Integer = 0
        Public Const acção_select_ficha As Integer = 1
        Public Const acção_select_registo As Integer = 2
        Public Const acção_select_lista As Integer = 3
        Public Const acção_select_fichas As Integer = 4
        Public Const acção_select_procura As Integer = 5
        Public Const acção_select_proximo As Integer = 6
        Public Const acção_select_vista As Integer = 7
        Public Const acção_update_ficha As Integer = 8
        Public Const acção_insert_ficha As Integer = 9
        Public Const acção_delete_ficha As Integer = 10
        Public Const acção_select_primeiro As Integer = 11
        Public Const acção_select_pesquisa As Integer = 12

        Protected acção_canal As datNaturezas
        Protected acção_parametros() As Object

#Region "Código de suporte da própria classe"

        ' criação:      frank   01/10/2003

        Public Erro As Boolean
        Public ErroAvisa As Boolean
        Public ErroResposta As MsgBoxResult



        Public Sub New()

            ' 01/10/2003    frank   Construtor de Serviço
            '                       a ausencia de qualquer tipo de iniciailização permite a criação 
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

            ' criação:      frank   01/10/2003

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

            ' criação:      frank   01/10/2003

            With the_clone

                ' ILET
                .l_info = Me.l_info
                If Not Me.l_atributos Is Nothing Then .l_atributos = CType(Me.l_atributos.Clone, Object())
                .l_separador = Me.l_separador

                If Not Me.l_acções Is Nothing Then .l_acções = CType(Me.l_acções.Clone, phAcção())
                .l_acção = Me.l_acção.Clone

                .acção_canal = Me.acção_canal
                If Not Me.acção_parametros Is Nothing Then .acção_parametros = CType(Me.acção_parametros.Clone, Object())

                ' IDBLET
                .b_conector = Me.b_conector
                .b_construtor = Me.b_construtor
                .b_excepção = Me.b_excepção

                ' IXMLET
                .x_ficheiro = Me.x_ficheiro
                .x_nobase = Me.x_nobase
                .x_excepção = Me.x_excepção
                .x_eliminado = Me.x_eliminado


            End With

        End Sub

        Protected Overridable Sub l_defineacção(ByVal acção As Integer)

            ' definição da acção 

            l_acções(acção).Pedido = New datPedido
            l_acções(acção).Pedido.Objecto = Me.l_info.Objecto

            Select Case acção

                Case acção_select_ficha
                    l_acções(acção).Pedido.Operação = datOperações.Consulta
                    l_acções(acção).Pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosTodos))
                    l_acções(acção).Pedido.MaisFiltros(datInscrição.Novas(l_info.Objecto.MembrosChave))
                    l_acções(acção).Pedido.Ordenações = Nothing

                Case acção_select_registo
                    l_acções(acção).Pedido.Operação = datOperações.Consulta
                    l_acções(acção).Pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosTodos))
                    l_acções(acção).Pedido.MaisFiltros(datInscrição.Novas(l_info.Objecto.MembrosPonteiro))
                    l_acções(acção).Pedido.Ordenações = Nothing

                Case acção_select_lista
                    l_acções(acção).Pedido.Operação = datOperações.Consulta
                    l_acções(acção).Pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosLista))
                    l_acções(acção).Pedido.Filtros = Nothing
                    l_acções(acção).Pedido.MaisOrdenações(datInscrição.Novas(l_info.Objecto.MembrosChave))

                Case acção_select_fichas
                    l_acções(acção).Pedido.Operação = datOperações.Consulta
                    l_acções(acção).Pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosTodos))
                    l_acções(acção).Pedido.Filtros = Nothing
                    l_acções(acção).Pedido.MaisOrdenações(datInscrição.Novas(l_info.Objecto.MembrosChave))

                Case acção_select_procura
                    l_acções(acção).Pedido.Operação = datOperações.Consulta
                    l_acções(acção).Pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosPonteiro))
                    l_acções(acção).Pedido.MaisFiltros(datInscrição.Novas(l_info.Objecto.MembrosChave))
                    l_acções(acção).Pedido.Ordenações = Nothing

                Case acção_select_vista
                    l_acções(acção).Pedido.Operação = datOperações.Consulta
                    l_acções(acção).Pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosVista))
                    l_acções(acção).Pedido.MaisFiltros(datInscrição.Novas(l_info.Objecto.MembrosPonteiro))
                    l_acções(acção).Pedido.Ordenações = Nothing

                Case acção_select_proximo
                    l_acções(acção).Pedido.Operação = datOperações.Consulta
                    l_acções(acção).Pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosPonteiro))
                    l_acções(acção).Pedido.Filtros = Nothing
                    l_acções(acção).Pedido.Ordenações = Nothing

                Case acção_update_ficha
                    l_acções(acção).Pedido.Operação = datOperações.Alteração
                    l_acções(acção).Pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosTodos))
                    l_acções(acção).Pedido.MaisFiltros(datInscrição.Novas(l_info.Objecto.MembrosPonteiro))
                    l_acções(acção).Pedido.Ordenações = Nothing

                Case acção_insert_ficha
                    l_acções(acção).Pedido.Operação = datOperações.Inserção
                    l_acções(acção).Pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosTodos))
                    l_acções(acção).Pedido.MaisFiltros(datInscrição.Novas(l_info.Objecto.MembrosTodos))
                    l_acções(acção).Pedido.Ordenações = Nothing

                Case acção_delete_ficha
                    l_acções(acção).Pedido.Operação = datOperações.Eliminação
                    l_acções(acção).Pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosTodos))
                    l_acções(acção).Pedido.MaisFiltros(datInscrição.Novas(l_info.Objecto.MembrosPonteiro))
                    l_acções(acção).Pedido.Ordenações = Nothing

                Case acção_select_primeiro
                    l_acções(acção).Pedido.Operação = datOperações.Consulta
                    l_acções(acção).Pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosTodos))
                    l_acções(acção).Pedido.Filtros = Nothing
                    l_acções(acção).Pedido.Ordenações = Nothing

                Case acção_select_pesquisa
                    l_acções(acção).Pedido.Operação = datOperações.Consulta
                    l_acções(acção).Pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosPesquisa))
                    l_acções(acção).Pedido.MaisFiltros(datInscrição.Novas(l_info.Objecto.MembrosDescrição))
                    l_acções(acção).Pedido.MaisOrdenações(datInscrição.Novas(l_info.Objecto.MembrosChave))

            End Select

            If Not l_filtros Is Nothing Then l_acções(acção).Pedido.MaisFiltros(l_filtros)
            l_acções(acção).Pedido.AutoCompleta()

        End Sub


        Protected Overridable Function b_exec(ByVal comandoSQL As String) As Boolean

            ' criação:      frank   01/10/2003

            Dim Comando As System.Data.IDbCommand
            Dim Cursor As System.Data.IDataReader

            Comando = b_conector.CreateCommand
            Comando.CommandType = CommandType.Text
            Comando.CommandText = comandoSQL

            b_parametriza(Comando)

            Do

                ErroResposta = MsgBoxResult.Ok
                b_excepção = Nothing
                Erro = False

                Try

                    l_acção.ResultadoAfectados = Comando.ExecuteNonQuery
                    Return True

                Catch execepção_ao_comando As System.Exception

                    Erro = True
                    b_excepção = execepção_ao_comando
                    l_acção.ResultadoAfectados = 0

                    ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcepção(b_excepção, Comando.CommandText, ErroAvisa, True, False)

                End Try

            Loop While ErroResposta = MsgBoxResult.Retry

            Return False

        End Function

        Protected Overridable Function b_select(ByVal comandoSQL As String, ByVal Cursor As System.Data.IDataReader, ByVal Escalar As Boolean) As Boolean

            ' criação:      frank   01/10/2003

            Dim Comando As System.Data.IDbCommand

            Comando = b_conector.CreateCommand
            Comando.CommandType = CommandType.Text
            Comando.CommandText = comandoSQL

            b_parametriza(Comando)

            Do

                Erro = False
                ErroResposta = MsgBoxResult.Ok
                b_excepção = Nothing

                Try

                    If Escalar Then
                        l_acção.ResultadoEscalar = Comando.ExecuteScalar
                    Else
                        Cursor = Comando.ExecuteReader
                    End If

                    If Cursor Is Nothing Then
                        Return False
                    Else
                        Return True
                    End If

                Catch execepção_ao_comando As System.Exception

                    Erro = True
                    b_excepção = execepção_ao_comando

                    ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcepção(b_excepção, Comando.CommandText, ErroAvisa, True, False)

                End Try

            Loop While ErroResposta = MsgBoxResult.Retry

            Return False

        End Function

        Protected Overridable Sub b_parametriza(ByVal comando As System.Data.IDbCommand)

            ' criação:      frank   01/10/2003
            ' alteração:    frank   22/12/2003      o conteudo do parametro pode ser tirado de uma 
            '                                       array de atributos paralelo

            Dim parametros() As System.Data.IDataParameter
            Dim f As Integer
            Dim c As Integer

            comando.Parameters.Clear()

            If Not l_acção.Filtros Is Nothing Then

                ReDim parametros(l_acção.Filtros.GetUpperBound(0))

                For f = 0 To l_acção.Filtros.GetUpperBound(0)

                    c = l_acção.Filtros(f)

                    parametros(f) = comando.CreateParameter
                    parametros(f).ParameterName = b_construtor.SintaxeDeParametroValor(l_acção.Pedido.Objecto.Membros(c))
                    parametros(f).DbType = l_acção.Pedido.Objecto.Membros(c).Tipo

                    If acção_parametros Is Nothing Then
                        parametros(f).Value = DatAtributo(c)
                    Else
                        parametros(f).Value = acção_parametros(f)
                    End If

                    If parametros(f).Value Is Nothing Then parametros(f).Value = ""

                    comando.Parameters.Add(parametros(f))

                Next

            End If

            acção_parametros = Nothing

        End Sub


        Protected Overridable Sub x_defxmlno(ByVal no_registo As System.Xml.XmlNode)

            ' criação:      frank   01/10/2003

            Dim a As Integer
            Dim c As Integer

            For a = 0 To l_acção.Pedido.Aceites.GetUpperBound(0)

                c = l_acção.Inscritos(a)
                no_registo.AppendChild(no_registo.OwnerDocument.CreateElement(l_info.Objecto.Membros(c).Nome))

            Next

        End Sub

        Protected Overridable Function x_select(ByVal no_datlet As System.Xml.XmlNode, ByVal filtros() As Integer) As System.Xml.XmlNode

            Dim no_registo As System.Xml.XmlNode
            Dim f As Integer
            Dim a As Integer
            Dim coincide As Boolean

            ' método parecido com x_selectvarios

            If no_datlet.HasChildNodes Then

                For Each no_registo In no_datlet.ChildNodes

                    coincide = False

                    If filtros Is Nothing Then
                        coincide = True
                    Else

                        For f = 0 To filtros.GetUpperBound(0)

                            a = filtros(f)

                            If acção_parametros Is Nothing Then

                                If no_registo.Item(l_info.Objecto.Membros(a).Nome).InnerText = CType(DatAtributo(a), String) Then
                                    coincide = coincide And True
                                End If

                            Else

                                If no_registo.Item(l_info.Objecto.Membros(a).Nome).InnerText = CType(acção_parametros(f), String) Then
                                    coincide = coincide And True
                                End If

                            End If

                        Next

                    End If

                    If coincide Then

                        ' a diferença com o método x_select é aqui:
                        ' este método quando encontra um nó retorna-o imediatamente sem continuara a procura

                        acção_parametros = Nothing
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

            ' método parecido com x_select 

            If no_datlet.HasChildNodes Then

                For Each no_registo In no_datlet.ChildNodes

                    coincide = False

                    If filtros Is Nothing Then
                        coincide = True
                    Else

                        For f = 0 To filtros.GetUpperBound(0)

                            a = filtros(f)

                            If acção_parametros Is Nothing Then

                                If no_registo.Item(l_info.Objecto.Membros(a).Nome).InnerText = CType(DatAtributo(a), String) Then
                                    coincide = coincide And True
                                End If

                            Else

                                If no_registo.Item(l_info.Objecto.Membros(a).Nome).InnerText = CType(acção_parametros(f), String) Then
                                    coincide = coincide And True
                                End If

                            End If

                        Next

                    End If

                    If coincide Then

                        ' a diferença com o método x_select é aqui:
                        ' este método quando encontra um nó adiciona-o a uma lista que retornará

                        ReDim Preserve no_lista(t)
                        no_lista(t) = no_registo
                        t = t + 1

                    End If

                Next

                acção_parametros = Nothing
                Return no_lista

            Else

                acção_parametros = Nothing
                Return Nothing

            End If

        End Function

        Protected Overrides Sub Finalize()

            Me.DatDispose()

            MyBase.Finalize()

        End Sub

#End Region

#Region "Implementação da interface ILET"

        Public Overridable Property DatInfo() As datInfo Implements ILET.DatInfo
            Get

                Return l_info

            End Get
            Set(ByVal Value As datInfo)

                l_info = Value

            End Set
        End Property

        Public Overridable Property DatAtributo(ByVal Index As Integer) As Object Implements ILET.DatAtributo

            ' criação:      frank   01/10/2003

            Get

                Return l_atributos(Index)

            End Get
            Set(ByVal Value As Object)

                l_atributos(Index) = Value

            End Set
        End Property

        Public Overridable Function DatClone() As ILET Implements ILET.DatClone


            ' 01/10/2003    frank   cria uma cópia desta classe
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

            ' criação:      frank   12/11/2003

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

#Region "Implementação da interface IDBLET"

        ' frank   01/10/2003:   criação

        Public Event DbJoin(ByVal Agregador As datMembro, ByVal Cursor As System.Data.IDataReader, ByVal InscriçõesAceites() As datAceitação, ByRef JoinIndex As Integer) Implements IDBLET.DbJoin

        Public Overridable Sub DbReset() Implements IDBLET.DbReset

            ' criação:      frank   01/10/2003

            b_excepção = Nothing

            l_acções = Nothing
            ReDim l_acções(acção_count)
            l_acção = Nothing

        End Sub

        Public Overridable Property DbConector() As System.Data.IDbConnection Implements IDBLET.DbConector

            ' criação:      frank   01/10/2003

            Get

                DbConector = b_conector

            End Get
            Set(ByVal Value As System.Data.IDbConnection)

                b_conector = Value

            End Set
        End Property

        Public Overridable Property DbConstrutor() As datConstrutorSQL Implements IDBLET.DbConstrutor

            ' criação:      frank   01/10/2003

            Get

                DbConstrutor = b_construtor

            End Get
            Set(ByVal Value As datConstrutorSQL)

                b_construtor = Value

            End Set
        End Property

        Public Overridable Property DbExcepção() As System.Exception Implements IDBLET.DbExcepção
            Get

                DbExcepção = b_excepção

            End Get
            Set(ByVal Value As System.Exception)

                b_excepção = Value

            End Set
        End Property


        Public Overridable Function DbPrepara(ByVal Acção As Integer) As Boolean Implements IDBLET.DbPrepara

            ' criação:      frank   01/10/2003
            ' alteração:    frank   11/12/2003          adição de novo alcance "acção_select_primeiro"
            ' alteração:    frank   22/12/2003          adição de novo alcance "acção_select_pesquisa"

            If Not l_acções(Acção).Definida Then

                ' definição da acção (objecto, inscritos, filtros, ordenação)

                l_defineacção(Acção)
                l_acções(Acção).Definida = True

            End If

            If Not l_acções(Acção).DbPreparada Then

                ' construção do sintaxe (comando sql) 
                ' e obtenção da lista de aceites (campos presentes no comando)

                l_acções(Acção).DbSintaxe = b_construtor.ConstroiSintaxe(l_acções(Acção).Pedido)
                l_acções(Acção).AssimilaPedido()
                l_acções(Acção).DbPreparada = True

            End If

            l_acção = l_acções(Acção)

            Return True

        End Function

        Public Overridable Function DbExecuta() As Boolean Implements IDBLET.DbExecuta

            ' criação:      frank   01/10/2003

            Return b_exec(l_acção.DbSintaxe)

        End Function

        Public Overridable Overloads Function DbConsulta() As Boolean Implements IDBLET.DbConsulta

            ' criação:      frank   01/10/2003

            Dim Cursor As System.Data.IDataReader

            DbConsulta = b_select(l_acção.DbSintaxe, Cursor, False)

            If DbConsulta Then

                If DbSaca(Cursor, True, l_acção.Pedido.Aceites, 0) Then l_acção.ResultadoAfectados = 1

                Cursor.Close()

            Else

                l_acção.ResultadoAfectados = 0

            End If

        End Function

        Public Overridable Function DbSelecciona() As IDBLET() Implements IDBLET.DbSelecciona

            ' criação:      frank   01/10/2003

            Dim cursor As System.Data.IDataReader
            Dim lista() As phDatlet
            Dim l As Integer
            Dim sucesso As Boolean

            l_acção.ResultadoAfectados = 0
            sucesso = b_select(l_acção.DbSintaxe, cursor, False)

            If sucesso Then

                l = -1
                While cursor.Read

                    l += 1
                    ReDim Preserve lista(l)

                    lista(l) = CType(Me.DatClone, phDatlet)
                    lista(l).l_acção.Pedido.Objecto = l_info.Objecto
                    lista(l).DbSaca(cursor, False, l_acção.Pedido.Aceites, 0)

                End While

                cursor.Close()

                l_acção.ResultadoAfectados = l + 1
                Return lista

            Else

                Return Nothing

            End If

        End Function

        Public Overridable ReadOnly Property DbAfectados() As Integer Implements IDBLET.DbAfectados

            ' criação:      frank   01/10/2003

            Get

                DbAfectados = l_acção.ResultadoAfectados

            End Get
        End Property

        Public Overridable Function DbSaca(ByVal Cursor As System.Data.IDataReader, ByVal CursorFetch As Boolean) As Boolean Implements IDBLET.DbSaca

            ' criação:      frank   01/10/2003

            Return DbSaca(Cursor, CursorFetch, l_acção.Pedido.Aceites, 0)

        End Function

        Public Overridable Function DbSaca(ByVal Cursor As System.Data.IDataReader, ByVal CursorFetch As Boolean, ByVal InscriçõesAceites() As datAceitação, ByRef JoinIndex As Integer) As Boolean Implements IDBLET.DbSaca

            ' criação:      frank   01/10/2003

            Dim i As Integer
            Dim m As Integer
            Dim ji As Integer

            If CursorFetch Then
                DbSaca = Cursor.Read
            Else
                DbSaca = True
            End If

            If DbSaca Then

                If InscriçõesAceites Is Nothing Then

                    '                                                                                       

                    For m = 0 To Cursor.FieldCount - 1

                        If Cursor.IsDBNull(m) Then
                            DatAtributo(m) = Nothing
                        Else
                            Select Case l_acção.Pedido.Objecto.Membros(m).Tipo

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

                    For i = 0 To InscriçõesAceites.GetUpperBound(0)

                        If InscriçõesAceites(i).JoinNivel = ji Then

                            m = InscriçõesAceites(i).Membro.Ordem

                            If Cursor.IsDBNull(i) Then
                                DatAtributo(m) = Nothing
                            Else
                                Select Case l_acção.Pedido.Objecto.Membros(m).Tipo

                                    Case DbType.Int32 : DatAtributo(m) = Cursor.GetInt32(i)
                                    Case DbType.Decimal : DatAtributo(m) = Cursor.GetDecimal(i)
                                    Case DbType.Date : DatAtributo(m) = Cursor.GetDateTime(i)
                                    Case DbType.Time : DatAtributo(m) = Cursor.GetDateTime(i)
                                    Case DbType.Boolean : DatAtributo(m) = Cursor.GetBoolean(i)
                                    Case Else : DatAtributo(m) = Cursor.GetString(i)

                                End Select
                            End If

                            If Not l_acção.Pedido.Objecto.Membros(m).Referencia Is Nothing Then

                                JoinIndex += 1
                                RaiseEvent DbJoin(l_acção.Pedido.Objecto.Membros(m), Cursor, l_acção.Pedido.Aceites, JoinIndex)

                            End If
                        End If

                    Next

                    '                                                                                       

                End If

            End If

        End Function

        Public Overridable Function DbLink(ByVal Objecto As datObjecto) As Integer() Implements IDBLET.DbLink

            ' criação:      frank   01/10/2003

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

            ' criação:      frank   12/11/2003

            b_conector = Nothing
            b_construtor = Nothing

        End Sub

        Public Overridable Property DbAcção() As datAcção Implements IDBLET.DbAcção
            Get

            End Get
            Set(ByVal Value As datAcção)

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

#Region "Implementação da interface IXMLLET"

        Public Overridable Sub XmlReset() Implements IXMLLET.XmlReset

            ' criação:      frank   01/10/2003

            x_excepção = Nothing
            x_eliminado = False
            l_acção = Nothing

        End Sub

        Public Overridable Property XmlFicheiro() As String Implements IXMLLET.XmlFicheiro

            ' criação:      frank   01/10/2003

            Get

                XmlFicheiro = x_ficheiro

            End Get
            Set(ByVal Value As String)

                x_ficheiro = Value

            End Set
        End Property

        Public Overridable Property XmlNoBase() As System.Xml.XmlNode Implements IXMLLET.XmlNoBase

            ' criação:      frank   01/10/2003

            Get

                XmlNoBase = Me.x_nobase

            End Get
            Set(ByVal Value As System.Xml.XmlNode)

                Me.x_nobase = Value

            End Set
        End Property

        Public Overridable Property XmlExcepção() As System.Exception Implements IXMLLET.XmlExcepção
            Get

                XmlExcepção = x_excepção

            End Get
            Set(ByVal Value As System.Exception)

                x_excepção = Value

            End Set
        End Property

        Public Overridable Function XmlPrepara(ByVal Acção As Integer) As Boolean Implements IXMLLET.XmlPrepara

            ' criação:      frank   01/10/2003
            ' alteração:    frank   11/12/2003      as acções xml passam a ser filtradas, 
            '                                       os registos são procurados tendo em consideração o conteudo
            '                                       de determinados atributos
            ' alteração:    frank   11/12/2003      novo alcance "acção_select_primeiro"
            ' 18/12/2003    frank   ao contrário das acções DB as acções XML já preparadas não são armazenadas
            '                       cada vez que se prepara uma acção XML todos os passos de preparação são efectuados

            If Not l_acções(Acção).Definida Then

                ' definição da acção (objecto, inscritros e filtros)

                l_defineacção(Acção)
                l_acções(Acção).Definida = True

            End If

            If Not l_acções(Acção).XmlPreparada Then

                l_acções(Acção).AssimilaPedido()
                l_acções(Acção).XmlPreparada = True

            End If

            l_acção = l_acções(Acção)

            Return True

        End Function

        Public Overridable Function XmlConsulta() As Boolean Implements IXMLLET.XmlConsulta

            ' criação:      frank   01/10/2003
            ' alteração:    frank   11/12/2003          dos nós existentes procura aquele cujo ponteiro seja igual
            '                                           ao conteudo do atributo ponteiro da classe
            '                                           se este valor for nulo "0" lé o primeiro nó disponível
            ' alteração:    frank   11/12/2003          dos nós existentes procura aquele cujo conteudos dos Membros
            '                                           filtrados seja igual aos dos atributos da classe

            Dim no_datlet As System.Xml.XmlNode
            Dim no_registo As System.Xml.XmlNode

            Do

                Erro = False
                x_excepção = Nothing
                no_datlet = XMLGetNo(x_nobase, x_ficheiro, l_info.Objecto.DbGrupo, False, Nothing, x_excepção)

                If Not x_excepção Is Nothing Then

                    Erro = True

                    ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcepção(x_excepção, x_ficheiro, ErroAvisa, True, False)

                End If

            Loop While ErroResposta = MsgBoxResult.Retry

            If no_datlet Is Nothing Then

                l_acção.ResultadoAfectados = 0
                Return False

            Else

                If no_datlet.FirstChild Is Nothing Then

                    l_acção.ResultadoAfectados = 0

                Else

                    If l_acção.Filtros Is Nothing Then
                        no_registo = no_datlet.FirstChild
                    Else
                        no_registo = x_select(no_datlet, l_acção.Filtros)
                    End If

                    If Not no_registo Is Nothing Then

                        XmlSaca(no_registo, l_acção.Inscritos)
                        l_acção.ResultadoAfectados = 1

                    Else

                        l_acção.ResultadoAfectados = 0

                    End If

                End If

                Return True

            End If

        End Function

        Public Overridable Function XmlSelecciona() As IXMLLET() Implements IXMLLET.XmlSelecciona

            ' criação:      frank   01/10/2003
            ' alteração:    frank   11/12/2003          dos nós existentes procura aqueles cujo conteudos dos Membros
            '                                           filtrados seja igual aos dos atributos da classe

            Dim no_datlet As System.Xml.XmlNode
            Dim no_registos() As System.Xml.XmlNode
            Dim lista() As IXMLLET
            Dim l As Integer

            Do

                Erro = False
                x_excepção = Nothing
                no_datlet = XMLGetNo(x_nobase, x_ficheiro, l_info.Objecto.DbGrupo, False, Nothing, x_excepção)

                If Not x_excepção Is Nothing Then

                    Erro = True

                    ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcepção(x_excepção, x_ficheiro, ErroAvisa, True, False)

                End If

            Loop While ErroResposta = MsgBoxResult.Retry

            If no_datlet Is Nothing Then

                l_acção.ResultadoAfectados = 0
                Return Nothing

            Else

                no_registos = x_selectvarios(no_datlet, l_acção.Filtros)

                ReDim lista(no_registos.GetUpperBound(0))

                For l = 0 To lista.GetUpperBound(0)

                    lista(l) = CType(Me.DatClone, phDatlet)
                    lista(l).XmlSaca(no_registos(l), l_acção.Inscritos)

                Next

                l_acção.ResultadoAfectados = l + 1
                Return lista

            End If

        End Function

        Public Overridable Overloads Function XmlEscreve() As Boolean Implements IXMLLET.XmlEscreve

            ' criação:      frank   01/10/2003
            ' alteração:    frank   11/12/2003      antes de gravar procura o nó corrospondente e modifica-o
            ' alteração:    frank   11/12/2003      verifica se o nó foi marcado para eliminação

            Dim xmldoc As System.Xml.XmlDocument
            Dim no_datlet As System.Xml.XmlNode
            Dim no_registo As System.Xml.XmlNode

            Dim n As Integer

            Do

                Erro = False
                x_excepção = Nothing
                no_datlet = XMLGetNo(x_nobase, x_ficheiro, l_info.Objecto.DbGrupo, False, xmldoc, x_excepção)

                If Not x_excepção Is Nothing Then

                    Erro = True

                    ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcepção(x_excepção, x_ficheiro, ErroAvisa, True, False)

                End If

            Loop While ErroResposta = MsgBoxResult.Retry

            If no_datlet Is Nothing Then

                l_acção.ResultadoAfectados = 0
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
                    XmlEnche(no_registo, l_acção.Inscritos)
                End If

                If Not xmldoc Is Nothing Then xmldoc.Save(XmlFicheiro)

                l_acção.ResultadoAfectados = 1
                Return True

            End If

        End Function

        Public Overridable Overloads Function XmlEscreve(ByVal Lista() As IXMLLET) As Boolean Implements IXMLLET.XmlEscreve

            ' criação:      frank   01/10/2003
            ' alteração.    frank   11/12/2003      uma a uma evoca o evento XMLEscreve de cada classe da lista

            Dim xmldoc As System.Xml.XmlDocument
            Dim no_datlet As System.Xml.XmlNode

            Dim l As Integer

            Do

                Erro = False
                x_excepção = Nothing
                no_datlet = XMLGetNo(x_nobase, x_ficheiro, l_info.Objecto.DbGrupo, False, xmldoc, x_excepção)

                If Not x_excepção Is Nothing Then

                    Erro = True

                    ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcepção(x_excepção, x_ficheiro, ErroAvisa, True, False)

                End If

            Loop While ErroResposta = MsgBoxResult.Retry

            If no_datlet Is Nothing Then

                l_acção.ResultadoAfectados = 0
                Return False

            Else

                For l = 0 To Lista.GetUpperBound(0)

                    Lista(l).XmlNoBase = no_datlet
                    Lista(l).XmlEscreve()
                    l_acção.ResultadoAfectados = l + Lista(l).XmlAfectados

                Next

                If Not xmldoc Is Nothing Then xmldoc.Save(XmlFicheiro)

                Return True

            End If

        End Function

        Public Overridable Function XmlElimina() As Boolean Implements IXMLLET.XmlElimina

            ' criação:      frank   11/12/2003

            Dim xmldoc As System.Xml.XmlDocument
            Dim no_datlet As System.Xml.XmlNode
            Dim no_registo As System.Xml.XmlNode

            Dim n As Integer

            x_eliminado = True
            x_excepção = Nothing

            If Not x_nobase Is Nothing Or x_ficheiro.Length > 0 Then

                Do

                    Erro = False
                    x_excepção = Nothing
                    no_datlet = XMLGetNo(x_nobase, x_ficheiro, l_info.Objecto.DbGrupo, False, xmldoc, x_excepção)

                    If Not x_excepção Is Nothing Then

                        Erro = True

                        ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcepção(x_excepção, x_ficheiro, ErroAvisa, True, False)

                    End If

                Loop While ErroResposta = MsgBoxResult.Retry

                If no_datlet Is Nothing Then

                    l_acção.ResultadoAfectados = 0
                    Return False

                Else

                    no_registo = x_select(no_datlet, l_info.Objecto.ArrayPonteiro)

                    If no_registo Is Nothing Then

                        l_acção.ResultadoAfectados = 0

                    Else

                        no_datlet.RemoveChild(no_registo)

                        If Not xmldoc Is Nothing Then xmldoc.Save(XmlFicheiro)

                        l_acção.ResultadoAfectados = 1

                    End If

                    Return True

                End If

            Else

                Return True

            End If

        End Function

        Public Overridable ReadOnly Property XmlAfectados() As Integer Implements IXMLLET.XmlAfectados
            Get

                XmlAfectados = l_acção.ResultadoAfectados

            End Get
        End Property

        Public Overridable Function XmlSaca(ByVal No As System.Xml.XmlNode, ByVal InscriçõesAceites() As Integer) As Boolean Implements IXMLLET.XmlSaca

            ' criação:      frank   01/10/2003
            ' alteração:    frank   10/11/2003      passamos a aceder ao nó pelo seu nome
            '                                       a vantagem de aceder ao nó pelo seu nome é de que a ordem 
            '                                       destes no XML pode ser alterada sem prejuizo para as funções, 
            '                                       mais: sem uma ordem fixa os nós podem até não estar presentes no XML

            Dim a As Integer
            Dim c As Integer

            For a = 0 To InscriçõesAceites.GetUpperBound(0)

                c = InscriçõesAceites(a)
                'DatAtributo(c) = No.ChildNodes(a).InnerText
                DatAtributo(c) = No.Item(l_acção.Pedido.Objecto.Membros(c).Nome).InnerText

            Next

            x_eliminado = False

            Return True

        End Function

        Public Overridable Function XmlEnche(ByVal No As System.Xml.XmlNode, ByVal InscriçõesAceites() As Integer) As Boolean Implements IXMLLET.XmlEnche

            ' criação:      frank   01/10/2003
            ' alteração:    frank   10/11/2003      passamos a aceder ao nó pelo seu nome
            '                                       a vantagem de aceder ao nó pelo seu nome é de que a ordem 
            '                                       destes no XML pode ser alterada sem prejuizo para as funções, 
            '                                       mais, sem uma ordem fixa os nós podem até não estar presentes no XML

            Dim a As Integer
            Dim c As Integer

            For a = 0 To InscriçõesAceites.GetUpperBound(0)

                c = InscriçõesAceites(a)
                'No.ChildNodes(a).InnerText = CType(DatAtributo(c), String)
                No.Item(l_acção.Pedido.Objecto.Membros(c).Nome).InnerText = CType(DatAtributo(c), String)

            Next

        End Function

        Public Overridable Sub XmlDispose() Implements IXMLLET.XmlDispose

            ' criação:      frank   12/11/2003

            x_ficheiro = Nothing
            x_nobase = Nothing

        End Sub

        Public Overridable Property XmlAcção() As datAcção Implements IXMLLET.XmlAcção
            Get

            End Get
            Set(ByVal Value As datAcção)

            End Set
        End Property

        Public Overridable Property XmlParametros() As Object() Implements IXMLLET.XmlParametros
            Get

            End Get
            Set(ByVal Value() As Object)

            End Set
        End Property

#End Region

#Region "Implementação da interface IDATLET"

        ' criação:      frank   01/10/2003

        Public Overridable Property DatCanal() As datCanal Implements IDATLET.DatCanal
            Get
                ' não expoe esta propriedade porque a phDatlet orienta-se pela Natureza
            End Get
            Set(ByVal Value As datCanal)

            End Set
        End Property

        Public Overridable Property DatNatureza() As datNaturezas
            Get

                Return acção_canal

            End Get
            Set(ByVal Value As datNaturezas)

                acção_canal = Value

            End Set
        End Property

        Public Overridable Property DatAcção() As datAcção Implements IDATLET.DatAcção
            Get
                ' a phDatlet usa um modelo de acção ligeiramente diferente por isso não pode expo-la
            End Get
            Set(ByVal Value As datAcção)

            End Set
        End Property

        Public Overridable Property DatParametros() As Object() Implements IDATLET.DatParametros
            Get

                Return acção_parametros

            End Get
            Set(ByVal Value() As Object)

                acção_parametros = Value

            End Set
        End Property



        Public Overridable Sub DatPrepara(ByVal Alcance As Integer) Implements IDATLET.DatPrepara

            Select Case acção_canal

                Case datNaturezas.Db

                    DbPrepara(acção_select_primeiro)

                Case datNaturezas.Xml

                    XmlPrepara(acção_select_primeiro)

            End Select

        End Sub

        Public Overridable Function DatConsultaPrimeiro() As Boolean Implements IDATLET.DatConsultaPrimeiro

            ' criação:      frank   01/10/2003

            Select Case acção_canal

                Case datNaturezas.Db

                    DatPrepara(acção_select_primeiro)
                    If DbConsulta() Then
                        Return (l_acção.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

                Case datNaturezas.Xml

                    DatPrepara(acção_select_primeiro)
                    If XmlConsulta() Then
                        Return (l_acção.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

            End Select

        End Function

        Public Overridable Overloads Function DatConsultaFicha() As Boolean Implements IDATLET.DatConsultaFicha

            ' criação:      frank   01/10/2003
            ' alteração:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case acção_canal

                Case datNaturezas.Db

                    DatPrepara(acção_select_ficha)
                    If DbConsulta() Then
                        Return (l_acção.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

                Case datNaturezas.Xml

                    DatPrepara(acção_select_ficha)
                    If XmlConsulta() Then
                        Return (l_acção.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

            End Select

        End Function

        Public Overridable Overloads Function DatConsultaRegisto() As Boolean Implements IDATLET.DatConsultaRegisto

            ' criação:      frank   01/10/2003
            ' alteração:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case acção_canal

                Case datNaturezas.Db

                    DatPrepara(acção_select_registo)
                    If DbConsulta() Then
                        Return (l_acção.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

                Case datNaturezas.Xml

                    DatPrepara(acção_select_registo)
                    If XmlConsulta() Then
                        Return (l_acção.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

            End Select

        End Function

        Public Overridable Overloads Function DatExiste() As Boolean Implements IDATLET.DatExiste

            ' criação:      frank   01/10/2003
            ' alteração:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case acção_canal

                Case datNaturezas.Db

                    DatPrepara(acção_select_procura)
                    b_select(l_acção.DbSintaxe, Nothing, True)
                    DatAtributo(l_acção.Pedido.Objecto.Ponteiro) = l_acção.ResultadoEscalar
                    Return (Not l_acção.ResultadoEscalar Is Nothing)

                Case datNaturezas.Xml

                    DatPrepara(acção_select_procura)
                    If XmlConsulta() Then
                        Return (l_acção.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

            End Select

        End Function

        Public Overridable Overloads Function DatSeleccionaLista() As IDATLET() Implements IDATLET.DatSeleccionaLista

            ' criação:      frank   01/10/2003
            ' alteração:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case acção_canal

                Case datNaturezas.Db

                    DatPrepara(acção_select_lista)
                    Return CType(DbSelecciona(), IDATLET())

                Case datNaturezas.Xml

                    DatPrepara(acção_select_lista)
                    Return CType(XmlSelecciona(), IDATLET())

            End Select

        End Function

        Public Overridable Overloads Function DatConsultaVista() As Boolean Implements IDATLET.DatConsultaVista

            ' criação:      frank   01/10/2003
            ' alteração:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case acção_canal

                Case datNaturezas.Db

                    DatPrepara(acção_select_vista)
                    If DbConsulta() Then
                        Return (l_acção.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

                Case datNaturezas.Xml

                    DatPrepara(acção_select_vista)
                    If XmlConsulta() Then
                        Return (l_acção.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

            End Select

        End Function

        Public Overridable Overloads Function DatPesquisa() As IDATLET() Implements IDATLET.DatPesquisa

            Select Case acção_canal

                Case datNaturezas.Db

                    DatPrepara(acção_select_pesquisa)
                    Return CType(DbSelecciona(), IDATLET())

                Case datNaturezas.Xml

                    DatPrepara(acção_select_pesquisa)
                    Return CType(XmlSelecciona(), IDATLET())

            End Select

        End Function

        Public Overridable Function DatSeleccionaFichas() As IDATLET() Implements IDATLET.DatSeleccionaFichas

            ' criação:      frank   01/10/2003
            ' alteração:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case acção_canal

                Case datNaturezas.Db

                    DatPrepara(acção_select_fichas)
                    Return CType(DbSelecciona(), IDATLET())

                Case datNaturezas.Xml

                    DatPrepara(acção_select_fichas)
                    Return CType(XmlSelecciona(), IDATLET())

            End Select

        End Function

        Public Overridable Function DatAutoId() As Boolean Implements IDATLET.DatAutoId

            ' 01/10/2003    frank       criação
            ' 11/12/2003    frank       de canal unico (DB) para multicanal (DB, XML)

            Select Case acção_canal

                Case datNaturezas.Db

                    DatPrepara(acção_select_proximo)
                    b_select(l_acção.DbSintaxe, Nothing, True)
                    If l_acção.ResultadoEscalar Is Nothing Then
                        If b_excepção Is Nothing Then
                            DatAtributo(l_acção.Pedido.Objecto.Ponteiro) = 10
                            DatAutoId = True
                        Else
                            DatAutoId = False
                        End If
                    Else
                        DatAtributo(l_acção.Pedido.Objecto.Ponteiro) = CInt(l_acção.ResultadoEscalar) + 10
                        DatAutoId = True
                    End If

                Case datNaturezas.Xml

                    Dim Lista() As IXMLLET

                    DatPrepara(acção_select_proximo)
                    Lista = XmlSelecciona()
                    If Lista Is Nothing Then
                        If x_excepção Is Nothing Then
                            DatAtributo(l_acção.Pedido.Objecto.Ponteiro) = 10
                            DatAutoId = True
                        Else
                            DatAutoId = False
                        End If
                    Else
                        DatAtributo(l_acção.Pedido.Objecto.Ponteiro) = CInt(Lista(Lista.GetUpperBound(0)).DatAtributo(l_info.Objecto.Ponteiro)) + 10
                        DatAutoId = False
                    End If

            End Select

        End Function

        Public Overridable Function DatAltera() As Boolean Implements IDATLET.DatAltera

            ' criação:      frank   01/10/2003
            ' alteração:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case acção_canal

                Case datNaturezas.Db

                    DatPrepara(acção_update_ficha)
                    If DbExecuta() Then
                        Return (l_acção.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

                Case datNaturezas.Xml

                    DatPrepara(acção_update_ficha)
                    If XmlEscreve() Then
                        Return (l_acção.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

            End Select

        End Function

        Public Overridable Function DatInsere() As Boolean Implements IDATLET.DatInsere

            ' criação:      frank   01/10/2003
            ' alteração:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case acção_canal

                Case datNaturezas.Db

                    DatPrepara(acção_insert_ficha)
                    If DbExecuta() Then
                        Return (l_acção.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

                Case datNaturezas.Xml

                    DatPrepara(acção_insert_ficha)
                    If XmlEscreve() Then
                        Return (l_acção.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

            End Select

        End Function

        Public Overridable Function DatElimina() As Boolean Implements IDATLET.DatElimina

            ' criação:      frank   01/10/2003
            ' alteração:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case acção_canal

                Case datNaturezas.Db

                    DatPrepara(acção_delete_ficha)
                    If DbExecuta() Then
                        Return (l_acção.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

                Case datNaturezas.Xml

                    DatPrepara(acção_delete_ficha)
                    If XmlElimina() Then
                        Return (l_acção.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

            End Select

        End Function

        Public Overridable Overloads Function DatConsultaFicha(ByVal ParamArray Chave() As Object) As Boolean Implements IDATLET.DatConsultaFicha

            ' criação:      frank   01/10/2003

            acção_parametros = Chave

            Return DatConsultaFicha()

        End Function

        Public Overridable Overloads Function DatConsultaRegisto(ByVal Ponteiro As Integer) As Boolean Implements IDATLET.DatConsultaRegisto

            ' criação:      frank   01/10/2003

            ReDim acção_parametros(0)
            acção_parametros(0) = Ponteiro

            Return DatConsultaRegisto()

        End Function

        Public Overridable Overloads Function DatExiste(ByVal ParamArray Chave() As Object) As Boolean Implements IDATLET.DatExiste

            ' criação:      frank   01/10/2003

            acção_parametros = Chave

            Return DatExiste()

        End Function

        Public Overridable Overloads Function DatConsultaVista(ByVal Ponteiro As Integer) As Boolean Implements IDATLET.DatConsultaVista

            ' criação:      frank   01/10/2003

            ReDim acção_parametros(0)
            acção_parametros(0) = Ponteiro

            Return DatConsultaVista()

        End Function

        Public Overridable Overloads Function DatPesquisa(ByVal ParamArray Descrição() As Object) As IDATLET() Implements IDATLET.DatPesquisa

            ' 22/12/2003    frank       criação:

            acção_parametros = Descrição

            Return DatPesquisa

        End Function

        Public Overridable Function DatActualiza() As Boolean Implements IDATLET.DatActualiza

            If CInt(DatAtributo(Me.l_info.Objecto.Ponteiro)) = 0 Then

                ' trata-se de um registo novo sem um ponteiro identificador

                If Not Me.DatAutoId Then

                    ' não foi possível a autonumeração
                    Return False

                End If

            End If

            If DatExiste() Then

                ' o registo já existe
                Return DatAltera()

            Else

                ' o registo não existe
                Return DatInsere()

            End If

        End Function

        Public Overridable Property DatFiltros() As datInscrição() Implements IDATLET.DatFiltros
            Get

                Return l_filtros

            End Get
            Set(ByVal Value() As datInscrição)

                l_filtros = Value
                ReDim l_acções(acção_count)

            End Set
        End Property


        Public Overridable Property DatExcepção() As System.Exception Implements IDATLET.DatExcepção
            Get

                Select Case acção_canal

                    Case datNaturezas.Db : Return b_excepção
                    Case datNaturezas.Xml : Return x_excepção

                End Select

            End Get
            Set(ByVal Value As System.Exception)

                Select Case acção_canal

                    Case datNaturezas.Db : b_excepção = Value
                    Case datNaturezas.Xml : x_excepção = Value

                End Select

            End Set
        End Property

        Public Overridable Function DatCast(ByVal Lista() As ILET) As ILET() Implements IDATLET.DatCast

            ' 01/10/2003    frank   efectua a conversão de uma lista de IDATLET para o tipo especifico da classe
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

            ' criação:      frank   01/10/2003

            DbReset()
            XmlReset()

            ' reinicialização da DATLET

            Me.DatInfo = l_info

            l_separador = " - "

        End Sub

        Public Overridable Sub DatDispose() Implements IDATLET.DatDispose

            ' criação:      frank   12/11/2003

            XmlDispose()
            DbDispose()

        End Sub

#End Region

        Shared Function CastTabela(ByVal Lista() As IDATLET, ByVal MinimoUmaLinha As Boolean, ByVal ParamArray Inscritos() As Integer) As System.Data.DataTable

            ' criação:      frank   12/11/2003

            Dim dt As New System.Data.DataTable
            Dim l As Integer
            Dim i As Integer
            Dim linha() As Object

            ' resumo:
            ' transforma uma lista de datlets numa tabela System.Data.DataTable compativel com ADO.NET
            ' caso a lista seja nula mas seja pedido pelo menos uma linha, uma instancia é adicionada à lista
            '   desta forma na tabela será colocada pelo menos uma linha

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

            ' criação:      frank   01/10/2003

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

            ' 01/10/2003    frank       cria uma cópia desta classe
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

            ' 01/10/2003    frank   efectua a conversão de uma lista de IDATLET para o tipo especifico da classe
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

            ' criação:      frank   12/11/2003

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

        ' 01/10/2003    frank       criação

        Protected b_excepção As System.Exception
        Protected b_conector As System.Data.IDbConnection
        Protected b_construtor As datConstrutorSQL
        Protected b_parametros() As Object

        Protected b_acções() As datAcção
        Protected b_acção As datAcção

        Public Erro As Boolean
        Public ErroAvisa As Boolean
        Public ErroResposta As MsgBoxResult




        Protected Overridable Sub b_defineacção(ByVal acção As Integer)

            ' definição da acção 

            b_acções(acção).Pedido = New datPedido
            b_acções(acção).Pedido.Objecto = Me.l_info.Objecto

            Select Case acção

                Case datAlcances.select_ficha
                    b_acções(acção).Pedido.Operação = datOperações.Consulta
                    b_acções(acção).Pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosTodos))
                    b_acções(acção).Pedido.MaisFiltros(datInscrição.Novas(l_info.Objecto.MembrosChave))
                    b_acções(acção).Pedido.Ordenações = Nothing

                Case datAlcances.select_registo
                    b_acções(acção).Pedido.Operação = datOperações.Consulta
                    b_acções(acção).Pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosTodos))
                    b_acções(acção).Pedido.MaisFiltros(datInscrição.Novas(l_info.Objecto.MembrosPonteiro))
                    b_acções(acção).Pedido.Ordenações = Nothing

                Case datAlcances.select_lista
                    b_acções(acção).Pedido.Operação = datOperações.Consulta
                    b_acções(acção).Pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosLista))
                    b_acções(acção).Pedido.Filtros = Nothing
                    b_acções(acção).Pedido.MaisOrdenações(datInscrição.Novas(l_info.Objecto.MembrosChave))

                Case datAlcances.select_fichas
                    b_acções(acção).Pedido.Operação = datOperações.Consulta
                    b_acções(acção).Pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosTodos))
                    b_acções(acção).Pedido.Filtros = Nothing
                    b_acções(acção).Pedido.MaisOrdenações(datInscrição.Novas(l_info.Objecto.MembrosChave))

                Case datAlcances.select_procura
                    b_acções(acção).Pedido.Operação = datOperações.Consulta
                    b_acções(acção).Pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosPonteiro))
                    b_acções(acção).Pedido.MaisFiltros(datInscrição.Novas(l_info.Objecto.MembrosChave))
                    b_acções(acção).Pedido.Ordenações = Nothing

                Case datAlcances.select_vista
                    b_acções(acção).Pedido.Operação = datOperações.Consulta
                    b_acções(acção).Pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosVista))
                    b_acções(acção).Pedido.MaisFiltros(datInscrição.Novas(l_info.Objecto.MembrosPonteiro))
                    b_acções(acção).Pedido.Ordenações = Nothing

                Case datAlcances.select_maximo
                    b_acções(acção).Pedido.Operação = datOperações.Consulta
                    b_acções(acção).Pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosPonteiro))
                    b_acções(acção).Pedido.Filtros = Nothing
                    b_acções(acção).Pedido.Ordenações = Nothing

                Case datAlcances.update_ficha
                    b_acções(acção).Pedido.Operação = datOperações.Alteração
                    b_acções(acção).Pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosTodos))
                    b_acções(acção).Pedido.MaisFiltros(datInscrição.Novas(l_info.Objecto.MembrosPonteiro))
                    b_acções(acção).Pedido.Ordenações = Nothing

                Case datAlcances.insert_ficha
                    b_acções(acção).Pedido.Operação = datOperações.Inserção
                    b_acções(acção).Pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosTodos))
                    b_acções(acção).Pedido.MaisFiltros(datInscrição.Novas(l_info.Objecto.MembrosTodos))
                    b_acções(acção).Pedido.Ordenações = Nothing

                Case datAlcances.delete_ficha
                    b_acções(acção).Pedido.Operação = datOperações.Eliminação
                    b_acções(acção).Pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosTodos))
                    b_acções(acção).Pedido.MaisFiltros(datInscrição.Novas(l_info.Objecto.MembrosPonteiro))
                    b_acções(acção).Pedido.Ordenações = Nothing

                Case datAlcances.select_primeiro
                    b_acções(acção).Pedido.Operação = datOperações.Consulta
                    b_acções(acção).Pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosTodos))
                    b_acções(acção).Pedido.Filtros = Nothing
                    b_acções(acção).Pedido.Ordenações = Nothing

                Case datAlcances.select_pesquisa
                    b_acções(acção).Pedido.Operação = datOperações.Consulta
                    b_acções(acção).Pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosPesquisa))
                    b_acções(acção).Pedido.MaisFiltros(datInscrição.Novas(l_info.Objecto.MembrosDescrição))
                    b_acções(acção).Pedido.MaisOrdenações(datInscrição.Novas(l_info.Objecto.MembrosChave))

            End Select

            b_acções(acção).Pedido.AutoCompleta()

        End Sub

        Protected Overridable Function b_exec(ByVal comandoSQL As String) As Boolean

            ' criação:      frank   01/10/2003

            Dim Comando As System.Data.IDbCommand
            Dim Cursor As System.Data.IDataReader

            Comando = b_conector.CreateCommand
            Comando.CommandType = CommandType.Text
            Comando.CommandText = comandoSQL

            b_parametriza(Comando)

            Do

                ErroResposta = MsgBoxResult.Ok
                b_excepção = Nothing
                Erro = False

                Try

                    b_acção.ResultadoAfectados = Comando.ExecuteNonQuery
                    Return True

                Catch execepção_ao_comando As System.Exception

                    Erro = True
                    b_excepção = execepção_ao_comando
                    b_acção.ResultadoAfectados = 0

                    ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcepção(b_excepção, Comando.CommandText, ErroAvisa, True, False)

                End Try

            Loop While ErroResposta = MsgBoxResult.Retry

            Return False

        End Function

        Protected Overridable Function b_select(ByVal comandoSQL As String, ByVal Cursor As System.Data.IDataReader, ByVal Escalar As Boolean) As Boolean

            ' criação:      frank   01/10/2003

            Dim Comando As System.Data.IDbCommand

            Comando = b_conector.CreateCommand
            Comando.CommandType = CommandType.Text
            Comando.CommandText = comandoSQL

            b_parametriza(Comando)

            Do

                Erro = False
                ErroResposta = MsgBoxResult.Ok
                b_excepção = Nothing

                Try

                    If Escalar Then
                        b_acção.ResultadoEscalar = Comando.ExecuteScalar
                    Else
                        Cursor = Comando.ExecuteReader
                    End If

                    If Cursor Is Nothing Then
                        Return False
                    Else
                        Return True
                    End If

                Catch execepção_ao_comando As System.Exception

                    Erro = True
                    b_excepção = execepção_ao_comando

                    ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcepção(b_excepção, Comando.CommandText, ErroAvisa, True, False)

                End Try

            Loop While ErroResposta = MsgBoxResult.Retry

            Return False

        End Function

        Protected Overridable Sub b_parametriza(ByVal comando As System.Data.IDbCommand)

            ' criação:      frank   01/10/2003
            ' alteração:    frank   22/12/2003      o conteudo do parametro pode ser tirado de uma 
            '                                       array de atributos paralelo

            Dim parametros() As System.Data.IDataParameter
            Dim f As Integer
            Dim c As Integer

            comando.Parameters.Clear()

            If Not b_acção.Filtros Is Nothing Then

                ReDim parametros(b_acção.Filtros.GetUpperBound(0))

                For f = 0 To b_acção.Filtros.GetUpperBound(0)

                    c = b_acção.Filtros(f)

                    parametros(f) = comando.CreateParameter
                    parametros(f).ParameterName = b_construtor.SintaxeDeParametroValor(b_acção.Pedido.Objecto.Membros(c))
                    parametros(f).DbType = b_acção.Pedido.Objecto.Membros(c).Tipo

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


        Public Event DbJoin(ByVal Agregador As datMembro, ByVal Cursor As System.Data.IDataReader, ByVal InscriçõesAceites() As datAceitação, ByRef JoinIndex As Integer) Implements IDBLET.DbJoin


        Public Overridable Sub DbReset() Implements IDBLET.DbReset

            ' criação:      frank       01/10/2003

            l_objectoreset()

            ReDim b_acções(datAlcances.count - 1)

        End Sub

        Public Overridable Property DbConector() As System.Data.IDbConnection Implements IDBLET.DbConector

            ' criação:      frank   01/10/2003

            Get

                DbConector = b_conector

            End Get
            Set(ByVal Value As System.Data.IDbConnection)

                b_conector = Value

            End Set
        End Property

        Public Overridable Property DbConstrutor() As datConstrutorSQL Implements IDBLET.DbConstrutor

            ' criação:      frank   01/10/2003

            Get

                DbConstrutor = b_construtor

            End Get
            Set(ByVal Value As datConstrutorSQL)

                b_construtor = Value

            End Set
        End Property

        Public Overridable Property DbExcepção() As System.Exception Implements IDBLET.DbExcepção
            Get

                DbExcepção = b_excepção

            End Get
            Set(ByVal Value As System.Exception)

                b_excepção = Value

            End Set
        End Property


        Public Overridable Function DbPrepara(ByVal Acção As Integer) As Boolean Implements IDBLET.DbPrepara

            ' criação:      frank   01/10/2003
            ' alteração:    frank   11/12/2003          adição de novo alcance "datAlcances.Select_primeiro"
            ' alteração:    frank   22/12/2003          adição de novo alcance "datAlcances.Select_pesquisa"

            If Not b_acções(Acção).Definida Then

                ' definição da acção (objecto, inscritos, filtros, ordenação)

                b_defineacção(Acção)
                b_acções(Acção).Definida = True

            End If

            If Not b_acções(Acção).Preparada Then

                ' construção do sintaxe (comando sql) 
                ' e obtenção da lista de aceites (campos presentes no comando)

                b_acções(Acção).Sintaxe = b_construtor.ConstroiSintaxe(b_acções(Acção).Pedido)
                b_acções(Acção).AssimilaPedido(b_acções(Acção).Pedido)
                b_acções(Acção).Preparada = True

            End If

            b_acção = b_acções(Acção)

            Return True

        End Function

        Public Overridable Function DbExecuta() As Boolean Implements IDBLET.DbExecuta

            ' criação:      frank   01/10/2003

            Return b_exec(b_acção.Sintaxe)

        End Function

        Public Overridable Overloads Function DbConsulta() As Boolean Implements IDBLET.DbConsulta

            Return DbConsulta(False)

        End Function

        Public Overridable Overloads Function DbConsulta(ByVal Escalar As Boolean) As Boolean Implements IDBLET.DbConsulta

            ' criação:      frank   01/10/2003

            Dim Cursor As System.Data.IDataReader

            DbConsulta = b_select(b_acção.Sintaxe, Cursor, Escalar)

            If DbConsulta Then

                If DbSaca(Cursor, True, b_acção.Pedido.Aceites, 0) Then b_acção.ResultadoAfectados = 1

                Cursor.Close()

            Else

                b_acção.ResultadoAfectados = 0

            End If

        End Function

        Public Overridable Function DbSelecciona() As IDBLET() Implements IDBLET.DbSelecciona

            ' criação:      frank   01/10/2003

            Dim cursor As System.Data.IDataReader
            Dim lista() As SpecDbLet
            Dim l As Integer
            Dim sucesso As Boolean

            b_acção.ResultadoAfectados = 0
            sucesso = b_select(b_acção.Sintaxe, cursor, False)

            If sucesso Then

                l = -1
                While cursor.Read

                    l += 1
                    ReDim Preserve lista(l)

                    lista(l) = CType(Me.DatClone, SpecDbLet)
                    lista(l).b_acção.Pedido.Objecto = l_info.Objecto
                    lista(l).DbSaca(cursor, False, b_acção.Pedido.Aceites, 0)

                End While

                cursor.Close()

                b_acção.ResultadoAfectados = l + 1
                Return lista

            Else

                Return Nothing

            End If

        End Function

        Public Overridable ReadOnly Property DbAfectados() As Integer Implements IDBLET.DbAfectados

            ' criação:      frank   01/10/2003

            Get

                DbAfectados = b_acção.ResultadoAfectados

            End Get
        End Property

        Public Overridable Function DbSaca(ByVal Cursor As System.Data.IDataReader, ByVal CursorFetch As Boolean) As Boolean Implements IDBLET.DbSaca

            ' criação:      frank   01/10/2003

            Return DbSaca(Cursor, CursorFetch, b_acção.Pedido.Aceites, 0)

        End Function

        Public Overridable Function DbSaca(ByVal Cursor As System.Data.IDataReader, ByVal CursorFetch As Boolean, ByVal InscriçõesAceites() As datAceitação, ByRef JoinIndex As Integer) As Boolean Implements IDBLET.DbSaca

            ' criação:      frank   01/10/2003

            Dim i As Integer
            Dim m As Integer
            Dim ji As Integer

            If CursorFetch Then
                DbSaca = Cursor.Read
            Else
                DbSaca = True
            End If

            If DbSaca Then

                If InscriçõesAceites Is Nothing Then

                    '                                                                                       

                    For m = 0 To Cursor.FieldCount - 1

                        If Cursor.IsDBNull(m) Then
                            MyBase.DatAtributo(m) = Nothing
                        Else
                            Select Case b_acção.Pedido.Objecto.Membros(m).Tipo

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

                    For i = 0 To InscriçõesAceites.GetUpperBound(0)

                        If InscriçõesAceites(i).JoinNivel = ji Then

                            m = InscriçõesAceites(i).Membro.Ordem

                            If Cursor.IsDBNull(i) Then
                                DatAtributo(m) = Nothing
                            Else
                                Select Case b_acção.Pedido.Objecto.Membros(m).Tipo

                                    Case DbType.Int32 : DatAtributo(m) = Cursor.GetInt32(i)
                                    Case DbType.Decimal : DatAtributo(m) = Cursor.GetDecimal(i)
                                    Case DbType.Date : DatAtributo(m) = Cursor.GetDateTime(i)
                                    Case DbType.Time : DatAtributo(m) = Cursor.GetDateTime(i)
                                    Case DbType.Boolean : DatAtributo(m) = Cursor.GetBoolean(i)
                                    Case Else : DatAtributo(m) = Cursor.GetString(i)

                                End Select
                            End If

                            If Not b_acção.Pedido.Objecto.Membros(m).Referencia Is Nothing Then

                                JoinIndex += 1
                                RaiseEvent DbJoin(b_acção.Pedido.Objecto.Membros(m), Cursor, b_acção.Pedido.Aceites, JoinIndex)

                            End If
                        End If

                    Next

                    '                                                                                       

                End If

            End If

        End Function

        Public Overridable Function DbLink(ByVal Objecto As datObjecto) As Integer() Implements IDBLET.DbLink

            ' criação:      frank   01/10/2003

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

            ' criação:      frank       12/11/2003

            l_info = Nothing
            l_atributos = Nothing

            b_excepção = Nothing
            b_conector = Nothing
            b_construtor = Nothing

            b_acção = Nothing
            b_acções = Nothing

        End Sub

        Public Overridable Property DbParametros() As Object() Implements IDBLET.DbParametros
            Get

                Return b_parametros

            End Get
            Set(ByVal Value() As Object)

                b_parametros = Value

            End Set
        End Property

        Public Property DbAcção() As datAcção Implements IDBLET.DbAcção
            Get

                Return b_acção

            End Get
            Set(ByVal Value As datAcção)

                b_acção = Value

            End Set
        End Property

    End Class

    Public Class SpecXmlLet
        Inherits SpecLet
        Implements IXMLLET

        ' todo: SpecXmlLet: vai usar o AdaptadorXML para aceder aos ficheiros XML)

        Protected x_ficheiro As String
        Protected x_nobase As System.Xml.XmlNode
        Protected x_excepção As System.Exception
        Protected x_eliminado As Boolean
        Protected x_parametros() As Object

        Protected x_acções() As datAcção
        Protected x_acção As datAcção

        Public Erro As Boolean
        Public ErroAvisa As Boolean
        Public ErroResposta As MsgBoxResult




        Protected Overridable Sub x_defxmlno(ByVal no_registo As System.Xml.XmlNode)

            ' criação:      frank   01/10/2003

            Dim a As Integer
            Dim c As Integer

            For a = 0 To x_acção.Pedido.Aceites.GetUpperBound(0)

                c = x_acção.Inscritos(a)
                no_registo.AppendChild(no_registo.OwnerDocument.CreateElement(l_info.Objecto.Membros(c).Nome))

            Next

        End Sub

        Protected Overridable Function x_select(ByVal no_datlet As System.Xml.XmlNode, ByVal filtros() As Integer) As System.Xml.XmlNode

            Dim no_registo As System.Xml.XmlNode
            Dim f As Integer
            Dim a As Integer
            Dim coincide As Boolean

            ' método parecido com x_selectvarios

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

                        ' a diferença com o método x_select é aqui:
                        ' este método quando encontra um nó retorna-o imediatamente sem continuara a procura

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

            ' método parecido com x_select 

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

                        ' a diferença com o método x_select é aqui:
                        ' este método quando encontra um nó adiciona-o a uma lista que retornará

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

            ' criação:      frank   01/10/2003

            l_objectoreset()

            ReDim x_acções(datAlcances.count - 1)

        End Sub

        Public Overridable Property XmlFicheiro() As String Implements IXMLLET.XmlFicheiro

            ' criação:      frank   01/10/2003

            Get

                Return x_ficheiro

            End Get
            Set(ByVal Value As String)

                x_ficheiro = Value

            End Set
        End Property

        Public Overridable Property XmlNoBase() As System.Xml.XmlNode Implements IXMLLET.XmlNoBase

            ' criação:      frank   01/10/2003

            Get

                Return Me.x_nobase

            End Get
            Set(ByVal Value As System.Xml.XmlNode)

                Me.x_nobase = Value

            End Set
        End Property

        Public Overridable Property XmlExcepção() As System.Exception Implements IXMLLET.XmlExcepção
            Get

                Return x_excepção

            End Get
            Set(ByVal Value As System.Exception)

                x_excepção = Value

            End Set
        End Property

        Public Overridable Function XmlPrepara(ByVal Acção As Integer) As Boolean Implements IXMLLET.XmlPrepara

            ' criação:      frank   01/10/2003
            ' alteração:    frank   11/12/2003      as acções xml passam a ser filtradas, 
            '                                       os registos são procurados tendo em consideração o conteudo
            '                                       de determinados atributos
            ' alteração:    frank   11/12/2003      novo alcance "datAlcances.Select_primeiro"
            ' 18/12/2003    frank   ao contrário das acções DB as acções XML já preparadas não são armazenadas
            '                       cada vez que se prepara uma acção XML todos os passos de preparação são efectuados

            If Not x_acções(Acção).Definida Then

                x_acções(Acção).Definida = True

            End If

            If Not x_acções(Acção).Preparada Then

                x_acções(Acção).AssimilaPedido(x_acções(Acção).Pedido)
                x_acções(Acção).Preparada = True

            End If

            x_acção = x_acções(Acção)

            Return True

        End Function

        Public Overridable Function XmlConsulta() As Boolean Implements IXMLLET.XmlConsulta

            ' criação:      frank   01/10/2003
            ' alteração:    frank   11/12/2003          dos nós existentes procura aquele cujo ponteiro seja igual
            '                                           ao conteudo do atributo ponteiro da classe
            '                                           se este valor for nulo "0" lé o primeiro nó disponível
            ' alteração:    frank   11/12/2003          dos nós existentes procura aquele cujo conteudos dos Membros
            '                                           filtrados seja igual aos dos atributos da classe

            Dim no_datlet As System.Xml.XmlNode
            Dim no_registo As System.Xml.XmlNode

            Do

                Erro = False
                x_excepção = Nothing
                no_datlet = XMLGetNo(x_nobase, x_ficheiro, l_info.Objecto.DbGrupo, False, Nothing, x_excepção)

                If Not x_excepção Is Nothing Then

                    Erro = True

                    ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcepção(x_excepção, x_ficheiro, ErroAvisa, True, False)

                End If

            Loop While ErroResposta = MsgBoxResult.Retry

            If no_datlet Is Nothing Then

                x_acção.ResultadoAfectados = 0
                Return False

            Else

                If no_datlet.FirstChild Is Nothing Then

                    x_acção.ResultadoAfectados = 0

                Else

                    If x_acção.Filtros Is Nothing Then
                        no_registo = no_datlet.FirstChild
                    Else
                        no_registo = x_select(no_datlet, x_acção.Filtros)
                    End If

                    If Not no_registo Is Nothing Then

                        XmlSaca(no_registo, x_acção.Inscritos)
                        x_acção.ResultadoAfectados = 1

                    Else

                        x_acção.ResultadoAfectados = 0

                    End If

                End If

                Return True

            End If

        End Function

        Public Overridable Function XmlSelecciona() As IXMLLET() Implements IXMLLET.XmlSelecciona

            ' criação:      frank   01/10/2003
            ' alteração:    frank   11/12/2003          dos nós existentes procura aqueles cujo conteudos dos Membros
            '                                           filtrados seja igual aos dos atributos da classe

            Dim no_datlet As System.Xml.XmlNode
            Dim no_registos() As System.Xml.XmlNode
            Dim lista() As SpecXmlLet
            Dim l As Integer

            Do

                Erro = False
                x_excepção = Nothing
                no_datlet = XMLGetNo(x_nobase, x_ficheiro, l_info.Objecto.DbGrupo, False, Nothing, x_excepção)

                If Not x_excepção Is Nothing Then

                    Erro = True

                    ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcepção(x_excepção, x_ficheiro, ErroAvisa, True, False)

                End If

            Loop While ErroResposta = MsgBoxResult.Retry

            If no_datlet Is Nothing Then

                x_acção.ResultadoAfectados = 0
                Return Nothing

            Else

                no_registos = x_selectvarios(no_datlet, x_acção.Filtros)

                ReDim lista(no_registos.GetUpperBound(0))

                For l = 0 To lista.GetUpperBound(0)

                    lista(l) = CType(Me.DatClone, SpecXmlLet)
                    lista(l).XmlSaca(no_registos(l), x_acção.Inscritos)

                Next

                x_acção.ResultadoAfectados = l + 1
                Return lista

            End If

        End Function

        Public Overridable Overloads Function XmlEscreve() As Boolean Implements IXMLLET.XmlEscreve

            ' criação:      frank   01/10/2003
            ' alteração:    frank   11/12/2003      antes de gravar procura o nó corrospondente e modifica-o
            ' alteração:    frank   11/12/2003      verifica se o nó foi marcado para eliminação

            Dim xmldoc As System.Xml.XmlDocument
            Dim no_datlet As System.Xml.XmlNode
            Dim no_registo As System.Xml.XmlNode

            Dim n As Integer

            Do

                Erro = False
                x_excepção = Nothing
                no_datlet = XMLGetNo(x_nobase, x_ficheiro, l_info.Objecto.DbGrupo, False, xmldoc, x_excepção)

                If Not x_excepção Is Nothing Then

                    Erro = True

                    ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcepção(x_excepção, x_ficheiro, ErroAvisa, True, False)

                End If

            Loop While ErroResposta = MsgBoxResult.Retry

            If no_datlet Is Nothing Then

                x_acção.ResultadoAfectados = 0
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
                    XmlEnche(no_registo, x_acção.Inscritos)
                End If

                If Not xmldoc Is Nothing Then xmldoc.Save(XmlFicheiro)

                x_acção.ResultadoAfectados = 1
                Return True

            End If

        End Function

        Public Overridable Overloads Function XmlEscreve(ByVal Lista() As IXMLLET) As Boolean Implements IXMLLET.XmlEscreve

            ' criação:      frank   01/10/2003
            ' alteração.    frank   11/12/2003      uma a uma evoca o evento XMLEscreve de cada classe da lista

            Dim xmldoc As System.Xml.XmlDocument
            Dim no_datlet As System.Xml.XmlNode

            Dim l As Integer

            Do

                Erro = False
                x_excepção = Nothing
                no_datlet = XMLGetNo(x_nobase, x_ficheiro, l_info.Objecto.DbGrupo, False, xmldoc, x_excepção)

                If Not x_excepção Is Nothing Then

                    Erro = True

                    ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcepção(x_excepção, x_ficheiro, ErroAvisa, True, False)

                End If

            Loop While ErroResposta = MsgBoxResult.Retry

            If no_datlet Is Nothing Then

                x_acção.ResultadoAfectados = 0
                Return False

            Else

                For l = 0 To Lista.GetUpperBound(0)

                    Lista(l).XmlNoBase = no_datlet
                    Lista(l).XmlEscreve()
                    x_acção.ResultadoAfectados = l + Lista(l).XmlAfectados

                Next

                If Not xmldoc Is Nothing Then xmldoc.Save(XmlFicheiro)

                Return True

            End If

        End Function

        Public Overridable Function XmlElimina() As Boolean Implements IXMLLET.XmlElimina

            ' criação:      frank   11/12/2003

            Dim xmldoc As System.Xml.XmlDocument
            Dim no_datlet As System.Xml.XmlNode
            Dim no_registo As System.Xml.XmlNode

            Dim n As Integer

            x_eliminado = True
            x_excepção = Nothing

            If Not x_nobase Is Nothing Or x_ficheiro.Length > 0 Then

                Do

                    Erro = False
                    x_excepção = Nothing
                    no_datlet = XMLGetNo(x_nobase, x_ficheiro, l_info.Objecto.DbGrupo, False, xmldoc, x_excepção)

                    If Not x_excepção Is Nothing Then

                        Erro = True

                        ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcepção(x_excepção, x_ficheiro, ErroAvisa, True, False)

                    End If

                Loop While ErroResposta = MsgBoxResult.Retry

                If no_datlet Is Nothing Then

                    x_acção.ResultadoAfectados = 0
                    Return False

                Else

                    no_registo = x_select(no_datlet, l_info.Objecto.ArrayPonteiro)

                    If no_registo Is Nothing Then

                        x_acção.ResultadoAfectados = 0

                    Else

                        no_datlet.RemoveChild(no_registo)

                        If Not xmldoc Is Nothing Then xmldoc.Save(XmlFicheiro)

                        x_acção.ResultadoAfectados = 1

                    End If

                    Return True

                End If

            Else

                Return True

            End If

        End Function

        Public Overridable ReadOnly Property XmlAfectados() As Integer Implements IXMLLET.XmlAfectados
            Get

                Return x_acção.ResultadoAfectados

            End Get
        End Property

        Public Overridable Function XmlSaca(ByVal No As System.Xml.XmlNode, ByVal InscriçõesAceites() As Integer) As Boolean Implements IXMLLET.XMLSaca

            ' criação:      frank   01/10/2003
            ' alteração:    frank   10/11/2003      passamos a aceder ao nó pelo seu nome
            '                                       a vantagem de aceder ao nó pelo seu nome é de que a ordem 
            '                                       destes no XML pode ser alterada sem prejuizo para as funções, 
            '                                       mais: sem uma ordem fixa os nós podem até não estar presentes no XML

            Dim a As Integer
            Dim c As Integer

            For a = 0 To InscriçõesAceites.GetUpperBound(0)

                c = InscriçõesAceites(a)
                'DatAtributo(c) = No.ChildNodes(a).InnerText
                DatAtributo(c) = No.Item(x_acção.Pedido.Objecto.Membros(c).Nome).InnerText

            Next

            x_eliminado = False

            Return True

        End Function

        Public Overridable Function XmlEnche(ByVal No As System.Xml.XmlNode, ByVal InscriçõesAceites() As Integer) As Boolean Implements IXMLLET.XMLEnche

            ' criação:      frank   01/10/2003
            ' alteração:    frank   10/11/2003      passamos a aceder ao nó pelo seu nome
            '                                       a vantagem de aceder ao nó pelo seu nome é de que a ordem 
            '                                       destes no XML pode ser alterada sem prejuizo para as funções, 
            '                                       mais, sem uma ordem fixa os nós podem até não estar presentes no XML

            Dim a As Integer
            Dim c As Integer

            For a = 0 To InscriçõesAceites.GetUpperBound(0)

                c = InscriçõesAceites(a)
                'No.ChildNodes(a).InnerText = CType(DatAtributo(c), String)
                No.Item(x_acção.Pedido.Objecto.Membros(c).Nome).InnerText = CType(DatAtributo(c), String)

            Next

        End Function

        Public Overridable Sub XmlDispose() Implements IXMLLET.XmlDispose

            ' criação:      frank   12/11/2003

            l_info = Nothing
            l_atributos = Nothing

            x_excepção = Nothing
            x_eliminado = False
            x_acções = Nothing
            x_acção = Nothing
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

        Public Property XmlAcção() As datAcção Implements IXMLLET.XmlAcção
            Get

                Return x_acção

            End Get
            Set(ByVal Value As datAcção)

                x_acção = Value

            End Set
        End Property

    End Class

    Public Class SpecDatlet
        Inherits SpecLet
        Implements IDATLET

        Protected d_acção_canal As datNaturezas

        Protected d_canalDB As IDBLET
        Protected d_canalXML As IXMLLET



        Public Overridable Property DatCanal() As DatCanal Implements IDATLET.DatCanal
            Get
                ' não expoe esta propriedade porque a SpecDatlet orienta-se pela Natureza
            End Get
            Set(ByVal Value As DatCanal)

            End Set
        End Property

        Public Overridable Property DatNatureza() As datNaturezas
            Get

                Return d_acção_canal

            End Get
            Set(ByVal Value As datNaturezas)

                d_acção_canal = Value

            End Set
        End Property

        Public Overridable Property DatParametros() As Object() Implements IDATLET.DatParametros
            Get

                Select Case d_acção_canal
                    Case datNaturezas.Db : Return d_canalDB.DbParametros
                    Case datNaturezas.Xml : Return d_canalXML.XmlParametros
                End Select

            End Get
            Set(ByVal Value() As Object)

                Select Case d_acção_canal
                    Case datNaturezas.Db : d_canalDB.DbParametros = Value
                    Case datNaturezas.Xml : d_canalXML.XmlParametros = Value
                End Select

            End Set
        End Property

        Public Overridable Property DatAcção() As DatAcção Implements IDATLET.DatAcção
            Get

                Select Case d_acção_canal
                    Case datNaturezas.Db : Return d_canalDB.DbAcção
                    Case datNaturezas.Xml : Return d_canalXML.XmlAcção
                End Select

            End Get
            Set(ByVal Value As datAcção)

                Select Case d_acção_canal
                    Case datNaturezas.Db : d_canalDB.DbAcção = Value
                    Case datNaturezas.Xml : d_canalXML.XmlAcção = Value
                End Select

            End Set
        End Property


        Public Overridable Sub DatPrepara(ByVal Alcance As Integer) Implements IDATLET.DatPrepara

            Select Case d_acção_canal

                Case datNaturezas.Db
                    d_canalDB.DbPrepara(datAlcances.select_primeiro)

                Case datNaturezas.Xml
                    d_canalXML.XmlPrepara(datAlcances.select_primeiro)

            End Select

        End Sub

        Public Overridable Function DatConsultaPrimeiro() As Boolean Implements IDATLET.DatConsultaPrimeiro

            ' criação:      frank   01/10/2003

            Select Case d_acção_canal

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

            ' criação:      frank   01/10/2003
            ' alteração:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case d_acção_canal

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

            ' criação:      frank   01/10/2003
            ' alteração:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case d_acção_canal

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

            ' criação:      frank   01/10/2003
            ' alteração:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case d_acção_canal

                Case datNaturezas.Db

                    DatPrepara(datAlcances.select_procura)
                    d_canalDB.DbConsulta(True)
                    DatAtributo(d_canalDB.DbAcção.Pedido.Objecto.Ponteiro) = d_canalDB.DbAcção.ResultadoEscalar
                    Return (Not d_canalDB.DbAcção.ResultadoEscalar Is Nothing)

                Case datNaturezas.Xml

                    DatPrepara(datAlcances.select_procura)
                    If d_canalXML.XmlConsulta() Then
                        Return (d_canalXML.XmlAcção.ResultadoAfectados > 0)
                    Else
                        Return False
                    End If

            End Select

        End Function

        Public Overridable Overloads Function DatSeleccionaLista() As IDATLET() Implements IDATLET.DatSeleccionaLista

            ' criação:      frank   01/10/2003
            ' alteração:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case d_acção_canal

                Case datNaturezas.Db

                    DatPrepara(datAlcances.select_lista)
                    Return CType(d_canalDB.DbSelecciona(), IDATLET())

                Case datNaturezas.Xml

                    DatPrepara(datAlcances.select_lista)
                    Return CType(d_canalXML.XmlSelecciona(), IDATLET())

            End Select

        End Function

        Public Overridable Overloads Function DatConsultaVista() As Boolean Implements IDATLET.DatConsultaVista

            ' criação:      frank   01/10/2003
            ' alteração:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case d_acção_canal

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

            Select Case d_acção_canal

                Case datNaturezas.Db

                    DatPrepara(datAlcances.select_pesquisa)
                    Return CType(d_canalDB.DbSelecciona(), IDATLET())

                Case datNaturezas.Xml

                    DatPrepara(datAlcances.select_pesquisa)
                    Return CType(d_canalXML.XmlSelecciona(), IDATLET())

            End Select

        End Function

        Public Overridable Function DatSeleccionaFichas() As IDATLET() Implements IDATLET.DatSeleccionaFichas

            ' criação:      frank   01/10/2003
            ' alteração:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case d_acção_canal

                Case datNaturezas.Db

                    DatPrepara(datAlcances.select_fichas)
                    Return CType(d_canalDB.DbSelecciona(), IDATLET())

                Case datNaturezas.Xml

                    DatPrepara(datAlcances.select_fichas)
                    Return CType(d_canalXML.XmlSelecciona(), IDATLET())

            End Select

        End Function

        Public Overridable Function DatAltera() As Boolean Implements IDATLET.DatAltera

            ' criação:      frank   01/10/2003
            ' alteração:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case d_acção_canal

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

            ' criação:      frank   01/10/2003
            ' alteração:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case d_acção_canal

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

            ' criação:      frank   01/10/2003
            ' alteração:    frank   05/12/2003      de canal unico (DB) para multicanal (DB, XML)

            Select Case d_acção_canal

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

            ' criação:      frank   01/10/2003

            Select Case d_acção_canal
                Case datNaturezas.Db : d_canalDB.DbParametros = Chave
                Case datNaturezas.Xml : d_canalXML.XmlParametros = Chave
            End Select

            Return DatConsultaFicha()

        End Function

        Public Overridable Overloads Function DatConsultaRegisto(ByVal Ponteiro As Integer) As Boolean Implements IDATLET.DatConsultaRegisto

            ' criação:      frank   01/10/2003

            Dim pars(0) As Object
            pars(0) = Ponteiro

            Select Case d_acção_canal
                Case datNaturezas.Db : d_canalDB.DbParametros = pars
                Case datNaturezas.Xml : d_canalXML.XmlParametros = pars
            End Select

            Return DatConsultaRegisto()

        End Function

        Public Overridable Overloads Function DatExiste(ByVal ParamArray Chave() As Object) As Boolean Implements IDATLET.DatExiste

            ' criação:      frank   01/10/2003

            Select Case d_acção_canal
                Case datNaturezas.Db : d_canalDB.DbParametros = Chave
                Case datNaturezas.Xml : d_canalXML.XmlParametros = Chave
            End Select

            Return DatExiste()

        End Function

        Public Overridable Overloads Function DatConsultaVista(ByVal Ponteiro As Integer) As Boolean Implements IDATLET.DatConsultaVista

            ' criação:      frank   01/10/2003

            Dim pars(0) As Object
            pars(0) = Ponteiro

            Select Case d_acção_canal
                Case datNaturezas.Db : d_canalDB.DbParametros = pars
                Case datNaturezas.Xml : d_canalXML.XmlParametros = pars
            End Select

            Return DatConsultaVista()

        End Function

        Public Overridable Overloads Function DatPesquisa(ByVal ParamArray Descrição() As Object) As IDATLET() Implements IDATLET.DatPesquisa

            ' 22/12/2003    frank       criação:

            Select Case d_acção_canal
                Case datNaturezas.Db : d_canalDB.DbParametros = Descrição
                Case datNaturezas.Xml : d_canalXML.XmlParametros = Descrição
            End Select

            Return DatPesquisa

        End Function

        Public Overridable Function DatActualiza() As Boolean Implements IDATLET.DatActualiza

            If CInt(DatAtributo(Me.l_info.Objecto.Ponteiro)) = 0 Then

                ' trata-se de um registo novo sem um ponteiro identificador

                If Not Me.DatAutoId Then

                    ' não foi possível a autonumeração
                    Return False

                End If

            End If

            If DatExiste() Then

                ' o registo já existe
                Return DatAltera()

            Else

                ' o registo não existe
                Return DatInsere()

            End If

        End Function

        Public Overridable Property DatFiltros() As datInscrição() Implements IDATLET.DatFiltros
            Get
            End Get
            Set(ByVal Value() As datInscrição)
            End Set
        End Property


        Public Overridable Function DatAutoId() As Boolean Implements IDATLET.DatAutoId

            ' 01/10/2003    frank       criação
            ' 11/12/2003    frank       de canal unico (DB) para multicanal (DB, XML)

            Select Case d_acção_canal
                Case datNaturezas.Db

                    DatPrepara(datAlcances.select_maximo)
                    d_canalDB.DbConsulta(True)
                    If d_canalDB.DbAcção.ResultadoEscalar Is Nothing Then
                        If d_canalDB.DbExcepção Is Nothing Then
                            DatAtributo(d_canalDB.DbAcção.Pedido.Objecto.Ponteiro) = 10
                            DatAutoId = True
                        Else
                            DatAutoId = False
                        End If
                    Else
                        DatAtributo(d_canalDB.DbAcção.Pedido.Objecto.Ponteiro) = CInt(d_canalDB.DbAcção.ResultadoEscalar) + 10
                        DatAutoId = True
                    End If

                Case datNaturezas.Xml

                    Dim Lista() As IXMLLET

                    DatPrepara(datAlcances.select_maximo)
                    Lista = d_canalXML.XmlSelecciona()
                    If Lista Is Nothing Then
                        If d_canalXML.XmlExcepção Is Nothing Then
                            DatAtributo(d_canalDB.DbAcção.Pedido.Objecto.Ponteiro) = 10
                            DatAutoId = True
                        Else
                            DatAutoId = False
                        End If
                    Else
                        DatAtributo(d_canalDB.DbAcção.Pedido.Objecto.Ponteiro) = CInt(Lista(Lista.GetUpperBound(0)).DatAtributo(d_canalDB.DbAcção.Pedido.Objecto.Ponteiro)) + 10
                        DatAutoId = False
                    End If

            End Select

        End Function

        Public Overridable Property DatExcepção() As System.Exception Implements IDATLET.DatExcepção
            Get

                Select Case d_acção_canal

                    Case datNaturezas.Db : Return d_canalDB.DbExcepção
                    Case datNaturezas.Xml : Return d_canalXML.XmlExcepção

                End Select

            End Get
            Set(ByVal Value As System.Exception)

                Select Case d_acção_canal

                    Case datNaturezas.Db : d_canalDB.DbExcepção = Value
                    Case datNaturezas.Xml : d_canalXML.XmlExcepção = Value

                End Select

            End Set
        End Property

        Public Overridable Sub DatReset() Implements IDATLET.DatReset

            ' criação:      frank   01/10/2003

            Me.DatInfo = l_info
            d_canalDB.DbReset()
            d_canalXML.XmlReset()

        End Sub

        Public Overridable Sub DatDispose() Implements IDATLET.DatDispose

                ' criação:      frank   12/11/2003

                d_canalXML.XmlDispose()
                d_canalDB.DbDispose()

        End Sub

        Public Overloads Function DatVerifica() As Boolean Implements IDATLET.DatVerifica

        End Function

        Public Overloads Function DatVerifica1(ByVal Ponteiro As Object) As Boolean Implements IDATLET.DatVerifica

        End Function
    End Class

End Namespace

