using System.ComponentModel.DataAnnotations;

namespace MotoAPI.DTOs
{
    /// <summary>
    /// Requisição para criação de cliente.
    /// </summary>
    public class CreateClienteDto
    {
        /// <example>Maria Silva</example>
        [Required]
        [StringLength(150)]
        public string Nome { get; set; } = string.Empty;

        /// <example>maria.silva@email.com</example>
        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;
    }
}
