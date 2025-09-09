

''' <summary>
''' Dxf object subclass string markers.
''' </summary>
Friend NotInheritable Class SubclassMarker
    Private Sub New()
    End Sub
    Public Const ApplicationId As String = "AcDbRegAppTableRecord"
    Public Const Table As String = "AcDbSymbolTable"
    Public Const TableRecord As String = "AcDbSymbolTableRecord"
    Public Const Layer As String = "AcDbLayerTableRecord"
    Public Const ViewPort As String = "AcDbViewportTableRecord"
    Public Const View As String = "AcDbViewTableRecord"
    Public Const LineType As String = "AcDbLinetypeTableRecord"
    Public Const TextStyle As String = "AcDbTextStyleTableRecord"
    Public Const DimensionStyleTable As String = "AcDbDimStyleTable"
    Public Const DimensionStyle As String = "AcDbDimStyleTableRecord"
    Public Const BlockRecord As String = "AcDbBlockTableRecord"
    Public Const BlockBegin As String = "AcDbBlockBegin"
    Public Const BlockEnd As String = "AcDbBlockEnd"
    Public Const Entity As String = "AcDbEntity"
    Public Const Arc As String = "AcDbArc"
    Public Const Circle As String = "AcDbCircle"
    Public Const Ellipse As String = "AcDbEllipse"
    Public Const Face3d As String = "AcDbFace"
    Public Const Insert As String = "AcDbBlockReference"
    Public Const Line As String = "AcDbLine"
    Public Const Point As String = "AcDbPoint"
    Public Const Vertex As String = "AcDbVertex"
    Public Const Polyline As String = "AcDb2dPolyline"
    Public Const LightWeightPolyline As String = "AcDbPolyline"
    Public Const PolylineVertex As String = "AcDb2dVertex "
    Public Const Polyline3d As String = "AcDb3dPolyline"
    Public Const Polyline3dVertex As String = "AcDb3dPolylineVertex"
    Public Const PolyfaceMesh As String = "AcDbPolyFaceMesh"
    Public Const PolyfaceMeshVertex As String = "AcDbPolyFaceMeshVertex"
    Public Const PolyfaceMeshFace As String = "AcDbFaceRecord"
    Public Const Solid As String = "AcDbTrace"
    Public Const Text As String = "AcDbText"
    Public Const Attribute As String = "AcDbAttribute"
    Public Const AttributeDefinition As String = "AcDbAttributeDefinition"
    Public Const Dictionary As String = "AcDbDictionary"
End Class

