using System;
using System.Data;
using System.Data.SqlClient;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using DataAccess.Common;
using DataAccess.Base;
using Application.Common;

namespace DataAccess.Entity
{
    public class ClsObject : ProcessBase
    {
        #region "Constructor"
        public ClsObject()
        {
        }
        #endregion

        #region "User Functions"
        public object ReturnObject(Hashtable Params, string CommandText, ClsUtility.ObjectEnum Obj)
        {
            int i;
            string cmdpara, cmdvalue, cmddbtype;
            ParameterDirection cmdDirection;
            SqlCommand theCmd = new SqlCommand();
            SqlTransaction theTran = (SqlTransaction)this.Transaction;
            SqlConnection cnn = null;
            StringBuilder strParam = new StringBuilder();

            try
            {
                if (null == this.Connection)
                {
                    cnn = (SqlConnection)DataMgr.GetConnection();
                }
                else
                {
                    cnn = (SqlConnection)this.Connection;
                }

                if (null == this.Transaction)
                {
                    theCmd = new SqlCommand(CommandText, cnn);
                }
                else
                {
                    theCmd = new SqlCommand(CommandText, cnn, theTran);
                }

                for (i = 1; i < Params.Count; )
                {
                    cmdpara = Params[i].ToString();
                    cmddbtype = Params[i + 1].ToString();
                    if (Params[i + 2] != null)
                    {
                        if (Params[i + 2].GetType() != ParameterDirection.Output.GetType())
                        {
                            cmdvalue = Params[i + 2].ToString();
                            theCmd.Parameters.Add(cmdpara, cmddbtype).Value = cmdvalue;
                        }
                        else
                        {
                            cmdDirection = (ParameterDirection)Params[i + 2];
                            theCmd.Parameters.Add(cmdpara, (SqlDbType)Params[i + 1]).Direction = cmdDirection;
                        }
                    }
                    else
                    {
                        cmdvalue = string.Empty;
                        theCmd.Parameters.Add(cmdpara, cmddbtype).Value = cmdvalue;
                    }
                    i = i + 3;
                }

                theCmd.CommandType = CommandType.StoredProcedure;
                theCmd.CommandTimeout = DataMgr.CommandTimeOut();
                string theSubstring = CommandText.Substring(0, 6).ToUpper();
                switch (theSubstring)
                {
                    case "SELECT":
                        theCmd.CommandType = CommandType.Text;
                        break;
                    case "UPDATE":
                        theCmd.CommandType = CommandType.Text;
                        break;
                    case "INSERT":
                        theCmd.CommandType = CommandType.Text;
                        break;
                    case "DELETE":
                        theCmd.CommandType = CommandType.Text;
                        break;
                }

                theCmd.Connection = cnn;

                if (Obj == ClsUtility.ObjectEnum.DataSet)
                {

                    SqlDataAdapter theAdpt = new SqlDataAdapter(theCmd);
                    DataSet theDS = new DataSet();
                    theAdpt.Fill(theDS);
                    theAdpt.Dispose();
                    return theDS;
                }

                if (Obj == ClsUtility.ObjectEnum.DataTable)
                {
                    SqlDataAdapter theAdpt = new SqlDataAdapter(theCmd);
                    DataTable theDT = new DataTable();
                    theAdpt.Fill(theDT);
                    theAdpt.Dispose();
                    return theDT;
                }

                if (Obj == ClsUtility.ObjectEnum.DataRow)
                {
                    SqlDataAdapter theAdpt = new SqlDataAdapter(theCmd);
                    DataTable theDT = new DataTable();
                    theAdpt.Fill(theDT);
                    theAdpt.Dispose();
                    return theDT.Rows[0];
                }


                if (Obj == ClsUtility.ObjectEnum.ExecuteNonQuery)
                {
                    int NoRowsAffected = 0;
                    NoRowsAffected = theCmd.ExecuteNonQuery();
                    if (theCmd.Parameters.Contains("@idNEW"))
                    {
                        if ((int)theCmd.Parameters["@idNEW"].Value > 0)
                            NoRowsAffected = (int)theCmd.Parameters["@idNEW"].Value;
                    }

                    return NoRowsAffected;
                }
                return 0;
            }
            catch (Exception err)
            {
                for (i = 1; i < Params.Count; )
                {
                    cmdpara = Params[i].ToString();
                    cmddbtype = Params[i + 1].ToString();
                    if (Params[i + 2] != null)
                    {
                        if (Params[i + 2].GetType() != ParameterDirection.Output.GetType())
                        {
                            cmdvalue = Params[i + 2].ToString();
                            strParam.Append("Name: " + cmdpara + ", Type: " + cmddbtype + ", Value: " + cmdvalue + ", Direction: Input");
                            strParam.Append(Environment.NewLine);
                        }
                        else
                        {
                            cmdDirection = (ParameterDirection)Params[i + 2];
                            strParam.Append("Name: " + cmdpara + ", Type: " + cmddbtype + ", Direction: Output");
                            strParam.Append(Environment.NewLine);
                        }

                    }
                    i = i + 3;
                }

                CLogger.WriteLog("Namespace: DataAccess.Entity, Class: ClsObject, Method: ReturnObject - Call started.", CommandText, strParam.ToString(), err.ToString());

                throw err;
            }
            finally
            {
                if (null != cnn)
                    if (null == this.Connection)
                        DataMgr.ReleaseConnection(cnn);
            }

        }

