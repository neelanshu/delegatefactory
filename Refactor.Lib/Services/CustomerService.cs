using Refactor.Lib.DataAccess;
using Refactor.Lib.DataServices;
using Refactor.Lib.Guards;
using Refactor.Lib.Model;

namespace Refactor.Lib.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IArchivedDataService _archivedDataService;
        private readonly ICustomerDataService _customerDataService ;

        public CustomerService(IArchivedDataService archivedDataService, ICustomerDataService customerDataService)
        {
            _archivedDataService = archivedDataService;
            _customerDataService = customerDataService;
        }

        public Customer GetArchivedCustomer(int customerId)
        {
            Requires.ArgumentsToBeGreaterThan1(customerId,"customerId");
            return _archivedDataService.GetArchivedCustomer(customerId);
        }

        public Customer GetCustomer(int customerId)
        {
            Requires.ArgumentsToBeGreaterThan1(customerId, "customerId");
            var customerResponse = _customerDataService.GetCustomerResponse(customerId);
            
            if (customerResponse.IsArchived)
            {
                return GetArchivedCustomer(customerId);
            }

             return customerResponse.Customer;
        }

        //remember to add a an endpoint for the old signature here 
    }
}
