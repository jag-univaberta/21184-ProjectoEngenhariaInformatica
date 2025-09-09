Imports System
Imports System.IO
Imports System.Xml.Serialization

''' <summary>
''' XML related stuff; (de)serializers ....
''' </summary>
''' <remarks></remarks>
Public Class cXML
    Private Shared ReadOnly _locker As New Object()
    Public Function FileSerializer(ByRef o As Object, ByVal fileName As String) As Boolean
        SyncLock _locker
            Try
                Dim serializer As XmlSerializer = New XmlSerializer(o.GetType())
                Dim stream As Stream = New FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None)
                serializer.Serialize(stream, o)
                stream.Close()
                Return True
            Catch ex As Exception
                Return False
            End Try
        End SyncLock 
    End Function

    Public Function FileDeserializer(ByRef o As Object, ByVal fileName As String) As Boolean
        SyncLock _locker
            Try
                Dim objStreamReader As New StreamReader(fileName)
                Dim deserializer As XmlSerializer = New XmlSerializer(o.GetType())
                o = deserializer.Deserialize(objStreamReader)
                objStreamReader.Close()
                Return True
            Catch ex As Exception
                Return False
            End Try
        End SyncLock 
    End Function

    Public Function StringSerializer(ByRef o As Object) As String
        SyncLock _locker
            Dim sw As New StringWriter()
            Try
                Dim serializer As XmlSerializer = New XmlSerializer(o.GetType())
                serializer.Serialize(sw, o)
                Return sw.ToString
            Catch ex As Exception
                Return ""
            End Try
        End SyncLock 
    End Function

    Public Function StringDeserializer(ByRef o As Object, ByVal stringXML As String) As Boolean
        SyncLock _locker
            Try
                Dim deserializer As XmlSerializer = New XmlSerializer(o.GetType())

                o = deserializer.Deserialize(New StringReader(stringXML))
                Return True

            Catch ex As Exception
                Return False
            End Try
        End SyncLock 
    End Function

End Class
