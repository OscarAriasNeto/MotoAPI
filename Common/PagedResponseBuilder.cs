using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace MotoAPI.Common
{
    public static class PagedResponseBuilder
    {
        public static PagedResponse<T> Build<T>(IEnumerable<T> items, int totalItems, PaginationParameters parameters, LinkGenerator linkGenerator, HttpContext httpContext, string routeName)
        {
            var meta = new PaginationMetadata(parameters.Page, parameters.PageSize, totalItems);
            var response = new PagedResponse<T>(items, meta);

            var selfLink = linkGenerator.GetUriByName(httpContext, routeName, new
            {
                page = parameters.Page,
                pageSize = parameters.PageSize
            });

            if (!string.IsNullOrWhiteSpace(selfLink))
            {
                response.Links.Add(new LinkDto(selfLink, "self", HttpMethods.Get));
            }

            if (meta.HasPrevious)
            {
                var previousLink = linkGenerator.GetUriByName(httpContext, routeName, new
                {
                    page = parameters.Page - 1,
                    pageSize = parameters.PageSize
                });

                if (!string.IsNullOrWhiteSpace(previousLink))
                {
                    response.Links.Add(new LinkDto(previousLink, "previous", HttpMethods.Get));
                }
            }

            if (meta.HasNext)
            {
                var nextLink = linkGenerator.GetUriByName(httpContext, routeName, new
                {
                    page = parameters.Page + 1,
                    pageSize = parameters.PageSize
                });

                if (!string.IsNullOrWhiteSpace(nextLink))
                {
                    response.Links.Add(new LinkDto(nextLink, "next", HttpMethods.Get));
                }
            }

            return response;
        }
    }
}
