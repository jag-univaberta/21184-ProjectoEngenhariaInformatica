using System;
using System.Collections.Generic;

namespace CadastroApi.Models;

public partial class Concessionario
{
    public int RecId { get; set; }

    public string Nif { get; set; } = null!;

    public string Nome { get; set; } = null!;

    public string Morada { get; set; } = null!;

    public string Dicofre { get; set; } = null!;

    public string Contacto { get; set; } = null!;
}
