Namespace Dataframe.Shell

    ' todo: phFRAME: Dataframe: Shell
    ' (appDB, appALIAS, Catalogo da estrutura)

    Public Enum catObjectosTipo

        Tabela = 1
        Vista = 2
        SP = 3

    End Enum

    Public Enum catMembrosTipo

        Campo = 1
        Variavel = 2

    End Enum

    Public Enum catLigaçõesTipo

        Opcional = 1
        Obrigatorio = 2

    End Enum

    '                                                                                               

    Public Class appBaseDeDados
        Inherits DatletBase

        Public ID As Integer
        Public Nome As String
        Public Titulo As String
        Public Obs As String
        Public PicPath As String
        Public PicIndex As Integer

        Private Shared ReadOnly _locker As New Object()

        Public Sub New()
            SyncLock _locker
                Me.l_info = New datInfo
                Me.l_info.Objecto = New datObjecto("appDB", 101)

                Me.l_info.Objecto.MembroDef("iDBdb", DbType.Int32) : Me.l_info.Objecto.MembroSetPonteiro()
                Me.l_info.Objecto.MembroDef("sDBnome", DbType.String) : Me.l_info.Objecto.MembroSetChave()
                Me.l_info.Objecto.MembroDef("sDBtit", DbType.String) : Me.l_info.Objecto.MembroSetDescrição()
                Me.l_info.Objecto.MembroDef("mDBobs", DbType.String)
                Me.l_info.Objecto.MembroDef("sDBpicpath", DbType.String)
                Me.l_info.Objecto.MembroDef("iDBpicindex", DbType.Int32)
                Me.l_info.AtributoDef(Me.l_info.Objecto)
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
                        Case 1 : Return Me.Nome
                        Case 2 : Return Me.Titulo
                        Case 3 : Return Me.Obs
                        Case 4 : Return Me.PicPath
                        Case 5 : Return Me.PicIndex

                    End Select
                End SyncLock
            End Get
            Set(ByVal Value As Object)
                SyncLock _locker
                    Select Case Index

                        Case 0 : Me.ID = CInt(Value)
                        Case 1 : Me.Nome = CType(Value, String)
                        Case 2 : Me.Titulo = CType(Value, String)
                        Case 3 : Me.Obs = CType(Value, String)
                        Case 4 : Me.PicPath = CType(Value, String)
                        Case 5 : Me.PicIndex = CInt(Value)

                    End Select
                End SyncLock
            End Set
        End Property

        Public Overrides Function DatClone() As ILET
            SyncLock _locker
                Dim r As New appBaseDeDados(False)
                private_clone(r)
                Return r
            End SyncLock
        End Function

        Public Overrides Function DatNew() As ILET
            SyncLock _locker
                Return New appBaseDeDados(False)
            End SyncLock
        End Function

        Public Overrides Function DatCast(ByVal Lista() As ILET) As ILET()
            SyncLock _locker
                Dim r() As appBaseDeDados
                Dim a As Integer

                If Lista Is Nothing Then
                    Return Nothing
                Else
                    ReDim r(Lista.GetUpperBound(0))

                    For a = 0 To Lista.GetUpperBound(0)

                        r(a) = DirectCast(Lista(a), appBaseDeDados)

                    Next

                    Return r
                End If
            End SyncLock
        End Function


    End Class

    Public Class appAlias
        Inherits DatletBase

        Public ID As Integer
        Public Objecto As New catObjecto
        Public BaseDeDados As New appBaseDeDados

        Private Shared ReadOnly _locker As New Object()

        Public Sub New()
            SyncLock _locker
                Me.l_info = New datInfo
                Me.l_info.Objecto = New datObjecto("appALIAS", 102)

                Me.l_info.Objecto.MembroDef("iALIalias", DbType.Int32) : Me.l_info.Objecto.MembroSetPonteiro()
                Me.l_info.Objecto.MembroDef("pALIobj", DbType.Int32, Objecto, datLigações.LeftJoin) : Me.l_info.Objecto.MembroSetChave()
                Me.l_info.Objecto.MembroDef("pALIdb", DbType.Int32, BaseDeDados, datLigações.LeftJoin) : Me.l_info.Objecto.MembroSetDescrição()
                Me.l_info.AtributoDef(Me.l_info.Objecto)
                Me.private_resetpreparados()
            End SyncLock
        End Sub

        Public Sub New(ByVal ConstrutorNulo As Boolean)

        End Sub

        Public Sub New(ByVal Canal As DatCanal)
            Me.New()
            SyncLock _locker

                Me.l_canal = Canal
            End SyncLock
        End Sub

        Public Overrides Property DatAtributo(ByVal Index As Integer) As Object
            Get
                SyncLock _locker
                    Select Case Index
                        Case 0 : Return Me.ID
                        Case 1 : Return Me.Objecto.ID
                        Case 2 : Return Me.BaseDeDados.ID
                    End Select
                End SyncLock
            End Get
            Set(ByVal Value As Object)
                SyncLock _locker
                    Select Case Index
                        Case 0 : Me.ID = CInt(Value)
                        Case 1 : Me.Objecto.ID = CInt(Value)
                        Case 2 : Me.BaseDeDados.ID = CInt(Value)
                    End Select
                End SyncLock
            End Set
        End Property

        Public Overrides Function DatClone() As ILET
            SyncLock _locker
                Dim r As New appAlias(False)
                private_clone(r)
                Return r
            End SyncLock
        End Function

        Public Overrides Function DatCast(ByVal Lista() As ILET) As ILET()
            SyncLock _locker
                Dim r() As appAlias
                Dim a As Integer

                If Lista Is Nothing Then
                    Return Nothing
                Else
                    ReDim r(Lista.GetUpperBound(0))

                    For a = 0 To Lista.GetUpperBound(0)

                        r(a) = DirectCast(Lista(a), appAlias)

                    Next

                    Return r
                End If
            End SyncLock
        End Function

        Public Overrides Function DatNew() As ILET
            SyncLock _locker
                Return New appAlias(False)
            End SyncLock
        End Function

    End Class

    Public Class catObjecto
        Inherits DatletBase

        Public ID As Integer


        Public Overrides Property DatAtributo(ByVal Index As Integer) As Object
            Get

            End Get
            Set(ByVal Value As Object)

            End Set
        End Property

        Public Overrides Function DatCast(ByVal Lista() As ILET) As ILET()

        End Function

        Public Overrides Function DatClone() As ILET

        End Function

        Public Overrides Function DatNew() As ILET

        End Function
    End Class

    '                                                                                               

End Namespace