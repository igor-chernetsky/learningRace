using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using LearningRace.Models;

namespace LearningRace
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: "591753204214322",
                appSecret: "4608e8bdf8a939f3c68835247a6c7b57");

            OAuthWebSecurity.RegisterClient(new LearningRace.Common.VKontakteAuthenticationClient(
                "3969828",
                "utFJZKOoWqKc38Byz713"), "Vkontakte", null);

            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
