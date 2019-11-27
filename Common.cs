using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using System.Net;
using System.IO;
using System.Configuration;
using System.ServiceModel.Web;
using GameConsole;
using GameConsole.Classes;
using System.Collections;
using System.Data;
using log4net;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Globalization;
using System.Threading;

public static class Common
{
    static DBFunctions objDb = new DBFunctions();
    static Hashtable parameters = new Hashtable();
    //static Hashtable parameters_login = new Hashtable();
    //static Hashtable parameters_language = new Hashtable();
    //static Hashtable parameters_booster = new Hashtable();
    static ConcurrentDictionary<string, object> parameters_cd = new ConcurrentDictionary<string, object>();
    static mailsCL objMails = new mailsCL();
    static WebOperationContext ctx = WebOperationContext.Current;
    static RijndaelCrypt objencdec = new RijndaelCrypt();
    static ILog logger = log4net.LogManager.GetLogger("ErrorLog");
    
    public static string Nonce()
    {
        string nonce = "";
        try
        {
            IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = request.Headers;
            nonce = request.Headers["nonce"];
        }
        catch (Exception)
        {
            ctx.OutgoingResponse.StatusCode = (System.Net.HttpStatusCode)200;
            nonce = "";
        }
        return nonce;
    }

    public static string ClientType()
    {
        string client = "";
        try
        {
            IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = request.Headers;
            client = request.Headers["ClientType"];
        }
        catch (Exception)
        {
            ctx.OutgoingResponse.StatusCode = (System.Net.HttpStatusCode)200;
            client = "";
        }
        return client;
    }    

    public static Int32 REG_HEADER()
    {
        //Get REGISTRATIONID from header
        Int32 objREGISTRATIONID = 0;
        Int32 objCOUNT = 0;
        string objREGID = string.Empty;
        try
        {
            IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = request.Headers;
            //Initialize Logger
            log4net.Config.XmlConfigurator.Configure();
            //Initialize Logger

            objREGID = request.Headers["REGISTRATIONID"];
            objREGISTRATIONID = objDb.DEC_REGID(objREGID);
            ////Console.WriteLine(request.Method + " " + request.UriTemplateMatch.RequestUri.AbsolutePath);
            //foreach (string headerName in headers.AllKeys)
            //{
            //    if (headerName == "REGISTRATIONID")
            //    {
            //        objREGID = headers[headerName];
            //        objREGISTRATIONID = objDb.DEC_REGID(headers[headerName]);
            //    }
            //    //Console.WriteLine(headerName + ": " + headers[headerName]);
            //}

            if (objREGISTRATIONID != 0)
            {
                System.Threading.Thread.Sleep(10);
                Hashtable parameters_login = new Hashtable();
                //Get REGISTRATIONID OF active user
                parameters_login.Clear();
                parameters_login.Add("@REGISTRATIONID", objREGISTRATIONID);

                objCOUNT = Convert.ToInt32(objDb.SendValue_Parameter("SELECT COUNT(1) FROM STBL_REGISTRATION WITH(NOLOCK) WHERE REGISTRATIONID=@REGISTRATIONID AND STATUS=1", parameters_login));
                if (objCOUNT == 0)
                {
                    logger.Error("Common.cs/REG_HEADER: " + objREGISTRATIONID.ToString());
                    objREGISTRATIONID = 0;
                }
                //Get REGISTRATIONID OF active user                
            }
        }
        catch (Exception ex)
        {
            ctx.OutgoingResponse.StatusCode = (System.Net.HttpStatusCode)200;
            logger.Error("Common.cs/REG_HEADER_Error: REGID:" + objREGID + "::" + objREGISTRATIONID.ToString() + "/" + ex.Message.ToString() + "::" + ex.StackTrace.ToString());
            objREGISTRATIONID = 0;
        }
        return objREGISTRATIONID;
    }
    
    public static string REG_FBID()
    {
        //Get FBID from header
        string objFBID = "";
        try
        {
            IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = request.Headers;
            objFBID = request.Headers["FBID"];

            ////Console.WriteLine(request.Method + " " + request.UriTemplateMatch.RequestUri.AbsolutePath);
            //foreach (string headerName in headers.AllKeys)
            //{
            //    if (headerName == "FBID")
            //    {
            //        objFBID = headers[headerName];
            //    }
            //    //Console.WriteLine(headerName + ": " + headers[headerName]);
            //}
        }
        catch (Exception)
        {
            objFBID = "";
        }
        return objFBID;
    }

