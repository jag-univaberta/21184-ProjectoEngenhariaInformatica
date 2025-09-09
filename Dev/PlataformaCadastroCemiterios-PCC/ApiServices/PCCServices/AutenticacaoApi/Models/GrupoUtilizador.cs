using System;
using System.Collections.Generic;

namespace AutenticacaoApi.Models;

public partial class GrupoUtilizador
{
    public int RecId { get; set; }

    public int UtilizadorId { get; set; }

    public int GrupoId { get; set; }
}
