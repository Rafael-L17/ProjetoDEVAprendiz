using System.ComponentModel.DataAnnotations;

namespace ProjetoMyrp.Models
{
    public class Produto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Preco { get; set; }

        [Range(0, int.MaxValue)]
        public int QuantidadeEmEstoque { get; set; } // Quantidade em estoque
    }
}
