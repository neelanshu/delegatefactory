using System.IO;
using Moq;
using NUnit.Framework;
using Refactor.Lib.DataAccess;
using Refactor.Lib.DataServices;
using Refactor.Lib.Dtos;
using Refactor.Lib.Model;
using Refactor.Lib.Services;


/*There are more than 1 test fixtures in this file
 Each testfixture correspond to a method in CustomerService*/

namespace Refactor.AllTests.Services.CustomerServiceTests
{
    [TestFixture]
    public class GetArchivedCustomer
    {
        private ICustomerService _sut;
        private Mock<IArchivedDataService> _mockedArchivedDataService;
        private Mock<ICustomerDataService> _mockedCustomerDataService;

        [SetUp]
        public void SetUp()
        {
            _mockedArchivedDataService = new Mock<IArchivedDataService>();
            _mockedCustomerDataService = new Mock<ICustomerDataService>();
            _sut = new CustomerService(_mockedArchivedDataService.Object, _mockedCustomerDataService.Object);
        }

        [Test]
        public void ShouldCallArchiveDataService()
        {
            _mockedArchivedDataService.Setup((x => x.GetArchivedCustomer(It.IsAny<int>()))).Verifiable();

            _sut.GetArchivedCustomer(10);

            _mockedArchivedDataService.Verify(x => x.GetArchivedCustomer(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void ShouldPassCorrectCustomerIdToArchiveDataService()
        {
            int customerIdFromRequest = 0;

            _mockedArchivedDataService
                .Setup((x => x.GetArchivedCustomer(It.IsAny<int>())))
                .Callback((int cId) => customerIdFromRequest = cId);

            _sut.GetArchivedCustomer(10);

            Assert.AreEqual(10, customerIdFromRequest);
        }

        [Test]
        public void ShouldReturnCustomerThatsComesFromArchiveDataService()
        {
            var expectedCustomer = new Customer() {Id = 1, Name = "abc"};
            _mockedArchivedDataService.Setup((x => x.GetArchivedCustomer(It.IsAny<int>()))).Returns(expectedCustomer);

            var actualCustomer = _sut.GetArchivedCustomer(10);

            Assert.IsNotNull(actualCustomer);
            Assert.AreEqual(expectedCustomer, actualCustomer);
        }
    }

    [TestFixture]
    public class GetCustomer
    {
        private ICustomerService _sut;
        private Mock<IArchivedDataService> _mockedArchivedDataService;
        private Mock<ICustomerDataService> _mockedCustomerDataService;

        [SetUp]
        public void SetUp()
        {
            _mockedArchivedDataService = new Mock<IArchivedDataService>();
            _mockedCustomerDataService = new Mock<ICustomerDataService>();
            _sut = new CustomerService(_mockedArchivedDataService.Object, _mockedCustomerDataService.Object);
        }

        [ExpectedException(typeof (InvalidDataException))]
        [TestCase(-1)]
        [TestCase(0)]
        public void ThrowsExceptionWhenCustomerIdIsLessThan1(int customerIdToPass)
        {
            _sut.GetCustomer(customerIdToPass);
        }


        [Test]
        public void ShouldCallCustomerDataService()
        {
            var expectedCustomerResponse = new CustomerResponse
            {
                Customer = new Customer() {Id = 1, Name = "abc"},
                IsArchived = false
            };

            _mockedCustomerDataService.Setup((x => x.GetCustomerResponse(It.IsAny<int>())))
                .Returns(expectedCustomerResponse)
                .Verifiable();

            _sut.GetCustomer(10);

            _mockedCustomerDataService.Verify(x => x.GetCustomerResponse(It.IsAny<int>()), Times.Once);

        }

        [Test]
        public void ShouldPassTheCorrectCustomerIdToCustomerDataService()
        {
            var expectedCustomerResponse = new CustomerResponse
            {
                Customer = new Customer() {Id = 1, Name = "abc"},
                IsArchived = false
            };

            int customerIdFromRequest=0;

            _mockedCustomerDataService
                .Setup((x => x.GetCustomerResponse(It.IsAny<int>())))
                .Callback((int cId) => customerIdFromRequest = cId)
                .Returns(expectedCustomerResponse);

            _sut.GetCustomer(10);

            Assert.AreEqual(10, customerIdFromRequest);
        }

        [Test]
        public void ShouldReturnSameNotArchivedCustomerThatCustomerDataServiceReturns()
        {
            var expectedCustomerResponse = new CustomerResponse
            {
                Customer = new Customer() {Id = 1, Name = "abc"},
                IsArchived = false
            };

            _mockedCustomerDataService.Setup((x => x.GetCustomerResponse(It.IsAny<int>())))
                .Returns(expectedCustomerResponse);

            var actualCustomer = _sut.GetCustomer(1);

            Assert.IsNotNull(actualCustomer);
            Assert.AreEqual(expectedCustomerResponse.Customer, actualCustomer);
        }

        [Test]
        public void ShouldCallArchiveDataServiceWhenItGetsArchivedCustomerBack()
        {
            var expectedCustomerResponse = new CustomerResponse
            {
                Customer = new Customer() {Id = 1, Name = "abc"},
                IsArchived = true
            };

            _mockedCustomerDataService.Setup((x => x.GetCustomerResponse(It.IsAny<int>())))
                .Returns(expectedCustomerResponse);

            _mockedArchivedDataService.Setup((x => x.GetArchivedCustomer(It.IsAny<int>()))).Verifiable();

            _sut.GetCustomer(10);

            _mockedArchivedDataService.Verify(x => x.GetArchivedCustomer(It.IsAny<int>()), Times.Once);
            //_mockedArchivedDataService.VerifyAll();
        }

        [Test]
        public void ShouldCallArchiveDataServiceWithCorrectCustomerIdWhenItGetsArchivedCustomerBack()
        {
            var expectedCustomerResponse = new CustomerResponse
            {
                Customer = new Customer() {Id = 1, Name = "abc"},
                IsArchived = true
            };

            int customerIdFromRequest=0;

            _mockedCustomerDataService.Setup((x => x.GetCustomerResponse(It.IsAny<int>())))
                .Returns(expectedCustomerResponse);

            _mockedArchivedDataService.Setup((x => x.GetArchivedCustomer(It.IsAny<int>()))).Verifiable();

            _mockedArchivedDataService
                .Setup((x => x.GetArchivedCustomer(It.IsAny<int>())))
                .Callback((int cId) => customerIdFromRequest = cId);
            
            _sut.GetCustomer(10);

            Assert.AreEqual(10, customerIdFromRequest);
        }

        [Test]
        public void ShouldReturnSameArchivedCustomerThatArchivedDataServiceReturns()
        {
            var expectedArchivedCustomer = new Customer() {Id = 1, Name = "abc"};

            var expectedCustomerResponse = new CustomerResponse
            {
                IsArchived = true
            };

            _mockedCustomerDataService.Setup((x => x.GetCustomerResponse(It.IsAny<int>()))).Returns(expectedCustomerResponse);

            _mockedArchivedDataService.Setup((x => x.GetArchivedCustomer(It.IsAny<int>()))).Returns(expectedArchivedCustomer);

            var actualCustomer = _sut.GetCustomer(1);

            Assert.IsNotNull(actualCustomer);
            Assert.AreEqual(expectedArchivedCustomer, actualCustomer);
        }
    }
}
