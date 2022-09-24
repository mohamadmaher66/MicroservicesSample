using CustomerMicroservice.DbContext;
using CustomerMicroservice.DTOs;
using CustomerMicroservice.GRPCServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace CustomerMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _config;
        public CustomerController(IConfiguration configuration)
        {
            _config = configuration;
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

        [HttpGet]
        [Route("GetAllProductsGRPC")]
        public IActionResult GetAllProductsGRPC()
        {
            ProductsGRPC productsGRPC = new ProductsGRPC(_config);
            return Ok(productsGRPC.GetAllProducts());
        }
    }
}
