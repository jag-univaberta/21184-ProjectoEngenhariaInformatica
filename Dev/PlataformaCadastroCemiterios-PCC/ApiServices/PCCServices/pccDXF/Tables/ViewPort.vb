Namespace Tables
    Friend Class ViewPort
        Inherits DxfObject
        Implements ITableObject


        Private ReadOnly m_name As String
        Private m_lowerLeftCorner As Vector2f = Vector2f.Zero
        Private m_upperRightCorner As New Vector2f(1, 1)
        Private m_snapBasePoint As Vector2f = Vector2f.Zero
        Private m_snapSpacing As New Vector2f(0.5F, 0.5F)
        Private m_gridSpacing As New Vector2f(10, 10)
        Private m_target As Vector3f = Vector3f.Zero
        Private m_camera As Vector3f = Vector3f.UnitZ

        Private Shared ReadOnly _locker As New Object()


        Friend Shared ReadOnly Property Active() As ViewPort
            Get
                SyncLock _locker
                    Return New ViewPort("*Active")
                End SyncLock
            End Get
        End Property
         
        ''' <summary>
        ''' Initializes a new instance of the <c>ViewPort</c> class.
        ''' </summary>
        Public Sub New(ByVal name As String)
            MyBase.New(DxfObjectCode.ViewPort)
            SyncLock _locker
                Me.m_name = name
            End SyncLock
        End Sub
         

        Public Property LowerLeftCorner() As Vector2f
            Get
                SyncLock _locker
                    Return Me.m_lowerLeftCorner
                End SyncLock
            End Get
            Set(ByVal value As Vector2f)
                SyncLock _locker
                    Me.m_lowerLeftCorner = value
                End SyncLock
            End Set
        End Property

        Public Property UpperRightCorner() As Vector2f
            Get
                SyncLock _locker
                    Return Me.m_upperRightCorner
                End SyncLock
            End Get
            Set(ByVal value As Vector2f)
                SyncLock _locker
                    Me.m_upperRightCorner = value
                End SyncLock
            End Set
        End Property

        Public Property SnapBasePoint() As Vector2f
            Get
                SyncLock _locker
                    Return Me.m_snapBasePoint
                End SyncLock
            End Get
            Set(ByVal value As Vector2f)
                SyncLock _locker
                    Me.m_snapBasePoint = value
                End SyncLock
            End Set
        End Property

        Public Property SnapSpacing() As Vector2f
            Get
                SyncLock _locker
                    Return Me.m_snapSpacing
                End SyncLock
            End Get
            Set(ByVal value As Vector2f)
                SyncLock _locker
                    Me.m_snapSpacing = value
                End SyncLock
            End Set
        End Property

        Public Property GridSpacing() As Vector2f
            Get
                SyncLock _locker
                    Return Me.m_gridSpacing
                End SyncLock
            End Get
            Set(ByVal value As Vector2f)
                SyncLock _locker
                    Me.m_gridSpacing = value
                End SyncLock
            End Set
        End Property

        Public Property Target() As Vector3f
            Get
                SyncLock _locker
                    Return Me.m_target
                End SyncLock
            End Get
            Set(ByVal value As Vector3f)
                SyncLock _locker
                    Me.m_target = value
                End SyncLock
            End Set
        End Property

        Public Property Camera() As Vector3f
            Get
                SyncLock _locker
                    Return Me.m_camera
                End SyncLock
            End Get
            Set(ByVal value As Vector3f)
                SyncLock _locker
                    Me.m_camera = value
                End SyncLock
            End Set
        End Property
         

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
