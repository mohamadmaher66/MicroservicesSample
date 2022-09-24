using CustomerMicroservice.DTOs;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using ProductMicroservice.Protos;
using System.Collections.Generic;
using System.Net.Http;

namespace CustomerMicroservice.GRPCServices
{
    public class ProductsGRPC
    {
        private readonly IConfiguration _config;

        public ProductsGRPC(IConfiguration configuration)
        {
            _config = configuration;
        }
        public List<ProductDTO> GetAllProducts()
        {
            List<ProductDTO> products = new List<ProductDTO>();

            var httpHandler = new HttpClientHandler();
            // Return `true` to allow certificates that are untrusted/invalid
            httpHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            var channel = GrpcChannel.ForAddress(_config["GRPC:Host"],
                new GrpcChannelOptions { HttpHandler = httpHandler });
            var client = new GRPCGetAllProducts.GRPCGetAllProductsClient(channel);
            var request = new GetAllProductsRequest();

            var response = client.GetAllProducts(request);

            foreach (var product in response.Products)
            {
                products.Add(new ProductDTO()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Category = product.Category,
                    Price = (decimal) product.Price
                });
            }
            
            return products;
        }
    }
}
