Imports System.Xml.Serialization
Imports System.IO

Imports System.Text
Imports pccDB4
Imports pccGeoMetadata4


<Serializable()> _
Public Enum pccGeov0GeometryType
    Point = 1
    LineString = 2
    Polygon = 3
    MultiPoint = 10
    MultiLineString = 20
    MultiPolygon = 30
    Collection = 255
End Enum
Public Class pccGeov0Utils

    Public Function GetStringFromMemoryStream(ByVal m As MemoryStream) As String

        If (m Is Nothing Or m.Length = 0) Then
            Return ""
        End If

        m.Flush()
        m.Position = 0
        Dim sr As StreamReader = New StreamReader(m)
        Dim s As String = sr.ReadToEnd()

        Return s

    End Function

    Public Function GetMemoryStreamFromString(ByVal s As String) As MemoryStream

        If (s = Nothing Or s.Length = 0) Then
            Return Nothing
        End If

        Dim m As MemoryStream = New MemoryStream()
        Dim sw As StreamWriter = New StreamWriter(m)
        sw.Write(s)
        sw.Flush()

        Return m

    End Function

    Public Function CoordsValid(ByVal coords() As String) As Boolean

        For i As Integer = 0 To coords.GetUpperBound(0)
            If Not IsNumeric(coords(i)) Then
                Return False
            End If
        Next

        Return True

    End Function
    Public Function CalcLength(ByRef points As pccGeov0Point()) As Double
        Dim ret As Double = 0

        For i As Integer = 0 To points.GetLength(0) - 2
            ret += Distance(points(i), points(i + 1))
        Next

        Return ret

    End Function
    Public Function CalcLenght(ByRef points As List(Of pccGeov0Point)) As Double
        Dim ret As Double = 0

        For i As Integer = 0 To points.Count - 2
            ret += Distance(points(i), points(i + 1))
        Next

        Return ret
    End Function

    Public Function CalcArea(ByRef points As pccGeov0Point()) As Double

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

    End Function

    'TODO: Testar -> List é coisa nova por aqui Testado: Jaugusto
    Public Function CalcArea(ByRef points As List(Of pccGeov0Point)) As Double

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

    End Function

    Public Sub CalcMBR(ByRef points() As pccGeov0Point, ByRef destpolygon As pccGeov0Rectangle)

        Dim destmbr(3) As pccGeov0Point

        Dim minx As Double = 9999999999
        Dim maxx As Double = -9999999999
        Dim miny As Double = 9999999999
        Dim maxy As Double = -9999999999

        For Each p As pccGeov0Point In points
            If p.X < minx Then minx = p.X
            If p.X > maxx Then maxx = p.X
            If p.Y < miny Then miny = p.Y
            If p.Y > maxy Then maxy = p.Y
        Next

        destmbr(0) = New pccGeov0Point(minx, miny)
        destmbr(1) = New pccGeov0Point(maxx, miny)
        destmbr(2) = New pccGeov0Point(maxx, maxy)
        destmbr(3) = New pccGeov0Point(minx, maxy)

        destpolygon = New pccGeov0Rectangle
        destpolygon.AddVertice(destmbr(0))
        destpolygon.AddVertice(destmbr(1))
        destpolygon.AddVertice(destmbr(2))
        destpolygon.AddVertice(destmbr(3))
        destpolygon.AddVertice(destmbr(0))

    End Sub

    'TODO: Testar -> List é coisa nova por aqui Testado: Jaugusto
    Public Sub CalcMBR(ByRef points As List(Of pccGeov0Point), ByRef destpolygon As pccGeov0Rectangle)

        Dim destmbr(3) As pccGeov0Point

        Dim minx As Double = 9999999999
        Dim maxx As Double = -9999999999
        Dim miny As Double = 9999999999
        Dim maxy As Double = -9999999999

        For Each p As pccGeov0Point In points
            If p.X < minx Then minx = p.X
            If p.X > maxx Then maxx = p.X
            If p.Y < miny Then miny = p.Y
            If p.Y > maxy Then maxy = p.Y
        Next

        destmbr(0) = New pccGeov0Point(minx, miny)
        destmbr(1) = New pccGeov0Point(maxx, miny)
        destmbr(2) = New pccGeov0Point(maxx, maxy)
        destmbr(3) = New pccGeov0Point(minx, maxy)

        destpolygon = New pccGeov0Rectangle
        destpolygon.AddVertice(destmbr(0))
        destpolygon.AddVertice(destmbr(1))
        destpolygon.AddVertice(destmbr(2))
        destpolygon.AddVertice(destmbr(3))
        destpolygon.AddVertice(destmbr(0))

    End Sub

    Public Function GeometryById(ByRef conn As pccDB4.Connection, ByVal id_entidade As String, ByVal storage_table As String) As Object

        Dim com As pccDB4.Command = Nothing
        Dim par As IDbDataParameter = Nothing
        Dim iReader As IDataReader = Nothing
        Dim ret As New pccGeov0Collection

        Try

            com = conn.CreateCommand

            com.CommandText = "select " + com.GetFunction(ProviderFunctions.GeoToWKT, "GEOM", "G", "") + " from " + storage_table + " G WHERE entgeom_id IN (SELECT REC_ID FROM ENTIDADES_" + storage_table + " WHERE ENTIDADE_id=?)"
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

    End Function

    Public Function GeometryConvexHullById(ByRef conn As pccDB4.Connection, ByVal id_entidade As String) As pccGeov0Polygon
        ' TODO: Issue #14 - implementar GeometryConvexHullById
        ' ver GeometryCentroidById embora a técnica da "média dos pontos" usada lá não se possa aplicar aqui.
        Return New pccGeov0Polygon()
    End Function

    Public Function GeometryCentroidById(ByRef conn As pccDB4.Connection, ByVal id_entidade As String, ByVal storage_table As String) As pccGeov0Point

        Dim com As pccDB4.Command = Nothing
        Dim par As IDbDataParameter = Nothing
        Dim iReader As IDataReader = Nothing
        Dim tmppoint As pccGeov0Point = Nothing
        Dim ret As pccGeov0Point = Nothing

        Dim X As Double = 0
        Dim Y As Double = 0
        Dim k As Long = 0

        Try

            com = conn.CreateCommand

            com.CommandText = "select " + com.GetFunction(ProviderFunctions.GeoToWKT, com.GetFunction(ProviderFunctions.Centroid, "geom", "G", "")) + " from " + storage_table + " G WHERE entgeom_id IN (SELECT rec_id FROM entidades_" + storage_table + " WHERE entidade_id=?)"
            com.CommandType = CommandType.Text

            par = com.CreateParameter(DbType.String, id_entidade)
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
                ret = New pccGeov0Point(X / k, Y / k)
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

    End Function

    ' Funções que transformam pccGeov0Polygon e pccGeov0MultiPolygon em Clipper.TDoublePoint
    ' para utilização das funções de intersecção de polígonos
    Public Function ClipperPolygonFromPolygon(ByRef pol As pccGeov0Polygon) As List(Of List(Of Clipper.TDoublePoint))

        Dim res As New List(Of List(Of Clipper.TDoublePoint))
        res.Add(ClipperPolygonFromOnePolygon(pol))

        For Each p As pccGeov0Polygon In pol.InnerPolygons
            res.Add(ClipperPolygonFromOnePolygon(p))
        Next

        Return res
    End Function

    Public Function ClipperPolygonFromMultiPolygon(ByRef pol As pccGeov0MultiPolygon) As List(Of List(Of Clipper.TDoublePoint))

        Dim res As New List(Of List(Of Clipper.TDoublePoint))

        For Each p As pccGeov0Polygon In pol.GetBasicGeometries
            res.Add(ClipperPolygonFromOnePolygon(p))
        Next

        Return res

    End Function

    Private Function ClipperPolygonFromOnePolygon(ByRef pol As pccGeov0Polygon) As List(Of Clipper.TDoublePoint)

        Dim res As New List(Of Clipper.TDoublePoint)

        For Each p As pccGeov0Point In pol.Vertices
            res.Add(New Clipper.TDoublePoint(p.X, p.Y))
        Next

        Return res

    End Function

    Public Function GeometryFromWKT(ByVal wkt As String) As Object

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
                        geo = New pccGeov0Point(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)))

                    ElseIf acoords.Length = 3 Then
                        geo = New pccGeov0Point(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)), CDbl(acoords(2).Replace(separadormilhares, separadordecimal)))

                    Else
                        geo = Nothing
                    End If

                    Return geo

                Case "linestring"

                    geo = New pccGeov0LineString()
                    Dim px As Integer = wkt.IndexOf("(") + 1
                    points = wkt.Substring(px, wkt.IndexOf(")", px) - px)

                    apoints = points.Split(",")

                    For i As Integer = 0 To apoints.Length - 1

                        acoords = apoints(i).Trim.Split(" ")
                        Dim auxgeo As pccGeov0Point

                        If acoords.Length = 2 Then
                            auxgeo = New pccGeov0Point(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)))
                        ElseIf acoords.Length = 3 Then
                            auxgeo = New pccGeov0Point(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)), CDbl(acoords(2).Replace(separadormilhares, separadordecimal)))
                        Else
                            auxgeo = Nothing
                        End If

                        If auxgeo IsNot Nothing Then
                            CType(geo, pccGeov0LineString).AddVertice(auxgeo)
                        End If

                    Next

                    Return geo

                Case "polygon"

                    geo = New pccGeov0Polygon
                    Dim outter As New pccGeov0Polygon
                    Dim inner As New pccGeov0Polygon
                    Dim AuxPol() As pccGeov0Polygon = Nothing
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
                        AuxPol(kAuxPol) = New pccGeov0Polygon
                        kAuxPol += 1

                        For j As Integer = 0 To apoints.Length - 1

                            acoords = apoints(j).Trim.Split(" ")
                            Dim auxgeo As pccGeov0Point

                            If acoords.Length = 2 Then
                                auxgeo = New pccGeov0Point(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)))
                            ElseIf acoords.Length = 3 Then
                                auxgeo = New pccGeov0Point(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)), CDbl(acoords(2).Replace(separadormilhares, separadordecimal)))
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
                        For Each v As pccGeov0Point In Invert(AuxPol(iPolMaior).Vertices)
                            CType(geo, pccGeov0Polygon).AddVertice(v)
                        Next
                    Else
                        For Each v As pccGeov0Point In AuxPol(iPolMaior).Vertices
                            CType(geo, pccGeov0Polygon).AddVertice(v)
                        Next
                    End If

                    ' TODO: rever a questão dos inners não trata inners
                    For j As Integer = 0 To AuxPol.GetLength(0) - 1
                        If j <> iPolMaior Then

                            If AuxPol(j).Area < -10 Then
                                Dim pp As New pccGeov0Polygon
                                For Each v As pccGeov0Point In Invert(AuxPol(j).Vertices)
                                    CType(pp, pccGeov0Polygon).AddVertice(v)
                                Next
                                CType(geo, pccGeov0Polygon).AddInnerPolygon(pp)
                            Else
                                If AuxPol(j).Area >= 10 Then
                                    CType(geo, pccGeov0Polygon).AddInnerPolygon(AuxPol(j))
                                End If
                            End If

                        End If

                    Next

                    Return geo

                Case "multipoint"

                    geo = New pccGeov0MultiPoint()
                    Dim px As Integer = wkt.IndexOf("(") + 1

                    points = wkt.Substring(px, wkt.IndexOf(")", px) - px)
                    apoints = points.Split(",")

                    For i As Integer = 0 To apoints.Length - 1
                        acoords = apoints(i).Trim.Split(" ")
                        Dim auxgeo As pccGeov0Point

                        If acoords.Length = 2 Then
                            auxgeo = New pccGeov0Point(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)))
                        ElseIf acoords.Length = 3 Then
                            auxgeo = New pccGeov0Point(CDbl(acoords(0).Replace(separadormilhares, separadordecimal)), CDbl(acoords(1).Replace(separadormilhares, separadordecimal)), CDbl(acoords(2).Replace(separadormilhares, separadordecimal)))
                        Else
                            auxgeo = Nothing
                        End If

                        If auxgeo IsNot Nothing Then
                            CType(geo, pccGeov0MultiPoint).AddPoint(auxgeo)
                        End If

                    Next

                    Return geo

                Case "multilinestring"

                    geo = New pccGeov0MultiLineString

                    Dim aux1 As New pccString4.pccString4(wkt)
                    Dim px As Integer = wkt.IndexOf("(") + 1
                    Dim aux2 As New pccString4.pccString4(wkt.Substring(px, aux1.IndexOf(")", "(", ")", px) - px))

                    Dim aaux As String()
                    aaux = aux2.Split(",", "(", ")")

                    For i As Integer = 0 To aaux.Length - 1
                        Dim ml As New pccGeov0LineString
                        ml = GeometryFromWKT("linestring" & Trim(aaux(i)))
                        If ml IsNot Nothing Then CType(geo, pccGeov0MultiLineString).AddLineString(ml)
                    Next

                    Return geo

                Case "multipolygon"

                    geo = New pccGeov0MultiPolygon

                    Dim aux1 As New pccString4.pccString4(wkt)
                    Dim px As Integer = wkt.IndexOf("(") + 1
                    Dim aux2 As New pccString4.pccString4(wkt.Substring(px, aux1.IndexOf(")", "(", ")", px) - px))

                    Dim aaux As String()
                    aaux = aux2.Split(",", "(", ")")

                    For i As Integer = 0 To aaux.Length - 1
                        Dim pl As New pccGeov0Polygon
                        pl = GeometryFromWKT("polygon" & Trim(aaux(i)))
                        If pl IsNot Nothing Then CType(geo, pccGeov0MultiPolygon).AddPolygon(pl)
                    Next

                    Return geo

                Case "geometrycollection"

                    geo = New pccGeov0Collection

                    Dim aux1 As New pccString4.pccString4(wkt)
                    Dim px As Integer = wkt.IndexOf("(") + 1
                    Dim aux2 As New pccString4.pccString4(wkt.Substring(px, aux1.IndexOf(")", "(", ")", px) - px))

                    Dim aaux As String()
                    aaux = aux2.Split(",", "(", ")")

                    For i As Integer = 0 To aaux.Length - 1
                        Dim ge As Object
                        ge = GeometryFromWKT(Trim(aaux(i)))
                        If ge IsNot Nothing Then CType(geo, pccGeov0Collection).AddGeometry(ge)
                    Next

                    Return geo

                Case Else

                    Return Nothing

            End Select

        Catch ex As Exception

            Return Nothing

        End Try

    End Function

    Public Function Invert(ByRef points As pccGeov0Point()) As pccGeov0Point()

        Dim ix As Integer = points.GetLength(0) - 1

        Dim ret(ix) As pccGeov0Point

        For i As Integer = 0 To ix
            ret(i) = points(ix - i)
        Next

        Return ret

    End Function

    Public Function Invert(ByRef points As List(Of pccGeov0Point)) As pccGeov0Point()

        Dim ix As Integer = points.Count - 1

        Dim ret(ix) As pccGeov0Point

        For i As Integer = 0 To ix
            ret(i) = points(ix - i)
        Next

        Return ret

    End Function

    Public Function Distance(ByRef p1 As pccGeov0Point, ByRef p2 As pccGeov0Point) As Double

        Dim d As Double = 0

        If p1.Is3d And p2.Is3d Then
            d = Math.Sqrt((p1.X - p2.X) ^ 2 + (p1.Y - p2.Y) ^ 2 + (p1.Z - p2.Z) ^ 2)
        Else
            d = Math.Sqrt((p1.X - p2.X) ^ 2 + (p1.Y - p2.Y) ^ 2)
        End If

        Return d

    End Function

    Public Function Simplify(ByRef points As pccGeov0Point(), ByVal tolerance As Double) As pccGeov0Point()

        Dim closed As Boolean = points(points.GetLowerBound(0)).IsEqual(points(points.GetUpperBound(0)))

        Dim ix As Integer = 0
        Dim len As Integer = points.GetLength(0)
        Dim ret(ix) As pccGeov0Point

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

    End Function


    Public Function Simplify(ByRef points As List(Of pccGeov0Point), ByVal tolerance As Double) As pccGeov0Point()

        Dim closed As Boolean = points(0).IsEqual(points(points.Count - 1))

        Dim ix As Integer = 0
        Dim len As Integer = points.Count
        Dim ret(ix) As pccGeov0Point

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

    End Function


    ' retorna o tipo de geometria do WKT: POINT, LINESTRING, POLYGON, ETC
    Private Function GeometryString(ByVal wkt As String) As String

        Dim p As Integer
        p = wkt.IndexOf("(")
        If p > 0 Then
            Return wkt.Substring(0, p).Trim.ToLower
        Else
            Return ""
        End If

    End Function

    Public Sub New()

    End Sub
