namespace MotoAPI.Common
{
    /// <summary>
    /// Metadados complementares para respostas paginadas.
    /// </summary>
    public record PaginationMetadata(int Page, int PageSize, int TotalItems)
    {
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);

        public bool HasPrevious => Page > 1;

        public bool HasNext => Page < TotalPages;
    }
}
