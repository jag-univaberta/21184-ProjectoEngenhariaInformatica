Namespace Dataframe

    Public Class datConstrutorSQL
        Implements ICONSTRUTOR

        Private Shared ReadOnly _locker As New Object()

        Public Function ConstroiSintaxe(ByVal Pedido As datPedido) As String Implements ICONSTRUTOR.ConstroiSintaxe
            SyncLock _locker
                Dim frase As String

                Pedido.ResolveComAlias(True)
                Pedido.Aceites = datAceita��o.Novas(Pedido)

                Select Case Pedido.Opera��o

                    Case datOpera��es.Consulta
                        frase = Me.SintaxeDeConsulta(Pedido) & vbCrLf & SintaxeDeFiltro(Pedido) & vbCrLf & SintaxeDeOrdena��o(Pedido)

                    Case datOpera��es.Inser��o
                        frase = Me.SintaxeDeInser��o(Pedido)

                    Case datOpera��es.Altera��o
                        frase = Me.SintaxeDeAltera��o(Pedido) & vbCrLf & SintaxeDeFiltro(Pedido)

                    Case datOpera��es.Elimina��o
                        frase = Me.SintaxeDeElimina��o(Pedido) & vbCrLf & SintaxeDeFiltro(Pedido)

                End Select

                Return frase
            End SyncLock
        End Function

        Public Function SintaxeDeConsulta(ByVal Pedido As datPedido) As String Implements ICONSTRUTOR.SintaxeDeConsulta
            SyncLock _locker
                Dim selec��o As String
                Dim subpedidos As String
                Dim com As String

                selec��o = MembrosAceites(Pedido)
                subpedidos = TabelasLigadas(Pedido)

                com = "SELECT " & selec��o & vbCrLf & "FROM " & Pedido.Objecto.DbAlias & " " & Pedido.ComAlias

                Select Case Pedido.Lock

                    Case datLocks.Generico : com = com & " WITH (TABLOCK, HOLDLOCK) "
                    Case datLocks.Exclusivo : com = com & " WITH (TABLOCKX, HOLDLOCK) "

                End Select

                com = com & " " & vbCrLf & subpedidos

                Return com
            End SyncLock
        End Function

        Public Function SintaxeDeInser��o(ByVal Pedido As datPedido) As String Implements ICONSTRUTOR.SintaxeDeInser��o
            SyncLock _locker
                Dim i As Integer
                Dim afectados As String
                Dim conteudos As String

                For i = Pedido.Inscritos.GetLowerBound(0) To Pedido.Inscritos.GetUpperBound(0)

                    If Not afectados Is Nothing Then afectados = afectados & ", "
                    If Not conteudos Is Nothing Then conteudos = conteudos & ", "

                    afectados = afectados & Pedido.Inscritos(i).Membro.DbAlias
                    conteudos = conteudos & Me.SintaxeDeParametroValor(Pedido.Inscritos(i).Membro)

                Next

                Return "INSERT INTO " & Pedido.Objecto.DbAlias & "(" & afectados & ") VALUES (" & conteudos & ")"
            End SyncLock
        End Function

        Public Function SintaxeDeAltera��o(ByVal Pedido As datPedido) As String Implements ICONSTRUTOR.SintaxeDeAltera��o
            SyncLock _locker
                Dim i As Integer
                Dim conteudos As String

                For i = Pedido.Inscritos.GetLowerBound(0) To Pedido.Inscritos.GetUpperBound(0)

                    If Not conteudos Is Nothing Then conteudos = conteudos & ", "
                    conteudos = conteudos & Pedido.Inscritos(i).Membro.DbAlias & "=" & Me.SintaxeDeParametroValor(Pedido.Inscritos(i).Membro)

                Next

                Return "UPDATE " & Pedido.Objecto.DbAlias & " SET " & conteudos
            End SyncLock
        End Function

        Public Function SintaxeDeElimina��o(ByVal Pedido As datPedido) As String Implements ICONSTRUTOR.SintaxeDeElimina��o
            SyncLock _locker
                Return "DELETE FROM " & Pedido.Objecto.DbAlias
            End SyncLock
        End Function

        Public Function SintaxeDeParametroValor(ByVal Membro As datMembro) As String Implements ICONSTRUTOR.SintaxeDeParametroValor
            SyncLock _locker
                Return "@set_" & Membro.Nome
            End SyncLock
        End Function

        Public Function SintaxeDeParametroFiltro(ByVal Membro As datMembro) As String Implements ICONSTRUTOR.SintaxeDeParametroFiltro
            SyncLock _locker
                Return "@filtro_" & Membro.Nome
            End SyncLock
        End Function



        Public Function SintaxeDeFiltro(ByVal Pedido As datPedido) As String Implements ICONSTRUTOR.SintaxeDeFiltro
            SyncLock _locker
                If Pedido.Filtros Is Nothing Then

                    Return ""

                Else

                    Return "WHERE (" & SintaxeDeCondi��o(Pedido.Filtros, datPontes.N�oDefinido) & ")"

                End If
            End SyncLock
        End Function

        Public Function SintaxeDeOrdena��o(ByVal Pedido As datPedido) As String Implements ICONSTRUTOR.SintaxeDeOrdena��o
            SyncLock _locker
                Dim s As String

                s = MembrosAOrdenar(Pedido)

                If s.Length = 0 Then

                    Return ""

                Else

                    Return "ORDER BY " & s

                End If
            End SyncLock
        End Function

        Public Function SintaxeDeInscri��o(ByVal Inscrito As datInscri��o) As String Implements ICONSTRUTOR.SintaxeDeInscri��o
            SyncLock _locker
                If Inscrito.FAgregada.Fun��o = datFAgregadas.N�oDefinida Then

                    Return SintaxeDeMembro(Inscrito)

                Else

                    Return SintaxeDeFAgreagada(Inscrito)

                End If
            End SyncLock
        End Function

        Public Function SintaxeDeCondi��o(ByVal Inscritos() As datInscri��o, ByVal For�aPonte As datPontes) As String Implements ICONSTRUTOR.SintaxeDeCondi��o
            SyncLock _locker
                Dim i As Integer
                Dim frase As String
                Dim m1 As String
                Dim m2 As String

                For i = 0 To Inscritos.GetUpperBound(0)

                    If For�aPonte <> datPontes.N�oDefinido And Inscritos(i).Ponte = datPontes.N�oDefinido Then
                        frase = frase & " " & SintaxeDePonte(For�aPonte)
                    End If
                    frase = frase & SintaxeDePonte(Inscritos(i).Ponte)

                    If Inscritos(i).NegaAntes Then frase = frase & " NOT "

                    If Inscritos(i).PriorSobe > 0 Then frase = frase & g10phPDF4.Basframe.Strings.Repete("(", Inscritos(i).PriorSobe)

                    m1 = SintaxeDeInscri��o(Inscritos(i))

                    If Inscritos(i).Parametro Is Nothing Then

                        m2 = Me.SintaxeDeParametroFiltro(Inscritos(i).Membro)

                    Else

                        If TypeOf Inscritos(i).Parametro Is datInscri��o Then

                            m2 = SintaxeDeMembro(CType(Inscritos(i).Parametro, datInscri��o))

                        Else

                            If SystemDbTypeAlfa(Inscritos(i).Membro.Tipo) Then
                                m2 = CType("'" & Inscritos(i).Parametro.ToString & "'", String)
                            Else
                                m2 = CType(Inscritos(i).Parametro, String)
                            End If

                        End If

                    End If

                    frase = frase & m1 & SintaxeDeOperador(Inscritos(i).Operador) & m2

                    If Inscritos(i).PriorDesce > 0 Then frase = frase & g10phPDF4.Basframe.Strings.Repete(")", Inscritos(i).PriorDesce)

                Next

                Return frase
            End SyncLock
        End Function

        Public Function SintaxeDeFAgreagada(ByVal Inscrito As datInscri��o) As String Implements ICONSTRUTOR.SintaxeDeFAgregada
            SyncLock _locker
                Dim i As Integer
                Dim p() As String
                Dim frase As String

                ' o primeiro parametro deve ser sempre o nome do membro

                If Inscrito.FAgregada.Parametros Is Nothing Then

                    ' n�o havendo parametros � acrescentado o nome do membro (onde tem que estar)
                    ReDim p(0)
                    p(0) = Inscrito.Pedido.ComAlias & "." & Inscrito.Membro.DbAlias

                Else

                    ReDim p(Inscrito.FAgregada.Parametros.GetUpperBound(0))

                    For i = 0 To p.GetUpperBound(0)

                        If TypeOf Inscrito.FAgregada.Parametros(i) Is datInscri��o Then

                            p(i) = SintaxeDeInscri��o(CType(Inscrito.FAgregada.Parametros(i), datInscri��o))

                        Else

                            p(i) = CType(Inscrito.FAgregada.Parametros(i), String)

                        End If

                    Next

                    ' se o primeiro parametro n�o foi indicado � automaticamente o nome do membro
                    If p(0) Is Nothing Then p(0) = Inscrito.Pedido.ComAlias & "." & Inscrito.Membro.DbAlias

                End If

                '  os parametros s�o usados consoante a fun��o indicada

                Select Case Inscrito.FAgregada.Fun��o

                    Case datFAgregadas.Maximo : Return "MAX(" & p(0) & ")"
                    Case datFAgregadas.Minimo : Return "MIN(" & p(0) & ")"
                    Case datFAgregadas.Media : Return "AVG(" & p(0) & ")"
                    Case datFAgregadas.Somatorio : Return "SUM(" & p(0) & ")"
                    Case datFAgregadas.Conta : Return "COUNT(" & p(0) & ")"
                    Case datFAgregadas.Maiusc : Return "UPPER(" & p(0) & ")"
                    Case datFAgregadas.Minusc : Return "LOWER(" & p(0) & ")"
                    Case datFAgregadas.Converte

                        Select Case CType(p(1), DbType)
                            Case DbType.String : frase = "varchar(" & p(2) & "," & p(3) & ")"
                            Case DbType.AnsiStringFixedLength : frase = "char(" & p(2) & "," & p(3) & ")"
                            Case DbType.Decimal : frase = "decimal(" & p(2) & "," & p(3) & ")"
                            Case DbType.Int32 : frase = "int"
                            Case DbType.Boolean : frase = "bit"
                            Case DbType.Date : frase = "datetime"
                            Case Else
                                g10phPDF4.Basframe.ErroLogger.DeuExcep��o(New Excep��oConstru��oSQLTipoDestinoN�oImplementado, CType(p(1), DbType).ToString, True, False, False)
                        End Select

                        Return "CONVERT(" & frase & "," & p(0) & ")"

                    Case datFAgregadas.Top

                        Return "TOP " & p(1) & " " & p(0)

                    Case Else

                        g10phPDF4.Basframe.ErroLogger.DeuExcep��o(New Excep��oConstru��oSQLFAgregadoN�oImplementado, Inscrito.FAgregada.Fun��o.ToString, True, False, False)

                End Select

                Return frase
            End SyncLock
        End Function

        Public Function SintaxeDeMembro(ByVal Inscrito As datInscri��o) As String Implements ICONSTRUTOR.SintaxeDeMembro
            SyncLock _locker
                Return Inscrito.Pedido.ComAlias & "." & Inscrito.Membro.DbAlias
            End SyncLock
        End Function

        Public Function SintaxeDeOperador(ByVal Operador As datOperadores) As String Implements ICONSTRUTOR.SintaxeDeOperador
            SyncLock _locker
                Select Case Operador

                    Case datOperadores.Igual : Return "="
                    Case datOperadores.Diferente : Return "<>"
                    Case datOperadores.Maior : Return ">"
                    Case datOperadores.MaiorIgual : Return ">="
                    Case datOperadores.Menor : Return "<"
                    Case datOperadores.MenorIgual : Return "<="
                    Case datOperadores.Semelhante : Return " LIKE "

                End Select
            End SyncLock
        End Function

        Public Function SintaxeDePonte(ByVal Ponte As datPontes) As String Implements ICONSTRUTOR.SintaxeDePonte
            SyncLock _locker
                Select Case Ponte

                    Case datPontes.E : Return "AND"
                    Case datPontes.Ou : Return "OR"

                End Select
            End SyncLock
        End Function



        Private Function MembrosAceites(ByVal Pedido As datPedido) As String
            SyncLock _locker
                Dim i As Integer
                Dim s As String
                Dim frase As String

                ' resolver os inscritos do pedido

                If Not Pedido.Inscritos Is Nothing Then
                    For i = 0 To Pedido.Inscritos.GetUpperBound(0)

                        s = SintaxeDeInscri��o(Pedido.Inscritos(i))
                        If frase Is Nothing Then frase = s Else frase = frase & ", " & s

                    Next
                End If

                ' resolver os subpedidos do pedido

                If Not Pedido.SubPedidos Is Nothing Then

                    For i = 0 To Pedido.SubPedidos.GetUpperBound(0)

                        frase = frase & ", " & MembrosAceites(Pedido.SubPedidos(i))

                    Next

                End If

                Return frase
            End SyncLock
        End Function

        Private Function MembrosAOrdenar(ByVal Pedido As datPedido) As String
            SyncLock _locker
                Dim i As Integer
                Dim frase As String = ""

                If Not Pedido.Ordena��es Is Nothing Then

                    For i = Pedido.Ordena��es.GetLowerBound(0) To Pedido.Ordena��es.GetUpperBound(0)

                        If frase.Length > 0 Then frase = frase & ", "
                        frase = frase & SintaxeDeMembro(Pedido.Ordena��es(i))

                        Select Case Pedido.Ordena��es(i).Direc��o

                            Case datDirec��es.Ascendente : frase = frase & " ASC"
                            Case datDirec��es.Descendente : frase = frase & " DESC"

                        End Select

                    Next

                End If

                If Not Pedido.SubPedidos Is Nothing Then

                    For i = 0 To Pedido.SubPedidos.GetUpperBound(0)

                        If Not Pedido.SubPedidos(i).Ordena��es Is Nothing Then

                            If frase.Length > 0 Then frase = frase & ", "
                            frase = frase & MembrosAOrdenar(Pedido.SubPedidos(i))

                        End If

                    Next

                End If

                Return frase
            End SyncLock
        End Function

        Private Function TabelasLigadas(ByVal Pedido As datPedido) As String
            SyncLock _locker
                '                                                                               

                Dim j As Integer
                Dim frase As String = ""

                If Not Pedido.SubPedidos Is Nothing Then

                    For j = 0 To Pedido.SubPedidos.GetUpperBound(0)

                        If frase.Length > 0 Then frase = frase & vbCrLf

                        frase = frase & g10phPDF4.Basframe.Strings.Repete(vbTab, Pedido.SubPedidos(j).Nivel)

                        frase = frase & TabelasLigadas(Pedido.SubPedidos(j), Pedido.SubCondi��es(j))

                        If Not Pedido.SubPedidos(j).Filtros Is Nothing Then frase = frase & " " & SintaxeDeCondi��o(Pedido.SubPedidos(j).Filtros, datPontes.E)

                    Next

                    Return frase

                End If
            End SyncLock
        End Function

        Private Function TabelasLigadas(ByVal Pedido As datPedido, ByVal Condi��o() As datInscri��o) As String
            SyncLock _locker
                Dim frase As String

                Select Case Pedido.Liga��o

                    Case datLiga��es.LeftJoin : frase = frase & "LEFT JOIN "
                    Case datLiga��es.InnerJoin : frase = frase & "INNER JOIN "
                    Case datLiga��es.RightJoin : frase = frase & "RIGHT JOIN "
                    Case datLiga��es.FullJoin : frase = frase & "FULL OUTER JOIN "
                    Case datLiga��es.CrossJoin : frase = frase & "CROSS JOIN "

                End Select

                frase = frase & Pedido.Objecto.DbAlias & " " & Pedido.ComAlias

                If Not Pedido.SubPedidos Is Nothing Then
                    frase = frase & vbCrLf & TabelasLigadas(Pedido) & vbCrLf & g10phPDF4.Basframe.Strings.Repete(vbTab, Pedido.Nivel)
                Else
                    frase = frase & " "
                End If

                frase = frase & "ON " & SintaxeDeCondi��o(Condi��o, datPontes.N�oDefinido)

                Return frase
            End SyncLock
        End Function

    End Class

    Public Class datConstrutorXML
        Implements ICONSTRUTOR

        Private Shared ReadOnly _locker As New Object()

        Public Function ConstroiSintaxe(ByVal Pedido As datPedido) As String Implements ICONSTRUTOR.ConstroiSintaxe
            SyncLock _locker
                Pedido.Aceites = datAceita��o.Novas(Pedido)
            End SyncLock
        End Function

        Public Function SintaxeDeAltera��o(ByVal Pedido As datPedido) As String Implements ICONSTRUTOR.SintaxeDeAltera��o

        End Function

        Public Function SintaxeDeCondi��o(ByVal Condi��es() As datInscri��o, ByVal For�aPonte As datPontes) As String Implements ICONSTRUTOR.SintaxeDeCondi��o

        End Function

        Public Function SintaxeDeConsulta(ByVal Pedido As datPedido) As String Implements ICONSTRUTOR.SintaxeDeConsulta

        End Function

        Public Function SintaxeDeElimina��o(ByVal Pedido As datPedido) As String Implements ICONSTRUTOR.SintaxeDeElimina��o

        End Function

        Public Function SintaxeDeFAgregada(ByVal Inscrito As datInscri��o) As String Implements ICONSTRUTOR.SintaxeDeFAgregada

        End Function

        Public Function SintaxeDeFiltro(ByVal Pedido As datPedido) As String Implements ICONSTRUTOR.SintaxeDeFiltro

        End Function

        Public Function SintaxeDeInscri��o(ByVal Inscri��o As datInscri��o) As String Implements ICONSTRUTOR.SintaxeDeInscri��o

        End Function

        Public Function SintaxeDeInser��o(ByVal Pedido As datPedido) As String Implements ICONSTRUTOR.SintaxeDeInser��o

        End Function

        Public Function SintaxeDeMembro(ByVal Membro As datInscri��o) As String Implements ICONSTRUTOR.SintaxeDeMembro

        End Function

        Public Function SintaxeDeOperador(ByVal Operador As datOperadores) As String Implements ICONSTRUTOR.SintaxeDeOperador

        End Function

        Public Function SintaxeDeOrdena��o(ByVal Pedido As datPedido) As String Implements ICONSTRUTOR.SintaxeDeOrdena��o

        End Function

        Public Function SintaxeDeParametroFiltro(ByVal Membro As datMembro) As String Implements ICONSTRUTOR.SintaxeDeParametroFiltro

        End Function

        Public Function SintaxeDeParametroValor(ByVal Membro As datMembro) As String Implements ICONSTRUTOR.SintaxeDeParametroValor

        End Function

        Public Function SintaxeDePonte(ByVal Pontes As datPontes) As String Implements ICONSTRUTOR.SintaxeDePonte

        End Function

    End Class

    '                                                                                                       

    Public Class datAdaptadorDB
        Implements IADAPTADOR

        Public Conex�o As System.Data.IDbConnection
        Public Transa��o As System.Data.IDbTransaction

        Public Erro As Boolean
        Public ErroAvisa As Boolean
        Public ErroResposta As MsgBoxResult

        Private l_excep��o As System.Exception

        Private Shared ReadOnly _locker As New Object()

        Public Sub New()

        End Sub

        Public Sub New(ByVal conex�o As System.Data.IDbConnection)
            SyncLock _locker
                Me.Conex�o = conex�o
            End SyncLock
        End Sub

        Public Overridable Property Excep��o() As System.Exception Implements IADAPTADOR.Excep��o
            Get
                SyncLock _locker
                    Return l_excep��o
                End SyncLock
            End Get
            Set(ByVal Value As System.Exception)
                SyncLock _locker
                    l_excep��o = Value
                End SyncLock
            End Set
        End Property


        Public Function Consulta(ByRef Ac��o As datAc��o, ByVal DatletACarregar As IDATLET) As Boolean Implements IADAPTADOR.Consulta
            SyncLock _locker
                Dim Cursor As System.Data.IDataReader

                Consulta = b_select(Ac��o, Cursor, False)

                If Consulta Then

                    If Cursor.Read Then

                        b_recolhe(DatletACarregar, Ac��o, Cursor, 0)
                        Ac��o.ResultadoAfectados = 1

                    Else

                        Ac��o.ResultadoAfectados = 0

                    End If

                    Cursor.Close()

                Else

                    Ac��o.ResultadoAfectados = 0

                End If
            End SyncLock
        End Function

        Public Function ConsultaEscalar(ByRef Ac��o As datAc��o) As Boolean Implements IADAPTADOR.ConsultaEscalar
            SyncLock _locker
                ConsultaEscalar = b_select(Ac��o, Nothing, True)

                If ConsultaEscalar And Not Ac��o.ResultadoEscalar Is Nothing Then

                    Ac��o.ResultadoAfectados = 1

                Else

                    Ac��o.ResultadoAfectados = 0

                End If
            End SyncLock
        End Function

        Public Function Executa(ByRef Ac��o As datAc��o) As Boolean Implements IADAPTADOR.Executa
            SyncLock _locker
                Return b_exec(Ac��o)
            End SyncLock
        End Function

        Public Function Selecciona(ByRef Ac��o As datAc��o, ByVal DatletAClonar As IDATLET) As IDATLET() Implements IADAPTADOR.Selecciona
            SyncLock _locker
                Dim cursor As System.Data.IDataReader
                Dim resultados() As IDATLET
                Dim l As Integer
                Dim sucesso As Boolean

                Ac��o.ResultadoAfectados = 0
                sucesso = b_select(Ac��o, cursor, False)

                If sucesso Then

                    l = -1
                    While cursor.Read

                        l += 1
                        ReDim Preserve resultados(l)

                        resultados(l) = CType(DatletAClonar.DatClone, IDATLET)
                        b_recolhe(resultados(l), Ac��o, cursor, 0)

                    End While

                    cursor.Close()

                    Ac��o.ResultadoAfectados = l + 1
                    Return resultados

                Else

                    Return Nothing

                End If
            End SyncLock
        End Function

        Public ReadOnly Property Natureza() As datNaturezas Implements IADAPTADOR.Natureza
            Get
                SyncLock _locker
                    Return datNaturezas.Db
                End SyncLock
            End Get
        End Property

        Public Function Transa��oAbre() As Boolean Implements IADAPTADOR.Transa��oAbre
            SyncLock _locker
                Transa��o = Conex�o.BeginTransaction
                Return (Not Transa��o Is Nothing)
            End SyncLock
        End Function

        Public Function Transa��oCommit() As Boolean Implements IADAPTADOR.Transa��oCommit
            SyncLock _locker
                Transa��o.Commit()
                Transa��o = Nothing
                Return True
            End SyncLock
        End Function

        Public Function Transa��oRollback() As Boolean Implements IADAPTADOR.Transa��oRollback
            SyncLock _locker
                Transa��o.Rollback()
                Transa��o = Nothing
                Return True
            End SyncLock
        End Function


        Public Function b_select(ByRef Ac��o As datAc��o, ByRef getCursor As System.Data.IDataReader, ByVal Escalar As Boolean) As Boolean
            SyncLock _locker
                Dim Comando As System.Data.IDbCommand

                Comando = Conex�o.CreateCommand
                Comando.CommandType = CommandType.Text
                Comando.CommandText = Ac��o.Sintaxe
                If Not Me.Transa��o Is Nothing Then Comando.Transaction = Me.Transa��o

                b_parametriza(Ac��o, Comando, Ac��o.Filtros, "", True, False, Ac��o.ParametrosFiltro)

                Do

                    Erro = False
                    ErroResposta = MsgBoxResult.Ok
                    Me.Excep��o = Nothing

                    Try

                        If Escalar Then
                            Ac��o.ResultadoEscalar = Comando.ExecuteScalar
                            Return (Not Ac��o.ResultadoEscalar Is Nothing)
                        Else
                            getCursor = Comando.ExecuteReader
                            Return (Not getCursor Is Nothing)
                        End If

                    Catch excep��o_ao_comando As System.Exception

                        Erro = True
                        Me.Excep��o = excep��o_ao_comando

                        ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcep��o(excep��o_ao_comando, Comando.CommandText, ErroAvisa, True, False)

                    End Try

                Loop While ErroResposta = MsgBoxResult.Retry

                Return False
            End SyncLock
        End Function

        Public Function b_exec(ByRef Ac��o As datAc��o) As Boolean
            SyncLock _locker
                Dim Comando As System.Data.IDbCommand
                Dim Cursor As System.Data.IDataReader

                Comando = Conex�o.CreateCommand
                Comando.CommandType = CommandType.Text
                Comando.CommandText = Ac��o.Sintaxe
                If Not Me.Transa��o Is Nothing Then Comando.Transaction = Me.Transa��o

                If Ac��o.Pedido.Opera��o = datOpera��es.Altera��o Or Ac��o.Pedido.Opera��o = datOpera��es.Inser��o Then
                    b_parametriza(Ac��o, Comando, Ac��o.Inscritos, "", False, True, Ac��o.ParametrosValor)
                End If

                If Ac��o.Pedido.Opera��o = datOpera��es.Altera��o Or Ac��o.Pedido.Opera��o = datOpera��es.Elimina��o Then
                    b_parametriza(Ac��o, Comando, Ac��o.Filtros, "", False, False, Ac��o.ParametrosFiltro)
                End If

                Do

                    ErroResposta = MsgBoxResult.Ok
                    Me.Excep��o = Nothing
                    Erro = False

                    Try

                        Ac��o.ResultadoAfectados = Comando.ExecuteNonQuery
                        Return True

                    Catch execep��o_ao_comando As System.Exception

                        Erro = True
                        Me.Excep��o = execep��o_ao_comando
                        Ac��o.ResultadoAfectados = 0

                        ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcep��o(execep��o_ao_comando, Comando.CommandText, ErroAvisa, True, False)

                    End Try

                Loop While ErroResposta = MsgBoxResult.Retry

                Return False
            End SyncLock
        End Function

        Public Sub b_recolhe(ByVal Datlet As IDATLET, ByRef Ac��o As datAc��o, ByVal Cursor As System.Data.IDataReader, ByRef JoinIndex As Integer)
            SyncLock _locker
                Dim i As Integer        ' incri��o a ler
                Dim m As Integer        ' membro que a inscri��o representa
                Dim ji As Integer       ' indice do join da presente recolha

                Dim getdt As IDATLET

                ji = JoinIndex

                For i = 0 To Ac��o.Pedido.Aceites.GetUpperBound(0)

                    If Ac��o.Pedido.Aceites(i).JoinNivel = ji Then

                        m = Ac��o.Pedido.Aceites(i).Membro.Ordem

                        If Cursor.IsDBNull(i) Then
                            Datlet.DatAtributo(m) = Nothing
                        Else
                            Select Case Ac��o.Pedido.Aceites(i).Membro.Tipo
                                Case DbType.Int32 : Datlet.DatAtributo(m) = Cursor.GetInt32(i)
                                Case DbType.Decimal : Datlet.DatAtributo(m) = Cursor.GetDecimal(i)
                                Case DbType.Date : Datlet.DatAtributo(m) = Cursor.GetDateTime(i)
                                Case DbType.Time : Datlet.DatAtributo(m) = Cursor.GetDateTime(i)
                                Case DbType.Boolean : Datlet.DatAtributo(m) = Cursor.GetBoolean(i)
                                Case Else : Datlet.DatAtributo(m) = Cursor.GetString(i)
                            End Select
                        End If

                        ' este membro referencia um membro de outro objecto?
                        If Not Ac��o.Pedido.Aceites(i).Membro.Referencia Is Nothing Then

                            ' Pq � necess�rio contar as liga��es que v�o sendo feitas por membros com referencias, o JoinIndex
                            ' � incrementado antes de tratar a referencia e � passado por byref de forma a, que se a referencia
                            ' contiver ele pr�pria outras referencias, sabermos do numero total de liga��es.

                            JoinIndex = JoinIndex + 1

                            Me.b_recolhe(Ac��o.Pedido.Aceites(i).Membro.Referencia, Ac��o, Cursor, JoinIndex)

                            '' temos uma datlet para apanhar os resultados de outro objecto?
                            'If Datlet.DatReferencia(Ac��o.Pedido.Aceites(i).Membro, getdt, Nothing) Then

                            '    ' apanhar resultados do outro objecto
                            '    Me.b_recolhe(getdt, Ac��o, Cursor, JoinIndex)

                            'End If

                        End If
                    End If

                Next
            End SyncLock
        End Sub

        Public Function b_parametriza(ByRef Ac��o As datAc��o, ByVal Comando As System.Data.IDbCommand, ByVal Inscritos() As Integer, ByRef getsetBufferParametrizados As String, ByVal ResetParametriza��o As Boolean, Optional ByVal Setting As Boolean = True, Optional ByVal Variaveis As Object() = Nothing) As Boolean
            SyncLock _locker
                Dim parametros() As System.Data.IDataParameter
                Dim i As Integer
                Dim m As Integer

                If ResetParametriza��o Then

                    Comando.Parameters.Clear()
                    getsetBufferParametrizados = ""

                End If

                If Inscritos Is Nothing Then

                    Return False

                Else

                    ReDim parametros(Inscritos.GetUpperBound(0))

                    For i = 0 To Inscritos.GetUpperBound(0)

                        m = Inscritos(i)

                        ' verificar se um membro j� foi parametrizado
                        ' tenho que verificar porque este pode aparecer v�rias vezes nas inscri��es (Fun��es; Concatena��es;)
                        ' pensar: datAdaptadorDB: n�o gosto desta forma de verificar se um membro j� foi parametrizado!

                        If getsetBufferParametrizados.IndexOf(m & ",") = -1 Then

                            getsetBufferParametrizados = getsetBufferParametrizados & m & ","

                            parametros(i) = Comando.CreateParameter
                            parametros(i).DbType = Ac��o.Pedido.Objecto.Membros(m).Tipo

                            If Setting Then
                                parametros(i).ParameterName = Ac��o.Construtor.SintaxeDeParametroValor(Ac��o.Pedido.Objecto.Membros(m))
                            Else
                                parametros(i).ParameterName = Ac��o.Construtor.SintaxeDeParametroFiltro(Ac��o.Pedido.Objecto.Membros(m))
                            End If

                            If Variaveis(i) Is Nothing Then
                                parametros(i).Value = ""
                            Else
                                parametros(i).Value = Variaveis(i)
                            End If

                            Comando.Parameters.Add(parametros(i))

                        End If

                    Next

                    Return True

                End If
            End SyncLock
        End Function

    End Class

    Public Class datAdaptadorXML
        Implements IADAPTADOR

        ' consulta: se n�o h� filtros l� toda a estrutura, se h� filtros filtra pelo conteudo dos respectivos atributos
        ' inser��o: se j� existe altera o respectivo n�, sen�o insere um novo.
        ' altera��o: se j� existe altera o respectivo n�, sen�o insere um novo.
        ' elimina��o: se existe elimina o n� da estrutura.
        ' "filtros" indicam os atributos e conteudos para load, save e delete.
        ' "inscritos" indicam quais os campos a ler, alterar ou a inserir.
        ' "ordens" indica quais os atributos pelos quais se deve ordenar uma consulta.
        ' "aceites" em ac��es xml nunca ser� usado.

        Public FicheiroPath As String
        Public NoBase As System.Xml.XmlNode

        ' a prioridade no destino � dada ao NoBase e s� se este for omitido se abre o FicheiroPath

        Public Erro As Boolean
        Public ErroAvisa As Boolean
        Public ErroResposta As MsgBoxResult

        Private l_excep��o As System.Exception

        Private Shared ReadOnly _locker As New Object()


        Public Sub New()

        End Sub

        Public Sub New(ByVal ficheiropath As String)
            SyncLock _locker
                Me.FicheiroPath = ficheiropath
            End SyncLock
        End Sub

        Public Sub New(ByVal nobase As System.Xml.XmlNode)
            SyncLock _locker
                Me.NoBase = nobase
            End SyncLock
        End Sub


        Public Overridable Property Excep��o() As System.Exception Implements IADAPTADOR.Excep��o
            Get
                SyncLock _locker
                    Return l_excep��o
                End SyncLock
            End Get
            Set(ByVal Value As System.Exception)
                SyncLock _locker
                    l_excep��o = Value
                End SyncLock
            End Set
        End Property


        Public Function Consulta(ByRef Ac��o As datAc��o, ByVal DatletACarregar As IDATLET) As Boolean Implements IADAPTADOR.Consulta
            SyncLock _locker
                ' executa as selec��es de um s� registo

                Return x_consulta(Ac��o, False, DatletACarregar)
            End SyncLock
        End Function

        Public Function ConsultaEscalar(ByRef Ac��o As datAc��o) As Boolean Implements IADAPTADOR.ConsultaEscalar
            SyncLock _locker
                ' executa as selec��es esclares

                Ac��o.ResultadoEscalar = Nothing
                Return x_consulta(Ac��o, True, Nothing)
            End SyncLock
        End Function

        Public Function Selecciona(ByRef Ac��o As datAc��o, ByVal DatletAClonar As IDATLET) As IDATLET() Implements IADAPTADOR.Selecciona
            SyncLock _locker
                ' executa as selec��es de n registos

                Return x_consulta_varios(Ac��o, DatletAClonar)
            End SyncLock
        End Function

        Public Function Executa(ByRef Ac��o As datAc��o) As Boolean Implements IADAPTADOR.Executa
            SyncLock _locker
                Select Case Ac��o.Pedido.Opera��o

                    Case datOpera��es.Altera��o : Return x_altera(Ac��o)

                    Case datOpera��es.Inser��o : Return x_insere(Ac��o)

                    Case datOpera��es.Elimina��o : Return x_altera(Ac��o)

                End Select
            End SyncLock
        End Function

        Public ReadOnly Property Natureza() As datNaturezas Implements IADAPTADOR.Natureza
            Get
                SyncLock _locker
                    Return datNaturezas.Xml
                End SyncLock
            End Get
        End Property

        Public Function Transa��oAbre() As Boolean Implements IADAPTADOR.Transa��oAbre

            ' todo: datAdptadorXML: Transa��oAbre

        End Function

        Public Function Transa��oCommit() As Boolean Implements IADAPTADOR.Transa��oCommit

            ' todo: datAdptadorXML: Transa��oCommit

        End Function

        Public Function Transa��oRollback() As Boolean Implements IADAPTADOR.Transa��oRollback

            ' todo: datAdptadorXML: Transa��oRollback

        End Function



        Public Function x_altera(ByRef Ac��o As datAc��o) As Boolean
            SyncLock _locker
                Dim xmldoc As System.Xml.XmlDocument
                Dim no_datlet As System.Xml.XmlNode
                Dim no_selec��o() As System.Xml.XmlNode
                Dim no_registo As System.Xml.XmlNode

                Ac��o.ResultadoAfectados = 0

                If x_getnodatlet(Ac��o, xmldoc, no_datlet) Then

                    no_selec��o = x_select(Ac��o, no_datlet, False, False)

                    For Each no_registo In no_selec��o

                        x_checkmembros(Ac��o, no_registo)
                        x_escreve(Ac��o, no_registo)
                        Ac��o.ResultadoAfectados = Ac��o.ResultadoAfectados + 1

                    Next

                    If Ac��o.ResultadoAfectados > 0 And Not xmldoc Is Nothing Then xmldoc.Save(FicheiroPath)

                    Return True

                Else

                    Return False

                End If
            End SyncLock
        End Function

        Public Function x_insere(ByRef Ac��o As datAc��o) As Boolean
            SyncLock _locker
                Dim xmldoc As System.Xml.XmlDocument
                Dim no_datlet As System.Xml.XmlNode
                Dim no_registo As System.Xml.XmlNode

                Ac��o.ResultadoAfectados = 0

                If x_getnodatlet(Ac��o, xmldoc, no_datlet) Then

                    no_registo = x_criano(Ac��o, no_datlet)
                    no_datlet.AppendChild(no_registo)
                    x_escreve(Ac��o, no_registo)
                    Ac��o.ResultadoAfectados = 1

                    If Not xmldoc Is Nothing Then xmldoc.Save(FicheiroPath)

                    Return True

                Else

                    Return False

                End If
            End SyncLock
        End Function

        Public Function x_elimina(ByVal Datlet As IDATLET, ByRef Ac��o As datAc��o) As Boolean
            SyncLock _locker
                Dim xmldoc As System.Xml.XmlDocument
                Dim no_datlet As System.Xml.XmlNode
                Dim no_selec��o() As System.Xml.XmlNode
                Dim no_registo As System.Xml.XmlNode

                Ac��o.ResultadoAfectados = 0

                If x_getnodatlet(Ac��o, xmldoc, no_datlet) Then

                    no_selec��o = x_select(Ac��o, no_datlet, False, False)

                    For Each no_registo In no_selec��o

                        no_datlet.RemoveChild(no_registo)

                        Ac��o.ResultadoAfectados = Ac��o.ResultadoAfectados + 1

                    Next

                    If Ac��o.ResultadoAfectados > 0 And Not xmldoc Is Nothing Then xmldoc.Save(FicheiroPath)

                    Return True

                Else

                    Return False

                End If
            End SyncLock
        End Function

        Public Function x_consulta(ByRef Ac��o As datAc��o, ByVal Escalar As Boolean, ByVal DatletACarregar As IDATLET) As Boolean
            SyncLock _locker
                Dim xmldoc As System.Xml.XmlDocument
                Dim no_datlet As System.Xml.XmlNode
                Dim no_selec��o() As System.Xml.XmlNode

                Ac��o.ResultadoAfectados = 0

                If x_getnodatlet(Ac��o, xmldoc, no_datlet) Then

                    no_selec��o = x_select(Ac��o, no_datlet, False, Escalar)

                    If Not no_selec��o Is Nothing Then

                        x_le(Ac��o, no_selec��o(0), Escalar, DatletACarregar)
                        Ac��o.ResultadoAfectados = 1

                    End If

                    Return True

                Else

                    Return False

                End If
            End SyncLock
        End Function

        Public Function x_consulta_varios(ByRef Ac��o As datAc��o, ByVal DatletAClonar As IDATLET) As IDATLET()
            SyncLock _locker
                Dim xmldoc As System.Xml.XmlDocument
                Dim no_datlet As System.Xml.XmlNode
                Dim no_selec��o() As System.Xml.XmlNode
                Dim no_registo As System.Xml.XmlNode
                Dim result() As IDATLET
                Dim r As Integer

                Ac��o.ResultadoAfectados = 0

                If x_getnodatlet(Ac��o, xmldoc, no_datlet) Then

                    no_selec��o = x_select(Ac��o, no_datlet, False, False)

                    ReDim result(no_selec��o.GetUpperBound(0))

                    For Each no_registo In no_selec��o

                        result(r) = CType(DatletAClonar.DatClone, IDATLET)
                        x_le(Ac��o, no_registo, False, result(r))
                        r = r + 1

                        Ac��o.ResultadoAfectados = Ac��o.ResultadoAfectados + 1

                    Next

                End If

                Return result
            End SyncLock
        End Function

        Public Function x_getnodatlet(ByRef Ac��o As datAc��o, ByRef getXmlDoc As System.Xml.XmlDocument, ByRef getNoDatlet As System.Xml.XmlNode) As Boolean
            SyncLock _locker
                Do

                    Erro = False
                    Me.Excep��o = Nothing
                    getNoDatlet = XMLGetNo(NoBase, FicheiroPath, Ac��o.Pedido.Objecto.DbGrupo, (Ac��o.Pedido.Opera��o = datOpera��es.Inser��o), getXmlDoc, Me.Excep��o)

                    If Me.Excep��o Is Nothing Then

                        Return True

                    Else

                        Erro = True

                        ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcep��o(Me.Excep��o, FicheiroPath, ErroAvisa, True, False)

                    End If

                Loop While ErroResposta = MsgBoxResult.Retry

                Return False
            End SyncLock
        End Function

        Public Function x_select(ByRef Ac��o As datAc��o, ByVal no_datlet As System.Xml.XmlNode, ByVal S�OPrimeiro As Boolean, ByVal Escalar As Boolean) As System.Xml.XmlNode()
            SyncLock _locker
                Dim no_result() As System.Xml.XmlNode
                Dim n As System.Xml.XmlNode
                Dim f As Integer
                Dim a As Integer
                Dim t As Integer
                Dim coincide As Boolean

                If no_datlet.HasChildNodes Then

                    For Each no_registo As System.Xml.XmlNode In no_datlet.ChildNodes

                        coincide = True

                        If Ac��o.Pedido.Filtros Is Nothing Then

                            coincide = True

                        Else

                            For f = 0 To Ac��o.Pedido.Filtros.GetUpperBound(0)

                                a = Ac��o.Filtros(f)
                                n = no_registo.Item(Ac��o.Pedido.Objecto.Membros(a).Nome)

                                ' verificar se o membro est� presente no n�

                                If n Is Nothing Then
                                    coincide = False
                                Else

                                    If n.InnerText = CType(Ac��o.ParametrosFiltro(f), String) Then
                                        coincide = coincide And True
                                    Else
                                        coincide = coincide And False
                                    End If

                                End If

                            Next

                        End If

                        If coincide Then

                            ReDim Preserve no_result(t)
                            no_result(t) = no_registo
                            t = t + 1

                        End If

                        If Escalar Then Exit For

                    Next

                End If

                Return no_result
            End SyncLock
        End Function

        Public Function x_criano(ByRef Ac��o As datAc��o, ByVal no_datlet As System.Xml.XmlNode) As System.Xml.XmlNode
            SyncLock _locker
                Dim no_registo As System.Xml.XmlNode
                Dim a As Integer
                Dim m As Integer

                no_registo = no_datlet.OwnerDocument.CreateElement(Ac��o.Pedido.Objecto.Nome)

                For a = 0 To Ac��o.Pedido.Aceites.GetUpperBound(0)

                    m = Ac��o.Aceites(a)
                    no_registo.AppendChild(no_datlet.OwnerDocument.CreateElement(Ac��o.Pedido.Objecto.Membros(m).Nome))

                Next

                Return no_registo
            End SyncLock
        End Function

        Public Sub x_checkmembros(ByRef Ac��o As datAc��o, ByVal no_registo As System.Xml.XmlNode)
            SyncLock _locker
                ' para cada aceite verificar se existe no estrutura

                Dim a As Integer
                Dim m As Integer
                Dim no_membro As System.Xml.XmlNode

                For a = 0 To Ac��o.Pedido.Aceites.GetUpperBound(0)

                    m = Ac��o.Aceites(a)

                    no_membro = no_registo.Item(Ac��o.Pedido.Objecto.Membros(m).Nome)

                    If no_membro Is Nothing Then
                        ' membro n�o existe na estrutura
                        no_registo.AppendChild(no_registo.OwnerDocument.CreateElement(Ac��o.Pedido.Objecto.Membros(m).Nome))
                    End If

                    no_membro = Nothing

                Next
            End SyncLock
        End Sub

        Public Sub x_escreve(ByRef Ac��o As datAc��o, ByVal no_registo As System.Xml.XmlNode)
            SyncLock _locker
                Dim a As Integer
                Dim m As Integer

                For a = 0 To Ac��o.Inscritos.GetUpperBound(0)

                    m = Ac��o.Inscritos(a)
                    no_registo.Item(Ac��o.Pedido.Objecto.Membros(m).Nome).InnerText = CType(Ac��o.ParametrosValor(a), String)

                Next
            End SyncLock
        End Sub

        Public Sub x_le(ByRef Ac��o As datAc��o, ByVal no_registo As System.Xml.XmlNode, ByVal Escalar As Boolean, ByVal DatletACarregar As IDATLET)
            SyncLock _locker
                Dim a As Integer
                Dim m As Integer
                Dim n As System.Xml.XmlNode

                For a = 0 To Ac��o.Aceites.GetUpperBound(0)

                    m = Ac��o.Aceites(a)
                    n = no_registo.Item(Ac��o.Pedido.Objecto.Membros(m).Nome)

                    If n Is Nothing Then
                        DatletACarregar.DatAtributo(m) = Nothing
                    Else
                        DatletACarregar.DatAtributo(m) = n.InnerText
                    End If

                    If Escalar Then
                        Ac��o.ResultadoEscalar = n.InnerText
                        Exit For
                    End If

                Next
            End SyncLock
        End Sub

    End Class

    '                                                                                                       

    Public Class datCanalDB
        Inherits datCanal

        Private Shared ReadOnly _locker As New Object()

        Public Sub New(ByVal Mania As datManias, ByVal conex�o As System.Data.IDbConnection)
            SyncLock _locker
                Select Case Mania

                    Case datManias.SqlServer : Me.Construtor = New datConstrutorSQL

                End Select

                Me.Adaptador = New datAdaptadorDB(conex�o)
            End SyncLock
        End Sub

    End Class

    Public Class datCanalXML
        Inherits datCanal

        Private Shared ReadOnly _locker As New Object()

        Public Sub New(ByVal Ficheiro As String)
            SyncLock _locker
                Me.Construtor = New datConstrutorXML
                Me.Adaptador = New datAdaptadorXML(Ficheiro)
            End SyncLock
        End Sub

        Public Sub New(ByVal NoBase As System.Xml.XmlNode)
            SyncLock _locker
                Me.Construtor = New datConstrutorXML
                Me.Adaptador = New datAdaptadorXML(NoBase)
            End SyncLock
        End Sub

    End Class

    Public MustInherit Class DatletBase
        Implements IDATLET


        ' todo: DatletBase: seleccionar v�rios registos a partir e para a mesma datlet
        ' todo: DatletBase: arranjar forma de depois da ac��o executada retornar a excep��o para a datlet



        Protected l_info As datInfo
        Protected l_canal As New datCanal
        Protected l_excep��o As System.Exception
        Protected l_parametros() As Object
        Protected l_preparados() As datAc��o
        Protected l_ac��o As datAc��o
        Protected l_filtro_geral() As datInscri��o

        Private Shared ReadOnly _locker As New Object()

