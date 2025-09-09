Imports mapFRAME.g10phPDF

Namespace Relacoes

    ' uma classe GIS representa uma entidade com determinadas caracteristicas orientadas 
    ' ao trabalho GIS 
    Public Interface ICLASSEGIS

        ' identificação e descrição própria desta classe
        Property Id() As String
        Property Desc() As String

        ' uma classe GIS pode ter interface visivel na area de trabalho
        ReadOnly Property UITem() As Boolean
        ReadOnly Property UILocalização() As System.Drawing.Point
        ReadOnly Property UIDimensão() As System.Drawing.Size

        ' gestor GIS ao qual este classe se associa
        Property GestorGIS() As IGESTORGIS

        ' uma classe GIS podem ser navegável (tem um ".INAVEGAVEL")
        ReadOnly Property Navegador() As Mapframe.INAVEGAVEL
        ' uma classe GIS pode participar em relações de localização 
        Function Localiza(ByVal Area As Mapframe.IEXTENSÃO_, ByVal Mandante As ICLASSEGIS) As Boolean
        Event Localizada(ByVal ClasseGIS As ICLASSEGIS, ByVal Area As Mapframe.IEXTENSÃO_, ByVal Mandante As ICLASSEGIS)

        ' uma classe GIS pode conter informação goegráfica (tem um ".ICARTA")
        ReadOnly Property Conteudo() As Mapframe.ICARTA
        ' uma classe GIS pode participar em relações de conteudo
        Function Edita(ByVal Conteudo As Mapframe.ILAYER, ByVal Activo As Boolean, ByVal Mandante As ICLASSEGIS) As Boolean
        Function Edita(ByVal Conteudo As Mapframe.IGRUPO, ByVal Activo As Boolean, ByVal Mandante As ICLASSEGIS) As Boolean
        Event Editada(ByVal ClasseGIS As ICLASSEGIS, ByVal Conteudo1 As Mapframe.ILAYER, ByVal Conteudo2 As Mapframe.IGRUPO, ByVal Activo As Boolean, ByVal Mandante As ICLASSEGIS)

    End Interface

    ' um gestor GIS controla as classes GIS activas num determinado momento
    Public Interface IGESTORGIS

        Property Classes() As Collection
        Function AdicionaClasse(ByVal ClasseGIS As ICLASSEGIS) As Boolean
        Function RemoveClasse(ByVal ClasseGIS As ICLASSEGIS) As Boolean

        Sub UINavega(ByVal Modo As Mapframe.mapNavModos)

        ReadOnly Property ClasseActual() As ICLASSEGIS

        Function ActivaClasse(ByVal ClasseGIS As ICLASSEGIS) As Boolean
        Function MoveClasse(ByVal ClasseGIS As ICLASSEGIS) As Boolean
        Function DimensionaClasse(ByVal ClasseGIS As ICLASSEGIS) As Boolean

        Event EmCriação(ByVal Gestor As IGESTORGIS, ByVal ClasseGIS As ICLASSEGIS)
        Event EmDestruição(ByVal Gestor As IGESTORGIS, ByVal ClasseGIS As ICLASSEGIS)
        Event EmActivação(ByVal Gestor As IGESTORGIS, ByVal ClasseGIS As ICLASSEGIS)
        Event EmDesactivação(ByVal Gestor As IGESTORGIS, ByVal ClasseGIS As ICLASSEGIS)
        Event EmMovimento(ByVal Gestor As IGESTORGIS, ByVal ClasseGIS As ICLASSEGIS)
        Event EmDimensao(ByVal Gestor As IGESTORGIS, ByVal ClasseGIS As ICLASSEGIS)

        Event EmNavegação(ByVal Gestor As IGESTORGIS, ByVal ClasseGIS As ICLASSEGIS)
        Event EmEdição(ByVal Gestor As IGESTORGIS, ByVal ClasseGIS As ICLASSEGIS)

    End Interface

End Namespace