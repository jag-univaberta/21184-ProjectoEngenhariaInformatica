using System;
using System.Collections.Generic;

namespace CadastroApi.Models;

public partial class TipoMovimento
{
    public int RecId { get; set; }

    public string Designacao { get; set; } = null!;
}
