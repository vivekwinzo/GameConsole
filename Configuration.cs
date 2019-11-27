using System.Collections.Generic;
using System.Security.Cryptography;


public static class Configuration
{
    public readonly static string ClientId;
    public readonly static string ClientSecret;

    // Static constructor for setting the readonly static members.
    static Configuration()
    {
        var config = GetConfig();
        ClientId = config["clientId"];
        ClientSecret = config["clientSecret"];
    }

    // Create the configuration map that contains mode and other optional configuration details.
    public static Dictionary<string, string> GetConfig()
    {
        return ConfigManager.Instance.GetProperties();
    }

    // Create accessToken
    private static string GetAccessToken()
    {
        // ###AccessToken
        // Retrieve the access token from
        // OAuthTokenCredential by passing in
        // ClientID and ClientSecret
        // It is not mandatory to generate Access Token on a per call basis.
        // Typically the access token can be generated once and
        // reused within the expiry window                
        string accessToken = new OAuthTokenCredential(ClientId, ClientSecret, GetConfig()).GetAccessToken();
        return accessToken;
    }

    // Returns APIContext object
    public static APIContext GetAPIContext(string accessToken = "")
    {
        // ### Api Context
        // Pass in a `APIContext` object to authenticate 
        // the call and to send a unique request id 
        // (that ensures idempotency). The SDK generates
        // a request id if you do not pass one explicitly. 
        var apiContext = new APIContext(string.IsNullOrEmpty(accessToken) ? GetAccessToken() : accessToken);
        apiContext.Config = GetConfig();

        // Use this variant if you want to pass in a request id  
        // that is meaningful in your application, ideally 
        // a order id.
        // String requestId = Long.toString(System.nanoTime();
        // APIContext apiContext = new APIContext(GetAccessToken(), requestId ));

        return apiContext;
    }

    ////Encrypt data using Asymmetric RSA
    //public static byte[] RSA_Encrypt(byte[] byteEncrypt, RSAParameters RSAInfo, bool isOAEP)
    //{
    //    try
    //    {
    //        byte[] encryptedData;
    //        //Create a new instance of RSACryptoServiceProvider.
    //        using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
    //        {
    //            //Import the RSA Key information. This only needs
    //            //toinclude the public key information.
    //            RSA.ImportParameters(RSAInfo);

    //            //Encrypt the passed byte array and specify OAEP padding.
    //            encryptedData = RSA.Encrypt(byteEncrypt, isOAEP);
    //        }
    //        return encryptedData;
    //    }
    //    //Catch and display a CryptographicException
    //    //to the console.
    //    catch (CryptographicException ex)
    //    {
    //        return null;
    //    }
    //}

    ////Decrypt data using Asymmetric RSA
    //public static byte[] RSA_Decrypt(byte[] byteDecrypt, RSAParameters RSAInfo, bool isOAEP)
    //{
    //    try
    //    {
    //        byte[] decryptedData;
    //        //Create a new instance of RSACryptoServiceProvider.
    //        using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
    //        {
    //            //Import the RSA Key information. This needs
    //            //to include the private key information.
    //            RSA.ImportParameters(RSAInfo);

    //            //Decrypt the passed byte array and specify OAEP padding.
    //            decryptedData = RSA.Decrypt(byteDecrypt, isOAEP);
    //        }
    //        return decryptedData;
    //    }
    //    //Catch and display a CryptographicException
    //    //to the console.
    //    catch (CryptographicException ex)
    //    {
    //        return null;
    //    }

    //}
}

