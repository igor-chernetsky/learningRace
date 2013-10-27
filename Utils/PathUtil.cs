using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public class PathUtil
    {
        public static string GetCategoryImagePath(Guid? id)
        {
            return string.Format(Constants.categoryFormat, id.ToString());
        }
    }
}
