using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace GameConsole.Classes
{
    public static class NonGenericCL
    {
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props =
            TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        public static string UnixTimeStampToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            //System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            //dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            string str_dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime().ToString("MM/dd/yyyy hh:mm:ss tt");
            return str_dtDateTime;
        }

        public static long DateTimeToUnixTimeStamp(DateTime inputdate)
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

        public static dynamic PvtTable_Transaction(string MOBILE, string TRANSACTIONID)
        {
            ILog logger = log4net.LogManager.GetLogger("ErrorLog");

            try
            {
                //Initialize Logger
                log4net.Config.XmlConfigurator.Configure();
                //Initialize Logger

                string url = ConfigurationManager.AppSettings["GameSpark_URL"] + MOBILE + "&transactionid=" + TRANSACTIONID;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                //logger.Error("RequestPayTM: " + url);

                request.Headers.Add("ContentType", "application/text");
                request.Method = "GET";

                string responseData = string.Empty;

                using (StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream()))
                {
                    responseData = responseReader.ReadToEnd();
                    //logger.Error("ResponsePVT: " + responseData);
                }

                //Response.Write("<br>" + "<br>" + "Response::" + responseData);
                dynamic rdata = JObject.Parse(responseData);
                return rdata;
            }
            catch (WebException web)
            {
                HttpWebResponse res = web.Response as HttpWebResponse;
                Stream s = res.GetResponseStream();
                dynamic message;
                using (StreamReader sr = new StreamReader(s))
                {
                    message = sr.ReadToEnd();
                    logger.Error("ResponsePVTWEBerror: " + message);
                    return message;
                }
            }
            catch (Exception ex)
            {
                dynamic objerror = ex.Message.ToString();
                logger.Error("ResponsePVTerror: " + objerror);
                return objerror;
            }
        }

        public static dynamic PvtTable_ValidateToken(string MOBILE)
        {
            ILog logger = log4net.LogManager.GetLogger("ErrorLog");
            try
            {
                //Initialize Logger
                log4net.Config.XmlConfigurator.Configure();
                //Initialize Logger

                string url = ConfigurationManager.AppSettings["GameSpark_TokenValidateURL"] + MOBILE;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                //logger.Error("RequestPayTM: " + url);

                request.Headers.Add("ContentType", "application/text");
                request.Method = "GET";

                string responseData = string.Empty;

                using (StreamReader responseReader = new StreamReader(request.GetResponse().GetResponseStream()))
                {
                    responseData = responseReader.ReadToEnd();
                    //logger.Error("ResponsePVT: " + responseData);
                }

                //Response.Write("<br>" + "<br>" + "Response::" + responseData);
                dynamic rdata = JObject.Parse(responseData);
                return rdata;
            }
            catch (WebException web)
            {
                HttpWebResponse res = web.Response as HttpWebResponse;
                Stream s = res.GetResponseStream();
                dynamic message;
                using (StreamReader sr = new StreamReader(s))
                {
                    message = sr.ReadToEnd();
                    //logger.Error("ResponsePVTWEBerror: " + message);
                    return message;
                }
            }
            catch (Exception ex)
            {
                dynamic objerror = ex.Message.ToString();
                //logger.Error("ResponsePVTerror: " + objerror);
                return objerror;
            }
        }
    }

    public class Users
    {
        public string _id { get; set; }
        public string tid { get; set; }
        public long ts { get; set; }
        public string mode { get; set; }
        public double amount { get; set; }
    }
}