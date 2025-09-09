Imports System.Collections.Generic
Imports pccDXF4.Tables

Namespace Entities

    Public Class Arc
        Inherits DxfObject
        Implements IEntityObject

        Private Const m_TYPE As EntityType = EntityType.Arc
        Private m_center As Vector3f
        Private m_radius As Single
        Private m_startAngle As Single
        Private m_endAngle As Single
        Private m_thickness As Single
        Private m_normal As Vector3f
        Private m_color As AciColor
        Private m_layer As Layer
        Private m_lineType As LineType
        Private m_xData As Dictionary(Of ApplicationRegistry, XData)

        Private Shared ReadOnly _locker As New Object()

        Public Sub New(ByVal center As Vector3f, ByVal radius As Single, ByVal startAngle As Single, ByVal endAngle As Single)
            MyBase.New(DxfObjectCode.Arc)
            SyncLock _locker
                Me.m_center = center
                Me.m_radius = radius
                Me.m_startAngle = startAngle
                Me.m_endAngle = endAngle
                Me.m_thickness = 0.0F
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_normal = Vector3f.UnitZ
            End SyncLock
        End Sub


        Public Sub New()
            MyBase.New(DxfObjectCode.Arc)
            SyncLock _locker
                Me.m_center = Vector3f.Zero
                Me.m_radius = 0.0F
                Me.m_startAngle = 0.0F
                Me.m_endAngle = 0.0F
                Me.m_thickness = 0.0F
                Me.m_layer = Layer.[Default]
                Me.m_color = AciColor.ByLayer
                Me.m_lineType = LineType.ByLayer
                Me.m_normal = Vector3f.UnitZ
            End SyncLock
        End Sub

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

        Public Property Radius() As Single
            Get
                SyncLock _locker
                    Return Me.m_radius
                End SyncLock
            End Get
            Set(ByVal value As Single)
                SyncLock _locker
                    If value <= 0 Then
                        Throw New ArgumentOutOfRangeException("value", value.ToString())
                    End If
                    Me.m_radius = value
                End SyncLock
            End Set
        End Property

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


        Public Property Normal() As Vector3f
            Get
                SyncLock _locker
                    Return Me.m_normal
                End SyncLock
            End Get
            Set(ByVal value As Vector3f)
                SyncLock _locker
                    If Vector3f.Zero = value Then
                        Throw New ArgumentNullException("value", "The normal can not be the zero vector")
                    End If
                    value.Normalize()
                    Me.m_normal = value
                End SyncLock
            End Set
        End Property


        Public ReadOnly Property Type() As EntityType Implements IEntityObject.Type
            Get
                SyncLock _locker
                    Return m_TYPE
                End SyncLock
            End Get
        End Property


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


        Public Function PoligonalVertexes(ByVal precision As Integer) As List(Of Vector2f)
            SyncLock _locker
                If precision < 2 Then
                    Throw New ArgumentOutOfRangeException("precision", precision, "The arc precision must be greater or equal to two")
                End If

                Dim ocsVertexes As New List(Of Vector2f)()
                Dim start As Single = CSng(Me.m_startAngle * MathHelper.DegToRad)
                Dim [end] As Single = CSng(Me.m_endAngle * MathHelper.DegToRad)
                Dim angle As Single = ([end] - start) / precision

                For i As Integer = 0 To precision
                    Dim sine As Single = CSng(Me.m_radius * Math.Sin(start + angle * i))
                    Dim cosine As Single = CSng(Me.m_radius * Math.Cos(start + angle * i))
                    ocsVertexes.Add(New Vector2f(cosine + Me.m_center.X, sine + Me.m_center.Y))
                Next

                Return ocsVertexes
            End SyncLock
        End Function

        Public Function PoligonalVertexes(ByVal precision As Integer, ByVal weldThreshold As Single) As List(Of Vector2f)
            SyncLock _locker
                If precision < 2 Then
                    Throw New ArgumentOutOfRangeException("precision", precision, "The arc precision must be greater or equal to two")
                End If

                Dim ocsVertexes As New List(Of Vector2f)()
                Dim start As Single = CSng(Me.m_startAngle * MathHelper.DegToRad)
                Dim [end] As Single = CSng(Me.m_endAngle * MathHelper.DegToRad)

                If 2 * Me.m_radius >= weldThreshold Then
                    Dim angulo As Single = ([end] - start) / precision
                    Dim prevPoint As Vector2f
                    Dim firstPoint As Vector2f

                    Dim sine As Single = CSng(Me.m_radius * Math.Sin(start))
                    Dim cosine As Single = CSng(Me.m_radius * Math.Cos(start))
                    firstPoint = New Vector2f(cosine + Me.m_center.X, sine + Me.m_center.Y)
                    ocsVertexes.Add(firstPoint)
                    prevPoint = firstPoint

                    For i As Integer = 1 To precision
                        sine = CSng(Me.m_radius * Math.Sin(start + angulo * i))
                        cosine = CSng(Me.m_radius * Math.Cos(start + angulo * i))
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


        Public Overrides Function ToString() As String
            SyncLock _locker
                Return m_TYPE.ToString()
            End SyncLock
        End Function



    End Class
End Namespace
