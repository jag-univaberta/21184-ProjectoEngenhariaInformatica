Imports System.Collections.Generic

Namespace Tables
    ''' <summary>
    ''' Represents a line type.
    ''' </summary>
    Public Class LineType
        Inherits DxfObject
        Implements ITableObject


        Private ReadOnly m_name As String
        Private m_description As String
        Private m_segments As List(Of Single)

        Private Shared ReadOnly _locker As New Object()

        ''' <summary>
        ''' Gets the ByLayer line type.
        ''' </summary>
        Public Shared ReadOnly Property ByLayer() As LineType
            Get
                SyncLock _locker
                    Return New LineType("ByLayer")
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Gets the ByBlock line type.
        ''' </summary>
        Public Shared ReadOnly Property ByBlock() As LineType
            Get
                SyncLock _locker
                    Return New LineType("ByBlock")
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Gets a predefined continuous line.
        ''' </summary>
        Public Shared ReadOnly Property Continuous() As LineType
            Get
                SyncLock _locker
                    Dim result = New LineType("Continuous") With { _
                     .Description = "Solid line" _
                    }
                    Return result
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Gets a predefined center line.
        ''' </summary>
        Public Shared ReadOnly Property Center() As LineType
            Get
                SyncLock _locker
                    Dim result = New LineType("Center") With { _
                     .Description = "Center, ____ _ ____ _ ____ _ ____ _ ____ _ ____" _
                    }
                    result.Segments.AddRange(New Single() {1.25F, -0.25F, 0.25F, -0.25F})
                    Return result
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Gets a predefined dash dot line.
        ''' </summary>
        Public Shared ReadOnly Property DashDot() As LineType
            Get
                SyncLock _locker
                    Dim result = New LineType("Dashdot") With { _
                     .Description = "Dash dot, __ . __ . __ . __ . __ . __ . __ . __" _
                    }
                    result.Segments.AddRange(New Single() {0.5F, -0.25F, 0.0F, -0.25F})
                    Return result
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Gets a predefined dashed line
        ''' </summary>
        Public Shared ReadOnly Property Dashed() As LineType
            Get
                SyncLock _locker
                    Dim result = New LineType("Dashed") With { _
                     .Description = "Dashed, __ __ __ __ __ __ __ __ __ __ __ __ __ _" _
                    }
                    result.Segments.AddRange(New Single() {0.5F, -0.25F})
                    Return result
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Gets a predefined dot line
        ''' </summary>
        Public Shared ReadOnly Property Dot() As LineType
            Get
                SyncLock _locker
                    Dim result = New LineType("Dot") With { _
                     .Description = "Dot, . . . . . . . . . . . . . . . . . . . . . . . ." _
                    }
                    result.Segments.AddRange(New Single() {0.0F, -0.25F})
                    Return result
                End SyncLock
            End Get
        End Property





        ''' <summary>
        ''' Initializes a new instance of the <c>LineType</c> class.
        ''' </summary>
        ''' <param name="name">Line type name.</param>
        Public Sub New(ByVal name As String)
            MyBase.New(DxfObjectCode.LineType)
            SyncLock _locker
                If String.IsNullOrEmpty(name) Then
                    Throw (New ArgumentNullException("name"))
                End If
                Me.m_name = name
                Me.m_description = String.Empty
                Me.m_segments = New List(Of Single)()
            End SyncLock
        End Sub





        ''' <summary>
        ''' Gets or sets the line type description.
        ''' </summary>
        Public Property Description() As String
            Get
                SyncLock _locker
                    Return Me.m_description
                End SyncLock
            End Get
            Set(ByVal value As String)
                SyncLock _locker
                    Me.m_description = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or stes the list of line type segments.
        ''' </summary>
        ''' <remarks>
        ''' Positive values means solid segments and negative values means spaces (one entry per element)
        ''' </remarks>
        Public Property Segments() As List(Of Single)
            Get
                SyncLock _locker
                    Return Me.m_segments
                End SyncLock
            End Get
            Set(ByVal value As List(Of Single))
                SyncLock _locker
                    If value Is Nothing Then
                        Throw New NullReferenceException("value")
                    End If
                    Me.m_segments = value
                End SyncLock
            End Set
        End Property




        ''' <summary>
        ''' Gets the total length of the line type.
        ''' </summary>
        Public Function Legth() As Single
            SyncLock _locker
                Dim result As Single = 0
                For Each s As Single In Me.m_segments
                    result += Math.Abs(s)
                Next
                Return result
            End SyncLock
        End Function





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
