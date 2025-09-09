Namespace Basframe

    Public Module Base

        Public Const NoConstruct As Boolean = True

        Public Const DivTick2Micro As Integer = 10

        Public Const DivTick2Mili As Integer = 10000

        Public Const DivTick2Seg As Integer = 10000000

        Public Const DivTick2Min As Integer = 600000000

        Private l_CD As String

        Public ErroLogger As IERROLOG

        Private ReadOnly _locker As New Object()

        Public Property CaracterDecimal() As String
            Get
                SyncLock _locker
                    If l_CD Is Nothing Then l_CD = CChar(Format(0, "#.0").Substring(0, 1))
                    Return l_CD
                End SyncLock
            End Get
            Set(ByVal Value As String)
                SyncLock _locker
                    l_CD = Value
                End SyncLock
            End Set
        End Property

        Public Sub CaracterDecimalReset()
            SyncLock _locker
                l_CD = Nothing
            End SyncLock
        End Sub

        Public Function NSF(ByVal Valor As String) As String
            SyncLock _locker
                Return Valor.Replace(CaracterDecimal, ".")
            End SyncLock
        End Function

        Public Function NEF(ByVal Valor As String) As String
            SyncLock _locker
                Return Valor.Replace(".", CaracterDecimal)
            End SyncLock
        End Function

        Public Function EstáContido(ByVal Contentor As Integer, ByVal Contido As Integer) As Boolean
            SyncLock _locker
                Return (Contentor And Contido) = Contido
            End SyncLock
        End Function

        Public Function ByteAdd(ByVal Base As Byte, ByVal Valor As Byte, ByVal ValorPositivo As Boolean) As Byte
            SyncLock _locker
                Dim Result As Integer

                If ValorPositivo Then
                    Result = CInt(Base) + CInt(Valor)
                Else
                    Result = CInt(Base) - CInt(Valor)
                End If

                If Result > 255 Then
                    Result = Result - 256
                ElseIf Result < 0 Then
                    Result = 256 + Result
                End If

                Return CByte(Result)
            End SyncLock
        End Function

        Public Function ByteAdd(ByVal Base As Byte, ByVal Valor As Byte) As Byte
            SyncLock _locker
                Return ByteAdd(Base, Valor, True)
            End SyncLock
        End Function

        Public Function ByteAleatorio() As Byte
            SyncLock _locker
                Return CByte(Rnd() * 255)
            End SyncLock
        End Function



        Public Function Cast2Boolean(ByVal Dado As Object) As Boolean
            SyncLock _locker
                If Dado Is Nothing Then Return False

                Select Case Dado.ToString.Trim.ToLower

                    Case "true", "verdade" : Return True
                    Case "yes", "sim" : Return True
                    Case "x", "1" : Return True

                    Case Else : Return False

                End Select
            End SyncLock
        End Function

        Public Function Date2ST(ByVal data As Date) As String
            SyncLock _locker
                Return (Format(data.Year, "0000") & Format(data.Month, "00") & Format(data.Day, "00"))
            End SyncLock
        End Function

        Public Function Time2ST(ByVal data As Date) As String
            SyncLock _locker
                Return (Format(data.Hour, "0000") & Format(data.Minute, "00") & Format(data.Second, "00"))
            End SyncLock
        End Function

    End Module

    Public Class basContexto

        Public ID As Integer
        Public Titulo As String
        Public Pai As basContexto
        Public Filho As basContexto
        Public Raiz As basContexto
        Public Topo As basContexto

        Private Shared ReadOnly _locker As New Object()

        Public Overrides Function ToString() As String
            SyncLock _locker
                ToString = ID & ": " & Titulo
            End SyncLock
        End Function

        Public Sub New(ByVal p_id As Integer, ByVal p_titulo As String)
            SyncLock _locker
                Me.ID = p_id
                Me.Titulo = p_titulo
                Me.Raiz = Me
                Me.Topo = Me
            End SyncLock
        End Sub

        Public Sub New(ByVal p_id As Integer, ByVal p_titulo As String, ByVal p_pai As basContexto)
            SyncLock _locker
                Me.ID = p_id
                Me.Titulo = p_titulo
                Me.Pai = p_pai
                Me.Raiz = p_pai.Raiz
                Me.Topo = Me
            End SyncLock
        End Sub

        Public Function Stack(ByVal p_id As Integer, ByVal p_titulo As String) As basContexto
            SyncLock _locker
                Me.Filho = New basContexto(p_id, p_titulo, Me)
                Me.Raiz.Topo = Me.Filho
                Return Me.Filho
            End SyncLock
        End Function

        Private Function FunçãoExemplo(ByVal ContextoPai As basContexto) As Boolean

            'Dim MyContexto As basContexto

            'MyContexto = ContextoPai.Stack(1200, "jkjkjkjkjkjkj")

            ' ou simplesmente:

            'ContextoPai.Stack(1200, "jkjkjkjkjkjkj")

            ' ou também:

            'ContextoPai.Stack(1200, DicionarioBase.Entradas(1239))

        End Function

    End Class

    Public Interface IERROLOG

        Property UltimaExcepção() As System.Exception
        Property Registando() As Boolean

        Function DeuExcepção(ByVal Excepção As System.Exception, ByVal Parametros As String, ByVal ComAviso As Boolean, ByVal AbortRetryIgnore As Boolean, ByVal Critico As Boolean) As MsgBoxResult

        Function RegistaExcepção(ByVal Excepção As System.Exception, ByVal Parametros As String, ByVal Critico As Boolean) As Boolean

        Event PréExcepção(ByVal Excepção As System.Exception, ByVal Parametros As String, ByVal ComAviso As Boolean, ByVal AbortRetryIgnore As Boolean, ByVal Critico As Boolean, ByRef Cancela As Boolean)
        Event PósExcepção(ByVal Excepção As System.Exception, ByVal Parametros As String, ByVal ComAviso As Boolean, ByVal AbortRetryIgnore As Boolean, ByVal Critico As Boolean, ByVal Opção As MsgBoxResult)

    End Interface

End Namespace
