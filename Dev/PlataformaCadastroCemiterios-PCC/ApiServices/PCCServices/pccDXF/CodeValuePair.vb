


''' <summary>
''' Represents the minimun information element in a dxf file.
''' </summary>
Friend Structure CodeValuePair
    Private ReadOnly m_code As Integer
    Private ReadOnly m_value As String

    Private Shared ReadOnly _locker As New Object()

    ''' <summary>
    ''' Initalizes a new instance of the <c>CodeValuePair</c> class.
    ''' </summary>
    ''' <param name="code">Dxf code.</param>
    ''' <param name="value">Value for the specified code.</param>
    Public Sub New(ByVal code As Integer, ByVal value As String)
        SyncLock _locker
            Me.m_code = code
            Me.m_value = value
        End SyncLock
    End Sub

    ''' <summary>
    ''' Gets the dxf code.
    ''' </summary>
    Public ReadOnly Property Code() As Integer
        Get
            SyncLock _locker
                Return Me.m_code
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Gets the value.
    ''' </summary>
    Public ReadOnly Property Value() As String
        Get
            SyncLock _locker
                Return Me.m_value
            End SyncLock
        End Get
    End Property
End Structure

