namespace MotoAPI.Common
{
    /// <summary>
    /// Representa um link HATEOAS.
    /// </summary>
    public record LinkDto(string Href, string Rel, string Method);
}
