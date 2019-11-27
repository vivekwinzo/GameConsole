using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace GameConsole.Classes.inputclasses
{
    public class UserIN
    {
        [DataContract]
        public class Change_PwdIN
        {
            [DataMember(IsRequired = true)]
            public string email { get; set; }

            [DataMember(IsRequired = true)]
            public string oldPassword { get; set; }

            [DataMember(IsRequired = true)]
            public string newPassword { get; set; }
        }

        [DataContract]
        public class User_LoginIN
        {
            [DataMember(IsRequired =true)]
            public string email { get; set; }

            [DataMember(IsRequired = true)]
            public string password { get; set; }
        }

        [DataContract]
        public class SignupIN
        {
            [DataMember(IsRequired = true)]
            public string name { get; set; }

            [DataMember(IsRequired = true)]
            public string email { get; set; }

            [DataMember]
            public string mobile { get; set; }

            [DataMember(IsRequired = true)]
            public string password { get; set; }
        }
    }
}