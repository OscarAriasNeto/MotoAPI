using System.ComponentModel.DataAnnotations;

namespace MotoAPI.DTOs
{
    /// <summary>
    /// Requisição para atualização de cliente.
    /// </summary>
    public class UpdateClienteDto
    {
        /// <example>Maria Souza</example>
        [StringLength(150)]
        public string? Nome { get; set; }

        /// <example>maria.souza@email.com</example>
        [EmailAddress]
        [StringLength(200)]
        public string? Email { get; set; }
    }
}
