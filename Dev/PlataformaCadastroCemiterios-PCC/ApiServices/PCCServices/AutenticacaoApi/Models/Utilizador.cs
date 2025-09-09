using System;
using System.Collections.Generic;

namespace AutenticacaoApi.Models;

public partial class Utilizador
{
    public int RecId { get; set; }

    public string Nome { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string PalavraPasse { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool? Activo { get; set; }
}
