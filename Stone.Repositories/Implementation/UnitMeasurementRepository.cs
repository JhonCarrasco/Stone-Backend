using Stone.Entities;
using Stone.Persistence;
using Stone.Repositories.Implements;
using Stone.Repositories.Interface;

namespace Stone.Repositories.Implementation
{
    public class UnitMeasurementRepository : RepositoryBase<UnitMeasurement>, IUnitMeasurementRepository
    {
        public UnitMeasurementRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
