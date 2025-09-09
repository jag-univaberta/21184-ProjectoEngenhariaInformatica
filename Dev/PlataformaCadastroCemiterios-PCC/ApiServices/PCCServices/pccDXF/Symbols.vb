
''' <summary>
''' Symbols for dxf text strings.
''' </summary>
''' <remarks>
''' These special strings translates to symbols in AutoCad. 
''' </remarks>
Public NotInheritable Class Symbols
    Private Sub New()
    End Sub
    ''' <summary>
    ''' Text string that shows as a diameter 'Ø' character.
    ''' </summary>
    Public Const Diameter As String = "%%c"

    ''' <summary>
    ''' Text string that shows as a degree '°' character.
    ''' </summary>
    Public Const Degree As String = "%%d"

    ''' <summary>
    ''' Text string that shows as a plus-minus '±' character.
    ''' </summary>
    Public Const PlusMinus As String = "%%p"
End Class

