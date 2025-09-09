using System;
using System.Collections.Generic;

namespace DocumentosApi.Models;

public partial class Residente
{
    public int RecId { get; set; }

    public string Nome { get; set; } = null!;

    public string? DataNascimento { get; set; }

    public string? DataFalecimento { get; set; }

    public string? DataInumacao { get; set; }
}
