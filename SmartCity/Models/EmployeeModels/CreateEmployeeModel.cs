namespace SmartCity.Models.EmployeeModels
{
    public class CreateEmployeeModel
    {
        public string EmployeeName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Image { get; set; }
        public string? FullName { get; set; }
        public string? EmployeePhone { get; set; }
        public string? EmployeeEmail { get; set; }
    }
}
