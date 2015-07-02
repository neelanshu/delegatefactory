using Moq;
using NUnit.Framework;
using Refactor.Lib.DataAccess;
using Refactor.Lib.Dtos;
using Refactor.Lib.Factories;
using Refactor.Lib.Model;

namespace Refactor.AllTests.Factories.CustomerResponseDataAccessFactoryTests
{
    [TestFixture]
    public class GetDataAccessResponseByFailoverState
    {
        private IDataAccessResponseFactory<CustomerResponse> _sut;
        private Mock<IFailoverCustomerDataAccess> _mockedFailoverCustomerDataAccess;
        private Mock<ICustomerDataAccess> _mockedCustomerDataAccess;

        [SetUp]
        public void SetUp()
        {
            _mockedCustomerDataAccess = new Mock<ICustomerDataAccess>();
            _mockedFailoverCustomerDataAccess = new Mock<IFailoverCustomerDataAccess>();
            _sut = new CustomerResponseDataAccessFactory(_mockedFailoverCustomerDataAccess.Object,_mockedCustomerDataAccess.Object);
        }

        [Test]
        public void ShouldReturnMethodInstanceFromFailoverCustomerDataAccessForDoFailover()
        {
            var customerResponseForDoFailover = new CustomerResponse() { Customer = new Customer() { Id = 10, Name = "CustomerNameForDoFailover" } };
            var customerResponseForDontFailover = new CustomerResponse() { Customer = new Customer() { Id = 20, Name = "CustomerNameForDontFailover" } };

            _mockedFailoverCustomerDataAccess.Setup(x => x.GetCustomerById(It.IsAny<int>())).Returns(customerResponseForDoFailover);
            _mockedCustomerDataAccess.Setup(x => x.LoadCustomer(It.IsAny<int>())).Returns(customerResponseForDontFailover);

            var delegateObj = _sut.GetDataAccessResponseByFailoverState(FailoverState.DoFailover);
            var returnValue = delegateObj.Invoke(1);

            Assert.AreEqual(customerResponseForDoFailover,returnValue);
        }

        [Test]
        public void ShouldReturnMethodInstanceFromCustomerDataAccessForDontFailover()
        {
            var customerResponseForDoFailover = new CustomerResponse() { Customer = new Customer() { Id = 10, Name = "CustomerNameForDoFailover" } };
            var customerResponseForDontFailover = new CustomerResponse() { Customer = new Customer() { Id = 20, Name = "CustomerNameForDontFailover" } };

            _mockedFailoverCustomerDataAccess.Setup(x => x.GetCustomerById(It.IsAny<int>())).Returns(customerResponseForDoFailover);
            _mockedCustomerDataAccess.Setup(x => x.LoadCustomer(It.IsAny<int>())).Returns(customerResponseForDontFailover);

            var delegateObj = _sut.GetDataAccessResponseByFailoverState(FailoverState.DontFailover);
            var returnValue = delegateObj.Invoke(1);

            Assert.AreEqual(customerResponseForDontFailover, returnValue);
        }
    }
}