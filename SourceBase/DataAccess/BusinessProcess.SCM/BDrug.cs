using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DataAccess.Base;
using DataAccess.Common;
using DataAccess.Entity;
using Interface.SCM;
using Application.Common;
using System.Collections;

namespace BusinessProcess.SCM
{
    public class BDrug : ProcessBase, IDrug
    {
        #region "Constructor"
        public BDrug()
        {
        }
        #endregion

        public DataSet GetPharmacyDispenseMasters(Int32 thePatientId, Int32 theStoreId)
        {
            lock (this)
            {
                ClsUtility.Init_Hashtable();
                ClsUtility.AddParameters("@Ptn_Pk", SqlDbType.Int, thePatientId.ToString());
                ClsUtility.AddParameters("@StoreId", SqlDbType.Int, theStoreId.ToString());
                ClsObject theManager = new ClsObject();
                return (DataSet)theManager.ReturnObject(ClsUtility.theParams, "Pr_SCM_GetPharmacyDispenseMasters_Futures", ClsDBUtility.ObjectEnum.DataSet);
            }
        }
        public DataSet CheckDispencedDate(Int32 thePatientId, Int32 LocationID, DateTime theDispDate, Int32 theOrderId)
        {
            ClsUtility.Init_Hashtable();
            ClsUtility.AddParameters("@Ptn_Pk", SqlDbType.Int, thePatientId.ToString());
            ClsUtility.AddParameters("@LocationId", SqlDbType.Int, LocationID.ToString());
            ClsUtility.AddParameters("@DispensedByDate", SqlDbType.DateTime, theDispDate.ToString());
            ClsUtility.AddParameters("@OrderId", SqlDbType.Int, theOrderId.ToString());
            ClsObject theManager = new ClsObject();
            return (DataSet)theManager.ReturnObject(ClsUtility.theParams, "pr_SCM_CheckDispencedDate_Futures", ClsDBUtility.ObjectEnum.DataSet);
        }
        public DataTable SavePharmacyDispense(Int32 thePatientId, Int32 theLocationId, Int32 theStoreId, Int32 theUserId, DateTime theDispDate,
            Int32 theOrderType, Int32 theProgramId, string theRegimen, Int32 theOrderId, DataTable theDT, DateTime PharmacyRefillDate)
        {
            DataTable thePharmacyDT = new DataTable();
            try
            {
                this.Connection = DataMgr.GetConnection();
                this.Transaction = DataMgr.BeginTransaction(this.Connection);
                ClsObject theManager = new ClsObject();
                ClsUtility.Init_Hashtable();
                ClsUtility.AddParameters("@Ptn_Pk", SqlDbType.Int, thePatientId.ToString());
                ClsUtility.AddParameters("@LocationId", SqlDbType.Int, theLocationId.ToString());
                ClsUtility.AddParameters("@DispensedBy", SqlDbType.Int, theUserId.ToString());
                ClsUtility.AddParameters("@DispensedByDate", SqlDbType.VarChar, String.Format("{0:dd-MMM-yyyy}", theDispDate).ToString());
                ClsUtility.AddParameters("@OrderType", SqlDbType.Int, theOrderType.ToString());
                ClsUtility.AddParameters("@ProgramId", SqlDbType.Int, theProgramId.ToString());
                ClsUtility.AddParameters("@StoreId", SqlDbType.Int, theStoreId.ToString());
                ClsUtility.AddParameters("@Regimen", SqlDbType.VarChar, theRegimen);
                ClsUtility.AddParameters("@UserId", SqlDbType.Int, theUserId.ToString());
                ClsUtility.AddParameters("@OrderId", SqlDbType.Int, theOrderId.ToString());
                ClsUtility.AddParameters("@PharmacyRefillAppDate", SqlDbType.DateTime, PharmacyRefillDate.ToString());
                thePharmacyDT = (DataTable)theManager.ReturnObject(ClsUtility.theParams, "pr_SCM_SavePharmacyDispenseOrder_Futures", ClsDBUtility.ObjectEnum.DataTable);

                foreach (DataRow theDR in theDT.Rows)
                {
                    ClsUtility.Init_Hashtable();
                    ClsUtility.AddParameters("@Ptn_Pk", SqlDbType.Int, thePatientId.ToString());
                    ClsUtility.AddParameters("@StoreId", SqlDbType.Int, theStoreId.ToString());
                    ClsUtility.AddParameters("@VisitId", SqlDbType.Int, thePharmacyDT.Rows[0]["VisitId"].ToString());
                    ClsUtility.AddParameters("@Ptn_Pharmacy_Pk", SqlDbType.Int, thePharmacyDT.Rows[0]["Ptn_Pharmacy_Pk"].ToString());
                    ClsUtility.AddParameters("@Drug_Pk", SqlDbType.Int, theDR["ItemId"].ToString());
                    ClsUtility.AddParameters("@StrengthId", SqlDbType.Int, theDR["StrengthId"].ToString());
                    ClsUtility.AddParameters("@FrequencyId", SqlDbType.Int, theDR["FrequencyId"].ToString());
                    ClsUtility.AddParameters("@DispensedQuantity", SqlDbType.Int, Convert.ToInt32(theDR["QtyDisp"]).ToString());
                    ClsUtility.AddParameters("@Prophylaxis", SqlDbType.Int, theDR["Prophylaxis"].ToString());
                    ClsUtility.AddParameters("@BatchId", SqlDbType.Int, theDR["BatchId"].ToString());
                    ClsUtility.AddParameters("@CostPrice", SqlDbType.Decimal, theDR["CostPrice"].ToString() != "" ? theDR["CostPrice"].ToString() : "0");
                    if (theDR["BatchNo"].ToString().Contains("("))
                    {
                        ClsUtility.AddParameters("@BatchNo", SqlDbType.VarChar, theDR["BatchNo"].ToString().Substring(0, theDR["BatchNo"].ToString().IndexOf('(')));
                    }
                    else
                    {
                        ClsUtility.AddParameters("@BatchNo", SqlDbType.VarChar, theDR["BatchNo"].ToString());
                    }
                    ClsUtility.AddParameters("@Margin", SqlDbType.Decimal, theDR["Margin"].ToString() != "" ? theDR["Margin"].ToString() : "0");
                    ClsUtility.AddParameters("@SellingPrice", SqlDbType.Decimal, theDR["SellingPrice"].ToString() != "" ? theDR["SellingPrice"].ToString() : "0");
                    ClsUtility.AddParameters("@BillAmount", SqlDbType.Decimal, theDR["BillAmount"].ToString() != "" ? theDR["BillAmount"].ToString() : "0");
                    ClsUtility.AddParameters("@ExpiryDate", SqlDbType.DateTime, theDR["ExpiryDate"].ToString());
                    ClsUtility.AddParameters("@DispensingUnit", SqlDbType.Int, theDR["DispensingUnitId"].ToString());
                    ClsUtility.AddParameters("@DispensedByDate", SqlDbType.DateTime, theDispDate.ToString());
                    ClsUtility.AddParameters("@LocationId", SqlDbType.Int, theLocationId.ToString());
                    ClsUtility.AddParameters("@UserId", SqlDbType.Int, theUserId.ToString());
                    ClsUtility.AddParameters("@DataStatus", SqlDbType.Int, theDR["DataStatus"].ToString());
                    ClsUtility.AddParameters("@PrescribeOrderedQuantity", SqlDbType.Decimal, theDR["OrderedQuantity"].ToString() != "" ? theDR["OrderedQuantity"].ToString() : "0");
                    ClsUtility.AddParameters("@Dose", SqlDbType.Decimal, theDR["Dose"].ToString() != "" ? theDR["Dose"].ToString() : "0");
                    ClsUtility.AddParameters("@Duration", SqlDbType.Decimal, theDR["Duration"].ToString() != "" ? theDR["Duration"].ToString() : "0");
                    ClsUtility.AddParameters("@PrintPrescriptionStatus", SqlDbType.Int, theDR["PrintPrescriptionStatus"].ToString());
                    ClsUtility.AddParameters("@PatientInstructions", SqlDbType.VarChar, theDR["PatientInstructions"].ToString());
                    Int32 theRowCount = (Int32)theManager.ReturnObject(ClsUtility.theParams, "pr_SCM_SavePharmacyDispenseOrderDetail_Futures", ClsDBUtility.ObjectEnum.ExecuteNonQuery);

                }

                //    return thePharmacyDT;

                DataMgr.CommitTransaction(this.Transaction);
                DataMgr.ReleaseConnection(this.Connection);
            }
            catch
            {
                DataMgr.RollBackTransation(this.Transaction);
                throw;
            }
            finally
            {
                if (this.Connection != null)
                    DataMgr.ReleaseConnection(this.Connection);
            }
            return thePharmacyDT;
        }

