Imports System
Imports System.Reflection
Imports System.Data
Imports System.Runtime.Serialization
Imports System.Xml.Serialization

<Serializable()> _
Public Enum DatabaseProvider

    SQLServer = 0
    Oracle = 1
    mySQL = 3
    Postgre = 4

End Enum

Public Enum ProviderFunctions

    GeoToWKT = 10
    WKTToGeo = 11
    WKTToGeoPrefix = 12
    WKTToGeoSufix = 13
    Length = 14
    Area = 15
    Centroid = 16
    MBR = 17
    GeometryFieldName = 18
    SRid = 19
    Max = 20
    MBRasText = 21

   

    IsValid = 25
    MakeValid = 26

    GetX = 30
    GetY = 31
     
    Intersection = 40

    DateTimeToString = 50
    StringToDateTime = 51

    NULLGeom = 90
    NULLGeomTest = 91

    Convert = 100
    Substring = 101
    LengthString = 102

    IntersectsPrefix = 150
    IntersectsSufix = 151
    GetBuffer = 152 

    BeginTransaction = 200
    RollBackTransaction = 201
    CommitTransaction = 202

    Simplify = 300

    CaseAccentInsensitiveComp = 301


End Enum

<Serializable()> _
Public Class Command
    Implements IDbCommand

    Private _comm As IDbCommand
    Private _provider As DatabaseProvider
    Private Shared ReadOnly _locker As New Object()

    Public Sub New(ByRef command As IDbCommand, ByVal provider As DatabaseProvider)
        SyncLock _locker
            _comm = command
            _provider = provider
        End SyncLock
    End Sub

    Public Property Provider() As DatabaseProvider
        Get
            SyncLock _locker
                Return _provider
            End SyncLock
        End Get
        Set(ByVal value As DatabaseProvider)
            SyncLock _locker
                _provider = value
            End SyncLock

        End Set
    End Property

    Public Function GetFunction(ByVal func As ProviderFunctions) As String
        SyncLock _locker
            Return GetFunction(func, "", "", "null")
        End SyncLock
    End Function

    Public Function GetFunction(ByVal func As ProviderFunctions, ByVal field As String) As String
        SyncLock _locker
            Return GetFunction(func, field, "", "null")
        End SyncLock

    End Function

    Public Function GetFunction(ByVal func As ProviderFunctions, ByVal field As String, ByVal tableAlias As String) As String
        SyncLock _locker
            Return GetFunction(func, field, tableAlias, "null")
        End SyncLock

    End Function

    Public Function GetFunction(ByVal func As ProviderFunctions, ByVal field As String, ByVal tableAlias As String, ByVal sParam As String) As String
        SyncLock _locker
            If tableAlias <> "" Then tableAlias &= "."

            Select Case func
                Case ProviderFunctions.CaseAccentInsensitiveComp
                    Select Case _provider
                        Case DatabaseProvider.mySQL
                        Case DatabaseProvider.Oracle
                        Case DatabaseProvider.Postgre
                            Return "translate(lower(" & tableAlias & field & "),'çâãäåāáàăąèééêëēĕėęěìíîïìĩīĭóôõöōŏőùúûüũūŭů','caaaaaaaaaeeeeeeeeeeiiiiiiiiooooooouuuuuuu') ilike translate(lower('%" + sParam + "%'),'çâãäåāáàăąèééêëēĕėęěìíîïìĩīĭóôõöōŏőùúûüũūŭů','caaaaaaaaaeeeeeeeeeeiiiiiiiiooooooouuuuuuu')"
                        Case DatabaseProvider.SQLServer
                    End Select

                Case ProviderFunctions.GeometryFieldName
                    Select Case _provider
                        Case DatabaseProvider.Oracle
                            Return tableAlias & field & "_wkt"
                        Case Else
                            Return tableAlias & field
                    End Select

                    ' Returns the spatial reference identifier 
                Case ProviderFunctions.SRid
                    Select Case _provider
                        Case DatabaseProvider.SQLServer
                            Return tableAlias & field & ".STSrid"
                        Case DatabaseProvider.mySQL
                            Return "SRID(" & tableAlias & field & ")"
                        Case DatabaseProvider.Oracle
                            Return "sde.st_srid(" & tableAlias & field & ")"
                        Case DatabaseProvider.Postgre
                            Return "st_srid(" & tableAlias & field & ")"
                        Case Else
                            Return tableAlias & field
                    End Select
                    ' Substring
                Case ProviderFunctions.Substring
                    Select Case _provider
                        Case DatabaseProvider.SQLServer
                            Return "Substring(" & tableAlias & field & "," & sParam & ")"
                        Case DatabaseProvider.mySQL
                            Return "Substring(" & tableAlias & field & "," & sParam & ")"
                        Case DatabaseProvider.Oracle
                            Return "Substring(" & tableAlias & field & "," & sParam & ")"
                        Case DatabaseProvider.Postgre
                            Return "Substr(" & tableAlias & field & "," & sParam & ")"
                        Case Else
                            Return tableAlias & field
                    End Select
                Case ProviderFunctions.LengthString
                    Select Case _provider
                        Case DatabaseProvider.SQLServer
                            Return "Len(" & tableAlias & field & ")"
                        Case DatabaseProvider.mySQL
                            Return "CHAR_LENGTH(" & tableAlias & field & ")"
                        Case DatabaseProvider.Oracle
                            Return "LENGTH(" & tableAlias & field & ")"
                        Case DatabaseProvider.Postgre
                            Return "length(" & tableAlias & field & ")"
                        Case Else
                            Return tableAlias & field
                    End Select

                    ' Simply
                Case ProviderFunctions.Simplify
                    Select Case _provider
                        Case DatabaseProvider.SQLServer
                            Return tableAlias & field & ".Reduce(" & sParam & ")"
                        Case DatabaseProvider.mySQL     ' TODO: Simplify MySQL
                            Return tableAlias & field
                        Case DatabaseProvider.Oracle    ' TODO: Simplify Oracle
                            Return tableAlias & field
                        Case DatabaseProvider.Postgre
                            Return "simplify (" & tableAlias & field & "," & sParam & ")"
                        Case Else
                            Return tableAlias & field
                    End Select

                    ' convert
                Case ProviderFunctions.Convert
                    Select Case _provider
                        Case DatabaseProvider.SQLServer
                            Return "CONVERT(" & sParam & "," & tableAlias & field & ")"
                        Case DatabaseProvider.mySQL
                            Return "CAST(" & tableAlias & field & " AS " & sParam & ")"
                        Case DatabaseProvider.Oracle
                            Return "CAST(" & tableAlias & field & " AS " & sParam & ")"
                        Case DatabaseProvider.Postgre
                            Return "CAST(" & tableAlias & field & " AS " & sParam & ")"
                        Case Else
                            Return tableAlias & field
                    End Select

                    ' get MBR
                Case ProviderFunctions.MBR
                    Select Case _provider
                        Case DatabaseProvider.SQLServer
                            Return tableAlias & field & ".STEnvelope()"
                        Case DatabaseProvider.mySQL
                            Return "Envelope(" & tableAlias & field & ")"
                        Case DatabaseProvider.Oracle
                            Return "SDO_GEOM.SDO_MBR(" & tableAlias & field & ")"
                        Case DatabaseProvider.Postgre
                            Return "ST_Envelope(" & tableAlias & field & ")"
                        Case Else
                            Return tableAlias & field
                    End Select
                Case ProviderFunctions.MBRasText
                    Select Case _provider
                        Case DatabaseProvider.SQLServer
                            Return tableAlias & field & ".STEnvelope().STAsText()"
                        Case DatabaseProvider.mySQL
                            Return "AsText(Envelope(" & tableAlias & field & "))"
                        Case DatabaseProvider.Oracle
                            Return "SDO_GEOM.SDO_MBR(" & tableAlias & field & ").get_wkt()"
                        Case DatabaseProvider.Postgre
                            Return "ST_AsText(ST_Envelope(" & tableAlias & field & "))"
                        Case Else
                            Return tableAlias & field
                    End Select

                    ' get Centroid 
                Case ProviderFunctions.Centroid
                    Select Case _provider
                        Case DatabaseProvider.SQLServer
                            ' Nova sintax para SQLServer pois a função STCentroid só calcula o centroide para poligonos e multipoligonos
                            Return "case when " & tableAlias & field & ".STGeometryType() = 'LineString' or " & tableAlias & field & ".STGeometryType() = 'Point' then " & tableAlias & field & ".STPointN(1) else " & tableAlias & field & ".STCentroid() end"
                            ' TODO: Falta implementar para os outros providers (?) Testar...
                            'Return tableAlias & field & ".STCentroid()"
                        Case DatabaseProvider.mySQL
                            Return "Centroid(" & tableAlias & field & ")"
                        Case DatabaseProvider.Oracle
                            Return "SDO_GEOM.SDO_CENTROID(" & tableAlias & field & "," & sParam & ")"
                        Case DatabaseProvider.Postgre
                            Return "ST_CENTROID(" & tableAlias & field & ")"
                        Case Else
                            Return tableAlias & field
                    End Select

                    ' get WellKnownText From GEO
                Case ProviderFunctions.GeoToWKT
                    Select Case _provider
                        Case DatabaseProvider.SQLServer
                            Return tableAlias & field & ".STAsText()"
                        Case DatabaseProvider.mySQL
                            Return "AsText(" & tableAlias & field & ")"
                        Case DatabaseProvider.Oracle
                            Return tableAlias & field & ".get_wkt()"
                        Case DatabaseProvider.Postgre
                            ' ATT: WKT format does not maintain precision so to prevent floating truncation, use ST_AsBinary or ST_AsEWKB format for
                            'transport.
                            Return ("ST_AsText(" & tableAlias & field & ")")
                        Case Else
                            Return tableAlias & field
                    End Select

                    ' get X Coord From GEO
                Case ProviderFunctions.GetX
                    Select Case _provider
                        Case DatabaseProvider.SQLServer
                            Return tableAlias & field & ".STX"
                        Case DatabaseProvider.mySQL
                            Return "X(" & tableAlias & field & ")"
                        Case DatabaseProvider.Oracle
                            Return tableAlias & field & ".X"
                        Case DatabaseProvider.Postgre
                            ' ATT: WKT format does not maintain precision so to prevent floating truncation, use ST_AsBinary or ST_AsEWKB format for
                            'transport.
                            Return ("ST_X(" & tableAlias & field & ")")
                        Case Else
                            Return tableAlias & field
                    End Select

                    ' get Y Coord From GEO
                Case ProviderFunctions.GetY
                    Select Case _provider
                        Case DatabaseProvider.SQLServer
                            Return tableAlias & field & ".STY"
                        Case DatabaseProvider.mySQL
                            Return "Y(" & tableAlias & field & ")"
                        Case DatabaseProvider.Oracle
                            Return tableAlias & field & ".Y"
                        Case DatabaseProvider.Postgre
                            ' ATT: WKT format does not maintain precision so to prevent floating truncation, use ST_AsBinary or ST_AsEWKB format for
                            'transport.
                            Return ("ST_Y(" & tableAlias & field & ")")
                        Case Else
                            Return tableAlias & field
                    End Select
                Case ProviderFunctions.NULLGeom
                    Select Case _provider
                        Case DatabaseProvider.SQLServer
                            Return "NULL"
                        Case DatabaseProvider.mySQL     ' TODO: Verificar NULLGeom mySQL
                            Return "NULL"
                        Case DatabaseProvider.Oracle
                            Return "NULL"
                        Case DatabaseProvider.Postgre   ' TODO: Verificar NULLGeom Postgre
                            Return "NULL"
                        Case Else
                            Return "NULL"
                    End Select

                Case ProviderFunctions.NULLGeomTest
                    Select Case _provider
                        Case DatabaseProvider.SQLServer  'TODO: SqlServer geom isnull
                            Return "NULL"
                        Case DatabaseProvider.mySQL      'TODO: mySql geom isnull
                            Return "NULL"
                        Case DatabaseProvider.Oracle
                            Return tableAlias & field & " is NULL"
                        Case DatabaseProvider.Postgre    'TODO: Postgre geom isnull
                            Return "NULL"
                        Case Else
                            Return "NULL"
                    End Select

                    ' get GEO From WellKnownText
                Case ProviderFunctions.WKTToGeo
                    Select Case _provider
                        Case DatabaseProvider.SQLServer
                            If Mid(field, 1, 1) = "'" Or field = "?" Then
                                Return field & "," & IIf(Val(sParam) = 0, 0, Val(sParam))
                            Else
                                Return "'" & field & "'," & IIf(Val(sParam) = 0, 0, Val(sParam))
                            End If
                        Case DatabaseProvider.mySQL 
                            If Mid(field, 1, 1) = "'" Or field = "?" Then
                                Return "" & field & "," & sParam
                            Else
                                Return "'" & field & "'," & sParam
                            End If
                        Case DatabaseProvider.Oracle
                            Return tableAlias & field
                        Case DatabaseProvider.Postgre
                            If Mid(field, 1, 1) = "'" Or field = "?" Then
                                Return tableAlias & field & "," & IIf(Val(sParam) = 0, -1, Val(sParam))
                            Else
                                Return "'" & tableAlias & field & "'," & IIf(Val(sParam) = 0, -1, Val(sParam))
                            End If
                        Case Else
                            Return tableAlias & field
                    End Select

                Case ProviderFunctions.WKTToGeoPrefix
                    Select Case _provider
                        Case DatabaseProvider.SQLServer
                            Return "geometry::STGeomFromText("
                        Case DatabaseProvider.mySQL
                            Return "GeomFromText("
                        Case DatabaseProvider.Oracle
                            Return ""
                        Case DatabaseProvider.Postgre
                            Return "ST_GeomFromText("
                        Case Else
                            Return ""
                    End Select

                Case ProviderFunctions.WKTToGeoSufix
                    Select Case _provider
                        Case DatabaseProvider.SQLServer
                            Return ")"
                        Case DatabaseProvider.mySQL
                            Return ")"
                        Case DatabaseProvider.Oracle
                            Return ""
                        Case DatabaseProvider.Postgre
                            Return ")"
                        Case Else
                            Return ""
                    End Select

                    ' get area
                Case ProviderFunctions.Area
                    Select Case _provider
                        Case DatabaseProvider.SQLServer
                            Return tableAlias & field & ".STArea()"
                        Case DatabaseProvider.mySQL
                            Return "Area(" & tableAlias & field & ")"
                        Case DatabaseProvider.Oracle
                            Return "SDO_GEOM.SDO_AREA(" & tableAlias & field & "," & sParam & ")"
                        Case DatabaseProvider.Postgre
                            Return "ST_Area(" & tableAlias & field & "," & sParam & ")"
                        Case Else
                            Return tableAlias & field
                    End Select

                    ' get length
                Case ProviderFunctions.Length
                    Select Case _provider
                        Case DatabaseProvider.SQLServer
                            Return tableAlias & field & ".STLength()"
                        Case DatabaseProvider.mySQL
                            Return "GLength(" & tableAlias & field & ")"
                        Case DatabaseProvider.Oracle
                            Return "SDO_GEOM.SDO_LENGTH(" & tableAlias & field & "," & sParam & ")"
                        Case DatabaseProvider.Postgre
                            Return "ST_Length(" & tableAlias & field & "," & sParam & ")"
                        Case Else
                            Return tableAlias & field
                    End Select
                Case ProviderFunctions.Max
                    Select Case _provider
                        Case DatabaseProvider.SQLServer
                            Return "MAX(" & tableAlias & field & ")"
                        Case DatabaseProvider.mySQL
                            Return "MAX(" & tableAlias & field & ")"
                        Case DatabaseProvider.Oracle
                            Return "MAX(" & tableAlias & field & ")"
                        Case DatabaseProvider.Postgre
                            Return "MAX(" & tableAlias & field & ")"
                        Case Else
                            Return tableAlias & field
                    End Select
                    ' get Intersection
                Case ProviderFunctions.Intersection
                    Select Case _provider
                        Case DatabaseProvider.SQLServer
                            Return tableAlias & field & ".STIntersection(" & sParam & ")"
                        Case DatabaseProvider.mySQL
                            Return "ST_Intersection" & tableAlias & field & "," & sParam & ")"
                        Case DatabaseProvider.Oracle
                            Return "SDO_GEOM.SDO_INTERSECTION(" & tableAlias & field & "," & sParam & ")"
                        Case DatabaseProvider.Postgre
                            Return "ST_Intersection(" & tableAlias & field & "," & sParam & ")"
                        Case Else
                            Return tableAlias & field
                    End Select
                    ' isvalid 
                Case ProviderFunctions.IsValid
                    Select Case _provider
                        Case DatabaseProvider.SQLServer
                            Return tableAlias & field & ".STIsValid()"
                        Case DatabaseProvider.mySQL
                            Return tableAlias & field
                        Case DatabaseProvider.Oracle
                            Return "SDO_GEOM.VALIDATE_GEOMETRY(" & tableAlias & field & "," & sParam & ")"
                        Case DatabaseProvider.Postgre
                            Return "STIsValid(" & tableAlias & field & "," & sParam & ")"
                        Case Else
                            Return tableAlias & field
                    End Select

                    ' isvalid 
                Case ProviderFunctions.MakeValid
                    Select Case _provider
                        Case DatabaseProvider.SQLServer
                            Return tableAlias & field & ".MakeValid()"
                        Case DatabaseProvider.mySQL
                            Return tableAlias & field
                        Case DatabaseProvider.Oracle
                            Return tableAlias & field
                        Case DatabaseProvider.Postgre
                            Return tableAlias & field
                        Case Else
                            Return tableAlias & field
                    End Select

                    ' intersects
                Case ProviderFunctions.IntersectsPrefix
                    Select Case _provider
                        Case DatabaseProvider.SQLServer
                            Return tableAlias & field
                        Case DatabaseProvider.mySQL
                            Return tableAlias & field
                        Case DatabaseProvider.Oracle
                            Return tableAlias & field
                        Case DatabaseProvider.Postgre
                            Return "ST_Intersects("
                        Case Else
                            Return tableAlias & field
                    End Select
                Case ProviderFunctions.IntersectsSufix
                    Select Case _provider
                        Case DatabaseProvider.SQLServer
                            Return tableAlias & field
                        Case DatabaseProvider.mySQL
                            Return tableAlias & field
                        Case DatabaseProvider.Oracle
                            Return tableAlias & field
                        Case DatabaseProvider.Postgre
                            Return ")"
                        Case Else
                            Return tableAlias & field
                    End Select
                    ' buffer
                Case ProviderFunctions.GetBuffer
                    Select Case _provider
                        Case DatabaseProvider.SQLServer
                            Return tableAlias & field & ".StBuffer(" & sParam & ")"
                        Case DatabaseProvider.mySQL
                            Return tableAlias & field
                        Case DatabaseProvider.Oracle
                            Return tableAlias & field
                        Case DatabaseProvider.Postgre
                            Return "ST_Buffer(" & tableAlias & field & "," & sParam & ")"
                        Case Else
                            Return tableAlias & field
                    End Select
                Case Else

                    Return tableAlias & field

            End Select
        End SyncLock

    End Function

    Private Function TranslateCommandText(ByVal commandText As String) As String
        SyncLock _locker
            Dim kParam As Integer = 0
            Dim res As String = ""
            Dim i As Integer
            Dim a As New pccString4.pccString4(commandText)
            Dim o As String() = a.Split("?", "'")

            For i = 0 To o.GetLength(0) - 1
                kParam += 1
                res &= o(i)
                If i < o.GetLength(0) - 1 Then res &= ParameterPrefix(kParam.ToString)
            Next

            Return res
        End SyncLock

    End Function

    Private Function ParameterPrefix(ByVal paramvalue As String) As String
        SyncLock _locker
            Select Case _provider
                Case DatabaseProvider.mySQL
                    Return "@" & paramvalue
                Case DatabaseProvider.Oracle
                    Return ":" & paramvalue
                Case DatabaseProvider.Postgre
                    Return ":" & paramvalue ' TEST: Testar prefixo de parâmetro em Postgre 
                Case DatabaseProvider.SQLServer
                    Return "@" & paramvalue
                Case Else
                    Return paramvalue
            End Select
        End SyncLock

    End Function

    Public Sub Cancel() Implements IDbCommand.Cancel
        SyncLock _locker
            Try
                _comm.Cancel()
            Catch ex As Exception
                Throw ex
            End Try
        End SyncLock

    End Sub

    Public Property CommandText() As String Implements IDbCommand.CommandText

        Get
            SyncLock _locker
                Return _comm.CommandText
            End SyncLock

        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _comm.CommandText = TranslateCommandText(value)
            End SyncLock

        End Set
    End Property

    Public Property CommandTimeout() As Integer Implements IDbCommand.CommandTimeout
        Get
            SyncLock _locker
                Return _comm.CommandTimeout
            End SyncLock

        End Get
        Set(ByVal value As Integer)
            SyncLock _locker
                _comm.CommandTimeout = value
            End SyncLock

        End Set
    End Property

    Public Property CommandType() As CommandType Implements IDbCommand.CommandType
        Get
            Return _comm.CommandType
        End Get
        Set(ByVal value As CommandType)
            _comm.CommandType = value
        End Set
    End Property

    Public Property Connection() As IDbConnection Implements IDbCommand.Connection
        Get
            Return _comm.Connection
        End Get
        Set(ByVal value As IDbConnection)
            _comm.Connection = value
        End Set
    End Property

    Public Function CreateParameter() As IDbDataParameter Implements IDbCommand.CreateParameter

        SyncLock _locker 
            Try
                Return _comm.CreateParameter

            Catch ex As Exception
                Throw ex
            End Try
        End SyncLock
    End Function

    Public Function CreateParameter(ByVal parameterType As SqlDbType, ByVal parameterValue As Object) As IDbDataParameter
        SyncLock _locker
            Dim parameter As IDbDataParameter = _comm.CreateParameter
            parameter.ParameterName = ""
            parameter.DbType = parameterType
            parameter.Value = parameterValue

            Try
                Return parameter
            Catch ex As Exception
                Throw ex
            End Try
        End SyncLock

    End Function

    Public Function ExecuteNonQuery() As Integer Implements IDbCommand.ExecuteNonQuery
        SyncLock _locker
            Try
                Return _comm.ExecuteNonQuery
            Catch ex As Exception
                Throw ex
            End Try
        End SyncLock

    End Function

    Public Function ExecuteReader() As IDataReader Implements IDbCommand.ExecuteReader
        SyncLock _locker
            Try
                Return _comm.ExecuteReader
            Catch ex As Exception
                Throw ex
            End Try
        End SyncLock

    End Function

    Public Function ExecuteReader(ByVal behavior As CommandBehavior) As IDataReader Implements IDbCommand.ExecuteReader

        Try
            Return _comm.ExecuteReader(behavior)
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Function ExecuteScalar() As Object Implements IDbCommand.ExecuteScalar
        SyncLock _locker
            Try
                Return _comm.ExecuteScalar
            Catch ex As Exception
                Throw ex
            End Try
        End SyncLock

    End Function

    Public Sub AddParameter(ByVal parameter As IDbDataParameter)
        SyncLock _locker
            Dim sPN As String = parameter.ParameterName
            If sPN = "" Then
                sPN = BuildParameterName()
            End If
            parameter.ParameterName = sPN
            _comm.Parameters.Add(parameter)
        End SyncLock

    End Sub

    Public Function BuildParameterName() As String
        SyncLock _locker
            Dim iP As Integer = _comm.Parameters.Count + 1

            Return ParameterPrefix(iP.ToString)
        End SyncLock

    End Function

    Public ReadOnly Property Parameters() As IDataParameterCollection Implements IDbCommand.Parameters

        Get
            SyncLock _locker
                Try
                    Return _comm.Parameters
                Catch ex As Exception
                    Throw ex
                End Try
            End SyncLock

        End Get

    End Property

    Public Sub Prepare() Implements IDbCommand.Prepare
        SyncLock _locker
            Try
                _comm.Prepare()
            Catch ex As Exception
                Throw ex
            End Try
        End SyncLock

    End Sub

    Public Property Transaction() As IDbTransaction Implements IDbCommand.Transaction

        Get
            SyncLock _locker
                Try
                    Return _comm.Transaction
                Catch ex As Exception
                    Throw ex
                End Try
            End SyncLock

        End Get

        Set(ByVal value As IDbTransaction)
            SyncLock _locker
                Try
                    _comm.Transaction = value
                Catch ex As Exception
                    Throw ex
                End Try
            End SyncLock

        End Set

    End Property

    Public Property UpdatedRowSource() As UpdateRowSource Implements IDbCommand.UpdatedRowSource

        Get
            SyncLock _locker
                Try
                    Return _comm.UpdatedRowSource
                Catch ex As Exception
                    Throw ex
                End Try
            End SyncLock

        End Get

        Set(ByVal value As UpdateRowSource)
            SyncLock _locker
                _comm.UpdatedRowSource = value
            End SyncLock

        End Set

    End Property

    Private disposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        SyncLock _locker
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: free managed resources when explicitly called
                End If
                _comm.Dispose()

                ' TODO: free shared unmanaged resources
            End If

            Me.disposedValue = True
        End SyncLock

    End Sub

