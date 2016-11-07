using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlWorks.Services.Data
{
    public abstract class BaseDto
    {
        public virtual string ToJson()
        {
            try
            {
                return JsonConvert.SerializeObject(this);
            }
            catch { return String.Empty; }
        }

    }
}
