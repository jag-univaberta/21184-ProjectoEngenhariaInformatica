
Namespace Tables
    Friend Class DimensionStyle
        Inherits DxfObject
        Implements ITableObject


        Private ReadOnly m_name As String

        Private Shared ReadOnly _locker As New Object()

         
        Friend Shared ReadOnly Property [Default]() As DimensionStyle
            Get
                SyncLock _locker
                    Return New DimensionStyle("Standard")
                End SyncLock
            End Get
        End Property
         

        ''' <summary>
        ''' Initializes a new instance of the <c>DimensionStyle</c> class.
        ''' </summary>
        Public Sub New(ByVal name As String)
            MyBase.New(DxfObjectCode.DimStyle)
            SyncLock _locker
                Me.m_name = name
            End SyncLock
        End Sub
         

        ''' <summary>
        ''' Gets the table name.
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
