using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MotoAPI.Common
{
    /// <summary>
    /// Parâmetros de paginação padrão.
    /// </summary>
    public class PaginationParameters
    {
        private const int MaxPageSize = 50;

        [FromQuery(Name = "page")]
        [Range(1, int.MaxValue, ErrorMessage = "Page deve ser maior ou igual a 1")]
        public int Page { get; set; } = 1;

        private int _pageSize = 10;

        [FromQuery(Name = "pageSize")]
        [Range(1, MaxPageSize, ErrorMessage = "pageSize deve estar entre 1 e 50")]
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}
