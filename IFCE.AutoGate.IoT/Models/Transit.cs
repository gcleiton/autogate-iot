using nanoFramework.Json;
using System;

namespace IFCE.AutoGate.IoT.Models
{
    class Transit
    {
        public string CardNumber { get; set; }
        public int TransitTypeId { get; set; }

        public string ToJson()
        {
            return JsonSerializer.SerializeObject(this);
        }
    }
}
 