End Class

<Serializable()>
<XmlInclude(GetType(pccGeov0Point))>
<XmlInclude(GetType(pccGeov0Polygon))>
<XmlInclude(GetType(pccGeov0Rectangle))>
<XmlInclude(GetType(pccGeov0LineString))>
<XmlInclude(GetType(pccGeov0MultiPoint))>
<XmlInclude(GetType(pccGeov0MultiPolygon))>
<XmlInclude(GetType(pccGeov0MultiLineString))>
<XmlInclude(GetType(pccGeov0Collection))>
Public MustInherit Class pccGeov0Geometry

    Protected _catalogo As pccGeov0Catalog 'friend 
    Protected _3d As Boolean
    Protected _length As Double
    Protected _area As Double
    Protected _convexhul As pccGeov0Polygon
    Protected _mbr As pccGeov0Polygon
    Protected _centroid As pccGeov0Point
    Protected _length_calc As Boolean 'friend 
    Protected _area_calc As Boolean 'friend 
    Protected _convexhul_calc As Boolean 'friend 
    Protected _centroid_calc As Boolean 'friend 
    Protected _mbr_calc As Boolean 'friend 
    Protected _wktfromdb As String 'friend 
    Protected _onetable As Boolean

    Public MustOverride Function IsMulti() As Boolean

    Public MustOverride Function Definition() As String

    Public Sub New()
        Me.New("", "")
        _length_calc = False
        _area_calc = False
        _convexhul_calc = False
        _mbr_calc = False
        _centroid_calc = False
    End Sub

    Public Sub New(ByVal id As String, ByVal nome As String)
        If id = "" And nome = "" Then
            Me._catalogo = Nothing
        Else
            Me._catalogo = New pccGeov0Catalog(nome)
        End If
        _3d = False
    End Sub

    Public Property Catalogo() As pccGeov0Catalog
        Get
            Return _catalogo
        End Get
        Set(ByVal value As pccGeov0Catalog)
            _catalogo = value
        End Set
    End Property

    Public ReadOnly Property Id() As String
        Get
            If _catalogo Is Nothing Then
                Return ""
            Else
                Return _catalogo.Id
            End If
        End Get
    End Property

    Public ReadOnly Property Nome() As String
        Get
            If _catalogo Is Nothing Then
                Return ""
            Else
                Return _catalogo.Nome
            End If
        End Get
    End Property

    Public Property WKTfromDB() As String
        Get
            Return _wktfromdb
        End Get
        Set(ByVal value As String)
            _wktfromdb = value
        End Set
    End Property


    Public Shared Function Compare(ByVal p1 As pccGeov0Geometry, ByVal p2 As pccGeov0Geometry) As Integer

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

    End Function


    Public Overridable Function Delete(ByRef conn As pccDB4.Connection, ByVal trans As IDbTransaction, ByVal entidade_id As String, ByVal storage_table As String) As Boolean

        Dim com As pccDB4.Command
        Dim par As IDataParameter
        Dim iReader As IDataReader = Nothing

        Try

            ' obter geometrias desta entidade
            Dim sIDs As String = ""

            com = conn.CreateCommand
            If trans IsNot Nothing Then com.Transaction = trans

            com.CommandType = CommandType.Text
            com.CommandText = "select " + com.GetFunction(pccDB4.ProviderFunctions.Convert, "geo_id", "eg", "char(36)") + " from entidades_" + storage_table + " where entidade_id=?"

            par = com.CreateParameter(DbType.String, entidade_id)
            com.AddParameter(par)

            iReader = com.ExecuteReader()

            While iReader.Read
                If sIDs.Length > 0 Then sIDs += ","
                sIDs += "'" + iReader.GetString(0) + "'"
            End While

            iReader.Close()

            If sIDs.Length > 0 Then

                com = conn.CreateCommand
                If trans IsNot Nothing Then com.Transaction = trans

                com.CommandType = CommandType.Text
                com.CommandText = "delete from " + storage_table + " where rec_id in (" + sIDs + ")"

                par = com.CreateParameter(DbType.String, entidade_id)
                com.AddParameter(par)

                com.ExecuteNonQuery()

            End If

            com = conn.CreateCommand
            If trans IsNot Nothing Then com.Transaction = trans

            com.CommandType = CommandType.Text
            com.CommandText = "delete from entidades_" + storage_table + " where entidade_id=?"

            par = com.CreateParameter(DbType.String, entidade_id)
            com.AddParameter(par)

            com.ExecuteNonQuery()

            If iReader IsNot Nothing Then
                If Not iReader.IsClosed Then iReader.Close()
            End If

            Return True

        Catch ex As Exception
            If iReader IsNot Nothing Then
                If Not iReader.IsClosed Then iReader.Close()
            End If

            Return False

        End Try

        Return True

    End Function

    Public Overridable Function Save(ByRef conn As pccDB4.Connection, ByVal trans As IDbTransaction, ByVal geo_recid As String, ByVal storage_table As String) As Boolean

        ' nada a fazer
        ' aqui deve fazer-se save de algo relacionado com geometria mas que seja comum a qualquer tipo de geometria (point, linestring, ... etc)
        Return True

    End Function

    Public MustOverride ReadOnly Property GetGeometryType() As pccGeov0GeometryType
    Public MustOverride Property WKT() As String
    Public MustOverride Function AddGeometry(ByRef g As pccGeov0Geometry) As Boolean
    Public MustOverride ReadOnly Property Is3D() As Boolean
    Public MustOverride ReadOnly Property Length() As Double
    Public MustOverride ReadOnly Property Area() As Double
    <XmlIgnore(), SoapIgnore()>
    Public MustOverride ReadOnly Property ConvexHul() As pccGeov0Polygon
    <XmlIgnore(), SoapIgnore()>
    Public MustOverride ReadOnly Property Centroid() As pccGeov0Point
    Public MustOverride ReadOnly Property MBR() As pccGeov0Rectangle
    Public MustOverride ReadOnly Property IsEqual(ByVal geo As pccGeov0Geometry) As Boolean
    <XmlIgnore(), SoapIgnore()>
    Public MustOverride Property GetBasicGeometries() As pccGeov0Geometry()

    ' Retorna um objecto de tipo Multi respectico:
    ' Ex: um pccGeov0Point.GetMulti() retorna um new pccGeov0MultiPoint
    Public MustOverride Function GetMulti() As pccGeov0Geometry

