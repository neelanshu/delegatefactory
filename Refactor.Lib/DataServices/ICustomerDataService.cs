using Refactor.Lib.Dtos;

namespace Refactor.Lib.DataAccess
{
    public interface ICustomerDataService
    {
        CustomerResponse GetCustomerResponse(int customerId);
    }
}