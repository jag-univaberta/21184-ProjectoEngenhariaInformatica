

Imports System.Collections.Generic
Imports pccDXF4.Tables

Namespace Entities

    ''' <summary>
    ''' Represents a polyface mesh <see cref="pccDXF4.Entities.IEntityObject">entity</see>.
    ''' </summary>
    Public Class PolyfaceMesh
        Inherits DxfObject
        Implements IPolyline

        Private ReadOnly m_endSequence As EndSequence
        Protected Const m_TYPE As EntityType = EntityType.PolyfaceMesh
        Private m_faces As List(Of PolyfaceMeshFace)
        Private m_vertexes As List(Of PolyfaceMeshVertex)
        Protected m_flags As PolylineTypeFlags
        Protected m_layer As Layer
        Protected m_color As AciColor
        Protected m_lineType As LineType
        Protected m_xData As Dictionary(Of ApplicationRegistry, XData)
 
        Private Shared ReadOnly _locker As New Object()

        ''' <summary>
        ''' Initializes a new instance of the <c>PolyfaceMesh</c> class.
        ''' </summary>
        ''' <param name="vertexes">Polyface mesh <see cref="PolyfaceMeshVertex">vertex</see> list.</param>
        ''' <param name="faces">Polyface mesh <see cref="PolyfaceMeshFace">faces</see> list.</param>
        Public Sub New(ByVal vertexes As List(Of PolyfaceMeshVertex), ByVal faces As List(Of PolyfaceMeshFace))
            MyBase.New(DxfObjectCode.Polyline)
            SyncLock _locker
                Me.m_flags = PolylineTypeFlags.PolyfaceMesh
                Me.m_vertexes = vertexes
                Me.m_faces = faces
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_endSequence = New EndSequence()
            End SyncLock
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <c>PolyfaceMesh</c> class.
        ''' </summary>
        Public Sub New()
            MyBase.New(DxfObjectCode.Polyline)
            SyncLock _locker
                Me.m_flags = PolylineTypeFlags.PolyfaceMesh
                Me.m_faces = New List(Of PolyfaceMeshFace)()
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_endSequence = New EndSequence()
            End SyncLock
        End Sub
         

        ''' <summary>
        ''' Gets or sets the polyface mesh <see cref="pccDXF4.Entities.PolyfaceMeshVertex">vertexes</see>.
        ''' </summary>
        Public Property Vertexes() As List(Of PolyfaceMeshVertex)
            Get
                SyncLock _locker
                    Return Me.m_vertexes
                End SyncLock
            End Get
            Set(ByVal value As List(Of PolyfaceMeshVertex))
                SyncLock _locker
                    If value Is Nothing Then
                        Throw New ArgumentNullException("value")
                    End If
                    Me.m_vertexes = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the polyface mesh <see cref="pccDXF4.Entities.PolyfaceMeshFace">faces</see>.
        ''' </summary>
        Public Property Faces() As List(Of PolyfaceMeshFace)
            Get
                SyncLock _locker
                    Return Me.m_faces
                End SyncLock
            End Get
            Set(ByVal value As List(Of PolyfaceMeshFace))
                SyncLock _locker
                    If value Is Nothing Then
                        Throw New ArgumentNullException("value")
                    End If
                    Me.m_faces = value
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
                entityNumber = Me.m_endSequence.AsignHandle(entityNumber)
                For Each v As PolyfaceMeshVertex In Me.m_vertexes
                    entityNumber = v.AsignHandle(entityNumber)
                Next
                For Each f As PolyfaceMeshFace In Me.m_faces
                    entityNumber = f.AsignHandle(entityNumber)
                Next
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
