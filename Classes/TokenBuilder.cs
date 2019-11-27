using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Common;
using System.Security.Authentication;
using System.Security.Cryptography;

namespace GameConsole.Classes
{
    public class TokenBuilder : ITokenBuilder
    {
        public string Build(Int32 creds)
        {
            string StaticToken = String.Empty;
            if (new CredentialsValidator().IsValid(creds))
            {
                StaticToken = BuildSecureToken(100);
                return StaticToken;
            }

            throw new AuthenticationException();
        }

        private static string BuildSecureToken(int length)
        {
            var buffer = new byte[length];
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                rngCryptoServiceProvider.GetNonZeroBytes(buffer);
            }
            return Convert.ToBase64String(buffer);
        }

        public string Build_New(Int32 creds)
        {
            string StaticToken = String.Empty;

            StaticToken = BuildSecureToken(100);
            return StaticToken;
        }
    }
}