#Region "       Construtores/Destrutores"

        Protected Overrides Sub Finalize()
            SyncLock _locker
                Me.DatDispose()

                MyBase.Finalize()
            End SyncLock
        End Sub

#End Region

#Region "       Suporte"

        Protected Overridable Sub private_clone(ByVal datalet_a_clonar As DatletBase)
            SyncLock _locker
                With datalet_a_clonar

                    .l_info = Me.DatInfo
                    .l_canal = Me.l_canal
                    .l_excep��o = Me.l_excep��o
                    ' n�o clonamos os parametros 
                    'If Not Me.l_parametros Is Nothing Then .l_parametros = CType(Me.l_parametros.Clone, Object())
                    If Not Me.l_preparados Is Nothing Then .l_preparados = CType(Me.l_preparados.Clone, datAc��o())
                    .l_ac��o = Me.l_ac��o
                    ' n�o clonamos o filtro geral
                    'If Not Me.l_filtro_geral Is Nothing Then .l_filtro_geral = CType(Me.l_filtro_geral.Clone, datInscri��o())

                    For a As Integer = 0 To l_info.Atributos.Length - 1
                        .DatAtributo(a) = Me.DatAtributo(a)
                    Next

                End With
            End SyncLock
        End Sub

        Protected Overridable Sub private_definepedido(ByVal pedido As datPedido, ByVal alcance As Integer)
            SyncLock _locker
                pedido.Objecto = Me.l_info.Objecto

                Select Case alcance

                    Case datAlcances.select_ficha
                        pedido.Opera��o = datOpera��es.Consulta
                        pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosTodos))
                        pedido.MaisFiltros(datInscri��o.Novas(l_info.Objecto.MembrosChave))
                        pedido.Ordena��es = Nothing

                    Case datAlcances.select_registo
                        pedido.Opera��o = datOpera��es.Consulta
                        pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosTodos))
                        pedido.MaisFiltros(datInscri��o.Novas(l_info.Objecto.MembrosPonteiro))
                        pedido.Ordena��es = Nothing

                    Case datAlcances.select_lista
                        pedido.Opera��o = datOpera��es.Consulta
                        pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosLista))
                        pedido.Filtros = Nothing
                        pedido.MaisOrdena��es(datInscri��o.Novas(l_info.Objecto.MembrosChave))

                    Case datAlcances.select_fichas
                        pedido.Opera��o = datOpera��es.Consulta
                        pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosTodos))
                        pedido.Filtros = Nothing
                        pedido.MaisOrdena��es(datInscri��o.Novas(l_info.Objecto.MembrosChave))

                    Case datAlcances.select_procura
                        pedido.Opera��o = datOpera��es.Consulta
                        pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosPonteiro))
                        pedido.MaisFiltros(datInscri��o.Novas(l_info.Objecto.MembrosChave))
                        pedido.Ordena��es = Nothing

                    Case datAlcances.select_vista
                        pedido.Opera��o = datOpera��es.Consulta
                        pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosVista))
                        pedido.MaisFiltros(datInscri��o.Novas(l_info.Objecto.MembrosPonteiro))
                        pedido.Ordena��es = Nothing

                    Case datAlcances.select_maximo
                        pedido.Opera��o = datOpera��es.Consulta
                        pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembroPonteiro, datFAgregadas.Maximo))
                        pedido.Filtros = Nothing
                        pedido.Ordena��es = Nothing
                        pedido.Lock = datLocks.Exclusivo

                    Case datAlcances.update_ficha
                        pedido.Opera��o = datOpera��es.Altera��o
                        pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosTodos))
                        pedido.MaisFiltros(datInscri��o.Novas(l_info.Objecto.MembrosPonteiro))
                        pedido.Ordena��es = Nothing

                    Case datAlcances.insert_ficha
                        pedido.Opera��o = datOpera��es.Inser��o
                        pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosTodos))
                        pedido.MaisFiltros(datInscri��o.Novas(l_info.Objecto.MembrosTodos))
                        pedido.Ordena��es = Nothing

                    Case datAlcances.delete_ficha
                        pedido.Opera��o = datOpera��es.Elimina��o
                        pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosTodos))
                        pedido.MaisFiltros(datInscri��o.Novas(l_info.Objecto.MembrosPonteiro))
                        pedido.Ordena��es = Nothing

                    Case datAlcances.select_primeiro
                        pedido.Opera��o = datOpera��es.Consulta
                        pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosTodos))
                        pedido.Inscritos(0).FAgregada = New datFun��o(datFAgregadas.Top, 1)
                        pedido.Filtros = Nothing
                        pedido.Ordena��es = Nothing

                    Case datAlcances.select_pesquisa
                        pedido.Opera��o = datOpera��es.Consulta
                        pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosPesquisa))
                        Dim d As Integer
                        Dim a As Integer
                        For d = 0 To l_info.Objecto.MembrosDescri��o.GetUpperBound(0)
                            a = l_info.Objecto.MembrosDescri��o(d).Ordem
                            If Not Me.DatAtributo(a) Is Nothing Or Not l_parametros(d) Is Nothing Then pedido.MaisFiltros(datInscri��o.Novas(l_info.Objecto.MembrosDescri��o(d), datOperadores.Semelhante, Nothing))
                        Next
                        pedido.MaisOrdena��es(datInscri��o.Novas(l_info.Objecto.MembrosChave))

                    Case datAlcances.select_verifica
                        pedido.Opera��o = datOpera��es.Consulta
                        pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosPonteiro))
                        pedido.MaisFiltros(datInscri��o.Novas(l_info.Objecto.MembroPonteiro))
                        pedido.Ordena��es = Nothing

                    Case datAlcances.insert_registo
                        pedido.Opera��o = datOpera��es.Inser��o
                        pedido.MaisInscritos(datInscri��o.Novas(l_info.Objecto.MembrosPonteiro))
                        pedido.MaisFiltros(datInscri��o.Novas(l_info.Objecto.MembrosPonteiro))
                        pedido.Ordena��es = Nothing

                End Select

                If Not l_filtro_geral Is Nothing Then pedido.MaisFiltros(l_filtro_geral)
                pedido.AutoCompleta()
            End SyncLock
        End Sub

        Protected Overridable Sub private_preparaac��o(ByRef ac��o As datAc��o)
            SyncLock _locker
                ac��o.PreparaAc��o(l_canal.Construtor)
            End SyncLock
        End Sub

        Protected Overridable Sub private_resetpreparados()
            SyncLock _locker
                l_preparados = Nothing
                ReDim l_preparados(datAlcances.count - 1)
            End SyncLock
        End Sub

        Protected Overridable Sub private_setcanal()
            SyncLock _locker
                For m As Integer = 0 To l_info.Objecto.Membros.GetUpperBound(0)

                    If Not l_info.Objecto.Membros(m).Referencia Is Nothing Then

                        l_info.Objecto.Membros(m).Referencia.DatCanal = l_canal

                    End If

                Next
            End SyncLock
        End Sub

        Protected Overridable Function private_atributosarray(ByVal ParamArray inscritos() As Integer) As Object()
            SyncLock _locker
                Dim atributos() As Object
                Dim m As Integer

                ReDim atributos(inscritos.GetUpperBound(0))

                For i As Integer = 0 To inscritos.GetUpperBound(0)

                    m = inscritos(i)
                    atributos(i) = Me.DatAtributo(m)

                Next

                Return atributos
            End SyncLock
        End Function

