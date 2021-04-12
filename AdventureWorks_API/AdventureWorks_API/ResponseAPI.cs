using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureWorks_API
{
    public class ResponseAPI
    {
        public ResponseAPI()
        {
            StatusCode = 200;
            IsError = false;
            Message = "";
            Data = null;
        }
        public int StatusCode { get; set; }
        public bool IsError { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
