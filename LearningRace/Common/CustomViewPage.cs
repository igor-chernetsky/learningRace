using LR.Data.Providers;
using LR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace System.Web.Mvc
{
    public class CustomViewPage<T> : WebViewPage<T>
    {
        private UserProfile currentUser;

        public UserProfile CurrentUser
        {
            get
            {
                if (currentUser == null)
                {
                    if (WebSecurity.Initialized)
                        currentUser = DataProvider.UserProfile.GetUserById(WebSecurity.CurrentUserId);
                }
                return currentUser;
            }
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}