using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using log4net;

namespace GameConsole
{
    public class DBFunctions
    {
        static string enc_deccodekey = "!@12345AaxzZ$#9870";
        public static string data_key = "!@12345AaxzZ#9870";
        public string con = AES_Decrypt(System.Configuration.ConfigurationManager.AppSettings["dsn_SQL"], enc_deccodekey);

        //  Codes for Generating Password
        public static string PassKey = "kAdNB6tsP2l4sA==";
        public static string PassChars = "01234AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz56789";

        public ILog logger = log4net.LogManager.GetLogger("ErrorLog");

        public string ENC_REGID(Int32 REGISTRATIONID)
        {
            string epoch = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds.ToString();
            epoch = epoch.Substring(0, epoch.IndexOf("."));
            string final_input = REGISTRATIONID.ToString() + "/" + epoch;
            return AES_Encrypt(final_input, enc_deccodekey);
        }

        public Int32 DEC_REGID(string REGISTRATIONID)
        {
            string[] obj_DecValue = AES_Decrypt(REGISTRATIONID, enc_deccodekey).Split('/');
            return Convert.ToInt32(obj_DecValue[0]);
        }



        public string PassKeyValue
        {
            get { return PassKey; }
        }

        //  CreatePasswordHash Function Definition
        public string CreatePasswordHash(string pwd)
        {

            string saltAndPwd = String.Concat(pwd, PassKey);
            string hashedPwd = FormsAuthentication.HashPasswordForStoringInConfigFile(saltAndPwd, "sha1");

            //  Function Returning Hashed Password
            return hashedPwd;
        }

        //Constructor
        public DBFunctions()
        {
            //
        }

        public string FB_IMAGE_URL(string FBID)
        {
            string FB_IMG_URL = "";
            if (!string.IsNullOrEmpty(FBID))
            {
                FB_IMG_URL = "https://graph.facebook.com/" + FBID + "/picture?width=120&height=120";
            }
            return FB_IMG_URL;
        }

        public string FB_IMAGE_URL_BIG(string FBID)
        {
            string FB_IMG_URL = "";
            if (!string.IsNullOrEmpty(FBID))
            {
                FB_IMG_URL = "https://graph.facebook.com/" + FBID + "/picture?width=512&height=512";
            }
            return FB_IMG_URL;
        }

