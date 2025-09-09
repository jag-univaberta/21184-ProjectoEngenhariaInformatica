Namespace Dataframe

    Public Class datConstrutorSQL
        Implements ICONSTRUTOR

        Private Shared ReadOnly _locker As New Object()

        Public Function ConstroiSintaxe(ByVal Pedido As datPedido) As String Implements ICONSTRUTOR.ConstroiSintaxe
            SyncLock _locker
                Dim frase As String

                Pedido.ResolveComAlias(True)
                Pedido.Aceites = datAceitação.Novas(Pedido)

                Select Case Pedido.Operação

                    Case datOperações.Consulta
                        frase = Me.SintaxeDeConsulta(Pedido) & vbCrLf & SintaxeDeFiltro(Pedido) & vbCrLf & SintaxeDeOrdenação(Pedido)

                    Case datOperações.Inserção
                        frase = Me.SintaxeDeInserção(Pedido)

                    Case datOperações.Alteração
                        frase = Me.SintaxeDeAlteração(Pedido) & vbCrLf & SintaxeDeFiltro(Pedido)

                    Case datOperações.Eliminação
                        frase = Me.SintaxeDeEliminação(Pedido) & vbCrLf & SintaxeDeFiltro(Pedido)

                End Select

                Return frase
            End SyncLock
        End Function

        Public Function SintaxeDeConsulta(ByVal Pedido As datPedido) As String Implements ICONSTRUTOR.SintaxeDeConsulta
            SyncLock _locker
                Dim selecção As String
                Dim subpedidos As String
                Dim com As String

                selecção = MembrosAceites(Pedido)
                subpedidos = TabelasLigadas(Pedido)

                com = "SELECT " & selecção & vbCrLf & "FROM " & Pedido.Objecto.DbAlias & " " & Pedido.ComAlias

                Select Case Pedido.Lock

                    Case datLocks.Generico : com = com & " WITH (TABLOCK, HOLDLOCK) "
                    Case datLocks.Exclusivo : com = com & " WITH (TABLOCKX, HOLDLOCK) "

                End Select

                com = com & " " & vbCrLf & subpedidos

                Return com
            End SyncLock
        End Function

        Public Function SintaxeDeInserção(ByVal Pedido As datPedido) As String Implements ICONSTRUTOR.SintaxeDeInserção
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

        Public Function SintaxeDeAlteração(ByVal Pedido As datPedido) As String Implements ICONSTRUTOR.SintaxeDeAlteração
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

        Public Function SintaxeDeEliminação(ByVal Pedido As datPedido) As String Implements ICONSTRUTOR.SintaxeDeEliminação
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

                    Return "WHERE (" & SintaxeDeCondição(Pedido.Filtros, datPontes.NãoDefinido) & ")"

                End If
            End SyncLock
        End Function

        Public Function SintaxeDeOrdenação(ByVal Pedido As datPedido) As String Implements ICONSTRUTOR.SintaxeDeOrdenação
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

        Public Function SintaxeDeInscrição(ByVal Inscrito As datInscrição) As String Implements ICONSTRUTOR.SintaxeDeInscrição
            SyncLock _locker
                If Inscrito.FAgregada.Função = datFAgregadas.NãoDefinida Then

                    Return SintaxeDeMembro(Inscrito)

                Else

                    Return SintaxeDeFAgreagada(Inscrito)

                End If
            End SyncLock
        End Function

        Public Function SintaxeDeCondição(ByVal Inscritos() As datInscrição, ByVal ForçaPonte As datPontes) As String Implements ICONSTRUTOR.SintaxeDeCondição
            SyncLock _locker
                Dim i As Integer
                Dim frase As String
                Dim m1 As String
                Dim m2 As String

                For i = 0 To Inscritos.GetUpperBound(0)

                    If ForçaPonte <> datPontes.NãoDefinido And Inscritos(i).Ponte = datPontes.NãoDefinido Then
                        frase = frase & " " & SintaxeDePonte(ForçaPonte)
                    End If
                    frase = frase & SintaxeDePonte(Inscritos(i).Ponte)

                    If Inscritos(i).NegaAntes Then frase = frase & " NOT "

                    If Inscritos(i).PriorSobe > 0 Then frase = frase & g10phPDF4.Basframe.Strings.Repete("(", Inscritos(i).PriorSobe)

                    m1 = SintaxeDeInscrição(Inscritos(i))

                    If Inscritos(i).Parametro Is Nothing Then

                        m2 = Me.SintaxeDeParametroFiltro(Inscritos(i).Membro)

                    Else

                        If TypeOf Inscritos(i).Parametro Is datInscrição Then

                            m2 = SintaxeDeMembro(CType(Inscritos(i).Parametro, datInscrição))

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

        Public Function SintaxeDeFAgreagada(ByVal Inscrito As datInscrição) As String Implements ICONSTRUTOR.SintaxeDeFAgregada
            SyncLock _locker
                Dim i As Integer
                Dim p() As String
                Dim frase As String

                ' o primeiro parametro deve ser sempre o nome do membro

                If Inscrito.FAgregada.Parametros Is Nothing Then

                    ' não havendo parametros é acrescentado o nome do membro (onde tem que estar)
                    ReDim p(0)
                    p(0) = Inscrito.Pedido.ComAlias & "." & Inscrito.Membro.DbAlias

                Else

                    ReDim p(Inscrito.FAgregada.Parametros.GetUpperBound(0))

                    For i = 0 To p.GetUpperBound(0)

                        If TypeOf Inscrito.FAgregada.Parametros(i) Is datInscrição Then

                            p(i) = SintaxeDeInscrição(CType(Inscrito.FAgregada.Parametros(i), datInscrição))

                        Else

                            p(i) = CType(Inscrito.FAgregada.Parametros(i), String)

                        End If

                    Next

                    ' se o primeiro parametro não foi indicado é automaticamente o nome do membro
                    If p(0) Is Nothing Then p(0) = Inscrito.Pedido.ComAlias & "." & Inscrito.Membro.DbAlias

                End If

                '  os parametros são usados consoante a função indicada

                Select Case Inscrito.FAgregada.Função

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
                                g10phPDF4.Basframe.ErroLogger.DeuExcepção(New ExcepçãoConstruçãoSQLTipoDestinoNãoImplementado, CType(p(1), DbType).ToString, True, False, False)
                        End Select

                        Return "CONVERT(" & frase & "," & p(0) & ")"

                    Case datFAgregadas.Top

                        Return "TOP " & p(1) & " " & p(0)

                    Case Else

                        g10phPDF4.Basframe.ErroLogger.DeuExcepção(New ExcepçãoConstruçãoSQLFAgregadoNãoImplementado, Inscrito.FAgregada.Função.ToString, True, False, False)

                End Select

                Return frase
            End SyncLock
        End Function

        Public Function SintaxeDeMembro(ByVal Inscrito As datInscrição) As String Implements ICONSTRUTOR.SintaxeDeMembro
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

                        s = SintaxeDeInscrição(Pedido.Inscritos(i))
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

                If Not Pedido.Ordenações Is Nothing Then

                    For i = Pedido.Ordenações.GetLowerBound(0) To Pedido.Ordenações.GetUpperBound(0)

                        If frase.Length > 0 Then frase = frase & ", "
                        frase = frase & SintaxeDeMembro(Pedido.Ordenações(i))

                        Select Case Pedido.Ordenações(i).Direcção

                            Case datDirecções.Ascendente : frase = frase & " ASC"
                            Case datDirecções.Descendente : frase = frase & " DESC"

                        End Select

                    Next

                End If

                If Not Pedido.SubPedidos Is Nothing Then

                    For i = 0 To Pedido.SubPedidos.GetUpperBound(0)

                        If Not Pedido.SubPedidos(i).Ordenações Is Nothing Then

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

                        frase = frase & TabelasLigadas(Pedido.SubPedidos(j), Pedido.SubCondições(j))

                        If Not Pedido.SubPedidos(j).Filtros Is Nothing Then frase = frase & " " & SintaxeDeCondição(Pedido.SubPedidos(j).Filtros, datPontes.E)

                    Next

                    Return frase

                End If
            End SyncLock
        End Function

        Private Function TabelasLigadas(ByVal Pedido As datPedido, ByVal Condição() As datInscrição) As String
            SyncLock _locker
                Dim frase As String

                Select Case Pedido.Ligação

                    Case datLigações.LeftJoin : frase = frase & "LEFT JOIN "
                    Case datLigações.InnerJoin : frase = frase & "INNER JOIN "
                    Case datLigações.RightJoin : frase = frase & "RIGHT JOIN "
                    Case datLigações.FullJoin : frase = frase & "FULL OUTER JOIN "
                    Case datLigações.CrossJoin : frase = frase & "CROSS JOIN "

                End Select

                frase = frase & Pedido.Objecto.DbAlias & " " & Pedido.ComAlias

                If Not Pedido.SubPedidos Is Nothing Then
                    frase = frase & vbCrLf & TabelasLigadas(Pedido) & vbCrLf & g10phPDF4.Basframe.Strings.Repete(vbTab, Pedido.Nivel)
                Else
                    frase = frase & " "
                End If

                frase = frase & "ON " & SintaxeDeCondição(Condição, datPontes.NãoDefinido)

                Return frase
            End SyncLock
        End Function

    End Class

    Public Class datConstrutorXML
        Implements ICONSTRUTOR

        Private Shared ReadOnly _locker As New Object()

        Public Function ConstroiSintaxe(ByVal Pedido As datPedido) As String Implements ICONSTRUTOR.ConstroiSintaxe
            SyncLock _locker
                Pedido.Aceites = datAceitação.Novas(Pedido)
            End SyncLock
        End Function

        Public Function SintaxeDeAlteração(ByVal Pedido As datPedido) As String Implements ICONSTRUTOR.SintaxeDeAlteração

        End Function

        Public Function SintaxeDeCondição(ByVal Condições() As datInscrição, ByVal ForçaPonte As datPontes) As String Implements ICONSTRUTOR.SintaxeDeCondição

        End Function

        Public Function SintaxeDeConsulta(ByVal Pedido As datPedido) As String Implements ICONSTRUTOR.SintaxeDeConsulta

        End Function

        Public Function SintaxeDeEliminação(ByVal Pedido As datPedido) As String Implements ICONSTRUTOR.SintaxeDeEliminação

        End Function

        Public Function SintaxeDeFAgregada(ByVal Inscrito As datInscrição) As String Implements ICONSTRUTOR.SintaxeDeFAgregada

        End Function

        Public Function SintaxeDeFiltro(ByVal Pedido As datPedido) As String Implements ICONSTRUTOR.SintaxeDeFiltro

        End Function

        Public Function SintaxeDeInscrição(ByVal Inscrição As datInscrição) As String Implements ICONSTRUTOR.SintaxeDeInscrição

        End Function

        Public Function SintaxeDeInserção(ByVal Pedido As datPedido) As String Implements ICONSTRUTOR.SintaxeDeInserção

        End Function

        Public Function SintaxeDeMembro(ByVal Membro As datInscrição) As String Implements ICONSTRUTOR.SintaxeDeMembro

        End Function

        Public Function SintaxeDeOperador(ByVal Operador As datOperadores) As String Implements ICONSTRUTOR.SintaxeDeOperador

        End Function

        Public Function SintaxeDeOrdenação(ByVal Pedido As datPedido) As String Implements ICONSTRUTOR.SintaxeDeOrdenação

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

        Public Conexão As System.Data.IDbConnection
        Public Transação As System.Data.IDbTransaction

        Public Erro As Boolean
        Public ErroAvisa As Boolean
        Public ErroResposta As MsgBoxResult

        Private l_excepção As System.Exception

        Private Shared ReadOnly _locker As New Object()

        Public Sub New()

        End Sub

        Public Sub New(ByVal conexão As System.Data.IDbConnection)
            SyncLock _locker
                Me.Conexão = conexão
            End SyncLock
        End Sub

        Public Overridable Property Excepção() As System.Exception Implements IADAPTADOR.Excepção
            Get
                SyncLock _locker
                    Return l_excepção
                End SyncLock
            End Get
            Set(ByVal Value As System.Exception)
                SyncLock _locker
                    l_excepção = Value
                End SyncLock
            End Set
        End Property


        Public Function Consulta(ByRef Acção As datAcção, ByVal DatletACarregar As IDATLET) As Boolean Implements IADAPTADOR.Consulta
            SyncLock _locker
                Dim Cursor As System.Data.IDataReader

                Consulta = b_select(Acção, Cursor, False)

                If Consulta Then

                    If Cursor.Read Then

                        b_recolhe(DatletACarregar, Acção, Cursor, 0)
                        Acção.ResultadoAfectados = 1

                    Else

                        Acção.ResultadoAfectados = 0

                    End If

                    Cursor.Close()

                Else

                    Acção.ResultadoAfectados = 0

                End If
            End SyncLock
        End Function

        Public Function ConsultaEscalar(ByRef Acção As datAcção) As Boolean Implements IADAPTADOR.ConsultaEscalar
            SyncLock _locker
                ConsultaEscalar = b_select(Acção, Nothing, True)

                If ConsultaEscalar And Not Acção.ResultadoEscalar Is Nothing Then

                    Acção.ResultadoAfectados = 1

                Else

                    Acção.ResultadoAfectados = 0

                End If
            End SyncLock
        End Function

        Public Function Executa(ByRef Acção As datAcção) As Boolean Implements IADAPTADOR.Executa
            SyncLock _locker
                Return b_exec(Acção)
            End SyncLock
        End Function

        Public Function Selecciona(ByRef Acção As datAcção, ByVal DatletAClonar As IDATLET) As IDATLET() Implements IADAPTADOR.Selecciona
            SyncLock _locker
                Dim cursor As System.Data.IDataReader
                Dim resultados() As IDATLET
                Dim l As Integer
                Dim sucesso As Boolean

                Acção.ResultadoAfectados = 0
                sucesso = b_select(Acção, cursor, False)

                If sucesso Then

                    l = -1
                    While cursor.Read

                        l += 1
                        ReDim Preserve resultados(l)

                        resultados(l) = CType(DatletAClonar.DatClone, IDATLET)
                        b_recolhe(resultados(l), Acção, cursor, 0)

                    End While

                    cursor.Close()

                    Acção.ResultadoAfectados = l + 1
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

        Public Function TransaçãoAbre() As Boolean Implements IADAPTADOR.TransaçãoAbre
            SyncLock _locker
                Transação = Conexão.BeginTransaction
                Return (Not Transação Is Nothing)
            End SyncLock
        End Function

        Public Function TransaçãoCommit() As Boolean Implements IADAPTADOR.TransaçãoCommit
            SyncLock _locker
                Transação.Commit()
                Transação = Nothing
                Return True
            End SyncLock
        End Function

        Public Function TransaçãoRollback() As Boolean Implements IADAPTADOR.TransaçãoRollback
            SyncLock _locker
                Transação.Rollback()
                Transação = Nothing
                Return True
            End SyncLock
        End Function


        Public Function b_select(ByRef Acção As datAcção, ByRef getCursor As System.Data.IDataReader, ByVal Escalar As Boolean) As Boolean
            SyncLock _locker
                Dim Comando As System.Data.IDbCommand

                Comando = Conexão.CreateCommand
                Comando.CommandType = CommandType.Text
                Comando.CommandText = Acção.Sintaxe
                If Not Me.Transação Is Nothing Then Comando.Transaction = Me.Transação

                b_parametriza(Acção, Comando, Acção.Filtros, "", True, False, Acção.ParametrosFiltro)

                Do

                    Erro = False
                    ErroResposta = MsgBoxResult.Ok
                    Me.Excepção = Nothing

                    Try

                        If Escalar Then
                            Acção.ResultadoEscalar = Comando.ExecuteScalar
                            Return (Not Acção.ResultadoEscalar Is Nothing)
                        Else
                            getCursor = Comando.ExecuteReader
                            Return (Not getCursor Is Nothing)
                        End If

                    Catch excepção_ao_comando As System.Exception

                        Erro = True
                        Me.Excepção = excepção_ao_comando

                        ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcepção(excepção_ao_comando, Comando.CommandText, ErroAvisa, True, False)

                    End Try

                Loop While ErroResposta = MsgBoxResult.Retry

                Return False
            End SyncLock
        End Function

        Public Function b_exec(ByRef Acção As datAcção) As Boolean
            SyncLock _locker
                Dim Comando As System.Data.IDbCommand
                Dim Cursor As System.Data.IDataReader

                Comando = Conexão.CreateCommand
                Comando.CommandType = CommandType.Text
                Comando.CommandText = Acção.Sintaxe
                If Not Me.Transação Is Nothing Then Comando.Transaction = Me.Transação

                If Acção.Pedido.Operação = datOperações.Alteração Or Acção.Pedido.Operação = datOperações.Inserção Then
                    b_parametriza(Acção, Comando, Acção.Inscritos, "", False, True, Acção.ParametrosValor)
                End If

                If Acção.Pedido.Operação = datOperações.Alteração Or Acção.Pedido.Operação = datOperações.Eliminação Then
                    b_parametriza(Acção, Comando, Acção.Filtros, "", False, False, Acção.ParametrosFiltro)
                End If

                Do

                    ErroResposta = MsgBoxResult.Ok
                    Me.Excepção = Nothing
                    Erro = False

                    Try

                        Acção.ResultadoAfectados = Comando.ExecuteNonQuery
                        Return True

                    Catch execepção_ao_comando As System.Exception

                        Erro = True
                        Me.Excepção = execepção_ao_comando
                        Acção.ResultadoAfectados = 0

                        ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcepção(execepção_ao_comando, Comando.CommandText, ErroAvisa, True, False)

                    End Try

                Loop While ErroResposta = MsgBoxResult.Retry

                Return False
            End SyncLock
        End Function

        Public Sub b_recolhe(ByVal Datlet As IDATLET, ByRef Acção As datAcção, ByVal Cursor As System.Data.IDataReader, ByRef JoinIndex As Integer)
            SyncLock _locker
                Dim i As Integer        ' incrição a ler
                Dim m As Integer        ' membro que a inscrição representa
                Dim ji As Integer       ' indice do join da presente recolha

                Dim getdt As IDATLET

                ji = JoinIndex

                For i = 0 To Acção.Pedido.Aceites.GetUpperBound(0)

                    If Acção.Pedido.Aceites(i).JoinNivel = ji Then

                        m = Acção.Pedido.Aceites(i).Membro.Ordem

                        If Cursor.IsDBNull(i) Then
                            Datlet.DatAtributo(m) = Nothing
                        Else
                            Select Case Acção.Pedido.Aceites(i).Membro.Tipo
                                Case DbType.Int32 : Datlet.DatAtributo(m) = Cursor.GetInt32(i)
                                Case DbType.Decimal : Datlet.DatAtributo(m) = Cursor.GetDecimal(i)
                                Case DbType.Date : Datlet.DatAtributo(m) = Cursor.GetDateTime(i)
                                Case DbType.Time : Datlet.DatAtributo(m) = Cursor.GetDateTime(i)
                                Case DbType.Boolean : Datlet.DatAtributo(m) = Cursor.GetBoolean(i)
                                Case Else : Datlet.DatAtributo(m) = Cursor.GetString(i)
                            End Select
                        End If

                        ' este membro referencia um membro de outro objecto?
                        If Not Acção.Pedido.Aceites(i).Membro.Referencia Is Nothing Then

                            ' Pq é necessário contar as ligações que vão sendo feitas por membros com referencias, o JoinIndex
                            ' é incrementado antes de tratar a referencia e é passado por byref de forma a, que se a referencia
                            ' contiver ele própria outras referencias, sabermos do numero total de ligações.

                            JoinIndex = JoinIndex + 1

                            Me.b_recolhe(Acção.Pedido.Aceites(i).Membro.Referencia, Acção, Cursor, JoinIndex)

                            '' temos uma datlet para apanhar os resultados de outro objecto?
                            'If Datlet.DatReferencia(Acção.Pedido.Aceites(i).Membro, getdt, Nothing) Then

                            '    ' apanhar resultados do outro objecto
                            '    Me.b_recolhe(getdt, Acção, Cursor, JoinIndex)

                            'End If

                        End If
                    End If

                Next
            End SyncLock
        End Sub

        Public Function b_parametriza(ByRef Acção As datAcção, ByVal Comando As System.Data.IDbCommand, ByVal Inscritos() As Integer, ByRef getsetBufferParametrizados As String, ByVal ResetParametrização As Boolean, Optional ByVal Setting As Boolean = True, Optional ByVal Variaveis As Object() = Nothing) As Boolean
            SyncLock _locker
                Dim parametros() As System.Data.IDataParameter
                Dim i As Integer
                Dim m As Integer

                If ResetParametrização Then

                    Comando.Parameters.Clear()
                    getsetBufferParametrizados = ""

                End If

                If Inscritos Is Nothing Then

                    Return False

                Else

                    ReDim parametros(Inscritos.GetUpperBound(0))

                    For i = 0 To Inscritos.GetUpperBound(0)

                        m = Inscritos(i)

                        ' verificar se um membro já foi parametrizado
                        ' tenho que verificar porque este pode aparecer várias vezes nas inscrições (Funções; Concatenações;)
                        ' pensar: datAdaptadorDB: não gosto desta forma de verificar se um membro já foi parametrizado!

                        If getsetBufferParametrizados.IndexOf(m & ",") = -1 Then

                            getsetBufferParametrizados = getsetBufferParametrizados & m & ","

                            parametros(i) = Comando.CreateParameter
                            parametros(i).DbType = Acção.Pedido.Objecto.Membros(m).Tipo

                            If Setting Then
                                parametros(i).ParameterName = Acção.Construtor.SintaxeDeParametroValor(Acção.Pedido.Objecto.Membros(m))
                            Else
                                parametros(i).ParameterName = Acção.Construtor.SintaxeDeParametroFiltro(Acção.Pedido.Objecto.Membros(m))
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

        ' consulta: se não há filtros lê toda a estrutura, se há filtros filtra pelo conteudo dos respectivos atributos
        ' inserção: se já existe altera o respectivo nó, senão insere um novo.
        ' alteração: se já existe altera o respectivo nó, senão insere um novo.
        ' eliminação: se existe elimina o nó da estrutura.
        ' "filtros" indicam os atributos e conteudos para load, save e delete.
        ' "inscritos" indicam quais os campos a ler, alterar ou a inserir.
        ' "ordens" indica quais os atributos pelos quais se deve ordenar uma consulta.
        ' "aceites" em acções xml nunca será usado.

        Public FicheiroPath As String
        Public NoBase As System.Xml.XmlNode

        ' a prioridade no destino é dada ao NoBase e só se este for omitido se abre o FicheiroPath

        Public Erro As Boolean
        Public ErroAvisa As Boolean
        Public ErroResposta As MsgBoxResult

        Private l_excepção As System.Exception

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


        Public Overridable Property Excepção() As System.Exception Implements IADAPTADOR.Excepção
            Get
                SyncLock _locker
                    Return l_excepção
                End SyncLock
            End Get
            Set(ByVal Value As System.Exception)
                SyncLock _locker
                    l_excepção = Value
                End SyncLock
            End Set
        End Property


        Public Function Consulta(ByRef Acção As datAcção, ByVal DatletACarregar As IDATLET) As Boolean Implements IADAPTADOR.Consulta
            SyncLock _locker
                ' executa as selecções de um só registo

                Return x_consulta(Acção, False, DatletACarregar)
            End SyncLock
        End Function

        Public Function ConsultaEscalar(ByRef Acção As datAcção) As Boolean Implements IADAPTADOR.ConsultaEscalar
            SyncLock _locker
                ' executa as selecções esclares

                Acção.ResultadoEscalar = Nothing
                Return x_consulta(Acção, True, Nothing)
            End SyncLock
        End Function

        Public Function Selecciona(ByRef Acção As datAcção, ByVal DatletAClonar As IDATLET) As IDATLET() Implements IADAPTADOR.Selecciona
            SyncLock _locker
                ' executa as selecções de n registos

                Return x_consulta_varios(Acção, DatletAClonar)
            End SyncLock
        End Function

        Public Function Executa(ByRef Acção As datAcção) As Boolean Implements IADAPTADOR.Executa
            SyncLock _locker
                Select Case Acção.Pedido.Operação

                    Case datOperações.Alteração : Return x_altera(Acção)

                    Case datOperações.Inserção : Return x_insere(Acção)

                    Case datOperações.Eliminação : Return x_altera(Acção)

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

        Public Function TransaçãoAbre() As Boolean Implements IADAPTADOR.TransaçãoAbre

            ' todo: datAdptadorXML: TransaçãoAbre

        End Function

        Public Function TransaçãoCommit() As Boolean Implements IADAPTADOR.TransaçãoCommit

            ' todo: datAdptadorXML: TransaçãoCommit

        End Function

        Public Function TransaçãoRollback() As Boolean Implements IADAPTADOR.TransaçãoRollback

            ' todo: datAdptadorXML: TransaçãoRollback

        End Function



        Public Function x_altera(ByRef Acção As datAcção) As Boolean
            SyncLock _locker
                Dim xmldoc As System.Xml.XmlDocument
                Dim no_datlet As System.Xml.XmlNode
                Dim no_selecção() As System.Xml.XmlNode
                Dim no_registo As System.Xml.XmlNode

                Acção.ResultadoAfectados = 0

                If x_getnodatlet(Acção, xmldoc, no_datlet) Then

                    no_selecção = x_select(Acção, no_datlet, False, False)

                    For Each no_registo In no_selecção

                        x_checkmembros(Acção, no_registo)
                        x_escreve(Acção, no_registo)
                        Acção.ResultadoAfectados = Acção.ResultadoAfectados + 1

                    Next

                    If Acção.ResultadoAfectados > 0 And Not xmldoc Is Nothing Then xmldoc.Save(FicheiroPath)

                    Return True

                Else

                    Return False

                End If
            End SyncLock
        End Function

        Public Function x_insere(ByRef Acção As datAcção) As Boolean
            SyncLock _locker
                Dim xmldoc As System.Xml.XmlDocument
                Dim no_datlet As System.Xml.XmlNode
                Dim no_registo As System.Xml.XmlNode

                Acção.ResultadoAfectados = 0

                If x_getnodatlet(Acção, xmldoc, no_datlet) Then

                    no_registo = x_criano(Acção, no_datlet)
                    no_datlet.AppendChild(no_registo)
                    x_escreve(Acção, no_registo)
                    Acção.ResultadoAfectados = 1

                    If Not xmldoc Is Nothing Then xmldoc.Save(FicheiroPath)

                    Return True

                Else

                    Return False

                End If
            End SyncLock
        End Function

        Public Function x_elimina(ByVal Datlet As IDATLET, ByRef Acção As datAcção) As Boolean
            SyncLock _locker
                Dim xmldoc As System.Xml.XmlDocument
                Dim no_datlet As System.Xml.XmlNode
                Dim no_selecção() As System.Xml.XmlNode
                Dim no_registo As System.Xml.XmlNode

                Acção.ResultadoAfectados = 0

                If x_getnodatlet(Acção, xmldoc, no_datlet) Then

                    no_selecção = x_select(Acção, no_datlet, False, False)

                    For Each no_registo In no_selecção

                        no_datlet.RemoveChild(no_registo)

                        Acção.ResultadoAfectados = Acção.ResultadoAfectados + 1

                    Next

                    If Acção.ResultadoAfectados > 0 And Not xmldoc Is Nothing Then xmldoc.Save(FicheiroPath)

                    Return True

                Else

                    Return False

                End If
            End SyncLock
        End Function

        Public Function x_consulta(ByRef Acção As datAcção, ByVal Escalar As Boolean, ByVal DatletACarregar As IDATLET) As Boolean
            SyncLock _locker
                Dim xmldoc As System.Xml.XmlDocument
                Dim no_datlet As System.Xml.XmlNode
                Dim no_selecção() As System.Xml.XmlNode

                Acção.ResultadoAfectados = 0

                If x_getnodatlet(Acção, xmldoc, no_datlet) Then

                    no_selecção = x_select(Acção, no_datlet, False, Escalar)

                    If Not no_selecção Is Nothing Then

                        x_le(Acção, no_selecção(0), Escalar, DatletACarregar)
                        Acção.ResultadoAfectados = 1

                    End If

                    Return True

                Else

                    Return False

                End If
            End SyncLock
        End Function

        Public Function x_consulta_varios(ByRef Acção As datAcção, ByVal DatletAClonar As IDATLET) As IDATLET()
            SyncLock _locker
                Dim xmldoc As System.Xml.XmlDocument
                Dim no_datlet As System.Xml.XmlNode
                Dim no_selecção() As System.Xml.XmlNode
                Dim no_registo As System.Xml.XmlNode
                Dim result() As IDATLET
                Dim r As Integer

                Acção.ResultadoAfectados = 0

                If x_getnodatlet(Acção, xmldoc, no_datlet) Then

                    no_selecção = x_select(Acção, no_datlet, False, False)

                    ReDim result(no_selecção.GetUpperBound(0))

                    For Each no_registo In no_selecção

                        result(r) = CType(DatletAClonar.DatClone, IDATLET)
                        x_le(Acção, no_registo, False, result(r))
                        r = r + 1

                        Acção.ResultadoAfectados = Acção.ResultadoAfectados + 1

                    Next

                End If

                Return result
            End SyncLock
        End Function

        Public Function x_getnodatlet(ByRef Acção As datAcção, ByRef getXmlDoc As System.Xml.XmlDocument, ByRef getNoDatlet As System.Xml.XmlNode) As Boolean
            SyncLock _locker
                Do

                    Erro = False
                    Me.Excepção = Nothing
                    getNoDatlet = XMLGetNo(NoBase, FicheiroPath, Acção.Pedido.Objecto.DbGrupo, (Acção.Pedido.Operação = datOperações.Inserção), getXmlDoc, Me.Excepção)

                    If Me.Excepção Is Nothing Then

                        Return True

                    Else

                        Erro = True

                        ErroResposta = g10phPDF4.Basframe.ErroLogger.DeuExcepção(Me.Excepção, FicheiroPath, ErroAvisa, True, False)

                    End If

                Loop While ErroResposta = MsgBoxResult.Retry

                Return False
            End SyncLock
        End Function

        Public Function x_select(ByRef Acção As datAcção, ByVal no_datlet As System.Xml.XmlNode, ByVal SóOPrimeiro As Boolean, ByVal Escalar As Boolean) As System.Xml.XmlNode()
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

                        If Acção.Pedido.Filtros Is Nothing Then

                            coincide = True

                        Else

                            For f = 0 To Acção.Pedido.Filtros.GetUpperBound(0)

                                a = Acção.Filtros(f)
                                n = no_registo.Item(Acção.Pedido.Objecto.Membros(a).Nome)

                                ' verificar se o membro está presente no nó

                                If n Is Nothing Then
                                    coincide = False
                                Else

                                    If n.InnerText = CType(Acção.ParametrosFiltro(f), String) Then
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

        Public Function x_criano(ByRef Acção As datAcção, ByVal no_datlet As System.Xml.XmlNode) As System.Xml.XmlNode
            SyncLock _locker
                Dim no_registo As System.Xml.XmlNode
                Dim a As Integer
                Dim m As Integer

                no_registo = no_datlet.OwnerDocument.CreateElement(Acção.Pedido.Objecto.Nome)

                For a = 0 To Acção.Pedido.Aceites.GetUpperBound(0)

                    m = Acção.Aceites(a)
                    no_registo.AppendChild(no_datlet.OwnerDocument.CreateElement(Acção.Pedido.Objecto.Membros(m).Nome))

                Next

                Return no_registo
            End SyncLock
        End Function

        Public Sub x_checkmembros(ByRef Acção As datAcção, ByVal no_registo As System.Xml.XmlNode)
            SyncLock _locker
                ' para cada aceite verificar se existe no estrutura

                Dim a As Integer
                Dim m As Integer
                Dim no_membro As System.Xml.XmlNode

                For a = 0 To Acção.Pedido.Aceites.GetUpperBound(0)

                    m = Acção.Aceites(a)

                    no_membro = no_registo.Item(Acção.Pedido.Objecto.Membros(m).Nome)

                    If no_membro Is Nothing Then
                        ' membro não existe na estrutura
                        no_registo.AppendChild(no_registo.OwnerDocument.CreateElement(Acção.Pedido.Objecto.Membros(m).Nome))
                    End If

                    no_membro = Nothing

                Next
            End SyncLock
        End Sub

        Public Sub x_escreve(ByRef Acção As datAcção, ByVal no_registo As System.Xml.XmlNode)
            SyncLock _locker
                Dim a As Integer
                Dim m As Integer

                For a = 0 To Acção.Inscritos.GetUpperBound(0)

                    m = Acção.Inscritos(a)
                    no_registo.Item(Acção.Pedido.Objecto.Membros(m).Nome).InnerText = CType(Acção.ParametrosValor(a), String)

                Next
            End SyncLock
        End Sub

        Public Sub x_le(ByRef Acção As datAcção, ByVal no_registo As System.Xml.XmlNode, ByVal Escalar As Boolean, ByVal DatletACarregar As IDATLET)
            SyncLock _locker
                Dim a As Integer
                Dim m As Integer
                Dim n As System.Xml.XmlNode

                For a = 0 To Acção.Aceites.GetUpperBound(0)

                    m = Acção.Aceites(a)
                    n = no_registo.Item(Acção.Pedido.Objecto.Membros(m).Nome)

                    If n Is Nothing Then
                        DatletACarregar.DatAtributo(m) = Nothing
                    Else
                        DatletACarregar.DatAtributo(m) = n.InnerText
                    End If

                    If Escalar Then
                        Acção.ResultadoEscalar = n.InnerText
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

        Public Sub New(ByVal Mania As datManias, ByVal conexão As System.Data.IDbConnection)
            SyncLock _locker
                Select Case Mania

                    Case datManias.SqlServer : Me.Construtor = New datConstrutorSQL

                End Select

                Me.Adaptador = New datAdaptadorDB(conexão)
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


        ' todo: DatletBase: seleccionar vários registos a partir e para a mesma datlet
        ' todo: DatletBase: arranjar forma de depois da acção executada retornar a excepção para a datlet



        Protected l_info As datInfo
        Protected l_canal As New datCanal
        Protected l_excepção As System.Exception
        Protected l_parametros() As Object
        Protected l_preparados() As datAcção
        Protected l_acção As datAcção
        Protected l_filtro_geral() As datInscrição

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
                    .l_excepção = Me.l_excepção
                    ' não clonamos os parametros 
                    'If Not Me.l_parametros Is Nothing Then .l_parametros = CType(Me.l_parametros.Clone, Object())
                    If Not Me.l_preparados Is Nothing Then .l_preparados = CType(Me.l_preparados.Clone, datAcção())
                    .l_acção = Me.l_acção
                    ' não clonamos o filtro geral
                    'If Not Me.l_filtro_geral Is Nothing Then .l_filtro_geral = CType(Me.l_filtro_geral.Clone, datInscrição())

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
                        pedido.Operação = datOperações.Consulta
                        pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosTodos))
                        pedido.MaisFiltros(datInscrição.Novas(l_info.Objecto.MembrosChave))
                        pedido.Ordenações = Nothing

                    Case datAlcances.select_registo
                        pedido.Operação = datOperações.Consulta
                        pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosTodos))
                        pedido.MaisFiltros(datInscrição.Novas(l_info.Objecto.MembrosPonteiro))
                        pedido.Ordenações = Nothing

                    Case datAlcances.select_lista
                        pedido.Operação = datOperações.Consulta
                        pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosLista))
                        pedido.Filtros = Nothing
                        pedido.MaisOrdenações(datInscrição.Novas(l_info.Objecto.MembrosChave))

                    Case datAlcances.select_fichas
                        pedido.Operação = datOperações.Consulta
                        pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosTodos))
                        pedido.Filtros = Nothing
                        pedido.MaisOrdenações(datInscrição.Novas(l_info.Objecto.MembrosChave))

                    Case datAlcances.select_procura
                        pedido.Operação = datOperações.Consulta
                        pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosPonteiro))
                        pedido.MaisFiltros(datInscrição.Novas(l_info.Objecto.MembrosChave))
                        pedido.Ordenações = Nothing

                    Case datAlcances.select_vista
                        pedido.Operação = datOperações.Consulta
                        pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosVista))
                        pedido.MaisFiltros(datInscrição.Novas(l_info.Objecto.MembrosPonteiro))
                        pedido.Ordenações = Nothing

                    Case datAlcances.select_maximo
                        pedido.Operação = datOperações.Consulta
                        pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembroPonteiro, datFAgregadas.Maximo))
                        pedido.Filtros = Nothing
                        pedido.Ordenações = Nothing
                        pedido.Lock = datLocks.Exclusivo

                    Case datAlcances.update_ficha
                        pedido.Operação = datOperações.Alteração
                        pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosTodos))
                        pedido.MaisFiltros(datInscrição.Novas(l_info.Objecto.MembrosPonteiro))
                        pedido.Ordenações = Nothing

                    Case datAlcances.insert_ficha
                        pedido.Operação = datOperações.Inserção
                        pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosTodos))
                        pedido.MaisFiltros(datInscrição.Novas(l_info.Objecto.MembrosTodos))
                        pedido.Ordenações = Nothing

                    Case datAlcances.delete_ficha
                        pedido.Operação = datOperações.Eliminação
                        pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosTodos))
                        pedido.MaisFiltros(datInscrição.Novas(l_info.Objecto.MembrosPonteiro))
                        pedido.Ordenações = Nothing

                    Case datAlcances.select_primeiro
                        pedido.Operação = datOperações.Consulta
                        pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosTodos))
                        pedido.Inscritos(0).FAgregada = New datFunção(datFAgregadas.Top, 1)
                        pedido.Filtros = Nothing
                        pedido.Ordenações = Nothing

                    Case datAlcances.select_pesquisa
                        pedido.Operação = datOperações.Consulta
                        pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosPesquisa))
                        Dim d As Integer
                        Dim a As Integer
                        For d = 0 To l_info.Objecto.MembrosDescrição.GetUpperBound(0)
                            a = l_info.Objecto.MembrosDescrição(d).Ordem
                            If Not Me.DatAtributo(a) Is Nothing Or Not l_parametros(d) Is Nothing Then pedido.MaisFiltros(datInscrição.Novas(l_info.Objecto.MembrosDescrição(d), datOperadores.Semelhante, Nothing))
                        Next
                        pedido.MaisOrdenações(datInscrição.Novas(l_info.Objecto.MembrosChave))

                    Case datAlcances.select_verifica
                        pedido.Operação = datOperações.Consulta
                        pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosPonteiro))
                        pedido.MaisFiltros(datInscrição.Novas(l_info.Objecto.MembroPonteiro))
                        pedido.Ordenações = Nothing

                    Case datAlcances.insert_registo
                        pedido.Operação = datOperações.Inserção
                        pedido.MaisInscritos(datInscrição.Novas(l_info.Objecto.MembrosPonteiro))
                        pedido.MaisFiltros(datInscrição.Novas(l_info.Objecto.MembrosPonteiro))
                        pedido.Ordenações = Nothing

                End Select

                If Not l_filtro_geral Is Nothing Then pedido.MaisFiltros(l_filtro_geral)
                pedido.AutoCompleta()
            End SyncLock
        End Sub

        Protected Overridable Sub private_preparaacção(ByRef acção As datAcção)
            SyncLock _locker
                acção.PreparaAcção(l_canal.Construtor)
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

        Public Overridable Property DatAcção() As datAcção Implements IDATLET.DatAcção
            Get
                SyncLock _locker
                    Return l_acção
                End SyncLock
            End Get
            Set(ByVal Value As datAcção)
                SyncLock _locker

                    l_acção = Value
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

                    private_preparaacção(l_preparados(Alcance))

                End If

                l_acção = l_preparados(Alcance)

                If Not l_acção.Inscritos Is Nothing Then l_acção.ParametrosValor = private_atributosarray(l_acção.Inscritos)

                If Me.l_parametros Is Nothing Then
                    If Not l_acção.Filtros Is Nothing Then l_acção.ParametrosFiltro = private_atributosarray(l_acção.Filtros)
                Else
                    l_acção.ParametrosFiltro = l_parametros
                End If

                l_acção.Reset()
            End SyncLock
        End Sub

        Public Overridable Overloads Function DatConsultaFicha() As Boolean Implements IDATLET.DatConsultaFicha
            SyncLock _locker
                DatPrepara(datAlcances.select_ficha)

                If l_canal.Adaptador.Consulta(l_acção, Me) Then

                    Return (l_acção.ResultadoAfectados > 0)

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

                If l_canal.Adaptador.Consulta(l_acção, Me) Then

                    Return (l_acção.ResultadoAfectados > 0)

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

                If l_canal.Adaptador.Consulta(l_acção, Me) Then

                    Return (l_acção.ResultadoAfectados > 0)

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

                Return l_canal.Adaptador.Selecciona(l_acção, Me)
            End SyncLock
        End Function

        Public Overridable Function DatSeleccionaLista() As IDATLET() Implements IDATLET.DatSeleccionaLista
            SyncLock _locker
                DatPrepara(datAlcances.select_lista)

                Return l_canal.Adaptador.Selecciona(l_acção, Me)
            End SyncLock
        End Function

        Public Overridable Function DatConsultaPrimeiro() As Boolean Implements IDATLET.DatConsultaPrimeiro
            SyncLock _locker
                DatPrepara(datAlcances.select_primeiro)

                If l_canal.Adaptador.Consulta(l_acção, Me) Then

                    Return (l_acção.ResultadoAfectados > 0)

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

                Return l_canal.Adaptador.Selecciona(l_acção, Me)
            End SyncLock
        End Function

        Public Overridable Overloads Function DatPesquisa(ByVal ParamArray Descrição() As Object) As IDATLET() Implements IDATLET.DatPesquisa
            SyncLock _locker
                l_parametros = Descrição

                DatPesquisa = (DatPesquisa())

                l_parametros = Nothing
            End SyncLock
        End Function

        Public Overridable Overloads Function DatExiste() As Boolean Implements IDATLET.DatExiste
            SyncLock _locker
                DatPrepara(datAlcances.select_procura)

                l_canal.Adaptador.Consulta(l_acção, Me)

                Return (l_acção.ResultadoAfectados > 0)
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

                l_canal.Adaptador.Consulta(l_acção, Me)

                Return (l_acção.ResultadoAfectados > 0)
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

                Return l_canal.Adaptador.Executa(l_acção)
            End SyncLock
        End Function

        Public Overridable Function DatAltera() As Boolean Implements IDATLET.DatAltera
            SyncLock _locker
                DatPrepara(datAlcances.update_ficha)

                Return l_canal.Adaptador.Executa(l_acção)
            End SyncLock
        End Function

        Public Overridable Function DatElimina() As Boolean Implements IDATLET.DatElimina
            SyncLock _locker
                DatPrepara(datAlcances.delete_ficha)

                Return l_canal.Adaptador.Executa(l_acção)
            End SyncLock
        End Function

        Public Overridable Function DatAutoId() As Boolean Implements IDATLET.DatAutoId
            SyncLock _locker
                Dim ok As Boolean

                l_canal.Adaptador.TransaçãoAbre()

                DatPrepara(datAlcances.select_maximo)

                If l_canal.Adaptador.ConsultaEscalar(l_acção) Then

                    If l_acção.ResultadoEscalar Is Nothing Then
                        DatAtributo(l_info.Objecto.Ponteiro) = 10
                    Else
                        DatAtributo(l_info.Objecto.Ponteiro) = CInt(l_acção.ResultadoEscalar) + 10
                    End If

                    DatPrepara(datAlcances.insert_registo)
                    ok = l_canal.Adaptador.Executa(l_acção)

                Else

                    ok = False

                End If

                l_canal.Adaptador.TransaçãoCommit()

                Return ok
            End SyncLock
        End Function

        Public Overridable Function DatActualiza() As Boolean Implements IDATLET.DatActualiza
            SyncLock _locker
                Dim Update As Boolean

                If SystemDbTypeNumerico(l_info.Objecto.Membros(l_info.Objecto.Ponteiro).Tipo) Then

                    ' o ponteiro é numérico:

                    If CInt(DatAtributo(l_info.Objecto.Ponteiro)) = 0 Then

                        ' não tem qq id:

                        If Not Me.DatAutoId Then

                            ' não foi possível a autonumeração!
                            Return False

                        Else

                            ' foi feita a autonumeração, portanto foi reservado registo, portanto faremos UPDATE.
                            Update = True

                        End If

                    Else

                        ' o id já foi atribuido, o registo já existe, portanto faremos UPDATE.
                        Update = True

                    End If

                Else

                    ' o ponteiro não é numérico:

                    If DatAtributo(Me.l_info.Objecto.Ponteiro) Is Nothing Then

                        ' o ponteiro é nulo e pq é alfanumérico não podemos fazer autonumeração, não podemos gravar nada!
                        Return False

                    Else

                        ' o id já foi atribuido, faremos UPDATE se se verificar a sua existencia!
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

        Public Overridable Property DatFiltros() As datInscrição() Implements IDATLET.DatFiltros
            Get
                SyncLock _locker
                    Return l_filtro_geral
                End SyncLock
            End Get
            Set(ByVal Value() As datInscrição)
                SyncLock _locker
                    l_filtro_geral = Value
                    private_resetpreparados()
                End SyncLock
            End Set
        End Property


        Public Overridable Property DatExcepção() As System.Exception Implements IDATLET.DatExcepção
            Get
                SyncLock _locker
                    DatExcepção = l_excepção
                End SyncLock
            End Get
            Set(ByVal Value As System.Exception)
                SyncLock _locker
                    l_excepção = Value
                End SyncLock
            End Set
        End Property

        Public Overridable Sub DatReset() Implements IDATLET.DatReset
            SyncLock _locker
                'não afecta "l_info"
                'não afecta "l_canal"
                l_excepção = Nothing
                l_parametros = Nothing
                l_acção = Nothing
                private_resetpreparados()
                l_filtro_geral = Nothing
            End SyncLock
        End Sub

        Public Overridable Sub DatDispose() Implements IDATLET.DatDispose
            SyncLock _locker
                l_info = Nothing
                l_canal = Nothing
                l_excepção = Nothing
                l_parametros = Nothing
                l_acção = Nothing
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

                ' o CTYPE tem dificuldade (não consegue!) em converter arrays de datlet para o tipo especifico
                ' embora a mesma conversão mas apenas de uma dessas posições de cada vez resulte com sucesso
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
