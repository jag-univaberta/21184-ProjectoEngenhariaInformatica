Namespace Basframe.Cripto

    Public Module Cripto

        Private ReadOnly _locker As New Object()

        Public Function HexaCode(ByVal FraseString As String) As String
            SyncLock _locker
                Dim Hexas() As String

                Hexas = Strings.TransStringEmHexas(FraseString)

                Return Strings.JuntaHexasEmString(Hexas)
            End SyncLock
        End Function

        Public Function HexaUncode(ByVal FraseHexas As String) As String
            SyncLock _locker
                Dim Hexas() As String

                Hexas = Strings.SeparaHexasDeString(FraseHexas)

                Return Strings.TransHexasEmString(Hexas)
            End SyncLock
        End Function

        Public Function HexaComp(ByVal FraseHexas As String) As String
            SyncLock _locker
                Dim C As Integer
                Dim S As String

                For C = 0 To FraseHexas.Length - 1 Step 2

                    S = FraseHexas.Substring(C, 2)

                    Select Case S

                        Case "20" : S = "G" ' espaço 
                        Case "61" : S = "H" ' a 
                        Case "65" : S = "I" ' e 
                        Case "69" : S = "J" ' i 
                        Case "6F" : S = "K" ' o 
                        Case "75" : S = "L" ' u 
                        Case "6D" : S = "M" ' m 
                        Case "70" : S = "N" ' p 
                        Case "72" : S = "O" ' r 
                        Case "73" : S = "P" ' s 
                        Case "6E" : S = "Q" ' n 
                        Case "71" : S = "R" ' q 
                        Case "6C" : S = "S" ' l 
                        Case "66" : S = "T" ' f 
                        Case "64" : S = "U" ' d 
                        Case "63" : S = "V" ' c 
                        Case "76" : S = "W" ' v 
                        Case "6A" : S = "X" ' j 
                        Case "68" : S = "Y" ' h 
                        Case "7E" : S = "Z" ' ~ 

                    End Select

                    HexaComp = HexaComp & S

                Next
            End SyncLock
        End Function

        Public Function HexaUncomp(ByVal FraseBase36 As String) As String
            SyncLock _locker
                Dim C As Integer
                Dim S As String

                For C = 0 To FraseBase36.Length - 1

                    S = FraseBase36.Substring(C, 1)

                    Select Case S

                        Case "G" : S = "20" ' espaço 
                        Case "H" : S = "61" ' a 
                        Case "I" : S = "65" ' e 
                        Case "J" : S = "69" ' i 
                        Case "K" : S = "6F" ' o 
                        Case "L" : S = "75" ' u 
                        Case "M" : S = "6D" ' m 
                        Case "N" : S = "70" ' p 
                        Case "O" : S = "72" ' r 
                        Case "P" : S = "73" ' s 
                        Case "Q" : S = "6E" ' n 
                        Case "R" : S = "71" ' q 
                        Case "S" : S = "6C" ' l 
                        Case "T" : S = "66" ' f 
                        Case "U" : S = "64" ' d 
                        Case "V" : S = "63" ' c 
                        Case "W" : S = "76" ' v 
                        Case "X" : S = "6A" ' j 
                        Case "Y" : S = "68" ' h 
                        Case "Z" : S = "7E" ' ~ 

                    End Select

                    HexaUncomp = HexaUncomp & S

                Next
            End SyncLock
        End Function

        Public Function BytesVicia(ByVal Bytes() As Byte, ByVal Chave() As Byte, ByVal Inverso As Boolean) As Byte()
            SyncLock _locker
                Dim C As Integer
                Dim B As Integer
                Dim Result() As Byte

                C = Chave.GetLowerBound(0)
                Result = CType(Bytes.Clone, Byte())

                For B = Result.GetLowerBound(0) To Result.GetUpperBound(0)

                    Result(B) = Basframe.ByteAdd(Result(B), Chave(C), Not Inverso)

                    C = C + 1
                    If C > Chave.GetUpperBound(0) Then C = Chave.GetLowerBound(0)

                Next

                Return Result
            End SyncLock
        End Function

        Public Function BytesSlide(ByVal Bytes() As Byte, ByVal Chaves() As Byte) As Byte()
            SyncLock _locker
                Dim Ch As Integer
                Dim Orig As Integer
                Dim Dest As Integer

                Dim Temp As Byte
                Dim Result() As Byte

                Result = CType(Bytes.Clone, Byte())

                Ch = Chaves.GetLowerBound(0) - 1

                For Orig = 0 To Result.GetUpperBound(0)

                    Ch = Ch + 1
                    If Ch > Chaves.GetUpperBound(0) Then Ch = Chaves.GetLowerBound(0)

                    Temp = Result(Orig)

                    Dest = Orig + Chaves(Ch)
                    While Dest > Result.GetUpperBound(0)
                        Dest = Dest - Result.GetUpperBound(0) - 1
                    End While

                    Result(Orig) = Result(Dest)
                    Result(Dest) = Temp

                Next

                Return Result
            End SyncLock
        End Function

        Public Function BytesUnslide(ByVal Bytes() As Byte, ByVal Chaves() As Byte) As Byte()
            SyncLock _locker
                Dim Ch As Integer
                Dim Orig As Integer
                Dim Temp As Byte
                Dim Dest As Integer
                Dim Result() As Byte

                Result = CType(Bytes.Clone, Byte())

                If Bytes.Length > Chaves.Length Then
                    Ch = Bytes.Length - (Chaves.Length * CInt(Math.Floor(Bytes.Length / Chaves.Length)))
                Else
                    Ch = Bytes.Length
                End If

                For Orig = Result.GetUpperBound(0) To 0 Step -1

                    Ch = Ch - 1
                    If Ch < Chaves.GetLowerBound(0) Then Ch = Chaves.GetUpperBound(0)

                    Temp = Result(Orig)

                    Dest = Orig + Chaves(Ch)
                    While Dest > Result.GetUpperBound(0)
                        Dest = Dest - Result.GetUpperBound(0) - 1
                    End While

                    Result(Orig) = Result(Dest)
                    Result(Dest) = Temp

                Next

                Return Result
            End SyncLock
        End Function

        Public Function BytesSlidePush(ByVal Bytes() As Byte, ByVal Chave() As Byte) As Byte()
            SyncLock _locker
                Dim Ch As Integer
                Dim Vez As Integer
                Dim Orig As Integer
                Dim Dest As Integer
                Dim Temp As Byte
                Dim Result() As Byte

                Result = CType(Bytes.Clone, Byte())

                Orig = 0
                Ch = Chave.GetLowerBound(0) - 1

                For Vez = 0 To Result.GetUpperBound(0)

                    Ch = Ch + 1
                    If Ch > Chave.GetUpperBound(0) Then Ch = Chave.GetLowerBound(0)

                    Temp = Result(Orig)

                    Dest = Orig + Chave(Ch)
                    While Dest > Result.GetUpperBound(0)
                        Dest = Dest - Result.GetUpperBound(0) - 1
                    End While

                    Result(Orig) = Result(Dest)
                    Result(Dest) = Temp

                    Orig = Dest + 1
                    While Orig > Result.GetUpperBound(0)
                        Orig = Orig - Result.GetUpperBound(0) - 1
                    End While

                Next

                ReDim Preserve Result(Result.GetUpperBound(0) + 2)
                Result(Result.GetUpperBound(0) - 1) = CByte(Dest)
                Result(Result.GetUpperBound(0)) = CByte(Ch + 1)

                Return Result
            End SyncLock
        End Function

        Public Function BytesUnSlidePush(ByVal Bytes() As Byte, ByVal Chave() As Byte) As Byte()
            SyncLock _locker
                Dim Ch As Integer
                Dim Vez As Integer
                Dim Temp As Byte
                Dim Dest As Integer
                Dim Orig As Integer
                Dim Result() As Byte

                Result = CType(Bytes.Clone, Byte())

                Dest = Result(Result.GetUpperBound(0) - 1)
                Ch = Result(Result.GetUpperBound(0))
                ReDim Preserve Result(Result.GetUpperBound(0) - 2)

                For Vez = 0 To Result.GetUpperBound(0)

                    Ch = Ch - 1
                    If Ch < Chave.GetLowerBound(0) Then Ch = Chave.GetUpperBound(0)

                    Temp = Result(Dest)

                    Orig = Dest - Chave(Ch)
                    While Orig < 0
                        Orig = Result.GetUpperBound(0) + Orig + 1
                    End While

                    Result(Dest) = Result(Orig)
                    Result(Orig) = Temp

                    Dest = Orig - 1
                    While Dest < 0
                        Dest = Result.GetUpperBound(0) + Dest + 1
                    End While

                Next

                Return Result
            End SyncLock
        End Function

        Public Function CharsTraduz(ByVal Chars() As Char, ByVal Alfabeto() As Char) As Char()
            SyncLock _locker
                Dim I As Integer
                Dim C As Char
                Dim A As Byte
                Dim Result() As Char

                Result = CType(Chars.Clone, Char())

                For I = Result.GetLowerBound(0) To Result.GetUpperBound(0)

                    C = Result(I)
                    A = CByte(Asc(C))

                    If A > 64 And A < 91 Then
                        Result(I) = Alfabeto(A - 65)
                    End If

                    If A > 47 And A < 58 Then
                        Result(I) = Alfabeto(26 + A - 48)
                    End If

                Next

                Return Result
            End SyncLock
        End Function

        Public Function CharsRetroTraduz(ByVal Chars() As Char, ByVal Alfabeto() As Char) As Char()
            SyncLock _locker
                Dim I As Integer
                Dim O As Integer
                Dim C As Char
                Dim A As Byte
                Dim Result() As Char

                Result = CType(Chars.Clone, Char())

                For I = Result.GetLowerBound(0) To Result.GetUpperBound(0)

                    C = Result(I)
                    O = Alfabeto.IndexOf(Alfabeto, C)

                    If O < 26 Then
                        A = CByte(65 + O)
                    Else
                        A = CByte(48 + (O - 26))
                    End If

                    Result(I) = Chr(A)

                Next

                Return Result
            End SyncLock
        End Function

        Public Function Alfabeto(ByVal Chaves() As Byte) As Char()
            SyncLock _locker
                Dim C As Integer
                Dim Chars(35) As Char
                Dim Bytes() As Byte

                For C = 65 To 90
                    Chars(C - 65) = Chr(C)
                Next

                For C = 48 To 57
                    Chars(26 + C - 48) = Chr(C)
                Next

                Bytes = Strings.TransCharsEmBytes(Chars)
                Bytes = BytesSlide(Bytes, Chaves)
                Chars = Strings.TransBytesEmChars(Bytes)

                Return Chars
            End SyncLock
        End Function

        Public Function CalculaSPVP(ByVal Bytes() As Byte, ByVal Inicio As Integer, ByVal Quantos As Integer) As Integer
            SyncLock _locker
                Dim Result As Integer
                Dim B As Integer

                For B = Inicio To Inicio + Quantos - 1

                    Result = Result + (Bytes(B) * (B + 1))

                Next

                Return Result
            End SyncLock
        End Function

        Public Function CalculaSPVP(ByVal Bytes() As Byte) As Integer
            SyncLock _locker
                Return CalculaSPVP(Bytes, 0, Bytes.GetLength(0))
            End SyncLock
        End Function

        Public Function CalculaSODEC(ByVal Bytes() As Byte, ByVal Inicio As Integer, ByVal Quantos As Integer) As Integer
            SyncLock _locker
                Dim Result As Integer
                Dim B As Integer

                For B = Inicio + 1 To Inicio + Quantos - 1

                    Result = Result + (CInt(Bytes(B)) - CInt(Bytes(B - 1)))

                Next

                Return Result
            End SyncLock
        End Function

        Public Function CalculaSODEC(ByVal Bytes() As Byte) As Integer
            SyncLock _locker
                Return CalculaSODEC(Bytes, 0, Bytes.GetLength(0))
            End SyncLock
        End Function

        Public Function CalculaSODEC(ByVal Inteiros() As Integer, ByVal Inicio As Integer, ByVal Quantos As Integer) As Integer
            SyncLock _locker
                Dim Result As Integer
                Dim B As Integer

                For B = Inicio + 1 To Inicio + Quantos - 1

                    Result = Result + (CInt(Inteiros(B)) - CInt(Inteiros(B - 1)))

                Next

                Return Result
            End SyncLock
        End Function

        Public Function CalculaSODEC(ByVal Inteiros() As Integer) As Integer
            SyncLock _locker
                Return CalculaSODEC(Inteiros, 0, Inteiros.GetLength(0))
            End SyncLock
        End Function

        Public Function HBBCode(ByVal Frase As String, ByVal Chave As String) As String
            SyncLock _locker
                Dim st As String
                Dim al As String
                Dim sp As String
                Dim SPVP As Integer

                Dim H1() As String
                Dim A1() As Char
                Dim B1() As Byte
                Dim K1(1) As Byte
                Dim C1() As Char

                st = Basframe.Cripto.HexaUncomp(Chave)
                If st.Length Mod 2 <> 0 Then st = st & "F"
                H1 = Basframe.Strings.SeparaHexasDeString(st)
                B1 = Basframe.Strings.TransHexasEmBytes(H1)

                K1(0) = Basframe.ByteAleatorio
                K1(1) = Basframe.ByteAleatorio
                ReDim Preserve B1(B1.GetUpperBound(0) + 2)
                B1(B1.GetUpperBound(0) - 1) = K1(0)
                B1(B1.GetUpperBound(0)) = K1(1)

                A1 = Basframe.Cripto.Alfabeto(B1)

                C1 = Frase.ToCharArray
                B1 = Basframe.Strings.TransCharsEmBytes(C1)
                SPVP = Basframe.Cripto.CalculaSPVP(B1)


                H1 = Basframe.Strings.TransStringEmHexas(Frase)
                st = Basframe.Strings.JuntaHexasEmString(H1)
                st = Basframe.Cripto.HexaComp(st)
                C1 = st.ToCharArray
                C1 = Basframe.Cripto.CharsTraduz(C1, A1)
                st = Basframe.Strings.JuntaCharsEmString(C1)

                al = Strings.JuntaHexasEmString(Strings.TransBytesEmHexas(K1))
                sp = Strings.JuntaHexasEmString(Strings.TransBytesEmHexas(Strings.TransIntEmBytes(SPVP)))

                Return al & st & sp
            End SyncLock
        End Function

        Public Function HBBUncode(ByVal Frase As String, ByVal Chave As String) As String
            SyncLock _locker
                Dim st As String
                Dim al As String
                Dim sp As String
                Dim SPVP As Integer
                Dim SPVPori As Integer

                Dim H1() As String
                Dim A1() As Char
                Dim B1() As Byte
                Dim K1(1) As Byte
                Dim C1() As Char

                al = Frase.Substring(0, 4)
                st = Frase.Substring(4, Frase.Length - 12)
                sp = Frase.Substring(Frase.Length - 8)

                H1 = Strings.SeparaHexasDeString(al)
                K1 = Strings.TransHexasEmBytes(H1)

                H1 = Strings.SeparaHexasDeString(sp)
                B1 = Strings.TransHexasEmBytes(H1)
                SPVP = Strings.TransBytesEmInt(B1)

                al = Basframe.Cripto.HexaUncomp(Chave)
                If al.Length Mod 2 <> 0 Then al = al & "F"
                H1 = Basframe.Strings.SeparaHexasDeString(al)
                B1 = Basframe.Strings.TransHexasEmBytes(H1)

                ReDim Preserve B1(B1.GetUpperBound(0) + 2)
                B1(B1.GetUpperBound(0) - 1) = K1(0)
                B1(B1.GetUpperBound(0)) = K1(1)

                A1 = Basframe.Cripto.Alfabeto(B1)

                C1 = st.ToCharArray
                C1 = Basframe.Cripto.CharsRetroTraduz(C1, A1)
                st = Strings.JuntaCharsEmString(C1)
                st = Basframe.Cripto.HexaUncomp(st)
                H1 = Strings.SeparaHexasDeString(st)
                st = Strings.TransHexasEmString(H1)

                C1 = st.ToCharArray
                B1 = Basframe.Strings.TransCharsEmBytes(C1)
                SPVPori = Basframe.Cripto.CalculaSPVP(B1)

                If SPVPori = SPVP Then
                    Return st
                Else
                    Return Nothing
                End If
            End SyncLock
        End Function

        Public Function Usa(ByVal Cifrador As ICIFRADOR, ByVal Conteudo() As Byte) As Byte()
            SyncLock _locker
                Dim PLoad As Integer
                Dim PSave As Integer
                Dim Destino() As Byte
                Dim Residuo() As Byte
                Dim BFecho As Integer
                Dim BSaida As Integer
                Dim BEntrada As Integer
                Dim Quantidade As Integer

                BFecho = Cifrador.BlocoFecho
                BSaida = Cifrador.BlocoSaida
                BEntrada = Cifrador.BlocoEntrada
                Quantidade = Conteudo.GetLength(0)

                PLoad = 0
                PSave = 0

                While Quantidade - BFecho - PLoad > BEntrada And BEntrada > 0

                    ReDim Preserve Destino(PSave + BSaida - 1)
                    Cifrador.Transforma(Conteudo, PLoad, BEntrada).CopyTo(Destino, PSave)
                    PLoad = PLoad + BEntrada
                    PSave = PSave + BSaida

                End While

                Residuo = Cifrador.TransformaFinal(Conteudo, PLoad, Quantidade - PLoad)
                ReDim Preserve Destino(PSave + Residuo.GetUpperBound(0))
                Residuo.CopyTo(Destino, PSave)

                Return Destino
            End SyncLock
        End Function

        Public Sub Usa(ByVal Cifrador As ICIFRADOR, ByVal Input As System.IO.Stream, ByVal InputInicio As Integer, ByVal InputQuantos As Integer, ByVal Output As System.IO.Stream, ByVal OutputInicio As Integer)
            SyncLock _locker
                Dim Lidos As Integer
                Dim Escritos As Integer
                Dim Bloco() As Byte
                Dim Destino() As Byte
                Dim BFecho As Integer
                Dim BSaida As Integer
                Dim BEntrada As Integer

                BFecho = Cifrador.BlocoFecho
                BSaida = Cifrador.BlocoSaida
                BEntrada = Cifrador.BlocoEntrada
                Lidos = 0
                Escritos = 0
                ReDim Bloco(BEntrada - 1)
                ReDim Destino(BSaida - 1)

                While InputQuantos - BFecho - Lidos > BEntrada And BEntrada > 0

                    Input.Read(Bloco, 0, BEntrada)
                    Destino = Cifrador.Transforma(Bloco, 0, BEntrada)
                    Output.Write(Destino, 0, BSaida)
                    Lidos = Lidos + BEntrada
                    Escritos = Escritos + BSaida

                End While

                Input.Read(Bloco, 0, InputQuantos - Lidos)
                Destino = Cifrador.TransformaFinal(Bloco, 0, InputQuantos - Lidos)
                Output.Write(Destino, 0, Destino.GetLength(0))
            End SyncLock
        End Sub

    End Module

    Public Interface ICIFRADOR

        Sub Reset()

        ReadOnly Property BlocoIndex() As Integer

        ReadOnly Property BlocoEntrada() As Integer
        ReadOnly Property BlocoSaida() As Integer

        ReadOnly Property BlocoFecho() As Integer

        Function Transforma(ByVal Bloco() As Byte) As Byte()
        Function Transforma(ByVal Bloco() As Byte, ByVal Inicio As Integer, ByVal Quantos As Integer) As Byte()

        Function TransformaFinal(ByVal Bloco() As Byte) As Byte()
        Function TransformaFinal(ByVal Bloco() As Byte, ByVal Inicio As Integer, ByVal Quantos As Integer) As Byte()

        ReadOnly Property Sucesso() As Boolean

    End Interface

    Public Interface ICRIPTOSISTEMA

        Sub DirectoReset()
        Function Directo(ByVal Conteudo() As Byte, ByVal Inicio As Integer, ByVal Quantos As Integer) As Byte()
        Function DirectoFinal(ByVal Conteudo() As Byte, ByVal Inicio As Integer, ByVal Quantos As Integer) As Byte()
        Function DirectoSucesso() As Boolean
        Function Codifica(ByVal Conteudo() As Byte) As Byte()
        Function Codifica(ByVal Frase As String) As String
        Sub Codifica(ByVal Input As System.IO.Stream, ByVal InputInicio As Integer, ByVal InputQuantos As Integer, ByVal Output As System.IO.Stream, ByVal OutputInicio As Integer)
        Sub Codifica(ByVal Input As System.IO.Stream, ByVal Output As System.IO.Stream)

        Sub InversoReset()
        Function Inverso(ByVal Conteudo() As Byte, ByVal Inicio As Integer, ByVal Quantos As Integer) As Byte()
        Function InversoFinal(ByVal Conteudo() As Byte, ByVal Inicio As Integer, ByVal Quantos As Integer) As Byte()
        Function InversoSucesso() As Boolean
        Function Descodifica(ByVal Conteudo() As Byte) As Byte()
        Function Descodifica(ByVal Frase As String) As String
        Sub Descodifica(ByVal Input As System.IO.Stream, ByVal InputInicio As Integer, ByVal InputQuantos As Integer, ByVal Output As System.IO.Stream, ByVal OutputInicio As Integer)
        Sub Descodifica(ByVal Input As System.IO.Stream, ByVal Output As System.IO.Stream)

    End Interface

    Public Class Cifra_AsciiAdd
        Implements ICIFRADOR

        Private l_blocoindex As Integer
        Private l_byteindex As Integer
        Private l_sucesso As Boolean
        Private l_sodecs() As Integer

        Public Chave() As Byte
        Private Shared ReadOnly _locker As New Object()

        Public ReadOnly Property BlocoEntrada() As Integer Implements ICIFRADOR.BlocoEntrada
            Get
                SyncLock _locker
                    Return 256
                End SyncLock
            End Get
        End Property

        Public ReadOnly Property BlocoSaida() As Integer Implements ICIFRADOR.BlocoSaida
            Get
                SyncLock _locker
                    Return 260
                End SyncLock
            End Get
        End Property

        Public ReadOnly Property BlocoFecho() As Integer Implements ICIFRADOR.BlocoFecho
            Get
                SyncLock _locker
                    Return 0
                End SyncLock
            End Get
        End Property

        Public ReadOnly Property BlocoIndex() As Integer Implements ICIFRADOR.BlocoIndex
            Get
                SyncLock _locker
                    Return l_blocoindex
                End SyncLock
            End Get
        End Property

        Public Sub Reset() Implements ICIFRADOR.Reset
            SyncLock _locker
                l_blocoindex = 0
                l_byteindex = 0
                l_sucesso = True
                l_sodecs = Nothing
            End SyncLock
        End Sub

        Public Overloads Function Transforma(ByVal Bloco() As Byte, ByVal Inicio As Integer, ByVal Quantos As Integer) As Byte() Implements ICIFRADOR.Transforma
            SyncLock _locker
                l_blocoindex = l_blocoindex + 1

                Dim buffer() As Byte
                Dim b As Integer
                Dim c As Integer
                Dim i As Integer
                Dim sodec As Integer

                ReDim buffer(Quantos - 1)

                For b = Inicio To Inicio + Quantos - 1

                    l_byteindex = l_byteindex + 1

                    buffer(i) = Basframe.ByteAdd(Bloco(b), Chave(c))

                    c = c + 1
                    If c > Chave.GetUpperBound(0) Then c = 0

                    i = i + 1

                Next

                sodec = Cripto.CalculaSODEC(Bloco, Inicio, Quantos)

                ReDim Preserve buffer(buffer.GetUpperBound(0) + 4)
                Strings.TransIntEmBytes(sodec).CopyTo(buffer, buffer.GetLength(0) - 4)

                l_sucesso = l_sucesso And True

                ReDim Preserve l_sodecs(l_blocoindex)
                l_sodecs(l_blocoindex) = sodec

                Return buffer
            End SyncLock
        End Function

        Public Overloads Function Transforma(ByVal Bloco() As Byte) As Byte() Implements ICIFRADOR.Transforma
            SyncLock _locker
                Return Transforma(Bloco, 0, Bloco.GetLength(0))
            End SyncLock
        End Function

        Public Overloads Function TransformaFinal(ByVal Bloco() As Byte, ByVal Inicio As Integer, ByVal Quantos As Integer) As Byte() Implements ICIFRADOR.TransformaFinal
            SyncLock _locker
                Dim buffer() As Byte
                Dim sodec_geral As Integer

                buffer = Me.Transforma(Bloco, Inicio, Quantos)

                ReDim Preserve buffer(buffer.GetUpperBound(0) + 4)

                sodec_geral = Cripto.CalculaSODEC(l_sodecs)
                Strings.TransIntEmBytes(sodec_geral).CopyTo(buffer, buffer.GetLength(0) - 4)

                l_sucesso = l_sucesso And True

                Return buffer
            End SyncLock
        End Function

        Public Overloads Function TransformaFinal(ByVal Bloco() As Byte) As Byte() Implements ICIFRADOR.TransformaFinal
            SyncLock _locker
                Return TransformaFinal(Bloco, 0, Bloco.GetLength(0))
            End SyncLock
        End Function

        Public ReadOnly Property Sucesso() As Boolean Implements ICIFRADOR.Sucesso
            Get
                SyncLock _locker
                    Sucesso = l_sucesso
                End SyncLock
            End Get
        End Property

        Public Sub New()

        End Sub

        Public Sub New(ByVal p_chave() As Byte)
            SyncLock _locker
                Me.Chave = p_chave
            End SyncLock
        End Sub

    End Class

    Public Class Cifra_AsciiSub
        Implements ICIFRADOR

        Private l_blocoindex As Integer
        Private l_byteindex As Integer
        Private l_sucesso As Boolean
        Private l_sodecs() As Integer

        Public Chave() As Byte
        Private Shared ReadOnly _locker As New Object()

        Public ReadOnly Property BlocoEntrada() As Integer Implements ICIFRADOR.BlocoEntrada
            Get
                SyncLock _locker
                    Return 260
                End SyncLock
            End Get
        End Property

        Public ReadOnly Property BlocoSaida() As Integer Implements ICIFRADOR.BlocoSaida
            Get
                SyncLock _locker
                    Return 256
                End SyncLock
            End Get
        End Property

        Public ReadOnly Property BlocoFecho() As Integer Implements ICIFRADOR.BlocoFecho
            Get
                SyncLock _locker
                    Return 4
                End SyncLock
            End Get
        End Property

        Public ReadOnly Property BlocoIndex() As Integer Implements ICIFRADOR.BlocoIndex
            Get
                SyncLock _locker
                    Return l_blocoindex
                End SyncLock
            End Get
        End Property

        Public Sub Reset() Implements ICIFRADOR.Reset
            SyncLock _locker
                l_blocoindex = 0
                l_byteindex = 0
                l_sucesso = True
                l_sodecs = Nothing
            End SyncLock
        End Sub

        Public Overloads Function Transforma(ByVal Bloco() As Byte, ByVal Inicio As Integer, ByVal Quantos As Integer) As Byte() Implements ICIFRADOR.Transforma
            SyncLock _locker
                l_blocoindex = l_blocoindex + 1

                Dim buffer() As Byte
                Dim b As Integer
                Dim c As Integer
                Dim i As Integer
                Dim sodec As Integer
                Dim sodec_ori As Integer

                sodec_ori = Strings.TransBytesEmInt(Array.SubArray(Bloco, Inicio + Quantos - 4, 4))

                ReDim buffer((Quantos - 1) - 4)

                For b = Inicio To Inicio + Quantos - 1 - 4

                    l_byteindex = l_byteindex + 1

                    buffer(i) = ByteAdd(Bloco(b), Chave(c), False)

                    c = c + 1
                    If c > Chave.GetUpperBound(0) Then c = 0

                    i = i + 1

                Next

                sodec = Cripto.CalculaSODEC(buffer, 0, Quantos - 4)

                l_sucesso = l_sucesso And (sodec = sodec_ori)

                ReDim Preserve l_sodecs(l_blocoindex)
                l_sodecs(l_blocoindex) = sodec_ori

                Return buffer
            End SyncLock
        End Function

        Public Overloads Function Transforma(ByVal Bloco() As Byte) As Byte() Implements ICIFRADOR.Transforma
            SyncLock _locker
                Return Transforma(Bloco, 0, Bloco.GetLength(0))
            End SyncLock
        End Function

        Public Overloads Function TransformaFinal(ByVal Bloco() As Byte, ByVal Inicio As Integer, ByVal Quantos As Integer) As Byte() Implements ICIFRADOR.TransformaFinal
            SyncLock _locker
                Dim sodec_ori As Integer
                Dim sodec As Integer
                Dim buffer() As Byte

                sodec_ori = Strings.TransBytesEmInt(Array.SubArray(Bloco, Inicio + Quantos - 4, 4))

                ReDim Preserve Bloco(Bloco.GetUpperBound(0) - 4)
                Quantos = Quantos - 4

                buffer = Me.Transforma(Bloco, Inicio, Quantos)

                sodec = Cripto.CalculaSODEC(l_sodecs)

                l_sucesso = l_sucesso And (sodec = sodec_ori)

                Return buffer
            End SyncLock
        End Function

        Public Overloads Function TransformaFinal(ByVal Bloco() As Byte) As Byte() Implements ICIFRADOR.TransformaFinal
            SyncLock _locker
                Return TransformaFinal(Bloco, 0, Bloco.GetLength(0))
            End SyncLock
        End Function

        Public ReadOnly Property Sucesso() As Boolean Implements ICIFRADOR.Sucesso
            Get
                SyncLock _locker
                    Sucesso = l_sucesso
                End SyncLock
            End Get
        End Property

        Public Sub New()

        End Sub

        Public Sub New(ByVal p_chave() As Byte)
            SyncLock _locker
                Me.Chave = p_chave
            End SyncLock
        End Sub

    End Class

    Public Class CriptoSistemaBase
        Implements ICRIPTOSISTEMA

        Public Codificador As ICIFRADOR
        Public Descodificador As ICIFRADOR

        Private Shared ReadOnly _locker As New Object()

        Public Overridable Sub DirectoReset() Implements ICRIPTOSISTEMA.DirectoReset
            SyncLock _locker
                Codificador.Reset()
            End SyncLock
        End Sub

        Public Overridable Function Directo(ByVal Conteudo() As Byte, ByVal Inicio As Integer, ByVal Quantos As Integer) As Byte() Implements ICRIPTOSISTEMA.Directo
            SyncLock _locker
                Return Codificador.Transforma(Conteudo, Inicio, Quantos)
            End SyncLock
        End Function

        Public Overridable Function DirectoFinal(ByVal Conteudo() As Byte, ByVal Inicio As Integer, ByVal Quantos As Integer) As Byte() Implements ICRIPTOSISTEMA.DirectoFinal
            SyncLock _locker
                Return Codificador.TransformaFinal(Conteudo, Inicio, Quantos)
            End SyncLock
        End Function

        Public Overridable Function DirectoSucesso() As Boolean Implements ICRIPTOSISTEMA.DirectoSucesso
            SyncLock _locker
                Return Codificador.Sucesso
            End SyncLock
        End Function

        Public Overridable Overloads Function Codifica(ByVal Conteudo() As Byte) As Byte() Implements ICRIPTOSISTEMA.Codifica
            SyncLock _locker
                DirectoReset()
                Return Cripto.Usa(Codificador, Conteudo)
            End SyncLock
        End Function

        Public Overridable Overloads Function Codifica(ByVal Frase As String) As String Implements ICRIPTOSISTEMA.Codifica
            SyncLock _locker
                Dim fonte() As Byte
                Dim destino() As Byte

                fonte = Strings.TransStringEmBytes(Frase)
                destino = Me.Codifica(fonte)
                Return Strings.TransBytesEmString(destino)
            End SyncLock
        End Function

        Public Overridable Overloads Sub Codifica(ByVal Input As System.IO.Stream, ByVal InputInicio As Integer, ByVal InputQuantos As Integer, ByVal Output As System.IO.Stream, ByVal OutputInicio As Integer) Implements ICRIPTOSISTEMA.Codifica
            SyncLock _locker
                DirectoReset()
                Cripto.Usa(Me.Codificador, Input, InputInicio, InputQuantos, Output, OutputInicio)
            End SyncLock
        End Sub

        Public Overridable Overloads Sub Codifica(ByVal Input As System.IO.Stream, ByVal Output As System.IO.Stream) Implements ICRIPTOSISTEMA.Codifica
            SyncLock _locker
                Input.Position = 0
                Output.Position = 0
                Me.Codifica(Input, 0, CInt(Input.Length), Output, 0)
            End SyncLock
        End Sub


        Public Overridable Sub InversoReset() Implements ICRIPTOSISTEMA.InversoReset
            SyncLock _locker
                Descodificador.Reset()
            End SyncLock
        End Sub

        Public Overridable Function Inverso(ByVal Conteudo() As Byte, ByVal Inicio As Integer, ByVal Quantos As Integer) As Byte() Implements ICRIPTOSISTEMA.Inverso
            SyncLock _locker
                Return Descodificador.Transforma(Conteudo, Inicio, Quantos)
            End SyncLock
        End Function

        Public Overridable Function InversoFinal(ByVal Conteudo() As Byte, ByVal Inicio As Integer, ByVal Quantos As Integer) As Byte() Implements ICRIPTOSISTEMA.InversoFinal
            SyncLock _locker
                Descodificador.TransformaFinal(Conteudo, Inicio, Quantos)
            End SyncLock
        End Function

        Public Overridable Function InversoSucesso() As Boolean Implements ICRIPTOSISTEMA.InversoSucesso
            SyncLock _locker
                Return Descodificador.Sucesso
            End SyncLock
        End Function

        Public Overridable Overloads Function Descodifica(ByVal Conteudo() As Byte) As Byte() Implements ICRIPTOSISTEMA.Descodifica
            SyncLock _locker
                InversoReset()
                Return Cripto.Usa(Descodificador, Conteudo)
            End SyncLock
        End Function

        Public Overridable Overloads Function Descodifica(ByVal Frase As String) As String Implements ICRIPTOSISTEMA.Descodifica
            SyncLock _locker
                Dim fonte() As Byte
                Dim destino() As Byte

                fonte = Strings.TransStringEmBytes(Frase)
                destino = Me.Descodifica(fonte)
                Return Strings.TransBytesEmString(destino)
            End SyncLock

        End Function

        Public Overridable Overloads Sub Descodifica(ByVal Input As System.IO.Stream, ByVal InputInicio As Integer, ByVal InputQuantos As Integer, ByVal Output As System.IO.Stream, ByVal OutputInicio As Integer) Implements ICRIPTOSISTEMA.Descodifica
            SyncLock _locker
                InversoReset()
                Cripto.Usa(Me.Descodificador, Input, InputInicio, InputQuantos, Output, OutputInicio)
            End SyncLock
        End Sub

        Public Overridable Overloads Sub Descodifica(ByVal Input As System.IO.Stream, ByVal Output As System.IO.Stream) Implements ICRIPTOSISTEMA.Descodifica
            SyncLock _locker
                Input.Position = 0
                Output.Position = 0
                Me.Descodifica(Input, 0, CInt(Input.Length), Output, 0)
            End SyncLock
        End Sub

    End Class

    Public Class CriptoSistema_AsciiAddSub
        Inherits CriptoSistemaBase

        Private Shared ReadOnly _locker As New Object()

        Private Sub Constructor(ByVal p_chave() As Byte)
            SyncLock _locker
                Me.Codificador = New Cifra_AsciiAdd(p_chave)
                Me.Descodificador = New Cifra_AsciiSub(p_chave)
            End SyncLock
        End Sub

        Public Sub New(ByVal p_chave() As Byte)
            SyncLock _locker
                Constructor(p_chave)
            End SyncLock
        End Sub

        Public Sub New(ByVal p_chave As String)
            SyncLock _locker
                Dim a_chave() As Byte

                a_chave = Strings.TransStringEmBytes(p_chave)
                Constructor(a_chave)
            End SyncLock
        End Sub

        Public Sub New(ByVal p_chave As System.IO.Stream)
            SyncLock _locker
                Dim a_chave() As Byte

                ReDim a_chave(CInt(Chave.Length) - 1)
                p_chave.Read(a_chave, 0, CInt(Chave.Length))
                Constructor(a_chave)
            End SyncLock
        End Sub

        Public Property Chave() As Byte()
            Get
                SyncLock _locker
                    Return CType(Me.Codificador, Cifra_AsciiAdd).Chave
                End SyncLock
            End Get
            Set(ByVal Value As Byte())
                SyncLock _locker
                    CType(Me.Codificador, Cifra_AsciiAdd).Chave = Value
                    CType(Me.Descodificador, Cifra_AsciiSub).Chave = Value
                End SyncLock
            End Set
        End Property

    End Class

End Namespace