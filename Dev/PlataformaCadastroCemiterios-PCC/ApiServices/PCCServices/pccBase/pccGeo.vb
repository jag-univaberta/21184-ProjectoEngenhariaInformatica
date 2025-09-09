Imports System.Xml.Serialization
Imports System.IO

Imports System.Text
Imports pccDB4
Imports pccDXF4
Imports pccGeoMetadata4
Imports System.Runtime.Serialization


<DataContract()> _
<Serializable()> _
Public Enum pccGeoGeometryType
    Point = 1
    LineString = 2
    Polygon = 3
    MultiPoint = 10
    MultiLineString = 20
    MultiPolygon = 30
    Collection = 255
End Enum

Public Class pccGeoUtils
    Private Shared ReadOnly _locker As New Object()
    Public Function SDFLoaderConverter(ByVal SDFLoaderLocation As String, ByVal Directorio As String, ByVal NomeFicheiro As String, ByVal apiSessionID As String) As String
        SyncLock _locker
            Dim res As Boolean = False
            Dim methodName As String = "SDFLoaderConverter"
            Try

                If Not Directorio.EndsWith("\") Then Directorio = Directorio + "\"


                Dim mapSDFLoader As String = SDFLoaderLocation


                ' Se nao estiver definido onde está localizado o sdfloader entao vamos ver se está na path
                If mapSDFLoader = Nothing Then
                    mapSDFLoader = "sdfld32i.exe"
                End If

                Dim DirectorioTemporario As String

                DirectorioTemporario = Directorio

                If Not DirectorioTemporario.EndsWith("\") Then DirectorioTemporario = DirectorioTemporario + "\"
                Dim extensao As String = UCase(Right(NomeFicheiro, 3))
                Dim TipoImport As String = ""

                If extensao = "SHP" Then TipoImport = "IH" 'ArcView Shapefile format (SHP)
                If extensao = "DXF" Then TipoImport = "IX" 'Autodesk Data eXchange Format (DXF)
                If extensao = "MIF" Then TipoImport = "IM" 'MapInfo Interchange format (MIF)
                If extensao = "DWG" Then TipoImport = "IW" 'AutoCAD, Autodesk Map, and Autodesk World drawing or DWG format
                If extensao = "DGN" Then TipoImport = "ID" 'Intergraph DGN Format (DGN)

                Dim TEMP As String = DirectorioTemporario
                ' If IO.File.Exists(TEMP) Then Kill(TEMP)

                'Dim Comandos As String = " /OL /KIF /WO /PL2PG " 'input.dwg output.sdf'"

                Dim Comandos As String = " /OL /WO /PL2PG " 'input.dwg output.sdl'"
                Dim Ficheiro As String = Directorio & NomeFicheiro

                Dim FicheiroOutput As String = Directorio & Mid(NomeFicheiro, 1, Len(NomeFicheiro) - 4) & ".sdl"
                Dim ComandoDOS As String = """" & mapSDFLoader & """" & " /" & TipoImport & Comandos & " " & """" & Ficheiro & """" & " " & """" & FicheiroOutput & """"

                If IO.File.Exists(FicheiroOutput) Then
                    Try
                        Kill(FicheiroOutput)
                    Catch ex As Exception
                        MsgBox("erro a eliminar fich " & ex.Message)

                    End Try

                End If
                If IO.File.Exists(mapSDFLoader) Then
                    'res = 8
                    Dim oWrite As System.IO.StreamWriter
                    oWrite = IO.File.CreateText(DirectorioTemporario & "\converte_" & apiSessionID & NomeFicheiro & ".bat")

                    ' oWrite.WriteLine("@ECHO OFF")
                    'oWrite.WriteLine("ECHO Aguarde um momento.....")
                    oWrite.WriteLine(ComandoDOS)
                    ' oWrite.WriteLine("""" & mapSDFLoader & """" & " /" & TipoImport & Comandos & " " & """" & Ficheiro & """" & " " & """" & FicheiroOutput & """")
                    oWrite.Close()
                    Try

                        Shell(DirectorioTemporario & "\converte_" & apiSessionID & NomeFicheiro & ".bat", AppWinStyle.MaximizedFocus, True)

                    Catch ex As Exception
                        MsgBox("exception " & ex.Message)
                    End Try
                    'Kill(DirectorioTemporario & "\converte.bat")

                    If IO.File.Exists(FicheiroOutput) Then

                        res = True

                    Else
                        Try
                            Dim output As System.IO.StreamReader
                            Dim auxsaida As String()
                            Dim info As ProcessStartInfo = New ProcessStartInfo()
                            info.RedirectStandardOutput = True
                            info.RedirectStandardError = True
                            info.UseShellExecute = False
                            info.WindowStyle = ProcessWindowStyle.Hidden
                            info.FileName = " cmd.exe " 'DirectorioTemporario & "converte.bat"
                            info.Arguments = " " & ComandoDOS

                            Dim proc As Process = New Process()
                            proc.StartInfo = info
                            proc.Start()

                            Dim mensagemerror As String = proc.StandardError.ReadToEnd()

                            Dim aoutput As String = proc.StandardOutput.ReadToEnd()


                            auxsaida = System.Text.RegularExpressions.Regex.Split(mensagemerror, "\r\n")

                            Dim oWriteErr As System.IO.StreamWriter
                            Try
                                oWriteErr = IO.File.CreateText(DirectorioTemporario & "pcc.err")
                                oWriteErr.WriteLine(mensagemerror)

                                oWriteErr.Close()
                            Catch ex As Exception
                                Dim totalex As String = "2 " & ex.Message.ToString
                            End Try

                            'Dim processInfo As New Process
                            'processInfo.StartInfo.UseShellExecute = False
                            '' You can start any process, HelloWorld is a do-nothing example.
                            'processInfo.StartInfo.FileName = DirectorioTemporario & "\converte.bat"
                            'processInfo.StartInfo.Arguments = ""
                            'processInfo.StartInfo.CreateNoWindow = True

                            'processInfo.StartInfo.RedirectStandardOutput = True
                            'processInfo.StartInfo.RedirectStandardInput = True
                            'processInfo.StartInfo.RedirectStandardError = True
                            'processInfo.Start()

                            'processInfo.WaitForExit()
                            'If (processInfo.ExitCode = 0) Then
                            '    res = True
                            'Else
                            '    res = False
                            'End If
                            'processInfo.Close()

                            If IO.File.Exists(FicheiroOutput) Then

                                res = True
                            Else
                                res = False
                            End If

                        Catch ex As Exception
                            res = False
                        End Try
                        res = False
                    End If

                Else
                    res = False
                End If

            Catch ex As Exception

                res = False
            End Try
            Return res
        End SyncLock


    End Function

    Public Function SendEmailMessage(ByVal strHostId As String, _
                                    ByVal strPort As Integer, _
                                    ByVal strSSL As String, _
                                    ByVal strUserId As String, _
                                    ByVal strPasswd As String, _
                                    ByVal strFrom As String, _
                                    ByVal strTo() As String, _
                                    ByVal strSubject As String, _
                                    ByVal strMessage As String, _
                                    ByVal strFileList As String) As String
        SyncLock _locker
            Try
                For Each item As String In strTo

                    Dim MailMsg As New Net.Mail.MailMessage(New Net.Mail.MailAddress(strFrom.Trim()), New Net.Mail.MailAddress(item))

                    MailMsg.BodyEncoding = System.Text.Encoding.GetEncoding(0)
                    MailMsg.Subject = strSubject.Trim()
                    MailMsg.Body = strMessage.Trim() & vbCrLf
                    MailMsg.Priority = Net.Mail.MailPriority.High
                    MailMsg.IsBodyHtml = True

                    Dim MsgAttach As New Net.Mail.Attachment(strFileList)
                    MailMsg.Attachments.Add(MsgAttach)

                    Dim SmtpMail As New Net.Mail.SmtpClient

                    SmtpMail.Host = strHostId
                    If strPort > 0 Then SmtpMail.Port = strPort
                    If strSSL = "True" Then SmtpMail.EnableSsl = True
                    If strSSL = "False" Then SmtpMail.EnableSsl = False
                    SmtpMail.Credentials = New System.Net.NetworkCredential(strUserId, strPasswd)

                    SmtpMail.Send(MailMsg)

                    Return "Ok"

                Next

                Return "Erro: Mensagem sem destinatários!"

            Catch ex As Exception

                Return ex.Message

            End Try
        End SyncLock


    End Function

    Public Function SendEmailMessage(ByVal strHostId As String, _
                                    ByVal strPort As Integer, _
                                    ByVal strSSL As String, _
                                    ByVal strUserId As String, _
                                    ByVal strPasswd As String, _
                                    ByVal strFrom As String, _
                                    ByVal strTo() As String, _
                                    ByVal strSubject As String, _
                                    ByVal strMessage As String, _
                                    ByVal strFileList() As String) As String
        SyncLock _locker
            Try
                For Each item As String In strTo

                    Dim MailMsg As New Net.Mail.MailMessage(New Net.Mail.MailAddress(strFrom.Trim()), New Net.Mail.MailAddress(item))

                    MailMsg.BodyEncoding = System.Text.Encoding.GetEncoding(0)
                    MailMsg.Subject = strSubject.Trim()
                    MailMsg.Body = strMessage.Trim() & vbCrLf
                    MailMsg.Priority = Net.Mail.MailPriority.High
                    MailMsg.IsBodyHtml = True

                    If strFileList.Length <> 0 Then
                        For Each EL As String In strFileList
                            Dim MsgAttach As New Net.Mail.Attachment(EL)
                            MailMsg.Attachments.Add(MsgAttach)
                        Next
                    End If

                    Dim SmtpMail As New Net.Mail.SmtpClient

                    SmtpMail.Host = strHostId
                    If strPort > 0 Then SmtpMail.Port = strPort
                    If strSSL = "True" Then SmtpMail.EnableSsl = True
                    If strSSL = "False" Then SmtpMail.EnableSsl = False
                    SmtpMail.Credentials = New System.Net.NetworkCredential(strUserId, strPasswd)

                    SmtpMail.Send(MailMsg)

                    Return "Ok"

                Next

                Return "Erro: Mensagem sem destinatários!"

            Catch ex As Exception

                Return ex.Message

            End Try
        End SyncLock


    End Function

    Public Function SendEmailMessage(ByVal strHostId As String, _
                                     ByVal strPort As Integer, _
                                     ByVal strSSL As String, _
                                     ByVal strUserId As String, _
                                     ByVal strPasswd As String, _
                                     ByVal strFrom As String, _
                                     ByVal strTo As String, _
                                     ByVal strSubject As String, _
                                     ByVal strMessage As String, _
                                     ByVal strFile As String) As String

        SyncLock _locker
            Try
                Dim MailMsg As New Net.Mail.MailMessage(New Net.Mail.MailAddress(strFrom.Trim()), New Net.Mail.MailAddress(strTo))

                MailMsg.BodyEncoding = System.Text.Encoding.GetEncoding(0)
                MailMsg.Subject = strSubject.Trim()
                MailMsg.Body = strMessage.Trim() & vbCrLf
                MailMsg.Priority = Net.Mail.MailPriority.High
                MailMsg.IsBodyHtml = False

                If Not strFile = "" Then
                    Dim MsgAttach As New Net.Mail.Attachment(strFile)
                    MailMsg.Attachments.Add(MsgAttach)
                End If

                Dim SmtpMail As New Net.Mail.SmtpClient

                SmtpMail.Host = strHostId
                If strPort > 0 Then SmtpMail.Port = strPort
                If strSSL = "True" Then SmtpMail.EnableSsl = True
                If strSSL = "False" Then SmtpMail.EnableSsl = False
                SmtpMail.Credentials = New System.Net.NetworkCredential(strUserId, strPasswd)

                SmtpMail.Send(MailMsg)

                Return "Ok"

            Catch ex As Exception

                Return ex.Message

            End Try
        End SyncLock


    End Function

    Public Function SendEmailMessage(ByVal strHostId As String, _
                                     ByVal strPort As Integer, _
                                     ByVal strSSL As String, _
                                     ByVal strUserId As String, _
                                     ByVal strPasswd As String, _
                                     ByVal strFrom As String, _
                                     ByVal strTo As String, _
                                     ByVal strSubject As String, _
                                     ByVal strMessage As String, _
                                     ByVal strFile() As String) As String
        SyncLock _locker
            Try
                Dim MailMsg As New Net.Mail.MailMessage(New Net.Mail.MailAddress(strFrom.Trim()), New Net.Mail.MailAddress(strTo))

                MailMsg.BodyEncoding = System.Text.Encoding.GetEncoding(0)
                MailMsg.Subject = strSubject.Trim()
                MailMsg.Body = strMessage.Trim() & vbCrLf
                MailMsg.Priority = Net.Mail.MailPriority.High
                MailMsg.IsBodyHtml = False
                If strFile IsNot Nothing Then
                    If strFile.Length <> 0 Then
                        Dim i As Long
                        For i = 0 To strFile.Length - 1
                            If Not (strFile(i) = Nothing) Then
                                If strFile(i) <> "" Then
                                    Dim MsgAttach As New Net.Mail.Attachment(strFile(i))
                                    MailMsg.Attachments.Add(MsgAttach)
                                End If
                            End If

                        Next
                    End If
                End If
                Dim SmtpMail As New Net.Mail.SmtpClient

                SmtpMail.Host = strHostId
                If strPort > 0 Then SmtpMail.Port = strPort
                If strSSL = "True" Then SmtpMail.EnableSsl = True
                If strSSL = "False" Then SmtpMail.EnableSsl = False
                SmtpMail.Credentials = New System.Net.NetworkCredential(strUserId, strPasswd)

                SmtpMail.Send(MailMsg)

                Return "Ok"

            Catch ex As Exception

                Return ex.Message

            End Try
        End SyncLock


    End Function

    Public Function GetStringFromMemoryStream(ByVal m As MemoryStream) As String
        SyncLock _locker
            If (m Is Nothing Or m.Length = 0) Then
                Return ""
            End If

            m.Flush()
            m.Position = 0

            Dim sr As StreamReader = New StreamReader(m)
            Dim s As String = sr.ReadToEnd()

            Return s
        End SyncLock


    End Function

    Public Function GetMemoryStreamFromString(ByVal s As String) As MemoryStream
        SyncLock _locker
            If (s = Nothing Or s.Length = 0) Then
                Return Nothing
            End If

            Dim m As MemoryStream = New MemoryStream()
            Dim sw As StreamWriter = New StreamWriter(m)

            sw.Write(s)
            sw.Flush()

            Return m
        End SyncLock


    End Function

    Public Function CoordsValid(ByVal coords() As String) As Boolean
        SyncLock _locker
            For i As Integer = 0 To coords.GetUpperBound(0)
                If Not IsNumeric(coords(i)) Then
                    Return False
                End If
            Next

            Return True
        End SyncLock


    End Function

    Public Function CalcLength(ByRef points As pccGeoPoint()) As Double
        SyncLock _locker
            Dim ret As Double = 0

            For i As Integer = 0 To points.GetLength(0) - 2
                ret += Distance(points(i), points(i + 1))
            Next

            Return ret
        End SyncLock


    End Function

    Public Function CalcLenght(ByRef points As List(Of pccGeoPoint)) As Double
        SyncLock _locker
            Dim ret As Double = 0

            For i As Integer = 0 To points.Count - 2
                ret += Distance(points(i), points(i + 1))
            Next

            Return ret
        End SyncLock


    End Function

    Public Function CalcArea(ByRef points As pccGeoPoint()) As Double
        SyncLock _locker
            Dim ret As Double = 0

            Dim ix As Integer = points.GetLength(0) - 2
            Dim closed As Boolean = points(0).X = points(ix + 1).X And points(0).Y = points(ix + 1).Y

            If points(0).Is3d Then closed = closed And points(0).Z = points(ix + 1).Z

            If closed Then ix -= 1

            Dim tMais As Double = 0
            Dim tMenos As Double = 0

            For i As Integer = 0 To ix
                tMais += points(i).X * points(i + 1).Y
                tMenos += points(i + 1).X * points(i).Y
            Next

            tMais += points(ix + 1).X * points(0).Y
            tMenos += points(0).X * points(ix + 1).Y

            Return (tMais - tMenos) / 2

        End SyncLock

    End Function

    'TODO: Testar -> List é coisa nova por aqui Testado: Jaugusto
    Public Function CalcArea(ByRef points As List(Of pccGeoPoint)) As Double
        SyncLock _locker
            Dim ret As Double = 0

            Dim ix As Integer = points.Count - 2
            Dim closed As Boolean = points(0).X = points(ix + 1).X And points(0).Y = points(ix + 1).Y

            If points(0).Is3d Then closed = closed And points(0).Z = points(ix + 1).Z

            If closed Then ix -= 1

            Dim tMais As Double = 0
            Dim tMenos As Double = 0

            For i As Integer = 0 To ix
                tMais += points(i).X * points(i + 1).Y
                tMenos += points(i + 1).X * points(i).Y
            Next

            tMais += points(ix + 1).X * points(0).Y
            tMenos += points(0).X * points(ix + 1).Y

            Return (tMais - tMenos) / 2
        End SyncLock


    End Function

    Public Sub CalcMBR(ByRef points() As pccGeoPoint, ByRef destpolygon As pccGeoRectangle) ' ByRef Height As Double, ByRef Width As Double)
        SyncLock _locker
            Dim destmbr(3) As pccGeoPoint

            Dim minx As Double = 9999999999
            Dim maxx As Double = -9999999999
            Dim miny As Double = 9999999999
            Dim maxy As Double = -9999999999

            For Each p As pccGeoPoint In points
                If p.X < minx Then minx = p.X
                If p.X > maxx Then maxx = p.X
                If p.Y < miny Then miny = p.Y
                If p.Y > maxy Then maxy = p.Y
            Next

            destmbr(0) = New pccGeoPoint(minx, miny)
            destmbr(1) = New pccGeoPoint(maxx, miny)
            destmbr(2) = New pccGeoPoint(maxx, maxy)
            destmbr(3) = New pccGeoPoint(minx, maxy)

            destpolygon = New pccGeoRectangle
            destpolygon.AddVertice(destmbr(0))
            destpolygon.AddVertice(destmbr(1))
            destpolygon.AddVertice(destmbr(2))
            destpolygon.AddVertice(destmbr(3))
            destpolygon.AddVertice(destmbr(0))
        End SyncLock


    End Sub

    'TODO: Testar -> List é coisa nova por aqui Testado: Jaugusto
    Public Sub CalcMBR(ByRef points As List(Of pccGeoPoint), ByRef destpolygon As pccGeoRectangle)
        SyncLock _locker
            Dim destmbr(3) As pccGeoPoint

            Dim minx As Double = 9999999999
            Dim maxx As Double = -9999999999
            Dim miny As Double = 9999999999
            Dim maxy As Double = -9999999999

            For Each p As pccGeoPoint In points
                If p.X < minx Then minx = p.X
                If p.X > maxx Then maxx = p.X
                If p.Y < miny Then miny = p.Y
                If p.Y > maxy Then maxy = p.Y
            Next

            destmbr(0) = New pccGeoPoint(minx, miny)
            destmbr(1) = New pccGeoPoint(maxx, miny)
            destmbr(2) = New pccGeoPoint(maxx, maxy)
            destmbr(3) = New pccGeoPoint(minx, maxy)

            destpolygon = New pccGeoRectangle
            destpolygon.AddVertice(destmbr(0))
            destpolygon.AddVertice(destmbr(1))
            destpolygon.AddVertice(destmbr(2))
            destpolygon.AddVertice(destmbr(3))
            destpolygon.AddVertice(destmbr(0))
        End SyncLock


    End Sub

    Public Function Intersection(ByRef conn As pccDB4.Connection, ByVal WKTObjecto1 As String, ByVal WKTObjecto2 As String) As pccGeoGeometry
        SyncLock _locker
            Dim ret As pccGeoGeometry

            ' Vamos calcular a interseccao
            Dim com As pccDB4.Command
            Dim iReader As IDataReader = Nothing


            Try

                If conn.Provider = DatabaseProvider.Postgre Then
                    com = conn.CreateCommand
                    com.CommandType = CommandType.Text

                    com.CommandText = "SELECT ST_AsText("
                    com.CommandText += "ST_Intersection("
                    com.CommandText += "ST_Buffer("
                    com.CommandText += com.GetFunction(pccDB4.ProviderFunctions.WKTToGeoPrefix)
                    com.CommandText += com.GetFunction(pccDB4.ProviderFunctions.WKTToGeo, "'" + WKTObjecto1 + "'", "", -1)
                    com.CommandText += com.GetFunction(pccDB4.ProviderFunctions.WKTToGeoSufix)
                    com.CommandText += ",0),"
                    com.CommandText += "ST_Buffer("
                    com.CommandText += com.GetFunction(pccDB4.ProviderFunctions.WKTToGeoPrefix)
                    com.CommandText += com.GetFunction(pccDB4.ProviderFunctions.WKTToGeo, "'" + WKTObjecto2 + "'", "", -1)
                    com.CommandText += com.GetFunction(pccDB4.ProviderFunctions.WKTToGeoSufix)
                    com.CommandText += ",0)"
                    com.CommandText += "))"

                End If
                If conn.Provider = DatabaseProvider.SQLServer Then
                    com = conn.CreateCommand
                    com.CommandType = CommandType.Text

                    com.CommandText = "SELECT "
                    com.CommandText += " geometry::STGeomFromText('" + WKTObjecto1 + "',0)"
                    com.CommandText += ".STBuffer(0)"
                    com.CommandText += ".STIntersection ("
                    com.CommandText += " geometry::STGeomFromText('" + WKTObjecto2 + "',0)"
                    com.CommandText += ".STBuffer(0)"
                    com.CommandText += ")"
                    com.CommandText += ".STBuffer(0)"
                    com.CommandText += ".STAsText()"

                End If
                Try
                    Dim logFile As String = IO.Path.GetTempPath & "Interseccao.log"
                    Using writer As New StreamWriter(logFile, True)
                        writer.WriteLine(com.CommandText)
                    End Using
                Catch ex As Exception

                End Try

                iReader = com.ExecuteReader()
                If iReader.Read Then
                    Dim geoUtil As New pccBase4.pccGeoUtils
                    ret = geoUtil.GeometryFromWKT(iReader.GetString(0).ToString)
                Else
                    ret = Nothing
                End If
                'Case Else
                'Throw New Exception("Intersecção ainda não foi implementada noutros providers.")
                'Return Nothing
                'End Select


                If iReader IsNot Nothing Then
                    If iReader.IsClosed = False Then iReader.Close()
                End If

            Catch ex As Exception

                If iReader IsNot Nothing Then
                    If iReader.IsClosed = False Then iReader.Close()
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

    Public Function GeometryById(ByRef conn As pccDB4.Connection, ByVal id_entidade As String, ByVal storage_table As String) As Object
        SyncLock _locker
            Dim com As pccDB4.Command = Nothing
            Dim par As IDbDataParameter = Nothing
            Dim iReader As IDataReader = Nothing
            Dim ret As New pccGeoCollection

            Try

                com = conn.CreateCommand

                com.CommandText = "select " + com.GetFunction(ProviderFunctions.GeoToWKT, "GEOM", "G", "") + " from " + storage_table + " G WHERE entgeom_id IN (SELECT REC_ID FROM " + storage_table + "_Geo WHERE geo_id=?)"
                com.CommandType = CommandType.Text

                par = com.CreateParameter(DbType.String, id_entidade)
                com.AddParameter(par)

                iReader = com.ExecuteReader()

                While iReader.Read
                    ret.AddGeometry(GeometryFromWKT(iReader.GetString(0)))
                End While

                If iReader IsNot Nothing Then
                    If Not iReader.IsClosed Then iReader.Close()
                End If

            Catch ex As Exception

                If iReader IsNot Nothing Then
                    If Not iReader.IsClosed Then iReader.Close()
                End If

                Throw ex
                ret = Nothing

            Finally

                If iReader IsNot Nothing Then
                    If Not iReader.IsClosed Then iReader.Close()
                End If

            End Try

            Return ret
        End SyncLock


    End Function

    Public Function GeometryConvexHullById(ByRef conn As pccDB4.Connection, ByVal id_entidade As String) As pccGeoPolygon
        SyncLock _locker
            ' TODO: Issue #14 - implementar GeometryConvexHullById
            ' ver GeometryCentroidById embora a técnica da "média dos pontos" usada lá não se possa aplicar aqui.
            Return New pccGeoPolygon()

        End SyncLock

    End Function

    Public Function GeometryCentroidById(ByRef conn As pccDB4.Connection, ByVal id_geom As String, ByVal storage_table As String) As pccGeoPoint
        SyncLock _locker
            Dim com As pccDB4.Command = Nothing
            Dim par As IDbDataParameter = Nothing
            Dim iReader As IDataReader = Nothing
            Dim tmppoint As pccGeoPoint = Nothing
            Dim ret As pccGeoPoint = Nothing

            Dim X As Double = 0
            Dim Y As Double = 0
            Dim k As Long = 0

            Try

                com = conn.CreateCommand

                com.CommandText = "select " + com.GetFunction(ProviderFunctions.GeoToWKT, com.GetFunction(ProviderFunctions.Centroid, "geom", "G", "")) + " from " + storage_table + "_Geo G WHERE rec_id=?"
                com.CommandType = CommandType.Text

                par = com.CreateParameter(DbType.String, id_geom)
                com.AddParameter(par)

                iReader = com.ExecuteReader()

                While iReader.Read
                    k += 1
                    tmppoint = GeometryFromWKT(iReader.GetString(0))
                    X += tmppoint.X
                    Y += tmppoint.Y
                End While

                If iReader IsNot Nothing Then
                    If Not iReader.IsClosed Then iReader.Close()
                End If

                If k > 0 Then
                    ret = New pccGeoPoint(X / k, Y / k)
                End If

                If iReader IsNot Nothing Then
                    If Not iReader.IsClosed Then iReader.Close()
                End If

            Catch ex As Exception

                If iReader IsNot Nothing Then
                    If Not iReader.IsClosed Then iReader.Close()
                End If

                Throw ex
                ret = Nothing

            Finally

                If iReader IsNot Nothing Then
                    If Not iReader.IsClosed Then iReader.Close()
                End If


            End Try

            Return ret

        End SyncLock

    End Function

    ' Funções que transformam pccGeoPolygon e pccGeoMultiPolygon em Clipper.TDoublePoint
    ' para utilização das funções de intersecção de polígonos
    Public Function ClipperPolygonFromPolygon(ByRef pol As pccGeoPolygon) As List(Of List(Of Clipper.TDoublePoint))
        SyncLock _locker
            Dim res As New List(Of List(Of Clipper.TDoublePoint))

            res.Add(ClipperPolygonFromOnePolygon(pol))

            For Each p As pccGeoPolygon In pol.InnerPolygons
                res.Add(ClipperPolygonFromOnePolygon(p))
            Next

            Return res
        End SyncLock


    End Function

    Public Function ClipperPolygonFromMultiPolygon(ByRef pol As pccGeoMultiPolygon) As List(Of List(Of Clipper.TDoublePoint))
        SyncLock _locker
            Dim res As New List(Of List(Of Clipper.TDoublePoint))

            For Each p As pccGeoPolygon In pol.GetBasicGeometries
                res.Add(ClipperPolygonFromOnePolygon(p))
            Next

            Return res
        End SyncLock


    End Function

    Private Function ClipperPolygonFromOnePolygon(ByRef pol As pccGeoPolygon) As List(Of Clipper.TDoublePoint)
        SyncLock _locker
            Dim res As New List(Of Clipper.TDoublePoint)

            For Each p As pccGeoPoint In pol.Vertices
                res.Add(New Clipper.TDoublePoint(p.X, p.Y))
            Next

            Return res
        End SyncLock


    End Function

    Public Function GeometryFromWKT(ByVal wkt As String) As Object
        SyncLock _locker
            wkt = wkt.Trim.ToLower

            Dim geo As Object
            Dim p As Integer = 0
            Dim coords As String = ""
            Dim acoords() As String
            Dim points As String = ""
            Dim apoints() As String
            Dim separadordecimal As String = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator.ToString
            Dim separadormilhares As String

            separadormilhares = "."

            If separadordecimal = "," Then separadormilhares = "."
            If separadordecimal = "." Then separadormilhares = ","

            Try

                Select Case GeometryString(wkt)

                    Case "point"

                        Dim px As Integer = wkt.IndexOf("(") + 1
                        coords = wkt.Substring(px, wkt.IndexOf(")", px) - px)
                        acoords = coords.Trim.Split(" ")


                        If acoords.Length = 2 Then
                            geo = New pccGeoPoint(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)))

                        ElseIf acoords.Length = 3 Then
                            geo = New pccGeoPoint(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)), CDbl(acoords(2).Replace(separadormilhares, separadordecimal)))

                        Else
                            geo = Nothing
                        End If

                        Return geo

                    Case "linestring"

                        geo = New pccGeoLineString()
                        Dim px As Integer = wkt.IndexOf("(") + 1
                        points = wkt.Substring(px, wkt.IndexOf(")", px) - px)

                        apoints = points.Split(",")

                        For i As Integer = 0 To apoints.Length - 1

                            acoords = apoints(i).Trim.Split(" ")
                            Dim auxgeo As pccGeoPoint

                            If acoords.Length = 2 Then
                                auxgeo = New pccGeoPoint(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)))
                            ElseIf acoords.Length = 3 Then
                                auxgeo = New pccGeoPoint(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)), CDbl(acoords(2).Replace(separadormilhares, separadordecimal)))
                            Else
                                auxgeo = Nothing
                            End If

                            If auxgeo IsNot Nothing Then
                                CType(geo, pccGeoLineString).AddVertice(auxgeo)
                            End If

                        Next

                        Return geo

                    Case "polygon"

                        geo = New pccGeoPolygon
                        Dim outter As New pccGeoPolygon
                        Dim inner As New pccGeoPolygon
                        Dim AuxPol() As pccGeoPolygon = Nothing
                        Dim kAuxPol As Integer = 0

                        Dim aux1 As New pccString4.pccString4(wkt)
                        Dim px As Integer = wkt.IndexOf("(")
                        Dim aux2 As New pccString4.pccString4(wkt.Substring(px + 1, aux1.IndexOf(")", "(", ")", px + 1) - px - 1))

                        Dim aaux As String()
                        aaux = aux2.Split(",", "(", ")")

                        Dim i As Integer

                        For i = 0 To aaux.Length - 1
                            aaux(i) = Trim(aaux(i))

                            'px = aaux(i).IndexOf("(") + 1
                            'points = aaux(i).Substring(px, aaux(i).IndexOf(")", px) - px)

                            aaux(i) = aaux(i).Substring(1, aaux(i).Length - 2)

                            apoints = aaux(i).Split(",")
                            ReDim Preserve AuxPol(kAuxPol)
                            AuxPol(kAuxPol) = New pccGeoPolygon
                            kAuxPol += 1

                            For j As Integer = 0 To apoints.Length - 1

                                acoords = apoints(j).Trim.Split(" ")
                                Dim auxgeo As pccGeoPoint

                                If acoords.Length = 2 Then
                                    auxgeo = New pccGeoPoint(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)))
                                ElseIf acoords.Length = 3 Then
                                    auxgeo = New pccGeoPoint(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)), CDbl(acoords(2).Replace(separadormilhares, separadordecimal)))
                                Else
                                    auxgeo = Nothing
                                End If

                                If auxgeo IsNot Nothing Then
                                    AuxPol(kAuxPol - 1).AddVertice(auxgeo)
                                End If

                            Next

                        Next

                        Dim iPolMaior As Integer = -1
                        Dim MaxArea As Double = 0

                        For w As Integer = 0 To AuxPol.GetLength(0) - 1
                            If Math.Abs(AuxPol(w).Area) > MaxArea Then
                                MaxArea = Math.Abs(AuxPol(w).Area)
                                iPolMaior = w
                            End If
                        Next
                        If iPolMaior = -1 Then iPolMaior = 0
                        If AuxPol(iPolMaior).Area > 0 Then
                            Dim aux As New List(Of pccGeoPoint)
                            For i = AuxPol(iPolMaior).Vertices.Count - 1 To 0 Step -1
                                aux.Add(AuxPol(iPolMaior).Vertices(i))
                            Next
                            'For Each v As pccGeoPoint In Invert(AuxPol(iPolMaior).Vertices)
                            For Each v As pccGeoPoint In aux
                                CType(geo, pccGeoPolygon).AddVertice(v)
                            Next
                        Else
                            For Each v As pccGeoPoint In AuxPol(iPolMaior).Vertices
                                CType(geo, pccGeoPolygon).AddVertice(v)
                            Next
                        End If

                        Dim xx As New pccGeoPolygon

                        xx = geo

                        If (xx.Centroid.X > 0) Then
                            Dim xyz As Integer = 0
                            xyz += 1
                        End If

                        ' TODO: rever a questão dos inners não trata inners
                        For j As Integer = 0 To AuxPol.GetLength(0) - 1
                            If j <> iPolMaior Then

                                If AuxPol(j).Area < -10 Then
                                    Dim aux As New List(Of pccGeoPoint)
                                    For i = AuxPol(j).Vertices.Count - 1 To 0 Step -1
                                        aux.Add(AuxPol(iPolMaior).Vertices(i))
                                    Next

                                    Dim pp As New pccGeoPolygon
                                    'For Each v As pccGeoPoint In Invert(AuxPol(j).Vertices)
                                    For Each v As pccGeoPoint In aux
                                        CType(pp, pccGeoPolygon).AddVertice(v)
                                    Next
                                    CType(geo, pccGeoPolygon).AddInnerPolygon(pp)
                                Else
                                    If AuxPol(j).Area >= 10 Then
                                        CType(geo, pccGeoPolygon).AddInnerPolygon(AuxPol(j))
                                    End If
                                End If

                            End If

                        Next

                        Return geo

                    Case "multipoint"

                        geo = New pccGeoMultiPoint()
                        Dim px As Integer = wkt.IndexOf("(") + 1

                        points = wkt.Substring(px, wkt.IndexOf(")", px) - px)
                        apoints = points.Split(",")

                        For i As Integer = 0 To apoints.Length - 1
                            acoords = apoints(i).Trim.Split(" ")
                            Dim auxgeo As pccGeoPoint

                            If acoords.Length = 2 Then
                                auxgeo = New pccGeoPoint(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)))
                            ElseIf acoords.Length = 3 Then
                                auxgeo = New pccGeoPoint(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)), CDbl(acoords(2).Replace(separadormilhares, separadordecimal)))
                            Else
                                auxgeo = Nothing
                            End If

                            If auxgeo IsNot Nothing Then
                                CType(geo, pccGeoMultiPoint).AddPoint(auxgeo)
                            End If

                        Next

                        Return geo

                    Case "multilinestring"

                        geo = New pccGeoMultiLineString

                        Dim aux1 As New pccString4.pccString4(wkt)
                        Dim px As Integer = wkt.IndexOf("(") + 1
                        Dim aux2 As New pccString4.pccString4(wkt.Substring(px, aux1.IndexOf(")", "(", ")", px) - px))

                        Dim aaux As String()
                        aaux = aux2.Split(",", "(", ")")

                        For i As Integer = 0 To aaux.Length - 1
                            Dim ml As New pccGeoLineString
                            ml = GeometryFromWKT("linestring" & Trim(aaux(i)))
                            If ml IsNot Nothing Then CType(geo, pccGeoMultiLineString).AddLineString(ml)
                        Next

                        Return geo

                    Case "multipolygon"

                        geo = New pccGeoMultiPolygon

                        Dim aux1 As New pccString4.pccString4(wkt)
                        Dim px As Integer = wkt.IndexOf("(") + 1
                        Dim aux2 As New pccString4.pccString4(wkt.Substring(px, aux1.IndexOf(")", "(", ")", px) - px))

                        Dim aaux As String()
                        aaux = aux2.Split(",", "(", ")")

                        For i As Integer = 0 To aaux.Length - 1
                            Dim pl As New pccGeoPolygon
                            pl = GeometryFromWKT("polygon" & Trim(aaux(i)))
                            If pl IsNot Nothing Then CType(geo, pccGeoMultiPolygon).AddPolygon(pl)
                        Next

                        Return geo

                    Case "geometrycollection"

                        geo = New pccGeoCollection

                        Dim aux1 As New pccString4.pccString4(wkt)
                        Dim px As Integer = wkt.IndexOf("(") + 1
                        Dim aux2 As New pccString4.pccString4(wkt.Substring(px, aux1.IndexOf(")", "(", ")", px) - px))

                        Dim aaux As String()
                        aaux = aux2.Split(",", "(", ")")

                        For i As Integer = 0 To aaux.Length - 1
                            Dim ge As Object
                            ge = GeometryFromWKT(Trim(aaux(i)))
                            If ge IsNot Nothing Then CType(geo, pccGeoCollection).AddGeometry(ge)
                        Next

                        Return geo

                    Case Else

                        Return Nothing

                End Select

            Catch ex As Exception

                Return Nothing

            End Try
        End SyncLock


    End Function

    Public Function GeometryFromWKT(ByVal geo_id As String, ByVal wkt As String) As Object
        SyncLock _locker
            wkt = wkt.Trim.ToLower

            Dim geo As Object
            Dim p As Integer = 0
            Dim coords As String = ""
            Dim acoords() As String
            Dim points As String = ""
            Dim apoints() As String
            Dim separadordecimal As String = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator.ToString
            Dim separadormilhares As String

            separadormilhares = "."

            If separadordecimal = "," Then separadormilhares = "."
            If separadordecimal = "." Then separadormilhares = ","

            Try

                Select Case GeometryString(wkt)

                    Case "point"

                        Dim px As Integer = wkt.IndexOf("(") + 1
                        coords = wkt.Substring(px, wkt.IndexOf(")", px) - px)
                        acoords = coords.Trim.Split(" ")


                        If acoords.Length = 2 Then
                            geo = New pccGeoPoint(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)))

                        ElseIf acoords.Length = 3 Then
                            geo = New pccGeoPoint(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)), CDbl(acoords(2).Replace(separadormilhares, separadordecimal)))

                        Else
                            geo = Nothing
                        End If
                        If (geo IsNot Nothing) Then geo.myID = geo_id
                        Return geo

                    Case "linestring"

                        geo = New pccGeoLineString()
                        Dim px As Integer = wkt.IndexOf("(") + 1
                        points = wkt.Substring(px, wkt.IndexOf(")", px) - px)

                        apoints = points.Split(",")

                        For i As Integer = 0 To apoints.Length - 1

                            acoords = apoints(i).Trim.Split(" ")
                            Dim auxgeo As pccGeoPoint

                            If acoords.Length = 2 Then
                                auxgeo = New pccGeoPoint(geo_id, "", CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)))
                            ElseIf acoords.Length = 3 Then
                                auxgeo = New pccGeoPoint(geo_id, "", CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)), CDbl(acoords(2).Replace(separadormilhares, separadordecimal)))
                            Else
                                auxgeo = Nothing
                            End If

                            If auxgeo IsNot Nothing Then
                                CType(geo, pccGeoLineString).AddVertice(auxgeo)
                            End If

                        Next
                        If (geo IsNot Nothing) Then geo.myID = geo_id
                        Return geo

                    Case "polygon"

                        geo = New pccGeoPolygon
                        Dim outter As New pccGeoPolygon
                        Dim inner As New pccGeoPolygon
                        Dim AuxPol() As pccGeoPolygon = Nothing
                        Dim kAuxPol As Integer = 0

                        Dim aux1 As New pccString4.pccString4(wkt)
                        Dim px As Integer = wkt.IndexOf("(")
                        Dim aux2 As New pccString4.pccString4(wkt.Substring(px + 1, aux1.IndexOf(")", "(", ")", px + 1) - px - 1))

                        Dim aaux As String()
                        aaux = aux2.Split(",", "(", ")")

                        Dim i As Integer

                        For i = 0 To aaux.Length - 1

                            'px = aaux(i).IndexOf("(") + 1
                            'points = aaux(i).Substring(px, aaux(i).IndexOf(")", px) - px)
                            aaux(i) = aaux(i).Substring(1, aaux(i).Length - 2)


                            apoints = aaux(i).Split(",")
                            ReDim Preserve AuxPol(kAuxPol)
                            AuxPol(kAuxPol) = New pccGeoPolygon
                            kAuxPol += 1

                            For j As Integer = 0 To apoints.Length - 1

                                acoords = apoints(j).Trim.Split(" ")
                                Dim auxgeo As pccGeoPoint

                                If acoords.Length = 2 Then
                                    auxgeo = New pccGeoPoint(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)))
                                ElseIf acoords.Length = 3 Then
                                    auxgeo = New pccGeoPoint(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)), CDbl(acoords(2).Replace(separadormilhares, separadordecimal)))
                                Else
                                    auxgeo = Nothing
                                End If

                                If auxgeo IsNot Nothing Then
                                    AuxPol(kAuxPol - 1).AddVertice(auxgeo)
                                End If

                            Next

                        Next

                        Dim iPolMaior As Integer = -1
                        Dim MaxArea As Double = 0

                        For w As Integer = 0 To AuxPol.GetLength(0) - 1
                            If Math.Abs(AuxPol(w).Area) > MaxArea Then
                                MaxArea = Math.Abs(AuxPol(w).Area)
                                iPolMaior = w
                            End If
                        Next

                        If AuxPol(iPolMaior).Area > 0 Then
                            Dim aux As New List(Of pccGeoPoint)
                            For i = AuxPol(iPolMaior).Vertices.Count - 1 To 0 Step -1
                                aux.Add(AuxPol(iPolMaior).Vertices(i))
                            Next
                            'For Each v As pccGeoPoint In Invert(AuxPol(iPolMaior).Vertices)
                            For Each v As pccGeoPoint In aux
                                CType(geo, pccGeoPolygon).AddVertice(v)
                            Next
                        Else
                            For Each v As pccGeoPoint In AuxPol(iPolMaior).Vertices
                                CType(geo, pccGeoPolygon).AddVertice(v)
                            Next
                        End If

                        Dim xx As New pccGeoPolygon

                        xx = geo

                        If (xx.Centroid.X > 0) Then
                            Dim xyz As Integer = 0
                            xyz += 1
                        End If

                        ' TODO: rever a questão dos inners não trata inners
                        For j As Integer = 0 To AuxPol.GetLength(0) - 1
                            If j <> iPolMaior Then

                                If AuxPol(j).Area < -10 Then
                                    Dim aux As New List(Of pccGeoPoint)
                                    For i = AuxPol(j).Vertices.Count - 1 To 0 Step -1
                                        aux.Add(AuxPol(iPolMaior).Vertices(i))
                                    Next

                                    Dim pp As New pccGeoPolygon
                                    'For Each v As pccGeoPoint In Invert(AuxPol(j).Vertices)
                                    For Each v As pccGeoPoint In aux
                                        CType(pp, pccGeoPolygon).AddVertice(v)
                                    Next
                                    CType(geo, pccGeoPolygon).AddInnerPolygon(pp)
                                Else
                                    If AuxPol(j).Area >= 10 Then
                                        CType(geo, pccGeoPolygon).AddInnerPolygon(AuxPol(j))
                                    End If
                                End If

                            End If

                        Next
                        If (geo IsNot Nothing) Then geo.myID = geo_id
                        Return geo

                    Case "multipoint"

                        geo = New pccGeoMultiPoint()
                        Dim px As Integer = wkt.IndexOf("(") + 1

                        points = wkt.Substring(px, wkt.IndexOf(")", px) - px)
                        apoints = points.Split(",")

                        For i As Integer = 0 To apoints.Length - 1
                            acoords = apoints(i).Trim.Split(" ")
                            Dim auxgeo As pccGeoPoint

                            If acoords.Length = 2 Then
                                auxgeo = New pccGeoPoint(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)))
                            ElseIf acoords.Length = 3 Then
                                auxgeo = New pccGeoPoint(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)), CDbl(acoords(2).Replace(separadormilhares, separadordecimal)))
                            Else
                                auxgeo = Nothing
                            End If

                            If auxgeo IsNot Nothing Then
                                CType(geo, pccGeoMultiPoint).AddPoint(auxgeo)
                            End If

                        Next
                        If (geo IsNot Nothing) Then geo.myID = geo_id
                        Return geo

                    Case "multilinestring"

                        geo = New pccGeoMultiLineString

                        Dim aux1 As New pccString4.pccString4(wkt)
                        Dim px As Integer = wkt.IndexOf("(") + 1
                        Dim aux2 As New pccString4.pccString4(wkt.Substring(px, aux1.IndexOf(")", "(", ")", px) - px))

                        Dim aaux As String()
                        aaux = aux2.Split(",", "(", ")")

                        For i As Integer = 0 To aaux.Length - 1
                            Dim ml As New pccGeoLineString
                            ml = GeometryFromWKT("linestring" & Trim(aaux(i)))
                            If ml IsNot Nothing Then CType(geo, pccGeoMultiLineString).AddLineString(ml)
                        Next
                        If (geo IsNot Nothing) Then geo.myID = geo_id
                        Return geo

                    Case "multipolygon"

                        geo = New pccGeoMultiPolygon

                        Dim aux1 As New pccString4.pccString4(wkt)
                        Dim px As Integer = wkt.IndexOf("(") + 1
                        Dim aux2 As New pccString4.pccString4(wkt.Substring(px, aux1.IndexOf(")", "(", ")", px) - px))

                        Dim aaux As String()
                        aaux = aux2.Split(",", "(", ")")

                        For i As Integer = 0 To aaux.Length - 1
                            Dim pl As New pccGeoPolygon
                            pl = GeometryFromWKT("polygon" & Trim(aaux(i)))
                            If pl IsNot Nothing Then CType(geo, pccGeoMultiPolygon).AddPolygon(pl)
                        Next
                        If (geo IsNot Nothing) Then geo.myID = geo_id
                        Return geo

                    Case "geometrycollection"

                        geo = New pccGeoCollection

                        Dim aux1 As New pccString4.pccString4(wkt)
                        Dim px As Integer = wkt.IndexOf("(") + 1
                        Dim aux2 As New pccString4.pccString4(wkt.Substring(px, aux1.IndexOf(")", "(", ")", px) - px))

                        Dim aaux As String()
                        aaux = aux2.Split(",", "(", ")")

                        For i As Integer = 0 To aaux.Length - 1
                            Dim ge As Object
                            ge = GeometryFromWKT(Trim(aaux(i)))
                            If ge IsNot Nothing Then CType(geo, pccGeoCollection).AddGeometry(ge)
                        Next
                        If (geo IsNot Nothing) Then geo.myID = geo_id
                        Return geo

                    Case Else

                        Return Nothing

                End Select

            Catch ex As Exception

                Return Nothing

            End Try
        End SyncLock

    End Function

    Public Function GeometryDxfFromWKT(ByVal wkt As String) As Object
        SyncLock _locker
            wkt = wkt.Trim.ToLower

            Dim geo As Object
            Dim p As Integer = 0
            Dim coords As String = ""
            Dim acoords() As String
            Dim points As String = ""
            Dim apoints() As String
            Dim separadordecimal As String = System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator.ToString
            Dim separadormilhares As String

            separadormilhares = "."

            If separadordecimal = "," Then separadormilhares = "."
            If separadordecimal = "." Then separadormilhares = ","

            Try

                Select Case GeometryString(wkt)

                    Case "point"

                        Dim px As Integer = wkt.IndexOf("(") + 1
                        coords = wkt.Substring(px, wkt.IndexOf(")", px) - px)
                        acoords = coords.Trim.Split(" ")


                        If acoords.Length = 2 Then
                            geo = New pccDXF4.Entities.Point(New Vector3d(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)), 0))

                        ElseIf acoords.Length = 3 Then
                            geo = New pccDXF4.Entities.Point(New Vector3d(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)), CDbl(acoords(2).Replace(separadormilhares, separadordecimal))))

                        Else
                            geo = Nothing
                        End If

                        Return geo

                    Case "linestring"

                        geo = New pccDXF4.Entities.Polyline()
                        Dim px As Integer = wkt.IndexOf("(") + 1
                        points = wkt.Substring(px, wkt.IndexOf(")", px) - px)

                        apoints = points.Split(",")

                        For i As Integer = 0 To apoints.Length - 1

                            acoords = apoints(i).Trim.Split(" ")
                            Dim auxgeo As pccDXF4.Entities.PolylineVertex

                            If acoords.Length = 2 Then
                                auxgeo = New pccDXF4.Entities.PolylineVertex(New Vector2f(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal))))
                            ElseIf acoords.Length = 3 Then
                                auxgeo = New pccDXF4.Entities.PolylineVertex(New Vector2f(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal))))
                            Else
                                auxgeo = Nothing
                            End If

                            If auxgeo IsNot Nothing Then
                                CType(geo, pccDXF4.Entities.Polyline).Vertexes.Add(auxgeo)
                            End If

                        Next

                        Return geo

                    Case "polygon"

                        geo = New pccDXF4.Entities.Polyline()
                        Dim outter As New pccDXF4.Entities.Polyline()
                        Dim inner As New pccDXF4.Entities.Polyline()
                        Dim AuxPol() As pccDXF4.Entities.Polyline = Nothing
                        Dim kAuxPol As Integer = 0

                        Dim aux1 As New pccString4.pccString4(wkt)
                        Dim px As Integer = wkt.IndexOf("(")
                        Dim aux2 As New pccString4.pccString4(wkt.Substring(px + 1, aux1.IndexOf(")", "(", ")", px + 1) - px - 1))

                        Dim aaux As String()
                        aaux = aux2.Split(",", "(", ")")

                        Dim i As Integer

                        For i = 0 To aaux.Length - 1

                            'px = aaux(i).IndexOf("(") + 1
                            'points = aaux(i).Substring(px, aaux(i).IndexOf(")", px) - px)
                            aaux(i) = aaux(i).Substring(1, aaux(i).Length - 2)


                            apoints = aaux(i).Split(",")
                            ReDim Preserve AuxPol(kAuxPol)
                            AuxPol(kAuxPol) = New pccDXF4.Entities.Polyline
                            kAuxPol += 1

                            For j As Integer = 0 To apoints.Length - 1

                                acoords = apoints(j).Trim.Split(" ")
                                Dim auxgeo As pccDXF4.Entities.PolylineVertex

                                If acoords.Length = 2 Then
                                    auxgeo = New pccDXF4.Entities.PolylineVertex(New Vector2f(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal))))
                                ElseIf acoords.Length = 3 Then
                                    auxgeo = New pccDXF4.Entities.PolylineVertex(New Vector2f(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal))))
                                Else
                                    auxgeo = Nothing
                                End If

                                If auxgeo IsNot Nothing Then
                                    AuxPol(kAuxPol - 1).Vertexes.Add(auxgeo)
                                End If

                            Next

                        Next


                        Return AuxPol

                    Case "multipoint"
                        Dim AuxPol() As pccDXF4.Entities.Point = Nothing
                        Dim kAuxPol As Integer = 0
                        geo = New pccGeoMultiPoint()
                        Dim px As Integer = wkt.IndexOf("(") + 1

                        points = wkt.Substring(px, wkt.IndexOf(")", px) - px)
                        apoints = points.Split(",")

                        For i As Integer = 0 To apoints.Length - 1
                            acoords = apoints(i).Trim.Split(" ")
                            Dim auxgeo As pccDXF4.Entities.Point

                            If acoords.Length = 2 Then
                                auxgeo = New pccDXF4.Entities.Point(New Vector3d(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)), 0))
                            ElseIf acoords.Length = 3 Then
                                auxgeo = New pccDXF4.Entities.Point(New Vector3d(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)), CDbl(acoords(2).Replace(separadormilhares, separadordecimal))))
                            Else
                                auxgeo = Nothing
                            End If

                            If auxgeo IsNot Nothing Then
                                ReDim Preserve AuxPol(kAuxPol)
                                AuxPol(kAuxPol) = New pccDXF4.Entities.Point
                                AuxPol(kAuxPol) = auxgeo
                                kAuxPol += 1
                            End If
                        Next

                        Return AuxPol

                    Case "multilinestring"
                        Dim AuxPol() As pccDXF4.Entities.Polyline = Nothing
                        Dim kAuxPol As Integer = 0
                        geo = New pccGeoMultiLineString

                        Dim aux1 As New pccString4.pccString4(wkt)
                        Dim px As Integer = wkt.IndexOf("(") + 1
                        Dim aux2 As New pccString4.pccString4(wkt.Substring(px, aux1.IndexOf(")", "(", ")", px) - px))

                        Dim aaux As String()
                        aaux = aux2.Split(",", "(", ")")

                        For i As Integer = 0 To aaux.Length - 1
                            Dim ml(1) As pccDXF4.Entities.Polyline

                            ml(0) = GeometryDxfFromWKT("linestring" & Trim(aaux(i)))
                            If ml IsNot Nothing Then
                                For j As Integer = 0 To ml.Length - 1
                                    ReDim Preserve AuxPol(kAuxPol)
                                    AuxPol(kAuxPol) = New pccDXF4.Entities.Polyline
                                    AuxPol(kAuxPol) = ml(j)
                                    kAuxPol += 1
                                Next
                            End If
                        Next

                        Return AuxPol

                    Case "multipolygon"
                        Dim AuxPol() As pccDXF4.Entities.Polyline = Nothing
                        Dim kAuxPol As Integer = 0
                        geo = New pccDXF4.Entities.Polyline()

                        Dim aux1 As New pccString4.pccString4(wkt)
                        Dim px As Integer = wkt.IndexOf("(") + 1
                        Dim aux2 As New pccString4.pccString4(wkt.Substring(px, aux1.IndexOf(")", "(", ")", px) - px))

                        Dim aaux As String()
                        aaux = aux2.Split(",", "(", ")")

                        For i As Integer = 0 To aaux.Length - 1
                            Dim pl() As pccDXF4.Entities.Polyline
                            pl = GeometryDxfFromWKT("polygon" & Trim(aaux(i)))

                            If pl IsNot Nothing Then
                                For j As Integer = 0 To pl.Length - 1
                                    ReDim Preserve AuxPol(kAuxPol)
                                    AuxPol(kAuxPol) = New pccDXF4.Entities.Polyline
                                    AuxPol(kAuxPol) = pl(j)
                                    kAuxPol += 1
                                Next
                            End If
                        Next

                        Return AuxPol

                    Case "geometrycollection"
                        Dim AuxPol() As pccDXF4.Entities.IEntityObject = Nothing
                        Dim kAuxPol As Integer = 0
                        geo = New pccGeoCollection

                        Dim aux1 As New pccString4.pccString4(wkt)
                        Dim px As Integer = wkt.IndexOf("(") + 1
                        Dim aux2 As New pccString4.pccString4(wkt.Substring(px, aux1.IndexOf(")", "(", ")", px) - px))

                        Dim aaux As String()
                        aaux = aux2.Split(",", "(", ")")

                        For i As Integer = 0 To aaux.Length - 1
                            Dim ge() As Object
                            ge = GeometryDxfFromWKT(Trim(aaux(i)))
                            If ge IsNot Nothing Then
                                For j As Integer = 0 To ge.Length - 1
                                    ReDim Preserve AuxPol(kAuxPol)
                                    AuxPol(kAuxPol) = ge(j)
                                    kAuxPol += 1
                                Next j
                            End If
                        Next

                        Return geo

                    Case Else

                        Return Nothing

                End Select

            Catch ex As Exception

                Return Nothing

            End Try
        End SyncLock


    End Function

    Public Function Invert(ByRef points As pccGeoPoint()) As pccGeoPoint()
        SyncLock _locker
            Dim ix As Integer = points.GetLength(0) - 1

            Dim ret(ix) As pccGeoPoint

            For i As Integer = 0 To ix
                ret(i) = points(ix - i)
            Next

            Return ret
        End SyncLock


    End Function

    Public Function Invert(ByRef points As List(Of pccGeoPoint)) As pccGeoPoint()
        SyncLock _locker
            Dim ix As Integer = points.Count - 1

            Dim ret(ix) As pccGeoPoint

            For i As Integer = 0 To ix
                ret(i) = points(ix - i)
            Next

            Return ret
        End SyncLock


    End Function

    Public Function Distance(ByRef p1 As pccGeoPoint, ByRef p2 As pccGeoPoint) As Double
        SyncLock _locker
            Dim d As Double = 0

            If p1.Is3d And p2.Is3d Then
                d = Math.Sqrt((p1.X - p2.X) ^ 2 + (p1.Y - p2.Y) ^ 2 + (p1.Z - p2.Z) ^ 2)
            Else
                d = Math.Sqrt((p1.X - p2.X) ^ 2 + (p1.Y - p2.Y) ^ 2)
            End If

            Return d
        End SyncLock


    End Function

    Public Function Simplify(ByRef points As pccGeoPoint(), ByVal tolerance As Double) As pccGeoPoint()
        SyncLock _locker
            Dim closed As Boolean = points(points.GetLowerBound(0)).IsEqual(points(points.GetUpperBound(0)))

            Dim ix As Integer = 0
            Dim len As Integer = points.GetLength(0)
            Dim ret(ix) As pccGeoPoint

            ret(ix) = points(0)

            For i As Integer = 1 To len - 2
                If Distance(ret(ix), points(i)) >= tolerance Then
                    ix += 1
                    ReDim Preserve ret(ix)
                    ret(ix) = points(i)
                End If
            Next

            ' TEST: testar simplificação de polígonos fechados e polilinhas (abertas)

            ix += 1

            ReDim Preserve ret(ix)

            If closed Then
                ret(ix) = ret(0)
            Else
                ret(ix) = points(len - 1)
            End If

            Return ret
        End SyncLock


    End Function

    Public Function Simplify(ByRef points As List(Of pccGeoPoint), ByVal tolerance As Double) As pccGeoPoint()
        SyncLock _locker
            Dim closed As Boolean = points(0).IsEqual(points(points.Count - 1))

            Dim ix As Integer = 0
            Dim len As Integer = points.Count
            Dim ret(ix) As pccGeoPoint

            ret(ix) = points(0)

            For i As Integer = 1 To len - 2
                If Distance(ret(ix), points(i)) >= tolerance Then
                    ix += 1
                    ReDim Preserve ret(ix)
                    ret(ix) = points(i)
                End If
            Next

            ' TEST: testar simplificação de polígonos fechados e polilinhas (abertas)

            ix += 1
            ReDim Preserve ret(ix)

            If closed Then
                ret(ix) = ret(0)
            Else
                ret(ix) = points(len - 1)
            End If

            Return ret
        End SyncLock


    End Function

    ' retorna o tipo de geometria do WKT: POINT, LINESTRING, POLYGON, ETC
    Private Function GeometryString(ByVal wkt As String) As String
        SyncLock _locker
            Dim p As Integer
            p = wkt.IndexOf("(")

            If p > 0 Then
                Return wkt.Substring(0, p).Trim.ToLower
            Else
                Return ""
            End If
        End SyncLock


    End Function

    Public Sub New()

    End Sub