        public DataTable GetPharmacyExistingRecord(Int32 thePatientId, Int32 theStoreId)
        {
            ClsUtility.Init_Hashtable();
            ClsUtility.AddParameters("@Ptn_Pk", SqlDbType.Int, thePatientId.ToString());
            ClsUtility.AddParameters("@StoreId", SqlDbType.Int, theStoreId.ToString());
            ClsObject theManager = new ClsObject();
            return (DataTable)theManager.ReturnObject(ClsUtility.theParams, "pr_SCM_GetExistingPharmacyDispense_Futures", ClsDBUtility.ObjectEnum.DataTable);
        }

        public DataSet GetPharmacyExistingRecordDetails(Int32 theOrderId)
        {
            ClsUtility.Init_Hashtable();
            ClsUtility.AddParameters("@Ptn_Pharmacy_Pk", SqlDbType.Int, theOrderId.ToString());
            ClsObject theManager = new ClsObject();
            return (DataSet)theManager.ReturnObject(ClsUtility.theParams, "pr_SCM_GetPharmacyOrderDetail_Futures", ClsDBUtility.ObjectEnum.DataSet);
        }
        public DataSet GetPharmacyPrescriptionDetails(int PharmacyID, int PatientId, int IQCareFlag)
        {
            ClsObject PharmacyManager = new ClsObject();
            ClsUtility.Init_Hashtable();
            ClsUtility.AddParameters("@Ptn_Pharmacy_Pk", SqlDbType.Int, PharmacyID.ToString());
            ClsUtility.AddParameters("@PatientID", SqlDbType.Int, PatientId.ToString());
            ClsUtility.AddParameters("@IQCareFlag", SqlDbType.Int, IQCareFlag.ToString());
            ClsUtility.AddParameters("@password", SqlDbType.VarChar, ApplicationAccess.DBSecurity);
            return (DataSet)PharmacyManager.ReturnObject(ClsUtility.theParams, "pr_SCM_GetPharmacyPrescription_Futures", ClsDBUtility.ObjectEnum.DataSet);

        }
        public DataSet GetPharmacyDetailsByDespenced(Int32 theOrderId)
        {
            ClsUtility.Init_Hashtable();
            ClsUtility.AddParameters("@Ptn_Pharmacy_Pk", SqlDbType.Int, theOrderId.ToString());
            ClsObject theManager = new ClsObject();
            return (DataSet)theManager.ReturnObject(ClsUtility.theParams, "pr_SCM_GetPharmacyDetailsByDispenced_Futures", ClsDBUtility.ObjectEnum.DataSet);
        }
        public DataSet GetDrugTypeID(Int32 ItemID)
        {
            ClsUtility.Init_Hashtable();
            ClsUtility.AddParameters("@Drug_Pk", SqlDbType.Int, ItemID.ToString());
            ClsObject theManager = new ClsObject();
            return (DataSet)theManager.ReturnObject(ClsUtility.theParams, "pr_GetDrugTypeId_futures", ClsDBUtility.ObjectEnum.DataSet);
        }
        public DataSet SaveArtData(Int32 PatientID, DateTime dispencedDate)
        {
            ClsUtility.Init_Hashtable();
            ClsUtility.AddParameters("@Ptn_Pk", SqlDbType.Int, PatientID.ToString());
            ClsUtility.AddParameters("@dispencedDate", SqlDbType.DateTime, dispencedDate.ToString());
            ClsObject theManager = new ClsObject();
            return (DataSet)theManager.ReturnObject(ClsUtility.theParams, "pr_SCM_SaveUpdateArtData_Futures", ClsDBUtility.ObjectEnum.DataSet);
        }
        public void SavePharmacyReturn(Int32 thePatientId, Int32 theLocationId, Int32 theStoreId, DateTime theReturnDate, Int32 theUserId, Int32 thePharmacyId, DataTable theDT)
        {
            try
            {
                this.Connection = DataMgr.GetConnection();
                this.Transaction = DataMgr.BeginTransaction(this.Connection);
                ClsObject theManager = new ClsObject();

                foreach (DataRow theDR in theDT.Rows)
                {
                    if (Convert.ToInt32(theDR["ReturnQty"]) > 0)
                    {
                        ClsUtility.Init_Hashtable();
                        ClsUtility.AddParameters("@Ptn_Pk", SqlDbType.Int, thePatientId.ToString());
                        ClsUtility.AddParameters("@StoreId", SqlDbType.Int, theStoreId.ToString());
                        ClsUtility.AddParameters("@VisitId", SqlDbType.Int, theDR["visitId"].ToString());
                        ClsUtility.AddParameters("@Ptn_Pharmacy_Pk", SqlDbType.Int, thePharmacyId.ToString());
                        ClsUtility.AddParameters("@Drug_Pk", SqlDbType.Int, theDR["ItemId"].ToString());
                        ClsUtility.AddParameters("@StrengthId", SqlDbType.Int, theDR["StrengthId"].ToString());
                        ClsUtility.AddParameters("@FrequencyId", SqlDbType.Int, theDR["FrequencyId"].ToString());
                        ClsUtility.AddParameters("@ReturnQuantity", SqlDbType.Int, theDR["ReturnQty"].ToString());
                        ClsUtility.AddParameters("@ReturnReason", SqlDbType.Int, theDR["ReturnReason"].ToString());
                        ClsUtility.AddParameters("@Prophylaxis", SqlDbType.Int, theDR["Prophylaxis"].ToString());
                        ClsUtility.AddParameters("@BatchId", SqlDbType.Int, theDR["BatchId"].ToString());
                        ClsUtility.AddParameters("@CostPrice", SqlDbType.Decimal, theDR["CostPrice"].ToString());
                        ClsUtility.AddParameters("@Margin", SqlDbType.Decimal, theDR["Margin"].ToString());
                        ClsUtility.AddParameters("@SellingPrice", SqlDbType.Decimal, theDR["SellingPrice"].ToString());
                        ClsUtility.AddParameters("@BillAmount", SqlDbType.Decimal, theDR["BillAmount"].ToString());
                        ClsUtility.AddParameters("@ExpiryDate", SqlDbType.DateTime, theDR["ExpiryDate"].ToString());
                        ClsUtility.AddParameters("@DispensingUnit", SqlDbType.Int, theDR["DispensingUnitId"].ToString());
                        ClsUtility.AddParameters("@ReturnDate", SqlDbType.DateTime, theReturnDate.ToString());
                        ClsUtility.AddParameters("@LocationId", SqlDbType.Int, theLocationId.ToString());
                        ClsUtility.AddParameters("@UserId", SqlDbType.Int, theUserId.ToString());
                        Int32 theRowCount = (Int32)theManager.ReturnObject(ClsUtility.theParams, "pr_SCM_SavePharmacyReturnDetail_Futures", ClsDBUtility.ObjectEnum.ExecuteNonQuery);
                    }
                }
                DataMgr.CommitTransaction(this.Transaction);
                DataMgr.ReleaseConnection(this.Connection);
            }
            catch
            {
                DataMgr.RollBackTransation(this.Transaction);
                throw;
            }
            finally
            {
                if (this.Connection != null)
                    DataMgr.ReleaseConnection(this.Connection);
            }

        }

