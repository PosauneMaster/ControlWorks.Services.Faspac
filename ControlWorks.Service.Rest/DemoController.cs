using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace ControlWorks.Service.Rest
{
    public class DemoController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Greeting()
        {
            return Ok("Hello World!");
        }
    }
}
