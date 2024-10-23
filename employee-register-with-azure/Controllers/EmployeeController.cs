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
        // TODO: Chamar o método SaveChanges do _context para salvar no Banco SQL

        var tableClient = GetTableClient();
        var employeeLog = new EmployeeLog(employee, ActionType.Inclusao, employee.Department, Guid.NewGuid().ToString());

        // TODO: Chamar o método UpsertEntity para salvar no Azure Table

        return CreatedAtAction(nameof(ObterPorId), new { id = employee.Id }, employee);
    }

    [HttpPut("{id}")]
    public IActionResult Atualizar(int id, Employee employee)
    {
        var employeeBanco = _context.Employees.Find(id);

        if (employeeBanco == null)
            return NotFound();

        employeeBanco.Name = employee.Name;
        employeeBanco.Address = employee.Address;
        // TODO: As propriedades estão incompletas

        // TODO: Chamar o método de Update do _context.Employees para salvar no Banco SQL
        _context.SaveChanges();

        var tableClient = GetTableClient();
        var employeeLog = new EmployeeLog(employeeBanco, ActionType.Atualizacao, employeeBanco.Department, Guid.NewGuid().ToString());

        // TODO: Chamar o método UpsertEntity para salvar no Azure Table

        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult Deletar(int id)
    {
        var employeeBanco = _context.Employees.Find(id);

        if (employeeBanco == null)
            return NotFound();

        // TODO: Chamar o método de Remove do _context.Employees para salvar no Banco SQL
        _context.SaveChanges();

        var tableClient = GetTableClient();
        var employeeLog = new EmployeeLog(employeeBanco, ActionType.Remocao, employeeBanco.Department, Guid.NewGuid().ToString());

        // TODO: Chamar o método UpsertEntity para salvar no Azure Table

        return NoContent();
    }
}
