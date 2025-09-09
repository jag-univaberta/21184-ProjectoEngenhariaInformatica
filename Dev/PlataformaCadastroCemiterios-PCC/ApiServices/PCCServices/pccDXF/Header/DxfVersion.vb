Namespace Header
    ''' <summary>
    ''' Defines de dxf file version.
    ''' </summary>
    Public Enum DxfVersion
        <StringValue("AC1009")> _
        AutoCad12
        '[StringValue("AC1012")] AutoCad13,
        '[StringValue("AC1014")] AutoCad14,
        <StringValue("AC1015")> _
        AutoCad2000
        <StringValue("AC1018")> _
        AutoCad2004
        <StringValue("AC1021")> _
        AutoCad2007
    End Enum
End Namespace
