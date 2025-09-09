''' <summary>
''' Classe Contactos
''' </summary>
''' <remarks></remarks>
Public Class mtdContact

    Private _name As String
    Private _organizationName As String
    Private _phone As String
    Private _facsimile As String
    Private _deliveryPoint As String
    Private _city As String
    Private _postalcode As String
    Private _country As String
    Private _electronicMailAddress As String
    Private _role As String

    Private Shared ReadOnly _locker As New Object()


    Public Sub New()

    End Sub

    ' TODO: completar com os restantes atributos
    Public Sub New(ByVal p_name As String, ByVal p_organizationName As String, ByVal p_phone As String, ByVal p_facsimile As String, _
             ByVal p_deliveryPoint As String, ByVal p_city As String, ByVal p_postalcode As String, ByVal p_country As String, _
             ByVal p_electronicMailAddress As String, ByVal p_role As String)
        SyncLock _locker
            Name = p_name
            OrganizationName = p_organizationName
            Phone = p_phone
            Facsimile = p_facsimile
            DeliveryPoint = p_deliveryPoint
            City = p_city
            PostalCode = p_postalcode
            Country = p_country
            ElectronicMailAddress = p_electronicMailAddress
            Role = p_role
        End SyncLock

    End Sub


    ''' <summary>
    ''' Nome do Contacto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Name As String
        Get
            SyncLock _locker
                Return _name
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _name = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Nome da Organização a que pertence o Contacto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OrganizationName As String
        Get
            SyncLock _locker
                Return _organizationName
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _organizationName = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Telefone do Contacto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Phone As String
        Get
            SyncLock _locker
                Return _phone
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _phone = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Fax do Contacto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Facsimile As String
        Get
            SyncLock _locker
                Return _facsimile
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _facsimile = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Morada do Contacto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DeliveryPoint As String
        Get
            SyncLock _locker
                Return _deliveryPoint
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _deliveryPoint = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Cidade do Contacto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property City As String
        Get
            SyncLock _locker
                Return _city
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _city = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Código Postal do Contacto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PostalCode As String
        Get
            SyncLock _locker
                Return _postalcode
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _postalcode = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' País do Contacto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Country As String
        Get
            SyncLock _locker
                Return _country
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _country = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Endereço Electrónico do Contacto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ElectronicMailAddress As String
        Get
            SyncLock _locker
                Return _electronicMailAddress
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _electronicMailAddress = value
            End SyncLock
        End Set
    End Property

    ''' <summary>
    ''' Função do Contacto na Organização
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Role As String
        Get
            SyncLock _locker
                Return _role
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _role = value
            End SyncLock
        End Set
    End Property

End Class
