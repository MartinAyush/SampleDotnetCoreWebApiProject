using System.ComponentModel.DataAnnotations;

namespace NZWalks.Api.Models.DTO.Regions
{
    public class RegionReq
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "hello world")]
        [MaxLength(100, ErrorMessage = "Name has to be under 100 characters")]
        public string? Name { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(5)]
        public string? Code { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
