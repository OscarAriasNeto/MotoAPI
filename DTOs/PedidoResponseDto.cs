using MotoAPI.Models;

namespace MotoAPI.DTOs
{
    /// <summary>
    /// Representação de retorno de pedido.
    /// </summary>
    public record PedidoResponseDto(
        int Id,
        int ClienteId,
        int MotoId,
        DateTime DataRetirada,
        DateTime? DataDevolucao,
        decimal ValorTotal,
        StatusPedido Status
    )
    {
        public static PedidoResponseDto FromEntity(Pedido pedido) => new(
            pedido.Id,
            pedido.ClienteId,
            pedido.MotoId,
            pedido.DataRetirada,
            pedido.DataDevolucao,
            pedido.ValorTotal,
            pedido.Status);
    }
}
