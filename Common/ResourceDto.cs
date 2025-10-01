namespace MotoAPI.Common
{
    /// <summary>
    /// Representa um recurso enriquecido com links HATEOAS.
    /// </summary>
    /// <typeparam name="T">Tipo do recurso de domínio.</typeparam>
    public class ResourceDto<T>
    {
        public T Data { get; set; }

        public IList<LinkDto> Links { get; set; } = new List<LinkDto>();

        public ResourceDto(T data)
        {
            Data = data;
        }
    }
}
