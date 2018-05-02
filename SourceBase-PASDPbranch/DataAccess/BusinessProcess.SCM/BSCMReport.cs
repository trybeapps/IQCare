using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DataAccess.Base;
using DataAccess.Common;
using DataAccess.Entity;
using Interface.SCM;
using Application.Common;
using System.Collections;

namespace BusinessProcess.SCM
{
    public class BSCMReport : ProcessBase, ISCMReport
    {
        #region "Constructor"

        public BSCMReport()
        {
        }

        #endregion
        public DataTable GetExperyReport(int StoreId, DateTime TransDate, DateTime ExpiryDate)
        {
            lock (this)
            {
                try
                {
                    ClsUtility.Init_Hashtable();
                    ClsObject objPOdetails = new ClsObject();
                    ClsUtility.AddParameters("@Storeid", SqlDbType.Int, StoreId.ToString());
                    ClsUtility.AddParameters("@TransDate", SqlDbType.Date, TransDate.ToString());
                    ClsUtility.AddParameters("@ExpiryDate", SqlDbType.Date, ExpiryDate.ToString());
                    return
                        (DataTable)
                        objPOdetails.ReturnObject(ClsUtility.theParams, "pr_SCM_GetExperyReport_Futures",
                                                  ClsUtility.ObjectEnum.DataTable);
                }
                catch
                {
                    //DataMgr.RollBackTransation(this.Transaction);
                    throw;
                }
                finally
                {
                    if (this.Connection != null)
                        DataMgr.ReleaseConnection(this.Connection);
                }
            }
        }
        public DataSet GetStockSummary(int StoreId, int ItemId, DateTime FromDate, DateTime ToDate)
        {
            lock (this)
            {
                try
                {
                    ClsUtility.Init_Hashtable();
                    ClsObject objPOdetails = new ClsObject();
                    ClsUtility.AddParameters("@StoreId", SqlDbType.Int, StoreId.ToString());
                    ClsUtility.AddParameters("@ItemId", SqlDbType.Int, ItemId.ToString());
                    ClsUtility.AddParameters("@FromDate", SqlDbType.DateTime, FromDate.ToString());
                    ClsUtility.AddParameters("@ToDate", SqlDbType.DateTime, ToDate.ToString());
                    return
                        (DataSet)
                        objPOdetails.ReturnObject(ClsUtility.theParams, "pr_SCM_GetStockSummary_Futures",
                                                  ClsUtility.ObjectEnum.DataSet);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (this.Connection != null)
                        DataMgr.ReleaseConnection(this.Connection);
                }
            }
        }
        public DataSet GetBatchSummary(int StoreId, int ItemId, DateTime FromDate, DateTime ToDate)
        {
            lock (this)
            {
                try
                {
                    ClsUtility.Init_Hashtable();
                    ClsObject objPOdetails = new ClsObject();
                    ClsUtility.AddParameters("@StoreId", SqlDbType.Int, StoreId.ToString());
                    ClsUtility.AddParameters("@ItemId", SqlDbType.Int, ItemId.ToString());
                    ClsUtility.AddParameters("@FromDate", SqlDbType.DateTime, FromDate.ToString());
                    ClsUtility.AddParameters("@ToDate", SqlDbType.DateTime, ToDate.ToString());
                    return
                        (DataSet)
                        objPOdetails.ReturnObject(ClsUtility.theParams, "pr_SCM_GetBatchSummary_Futures",
                                                  ClsUtility.ObjectEnum.DataSet);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (this.Connection != null)
                        DataMgr.ReleaseConnection(this.Connection);
                }
            }
        }
        public DataSet GetStockLedgerData(int StoreId, DateTime FromDate, DateTime ToDate)
        {
            lock (this)
            {
                try
                {
                    ClsUtility.Init_Hashtable();
                    ClsObject objPOdetails = new ClsObject();
                    ClsUtility.AddParameters("@StoreId", SqlDbType.Int, StoreId.ToString());
                    ClsUtility.AddParameters("@StartDate", SqlDbType.DateTime, FromDate.ToString());
                    ClsUtility.AddParameters("@EndDate", SqlDbType.DateTime, ToDate.ToString());
                    ClsUtility.AddParameters("@Password", SqlDbType.VarChar, ApplicationAccess.DBSecurity);
                    return
                        (DataSet)
                        objPOdetails.ReturnObject(ClsUtility.theParams, "Pr_SCM_GetStockLedger_Futures",
                                                  ClsUtility.ObjectEnum.DataSet);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (this.Connection != null)
                        DataMgr.ReleaseConnection(this.Connection);
                }
            }

        }

        public DataSet GetBINCard(int StoreId, int ItemId, DateTime FromDate, DateTime ToDate, int LocationId)
        {
            lock (this)
            {
                try
                {
                    ClsUtility.Init_Hashtable();
                    ClsObject objPOdetails = new ClsObject();
                    ClsUtility.AddParameters("@StoreId", SqlDbType.Int, StoreId.ToString());
                    ClsUtility.AddParameters("@ItemId", SqlDbType.Int, ItemId.ToString());
                    ClsUtility.AddParameters("@FromDate", SqlDbType.DateTime, FromDate.ToString());
                    ClsUtility.AddParameters("@ToDate", SqlDbType.DateTime, ToDate.ToString());
                    ClsUtility.AddParameters("@LocationId", SqlDbType.Int, LocationId.ToString());
                    return (DataSet)objPOdetails.ReturnObject(ClsUtility.theParams, "pr_SCM_BinCard_Futures", ClsUtility.ObjectEnum.DataSet);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (this.Connection != null)
                        DataMgr.ReleaseConnection(this.Connection);
                }
            }
        }
    }
}
