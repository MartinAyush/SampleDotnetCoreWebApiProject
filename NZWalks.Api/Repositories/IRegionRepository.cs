using NZWalks.Api.Models.Domain;
using NZWalks.Api.Models.DTO.Regions;

namespace NZWalks.Api.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync(string? filterParameter = null, string? filterQuery = null
            , bool orderByAsc = true, string? orderColumn = null
            , int pageNumber = 1, int pageSize = 1000);

        Task<Region?> GetByIdAsync(Guid id);

        Task<Region> CreateRegionAsync(Region region);

        Task<Region> UpdateRegionAsync(Guid id, RegionReq requestDto);

        Task<Region?> DeleteRegionAsync(Guid id);

    }
}