    public static string REG_FB_ACCESSTOKEN()
    {
        //Get FBID from header
        string objFB_ACCESSTOKEN = "";
        try
        {
            IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = request.Headers;
            objFB_ACCESSTOKEN = request.Headers["FB_ACCESSTOKEN"];
            ////Console.WriteLine(request.Method + " " + request.UriTemplateMatch.RequestUri.AbsolutePath);
            //foreach (string headerName in headers.AllKeys)
            //{
            //    if (headerName == "FB_ACCESSTOKEN")
            //    {
            //        objFB_ACCESSTOKEN = headers[headerName];
            //    }
            //    //Console.WriteLine(headerName + ": " + headers[headerName]);
            //}
        }
        catch (Exception)
        {
            objFB_ACCESSTOKEN = "";
        }
        return objFB_ACCESSTOKEN;
    }

    public static string REG_TOKEN()
    {
        //Get FBID from header
        string objTOKEN = "";
        try
        {
            IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = request.Headers;
            objTOKEN = request.Headers["Token"];

            ////Console.WriteLine(request.Method + " " + request.UriTemplateMatch.RequestUri.AbsolutePath);
            //foreach (string headerName in headers.AllKeys)
            //{
            //    if (headerName == "Token")
            //    {
            //        objTOKEN = headers[headerName];
            //    }
            //    //Console.WriteLine(headerName + ": " + headers[headerName]);
            //}
        }
        catch (Exception)
        {
            objTOKEN = "";
        }
        return objTOKEN;
    }

    public static string USER_AGENT()
    {
        //Get FBID from header
        string objUSER_AGENT = "";
        try
        {
            IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = request.Headers;
            objUSER_AGENT = request.Headers["User-Agent"];

            ////Console.WriteLine(request.Method + " " + request.UriTemplateMatch.RequestUri.AbsolutePath);
            //foreach (string headerName in headers.AllKeys)
            //{
            //    if (headerName == "User-Agent")
            //    {
            //        objUSER_AGENT = headers[headerName];
            //    }
            //    //Console.WriteLine(headerName + ": " + headers[headerName]);
            //}
        }
        catch (Exception)
        {
            objUSER_AGENT = "";
        }
        return objUSER_AGENT;
    }
    
    public static string REG_LANGUAGEID()
    {
        //Initialize Logger
        log4net.Config.XmlConfigurator.Configure();
        //Initialize Logger

        //Get FBID from header
        string objLANGUAGEID = "1", objLANGUAGECODE = "en";
        Int32 objLANGID = 0;
        try
        {
            IncomingWebRequestContext request = WebOperationContext.Current.IncomingRequest;
            WebHeaderCollection headers = request.Headers;
            objLANGUAGECODE = !string.IsNullOrEmpty(request.Headers["LANGUAGEID"]) ? request.Headers["LANGUAGEID"] : "en";
            if (objLANGUAGECODE == "en" || objLANGUAGECODE == "")
            {
                objLANGUAGEID = "1"; //Fixed for english default
            }
            else
            {
                Hashtable parameters_language = new Hashtable();
                parameters_language.Clear();
                parameters_language.Add("@LANGUAGE_CODE", objLANGUAGECODE);
                objLANGID = Convert.ToInt32(objDb.SendValue_Parameter("SELECT LANGUAGEID FROM STBL_LANGUAGES_MASTER WITH(NOLOCK) WHERE LANGUAGE_CODE=@LANGUAGE_CODE AND ACTIVE=1", parameters_language));
                objLANGUAGEID = objLANGID.ToString();
                if (objLANGUAGECODE == "bho") //set bhojpuri as hindi as not available in culture on server
                {
                    objLANGUAGECODE = "hi";
                }
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(objLANGUAGECODE + "-IN");
            }

            ////Console.WriteLine(request.Method + " " + request.UriTemplateMatch.RequestUri.AbsolutePath);
            //foreach (string headerName in headers.AllKeys)
            //{
            //    if (headerName == "LANGUAGEID")
            //    {
            //        objLANGUAGECODE = headers[headerName];
            //        if (objLANGUAGECODE == "en" || objLANGUAGECODE == "")
            //        {
            //            objLANGUAGEID = "1"; //Fixed for english default
            //        }
            //        else
            //        {
            //            Hashtable parameters_language = new Hashtable();
            //            parameters_language.Clear();
            //            parameters_language.Add("@LANGUAGE_CODE", objLANGUAGECODE);
            //            objLANGID = Convert.ToInt32(objDb.SendValue_Parameter("SELECT LANGUAGEID FROM STBL_LANGUAGES_MASTER WITH(NOLOCK) WHERE LANGUAGE_CODE=@LANGUAGE_CODE AND ACTIVE=1", parameters_language));
            //            objLANGUAGEID = objLANGID.ToString();
            //        }
            //    }
            //    //Console.WriteLine(headerName + ": " + headers[headerName]);
            //}
            if (objLANGUAGEID == "")
            {
                objLANGUAGEID = "1"; //Fixed for english default
            }
        }
        catch (Exception ex)
        {
            objLANGUAGEID = "1";
            logger.Error("Common.cs/REG_LANGUAGEID: " + ex.Message.ToString() + "::" + ex.StackTrace.ToString());
        }
        return objLANGUAGEID;
    }

