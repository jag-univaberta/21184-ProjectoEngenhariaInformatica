using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema; 
using System.ComponentModel.DataAnnotations; 


namespace SIGApi.Models;

public partial class Cartografialayer
{
    [Key] // Chave Primária da Layer
    public int RecId { get; set; } 

    public int Parent { get; set; }

    public string? Layer { get; set; } 
   
}