#Region " IDisposable Support "
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        SyncLock _locker
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End SyncLock

    End Sub
#End Region

End Class

<Serializable()> _
Public Class Connection
    Implements IDbConnection

    Private _conn As IDbConnection
    Private _provider As DatabaseProvider
    Private _assemblyfilename As String
    Private _asm As [Assembly]
    Private _types As Type()
    Private _connectionclassname As String
    Private _connectionstring As String
    Private Shared ReadOnly _locker As New Object()

    Public Sub New(ByRef p_conn As pccDB4.Connection)

        Me.New(p_conn._assemblyfilename, p_conn.ConnectionClassName, p_conn.Provider)
        SyncLock _locker
            Me.ConnectionString = p_conn._connectionstring
        End SyncLock
    End Sub

    Public Sub New(ByVal assemblyFileName As String, ByVal connectionClassName As String, ByVal provider As DatabaseProvider)

        Me.New(assemblyFileName, connectionClassName, provider, BindingFlags.NonPublic Or BindingFlags.Public Or BindingFlags.Static Or _
                BindingFlags.Instance Or BindingFlags.DeclaredOnly)

    End Sub

    Public Sub New(ByVal assemblyFileName As String, ByVal connectionClassName As String, ByVal provider As DatabaseProvider, ByVal bindingFlags As BindingFlags)
        SyncLock _locker
            _provider = provider
            _assemblyfilename = assemblyFileName
            _connectionclassname = connectionClassName

            Try
                _asm = [Assembly].LoadFrom(assemblyFileName)
                _types = _asm.GetTypes()
                _conn = GetObjectByName(connectionClassName)
            Catch ex As Exception
                Throw ex

            End Try
        End SyncLock
    End Sub

    Public Function GetObjectByName(ByVal className As String) As Object

        ' procura classe Connection
        For Each oType As Type In _types
            If oType.Name.ToLower = className.ToLower Then
                Return _asm.CreateInstance(oType.FullName.ToLower, True)
            End If
        Next oType

        Return Nothing

    End Function

    Public ReadOnly Property Provider() As DatabaseProvider

        Get
            SyncLock _locker
                Return _provider
            End SyncLock
        End Get

    End Property

    Public ReadOnly Property AssemblyFileName() As String

        Get
            SyncLock _locker
                Return _assemblyfilename
            End SyncLock
        End Get

    End Property

    Public ReadOnly Property ConnectionClassName() As String

        Get
            SyncLock _locker
                Return _connectionclassname
            End SyncLock
        End Get

    End Property

    Public Function BeginTransaction() As IDbTransaction Implements IDbConnection.BeginTransaction
        SyncLock _locker
            Try
                Return _conn.BeginTransaction()
            Catch ex As Exception
                Throw ex
            End Try
        End SyncLock
    End Function

    Public Function BeginTransaction(ByVal il As IsolationLevel) As IDbTransaction Implements IDbConnection.BeginTransaction

        Try
            Return _conn.BeginTransaction(il)
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub ChangeDatabase(ByVal databaseName As String) Implements IDbConnection.ChangeDatabase
        SyncLock _locker
            Try
                _conn.ChangeDatabase(databaseName)
            Catch ex As Exception
                Throw ex
            End Try
        End SyncLock
    End Sub

    Public Sub Close() Implements IDbConnection.Close
        SyncLock _locker
            Try
                _conn.Close()

                '_conn.Dispose()
            Catch ex As Exception
                Throw ex
            End Try
        End SyncLock
    End Sub

    Public Property ConnectionString() As String Implements IDbConnection.ConnectionString

        Get
            SyncLock _locker
                Return _conn.ConnectionString
            End SyncLock
        End Get

        Set(ByVal value As String)
            SyncLock _locker
                _conn.ConnectionString = value
                _connectionstring = value
            End SyncLock
        End Set

    End Property

    Public ReadOnly Property ConnectionTimeout() As Integer Implements IDbConnection.ConnectionTimeout
        Get
            SyncLock _locker
                Return _conn.ConnectionTimeout
            End SyncLock
        End Get
    End Property

    Public Function CreateCommand() As IDbCommand Implements IDbConnection.CreateCommand
        SyncLock _locker
            Try

                Return New Command(_conn.CreateCommand(), _provider)

            Catch ex As Exception
                Throw ex
            End Try
        End SyncLock
    End Function

    Public ReadOnly Property Database() As String Implements IDbConnection.Database
        Get
            SyncLock _locker
                Return _conn.Database
            End SyncLock
        End Get
    End Property

    Public Sub Open() Implements IDbConnection.Open
        SyncLock _locker
            Try
                _conn.Open()
            Catch ex As Exception
                Throw ex
            End Try
        End SyncLock
    End Sub

    Public ReadOnly Property State() As ConnectionState Implements IDbConnection.State

        Get
            SyncLock _locker
                Return _conn.State
            End SyncLock
        End Get
    End Property

    Private disposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        SyncLock _locker
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: free managed resources when explicitly called
                End If
                _conn.Dispose()
                ' TODO: free shared unmanaged resources
            End If
            Me.disposedValue = True
        End SyncLock
    End Sub

#Region " IDisposable Support "
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        SyncLock _locker
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End SyncLock
    End Sub
#End Region

End Class
