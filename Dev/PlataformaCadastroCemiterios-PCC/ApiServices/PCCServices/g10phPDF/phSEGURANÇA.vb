Namespace Dataframe.Seguran�a

    Public Enum datPermiss�es

        ' 01/10/2003    frank   cria��o (segundo esquema do phPermit)
        ' 01/04/2004    frank   renova��o (segundo novo esquema)

        Acesso = 1
        Consulta = 2
        Cria��o = 4
        Altera��o = 8
        Elimina��o = 16
        Impress�o = 32

    End Enum

    Public Enum datFuncionalidades

        ' cria��o:      frank   01/10/2003

        Aplica��o = 1
        Menu = 2
        Ficha = 3
        Op��o = 4
        Area = 5
        Motor = 6
        Modelo = 7
        Director = 8

    End Enum

    Public Enum datFerramentas

        ' cria��o:      frank   01/10/2003

        Rollbar = 1
        Taskbar = 2
        Pushbutton = 3
        Menu = 4
        Menuitem = 5
        Pushlist = 6
        Droplist = 7
        Combobox = 8

    End Enum

    ' todo: phFRAME: Dataframe: Seguran�a
    ' (appUID, appUIDGRP, appPERM, appFUN, appFUNOBJ)

End Namespace