#End Region

#Region "       IDATLET"

        Public Overridable Property DatInfo() As datInfo Implements ILET.DatInfo
            Get
                SyncLock _locker
                    Return l_info
                End SyncLock
            End Get
            Set(ByVal Value As datInfo)
                SyncLock _locker
                    l_info = Value
                    private_resetpreparados()
                End SyncLock
            End Set
        End Property

        Public MustOverride Property DatAtributo(ByVal Index As Integer) As Object Implements ILET.DatAtributo

        Public MustOverride Function DatClone() As ILET Implements ILET.DatClone

        Public MustOverride Function DatNew() As ILET Implements ILET.DatNew

        Public MustOverride Function DatCast(ByVal Lista() As ILET) As ILET() Implements ILET.DatCast

        Public Overridable Sub DatCopy(ByVal datalet As ILET) Implements ILET.DatCopy
            SyncLock _locker
                Dim a As Integer

                For a = 0 To Me.DatInfo.Atributos.Length - 1

                    Me.DatAtributo(a) = datalet.DatAtributo(a)

                Next
            End SyncLock
        End Sub

        Public Overrides Function ToString() As String Implements ILET.ToString
            SyncLock _locker
                Dim c As Integer
                Dim t As String

                For c = 0 To Me.l_info.Objecto.Membros.GetUpperBound(0)

                    If Me.l_info.Objecto.Membros(c).Descritivo Then

                        If Not t Is Nothing AndAlso t.Length > 0 Then t = t & " - "
                        t = t & CType(Me.DatAtributo(c), String)

                    End If

                Next

                Return t
            End SyncLock
        End Function



        Public Overridable Property DatCanal() As datCanal Implements IDATLET.DatCanal
            Get
                SyncLock _locker
                    Return l_canal
                End SyncLock
            End Get
            Set(ByVal Value As datCanal)
                SyncLock _locker
                    l_canal = Value
                    private_resetpreparados()
                End SyncLock
            End Set
        End Property

        Public Overridable Property DatAc��o() As datAc��o Implements IDATLET.DatAc��o
            Get
                SyncLock _locker
                    Return l_ac��o
                End SyncLock
            End Get
            Set(ByVal Value As datAc��o)
                SyncLock _locker

                    l_ac��o = Value
                End SyncLock
            End Set
        End Property

        Public Overridable Property DatParametros() As Object() Implements IDATLET.DatParametros
            Get
                SyncLock _locker
                    Return l_parametros
                End SyncLock
            End Get
            Set(ByVal Value() As Object)
                SyncLock _locker
                    l_parametros = Value
                End SyncLock
            End Set
        End Property

        Public Overridable Sub DatPrepara(ByVal Alcance As Integer) Implements IDATLET.DatPrepara
            SyncLock _locker
                If Not l_preparados(Alcance).Definida Then

                    l_preparados(Alcance).Pedido = New datPedido
                    private_definepedido(l_preparados(Alcance).Pedido, Alcance)
                    l_preparados(Alcance).Definida = True

                End If

                If Not l_preparados(Alcance).Preparada Then

                    private_preparaac��o(l_preparados(Alcance))

                End If

                l_ac��o = l_preparados(Alcance)

                If Not l_ac��o.Inscritos Is Nothing Then l_ac��o.ParametrosValor = private_atributosarray(l_ac��o.Inscritos)

                If Me.l_parametros Is Nothing Then
                    If Not l_ac��o.Filtros Is Nothing Then l_ac��o.ParametrosFiltro = private_atributosarray(l_ac��o.Filtros)
                Else
                    l_ac��o.ParametrosFiltro = l_parametros
                End If

                l_ac��o.Reset()
            End SyncLock
        End Sub

        Public Overridable Overloads Function DatConsultaFicha() As Boolean Implements IDATLET.DatConsultaFicha
            SyncLock _locker
                DatPrepara(datAlcances.select_ficha)

                If l_canal.Adaptador.Consulta(l_ac��o, Me) Then

                    Return (l_ac��o.ResultadoAfectados > 0)

                Else

                    Return False

                End If
            End SyncLock
        End Function

        Public Overridable Overloads Function DatConsultaFicha(ByVal ParamArray Chave() As Object) As Boolean Implements IDATLET.DatConsultaFicha
            SyncLock _locker
                l_parametros = Chave

                DatConsultaFicha = (DatConsultaFicha())

                l_parametros = Nothing
            End SyncLock
        End Function

        Public Overridable Overloads Function DatConsultaRegisto() As Boolean Implements IDATLET.DatConsultaRegisto
            SyncLock _locker
                DatPrepara(datAlcances.select_registo)

                If l_canal.Adaptador.Consulta(l_ac��o, Me) Then

                    Return (l_ac��o.ResultadoAfectados > 0)

                Else

                    Return False

                End If
            End SyncLock
        End Function

        Public Overridable Overloads Function DatConsultaRegisto(ByVal Ponteiro As Integer) As Boolean Implements IDATLET.DatConsultaRegisto
            SyncLock _locker
                ReDim l_parametros(0)
                l_parametros(0) = Ponteiro

                DatConsultaRegisto = (DatConsultaRegisto())

                l_parametros = Nothing
            End SyncLock
        End Function

        Public Overridable Overloads Function DatConsultaVista() As Boolean Implements IDATLET.DatConsultaVista
            SyncLock _locker
                DatPrepara(datAlcances.select_vista)

                If l_canal.Adaptador.Consulta(l_ac��o, Me) Then

                    Return (l_ac��o.ResultadoAfectados > 0)

                Else

                    Return False

                End If
            End SyncLock
        End Function

        Public Overridable Overloads Function DatConsultaVista(ByVal Ponteiro As Integer) As Boolean Implements IDATLET.DatConsultaVista
            SyncLock _locker
                ReDim l_parametros(0)
                l_parametros(0) = Ponteiro

                DatConsultaVista = (DatConsultaVista())

                l_parametros = Nothing
            End SyncLock
        End Function

        Public Overridable Function DatSeleccionaFichas() As IDATLET() Implements IDATLET.DatSeleccionaFichas
            SyncLock _locker
                DatPrepara(datAlcances.select_fichas)

                Return l_canal.Adaptador.Selecciona(l_ac��o, Me)
            End SyncLock
        End Function

        Public Overridable Function DatSeleccionaLista() As IDATLET() Implements IDATLET.DatSeleccionaLista
            SyncLock _locker
                DatPrepara(datAlcances.select_lista)

                Return l_canal.Adaptador.Selecciona(l_ac��o, Me)
            End SyncLock
        End Function

        Public Overridable Function DatConsultaPrimeiro() As Boolean Implements IDATLET.DatConsultaPrimeiro
            SyncLock _locker
                DatPrepara(datAlcances.select_primeiro)

                If l_canal.Adaptador.Consulta(l_ac��o, Me) Then

                    Return (l_ac��o.ResultadoAfectados > 0)

                Else

                    Return False

                End If
            End SyncLock
        End Function

        Public Overridable Overloads Function DatPesquisa() As IDATLET() Implements IDATLET.DatPesquisa
            SyncLock _locker
                l_preparados(datAlcances.select_pesquisa).Definida = False
                l_preparados(datAlcances.select_pesquisa).Preparada = False

                DatPrepara(datAlcances.select_pesquisa)

                Return l_canal.Adaptador.Selecciona(l_ac��o, Me)
            End SyncLock
        End Function

        Public Overridable Overloads Function DatPesquisa(ByVal ParamArray Descri��o() As Object) As IDATLET() Implements IDATLET.DatPesquisa
            SyncLock _locker
                l_parametros = Descri��o

                DatPesquisa = (DatPesquisa())

                l_parametros = Nothing
            End SyncLock
        End Function

        Public Overridable Overloads Function DatExiste() As Boolean Implements IDATLET.DatExiste
            SyncLock _locker
                DatPrepara(datAlcances.select_procura)

                l_canal.Adaptador.Consulta(l_ac��o, Me)

                Return (l_ac��o.ResultadoAfectados > 0)
            End SyncLock
        End Function

        Public Overridable Overloads Function DatExiste(ByVal ParamArray Chave() As Object) As Boolean Implements IDATLET.DatExiste
            SyncLock _locker
                l_parametros = Chave
                DatExiste = (DatExiste())
                l_parametros = Nothing
            End SyncLock
        End Function

        Public Overridable Overloads Function DatVerifica() As Boolean Implements IDATLET.DatVerifica
            SyncLock _locker
                DatPrepara(datAlcances.select_verifica)

                l_canal.Adaptador.Consulta(l_ac��o, Me)

                Return (l_ac��o.ResultadoAfectados > 0)
            End SyncLock
        End Function

        Public Overridable Overloads Function DatVerifica(ByVal Ponteiro As Object) As Boolean Implements IDATLET.DatVerifica
            SyncLock _locker
                l_parametros = g10phPDF4.Basframe.Array.Def(Ponteiro)
                DatVerifica = (DatVerifica())
                l_parametros = Nothing
            End SyncLock
        End Function

        Public Overridable Function DatInsere() As Boolean Implements IDATLET.DatInsere
            SyncLock _locker
                DatPrepara(datAlcances.insert_ficha)

                Return l_canal.Adaptador.Executa(l_ac��o)
            End SyncLock
        End Function

        Public Overridable Function DatAltera() As Boolean Implements IDATLET.DatAltera
            SyncLock _locker
                DatPrepara(datAlcances.update_ficha)

                Return l_canal.Adaptador.Executa(l_ac��o)
            End SyncLock
        End Function

        Public Overridable Function DatElimina() As Boolean Implements IDATLET.DatElimina
            SyncLock _locker
                DatPrepara(datAlcances.delete_ficha)

                Return l_canal.Adaptador.Executa(l_ac��o)
            End SyncLock
        End Function

        Public Overridable Function DatAutoId() As Boolean Implements IDATLET.DatAutoId
            SyncLock _locker
                Dim ok As Boolean

                l_canal.Adaptador.Transa��oAbre()

                DatPrepara(datAlcances.select_maximo)

                If l_canal.Adaptador.ConsultaEscalar(l_ac��o) Then

                    If l_ac��o.ResultadoEscalar Is Nothing Then
                        DatAtributo(l_info.Objecto.Ponteiro) = 10
                    Else
                        DatAtributo(l_info.Objecto.Ponteiro) = CInt(l_ac��o.ResultadoEscalar) + 10
                    End If

                    DatPrepara(datAlcances.insert_registo)
                    ok = l_canal.Adaptador.Executa(l_ac��o)

                Else

                    ok = False

                End If

                l_canal.Adaptador.Transa��oCommit()

                Return ok
            End SyncLock
        End Function

        Public Overridable Function DatActualiza() As Boolean Implements IDATLET.DatActualiza
            SyncLock _locker
                Dim Update As Boolean

                If SystemDbTypeNumerico(l_info.Objecto.Membros(l_info.Objecto.Ponteiro).Tipo) Then

                    ' o ponteiro � num�rico:

                    If CInt(DatAtributo(l_info.Objecto.Ponteiro)) = 0 Then

                        ' n�o tem qq id:

                        If Not Me.DatAutoId Then

                            ' n�o foi poss�vel a autonumera��o!
                            Return False

                        Else

                            ' foi feita a autonumera��o, portanto foi reservado registo, portanto faremos UPDATE.
                            Update = True

                        End If

                    Else

                        ' o id j� foi atribuido, o registo j� existe, portanto faremos UPDATE.
                        Update = True

                    End If

                Else

                    ' o ponteiro n�o � num�rico:

                    If DatAtributo(Me.l_info.Objecto.Ponteiro) Is Nothing Then

                        ' o ponteiro � nulo e pq � alfanum�rico n�o podemos fazer autonumera��o, n�o podemos gravar nada!
                        Return False

                    Else

                        ' o id j� foi atribuido, faremos UPDATE se se verificar a sua existencia!
                        Update = Me.DatVerifica

                    End If

                End If

                If Update Then

                    Return Me.DatAltera()

                Else

                    Return Me.DatInsere()

                End If
            End SyncLock
        End Function

        Public Overridable Property DatFiltros() As datInscri��o() Implements IDATLET.DatFiltros
            Get
                SyncLock _locker
                    Return l_filtro_geral
                End SyncLock
            End Get
            Set(ByVal Value() As datInscri��o)
                SyncLock _locker
                    l_filtro_geral = Value
                    private_resetpreparados()
                End SyncLock
            End Set
        End Property


        Public Overridable Property DatExcep��o() As System.Exception Implements IDATLET.DatExcep��o
            Get
                SyncLock _locker
                    DatExcep��o = l_excep��o
                End SyncLock
            End Get
            Set(ByVal Value As System.Exception)
                SyncLock _locker
                    l_excep��o = Value
                End SyncLock
            End Set
        End Property

        Public Overridable Sub DatReset() Implements IDATLET.DatReset
            SyncLock _locker
                'n�o afecta "l_info"
                'n�o afecta "l_canal"
                l_excep��o = Nothing
                l_parametros = Nothing
                l_ac��o = Nothing
                private_resetpreparados()
                l_filtro_geral = Nothing
            End SyncLock
        End Sub

        Public Overridable Sub DatDispose() Implements IDATLET.DatDispose
            SyncLock _locker
                l_info = Nothing
                l_canal = Nothing
                l_excep��o = Nothing
                l_parametros = Nothing
                l_ac��o = Nothing
                l_preparados = Nothing
                l_filtro_geral = Nothing
            End SyncLock
        End Sub