End Class

<DataContract()>
<KnownType(GetType(pccGeoPoint))>
<KnownType(GetType(pccGeoPolygon))>
<KnownType(GetType(pccGeoRectangle))>
<KnownType(GetType(pccGeoLineString))>
<KnownType(GetType(pccGeoMultiPoint))>
<KnownType(GetType(pccGeoMultiPolygon))>
<KnownType(GetType(pccGeoMultiLineString))>
<KnownType(GetType(pccGeoCollection))>
<Serializable()>
<XmlInclude(GetType(pccGeoPoint))>
<XmlInclude(GetType(pccGeoPolygon))>
<XmlInclude(GetType(pccGeoRectangle))>
<XmlInclude(GetType(pccGeoLineString))>
<XmlInclude(GetType(pccGeoMultiPoint))>
<XmlInclude(GetType(pccGeoMultiPolygon))>
<XmlInclude(GetType(pccGeoMultiLineString))>
<XmlInclude(GetType(pccGeoCollection))>
Public MustInherit Class pccGeoGeometry
    <DataMember(Name:="Id")>
    Protected _id As String
    <DataMember(Name:="Nome")>
    Protected _nome As String
    Protected _catalogo As pccGeoCatalog 'friend 
    Protected _3d As Boolean
    <DataMember(Name:="Length")>
    Protected _length As Double
    <DataMember(Name:="Area")>
    Protected _area As Double
    Protected _convexhul As pccGeoPolygon
    <DataMember(Name:="MBR")>
    Protected _mbr As pccGeoPolygon
    <DataMember(Name:="Centroid")>
    Protected _centroid As pccGeoCentroid
    Protected _length_calc As Boolean 'friend 
    Protected _area_calc As Boolean 'friend 
    Protected _convexhul_calc As Boolean 'friend 
    Protected _centroid_calc As Boolean 'friend 
    Protected _mbr_calc As Boolean 'friend 
    <DataMember(Name:="WKT")>
    Protected _wkt As String 'friend
    Protected _wktfromdb As String 'friend
    Protected _myID As String = ""
    Protected _isNew As Boolean
    Protected _description As String = ""
    Protected _type As Integer
    Protected _srid As Integer

    Private Shared ReadOnly _locker As New Object()

    Public MustOverride Function IsMulti() As Boolean

    Public MustOverride Function Definition() As String

    Public Sub New()
        Me.New("", "")
        SyncLock _locker
            _length_calc = False
            _area_calc = False
            _convexhul_calc = False
            _mbr_calc = False
            _centroid_calc = False
            _srid = 0
        End SyncLock

    End Sub

    Public Sub New(ByVal id As String, ByVal nome As String)
        SyncLock _locker
            If id = "" And nome = "" Then
                Me._catalogo = Nothing
                Me._id = Nothing
            Else
                Me._catalogo = New pccGeoCatalog(nome)
                Me._id = _catalogo.Id
            End If
            _3d = False
        End SyncLock

    End Sub

    Public Property Catalogo() As pccGeoCatalog
        Get
            SyncLock _locker
                Return _catalogo
            End SyncLock

        End Get
        Set(ByVal value As pccGeoCatalog)
            SyncLock _locker
                _id = _catalogo.Id
                _nome = _catalogo.Nome
                _catalogo = value
            End SyncLock

        End Set
    End Property
    <DataMember()>
    Public Property Type() As Integer
        Get
            SyncLock _locker
                Return _type
            End SyncLock

        End Get
        Set(ByVal value As Integer)
            SyncLock _locker
                _type = value
            End SyncLock

        End Set
    End Property
    <DataMember()>
    Public Property myID() As String
        Get
            SyncLock _locker
                Return _myID
            End SyncLock

        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _myID = value
            End SyncLock

        End Set
    End Property
    <DataMember()>
    Public Property description() As String
        Get
            SyncLock _locker
                Return _description
            End SyncLock

        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _description = value
            End SyncLock

        End Set
    End Property

    Public Property IsNew() As Boolean
        Get
            SyncLock _locker
                Return _isNew
            End SyncLock

        End Get
        Set(ByVal value As Boolean)
            SyncLock _locker
                _isNew = value
            End SyncLock

        End Set
    End Property

    Public ReadOnly Property Id() As String
        Get
            SyncLock _locker
                Return _id
            End SyncLock

        End Get

    End Property

    Public Property Nome() As String
        Get
            SyncLock _locker
                Return _nome
            End SyncLock

        End Get

        Set(value As String)
            SyncLock _locker
                _nome = value
            End SyncLock

        End Set

    End Property

    Public Property WKTfromDB() As String
        Get
            SyncLock _locker
                Return _wktfromdb
            End SyncLock

        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _wktfromdb = value
            End SyncLock

        End Set
    End Property
    <DataMember()>
    Public Property SRID() As String
        Get
            SyncLock _locker
                Return _srid
            End SyncLock

        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _srid = value
            End SyncLock

        End Set
    End Property

    Public Shared Function Compare(ByVal p1 As pccGeoGeometry, ByVal p2 As pccGeoGeometry) As Integer
        SyncLock _locker
            ' retorna:
            '    -1 se (p1 < p2)
            '     0 se (p1 = p2)
            '     1 se (p1 > p2)

            ' e para este efeito, compara as coordenadas por ordem crescente de X, Y e Z

            If p1 Is Nothing Then
                If p2 Is Nothing Then
                    Return 0
                Else
                    Return -1
                End If
            Else
                If p2 Is Nothing Then
                    Return 1
                Else
                    If p1.Definition > p2.Definition Then
                        Return 1
                    ElseIf p1.Definition < p2.Definition Then
                        Return -1
                    Else
                        Return 0
                    End If
                End If
            End If
        End SyncLock


    End Function

    Public Overridable Function GetBuffer(ByVal conn As pccDB4.Connection, ByVal dist As Double) As String

        SyncLock _locker
            Dim com As pccDB4.Command
            Dim iReader As IDataReader = Nothing

            Dim distAux As String = dist.ToString().Replace(",", ".")

            Dim res As String = ""

            Try

                com = conn.CreateCommand

                com.CommandType = CommandType.Text
                com.CommandText = "select " + com.GetFunction(pccDB4.ProviderFunctions.GeoToWKT, com.GetFunction(pccDB4.ProviderFunctions.GetBuffer, "'" + Me.WKT + "'", "", distAux)) + ";"

                iReader = com.ExecuteReader()

                If iReader.Read Then

                    res = iReader.GetString(0)
                Else

                    res = Nothing
                End If

                If iReader IsNot Nothing Then
                    If Not iReader.IsClosed Then iReader.Close()
                End If

            Catch ex As Exception

                If iReader IsNot Nothing Then
                    If Not iReader.IsClosed Then iReader.Close()
                End If
                res = Nothing

            Finally

                If iReader IsNot Nothing Then
                    If Not iReader.IsClosed Then iReader.Close()
                End If

            End Try
            Return res
        End SyncLock

    End Function

    Public Function Intersects(ByVal conn As pccDB4.Connection, ByVal wktToComp As String) As Boolean
        SyncLock _locker
            Dim com As pccDB4.Command
            Dim iReader As IDataReader = Nothing
            Dim res As Boolean = False

            Try

                com = conn.CreateCommand

                com.CommandType = CommandType.Text
                com.CommandText = "select " + com.GetFunction(pccDB4.ProviderFunctions.IntersectsPrefix) + "'" + Me.WKT + "','" + wktToComp + "'" + com.GetFunction(pccDB4.ProviderFunctions.IntersectsSufix) + ";"

                iReader = com.ExecuteReader()

                If iReader.Read Then

                    res = iReader.GetBoolean(0)
                Else

                    res = False
                End If

            Catch ex As Exception

                If iReader IsNot Nothing Then
                    If Not iReader.IsClosed Then iReader.Close()
                End If

                res = False

            Finally

                If iReader IsNot Nothing Then
                    If Not iReader.IsClosed Then iReader.Close()
                End If

            End Try
            Return res
        End SyncLock

    End Function

    Public Function getGeometriesIDsByElement(ByRef conn As pccDB4.Connection, ByVal element_id As String, ByVal storage_table As String) As String
        SyncLock _locker
            Dim com As pccDB4.Command
            Dim par As IDataParameter
            Dim sIDs As String = ""
            Dim iReader As IDataReader = Nothing

            Try

                com = conn.CreateCommand

                ' obter geometrias desta entidade

                com.CommandType = CommandType.Text
                com.CommandText = "select " + com.GetFunction(pccDB4.ProviderFunctions.Convert, "geo_id", "eg", "char(36)") + " from " + storage_table + "_Elements_Geo eg where element_id=?"

                par = com.CreateParameter(DbType.String, element_id)
                com.AddParameter(par)

                iReader = com.ExecuteReader()

                While iReader.Read
                    If sIDs.Length > 0 Then sIDs += "||"
                    sIDs += iReader.GetString(0)
                End While

                If iReader IsNot Nothing Then
                    If Not iReader.IsClosed Then iReader.Close()
                End If

            Catch ex As Exception

                If iReader IsNot Nothing Then
                    If Not iReader.IsClosed Then iReader.Close()
                End If

                Return False

            End Try

            Return sIDs
        End SyncLock


    End Function

    Public Overridable Function Delete(ByRef conn As pccDB4.Connection, ByVal trans As IDbTransaction, ByVal entidade_id As String, ByVal storage_table As String) As Boolean
        SyncLock _locker
            Dim com As pccDB4.Command
            Dim par As IDataParameter

            Try

                ' obter geometrias desta entidade
                Dim sIDs As String = ""

                com = conn.CreateCommand
                If trans IsNot Nothing Then com.Transaction = trans

                com.CommandType = CommandType.Text
                com.CommandText = "delete from " + storage_table + "_Elements_Geo where geo_id=?"

                par = com.CreateParameter(DbType.String, Me.myID)
                com.AddParameter(par)

                com.ExecuteNonQuery()

                com = conn.CreateCommand
                If trans IsNot Nothing Then com.Transaction = trans

                com.CommandType = CommandType.Text
                com.CommandText = "delete from " + storage_table + "_Geo where rec_id=?"

                par = com.CreateParameter(DbType.String, Me.myID)
                com.AddParameter(par)

                com.ExecuteNonQuery()

                Return True

            Catch ex As Exception

                Return False

            End Try

            Return True
        End SyncLock


    End Function

    Public Function setGeomVisibility(ByRef conn As pccDB4.Connection, ByVal trans As IDbTransaction, ByVal id_geom As String, ByVal visibility As Integer, ByVal storage_table As String) As Boolean
        SyncLock _locker
            Dim com As pccDB4.Command
            Dim par As IDataParameter
            Try
                com = conn.CreateCommand
                If trans IsNot Nothing Then com.Transaction = trans

                com.CommandType = CommandType.Text
                com.CommandText = "update " + storage_table + "_Geo set visible=? where rec_id=?"

                par = com.CreateParameter(DbType.Int16, visibility)
                com.AddParameter(par)

                par = com.CreateParameter(DbType.String, id_geom)
                com.AddParameter(par)

                com.ExecuteNonQuery()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End SyncLock

    End Function

    Public Function setGeomSelected(ByRef conn As pccDB4.Connection, ByRef trans As IDbTransaction, ByVal id_geom As String, ByVal storage_table As String) As Boolean
        SyncLock _locker
            Dim com As pccDB4.Command
            Dim par As IDataParameter
            Try
                com = conn.CreateCommand
                If trans IsNot Nothing Then com.Transaction = trans

                com.CommandType = CommandType.Text
                com.CommandText = "update " + storage_table + "_Geo set selected=1 where rec_id=?"

                par = com.CreateParameter(DbType.String, id_geom)
                com.AddParameter(par)

                com.ExecuteNonQuery()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End SyncLock

    End Function

    Public Function setGeometriesVisibility(ByRef conn As pccDB4.Connection, ByVal trans As IDbTransaction, ByVal visibility As Integer, ByVal storage_table As String, ByVal user_id As String) As Boolean
        SyncLock _locker
            Dim com As pccDB4.Command
            Dim par As IDataParameter

            Try
                com = conn.CreateCommand
                If trans IsNot Nothing Then com.Transaction = trans

                com.CommandType = CommandType.Text
                If visibility = 1 Then
                    com.CommandText = "update " + storage_table + "_geo set visible = 1 where visible = 0 and rec_id IN(select geo_id from " + storage_table + "_elements_geo where element_id IN(select rec_id from " + storage_table + "_Elements where user_id=?))"
                Else
                    com.CommandText = "update " + storage_table + "_geo set visible = 0 where visible = 1 and rec_id IN(select geo_id from " + storage_table + "_elements_geo where element_id IN(select rec_id from " + storage_table + "_Elements where user_id=?))"
                End If

                par = com.CreateParameter(DbType.String, user_id)
                com.AddParameter(par)

                com.ExecuteNonQuery()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End SyncLock

    End Function

    Public Overridable Function Save(ByRef conn As pccDB4.Connection, ByVal trans As IDbTransaction, ByVal id_entidade As String, ByVal storage_table As String, ByVal isUpdate As Boolean) As Boolean
        SyncLock _locker
            Dim com As pccDB4.Command
            Dim geoid As String = Guid.NewGuid.ToString
            Dim par As IDataParameter

            Try
                com = conn.CreateCommand
                If trans IsNot Nothing Then com.Transaction = trans

                If (Not isUpdate) Then

                    com.CommandType = CommandType.Text
                    'iKey, rec_id, k, geom, catalogo, mtd_id
                    If storage_table = "prt" Or storage_table = "pretensaoobj" Then

                        com.CommandText = "insert into " + storage_table + "_Geo (rec_id, k," & com.GetFunction(ProviderFunctions.GeometryFieldName, "geom") & ",catalogo,mtd_id,texto,tipo,descricao) values(?,?," & com.GetFunction(ProviderFunctions.WKTToGeoPrefix) & com.GetFunction(ProviderFunctions.WKTToGeo, "?", "", "0") & com.GetFunction(ProviderFunctions.WKTToGeoSufix) & ",?,?,?,?,?)"
                    ElseIf storage_table = "regimp" Then
                        com.CommandText = "insert into " + storage_table + "_Geo (rec_id, k," & com.GetFunction(ProviderFunctions.GeometryFieldName, "geom") & ",catalogo,mtd_id,texto,tipo) values(?,?," & com.GetFunction(ProviderFunctions.WKTToGeoPrefix) & com.GetFunction(ProviderFunctions.WKTToGeo, "?", "", "0") & com.GetFunction(ProviderFunctions.WKTToGeoSufix) & ",?,?,?,?)"
                    ElseIf storage_table = "epl" Then
                        com.CommandText = "insert into " + storage_table + "_Geo (rec_id, k," & com.GetFunction(ProviderFunctions.GeometryFieldName, "geom") & ",catalogo,mtd_id,tipo) values(?,?," & com.GetFunction(ProviderFunctions.WKTToGeoPrefix) & com.GetFunction(ProviderFunctions.WKTToGeo, "?", "", "0") & com.GetFunction(ProviderFunctions.WKTToGeoSufix) & ",?,?,?)"
                    Else
                        com.CommandText = "insert into " + storage_table + "_Geo (rec_id, k," & com.GetFunction(ProviderFunctions.GeometryFieldName, "geom") & ",catalogo,mtd_id) values(?,?," & com.GetFunction(ProviderFunctions.WKTToGeoPrefix) & com.GetFunction(ProviderFunctions.WKTToGeo, "?", "", "0") & com.GetFunction(ProviderFunctions.WKTToGeoSufix) & ",?,?)"
                    End If

                    par = com.CreateParameter(DbType.String, geoid)
                    com.AddParameter(par)

                    par = com.CreateParameter(DbType.Decimal, 0)
                    com.AddParameter(par)

                    par = com.CreateParameter(DbType.String, Me.WKT)
                    com.AddParameter(par)

                    Dim sXMLCat As String = ""
                    Dim sMtdId As String = ""

                    If Me.Catalogo IsNot Nothing Then

                        If Me.Catalogo.Objecto IsNot Nothing Then

                            Dim srcustomer As New XmlSerializer(Me.Catalogo.Objecto.GetType)
                            Dim writestream As New MemoryStream()
                            srcustomer.Serialize(writestream, Me.Catalogo.Objecto)

                            Dim gu As New pccGeoUtils
                            sXMLCat = gu.GetStringFromMemoryStream(writestream)

                        End If

                        If Me.Catalogo.Metadata IsNot Nothing Then
                            sMtdId = Me.Catalogo.Metadata.GetId
                        End If

                    End If

                    par = com.CreateParameter(DbType.String, sXMLCat)
                    com.AddParameter(par)

                    par = com.CreateParameter(DbType.String, sMtdId)
                    com.AddParameter(par)

                    If storage_table = "epl" Then
                        par = com.CreateParameter(DbType.Int32, Me.Type)
                        com.AddParameter(par)
                    End If

                    If storage_table = "prt" Or storage_table = "regimp" Or storage_table = "pretensaoobj" Then
                        par = com.CreateParameter(DbType.String, Me.description)
                        com.AddParameter(par)
                        par = com.CreateParameter(DbType.Int32, Me.Type)
                        com.AddParameter(par)
                    End If

                    If storage_table = "prt" Or storage_table = "pretensaoobj" Then
                        If Me.Nome Is Nothing Then
                            par = com.CreateParameter(DbType.String, "")
                            com.AddParameter(par)
                        Else
                            par = com.CreateParameter(DbType.String, Me.Nome)
                            com.AddParameter(par)
                        End If
                    End If

                    com.ExecuteNonQuery()

                    com = conn.CreateCommand
                    If trans IsNot Nothing Then com.Transaction = trans

                    com.CommandType = CommandType.Text
                    com.CommandText = "insert into " + storage_table + "_Elements_Geo (geo_id, element_id) values(?,?)"

                    par = com.CreateParameter(DbType.String, geoid)
                    com.AddParameter(par)

                    par = com.CreateParameter(DbType.String, id_entidade)
                    com.AddParameter(par)

                    com.ExecuteNonQuery()

                Else
                    com.CommandType = CommandType.Text
                    'iKey, rec_id, k, geom, catalogo, mtd_id
                    If storage_table = "prt" Or storage_table = "pretensaoobj" Then
                        com.CommandText = "update " + storage_table + "_Geo SET geom=" & com.GetFunction(ProviderFunctions.WKTToGeoPrefix) & com.GetFunction(ProviderFunctions.WKTToGeo, "?", "", "0") & com.GetFunction(ProviderFunctions.WKTToGeoSufix) & ", descricao=? where rec_id=?"
                    Else
                        com.CommandText = "update " + storage_table + "_Geo SET geom=" & com.GetFunction(ProviderFunctions.WKTToGeoPrefix) & com.GetFunction(ProviderFunctions.WKTToGeo, "?", "", "0") & com.GetFunction(ProviderFunctions.WKTToGeoSufix) & " where rec_id=?"
                    End If
                    par = com.CreateParameter(DbType.String, Me.WKT)
                    com.AddParameter(par)

                    If storage_table = "prt" Or storage_table = "pretensaoobj" Then
                        If Me.Nome Is Nothing Then
                            par = com.CreateParameter(DbType.String, "")
                            com.AddParameter(par)
                        Else
                            par = com.CreateParameter(DbType.String, Me.Nome)
                            com.AddParameter(par)
                        End If

                    End If

                    par = com.CreateParameter(DbType.String, Me.myID)
                    com.AddParameter(par)

                    com.ExecuteNonQuery()
                End If

                Return True

            Catch ex As Exception

                Return False

            End Try
        End SyncLock



    End Function

    Function setGeomName(ByRef conn As pccDB4.Connection, trans As IDbTransaction, id As String, nome As String, ByVal storage_table As String) As Boolean
        SyncLock _locker
            Dim com As pccDB4.Command
            Dim par As IDataParameter

            Try
                com = conn.CreateCommand
                If trans IsNot Nothing Then com.Transaction = trans

                com.CommandType = CommandType.Text
                'iKey, rec_id, k, geom, catalogo, mtd_id
                com.CommandText = "update " + storage_table + "_Geo SET descricao=? where rec_id=?"

                par = com.CreateParameter(DbType.String, nome)
                com.AddParameter(par)

                par = com.CreateParameter(DbType.String, id)
                com.AddParameter(par)

                com.ExecuteNonQuery()

                'iKey, rec_id, k, geom, catalogo, mtd_id
                com.CommandText = "update pretensaoobj_view_table SET descricao=? where geo_id=?"

                par = com.CreateParameter(DbType.String, nome)
                com.AddParameter(par)

                par = com.CreateParameter(DbType.String, id)
                com.AddParameter(par)

                com.ExecuteNonQuery()

                Return True

            Catch ex As Exception

                Return False

            End Try
        End SyncLock

    End Function

    Public MustOverride ReadOnly Property GetGeometryType() As pccGeoGeometryType

    Public MustOverride ReadOnly Property WKT() As String
    Public MustOverride Function AddGeometry(ByRef g As pccGeoGeometry) As Boolean
    Public MustOverride ReadOnly Property Is3D() As Boolean

    Public MustOverride ReadOnly Property Length() As Double

    Public MustOverride ReadOnly Property Area() As Double
    <XmlIgnore(), SoapIgnore()>
    Public MustOverride ReadOnly Property ConvexHul() As pccGeoPolygon
    '<XmlIgnore(), SoapIgnore()> _
    Public MustOverride ReadOnly Property Centroid() As pccGeoCentroid

    Public MustOverride ReadOnly Property MBR() As pccGeoRectangle
    Public MustOverride ReadOnly Property IsEqual(ByVal geo As pccGeoGeometry) As Boolean
    <XmlIgnore(), SoapIgnore()>
    Public MustOverride Property GetBasicGeometries() As pccGeoGeometry()

    ' Retorna um objecto de tipo Multi respectico:
    ' Ex: um pccGeoPoint.GetMulti() retorna um new pccGeoMultiPoint
    Public MustOverride Function GetMulti() As pccGeoGeometry