End Class

<Serializable()>
<XmlInclude(GetType(pccGeov0Point))>
<XmlInclude(GetType(pccGeov0Polygon))>
<XmlInclude(GetType(pccGeov0LineString))>
<XmlInclude(GetType(pccGeov0Rectangle))>
<XmlInclude(GetType(pccGeov0MultiPoint))>
<XmlInclude(GetType(pccGeov0MultiPolygon))>
<XmlInclude(GetType(pccGeov0MultiLineString))>
<XmlInclude(GetType(pccGeov0Collection))>
Public Class pccGeov0Catalog

    Private _nome As String
    Private _id As String
    Private _objecto As Object
    Private _metadata As mtdMetadata

    Public Sub New()
        Me.New("")
    End Sub

    Public Sub New(ByVal p_nome As String)
        _id = Guid.NewGuid.ToString
        _nome = p_nome
    End Sub

    Public Property Nome() As String
        Get
            Return _nome
        End Get
        Set(ByVal value As String)
            _nome = value
        End Set
    End Property

    Public Property Id() As String
        Get
            Return _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Public Property Objecto() As Object
        Get
            Return _objecto
        End Get
        Set(ByVal value As Object)
            _objecto = value
        End Set
    End Property

    Public Property Metadata As mtdMetadata
        Get
            Return _metadata
        End Get
        Set(ByVal value As mtdMetadata)
            _metadata = value
        End Set
    End Property

    ' TODO: getbyid do catalogo
    Public Function GetById(ByRef conn As pccDB4.Connection, ByVal p_id As String) As Boolean

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
    End Function


End Class

Public Class pccGeov0Layer
    Inherits pccGeov0Catalog

    Public Sub New()

    End Sub

    Public Overrides Function ToString() As String

        Return "Layer:" & Me.Nome

    End Function

End Class

<Serializable()>
Public Class pccGeov0Point
    Inherits pccGeov0Geometry

    Private _x As Double
    Private _y As Double
    Private _z As Double

    Public Sub New()
        Me.New("", "")
    End Sub

    Public Sub New(ByVal id As String, ByVal nome As String, ByVal x As Double, ByVal y As Double)
        Me.New(id, nome, x, y, 0)
        MyBase._3d = False
    End Sub

    Public Sub New(ByVal id As String, ByVal nome As String)
        Me.New(id, nome, 0, 0, 0)
    End Sub

    Public Sub New(ByVal x As Double, ByVal y As Double)
        Me.New("", "", x, y, 0)
        MyBase._3d = False
    End Sub

    Public Sub New(ByVal x As Double, ByVal y As Double, ByVal z As Double)
        Me.New("", "", x, y, z)
        MyBase._3d = True
    End Sub

    Public Sub New(ByVal id As String, ByVal nome As String, ByVal x As Double, ByVal y As Double, ByVal z As Double)
        MyBase.New(id, nome)
        _x = x
        _y = y
        _z = z
        MyBase._3d = True
    End Sub

    Public Property X() As Double
        Get
            Return _x
        End Get
        Set(ByVal value As Double)
            _x = value
        End Set
    End Property

    Public Property Y() As Double
        Get
            Return _y
        End Get
        Set(ByVal value As Double)
            _y = value
        End Set
    End Property

    Public Property Z() As Double
        Get
            If Not Me.Is3d Then
                Return 0
            Else
                Return _z
            End If
        End Get
        Set(ByVal value As Double)
            If Not Me.Is3d Then
                _z = value
            Else
                _z = value
            End If
        End Set
    End Property

    Public Function Coords() As String
        Dim ret As New StringBuilder(_x.ToString.Replace(",", ".") & " " & _y.ToString.Replace(",", "."))
        If MyBase._3d Then ret.Append(_z.ToString.Replace(",", "."))
        Return ret.ToString
    End Function

    Public Overrides ReadOnly Property GetGeometryType() As pccGeov0GeometryType
        Get
            Return pccGeov0GeometryType.Point
        End Get
    End Property

    Public Overrides Property WKT() As String
        Get
            Dim ret As String = "POINT(" & Coords() & ")"
            Return ret
        End Get
        Set(ByVal value As String)
            Throw New Exception("pcc by design: WKT is a readonly property")
        End Set
    End Property


    Public Overrides ReadOnly Property Is3d() As Boolean
        Get
            Return MyBase._3d
        End Get
    End Property

    Public Overrides ReadOnly Property Area() As Double
        Get
            Return 0
        End Get
    End Property

    <XmlIgnore()>
    Public Overrides ReadOnly Property ConvexHul() As pccGeov0Polygon
        Get
            Return Nothing
        End Get
    End Property

    Public Overrides ReadOnly Property Length() As Double
        Get
            Return 0
        End Get
    End Property

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Overrides ReadOnly Property MBR() As pccGeov0Rectangle
        Get
            Return Nothing
        End Get
    End Property

    Public Overrides ReadOnly Property IsEqual(ByVal geo As pccGeov0Geometry) As Boolean
        Get

            If Definition() <> geo.Definition Then
                Return False
            End If

            If Me.X = CType(geo, pccGeov0Point).X And Me.Y = CType(geo, pccGeov0Point).Y And Me.Z = CType(geo, pccGeov0Point).Z Then

                'TEST: Issue #4 - IsEqual
                Return True
            Else
                Return False
            End If

        End Get
    End Property

    <XmlIgnore(), SoapIgnore()>
    Public Overrides Property GetBasicGeometries() As pccGeov0Geometry()

        Get
            Dim g(0) As pccGeov0Point
            g(0) = New pccGeov0Point()
            g(0) = Me
            Return g
        End Get

        Set(ByVal value As pccGeov0Geometry())
            Throw New Exception("pcc by design: GetBasicGeometries is a readonly property")
        End Set

    End Property

    Public Overrides Function ToString() As String

        Return MyBase.ToString() & vbCrLf & " x:" & _x & " y:" & _y & " z:" & _z

    End Function

    Public Overrides Function Save(ByRef conn As pccDB4.Connection, ByVal trans As IDbTransaction, ByVal id_entidade As String, ByVal storage_table As String) As Boolean

        Dim com As pccDB4.Command
        Dim geoid As String = Guid.NewGuid.ToString
        Dim par As IDataParameter

        Try

            ' save de pccGeov0Geometry
            If Not MyBase.Save(conn, trans, id_entidade, storage_table) Then
                Return False
            End If

            com = conn.CreateCommand
            If trans IsNot Nothing Then com.Transaction = trans

            com.CommandType = CommandType.Text
            'iKey, rec_id, k, geom, catalogo, mtd_id
            com.CommandText = "insert into " + storage_table + "(rec_id, k," & com.GetFunction(ProviderFunctions.GeometryFieldName, "geom") & ",catalogo,mtd_id) values(?,?," & com.GetFunction(ProviderFunctions.WKTToGeoPrefix) & com.GetFunction(ProviderFunctions.WKTToGeo, "?", "", "0") & com.GetFunction(ProviderFunctions.WKTToGeoSufix) & ",?,?)"

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

                    Dim gu As New pccGeov0Utils
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

            com.ExecuteNonQuery()

            com = conn.CreateCommand
            If trans IsNot Nothing Then com.Transaction = trans

            com.CommandType = CommandType.Text
            com.CommandText = "insert into entidades_" + storage_table + "(geo_id, entidade_id) values(?,?)"

            par = com.CreateParameter(DbType.String, geoid)
            com.AddParameter(par)

            par = com.CreateParameter(DbType.String, id_entidade)
            com.AddParameter(par)

            com.ExecuteNonQuery()

            Return True

        Catch ex As Exception

            Return False

        End Try

    End Function

    Public Overrides Function GetMulti() As pccGeov0Geometry
        Dim g As New pccGeov0MultiPoint
        Return g
    End Function

    Public Overrides Function AddGeometry(ByRef g As pccGeov0Geometry) As Boolean
        Me.Catalogo = g.Catalogo

        Me.X = CType(g, pccGeov0Point).X
        Me.Y = CType(g, pccGeov0Point).Y
        Me.Z = CType(g, pccGeov0Point).Z
    End Function

    Public Overrides ReadOnly Property Centroid() As pccGeov0Point
        Get
            Return Me
        End Get
    End Property

    Public Overrides Function IsMulti() As Boolean
        Return False
    End Function

    Public Overrides Function Definition() As String
        Return "PT|0|" & X.ToString & "|" & Y.ToString & "|" & Z.ToString & "|" & Area.ToString & "|" & Length.ToString
    End Function
