using Refactor.Lib.Model;

namespace Refactor.Lib.Services
{
    public interface ICustomerService
    {
        Customer GetCustomer(int customerId);
        Customer GetArchivedCustomer(int customerId);
    }
}