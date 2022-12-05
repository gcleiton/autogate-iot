using System;

namespace IFCE.AutoGate.IoT.Models
{
    class HttpException : Exception
    {
        public HttpException(): base("HTTP request failed") { }
    }
}
