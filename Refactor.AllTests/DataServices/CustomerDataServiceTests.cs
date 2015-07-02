using System.IO;
using Moq;
using NUnit.Framework;
using Refactor.Lib.DataAccess;
using Refactor.Lib.DataServices;
using Refactor.Lib.Dtos;
using Refactor.Lib.Factories;
using Refactor.Lib.Model;
using Refactor.Lib.Services;

namespace Refactor.AllTests.DataServices.CustomerDataServiceTests
{
    [TestFixture]
    public class GetCustomerResponse
    {
        private ICustomerDataService _sut;
        private Mock<IDataAccessResponseFactory<CustomerResponse>> _mockedResponseFactory;
        private Mock<IFailoverService> _mockedFailoverService;

        [SetUp]
        public void SetUp()
        {
            _mockedFailoverService = new Mock<IFailoverService>();
            _mockedResponseFactory = new Mock<IDataAccessResponseFactory<CustomerResponse>>();
            _sut= new CustomerDataService(_mockedResponseFactory.Object, _mockedFailoverService.Object);
        }

        [ExpectedException(typeof(InvalidDataException))]
        [TestCase(-1)]
        [TestCase(0)]
        public void ThrowsExceptionWhenCustomerIdIsLessThan1(int customerIdToPass)
        {
            _sut.GetCustomerResponse(customerIdToPass);
        }

        [Test]
        public void ShouldCallFailoverServiceToGetFailoverState()
        {
            _mockedResponseFactory.Setup(x => x.GetDataAccessResponseByFailoverState(It.IsAny<FailoverState>()))
                .Returns(FakeMethod);

            _mockedFailoverService.SetupGet(x => x.CurrentFailoverState).Returns(FailoverState.DoFailover).Verifiable();

            _sut.GetCustomerResponse(1);

            _mockedFailoverService.VerifyAll();
        }

        [Test]
        public void ShouldCallResponseFactoryToGetDataAccessMethod()
        {
            _mockedResponseFactory.Setup(x => x.GetDataAccessResponseByFailoverState(It.IsAny<FailoverState>()))
                .Returns(FakeMethod).Verifiable();

            _mockedFailoverService.SetupGet(x => x.CurrentFailoverState).Returns(FailoverState.DoFailover);

            _sut.GetCustomerResponse(1);

            _mockedResponseFactory.Verify(x=>x.GetDataAccessResponseByFailoverState(It.IsAny<FailoverState>()), Times.Once);
        }

        [Test]
        public void ShouldCallResponseFactoryWithTheFailoverStateReturnedByFailoverService()
        {
            var expectedFailoverStateInRequest=FailoverState.DontFailover;
            _mockedFailoverService.SetupGet(x => x.CurrentFailoverState).Returns(FailoverState.DoFailover);


            _mockedResponseFactory.Setup(x => x.GetDataAccessResponseByFailoverState(It.IsAny<FailoverState>()))
                .Returns(FakeMethod)
                .Callback((FailoverState fs)=>expectedFailoverStateInRequest= fs)
                .Verifiable();

            
            _sut.GetCustomerResponse(1);

            Assert.AreEqual(FailoverState.DoFailover,expectedFailoverStateInRequest);
        }


        [Test]
        public void ShouldInvokeMethodWithCorrectCustomerId()
        {
            var wasCalled = false;

            _mockedFailoverService.SetupGet(x => x.CurrentFailoverState).Returns(FailoverState.DoFailover);
            _mockedResponseFactory.Setup(x => x.GetDataAccessResponseByFailoverState(It.IsAny<FailoverState>()))
                .Returns(() => {wasCalled = true; return FakeDataAccess.FakeMethod; } ).Verifiable();

            _sut.GetCustomerResponse(1);

            Assert.IsTrue(wasCalled);
            Assert.AreEqual(1,FakeDataAccess.IdPassedToFakeMethod);
        }

        public CustomerResponse FakeMethod(int id)
        {
            return new CustomerResponse();
        }
    }

    public static class FakeDataAccess
    {
        public static int IdPassedToFakeMethod;

        public static CustomerResponse FakeMethod(int id)
        {
            IdPassedToFakeMethod = id; 
            return new CustomerResponse();
        }
    }
}