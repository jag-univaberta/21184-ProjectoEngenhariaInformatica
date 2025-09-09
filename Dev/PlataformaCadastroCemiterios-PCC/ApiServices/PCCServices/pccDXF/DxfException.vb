

Public Class DxfException
    Inherits Exception
    Private ReadOnly m_file As String

    Private Shared ReadOnly _locker As New Object()

    Public Sub New(ByVal file As String)
        SyncLock _locker
            Me.m_file = file
        End SyncLock
    End Sub

    Public Sub New(ByVal file As String, ByVal message As String)
        MyBase.New(message)
        SyncLock _locker
            Me.m_file = file
        End SyncLock
    End Sub

    Public Sub New(ByVal file As String, ByVal message As String, ByVal innerException As Exception)
        MyBase.New(message, innerException)
        SyncLock _locker
            Me.m_file = file
        End SyncLock
    End Sub

    Public ReadOnly Property File() As String
        Get
            SyncLock _locker
                Return Me.m_file
            End SyncLock
        End Get
    End Property
End Class



Public Class DxfHeaderVariableException
    Inherits DxfException
    Private ReadOnly m_name As String

    Private Shared ReadOnly _locker As New Object()

    Public Sub New(ByVal name As String, ByVal file As String)
        MyBase.New(file)
        SyncLock _locker
            Me.m_name = name
        End SyncLock
    End Sub

    Public Sub New(ByVal name As String, ByVal file As String, ByVal message As String)
        MyBase.New(file, message)
        SyncLock _locker
            Me.m_name = name
        End SyncLock
    End Sub

    Public ReadOnly Property Name() As String
        Get
            SyncLock _locker
                Return Me.m_name
            End SyncLock
        End Get
    End Property
End Class

Public Class DxfSectionException
    Inherits DxfException
    Private ReadOnly m_section As String

    Private Shared ReadOnly _locker As New Object()

    Public Sub New(ByVal section As String, ByVal file As String)
        MyBase.New(file)
        SyncLock _locker
            Me.m_section = section
        End SyncLock
    End Sub

    Public Sub New(ByVal section As String, ByVal file As String, ByVal message As String)
        MyBase.New(file, message)
        SyncLock _locker
            Me.m_section = section
        End SyncLock
    End Sub

    Public ReadOnly Property Section() As String
        Get
            SyncLock _locker
                Return Me.m_section
            End SyncLock
        End Get
    End Property
End Class

Public Class InvalidDxfSectionException
    Inherits DxfSectionException

    Private Shared ReadOnly _locker As New Object()

    Public Sub New(ByVal section As String, ByVal file As String)
        MyBase.New(section, file)
    End Sub

    Public Sub New(ByVal section As String, ByVal file As String, ByVal message As String)
        MyBase.New(section, file, message)
    End Sub
End Class

Public Class OpenDxfSectionException
    Inherits DxfSectionException

    Private Shared ReadOnly _locker As New Object()

    Public Sub New(ByVal section As String, ByVal file As String)
        MyBase.New(section, file)
    End Sub

    Public Sub New(ByVal section As String, ByVal file As String, ByVal message As String)
        MyBase.New(section, file, message)
    End Sub
End Class

Public Class ClosedDxfSectionException
    Inherits DxfSectionException

    Private Shared ReadOnly _locker As New Object()

    Public Sub New(ByVal section As String, ByVal file As String)
        MyBase.New(section, file)
    End Sub

    Public Sub New(ByVal section As String, ByVal file As String, ByVal message As String)
        MyBase.New(section, file, message)
    End Sub
End Class

 
Public Class DxfTableException
    Inherits DxfException

    Private Shared ReadOnly _locker As New Object()

    Private ReadOnly m_table As String

    Public Sub New(ByVal table As String, ByVal file As String)
        MyBase.New(file)
        SyncLock _locker
            Me.m_table = table
        End SyncLock
    End Sub

    Public Sub New(ByVal table As String, ByVal file As String, ByVal message As String)
        MyBase.New(file, message)
        SyncLock _locker
            Me.m_table = table
        End SyncLock
    End Sub

    Public ReadOnly Property Table() As String
        Get
            SyncLock _locker
                Return Me.m_table
            End SyncLock
        End Get
    End Property
End Class

Public Class InvalidDxfTableException
    Inherits DxfTableException

    Private Shared ReadOnly _locker As New Object()

    Public Sub New(ByVal table As String, ByVal file As String)
        MyBase.New(table, file)
    End Sub

    Public Sub New(ByVal table As String, ByVal file As String, ByVal message As String)
        MyBase.New(table, file, message)
    End Sub
End Class

Public Class OpenDxfTableException
    Inherits DxfTableException

    Private Shared ReadOnly _locker As New Object()

    Public Sub New(ByVal table As String, ByVal file As String)
        MyBase.New(table, file)
    End Sub

    Public Sub New(ByVal table As String, ByVal file As String, ByVal message As String)
        MyBase.New(table, file, message)
    End Sub
End Class

Public Class ClosedDxfTableException
    Inherits DxfTableException

    Private Shared ReadOnly _locker As New Object()

    Public Sub New(ByVal table As String, ByVal file As String)
        MyBase.New(table, file)
    End Sub

    Public Sub New(ByVal table As String, ByVal file As String, ByVal message As String)
        MyBase.New(table, file, message)
    End Sub
End Class

 
Public Class DxfEntityException
    Inherits DxfException
    Private ReadOnly m_name As String

    Private Shared ReadOnly _locker As New Object()

    Public Sub New(ByVal name As String, ByVal file As String, ByVal message As String)
        MyBase.New(file, message)
        SyncLock _locker
            Me.m_name = name
        End SyncLock
    End Sub

    Public ReadOnly Property Name() As String
        Get
            SyncLock _locker
                Return Me.m_name
            End SyncLock
        End Get
    End Property
End Class

Public Class DxfInvalidCodeValueEntityException
    Inherits DxfException
    Private ReadOnly m_code As Integer
    Private ReadOnly m_value As String

    Private Shared ReadOnly _locker As New Object()

    Public Sub New(ByVal code As Integer, ByVal value As String, ByVal file As String, ByVal message As String)
         
        MyBase.New(file, message)
        SyncLock _locker
            Me.m_code = code
            Me.m_value = value
        End SyncLock
    End Sub

    Public ReadOnly Property Code() As Integer
        Get
            SyncLock _locker
                Return Me.m_code
            End SyncLock
        End Get
    End Property

    Public ReadOnly Property Value() As String
        Get
            SyncLock _locker
                Return Me.m_value
            End SyncLock
        End Get
    End Property
End Class



