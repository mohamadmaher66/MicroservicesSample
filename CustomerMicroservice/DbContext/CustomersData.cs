using CustomerMicroservice.DTOs;
using System.Collections.Generic;

namespace CustomerMicroservice.DbContext
{
    public class CustomersData
    {
        public static List<CustomerDTO> customers = new List<CustomerDTO>()
        {
            new CustomerDTO()
            {
                Id = 1,
                FirstName = "Mohamad",
                LastName =  "Maher"
            },
            new CustomerDTO()
            {
                Id = 1,
                FirstName = "Ahmad",
                LastName =  "Mohamad"
            },
            new CustomerDTO()
            {
                Id = 1,
                FirstName = "Mahmoud",
                LastName =  "Ahmad"
            }
        };
    }
}
