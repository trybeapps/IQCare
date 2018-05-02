using System;
using System.Linq;
using log4net;
using log4net.Config;
using log4net.Appender;
using System.IO;


namespace Application.Common
{
    public static class CLogger
    {
        #region Members
        private static readonly ILog logger = LogManager.GetLogger(typeof(CLogger));
        #endregion


        #region Constructors
        static CLogger()
        {
            XmlConfigurator.Configure();


        }
        #endregion


        #region Methods
        public static void WriteLog(ELogLevel logLevel, String log)
        {
            WriteLog(logLevel, log, null);
        }

        public static void WriteLog(String raisedFrom, String commandText, String parameters, String exception)
        {
            WriteLog(ELogLevel.ERROR, raisedFrom, null);
            if (!string.IsNullOrEmpty(commandText))
            {
                WriteLog(ELogLevel.ERROR, "CommandText: " + commandText, null);
            }
            string strParamData = "Parameters: " + Environment.NewLine + parameters.ToString();
            if (!string.IsNullOrEmpty(strParamData))
            {
                WriteLog(ELogLevel.ERROR, strParamData, null);
            }
            WriteLog(ELogLevel.ERROR, "Exception Message: " + exception.ToString(), null);
            WriteLog(ELogLevel.ERROR, "---------------------------------------------------------------------------------------");
        }

        public static void WriteLog(ELogLevel logLevel, String log, Exception exception)
        {
            if (logLevel.Equals(ELogLevel.DEBUG))
            {
                logger.Debug(log);
            }
            else if (logLevel.Equals(ELogLevel.ERROR))
            {
                logger.Error(log, exception);
            }
            else if (logLevel.Equals(ELogLevel.FATAL))
            {
                logger.Fatal(log, exception);
            }
            else if (logLevel.Equals(ELogLevel.INFO))
            {
                logger.Info(log);
            }
            else if (logLevel.Equals(ELogLevel.WARN))
            {
                logger.Warn(log);
            }
        }
        #endregion


        #region Clean Up Methods
        /// <summary>
        /// Cleans up. Auto configures the cleanup based on the log4net configuration
        /// </summary>
        public static void CleanUp()
        {
            if (!object.Equals(System.Configuration.ConfigurationManager.AppSettings["DaysToKeepLogFile"], null))
            {
                int daysToKeep = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DaysToKeepLogFile"].ToString());
                if (daysToKeep > 0)
                {
                    DateTime date = DateTime.Now.AddDays(-daysToKeep);
                    CleanUp(date);
                }

            }
        }

        /// <summary>
        /// Cleans up. Auto configures the cleanup based on the log4net configuration
        /// </summary>
        /// <param name="date">Anything prior will not be kept.</param>
        private static void CleanUp(DateTime date)
        {
            string directory = string.Empty;
            string filePrefix = string.Empty;

            var repo = LogManager.GetAllRepositories().FirstOrDefault(); ;
            if (repo == null)
            {
                //throw new NotSupportedException("Log4Net has not been configured yet.");
                return;
            }
            else
            {
                var app = repo.GetAppenders().Where(x => x.GetType() == typeof(RollingFileAppender)).FirstOrDefault();
                if (app != null)
                {
                    var appender = app as RollingFileAppender;

                    directory = Path.GetDirectoryName(appender.File);
                    filePrefix = Path.GetFileName(appender.File);

                    CleanUp(directory, filePrefix, date);
                }
            }
        }

        /// <summary>
        /// Cleans up.
        /// </summary>
        /// <param name="logDirectory">The log directory.</param>
        /// <param name="logPrefix">The log prefix. Example: logfile dont include the file extension.</param>
        /// <param name="date">Anything prior will not be kept.</param>
        private static void CleanUp(string logDirectory, string logPrefix, DateTime date)
        {
            if (string.IsNullOrEmpty(logDirectory))
                return;
            //throw new ArgumentException("logDirectory is missing");

            if (string.IsNullOrEmpty(logDirectory))
                return;
            //throw new ArgumentException("logPrefix is missing");

            var dirInfo = new DirectoryInfo(logDirectory);
            if (!dirInfo.Exists)
                return;

            //var fileInfos = dirInfo.GetFiles("{0}*.*".Sub(logPrefix));
            var fileInfos = dirInfo.GetFiles();
            if (fileInfos.Length == 0)
                return;

            foreach (var info in fileInfos)
            {
                if (string.Compare(info.Name, logPrefix, true) != 0)
                {
                    if (info.CreationTime < date)
                    {
                        info.Delete();
                    }
                }
            }

        }
        #endregion
    }

    public enum ELogLevel
    {
        DEBUG = 1,
        ERROR,
        FATAL,
        INFO,
        WARN
    }
}


