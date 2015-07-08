using System.Configuration;
using Refactor.Lib.Helpers;

namespace Refactor.Lib.Services
{
    public class FailoverAppSettings : IFailoverAppSettings
    {
        private readonly ITypeConverter _typeConverter;

        public FailoverAppSettings(ITypeConverter typeConverter)
        {
            _typeConverter = typeConverter;
        }

        public bool IsFailoverModeEnabled
        {
            get
            {
                return _typeConverter.SafeConvertStringToBool(ConfigurationManager.AppSettings["IsFailoverModeEnabled"]); 
                
            }
        }

        public int FailoverRequestsThreshold
        {
            get
            {
                return _typeConverter.SafeConvertStringToInt(ConfigurationManager.AppSettings["FailoverRequestsThreshold"]); 
                
            }
        }

        public int FailoverTimeThresholdInMin
        {
            get
            {
                return _typeConverter.SafeConvertStringToNegativeInt(ConfigurationManager.AppSettings["FailoverTimeThresholdInMin"]);

            }
        }
    }
}