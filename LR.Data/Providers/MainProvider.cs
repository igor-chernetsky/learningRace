using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LR.Data.Providers
{
    public class MainProvider
    {
        private DataContext ctx;

        protected DataContext Context
        {
            get
            {
                if (ctx == null)
                {
                    ctx = new DataContext();
                }
                return ctx;
            }
        }
    }
}
