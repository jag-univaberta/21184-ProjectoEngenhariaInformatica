
Namespace Entities
    ''' <summary>
    ''' Represents a lightweight polyline vertex.
    ''' </summary>
    Public Class LightWeightPolylineVertex


        Protected Const m_TYPE As EntityType = EntityType.LightWeightPolylineVertex
        Protected m_location As Vector2f
        Protected m_beginThickness As Single
        Protected m_endThickness As Single
        Protected m_bulge As Single

        Private Shared ReadOnly _locker As New Object()
         

        ''' <summary>
        ''' Initializes a new instance of the <c>LightWeightPolylineVertex</c> class.
        ''' </summary>
        Public Sub New()
            SyncLock _locker
                Me.m_location = Vector2f.Zero
                Me.m_bulge = 0.0F
                Me.m_beginThickness = 0.0F
                Me.m_endThickness = 0.0F
            End SyncLock
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <c>LightWeightPolylineVertex</c> class.
        ''' </summary>
        ''' <param name="location">Lightweight polyline <see cref="pccDXF4.Vector2f">vertex</see> coordinates.</param>
        Public Sub New(ByVal location As Vector2f)
            SyncLock _locker
                Me.m_location = location
                Me.m_bulge = 0.0F
                Me.m_beginThickness = 0.0F
                Me.m_endThickness = 0.0F
            End SyncLock
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <c>LightWeightPolylineVertex</c> class.
        ''' </summary>
        ''' <param name="x">X coordinate.</param>
        ''' <param name="y">Y coordinate.</param>
        Public Sub New(ByVal x As Single, ByVal y As Single)
            SyncLock _locker
                Me.m_location = New Vector2f(x, y)
                Me.m_bulge = 0.0F
                Me.m_beginThickness = 0.0F
                Me.m_endThickness = 0.0F
            End SyncLock
        End Sub





        ''' <summary>
        ''' Gets or sets the polyline vertex <see cref="pccDXF4.Vector2f">location</see>.
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
        ''' Gets or sets the light weight polyline begin thickness.
        ''' </summary>
        Public Property BeginThickness() As Single
            Get
                SyncLock _locker
                    Return Me.m_beginThickness
                End SyncLock
            End Get
            Set(ByVal value As Single)
                SyncLock _locker
                    Me.m_beginThickness = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the light weight polyline end thickness.
        ''' </summary>
        Public Property EndThickness() As Single
            Get
                SyncLock _locker
                    Return Me.m_endThickness
                End SyncLock
            End Get
            Set(ByVal value As Single)
                SyncLock _locker
                    Me.m_endThickness = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or set the light weight polyline bulge. Accepted values range from -1 to 1.
        ''' </summary>
        ''' <remarks>
        ''' The bulge is the tangent of one fourth the included angle for an arc segment, 
        ''' made negative if the arc goes clockwise from the start point to the endpoint. 
        ''' A bulge of 0 indicates a straight segment, and a bulge of 1 is a semicircle.
        ''' </remarks>
        Public Property Bulge() As Single
            Get
                SyncLock _locker
                    Return Me.m_bulge
                End SyncLock
            End Get
            Set(ByVal value As Single)
                SyncLock _locker
                    If Me.m_bulge < -1.0 OrElse Me.m_bulge > 1.0F Then
                        Throw New ArgumentOutOfRangeException("value", value, "The bulge must be a value between minus one and plus one")
                    End If
                    Me.m_bulge = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets the entity <see cref="pccDXF4.Entities.EntityType">type</see>.
        ''' </summary>
        Public ReadOnly Property Type() As EntityType
            Get
                SyncLock _locker
                    Return m_TYPE
                End SyncLock
            End Get
        End Property





        ''' <summary>
        ''' Converts the value of this instance to its equivalent string representation.
        ''' </summary>
        ''' <returns>The string representation.</returns>
        Public Overrides Function ToString() As String
            SyncLock _locker
                Return [String].Format("{0} ({1})", m_TYPE, Me.m_location)
            End SyncLock
        End Function


    End Class
End Namespace
