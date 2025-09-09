Imports phBASFRAME.g10phPDF
Imports phSETFRAME.g10phPDF

Namespace Acompanhamento

    Public Interface IMORDOMO

        Property Dicionario() As String()
        Property Contexto() As Basframe.basContexto
        Property Mensageiro() As IMENSAGEIRO
        Property ErroLogger() As Basframe.IERROLOG

        Property Debugging() As Boolean

    End Interface

    Public Interface IMENSAGEIRO

        Function PostMensagem(ByVal Mensagem As String, ByVal BotõesIcons As MsgBoxStyle) As MsgBoxResult

        Function ExcepçãoAvisaUtilizador(ByVal Excepção As System.Exception, ByVal Parametros As String, ByVal AbortRetryIgnore As Boolean, ByVal Critico As Boolean) As MsgBoxResult

        Function ExcepçãoAvisaProgramador(ByVal Excepção As System.Exception, ByVal Parametros As String, ByVal AbortRetryIgnore As Boolean, ByVal Critico As Boolean) As MsgBoxResult

        Function InfoFicheiroNãoEncontrado(ByVal Caminho As String, ByVal PerguntaSeCria As Boolean) As MsgBoxResult
        Function InfoPastaNãoExiste(ByVal Caminho As String, ByVal PerguntaSeCria As Boolean) As MsgBoxResult
        Function AskCriarFicheiro(ByVal Caminho As String) As MsgBoxResult
        Function AskCriarPasta(ByVal Caminho As String) As MsgBoxResult

        Function InfoAplicaçãoNãoConfigurada(ByVal Aplicação As Setframe.setAplicação) As MsgBoxResult
        Function InfoClienteDbNãoConfigurado(ByVal ClienteDB As Setframe.setClienteDb) As MsgBoxResult

        Function InfoConectorDbNãoObtido(ByVal Repositorio As Setframe.setRepositorio) As MsgBoxResult
        Function InfoConexãoDbNãoEstabelecida(ByVal Repositorio As Setframe.setRepositorio) As MsgBoxResult

        Function InfoSessãoNãoConfigurada(ByVal Aplicação As Setframe.setAplicação) As MsgBoxResult

        Function InfoImpossivelLançarAplicação(ByVal Aplicação As Setframe.setAplicação) As MsgBoxResult
        Function InfoImpossivelLançarSessão(ByVal Aplicação As Setframe.setAplicação) As MsgBoxResult

    End Interface

    Public Class MordomoVB
        Implements IMORDOMO

        Private l_contexto As Basframe.basContexto
        Private l_dicionario() As String
        Private l_mensageiro As IMENSAGEIRO
        Private l_cusco As Basframe.IERROLOG
        Private l_debugging As Boolean

        Private Shared ReadOnly _locker As New Object()

        Public Sub New()

        End Sub

        Public Sub New(ByVal p_contexto As Basframe.basContexto)
            SyncLock _locker
                l_contexto = p_contexto
            End SyncLock
        End Sub

        Public Property Contexto() As Basframe.basContexto Implements IMORDOMO.Contexto
            Get
                SyncLock _locker
                    Return l_contexto
                End SyncLock
            End Get
            Set(ByVal Value As Basframe.basContexto)
                SyncLock _locker
                    l_contexto = Value
                End SyncLock

            End Set
        End Property

        Public Property Dicionario() As String() Implements IMORDOMO.Dicionario
            Get
                SyncLock _locker
                    Return l_dicionario
                End SyncLock
            End Get
            Set(ByVal Value() As String)
                SyncLock _locker
                    l_dicionario = Value
                End SyncLock
            End Set
        End Property

        Public Property Mensageiro() As IMENSAGEIRO Implements IMORDOMO.Mensageiro
            Get
                SyncLock _locker
                    Return l_mensageiro
                End SyncLock
            End Get
            Set(ByVal Value As IMENSAGEIRO)
                SyncLock _locker
                    l_mensageiro = Value
                End SyncLock
            End Set
        End Property

        Public Property Cusco() As Basframe.IERROLOG Implements IMORDOMO.ErroLogger
            Get
                SyncLock _locker
                    Return l_cusco
                End SyncLock
            End Get
            Set(ByVal Value As Basframe.IERROLOG)
                SyncLock _locker
                    l_cusco = Value
                End SyncLock
            End Set
        End Property

        Public Property Debugging() As Boolean Implements IMORDOMO.Debugging
            Get
                SyncLock _locker
                    Return l_debugging
                End SyncLock
            End Get
            Set(ByVal Value As Boolean)
                SyncLock _locker
                    l_debugging = Value
                End SyncLock
            End Set
        End Property

    End Class

    Public Class MensageiroWinforms
        Implements IMENSAGEIRO

        Private Shared ReadOnly _locker As New Object()

        Protected Mordomo As IMORDOMO

        Public Sub New(ByVal ConstrutorNulo As Boolean)

        End Sub

        Public Sub New(ByVal p_mordomo As IMORDOMO)
            SyncLock _locker
                Me.Mordomo = p_mordomo
            End SyncLock
        End Sub

        Public Overridable Function PostMensagem(ByVal Mensagem As String, ByVal BotõesIcons As Microsoft.VisualBasic.MsgBoxStyle) As Microsoft.VisualBasic.MsgBoxResult Implements IMENSAGEIRO.PostMensagem
            SyncLock _locker
                Return MsgBox(Mensagem, BotõesIcons, Me.Mordomo.Contexto.Topo.Titulo)
            End SyncLock
        End Function

        Public Function InfoFicheiroNãoEncontrado(ByVal Caminho As String, ByVal PerguntaSeCria As Boolean) As MsgBoxResult Implements IMENSAGEIRO.InfoFicheiroNãoEncontrado
            SyncLock _locker
                Dim Msg As String
                Dim Estilo As MsgBoxStyle

                If PerguntaSeCria Then
                    Msg = Me.Mordomo.Dicionario(52)
                    Estilo = MsgBoxStyle.Question Or MsgBoxStyle.YesNoCancel
                Else
                    Msg = Me.Mordomo.Dicionario(51)
                    Estilo = MsgBoxStyle.Exclamation Or MsgBoxStyle.AbortRetryIgnore
                End If

                ParametrizaSobrePaths(Msg, Caminho)

                Return PostMensagem(Msg, Estilo)
            End SyncLock
        End Function

        Public Function InfoPastaNãoExiste(ByVal Caminho As String, ByVal PerguntaSeCria As Boolean) As MsgBoxResult Implements IMENSAGEIRO.InfoPastaNãoExiste
            SyncLock _locker
                Dim Msg As String
                Dim Estilo As MsgBoxStyle

                If PerguntaSeCria Then
                    Msg = Me.Mordomo.Dicionario(53)
                    Estilo = MsgBoxStyle.Question Or MsgBoxStyle.YesNoCancel
                Else
                    Msg = Me.Mordomo.Dicionario(51)
                    Estilo = MsgBoxStyle.Exclamation Or MsgBoxStyle.AbortRetryIgnore
                End If

                ParametrizaSobrePaths(Msg, Caminho)

                Return PostMensagem(Msg, Estilo)
            End SyncLock
        End Function

        Public Function AskCriarFicheiro(ByVal Caminho As String) As MsgBoxResult Implements IMENSAGEIRO.AskCriarFicheiro
            SyncLock _locker
                Dim Msg As String
                Dim Estilo As MsgBoxStyle

                Msg = Me.Mordomo.Dicionario(54)
                Estilo = MsgBoxStyle.Question Or MsgBoxStyle.YesNoCancel

                ParametrizaSobrePaths(Msg, Caminho)

                Return PostMensagem(Msg, Estilo)
            End SyncLock
        End Function

        Public Function AskCriarPasta(ByVal Caminho As String) As MsgBoxResult Implements IMENSAGEIRO.AskCriarPasta
            SyncLock _locker
                Dim Msg As String
                Dim Estilo As MsgBoxStyle

                Msg = Me.Mordomo.Dicionario(55)
                Estilo = MsgBoxStyle.Question Or MsgBoxStyle.YesNoCancel

                ParametrizaSobrePaths(Msg, Caminho)

                Return PostMensagem(Msg, Estilo)
            End SyncLock
        End Function

        Public Function ExcepçãoAvisaUtilizador(ByVal Excepção As System.Exception, ByVal Parametros As String, ByVal AbortRetryIgnore As Boolean, ByVal Critico As Boolean) As Microsoft.VisualBasic.MsgBoxResult Implements IMENSAGEIRO.ExcepçãoAvisaUtilizador
            SyncLock _locker
                Dim ifc As Microsoft.VisualBasic.MsgBoxStyle
                Dim aviso As String

                aviso = Avisos.ExcepçãoGetAviso(Excepção, Parametros)
                If Me.Mordomo.Debugging Then aviso = aviso & vbCrLf & vbCrLf & "vv PRÓPRIO DE DEBUG! vv" & vbCrLf & Avisos.ExcepçãoGetRegisto(Excepção, Parametros)

                If Critico Then
                    ifc = MsgBoxStyle.Critical
                Else
                    ifc = MsgBoxStyle.Exclamation
                End If

                If AbortRetryIgnore Then ifc = ifc Or MsgBoxStyle.AbortRetryIgnore

                Return PostMensagem(aviso, ifc)
            End SyncLock
        End Function

        Public Function ExcepçãoAvisaProgramador(ByVal Excepção As System.Exception, ByVal Parametros As String, ByVal AbortRetryIgnore As Boolean, ByVal Critico As Boolean) As Microsoft.VisualBasic.MsgBoxResult Implements IMENSAGEIRO.ExcepçãoAvisaProgramador
            SyncLock _locker
                Dim ifc As Microsoft.VisualBasic.MsgBoxStyle
                Dim aviso As String

                If Not Me.Mordomo.Debugging Then Return MsgBoxResult.Ok

                aviso = Avisos.ExcepçãoGetAviso(Excepção, Parametros)
                aviso = aviso & vbCrLf & vbCrLf & Avisos.ExcepçãoGetRegisto(Excepção, Parametros)
                aviso = aviso & vbCrLf & vbCrLf & "-- AVISO EXCLUSIVO DE DEBUG! --"

                If Critico Then
                    ifc = MsgBoxStyle.Critical
                Else
                    ifc = MsgBoxStyle.Exclamation
                End If

                If AbortRetryIgnore Then ifc = ifc Or MsgBoxStyle.AbortRetryIgnore

                Return PostMensagem(aviso, ifc)
            End SyncLock
        End Function


        Public Function InfoAplicaçãoNãoConfigurada(ByVal Aplicação As Setframe.setAplicação) As Microsoft.VisualBasic.MsgBoxResult Implements IMENSAGEIRO.InfoAplicaçãoNãoConfigurada
            SyncLock _locker
                Dim Msg As String
                Dim Estilo As MsgBoxStyle

                Msg = Me.Mordomo.Dicionario(21)
                Estilo = MsgBoxStyle.Exclamation

                Return PostMensagem(Msg, Estilo)
            End SyncLock
        End Function

        Public Function InfoClienteDbNãoConfigurado(ByVal ClienteDB As Setframe.setClienteDb) As Microsoft.VisualBasic.MsgBoxResult Implements IMENSAGEIRO.InfoClienteDbNãoConfigurado
            SyncLock _locker
                Dim Msg As String
                Dim Estilo As MsgBoxStyle

                Msg = Me.Mordomo.Dicionario(22)
                Estilo = MsgBoxStyle.Critical

                Return PostMensagem(Msg, Estilo)
            End SyncLock

        End Function


        Public Function InfoConectorDbNãoObtido(ByVal Repositorio As Setframe.setRepositorio) As Microsoft.VisualBasic.MsgBoxResult Implements IMENSAGEIRO.InfoConectorDbNãoObtido
            SyncLock _locker
                Dim Msg As String
                Dim Estilo As MsgBoxStyle

                Msg = Me.Mordomo.Dicionario(23)
                Estilo = MsgBoxStyle.Critical

                Return PostMensagem(Msg, Estilo)
            End SyncLock
        End Function

        Public Function InfoConexãoDbNãoEstabelecida(ByVal Repositorio As Setframe.setRepositorio) As Microsoft.VisualBasic.MsgBoxResult Implements IMENSAGEIRO.InfoConexãoDbNãoEstabelecida
            SyncLock _locker
                Dim Msg As String
                Dim Estilo As MsgBoxStyle

                Msg = Me.Mordomo.Dicionario(24)
                Estilo = MsgBoxStyle.Critical

                Return PostMensagem(Msg, Estilo)
            End SyncLock
        End Function

        Public Function InfoSessãoNãoConfigurada(ByVal Aplicação As Setframe.setAplicação) As Microsoft.VisualBasic.MsgBoxResult Implements IMENSAGEIRO.InfoSessãoNãoConfigurada
            SyncLock _locker
                Dim Msg As String
                Dim Estilo As MsgBoxStyle

                Msg = Me.Mordomo.Dicionario(25)
                Estilo = MsgBoxStyle.Critical

                Return PostMensagem(Msg, Estilo)
            End SyncLock
        End Function


        Public Function InfoImpossivelLançarAplicação(ByVal Aplicação As Setframe.setAplicação) As Microsoft.VisualBasic.MsgBoxResult Implements IMENSAGEIRO.InfoImpossivelLançarAplicação
            SyncLock _locker
                Dim Msg As String
                Dim Estilo As MsgBoxStyle

                Msg = Me.Mordomo.Dicionario(31)
                Estilo = MsgBoxStyle.Critical

                Return PostMensagem(Msg, Estilo)
            End SyncLock
        End Function

        Public Function InfoImpossivelLançarSessão(ByVal Aplicação As Setframe.setAplicação) As Microsoft.VisualBasic.MsgBoxResult Implements IMENSAGEIRO.InfoImpossivelLançarSessão
            SyncLock _locker
                Dim Msg As String
                Dim Estilo As MsgBoxStyle

                Msg = Me.Mordomo.Dicionario(32)
                Estilo = MsgBoxStyle.Critical

                Return PostMensagem(Msg, Estilo)
            End SyncLock
        End Function

    End Class

    Public Class ErroLoggerFS
        Implements Basframe.IERROLOG

        Private l_mensageiro As IMENSAGEIRO
        Private l_excepção As System.Exception
        Private l_registando As Boolean

        Private l_suspende_registos As Boolean

        Public Mordomo As IMORDOMO
        Public FicheiroPath As String

        Public Event PósExcepção(ByVal Excepção As System.Exception, ByVal Parametros As String, ByVal ComAviso As Boolean, ByVal AbortRetryIgnore As Boolean, ByVal Critico As Boolean, ByVal Opção As Microsoft.VisualBasic.MsgBoxResult) Implements Basframe.IERROLOG.PósExcepção
        Public Event PréExcepção(ByVal Excepção As System.Exception, ByVal Parametros As String, ByVal ComAviso As Boolean, ByVal AbortRetryIgnore As Boolean, ByVal Critico As Boolean, ByRef Cancela As Boolean) Implements Basframe.IERROLOG.PréExcepção

        Private Shared ReadOnly _locker As New Object()

        Public Sub New(ByVal p_mordomo As IMORDOMO, ByVal p_ficheiropath As String)
            SyncLock _locker
                Me.Mordomo = p_mordomo
                Me.FicheiroPath = p_ficheiropath
            End SyncLock
        End Sub

        Public Property UltimaExcepção() As System.Exception Implements Basframe.IERROLOG.UltimaExcepção
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

        Public Property Registando() As Boolean Implements Basframe.IERROLOG.Registando
            Get
                SyncLock _locker
                    Return l_registando
                End SyncLock
            End Get
            Set(ByVal Value As Boolean)
                SyncLock _locker
                    l_registando = Value
                End SyncLock
            End Set
        End Property

        Public Function DeuExcepção(ByVal Excepção As System.Exception, ByVal Parametros As String, ByVal ComAviso As Boolean, ByVal AbortRetryIgnore As Boolean, ByVal Critico As Boolean) As Microsoft.VisualBasic.MsgBoxResult Implements Basframe.IERROLOG.DeuExcepção
            SyncLock _locker
                Me.l_excepção = Excepção

                If l_registando And Not l_suspende_registos Then Me.RegistaExcepção(Excepção, Parametros, Critico)

                If ComAviso Then

                    Return Me.Mordomo.Mensageiro.ExcepçãoAvisaUtilizador(Excepção, Parametros, AbortRetryIgnore, Critico)

                Else

                    Return Me.Mordomo.Mensageiro.ExcepçãoAvisaProgramador(Excepção, Parametros, AbortRetryIgnore, Critico)

                End If
            End SyncLock
        End Function

        Public Function RegistaExcepção(ByVal Excepção As System.Exception, ByVal Parametros As String, ByVal Critico As Boolean) As Boolean Implements Basframe.IERROLOG.RegistaExcepção
            SyncLock _locker
                Dim registo As String

                If l_registando And Not l_suspende_registos Then

                    registo = Avisos.ExcepçãoGetRegisto(Excepção, Parametros)

                    l_suspende_registos = True

                    Basframe.File.Escreve(Me.FicheiroPath, vbCrLf & vbCrLf & registo, IO.FileMode.Append, False, Nothing)

                    l_suspende_registos = False

                End If
            End SyncLock

        End Function

    End Class

    Public Module Avisos

        ' ficheiro_log = File.GetPastaAssemblyBase & "\" & File.GetNomeAssemblyBase & ".log"
        Private ReadOnly _locker As New Object()

        Public Mordomo As IMORDOMO

        Public Sub DicionarioSetBase(ByRef QualDicionario() As String)
            SyncLock _locker
                If QualDicionario Is Nothing OrElse QualDicionario.GetUpperBound(0) < 100 Then ReDim Preserve QualDicionario(100)

                QualDicionario(1) = "Inicialização, Lançamento e Validação da aplicação."
                QualDicionario(2) = "Inicialização da aplicação."
                QualDicionario(3) = "Validação da aplicação."
                QualDicionario(4) = "Lançamento da aplicação."

                QualDicionario(11) = "Inicialização, Lançamento e Validação da sessão de trabalho."
                QualDicionario(12) = "Inicialização da sessão de trabalho."
                QualDicionario(13) = "Validação da sessão de trabalho."
                QualDicionario(14) = "Lançamento da sessão de trabalho."

                QualDicionario(21) = "Não foi possível configurar a aplicação em lançamento!"
                QualDicionario(22) = "Não foi possível configurar o cliente da base de dados!"
                QualDicionario(23) = "Não foi possível obter um conector para a base de dados!"
                QualDicionario(24) = "Não foi possível obter uma conexão à base de dados!"
                QualDicionario(25) = "Não foi possível configurar a sessão de trabalho!"

                QualDicionario(31) = "Não é possível lançar a aplicação!"
                QualDicionario(32) = "Não é possível lançar a sessão de trabalho!"

                QualDicionario(41) = "Processo de login abortado."
                QualDicionario(42) = "Login recusado."
                QualDicionario(43) = "Login Autorizado."

                QualDicionario(50) = "O ficheiro ""%1"" não foi encontrado!"
                QualDicionario(51) = "O directório ""%1"" não existe!"
                QualDicionario(52) = "O ficheiro ""%1"" não foi encontrado!" & vbCrLf & vbCrLf & "Deseja cria-lo?"
                QualDicionario(53) = "O directório ""%1"" não existe!" & vbCrLf & vbCrLf & "Deseja cria-lo?"
                QualDicionario(54) = "Deseja criar o ficheiro ""%1""?"
                QualDicionario(55) = "Deseja criar o directório ""%1""?"
            End SyncLock

        End Sub

        Public Sub ParametrizaSobrePaths(ByRef Frase As String, ByVal Caminho As String)
            SyncLock _locker
                ' %1 representa o caminho completo em questão
                ' %2 representa apenas o caminho para a directoria do caminho em questão
                ' %3 representa apenas o nome do ficheiro do caminho em questão

                If Frase.IndexOf("%1") > -1 Then Frase = Frase.Replace("%1", Caminho)
                If Frase.IndexOf("%2") > -1 Then Frase = Frase.Replace("%2", System.IO.Path.GetDirectoryName(Caminho))
                If Frase.IndexOf("%3") > -1 Then Frase = Frase.Replace("%3", System.IO.Path.GetFileName(Caminho))
            End SyncLock
        End Sub

        Public Function ExcepçãoGetPilha() As String
            SyncLock _locker
                Dim s As Integer
                Dim pilha As New StackTrace(True)
                Dim item As StackFrame
                Dim t As String

                For s = 0 To pilha.FrameCount - 1

                    item = pilha.GetFrame(s)

                    t = t & "   " & item.GetMethod.Name & " [" & item.GetFileLineNumber & ", " & item.GetFileColumnNumber & "] em " & item.GetFileName

                    If s < pilha.FrameCount - 1 Then t = t & vbCrLf

                Next

                Return t
            End SyncLock
        End Function

        Public Function ExcepçãoGetRegisto(ByVal Excepção As System.Exception, ByVal Parametros As String) As String
            SyncLock _locker
                Dim registo As String

                registo = "------------------------------------------------------------------------------------------------------------"
                registo = registo & vbCrLf & "DATA: " & Now.ToString
                registo = registo & vbCrLf & "ERRO: " & Err.Number & " - " & Excepção.GetType.Name & vbCrLf & "   " & Excepção.Message
                registo = registo & vbCrLf & "CTX: " & Avisos.Mordomo.Contexto.Topo.ToString
                registo = registo & vbCrLf & "PAR: " & Parametros
                registo = registo & vbCrLf & "MOD: " & Excepção.Source & "; " & Err.Erl
                registo = registo & vbCrLf & "HELP: " & Err.HelpFile & "; " & Err.HelpContext
                registo = registo & vbCrLf & "FUN: " & Excepção.TargetSite.Name
                registo = registo & vbCrLf & "PILHA: " & vbCrLf & ExcepçãoGetPilha()

                Return registo
            End SyncLock
        End Function

        Public Function ExcepçãoGetAviso(ByVal Excepção As System.Exception, ByVal Parametros As String) As String
            SyncLock _locker
                Dim aviso As String

                aviso = aviso & Err.Number & " - " & Excepção.GetType.Name & vbCrLf & Excepção.Message
                If Parametros.Length > 0 Then aviso = aviso & vbCrLf & vbCrLf & Parametros

                Return aviso
            End SyncLock
        End Function

    End Module

End Namespace
