
''' <summary>
''' Dxf sections.
''' </summary>
Friend NotInheritable Class StringCode
    Private Sub New()
    End Sub
    ''' <summary>
    ''' not defined.
    ''' </summary>
    Public Const Unknown As String = ""

    ''' <summary>
    ''' header.
    ''' </summary>
    Public Const HeaderSection As String = "HEADER"

    ''' <summary>
    ''' clases.
    ''' </summary>
    Public Const ClassesSection As String = "CLASSES"

    ''' <summary>
    ''' tables.
    ''' </summary>
    Public Const TablesSection As String = "TABLES"

    ''' <summary>
    ''' blocks.
    ''' </summary>
    Public Const BlocksSection As String = "BLOCKS"

    ''' <summary>
    ''' entities.
    ''' </summary>
    Public Const EntitiesSection As String = "ENTITIES"

    ''' <summary>
    ''' objects.
    ''' </summary>
    Public Const ObjectsSection As String = "OBJECTS"

    ''' <summary>
    ''' dxf name string.
    ''' </summary>
    Public Const BeginSection As String = "SECTION"

    ''' <summary>
    ''' end secction code.
    ''' </summary>
    Public Const EndSection As String = "ENDSEC"

    ''' <summary>
    ''' layers.
    ''' </summary>
    Public Const LayerTable As String = "LAYER"

    ''' <summary>
    ''' view ports.
    ''' </summary>
    Public Const ViewPortTable As String = "VPORT"

    ''' <summary>
    ''' views.
    ''' </summary>
    Public Const ViewTable As String = "VIEW"

    ''' <summary>
    ''' ucs.
    ''' </summary>
    Public Const UcsTable As String = "UCS"

    ''' <summary>
    ''' block records.
    ''' </summary>
    Public Const BlockRecordTable As String = "BLOCK_RECORD"

    ''' <summary>
    ''' line types.
    ''' </summary>
    Public Const LineTypeTable As String = "LTYPE"

    ''' <summary>
    ''' text styles.
    ''' </summary>
    Public Const TextStyleTable As String = "STYLE"

    ''' <summary>
    ''' dim styles.
    ''' </summary>
    Public Const DimensionStyleTable As String = "DIMSTYLE"

    ''' <summary>
    ''' extended data application registry.
    ''' </summary>
    Public Const ApplicationIDTable As String = "APPID"

    ''' <summary>
    ''' end table code.
    ''' </summary>
    Public Const EndTable As String = "ENDTAB"

    ''' <summary>
    ''' dxf name string.
    ''' </summary>
    Public Const Table As String = "TABLE"

    ''' <summary>
    ''' dxf name string.
    ''' </summary>
    Public Const BeginBlock As String = "BLOCK"

    ''' <summary>
    ''' end table code.
    ''' </summary>
    Public Const EndBlock As String = "ENDBLK"

    ''' <summary>
    ''' end of an element sequence
    ''' </summary>
    Public Const EndSequence As String = "SEQEND"

    ''' <summary>
    ''' dictionary
    ''' </summary>
    Public Const Dictionary As String = "DICTIONARY"

    ''' <summary>
    ''' end of file
    ''' </summary>
    Public Const EndOfFile As String = "EOF"
End Class

