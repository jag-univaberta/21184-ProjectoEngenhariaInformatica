namespace SIGApi.Models
{ 
    public partial class ArvoreCartografia
    {
         

        public int RecId { get; set; }  
        public string Nome { get; set; }
        public int? Parent { get; set; }
        public int Nivel { get; set; }
        public string Layers { get; set; }
        public int? Ordem { get; set; }
    }
  

}
