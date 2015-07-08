using Refactor.Lib.Model;

namespace Refactor.Lib.DataServices
{
    /// <summary>
    /// As per test guidelines
    /// This class is unchanged
    /// Its only been extracted into an interface for injection purposes 
    /// </summary>
    public class ArchivedDataService : IArchivedDataService 
    {
        public Customer GetArchivedCustomer(int customerId)
        {
            // retrieve customer from archive data service
            return new Customer();
        }
    }
}