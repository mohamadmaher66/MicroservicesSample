using ProductMicroservice.DTOs;
using System.Collections.Generic;

namespace ProductMicroservice.DbContext
{
    public static class ProductsData
    {
        public static List<ProductDTO> products = new List<ProductDTO>()
        {
            new ProductDTO()
            {
               Id = 1,
               Name = "PC LED Screen",
               Category = "Screens",
               Price = 1000
            },
            new ProductDTO()
            {
               Id = 2,
               Name = "OPPO Reno 5",
               Category = "Mobiles",
               Price = 5000
            },
            new ProductDTO()
            {
               Id = 1,
               Name = "MACbook",
               Category = "Laptops",
               Price = 6000
            }
        };
    }
}
