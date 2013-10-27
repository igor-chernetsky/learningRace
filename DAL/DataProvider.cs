using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.DataManager;

namespace DAL
{
    public class DataProvider
    {
        public static CategoryManager CategoryManager
        {
            get
            {
                return new CategoryManager();
            }
        }

        public static QuestionManager QuestionManager
        {
            get
            {
                return new QuestionManager();
            }
        }

        public static RaceManager RaceManager
        {
            get
            {
                return new RaceManager();
            }
        }
    }
}