End Class
<DataContract()>
<KnownType(GetType(pccGeoPoint))>
<KnownType(GetType(pccGeoPolygon))>
<KnownType(GetType(pccGeoRectangle))>
<KnownType(GetType(pccGeoLineString))>
<KnownType(GetType(pccGeoMultiPoint))>
<KnownType(GetType(pccGeoMultiPolygon))>
<KnownType(GetType(pccGeoMultiLineString))>
<KnownType(GetType(pccGeoCollection))>
<Serializable()>
<XmlInclude(GetType(pccGeoPoint))>
<XmlInclude(GetType(pccGeoPolygon))>
<XmlInclude(GetType(pccGeoLineString))>
<XmlInclude(GetType(pccGeoRectangle))>
<XmlInclude(GetType(pccGeoMultiPoint))>
<XmlInclude(GetType(pccGeoMultiPolygon))>
<XmlInclude(GetType(pccGeoMultiLineString))>
<XmlInclude(GetType(pccGeoCollection))>
Public Class pccGeoCatalog

    Private _nome As String
    Private _id As String
    Private _objecto As Object
    Private _metadata As mtdMetadata

    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

        Me.New("")

    End Sub

    Public Sub New(ByVal p_nome As String)
        SyncLock _locker
            _id = Guid.NewGuid.ToString
            _nome = p_nome
        End SyncLock

    End Sub

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

    Public Property Objecto() As Object
        Get
            SyncLock _locker
                Return _objecto
            End SyncLock
        End Get
        Set(ByVal value As Object)
            SyncLock _locker
                _objecto = value
            End SyncLock
        End Set
    End Property

    Public Property Metadata As mtdMetadata
        Get
            SyncLock _locker
                Return _metadata
            End SyncLock
        End Get
        Set(ByVal value As mtdMetadata)
            SyncLock _locker
                _metadata = value
            End SyncLock
        End Set
    End Property

    ' TODO: getbyid do catalogo
    Public Function GetById(ByRef conn As pccDB4.Connection, ByVal p_id As String) As Boolean
        SyncLock _locker
            Dim com As pccDB4.Command = Nothing
            Dim par As IDbDataParameter = Nothing
            Dim iReader As IDataReader = Nothing
            Dim ret As Boolean = False
            Dim xmlds As New cXML

            Try


                com = conn.CreateCommand

                '   0       1      2      3         4
                'rec_id, geo_id, nome, objecto, metadados
                com.CommandText = "select " + com.GetFunction(pccDB4.ProviderFunctions.Convert, "rec_id", "p", "char(36)") & "," & com.GetFunction(pccDB4.ProviderFunctions.Convert, "geo_id", "p", "char(36)") & ", p.nome, p.objecto, p.mtd_id from catalogo p where p.rec_id=?"
                com.CommandType = CommandType.Text

                par = com.CreateParameter(DbType.String, p_id)
                com.AddParameter(par)

                iReader = com.ExecuteReader()

                If iReader.Read Then
                    Me.Id = iReader.GetString(0)
                    Me.Nome = iReader.GetString(2)

                    ' tem objecto
                    If Not iReader.IsDBNull(3) Then
                        Me.Objecto = New Object()
                        If Not xmlds.StringDeserializer(Me.Objecto, iReader.GetString(3)) Then
                            'TODO: deu erro a deserializar; é preciso testar
                        End If
                    End If

                    ' tem metadados
                    If Not iReader.IsDBNull(4) Then
                        Dim mtd_id As String = iReader.GetString(4)
                        If Not iReader.IsClosed Then iReader.Close()
                        Me.Metadata = New mtdMetadata()
                        Me.Metadata = Me.Metadata.GetById(conn, mtd_id)
                    End If

                    ret = True

                Else

                    ret = False

                End If

                If iReader IsNot Nothing Then
                    If Not iReader.IsClosed Then iReader.Close()
                End If

            Catch ex As Exception


                ret = False

            Finally

                If iReader IsNot Nothing Then
                    If Not iReader.IsClosed Then iReader.Close()
                End If

            End Try

            Return ret
        End SyncLock

    End Function

