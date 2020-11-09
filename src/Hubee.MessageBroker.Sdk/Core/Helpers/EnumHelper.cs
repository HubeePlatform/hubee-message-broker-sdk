using System;

namespace Hubee.MessageBroker.Sdk.Core.Helpers
{
    internal static class EnumHelper
    {
        public static T Parse<T>(string value)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch
            {
                return (T)Enum.Parse(typeof(T), "Undefined", true);
            }
        }
    }
}