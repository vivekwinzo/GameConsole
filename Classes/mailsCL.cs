using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Data;
using System.Security;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using System.Web;
using System.ServiceModel.Channels;
using System.Net;
using System.Configuration;
using System.Web.UI;
using System.Web.Security;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using GameConsole.Classes;
using log4net;

namespace GameConsole.Classes
{
    public class mailsCL
    {
        DBFunctions objDb = new DBFunctions();
        Hashtable parameters = new Hashtable();
        ILog logger = log4net.LogManager.GetLogger("ErrorLog");

        //Winner Vouchers
        public void send_mail_29(Int32 WINNERID)
        {
            return;
            //Initialize Logger
            log4net.Config.XmlConfigurator.Configure();
            //Initialize Logger

            try
            {
                //Declare objects
                string SenderId = string.Empty;
                string SenderName = string.Empty;
                string recepientID = string.Empty;
                string ccmail = string.Empty;
                string bccmail = string.Empty;
                string strMessageSubject = string.Empty;
                string strMessageBody = string.Empty;
                string strAattachment = string.Empty;
                string useremail = string.Empty;
                Int32 struserid = 0;
                //Declare objects

                //Find user email
                parameters.Clear();
                parameters.Add("@WINNERID", WINNERID);
                DataSet obj_Receiver = objDb.senddataset_Parameter("SELECT REG.EMAIL, W.REGISTRATIONID FROM STBL_REGISTRATION REG WITH(NOLOCK) INNER JOIN STBL_WINNERS W WITH(NOLOCK) ON W.REGISTRATIONID=REG.REGISTRATIONID WHERE W.WINNERID=@WINNERID", parameters);

                if (obj_Receiver.Tables[0].Rows.Count > 0)
                {
                    useremail = HttpUtility.HtmlDecode(obj_Receiver.Tables[0].Rows[0]["EMAIL"].ToString());

                    if (useremail == "") //No mail id found
                    {
                        return;
                    }
                    DataSet objDataSet = new DataSet();
                    parameters.Clear();
                    parameters.Add("@MAILID", 29);
                    objDataSet = objDb.senddataset_Parameter("SELECT FROM_MAIL, FROM_DISPLAY_NAME, CC_MAIL, BCC_MAIL, SUBJECT FROM STBL_MAIL_MASTER WITH(NOLOCK) WHERE MAILID=@MAILID", parameters);
                    if (objDataSet.Tables[0].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["dummy_ToMail"].ToString()))
                        {
                            recepientID = HttpUtility.HtmlDecode(ConfigurationManager.AppSettings["dummy_ToMail"].ToString());
                        }
                        else
                        {
                            recepientID = useremail;
                        }
                        SenderId = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["FROM_MAIL"].ToString());
                        SenderName = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["FROM_DISPLAY_NAME"].ToString());
                        ccmail = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["CC_MAIL"].ToString());
                        bccmail = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["BCC_MAIL"].ToString());
                        strMessageSubject = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["SUBJECT"].ToString());
                        struserid = Convert.ToInt32(obj_Receiver.Tables[0].Rows[0]["REGISTRATIONID"].ToString());

                        StringWriter sw = new StringWriter();
                        HtmlTextWriter hWriter = new HtmlTextWriter(sw);

                        //  HttpContext.Current.Server.Execute("~/mail/delivered.aspx?winnerid=" + WINNERID, hWriter, true);                
                        HttpContext.Current.Server.Execute("~/mails/email.aspx?wid=" + WINNERID + "&mailid=29", hWriter, true);

                        strMessageBody = sw.ToString();
                        //System.Threading.Thread.Sleep(100);
                        //Create file name to find generated as PDF
                        parameters.Clear();
                        parameters.Add("@WINNERID", WINNERID);
                        strAattachment = Convert.ToString(objDb.SendValue_Parameter("SELECT STUFF((SELECT ',' + COUPON_CODE + '.pdf' AS [text()] FROM  STBL_DEALS_COUPONS WITH(NOLOCK) WHERE WINNERID=@WINNERID AND WINNERTYPE='WINNER' For XML PATH ('')),1,1,'') ", parameters));
                        //SEND MAIL
                        bool status = objDb.Sendmail(SenderId, SenderName, recepientID, ccmail, bccmail, strMessageSubject, strMessageBody, strAattachment);
                        //System.Threading.Thread.Sleep(100);
                        //if (status == true)
                        //{
                        //    objDb.savemail("29", SenderId, recepientID, ccmail, bccmail, strMessageSubject, strMessageBody, struserid);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("mailsCL.cs/send_mail_29: " + ex.Message.ToString() + "::" + ex.StackTrace.ToString());
            }
        }

        // Redeem vouchers
        public void send_mail_30(Int32 REDEEMID)
        {
            return;
            //Initialize Logger
            log4net.Config.XmlConfigurator.Configure();
            //Initialize Logger

            try
            {
                //Declare objects
                string SenderId = string.Empty;
                string SenderName = string.Empty;
                string recepientID = string.Empty;
                string ccmail = string.Empty;
                string bccmail = string.Empty;
                string strMessageSubject = string.Empty;
                string strMessageBody = string.Empty;
                string strAattachment = string.Empty;
                string useremail = string.Empty;
                Int32 struserid = 0;
                //Declare objects

                //Find user email
                parameters.Clear();
                parameters.Add("@REDEEMID", REDEEMID);
                DataSet obj_Receiver = objDb.senddataset_Parameter("SELECT REG.EMAIL, R.REDEEMID FROM STBL_REGISTRATION REG WITH(NOLOCK) INNER JOIN STBL_REDEEM R WITH(NOLOCK) ON R.REGISTRATIONID=REG.REGISTRATIONID WHERE R.REDEEMID=@REDEEMID", parameters);

                if (obj_Receiver.Tables[0].Rows.Count > 0)
                {
                    useremail = HttpUtility.HtmlDecode(obj_Receiver.Tables[0].Rows[0]["EMAIL"].ToString());

                    if (useremail == "") //No mail id found
                    {
                        return;
                    }

                    DataSet objDataSet = new DataSet();
                    parameters.Clear();
                    parameters.Add("@MAILID", 30);
                    objDataSet = objDb.senddataset_Parameter("SELECT FROM_MAIL, FROM_DISPLAY_NAME, CC_MAIL, BCC_MAIL, SUBJECT FROM STBL_MAIL_MASTER WITH(NOLOCK) WHERE MAILID=@MAILID", parameters);
                    if (objDataSet.Tables[0].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["dummy_ToMail"].ToString()))
                        {
                            recepientID = HttpUtility.HtmlDecode(ConfigurationManager.AppSettings["dummy_ToMail"].ToString());
                        }
                        else
                        {
                            recepientID = useremail;
                        }
                        SenderId = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["FROM_MAIL"].ToString());
                        SenderName = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["FROM_DISPLAY_NAME"].ToString());
                        ccmail = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["CC_MAIL"].ToString());
                        bccmail = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["BCC_MAIL"].ToString());
                        strMessageSubject = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["SUBJECT"].ToString());
                        struserid = Convert.ToInt32(obj_Receiver.Tables[0].Rows[0]["REDEEMID"].ToString());

                        StringWriter sw = new StringWriter();
                        HtmlTextWriter hWriter = new HtmlTextWriter(sw);

                        //  HttpContext.Current.Server.Execute("~/mail/delivered.aspx?winnerid=" + WINNERID, hWriter, true);                
                        HttpContext.Current.Server.Execute("~/mails/email.aspx?rid=" + REDEEMID + "&mailid=30", hWriter, true);

                        strMessageBody = sw.ToString();
                        //System.Threading.Thread.Sleep(100);
                        //Create file name to find generated as PDF
                        parameters.Clear();
                        parameters.Add("@WINNERID", REDEEMID);
                        strAattachment = Convert.ToString(objDb.SendValue_Parameter("SELECT STUFF((SELECT ',' + COUPON_CODE + '.pdf' AS [text()] FROM  STBL_DEALS_COUPONS WITH(NOLOCK) WHERE WINNERID=@WINNERID AND WINNERTYPE='REDEEM' For XML PATH ('')),1,1,'') ", parameters));
                        //SEND MAIL
                        bool status = objDb.Sendmail(SenderId, SenderName, recepientID, ccmail, bccmail, strMessageSubject, strMessageBody, strAattachment);
                        //System.Threading.Thread.Sleep(100);
                        //if (status == true)
                        //{
                        //    objDb.savemail("30", SenderId, recepientID, ccmail, bccmail, strMessageSubject, strMessageBody, struserid);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("mailsCL.cs/send_mail_30: " + ex.Message.ToString() + "::" + ex.StackTrace.ToString());
            }
        }

        // Mail Send To User For Referral used
        public void send_mail_36(Int32 REGISTRATIONID, Int32 REFER_BY)
        {
            return;
            //Initialize Logger
            log4net.Config.XmlConfigurator.Configure();
            //Initialize Logger

            try
            {
                string SenderId = string.Empty;
                string SenderName = string.Empty;
                string recepientID = string.Empty;
                string ccmail = string.Empty;
                string bccmail = string.Empty;
                string strMessageSubject = string.Empty;
                string strMessageBody = string.Empty;
                string useremail = string.Empty;
                Hashtable parameters = new Hashtable();
                parameters.Add("@REGISTRATIONID", REFER_BY);
                useremail = Convert.ToString(objDb.SendValue_Parameter("SELECT EMAIL FROM STBL_REGISTRATION WITH(NOLOCK) WHERE REGISTRATIONID=@REGISTRATIONID", parameters));
                if (useremail == "") //No mail id found
                {
                    return;
                }
                DataSet objDataSet = new DataSet();
                parameters.Clear();
                parameters.Add("@MAILID", 36);
                objDataSet = objDb.senddataset_Parameter("SELECT FROM_MAIL, FROM_DISPLAY_NAME, CC_MAIL, BCC_MAIL, SUBJECT FROM STBL_MAIL_MASTER WITH(NOLOCK) WHERE MAILID=@MAILID", parameters);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["dummy_ToMail"].ToString()))
                    {
                        recepientID = ConfigurationManager.AppSettings["dummy_ToMail"].ToString();
                    }
                    else
                    {
                        recepientID = useremail.Trim();
                    }
                    SenderId = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["FROM_MAIL"].ToString());
                    SenderName = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["FROM_DISPLAY_NAME"].ToString());
                    ccmail = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["CC_MAIL"].ToString());
                    bccmail = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["BCC_MAIL"].ToString());
                    strMessageSubject = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["SUBJECT"].ToString());
                }

                StringWriter sw = new StringWriter();
                HtmlTextWriter hWriter = new HtmlTextWriter(sw);
                HttpContext.Current.Server.Execute("~/mails/email.aspx?rid=" + REGISTRATIONID + "&mailid=36", hWriter, true);

                strMessageBody = sw.ToString();
                //System.Threading.Thread.Sleep(100);
                bool status = objDb.Sendmail(SenderId, SenderName, recepientID, ccmail, bccmail, strMessageSubject, strMessageBody);
                //if (status == true)
                //{
                //    objDb.savemail("36", SenderId, recepientID, ccmail, bccmail, strMessageSubject, strMessageBody, REFER_BY);
                //}
            }
            catch (Exception ex)
            {
                logger.Error("mailsCL.cs/send_mail_36: " + ex.Message.ToString() + "::" + ex.StackTrace.ToString());
            }

        }

        // Mail Send To User For Registration successful
        public void send_mail_15(Int32 REGISTRATIONID)
        {
            return;
            //Initialize Logger
            log4net.Config.XmlConfigurator.Configure();
            //Initialize Logger

            try
            {
                string SenderId = string.Empty;
                string SenderName = string.Empty;
                string recepientID = string.Empty;
                string ccmail = string.Empty;
                string bccmail = string.Empty;
                string strMessageSubject = string.Empty;
                string strMessageBody = string.Empty;
                string useremail = string.Empty;
                Hashtable parameters = new Hashtable();
                parameters.Add("@REGISTRATIONID", REGISTRATIONID);
                useremail = Convert.ToString(objDb.SendValue_Parameter("SELECT EMAIL FROM STBL_REGISTRATION WITH(NOLOCK) WHERE REGISTRATIONID=@REGISTRATIONID", parameters));
                if (useremail == "") //No mail id found
                {
                    return;
                }
                DataSet objDataSet = new DataSet();
                parameters.Clear();
                parameters.Add("@MAILID", 15);
                objDataSet = objDb.senddataset_Parameter("SELECT FROM_MAIL, FROM_DISPLAY_NAME, CC_MAIL, BCC_MAIL, SUBJECT FROM STBL_MAIL_MASTER WITH(NOLOCK) WHERE MAILID=@MAILID", parameters);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["dummy_ToMail"].ToString()))
                    {
                        recepientID = ConfigurationManager.AppSettings["dummy_ToMail"].ToString();
                    }
                    else
                    {
                        recepientID = useremail.Trim();
                    }
                    SenderId = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["FROM_MAIL"].ToString());
                    SenderName = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["FROM_DISPLAY_NAME"].ToString());
                    ccmail = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["CC_MAIL"].ToString());
                    bccmail = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["BCC_MAIL"].ToString());
                    strMessageSubject = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["SUBJECT"].ToString());
                }

                StringWriter sw = new StringWriter();
                HtmlTextWriter hWriter = new HtmlTextWriter(sw);
                HttpContext.Current.Server.Execute("~/mails/email.aspx?rid=" + REGISTRATIONID + "&mailid=15", hWriter, true); 

                strMessageBody = sw.ToString();
                //System.Threading.Thread.Sleep(100);
                bool status = objDb.Sendmail(SenderId, SenderName, recepientID, ccmail, bccmail, strMessageSubject, strMessageBody);
                //if (status == true)
                //{
                //    objDb.savemail("15", SenderId, recepientID, ccmail, bccmail, strMessageSubject, strMessageBody, REGISTRATIONID);
                //}
            }
            catch (Exception ex)
            {
                logger.Error("mailsCL.cs/send_mail_15: " + ex.Message.ToString() + "::" + ex.StackTrace.ToString());
            }

        }

        // Mail Send To User For Mobile Number Changed	
        public void send_mail_5(Int32 REGISTRATIONID)
        {
            return;
            //Initialize Logger
            log4net.Config.XmlConfigurator.Configure();
            //Initialize Logger

            try
            {
                string SenderId = string.Empty;
                string SenderName = string.Empty;
                string recepientID = string.Empty;
                string ccmail = string.Empty;
                string bccmail = string.Empty;
                string strMessageSubject = string.Empty;
                string strMessageBody = string.Empty;
                string useremail = string.Empty;
                Hashtable parameters = new Hashtable();
                parameters.Add("@REGISTRATIONID", REGISTRATIONID);
                useremail = Convert.ToString(objDb.SendValue_Parameter("SELECT EMAIL FROM STBL_REGISTRATION WITH(NOLOCK) WHERE REGISTRATIONID=@REGISTRATIONID", parameters));
                if (useremail == "") //No mail id found
                {
                    return;
                }
                DataSet objDataSet = new DataSet();
                parameters.Clear();
                parameters.Add("@MAILID", 5);
                objDataSet = objDb.senddataset_Parameter("SELECT FROM_MAIL, FROM_DISPLAY_NAME, CC_MAIL, BCC_MAIL, SUBJECT FROM STBL_MAIL_MASTER WITH(NOLOCK) WHERE MAILID=@MAILID", parameters);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["dummy_ToMail"].ToString()))
                    {
                        recepientID = ConfigurationManager.AppSettings["dummy_ToMail"].ToString();
                    }
                    else
                    {
                        recepientID = useremail.Trim();
                    }
                    SenderId = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["FROM_MAIL"].ToString());
                    SenderName = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["FROM_DISPLAY_NAME"].ToString());
                    ccmail = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["CC_MAIL"].ToString());
                    bccmail = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["BCC_MAIL"].ToString());
                    strMessageSubject = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["SUBJECT"].ToString());
                }

                StringWriter sw = new StringWriter();
                HtmlTextWriter hWriter = new HtmlTextWriter(sw);
                HttpContext.Current.Server.Execute("~/mails/email.aspx?rid=" + REGISTRATIONID + "&mailid=5", hWriter, true); //5 for Mobile changed
                strMessageBody = sw.ToString();
                //System.Threading.Thread.Sleep(100);
                bool status = objDb.Sendmail(SenderId, SenderName, recepientID, ccmail, bccmail, strMessageSubject, strMessageBody);
                //if (status == true)
                //{
                //    objDb.savemail("5", SenderId, recepientID, ccmail, bccmail, strMessageSubject, strMessageBody, REGISTRATIONID);
                //}
            }
            catch (Exception ex)
            {
                logger.Error("mailsCL.cs/send_mail_5: " + ex.Message.ToString() + "::" + ex.StackTrace.ToString());
            }

        }

        // Mail Send To User For Level Redeemed Voucher
        public void send_mail_33(Int32 REDEEMID)
        {
            return;
            //Initialize Logger
            log4net.Config.XmlConfigurator.Configure();
            //Initialize Logger

            try
            {
                string SenderId = string.Empty;
                string SenderName = string.Empty;
                string recepientID = string.Empty;
                string ccmail = string.Empty;
                string bccmail = string.Empty;
                string strMessageSubject = string.Empty;
                string strMessageBody = string.Empty;
                string useremail = string.Empty;
                parameters.Clear();
                parameters.Add("@REDEEMID", REDEEMID);
                useremail = Convert.ToString(objDb.SendValue_Parameter("SELECT R.EMAIL FROM STBL_REDEEM RD WITH(NOLOCK) JOIN STBL_REGISTRATION R WITH(NOLOCK) ON R.REGISTRATIONID=RD.REGISTRATIONID WHERE RD.REDEEMID=@REDEEMID", parameters));
                if (useremail == "") //No mail id found
                {
                    return;
                }
                DataSet objDataSet = new DataSet();
                parameters.Clear();
                parameters.Add("@MAILID", 33);
                objDataSet = objDb.senddataset_Parameter("SELECT FROM_MAIL, FROM_DISPLAY_NAME, CC_MAIL, BCC_MAIL, SUBJECT FROM STBL_MAIL_MASTER WITH(NOLOCK) WHERE MAILID=@MAILID", parameters);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["dummy_ToMail"].ToString()))
                    {
                        recepientID = ConfigurationManager.AppSettings["dummy_ToMail"].ToString();
                    }
                    else
                    {
                        recepientID = useremail.Trim();
                    }
                    SenderId = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["FROM_MAIL"].ToString());
                    SenderName = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["FROM_DISPLAY_NAME"].ToString());
                    ccmail = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["CC_MAIL"].ToString());
                    bccmail = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["BCC_MAIL"].ToString());
                    strMessageSubject = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["SUBJECT"].ToString());
                }

                StringWriter sw = new StringWriter();
                HtmlTextWriter hWriter = new HtmlTextWriter(sw);
                HttpContext.Current.Server.Execute("~/mails/email.aspx?rid=" + REDEEMID + "&mailid=33", hWriter, true);
                strMessageBody = sw.ToString();
                //System.Threading.Thread.Sleep(100);
                bool status = objDb.Sendmail(SenderId, SenderName, recepientID, ccmail, bccmail, strMessageSubject, strMessageBody);
                //if (status == true)
                //{
                //    objDb.savemail("33", SenderId, recepientID, ccmail, bccmail, strMessageSubject, strMessageBody, REDEEMID);
                //}

            }
            catch (Exception ex)
            {
                logger.Error("mailsCL.cs/send_mail_33: " + ex.Message.ToString() + "::" + ex.StackTrace.ToString());
            }
        }

        // Mail Send To User For Level Redeemed Products
        public void send_mail_8(Int32 REDEEMID)
        {
            return;
            //Initialize Logger
            log4net.Config.XmlConfigurator.Configure();
            //Initialize Logger

            try
            {
                string SenderId = string.Empty;
                string SenderName = string.Empty;
                string recepientID = string.Empty;
                string ccmail = string.Empty;
                string bccmail = string.Empty;
                string strMessageSubject = string.Empty;
                string strMessageBody = string.Empty;
                string useremail = string.Empty;
                parameters.Clear();
                parameters.Add("@REDEEMID", REDEEMID);
                useremail = Convert.ToString(objDb.SendValue_Parameter("SELECT R.EMAIL FROM STBL_REDEEM RD WITH(NOLOCK) JOIN STBL_REGISTRATION R WITH(NOLOCK) ON R.REGISTRATIONID=RD.REGISTRATIONID WHERE RD.REDEEMID=@REDEEMID", parameters));
                if (useremail == "") //No mail id found
                {
                    return;
                }
                DataSet objDataSet = new DataSet();
                parameters.Clear();
                parameters.Add("@MAILID", 8);
                objDataSet = objDb.senddataset_Parameter("SELECT FROM_MAIL, FROM_DISPLAY_NAME, CC_MAIL, BCC_MAIL, SUBJECT FROM STBL_MAIL_MASTER WITH(NOLOCK) WHERE MAILID=@MAILID", parameters);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["dummy_ToMail"].ToString()))
                    {
                        recepientID = ConfigurationManager.AppSettings["dummy_ToMail"].ToString();
                    }
                    else
                    {
                        recepientID = useremail.Trim();
                    }
                    SenderId = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["FROM_MAIL"].ToString());
                    SenderName = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["FROM_DISPLAY_NAME"].ToString());
                    ccmail = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["CC_MAIL"].ToString());
                    bccmail = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["BCC_MAIL"].ToString());
                    strMessageSubject = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["SUBJECT"].ToString());
                }

                StringWriter sw = new StringWriter();
                HtmlTextWriter hWriter = new HtmlTextWriter(sw);
                HttpContext.Current.Server.Execute("~/mails/email.aspx?rid=" + REDEEMID + "&mailid=8", hWriter, true);
                strMessageBody = sw.ToString();
                //System.Threading.Thread.Sleep(300);
                bool status = objDb.Sendmail(SenderId, SenderName, recepientID, ccmail, bccmail, strMessageSubject, strMessageBody);
                //if (status == true)
                //{
                //    objDb.savemail("8", SenderId, recepientID, ccmail, bccmail, strMessageSubject, strMessageBody, REDEEMID);
                //}

            }
            catch (Exception ex)
            {
                logger.Error("mailsCL.cs/send_mail_8: " + ex.Message.ToString() + "::" + ex.StackTrace.ToString());
            }
        }

        // Mail Send To User For Subscription - 1st Time	
        public Boolean send_mail_16(Int32 REGISTRATIONID)
        {
            return false;
            //Initialize Logger
            log4net.Config.XmlConfigurator.Configure();
            //Initialize Logger

            try
            {
                string SenderId = string.Empty;
                string SenderName = string.Empty;
                string recepientID = string.Empty;
                string ccmail = string.Empty;
                string bccmail = string.Empty;
                string strMessageSubject = string.Empty;
                string strMessageBody = string.Empty;
                string useremail = string.Empty;
                
                parameters.Clear();
                parameters.Add("@REGISTRATIONID", REGISTRATIONID);
                useremail = Convert.ToString(objDb.SendValue_Parameter("SELECT EMAIL FROM STBL_REGISTRATION WITH(NOLOCK) WHERE REGISTRATIONID=@REGISTRATIONID", parameters));
                if (useremail == "") //No mail id found
                {
                    return false;
                }
                DataSet objDataSet = new DataSet();
                parameters.Clear();
                parameters.Add("@MAILID", 16);
                objDataSet = objDb.senddataset_Parameter("SELECT FROM_MAIL, FROM_DISPLAY_NAME, CC_MAIL, BCC_MAIL, SUBJECT FROM STBL_MAIL_MASTER WITH(NOLOCK) WHERE MAILID=@MAILID", parameters);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["dummy_ToMail"].ToString()))
                    {
                        recepientID = ConfigurationManager.AppSettings["dummy_ToMail"].ToString();
                    }
                    else
                    {
                        recepientID = useremail.Trim();
                    }
                    SenderId = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["FROM_MAIL"].ToString());
                    SenderName = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["FROM_DISPLAY_NAME"].ToString());
                    ccmail = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["CC_MAIL"].ToString());
                    bccmail = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["BCC_MAIL"].ToString());
                    strMessageSubject = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["SUBJECT"].ToString());
                }

                StringWriter sw = new StringWriter();
                HtmlTextWriter hWriter = new HtmlTextWriter(sw);
                HttpContext.Current.Server.Execute("~/mails/email.aspx?rid=" + REGISTRATIONID + "&mailid=16", hWriter, true);
                strMessageBody = sw.ToString();
                //System.Threading.Thread.Sleep(100);
                bool status = objDb.Sendmail(SenderId, SenderName, recepientID, ccmail, bccmail, strMessageSubject, strMessageBody);
                //if (status == true)
                //{
                //    objDb.savemail("16", SenderId, recepientID, ccmail, bccmail, strMessageSubject, strMessageBody, REGISTRATIONID);
                //}
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("mailsCL.cs/send_mail_16: " + ex.Message.ToString() + "::" + ex.StackTrace.ToString());
                return false;
            }

        }

        // Mail Send To User For Subscription - Renewal	
        public Boolean send_mail_12(Int32 PAYMENTID, Int32 REGISTRATIONID)
        {
            return false;
            //Initialize Logger
            log4net.Config.XmlConfigurator.Configure();
            //Initialize Logger

            try
            {
                string SenderId = string.Empty;
                string SenderName = string.Empty;
                string recepientID = string.Empty;
                string ccmail = string.Empty;
                string bccmail = string.Empty;
                string strMessageSubject = string.Empty;
                string strMessageBody = string.Empty;
                string useremail = string.Empty;
                
                parameters.Clear();
                parameters.Add("@REGISTRATIONID", REGISTRATIONID);
                useremail = Convert.ToString(objDb.SendValue_Parameter("SELECT EMAIL FROM STBL_REGISTRATION WITH(NOLOCK) WHERE REGISTRATIONID=@REGISTRATIONID", parameters));
                if (useremail == "") //No mail id found
                {
                    return false;
                }
                DataSet objDataSet = new DataSet();
                parameters.Clear();
                parameters.Add("@MAILID", 12);
                objDataSet = objDb.senddataset_Parameter("SELECT FROM_MAIL, FROM_DISPLAY_NAME, CC_MAIL, BCC_MAIL, SUBJECT FROM STBL_MAIL_MASTER WITH(NOLOCK) WHERE MAILID=@MAILID", parameters);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["dummy_ToMail"].ToString()))
                    {
                        recepientID = ConfigurationManager.AppSettings["dummy_ToMail"].ToString();
                    }
                    else
                    {
                        recepientID = useremail.Trim();
                    }
                    SenderId = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["FROM_MAIL"].ToString());
                    SenderName = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["FROM_DISPLAY_NAME"].ToString());
                    ccmail = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["CC_MAIL"].ToString());
                    bccmail = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["BCC_MAIL"].ToString());
                    strMessageSubject = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["SUBJECT"].ToString());
                }

                StringWriter sw = new StringWriter();
                HtmlTextWriter hWriter = new HtmlTextWriter(sw);
                HttpContext.Current.Server.Execute("~/mails/email.aspx?rid=" + PAYMENTID + "&mailid=12", hWriter, true);
                strMessageBody = sw.ToString();
                //System.Threading.Thread.Sleep(100);
                bool status = objDb.Sendmail(SenderId, SenderName, recepientID, ccmail, bccmail, strMessageSubject, strMessageBody);
                //if (status == true)
                //{
                //    objDb.savemail("12", SenderId, recepientID, ccmail, bccmail, strMessageSubject, strMessageBody, REGISTRATIONID);
                //}
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("mailsCL.cs/send_mail_12: " + ex.Message.ToString() + "::" + ex.StackTrace.ToString());
                return false;
            }

        }

        // Mail Send To User For Subscription - Unpaid	
        public Boolean send_mail_43(Int32 PAYMENTID, Int32 REGISTRATIONID)
        {
            return false;
            //Initialize Logger
            log4net.Config.XmlConfigurator.Configure();
            //Initialize Logger

            try
            {
                string SenderId = string.Empty;
                string SenderName = string.Empty;
                string recepientID = string.Empty;
                string ccmail = string.Empty;
                string bccmail = string.Empty;
                string strMessageSubject = string.Empty;
                string strMessageBody = string.Empty;
                string useremail = string.Empty;

                parameters.Clear();
                parameters.Add("@REGISTRATIONID", REGISTRATIONID);
                useremail = Convert.ToString(objDb.SendValue_Parameter("SELECT EMAIL FROM STBL_REGISTRATION WITH(NOLOCK) WHERE REGISTRATIONID=@REGISTRATIONID", parameters));
                if (useremail == "") //No mail id found
                {
                    return false;
                }
                DataSet objDataSet = new DataSet();
                parameters.Clear();
                parameters.Add("@MAILID", 43);
                objDataSet = objDb.senddataset_Parameter("SELECT FROM_MAIL, FROM_DISPLAY_NAME, CC_MAIL, BCC_MAIL, SUBJECT FROM STBL_MAIL_MASTER WITH(NOLOCK) WHERE MAILID=@MAILID", parameters);
                if (objDataSet.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["dummy_ToMail"].ToString()))
                    {
                        recepientID = ConfigurationManager.AppSettings["dummy_ToMail"].ToString();
                    }
                    else
                    {
                        recepientID = useremail.Trim();
                    }
                    SenderId = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["FROM_MAIL"].ToString());
                    SenderName = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["FROM_DISPLAY_NAME"].ToString());
                    ccmail = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["CC_MAIL"].ToString());
                    bccmail = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["BCC_MAIL"].ToString());
                    strMessageSubject = HttpUtility.HtmlDecode(objDataSet.Tables[0].Rows[0]["SUBJECT"].ToString());
                }

                StringWriter sw = new StringWriter();
                HtmlTextWriter hWriter = new HtmlTextWriter(sw);
                HttpContext.Current.Server.Execute("~/mails/email.aspx?rid=" + PAYMENTID + "&mailid=43", hWriter, true);
                strMessageBody = sw.ToString();
                //System.Threading.Thread.Sleep(100);
                bool status = objDb.Sendmail(SenderId, SenderName, recepientID, ccmail, bccmail, strMessageSubject, strMessageBody);
                //if (status == true)
                //{
                //    objDb.savemail("43", SenderId, recepientID, ccmail, bccmail, strMessageSubject, strMessageBody, REGISTRATIONID);
                //}
                return true;
            }
            catch (Exception ex)
            {
                logger.Error("mailsCL.cs/send_mail_43: " + ex.Message.ToString() + "::" + ex.StackTrace.ToString());
                return false;
            }

        }
    }
}