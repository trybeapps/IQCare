using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

namespace Interface.Clinical
{
    public interface IKNHStaticForms
    {
        //DataSet SaveUpdateExpressFormTriageTab(int ptn_pk, int visit_pk, int locationid, int userid, int ptnaccbycare, int caregiverrelationship, decimal temp, decimal rr, decimal hr, decimal systolic, decimal diastolic, decimal height, decimal weight, int medCond, DataTable dt, int onfollowup, string lastfollowup, DateTime visitDate, DateTime startTime);
        DataSet GetExistKNHStaticFormbydate(int PatientID, string VisitdByDate, int locationID, int Visittype);
        DataSet SaveUpdateExpressFormTriageTab(Hashtable theHT, DataTable dt, DataTable referredTo);
        DataSet GetExpressFormData(int ptn_pk, int visit_pk);
        DataSet SaveUpdateExpressFormClinicalAssessmentTab(Hashtable theHT, DataTable ARVShortTermEffects, DataTable ARVLongTermEffects, DataTable Eligiblethrough, DataTable ARTchangecode, DataTable ARTstopcode);
        DataSet SaveUpdateARVTherapy(Hashtable theHT);
        DataSet SaveUpdateTBScreening(Hashtable theHT,DataTable TBAssessment, DataTable IPTStopReason, DataTable ReviewCheckList, DataTable SignsOfHepatitis);
        //DataSet SaveUpdateExpressFormClinicalAssessmentTab(int ptn_pk, int visit_pk, int locationid, int userid,
        //    int missedAnyDoses, string specifyWhyDosesMissed, int delayedTakingMedication, int labEvaluation, string LabReviewOtherTests, int OIProphylaxis,
        //    int cotrimoxazolePrescribed, string specifyOtherOIProphylaxis, string Plan, int PwPMessageGiven, int issuedWithCondoms, string reasonCondomsNotIssued,
        //    int pregIntBeforeNxtVist, int fertilityOptions, int dualContraception, int otherFPMethod, int specifyOtherFPMethod, int screenedForCancer,
        //    int CaCervixScreeningResults, int referredForCervicalScreening, DateTime startTime);

        //DataSet SaveUpdateTBScreening(int ptn_pk, int visit_pk, int locationid, int userid, int TBFindings, int? TBAvailableResults,
        //   int SputumSmear, DateTime SputumSmearDate, int GeneExpert, DateTime GeneExpertDate, int SputumDST, DateTime SputumDSTDate,
        //   int? ChestXRay, DateTime ChestXRayDate, int? TissueBiopsy, DateTime TissueBiopsyDate, int CXRResults, string OtherCXR,
        //   int TBClassification, int PatientClassification, int TBPlan, string OtherTBPlan, int TBRegimen, string OtherTBRegimen,
        //   DateTime TBRegimenStartDate, DateTime TBRegimenEndDate, int TBTreatmentOutcome, int? IPT, DateTime INHStartDate, DateTime INHEndDate,
        //   DateTime PyridoxineStartDate, DateTime PyridoxineEndDate, int? AdherenceAddressed, int? AnyMissedDoses, int? ReferredForAdherence,
        //   string OtherTBSideEffects, int? ContactsScreenedForTB, string IfNoSpecifyWhy,
        //   DataTable TBAssessment, DataTable IPTStopReason, DataTable ReviewCheckList, double age, DateTime startTime);

        DataSet GetTBScreeningFormData(int ptn_pk, int visit_pk);
        DataSet GetPwPFormData(int ptn_pk, int visit_pk);
        DataSet GetLastRegimenDispensed(int ptn_pk);
        DataSet useExpressFormRules(int ptn_pk);
        DataSet GetExtruderData(int ptn_pk);
        string GetSignature(string tabName, int visit_pk);
        DataSet SaveUpdatePwP(Hashtable theHT, DataTable HighRisk, DataTable TransitionPreparation, DataTable ReferredTo, DataTable Counselling);
        DataSet CheckIfPreviuosTabSaved(string tabName, int visit_pk);
        DataSet GetTabID(string tabName);
        DataSet CheckIfTabSaved(int tabID, int visit_pk);
        DataTable GetPatientFeatures(int Ptn_pk);
        DataSet GetExpressFormAutoPopulatingData(int ptn_pk);
        DataSet GetTBScreeningAutoPopulatingData(int ptn_pk);
        DataSet GetPEPFormAutoPopulatingData(int ptn_pk);
        DataSet GetPwPAutoPopulatingData(int ptn_pk);
        DataSet checkDuplicateVisit(string visitDate, int visitType, int ptnPk);
        DataSet GetLatestWHOStage(int ptn_pk);
        DataSet GetPatientDrugHistory(int ptn_pk);
    }
}
