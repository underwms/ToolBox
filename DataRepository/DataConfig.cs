using System;
using System.Configuration;

namespace DataTemplate
{
    public class DataConfig
    {
        public static string SqlDefenderConnection { get { return GetConnectString("", string.Empty); } }
        public static string OleDBSpreadsheetConnection(string path) =>
            string.Format(GetConnectString("", string.Empty), path);

        private static string GetConnectString(string setting, string defaultValue)
        {
            var retVal = defaultValue;

            try
            {
                var work = ConfigurationManager.ConnectionStrings[setting].ConnectionString;

                if (!string.IsNullOrEmpty(work))
                { retVal = work; }
            }
            catch (Exception)
            { throw; }

            return retVal;
        }
    }
}

/*
 * Web.config example
 * 
<connectionStrings>
    <add name="" connectionString="Data Source=;Initial Catalog=;Integrated Security=True" providerName="System.Data.SqlClient" />
    <add name="" connectionString="Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=yes;IMEX=1'" /> <!-- IMEX=1 prevents OleDb from trying to interpret type and treats everything as a string -->
</connectionStrings>

 * 
 */
