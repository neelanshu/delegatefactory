using Refactor.Lib.Dtos;

namespace Refactor.Lib.DataAccess
{
    public interface IFailoverCustomerDataAccess
    {
        CustomerResponse GetCustomerById(int id);
    }

    /*In the notes its explicitly stated that i can not change 
    * "Signature of the FailoverCustomerDataAccess"
    * I am not entirely sure if the statement is deliberately ambiguous 
    * Because I can remove "static" modifier from the method below without changing the Signature 
    * because a signature of a method is its Method name, number of parameters, parameter type and order of parameters */

    public class FailoverCustomerDataAccess : IFailoverCustomerDataAccess //cant change
    {
        public CustomerResponse GetCustomerById(int id)
        {
            // retrieve customer from database
            return new CustomerResponse();
        }
    }
}