using Azure.Data.Tables;
using employee_register_with_azure.Context;
using employee_register_with_azure.Models;
using Microsoft.AspNetCore.Mvc;

namespace employee_register_with_azure.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly HRContext _context;
    private readonly string _connectionString;
    private readonly string _tableName;

    public EmployeeController(HRContext context, IConfiguration configuration)
    {
        _context = context;
        _connectionString = configuration.GetValue<string>("ConnectionStrings:SAConnectionString");
        _tableName = configuration.GetValue<string>("ConnectionStrings:AzureTableName");
    }

    private TableClient GetTableClient()
    {
        var serviceClient = new TableServiceClient(_connectionString);
        var tableClient = serviceClient.GetTableClient(_tableName);

        tableClient.CreateIfNotExists();
        return tableClient;
    }

    [HttpGet("{id}")]
    public IActionResult ObterPorId(int id)
    {
        var employee = _context.Employees.Find(id);

        if (employee == null)
            return NotFound();

        return Ok(employee);
    }

    [HttpPost]
    public IActionResult Criar(Employee employee)
    {
        _context.Employees.Add(employee);
        _context.SaveChanges();

        var tableClient = GetTableClient();
        var employeeLog = new EmployeeLog(employee, ActionType.Inclusao, employee.Department, Guid.NewGuid().ToString());

        tableClient.CreateIfNotExists();

        return CreatedAtAction(nameof(ObterPorId), new { id = employee.Id }, employee);
    }

    [HttpPut("{id}")]
    public IActionResult Atualizar(int id, Employee employee)
    {
        var dbEmployee = _context.Employees.Find(id);

        if (dbEmployee == null)
            return NotFound();

        dbEmployee.Name = employee.Name;
        dbEmployee.Address = employee.Address;
        dbEmployee.Ramal = employee.Ramal;
        dbEmployee.Email = employee.Email;
        dbEmployee.Department = employee.Department;
        dbEmployee.Salary = employee.Salary;
        dbEmployee.AdmissionDate = employee.AdmissionDate;


        _context.Employees.Update(dbEmployee);
        _context.SaveChanges();

        var tableClient = GetTableClient();
        var employeeLog = new EmployeeLog(dbEmployee, ActionType.Atualizacao, dbEmployee.Department, Guid.NewGuid().ToString());

        tableClient.UpsertEntity(employeeLog);

        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult Deletar(int id)
    {
        var dbEmployee = _context.Employees.Find(id);

        if (dbEmployee == null)
            return NotFound();

        _context.Employees.Remove(dbEmployee);
        _context.SaveChanges();

        var tableClient = GetTableClient();
        var employeeLog = new EmployeeLog(dbEmployee, ActionType.Remocao, dbEmployee.Department, Guid.NewGuid().ToString());

        tableClient.UpsertEntity(employeeLog);

        return NoContent();
    }
}
