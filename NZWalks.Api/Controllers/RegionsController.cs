using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.Api.CustomActionFilters;
using NZWalks.Api.Data;
using NZWalks.Api.Models.Domain;
using NZWalks.Api.Models.DTO.Regions;
using NZWalks.Api.Repositories;

namespace NZWalks.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbContext;
        private readonly IRegionRepository _regionRepository;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository)
        {
            this._dbContext = dbContext;
            this._regionRepository = regionRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAllRegionsAsync([FromQuery] string? filterParameter, [FromQuery] string? filterQuery
            , [FromQuery] bool? orderByAsc, [FromQuery] string? orderColumn
            , [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            //List<Region> regionsDomainResp = await _dbContext.Regions.ToListAsync();
            List<Region> regionsDomainResp = await _regionRepository.GetAllAsync(filterParameter!, filterQuery!, orderByAsc ?? true, orderColumn!, pageNumber, pageSize);
            // Got domain models from the database response

            // Convert domain model into the Response DTO
            List<RegionResp> regionDtoResp = new List<RegionResp>();
            foreach (Region regionDomain in regionsDomainResp)
            {
                regionDtoResp.Add(new RegionResp()
                {
                    Code = regionDomain.Code,
                    Id = regionDomain.Id,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl,
                });
            }

            return Ok(regionDtoResp);
        }

        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            Region? region = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            // Convert Domain Response into DTO Response
            if (region == null)
                return NotFound("Region Not Found");

            RegionResp regionDtoResp = new RegionResp()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl,
            };

            return Ok(regionDtoResp);
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateRegionAsync(RegionReq requestDto)
        {
            Region region = new Region()
            {
                Id = Guid.NewGuid(),
                Name = requestDto.Name!,
                RegionImageUrl = requestDto.RegionImageUrl,
                Code = requestDto.Code!
            };

            region = await _regionRepository.CreateRegionAsync(region);

            RegionResp response = new RegionResp()
            {
                Code = region.Code,
                Name = region.Name,
                Id = region.Id,
                RegionImageUrl = region.RegionImageUrl,
            };

            return CreatedAtAction(nameof(GetByIdAsync), new { id = region.Id }, response);
        }

        [HttpPut]
        [ValidateModel]
        [Authorize(Roles = "Reader,Writer")]
        public async Task<IActionResult> UpdateRegionAsync(Guid id, RegionReq requestDto)
        {
            Region regionDomainModel = await _regionRepository.UpdateRegionAsync(id, requestDto);
            if(regionDomainModel == null)
            {
                return NotFound();
            }

            // Map domain modal into response Dto
            RegionResp regionRes = new RegionResp()
            {
                Id = id,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
                Name = regionDomainModel.Name
            };

            return Ok(regionRes);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteRegionAsync([FromRoute] Guid id)
        {
            var regionDomainModel = await _regionRepository.DeleteRegionAsync(id);
            if(regionDomainModel == null)
            {
                return NotFound();
            }

            // Map domain model into dto
            RegionResp regionDto = new RegionResp()
            {
                Id = id,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
                Name = regionDomainModel.Name
            };

            return Ok(regionDto);
        }   
    }
}
