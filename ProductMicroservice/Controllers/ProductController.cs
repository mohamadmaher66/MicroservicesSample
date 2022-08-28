using Microsoft.AspNetCore.Mvc;
using ProductMicroservice.DbContext;
using ProductMicroservice.DTOs;
using ProductMicroservice.RabbitMQ;
using System;
using System.Linq;

namespace ProductMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IMessageProducer _messageProducer;

        public ProductController(IMessageProducer messageProducer)
        {
            _messageProducer = messageProducer;
        }

        [HttpGet]
        [Route("GetAllProducts")]
        public IActionResult GetAllProducts()
        {
            return Ok(ProductsData.products);
        }

        [HttpGet]
        [Route("GetProductById/{id}")]
        public IActionResult GetProductById(int id)
        {
            return Ok(ProductsData.products.FirstOrDefault(p => p.Id == id));
        }

        [HttpPost]
        [Route("AddProduct")]
        public IActionResult AddProduct(ProductDTO product)
        {
            ProductsData.products.Add(product);
            _messageProducer.SendMessage(product);

            return Ok("Product added successfully");
        }
    }
}
