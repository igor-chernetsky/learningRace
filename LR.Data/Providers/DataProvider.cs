using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Data.Providers
{
    public class DataProvider
    {
        private static QuestionProvider questions;
        private static CategoryProvider category;
        private static UserProvider userProfile;

        public static QuestionProvider Questions
        {
            get
            {
                if (questions == null)
                {
                    questions = new QuestionProvider();
                }
                return questions;
            }
        }

        public static CategoryProvider Category
        {
            get
            {
                if (category == null)
                {
                    category = new CategoryProvider();
                }
                return category;
            }
        }

        public static UserProvider UserProfile
        {
            get
            {
                if (userProfile == null)
                {
                    userProfile = new UserProvider();
                }
                return userProfile;
            }
        }
    }
}
