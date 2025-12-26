using homework1;
using homework1.DTO;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private static readonly List<Customer> _customers = new()
    {
        new Customer { Id = 1, FirstName = "Ivan", LastName = "Petrenko", BirthDate = new DateTime(1998, 5, 12) },
        new Customer { Id = 2, FirstName = "Olena", LastName = "Shevchenko", BirthDate = new DateTime(2003, 3, 20) },
        new Customer { Id = 3, FirstName = "Andrii", LastName = "Koval", BirthDate = new DateTime(2001, 11, 5) }
    };

    [HttpGet]
    public ActionResult<IEnumerable<Customer>> GetAllSortedByName()
    {
        var result = _customers
            .OrderBy(c => c.FirstName)
            .ToList();

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public ActionResult<Customer> GetById(int id)
    {
        var customer = _customers.FirstOrDefault(c => c.Id == id);

        if (customer == null)
            return NotFound();

        return Ok(customer);
    }

    [HttpGet("born-after-2000")]
    public ActionResult<IEnumerable<Customer>> GetBornAfter2000()
    {
        var result = _customers
            .Where(c => c.BirthDate.Year > 2000)
            .ToList();

        return Ok(result);
    }

    [HttpPost]
    public ActionResult<Customer> Create(CreateCustomerDto dto)
    {
        var newCustomer = new Customer
        {
            Id = _customers.Max(c => c.Id) + 1,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            BirthDate = dto.BirthDate
        };

        _customers.Add(newCustomer);

        return CreatedAtAction(
            nameof(GetById),
            new { id = newCustomer.Id },
            newCustomer
        );
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var customer = _customers.FirstOrDefault(c => c.Id == id);

        if (customer == null)
            return NotFound();

        _customers.Remove(customer);
        return NoContent();
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, UpdateCustomerDto dto)
    {
        var customer = _customers.FirstOrDefault(c => c.Id == id);

        if (customer == null)
            return NotFound();

        customer.FirstName = dto.FirstName;
        customer.LastName = dto.LastName;
        customer.BirthDate = dto.BirthDate;

        return NoContent();
    }
}