End Class

Public Class pccGeoLayer
    Inherits pccGeoCatalog

    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

    End Sub

    Public Overrides Function ToString() As String
        SyncLock _locker
            Return "Layer:" & Me.Nome
        End SyncLock


    End Function

End Class

<DataContract()>
<Serializable()>
Public Class pccGeoPoint
    Inherits pccGeoGeometry

    Private _x As Double
    Private _y As Double
    Private _z As Double

    Private Shared ReadOnly _locker As New Object()

    Public Sub New()
        Me.New("", "")
    End Sub

    Public Sub New(ByVal id As String, ByVal nome As String, ByVal x As Double, ByVal y As Double)
        Me.New(id, nome, x, y, 0)
        SyncLock _locker
            MyBase._3d = False
        End SyncLock

    End Sub

    Public Sub New(ByVal id As String, ByVal nome As String)
        Me.New(id, nome, 0, 0, 0)
        SyncLock _locker
            MyBase._3d = False
        End SyncLock
    End Sub

    Public Sub New(ByVal x As Double, ByVal y As Double)
        Me.New("", "", x, y, 0)
        SyncLock _locker
            MyBase._3d = False
        End SyncLock

    End Sub

    Public Sub New(ByVal x As Double, ByVal y As Double, ByVal z As Double)
        Me.New("", "", x, y, z)
        SyncLock _locker
            MyBase._3d = True
        End SyncLock

    End Sub

    Public Sub New(ByVal id As String, ByVal nome As String, ByVal x As Double, ByVal y As Double, ByVal z As Double)
        MyBase.New(id, nome)
        SyncLock _locker
            _x = x
            _y = y
            _z = z
            MyBase._3d = True
        End SyncLock

    End Sub

    <DataMember()>
    Public Property X() As Double
        Get
            SyncLock _locker
                Return _x
            End SyncLock
        End Get
        Set(ByVal value As Double)
            SyncLock _locker
                _x = value
            End SyncLock
        End Set
    End Property
    <DataMember()>
    Public Property Y() As Double
        Get
            SyncLock _locker
                Return _y
            End SyncLock
        End Get
        Set(ByVal value As Double)
            SyncLock _locker
                _y = value
            End SyncLock
        End Set
    End Property
    <DataMember()>
    Public Property Z() As Double
        Get
            SyncLock _locker
                If Not Me.Is3d Then
                    Return 0
                Else
                    Return _z
                End If
            End SyncLock
        End Get
        Set(ByVal value As Double)
            SyncLock _locker
                If Not Me.Is3d Then
                    _z = value
                Else
                    _z = value
                End If
            End SyncLock

        End Set
    End Property

    Public Function Coords() As String
        SyncLock _locker
            Dim ret As New StringBuilder(_x.ToString.Replace(",", ".") & " " & _y.ToString.Replace(",", "."))
            If MyBase._3d Then
                ret.Append(" ")
                ret.Append(_z.ToString.Replace(",", "."))
            End If
            Return ret.ToString
        End SyncLock

    End Function

    Public Overrides ReadOnly Property GetGeometryType() As pccGeoGeometryType
        Get
            SyncLock _locker
                Return pccGeoGeometryType.Point
            End SyncLock
        End Get
    End Property

    Public Overrides ReadOnly Property WKT() As String
        Get
            SyncLock _locker
                Dim ret As String = "POINT(" & Coords() & ")"
                MyBase._wkt = ret
                Return ret
            End SyncLock

        End Get
        'Set(ByVal value As String)
        '    Throw New Exception("pcc by design: WKT is a readonly property")
        'End Set
    End Property

    Public Overrides ReadOnly Property Is3d() As Boolean
        Get
            SyncLock _locker
                Return MyBase._3d
            End SyncLock
        End Get
    End Property
    <XmlIgnore(), SoapIgnore()>
    Public Overrides ReadOnly Property Area() As Double
        Get
            SyncLock _locker
                MyBase._area = 0
                Return 0
            End SyncLock

        End Get
        'Set(ByVal value As Double)
        '    Throw New Exception("pcc by design: Area is a readonly property")
        'End Set
    End Property

    <XmlIgnore(), SoapIgnore()>
    Public Overrides ReadOnly Property ConvexHul() As pccGeoPolygon
        Get
            SyncLock _locker
                Return Nothing
            End SyncLock
        End Get
    End Property
    <XmlIgnore(), SoapIgnore()>
    Public Overrides ReadOnly Property Length() As Double
        Get
            SyncLock _locker
                MyBase._length = 0
                Return 0
            End SyncLock

        End Get

    End Property

    Protected Overrides Sub Finalize()
        SyncLock _locker
            MyBase.Finalize()
        End SyncLock
    End Sub
    <XmlIgnore(), SoapIgnore()>
    Public Overrides ReadOnly Property MBR() As pccGeoRectangle
        Get
            SyncLock _locker
                _mbr = Nothing
                Return Nothing
            End SyncLock

        End Get

    End Property

    Public Overrides ReadOnly Property IsEqual(ByVal geo As pccGeoGeometry) As Boolean
        Get
            SyncLock _locker
                If Definition() <> geo.Definition Then
                    Return False
                End If

                If Me.X = CType(geo, pccGeoPoint).X And Me.Y = CType(geo, pccGeoPoint).Y And Me.Z = CType(geo, pccGeoPoint).Z Then

                    'TEST: Issue #4 - IsEqual
                    Return True
                Else
                    Return False
                End If
            End SyncLock
        End Get
    End Property

    <XmlIgnore(), SoapIgnore()>
    Public Overrides Property GetBasicGeometries() As pccGeoGeometry()

        Get
            SyncLock _locker
                Dim g(0) As pccGeoPoint
                g(0) = New pccGeoPoint()
                g(0) = Me
                Return g
            End SyncLock

        End Get

        Set(ByVal value As pccGeoGeometry())
            SyncLock _locker
                Throw New Exception("pcc by design: GetBasicGeometries is a readonly property")
            End SyncLock
        End Set

    End Property

    Public Overrides Function ToString() As String
        SyncLock _locker
            Return MyBase.ToString() & vbCrLf & " x:" & _x & " y:" & _y & " z:" & _z
        End SyncLock
    End Function

    Public Overrides Function GetMulti() As pccGeoGeometry
        SyncLock _locker
            Dim g As New pccGeoMultiPoint
            Return g
        End SyncLock

    End Function

    Public Overrides Function AddGeometry(ByRef g As pccGeoGeometry) As Boolean
        SyncLock _locker
            Me.Catalogo = g.Catalogo

            Me.X = CType(g, pccGeoPoint).X
            Me.Y = CType(g, pccGeoPoint).Y
            Me.Z = CType(g, pccGeoPoint).Z
        End SyncLock

    End Function

    Public Overrides ReadOnly Property Centroid() As pccGeoCentroid
        Get
            SyncLock _locker
                Return New pccGeoCentroid(_x, _y, _z)
            End SyncLock

        End Get

    End Property

    Public Overrides Function IsMulti() As Boolean
        SyncLock _locker
            Return False
        End SyncLock

    End Function

    Public Overrides Function Definition() As String
        SyncLock _locker
            Return "PT|0|" & X.ToString & "|" & Y.ToString & "|" & Z.ToString & "|" & Area.ToString & "|" & Length.ToString
        End SyncLock
    End Function
