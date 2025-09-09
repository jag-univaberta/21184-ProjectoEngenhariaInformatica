
Imports System.Collections.Generic
Imports pccDXF4.Tables

Namespace Entities
    ''' <summary>
    ''' Defines which edges are hidden.
    ''' </summary>
    <Flags()> _
    Public Enum EdgeFlags
        ''' <summary>
        ''' All edges as visibles (default).
        ''' </summary>
        Visibles = 0
        ''' <summary>
        ''' First edge is invisible.
        ''' </summary>
        First = 1
        ''' <summary>
        ''' Second edge is invisible.
        ''' </summary>
        Second = 2
        ''' <summary>
        ''' Third edge is invisible.
        ''' </summary>
        Third = 4
        ''' <summary>
        ''' Fourth edge is invisible.
        ''' </summary>
        Fourth = 8
    End Enum

    ''' <summary>
    ''' Represents a 3DFace <see cref="pccDXF4.Entities.IEntityObject">entity</see>.
    ''' </summary>
    Public Class Face3d
        Inherits DxfObject
        Implements IEntityObject


        Private Const m_TYPE As EntityType = EntityType.Face3D
        Private m_firstVertex As Vector3f
        Private m_secondVertex As Vector3f
        Private m_thirdVertex As Vector3f
        Private m_fourthVertex As Vector3f
        Private m_edgeFlags As EdgeFlags
        Private m_layer As Layer
        Private m_color As AciColor
        Private m_lineType As LineType
        Private m_xData As Dictionary(Of ApplicationRegistry, XData)

        Private Shared ReadOnly _locker As New Object()


        ''' <summary>
        ''' Initializes a new instance of the <c>Face3D</c> class.
        ''' </summary>
        ''' <param name="firstVertex">3d face <see cref="Vector3f">first vertex</see>.</param>
        ''' <param name="secondVertex">3d face <see cref="Vector3f">second vertex</see>.</param>
        ''' <param name="thirdVertex">3d face <see cref="Vector3f">third vertex</see>.</param>
        ''' <param name="fourthVertex">3d face <see cref="Vector3f">fourth vertex</see>.</param>
        Public Sub New(ByVal firstVertex As Vector3f, ByVal secondVertex As Vector3f, ByVal thirdVertex As Vector3f, ByVal fourthVertex As Vector3f)
            MyBase.New(DxfObjectCode.Face3D)
            SyncLock _locker
                Me.m_firstVertex = firstVertex
                Me.m_secondVertex = secondVertex
                Me.m_thirdVertex = thirdVertex
                Me.m_fourthVertex = fourthVertex
                Me.m_edgeFlags = EdgeFlags.Visibles
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_xData = New Dictionary(Of ApplicationRegistry, XData)()
            End SyncLock
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <c>Face3D</c> class.
        ''' </summary>
        Public Sub New()
            MyBase.New(DxfObjectCode.Face3D)
            SyncLock _locker
                Me.m_firstVertex = Vector3f.Zero
                Me.m_secondVertex = Vector3f.Zero
                Me.m_thirdVertex = Vector3f.Zero
                Me.m_fourthVertex = Vector3f.Zero
                Me.m_edgeFlags = EdgeFlags.Visibles
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_xData = New Dictionary(Of ApplicationRegistry, XData)()
            End SyncLock
        End Sub

         
        ''' <summary>
        ''' Gets or sets the first 3d face <see cref="pccDXF4.Vector3f">vertex</see>.
        ''' </summary>
        Public Property FirstVertex() As Vector3f
            Get
                SyncLock _locker
                    Return Me.m_firstVertex
                End SyncLock
            End Get
            Set(ByVal value As Vector3f)
                SyncLock _locker
                    Me.m_firstVertex = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the second 3d face <see cref="pccDXF4.Vector3f">vertex</see>.
        ''' </summary>
        Public Property SecondVertex() As Vector3f
            Get
                SyncLock _locker
                    Return Me.m_secondVertex
                End SyncLock
            End Get
            Set(ByVal value As Vector3f)
                SyncLock _locker
                    Me.m_secondVertex = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the third 3d face <see cref="pccDXF4.Vector3f">vertex</see>.
        ''' </summary>
        Public Property ThirdVertex() As Vector3f
            Get
                SyncLock _locker
                    Return Me.m_thirdVertex
                End SyncLock
            End Get
            Set(ByVal value As Vector3f)
                SyncLock _locker
                    Me.m_thirdVertex = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the fourth 3d face <see cref="pccDXF4.Vector3f">vertex</see>.
        ''' </summary>
        Public Property FourthVertex() As Vector3f
            Get
                SyncLock _locker
                    Return Me.m_fourthVertex
                End SyncLock
            End Get
            Set(ByVal value As Vector3f)
                SyncLock _locker
                    Me.m_fourthVertex = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or set the 3d face edge visibility.
        ''' </summary>
        Public Property EdgeFlags() As EdgeFlags
            Get
                SyncLock _locker
                    Return Me.m_edgeFlags
                End SyncLock
            End Get
            Set(ByVal value As EdgeFlags)
                SyncLock _locker
                    Me.m_edgeFlags = value
                End SyncLock
            End Set
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
