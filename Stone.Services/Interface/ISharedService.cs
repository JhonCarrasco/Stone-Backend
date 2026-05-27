using Stone.Dto.Response;
using Stone.Entities;

namespace Stone.Services.Interface
{
    public interface ISharedService
    {
        Task<BaseResponseGeneric<ICollection<Category>>> GetCategoryAsync();
        Task<BaseResponseGeneric<ICollection<Manufacturer>>> GetManufacturerAsync();
        Task<BaseResponseGeneric<ICollection<Bank>>> GetBankAsync();
        Task<BaseResponseGeneric<ICollection<TypeAccount>>> GetTypeAccountAsync();
        Task<BaseResponseGeneric<ICollection<Region>>> GetRegionAsync();
        Task<BaseResponseGeneric<ICollection<Commune>>> GetCommuneAsync();
        Task<BaseResponseGeneric<ICollection<UnitMeasurement>>> GetUnitMeasurementAsync();
    }
}
