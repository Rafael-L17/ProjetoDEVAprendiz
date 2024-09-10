using System;
using System.ComponentModel.DataAnnotations;

namespace ProjetoMyrp.Models
{
    public class Venda
    {
        public int Id { get; set; }

        [Required]
        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }

        [Required]
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantidade { get; set; }

        [Required]
        public DateTime DataVenda { get; set; }

        public decimal ValorTotal => Produto != null ? Quantidade * Produto.Preco : 0m;
    }
}
