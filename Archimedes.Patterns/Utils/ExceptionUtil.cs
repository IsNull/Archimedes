using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Patterns.Utils
{
    public static class ExceptionUtil
    {
        /// <summary>
        /// Creates an hirarchical exception message composition
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string ComposeDetailMessage(Exception ex)
        {
            if (ex.InnerException == null)
                return ex.Message;

            return ex.Message + Environment.NewLine + ComposeDetailMessage(ex.InnerException);
        }
    }
}
