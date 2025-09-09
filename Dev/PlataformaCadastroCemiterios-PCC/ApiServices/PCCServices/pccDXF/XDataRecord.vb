
''' <summary>
''' Represents an entry in the extended data of an entity.
''' </summary>
Public Structure XDataRecord


    Private m_value As Object
    Private m_code As Integer

     

    ''' <summary>
    ''' An extended data control string can be either �{�or �}�.
    ''' These braces enable applications to organize their data by subdividing the data into lists.
    ''' The left brace begins a list, and the right brace terminates the most recent list. Lists can be nested
    ''' </summary>
    Public Shared ReadOnly Property OpenControlString() As XDataRecord
        Get

            Return New XDataRecord(XDataCode.ControlString, "{")

        End Get
    End Property

    ''' <summary>
    ''' An extended data control string can be either "{" or "}".
    ''' These braces enable applications to organize their data by subdividing the data into lists.
    ''' The left brace begins a list, and the right brace terminates the most recent list. Lists can be nested
    ''' </summary>
    Public Shared ReadOnly Property CloseControlString() As XDataRecord
        Get

            Return New XDataRecord(XDataCode.ControlString, "}")

        End Get
    End Property



    ''' <summary>
    ''' Initializes a new XDataRecord.
    ''' </summary>
    ''' <param name="code">XData code.</param>
    ''' <param name="value">XData value.</param>
    Public Sub New(ByVal code As Integer, ByVal value As Object)

        Me.m_code = code
        Me.m_value = value

    End Sub





    ''' <summary>
    ''' Gets or set the XData code.
    ''' </summary>
    Public Property Code() As Integer
        Get

            Return Me.m_code

        End Get
        Set(ByVal value As Integer)

            Me.m_code = m_value

        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the XData value.
    ''' </summary>
    Public Property Value() As Object
        Get

            Return Me.m_value

        End Get
        Set(ByVal value As Object)

            Me.m_value = m_value

        End Set
    End Property



    Public Overrides Function ToString() As String

            Return String.Format("{0} - {1}", Me.m_code, Me.m_value)

    End Function
End Structure 