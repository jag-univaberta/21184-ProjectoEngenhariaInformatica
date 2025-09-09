using System;
using System.Collections.Generic;

namespace AutenticacaoApi.Models;

public partial class Permissoes
{
    public int RecId { get; set; }

    public int UtilizadorId { get; set; }

    public int GrupoId { get; set; }

    public int FuncionalidadeId { get; set; }

    public bool? Permissao { get; set; }
}