#End Region

        Shared Function Cast2Tabela(ByVal Lista() As IDATLET, ByVal ParamArray Inscritos() As Integer) As System.Data.DataTable
            SyncLock _locker
                Dim dt As New System.Data.DataTable
                Dim l As Integer
                Dim i As Integer
                Dim linha() As Object

                If Not Lista Is Nothing Then

                    If Inscritos Is Nothing Then Inscritos = Lista(0).DatInfo.Objecto.ArrayTodos

                    ReDim linha(Inscritos.GetUpperBound(0))

                    For i = 0 To Inscritos.GetUpperBound(0)
                        dt.Columns.Add(Lista(0).DatInfo.Objecto.Membros(Inscritos(i)).Titulo)
                    Next

                    For l = 0 To Lista.GetUpperBound(0)

                        For i = 0 To Inscritos.GetUpperBound(0)

                            linha(i) = Lista(l).DatAtributo(Inscritos(i))

                        Next

                        dt.Rows.Add(linha)

                    Next

                    Return dt

                End If
            End SyncLock
        End Function

    End Class

    Public Class DatletIstancia
        Inherits DatletBase

        Private Shared ReadOnly _locker As New Object()

        Protected l_atributos() As Object

#Region "        Construtores/Destrutores"

        Public Sub New()

        End Sub

        Public Sub New(ByVal ConstrutorNulo As Boolean)

        End Sub

        Public Sub New(ByVal Info As datInfo)
            SyncLock _locker
                If Not Info Is Nothing Then Me.DatInfo = Info
            End SyncLock
        End Sub

        Public Sub New(ByVal Canal As datCanal)
            SyncLock _locker
                If Not Canal Is Nothing Then Me.DatCanal = Canal
            End SyncLock
        End Sub

        Public Sub New(ByVal Info As datInfo, ByVal Canal As datCanal)
            SyncLock _locker
                If Not Info Is Nothing Then Me.DatInfo = Info
                If Not Canal Is Nothing Then DatCanal = Canal
            End SyncLock
        End Sub

        Public Sub New(ByVal Info As datInfo, ByVal Construtor As ICONSTRUTOR, ByVal Adaptador As IADAPTADOR)
            SyncLock _locker
                If Not Info Is Nothing Then Me.DatInfo = Info
                If Not Construtor Is Nothing Then Me.l_canal.Construtor = Construtor
                If Not Adaptador Is Nothing Then Me.l_canal.Adaptador = Adaptador
            End SyncLock
        End Sub

        Public Sub New(ByVal DatletTemplate As IDATLET)
            SyncLock _locker
                Me.DatInfo = DatletTemplate.DatInfo
                Me.DatCanal = DatletTemplate.DatCanal
            End SyncLock
        End Sub

        Public Sub New(ByVal Objecto As datObjecto)
            SyncLock _locker
                Me.l_info = New datInfo
                Me.l_info.Objecto = Objecto
                Me.l_info.AtributoDef(Objecto)
                Me.private_resetatributos()
            End SyncLock
        End Sub