End Class

<Serializable()>
Public Class pccGeov0MultiPoint
    Inherits pccGeov0Geometry

    Dim gu As New pccGeov0Utils

    Private _id As String
    Private _name As String

    Private _points As List(Of pccGeov0Point)
    Private _kpoints As Integer

    Public Sub New()
        Me.New("", "")
    End Sub
    Public Sub New(ByVal id As String, ByVal nome As String)
        MyBase.New(id, nome)
        _kpoints = 0
        _points = New List(Of pccGeov0Point)
    End Sub

    Public Sub AddPoint(ByRef p As pccGeov0Point)
        _kpoints += 1
        _points.Add(p)

        If _kpoints = 1 Then
            MyBase._3d = p.Is3d
        Else
            MyBase._3d = MyBase._3d And p.Is3d
        End If
    End Sub
    Public Overrides ReadOnly Property GetGeometryType() As pccGeov0GeometryType
        Get
            Return pccGeov0GeometryType.MultiPoint
        End Get
    End Property
    Public Overrides Property WKT() As String
        Get
            Dim ret As String = ""
            If _kpoints = 0 Then
                ret &= "null"
            Else
                For Each p As pccGeov0Point In _points
                    If ret = "" Then
                        ret &= "MULTIPOINT("
                    Else
                        ret &= ","
                    End If
                    ret &= p.Coords
                Next
            End If
            ret &= ")"
            Return ret
        End Get
        Set(ByVal value As String)
            Throw New Exception("pcc by design: WKT is a readonly property")
        End Set

    End Property
    Public ReadOnly Property Points() As List(Of pccGeov0Point)
        Get
            Return _points
        End Get
    End Property

    Public Overrides ReadOnly Property Is3d() As Boolean
        Get
            Return MyBase._3d
        End Get
    End Property
    Public Overrides ReadOnly Property Area() As Double
        Get
            If Not _area_calc Then
                _area = 0
                _area = gu.CalcArea(_points)
                _area_calc = True
            End If
            Return _area
        End Get
    End Property
    Public Overrides ReadOnly Property ConvexHul() As pccGeov0Polygon
        Get
            If Not _convexhul_calc Then
                ' TODO: calcular convexhul
                _convexhul_calc = True
            End If
            Return MyBase._convexhul
        End Get
    End Property
    Public Overrides ReadOnly Property Length() As Double
        Get
            If Not _length_calc Then
                _length = 0
                _length = gu.CalcLenght(_points)
                _length_calc = True
            End If
            Return _length
        End Get
    End Property
    Public Overrides ReadOnly Property MBR() As pccGeov0Rectangle
        Get
            If Not _mbr_calc Then
                gu.CalcMBR(_points, _mbr)
                _mbr_calc = True
            End If
            Return _mbr
        End Get
    End Property
    Public Overrides ReadOnly Property IsEqual(ByVal geo As pccGeov0Geometry) As Boolean
        Get
            'TEST: Issue #5 - IsEqual

            If Definition() <> geo.Definition Then
                Return False
            End If

            Dim Resultado As Boolean = True
            Dim Size As Double = 0
            Dim i As Double = 0
            Dim j As Double = 0

            Dim Lista1 As List(Of pccGeov0Point)
            Dim Lista2 As List(Of pccGeov0Point)

            ' Se não tiverem igual número de pontos não é idêntico
            If _kpoints <> CType(geo, pccGeov0MultiPoint)._kpoints Then
                Resultado = False
            Else

                ' Primeiro tem que se ordenar ambos os arrays de pontos 
                Lista1 = New List(Of pccGeov0Point)(_points)
                Lista2 = New List(Of pccGeov0Point)(CType(geo, pccGeov0MultiPoint)._points)

                Lista1.Sort(AddressOf pccGeov0Point.Compare)
                Lista2.Sort(AddressOf pccGeov0Point.Compare)
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

        End Get
    End Property

    Public Overrides Property GetBasicGeometries() As pccGeov0Geometry()
        Get
            Dim g(0) As pccGeov0MultiPoint
            g(0) = New pccGeov0MultiPoint()
            g(0) = Me
            Return g
        End Get
        Set(ByVal value As pccGeov0Geometry())
            Throw New Exception("pcc by design: GetBasicGeometries is a readonly property")
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return MyBase.ToString() & vbCrLf & " k:" & _kpoints
    End Function

    Public Overrides Function Save(ByRef conn As pccDB4.Connection, ByVal trans As IDbTransaction, ByVal id_entidade As String, ByVal storage_table As String) As Boolean

        Dim com As pccDB4.Command
        Dim geoid As String = Guid.NewGuid.ToString
        Dim par As IDataParameter

        Try

            ' save de pccGeov0Geometry
            If Not MyBase.Save(conn, trans, id_entidade, storage_table) Then
                Return False
            End If

            com = conn.CreateCommand
            If trans IsNot Nothing Then com.Transaction = trans

            com.CommandType = CommandType.Text
            'iKey, rec_id, k, geom, catalogo, mtd_id
            com.CommandText = "insert into " + storage_table + "(rec_id, k," & com.GetFunction(ProviderFunctions.GeometryFieldName, "geom") & ",catalogo,mtd_id) values(?,?," & com.GetFunction(ProviderFunctions.WKTToGeoPrefix) & com.GetFunction(ProviderFunctions.WKTToGeo, "?", "", "0") & com.GetFunction(ProviderFunctions.WKTToGeoSufix) & ",?,?)"

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

                    Dim gu As New pccGeov0Utils
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

            com.ExecuteNonQuery()

            com = conn.CreateCommand
            If trans IsNot Nothing Then com.Transaction = trans

            com.CommandType = CommandType.Text
            com.CommandText = "insert into entidades_" + storage_table + "(geo_id, entidade_id) values(?,?)"

            par = com.CreateParameter(DbType.String, geoid)
            com.AddParameter(par)

            par = com.CreateParameter(DbType.String, id_entidade)
            com.AddParameter(par)

            com.ExecuteNonQuery()

            Return True

        Catch ex As Exception

            Return False

        End Try

    End Function

    Public Overrides Function GetMulti() As pccGeov0Geometry
        Dim g As New pccGeov0Collection
        Return g
    End Function

    Public Overrides Function AddGeometry(ByRef g As pccGeov0Geometry) As Boolean
        AddPoint(g)
    End Function

    Public Overrides ReadOnly Property Centroid() As pccGeov0Point
        Get
            If Not _centroid_calc Then
                Dim tx As Double = 0
                Dim ty As Double = 0
                For i As Integer = 0 To _points.Count - 1
                    tx += _points(i).X
                    ty += _points(i).Y
                Next
                _centroid = New pccGeov0Point("", "", tx / _points.Count, ty / _points.Count)
                _centroid_calc = True
            End If
            Return _centroid
        End Get
    End Property



    Public Overrides Function IsMulti() As Boolean
        Return True
    End Function

    Public Overrides Function Definition() As String

        Return "MPT|" & _kpoints.ToString & "|" & Centroid.X.ToString & "|" & Centroid.Y.ToString & "|" & Centroid.Z.ToString & "|" & Area.ToString & "|" & Length.ToString

    End Function
End Class

