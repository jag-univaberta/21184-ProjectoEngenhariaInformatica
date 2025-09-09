

Imports System.Collections.Generic
Imports pccDXF4.Blocks
Imports pccDXF4.Tables

Namespace Entities

    ''' <summary>
    ''' Represents a block insertion <see cref="pccDXF4.Entities.IEntityObject">entity</see>.
    ''' </summary>
    Public Class Insert
        Inherits DxfObject
        Implements IEntityObject


        Private ReadOnly m_endSequence As EndSequence
        Private Const m_TYPE As EntityType = EntityType.Insert
        Private m_color As AciColor
        Private m_layer As Layer
        Private m_lineType As LineType
        Private ReadOnly m_block As Block
        Private m_insertionPoint As Vector3f
        Private m_scale As Vector3f
        Private m_rotation As Single
        Private m_normal As Vector3f
        Private ReadOnly m_attributes As List(Of Attribute)
        Private m_xData As Dictionary(Of ApplicationRegistry, XData)

        Private Shared ReadOnly _locker As New Object()


        ''' <summary>
        ''' Initializes a new instance of the <c>Insert</c> class.
        ''' </summary>
        ''' <param name="block">Insert block definition.</param>
        ''' <param name="insertionPoint">Insert <see cref="Vector3f">point</see>.</param>
        Public Sub New(ByVal block As Block, ByVal insertionPoint As Vector3f)
            MyBase.New(DxfObjectCode.Insert)
            SyncLock _locker
                If block Is Nothing Then
                    Throw New ArgumentNullException("block")
                End If

                Me.m_block = block
                Me.m_insertionPoint = insertionPoint
                Me.m_scale = New Vector3f(1.0F, 1.0F, 1.0F)
                Me.m_rotation = 0.0F
                Me.m_normal = Vector3f.UnitZ
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_attributes = New List(Of Attribute)()
                For Each attdef As AttributeDefinition In block.Attributes.Values
                    Me.m_attributes.Add(New Attribute(attdef))
                Next
                Me.m_endSequence = New EndSequence()
            End SyncLock
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <c>Insert</c> class.
        ''' </summary>
        ''' <param name="block">Insert <see cref="Blocks.Block">block definition</see>.</param>
        Public Sub New(ByVal block As Block)
            MyBase.New(DxfObjectCode.Insert)
            SyncLock _locker
                If block Is Nothing Then
                    Throw New ArgumentNullException("block")
                End If

                Me.m_block = block
                Me.m_insertionPoint = Vector3f.Zero
                Me.m_scale = New Vector3f(1.0F, 1.0F, 1.0F)
                Me.m_rotation = 0.0F
                Me.m_normal = Vector3f.UnitZ
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_attributes = New List(Of Attribute)()
                For Each attdef As AttributeDefinition In block.Attributes.Values
                    Me.m_attributes.Add(New Attribute(attdef))
                Next
                Me.m_endSequence = New EndSequence()
            End SyncLock
        End Sub


        ''' <summary>
        ''' Gets the insert list of <see cref="Attribute">attributes</see>.
        ''' </summary>
        Public ReadOnly Property Attributes() As List(Of Attribute)
            Get
                SyncLock _locker
                    Return Me.m_attributes
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Gets the insert <see cref="Blocks.Block">block definition</see>.
        ''' </summary>
        Public ReadOnly Property Block() As Block
            Get
                SyncLock _locker
                    Return Me.m_block
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the insert <see cref="Vector3f">point</see>.
        ''' </summary>
        Public Property InsertionPoint() As Vector3f
            Get
                SyncLock _locker
                    Return Me.m_insertionPoint
                End SyncLock
            End Get
            Set(ByVal value As Vector3f)
                SyncLock _locker
                    Me.m_insertionPoint = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the insert <see cref="Vector3f">scale</see>.
        ''' </summary>
        Public Property Scale() As Vector3f
            Get
                SyncLock _locker
                    Return Me.m_scale
                End SyncLock
            End Get
            Set(ByVal value As Vector3f)
                SyncLock _locker
                    Me.m_scale = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the insert rotation along the normal vector in degrees.
        ''' </summary>
        Public Property Rotation() As Single
            Get
                SyncLock _locker
                    Return Me.m_rotation
                End SyncLock
            End Get
            Set(ByVal value As Single)
                SyncLock _locker
                    Me.m_rotation = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the insert <see cref="Vector3f">normal</see>.
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

        Friend ReadOnly Property EndSequence() As EndSequence
            Get
                SyncLock _locker
                    Return Me.m_endSequence
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
                For Each attrib As Attribute In Me.m_attributes
                    entityNumber = attrib.AsignHandle(entityNumber)
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
