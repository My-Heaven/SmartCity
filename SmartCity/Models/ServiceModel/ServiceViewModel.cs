namespace SmartCity.Models.ServiceModel
{
    public class ServiceViewModel
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = null!;
        public string? Image { get; set; }
        public string? Description { get; set; }
        public double? PriceOfHour { get; set; }
        public double? EmployeePrice { get; set; }
        public int? SkillId { get; set; }
    }
}