        #region "Other Function/Calculations"
        // For Encryption/Decryption Start
        public static string AES_Decrypt(string input, string pass)
        {
            System.Security.Cryptography.RijndaelManaged AES = new System.Security.Cryptography.RijndaelManaged();
            System.Security.Cryptography.MD5CryptoServiceProvider Hash_AES = new System.Security.Cryptography.MD5CryptoServiceProvider();
            string decrypted = "";
            try
            {
                byte[] hash = new byte[32];
                byte[] temp = Hash_AES.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(pass));
                Array.Copy(temp, 0, hash, 0, 16);
                Array.Copy(temp, 0, hash, 15, 16);
                AES.Key = hash;
                AES.Mode = System.Security.Cryptography.CipherMode.ECB;
                System.Security.Cryptography.ICryptoTransform DESDecrypter = AES.CreateDecryptor();
                byte[] Buffer = Convert.FromBase64String(input);
                decrypted = System.Text.ASCIIEncoding.ASCII.GetString(DESDecrypter.TransformFinalBlock(Buffer, 0, Buffer.Length));
                return decrypted;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string AES_Encrypt(string input, string pass)
        {
            System.Security.Cryptography.RijndaelManaged AES = new System.Security.Cryptography.RijndaelManaged();
            System.Security.Cryptography.MD5CryptoServiceProvider Hash_AES = new System.Security.Cryptography.MD5CryptoServiceProvider();
            string encrypted = "";
            try
            {
                byte[] hash = new byte[32];
                byte[] temp = Hash_AES.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes(pass));
                Array.Copy(temp, 0, hash, 0, 16);
                Array.Copy(temp, 0, hash, 15, 16);
                AES.Key = hash;
                AES.Mode = System.Security.Cryptography.CipherMode.ECB;
                System.Security.Cryptography.ICryptoTransform DESEncrypter = AES.CreateEncryptor();
                byte[] Buffer = System.Text.ASCIIEncoding.ASCII.GetBytes(input);
                encrypted = Convert.ToBase64String(DESEncrypter.TransformFinalBlock(Buffer, 0, Buffer.Length));
                return encrypted;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        // For Encryption/Decryption End
        public DataSet senddataset_Parameter(string stored_text, Hashtable parameters)
        {
            DataSet functionReturnValue = null;
            SqlConnection connection = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand(stored_text, connection);
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataSet dataset = new DataSet();
                DictionaryEntry Item = default(DictionaryEntry);
                cmd.CommandType = CommandType.Text;
                foreach (DictionaryEntry Item_loopVariable in parameters)
                {
                    Item = Item_loopVariable;
                    cmd.Parameters.AddWithValue(Item.Key.ToString(), Item.Value.ToString());
                }
                cmd.Connection.Open();
                adapter.SelectCommand = cmd;
                adapter.Fill(dataset);
                functionReturnValue = dataset;
                cmd.Connection.Close();
            }
            catch (Exception err)
            {
                cmd.Connection.Close();
                throw (err);
            }
            return functionReturnValue;
        }

        public void ExecuteQry_Parameter(string stored_text, Hashtable parameters)
        {
            SqlConnection connection = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand(stored_text, connection);
            try
            {
                DictionaryEntry Item = default(DictionaryEntry);
                cmd.CommandType = CommandType.Text;
                foreach (DictionaryEntry Item_loopVariable in parameters)
                {
                    Item = Item_loopVariable;
                    cmd.Parameters.AddWithValue(Item.Key.ToString(), Item.Value.ToString());
                }
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception err)
            {
                cmd.Connection.Close();
                throw (err);
            }
        }

        public object SendValue_Parameter(string stored_text, Hashtable parameters)
        {
            object functionReturnValue = null;
            SqlConnection connection = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand(stored_text, connection);
            try
            {
                DictionaryEntry Item = default(DictionaryEntry);
                cmd.CommandType = CommandType.Text;
                foreach (DictionaryEntry Item_loopVariable in parameters)
                {
                    Item = Item_loopVariable;
                    cmd.Parameters.AddWithValue(Item.Key.ToString(), Item.Value.ToString());

                }
                cmd.Connection.Open();
                functionReturnValue = cmd.ExecuteScalar();
                cmd.Connection.Close();
            }
            catch (Exception err)
            {
                cmd.Connection.Close();
                throw (err);
            }
            return functionReturnValue;
        }

        //public object SendValue_Parameter(string stored_text, ConcurrentDictionary<string, object> parameters)
        //{
        //    object functionReturnValue = null;
        //    SqlConnection connection = new SqlConnection(con);
        //    SqlCommand cmd = new SqlCommand(stored_text, connection);
        //    try
        //    {
        //        DictionaryEntry Item = default(DictionaryEntry);
        //        cmd.CommandType = CommandType.Text;
        //        foreach (DictionaryEntry Item_loopVariable in parameters.Values)
        //        {
        //            Item = Item_loopVariable;
        //            cmd.Parameters.AddWithValue(Item.Key.ToString(), Item.Value.ToString());

        //        }
        //        cmd.Connection.Open();
        //        functionReturnValue = cmd.ExecuteScalar();
        //        cmd.Connection.Close();
        //    }
        //    catch (Exception err)
        //    {
        //        cmd.Connection.Close();
        //        throw (err);
        //    }
        //    return functionReturnValue;
        //}

        public DataSet senddataset_SP(string stored_proc, Hashtable parameters)
        {
            DataSet functionReturnValue = null;
            SqlConnection connection = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand(stored_proc, connection);
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                DataSet dataset = new DataSet();
                DictionaryEntry Item = default(DictionaryEntry);
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (DictionaryEntry Item_loopVariable in parameters)
                {
                    Item = Item_loopVariable;
                    cmd.Parameters.AddWithValue(Item.Key.ToString(), Item.Value);
                }
                cmd.Connection.Open();
                adapter.SelectCommand = cmd;
                adapter.Fill(dataset);
                functionReturnValue = dataset;
                cmd.Connection.Close();
            }
            catch (Exception err)
            {
                cmd.Connection.Close();
                throw (err);
            }
            return functionReturnValue;
        }

