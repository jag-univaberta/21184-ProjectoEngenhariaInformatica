

Module SuporteAplicacional
    Private ReadOnly _locker As New Object()

    Public Function DialogoCria(ByVal FicheiroNome As String, ByVal Confirma As Boolean, ByRef getResposta As MsgBoxResult, ByRef getExcepção As Exception) As Boolean
        SyncLock _locker
            Dim ligação As System.IO.FileStream
            Dim Caminho As String

            Caminho = System.IO.Path.GetDirectoryName(FicheiroNome)

            If Confirma Then
                getResposta = g10phPDF4.Acompanhamento.Mordomo.Mensageiro.AskCriarFicheiro(FicheiroNome)
            Else
                getResposta = MsgBoxResult.Yes
            End If

            If getResposta = MsgBoxResult.Yes Then

                Do

                    getExcepção = Nothing
                    getResposta = MsgBoxResult.Ok

                    Try

                        ligação = System.IO.File.Create(FicheiroNome)
                        ligação.Close()
                        Return True

                    Catch Exp As System.IO.DirectoryNotFoundException

                        getExcepção = Exp
                        getResposta = g10phPDF4.Acompanhamento.Mordomo.Mensageiro.InfoPastaNãoExiste(Caminho, True) ' pergunta se cria

                        If getResposta = MsgBoxResult.Yes Then
                            If DialogoCriaPasta(Caminho, False, getResposta, getExcepção) Then getResposta = MsgBoxResult.Retry
                        End If

                        If getResposta = MsgBoxResult.No Then getResposta = MsgBoxResult.Retry

                    Catch Exp As System.Exception

                        getExcepção = Exp
                        getResposta = g10phPDF4.Acompanhamento.Mordomo.ErroLogger.DeuExcepção(Exp, FicheiroNome, True, True, False)

                    End Try

                Loop Until getResposta <> MsgBoxResult.Retry

            End If

            Return False
        End SyncLock
    End Function

    Public Function DialogoCriaPasta(ByVal Caminho As String, ByVal Confirma As Boolean, ByRef getResposta As MsgBoxResult, ByRef getExcepção As Exception) As Boolean
        SyncLock _locker
            If Confirma Then
                getResposta = g10phPDF4.Acompanhamento.Mordomo.Mensageiro.AskCriarPasta(Caminho)
            Else
                getResposta = MsgBoxResult.Yes
            End If

            If getResposta = MsgBoxResult.Yes Then

                Do

                    getExcepção = Nothing
                    getResposta = MsgBoxResult.Ok

                    Try

                        System.IO.Directory.CreateDirectory(Caminho)
                        Return True

                    Catch Exp As System.Exception

                        getExcepção = Exp
                        getResposta = g10phPDF4.Acompanhamento.Mordomo.ErroLogger.DeuExcepção(Exp, Caminho, True, True, False)

                    End Try

                Loop Until getResposta <> MsgBoxResult.Retry

            End If

            Return False
        End SyncLock
    End Function

    Public Function DialogoAbre(ByVal FicheiroNome As String, ByVal Modo As System.IO.FileMode, ByRef getPonteiro As System.IO.FileStream, ByRef getResposta As MsgBoxResult, ByRef getExcepção As Exception) As Boolean
        SyncLock _locker
            Dim Caminho As String

            Caminho = System.IO.Path.GetDirectoryName(FicheiroNome)

            Do

                getExcepção = Nothing
                getResposta = MsgBoxResult.Ok

                Try

                    getPonteiro = System.IO.File.Open(FicheiroNome, Modo)
                    Return True

                Catch Exp As System.IO.FileNotFoundException

                    getExcepção = Exp
                    getResposta = g10phPDF4.Acompanhamento.Mordomo.Mensageiro.InfoFicheiroNãoEncontrado(FicheiroNome, True)

                    If getResposta = MsgBoxResult.Yes Then

                        If DialogoCria(FicheiroNome, False, getResposta, getExcepção) Then getResposta = MsgBoxResult.Retry

                    End If

                    If getResposta = MsgBoxResult.No Then getResposta = MsgBoxResult.Retry

                Catch Exp As System.IO.DirectoryNotFoundException

                    getExcepção = Exp
                    getResposta = g10phPDF4.Acompanhamento.Mordomo.Mensageiro.InfoPastaNãoExiste(Caminho, True) ' pergunta_se_cria

                    If getResposta = MsgBoxResult.Yes Then

                        If DialogoCriaPasta(Caminho, False, getResposta, getExcepção) Then getResposta = MsgBoxResult.Retry

                    End If

                    If getResposta = MsgBoxResult.No Then getResposta = MsgBoxResult.Retry

                Catch Exp As System.Exception

                    getExcepção = Exp
                    getResposta = g10phPDF4.Acompanhamento.Mordomo.ErroLogger.DeuExcepção(Exp, FicheiroNome, True, True, False)

                End Try

            Loop Until getResposta <> MsgBoxResult.Retry

            Return False
        End SyncLock
    End Function

    Public Function DialogoFecha(ByVal Ligação As System.IO.FileStream, ByRef getResposta As MsgBoxResult, ByRef getExcepção As Exception) As Boolean
        SyncLock _locker
            Do

                getExcepção = Nothing
                getResposta = MsgBoxResult.Ok

                Try

                    Ligação.Close()
                    Return True

                Catch Exp As System.Exception

                    getExcepção = Exp
                    getResposta = g10phPDF4.Acompanhamento.Mordomo.ErroLogger.DeuExcepção(Exp, "", True, True, False)

                End Try

            Loop Until getResposta <> MsgBoxResult.Retry

            Return False
        End SyncLock
    End Function


    Public Function DialogoEscreve(ByVal Ligação As System.IO.FileStream, ByVal Linha As String, ByRef getResposta As MsgBoxResult, ByRef getExcepção As Exception) As Boolean
        SyncLock _locker
            Do

                getExcepção = Nothing
                getResposta = MsgBoxResult.Ok

                Try

                    Ligação.Write(g10phPDF4.Basframe.Strings.TransStringEmBytes(Linha), 0, Linha.Length)
                    Return True

                Catch Exp As System.Exception

                    getExcepção = Exp
                    getResposta = g10phPDF4.Acompanhamento.Mordomo.ErroLogger.DeuExcepção(Exp, Linha, True, True, False)

                End Try

            Loop Until getResposta <> MsgBoxResult.Retry

            Return False
        End SyncLock
    End Function

    Public Function DialogoEscreve(ByVal Ligação As System.IO.FileStream, ByVal Dados() As Byte, ByRef getResposta As MsgBoxResult, ByRef getExcepção As Exception) As Boolean
        SyncLock _locker
            Do

                getExcepção = Nothing
                getResposta = MsgBoxResult.Ok

                Try

                    Ligação.Write(Dados, 0, Dados.GetUpperBound(0) + 1)
                    Return True

                Catch Exp As System.Exception

                    getExcepção = Exp
                    getResposta = g10phPDF4.Acompanhamento.Mordomo.ErroLogger.DeuExcepção(Exp, g10phPDF4.Basframe.Strings.TransBytesEmString(Dados), True, True, False)

                End Try

            Loop Until getResposta <> MsgBoxResult.Retry

            Return False
        End SyncLock
    End Function

    Public Function DialogoEscreve(ByVal FicheiroNome As String, ByVal Linha As String, ByVal Modo As System.IO.FileMode, ByVal CriaFicheiro As Boolean, ByRef getResposta As MsgBoxResult, ByRef getExcepção As Exception) As Boolean
        SyncLock _locker
            Dim ligação As System.IO.FileStream

            getExcepção = Nothing

            If DialogoAbre(FicheiroNome, Modo, ligação, getResposta, getExcepção) Then

                If DialogoEscreve(ligação, Linha, getResposta, getExcepção) Then
                    Return (g10phPDF4.Basframe.File.Fecha(ligação, getExcepção))
                Else
                    DialogoFecha(ligação, getResposta, getExcepção)
                    Return False
                End If

            End If
        End SyncLock
    End Function

    Public Function DialogoEscreve(ByVal FicheiroNome As String, ByVal Dados() As Byte, ByVal Modo As System.IO.FileMode, ByVal CriaFicheiro As Boolean, ByRef getResposta As MsgBoxResult, ByRef getExcepção As Exception) As Boolean
        SyncLock _locker
            Dim ligação As System.IO.FileStream

            getExcepção = Nothing

            If DialogoAbre(FicheiroNome, Modo, ligação, getResposta, getExcepção) Then

                If DialogoEscreve(ligação, Dados, getResposta, getExcepção) Then
                    Return (DialogoFecha(ligação, getResposta, getExcepção))
                Else
                    DialogoFecha(ligação, getResposta, getExcepção)
                    Return False
                End If

            End If
        End SyncLock
    End Function

    Public Function DialogoLe(ByVal Ligação As System.IO.FileStream, ByRef getResposta As MsgBoxResult, ByRef getExcepção As Exception) As String
        SyncLock _locker
            Dim Conteudo() As Byte

            ReDim Conteudo(CInt(Ligação.Length))

            getExcepção = Nothing

            Try

                Ligação.Read(Conteudo, 0, CInt(Ligação.Length))
                ReDim Preserve Conteudo(CInt(Ligação.Length - 1)) ' é necessário retirar o ultimo caracter NULO
                Return g10phPDF4.Basframe.Strings.TransBytesEmString(Conteudo)

            Catch Exp As System.Exception

                getExcepção = Exp
                getResposta = g10phPDF4.Acompanhamento.Mordomo.ErroLogger.DeuExcepção(Exp, "", True, True, False)

            End Try

            Return Nothing
        End SyncLock
    End Function

    Public Function DialogoLe(ByVal FicheiroNome As String, ByRef getResposta As MsgBoxResult, ByRef getExcepção As Exception) As String
        SyncLock _locker
            Dim ligação As System.IO.FileStream
            Dim st As String

            getExcepção = Nothing

            If DialogoAbre(FicheiroNome, IO.FileMode.Open, ligação, getResposta, getExcepção) Then

                st = DialogoLe(ligação, getResposta, getExcepção)
                If g10phPDF4.Basframe.File.Fecha(ligação, getExcepção) Then
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