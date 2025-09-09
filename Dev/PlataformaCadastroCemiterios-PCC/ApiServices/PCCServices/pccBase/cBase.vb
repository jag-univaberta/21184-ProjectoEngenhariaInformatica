Imports System
Imports System.IO
Imports System.Reflection
Imports System.Data
Imports System.Runtime.Serialization
Imports System.Xml.Serialization
Imports pccDB4


Public Class ListItem
    Private _name As String
    Private _key As String
    Private _object As Object
    Private Shared ReadOnly _locker As New Object()

    Public Sub New(ByVal p_name As String, ByVal p_key As String, ByRef p_object As Object)
        SyncLock _locker
            _name = p_name
            _key = p_key
            _object = p_object
        End SyncLock
    End Sub
    Public Function Key() As String
        SyncLock _locker
            Return _key
        End SyncLock
    End Function

    Public Function Name() As String
        SyncLock _locker
            Return _name
        End SyncLock
    End Function

    Public Overrides Function ToString() As String
        SyncLock _locker
            Return _name
        End SyncLock
    End Function

    Public Function [Object]() As Object
        SyncLock _locker
            Return _object
        End SyncLock 
    End Function

End Class
<DataContract()> _
<Serializable()> _
Public MustInherit Class Entidade
    <DataMember(Name:="IsNew")> _
    Private _isnew As Boolean
    Private _lasterror As String
    <DataMember(Name:="Id")> _
    Private _id As String
    <DataMember(Name:="Nome")> _
    Private _nome As String

    Private Shared ReadOnly _locker As New Object()

    Public Sub New()
        SyncLock _locker
            _id = Guid.NewGuid().ToString
            _nome = ""
            IsNew = True
        End SyncLock
    End Sub

    Public Sub New(ByVal id As String, ByVal nome As String)
        SyncLock _locker
            If id = "" Then id = Guid.NewGuid().ToString
            _id = id
            _nome = nome
            IsNew = True
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
                IsNew = True
            End SyncLock
        End Set
    End Property

    Public Property IsNew() As Boolean
        Get
            SyncLock _locker
                Return _isnew
            End SyncLock
        End Get
        Set(ByVal value As Boolean)
            SyncLock _locker
                _isnew = value
            End SyncLock
        End Set
    End Property

    Public Property Nome() As String
        Get
            SyncLock _locker
                Return _nome
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _nome = value
            End SyncLock
        End Set
    End Property

    Public Shadows Function ToString() As String
        SyncLock _locker
            Return Nome
        End SyncLock
    End Function

    Public Property LastError() As String
        Set(ByVal value As String)
            SyncLock _locker
                _lasterror = value
            End SyncLock
        End Set
        Get
            SyncLock _locker
                Return _lasterror
            End SyncLock
        End Get
    End Property

    Public MustOverride Function Save(ByRef conn As pccDB4.Connection, ByVal trans As IDbTransaction) As Boolean
    Public MustOverride Function Delete(ByRef conn As pccDB4.Connection, ByVal trans As IDbTransaction) As Boolean
    Public MustOverride Function GetById(ByRef conn As pccDB4.Connection, ByVal p_id As String) As Boolean

