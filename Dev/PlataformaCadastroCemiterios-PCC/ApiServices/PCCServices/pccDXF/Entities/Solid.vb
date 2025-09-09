
Imports System.Collections.Generic
Imports pccDXF4.Tables

Namespace Entities
    ''' <summary>
    ''' Represents a solid <see cref="IEntityObject">entity</see>.
    ''' </summary>
    Public Class Solid
        Inherits DxfObject
        Implements IEntityObject

        Private Const m_TYPE As EntityType = EntityType.Solid
        Private m_firstVertex As Vector3f
        Private m_secondVertex As Vector3f
        Private m_thirdVertex As Vector3f
        Private m_fourthVertex As Vector3f
        Private m_thickness As Single
        Private m_normal As Vector3f
        Private m_layer As Layer
        Private m_color As AciColor
        Private m_lineType As LineType
        Private m_xData As Dictionary(Of ApplicationRegistry, XData)

        Private Shared ReadOnly _locker As New Object()

         
        ''' <summary>
        ''' Initializes a new instance of the <c>Solid</c> class.
        ''' </summary>
        ''' <param name="firstVertex">Solid <see cref="Vector3f">first vertex</see>.</param>
        ''' <param name="secondVertex">Solid <see cref="Vector3f">second vertex</see>.</param>
        ''' <param name="thirdVertex">Solid <see cref="Vector3f">third vertex</see>.</param>
        ''' <param name="fourthVertex">Solid <see cref="Vector3f">fourth vertex</see>.</param>
        Public Sub New(ByVal firstVertex As Vector3f, ByVal secondVertex As Vector3f, ByVal thirdVertex As Vector3f, ByVal fourthVertex As Vector3f)
            MyBase.New(DxfObjectCode.Solid)
            SyncLock _locker
                Me.m_firstVertex = firstVertex
                Me.m_secondVertex = secondVertex
                Me.m_thirdVertex = thirdVertex
                Me.m_fourthVertex = fourthVertex
                Me.m_thickness = 0.0F
                Me.m_normal = Vector3f.UnitZ
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
            End SyncLock
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <c>Solid</c> class.
        ''' </summary>
        Public Sub New()
            MyBase.New(DxfObjectCode.Solid)
            SyncLock _locker
                Me.m_firstVertex = Vector3f.Zero
                Me.m_secondVertex = Vector3f.Zero
                Me.m_thirdVertex = Vector3f.Zero
                Me.m_fourthVertex = Vector3f.Zero
                Me.m_thickness = 0.0F
                Me.m_normal = Vector3f.UnitZ
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
            End SyncLock
        End Sub

         

        ''' <summary>
        ''' Gets or sets the first solid <see cref="pccDXF4.Vector3f">vertex</see>.
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
        ''' Gets or sets the second solid <see cref="pccDXF4.Vector3f">vertex</see>.
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
        ''' Gets or sets the third solid <see cref="pccDXF4.Vector3f">vertex</see>.
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
        ''' Gets or sets the fourth solid <see cref="pccDXF4.Vector3f">vertex</see>.
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
        ''' Gets or sets the thickness of the solid.
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
        ''' Gets or sets the solid <see cref="pccDXF4.Vector3f">normal</see>.
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
