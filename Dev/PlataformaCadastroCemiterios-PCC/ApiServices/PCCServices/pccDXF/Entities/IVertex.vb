

Namespace Entities
    ''' <summary>
    ''' Defines the vertex type.
    ''' </summary>
    <Flags()> _
    Public Enum VertexTypeFlags
        ''' <summary>
        ''' 2d polyline vertex
        ''' </summary>
        PolylineVertex = 0
        ''' <summary>
        ''' Extra vertex created by curve-fitting
        ''' </summary>
        CurveFittingExtraVertex = 1
        ''' <summary>
        ''' Curve-fit tangent defined for this vertex.
        ''' A curve-fit tangent direction of 0 may be omitted from DXF output but is significant if this bit is set,
        ''' </summary>
        CurveFitTangent = 2
        ''' <summary>
        ''' Not used
        ''' </summary>
        NotUsed = 4
        ''' <summary>
        ''' Spline vertex created by spline-fitting
        ''' </summary>
        SplineVertexFromSplineFitting = 8
        ''' <summary>
        ''' Spline frame control point
        ''' </summary>
        SplineFrameControlPoint = 16
        ''' <summary>
        ''' 3D polyline vertex
        ''' </summary>
        Polyline3dVertex = 32
        ''' <summary>
        ''' 3D polygon mesh
        ''' </summary>
        Polygon3dMesh = 64
        ''' <summary>
        ''' Polyface mesh vertex
        ''' </summary>
        PolyfaceMeshVertex = 128
    End Enum

    ''' <summary>
    ''' Represents a generic vertex.
    ''' </summary>
    Friend Interface IVertex
        Inherits IEntityObject
        ''' <summary>
        ''' Gets the Vertex type.
        ''' </summary>
        ReadOnly Property Flags() As VertexTypeFlags
    End Interface
End Namespace