End Class
<DataContract()>
<KnownType(GetType(List(Of pccGeoPoint)))>
<KnownType(GetType(pccGeoPoint))>
<KnownType(GetType(pccGeoPolygon))>
<KnownType(GetType(pccGeoRectangle))>
<KnownType(GetType(pccGeoLineString))>
<KnownType(GetType(pccGeoMultiPoint))>
<KnownType(GetType(pccGeoMultiPolygon))>
<KnownType(GetType(pccGeoMultiLineString))>
<KnownType(GetType(pccGeoCollection))>
<Serializable()>
Public Class pccGeoMultiPoint
    Inherits pccGeoGeometry

    Dim gu As New pccGeoUtils

    Private _id As String
    Private _name As String

    Private _points As List(Of pccGeoPoint)
    Private _kpoints As Integer
    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

        Me.New("", "")
    End Sub

    Public Sub New(ByVal id As String, ByVal nome As String)

        MyBase.New(id, nome)
        SyncLock _locker
            _kpoints = 0
            _points = New List(Of pccGeoPoint)
        End SyncLock

    End Sub

    Public Sub AddPoint(ByRef p As pccGeoPoint)
        SyncLock _locker
            _kpoints += 1
            _points.Add(p)

            If _kpoints = 1 Then
                MyBase._3d = p.Is3d
            Else
                MyBase._3d = MyBase._3d And p.Is3d
            End If
        End SyncLock

    End Sub

    Public Overrides ReadOnly Property GetGeometryType() As pccGeoGeometryType
        Get

            SyncLock _locker
                Return pccGeoGeometryType.MultiPoint
            End SyncLock
        End Get
    End Property

    Public Overrides ReadOnly Property WKT() As String
        Get
            SyncLock _locker
                Dim ret As String = ""
                If _kpoints = 0 Then
                    ret &= "null"
                Else
                    For Each p As pccGeoPoint In _points
                        If ret = "" Then
                            ret &= "MULTIPOINT("
                        Else
                            ret &= ","
                        End If
                        ret &= p.Coords
                    Next
                End If
                ret &= ")"
                _wkt = ret
                Return ret
            End SyncLock

        End Get
    End Property

    Public ReadOnly Property Points() As List(Of pccGeoPoint)
        Get
            SyncLock _locker
                Return _points
            End SyncLock

        End Get
    End Property

    Public Overrides ReadOnly Property Is3d() As Boolean
        Get

            SyncLock _locker
                Return MyBase._3d
            End SyncLock
        End Get
    End Property

    Public Overrides ReadOnly Property Area() As Double
        Get
            SyncLock _locker
                If Not _area_calc Then
                    _area = 0
                    _area = gu.CalcArea(_points)
                    _area_calc = True
                End If
                Return _area
            End SyncLock
        End Get

    End Property

    Public Overrides ReadOnly Property ConvexHul() As pccGeoPolygon
        Get
            SyncLock _locker
                If Not _convexhul_calc Then
                    ' TODO: calcular convexhul
                    _convexhul_calc = True
                End If
                Return MyBase._convexhul
            End SyncLock
        End Get
    End Property

    Public Overrides ReadOnly Property Length() As Double
        Get
            SyncLock _locker
                If Not _length_calc Then
                    _length = 0
                    _length = gu.CalcLenght(_points)
                    _length_calc = True
                End If
                Return _length
            End SyncLock
        End Get

    End Property

    Public Overrides ReadOnly Property MBR() As pccGeoRectangle
        Get
            SyncLock _locker
                If Not _mbr_calc Then
                    gu.CalcMBR(_points, _mbr)
                    _mbr_calc = True
                End If
                Return _mbr
            End SyncLock
        End Get

    End Property

    Public Overrides ReadOnly Property IsEqual(ByVal geo As pccGeoGeometry) As Boolean
        Get
            SyncLock _locker
                'TEST: Issue #5 - IsEqual

                If Definition() <> geo.Definition Then
                    Return False
                End If

                Dim Resultado As Boolean = True
                Dim Size As Double = 0
                Dim i As Double = 0
                Dim j As Double = 0

                Dim Lista1 As List(Of pccGeoPoint)
                Dim Lista2 As List(Of pccGeoPoint)

                ' Se não tiverem igual número de pontos não é idêntico
                If _kpoints <> CType(geo, pccGeoMultiPoint)._kpoints Then
                    Resultado = False
                Else

                    ' Primeiro tem que se ordenar ambos os arrays de pontos 
                    Lista1 = New List(Of pccGeoPoint)(_points)
                    Lista2 = New List(Of pccGeoPoint)(CType(geo, pccGeoMultiPoint)._points)

                    Lista1.Sort(AddressOf pccGeoPoint.Compare)
                    Lista2.Sort(AddressOf pccGeoPoint.Compare)
                    ' Aqui ambos as listas de GeoPoints estao ordenadas segundo o mesmo critério
                    ' Agora é verificar se são identicas

                    Size = Lista1.Count

                    i = 0
                    j = 0
                    Dim consecutive_matches As Double = 0
                    Dim Total_Comparations As Double = 0
                    While Total_Comparations < 2 * Size And consecutive_matches < Size
                        If Lista1(i).IsEqual(Lista2(j)) Then

                            consecutive_matches = consecutive_matches + 1
                            i = i + 1
                            If i >= Size Then i = 0
                        Else
                            consecutive_matches = 0
                        End If
                        j = j + 1
                        If j >= Size Then j = 0
                        Total_Comparations = Total_Comparations + 1
                    End While
                    Resultado = (consecutive_matches = Size)
                    Lista1.Clear()
                    Lista2.Clear()
                End If

                Return Resultado
            End SyncLock
        End Get
    End Property

    <XmlIgnore(), SoapIgnore()>
    Public Overrides Property GetBasicGeometries() As pccGeoGeometry()
        Get
            SyncLock _locker
                Dim g(0) As pccGeoMultiPoint
                g(0) = New pccGeoMultiPoint()
                g(0) = Me
                Return g
            End SyncLock

        End Get
        Set(ByVal value As pccGeoGeometry())
            SyncLock _locker
                Throw New Exception("pcc by design: GetBasicGeometries is a readonly property")
            End SyncLock

        End Set
    End Property

    Public Overrides Function ToString() As String
        SyncLock _locker
            Return MyBase.ToString() & vbCrLf & " k:" & _kpoints
        End SyncLock

    End Function


    Public Overrides Function GetMulti() As pccGeoGeometry
        SyncLock _locker
            Dim g As New pccGeoCollection
            Return g
        End SyncLock

    End Function

    Public Overrides Function AddGeometry(ByRef g As pccGeoGeometry) As Boolean
        SyncLock _locker
            AddPoint(g)
        End SyncLock

    End Function

    Public Overrides ReadOnly Property Centroid() As pccGeoCentroid
        Get

            SyncLock _locker
                If Not _centroid_calc Then
                    Dim tx As Double = 0
                    Dim ty As Double = 0
                    For i As Integer = 0 To _points.Count - 1
                        tx += _points(i).X
                        ty += _points(i).Y
                    Next
                    _centroid = New pccGeoCentroid(tx / _points.Count, ty / _points.Count, 0)
                    _centroid_calc = True
                End If
                Return _centroid
            End SyncLock
        End Get

    End Property

    Public Overrides Function IsMulti() As Boolean
        SyncLock _locker
            Return True
        End SyncLock

    End Function

    Public Overrides Function Definition() As String
        SyncLock _locker
            Return "MPT|" & _kpoints.ToString & "|" & Centroid.X.ToString & "|" & Centroid.Y.ToString & "|" & Centroid.Z.ToString & "|" & Area.ToString & "|" & Length.ToString

        End SyncLock

    End Function
