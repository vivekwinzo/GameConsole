using DevTrends.WCFDataAnnotations;
using log4net;
using Microsoft.Security.Application;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using static Common;
using GameConsole.Classes;
using static GameConsole.Classes.UserBClass;
using static GameConsole.Classes.inputclasses.UserIN;

namespace GameConsole
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "User" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select User.svc or User.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple)]
    [ValidateDataAnnotationsBehavior]
    public class User : IUser
    {
        WebOperationContext ctx = WebOperationContext.Current;
        ILog logger = log4net.LogManager.GetLogger("ErrorLog");
        DBFunctions objDb = new DBFunctions();
        mailsCL objMails = new mailsCL();
        RijndaelCrypt objencdec = new RijndaelCrypt();

        //User Change Password
        public ChangePwdCL Change_Pwd(Change_PwdIN changePwd)
        {
            //Initialize Logger
            log4net.Config.XmlConfigurator.Configure();
            //Initialize Logger

            ChangePwdCL results_final = new ChangePwdCL();

            try
            {
                string objUSER_AGENT = Common.USER_AGENT();
                if (objUSER_AGENT != ConfigurationManager.AppSettings["User-Agent"].ToString())
                {
                    ctx.OutgoingResponse.StatusCode = (System.Net.HttpStatusCode)401;
                    goto Finish;
                }
                
                string encOldPWD = Common.Password_Encrypt(changePwd.oldPassword);
                string encNewPWD = Common.Password_Encrypt(changePwd.newPassword);
                Hashtable parameters = new Hashtable();
                parameters.Clear();
                parameters.Add("@EMAIL", changePwd.email);
                parameters.Add("@OLD_PASSWORD", encOldPWD);
                parameters.Add("@PASSWORD", encNewPWD);
                parameters.Add("@MODE", "CHANGE_PWD");
                DataSet dsrec = objDb.senddataset_SP("GC_REGISTRATION_PRC", parameters);

                if (dsrec.Tables[0].Rows.Count > 0)
                {
                    if (dsrec.Tables[0].Rows[0]["RSTATUS"].ToString().ToLower() == "invalid")
                    {
                        //ctx.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;
                        ctx.OutgoingResponse.StatusCode = (System.Net.HttpStatusCode)211;
                        goto Finish;
                    }

                    results_final.SERVER_DATETIME = Common.GetUTCdatetime_epoch();
                    
                }
                else
                {
                    ctx.OutgoingResponse.StatusCode = (System.Net.HttpStatusCode)209;
                }
                Finish:
                return results_final;
            }
            catch (Exception ex)
            {
                ctx.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                logger.Error("User.svc/Change_Pwd: " + ex.Message.ToString() + "::" + ex.StackTrace.ToString());
                return results_final;
            }
        }

        //User Login
        public LoginCL User_Login(User_LoginIN user_Login)
        {
            //Initialize Logger
            log4net.Config.XmlConfigurator.Configure();
            //Initialize Logger

            LoginCL results_final = new LoginCL();

            try
            {
                string objUSER_AGENT = Common.USER_AGENT();
                if (objUSER_AGENT != ConfigurationManager.AppSettings["User-Agent"].ToString())
                {
                    ctx.OutgoingResponse.StatusCode = (System.Net.HttpStatusCode)401;
                    goto Finish;
                }

                //get IPAddress
                OperationContext context = OperationContext.Current;
                MessageProperties messageProperties = context.IncomingMessageProperties;
                RemoteEndpointMessageProperty endpointProperty = messageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                //get IPAddress

                string encPWD = Common.Password_Encrypt(user_Login.password);
                Hashtable parameters = new Hashtable();
                parameters.Clear();
                parameters.Add("@EMAIL", user_Login.email);
                parameters.Add("@PASSWORD", encPWD);
                parameters.Add("@IPADDRESS", endpointProperty.Address + ":" + endpointProperty.Port);
                parameters.Add("@MODE", "LOGIN");
                DataSet dsrec = objDb.senddataset_SP("GC_REGISTRATION_PRC", parameters);

                if (dsrec.Tables[0].Rows.Count > 0)
                {
                    if (dsrec.Tables[0].Rows[0]["RSTATUS"].ToString().ToLower() == "invalid")
                    {
                        //ctx.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;
                        ctx.OutgoingResponse.StatusCode = (System.Net.HttpStatusCode)211;
                        goto Finish;
                    }
                    if (dsrec.Tables[0].Rows[0]["RSTATUS"].ToString().ToLower() == "email not verified")
                    {
                        //Send email verification mail here..
                        //ctx.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;
                        ctx.OutgoingResponse.StatusCode = (System.Net.HttpStatusCode)415;
                        goto Finish;
                    }
                    if (dsrec.Tables[0].Rows[0]["RSTATUS"].ToString().ToLower() == "user blocked")
                    {
                        ctx.OutgoingResponse.StatusCode = (System.Net.HttpStatusCode)419;
                        goto Finish;
                    }
                    
                    //string objResponse = string.Empty;
                    //Int32 objRegistrationId = Convert.ToInt32(dsrec.Tables[0].Rows[0]["REGISTRATIONID"].ToString());
                    //if (objRegistrationId == 0)
                    //{
                    //    ctx.OutgoingResponse.StatusCode = (System.Net.HttpStatusCode)211;
                    //    goto Finish;
                    //}

                    results_final.RegistrationId = objDb.ENC_REGID(Convert.ToInt32(dsrec.Tables[0].Rows[0]["REGISTRATIONID"].ToString()));
                    results_final.name = dsrec.Tables[0].Rows[0]["NAME"].ToString();
                    results_final.email = dsrec.Tables[0].Rows[0]["EMAIL"].ToString();
                    results_final.mobile = dsrec.Tables[0].Rows[0]["MOBILE"].ToString();
                    results_final.SERVER_DATETIME = Common.GetUTCdatetime_epoch();
                }
                else
                {
                    ctx.OutgoingResponse.StatusCode = (System.Net.HttpStatusCode)211;
                }
                Finish:
                return results_final;
            }
            catch (Exception ex)
            {
                ctx.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                logger.Error("User.svc/User_Login: " + ex.Message.ToString() + "::" + ex.StackTrace.ToString());
                return results_final;
            }
        }

        //User Login
        public SignupCL Signup(SignupIN signup)
        {
            //Initialize Logger
            log4net.Config.XmlConfigurator.Configure();
            //Initialize Logger

            SignupCL results_final = new SignupCL();

            try
            {
                string objUSER_AGENT = Common.USER_AGENT();
                if (objUSER_AGENT != ConfigurationManager.AppSettings["User-Agent"].ToString())
                {
                    ctx.OutgoingResponse.StatusCode = (System.Net.HttpStatusCode)401;
                    goto Finish;
                }

                //get IPAddress
                OperationContext context = OperationContext.Current;
                MessageProperties messageProperties = context.IncomingMessageProperties;
                RemoteEndpointMessageProperty endpointProperty = messageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                //get IPAddress

                string encPWD = Common.Password_Encrypt(signup.password);
                Hashtable parameters = new Hashtable();
                parameters.Clear();
                parameters.Add("@NAME", signup.name);
                parameters.Add("@EMAIL", signup.email);
                parameters.Add("@MOBILE", signup.mobile);
                parameters.Add("@PASSWORD", encPWD);
                parameters.Add("@IPADDRESS", endpointProperty.Address + ":" + endpointProperty.Port);
                parameters.Add("@MODE", "SIGNUP");
                DataSet dsrec = objDb.senddataset_SP("GC_REGISTRATION_PRC", parameters);

                if (dsrec.Tables[0].Rows.Count > 0)
                {
                    if (dsrec.Tables[0].Rows[0]["RSTATUS"].ToString().ToLower() == "invalid")
                    {
                        //ctx.OutgoingResponse.StatusCode = HttpStatusCode.NoContent;
                        ctx.OutgoingResponse.StatusCode = (System.Net.HttpStatusCode)211;
                        goto Finish;
                    }                    
                    
                    results_final.SERVER_DATETIME = Common.GetUTCdatetime_epoch();

                    //Send email verification mail here..
                }
                else
                {
                    ctx.OutgoingResponse.StatusCode = (System.Net.HttpStatusCode)209;
                }
                Finish:
                return results_final;
            }
            catch (Exception ex)
            {
                ctx.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                logger.Error("User.svc/Signup: " + ex.Message.ToString() + "::" + ex.StackTrace.ToString());
                return results_final;
            }
        }
    }
}
