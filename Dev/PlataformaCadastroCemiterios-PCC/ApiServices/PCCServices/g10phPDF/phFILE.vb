Namespace Basframe.File

    Public Module File

        Private ReadOnly _locker As New Object()


        Public Function ExistePasta(ByVal Caminho As String) As Boolean
            SyncLock _locker
                Return System.IO.Directory.Exists(Caminho)
            End SyncLock
        End Function

        Public Function CriaPasta(ByVal Caminho As String, ByRef getExcep��o As Exception) As Boolean
            SyncLock _locker
                getExcep��o = Nothing

                Dim DI As System.IO.DirectoryInfo

                Try

                    DI = System.IO.Directory.CreateDirectory(Caminho)
                    Return True

                Catch exp As Exception

                    getExcep��o = exp
                    Return False

                End Try
            End SyncLock
        End Function

        Public Function Existe(ByVal FicheiroNome As String) As Boolean
            SyncLock _locker
                Return System.IO.File.Exists(FicheiroNome)
            End SyncLock
        End Function

        Public Function Cria(ByVal FicheiroNome As String, ByRef getExcep��o As Exception) As Boolean
            SyncLock _locker
                Dim Liga��o As System.IO.FileStream

                getExcep��o = Nothing

                Try

                    Liga��o = System.IO.File.Create(FicheiroNome)
                    Liga��o.Close()
                    Return True

                Catch exp As Exception

                    getExcep��o = exp
                    Return False

                End Try
            End SyncLock
        End Function

        Public Function Abre(ByVal FicheiroNome As String, ByVal Modo As System.IO.FileMode, ByRef getPonteiro As System.IO.FileStream, ByRef getExcep��o As Exception) As Boolean
            SyncLock _locker
                getExcep��o = Nothing

                Try

                    getPonteiro = System.IO.File.Open(FicheiroNome, Modo)
                    Return True

                Catch exp As Exception

                    getExcep��o = exp
                    getPonteiro = Nothing
                    Return False

                End Try
            End SyncLock
        End Function

        Public Function Fecha(ByVal Liga��o As System.IO.FileStream, ByRef getExcep��o As Exception) As Boolean
            SyncLock _locker
                getExcep��o = Nothing

                Try

                    Liga��o.Close()
                    Return True

                Catch Exp As System.Exception

                    getExcep��o = Exp
                    Return False

                End Try
            End SyncLock
        End Function



        Public Function Escreve(ByVal Liga��o As System.IO.FileStream, ByVal Linha As String, ByRef getExcep��o As Exception) As Boolean
            SyncLock _locker
                getExcep��o = Nothing

                Try

                    Liga��o.Write(Basframe.Strings.TransStringEmBytes(Linha), 0, Linha.Length)

                    Return True

                Catch Exp As System.Exception

                    getExcep��o = Exp
                    Return False

                End Try
            End SyncLock
        End Function

        Public Function Escreve(ByVal Liga��o As System.IO.FileStream, ByVal Dados() As Byte, ByRef getExcep��o As Exception) As Boolean
            SyncLock _locker
                getExcep��o = Nothing

                Try

                    Liga��o.Write(Dados, 0, Dados.GetUpperBound(0) + 1)
                    Return True

                Catch Exp As System.Exception

                    getExcep��o = Exp
                    Return False

                End Try
            End SyncLock
        End Function

        Public Function Escreve(ByVal FicheiroNome As String, ByVal Linha As String, ByVal Modo As System.IO.FileMode, ByVal CriaFicheiro As Boolean, ByRef getExcep��o As Exception) As Boolean
            SyncLock _locker
                Dim liga��o As System.IO.FileStream

                getExcep��o = Nothing

                If Not Existe(FicheiroNome) Then Cria(FicheiroNome, getExcep��o)

                If Abre(FicheiroNome, Modo, liga��o, getExcep��o) Then

                    If Escreve(liga��o, Linha, getExcep��o) Then
                        Return (Fecha(liga��o, getExcep��o))
                    Else
                        Fecha(liga��o, getExcep��o)
                        Return False
                    End If

                End If
            End SyncLock
        End Function

        Public Function Escreve(ByVal FicheiroNome As String, ByVal Dados() As Byte, ByVal Modo As System.IO.FileMode, ByVal CriaFicheiro As Boolean, ByRef getExcep��o As Exception) As Boolean
            SyncLock _locker
                Dim liga��o As System.IO.FileStream

                getExcep��o = Nothing

                If Not Existe(FicheiroNome) Then Cria(FicheiroNome, getExcep��o)

                If Abre(FicheiroNome, Modo, liga��o, getExcep��o) Then

                    If Escreve(liga��o, Dados, getExcep��o) Then
                        Return (Fecha(liga��o, getExcep��o))
                    Else
                        Fecha(liga��o, getExcep��o)
                        Return False
                    End If

                End If
            End SyncLock
        End Function

        Public Function Le(ByVal Liga��o As System.IO.FileStream, ByRef getExcep��o As Exception) As String
            SyncLock _locker
                Dim Conteudo() As Byte

                ReDim Conteudo(CInt(Liga��o.Length))

                getExcep��o = Nothing

                Try
                    ReDim Conteudo(CInt(Liga��o.Length) - 1)
                    Liga��o.Read(Conteudo, 0, CInt(Liga��o.Length) - 1)
                    Return Basframe.Strings.TransBytesEmString(Conteudo)

                Catch Exp As System.Exception

                    getExcep��o = Exp
                    Return Nothing

                End Try
            End SyncLock
        End Function

        Public Function Le(ByVal FicheiroNome As String, ByRef getExcep��o As Exception) As String
            SyncLock _locker
                Dim liga��o As System.IO.FileStream
                Dim st As String

                getExcep��o = Nothing

                If Abre(FicheiroNome, IO.FileMode.Open, liga��o, getExcep��o) Then

                    st = Le(liga��o, getExcep��o)
                    If Fecha(liga��o, getExcep��o) Then
                        Return st
                    Else
                        Return Nothing
                    End If

                Else

                    Return Nothing

                End If
            End SyncLock
        End Function



        Public Function GetPastaAssemblyBase() As String
            SyncLock _locker
                Return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly.Location)
            End SyncLock
        End Function

        Public Function GetPastaAssemblyActual() As String
            SyncLock _locker
                Return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)
            End SyncLock
        End Function

        Public Function GetPastaFrameworkNet() As String
            SyncLock _locker
                Return Environment.GetEnvironmentVariable("windir") & "\microsoft.net\framework\v" & Environment.Version.Major.ToString & "." & Environment.Version.Minor.ToString & "." & Environment.Version.Build.ToString
            End SyncLock
        End Function

        Public Function GetPastaDesktop() As String
            SyncLock _locker
                Return Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            End SyncLock
        End Function

        Public Function GetNomeAssemblyBase() As String
            SyncLock _locker
                Return System.Reflection.Assembly.GetEntryAssembly.GetName.Name
            End SyncLock
        End Function

        Public Function GetNomeAssemblyActual() As String
            SyncLock _locker
                Return System.Reflection.Assembly.GetExecutingAssembly.GetName.Name
            End SyncLock
        End Function

        Public Function GetNomeAssemblyActualPai() As String
            SyncLock _locker
                Return System.Reflection.Assembly.GetExecutingAssembly.GetCallingAssembly.GetName.Name
            End SyncLock
        End Function


        Public Function PathFile(ByVal caminho As String) As String
            SyncLock _locker
                ' retorna apenas o nome do ficheiro com extens�o

                Dim N1 As Integer

                N1 = caminho.LastIndexOf("\")

                If N1 = -1 Or N1 = caminho.Length Then
                    Return ""
                Else
                    Return caminho.Substring(N1 + 1)
                End If
            End SyncLock
        End Function

    End Module

    'Public Class Ficheiro
    '    Inherits System.IO.FileStream

    '    Public Sub New(ByVal Caminho As String, ByVal Modo As System.IO.FileMode)

    '        MyBase.New(Caminho, Modo)

    '    End Sub

    '    Public ReadOnly Property Text() As String
    '        Get
    '            Dim bytes() As Byte
    '            Me.Read(bytes, 0, CInt(Me.Length))
    '            Return Strings.TransBytesEmString(bytes)
    '        End Get
    '    End Property

    'End Class

End Namespace