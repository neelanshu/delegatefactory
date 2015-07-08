using Refactor.Lib.Model;

namespace Refactor.Lib.DataServices
{
    public interface IArchivedDataService
    {
        Customer GetArchivedCustomer(int customerId);
    }
}