using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemos.Core.DTOs.Response
{
    public class ResponseDTO
    {
        public ResponseDTO()
        {
            Status = true;
            Message = "Success";
        }
        public object Data { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
    }
}