<Serializable()>
Public Class pccGeov0LineString
    Inherits pccGeov0Geometry

    Dim gu As New pccGeov0Utils

    Private _vertices As List(Of pccGeov0Point)
    Private _kvertices As Integer

    Public Sub New()
        Me.New("", "")
    End Sub

    Public Sub New(ByVal id As String, ByVal nome As String)
        MyBase.New(id, nome)
        _kvertices = 0
        _vertices = New List(Of pccGeov0Point)
    End Sub

    Public Sub AddVertice(ByRef p As pccGeov0Point)

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

    End Sub

    Public Function Coords() As String
        Dim ret As String = ""
        If _kvertices = 0 Then
            ret &= "null"
        Else
            For Each p As pccGeov0Point In _vertices
                If ret <> "" Then ret &= ","
                ret &= p.Coords
            Next
        End If
        Return ret
    End Function

    Public Overrides ReadOnly Property GetGeometryType() As pccGeov0GeometryType
        Get
            Return pccGeov0GeometryType.LineString
        End Get
    End Property
    Public Overrides Property WKT() As String
        Get
            Dim ret As String = "LINESTRING(" & Coords() & ")"
            Return ret
        End Get
        Set(ByVal value As String)
            Throw New Exception("pcc by design: WKT is a readonly property")
        End Set

    End Property

    Public Overrides ReadOnly Property Is3d() As Boolean
        Get
            Return MyBase._3d
        End Get
    End Property

    Public Overrides ReadOnly Property Area() As Double
        Get
            If Not _area_calc Then
                _area = 0
                _area = gu.CalcArea(_vertices)
                _area_calc = True
            End If
            Return _area
        End Get
    End Property

    Public Overrides ReadOnly Property ConvexHul() As pccGeov0Polygon
        Get
            If Not _convexhul_calc Then
                'TODO: calcular convexhul
                _convexhul_calc = True
            End If
            Return MyBase._convexhul
        End Get
    End Property

    Public Overrides ReadOnly Property Length() As Double
        Get
            If Not _length_calc Then
                _length = 0
                _length = gu.CalcLenght(_vertices)
                _length_calc = True
            End If
            Return _length
        End Get
    End Property

    Public Overrides ReadOnly Property MBR() As pccGeov0Rectangle
        Get
            If Not _mbr_calc Then
                gu.CalcMBR(_vertices, _mbr)
                _mbr_calc = True
            End If
            Return _mbr
        End Get
    End Property

    Public ReadOnly Property Vertices As List(Of pccGeov0Point)
        Get
            Return _vertices
        End Get
    End Property

    Public Overrides ReadOnly Property IsEqual(ByVal geo As pccGeov0Geometry) As Boolean
        Get
            Dim i As Integer = 0
            Dim Resultado As Boolean = True

            If Definition() <> geo.Definition Then
                Return False
            End If

            'TEST: Issue #6 - IsEqual
            'Se numero de vértices for diferente não são iguais
            If _kvertices = CType(geo, pccGeov0LineString).Vertices.Count Then
                'Compara do inicio para o fim 
                For i = 0 To _kvertices - 1
                    If Not _vertices(i).IsEqual(CType(geo, pccGeov0LineString).Vertices(i)) Then
                        Resultado = False
                        Exit For
                    End If
                Next
                'Se ainda não são iguais vai comparar do fim para o inicio 
                If Not Resultado Then
                    Resultado = True
                    For i = 0 To _kvertices - 1
                        If Not _vertices(i).IsEqual(CType(geo, pccGeov0LineString).Vertices(_kvertices - i - 1)) Then
                            Resultado = False
                            Exit For
                        End If
                    Next
                End If
            Else
                Resultado = False
            End If
            Return Resultado
        End Get
    End Property

    Public Overrides Property GetBasicGeometries() As pccGeov0Geometry()
        Get
            Dim g(0) As pccGeov0LineString
            g(0) = New pccGeov0LineString
            g(0) = Me
            Return g
        End Get
        Set(ByVal value As pccGeov0Geometry())
            Throw New Exception("pcc by design: GetBasicGeometries is a readonly property")
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return MyBase.ToString() & vbCrLf & " k:" & _kvertices
    End Function

    Public Overrides Function Save(ByRef conn As pccDB4.Connection, ByVal trans As IDbTransaction, ByVal id_entidade As String, ByVal storage_table As String) As Boolean

        Dim com As pccDB4.Command
        Dim geoid As String = Guid.NewGuid.ToString
        Dim par As IDataParameter

        Try

            ' save de pccGeov0Geometry
            If Not MyBase.Save(conn, trans, id_entidade, storage_table) Then
                Return False
            End If

            com = conn.CreateCommand
            If trans IsNot Nothing Then com.Transaction = trans

            com.CommandType = CommandType.Text
            'iKey, rec_id, k, geom, catalogo, mtd_id
            com.CommandText = "insert into " + storage_table + "(rec_id, k," & com.GetFunction(ProviderFunctions.GeometryFieldName, "geom") & ",catalogo,mtd_id) values(?,?," & com.GetFunction(ProviderFunctions.WKTToGeoPrefix) & com.GetFunction(ProviderFunctions.WKTToGeo, "?", "", "0") & com.GetFunction(ProviderFunctions.WKTToGeoSufix) & ",?,?)"

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

                    Dim gu As New pccGeov0Utils
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

            com.ExecuteNonQuery()

            com = conn.CreateCommand
            If trans IsNot Nothing Then com.Transaction = trans

            com.CommandType = CommandType.Text
            com.CommandText = "insert into entidades_" + storage_table + "(geo_id, entidade_id) values(?,?)"

            par = com.CreateParameter(DbType.String, geoid)
            com.AddParameter(par)

            par = com.CreateParameter(DbType.String, id_entidade)
            com.AddParameter(par)

            com.ExecuteNonQuery()

            Return True

        Catch ex As Exception

            Return False

        End Try

    End Function

    Public Overrides Function GetMulti() As pccGeov0Geometry

        Dim g As New pccGeov0MultiLineString
        Return g

    End Function

    Public Overrides Function AddGeometry(ByRef g As pccGeov0Geometry) As Boolean

        Me.Catalogo = g.Catalogo

        Me._vertices.Clear()
        Me._kvertices = 0

        For Each v As pccGeov0Point In CType(g, pccGeov0LineString).Vertices
            Me.AddVertice(v)
        Next

        _centroid_calc = False
        _convexhul_calc = False
        _length_calc = False
        _mbr_calc = False

    End Function

    Public Overrides ReadOnly Property Centroid() As pccGeov0Point

        Get

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

                _centroid = New pccGeov0Point(tx / tLen, ty / tLen)
                _centroid_calc = True

                ' aproveita para fazer set de Length
                If Not _length_calc Then
                    _length = tLen
                    _length_calc = True
                End If

            End If

            Return _centroid

        End Get

    End Property

    Public Overrides Function IsMulti() As Boolean
        Return False
    End Function

    Public Overrides Function Definition() As String

        Return "PL|" & _kvertices.ToString & "|" & Centroid.X.ToString & "|" & Centroid.Y.ToString & "|" & Centroid.Z.ToString & "|" & Area.ToString & "|" & Length.ToString

    End Function

End Class

<Serializable()>
Public Class pccGeov0MultiLineString
    Inherits pccGeov0Geometry

    Dim gu As New pccGeov0Utils

    Private _linestrings As List(Of pccGeov0LineString)
    Private _klinestrings As Integer

    Public Sub New()
        Me.New("", "")
    End Sub

    Public Sub New(ByVal id As String, ByVal nome As String)
        MyBase.New(id, nome)
        _klinestrings = 0
        _linestrings = New List(Of pccGeov0LineString)
    End Sub

    Public Sub AddLineString(ByRef ls As pccGeov0LineString)
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

    End Sub

    Public Overrides ReadOnly Property GetGeometryType() As pccGeov0GeometryType
        Get
            Return pccGeov0GeometryType.MultiLineString
        End Get
    End Property

    Public Overrides Property WKT() As String
        Get
            Dim ret As String = ""
            If _klinestrings = 0 Then
                ret &= "null"
            Else
                For Each ls As pccGeov0LineString In _linestrings
                    If ret = "" Then
                        ret = "MULTILINESTRING("
                    Else
                        ret &= ","
                    End If
                    ret &= ls.WKT.Substring(10)
                Next
            End If
            ret &= ")"
            Return ret
        End Get
        Set(ByVal value As String)
            Throw New Exception("pcc by design: WKT is a readonly property")
        End Set

    End Property

    Public Overrides ReadOnly Property Is3d() As Boolean
        Get
            Return MyBase._3d
        End Get
    End Property

    Public Overrides ReadOnly Property Area() As Double
        Get
            If Not _area_calc Then
                _area = 0
                If _linestrings IsNot Nothing Then
                    For Each ls As pccGeov0LineString In _linestrings
                        _area += ls.Area
                    Next
                End If
                _area_calc = True
            End If
            Return _area
        End Get
    End Property

    Public Overrides ReadOnly Property ConvexHul() As pccGeov0Polygon
        Get
            If Not _convexhul_calc Then
                ' TODO: calcular convexhul
                _convexhul_calc = True
            End If
            Return _convexhul
        End Get
    End Property

    Public Overrides ReadOnly Property Length() As Double
        Get
            If Not _length_calc Then
                _length = 0
                If _linestrings IsNot Nothing Then
                    For Each ls As pccGeov0LineString In _linestrings
                        _length += ls.Length
                    Next
                End If
                _length_calc = True
            End If
            Return _length
        End Get
    End Property

    Public Overrides ReadOnly Property MBR() As pccGeov0Rectangle
        Get
            If Not _mbr_calc Then
                Dim cents As New pccGeov0MultiPoint()
                For Each ls As pccGeov0LineString In _linestrings
                    For Each p As pccGeov0Point In ls.MBR.Vertices
                        cents.AddPoint(p)
                    Next
                Next
                gu.CalcMBR(cents.Points, _mbr)
                _mbr_calc = True
            End If
            Return _mbr
        End Get
    End Property

    Public Overrides ReadOnly Property IsEqual(ByVal geo As pccGeov0Geometry) As Boolean
        Get
            'TEST: Issue #7 - IsEqual
            If Definition() <> geo.Definition Then
                Return False
            End If

            'Ter em atenção que as várias linhas podem não aparecer pela mesma ordem.
            Dim Resultado As Boolean = True
            Dim i As Long = 0
            Dim j As Long = 0

            Dim Linhas1 As List(Of pccGeov0LineString)
            Dim Linhas2 As List(Of pccGeov0LineString)

            If _klinestrings <> CType(geo, pccGeov0MultiLineString)._klinestrings Then
                Resultado = False
            Else

                ' Primeiro tem que se ordenar ambos os arrays de pontos 
                Linhas1 = New List(Of pccGeov0LineString)(_linestrings)
                Linhas2 = New List(Of pccGeov0LineString)(CType(geo, pccGeov0MultiLineString)._linestrings)

                Linhas1.Sort(AddressOf pccGeov0LineString.Compare)
                Linhas2.Sort(AddressOf pccGeov0LineString.Compare)
                ' Aqui ambos as listas de GeoGeoMultiPolygons estao ordenadas segundo o mesmo critério
                ' Agora é verificar se são identicas

                For i = 0 To Linhas1.Count - 1
                    If Not Linhas1(i).IsEqual(Linhas2(i)) Then
                        Resultado = False
                        Exit For
                    End If
                Next
                'For Each i1 As pccGeov0Geometry In Linhas1
                '    For Each i2 As pccGeov0Geometry In Linhas2
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
        End Get
    End Property

    Public Overrides Property GetBasicGeometries() As pccGeov0Geometry()
        Get
            Dim g(0) As pccGeov0MultiLineString
            g(0) = New pccGeov0MultiLineString()
            g(0) = Me
            Return g
        End Get
        Set(ByVal value As pccGeov0Geometry())
            Throw New Exception("pcc by design: GetBasicGeometries is a readonly property")
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return MyBase.ToString() & vbCrLf & " k:" & _klinestrings
    End Function

    Public Overrides Function Save(ByRef conn As pccDB4.Connection, ByVal trans As IDbTransaction, ByVal id_entidade As String, ByVal storage_table As String) As Boolean

        Dim com As pccDB4.Command
        Dim geoid As String = Guid.NewGuid.ToString
        Dim par As IDataParameter

        Try

            ' save de pccGeov0Geometry
            If Not MyBase.Save(conn, trans, id_entidade, storage_table) Then
                Return False
            End If

            com = conn.CreateCommand
            If trans IsNot Nothing Then com.Transaction = trans

            com.CommandType = CommandType.Text
            'iKey, rec_id, k, geom, catalogo, mtd_id
            com.CommandText = "insert into " + storage_table + "(rec_id, k," & com.GetFunction(ProviderFunctions.GeometryFieldName, "geom") & ",catalogo,mtd_id) values(?,?," & com.GetFunction(ProviderFunctions.WKTToGeoPrefix) & com.GetFunction(ProviderFunctions.WKTToGeo, "?", "", "0") & com.GetFunction(ProviderFunctions.WKTToGeoSufix) & ",?,?)"

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

                    Dim gu As New pccGeov0Utils
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

            com.ExecuteNonQuery()

            com = conn.CreateCommand
            If trans IsNot Nothing Then com.Transaction = trans

            com.CommandType = CommandType.Text
            com.CommandText = "insert into entidades_" + storage_table + "(geo_id, entidade_id) values(?,?)"

            par = com.CreateParameter(DbType.String, geoid)
            com.AddParameter(par)

            par = com.CreateParameter(DbType.String, id_entidade)
            com.AddParameter(par)

            com.ExecuteNonQuery()

            Return True

        Catch ex As Exception

            Return False

        End Try

    End Function

    Public Overrides Function GetMulti() As pccGeov0Geometry
        Dim g As New pccGeov0Collection
        Return g
    End Function

    Public Overrides Function AddGeometry(ByRef g As pccGeov0Geometry) As Boolean
        Me.AddLineString(g)
    End Function

    Public Overrides ReadOnly Property Centroid() As pccGeov0Point

        Get

            If Not _centroid_calc Then

                Dim tLen As Double = 0

                Dim a_centroids(Me._klinestrings - 1) As pccGeov0Point
                Dim a_lens(Me._klinestrings - 1) As Double
                Dim k As Integer = 0

                For Each l As pccGeov0LineString In _linestrings
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

                _centroid = New pccGeov0Point(tx / tLen, ty / tLen)
                _centroid_calc = True

                ' aproveita para fazer set de Length
                If Not _length_calc Then
                    _length = tLen
                    _length_calc = True
                End If

            End If

            Return _centroid

        End Get

    End Property

    Public Overrides Function IsMulti() As Boolean
        Return True
    End Function

    Public Overrides Function Definition() As String

        Return "MPL|" & _klinestrings.ToString & "|" & Centroid.X.ToString & "|" & Centroid.Y.ToString & "|" & Centroid.Z.ToString & "|" & Area.ToString & "|" & Length.ToString

    End Function
