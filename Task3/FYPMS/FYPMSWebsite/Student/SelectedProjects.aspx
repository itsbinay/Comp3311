<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SelectedProjects.aspx.cs" Inherits="FYPMSWebsite.Student.SelectedProjects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <h4><span style="text-decoration: underline; color: #800000" class="h4"><strong>Projects For Which Your Group Has Indicated An Interest</strong></span></h4>
        <asp:Label ID="lblResultMessage" runat="server" Font-Bold="True" CssClass="label" Font-Names="Arial" Font-Size="Small"></asp:Label>
        <asp:Panel ID="pnlSelectedProjects" runat="server" Visible="False">
            <hr />
            <div class="form-group">
                <div class="col-md-12">
                    <asp:GridView ID="gvSelectedProjects" runat="server" CssClass="table-condensed" BorderStyle="Solid" CellPadding="0"
                        Font-Names="Arial" Font-Size="Small" OnRowDataBound="GvSelectedProjects_RowDataBound">
                        <Columns>
                            <asp:HyperLinkField Text="Details" DataNavigateUrlFields="FYPID" NavigateUrl="../DisplayProjectDetails.aspx"
                                DataNavigateUrlFormatString="../DisplayProjectDetails.aspx?fypId={0}&returnUrl=~/Student/SelectedProjects.aspx" />
                        </Columns>
                        <EditRowStyle Font-Names="Arial" Font-Size="Small" />
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" />
                        <RowStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:GridView>
                    <br />
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlSelectProjects" runat="server" Visible="False">
            <div class="form-group">
                <div class="col-md-12">
                    <asp:HyperLink ID="hlSelectProjects" runat="server" NavigateUrl="~/Student/AvailableProjects.aspx">Show Available Projects</asp:HyperLink>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlFormProjectGroup" runat="server" Visible="False">
            <div class="form-group">
                <div class="col-md-12">
                    <asp:HyperLink ID="hlCreateProjectGroup" runat="server" NavigateUrl="~/Student/ManageProjectGroup.aspx">Create Project Group</asp:HyperLink>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>