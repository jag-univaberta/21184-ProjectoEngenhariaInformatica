

Module SuporteAplicacional
    Private ReadOnly _locker As New Object()

    Public Function DialogoCria(ByVal FicheiroNome As String, ByVal Confirma As Boolean, ByRef getResposta As MsgBoxResult, ByRef getExcep��o As Exception) As Boolean
        SyncLock _locker
            Dim liga��o As System.IO.FileStream
            Dim Caminho As String

            Caminho = System.IO.Path.GetDirectoryName(FicheiroNome)

            If Confirma Then
                getResposta = g10phPDF4.Acompanhamento.Mordomo.Mensageiro.AskCriarFicheiro(FicheiroNome)
            Else
                getResposta = MsgBoxResult.Yes
            End If

            If getResposta = MsgBoxResult.Yes Then

                Do

                    getExcep��o = Nothing
                    getResposta = MsgBoxResult.Ok

                    Try

                        liga��o = System.IO.File.Create(FicheiroNome)
                        liga��o.Close()
                        Return True

                    Catch Exp As System.IO.DirectoryNotFoundException

                        getExcep��o = Exp
                        getResposta = g10phPDF4.Acompanhamento.Mordomo.Mensageiro.InfoPastaN�oExiste(Caminho, True) ' pergunta se cria

                        If getResposta = MsgBoxResult.Yes Then
                            If DialogoCriaPasta(Caminho, False, getResposta, getExcep��o) Then getResposta = MsgBoxResult.Retry
                        End If

                        If getResposta = MsgBoxResult.No Then getResposta = MsgBoxResult.Retry

                    Catch Exp As System.Exception

                        getExcep��o = Exp
                        getResposta = g10phPDF4.Acompanhamento.Mordomo.ErroLogger.DeuExcep��o(Exp, FicheiroNome, True, True, False)

                    End Try

                Loop Until getResposta <> MsgBoxResult.Retry

            End If

            Return False
        End SyncLock
    End Function

    Public Function DialogoCriaPasta(ByVal Caminho As String, ByVal Confirma As Boolean, ByRef getResposta As MsgBoxResult, ByRef getExcep��o As Exception) As Boolean
        SyncLock _locker
            If Confirma Then
                getResposta = g10phPDF4.Acompanhamento.Mordomo.Mensageiro.AskCriarPasta(Caminho)
            Else
                getResposta = MsgBoxResult.Yes
            End If

            If getResposta = MsgBoxResult.Yes Then

                Do

                    getExcep��o = Nothing
                    getResposta = MsgBoxResult.Ok

                    Try

                        System.IO.Directory.CreateDirectory(Caminho)
                        Return True

                    Catch Exp As System.Exception

                        getExcep��o = Exp
                        getResposta = g10phPDF4.Acompanhamento.Mordomo.ErroLogger.DeuExcep��o(Exp, Caminho, True, True, False)

                    End Try

                Loop Until getResposta <> MsgBoxResult.Retry

            End If

            Return False
        End SyncLock
    End Function

    Public Function DialogoAbre(ByVal FicheiroNome As String, ByVal Modo As System.IO.FileMode, ByRef getPonteiro As System.IO.FileStream, ByRef getResposta As MsgBoxResult, ByRef getExcep��o As Exception) As Boolean
        SyncLock _locker
            Dim Caminho As String

            Caminho = System.IO.Path.GetDirectoryName(FicheiroNome)

            Do

                getExcep��o = Nothing
                getResposta = MsgBoxResult.Ok

                Try

                    getPonteiro = System.IO.File.Open(FicheiroNome, Modo)
                    Return True

                Catch Exp As System.IO.FileNotFoundException

                    getExcep��o = Exp
                    getResposta = g10phPDF4.Acompanhamento.Mordomo.Mensageiro.InfoFicheiroN�oEncontrado(FicheiroNome, True)

                    If getResposta = MsgBoxResult.Yes Then

                        If DialogoCria(FicheiroNome, False, getResposta, getExcep��o) Then getResposta = MsgBoxResult.Retry

                    End If

                    If getResposta = MsgBoxResult.No Then getResposta = MsgBoxResult.Retry

                Catch Exp As System.IO.DirectoryNotFoundException

                    getExcep��o = Exp
                    getResposta = g10phPDF4.Acompanhamento.Mordomo.Mensageiro.InfoPastaN�oExiste(Caminho, True) ' pergunta_se_cria

                    If getResposta = MsgBoxResult.Yes Then

                        If DialogoCriaPasta(Caminho, False, getResposta, getExcep��o) Then getResposta = MsgBoxResult.Retry

                    End If

                    If getResposta = MsgBoxResult.No Then getResposta = MsgBoxResult.Retry

                Catch Exp As System.Exception

                    getExcep��o = Exp
                    getResposta = g10phPDF4.Acompanhamento.Mordomo.ErroLogger.DeuExcep��o(Exp, FicheiroNome, True, True, False)

                End Try

            Loop Until getResposta <> MsgBoxResult.Retry

            Return False
        End SyncLock
    End Function

    Public Function DialogoFecha(ByVal Liga��o As System.IO.FileStream, ByRef getResposta As MsgBoxResult, ByRef getExcep��o As Exception) As Boolean
        SyncLock _locker
            Do

                getExcep��o = Nothing
                getResposta = MsgBoxResult.Ok

                Try

                    Liga��o.Close()
                    Return True

                Catch Exp As System.Exception

                    getExcep��o = Exp
                    getResposta = g10phPDF4.Acompanhamento.Mordomo.ErroLogger.DeuExcep��o(Exp, "", True, True, False)

                End Try

            Loop Until getResposta <> MsgBoxResult.Retry

            Return False
        End SyncLock
    End Function


    Public Function DialogoEscreve(ByVal Liga��o As System.IO.FileStream, ByVal Linha As String, ByRef getResposta As MsgBoxResult, ByRef getExcep��o As Exception) As Boolean
        SyncLock _locker
            Do

                getExcep��o = Nothing
                getResposta = MsgBoxResult.Ok

                Try

                    Liga��o.Write(g10phPDF4.Basframe.Strings.TransStringEmBytes(Linha), 0, Linha.Length)
                    Return True

                Catch Exp As System.Exception

                    getExcep��o = Exp
                    getResposta = g10phPDF4.Acompanhamento.Mordomo.ErroLogger.DeuExcep��o(Exp, Linha, True, True, False)

                End Try

            Loop Until getResposta <> MsgBoxResult.Retry

            Return False
        End SyncLock
    End Function

    Public Function DialogoEscreve(ByVal Liga��o As System.IO.FileStream, ByVal Dados() As Byte, ByRef getResposta As MsgBoxResult, ByRef getExcep��o As Exception) As Boolean
        SyncLock _locker
            Do

                getExcep��o = Nothing
                getResposta = MsgBoxResult.Ok

                Try

                    Liga��o.Write(Dados, 0, Dados.GetUpperBound(0) + 1)
                    Return True

                Catch Exp As System.Exception

                    getExcep��o = Exp
                    getResposta = g10phPDF4.Acompanhamento.Mordomo.ErroLogger.DeuExcep��o(Exp, g10phPDF4.Basframe.Strings.TransBytesEmString(Dados), True, True, False)

                End Try

            Loop Until getResposta <> MsgBoxResult.Retry

            Return False
        End SyncLock
    End Function

    Public Function DialogoEscreve(ByVal FicheiroNome As String, ByVal Linha As String, ByVal Modo As System.IO.FileMode, ByVal CriaFicheiro As Boolean, ByRef getResposta As MsgBoxResult, ByRef getExcep��o As Exception) As Boolean
        SyncLock _locker
            Dim liga��o As System.IO.FileStream

            getExcep��o = Nothing

            If DialogoAbre(FicheiroNome, Modo, liga��o, getResposta, getExcep��o) Then

                If DialogoEscreve(liga��o, Linha, getResposta, getExcep��o) Then
                    Return (g10phPDF4.Basframe.File.Fecha(liga��o, getExcep��o))
                Else
                    DialogoFecha(liga��o, getResposta, getExcep��o)
                    Return False
                End If

            End If
        End SyncLock
    End Function

    Public Function DialogoEscreve(ByVal FicheiroNome As String, ByVal Dados() As Byte, ByVal Modo As System.IO.FileMode, ByVal CriaFicheiro As Boolean, ByRef getResposta As MsgBoxResult, ByRef getExcep��o As Exception) As Boolean
        SyncLock _locker
            Dim liga��o As System.IO.FileStream

            getExcep��o = Nothing

            If DialogoAbre(FicheiroNome, Modo, liga��o, getResposta, getExcep��o) Then

                If DialogoEscreve(liga��o, Dados, getResposta, getExcep��o) Then
                    Return (DialogoFecha(liga��o, getResposta, getExcep��o))
                Else
                    DialogoFecha(liga��o, getResposta, getExcep��o)
                    Return False
                End If

            End If
        End SyncLock
    End Function

    Public Function DialogoLe(ByVal Liga��o As System.IO.FileStream, ByRef getResposta As MsgBoxResult, ByRef getExcep��o As Exception) As String
        SyncLock _locker
            Dim Conteudo() As Byte

            ReDim Conteudo(CInt(Liga��o.Length))

            getExcep��o = Nothing

            Try

                Liga��o.Read(Conteudo, 0, CInt(Liga��o.Length))
                ReDim Preserve Conteudo(CInt(Liga��o.Length - 1)) ' � necess�rio retirar o ultimo caracter NULO
                Return g10phPDF4.Basframe.Strings.TransBytesEmString(Conteudo)

            Catch Exp As System.Exception

                getExcep��o = Exp
                getResposta = g10phPDF4.Acompanhamento.Mordomo.ErroLogger.DeuExcep��o(Exp, "", True, True, False)

            End Try

            Return Nothing
        End SyncLock
    End Function

    Public Function DialogoLe(ByVal FicheiroNome As String, ByRef getResposta As MsgBoxResult, ByRef getExcep��o As Exception) As String
        SyncLock _locker
            Dim liga��o As System.IO.FileStream
            Dim st As String

            getExcep��o = Nothing

            If DialogoAbre(FicheiroNome, IO.FileMode.Open, liga��o, getResposta, getExcep��o) Then

                st = DialogoLe(liga��o, getResposta, getExcep��o)
                If g10phPDF4.Basframe.File.Fecha(liga��o, getExcep��o) Then
                    Return st
                Else
                    Return Nothing
                End If

            Else

                Return Nothing

            End If
        End SyncLock
    End Function

End Module