namespace Refactor.Lib.Services
{
    public interface IFailoverAppSettings
    {
        bool IsFailoverModeEnabled { get;} //true
        int FailoverRequestsThreshold { get; } //100
        int FailoverTimeThresholdInMin{ get; } //-10
    }
}