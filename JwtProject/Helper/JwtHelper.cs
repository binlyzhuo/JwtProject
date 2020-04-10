using ICSharpCode.SharpZipLib.Zip;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using ServiceStack.Text;

namespace JwtProject.Helper
{
    public class JwtHelper
    {
        private static string secret = ConfigurationManager.AppSettings["JwtSecret"].ToString();

        public static string SetJwtEncode(Dictionary<string, object> payload)
        {
            IDateTimeProvider provider = new UtcDateTimeProvider();
            var now = provider.GetNow();
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var secondsSinceEpoch = Math.Round((now - unixEpoch).TotalSeconds);

            payload.Add("exp",secondsSinceEpoch+2);
            payload.Add("iat",now.ToString());
            payload.Add("issuer","eduplat");
            payload.Add("audience","");
            payload.Add("jti",Guid.NewGuid().ToString());

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(payload, secret);

            var key = EncryptionAlgorithm.Hash(token);
            
            return token;
        }

        public static UserInfo GetJwtDecode(string token)
        {
            try
            {
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
                var userInfo = decoder.DecodeToObject<UserInfo>(token, secret, verify: true);//token为之前生成的字符串
                return userInfo;
            }
            catch(JWT.TokenExpiredException ex)
            {
                return null;
            }
            catch(JWT.SignatureVerificationException ex)
            {
                return null;
            }
        }
    }
}