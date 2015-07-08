using Refactor.Lib.DataAccess;
using Refactor.Lib.Dtos;
using Refactor.Lib.Factories;
using Refactor.Lib.Guards;
using Refactor.Lib.Services;

namespace Refactor.Lib.DataServices
{
    public class CustomerDataService : ICustomerDataService
    {
        private readonly IDataAccessResponseFactory<CustomerResponse> _dataAccessCustomerResponseFactory;
        private readonly IFailoverService _failoverService;

        public CustomerDataService(IDataAccessResponseFactory<CustomerResponse> dataAccessCustomerResponseFactory, IFailoverService failoverService)
        {
            _dataAccessCustomerResponseFactory = dataAccessCustomerResponseFactory;
            _failoverService = failoverService;
        }

        public CustomerResponse GetCustomerResponse(int customerId)
        {
            Requires.ArgumentsToBeGreaterThan1(customerId, "customerId");

            var failoverState = _failoverService.CurrentFailoverState; 

            return
                _dataAccessCustomerResponseFactory.GetDataAccessResponseByFailoverState(failoverState)
                    .Invoke(customerId);
        }
    }
}