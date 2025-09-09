using System;
using System.Collections.Generic;

namespace CadastroApi.Models;

public partial class Concelho
{
    public string RecId { get; set; }

    public string Nome { get; set; } = "";

    public string Di { get; set; } = ""!;
    public string Co { get; set; } = ""!;
}
