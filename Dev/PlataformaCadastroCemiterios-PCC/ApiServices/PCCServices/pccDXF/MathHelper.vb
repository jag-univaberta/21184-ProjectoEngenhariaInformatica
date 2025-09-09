

Imports System.Collections.Generic


''' <summary>
''' Utility math functions and constants.
''' </summary>
Public Class MathHelper

    Private Shared ReadOnly _locker As New Object()


    ''' <summary>
    ''' Defines the coordinate system reference.
    ''' </summary>
    Public Enum CoordinateSystem
        ''' <summary>
        ''' World coordinates.
        ''' </summary>
        World
        ''' <summary>
        ''' Object coordinates.
        ''' </summary>
        [Object]
    End Enum



    ''' <summary>
    ''' A doble precision number close to zero.
    ''' </summary>
    Public Const EpsilonD As Double = 0.000000001

    ''' <summary>
    ''' A simple precision number close to zero.
    ''' </summary>
    Public Const EpsilonF As Single = 0.00001F

    ''' <summary>
    ''' Constant to transform an angle between degrees and radians.
    ''' </summary>
    Public Const DegToRad As Double = Math.PI / 180.0

    ''' <summary>
    ''' Constant to transform an angle between degrees and radians.
    ''' </summary>
    Public Const RadToDeg As Double = 180.0 / Math.PI

    ''' <summary>
    ''' PI/2 (90 degrees)
    ''' </summary>
    Public Const HalfPI As Double = Math.PI * 0.5F

    ''' <summary>
    ''' 2*PI (360 degrees)
    ''' </summary>
    Public Const TwoPI As Double = 2 * Math.PI

    ''' <summary>
    ''' Checks if a number is close to one.
    ''' </summary>
    ''' <param name="number">Simple precision number.</param>
    ''' <param name="threshold">Tolerance.</param>
    ''' <returns>True if its close to one or false in anyother case.</returns>
    Public Shared Function IsOne(ByVal number As Single, ByVal threshold As Single) As Boolean
        SyncLock _locker
            Return IsZero(number - 1, threshold)
        End SyncLock
    End Function

    ''' <summary>
    ''' Checks if a number is close to one.
    ''' </summary>
    ''' <param name="number">Simple precision number.</param>
    ''' <returns>True if its close to one or false in anyother case.</returns>
    ''' <remarks>By default a tolerance of the constant EPSILON_F will be used.</remarks>
    Public Shared Function IsOne(ByVal number As Single) As Boolean
        SyncLock _locker
            Return IsZero(number - 1)
        End SyncLock
    End Function

    ''' <summary>
    ''' Checks if a number is close to one.
    ''' </summary>
    ''' <param name="number">Double precision number.</param>
    ''' <param name="threshold">Tolerance.</param>
    ''' <returns>True if its close to one or false in anyother case.</returns>
    Public Shared Function IsOne(ByVal number As Double, ByVal threshold As Double) As Boolean
        SyncLock _locker
            Return IsZero(number - 1, threshold)
        End SyncLock
    End Function

    ''' <summary>
    ''' Checks if a number is close to one.
    ''' </summary>
    ''' <param name="number">Double precision number.</param>
    ''' <returns>True if its close to one or false in anyother case.</returns>
    ''' <remarks>By default a tolerance of the constant EPSILON_D will be used.</remarks>
    Public Shared Function IsOne(ByVal number As Double) As Boolean
        SyncLock _locker
            Return IsZero(number - 1)
        End SyncLock
    End Function

    ''' <summary>
    ''' Checks if a number is close to zero.
    ''' </summary>
    ''' <param name="number">Simple precision number.</param>
    ''' <param name="threshold">Tolerance.</param>
    ''' <returns>True if its close to one or false in anyother case.</returns>
    Public Shared Function IsZero(ByVal number As Single, ByVal threshold As Single) As Boolean
        SyncLock _locker
            Return (number >= -threshold AndAlso number <= threshold)
        End SyncLock
    End Function

    ''' <summary>
    ''' Checks if a number is close to zero.
    ''' </summary>
    ''' <param name="number">Simple precision number.</param>
    ''' <returns>True if its close to one or false in anyother case.</returns>
    ''' <remarks>By default a tolerance of the constant EPSILON_F will be used.</remarks>
    Public Shared Function IsZero(ByVal number As Single) As Boolean
        SyncLock _locker
            Return IsZero(number, EpsilonF)
        End SyncLock
    End Function

    ''' <summary>
    ''' Checks if a number is close to zero.
    ''' </summary>
    ''' <param name="number">Double precision number.</param>
    ''' <param name="threshold">Tolerance.</param>
    ''' <returns>True if its close to one or false in anyother case.</returns>
    Public Shared Function IsZero(ByVal number As Double, ByVal threshold As Double) As Boolean
        SyncLock _locker
            Return number >= -threshold AndAlso number <= threshold
        End SyncLock
    End Function

    ''' <summary>
    ''' Checks if a number is close to zero.
    ''' </summary>
    ''' <param name="number">Double precision number.</param>
    ''' <returns>True if its close to one or false in anyother case.</returns>
    ''' <remarks>By default a tolerance of the constant EPSILON_D will be used.</remarks>
    Public Shared Function IsZero(ByVal number As Double) As Boolean
        SyncLock _locker
            Return IsZero(number, EpsilonD)
        End SyncLock
    End Function

    ''' <summary>
    ''' Transforms a point between coordinate systems.
    ''' </summary>
    ''' <param name="point">Point to transform.</param>
    ''' <param name="zAxis">Object normal vector.</param>
    ''' <param name="from">Point coordinate system.</param>
    ''' <param name="to">Coordinate system of the transformed point.</param>
    ''' <returns>Transormed point.</returns>
    Public Shared Function Transform(ByVal point As Vector3d, ByVal zAxis As Vector3d, ByVal from As CoordinateSystem, ByVal [to] As CoordinateSystem) As Vector3d
        SyncLock _locker
            Dim trans As Matrix3d = ArbitraryAxis(zAxis)
            If from = CoordinateSystem.World AndAlso [to] = CoordinateSystem.[Object] Then
                trans = trans.Traspose()
                Return trans * point
            End If
            If from = CoordinateSystem.[Object] AndAlso [to] = CoordinateSystem.World Then
                Return trans * point
            End If
            Return point
        End SyncLock
    End Function

    ''' <summary>
    ''' Transforms a point list between coordinate systems.
    ''' </summary>
    ''' <param name="points">Points to transform.</param>
    ''' <param name="zAxis">Object normal vector.</param>
    ''' <param name="from">Points coordinate system.</param>
    ''' <param name="to">Coordinate system of the transformed points.</param>
    ''' <returns>Transormed point list.</returns>
    Public Shared Function Transform(ByVal points As IList(Of Vector3d), ByVal zAxis As Vector3d, ByVal from As CoordinateSystem, ByVal [to] As CoordinateSystem) As IList(Of Vector3d)
        SyncLock _locker
            Dim trans As Matrix3d = ArbitraryAxis(zAxis)
            Dim transPoints As List(Of Vector3d)
            If from = CoordinateSystem.World AndAlso [to] = CoordinateSystem.[Object] Then
                transPoints = New List(Of Vector3d)()
                trans = trans.Traspose()
                For Each p As Vector3d In points
                    transPoints.Add(trans * p)
                Next
                Return transPoints
            End If
            If from = CoordinateSystem.[Object] AndAlso [to] = CoordinateSystem.World Then
                transPoints = New List(Of Vector3d)()
                For Each p As Vector3d In points
                    transPoints.Add(trans * p)
                Next
                Return transPoints
            End If
            Return points
        End SyncLock
    End Function

    ''' <summary>
    ''' Gets the rotation matrix from the normal vector (extrusion direction) of an entity.
    ''' </summary>
    ''' <param name="zAxis">Normal vector.</param>
    ''' <returns>Rotation matriz.</returns>
    Public Shared Function ArbitraryAxis(ByVal zAxis As Vector3d) As Matrix3d
        SyncLock _locker
            zAxis.Normalize()
            Dim wY As Vector3d = Vector3d.UnitY
            Dim wZ As Vector3d = Vector3d.UnitZ
            Dim aX As Vector3d

            If (Math.Abs(zAxis.X) < 1 / 64.0) AndAlso (Math.Abs(zAxis.Y) < 1 / 64.0) Then
                aX = Vector3d.CrossProduct(wY, zAxis)
            Else
                aX = Vector3d.CrossProduct(wZ, zAxis)
            End If

            aX.Normalize()

            Dim aY As Vector3d = Vector3d.CrossProduct(zAxis, aX)
            aY.Normalize()

            Return New Matrix3d(aX.X, aY.X, zAxis.X, aX.Y, aY.Y, zAxis.Y, _
             aX.Z, aY.Z, zAxis.Z)
        End SyncLock
    End Function
End Class

