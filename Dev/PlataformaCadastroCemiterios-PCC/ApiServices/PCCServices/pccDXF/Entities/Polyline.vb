
Imports System.Collections.Generic
Imports pccDXF4.Tables

Namespace Entities
    ''' <summary>
    ''' Represents a polyline <see cref="pccDXF4.Entities.IEntityObject">entity</see>.
    ''' </summary>
    ''' <remarks>
    ''' The <see cref="pccDXF4.Entities.LightWeightPolyline">LightWeightPolyline</see> and
    ''' the <see cref="pccDXF4.Entities.Polyline">Polyline</see> are essentially the same entity, they are both here for compatibility reasons.
    ''' When a AutoCad12 file is saved all lightweight polylines will be converted to polylines, while for AutoCad2000 and later versions all
    ''' polylines will be converted to lightweight polylines.
    ''' </remarks>
    Public Class Polyline
        Inherits DxfObject
        Implements IPolyline


        Private ReadOnly m_endSequence As EndSequence
        Private Const m_TYPE As EntityType = EntityType.Polyline
        Private m_vertexes As List(Of PolylineVertex)
        Private m_isClosed As Boolean
        Private m_flags As PolylineTypeFlags
        Private m_layer As Layer
        Private m_color As AciColor
        Private m_lineType As LineType
        Private m_normal As Vector3f
        Private m_elevation As Single
        Private m_thickness As Single
        Private m_xData As Dictionary(Of ApplicationRegistry, XData)

        Private Shared ReadOnly _locker As New Object()


        ''' <summary>
        ''' Initializes a new instance of the <c>Polyline</c> class.
        ''' </summary>
        ''' <param name="vertexes">Polyline vertex list in object coordinates.</param>
        ''' <param name="isClosed">Sets if the polyline is closed</param>
        Public Sub New(ByVal vertexes As List(Of PolylineVertex), ByVal isClosed As Boolean)
            MyBase.New(DxfObjectCode.Polyline)
            SyncLock _locker
                Me.m_vertexes = vertexes
                Me.m_isClosed = isClosed
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_normal = Vector3f.UnitZ
                Me.m_elevation = 0.0F
                Me.m_thickness = 0.0F
                Me.m_flags = If(isClosed, PolylineTypeFlags.ClosedPolylineOrClosedPolygonMeshInM, PolylineTypeFlags.OpenPolyline)
                Me.m_endSequence = New EndSequence()
            End SyncLock
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <c>Polyline</c> class.
        ''' </summary>
        ''' <param name="vertexes">Polyline <see cref="PolylineVertex">vertex</see> list in object coordinates.</param>
        Public Sub New(ByVal vertexes As List(Of PolylineVertex))
            MyBase.New(DxfObjectCode.Polyline)
            SyncLock _locker
                Me.m_vertexes = vertexes
                Me.m_isClosed = False
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_normal = Vector3f.UnitZ
                Me.m_elevation = 0.0F
                Me.m_thickness = 0.0F
                Me.m_flags = PolylineTypeFlags.OpenPolyline
                Me.m_endSequence = New EndSequence()
            End SyncLock
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <c>Polyline</c> class.
        ''' </summary>
        Public Sub New()
            MyBase.New(DxfObjectCode.Polyline)
            SyncLock _locker
                Me.m_vertexes = New List(Of PolylineVertex)()
                Me.m_isClosed = False
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_normal = Vector3f.UnitZ
                Me.m_elevation = 0.0F
                Me.m_flags = PolylineTypeFlags.OpenPolyline
                Me.m_endSequence = New EndSequence()
            End SyncLock
        End Sub





        ''' <summary>
        ''' Gets or sets the polyline <see cref="pccDXF4.Entities.PolylineVertex">vertex</see> list.
        ''' </summary>
        Public Property Vertexes() As List(Of PolylineVertex)
            Get
                SyncLock _locker
                    Return Me.m_vertexes
                End SyncLock
            End Get
            Set(ByVal value As List(Of PolylineVertex))
                SyncLock _locker
                    If value Is Nothing Then
                        Throw New ArgumentNullException("value")
                    End If
                    Me.m_vertexes = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets if the polyline is closed.
        ''' </summary>
        Public Overridable Property IsClosed() As Boolean
            Get
                SyncLock _locker
                    Return Me.m_isClosed
                End SyncLock
            End Get
            Set(ByVal value As Boolean)
                SyncLock _locker
                    Me.m_flags = Me.m_flags Or If(value, PolylineTypeFlags.ClosedPolylineOrClosedPolygonMeshInM, PolylineTypeFlags.OpenPolyline)
                    Me.m_isClosed = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the polyline <see cref="pccDXF4.Vector3f">normal</see>.
        ''' </summary>
        Public Property Normal() As Vector3f
            Get
                SyncLock _locker
                    Return Me.m_normal
                End SyncLock
            End Get
            Set(ByVal value As Vector3f)
                SyncLock _locker
                    If Vector3f.Zero = value Then
                        Throw New ArgumentNullException("value", "The normal can not be the zero vector")
                    End If
                    value.Normalize()
                    Me.m_normal = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the polyline thickness.
        ''' </summary>
        Public Property Thickness() As Single
            Get
                SyncLock _locker
                    Return Me.m_thickness
                End SyncLock
            End Get
            Set(ByVal value As Single)
                SyncLock _locker
                    Me.m_thickness = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the polyline elevation.
        ''' </summary>
        Public Property Elevation() As Single
            Get
                SyncLock _locker
                    Return Me.m_elevation
                End SyncLock
            End Get
            Set(ByVal value As Single)
                SyncLock _locker
                    Me.m_elevation = value
                End SyncLock
            End Set
        End Property

        Friend ReadOnly Property EndSequence() As EndSequence
            Get
                SyncLock _locker
                    Return Me.m_endSequence
                End SyncLock
            End Get
        End Property
         

        ''' <summary>
        ''' Gets the polyline type.
        ''' </summary>
        Public ReadOnly Property Flags() As PolylineTypeFlags Implements IPolyline.Flags
            Get
                SyncLock _locker
                    Return Me.m_flags
                End SyncLock
            End Get
        End Property

 

        ''' <summary>
        ''' Gets the entity <see cref="pccDXF4.Entities.EntityType">type</see>.
        ''' </summary>
        Public ReadOnly Property Type() As EntityType Implements IEntityObject.Type
            Get
                SyncLock _locker
                    Return m_TYPE
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the entity <see cref="pccDXF4.AciColor">color</see>.
        ''' </summary>
        Public Property Color() As AciColor Implements IEntityObject.Color
            Get
                SyncLock _locker
                    Return Me.m_color
                End SyncLock
            End Get
            Set(ByVal value As AciColor)
                SyncLock _locker
                    If value Is Nothing Then
                        Throw New ArgumentNullException("value")
                    End If
                    Me.m_color = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the entity <see cref="pccDXF4.Tables.Layer">layer</see>.
        ''' </summary>
        Public Property Layer() As Layer Implements IEntityObject.Layer
            Get
                SyncLock _locker
                    Return Me.m_layer
                End SyncLock
            End Get
            Set(ByVal value As Layer)
                SyncLock _locker
                    If value Is Nothing Then
                        Throw New ArgumentNullException("value")
                    End If
                    Me.m_layer = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the entity <see cref="pccDXF4.Tables.LineType">line type</see>.
        ''' </summary>
        Public Property LineType() As LineType Implements IEntityObject.LineType
            Get
                SyncLock _locker
                    Return Me.m_lineType
                End SyncLock
            End Get
            Set(ByVal value As LineType)
                SyncLock _locker
                    If value Is Nothing Then
                        Throw New ArgumentNullException("value")
                    End If
                    Me.m_lineType = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the entity <see cref="pccDXF4.XData">extende data</see>.
        ''' </summary>
        Public Property XData() As Dictionary(Of ApplicationRegistry, XData) Implements IEntityObject.XData
            Get
                SyncLock _locker
                    Return Me.m_xData
                End SyncLock
            End Get
            Set(ByVal value As Dictionary(Of ApplicationRegistry, XData))
                SyncLock _locker
                    Me.m_xData = value
                End SyncLock
            End Set
        End Property

 

        ''' <summary>
        ''' Sets a constant width for all the polyline segments.
        ''' </summary>
        ''' <param name="width">Polyline width.</param>
        Public Sub SetConstantWidth(ByVal width As Single)
            SyncLock _locker
                For Each v As PolylineVertex In Me.m_vertexes
                    v.BeginThickness = width
                    v.EndThickness = width
                Next
            End SyncLock
        End Sub

        ''' <summary>
        ''' Converts the polyline in a <see cref="pccDXF4.Entities.LightWeightPolyline">LightWeightPolyline</see>.
        ''' </summary>
        ''' <returns>A new instance of <see cref="LightWeightPolyline">LightWeightPolyline</see> that represents the lightweight polyline.</returns>
        Public Function ToLightWeightPolyline() As LightWeightPolyline
            SyncLock _locker
                Dim polyVertexes As New List(Of LightWeightPolylineVertex)()
                For Each v As PolylineVertex In Me.m_vertexes
                    polyVertexes.Add(New LightWeightPolylineVertex(v.Location) With { _
                     .BeginThickness = v.BeginThickness, _
                     .Bulge = v.Bulge, _
                     .EndThickness = v.EndThickness _
                    })
                Next

                Return New LightWeightPolyline(polyVertexes, Me.m_isClosed) With { _
                 .Color = Me.m_color, _
                 .Layer = Me.m_layer, _
                 .LineType = Me.m_lineType, _
                 .Normal = Me.m_normal, _
                 .Elevation = Me.m_elevation, _
                 .Thickness = Me.m_thickness, _
                 .XData = Me.m_xData _
                }
            End SyncLock
        End Function

        ''' <summary>
        ''' Obtains a list of vertexes that represent the polyline approximating the curve segments as necessary.
        ''' </summary>
        ''' <param name="bulgePrecision">Curve segments precision (a value of zero means that no approximation will be made).</param>
        ''' <param name="weldThreshold">Tolerance to consider if two new generated vertexes are equal.</param>
        ''' <param name="bulgeThreshold">Minimun distance from which approximate curved segments of the polyline.</param>
        ''' <returns>The return vertexes are expresed in object coordinate system.</returns>
        Public Function PoligonalVertexes(ByVal bulgePrecision As Integer, ByVal weldThreshold As Single, ByVal bulgeThreshold As Single) As List(Of Vector2f)
            SyncLock _locker
                Dim ocsVertexes As New List(Of Vector2f)()

                Dim index As Integer = 0

                For Each vertex As PolylineVertex In Me.Vertexes
                    Dim bulge As Single = vertex.Bulge
                    Dim p1 As Vector2f
                    Dim p2 As Vector2f

                    If index = Me.Vertexes.Count - 1 Then
                        p1 = New Vector2f(vertex.Location.X, vertex.Location.Y)
                        p2 = New Vector2f(Me.m_vertexes(0).Location.X, Me.m_vertexes(0).Location.Y)
                    Else
                        p1 = New Vector2f(vertex.Location.X, vertex.Location.Y)
                        p2 = New Vector2f(Me.m_vertexes(index + 1).Location.X, Me.m_vertexes(index + 1).Location.Y)
                    End If

                    If Not p1.Equals(p2, weldThreshold) Then
                        If bulge = 0 OrElse bulgePrecision = 0 Then
                            ocsVertexes.Add(p1)
                        Else
                            Dim c As Single = Vector2f.Distance(p1, p2)
                            If c >= bulgeThreshold Then
                                Dim s As Single = (c / 2) * Math.Abs(bulge)
                                Dim r As Single = ((c / 2) * (c / 2) + s * s) / (2 * s)
                                Dim theta As Single = CSng(4 * Math.Atan(Math.Abs(bulge)))
                                Dim gamma As Single = CSng((Math.PI - theta) / 2)
                                Dim phi As Single

                                If bulge > 0 Then
                                    phi = Vector2f.AngleBetween(Vector2f.UnitX, p2 - p1) + gamma
                                Else
                                    phi = Vector2f.AngleBetween(Vector2f.UnitX, p2 - p1) - gamma
                                End If

                                Dim center As New Vector2f(CSng(p1.X + r * Math.Cos(phi)), CSng(p1.Y + r * Math.Sin(phi)))
                                Dim a1 As Vector2f = p1 - center
                                Dim angle As Single = 4 * CSng(Math.Atan(bulge)) / (bulgePrecision + 1)

                                ocsVertexes.Add(p1)
                                For i As Integer = 1 To bulgePrecision
                                    Dim curvePoint As New Vector2f()
                                    Dim prevCurvePoint As New Vector2f(Me.m_vertexes(Me.m_vertexes.Count - 1).Location.X, Me.m_vertexes(Me.m_vertexes.Count - 1).Location.Y)
                                    curvePoint.X = center.X + CSng(Math.Cos(i * angle) * a1.X - Math.Sin(i * angle) * a1.Y)
                                    curvePoint.Y = center.Y + CSng(Math.Sin(i * angle) * a1.X + Math.Cos(i * angle) * a1.Y)

                                    If Not curvePoint.Equals(prevCurvePoint, weldThreshold) AndAlso Not curvePoint.Equals(p2, weldThreshold) Then
                                        ocsVertexes.Add(curvePoint)
                                    End If
                                Next
                            Else
                                ocsVertexes.Add(p1)
                            End If
                        End If
                    End If
                    index += 1
                Next

                Return ocsVertexes
            End SyncLock
        End Function





        ''' <summary>
        ''' Asigns a handle to the object based in a integer counter.
        ''' </summary>
        ''' <param name="entityNumber">Number to asign.</param>
        ''' <returns>Next avaliable entity number.</returns>
        ''' <remarks>
        ''' Some objects might consume more than one, is, for example, the case of polylines that will asign
        ''' automatically a handle to its vertexes. The entity number will be converted to an hexadecimal number.
        ''' </remarks>
        Friend Overrides Function AsignHandle(ByVal entityNumber As Integer) As Integer
            SyncLock _locker
                For Each v As PolylineVertex In Me.m_vertexes
                    entityNumber = v.AsignHandle(entityNumber)
                Next
                entityNumber = Me.m_endSequence.AsignHandle(entityNumber)
                Return MyBase.AsignHandle(entityNumber)
            End SyncLock
        End Function

        ''' <summary>
        ''' Converts the value of this instance to its equivalent string representation.
        ''' </summary>
        ''' <returns>The string representation.</returns>
        Public Overrides Function ToString() As String
            SyncLock _locker
                Return m_TYPE.ToString()
            End SyncLock
        End Function


    End Class
End Namespace
