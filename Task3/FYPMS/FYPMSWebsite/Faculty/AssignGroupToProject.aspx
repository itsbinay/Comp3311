<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AssignGroupToProject.aspx.cs" Inherits="FYPMSWebsite.Faculty.AssignGroupToProject" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <h4><span style="text-decoration: underline; color: #800000" class="h4"><strong>Assign Groups To Projects</strong></span></h4>
        <asp:Label ID="lblResultMessage" runat="server" Font-Bold="True" CssClass="label" Font-Names="Arial" Font-Size="Small"></asp:Label>
        <asp:Panel ID="pnlSelectProject" runat="server">
            <hr />
            <div class="form-group">
                <asp:Label runat="server" Text="Project:" CssClass="control-label col-md-1" AssociatedControlID="ddlProjects" Font-Names="Arial" Font-Size="Small"></asp:Label>
                <div class="col-md-11">
                    <asp:DropDownList ID="ddlProjects" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DdlProjects_SelectedIndexChanged" Font-Names="Arial" Font-Size="Small"></asp:DropDownList>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlGroupsCurrentlyAssigned" runat="server" Visible="False">
            <hr />
            <h5><span style="text-decoration: underline; color: #800000" class="h5"><strong>Groups Assigned To This Project:</strong></span></h5>
            <asp:Label ID="lblAssignedResultMessage" runat="server" Font-Bold="True" CssClass="label" Font-Names="Arial" Font-Size="Small"></asp:Label>
            <div class="form-group">
                <div class="col-md-12">
                    <asp:GridView ID="gvCurrentlyAssigned" runat="server" CssClass="table-condensed" BorderStyle="Solid" CellPadding="0" 
                        OnRowDataBound="GvCurrentlyAssigned_RowDataBound" Font-Names="Arial" Font-Size="Small">
                        <EditRowStyle Font-Names="Arial" Font-Size="Small" />
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" Wrap="False" />
                        <RowStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlGroupsAvailableForAssignment" runat="server" Visible="False">
            <hr />
            <h5><span style="text-decoration: underline; color: #800000" class="h5"><strong>Groups Available To Be Assigned To This Project:</strong></span></h5>
            <asp:Label ID="lblAvailableResultMessage" runat="server" Font-Bold="True" CssClass="label" Font-Names="Arial" Font-Size="Small"></asp:Label>
            <div class="form-group">
                <div class="col-md-12">
                    <asp:GridView ID="gvAvailableForAssignment" runat="server" CssClass="table-condensed" BorderStyle="Solid" CellPadding="0"
                        OnRowDataBound="GvAvailableForAssignment_RowDataBound" Font-Names="Arial" Font-Size="Small">
                        <Columns>
                            <asp:TemplateField HeaderText="SELECT">
                                <EditItemTemplate>
                                    <asp:CheckBox ID="chkSelected" runat="server" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelected" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle Font-Names="Arial" Font-Size="Small" />
                        <HeaderStyle Font-Names="Arial" Font-Size="Small" />
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlBtnAssignGroups" runat="server" Visible="False">
            <br />
            <div class="form-group">
                <div class="col-md-offset-1" "col-md-11">
                    <asp:Button ID="btnAssignGroups" runat="server" Text="Assign Selected Groups" CssClass="btn-sm" OnClick="BtnAssignGroups_Click" Font-Size="Small" Font-Names="Arial" />
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>