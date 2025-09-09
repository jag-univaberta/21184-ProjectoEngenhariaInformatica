
Imports System.Collections.Generic
Imports pccDXF4.Tables

Namespace Entities
    ''' <summary>
    ''' Defines the entity type.
    ''' </summary>
    Public Enum EntityType
        ''' <summary>
        ''' line.
        ''' </summary>
        Line

        ''' <summary>
        ''' polyline.
        ''' </summary>
        Polyline

        ''' <summary>
        ''' 3d polyline .
        ''' </summary>
        Polyline3d

        ''' <summary>
        ''' lightweight polyline.
        ''' </summary>
        LightWeightPolyline

        ''' <summary>
        ''' polyface mesh.
        ''' </summary>
        PolyfaceMesh

        ''' <summary>
        ''' circle.
        ''' </summary>
        Circle

        ''' <summary>
        ''' nurbs curve
        ''' </summary>
        NurbsCurve

        ''' <summary>
        ''' ellipse.
        ''' </summary>
        Ellipse

        ''' <summary>
        ''' point.
        ''' </summary>
        Point

        ''' <summary>
        ''' arc.
        ''' </summary>
        Arc

        ''' <summary>
        ''' text string.
        ''' </summary>
        Text

        ''' <summary>
        ''' 3d face.
        ''' </summary>
        Face3D

        ''' <summary>
        ''' solid.
        ''' </summary>
        Solid

        ''' <summary>
        ''' block insertion.
        ''' </summary>
        Insert

        ''' <summary>
        ''' hatch.
        ''' </summary>
        Hatch

        ''' <summary>
        ''' attribute.
        ''' </summary>
        Attribute

        ''' <summary>
        ''' attribute definition.
        ''' </summary>
        AttributeDefinition

        ''' <summary>
        ''' lightweight polyline vertex.
        ''' </summary>
        LightWeightPolylineVertex

        ''' <summary>
        ''' polyline vertex.
        ''' </summary>
        PolylineVertex

        ''' <summary>
        ''' polyline 3d vertex.
        ''' </summary>
        Polyline3dVertex

        ''' <summary>
        ''' polyface mesh vertex.
        ''' </summary>
        PolyfaceMeshVertex

        ''' <summary>
        ''' polyface mesh face.
        ''' </summary>
        PolyfaceMeshFace

        ''' <summary>
        ''' dim.
        ''' </summary>
        Dimension

        ''' <summary>
        ''' A generi Vertex
        ''' </summary>
        Vertex
    End Enum

    ''' <summary>
    ''' Represents a generic entity.
    ''' </summary>
    Public Interface IEntityObject
        ''' <summary>
        ''' Gets the entity <see cref="EntityType">type</see>.
        ''' </summary>
        ReadOnly Property Type() As EntityType

        ''' <summary>
        ''' Gets or sets the entity <see cref="AciColor">color</see>.
        ''' </summary>
        Property Color() As AciColor

        ''' <summary>
        ''' Gets or sets the entity <see cref="Layer">layer</see>.
        ''' </summary>
        Property Layer() As Layer

        ''' <summary>
        ''' Gets or sets the entity <see cref="LineType">line type</see.
        ''' </summary>
        Property LineType() As LineType

        ''' <summary>
        ''' Gets or sets the entity <see cref="XData">extended data</see.
        ''' </summary>
        Property XData() As Dictionary(Of ApplicationRegistry, XData)
    End Interface
End Namespace
