using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameConsole.Classes;
using static Common;
using System.Security.Policy;
using System.ServiceModel.Web;
using System.Net;
using System.Collections;

namespace GameConsole.Classes
{
    public class CredentialsValidator : ICredentialsValidator
    {
        //WebOperationContext ctx = WebOperationContext.Current;
        DBFunctions objDb = new DBFunctions();
        Hashtable parameters = new Hashtable();

        public bool IsValid(Int32 CREDS)
        {
            //// Check for valid creds here
            //// I compare using hashes only for example purposes
            //if (creds.User == "user1" && Hash.Get(creds.Password, Hash.HashType.MD5) == Hash.Get("pass1", Hash.HashType.MD5))
            //    return true;
            //return false;

            // Check for valid creds here
            // I compare using hashes only for example purposes

            Int32 objREGISTRATIONID = Common.REG_HEADER();
            if (objREGISTRATIONID == 0)
            {                
                return false;
            }

            parameters.Clear();
            parameters.Add("@REGISTRATIONID", objREGISTRATIONID);
            Int32 objREGID = Convert.ToInt32(objDb.SendValue_Parameter("SELECT REGISTRATIONID FROM STBL_REGISTRATION WITH(NOLOCK) WHERE STATUS=1 AND REGISTRATIONID=@REGISTRATIONID", parameters));

            if (Hash.Get(objREGID.ToString(), Hash.HashType.MD5) == Hash.Get(objREGISTRATIONID.ToString(), Hash.HashType.MD5))
                return true;
            return false;
        }
    }
}