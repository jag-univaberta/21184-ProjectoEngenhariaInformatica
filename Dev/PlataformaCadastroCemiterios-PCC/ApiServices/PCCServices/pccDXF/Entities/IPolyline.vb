

Namespace Entities
    ''' <summary>
    ''' Defines the polyline type.
    ''' </summary>
    ''' <remarks>Bit flag.</remarks>
    <Flags()> _
    Public Enum PolylineTypeFlags
        OpenPolyline = 0
        ClosedPolylineOrClosedPolygonMeshInM = 1
        CurveFit = 2
        SplineFit = 4
        Polyline3D = 8
        PolygonMesh = 16
        ClosedPolygonMeshInN = 32
        PolyfaceMesh = 64
        ContinuousLineTypePatter = 128
    End Enum

    ''' <summary>
    ''' Defines the curves and smooth surface type.
    ''' </summary>
    Public Enum SmoothType
        ''' <summary>
        ''' No smooth surface fitted
        ''' </summary>
        NoSmooth = 0
        ''' <summary>
        ''' Quadratic B-spline surface
        ''' </summary>
        Quadratic = 5
        ''' <summary>
        ''' Cubic B-spline surface
        ''' </summary>
        Cubic = 6
        ''' <summary>
        ''' Bezier surface
        ''' </summary>
        Bezier = 8
    End Enum

    ''' <summary>
    ''' Represents a generic polyline.
    ''' </summary>
    Public Interface IPolyline
        Inherits IEntityObject
        ''' <summary>
        ''' Gets the polyline type.
        ''' </summary>
        ReadOnly Property Flags() As PolylineTypeFlags
    End Interface
End Namespace
