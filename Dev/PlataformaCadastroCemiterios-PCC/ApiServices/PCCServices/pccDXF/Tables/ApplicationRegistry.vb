Namespace Tables

    ''' <summary>
    ''' Represents a registered application name to which the <see cref="pccDXF4.XData">extended data</see> is associated.
    ''' </summary>
    Public Class ApplicationRegistry
        Inherits DxfObject
        Implements ITableObject

        Private ReadOnly m_name As String

        Private Shared ReadOnly _locker As New Object()

        ''' <summary>
        ''' Initializes a new instance of the <c>ApplicationRegistry</c> class.
        ''' </summary>
        ''' <param name="name">Layer name.</param>
        Public Sub New(ByVal name As String)
            MyBase.New(DxfObjectCode.AppId)
            SyncLock _locker
                If String.IsNullOrEmpty(name) Then
                    Throw (New ArgumentNullException("name"))
                End If
                Me.m_name = name
            End SyncLock
        End Sub

        ''' <summary>
        ''' Gets the default application registry.
        ''' </summary>
        Friend Shared ReadOnly Property [Default]() As ApplicationRegistry
            Get
                SyncLock _locker
                    Return New ApplicationRegistry("ACAD")
                End SyncLock
            End Get
        End Property


        ''' <summary>
        ''' Gets the application registry name.
        ''' </summary>
        Public ReadOnly Property Name() As String Implements ITableObject.Name
            Get
                SyncLock _locker
                    Return Me.m_name
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Determines whether the specified <see cref="pccDXF4.Tables.ApplicationRegistry" /> is equal to the current <see cref="pccDXF4.Tables.ApplicationRegistry" />.
        ''' </summary>
        ''' <returns>
        ''' True if the specified <see cref="pccDXF4.Tables.ApplicationRegistry" /> is equal to the current <see cref="pccDXF4.Tables.ApplicationRegistry" />; otherwise, false.
        ''' </returns>
        ''' <remarks>Two <see cref="pccDXF4.Tables.ApplicationRegistry" /> instances are equal if their names are equal.</remarks>
        ''' <exception cref="T:System.NullReferenceException">
        ''' The <paramref name="obj" /> parameter is null.
        ''' </exception>
        Public Overloads Function Equals(ByVal obj As ApplicationRegistry) As Boolean
            SyncLock _locker
                Return obj.Name = Me.m_name
            End SyncLock
        End Function

        ''' <summary>
        ''' Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.
        ''' </summary>
        ''' <returns>
        ''' True if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.
        ''' </returns>
        ''' <param name="obj"> The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />.</param>
        ''' <exception cref="T:System.NullReferenceException">
        ''' The <paramref name="obj" /> parameter is null.
        ''' </exception>
        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            SyncLock _locker
                If TypeOf obj Is ApplicationRegistry Then
                    Return Me.Equals(DirectCast(obj, ApplicationRegistry))
                End If
                Return False
            End SyncLock
        End Function

        '''<summary>
        ''' Serves as a hash function for a particular type. 
        '''</summary>
        '''<returns>
        ''' A hash code for the current <see cref="T:System.Object" />.
        '''</returns>
        Public Overrides Function GetHashCode() As Integer
            SyncLock _locker
                Return Me.m_name.GetHashCode()
            End SyncLock
        End Function

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
