Imports System.Net
Imports System.Drawing
Imports System.IO
Imports System.Xml
Imports pccBase
Imports OSGeo.MapGuide
Imports System.Configuration
Imports pccGeoMetadata
Imports pccDB
Imports System.Runtime.Serialization

<DataContract()>
Public Enum pccMapServerType

    MapGuideEnterprise = 1

End Enum
<DataContract()>
Public Enum pccMapObjectTypes

    Null = 0
    [Boolean] = 1
    [Byte] = 2
    [DateTime] = 3
    [Single] = 4
    [Double] = 5
    Int16 = 6
    Int32 = 7
    Int64 = 8
    [String] = 9
    Blob = 10
    Clob = 11
    Feature = 12
    Geometry = 13
    Raster = 14

End Enum
<DataContract()>
Public Enum pccSizeContext
    Null = 0
    DeviceUnits = 1
    MapUnits = 2
End Enum
<DataContract()>
Public Enum pccLineStyle
    Null = 0
    Solid = 1
End Enum
<DataContract()>
Public Enum pccUnits
    Null = 0
    Points = 1
    Inches = 2
    Feet = 3
    Yards = 4
    Millimeters = 5
    Centimeters = 6
    Meters = 7
    Kilometers = 8
End Enum

<DataContract()>
Public Enum pccMapLayerType
    Null = 0
    Vector = 1
    Raster = 2
End Enum

Public Interface IG10IMapServer

    Property TestConnect() As Boolean
    Property AgentURL() As String
    ReadOnly Property SiteConnection() As Object
    ReadOnly Property SessionID() As String
    ReadOnly Property Message() As String

End Interface

Public Interface IG10IMap

    ' TODO: Issue #18 - Incluir métodos de conversão de coordenadas; talvez em pccMapView 

    Property SuportaSQL(p_layername As String) As Boolean

    Property ObjectosCorrectos(p_layername As String) As Boolean

    Function GetObjectsIntersection(p_layername As String, p_selectgeometry As pccGeoGeometry, Optional contador As Integer = 0) As pccMapguideObject()

    Function GetLayerDefinition(p_layername As String) As String

    Property GetLegend(ByVal p_imagewidth As Long, ByVal p_imageheight As Long) As Byte()
    Property ZoomScale(ByVal p_center As pccGeoPoint, ByVal p_newscale As Long, ByVal p_imagewidth As Long, ByVal p_imageheight As Long) As Byte()
    Property ZoomScaleDPI(ByVal p_center As pccBase.pccGeoPoint, ByVal p_newscale As Long, ByVal p_imagewidth As Long, ByVal p_imageheight As Long, ByVal DPI As Long) As Byte()
    Property ZoomScaleDPI_url(ByVal p_center As pccBase.pccGeoPoint, ByVal p_newscale As Long, ByVal p_imagewidth As Long, ByVal p_imageheight As Long, ByVal DPI As Long) As String
    Property LayerBaseShowHide(ByVal p_center As pccBase.pccGeoPoint, ByVal p_newscale As Long, ByVal p_imagewidth As Long, ByVal p_imageheight As Long, ByVal DPI As Long, ByVal showgroups As String, ByVal hidegroups As String) As Byte()

    Property ZoomScaleBase64(ByVal p_center As pccGeoPoint, ByVal p_newscale As Long, ByVal p_imagewidth As Long, ByVal p_imageheight As Long) As String
    Property ZoomWindow(ByVal p_center As pccGeoPoint, ByVal p_mapwidth As Double, ByVal p_mapheight As Double, ByVal p_imagewidth As Long, ByVal p_imageheight As Long) As Byte()
    Property ZoomWindowBase64(ByVal p_center As pccGeoPoint, ByVal p_mapwidth As Double, ByVal p_mapheight As Double, ByVal p_imagewidth As Long, ByVal p_imageheight As Long) As String
    Function AddLayerfromServer(ByVal layerdefinition As String, ByVal name As String, ByVal legendlabel As String, Optional ByVal order As Integer = -1, Optional ByVal selectable As Boolean = True) As Boolean
    Function AddLayerfromFile(ByVal layerdefinition As String, ByVal name As String, ByVal legendlabel As String, Optional ByVal order As Integer = -1, Optional ByVal selectable As Boolean = True) As Boolean
    Property GetLayer(ByVal p_name As String) As pccMapguideLayer

    Property GetGrupo(ByVal p_name As String) As pccMapguideGroup

    Property GetLayerBase(ByVal p_name As String) As pccMapguideBaseLayer

    Property GetLayers() As pccMapLayerSet
    Property GetLayersBase() As pccMapBaseLayerSet
    Function GetLayersOn() As pccMapLayerSet
    Function GetLayersWithVisibleObjects() As pccMapLayerSet
    Property GetInitialView() As pccMapView
    Property GetExtent() As pccGeoRectangle
    Property GetExtentCenter() As pccGeoPoint
    Property ActualSelection() As Object
    ReadOnly Property LastException() As Exception
    ReadOnly Property Erro() As Boolean
    Sub Save()
    Sub SetInitialView(ByRef p_view As pccMapView)
    Function GetActualView() As pccMapView
    Sub SetActualView(ByVal p_view As pccMapView)
    Sub Refresh()
    Function GetObjects(ByVal p_layername As String, ByVal p_selectgeometry As pccGeoGeometry, Optional ByVal contador As Integer = 0, Optional ByVal getAllProp As Boolean = True) As pccMapguideObject()
    Function GetAllObjects(ByVal p_layername As String, Optional ByVal contador As Integer = 0, Optional ByVal getAllProp As Boolean = True) As pccMapguideObject()
    Function SelectionByParametersQuery(ByVal p_layername As String, ByVal parametersexpression As String, Optional ByVal resetSelection As Boolean = True) As Boolean
    Function SelectionBySpatialQuery(ByVal p_layername As String, ByVal p_selectgeometry As pccGeoGeometry, Optional ByVal resetSelection As Boolean = True) As Boolean
    Function SelectionClear() As Boolean
    Function GetObjectsEx(ByVal p_selectgeometry As pccGeoGeometry, Optional ByVal getAllProp As Boolean = True) As pccMapguideObject()
    Function GetObjectOnTop(ByVal p_selectgeometry As pccGeoGeometry) As pccMapguideObject
    Function GetSelectedObject(ByVal p_selectgeometry As pccGeoGeometry, ByRef conn As pccDB.Connection, ByRef trans As IDbTransaction) As Boolean
    Function GetMapServer() As pccMapguideServer
    Function GetMetricCoordinateSystem() As mtdReferenceSystemInfo
    Function GetCoordinateSystem() As mtdReferenceSystemInfo
    Function CreateCoordinateSystem(ByVal p_cswkt As String) As mtdReferenceSystemInfo
    Function CreateCoordinateSystem(ByVal p_epsgcode As Integer) As mtdReferenceSystemInfo
    Function Transform(ByRef p_geometry As pccGeoPoint, ByRef p_cssrc As mtdReferenceSystemInfo, ByRef p_csdst As mtdReferenceSystemInfo) As pccGeoPoint
    Function PlotDWF() As Byte()
    Function RemoveLayer(ByVal layer As pccMapguideLayer) As Boolean
    Function getMapXMLDefinition() As String
    Function GetMetersPerUnit() As Double
    Function ResourceService() As MgResourceService
    Function IsBaseLayer(ByVal p_layername As String) As Boolean
    Function GetGruposBaseNome() As String
    Function GetLayersBaseNome() As String
End Interface

Public Interface IG10IMapGroup

    Property Name() As String
    Property LegendLabel() As String
    Property GetVisibility() As Boolean
    Property IsVisible() As Boolean
    Property GetSelectable() As Boolean
    Sub SetVisibility(ByVal p_visible As Boolean)
    'Property IsBaseLayer() As Boolean

End Interface

Public Interface IG10IMapLayer
    Inherits IG10IMapGroup

    ReadOnly Property [Object]() As Object
    ReadOnly Property Group() As IG10IMapGroup
    Property GetLayerType As Integer
    Property Id() As String
    'Property IsBaseLayer() As Boolean

End Interface

Public Interface IG10IMapBaseLayer
    Inherits IG10IMapGroup

    ReadOnly Property [Object]() As Object

    Property Id() As String
End Interface


Public Interface IG10IMapLayerDefinition

    Property Filter As String
    Property GeometryField As String
    Property UrlField As String
    Property TooltipField As String

End Interface

Public Interface IG10LineSymbolization
    Property LineStyle As pccLineStyle
    Property Thickness As Long
    Property Color As Long
    Property Unit As pccUnits
    Property SizeContext As pccSizeContext
End Interface

Public Interface IG10IMapObject

    Property Id() As String
    Property Name() As String
    Property Url() As String
    Property Geom() As pccGeoGeometry
    Property Layer() As pccMapguideLayer
    Property Properties() As pccMapObjectProperty()

End Interface

<DataContract()>
Public Class pccScaleRange
    Private _min As Double
    Private _max As Double ' <=0 means infinity
    Private Shared ReadOnly _locker As New Object()
    Public Sub New()
        SyncLock _locker
            _min = 0
            _max = -1
        End SyncLock
    End Sub
    Public Sub New(ByVal p_min As Double, ByVal p_max As Double)
        SyncLock _locker
            _min = p_min
            _max = p_max
        End SyncLock
    End Sub
    <DataMember()>
    Public Property Min As Double
        Get
            SyncLock _locker
                Return _min
            End SyncLock
        End Get
        Set(ByVal value As Double)
            SyncLock _locker
                _min = value
            End SyncLock
        End Set
    End Property
    <DataMember()>
    Public Property Max As Double
        Get
            SyncLock _locker
                Return _max
            End SyncLock
        End Get
        Set(ByVal value As Double)
            SyncLock _locker
                _max = value
            End SyncLock
        End Set
    End Property
    Public ReadOnly Property MaxIsInfinity As Boolean
        Get
            SyncLock _locker
                Return (_max <= 0)
            End SyncLock
        End Get
    End Property
End Class


<DataContract()>
<KnownType(GetType(List(Of pccMapguideLayer)))>
<KnownType(GetType(pccMapguideLayer))>
Public Class pccMapLayerSet
    Private _layers As List(Of pccMapguideLayer)
    Private Shared ReadOnly _locker As New Object()
    Public Sub New()
        SyncLock _locker
            _layers = New List(Of pccMapguideLayer)
        End SyncLock
    End Sub
    <DataMember()>
    Public Property Layers As List(Of pccMapguideLayer)
        Get
            SyncLock _locker
                Return _layers
            End SyncLock
        End Get
        Set(ByVal value As List(Of pccMapguideLayer))
            SyncLock _locker
                _layers = value
            End SyncLock
        End Set
    End Property

    Public Function Size() As Long
        SyncLock _locker
            Return _layers.Count
        End SyncLock
    End Function

    Public Sub Add(ByRef p_layer As IG10IMapLayer)
        SyncLock _locker
            _layers.Add(p_layer)
        End SyncLock
    End Sub

    Public ReadOnly Property Item(ByVal p_idx As Long) As IG10IMapLayer
        Get
            SyncLock _locker
                If p_idx >= Size() Then
                    Return Nothing
                Else
                    Return _layers.Item(p_idx)
                End If
            End SyncLock
        End Get
    End Property

End Class

<DataContract()>
<KnownType(GetType(List(Of pccMapguideBaseLayer)))>
<KnownType(GetType(pccMapguideBaseLayer))>
Public Class pccMapBaseLayerSet
    Private _layers As List(Of pccMapguideBaseLayer)
    Private Shared ReadOnly _locker As New Object()
    Public Sub New()
        SyncLock _locker
            _layers = New List(Of pccMapguideBaseLayer)
        End SyncLock
    End Sub
    <DataMember()>
    Public Property Layers As List(Of pccMapguideBaseLayer)
        Get
            SyncLock _locker
                Return _layers
            End SyncLock
        End Get
        Set(ByVal value As List(Of pccMapguideBaseLayer))
            SyncLock _locker
                _layers = value
            End SyncLock
        End Set
    End Property

    Public Function Size() As Long
        SyncLock _locker
            Return _layers.Count
        End SyncLock
    End Function

    Public Sub Add(ByRef p_layer As IG10IMapBaseLayer)
        SyncLock _locker
            _layers.Add(p_layer)
        End SyncLock
    End Sub

    Public ReadOnly Property Item(ByVal p_idx As Long) As IG10IMapBaseLayer
        Get
            SyncLock _locker
                If p_idx >= Size() Then
                    Return Nothing
                Else
                    Return _layers.Item(p_idx)
                End If
            End SyncLock
        End Get
    End Property

End Class

<DataContract(), KnownType(GetType(pccGeoMultiPoint)), KnownType(GetType(pccGeoPoint))>
<KnownType(GetType(pccGeoMultiLineString)), KnownType(GetType(pccGeoLineString))>
<KnownType(GetType(pccGeoMultiPolygon)), KnownType(GetType(pccGeoPolygon))>
<KnownType(GetType(pccGeoCollection)), KnownType(GetType(pccGeoRectangle))>
<Serializable()>
Public Class pccMapView

    Private _center As pccGeoPoint ' metricCS
    Private _dpi As Long
    Private _mapwidth As Double ' metricCS
    Private _mapheight As Double ' metricCS
    Private _imagewidth As Long
    Private _imageheight As Long
    Private _scale As Double
    Private Shared ReadOnly _locker As New Object()
    Public Sub New()

    End Sub

    Public Sub New(ByVal p_center As pccGeoPoint, ByVal p_scale As Long, ByVal p_dpi As Long, ByVal p_mapwidth As Double, ByVal p_mapheight As Double, ByVal p_imagewidth As Double, ByVal p_imageheight As Double)
        SyncLock _locker
            Center = p_center
            Dpi = p_dpi
            MapWidth = p_mapwidth
            MapHeight = p_mapheight
            ImageWidth = p_imagewidth
            ImageHeight = p_imageheight
            Scale = p_scale
        End SyncLock
    End Sub
    <DataMember()>
    Public Property Scale() As Double
        Get
            SyncLock _locker
                Return _scale
            End SyncLock
        End Get
        Set(ByVal value As Double)
            SyncLock _locker
                _scale = value
            End SyncLock
        End Set
    End Property

    ' TODO: Em WGS84, zonas do ecran afastadas do centro (em X) causam uma conversão com erro de escala em X
    Public Function GetCoordsImage2Map(ByRef p_coords_image As pccGeoPoint, ByRef m As pccMapguideMap, ByVal p_imagewidth As Double, ByVal p_imageheight As Double) As pccGeoPoint
        SyncLock _locker
            ' cs of the map
            Dim mapCS As mtdReferenceSystemInfo = m.GetCoordinateSystem()
            Dim oSrcCS As MgCoordinateSystem = mapCS.Object

            ' temporary metric CS
            Dim oMetrCS As mtdReferenceSystemInfo = m.GetMetricCoordinateSystem

            ' coordinates os the map's center in metric CS
            Dim pDst As New pccGeoPoint
            pDst = m.Transform(Center, mapCS, m.GetMetricCoordinateSystem)

            Dim nX As Double = pDst.X + (((p_coords_image.X - p_imagewidth / 2) / 96) * 2.54 * Scale) / 100
            Dim nY As Double = pDst.Y + (((p_imageheight - p_coords_image.Y - p_imageheight / 2) / 96) * 2.54 * Scale) / 100


            ' coordinates of the ecranPoint
            Dim pSrc As pccGeoPoint = New pccGeoPoint(nX, nY)

            If mapCS.IsEqual(oMetrCS) Then
                Return pSrc
            Else
                ' world coordinates of the image Point
                Return m.Transform(pSrc, oMetrCS, mapCS)
            End If
        End SyncLock
    End Function

    ' TODO: Deve estar muito mal; nenhum uso ainda; mas tem que se mexer nisto
    Public Function GetCoordsMap2Image(ByRef p_coords_map As pccGeoPoint, ByRef m As pccMapguideMap, ByVal p_imagewidth As Double, ByVal p_imageheight As Double) As pccGeoPoint
        SyncLock _locker
            ' cs of the map
            Dim oMapCS As mtdReferenceSystemInfo = m.GetCoordinateSystem()
            ' temporary metric CS
            Dim oMetrCS As mtdReferenceSystemInfo = m.GetMetricCoordinateSystem()

            Dim pSrc As pccGeoPoint
            Dim pCenter As New pccGeoPoint(Center.X, Center.Y)

            If oMapCS.IsEqual(oMetrCS) Then
                pSrc = p_coords_map
            Else
                pSrc = m.Transform(p_coords_map, oMapCS, oMetrCS)
                pCenter = m.Transform(pCenter, oMapCS, oMetrCS)
            End If
            Dim aX As Double = (p_imagewidth / 2) + (((pSrc.X - pCenter.X) * 96 * 100) / (2.54 * Scale))
            Dim aY As Double = (p_imageheight / 2) - (((pSrc.Y - pCenter.Y) * 96 * 100) / (2.54 * Scale))
            Return New pccGeoPoint(aX, aY)
        End SyncLock
    End Function
    <DataMember()>
    Public Property Center() As pccGeoPoint
        Get
            SyncLock _locker
                Return _center
            End SyncLock
        End Get
        Set(ByVal value As pccGeoPoint)
            SyncLock _locker
                _center = value
            End SyncLock
        End Set
    End Property
    <DataMember()>
    Public Property Dpi() As Long
        Get
            SyncLock _locker
                Return _dpi
            End SyncLock
        End Get
        Set(ByVal value As Long)
            SyncLock _locker
                _dpi = value
            End SyncLock
        End Set
    End Property
    <DataMember()>
    Public Property MapWidth() As Double
        Get
            SyncLock _locker
                Return _mapwidth
            End SyncLock
        End Get
        Set(ByVal value As Double)
            SyncLock _locker
                _mapwidth = value
            End SyncLock
        End Set
    End Property
    <DataMember()>
    Public Property MapHeight() As Double
        Get
            SyncLock _locker
                Return _mapheight
            End SyncLock
        End Get
        Set(ByVal value As Double)
            SyncLock _locker
                _mapheight = value
            End SyncLock
        End Set
    End Property
    <DataMember()>
    Public Property ImageWidth() As Long
        Get
            SyncLock _locker
                Return _imagewidth
            End SyncLock
        End Get
        Set(ByVal value As Long)
            SyncLock _locker
                _imagewidth = value
            End SyncLock
        End Set
    End Property
    <DataMember()>
    Public Property ImageHeight() As Long
        Get
            SyncLock _locker
                Return _imageheight
            End SyncLock
        End Get
        Set(ByVal value As Long)
            SyncLock _locker
                _imageheight = value
            End SyncLock
        End Set
    End Property