        public DataSet SaveHivTreatementPharmacyField(Int32 theOrderId, string weight, string height, int Program, int PeriodTaken, int Provider, int RegimenLine, DateTime NxtAppDate, int Reason)
        {
            ClsUtility.Init_Hashtable();
            ClsUtility.AddParameters("@OrderID", SqlDbType.Int, theOrderId.ToString());
            ClsUtility.AddParameters("@weight", SqlDbType.VarChar, weight.ToString());
            ClsUtility.AddParameters("@height", SqlDbType.VarChar, height.ToString());
            ClsUtility.AddParameters("@Programe", SqlDbType.Int, Program.ToString());
            ClsUtility.AddParameters("@Periodtaken", SqlDbType.Int, PeriodTaken.ToString());
            ClsUtility.AddParameters("@Provider", SqlDbType.Int, Provider.ToString());
            ClsUtility.AddParameters("@RegimenLine", SqlDbType.Int, RegimenLine.ToString());
            ClsUtility.AddParameters("@NxtAppDate", SqlDbType.DateTime, NxtAppDate.ToString());
            ClsUtility.AddParameters("@Region", SqlDbType.Int, Reason.ToString());

            ClsObject theManager = new ClsObject();
            return (DataSet)theManager.ReturnObject(ClsUtility.theParams, "pr_SCM_SaveUpdateHivTreatementPharmacyField_Futures", ClsDBUtility.ObjectEnum.DataSet);
        }

        public DataTable GetPersonDispensingDrugs(string UserName)
        {
            lock (this)
            {
                ClsUtility.Init_Hashtable();
                ClsUtility.AddParameters("@UserName", SqlDbType.VarChar, UserName.ToString());
                ClsObject theManager = new ClsObject();
                return (DataTable)theManager.ReturnObject(ClsUtility.theParams, "Pr_SCM_GetPersonDispensingDrugs", ClsDBUtility.ObjectEnum.DataTable);
            }
        }

        public DataTable CheckPaperlessClinic(int LocationID)
        {
            lock (this)
            {
                ClsUtility.Init_Hashtable();
                ClsUtility.AddParameters("@LocationID", SqlDbType.Int, LocationID.ToString());
                ClsObject theManager = new ClsObject();
                return (DataTable)theManager.ReturnObject(ClsUtility.theParams, "Pr_SCM_CheckPaperlessClinic", ClsDBUtility.ObjectEnum.DataTable);
            }
        }








    }
}
