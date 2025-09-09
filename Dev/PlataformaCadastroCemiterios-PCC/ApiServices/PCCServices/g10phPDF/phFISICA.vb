Namespace Basframe.Fisica

    Public Module Fisica

        Private ReadOnly _locker As New Object()
         
        ' N: 1 Newton representa a energia necessária para mover 1 Kg num 1 M em 1 s.

        Public Function EnergiaCinetica(ByVal MassaKg As Double, ByVal VelocidadeMS As Double) As Double
            SyncLock _locker
                ' 15/1/2004     frank       Energia Cinética (N)
                '                           Ec = 1/2m.v^2
                '                           m (Kg) massa
                '                           v (ms) velocidade

                Return (MassaKg / 2) * (VelocidadeMS ^ 2)
            End SyncLock
        End Function

        Public Function EnergiaCinetica(ByVal MassaKg As Double, ByVal VelocidadeKmH As Long) As Double
            SyncLock _locker
                ' 15/1/2004     frank       Energia Cinética (N)

                Return EnergiaCinetica(MassaKg, ((VelocidadeKmH * 1000) / 3600))
            End SyncLock
        End Function

        Public Function EnergiaPotGrav(ByVal MassaKg As Double, ByVal AlturaM As Double, ByVal AceleraçãoMS2 As Double) As Double
            SyncLock _locker
                ' 15/1/2004     frank       Energia Potencial Gravitica (N)
                '                           Ep = m.g.h
                '                           m (Kg) massa
                '                           g (ms^2) aceleração
                '                           h (m) altura

                Return MassaKg * AceleraçãoMS2 * AlturaM
            End SyncLock
        End Function

        Public Function EnergiaPotGrav(ByVal MassaKg As Double, ByVal AlturaM As Double) As Double
            SyncLock _locker
                ' 15/1/2004     frank       Energia Potencial Gravitica (N)

                Return EnergiaPotGrav(MassaKg, AlturaM, 9.8)
            End SyncLock
        End Function

    End Module

End Namespace
