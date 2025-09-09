' Original source code
' http://www.codeproject.com/KB/cs/stringenum.aspx
' CodeBureau - Matt Simner

Imports System.Collections
Imports System.Reflection





''' <summary>
''' Helper class for working with 'extended' enums using <see cref="StringValueAttribute"/> attributes.
''' </summary>
Public Class StringEnum



    Private ReadOnly m_enumType As Type
    Private Shared ReadOnly stringValues As New Hashtable()

    Private Shared ReadOnly _locker As New Object()

    ''' <summary>
    ''' Creates a new <see cref="StringEnum"/> instance.
    ''' </summary>
    ''' <param name="enumType">Enum type.</param>
    Public Sub New(ByVal enumType As Type)
        SyncLock _locker
            If Not enumType.IsEnum Then
                Throw New ArgumentException([String].Format("Supplied type must be an Enum.  Type was {0}", enumType))
            End If

            Me.m_enumType = enumType
        End SyncLock
    End Sub

    ''' <summary>
    ''' Gets the string value associated with the given enum value.
    ''' </summary>
    ''' <param name="valueName">Name of the enum value.</param>
    ''' <returns>String Value</returns>
    Public Function GetStringValue(ByVal valueName As String) As String
        SyncLock _locker
            Dim stringValue As String
            Try
                Dim type As [Enum] = DirectCast([Enum].Parse(Me.m_enumType, valueName), [Enum])
                stringValue = GetStringValue(type)
            Catch
                Return Nothing
            End Try
            'Swallow!
            Return stringValue
        End SyncLock
    End Function

    ''' <summary>
    ''' Gets the string values associated with the enum.
    ''' </summary>
    ''' <returns>String value array</returns>
    Public Function GetStringValues() As Array
        SyncLock _locker
            Dim values As New ArrayList()
            'Look for our string value associated with fields in this enum
            For Each fi As FieldInfo In Me.m_enumType.GetFields()
                'Check for our custom attribute
                Dim attrs As StringValueAttribute() = TryCast(fi.GetCustomAttributes(GetType(StringValueAttribute), False), StringValueAttribute())
                If attrs IsNot Nothing Then
                    If attrs.Length > 0 Then
                        values.Add(attrs(0).Value)
                    End If
                End If
            Next

            Return values.ToArray()
        End SyncLock
    End Function

    ''' <summary>
    ''' Gets the values as a 'bindable' list datasource.
    ''' </summary>
    ''' <returns>IList for data binding</returns>
    Public Function GetListValues() As IList
        SyncLock _locker
            Dim underlyingType As Type = [Enum].GetUnderlyingType(Me.m_enumType)
            Dim values As New ArrayList()
            'Look for our string value associated with fields in this enum
            For Each fi As FieldInfo In Me.m_enumType.GetFields()
                'Check for our custom attribute
                Dim attrs As StringValueAttribute() = TryCast(fi.GetCustomAttributes(GetType(StringValueAttribute), False), StringValueAttribute())
                If attrs IsNot Nothing Then
                    If attrs.Length > 0 Then
                        values.Add(New DictionaryEntry(Convert.ChangeType([Enum].Parse(Me.m_enumType, fi.Name), underlyingType), attrs(0).Value))
                    End If
                End If
            Next

            Return values
        End SyncLock

    End Function

    ''' <summary>
    ''' Return the existence of the given string value within the enum.
    ''' </summary>
    ''' <param name="stringValue">String value.</param>
    ''' <returns>Existence of the string value</returns>
    Public Function IsStringDefined(ByVal stringValue As String) As Boolean
        SyncLock _locker
            Return Parse(Me.m_enumType, stringValue) IsNot Nothing
        End SyncLock

    End Function

    ''' <summary>
    ''' Return the existence of the given string value within the enum.
    ''' </summary>
    ''' <param name="stringValue">String value.</param>
    ''' <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
    ''' <returns>Existence of the string value</returns>
    Public Function IsStringDefined(ByVal stringValue As String, ByVal ignoreCase As Boolean) As Boolean
        Return Parse(Me.m_enumType, stringValue, ignoreCase) IsNot Nothing
    End Function

    ''' <summary>
    ''' Gets the underlying enum type for this instance.
    ''' </summary>
    ''' <value></value>
    Public ReadOnly Property EnumType() As Type
        Get
            SyncLock _locker
                Return Me.m_enumType
            End SyncLock

        End Get
    End Property





    ''' <summary>
    ''' Gets a string value for a particular enum value.
    ''' </summary>
    ''' <param name="value">Value.</param>
    ''' <returns>String Value associated via a <see cref="StringValueAttribute"/> attribute, or null if not found.</returns>
    Public Shared Function GetStringValue(ByVal value As [Enum]) As String
        SyncLock _locker
            Dim output As String = Nothing
            Dim type As Type = value.[GetType]()

            If stringValues.ContainsKey(value) Then
                output = DirectCast(stringValues(value), StringValueAttribute).Value
            Else
                'Look for our 'StringValueAttribute' in the field's custom attributes
                Dim fi As FieldInfo = type.GetField(value.ToString())
                Dim attrs As StringValueAttribute() = TryCast(fi.GetCustomAttributes(GetType(StringValueAttribute), False), StringValueAttribute())
                If attrs IsNot Nothing Then
                    If attrs.Length > 0 Then
                        stringValues.Add(value, attrs(0))
                        output = attrs(0).Value
                    End If
                End If
            End If
            Return output
        End SyncLock

    End Function

    ''' <summary>
    ''' Parses the supplied enum and string value to find an associated enum value (case sensitive).
    ''' </summary>
    ''' <param name="type">Type.</param>
    ''' <param name="stringValue">String value.</param>
    ''' <returns>Enum value associated with the string value, or null if not found.</returns>
    Public Shared Function Parse(ByVal type As Type, ByVal stringValue As String) As Object
        SyncLock _locker
            Return Parse(type, stringValue, False)
        End SyncLock

    End Function

    ''' <summary>
    ''' Parses the supplied enum and string value to find an associated enum value.
    ''' </summary>
    ''' <param name="type">Type.</param>
    ''' <param name="stringValue">String value.</param>
    ''' <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
    ''' <returns>Enum value associated with the string value, or null if not found.</returns>
    Public Shared Function Parse(ByVal type As Type, ByVal stringValue As String, ByVal ignoreCase As Boolean) As Object
        SyncLock _locker
            Dim output As Object = Nothing
            Dim enumStringValue As String = Nothing

            If Not type.IsEnum Then
                Throw New ArgumentException([String].Format("Supplied type must be an Enum.  Type was {0}", type))
            End If

            'Look for our string value associated with fields in this enum
            For Each fi As FieldInfo In type.GetFields()
                'Check for our custom attribute
                Dim attrs As StringValueAttribute() = TryCast(fi.GetCustomAttributes(GetType(StringValueAttribute), False), StringValueAttribute())
                If attrs IsNot Nothing Then
                    If attrs.Length > 0 Then
                        enumStringValue = attrs(0).Value
                    End If
                End If

                'Check for equality then select actual enum value.
                If String.Compare(enumStringValue, stringValue, ignoreCase) = 0 Then
                    output = [Enum].Parse(type, fi.Name)
                    Exit For
                End If
            Next

            Return output
        End SyncLock

    End Function

    ''' <summary>
    ''' Return the existence of the given string value within the enum.
    ''' </summary>
    ''' <param name="stringValue">String value.</param>
    ''' <param name="enumType">Type of enum</param>
    ''' <returns>Existence of the string value</returns>
    Public Shared Function IsStringDefined(ByVal enumType As Type, ByVal stringValue As String) As Boolean
        SyncLock _locker
            Return Parse(enumType, stringValue) IsNot Nothing
        End SyncLock

    End Function

    ''' <summary>
    ''' Return the existence of the given string value within the enum.
    ''' </summary>
    ''' <param name="stringValue">String value.</param>
    ''' <param name="enumType">Type of enum</param>
    ''' <param name="ignoreCase">Denotes whether to conduct a case-insensitive match on the supplied string value</param>
    ''' <returns>Existence of the string value</returns>
    Public Shared Function IsStringDefined(ByVal enumType As Type, ByVal stringValue As String, ByVal ignoreCase As Boolean) As Boolean
        SyncLock _locker
            Return Parse(enumType, stringValue, ignoreCase) IsNot Nothing
        End SyncLock

    End Function


End Class




''' <summary>
''' Simple attribute class for storing String Values
''' </summary>
Public Class StringValueAttribute
    Inherits Attribute
    Private ReadOnly m_value As String

    Private Shared ReadOnly _locker As New Object()

    ''' <summary>
    ''' Creates a new <see cref="StringValueAttribute"/> instance.
    ''' </summary>
    ''' <param name="value">Value.</param>
    Public Sub New(ByVal value As String)
        SyncLock _locker
            Me.m_value = value
        End SyncLock

    End Sub

    ''' <summary>
    ''' Gets the value.
    ''' </summary>
    ''' <value></value>
    Public ReadOnly Property Value() As String
        Get
            SyncLock _locker
                Return Me.m_value
            End SyncLock
        End Get
    End Property
End Class



