using System;
using System.Linq;
using Refactor.Lib.Model;
using Refactor.Lib.Repositories;

namespace Refactor.Lib.Services
{
    public class FailoverService : IFailoverService
    {
        private readonly IFailoverRepository _failoverRepository;
        private readonly IFailoverAppSettings _failoverAppSettings;

        public FailoverService(IFailoverRepository failoverRepository, IFailoverAppSettings failoverAppSettings)
        {
            _failoverRepository = failoverRepository;
            _failoverAppSettings = failoverAppSettings;
        }

        public FailoverState CurrentFailoverState
        {
            get
            {
                return HasFailedover() ? FailoverState.DoFailover : FailoverState.DontFailover;
            }
        }

        private bool HasFailedover()
        {
            return IsFailoverThresholdExceeded() && _failoverAppSettings.IsFailoverModeEnabled;
        }

        private bool IsFailoverThresholdExceeded()
        {
            return GetFailedRequestsFromRepo() > _failoverAppSettings.FailoverRequestsThreshold;
        }

        private int GetFailedRequestsFromRepo()
        {
            var allFailoverEntries = _failoverRepository.GetFailOverEntries();
            var failoverTimeThreshold = _failoverAppSettings.FailoverTimeThresholdInMin; 
            var failedRequests = allFailoverEntries.Count(failoverEntry => failoverEntry.DateTime > DateTime.Now.AddMinutes(failoverTimeThreshold));
            return failedRequests;
        }
    }
}