using CustomerMicroservice.DbContext;
using CustomerMicroservice.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CustomerMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        public CustomerController()
        {

        }

        [HttpGet]
        [Route("GetAllCustomers")]
        public IActionResult GetAllCustomers()
        {
            return Ok(CustomersData.customers);
        }

        [HttpGet]
        [Route("GetCustomerById/{id}")]
        public IActionResult GetCustomerById(int id)
        {
            return Ok(CustomersData.customers.FirstOrDefault(p => p.Id == id));
        }

        [HttpPost]
        [Route("AddCustomer")]
        public IActionResult AddCustomer(CustomerDTO customer)
        {
            CustomersData.customers.Add(customer);
            return Ok("Customer added successfully");
        }
    }
}
