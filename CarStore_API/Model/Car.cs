namespace CarStore_API.Model
{
    public class Car
    {
        public int Id { get; set; }
        public string Condition { get; set; }
        public string Brand { get; set; }
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
