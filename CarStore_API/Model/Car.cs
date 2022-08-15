using System.ComponentModel.DataAnnotations;

namespace CarStore_API.Model
{
    public class Car
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Condition { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Model { get; set; }


        public string BodyType { get; set; }
        public string Fuel { get; set; }
        public string WheelDrive { get; set; }
        public string UsageType { get; set; }
        public double Price { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
