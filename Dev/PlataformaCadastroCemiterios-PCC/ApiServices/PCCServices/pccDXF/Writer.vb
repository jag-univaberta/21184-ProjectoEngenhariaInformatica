Imports System.Collections.Generic
Imports System.Globalization
Imports System.IO
Imports System.Text
Imports System.Threading
Imports pccDXF4.Blocks
Imports pccDXF4.Entities
Imports pccDXF4.Header
Imports pccDXF4.Objects
Imports pccDXF4.Tables
Imports Attribute = pccDXF4.Entities.Attribute


''' <summary>
''' Low level dxf writer.
''' </summary>
Friend NotInheritable Class DxfWriter


    Private reservedHandles As Integer = 10
    Private ReadOnly file As String
    Private m_isFileOpen As Boolean
    Private m_activeSection As String = StringCode.Unknown
    Private activeTable As String = StringCode.Unknown
    Private isHeader As Boolean
    Private isClassesSection As Boolean
    Private isTableSection As Boolean
    Private isBlockDefinition As Boolean
    Private isBlockEntities As Boolean
    Private isEntitiesSection As Boolean
    Private isObjectsSection As Boolean
    Private output As Stream
    Private writer As StreamWriter
    Private ReadOnly version As DxfVersion

     
    Private Shared ReadOnly _locker As New Object()

    Public Sub New(ByVal file As String, ByVal version As DxfVersion)
        SyncLock _locker
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture
            Me.file = file
            Me.version = version
        End SyncLock
    End Sub





    ''' <summary>
    ''' Gets the active section.
    ''' </summary>
    Public ReadOnly Property ActiveSection() As [String]
        Get
            SyncLock _locker
                Return Me.m_activeSection
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Gets if the file is opent.
    ''' </summary>
    Public ReadOnly Property IsFileOpen() As Boolean
        Get
            SyncLock _locker
                Return Me.m_isFileOpen
            End SyncLock
        End Get
    End Property



    Public Sub Open()
        SyncLock _locker
            If Me.m_isFileOpen Then
                Throw New DxfException(Me.file, "The file is already open")
            End If
            Try
                Me.output = System.IO.File.Create(Me.file)
                Me.writer = New StreamWriter(Me.output, Encoding.ASCII)
                Me.m_isFileOpen = True
            Catch ex As Exception
                Throw (New DxfException(Me.file, "Error when trying to create the dxf file", ex))
            End Try
        End SyncLock
    End Sub

    ''' <summary>
    ''' Closes the dxf file.
    ''' </summary>
    Public Sub Close()
        SyncLock _locker
            If Me.m_activeSection <> StringCode.Unknown Then
                Throw New OpenDxfSectionException(Me.m_activeSection, Me.file)
            End If
            If Me.activeTable <> StringCode.Unknown Then
                Throw New OpenDxfTableException(Me.activeTable, Me.file)
            End If
            Me.WriteCodePair(0, StringCode.EndOfFile)

            If Me.m_isFileOpen Then
                Me.writer.Close()
                Me.output.Close()
            End If

            Me.m_isFileOpen = False
        End SyncLock
    End Sub

    ''' <summary>
    ''' Opens a new section.
    ''' </summary>
    ''' <param name="section">Section type to open.</param>
    ''' <remarks>There can be only one type section.</remarks>
    Public Sub BeginSection(ByVal section As String)
        SyncLock _locker
            If Not Me.m_isFileOpen Then
                Throw New DxfException(Me.file, "The file is not open")
            End If
            If Me.m_activeSection <> StringCode.Unknown Then
                Throw New OpenDxfSectionException(Me.m_activeSection, Me.file)
            End If

            Me.WriteCodePair(0, StringCode.BeginSection)

            If section = StringCode.HeaderSection Then
                If Me.isHeader Then
                    Throw (New ClosedDxfSectionException(StringCode.HeaderSection, Me.file))
                End If
                Me.WriteCodePair(2, StringCode.HeaderSection)
                Me.isHeader = True
            End If
            If section = StringCode.ClassesSection Then
                If Me.isClassesSection Then
                    Throw (New ClosedDxfSectionException(StringCode.ClassesSection, Me.file))
                End If
                Me.WriteCodePair(2, StringCode.ClassesSection)
                Me.isClassesSection = True
            End If
            If section = StringCode.TablesSection Then
                If Me.isTableSection Then
                    Throw (New ClosedDxfSectionException(StringCode.TablesSection, Me.file))
                End If
                Me.WriteCodePair(2, StringCode.TablesSection)
                Me.isTableSection = True
            End If
            If section = StringCode.BlocksSection Then
                If Me.isBlockDefinition Then
                    Throw (New ClosedDxfSectionException(StringCode.BlocksSection, Me.file))
                End If
                Me.WriteCodePair(2, StringCode.BlocksSection)
                Me.isBlockDefinition = True
            End If
            If section = StringCode.EntitiesSection Then
                If Me.isEntitiesSection Then
                    Throw (New ClosedDxfSectionException(StringCode.EntitiesSection, Me.file))
                End If
                Me.WriteCodePair(2, StringCode.EntitiesSection)
                Me.isEntitiesSection = True
            End If
            If section = StringCode.ObjectsSection Then
                If Me.isObjectsSection Then
                    Throw (New ClosedDxfSectionException(StringCode.ObjectsSection, Me.file))
                End If
                Me.WriteCodePair(2, StringCode.ObjectsSection)
                Me.isObjectsSection = True
            End If
            Me.m_activeSection = section
        End SyncLock
    End Sub

    ''' <summary>
    ''' Closes the active section.
    ''' </summary>
    Public Sub EndSection()
        SyncLock _locker
            If Me.m_activeSection = StringCode.Unknown Then
                Throw New ClosedDxfSectionException(StringCode.Unknown, Me.file)
            End If
            Me.WriteCodePair(0, StringCode.EndSection)
            Select Case Me.m_activeSection
                Case StringCode.HeaderSection
                    Me.isEntitiesSection = False
                    Exit Select
                Case StringCode.ClassesSection
                    Me.isEntitiesSection = False
                    Exit Select
                Case StringCode.TablesSection
                    Me.isTableSection = False
                    Exit Select
                Case StringCode.BlocksSection
                    Me.isBlockDefinition = True
                    Exit Select
                Case StringCode.EntitiesSection
                    Me.isEntitiesSection = False
                    Exit Select
                Case StringCode.ObjectsSection
                    Me.isEntitiesSection = False
                    Exit Select
            End Select
            Me.m_activeSection = StringCode.Unknown
        End SyncLock
    End Sub

    ''' <summary>
    ''' Opens a new table.
    ''' </summary>
    ''' <param name="table">Table type to open.</param>
    Public Sub BeginTable(ByVal table As String)
        SyncLock _locker
            If Not Me.m_isFileOpen Then
                Throw New DxfException(Me.file, "The file is not open")
            End If
            If Me.activeTable <> StringCode.Unknown Then
                Throw New OpenDxfTableException(table, Me.file)
            End If
            Me.WriteCodePair(0, StringCode.Table)
            Me.WriteCodePair(2, table)
            Me.WriteCodePair(5, System.Math.Max(System.Threading.Interlocked.Increment(Me.reservedHandles), Me.reservedHandles - 1))
            Me.WriteCodePair(100, SubclassMarker.Table)

            If table = StringCode.DimensionStyleTable Then
                Me.WriteCodePair(100, SubclassMarker.DimensionStyleTable)
            End If
            Me.activeTable = table
        End SyncLock
    End Sub

    ''' <summary>
    ''' Closes the active table.
    ''' </summary>
    Public Sub EndTable()
        SyncLock _locker
            If Me.activeTable = StringCode.Unknown Then
                Throw New ClosedDxfTableException(StringCode.Unknown, Me.file)
            End If

            Me.WriteCodePair(0, StringCode.EndTable)
            Me.activeTable = StringCode.Unknown
        End SyncLock
    End Sub



    Public Sub WriteComment(ByVal comment As String)
        SyncLock _locker
            If Not String.IsNullOrEmpty(comment) Then
                Me.WriteCodePair(999, comment)
            End If
        End SyncLock
    End Sub

    Public Sub WriteSystemVariable(ByVal variable As HeaderVariable)
        SyncLock _locker
            If Me.m_activeSection <> StringCode.HeaderSection Then
                Throw New InvalidDxfSectionException(Me.m_activeSection, Me.file)
            End If
            Me.WriteCodePair(HeaderVariable.NAME_CODE_GROUP, variable.Name)
            Me.WriteCodePair(variable.CodeGroup, variable.Value)
        End SyncLock
    End Sub




    ''' <summary>
    ''' Writes a new extended data application registry to the table section.
    ''' </summary>
    ''' <param name="appReg">Nombre del registro de aplicación.</param>
    Public Sub RegisterApplication(ByVal appReg As ApplicationRegistry)
        SyncLock _locker
            If Me.activeTable <> StringCode.ApplicationIDTable Then
                Throw New InvalidDxfTableException(StringCode.ApplicationIDTable, Me.file)
            End If

            Me.WriteCodePair(0, StringCode.ApplicationIDTable)
            Me.WriteCodePair(5, appReg.Handle)
            Me.WriteCodePair(100, SubclassMarker.TableRecord)
            Me.WriteCodePair(100, SubclassMarker.ApplicationId)
            Me.WriteCodePair(2, appReg)
            Me.WriteCodePair(70, 0)
        End SyncLock
    End Sub

    ''' <summary>
    ''' Writes a new view port to the table section.
    ''' </summary>
    ''' <param name="vp">Viewport.</param>
    Public Sub WriteViewPort(ByVal vp As ViewPort)
        SyncLock _locker
            If Me.activeTable <> StringCode.ViewPortTable Then
                Throw New InvalidDxfTableException(Me.activeTable, Me.file)
            End If
            Me.WriteCodePair(0, vp.CodeName)
            Me.WriteCodePair(5, vp.Handle)
            Me.WriteCodePair(100, SubclassMarker.TableRecord)

            Me.WriteCodePair(100, SubclassMarker.ViewPort)
            Me.WriteCodePair(2, vp)
            Me.WriteCodePair(70, 0)

            Me.WriteCodePair(10, vp.LowerLeftCorner.X)
            Me.WriteCodePair(20, vp.LowerLeftCorner.Y)

            Me.WriteCodePair(11, vp.UpperRightCorner.X)
            Me.WriteCodePair(21, vp.UpperRightCorner.Y)

            Me.WriteCodePair(12, vp.LowerLeftCorner.X - vp.UpperRightCorner.X)
            Me.WriteCodePair(22, vp.UpperRightCorner.Y - vp.LowerLeftCorner.Y)

            Me.WriteCodePair(13, vp.SnapBasePoint.X)
            Me.WriteCodePair(23, vp.SnapBasePoint.Y)

            Me.WriteCodePair(14, vp.SnapSpacing.X)
            Me.WriteCodePair(24, vp.SnapSpacing.Y)

            Me.WriteCodePair(15, vp.GridSpacing.X)
            Me.WriteCodePair(25, vp.GridSpacing.Y)

            Dim dir As Vector3f = vp.Camera - vp.Target
            Me.WriteCodePair(16, dir.X)
            Me.WriteCodePair(26, dir.Y)
            Me.WriteCodePair(36, dir.Z)

            Me.WriteCodePair(17, vp.Target.X)
            Me.WriteCodePair(27, vp.Target.Y)
            Me.WriteCodePair(37, vp.Target.Z)
        End SyncLock
    End Sub

    ''' <summary>
    ''' Writes a new dimension style to the table section.
    ''' </summary>
    ''' <param name="dimStyle">DimensionStyle.</param>
    Public Sub WriteDimensionStyle(ByVal dimStyle As DimensionStyle)
        SyncLock _locker
            If Me.activeTable <> StringCode.DimensionStyleTable Then
                Throw New InvalidDxfTableException(Me.activeTable, Me.file)
            End If
            Me.WriteCodePair(0, dimStyle.CodeName)
            Me.WriteCodePair(105, dimStyle.Handle)

            Me.WriteCodePair(100, SubclassMarker.TableRecord)

            Me.WriteCodePair(100, SubclassMarker.DimensionStyle)

            Me.WriteCodePair(2, dimStyle)

            ' flags
            Me.WriteCodePair(70, 0)
        End SyncLock
    End Sub

    ''' <summary>
    ''' Writes a new block record to the table section.
    ''' </summary>
    ''' <param name="blockRecord">Block.</param>
    Public Sub WriteBlockRecord(ByVal blockRecord As BlockRecord)
        SyncLock _locker
            If Me.activeTable <> StringCode.BlockRecordTable Then
                Throw New InvalidDxfTableException(Me.activeTable, Me.file)
            End If
            Me.WriteCodePair(0, blockRecord.CodeName)
            Me.WriteCodePair(5, blockRecord.Handle)
            Me.WriteCodePair(100, SubclassMarker.TableRecord)

            Me.WriteCodePair(100, SubclassMarker.BlockRecord)

            Me.WriteCodePair(2, blockRecord)
        End SyncLock
    End Sub

    ''' <summary>
    ''' Writes a new line type to the table section.
    ''' </summary>
    ''' <param name="tl">Line type.</param>
    Public Sub WriteLineType(ByVal tl As LineType)
        SyncLock _locker
            If Me.version = DxfVersion.AutoCad12 Then
                If tl.Name = "ByLayer" OrElse tl.Name = "ByBlock" Then
                    Return
                End If
            End If

            If Me.activeTable <> StringCode.LineTypeTable Then
                Throw New InvalidDxfTableException(Me.activeTable, Me.file)
            End If

            Me.WriteCodePair(0, tl.CodeName)
            Me.WriteCodePair(5, tl.Handle)
            Me.WriteCodePair(100, SubclassMarker.TableRecord)

            Me.WriteCodePair(100, SubclassMarker.LineType)

            Me.WriteCodePair(70, 0)
            Me.WriteCodePair(2, tl)
            Me.WriteCodePair(3, tl.Description)
            Me.WriteCodePair(72, 65)
            Me.WriteCodePair(73, tl.Segments.Count)
            Me.WriteCodePair(40, tl.Legth())
            For Each s As Single In tl.Segments
                Me.WriteCodePair(49, s)
                If Me.version <> DxfVersion.AutoCad12 Then
                    Me.WriteCodePair(74, 0)
                End If
            Next
        End SyncLock
    End Sub

    ''' <summary>
    ''' Writes a new layer to the table section.
    ''' </summary>
    ''' <param name="layer">Layer.</param>
    Public Sub WriteLayer(ByVal layer__1 As Layer)
        SyncLock _locker
            If Me.activeTable <> StringCode.LayerTable Then
                Throw New InvalidDxfTableException(Me.activeTable, Me.file)
            End If

            Me.WriteCodePair(0, layer__1.CodeName)
            Me.WriteCodePair(5, layer__1.Handle)
            Me.WriteCodePair(100, SubclassMarker.TableRecord)

            Me.WriteCodePair(100, SubclassMarker.Layer)
            Me.WriteCodePair(70, 0)
            Me.WriteCodePair(2, layer__1)

            'a negative color represents a hidden layer.
            If layer__1.IsVisible Then
                Me.WriteCodePair(62, layer__1.Color.Index)
            Else
                Me.WriteCodePair(62, -layer__1.Color.Index)
            End If

            Me.WriteCodePair(6, layer__1.LineType.Name)
            If Me.version <> DxfVersion.AutoCad12 Then
                Me.WriteCodePair(390, Layer.PlotStyleHandle)
            End If
        End SyncLock
    End Sub

    ''' <summary>
    ''' Writes a new text style to the table section.
    ''' </summary>
    ''' <param name="style">TextStyle.</param>
    Public Sub WriteTextStyle(ByVal style As TextStyle)
        SyncLock _locker
            If Me.activeTable <> StringCode.TextStyleTable Then
                Throw New InvalidDxfTableException(Me.activeTable, Me.file)
            End If

            Me.WriteCodePair(0, style.CodeName)
            Me.WriteCodePair(5, style.Handle)
            Me.WriteCodePair(100, SubclassMarker.TableRecord)

            Me.WriteCodePair(100, SubclassMarker.TextStyle)

            Me.WriteCodePair(2, style)
            Me.WriteCodePair(3, style.Font)

            If style.IsVertical Then
                Me.WriteCodePair(70, 4)
            Else
                Me.WriteCodePair(70, 0)
            End If

            If style.IsBackward AndAlso style.IsUpsideDown Then
                Me.WriteCodePair(71, 6)
            ElseIf style.IsBackward Then
                Me.WriteCodePair(71, 2)
            ElseIf style.IsUpsideDown Then
                Me.WriteCodePair(71, 4)
            Else
                Me.WriteCodePair(71, 0)
            End If

            Me.WriteCodePair(40, style.Height)
            Me.WriteCodePair(41, style.WidthFactor)
            Me.WriteCodePair(42, style.Height)
            Me.WriteCodePair(50, style.ObliqueAngle)
        End SyncLock
    End Sub


    Public Sub WriteBlock(ByVal block As Block, ByVal entityObjects As List(Of IEntityObject))
        SyncLock _locker
            If Me.version = DxfVersion.AutoCad12 Then
                If block.Name = "*Model_Space" OrElse block.Name = "*Paper_Space" Then
                    Return
                End If
            End If

            If Me.m_activeSection <> StringCode.BlocksSection Then
                Throw New InvalidDxfSectionException(Me.m_activeSection, Me.file)
            End If

            Me.WriteCodePair(0, block.CodeName)
            Me.WriteCodePair(5, block.Handle)
            Me.WriteCodePair(100, SubclassMarker.Entity)
            Me.WriteCodePair(8, block.Layer)

            Me.WriteCodePair(100, SubclassMarker.BlockBegin)

            Me.WriteCodePair(2, block)

            'flags
            If block.Attributes.Count = 0 Then
                Me.WriteCodePair(70, 0)
            Else
                Me.WriteCodePair(70, 2)
            End If

            Me.WriteCodePair(10, block.BasePoint.X)
            Me.WriteCodePair(20, block.BasePoint.Y)
            Me.WriteCodePair(30, block.BasePoint.Z)

            Me.WriteCodePair(3, block)

            For Each attdef As AttributeDefinition In block.Attributes.Values
                Me.WriteAttributeDefinition(attdef)
            Next

            'block entities, if version is AutoCad12 we will write the converted entities
            Me.isBlockEntities = True
            For Each entity As IEntityObject In entityObjects
                Me.WriteEntity(entity)
            Next
            Me.isBlockEntities = False

            Me.WriteBlockEnd(block.[End])
        End SyncLock
    End Sub

    Public Sub WriteBlockEnd(ByVal blockEnd As BlockEnd)
        SyncLock _locker
            Me.WriteCodePair(0, blockEnd.CodeName)
            Me.WriteCodePair(5, blockEnd.Handle)
            Me.WriteCodePair(100, SubclassMarker.Entity)
            Me.WriteCodePair(8, blockEnd.Layer)

            Me.WriteCodePair(100, SubclassMarker.BlockEnd)
        End SyncLock
    End Sub

    Public Sub WriteEntity(ByVal entity As IEntityObject)
        SyncLock _locker
            Select Case entity.Type
                Case EntityType.Arc
                    Me.WriteArc(DirectCast(entity, Arc))
                    Exit Select
                Case EntityType.Circle
                    Me.WriteCircle(DirectCast(entity, Circle))
                    Exit Select
                Case EntityType.Ellipse
                    Me.WriteEllipse(DirectCast(entity, Ellipse))
                    Exit Select
                Case EntityType.NurbsCurve
                    Me.WriteNurbsCurve(DirectCast(entity, NurbsCurve))
                    Exit Select
                Case EntityType.Point
                    Me.WritePoint(DirectCast(entity, Point))
                    Exit Select
                Case EntityType.Face3D
                    Me.WriteFace3D(DirectCast(entity, Face3d))
                    Exit Select
                Case EntityType.Solid
                    Me.WriteSolid(DirectCast(entity, Solid))
                    Exit Select
                Case EntityType.Insert
                    Me.WriteInsert(DirectCast(entity, Insert))
                    Exit Select
                Case EntityType.Line
                    Me.WriteLine(DirectCast(entity, Line))
                    Exit Select
                Case EntityType.LightWeightPolyline
                    Me.WriteLightWeightPolyline(DirectCast(entity, LightWeightPolyline))
                    Exit Select
                Case EntityType.Polyline
                    Me.WritePolyline2d(DirectCast(entity, Polyline))
                    Exit Select
                Case EntityType.Polyline3d
                    Me.WritePolyline3d(DirectCast(entity, Polyline3d))
                    Exit Select
                Case EntityType.PolyfaceMesh
                    Me.WritePolyfaceMesh(DirectCast(entity, PolyfaceMesh))
                    Exit Select
                Case EntityType.Text
                    Me.WriteText(DirectCast(entity, Text))
                    Exit Select
                Case Else
                    Throw New NotImplementedException(entity.Type.ToString())
            End Select
        End SyncLock
    End Sub

    Private Sub WriteArc(ByVal arc As Arc)
        SyncLock _locker
            If Me.m_activeSection <> StringCode.EntitiesSection AndAlso Not Me.isBlockEntities Then
                Throw New InvalidDxfSectionException(Me.m_activeSection, Me.file)
            End If

            Me.WriteCodePair(0, arc.CodeName)
            Me.WriteCodePair(5, arc.Handle)
            Me.WriteCodePair(100, SubclassMarker.Entity)
            Me.WriteEntityCommonCodes(arc)
            Me.WriteCodePair(100, SubclassMarker.Circle)

            Me.WriteCodePair(39, arc.Thickness)

            Me.WriteCodePair(10, arc.Center.X)
            Me.WriteCodePair(20, arc.Center.Y)
            Me.WriteCodePair(30, arc.Center.Z)

            Me.WriteCodePair(40, arc.Radius)

            Me.WriteCodePair(210, arc.Normal.X)
            Me.WriteCodePair(220, arc.Normal.Y)
            Me.WriteCodePair(230, arc.Normal.Z)

            Me.WriteCodePair(100, SubclassMarker.Arc)
            Me.WriteCodePair(50, arc.StartAngle)
            Me.WriteCodePair(51, arc.EndAngle)

            Me.WriteXData(arc.XData)
        End SyncLock
    End Sub

    Private Sub WriteCircle(ByVal circle As Circle)
        SyncLock _locker
            If Me.m_activeSection <> StringCode.EntitiesSection AndAlso Not Me.isBlockEntities Then
                Throw New InvalidDxfSectionException(Me.m_activeSection, Me.file)
            End If

            Me.WriteCodePair(0, circle.CodeName)
            Me.WriteCodePair(5, circle.Handle)
            Me.WriteCodePair(100, SubclassMarker.Entity)
            Me.WriteEntityCommonCodes(circle)
            Me.WriteCodePair(100, SubclassMarker.Circle)


            Me.WriteCodePair(10, circle.Center.X)
            Me.WriteCodePair(20, circle.Center.Y)
            Me.WriteCodePair(30, circle.Center.Z)

            Me.WriteCodePair(40, circle.Radius)

            Me.WriteCodePair(39, circle.Thickness)

            Me.WriteCodePair(210, circle.Normal.X)
            Me.WriteCodePair(220, circle.Normal.Y)
            Me.WriteCodePair(230, circle.Normal.Z)

            Me.WriteXData(circle.XData)
        End SyncLock
    End Sub

    Private Sub WriteEllipse(ByVal ellipse As Ellipse)
        SyncLock _locker
            If Me.m_activeSection <> StringCode.EntitiesSection AndAlso Not Me.isBlockEntities Then
                Throw New InvalidDxfSectionException(Me.m_activeSection, Me.file)
            End If

            If Me.version = DxfVersion.AutoCad12 Then
                Me.WriteEllipseAsPolyline(ellipse)
                Return
            End If

            Me.WriteCodePair(0, ellipse.CodeName)
            Me.WriteCodePair(5, ellipse.Handle)
            Me.WriteCodePair(100, SubclassMarker.Entity)
            Me.WriteEntityCommonCodes(ellipse)
            Me.WriteCodePair(100, SubclassMarker.Ellipse)


            Me.WriteCodePair(10, ellipse.Center.X)
            Me.WriteCodePair(20, ellipse.Center.Y)
            Me.WriteCodePair(30, ellipse.Center.Z)


            Dim sine As Single = CSng(0.5 * ellipse.MajorAxis * Math.Sin(ellipse.Rotation * MathHelper.DegToRad))
            Dim cosine As Single = CSng(0.5 * ellipse.MajorAxis * Math.Cos(ellipse.Rotation * MathHelper.DegToRad))
            Dim axisPoint As Vector3d = MathHelper.Transform(CType(New Vector3f(cosine, sine, 0), Vector3d), CType(ellipse.Normal, Vector3d), MathHelper.CoordinateSystem.[Object], MathHelper.CoordinateSystem.World)

            Me.WriteCodePair(11, axisPoint.X)
            Me.WriteCodePair(21, axisPoint.Y)
            Me.WriteCodePair(31, axisPoint.Z)

            Me.WriteCodePair(210, ellipse.Normal.X)
            Me.WriteCodePair(220, ellipse.Normal.Y)
            Me.WriteCodePair(230, ellipse.Normal.Z)

            Me.WriteCodePair(40, ellipse.MinorAxis / ellipse.MajorAxis)
            Me.WriteCodePair(41, ellipse.StartAngle * MathHelper.DegToRad)
            Me.WriteCodePair(42, ellipse.EndAngle * MathHelper.DegToRad)

            Me.WriteXData(ellipse.XData)
        End SyncLock
    End Sub

    Private Sub WriteEllipseAsPolyline(ByVal ellipse As Ellipse)
        SyncLock _locker
            'we will draw the ellipse as a polyline, it is not supported in AutoCad12 dxf files
            Me.WriteCodePair(0, DxfObjectCode.Polyline)

            Me.WriteEntityCommonCodes(ellipse)

            'closed polyline
            Me.WriteCodePair(70, 1)

            'dummy point
            Me.WriteCodePair(10, 0.0F)
            Me.WriteCodePair(20, 0.0F)
            Me.WriteCodePair(30, ellipse.Center.Z)

            Me.WriteCodePair(39, ellipse.Thickness)

            Me.WriteCodePair(210, ellipse.Normal.X)
            Me.WriteCodePair(220, ellipse.Normal.Y)
            Me.WriteCodePair(230, ellipse.Normal.Z)

            'Obsolete; formerly an “entities follow flag” (optional; ignore if present)
            'but its needed to load the dxf file in AutoCAD
            Me.WriteCodePair(66, "1")

            Me.WriteXData(ellipse.XData)

            Dim points As List(Of Vector2f) = ellipse.PolygonalVertexes(ellipse.CurvePoints)
            For Each v As Vector2f In points
                Me.WriteCodePair(0, DxfObjectCode.Vertex)
                Me.WriteCodePair(8, ellipse.Layer)
                Me.WriteCodePair(70, 0)
                Me.WriteCodePair(10, v.X)
                Me.WriteCodePair(20, v.Y)
            Next
            Me.WriteCodePair(0, StringCode.EndSequence)
        End SyncLock
    End Sub

    Private Sub WriteNurbsCurve(ByVal nurbsCurve As NurbsCurve)
        SyncLock _locker
            If Me.m_activeSection <> StringCode.EntitiesSection AndAlso Not Me.isBlockEntities Then
                Throw New InvalidDxfSectionException(Me.m_activeSection, Me.file)
            End If


            'we will draw the nurbsCurve as a polyline, it is not supported in AutoCad12 dxf files
            Me.WriteCodePair(0, DxfObjectCode.Polyline)

            Me.WriteEntityCommonCodes(nurbsCurve)

            'open polyline
            Me.WriteCodePair(70, 0)

            'dummy point
            Me.WriteCodePair(10, 0.0F)
            Me.WriteCodePair(20, 0.0F)
            Me.WriteCodePair(30, nurbsCurve.Elevation)

            Me.WriteCodePair(39, nurbsCurve.Thickness)

            Me.WriteCodePair(210, nurbsCurve.Normal.X)
            Me.WriteCodePair(220, nurbsCurve.Normal.Y)
            Me.WriteCodePair(230, nurbsCurve.Normal.Z)

            'Obsolete; formerly an “entities follow flag” (optional; ignore if present)
            'but its needed to load the dxf file in AutoCAD
            Me.WriteCodePair(66, "1")

            Me.WriteXData(nurbsCurve.XData)

            Dim points As List(Of Vector2f) = nurbsCurve.PolygonalVertexes(nurbsCurve.CurvePoints)
            For Each v As Vector2f In points
                Me.WriteCodePair(0, DxfObjectCode.Vertex)
                Me.WriteCodePair(8, nurbsCurve.Layer)
                Me.WriteCodePair(70, 0)
                Me.WriteCodePair(10, v.X)
                Me.WriteCodePair(20, v.Y)
            Next
            Me.WriteCodePair(0, StringCode.EndSequence)
        End SyncLock
    End Sub

    Private Sub WriteSolid(ByVal solid As Solid)
        SyncLock _locker
            If Me.m_activeSection <> StringCode.EntitiesSection AndAlso Not Me.isBlockEntities Then
                Throw New InvalidDxfSectionException(Me.m_activeSection, Me.file)
            End If

            Me.WriteCodePair(0, solid.CodeName)
            Me.WriteCodePair(5, solid.Handle)
            Me.WriteCodePair(100, SubclassMarker.Entity)
            Me.WriteEntityCommonCodes(solid)
            Me.WriteCodePair(100, SubclassMarker.Solid)

            Me.WriteCodePair(10, solid.FirstVertex.X)
            Me.WriteCodePair(20, solid.FirstVertex.Y)
            Me.WriteCodePair(30, solid.FirstVertex.Z)

            Me.WriteCodePair(11, solid.SecondVertex.X)
            Me.WriteCodePair(21, solid.SecondVertex.Y)
            Me.WriteCodePair(31, solid.SecondVertex.Z)

            Me.WriteCodePair(12, solid.ThirdVertex.X)
            Me.WriteCodePair(22, solid.ThirdVertex.Y)
            Me.WriteCodePair(32, solid.ThirdVertex.Z)

            Me.WriteCodePair(13, solid.FourthVertex.X)
            Me.WriteCodePair(23, solid.FourthVertex.Y)
            Me.WriteCodePair(33, solid.FourthVertex.Z)

            Me.WriteCodePair(39, solid.Thickness)

            Me.WriteCodePair(210, solid.Normal.X)
            Me.WriteCodePair(220, solid.Normal.Y)
            Me.WriteCodePair(230, solid.Normal.Z)

            Me.WriteXData(solid.XData)
        End SyncLock
    End Sub

    Private Sub WriteFace3D(ByVal face As Face3d)
        SyncLock _locker
            If Me.m_activeSection <> StringCode.EntitiesSection AndAlso Not Me.isBlockEntities Then
                Throw New InvalidDxfSectionException(Me.m_activeSection, Me.file)
            End If

            Me.WriteCodePair(0, face.CodeName)
            Me.WriteCodePair(5, face.Handle)
            Me.WriteCodePair(100, SubclassMarker.Entity)
            Me.WriteEntityCommonCodes(face)
            Me.WriteCodePair(100, SubclassMarker.Face3d)

            Me.WriteCodePair(10, face.FirstVertex.X)
            Me.WriteCodePair(20, face.FirstVertex.Y)
            Me.WriteCodePair(30, face.FirstVertex.Z)

            Me.WriteCodePair(11, face.SecondVertex.X)
            Me.WriteCodePair(21, face.SecondVertex.Y)
            Me.WriteCodePair(31, face.SecondVertex.Z)

            Me.WriteCodePair(12, face.ThirdVertex.X)
            Me.WriteCodePair(22, face.ThirdVertex.Y)
            Me.WriteCodePair(32, face.ThirdVertex.Z)

            Me.WriteCodePair(13, face.FourthVertex.X)
            Me.WriteCodePair(23, face.FourthVertex.Y)
            Me.WriteCodePair(33, face.FourthVertex.Z)

            Me.WriteCodePair(70, Convert.ToInt32(face.EdgeFlags))

            Me.WriteXData(face.XData)
        End SyncLock
    End Sub

    Private Sub WriteInsert(ByVal insert As Insert)
        SyncLock _locker
            If Me.m_activeSection <> StringCode.EntitiesSection AndAlso Not Me.isBlockEntities Then
                Throw New InvalidDxfSectionException(Me.m_activeSection, Me.file)
            End If

            Me.WriteCodePair(0, insert.CodeName)
            Me.WriteCodePair(5, insert.Handle)
            Me.WriteCodePair(100, SubclassMarker.Entity)
            Me.WriteEntityCommonCodes(insert)
            Me.WriteCodePair(100, SubclassMarker.Insert)

            Me.WriteCodePair(2, insert.Block)

            Me.WriteCodePair(10, insert.InsertionPoint.X)
            Me.WriteCodePair(20, insert.InsertionPoint.Y)
            Me.WriteCodePair(30, insert.InsertionPoint.Z)

            Me.WriteCodePair(41, insert.Scale.X)
            Me.WriteCodePair(42, insert.Scale.Y)
            Me.WriteCodePair(43, insert.Scale.Z)

            Me.WriteCodePair(50, insert.Rotation)

            Me.WriteCodePair(210, insert.Normal.X)
            Me.WriteCodePair(220, insert.Normal.Y)
            Me.WriteCodePair(230, insert.Normal.Z)

            If insert.Attributes.Count > 0 Then
                'Obsolete; formerly an “entities follow flag” (optional; ignore if present)
                'but its needed to load the dxf file in AutoCAD
                Me.WriteCodePair(66, "1")

                Me.WriteXData(insert.XData)

                For Each attrib As Attribute In insert.Attributes
                    Me.WriteAttribute(attrib, insert.InsertionPoint)
                Next

                Me.WriteCodePair(0, insert.EndSequence.CodeName)
                Me.WriteCodePair(5, insert.EndSequence.Handle)
                Me.WriteCodePair(100, SubclassMarker.Entity)
                Me.WriteCodePair(8, insert.EndSequence.Layer)
            Else
                Me.WriteXData(insert.XData)
            End If
        End SyncLock
    End Sub

    Private Sub WriteLine(ByVal line As Line)
        SyncLock _locker
            If Me.m_activeSection <> StringCode.EntitiesSection AndAlso Not Me.isBlockEntities Then
                Throw New InvalidDxfSectionException(Me.m_activeSection, Me.file)
            End If

            Me.WriteCodePair(0, line.CodeName)
            Me.WriteCodePair(100, SubclassMarker.Entity)
            Me.WriteEntityCommonCodes(line)
            Me.WriteCodePair(5, line.Handle)
            Me.WriteCodePair(100, SubclassMarker.Line)

            Me.WriteCodePair(10, line.StartPoint.X)
            Me.WriteCodePair(20, line.StartPoint.Y)
            Me.WriteCodePair(30, line.StartPoint.Z)

            Me.WriteCodePair(11, line.EndPoint.X)
            Me.WriteCodePair(21, line.EndPoint.Y)
            Me.WriteCodePair(31, line.EndPoint.Z)

            Me.WriteCodePair(39, line.Thickness)

            Me.WriteCodePair(210, line.Normal.X)
            Me.WriteCodePair(220, line.Normal.Y)
            Me.WriteCodePair(230, line.Normal.Z)

            Me.WriteXData(line.XData)
        End SyncLock
    End Sub

    Private Sub WritePolyline2d(ByVal polyline As Polyline)
        SyncLock _locker
            If Me.m_activeSection <> StringCode.EntitiesSection AndAlso Not Me.isBlockEntities Then
                Throw New InvalidDxfSectionException(Me.m_activeSection, Me.file)
            End If

            Me.WriteCodePair(0, polyline.CodeName)
            Me.WriteCodePair(100, SubclassMarker.Entity)
            Me.WriteEntityCommonCodes(polyline)
            Me.WriteCodePair(5, polyline.Handle)
            Me.WriteCodePair(100, SubclassMarker.Polyline)

            Me.WriteCodePair(70, CInt(polyline.Flags))

            'dummy point
            Me.WriteCodePair(10, 0.0)
            Me.WriteCodePair(20, 0.0)

            Me.WriteCodePair(30, polyline.Elevation)
            Me.WriteCodePair(39, polyline.Thickness)

            Me.WriteCodePair(210, polyline.Normal.X)
            Me.WriteCodePair(220, polyline.Normal.Y)
            Me.WriteCodePair(230, polyline.Normal.Z)

            'Obsolete; formerly an “entities follow flag” (optional; ignore if present)
            'but its needed to load the dxf file in AutoCAD
            Me.WriteCodePair(66, "1")

            Me.WriteXData(polyline.XData)

            For Each v As PolylineVertex In polyline.Vertexes

                Me.WriteCodePair(0, v.CodeName)
                Me.WriteCodePair(5, v.Handle)
                Me.WriteCodePair(100, SubclassMarker.Entity)
                Me.WriteCodePair(8, v.Layer)
                Me.WriteCodePair(100, SubclassMarker.Vertex)
                Me.WriteCodePair(100, SubclassMarker.PolylineVertex)
                Me.WriteCodePair(70, CInt(v.Flags))
                Me.WriteCodePair(10, v.Location.X)
                Me.WriteCodePair(20, v.Location.Y)
                Me.WriteCodePair(40, v.BeginThickness)
                Me.WriteCodePair(41, v.EndThickness)
                Me.WriteCodePair(42, v.Bulge)

                Me.WriteXData(v.XData)
            Next

            Me.WriteCodePair(0, polyline.EndSequence.CodeName)
            Me.WriteCodePair(5, polyline.EndSequence.Handle)
            Me.WriteCodePair(100, SubclassMarker.Entity)
            Me.WriteCodePair(8, polyline.EndSequence.Layer)
        End SyncLock
    End Sub

    Private Sub WriteLightWeightPolyline(ByVal polyline As LightWeightPolyline)
        SyncLock _locker
            If Me.m_activeSection <> StringCode.EntitiesSection AndAlso Not Me.isBlockEntities Then
                Throw New InvalidDxfSectionException(Me.m_activeSection, Me.file)
            End If

            Me.WriteCodePair(0, DxfObjectCode.LightWeightPolyline)
            Me.WriteCodePair(100, SubclassMarker.Entity)
            Me.WriteEntityCommonCodes(polyline)
            Me.WriteCodePair(5, polyline.Handle)
            Me.WriteCodePair(100, SubclassMarker.LightWeightPolyline)
            Me.WriteCodePair(90, polyline.Vertexes.Count)
            Me.WriteCodePair(70, CInt(polyline.Flags))

            Me.WriteCodePair(38, polyline.Elevation)
            Me.WriteCodePair(39, polyline.Thickness)


            For Each v As LightWeightPolylineVertex In polyline.Vertexes
                Me.WriteCodePair(10, v.Location.X)
                Me.WriteCodePair(20, v.Location.Y)
                Me.WriteCodePair(40, v.BeginThickness)
                Me.WriteCodePair(41, v.EndThickness)
                Me.WriteCodePair(42, v.Bulge)
            Next

            Me.WriteCodePair(210, polyline.Normal.X)
            Me.WriteCodePair(220, polyline.Normal.Y)
            Me.WriteCodePair(230, polyline.Normal.Z)

            Me.WriteXData(polyline.XData)
        End SyncLock
    End Sub

    Private Sub WritePolyline3d(ByVal polyline As Polyline3d)
        SyncLock _locker
            If Me.m_activeSection <> StringCode.EntitiesSection AndAlso Not Me.isBlockEntities Then
                Throw New InvalidDxfSectionException(Me.m_activeSection, Me.file)
            End If

            Me.WriteCodePair(0, polyline.CodeName)
            Me.WriteCodePair(100, SubclassMarker.Entity)
            Me.WriteEntityCommonCodes(polyline)
            Me.WriteCodePair(5, polyline.Handle)
            Me.WriteCodePair(100, SubclassMarker.Polyline3d)

            Me.WriteCodePair(70, CInt(polyline.Flags))

            'dummy point
            Me.WriteCodePair(10, 0.0)
            Me.WriteCodePair(20, 0.0)
            Me.WriteCodePair(30, 0.0)

            'Obsolete; formerly an “entities follow flag” (optional; ignore if present)
            'but its needed to load the dxf file in AutoCAD
            Me.WriteCodePair(66, "1")

            Me.WriteXData(polyline.XData)

            For Each v As Polyline3dVertex In polyline.Vertexes
                Me.WriteCodePair(0, v.CodeName)
                Me.WriteCodePair(5, v.Handle)
                Me.WriteCodePair(100, SubclassMarker.Entity)
                Me.WriteCodePair(8, v.Layer)
                Me.WriteCodePair(100, SubclassMarker.Vertex)
                Me.WriteCodePair(100, SubclassMarker.Polyline3dVertex)
                Me.WriteCodePair(70, CInt(v.Flags))
                Me.WriteCodePair(10, v.Location.X)
                Me.WriteCodePair(20, v.Location.Y)
                Me.WriteCodePair(30, v.Location.Z)

                Me.WriteXData(v.XData)
            Next
            Me.WriteCodePair(0, polyline.EndSequence.CodeName)
            Me.WriteCodePair(5, polyline.EndSequence.Handle)
            Me.WriteCodePair(100, SubclassMarker.Entity)
            Me.WriteCodePair(8, polyline.EndSequence.Layer)
        End SyncLock
    End Sub

    Private Sub WritePolyfaceMesh(ByVal mesh As PolyfaceMesh)
        SyncLock _locker
            If Me.m_activeSection <> StringCode.EntitiesSection AndAlso Not Me.isBlockEntities Then
                Throw New InvalidDxfSectionException(Me.m_activeSection, Me.file)
            End If

            Me.WriteCodePair(0, mesh.CodeName)
            Me.WriteCodePair(100, SubclassMarker.Entity)
            Me.WriteEntityCommonCodes(mesh)
            Me.WriteCodePair(5, mesh.Handle)
            Me.WriteCodePair(100, SubclassMarker.PolyfaceMesh)

            Me.WriteCodePair(70, CInt(mesh.Flags))

            Me.WriteCodePair(71, mesh.Vertexes.Count)

            Me.WriteCodePair(72, mesh.Faces.Count)

            'dummy point
            Me.WriteCodePair(10, 0.0)
            Me.WriteCodePair(20, 0.0)
            Me.WriteCodePair(30, 0.0)

            'Obsolete; formerly an “entities follow flag” (optional; ignore if present)
            'but its needed to load the dxf file in AutoCAD
            Me.WriteCodePair(66, "1")

            If mesh.XData IsNot Nothing Then
                Me.WriteXData(mesh.XData)
            End If

            For Each v As PolyfaceMeshVertex In mesh.Vertexes
                Me.WriteCodePair(0, v.CodeName)
                Me.WriteCodePair(5, v.Handle)
                Me.WriteCodePair(100, SubclassMarker.Entity)
                Me.WriteCodePair(8, v.Layer)
                Me.WriteCodePair(100, SubclassMarker.Vertex)
                Me.WriteCodePair(100, SubclassMarker.PolyfaceMeshVertex)
                Me.WriteCodePair(70, CInt(v.Flags))
                Me.WriteCodePair(10, v.Location.X)
                Me.WriteCodePair(20, v.Location.Y)
                Me.WriteCodePair(30, v.Location.Z)

                Me.WriteXData(v.XData)
            Next

            For Each face As PolyfaceMeshFace In mesh.Faces
                Me.WriteCodePair(0, face.CodeName)
                Me.WriteCodePair(5, face.Handle)
                Me.WriteCodePair(100, SubclassMarker.Entity)
                Me.WriteCodePair(8, face.Layer)
                Me.WriteCodePair(100, SubclassMarker.PolyfaceMeshFace)
                Me.WriteCodePair(70, CInt(VertexTypeFlags.PolyfaceMeshVertex))
                Me.WriteCodePair(10, 0)
                Me.WriteCodePair(20, 0)
                Me.WriteCodePair(30, 0)

                Me.WriteCodePair(71, face.VertexIndexes(0))
                Me.WriteCodePair(72, face.VertexIndexes(1))
                Me.WriteCodePair(73, face.VertexIndexes(2))
            Next

            Me.WriteCodePair(0, mesh.EndSequence.CodeName)
            Me.WriteCodePair(5, mesh.EndSequence.Handle)
            Me.WriteCodePair(100, SubclassMarker.Entity)
            Me.WriteCodePair(8, mesh.EndSequence.Layer)
        End SyncLock
    End Sub

    Private Sub WritePoint(ByVal point As Point)
        SyncLock _locker
            If Me.m_activeSection <> StringCode.EntitiesSection AndAlso Not Me.isBlockEntities Then
                Throw New InvalidDxfSectionException(Me.m_activeSection, Me.file)
            End If

            Me.WriteCodePair(0, point.CodeName)
            Me.WriteCodePair(5, point.Handle)
            Me.WriteCodePair(100, SubclassMarker.Entity)
            Me.WriteEntityCommonCodes(point)
            Me.WriteCodePair(100, SubclassMarker.Point)

            Me.WriteCodePair(10, point.Location.X)
            Me.WriteCodePair(20, point.Location.Y)
            Me.WriteCodePair(30, point.Location.Z)

            Me.WriteCodePair(39, point.Thickness)

            Me.WriteCodePair(210, point.Normal.X)
            Me.WriteCodePair(220, point.Normal.Y)
            Me.WriteCodePair(230, point.Normal.Z)

            Me.WriteXData(point.XData)
        End SyncLock

    End Sub

    Private Sub WriteText(ByVal text As Text)
        SyncLock _locker
            If Me.m_activeSection <> StringCode.EntitiesSection AndAlso Not Me.isBlockEntities Then
                Throw New InvalidDxfSectionException(Me.m_activeSection, Me.file)
            End If

            Me.WriteCodePair(0, text.CodeName)
            Me.WriteCodePair(100, SubclassMarker.Entity)
            Me.WriteEntityCommonCodes(text)
            Me.WriteCodePair(5, text.Handle)
            Me.WriteCodePair(100, SubclassMarker.Text)

            Me.WriteCodePair(1, text.Value)

            Me.WriteCodePair(10, text.BasePoint.X)
            Me.WriteCodePair(20, text.BasePoint.Y)
            Me.WriteCodePair(30, text.BasePoint.Z)

            Me.WriteCodePair(40, text.Height)

            Me.WriteCodePair(41, text.WidthFactor)

            Me.WriteCodePair(50, text.Rotation)

            Me.WriteCodePair(51, text.ObliqueAngle)

            Me.WriteCodePair(7, text.Style)

            Me.WriteCodePair(11, text.BasePoint.X)
            Me.WriteCodePair(21, text.BasePoint.Y)
            Me.WriteCodePair(31, text.BasePoint.Z)

            Me.WriteCodePair(210, text.Normal.X)
            Me.WriteCodePair(220, text.Normal.Y)
            Me.WriteCodePair(230, text.Normal.Z)

            Select Case text.Alignment
                Case TextAlignment.TopLeft

                    Me.WriteCodePair(72, 0)
                    Me.WriteCodePair(100, SubclassMarker.Text)
                    Me.WriteCodePair(73, 3)
                    Exit Select

                Case TextAlignment.TopCenter

                    Me.WriteCodePair(72, 1)
                    Me.WriteCodePair(100, SubclassMarker.Text)
                    Me.WriteCodePair(73, 3)
                    Exit Select

                Case TextAlignment.TopRight

                    Me.WriteCodePair(72, 2)
                    Me.WriteCodePair(100, SubclassMarker.Text)
                    Me.WriteCodePair(73, 3)
                    Exit Select

                Case TextAlignment.MiddleLeft

                    Me.WriteCodePair(72, 0)
                    Me.WriteCodePair(100, SubclassMarker.Text)
                    Me.WriteCodePair(73, 2)
                    Exit Select

                Case TextAlignment.MiddleCenter

                    Me.WriteCodePair(72, 1)
                    Me.WriteCodePair(100, SubclassMarker.Text)
                    Me.WriteCodePair(73, 2)
                    Exit Select

                Case TextAlignment.MiddleRight

                    Me.WriteCodePair(72, 2)
                    Me.WriteCodePair(100, SubclassMarker.Text)
                    Me.WriteCodePair(73, 2)
                    Exit Select

                Case TextAlignment.BottomLeft

                    Me.WriteCodePair(72, 0)
                    Me.WriteCodePair(100, SubclassMarker.Text)
                    Me.WriteCodePair(73, 1)
                    Exit Select
                Case TextAlignment.BottomCenter

                    Me.WriteCodePair(72, 1)
                    Me.WriteCodePair(100, SubclassMarker.Text)
                    Me.WriteCodePair(73, 1)
                    Exit Select

                Case TextAlignment.BottomRight

                    Me.WriteCodePair(72, 2)
                    Me.WriteCodePair(100, SubclassMarker.Text)
                    Me.WriteCodePair(73, 1)
                    Exit Select

                Case TextAlignment.BaselineLeft
                    Me.WriteCodePair(72, 0)
                    Me.WriteCodePair(100, SubclassMarker.Text)
                    Me.WriteCodePair(73, 0)
                    Exit Select

                Case TextAlignment.BaselineCenter
                    Me.WriteCodePair(72, 1)
                    Me.WriteCodePair(100, SubclassMarker.Text)
                    Me.WriteCodePair(73, 0)
                    Exit Select

                Case TextAlignment.BaselineRight
                    Me.WriteCodePair(72, 2)
                    Me.WriteCodePair(100, SubclassMarker.Text)
                    Me.WriteCodePair(73, 0)
                    Exit Select
            End Select

            Me.WriteXData(text.XData)
        End SyncLock
    End Sub



    Public Sub WriteDictionary(ByVal dictionary As Dictionary)
        SyncLock _locker
            'if (this.activeTable != StringCode.ObjectsSection)
            '{
            '    throw new InvalidDxfTableException(this.activeTable, this.file);
            '}

            Me.WriteCodePair(0, StringCode.Dictionary)
            Me.WriteCodePair(5, Convert.ToString(10, 16))
            Me.WriteCodePair(100, SubclassMarker.Dictionary)
            Me.WriteCodePair(281, 1)
            Me.WriteCodePair(3, dictionary)
            Me.WriteCodePair(350, Convert.ToString(11, 16))

            Me.WriteCodePair(0, StringCode.Dictionary)
            Me.WriteCodePair(5, Convert.ToString(11, 16))
            Me.WriteCodePair(100, SubclassMarker.Dictionary)
            Me.WriteCodePair(281, 1)
        End SyncLock
    End Sub




    Private Sub WriteAttributeDefinition(ByVal def As AttributeDefinition)
        SyncLock _locker
            Me.WriteCodePair(0, DxfObjectCode.AttributeDefinition)
            Me.WriteCodePair(5, def.Handle)

            Me.WriteCodePair(100, SubclassMarker.Entity)
            Me.WriteEntityCommonCodes(def)
            Me.WriteCodePair(100, SubclassMarker.Text)
            Me.WriteCodePair(10, def.BasePoint.X)
            Me.WriteCodePair(20, def.BasePoint.Y)
            Me.WriteCodePair(30, def.BasePoint.Z)
            Me.WriteCodePair(40, def.Height)
            Me.WriteCodePair(1, def.Value)

            Me.WriteCodePair(7, def.Style)

            Me.WriteCodePair(41, def.WidthFactor)

            Me.WriteCodePair(50, def.Rotation)

            Me.WriteCodePair(100, SubclassMarker.AttributeDefinition)
            Me.WriteCodePair(2, def.Id)

            Me.WriteCodePair(3, def.Text)

            Me.WriteCodePair(70, CInt(def.Flags))

            Select Case def.Alignment
                Case TextAlignment.TopLeft

                    Me.WriteCodePair(72, 0)
                    Me.WriteCodePair(74, 3)
                    Exit Select
                Case TextAlignment.TopCenter

                    Me.WriteCodePair(72, 1)
                    Me.WriteCodePair(74, 3)
                    Exit Select
                Case TextAlignment.TopRight

                    Me.WriteCodePair(72, 2)
                    Me.WriteCodePair(74, 3)
                    Exit Select
                Case TextAlignment.MiddleLeft

                    Me.WriteCodePair(72, 0)
                    Me.WriteCodePair(74, 2)
                    Exit Select
                Case TextAlignment.MiddleCenter

                    Me.WriteCodePair(72, 1)
                    Me.WriteCodePair(74, 2)
                    Exit Select
                Case TextAlignment.MiddleRight

                    Me.WriteCodePair(72, 2)
                    Me.WriteCodePair(74, 2)
                    Exit Select
                Case TextAlignment.BottomLeft

                    Me.WriteCodePair(72, 0)
                    Me.WriteCodePair(74, 1)
                    Exit Select
                Case TextAlignment.BottomCenter

                    Me.WriteCodePair(72, 1)
                    Me.WriteCodePair(74, 1)
                    Exit Select
                Case TextAlignment.BottomRight

                    Me.WriteCodePair(72, 2)
                    Me.WriteCodePair(74, 1)
                    Exit Select
                Case TextAlignment.BaselineLeft

                    Me.WriteCodePair(72, 0)
                    Me.WriteCodePair(74, 0)
                    Exit Select
                Case TextAlignment.BaselineCenter

                    Me.WriteCodePair(72, 1)
                    Me.WriteCodePair(74, 0)
                    Exit Select
                Case TextAlignment.BaselineRight

                    Me.WriteCodePair(72, 2)
                    Me.WriteCodePair(74, 0)
                    Exit Select
            End Select

            Me.WriteCodePair(11, def.BasePoint.X)
            Me.WriteCodePair(21, def.BasePoint.Y)
            Me.WriteCodePair(31, def.BasePoint.Z)
        End SyncLock
    End Sub

    Private Sub WriteAttribute(ByVal attrib As Attribute, ByVal puntoInsercion As Vector3f)
        SyncLock _locker
            Me.WriteCodePair(0, DxfObjectCode.Attribute)
            Me.WriteCodePair(5, attrib.Handle)
            Me.WriteCodePair(100, SubclassMarker.Entity)
            Me.WriteEntityCommonCodes(attrib)

            Me.WriteCodePair(100, SubclassMarker.Text)
            Me.WriteCodePair(10, attrib.Definition.BasePoint.X + puntoInsercion.X)
            Me.WriteCodePair(20, attrib.Definition.BasePoint.Y + puntoInsercion.Y)
            Me.WriteCodePair(30, attrib.Definition.BasePoint.Z + puntoInsercion.Z)

            Me.WriteCodePair(1, attrib.Value)

            Me.WriteCodePair(40, attrib.Definition.Height)

            Me.WriteCodePair(41, attrib.Definition.WidthFactor)

            Me.WriteCodePair(50, attrib.Definition.Rotation)

            Me.WriteCodePair(7, attrib.Definition.Style)

            Me.WriteCodePair(100, SubclassMarker.Attribute)

            Me.WriteCodePair(2, attrib.Definition.Id)

            Me.WriteCodePair(70, CInt(attrib.Definition.Flags))


            Me.WriteCodePair(11, attrib.Definition.BasePoint.X + puntoInsercion.X)
            Me.WriteCodePair(21, attrib.Definition.BasePoint.Y + puntoInsercion.Y)
            Me.WriteCodePair(31, attrib.Definition.BasePoint.Z + puntoInsercion.Z)
        End SyncLock
    End Sub

    Private Sub WriteXData(ByVal xData As Dictionary(Of ApplicationRegistry, XData))
        SyncLock _locker
            If xData Is Nothing Then
                Return
            End If

            For Each appReg As ApplicationRegistry In xData.Keys
                Me.WriteCodePair(XDataCode.AppReg, appReg)
                For Each x As XDataRecord In xData(appReg).XDataRecord
                    Me.WriteCodePair(x.Code, x.Value.ToString())
                Next
            Next
        End SyncLock
    End Sub

    Private Sub WriteEntityCommonCodes(ByVal entity As IEntityObject)
        SyncLock _locker
            Me.WriteCodePair(8, entity.Layer)
            Me.WriteCodePair(62, entity.Color.Index)
            Me.WriteCodePair(6, entity.LineType)
        End SyncLock
    End Sub

    Private Sub WriteCodePair(ByVal codigo As Integer, ByVal valor As Object)
        SyncLock _locker
            ' AutoCad12 does not allow strings with spaces
            Dim nameConversion As String
            nameConversion = If(valor Is Nothing, String.Empty, valor.ToString())

            If Me.version = DxfVersion.AutoCad12 AndAlso TypeOf valor Is DxfObject Then
                nameConversion = nameConversion.Replace(" "c, "_"c)
            End If
            Me.writer.WriteLine(codigo)
            Me.writer.WriteLine(nameConversion)
        End SyncLock
    End Sub
     
End Class 
