using System;

namespace Refactor.Lib.Helpers
{
    public class TypeConverter : ITypeConverter
    {
        public bool SafeConvertStringToBool(string strToConvert)
        {
            try
            {
                return bool.Parse(strToConvert);
            }

                /* At this point i dont know how to handle exceptions like "FormatException" or "ArgumentNullReferenceException"
             * So just return false
             * Hence the method name SafeConvert */

            catch (Exception)
            {
                return false; 
            }
        }

        public int SafeConvertStringToInt(string strToConvert)
        {
            try
            {
                return int.Parse(strToConvert);
            }

                /* At this point i dont know how to handle exceptions like "FormatException" or "ArgumentNullReferenceException"
             * So just return default int OR 0
             * Hence the method name SafeConvert */

            catch (Exception)
            {
                return 0; //Or default(int) both are same, for readability prefer to return 0 
            }
        }

        public int SafeConvertStringToNegativeInt(string strToConvert)
        {
            try
            {
                var val =  int.Parse(strToConvert);
                if (val > 0) val = (-1*val);

                return val;
            }

                /* At this point i dont know how to handle exceptions like "FormatException" or "ArgumentNullReferenceException"
             * So just return default int OR 0
             * Hence the method name SafeConvert */

            catch (Exception)
            {
                return -1; //Or default(int) both are same, for readability prefer to return 0 
            }
        }
    }
}