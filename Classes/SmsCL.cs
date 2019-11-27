using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Configuration;

namespace GameConsole.Classes
{
  public class SmsCL
  {
    DBFunctions objDb = new DBFunctions();
    Hashtable parameters = new Hashtable();

    public Boolean SendSMS_Phonecontact(string MOBILE, string LINK)
    {
      try
      {
        string objSMS = "Bro yeh le - WinZO! earning App " + LINK + " Download k baad \'Phone Setting\' mei \'Unknown Source\' ko \'Ok\' karna. Rs.20 milenge. Aaja!";
        objDb.sendSMS(MOBILE, objSMS);
        SAVE_SMS(MOBILE, objSMS, "Phone Contact Invite");
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public Boolean SendSmsFantasyPurchase(string MOBILE)
    {
      try
      {
        string objSMS = "Please update your WinZO App - select Captain/Vice-Captain, and get double points. You will not receive if you will not update. Link - https://www.winzogames.com";
        objDb.sendSMS(MOBILE, objSMS);
        SAVE_SMS(MOBILE, objSMS, "Add Cash");
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public Boolean SendSMS_ADD_CASH(string MOBILE, string AMOUNT)
    {
      try
      {
        string objSMS = "Hello, You have added Rs." + AMOUNT + " in your WinZO Gold wallet.";
        objDb.sendSMS(MOBILE, objSMS);
        SAVE_SMS(MOBILE, objSMS, "Add Cash");
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public Boolean SendSMS_SUBSCRIPTION_WK(string MOBILE, string DURATION)
    {
      try
      {
        string objSMS = "Shukriya, Your payment is successful. You can now freely play WinZO for next " + DURATION + " days. Cheers!";
        objDb.sendSMS(MOBILE, objSMS);
        SAVE_SMS(MOBILE, objSMS, "Subscription WinZO");
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public Boolean SendSMS_Fantasy_Redeem(string MOBILE, string CONTEST_NAME, string AMOUNT, string LINK)
    {
      try
      {
        string objSMS = "Congratulations for unlocking your ticket for match " + CONTEST_NAME + ". Click on the link and select 11 players to play for Rs." + AMOUNT + " cash pool! " + LINK;
        objDb.sendSMS(MOBILE, objSMS);
        SAVE_SMS(MOBILE, objSMS, "FANTASY_REDEEM");
        return true;
      }
      catch
      {
        return false;
      }
    }

    public Boolean SendSMS_IPL_REDEEM(string MOBILE, string CONTEST_NAME, string AMOUNT, string LINK)
    {
      try
      {
        string objSMS = "Congratulations for unlocking your ticket for the match " + CONTEST_NAME + ". Click on the link and answer all questions correctly to receive Rs." + AMOUNT + " cash! " + LINK;
        objDb.sendSMS(MOBILE, objSMS);
        SAVE_SMS(MOBILE, objSMS, "IPL_REDEEM");
        return true;
      }
      catch
      {
        return false;
      }
    }

    public Boolean SendSMS_IPL_WINNER(string MOBILE, string CONTEST_NAME, string AMOUNT)
    {
      try
      {
        string objSMS = "Hurray! You are the Captain! All your answers for the match " + CONTEST_NAME + " were correct! Rs." + AMOUNT + " will be credited in your wallet soon.";
        objDb.sendSMS(MOBILE, objSMS);
        SAVE_SMS(MOBILE, objSMS, "IPL_WINNER");
        return true;
      }
      catch
      {
        return false;
      }
    }

    public Boolean SendSMS_IPL_NON_WINNER(string MOBILE)
    {
      try
      {
        string objSMS = "Good Luck for the next match! Yesterday you were good but you need to answer all questions correctly to be the winner. Play again in the next match and be the Shehenshah of cricket!";
        objDb.sendSMS(MOBILE, objSMS);
        SAVE_SMS(MOBILE, objSMS, "IPL_WINNER");
        return true;
      }
      catch
      {
        return false;
      }
    }

    public Boolean SendSMS_WALLET_WITHDRAWL_UNPAID(string MOBILE, string AMOUNT)
    {
      try
      {
        string objSMS = "Congratulations! You have successfully withdrawn Rs." + AMOUNT + " from WinZO Wallet. Install WinZO Gold and earn aur zyada money. goo.gl/uhEST7";
        objDb.sendSMS(MOBILE, objSMS);
        SAVE_SMS(MOBILE, objSMS, "Wallet Withdrawl");
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public Boolean SendSMS_WALLET_WITHDRAWL(string MOBILE, string AMOUNT)
    {
      try
      {
        string objSMS = "Congratulations! You have successfully withdrawn Rs." + AMOUNT + " from WinZO Wallet into your PayTM Account. It may take upto 5 working days to receive your money in your PayTM Wallet.";
        objDb.sendSMS(MOBILE, objSMS);
        SAVE_SMS(MOBILE, objSMS, "Wallet Withdrawl");
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public Boolean SendSMS_FREE_PACKAGE(string MOBILE)
    {
      try
      {
        string objSMS = "Congratulations! Aap jeete hai WinZO ka Rs.25 ka subscription. Ab khelo jyada aur jeeto jyada.";
        objDb.sendSMS(MOBILE, objSMS);
        SAVE_SMS(MOBILE, objSMS, "Free Package");
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public Boolean SendSMS_BOOSTER_MONEY(string MOBILE, string TOTAL_CORRECT, string BOOST_ATTEMPTS, string PACKAGE_AMT, string BOOST_AMT)
    {
      try
      {
        string objSMS = "Yay! You correctly answered " + TOTAL_CORRECT + " out of your 1st " + BOOST_ATTEMPTS + " attempts of your current Rs." + PACKAGE_AMT + " plan. You have received Rs." + BOOST_AMT + " in your Wallet.";
        objDb.sendSMS(MOBILE, objSMS);
        SAVE_SMS(MOBILE, objSMS, "Booster Money !!");
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public Boolean SendSMS_SUBSCRIBED_REFERRED_FRIEND(string MOBILE, string NAME, string RNAME)
    {
      try
      {
        parameters.Clear();
        var objFRIEND_SUBSCRIBE_BONUS = objDb.SendValue_Parameter("SELECT FRIEND_SUBSCRIBE_BONUS FROM STBL_REFER_CONFIG WITH(NOLOCK)", parameters);
        string objSMS = "Hey " + NAME + ", Your friend " + RNAME + " thanks you for subscribing, as WinZO has transferred Rs." + objFRIEND_SUBSCRIBE_BONUS + " in their Cash Wallet on the occasion of your purchase.";
        objDb.sendSMS(MOBILE, objSMS);
        SAVE_SMS(MOBILE, objSMS, "Referral Subscribe");
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public Boolean SendSMS_SUBSCRIBED_REFERRED(string MOBILE, string NAME, string RNAME)
    {
      try
      {
        var objFRIEND_SUBSCRIBE_BONUS = objDb.SendValue_Parameter("SELECT FRIEND_SUBSCRIBE_BONUS FROM STBL_REFER_CONFIG WITH(NOLOCK)", parameters);
        string objSMS = "Hey " + RNAME + ", Your friend " + NAME + ", has purchased a subscription pack on WinZO. As a bonus, WinZO is transferring Rs." + objFRIEND_SUBSCRIBE_BONUS + " in your Cash Wallet.";
        objDb.sendSMS(MOBILE, objSMS);
        SAVE_SMS(MOBILE, objSMS, "Referral Subscribe");
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public Boolean SendSMS_WINNER_REFERRED_FRIEND(string MOBILE, string NAME, string RNAME)
    {
      try
      {
        var objFRIEND_WON_BONUS = objDb.SendValue_Parameter("SELECT FRIEND_WON_BONUS FROM STBL_REFER_CONFIG WITH(NOLOCK)", parameters);
        string objSMS = "Hey " + RNAME + ", Your friend " + NAME + " thanks you for getting the prize, as WinZO has transferred Rs." + objFRIEND_WON_BONUS + " in their Cash Wallet on the occasion of your victory.";
        objDb.sendSMS(MOBILE, objSMS);
        SAVE_SMS(MOBILE, objSMS, "Referral Winner");
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public Boolean SendSMS_WINNER_REFERRED(string MOBILE, string NAME, string RNAME, string DEAL_TITLE, string PRICE)
    {
      try
      {
        var objFRIEND_WON_BONUS = objDb.SendValue_Parameter("SELECT FRIEND_WON_BONUS FROM STBL_REFER_CONFIG WITH(NOLOCK)", parameters);
        string objSMS = "Hey " + NAME + ", Your friend " + RNAME + ", has cracked the deal, " + DEAL_TITLE + " with a prize amount of Rs." + PRICE + ". As a bonus, WinZO is transferring Rs." + objFRIEND_WON_BONUS.ToString() + " in your Cash Wallet.";
        objDb.sendSMS(MOBILE, objSMS);
        SAVE_SMS(MOBILE, objSMS, "Referral Winner");
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public Boolean SendSMS_REFERRAL_USED(string MOBILE, string NAME, string AMOUNT)
    {
      try
      {
        string objSMS = "Your referral code was used by " + NAME + ". Congratulations! Rs." + AMOUNT + " will be added to your and your friend's wallet within 48 hours. -WinZO";
        objDb.sendSMS(MOBILE, objSMS);
        SAVE_SMS(MOBILE, objSMS, "Referral Used");
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public Boolean SendSMS_MOBILE_UPDATE(string MOBILE)
    {
      try
      {
        string objSMS = "You have successfully updated your mobile number on WinZO! The new number registered with your account is " + MOBILE + "";
        objDb.sendSMS(MOBILE, objSMS);
        SAVE_SMS(MOBILE, objSMS, "Mobile Update");
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public Boolean SendSMS_LEVEL_REDEEM_VOUCHER(string MOBILE, string NAME, string LEVEL_USED, string PRODUCT_TITLE, string ACTUAL_AMOUNT, string PAYAMOUNT)
    {
      try
      {
        string objSMS = "Hey " + NAME + "! Congratulations on redeeming the " + PRODUCT_TITLE + " worth Rs." + ACTUAL_AMOUNT + ", for " + LEVEL_USED + " Levels with a payment of Rs." + PAYAMOUNT + ". -WinZO";
        objDb.sendSMS(MOBILE, objSMS);
        SAVE_SMS(MOBILE, objSMS, "Level Redeem Voucher");
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public Boolean SendSMS_LEVEL_REDEEM_PRODUCT(string MOBILE, string NAME, string LEVEL_USED, string PRODUCT_TITLE, string PAYAMOUNT)
    {
      try
      {
        string objSMS = "Hey " + NAME + ", Congrats! You have just redeemed " + LEVEL_USED + " Levels with a payment of Rs." + PAYAMOUNT + " for " + PRODUCT_TITLE + ". We have received your request and will keep you updated. -WinZO";
        objDb.sendSMS(MOBILE, objSMS);
        SAVE_SMS(MOBILE, objSMS, "Level Redeem Product");
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public Boolean SendSMS_SUBSCRIPTION_RENEWAL(string MOBILE)
    {
      try
      {
        string objSMS = "Shukriya, Your payment is successful. Once again, Enjoy these upcoming Challenges on WinZO and get prizes once again. To know how to play the various challenges, click https://goo.gl/hu5UaB.";
        objDb.sendSMS(MOBILE, objSMS);
        SAVE_SMS(MOBILE, objSMS, "Subscription Renewal");
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public Boolean SendSMS_SUBSCRIPTION_1ST(string MOBILE, string PAYAMOUNT)
    {
      try
      {
        string objSMS = "Shukriya, Your payment of Rs." + PAYAMOUNT + " was successful. Your future earnings will be automatically credited in your account. Keep Playing Keep Earning. To learn how to play, click https://goo.gl/hu5UaB.";
        objDb.sendSMS(MOBILE, objSMS);
        SAVE_SMS(MOBILE, objSMS, "Subscription 1st");
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public Boolean SendSMS_OTP(string MOBILE, string VERIFICATION_CODE)
    {
      try
      {
        // string objSMS = "<#> Your OTP for WinZO is " + VERIFICATION_CODE + ". It is valid for 24 Hours. Please validate your account and continue your WinZO journey. j/sAbb2V+UH";
        string objSMS = "<#> Your OTP for WinZO is " + VERIFICATION_CODE + ". It is valid for 30 minutes. Please validate your account and continue your WinZO journey. w1CFnFpuO6s";
        objDb.sendSMS_v1(MOBILE, objSMS);
        //SAVE_SMS(MOBILE, objSMS, "OTP");
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    protected void SAVE_SMS(string MOBILE, string SMS, string MSGTYPE)
    {
      // SMS HISTORY         
      parameters.Clear();
      parameters.Add("@TRID", 0);
      parameters.Add("@MOBILENO", MOBILE);
      parameters.Add("@SMS_TYPE", MSGTYPE);
      parameters.Add("@SMS_BODY", SMS);
      parameters.Add("@MODE", "CREATE");
      objDb.ExecuteQry_SP("UDSP_SMS_HISTORY", parameters);
    }
  }
}