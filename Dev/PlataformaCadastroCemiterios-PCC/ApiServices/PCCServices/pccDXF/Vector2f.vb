
''' <summary>
''' Represent a two component vector of single precision.
''' </summary>
Public Structure Vector2f


    Private m_x As Single
    Private m_y As Single
     
     
    ''' <summary>
    ''' Initializes a new instance of Vector2f.
    ''' </summary>
    ''' <param name="x">X component.</param>
    ''' <param name="y">Y component.</param>
    Public Sub New(ByVal x As Single, ByVal y As Single)

        Me.m_x = x
        Me.m_y = y

    End Sub

    ''' <summary>
    ''' Initializes a new instance of Vector2f.
    ''' </summary>
    ''' <param name="array">Array of two elements that represents the vector.</param>
    Public Sub New(ByVal array As Single())

        If array.Length <> 2 Then
            Throw New ArgumentOutOfRangeException("array", array.Length, "The dimension of the array must be two")
        End If
        Me.m_x = array(0)
        Me.m_y = array(1)

    End Sub

    ''' <summary>
    ''' Zero vector.
    ''' </summary>
    Public Shared ReadOnly Property Zero() As Vector2f
        Get

            Return New Vector2f(0, 0)

        End Get
    End Property

    ''' <summary>
    ''' Unit X vector.
    ''' </summary>
    Public Shared ReadOnly Property UnitX() As Vector2f
        Get

            Return New Vector2f(1, 0)

        End Get
    End Property

    ''' <summary>
    ''' Unit Y vector.
    ''' </summary>
    Public Shared ReadOnly Property UnitY() As Vector2f
        Get

            Return New Vector2f(0, 1)

        End Get
    End Property



    ''' <summary>
    ''' Gets or sets the X component.
    ''' </summary>
    Public Property X() As Single
        Get

            Return Me.m_x

        End Get
        Set(ByVal value As Single)

            Me.m_x = value

        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the Y component.
    ''' </summary>
    Public Property Y() As Single
        Get

            Return Me.m_y

        End Get
        Set(ByVal value As Single)

            Me.m_y = value

        End Set
    End Property

    ''' <summary>
    ''' Gets or sets a vector element defined by its index.
    ''' </summary>
    ''' <param name="index">Index of the element.</param>
    Default Public Property Item(ByVal index As Integer) As Single

        Get

            Select Case index
                Case 0
                    Return Me.m_x
                Case 1

                    Return Me.m_y
                Case Else

                    Throw (New ArgumentOutOfRangeException("index"))
            End Select

        End Get
        Set(ByVal value As Single)

            Select Case index
                Case 0
                    Me.m_x = value
                    Exit Select
                Case 1

                    Me.m_y = value
                    Exit Select
                Case Else

                    Throw (New ArgumentOutOfRangeException("index"))
            End Select

        End Set
    End Property




    ''' <summary>
    ''' Obtains the dot product of two vectors.
    ''' </summary>
    ''' <param name="u">Vector2f.</param>
    ''' <param name="v">Vector2f.</param>
    ''' <returns>The dot product.</returns>
    Public Shared Function DotProduct(ByVal u As Vector2f, ByVal v As Vector2f) As Single

        Return (u.X * v.X) + (u.Y * v.Y)

    End Function

    ''' <summary>
    ''' Obtains the cross product of two vectors.
    ''' </summary>
    ''' <param name="u">Vector2f.</param>
    ''' <param name="v">Vector2f.</param>
    ''' <returns>Vector2f.</returns>
    Public Shared Function CrossProduct(ByVal u As Vector2f, ByVal v As Vector2f) As Single

        Return (u.X * v.Y) - (u.Y * v.X)

    End Function

    ''' <summary>
    ''' Obtains the counter clockwise perpendicular vector .
    ''' </summary>
    ''' <param name="u">Vector2f.</param>
    ''' <returns>Vector2f.</returns>
    Public Shared Function Perpendicular(ByVal u As Vector2f) As Vector2f

        Return New Vector2f(-u.Y, u.X)

    End Function

    ''' <summary>
    ''' Obtains the distance between two points.
    ''' </summary>
    ''' <param name="u">Vector2f.</param>
    ''' <param name="v">Vector2f.</param>
    ''' <returns>Distancie.</returns>
    Public Shared Function Distance(ByVal u As Vector2f, ByVal v As Vector2f) As Single

        Return CSng(Math.Sqrt((u.X - v.X) * (u.X - v.X) + (u.Y - v.Y) * (u.Y - v.Y)))

    End Function

    ''' <summary>
    ''' Obtains the square distance between two points.
    ''' </summary>
    ''' <param name="u">Vector2f.</param>
    ''' <param name="v">Vector2f.</param>
    ''' <returns>Square distance.</returns>
    Public Shared Function SquareDistance(ByVal u As Vector2f, ByVal v As Vector2f) As Single

        Return (u.X - v.X) * (u.X - v.X) + (u.Y - v.Y) * (u.Y - v.Y)

    End Function

    ''' <summary>
    ''' Obtains the angle between two vectors.
    ''' </summary>
    ''' <param name="u">Vector2f.</param>
    ''' <param name="v">Vector2f.</param>
    ''' <returns>Angle in radians.</returns>
    Public Shared Function AngleBetween(ByVal u As Vector2f, ByVal v As Vector2f) As Single

        Dim cos As Single = DotProduct(u, v) / (u.Modulus() * v.Modulus())
        If MathHelper.IsOne(cos) Then
            Return 0
        End If
        If MathHelper.IsOne(-cos) Then
            Return CSng(Math.PI)
        End If
        Return CSng(Math.Acos(cos))

        'if (AreParallel(u, v))
        '{
        '    if (Math.Sign(u.X) == Math.Sign(v.X) && Math.Sign(u.Y) == Math.Sign(v.Y))
        '    {
        '        return 0;
        '    }
        '    return (float)Math.PI;
        '}
        'Vector3f normal = Vector3f.CrossProduct(new Vector3f(u.X, u.Y, 0), new Vector3f(v.X, v.Y, 0));

        'if (normal.Z < 0)
        '{
        '    return (float)(2 * Math.PI - Math.Acos(DotProduct(u, v) / (u.Modulus() * v.Modulus())));
        '}
        'return (float)(Math.Acos(DotProduct(u, v) / (u.Modulus() * v.Modulus())));

    End Function


    ''' <summary>
    ''' Obtains the midpoint.
    ''' </summary>
    ''' <param name="u">Vector2f.</param>
    ''' <param name="v">Vector2f.</param>
    ''' <returns>Vector2f.</returns>
    Public Shared Function MidPoint(ByVal u As Vector2f, ByVal v As Vector2f) As Vector2f

        Return New Vector2f((v.X + u.X) * 0.5F, (v.Y + u.Y) * 0.5F)

    End Function

    ''' <summary>
    ''' Checks if two vectors are perpendicular.
    ''' </summary>
    ''' <param name="u">Vector2f.</param>
    ''' <param name="v">Vector2f.</param>
    ''' <param name="threshold">Tolerance used.</param>
    ''' <returns>True if are penpendicular or false in anyother case.</returns>
    Public Shared Function ArePerpendicular(ByVal u As Vector2f, ByVal v As Vector2f, ByVal threshold As Single) As Boolean

        Return MathHelper.IsZero(DotProduct(u, v), threshold)

    End Function

    ''' <summary>
    ''' Checks if two vectors are parallel.
    ''' </summary>
    ''' <param name="u">Vector2f.</param>
    ''' <param name="v">Vector2f.</param>
    ''' <param name="threshold">Tolerance used.</param>
    ''' <returns>True if are parallel or false in anyother case.</returns>
    Public Shared Function AreParallel(ByVal u As Vector2f, ByVal v As Vector2f, ByVal threshold As Single) As Boolean

        Dim a As Single = u.X * v.Y - u.Y * v.X
        Return MathHelper.IsZero(a, threshold)

    End Function

    ''' <summary>
    ''' Rounds the components of a vector.
    ''' </summary>
    ''' <param name="u">Vector2f.</param>
    ''' <param name="numDigits">Number of significative defcimal digits.</param>
    ''' <returns>Vector2f.</returns>
    Public Shared Function Round(ByVal u As Vector2f, ByVal numDigits As Integer) As Vector2f

        Return New Vector2f(CSng(Math.Round(u.X, numDigits)), CSng(Math.Round(u.Y, numDigits)))

    End Function




    Public Shared Operator =(ByVal u As Vector2f, ByVal v As Vector2f) As Boolean

        Return ((v.X = u.X) AndAlso (v.Y = u.Y))

    End Operator

    Public Shared Operator <>(ByVal u As Vector2f, ByVal v As Vector2f) As Boolean

        Return ((v.X <> u.X) OrElse (v.Y <> u.Y))

    End Operator

    Public Shared Operator +(ByVal u As Vector2f, ByVal v As Vector2f) As Vector2f

        Return New Vector2f(u.X + v.X, u.Y + v.Y)

    End Operator

    Public Shared Operator -(ByVal u As Vector2f, ByVal v As Vector2f) As Vector2f

        Return New Vector2f(u.X - v.X, u.Y - v.Y)

    End Operator

    Public Shared Operator -(ByVal u As Vector2f) As Vector2f

        Return New Vector2f(-u.X, -u.Y)

    End Operator

    Public Shared Operator *(ByVal u As Vector2f, ByVal a As Single) As Vector2f

        Return New Vector2f(u.X * a, u.Y * a)

    End Operator

    Public Shared Operator *(ByVal a As Single, ByVal u As Vector2f) As Vector2f

        Return New Vector2f(u.X * a, u.Y * a)

    End Operator

    Public Shared Operator /(ByVal u As Vector2f, ByVal a As Single) As Vector2f

        Dim invEscalar As Single = 1 / a
        Return New Vector2f(u.X * invEscalar, u.Y * invEscalar)

    End Operator

    Public Shared Operator /(ByVal a As Single, ByVal u As Vector2f) As Vector2f

        Dim invEscalar As Single = 1 / a
        Return New Vector2f(u.X * invEscalar, u.Y * invEscalar)

    End Operator




    ''' <summary>
    ''' Normalizes the vector.
    ''' </summary>
    Public Sub Normalize()

        Dim [mod] As Single = Me.Modulus()
        If [mod] = 0 Then
            Throw New ArithmeticException("Cannot normalize a zero vector")
        End If
        Dim modInv As Single = 1 / [mod]
        Me.m_x *= modInv
        Me.m_y *= modInv

    End Sub

    ''' <summary>
    ''' Obtains the modulus of the vector.
    ''' </summary>
    ''' <returns>Vector modulus.</returns>
    Public Function Modulus() As Single

        Return CSng(Math.Sqrt(DotProduct(Me, Me)))

    End Function

    ''' <summary>
    ''' Returns an array that represents the vector.
    ''' </summary>
    ''' <returns>Array.</returns>
    Public Function ToArray() As Single()

        Dim u = New Single() {Me.m_x, Me.m_y}
        Return u

    End Function




    ''' </summary>
    ''' Check if the components of two vectors are approximate equals.
    ''' <param name="obj">Vector2f.</param>
    ''' <param name="threshold">Maximun tolerance.</param>
    ''' <returns>True if the three components are almost equal or false in anyother case.</returns>
    Public Overloads Function Equals(ByVal obj As Vector2f, ByVal threshold As Single) As Boolean

        If Math.Abs(obj.X - Me.m_x) > threshold Then
            Return False
        End If
        If Math.Abs(obj.Y - Me.m_y) > threshold Then
            Return False
        End If

        Return True

    End Function

    Public Overloads Function Equals(ByVal obj As Vector2f) As Boolean

        Return obj.X = Me.m_x AndAlso obj.Y = Me.m_y

    End Function

    Public Overrides Function Equals(ByVal obj As Object) As Boolean

        If TypeOf obj Is Vector2f Then
            Return Me.Equals(CType(obj, Vector2f))
        End If
        Return False

    End Function

    Public Overrides Function GetHashCode() As Integer

        Return Me.m_x.GetHashCode() Xor Me.m_y.GetHashCode()

    End Function





    ''' <summary>
    ''' Obtains a string that represents the vector.
    ''' </summary>
    ''' <returns>A string text.</returns>
    Public Overrides Function ToString() As String

        Return String.Format("{0};{1}", Me.m_x, Me.m_y)

    End Function

    ''' <summary>
    ''' Obtains a string that represents the vector.
    ''' </summary>
    ''' <param name="provider">An IFormatProvider interface implementation that supplies culture-specific formatting information. </param>
    ''' <returns>A string text.</returns>
    Public Overloads Function ToString(ByVal provider As IFormatProvider) As String

        Return String.Format("{0};{1}", Me.m_x.ToString(provider), Me.m_y.ToString(provider))

    End Function


End Structure