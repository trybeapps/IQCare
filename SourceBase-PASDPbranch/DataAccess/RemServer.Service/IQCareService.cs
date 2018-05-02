using System;
using System.Data;
using System.Data.SqlClient;
using System.ServiceProcess;
using System.Diagnostics;

using System.Runtime;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization;

using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using Application.BusinessProcess;
using System.Windows.Forms;
using Application.Common;

//for auto backup service
using System.Threading;
using System.Runtime.InteropServices;

namespace RemServer.Service
{
    public class IQCareService : ServiceBase 
    {
        private System.ComponentModel.Container theContainer = null;
        private static EventLog theLog = new EventLog();
        public static string theSRV_Name = "IQCare";
        SqlConnection cnBKTest;
        public DateTime dtBackupTime;
        public string strBackupDrive;
        public string constr;
        System.Threading.Timer oTimer;

        public IQCareService()
        {
            this.ServiceName = theSRV_Name;
        }

        static void Main()
        {
            try
            {
                //theLog.Log = "IQCare";
                //theLog.MaximumKilobytes = 4096;
                theLog.Source = theSRV_Name;
                theLog.WriteEntry(string.Format("{0} Initializing", theSRV_Name));
                ServiceBase.Run(new IQCareService());
            }
            catch(Exception err)
            {
                theLog.WriteEntry(err.ToString());
            }
        }

        private void InitializeComponent()
        {
            theContainer = new System.ComponentModel.Container();
            this.ServiceName = "IQCare";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (theContainer != null)
                {
                    theContainer.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                
                Process theProc = Process.GetCurrentProcess();
                string Config = theProc.MainModule.FileName;
                Config = Config + ".config";

                #region "Connection Parameters"
                Utility clsUtil = new Utility();
                constr = clsUtil.Decrypt(((NameValueCollection)ConfigurationSettings.GetConfig("appSettings"))["ConnectionString"]);
                if (constr.Trim() == "")
                {
                    frmConnection frm = new frmConnection();
                    frm.ShowDialog();
                    constr = ((NameValueCollection)ConfigurationSettings.GetConfig("appSettings"))["ConnectionString"];
                    if (constr == "")
                    {
                        Environment.Exit(1);
                    }
                }
                #endregion
                DoDelayedTasks();
                RemotingConfiguration.Configure(Config);
                RemotingConfiguration.RegisterWellKnownServiceType(typeof(BusinessServerFactory), "BusinessProcess.rem", WellKnownObjectMode.Singleton);
                //theLog.WriteEntry(string.Format("{0} Started", theSRV_Name));
            }
            catch(Exception err)  
            {
                theLog.WriteEntry(err.Message);
            }
        }

        /// <summary>
        /// This process will pick the backup time and backdrive from database.
        /// </summary>
        public void DoDelayedTasks()
        {
            cnBKTest = new SqlConnection(constr);
            const Int32 iTIME_INTERVAL = 50000; //    ' 50 seconds.
            TimerCallback timerDelegate = new TimerCallback(DBEntry);
            oTimer = new System.Threading.Timer(timerDelegate, null, 0, iTIME_INTERVAL);
        }

        protected override void OnStop()
        {
            try
            {
                //theLog.WriteEntry(string.Format("{0} Stopped.", theSRV_Name));
            }
            catch(Exception err)
            {
                theLog.WriteEntry(err.Message); 
            }
        }

        /// <summary>
        /// For automated backup service.
        /// This function takes the backup when server time matches with user specified time.
        /// </summary>
        /// <param name="Message"></param>

        public void DBEntry(Object Message)
        {
            try
            {
                SqlCommand cmdTest;
                cmdTest = new SqlCommand("pr_SystemAdmin_GetBackupTime_Constella", cnBKTest);
                cmdTest.CommandType = CommandType.StoredProcedure;
                int theTimeOut = Convert.ToInt32(((NameValueCollection)ConfigurationSettings.GetConfig("appSettings"))["CommandTimeOut"]);
                cmdTest.CommandTimeout = theTimeOut;
                cnBKTest.Open();
                SqlDataReader readerBackupDetail;
                readerBackupDetail = cmdTest.ExecuteReader();
                if (readerBackupDetail.HasRows)
                {
                    readerBackupDetail.Read();
                    if (readerBackupDetail["BackupTime"].ToString() != "" || readerBackupDetail.IsDBNull(0) != true)
                        dtBackupTime = (DateTime)readerBackupDetail["BackupTime"];
                    if (readerBackupDetail["BackupDrive"].ToString() != "" || readerBackupDetail.IsDBNull(1) != true)
                        strBackupDrive = (string)readerBackupDetail["BackupDrive"];

                }
                cnBKTest.Close();

                if (dtBackupTime.ToString("hh:mm") == DateTime.Now.ToString("hh:mm"))
                {
                    //this.RequestAdditionalTime(50000);
                    cmdTest = new SqlCommand("pr_SystemAdmin_Backup_Constella", cnBKTest);
                    cmdTest.CommandType = CommandType.StoredProcedure;
                    cmdTest.Parameters.Add(new SqlParameter("@FileName", SqlDbType.VarChar, 500));
                    cmdTest.Parameters["@FileName"].Value = strBackupDrive + "\\IQCareDBBackup";
                    cmdTest.Parameters.Add("@Deidentified", SqlDbType.Int).Value = 0;
                    cmdTest.Parameters.Add("@LocationId", SqlDbType.Int).Value = 0;
                    cmdTest.Parameters.Add("@dbKey", SqlDbType.VarChar).Value = ApplicationAccess.DBSecurity.ToString(); 
                    cmdTest.CommandTimeout = theTimeOut;
                    cnBKTest.Open();
                    cmdTest.ExecuteNonQuery();
                }
                cnBKTest.Close();
            }
            catch (Exception err)
            {
                cnBKTest.Close();
                theLog.WriteEntry(err.Message);
            }
        }
    }
}
