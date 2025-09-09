
Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Globalization
Imports System.IO
Imports System.Threading
Imports pccDXF4.Blocks
Imports pccDXF4.Entities
Imports pccDXF4.Header
Imports pccDXF4.Objects
Imports pccDXF4.Tables
Imports Attribute = pccDXF4.Entities.Attribute


''' <summary>
''' Represents a document to read and write dxf ASCII files.
''' </summary>
''' <remarks>
''' The dxf object names (application registries, layers, text styles, blocks, line types,...) for AutoCad12 can not contain spaces,
''' if this situation happens all spaces will be replaced by an underscore character '_'.
''' </remarks>
Public Class DxfDocument



    Private m_fileName As String
    Private m_version As DxfVersion
    Private handleCount As Integer = 100
    'we will reserve the first handles for special cases


    Private m_appRegisterNames As Dictionary(Of String, ApplicationRegistry)
    Private ReadOnly viewports As Dictionary(Of String, ViewPort)
    Private m_layers As Dictionary(Of String, Layer)
    Private m_lineTypes As Dictionary(Of String, LineType)
    Private m_textStyles As Dictionary(Of String, TextStyle)
    Private ReadOnly dimStyles As Dictionary(Of String, DimensionStyle)




    Private m_blocks As Dictionary(Of String, Block)




    Private ReadOnly addedObjects As Hashtable
    Private m_arcs As List(Of Arc)
    Private m_circles As List(Of Circle)
    Private m_ellipses As List(Of Ellipse)
    Private m_nurbsCurves As List(Of NurbsCurve)
    Private m_solids As List(Of Solid)
    Private m_faces3d As List(Of Face3d)
    Private m_inserts As List(Of Insert)
    Private m_lines As List(Of Line)
    Private m_points As List(Of Point)
    Private m_polylines As List(Of IPolyline)
    Private m_texts As List(Of Text)


    Private Shared ReadOnly _locker As New Object()

     

    ''' <summary>
    ''' Initalizes a new instance of the <c>DxfDocument</c> class.
    ''' </summary>
    Public Sub New()
        SyncLock _locker
            Me.addedObjects = New Hashtable()
            ' keeps track of the added object to avoid duplicates
            Me.m_version = Me.m_version
            Me.viewports = New Dictionary(Of String, ViewPort)()
            Me.m_layers = New Dictionary(Of String, Layer)()
            Me.m_lineTypes = New Dictionary(Of String, LineType)()
            Me.m_textStyles = New Dictionary(Of String, TextStyle)()
            Me.m_blocks = New Dictionary(Of String, Block)()
            Me.m_appRegisterNames = New Dictionary(Of String, ApplicationRegistry)()
            Me.dimStyles = New Dictionary(Of String, DimensionStyle)()

            '''/add default viewports
            'ViewPort active = ViewPort.Active;
            'this.viewports.Add(active.Name, active);
            'this.handleCount = active.AsignHandle(this.handleCount);

            '''/add default layer
            'Layer.PlotStyleHandle = Conversion.Hex(this.handleCount++);
            'Layer byDefault = Layer.Default;
            'this.layers.Add(byDefault.Name, byDefault);
            'this.handleCount = byDefault.AsignHandle(this.handleCount);

            '''/ add default line types
            'LineType byLayer = LineType.ByLayer;
            'LineType byBlock = LineType.ByBlock;
            'this.lineTypes.Add(byLayer.Name, byLayer);
            'this.handleCount = byLayer.AsignHandle(this.handleCount);
            'this.lineTypes.Add(byBlock.Name, byBlock);
            'this.handleCount = byBlock.AsignHandle(this.handleCount);

            '''/ add default text style
            'TextStyle defaultStyle = TextStyle.Default;
            'this.textStyles.Add(defaultStyle.Name, defaultStyle);
            'this.handleCount = defaultStyle.AsignHandle(this.handleCount);

            '''/ add default blocks
            'Block modelSpace = Block.ModelSpace;
            'Block paperSpace = Block.PaperSpace;
            'this.blocks.Add(modelSpace.Name, modelSpace);
            'this.handleCount = modelSpace.AsignHandle(this.handleCount);
            'this.blocks.Add(paperSpace.Name, paperSpace);
            'this.handleCount = paperSpace.AsignHandle(this.handleCount);

            '''/ add default application registry
            'ApplicationRegistry defaultAppId = ApplicationRegistry.Default;
            'this.appRegisterNames.Add(defaultAppId.Name, defaultAppId);
            'this.handleCount = defaultAppId.AsignHandle(this.handleCount);

            '''/add default dimension style
            'DimensionStyle defaultDimStyle = DimensionStyle.Default;
            'this.dimStyles.Add(defaultDimStyle.Name, defaultDimStyle);
            'this.handleCount = defaultDimStyle.AsignHandle(this.handleCount);

            Me.m_arcs = New List(Of Arc)()
            Me.m_ellipses = New List(Of Ellipse)()
            Me.m_nurbsCurves = New List(Of NurbsCurve)()
            Me.m_faces3d = New List(Of Face3d)()
            Me.m_solids = New List(Of Solid)()
            Me.m_inserts = New List(Of Insert)()
            Me.m_polylines = New List(Of IPolyline)()
            Me.m_lines = New List(Of Line)()
            Me.m_circles = New List(Of Circle)()
            Me.m_points = New List(Of Point)()
            Me.m_texts = New List(Of Text)()
        End SyncLock
    End Sub






    ''' <summary>
    ''' Gets the dxf file <see cref="DxfVersion">version</see>.
    ''' </summary>
    Public ReadOnly Property Version() As DxfVersion
        Get
            SyncLock _locker
                Return Me.m_version
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Gets the name of the dxf document, once a file is saved or loaded this field is equals the file name without extension.
    ''' </summary>
    Public ReadOnly Property FileName() As String
        Get
            SyncLock _locker
                Return Me.m_fileName
            End SyncLock
        End Get
    End Property




    ''' <summary>
    ''' Gets the application registered names.
    ''' </summary>
    Public ReadOnly Property AppRegisterNames() As ReadOnlyCollection(Of ApplicationRegistry)
        Get
            SyncLock _locker
                Dim list As New List(Of ApplicationRegistry)()
                list.AddRange(Me.m_appRegisterNames.Values)
                Return list.AsReadOnly()
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Gets the <see cref="Layer">layer</see> list.
    ''' </summary>
    Public ReadOnly Property Layers() As ReadOnlyCollection(Of Layer)
        Get
            SyncLock _locker
                Dim list As New List(Of Layer)()
                list.AddRange(Me.m_layers.Values)
                Return list.AsReadOnly()
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Gets the <see cref="LineType">linetype</see> list.
    ''' </summary>
    Public ReadOnly Property LineTypes() As ReadOnlyCollection(Of LineType)
        Get
            SyncLock _locker
                Dim list As New List(Of LineType)()
                list.AddRange(Me.m_lineTypes.Values)
                Return list.AsReadOnly()
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Gets the <see cref="TextStyle">text style</see> list.
    ''' </summary>
    Public ReadOnly Property TextStyles() As ReadOnlyCollection(Of TextStyle)
        Get
            SyncLock _locker
                Dim list As New List(Of TextStyle)()
                list.AddRange(Me.m_textStyles.Values)
                Return list.AsReadOnly()
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Gets the <see cref="Block">block</see> list.
    ''' </summary>
    Public ReadOnly Property Blocks() As ReadOnlyCollection(Of Block)
        Get
            SyncLock _locker
                Dim list As New List(Of Block)()
                list.AddRange(Me.m_blocks.Values)
                Return list.AsReadOnly()
            End SyncLock
        End Get
    End Property




    ''' <summary>
    ''' Gets the <see cref="pccDXF4.Entities.Arc">arc</see> list.
    ''' </summary>
    Public ReadOnly Property Arcs() As ReadOnlyCollection(Of Arc)
        Get
            SyncLock _locker
                Return Me.m_arcs.AsReadOnly()
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Gets the <see cref="pccDXF4.Entities.Ellipse">ellipse</see> list.
    ''' </summary>
    Public ReadOnly Property Ellipses() As ReadOnlyCollection(Of Ellipse)
        Get
            SyncLock _locker
                Return Me.m_ellipses.AsReadOnly()
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Gets the <see cref="pccDXF4.Entities.NurbsCurve">NURBS Curve</see> list.
    ''' </summary>
    Public ReadOnly Property NurbsCurves() As ReadOnlyCollection(Of NurbsCurve)
        Get
            SyncLock _locker
                Return Me.m_nurbsCurves.AsReadOnly()
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Gets the <see cref="pccDXF4.Entities.Circle">circle</see> list.
    ''' </summary>
    Public ReadOnly Property Circles() As ReadOnlyCollection(Of Circle)
        Get
            SyncLock _locker
                Return Me.m_circles.AsReadOnly()
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Gets the <see cref="pccDXF4.Entities.Face3d">3d face</see> list.
    ''' </summary>
    Public ReadOnly Property Faces3d() As ReadOnlyCollection(Of Face3d)
        Get
            SyncLock _locker
                Return Me.m_faces3d.AsReadOnly()
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Gets the <see cref="pccDXF4.Entities.Solid">solid</see> list.
    ''' </summary>
    Public ReadOnly Property Solids() As ReadOnlyCollection(Of Solid)
        Get
            SyncLock _locker
                Return Me.m_solids.AsReadOnly()
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Gets the <see cref="pccDXF4.Entities.Insert">insert</see> list.
    ''' </summary>
    Public ReadOnly Property Inserts() As ReadOnlyCollection(Of Insert)
        Get
            SyncLock _locker
                Return Me.m_inserts.AsReadOnly()
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Gets the <see cref="pccDXF4.Entities.Line">line</see> list.
    ''' </summary>
    Public ReadOnly Property Lines() As ReadOnlyCollection(Of Line)
        Get
            SyncLock _locker
                Return Me.m_lines.AsReadOnly()
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Gets the <see cref="pccDXF4.Entities.IPolyline">polyline</see> list.
    ''' </summary>
    ''' <remarks>
    ''' The polyline list contains all entities that are considered polylines in the dxf, they are:
    ''' <see cref="Polyline">polylines</see>, <see cref="Polyline3d">3d polylines</see> and <see cref="PolyfaceMesh">polyface meshes</see>
    ''' </remarks>
    Public ReadOnly Property Polylines() As ReadOnlyCollection(Of IPolyline)
        Get
            SyncLock _locker
                Return Me.m_polylines.AsReadOnly()
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Gets the <see cref="pccDXF4.Entities.Point">point</see> list.
    ''' </summary>
    Public ReadOnly Property Points() As ReadOnlyCollection(Of Point)
        Get
            SyncLock _locker
                Return Me.m_points.AsReadOnly()
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Gets the <see cref="pccDXF4.Entities.Text">text</see> list.
    ''' </summary>
    Public ReadOnly Property Texts() As ReadOnlyCollection(Of Text)
        Get
            SyncLock _locker
                Return Me.m_texts.AsReadOnly()
            End SyncLock
        End Get
    End Property





    ''' <summary>
    ''' Gets a text style from the the table.
    ''' </summary>
    ''' <param name="name">TextStyle name</param>
    ''' <returns>TextStyle.</returns>
    Public Function GetTextStyle(ByVal name As String) As TextStyle
        SyncLock _locker
            Return Me.m_textStyles(name)
        End SyncLock
    End Function

    ''' <summary>
    ''' Determines if a specified text style exists in the table.
    ''' </summary>
    ''' <param name="textStyle">Text style to locate.</param>
    ''' <returns>True if the specified text style exists or false in any other case.</returns>
    Public Function ContainsTextStyle(ByVal textStyle As TextStyle) As Boolean
        SyncLock _locker
            Return Me.m_textStyles.ContainsKey(textStyle.Name)
        End SyncLock
    End Function

    ''' <summary>
    ''' Gets a block from the the table.
    ''' </summary>
    ''' <param name="name">Block name</param>
    ''' <returns>Block.</returns>
    Public Function GetBlock(ByVal name As String) As Block
        SyncLock _locker
            Return Me.m_blocks(name)
        End SyncLock
    End Function

    ''' <summary>
    ''' Determines if a specified block exists in the table.
    ''' </summary>
    ''' <param name="block">Block to locate.</param>
    ''' <returns>True if the specified block exists or false in any other case.</returns>
    Public Function ContainsBlock(ByVal block As Block) As Boolean
        SyncLock _locker
            Return Me.m_blocks.ContainsKey(block.Name)
        End SyncLock
    End Function

    ''' <summary>
    ''' Gets a line type from the the table.
    ''' </summary>
    ''' <param name="name">LineType name</param>
    ''' <returns>LineType.</returns>
    Public Function GetLineType(ByVal name As String) As LineType
        SyncLock _locker
            Return Me.m_lineTypes(name)
        End SyncLock
    End Function

    ''' <summary>
    ''' Determines if a specified line type exists in the table.
    ''' </summary>
    ''' <param name="lineType">Line type to locate.</param>
    ''' <returns>True if the specified line type exists or false in any other case.</returns>
    Public Function ContainsLineType(ByVal lineType As LineType) As Boolean
        SyncLock _locker
            Return Me.m_lineTypes.ContainsKey(lineType.Name)
        End SyncLock
    End Function

    ''' <summary>
    ''' Gets a layer from the the table.
    ''' </summary>
    ''' <param name="name">Layer name</param>
    ''' <returns>Layer.</returns>
    Public Function GetLayer(ByVal name As String) As Layer
        SyncLock _locker
            Return Me.m_layers(name)
        End SyncLock
    End Function

    ''' <summary>
    ''' Determines if a specified layer exists in the table.
    ''' </summary>
    ''' <param name="layer">Layer to locate.</param>
    ''' <returns>True if the specified layer exists or false in any other case.</returns>
    Public Function ContainsLayer(ByVal layer As Layer) As Boolean
        SyncLock _locker
            Return Me.m_layers.ContainsKey(layer.Name)
        End SyncLock
    End Function




    ''' <summary>
    ''' Adds a new <see cref="IEntityObject">entity</see> to the document.
    ''' </summary>
    ''' <param name="entity">An <see cref="IEntityObject">entity</see></param>
    Public Sub AddEntity(ByVal entity As IEntityObject)
        SyncLock _locker
            ' check if the entity has not been added to the document
            If Me.addedObjects.ContainsKey(entity) Then
                Throw New ArgumentException("The entity " & Convert.ToString(entity.Type) & " object has already been added to the document.", "entity")
            End If

            Me.addedObjects.Add(entity, entity)

            If entity.XData IsNot Nothing Then
                For Each appReg As ApplicationRegistry In entity.XData.Keys
                    If Not Me.m_appRegisterNames.ContainsKey(appReg.Name) Then
                        Me.m_appRegisterNames.Add(appReg.Name, appReg)
                    End If
                Next
            End If

            If Not Me.m_layers.ContainsKey(entity.Layer.Name) Then
                If Not Me.m_lineTypes.ContainsKey(entity.Layer.LineType.Name) Then
                    Me.m_lineTypes.Add(entity.Layer.LineType.Name, entity.Layer.LineType)
                End If
                Me.m_layers.Add(entity.Layer.Name, entity.Layer)
            End If

            If Not Me.m_lineTypes.ContainsKey(entity.LineType.Name) Then
                Me.m_lineTypes.Add(entity.LineType.Name, entity.LineType)
            End If

            Select Case entity.Type
                Case EntityType.Arc
                    Me.m_arcs.Add(DirectCast(entity, Arc))
                    Exit Select
                Case EntityType.Circle
                    Me.m_circles.Add(DirectCast(entity, Circle))
                    Exit Select
                Case EntityType.Ellipse
                    Me.m_ellipses.Add(DirectCast(entity, Ellipse))
                    Exit Select
                Case EntityType.NurbsCurve
                    Throw New NotImplementedException("Nurbs curves not avaliable at the moment.")
                    Me.m_nurbsCurves.Add(DirectCast(entity, NurbsCurve))
                    Exit Select
                Case EntityType.Point
                    Me.m_points.Add(DirectCast(entity, Point))
                    Exit Select
                Case EntityType.Face3D
                    Me.m_faces3d.Add(DirectCast(entity, Face3d))
                    Exit Select
                Case EntityType.Solid
                    Me.m_solids.Add(DirectCast(entity, Solid))
                    Exit Select
                Case EntityType.Insert
                    ' if the block definition has already been added, we do not need to do anything else
                    If Not Me.m_blocks.ContainsKey(DirectCast(entity, Insert).Block.Name) Then
                        Me.m_blocks.Add(DirectCast(entity, Insert).Block.Name, DirectCast(entity, Insert).Block)

                        If Not Me.m_layers.ContainsKey(DirectCast(entity, Insert).Block.Layer.Name) Then
                            Me.m_layers.Add(DirectCast(entity, Insert).Block.Layer.Name, DirectCast(entity, Insert).Block.Layer)
                        End If

                        'for new block definitions configure its entities
                        For Each blockEntity As IEntityObject In DirectCast(entity, Insert).Block.Entities
                            ' check if the entity has not been added to the document
                            If Me.addedObjects.ContainsKey(blockEntity) Then
                                Throw New ArgumentException(("The entity " + blockEntity.Type & " object of the block ") + DirectCast(entity, Insert).Block.Name & " has already been added to the document.", "entity")
                            End If
                            Me.addedObjects.Add(blockEntity, blockEntity)

                            If Not Me.m_layers.ContainsKey(blockEntity.Layer.Name) Then
                                Me.m_layers.Add(blockEntity.Layer.Name, blockEntity.Layer)
                            End If
                            If Not Me.m_lineTypes.ContainsKey(blockEntity.LineType.Name) Then
                                Me.m_lineTypes.Add(blockEntity.LineType.Name, blockEntity.LineType)
                            End If
                        Next
                        'for new block definitions configure its attributes
                        For Each attribute As Attribute In DirectCast(entity, Insert).Attributes
                            If Not Me.m_layers.ContainsKey(attribute.Layer.Name) Then
                                Me.m_layers.Add(attribute.Layer.Name, attribute.Layer)
                            End If
                            If Not Me.m_lineTypes.ContainsKey(attribute.LineType.Name) Then
                                Me.m_lineTypes.Add(attribute.LineType.Name, attribute.LineType)
                            End If

                            Dim attDef As AttributeDefinition = attribute.Definition
                            If Not Me.m_layers.ContainsKey(attDef.Layer.Name) Then
                                Me.m_layers.Add(attDef.Layer.Name, attDef.Layer)
                            End If

                            If Not Me.m_lineTypes.ContainsKey(attDef.LineType.Name) Then
                                Me.m_lineTypes.Add(attDef.LineType.Name, attDef.LineType)
                            End If

                            If Not Me.m_textStyles.ContainsKey(attDef.Style.Name) Then
                                Me.m_textStyles.Add(attDef.Style.Name, attDef.Style)
                            End If
                        Next
                    End If

                    Me.m_inserts.Add(DirectCast(entity, Insert))
                    Exit Select
                Case EntityType.Line
                    Me.m_lines.Add(DirectCast(entity, Line))
                    Exit Select
                Case EntityType.LightWeightPolyline
                    Me.m_polylines.Add(DirectCast(entity, IPolyline))
                    Exit Select
                Case EntityType.Polyline
                    Me.m_polylines.Add(DirectCast(entity, IPolyline))
                    Exit Select
                Case EntityType.Polyline3d
                    Me.m_polylines.Add(DirectCast(entity, IPolyline))
                    Exit Select
                Case EntityType.PolyfaceMesh
                    Me.m_polylines.Add(DirectCast(entity, IPolyline))
                    Exit Select
                Case EntityType.Text
                    If Not Me.m_textStyles.ContainsKey(DirectCast(entity, Text).Style.Name) Then
                        Me.m_textStyles.Add(DirectCast(entity, Text).Style.Name, DirectCast(entity, Text).Style)
                    End If
                    Me.m_texts.Add(DirectCast(entity, Text))
                    Exit Select
                Case EntityType.Vertex
                    Throw New ArgumentException("The entity " & Convert.ToString(entity.Type) & " is only allowed as part of another entity", "entity")

                Case EntityType.PolylineVertex
                    Throw New ArgumentException("The entity " & Convert.ToString(entity.Type) & " is only allowed as part of another entity", "entity")

                Case EntityType.Polyline3dVertex
                    Throw New ArgumentException("The entity " & Convert.ToString(entity.Type) & " is only allowed as part of another entity", "entity")

                Case EntityType.PolyfaceMeshVertex
                    Throw New ArgumentException("The entity " & Convert.ToString(entity.Type) & " is only allowed as part of another entity", "entity")

                Case EntityType.PolyfaceMeshFace
                    Throw New ArgumentException("The entity " & Convert.ToString(entity.Type) & " is only allowed as part of another entity", "entity")

                Case EntityType.AttributeDefinition
                    Throw New ArgumentException("The entity " & Convert.ToString(entity.Type) & " is only allowed as part of another entity", "entity")

                Case EntityType.Attribute
                    Throw New ArgumentException("The entity " & Convert.ToString(entity.Type) & " is only allowed as part of another entity", "entity")
                Case Else

                    Throw New NotImplementedException("The entity " & Convert.ToString(entity.Type) & " is not implemented or unknown")
            End Select
        End SyncLock
    End Sub

    ''' <summary>
    ''' Loads a dxf ASCII file.
    ''' </summary>
    ''' <param name="file">File name.</param>
    Public Sub Load(ByVal file__1 As String)
        SyncLock _locker
            If Not File.Exists(file__1) Then
                Throw New FileNotFoundException("File " & file__1 & " not found.", file__1)
            End If

            Me.m_fileName = Path.GetFileNameWithoutExtension(file__1)

            Dim cultureInfo__2 As CultureInfo = CultureInfo.CurrentCulture
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture

            Dim dxfReader As New DxfReader(file__1)
            dxfReader.Open()
            dxfReader.Read()
            dxfReader.Close()

            'header information
            Me.m_version = dxfReader.Version
            Me.handleCount = Convert.ToInt32(dxfReader.HandleSeed, 16)

            'tables information
            Me.m_appRegisterNames = dxfReader.ApplicationRegistrationIds
            Me.m_layers = dxfReader.Layers
            Me.m_lineTypes = dxfReader.LineTypes
            Me.m_textStyles = dxfReader.TextStyles
            Me.m_blocks = dxfReader.Blocks

            'entities information
            Me.m_arcs = dxfReader.Arcs
            Me.m_circles = dxfReader.Circles
            Me.m_ellipses = dxfReader.Ellipses
            Me.m_points = dxfReader.Points
            Me.m_faces3d = dxfReader.Faces3d
            Me.m_solids = dxfReader.Solids
            Me.m_polylines = dxfReader.Polylines
            Me.m_lines = dxfReader.Lines
            Me.m_inserts = dxfReader.Inserts
            Me.m_texts = dxfReader.Texts

            Thread.CurrentThread.CurrentCulture = cultureInfo__2

            '#Region "reasign handles"
            'add default viewports
            'foreach (ViewPort viewPort in this.viewports.Values )
            '{
            '    this.handleCount = viewPort.AsignHandle(this.handleCount);
            '}

            '''/add default layer
            'Layer.PlotStyleHandle = Conversion.Hex(this.handleCount++);
            'Layer byDefault = Layer.Default;
            'this.layers.Add(byDefault.Name, byDefault);
            'this.handleCount = byDefault.AsignHandle(this.handleCount);

            '''/ add default line types
            'LineType byLayer = LineType.ByLayer;
            'LineType byBlock = LineType.ByBlock;
            'this.lineTypes.Add(byLayer.Name, byLayer);
            'this.handleCount = byLayer.AsignHandle(this.handleCount);
            'this.lineTypes.Add(byBlock.Name, byBlock);
            'this.handleCount = byBlock.AsignHandle(this.handleCount);

            '''/ add default text style
            'TextStyle defaultStyle = TextStyle.Default;
            'this.textStyles.Add(defaultStyle.Name, defaultStyle);
            'this.handleCount = defaultStyle.AsignHandle(this.handleCount);

            '''/ add default blocks
            'Block modelSpace = Block.ModelSpace;
            'Block paperSpace = Block.PaperSpace;
            'this.blocks.Add(modelSpace.Name, modelSpace);
            'this.handleCount = modelSpace.AsignHandle(this.handleCount);
            'this.blocks.Add(paperSpace.Name, paperSpace);
            'this.handleCount = paperSpace.AsignHandle(this.handleCount);

            '''/ add default application registry
            'ApplicationRegistry defaultAppId = ApplicationRegistry.Default;
            'this.appRegisterNames.Add(defaultAppId.Name, defaultAppId);
            'this.handleCount = defaultAppId.AsignHandle(this.handleCount);

            'add default dimension style
            'foreach (DimensionStyle dimStyle in this.dimStyles.Values)
            '{
            '    this.handleCount = dimStyle.AsignHandle(this.handleCount);
            '}

            'foreach (Block block in this.blocks.Values)
            '{
            '    this.handleCount = block.AsignHandle(this.handleCount);
            '}

            '
        End SyncLock
    End Sub

    ''' <summary>
    ''' Saves the database of the actual DxfDocument to a dxf ASCII file.
    ''' </summary>
    ''' <param name="file">File name.</param>
    ''' <param name="dxfVersion">Dxf file <see cref="DxfVersion">version</see>.</param>
    Public Sub Save(ByVal file As String, ByVal dxfVersion__1 As DxfVersion)
        SyncLock _locker
            ReAsignHandlersAndDefaultObjects()
            Me.m_fileName = Path.GetFileNameWithoutExtension(file)
            Me.m_version = dxfVersion__1

            Dim ellipsePolys As List(Of Polyline) = Nothing
            Dim lwPolys As List(Of IPolyline)
            Dim blockEntities As Dictionary(Of String, List(Of IEntityObject))
            If Me.m_version = DxfVersion.AutoCad12 Then
                ' since AutoCad dxf Version 12 doesn't support ellipses, we will transform them in polylines
                ellipsePolys = New List(Of Polyline)()
                For Each ellipse As Ellipse In Me.m_ellipses
                    Dim poly As Polyline = ellipse.ToPolyline(ellipse.CurvePoints)
                    Me.handleCount = poly.AsignHandle(Me.handleCount)
                    ellipsePolys.Add(poly)
                Next

                ' since AutoCad dxf Version 12 doesn't support lwpolylines, we will transform them in polylines
                lwPolys = New List(Of IPolyline)()
                For Each lwPoly As IPolyline In Me.m_polylines
                    If TypeOf lwPoly Is LightWeightPolyline Then
                        Dim poly As Polyline = DirectCast(lwPoly, LightWeightPolyline).ToPolyline()
                        Me.handleCount = poly.AsignHandle(Me.handleCount)
                        lwPolys.Add(poly)
                    Else
                        lwPolys.Add(lwPoly)

                    End If
                Next

                ' since AutoCad dxf Version 12 doesn't support lwpolylines in blocks, we will transform them in polylines
                blockEntities = New Dictionary(Of String, List(Of IEntityObject))()
                For Each block As Block In Me.m_blocks.Values
                    blockEntities.Add(block.Name, New List(Of IEntityObject)())
                    For Each entity As IEntityObject In block.Entities
                        If TypeOf entity Is LightWeightPolyline Then
                            Dim poly As Polyline = DirectCast(entity, LightWeightPolyline).ToPolyline()
                            Me.handleCount = poly.AsignHandle(Me.handleCount)
                            blockEntities(block.Name).Add(poly)
                        Else
                            blockEntities(block.Name).Add(entity)
                        End If
                    Next
                Next
            Else
                ' since AutoCad dxf Version 12 doesn't support lwpolylines, we will transform them in polylines
                lwPolys = New List(Of IPolyline)()
                For Each lwPoly As IPolyline In Me.m_polylines
                    If (TypeOf lwPoly Is Polyline) Then
                        Dim poly As LightWeightPolyline = DirectCast(lwPoly, Polyline).ToLightWeightPolyline()
                        Me.handleCount = poly.AsignHandle(Me.handleCount)
                        lwPolys.Add(poly)
                    Else
                        lwPolys.Add(lwPoly)
                    End If
                Next

                ' since latter AutoCad dxf Version doesn't support polylines in blocks, we will transform them in lightweightpolylines
                blockEntities = New Dictionary(Of String, List(Of IEntityObject))()
                For Each block As Block In Me.m_blocks.Values
                    blockEntities.Add(block.Name, New List(Of IEntityObject)())
                    For Each entity As IEntityObject In block.Entities
                        If TypeOf entity Is Polyline Then
                            Dim poly As LightWeightPolyline = DirectCast(entity, Polyline).ToLightWeightPolyline()
                            Me.handleCount = poly.AsignHandle(Me.handleCount)
                            blockEntities(block.Name).Add(poly)
                        Else
                            blockEntities(block.Name).Add(entity)
                        End If
                    Next
                Next
            End If

            Dim cultureInfo__2 As CultureInfo = CultureInfo.CurrentCulture
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture

            Dim dxfWriter As New DxfWriter(file, dxfVersion__1)
            dxfWriter.Open()
            dxfWriter.WriteComment("Dxf file generated by g10Dxf, Copyright(C) 2009 Daniel Carvajal, Licensed under LGPL")

            'HEADER SECTION
            dxfWriter.BeginSection(StringCode.HeaderSection)
            dxfWriter.WriteSystemVariable(New HeaderVariable(SystemVariable.DabaseVersion, StringEnum.GetStringValue(Me.m_version)))
            dxfWriter.WriteSystemVariable(New HeaderVariable(SystemVariable.HandSeed, Convert.ToString(Me.handleCount, 16)))
            dxfWriter.EndSection()

            '''/CLASSES SECTION
            'dxfWriter.BeginSection(StringCode.ClassesSection);
            'dxfWriter.EndSection();

            'TABLES SECTION
            dxfWriter.BeginSection(StringCode.TablesSection)

            'viewport tables
            If Me.m_version <> DxfVersion.AutoCad12 Then
                dxfWriter.BeginTable(StringCode.ViewPortTable)
                For Each vport As ViewPort In Me.viewports.Values
                    dxfWriter.WriteViewPort(vport)
                Next
                dxfWriter.EndTable()
            End If

            'line type tables
            dxfWriter.BeginTable(StringCode.LineTypeTable)
            For Each lineType As LineType In Me.m_lineTypes.Values
                dxfWriter.WriteLineType(lineType)
            Next
            dxfWriter.EndTable()

            'layer tables
            dxfWriter.BeginTable(StringCode.LayerTable)
            For Each layer As Layer In Me.m_layers.Values
                dxfWriter.WriteLayer(layer)
            Next
            dxfWriter.EndTable()

            'text style tables
            dxfWriter.BeginTable(StringCode.TextStyleTable)
            For Each style As TextStyle In Me.m_textStyles.Values
                dxfWriter.WriteTextStyle(style)
            Next
            dxfWriter.EndTable()

            'view
            dxfWriter.BeginTable(StringCode.ViewTable)
            dxfWriter.EndTable()

            'ucs
            dxfWriter.BeginTable(StringCode.UcsTable)
            dxfWriter.EndTable()

            'registered application tables
            dxfWriter.BeginTable(StringCode.ApplicationIDTable)
            For Each id As ApplicationRegistry In Me.m_appRegisterNames.Values
                dxfWriter.RegisterApplication(id)
            Next
            dxfWriter.EndTable()

            'dimension style tables
            If Me.m_version <> DxfVersion.AutoCad12 Then
                dxfWriter.BeginTable(StringCode.DimensionStyleTable)
                For Each style As DimensionStyle In Me.dimStyles.Values
                    dxfWriter.WriteDimensionStyle(style)
                Next
                dxfWriter.EndTable()
            End If

            'block record tables, this table isnot recognized by AutoCad12
            If Me.m_version <> DxfVersion.AutoCad12 Then
                dxfWriter.BeginTable(StringCode.BlockRecordTable)
                For Each block As Block In Me.m_blocks.Values
                    dxfWriter.WriteBlockRecord(block.Record)
                Next
                dxfWriter.EndTable()
            End If

            dxfWriter.EndSection()
            'End section tables
            dxfWriter.BeginSection(StringCode.BlocksSection)
            For Each block As Block In Me.m_blocks.Values
                dxfWriter.WriteBlock(block, blockEntities(block.Name))
            Next

            dxfWriter.EndSection()
            'End section blocks
            'ENTITIES SECTION
            dxfWriter.BeginSection(StringCode.EntitiesSection)

            '#Region "writting entities"

            For Each arc As Arc In Me.m_arcs
                dxfWriter.WriteEntity(arc)
            Next
            For Each circle As Circle In Me.m_circles
                dxfWriter.WriteEntity(circle)
            Next

            ' only for version 12 draw polylines instead of ellipses
            If Me.m_version = DxfVersion.AutoCad12 Then
                If ellipsePolys IsNot Nothing Then
                    For Each ellipse As Polyline In ellipsePolys
                        dxfWriter.WriteEntity(ellipse)
                    Next
                End If
            Else
                For Each ellipse As Ellipse In Me.m_ellipses
                    dxfWriter.WriteEntity(ellipse)
                Next
            End If
            For Each nurbsCurve As NurbsCurve In Me.m_nurbsCurves
                dxfWriter.WriteEntity(nurbsCurve)
            Next
            For Each point As Point In Me.m_points
                dxfWriter.WriteEntity(point)
            Next
            For Each face As Face3d In Me.m_faces3d
                dxfWriter.WriteEntity(face)
            Next
            For Each solid As Solid In Me.m_solids
                dxfWriter.WriteEntity(solid)
            Next
            For Each insert As Insert In Me.m_inserts
                dxfWriter.WriteEntity(insert)
            Next
            For Each line As Line In Me.m_lines
                dxfWriter.WriteEntity(line)
            Next

            ' lwpolyline in Acad12 are written as polylines

            For Each pol As IPolyline In lwPolys
                dxfWriter.WriteEntity(pol)
            Next

            'foreach (IPolyline polyline in this.polylines)
            '{
            '    // avoid write lwpolylines in Acad12
            '    if (this.version != DxfVersion.AutoCad12 || !(polyline is LightWeightPolyline))
            '    {
            '        dxfWriter.WriteEntity(polyline);
            '    }
            '}

            For Each text As Text In Me.m_texts
                dxfWriter.WriteEntity(text)
            Next
            '

            dxfWriter.EndSection()
            'End section entities
            'OBJECTS SECTION
            dxfWriter.BeginSection(StringCode.ObjectsSection)
            dxfWriter.WriteDictionary(Dictionary.[Default])
            dxfWriter.EndSection()

            dxfWriter.Close()

            Thread.CurrentThread.CurrentCulture = cultureInfo__2
        End SyncLock
    End Sub

    Private Sub ReAsignHandlersAndDefaultObjects()
        SyncLock _locker
            Me.handleCount = 100

            'add default viewports
            Dim active As ViewPort = ViewPort.Active
            If Not Me.viewports.ContainsKey(active.Name) Then
                Me.viewports.Add(active.Name, active)
            End If
            For Each viewPort__1 As ViewPort In Me.viewports.Values
                Me.handleCount = viewPort__1.AsignHandle(Me.handleCount)
            Next

            'add default layer
            Layer.PlotStyleHandle = Convert.ToString(System.Math.Max(System.Threading.Interlocked.Increment(Me.handleCount), Me.handleCount - 1), 16)
            Dim byDefault As Layer = Layer.[Default]
            If Not Me.m_layers.ContainsKey(byDefault.Name) Then
                Me.m_layers.Add(byDefault.Name, byDefault)
            End If
            For Each layer__2 As Layer In Me.m_layers.Values
                Me.handleCount = layer__2.AsignHandle(Me.handleCount)
            Next

            ' add default line types
            Dim byLayer As LineType = LineType.ByLayer
            Dim byBlock As LineType = LineType.ByBlock
            If Not Me.m_lineTypes.ContainsKey(byLayer.Name) Then
                Me.m_lineTypes.Add(byLayer.Name, byLayer)
            End If
            If Not Me.m_lineTypes.ContainsKey(byBlock.Name) Then
                Me.m_lineTypes.Add(byBlock.Name, byBlock)
            End If
            For Each lineType__3 As LineType In Me.m_lineTypes.Values
                Me.handleCount = lineType__3.AsignHandle(Me.handleCount)
            Next

            ' add default text style
            Dim defaultStyle As TextStyle = TextStyle.[Default]
            If Not Me.m_textStyles.ContainsKey(defaultStyle.Name) Then
                Me.m_textStyles.Add(defaultStyle.Name, defaultStyle)
            End If
            For Each textStyle__4 As TextStyle In Me.m_textStyles.Values
                Me.handleCount = textStyle__4.AsignHandle(Me.handleCount)
            Next

            ' add default blocks
            Dim modelSpace As Block = Block.ModelSpace
            Dim paperSpace As Block = Block.PaperSpace
            If Not Me.m_blocks.ContainsKey(modelSpace.Name) Then
                Me.m_blocks.Add(modelSpace.Name, modelSpace)
            End If
            If Not Me.m_blocks.ContainsKey(paperSpace.Name) Then
                Me.m_blocks.Add(paperSpace.Name, paperSpace)
            End If
            For Each block__5 As Block In Me.m_blocks.Values
                Me.handleCount = block__5.AsignHandle(Me.handleCount)
            Next

            ' add default application registry
            Dim defaultAppId As ApplicationRegistry = ApplicationRegistry.[Default]
            If Not Me.m_appRegisterNames.ContainsKey(defaultAppId.Name) Then
                Me.m_appRegisterNames.Add(defaultAppId.Name, defaultAppId)
            End If
            For Each appId As ApplicationRegistry In Me.m_appRegisterNames.Values
                Me.handleCount = appId.AsignHandle(Me.handleCount)
            Next

            'add default dimension style
            Dim defaultDimStyle As DimensionStyle = DimensionStyle.[Default]
            If Not Me.dimStyles.ContainsKey(defaultDimStyle.Name) Then
                Me.dimStyles.Add(defaultDimStyle.Name, defaultDimStyle)
            End If
            For Each style As DimensionStyle In Me.dimStyles.Values
                Me.handleCount = style.AsignHandle(Me.handleCount)
            Next

            For Each entity As Arc In Me.m_arcs
                Me.handleCount = entity.AsignHandle(Me.handleCount)
            Next
            For Each entity As Ellipse In Me.m_ellipses
                Me.handleCount = entity.AsignHandle(Me.handleCount)
            Next
            For Each entity As Face3d In Me.m_faces3d
                Me.handleCount = entity.AsignHandle(Me.handleCount)
            Next
            For Each entity As Solid In Me.m_solids
                Me.handleCount = entity.AsignHandle(Me.handleCount)
            Next
            For Each entity As Insert In Me.m_inserts
                Me.handleCount = entity.AsignHandle(Me.handleCount)
            Next
            For Each entity As IPolyline In Me.m_polylines
                Me.handleCount = DirectCast(entity, DxfObject).AsignHandle(Me.handleCount)
            Next
            For Each entity As Line In Me.m_lines
                Me.handleCount = entity.AsignHandle(Me.handleCount)
            Next
            For Each entity As Circle In Me.m_circles
                Me.handleCount = entity.AsignHandle(Me.handleCount)
            Next
            For Each entity As Point In Me.m_points
                Me.handleCount = entity.AsignHandle(Me.handleCount)
            Next
            For Each entity As Text In Me.m_texts
                Me.handleCount = entity.AsignHandle(Me.handleCount)
            Next
        End SyncLock
    End Sub

End Class

