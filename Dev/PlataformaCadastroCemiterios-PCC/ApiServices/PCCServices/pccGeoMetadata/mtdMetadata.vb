Imports System.Xml.Serialization
Imports System.IO
Imports System.Text

Imports pccDB4

''' <summary>
''' Classe Metadados - Principal
''' </summary>
''' <remarks></remarks>
Public Class mtdMetadata

    Private _meta_metadata As mtdMetaMetadata ' Meta-metadados
    Private _contact As mtdContact ' Contactos
    Private _referencesystem As mtdReferenceSystemInfo ' Sistema de Referência
    Private _identification As mtdIdentificationInfo ' Identificação
    Private _dataquality As mtdDataQualityInfo ' Qualidade dos Dados
    Private _distribution As mtdDistributionInfo ' Distribuição
    Private _isnew As Boolean ' flag que indica se vai fazer INSERT ou UPDATE na base de dados
    Private _id As String ' id do registo na base de dados

    Private Shared ReadOnly _locker As New Object()

    Public Sub New()
        SyncLock _locker
            _isnew = True
        End SyncLock
    End Sub

    ''' <summary>
    ''' Meta-Metadados
    ''' </summary>
    ''' <value>novo Meta-Metadados</value>
    ''' <returns>Meta-Metadados</returns>
    ''' <remarks></remarks>
    Public Property Meta_Metadata As mtdMetaMetadata
        Get
            SyncLock _locker
                Return _meta_metadata
            End SyncLock
        End Get
        Set(ByVal value As mtdMetaMetadata)
            SyncLock _locker
                _meta_metadata = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Contactos
    ''' </summary>
    ''' <value>novo Contacto</value>
    ''' <returns>Contacto</returns>
    ''' <remarks></remarks>
    Public Property Contact As mtdContact
        Get
            SyncLock _locker
                Return _contact
            End SyncLock
        End Get
        Set(ByVal value As mtdContact)
            SyncLock _locker
                _contact = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Sistema de Referência
    ''' </summary>
    ''' <value>novo Sistema de Referência</value>
    ''' <returns>Sistema de Referência</returns>
    ''' <remarks></remarks>
    Public Property ReferenceSystem As mtdReferenceSystemInfo
        Get
            SyncLock _locker
                Return _referencesystem
            End SyncLock
        End Get
        Set(ByVal value As mtdReferenceSystemInfo)
            SyncLock _locker
                _referencesystem = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Identificação
    ''' </summary>
    ''' <value>nova Identificação</value>
    ''' <returns>Identificação</returns>
    ''' <remarks></remarks>
    Public Property Identification As mtdIdentificationInfo
        Get
            SyncLock _locker
                Return _identification
            End SyncLock
        End Get
        Set(ByVal value As mtdIdentificationInfo)
            SyncLock _locker
                _identification = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Qualidade dos Dados
    ''' </summary>
    ''' <value>nova Qualidade dos Dados</value>
    ''' <returns>Qualidade dos Dados</returns>
    ''' <remarks></remarks>
    Public Property DataQuality As mtdDataQualityInfo
        Get
            SyncLock _locker
                Return _dataquality
            End SyncLock
        End Get
        Set(ByVal value As mtdDataQualityInfo)
            SyncLock _locker
                _dataquality = value
            End SyncLock
        End Set
    End Property

    Public Function GetId() As String
        SyncLock _locker
            Return _id
        End SyncLock
    End Function

    ''' <summary>
    ''' Distribuição
    ''' </summary>
    ''' <value>nova Distribuição</value>
    ''' <returns>Distribuição</returns>
    ''' <remarks></remarks>
    Public Property Distribution As mtdDistributionInfo
        Get
            SyncLock _locker
                Return _distribution
            End SyncLock
        End Get
        Set(ByVal value As mtdDistributionInfo)
            SyncLock _locker
                _distribution = value
            End SyncLock
        End Set
    End Property

    ' TODO: testar isto muito bem
    Public Function Save(ByRef conn As Connection, ByVal trans As IDbTransaction) As Boolean
        SyncLock _locker
            Dim com As Command
            Dim par As IDbDataParameter

            Try

                com = conn.CreateCommand

                If trans IsNot Nothing Then com.Transaction = trans

                If _isnew Then
                    com.CommandText = "insert into metadados(metadados,rec_id) "
                    com.CommandText &= "values (?,?)"
                Else
                    com.CommandText = "update metadados set metadados=? where rec_id=?"

                End If
                com.CommandType = CommandType.Text

                If _isnew Then _id = Guid.NewGuid.ToString

                Dim serializer As New XmlSerializer(Me.GetType)
                Dim writestream As New MemoryStream()
                serializer.Serialize(writestream, Me)

                writestream.Position = 0
                Dim sr As StreamReader = New StreamReader(writestream)
                Dim sXML As String = sr.ReadToEnd()

                par = com.CreateParameter(DbType.String, sXML)
                com.AddParameter(par)

                par = com.CreateParameter(DbType.String, _id)
                com.AddParameter(par)

                com.ExecuteNonQuery()

            Catch ex As Exception
                Return False

            End Try

            Return True
        End SyncLock
    End Function

    Public Function Delete(ByRef conn As Connection, ByVal trans As IDbTransaction) As Boolean
        SyncLock _locker
            Dim com As Command
            Dim par As IDbDataParameter

            Try

                com = conn.CreateCommand
                If trans IsNot Nothing Then com.Transaction = trans

                com.CommandText = "delete from metadados m where m.rec_id=?"
                com.CommandType = CommandType.Text

                par = com.CreateParameter(DbType.String, _id)
                com.AddParameter(par)

                com.ExecuteNonQuery()

            Catch ex As Exception

                Return False

            End Try

            Return True
        End SyncLock
    End Function

    Public Function GetById(ByRef conn As Connection, ByVal p_id As String) As mtdMetadata
        SyncLock _locker
            Dim com As pccDB4.Command = Nothing
            Dim par As IDbDataParameter = Nothing
            Dim iReader As IDataReader = Nothing
            Dim ret As New mtdMetadata

            Try

                com = conn.CreateCommand

                com.CommandText = "select " + com.GetFunction(ProviderFunctions.Convert, "rec_id", "d", "char(36)") & ", d.metadados from metadados d where d.rec_id=?"
                com.CommandType = CommandType.Text

                par = com.CreateParameter(DbType.String, p_id)
                com.AddParameter(par)

                iReader = com.ExecuteReader()

                If iReader.Read Then

                    Dim sXml As String

                    If iReader.IsDBNull(1) Then
                        sXml = ""
                    Else
                        sXml = iReader.GetString(1)
                    End If

                    Try
                        Dim serializer As XmlSerializer = New XmlSerializer(ret.GetType())
                        ret = serializer.Deserialize(New StringReader(sXml))
                        ret._isnew = False
                        Return ret

                    Catch ex As Exception
                        ret = Nothing
                    End Try

                Else
                    ret = Nothing
                End If

                If iReader IsNot Nothing Then
                    If Not iReader.IsClosed Then iReader.Close()
                End If
            Catch ex As Exception

                If iReader IsNot Nothing Then
                    If Not iReader.IsClosed Then iReader.Close()
                End If
                ret = Nothing

            Finally

                If iReader IsNot Nothing Then
                    If Not iReader.IsClosed Then iReader.Close()
                End If

            End Try

            Return ret
        End SyncLock
    End Function

End Class
