using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contextual
{
    public class AppSetting
    {
        /*
        Configuration Config;

        public AppSetting() 
        { 
            Config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }

        public string GetConnectionString(string key)
        {
            return Config.ConnectionStrings.ConnectionStrings[key].ConnectionString;
        }

        public void SaveConnectionString(string key, string value)
        {
            Config.ConnectionStrings.ConnectionStrings[key].ConnectionString = value;
            Config.ConnectionStrings.ConnectionStrings[key].ProviderName = "System.Data.SqlClient";
            Config.Save(ConfigurationSaveMode.Modified);
        }
        */
    }
}
