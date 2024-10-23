using System.Text.Json;
using Azure;
using Azure.Data.Tables;
using Microsoft.WindowsAzure.Storage.Table;

namespace employee_register_with_azure.Models
{
    public class EmployeeLog : Employee, ITableEntity
    {
        public EmployeeLog() { }

        public EmployeeLog(Employee employee, ActionType actionType, string partitionKey, string rowKey)
        {
            base.Id = employee.Id;
            base.Name = employee.Name;
            base.Address = employee.Address;
            base.Ramal = employee.Ramal;
            base.Email = employee.Email;
            base.Department = employee.Department;
            base.Salary = employee.Salary;
            base.AdmissionDate = employee.AdmissionDate;
            ActionType = actionType;
            JSON = JsonSerializer.Serialize(employee);
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        public ActionType ActionType { get; set; }
        public string JSON { get; set; }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
