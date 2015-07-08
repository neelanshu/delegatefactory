using System;
using System.IO;

namespace Refactor.Lib.Guards
{
    internal static class Requires
    {
        public static void ArgumentsToBeNotNull(object value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void ArgumentsToBeNotNullOrEmpty(string value, string name)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("String cannot be null or empty", name);
            }
        }

        public static void ArgumentsToBeGreaterThan1(int value, string name)
        {
            if (value < 1)
            {
                throw new InvalidDataException(name);
            }
        }
    }
}