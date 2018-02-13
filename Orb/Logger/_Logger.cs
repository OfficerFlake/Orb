using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Orb
{
    public static partial class Logger
    {
        public static partial class Log
        {
            public static bool Silent = false;
        }
        public static partial class Console
        {
            public static bool Silent = false;
        }
        #region Auxillary Functions
        private static string FormattedDateTime(DateTime CurrentTime)
        {
            string[] FormattedTime = Utilities.DateTimeUtilities.FormatDateTime(CurrentTime);
            return String.Format("{0}/{1}/{2} {3}:{4}:{5}", FormattedTime[0], FormattedTime[1], FormattedTime[2], FormattedTime[3], FormattedTime[4], FormattedTime[5]);
        }
        private static string FormattedDate(DateTime CurrentTime)
        {
            string[] FormattedTime = Utilities.DateTimeUtilities.FormatDateTime(CurrentTime);
            return String.Format("{0}-{1}-{2}", FormattedTime[0], FormattedTime[1], FormattedTime[2]);
        }
        private static void PrepareLog(string Filename)
        {
            try
            {
                string[] Message = { String.Format("--- Log Created {0} ---", FormattedDateTime(DateTime.Now)) };
                if (Utilities.IO.PrepareFile(Filename)) Utilities.IO.WriteFile(Filename, Message);
            }
            catch
            {
                //Can't prepare the log, it's being written by something else!
            }
            
        }
        #endregion
    }
}
