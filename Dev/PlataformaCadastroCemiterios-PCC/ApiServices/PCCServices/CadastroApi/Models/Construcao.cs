using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace CadastroApi.Models;

public partial class Construcao
{
    public int RecId { get; set; }

    public int TipoconstrucaoId { get; set; }

    public string Designacao { get; set; } = null!;

    public int TalhaoId { get; set; }

    public Geometry? Geometria { get; set; }
}
