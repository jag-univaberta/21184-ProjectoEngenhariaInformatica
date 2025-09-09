

Imports System.Collections.Generic
Imports pccDXF4.Tables


''' <summary>
''' Represents the extended data information of an entity.
''' </summary>
Public Class XData


    Private ReadOnly appReg As ApplicationRegistry
    Private xData As List(Of XDataRecord)


    Private Shared ReadOnly _locker As New Object()



    ''' <summary>
    ''' Initialize a new instance of the <c>XData</c> class .
    ''' </summary>
    ''' <param name="appReg">Name of the application associated with the list of extended data records.</param>
    Public Sub New(ByVal appReg As ApplicationRegistry)
        SyncLock _locker
            Me.appReg = appReg
            Me.xData = New List(Of XDataRecord)()
        End SyncLock
    End Sub


    ''' <summary>
    ''' Gets the name of the application associated with the list of extended data records.
    ''' </summary>
    Public ReadOnly Property ApplicationRegistry() As ApplicationRegistry
        Get
            SyncLock _locker
                Return Me.appReg
            End SyncLock
        End Get
    End Property

    ''' <summary>
    ''' Gets or sets the list of extended data records.
    ''' </summary>
    ''' <remarks>
    ''' This list cannot contain a XDataRecord with a XDataCode of AppReg, this code is reserved to register the name of the application.
    ''' Any record with this code will be ommited.
    ''' </remarks>
    Public Property XDataRecord() As List(Of XDataRecord)
        Get
            SyncLock _locker
                Return Me.xData
            End SyncLock
        End Get
        Set(ByVal value As List(Of XDataRecord))
            SyncLock _locker
                If value Is Nothing Then
                    Throw New NullReferenceException("value")
                End If
                Me.xData = value
            End SyncLock
        End Set
    End Property



    ''' <summary>
    ''' Converts the value of this instance to its equivalent string representation.
    ''' </summary>
    ''' <returns>The string representation.</returns>
    Public Overrides Function ToString() As String
        SyncLock _locker
            Return Me.ApplicationRegistry.Name
        End SyncLock
    End Function
End Class 