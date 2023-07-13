namespace SmartCity.Models.EmployeeModels
{
    public class EmployeeViewModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = null!;
        public string? Image { get; set; }
        public string? FullName { get; set; }
        public string? EmployeeEmail { get; set; }
        public int? Status { get; set; }
        public int? AccountId { get; set; }
        public string? EmployeePhone { get; set; }
    }
}