End Class
<DataContract(), KnownType(GetType(pccGeoMultiPoint)), KnownType(GetType(pccGeoPoint))>
<KnownType(GetType(pccGeoMultiLineString)), KnownType(GetType(pccGeoLineString))>
<KnownType(GetType(pccGeoMultiPolygon)), KnownType(GetType(pccGeoPolygon))>
<KnownType(GetType(pccGeoCollection)), KnownType(GetType(pccGeoRectangle))>
<Serializable()>
Public MustInherit Class EntidadeGeo
    Inherits Entidade

    Protected geoUtil As New pccGeoUtils
    <DataMember(Name:="Geometry")>
    Private _geom As List(Of pccGeoGeometry)

    Public MustOverride Property GetStorageTable() As String
    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

        MyBase.New()
        SyncLock _locker
            _geom = New List(Of pccGeoGeometry)
        End SyncLock
    End Sub

    Public Sub New(ByVal id As String, ByVal nome As String)

        MyBase.New(IIf(id = "", Guid.NewGuid().ToString, id), nome)
        SyncLock _locker
            _geom = New List(Of pccGeoGeometry)
        End SyncLock

    End Sub
    <DataMember()>
    Public Property Size() As Integer
        Get
            SyncLock _locker
                If _geom Is Nothing Then
                    Return 0
                End If
                Return _geom.Count
            End SyncLock
        End Get
        Set(ByVal value As Integer)

        End Set
    End Property

    <XmlIgnore(), SoapIgnore()>
    Public Property Geometry(Optional ByVal idx As Integer = 0) As pccGeoGeometry
        Get
            SyncLock _locker
                Return _geom(idx)
            End SyncLock
        End Get
        Set(ByVal value As pccGeoGeometry)
            SyncLock _locker
                If (_geom.Count <= idx) Then
                    _geom.Add(value)
                Else
                    _geom(idx) = value
                End If
            End SyncLock
        End Set
    End Property
    <XmlIgnore(), SoapIgnore()>
    Public ReadOnly Property WKT(Optional ByVal idx As Integer = 0) As String
        Get
            SyncLock _locker
                'If _geom IsNot Nothing Then
                '    Return ""
                'Else
                If _geom IsNot Nothing And _geom.Count > 0 Then
                    Return CType(_geom(idx), pccGeoGeometry).WKT
                Else
                    Return ""
                End If
                'End If
            End SyncLock
        End Get

    End Property

    Public Function SaveGeometry(ByRef conn As pccDB4.Connection, ByVal trans As IDbTransaction, ByVal isUpdate As Boolean, ByVal idx As Integer) As Boolean
        SyncLock _locker
            Try

                Dim geos As pccGeoGeometry = Me.Geometry(idx)

                Dim com As pccDB4.Command = conn.CreateCommand

                Dim houveerro As Boolean = False

                'For Each g As pccGeoGeometry In geos
                If Not houveerro Then
                    houveerro = Not geos.Save(conn, trans, Me.Id, Me.GetStorageTable, isUpdate)
                End If
                'Next

                Return Not houveerro

            Catch ex As Exception

                Return False

            End Try
        End SyncLock

    End Function

    Public Function DeleteGeometry(ByRef conn As pccDB4.Connection, ByVal trans As IDbTransaction, ByVal idx As Integer) As Boolean
        SyncLock _locker
            Try
                'TODO: Ver este procedimento devido às alterações nas tabelas...

                Dim geos As pccGeoGeometry = Me.Geometry(idx)

                Dim com As pccDB4.Command = conn.CreateCommand

                Dim houveerro As Boolean = False

                'For Each g As pccGeoGeometry In geos
                If Not houveerro Then
                    houveerro = Not geos.Delete(conn, trans, Me.Id, GetStorageTable)
                End If
                'Next

                Return Not houveerro

            Catch ex As Exception

                Return False

            End Try
        End SyncLock

    End Function

End Class
<DataContract(), KnownType(GetType(EntidadeGeo))>
<Serializable()>
Public Class Entidades
    <DataMember(Name:="Entidade")>
    Private _entidades() As EntidadeGeo
    Private _kentidades As Integer
    Private Shared ReadOnly _locker As New Object()

    Public Sub New()
        SyncLock _locker
            _kentidades = 0
        End SyncLock
    End Sub

    Public Sub Add(ByRef ent As EntidadeGeo)
        SyncLock _locker
            _kentidades += 1
            If _kentidades = 1 Then
                ReDim _entidades(_kentidades - 1)
            Else
                ReDim Preserve _entidades(_kentidades - 1)
            End If
            _entidades(_kentidades - 1) = ent
        End SyncLock
    End Sub

    Public Property Entidades() As EntidadeGeo()
        Get
            SyncLock _locker
                Return _entidades
            End SyncLock
        End Get
        Set(ByVal value As EntidadeGeo())
            SyncLock _locker
                _entidades = value
            End SyncLock
        End Set
    End Property

