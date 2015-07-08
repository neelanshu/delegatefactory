namespace Refactor.Lib.Helpers
{
    /*This abstraction is responsibile for performing all type conversions 
     * Like safely convert string to bool 
     * Or safely convert string to int */
    public interface ITypeConverter
    {
        bool SafeConvertStringToBool(string strToConvert);
        int SafeConvertStringToInt(string strToConvert);
        int SafeConvertStringToNegativeInt(string strToConvert);
    }
}