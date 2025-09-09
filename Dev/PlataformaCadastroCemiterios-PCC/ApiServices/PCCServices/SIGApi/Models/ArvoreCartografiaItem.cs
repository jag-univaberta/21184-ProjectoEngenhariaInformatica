namespace SIGApi.Models
{ 
    public partial class ArvoreCartografiaItem
    {
        public long? Id { get; set; }
        public string? Tipo { get; set; }
        public int? RecId { get; set; } 
        public string? Nome { get; set; }
        public int? Parent { get; set; } 
        public int? Ordem { get; set; }
        public int? Nivel { get; set; }
        public string? Layers { get; set; } 
    }
    	 

}
