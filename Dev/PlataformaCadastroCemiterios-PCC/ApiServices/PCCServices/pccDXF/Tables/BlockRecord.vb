

Namespace Tables
    ''' <summary>
    ''' Represent the record of a block in the tables section.
    ''' </summary>
    Friend Class BlockRecord
        Inherits DxfObject
        Implements ITableObject


        Private ReadOnly m_name As String

        Private Shared ReadOnly _locker As New Object()


        ''' <summary>
        ''' Initializes a new instance of the <c>BlockRecord</c> class.
        ''' </summary>
        ''' <param name="name">Block definition name.</param>
        Public Sub New(ByVal name As String)
            MyBase.New(DxfObjectCode.BlockRecord)
            SyncLock _locker
                If String.IsNullOrEmpty(name) Then
                    Throw (New ArgumentNullException("name"))
                End If
                Me.m_name = name
            End SyncLock
        End Sub

        ''' <summary>
        ''' Gets the block record name.
        ''' </summary>
        Public ReadOnly Property Name() As String Implements ITableObject.Name
            Get
                SyncLock _locker
                    Return Me.m_name
                End SyncLock
            End Get
        End Property

         
        ''' <summary>
        ''' Converts the value of this instance to its equivalent string representation.
        ''' </summary>
        ''' <returns>The string representation.</returns>
        Public Overrides Function ToString() As String
            SyncLock _locker
                Return Me.m_name
            End SyncLock
        End Function


    End Class
End Namespace
