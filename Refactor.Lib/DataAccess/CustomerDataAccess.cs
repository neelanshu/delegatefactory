using Refactor.Lib.Dtos;

namespace Refactor.Lib.DataAccess
{
    public interface ICustomerDataAccess
    {
        CustomerResponse LoadCustomer(int customerId);
    }

    public class CustomerDataAccess : ICustomerDataAccess //cant change
    {
        public CustomerResponse LoadCustomer(int customerId)
        {
            // rettrieve customer from 3rd party webservice
            return new CustomerResponse();
        }
    }
}