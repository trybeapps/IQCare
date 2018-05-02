using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
namespace Interface.Laboratory
{
    public interface ILabFunctions
    {
        DataTable SaveNewLabOrder(Hashtable ht, DataTable dt, string strCustomField, string paperless, DataTable theCustomFieldData);
        DataSet GetPatientInfo(string patientid);
        DataSet GetPatientLabOrder(string PatientID);
        DataSet GetPatientLab(String LabId);
        DataSet GetLabValues();
        int DeleteLabForms(string FormName, int OrderNo, int PatientId,int UserID);
        DataTable GetEmployeeDetails();
        DataTable GetLaborderdate(int PatientID, int LocationID, int LabId);
        DataTable GetBmiValue(int PatientID, int LocationID, int VisitID, int statusHW);
        DataSet SaveLabOrderTests(int Ptn_pk, int LocationID, DataTable ParameterID, int UserId, int OrderedByName, string OrderedByDate, string LabID, int FlagExist, string PreClinicLabDate);
        DataSet GetPatientLabTestID(String LabId);
        DataTable ReturnLabQuery(string theQuery);
        DataSet GetPatientRecordformStatus(int PatientID);
        DataSet GetPatientLabTestIDTouch(BIQTouchLabFields LabFields);
        DataSet GetLabs();
        DataSet GetPreDefinedLablist(int SystemId);
        #region IQTouch Methord Declaration
        DataSet IQTouchGetPatientLabTestID(BIQTouchLabFields labFields);
        DataSet IQTouchGetlabDemo(BIQTouchLabFields labFields);
        DataSet IQTouchLaboratory_GetLabOrder(BIQTouchLabFields labFields);
        DataSet IQTouchLaboratoryGetArvMutationMasterList(BIQTouchLabFields labFields);
        DataSet IQTouchLaboratoryGetArvMutationDetails(BIQTouchLabFields labFields);
        DataSet IQTouchLaboratoryGetGenXpertDetails(BIQTouchLabFields objLabFields, int TestId);
        int IQTouchSaveLabOrderTests(BIQTouchLabFields labFields, List<BIQTouchLabFields> labIds, List<BIQTouchLabFields> ArvMutationFields, DataTable DTGenXpert);
        #endregion


    }
    #region IQTouchLabTest Property Decaration Class
    [Serializable()]
    public class BIQTouchLabFields
    {
        public int Ptnpk { get; set; }
        public int LocationId { get; set; }
        public int UserId { get; set; }
        public string Flag { get; set; }
        public int IntFlag { get; set; }
        public int LabTestID { get; set; }
        public string LabTestName { get; set; }
        public string LabTestIDs { get; set; }
        public int OrderedByName { get; set; }
        public int SubTestID { get; set; }
        public DateTime OrderedByDate { get; set; }
        public DateTime PreClinicLabDate { get; set; }
        public int LabOrderId { get; set; }
        public string TestResults { get; set; }
        public int TestResultId { get; set; }
        public int ReportedByName { get; set; }
        public DateTime ReportedByDate { get; set; }
        private int _arvTypeID = 0;
        private int _MutationID = 0;
        private string _OtherMutation = "";
        


        public int ArvTypeID
        {
            get
            {
                return _arvTypeID;
            }
            set
            {
                _arvTypeID = value;
            }
        }
        public int MutationID
        {
            get
            {
                return _MutationID;
            }
            set
            {
                _MutationID = value;
            }
        }
        public string OtherMutation
        {
            get
            {
                return _OtherMutation;
            }
            set
            {
                _OtherMutation = value;
            }
        }


        




    }
    #endregion
}
