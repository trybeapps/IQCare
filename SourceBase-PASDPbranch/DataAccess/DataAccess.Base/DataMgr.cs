using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Specialized;
using MySql.Data.MySqlClient;
using System.Configuration;
using Application.Common;

namespace DataAccess.Base
{
    public class DataMgr 
    {
        #region "Constructor"
        public DataMgr()
        {
        }
        #endregion

        #region "Custom Properties"
        public static object GetConnection()
        {
            Utility objUtil = new Utility();
            string constr = objUtil.Decrypt(((NameValueCollection)ConfigurationSettings.GetConfig("appSettings"))["ConnectionString"]);
            constr += ";connect timeout=" + ((NameValueCollection)ConfigurationSettings.GetConfig("appSettings"))["SessionTimeOut"].ToString();
            constr += ";packet size=4128;Min Pool Size=3;Max Pool Size=200;";
            SqlConnection connection = new SqlConnection(constr);
            connection.Open();
            return connection;
        }

        public static object GetMySQLConnection()
        {
            Utility objUtil = new Utility();
            string constr = objUtil.Decrypt(((NameValueCollection)ConfigurationSettings.GetConfig("appSettings"))["MySQLConnectionString"]);
            //constr += "connect timeout=" + ((NameValueCollection)ConfigurationSettings.GetConfig("appSettings"))["SessionTimeOut"].ToString();
            //constr += ";packet size=4128;Min Pool Size=3;Max Pool Size=200;";
            MySqlConnection connection = new MySqlConnection(constr);
            connection.Open();
            return connection;
        }

        public static object GetConnection_Master()
        {
            Utility objUtil = new Utility();
            string constr = objUtil.Decrypt(((NameValueCollection)ConfigurationSettings.GetConfig("appSettings"))["ConnectionString"]);
           // constr = constr.Substring(0, constr.Length - 6);
           // constr += "master";
            constr = constr.Substring(0, constr.IndexOf("g=") + 2).ToString() + "Master";
            constr += ";connect timeout=" + ((NameValueCollection)ConfigurationSettings.GetConfig("appSettings"))["SessionTimeOut"].ToString();
            constr += ";packet size=4128;Min Pool Size=3;Max Pool Size=200;";
            //set nocount off;set arithabort on;set concat_null_yields_null on;set ansi_nulls on;";
            //constr += "set cursor_close_on_commit off;set ansi_null_dflt_on on;set implicit_transactions off;set ansi_padding on;set ansi_warnings on;";
            //constr += "set quoted_identifier on;";
            SqlConnection connection = new SqlConnection(constr);
            connection.Open();
            return connection;
            
        }

        public static int CommandTimeOut()
        {
            int TimeOut = Convert.ToInt32(((NameValueCollection)ConfigurationSettings.GetConfig("appSettings"))["CommandTimeOut"]);
            return TimeOut;
        }

        public static void ReleaseConnection(object Connection)
        {
            SqlConnection cnn = (SqlConnection)Connection;
            if (cnn != null)
            {
                if (cnn.State != ConnectionState.Closed)
                    cnn.Close();
                cnn.Dispose();
            }
        }

        public static void ReleaseMySQLConnection(object Connection)
        {
            MySqlConnection cnn = (MySqlConnection)Connection;
            if (cnn != null)
            {
                if (cnn.State != ConnectionState.Closed)
                    cnn.Close();
                cnn.Dispose();
            }
        }
        public static object BeginTransaction(object Connection)
        {
            return ((SqlConnection)Connection).BeginTransaction();
        }

        public static void CommitTransaction(object Transation)
        {
            ((SqlTransaction)Transation).Commit();
        }

        public static void RollBackTransation(object Transaction)
        {
            ((SqlTransaction)Transaction).Rollback();
        }

        #endregion
    }
}
