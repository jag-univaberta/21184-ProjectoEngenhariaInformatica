

Imports System.Collections.Generic
Imports pccDXF4.Tables

Namespace Entities
    ''' <summary>
    ''' Represents a Text <see cref="IEntityObject">entity</see>.
    ''' </summary>
    Public Class Text
        Inherits DxfObject
        Implements IEntityObject


        Private Const m_TYPE As EntityType = EntityType.Text
        Private m_alignment As TextAlignment
        Private m_basePoint As Vector3f
        Private m_color As AciColor
        Private m_layer As Tables.Layer
        Private m_lineType As Tables.LineType
        Private m_normal As Vector3f
        Private m_obliqueAngle As Single
        Private m_style As Tables.TextStyle
        Private m_value As String
        Private m_height As Single
        Private m_widthFactor As Single
        Private m_rotation As Single
        Private m_xData As Dictionary(Of Tables.ApplicationRegistry, XData)

        Private Shared ReadOnly _locker As New Object()

        ''' <summary>
        ''' Initializes a new instance of the <c>Text</c> class.
        ''' </summary>
        Public Sub New()
            MyBase.New(DxfObjectCode.Text)
            SyncLock _locker
                Me.m_value = String.Empty
                Me.m_basePoint = Vector3f.Zero
                Me.m_alignment = TextAlignment.BaselineLeft
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_normal = Vector3f.UnitZ
                Me.m_style = TextStyle.[Default]
                Me.m_rotation = 0.0F
                Me.m_height = 0.0F
                Me.m_widthFactor = 1.0F
                Me.m_obliqueAngle = 0.0F
            End SyncLock
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <c>Text</c> class.
        ''' </summary>
        ''' <param name="text__1">Text string.</param>
        ''' <param name="basePoint">Text base <see cref="Vector3f">point</see>.</param>
        ''' <param name="height">Text height.</param>
        Public Sub New(ByVal text__1 As String, ByVal basePoint As Vector3f, ByVal height As Single)
            MyBase.New(DxfObjectCode.Text)
            SyncLock _locker
                Me.m_value = text__1
                Me.m_basePoint = basePoint
                Me.m_alignment = TextAlignment.BaselineLeft
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_normal = Vector3f.UnitZ
                Me.m_style = TextStyle.[Default]
                Me.m_rotation = 0.0F
                Me.m_height = height
                Me.m_widthFactor = 1.0F
                Me.m_obliqueAngle = 0.0F
            End SyncLock
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <c>Text</c> class.
        ''' </summary>
        ''' <param name="text__1">Text string.</param>
        ''' <param name="basePoint">Text base <see cref="Vector3f">point</see>.</param>
        ''' <param name="height">Text height.</param>
        ''' <param name="style">Text <see cref="TextStyle">style</see>.</param>
        Public Sub New(ByVal text__1 As String, ByVal basePoint As Vector3f, ByVal height As Single, ByVal style As TextStyle)
            MyBase.New(DxfObjectCode.Text)
            SyncLock _locker
                Me.m_value = text__1
                Me.m_basePoint = basePoint
                Me.m_alignment = TextAlignment.BaselineLeft
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_normal = Vector3f.UnitZ
                Me.m_style = style
                Me.m_height = height
                Me.m_widthFactor = style.WidthFactor
                Me.m_obliqueAngle = style.ObliqueAngle
                Me.m_rotation = 0.0F
            End SyncLock
        End Sub
         
        ''' <summary>
        ''' Gets or sets the text rotation.
        ''' </summary>
        Public Property Rotation() As Single
            Get
                SyncLock _locker
                    Return Me.m_rotation
                End SyncLock
            End Get
            Set(ByVal value As Single)
                SyncLock _locker
                    Me.m_rotation = m_value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the text height.
        ''' </summary>
        Public Property Height() As Single
            Get
                SyncLock _locker
                    Return Me.m_height
                End SyncLock
            End Get
            Set(ByVal value As Single)
                SyncLock _locker
                    If m_value < 0 Then
                        Throw New ArgumentOutOfRangeException("value", m_value.ToString())
                    End If
                    Me.m_height = m_value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the width factor.
        ''' </summary>
        Public Property WidthFactor() As Single
            Get
                SyncLock _locker
                    Return Me.m_widthFactor
                End SyncLock
            End Get
            Set(ByVal value As Single)
                SyncLock _locker
                    If m_value <= 0 Then
                        Throw New ArgumentOutOfRangeException("value", m_value.ToString())
                    End If
                    Me.m_widthFactor = m_value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the font oblique angle.
        ''' </summary>
        Public Property ObliqueAngle() As Single
            Get
                SyncLock _locker
                    Return Me.m_obliqueAngle
                End SyncLock
            End Get
            Set(ByVal value As Single)
                SyncLock _locker
                    Me.m_obliqueAngle = m_value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the <see cref="pccDXF4.Tables.TextStyle">text style</see>.
        ''' </summary>
        Public Property Style() As TextStyle
            Get
                SyncLock _locker
                    Return Me.m_style
                End SyncLock
            End Get
            Set(ByVal value As TextStyle)
                SyncLock _locker
                    Me.m_style = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the text base <see cref="pccDXF4.Vector3f">point</see>.
        ''' </summary>
        Public Property BasePoint() As Vector3f
            Get
                SyncLock _locker
                    Return Me.m_basePoint
                End SyncLock
            End Get
            Set(ByVal value As Vector3f)
                SyncLock _locker
                    Me.m_basePoint = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the text alignment.
        ''' </summary>
        Public Property Alignment() As TextAlignment
            Get
                SyncLock _locker
                    Return Me.m_alignment
                End SyncLock
            End Get
            Set(ByVal value As TextAlignment)
                SyncLock _locker
                    Me.m_alignment = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the text <see cref="pccDXF4.Vector3f">normal</see>.
        ''' </summary>
        Public Property Normal() As Vector3f
            Get
                SyncLock _locker
                    Return Me.m_normal
                End SyncLock
            End Get
            Set(ByVal value As Vector3f)
                SyncLock _locker
                    value.Normalize()
                    Me.m_normal = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the text string.
        ''' </summary>
        Public Property Value() As String
            Get
                SyncLock _locker
                    Return Me.m_value
                End SyncLock
            End Get
            Set(ByVal value As String)
                SyncLock _locker
                    Me.m_value = value
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
                    If m_value Is Nothing Then
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
                    If m_value Is Nothing Then
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
                    If m_value Is Nothing Then
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
