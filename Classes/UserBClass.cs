using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace GameConsole.Classes
{
    public class UserBClass
    {
        [DataContract]
        public class ChangePwdCL
        {
            [DataMember(IsRequired = false, Order = 101)]
            public long SERVER_DATETIME { get; set; }
        }

        [DataContract]
        public class LoginCL
        {
            [DataMember(IsRequired = false, Order = 1)]
            public string RegistrationId { get; set; }

            [DataMember(IsRequired = false, Order = 2)]
            public string name { get; set; }

            [DataMember(IsRequired = false, Order = 3)]
            public string email { get; set; }

            [DataMember(IsRequired = false, Order = 4)]
            public string mobile { get; set; }

            [DataMember(IsRequired = false, Order = 101)]
            public long SERVER_DATETIME { get; set; }
        }

        [DataContract]
        public class SignupCL
        {
            [DataMember(IsRequired = false, Order = 101)]
            public long SERVER_DATETIME { get; set; }
        }
    }
}