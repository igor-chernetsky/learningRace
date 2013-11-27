using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetOpenAuth.AspNet;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

namespace LearningRace.Common
{
    public class VKontakteAuthenticationClient : IAuthenticationClient
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public string RedirectUri { get; set; }

        public VKontakteAuthenticationClient(string appId, string appSecret)
        {
            AppId = appId;
            AppSecret = appSecret;
        }

        public string ProviderName
        {
            get { return "vkontakte"; }
        }

        public void RequestAuthentication(HttpContextBase context, Uri returnUrl)
        {
            var APP_ID = AppId;
            RedirectUri = context.Server.UrlEncode(returnUrl.ToString());
            var address = String.Format(
                    "https://oauth.vk.com/authorize?client_id={0}&redirect_uri={1}&response_type=code",
                    APP_ID, RedirectUri
                );

            HttpContext.Current.Response.Redirect(address, false);
        }

        public AuthenticationResult VerifyAuthentication(HttpContextBase context)
        {
            try
            {
                string code = context.Request["code"];

                var address = String.Format(
                        "https://oauth.vk.com/access_token?client_id={0}&client_secret={1}&code={2}&redirect_uri={3}",
                        AppId, AppSecret, code, RedirectUri);

                var response = VKontakteAuthenticationClient.Load(address);
                var accessToken = VKontakteAuthenticationClient.DeserializeJson<AccessToken>(response);

                address = String.Format(
                        "https://api.vk.com/method/users.get?uids={0}&fields=photo_50",
                        accessToken.user_id);

                response = VKontakteAuthenticationClient.Load(address);
                var usersData = VKontakteAuthenticationClient.DeserializeJson<UsersData>(response);
                var userData = usersData.response.First();

                return new AuthenticationResult(
                    true, (this as IAuthenticationClient).ProviderName, accessToken.user_id,
                    userData.first_name + " " + userData.last_name,
                    new Dictionary<string, string>());
            }
            catch (Exception ex)
            {
                return new AuthenticationResult(ex);
            }
        }

        #region private_part

        private static string Load(string address)
        {
            var request = WebRequest.Create(address) as HttpWebRequest;
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        private static T DeserializeJson<T>(string input)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<T>(input);
        }

        private class AccessToken
        {
            public string access_token = null;
            public string user_id = null;
        }

        private class UserData
        {
            public string uid = null;
            public string first_name = null;
            public string last_name = null;
            public string photo_50 = null;
        }

        private class UsersData
        {
            public UserData[] response = null;
        }

        #endregion
    }
}