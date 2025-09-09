using System;
using System.Collections.Generic;

namespace AutenticacaoApi.Models;

public partial class Funcionalidade
{
    public int CodigoFuncionalidade { get; set; }

    public string Designacao { get; set; } = null!;

    public int? CodigoPai { get; set; }
}
