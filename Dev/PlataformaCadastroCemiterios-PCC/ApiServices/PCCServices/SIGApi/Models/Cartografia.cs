
using System.ComponentModel.DataAnnotations;

namespace SIGApi.Models;

public partial class Cartografia
{
    [Key] // Chave Primária
    public int RecId { get; set; }

    public string? Nome { get; set; }

    public int? Parent { get; set; }

    public int? Ordem { get; set; }

    public virtual ICollection<Cartografialayer> Layers { get; set; } = new List<Cartografialayer>();
}