End Class
<DataContract()>
<KnownType(GetType(List(Of pccGeoPoint)))>
<KnownType(GetType(pccGeoPoint))>
<KnownType(GetType(pccGeoPolygon))>
<KnownType(GetType(pccGeoRectangle))>
<KnownType(GetType(pccGeoLineString))>
<KnownType(GetType(pccGeoMultiPoint))>
<KnownType(GetType(pccGeoMultiPolygon))>
<KnownType(GetType(pccGeoMultiLineString))>
<KnownType(GetType(pccGeoCollection))>
<Serializable()>
Public Class pccGeoLineString
    Inherits pccGeoGeometry

    Dim gu As New pccGeoUtils

    Private _vertices As List(Of pccGeoPoint)
    Private _kvertices As Integer
    Private Shared ReadOnly _locker As New Object()

    Public Sub New()
        Me.New("", "")
    End Sub

    Public Sub New(ByVal id As String, ByVal nome As String)
        MyBase.New(id, nome)
        SyncLock _locker
            _kvertices = 0
            _vertices = New List(Of pccGeoPoint)
        End SyncLock

    End Sub

    Public Sub AddVertice(ByRef p As pccGeoPoint)
        SyncLock _locker
            _kvertices += 1
            _vertices.Add(p)

            If _kvertices = 1 Then
                MyBase._3d = p.Is3d
            Else
                MyBase._3d = MyBase._3d And p.Is3d
            End If

            _centroid_calc = False
            _convexhul_calc = False
            _length_calc = False
            _mbr_calc = False
        End SyncLock
    End Sub

    Public Function Coords() As String
        SyncLock _locker
            Dim ret As String = ""
            If _kvertices = 0 Then
                ret &= "null"
            Else
                For Each p As pccGeoPoint In _vertices
                    If ret <> "" Then ret &= ","
                    ret &= p.Coords
                Next
            End If
            Return ret
        End SyncLock
    End Function

    Public Overrides ReadOnly Property GetGeometryType() As pccGeoGeometryType
        Get
            SyncLock _locker
                Return pccGeoGeometryType.LineString
            End SyncLock
        End Get
    End Property

    Public Overrides ReadOnly Property WKT() As String
        Get
            SyncLock _locker
                Dim ret As String = "LINESTRING(" & Coords() & ")"
                _wkt = ret
                Return ret
            End SyncLock
        End Get

    End Property

    Public Overrides ReadOnly Property Is3d() As Boolean
        Get
            SyncLock _locker
                Return MyBase._3d
            End SyncLock
        End Get
    End Property

    Public Overrides ReadOnly Property Area() As Double
        Get
            SyncLock _locker
                If Not _area_calc Then
                    _area = 0
                    _area = gu.CalcArea(_vertices)
                    _area_calc = True
                End If

                Return _area
            End SyncLock

        End Get

    End Property

    Public Overrides ReadOnly Property ConvexHul() As pccGeoPolygon
        Get

            SyncLock _locker
                If Not _convexhul_calc Then
                    'TODO: calcular convexhul
                    _convexhul_calc = True
                End If
                Return MyBase._convexhul
            End SyncLock
        End Get
    End Property

    Public Overrides ReadOnly Property Length() As Double
        Get

            SyncLock _locker
                If Not _length_calc Then
                    _length = 0
                    _length = gu.CalcLenght(_vertices)
                    _length_calc = True
                End If
                Return _length
            End SyncLock
        End Get

    End Property

    Public Overrides ReadOnly Property MBR() As pccGeoRectangle
        Get
            SyncLock _locker
                If Not _mbr_calc Then
                    gu.CalcMBR(_vertices, _mbr)
                    _mbr_calc = True
                End If
                Return _mbr
            End SyncLock
        End Get

    End Property

    Public ReadOnly Property Vertices As List(Of pccGeoPoint)
        Get
            SyncLock _locker
                Return _vertices
            End SyncLock
        End Get
    End Property

    Public Overrides ReadOnly Property IsEqual(ByVal geo As pccGeoGeometry) As Boolean
        Get
            SyncLock _locker
                Dim i As Integer = 0
                Dim Resultado As Boolean = True

                If Definition() <> geo.Definition Then
                    Return False
                End If

                'TEST: Issue #6 - IsEqual
                'Se numero de vértices for diferente não são iguais
                If _kvertices = CType(geo, pccGeoLineString).Vertices.Count Then
                    'Compara do inicio para o fim 
                    For i = 0 To _kvertices - 1
                        If Not _vertices(i).IsEqual(CType(geo, pccGeoLineString).Vertices(i)) Then
                            Resultado = False
                            Exit For
                        End If
                    Next
                    'Se ainda não são iguais vai comparar do fim para o inicio 
                    If Not Resultado Then
                        Resultado = True
                        For i = 0 To _kvertices - 1
                            If Not _vertices(i).IsEqual(CType(geo, pccGeoLineString).Vertices(_kvertices - i - 1)) Then
                                Resultado = False
                                Exit For
                            End If
                        Next
                    End If
                Else
                    Resultado = False
                End If
                Return Resultado
            End SyncLock
        End Get
    End Property
    <XmlIgnore(), SoapIgnore()>
    Public Overrides Property GetBasicGeometries() As pccGeoGeometry()
        Get
            SyncLock _locker
                Dim g(0) As pccGeoLineString
                g(0) = New pccGeoLineString
                g(0) = Me
                Return g
            End SyncLock

        End Get
        Set(ByVal value As pccGeoGeometry())
            SyncLock _locker
                Throw New Exception("pcc by design: GetBasicGeometries is a readonly property")
            End SyncLock

        End Set
    End Property

    Public Overrides Function ToString() As String
        SyncLock _locker
            Return MyBase.ToString() & vbCrLf & " k:" & _kvertices
        End SyncLock
    End Function

    Public Overrides Function GetMulti() As pccGeoGeometry
        SyncLock _locker
            Dim g As New pccGeoMultiLineString
            Return g
        End SyncLock
    End Function

    Public Overrides Function AddGeometry(ByRef g As pccGeoGeometry) As Boolean
        SyncLock _locker
            Me.Catalogo = g.Catalogo

            Me._vertices.Clear()
            Me._kvertices = 0

            For Each v As pccGeoPoint In CType(g, pccGeoLineString).Vertices
                Me.AddVertice(v)
            Next

            _centroid_calc = False
            _convexhul_calc = False
            _length_calc = False
            _mbr_calc = False
        End SyncLock
    End Function

    Public Overrides ReadOnly Property Centroid() As pccGeoCentroid

        Get
            SyncLock _locker
                Dim len As Double = 0
                Dim tLen As Double = 0

                Dim tx As Double = 0
                Dim ty As Double = 0

                If Not _centroid_calc Then

                    For i As Integer = 0 To _vertices.Count - 2
                        len = gu.Distance(_vertices(i), _vertices(i + 1)) / 2
                        tx += len * (_vertices(i + 1).X + _vertices(i).X)
                        ty += len * (_vertices(i + 1).Y + _vertices(i).Y)
                        tLen += (len * 2)
                    Next

                    _centroid = New pccGeoCentroid(tx / tLen, ty / tLen, 0)
                    _centroid_calc = True

                    ' aproveita para fazer set de Length
                    If Not _length_calc Then
                        _length = tLen
                        _length_calc = True
                    End If

                End If

                Return _centroid
            End SyncLock
        End Get

    End Property

    Public Overrides Function IsMulti() As Boolean
        SyncLock _locker
            Return False
        End SyncLock
    End Function

    Public Overrides Function Definition() As String
        SyncLock _locker
            Return "PL|" & _kvertices.ToString & "|" & Centroid.X.ToString & "|" & Centroid.Y.ToString & "|" & Centroid.Z.ToString & "|" & Area.ToString & "|" & Length.ToString
        End SyncLock
    End Function

End Class
<DataContract()>
<KnownType(GetType(List(Of pccGeoLineString)))>
<KnownType(GetType(pccGeoPoint))>
<KnownType(GetType(pccGeoPolygon))>
<KnownType(GetType(pccGeoRectangle))>
<KnownType(GetType(pccGeoLineString))>
<KnownType(GetType(pccGeoMultiPoint))>
<KnownType(GetType(pccGeoMultiPolygon))>
<KnownType(GetType(pccGeoMultiLineString))>
<KnownType(GetType(pccGeoCollection))>
<Serializable()>
Public Class pccGeoMultiLineString
    Inherits pccGeoGeometry

    Dim gu As New pccGeoUtils

    Private _linestrings As List(Of pccGeoLineString)
    Private _klinestrings As Integer

    Private Shared ReadOnly _locker As New Object()

    Public Sub New()
        Me.New("", "")
    End Sub

    Public Sub New(ByVal id As String, ByVal nome As String)
        MyBase.New(id, nome)
        SyncLock _locker
            _klinestrings = 0
            _linestrings = New List(Of pccGeoLineString)
        End SyncLock

    End Sub

    Public Sub AddLineString(ByRef ls As pccGeoLineString)
        SyncLock _locker
            _klinestrings += 1
            _linestrings.Add(ls)

            If _klinestrings = 1 Then
                MyBase._3d = ls.Is3d
            Else
                MyBase._3d = MyBase._3d And ls.Is3d
            End If

            _centroid_calc = False
            _convexhul_calc = False
            _length_calc = False
            _mbr_calc = False
        End SyncLock


    End Sub

    Public Overrides ReadOnly Property GetGeometryType() As pccGeoGeometryType
        Get

            SyncLock _locker
                Return pccGeoGeometryType.MultiLineString
            End SyncLock
        End Get
    End Property

    Public Overrides ReadOnly Property WKT() As String
        Get
            SyncLock _locker
                Dim ret As String = ""
                If _klinestrings = 0 Then
                    ret &= "null"
                Else
                    For Each ls As pccGeoLineString In _linestrings
                        If ret = "" Then
                            ret = "MULTILINESTRING("
                        Else
                            ret &= ","
                        End If
                        ret &= ls.WKT.Substring(10)
                    Next
                End If
                ret &= ")"
                _wkt = ret
                Return ret
            End SyncLock
        End Get

    End Property

    Public Overrides ReadOnly Property Is3d() As Boolean
        Get
            SyncLock _locker
                Return MyBase._3d
            End SyncLock
        End Get
    End Property

    Public Overrides ReadOnly Property Area() As Double
        Get
            SyncLock _locker
                If Not _area_calc Then
                    _area = 0
                    If _linestrings IsNot Nothing Then
                        For Each ls As pccGeoLineString In _linestrings
                            _area += ls.Area
                        Next
                    End If
                    _area_calc = True
                End If
                Return _area
            End SyncLock
        End Get

    End Property

    Public Overrides ReadOnly Property ConvexHul() As pccGeoPolygon
        Get
            SyncLock _locker
                If Not _convexhul_calc Then
                    ' TODO: calcular convexhul
                    _convexhul_calc = True
                End If
                Return _convexhul
            End SyncLock
        End Get
    End Property

    Public Overrides ReadOnly Property Length() As Double
        Get
            SyncLock _locker
                If Not _length_calc Then
                    _length = 0
                    If _linestrings IsNot Nothing Then
                        For Each ls As pccGeoLineString In _linestrings
                            _length += ls.Length
                        Next
                    End If
                    _length_calc = True
                End If
                Return _length
            End SyncLock
        End Get

    End Property

    Public Overrides ReadOnly Property MBR() As pccGeoRectangle
        Get
            SyncLock _locker
                If Not _mbr_calc Then
                    Dim cents As New pccGeoMultiPoint()
                    For Each ls As pccGeoLineString In _linestrings
                        For Each p As pccGeoPoint In ls.MBR.Vertices
                            cents.AddPoint(p)
                        Next
                    Next
                    gu.CalcMBR(cents.Points, _mbr)
                    _mbr_calc = True
                End If
                Return _mbr
            End SyncLock
        End Get

    End Property

    Public Overrides ReadOnly Property IsEqual(ByVal geo As pccGeoGeometry) As Boolean
        Get
            SyncLock _locker
                'TEST: Issue #7 - IsEqual
                If Definition() <> geo.Definition Then
                    Return False
                End If

                'Ter em atenção que as várias linhas podem não aparecer pela mesma ordem.
                Dim Resultado As Boolean = True
                Dim i As Long = 0
                Dim j As Long = 0

                Dim Linhas1 As List(Of pccGeoLineString)
                Dim Linhas2 As List(Of pccGeoLineString)

                If _klinestrings <> CType(geo, pccGeoMultiLineString)._klinestrings Then
                    Resultado = False
                Else

                    ' Primeiro tem que se ordenar ambos os arrays de pontos 
                    Linhas1 = New List(Of pccGeoLineString)(_linestrings)
                    Linhas2 = New List(Of pccGeoLineString)(CType(geo, pccGeoMultiLineString)._linestrings)

                    Linhas1.Sort(AddressOf pccGeoLineString.Compare)
                    Linhas2.Sort(AddressOf pccGeoLineString.Compare)
                    ' Aqui ambos as listas de GeoGeoMultiPolygons estao ordenadas segundo o mesmo critério
                    ' Agora é verificar se são identicas

                    For i = 0 To Linhas1.Count - 1
                        If Not Linhas1(i).IsEqual(Linhas2(i)) Then
                            Resultado = False
                            Exit For
                        End If
                    Next
                    'For Each i1 As pccGeoGeometry In Linhas1
                    '    For Each i2 As pccGeoGeometry In Linhas2
                    '        If Not i1.IsEqual(i2) Then
                    '            Resultado = False
                    '            Exit For
                    '        End If
                    '    Next
                    '    If Not Resultado Then
                    '        Exit For
                    '    End If
                    'Next
                    Return Resultado
                End If
            End SyncLock
        End Get
    End Property
    <XmlIgnore(), SoapIgnore()>
    Public Overrides Property GetBasicGeometries() As pccGeoGeometry()
        Get
            SyncLock _locker
                Dim g(0) As pccGeoMultiLineString
                g(0) = New pccGeoMultiLineString()
                g(0) = Me
                Return g
            End SyncLock
        End Get
        Set(ByVal value As pccGeoGeometry())
            SyncLock _locker
                Throw New Exception("pcc by design: GetBasicGeometries is a readonly property")
            End SyncLock
        End Set
    End Property

    Public Overrides Function ToString() As String
        SyncLock _locker
            Return MyBase.ToString() & vbCrLf & " k:" & _klinestrings
        End SyncLock
    End Function

    Public Overrides Function GetMulti() As pccGeoGeometry
        SyncLock _locker
            Dim g As New pccGeoCollection
            Return g
        End SyncLock
    End Function

    Public Overrides Function AddGeometry(ByRef g As pccGeoGeometry) As Boolean
        SyncLock _locker
            Me.AddLineString(g)
        End SyncLock
    End Function

    Public Overrides ReadOnly Property Centroid() As pccGeoCentroid

        Get
            SyncLock _locker
                If Not _centroid_calc Then

                    Dim tLen As Double = 0

                    Dim a_centroids(Me._klinestrings - 1) As pccGeoCentroid
                    Dim a_lens(Me._klinestrings - 1) As Double
                    Dim k As Integer = 0

                    For Each l As pccGeoLineString In _linestrings
                        k += 1
                        a_centroids(k - 1) = l.Centroid
                        a_lens(k - 1) = l.Length
                        tLen += a_lens(k - 1)
                    Next

                    Dim tx As Double = 0
                    Dim ty As Double = 0

                    tLen = 0

                    For i As Integer = 0 To a_centroids.Length - 1
                        tx += a_lens(i) * a_centroids(i).X
                        ty += a_lens(i) * a_centroids(i).Y
                        tLen += a_lens(i)
                    Next

                    _centroid = New pccGeoCentroid(tx / tLen, ty / tLen, 0)
                    _centroid_calc = True

                    ' aproveita para fazer set de Length
                    If Not _length_calc Then
                        _length = tLen
                        _length_calc = True
                    End If

                End If

                Return _centroid
            End SyncLock
        End Get

    End Property

    Public Overrides Function IsMulti() As Boolean
        SyncLock _locker
            Return True
        End SyncLock
    End Function

    Public Overrides Function Definition() As String
        SyncLock _locker
            Return "MPL|" & _klinestrings.ToString & "|" & Centroid.X.ToString & "|" & Centroid.Y.ToString & "|" & Centroid.Z.ToString & "|" & Area.ToString & "|" & Length.ToString
        End SyncLock
    End Function
