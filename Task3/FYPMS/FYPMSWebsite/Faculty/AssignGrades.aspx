<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AssignGrades.aspx.cs" Inherits="FYPMSWebsite.Faculty.AssignGrades" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <div class="form-horizontal">
        <h4><span style="text-decoration: underline; color: #800000" class="h4"><strong>Assign Grades To Students</strong></span></h4>
        <asp:Label ID="lblResultMessage" runat="server" Font-Bold="True" CssClass="label" Font-Names="Arial" Font-Size="Small"></asp:Label>
        <asp:Panel ID="pnlSelectGroup" runat="server">
            <hr />
            <div class="form-group">
                <asp:Label runat="server" Text="Group:" CssClass="control-label col-md-1" AssociatedControlID="ddlGroups"></asp:Label>
                <div class="col-md-11">
                    <asp:DropDownList ID="ddlGroups" runat="server" AutoPostBack="True" Font-Names="Arial" Font-Size="Small" OnSelectedIndexChanged="DdlGroups_SelectedIndexChanged"></asp:DropDownList>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlGroupMembers" runat="server" Visible="False">
            <hr />
            <h5><span style="text-decoration: underline; color: #800000" class="h5"><strong>Students in the Selected Group:</strong></span></h5>
            <div class="form-group">
                <div class="col-md-12">
                    <asp:GridView ID="gvStudents" runat="server" CssClass="table-condensed" BorderStyle="Solid" CellPadding="0" 
                        Font-Names="Arial" Font-Size="Small" OnRowCancelingEdit="GvStudents_RowCancelingEdit" 
                        OnRowEditing="GvStudents_RowEditing" OnRowUpdating="GvStudents_RowUpdating" OnRowDataBound="GvStudents_RowDataBound" 
                        AutoGenerateEditButton="True" DataKeyNames="STUDENTNAME">
                        <EditRowStyle Font-Names="Arial" Font-Size="Small" />
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" />
                        <RowStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:GridView>
                </div>
            </div>
            <div class="form-group">
                <br />
                <asp:Label ID="lblGradeErrorMessage" runat="server" Font-Bold="True" CssClass="label col-md-offset-1" Font-Names="Arial" Font-Size="Small"></asp:Label>
            </div>
        </asp:Panel>
    </div>
</asp:Content>