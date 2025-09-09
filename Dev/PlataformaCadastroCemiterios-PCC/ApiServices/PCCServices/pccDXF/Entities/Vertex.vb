

Imports System.Collections.Generic
Imports pccDXF4.Tables

Namespace Entities
    ''' <summary>
    ''' Represents a dxf Vertex.
    ''' </summary>
    ''' <remarks>
    ''' The Vertex class holds all the information read from the dxf file even if its needed or not.
    ''' For internal use only.
    ''' </remarks>
    Friend Class Vertex
        Inherits DxfObject




        Private m_flags As VertexTypeFlags
        Private m_location As Vector3f
        Private m_vertexIndexes As Integer()
        Private m_beginThickness As Single
        Private m_endThickness As Single
        Private m_bulge As Single
        Private m_color As AciColor
        Private m_layer As Layer
        Private m_lineType As LineType
        Private m_xData As Dictionary(Of ApplicationRegistry, XData)
         
        Private Shared ReadOnly _locker As New Object()
         
        ''' <summary>
        ''' Initializes a new instance of the <c>Vertex</c> class.
        ''' </summary>
        Public Sub New()
            MyBase.New(DxfObjectCode.Vertex)
            SyncLock _locker
                Me.m_flags = VertexTypeFlags.PolylineVertex
                Me.m_location = Vector3f.Zero
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_bulge = 0.0F
                Me.m_beginThickness = 0.0F
                Me.m_endThickness = 0.0F
            End SyncLock
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <c>Vertex</c> class.
        ''' </summary>
        ''' <param name="location">Vertex <see cref="pccDXF4.Vector3f">location</see>.</param>
        Public Sub New(ByVal location As Vector3f)
            MyBase.New(DxfObjectCode.Vertex)
            SyncLock _locker
                Me.m_flags = VertexTypeFlags.PolylineVertex
                Me.m_location = location
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_bulge = 0.0F
                Me.m_beginThickness = 0.0F
                Me.m_endThickness = 0.0F
            End SyncLock
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <c>Vertex</c> class.
        ''' </summary>
        ''' <param name="location">Vertex <see cref="pccDXF4.Vector2f">location</see>.</param>
        Public Sub New(ByVal location As Vector2f)
            MyBase.New(DxfObjectCode.Vertex)
            SyncLock _locker
                Me.m_flags = VertexTypeFlags.PolylineVertex
                Me.m_location = New Vector3f(location.X, location.Y, 0.0F)
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_bulge = 0.0F
                Me.m_beginThickness = 0.0F
                Me.m_endThickness = 0.0F
            End SyncLock
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <c>Vertex</c> class.
        ''' </summary>
        ''' <param name="x">X coordinate.</param>
        ''' <param name="y">Y coordinate.</param>
        ''' <param name="z">Z coordinate.</param>
        Public Sub New(ByVal x As Single, ByVal y As Single, ByVal z As Single)
            MyBase.New(DxfObjectCode.Vertex)
            SyncLock _locker
                Me.m_flags = VertexTypeFlags.PolylineVertex
                Me.m_location = New Vector3f(x, y, z)
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_bulge = 0.0F
                Me.m_beginThickness = 0.0F
                Me.m_endThickness = 0.0F
            End SyncLock
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <c>Vertex</c> class.
        ''' </summary>
        ''' <param name="x">X coordinate.</param>
        ''' <param name="y">Y coordinate.</param>
        Public Sub New(ByVal x As Single, ByVal y As Single)
            MyBase.New(DxfObjectCode.Vertex)
            SyncLock _locker
                Me.m_flags = VertexTypeFlags.PolylineVertex
                Me.m_location = New Vector3f(x, y, 0.0F)
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_bulge = 0.0F
                Me.m_beginThickness = 0.0F
                Me.m_endThickness = 0.0F
            End SyncLock
        End Sub





        ''' <summary>
        ''' Gets or sets the polyline vertex <see cref="pccDXF4.Vector3f">location</see>.
        ''' </summary>
        Public Property Location() As Vector3f
            Get
                SyncLock _locker
                    Return Me.m_location
                End SyncLock
            End Get
            Set(ByVal value As Vector3f)
                SyncLock _locker
                    Me.m_location = value
                End SyncLock
            End Set
        End Property

        Public Property VertexIndexes() As Integer()
            Get
                SyncLock _locker
                    Return Me.m_vertexIndexes
                End SyncLock
            End Get
            Set(ByVal value As Integer())
                SyncLock _locker
                    Me.m_vertexIndexes = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the light weight polyline begin thickness.
        ''' </summary>
        Public Property BeginThickness() As Single
            Get
                SyncLock _locker
                    Return Me.m_beginThickness
                End SyncLock
            End Get
            Set(ByVal value As Single)
                SyncLock _locker
                    Me.m_beginThickness = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the light weight polyline end thickness.
        ''' </summary>
        Public Property EndThickness() As Single
            Get
                SyncLock _locker
                    Return Me.m_endThickness
                End SyncLock
            End Get
            Set(ByVal value As Single)
                SyncLock _locker
                    Me.m_endThickness = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or set the light weight polyline bulge.Accepted values range from 0 to 1.
        ''' </summary>
        ''' <remarks>
        ''' The bulge is the tangent of one fourth the included angle for an arc segment, 
        ''' made negative if the arc goes clockwise from the start point to the endpoint. 
        ''' A bulge of 0 indicates a straight segment, and a bulge of 1 is a semicircle.
        ''' </remarks>
        Public Property Bulge() As Single
            Get
                SyncLock _locker
                    Return Me.m_bulge
                End SyncLock
            End Get
            Set(ByVal value As Single)
                SyncLock _locker
                    If Me.m_bulge < 0.0 OrElse Me.m_bulge > 1.0F Then
                        Throw New ArgumentOutOfRangeException("value", value, "The bulge must be a value between zero and one")
                    End If
                    Me.m_bulge = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the vertyex type.
        ''' </summary>
        Public Property Flags() As VertexTypeFlags
            Get
                SyncLock _locker
                    Return Me.m_flags
                End SyncLock
            End Get
            Set(ByVal value As VertexTypeFlags)
                SyncLock _locker
                    Me.m_flags = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the entity color.
        ''' </summary>
        Public Property Color() As AciColor
            Get
                SyncLock _locker
                    Return Me.m_color
                End SyncLock
            End Get
            Set(ByVal value As AciColor)
                SyncLock _locker
                    Me.m_color = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the entity layer.
        ''' </summary>
        Public Property Layer() As Layer
            Get
                SyncLock _locker
                    Return Me.m_layer
                End SyncLock
            End Get
            Set(ByVal value As Layer)
                SyncLock _locker
                    Me.m_layer = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the entity line type.
        ''' </summary>
        Public Property LineType() As LineType
            Get
                SyncLock _locker
                    Return Me.m_lineType
                End SyncLock
            End Get
            Set(ByVal value As LineType)
                SyncLock _locker
                    Me.m_lineType = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the entity extended data.
        ''' </summary>
        Public Property XData() As Dictionary(Of ApplicationRegistry, XData)
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





        Public Overrides Function ToString() As String
            SyncLock _locker
                Return Me.CodeName
            End SyncLock
        End Function


    End Class
End Namespace
