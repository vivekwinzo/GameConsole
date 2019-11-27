using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using static Common;

namespace GameConsole.Classes
{
    public class TokenValidator : ITokenValidator
    {
        DBFunctions objDb = new DBFunctions();
        Hashtable parameters = new Hashtable();

        public bool IsValid(string token, Int32 REGID)
        {
            string objUSER_AGENT = Common.USER_AGENT();
           // parameters.Clear();
            //parameters.Add("@TOKEN", token);
            //Int32 objREGID = 0;
            //objREGID = Convert.ToInt32(objDb.SendValue_Parameter("SELECT REGISTRATIONID FROM STBL_REGISTRATION WITH(NOLOCK) WHERE STATUS=1 AND USER_TOKEN=@TOKEN AND USER_TOKEN_EXP>GETDATE()", parameters));
            if (objUSER_AGENT == ConfigurationManager.AppSettings["User-Agent"].ToString())
            {
                return true;
            }
            else
            {
                return false;
            }


            //parameters.Clear();
            //parameters.Add("@TOKEN", token);
            //Int32 objREGID = 0;
            //objREGID = Convert.ToInt32(objDb.SendValue_Parameter("SELECT REGISTRATIONID FROM STBL_REGISTRATION WITH(NOLOCK) WHERE STATUS=1 AND USER_TOKEN=@TOKEN AND USER_TOKEN_EXP>GETDATE()", parameters));
            //if (REGID == objREGID && token != "" && objUSER_AGENT == ConfigurationManager.AppSettings["User-Agent"].ToString())
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            // return TokenBuilder.StaticToken == token;
        }
    }
}