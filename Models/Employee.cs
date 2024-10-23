namespace employee_register_with_azure.Models
{
    public class Employee
    {
        public Employee() { }

        public Employee(int id, string name, string address, string ramal, string email, string department, decimal salary, DateTime admissionDate)
        {
            Id = id;
            Name = name;
            Address = address;
            Ramal = ramal;
            Email = email;
            Department = department;
            Salary = salary;
            AdmissionDate = admissionDate;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Ramal { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public decimal Salary { get; set; }
        public DateTimeOffset? AdmissionDate { get; set; }
    }
}
