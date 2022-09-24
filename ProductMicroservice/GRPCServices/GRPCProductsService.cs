using Grpc.Core;
using ProductMicroservice.DbContext;
using ProductMicroservice.DTOs;
using ProductMicroservice.Protos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductMicroservice.GRPCServices
{
    public class GRPCProductsService : Protos.GRPCGetAllProducts.GRPCGetAllProductsBase
    {
        public override Task<ProductResponse> GetAllProducts(GetAllProductsRequest request, ServerCallContext context)
        {
            List<ProductDTO> products = ProductsData.products;
            return Task.FromResult(MapProducts(products));
        }

        private ProductResponse MapProducts(List<ProductDTO> products)
        {
            List<ProductModel> mappedProducts = new List<ProductModel>();
            ProductResponse response = new ProductResponse();

            foreach (var product in products)
            {
                response.Products.Add(new ProductModel()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Category = product.Category,    
                    //Price = (float) product.Price
                });
            }

            return response;
        }
    }
}
