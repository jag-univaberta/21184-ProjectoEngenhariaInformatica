
Namespace Tables
    ''' <summary>
    ''' Represents a layer.
    ''' </summary>
    Public Class Layer
        Inherits DxfObject
        Implements ITableObject

        Private Shared m_plotStyleHandle As String
        Private ReadOnly m_name As String
        Private m_color As AciColor
        Private m_isVisible As Boolean
        Private m_lineType As LineType 

        Private Shared ReadOnly _locker As New Object()

        ''' <summary>
        ''' Initializes a new instance of the <c>Layer</c> class.
        ''' </summary>
        ''' <param name="name">Layer name.</param>
        Public Sub New(ByVal name As String)
            MyBase.New(DxfObjectCode.Layer)
            SyncLock _locker
                If String.IsNullOrEmpty(name) Then
                    Throw (New ArgumentNullException("name"))
                End If
                Me.m_name = name
                Me.m_color = AciColor.[Default]
                Me.m_lineType = LineType.Continuous
                Me.m_isVisible = True
            End SyncLock
        End Sub
        ''' <summary>
        ''' Gets the default Layer.
        ''' </summary>
        Public Shared ReadOnly Property [Default]() As Layer
            Get
                SyncLock _locker
                    Return New Layer("0")
                End SyncLock
            End Get
        End Property
         
        Friend Shared Property PlotStyleHandle() As String
            Get
                SyncLock _locker
                    Return m_plotStyleHandle
                End SyncLock
            End Get
            Set(ByVal value As String)
                SyncLock _locker
                    m_plotStyleHandle = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the layer <see cref="LineType"line type></see>.
        ''' </summary>
        Public Property LineType() As LineType
            Get
                SyncLock _locker
                    Return Me.m_lineType
                End SyncLock
            End Get
            Set(ByVal value As LineType)
                SyncLock _locker
                    If value Is Nothing Then
                        Throw New NullReferenceException("value")
                    End If
                    Me.m_lineType = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the layer <see cref="AciColor">color</see>.
        ''' </summary>
        Public Property Color() As AciColor
            Get
                SyncLock _locker
                    Return Me.m_color
                End SyncLock
            End Get
            Set(ByVal value As AciColor)
                SyncLock _locker
                    If value Is Nothing Then
                        Throw New NullReferenceException("value")
                    End If
                    Me.m_color = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets if the layer is visible.
        ''' </summary>
        Public Property IsVisible() As Boolean
            Get
                SyncLock _locker
                    Return Me.m_isVisible
                End SyncLock
            End Get
            Set(ByVal value As Boolean)
                SyncLock _locker
                    Me.m_isVisible = value
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
