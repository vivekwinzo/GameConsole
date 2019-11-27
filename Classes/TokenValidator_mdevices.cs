using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using static Common;
using log4net;

namespace GameConsole.Classes
{
    public class TokenValidator_mdevices : ITokenValidator_mdevices
    {
        DBFunctions objDb = new DBFunctions();
        Hashtable parameters = new Hashtable();
        ILog logger = log4net.LogManager.GetLogger("ErrorLog");

        public bool IsValid(string token, Int32 REGID)
        {
            //Initialize Logger
            log4net.Config.XmlConfigurator.Configure();
            //Initialize Logger

            string objUSER_AGENT = Common.USER_AGENT();
            parameters.Clear();
            parameters.Add("@TOKEN", token);
            parameters.Add("@REGISTRATIONID", REGID);
            Int32 objCOUNT = 0;
            objCOUNT = Convert.ToInt32(objDb.SendValue_Parameter("SELECT COUNT(1) FROM STBL_REGISTRATION R WITH(NOLOCK) INNER JOIN STBL_REGISTRATION_APP RA WITH(NOLOCK) ON R.REGISTRATIONID=RA.REGISTRATIONID WHERE R.STATUS=1 AND R.MOBILE_VERIFY=1 AND RA.USER_TOKEN=@TOKEN AND RA.USER_TOKEN_EXP>GETDATE() AND R.REGISTRATIONID=@REGISTRATIONID", parameters));
            //logger.Error("TokenValidator_mdevices.cs/User-Agent:" + objUSER_AGENT);
            if (objCOUNT == 1 && objUSER_AGENT == ConfigurationManager.AppSettings["User-Agent"].ToString())
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