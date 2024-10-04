using System.ComponentModel.DataAnnotations;

namespace ProjetoMyrp.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [StringLength(14, MinimumLength = 11)]
        public string CPFouCNPJ { get; set; } // CPF (11 dígitos) ou CNPJ (14 dígitos)
    }
}
