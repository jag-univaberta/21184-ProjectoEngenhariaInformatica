using System;
using System.Collections.Generic;

namespace DocumentosApi.Models;

public partial class Movimento
{
    public int RecId { get; set; }

    public string DataMovimento { get; set; } = null!;

    public int ResidenteId { get; set; }

    public int TipomovimentoId { get; set; }

    public int ConstrucaodestinoId { get; set; }
}
