Namespace Tables
    ''' <summary>
    ''' Represents a text style.
    ''' </summary>
    ''' <remarks>
    ''' AutoCad12 does not support true type fonts.
    ''' </remarks>
    Public Class TextStyle
        Inherits DxfObject
        Implements ITableObject


        Private ReadOnly m_font As String
        Private ReadOnly m_name As String
        Private m_height As Single
        Private m_isBackward As Boolean
        Private m_isUpsideDown As Boolean
        Private m_isVertical As Boolean
        Private m_obliqueAngle As Single
        Private m_widthFactor As Single

        Private Shared ReadOnly _locker As New Object()

        ''' <summary>
        ''' Gets the default text style.
        ''' </summary>
        Public Shared ReadOnly Property [Default]() As TextStyle
            Get
                SyncLock _locker
                    Return New TextStyle("Standard", "simplex")
                End SyncLock
            End Get
        End Property


        ''' <summary>
        ''' Initializes a new instance of the <c>TextStyle</c> class.
        ''' </summary>
        ''' <param name="name">Text style name.</param>
        ''' <param name="font">Text style font name.</param>
        Public Sub New(ByVal name As String, ByVal font As String)
            MyBase.New(DxfObjectCode.TextStyle)
            SyncLock _locker
                If String.IsNullOrEmpty(name) Then
                    Throw (New ArgumentNullException("name"))
                End If
                Me.m_name = name
                If String.IsNullOrEmpty(font) Then
                    font = "simplex"
                End If
                Me.m_font = font
                Me.m_widthFactor = 1.0F
                Me.m_obliqueAngle = 0.0F
                Me.m_height = 0.0F
                Me.m_isVertical = False
                Me.m_isBackward = False
                Me.m_isUpsideDown = False
            End SyncLock
        End Sub

        ''' <summary>
        ''' Gets the text style font name.
        ''' </summary>
        Public ReadOnly Property Font() As String
            Get
                SyncLock _locker
                    Return Me.m_font
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the text height.
        ''' </summary>
        ''' <remarks>Fixed text height; 0 if not fixed.</remarks>
        Public Property Height() As Single
            Get
                SyncLock _locker
                    Return Me.m_height
                End SyncLock
            End Get
            Set(ByVal value As Single)
                SyncLock _locker
                    If value < 0 Then
                        Throw (New ArgumentOutOfRangeException("value", value, "The height can not be less than zero."))
                    End If
                    Me.m_height = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the width factor.
        ''' </summary>
        Public Property WidthFactor() As Single
            Get
                SyncLock _locker
                    Return Me.m_widthFactor
                End SyncLock
            End Get
            Set(ByVal value As Single)
                SyncLock _locker
                    If value <= 0 Then
                        Throw (New ArgumentOutOfRangeException("value", value, "The width factor should be greater than zero."))
                    End If
                    Me.m_widthFactor = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the font oblique angle.
        ''' </summary>
        Public Property ObliqueAngle() As Single
            Get
                SyncLock _locker
                    Return Me.m_obliqueAngle
                End SyncLock
            End Get
            Set(ByVal value As Single)
                SyncLock _locker
                    Me.m_obliqueAngle = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the text is vertical.
        ''' </summary>
        Public Property IsVertical() As Boolean
            Get
                SyncLock _locker
                    Return Me.m_isVertical
                End SyncLock
            End Get
            Set(ByVal value As Boolean)
                SyncLock _locker
                    Me.m_isVertical = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets if the text is backward (mirrored in X).
        ''' </summary>
        Public Property IsBackward() As Boolean
            Get
                SyncLock _locker
                    Return Me.m_isBackward
                End SyncLock
            End Get
            Set(ByVal value As Boolean)
                SyncLock _locker
                    Me.m_isBackward = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets if the text is upside down (mirrored in Y).
        ''' </summary>
        Public Property IsUpsideDown() As Boolean
            Get
                SyncLock _locker
                    Return Me.m_isUpsideDown
                End SyncLock
            End Get
            Set(ByVal value As Boolean)
                SyncLock _locker
                    Me.m_isUpsideDown = value
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
