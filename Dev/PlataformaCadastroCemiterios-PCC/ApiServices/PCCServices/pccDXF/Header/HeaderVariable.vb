Imports System.Collections.Generic

Namespace Header
    ''' <summary>
    ''' Defines de dxf file version.
    ''' </summary>
    Friend Class HeaderVariable
        Public Const NAME_CODE_GROUP As Integer = 9
        Public Shared ReadOnly Allowed As Dictionary(Of String, Integer) = InitializeSystemVariables()
        Private ReadOnly m_name As String
        Private ReadOnly m_codeGroup As Integer
        Private ReadOnly m_value As Object

        Private Shared ReadOnly _locker As New Object()

        Public Sub New(ByVal name As String, ByVal value As Object)
            SyncLock _locker
                If Not Allowed.ContainsKey(name) Then
                    Throw New ArgumentOutOfRangeException("name", name, "Variable name " & name & " not defined.")
                End If
                Me.m_codeGroup = Allowed(name)
                Me.m_name = name
                Me.m_value = value
            End SyncLock
        End Sub

        Public ReadOnly Property Name() As String
            Get
                SyncLock _locker
                    Return Me.m_name
                End SyncLock
            End Get
        End Property

        Public ReadOnly Property CodeGroup() As Integer
            Get
                SyncLock _locker
                    Return Me.m_codeGroup
                End SyncLock
            End Get
        End Property

        Public ReadOnly Property Value() As Object
            Get
                SyncLock _locker
                    Return Me.m_value
                End SyncLock
            End Get
        End Property

        Public Overrides Function ToString() As String
            SyncLock _locker
                Return [String].Format("{0} : {1}", Me.m_name, Me.m_value)
            End SyncLock
        End Function

        Private Shared Function InitializeSystemVariables() As Dictionary(Of String, Integer)

            Return New Dictionary(Of String, Integer)() From { _
             {SystemVariable.DabaseVersion, 1}, _
             {SystemVariable.HandSeed, 5} _
            }
        End Function
    End Class
End Namespace
