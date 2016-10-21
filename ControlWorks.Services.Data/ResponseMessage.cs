using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlWorks.Services.Data
{
    public class ResponseMessage
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public ErrorResponse[] Errors {get; set;}
    }

    public class ErrorResponse
    {
        public string Error { get; set; }
    }
}
