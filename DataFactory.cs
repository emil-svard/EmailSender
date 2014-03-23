using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSenderProgram
{
    public static class DataFactory
    {
        public static IEnumerable<Customer> GetNewCustomers()
        {
            return from customer in DataLayer.ListCustomers()
                   where customer.CreatedDateTime > DateTime.Now.AddDays(-1)
                   select customer;
        }

        public static IEnumerable<Customer> GetCustomersWithNoOrders()
        {
            return from customer in DataLayer.ListCustomers()
                   where !(DataLayer.ListOrders().Any(order => order.CustomerEmail == customer.Email))
                   select customer;
        }
    }
}
