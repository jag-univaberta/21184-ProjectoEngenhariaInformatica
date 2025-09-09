using System;
using System.Collections.Generic;

namespace DocumentosApi.Models;

public partial class ConstrucaoConcessionario
{
    public int RecId { get; set; }

    public int ConcessionarioId { get; set; }

    public int ConstrucaoId { get; set; }

    public string DataInicio { get; set; } = null!;

    public string? DataFim { get; set; }
}
