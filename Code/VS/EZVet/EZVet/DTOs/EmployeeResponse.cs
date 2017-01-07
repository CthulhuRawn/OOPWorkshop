namespace EZVet.DTOs
{
    public class EmployeeResponse
    {
        public bool AlreadyExists { get; set; }

        public Employee Employee { get; set; }
    }
}