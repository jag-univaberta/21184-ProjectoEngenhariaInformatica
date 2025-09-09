using System;
using System.Collections.Generic;

namespace CadastroApi.Models;

public partial class TipoConstrucao
{
    public int RecId { get; set; }

    public string Designacao { get; set; } = "";

    public string Observacao { get; set; } = ""!;

    public bool Movimentosn { get; set; } = false!;

    
}
