using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace GameConsole.Classes
{
    public class EncOutputCL
    {
        [DataMember(IsRequired = false, Order = 1)]
        public string DATA { get; set; }
    }

    public class EncInputCL
    {
        [DataMember(IsRequired = false, Order = 1)]
        public string DATA { get; set; }
    }
}