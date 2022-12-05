using System;

namespace IFCE.AutoGate.IoT.Models
{
    class RfidException : Exception
    {
        public RfidException(): base("Failed to read RFID card") { }
    }
}
