namespace NZWalks.Api.Models.DTO.Regions
{
    public class RegionResp
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
