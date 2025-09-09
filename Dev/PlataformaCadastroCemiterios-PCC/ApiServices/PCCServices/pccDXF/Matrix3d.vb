

Imports System.Text


''' <summary>
''' Represents a 3x3 double precision matrix.
''' </summary>
Public Structure Matrix3d


    Private mM11 As Double
    Private mM12 As Double
    Private mM13 As Double
    Private mM21 As Double
    Private mM22 As Double
    Private mM23 As Double
    Private mM31 As Double
    Private mM32 As Double
    Private mM33 As Double


     


    ''' <summary>
    ''' Initializes a new instance of Matrix3d.
    ''' </summary>
    ''' <param name="m11">Element [0,0].</param>
    ''' <param name="m12">Element [0,1].</param>
    ''' <param name="m13">Element [0,1].</param>
    ''' <param name="m21">Element [1,0].</param>
    ''' <param name="m22">Element [1,1].</param>
    ''' <param name="m23">Element [1,2].</param>
    ''' <param name="m31">Element [2,0].</param>
    ''' <param name="m32">Element [2,1].</param>
    ''' <param name="m33">Element [2,2].</param>
    Public Sub New(ByVal m11 As Double, ByVal m12 As Double, ByVal m13 As Double, ByVal m21 As Double, ByVal m22 As Double, ByVal m23 As Double, _
     ByVal m31 As Double, ByVal m32 As Double, ByVal m33 As Double)

        Me.mM11 = m11
        Me.mM12 = m12
        Me.mM13 = m13

        Me.mM21 = m21
        Me.mM22 = m22
        Me.mM23 = m23

        Me.mM31 = m31
        Me.mM32 = m32
        Me.mM33 = m33

    End Sub





    ''' <summary>
    ''' Gets the zero matrix.
    ''' </summary>
    Public Shared ReadOnly Property Zero() As Matrix3d
        Get

            Return New Matrix3d(0, 0, 0, 0, 0, 0, _
             0, 0, 0)

        End Get
    End Property

    ''' <summary>
    ''' Getx the identity matrix.
    ''' </summary>
    Public Shared ReadOnly Property Identity() As Matrix3d
        Get

            Return New Matrix3d(1, 0, 0, 0, 1, 0, _
             0, 0, 1)

        End Get
    End Property





    ''' <summary>
    ''' Gets or sets the matrix element [0,0].
    ''' </summary>
    Public Property M11() As Double
        Get

            Return Me.mM11

        End Get
        Set(ByVal value As Double)

            Me.mM11 = value

        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the matrix element [0,1].
    ''' </summary>
    Public Property M12() As Double
        Get

            Return Me.mM12

        End Get
        Set(ByVal value As Double)

            Me.mM12 = value

        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the matrix element [0,2].
    ''' </summary>
    Public Property M13() As Double
        Get

            Return Me.mM13

        End Get
        Set(ByVal value As Double)

            Me.mM13 = value

        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the matrix element [1,0].
    ''' </summary>
    Public Property M21() As Double
        Get

            Return Me.mM21

        End Get
        Set(ByVal value As Double)

            Me.mM21 = value

        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the matrix element [1,1].
    ''' </summary>
    Public Property M22() As Double
        Get
            Return Me.mM22
        End Get
        Set(ByVal value As Double)
            Me.mM22 = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the matrix element [1,2].
    ''' </summary>
    Public Property M23() As Double
        Get

            Return Me.mM23

        End Get
        Set(ByVal value As Double)

            Me.mM23 = value

        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the matrix element [2,0].
    ''' </summary>
    Public Property M31() As Double
        Get

            Return Me.mM31

        End Get
        Set(ByVal value As Double)

            Me.mM31 = value

        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the matrix element [2,1].
    ''' </summary>
    Public Property M32() As Double
        Get

            Return Me.mM32

        End Get
        Set(ByVal value As Double)

            Me.mM32 = value

        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the matrix element [2,2].
    ''' </summary>
    Public Property M33() As Double
        Get

            Return Me.mM33

        End Get
        Set(ByVal value As Double)

            Me.mM33 = value

        End Set
    End Property




    ''' <summary>
    ''' Matrix addition.
    ''' </summary>
    ''' <param name="a">Matrix3d.</param>
    ''' <param name="b">Matrix3d.</param>
    ''' <returns>Matrix3d.</returns>
    Public Shared Operator +(ByVal a As Matrix3d, ByVal b As Matrix3d) As Matrix3d

        Return New Matrix3d(a.M11 + b.M11, a.M12 + b.M12, a.M13 + b.M13, a.M21 + b.M21, a.M22 + b.M22, a.M23 + b.M23, _
         a.M31 + b.M31, a.M32 + b.M32, a.M33 + b.M33)

    End Operator

    ''' <summary>
    ''' Matrix substraction.
    ''' </summary>
    ''' <param name="a">Matrix3d.</param>
    ''' <param name="b">Matrix3d.</param>
    ''' <returns>Matrix3d.</returns>
    Public Shared Operator -(ByVal a As Matrix3d, ByVal b As Matrix3d) As Matrix3d

        Return New Matrix3d(a.M11 - b.M11, a.M12 - b.M12, a.M13 - b.M13, a.M21 - b.M21, a.M22 - b.M22, a.M23 - b.M23, _
               a.M31 - b.M31, a.M32 - b.M32, a.M33 - b.M33)


    End Operator

    ''' <summary>
    ''' Product of two matrices.
    ''' </summary>
    ''' <param name="a">Matrix3d.</param>
    ''' <param name="b">Matrix3d.</param>
    ''' <returns>Matrix3d.</returns>
    Public Shared Operator *(ByVal a As Matrix3d, ByVal b As Matrix3d) As Matrix3d

        Return New Matrix3d(a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31, a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32, a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33, a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31, a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32, a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33, _
                a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31, a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32, a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33)


    End Operator

    ''' <summary>
    ''' Product of a matrix with a vector.
    ''' </summary>
    ''' <param name="a">Matrix3d.</param>
    ''' <param name="u">Vector3f.</param>
    ''' <returns>Matrix3d.</returns>
    Public Shared Operator *(ByVal a As Matrix3d, ByVal u As Vector3d) As Vector3d

        Return New Vector3d(a.M11 * u.X + a.M12 * u.Y + a.M13 * u.Z, a.M21 * u.X + a.M22 * u.Y + a.M23 * u.Z, a.M31 * u.X + a.M32 * u.Y + a.M33 * u.Z)


    End Operator

    ''' <summary>
    ''' Product of a matrix with a scalar.
    ''' </summary>
    ''' <param name="m">Matrix3d.</param>
    ''' <param name="a">Scalar.</param>
    ''' <returns>Matrix3d.</returns>
    Public Shared Operator *(ByVal m As Matrix3d, ByVal a As Double) As Matrix3d

        Return New Matrix3d(m.M11 * a, m.M12 * a, m.M13 * a, m.M21 * a, m.M22 * a, m.M23 * a, _
                 m.M31 * a, m.M32 * a, m.M33 * a)


    End Operator




    ''' <summary>
    ''' Calculate the determinant of the actual matrix.
    ''' </summary>
    ''' <returns>Determiant.</returns>
    Public Function Determinant() As Double

        Return Me.mM11 * Me.mM22 * Me.mM33 + Me.mM12 * Me.mM23 * Me.mM31 + Me.mM13 * Me.mM21 * Me.mM32 - Me.mM13 * Me.mM22 * Me.mM31 - Me.mM11 * Me.mM23 * Me.mM32 - Me.mM12 * Me.mM21 * Me.mM33


    End Function

    ''' <summary>
    ''' Calculates the inverse matrix.
    ''' </summary>
    ''' <returns>Inverse Matrix3d.</returns>
    Public Function Inverse() As Matrix3d

        Dim det As Double = Me.Determinant()
        Dim resultado = New Matrix3d()
        If det = 0 Then
            Throw (New ArithmeticException("The matrix is not invertible"))
        End If
        det = 1 / det

        resultado.M11 = det * (Me.mM22 * Me.mM33 - Me.mM23 * Me.mM32)
        resultado.M12 = det * (Me.mM13 * Me.mM32 - Me.mM12 * Me.mM33)
        resultado.M13 = det * (Me.mM12 * Me.mM23 - Me.mM13 * Me.mM22)

        resultado.M21 = det * (Me.mM23 * Me.mM31 - Me.mM21 * Me.mM33)
        resultado.M22 = det * (Me.mM11 * Me.mM33 - Me.mM13 * Me.mM31)
        resultado.M23 = det * (Me.mM13 * Me.mM21 - Me.mM11 * Me.mM23)

        resultado.M31 = det * (Me.mM21 * Me.mM32 - Me.mM22 * Me.mM31)
        resultado.M32 = det * (Me.mM12 * Me.mM31 - Me.mM11 * Me.mM32)
        resultado.M33 = det * (Me.mM11 * Me.mM22 - Me.mM12 * Me.mM21)

        Return resultado

    End Function

    ''' <summary>
    ''' Obtains the traspose matrix.
    ''' </summary>
    ''' <returns>Transpose matrix.</returns>
    Public Function Traspose() As Matrix3d

        Return New Matrix3d(Me.mM11, Me.mM21, Me.mM31, Me.mM12, Me.mM22, Me.mM32, _
         Me.mM13, Me.mM23, Me.mM33)

    End Function





    ''' <summary>
    ''' Obtains a string that represents the matrix.
    ''' </summary>
    ''' <returns>A string text.</returns>
    Public Overrides Function ToString() As String

        Dim s = New StringBuilder()
        s.Append(String.Format("|{0};{1};{2}|" & Environment.NewLine, Me.mM11, Me.mM12, Me.mM13))
        s.Append(String.Format("|{0};{1};{2}|" & Environment.NewLine, Me.mM21, Me.mM22, Me.mM23))
        s.Append(String.Format("|{0};{1};{2}|" & Environment.NewLine, Me.mM31, Me.mM32, Me.mM33))

        Return s.ToString()

    End Function

    ''' <summary>
    ''' Obtains a string that represents the matrix.
    ''' </summary>
    ''' <param name="provider">An IFormatProvider interface implementation that supplies culture-specific formatting information. </param>
    ''' <returns>A string text.</returns>
    Public Overloads Function ToString(ByVal provider As IFormatProvider) As String

        Dim s = New StringBuilder()
        s.Append(String.Format("|{0};{1};{2}|" & Environment.NewLine, Me.mM11.ToString(provider), Me.mM12.ToString(provider), Me.mM13.ToString(provider)))
        s.Append(String.Format("|{0};{1};{2}|" & Environment.NewLine, Me.mM21.ToString(provider), Me.mM22.ToString(provider), Me.mM23.ToString(provider)))
        s.Append(String.Format("|{0};{1};{2}|" & Environment.NewLine, Me.mM31.ToString(provider), Me.mM32.ToString(provider), Me.mM33.ToString(provider)))

        Return s.ToString()

    End Function


End Structure

