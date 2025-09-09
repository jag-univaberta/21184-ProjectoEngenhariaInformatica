
''' <summary>
''' Defines the extended data code.
''' </summary>
Public NotInheritable Class XDataCode
    ''' <summary>
    ''' Strings in extended data can be up to 255 bytes long.
    ''' </summary>
    Public Const [String] As Integer = 1000

    ''' <summary>
    ''' Application names can be up to 31 bytes long.
    ''' </summary>
    Friend Const AppReg As Integer = 1001

    ''' <summary>
    ''' An extended data control string can be either �{�or �}�.
    ''' These braces enable applications to organize their data by subdividing the data into lists.
    ''' The left brace begins a list, and the right brace terminates the most recent list. Lists can be nested
    ''' </summary>
    Friend Const ControlString As Integer = 1002

    ''' <summary>
    ''' Name of the layer associated with the extended data
    ''' </summary>
    Public Const LayerName As Integer = 1003

    ''' <summary>
    ''' Binary data is organized into variable-length chunks.
    ''' The maximum length of each chunk is 127 bytes.
    ''' In ASCII DXF files, binary data is represented as a string of hexadecimal digits, two per binary byte
    ''' </summary>
    Public Const BinaryData As Integer = 1004

    ''' <summary>
    ''' Handles of entities in the drawing database
    ''' </summary>
    Public Const DatabaseHandle As Integer = 1005

    ''' <summary>
    ''' Three real values, in the order X, Y, Z.
    ''' They can be used as a point or vector record. AutoCAD never alters their value
    ''' </summary>
    Public Const RealX As Integer = 1010

    ''' <summary>
    ''' Three real values, in the order X, Y, Z.
    ''' They can be used as a point or vector record. AutoCAD never alters their value
    ''' </summary>
    Public Const RealY As Integer = 1020

    ''' <summary>
    ''' Three real values, in the order X, Y, Z.
    ''' They can be used as a point or vector record. AutoCAD never alters their value
    ''' </summary>
    Public Const RealZ As Integer = 1030

    ''' <summary>
    ''' Unlike a simple 3D point, the world space coordinates are moved, scaled, rotated, and mirrored 
    ''' along with the parent entity to which the extended data belongs. 
    ''' The world space position is also stretched when the STRETCH command is applied to the parent entity and
    ''' this point lies within the select window
    ''' </summary>
    Public Const WorldSpacePositionX As Integer = 1011

    ''' <summary>
    ''' Unlike a simple 3D point, the world space coordinates are moved, scaled, rotated, and mirrored 
    ''' along with the parent entity to which the extended data belongs. 
    ''' The world space position is also stretched when the STRETCH command is applied to the parent entity and
    ''' this point lies within the select window
    ''' </summary>
    Public Const WorldSpacePositionY As Integer = 1021

    ''' <summary>
    ''' Unlike a simple 3D point, the world space coordinates are moved, scaled, rotated, and mirrored 
    ''' along with the parent entity to which the extended data belongs. 
    ''' The world space position is also stretched when the STRETCH command is applied to the parent entity and
    ''' this point lies within the select window
    ''' </summary>
    Public Const WorldSpacePositionZ As Integer = 1031

    ''' <summary>
    ''' Also a 3D point that is scaled, rotated, and mirrored along with the parent (but is not moved or stretched)
    ''' </summary>
    Public Const WorldSpaceDisplacementX As Integer = 1012

    ''' <summary>
    ''' Also a 3D point that is scaled, rotated, and mirrored along with the parent (but is not moved or stretched)
    ''' </summary>
    Public Const WorldSpaceDisplacementY As Integer = 1022

    ''' <summary>
    ''' Also a 3D point that is scaled, rotated, and mirrored along with the parent (but is not moved or stretched)
    ''' </summary>
    Public Const WorldSpaceDisplacementZ As Integer = 1032

    ''' <summary>
    ''' Also a 3D point that is rotated and mirrored along with the parent (but is not moved, scaled, or stretched)
    ''' </summary>
    Public Const WorldDirectionX As Integer = 1013

    ''' <summary>
    ''' Also a 3D point that is rotated and mirrored along with the parent (but is not moved, scaled, or stretched)
    ''' </summary>
    Public Const WorldDirectionY As Integer = 1023

    ''' <summary>
    ''' Also a 3D point that is rotated and mirrored along with the parent (but is not moved, scaled, or stretched)
    ''' </summary>
    Public Const WorldDirectionZ As Integer = 1033

    ''' <summary>
    ''' A real value.
    ''' </summary>
    Public Const Real As Integer = 1040

    ''' <summary>
    ''' A real value that is scaled along with the parent entity
    ''' </summary>
    Public Const Distance As Integer = 1041

    ''' <summary>
    ''' Also a real value that is scaled along with the parent.
    ''' The difference between a distance and a scale factor is application-defined
    ''' </summary>
    Public Const ScaleFactor As Integer = 1042

    ''' <summary>
    ''' A 16-bit integer (signed or unsigned)
    ''' </summary>
    Public Const [Integer] As Integer = 1070

    ''' <summary>
    ''' A 32-bit signed (long) integer
    ''' </summary>
    Public Const [Long] As Integer = 1071
End Class 