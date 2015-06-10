using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fosol.Core.Configuration
{
    /// <summary>
    /// ConfigurationHelper static class, provides methods for configuration settings.
    /// </summary>
    public static class ConfigurationHelper
    {
        #region Methods
        /// <summary>
        /// Get the AppSetting key value from the configuration file for the specified assembly.
        /// This method is useful when using assemblies from other sources.
        /// </summary>
        /// <exception cref="System.IndexOutOfRangeException">Configuration collection does not contain requested key.</exception>
        /// <param name="assembly">Assembly which you want the key value from.</param>
        /// <param name="key">Key name.</param>
        /// <returns>Value of AppSetting key value.</returns>
        public static string GetAppSetting(System.Reflection.Assembly assembly, string key)
        {
            var config = System.Configuration.ConfigurationManager.OpenExeConfiguration(assembly.Location);

            if (config == null || config.AppSettings.Settings[key] == null)
                throw new IndexOutOfRangeException(string.Format(Resources.Multilingual.ConfigurationHelper_GetAppSetting_KeyDoesNotExist, key));

            return config.AppSettings.Settings[key].Value;
        }
        #endregion
    }
}
