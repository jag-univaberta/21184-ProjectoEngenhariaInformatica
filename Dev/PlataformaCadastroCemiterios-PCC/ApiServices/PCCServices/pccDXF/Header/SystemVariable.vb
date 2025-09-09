Namespace Header
    ''' <summary>
    ''' Strings system variables
    ''' </summary>
    Public NotInheritable Class SystemVariable
        Private Sub New()
        End Sub
        ''' <summary>
        ''' The AutoCAD drawing database version number.
        ''' </summary>
        Public Const DabaseVersion As String = "$ACADVER"

        ''' <summary>
        ''' Next available handle (this variable must be present in the header section)
        ''' </summary>
        Public Const HandSeed As String = "$HANDSEED"
    End Class
End Namespace
