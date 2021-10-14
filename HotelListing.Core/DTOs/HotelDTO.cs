using System.ComponentModel.DataAnnotations;

namespace HotelListing.Core.DTOs
{
    public class CreateHotelDTO
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string Address { get; set; }

        [Required]
        [Range(1, 5)]
        public double Rating { get; set; }

        //[Required]
        public int CountryId { get; set; }
    }

    public class HotelDTO : CreateHotelDTO
    {
        [Required]
        public int Id { get; set; }

        public CountryDTO Country { get; set; }
    }

    public class UpdateHotelDTO : CreateHotelDTO
    {

    }
}
