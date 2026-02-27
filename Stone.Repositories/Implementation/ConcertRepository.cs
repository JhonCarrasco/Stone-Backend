using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Stone.Dto.Request;
using Stone.Entities;
using Stone.Entities.Info;
using Stone.Persistence;
using Stone.Repositories.Implements;
using Stone.Repositories.Interface;
using Stone.Repositories.Utils;
using System.Net.Http;

namespace Stone.Repositories
{
    public class ConcertRepository : RepositoryBase<Concert>, IConcertRepository
    {
        private readonly IHttpContextAccessor httpContext;//forma de acceder a los headers

        public ConcertRepository(ApplicationDbContext context, IHttpContextAccessor httpContext) : base(context)
        {
            this.httpContext = httpContext;
        }

        //public override async Task<ICollection<Concert>> GetAsync()
        //{
        //    //eager loading approach
        //    return await context.Set<Concert>()
        //        .Include(x => x.Genre)
        //        .AsNoTracking()
        //        .ToListAsync();
        //}

        public async Task<ICollection<ConcertInfo>> GetAsync(string? title, PaginationDto pagination)
        {
            ////eager loading approach optimizado
            var queryable = context.Set<Concert>()
                .Include(x => x.Genre)
                .Where(x => x.Title.Contains(title ?? string.Empty))
                .IgnoreAutoIncludes()
                .AsNoTracking()
                .Select(x => new ConcertInfo
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    Place = x.Place,
                    UnitPrice = x.UnitPrice,
                    GenreId = x.GenreId,
                    Genre = x.Genre.Name,
                    DateEvent = x.DateEvent.ToShortDateString(),
                    TimeEvent = x.DateEvent.ToShortTimeString(),
                    ImageUrl = x.ImageUrl,
                    TicketsQuantity = x.TicketsQuantity,
                    Finalized = x.Finalized,
                    Active = x.Active ? "Activo" : "Inactivo"
                })
            .AsQueryable();

            await httpContext.HttpContext.InsertarPaginacionHeader(queryable);
            var response = await queryable.OrderBy(x => x.Id).Paginate(pagination).ToListAsync();
            return response;

            ////lazy loading approach
            //return await context.Set<Concert>()
            //    //.Include(x => x.Genre)
            //    .Where(x => x.Title.Contains(title ?? string.Empty))
            //    .AsNoTracking()
            //    .Select(x => new ConcertInfo
            //    {
            //        Id = x.Id,
            //        Title = x.Title,
            //        Description = x.Description,
            //        Place = x.Place,
            //        UnitPrice = x.UnitPrice,
            //        GenreId = x.GenreId,
            //        Genre = x.Genre.Name,
            //        DateEvent = x.DateEvent.ToShortDateString(),
            //        TimeEvent = x.DateEvent.ToShortTimeString(),
            //        ImageUrl = x.ImageUrl,
            //        TicketsQuantity = x.TicketsQuantity,
            //        Finalized = x.Finalized,
            //        Status = x.Status ? "Activo" : "Inactivo"
            //    })
            //    .ToListAsync();

            //var query = context.Set<ConcertInfo>().FromSqlRaw("sp_get_concerts_title {0}", title ?? string.Empty);
            //return await query.ToListAsync();
        }
        
        public async Task FinalizeAsync(int id)
        {
            var entity = await GetAsync(id);
            if(entity is not null)
            {
                entity.Finalized = true;
                await UpdateAsync();
            }
        }
    }
}
