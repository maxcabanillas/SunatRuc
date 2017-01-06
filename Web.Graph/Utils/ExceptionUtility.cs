using System;
using System.IO;

namespace Web.Graph.Utils
{
    /*
    /// <summary>
    /// Class for save exceptions.
    /// </summary>
    public static class ExceptionUtility
    {
        // All methods are static, so this can be private 

        // Log an Exception 
        /// <summary>
        /// Añade el detalle de una Excepcion.
        /// </summary>
        /// <param name="exc"></param>
        /// <param name="source"></param>
        public static void LogException(Exception exc, string source)
        {
            // Include enterprise logic for logging exceptions 
            // Get the absolute path to the log file 

            // Open the log file for append and write the log
            using (var sw = new StreamWriter(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/log.txt"), true))
            {
                sw.WriteLine("********** {0} **********", DateTime.Now);
                if (exc.InnerException != null)
                {
                    sw.Write("Inner Exception Type: ");
                    sw.WriteLine(exc.InnerException.GetType().ToString());
                    sw.Write("Inner Exception: ");
                    sw.WriteLine(exc.InnerException.Message);
                    sw.Write("Inner Source: ");
                    sw.WriteLine(exc.InnerException.Source);
                    if (exc.InnerException.StackTrace != null)
                    {
                        sw.WriteLine("Inner Stack Trace: ");
                        sw.WriteLine(exc.InnerException.StackTrace);
                    }
                }
                sw.Write("Exception Type: ");
                sw.WriteLine(exc.GetType().ToString());
                sw.WriteLine("Exception: " + exc.Message);
                sw.WriteLine("Source: " + source);
                sw.WriteLine("Stack Trace: ");
                if (exc.StackTrace != null)
                {
                    sw.WriteLine(exc.StackTrace);
                    sw.WriteLine();
                }
            }
        }
    }*/
}