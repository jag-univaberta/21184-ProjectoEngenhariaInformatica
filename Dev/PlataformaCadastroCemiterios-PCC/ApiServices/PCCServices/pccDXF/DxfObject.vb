

''' <summary>
''' Represents the base class for all dxf objects.
''' </summary>
Public Class DxfObject


    Private ReadOnly m_codeName As String
    Private m_handle As String

    Private Shared ReadOnly _locker As New Object()

    ''' <summary>
    ''' Initializes a new instance of the <c>DxfObject</c> class.
    ''' </summary>
    Public Sub New(ByVal codeName As String)
        SyncLock _locker
            Me.m_codeName = codeName
        End SyncLock
    End Sub

    ''' <summary>
    ''' Gets the dxf entity type string.
    ''' </summary>
    Public ReadOnly Property CodeName() As String
        Get
            SyncLock _locker
                Return Me.m_codeName
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets the handle asigned of the dxf object.
    ''' </summary>
    ''' <remarks>Only the getter is public.</remarks>
    Public Property Handle() As String
        Get
            SyncLock _locker
                Return Me.m_handle
            End SyncLock
        End Get
        Friend Set(ByVal value As String)
            SyncLock _locker
                Me.m_handle = value
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
    Friend Overridable Function AsignHandle(ByVal entityNumber As Integer) As Integer
        SyncLock _locker
            Me.m_handle = Convert.ToString(entityNumber, 16)
            Return entityNumber + 1
        End SyncLock
    End Function


End Class 