End Class

<DataContract()>
<Serializable()>
Public Class pccMapObjectType

    Private _type As Integer
    Private Shared ReadOnly _locker As New Object()
    Public Sub New()

    End Sub

    Public Sub New(ByVal p_servertype As pccMapServerType, ByVal p_type As Object)
        SyncLock _locker
            Select Case p_servertype

                Case pccMapServerType.MapGuideEnterprise
                    Select Case CType(p_type, Integer)
                        Case 0 : _type = pccMapObjectTypes.Null
                        Case 1 : _type = pccMapObjectTypes.Boolean
                        Case 2 : _type = pccMapObjectTypes.Byte
                        Case 3 : _type = pccMapObjectTypes.DateTime
                        Case 4 : _type = pccMapObjectTypes.Single
                        Case 5 : _type = pccMapObjectTypes.Double
                        Case 6 : _type = pccMapObjectTypes.Int16
                        Case 7 : _type = pccMapObjectTypes.Int32
                        Case 8 : _type = pccMapObjectTypes.Int64
                        Case 9 : _type = pccMapObjectTypes.String
                        Case 10 : _type = pccMapObjectTypes.Blob
                        Case 11 : _type = pccMapObjectTypes.Clob
                        Case 12 : _type = pccMapObjectTypes.Feature
                        Case 13 : _type = pccMapObjectTypes.Geometry
                        Case 14 : _type = pccMapObjectTypes.Raster
                        Case Else
                            Throw New Exception("SRC: pccMapObjectType.New Invalid p_type")
                    End Select

            End Select
        End SyncLock
    End Sub

    Public Overrides Function ToString() As String
        SyncLock _locker
            Return _type.ToString
        End SyncLock
    End Function

End Class

<DataContract()>
<Serializable()>
Public Class pccMapObjectProperty

    Private _name As String
    Private _type As pccMapObjectType
    Private _value As Object
    Private Shared ReadOnly _locker As New Object()
    Public Sub New()
    End Sub

    Public Sub New(ByVal p_name As String, ByVal p_type As pccMapObjectType, ByVal p_value As Object)
        SyncLock _locker
            _name = p_name
            _type = p_type
            _value = p_value
        End SyncLock
    End Sub
    <DataMember()>
    Public Property Name() As String
        Get
            SyncLock _locker
                Return _name
            End SyncLock
        End Get
        Set(ByVal value As String)

        End Set
    End Property
    <DataMember()>
    Public Property Type() As pccMapObjectType
        Get
            SyncLock _locker
                Return _type
            End SyncLock
        End Get
        Set(ByVal value As pccMapObjectType)

        End Set
    End Property
    '<DataMember()> _
    Public Property Value() As Object
        Get
            SyncLock _locker
                Return _value
            End SyncLock
        End Get
        Set(ByVal value As Object)
            SyncLock _locker
                _value = value
            End SyncLock
        End Set
    End Property

End Class

<DataContract()>
<Serializable()>
Public Class pccMapguideServer
    Implements IG10IMapServer

    Private _sessionid As String
    Private _siteConnection As Object
    Private _message As String
    Private _agentUrl As String
    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

    End Sub

    'Public Sub New(ByVal p_configFile As String, ByVal p_user As String, ByVal p_password As String)

    '    Try

    '        MapGuideApi.MgInitializeWebTier(p_configFile)

    '        Dim userInfo As MgUserInformation = New MgUserInformation(p_user, p_password)

    '        Dim site As MgSite = New MgSite()
    '        site.Open(userInfo)
    '        _sessionid = site.CreateSession()

    '        _siteConnection = New MgSiteConnection()
    '        _siteConnection.Open(userInfo)

    '        _weburl = ""
    '    Catch ex As Exception
    '        Dim a As Exception = ex.InnerException
    '        _message = ex.Message.ToString & " " & ex.InnerException.ToString & " " & ex.InnerException.InnerException.ToString


    '    End Try

    'End Sub

    Public Sub New(ByVal p_configFile As String, ByVal p_user As String, ByVal p_password As String, ByVal AgentUrl As String)
        SyncLock _locker
            Try

                MapGuideApi.MgInitializeWebTier(p_configFile)

                Dim userInfo As MgUserInformation = New MgUserInformation(p_user, p_password)

                Dim site As MgSite = New MgSite()
                site.Open(userInfo)
                _sessionid = site.CreateSession()

                _siteConnection = New MgSiteConnection()
                _siteConnection.Open(userInfo)
                _agentUrl = AgentUrl
            Catch ex As Exception
                Dim a As Exception = ex.InnerException
                _message = ex.Message.ToString & " " & ex.InnerException.ToString & " " & ex.InnerException.InnerException.ToString


            End Try
        End SyncLock
    End Sub
    Public Sub New(ByVal p_configFile As String, ByVal sessionId As String, ByVal AgentUrl As String)
        SyncLock _locker
            Try

                MapGuideApi.MgInitializeWebTier(p_configFile)

                Dim userInfo As MgUserInformation = New MgUserInformation(sessionId)

                _siteConnection = New MgSiteConnection()
                _siteConnection.Open(userInfo)

                _agentUrl = AgentUrl
                _sessionid = sessionId

            Catch ex As Exception
                Dim a As Exception = ex.InnerException
                _message = ex.Message.ToString & " " & ex.InnerException.ToString & " " & ex.InnerException.InnerException.ToString


            End Try
        End SyncLock
    End Sub
    Public ReadOnly Property Message() As String Implements IG10IMapServer.Message
        Get
            SyncLock _locker
                Return _message
            End SyncLock
        End Get
    End Property
    <DataMember()>
    Public Property TestConnect() As Boolean Implements IG10IMapServer.TestConnect

        Get
            SyncLock _locker
                Try
                    Dim o As MgSite
                    o = _siteConnection.GetSite()
                    Return o IsNot Nothing

                Catch ex As Exception

                    Return False

                End Try
            End SyncLock
        End Get
        Set(ByVal value As Boolean)

        End Set

    End Property

    Public ReadOnly Property SiteConnection() As Object Implements IG10IMapServer.SiteConnection
        Get
            SyncLock _locker
                Return _siteConnection
            End SyncLock
        End Get
    End Property
    <DataMember()>
    Public Property AgentURL() As String Implements IG10IMapServer.AgentURL

        Get
            SyncLock _locker
                Return _agentUrl
            End SyncLock

        End Get
        Set(ByVal value As String)

        End Set

    End Property


    Public ReadOnly Property SessionID() As String Implements IG10IMapServer.SessionID
        Get
            SyncLock _locker
                Return _sessionid
            End SyncLock
        End Get
    End Property

End Class

