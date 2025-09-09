
''' <summary>
''' Represent a two component vector of double precision.
''' </summary>
Public Structure Vector2d


    Private m_x As Double
    Private m_y As Double 

    
    ''' <summary>
    ''' Initializes a new instance of Vector2d.
    ''' </summary>
    ''' <param name="x">X component.</param>
    ''' <param name="y">Y component.</param>
    Public Sub New(ByVal x As Double, ByVal y As Double)

        Me.m_x = x
        Me.m_y = y

    End Sub

    ''' <summary>
    ''' Initializes a new instance of Vector2d.
    ''' </summary>
    ''' <param name="array">Array of two elements that represents the vector.</param>
    Public Sub New(ByVal array As Double())

        If array.Length <> 2 Then
            Throw New ArgumentOutOfRangeException("array", array.Length, "The dimension of the array must be two")
        End If
        Me.m_x = array(0)
        Me.m_y = array(1)

    End Sub
    ''' <summary>
    ''' Zero vector.
    ''' </summary>
    Public Shared ReadOnly Property Zero() As Vector2d
        Get

            Return New Vector2d(0, 0)

        End Get
    End Property

    ''' <summary>
    ''' Unit X vector.
    ''' </summary>
    Public Shared ReadOnly Property UnitX() As Vector2d
        Get

            Return New Vector2d(1, 0)

        End Get
    End Property

    ''' <summary>
    ''' Unit Y vector.
    ''' </summary>
    Public Shared ReadOnly Property UnitY() As Vector2d
        Get

            Return New Vector2d(0, 1)

        End Get
    End Property

    ''' <summary>
    ''' Gets or sets the X component.
    ''' </summary>
    Public Property X() As Double
        Get

            Return Me.m_x

        End Get
        Set(ByVal value As Double)

            Me.m_x = value

        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the Y component.
    ''' </summary>
    Public Property Y() As Double
        Get

            Return Me.m_y

        End Get
        Set(ByVal value As Double)

            Me.m_y = value

        End Set
    End Property

    ''' <summary>
    ''' Gets or sets a vector element defined by its index.
    ''' </summary>
    ''' <param name="index">Index of the element.</param>
    Default Public Property Item(ByVal index As Integer) As Double
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
        Set(ByVal value As Double)

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
    ''' <param name="u">Vector2d.</param>
    ''' <param name="v">Vector2d.</param>
    ''' <returns>The dot product.</returns>
    Public Shared Function DotProduct(ByVal u As Vector2d, ByVal v As Vector2d) As Double

        Return (u.X * v.X) + (u.Y * v.Y)

    End Function

    ''' <summary>
    ''' Obtains the cross product of two vectors.
    ''' </summary>
    ''' <param name="u">Vector2d.</param>
    ''' <param name="v">Vector2d.</param>
    ''' <returns>Vector2d.</returns>
    Public Shared Function CrossProduct(ByVal u As Vector2d, ByVal v As Vector2d) As Double

        Return (u.X * v.Y) - (u.Y * v.X)

    End Function

    ''' <summary>
    ''' Obtains the counter clockwise perpendicular vector .
    ''' </summary>
    ''' <param name="u">Vector2d.</param>
    ''' <returns>Vector2d.</returns>
    Public Shared Function Perpendicular(ByVal u As Vector2d) As Vector2d

        Return New Vector2d(-u.Y, u.X)

    End Function

    ''' <summary>
    ''' Obtains the distance between two points.
    ''' </summary>
    ''' <param name="u">Vector2d.</param>
    ''' <param name="v">Vector2d.</param>
    ''' <returns>Distancie.</returns>
    Public Shared Function Distance(ByVal u As Vector2d, ByVal v As Vector2d) As Double

        Return (Math.Sqrt((u.X - v.X) * (u.X - v.X) + (u.Y - v.Y) * (u.Y - v.Y)))

    End Function

    ''' <summary>
    ''' Obtains the square distance between two points.
    ''' </summary>
    ''' <param name="u">Vector2d.</param>
    ''' <param name="v">Vector2d.</param>
    ''' <returns>Square distance.</returns>
    Public Shared Function SquareDistance(ByVal u As Vector2d, ByVal v As Vector2d) As Double

        Return (u.X - v.X) * (u.X - v.X) + (u.Y - v.Y) * (u.Y - v.Y)

    End Function

    ''' <summary>
    ''' Obtains the angle between two vectors.
    ''' </summary>
    ''' <param name="u">Vector2d.</param>
    ''' <param name="v">Vector2d.</param>
    ''' <returns>Angle in radians.</returns>
    Public Shared Function AngleBetween(ByVal u As Vector2d, ByVal v As Vector2d) As Double

        Dim cos As Double = DotProduct(u, v) / (u.Modulus() * v.Modulus())
        If MathHelper.IsOne(cos) Then
            Return 0
        End If
        If MathHelper.IsOne(-cos) Then
            Return Math.PI
        End If
        Return Math.Acos(cos)

        'if (AreParallel(u, v))
        '{
        '    if (Math.Sign(u.X) == Math.Sign(v.X) && Math.Sign(u.Y) == Math.Sign(v.Y))
        '    {
        '        return 0;
        '    }
        '    return (double)Math.PI;
        '}
        'Vector3f normal = Vector3f.CrossProduct(new Vector3f(u.X, u.Y, 0), new Vector3f(v.X, v.Y, 0));

        'if (normal.Z < 0)
        '{
        '    return (double)(2 * Math.PI - Math.Acos(DotProduct(u, v) / (u.Modulus() * v.Modulus())));
        '}
        'return (double)(Math.Acos(DotProduct(u, v) / (u.Modulus() * v.Modulus())));

    End Function


    ''' <summary>
    ''' Obtains the midpoint.
    ''' </summary>
    ''' <param name="u">Vector2d.</param>
    ''' <param name="v">Vector2d.</param>
    ''' <returns>Vector2d.</returns>
    Public Shared Function MidPoint(ByVal u As Vector2d, ByVal v As Vector2d) As Vector2d

        Return New Vector2d((v.X + u.X) * 0.5F, (v.Y + u.Y) * 0.5F)

    End Function

    ''' <summary>
    ''' Checks if two vectors are perpendicular.
    ''' </summary>
    ''' <param name="u">Vector2d.</param>
    ''' <param name="v">Vector2d.</param>
    ''' <param name="threshold">Tolerance used.</param>
    ''' <returns>True if are penpendicular or false in anyother case.</returns>
    Public Shared Function ArePerpendicular(ByVal u As Vector2d, ByVal v As Vector2d, ByVal threshold As Double) As Boolean

        Return MathHelper.IsZero(DotProduct(u, v), threshold)

    End Function

    ''' <summary>
    ''' Checks if two vectors are parallel.
    ''' </summary>
    ''' <param name="u">Vector2d.</param>
    ''' <param name="v">Vector2d.</param>
    ''' <param name="threshold">Tolerance used.</param>
    ''' <returns>True if are parallel or false in anyother case.</returns>
    Public Shared Function AreParallel(ByVal u As Vector2d, ByVal v As Vector2d, ByVal threshold As Double) As Boolean

        Dim a As Double = u.X * v.Y - u.Y * v.X
        Return MathHelper.IsZero(a, threshold)

    End Function

    ''' <summary>
    ''' Rounds the components of a vector.
    ''' </summary>
    ''' <param name="u">Vector2d.</param>
    ''' <param name="numDigits">Number of significative defcimal digits.</param>
    ''' <returns>Vector2d.</returns>
    Public Shared Function Round(ByVal u As Vector2d, ByVal numDigits As Integer) As Vector2d

        Return New Vector2d(Math.Round(u.X, numDigits), Math.Round(u.Y, numDigits))

    End Function




    Public Shared Operator =(ByVal u As Vector2d, ByVal v As Vector2d) As Boolean

        Return ((v.X = u.X) AndAlso (v.Y = u.Y))

    End Operator

    Public Shared Operator <>(ByVal u As Vector2d, ByVal v As Vector2d) As Boolean

        Return ((v.X <> u.X) OrElse (v.Y <> u.Y))

    End Operator

    Public Shared Operator +(ByVal u As Vector2d, ByVal v As Vector2d) As Vector2d

        Return New Vector2d(u.X + v.X, u.Y + v.Y)

    End Operator

    Public Shared Operator -(ByVal u As Vector2d, ByVal v As Vector2d) As Vector2d

        Return New Vector2d(u.X - v.X, u.Y - v.Y)

    End Operator

    Public Shared Operator -(ByVal u As Vector2d) As Vector2d

        Return New Vector2d(-u.X, -u.Y)

    End Operator

    Public Shared Operator *(ByVal u As Vector2d, ByVal a As Double) As Vector2d

        Return New Vector2d(u.X * a, u.Y * a)

    End Operator

    Public Shared Operator *(ByVal a As Double, ByVal u As Vector2d) As Vector2d

        Return New Vector2d(u.X * a, u.Y * a)

    End Operator

    Public Shared Operator /(ByVal u As Vector2d, ByVal a As Double) As Vector2d

        Dim invEscalar As Double = 1 / a
        Return New Vector2d(u.X * invEscalar, u.Y * invEscalar)

    End Operator

    Public Shared Operator /(ByVal a As Double, ByVal u As Vector2d) As Vector2d

        Dim invEscalar As Double = 1 / a
        Return New Vector2d(u.X * invEscalar, u.Y * invEscalar)

    End Operator




    ''' <summary>
    ''' Normalizes the vector.
    ''' </summary>
    Public Sub Normalize()

        Dim [mod] As Double = Me.Modulus()
        If [mod] = 0 Then
            Throw New ArithmeticException("Cannot normalize a zero vector")
        End If
        Dim modInv As Double = 1 / [mod]
        Me.m_x *= modInv
        Me.m_y *= modInv

    End Sub

    ''' <summary>
    ''' Obtains the modulus of the vector.
    ''' </summary>
    ''' <returns>Vector modulus.</returns>
    Public Function Modulus() As Double

        Return (Math.Sqrt(DotProduct(Me, Me)))

    End Function

    ''' <summary>
    ''' Returns an array that represents the vector.
    ''' </summary>
    ''' <returns>Array.</returns>
    Public Function ToArray() As Double()

        Dim u = New Double() {Me.m_x, Me.m_y}
        Return u

    End Function




    ''' </summary>
    ''' Check if the components of two vectors are approximate equals.
    ''' <param name="obj">Vector2d.</param>
    ''' <param name="threshold">Maximun tolerance.</param>
    ''' <returns>True if the three components are almost equal or false in anyother case.</returns>
    Public Overloads Function Equals(ByVal obj As Vector2d, ByVal threshold As Double) As Boolean

        If Math.Abs(obj.X - Me.m_x) > threshold Then
            Return False
        End If
        If Math.Abs(obj.Y - Me.m_y) > threshold Then
            Return False
        End If

        Return True

    End Function

    Public Overloads Function Equals(ByVal obj As Vector2d) As Boolean

        Return obj.X = Me.m_x AndAlso obj.Y = Me.m_y

    End Function

    Public Overrides Function Equals(ByVal obj As Object) As Boolean

        If TypeOf obj Is Vector2d Then
            Return Me.Equals(CType(obj, Vector2d))
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