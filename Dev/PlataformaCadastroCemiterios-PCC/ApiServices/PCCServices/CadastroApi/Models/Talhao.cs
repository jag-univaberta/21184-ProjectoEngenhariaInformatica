using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace CadastroApi.Models;

public partial class Talhao
{
    public int RecId { get; set; }

    public string Codigo { get; set; } = null!;

    public int CemiterioId { get; set; }

    public Geometry? Geometria { get; set; }
}
