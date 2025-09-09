Imports pccDXF4.Tables

Namespace Objects
    Friend Class Dictionary
        Inherits DxfObject
        Implements ITableObject

        Private ReadOnly m_name As String

        Private Shared ReadOnly _locker As New Object()

        Friend Shared ReadOnly Property [Default]() As Dictionary
            Get
                SyncLock _locker
                    Return New Dictionary("ACAD_GROUP")
                End SyncLock
            End Get
        End Property


        Public Sub New(ByVal name As String)
            MyBase.New(DxfObjectCode.Dictionary)
            SyncLock _locker
                Me.m_name = name
            End SyncLock
        End Sub

        Public ReadOnly Property Name() As String Implements ITableObject.Name
            Get
                SyncLock _locker
                    Return Me.m_name
                End SyncLock
            End Get
        End Property


        Public Overrides Function ToString() As String
            SyncLock _locker
                Return Me.m_name
            End SyncLock
        End Function

    End Class
End Namespace