        public void ExecuteQry_SP(string stored_proc, Hashtable parameters)
        {
            SqlConnection connection = new System.Data.SqlClient.SqlConnection(con);
            SqlCommand cmd = new SqlCommand(stored_proc, connection);
            try
            {
                DictionaryEntry Item = default(DictionaryEntry);
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (DictionaryEntry Item_loopVariable in parameters)
                {
                    Item = Item_loopVariable;
                    //cmd.Parameters.AddWithValue(Item.Key, Item.Value);
                    cmd.Parameters.AddWithValue(Item.Key.ToString(), Item.Value);
                }
                if (cmd.Connection.State == ConnectionState.Open || cmd.Connection.State == ConnectionState.Broken)
                {
                    cmd.Connection.Close();
                }
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ERR)
            {
                throw (ERR);
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        public object SendValue_SP(string stored_proc, Hashtable parameters)
        {
            object functionReturnValue = null;
            SqlConnection connection = new SqlConnection(con);
            SqlCommand cmd = new SqlCommand(stored_proc, connection);
            try
            {
                DictionaryEntry Item = default(DictionaryEntry);
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (DictionaryEntry Item_loopVariable in parameters)
                {
                    Item = Item_loopVariable;
                    cmd.Parameters.AddWithValue(Item.Key.ToString(), Item.Value.ToString());
                }
                cmd.Connection.Open();
                functionReturnValue = cmd.ExecuteScalar();
                cmd.Connection.Close();
            }
            catch (Exception err)
            {
                cmd.Connection.Close();
                throw (err);
            }
            return functionReturnValue;
        }
        // Save Email history
        public void savemail(string mailid, string SenderId, string recepientID, string ccmail, string bccmail, string strMessageSubject, string strMessageBody, Int32 userid = 0)
        {
            //Initialize Logger
            log4net.Config.XmlConfigurator.Configure();
            //Initialize Logger

            try
            {

                Hashtable parameters = new Hashtable();
                parameters.Add("@TRID", 0);
                parameters.Add("@REGID", userid);
                parameters.Add("@MAILID", mailid);
                parameters.Add("@SUBJECT", strMessageSubject.Trim());
                parameters.Add("@FROM_MAIL", SenderId.Trim());
                parameters.Add("@TO_MAIL", recepientID.Trim());
                parameters.Add("@CC_MAIL", ccmail.Trim());
                parameters.Add("@BCC_MAIL", bccmail.Trim());
                parameters.Add("@BODY", strMessageBody.Trim());
                parameters.Add("@SIGNATURE", string.Empty);
                parameters.Add("@MODE", "Insert");
                ExecuteQry_SP("UDSP_MAIL_HISTORY", parameters);

            }
            catch (Exception ex)
            {
                logger.Error("DBFunctions.cs/savemail: " + ex.Message.ToString() + "::" + ex.StackTrace.ToString());               
            }
        }
        public bool sendSMS(string number, string msg)
        {
            bool flag = true;
            try
            {
                WebRequest request = WebRequest.Create("http://nimbusit.co.in/api/swsendSingle.asp?username=" + ConfigurationManager.AppSettings["loginid"].ToString() + "&password=" + ConfigurationManager.AppSettings["loginpwd"].ToString() + "&sender=" + ConfigurationManager.AppSettings["sender"].ToString() + "&sendto=91" + number + "&message=" + HttpUtility.UrlEncode(msg));
                request.Credentials = CredentialCache.DefaultCredentials;
                request.Timeout = 1000; //1 sec timeout

                // Get the response.
                WebResponse response = request.GetResponse();
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                // Clean up the streams and the response.
                reader.Close();
                response.Close();

                // Response.Write(responseFromServer);
            }
            catch (Exception)
            {

                flag = false;
            }

            return flag;
        }

        public bool sendSMS_v1(string number, string msg)
        {
            bool flag = true;
            try
            {
                WebRequest request = WebRequest.Create("http://203.212.70.200/smpp/sendsms?username=" + ConfigurationManager.AppSettings["loginidv1"].ToString() + "&password=" + ConfigurationManager.AppSettings["loginpwdv1"].ToString() + "&from=" + ConfigurationManager.AppSettings["senderv1"].ToString() + "&to=91" + number + "&text=" + HttpUtility.UrlEncode(msg));
                request.Credentials = CredentialCache.DefaultCredentials;

                // Get the response.
                WebResponse response = request.GetResponse();
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                // Clean up the streams and the response.
                reader.Close();
                response.Close();

                // Response.Write(responseFromServer);
            }
            catch (Exception)
            {

                flag = false;
            }

            return flag;
        }

        //*******Updated by Prakash to send mail on any internet provider(like gmail, yahoo etc)      
        public bool Sendmail(string SenderId, string SenderName, string recepientID, string ccmail, string bccmail, string strMessageSubject, string strMessageBody, string strAattachment = "")
        {
            //Initialize Logger
            log4net.Config.XmlConfigurator.Configure();
            //Initialize Logger

            bool Status = false;
            try
            {

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(SenderId, SenderName);
                msg.To.Add(recepientID);
                if (!string.IsNullOrEmpty(ccmail.Trim()))
                {
                    msg.CC.Add(ccmail.Trim());
                }
                if (!string.IsNullOrEmpty(bccmail.Trim()))
                {
                    msg.Bcc.Add(bccmail.Trim());
                }
                // Attachments
                if (strAattachment != "")
                {
                    string[] AttachArray = strAattachment.Split(',');
                    List<string> AttachList = new List<string>(AttachArray.Length);
                    AttachList.AddRange(AttachArray);

                    for (Int32 i1 = 0; i1 < AttachList.Count; i1++)
                    {
                        msg.Attachments.Add(new Attachment(ConfigurationManager.AppSettings["COUPON_PATH"].ToString() + AttachList[i1].ToString()));
                    }
                }
                //if (!string.IsNullOrEmpty(strAattachment))
                //{
                //    msg.Attachments.Add(new Attachment(strAattachment.Trim()));
                //}

                msg.Subject = strMessageSubject;
                msg.Body = strMessageBody;
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.High;
                //// Local
                //SmtpClient client = new SmtpClient("192.168.100.33");
                //client.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                //// End Local
                //Live
                SmtpClient client = new SmtpClient();
                client.Host = ConfigurationManager.AppSettings["SmtpServer"].ToString();
                client.Port = 25;
                //client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["authMail"].ToString(), ConfigurationManager.AppSettings["authPassword"].ToString());
                client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["authMail"].ToString(), "As3ER710");
                //client.EnableSsl = true;
                // End Live
                client.Send(msg);
                Status = true;
            }
            catch (SmtpException ex)
            {
                logger.Error("DBFunctions.cs/Sendmail: " + ex.Message.ToString() + "::" + ex.StackTrace.ToString());
                Status = false;
                return Status;
            }
            catch (Exception ex)
            {
                logger.Error("DBFunctions.cs/Sendmail_E: " + ex.Message.ToString() + "::" + ex.StackTrace.ToString());
                Status = false;
                return Status;
            }
            return Status;
        }

        #endregion
        
        public string Verify_FBToken(string ACCESS_TOKEN)
        {
            string objResponse = string.Empty;
            //Initialize Logger
            log4net.Config.XmlConfigurator.Configure();
            //Initialize Logger
            try
            {
                //Use below code after first step where we generate code from api
                String uri = "https://graph.facebook.com/me?access_token=" + ACCESS_TOKEN;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "POST";
                //request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes("ecomexpress" + ":" + "Ke$3c@4oT5m6h#$")));

                string postData = "";

                byte[] data = Encoding.ASCII.GetBytes(postData);
                request.ContentType = "application/x-www-form-urlencoded";
                //request.Headers.Add("Authorization: Bearer");
                request.ContentLength = data.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();

                HttpWebResponse myHttpWebResponse = (HttpWebResponse)request.GetResponse();

                Stream responseStream = myHttpWebResponse.GetResponseStream();

                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.Default);

                string pageContent = myStreamReader.ReadToEnd();

                myStreamReader.Close();
                responseStream.Close();

                myHttpWebResponse.Close();
                objResponse = pageContent;
                //Response.Write(pageContent);
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    objResponse = reader.ReadToEnd();
                    //logger.Error("DBFunctions.cs/Verify_FBToken: " + objResponse);
                    //Response.Write(reader.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                objResponse = ex.Message.ToString();
                logger.Error("DBFunctions.cs/Verify_FBToken: " + objResponse);
                //Response.Write(ex.Message.ToString());
            }

            return objResponse;
            //Response below
            //Token need to generate every time
            //{ "access_token" : "ya29.Glz5A98keywzQnS0cW8swosoipCh9gGmvAvYr6m6HaLXaIxOsUpKVoG_NDQWvWY_a1k81gyDmiM6FYPGMAE_hVqLX8i-UV1uV2avTocS4SzQQmcmUSqNmXSOs3JJ8A", "expires_in" : 3600, "token_type" : "Bearer" }
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

        public static string SpecialCharaterReplace(string value)
        {
            string lvalue = Regex.Replace(value, @"[^a-zA-Z0-9.]", "");
            return lvalue;
        }

        public string SpecialCharaterReplace_onlyalpha(string value)
        {
            string lvalue = Regex.Replace(value, @"[^a-zA-Z0-9]", "");
            return lvalue;
        }
    }
   
}