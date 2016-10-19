using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlWorks.Services.Configuration
{
    public class ConfigurationSettings
    {

        public void GetSettings()
        {
            dynamic settings = new ExpandoObject();
            var dict = (IDictionary<string, object>)settings;

            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                dict.Add(key, ConfigurationManager.AppSettings[key]);
            }

            foreach (var key in ConfigurationManager.ConnectionStrings)
            {

            }


            var json = JsonConvert.SerializeObject(settings);
        }
    }
}
