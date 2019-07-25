using FYPMSWebsite.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace FYPMSWebsite.Student
{
    public partial class AvailableProjects : System.Web.UI.Page
    {
        //*************************************************
        // Uses TODO 24, 25; in SharedAccess.cs 34,36, 38 *
        //*************************************************
        private FYPMSDB myFYPMSDB = new FYPMSDB();
        private Helpers myHelpers = new Helpers();
        private SharedAccess mySharedAccess = new SharedAccess();
        readonly string loggedinUsername = HttpContext.Current.User.Identity.Name;


        /***** Private Methods *****/

        private void GetProjectsAvailableToSelect(string groupId)
        {
            lblResultMessage.Visible = false;

            // If the group id is empty, the student is not yet a member of any group.
            if (groupId != "")
            {
                // Uses TODO 36 in SharedAccess.cs
                DataTable dtProjects = mySharedAccess.GetGroupAvailableProjectDigests(groupId, lblResultMessage);

                // Display the query result if it is valid.
                if (dtProjects != null)
                {
                    if (dtProjects.Rows.Count != 0)
                    {
                        gvAvailableForSelection.DataSource = dtProjects;
                        gvAvailableForSelection.DataBind();
                        ViewState["groupId"] = groupId;
                        pnlProjectsAvailableForSelection.Visible = true;
                        pnlBtnSelectProjects.Visible = true;
                    }
                    else // Nothing to display
                    {
                        myHelpers.ShowMessage(lblResultMessage, "All projects are closed.");
                    }
                }
            }
            else
            {
                myHelpers.ShowMessage(lblResultMessage, "You have not yet joined a group.");
                pnlFormProjectGroup.Visible = true;
            }
        }


        /***** Protected Methods *****/

        protected void BtnUpdateProjectInterest_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                string groupId = ViewState["groupId"].ToString();
                string fypId;
                string priority;

                // For each project for which a priority has been specified, get the priority and update the InterestedIn table.
                for (int i = 0; i < gvAvailableForSelection.Rows.Count; i++)
                {
                    DropDownList ddlPriority = (DropDownList)gvAvailableForSelection.Rows[i].FindControl("ddlPriority");
                    fypId = gvAvailableForSelection.Rows[i].Cells[2].Text;
                    if (ddlPriority.SelectedIndex != 0)
                    {
                        priority = ddlPriority.SelectedItem.Value;

                        //***************
                        // Uses TODO 24 *
                        //***************
                        if (!myFYPMSDB.CreateInterestedIn(fypId, groupId, priority))
                        {
                            // An SQL error occurred. Exit the insert.
                            myHelpers.ShowMessage(lblResultMessage, "*** SQL error in TODO 24: " + Global.sqlError + ".");
                            return;
                        }
                    }
                }
                // Refresh the web form.
                Response.Redirect("~/Student/SelectedProjects.aspx");
            }
        }

        protected void GvAvailableForSelection_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Controls.Count == 8)
            {
                e.Row.Cells[2].Visible = false;
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    myHelpers.RenameGridViewColumn(e, "FYPCATEGORY", "CATEGORY");
                    myHelpers.RenameGridViewColumn(e, "FYPTYPE", "TYPE");
                    myHelpers.RenameGridViewColumn(e, "MINSTUDENTS", "MIN");
                    myHelpers.RenameGridViewColumn(e, "MAXSTUDENTS", "MAX");
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Uses TODO 38 in SharedAccess.cs
            string groupId = mySharedAccess.GetStudentGroupId(loggedinUsername, lblResultMessage);

            if (groupId != "SQL_ERROR")
            {
                if (!IsPostBack)
                {
                    // If the group id is empty, the student is not yet a member of any group.
                    if (groupId != "")
                    {
                        // Uses TODO 34 in SharedAccess.cs.
                        string isAssigned = mySharedAccess.IsGroupAssigned(groupId, lblResultMessage);
                        if (isAssigned != "SQL_ERROR")
                        {
                            if (isAssigned == "false")
                            {
                                GetProjectsAvailableToSelect(groupId);
                            }
                            else // Group is already assigned to a project
                            {
                                //***************
                                // Uses TODO 25 *
                                //***************
                                DataTable dtProjectAssigned = myFYPMSDB.GetProjectAssignedToGroup(groupId);

                                // Attributes expected to be returned by the query result.
                                var attributeList = new List<string> { "TITLE" };

                                // Display the query result if it is valid.
                                if (myHelpers.IsQueryResultValid("25", dtProjectAssigned, attributeList, lblResultMessage))
                                {
                                    if (dtProjectAssigned.Rows.Count != 0)
                                    {
                                        myHelpers.ShowMessage(lblResultMessage, "This group is already assigned to project " + dtProjectAssigned.Rows[0]["TITLE"].ToString() + ".");
                                    }
                                    else // Nothing returned; should not happen!
                                    {
                                        myHelpers.ShowMessage(lblResultMessage, "There is no group with group id " + groupId + ". Please check your database.");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        myHelpers.ShowMessage(lblResultMessage, "You have not yet joined a group.");
                        pnlFormProjectGroup.Visible = true;
                    }
                }
            }
        }
    }
}