        public object ReturnMySQLObject(Hashtable Params, string CommandText, ClsUtility.ObjectEnum Obj)
        {
            int i;
            string cmdpara, cmdvalue, cmddbtype;
            MySqlCommand theCmd = new MySqlCommand();
            MySqlTransaction theTran = (MySqlTransaction)this.Transaction;
            MySqlConnection cnn = null;
            StringBuilder strParam = new StringBuilder();

            try
            {
                if (null == this.Connection)
                {
                    cnn = (MySqlConnection)DataMgr.GetMySQLConnection();
                }
                else
                {
                    cnn = (MySqlConnection)this.Connection;
                }

                if (null == this.Transaction)
                {
                    theCmd = new MySqlCommand(CommandText, cnn);
                }
                else
                {
                    theCmd = new MySqlCommand(CommandText, cnn, theTran);
                }

                for (i = 1; i < Params.Count; )
                {
                    cmdpara = Params[i].ToString();
                    cmddbtype = Params[i + 1].ToString();
                    cmdvalue = Params[i + 2].ToString();
                    theCmd.Parameters.Add(cmdpara, cmddbtype).Value = cmdvalue;
                    i = i + 3;
                }

                theCmd.CommandType = CommandType.StoredProcedure;
                theCmd.CommandTimeout = DataMgr.CommandTimeOut();
                string theSubstring = CommandText.Substring(0, 6).ToUpper();
                switch (theSubstring)
                {
                    case "SELECT":
                        theCmd.CommandType = CommandType.Text;
                        break;
                    case "UPDATE":
                        theCmd.CommandType = CommandType.Text;
                        break;
                    case "INSERT":
                        theCmd.CommandType = CommandType.Text;
                        break;
                    case "DELETE":
                        theCmd.CommandType = CommandType.Text;
                        break;
                }

                theCmd.Connection = cnn;

                if (Obj == ClsUtility.ObjectEnum.DataSet)
                {
                    MySqlDataAdapter theAdpt = new MySqlDataAdapter(theCmd);
                    DataSet theDS = new DataSet();
                    theAdpt.Fill(theDS);
                    theAdpt.Dispose();
                    return theDS;
                }
                if (Obj == ClsUtility.ObjectEnum.DataTable)
                {
                    MySqlDataAdapter theAdpt = new MySqlDataAdapter(theCmd);
                    DataTable theDT = new DataTable();
                    theAdpt.Fill(theDT);
                    theAdpt.Dispose();
                    return theDT;
                }
                if (Obj == ClsUtility.ObjectEnum.DataRow)
                {
                    MySqlDataAdapter theAdpt = new MySqlDataAdapter(theCmd);
                    DataTable theDT = new DataTable();
                    theAdpt.Fill(theDT);
                    theAdpt.Dispose();
                    return theDT.Rows[0];
                }
                if (Obj == ClsUtility.ObjectEnum.ExecuteNonQuery)
                {
                    int NoRowsAffected = theCmd.ExecuteNonQuery();
                    return NoRowsAffected;
                }
                return 0;
            }
            catch (Exception err)
            {
                for (i = 1; i < Params.Count; )
                {
                    cmdpara = Params[i].ToString();
                    cmddbtype = Params[i + 1].ToString();
                    cmdvalue = Params[i + 2].ToString();
                    strParam.Append("Name: " + cmdpara + ", Type: " + cmddbtype + ", Value: " + cmdvalue + " ");
                    strParam.Append(Environment.NewLine);

                    i = i + 3;
                }

                CLogger.WriteLog("Namespace: DataAccess.Entity, Class: ClsObject, Method: ReturnMySQLObject - Call started.", CommandText, strParam.ToString(), err.ToString());

                throw err;
            }
            finally
            {
                if (null != cnn)
                    if (null == this.Connection)
                        DataMgr.ReleaseMySQLConnection(cnn);
            }

        }
        #endregion

        public bool CreateErrorLogs(Exception expOccured, Hashtable Params, string CommandText, ClsUtility.ObjectEnum Obj)
        {
            string fileName = @"c:\IQCare_Error_Logs_KNH.txt";
            try
            {
                if (System.IO.File.Exists(fileName))
                {
                    //append
                    System.IO.File.Open(fileName, System.IO.FileMode.Append);
                    System.IO.File.WriteAllText(fileName, "Error occured - Object = " + Obj.ToString() + Environment.NewLine +
                    " --- CommandText = " + Environment.NewLine +
                    " --- Params = " + Params.Values.ToString() +
                    "----- Exception Occured = " + expOccured.InnerException.ToString());

                }
                {
                    //create
                    System.IO.File.Create(fileName);
                    //append
                    System.IO.File.Open(fileName, System.IO.FileMode.Append);
                    System.IO.File.WriteAllText(fileName, "Error occured - Object = " + Obj.ToString() + Environment.NewLine +
                    " --- CommandText = " + Environment.NewLine +
                    " --- Params = " + Params.Values.ToString() +
                    "----- Exception Occured = " + expOccured.InnerException.ToString());
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

    }
}