End Class

<Serializable()>
Public Class pccGeov0Polygon
    Inherits pccGeov0Geometry

    Dim gu As New pccGeov0Utils

    Private _vertices As List(Of pccGeov0Point)
    Private _kvertices As Integer
    Private _innerpols As List(Of pccGeov0Polygon)
    Private _kinnerpols As Integer

    Public Sub New()
        Me.New("", "")
    End Sub
    Public Sub New(ByVal id As String, ByVal nome As String)
        MyBase.New(id, nome)
        _kvertices = 0
        _kinnerpols = 0
        _vertices = New List(Of pccGeov0Point)
        _innerpols = New List(Of pccGeov0Polygon)
    End Sub

    Public Sub AddVertice(ByRef p As pccGeov0Point)

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
    End Sub
    Public Sub AddInnerPolygon(ByRef p As pccGeov0Polygon)
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

    End Sub

    Public Property Vertices As List(Of pccGeov0Point)
        Get
            Return _vertices
        End Get
        Set(ByVal value As List(Of pccGeov0Point))
            Throw New Exception("pcc by design: Vertices is a readonly property")
        End Set

    End Property

    Public Function Coords() As String
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
    End Function
    Public Function InnerPolygons() As List(Of pccGeov0Polygon)
        Return _innerpols
    End Function

    Public Overrides ReadOnly Property GetGeometryType() As pccGeov0GeometryType
        Get
            Return pccGeov0GeometryType.Polygon
        End Get
    End Property


    Public Overrides Property WKT() As String
        Get
            Dim ret As String = "POLYGON(" & Coords() & ")"
            Return ret
        End Get
        Set(ByVal value As String)
            Throw New Exception("pcc by design: WKT is a readonly property")
        End Set

    End Property

    Public Overrides ReadOnly Property Is3d() As Boolean
        Get
            Return MyBase._3d
        End Get
    End Property

    Public Overrides ReadOnly Property Area() As Double
        Get
            If Not _area_calc Then
                _area = gu.CalcArea(_vertices)
                If _innerpols IsNot Nothing Then
                    For Each p As pccGeov0Polygon In _innerpols
                        _area += p.Area
                    Next
                End If
                _area_calc = True
            End If
            Return _area
        End Get
    End Property

    Public Overrides ReadOnly Property ConvexHul() As pccGeov0Polygon
        Get
            If Not _convexhul_calc Then
                ' TODO: calcular convexhul
                _convexhul_calc = True
            End If
            Return _convexhul
        End Get
    End Property

    Public Overrides ReadOnly Property Length() As Double
        Get
            If Not _length_calc Then
                _length = 0
                _length = gu.CalcLenght(_vertices)
                _length_calc = True
            End If
            Return _length
        End Get
    End Property

    Public Overrides ReadOnly Property MBR() As pccGeov0Rectangle
        Get
            If Not _mbr_calc Then
                Dim cents As New pccGeov0MultiPoint()
                For Each p As pccGeov0Point In Vertices()
                    cents.AddPoint(p)
                Next
                gu.CalcMBR(cents.Points, _mbr)
                _mbr_calc = True
            End If
            Return _mbr
        End Get
    End Property

    Public Function IsHole() As Boolean
        Return (Area() < 0)
    End Function

    Public Overrides ReadOnly Property IsEqual(ByVal geo As pccGeov0Geometry) As Boolean
        Get
            'TEST: Issue #8 - IsEqual
            If Definition() <> geo.Definition Then
                Return False
            End If
            Dim Resultado As Boolean = True
            Dim Size As Double = 0
            Dim i As Double = 0
            Dim j As Double = 0

            Dim PoligonoExterior1 As List(Of pccGeov0Point)
            Dim PoligonoExterior2 As List(Of pccGeov0Point)
            Dim PoligonosInteriores1 As List(Of pccGeov0Polygon)
            Dim PoligonosInteriores2 As List(Of pccGeov0Polygon)
            ' Se não tiverem igual número de poligonos não é idêntico de certeza
            If _kvertices <> CType(geo, pccGeov0Polygon)._kvertices Or _kinnerpols <> CType(geo, pccGeov0Polygon)._kinnerpols Then
                Resultado = False
            Else

                ' Vamos tratar os poligonos exteriores primeiro
                ' Ordenar ambos os arrays de poligonos 
                PoligonoExterior1 = New List(Of pccGeov0Point)(_vertices)
                PoligonoExterior2 = New List(Of pccGeov0Point)(CType(geo, pccGeov0Polygon)._vertices)

                PoligonoExterior1.Sort(AddressOf pccGeov0Point.Compare)
                PoligonoExterior2.Sort(AddressOf pccGeov0Point.Compare)
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

                If _kinnerpols <> 0 And Resultado And CType(geo, pccGeov0Polygon)._kinnerpols - 1 <> 0 Then
                    ' Temos ilhas e os Poligono exterior são idênticos
                    PoligonosInteriores1 = _innerpols
                    PoligonosInteriores2 = CType(geo, pccGeov0Polygon)._innerpols

                    PoligonosInteriores1.Sort(AddressOf pccGeov0Polygon.Compare)
                    PoligonosInteriores2.Sort(AddressOf pccGeov0Polygon.Compare)
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
        End Get
    End Property


    Public Overrides Property GetBasicGeometries() As pccGeov0Geometry()

        Get

            Dim g() As pccGeov0Polygon
            Dim kg As Integer = 0

            ReDim Preserve g(kg)
            g(kg) = New pccGeov0Polygon

            If _vertices IsNot Nothing Then

                For Each v As pccGeov0Point In _vertices
                    g(kg).AddVertice(v)
                Next

                If _innerpols IsNot Nothing Then
                    For Each po As pccGeov0Polygon In _innerpols
                        For Each poo As pccGeov0Polygon In po.GetBasicGeometries
                            kg += 1
                            ReDim Preserve g(kg)
                            g(kg) = poo
                        Next
                    Next
                End If

            End If

            Return g

        End Get

        Set(ByVal value As pccGeov0Geometry())
            Throw New Exception("pcc by design: GetBasicGeometries is a readonly property")
        End Set

    End Property

    Public Overrides Function ToString() As String
        Return MyBase.ToString() & vbCrLf & " k:" & _kvertices & " holes:" & _kinnerpols
    End Function

    Public Overrides Function Save(ByRef conn As pccDB4.Connection, ByVal trans As IDbTransaction, ByVal id_entidade As String, ByVal storage_table As String) As Boolean

        Dim com As pccDB4.Command
        Dim geoid As String = Guid.NewGuid.ToString
        Dim par As IDataParameter

        Try

            ' save de pccGeov0Geometry
            If Not MyBase.Save(conn, trans, id_entidade, storage_table) Then
                Return False
            End If

            com = conn.CreateCommand
            If trans IsNot Nothing Then com.Transaction = trans

            com.CommandType = CommandType.Text
            'iKey, rec_id, k, geom, catalogo, mtd_id
            com.CommandText = "insert into " + storage_table + "(rec_id, k," & com.GetFunction(ProviderFunctions.GeometryFieldName, "geom") & ",catalogo,mtd_id) values(?,?," & com.GetFunction(ProviderFunctions.WKTToGeoPrefix) & com.GetFunction(ProviderFunctions.WKTToGeo, "?", "", "0") & com.GetFunction(ProviderFunctions.WKTToGeoSufix) & ",?,?)"

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

                    Dim gu As New pccGeov0Utils
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

            com.ExecuteNonQuery()

            com = conn.CreateCommand
            If trans IsNot Nothing Then com.Transaction = trans

            com.CommandType = CommandType.Text
            com.CommandText = "insert into entidades_" + storage_table + "(geo_id, entidade_id) values(?,?)"

            par = com.CreateParameter(DbType.String, geoid)
            com.AddParameter(par)

            par = com.CreateParameter(DbType.String, id_entidade)
            com.AddParameter(par)

            com.ExecuteNonQuery()

            Return True

        Catch ex As Exception

            Return False

        End Try

    End Function

    Public Overrides Function GetMulti() As pccGeov0Geometry
        Dim g As New pccGeov0MultiPolygon
        Return g
    End Function

    Public Overrides Function AddGeometry(ByRef g As pccGeov0Geometry) As Boolean

        Catalogo = g.Catalogo

        _vertices.Clear()
        _kvertices = 0
        _innerpols.Clear()
        _kinnerpols = 0

        For Each v As pccGeov0Point In CType(g, pccGeov0Polygon).Vertices
            Me.AddVertice(v)
        Next

        If CType(g, pccGeov0Polygon)._innerpols IsNot Nothing Then
            For Each ip As pccGeov0Polygon In CType(g, pccGeov0Polygon)._innerpols
                Me.AddInnerPolygon(ip)
            Next
        End If

    End Function

    <XmlIgnore(), SoapIgnore()>
    Public Overrides ReadOnly Property Centroid() As pccGeov0Point

        Get

            If Not _centroid_calc Then

                Dim tLen As Double = 0

                Dim a_centroids(Me._kinnerpols) As pccGeov0Point  'já contempla o pol principal
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

                a_centroids(0) = New pccGeov0Point(tx / (6 * a_areas(0)), ty / (6 * a_areas(0)))

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

                _centroid = New pccGeov0Point(tx / tArea, ty / tArea)
                _centroid_calc = True

                If Not _area_calc Then
                    _area = tArea
                    _area_calc = True
                End If

            End If

            Return _centroid

        End Get
    End Property

    Public Overrides Function IsMulti() As Boolean
        Return False
    End Function

    Public Overrides Function Definition() As String

        Return "PG|" & _kvertices.ToString & "|" & Centroid.X.ToString & "|" & Centroid.Y.ToString & "|" & Centroid.Z.ToString & "|" & Area.ToString & "|" & Length.ToString

    End Function
