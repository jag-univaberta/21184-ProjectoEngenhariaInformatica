
Imports System.Collections.Generic
Imports pccDXF4.Tables

Namespace Entities
    ''' <summary>
    ''' Represents a circle <see cref="pccDXF4.Entities.IEntityObject">entity</see>.
    ''' </summary>
    Public Class Circle
        Inherits DxfObject
        Implements IEntityObject



        Private Const m_TYPE As EntityType = EntityType.Circle
        Private m_center As Vector3f
        Private m_radius As Single
        Private m_thickness As Single
        Private m_layer As Layer
        Private m_color As AciColor
        Private m_lineType As LineType
        Private m_normal As Vector3f
        Private m_xData As Dictionary(Of ApplicationRegistry, XData)


        Private Shared ReadOnly _locker As New Object()



        ''' <summary>
        ''' Initializes a new instance of the <c>Circle</c> class.
        ''' </summary>
        ''' <param name="center">Circle <see cref="Vector3f">center</see> in object coordinates.</param>
        ''' <param name="radius">Circle radius.</param>
        ''' <remarks>The center Z coordinate represents the elevation of the arc along the normal.</remarks>
        Public Sub New(ByVal center As Vector3f, ByVal radius As Single)
            MyBase.New(DxfObjectCode.Circle)
            SyncLock _locker
                Me.m_center = center
                Me.m_radius = radius
                Me.m_thickness = 0.0F
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_normal = Vector3f.UnitZ
            End SyncLock
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <c>Circle</c> class.
        ''' </summary>
        Public Sub New()
            MyBase.New(DxfObjectCode.Circle)
            SyncLock _locker
                Me.m_center = Vector3f.Zero
                Me.m_radius = 1.0F
                Me.m_thickness = 0.0F
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_normal = Vector3f.UnitZ
            End SyncLock
        End Sub





        ''' <summary>
        ''' Gets or sets the circle <see cref="pccDXF4.Vector3f">center</see>.
        ''' </summary>
        ''' <remarks>The center Z coordinate represents the elevation of the arc along the normal.</remarks>
        Public Property Center() As Vector3f
            Get
                SyncLock _locker
                    Return Me.m_center
                End SyncLock
            End Get
            Set(ByVal value As Vector3f)
                SyncLock _locker
                    Me.m_center = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or set the circle radius.
        ''' </summary>
        Public Property Radius() As Single
            Get
                SyncLock _locker
                    Return Me.m_radius
                End SyncLock
            End Get
            Set(ByVal value As Single)
                SyncLock _locker
                    Me.m_radius = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the arc thickness.
        ''' </summary>
        Public Property Thickness() As Single
            Get
                SyncLock _locker
                    Return Me.m_thickness
                End SyncLock
            End Get
            Set(ByVal value As Single)
                SyncLock _locker
                    Me.m_thickness = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the circle <see cref="pccDXF4.Vector3f">normal</see>.
        ''' </summary>
        Public Property Normal() As Vector3f
            Get
                SyncLock _locker
                    Return Me.m_normal
                End SyncLock
            End Get
            Set(ByVal value As Vector3f)
                SyncLock _locker
                    value.Normalize()
                    Me.m_normal = value
                End SyncLock
            End Set
        End Property


        ''' <summary>
        ''' Gets the entity <see cref="pccDXF4.Entities.EntityType">type</see>.
        ''' </summary>
        Public ReadOnly Property Type() As EntityType Implements IEntityObject.Type
            Get
                SyncLock _locker
                    Return m_TYPE
                End SyncLock
            End Get
        End Property

        ''' <summary>
        ''' Gets or sets the entity <see cref="pccDXF4.AciColor">color</see>.
        ''' </summary>
        Public Property Color() As AciColor Implements IEntityObject.Color
            Get
                SyncLock _locker
                    Return Me.m_color
                End SyncLock
            End Get
            Set(ByVal value As AciColor)
                SyncLock _locker
                    If value Is Nothing Then
                        Throw New ArgumentNullException("value")
                    End If
                    Me.m_color = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the entity <see cref="pccDXF4.Tables.Layer">layer</see>.
        ''' </summary>
        Public Property Layer() As Layer Implements IEntityObject.Layer
            Get
                SyncLock _locker
                    Return Me.m_layer
                End SyncLock
            End Get
            Set(ByVal value As Layer)
                SyncLock _locker
                    If value Is Nothing Then
                        Throw New ArgumentNullException("value")
                    End If
                    Me.m_layer = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the entity <see cref="pccDXF4.Tables.LineType">line type</see>.
        ''' </summary>
        Public Property LineType() As LineType Implements IEntityObject.LineType
            Get
                SyncLock _locker
                    Return Me.m_lineType
                End SyncLock
            End Get
            Set(ByVal value As LineType)
                SyncLock _locker
                    If value Is Nothing Then
                        Throw New ArgumentNullException("value")
                    End If
                    Me.m_lineType = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the entity <see cref="pccDXF4.XData">extende data</see>.
        ''' </summary>
        Public Property XData() As Dictionary(Of ApplicationRegistry, XData) Implements IEntityObject.XData
            Get
                SyncLock _locker
                    Return Me.m_xData
                End SyncLock
            End Get
            Set(ByVal value As Dictionary(Of ApplicationRegistry, XData))
                SyncLock _locker
                    Me.m_xData = value
                End SyncLock
            End Set
        End Property


        ''' <summary>
        ''' Converts the circle in a list of vertexes.
        ''' </summary>
        ''' <param name="precision">Number of vertexes generated.</param>
        ''' <returns>A list vertexes that represents the circle expresed in object coordinate system.</returns>
        Public Function PoligonalVertexes(ByVal precision As Integer) As List(Of Vector2f)
            SyncLock _locker
                If precision < 3 Then
                    Throw New ArgumentOutOfRangeException("precision", precision, "The circle precision must be greater or equal to three")
                End If

                Dim ocsVertexes As New List(Of Vector2f)()

                Dim angle As Single = CSng(MathHelper.TwoPI / precision)

                For i As Integer = 0 To precision - 1
                    Dim sine As Single = CSng(Me.m_radius * Math.Sin(MathHelper.HalfPI + angle * i))
                    Dim cosine As Single = CSng(Me.m_radius * Math.Cos(MathHelper.HalfPI + angle * i))
                    ocsVertexes.Add(New Vector2f(cosine + Me.m_center.X, sine + Me.m_center.Y))
                Next

                Return ocsVertexes
            End SyncLock
        End Function

        ''' <summary>
        ''' Converts the circle in a list of vertexes.
        ''' </summary>
        ''' <param name="precision">Number of vertexes generated.</param>
        ''' <param name="weldThreshold">Tolerance to consider if two new generated vertexes are equal.</param>
        ''' <returns>A list vertexes that represents the circle expresed in object coordinate system.</returns>
        Public Function PoligonalVertexes(ByVal precision As Integer, ByVal weldThreshold As Single) As List(Of Vector2f)
            SyncLock _locker


                If precision < 3 Then
                    Throw New ArgumentOutOfRangeException("precision", precision, "The circle precision must be greater or equal to three")
                End If

                Dim ocsVertexes As New List(Of Vector2f)()

                If 2 * Me.m_radius >= weldThreshold Then
                    Dim angulo As Single = CSng(MathHelper.TwoPI / precision)
                    Dim prevPoint As Vector2f
                    Dim firstPoint As Vector2f

                    Dim sine As Single = CSng(Me.m_radius * Math.Sin(MathHelper.HalfPI * 0.5))
                    Dim cosine As Single = CSng(Me.m_radius * Math.Cos(MathHelper.HalfPI * 0.5))
                    firstPoint = New Vector2f(cosine + Me.m_center.X, sine + Me.m_center.Y)
                    ocsVertexes.Add(firstPoint)
                    prevPoint = firstPoint

                    For i As Integer = 1 To precision - 1
                        sine = CSng(Me.m_radius * Math.Sin(MathHelper.HalfPI + angulo * i))
                        cosine = CSng(Me.m_radius * Math.Cos(MathHelper.HalfPI + angulo * i))
                        Dim point As New Vector2f(cosine + Me.m_center.X, sine + Me.m_center.Y)

                        If Not point.Equals(prevPoint, weldThreshold) AndAlso Not point.Equals(firstPoint, weldThreshold) Then
                            ocsVertexes.Add(point)
                            prevPoint = point
                        End If
                    Next
                End If

                Return ocsVertexes
            End SyncLock
        End Function
         
        ''' <summary>
        ''' Converts the value of this instance to its equivalent string representation.
        ''' </summary>
        ''' <returns>The string representation.</returns>
        Public Overrides Function ToString() As String
            SyncLock _locker
                Return m_TYPE.ToString()
            End SyncLock
        End Function

 

    

         
    End Class
End Namespace
