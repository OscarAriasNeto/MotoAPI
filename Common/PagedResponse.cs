namespace MotoAPI.Common
{
    /// <summary>
    /// Resposta padrão paginada com links HATEOAS.
    /// </summary>
    /// <typeparam name="T">Tipo da coleção retornada.</typeparam>
    public class PagedResponse<T>
    {
        public IEnumerable<T> Items { get; }

        public PaginationMetadata Meta { get; }

        public IList<LinkDto> Links { get; } = new List<LinkDto>();

        public PagedResponse(IEnumerable<T> items, PaginationMetadata meta)
        {
            Items = items;
            Meta = meta;
        }
    }
}
