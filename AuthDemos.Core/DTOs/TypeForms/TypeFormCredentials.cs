using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemos.Core.DTOs.TypeForms
{
    public class TypeFormCredentials
    {
        public string BaseUrl { get; set; }
        public string Secret { get; set; }
        public string AccessToken { get; set; }
    }
}
