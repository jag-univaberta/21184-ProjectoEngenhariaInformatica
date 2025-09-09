Namespace Basframe.Strings

    Public Enum AlinhamentosHorizontais

        Centro = 1
        Direita = 2
        Esquerda = 3

    End Enum

    Public Enum AlinhamentosVerticais

        Superior = 1
        MeiaAltura = 2
        Central = 3
        Base = 4
        Inferior = 5

    End Enum

    Public Structure Alinhamento

        Public Horiontal As AlinhamentosHorizontais
        Public Vertical As AlinhamentosVerticais

    End Structure

    Public Module Strings
        Private ReadOnly _locker As New Object()

        Public Function Corta(ByVal Index As Integer, ByVal Frase As String, ByVal ParamArray Separador() As Char) As String

            Dim Partes() As String

            Partes = Frase.Split(Separador, Index + 1)

            If Index - 1 <= Partes.GetUpperBound(0) Then
                Return Partes(Index - 1)
            Else
                Return ""
            End If

        End Function

        Public Function Corta1ªPalavra(ByVal Frase As String) As String

            Dim E As Integer

            E = Frase.IndexOf(" ")

            If E = -1 Then E = Frase.Length

            Return Frase.Substring(0, E)

        End Function

        Public Function Sem1ªPalavra(ByVal Frase As String) As String

            Dim E As Integer

            E = Frase.IndexOf(" ")

            If E = -1 Then E = -1

            Return Frase.Substring(E + 1)

        End Function

        Public Function Troca(ByVal Frase As String, ByVal Antes As String, ByVal Depois As String) As String

            Dim N As Integer
            Dim Buffer As String

            Buffer = Frase

            N = Buffer.IndexOf(Antes)

            While N >= 0

                Frase = Frase.Remove(N, Antes.Length)
                Frase = Frase.Insert(N, Depois)

                N = Frase.IndexOf(Antes)

            End While

            Return Frase

        End Function

        Public Function Repete(ByVal Frase As String, ByVal QuantasVezes As Integer) As String

            Dim r As Integer

            For r = 1 To QuantasVezes

                Repete = Repete & Frase

            Next

        End Function


        Public Function Onde1º(ByVal Lista() As String, ByVal Inicio As Integer, ByVal Fim As Integer, ByVal Frase As String) As Integer

            Dim N As Integer

            If Inicio = -1 Then Inicio = Lista.GetLowerBound(0)
            If Fim = -1 Then Fim = Lista.GetUpperBound(0)

            For N = Inicio To Fim

                If Lista(N) = Frase Then Exit For

            Next

            If N <= Lista.GetUpperBound(0) Then
                Onde1º = N
            Else
                Onde1º = -1
            End If

        End Function

        Public Function Onde1º(ByVal Frase As String, ByVal Inicio As Integer, ByVal Fim As Integer, ByRef getQual As String, ByRef getQualIndex As Integer, ByVal ParamArray Quais() As String) As Integer

            Dim q As Integer
            Dim i As Integer

            If Inicio = -1 Then Inicio = 0
            If Fim = -1 Then Fim = Frase.Length - 1

            Onde1º = -1

            For q = Inicio To Fim

                i = Frase.IndexOf(Quais(q))

                If i > -1 Then

                    getQualIndex = q
                    getQual = Quais(q)
                    Onde1º = i

                    Exit For

                End If

            Next

        End Function


        Public Function AleatoriaString(ByVal Quantos As Integer) As String

            Dim n As Integer
            Dim m As Integer

            For n = 1 To Quantos
                m = CInt(Rnd() * 35)
                If m < 26 Then
                    AleatoriaString = AleatoriaString & Chr(65 + m)
                Else
                    AleatoriaString = AleatoriaString & Chr(48 + m - 26)
                End If
            Next

        End Function

        Public Function SemAcentos(ByVal Letra As Char) As Char

            Select Case LCase(Letra)

                Case "á"c, "à"c, "ã"c, "â"c, "ä"c
                    Return "a"c

                Case "é"c, "è"c, "ê"c, "ë"c
                    Return "e"c

                Case "í"c, "ì"c, "î"c, "ï"c
                    Return "i"c

                Case "ó"c, "ò"c, "õ"c, "ô"c, "ö"c
                    Return "o"c

                Case "ú"c, "ù"c, "û"c, "ü"c
                    Return "u"c

                Case "ç"c
                    Return "c"c

                Case Else
                    Return Letra

            End Select

        End Function

        Public Function SemAcentos(ByVal Frase As String) As String

            Dim c As Integer

            SemAcentos = ""

            For c = 0 To Frase.Length - 1

                SemAcentos = SemAcentos & SemAcentos(Frase.Chars(c))

            Next

        End Function

        Public Function PrimLetraMaiusc(ByVal Frase As String) As String

            Dim C As Integer

            For C = 0 To Frase.Length - 1

                If Frase.Substring(C, 1) = " " And C < Frase.Length - 1 Or C = 0 Then

                    If C > 0 Then C = C + 1
                    Frase = Frase.Substring(0, C) & Frase.Substring(C, 1).ToUpper & Frase.Substring(C + 1)

                End If

            Next

            Return Frase

        End Function

        Public Function JuntaListaEmString(ByVal Lista() As String, ByVal Inicio As Integer, ByVal Fim As Integer, ByVal Separador As String) As String

            Dim p As Integer

            If Inicio = -1 Then Inicio = Lista.GetLowerBound(0)
            If Fim = -1 Then Fim = Lista.GetUpperBound(0)

            For p = Inicio To Fim

                JuntaListaEmString = JuntaListaEmString + Lista(p) & Separador

            Next

            If Fim > Inicio Then JuntaListaEmString = JuntaListaEmString.Substring(0, JuntaListaEmString.Length - Separador.Length)

        End Function

        Public Function JuntaListaEmString(ByVal Lista() As String, ByVal Inicio As Integer, ByVal ItemFim As String, ByVal Separador As String, ByVal Inclusive As Boolean, ByRef getQuantos As Integer) As String

            getQuantos = 0

            If Inicio = -1 Then Inicio = Lista.GetLowerBound(0)

            For Inicio = Inicio To Lista.GetUpperBound(0)

                If Lista(Inicio) = ItemFim And Not Inclusive Then Exit For

                getQuantos = getQuantos + 1

                JuntaListaEmString = JuntaListaEmString + Lista(Inicio) & Separador

                If Lista(Inicio) = ItemFim Then Exit For

            Next

            JuntaListaEmString = JuntaListaEmString.Substring(0, JuntaListaEmString.Length - Separador.Length)

        End Function

        Public Function JuntaHexasEmString(ByVal Hexas() As String) As String

            Dim B As Integer

            For B = Hexas.GetLowerBound(0) To Hexas.GetUpperBound(0)

                If Hexas(B).Length = 1 Then
                    JuntaHexasEmString = JuntaHexasEmString & "0" & Hexas(B)
                Else
                    JuntaHexasEmString = JuntaHexasEmString & Hexas(B)
                End If

            Next

        End Function

        Public Function JuntaBytesEmString(ByVal Bytes() As Byte, ByVal Inicio As Integer, ByVal Quantos As Integer) As String

            Dim B As Integer

            For B = Inicio To Inicio + Quantos - 1

                JuntaBytesEmString = JuntaBytesEmString & Format(Bytes(B), "000")

            Next

        End Function

        Public Function JuntaBytesEmString(ByVal Bytes() As Byte) As String

            Return JuntaBytesEmString(Bytes, 0, Bytes.GetLength(0))

        End Function

        Public Function JuntaCharsEmString(ByVal Chars() As Char) As String

            Dim B As Integer

            For B = Chars.GetLowerBound(0) To Chars.GetUpperBound(0)

                JuntaCharsEmString = JuntaCharsEmString & Chars(B)

            Next

        End Function

        Public Function SeparaHexasDeString(ByVal Frase As String) As String()

            Dim H As Integer
            Dim Result() As String

            ReDim Result(CInt(Frase.Length / 2) - 1)

            For H = 1 To CInt(Frase.Length / 2)

                Result(H - 1) = Frase.Substring((H - 1) * 2, 2)

            Next

            Return Result

        End Function

        Public Function SeparaBytesDeString(ByVal Frase As String) As Byte()

            Dim H As Integer
            Dim Result() As Byte

            ReDim Result(CInt(Frase.Length / 3) - 1)

            For H = 0 To CInt(Frase.Length / 3)

                Result(H - 1) = CByte(Frase.Substring((H - 1) * 3, 3))

            Next

            Return Result

        End Function

        Public Function SeparaCharsDeString(ByVal Frase As String) As Char()

            Return Frase.ToCharArray

        End Function

        Public Function TransStringEmBytes(ByVal Frase As String) As Byte()

            Dim b As Integer
            Dim buffer() As Byte

            ReDim buffer(Frase.Length - 1)

            For b = 0 To Frase.Length - 1

                buffer(b) = CByte(Asc(Frase.Substring(b, 1)))

            Next

            Return buffer

        End Function

        Public Function TransStringEmHexas(ByVal Frase As String) As String()

            Dim b As Integer
            Dim buffer() As String

            ReDim buffer(Frase.Length - 1)

            For b = 0 To Frase.Length - 1

                buffer(b) = Hex(Asc(Frase.Substring(b, 1)))

            Next

            Return buffer

        End Function

        Public Function TransHexasEmBytes(ByVal Hexas() As String) As Byte()

            Dim H As Integer
            Dim Result() As Byte

            ReDim Result(Hexas.GetUpperBound(0) - Hexas.GetLowerBound(0))

            For H = Hexas.GetLowerBound(0) To Hexas.GetUpperBound(0)

                Result(H) = CByte("&h" & Hexas(H))

            Next

            Return Result

        End Function

        Public Function TransHexasEmString(ByVal Hexas() As String) As String

            Dim H As Integer

            For H = Hexas.GetLowerBound(0) To Hexas.GetUpperBound(0)

                TransHexasEmString = TransHexasEmString & Chr(CByte("&h" & Hexas(H)))

            Next

        End Function

        Public Function TransBytesEmHexas(ByVal Bytes() As Byte) As String()

            Dim B As Integer
            Dim Result() As String

            ReDim Result(Bytes.GetUpperBound(0) - Bytes.GetLowerBound(0))

            For B = Bytes.GetLowerBound(0) To Bytes.GetUpperBound(0)

                Result(B) = Hex(Bytes(B))

            Next

            Return Result

        End Function

        Public Function TransBytesEmString(ByVal Bytes() As Byte) As String

            Return TransBytesEmString(Bytes, 0, Bytes.GetLength(0))

        End Function

        Public Function TransBytesEmString(ByVal Bytes() As Byte, ByVal Inicio As Integer, ByVal Quantos As Integer) As String

            Dim B As Integer
            Dim Result As String

            For B = Inicio To Inicio + Quantos - 1

                'Result = Result & Chr(Bytes(B))
                Result = Result.Concat(Result, Chr(Bytes(B)))

            Next

            Return Result

        End Function

        Public Function TransCharsEmBytes(ByVal Chars() As Char) As Byte()

            Dim H As Integer
            Dim Result() As Byte

            ReDim Result(Chars.GetUpperBound(0) - Chars.GetLowerBound(0))

            For H = Chars.GetLowerBound(0) To Chars.GetUpperBound(0)

                Result(H) = CByte(Asc(Chars(H)))

            Next

            Return Result

        End Function

        Public Function TransBytesEmChars(ByVal Bytes() As Byte) As Char()

            Dim B As Integer
            Dim Result() As Char

            ReDim Result(Bytes.GetUpperBound(0) - Bytes.GetLowerBound(0))

            For B = Bytes.GetLowerBound(0) To Bytes.GetUpperBound(0)

                Result(B) = Chr(Bytes(B))

            Next

            Return Result

        End Function


        Public Function TransIntEmBytes(ByVal Inteiro As Integer) As Byte()

            Dim bruto As Long
            Dim n As Byte
            Dim p As Long
            Dim b As Integer
            Dim Result(3) As Byte

            bruto = Inteiro + 2147483648

            For b = 4 To 1 Step -1

                p = CInt(256 ^ (b - 1))

                n = CByte(Math.Floor(bruto / p))

                Result(b - 1) = n

                bruto = bruto - CLng(n * p)

            Next

            If Inteiro >= 0 Then
                Result(3) = CByte(Result(3) - 128)
            Else
                Result(3) = CByte(Result(3) + 128)
            End If

            Return Result

        End Function

        Public Function TransBytesEmInt(ByVal Bytes() As Byte) As Integer

            Dim bruto As Long
            Dim b As Integer
            Dim p As Long

            If Bytes(3) > 128 Then
                Bytes(3) = CByte(Bytes(3) - 128)
            Else
                Bytes(3) = CByte(Bytes(3) + 128)
            End If

            For b = 1 To 4

                p = CInt(256 ^ (b - 1))

                bruto = bruto + (Bytes(b - 1) * p)

            Next

            Return CInt(bruto - 2147483648)

        End Function


    End Module

End Namespace
