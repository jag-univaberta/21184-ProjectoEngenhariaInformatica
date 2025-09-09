
Imports System.Collections.Generic
Imports pccDXF4.Tables

Namespace Entities
    ''' <summary>
    ''' Represents a nurbs curve <see cref="pccDXF4.Entities.IEntityObject">entity</see>.
    ''' </summary>
    ''' <remarks>The nurbs curve uses a default open uniform knot vector.</remarks>
    Public Class NurbsCurve
        Implements IEntityObject


        Private Const m_TYPE As EntityType = EntityType.NurbsCurve
        Private Const DXF_NAME As String = DxfObjectCode.Polyline
        Private m_color As AciColor
        Private m_layer As Layer
        Private m_lineType As LineType
        Private m_xData As Dictionary(Of ApplicationRegistry, XData)
        Private m_controlPoints As List(Of NurbsVertex)
        Private knotVector As Single()
        Private m_order As Integer
        Private m_elevation As Single
        Private m_thickness As Single
        Private m_normal As Vector3f
        Private m_curvePoints As Integer

        Private Shared ReadOnly _locker As New Object()




        ''' <summary>
        ''' Initializes a new instance of the <c>NurbsCurve</c> class.
        ''' </summary>
        Public Sub New()
            SyncLock _locker
                Me.m_controlPoints = New List(Of NurbsVertex)()
                Me.m_normal = Vector3f.UnitZ
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_order = 0
                Me.m_curvePoints = 30
                Me.m_elevation = 0.0F
                Me.m_thickness = 0.0F
                Me.m_normal = Vector3f.UnitZ
            End SyncLock
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <c>NurbsCurve</c> class.
        ''' </summary>
        ''' <param name="controlPoints">The nurbs curve <see cref="pccDXF4.Entities.NurbsVertex">control point</see> list.</param>
        ''' <param name="order">The nurbs curve order.</param>
        Public Sub New(ByVal controlPoints As List(Of NurbsVertex), ByVal order As Integer)
            SyncLock _locker
                If controlPoints.Count < order Then
                    Throw New ArgumentOutOfRangeException("order", order, "The order of the curve must be less or equal the number of control points.")
                End If
                Me.m_controlPoints = controlPoints
                Me.m_normal = Vector3f.UnitZ
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_order = order
                Me.m_curvePoints = 30
                Me.m_elevation = 0.0F
                Me.m_thickness = 0.0F
                Me.m_normal = Vector3f.UnitZ
            End SyncLock
        End Sub






        ''' <summary>
        ''' Gets the nurbs curve <see cref="NurbsVertex">control point</see> list.
        ''' </summary>
        Public Property ControlPoints() As List(Of NurbsVertex)
            Get
                SyncLock _locker
                    Return Me.m_controlPoints
                End SyncLock
            End Get
            Set(ByVal value As List(Of NurbsVertex))
                SyncLock _locker
                    If value Is Nothing Then
                        Throw New ArgumentNullException("value")
                    End If
                    Me.m_controlPoints = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the nurbs curve order.
        ''' </summary>
        Public Property Order() As Integer
            Get
                SyncLock _locker
                    Return Me.m_order
                End SyncLock
            End Get
            Set(ByVal value As Integer)
                SyncLock _locker
                    Me.m_order = value
                End SyncLock
            End Set
        End Property

        ''' <summary>
        ''' Gets or sets the nurbs curve <see cref="pccDXF4.Vector3f">normal</see>.
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
        ''' Gets or sets the number of points generated along the nurbs curve during the conversion to a polyline.
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
        ''' Gets or sets the nurbs curve thickness.
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
        ''' Gets or sets the nurbs curve elevation.
        ''' </summary>
        Public Property Elevation() As Single
            Get
                SyncLock _locker
                    Return Me.m_elevation
                End SyncLock
            End Get
            Set(ByVal value As Single)
                SyncLock _locker
                    Me.m_elevation = value
                End SyncLock
            End Set
        End Property
 

        ''' <summary>
        ''' Gets the dxf code that represents the entity.
        ''' </summary>
        Public ReadOnly Property DxfName() As String
            Get
                SyncLock _locker
                    Return DXF_NAME
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
        ''' Obtains a list of vertexes that represent the nurbs curve.
        ''' </summary>
        ''' <param name="precision">Number of point to approximate the curve to a polyline.</param>
        ''' <returns>The vertexes are expresed in object coordinate system.</returns>
        Public Function PolygonalVertexes(ByVal precision As Integer) As List(Of Vector2f)
            SyncLock _locker
                If Me.m_controlPoints.Count < Me.m_order Then
                    Throw New ArithmeticException("The order of the curve must be less or equal the number of control points.")
                End If

                Me.knotVector = Me.SetKnotVector()
                Dim nurbsBasisFunctions As Single()()() = Me.DefineBasisFunctions(precision)

                Dim vertexes As New List(Of Vector2f)()

                For i As Integer = 0 To precision - 1
                    Dim x As Single = 0.0F
                    Dim y As Single = 0.0F
                    For ctrlPointIndex As Integer = 0 To Me.m_controlPoints.Count - 1
                        x += Me.m_controlPoints(ctrlPointIndex).Location.X * nurbsBasisFunctions(i)(ctrlPointIndex)(Me.m_order - 1)
                        y += Me.m_controlPoints(ctrlPointIndex).Location.Y * nurbsBasisFunctions(i)(ctrlPointIndex)(Me.m_order - 1)
                    Next

                    vertexes.Add(New Vector2f(x, y))
                Next

                Return vertexes
            End SyncLock
        End Function

        ''' <summary>
        ''' Sets a constant weight for all the nurbs curve <see cref="NurbsVertex">vertex</see> list.
        ''' </summary>
        ''' <param name="weight">Nurbs vertex weight.</param>
        Public Sub SetUniformWeights(ByVal weight As Single)
            SyncLock _locker
                For Each v As NurbsVertex In Me.m_controlPoints
                    v.Weight = weight
                Next
            End SyncLock
        End Sub
 

        Private Function DefineBasisFunctions(ByVal precision As Integer) As Single()()()
            SyncLock _locker
                Dim nurbsBasisFunctions As Single()()()
                Dim basisFunctions As Single()()()

                basisFunctions = New Single(precision - 1)()() {}

                nurbsBasisFunctions = New Single(precision - 1)()() {}

                For vertexIndex As Integer = 0 To precision - 1
                    basisFunctions(vertexIndex) = New Single(Me.m_controlPoints.Count)() {}
                    nurbsBasisFunctions(vertexIndex) = New Single(Me.m_controlPoints.Count)() {}

                    Dim t As Single = vertexIndex / CSng(precision - 1)

                    If t = 1.0F Then
                        t = 1.0F - MathHelper.EpsilonF
                    End If

                    For ctrlPointIndex As Integer = 0 To Me.m_controlPoints.Count
                        basisFunctions(vertexIndex)(ctrlPointIndex) = New Single(Me.m_order - 1) {}
                        nurbsBasisFunctions(vertexIndex)(ctrlPointIndex) = New Single(Me.m_order - 1) {}

                        If t >= Me.knotVector(ctrlPointIndex) AndAlso t < Me.knotVector(ctrlPointIndex + 1) Then
                            basisFunctions(vertexIndex)(ctrlPointIndex)(0) = 1.0F
                        Else
                            basisFunctions(vertexIndex)(ctrlPointIndex)(0) = 0.0F
                        End If
                    Next
                Next

                For orderIndex As Integer = 1 To Me.m_order - 1
                    For ctrlPointIndex As Integer = 0 To Me.m_controlPoints.Count - 1
                        For vertexIndex As Integer = 0 To precision - 1
                            Dim t As Single = vertexIndex / CSng(precision - 1)

                            Dim Nikm1 As Single = basisFunctions(vertexIndex)(ctrlPointIndex)(orderIndex - 1)
                            Dim Nip1km1 As Single = basisFunctions(vertexIndex)(ctrlPointIndex + 1)(orderIndex - 1)

                            Dim xi As Single = Me.knotVector(ctrlPointIndex)
                            Dim xikm1 As Single = Me.knotVector(ctrlPointIndex + orderIndex - 1 + 1)
                            Dim xik As Single = Me.knotVector(ctrlPointIndex + orderIndex + 1)
                            Dim xip1 As Single = Me.knotVector(ctrlPointIndex + 1)

                            Dim FirstTermBasis As Single
                            If Math.Abs(xikm1 - xi) < MathHelper.EpsilonF Then
                                FirstTermBasis = 0.0F
                            Else
                                FirstTermBasis = ((t - xi) * Nikm1) / (xikm1 - xi)
                            End If

                            Dim SecondTermBasis As Single
                            If Math.Abs(xik - xip1) < MathHelper.EpsilonF Then
                                SecondTermBasis = 0.0F
                            Else
                                SecondTermBasis = ((xik - t) * Nip1km1) / (xik - xip1)
                            End If

                            basisFunctions(vertexIndex)(ctrlPointIndex)(orderIndex) = FirstTermBasis + SecondTermBasis
                        Next
                    Next
                Next

                For orderIndex As Integer = 1 To Me.m_order - 1
                    For ctrlPointIndex As Integer = 0 To Me.m_controlPoints.Count - 1
                        For vertexIndex As Integer = 0 To precision - 1
                            Dim denominator As Single = 0.0F
                            For controlWeight As Integer = 0 To Me.m_controlPoints.Count - 1
                                denominator += Me.m_controlPoints(controlWeight).Weight * basisFunctions(vertexIndex)(controlWeight)(orderIndex)
                            Next

                            nurbsBasisFunctions(vertexIndex)(ctrlPointIndex)(orderIndex) = Me.m_controlPoints(ctrlPointIndex).Weight * basisFunctions(vertexIndex)(ctrlPointIndex)(orderIndex) / denominator
                        Next
                    Next
                Next

                Return nurbsBasisFunctions
            End SyncLock
        End Function

        Private Function SetKnotVector() As Single()
            SyncLock _locker
                'This code creates an open uniform knot vector
                Dim knots As Single() = New Single(Me.m_controlPoints.Count + (Me.m_order - 1)) {}
                Dim knotValue As Integer = 0
                For i As Integer = 0 To Me.m_order + (Me.m_controlPoints.Count - 1)
                    If i <= Me.m_controlPoints.Count AndAlso i >= Me.m_order Then
                        knotValue += 1
                    End If

                    knots(i) = knotValue / CSng(Me.m_controlPoints.Count - Me.m_order + 1)
                Next
                Return knots
            End SyncLock
        End Function


    End Class
End Namespace
