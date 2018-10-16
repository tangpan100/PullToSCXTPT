using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace PullToScxtpt_px.Helper
{
    public class LogHelper
    {
        public static ILog GetLog<T>(T t)
        {
            ILog _log = LogManager.GetLogger("");
            if (t != null)
            {
                _log = LogManager.GetLogger(t.GetType());
            }
            return _log;
        }
    }
}