End Class

Public Class pccGeov0Rectangle
    Inherits pccGeov0Polygon

    Public Overrides ReadOnly Property MBR As pccGeov0Rectangle
        Get
            Return Nothing
        End Get
    End Property

    Public Overrides Property GetBasicGeometries As pccGeov0Geometry()
        Get
            Return Nothing
        End Get
        Set(ByVal value As pccGeov0Geometry())

        End Set
    End Property

End Class

<Serializable()>
Public Class pccGeov0MultiPolygon
    Inherits pccGeov0Geometry

    Dim gu As New pccGeov0Utils

    Private _polygons As List(Of pccGeov0Polygon)
    Private _kpolygons As Integer

    Public Sub New()
        Me.New("", "")
    End Sub
    Public Sub New(ByVal id As String, ByVal nome As String)
        MyBase.New(id, nome)
        _kpolygons = 0
        _polygons = New List(Of pccGeov0Polygon)
    End Sub

    Public Sub AddPolygon(ByRef p As pccGeov0Polygon)
        _kpolygons += 1
        _polygons.Add(p)
        If _kpolygons = 1 Then
            MyBase._3d = p.Is3d
        Else
            MyBase._3d = MyBase._3d And p.Is3d
        End If
        _area_calc = False
    End Sub

    Public Overrides ReadOnly Property GetGeometryType() As pccGeov0GeometryType
        Get
            Return pccGeov0GeometryType.MultiPolygon
        End Get
    End Property
    Public Overrides Property WKT() As String
        Get
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

            Return ret
        End Get
        Set(ByVal value As String)
            Throw New Exception("pcc by design: WKT is a readonly property")
        End Set

    End Property

    Public Overrides ReadOnly Property Is3d() As Boolean
        Get
            Return MyBase._3d
        End Get
    End Property

    Public Overrides ReadOnly Property Area() As Double
        Get
            If Not _area_calc Then
                _area = 0
                If _polygons IsNot Nothing Then
                    For Each p As pccGeov0Polygon In _polygons
                        _area += p.Area
                    Next
                End If
                _area_calc = True
            End If
            Return _area
        End Get
    End Property

    Public Overrides ReadOnly Property ConvexHul() As pccGeov0Polygon

        Get

            If Not _convexhul_calc Then
                ' TODO: calcular convexhul
                _convexhul_calc = True
            End If

            Return _convexhul

        End Get

    End Property

    Public Overrides ReadOnly Property Length() As Double

        Get

            If Not _length_calc Then
                _length = 0
                If _polygons IsNot Nothing Then
                    For Each p As pccGeov0Polygon In _polygons
                        _length += p.Length
                    Next
                End If
                _length_calc = True
            End If

            Return _length

        End Get

    End Property

    Public Overrides ReadOnly Property MBR() As pccGeov0Rectangle

        Get

            If Not _mbr_calc Then
                Dim cents As New pccGeov0MultiPoint()
                For Each pol As pccGeov0Polygon In _polygons
                    For Each p As pccGeov0Point In pol.MBR.Vertices
                        cents.AddPoint(p)
                    Next
                Next

                gu.CalcMBR(cents.Points, _mbr)
                _mbr_calc = True
            End If

            Return _mbr

        End Get

    End Property

    Public Overrides ReadOnly Property IsEqual(ByVal geo As pccGeov0Geometry) As Boolean
        Get
            'TEST: Issue #9 - IsEqual
            'Ter em atenção que os vários polígonos podem não aparecer pela mesma ordem.

            If Definition() <> geo.Definition Then
                Return False
            End If

            Dim Resultado As Boolean = True
            Dim i As Long = 0
            Dim j As Long = 0

            Dim PoligonoExterior1 As List(Of pccGeov0Polygon)
            Dim PoligonoExterior2 As List(Of pccGeov0Polygon)

            If _kpolygons <> CType(geo, pccGeov0MultiPolygon)._kpolygons Then
                Resultado = False
            Else

                ' Primeiro tem que se ordenar ambos os arrays de pontos 

                PoligonoExterior1 = New List(Of pccGeov0Polygon)(_polygons)
                PoligonoExterior2 = New List(Of pccGeov0Polygon)(CType(geo, pccGeov0MultiPolygon)._polygons)

                PoligonoExterior1.Sort(AddressOf pccGeov0Polygon.Compare)
                PoligonoExterior2.Sort(AddressOf pccGeov0Polygon.Compare)
                ' Aqui ambos as listas de GeoGeoMultiPolygons estao ordenadas segundo o mesmo critério
                ' Agora é verificar se são identicas


                For i = 0 To PoligonoExterior1.Count - 1
                    If Not PoligonoExterior1(i).IsEqual(PoligonoExterior2(i)) Then
                        Resultado = False
                        Exit For
                    End If
                Next

                'For Each i1 As pccGeov0Geometry In PoligonoExterior1 ' _kpolygons - 1
                '    For Each i2 As pccGeov0Geometry In PoligonoExterior2 ' CType(geo, pccGeov0MultiPolygon)._kpolygons - 1
                '        If Not i1.IsEqual(i2) Then
                '            'If Not _polygons(i).IsEqual(CType(geo, pccGeov0MultiPolygon)._polygons(j)) Then
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
        End Get
    End Property

    Public Overrides Property GetBasicGeometries() As pccGeov0Geometry()

        Get
            Dim ret() As pccGeov0Geometry
            ReDim ret(_polygons.Count - 1)
            Dim k As Integer = 0

            For Each g As pccGeov0Geometry In _polygons
                ret(k) = g
                k += 1
            Next
            Return ret
        End Get

        Set(ByVal value As pccGeov0Geometry())
            Throw New Exception("pcc by design: GetBasicGeometries is a readonly property")
        End Set

    End Property

    Public Overrides Function ToString() As String

        Return MyBase.ToString() & vbCrLf & " k:" & _kpolygons

    End Function

    Public Overrides Function Save(ByRef conn As pccDB4.Connection, ByVal trans As IDbTransaction, ByVal id_entidade As String, ByVal storage_table As String) As Boolean

        Dim com As pccDB4.Command
        Dim geoid As String = Guid.NewGuid.ToString
        Dim par As IDataParameter

        Try

            ' save de pccGeov0Geometry
            If Not MyBase.Save(conn, trans, id_entidade, storage_table) Then
                Return False
            End If

            com = conn.CreateCommand
            If trans IsNot Nothing Then com.Transaction = trans

            com.CommandType = CommandType.Text
            'iKey, rec_id, k, geom, catalogo, mtd_id
            com.CommandText = "insert into " + storage_table + "(rec_id, k," & com.GetFunction(ProviderFunctions.GeometryFieldName, "geom") & ",catalogo,mtd_id) values(?,?," & com.GetFunction(ProviderFunctions.WKTToGeoPrefix) & com.GetFunction(ProviderFunctions.WKTToGeo, "?", "", "0") & com.GetFunction(ProviderFunctions.WKTToGeoSufix) & ",?,?)"

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

                    Dim gu As New pccGeov0Utils
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

            com.ExecuteNonQuery()

            com = conn.CreateCommand
            If trans IsNot Nothing Then com.Transaction = trans

            com.CommandType = CommandType.Text
            com.CommandText = "insert into entidades_" + storage_table + "(geo_id, entidade_id) values(?,?)"

            par = com.CreateParameter(DbType.String, geoid)
            com.AddParameter(par)

            par = com.CreateParameter(DbType.String, id_entidade)
            com.AddParameter(par)

            com.ExecuteNonQuery()

            Return True

        Catch ex As Exception

            Return False

        End Try

    End Function

    Public Overrides Function GetMulti() As pccGeov0Geometry
        Dim g As New pccGeov0Collection
        Return g
    End Function

    Public Overrides Function AddGeometry(ByRef g As pccGeov0Geometry) As Boolean
        Me.AddPolygon(g)
    End Function

    Public Overrides ReadOnly Property Centroid() As pccGeov0Point

        Get

            If Not _centroid_calc Then

                Dim tArea As Double = 0

                Dim a_centroids(Me._kpolygons - 1) As pccGeov0Point
                Dim a_areas(Me._kpolygons - 1) As Double
                Dim k As Integer = 0

                For Each p As pccGeov0Polygon In _polygons
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

                _centroid = New pccGeov0Point(tx / tArea, ty / tArea)
                _centroid_calc = True

                ' aproveita para fazer set de Length
                If Not _area_calc Then
                    _area = tArea
                    _area_calc = True
                End If

            End If

            Return _centroid

        End Get

    End Property

    Public Overrides Function IsMulti() As Boolean
        Return True
    End Function

    Public Overrides Function Definition() As String

        Return "MPG|" & _kpolygons.ToString & "|" & Centroid.X.ToString & "|" & Centroid.Y.ToString & "|" & Centroid.Z.ToString & "|" & Area.ToString & "|" & Length.ToString

    End Function
