

Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.IO
Imports System.Text
Imports pccDXF4.Blocks
Imports pccDXF4.Entities
Imports pccDXF4.Header
Imports pccDXF4.Tables
Imports Attribute = pccDXF4.Entities.Attribute


''' <summary>
''' Low level dxf reader
''' </summary>
Friend NotInheritable Class DxfReader

    Private ReadOnly file As String
    Private fileLine As Integer
    Private m_isFileOpen As Boolean
    Private input As Stream
    Private reader As StreamReader

    'header
    Private m_comments As List(Of String)
    Private m_version As Header.DxfVersion
    Private m_handleSeed As String

    'entities
    Private m_arcs As List(Of Entities.Arc)
    Private m_circles As List(Of Entities.Circle)
    Private m_points As List(Of Entities.Point)
    Private m_ellipses As List(Of Entities.Ellipse)
    Private m_faces3d As List(Of Entities.Face3d)
    Private m_solids As List(Of Entities.Solid)
    Private m_inserts As List(Of Entities.Insert)
    Private m_lines As List(Of Entities.Line)
    Private m_polylines As List(Of Entities.IPolyline)
    Private m_texts As List(Of Entities.Text)

    'tables
    Private appIds As Dictionary(Of String, Tables.ApplicationRegistry)
    Private m_layers As Dictionary(Of String, Tables.Layer)
    Private m_lineTypes As Dictionary(Of String, Tables.LineType)
    Private m_textStyles As Dictionary(Of String, Tables.TextStyle)

    'blocks
    Private m_blocks As Dictionary(Of String, Blocks.Block)

    Private Shared ReadOnly _locker As New Object()

    Public Sub New(ByVal file As String)
        SyncLock _locker
            Me.file = file
        End SyncLock
    End Sub


    Public ReadOnly Property IsFileOpen() As Boolean
        Get
            SyncLock _locker
                Return Me.m_isFileOpen
            End SyncLock
        End Get
    End Property

    Public ReadOnly Property Comments() As List(Of String)
        Get
            SyncLock _locker
                Return Me.m_comments
            End SyncLock
        End Get
    End Property



    Public ReadOnly Property HandleSeed() As String
        Get
            SyncLock _locker
                Return Me.m_handleSeed
            End SyncLock
        End Get
    End Property

    Public ReadOnly Property Version() As DxfVersion
        Get
            SyncLock _locker
                Return Me.m_version
            End SyncLock
        End Get
    End Property



    Public ReadOnly Property Arcs() As List(Of Arc)
        Get
            SyncLock _locker
                Return Me.m_arcs
            End SyncLock
        End Get
    End Property

    Public ReadOnly Property Circles() As List(Of Circle)
        Get
            SyncLock _locker
                Return Me.m_circles
            End SyncLock
        End Get
    End Property

    Public ReadOnly Property Ellipses() As List(Of Ellipse)
        Get
            SyncLock _locker
                Return Me.m_ellipses
            End SyncLock
        End Get
    End Property

    Public ReadOnly Property Points() As List(Of Point)
        Get
            SyncLock _locker
                Return Me.m_points
            End SyncLock
        End Get
    End Property

    Public ReadOnly Property Faces3d() As List(Of Face3d)
        Get
            SyncLock _locker
                Return Me.m_faces3d
            End SyncLock
        End Get
    End Property

    Public ReadOnly Property Solids() As List(Of Solid)
        Get
            SyncLock _locker
                Return Me.m_solids
            End SyncLock
        End Get
    End Property

    Public ReadOnly Property Lines() As List(Of Line)
        Get
            SyncLock _locker
                Return Me.m_lines
            End SyncLock
        End Get
    End Property

    Public ReadOnly Property Polylines() As List(Of IPolyline)
        Get
            SyncLock _locker
                Return Me.m_polylines
            End SyncLock
        End Get
    End Property

    Public ReadOnly Property Inserts() As List(Of Insert)
        Get
            SyncLock _locker
                Return Me.m_inserts
            End SyncLock
        End Get
    End Property

    Public ReadOnly Property Texts() As List(Of Text)
        Get
            SyncLock _locker
                Return Me.m_texts
            End SyncLock
        End Get
    End Property


    Public ReadOnly Property ApplicationRegistrationIds() As Dictionary(Of String, ApplicationRegistry)
        Get
            SyncLock _locker
                Return Me.appIds
            End SyncLock
        End Get
    End Property

    Public ReadOnly Property Layers() As Dictionary(Of String, Layer)
        Get
            SyncLock _locker
                Return Me.m_layers
            End SyncLock
        End Get
    End Property

    Public ReadOnly Property LineTypes() As Dictionary(Of String, LineType)
        Get
            SyncLock _locker
                Return Me.m_lineTypes
            End SyncLock
        End Get
    End Property

    Public ReadOnly Property TextStyles() As Dictionary(Of String, TextStyle)
        Get
            Return Me.m_textStyles
        End Get
    End Property

    Public ReadOnly Property Blocks() As Dictionary(Of String, Block)
        Get
            SyncLock _locker
                Return Me.m_blocks
            End SyncLock
        End Get
    End Property



    Public Sub Open()
        SyncLock _locker
            Try
                Me.input = System.IO.File.OpenRead(Me.file)
                Me.reader = New StreamReader(Me.input, Encoding.ASCII)
                Me.m_isFileOpen = True

            Catch ex As Exception

                Throw (New DxfException(Me.file, "Error al intentar abrir el archivo.", ex))

            End Try
        End SyncLock
    End Sub

    Public Sub Close()
        SyncLock _locker
            If Me.m_isFileOpen Then
                Me.reader.Close()
                Me.input.Close()
            End If
            Me.m_isFileOpen = False
        End SyncLock
    End Sub

    Public Sub Read()
        SyncLock _locker
            Me.m_comments = New List(Of String)()
            Me.appIds = New Dictionary(Of String, ApplicationRegistry)()
            Me.m_layers = New Dictionary(Of String, Layer)()
            Me.m_lineTypes = New Dictionary(Of String, LineType)()
            Me.m_textStyles = New Dictionary(Of String, TextStyle)()
            Me.m_blocks = New Dictionary(Of String, Block)()

            Me.m_arcs = New List(Of Arc)()
            Me.m_circles = New List(Of Circle)()
            Me.m_faces3d = New List(Of Face3d)()
            Me.m_ellipses = New List(Of Ellipse)()
            Me.m_solids = New List(Of Solid)()
            Me.m_inserts = New List(Of Insert)()
            Me.m_lines = New List(Of Line)()
            Me.m_polylines = New List(Of IPolyline)()
            Me.m_points = New List(Of Point)()
            Me.m_texts = New List(Of Text)()
            Me.fileLine = -1

            Dim code As CodeValuePair = Me.ReadCodePair()
            While code.Value <> StringCode.EndOfFile
                If code.Value = StringCode.BeginSection Then
                    code = Me.ReadCodePair()
                    Select Case code.Value
                        Case StringCode.HeaderSection
                            Me.ReadHeader()
                            Exit Select
                        Case StringCode.ClassesSection
                            Me.ReadClasses()
                            Exit Select
                        Case StringCode.TablesSection
                            Me.ReadTables()
                            Exit Select
                        Case StringCode.BlocksSection
                            Me.ReadBlocks()
                            Exit Select
                        Case StringCode.EntitiesSection
                            Me.ReadEntities()
                            Exit Select
                        Case StringCode.ObjectsSection
                            Me.ReadObjects()
                            Exit Select
                        Case Else
                            Throw New InvalidDxfSectionException(code.Value, Me.file, "Unknown section " + code.Value & " line " & Me.fileLine)
                    End Select
                End If
                code = Me.ReadCodePair()
            End While
        End SyncLock
    End Sub


    Private Sub ReadHeader()
        SyncLock _locker
            Dim code As CodeValuePair = Me.ReadCodePair()
            While code.Value <> StringCode.EndSection
                If HeaderVariable.Allowed.ContainsKey(code.Value) Then
                    Dim codeGroup As Integer = HeaderVariable.Allowed(code.Value)
                    Dim variableName As String = code.Value
                    code = Me.ReadCodePair()
                    If code.Code <> codeGroup Then
                        Throw New DxfHeaderVariableException(variableName, Me.file, "Invalid variable name and code group convination in line " & Me.fileLine)
                    End If
                    Select Case variableName
                        Case SystemVariable.DabaseVersion
                            Me.m_version = DirectCast(StringEnum.Parse(GetType(DxfVersion), code.Value), DxfVersion)
                            Exit Select
                        Case SystemVariable.HandSeed
                            Me.m_handleSeed = code.Value
                            Exit Select

                    End Select
                End If
                code = Me.ReadCodePair()
            End While
        End SyncLock
    End Sub

    Private Sub ReadClasses()
        SyncLock _locker
            Dim code As CodeValuePair = Me.ReadCodePair()
            While code.Value <> StringCode.EndSection
                code = Me.ReadCodePair()
            End While
        End SyncLock
    End Sub

    Private Sub ReadTables()
        SyncLock _locker
            Dim code As CodeValuePair = Me.ReadCodePair()
            While code.Value <> StringCode.EndSection
                code = Me.ReadCodePair()
                Select Case code.Value
                    Case StringCode.ApplicationIDTable
                        Debug.Assert(code.Code = 2)
                        Me.ReadApplicationsId()
                        Exit Select
                    Case StringCode.LayerTable
                        Debug.Assert(code.Code = 2)
                        Me.ReadLayers()
                        Exit Select
                    Case StringCode.LineTypeTable
                        Debug.Assert(code.Code = 2)
                        Me.ReadLineTypes()
                        Exit Select
                    Case StringCode.TextStyleTable
                        Debug.Assert(code.Code = 2)
                        Me.ReadTextStyles()
                        Exit Select
                End Select
            End While
        End SyncLock
    End Sub

    Private Sub ReadApplicationsId()
        SyncLock _locker
            Dim code As CodeValuePair = Me.ReadCodePair()
            While code.Value <> StringCode.EndTable
                If code.Value = StringCode.ApplicationIDTable Then
                    Debug.Assert(code.Code = 0)
                    Dim appId As ApplicationRegistry = Me.ReadApplicationId(code)
                    Me.appIds.Add(appId.Name, appId)
                Else
                    code = Me.ReadCodePair()
                End If
            End While
                End SyncLock 
    End Sub

    Private Function ReadApplicationId(ByRef code As CodeValuePair) As ApplicationRegistry
        SyncLock _locker
            Dim appId As String = String.Empty
            Dim handle As String = String.Empty
            code = Me.ReadCodePair()

            While code.Code <> 0
                Select Case code.Code
                    Case 2
                        If String.IsNullOrEmpty(code.Value) Then
                            Throw New DxfInvalidCodeValueEntityException(code.Code, code.Value, Me.file, "Invalid value " & Convert.ToString(code.Value) & " in code " & Convert.ToString(code.Code) & " line " & Me.fileLine)
                        End If
                        appId = code.Value
                        Exit Select
                    Case 5
                        handle = code.Value
                        Exit Select
                End Select
                code = Me.ReadCodePair()
            End While
            Return New ApplicationRegistry(appId) With { _
                .Handle = handle _
             }
        End SyncLock
    End Function

    Private Sub ReadBlocks()
        SyncLock _locker
            Dim code As CodeValuePair = Me.ReadCodePair()
            While code.Value <> StringCode.EndSection
                Select Case code.Value
                    Case StringCode.BeginBlock
                        Dim block As Block = Me.ReadBlock(code)
                        Me.m_blocks.Add(block.Name, block)
                        Exit Select
                    Case Else
                        code = Me.ReadCodePair()
                        Exit Select
                End Select
            End While
        End SyncLock
    End Sub

    Private Function ReadBlock(ByRef code As CodeValuePair) As Block
        SyncLock _locker
            Dim layer As Layer = Nothing
            Dim name As String = String.Empty
            Dim handle As String = String.Empty
            Dim basePoint As Vector3f = Vector3f.Zero
            Dim entities As New List(Of IEntityObject)()
            Dim attdefs As New Dictionary(Of String, AttributeDefinition)()

            code = Me.ReadCodePair()
            While code.Value <> StringCode.EndBlock
                Select Case code.Code
                    Case 5
                        handle = code.Value
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 8
                        layer = Me.GetLayer(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 2
                        name = code.Value
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 10
                        basePoint.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 20
                        basePoint.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 30
                        basePoint.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 3
                        name = code.Value
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 0
                        ' entity
                        Dim entity As IEntityObject
                        entity = Me.ReadBlockEntity(code)
                        If entity IsNot Nothing Then
                            If entity.Type = EntityType.AttributeDefinition Then
                                attdefs.Add(DirectCast(entity, AttributeDefinition).Id, DirectCast(entity, AttributeDefinition))
                            Else
                                entities.Add(entity)
                            End If
                        End If
                        Exit Select
                    Case Else
                        code = Me.ReadCodePair()
                        Exit Select
                End Select
            End While

            ' read the end bloc object until a new element is found
            code = Me.ReadCodePair()
            Dim endBlockHandle As String = String.Empty
            Dim endBlockLayer As Layer = layer
            While code.Code <> 0
                Select Case code.Code
                    Case 5
                        endBlockHandle = code.Value
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 8
                        endBlockLayer = Me.GetLayer(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case Else
                        code = Me.ReadCodePair()
                        Exit Select
                End Select
            End While
            Dim block As New pccDXF4.Blocks.Block(name) With {.BasePoint = basePoint, .Layer = layer, .Entities = entities, .Attributes = attdefs, .Handle = handle}
            block.[End].Handle = endBlockHandle
            block.[End].Layer = endBlockLayer

            Return block
        End SyncLock
    End Function

    Private Function ReadBlockEntity(ByRef code As CodeValuePair) As IEntityObject
        SyncLock _locker
            Dim entity As IEntityObject = Nothing

            Select Case code.Value
                Case DxfObjectCode.Arc
                    entity = Me.ReadArc(code)
                    Exit Select
                Case DxfObjectCode.Circle
                    entity = Me.ReadCircle(code)
                    Exit Select
                Case DxfObjectCode.Face3D
                    entity = Me.ReadFace3D(code)
                    Exit Select
                Case DxfObjectCode.Solid
                    entity = Me.ReadSolid(code)
                    Exit Select
                Case DxfObjectCode.Insert
                    code = Me.ReadCodePair()
                    Exit Select
                Case DxfObjectCode.Line
                    entity = Me.ReadLine(code)
                    Exit Select
                Case DxfObjectCode.LightWeightPolyline
                    entity = Me.ReadLightWeightPolyline(code)
                    Exit Select
                Case DxfObjectCode.Polyline
                    entity = Me.ReadPolyline(code)
                    Exit Select
                Case DxfObjectCode.Text
                    entity = Me.ReadText(code)
                    Exit Select
                Case DxfObjectCode.AttributeDefinition
                    entity = Me.ReadAttributeDefinition(code)
                    Exit Select
                Case Else
                    ReadUnknowEntity(code)
                    'code = this.ReadCodePair();
                    Exit Select
            End Select

            Return entity
        End SyncLock
    End Function

    Private Function ReadAttributeDefinition(ByRef code As CodeValuePair) As AttributeDefinition
        SyncLock _locker
            Dim handle As String = String.Empty
            Dim id As String = String.Empty
            Dim text As String = String.Empty
            Dim value As Object = Nothing
            Dim flags As AttributeFlags = AttributeFlags.Visible
            Dim firstAlignmentPoint As Vector3f = Vector3f.Zero
            Dim secondAlignmentPoint As Vector3f = Vector3f.Zero
            Dim layer__1 As Layer = Layer.[Default]
            Dim color As AciColor = AciColor.ByLayer
            Dim lineType__2 As LineType = LineType.ByLayer
            Dim style As TextStyle = TextStyle.[Default]
            Dim height As Single = 0
            Dim widthFactor As Single = 0
            Dim horizontalAlignment As Integer = 0
            Dim verticalAlignment As Integer = 0
            Dim rotation As Single = 0
            Dim normal As Vector3f = Vector3f.UnitZ

            code = Me.ReadCodePair()
            While code.Code <> 0
                Select Case code.Code
                    Case 5
                        handle = code.Value
                        Exit Select
                    Case 2
                        id = code.Value
                        Exit Select
                    Case 3
                        text = code.Value
                        Exit Select
                    Case 1
                        value = code.Value
                        Exit Select
                    Case 8
                        'layer code
                        layer__1 = Me.GetLayer(code.Value)
                        Exit Select
                    Case 62
                        'aci color code
                        color = New AciColor(Short.Parse(code.Value))
                        Exit Select
                    Case 6
                        'type line code
                        lineType__2 = Me.GetLineType(code.Value)
                        Exit Select
                    Case 70
                        flags = DirectCast(Integer.Parse(code.Value), AttributeFlags)
                        Exit Select
                    Case 10
                        firstAlignmentPoint.X = Single.Parse(code.Value)
                        Exit Select
                    Case 20
                        firstAlignmentPoint.Y = Single.Parse(code.Value)
                        Exit Select
                    Case 30
                        firstAlignmentPoint.Z = Single.Parse(code.Value)
                        Exit Select
                    Case 11
                        secondAlignmentPoint.X = Single.Parse(code.Value)
                        Exit Select
                    Case 21
                        secondAlignmentPoint.Y = Single.Parse(code.Value)
                        Exit Select
                    Case 31
                        secondAlignmentPoint.Z = Single.Parse(code.Value)
                        Exit Select
                    Case 7
                        style = Me.GetTextStyle(code.Value)
                        Exit Select
                    Case 40
                        height = Single.Parse(code.Value)
                        Exit Select
                    Case 41
                        widthFactor = Single.Parse(code.Value)
                        Exit Select
                    Case 50
                        rotation = Single.Parse(code.Value)
                        Exit Select
                    Case 72
                        horizontalAlignment = Integer.Parse(code.Value)
                        Exit Select
                    Case 74
                        verticalAlignment = Integer.Parse(code.Value)
                        Exit Select
                    Case 210
                        normal.X = Single.Parse(code.Value)
                        Exit Select
                    Case 220
                        normal.Y = Single.Parse(code.Value)
                        Exit Select
                    Case 230
                        normal.Z = Single.Parse(code.Value)
                        Exit Select
                End Select

                code = Me.ReadCodePair()
            End While

            Dim alignment As TextAlignment = ObtainAlignment(horizontalAlignment, verticalAlignment)

            Return New AttributeDefinition(id) With { _
              .BasePoint = (If(alignment = TextAlignment.BaselineLeft, firstAlignmentPoint, secondAlignmentPoint)), _
              .Normal = normal, _
              .Alignment = alignment, _
             .Text = text, _
             .Value = value, _
             .Flags = flags, _
             .Layer = layer__1, _
             .Color = color, _
             .LineType = lineType__2, _
             .Style = style, _
             .Height = height, _
             .WidthFactor = widthFactor, _
             .Rotation = rotation, _
             .Handle = handle _
            }
        End SyncLock
    End Function

    Private Function ReadAttribute(ByVal block As Block, ByRef code As CodeValuePair) As Attribute
        SyncLock _locker
            Dim handle As String = String.Empty
            Dim attdef As AttributeDefinition = Nothing
            Dim layer__1 As Layer = Layer.[Default]
            Dim color As AciColor = AciColor.ByLayer
            Dim lineType__2 As LineType = LineType.ByLayer
            Dim value As [Object] = Nothing
            code = Me.ReadCodePair()
            While code.Code <> 0
                Select Case code.Code
                    Case 5
                        handle = code.Value
                        Exit Select
                    Case 2
                        attdef = block.Attributes(code.Value)
                        Exit Select
                    Case 1
                        value = code.Value
                        Exit Select
                    Case 8
                        'layer code
                        layer__1 = Me.GetLayer(code.Value)
                        Exit Select
                    Case 62
                        'aci color code
                        color = New AciColor(Short.Parse(code.Value))
                        Exit Select
                    Case 6
                        'type line code
                        lineType__2 = Me.GetLineType(code.Value)
                        Exit Select
                End Select
                code = Me.ReadCodePair()
            End While

            Return New Attribute(attdef) With { _
              .Color = color, _
              .Layer = layer__1, _
              .LineType = lineType__2, _
              .Value = value, _
              .Handle = handle _
            }
        End SyncLock
    End Function

    Private Sub ReadEntities()
        SyncLock _locker
            Dim code As CodeValuePair = Me.ReadCodePair()
            While code.Value <> StringCode.EndSection
                Dim entity As IEntityObject
                Select Case code.Value
                    Case DxfObjectCode.Arc
                        entity = Me.ReadArc(code)
                        Me.m_arcs.Add(DirectCast(entity, Arc))
                        Exit Select
                    Case DxfObjectCode.Circle
                        entity = Me.ReadCircle(code)
                        Me.m_circles.Add(DirectCast(entity, Circle))
                        Exit Select
                    Case DxfObjectCode.Point
                        entity = Me.ReadPoint(code)
                        Me.m_points.Add(DirectCast(entity, Point))
                        Exit Select
                    Case DxfObjectCode.Ellipse
                        entity = Me.ReadEllipse(code)
                        Me.m_ellipses.Add(DirectCast(entity, Ellipse))
                        Exit Select
                    Case DxfObjectCode.Face3D
                        entity = Me.ReadFace3D(code)
                        Me.m_faces3d.Add(DirectCast(entity, Face3d))
                        Exit Select
                    Case DxfObjectCode.Solid
                        entity = Me.ReadSolid(code)
                        Me.m_solids.Add(DirectCast(entity, Solid))
                        Exit Select
                    Case DxfObjectCode.Insert
                        entity = Me.ReadInsert(code)
                        Me.m_inserts.Add(DirectCast(entity, Insert))
                        Exit Select
                    Case DxfObjectCode.Line
                        entity = Me.ReadLine(code)
                        Me.m_lines.Add(DirectCast(entity, Line))
                        Exit Select
                    Case DxfObjectCode.LightWeightPolyline
                        entity = Me.ReadLightWeightPolyline(code)
                        Me.m_polylines.Add(DirectCast(entity, IPolyline))
                        Exit Select
                    Case DxfObjectCode.Polyline
                        entity = Me.ReadPolyline(code)
                        Me.m_polylines.Add(DirectCast(entity, IPolyline))
                        Exit Select
                    Case DxfObjectCode.Text
                        entity = Me.ReadText(code)
                        Me.m_texts.Add(DirectCast(entity, Text))
                        Exit Select
                    Case Else
                        ReadUnknowEntity(code)
                        'code = this.ReadCodePair();
                        Exit Select
                End Select
            End While
        End SyncLock
    End Sub

    Private Sub ReadObjects()
        SyncLock _locker
            Dim code As CodeValuePair = Me.ReadCodePair()
            While code.Value <> StringCode.EndSection
                code = Me.ReadCodePair()
            End While
        End SyncLock
    End Sub


    Private Sub ReadLayers()
        SyncLock _locker
            Dim code As CodeValuePair = Me.ReadCodePair()
            While code.Value <> StringCode.EndTable
                If code.Value = StringCode.LayerTable Then
                    Debug.Assert(code.Code = 0)
                    Dim capa As Layer = Me.ReadLayer(code)
                    Me.m_layers.Add(capa.Name, capa)
                Else
                    code = Me.ReadCodePair()
                End If
            End While
        End SyncLock
    End Sub

    Private Function ReadLayer(ByRef code As CodeValuePair) As Layer
        SyncLock _locker
            Dim handle As String = String.Empty
            Dim name As String = String.Empty
            Dim isVisible As Boolean = True
            Dim color As AciColor = Nothing
            Dim lineType As LineType = Nothing

            code = Me.ReadCodePair()

            While code.Code <> 0
                Select Case code.Code
                    Case 5
                        handle = code.Value
                        Exit Select
                    Case 2
                        If String.IsNullOrEmpty(code.Value) Then
                            Throw New DxfInvalidCodeValueEntityException(code.Code, code.Value, Me.file, "Invalid value " & Convert.ToString(code.Value) & " in code " & Convert.ToString(code.Code) & " line " & Me.fileLine)
                        End If
                        name = code.Value
                        Exit Select
                    Case 62
                        Dim index As Short
                        If Short.TryParse(code.Value, index) Then
                            If index < 0 Then
                                isVisible = False
                                index = Math.Abs(index)
                            End If
                            If index > 256 Then
                                Throw New DxfInvalidCodeValueEntityException(code.Code, code.Value, Me.file, "Invalid value " & Convert.ToString(code.Value) & " in code " & Convert.ToString(code.Code) & " line " & Me.fileLine)
                            End If
                        Else
                            Throw New DxfInvalidCodeValueEntityException(code.Code, code.Value, Me.file, "Invalid value " & Convert.ToString(code.Value) & " in code " & Convert.ToString(code.Code) & " line " & Me.fileLine)
                        End If

                        color = New AciColor(Short.Parse(code.Value))
                        Exit Select
                    Case 6
                        If String.IsNullOrEmpty(code.Value) Then
                            Throw New DxfInvalidCodeValueEntityException(code.Code, code.Value, Me.file, "Invalid value " & Convert.ToString(code.Value) & " in code " & Convert.ToString(code.Code) & " line " & Me.fileLine)
                        End If
                        lineType = Me.GetLineType(code.Value)
                        Exit Select
                End Select


                code = Me.ReadCodePair()
            End While

            Return New Layer(name) With { _
              .Color = color, _
              .LineType = lineType, _
              .IsVisible = isVisible, _
              .Handle = handle _
            }
        End SyncLock
    End Function


    Private Sub ReadLineTypes()
        SyncLock _locker
            Dim code As CodeValuePair = Me.ReadCodePair()
            While code.Value <> StringCode.EndTable
                If code.Value = StringCode.LineTypeTable Then
                    Debug.Assert(code.Code = 0)
                    'el código 0 indica el inicio de una nueva capa
                    Dim tl As LineType = Me.ReadLineType(code)
                    Me.m_lineTypes.Add(tl.Name, tl)
                Else
                    code = Me.ReadCodePair()
                End If
            End While
        End SyncLock
    End Sub

    Private Function ReadLineType(ByRef code As CodeValuePair) As LineType
        SyncLock _locker
            Dim handle As String = String.Empty
            Dim name As String = String.Empty
            Dim description As String = String.Empty
            Dim segments = New List(Of Single)()

            code = Me.ReadCodePair()

            While code.Code <> 0
                Select Case code.Code
                    Case 5
                        handle = code.Value
                        Exit Select
                    Case 2
                        If String.IsNullOrEmpty(code.Value) Then
                            Throw New DxfInvalidCodeValueEntityException(code.Code, code.Value, Me.file, "Invalid value " & Convert.ToString(code.Value) & " in code " & Convert.ToString(code.Code) & " line " & Me.fileLine)
                        End If
                        name = code.Value
                        Exit Select
                    Case 3
                        'descripción del tipo de línea
                        description = code.Value
                        Exit Select
                    Case 73
                        'number of segments (not needed)
                        Exit Select
                    Case 40
                        'length of the line type segments (not needed)
                        Exit Select
                    Case 49
                        segments.Add(Single.Parse(code.Value))
                        Exit Select
                End Select
                code = Me.ReadCodePair()
            End While

            Return New LineType(name) With { _
              .Description = description, _
              .Segments = segments, _
              .Handle = handle _
            }
        End SyncLock
    End Function

    Private Sub ReadTextStyles()
        SyncLock _locker
            Dim code As CodeValuePair = Me.ReadCodePair()
            While code.Value <> StringCode.EndTable
                If code.Value = StringCode.TextStyleTable Then
                    Debug.Assert(code.Code = 0)
                    'el código 0 indica el inicio de una nueva capa
                    Dim style As TextStyle = Me.ReadTextStyle(code)
                    Me.m_textStyles.Add(style.Name, style)
                Else
                    code = Me.ReadCodePair()
                End If
            End While
        End SyncLock
    End Sub

    Private Function ReadTextStyle(ByRef code As CodeValuePair) As TextStyle
        SyncLock _locker
            Dim handle As String = String.Empty
            Dim name As String = String.Empty
            Dim font As String = String.Empty
            Dim isVertical As Boolean = False
            Dim isBackward As Boolean = False
            Dim isUpsideDown As Boolean = False
            Dim height As Single = 0.0F
            Dim widthFactor As Single = 0.0F
            Dim obliqueAngle As Single = 0.0F

            code = Me.ReadCodePair()

            'leer los datos mientras no encontramos el código 0 que indicaría el final de la capa
            While code.Code <> 0
                Select Case code.Code
                    Case 5
                        handle = code.Value
                        Exit Select
                    Case 2
                        If String.IsNullOrEmpty(code.Value) Then
                            Throw New DxfInvalidCodeValueEntityException(code.Code, code.Value, Me.file, "Invalid value " & Convert.ToString(code.Value) & " in code " & Convert.ToString(code.Code) & " line " & Me.fileLine)
                        End If
                        name = code.Value
                        Exit Select
                    Case 3
                        If String.IsNullOrEmpty(code.Value) Then
                            Throw New DxfInvalidCodeValueEntityException(code.Code, code.Value, Me.file, "Invalid value " & Convert.ToString(code.Value) & " in code " & Convert.ToString(code.Code) & " line " & Me.fileLine)
                        End If
                        font = code.Value
                        Exit Select

                    Case 70
                        If Integer.Parse(code.Value) = 4 Then
                            isVertical = True
                        End If
                        Exit Select
                    Case 71
                        'orientación texto (normal)
                        If Integer.Parse(code.Value) = 6 Then
                            isBackward = True
                            isUpsideDown = True
                        ElseIf Integer.Parse(code.Value) = 2 Then
                            isBackward = True
                        ElseIf Integer.Parse(code.Value) = 4 Then
                            isUpsideDown = True
                        End If
                        Exit Select
                    Case 40
                        height = Single.Parse(code.Value)
                        Exit Select
                    Case 41
                        widthFactor = Single.Parse(code.Value)
                        Exit Select
                    Case 42
                        'last text height used (not aplicable)
                        Exit Select
                    Case 50
                        obliqueAngle = (Single.Parse(code.Value))
                        Exit Select
                End Select
                code = Me.ReadCodePair()
            End While

            Return New TextStyle(name, font) With { _
             .Height = height, _
             .IsBackward = isBackward, _
             .IsUpsideDown = isUpsideDown, _
             .IsVertical = isVertical, _
             .ObliqueAngle = obliqueAngle, _
             .WidthFactor = widthFactor, _
             .Handle = handle _
            }
        End SyncLock
    End Function

    Private Function ReadArc(ByRef code As CodeValuePair) As Arc
        SyncLock _locker
            Dim arc = New Arc()
            Dim center As Vector3f = Vector3f.Zero
            Dim normal As Vector3f = Vector3f.UnitZ
            Dim xData As New Dictionary(Of ApplicationRegistry, XData)()
            code = Me.ReadCodePair()
            While code.Code <> 0
                Select Case code.Code
                    Case 5
                        arc.Handle = code.Value
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 8
                        'layer code
                        arc.Layer = Me.GetLayer(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 62
                        'aci color code
                        arc.Color = New AciColor(Short.Parse(code.Value))
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 6
                        'type line code
                        arc.LineType = Me.GetLineType(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 10
                        center.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 20
                        center.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 30
                        center.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 40
                        arc.Radius = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 50
                        arc.StartAngle = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 51
                        arc.EndAngle = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 39
                        arc.Thickness = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 210
                        normal.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 220
                        normal.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 230
                        normal.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 1001
                        Dim xDataItem As XData = Me.ReadXDataRecord(code.Value, code)
                        xData.Add(xDataItem.ApplicationRegistry, xDataItem)
                        Exit Select
                    Case Else
                        If code.Code >= 1000 AndAlso code.Code <= 1071 Then
                            Throw New DxfInvalidCodeValueEntityException(code.Code, code.Value, Me.file, "The extended data of an entity must start with the application registry code " & Me.fileLine)
                        End If
                        code = Me.ReadCodePair()
                        Exit Select
                End Select
            End While

            arc.XData = xData
            arc.Center = center
            arc.Normal = normal
            Return arc
        End SyncLock
    End Function

    Private Function ReadCircle(ByRef code As CodeValuePair) As Circle
        SyncLock _locker
            Dim circle = New Circle()
            Dim center As Vector3f = Vector3f.Zero
            Dim normal As Vector3f = Vector3f.UnitZ
            Dim xData As New Dictionary(Of ApplicationRegistry, XData)()

            code = Me.ReadCodePair()
            While code.Code <> 0
                Select Case code.Code
                    Case 5
                        circle.Handle = code.Value
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 8
                        'layer code
                        circle.Layer = Me.GetLayer(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 62
                        'aci color code
                        circle.Color = New AciColor(Short.Parse(code.Value))
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 6
                        'type line code
                        circle.LineType = Me.GetLineType(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 10
                        center.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 20
                        center.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 30
                        center.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 40
                        circle.Radius = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 39
                        circle.Thickness = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 210
                        normal.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 220
                        normal.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 230
                        normal.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 1001
                        Dim xDataItem As XData = Me.ReadXDataRecord(code.Value, code)
                        xData.Add(xDataItem.ApplicationRegistry, xDataItem)
                        Exit Select
                    Case Else
                        If code.Code >= 1000 AndAlso code.Code <= 1071 Then
                            Throw New DxfInvalidCodeValueEntityException(code.Code, code.Value, Me.file, "The extended data of an entity must start with the application registry code " & Me.fileLine)
                        End If
                        code = Me.ReadCodePair()
                        Exit Select
                End Select
            End While

            circle.XData = xData
            circle.Center = center
            circle.Normal = normal
            Return circle
        End SyncLock
    End Function

    Private Function ReadEllipse(ByRef code As CodeValuePair) As Ellipse
        SyncLock _locker
            Dim ellipse = New Ellipse()
            Dim center As Vector3f = Vector3f.Zero
            Dim axisPoint As Vector3f = Vector3f.Zero
            Dim normal As Vector3f = Vector3f.UnitZ
            Dim ratio As Single = 0
            Dim xData As New Dictionary(Of ApplicationRegistry, XData)()

            code = Me.ReadCodePair()
            While code.Code <> 0
                Select Case code.Code
                    Case 5
                        ellipse.Handle = code.Value
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 8
                        'layer code
                        ellipse.Layer = Me.GetLayer(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 62
                        'aci color code
                        ellipse.Color = New AciColor(Short.Parse(code.Value))
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 6
                        'type line code
                        ellipse.LineType = Me.GetLineType(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 10
                        center.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 20
                        center.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 30
                        center.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 11
                        axisPoint.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 21
                        axisPoint.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 31
                        axisPoint.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 40
                        ratio = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 41
                        ellipse.StartAngle = CSng(Double.Parse(code.Value) * MathHelper.RadToDeg)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 42
                        ellipse.EndAngle = CSng(Double.Parse(code.Value) * MathHelper.RadToDeg)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 210
                        normal.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 220
                        normal.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 230
                        normal.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 1001
                        Dim xDataItem As XData = Me.ReadXDataRecord(code.Value, code)
                        xData.Add(xDataItem.ApplicationRegistry, xDataItem)
                        Exit Select
                    Case Else
                        If code.Code >= 1000 AndAlso code.Code <= 1071 Then
                            Throw New DxfInvalidCodeValueEntityException(code.Code, code.Value, Me.file, "The extended data of an entity must start with the application registry code " & Me.fileLine)
                        End If
                        code = Me.ReadCodePair()
                        Exit Select
                End Select
            End While

            Dim ocsAxisPoint As Vector3d = MathHelper.Transform(CType(axisPoint, Vector3d), CType(normal, Vector3d), MathHelper.CoordinateSystem.World, MathHelper.CoordinateSystem.[Object])
            Dim rotation As Double = CSng(Vector2d.AngleBetween(Vector2d.UnitX, New Vector2d(ocsAxisPoint.X, ocsAxisPoint.Y)))

            ellipse.MajorAxis = 2 * axisPoint.Modulus()
            ellipse.MinorAxis = ellipse.MajorAxis * ratio
            ellipse.Rotation = CSng(rotation * MathHelper.RadToDeg)
            ellipse.Center = center
            ellipse.Normal = normal
            ellipse.XData = xData
            Return ellipse
        End SyncLock
    End Function

    Private Function ReadPoint(ByRef code As CodeValuePair) As Point
        SyncLock _locker
            Dim point = New Point()
            Dim location As Vector3f = Vector3f.Zero
            Dim normal As Vector3f = Vector3f.UnitZ
            Dim xData As New Dictionary(Of ApplicationRegistry, XData)()

            code = Me.ReadCodePair()
            While code.Code <> 0
                Select Case code.Code
                    Case 5
                        point.Handle = code.Value
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 8
                        'layer code
                        point.Layer = Me.GetLayer(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 62
                        'aci color code
                        point.Color = New AciColor(Short.Parse(code.Value))
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 6
                        'type line code
                        point.LineType = Me.GetLineType(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 10
                        location.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 20
                        location.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 30
                        location.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 39
                        point.Thickness = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 210
                        normal.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 220
                        normal.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 230
                        normal.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 1001
                        Dim xDataItem As XData = Me.ReadXDataRecord(code.Value, code)
                        xData.Add(xDataItem.ApplicationRegistry, xDataItem)
                        Exit Select
                    Case Else
                        If code.Code >= 1000 AndAlso code.Code <= 1071 Then
                            Throw New DxfInvalidCodeValueEntityException(code.Code, code.Value, Me.file, "The extended data of an entity must start with the application registry code " & Me.fileLine)
                        End If
                        code = Me.ReadCodePair()
                        Exit Select
                End Select
            End While

            point.XData = xData
            point.Location = location
            point.Normal = normal
            Return point
        End SyncLock
    End Function

    Private Function ReadFace3D(ByRef code As CodeValuePair) As Face3d
        SyncLock _locker
            Dim face = New Face3d()
            Dim v0 As Vector3f = Vector3f.Zero
            Dim v1 As Vector3f = Vector3f.Zero
            Dim v2 As Vector3f = Vector3f.Zero
            Dim v3 As Vector3f = Vector3f.Zero
            Dim xData As New Dictionary(Of ApplicationRegistry, XData)()

            code = Me.ReadCodePair()
            While code.Code <> 0
                Select Case code.Code
                    Case 5
                        face.Handle = code.Value
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 8
                        'layer code
                        face.Layer = Me.GetLayer(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 62
                        'aci color code
                        face.Color = New AciColor(Short.Parse(code.Value))
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 6
                        'type line code
                        face.LineType = Me.GetLineType(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 10
                        v0.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 20
                        v0.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 30
                        v0.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 11
                        v1.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 21
                        v1.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 31
                        v1.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 12
                        v2.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 22
                        v2.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 32
                        v2.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 13
                        v3.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 23
                        v3.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 33
                        v3.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 70
                        face.EdgeFlags = DirectCast(Integer.Parse(code.Value), EdgeFlags)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 1001
                        Dim xDataItem As XData = Me.ReadXDataRecord(code.Value, code)
                        xData.Add(xDataItem.ApplicationRegistry, xDataItem)
                        Exit Select
                    Case Else
                        If code.Code >= 1000 AndAlso code.Code <= 1071 Then
                            Throw New DxfInvalidCodeValueEntityException(code.Code, code.Value, Me.file, "The extended data of an entity must start with the application registry code " & Me.fileLine)
                        End If

                        code = Me.ReadCodePair()
                        Exit Select
                End Select
            End While

            face.FirstVertex = v0
            face.SecondVertex = v1
            face.ThirdVertex = v2
            face.FourthVertex = v3
            face.XData = xData
            Return face
        End SyncLock
    End Function

    Private Function ReadSolid(ByRef code As CodeValuePair) As Solid
        SyncLock _locker
            Dim solid = New Solid()
            Dim v0 As Vector3f = Vector3f.Zero
            Dim v1 As Vector3f = Vector3f.Zero
            Dim v2 As Vector3f = Vector3f.Zero
            Dim v3 As Vector3f = Vector3f.Zero
            Dim normal As Vector3f = Vector3f.UnitZ
            Dim xData As New Dictionary(Of ApplicationRegistry, XData)()

            code = Me.ReadCodePair()
            While code.Code <> 0
                Select Case code.Code
                    Case 5
                        solid.Handle = code.Value
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 8
                        'layer code
                        solid.Layer = Me.GetLayer(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 62
                        'aci color code
                        solid.Color = New AciColor(Short.Parse(code.Value))
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 6
                        'type line code
                        solid.LineType = Me.GetLineType(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 10
                        v0.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 20
                        v0.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 30
                        v0.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 11
                        v1.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 21
                        v1.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 31
                        v1.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 12
                        v2.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 22
                        v2.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 32
                        v2.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 13
                        v3.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 23
                        v3.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 33
                        v3.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 70
                        solid.Thickness = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 210
                        normal.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 220
                        normal.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 230
                        normal.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 1001
                        Dim xDataItem As XData = Me.ReadXDataRecord(code.Value, code)
                        xData.Add(xDataItem.ApplicationRegistry, xDataItem)
                        Exit Select
                    Case Else
                        If code.Code >= 1000 AndAlso code.Code <= 1071 Then
                            Throw New DxfInvalidCodeValueEntityException(code.Code, code.Value, Me.file, "The extended data of an entity must start with the application registry code " & Me.fileLine)
                        End If

                        code = Me.ReadCodePair()
                        Exit Select
                End Select
            End While

            solid.FirstVertex = v0
            solid.SecondVertex = v1
            solid.ThirdVertex = v2
            solid.FourthVertex = v3
            solid.Normal = normal
            solid.XData = xData
            Return solid
        End SyncLock
    End Function

    Private Function ReadInsert(ByRef code As CodeValuePair) As Insert
        SyncLock _locker
            Dim handle As String = String.Empty
            Dim basePoint As Vector3f = Vector3f.Zero
            Dim normal As Vector3f = Vector3f.UnitZ
            Dim scale As New Vector3f(1, 1, 1)
            Dim rotation As Single = 0.0F
            Dim block As Block = Nothing
            Dim layer__1 As Layer = Layer.[Default]
            Dim color As AciColor = AciColor.ByLayer
            Dim lineType__2 As LineType = LineType.ByLayer
            Dim attributes As New List(Of Attribute)()
            Dim xData As New Dictionary(Of ApplicationRegistry, XData)()

            code = Me.ReadCodePair()
            While code.Code <> 0
                Select Case code.Code
                    Case 5
                        handle = code.Value
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 2
                        block = Me.GetBlock(code.Value)
                        If block Is Nothing Then
                            Throw New DxfEntityException(DxfObjectCode.Insert, Me.file, "Block " & Convert.ToString(code.Value) & " not defined line " & Me.fileLine)
                        End If
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 8
                        'layer code
                        layer__1 = Me.GetLayer(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 62
                        'aci color code
                        color = New AciColor(Short.Parse(code.Value))
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 6
                        'type line code
                        lineType__2 = Me.GetLineType(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 10
                        basePoint.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 20
                        basePoint.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 30
                        basePoint.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 41
                        scale.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 42
                        scale.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 43
                        scale.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 50
                        rotation = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 210
                        normal.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 220
                        normal.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 230
                        normal.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 1001
                        Dim xDataItem As XData = Me.ReadXDataRecord(code.Value, code)
                        xData.Add(xDataItem.ApplicationRegistry, xDataItem)
                        Exit Select
                    Case Else
                        If code.Code >= 1000 AndAlso code.Code <= 1071 Then
                            Throw New DxfInvalidCodeValueEntityException(code.Code, code.Value, Me.file, "The extended data of an entity must start with the application registry code " & Me.fileLine)
                        End If

                        code = Me.ReadCodePair()
                        Exit Select
                End Select
            End While

            ' if there are attributes
            Dim endSequenceHandle As String = String.Empty
            Dim endSequenceLayer As Layer = Layer.[Default]
            If code.Value = DxfObjectCode.Attribute Then
                While code.Value <> StringCode.EndSequence
                    If code.Value = DxfObjectCode.Attribute Then
                        Debug.Assert(code.Code = 0)
                        Dim attribute As Attribute = Me.ReadAttribute(block, code)
                        attributes.Add(attribute)
                    End If
                End While
                ' read the end end sequence object until a new element is found
                code = Me.ReadCodePair()
                While code.Code <> 0
                    Select Case code.Code
                        Case 5
                            endSequenceHandle = code.Value
                            code = Me.ReadCodePair()
                            Exit Select
                        Case 8
                            endSequenceLayer = Me.GetLayer(code.Value)
                            code = Me.ReadCodePair()
                            Exit Select
                        Case Else
                            code = Me.ReadCodePair()
                            Exit Select
                    End Select
                End While
            End If

            Dim insert As New Insert(block) With { _
             .Color = color, _
             .Layer = layer__1, _
             .LineType = lineType__2, _
             .InsertionPoint = basePoint, _
             .Rotation = rotation, _
             .Scale = scale, _
             .Normal = normal, _
             .Handle = handle _
            }

            insert.EndSequence.Handle = endSequenceHandle
            insert.EndSequence.Layer = endSequenceLayer
            insert.Attributes.Clear()
            insert.Attributes.AddRange(attributes)
            insert.XData = xData

            Return insert
        End SyncLock
    End Function

    Private Function ReadLine(ByRef code As CodeValuePair) As Line
        SyncLock _locker
            Dim line = New Line()
            Dim start As Vector3f = Vector3f.Zero
            Dim [end] As Vector3f = Vector3f.Zero
            Dim normal As Vector3f = Vector3f.UnitZ
            Dim xData As New Dictionary(Of ApplicationRegistry, XData)()

            code = Me.ReadCodePair()
            While code.Code <> 0
                Select Case code.Code
                    Case 5
                        line.Handle = code.Value
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 8
                        'layer code
                        line.Layer = Me.GetLayer(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 62
                        'aci color code
                        line.Color = New AciColor(Short.Parse(code.Value))
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 6
                        'type line code
                        line.LineType = Me.GetLineType(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 10
                        start.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 20
                        start.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 30
                        start.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 11
                        [end].X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 21
                        [end].Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 31
                        [end].Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 39
                        line.Thickness = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 210
                        normal.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 220
                        normal.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 230
                        normal.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 1001
                        Dim xDataItem As XData = Me.ReadXDataRecord(code.Value, code)
                        xData.Add(xDataItem.ApplicationRegistry, xDataItem)
                        Exit Select
                    Case Else
                        If code.Code >= 1000 AndAlso code.Code <= 1071 Then
                            Throw New DxfInvalidCodeValueEntityException(code.Code, code.Value, Me.file, "The extended data of an entity must start with the application registry code " & Me.fileLine)
                        End If

                        code = Me.ReadCodePair()
                        Exit Select
                End Select
            End While

            line.StartPoint = start
            line.EndPoint = [end]
            line.Normal = normal
            line.XData = xData

            Return line
        End SyncLock
    End Function

    Private Function ReadLightWeightPolyline(ByRef code As CodeValuePair) As LightWeightPolyline
        SyncLock _locker
            Dim pol = New LightWeightPolyline()
            'int numVertexes;
            Dim constantWidth As Single = 0.0F
            Dim v As New LightWeightPolylineVertex()
            Dim vX As Single = 0.0F
            Dim normal As Vector3f = Vector3f.UnitZ
            Dim xData As New Dictionary(Of ApplicationRegistry, XData)()

            code = Me.ReadCodePair()

            While code.Code <> 0
                Select Case code.Code
                    Case 5
                        pol.Handle = code.Value
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 8
                        pol.Layer = Me.GetLayer(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 62
                        pol.Color = New AciColor(Short.Parse(code.Value))
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 6
                        pol.LineType = Me.GetLineType(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 38
                        pol.Elevation = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 39
                        pol.Thickness = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 43
                        constantWidth = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 70
                        If Integer.Parse(code.Value) = 0 Then
                            pol.IsClosed = False
                        ElseIf Integer.Parse(code.Value) = 1 Then
                            pol.IsClosed = True
                        End If
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 90
                        'numVertexes = int.Parse(code.Value);
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 10
                        v = New LightWeightPolylineVertex() With { _
                         .BeginThickness = constantWidth, _
                         .EndThickness = constantWidth _
                        }
                        vX = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 20
                        Dim vY As Single = Single.Parse(code.Value)
                        v.Location = New Vector2f(vX, vY)
                        pol.Vertexes.Add(v)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 40
                        v.BeginThickness = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 41
                        v.EndThickness = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 42
                        v.Bulge = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 210
                        normal.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 220
                        normal.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 230
                        normal.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 1001
                        Dim xDataItem As XData = Me.ReadXDataRecord(code.Value, code)
                        xData.Add(xDataItem.ApplicationRegistry, xDataItem)
                        Exit Select
                    Case Else
                        If code.Code >= 1000 AndAlso code.Code <= 1071 Then
                            Throw New DxfInvalidCodeValueEntityException(code.Code, code.Value, Me.file, "The extended data of an entity must start with the application registry code " & Me.fileLine)
                        End If

                        code = Me.ReadCodePair()
                        Exit Select
                End Select
            End While

            pol.Normal = normal
            pol.XData = xData

            Return pol
        End SyncLock
    End Function

    Private Function ReadPolyline(ByRef code As CodeValuePair) As IPolyline
        SyncLock _locker
            Dim handle As String = String.Empty
            Dim layer__1 As Layer = Layer.[Default]
            Dim color As AciColor = AciColor.ByLayer
            Dim lineType__2 As LineType = LineType.ByLayer
            Dim flags As PolylineTypeFlags = PolylineTypeFlags.OpenPolyline
            Dim elevation As Single = 0.0F
            Dim thickness As Single = 0.0F
            Dim normal As Vector3f = Vector3f.UnitZ
            Dim vertexes As New List(Of Vertex)()
            Dim xData As New Dictionary(Of ApplicationRegistry, XData)()
            'int numVertexes = -1;
            'int numFaces = -1;

            code = Me.ReadCodePair()

            While code.Code <> 0
                Select Case code.Code
                    Case 5
                        handle = code.Value
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 8
                        layer__1 = Me.GetLayer(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 62
                        color = New AciColor(Short.Parse(code.Value))
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 6
                        lineType__2 = Me.GetLineType(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 30
                        elevation = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 39
                        thickness = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 70
                        flags = DirectCast(Integer.Parse(code.Value), PolylineTypeFlags)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 71
                        'this field might not exist for polyface meshes, we cannot depend on it
                        'numVertexes = int.Parse(code.Value); code = this.ReadCodePair();
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 72
                        'this field might not exist for polyface meshes, we cannot depend on it
                        'numFaces  = int.Parse(code.Value);
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 210
                        normal.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 220
                        normal.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 230
                        normal.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 1001
                        Dim xDataItem As XData = Me.ReadXDataRecord(code.Value, code)
                        xData.Add(xDataItem.ApplicationRegistry, xDataItem)
                        Exit Select
                    Case Else
                        If code.Code >= 1000 AndAlso code.Code <= 1071 Then
                            Throw New DxfInvalidCodeValueEntityException(code.Code, code.Value, Me.file, "The extended data of an entity must start with the application registry code " & Me.fileLine)
                        End If

                        code = Me.ReadCodePair()
                        Exit Select
                End Select
            End While

            'begin to read the vertex list
            If code.Value <> DxfObjectCode.Vertex Then
                Throw New DxfEntityException(DxfObjectCode.Polyline, Me.file, "Vertex not found in line " & Me.fileLine)
            End If
            While code.Value <> StringCode.EndSequence
                If code.Value = DxfObjectCode.Vertex Then
                    Debug.Assert(code.Code = 0)
                    Dim vertex As Vertex = Me.ReadVertex(code)
                    vertexes.Add(vertex)
                End If
            End While

            ' read the end end sequence object until a new element is found
            If code.Value <> StringCode.EndSequence Then
                Throw New DxfEntityException(DxfObjectCode.Polyline, Me.file, "End sequence entity not found in line " & Me.fileLine)
            End If
            code = Me.ReadCodePair()
            Dim endSequenceHandle As String = String.Empty
            Dim endSequenceLayer As Layer = layer__1
            While code.Code <> 0
                Select Case code.Code
                    Case 5
                        endSequenceHandle = code.Value
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 8
                        endSequenceLayer = Me.GetLayer(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case Else
                        code = Me.ReadCodePair()
                        Exit Select
                End Select
            End While

            Dim pol As IPolyline
            Dim isClosed As Boolean = False

            If (flags And PolylineTypeFlags.ClosedPolylineOrClosedPolygonMeshInM) = PolylineTypeFlags.ClosedPolylineOrClosedPolygonMeshInM Then
                isClosed = True
            End If

            'to avoid possible error between the vertex type and the polyline type
            'the polyline type will decide which information to use from the read vertex
            If (flags And PolylineTypeFlags.Polyline3D) = PolylineTypeFlags.Polyline3D Then
                Dim polyline3dVertexes As New List(Of Polyline3dVertex)()
                For Each v As Vertex In vertexes
                    Dim vertex As New Polyline3dVertex() With { _
                     .Color = v.Color, _
                     .Layer = v.Layer, _
                     .LineType = v.LineType, _
                     .Location = v.Location, _
                     .Handle = v.Handle _
                    }
                    vertex.XData = v.XData
                    polyline3dVertexes.Add(vertex)
                Next

                '''/posible error avoidance, the polyline is marked as polyline3d code:(70,8) but the vertex is marked as PolylineVertex code:(70,0)
                'if (v.Type == EntityType.PolylineVertex)
                '{
                '    Polyline3dVertex polyline3dVertex = new Polyline3dVertex(((PolylineVertex)v).Location.X, ((PolylineVertex)v).Location.Y,0);
                '    polyline3dVertexes.Add(polyline3dVertex);
                '}
                'else
                '{
                '    polyline3dVertexes.Add((Polyline3dVertex)v);
                '}
                '}
                pol = New Polyline3d(polyline3dVertexes, isClosed) With { _
                 .Handle = handle _
                }
                DirectCast(pol, Polyline3d).EndSequence.Handle = endSequenceHandle
                DirectCast(pol, Polyline3d).EndSequence.Layer = endSequenceLayer
            ElseIf (flags And PolylineTypeFlags.PolyfaceMesh) = PolylineTypeFlags.PolyfaceMesh Then
                'the vertex list created contains vertex and face information
                Dim polyfaceVertexes As New List(Of PolyfaceMeshVertex)()
                Dim polyfaceFaces As New List(Of PolyfaceMeshFace)()
                For Each v As Vertex In vertexes
                    If (v.Flags And (VertexTypeFlags.PolyfaceMeshVertex Or VertexTypeFlags.Polygon3dMesh)) = (VertexTypeFlags.PolyfaceMeshVertex Or VertexTypeFlags.Polygon3dMesh) Then
                        Dim vertex As New PolyfaceMeshVertex() With { _
                         .Color = v.Color, _
                         .Layer = v.Layer, _
                         .LineType = v.LineType, _
                         .Location = v.Location, _
                         .Handle = v.Handle _
                        }
                        vertex.XData = xData
                        polyfaceVertexes.Add(vertex)
                    ElseIf (v.Flags And (VertexTypeFlags.PolyfaceMeshVertex)) = (VertexTypeFlags.PolyfaceMeshVertex) Then
                        Dim vertex As New PolyfaceMeshFace() With { _
                         .Color = v.Color, _
                         .Layer = v.Layer, _
                         .LineType = v.LineType, _
                         .VertexIndexes = v.VertexIndexes, _
                         .Handle = v.Handle _
                        }
                        vertex.XData = xData
                        polyfaceFaces.Add(vertex)

                        'if (v.Type == EntityType.PolyfaceMeshVertex)
                        '{
                        '    polyfaceVertexes.Add((PolyfaceMeshVertex) v);
                        '}
                        'else if (v.Type == EntityType.PolyfaceMeshFace)
                        '{
                        '    polyfaceFaces.Add((PolyfaceMeshFace) v);
                        '}
                        'else
                        '{
                        '    throw new EntityDxfException(v.Type.ToString(), this.file, "Error in vertex type.");
                        '}
                    End If
                Next
                pol = New PolyfaceMesh(polyfaceVertexes, polyfaceFaces) With { _
                 .Handle = handle _
                }
                DirectCast(pol, PolyfaceMesh).EndSequence.Handle = endSequenceHandle
                DirectCast(pol, PolyfaceMesh).EndSequence.Layer = endSequenceLayer
            Else
                Dim polylineVertexes As New List(Of PolylineVertex)()
                For Each v As Vertex In vertexes
                    Dim vertex As New PolylineVertex() With { _
                     .Location = New Vector2f(v.Location.X, v.Location.Y), _
                     .BeginThickness = v.BeginThickness, _
                     .Bulge = v.Bulge, _
                     .Color = v.Color, _
                     .EndThickness = v.EndThickness, _
                     .Layer = v.Layer, _
                     .LineType = v.LineType, _
                     .Handle = v.Handle _
                    }
                    vertex.XData = xData

                    '''/posible error avoidance, the polyline is marked as polyline code:(70,0) but the vertex is marked as Polyline3dVertex code:(70,32)
                    'if (v.Type==EntityType.Polyline3dVertex)
                    '{
                    '    PolylineVertex polylineVertex = new PolylineVertex(((Polyline3dVertex)v).Location.X, ((Polyline3dVertex)v).Location.Y);
                    '    polylineVertexes.Add(polylineVertex);
                    '}
                    'else
                    '{
                    '    polylineVertexes.Add((PolylineVertex) v);
                    '}
                    polylineVertexes.Add(vertex)
                Next

                pol = New Polyline(polylineVertexes, isClosed) With { _
                 .Thickness = thickness, _
                 .Elevation = elevation, _
                 .Normal = normal, _
                 .Handle = handle _
                }
                DirectCast(pol, Polyline).EndSequence.Handle = endSequenceHandle
                DirectCast(pol, Polyline).EndSequence.Layer = endSequenceLayer
            End If

            pol.Color = color
            pol.Layer = layer__1
            pol.LineType = lineType__2
            pol.XData = xData

            Return pol
        End SyncLock
    End Function

    Private Function ReadText(ByRef code As CodeValuePair) As Text
        SyncLock _locker
            Dim text = New Text()

            Dim firstAlignmentPoint As Vector3f = Vector3f.Zero
            Dim secondAlignmentPoint As Vector3f = Vector3f.Zero
            Dim normal As Vector3f = Vector3f.UnitZ
            Dim horizontalAlignment As Integer = 0
            Dim verticalAlignment As Integer = 0
            Dim xData As New Dictionary(Of ApplicationRegistry, XData)()

            code = Me.ReadCodePair()
            While code.Code <> 0
                Select Case code.Code
                    Case 5
                        text.Handle = code.Value
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 1
                        text.Value = code.Value
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 8
                        'layer code
                        text.Layer = Me.GetLayer(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 62
                        'aci color code
                        text.Color = New AciColor(Short.Parse(code.Value))
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 6
                        'type line code
                        text.LineType = Me.GetLineType(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 10
                        firstAlignmentPoint.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 20
                        firstAlignmentPoint.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 30
                        firstAlignmentPoint.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 11
                        secondAlignmentPoint.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 21
                        secondAlignmentPoint.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 31
                        secondAlignmentPoint.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 40
                        text.Height = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 41
                        text.WidthFactor = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 50
                        text.Rotation = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 51
                        text.ObliqueAngle = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 7
                        text.Style = Me.GetTextStyle(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 72
                        horizontalAlignment = Integer.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 73
                        verticalAlignment = Integer.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 210
                        normal.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 220
                        normal.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 230
                        normal.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 1001
                        Dim xDataItem As XData = Me.ReadXDataRecord(code.Value, code)
                        xData.Add(xDataItem.ApplicationRegistry, xDataItem)
                        Exit Select
                    Case Else
                        If code.Code >= 1000 AndAlso code.Code <= 1071 Then
                            Throw New DxfInvalidCodeValueEntityException(code.Code, code.Value, Me.file, "The extended data of an entity must start with the application registry code " & Me.fileLine)
                        End If

                        code = Me.ReadCodePair()
                        Exit Select
                End Select
            End While

            Dim alignment As TextAlignment = ObtainAlignment(horizontalAlignment, verticalAlignment)

            text.BasePoint = If(alignment = TextAlignment.BaselineLeft, firstAlignmentPoint, secondAlignmentPoint)
            text.Normal = normal
            text.Alignment = alignment
            text.XData = xData

            Return text
        End SyncLock
    End Function

    Private Function ReadVertex(ByRef code As CodeValuePair) As Vertex
        SyncLock _locker
            Dim handle As String = String.Empty
            Dim layer__1 As Layer = Layer.[Default]
            Dim color As AciColor = AciColor.ByLayer
            Dim lineType__2 As LineType = LineType.ByLayer
            Dim location As New Vector3f()
            Dim xData As New Dictionary(Of ApplicationRegistry, XData)()
            Dim endThickness As Single = 0.0F
            Dim beginThickness As Single = 0.0F
            Dim bulge As Single = 0.0F
            Dim vertexIndexes As New List(Of Integer)()
            Dim flags As VertexTypeFlags = VertexTypeFlags.PolylineVertex

            code = Me.ReadCodePair()

            While code.Code <> 0
                Select Case code.Code
                    Case 5
                        handle = code.Value
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 8
                        layer__1 = Me.GetLayer(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 62
                        color = New AciColor(Short.Parse(code.Value))
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 6
                        lineType__2 = Me.GetLineType(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 10
                        location.X = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 20
                        location.Y = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 30
                        location.Z = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 40
                        beginThickness = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 41
                        endThickness = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 42
                        bulge = Single.Parse(code.Value)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 70
                        flags = DirectCast(Integer.Parse(code.Value), VertexTypeFlags)
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 71
                        vertexIndexes.Add(Integer.Parse(code.Value))
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 72
                        vertexIndexes.Add(Integer.Parse(code.Value))
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 73
                        vertexIndexes.Add(Integer.Parse(code.Value))
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 74
                        vertexIndexes.Add(Integer.Parse(code.Value))
                        code = Me.ReadCodePair()
                        Exit Select
                    Case 1001
                        Dim xDataItem As XData = Me.ReadXDataRecord(code.Value, code)
                        xData.Add(xDataItem.ApplicationRegistry, xDataItem)
                        Exit Select
                    Case Else
                        If code.Code >= 1000 AndAlso code.Code <= 1071 Then
                            Throw New DxfInvalidCodeValueEntityException(code.Code, code.Value, Me.file, "The extended data of an entity must start with the application registry code " & Me.fileLine)
                        End If

                        code = Me.ReadCodePair()
                        Exit Select
                End Select
            End While

            Return New Vertex() With { _
             .Flags = flags, _
             .Location = location, _
             .BeginThickness = beginThickness, _
             .Bulge = bulge, _
             .Color = color, _
             .EndThickness = endThickness, _
             .Layer = layer__1, _
             .LineType = lineType__2, _
             .VertexIndexes = vertexIndexes.ToArray(), _
             .XData = xData, _
             .Handle = handle _
            }

            'IVertex vertex;
            'if ((flags & (VertexTypeFlags.PolyfaceMeshVertex | VertexTypeFlags.Polygon3dMesh)) == (VertexTypeFlags.PolyfaceMeshVertex | VertexTypeFlags.Polygon3dMesh))
            '{
            '    vertex = new PolyfaceMeshVertex
            '                 {
            '                     Location=location
            '                 };
            '    vertex.XData=xData;
            '}
            'else if ((flags & (VertexTypeFlags.PolyfaceMeshVertex)) == (VertexTypeFlags.PolyfaceMeshVertex))
            '{
            '    vertex = new PolyfaceMeshFace(vertexIndexes.ToArray());
            '}
            'else if ((flags & (VertexTypeFlags.Polyline3dVertex)) == (VertexTypeFlags.Polyline3dVertex))
            '{
            '    vertex = new Polyline3dVertex
            '    {
            '        Location = location,
            '    };

            '    vertex.XData=xData;
            '}
            'else
            '{
            '    vertex = new PolylineVertex
            '                 {
            '                     Location =new Vector2f(location.X, location.Y),
            '                     Bulge = bulge,
            '                     BeginThickness = beginThickness,
            '                     EndThickness = endThickness
            '                 };

            '    vertex.XData=xData;
            '}

            'return vertex;
        End SyncLock
    End Function

    Private Sub ReadUnknowEntity(ByRef code As CodeValuePair)
        SyncLock _locker
            code = Me.ReadCodePair()
            While code.Code <> 0
                code = Me.ReadCodePair()
            End While
        End SyncLock
    End Sub


    Private Shared Function ObtainAlignment(ByVal horizontal As Integer, ByVal vertical As Integer) As TextAlignment
        SyncLock _locker

            Dim alignment As TextAlignment = TextAlignment.BaselineLeft

            If horizontal = 0 AndAlso vertical = 3 Then
                alignment = TextAlignment.TopLeft

            ElseIf horizontal = 1 AndAlso vertical = 3 Then
                alignment = TextAlignment.TopCenter

            ElseIf horizontal = 2 AndAlso vertical = 3 Then
                alignment = TextAlignment.TopRight

            ElseIf horizontal = 0 AndAlso vertical = 2 Then
                alignment = TextAlignment.MiddleLeft

            ElseIf horizontal = 1 AndAlso vertical = 2 Then
                alignment = TextAlignment.MiddleCenter

            ElseIf horizontal = 2 AndAlso vertical = 2 Then
                alignment = TextAlignment.MiddleRight

            ElseIf horizontal = 0 AndAlso vertical = 1 Then
                alignment = TextAlignment.BottomLeft

            ElseIf horizontal = 1 AndAlso vertical = 1 Then
                alignment = TextAlignment.BottomCenter

            ElseIf horizontal = 2 AndAlso vertical = 1 Then
                alignment = TextAlignment.BottomRight

            ElseIf horizontal = 0 AndAlso vertical = 0 Then
                alignment = TextAlignment.BaselineLeft
            End If

            If horizontal = 1 AndAlso vertical = 0 Then
                alignment = TextAlignment.BaselineCenter

            ElseIf horizontal = 2 AndAlso vertical = 0 Then
                alignment = TextAlignment.BaselineRight
            End If

            Return alignment
        End SyncLock
    End Function

    Private Function GetBlock(ByVal name As String) As Block
        SyncLock _locker
            If Me.m_blocks.ContainsKey(name) Then
                Return Me.m_blocks(name)
            End If
            Return Nothing
        End SyncLock
    End Function

    Private Function GetLayer(ByVal name As String) As Layer
        SyncLock _locker
            If Me.m_layers.ContainsKey(name) Then
                Return Me.m_layers(name)
            End If

            'just in case the layer has not been defined in the table section
            Dim layer = New Layer(name)
            Me.m_layers.Add(layer.Name, layer)
            Return layer
        End SyncLock
    End Function

    Private Function GetLineType(ByVal name As String) As LineType
        SyncLock _locker
            If Me.m_lineTypes.ContainsKey(name) Then
                Return Me.m_lineTypes(name)
            End If

            'just in case the line type has not been defined in the table section
            Dim lineType = New LineType(name)
            Me.m_lineTypes.Add(lineType.Name, lineType)
            Return lineType
        End SyncLock
    End Function

    Private Function GetTextStyle(ByVal name As String) As TextStyle
        SyncLock _locker
            If Me.m_textStyles.ContainsKey(name) Then
                Return Me.m_textStyles(name)
            End If

            'just in case the text style has not been defined in the table section
            Dim textStyle = New TextStyle(name, "Arial")
            Me.m_textStyles.Add(textStyle.Name, textStyle)
            Return textStyle
        End SyncLock
    End Function

    Private Function ReadCodePair() As CodeValuePair
        SyncLock _locker
            Dim code As Integer
            Dim readCode As String = Me.reader.ReadLine()
            Me.fileLine += 1
            If Not Integer.TryParse(readCode, code) Then
                Throw (New DxfException("Invalid group code " & readCode & " in line " & Me.fileLine))
            End If
            Dim value As String = Me.reader.ReadLine()
            Me.fileLine += 1
            Return New CodeValuePair(code, value)
        End SyncLock
    End Function

    Private Function ReadXDataRecord(ByVal appId As String, ByRef code As CodeValuePair) As XData
        SyncLock _locker
            Dim xData As New XData(Me.appIds(appId))
            code = Me.ReadCodePair()

            While code.Code >= 1000 AndAlso code.Code <= 1071
                If code.Code = XDataCode.AppReg Then
                    Exit While
                End If

                Dim xDataRecord As New XDataRecord(code.Code, code.Value)
                xData.XDataRecord.Add(xDataRecord)
                code = Me.ReadCodePair()
            End While

            Return xData
        End SyncLock
    End Function

End Class 