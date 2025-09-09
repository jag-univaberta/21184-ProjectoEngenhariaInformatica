Imports System.Collections.Generic
Imports pccDXF4.Tables

Namespace Entities

    Public Class Attribute
        Inherits DxfObject
        Implements IEntityObject

        Private Const m_TYPE As EntityType = EntityType.Attribute
        Private ReadOnly m_definition As AttributeDefinition
        Private m_value As Object
        Private m_color As AciColor
        Private m_layer As Layer
        Private m_lineType As LineType
        Private m_xData As Dictionary(Of ApplicationRegistry, XData)

        Private Shared ReadOnly _locker As New Object()


        Public Sub New(ByVal definition As AttributeDefinition)
            MyBase.New(DxfObjectCode.Attribute)
            SyncLock _locker
                Me.m_definition = definition
                Me.m_value = Nothing
                Me.m_color = definition.Color
                Me.m_layer = definition.Layer
                Me.m_lineType = definition.LineType
            End SyncLock
        End Sub

        Public Sub New(ByVal definition As AttributeDefinition, ByVal value As Object)
            MyBase.New(DxfObjectCode.Attribute)
            SyncLock _locker
                Me.m_definition = definition
                Me.m_value = value
                Me.m_color = definition.Color
                Me.m_layer = definition.Layer
                Me.m_lineType = definition.LineType
            End SyncLock
        End Sub


        Public ReadOnly Property Definition() As AttributeDefinition
            Get
                SyncLock _locker
                    Return Me.m_definition
                End SyncLock
            End Get
        End Property


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


        Public ReadOnly Property Type() As EntityType Implements IEntityObject.Type
            Get
                SyncLock _locker
                    Return m_TYPE
                End SyncLock
            End Get
        End Property

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


        Public Property XData() As Dictionary(Of ApplicationRegistry, XData) Implements IEntityObject.XData
            Get
                SyncLock _locker
                    Return Me.m_xData
                End SyncLock
            End Get
            Set(ByVal value As Dictionary(Of ApplicationRegistry, XData))
                SyncLock _locker
                    Throw New ArgumentException("Extended data not avaliable for attributes", "value")
                End SyncLock
            End Set
        End Property

        Public Overrides Function ToString() As String
            SyncLock _locker
                Return m_TYPE.ToString()
            End SyncLock
        End Function

    End Class
End Namespace
