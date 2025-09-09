using System;
using System.Collections.Generic;

namespace AutenticacaoApi.Models;

public partial class Grupo
{
    public int RecId { get; set; }

    public string Nome { get; set; } = null!;

    public bool? Activo { get; set; }
}
