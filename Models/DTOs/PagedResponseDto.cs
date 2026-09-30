using System.Collections.Generic;

namespace ECommerceApi.Models.DTOs
{
    public record PagedResponseDto<T>(
        IEnumerable<T> Data,
        int PageNumber,
        int PageSize,
        int TotalRecords
    );
}
