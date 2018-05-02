using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
namespace Interface.Administration
{
    public interface ILabMst
    {
        DataSet GetDepartments();
        DataSet GetLabs();
        DataSet GetLabType();
        DataSet GetLabTypeByID(int labtypeid);
        DataSet GetLabByID(int labid);
        DataSet GetDropDowns();
       // DataSet DeleteLab(int LabID);
        DataSet DeleteLabtype(int labtypeid);
        DataTable SaveNewLab(string LabName, int DepartmentID, int LabTypeID, int UserID,string DataType,decimal MaxBoundary,decimal MinBoundary,  int Sequence);
        DataTable UpdateLab(int LabID, string LabName, int LabDepartmentID, int LabTypeID, int UserID, int DeleteFlag, string DataType, decimal MaxBoundary, decimal MinBoundary, int LabValueId, int Sequence);
        //int SaveNewLabType(string LabTypeName, int UserID);
        //int UpdateLabType(int LabTypeID, string LabTypeName, int UserID);
        DataSet GetLabTestList();
        DataSet GetSubTestDetails(int SubTestID);
        DataTable SaveLabUnitLinks(int ID, int SubTestID, decimal MinBoundaryValue, decimal MaxBoundaryValue, int UnitID, int DefaultUnit);
        DataTable ChangeDefaultUnit(int ID);
        DataTable CheckDefaultUnit(int ID);
        int SaveNewLabselectList(int testid, DataTable theDTselectList, int UserID);
    }
}
