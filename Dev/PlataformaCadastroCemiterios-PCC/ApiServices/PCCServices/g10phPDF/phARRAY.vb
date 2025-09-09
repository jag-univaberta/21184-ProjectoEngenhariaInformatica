Namespace Basframe.Array

    Public Module Array
        Private ReadOnly _locker As New Object()

        Public Function Def(ByVal ParamArray Items() As String) As String()
            SyncLock _locker
                Dim I As Integer
                Dim UmArray() As String

                ReDim UmArray(Items.GetUpperBound(0) - Items.GetLowerBound(0))

                For I = Items.GetLowerBound(0) To Items.GetUpperBound(0)

                    UmArray(I) = Items(I)

                Next

                Return UmArray
            End SyncLock
        End Function

        Public Function Def(ByVal ParamArray Items() As Byte) As Byte()
            SyncLock _locker
                Dim I As Integer
                Dim UmArray() As Byte

                ReDim UmArray(Items.GetUpperBound(0) - Items.GetLowerBound(0))

                For I = Items.GetLowerBound(0) To Items.GetUpperBound(0)

                    UmArray(I) = Items(I)

                Next

                Return UmArray
            End SyncLock
        End Function

        Public Function Def(ByVal ParamArray Items() As Integer) As Integer()
            SyncLock _locker
                Dim I As Integer
                Dim UmArray() As Integer

                ReDim UmArray(Items.GetUpperBound(0) - Items.GetLowerBound(0))

                For I = Items.GetLowerBound(0) To Items.GetUpperBound(0)

                    UmArray(I) = Items(I)

                Next

                Return UmArray
            End SyncLock
        End Function

        Public Function Def(ByVal ParamArray Items() As Object) As Object()
            SyncLock _locker
                Dim I As Integer
                Dim UmArray() As Object

                ReDim UmArray(Items.GetUpperBound(0) - Items.GetLowerBound(0))

                For I = Items.GetLowerBound(0) To Items.GetUpperBound(0)

                    UmArray(I) = Items(I)

                Next

                Return UmArray
            End SyncLock
        End Function

        Public Function SubArray(ByVal Array() As Object, ByVal Inicio As Integer, ByVal Quantos As Integer) As Object()
            SyncLock _locker
                Dim i As Integer
                Dim a As Integer
                Dim buffer() As Object

                ReDim buffer(Quantos - 1)

                For a = Inicio To Inicio + Quantos - 1

                    buffer(i) = Array(a)

                    i = i + 1

                Next

                Return buffer
            End SyncLock
        End Function

        Public Function SubArray(ByVal Array() As Integer, ByVal Inicio As Integer, ByVal Quantos As Integer) As Integer()
            SyncLock _locker
                Dim i As Integer
                Dim a As Integer
                Dim buffer() As Integer

                ReDim buffer(Quantos - 1)

                For a = Inicio To Inicio + Quantos - 1

                    buffer(i) = Array(a)

                    i = i + 1

                Next

                Return buffer
            End SyncLock
        End Function

        Public Function SubArray(ByVal Array() As Byte, ByVal Inicio As Integer, ByVal Quantos As Integer) As Byte()
            SyncLock _locker
                Dim i As Integer
                Dim a As Integer
                Dim buffer() As Byte

                ReDim buffer(Quantos - 1)

                For a = Inicio To Inicio + Quantos - 1

                    buffer(i) = Array(a)

                    i = i + 1

                Next

                Return buffer
            End SyncLock
        End Function


        Public Function Adiciona(ByVal Array() As Object, ByVal Item As Object) As Object()
            SyncLock _locker
                Dim buffer() As Object

                If Array Is Nothing Then
                    ReDim buffer(0)
                Else
                    buffer = CType(Array.Clone, Object())
                    ReDim Preserve buffer(buffer.GetUpperBound(0) + 1)
                End If
                buffer(buffer.GetUpperBound(0)) = Item

                Return buffer
            End SyncLock
        End Function

        Public Function Remove(ByVal Array() As Object, ByVal Posição As Integer) As Object()
            SyncLock _locker
                Dim o As Integer
                Dim buffer() As Object

                buffer = CType(Array.Clone, Object())

                For o = Posição To Array.GetUpperBound(0) - 1
                    buffer(o) = buffer(o + 1)
                Next
                ReDim Preserve buffer(buffer.GetUpperBound(0) - 1)

                Return buffer
            End SyncLock
        End Function

        Public Function Remove(ByVal Array() As Object, ByVal Item As Object) As Object()
            SyncLock _locker
                Dim o As Integer

                o = Array.BinarySearch(Array, Item)
                If o >= 0 Then Return Remove(Array, o)
            End SyncLock
        End Function

    End Module

End Namespace