<DataContract(), KnownType(GetType(pccGeoMultiPoint)), KnownType(GetType(pccGeoPoint))>
<KnownType(GetType(pccGeoMultiLineString)), KnownType(GetType(pccGeoLineString))>
<KnownType(GetType(pccGeoMultiPolygon)), KnownType(GetType(pccGeoPolygon))>
<KnownType(GetType(pccGeoCollection)), KnownType(GetType(pccGeoRectangle))>
<Serializable()>
Public Class pccMapguideMap

    Implements IG10IMap

    Private _mapserver As pccMapguideServer
    Private _mapname As String
    Private _resourceservice As MgResourceService
    Private _featureservice As MgFeatureService
    Private _map As MgMap
    Private _mapdefid As MgResourceIdentifier
    Private _mapid As MgResourceIdentifier
    Private _selid As MgResourceIdentifier
    Private _cs As mtdReferenceSystemInfo
    Private _csmetric As mtdReferenceSystemInfo
    Private _actualview As pccMapView
    Private _initialview As pccMapView
    Private _layers As pccMapLayerSet
    Private _layersbase As pccMapBaseLayerSet
    Private _grupos As pccMapGroupLayerSet
    Private _grupobasenome As String
    Private _layersbasenome As String
    Private _extent As pccGeoRectangle
    Private _actual_selection As MgSelection

    Private _lastexception As Exception
    Private _erro As Boolean

    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

    End Sub
    Public Sub Save() Implements IG10IMap.Save
        _map.Save()
    End Sub
    Public Sub New(ByRef p_mapserver As pccMapguideServer, ByVal p_mapdefinition As String, ByVal p_mapName As String, ByVal p_metriccs_wkt As String)
        SyncLock _locker
            Try
                _mapserver = p_mapserver
                _mapname = p_mapName
                _resourceservice = _mapserver.SiteConnection.CreateService(MgServiceType.ResourceService)
                _featureservice = _mapserver.SiteConnection.CreateService(MgServiceType.FeatureService)

                ' _map = New MgMap()
                _map = New MgMap(_mapserver.SiteConnection)
                _mapdefid = New MgResourceIdentifier(p_mapdefinition)
                _map.Create(_resourceservice, _mapdefid, p_mapName)

                _mapid = New MgResourceIdentifier("Session:" & _mapserver.SessionID() & "//" & p_mapName & "." & MgResourceType.Map)
                _map.Save(_resourceservice, _mapid)
                _initialview = New pccMapView(New pccGeoPoint(_map.GetViewCenter.Coordinate.GetX, _map.GetViewCenter.Coordinate.GetY), _map.GetViewScale, _map.GetDisplayDpi, _map.GetMapExtent.GetWidth, _map.GetMapExtent.GetHeight, _map.GetDisplayWidth, _map.GetDisplayHeight)
                _actualview = _initialview


                Dim sel As MgSelection
                sel = New MgSelection(_map)

                _selid = New MgResourceIdentifier("Session:" & _mapserver.SessionID() & "//" & p_mapName & "." & MgResourceType.Selection)
                sel.Save(_resourceservice, _selid)

                '' obtem layers
                'Dim layerscol As MgLayerCollection
                'layerscol = _map.GetLayers()

                '_layers = New pccMapLayerSet
                '_layersbase = New pccMapBaseLayerSet

                'Threading.Tasks.Parallel.ForEach(layerscol, Sub(l)

                '                                                'verificar se o l é base
                '                                                If Not IsBaseLayer(l.Name) Then
                '                                                    Dim obj As pccMapguideLayer = New pccMapguideLayer(l)
                '                                                    obj.IsBaseLayer = False
                '                                                    _layers.Add(obj)
                '                                                Else
                '                                                    Dim obj As pccMapguideLayer = New pccMapguideLayer(l)
                '                                                    obj.IsBaseLayer = True
                '                                                    _layersbase.Add(obj)
                '                                                End If

                '                                            End Sub)

                ''For Each l As MgLayer In layerscol
                ''    _layers.Add(New pccMapguideLayer(l))
                ''Next

                ' obtem layers
                Dim layerscol As MgLayerCollection
                Dim auxlayerscol As MgLayerCollection

                layerscol = _map.GetLayers()
                auxlayerscol = _map.GetLayers()
                'auxlayerscol = layerscol

                _layersbase = New pccMapBaseLayerSet
                _grupos = New pccMapGroupLayerSet

                _grupobasenome = GetGruposBaseNome()
                _layersbasenome = GetLayersBaseNome()

                _layers = New pccMapLayerSet
                _layersbase = New pccMapBaseLayerSet

                For Each group As MgLayerGroup In _map.GetLayerGroups()
                    If InStr(_grupobasenome, "," + group.Name + ",") > 0 Then
                        Dim aux As New pccMapguideBaseLayer(group)
                        _layersbase.Add(aux)
                    Else
                        Dim aux As New pccMapguideGroup(group)
                        _grupos.Add(aux)
                    End If
                Next

                For Each ll As MgLayer In auxlayerscol
                    If InStr(_layersbasenome, "," + ll.Name + ",") > 0 Then
                        Dim aux As New pccMapguideLayer(ll)
                        layerscol.Remove(CType(aux.Object, MgLayer))
                    End If
                Next

                Threading.Tasks.Parallel.ForEach(layerscol, Sub(l)
                                                                _layers.Add(New pccMapguideLayer(l))
                                                            End Sub)

                'Dim srs As String = _map.GetMapSRS()
                '_cs = CreateCoordinateSystem(srs)
                '_csmetric = CreateCoordinateSystem(p_metriccs_wkt)


                Try
                    Dim srs As String = _map.GetMapSRS()
                    _cs = CreateCoordinateSystem(srs)
                Catch ex As Exception
                    _cs = CreateCoordinateSystem(p_metriccs_wkt)
                End Try

                _csmetric = CreateCoordinateSystem(p_metriccs_wkt)



                ' map extent
                Dim x1 As Double = _map.DataExtent.GetLowerLeftCoordinate.X
                Dim y1 As Double = _map.DataExtent.GetLowerLeftCoordinate.Y
                Dim x2 As Double = _map.DataExtent.GetUpperRightCoordinate.X
                Dim y2 As Double = _map.DataExtent.GetUpperRightCoordinate.Y

                _extent = New pccGeoRectangle()
                _extent.AddVertice(New pccGeoPoint(x1, y1))
                _extent.AddVertice(New pccGeoPoint(x2, y1))
                _extent.AddVertice(New pccGeoPoint(x2, y2))
                _extent.AddVertice(New pccGeoPoint(x1, y2))
                _extent.AddVertice(New pccGeoPoint(x1, y1))

                _actual_selection = sel 'New MgSelection(_map)
                _erro = False
                _lastexception = Nothing
            Catch ex As Exception

                Dim a As String = ex.Message.ToString
                _erro = True
                _lastexception = ex
            End Try
        End SyncLock
    End Sub

    Public Sub New(ByRef p_mapserver As IG10IMapServer, ByVal p_mapName As String, ByVal p_metriccs_wkt As String)
        SyncLock _locker
            Try
                Dim map As New MgMap(p_mapserver.SiteConnection)
                map.Open(p_mapName)

                _mapserver = p_mapserver
                _mapname = p_mapName
                _resourceservice = _mapserver.SiteConnection.CreateService(MgServiceType.ResourceService)
                _featureservice = _mapserver.SiteConnection.CreateService(MgServiceType.FeatureService)

                ' _map = New MgMap()
                _map = New MgMap(_mapserver.SiteConnection)
                _map.Open(p_mapName)

                _mapid = New MgResourceIdentifier("Session:" & _mapserver.SessionID() & "//" & p_mapName & "." & MgResourceType.Map)
                _map.Save()
                _initialview = New pccMapView(New pccGeoPoint(_map.GetViewCenter.Coordinate.GetX, _map.GetViewCenter.Coordinate.GetY), _map.GetViewScale, _map.GetDisplayDpi, _map.GetMapExtent.GetWidth, _map.GetMapExtent.GetHeight, _map.GetDisplayWidth, _map.GetDisplayHeight)
                _actualview = _initialview

                Dim sel As MgSelection
                sel = New MgSelection(_map)
                sel.Open(_resourceservice, p_mapName)

                '_selid = New MgResourceIdentifier("Session:" & _mapserver.SessionID() & "//" & p_mapName & "." & MgResourceType.Selection)
                'sel.Save(_resourceservice, _selid) '

                ' obtem layers
                Dim layerscol As MgLayerCollection
                Dim auxlayerscol As New List(Of MgLayer)

                layerscol = _map.GetLayers

                'For Each l As MgLayer In layerscol
                '    auxlayerscol.Add(l)
                'Next

                _layersbase = New pccMapBaseLayerSet
                '_grupobase = New pccMapguideGroup

                _grupobasenome = GetGruposBaseNome()
                _layersbasenome = GetLayersBaseNome()

                _layers = New pccMapLayerSet
                _layersbase = New pccMapBaseLayerSet

                For Each group As MgLayerGroup In _map.GetLayerGroups()
                    If InStr(_grupobasenome, "," + group.Name + ",") > 0 Then
                        Dim aux As New pccMapguideBaseLayer(group)
                        _layersbase.Add(aux)
                    End If
                Next

                'For Each ll As MgLayer In auxlayerscol
                '    If InStr(_layersbasenome, "," + ll.Name + ",") > 0 Then
                '        Dim aux As New pccMapguideLayer(ll)
                '        layerscol.Remove(CType(aux.Object, MgLayer))
                '    End If
                'Next

                Threading.Tasks.Parallel.ForEach(layerscol, Sub(l)
                                                                _layers.Add(New pccMapguideLayer(l))
                                                            End Sub)

                For Each l As MgLayer In layerscol
                    l.GetObjectId()
                Next

                'Dim srs As String = _map.GetMapSRS()
                '_cs = CreateCoordinateSystem(srs)
                '_csmetric = CreateCoordinateSystem(p_metriccs_wkt)



                Try
                    Dim srs As String = _map.GetMapSRS()
                    _cs = CreateCoordinateSystem(srs)
                Catch ex As Exception
                    _cs = CreateCoordinateSystem(p_metriccs_wkt)
                End Try

                _csmetric = CreateCoordinateSystem(p_metriccs_wkt)

                ' map extent
                Dim x1 As Double = _map.DataExtent.GetLowerLeftCoordinate.X
                Dim y1 As Double = _map.DataExtent.GetLowerLeftCoordinate.Y
                Dim x2 As Double = _map.DataExtent.GetUpperRightCoordinate.X
                Dim y2 As Double = _map.DataExtent.GetUpperRightCoordinate.Y

                _extent = New pccGeoRectangle()
                _extent.AddVertice(New pccGeoPoint(x1, y1))
                _extent.AddVertice(New pccGeoPoint(x2, y1))
                _extent.AddVertice(New pccGeoPoint(x2, y2))
                _extent.AddVertice(New pccGeoPoint(x1, y2))
                _extent.AddVertice(New pccGeoPoint(x1, y1))

                _actual_selection = sel 'New MgSelection(_map)
                _erro = False
                _lastexception = Nothing
            Catch ex As Exception

                Dim a As String = ex.Message.ToString
                _erro = True
                _lastexception = ex
            End Try
        End SyncLock
    End Sub

    Public ReadOnly Property MapObject As MgMap
        Get
            SyncLock _locker
                Return _map
            End SyncLock
        End Get
    End Property

    Public ReadOnly Property LastException As Exception Implements IG10IMap.LastException
        Get
            SyncLock _locker
                Return _lastexception
            End SyncLock
        End Get
    End Property
    Public ReadOnly Property Erro As Boolean Implements IG10IMap.Erro
        Get
            SyncLock _locker
                Return _erro
            End SyncLock
        End Get
    End Property
    Public Function ResourceService() As MgResourceService Implements IG10IMap.ResourceService
        SyncLock _locker
            Return _resourceservice
        End SyncLock
    End Function
    <DataMember()>
    Public Property TestConnect() As Boolean
        Get
            SyncLock _locker
                Return _mapserver.TestConnect()

            End SyncLock
        End Get
        Set(ByVal value As Boolean)
            Throw New Exception("pcc by design: pccMapguideMap.TestConnect() is a readonly property.")
        End Set
    End Property

    Public Function GetActualView() As pccMapView Implements IG10IMap.GetActualView
        SyncLock _locker
            Return _actualview
        End SyncLock
    End Function

    Public Sub SetActualView(ByVal p_view As pccMapView) Implements IG10IMap.SetActualView
        SyncLock _locker
            _actualview = p_view
        End SyncLock
    End Sub

    <DataMember()>
    Public Property GetInitialView() As pccMapView Implements IG10IMap.GetInitialView
        Get
            SyncLock _locker
                Return _initialview
            End SyncLock
        End Get
        Set(ByVal value As pccMapView)

        End Set
    End Property

    Public Sub Refresh() Implements IG10IMap.Refresh
        SyncLock _locker
            _actualview = _initialview

            ' obtem layers
            Dim layerscol As MgLayerCollection
            layerscol = _map.GetLayers()

            _layers = New pccMapLayerSet

            For Each l As MgLayer In layerscol
                _layers.Add(New pccMapguideLayer(l))
            Next
        End SyncLock
    End Sub

    Public Function RemoveLayer(ByVal layer As pccMapguideLayer) As Boolean Implements IG10IMap.RemoveLayer
        SyncLock _locker
            Dim layerscol As MgLayerCollection
            layerscol = _map.GetLayers()

            Return layerscol.Remove(CType(layer.Object, MgLayer))
        End SyncLock
    End Function

    Public Sub SetInitialView(ByRef p_view As pccMapView) Implements IG10IMap.SetInitialView
        SyncLock _locker
            _initialview = p_view
        End SyncLock
    End Sub

    Public Function getMapXMLDefinition() As String Implements IG10IMap.getMapXMLDefinition
        SyncLock _locker
            Try

                Dim mapadefRI As MgResourceIdentifier
                mapadefRI = Me.MapObject.GetMapDefinition

                Dim br As MgByteReader
                br = Me.ResourceService.GetResourceContent(mapadefRI)

                Dim doc As New XmlDocument
                Dim mapadefStringXML As String = br.ToString
                _erro = False
                _lastexception = Nothing

                Return mapadefStringXML

            Catch ex As Exception
                _erro = True
                _lastexception = ex
                Return ""

            End Try
        End SyncLock
    End Function

    Public Function GetObjects(ByVal p_layername As String, ByVal p_selectgeometry As pccBase.pccGeoGeometry, Optional ByVal contador As Integer = 0, Optional ByVal getAllProp As Boolean = True) As pccMapguideObject() Implements IG10IMap.GetObjects

        SyncLock _locker
            Dim wktReaderWriter As MgWktReaderWriter = New MgWktReaderWriter()

            Dim sWKT As String = p_selectgeometry.WKT

            Dim filtergeom As MgGeometry
            Dim aux As Integer = 0

            _erro = False
            _lastexception = Nothing

            Try

                filtergeom = wktReaderWriter.Read(sWKT)

                Dim l As pccMapguideLayer = Me.GetLayer(p_layername)

                If l Is Nothing Then

                    Return Nothing

                Else

                    Dim ml As MgLayer = l.Object

                    Dim s As String = ml.GetLegendLabel

                    Dim FeatureGeometryFieldName As String = ml.GetFeatureGeometryName
                    If LCase(FeatureGeometryFieldName) = "image" Or LCase(FeatureGeometryFieldName) = "raster" Then Return Nothing

                    Dim FeatureSrvc As MgFeatureService = Me.GetMapServer.SiteConnection.CreateService(MgServiceType.FeatureService)
                    Dim featureSource As MgResourceIdentifier = New MgResourceIdentifier(ml.GetFeatureSourceId())

                    ' Get Layer CoordinateSystem
                    Dim spatialContextReader As MgSpatialContextReader
                    spatialContextReader = FeatureSrvc.GetSpatialContexts(featureSource, False)
                    spatialContextReader.ReadNext()
                    Dim CoordSyscode As String
                    Dim CoordSyswkt As String
                    CoordSyscode = spatialContextReader.GetCoordinateSystem()
                    CoordSyswkt = spatialContextReader.GetCoordinateSystemWkt()
                    Dim label As String = ""
                    Dim csmapa As pccGeoMetadata.mtdReferenceSystemInfo = Me.GetCoordinateSystem()
                    If csmapa.WKT <> CoordSyswkt Then
                        Try

                            Dim csforigem As New OSGeo.MapGuide.MgCoordinateSystemFactory
                            Dim codeorigem As String = csforigem.ConvertWktToCoordinateSystemCode(CoordSyswkt)
                            Dim epsgcodeorigem As String = csforigem.ConvertWktToEpsgCode(CoordSyswkt)
                            Dim rsorigem As New pccGeoMetadata.mtdReferenceSystemInfo(CoordSyswkt, csforigem.CreateFromCode(codeorigem))
                            rsorigem.Code = epsgcodeorigem
                            rsorigem.CodeSpace = "EPSG"

                            Dim csfdestino As New OSGeo.MapGuide.MgCoordinateSystemFactory
                            Dim codedestino As String = csfdestino.ConvertWktToCoordinateSystemCode(csmapa.WKT)
                            Dim epsgcodedestino As String = csfdestino.ConvertWktToEpsgCode(csmapa.WKT)
                            Dim rsdestino As New pccGeoMetadata.mtdReferenceSystemInfo(csmapa.WKT, csfdestino.CreateFromCode(codedestino))
                            rsdestino.Code = epsgcodedestino
                            rsdestino.CodeSpace = "EPSG"
                            Dim ggg As Boolean = (epsgcodeorigem <> "0" And epsgcodedestino <> "0")
                            If ggg Then
                                wktReaderWriter = New OSGeo.MapGuide.MgWktReaderWriter
                                Dim geosrc As OSGeo.MapGuide.MgGeometry = wktReaderWriter.Read(p_selectgeometry.WKT)

                                Dim mf As New OSGeo.MapGuide.MgCoordinateSystemFactory
                                Dim t As OSGeo.MapGuide.MgCoordinateSystemTransform

                                t = mf.GetTransform(rsdestino.Object, rsorigem.Object)

                                Dim coordsrc As OSGeo.MapGuide.MgCoordinate = geosrc.Centroid.Coordinate

                                t.TransformCoordinate(coordsrc)
                                Dim ponto3 As pccBase.pccGeoPoint = New pccGeoPoint(coordsrc.X, coordsrc.Y)

                                wktReaderWriter = New OSGeo.MapGuide.MgWktReaderWriter()
                                filtergeom = wktReaderWriter.Read(ponto3.WKT)
                            End If
                        Catch ex As Exception
                            Dim k_objects1 As Integer = 0
                        End Try
                    End If
                    Dim qryoptions As New MgFeatureQueryOptions()

                    If filtergeom.GetGeometryType = 1 Then
                        qryoptions.SetSpatialFilter(FeatureGeometryFieldName, filtergeom.Buffer(Me._actualview.Scale / 500, Nothing), MgFeatureSpatialOperations.Intersects)
                    Else
                        qryoptions.SetSpatialFilter(FeatureGeometryFieldName, filtergeom, MgFeatureSpatialOperations.Intersects)
                    End If
                    qryoptions.SetFilter(ml.Filter)


                    'Dim featureSource As MgResourceIdentifier = New MgResourceIdentifier(ml.GetFeatureSourceId())
                    Dim FeatureClassName As String = ml.GetFeatureClassName()

                    'Dim FeatureSrvc As MgFeatureService = Me.GetMapServer.SiteConnection.CreateService(MgServiceType.FeatureService)
                    Dim features As MgFeatureReader
                    Try

                        features = FeatureSrvc.SelectFeatures(featureSource, FeatureClassName, qryoptions)
                        _erro = False
                        _lastexception = Nothing
                    Catch ex As Exception
                        _erro = True
                        _lastexception = ex
                    End Try

                    Dim k_objects As Integer = 0
                    Dim sair As Boolean = False
                    Dim a_objects As pccMapguideObject() = Nothing
                    Dim featureWKT As String = ""
                    If features IsNot Nothing Then
                        Dim entrou As Boolean = False

                        While features.ReadNext And Not sair
                            entrou = True
                            ReDim Preserve a_objects(k_objects)

                            a_objects(k_objects) = New pccMapguideObject()

                            Dim k_prop As Integer = features.GetPropertyCount()
                            Dim a_prop As pccMapObjectProperty()
                            ReDim a_prop(k_prop - 1)
                            Dim propName As String = "geom"
                            Dim aaaa As Boolean = False

                            ' If getAllProp Then
                            For j As Integer = 0 To k_prop - 1
                                propName = features.GetPropertyName(j)
                                If features.GetPropertyType(propName) = 13 Then
                                    aaaa = True
                                    Exit For
                                End If
                            Next
                            If Not aaaa Then
                                propName = "geom"
                            End If
                            'End If
                            Try

                                ' teste
                                Dim byteReader As MgByteReader = features.GetGeometry(propName)
                                Dim geometryReaderAgf As MgAgfReaderWriter = New MgAgfReaderWriter()
                                Dim geometryReaderWkt As MgWktReaderWriter = New MgWktReaderWriter()
                                Dim geometry As MgGeometry = geometryReaderAgf.Read(byteReader)

                                Dim b As New pccGeoUtils

                                featureWKT = geometryReaderWkt.Write(geometry)

                                Dim geometry1 As MgGeometry = geometry.Intersection(filtergeom)
                                Dim interseccaoWKT As String = geometryReaderWkt.Write(geometry1)

                                a_objects(k_objects).Geom = b.GeometryFromWKT(featureWKT)
                            Catch ex As Exception
                                featureWKT = ex.Message
                                featureWKT = ""

                            End Try

                            If getAllProp Then
                                For j As Integer = 0 To k_prop - 1

                                    Dim o_value As Object
                                    propName = features.GetPropertyName(j)

                                    If features.IsNull(propName) Then
                                        o_value = "null"
                                    Else
                                        Select Case features.GetPropertyType(propName) '9: strings ; 13: geometries ; 7: numbers
                                            Case 9
                                                o_value = features.GetString(propName)
                                            Case 7
                                                o_value = features.GetInt32(propName)
                                            Case 13
                                                Dim byteReader As MgByteReader = features.GetGeometry(propName)
                                                Dim geometryReaderAgf As MgAgfReaderWriter = New MgAgfReaderWriter()
                                                Dim geometryReaderWkt As MgWktReaderWriter = New MgWktReaderWriter()
                                                Dim geometry As MgGeometry = geometryReaderAgf.Read(byteReader)

                                                Dim b As New pccGeoUtils

                                                featureWKT = geometryReaderWkt.Write(geometry)
                                                Dim a As New pccGeoUtils

                                                'o_value = a.GeometryFromWKT(propName)
                                                o_value = a.GeometryFromWKT(featureWKT)
                                            'o_value = byteReader.ToString()
                                            Case 5
                                                o_value = features.GetDouble(propName)
                                            Case Else
                                                o_value = "?"
                                        End Select
                                    End If

                                    ' read ToolTip from XML Layer File
                                    Dim laydefRI As MgResourceIdentifier
                                    laydefRI = CType(l.Object, MgLayer).GetLayerDefinition

                                    Dim br As MgByteReader
                                    br = ResourceService.GetResourceContent(laydefRI)

                                    Dim LayerXMLString As String = br.ToString

                                    Dim tooltip_idx1 As Int32 = LayerXMLString.IndexOf("<ToolTip>")
                                    Dim tooltip_idx2 As Int32 = LayerXMLString.IndexOf("</ToolTip>")
                                    Dim tooltip As String = ""
                                    If tooltip_idx1 = -1 Or tooltip_idx2 = -1 Then

                                        tooltip = "" ' <ToolTip> - 9 caracteres
                                    Else
                                        tooltip = LayerXMLString.Substring(tooltip_idx1 + 9, tooltip_idx2 - (tooltip_idx1 + 9)) ' <ToolTip> - 9 caracteres
                                    End If

                                    If tooltip_idx1 > 0 Then
                                        If tooltip.Contains("||||") Then
                                            Dim tooltipAux() As String = tooltip.Split({"||||"}, StringSplitOptions.None)
                                            For Each el As String In tooltipAux
                                                If propName.ToUpper = el.ToUpper Then
                                                    If a_objects(k_objects).Name <> "" Then a_objects(k_objects).Name += "                   "
                                                    a_objects(k_objects).Name += propName.ToUpperInvariant + ": " + o_value.ToString
                                                End If
                                            Next
                                            If propName.ToUpper = "GEOM_ID" Then
                                                a_objects(k_objects).Id = o_value
                                            End If
                                        Else
                                            If propName.ToUpper = tooltip.ToUpper Then
                                                a_objects(k_objects).Name = o_value
                                            End If
                                            If propName.ToUpper = "GEOM_ID" Then
                                                a_objects(k_objects).Id = o_value
                                            End If
                                            If a_objects(k_objects).Name = "" Then
                                                a_objects(k_objects).Name = tooltip
                                            End If
                                        End If
                                    End If
                                    Dim url_idx1 As Int32 = LayerXMLString.IndexOf("<Url>")
                                    Dim url_idx2 As Int32 = LayerXMLString.IndexOf("</Url>")
                                    Dim url As String = ""
                                    If url_idx1 = -1 Or url_idx2 = -1 Then
                                        url = "" ' <Url> - 5 caracteres
                                    Else
                                        url = LayerXMLString.Substring(url_idx1 + 5, url_idx2 - (url_idx1 + 5)) ' <Url> - 5 caracteres
                                    End If

                                    If url_idx1 > 0 Then
                                        If propName.ToUpper = url.ToUpper Then
                                            a_objects(k_objects).URL = o_value
                                        End If
                                    End If

                                    Dim campolabel_idx1 As Int32 = LayerXMLString.IndexOf("<Label>")
                                    Dim campolabel_idx2 As Int32 = LayerXMLString.IndexOf("</Label>")
                                    label = ""
                                    If campolabel_idx1 = -1 Or campolabel_idx2 = -1 Then

                                        label = "" ' <Label> - 7 caracteres
                                    Else
                                        label = LayerXMLString.Substring(campolabel_idx1 + 7, campolabel_idx2 - (campolabel_idx1 + 7)) ' <Label> - 7 caracteres
                                    End If

                                    If campolabel_idx1 > 0 Then

                                        campolabel_idx1 = label.IndexOf("<Text>")
                                        campolabel_idx2 = label.IndexOf("</Text>")
                                        If campolabel_idx1 = -1 Or campolabel_idx2 = -1 Then

                                            label = "" ' <Text> - 6 caracteres
                                        Else
                                            label = label.Substring(campolabel_idx1 + 6, campolabel_idx2 - (campolabel_idx1 + 6)) ' <Text> - 6 caracteres
                                        End If
                                        a_objects(k_objects).Name = label
                                    End If

                                    a_prop(j) = New pccMapObjectProperty(propName, New pccMapObjectType(pccMapServerType.MapGuideEnterprise, features.GetPropertyType(propName)), o_value)

                                Next
                            End If

                            a_objects(k_objects).Properties = a_prop
                            k_objects += 1
                            If contador <> 0 Then
                                If contador = k_objects Then
                                    sair = True
                                End If
                            End If
                        End While

                        If Not entrou Then
                            ' Vamos verificar se o layer tem erro !

                        End If
                        features.Close()

                        Return a_objects
                    End If
                End If

            Catch ex As Exception
                _erro = True
                _lastexception = ex
                Return Nothing

            End Try
        End SyncLock
    End Function

    Public Function GetAllObjects(ByVal p_layername As String, Optional ByVal contador As Integer = 0, Optional ByVal getAllProp As Boolean = True) As pccMapguideObject() Implements IG10IMap.GetAllObjects

        SyncLock _locker
            Dim wktReaderWriter As MgWktReaderWriter = New MgWktReaderWriter()

            Dim aux As Integer = 0
            Try

                Dim l As pccMapguideLayer = Me.GetLayer(p_layername)

                If l Is Nothing Then

                    Return Nothing

                Else

                    Dim ml As MgLayer = l.Object
                    Dim s As String = ml.GetLegendLabel

                    Dim FeatureGeometryFieldName As String = ml.GetFeatureGeometryName
                    If LCase(FeatureGeometryFieldName) = "image" Or LCase(FeatureGeometryFieldName) = "raster" Then Return Nothing

                    Dim FeatureSrvc As MgFeatureService = Me.GetMapServer.SiteConnection.CreateService(MgServiceType.FeatureService)
                    Dim featureSource As MgResourceIdentifier = New MgResourceIdentifier(ml.GetFeatureSourceId())

                    Dim qryoptions As New MgFeatureQueryOptions()
                    qryoptions.SetFilter(ml.Filter)

                    'Dim featureSource As MgResourceIdentifier = New MgResourceIdentifier(ml.GetFeatureSourceId())
                    Dim FeatureClassName As String = ml.GetFeatureClassName()

                    'Dim FeatureSrvc As MgFeatureService = Me.GetMapServer.SiteConnection.CreateService(MgServiceType.FeatureService)

                    Dim features As MgFeatureReader = FeatureSrvc.SelectFeatures(featureSource, FeatureClassName, qryoptions)

                    Dim k_objects As Integer = 0
                    Dim sair As Boolean = False
                    Dim a_objects As pccMapguideObject() = Nothing
                    Dim featureWKT As String = ""
                    While features.ReadNext And Not sair

                        ReDim Preserve a_objects(k_objects)

                        a_objects(k_objects) = New pccMapguideObject()

                        Dim k_prop As Integer = features.GetPropertyCount()
                        Dim a_prop As pccMapObjectProperty()
                        ReDim a_prop(k_prop - 1)
                        Dim propName As String = "geom"

                        Dim aaaa As Boolean = False

                        If getAllProp Then
                            For j As Integer = 0 To k_prop - 1
                                propName = features.GetPropertyName(j)
                                If features.GetPropertyType(propName) = 13 Then
                                    aaaa = True
                                    Exit For
                                End If
                            Next
                        End If
                        If Not aaaa Then
                            propName = "geom"
                        End If
                        Try

                            ' teste
                            Dim byteReader As MgByteReader = features.GetGeometry(propName)
                            Dim geometryReaderAgf As MgAgfReaderWriter = New MgAgfReaderWriter()
                            Dim geometryReaderWkt As MgWktReaderWriter = New MgWktReaderWriter()
                            Dim geometry As MgGeometry = geometryReaderAgf.Read(byteReader)

                            Dim b As New pccGeoUtils

                            featureWKT = geometryReaderWkt.Write(geometry)

                            a_objects(k_objects).Geom = b.GeometryFromWKT(featureWKT)
                        Catch ex As Exception
                            featureWKT = ex.Message
                            featureWKT = ""

                        End Try

                        If getAllProp Then
                            For j As Integer = 0 To k_prop - 1

                                Dim o_value As Object
                                propName = features.GetPropertyName(j)

                                If features.IsNull(propName) Then
                                    o_value = "null"
                                Else
                                    Select Case features.GetPropertyType(propName) '9: strings ; 13: geometries ; 7: numbers
                                        Case 9
                                            o_value = features.GetString(propName)
                                        Case 7
                                            o_value = features.GetInt32(propName)
                                        Case 13
                                            Dim byteReader As MgByteReader = features.GetGeometry(propName)
                                            Dim geometryReaderAgf As MgAgfReaderWriter = New MgAgfReaderWriter()
                                            Dim geometryReaderWkt As MgWktReaderWriter = New MgWktReaderWriter()
                                            Dim geometry As MgGeometry = geometryReaderAgf.Read(byteReader)

                                            Dim b As New pccGeoUtils

                                            featureWKT = geometryReaderWkt.Write(geometry)
                                            Dim a As New pccGeoUtils

                                            'o_value = a.GeometryFromWKT(propName)
                                            o_value = a.GeometryFromWKT(featureWKT)
                                        'o_value = byteReader.ToString()
                                        Case 5
                                            o_value = features.GetDouble(propName)
                                        Case Else
                                            o_value = "?"
                                    End Select
                                End If

                                ' read ToolTip from XML Layer File
                                Dim laydefRI As MgResourceIdentifier
                                laydefRI = CType(l.Object, MgLayer).GetLayerDefinition

                                Dim br As MgByteReader
                                br = ResourceService.GetResourceContent(laydefRI)

                                Dim LayerXMLString As String = br.ToString

                                Dim tooltip_idx1 As Int32 = LayerXMLString.IndexOf("<ToolTip>")
                                Dim tooltip_idx2 As Int32 = LayerXMLString.IndexOf("</ToolTip>")
                                Dim tooltip As String = ""
                                If tooltip_idx1 = -1 Or tooltip_idx1 = -1 Then

                                    tooltip = "" ' <ToolTip> - 9 caracteres
                                Else
                                    tooltip = LayerXMLString.Substring(tooltip_idx1 + 9, tooltip_idx2 - (tooltip_idx1 + 9)) ' <ToolTip> - 9 caracteres
                                End If

                                If tooltip_idx1 > 0 Then
                                    If tooltip.Contains("||||") Then
                                        Dim tooltipAux() As String = tooltip.Split({"||||"}, StringSplitOptions.None)
                                        For Each el As String In tooltipAux
                                            If propName.ToUpper = el.ToUpper Then
                                                If a_objects(k_objects).Name <> "" Then a_objects(k_objects).Name += "                   "
                                                a_objects(k_objects).Name += propName.ToUpperInvariant + ": " + o_value.ToString
                                            End If
                                        Next
                                        If propName.ToUpper = "GEOM_ID" Then
                                            a_objects(k_objects).Id = o_value
                                        End If
                                    Else
                                        If propName.ToUpper = tooltip.ToUpper Then
                                            a_objects(k_objects).Name = o_value
                                        End If
                                        If propName.ToUpper = "GEOM_ID" Then
                                            a_objects(k_objects).Id = o_value
                                        End If
                                        If a_objects(k_objects).Name = "" Then
                                            a_objects(k_objects).Name = tooltip
                                        End If
                                    End If
                                End If
                                Dim url_idx1 As Int32 = LayerXMLString.IndexOf("<Url>")
                                Dim url_idx2 As Int32 = LayerXMLString.IndexOf("</Url>")
                                Dim url As String = ""
                                If url_idx1 = -1 Or tooltip_idx1 = -1 Then
                                    url = "" ' <Url> - 5 caracteres
                                Else
                                    url = LayerXMLString.Substring(url_idx1 + 5, url_idx2 - (url_idx1 + 5)) ' <Url> - 5 caracteres
                                End If

                                If url_idx1 > 0 Then
                                    If propName.ToUpper = url.ToUpper Then
                                        a_objects(k_objects).URL = o_value
                                    End If
                                End If
                                a_prop(j) = New pccMapObjectProperty(propName, New pccMapObjectType(pccMapServerType.MapGuideEnterprise, features.GetPropertyType(propName)), o_value)

                            Next
                        End If

                        a_objects(k_objects).Properties = a_prop
                        k_objects += 1
                        If contador <> 0 Then
                            If contador = k_objects Then
                                sair = True
                            End If
                        End If
                    End While

                    features.Close()

                    Return a_objects

                End If
            Catch ex As Exception
                Return Nothing
            End Try
        End SyncLock
    End Function

    Public Function GetObjectsIntersection(ByVal p_layername As String, ByVal p_selectgeometry As pccBase.pccGeoGeometry, Optional ByVal contador As Integer = 0) As pccMapguideObject() Implements IG10IMap.GetObjectsIntersection
        SyncLock _locker
            Dim getAllProp As Boolean = True
            Dim wktReaderWriter As MgWktReaderWriter = New MgWktReaderWriter()

            Dim sWKT As String = p_selectgeometry.WKT

            Dim filtergeom As MgGeometry
            Dim aux As Integer = 0
            Try

                filtergeom = wktReaderWriter.Read(sWKT)

                Dim l As pccMapguideLayer = Me.GetLayer(p_layername)

                If l Is Nothing Then

                    Return Nothing

                Else

                    Dim ml As MgLayer = l.Object

                    Dim s As String = ml.GetLegendLabel

                    Dim FeatureGeometryFieldName As String = ml.GetFeatureGeometryName
                    If LCase(FeatureGeometryFieldName) = "image" Or LCase(FeatureGeometryFieldName) = "raster" Then Return Nothing

                    Dim FeatureSrvc As MgFeatureService = Me.GetMapServer.SiteConnection.CreateService(MgServiceType.FeatureService)
                    Dim featureSource As MgResourceIdentifier = New MgResourceIdentifier(ml.GetFeatureSourceId())

                    ' Get Layer CoordinateSystem
                    Dim spatialContextReader As MgSpatialContextReader
                    spatialContextReader = FeatureSrvc.GetSpatialContexts(featureSource, False)
                    spatialContextReader.ReadNext()
                    Dim CoordSyscode As String
                    Dim CoordSyswkt As String
                    CoordSyscode = spatialContextReader.GetCoordinateSystem()
                    CoordSyswkt = spatialContextReader.GetCoordinateSystemWkt()

                    Dim csmapa As pccGeoMetadata.mtdReferenceSystemInfo = Me.GetCoordinateSystem()
                    If csmapa.WKT <> CoordSyswkt Then
                        Try

                            Dim csforigem As New OSGeo.MapGuide.MgCoordinateSystemFactory
                            Dim codeorigem As String = csforigem.ConvertWktToCoordinateSystemCode(CoordSyswkt)
                            Dim epsgcodeorigem As String = csforigem.ConvertWktToEpsgCode(CoordSyswkt)
                            Dim rsorigem As New pccGeoMetadata.mtdReferenceSystemInfo(CoordSyswkt, csforigem.CreateFromCode(codeorigem))
                            rsorigem.Code = epsgcodeorigem
                            rsorigem.CodeSpace = "EPSG"

                            Dim csfdestino As New OSGeo.MapGuide.MgCoordinateSystemFactory
                            Dim codedestino As String = csfdestino.ConvertWktToCoordinateSystemCode(csmapa.WKT)
                            Dim epsgcodedestino As String = csfdestino.ConvertWktToEpsgCode(csmapa.WKT)
                            Dim rsdestino As New pccGeoMetadata.mtdReferenceSystemInfo(csmapa.WKT, csfdestino.CreateFromCode(codedestino))
                            rsdestino.Code = epsgcodedestino
                            rsdestino.CodeSpace = "EPSG"

                            wktReaderWriter = New OSGeo.MapGuide.MgWktReaderWriter()
                            Dim geosrc As OSGeo.MapGuide.MgGeometry = wktReaderWriter.Read(p_selectgeometry.WKT)

                            Dim mf As New OSGeo.MapGuide.MgCoordinateSystemFactory
                            Dim t As OSGeo.MapGuide.MgCoordinateSystemTransform

                            t = mf.GetTransform(rsdestino.Object, rsorigem.Object)

                            Dim coordsrc As OSGeo.MapGuide.MgCoordinate = geosrc.Centroid.Coordinate

                            t.TransformCoordinate(coordsrc)
                            Dim ponto3 As pccBase.pccGeoPoint = New pccGeoPoint(coordsrc.X, coordsrc.Y)

                            wktReaderWriter = New OSGeo.MapGuide.MgWktReaderWriter()
                            filtergeom = wktReaderWriter.Read(ponto3.WKT)
                        Catch ex As Exception
                            Dim k_objects1 As Integer = 0
                        End Try
                    End If
                    Dim qryoptions As New MgFeatureQueryOptions()

                    If filtergeom.GetGeometryType = 1 Then
                        qryoptions.SetSpatialFilter(FeatureGeometryFieldName, filtergeom.Buffer(Me._actualview.Scale / 500, Nothing), MgFeatureSpatialOperations.Intersects)
                    Else
                        qryoptions.SetSpatialFilter(FeatureGeometryFieldName, filtergeom, MgFeatureSpatialOperations.Intersects)
                    End If
                    qryoptions.SetFilter(ml.Filter)


                    'Dim featureSource As MgResourceIdentifier = New MgResourceIdentifier(ml.GetFeatureSourceId())
                    Dim FeatureClassName As String = ml.GetFeatureClassName()

                    'Dim FeatureSrvc As MgFeatureService = Me.GetMapServer.SiteConnection.CreateService(MgServiceType.FeatureService)

                    Dim features As MgFeatureReader = FeatureSrvc.SelectFeatures(featureSource, FeatureClassName, qryoptions)

                    Dim k_objects As Integer = 0
                    Dim sair As Boolean = False
                    Dim a_objects As pccMapguideObject() = Nothing
                    Dim featureWKT As String = ""
                    While features.ReadNext And Not sair

                        ReDim Preserve a_objects(k_objects)

                        a_objects(k_objects) = New pccMapguideObject()

                        Dim k_prop As Integer = features.GetPropertyCount()
                        Dim a_prop As pccMapObjectProperty()
                        ReDim a_prop(k_prop - 1)
                        Dim propName As String = "geom"

                        Dim aaaa As Boolean = False

                        If getAllProp Then
                            For j As Integer = 0 To k_prop - 1
                                propName = features.GetPropertyName(j)
                                If features.GetPropertyType(propName) = 13 Then
                                    aaaa = True
                                    Exit For
                                End If
                            Next
                        End If
                        If Not aaaa Then
                            propName = "geom"
                        End If

                        Try

                            ' teste
                            Dim byteReader As MgByteReader = features.GetGeometry(propName)
                            Dim geometryReaderAgf As MgAgfReaderWriter = New MgAgfReaderWriter()
                            Dim geometryReaderWkt As MgWktReaderWriter = New MgWktReaderWriter()
                            Dim geometry As MgGeometry = geometryReaderAgf.Read(byteReader)

                            Dim b As New pccGeoUtils

                            featureWKT = geometryReaderWkt.Write(geometry)

                            Dim geometry1 As MgGeometry = geometry.Intersection(filtergeom)
                            Dim interseccaoWKT As String = geometryReaderWkt.Write(geometry1)

                            'a_objects(k_objects).Geom = b.GeometryFromWKT(featureWKT)

                            a_objects(k_objects).Geom = b.GeometryFromWKT(interseccaoWKT)
                        Catch ex As Exception
                            featureWKT = ex.Message
                            featureWKT = ""

                        End Try

                        If getAllProp Then
                            For j As Integer = 0 To k_prop - 1

                                Dim o_value As Object
                                propName = features.GetPropertyName(j)

                                If features.IsNull(propName) Then
                                    o_value = "null"
                                Else
                                    Select Case features.GetPropertyType(propName) '9: strings ; 13: geometries ; 7: numbers
                                        Case 9
                                            o_value = features.GetString(propName)
                                        Case 7
                                            o_value = features.GetInt32(propName)
                                        Case 13
                                            Dim byteReader As MgByteReader = features.GetGeometry(propName)
                                            Dim geometryReaderAgf As MgAgfReaderWriter = New MgAgfReaderWriter()
                                            Dim geometryReaderWkt As MgWktReaderWriter = New MgWktReaderWriter()
                                            Dim geometry As MgGeometry = geometryReaderAgf.Read(byteReader)

                                            Dim b As New pccGeoUtils

                                            featureWKT = geometryReaderWkt.Write(geometry)
                                            Dim a As New pccGeoUtils

                                            'o_value = a.GeometryFromWKT(propName)
                                            o_value = a.GeometryFromWKT(featureWKT)
                                        'o_value = byteReader.ToString()
                                        Case 5
                                            o_value = features.GetDouble(propName)
                                        Case Else
                                            o_value = "?"
                                    End Select
                                End If

                                ' read ToolTip from XML Layer File
                                Dim laydefRI As MgResourceIdentifier
                                laydefRI = CType(l.Object, MgLayer).GetLayerDefinition

                                Dim br As MgByteReader
                                br = ResourceService.GetResourceContent(laydefRI)

                                Dim LayerXMLString As String = br.ToString

                                Dim tooltip_idx1 As Int32 = LayerXMLString.IndexOf("<ToolTip>")
                                Dim tooltip_idx2 As Int32 = LayerXMLString.IndexOf("</ToolTip>")
                                Dim tooltip As String = ""
                                If tooltip_idx1 = -1 Or tooltip_idx1 = -1 Then

                                    tooltip = "" ' <ToolTip> - 9 caracteres
                                Else
                                    tooltip = LayerXMLString.Substring(tooltip_idx1 + 9, tooltip_idx2 - (tooltip_idx1 + 9)) ' <ToolTip> - 9 caracteres
                                End If

                                If tooltip_idx1 > 0 Then
                                    If tooltip.Contains("||||") Then
                                        Dim tooltipAux() As String = tooltip.Split({"||||"}, StringSplitOptions.None)
                                        For Each el As String In tooltipAux
                                            If propName.ToUpper = el.ToUpper Then
                                                If a_objects(k_objects).Name <> "" Then a_objects(k_objects).Name += "                   "
                                                a_objects(k_objects).Name += propName.ToUpperInvariant + ": " + o_value.ToString
                                            End If
                                        Next
                                        If propName.ToUpper = "GEOM_ID" Then
                                            a_objects(k_objects).Id = o_value
                                        End If
                                    Else
                                        If propName.ToUpper = tooltip.ToUpper Then
                                            a_objects(k_objects).Name = o_value
                                        End If
                                        If propName.ToUpper = "GEOM_ID" Then
                                            a_objects(k_objects).Id = o_value
                                        End If
                                        If a_objects(k_objects).Name = "" Then
                                            a_objects(k_objects).Name = tooltip
                                        End If
                                    End If
                                End If
                                Dim url_idx1 As Int32 = LayerXMLString.IndexOf("<Url>")
                                Dim url_idx2 As Int32 = LayerXMLString.IndexOf("</Url>")
                                Dim url As String = ""
                                If url_idx1 = -1 Or tooltip_idx1 = -1 Then
                                    url = "" ' <Url> - 5 caracteres
                                Else
                                    url = LayerXMLString.Substring(url_idx1 + 5, url_idx2 - (url_idx1 + 5)) ' <Url> - 5 caracteres
                                End If

                                If url_idx1 > 0 Then
                                    If propName.ToUpper = url.ToUpper Then
                                        a_objects(k_objects).URL = o_value
                                    End If
                                End If
                                a_prop(j) = New pccMapObjectProperty(propName, New pccMapObjectType(pccMapServerType.MapGuideEnterprise, features.GetPropertyType(propName)), o_value)

                            Next
                        End If

                        a_objects(k_objects).Properties = a_prop
                        k_objects += 1
                        If contador <> 0 Then
                            If contador = k_objects Then
                                sair = True
                            End If
                        End If
                    End While

                    features.Close()

                    Return a_objects

                End If
            Catch ex As Exception
                Return Nothing
            End Try
        End SyncLock
    End Function

    Public Function SelectionClear() As Boolean Implements IG10IMap.SelectionClear
        SyncLock _locker
            Try

                _actual_selection = New MgSelection(_map)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End SyncLock
    End Function

    Public Function SelectionByParametersQuery(ByVal p_layername As String, ByVal parametersexpression As String, Optional ByVal resetSelection As Boolean = True) As Boolean Implements IG10IMap.SelectionByParametersQuery
        SyncLock _locker
            Dim wktReaderWriter As MgWktReaderWriter = New MgWktReaderWriter()

            Dim l As pccMapguideLayer = Me.GetLayer(p_layername)

            If l Is Nothing Or l.GetLayerType() = pccMapLayerType.Raster Then

                Return False

            Else

                Dim ml As MgLayer = l.Object

                Dim s As String = ml.GetLegendLabel

                Dim FeatureGeometryFieldName As String = ml.GetFeatureGeometryName

                Dim qryoptions As New MgFeatureQueryOptions()
                qryoptions.SetFilter(parametersexpression)

                Dim featureSource As MgResourceIdentifier = New MgResourceIdentifier(ml.GetFeatureSourceId())
                Dim FeatureClassName As String = ml.GetFeatureClassName()
                Dim FeatureSrvc As MgFeatureService = Me.GetMapServer.SiteConnection.CreateService(MgServiceType.FeatureService)
                Dim features As MgFeatureReader = FeatureSrvc.SelectFeatures(featureSource, FeatureClassName, qryoptions)
                If resetSelection Then
                    _actual_selection = New MgSelection(_map)
                End If
                _actual_selection.AddFeatures(ml, features, 0)
                features.Close()

                Return True

            End If
        End SyncLock
    End Function

    Public Function SelectionBySpatialQuery(ByVal p_layername As String, ByVal p_selectgeometry As pccBase.pccGeoGeometry, Optional ByVal resetSelection As Boolean = True) As Boolean Implements IG10IMap.SelectionBySpatialQuery
        SyncLock _locker
            Dim wktReaderWriter As MgWktReaderWriter = New MgWktReaderWriter()

            Dim sWKT As String = p_selectgeometry.WKT

            Dim filtergeom As MgGeometry
            If p_selectgeometry.WKT = "" Then
                filtergeom = wktReaderWriter.Read(sWKT)
            Else
                filtergeom = wktReaderWriter.Read(sWKT)
            End If

            Dim l As pccMapguideLayer = Me.GetLayer(p_layername)

            If l Is Nothing Or l.GetLayerType() = pccMapLayerType.Raster Then

                Return False

            Else

                Dim ml As MgLayer = l.Object

                Dim s As String = ml.GetLegendLabel

                Dim FeatureGeometryFieldName As String = ml.GetFeatureGeometryName

                Dim qryoptions As New MgFeatureQueryOptions()
                If filtergeom.GetGeometryType = 1 Then
                    qryoptions.SetSpatialFilter(FeatureGeometryFieldName, filtergeom.Buffer(Me._actualview.Scale / 500, Nothing), MgFeatureSpatialOperations.Intersects)
                Else
                    qryoptions.SetSpatialFilter(FeatureGeometryFieldName, filtergeom, MgFeatureSpatialOperations.Intersects)
                End If
                qryoptions.SetFilter(ml.Filter)
                Dim featureSource As MgResourceIdentifier = New MgResourceIdentifier(ml.GetFeatureSourceId())
                Dim FeatureClassName As String = ml.GetFeatureClassName()
                Dim FeatureSrvc As MgFeatureService = Me.GetMapServer.SiteConnection.CreateService(MgServiceType.FeatureService)
                Dim features As MgFeatureReader = FeatureSrvc.SelectFeatures(featureSource, FeatureClassName, qryoptions)
                If resetSelection Then
                    _actual_selection = New MgSelection(_map)
                End If
                _actual_selection.AddFeatures(ml, features, 0)
                features.Close()

                Return True

            End If
        End SyncLock
    End Function

    Public Function GetObjectsEx(ByVal p_selectgeometry As pccBase.pccGeoGeometry, Optional ByVal getAllProp As Boolean = True) As pccMapguideObject() Implements IG10IMap.GetObjectsEx
        SyncLock _locker

            Try
                Dim layers As pccMapLayerSet
                layers = Me.GetLayers()

                Dim col As New Collection

                For Each l As IG10IMapLayer In layers.Layers
                    If l.GetVisibility Then
                        Dim objs As pccMapguideObject()
                        objs = GetObjects(l.Name, p_selectgeometry, 0, getAllProp)
                        If objs IsNot Nothing Then
                            For Each obj As pccMapguideObject In objs
                                obj.Layer = l
                                col.Add(obj)
                            Next
                        End If
                    End If
                Next

                Dim res As pccMapguideObject()
                ReDim res(col.Count - 1)

                Dim i As Long = 0
                For Each obj As Object In col
                    res(i) = CType(obj, pccMapguideObject)
                    i += 1
                Next

                Return res
            Catch ex As Exception
                Dim xxx As String = ""
                Return Nothing
            End Try
        End SyncLock
    End Function

    Public Function GetObjectOnTop(ByVal p_selectgeometry As pccBase.pccGeoGeometry) As pccMapguideObject Implements IG10IMap.GetObjectOnTop
        SyncLock _locker
            Try

                Dim layers As pccMapLayerSet
                layers = Me.GetLayersWithVisibleObjects()

                Dim col As New Collection
                Dim ok As Boolean = False

                For Each l As IG10IMapLayer In layers.Layers
                    If l.GetVisibility Then
                        Dim objs As pccMapguideObject()
                        objs = GetObjects(l.Name, p_selectgeometry, 1)
                        If objs IsNot Nothing Then
                            For Each obj As pccMapguideObject In objs
                                obj.Layer = l
                                If obj.Name IsNot Nothing Then
                                    For Each pr As pccMapObjectProperty In obj.Properties

                                        If pr.Type.ToString() <> pccMapObjectTypes.Geometry.ToString And pr.Type.ToString() <> pccMapObjectTypes.Raster.ToString And pr.Type.ToString() <> pccMapObjectTypes.Blob.ToString And pr.Type.ToString() <> pccMapObjectTypes.Clob.ToString And pr.Type.ToString() <> pccMapObjectTypes.Feature.ToString And pr.Type.ToString() <> pccMapObjectTypes.Null.ToString Then
                                            Try
                                                obj.Name = obj.Name.Replace("%" & LCase(pr.Name) & "%", pr.Value.ToString)


                                            Catch ex As Exception

                                            End Try
                                        End If
                                    Next
                                    obj.Name = obj.Name.Replace("\n", "<br />")
                                    col.Add(obj)
                                    ok = True
                                End If
                                If ok Then Exit For
                            Next
                        End If
                    End If
                    If ok Then Exit For
                Next

                If col.Count > 0 Then
                    Return CType(col.Item(1), pccMapguideObject)
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Return Nothing
            End Try
        End SyncLock
    End Function

    Public Function GetSelectedObject(ByVal p_selectgeometry As pccBase.pccGeoGeometry, ByRef conn As pccDB.Connection, ByRef trans As IDbTransaction) As Boolean Implements IG10IMap.GetSelectedObject
        SyncLock _locker
            Try

                Dim layers As pccMapLayerSet
                layers = Me.GetLayers()
                Dim res As Boolean

                Dim col As New Collection
                Dim ok As Boolean = False

                For Each l As IG10IMapLayer In layers.Layers
                    If l.GetVisibility And l.GetSelectable Then
                        Dim objs As pccMapguideObject()
                        objs = GetObjects(l.Name, p_selectgeometry)
                        If objs IsNot Nothing Then
                            res = p_selectgeometry.setGeomSelected(conn, trans, objs(0).Id, "epl")
                        End If
                    End If
                    If ok Then Exit For
                Next

                Return res

            Catch ex As Exception
                Return Nothing
            End Try
        End SyncLock
    End Function

    Public Function GetMapServer() As pccMapguideServer Implements IG10IMap.GetMapServer
        SyncLock _locker
            Return _mapserver
        End SyncLock
    End Function

    '_siteConnection.GetSite().GetCurrentSiteAddress()
    <DataMember()>
    Public Property ZoomScaleDPI(ByVal p_center As pccBase.pccGeoPoint, ByVal p_newscale As Long, ByVal p_imagewidth As Long, ByVal p_imageheight As Long, ByVal DPI As Long) As Byte() Implements IG10IMap.ZoomScaleDPI

        Get
            SyncLock _locker
                Dim url As String = ""
                url += _mapserver.AgentURL & "?OPERATION=GETMAPIMAGE"
                url += "&VERSION=1.0.0"
                url += "&CLIENTAGENT=G10"
                url += "&LOCALE=en"
                url += "&SESSION=" & _map.SessionId
                url += "&MAPNAME=" & _map.Name
                If p_imagewidth > 4095 Or p_imageheight > 4095 Then
                    url += "&FORMAT=JPG"
                Else
                    url += "&FORMAT=PNG"
                End If
                url += "&SETDISPLAYWIDTH=" & p_imagewidth
                url += "&SETDISPLAYHEIGHT=" & p_imageheight
                url += "&SETDISPLAYDPI=" & DPI
                url += "&SETVIEWSCALE=" & p_newscale
                url += "&SETVIEWCENTERX=" & p_center.X
                url += "&SETVIEWCENTERY=" & p_center.Y
                'url += "&SEQ=0.6"
                url += "&KEEPSELECTION=1"

                Dim retVal As System.Drawing.Image = Nothing
                Try
                    Dim response As WebResponse
                    Dim request As HttpWebRequest = DirectCast(HttpWebRequest.Create(url), HttpWebRequest)

                    response = DirectCast(request.GetResponse(), HttpWebResponse)
                    retVal = System.Drawing.Image.FromStream(response.GetResponseStream())

                    Dim ms As MemoryStream = New MemoryStream()
                    If p_imagewidth > 4095 Or p_imageheight > 4095 Then
                        retVal.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
                    Else
                        retVal.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
                    End If
                    'retVal.Save(ms, System.Drawing.Imaging.ImageFormat.Png)

                    Dim geoFact As MgGeometryFactory = New MgGeometryFactory()
                    Dim center As MgCoordinate = geoFact.CreateCoordinateXY(p_center.X, p_center.Y)


                    ' TODO: é preciso actualizar a largura e altura da zona do mapa retornada; não pode ser _map.GetMapExtent.GetWidth, _map.GetMapExtent.GetHeight
                    _actualview = New pccMapView(New pccGeoPoint(p_center.X, p_center.Y), p_newscale, _map.GetDisplayDpi, _map.GetMapExtent.GetWidth, _map.GetMapExtent.GetHeight, p_imagewidth, p_imageheight)

                    Dim canto As pccGeoPoint = _actualview.GetCoordsImage2Map(New pccGeoPoint(0, 0), Me, p_imagewidth, p_imageheight)

                    Dim canto1 As pccGeoPoint = _actualview.GetCoordsImage2Map(New pccGeoPoint(p_imagewidth, p_imageheight), Me, p_imagewidth, p_imageheight)

                    Dim mapwidth As Double = Math.Abs(canto1.X - canto.X)
                    Dim mapheight As Double = Math.Abs(canto1.Y - canto.Y)
                    _actualview = New pccMapView(New pccGeoPoint(p_center.X, p_center.Y), p_newscale, _map.GetDisplayDpi, mapwidth, mapheight, p_imagewidth, p_imageheight)
                    Dim aux As Byte() = ms.ToArray
                    ms.SetLength(0)
                    ms.Position = 0
                    Return aux
                Catch generatedExceptionName As Exception
                    retVal = Nothing
                    Return Nothing
                End Try
            End SyncLock
        End Get

        Set(ByVal value As Byte())
            Throw New Exception("pcc by design: pccMapguideMap.ZoomScale() is a readonly property.")
        End Set

    End Property

    Public Property ZoomScaleDPI_url(ByVal p_center As pccBase.pccGeoPoint, ByVal p_newscale As Long, ByVal p_imagewidth As Long, ByVal p_imageheight As Long, ByVal DPI As Long) As String Implements IG10IMap.ZoomScaleDPI_url

        Get
            SyncLock _locker
                Dim url As String = ""
                url += _mapserver.AgentURL & "?OPERATION=GETMAPIMAGE"
                url += "&VERSION=1.0.0"
                url += "&CLIENTAGENT=G10"
                url += "&LOCALE=en"
                url += "&SESSION=" & _map.SessionId
                url += "&MAPNAME=" & _map.Name
                If p_imagewidth > 4095 Or p_imagewidth > 4095 Then
                    url += "&FORMAT=JPG"
                Else
                    url += "&FORMAT=PNG"
                End If
                url += "&SETDISPLAYWIDTH=" & p_imagewidth
                url += "&SETDISPLAYHEIGHT=" & p_imageheight
                url += "&SETDISPLAYDPI=" & DPI
                url += "&SETVIEWSCALE=" & p_newscale
                url += "&SETVIEWCENTERX=" & p_center.X
                url += "&SETVIEWCENTERY=" & p_center.Y
                'url += "&SEQ=0.6"
                url += "&KEEPSELECTION=1"

                Return url

            End SyncLock
        End Get

        Set(ByVal value As String)
            Throw New Exception("pcc by design: pccMapguideMap.ZoomScaleDPI_url() is a readonly property.")
        End Set

    End Property

    <DataMember()>
    Public Property ZoomScale(ByVal p_center As pccBase.pccGeoPoint, ByVal p_newscale As Long, ByVal p_imagewidth As Long, ByVal p_imageheight As Long) As Byte() Implements IG10IMap.ZoomScale

        Get
            SyncLock _locker
                If p_imagewidth = 0 Or p_imageheight = 0 Then
                    Return Nothing
                End If
                Dim geoFact As MgGeometryFactory = New MgGeometryFactory()
                Dim center As MgCoordinate = geoFact.CreateCoordinateXY(p_center.X, p_center.Y)
                Dim color As MgColor = New MgColor(255, 255, 255)

                Dim byteReader As MgByteReader
                Dim renderingService As MgRenderingService = _mapserver.SiteConnection.CreateService(MgServiceType.RenderingService)



                ' TODO: tipo de imagem PNG ou JPG deve ser passado como parâmetro, vindo talvez de um ficheiro de configuração web.config ou alike. ya??
                byteReader = renderingService.RenderMap(_map, _actual_selection, center, p_newscale, p_imagewidth, p_imageheight, color, "PNG", True)


                ' TODO: é preciso actualizar a largura e altura da zona do mapa retornada; não pode ser _map.GetMapExtent.GetWidth, _map.GetMapExtent.GetHeight
                _actualview = New pccMapView(New pccGeoPoint(p_center.X, p_center.Y), p_newscale, _map.GetDisplayDpi, _map.GetMapExtent.GetWidth, _map.GetMapExtent.GetHeight, p_imagewidth, p_imageheight)

                Dim canto As pccGeoPoint = _actualview.GetCoordsImage2Map(New pccGeoPoint(0, 0), Me, p_imagewidth, p_imageheight)
                Dim canto1 As pccGeoPoint = _actualview.GetCoordsImage2Map(New pccGeoPoint(p_imagewidth, p_imageheight), Me, p_imagewidth, p_imageheight)

                Dim mapwidth As Double
                mapwidth = Math.Abs(canto1.X - canto.X)
                Dim mapheight As Double
                mapheight = Math.Abs(canto1.Y - canto.Y)
                _actualview = New pccMapView(New pccGeoPoint(p_center.X, p_center.Y), p_newscale, _map.GetDisplayDpi, mapwidth, mapheight, p_imagewidth, p_imageheight)

                'Dim mapwidth As Double
                'mapwidth = (p_imagewidth / _map.GetDisplayDpi) * 2.54 * p_newscale
                'Dim mapheight As Double
                'mapheight = (p_imageheight / _map.GetDisplayDpi) * 2.54 * p_newscale
                '_actualview = New pccMapView(New pccGeoPoint(p_center.X, p_center.Y), p_newscale, _map.GetDisplayDpi, mapwidth, mapheight, p_imagewidth, p_imageheight)

                Dim b(byteReader.GetLength) As Byte
                Dim len As Integer = 1
                Dim tmp(2048) As Byte

                Dim st As Integer = 0

                While len > 0
                    len = byteReader.Read(tmp, 2048)
                    If len > 0 Then
                        Buffer.BlockCopy(tmp, 0, b, st, len)
                        st += len
                    End If
                End While
                _map.Save()

                Try
                    Dim resId As MgResourceIdentifier = _map.GetResourceId

                    Dim szXML As String = Me.ResourceService.GetResourceContent(resId).ToString()
                    Dim nByteCount As Integer
                    Dim arrByte(szXML.Length) As Byte
                    nByteCount = System.Text.Encoding.UTF8.GetBytes(szXML, 0, szXML.Length, arrByte, 0)
                    Dim memStream As New IO.MemoryStream(arrByte)
                    Dim retDoc As New XmlDocument
                    retDoc.Load(memStream)
                    retDoc.ToString()


                    Dim mapadefStringXML As String = retDoc.ToString


                Catch ex As Exception

                End Try
                Return b
            End SyncLock
        End Get

        Set(ByVal value As Byte())
            Throw New Exception("pcc by design: pccMapguideMap.ZoomScale() is a readonly property.")
        End Set

    End Property

    <DataMember()>
    Public Property GetLegend(ByVal p_imagewidth As Long, ByVal p_imageheight As Long) As Byte() Implements IG10IMap.GetLegend

        Get
            SyncLock _locker
                Dim color As MgColor = New MgColor(255, 255, 255)

                Dim byteReader As MgByteReader
                Dim renderingService As MgRenderingService = _mapserver.SiteConnection.CreateService(MgServiceType.RenderingService)

                ' TODO: tipo de imagem PNG ou JPG deve ser passado como parâmetro, vindo talvez de um ficheiro de configuração web.config ou alike. ya??
                byteReader = renderingService.RenderMapLegend(_map, p_imagewidth, p_imageheight, color, "PNG")

                Try
                    Dim b(byteReader.GetLength) As Byte
                    Dim len As Integer = 1
                    Dim tmp(2048) As Byte

                    Dim st As Integer = 0

                    While len > 0
                        len = byteReader.Read(tmp, 2048)
                        If len > 0 Then
                            Buffer.BlockCopy(tmp, 0, b, st, len)
                            st += len
                        End If
                    End While

                    Return b

                Catch ex As Exception

                    Return Nothing

                End Try
            End SyncLock
        End Get
        Set(ByVal value As Byte())
            Throw New Exception("pcc by design: pccMapguideMap.GetLegend() is a readonly property.")
        End Set

    End Property

    <DataMember()>
    Public Property ZoomWindow(ByVal p_center As pccBase.pccGeoPoint, ByVal p_mapwidth As Double, ByVal p_mapheight As Double, ByVal p_imagewidth As Long, ByVal p_imageheight As Long) As Byte() Implements pccMap.IG10IMap.ZoomWindow

        Get
            SyncLock _locker
                Dim sc_w As Double = p_mapwidth / p_imagewidth
                Dim sc_h As Double = p_mapheight / p_imageheight

                If sc_w < sc_h Then
                    p_mapwidth *= (sc_h / sc_w)
                Else
                    p_mapheight *= (sc_h / sc_w)
                End If


                Dim new_scale As Double = (100 * p_mapwidth) / ((p_imagewidth / 96) * 2.54)

                Return ZoomScale(p_center, new_scale, p_imagewidth, p_imageheight)

            End SyncLock
        End Get
        Set(ByVal value As Byte())
            Throw New Exception("pcc by design: pccMapguideMap.ZoomWindow() is a readonly property.")
        End Set
    End Property

    <DataMember()>
    Public Property ZoomWindowBase64(ByVal p_center As pccBase.pccGeoPoint, ByVal p_mapwidth As Double, ByVal p_mapheight As Double, ByVal p_imagewidth As Long, ByVal p_imageheight As Long) As String Implements pccMap.IG10IMap.ZoomWindowBase64

        Get
            SyncLock _locker
                Dim sc_w As Double = p_mapwidth / p_imagewidth
                Dim sc_h As Double = p_mapheight / p_imageheight

                If sc_w < sc_h Then
                    p_mapwidth *= (sc_h / sc_w)
                Else
                    p_mapheight *= (sc_h / sc_w)
                End If


                Dim new_scale As Double = (100 * p_mapwidth) / ((p_imagewidth / 96) * 2.54)

                Return ZoomScaleBase64(p_center, new_scale, p_imagewidth, p_imageheight)
            End SyncLock

        End Get
        Set(ByVal value As String)
            Throw New Exception("pcc by design: pccMapguideMap.ZoomWindowBase64() is a readonly property.")
        End Set
    End Property

    <DataMember()>
    Public Property GetLayer(ByVal p_layername As String) As pccMapguideLayer Implements IG10IMap.GetLayer
        Get

            SyncLock _locker
                For i As Integer = 0 To _layers.Size - 1
                    'If _layers.Item(i).Name = p_layername And Not IsBaseLayer(p_layername) Then
                    If _layers.Item(i).Name = p_layername Then
                        Return _layers.Item(i)
                    End If
                Next

                Return Nothing
            End SyncLock
        End Get

        Set(ByVal value As pccMapguideLayer)
            Throw New Exception("pcc by design: pccMapguideMap.GetLayer() is a readonly property.")
        End Set

    End Property


    <DataMember()>
    Public Property GetGrupo(ByVal p_groupname As String) As pccMapguideGroup Implements IG10IMap.GetGrupo
        Get

            SyncLock _locker
                For i As Integer = 0 To _grupos.Size - 1
                    'If _layers.Item(i).Name = p_layername And Not IsBaseLayer(p_layername) Then
                    If _grupos.Item(i).Name = p_groupname Then
                        Return _grupos.Item(i)
                    End If
                Next

                Return Nothing
            End SyncLock
        End Get

        Set(ByVal value As pccMapguideGroup)
            Throw New Exception("pcc by design: pccMapguideMap.GetGrupo() is a readonly property.")
        End Set

    End Property


    <DataMember()>
    Public Property GetLayerBase(ByVal p_layername As String) As pccMapguideBaseLayer Implements IG10IMap.GetLayerBase
        Get

            SyncLock _locker
                For i As Integer = 0 To _layersbase.Size - 1
                    'If _layers.Item(i).Name = p_layername And Not IsBaseLayer(p_layername) Then
                    If _layersbase.Item(i).Name = p_layername Then
                        Return _layersbase.Item(i)
                    End If
                Next

                Return Nothing
            End SyncLock
        End Get

        Set(ByVal value As pccMapguideBaseLayer)
            Throw New Exception("pcc by design: pccMapguideMap.GetLayerBase() is a readonly property.")
        End Set

    End Property


    <DataMember()>
    Public Property LayerBaseShowHide(ByVal p_center As pccBase.pccGeoPoint, ByVal p_newscale As Long, ByVal p_imagewidth As Long, ByVal p_imageheight As Long, ByVal DPI As Long, ByVal showgroups As String, ByVal hidegroups As String) As Byte() Implements IG10IMap.LayerBaseShowHide

        Get
            SyncLock _locker
                Dim url As String = ""
                url += _mapserver.AgentURL & "?OPERATION=GETDYNAMICMAPOVERLAYIMAGE"
                url += "&FORMAT=PNG&VERSION=2.1.0"
                url += "&CLIENTAGENT=G1&0BEHAVIOR=2"

                url += "&LOCALE=en"
                url += "&SESSION=" & _map.SessionId
                url += "&MAPNAME=" & _map.Name
                If p_imagewidth > 4095 Or p_imageheight > 4095 Then
                    url += "&FORMAT=JPG"
                Else
                    url += "&FORMAT=PNG"
                End If
                url += "&SETDISPLAYWIDTH=" & p_imagewidth
                url += "&SETDISPLAYHEIGHT=" & p_imageheight
                url += "&SETDISPLAYDPI=" & DPI
                url += "&SETVIEWSCALE=" & p_newscale
                url += "&SETVIEWCENTERX=" & p_center.X
                url += "&SETVIEWCENTERY=" & p_center.Y
                url += "&SHOWGROUPS=" & showgroups
                url += "&HIDEGROUPS=" & hidegroups
                url += "&SHOWLAYERS="
                url += "&HIDELAYERS="


                Dim retVal As System.Drawing.Image = Nothing
                Try
                    Dim response As WebResponse
                    Dim request As HttpWebRequest = DirectCast(HttpWebRequest.Create(url), HttpWebRequest)

                    response = DirectCast(request.GetResponse(), HttpWebResponse)
                    retVal = System.Drawing.Image.FromStream(response.GetResponseStream())

                    Dim ms As MemoryStream = New MemoryStream()
                    If p_imagewidth > 4095 Or p_imageheight > 4095 Then
                        retVal.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg)
                    Else
                        retVal.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
                    End If
                    'retVal.Save(ms, System.Drawing.Imaging.ImageFormat.Png)

                    'Dim geoFact As MgGeometryFactory = New MgGeometryFactory()
                    'Dim center As MgCoordinate = geoFact.CreateCoordinateXY(p_center.X, p_center.Y)


                    '' TODO: é preciso actualizar a largura e altura da zona do mapa retornada; não pode ser _map.GetMapExtent.GetWidth, _map.GetMapExtent.GetHeight
                    '_actualview = New pccMapView(New pccGeoPoint(p_center.X, p_center.Y), p_newscale, _map.GetDisplayDpi, _map.GetMapExtent.GetWidth, _map.GetMapExtent.GetHeight, p_imagewidth, p_imageheight)

                    'Dim canto As pccGeoPoint = _actualview.GetCoordsImage2Map(New pccGeoPoint(0, 0), Me, p_imagewidth, p_imageheight)

                    'Dim canto1 As pccGeoPoint = _actualview.GetCoordsImage2Map(New pccGeoPoint(p_imagewidth, p_imageheight), Me, p_imagewidth, p_imageheight)

                    'Dim mapwidth As Double = Math.Abs(canto1.X - canto.X)
                    'Dim mapheight As Double = Math.Abs(canto1.Y - canto.Y)
                    '_actualview = New pccMapView(New pccGeoPoint(p_center.X, p_center.Y), p_newscale, _map.GetDisplayDpi, mapwidth, mapheight, p_imagewidth, p_imageheight)
                    Dim aux As Byte() = ms.ToArray
                    ms.SetLength(0)
                    ms.Position = 0
                    Return aux
                Catch generatedExceptionName As Exception
                    retVal = Nothing
                    Return Nothing
                End Try
            End SyncLock
        End Get

        Set(ByVal value As Byte())
            Throw New Exception("pcc by design: pccMapguideMap.LayerBaseShowHide() is a readonly property.")
        End Set

    End Property


    Public Function IsBaseLayer(ByVal p_layername As String) As Boolean Implements IG10IMap.IsBaseLayer
        '   SyncLock _locker

        Dim mapadefStringXML As String = getMapXMLDefinition()
        Dim xmlDoc As New System.Xml.XmlDocument
        Dim xmlLista As System.Xml.XmlNodeList
        Dim xmlListaBaseLayerGr As System.Xml.XmlNodeList
        Dim xmlBaseLayerGrNome As System.Xml.XmlNode
        Dim strNome As String

        xmlDoc.LoadXml(mapadefStringXML)
        xmlLista = xmlDoc.SelectNodes("MapDefinition/BaseMapDefinition")

        For Each xmlnode As System.Xml.XmlNode In xmlLista
            xmlListaBaseLayerGr = xmlnode.SelectNodes("BaseMapLayerGroup/BaseMapLayer")
            For Each xmlgrnode As System.Xml.XmlNode In xmlListaBaseLayerGr
                xmlBaseLayerGrNome = xmlgrnode.SelectSingleNode("Name")
                strNome = xmlBaseLayerGrNome.InnerText

                If p_layername = strNome Then
                    Return True
                End If
                'If ll_base.Length > 0 Then ll_base.Append(SeparadorNivel_I(0).ToString)
                'll_base = ll_base.Append(strNome)

            Next
        Next

        '   End SyncLock
    End Function
    Public Function GetGruposBaseNome() As String Implements IG10IMap.GetGruposBaseNome
        SyncLock _locker
            Dim mapadefStringXML As String = getMapXMLDefinition()
            Dim xmlDoc As New System.Xml.XmlDocument
            Dim xmlLista As System.Xml.XmlNodeList
            Dim xmlListaBaseLayerGr As System.Xml.XmlNodeList
            Dim xmlBaseLayerGrNome As System.Xml.XmlNode
            Dim strNome As String
            Dim gg_base As String

            xmlDoc.LoadXml(mapadefStringXML)
            xmlLista = xmlDoc.SelectNodes("MapDefinition/BaseMapDefinition")
            gg_base = ","
            For Each xmlnode As System.Xml.XmlNode In xmlLista
                xmlListaBaseLayerGr = xmlnode.SelectNodes("BaseMapLayerGroup")
                For Each xmlgrnode As System.Xml.XmlNode In xmlListaBaseLayerGr
                    xmlBaseLayerGrNome = xmlgrnode.SelectSingleNode("Name")
                    strNome = xmlBaseLayerGrNome.InnerText
                    gg_base = gg_base + strNome + ","
                Next
            Next
            Return gg_base
        End SyncLock
    End Function
    Public Function GetLayersBaseNome() As String Implements IG10IMap.GetLayersBaseNome
        SyncLock _locker
            Dim mapadefStringXML As String = getMapXMLDefinition()
            Dim xmlDoc As New System.Xml.XmlDocument
            Dim xmlLista As System.Xml.XmlNodeList
            Dim xmlListaBaseLayer As System.Xml.XmlNodeList
            Dim xmlBaseLayerNome As System.Xml.XmlNode
            Dim strNome As String
            Dim ll_base As String

            xmlDoc.LoadXml(mapadefStringXML)
            xmlLista = xmlDoc.SelectNodes("MapDefinition/BaseMapDefinition")
            ll_base = ","
            For Each xmlnode As System.Xml.XmlNode In xmlLista
                xmlListaBaseLayer = xmlnode.SelectNodes("BaseMapLayerGroup/BaseMapLayer")
                For Each xmlgrnode As System.Xml.XmlNode In xmlListaBaseLayer
                    xmlBaseLayerNome = xmlgrnode.SelectSingleNode("Name")
                    strNome = xmlBaseLayerNome.InnerText
                    ll_base = ll_base + strNome + ","
                Next
            Next
            Return ll_base
        End SyncLock
    End Function
    Public Function GetLayerDefinition(ByVal p_layername As String) As String Implements IG10IMap.GetLayerDefinition
        SyncLock _locker
            Dim ret As String = ""

            Dim l As pccMapguideLayer
            l = GetLayer(p_layername)

            If l IsNot Nothing Then

                Dim laydefRI As MgResourceIdentifier
                laydefRI = CType(l.Object, MgLayer).GetLayerDefinition

                Dim br As MgByteReader
                br = ResourceService.GetResourceContent(laydefRI)


                Dim laydefStringXML As String = br.ToString
                Try
                    Dim doc As New XmlDocument
                    doc.LoadXml(laydefStringXML)
                    doc.Save(IO.Path.GetTempPath & "\layer.xml")
                Catch ex As Exception

                End Try
                ret = laydefStringXML
            End If

            Return ret
        End SyncLock
    End Function


    <DataMember()>
    Public Property SuportaSQL(ByVal p_layername As String) As Boolean Implements IG10IMap.SuportaSQL
        Get
            SyncLock _locker
                Dim layerid As String = ""
                For i As Integer = 0 To _layers.Size - 1
                    If _layers.Item(i).Name = p_layername Then
                        layerid = CType(_layers.Item(i).Object, MgLayer).GetFeatureSourceId()
                    End If
                Next

                If layerid <> "" Then
                    Dim featureSource As MgResourceIdentifier = New MgResourceIdentifier(layerid)

                    Dim resoucexmlbyte As MgByteReader = _resourceservice.GetResourceContent(featureSource)
                    Dim resourcexml As String = resoucexmlbyte.ToString

                    Dim idx1 As Int32 = resourcexml.IndexOf("<Provider>")
                    Dim idx2 As Int32 = resourcexml.IndexOf("</Provider>")
                    Dim providername As String = ""
                    If idx1 = -1 Then
                        providername = "" ' <Provider> - 10 caracteres
                    Else
                        providername = resourcexml.Substring(idx1 + 10, idx2 - (idx1 + 10)) ' <Provider> - 10 caracteres
                    End If
                    'Dim providername As String = resourcexml  '<Provider>OSGeo.PostgreSQL</Provider>
                    Dim resoucexmlbyte2 As MgByteReader = _featureservice.GetCapabilities(providername)
                    Dim resourcexml2 As String = resoucexmlbyte2.ToString

                    Dim idx11 As Int32 = resourcexml2.IndexOf("<SupportsSQL>")
                    Dim idx12 As Int32 = resourcexml2.IndexOf("</SupportsSQL>")
                    Dim SupportsSQLstr As String
                    If idx11 = -1 Then
                        SupportsSQLstr = "" ' <SupportsSQL> - 13 caracteres
                    Else
                        SupportsSQLstr = resourcexml2.Substring(idx11 + 13, idx12 - (idx11 + 13)) ' <SupportsSQL> - 13 caracteres
                    End If

                    Dim FazTeste As Boolean
                    FazTeste = IIf(SupportsSQLstr = "true", True, False)
                    Return FazTeste
                End If
            End SyncLock
        End Get
        Set(ByVal value As Boolean)
            'TODO: throw error
            Throw New Exception("pcc by design: pccMapguideLayer.SupportsSQL() is a readonly property.")
        End Set
    End Property
    <DataMember()>
    Public Property ObjectosCorrectos(ByVal p_layername As String) As Boolean Implements IG10IMap.ObjectosCorrectos
        Get
            SyncLock _locker
                Dim Saida As Boolean = False
                Try

                    Dim layerid As String = ""
                    Dim l As MgLayer
                    For i As Integer = 0 To _layers.Size - 1
                        If _layers.Item(i).Name = p_layername Then
                            layerid = CType(_layers.Item(i).Object, MgLayer).GetFeatureSourceId()
                            l = CType(_layers.Item(i).Object, MgLayer)
                            i = _layers.Size
                        End If
                    Next

                    If layerid <> "" Then
                        Dim featureSource As MgResourceIdentifier = New MgResourceIdentifier(layerid)

                        Dim resoucexmlbyte As MgByteReader = _resourceservice.GetResourceContent(featureSource)
                        Dim resourcexml As String = resoucexmlbyte.ToString

                        Dim idx1 As Int32 = resourcexml.IndexOf("<Provider>")
                        Dim idx2 As Int32 = resourcexml.IndexOf("</Provider>")
                        Dim providername As String = ""
                        If idx1 = -1 Then
                            providername = "" ' <Provider> - 10 caracteres
                        Else
                            providername = resourcexml.Substring(idx1 + 10, idx2 - (idx1 + 10)) ' <Provider> - 10 caracteres
                        End If
                        'Dim providername As String = resourcexml  '<Provider>OSGeo.PostgreSQL</Provider>
                        Dim resoucexmlbyte2 As MgByteReader = _featureservice.GetCapabilities(providername)
                        Dim resourcexml2 As String = resoucexmlbyte2.ToString

                        Dim idx11 As Int32 = resourcexml2.IndexOf("<SupportsSQL>")
                        Dim idx12 As Int32 = resourcexml2.IndexOf("</SupportsSQL>")
                        Dim SupportsSQLstr As String
                        If idx11 = -1 Then
                            SupportsSQLstr = "" ' <SupportsSQL> - 13 caracteres
                        Else
                            SupportsSQLstr = resourcexml2.Substring(idx11 + 13, idx12 - (idx11 + 13)) ' <SupportsSQL> - 13 caracteres
                        End If

                        Dim FazTeste As Boolean
                        FazTeste = IIf(SupportsSQLstr = "true", True, False)
                        If FazTeste Then
                            Saida = True
                            Dim comandosql As String = "select count(*) from """ & Replace(l.FeatureClassName, ":", """.""") & """ where st_isvalid(" & l.GetFeatureGeometryName & " )=false "
                            Dim sqlDataReader As MgSqlDataReader
                            Try
                                If _featureservice.TestConnection(featureSource) Then
                                    sqlDataReader = _featureservice.ExecuteSqlQuery(featureSource, comandosql)
                                    While sqlDataReader.ReadNext()
                                        Dim i As Long = sqlDataReader.GetInt64(0)
                                        If i <> 0 Then
                                            Saida = False
                                        End If
                                    End While
                                End If
                            Catch ex As Exception

                                Dim a As Exception = ex.InnerException
                                Dim message As String
                                If a IsNot Nothing Then
                                    message = ex.Message.ToString & " " & ex.InnerException.ToString
                                Else
                                    message = ex.Message.ToString
                                End If

                            End Try
                            If sqlDataReader IsNot Nothing Then
                                sqlDataReader.Close()
                            End If
                        Else
                            Saida = False
                        End If
                    End If
                Catch ex As Exception
                    Saida = False
                End Try
                Return Saida
            End SyncLock
        End Get
        Set(ByVal value As Boolean)
            'TODO: throw error
            Throw New Exception("pcc by design: pccMapguideLayer.LeituraOk() is a readonly property.")
        End Set
    End Property
    Public Function PlotDWF() As Byte() Implements IG10IMap.PlotDWF
        SyncLock _locker
            Dim mapService As MgMappingService = DirectCast(_mapserver.SiteConnection.CreateService(MgServiceType.MappingService), MgMappingService)
            Dim dwfVersion As New MgDwfVersion("6.01", "1.2")
            Dim plospc As New MgPlotSpecification(297, 420, MgPageUnitsType.Millimeters, 0, 0, 0, 0)

            Dim geoFact As MgGeometryFactory = New MgGeometryFactory()
            Dim center As MgCoordinate = geoFact.CreateCoordinateXY(_actualview.Center.X, _actualview.Center.Y)

            Dim byteReader As MgByteReader = mapService.GeneratePlot(_map, center, _actualview.Scale, plospc, Nothing, dwfVersion)

            'Dim byteReader As MgByteReader = mapService.GeneratePlot(_map, plospc, Nothing, dwfVersion)

            Dim b(byteReader.GetLength) As Byte
            Dim len As Integer = 1
            Dim tmp(2048) As Byte

            Dim st As Integer = 0

            While len > 0
                len = byteReader.Read(tmp, 2048)
                If len > 0 Then
                    Buffer.BlockCopy(tmp, 0, b, st, len)
                    st += len
                End If
            End While

            Return b

            'Dim byteSink As New MgByteSink(byteReader)
            'Dim filePath As String = "C:\Temp\Map.DWF"
            'byteSink.ToFile(filePath)
        End SyncLock
    End Function

    ' TODO: testar
    Public Function GetLayersWithVisibleObjects() As pccMapLayerSet Implements IG10IMap.GetLayersWithVisibleObjects
        SyncLock _locker
            Dim ls As New pccMapLayerSet

            Dim av As New pccMapView
            av = GetActualView()

            Dim rect As New pccBase.pccGeoPolygon()
            rect.AddVertice(New pccBase.pccGeoPoint(av.Center.X - av.MapWidth / 2, av.Center.Y - av.MapHeight / 2))
            rect.AddVertice(New pccBase.pccGeoPoint(av.Center.X - av.MapWidth / 2, av.Center.Y + av.MapHeight / 2))
            rect.AddVertice(New pccBase.pccGeoPoint(av.Center.X + av.MapWidth / 2, av.Center.Y + av.MapHeight / 2))
            rect.AddVertice(New pccBase.pccGeoPoint(av.Center.X + av.MapWidth / 2, av.Center.Y - av.MapHeight / 2))
            rect.AddVertice(New pccBase.pccGeoPoint(av.Center.X - av.MapWidth / 2, av.Center.Y - av.MapHeight / 2))

            Dim rect1 As New pccBase.pccGeoPolygon()
            rect1.AddVertice(New pccBase.pccGeoPoint(av.Center.X - av.MapWidth / 4, av.Center.Y - av.MapHeight / 4))
            rect1.AddVertice(New pccBase.pccGeoPoint(av.Center.X - av.MapWidth / 4, av.Center.Y + av.MapHeight / 4))
            rect1.AddVertice(New pccBase.pccGeoPoint(av.Center.X + av.MapWidth / 4, av.Center.Y + av.MapHeight / 4))
            rect1.AddVertice(New pccBase.pccGeoPoint(av.Center.X + av.MapWidth / 4, av.Center.Y - av.MapHeight / 4))
            rect1.AddVertice(New pccBase.pccGeoPoint(av.Center.X - av.MapWidth / 4, av.Center.Y - av.MapHeight / 4))


            Dim filtergeom As MgGeometry
            Dim wktReaderWriter As MgWktReaderWriter = New MgWktReaderWriter()
            filtergeom = wktReaderWriter.Read(rect.WKT)

            Dim filtergeom1 As MgGeometry
            filtergeom1 = wktReaderWriter.Read(rect1.WKT)

            For Each l As pccMapguideLayer In GetLayers.Layers

                If l.GetVisibility And l.IsVisible Then ' l.GetVisibility Then
                    Dim ml As MgLayer = l.Object

                    Dim FeatureGeometryFieldName As String = ml.GetFeatureGeometryName
                    If FeatureGeometryFieldName <> "" And LCase(FeatureGeometryFieldName) <> "image" And LCase(FeatureGeometryFieldName) <> "raster" Then
                        Dim layerdef As String = Me.ResourceService.GetResourceContent(ml.GetLayerDefinition).ToString().Replace(vbCrLf, "")

                        Dim encoding As New System.Text.UTF8Encoding()
                        Dim barray As Byte() = encoding.GetBytes(layerdef)
                        Dim xmlDoc As New XmlDocument()
                        xmlDoc.LoadXml(layerdef.Remove(0, layerdef.IndexOf(">") + 1))

                        Dim xmlNL As XmlNodeList = xmlDoc.GetElementsByTagName("MaxScale")

                        Dim maxScale As Double = -1

                        For Each el As XmlNode In xmlNL
                            If el IsNot Nothing Then
                                If IsNumeric(el.InnerText) Then
                                    If CDbl(el.InnerText) > maxScale Then
                                        maxScale = CDbl(el.InnerText)
                                    End If
                                End If
                            End If
                        Next

                        If av.Scale > maxScale And maxScale <> -1 Then
                            Continue For
                        End If

                        Dim qryoptions As New MgFeatureQueryOptions()
                        If filtergeom.GetGeometryType = 1 Then
                            qryoptions.SetSpatialFilter(FeatureGeometryFieldName, filtergeom.Buffer(Me._actualview.Scale / 500, Nothing), MgFeatureSpatialOperations.Intersects)
                        Else
                            qryoptions.SetSpatialFilter(FeatureGeometryFieldName, filtergeom, MgFeatureSpatialOperations.Intersects)
                        End If
                        qryoptions.SetFilter(ml.Filter)
                        Dim qryoptions1 As New MgFeatureQueryOptions()
                        If filtergeom.GetGeometryType = 1 Then
                            qryoptions1.SetSpatialFilter(FeatureGeometryFieldName, filtergeom.Buffer(Me._actualview.Scale / 500, Nothing), MgFeatureSpatialOperations.Intersects)
                        Else
                            qryoptions1.SetSpatialFilter(FeatureGeometryFieldName, filtergeom, MgFeatureSpatialOperations.Intersects)
                        End If
                        qryoptions1.SetFilter(ml.Filter)
                        Dim featureSource As MgResourceIdentifier = New MgResourceIdentifier(ml.GetFeatureSourceId())
                        Dim FeatureClassName As String = ml.GetFeatureClassName()
                        Dim FeatureSrvc As MgFeatureService = Me.GetMapServer.SiteConnection.CreateService(MgServiceType.FeatureService)
                        Try
                            Dim features As MgFeatureReader = FeatureSrvc.SelectFeatures(featureSource, FeatureClassName, qryoptions)

                            If features.ReadNext Then
                                ls.Add(l)

                            Else
                                Dim features1 As MgFeatureReader = FeatureSrvc.SelectFeatures(featureSource, FeatureClassName, qryoptions1)

                                If features1.ReadNext Then
                                    ls.Add(l)

                                End If
                            End If

                            Try

                                features.Close()

                            Catch ex As Exception
                                Debug.Print(ex.Message)
                            End Try

                        Catch ex As Exception
                            Debug.Print(ex.Message)
                        End Try
                    Else
                        ls.Add(l)
                    End If
                End If

            Next

            Return ls
        End SyncLock
    End Function
    Public Function GetLayersOn() As pccMapLayerSet Implements IG10IMap.GetLayersOn
        SyncLock _locker
            Dim ls As New pccMapLayerSet


            For Each l As pccMapguideLayer In GetLayers.Layers

                If l.GetVisibility And l.IsVisible Then
                    ls.Add(l)

                End If

            Next

            Return ls
        End SyncLock
    End Function

    <DataMember()>
    Public Property GetLayers As pccMapLayerSet Implements IG10IMap.GetLayers
        Get
            SyncLock _locker
                Return _layers
            End SyncLock
        End Get
        Set(ByVal value As pccMapLayerSet)
            Throw New Exception("pcc by design: pccMapguideMap.GetLayers() is a readonly property.")
        End Set
    End Property
    <DataMember()>
    Public Property GetLayersBase As pccMapBaseLayerSet Implements IG10IMap.GetLayersBase
        Get
            SyncLock _locker
                Return _layersbase
            End SyncLock
        End Get
        Set(ByVal value As pccMapBaseLayerSet)
            Throw New Exception("pcc by design: pccMapguideMap.GetLayersBase() is a readonly property.")
        End Set
    End Property
    <DataMember()>
    Public Property ZoomScaleBase64(ByVal p_center As pccBase.pccGeoPoint, ByVal p_newscale As Long, ByVal p_imagewidth As Long, ByVal p_imageheight As Long) As String Implements IG10IMap.ZoomScaleBase64
        Get
            SyncLock _locker
                Dim bb As Byte() = ZoomScale(p_center, p_newscale, p_imagewidth, p_imageheight)
                Return Convert.ToBase64String(bb)
            End SyncLock
        End Get
        Set(ByVal value As String)
            Throw New Exception("pcc by design: pccMapguideMap.ZoomScaleBase64() is a readonly property.")
        End Set
    End Property

    Public Function AddLayerfromServer(ByVal layerdefinition As String, ByVal name As String, ByVal legendlabel As String, Optional ByVal order As Integer = -1, Optional ByVal selectable As Boolean = True) As Boolean Implements IG10IMap.AddLayerfromServer
        SyncLock _locker

            Dim res As Boolean = False
            Try
                ' TODO: Testar

                Dim layerResourceID As New MgResourceIdentifier(layerdefinition)

                Dim mglayernovo As New MgLayer(layerResourceID, _resourceservice)
                If Trim(legendlabel) <> "" Then mglayernovo.SetLegendLabel(legendlabel)
                If Trim(name) <> "" Then mglayernovo.SetName(name)
                mglayernovo.SetDisplayInLegend(True)
                mglayernovo.SetSelectable(selectable)
                mglayernovo.SetVisible(True)
                mglayernovo.ForceRefresh()

                Dim colLayers As MgLayerCollection
                colLayers = Me.MapObject.GetLayers()

                'colLayers.Add(mglayernovo)

                If order <> -1 Then
                    colLayers.Insert(order, mglayernovo)
                Else
                    colLayers.Insert(0, mglayernovo)
                End If

                Me.MapObject.Save()
                Me.Refresh()

                res = True
            Catch ex As Exception
                Return False
            End Try

            Return res
        End SyncLock
    End Function

    Public Function AddLayerfromFile(ByVal layerdefinition As String, ByVal name As String, ByVal legendlabel As String, Optional ByVal order As Integer = -1, Optional ByVal selectable As Boolean = True) As Boolean Implements IG10IMap.AddLayerfromFile
        SyncLock _locker
            Dim res As Boolean = False

            Try
                '    ' TODO: Testar

                '%nomelayer%
                layerdefinition = Replace(layerdefinition, "%nomelayer%", name)
                Dim encoding As New System.Text.UTF8Encoding()
                Dim barray As Byte() = encoding.GetBytes(layerdefinition)

                ''Adicionar o layer ao mapa da sessão
                Dim byteSource1 As New MgByteSource(barray, Len(layerdefinition))
                byteSource1.SetMimeType(MgMimeType.Xml)
                Dim layerResourceID As New MgResourceIdentifier("Session:" & _mapserver.SessionID & "//" & name & "." & MgResourceType.LayerDefinition)
                Dim nullreader As MgByteReader
                _resourceservice.SetResource(layerResourceID, byteSource1.GetReader, nullreader)

                Dim mglayernovo As New MgLayer(layerResourceID, _resourceservice)
                If Trim(legendlabel) <> "" Then mglayernovo.SetLegendLabel(legendlabel)
                If Trim(name) <> "" Then mglayernovo.SetName(name)
                mglayernovo.SetDisplayInLegend(True)
                mglayernovo.SetSelectable(selectable)
                mglayernovo.SetVisible(True)
                mglayernovo.ForceRefresh()

                Dim colLayers As MgLayerCollection
                colLayers = Me.MapObject.GetLayers()

                If order <> -1 Then
                    colLayers.Insert(order, mglayernovo)
                Else
                    colLayers.Insert(0, mglayernovo)
                End If

                Me.MapObject.Save()
                Me.Refresh()


                res = True

            Catch ex As Exception

                res = False
            End Try

            Return res
        End SyncLock
    End Function

    Public Function GetCoordinateSystem() As mtdReferenceSystemInfo Implements IG10IMap.GetCoordinateSystem
        SyncLock _locker
            Return _cs
        End SyncLock

    End Function

    Public Function CreateCoordinateSystem(ByVal p_cswkt As String) As mtdReferenceSystemInfo Implements IG10IMap.CreateCoordinateSystem
        SyncLock _locker
            Dim csf As New MgCoordinateSystemFactory
            'Dim srs As String = _map.GetMapSRS()
            Dim code As String = csf.ConvertWktToCoordinateSystemCode(p_cswkt)

            Dim epsgcode As String = csf.ConvertWktToEpsgCode(p_cswkt)

            Dim rs As New mtdReferenceSystemInfo(p_cswkt, csf.CreateFromCode(code))
            rs.Code = epsgcode
            rs.CodeSpace = "EPSG"
            Return rs
        End SyncLock


    End Function

    Public Function CreateCoordinateSystem(ByVal p_epsgcode As Integer) As mtdReferenceSystemInfo Implements IG10IMap.CreateCoordinateSystem
        SyncLock _locker
            Dim csf As New MgCoordinateSystemFactory
            'Dim srs As String = _map.GetMapSRS()
            Dim wkt As String = csf.ConvertEpsgCodeToWkt(p_epsgcode)
            Dim code As String = csf.ConvertWktToCoordinateSystemCode(wkt)

            Dim rs As New mtdReferenceSystemInfo(wkt, csf.CreateFromCode(code))
            rs.Code = p_epsgcode.ToString
            rs.CodeSpace = "EPSG"
            Return rs
        End SyncLock


    End Function

    Public Function Transform(ByRef p_geometry As pccBase.pccGeoPoint, ByRef p_cssrc As mtdReferenceSystemInfo, ByRef p_csdst As mtdReferenceSystemInfo) As pccBase.pccGeoPoint Implements IG10IMap.Transform
        SyncLock _locker
            ' TODO: Refactoring desta coisa; é preciso ligar isto a pccGeoGeometry
            ' converter um ponto de cada vez não parece muito eficiente
            ' tudo até [t.TransformCoordinate(coordsrc)] devia acontecer uma só vez
            ' e depois sim , um ciclo de t.TransformCoordinate(coordsrc)

            Dim wktReaderWriter As MgWktReaderWriter = New MgWktReaderWriter()
            Dim geosrc As MgGeometry = wktReaderWriter.Read(p_geometry.WKT)

            Dim mf As New MgCoordinateSystemFactory
            Dim t As MgCoordinateSystemTransform

            t = mf.GetTransform(p_cssrc.Object, p_csdst.Object)

            Dim coordsrc As MgCoordinate = geosrc.Centroid.Coordinate

            t.TransformCoordinate(coordsrc)

            Return New pccGeoPoint(coordsrc.X, coordsrc.Y)
        End SyncLock
    End Function

    Public Function GetMetricCoordinateSystem() As mtdReferenceSystemInfo Implements IG10IMap.GetMetricCoordinateSystem
        SyncLock _locker
            Return _csmetric
        End SyncLock

    End Function

    Public Function GetMetersPerUnit() As Double Implements IG10IMap.GetMetersPerUnit
        SyncLock _locker
            Dim csf As New MgCoordinateSystemFactory
            'Dim srs As String = _map.GetMapSRS()
            Dim code As String = csf.ConvertWktToCoordinateSystemCode(_cs.WKT)

            Dim epsgcode As String = csf.ConvertWktToEpsgCode(_cs.WKT)
            Dim geogCS As MgCoordinateSystem = csf.CreateFromCode(code)
            Dim meters As Double = geogCS.ConvertCoordinateSystemUnitsToMeters(1)
            Return meters
        End SyncLock

    End Function



    Public Overrides Function GetHashCode() As Integer
        SyncLock _locker
            ' overrides para refrectir o estado dos layers no hashcode do mapa
            Dim hash As Integer = MyBase.GetHashCode
            If Me._mapdefid IsNot Nothing Then
                hash = Me._mapdefid.GetHashCode
            Else
                hash = 0
            End If
            If Not (_layers Is Nothing) Then
                For Each l As pccMapguideLayer In _layers.Layers
                    If l.GetVisibility Then hash = hash Xor l.GetHashCode
                Next
            End If
            Return hash
        End SyncLock

    End Function
    <DataMember()>
    Public Property GetExtent As pccBase.pccGeoRectangle Implements IG10IMap.GetExtent
        Get
            SyncLock _locker
                Return _extent
            End SyncLock
        End Get
        Set(ByVal value As pccBase.pccGeoRectangle)
            Throw New Exception("pcc by design: pccMapguideMap.GetExtent() is a readonly property.")
        End Set
    End Property
    <DataMember()>
    Public Property GetExtentCenter As pccGeoPoint Implements IG10IMap.GetExtentCenter
        Get
            SyncLock _locker
                Return New pccGeoPoint(_extent.Centroid.X, _extent.Centroid.Y)
            End SyncLock
        End Get
        Set(ByVal value As pccGeoPoint)
            Throw New Exception("pcc by design: pccMapguideMap.GetExtentCenter() is a readonly property.")
        End Set
    End Property
    <DataMember()>
    Public Property ActualSelection As Object Implements IG10IMap.ActualSelection
        Get
            SyncLock _locker
                Return _actual_selection
                'Return CType(_actual_selection, MgSelection)
            End SyncLock
        End Get
        Set(ByVal value As Object)
            Throw New Exception("pcc by design: pccMapguideMap.ActualSelection() is a readonly property.")
        End Set
    End Property

End Class

<DataContract()>
<Serializable()>
Public Class pccMapguideLayer
    Implements IG10IMapLayer

    Private _layer As Object
    Private _group As IG10IMapGroup
    <DataMember(Name:="Layer")>
    Private _internallayer As MgLayer
    Private Shared ReadOnly _locker As New Object()
    Private _base As Boolean

    Public Sub New()

    End Sub

    Public Sub New(ByRef p_maplayer As Object)
        SyncLock _locker
            _layer = p_maplayer
            _internallayer = CType(p_maplayer, MgLayer)
            _group = New pccMapguideGroup(CType(_layer, MgLayer).GetGroup)
            '_base = New pccMapguideLayer(CType(_layer, MgLayer).isbaselayer)
        End SyncLock
    End Sub

    <DataMember()>
    Public Property Name() As String Implements IG10IMapLayer.Name
        Get
            SyncLock _locker
                Return CType(_layer, MgLayer).GetName
            End SyncLock
        End Get
        Set(ByVal value As String)
            'TODO: throw error
            Throw New Exception("pcc by design: pccMapguideLayer.Name() is a readonly property.")
        End Set
    End Property


    <DataMember()>
    Public Property Id() As String Implements IG10IMapLayer.Id
        Get
            SyncLock _locker
                Return CType(_layer, MgLayer).GetObjectId
            End SyncLock
        End Get
        Set(ByVal value As String)
            'TODO: throw error
            Throw New Exception("pcc by design: pccMapguideLayer.Id() is a readonly property.")
        End Set
    End Property

    Public ReadOnly Property [Object]() As Object Implements IG10IMapLayer.Object
        Get
            SyncLock _locker
                Return _layer
            End SyncLock
        End Get
    End Property
    <DataMember()>
    Public Property LegendLabel() As String Implements IG10IMapLayer.LegendLabel
        Get
            SyncLock _locker
                Return CType(_layer, MgLayer).GetLegendLabel
            End SyncLock
        End Get
        Set(ByVal value As String)
            'TODO: throw error
            Throw New Exception("pcc by design: pccMapguideLayer.LegendLabel() is a readonly property.")
        End Set
    End Property
    <DataMember()>
    Public Property GetVisibility() As Boolean Implements IG10IMapLayer.GetVisibility
        Get
            SyncLock _locker
                Return CType(_layer, MgLayer).GetVisible()
            End SyncLock
        End Get
        Set(ByVal value As Boolean)
            'TODO: throw error
            Throw New Exception("pcc by design: pccMapguideLayer.GetVisibility() is a readonly property.")
        End Set
    End Property
    <DataMember()>
    Public Property IsVisible() As Boolean Implements IG10IMapLayer.IsVisible
        Get
            SyncLock _locker
                Return CType(_layer, MgLayer).IsVisible()
            End SyncLock
        End Get
        Set(ByVal value As Boolean)
            'TODO: throw error
            Throw New Exception("pcc by design: pccMapguideLayer.IsVisible() is a readonly property.")
        End Set
    End Property
    <DataMember()>
    Public Property GetSelectable() As Boolean Implements IG10IMapLayer.GetSelectable
        Get
            SyncLock _locker
                Return CType(_layer, MgLayer).GetSelectable()
            End SyncLock
        End Get
        Set(ByVal value As Boolean)
            'TODO: throw error
            Throw New Exception("pcc by design: pccMapguideLayer.GetSelectable() is a readonly property.")
        End Set
    End Property

    <DataMember()>
    Public Property GetLayerType() As Integer Implements IG10IMapLayer.GetLayerType
        Get
            SyncLock _locker
                Dim aux As String = CType(_layer, MgLayer).GetFeatureGeometryName
                '"Image" para o Autodesk raster provider e "Raster" para o GDAL provider. 
                If LCase(aux) = "image" Or LCase(aux) = "raster" Then
                    Return pccMapLayerType.Raster
                Else
                    Return pccMapLayerType.Vector
                End If
            End SyncLock
        End Get
        Set(ByVal value As Integer)
            'TODO: throw error
            Throw New Exception("pcc by design: pccMapguideLayer.GetLayerType() is a readonly property.")
        End Set
    End Property

    Public Sub SetVisibility(ByVal p_visible As Boolean) Implements IG10IMapLayer.SetVisibility
        SyncLock _locker

            CType(_layer, MgLayer).SetVisible(p_visible)

        End SyncLock
    End Sub

    Public ReadOnly Property Group() As IG10IMapGroup Implements IG10IMapLayer.Group
        Get
            SyncLock _locker
                Return _group
            End SyncLock
        End Get
    End Property
    Public Property IsBaseLayer() As Boolean 'Implements IG10IMap.IsBaseLayer
        Get
            SyncLock _locker
                Return _base
            End SyncLock
        End Get
        Set(ByVal value As Boolean)
            _base = value
        End Set
    End Property
    Public Overrides Function GetHashCode() As Integer
        SyncLock _locker
            Return Name.GetHashCode
        End SyncLock
    End Function

End Class



<DataContract()>
<Serializable()>
Public Class pccMapguideBaseLayer
    Implements IG10IMapBaseLayer

    Private _group As MgLayerGroup
    Private Shared ReadOnly _locker As New Object()
    Public Sub New()

    End Sub

    Public Sub New(ByVal p_maplayergroup As Object)
        SyncLock _locker
            _group = p_maplayergroup
        End SyncLock
    End Sub
    <DataMember()>
    Public Property Name() As String Implements IG10IMapBaseLayer.Name

        Get
            SyncLock _locker
                Return CType(_group, MgLayerGroup).GetName
            End SyncLock
        End Get
        Set(ByVal value As String)
            'TODO: throw error
            Throw New Exception("pcc by design: pccMapguideGroup.Name() is a readonly property.")
        End Set

    End Property

    <DataMember()>
    Public Property Id() As String Implements IG10IMapBaseLayer.Id
        Get
            SyncLock _locker
                Return CType(_group, MgLayerGroup).GetObjectId
            End SyncLock
        End Get
        Set(ByVal value As String)
            'TODO: throw error
            Throw New Exception("pcc by design: pccMapguideGroup.Id() is a readonly property.")
        End Set
    End Property

    <DataMember()>
    Public Property LegendLabel() As String Implements IG10IMapBaseLayer.LegendLabel

        Get
            SyncLock _locker
                Return CType(_group, MgLayerGroup).GetLegendLabel
            End SyncLock
        End Get
        Set(ByVal value As String)
            'TODO: throw error
            Throw New Exception("pcc by design: pccMapguideGroup.LegendLabel() is a readonly property.")
        End Set

    End Property
    Public ReadOnly Property [Object]() As Object Implements IG10IMapBaseLayer.Object
        Get
            SyncLock _locker
                Return _group
            End SyncLock
        End Get
    End Property
    <DataMember()>
    Public Property GetVisibility() As Boolean Implements IG10IMapBaseLayer.GetVisibility
        Get
            SyncLock _locker
                Return CType(_group, MgLayerGroup).GetVisible()
            End SyncLock
        End Get
        Set(ByVal value As Boolean)
            'TODO; throw error
            Throw New Exception("pcc by design: pccMapguideGroup.GetVisibility() is a readonly property.")
        End Set
    End Property
    <DataMember()>
    Public Property IsVisible() As Boolean Implements IG10IMapBaseLayer.IsVisible
        Get
            SyncLock _locker
                Return CType(_group, MgLayerGroup).IsVisible()
            End SyncLock
        End Get
        Set(ByVal value As Boolean)
            'TODO: throw error
            Throw New Exception("pcc by design: pccMapguideLayer.IsVisible() is a readonly property.")
        End Set
    End Property

    <DataMember()>
    Public Property GetSelectable() As Boolean Implements IG10IMapBaseLayer.GetSelectable
        Get
            SyncLock _locker
                Return Nothing
            End SyncLock
        End Get
        Set(ByVal value As Boolean)
            'TODO; throw error
            Throw New Exception("pcc by design: pccMapguideGroup.GetSelectable() is a readonly property.")
        End Set
    End Property

    Public Sub SetVisibility(ByVal p_visible As Boolean) Implements IG10IMapBaseLayer.SetVisibility
        SyncLock _locker
            CType(_group, MgLayerGroup).SetVisible(p_visible)
        End SyncLock
    End Sub
End Class

<DataContract()>
<Serializable()>
Public Class pccMapguideGroup
    Implements IG10IMapGroup

    Private _group As MgLayerGroup
    Private Shared ReadOnly _locker As New Object()
    Public Sub New()

    End Sub

    Public Sub New(ByVal p_maplayergroup As Object)
        SyncLock _locker
            _group = p_maplayergroup
        End SyncLock
    End Sub
    <DataMember()>
    Public Property Name() As String Implements IG10IMapGroup.Name

        Get
            SyncLock _locker
                Return CType(_group, MgLayerGroup).GetName
            End SyncLock
        End Get
        Set(ByVal value As String)
            'TODO: throw error
            Throw New Exception("pcc by design: pccMapguideGroup.Name() is a readonly property.")
        End Set

    End Property
    <DataMember()>
    Public Property LegendLabel() As String Implements IG10IMapGroup.LegendLabel

        Get
            SyncLock _locker
                Return CType(_group, MgLayerGroup).GetLegendLabel
            End SyncLock
        End Get
        Set(ByVal value As String)
            'TODO: throw error
            Throw New Exception("pcc by design: pccMapguideGroup.LegendLabel() is a readonly property.")
        End Set

    End Property
    <DataMember()>
    Public Property GetVisibility() As Boolean Implements IG10IMapGroup.GetVisibility
        Get
            SyncLock _locker
                Return CType(_group, MgLayerGroup).GetVisible()
            End SyncLock
        End Get
        Set(ByVal value As Boolean)
            'TODO; throw error
            Throw New Exception("pcc by design: pccMapguideGroup.GetVisibility() is a readonly property.")
        End Set
    End Property
    <DataMember()>
    Public Property IsVisible() As Boolean Implements IG10IMapGroup.IsVisible
        Get
            SyncLock _locker
                Return CType(_group, MgLayerGroup).IsVisible()
            End SyncLock
        End Get
        Set(ByVal value As Boolean)
            'TODO: throw error
            Throw New Exception("pcc by design: pccMapguideLayer.IsVisible() is a readonly property.")
        End Set
    End Property

    <DataMember()>
    Public Property GetSelectable() As Boolean Implements IG10IMapGroup.GetSelectable
        Get
            SyncLock _locker
                Return Nothing
            End SyncLock
        End Get
        Set(ByVal value As Boolean)
            'TODO; throw error
            Throw New Exception("pcc by design: pccMapguideGroup.GetSelectable() is a readonly property.")
        End Set
    End Property

    Public Sub SetVisibility(ByVal p_visible As Boolean) Implements IG10IMapGroup.SetVisibility
        SyncLock _locker
            CType(_group, MgLayerGroup).SetVisible(p_visible)
        End SyncLock
    End Sub

End Class


<DataContract()>
<KnownType(GetType(List(Of pccMapguideGroup)))>
<KnownType(GetType(pccMapguideGroup))>
Public Class pccMapGroupLayerSet
    Private _layers As List(Of pccMapguideGroup)
    Private Shared ReadOnly _locker As New Object()
    Public Sub New()
        SyncLock _locker
            _layers = New List(Of pccMapguideGroup)
        End SyncLock
    End Sub
    <DataMember()>
    Public Property Layers As List(Of pccMapguideGroup)
        Get
            SyncLock _locker
                Return _layers
            End SyncLock
        End Get
        Set(ByVal value As List(Of pccMapguideGroup))
            SyncLock _locker
                _layers = value
            End SyncLock
        End Set
    End Property

    Public Function Size() As Long
        SyncLock _locker
            Return _layers.Count
        End SyncLock
    End Function

    Public Sub Add(ByRef p_layer As IG10IMapGroup)
        SyncLock _locker
            _layers.Add(p_layer)
        End SyncLock
    End Sub

    Public ReadOnly Property Item(ByVal p_idx As Long) As IG10IMapGroup
        Get
            SyncLock _locker
                If p_idx >= Size() Then
                    Return Nothing
                Else
                    Return _layers.Item(p_idx)
                End If
            End SyncLock
        End Get
    End Property

End Class


<DataContract(), KnownType(GetType(pccGeoMultiPoint)), KnownType(GetType(pccGeoPoint))>
<KnownType(GetType(pccGeoMultiLineString)), KnownType(GetType(pccGeoLineString))>
<KnownType(GetType(pccGeoMultiPolygon)), KnownType(GetType(pccGeoPolygon))>
<KnownType(GetType(pccGeoCollection)), KnownType(GetType(pccGeoRectangle))>
<KnownType(GetType(pccMapguideLayer))>
<KnownType(GetType(pccGeoGeometry))>
<KnownType(GetType(pccMapObjectProperty))>
<Serializable()>
Public Class pccMapguideObject
    Implements IG10IMapObject


    Private _id As String
    Private _name As String
    Private _url As String
    Private _layer As pccMapguideLayer
    Private _geom As pccGeoGeometry
    Private _properties As pccMapObjectProperty()
    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

    End Sub
    <DataMember()>
    Public Property Layer() As pccMapguideLayer Implements IG10IMapObject.Layer

        Get
            SyncLock _locker
                Return _layer
            End SyncLock
        End Get

        Set(ByVal value As pccMapguideLayer)
            SyncLock _locker
                _layer = value
            End SyncLock
        End Set

    End Property
    <DataMember()>
    Public Property Properties() As pccMapObjectProperty() Implements IG10IMapObject.Properties

        Get
            SyncLock _locker
                Return _properties
            End SyncLock
        End Get

        Set(ByVal value As pccMapObjectProperty())
            SyncLock _locker
                _properties = value
            End SyncLock
        End Set

    End Property
    <DataMember()>
    Public Property Geom() As pccBase.pccGeoGeometry Implements IG10IMapObject.Geom

        Get
            SyncLock _locker
                Return _geom
            End SyncLock
        End Get

        Set(ByVal value As pccBase.pccGeoGeometry)
            SyncLock _locker
                _geom = value
            End SyncLock
        End Set

    End Property
    <DataMember()>
    Public Property Id() As String Implements IG10IMapObject.Id

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
    <DataMember()>
    Public Property Name() As String Implements IG10IMapObject.Name

        Get
            SyncLock _locker
                Return _name
            End SyncLock
        End Get

        Set(ByVal value As String)
            SyncLock _locker
                _name = value
            End SyncLock
        End Set

    End Property
    <DataMember()>
    Public Property URL() As String Implements IG10IMapObject.Url

        Get
            SyncLock _locker
                Return _url
            End SyncLock
        End Get

        Set(ByVal value As String)
            SyncLock _locker
                _url = value
            End SyncLock
        End Set

    End Property
End Class

