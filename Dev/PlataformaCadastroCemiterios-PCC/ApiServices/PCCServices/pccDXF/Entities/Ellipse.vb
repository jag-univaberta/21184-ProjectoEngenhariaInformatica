
Imports System.Collections.Generic
Imports pccDXF4.Tables

Namespace Entities
    ''' <summary>
    ''' Represents an ellipse <see cref="pccDXF4.Entities.IEntityObject">entity</see>.
    ''' </summary>
    Public Class Ellipse
        Inherits DxfObject
        Implements IEntityObject

        Private Const m_TYPE As EntityType = EntityType.Ellipse
        Private m_center As Vector3f
        Private m_majorAxis As Single
        Private m_minorAxis As Single
        Private m_rotation As Single
        Private m_startAngle As Single
        Private m_endAngle As Single
        Private m_thickness As Single
        Private m_layer As Layer
        Private m_color As AciColor
        Private m_lineType As LineType
        Private m_normal As Vector3f
        Private m_curvePoints As Integer
        Private m_xData As Dictionary(Of ApplicationRegistry, XData)

        Private Shared ReadOnly _locker As New Object()
         
        ''' <summary>
        ''' Initializes a new instance of the <c>Ellipse</c> class.
        ''' </summary>
        ''' <param name="center">Ellipse <see cref="Vector3f">center</see> in object coordinates.</param>
        ''' <param name="majorAxis">Ellipse major axis.</param>
        ''' <param name="minorAxis">Ellipse minor axis.</param>
        ''' <remarks>The center Z coordinate represents the elevation of the arc along the normal.</remarks>
        Public Sub New(ByVal center As Vector3f, ByVal majorAxis As Single, ByVal minorAxis As Single)
            MyBase.New(DxfObjectCode.Ellipse)
            SyncLock _locker
                Me.m_center = center
                Me.m_majorAxis = majorAxis
                Me.m_minorAxis = minorAxis
                Me.m_startAngle = 0.0F
                Me.m_endAngle = 360.0F
                Me.m_rotation = 0.0F
                Me.m_curvePoints = 30
                Me.m_thickness = 0.0F
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_normal = Vector3f.UnitZ
            End SyncLock
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <c>ellipse</c> class.
        ''' </summary>
        Public Sub New()
            MyBase.New(DxfObjectCode.Ellipse)
            SyncLock _locker
                Me.m_center = Vector3f.Zero
                Me.m_majorAxis = 1.0F
                Me.m_minorAxis = 0.5F
                Me.m_rotation = 0.0F
                Me.m_startAngle = 0.0F
                Me.m_endAngle = 360.0F
                Me.m_rotation = 0.0F
                Me.m_curvePoints = 30
                Me.m_thickness = 0.0F
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_normal = Vector3f.UnitZ
            End SyncLock
        End Sub





        ''' <summary>
        ''' Gets or sets the ellipse <see cref="pccDXF4.Vector3f">center</see>.
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
        ''' Gets or sets the ellipse mayor axis.
        ''' </summary>
        Public Property MajorAxis() As Single
            Get
                SyncLock _locker
                    Return Me.m_majorAxis
                End SyncLock
            End Get
            Set(ByVal value As Single)
                SyncLock _locker
                    If value <= 0 Then
                        Throw New ArgumentOutOfRangeException("value", value, "The major axis value must be greater than zero.")
                    End If
                    Me.m_majorAxis = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the ellipse minor axis.
        ''' </summary>
        Public Property MinorAxis() As Single
            Get
                SyncLock _locker
                    Return Me.m_minorAxis
                End SyncLock
            End Get
            Set(ByVal value As Single)
                SyncLock _locker
                    If value <= 0 Then
                        Throw New ArgumentOutOfRangeException("value", value, "The minor axis value must be greater than zero.")
                    End If
                    Me.m_minorAxis = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the ellipse rotation along its normal.
        ''' </summary>
        Public Property Rotation() As Single
            Get
                SyncLock _locker
                    Return Me.m_rotation
                End SyncLock
            End Get
            Set(ByVal value As Single)
                SyncLock _locker
                    Me.m_rotation = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the ellipse start angle in degrees.
        ''' </summary>
        ''' <remarks><c>StartAngle</c> equals 0 and <c>EndAngle</c> equals 360 for a full ellipse.</remarks>
        Public Property StartAngle() As Single
            Get
                SyncLock _locker
                    Return Me.m_startAngle
                End SyncLock
            End Get
            Set(ByVal value As Single)
                SyncLock _locker
                    Me.m_startAngle = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the ellipse end angle in degrees.
        ''' </summary>
        ''' <remarks><c>StartAngle</c> equals 0 and <c>EndAngle</c> equals 360 for a full ellipse.</remarks>
        Public Property EndAngle() As Single
            Get
                SyncLock _locker
                    Return Me.m_endAngle
                End SyncLock
            End Get
            Set(ByVal value As Single)
                SyncLock _locker
                    Me.m_endAngle = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the ellipse <see cref="pccDXF4.Vector3f">normal</see>.
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
        ''' Gets or sets the number of points generated along the ellipse during the conversion to a polyline.
        ''' </summary>
        Public Property CurvePoints() As Integer
            Get
                SyncLock _locker
                    Return Me.m_curvePoints
                End SyncLock
            End Get
            Set(ByVal value As Integer)
                SyncLock _locker
                    Me.m_curvePoints = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the ellipse thickness.
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
        ''' Checks if the the actual instance is a full ellipse.
        ''' </summary>
        Public ReadOnly Property IsFullEllipse() As Boolean
            Get
                SyncLock _locker
                    Return (Me.m_startAngle + Me.m_endAngle = 360)
                End SyncLock
            End Get
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
        ''' Converts the ellipse in a Polyline.
        ''' </summary>
        ''' <param name="precision">Number of vertexes generated.</param>
        ''' <returns>A new instance of <see cref="Polyline">Polyline</see> that represents the ellipse.</returns>
        Public Function ToPolyline(ByVal precision As Integer) As Polyline
            SyncLock _locker
                Dim vertexes As List(Of Vector2f) = Me.PolygonalVertexes(precision)
                Dim ocsCenter As Vector3d = MathHelper.Transform(CType(Me.m_center, Vector3d), CType(Me.m_normal, Vector3d), MathHelper.CoordinateSystem.World, MathHelper.CoordinateSystem.[Object])
                Dim poly As New Polyline() With { _
                  .Color = Me.m_color, _
                  .Layer = Me.m_layer, _
                  .LineType = Me.m_lineType, _
                  .Normal = Me.m_normal, _
                  .Elevation = CSng(ocsCenter.Z), _
                  .Thickness = Me.m_thickness _
                }
                poly.XData = Me.m_xData

                For Each v As Vector2f In vertexes
                    poly.Vertexes.Add(New PolylineVertex(CSng(v.X + ocsCenter.X), CSng(v.Y + ocsCenter.Y)))
                Next
                If Me.IsFullEllipse Then
                    poly.IsClosed = True
                End If

                Return poly
            End SyncLock
        End Function

        ''' <summary>
        ''' Converts the ellipse in a list of vertexes.
        ''' </summary>
        ''' <param name="precision">Number of vertexes generated.</param>
        ''' <returns>A list vertexes that represents the ellipse expresed in object coordinate system.</returns>
        Public Function PolygonalVertexes(ByVal precision As Integer) As List(Of Vector2f)
            SyncLock _locker
                Dim points As New List(Of Vector2f)()
                Dim beta As Single = CSng(Me.m_rotation * MathHelper.DegToRad)
                Dim sinbeta As Single = CSng(Math.Sin(beta))
                Dim cosbeta As Single = CSng(Math.Cos(beta))

                If Me.IsFullEllipse Then
                    Dim i As Integer = 0
                    While i < 360
                        Dim alpha As Single = CSng(i * MathHelper.DegToRad)
                        Dim sinalpha As Single = CSng(Math.Sin(alpha))
                        Dim cosalpha As Single = CSng(Math.Cos(alpha))

                        Dim pointX As Single = 0.5F * (Me.m_majorAxis * cosalpha * cosbeta - Me.m_minorAxis * sinalpha * sinbeta)
                        Dim pointY As Single = 0.5F * (Me.m_majorAxis * cosalpha * sinbeta + Me.m_minorAxis * sinalpha * cosbeta)

                        points.Add(New Vector2f(pointX, pointY))
                        i += 360 \ precision
                    End While
                Else
                    For i As Integer = 0 To precision
                        Dim angle As Single = Me.m_startAngle + i * (Me.m_endAngle - Me.m_startAngle) / precision
                        points.Add(Me.PointFromEllipse(angle))
                    Next
                End If
                Return points
            End SyncLock
        End Function

        Private Function PointFromEllipse(ByVal degrees As Single) As Vector2f
            SyncLock _locker
                ' Convert the basic input into something more usable
                Dim ptCenter As New Vector2f(Me.m_center.X, Me.m_center.Y)
                Dim radians As Single = CSng(degrees * MathHelper.DegToRad)

                ' Calculate the radius of the ellipse for the given angle
                Dim a As Single = Me.m_majorAxis
                Dim b As Single = Me.m_minorAxis
                Dim eccentricity As Single = CSng(Math.Sqrt(1 - (b * b) / (a * a)))
                Dim radiusAngle As Single = b / CSng(Math.Sqrt(1 - (eccentricity * eccentricity) * Math.Pow(Math.Cos(radians), 2)))

                ' Convert the radius back to Cartesian coordinates
                Return New Vector2f(ptCenter.X + radiusAngle * CSng(Math.Cos(radians)), ptCenter.Y + radiusAngle * CSng(Math.Sin(radians)))
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
