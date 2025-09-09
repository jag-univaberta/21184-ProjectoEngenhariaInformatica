Imports System.Text

Public Class pccString4

    Private _valor As String

    Private Shared ReadOnly _locker As New Object()

    Public Sub New()
        SyncLock _locker
            _valor = ""
        End SyncLock
    End Sub

    Public Sub New(ByVal valor As String)
        SyncLock _locker
            _valor = valor
        End SyncLock
    End Sub

    Public Property Valor() As String

        Get
            SyncLock _locker
                Return _valor
            End SyncLock
        End Get
        Set(ByVal value As String)
            SyncLock _locker
                _valor = value
            End SyncLock
        End Set

    End Property

    Public Function IsDate() As Date
        SyncLock _locker
            If Len(_valor) <> 8 Then Return Nothing

            Try
                Dim d As New Date(Val(Left(_valor, 4)), Val(Mid(_valor, 5, 2)), Val(Mid(_valor, 7, 2)))
                Return d
            Catch ex As Exception
                Return Nothing
            End Try
        End SyncLock
    End Function

    Public Function IsDateTime() As DateTime
        SyncLock _locker
            If Len(_valor) <> 14 Then Return Nothing

            Try
                Dim d As New DateTime(Val(Left(_valor, 4)), Val(Mid(_valor, 5, 2)), Val(Mid(_valor, 7, 2)), Val(Mid(_valor, 9, 2)), Val(Mid(_valor, 11, 2)), Val(Mid(_valor, 13, 2)))
                Return d
            Catch ex As Exception
                Return Nothing
            End Try
        End SyncLock
    End Function

    Public Function Split(ByVal separador As String) As String()
        SyncLock _locker
            Return Valor.Split(separador)
        End SyncLock
    End Function

    Public Function Split(ByVal separador As Char, ByVal inigroup As Char, ByVal fimgroup As Char) As String()
        SyncLock _locker
            Dim grouplevel As Integer = 0
            Dim ret() As String = Nothing
            Dim kret As Integer = 0
            Dim item As New StringBuilder("")
            Dim texto As String = Valor

            Dim i As Integer = 0
            Dim iLength As Integer = texto.Length

            Dim pSep As Integer = 0
            Dim pIniGroup As Integer = 0
            Dim pFimGroup As Integer = 0

            While i < iLength

                pSep = texto.IndexOf(separador, i)
                pIniGroup = texto.IndexOf(inigroup, i)
                pFimGroup = texto.IndexOf(fimgroup, i)

                'If i <> 0 Then
                If pSep = -1 Then pSep = iLength
                If pIniGroup = -1 Then pIniGroup = iLength
                If pFimGroup = -1 Then pFimGroup = iLength
                'End If

                If pSep <= iLength And pSep < pIniGroup And pSep < pFimGroup Then

                    If grouplevel = 0 Then
                        kret += 1
                        ReDim Preserve ret(kret)
                        item.Append(texto.Substring(i, pSep - i))
                        ret(kret - 1) = item.ToString
                        item = New StringBuilder("")
                    Else
                        item.Append(texto.Substring(i, pSep - i + 1))
                    End If

                    i = pSep + 1

                ElseIf pIniGroup <= iLength And pIniGroup < pSep And pIniGroup < pFimGroup Then

                    grouplevel += 1
                    item.Append(texto.Substring(i, pIniGroup - i + 1))
                    i = pIniGroup + 1

                ElseIf pFimGroup <= iLength And pFimGroup < pSep And pFimGroup < pIniGroup Then

                    grouplevel -= 1
                    item.Append(texto.Substring(i, pFimGroup - i + 1))
                    i = pFimGroup + 1
                Else

                    item.Append(texto.Substring(i, pFimGroup - i))
                    i = pFimGroup + 1

                End If

            End While

            If item.Length > 0 Then
                kret += 1
                ReDim Preserve ret(kret - 1)
                ret(kret - 1) = item.ToString
                item = New StringBuilder("")
            End If

            Return ret
        End SyncLock
    End Function

    Public Function Split(ByVal separador As Char, ByVal inifimgroup As Char) As String()
        SyncLock _locker
            Dim grouplevel As Boolean = False
            Dim ret() As String = Nothing
            Dim kret As Integer = 0
            Dim item As New StringBuilder("")
            Dim texto As String = Valor

            Dim i As Integer = 0
            Dim iLength As Integer = texto.Length

            Dim pSep As Integer = 0
            Dim pIniFimGroup As Integer = 0

            While i < iLength

                pSep = texto.IndexOf(separador, i)
                pIniFimGroup = texto.IndexOf(inifimgroup, i)

                'If i <> 0 Then
                If pSep = -1 Then pSep = iLength
                If pIniFimGroup = -1 Then pIniFimGroup = iLength
                'End If

                If pSep <= iLength And pSep < pIniFimGroup Then

                    If Not grouplevel Then
                        kret += 1
                        ReDim Preserve ret(kret)
                        item.Append(texto.Substring(i, pSep - i))
                        ret(kret - 1) = item.ToString
                        item = New StringBuilder("")
                    Else
                        item.Append(texto.Substring(i, pSep - i))
                    End If

                    i = pSep + 1

                ElseIf pIniFimGroup <= iLength And pIniFimGroup < pSep Then

                    grouplevel = Not grouplevel
                    item.Append(texto.Substring(i, pIniFimGroup - i + 1))
                    i = pIniFimGroup + 1

                Else
                    item.Append(texto.Substring(i, pSep - i))
                    i = pSep + 1
                End If

            End While

            If item.Length > 0 Then
                kret += 1
                ReDim Preserve ret(kret - 1)
                ret(kret - 1) = item.ToString
                item = New StringBuilder("")
            End If

            Return ret
        End SyncLock
    End Function

    Public Function IndexOf(ByVal c As Char) As Integer
        SyncLock _locker
            Return Valor.IndexOf(c)
        End SyncLock
    End Function

    Public Function IndexOf(ByVal s As String) As Integer
        SyncLock _locker
            Return Valor.IndexOf(s)
        End SyncLock
    End Function

    Public Function IndexOf(ByVal c As Char, ByVal startIndex As Integer) As Integer
        SyncLock _locker
            Return Valor.IndexOf(c, startIndex)
        End SyncLock
    End Function

    Public Function IndexOf(ByVal s As String, ByVal startIndex As Integer) As Integer
        SyncLock _locker
            Return Valor.IndexOf(s, startIndex)
        End SyncLock
    End Function

    Public Function IndexOf(ByVal ch As Char, ByVal inigroup As Char, ByVal fimgroup As Char) As Integer
        SyncLock _locker
            Return IndexOf(ch, inigroup, fimgroup, 0)
        End SyncLock
    End Function

    Public Function IndexOf(ByVal ch As Char, ByVal inigroup As Char, ByVal fimgroup As Char, ByVal startIndex As Integer) As Integer
        SyncLock _locker
            Dim grouplevel As Integer = 0
            Dim texto As String = Valor

            Dim i As Integer = startIndex
            Dim iLength As Integer = texto.Length

            Dim pChar As Integer = texto.IndexOf(ch, i)

            ' ACELERADOR 
            If pChar = -1 Then Return -1

            Dim pIniGroup As Integer = 0
            Dim pFimGroup As Integer = 0

            While i <= iLength

                pChar = texto.IndexOf(ch, i)
                pIniGroup = texto.IndexOf(inigroup, i)
                pFimGroup = texto.IndexOf(fimgroup, i)

                If i <> 0 Then
                    If pChar = -1 Then pChar = iLength
                    If pIniGroup = -1 Then pIniGroup = iLength
                    If pFimGroup = -1 Then pFimGroup = iLength
                End If

                If pChar <= iLength And pChar <= pIniGroup And pChar <= pFimGroup Then

                    If grouplevel = 0 Then
                        If pChar >= iLength Then
                            Return -1
                        Else
                            Return pChar
                        End If
                    Else
                        If pChar = pFimGroup Then
                            grouplevel -= 1
                        ElseIf pChar = pIniGroup Then
                            grouplevel += 1
                        End If

                        i = pChar + 1

                    End If

                ElseIf pIniGroup <= iLength And pIniGroup < pChar And pIniGroup < pFimGroup Then

                    grouplevel += 1
                    i = pIniGroup + 1

                ElseIf pFimGroup <= iLength And pFimGroup < pChar And pFimGroup < pIniGroup Then

                    grouplevel -= 1
                    i = pFimGroup + 1

                Else

                    i = pFimGroup + 1

                End If

            End While

            Return -1
        End SyncLock
    End Function

    Public Function IndexOf(ByVal ch As Char, ByVal inifimgroup As Char) As Integer
        SyncLock _locker
            Return IndexOf(ch, inifimgroup, 0)
        End SyncLock
    End Function

    Public Function IndexOf(ByVal ch As Char, ByVal inifimgroup As Char, ByVal startIndex As Integer) As Integer
        SyncLock _locker
            Dim grouplevel As Boolean = False
            Dim texto As String = Valor

            Dim i As Integer = startIndex
            Dim iLength As Integer = texto.Length

            Dim pChar As Integer = 0
            Dim pIniFimGroup As Integer = 0

            While i < iLength

                pChar = texto.IndexOf(ch, i)
                pIniFimGroup = texto.IndexOf(inifimgroup, i)

                If i <> 0 Then
                    If pChar = -1 Then pChar = iLength
                    If pIniFimGroup = -1 Then pIniFimGroup = iLength
                End If

                If pChar <= iLength And pChar < pIniFimGroup Then

                    If Not grouplevel Then
                        Return pChar
                    Else
                        If pChar = pIniFimGroup Then
                            grouplevel = Not grouplevel
                        End If
                        i = pChar + 1
                    End If

                ElseIf pIniFimGroup <= iLength And pIniFimGroup < pChar Then

                    grouplevel = Not grouplevel
                    i = pIniFimGroup + 1

                Else

                    i = pIniFimGroup + 1

                End If

            End While

            Return -1
        End SyncLock
    End Function

End Class
