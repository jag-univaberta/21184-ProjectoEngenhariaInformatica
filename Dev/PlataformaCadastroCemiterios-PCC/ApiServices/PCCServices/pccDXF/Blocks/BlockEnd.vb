Imports pccDXF4.Tables

Namespace Blocks

    Friend Class BlockEnd
        Inherits DxfObject
        Private m_layer As Layer

        Private Shared ReadOnly _locker As New Object()

        Public Sub New(ByVal layer As Layer)
            MyBase.New(DxfObjectCode.BlockEnd)
            SyncLock _locker
                Me.m_layer = layer
            End SyncLock
        End Sub


        Public Property Layer() As Layer
            Get
                SyncLock _locker
                    Return Me.m_layer
                End SyncLock
            End Get
            Set(ByVal value As Layer)
                SyncLock _locker
                    If value IsNot Nothing Then
                        Me.m_layer = value
                    End If
                End SyncLock
            End Set
        End Property
    End Class
End Namespace