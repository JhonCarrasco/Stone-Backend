using Stone.Dto.Request;

namespace Stone.Repositories.Utils
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDto paginationDto)
        {
            return queryable
                //.Skip((paginationDto.OffSet - 1) * paginationDto.Limit)
                .Skip(paginationDto.OffSet)
                .Take(paginationDto.Limit);
        }
    }
}