    public static long GetUTCdatetime_epoch()
    {
        long epoch_time = 0;
        string epoch = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds.ToString();
        //(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        epoch_time = Convert.ToInt64(epoch.Substring(0, epoch.IndexOf(".")));
        return epoch_time;
    }

    public static long GetTomorrowDateInLong()
    {
        long epoch_time = 0;
        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        Int32 year = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddDays(1), INDIAN_ZONE).Year;
        Int32 month = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddDays(1), INDIAN_ZONE).Month;
        Int32 day = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddDays(1), INDIAN_ZONE).Day;
        string epoch = (new DateTime(year, month, day) - new DateTime(1970, 1, 1, 5, 30, 0)).TotalSeconds.ToString();
        epoch_time = Convert.ToInt64(epoch);
        return epoch_time;
    }

    public static long GetUTCdatetime_custom_epoch(DateTime inputdate)
    {
        long epoch_time = 0;
        string epoch = (TimeZone.CurrentTimeZone.ToUniversalTime(inputdate) - new DateTime(1970, 1, 1)).TotalSeconds.ToString();
        //(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        if (epoch.Contains('.'))
        {
            epoch_time = Convert.ToInt64(epoch.Substring(0, epoch.IndexOf(".")));
        }
        else
        {
            epoch_time = Convert.ToInt64(epoch);
        }
        return epoch_time;
    }

    public static DateTime epochToDateTime(long epochTimeStamp)
    {
        // Unix timestamp is milisecond past epoch
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddMilliseconds(epochTimeStamp).ToLocalTime();
        return dtDateTime;
    }

    public static string UnixTimeStampToDateTime(long unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        //System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        //dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        string str_dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime().ToString("MM/dd/yyyy hh:mm:ss.fff tt", CultureInfo.CreateSpecificCulture("en-IN"));
        return str_dtDateTime;
    }

    public static string FormatJsonString(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return string.Empty;
        }

        if (json.StartsWith("["))
        {
            // Hack to get around issue with the older Newtonsoft library
            // not handling a JSON array that contains no outer element.
            json = "{\"list\":" + json + "}";
            var formattedText = JObject.Parse(json).ToString(Formatting.Indented);
            formattedText = formattedText.Substring(13, formattedText.Length - 14).Replace("\n  ", "\n");
            return formattedText;
        }
        return JObject.Parse(json).ToString(Formatting.Indented);
    }

    /// <summary>
    /// Gets a random invoice number to be used with a sample request that requires an invoice number.
    /// </summary>
    /// <returns>A random invoice number in the range of 0 to 999999</returns>
    public static string GetRandomInvoiceNumber()
    {
        return new Random().Next(999999).ToString();
    }

    public static string call_API(string url, Dictionary<string, string> postParameters, string objAccess_Token)
    {
        string postData = "";
        string responseData = "";
        foreach (string key in postParameters.Keys)
        {
            postData += HttpUtility.UrlEncode(key) + "="
                  + HttpUtility.UrlEncode(postParameters[key]) + "&";
        }

        try
        {
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            myHttpWebRequest.Method = "POST";

            byte[] data = Encoding.ASCII.GetBytes(postData);

            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
            myHttpWebRequest.ContentLength = data.Length;
            if (objAccess_Token != "")
            {
                myHttpWebRequest.Headers.Add("Authorization", "Bearer " + objAccess_Token);
            }
            Stream requestStream = myHttpWebRequest.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();

            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

            Stream responseStream = myHttpWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

            responseData = myStreamReader.ReadToEnd();

            myStreamReader.Close();
            responseStream.Close();
            myHttpWebResponse.Close();
        }
        catch (WebException web)
        {
            HttpWebResponse res = web.Response as HttpWebResponse;
            Stream s = res.GetResponseStream();
            string message;
            using (StreamReader sr = new StreamReader(s))
            {
                message = sr.ReadToEnd();
                //Response.Write(message);
                responseData = "Error: " + message;
            }
        }
        catch (Exception ex)
        {
            //Response.Write(ex.Message);
            responseData = "Error: " + ex.Message.ToString();
        }
        return responseData;
    }

    public static string call_API_GET(string url)
    {
        string responseData = "";
        try
        {
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            myHttpWebRequest.Method = "GET";

            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";

            myHttpWebRequest.Headers.Add("X-Api-Key", ConfigurationManager.AppSettings["instamojo_apikey"]);
            myHttpWebRequest.Headers.Add("X-Auth-Token", ConfigurationManager.AppSettings["instamojo_authtoken"]);


            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

            Stream responseStream = myHttpWebResponse.GetResponseStream();

            StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

            responseData = myStreamReader.ReadToEnd();

            myStreamReader.Close();
            responseStream.Close();
            myHttpWebResponse.Close();
        }
        catch (WebException web)
        {
            HttpWebResponse res = web.Response as HttpWebResponse;
            Stream s = res.GetResponseStream();
            string message;
            using (StreamReader sr = new StreamReader(s))
            {
                message = sr.ReadToEnd();
                //Response.Write(message);
                responseData = "Error: " + message;
            }
        }
        catch (Exception ex)
        {
            //Response.Write(ex.Message);
            responseData = "Error: " + ex.Message.ToString();
        }
        return responseData;
    }

    public interface ICredentialsValidator
    {
        bool IsValid(Int32 creds);
    }

    public interface ITokenBuilder
    {
        string Build(Int32 creds);
    }

    public interface ITokenValidator
    {
        bool IsValid(string token, Int32 regid);
    }

    public interface ITokenValidator_mdevices
    {
        bool IsValid(string token, Int32 regid);
    }

    public static string transid(Int32 REGISTRATIONID)
    {
        string txnid = string.Empty;
        Random rnd = new Random();
        string strHash = Generatehash512(rnd.ToString() + REGISTRATIONID.ToString() + DateTime.Now.ToString() + DateTime.Now.Millisecond.ToString());
        txnid = strHash.ToString().Substring(0, 20);
        System.Threading.Thread.Sleep(10);
        return txnid;
    }

    public static string Generatehash512(string text)
    {

        byte[] message = Encoding.UTF8.GetBytes(text);

        UnicodeEncoding UE = new UnicodeEncoding();
        byte[] hashValue;
        SHA512Managed hashString = new SHA512Managed();
        string hex = "";
        hashValue = hashString.ComputeHash(message);
        foreach (byte x in hashValue)
        {
            hex += String.Format("{0:x2}", x);
        }
        return hex;

    }

    //Password encryption
    public static string Password_Encrypt(string PASSWORD)
    {
        //Initialize Logger
        log4net.Config.XmlConfigurator.Configure();
        //Initialize Logger

        string response_value = "";
        try
        {   
            string sPassKey = objDb.PassKeyValue;
            string sCryptedPassword = objDb.CreatePasswordHash(PASSWORD);
            response_value = sCryptedPassword;
            //return response_value;
        }
        catch (Exception ex)
        {
            logger.Error("Common.cs/Password_Encrypt: " + ex.Message.ToString());
            //return "Error: " + ex.Message.ToString();
        }
        return response_value;

    }

}
