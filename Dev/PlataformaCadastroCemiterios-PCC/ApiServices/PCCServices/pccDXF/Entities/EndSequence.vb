Imports pccDXF4.Tables

Namespace Entities

    ''' <summary>
    ''' Represents the terminator element of a vertex sequence in polylines or attributes in a block reference.
    ''' </summary>
    Friend Class EndSequence
        Inherits DxfObject
        Private m_layer As Layer

        Private Shared ReadOnly _locker As New Object()

        ''' <summary>
        ''' Initializes a new instance of the <c>EndSequence</c> class.
        ''' </summary>
        Public Sub New()
            MyBase.New(DxfObjectCode.EndSequence)
            SyncLock _locker
                Me.m_layer = Layer.[Default]
            End SyncLock
        End Sub

        ''' <summary>
        ''' Gets or sets the end sequence <see cref="pccDXF4.Tables.Layer">layer</see>
        ''' </summary>
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