End Class

Public MustInherit Class EntidadeGeov0
    Inherits Entidade

    Protected geoUtil As New pccGeoUtils
    Private _geom As pccGeov0Geometry
    Private Shared ReadOnly _locker As New Object()

    Public MustOverride Property GetStorageTable() As String

    Public Sub New()

        MyBase.New()
        SyncLock _locker
            _geom = Nothing
        End SyncLock
    End Sub

    Public Sub New(ByVal id As String, ByVal nome As String)

        MyBase.New(IIf(id = "", Guid.NewGuid().ToString, id), nome)
        SyncLock _locker
            _geom = Nothing
        End SyncLock
    End Sub

    <XmlIgnore()>
    Public Property Geometry() As pccGeov0Geometry
        Get
            SyncLock _locker
                Return _geom
            End SyncLock
        End Get
        Set(ByVal value As pccGeov0Geometry)
            SyncLock _locker
                _geom = value
            End SyncLock
        End Set
    End Property

    Public ReadOnly Property WKT() As String
        Get
            SyncLock _locker
                'If _geom IsNot Nothing Then
                '    Return ""
                'Else
                If _geom IsNot Nothing Then
                    Return CType(_geom, pccGeov0Geometry).WKT
                Else
                    Return ""
                End If
                'End If
            End SyncLock
        End Get

    End Property

    Public Function SaveGeometry(ByRef conn As pccDB4.Connection, ByVal trans As IDbTransaction) As Boolean
        SyncLock _locker
            Try

                Dim geos() As pccGeov0Geometry = Me.Geometry.GetBasicGeometries()

                Dim com As pccDB4.Command = conn.CreateCommand

                Dim houveerro As Boolean = False

                For Each g As pccGeov0Geometry In geos
                    If Not houveerro Then
                        houveerro = Not g.Save(conn, trans, Me.Id, Me.GetStorageTable)
                    End If
                Next

                Return Not houveerro

            Catch ex As Exception

                Return False

            End Try
        End SyncLock

    End Function

    Public Function DeleteGeometry(ByRef conn As pccDB4.Connection, ByVal trans As IDbTransaction) As Boolean
        SyncLock _locker
            Try
                'TODO: Ver este procedimento devido às alterações nas tabelas...

                Dim geos() As pccGeov0Geometry = Me.Geometry.GetBasicGeometries()

                Dim com As pccDB4.Command = conn.CreateCommand

                Dim houveerro As Boolean = False

                For Each g As pccGeov0Geometry In geos
                    If Not houveerro Then
                        houveerro = Not g.Delete(conn, trans, Me.Id, GetStorageTable)
                    End If
                Next

                Return Not houveerro

            Catch ex As Exception

                Return False

            End Try
        End SyncLock

    End Function

End Class

