

Namespace Entities
    ''' <summary>
    ''' Represents a nurbs curve vertex.
    ''' </summary>
    Public Class NurbsVertex


        Private m_location As Vector2f
        Private m_weight As Single


        Private Shared ReadOnly _locker As New Object()



        ''' <summary>
        ''' Initializes a new instance of the <c>NurbsVertex</c> class.
        ''' </summary>
        Public Sub New()
            SyncLock _locker
                Me.m_location = Vector2f.Zero
                Me.m_weight = 1
            End SyncLock
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <c>NurbsVertex</c> class.
        ''' </summary>
        ''' <param name="x">X coordinate.</param>
        ''' <param name="y">Y coordinate.</param>
        Public Sub New(ByVal x As Single, ByVal y As Single)
            SyncLock _locker
                Me.m_location = New Vector2f(x, y)
                Me.m_weight = 1
            End SyncLock
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <c>NurbsVertex</c> class.
        ''' </summary>
        ''' <param name="x">X coordinate.</param>
        ''' <param name="y">Y coordinate.</param>
        ''' <param name="weight">Nurbs vertex weight.</param>
        Public Sub New(ByVal x As Single, ByVal y As Single, ByVal weight As Single)
            SyncLock _locker
                Me.m_location = New Vector2f(x, y)
                Me.m_weight = weight
            End SyncLock
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <c>NurbsVertex</c> class.
        ''' </summary>
        ''' <param name="location">Nurbs vertex <see cref="Vector2f">location</see>.
        Public Sub New(ByVal location As Vector2f)
            SyncLock _locker
                Me.m_location = location
                Me.m_weight = 1
            End SyncLock
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <c>NurbsVertex</c> class.
        ''' </summary>
        ''' <param name="location">Nurbs vertex <see cref="Vector2f">location</see>.
        ''' <param name="weight">Nurbs vertex weight.</param>
        Public Sub New(ByVal location As Vector2f, ByVal weight As Single)
            SyncLock _locker
                Me.m_location = location
                Me.m_weight = weight
            End SyncLock
        End Sub





        ''' <summary>
        ''' Gets or sets the vertex <see cref="pccDXF4.Vector2f">location</see>.
        ''' </summary>
        Public Property Location() As Vector2f
            Get
                SyncLock _locker
                    Return Me.m_location
                End SyncLock
            End Get
            Set(ByVal value As Vector2f)
                SyncLock _locker
                    Me.m_location = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the vertex weight.
        ''' </summary>
        Public Property Weight() As Single
            Get
                SyncLock _locker
                    Return Me.m_weight
                End SyncLock
            End Get
            Set(ByVal value As Single)
                SyncLock _locker
                    Me.m_weight = value
                End SyncLock
            End Set
        End Property


    End Class
End Namespace
