Imports System.Runtime.Serialization

''' <summary>
''' Implementa a Classe com informação sobre o Sistema de Coordenadas
''' </summary>
''' <remarks></remarks>
<DataContract()> _
Public Class mtdReferenceSystemInfo

    Private _code As String ' Código - 3763
    Private _codespace As String ' Âmbito - Entidade Responsavel pela Gestão - EPSG
    Private _srs As String ' wkt do coordinatesystem
    Private _cs As Object ' objecto coordinatesystem específico do provider

    Private Shared ReadOnly _locker As New Object()

    Public Sub New()

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_srswkt">WKT (Well Known Text) do Sistema de Coordenadas </param>
    ''' <param name="p_cs">Objecto Coordinatesystem específico do provider</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal p_srswkt As String, ByRef p_cs As Object)
        SyncLock _locker
            WKT = p_srswkt
            [Object] = p_cs
        End SyncLock
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="p_code">Código EPSG do Sistema de Coordenadas</param>
    ''' <param name="p_codespace">Entidade Responsável pelo Sistema de Coordenadas</param>
    ''' <param name="p_srswkt">WKT (Well Known Text) do Sistema de Coordenadas</param>
    ''' <param name="p_cs">Objecto Coordinatesystem específico do provider</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal p_code As String, ByVal p_codespace As String, ByVal p_srswkt As String, ByRef p_cs As Object)
        Me.New(p_srswkt, p_cs)
        SyncLock _locker
            _code = p_code
            _codespace = p_codespace
        End SyncLock
    End Sub

    ''' <summary>
    ''' WKT (Well Known Text) do Sistema de Coordenadas 
    ''' </summary>
    ''' <value>novo WKT (Well Known Text) do Sistema de Coordenadas</value>
    ''' <returns>WKT (Well Known Text) do Sistema de Coordenadas</returns>
    ''' <remarks></remarks>
    <DataMember()> _
    Public Property WKT() As String
        Get
            SyncLock _locker
                Return _srs
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _srs = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Objecto Coordinatesystem específico do provider
    ''' </summary>
    ''' <value>novo Objecto CoordinateSystem específico do provider</value>
    ''' <returns>Objecto Coordinatesystem específico do provider</returns>
    ''' <remarks></remarks>

    Public Property [Object]() As Object
        Get
            SyncLock _locker
                Return _cs
            End SyncLock
        End Get
        Set(ByVal value As Object)
            SyncLock _locker
                _cs = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Código EPSG do Sistema de Coordenadas
    ''' </summary>
    ''' <value>novo Código EPSG do Sistema de Coordenadas</value>
    ''' <returns>Código EPSG do Sistema de Coordenadas</returns>
    ''' <remarks></remarks>
    <DataMember()> _
    Public Property Code As String
        Get
            SyncLock _locker
                Return _code
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _code = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Entidade Responsável pelo Sistema de Coordenadas
    ''' </summary>
    ''' <value>novo Nome da Entidade Responsável pelo Sistema de Coordenadas</value>
    ''' <returns>>Nome da Entidade Responsável pelo Sistema de Coordenadas</returns>
    ''' <remarks></remarks>
    <DataMember()> _
    Public Property CodeSpace As String
        Get
            SyncLock _locker
                Return _codespace
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _codespace = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Verifica se o WKT de dois Sistemas de Coordenadas é idêntico
    ''' </summary>
    ''' <param name="cs">Sistema de Coordenadas a comparar</param>
    ''' <returns>Verdadeiro ou Falso</returns>
    ''' <remarks></remarks>
    Public Function IsEqual(ByRef cs As mtdReferenceSystemInfo) As Boolean
        SyncLock _locker
            Return WKT.ToLower = cs.WKT.ToLower
        End SyncLock
    End Function

End Class


