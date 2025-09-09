

Imports System.Collections.Generic
Imports System.Drawing


''' <summary>
''' Represents an indexed color.
''' </summary>
Public Class AciColor


    Private Shared ReadOnly aciColors As Dictionary(Of Byte, Byte()) = AciColorsF()
    Private m_index As Short 

    Private Shared ReadOnly _locker As New Object()

    ''' <summary>
    ''' Gets the ByLayer color.
    ''' </summary>
    Public Shared ReadOnly Property ByLayer() As AciColor
        Get
            SyncLock _locker
                Return New AciColor(256)
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Gets the ByBlock color.
    ''' </summary>
    Public Shared ReadOnly Property ByBlock() As AciColor
        Get
            SyncLock _locker
                Return New AciColor(0)
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Defines a default red color.
    ''' </summary>
    Public Shared ReadOnly Property Red() As AciColor
        Get
            SyncLock _locker
                Return New AciColor(1)
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Defines a default yellow color.
    ''' </summary>
    Public Shared ReadOnly Property Yellow() As AciColor
        Get
            SyncLock _locker
                Return New AciColor(2)
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Defines a default green color.
    ''' </summary>
    Public Shared ReadOnly Property Green() As AciColor
        Get
            SyncLock _locker
                Return New AciColor(3)
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Defines a default cyan color.
    ''' </summary>
    Public Shared ReadOnly Property Cyan() As AciColor
        Get
            SyncLock _locker
                Return New AciColor(4)
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Defines a default blue color.
    ''' </summary>
    Public Shared ReadOnly Property Blue() As AciColor
        Get
            SyncLock _locker
                Return New AciColor(5)
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Defines a default magenta color.
    ''' </summary>
    Public Shared ReadOnly Property Magenta() As AciColor
        Get
            SyncLock _locker
                Return New AciColor(6)
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Defines a default white/black color.
    ''' </summary>
    Public Shared ReadOnly Property [Default]() As AciColor
        Get
            SyncLock _locker
                Return New AciColor(7)
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Defines a default dark grey color.
    ''' </summary>
    Public Shared ReadOnly Property DarkGrey() As AciColor
        Get
            SyncLock _locker
                Return New AciColor(8)
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Defines a default light grey color.
    ''' </summary>
    Public Shared ReadOnly Property LightGrey() As AciColor
        Get
            SyncLock _locker
                Return New AciColor(9)
            End SyncLock
        End Get
    End Property
     
    ''' <summary>
    ''' Initializes a new instance of the <c>AciColor</c> class.
    ''' </summary>
    '''<param name="r">Red component.</param>
    '''<param name="g">Green component.</param>
    '''<param name="b">Blue component.</param>
    ''' <remarks>Since only 255 indexed colors are posible the conversion won't be exact.</remarks>
    Public Sub New(ByVal r As Byte, ByVal g As Byte, ByVal b As Byte)
        SyncLock _locker
            Me.m_index = RGBtoACI(r, g, b)
        End SyncLock
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the <c>AciColor</c> class.
    ''' </summary>
    '''<param name="r">Red component.</param>
    '''<param name="g">Green component.</param>
    '''<param name="b">Blue component.</param>
    ''' <remarks>Since only 255 indexed are posible the conversion won't be exact.</remarks>
    Public Sub New(ByVal r As Single, ByVal g As Single, ByVal b As Single)
        SyncLock _locker
            Me.m_index = RGBtoACI(CByte(Math.Truncate(r * 255)), CByte(Math.Truncate(g * 255)), CByte(Math.Truncate(b * 255)))
        End SyncLock
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the <c>AciColor</c> class.
    ''' </summary>
    '''<param name="color">A <see cref="Color">color</see>.</param>
    Public Sub New(ByVal color As Color)
        SyncLock _locker
            Me.m_index = RGBtoACI(color.R, color.G, color.B)
        End SyncLock
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the <c>AciColor</c> class.
    ''' </summary>
    ''' <param name="index">Color index.</param>
    ''' <remarks>
    ''' Accepted color index values range from 0 to 256.
    ''' Indexes from 1 to 255 represents a color, the index 256 is reserved to define a color bylayer and the index 0 represents byblock.
    ''' </remarks>
    Public Sub New(ByVal index As Short)
        SyncLock _locker
            If index < 0 OrElse index > 256 Then
                Throw (New ArgumentException("index"))
            End If
            Me.m_index = index
        End SyncLock
    End Sub
     

    ''' <summary>
    ''' Gets or sets the color index.
    ''' </summary>
    ''' <remarks>
    ''' Accepted color index values range from 0 to 256.
    ''' Indexes from 1 to 255 represents a color, the index 256 is reserved to define a color bylayer and the index 0 represents byblock.
    ''' </remarks>
    Public Property Index() As Short
        Get
            SyncLock _locker
                Return Me.m_index
            End SyncLock
        End Get
        Set(ByVal value As Short)
            SyncLock _locker
                If value < 0 OrElse value > 256 Then
                    Throw (New ArgumentException("value"))
                End If
                Me.m_index = value
            End SyncLock
        End Set
    End Property





    ''' <summary>
    ''' Converts the value of this instance to its equivalent string representation.
    ''' </summary>
    ''' <returns>The string representation.</returns>
    Public Overrides Function ToString() As String
        SyncLock _locker
            If Me.m_index = 0 Then
                Return "ByBlock"
            End If
            If Me.m_index = 256 Then
                Return "ByLayer"
            End If
            Return Me.m_index.ToString()
        End SyncLock
    End Function
    ''' <summary>
    ''' Converts the AciColor to a <see cref="Color">color</see>.
    ''' </summary>
    ''' <returns>A default color white will be used for byblock and bylayer colors.</returns>
    Public Function ToColor() As Color
        SyncLock _locker
            If Me.m_index < 1 OrElse Me.m_index > 255 Then
                'default color definition for byblock and bylayer colors
                Return Color.White
            End If

            Dim rgb As Byte() = aciColors(CByte(Me.m_index))
            Return Color.FromArgb(rgb(0), rgb(1), rgb(2))
        End SyncLock
    End Function

    ''' <summary>
    ''' Obtains a color index from the rgb components.
    ''' </summary>
    '''<param name="r">Red component.</param>
    '''<param name="g">Green component.</param>
    '''<param name="b">Blue component.</param>
    Private Shared Function RGBtoACI(ByVal r As Byte, ByVal g As Byte, ByVal b As Byte) As Byte
        SyncLock _locker
            Dim prevDist As Integer = Integer.MaxValue
            Dim index As Byte = 0
            For Each key As Byte In aciColors.Keys
                Dim color As Byte() = aciColors(key)
                Dim dist As Integer = Math.Abs((r - color(0)) * (r - color(0)) + (g - color(1)) * (g - color(1)) + (b - color(2)) * (b - color(2)))
                If dist < prevDist Then
                    prevDist = dist
                    index = key
                End If
            Next

            Return index
        End SyncLock
    End Function

    Private Shared Function AciColorsF() As Dictionary(Of Byte, Byte())

        Dim lista = New Dictionary(Of Byte, Byte())() From { _
         {1, New Byte() {255, 0, 0}}, _
         {2, New Byte() {255, 255, 0}}, _
         {3, New Byte() {0, 255, 0}}, _
         {4, New Byte() {0, 255, 255}}, _
         {5, New Byte() {0, 0, 255}}, _
         {6, New Byte() {255, 0, 255}}, _
         {7, New Byte() {255, 255, 255}}, _
         {8, New Byte() {128, 128, 128}}, _
         {9, New Byte() {192, 192, 192}}, _
         {10, New Byte() {255, 0, 0}}, _
         {11, New Byte() {255, 127, 127}}, _
         {12, New Byte() {204, 0, 0}}, _
         {13, New Byte() {204, 102, 102}}, _
         {14, New Byte() {153, 0, 0}}, _
         {15, New Byte() {153, 76, 76}}, _
         {16, New Byte() {127, 0, 0}}, _
         {17, New Byte() {127, 63, 63}}, _
         {18, New Byte() {76, 0, 0}}, _
         {19, New Byte() {76, 38, 38}}, _
         {20, New Byte() {255, 63, 0}}, _
         {21, New Byte() {255, 159, 127}}, _
         {22, New Byte() {204, 51, 0}}, _
         {23, New Byte() {204, 127, 102}}, _
         {24, New Byte() {153, 38, 0}}, _
         {25, New Byte() {153, 95, 76}}, _
         {26, New Byte() {127, 31, 0}}, _
         {27, New Byte() {127, 79, 63}}, _
         {28, New Byte() {76, 19, 0}}, _
         {29, New Byte() {76, 47, 38}}, _
         {30, New Byte() {255, 127, 0}}, _
         {31, New Byte() {255, 191, 127}}, _
         {32, New Byte() {204, 102, 0}}, _
         {33, New Byte() {204, 153, 102}}, _
         {34, New Byte() {153, 76, 0}}, _
         {35, New Byte() {153, 114, 76}}, _
         {36, New Byte() {127, 63, 0}}, _
         {37, New Byte() {127, 95, 63}}, _
         {38, New Byte() {76, 38, 0}}, _
         {39, New Byte() {76, 57, 38}}, _
         {40, New Byte() {255, 191, 0}}, _
         {41, New Byte() {255, 223, 127}}, _
         {42, New Byte() {204, 153, 0}}, _
         {43, New Byte() {204, 178, 102}}, _
         {44, New Byte() {153, 114, 0}}, _
         {45, New Byte() {153, 133, 76}}, _
         {46, New Byte() {127, 95, 0}}, _
         {47, New Byte() {127, 111, 63}}, _
         {48, New Byte() {76, 57, 0}}, _
         {49, New Byte() {76, 66, 38}}, _
         {50, New Byte() {255, 255, 0}}, _
         {51, New Byte() {255, 255, 127}}, _
         {52, New Byte() {204, 204, 0}}, _
         {53, New Byte() {204, 204, 102}}, _
         {54, New Byte() {153, 153, 0}}, _
         {55, New Byte() {153, 153, 76}}, _
         {56, New Byte() {127, 127, 0}}, _
         {57, New Byte() {127, 127, 63}}, _
         {58, New Byte() {76, 76, 0}}, _
         {59, New Byte() {76, 76, 38}}, _
         {60, New Byte() {191, 255, 0}}, _
         {61, New Byte() {223, 255, 127}}, _
         {62, New Byte() {153, 204, 0}}, _
         {63, New Byte() {178, 204, 102}}, _
         {64, New Byte() {114, 153, 0}}, _
         {65, New Byte() {133, 153, 76}}, _
         {66, New Byte() {95, 127, 0}}, _
         {67, New Byte() {111, 127, 63}}, _
         {68, New Byte() {57, 76, 0}}, _
         {69, New Byte() {66, 76, 38}}, _
         {70, New Byte() {127, 255, 0}}, _
         {71, New Byte() {191, 255, 127}}, _
         {72, New Byte() {102, 204, 0}}, _
         {73, New Byte() {153, 204, 102}}, _
         {74, New Byte() {76, 153, 0}}, _
         {75, New Byte() {114, 153, 76}}, _
         {76, New Byte() {63, 127, 0}}, _
         {77, New Byte() {95, 127, 63}}, _
         {78, New Byte() {38, 76, 0}}, _
         {79, New Byte() {57, 76, 38}}, _
         {80, New Byte() {63, 255, 0}}, _
         {81, New Byte() {159, 255, 127}}, _
         {82, New Byte() {51, 204, 0}}, _
         {83, New Byte() {127, 204, 102}}, _
         {84, New Byte() {38, 153, 0}}, _
         {85, New Byte() {95, 153, 76}}, _
         {86, New Byte() {31, 127, 0}}, _
         {87, New Byte() {79, 127, 63}}, _
         {88, New Byte() {19, 76, 0}}, _
         {89, New Byte() {47, 76, 38}}, _
         {90, New Byte() {0, 255, 0}}, _
         {91, New Byte() {127, 255, 127}}, _
         {92, New Byte() {0, 204, 0}}, _
         {93, New Byte() {102, 204, 102}}, _
         {94, New Byte() {0, 153, 0}}, _
         {95, New Byte() {76, 153, 76}}, _
         {96, New Byte() {0, 127, 0}}, _
         {97, New Byte() {63, 127, 63}}, _
         {98, New Byte() {0, 76, 0}}, _
         {99, New Byte() {38, 76, 38}}, _
         {100, New Byte() {0, 255, 63}}, _
         {101, New Byte() {127, 255, 159}}, _
         {102, New Byte() {0, 204, 51}}, _
         {103, New Byte() {102, 204, 127}}, _
         {104, New Byte() {0, 153, 38}}, _
         {105, New Byte() {76, 153, 95}}, _
         {106, New Byte() {0, 127, 31}}, _
         {107, New Byte() {63, 127, 79}}, _
         {108, New Byte() {0, 76, 19}}, _
         {109, New Byte() {38, 76, 47}}, _
         {110, New Byte() {0, 255, 127}}, _
         {111, New Byte() {127, 255, 191}}, _
         {112, New Byte() {0, 204, 102}}, _
         {113, New Byte() {102, 204, 153}}, _
         {114, New Byte() {0, 153, 76}}, _
         {115, New Byte() {76, 153, 114}}, _
         {116, New Byte() {0, 127, 63}}, _
         {117, New Byte() {63, 127, 95}}, _
         {118, New Byte() {0, 76, 38}}, _
         {119, New Byte() {38, 76, 57}}, _
         {120, New Byte() {0, 255, 191}}, _
         {121, New Byte() {127, 255, 223}}, _
         {122, New Byte() {0, 204, 153}}, _
         {123, New Byte() {102, 204, 178}}, _
         {124, New Byte() {0, 153, 114}}, _
         {125, New Byte() {76, 153, 133}}, _
         {126, New Byte() {0, 127, 95}}, _
         {127, New Byte() {63, 127, 111}}, _
         {128, New Byte() {0, 76, 57}}, _
         {129, New Byte() {38, 76, 66}}, _
         {130, New Byte() {0, 255, 255}}, _
         {131, New Byte() {127, 255, 255}}, _
         {132, New Byte() {0, 204, 204}}, _
         {133, New Byte() {102, 204, 204}}, _
         {134, New Byte() {0, 153, 153}}, _
         {135, New Byte() {76, 153, 153}}, _
         {136, New Byte() {0, 127, 127}}, _
         {137, New Byte() {63, 127, 127}}, _
         {138, New Byte() {0, 76, 76}}, _
         {139, New Byte() {38, 76, 76}}, _
         {140, New Byte() {0, 191, 255}}, _
         {141, New Byte() {127, 223, 255}}, _
         {142, New Byte() {0, 153, 204}}, _
         {143, New Byte() {102, 178, 204}}, _
         {144, New Byte() {0, 114, 153}}, _
         {145, New Byte() {76, 133, 153}}, _
         {146, New Byte() {0, 95, 127}}, _
         {147, New Byte() {63, 111, 127}}, _
         {148, New Byte() {0, 57, 76}}, _
         {149, New Byte() {38, 66, 76}}, _
         {150, New Byte() {0, 127, 255}}, _
         {151, New Byte() {127, 191, 255}}, _
         {152, New Byte() {0, 102, 204}}, _
         {153, New Byte() {102, 153, 204}}, _
         {154, New Byte() {0, 76, 153}}, _
         {155, New Byte() {76, 114, 153}}, _
         {156, New Byte() {0, 63, 127}}, _
         {157, New Byte() {63, 95, 127}}, _
         {158, New Byte() {0, 38, 76}}, _
         {159, New Byte() {38, 57, 76}}, _
         {160, New Byte() {0, 63, 255}}, _
         {161, New Byte() {127, 159, 255}}, _
         {162, New Byte() {0, 51, 204}}, _
         {163, New Byte() {102, 127, 204}}, _
         {164, New Byte() {0, 38, 153}}, _
         {165, New Byte() {76, 95, 153}}, _
         {166, New Byte() {0, 31, 127}}, _
         {167, New Byte() {63, 79, 127}}, _
         {168, New Byte() {0, 19, 76}}, _
         {169, New Byte() {38, 47, 76}}, _
         {170, New Byte() {0, 0, 255}}, _
         {171, New Byte() {127, 127, 255}}, _
         {172, New Byte() {0, 0, 204}}, _
         {173, New Byte() {102, 102, 204}}, _
         {174, New Byte() {0, 0, 153}}, _
         {175, New Byte() {76, 76, 153}}, _
         {176, New Byte() {0, 0, 127}}, _
         {177, New Byte() {63, 63, 127}}, _
         {178, New Byte() {0, 0, 76}}, _
         {179, New Byte() {38, 38, 76}}, _
         {180, New Byte() {63, 0, 255}}, _
         {181, New Byte() {159, 127, 255}}, _
         {182, New Byte() {51, 0, 204}}, _
         {183, New Byte() {127, 102, 204}}, _
         {184, New Byte() {38, 0, 153}}, _
         {185, New Byte() {95, 76, 153}}, _
         {186, New Byte() {31, 0, 127}}, _
         {187, New Byte() {79, 63, 127}}, _
         {188, New Byte() {19, 0, 76}}, _
         {189, New Byte() {47, 38, 76}}, _
         {190, New Byte() {127, 0, 255}}, _
         {191, New Byte() {191, 127, 255}}, _
         {192, New Byte() {102, 0, 204}}, _
         {193, New Byte() {153, 102, 204}}, _
         {194, New Byte() {76, 0, 153}}, _
         {195, New Byte() {114, 76, 153}}, _
         {196, New Byte() {63, 0, 127}}, _
         {197, New Byte() {95, 63, 127}}, _
         {198, New Byte() {38, 0, 76}}, _
         {199, New Byte() {57, 38, 76}}, _
         {200, New Byte() {191, 0, 255}}, _
         {201, New Byte() {223, 127, 255}}, _
         {202, New Byte() {153, 0, 204}}, _
         {203, New Byte() {178, 102, 204}}, _
         {204, New Byte() {114, 0, 153}}, _
         {205, New Byte() {133, 76, 153}}, _
         {206, New Byte() {95, 0, 127}}, _
         {207, New Byte() {111, 63, 127}}, _
         {208, New Byte() {57, 0, 76}}, _
         {209, New Byte() {66, 38, 76}}, _
         {210, New Byte() {255, 0, 255}}, _
         {211, New Byte() {255, 127, 255}}, _
         {212, New Byte() {204, 0, 204}}, _
         {213, New Byte() {204, 102, 204}}, _
         {214, New Byte() {153, 0, 153}}, _
         {215, New Byte() {153, 76, 153}}, _
         {216, New Byte() {127, 0, 127}}, _
         {217, New Byte() {127, 63, 127}}, _
         {218, New Byte() {76, 0, 76}}, _
         {219, New Byte() {76, 38, 76}}, _
         {220, New Byte() {255, 0, 191}}, _
         {221, New Byte() {255, 127, 223}}, _
         {222, New Byte() {204, 0, 153}}, _
         {223, New Byte() {204, 102, 178}}, _
         {224, New Byte() {153, 0, 114}}, _
         {225, New Byte() {153, 76, 133}}, _
         {226, New Byte() {127, 0, 95}}, _
         {227, New Byte() {127, 63, 111}}, _
         {228, New Byte() {76, 0, 57}}, _
         {229, New Byte() {76, 38, 66}}, _
         {230, New Byte() {255, 0, 127}}, _
         {231, New Byte() {255, 127, 191}}, _
         {232, New Byte() {204, 0, 102}}, _
         {233, New Byte() {204, 102, 153}}, _
         {234, New Byte() {153, 0, 76}}, _
         {235, New Byte() {153, 76, 114}}, _
         {236, New Byte() {127, 0, 63}}, _
         {237, New Byte() {127, 63, 95}}, _
         {238, New Byte() {76, 0, 38}}, _
         {239, New Byte() {76, 38, 57}}, _
         {240, New Byte() {255, 0, 63}}, _
         {241, New Byte() {255, 127, 159}}, _
         {242, New Byte() {204, 0, 51}}, _
         {243, New Byte() {204, 102, 127}}, _
         {244, New Byte() {153, 0, 38}}, _
         {245, New Byte() {153, 76, 95}}, _
         {246, New Byte() {127, 0, 31}}, _
         {247, New Byte() {127, 63, 79}}, _
         {248, New Byte() {76, 0, 19}}, _
         {249, New Byte() {76, 38, 47}}, _
         {250, New Byte() {51, 51, 51}}, _
         {251, New Byte() {91, 91, 91}}, _
         {252, New Byte() {132, 132, 132}}, _
         {253, New Byte() {173, 173, 173}}, _
         {254, New Byte() {214, 214, 214}}, _
         {255, New Byte() {255, 255, 255}} _
        }

        Return lista

    End Function


End Class

