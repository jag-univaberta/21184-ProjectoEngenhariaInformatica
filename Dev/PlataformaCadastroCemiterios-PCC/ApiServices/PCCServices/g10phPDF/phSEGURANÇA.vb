Namespace Dataframe.Segurança

    Public Enum datPermissões

        ' 01/10/2003    frank   criação (segundo esquema do phPermit)
        ' 01/04/2004    frank   renovação (segundo novo esquema)

        Acesso = 1
        Consulta = 2
        Criação = 4
        Alteração = 8
        Eliminação = 16
        Impressão = 32

    End Enum

    Public Enum datFuncionalidades

        ' criação:      frank   01/10/2003

        Aplicação = 1
        Menu = 2
        Ficha = 3
        Opção = 4
        Area = 5
        Motor = 6
        Modelo = 7
        Director = 8

    End Enum

    Public Enum datFerramentas

        ' criação:      frank   01/10/2003

        Rollbar = 1
        Taskbar = 2
        Pushbutton = 3
        Menu = 4
        Menuitem = 5
        Pushlist = 6
        Droplist = 7
        Combobox = 8

    End Enum

    ' todo: phFRAME: Dataframe: Segurança
    ' (appUID, appUIDGRP, appPERM, appFUN, appFUNOBJ)

End Namespace
