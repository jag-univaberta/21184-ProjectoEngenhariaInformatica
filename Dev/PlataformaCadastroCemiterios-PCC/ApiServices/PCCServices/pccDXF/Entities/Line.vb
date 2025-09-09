
Imports System.Collections.Generic
Imports pccDXF4.Tables

Namespace Entities
    ''' <summary>
    ''' Represents a line <see cref="pccDXF4.Entities.IEntityObject">entity</see>.
    ''' </summary>
    Public Class Line
        Inherits DxfObject
        Implements IEntityObject


        Private Const m_TYPE As EntityType = EntityType.Line
        Private m_startPoint As Vector3f
        Private m_endPoint As Vector3f
        Private m_thickness As Single
        Private m_color As AciColor
        Private m_layer As Layer
        Private m_lineType As LineType
        Private m_normal As Vector3f
        Private m_xData As Dictionary(Of ApplicationRegistry, XData)

        Private Shared ReadOnly _locker As New Object()


        ''' <summary>
        ''' Initializes a new instance of the <c>Line</c> class.
        ''' </summary>
        ''' <param name="startPoint">Line <see cref="Vector3f">start point.</see></param>
        ''' <param name="endPoint">Line <see cref="Vector3f">end point.</see></param>
        Public Sub New(ByVal startPoint As Vector3f, ByVal endPoint As Vector3f)
            MyBase.New(DxfObjectCode.Line)
            SyncLock _locker
                Me.m_startPoint = startPoint
                Me.m_endPoint = endPoint
                Me.m_thickness = 0.0F
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_normal = Vector3f.UnitZ
            End SyncLock
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <c>Line</c> class.
        ''' </summary>
        Public Sub New()
            MyBase.New(DxfObjectCode.Line)
            SyncLock _locker
                Me.m_startPoint = Vector3f.Zero
                Me.m_endPoint = Vector3f.Zero
                Me.m_thickness = 0.0F
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_normal = Vector3f.UnitZ
            End SyncLock
        End Sub





        ''' <summary>
        ''' Gets or sets the line <see cref="pccDXF4.Vector3f">start point</see>.
        ''' </summary>
        Public Property StartPoint() As Vector3f
            Get
                SyncLock _locker
                    Return Me.m_startPoint
                End SyncLock
            End Get
            Set(ByVal value As Vector3f)
                SyncLock _locker
                    Me.m_startPoint = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the line <see cref="pccDXF4.Vector3f">end point</see>.
        ''' </summary>
        Public Property EndPoint() As Vector3f
            Get
                SyncLock _locker
                    Return Me.m_endPoint
                End SyncLock
            End Get
            Set(ByVal value As Vector3f)
                SyncLock _locker
                    Me.m_endPoint = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the line thickness.
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
        ''' Gets or sets the line <see cref="pccDXF4.Vector3f">normal</see>.
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
