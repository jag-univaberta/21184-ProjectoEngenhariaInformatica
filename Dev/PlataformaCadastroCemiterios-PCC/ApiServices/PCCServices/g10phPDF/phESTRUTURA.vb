Namespace Dataframe.Estrutura

    Public Enum dicContextos

        ServNãoIni = 1
        ServNãoObtCon = 2
        AppNãoIni = 3
        ProcLoginAbort = 4
        ProcLoginRecus = 5
        ProcLoginAuto = 6
        ServNãoConect = 7

    End Enum

    Public Class appDicionario
        Inherits DatletBase

        Public ID As Integer
        Public Titulo As String

        Private Shared ReadOnly _locker As New Object()

        Public Property Entradas() As appDiccionarioEntrada()
            Get
                SyncLock _locker
                    If l_entradas Is Nothing Then l_entradas = (New appDiccionarioEntrada(Me.l_canal)).DatSeleccionaFichasPorDic(Me.ID)
                    Return l_entradas
                End SyncLock
            End Get
            Set(ByVal Value As appDiccionarioEntrada())
                SyncLock _locker
                    l_entradas = Value
                End SyncLock
            End Set
        End Property
        Public Property Textos() As String()
            Get
                SyncLock _locker
                    If l_textos Is Nothing Then
                        If l_entradas Is Nothing Then
                            l_textos = (New appDiccionarioEntrada(Me.l_canal)).DatSeleccionaTextosPorDic(Me.ID)
                        Else
                            l_textos = ArranjaEntradasEmTextos(l_entradas)
                        End If
                    End If
                    Return (l_textos)
                End SyncLock
            End Get
            Set(ByVal Value As String())
                SyncLock _locker
                    l_textos = Value
                End SyncLock
            End Set
        End Property

        Private l_entradas() As appDiccionarioEntrada
        Private l_textos() As String

        Public Sub New()
            SyncLock _locker
                Me.l_info = New datInfo
                Me.l_info.Objecto = New datObjecto("appDIC", 126)

                Me.l_info.Objecto.MembroDef("iDICdic", DbType.Int32) : Me.l_info.Objecto.MembroSetPonteiro()
                Me.l_info.Objecto.MembroDef("sDICtit", DbType.String) : Me.l_info.Objecto.MembroSetChave()

                Me.l_info.AtributoDef(Me.l_info.Objecto)
                Me.l_info.AtributoDef("#entradas", DbType.Object, True, False, True, True)
                Me.l_info.AtributoDef("#textos", DbType.String, True, False, False, True, 2)
                Me.l_info.LigaçãoDef(2, "phFRAME.DataFrame", "appDicionarioEntrada", 1, datLigações.LeftJoin)

                Me.private_resetpreparados()
            End SyncLock
        End Sub

        Public Sub New(ByVal ConstrutorNulo As Boolean)

        End Sub

        Public Sub New(ByVal Canal As DatCanal)

            Me.New()
            SyncLock _locker
                Me.DatCanal = Canal
            End SyncLock
        End Sub

        Public Sub New(ByVal Info As DatInfo)
            SyncLock _locker
                Me.DatInfo = Info
            End SyncLock
        End Sub

        Public Sub New(ByVal Canal As DatCanal, ByVal Info As DatInfo)
            SyncLock _locker
                Me.DatCanal = Canal
                Me.DatInfo = Info
            End SyncLock
        End Sub

        Public Overrides Property DatAtributo(ByVal Index As Integer) As Object
            Get
                SyncLock _locker
                    Select Case Index
                        Case 0 : Return Me.ID
                        Case 1 : Return Me.Titulo
                        Case 2 : Return Me.Entradas
                        Case 3 : Return Me.Textos
                    End Select
                End SyncLock
            End Get
            Set(ByVal Value As Object)
                SyncLock _locker
                    Select Case Index
                        Case 0 : Me.ID = CInt(Value)
                        Case 1 : Me.Titulo = CType(Value, String)
                        Case 2 : Me.l_entradas = CType(Value, appDiccionarioEntrada())
                        Case 3 : Me.l_textos = CType(Value, String())
                    End Select
                End SyncLock
            End Set
        End Property

        Public Overrides Function DatClone() As ILET
            SyncLock _locker
                Dim the_clone As New appDicionario(False)
                private_clone(the_clone)
                Return the_clone
            End SyncLock
        End Function

        Public Overrides Function DatNew() As ILET
            SyncLock _locker
                Return New appDicionario(False)
            End SyncLock
        End Function

        Public Overrides Function DatCast(ByVal Lista() As ILET) As ILET()
            SyncLock _locker
                Dim r() As appDicionario
                Dim a As Integer

                If Lista Is Nothing Then
                    Return Nothing
                Else
                    ReDim r(Lista.GetUpperBound(0))

                    For a = 0 To Lista.GetUpperBound(0)

                        r(a) = DirectCast(Lista(a), appDicionario)

                    Next

                    Return r
                End If
            End SyncLock
        End Function

        Public Function DatDirectCast(ByVal Lista() As ILET) As appDicionario()
            SyncLock _locker
                Dim r() As appDicionario
                Dim a As Integer

                If Lista Is Nothing Then
                    Return Nothing
                Else
                    ReDim r(Lista.GetUpperBound(0))

                    For a = 0 To Lista.GetUpperBound(0)

                        r(a) = DirectCast(Lista(a), appDicionario)

                    Next

                    Return r
                End If
            End SyncLock
        End Function

        Public Overrides Sub DatPrepara(ByVal Alcance As Integer)
            SyncLock _locker
                l_entradas = Nothing
                l_textos = Nothing

                MyBase.DatPrepara(Alcance)
            End SyncLock
        End Sub

        Public Function ArranjaEntradasEmTextos(ByVal Array_Entradas() As appDiccionarioEntrada) As String()
            SyncLock _locker
                Dim e As Integer
                Dim i As Integer
                Dim lista() As String

                For e = 0 To Array_Entradas.GetUpperBound(0)

                    i = Array_Entradas(e).Contexto
                    ReDim Preserve lista(i)
                    lista(i) = Array_Entradas(e).Texto

                Next

                Return lista
            End SyncLock
        End Function

        Public Sub DatPopulaEntradas()
            SyncLock _locker
                DatPopulaEntradas(Me.l_textos)
            End SyncLock
        End Sub

        Public Sub DatPopulaEntradas(ByVal Textos() As String)
            SyncLock _locker
                Dim t As Integer
                Dim u As Integer

                u = -1

                For t = 0 To Textos.GetUpperBound(0)

                    If Not Textos(t) Is Nothing AndAlso Textos(t).Length > 0 Then
                        u = u + 1
                        ReDim Preserve l_entradas(u)
                        l_entradas(u) = New appDiccionarioEntrada(Me.DatCanal)
                        l_entradas(u).Diccionario = Me.ID
                        l_entradas(u).Contexto = t
                        l_entradas(u).Texto = Textos(t)
                    End If

                Next
            End SyncLock
        End Sub

    End Class

    Public Class appDiccionarioEntrada
        Inherits DatletBase

        Public ID As Integer
        Public Diccionario As Integer
        Public Contexto As Integer
        Public Texto As String

        Private acção_fichaspordic As New datAcção

        Private Shared ReadOnly _locker As New Object()

        Public Sub New()
            SyncLock _locker
                Dim objecto As New datObjecto("appDICPAL", 106)

                objecto.MembroDef("iDICPentrada", DbType.Int32) : objecto.MembroSetPonteiro()
                objecto.MembroDef("iDICPdic", DbType.Int32) : objecto.MembroSetChave()
                objecto.MembroDef("iDICPctx", DbType.Int32) : objecto.MembroSetChave()
                objecto.MembroDef("sDICPtexto", DbType.String) : objecto.MembroSetDescrição()

                Dim info As New datInfo
                info.Objecto = objecto
                info.AtributoDef(info.Objecto)

                Me.DatInfo = info
            End SyncLock
        End Sub

        Public Sub New(ByVal ConstrutorNulo As Boolean)

        End Sub

        Public Sub New(ByVal Canal As DatCanal)

            Me.New()
            SyncLock _locker
                Me.DatCanal = Canal
            End SyncLock
        End Sub

        Public Sub New(ByVal Info As DatInfo)
            SyncLock _locker
                Me.DatInfo = Info
            End SyncLock
        End Sub

        Public Sub New(ByVal Canal As DatCanal, ByVal Info As DatInfo)
            SyncLock _locker
                Me.DatCanal = Canal
                Me.DatInfo = Info
            End SyncLock
        End Sub

        Public Sub New(ByVal EntradaTemplate As appDiccionarioEntrada)
            SyncLock _locker
                Me.DatCanal = EntradaTemplate.DatCanal
                Me.DatInfo = EntradaTemplate.DatInfo
            End SyncLock
        End Sub

        Public Overrides Property DatAtributo(ByVal Index As Integer) As Object
            Get
                SyncLock _locker
                    Select Case Index
                        Case 0 : Return ID
                        Case 1 : Return Diccionario
                        Case 2 : Return Contexto
                        Case 3 : Return Texto
                    End Select
                End SyncLock
            End Get
            Set(ByVal Value As Object)
                SyncLock _locker
                    Select Case Index
                        Case 0 : ID = CInt(Value)
                        Case 1 : Diccionario = CInt(Value)
                        Case 2 : Contexto = CInt(Value)
                        Case 3 : Texto = CType(Value, String)
                    End Select
                End SyncLock
            End Set
        End Property

        Public Overrides Function DatClone() As ILET
            SyncLock _locker
                Dim r As New appDiccionarioEntrada(False)
                private_clone(r)
                Return r
            End SyncLock
        End Function

        Public Overrides Function DatNew() As ILET
            SyncLock _locker
                Return New appDiccionarioEntrada(False)
            End SyncLock
        End Function

        Public Overrides Function DatCast(ByVal Lista() As ILET) As ILET()
            SyncLock _locker
                Dim r() As appDiccionarioEntrada
                Dim a As Integer

                If Lista Is Nothing Then
                    Return Nothing
                Else
                    ReDim r(Lista.GetUpperBound(0))

                    For a = 0 To Lista.GetUpperBound(0)

                        r(a) = DirectCast(Lista(a), appDiccionarioEntrada)

                    Next

                    Return r
                End If
            End SyncLock
        End Function

        Public Function DatDirectCast(ByVal Lista() As ILET) As appDiccionarioEntrada()
            SyncLock _locker
                Dim r() As appDiccionarioEntrada
                Dim a As Integer

                If Lista Is Nothing Then
                    Return Nothing
                Else
                    ReDim r(Lista.GetUpperBound(0))

                    For a = 0 To Lista.GetUpperBound(0)

                        r(a) = DirectCast(Lista(a), appDiccionarioEntrada)

                    Next

                    Return r
                End If
            End SyncLock
        End Function

        Public Function DatSeleccionaFichasPorDic() As appDiccionarioEntrada()
            SyncLock _locker
                Return DatSeleccionaFichasPorDic(Me.Diccionario)
            End SyncLock
        End Function

        Public Function DatSeleccionaFichasPorDic(ByVal DiccionarioID As Integer) As appDiccionarioEntrada()
            SyncLock _locker
                Dim lista() As appDiccionarioEntrada

                If Not acção_fichaspordic.Definida Then
                    acção_fichaspordic.Pedido = New datPedido(Me.l_info.Objecto, datOperações.Consulta)
                    acção_fichaspordic.Pedido.MaisInscritos(Me.l_info.Objecto.InscriçõesTodas)
                    acção_fichaspordic.Pedido.MaisFiltros(Me.l_info.Objecto.Inscrições(1))
                    acção_fichaspordic.Pedido.MaisOrdenações(Me.l_info.Objecto.Inscrições(2))
                End If

                If Not acção_fichaspordic.Preparada Then
                    acção_fichaspordic.PreparaAcção(Me.l_canal.Construtor)
                End If

                Me.Diccionario = DiccionarioID
                lista = Me.DatDirectCast(Me.l_canal.Adaptador.Selecciona(acção_fichaspordic, Me))

                Return lista
            End SyncLock
        End Function

        Public Function DatSeleccionaTextosPorDic() As String()
            SyncLock _locker
                Dim pedido As datPedido
                Dim acção As datAcção
                Dim lista() As appDiccionarioEntrada
                Dim l As Integer
                Dim textos() As String
                Dim t As Integer

                ' esta acção não é armazenada depois de definida e preparada só para mostrar como se faz

                ' definição:
                pedido = New datPedido(Me.l_info.Objecto, datOperações.Consulta)
                pedido.MaisInscritos(Me.l_info.Objecto.Inscrições(2, 3))
                pedido.MaisFiltros(Me.l_info.Objecto.Inscrições(1))
                pedido.MaisOrdenações(Me.l_info.Objecto.Inscrições(2))

                ' preparação:
                acção.ParametrosFiltro = g10phPDF4.Basframe.Array.Def(CType(Me.Diccionario, Object))
                acção.Sintaxe = Me.l_canal.Construtor.ConstroiSintaxe(pedido)
                acção.AssimilaPedido(pedido)

                ' execução:
                lista = Me.DatDirectCast(Me.l_canal.Adaptador.Selecciona(acção, Me))

                ' recolha.
                For l = 0 To lista.GetUpperBound(0)
                    t = lista(l).Contexto
                    ReDim Preserve textos(t)
                    textos(t) = lista(l).Texto
                Next

                Return textos
            End SyncLock
        End Function

        Public Function DatSeleccionaTextosPorDic(ByVal DiccionarioID As Integer) As String()
            SyncLock _locker
                Me.Diccionario = DiccionarioID
                Return DatSeleccionaTextosPorDic()
            End SyncLock
        End Function

    End Class

    ' todo: phFRAME: Dataframe: Estrutura
    ' (appDIC, appDICPAL, appFER, appPST, appPRN, appPRNIMG)

End Namespace