Public Class Company

    Private _isnew As Boolean
    Private _lasterror As String

    Private _rec_id As String
    Private _ad_login As Boolean
    Private _ad_domain_id As String
    Private _email_host_id As String
    Private _email_user_id As String
    Private _email_user_pwd As String
    Private _email_from_id As String
    Private _email_host_ssl As Boolean
    Private _email_host_port As Integer
    Private _pwd_login_length As Integer
    Private _pwd_recup_length As Integer
    Private Shared ReadOnly _locker As New Object()

    Public Sub New()
        SyncLock _locker
            _rec_id = Guid.NewGuid().ToString

            _ad_login = False
            _ad_domain_id = ""
            _email_host_id = ""
            _email_user_id = ""
            _email_user_pwd = ""
            _email_from_id = ""
            _email_host_ssl = False
            _email_host_port = 25
            _pwd_login_length = 4
            _pwd_recup_length = 10

            IsNew = True
        End SyncLock
    End Sub

    Public Sub New(ByVal rec_id As String, ByVal ad_login As Boolean, ByVal ad_domain_id As String,
                   ByVal email_host_id As String, ByVal email_user_id As String, ByVal email_user_pwd As String,
                   ByVal email_from_id As String, ByVal email_host_ssl As Boolean, ByVal email_host_port As Integer,
                   ByVal pwd_login_length As Integer, ByVal pwd_recup_length As Integer)
        SyncLock _locker
            If rec_id = "" Then rec_id = Guid.NewGuid().ToString

            _rec_id = rec_id
            _ad_login = ad_login
            _ad_domain_id = ad_domain_id
            _email_host_id = email_host_id
            _email_user_id = email_user_id
            _email_user_pwd = email_user_pwd
            _email_from_id = email_from_id
            _email_host_ssl = email_host_ssl
            _email_host_port = email_host_port
            _pwd_login_length = pwd_login_length
            _pwd_recup_length = pwd_recup_length

            IsNew = True
        End SyncLock

    End Sub

    Public Property IsNew() As Boolean
        Get
            SyncLock _locker
                Return _isnew
            End SyncLock

        End Get
        Set(ByVal value As Boolean)
            SyncLock _locker
                _isnew = value
            End SyncLock
        End Set
    End Property

    Public Property LastError() As String
        Set(ByVal value As String)
            SyncLock _locker
                _lasterror = value
            End SyncLock

        End Set
        Get
            SyncLock _locker
                Return _lasterror
            End SyncLock

        End Get
    End Property

    Public Property Rec_Id() As String
        Get
            SyncLock _locker
                Return _rec_id
            End SyncLock

        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _rec_id = value
                IsNew = True
            End SyncLock

        End Set
    End Property

    Public Property Ad_Login() As Boolean
        Get
            SyncLock _locker
                Return _ad_login
            End SyncLock
        End Get
        Set(ByVal value As Boolean)
            SyncLock _locker
                _ad_login = value
                IsNew = True
            End SyncLock
        End Set
    End Property

    Public Property Ad_Domain_Id() As String
        Get
            SyncLock _locker
                Return _ad_domain_id
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _ad_domain_id = value
                IsNew = True
            End SyncLock
        End Set
    End Property

    Public Property Email_Host_Id() As String
        Get
            SyncLock _locker
                Return _email_host_id
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _email_host_id = value
                IsNew = True
            End SyncLock
        End Set
    End Property

    Public Property Email_User_Id() As String
        Get
            SyncLock _locker
                Return _email_user_id
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _email_user_id = value
                IsNew = True
            End SyncLock
        End Set
    End Property

    Public Property Email_User_Pwd() As String
        Get
            SyncLock _locker
                Return _email_user_pwd
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _email_user_pwd = value
                IsNew = True
            End SyncLock
        End Set
    End Property

    Public Property Email_From_Id() As String
        Get
            SyncLock _locker
                Return _email_from_id
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _email_from_id = value
                IsNew = True
            End SyncLock
        End Set
    End Property

    Public Property Email_Host_Ssl() As Boolean
        Get
            SyncLock _locker
                Return _email_host_ssl
            End SyncLock
        End Get
        Set(ByVal value As Boolean)
            SyncLock _locker
                _email_host_ssl = value
                IsNew = True
            End SyncLock
        End Set
    End Property

    Public Property Email_Host_Port() As Integer
        Get
            SyncLock _locker
                Return _email_host_port
            End SyncLock
        End Get
        Set(ByVal value As Integer)
            SyncLock _locker
                _email_host_port = value
                IsNew = True
            End SyncLock
        End Set
    End Property

    Public Property Pwd_Login_Length() As Integer
        Get
            SyncLock _locker
                Return _pwd_login_length
            End SyncLock
        End Get
        Set(ByVal value As Integer)
            SyncLock _locker
                _pwd_login_length = value
                IsNew = True
            End SyncLock
        End Set
    End Property

    Public Property Pwd_Recup_Length() As Integer
        Get
            SyncLock _locker
                Return _pwd_recup_length
            End SyncLock
        End Get
        Set(ByVal value As Integer)
            SyncLock _locker
                _pwd_recup_length = value
                IsNew = True
            End SyncLock
        End Set
    End Property

    Public Function Save(ByRef conn As pccDB4.Connection, ByVal trans As System.Data.IDbTransaction) As Boolean
        SyncLock _locker
            Dim com As pccDB4.Command = Nothing
            Dim par As IDbDataParameter = Nothing
            Dim res As Boolean = False

            Try

                com = conn.CreateCommand

                If trans IsNot Nothing Then com.Transaction = trans

                If Me.IsNew Then
                    com.CommandText = "insert into appconfig (ad_login, ad_domain_id, email_host_id, email_user_id, email_user_pwd, email_from_id, email_host_ssl, email_host_port, pwd_login_length, pwd_recup_length, rec_id) "
                    com.CommandText &= "values (?,?,?,?,?,?,?,?,?,?,?)"
                Else
                    Dim s As String = ""

                    If Me.Email_User_Pwd.Length > 0 Then
                        s = "update utilizadores set ad_login=?, ad_domain_id=?, email_host_id=?, email_user_id=?, email_user_pwd=?, email_from_id=?, email_host_ssl=?, email_host_port=?, pwd_login_length=?, pwd_recup_length=? where rec_id=?"
                    Else
                        s = "update utilizadores set ad_login=?, ad_domain_id=?, email_host_id=?, email_user_id=?, email_from_id=?, email_host_ssl=?, email_host_port=?, pwd_login_length=?, pwd_recup_length=? where rec_id=?"
                    End If

                    com.CommandText = s

                End If

                com.CommandType = CommandType.Text

                par = com.CreateParameter(DbType.String, Me.Ad_Login)
                com.AddParameter(par)

                par = com.CreateParameter(DbType.String, Me.Ad_Domain_Id)
                com.AddParameter(par)

                par = com.CreateParameter(DbType.Boolean, Me.Email_Host_Id)
                com.AddParameter(par)

                par = com.CreateParameter(DbType.String, Me.Email_User_Id)
                com.AddParameter(par)

                If Me.Email_User_Pwd.Length > 0 Then
                    par = com.CreateParameter(DbType.Int16, Me.Email_User_Pwd)
                    com.AddParameter(par)
                End If

                par = com.CreateParameter(DbType.Boolean, Me.Email_From_Id)
                com.AddParameter(par)

                par = com.CreateParameter(DbType.String, Me.Email_Host_Ssl)
                com.AddParameter(par)

                par = com.CreateParameter(DbType.String, Me.Email_Host_Port)
                com.AddParameter(par)

                par = com.CreateParameter(DbType.String, Me.Pwd_Login_Length)
                com.AddParameter(par)

                par = com.CreateParameter(DbType.String, Me.Pwd_Recup_Length)
                com.AddParameter(par)

                'par = com.CreateParameter(DbType.String, Me.Rec_Id)
                par = com.CreateParameter(DbType.String, "11111111-1111-1111-1111-111111111111")
                com.AddParameter(par)

                com.ExecuteNonQuery()

                Me.IsNew = False
                res = True

            Catch ex As Exception


                res = False

            End Try

            Return res
        End SyncLock
    End Function

    Public Function GetByRecord(ByRef conn As pccDB4.Connection, ByVal p_id As String) As Boolean
        SyncLock _locker
            Dim com As pccDB4.Command = Nothing
            Dim par As IDbDataParameter = Nothing
            Dim iReader As IDataReader = Nothing
            Dim ret As Boolean = False

            Try

                com = conn.CreateCommand

                com.CommandText = "select " + com.GetFunction(pccDB4.ProviderFunctions.Convert, "rec_id", "p", "char(36)") & ", p.ad_login, p.ad_domain_id, p.email_host_id, p.email_user_id, p.email_user_pwd, p.email_from_id, p.email_host_ssl, p.email_host_port, p.pwd_login_length, p.pwd_recup_length from appconfig p where p.rec_id=?"

                com.CommandType = CommandType.Text

                'par = com.CreateParameter(DbType.String, p_id)
                par = com.CreateParameter(DbType.String, "11111111-1111-1111-1111-111111111111")
                com.AddParameter(par)

                iReader = com.ExecuteReader()

                If iReader.Read Then

                    Me.Rec_Id = iReader.GetString(0)

                    If Not iReader.IsDBNull(1) Then
                        Me.Ad_Login = iReader.GetBoolean(1)
                    Else
                        Me.Ad_Login = False
                    End If

                    If Not iReader.IsDBNull(2) Then
                        Me.Ad_Domain_Id = iReader.GetString(2)
                    Else
                        Me.Ad_Domain_Id = ""
                    End If

                    If Not iReader.IsDBNull(3) Then
                        Me.Email_Host_Id = iReader.GetString(3)
                    Else
                        Me.Email_Host_Id = ""
                    End If

                    If Not iReader.IsDBNull(4) Then
                        Me.Email_User_Id = iReader.GetString(4)
                    Else
                        Me.Email_User_Id = ""
                    End If

                    If Not iReader.IsDBNull(5) Then
                        Me.Email_User_Pwd = iReader.GetString(5)
                    Else
                        Me.Email_User_Pwd = ""
                    End If

                    If Not iReader.IsDBNull(6) Then
                        Me.Email_From_Id = iReader.GetString(6)
                    Else
                        Me.Email_From_Id = ""
                    End If

                    If Not iReader.IsDBNull(7) Then
                        Me.Email_Host_Ssl = iReader.GetBoolean(7)
                    Else
                        Me.Email_Host_Ssl = False
                    End If

                    If Not iReader.IsDBNull(8) Then
                        Me.Email_Host_Port = iReader.GetInt32(8)
                    Else
                        Me.Email_Host_Port = 25
                    End If

                    If Not iReader.IsDBNull(9) Then
                        Me.Pwd_Login_Length = iReader.GetInt32(9)
                    Else
                        Me.Pwd_Login_Length = 4
                    End If

                    If Not iReader.IsDBNull(10) Then
                        Me.Pwd_Recup_Length = iReader.GetInt32(10)
                    Else
                        Me.Pwd_Recup_Length = 10
                    End If

                    Me.IsNew = False
                    ret = True

                Else

                    ret = False

                End If

                If iReader IsNot Nothing Then
                    If Not iReader.IsClosed Then iReader.Close()
                End If

            Catch ex As Exception

                If iReader IsNot Nothing Then
                    If Not iReader.IsClosed Then iReader.Close()
                End If
                ret = False

            Finally

                If iReader IsNot Nothing Then
                    If Not iReader.IsClosed Then iReader.Close()
                End If

            End Try

            Return ret
        End SyncLock
    End Function

    Public Function Delete(ByRef conn As pccDB4.Connection, ByVal trans As System.Data.IDbTransaction) As Boolean
        SyncLock _locker
            Dim com As pccDB4.Command = Nothing
            Dim par As IDbDataParameter = Nothing
            Dim res As Boolean = False

            Try

                com = conn.CreateCommand
                If trans IsNot Nothing Then com.Transaction = trans

                com.CommandText = "delete from appconfig where rec_id=?"

                com.CommandType = CommandType.Text

                'par = com.CreateParameter(DbType.String, Me.Rec_Id)
                par = com.CreateParameter(DbType.String, "11111111-1111-1111-1111-111111111111")
                com.AddParameter(par)

                com.ExecuteNonQuery()

                IsNew = True
                res = True

            Catch ex As Exception

                res = False

            End Try

            Return res
        End SyncLock
    End Function

End Class