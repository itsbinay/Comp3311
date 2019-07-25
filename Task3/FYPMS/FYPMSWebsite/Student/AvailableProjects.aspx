<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AvailableProjects.aspx.cs" Inherits="FYPMSWebsite.Student.AvailableProjects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <h4><span style="text-decoration: underline; color: #800000" class="h4"><strong>Projects For Which Your Group Can Indicate An Interest</strong></span></h4>
        <asp:Label ID="lblResultMessage" runat="server" Font-Bold="True" CssClass="label" Font-Names="Arial" Font-Size="Small"></asp:Label>
        <asp:Panel ID="pnlProjectsAvailableForSelection" runat="server" Visible="False">
            <hr />
            <div class="form-group">
                <div class="col-md-12">
                    <asp:GridView ID="gvAvailableForSelection" runat="server" CssClass="table-condensed" BorderStyle="Solid" CellPadding="0"
                        Font-Names="Arial" Font-Size="Small" OnRowDataBound="GvAvailableForSelection_RowDataBound">
                        <Columns>
                            <asp:HyperLinkField Text="Details" DataNavigateUrlFields="FYPID" NavigateUrl="../DisplayProjectDetails.aspx"
                                DataNavigateUrlFormatString="../DisplayProjectDetails.aspx?fypId={0}&returnUrl=~/Student/AvailableProjects.aspx" />
                            <asp:TemplateField HeaderText="PRIORITY">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlPriority" runat="server">
                                        <asp:ListItem>Select</asp:ListItem>
                                        <asp:ListItem>1</asp:ListItem>
                                        <asp:ListItem>2</asp:ListItem>
                                        <asp:ListItem>3</asp:ListItem>
                                        <asp:ListItem>4</asp:ListItem>
                                        <asp:ListItem>5</asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlBtnSelectProjects" runat="server" Visible="False">
            <br />
            <div class="form-group">
                <div class="col-md-3">
                    <asp:HyperLink ID="hlSelectedProjects" runat="server" NavigateUrl="~/Student/SelectedProjects.aspx">Show Selected Projects</asp:HyperLink>
                </div>
                <div>
                    <asp:Button ID="btnUpdateProjectInterest" runat="server" Text="Update Project Interest" CssClass="btn-sm"
                        Font-Size="Small" Font-Names="Arial" OnClick="BtnUpdateProjectInterest_Click" />
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