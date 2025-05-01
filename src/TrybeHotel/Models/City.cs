namespace TrybeHotel.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class City {
        public int CityId { get; set; }
        public string? Name { get; set; }
        public string? State { get; set; }
        public IEnumerable<Hotel>? Hotels { get; set; }
    }
}