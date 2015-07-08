using System;
using Refactor.Lib.DataAccess;
using Refactor.Lib.Dtos;
using Refactor.Lib.Model;

namespace Refactor.Lib.Factories
{
    public class CustomerResponseDataAccessFactory : IDataAccessResponseFactory<CustomerResponse>
    {
        private readonly IFailoverCustomerDataAccess _failoverCustomerDataAccess;
        private readonly ICustomerDataAccess _customerDataAccess;

        public CustomerResponseDataAccessFactory(IFailoverCustomerDataAccess failoverCustomerDataAccess, ICustomerDataAccess customerDataAccess)
        {
            _failoverCustomerDataAccess = failoverCustomerDataAccess;
            _customerDataAccess = customerDataAccess;
        }

        public Func<int, CustomerResponse> GetDataAccessResponseByFailoverState(FailoverState failoverState)
        {
            if (failoverState == FailoverState.DoFailover)
                return _failoverCustomerDataAccess.GetCustomerById;
            return _customerDataAccess.LoadCustomer;
        }
    }
}