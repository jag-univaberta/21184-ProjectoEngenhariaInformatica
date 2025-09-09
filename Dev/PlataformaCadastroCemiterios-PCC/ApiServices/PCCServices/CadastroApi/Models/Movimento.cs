using System;
using System.Collections.Generic;
using System.Drawing;

namespace CadastroApi.Models;

public partial class Movimento
{
    public int RecId { get; set; }

    public string DataMovimento { get; set; } = null!;

    public int ResidenteId { get; set; }

    public int TipomovimentoId { get; set; }

    public int ConstrucaodestinoId { get; set; }
}

public partial class InsertMovimento
{
    public string Construcao_id { get; set; }

    public string Tipomovimento_Id { get; set; } = null!;

    public string Data_movimento { get; set; }

    public string Nome { get; set; }

    public string Data_nascimento { get; set; }
    public string Data_falecimento { get; set; }
    public string Data_inumacao { get; set; }
}
public class MovimentoItem
{
    public string RecId { get; set; } 
    public string DataMovimento { get; set; } = null!;
    public string ResidenteId { get; set; }
    public string TipomovimentoId { get; set; }
    public string ConstrucaodestinoId { get; set; }

    public string TipomovimentoNome { get; set; }

    public string ResidenteNome { get; set; }
    public string Residente_Datanascimento { get; set; }
    public string Residente_Datafalecimento { get; set; }
    public string Residente_Datainumacao { get; set; }
}