#End Region

#Region "        Suporte"

        Protected Sub private_resetatributos()
            SyncLock _locker
                If Not l_info.Objecto Is Nothing Then
                    If Not l_info.Atributos Is Nothing Then

                        Dim c As Integer

                        ReDim l_atributos(l_info.Atributos.GetUpperBound(0))

                        For c = 0 To l_atributos.GetUpperBound(0)
                            l_atributos(c) = Nothing
                        Next

                    End If
                End If
            End SyncLock
        End Sub

#End Region

#Region "        IDATLET DatletBase overrides"

        Public Overrides Property DatAtributo(ByVal Index As Integer) As Object
            Get
                SyncLock _locker
                    DatAtributo = l_atributos(Index)
                End SyncLock
            End Get
            Set(ByVal Value As Object)
                SyncLock _locker
                    l_atributos(Index) = Value
                End SyncLock
            End Set
        End Property

        Public Overrides Function DatCast(ByVal Lista() As ILET) As ILET()
            SyncLock _locker
                Dim r() As DatletIstancia
                Dim a As Integer

                If Lista Is Nothing Then
                    Return Nothing
                Else
                    ReDim r(Lista.GetUpperBound(0))

                    For a = 0 To Lista.GetUpperBound(0)

                        r(a) = DirectCast(Lista(a), DatletIstancia)

                    Next

                    Return r
                End If

                ' o CTYPE tem dificuldade (n�o consegue!) em converter arrays de datlet para o tipo especifico
                ' embora a mesma convers�o mas apenas de uma dessas posi��es de cada vez resulte com sucesso
            End SyncLock
        End Function

        Public Overrides Function DatClone() As ILET
            SyncLock _locker
                Dim the_clone As New DatletIstancia(False)
                the_clone.DatInfo = Me.l_info
                private_clone(the_clone)
                Return the_clone
            End SyncLock
        End Function

        Public Overrides Function DatNew() As ILET
            SyncLock _locker
                Return New DatletIstancia(False)
            End SyncLock
        End Function

        Public Overrides Property DatInfo() As datInfo
            Get
                SyncLock _locker
                    Return l_info
                End SyncLock
            End Get
            Set(ByVal Value As datInfo)
                SyncLock _locker
                    l_info = Value
                    private_resetatributos()
                    private_resetpreparados()
                End SyncLock
            End Set
        End Property

        Public Overrides Sub DatReset()
            SyncLock _locker
                MyBase.DatReset()
                private_resetatributos()
            End SyncLock
        End Sub

        Public Overrides Sub DatDispose()
            SyncLock _locker
                MyBase.DatDispose()
                l_atributos = Nothing
            End SyncLock
        End Sub

#End Region

    End Class

End Namespace