End Class

<DataContract()>
<KnownType(GetType(List(Of pccGeoPoint)))>
<KnownType(GetType(List(Of pccGeoPolygon)))>
<KnownType(GetType(pccGeoPoint))>
<KnownType(GetType(pccGeoPolygon))>
<KnownType(GetType(pccGeoRectangle))>
<KnownType(GetType(pccGeoLineString))>
<KnownType(GetType(pccGeoMultiPoint))>
<KnownType(GetType(pccGeoMultiPolygon))>
<KnownType(GetType(pccGeoMultiLineString))>
<KnownType(GetType(pccGeoCollection))>
<Serializable()>
Public Class pccGeoPolygon
    Inherits pccGeoGeometry

    Dim gu As New pccGeoUtils

    Private _vertices As List(Of pccGeoPoint)
    Private _kvertices As Integer
    Private _innerpols As List(Of pccGeoPolygon)
    Private _kinnerpols As Integer

    Private Shared ReadOnly _locker As New Object()

    Public Sub New()
        Me.New("", "")
    End Sub

    Public Sub New(ByVal id As String, ByVal nome As String)
        MyBase.New(id, nome)
        SyncLock _locker
            _kvertices = 0
            _kinnerpols = 0
            _vertices = New List(Of pccGeoPoint)
            _innerpols = New List(Of pccGeoPolygon)
        End SyncLock
    End Sub

    Public Sub AddVertice(ByRef p As pccGeoPoint)
        SyncLock _locker
            _kvertices += 1
            _vertices.Add(p)

            If _kvertices = 1 Then
                If _kinnerpols = 0 Then
                    MyBase._3d = p.Is3d
                Else
                    MyBase._3d = MyBase._3d And p.Is3d
                End If
            Else
                MyBase._3d = MyBase._3d And p.Is3d
            End If
            _area_calc = False
        End SyncLock
    End Sub

    Public Sub AddInnerPolygon(ByRef p As pccGeoPolygon)
        SyncLock _locker
            _kinnerpols += 1
            _innerpols.Add(p)
            If _kinnerpols = 1 Then
                If _kvertices = 0 Then
                    MyBase._3d = p.Is3d
                Else
                    MyBase._3d = MyBase._3d And p.Is3d
                End If
            Else
                MyBase._3d = MyBase._3d And p.Is3d
            End If
            _area_calc = False
            _centroid_calc = False
        End SyncLock
    End Sub

    Public Property Vertices As List(Of pccGeoPoint)
        Get
            SyncLock _locker
                Return _vertices
            End SyncLock
        End Get
        Set(ByVal value As List(Of pccGeoPoint))
            SyncLock _locker
                Throw New Exception("pcc by design: Vertices is a readonly property")
            End SyncLock
        End Set

    End Property

    Public Function Coords() As String
        SyncLock _locker
            Dim ret As New StringBuilder("(")
            If _kvertices = 0 Then
                ret.Append("null")
            Else
                For i As Integer = 0 To _kvertices - 1
                    If i > 0 Then ret.Append(", ")
                    ret.Append(_vertices(i).Coords)
                Next
            End If

            'If ret.Length > 1 Then ret &= "," & _vertices(0).Coords
            ret.Append(")")

            If _kinnerpols > 0 Then
                For i As Integer = 0 To _kinnerpols - 1
                    ret.Append("," & _innerpols(i).Coords)
                Next
            End If

            Return ret.ToString
        End SyncLock
    End Function

    Public Function InnerPolygons() As List(Of pccGeoPolygon)
        SyncLock _locker
            Return _innerpols
        End SyncLock
    End Function

    Public Overrides ReadOnly Property GetGeometryType() As pccGeoGeometryType
        Get
            SyncLock _locker
                Return pccGeoGeometryType.Polygon
            End SyncLock
        End Get
    End Property

    Public Overrides ReadOnly Property WKT() As String
        Get
            SyncLock _locker
                Dim ret As String = "POLYGON(" & Coords() & ")"
                _wkt = ret
                Return ret
            End SyncLock
        End Get

    End Property

    Public Overrides ReadOnly Property Is3d() As Boolean
        Get
            SyncLock _locker
                Return MyBase._3d
            End SyncLock
        End Get
    End Property

    Public Overrides ReadOnly Property Area() As Double
        Get
            SyncLock _locker
                If Not _area_calc Then
                    _area = gu.CalcArea(_vertices)
                    If _innerpols IsNot Nothing Then
                        For Each p As pccGeoPolygon In _innerpols
                            _area += p.Area
                        Next
                    End If
                    _area_calc = True
                End If
                Return _area
            End SyncLock
        End Get

    End Property

    Public Overrides ReadOnly Property ConvexHul() As pccGeoPolygon
        Get
            SyncLock _locker
                If Not _convexhul_calc Then
                    ' TODO: calcular convexhul
                    _convexhul_calc = True
                End If
                Return _convexhul
            End SyncLock
        End Get
    End Property

    Public Overrides ReadOnly Property Length() As Double
        Get
            SyncLock _locker
                If Not _length_calc Then
                    _length = 0
                    _length = gu.CalcLenght(_vertices)
                    _length_calc = True
                End If
                Return _length
            End SyncLock
        End Get

    End Property

    Public Overrides ReadOnly Property MBR() As pccGeoRectangle
        Get
            SyncLock _locker
                If Not _mbr_calc Then
                    Dim cents As New pccGeoMultiPoint()
                    For Each p As pccGeoPoint In Vertices()
                        cents.AddPoint(p)
                    Next
                    gu.CalcMBR(cents.Points, _mbr)
                    _mbr_calc = True
                End If
                Return _mbr
            End SyncLock
        End Get

    End Property

    Public Function IsHole() As Boolean
        SyncLock _locker
            Return (Area() < 0)
        End SyncLock
    End Function

    Public Overrides ReadOnly Property IsEqual(ByVal geo As pccGeoGeometry) As Boolean
        Get
            SyncLock _locker
                'TEST: Issue #8 - IsEqual
                If Definition() <> geo.Definition Then
                    Return False
                End If
                Dim Resultado As Boolean = True
                Dim Size As Double = 0
                Dim i As Double = 0
                Dim j As Double = 0

                Dim PoligonoExterior1 As List(Of pccGeoPoint)
                Dim PoligonoExterior2 As List(Of pccGeoPoint)
                Dim PoligonosInteriores1 As List(Of pccGeoPolygon)
                Dim PoligonosInteriores2 As List(Of pccGeoPolygon)
                ' Se não tiverem igual número de poligonos não é idêntico de certeza
                If _kvertices <> CType(geo, pccGeoPolygon)._kvertices Or _kinnerpols <> CType(geo, pccGeoPolygon)._kinnerpols Then
                    Resultado = False
                Else

                    ' Vamos tratar os poligonos exteriores primeiro
                    ' Ordenar ambos os arrays de poligonos 
                    PoligonoExterior1 = New List(Of pccGeoPoint)(_vertices)
                    PoligonoExterior2 = New List(Of pccGeoPoint)(CType(geo, pccGeoPolygon)._vertices)

                    PoligonoExterior1.Sort(AddressOf pccGeoPoint.Compare)
                    PoligonoExterior2.Sort(AddressOf pccGeoPoint.Compare)
                    ' Aqui ambos as listas de GeoPolygon Exteriores estao ordenados segundo o mesmo critério
                    ' Agora é verificar se são identicos
                    Size = PoligonoExterior1.Count
                    i = 0
                    j = 0
                    Dim consecutive_matches As Double = 0
                    Dim Total_Comparations As Double = 0
                    While Total_Comparations < 2 * Size And consecutive_matches < Size
                        If PoligonoExterior1(i).IsEqual(PoligonoExterior2(j)) Then

                            consecutive_matches = consecutive_matches + 1
                            i = i + 1
                            If i >= Size Then i = 0
                        Else
                            consecutive_matches = 0
                        End If
                        j = j + 1
                        If j >= Size Then j = 0
                        Total_Comparations = Total_Comparations + 1
                    End While

                    Resultado = (consecutive_matches = Size)

                    ' Fim da comparação dos poligonos exteriores

                    'Comparação dos poligonos interiores (ilhas)

                    If _kinnerpols <> 0 And Resultado And CType(geo, pccGeoPolygon)._kinnerpols - 1 <> 0 Then
                        ' Temos ilhas e os Poligono exterior são idênticos
                        PoligonosInteriores1 = _innerpols
                        PoligonosInteriores2 = CType(geo, pccGeoPolygon)._innerpols

                        PoligonosInteriores1.Sort(AddressOf pccGeoPolygon.Compare)
                        PoligonosInteriores2.Sort(AddressOf pccGeoPolygon.Compare)
                        ' Aqui ambos as listas de GeoPolygon Interiores estao ordenados segundo o mesmo critério
                        For i = 0 To _kinnerpols - 1
                            If Not PoligonosInteriores1.Item(i).IsEqual(PoligonosInteriores2.Item(i)) Then
                                Resultado = False
                                Exit For
                            End If
                        Next

                    End If

                End If
                Return Resultado
            End SyncLock
        End Get
    End Property
    <XmlIgnore(), SoapIgnore()>
    Public Overrides Property GetBasicGeometries() As pccGeoGeometry()

        Get
            SyncLock _locker
                Dim g() As pccGeoPolygon
                Dim kg As Integer = 0

                ReDim Preserve g(kg)
                g(kg) = New pccGeoPolygon

                If _vertices IsNot Nothing Then

                    For Each v As pccGeoPoint In _vertices
                        g(kg).AddVertice(v)
                    Next

                    If _innerpols IsNot Nothing Then
                        For Each po As pccGeoPolygon In _innerpols
                            For Each poo As pccGeoPolygon In po.GetBasicGeometries
                                kg += 1
                                ReDim Preserve g(kg)
                                g(kg) = poo
                            Next
                        Next
                    End If

                End If

                Return g
            End SyncLock
        End Get

        Set(ByVal value As pccGeoGeometry())
            SyncLock _locker
                Throw New Exception("pcc by design: GetBasicGeometries is a readonly property")
            End SyncLock
        End Set

    End Property

    Public Overrides Function ToString() As String
        SyncLock _locker
            Return MyBase.ToString() & vbCrLf & " k:" & _kvertices & " holes:" & _kinnerpols
        End SyncLock
    End Function

    Public Overrides Function GetMulti() As pccGeoGeometry
        SyncLock _locker
            Dim g As New pccGeoMultiPolygon
            Return g
        End SyncLock
    End Function

    Public Overrides Function AddGeometry(ByRef g As pccGeoGeometry) As Boolean
        SyncLock _locker
            Catalogo = g.Catalogo

            _vertices.Clear()
            _kvertices = 0
            _innerpols.Clear()
            _kinnerpols = 0

            For Each v As pccGeoPoint In CType(g, pccGeoPolygon).Vertices
                Me.AddVertice(v)
            Next

            If CType(g, pccGeoPolygon)._innerpols IsNot Nothing Then
                For Each ip As pccGeoPolygon In CType(g, pccGeoPolygon)._innerpols
                    Me.AddInnerPolygon(ip)
                Next
            End If
        End SyncLock
    End Function

    '<XmlIgnore(), SoapIgnore()> _

    Public Overrides ReadOnly Property Centroid() As pccGeoCentroid

        Get
            SyncLock _locker
                If Not _centroid_calc Then

                    Dim tLen As Double = 0

                    Dim a_centroids(Me._kinnerpols) As pccGeoCentroid  'já contempla o pol principal
                    Dim a_areas(Me._kinnerpols) As Double
                    Dim k As Integer = 0

                    a_areas(0) = gu.CalcArea(_vertices)

                    Dim tx As Double = 0
                    Dim ty As Double = 0
                    Dim d_aux As Double = 0

                    tLen = 0

                    For i As Integer = 0 To _kvertices - 2
                        d_aux = _vertices(i).X * _vertices(i + 1).Y - _vertices(i + 1).X * _vertices(i).Y
                        tx += (_vertices(i).X + _vertices(i + 1).X) * d_aux
                        ty += (_vertices(i).Y + _vertices(i + 1).Y) * d_aux
                    Next

                    a_centroids(0) = New pccGeoCentroid(tx / (6 * a_areas(0)), ty / (6 * a_areas(0)), 0)

                    For i As Integer = 0 To _kinnerpols - 1
                        a_centroids(i + 1) = _innerpols(i).Centroid
                        a_areas(i + 1) = gu.CalcArea(_innerpols(i)._vertices)
                    Next

                    tx = 0
                    ty = 0

                    Dim tArea As Double = 0

                    For i As Integer = 0 To a_centroids.Length - 1
                        tx += a_areas(i) * a_centroids(i).X
                        ty += a_areas(i) * a_centroids(i).Y
                        tArea += a_areas(i)
                    Next

                    _centroid = New pccGeoCentroid(tx / tArea, ty / tArea, 0)
                    _centroid_calc = True

                    If Not _area_calc Then
                        _area = tArea
                        _area_calc = True
                    End If

                End If

                Return _centroid
            End SyncLock
        End Get

    End Property

    Public Overrides Function IsMulti() As Boolean
        SyncLock _locker
            Return False
        End SyncLock
    End Function

    Public Overrides Function Definition() As String
        SyncLock _locker
            Return "PG|" & _kvertices.ToString & "|" & Centroid.X.ToString & "|" & Centroid.Y.ToString & "|" & Centroid.Z.ToString & "|" & Area.ToString & "|" & Length.ToString
        End SyncLock
    End Function
End Class

<DataContract()>
Public Class pccGeoRectangle
    Inherits pccGeoPolygon

    'Private _width As Double
    'Private _height As Double
    Private Shared ReadOnly _locker As New Object()

    Public Overrides ReadOnly Property MBR As pccGeoRectangle
        Get
            SyncLock _locker
                _mbr = Nothing
                Return Nothing
            End SyncLock
        End Get

    End Property

    <XmlIgnore(), SoapIgnore()>
    Public Overrides Property GetBasicGeometries As pccGeoGeometry()
        Get
            SyncLock _locker
                Return Nothing
            End SyncLock
        End Get
        Set(ByVal value As pccGeoGeometry())

        End Set
    End Property

    '<XmlIgnore(), SoapIgnore()> _
    'Public Property Width As Double
    '    Get
    '        Return Math.Abs(Me.Vertices(0).X - Me.Vertices(2).X)
    '    End Get
    '    Set(ByVal value As Double)

    '    End Set
    'End Property
    '<XmlIgnore(), SoapIgnore()> _
    'Public Property Height As Double
    '    Get
    '        Return Math.Abs(Me.Vertices(0).Y - Me.Vertices(2).Y)
    '    End Get
    '    Set(ByVal value As Double)

    '    End Set
    'End Property

End Class
<DataContract()>
<Serializable()>
Public Class pccGeoCentroid
    'Inherits pccGeoPoint
    <DataMember(Name:="X")>
    Private _x As Double
    <DataMember(Name:="Y")>
    Private _y As Double
    <DataMember(Name:="Z")>
    Private _z As Double
    <DataMember(Name:="WKT")>
    Private _wkt As String
    <DataMember(Name:="Is3d")>
    Private _3d As Boolean

    Private Shared ReadOnly _locker As New Object()

    Public Sub New()
        Me.New(0, 0, 0)
        SyncLock _locker
            _3d = False
        End SyncLock
    End Sub
    Public Sub New(ByVal id As String, ByVal nome As String, ByVal x As Double, ByVal y As Double)
        Me.New(x, y, 0)
        SyncLock _locker
            _3d = False
        End SyncLock
    End Sub
    Public Sub New(ByVal x As Double, ByVal y As Double, ByVal z As Double)
        SyncLock _locker
            _x = x
            _y = y
            _z = z
            _3d = True
        End SyncLock
    End Sub

    Public Property X() As Double
        Get
            SyncLock _locker
                Return _x
            End SyncLock
        End Get
        Set(ByVal value As Double)
            SyncLock _locker
                _x = value
            End SyncLock
        End Set
    End Property

    Public Property Y() As Double
        Get
            SyncLock _locker
                Return _y
            End SyncLock
        End Get
        Set(ByVal value As Double)
            SyncLock _locker
                _y = value
            End SyncLock
        End Set
    End Property

    Public Property Z() As Double
        Get
            SyncLock _locker
                If Not Me.Is3d Then
                    Return 0
                Else
                    Return _z
                End If
            End SyncLock
        End Get
        Set(ByVal value As Double)
            SyncLock _locker
                If Not Me.Is3d Then
                    _z = value
                Else
                    _z = value
                End If
            End SyncLock
        End Set
    End Property

    Public Property Is3d() As Boolean
        Get
            SyncLock _locker
                Return _3d
            End SyncLock
        End Get
        Set(ByVal value As Boolean)
            SyncLock _locker
                _3d = value
            End SyncLock
        End Set
    End Property
    Public ReadOnly Property WKT As String
        Get
            SyncLock _locker
                Dim ret As String = "POINT(" & Coords() & ")"
                _wkt = ret
                Return ret
            End SyncLock
        End Get

    End Property
    <XmlIgnore(), SoapIgnore()>
    Public Property MBR As pccGeoRectangle
        Get
            SyncLock _locker
                Return Nothing
            End SyncLock
        End Get
        Set(ByVal value As pccGeoRectangle)
            SyncLock _locker
                Throw New Exception("pcc by design: MBR is a readonly property")
            End SyncLock
        End Set
    End Property

    <XmlIgnore(), SoapIgnore()>
    Public Property GetBasicGeometries As pccGeoGeometry()
        Get
            SyncLock _locker
                Return Nothing
            End SyncLock
        End Get
        Set(ByVal value As pccGeoGeometry())
            SyncLock _locker
                Throw New Exception("pcc by design: GetBasicGeometries is a readonly property")
            End SyncLock
        End Set
    End Property
    Public Function Coords() As String
        SyncLock _locker
            Dim ret As New StringBuilder(_x.ToString.Replace(",", ".") & " " & _y.ToString.Replace(",", "."))
            If _3d Then ret.Append(_z.ToString.Replace(",", "."))
            Return ret.ToString
        End SyncLock
    End Function
