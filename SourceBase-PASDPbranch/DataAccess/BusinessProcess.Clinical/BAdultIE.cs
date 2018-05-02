using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.Base;
using Interface.Clinical;
using DataAccess.Common;
using System.Data;
using DataAccess.Entity;

namespace BusinessProcess.Clinical
{
    class BAdultIE : ProcessBase, IKNHAdultIE
    {
        public DataTable GetKnhAdultIEData(BIQAdultIE adultIEFields)
        {
            ClsUtility.Init_Hashtable();

            ClsUtility.AddParameters("@Flag", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.Flag).ToString());
            ClsUtility.AddParameters("@Ptn_pk", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.PtnPk).ToString());
            ClsUtility.AddParameters("@LocationId", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.LocationId).ToString());
            ClsUtility.AddParameters("@VisitPk", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.VisitPk).ToString());  // ID here Visit PK
            ClsUtility.AddParameters("@ID", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ID).ToString());  // ID here Visit PK
            ClsUtility.AddParameters("@fieldName", SqlDbType.VarChar, ConverTotValue.NullToInt(adultIEFields.FieldName).ToString());  // ID here Visit PK

            ClsObject GetRecs = new ClsObject();
            DataTable regDT = (DataTable)GetRecs.ReturnObject(ClsUtility.theParams, "Pr_Clinical_GetKNHAdultIE", ClsUtility.ObjectEnum.DataTable);
            return regDT;
        }
        public DataTable GetPatientVisitIdAdultIE(int PatientId, int Visittype)
        {
            ClsUtility.Init_Hashtable();
            ClsUtility.AddParameters("@Ptn_pk", SqlDbType.Int, ConverTotValue.NullToInt(PatientId).ToString());
            ClsUtility.AddParameters("@VisitType", SqlDbType.Int, ConverTotValue.NullToInt(Visittype).ToString());  // ID here Visit PK
            
            ClsObject GetRecs = new ClsObject();
            DataTable regDT = (DataTable)GetRecs.ReturnObject(ClsUtility.theParams, "Pr_Clinical_GetPatientVisitIdAdultIE", ClsUtility.ObjectEnum.DataTable);
            return regDT;
        }

        public DataSet GetKnhAdultIEFormData(BIQAdultIE adultIEFields)
        {
            ClsUtility.Init_Hashtable();
            ClsUtility.AddParameters("@Flag", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.Flag).ToString());
            ClsUtility.AddParameters("@Ptn_pk", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.PtnPk).ToString());
            ClsUtility.AddParameters("@LocationId", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.LocationId).ToString());
            ClsUtility.AddParameters("@VisitPk", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.VisitPk).ToString());  // ID here Visit PK
            ClsUtility.AddParameters("@ID", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ID).ToString());  // ID here Visit PK
            ClsUtility.AddParameters("@fieldName", SqlDbType.VarChar, ConverTotValue.NullToInt(adultIEFields.FieldName).ToString());  // ID here Visit PK

            ClsObject GetRecs = new ClsObject();
            DataSet regDS = (DataSet)GetRecs.ReturnObject(ClsUtility.theParams, "Pr_Clinical_GetKNHAdultIE", ClsUtility.ObjectEnum.DataSet);
            return regDS;
        }
        public DataTable SaveAdultIE(List<BIQAdultIE> lstadultIEFields, StringBuilder strMultiselect )
        {
            ClsObject expressManagerTest = new ClsObject();
            int theRowAffected = 0;
            DataTable theDT = new DataTable();
            try
            {
                this.Connection = DataMgr.GetConnection();
                this.Transaction = DataMgr.BeginTransaction(this.Connection);
                expressManagerTest.Connection = this.Connection;
                expressManagerTest.Transaction = this.Transaction;

                if (lstadultIEFields.Count > 0)
                {
                    foreach (var adultIEFields in lstadultIEFields)
                    {
                        
                        ClsUtility.Init_Hashtable();
                        ClsUtility.AddParameters("@ID", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ID).ToString());
                        ClsUtility.AddParameters("@Ptn_pk", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.PtnPk).ToString());
                        ClsUtility.AddParameters("@VisitId", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.VisitPk).ToString());
                        ClsUtility.AddParameters("@LocationId", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.LocationId).ToString());
                        ClsUtility.AddParameters("@UserId", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.UserId).ToString());
                        ClsUtility.AddParameters("@Temperature", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.Temperature).ToString());
                        ClsUtility.AddParameters("@RespirationRate", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.RespirationRate).ToString());
                        ClsUtility.AddParameters("@HeartRate", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.HeartRate).ToString());
                        ClsUtility.AddParameters("@SystolicBloodPressure", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.SystolicBloodPressure).ToString());
                        ClsUtility.AddParameters("@DiastolicBloodPressure", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.DiastolicBloodPressure).ToString());
                        ClsUtility.AddParameters("@Height", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.Height).ToString());
                        ClsUtility.AddParameters("@Weight", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.Weight).ToString());
                        ClsUtility.AddParameters("@HeadCircum", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.txtheadcircumference).ToString());
                        ClsUtility.AddParameters("@DiagnosisConfirmed", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.DiagnosisConfirmed).ToString());
                        ClsUtility.AddParameters("@ConfirmHIVPosDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.ConfirmHIVPosDate).ToString());
                        ClsUtility.AddParameters("@ChildAccompaniedByCaregiver", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ChildAccompaniedByCaregiver).ToString());
                        ClsUtility.AddParameters("@TreatmentSupporterRelationship", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.TreatmentSupporterRelationship).ToString());
                        ClsUtility.AddParameters("@HealthEducation", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.HealthEducation).ToString());
                        ClsUtility.AddParameters("@DisclosureStatus", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.DisclosureStatus).ToString());
                        ClsUtility.AddParameters("@Reasondisclosed", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.reasonnotdisclosed).ToString());
                        
                        ClsUtility.AddParameters("@OtherDisclosurestatus", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.otherdisclosurestatus).ToString());
                        ClsUtility.AddParameters("@SchoolingStatus", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.SchoolingStatus).ToString());
                        //ClsUtility.AddParameters("@HighestLevelattained", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.Highestlevelattained).ToString());
                        ClsUtility.AddParameters("@HIVSupportgroup", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.HIVSupportgroup).ToString());
                        ClsUtility.AddParameters("@HIVSupportGroupMembership", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.supportgroupmembership).ToString());
                        ClsUtility.AddParameters("@PatientReferredFrom", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.PatientReferredFrom).ToString());
                        ClsUtility.AddParameters("@NursesComments", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.NursesComments).ToString());
                     
                        ClsUtility.AddParameters("@PresentingComplaints", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.PresentingComplaints).ToString());
                        ClsUtility.AddParameters("@OtherPresentingcomplaints", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.otherspecifiedcomplaints).ToString());
                        ClsUtility.AddParameters("@AdditionalComplaints", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.Additionalcomplaints).ToString());
                        ClsUtility.AddParameters("@MedHistoryFP", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.MedHistoryFP).ToString());
                        ClsUtility.AddParameters("@MedHistoryLastFP", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.MedHistoryLastFP).ToString());

                        ClsUtility.AddParameters("@RespiratoryDiseaseName", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.RespiratoryDiseaseName).ToString());
                        ClsUtility.AddParameters("@RespiratoryDiseaseDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.RespiratoryDiseaseDate).ToString());
                        ClsUtility.AddParameters("@RespiratoryDiseaseTreatment", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.RespiratoryDiseaseTreatment).ToString());
                        ClsUtility.AddParameters("@CardiovascularDiseaseName", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.CardiovascularDiseaseName).ToString());
                        ClsUtility.AddParameters("@CardiovascularDiseaseDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.CardiovascularDiseaseDate).ToString());
                        ClsUtility.AddParameters("@CardiovascularDiseaseTreatment", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.CardiovascularDiseaseTreatment).ToString());
                        ClsUtility.AddParameters("@GastroIntestinalDiseaseName", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.GastroIntestinalDiseaseName).ToString());
                        ClsUtility.AddParameters("@GastroIntestinalDiseaseDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.GastroIntestinalDiseaseDate).ToString());
                        ClsUtility.AddParameters("@GastroIntestinalDiseaseTreatment", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.GastroIntestinalDiseaseTreatment).ToString());
                        ClsUtility.AddParameters("@NervousDiseaseName", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.NervousDiseaseName).ToString());
                        ClsUtility.AddParameters("@NervousDiseaseDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.NervousDiseaseDate).ToString());
                        ClsUtility.AddParameters("@NervousDiseaseTreatment", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.NervousDiseaseTreatment).ToString());
                        ClsUtility.AddParameters("@DermatologyDiseaseName", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.DermatologyDiseaseName).ToString());
                        ClsUtility.AddParameters("@DermatologyDiseaseDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.DermatologyDiseaseDate).ToString());
                        ClsUtility.AddParameters("@DermatologyDiseaseTreatment", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.DermatologyDiseaseTreatment).ToString());
                        ClsUtility.AddParameters("@MusculoskeletalDiseaseName", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.MusculoskeletalDiseaseName).ToString());
                        ClsUtility.AddParameters("@MusculoskeletalDiseaseDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.MusculoskeletalDiseaseDate).ToString());
                        ClsUtility.AddParameters("@MusculoskeletalDiseaseTreatment", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.MusculoskeletalDiseaseTreatment).ToString());
                        ClsUtility.AddParameters("@PsychiatricDiseaseName", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.PsychiatricDiseaseName).ToString());
                        ClsUtility.AddParameters("@PsychiatricDiseaseDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.PsychiatricDiseaseDate).ToString());
                        ClsUtility.AddParameters("@PsychiatricDiseaseTreatment", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.PsychiatricDiseaseTreatment).ToString());
                        ClsUtility.AddParameters("@HematologicalDiseaseName", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.HematologicalDiseaseName).ToString());
                        ClsUtility.AddParameters("@HematologicalDiseaseDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.HematologicalDiseaseDate).ToString());
                        ClsUtility.AddParameters("@HematologicalDiseaseTreatment", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.HematologicalDiseaseTreatment).ToString());
                        ClsUtility.AddParameters("@GenitalUrinaryDiseaseName", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.GenitalUrinaryDiseaseName).ToString());
                        ClsUtility.AddParameters("@GenitalUrinaryDiseaseDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.GenitalUrinaryDiseaseDate).ToString());
                        ClsUtility.AddParameters("@GenitalUrinaryDiseaseTreatment", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.GenitalUrinaryDiseaseTreatment).ToString());
                        ClsUtility.AddParameters("@OphthamologyDiseaseName", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OphthamologyDiseaseName).ToString());
                        ClsUtility.AddParameters("@OphthamologyDiseaseDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.OphthamologyDiseaseDate).ToString());
                        ClsUtility.AddParameters("@OphthamologyDiseaseTreatment", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OphthamologyDiseaseTreatment).ToString());
                        ClsUtility.AddParameters("@ENTDiseaseName", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.ENTDiseaseName).ToString());
                        ClsUtility.AddParameters("@ENTDiseaseDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.ENTDiseaseDate).ToString());
                        ClsUtility.AddParameters("@ENTDiseaseTreatment", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.ENTDiseaseTreatment).ToString()); 
                        ClsUtility.AddParameters("@OtherDiseaseName", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherDiseaseName).ToString());
                        ClsUtility.AddParameters("@OtherDiseaseDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.OtherDiseaseDate).ToString());

                        ClsUtility.AddParameters("@LMPassessmentValid", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.LMPassessmentValid).ToString());

                        ClsUtility.AddParameters("@EDD", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.EDD).ToString());
                        ClsUtility.AddParameters("@OtherDiseaseTreatment", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherDiseaseTreatment).ToString());
                        ClsUtility.AddParameters("@SchoolPerfomance", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.SchoolPerfomance).ToString());

                        ClsUtility.AddParameters("@TBAssessmentICF", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.TBAssessementICF).ToString());
                        ClsUtility.AddParameters("@TBFindings", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.TBFindings).ToString());
                        ClsUtility.AddParameters("@TBresultsAvailable", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.TBresultsAvailable).ToString());
                        ClsUtility.AddParameters("@SputumSmear", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.SputumSmear).ToString());
                        ClsUtility.AddParameters("@SputumSmearDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.SputumSmearDate).ToString());
                        ClsUtility.AddParameters("@ChestXRay", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ChestXRay).ToString());
                        ClsUtility.AddParameters("@ChestXRayDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.ChestXRayDate).ToString());
                        ClsUtility.AddParameters("@TissueBiopsy", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.TissueBiopsy).ToString());
                        ClsUtility.AddParameters("@TissueBiopsyDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.TissueBiopsyDate).ToString());
                        ClsUtility.AddParameters("@CXR", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.CXR).ToString());
                        ClsUtility.AddParameters("@OtherCXR", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherCXR).ToString());

                        ClsUtility.AddParameters("@TBType", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.TBType).ToString());
                        ClsUtility.AddParameters("@TBPatientType", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.TBPatientType).ToString());
                        ClsUtility.AddParameters("@TBPlan", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.TBPlan).ToString());
                        ClsUtility.AddParameters("@TBPlanOther", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.TBPlanOther).ToString());
                        ClsUtility.AddParameters("@TBRegimen", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.TBRegimen).ToString());
                        ClsUtility.AddParameters("@TBRegimenStartDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.TBRegimenStartDate).ToString());
                        ClsUtility.AddParameters("@TBRegimenEndDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.TBRegimenEndDate).ToString());
                        ClsUtility.AddParameters("@TBReason", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.TBRegimenother).ToString());
                        ClsUtility.AddParameters("@TBTreatmentOutcomes", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.TBTreatmentOutcomes).ToString());

                        ClsUtility.AddParameters("@NoTB", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.NoTBSign).ToString());
                        //ClsUtility.AddParameters("@pyridoxine", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.pyridoxine).ToString());
                        ClsUtility.AddParameters("@INHStartDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.INHStartDate).ToString());
                        ClsUtility.AddParameters("@INHEndDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.INHEndDate).ToString());
                        ClsUtility.AddParameters("@PyridoxineStartDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.PyridoxineStartDate).ToString());
                        ClsUtility.AddParameters("@PyridoxineEndDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.PyridoxineEndDate).ToString());
                        //ClsUtility.AddParameters("@AdherenceAssessed", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.adherenceassessed).ToString());
                        //ClsUtility.AddParameters("@DosesMissed", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.dosesmissed).ToString());
                        //ClsUtility.AddParameters("@Adherencereferred", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.adherencereferred).ToString());
                        //ClsUtility.AddParameters("@DiscontinuedIPT", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.DiscontinueIPT).ToString());
                        ClsUtility.AddParameters("@SuspectTB", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.SuspectTB).ToString());
                        ClsUtility.AddParameters("@StopINHDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.StopINHDate).ToString());
                        ClsUtility.AddParameters("@ContactsScreenedForTB", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ContactsScreenedForTB).ToString());
                        ClsUtility.AddParameters("@TBNotScreenedSpecify", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.TBNotScreenedSpecify).ToString());
                        ClsUtility.AddParameters("@LongTermMedications", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.LongTermMedications).ToString());

                        ///ClsUtility.AddParameters("@Cotrimoxazole", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.Cotrimoxazole).ToString());
                        ClsUtility.AddParameters("@HormonalContraceptivesDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.HormonalContraceptivesDate).ToString());
                        ClsUtility.AddParameters("@AntihypertensivesDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.AntihypertensivesDate).ToString());
                        ClsUtility.AddParameters("@HypoglycemicsDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.HypoglycemicsDate).ToString());
                        ClsUtility.AddParameters("@FluconazoleDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.Fluconazole).ToString());
                        ClsUtility.AddParameters("@AnticonvulsantsDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.AnticonvulsantsDate).ToString());
                        ClsUtility.AddParameters("@OtherLongTermMedications", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherLongTermMedications).ToString());
                        ClsUtility.AddParameters("@OtherCurrentLongTermMedications", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.OtherCurrentLongTermMedications).ToString());

                        //ClsUtility.AddParameters("@HIVRelatedTests", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.HIVRelatedTest).ToString());
                        ClsUtility.AddParameters("@HIVRelatedHistory", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.HIVRelatedHistory).ToString());
                        ClsUtility.AddParameters("@InitialCD4", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.InitialCD4).ToString());
                        ClsUtility.AddParameters("@InitialCD4Percent", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.InitialCD4Percent).ToString());
                        ClsUtility.AddParameters("@InitialCD4Date", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.InitialCD4Date).ToString());
                        ClsUtility.AddParameters("@HighestCD4Ever", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.HighestCD4Ever).ToString());
                        ClsUtility.AddParameters("@HighestCD4Percent", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.HighestCD4Percent).ToString());
                        ClsUtility.AddParameters("@HighestCD4EverDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.HighestCD4EverDate).ToString());
                        ClsUtility.AddParameters("@CD4atARTInitiation", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.CD4atARTInitiation).ToString());
                        ClsUtility.AddParameters("@CD4AtARTInitiationPercent", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.CD4AtARTInitiationPercent).ToString());
                        ClsUtility.AddParameters("@CD4atARTInitiationDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.CD4atARTInitiationDate).ToString());
                        ClsUtility.AddParameters("@MostRecentCD4", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.MostRecentCD4).ToString());
                        ClsUtility.AddParameters("@MostRecentCD4Percent", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.MostRecentCD4Percent).ToString());
                        ClsUtility.AddParameters("@MostRecentCD4Date", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.MostRecentCD4Date).ToString());
                        ClsUtility.AddParameters("@PreviousViralLoad", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.PreviousViralLoad).ToString());
                        ClsUtility.AddParameters("@PreviousViralLoadDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.PreviousViralLoadDate).ToString());
                        ClsUtility.AddParameters("@OtherHIVRelatedHistory", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherHIVRelatedHistory).ToString());

                        ClsUtility.AddParameters("@AnyARVExposure", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ARVExposure).ToString());
                        ClsUtility.AddParameters("@PMTC1StartDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.PMTC1StartDate).ToString());
                        ClsUtility.AddParameters("@PMTC1Regimen", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.PMTC1Regimen).ToString());
                        ClsUtility.AddParameters("@PEP1Regimen", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.PEP1Regimen).ToString());
                        ClsUtility.AddParameters("@PEP1StartDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.PEP1StartDate).ToString());
                        ClsUtility.AddParameters("@HAART1Regimen", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.HAART1Regimen).ToString());
                        ClsUtility.AddParameters("@HAART1StartDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.HAART1StartDate).ToString());
                        ClsUtility.AddParameters("@ARVExposerdosesmissed", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ARVExposerdosesmissed).ToString());
                        ClsUtility.AddParameters("@ARVExposerdelaydoses", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ARVExposerdelaydoses).ToString());
                        ClsUtility.AddParameters("@Impression", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.Impression).ToString());
                        ClsUtility.AddParameters("@Diagnosis", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.Diagnosis).ToString());

                        //ClsUtility.AddParameters("@HIVRelatedOI", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.HIVRelatedOI).ToString()); Fields are handled in Multiselect
                        //ClsUtility.AddParameters("@NonHIVRelatedOI", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.NonHIVRelatedOI).ToString()); Fields are handled in Multiselect
                        //ClsUtility.AddParameters("@InitialEvaluation", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.InitialEvaluation).ToString());
                        ClsUtility.AddParameters("@WHOStageIConditions", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.WHOStageIConditions).ToString());
                        ClsUtility.AddParameters("@WHOStageIIConditions", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.WHOStageIIConditions).ToString());
                        ClsUtility.AddParameters("@WHOStageIIIConditions", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.WHOStageIIIConditions).ToString());
                        ClsUtility.AddParameters("@WHOStageIVConditions", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.WHOStageIVConditions).ToString());
                        ClsUtility.AddParameters("@InitiationWHOstage", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.InitiationWHOstage).ToString());
                        ClsUtility.AddParameters("@WHOStage", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.WHOStage).ToString());
                        ClsUtility.AddParameters("@WABStage", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.WABStage).ToString());
                        ClsUtility.AddParameters("@TannerStaging", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.TannerStaging).ToString());
                        ClsUtility.AddParameters("@Mernarche", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.Mernarche).ToString());
                        ClsUtility.AddParameters("@MernarcheDate", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.Mernarchedate).ToString());
                        
                        
                        ClsUtility.AddParameters("@SpecifyAntibioticAllery", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.SpecifyAntibioticAllery).ToString());
                        ClsUtility.AddParameters("@OtherDrugAllergy", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherDrugAllergy).ToString());
                        //ClsUtility.AddParameters("@SpecifyARVAllergy", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.ARVDrugAllergy).ToString());
                        //ClsUtility.AddParameters("@SulfaTMPDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.SulfaTMPDate).ToString());
                        //ClsUtility.AddParameters("@DrugAllergiesToxicitiesPaeds", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.DrugAllergiesToxicitiesPaeds).ToString());
                        ClsUtility.AddParameters("@ARVSideEffects", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.AnyARVSideEffects).ToString());
                        
                        ClsUtility.AddParameters("@ShortTermEffects", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ShortTermEffects).ToString());
                        //ClsUtility.AddParameters("@OtherShortTermEffects", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherShortTermEffects).ToString()); //handled in multiselect
                        ClsUtility.AddParameters("@LongTermEffects", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.LongTermEffects).ToString());
                        //ClsUtility.AddParameters("@OtherLongtermEffects", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherLongtermEffects).ToString()); //handled in multiselect
                        ClsUtility.AddParameters("@WorkUpPlan", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.WorkUpPlan).ToString());
                      
                        ClsUtility.AddParameters("@LabEvaluation", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.LabEvaluation).ToString());
                        ClsUtility.AddParameters("@SpecifyLabEvaluation", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.LabEvaluationsub).ToString());
                        // ClsUtility.AddParameters("@Counselling", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.Counselling).ToString()); //handled in multiselect
                        // ClsUtility.AddParameters("@OtherCounselling", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherCounselling).ToString()); //handled in multiselect
                  
                        ClsUtility.AddParameters("@WardAdmission", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.WardAdmission).ToString());
                        ClsUtility.AddParameters("@ReferToSpecialistClinic", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.ReferToSpecialistClinic).ToString());
                        ClsUtility.AddParameters("@TransferOut", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.TransferOut).ToString());
                        ClsUtility.AddParameters("@ARTTreatmentPlan", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ARTTreatmentPlan).ToString());
                        ClsUtility.AddParameters("@SwitchReason", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.SwitchReason).ToString()); 
                        ClsUtility.AddParameters("@StartART", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.StartART).ToString());
                        ClsUtility.AddParameters("@ARTEligibilityCriteria", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ARTEligibilityCriteria).ToString());
                        ClsUtility.AddParameters("@OtherARTEligibilityCriteria", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherARTEligibilityCriteria).ToString());
                        
                        ClsUtility.AddParameters("@SubstituteRegimen", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.SubstituteRegimen).ToString());
                        ClsUtility.AddParameters("@NumberDrugsSubstituted", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.NumberDrugsSubstituted).ToString());
                        ClsUtility.AddParameters("@StopTreatment", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.StopTreatment).ToString());
                        ClsUtility.AddParameters("@StopTreatmentCodes", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.StopTreatmentCodes).ToString());
                        ClsUtility.AddParameters("@RegimenPrescribed", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.RegimenPrescribed).ToString());
                        ClsUtility.AddParameters("@OtherRegimenPrescribed", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherRegimenPrescribed).ToString());
                        ClsUtility.AddParameters("@OIProphylaxis", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.OIProphylaxis).ToString());
                        ClsUtility.AddParameters("@ReasonCTXPrescribed", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ReasonCoTrimoxPrescribed).ToString());
                        ClsUtility.AddParameters("@OtherTreatment", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherTreatment).ToString());

                        ClsUtility.AddParameters("@SexualActiveness", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.SexualActiveness).ToString());
                        ClsUtility.AddParameters("@SexualOrientation", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.SexualOrientation).ToString());
                        ClsUtility.AddParameters("@HighRisk", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.HighRisk).ToString());
                        ClsUtility.AddParameters("@KnowSexualPartnerHIVStatus", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.KnowSexualPartnerHIVStatus).ToString());
                        ClsUtility.AddParameters("@ParnerHIVStatus", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.PartnerHIVStatus).ToString());
                        
                        ClsUtility.AddParameters("@GivenPWPMessages", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.GivenPWPMessages).ToString());
                        ClsUtility.AddParameters("@SaferSexImportanceExplained", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.SaferSexImportanceExplained).ToString());
                        ClsUtility.AddParameters("@UnsafeSexImportanceExplained", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.UnsafeSexImportanceExplained).ToString());
                        ClsUtility.AddParameters("@LMPDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.LMPDate).ToString());
                        ClsUtility.AddParameters("@LMPNotaccessedReason", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.LMPNotaccessedReason).ToString());

                        ClsUtility.AddParameters("@PDTDone", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.PDTDone).ToString());
                        ClsUtility.AddParameters("@Pregnant", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ClientPregnant).ToString());
                        ClsUtility.AddParameters("@PMTCTOffered", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.PMTCTOffered).ToString());
                        ClsUtility.AddParameters("@IntentionOfPregnancy", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.IntentionOfPregnancy).ToString());
                        ClsUtility.AddParameters("@DiscussedFertilityOptions", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.DiscussedFertilityOptions).ToString());
                        ClsUtility.AddParameters("@DiscussedDualContraception", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.DiscussedDualContraception).ToString());
                        ClsUtility.AddParameters("@CondomsIssued", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.CondomsIssued).ToString());
                        ClsUtility.AddParameters("@CondomNotIssued", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.ReasonCondomNotIssued).ToString());
                        ClsUtility.AddParameters("@STIScreened", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.STIScreened).ToString());
                        ClsUtility.AddParameters("@VaginalDischarge", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.VaginalDischarge).ToString());
                        ClsUtility.AddParameters("@UrethralDischarge", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.UrethralDischarge).ToString());
                        ClsUtility.AddParameters("@GenitalUlceration", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.GenitalUlceration).ToString());
                        ClsUtility.AddParameters("@STITreatmentPlan", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.STITreatmentPlan).ToString());
                        ClsUtility.AddParameters("@OnFP", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.OnFP).ToString());
                        ClsUtility.AddParameters("@FPMethod", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.FPMethod).ToString());
                        ClsUtility.AddParameters("@CervicalCancerScreened", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.CervicalCancerScreened).ToString());
                        ClsUtility.AddParameters("@CervicalCancerScreeningResults", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.CervicalCancerScreeningResults).ToString());
                        ClsUtility.AddParameters("@ReferredForCervicalCancerScreening", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ReferredForCervicalCancerScreening).ToString());
                        ClsUtility.AddParameters("@HPVOffered", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.HPVOffered).ToString());
                        ClsUtility.AddParameters("@OfferedHPVVaccine", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.OfferedHPVVaccine).ToString());
                        ClsUtility.AddParameters("@HPVDoseDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.HPVDoseDate).ToString());
                        ClsUtility.AddParameters("@RefferedToFupF", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.RefferedToFupF).ToString());
                        ClsUtility.AddParameters("@SpecifyOtherRefferedTo", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.SpecifyOtherRefferedTo).ToString());
                        ClsUtility.AddParameters("@SignatureID", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.SignatureID).ToString());
                        ClsUtility.AddParameters("@VisitDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.VisitDate).ToString());
                         //ClsUtility.AddParameters("@ValueID", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ValueID).ToString());
                        //ClsUtility.AddParameters("@FieldID", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.FieldID).ToString());
                        //ClsUtility.AddParameters("@Diseasename", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.Diseasename).ToString());
                        //ClsUtility.AddParameters("@DiagnosisDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.DiagnosisDate).ToString());
                        //ClsUtility.AddParameters("@Treatment", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.Treatment).ToString());
                        //ClsUtility.AddParameters("@SectionID", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.SectionID).ToString());
                        //ClsUtility.AddParameters("@ConditionId", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ConditionId).ToString());
                        //ClsUtility.AddParameters("@OtherCondition", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherCondition).ToString());
                        //ClsUtility.AddParameters("@CurrentDate", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.CurrentDate).ToString());
                        //ClsUtility.AddParameters("@Historic", SqlDbType.DateTime, ConverTotValue.NullToDate(adultIEFields.Historic).ToString());
                        theDT = (DataTable)expressManagerTest.ReturnObject(ClsUtility.theParams, "Pr_Clinical_AddAdultIE", ClsUtility.ObjectEnum.DataTable);
                        StringBuilder Insertcbl = new StringBuilder();
                        ClsUtility.Init_Hashtable();
                        StringBuilder UpdateStrMultiselect = new StringBuilder();
                        if (Convert.ToInt32(adultIEFields.VisitPk) > 0)
                        {
                            UpdateStrMultiselect.Append("Delete from dtl_Multiselect_line where ptn_pk=" + adultIEFields.PtnPk + " and Visit_Pk=" + adultIEFields.VisitPk + " ");
                            UpdateStrMultiselect.Append(strMultiselect);
                            ClsUtility.AddParameters("@Insert", SqlDbType.VarChar, UpdateStrMultiselect.ToString());
                        }
                        else
                        {
                            ClsUtility.AddParameters("@Insert", SqlDbType.VarChar, strMultiselect.ToString());
                        }
                        theRowAffected = (int)expressManagerTest.ReturnObject(ClsUtility.theParams, "pr_Clinical_SaveCustomForm_Constella", ClsUtility.ObjectEnum.ExecuteNonQuery); 
                    }
                }

                DataMgr.CommitTransaction(this.Transaction);
                DataMgr.ReleaseConnection(this.Connection);
                return theDT;
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

        public DataTable SaveAdultIE_TriageTab(List<BIQAdultIE> lstadultIEFields, StringBuilder strMultiselect)
        {
            ClsObject expressManagerTest = new ClsObject();
            int theRowAffected = 0;
            DataTable theDT = new DataTable();
            try
            {
                this.Connection = DataMgr.GetConnection();
                this.Transaction = DataMgr.BeginTransaction(this.Connection);
                expressManagerTest.Connection = this.Connection;
                expressManagerTest.Transaction = this.Transaction;

                if (lstadultIEFields.Count > 0)
                {
                    foreach (var adultIEFields in lstadultIEFields)
                    {

                        ClsUtility.Init_Hashtable();
                        ClsUtility.AddParameters("@ID", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ID).ToString());
                        ClsUtility.AddParameters("@Ptn_pk", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.PtnPk).ToString());
                        ClsUtility.AddParameters("@VisitId", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.VisitPk).ToString());
                        ClsUtility.AddParameters("@LocationId", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.LocationId).ToString());
                        ClsUtility.AddParameters("@UserId", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.UserId).ToString());
                        ClsUtility.AddParameters("@Temperature", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.Temperature).ToString());
                        ClsUtility.AddParameters("@RespirationRate", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.RespirationRate).ToString());
                        ClsUtility.AddParameters("@HeartRate", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.HeartRate).ToString());
                        ClsUtility.AddParameters("@SystolicBloodPressure", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.SystolicBloodPressure).ToString());
                        ClsUtility.AddParameters("@DiastolicBloodPressure", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.DiastolicBloodPressure).ToString());
                        ClsUtility.AddParameters("@Height", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.Height).ToString());
                        ClsUtility.AddParameters("@Weight", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.Weight).ToString());
                        ClsUtility.AddParameters("@HeadCircum", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.txtheadcircumference).ToString());
                        ClsUtility.AddParameters("@WeightForAge", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ddlweightforage).ToString());
                        ClsUtility.AddParameters("@WeightForHeight", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.txtweightforheight).ToString());
                        ClsUtility.AddParameters("@DiagnosisConfirmed", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.DiagnosisConfirmed).ToString());
                        ClsUtility.AddParameters("@ConfirmHIVPosDate", SqlDbType.VarChar, String.Format("{0:dd-MMM-yyyy}",ConverTotValue.NullToDate(adultIEFields.ConfirmHIVPosDate).ToString()));
                        ClsUtility.AddParameters("@ChildAccompaniedByCaregiver", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ChildAccompaniedByCaregiver).ToString());
                        ClsUtility.AddParameters("@TreatmentSupporterRelationship", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.TreatmentSupporterRelationship).ToString());
                        ClsUtility.AddParameters("@HealthEducation", SqlDbType.Bit, ConverTotValue.NullToBoolean(adultIEFields.HealthEducation).ToString());
                        ClsUtility.AddParameters("@DisclosureStatus", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.DisclosureStatus).ToString());
                        ClsUtility.AddParameters("@Reasondisclosed", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.reasonnotdisclosed).ToString());
                        
                        ClsUtility.AddParameters("@OtherDisclosurestatus", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.otherdisclosurestatus).ToString());
                        ClsUtility.AddParameters("@SchoolingStatus", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.SchoolingStatus).ToString());
                        ClsUtility.AddParameters("@HighestLevelattained", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.Highestlevelattained).ToString());
                        ClsUtility.AddParameters("@HIVSupportgroup", SqlDbType.Bit, ConverTotValue.NullToBoolean(adultIEFields.HIVSupportgroup).ToString());
                        ClsUtility.AddParameters("@HIVSupportGroupMembership", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.supportgroupmembership).ToString());
                        ClsUtility.AddParameters("@PatientReferredFrom", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.PatientReferredFrom).ToString());
                        ClsUtility.AddParameters("@PatientReferredFromOthers", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherPatientReferredFrom).ToString());
                        ClsUtility.AddParameters("@NursesComments", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.NursesComments).ToString());

                        ClsUtility.AddParameters("@ReferToSpecialistClinic", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.ReferToSpecialistClinic).ToString());
                        
                        ClsUtility.AddParameters("@SpecifyOtherRefferedTo", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.SpecifyOtherRefferedTo).ToString());
                        ClsUtility.AddParameters("@SignatureID", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.SignatureID).ToString());
                        ClsUtility.AddParameters("@VisitDate", SqlDbType.VarChar, String.Format("{0:dd-MMM-yyyy}",ConverTotValue.NullToDate(adultIEFields.VisitDate).ToString()));
                        ClsUtility.AddParameters("@StartTime", SqlDbType.VarChar, String.Format("{0:yyyy-MM-dd hh:mm:ss}", DateTime.Now));
                        theDT = (DataTable)expressManagerTest.ReturnObject(ClsUtility.theParams, "Pr_clinical_addadultie_triagetab", ClsUtility.ObjectEnum.DataTable);
                        
                        
                        ClsUtility.Init_Hashtable();
                        
                        ClsUtility.AddParameters("@Insert", SqlDbType.VarChar, strMultiselect.ToString());
                       
                        theRowAffected = (int)expressManagerTest.ReturnObject(ClsUtility.theParams, "pr_Clinical_SaveCustomForm_Constella", ClsUtility.ObjectEnum.ExecuteNonQuery);
                    }
                }

                DataMgr.CommitTransaction(this.Transaction);
                DataMgr.ReleaseConnection(this.Connection);
                return theDT;
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
        public DataTable SaveAdultIE_CATab(List<BIQAdultIE> lstadultIEFields, StringBuilder strMultiselect)
        {
            ClsObject expressManagerTest = new ClsObject();
            int theRowAffected = 0;
            DataTable theDT = new DataTable();
            try
            {
                this.Connection = DataMgr.GetConnection();
                this.Transaction = DataMgr.BeginTransaction(this.Connection);
                expressManagerTest.Connection = this.Connection;
                expressManagerTest.Transaction = this.Transaction;

                if (lstadultIEFields.Count > 0)
                {
                    foreach (var adultIEFields in lstadultIEFields)
                    {

                        ClsUtility.Init_Hashtable();
                        ClsUtility.AddParameters("@ID", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ID).ToString());
                        ClsUtility.AddParameters("@Ptn_pk", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.PtnPk).ToString());
                        ClsUtility.AddParameters("@VisitId", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.VisitPk).ToString());
                        ClsUtility.AddParameters("@LocationId", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.LocationId).ToString());
                        ClsUtility.AddParameters("@UserId", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.UserId).ToString());
                        ClsUtility.AddParameters("@OtherPresentingcomplaints", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.otherspecifiedcomplaints).ToString());
                        ClsUtility.AddParameters("@AdditionalComplaints", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.Additionalcomplaints).ToString());
                        ClsUtility.AddParameters("@OtherMedicalConditions", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.otherspecifiedcomplaints).ToString());
                        ClsUtility.AddParameters("@MedHistoryFP", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.MedHistoryFP).ToString());
                        ClsUtility.AddParameters("@MedHistoryLastFP", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.MedHistoryLastFP).ToString());                        
                        //ClsUtility.AddParameters("@SignatureID", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.SignatureID).ToString());
                        ClsUtility.AddParameters("@VisitDate", SqlDbType.VarChar, String.Format("{0:dd-MMM-yyyy}",ConverTotValue.NullToDate(adultIEFields.VisitDate).ToString()));
                        ClsUtility.AddParameters("@StartTime", SqlDbType.VarChar, String.Format("{0:yyyy-MM-dd hh:mm:ss}", DateTime.Now));
                        theDT = (DataTable)expressManagerTest.ReturnObject(ClsUtility.theParams, "Pr_clinical_addadultie_CATab", ClsUtility.ObjectEnum.DataTable);
                        
                        ClsUtility.Init_Hashtable();                        
                        ClsUtility.AddParameters("@Insert", SqlDbType.VarChar, strMultiselect.ToString());
                        
                        theRowAffected = (int)expressManagerTest.ReturnObject(ClsUtility.theParams, "pr_Clinical_SaveCustomForm_Constella", ClsUtility.ObjectEnum.ExecuteNonQuery);
                    }
                }

                DataMgr.CommitTransaction(this.Transaction);
                DataMgr.ReleaseConnection(this.Connection);
                return theDT;
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
        public DataTable SaveAdultIE_ExamTab(List<BIQAdultIE> lstadultIEFields, StringBuilder strMultiselect)
        {
            ClsObject expressManagerTest = new ClsObject();
            int theRowAffected = 0;
            DataTable theDT = new DataTable();
            try
            {
                this.Connection = DataMgr.GetConnection();
                this.Transaction = DataMgr.BeginTransaction(this.Connection);
                expressManagerTest.Connection = this.Connection;
                expressManagerTest.Transaction = this.Transaction;

                if (lstadultIEFields.Count > 0)
                {
                    foreach (var adultIEFields in lstadultIEFields)
                    {

                        ClsUtility.Init_Hashtable();
                        ClsUtility.AddParameters("@ID", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ID).ToString());
                        ClsUtility.AddParameters("@Ptn_pk", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.PtnPk).ToString());
                        ClsUtility.AddParameters("@VisitId", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.VisitPk).ToString());
                        ClsUtility.AddParameters("@LocationId", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.LocationId).ToString());
                        ClsUtility.AddParameters("@UserId", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.UserId).ToString());
                        
                        ClsUtility.AddParameters("@OtherLongTermMedications", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherLongTermMedications).ToString());
                        ClsUtility.AddParameters("@OtherGeneralConditions", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherGeneralConditions).ToString());
                        ClsUtility.AddParameters("@OtherAbdomenConditions", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherAbdomenConditions).ToString());
                        ClsUtility.AddParameters("@OtherCardiovascularConditions", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherCardiovascularConditions).ToString());
                        ClsUtility.AddParameters("@OtherOralCavityConditions", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherOralCavityConditions).ToString());
                        ClsUtility.AddParameters("@OtherGenitourinaryConditions", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherGenitourinaryConditions).ToString());
                        ClsUtility.AddParameters("@OtherCNSConditions", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherCNSConditions).ToString());
                        ClsUtility.AddParameters("@OtherChestLungsConditions", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherChestLungsConditions).ToString());
                        ClsUtility.AddParameters("@OtherSkinConditions", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherSkinConditions).ToString());
                        ClsUtility.AddParameters("@OtherMedicalConditionNotes", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.Additionalmedicalconditionnotes).ToString());
                        
                        ClsUtility.AddParameters("@InitialCD4", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.InitialCD4).ToString());
                        ClsUtility.AddParameters("@InitialCD4Percent", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.InitialCD4Percent).ToString());
                        ClsUtility.AddParameters("@InitialCD4Date", SqlDbType.VarChar, String.Format("{0:dd-MMM-yyyy}",ConverTotValue.NullToDate(adultIEFields.InitialCD4Date).ToString()));
                        ClsUtility.AddParameters("@HighestCD4Ever", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.HighestCD4Ever).ToString());
                        ClsUtility.AddParameters("@HighestCD4Percent", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.HighestCD4Percent).ToString());
                        ClsUtility.AddParameters("@HighestCD4EverDate", SqlDbType.VarChar, String.Format("{0:dd-MMM-yyyy}",ConverTotValue.NullToDate(adultIEFields.HighestCD4EverDate).ToString()));
                        ClsUtility.AddParameters("@CD4atARTInitiation", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.CD4atARTInitiation).ToString());
                        ClsUtility.AddParameters("@CD4AtARTInitiationPercent", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.CD4AtARTInitiationPercent).ToString());
                        ClsUtility.AddParameters("@CD4atARTInitiationDate", SqlDbType.VarChar, String.Format("{0:dd-MMM-yyyy}",ConverTotValue.NullToDate(adultIEFields.CD4atARTInitiationDate).ToString()));
                        ClsUtility.AddParameters("@MostRecentCD4", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.MostRecentCD4).ToString());
                        ClsUtility.AddParameters("@MostRecentCD4Percent", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.MostRecentCD4Percent).ToString());
                        ClsUtility.AddParameters("@MostRecentCD4Date", SqlDbType.VarChar, String.Format("{0:dd-MMM-yyyy}",ConverTotValue.NullToDate(adultIEFields.MostRecentCD4Date).ToString()));
                        ClsUtility.AddParameters("@PreviousViralLoad", SqlDbType.Decimal, ConverTotValue.NullToInt(adultIEFields.PreviousViralLoad).ToString());
                        ClsUtility.AddParameters("@PreviousViralLoadDate", SqlDbType.VarChar, String.Format("{0:dd-MMM-yyyy}",ConverTotValue.NullToDate(adultIEFields.PreviousViralLoadDate).ToString()));
                        ClsUtility.AddParameters("@OtherHIVRelatedHistory", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherHIVRelatedHistory).ToString());

                        ClsUtility.AddParameters("@PMTC1StartDate", SqlDbType.VarChar, String.Format("{0:dd-MMM-yyyy}",ConverTotValue.NullToDate(adultIEFields.PMTC1StartDate).ToString()));
                        ClsUtility.AddParameters("@PMTC1Regimen", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.PMTC1Regimen).ToString());
                        ClsUtility.AddParameters("@PEP1Regimen", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.PEP1Regimen).ToString());
                        ClsUtility.AddParameters("@PEP1StartDate", SqlDbType.VarChar, String.Format("{0:dd-MMM-yyyy}",ConverTotValue.NullToDate(adultIEFields.PEP1StartDate).ToString()));
                        ClsUtility.AddParameters("@HAART1Regimen", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.HAART1Regimen).ToString());
                        ClsUtility.AddParameters("@HAART1StartDate", SqlDbType.VarChar, String.Format("{0:dd-MMM-yyyy}",ConverTotValue.NullToDate(adultIEFields.HAART1StartDate).ToString()));
                        ClsUtility.AddParameters("@ARVExposerdosesmissed", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ARVExposerdosesmissed).ToString());
                        ClsUtility.AddParameters("@ARVExposerdelaydoses", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ARVExposerdelaydoses).ToString());
                        ClsUtility.AddParameters("@Impression", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.Impression).ToString());
                        ClsUtility.AddParameters("@HIVRelatedOI", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.HIVRelatedOI).ToString());

                        ClsUtility.AddParameters("@ProgressionInWHOstage", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ProgressionInWHOstage).ToString());
                        ClsUtility.AddParameters("@SpecifyWHOprogression", SqlDbType.VarChar, ConverTotValue.NullToInt(adultIEFields.SpecifyWHOprogression).ToString());

                        ClsUtility.AddParameters("@NonHIVRelatedOI", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.NonHIVRelatedOI).ToString()); 
                        ClsUtility.AddParameters("@WHOStage", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.WHOStage).ToString());
                        ClsUtility.AddParameters("@WABStage", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.WABStage).ToString());
                        ClsUtility.AddParameters("@TannerStaging", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.TannerStaging).ToString());
                        ClsUtility.AddParameters("@Mernarche", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.Mernarche).ToString());
                        ClsUtility.AddParameters("@MernarcheDate", SqlDbType.VarChar, String.Format("{0:dd-MMM-yyyy}",ConverTotValue.NullToInt(adultIEFields.Mernarchedate).ToString()));
                        ClsUtility.AddParameters("@StartTime", SqlDbType.VarChar, String.Format("{0:yyyy-MM-dd hh:mm:ss}", DateTime.Now));

                        theDT = (DataTable)expressManagerTest.ReturnObject(ClsUtility.theParams, "Pr_clinical_addadultie_ExamTab", ClsUtility.ObjectEnum.DataTable);
                        
                        ClsUtility.Init_Hashtable();                        
                        ClsUtility.AddParameters("@Insert", SqlDbType.VarChar, strMultiselect.ToString());
                        
                        theRowAffected = (int)expressManagerTest.ReturnObject(ClsUtility.theParams, "pr_Clinical_SaveCustomForm_Constella", ClsUtility.ObjectEnum.ExecuteNonQuery);
                    }
                }

                DataMgr.CommitTransaction(this.Transaction);
                DataMgr.ReleaseConnection(this.Connection);
                return theDT;
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
        public DataTable SaveAdultIE_MgtTab(List<BIQAdultIE> lstadultIEFields, StringBuilder strMultiselect)
        {
            ClsObject expressManagerTest = new ClsObject();
            int theRowAffected = 0;
            DataTable theDT = new DataTable();
            try
            {
                this.Connection = DataMgr.GetConnection();
                this.Transaction = DataMgr.BeginTransaction(this.Connection);
                expressManagerTest.Connection = this.Connection;
                expressManagerTest.Transaction = this.Transaction;

                if (lstadultIEFields.Count > 0)
                {
                    foreach (var adultIEFields in lstadultIEFields)
                    {

                        ClsUtility.Init_Hashtable();
                        ClsUtility.AddParameters("@ID", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ID).ToString());
                        ClsUtility.AddParameters("@Ptn_pk", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.PtnPk).ToString());
                        ClsUtility.AddParameters("@VisitId", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.VisitPk).ToString());
                        ClsUtility.AddParameters("@LocationId", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.LocationId).ToString());
                        ClsUtility.AddParameters("@UserId", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.UserId).ToString());
                        
                        ClsUtility.AddParameters("@OtherShortTermEffects", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherShortTermEffects).ToString()); 
                        ClsUtility.AddParameters("@OtherLongtermEffects", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherLongtermEffects).ToString()); 
                        ClsUtility.AddParameters("@WorkUpPlan", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.WorkUpPlan).ToString());
                        ClsUtility.AddParameters("@SpecifyLabEvaluation", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherLabReview).ToString());
                        
                        ClsUtility.AddParameters("@ARTTreatmentPlan", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ARTTreatmentPlan).ToString());                        
                        ClsUtility.AddParameters("@OtherARTEligibilityCriteria", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherARTEligibilityCriteria).ToString());
                        ClsUtility.AddParameters("@NumberDrugsSubstituted", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.NumberDrugsSubstituted).ToString());
                        ClsUtility.AddParameters("@SpecifyotherARTchangereason", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherARTChangeCode).ToString());
                        ClsUtility.AddParameters("@OtherARTStopCode", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherARTStopCode).ToString());
                        ClsUtility.AddParameters("@secondLineRegimenSwitch", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.SubstituteRegimen).ToString());
                        //ClsUtility.AddParameters("@OtherRegimenPrescribed", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherRegimenPrescribed).ToString());
                        ClsUtility.AddParameters("@OIProphylaxis", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.OIProphylaxis).ToString());
                        ClsUtility.AddParameters("@ReasonCTXPrescribed", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ReasonCoTrimoxPrescribed).ToString());
                        ClsUtility.AddParameters("@OtherTreatment", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.OtherTreatment).ToString());
                        ClsUtility.AddParameters("@Fluconazole", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.ReasonFluconazolePrescribed).ToString());
                        ClsUtility.AddParameters("@OtherOIProphylaxis", SqlDbType.VarChar, ConverTotValue.NullToString(adultIEFields.SexualOrientation).ToString());
                        ClsUtility.AddParameters("@signature", SqlDbType.Int, ConverTotValue.NullToInt(adultIEFields.SignatureID).ToString());
                        ClsUtility.AddParameters("@VisitDate", SqlDbType.VarChar, String.Format("{0:dd-MMM-yyyy}",ConverTotValue.NullToDate(adultIEFields.VisitDate).ToString()));
                        ClsUtility.AddParameters("@StartTime", SqlDbType.VarChar, String.Format("{0:yyyy-MM-dd hh:mm:ss}", DateTime.Now));
                        theDT = (DataTable)expressManagerTest.ReturnObject(ClsUtility.theParams, "Pr_clinical_addadultie_MgtTab", ClsUtility.ObjectEnum.DataTable);
                        
                        ClsUtility.Init_Hashtable();                        
                        ClsUtility.AddParameters("@Insert", SqlDbType.VarChar, strMultiselect.ToString());                       
                        theRowAffected = (int)expressManagerTest.ReturnObject(ClsUtility.theParams, "pr_Clinical_SaveCustomForm_Constella", ClsUtility.ObjectEnum.ExecuteNonQuery);
                    }
                }

                DataMgr.CommitTransaction(this.Transaction);
                DataMgr.ReleaseConnection(this.Connection);
                return theDT;
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
    }
}
