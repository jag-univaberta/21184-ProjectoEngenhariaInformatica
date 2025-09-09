Namespace Dataframe

    <AttributeUsage(AttributeTargets.All)> _
    Public Class datCatalogoFlag
        Inherits System.Attribute
    End Class

    Public Structure datDatletDesc

        ' 14/05/2004    Frank       Criação.

        Public CatNamespace As String
        Public CatAlias As String

    End Structure

    Public Interface IDATCAT

        ' 14/05/2004    Frank       Criação.

        ReadOnly Property Quantas() As Integer

        ReadOnly Property ClasseDesc(ByVal Index As Integer) As datDatletDesc

        ReadOnly Property ClasseInstancia(ByVal Index As Integer) As IDATLET

    End Interface

End Namespace
