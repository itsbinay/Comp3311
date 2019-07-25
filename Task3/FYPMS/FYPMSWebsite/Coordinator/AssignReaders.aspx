<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AssignReaders.aspx.cs" Inherits="FYPMSWebsite.Coordinator.AssignReaders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <h4><span style="text-decoration: underline; color: #800000" class="h4"><strong>Assign Readers to Projects</strong></span></h4>
        <asp:Label ID="lblResultMessage" runat="server" Font-Bold="True" CssClass="label" Font-Names="Arial" Font-Size="Small"></asp:Label>
        <asp:Panel ID="pnlDisplayProjectsWithoutReaders" runat="server" Visible="False">
            <hr />
            <div class="form-group" role="row">
                <h5><span style="text-decoration: underline; color: #800000" class="h5"><strong>Project Without Readers</strong></span></h5>
                <div class="col-md-12">
                    <asp:GridView ID="gvProjectsWithoutReaders" runat="server" CssClass="table-condensed" BorderStyle="Solid" CellPadding="0"
                        AutoGenerateSelectButton="True" Font-Names="Arial" Font-Size="Small"
                        OnSelectedIndexChanged="GvProjectsWithoutReaders_SelectedIndexChanged" OnRowDataBound="GvProjectsWithoutReaders_RowDataBound">
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlAssignReader" runat="server" Visible="False">
            <hr />
            <div class="form-group" role="row">
                <h5><span style="text-decoration: underline; color: #800000" class="h5"><strong>Readers Available For Assignment</strong></span></h5>
                <!-- Title textbox -->
                <asp:Label runat="server" Text="Selected project:" CssClass="control-label col-md-2" AssociatedControlID="txtTitle" Font-Names="Arial"
                    Font-Size="Small"></asp:Label>
                <div class="col-md-10">
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control-static" Font-Names="Arial" Font-Size="Small"
                        ReadOnly="True" Wrap="False" BorderColor="White" BorderStyle="None" BorderWidth="0px" Width="100%"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-12">
                    <asp:GridView ID="gvAvailableReaders" runat="server" AutoGenerateSelectButton="True" BorderStyle="Solid"
                        CssClass="table-condensed" Font-Names="Arial" Font-Size="Small"
                        OnSelectedIndexChanged="GvAvailableReaders_SelectedIndexChanged" OnRowDataBound="GvAvailableReaders_RowDataBound">
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>
        <br />
        <div class="form-group">
            <div class="col-md-3">
                <asp:HyperLink ID="hlDisplayReaders" runat="server" NavigateUrl="~/Coordinator/DisplayReaders.aspx">Projects With Readers</asp:HyperLink>
            </div>
        </div>
    </div>
</asp:Content>