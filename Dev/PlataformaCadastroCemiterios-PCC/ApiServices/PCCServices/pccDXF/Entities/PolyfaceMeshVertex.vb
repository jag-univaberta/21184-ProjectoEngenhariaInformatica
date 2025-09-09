
Imports System.Collections.Generic
Imports pccDXF4.Tables

Namespace Entities
    ''' <summary>
    ''' Represents a polyface mesh vertex. 
    ''' </summary>
    Public Class PolyfaceMeshVertex
        Inherits DxfObject
        Implements IVertex


        Protected Const m_TYPE As EntityType = EntityType.PolyfaceMeshVertex
        Protected m_flags As VertexTypeFlags
        Protected m_location As Vector3f
        Protected m_color As AciColor
        Protected m_layer As Layer
        Protected m_lineType As LineType
        Protected m_xData As Dictionary(Of ApplicationRegistry, XData)

        Private Shared ReadOnly _locker As New Object()

        ''' <summary>
        ''' Initializes a new instance of the <c>PolylineVertex</c> class.
        ''' </summary>
        Public Sub New()
            MyBase.New(DxfObjectCode.Vertex)
            SyncLock _locker
                Me.m_flags = VertexTypeFlags.PolyfaceMeshVertex Or VertexTypeFlags.Polygon3dMesh
                Me.m_location = Vector3f.Zero
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
            End SyncLock
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <c>PolylineVertex</c> class.
        ''' </summary>
        ''' <param name="location">Polyface mesh vertex <see cref="Vector3f">location</see>.</param>
        Public Sub New(ByVal location As Vector3f)
            MyBase.New(DxfObjectCode.Vertex)
            SyncLock _locker
                Me.m_flags = VertexTypeFlags.PolyfaceMeshVertex Or VertexTypeFlags.Polygon3dMesh
                Me.m_location = location
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
            End SyncLock
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the PolylineVertex class.
        ''' </summary>
        ''' <param name="x">X coordinate.</param>
        ''' <param name="y">Y coordinate.</param>
        ''' <param name="z">Z coordinate.</param>
        Public Sub New(ByVal x As Single, ByVal y As Single, ByVal z As Single)
            MyBase.New(DxfObjectCode.Vertex)
            SyncLock _locker
                Me.m_flags = VertexTypeFlags.PolyfaceMeshVertex Or VertexTypeFlags.Polygon3dMesh
                Me.m_location = New Vector3f(x, y, z)
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
            End SyncLock
        End Sub
         

        ''' <summary>
        ''' Gets or sets the polyface mesh vertex <see cref="pccDXF4.Vector3f">location</see>.
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

 

        ''' <summary>
        ''' Gets the vertex type.
        ''' </summary>
        Public ReadOnly Property Flags() As VertexTypeFlags Implements IVertex.Flags
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
        ''' Converts the value of this instance to its equivalent string representation.
        ''' </summary>
        ''' <returns>The string representation.</returns>
        Public Overrides Function ToString() As String
            SyncLock _locker
                Return [String].Format("{0} {1}", m_TYPE, Me.m_location)
            End SyncLock
        End Function


    End Class
End Namespace
