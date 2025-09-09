
''' <summary>
''' Represent a three component vector of simple precision.
''' </summary>
Public Structure Vector3f


    Private m_x As Single
    Private m_y As Single
    Private m_z As Single 

    
    ''' <summary>
    ''' Initializes a new instance of Vector3f.
    ''' </summary>
    ''' <param name="x">X component.</param>
    ''' <param name="y">Y component.</param>
    ''' <param name="z">Z component.</param>
    Public Sub New(ByVal x As Single, ByVal y As Single, ByVal z As Single)

        Me.m_x = x
        Me.m_y = y
        Me.m_z = z

    End Sub

    ''' <summary>
    ''' Initializes a new instance of Vector3f.
    ''' </summary>
    ''' <param name="array">Array of three elements that represents the vector.</param>
    Public Sub New(ByVal array As Single())

        If array.Length <> 3 Then
            Throw New ArgumentOutOfRangeException("array", array.Length, "The dimension of the array must be three.")
        End If
        Me.m_x = array(0)
        Me.m_y = array(1)
        Me.m_z = array(2)

    End Sub


    ''' <summary>
    ''' Zero vector.
    ''' </summary>
    Public Shared ReadOnly Property Zero() As Vector3f
        Get

            Return New Vector3f(0, 0, 0)

        End Get
    End Property

    ''' <summary>
    ''' Unit X vector.
    ''' </summary>
    Public Shared ReadOnly Property UnitX() As Vector3f
        Get

            Return New Vector3f(1, 0, 0)

        End Get
    End Property

    ''' <summary>
    ''' Unit Y vector.
    ''' </summary>
    Public Shared ReadOnly Property UnitY() As Vector3f
        Get

            Return New Vector3f(0, 1, 0)

        End Get
    End Property

    ''' <summary>
    ''' Unit Z vector.
    ''' </summary>
    Public Shared ReadOnly Property UnitZ() As Vector3f
        Get

            Return New Vector3f(0, 0, 1)

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
    ''' Gets or sets the Z component.
    ''' </summary>
    Public Property Z() As Single
        Get

            Return Me.m_z

        End Get
        Set(ByVal value As Single)

            Me.m_z = value

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
                Case 2

                    Return Me.m_z
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
                Case 2

                    Me.m_z = value
                    Exit Select
                Case Else
                    Throw (New ArgumentOutOfRangeException("index"))
            End Select

        End Set
    End Property


    ''' <summary>
    ''' Obtains the dot product of two vectors.
    ''' </summary>
    ''' <param name="u">Vector3f.</param>
    ''' <param name="v">Vector3f.</param>
    ''' <returns>The dot product.</returns>
    Public Shared Function DotProduct(ByVal u As Vector3f, ByVal v As Vector3f) As Single

        Return (u.X * v.X) + (u.Y * v.Y) + (u.Z * v.Z)

    End Function

    ''' <summary>
    ''' Obtains the cross product of two vectors.
    ''' </summary>
    ''' <param name="u">Vector3f.</param>
    ''' <param name="v">Vector3f.</param>
    ''' <returns>Vector3f.</returns>
    Public Shared Function CrossProduct(ByVal u As Vector3f, ByVal v As Vector3f) As Vector3f

        Dim a As Single = u.Y * v.Z - u.Z * v.Y
        Dim b As Single = u.Z * v.X - u.X * v.Z
        Dim c As Single = u.X * v.Y - u.Y * v.X
        Return New Vector3f(a, b, c)

    End Function

    ''' <summary>
    ''' Obtains the distance between two points.
    ''' </summary>
    ''' <param name="u">Vector3f.</param>
    ''' <param name="v">Vector3f.</param>
    ''' <returns>Distancie.</returns>
    Public Shared Function Distance(ByVal u As Vector3f, ByVal v As Vector3f) As Single

        Return CSng(Math.Sqrt((u.X - v.X) * (u.X - v.X) + (u.Y - v.Y) * (u.Y - v.Y) + (u.Z - v.Z) * (u.Z - v.Z)))

    End Function

    ''' <summary>
    ''' Obtains the square distance between two points.
    ''' </summary>
    ''' <param name="u">Vector3f.</param>
    ''' <param name="v">Vector3f.</param>
    ''' <returns>Square distance.</returns>
    Public Shared Function SquareDistance(ByVal u As Vector3f, ByVal v As Vector3f) As Single

        Return (u.X - v.X) * (u.X - v.X) + (u.Y - v.Y) * (u.Y - v.Y) + (u.Z - v.Z) * (u.Z - v.Z)

    End Function

    ''' <summary>
    ''' Obtains the angle between two vectors.
    ''' </summary>
    ''' <param name="u">Vector3f.</param>
    ''' <param name="v">Vector3f.</param>
    ''' <returns>Angle in radians.</returns>
    Public Shared Function AngleBetween(ByVal u As Vector3f, ByVal v As Vector3f) As Single

        Dim cos As Single = DotProduct(u, v) / (u.Modulus() * v.Modulus())
        If MathHelper.IsOne(cos) Then
            Return 0
        End If
        If MathHelper.IsOne(-cos) Then
            Return CSng(Math.PI)
        End If
        Return CSng(Math.Acos(cos))

    End Function

    ''' <summary>
    ''' Obtains the midpoint.
    ''' </summary>
    ''' <param name="u">Vector3f.</param>
    ''' <param name="v">Vector3f.</param>
    ''' <returns>Vector3f.</returns>
    Public Shared Function MidPoint(ByVal u As Vector3f, ByVal v As Vector3f) As Vector3f

        Return New Vector3f((v.X + u.X) * 0.5F, (v.Y + u.Y) * 0.5F, (v.Z + u.Z) * 0.5F)

    End Function

    ''' <summary>
    ''' Checks if two vectors are perpendicular.
    ''' </summary>
    ''' <param name="u">Vector3f.</param>
    ''' <param name="v">Vector3f.</param>
    ''' <param name="threshold">Tolerance used.</param>
    ''' <returns>True if are penpendicular or false in anyother case.</returns>
    Public Shared Function ArePerpendicular(ByVal u As Vector3f, ByVal v As Vector3f, ByVal threshold As Single) As Boolean

        Return MathHelper.IsZero(DotProduct(u, v), threshold)


    End Function

    ''' <summary>
    ''' Checks if two vectors are parallel.
    ''' </summary>
    ''' <param name="u">Vector3f.</param>
    ''' <param name="v">Vector3f.</param>
    ''' <param name="threshold">Tolerance used.</param>
    ''' <returns>True if are parallel or false in anyother case.</returns>
    Public Shared Function AreParallel(ByVal u As Vector3f, ByVal v As Vector3f, ByVal threshold As Single) As Boolean

        Dim a As Single = u.Y * v.Z - u.Z * v.Y
        Dim b As Single = u.Z * v.X - u.X * v.Z
        Dim c As Single = u.X * v.Y - u.Y * v.X
        If Not MathHelper.IsZero(a, threshold) Then
            Return False
        End If
        If Not MathHelper.IsZero(b, threshold) Then
            Return False
        End If
        If Not MathHelper.IsZero(c, threshold) Then
            Return False
        End If
        Return True


    End Function

    ''' <summary>
    ''' Rounds the components of a vector.
    ''' </summary>
    ''' <param name="u">Vector3f.</param>
    ''' <param name="numDigits">Number of significative defcimal digits.</param>
    ''' <returns>Vector3F.</returns>
    Public Shared Function Round(ByVal u As Vector3f, ByVal numDigits As Integer) As Vector3f

        Return New Vector3f(CSng(Math.Round(u.X, numDigits)), CSng(Math.Round(u.Y, numDigits)), CSng(Math.Round(u.Z, numDigits)))

    End Function

    Public Shared Narrowing Operator CType(ByVal u As Vector3f) As Vector3d

        Return New Vector3d(u.X, u.Y, u.Z)

    End Operator

    Public Shared Operator =(ByVal u As Vector3f, ByVal v As Vector3f) As Boolean

        Return ((v.X = u.X) AndAlso (v.Y = u.Y) AndAlso (v.Z = u.Z))

    End Operator

    Public Shared Operator <>(ByVal u As Vector3f, ByVal v As Vector3f) As Boolean

        Return ((v.X <> u.X) OrElse (v.Y <> u.Y) OrElse (v.Z <> u.Z))

    End Operator


    Public Shared Operator +(ByVal u As Vector3f, ByVal v As Vector3f) As Vector3f

        Return New Vector3f(u.X + v.X, u.Y + v.Y, u.Z + v.Z)

    End Operator

    Public Shared Operator -(ByVal u As Vector3f, ByVal v As Vector3f) As Vector3f

        Return New Vector3f(u.X - v.X, u.Y - v.Y, u.Z - v.Z)

    End Operator

    Public Shared Operator -(ByVal u As Vector3f) As Vector3f

        Return New Vector3f(-u.X, -u.Y, -u.Z)

    End Operator

    Public Shared Operator *(ByVal u As Vector3f, ByVal a As Single) As Vector3f

        Return New Vector3f(u.X * a, u.Y * a, u.Z * a)

    End Operator

    Public Shared Operator *(ByVal a As Single, ByVal u As Vector3f) As Vector3f

        Return New Vector3f(u.X * a, u.Y * a, u.Z * a)

    End Operator

    Public Shared Operator /(ByVal u As Vector3f, ByVal a As Single) As Vector3f

        Dim invEscalar As Single = 1 / a
        Return New Vector3f(u.X * invEscalar, u.Y * invEscalar, u.Z * invEscalar)

    End Operator


    Public Shared Operator /(ByVal a As Single, ByVal u As Vector3f) As Vector3f

        Dim invEscalar As Single = 1 / a
        Return New Vector3f(u.X * invEscalar, u.Y * invEscalar, u.Z * invEscalar)

    End Operator



    ''' <summary>
    ''' Normalizes the vector.
    ''' </summary>
    Public Sub Normalize()

        Dim [mod] As Single = Me.Modulus()
        If [mod] = 0 Then
            Throw New ArithmeticException("Cannot normalize a zero vector.")
        End If
        Dim modInv As Single = 1 / [mod]
        Me.m_x *= modInv
        Me.m_y *= modInv
        Me.m_z *= modInv

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

        Dim u As Single() = New Single() {Me.m_x, Me.m_y, Me.m_z}
        Return u

    End Function


    ''' </summary>
    ''' Check if the components of two vectors are approximate equals.
    ''' <param name="obj">Vector3f.</param>
    ''' <param name="threshold">Maximun tolerance.</param>
    ''' <returns>True if the three components are almost equal or false in anyother case.</returns>
    Public Overloads Function Equals(ByVal obj As Vector3f, ByVal threshold As Single) As Boolean

        If Math.Abs(obj.X - Me.m_x) > threshold Then
            Return False
        End If
        If Math.Abs(obj.Y - Me.m_y) > threshold Then
            Return False
        End If
        If Math.Abs(obj.Z - Me.m_z) > threshold Then
            Return False
        End If

        Return True

    End Function

    Public Overloads Function Equals(ByVal obj As Vector3f) As Boolean

        Return obj.X = Me.m_x AndAlso obj.Y = Me.m_y AndAlso obj.Z = Me.m_z

    End Function

    Public Overrides Function Equals(ByVal obj As Object) As Boolean

        If TypeOf obj Is Vector3f Then
            Return Me.Equals(CType(obj, Vector3f))
        End If
        Return False

    End Function

    Public Overrides Function GetHashCode() As Integer

        Return Me.m_x.GetHashCode() Xor Me.m_y.GetHashCode() Xor Me.m_z.GetHashCode()

    End Function





    ''' <summary>
    ''' Obtains a string that represents the vector.
    ''' </summary>
    ''' <returns>A string text.</returns>
    Public Overrides Function ToString() As String

        Return String.Format("{0};{1};{2}", Me.m_x, Me.m_y, Me.m_z)

    End Function

    ''' <summary>
    ''' Obtains a string that represents the vector.
    ''' </summary>
    ''' <param name="provider">An IFormatProvider interface implementation that supplies culture-specific formatting information. </param>
    ''' <returns>A string text.</returns>
    Public Overloads Function ToString(ByVal provider As IFormatProvider) As String

        Return String.Format("{0};{1};{2}", Me.m_x.ToString(provider), Me.m_y.ToString(provider), Me.m_z.ToString(provider))


    End Function

End Structure