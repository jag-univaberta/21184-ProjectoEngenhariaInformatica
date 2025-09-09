
Imports System.Collections.Generic
Imports pccDXF4.Tables

Namespace Entities
    ''' <summary>
    ''' Represents a 3d polyline <see cref="IEntityObject">entity</see>.
    ''' </summary>
    Public Class Polyline3d
        Inherits DxfObject
        Implements IPolyline


        Private ReadOnly m_endSequence As EndSequence
        Protected Const m_TYPE As EntityType = EntityType.Polyline3d
        Protected m_vertexes As List(Of Polyline3dVertex)
        Protected m_flags As PolylineTypeFlags
        Protected m_layer As Layer
        Protected m_color As AciColor
        Protected m_lineType As LineType
        Protected m_xData As Dictionary(Of ApplicationRegistry, XData)

        Private Shared ReadOnly _locker As New Object()

         
        ''' <summary>
        ''' Initializes a new instance of the <c>Polyline3d</c> class.
        ''' </summary>
        ''' <param name="vertexes">3d polyline <see cref="Polyline3dVertex">vertex</see> list.</param>
        ''' <param name="isClosed">Sets if the polyline is closed</param>
        Public Sub New(ByVal vertexes As List(Of Polyline3dVertex), ByVal isClosed As Boolean)
            MyBase.New(DxfObjectCode.Polyline)
            SyncLock _locker
                Me.m_flags = If(isClosed, PolylineTypeFlags.ClosedPolylineOrClosedPolygonMeshInM Or PolylineTypeFlags.Polyline3D, PolylineTypeFlags.Polyline3D)
                Me.m_vertexes = vertexes
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_endSequence = New EndSequence()
            End SyncLock
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <c>Polyline3d</c> class.
        ''' </summary>
        ''' <param name="vertexes">3d polyline <see cref="Polyline3dVertex">vertex</see> list.</param>
        Public Sub New(ByVal vertexes As List(Of Polyline3dVertex))
            MyBase.New(DxfObjectCode.Polyline)
            SyncLock _locker
                Me.m_flags = PolylineTypeFlags.Polyline3D
                Me.m_vertexes = vertexes
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_endSequence = New EndSequence()
            End SyncLock
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <c>Polyline3d</c> class.
        ''' </summary>
        Public Sub New()
            MyBase.New(DxfObjectCode.Polyline)
            SyncLock _locker
                Me.m_flags = PolylineTypeFlags.Polyline3D
                Me.m_vertexes = New List(Of Polyline3dVertex)()
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_endSequence = New EndSequence()
            End SyncLock
        End Sub

         
        ''' <summary>
        ''' Gets or sets the polyline <see cref="pccDXF4.Entities.Polyline3dVertex">vertex</see> list.
        ''' </summary>
        Public Property Vertexes() As List(Of Polyline3dVertex)
            Get
                SyncLock _locker
                    Return Me.m_vertexes
                End SyncLock
            End Get
            Set(ByVal value As List(Of Polyline3dVertex))
                SyncLock _locker
                    If value Is Nothing Then
                        Throw New ArgumentNullException("value")
                    End If
                    Me.m_vertexes = value
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
                For Each v As Polyline3dVertex In Me.m_vertexes
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
