<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DisplayProjectDetails.aspx.cs" Inherits="FYPMSWebsite.DisplayProjectDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <h4><span style="text-decoration: underline; color: #800000" class="h4"><strong>Project Details</strong></span></h4>
        <asp:Label ID="lblResultMessage" runat="server" Font-Bold="True" CssClass="label" Font-Names="Arial" Font-Size="Small"></asp:Label>
        <asp:Panel ID="pnlProjectInfo" runat="server">
            <hr />
            <div class="form-group" role="row">
                <!-- Title control -->
                <asp:Label runat="server" Text="Title:" CssClass="control-label col-md-2" AssociatedControlID="txtTitle" Font-Names="Arial"
                    Font-Size="Small"></asp:Label>
                <div class="col-md-10">
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control-static" Font-Names="Arial" Font-Size="Small" 
                        ReadOnly="True" BackColor="White" BorderStyle="None" BorderWidth="0px" Width="100%"></asp:TextBox>
                </div>
            </div>
            <div class="form-group" role="row">
                <!-- Description control -->
                <asp:Label runat="server" Text="Description:" CssClass="control-label col-md-2" AssociatedControlID="txtDescription" 
                    Font-Names="Arial" Font-Size="Small"></asp:Label>
                <div class="col-md-10">
                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control-static" Font-Names="Arial" Font-Size="Small" 
                        ReadOnly="True" BackColor="White" BorderStyle="None" Height="150px" TextMode="MultiLine" BorderWidth="0px" Width="100%"></asp:TextBox>
                </div>
            </div>
            <div class="form-group" role="row">
                <!-- Supervisor control -->
                <asp:Label runat="server" Text="Supervisor(s):" CssClass="control-label col-md-2" AssociatedControlID="txtSupervisor" 
                    Font-Names="Arial" Font-Size="Small"></asp:Label>
                <div class="col-md-3">
                    <asp:TextBox ID="txtSupervisor" runat="server" CssClass="form-control-static" Font-Names="Arial" Font-Size="Small" 
                        ReadOnly="True" BackColor="White" BorderStyle="None" Width="100%"></asp:TextBox>
                </div>
                <!-- Category control -->
                <asp:Label runat="server" Text="Category:" CssClass="control-label col-md-1" AssociatedControlID="txtCategory" 
                    Font-Names="Arial" Font-Size="Small"></asp:Label>
                <div class="col-md-3">
                    <asp:TextBox ID="txtCategory" runat="server" CssClass="form-control-static" Font-Names="Arial" Font-Size="Small" 
                        ReadOnly="True" BackColor="White" BorderStyle="None" Width="100%"></asp:TextBox>
                </div>
                <!-- Project type control -->
                <asp:Label runat="server" Text="Type:" CssClass="control-label col-md-1" AssociatedControlID="txtType" 
                    Font-Names="Arial" Font-Size="Small"></asp:Label>
                <div class="col-md-2">
                    <asp:TextBox ID="txtType" runat="server" CssClass="form-control-static" Font-Names="Arial" Font-Size="Small" 
                        ReadOnly="True" BackColor="White" BorderStyle="None" Width="100%"></asp:TextBox>
                </div>
            </div>
            <div class="form-group" role="row">
                <!-- Requirement control -->
                <asp:Label runat="server" Text="Requirement:" CssClass="control-label col-md-2" AssociatedControlID="txtRequirement" 
                    Font-Names="Arial" Font-Size="Small"></asp:Label>
                <div class="col-md-10">
                    <asp:TextBox ID="txtRequirement" runat="server" CssClass="form-control-static" Font-Names="Arial" Font-Size="Small" 
                        ReadOnly="True" BackColor="White" BorderStyle="None" Width="100%"></asp:TextBox>
                </div>
            </div>
            <div class="form-group" role="row">
                <!-- Minimum number of students control -->
                <asp:Label runat="server" Text="Minimum students:" CssClass="control-label col-md-offset-2 col-md-2"
                    AssociatedControlID="txtMinStudents" Font-Names="Arial" Font-Size="Small"></asp:Label>
                <div class="col-md-3">
                    <asp:TextBox ID="txtMinStudents" runat="server" CssClass="form-control-static" Font-Names="Arial" Font-Size="Small" 
                        ReadOnly="True" BackColor="White" BorderStyle="None"></asp:TextBox>
                </div>
                <!-- Maximum number of students control -->
                <asp:Label runat="server" Text="Maximum students:" CssClass="control-label col-md-2"
                    AssociatedControlID="txtMaxStudents" Font-Names="Arial" Font-Size="Small"></asp:Label>
                <div class="col-md-3">
                    <asp:TextBox ID="txtMaxStudents" runat="server" CssClass="form-control-static" Font-Names="Arial" Font-Size="Small" 
                        ReadOnly="True" BackColor="White" BorderStyle="None"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                    <asp:LinkButton ID="btnReturn" runat="server" OnClick="BtnReturn_Click"><<< Go Back</asp:LinkButton>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>