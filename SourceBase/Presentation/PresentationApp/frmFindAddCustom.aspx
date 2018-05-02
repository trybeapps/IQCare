<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/IQCare.master" AutoEventWireup="true" CodeBehind="frmFindAddCustom.aspx.cs" Inherits="IQCare.Web.frmFindAddCustom" EnableEventValidation="False" %>

<%@ Register src="PatientFinder.ascx" tagname="PatientFinder" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="IQCareContentPlaceHolder" runat="server">
  <uc1:PatientFinder ID="FindPatient" runat="server" IncludeEnrollement="True" FilterByServiceLines="False" 
  AutoLoadRecords="False" NumberofRecords="100" CanAddPatient="True"/>
    <asp:HiddenField ID="HFormName" runat="server" />
     <asp:HiddenField ID="HPatientID" runat="server" />
      <asp:HiddenField ID="HLocationID" runat="server" />
       <asp:HiddenField ID="HModuleID" runat="server" />
</asp:Content>
