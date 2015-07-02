using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Refactor.Lib.Model;
using Refactor.Lib.Repositories;
using Refactor.Lib.Services;

namespace Refactor.AllTests.Services.FailoverServiceTests
{
    [TestFixture]
    public class CurrentFailoverState
    {
        private IFailoverService _sut;
        private Mock<IFailoverRepository> _mockedFailoverRepository;
        private Mock<IFailoverAppSettings> _mockedFailoverAppSettings;

        [SetUp]
        public void SetUp()
        {
            _mockedFailoverAppSettings  = new Mock<IFailoverAppSettings>();
            _mockedFailoverRepository = new Mock<IFailoverRepository>();

            _sut = new FailoverService(_mockedFailoverRepository.Object, _mockedFailoverAppSettings.Object);
        }

        [Test]
        public void ShouldBeDoFailoverWhenFailoverModeIsEnabledAndFailoverThresholdExceeded()
        {
            var failedOverEntries = new List<FailoverEntry>();
            for (int i = 1; i <= 2; i++)
            {
               failedOverEntries.Add(new FailoverEntry(){DateTime = DateTime.Now});
            }

            _mockedFailoverAppSettings.SetupGet(x => x.IsFailoverModeEnabled).Returns(true);
            _mockedFailoverAppSettings.SetupGet(x => x.FailoverRequestsThreshold).Returns(1);
            _mockedFailoverAppSettings.SetupGet(x => x.FailoverTimeThresholdInMin).Returns(-1);
            
            _mockedFailoverRepository.Setup(x => x.GetFailOverEntries()).Returns(failedOverEntries);

            var failoverState = _sut.CurrentFailoverState;

            Assert.AreEqual(FailoverState.DoFailover, failoverState);

        }

        [Test]
        public void ShouldBeDonotFailoverWhenFailoverModeIsDisabled()
        {
            var failedOverEntries = new List<FailoverEntry>();
            for (int i = 1; i <= 2; i++)
            {
                failedOverEntries.Add(new FailoverEntry() { DateTime = DateTime.Now });
            }

            _mockedFailoverAppSettings.SetupGet(x => x.IsFailoverModeEnabled).Returns(false);
            _mockedFailoverAppSettings.SetupGet(x => x.FailoverRequestsThreshold).Returns(1);

            _mockedFailoverRepository.Setup(x => x.GetFailOverEntries()).Returns(failedOverEntries);

            var failoverState = _sut.CurrentFailoverState;

            Assert.AreEqual(FailoverState.DontFailover, failoverState);

        }

        [Test]
        public void ShouldBeDonotFailoverWhenFailoverThresholdIsNotExceeded()
        {
            var failedOverEntries = new List<FailoverEntry>();
            for (int i = 1; i <= 2 ; i++)
            {
                failedOverEntries.Add(new FailoverEntry() { DateTime = DateTime.Now });
            }

            _mockedFailoverAppSettings.SetupGet(x => x.IsFailoverModeEnabled).Returns(false);
            _mockedFailoverAppSettings.SetupGet(x => x.FailoverRequestsThreshold).Returns(3);

            _mockedFailoverRepository.Setup(x => x.GetFailOverEntries()).Returns(failedOverEntries);

            var failoverState = _sut.CurrentFailoverState;

            Assert.AreEqual(FailoverState.DontFailover, failoverState);

        }

        [Test]
        public void ShouldCallFailoverRepository()
        {
            var failedOverEntries = new List<FailoverEntry>();
            for (int i = 1; i <= 2; i++)
            {
                failedOverEntries.Add(new FailoverEntry() { DateTime = DateTime.Now });
            }

            _mockedFailoverAppSettings.SetupGet(x => x.IsFailoverModeEnabled).Returns(false);
            _mockedFailoverAppSettings.SetupGet(x => x.FailoverRequestsThreshold).Returns(3);

            _mockedFailoverRepository.Setup(x => x.GetFailOverEntries()).Returns(failedOverEntries).Verifiable();

            var failoverState = _sut.CurrentFailoverState;

            _mockedFailoverRepository.Verify(x=>x.GetFailOverEntries(), Times.Once);

        }

