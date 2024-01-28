using Microsoft.EntityFrameworkCore;
using NZWalks.Api.Data;
using NZWalks.Api.Models.Domain;
using NZWalks.Api.Models.DTO.Regions;

namespace NZWalks.Api.Repositories
{
    public class SqlRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext _dbContext;

        public SqlRegionRepository(NZWalksDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Region> CreateRegionAsync(Region region)
        {
            await _dbContext.Regions.AddAsync(region);   
            await _dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteRegionAsync(Guid id)
        {
            Region? region =  await _dbContext.Regions.FirstOrDefaultAsync(regions => regions.Id == id);
            if(region != null)
            {
                _dbContext.Regions.Remove(region);
                await _dbContext.SaveChangesAsync();
            }
            return region;
        }

        public async Task<List<Region>> GetAllAsync(string? filterParameter = null, string? filterQuery = null
            , bool orderByAsc = true, string? orderColumn = null
            , int pageNumber = 1, int pageSize = 1000)
        {
            var regions = _dbContext.Regions.AsQueryable();

            // Data Filtering
            if (regions != null && regions.Any() && !string.IsNullOrEmpty(filterQuery) && !string.IsNullOrEmpty(filterParameter))
            {
                if (filterParameter.Equals("name", StringComparison.OrdinalIgnoreCase))
                    regions = regions.Where(region => region.Name.Contains(filterQuery));
            }

            // Sorting
            if(regions != null && regions.Any() && !string.IsNullOrEmpty(orderColumn))
            {
                if (orderColumn.Equals("name", StringComparison.OrdinalIgnoreCase))
                    regions = orderByAsc ? regions.OrderBy(x => x.Name) : regions.OrderByDescending(x => x.Name);
            }

            // Pagination
            int skipRecords = (pageNumber - 1) * pageSize;

            return await regions!.Skip(skipRecords).Take(pageSize).ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Regions.FirstOrDefaultAsync(region => region.Id == id);
        }

        public async Task<Region> UpdateRegionAsync(Guid id, RegionReq requestDto)
        {
            Region? region = await _dbContext.Regions.FirstOrDefaultAsync(region => region.Id == id);
            if(region != null)
            {
                region.Name = requestDto.Name;
                region.Code = requestDto.Code;
                region.RegionImageUrl = requestDto.RegionImageUrl;
                await _dbContext.SaveChangesAsync();
            }
            return region;
        }
    }
}
