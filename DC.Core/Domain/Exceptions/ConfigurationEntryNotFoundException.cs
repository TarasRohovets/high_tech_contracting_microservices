using System;
namespace DC.Core.Domain.Exceptions
{
    public class ConfigurationEntryNotFoundException : Exception
    {
        public ConfigurationEntryNotFoundException(){}
        public ConfigurationEntryNotFoundException(string message) : base(message) { }
    }
}
