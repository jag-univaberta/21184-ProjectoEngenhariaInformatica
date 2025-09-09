Imports System.Collections.Generic
Imports pccDXF4.Tables

Namespace Entities
    '''<summary>Attribute flags.</summary>
    <Flags()> _
    Public Enum AttributeFlags
        ''' <summary>
        ''' Attribute is visible 
        ''' </summary>
        Visible = 0
        ''' <summary>
        ''' Attribute is invisible (does not appear)
        ''' </summary>
        Hidden = 1
        ''' <summary>
        ''' This is a constant attribute
        ''' </summary>
        Constant = 2
        ''' <summary>
        ''' Verification is required on input of this attribute
        ''' </summary>
        Verify = 4
        ''' <summary>
        ''' Attribute is preset (no prompt during insertion)
        ''' </summary>
        Predefined = 8
    End Enum

    ''' <summary>
    ''' Represents a attribute definition <see cref="pccDXF4.Entities.IEntityObject">entity</see>.
    ''' </summary>
    Public Class AttributeDefinition
        Inherits DxfObject
        Implements IEntityObject

        Private Const m_TYPE As EntityType = EntityType.AttributeDefinition
        Private ReadOnly m_id As String
        Private m_text As String
        Private m_value As Object
        Private m_style As TextStyle
        Private m_color As AciColor
        Private m_basePoint As Vector3f
        Private m_layer As Layer
        Private m_lineType As LineType
        Private m_flags As AttributeFlags
        Private m_alignment As TextAlignment
        Private m_height As Single
        Private m_widthFactor As Single
        Private m_rotation As Single
        Private m_normal As Vector3f
        Private m_xData As Dictionary(Of ApplicationRegistry, XData)

        Private Shared ReadOnly _locker As New Object()

        ''' <summary>
        ''' Intitializes a new instance of the <c>AttributeDefiniton</c> class.
        ''' </summary>
        ''' <param name="id">Attribute identifier, the parameter <c>id</c> string cannot contain spaces.</param>
        Public Sub New(ByVal id As String)
            MyBase.New(DxfObjectCode.AttributeDefinition)
            SyncLock _locker
                If id.Contains(" ") Then
                    Throw New ArgumentException("The id string cannot contain spaces", "id")
                End If
                Me.m_id = id
                Me.m_flags = AttributeFlags.Visible
                Me.m_text = String.Empty
                Me.m_value = Nothing
                Me.m_basePoint = Vector3f.Zero
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_style = TextStyle.[Default]
                Me.m_alignment = TextAlignment.BaselineLeft
                Me.m_height = If(Me.m_style.Height = 0, 1.0F, Me.m_style.Height)
                Me.m_widthFactor = Me.m_style.WidthFactor
                Me.m_rotation = 0.0F
                Me.m_normal = Vector3f.UnitZ
            End SyncLock
        End Sub

        ''' <summary>
        ''' Intitializes a new instance of the <c>AttributeDefiniton</c> class.
        ''' </summary>
        ''' <param name="id">Attribute identifier, the parameter <c>id</c> string cannot contain spaces.</param>
        ''' <param name="style">Attribute <see cref="pccDXF4.Tables.TextStyle">text style.</see></param>
        Public Sub New(ByVal id As String, ByVal style As TextStyle)
            MyBase.New(DxfObjectCode.AttributeDefinition)
            SyncLock _locker
                If id.Contains(" ") Then
                    Throw New ArgumentException("The id string cannot contain spaces", "id")
                End If
                Me.m_id = id
                Me.m_flags = AttributeFlags.Visible
                Me.m_text = String.Empty
                Me.m_value = Nothing
                Me.m_basePoint = Vector3f.Zero
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_style = style
                Me.m_alignment = TextAlignment.BaselineLeft
                Me.m_height = If(style.Height = 0, 1.0F, style.Height)
                Me.m_widthFactor = style.WidthFactor
                Me.m_rotation = 0.0F
                Me.m_normal = Vector3f.UnitZ
            End SyncLock
        End Sub

        ''' <summary>
        ''' Gets the attribute identifier.
        ''' </summary>
        Public ReadOnly Property Id() As String
            Get
                SyncLock _locker
                    Return Me.m_id
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the attribute information text.
        ''' </summary>
        Public Property Text() As String
            Get
                SyncLock _locker
                    Return Me.m_text
                End SyncLock
            End Get
            Set(ByVal value As String)
                SyncLock _locker
                    Me.m_text = m_value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the <see cref="pccDXF4.TextAlignment">text alignment.</see>
        ''' </summary>
        Public Property Alignment() As TextAlignment
            Get
                SyncLock _locker
                    Return Me.m_alignment
                End SyncLock
            End Get
            Set(ByVal value As TextAlignment)
                SyncLock _locker
                    Me.m_alignment = m_value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the attribute text height.
        ''' </summary>
        Public Property Height() As Single
            Get
                SyncLock _locker
                    Return Me.m_height
                End SyncLock
            End Get
            Set(ByVal value As Single)
                SyncLock _locker
                    If m_value <= 0 Then
                        Throw (New ArgumentOutOfRangeException("value", m_value, "The height should be greater than zero."))
                    End If
                    Me.m_height = m_value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the attribute text width factor.
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
                        Throw (New ArgumentOutOfRangeException("value", m_value, "The width factor should be greater than zero."))
                    End If
                    Me.m_widthFactor = m_value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the attribute text rotation in degrees.
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
        ''' Gets or sets the attribute default value.
        ''' </summary>
        Public Property Value() As Object
            Get
                SyncLock _locker
                    Return Me.m_value
                End SyncLock
            End Get
            Set(ByVal value As Object)
                SyncLock _locker
                    Me.m_value = m_value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets  the attribute text style.
        ''' </summary>
        ''' <remarks>
        ''' The <see cref="pccDXF4.Tables.TextStyle">text style</see> defines the basic properties of the information text.
        ''' </remarks>
        Public Property Style() As TextStyle
            Get
                SyncLock _locker
                    Return Me.m_style
                End SyncLock
            End Get
            Set(ByVal value As TextStyle)
                SyncLock _locker
                    If m_value Is Nothing Then
                        Throw New NullReferenceException("value")
                    End If
                    Me.m_style = m_value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the attribute <see cref="pccDXF4.Vector3f">insertion point</see>.
        ''' </summary>
        Public Property BasePoint() As Vector3f
            Get
                SyncLock _locker
                    Return Me.m_basePoint
                End SyncLock
            End Get
            Set(ByVal value As Vector3f)
                SyncLock _locker
                    Me.m_basePoint = m_value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the attribute flags.
        ''' </summary>
        Public Property Flags() As AttributeFlags
            Get
                SyncLock _locker
                    Return Me.m_flags
                End SyncLock
            End Get
            Set(ByVal value As AttributeFlags)
                SyncLock _locker
                    Me.m_flags = m_value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the attribute <see cref="pccDXF4.Vector3f">normal</see>.
        ''' </summary>
        Public Property Normal() As Vector3f
            Get
                SyncLock _locker
                    Return Me.m_normal
                End SyncLock
            End Get
            Set(ByVal value As Vector3f)
                SyncLock _locker
                    If Vector3f.Zero = m_value Then
                        Throw New ArgumentNullException("value", "The normal can not be the zero vector")
                    End If
                    m_value.Normalize()
                    Me.m_normal = m_value
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
                    Me.m_color = m_value
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
                    Me.m_layer = m_value
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
                    Me.m_lineType = m_value
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
                    Throw New ArgumentException("Extended data not avaliable for attribute definitions", "value")
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
