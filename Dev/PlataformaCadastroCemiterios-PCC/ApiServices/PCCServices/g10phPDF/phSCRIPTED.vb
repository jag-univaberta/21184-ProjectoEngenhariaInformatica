Namespace Report.Scripted

    ' todo: phFRAME: Setframe: Scripted
    ' (areas, motores, modelos e directores)

    Public Class modelos
        Inherits g10phPDF4.Dataframe.DatletBase

        Public Id As Integer
        Public Tit As String
        Public Desc As String
        Public Cod As String
        Public Script As String
        Public Tipo As ScriptTipo

        Private Shared ReadOnly _locker As New Object()

        Public Sub New()
            SyncLock _locker
                Dim obj As New g10phPDF4.Dataframe.datObjecto("appMOD", 101)

                obj.MembroDef("iMODmod", DbType.Int32) : obj.MembroSetPonteiro()
                obj.MembroDef("sMODtit", DbType.String) : obj.MembroSetDescrição()
                obj.MembroDef("sMODdesc", DbType.String)
                obj.MembroDef("sMODcod", DbType.String) : obj.MembroSetChave()
                obj.MembroDef("mMODscript", DbType.String)
                obj.MembroDef("tMODtipo", DbType.Int32)

                Me.DatInfo = New g10phPDF4.Dataframe.datInfo
                Me.DatInfo.Objecto = obj
                Me.DatInfo.AtributoDef(obj)

                Me.private_resetpreparados()
            End SyncLock
        End Sub

        Public Sub New(ByVal Canal As g10phPDF4.Dataframe.datCanal)
            Me.New()
            SyncLock _locker
                Me.DatCanal = Canal

            End SyncLock
        End Sub

        Public Sub New(ByVal ContrutorFalso As Boolean)

        End Sub

        Public Overrides Property DatAtributo(ByVal Index As Integer) As Object
            Get
                SyncLock _locker
                    Select Case Index
                        Case 0 : Return Id
                        Case 1 : Return Tit
                        Case 2 : Return Desc
                        Case 3 : Return Cod
                        Case 4 : Return Script
                        Case 5 : Return Tipo
                    End Select
                End SyncLock
            End Get
            Set(ByVal Value As Object)
                SyncLock _locker
                    Select Case Index
                        Case 0 : Me.Id = CInt(Value)
                        Case 1 : Me.Tit = CType(Value, String)
                        Case 2 : Me.Desc = CType(Value, String)
                        Case 3 : Me.Cod = CType(Value, String)
                        Case 4 : Me.Script = CType(Value, String)
                        Case 5 : Me.Tipo = CType(Value, ScriptTipo)
                    End Select
                End SyncLock
            End Set
        End Property

        Public Overrides Function DatCast(ByVal Lista() As g10phPDF4.Dataframe.ILET) As g10phPDF4.Dataframe.ILET()
            SyncLock _locker
                Dim r() As modelos
                Dim a As Integer

                If Lista Is Nothing Then
                    Return Nothing
                Else
                    ReDim r(Lista.GetUpperBound(0))

                    For a = 0 To Lista.GetUpperBound(0)
                        r(a) = DirectCast(Lista(a), modelos)
                    Next

                    Return r
                End If
            End SyncLock
        End Function

        Public Overrides Function DatClone() As g10phPDF4.Dataframe.ILET
            SyncLock _locker
                Dim the_clone As New modelos(False)
                private_clone(the_clone)
                Return the_clone
            End SyncLock
        End Function

        Public Overrides Function DatNew() As g10phPDF4.Dataframe.ILET
            SyncLock _locker
                Return New modelos(False)
            End SyncLock
        End Function


    End Class

End Namespace