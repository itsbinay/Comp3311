<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DisplayAllProjects.aspx.cs" Inherits="FYPMSWebsite.DisplayAllProjects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <h4><span style="text-decoration: underline; color: #800000" class="h4"><strong>Project Information</strong></span></h4>
        <asp:Label ID="lblResultMessage" runat="server" Font-Bold="True" CssClass="label" Font-Names="Arial" Font-Size="Small"></asp:Label>
        <asp:Panel ID="pnlProjectInfo" runat="server">
            <hr />
            <div class="form-group">
                <div class="col-md-12">
                    <asp:GridView ID="gvProjects" runat="server" CssClass="table-condensed" OnRowDataBound="GvProjects_RowDataBound" Font-Names="Arial" Font-Size="Small" AllowPaging="True" OnPageIndexChanging="gvProjects_PageIndexChanging" PageSize="15">
                        <Columns>
                            <asp:HyperLinkField DataNavigateUrlFields="FYPID" NavigateUrl="DisplayProjectDetails.aspx" Text="Details"
                                DataNavigateUrlFormatString="DisplayProjectDetails?fypId={0}&returnUrl=DisplayAllProjects.aspx" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <br />
        </asp:Panel>
    </div>
</asp:Content>