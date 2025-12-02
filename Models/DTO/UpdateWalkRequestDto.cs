using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTO
{
    public class UpdateWalkRequestDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public String Description { get; set; }
        [Range(1, 100)]
        public double LenghtInKm { get; set; }
        
        public string? WalkImageUrl { get; set; }
        [Required]
        public Guid DifficultyId { get; set; }
        [Required]
        public Guid RegionId { get; set; }

    }
}
