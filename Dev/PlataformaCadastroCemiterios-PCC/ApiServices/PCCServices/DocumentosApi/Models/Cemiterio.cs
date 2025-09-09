using System;
using System.Collections.Generic;

namespace DocumentosApi.Models;

public partial class Cemiterio
{
    public int RecId { get; set; }

    public string Nome { get; set; } = null!;

    public string Morada { get; set; } = null!;

    public string Dicofre { get; set; } = null!;
}
