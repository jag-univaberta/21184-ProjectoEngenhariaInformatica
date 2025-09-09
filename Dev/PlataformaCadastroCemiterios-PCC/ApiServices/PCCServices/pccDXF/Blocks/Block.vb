Imports System
Imports System.Collections.Generic
Imports pccDXF4.Entities
Imports pccDXF4.Tables

Namespace Blocks
    Public Class Block
        Inherits DxfObject

        Private ReadOnly m_record As BlockRecord
        Private ReadOnly m_end As BlockEnd
        Private ReadOnly m_name As String
        Private m_layer As Layer
        Private m_basePoint As Vector3f
        Private m_attributes As Dictionary(Of String, AttributeDefinition)
        Private m_entities As List(Of IEntityObject)

        Private Shared ReadOnly _locker As New Object()

        Friend Shared ReadOnly Property ModelSpace() As Block
            Get
                SyncLock _locker
                    Return New Block("*Model_Space")
                End SyncLock
            End Get
        End Property

        Friend Shared ReadOnly Property PaperSpace() As Block
            Get
                SyncLock _locker
                    Return New Block("*Paper_Space")
                End SyncLock
            End Get
        End Property

        Public Sub New(ByVal name As String)
            MyBase.New(DxfObjectCode.Block)
            SyncLock _locker
                If String.IsNullOrEmpty(name) Then
                    Throw (New ArgumentNullException("name"))
                End If

                Me.m_name = name
                Me.m_basePoint = Vector3f.Zero
                Me.m_layer = Layer.[Default]
                Me.m_attributes = New Dictionary(Of String, AttributeDefinition)()
                Me.m_entities = New List(Of IEntityObject)()
                Me.m_record = New BlockRecord(name)
                Me.m_end = New BlockEnd(Me.m_layer)
            End SyncLock
        End Sub

        Public ReadOnly Property Name() As String
            Get
                SyncLock _locker
                    Return Me.m_name
                End SyncLock
            End Get
        End Property

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

        Public Property Layer() As Layer
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
                    Me.m_end.Layer = value
                End SyncLock
            End Set
        End Property

        Public Property Attributes() As Dictionary(Of String, AttributeDefinition)
            Get
                SyncLock _locker
                    Return Me.m_attributes
                End SyncLock
            End Get
            Set(ByVal value As Dictionary(Of String, AttributeDefinition))
                SyncLock _locker
                    If value Is Nothing Then
                        Throw New NullReferenceException("value")
                    End If
                    Me.m_attributes = value
                End SyncLock
            End Set
        End Property

        Public Property Entities() As List(Of IEntityObject)
            Get
                SyncLock _locker
                    Return Me.m_entities
                End SyncLock
            End Get
            Set(ByVal value As List(Of IEntityObject))
                SyncLock _locker
                    If value Is Nothing Then
                        Throw New NullReferenceException("value")
                    End If
                    Me.m_entities = value
                End SyncLock
            End Set
        End Property

        Friend ReadOnly Property Record() As BlockRecord
            Get
                SyncLock _locker
                    Return Me.m_record
                End SyncLock
            End Get
        End Property

        Friend ReadOnly Property [End]() As BlockEnd
            Get
                SyncLock _locker
                    Return Me.m_end
                End SyncLock
            End Get
        End Property

        Friend Overrides Function AsignHandle(ByVal entityNumber As Integer) As Integer
            SyncLock _locker
                entityNumber = Me.m_record.AsignHandle(entityNumber)
                entityNumber = Me.m_end.AsignHandle(entityNumber)
                For Each attDef As AttributeDefinition In Me.m_attributes.Values
                    entityNumber = attDef.AsignHandle(entityNumber)
                Next
                For Each entity As IEntityObject In Me.m_entities
                    entityNumber = DirectCast(entity, DxfObject).AsignHandle(entityNumber)
                Next
                Return MyBase.AsignHandle(entityNumber)
            End SyncLock
        End Function

        Public Overrides Function ToString() As String
            SyncLock _locker
                Return Me.m_name
            End SyncLock
        End Function

    End Class

End Namespace
