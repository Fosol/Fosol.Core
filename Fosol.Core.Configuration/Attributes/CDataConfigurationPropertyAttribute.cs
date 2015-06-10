using System;

namespace Fosol.Core.Configuration.Attributes
{
    /// <summary>
    /// CDataConfigurationPropertyAttribute class provides a way to access text data within CDATA nodes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CDataConfigurationPropertyAttribute
        : Attribute
    {
    }
}
