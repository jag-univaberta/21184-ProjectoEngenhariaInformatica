Imports System
Imports System.IO

Public Class History
    Private _filename As String
    Private _lasterror As String
    Private _version As String
    Private _valid As Boolean
    Private Shared ReadOnly _locker As New Object()

    Public Sub New(ByVal filename As String, ByVal create As Boolean)
        SyncLock _locker
            _filename = filename
            _lasterror = ""
            _valid = False

            If File.Exists(filename) Then
                _valid = Validate()
            Else
                If create Then
                    Dim sw As StreamWriter
                    sw = File.CreateText(filename)
                    sw.WriteLine("PH-Informática, SA")
                    sw.WriteLine("==================")
                    sw.WriteLine("DateCreated: " & Now.ToLongDateString)
                    sw.WriteLine("LogFile v.1")
                    sw.WriteLine("")
                    sw.WriteLine("")
                    sw.WriteLine("")
                    sw.Close()
                    _valid = True
                Else
                    _lasterror = filename & " não existe e opção CREATE=false"
                End If
            End If
        End SyncLock


    End Sub

    Public Function Validate() As Boolean
        SyncLock _locker
            Dim res As Boolean = False

            Dim sr As StreamReader = File.OpenText(_filename)
            Dim s As String = sr.ReadLine
            s = sr.ReadLine
            s = sr.ReadLine
            s = sr.ReadLine
            If s.Substring(0, 10) = "LogFile v." Then
                _lasterror = ""
                res = True
            Else
                _lasterror = _filename & " não é um ficheiro LOG válido (versão não encontrada)."
            End If
            sr.Close()

            Return res
        End SyncLock


    End Function

    Public Function Version() As String
        SyncLock _locker
            Return _version
        End SyncLock

    End Function

    Public Function IsValid() As Boolean
        SyncLock _locker
            Return _valid
        End SyncLock

    End Function

    Public Function LastError() As String
        SyncLock _locker
            Return _lasterror
        End SyncLock

    End Function

    Public Function AddEvent(ByRef histevent As HistoryEvent) As Boolean
        SyncLock _locker
            Dim res As Boolean = False

            Dim timeout As Long = 1000
            Dim k As Long = 0
            Dim write As Boolean = False

            Dim s As String = ""
            s &= "       Id: " & histevent.Id & vbCrLf
            s &= "ServiceTS: " & Now
            s &= "   Origem: " & histevent.Origem & vbCrLf
            s &= "Timestamp: " & histevent.Timestamp & vbCrLf
            s &= "Descrição: " & histevent.Descricao & vbCrLf
            s &= "    Valor: " & histevent.Valor.ToString & vbCrLf & vbCrLf

            Dim b As Byte() = New System.Text.UTF8Encoding().GetBytes(s)

            Try
                'Dim fs As FileStream
                ''TODO: é necessário testar acesso concorrente 
                'fs = File.Open(_filename, FileMode.Append, FileAccess.Write)
                'fs.Write(b, 0, b.GetUpperBound(0) + 1)
                'fs.Close()
                _lasterror = ""
                res = True

            Catch ex As Exception
                _lasterror = ex.Message
                Throw ex
            End Try

            Return res
        End SyncLock


    End Function

End Class

Public Class HistoryEvent

    Private _id As String
    Private _origem As String
    Private _descricao As String
    Private _valor As Object
    Private _timestamp As DateTime
    Private Shared ReadOnly _locker As New Object()

    Public Sub New()
        SyncLock _locker
            _timestamp = Now
        End SyncLock

    End Sub

    Public Sub New(ByVal id As String, ByVal origem As String, ByVal descricao As String, ByVal valor As Object, ByVal timestamp As DateTime)
        SyncLock _locker
            _id = id
            _origem = origem
            _descricao = descricao
            _valor = valor
            _timestamp = timestamp
        End SyncLock

    End Sub

    Public Property Id() As String
        Get
            SyncLock _locker
                Return _id
            End SyncLock

        End Get
        Set(ByVal value As String)

            SyncLock _locker
                _id = value
            End SyncLock

        End Set
    End Property

    Public Property Origem() As String
        Get
            SyncLock _locker
                Return _origem
            End SyncLock

        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _origem = value
            End SyncLock

        End Set
    End Property

    Public Property Descricao() As String
        Get
            SyncLock _locker

            End SyncLock
            Return _descricao
        End Get
        Set(ByVal value As String)
            SyncLock _locker

            End SyncLock
            _descricao = value
        End Set
    End Property

    Public Property Timestamp() As DateTime
        Get
            SyncLock _locker
                Return _timestamp
            End SyncLock

        End Get
        Set(ByVal value As DateTime)
            SyncLock _locker
                _timestamp = value
            End SyncLock

        End Set
    End Property

    Public Property Valor() As Object
        Get
            SyncLock _locker
                Return _valor
            End SyncLock

        End Get
        Set(ByVal value As Object)

            SyncLock _locker
                _valor = value
            End SyncLock

        End Set
    End Property

End Class

