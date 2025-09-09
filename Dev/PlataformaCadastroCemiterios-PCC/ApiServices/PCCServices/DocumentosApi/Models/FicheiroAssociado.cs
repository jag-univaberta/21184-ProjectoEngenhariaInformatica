using System;
using System.Collections.Generic;

namespace DocumentosApi.Models;

public partial class FicheiroAssociado
{
    public int RecId { get; set; }

    public string NomeDocumento { get; set; } = null!;

    public string NomeAtribuido { get; set; } = null!; 

    public string DescricaoDocumento { get; set; } = null!;
    public string ObservacaoDocumento { get; set; } = null!;
    

    public string? Datahoraupload { get; set; }

    public string? TipoAssociacao { get; set; }

    public int CodigoAssociacao { get; set; }
}