End Class
<DataContract()>
<KnownType(GetType(pccGeoPolygon))>
<KnownType(GetType(List(Of pccGeoPolygon)))>
<KnownType(GetType(pccGeoPoint))>
<KnownType(GetType(pccGeoPolygon))>
<KnownType(GetType(pccGeoRectangle))>
<KnownType(GetType(pccGeoLineString))>
<KnownType(GetType(pccGeoMultiPoint))>
<KnownType(GetType(pccGeoMultiPolygon))>
<KnownType(GetType(pccGeoMultiLineString))>
<KnownType(GetType(pccGeoCollection))>
<Serializable()>
Public Class pccGeoMultiPolygon
    Inherits pccGeoGeometry

    Dim gu As New pccGeoUtils
    <DataMember(Name:="Lista")>
    Private _polygons As List(Of pccGeoPolygon)
    <DataMember(Name:="Size")>
    Private _kpolygons As Integer

    Private Shared ReadOnly _locker As New Object()

    Public Sub New()
        Me.New("", "")
    End Sub

    Public Sub New(ByVal id As String, ByVal nome As String)
        MyBase.New(id, nome)
        SyncLock _locker
            _kpolygons = 0
            _polygons = New List(Of pccGeoPolygon)
        End SyncLock
    End Sub

    Public Sub AddPolygon(ByRef p As pccGeoPolygon)
        SyncLock _locker
            _kpolygons += 1
            _polygons.Add(p)
            If _kpolygons = 1 Then
                MyBase._3d = p.Is3d
            Else
                MyBase._3d = MyBase._3d And p.Is3d
            End If
            _area_calc = False
        End SyncLock
    End Sub

    Public Overrides ReadOnly Property GetGeometryType() As pccGeoGeometryType
        Get
            SyncLock _locker
                Return pccGeoGeometryType.MultiPolygon
            End SyncLock
        End Get
    End Property

    Public Overrides ReadOnly Property WKT() As String
        Get
            SyncLock _locker
                Dim ret As String = "MULTIPOLYGON("
                If _kpolygons = 0 Then
                    ret &= "null"
                Else
                    For i As Integer = 0 To _kpolygons - 1
                        If i > 0 Then ret &= ", "
                        ret &= _polygons(i).WKT
                    Next
                End If
                ret &= ")"

                ret = ret.Replace("(POLYGON", "(")
                ret = ret.Replace(" POLYGON", " ")
                _wkt = ret
                Return ret
            End SyncLock
        End Get
    End Property

    Public Overrides ReadOnly Property Is3d() As Boolean
        Get
            SyncLock _locker
                Return MyBase._3d
            End SyncLock
        End Get
    End Property

    Public Overrides ReadOnly Property Area() As Double
        Get
            SyncLock _locker
                If Not _area_calc Then
                    _area = 0
                    If _polygons IsNot Nothing Then
                        For Each p As pccGeoPolygon In _polygons
                            _area += p.Area
                        Next
                    End If
                    _area_calc = True
                End If
                Return _area
            End SyncLock
        End Get

    End Property

    Public Overrides ReadOnly Property ConvexHul() As pccGeoPolygon

        Get
            SyncLock _locker
                If Not _convexhul_calc Then
                    ' TODO: calcular convexhul
                    _convexhul_calc = True
                End If

                Return _convexhul
            End SyncLock
        End Get

    End Property

    Public Overrides ReadOnly Property Length() As Double

        Get
            SyncLock _locker
                If Not _length_calc Then
                    _length = 0
                    If _polygons IsNot Nothing Then
                        For Each p As pccGeoPolygon In _polygons
                            _length += p.Length
                        Next
                    End If
                    _length_calc = True
                End If

                Return _length
            End SyncLock
        End Get

    End Property

    Public Overrides ReadOnly Property MBR() As pccGeoRectangle

        Get
            SyncLock _locker
                If Not _mbr_calc Then
                    Dim cents As New pccGeoMultiPoint()
                    For Each pol As pccGeoPolygon In _polygons
                        For Each p As pccGeoPoint In pol.MBR.Vertices
                            cents.AddPoint(p)
                        Next
                    Next

                    gu.CalcMBR(cents.Points, _mbr)
                    _mbr_calc = True
                End If

                Return _mbr
            End SyncLock
        End Get

    End Property

    Public Overrides ReadOnly Property IsEqual(ByVal geo As pccGeoGeometry) As Boolean
        Get
            SyncLock _locker
                'TEST: Issue #9 - IsEqual
                'Ter em atenção que os vários polígonos podem não aparecer pela mesma ordem.

                If Definition() <> geo.Definition Then
                    Return False
                End If

                Dim Resultado As Boolean = True
                Dim i As Long = 0
                Dim j As Long = 0

                Dim PoligonoExterior1 As List(Of pccGeoPolygon)
                Dim PoligonoExterior2 As List(Of pccGeoPolygon)

                If _kpolygons <> CType(geo, pccGeoMultiPolygon)._kpolygons Then
                    Resultado = False
                Else

                    ' Primeiro tem que se ordenar ambos os arrays de pontos 

                    PoligonoExterior1 = New List(Of pccGeoPolygon)(_polygons)
                    PoligonoExterior2 = New List(Of pccGeoPolygon)(CType(geo, pccGeoMultiPolygon)._polygons)

                    PoligonoExterior1.Sort(AddressOf pccGeoPolygon.Compare)
                    PoligonoExterior2.Sort(AddressOf pccGeoPolygon.Compare)
                    ' Aqui ambos as listas de GeoGeoMultiPolygons estao ordenadas segundo o mesmo critério
                    ' Agora é verificar se são identicas


                    For i = 0 To PoligonoExterior1.Count - 1
                        If Not PoligonoExterior1(i).IsEqual(PoligonoExterior2(i)) Then
                            Resultado = False
                            Exit For
                        End If
                    Next

                    'For Each i1 As pccGeoGeometry In PoligonoExterior1 ' _kpolygons - 1
                    '    For Each i2 As pccGeoGeometry In PoligonoExterior2 ' CType(geo, pccGeoMultiPolygon)._kpolygons - 1
                    '        If Not i1.IsEqual(i2) Then
                    '            'If Not _polygons(i).IsEqual(CType(geo, pccGeoMultiPolygon)._polygons(j)) Then
                    '            Resultado = False
                    '            Exit For
                    '        End If
                    '    Next
                    '    If Not Resultado Then
                    '        Exit For
                    '    End If
                    'Next
                    Return Resultado
                End If
            End SyncLock
        End Get
    End Property

    <XmlIgnore(), SoapIgnore()>
    Public Overrides Property GetBasicGeometries() As pccGeoGeometry()

        Get
            SyncLock _locker
                Dim ret() As pccGeoGeometry
                ReDim ret(_polygons.Count - 1)
                Dim k As Integer = 0

                For Each g As pccGeoGeometry In _polygons
                    ret(k) = g
                    k += 1
                Next
                Return ret
            End SyncLock
        End Get

        Set(ByVal value As pccGeoGeometry())
            SyncLock _locker
                Throw New Exception("pcc by design: GetBasicGeometries is a readonly property")
            End SyncLock
        End Set

    End Property

    Public Overrides Function ToString() As String
        SyncLock _locker
            Return MyBase.ToString() & vbCrLf & " k:" & _kpolygons
        End SyncLock
    End Function

    Public Overrides Function GetMulti() As pccGeoGeometry
        SyncLock _locker
            Dim g As New pccGeoCollection
            Return g
        End SyncLock
    End Function

    Public Overrides Function AddGeometry(ByRef g As pccGeoGeometry) As Boolean
        SyncLock _locker
            Me.AddPolygon(g)
        End SyncLock
    End Function

    Public Overrides ReadOnly Property Centroid() As pccGeoCentroid

        Get
            SyncLock _locker
                If Not _centroid_calc Then

                    Dim tArea As Double = 0

                    Dim a_centroids(Me._kpolygons - 1) As pccGeoCentroid
                    Dim a_areas(Me._kpolygons - 1) As Double
                    Dim k As Integer = 0

                    For Each p As pccGeoPolygon In _polygons
                        k += 1
                        a_centroids(k - 1) = p.Centroid
                        a_areas(k - 1) = p.Area
                        tArea += a_areas(k - 1)
                    Next

                    Dim tx As Double = 0
                    Dim ty As Double = 0

                    For i As Integer = 0 To a_centroids.Length - 1
                        tx += a_areas(i) * a_centroids(i).X
                        ty += a_areas(i) * a_centroids(i).Y
                    Next

                    _centroid = New pccGeoCentroid(tx / tArea, ty / tArea, 0)
                    _centroid_calc = True

                    ' aproveita para fazer set de Length
                    If Not _area_calc Then
                        _area = tArea
                        _area_calc = True
                    End If

                End If

                Return _centroid
            End SyncLock
        End Get

    End Property

    Public Overrides Function IsMulti() As Boolean
        SyncLock _locker
            Return True
        End SyncLock
    End Function

    Public Overrides Function Definition() As String
        SyncLock _locker
            Return "MPG|" & _kpolygons.ToString & "|" & Centroid.X.ToString & "|" & Centroid.Y.ToString & "|" & Centroid.Z.ToString & "|" & Area.ToString & "|" & Length.ToString
        End SyncLock
    End Function
End Class
<DataContract()>
<KnownType(GetType(pccGeoGeometry))>
<KnownType(GetType(List(Of pccGeoGeometry)))>
<KnownType(GetType(pccGeoPoint))>
<KnownType(GetType(pccGeoPolygon))>
<KnownType(GetType(pccGeoRectangle))>
<KnownType(GetType(pccGeoLineString))>
<KnownType(GetType(pccGeoMultiPoint))>
<KnownType(GetType(pccGeoMultiPolygon))>
<KnownType(GetType(pccGeoMultiLineString))>
<KnownType(GetType(pccGeoCollection))>
<Serializable()>
Public Class pccGeoCollection
    Inherits pccGeoGeometry

    Dim gu As New pccGeoUtils
    <DataMember(Name:="Lista")>
    Private _geometries As List(Of pccGeoGeometry)
    <DataMember(Name:="Size")>
    Private _kgeometries As Integer

    Private Shared ReadOnly _locker As New Object()

    Public Sub New()
        Me.New("", "")
    End Sub

    Public Sub New(ByVal id As String, ByVal nome As String)
        MyBase.New(id, nome)
        SyncLock _locker
            _kgeometries = 0
            _geometries = New List(Of pccGeoGeometry)
        End SyncLock
    End Sub

    Public Sub New(ByRef conn As pccDB4.Connection, ByVal id_geometry As String, ByVal storage_table As String)
        Me.New("", "")
        SyncLock _locker
            Dim gu As New pccGeoUtils
            Dim g As New pccGeoCollection
            g = gu.GeometryById(conn, id_geometry, storage_table)
            For Each og As pccGeoGeometry In g.GetBasicGeometries
                Me.AddGeometry(og)
            Next
        End SyncLock
    End Sub

    Public Sub AddOldGeometry(ByRef g As pccGeoGeometry)
        SyncLock _locker
            _kgeometries += 1
            _geometries.Add(g)
            If _kgeometries = 1 Then
                MyBase._3d = g.Is3D
            Else
                MyBase._3d = MyBase._3d And g.Is3D
            End If
            _area_calc = False
        End SyncLock
    End Sub

    Public Overrides ReadOnly Property GetGeometryType() As pccGeoGeometryType
        Get
            SyncLock _locker
                Return pccGeoGeometryType.Collection
            End SyncLock
        End Get
    End Property

    Public Overrides ReadOnly Property WKT() As String
        Get
            SyncLock _locker
                Dim ret As String = ""
                If _kgeometries = 0 Then
                    ret &= "null"
                Else
                    For Each g As pccGeoGeometry In _geometries
                        If ret = "" Then
                            ret = "GEOMETRYCOLLECTION("
                        Else
                            ret &= ","
                        End If
                        ret &= g.WKT
                    Next
                End If
                ret &= ")"
                _wkt = ret
                Return ret
            End SyncLock
        End Get


    End Property

    Public Overrides ReadOnly Property Is3d() As Boolean
        Get
            SyncLock _locker
                Return MyBase._3d
            End SyncLock
        End Get
    End Property

    Public Overrides ReadOnly Property Area() As Double
        Get
            SyncLock _locker
                If Not _area_calc Then
                    _area = 0
                    If _geometries IsNot Nothing Then
                        For Each p As pccGeoGeometry In _geometries
                            _area += p.Area
                        Next
                    End If
                    _area_calc = True
                End If
                Return _area
            End SyncLock
        End Get

    End Property

    <XmlIgnore(), SoapIgnore()>
    Public Overrides ReadOnly Property ConvexHul() As pccGeoPolygon
        Get
            SyncLock _locker
                If Not _convexhul_calc Then
                    ' TODO: calcular convexhul
                    _convexhul_calc = True
                End If
                Return _convexhul
            End SyncLock
        End Get
    End Property

    Public Overrides ReadOnly Property Length() As Double
        Get
            SyncLock _locker
                If Not _length_calc Then
                    _length = 0
                    If _geometries IsNot Nothing Then
                        For Each p As pccGeoGeometry In _geometries
                            _length += p.Length
                        Next
                    End If
                    _length_calc = True
                End If
                Return _length
            End SyncLock
        End Get

    End Property

    Public Overrides ReadOnly Property MBR() As pccGeoRectangle
        Get
            SyncLock _locker
                If Not _mbr_calc Then
                    Dim cents As New pccGeoMultiPoint()
                    For Each pol As pccGeoGeometry In _geometries
                        Dim pol2 As New pccGeoRectangle
                        Dim xx As pccGeoGeometry
                        xx = pol
                        pol2 = xx.MBR
                        For Each p As pccGeoPoint In pol2.Vertices
                            cents.AddPoint(p)
                        Next
                    Next
                    gu.CalcMBR(cents.Points, _mbr)
                    _mbr_calc = True
                End If
                Return _mbr
            End SyncLock
        End Get

    End Property

    Public Overrides ReadOnly Property IsEqual(ByVal geo As pccGeoGeometry) As Boolean
        Get
            SyncLock _locker
                'TEST: Issue #10 - IsEqual

                If Definition() <> geo.Definition Then
                    Return False
                End If

                'Ter em atenção que as várias linhas podem não aparecer pela mesma ordem.
                Dim Resultado As Boolean = True
                Dim i As Long = 0
                Dim j As Long = 0

                Dim Coleccao1 As List(Of pccGeoGeometry)
                Dim Coleccao2 As List(Of pccGeoGeometry)

                If _kgeometries <> CType(geo, pccGeoCollection)._kgeometries Then
                    Resultado = False
                Else

                    ' Primeiro tem que se ordenar ambos os arrays de geometrias 

                    Coleccao1 = New List(Of pccGeoGeometry)(_geometries)
                    Coleccao2 = New List(Of pccGeoGeometry)(CType(geo, pccGeoCollection)._geometries)

                    Coleccao1.Sort(AddressOf pccGeoGeometry.Compare)
                    Coleccao2.Sort(AddressOf pccGeoGeometry.Compare)

                    ' Aqui ambos as listas estão ordenadas segundo o mesmo critério
                    ' Agora é verificar se são identicas

                    For Each i1 As pccGeoGeometry In Coleccao1
                        For Each i2 As pccGeoGeometry In Coleccao2
                            If Not i1.IsEqual(i2) Then
                                Resultado = False
                                Exit For
                            End If
                        Next
                        If Not Resultado Then Exit For
                    Next
                    Return Resultado
                End If
            End SyncLock
        End Get

    End Property

    <XmlIgnore(), SoapIgnore()>
    Public Overrides Property GetBasicGeometries() As pccGeoGeometry()

        Get
            SyncLock _locker
                ' TODO: verificar se é multi, isto é, se todas as geometrias são do mesmo tipo 

                Dim type As System.Type = Nothing
                Dim sameType As Boolean = False
                Dim g() As pccGeoGeometry = Nothing
                Dim kg As Integer = 0

                If _geometries IsNot Nothing Then
                    For Each po As pccGeoGeometry In _geometries
                        If kg = 0 Then
                            type = po.GetType
                            sameType = True
                        Else
                            If po.GetType IsNot type Then
                                sameType = False
                            End If
                        End If
                        ReDim Preserve g(kg)
                        g(kg) = po
                        kg += 1
                    Next

                End If

                ' são todos do mesmo tipo, pode retornar o MULTI respectivo
                If sameType And _kgeometries > 1 Then
                    Dim mg(0) As pccGeoGeometry
                    mg(0) = _geometries(0).GetMulti()

                    For Each ug As pccGeoGeometry In _geometries
                        mg(0).AddGeometry(ug)
                    Next
                    Return mg
                Else
                    Return g
                End If
            End SyncLock
        End Get
        Set(ByVal value As pccGeoGeometry())
            SyncLock _locker
                Throw New Exception("pcc by design: GetBasicGeometries is a readonly property")
            End SyncLock
        End Set

    End Property

    Public Overrides Function ToString() As String
        SyncLock _locker
            Return MyBase.ToString() & vbCrLf & " k:" & _kgeometries
        End SyncLock
    End Function

    Public Overrides Function Save(ByRef conn As pccDB4.Connection, ByVal trans As IDbTransaction, ByVal id_entidade As String, ByVal storage_table As String, ByVal isUpdate As Boolean) As Boolean
        SyncLock _locker
            For Each p As pccGeoGeometry In _geometries
                If Not p.Save(conn, trans, id_entidade, storage_table, isUpdate) Then
                    Return False
                End If
            Next

            Return True
        End SyncLock
    End Function

    Public Overrides Function GetMulti() As pccGeoGeometry
        SyncLock _locker
            Dim g As New pccGeoCollection
            Return g
        End SyncLock
    End Function

    Public Overrides Function AddGeometry(ByRef g As pccGeoGeometry) As Boolean
        _kgeometries += 1
        If _kgeometries = 1 Then
            MyBase._3d = g.Is3D
        Else
            MyBase._3d = MyBase._3d And g.Is3D
        End If
        _geometries.Add(g)
        _area_calc = False
    End Function

    Public Overrides ReadOnly Property Centroid() As pccGeoCentroid
        Get
            SyncLock _locker
                'TODO: Testar -> Implementar centroide Testado: Jaugusto, mas o centroide nao tem z nunca!!!!

                If Not _centroid_calc Then

                    Dim mp As New pccGeoMultiPoint

                    For Each objecto As pccGeoGeometry In _geometries
                        mp.AddPoint(New pccGeoPoint(objecto.Centroid.X, objecto.Centroid.Y, objecto.Centroid.Z))
                    Next

                    _centroid = New pccGeoCentroid(mp.Centroid.X, mp.Centroid.Y, mp.Centroid.Z)
                    _centroid_calc = True

                End If
                Return _centroid
            End SyncLock
        End Get

    End Property

    Public Overrides Function IsMulti() As Boolean
        SyncLock _locker
            Return True
        End SyncLock
    End Function

    Public Overrides Function Definition() As String
        SyncLock _locker
            Return "COL|" & _kgeometries.ToString & "|" & Centroid.X.ToString & "|" & Centroid.Y.ToString & "|" & Centroid.Z.ToString & "|" & Area.ToString & "|" & Length.ToString
        End SyncLock
    End Function

End Class

