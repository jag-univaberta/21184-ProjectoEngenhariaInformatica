Imports phDATAFRAME.g10phPDF

Namespace Setframe

    Public Module Setframe

        Private ReadOnly _locker As New Object()

        Public Function ManiaPorNome(ByVal ManiaNome As String) As Dataframe.datManias
            SyncLock _locker
                Select Case ManiaNome.ToLower

                    Case "access" : Return Dataframe.datManias.Access
                    Case "informix" : Return Dataframe.datManias.Informix
                    Case "oracle", "oracle7" : Return Dataframe.datManias.Oracle7
                    Case "oracle8" : Return Dataframe.datManias.Oracle8
                    Case "sqlserver" : Return Dataframe.datManias.SqlServer
                    Case "postgre" : Return Dataframe.datManias.Postgre
                    Case Else : Return Dataframe.datManias.NãoDefinido

                End Select
            End SyncLock
        End Function

        Public Function ProviderPorNome(ByVal ProviderNome As String) As setProviders
            SyncLock _locker
                Select Case ProviderNome.ToLower

                    Case "odbcclient" : Return setProviders.OdbcClient
                    Case "oledbclient" : Return setProviders.OleDbClient
                    Case "msoracleclient" : Return setProviders.MsOracleClient
                    Case "sqlclient" : Return setProviders.SqlClient
                    Case "postgreclient" : Return setProviders.PostgreClient
                    Case Else : Return setProviders.NãoDefinido

                End Select
            End SyncLock
        End Function

        Public Function ProviderSugereDll(ByVal Provider As setProviders) As String
            SyncLock _locker
                Select Case Provider

                    Case setProviders.OdbcClient : Return "System.Data.dll"
                    Case setProviders.OleDbClient : Return "System.Data.dll"
                    Case setProviders.MsOracleClient : Return "System.Data.OracleClient.dll"
                    Case setProviders.SqlClient : Return "System.Data.dll"
                    Case setProviders.PostgreClient : Return "Npgsql.dll"
                End Select
            End SyncLock
        End Function

        Public Function ProviderSugereModulo(ByVal Provider As setProviders) As String
            SyncLock _locker
                Select Case Provider

                    Case setProviders.OdbcClient : Return "System.Data.dll"
                    Case setProviders.OleDbClient : Return "System.Data.dll"
                    Case setProviders.MsOracleClient : Return "System.Data.OracleClient.dll"
                    Case setProviders.SqlClient : Return "System.Data.dll"
                    Case setProviders.PostgreClient : Return "Npgsql.dll"
                End Select
            End SyncLock
        End Function

        Public Function ProviderSugereTipo(ByVal Provider As setProviders) As String
            SyncLock _locker
                Select Case Provider

                    Case setProviders.OdbcClient : Return "OdbcConnection"
                    Case setProviders.OleDbClient : Return "OleDbConnection"
                    Case setProviders.MsOracleClient : Return "OracleConnection"
                    Case setProviders.SqlClient : Return "SqlConnection"
                    Case setProviders.PostgreClient : Return "NpgsqlConnection"
                End Select
            End SyncLock
        End Function

        Public Function ProviderSugereNome(ByVal Provider As setProviders) As String
            SyncLock _locker
                Select Case Provider

                    Case setProviders.OdbcClient : Return "OdbcClient"
                    Case setProviders.OleDbClient : Return "OleDbClient"
                    Case setProviders.MsOracleClient : Return "MsOracleClient"
                    Case setProviders.SqlClient : Return "SqlClient"
                    Case setProviders.PostgreClient : Return "PostgreClient"
                End Select
            End SyncLock
        End Function


        Public Function AssemblagemLoad(ByVal FicheiroDLL As String) As System.Reflection.Assembly
            SyncLock _locker
                If FicheiroDLL.Length > 0 Then

                    Try

                        Return System.Reflection.Assembly.LoadFrom(FicheiroDLL)

                    Catch Exp As Exception

                        g10phPDF4.Basframe.ErroLogger.DeuExcepção(Exp, "", True, False, False)

                    End Try

                End If

                Return Nothing
            End SyncLock
        End Function

        Public Function AssemblagemLoadModulo(ByVal FicheiroDLL As String, ByVal ModuloNome As String) As System.Reflection.Module
            SyncLock _locker
                Dim Assemblagem As System.Reflection.Assembly
                Dim Modulo As System.Reflection.Module
                Dim Tipo As System.Type

                Assemblagem = AssemblagemLoad(FicheiroDLL)

                If Assemblagem Is Nothing Then

                    Return Nothing

                Else

                    For Each Modulo In Assemblagem.GetModules

                        If Modulo.Name = ModuloNome Then

                            Return Modulo
                            Exit For

                        End If

                    Next

                    Return Nothing

                End If
            End SyncLock
        End Function


        Public Function AssemblagemLoadTipo(ByVal FicheiroDLL As String, ByVal ModuloNome As String, ByVal TipoNome As String) As System.Type
            SyncLock _locker
                Dim Assemblagem As System.Reflection.Assembly
                Dim Modulo As System.Reflection.Module
                Dim Tipo As System.Type

                If ModuloNome Is Nothing Then ModuloNome = ""

                Assemblagem = AssemblagemLoad(FicheiroDLL)

                If Assemblagem Is Nothing Then

                    Return Nothing

                Else

                    For Each Modulo In Assemblagem.GetModules

                        If Modulo.Name = ModuloNome Or ModuloNome.Length = 0 Then

                            For Each Tipo In Modulo.GetTypes

                                If Tipo.Name = TipoNome Then

                                    Return Tipo
                                    Exit For

                                End If

                            Next

                            Exit For

                        End If

                    Next

                    Return Nothing

                End If
            End SyncLock
        End Function

        Public Function AssemblagemLoadTipo(ByVal FicheiroDLL As String, ByVal ModuloNome As String, ByVal AtributoTemplate As System.Attribute) As System.Type
            SyncLock _locker
                Dim Assemblagem As System.Reflection.Assembly
                Dim Modulo As System.Reflection.Module
                Dim Tipo As System.Type

                If ModuloNome Is Nothing Then ModuloNome = ""

                Assemblagem = AssemblagemLoad(FicheiroDLL)

                If Assemblagem Is Nothing Then

                    Return Nothing

                Else

                    For Each Modulo In Assemblagem.GetModules

                        If Modulo.Name = ModuloNome Or ModuloNome.Length = 0 Then

                            For Each Tipo In Modulo.GetTypes

                                If Not AssemblagemLoadAtributo(Tipo, AtributoTemplate) Is Nothing Then

                                    Return Tipo

                                End If

                            Next

                            Exit For

                        End If

                    Next

                    Return Nothing

                End If
            End SyncLock
        End Function

        Public Function AssemblagemLoadTipo(ByVal FicheiroDLL As String, ByVal ModuloNome As String, ByVal AtributoTemplate As System.Attribute, ByRef getTipos() As System.Type) As Boolean
            SyncLock _locker
                Dim Assemblagem As System.Reflection.Assembly
                Dim Modulo As System.Reflection.Module
                Dim Tipo As System.Type
                Dim t As Integer

                If ModuloNome Is Nothing Then ModuloNome = ""

                Assemblagem = AssemblagemLoad(FicheiroDLL)

                If Assemblagem Is Nothing Then

                    Return False

                Else

                    t = -1

                    For Each Modulo In Assemblagem.GetModules

                        If Modulo.Name = ModuloNome Or ModuloNome.Length = 0 Then

                            For Each Tipo In Modulo.GetTypes

                                If Not AssemblagemLoadAtributo(Tipo, AtributoTemplate) Is Nothing Then

                                    t = t + 1
                                    ReDim Preserve getTipos(t)
                                    getTipos(t) = Tipo

                                End If

                            Next

                            Exit For

                        End If

                    Next

                    Return (t > -1)

                End If
            End SyncLock
        End Function

        Public Function AssemblagemLoadTipo(ByVal Assemblagem As System.Reflection.Assembly, ByVal ModuloNome As String, ByVal TipoNome As String) As System.Type
            SyncLock _locker
                Dim Modulo As System.Reflection.Module
                Dim Tipo As System.Type

                If ModuloNome Is Nothing Then ModuloNome = ""

                If Assemblagem Is Nothing Then

                    Return Nothing

                Else

                    For Each Modulo In Assemblagem.GetModules

                        If Modulo.Name = ModuloNome Or ModuloNome.Length = 0 Then

                            For Each Tipo In Modulo.GetTypes

                                If Tipo.Name = TipoNome Then

                                    Return Tipo
                                    Exit For

                                End If

                            Next

                            Exit For

                        End If

                    Next

                    Return Nothing

                End If
            End SyncLock

        End Function

        Public Function AssemblagemLoadTipo(ByVal Assemblagem As System.Reflection.Assembly, ByVal ModuloNome As String, ByVal AtributoTemplate As System.Attribute) As System.Type
            SyncLock _locker
                Dim Modulo As System.Reflection.Module
                Dim Tipo As System.Type

                If ModuloNome Is Nothing Then ModuloNome = ""

                If Assemblagem Is Nothing Then

                    Return Nothing

                Else

                    For Each Modulo In Assemblagem.GetModules

                        If Modulo.Name = ModuloNome Or ModuloNome.Length = 0 Then

                            For Each Tipo In Modulo.GetTypes

                                If Not AssemblagemLoadAtributo(Tipo, AtributoTemplate) Is Nothing Then

                                    Return Tipo

                                End If

                            Next

                            Exit For

                        End If

                    Next

                    Return Nothing

                End If
            End SyncLock
        End Function

        Public Function AssemblagemLoadTipo(ByVal Assemblagem As System.Reflection.Assembly, ByVal ModuloNome As String, ByVal AtributoTemplate As System.Attribute, ByRef getTipos() As System.Type) As Boolean
            SyncLock _locker
                Dim Modulo As System.Reflection.Module
                Dim Tipo As System.Type
                Dim t As Integer

                If ModuloNome Is Nothing Then ModuloNome = ""

                If Assemblagem Is Nothing Then

                    Return False

                Else

                    t = -1

                    For Each Modulo In Assemblagem.GetModules

                        If Modulo.Name = ModuloNome Or ModuloNome.Length = 0 Then

                            For Each Tipo In Modulo.GetTypes

                                If Not AssemblagemLoadAtributo(Tipo, AtributoTemplate) Is Nothing Then

                                    t = t + 1
                                    ReDim Preserve getTipos(t)
                                    getTipos(t) = Tipo

                                End If

                            Next

                            Exit For

                        End If

                    Next

                    Return (t > -1)

                End If
            End SyncLock
        End Function



        Public Function AssemblagemConstroiTipo(ByVal FicheiroDLL As String, ByVal Modulo As String, ByVal TipoNomeCompleto As String) As Object
            SyncLock _locker
                Dim Assemblagem As System.Reflection.Assembly
                Dim Tipo As System.Type
                Dim Objecto As System.Runtime.Remoting.ObjectHandle

                Assemblagem = AssemblagemLoad(FicheiroDLL)
                Tipo = AssemblagemLoadTipo(Assemblagem, Modulo, TipoNomeCompleto)

                If Assemblagem Is Nothing Then

                    Return Nothing

                Else

                    Return AssemblagemConstroiTipo(Assemblagem, Tipo)

                End If
            End SyncLock
        End Function

        Public Function AssemblagemConstroiTipo(ByVal Assemblagem As System.Reflection.Assembly, ByVal Modulo As String, ByVal TipoNomeCompleto As String) As Object
            SyncLock _locker
                Dim Tipo As System.Type
                Dim Objecto As System.Runtime.Remoting.ObjectHandle

                Tipo = AssemblagemLoadTipo(Assemblagem, Modulo, TipoNomeCompleto)

                If Assemblagem Is Nothing Or Tipo Is Nothing Then

                    Return Nothing

                Else

                    Return AssemblagemConstroiTipo(Assemblagem, Tipo)

                End If
            End SyncLock
        End Function

        Public Function AssemblagemConstroiTipo(ByVal Assemblagem As System.Reflection.Assembly, ByVal Tipo As System.Type) As Object
            SyncLock _locker
                Dim Objecto As System.Runtime.Remoting.ObjectHandle

                Try

                    ' o método Assembly.CreateInstance usa o System Activator!
                    ' esta forma obriga a que o assembly esteja na pasta da aplicação corrente
                    Objecto = Activator.CreateInstance(Assemblagem.FullName, _
                                                        Tipo.FullName, _
                                                        True, _
                                                        Reflection.BindingFlags.CreateInstance Or _
                                                        Reflection.BindingFlags.Instance Or _
                                                        Reflection.BindingFlags.Public, _
                                                        Nothing, _
                                                        Nothing, _
                                                        Nothing, _
                                                        Nothing, _
                                                        Nothing)

                    Return Objecto.Unwrap

                Catch Exp As Exception

                    g10phPDF4.Basframe.ErroLogger.DeuExcepção(Exp, "", True, False, False)

                End Try

                Return Nothing
            End SyncLock
        End Function

        Public Function AssemblagemConstroiTipoDirecto(ByVal Tipo As System.Type) As Object
            SyncLock _locker
                Try

                    Return (Activator.CreateInstance(Tipo))

                Catch Exp As Exception

                    g10phPDF4.Basframe.ErroLogger.DeuExcepção(Exp, "", True, False, False)

                End Try

                Return Nothing
            End SyncLock
        End Function

        Public Function AssemblagemLoadAtributo(ByVal Tipo As System.Type, ByVal AtributoTemplate As System.Attribute) As System.Attribute
            SyncLock _locker
                Dim atributo As System.Attribute

                For Each atributo In Tipo.GetCustomAttributes(False)

                    If atributo.GetType Is AtributoTemplate.GetType Then Return atributo

                Next
            End SyncLock
        End Function

    End Module

    Public Enum setProviders

        NãoDefinido = 0

        OdbcClient = 1
        OleDbClient = 2
        MsOracleClient = 3
        SqlClient = 4
        PostgreClient = 5
    End Enum

    '                                                                                                   

    Public Class setLogin

        Public Uid As String
        Public Pwd As String
        Public ComCifra As Boolean

        Public Cifra As g10phPDF4.Basframe.Cripto.ICRIPTOSISTEMA

        Public Encriptado As Boolean
        Public OriginalUid As String
        Public OriginalPwd As String

        Private Shared ReadOnly _locker As New Object()

        Public Sub Encripta()
            SyncLock _locker
                Me.OriginalUid = Me.Uid
                Me.OriginalPwd = Me.Pwd

                Me.Uid = Me.Cifra.Codifica(Me.Uid)
                Me.Pwd = Me.Cifra.Codifica(Me.Pwd)

                Me.Encriptado = True
            End SyncLock
        End Sub

        Public Sub Desencripta()
            SyncLock _locker
                Me.OriginalUid = Me.Uid
                Me.OriginalPwd = Me.Pwd

                Me.Uid = Me.Cifra.Descodifica(Me.Uid)
                Me.Pwd = Me.Cifra.Descodifica(Me.Pwd)

                Me.Encriptado = False
            End SyncLock
        End Sub

    End Class

    Public Class setUtilizador
        Inherits setLogin

    End Class

    ' 01/01/2004    frank       Servidor e Canais de Acesso
    ' O ClienteDB recebe um canal para um ficheiro de configuração, através dele auto-configura-se, 
    ' com essa informação pode criar o primeiro canal de acesso à base de dados de trabalho
    ' esse canal pode ser usado para tentar conexão (logins e permissões) à base de dados
    ' uma vez validado o login pode ser ordenada a criação dos acessos secundários aos repositorios auxiliares.

    ' O login a usar pela conexão ao servidor já está definido na classe Servidor e presume-se autorizado.
    ' O ciclo de login trabalha com o utilizador da aplicação, e deve verificar se o utilizador está autorizado.
    ' Eventualmente o login da conexão não foi indicado e então será igual ao utilizador da aplicação
    '   caso a conexão funcione o login da conexão ao servidor passa a ser esse.
    ' Após cada verificação do login o servidor irá criar todos os canais de acesso auxiliares.

    Public Class setClienteDb
        Inherits Dataframe.DatletBase

        ' todo: setClienteDN: método para, a partir do id de uma tabela, saber qual o repositório e canal a usar

        Public Mania As Dataframe.datManias
        Public Provider As setProviders
        Public ProviderDLL As String
        Public ProviderModulo As String
        Public ProviderTipo As String
        Public Datasource As String
        Public Database As String
        Public Setup As String
        Public DbLogin As New setLogin

        Public AplicaçãoNome As String
        Public Repositorios() As setRepositorio

        Private Shared ReadOnly _locker As New Object()

        Public Sub New()
            SyncLock _locker

                Me.l_info = New Dataframe.datInfo
                Me.l_info.Objecto = New Dataframe.datObjecto("conector", 101)

                Me.l_info.Objecto.MembroDef("mania", DbType.String)
                Me.l_info.Objecto.MembroDef("provider", DbType.String)
                Me.l_info.Objecto.MembroDef("dll", DbType.String)
                Me.l_info.Objecto.MembroDef("modulo", DbType.String)
                Me.l_info.Objecto.MembroDef("tipo", DbType.String)
                Me.l_info.Objecto.MembroDef("datasource", DbType.String)
                Me.l_info.Objecto.MembroDef("database", DbType.String)
                Me.l_info.Objecto.MembroDef("setup", DbType.String)
                Me.l_info.Objecto.MembroDef("uid", DbType.String)
                Me.l_info.Objecto.MembroDef("pwd", DbType.String)
                Me.l_info.Objecto.MembroDef("cifrado", DbType.String)
                Me.l_info.AtributoDef(Me.l_info.Objecto)
                Me.private_resetpreparados()
            End SyncLock
        End Sub

        Public Sub New(ByVal Canal As Dataframe.datCanal)

            Me.new()
            SyncLock _locker
                Me.l_canal = Canal
            End SyncLock
        End Sub

        Public Overrides Property DatAtributo(ByVal Index As Integer) As Object
            Get
                SyncLock _locker
                    Select Case Index

                        Case 0 : Return Me.Mania
                        Case 1 : Return Me.Provider
                        Case 2 : Return Me.ProviderDLL
                        Case 3 : Return Me.ProviderModulo
                        Case 4 : Return Me.ProviderTipo
                        Case 5 : Return Me.Datasource
                        Case 6 : Return Me.Database
                        Case 7 : Return Me.Setup
                        Case 8 : Return Me.DbLogin.Uid
                        Case 9 : Return Me.DbLogin.Pwd
                        Case 10 : Return Me.DbLogin.ComCifra

                    End Select
                End SyncLock
            End Get
            Set(ByVal Value As Object)
                SyncLock _locker
                    Select Case Index

                        Case 0
                            If TypeOf Value Is String Then
                                Me.Mania = Setframe.ManiaPorNome(CType(Value, String))
                            Else
                                Me.Mania = CType(Value, Dataframe.datManias)
                            End If
                        Case 1
                            If TypeOf Value Is String Then
                                Me.Provider = Setframe.ProviderPorNome(CType(Value, String))
                            Else
                                Me.Provider = CType(Value, setProviders)
                            End If
                        Case 2 : Me.ProviderDLL = CType(Value, String)
                        Case 3 : Me.ProviderModulo = CType(Value, String)
                        Case 4 : Me.ProviderTipo = CType(Value, String)
                        Case 5 : Me.Datasource = CType(Value, String)
                        Case 6 : Me.Database = CType(Value, String)
                        Case 7 : Me.Setup = CType(Value, String)
                        Case 8 : Me.DbLogin.Uid = CType(Value, String)
                        Case 9 : Me.DbLogin.Pwd = CType(Value, String)
                        Case 10 : Me.DbLogin.ComCifra = g10phPDF4.Basframe.Cast2Boolean(Value)

                    End Select
                End SyncLock
            End Set
        End Property

        Public Function Config() As Boolean
            SyncLock _locker
                If Me.DatConsultaPrimeiro() Then

                    ' acerto dos dados lidos:
                    ' alguns dos dados são opcionais pelo que na sua omissão estes devem ser os valores a usar

                    If ProviderDLL.Length = 0 Then ProviderDLL = g10phPDF4.Basframe.File.GetPastaFrameworkNet() & "\" & ProviderSugereDll(Me.Provider)
                    If ProviderModulo.Length = 0 Then ProviderModulo = ProviderSugereModulo(Me.Provider)
                    If ProviderTipo.Length = 0 Then ProviderTipo = ProviderSugereTipo(Me.Provider)

                    If Me.DbLogin.ComCifra Then
                        Me.DbLogin.Desencripta()
                    Else
                        Me.DbLogin.Encriptado = False
                    End If

                    Return True

                Else

                    Return False

                End If
            End SyncLock
        End Function

        Property AcessoPrimario() As setRepositorio
            Get
                SyncLock _locker
                    Return Repositorios(0)
                End SyncLock
            End Get
            Set(ByVal Value As setRepositorio)
                SyncLock _locker
                    If Repositorios Is Nothing Then ReDim Repositorios(0)
                    Repositorios(0) = Value
                End SyncLock
            End Set
        End Property

        Public Function CriaAcessoPrimario() As Boolean
            SyncLock _locker
                Dim repositorio As New setRepositorio

                repositorio.Mania = Me.Mania
                repositorio.Provider = Me.Provider
                repositorio.ProviderDLL = Me.ProviderDLL
                repositorio.ProviderModulo = Me.ProviderModulo
                repositorio.ProviderTipo = Me.ProviderTipo
                repositorio.Datasource = Me.Datasource
                repositorio.Database = Me.Database
                repositorio.Setup = Me.Setup
                repositorio.ClienteDb = Me

                If repositorio.Cria() Then

                    ReDim Repositorios(0)
                    Repositorios(0) = repositorio

                    Return True

                Else

                    Return False

                End If
            End SyncLock
        End Function

        Public Function CriaAcessos() As Boolean
            SyncLock _locker
                Dim LeitorDeAcessos As New setRepositorio(Me.Repositorios(0).DatCanal)
                Dim Acessos() As setRepositorio
                Dim Sucesso As Boolean

                Sucesso = True
                Acessos = CType(LeitorDeAcessos.DatCast(LeitorDeAcessos.DatSeleccionaFichas), setRepositorio())

                If Acessos Is Nothing Then

                    Return False

                Else

                    For Each Acesso As setRepositorio In Acessos

                        Acesso.ClienteDb = Me

                        If Not Acesso.Cria() Then

                            Sucesso = False
                            Exit For

                        End If
                    Next

                    If Sucesso Then

                        ReDim Preserve Me.Repositorios(Acessos.GetUpperBound(0) + 1)
                        Acessos.CopyTo(Me.Repositorios, 1)

                    End If

                End If

                Return Sucesso
            End SyncLock
        End Function

        Public Overrides Function DatCast(ByVal Lista() As Dataframe.ILET) As Dataframe.ILET()

        End Function

        Public Overrides Function DatClone() As Dataframe.ILET

        End Function

        Public Overrides Function DatNew() As Dataframe.ILET

        End Function
    End Class

    Public Class setRepositorio
        Inherits Dataframe.DatletBase

        Public ID As Integer
        Public Nome As String
        Public Titulo As String
        Public Obs As String
        Public PicPath As String
        Public PicIndex As Integer

        Public Mania As Dataframe.datManias
        Public Provider As setProviders
        Public ProviderDLL As String
        Public ProviderModulo As String
        Public ProviderTipo As String
        Public Datasource As String
        Public Database As String
        Public Setup As String
        Public Dono As String

        Public ClienteDb As setClienteDb
        Public Assemblagem As System.Reflection.Assembly
        Public Conector As System.Data.IDbConnection

        Public StringDeConexão As String

        Private Shared ReadOnly _locker As New Object()

        Public Sub New()
            SyncLock _locker

                Me.l_info = New Dataframe.datInfo
                Me.l_info.Objecto = New Dataframe.datObjecto("appDB", 101)


                Me.l_info.Objecto.MembroDef("iDBdb", DbType.Int32) : Me.l_info.Objecto.MembroSetPonteiro()
                Me.l_info.Objecto.MembroDef("sDBnome", DbType.String) : Me.l_info.Objecto.MembroSetChave()
                Me.l_info.Objecto.MembroDef("sDBtit", DbType.String) : Me.l_info.Objecto.MembroSetDescrição()
                Me.l_info.Objecto.MembroDef("mDBobs", DbType.String)
                Me.l_info.Objecto.MembroDef("sDBpicpath", DbType.String)
                Me.l_info.Objecto.MembroDef("iDBpicindex", DbType.Int32)
                Me.l_info.Objecto.MembroDef("tDBmania", DbType.Int32)
                Me.l_info.Objecto.MembroDef("tDBprovider", DbType.Int32)
                Me.l_info.Objecto.MembroDef("sDBproviderDLL", DbType.String)
                Me.l_info.Objecto.MembroDef("sDBproviderMOD", DbType.String)
                Me.l_info.Objecto.MembroDef("sDBproviderTYP", DbType.String)
                Me.l_info.Objecto.MembroDef("sDBdatasource", DbType.String)
                Me.l_info.Objecto.MembroDef("sDBdatabase", DbType.String)
                Me.l_info.Objecto.MembroDef("sDBsetup", DbType.String)
                Me.l_info.Objecto.MembroDef("sDBdono", DbType.String)

                Me.l_info.AtributoDef(Me.l_info.Objecto)
                Me.private_resetpreparados()
            End SyncLock
        End Sub

        Public Sub New(ByVal Canal As Dataframe.datCanal)

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
                        Case 1 : Return Me.Nome
                        Case 2 : Return Me.Titulo
                        Case 3 : Return Me.Obs
                        Case 4 : Return Me.PicPath
                        Case 5 : Return Me.PicIndex
                        Case 6 : Return Me.Mania
                        Case 7 : Return Me.Provider
                        Case 8 : Return Me.ProviderDLL
                        Case 9 : Return Me.ProviderModulo
                        Case 0 : Return Me.ProviderTipo
                        Case 11 : Return Me.Datasource
                        Case 12 : Return Me.Database
                        Case 13 : Return Me.Setup
                        Case 14 : Return Me.Dono

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
                        Case 6 : Me.Mania = CType(Value, Dataframe.datManias)
                        Case 7 : Me.Provider = CType(Value, setProviders)
                        Case 8 : Me.ProviderDLL = CType(Value, String)
                        Case 9 : Me.ProviderModulo = CType(Value, String)
                        Case 10 : Me.ProviderTipo = CType(Value, String)
                        Case 11 : Me.Datasource = CType(Value, String)
                        Case 12 : Me.Database = CType(Value, String)
                        Case 13 : Me.Setup = CType(Value, String)
                        Case 14 : Me.Dono = CType(Value, String)

                    End Select
                End SyncLock
            End Set
        End Property

        Public Overrides Function DatClone() As Dataframe.ILET
            SyncLock _locker
                Dim r As New setRepositorio

                MyBase.private_clone(r)

                r.ID = Me.ID
                r.Nome = Me.Nome
                r.Titulo = Me.Titulo
                r.Obs = Me.Obs
                r.PicPath = Me.PicPath
                r.PicIndex = Me.PicIndex

                r.Mania = Me.Mania
                r.Provider = Me.Provider
                r.ProviderDLL = Me.ProviderDLL
                r.ProviderModulo = ProviderModulo
                r.ProviderTipo = Me.ProviderTipo
                r.Datasource = Me.Datasource
                r.Database = Me.Database
                r.Setup = Me.Setup
                r.Dono = Me.Dono

                r.ClienteDb = Me.ClienteDb
                r.Assemblagem = Me.Assemblagem
                r.Conector = Me.Conector

                Return r
            End SyncLock
        End Function

        Public Overrides Function DatNew() As Dataframe.ILET
            SyncLock _locker
                Return New setRepositorio
            End SyncLock
        End Function

        Public Overrides Function DatCast(ByVal Lista() As Dataframe.ILET) As Dataframe.ILET()
            SyncLock _locker
                Dim r() As setRepositorio
                Dim a As Integer

                If Lista Is Nothing Then
                    Return Nothing
                Else
                    ReDim r(Lista.GetUpperBound(0))

                    For a = 0 To Lista.GetUpperBound(0)

                        r(a) = DirectCast(Lista(a), setRepositorio)

                    Next

                    Return r
                End If
            End SyncLock
        End Function


        Public Function Cria() As Boolean
            SyncLock _locker
                Me.DatCanal = New Dataframe.datCanal

                Select Case Mania

                    Case Dataframe.datManias.SqlServer
                        Me.DatCanal.Construtor = New Dataframe.datConstrutorSQL

                End Select

                If Me.LoadDLL() Then

                    If Me.BuildConector() Then

                        Me.DatCanal.Adaptador = New Dataframe.datAdaptadorDB(Me.Conector)
                        Return True

                    End If

                End If

                Return False
            End SyncLock
        End Function


        Public Function LoadDLL() As Boolean
            SyncLock _locker
                Assemblagem = Setframe.AssemblagemLoad(ProviderDLL)

                Return (Not Assemblagem Is Nothing)
            End SyncLock
        End Function

        Public Function BuildConector() As Boolean
            SyncLock _locker
                Me.Conector = CType(Setframe.AssemblagemConstroiTipo(Assemblagem, ProviderModulo, ProviderTipo), System.Data.IDbConnection)

                Return (Not Me.Conector Is Nothing)
            End SyncLock
        End Function

        Public Function Conecta() As Boolean
            SyncLock _locker
                Me.StringDeConexão = "application name=" & ClienteDb.AplicaçãoNome & " (" & Database & ")" & "; data source=" & Datasource & "; database=" & Database & "; uid=" & ClienteDb.DbLogin.Uid & "; pwd=" & ClienteDb.DbLogin.Pwd
                If Setup.Length > 0 Then Me.StringDeConexão = Me.StringDeConexão & "; " & Setup

                Me.Conector.ConnectionString = Me.StringDeConexão

                Try
                    Me.Conector.Open()
                Catch Exp As Exception
                    g10phPDF4.Basframe.ErroLogger.DeuExcepção(Exp, "", True, False, False)
                End Try

                Return (Me.Conector.State = ConnectionState.Open)
            End SyncLock
        End Function

        Public Function Desconecta() As Boolean
            SyncLock _locker
                Me.Conector.Close()

                Return (Me.Conector.State = ConnectionState.Closed)
            End SyncLock
        End Function

    End Class

    '                                                                                                   

    Public Class setAplicação
        Inherits Dataframe.DatletBase

        Public Id As Integer
        Public Prefixo As String
        Public Nome As String
        Public Titulo As String
        Public Desc As String
        Public AppUtil As New setUtilizador
        Public Cenario As Integer

        Public AppUtilCifra As g10phPDF4.Basframe.Cripto.ICRIPTOSISTEMA
        Public DbLoginCifra As g10phPDF4.Basframe.Cripto.ICRIPTOSISTEMA

        Private Shared ReadOnly _locker As New Object()

        Public Sub New()
            SyncLock _locker
                Me.l_info = New Dataframe.datInfo
                Me.l_info.Objecto = New Dataframe.datObjecto("aplicacao", -1)

                Me.l_info.Objecto.MembroDef("uid", System.Data.DbType.String) : Me.l_info.Objecto.MembroSetPonteiro() : Me.l_info.Objecto.MembroSetChave()
                Me.l_info.Objecto.MembroDef("pwd", System.Data.DbType.String)
                Me.l_info.Objecto.MembroDef("cifrado", System.Data.DbType.Boolean)
                Me.l_info.Objecto.MembroDef("cenario", System.Data.DbType.Int32)
                Me.l_info.Objecto.MembroDef("titulo", System.Data.DbType.String)
                Me.l_info.Objecto.MembroDef("desc", System.Data.DbType.String)
                Me.l_info.AtributoDef(Me.l_info.Objecto)
                Me.private_resetpreparados()
            End SyncLock
        End Sub

        Public Sub New(ByVal Canal As Dataframe.datCanal)

            Me.new()
            SyncLock _locker
                Me.l_canal = Canal
            End SyncLock
        End Sub

        Public Sub New(ByVal ConstrutorNulo As Boolean)

        End Sub

        Public Overrides Property datAtributo(ByVal Index As Integer) As Object
            Get
                SyncLock _locker
                    Select Case Index
                        Case 0 : Return AppUtil.Uid
                        Case 1 : Return AppUtil.Pwd
                        Case 2 : Return AppUtil.ComCifra
                        Case 3 : Return Cenario
                        Case 4 : Return Titulo
                        Case 5 : Return Desc
                    End Select
                End SyncLock
            End Get
            Set(ByVal Value As Object)
                SyncLock _locker
                    Select Case Index
                        Case 0 : AppUtil.Uid = CType(Value, String)
                        Case 1 : AppUtil.Pwd = CType(Value, String)
                        Case 2 : AppUtil.ComCifra = g10phPDF4.Basframe.Cast2Boolean(Value)
                        Case 3 : Cenario = CInt(Value)
                        Case 4 : Titulo = CType(Value, String)
                        Case 5 : Desc = CType(Value, String)
                    End Select
                End SyncLock
            End Set
        End Property

        Public Function Config() As Boolean
            SyncLock _locker
                ' acerto dos dados lidos:
                ' alguns dos dados são opcionais pelo que na sua omissão estes devem ser os valores a usar

                If Me.DatConsultaPrimeiro() Then

                    Me.AppUtil.Cifra = Me.AppUtilCifra

                    If Me.AppUtil.ComCifra Then
                        Me.AppUtil.Desencripta()
                    Else
                        Me.AppUtil.Encriptado = False
                    End If

                    If Me.AppUtil.Uid.Length = 0 Then Me.AppUtil = Nothing

                    Return True

                Else

                    Return False

                End If
            End SyncLock
        End Function

        Public Function Config(ByVal FicheiroXML As String) As Boolean
            SyncLock _locker
                Me.DatCanal = New Dataframe.datCanalXML(FicheiroXML)
                Return Me.Config()
            End SyncLock
        End Function

        Public Overrides Function DatCast(ByVal Lista() As Dataframe.ILET) As Dataframe.ILET()

        End Function

        Public Overrides Function DatClone() As Dataframe.ILET

        End Function

        Public Overrides Function DatNew() As Dataframe.ILET

        End Function
    End Class

    Public Class setCicloDeLogin

        Public AppUtil As setUtilizador
        Public Autorizado As Boolean
        Public Abortado As Boolean

        Public Event Dialogando(ByVal Ciclo As setCicloDeLogin)
        Public Event Terminando(ByVal Ciclo As setCicloDeLogin)

        Public Event TentandoLogin(ByVal Ciclo As setCicloDeLogin, ByRef getRecusa As Boolean)
        Public Event AbortandoLogin(ByVal Ciclo As setCicloDeLogin, ByRef getCancelamento As Boolean)

        Public Event LoginAutorizado(ByVal Ciclo As setCicloDeLogin)
        Public Event LoginRecusado(ByVal Ciclo As setCicloDeLogin)
        Public Event LoginAbortado(ByVal Ciclo As setCicloDeLogin)

        Private Shared ReadOnly _locker As New Object()

        Public Overridable Sub DialogoLança()
            SyncLock _locker
                RaiseEvent Dialogando(Me)
            End SyncLock
        End Sub
        Public Overridable Sub DialogoTermina()
            SyncLock _locker
                RaiseEvent Terminando(Me)

                If Abortado Then
                    RaiseEvent LoginAbortado(Me)
                Else
                    If Autorizado Then
                        RaiseEvent LoginAutorizado(Me)
                    Else
                        RaiseEvent LoginRecusado(Me)
                    End If
                End If
            End SyncLock
        End Sub

        Public Overridable Function LoginTenta() As Boolean
            SyncLock _locker
                Dim Recusa As Boolean

                RaiseEvent TentandoLogin(Me, Recusa)

                LoginTenta = Not Recusa
                Autorizado = Not Recusa
                Abortado = False

                If Autorizado Then DialogoTermina() Else RaiseEvent LoginRecusado(Me)
            End SyncLock
        End Function

        Public Overridable Function LoginAborta() As Boolean
            SyncLock _locker
                Dim Cancela As Boolean

                If Not Autorizado Then

                    RaiseEvent AbortandoLogin(Me, Cancela)

                    LoginAborta = Not Cancela
                    Autorizado = False
                    Abortado = Not Cancela

                    If Abortado Then DialogoTermina()

                End If
            End SyncLock
        End Function

    End Class

    Public Class setConfigurador

        Public Servidor As New setClienteDb
        Public Aplicação As New setAplicação
        Public Dicionario As New Dataframe.Estrutura.appDiccionarioEntrada
        Public Appendice() As String

        Public l_canal As Dataframe.datCanal

        Private Shared ReadOnly _locker As New Object()

        Public Sub New()

        End Sub

        Public Sub New(ByVal Canal As Dataframe.datCanal)
            SyncLock _locker
                l_canal = Canal
                Servidor = New setClienteDb(Canal)
                Aplicação = New setAplicação(Canal)
                Dicionario = New Dataframe.Estrutura.appDiccionarioEntrada(Canal)
            End SyncLock
        End Sub

        Public Property DatCanal() As Dataframe.datCanal
            Get
                SyncLock _locker
                    Return l_canal
                End SyncLock
            End Get
            Set(ByVal Value As Dataframe.datCanal)
                SyncLock _locker
                    l_canal = Value
                    Servidor.DatCanal = l_canal
                    Aplicação.DatCanal = l_canal
                    Dicionario.DatCanal = l_canal
                End SyncLock
            End Set
        End Property

        Public Function Config() As Boolean
            SyncLock _locker
                Dim ok As Boolean

                ok = True
                ok = Aplicação.Config()
                Servidor.AplicaçãoNome = Aplicação.Titulo
                Servidor.DbLogin.Cifra = Aplicação.DbLoginCifra
                ok = ok And Servidor.Config()
                Appendice = Dicionario.DatSeleccionaTextosPorDic
                ok = ok And (Not Appendice Is Nothing)

                Return ok
            End SyncLock
        End Function

        Public Function ConfigUpdate() As Boolean
            SyncLock _locker
                ' todo: setConfigurador (datAdaptadorXML): datlets filhas fora da arvore xml
                ' este sistema não força o servidor e a aplicação a ficarem dentro da arvore xml do configurador!
                ' este é um problema do canal xml

                Dim ok As Boolean

                Dim datlet As New Dataframe.DatletIstancia(New Dataframe.datInfo, l_canal)
                datlet.DatInfo.Objecto = New Dataframe.datObjecto("config", 0)
                datlet.DatInfo.Objecto.MembroDef("dataset_" & Servidor.DatInfo.Objecto.Nome, DbType.String) : datlet.DatInfo.Objecto.MembroSetTudo()
                datlet.DatInfo.Objecto.MembroDef("dataset_" & Aplicação.DatInfo.Objecto.Nome, DbType.String)
                datlet.DatInfo.Objecto.MembroDef("dataset_" & Dicionario.DatInfo.Objecto.Nome, DbType.String)
                datlet.DatReset()

                ok = True
                If Not datlet.DatExiste() Then ok = ok And datlet.DatInsere()
                ok = ok And Servidor.DatActualiza
                ok = ok And Aplicação.DatActualiza

                Return ok
            End SyncLock
        End Function

    End Class

End Namespace