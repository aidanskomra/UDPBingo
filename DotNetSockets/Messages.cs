using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace DotNetSockets
{
    public class Messages
    {
        public string Message { get; set; }
        public EndPoint RemoteEP { get; set; }
    }
}
