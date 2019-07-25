<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DisplayProjects.aspx.cs" Inherits="FYPMSWebsite.Faculty.DisplayProjects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <h4><span style="text-decoration: underline; color: #800000" class="h4"><strong>My Final Year Projects</strong></span></h4>
        <asp:Label ID="lblResultMessage" runat="server" Font-Bold="True" CssClass="label" Font-Names="Arial" Font-Size="Small"></asp:Label>
        <asp:Panel ID="pnlProjectInfo" runat="server">
            <hr />
            <div class="form-group">
                <div class="col-md-12">
                    <asp:GridView ID="gvProjects" runat="server" CssClass="table-condensed" OnRowDataBound="GvProjects_RowDataBound" 
                        Font-Names="Arial" Font-Size="Small">
                        <Columns>
                            <asp:HyperLinkField DataNavigateUrlFields="FYPID, TITLE" DataNavigateUrlFormatString="EditProject.aspx?fypId={0}&title={1}" 
                                NavigateUrl="~/Faculty/EditProject.aspx" Text="Edit" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <br />
        </asp:Panel>
    </div>
</asp:Content>