        [Test]
        public void ShouldAlwaysUseFailoverTimeThreshold()
        {
            var failedOverEntries = new List<FailoverEntry>();
            for (int i = 1; i <= 2; i++)
            {
                failedOverEntries.Add(new FailoverEntry() { DateTime = DateTime.Now });
            }

            _mockedFailoverAppSettings.SetupGet(x => x.IsFailoverModeEnabled).Returns(false);
            _mockedFailoverAppSettings.SetupGet(x => x.FailoverRequestsThreshold).Returns(3);
            _mockedFailoverAppSettings.SetupGet(x => x.FailoverTimeThresholdInMin).Returns(-1);

            _mockedFailoverRepository.Setup(x => x.GetFailOverEntries()).Returns(failedOverEntries).Verifiable();

            var failoverState = _sut.CurrentFailoverState;

            _mockedFailoverAppSettings.Verify(x => x.FailoverTimeThresholdInMin, Times.Once);
        }


        [Test]
        public void ShouldNotCheckForFailoverModeIsThresholdNotExceeded()
        {
            var failedOverEntries = new List<FailoverEntry>();
            for (int i = 1; i <= 2; i++)
            {
                failedOverEntries.Add(new FailoverEntry() { DateTime = DateTime.Now });
            }

            _mockedFailoverAppSettings.SetupGet(x => x.IsFailoverModeEnabled).Returns(true).Verifiable();
            _mockedFailoverAppSettings.SetupGet(x => x.FailoverRequestsThreshold).Returns(3).Verifiable();

            _mockedFailoverRepository.Setup(x => x.GetFailOverEntries()).Returns(failedOverEntries);

            var failoverState = _sut.CurrentFailoverState;

            _mockedFailoverAppSettings.Verify(x => x.FailoverRequestsThreshold, Times.Once);
            _mockedFailoverAppSettings.Verify(x => x.IsFailoverModeEnabled, Times.Never);

        }

        [Test]
        public void ShouldCheckForFailoverModeIfThresholdExceeded()
        {
            var failedOverEntries = new List<FailoverEntry>();
            for (int i = 1; i <= 2; i++)
            {
                failedOverEntries.Add(new FailoverEntry() { DateTime = DateTime.Now });
            }

            _mockedFailoverAppSettings.SetupGet(x => x.IsFailoverModeEnabled).Returns(true).Verifiable();
            _mockedFailoverAppSettings.SetupGet(x => x.FailoverRequestsThreshold).Returns(1).Verifiable();
            _mockedFailoverAppSettings.SetupGet(x => x.FailoverTimeThresholdInMin).Returns(-1);
            
            _mockedFailoverRepository.Setup(x => x.GetFailOverEntries()).Returns(failedOverEntries);

            var failoverState = _sut.CurrentFailoverState;

            _mockedFailoverAppSettings.Verify(x => x.FailoverRequestsThreshold, Times.Once);
            _mockedFailoverAppSettings.Verify(x => x.IsFailoverModeEnabled, Times.Once);
        }



        [Test]
        public void ShouldOnlyVerifyFailoverThresholdAgainstEntiresInLastXMinutes()
        {
            var failedOverEntries = new List<FailoverEntry>();
            var failoverTimeThresholdInMin = -1;

            for (int i = 1; i <= 3; i++)
            {
                failedOverEntries.Add(new FailoverEntry() { DateTime = DateTime.Now });
            }

            for (int i = 1; i <= 5; i++)
            {
                failedOverEntries.Add(new FailoverEntry() { DateTime = DateTime.Now.AddMinutes(-5) });
            }

            _mockedFailoverAppSettings.SetupGet(x => x.IsFailoverModeEnabled).Returns(true).Verifiable();
            _mockedFailoverAppSettings.SetupGet(x => x.FailoverRequestsThreshold).Returns(2).Verifiable();
            _mockedFailoverAppSettings.SetupGet(x => x.FailoverTimeThresholdInMin).Returns(failoverTimeThresholdInMin).Verifiable();
            
            _mockedFailoverRepository.Setup(x => x.GetFailOverEntries()).Returns(failedOverEntries);

            var failoverState = _sut.CurrentFailoverState;

            Assert.AreEqual(FailoverState.DoFailover, failoverState);
        }
    }
}