End Class

<Serializable()>
Public Class pccGeov0Collection
    Inherits pccGeov0Geometry

    Dim gu As New pccGeov0Utils

    Private _geometries As List(Of pccGeov0Geometry)
    Private _kgeometries As Integer

    Public Sub New()
        Me.New("", "")
    End Sub

    Public Sub New(ByVal id As String, ByVal nome As String)
        MyBase.New(id, nome)
        _kgeometries = 0
        _geometries = New List(Of pccGeov0Geometry)
    End Sub

    Public Sub New(ByRef conn As pccDB4.Connection, ByVal id_geometry As String, ByVal storage_table As String)
        Me.New("", "")
        Dim gu As New pccGeov0Utils
        Dim g As New pccGeov0Collection
        g = gu.GeometryById(conn, id_geometry, storage_table)
        For Each og As pccGeov0Geometry In g.GetBasicGeometries
            Me.AddGeometry(og)
        Next
    End Sub

    Public Sub AddOldGeometry(ByRef g As pccGeov0Geometry)
        _kgeometries += 1
        _geometries.Add(g)
        If _kgeometries = 1 Then
            MyBase._3d = g.Is3D
        Else
            MyBase._3d = MyBase._3d And g.Is3D
        End If
        _area_calc = False
    End Sub

    Public Overrides ReadOnly Property GetGeometryType() As pccGeov0GeometryType
        Get
            Return pccGeov0GeometryType.Collection
        End Get
    End Property

    Public Overrides Property WKT() As String
        Get
            Dim ret As String = ""
            If _kgeometries = 0 Then
                ret &= "null"
            Else
                For Each g As pccGeov0Geometry In _geometries
                    If ret = "" Then
                        ret = "GEOMETRYCOLLECTION("
                    Else
                        ret &= ","
                    End If
                    ret &= g.WKT
                Next
            End If
            ret &= ")"
            Return ret
        End Get
        Set(ByVal value As String)
            Throw New Exception("pcc by design: WKT is a readonly property")
        End Set

    End Property

    Public Overrides ReadOnly Property Is3d() As Boolean
        Get
            Return MyBase._3d
        End Get
    End Property

    Public Overrides ReadOnly Property Area() As Double
        Get
            If Not _area_calc Then
                _area = 0
                If _geometries IsNot Nothing Then
                    For Each p As pccGeov0Geometry In _geometries
                        _area += p.Area
                    Next
                End If
                _area_calc = True
            End If
            Return _area
        End Get
    End Property

    <XmlIgnore(), SoapIgnore()>
    Public Overrides ReadOnly Property ConvexHul() As pccGeov0Polygon
        Get
            If Not _convexhul_calc Then
                ' TODO: calcular convexhul
                _convexhul_calc = True
            End If
            Return _convexhul
        End Get
    End Property

    Public Overrides ReadOnly Property Length() As Double
        Get
            If Not _length_calc Then
                _length = 0
                If _geometries IsNot Nothing Then
                    For Each p As pccGeov0Geometry In _geometries
                        _length += p.Length
                    Next
                End If
                _length_calc = True
            End If
            Return _length
        End Get
    End Property

    Public Overrides ReadOnly Property MBR() As pccGeov0Rectangle
        Get
            If Not _mbr_calc Then
                Dim cents As New pccGeov0MultiPoint()
                For Each pol As pccGeov0Geometry In _geometries
                    Dim pol2 As New pccGeov0Rectangle
                    Dim xx As pccGeov0Geometry
                    xx = pol
                    pol2 = xx.MBR
                    For Each p As pccGeov0Point In pol2.Vertices
                        cents.AddPoint(p)
                    Next
                Next
                gu.CalcMBR(cents.Points, _mbr)
                _mbr_calc = True
            End If
            Return _mbr
        End Get
    End Property

    Public Overrides ReadOnly Property IsEqual(ByVal geo As pccGeov0Geometry) As Boolean
        Get
            'TEST: Issue #10 - IsEqual

            If Definition() <> geo.Definition Then
                Return False
            End If

            'Ter em atenção que as várias linhas podem não aparecer pela mesma ordem.
            Dim Resultado As Boolean = True
            Dim i As Long = 0
            Dim j As Long = 0

            Dim Coleccao1 As List(Of pccGeov0Geometry)
            Dim Coleccao2 As List(Of pccGeov0Geometry)

            If _kgeometries <> CType(geo, pccGeov0Collection)._kgeometries Then
                Resultado = False
            Else

                ' Primeiro tem que se ordenar ambos os arrays de geometrias 

                Coleccao1 = New List(Of pccGeov0Geometry)(_geometries)
                Coleccao2 = New List(Of pccGeov0Geometry)(CType(geo, pccGeov0Collection)._geometries)

                Coleccao1.Sort(AddressOf pccGeov0Geometry.Compare)
                Coleccao2.Sort(AddressOf pccGeov0Geometry.Compare)

                ' Aqui ambos as listas estão ordenadas segundo o mesmo critério
                ' Agora é verificar se são identicas

                For Each i1 As pccGeov0Geometry In Coleccao1
                    For Each i2 As pccGeov0Geometry In Coleccao2
                        If Not i1.IsEqual(i2) Then
                            Resultado = False
                            Exit For
                        End If
                    Next
                    If Not Resultado Then Exit For
                Next
                Return Resultado
            End If
        End Get

    End Property

    <XmlIgnore(), SoapIgnore()>
    Public Overrides Property GetBasicGeometries() As pccGeov0Geometry()

        Get
            ' TODO: verificar se é multi, isto é, se todas as geometrias são do mesmo tipo 

            Dim type As System.Type = Nothing
            Dim sameType As Boolean = False
            Dim g() As pccGeov0Geometry = Nothing
            Dim kg As Integer = 0

            If _geometries IsNot Nothing Then
                For Each po As pccGeov0Geometry In _geometries
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
                Dim mg(0) As pccGeov0Geometry
                mg(0) = _geometries(0).GetMulti()

                For Each ug As pccGeov0Geometry In _geometries
                    mg(0).AddGeometry(ug)
                Next
                Return mg
            Else
                Return g
            End If

        End Get
        Set(ByVal value As pccGeov0Geometry())
            Throw New Exception("pcc by design: GetBasicGeometries is a readonly property")
        End Set

    End Property

    Public Overrides Function ToString() As String
        Return MyBase.ToString() & vbCrLf & " k:" & _kgeometries
    End Function

    Public Overrides Function Save(ByRef conn As pccDB4.Connection, ByVal trans As IDbTransaction, ByVal id_entidade As String, ByVal storage_table As String) As Boolean

        For Each p As pccGeov0Geometry In _geometries
            If Not p.Save(conn, trans, id_entidade, storage_table) Then
                Return False
            End If
        Next

        Return True

    End Function

    Public Overrides Function GetMulti() As pccGeov0Geometry
        Dim g As New pccGeov0Collection
        Return g
    End Function

    Public Overrides Function AddGeometry(ByRef g As pccGeov0Geometry) As Boolean
        _kgeometries += 1
        If _kgeometries = 1 Then
            MyBase._3d = g.Is3D
        Else
            MyBase._3d = MyBase._3d And g.Is3D
        End If
        _geometries.Add(g)
        _area_calc = False
    End Function

    Public Overrides ReadOnly Property Centroid() As pccGeov0Point
        Get
            'TODO: Testar -> Implementar centroide Testado: Jaugusto, mas o centroide nao tem z nunca!!!!

            If Not _centroid_calc Then

                Dim mp As New pccGeov0MultiPoint

                For Each objecto As pccGeov0Geometry In _geometries
                    mp.AddPoint(objecto.Centroid)
                Next

                _centroid = New pccGeov0Point(mp.Centroid.X, mp.Centroid.Y)
                _centroid_calc = True

            End If
            Return _centroid
        End Get
    End Property

    Public Overrides Function IsMulti() As Boolean
        Return True
    End Function

    Public Overrides Function Definition() As String

        Return "COL|" & _kgeometries.ToString & "|" & Centroid.X.ToString & "|" & Centroid.Y.ToString & "|" & Centroid.Z.ToString & "|" & Area.ToString & "|" & Length.ToString

    End Function

End Class


