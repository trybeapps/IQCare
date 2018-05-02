#region Usings
//.Net Libs
using System;
using System.Data;
using System.Collections;
using System.Reflection;

//IQCare Libs
using Interface.Clinical;
using Application.Presentation;
using Touch.FormObjects;
using BusinessProcess.Clinical;

//Third Party Libs
using Telerik.Web.UI;

#endregion

namespace Touch
{
    /// <summary>
    /// Holds search methods for IQCare and third party Databases
    /// </summary>
    public class Search
    {
        public static DataTable All(RadComboBox theServiceDropDownList, string lastName, string firstName, string identificationNo, string DOB, string FacilityId, string folderNo = null)
        {
            IIQTouchPatientRegistration PatientManager;
            PatientManager = (IIQTouchPatientRegistration)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BIQTouchPatientRegistration, BusinessProcess.Clinical");
            DataSet dsPatient = new DataSet();
            IQCareUtils theUtil = new IQCareUtils();
            DOB = theUtil.MakeDate(DOB);
            dsPatient = PatientManager.GetPatientSearchResults(Convert.ToInt32(FacilityId), lastName.Replace("'", "''").ToString(), "",
                firstName.Replace("'", "''").ToString(), identificationNo, "",
                Convert.ToDateTime(DOB), "", Convert.ToInt32(theServiceDropDownList.SelectedValue), folderNo.ToString());

            //if (TouchGlobals.D9Search)
            //{

            //}

            return dsPatient.Tables[0];
        }
    }

    public class FormValues
    {
        public static bool Update<T>(T theForm, bool CommitToDB)
        {
            bool UpdateYN = false;
            try
            {
                object _theForm = (object)theForm;
                if ((new objRegistration()).Equals(theForm.GetType()))
                {
                    TouchGlobals.AllForms.Registration = (objRegistration)_theForm; UpdateYN = true;
                    //if CommitToDB is set then save the form to DB directly
                    if (CommitToDB)
                        UpdateYN = Commit<objRegistration>((objRegistration)_theForm);
                }
                else if ((new objPharmacy()).Equals(theForm.GetType()))
                {
                    TouchGlobals.AllForms.Pharmacy = (objPharmacy)_theForm; UpdateYN = true;
                    if (CommitToDB)
                        UpdateYN = Commit<objPharmacy>((objPharmacy)_theForm);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return UpdateYN;
    }

        public static bool Commit<T>(T theForm)
        {
            if ((new objRegistration()).Equals(theForm.GetType()))
                return true;

            return true;
        }
    }

    /// <summary>
    /// Allows for Transfers of patients
    /// </summary>
    public class Transfer
    {


        public static bool PatientToFacility(string patientId, string oldLocationId, string newLocationId, int AppUserId)
        {
            IPatientTransfer PatientTransferMgr = (IPatientTransfer)ObjectFactory.CreateInstance("BusinessProcess.Clinical.BPatientTransfer, BusinessProcess.Clinical");
            DataTable theDT = PatientTransferMgr.GetSatelliteID(patientId);
            string FromLocationID = theDT.Rows[0]["LocationID"].ToString();
            int Transfer = (int)PatientTransferMgr.SaveUpdate("", patientId, oldLocationId, newLocationId, DateTime.Now.ToString("dd-MMM-yyyy"), AppUserId, "", 0);
            return Transfer > 0 ? true: false;
    }
    }



}
