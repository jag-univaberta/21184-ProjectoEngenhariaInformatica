
Imports System.Collections.Generic
Imports pccDXF4.Tables

Namespace Entities
    ''' <summary>
    ''' Represents a polyface mesh face. 
    ''' </summary>
    ''' <remarks>
    ''' The way the vertex indexes for a polyfacemesh are defined follows the dxf documentation.
    ''' The values of the vertex indexes specify one of the previously defined vertexes by the index in the list plus one.
    ''' If the index is negative, the edge that begins with that vertex is invisible.
    ''' For example if the vertex index in the list is 0 the vertex index for the face will be 1, and
    ''' if the edge between the vertexes 0 and 1 is hidden the vertex index for the face will be -1.
    ''' The maximum number of vertex indexes in a face is 4.
    ''' </remarks>
    Public Class PolyfaceMeshFace
        Inherits DxfObject
        Implements IVertex

        Protected Const m_TYPE As EntityType = EntityType.PolylineVertex
        Protected m_flags As VertexTypeFlags
        Protected m_vertexIndexes As Integer()
        Protected m_color As AciColor
        Protected m_layer As Layer
        Protected m_lineType As LineType
        Protected m_xData As Dictionary(Of ApplicationRegistry, XData)
         
        Private Shared ReadOnly _locker As New Object()

        ''' <summary>
        ''' Initializes a new instance of the <c>PolyfaceMeshFace</c> class.
        ''' </summary>
        ''' <remarks>
        ''' By default the face is made up of three vertexes.
        ''' </remarks>
        Public Sub New()
            MyBase.New(DxfObjectCode.Vertex)
            SyncLock _locker
                Me.m_flags = VertexTypeFlags.PolyfaceMeshVertex
                Me.m_vertexIndexes = New Integer(2) {}
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
            End SyncLock
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <c>PolyfaceMeshFace</c> class.
        ''' </summary>
        ''' <param name="vertexIndexes">Array of indexes to the vertex list of a polyface mesh that makes up the face.</param>
        Public Sub New(ByVal vertexIndexes As Integer())
            MyBase.New(DxfObjectCode.Vertex)
            SyncLock _locker
                If vertexIndexes Is Nothing Then
                    Throw New ArgumentNullException("vertexIndexes")
                End If
                If vertexIndexes.Length > 4 Then
                    Throw New ArgumentOutOfRangeException("vertexIndexes", vertexIndexes.Length, "The maximun number of index vertexes in a face is 4")
                End If

                Me.m_flags = VertexTypeFlags.PolyfaceMeshVertex
                Me.m_vertexIndexes = vertexIndexes
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
            End SyncLock
        End Sub

         
        ''' <summary>
        ''' Gets or sets the array of indexes to the vertex list of a polyface mesh that makes up the face.
        ''' </summary>
        Public Property VertexIndexes() As Integer()
            Get
                SyncLock _locker
                    Return Me.m_vertexIndexes
                End SyncLock
            End Get
            Set(ByVal value As Integer())
                SyncLock _locker
                    If value Is Nothing Then
                        Throw New ArgumentNullException("value")
                    End If
                    If value.Length > 4 Then
                        Throw New ArgumentOutOfRangeException("value", Me.m_vertexIndexes.Length, "The maximun number of index vertexes in a face is 4")
                    End If

                    Me.m_vertexIndexes = value
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
                Return m_TYPE.ToString()
            End SyncLock
        End Function


    End Class
End Namespace
