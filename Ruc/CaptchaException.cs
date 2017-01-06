using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ruc
{
    /// <summary>
    /// Class for Exception in Captcha.
    /// </summary>
    public class CaptchaException : Exception
    {
        /// <summary>
        /// New instance for <see cref="CaptchaException"/>
        /// </summary>
        /// <param name="message">Message of error</param>
        public CaptchaException(string message)
            : base(message)
        {
            
        }
    }
}
