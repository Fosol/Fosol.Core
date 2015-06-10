using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Fosol.Core.Data
{
    /// <summary>
    /// SqlHelper static class, provides a handful of helpful methods to interact with databases.
    /// </summary>
    public static class SqlHelper
    {
        #region Methods
        /// <summary>
        /// Execute the script specified at the path.
        /// </summary>
        /// <param name="connection">Connection to the database.</param>
        /// <param name="path">Path to the SQL script file.</param>
        /// <param name="commandTimeout">The time in seconds to wait for the command to execute.  '0' means no timeout.</param>
        /// <returns>The number of rows affected by the script.</returns>
        public static int ExecuteScript(System.Data.IDbConnection connection, string path, int commandTimeout = 0)
        {
            Validation.Argument.Assert.IsNotNull(connection, nameof(connection));
            Validation.Argument.Assert.IsNotNullOrEmpty(path, nameof(path));
            
            var script = File.ReadAllText(path);

            // Need to parse the script for each individual 'GO' statement because a DbCommand cannot execute that keyword.
            var steps = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant).Where(s => s != string.Empty);
            int result = 0;

            var cmd = connection.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandTimeout = commandTimeout;

            foreach (var sql in steps)
            {
                cmd.CommandText = sql;
                result += cmd.ExecuteNonQuery();
            }

            return result;
        }
        #endregion
    }
}
