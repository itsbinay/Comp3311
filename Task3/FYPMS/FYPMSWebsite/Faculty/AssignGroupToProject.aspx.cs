using System;
using System.Data;
using FYPMSWebsite.App_Code;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web;

namespace FYPMSWebsite.Faculty
{
    public partial class AssignGroupToProject : System.Web.UI.Page
    {
        //******************************************************************
        // Uses TODO 09, 10, 11, 12, 13, 14, 15, 16; in SharedAccess.cs 33 *
        //******************************************************************
        private FYPMSDB myFYPMSDB = new FYPMSDB();
        private Helpers myHelpers = new Helpers();
        private SharedAccess mySharedAccess = new SharedAccess();
        private decimal maxGroups = 4;
        private readonly string loggedinUsername = HttpContext.Current.User.Identity.Name;


        /***** Private Methods *****/

        private string CreateGroupCode(string fypId)
        {
            string groupCode = "";

            //***************
            // Uses TODO 09 *
            //***************
            DataTable dtFacultyCodes = myFYPMSDB.GetProjectFacultyCodes(fypId);

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "FACULTYCODE" };

            if (myHelpers.IsQueryResultValid("09", dtFacultyCodes, attributeList, lblResultMessage))
            {
                if (dtFacultyCodes.Rows.Count != 0)
                {
                    foreach (DataRow row in dtFacultyCodes.Rows)
                    {
                        groupCode = groupCode + row["FACULTYCODE"].ToString().Trim();
                    }
                    // Determine the sequence number for the group code.
                    //***************
                    // Uses TODO 10 *
                    //***************
                    decimal sequenceCode = myFYPMSDB.GetFacultyCodeSequenceNumber(groupCode);

                    if (sequenceCode != -1)
                    {
                        groupCode = groupCode + (sequenceCode + 1).ToString();
                    }
                    else
                    {
                        myHelpers.ShowMessage(lblResultMessage, "*** SQL error in TODO 10: " + Global.sqlError + ".");
                        groupCode = "";
                    }
                }
                else // Nothing to display
                {
                    myHelpers.ShowMessage(lblResultMessage, "There are no faculty that supervise project" + fypId + ".");
                    lblResultMessage.Visible = true;
                }
            }
            return groupCode;
        }

        private DataTable CreateGroupsAvailableForAssignmentGroupList(DataTable dtGroups)
        {
            // Create a new DataTable containing only one row for each group
            DataTable dtGroupList = new DataTable();
            dtGroupList.Columns.Add("GROUPID", typeof(string));
            dtGroupList.Columns.Add("PRIORITY", typeof(string));
            dtGroupList.Columns.Add("MEMBERS", typeof(string));
            string previousGroupId = "";
            string groupId = "";
            string priority = "";
            string members = "";
            foreach (DataRow row in dtGroups.Rows)
            {
                groupId = row["GROUPID"].ToString();
                if (previousGroupId != groupId && previousGroupId != "")
                {
                    members = members.Remove(members.Length - 2);
                    dtGroupList.Rows.Add(new object[] { previousGroupId, priority, members });
                    members = "";
                }
                priority = row["FYPPRIORITY"].ToString();
                members = members + row["STUDENTNAME"].ToString() + ", ";
                previousGroupId = groupId;
            }
            members = members.Remove(members.Length - 2);
            dtGroupList.Rows.Add(new object[] { groupId, priority, members });
            return dtGroupList;
        }

        private DataTable CreateGroupsCurrentlyAssignedGroupList(DataTable dtGroups)
        {
            // Create a new DataTable containing only one row for each group.
            DataTable dtGroupList = new DataTable();
            dtGroupList.Columns.Add("GROUPID", typeof(string));
            dtGroupList.Columns.Add("CODE", typeof(string));
            dtGroupList.Columns.Add("MEMBERS", typeof(string));
            string previousGroupId = "";
            string groupId = "";
            string groupCode = "";
            string members = "";
            foreach (DataRow row in dtGroups.Rows)
            {
                groupId = row["GROUPID"].ToString();
                if (previousGroupId != groupId && previousGroupId != "")
                {
                    members = members.Remove(members.Length - 2);
                    dtGroupList.Rows.Add(new object[] { previousGroupId, groupCode, members });
                    members = "";
                }
                groupCode = row["GROUPCODE"].ToString();
                members = members + row["STUDENTNAME"].ToString() + ", ";
                previousGroupId = groupId;
            }
            members = members.Remove(members.Length - 2);
            dtGroupList.Rows.Add(new object[] { groupId, groupCode, members });
            return dtGroupList;
        }

        private bool IsMaxGroups(decimal numGroups)
        {
            bool result = true;
            if (numGroups != maxGroups)
            {
                result = false;
            }
            else // Already supervising the maximum number of groups.
            {
                myHelpers.ShowMessage(lblResultMessage, "You are supervising or cosupervising the maximum of " + maxGroups + " groups.");
                pnlGroupsAvailableForAssignment.Visible = false;
            }
            return result;
        }

        private void PopulateGroupsAvailableForAssignment(string fypId)
        {
            lblAvailableResultMessage.Visible = false;
            gvAvailableForAssignment.Visible = false;
            pnlGroupsAvailableForAssignment.Visible = false;
            pnlBtnAssignGroups.Visible = false;

            // Check if maximum allowed project groups has been reached.
            if (!IsMaxGroups(Convert.ToDecimal(ViewState["numGroups"])))
            {
                // Check if the project is open.
                if (ProjectIsOpen(fypId))
                {
                    //***************
                    // Uses TODO 11 *
                    //***************
                    DataTable dtAvailableForAssignment = myFYPMSDB.GetGroupsAvailableToAssign(fypId);

                    // Attributes expected to be returned by the query result.
                    var attributeList = new List<string> { "GROUPID", "FYPPRIORITY", "STUDENTNAME", "USERNAME" };

                    // Display the query result if it is valid.
                    if (myHelpers.IsQueryResultValid("11", dtAvailableForAssignment, attributeList, lblResultMessage))
                    {
                        if (dtAvailableForAssignment.Rows.Count != 0)
                        {
                            ViewState["dtAvailableForAssignment"] = dtAvailableForAssignment;
                            dtAvailableForAssignment = CreateGroupsAvailableForAssignmentGroupList(dtAvailableForAssignment);
                            gvAvailableForAssignment.DataSource = dtAvailableForAssignment;
                            gvAvailableForAssignment.DataBind();
                            gvAvailableForAssignment.Visible = true;
                            pnlBtnAssignGroups.Visible = true;
                        }
                        else
                        {
                            myHelpers.ShowMessage(lblAvailableResultMessage, "There are no groups available to asssign to this project.");
                            lblAvailableResultMessage.Visible = true;
                        }
                        pnlGroupsAvailableForAssignment.Visible = true;
                    }

                }
                else
                {
                    myHelpers.ShowMessage(lblAvailableResultMessage, "This project is closed.");
                    lblAvailableResultMessage.Visible = true;
                    pnlGroupsAvailableForAssignment.Visible = true;
                }
            }
        }

        private bool PopulateGroupsCurrentlyAssigned(string fypId)
        {
            bool result = false;
            lblAssignedResultMessage.Visible = false;
            gvCurrentlyAssigned.Visible = false;
            pnlGroupsCurrentlyAssigned.Visible = false;

            //***************
            // Uses TODO 12 *
            //***************
            DataTable dtCurrentlyAssigned = myFYPMSDB.GetGroupsCurrentlyAssigned(fypId);

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "GROUPID", "GROUPCODE", "STUDENTNAME" };

            // Display the query result if it is not null.
            if (myHelpers.IsQueryResultValid("12", dtCurrentlyAssigned, attributeList, lblResultMessage))
            {
                if (dtCurrentlyAssigned.Rows.Count != 0)
                {
                    dtCurrentlyAssigned = CreateGroupsCurrentlyAssignedGroupList(dtCurrentlyAssigned);
                    gvCurrentlyAssigned.DataSource = dtCurrentlyAssigned;
                    gvCurrentlyAssigned.DataBind();
                    gvCurrentlyAssigned.Visible = true;
                }
                else // Nothing to display
                {
                    myHelpers.ShowMessage(lblAssignedResultMessage, "There are no groups assigned to this project.");
                    lblAssignedResultMessage.Visible = true;
                }
                result = true;
                pnlGroupsCurrentlyAssigned.Visible = true;
            }
            return result;
        }

        private void PopulateProjectsDropDownList()
        {
            //***************
            // Uses TODO 13 *
            //***************
            DataTable dtProjects = myFYPMSDB.GetFacultyProjects(loggedinUsername);

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "FYPID", "TITLE" };

            // Display the query result if it is validl.
            if (myHelpers.IsQueryResultValid("13", dtProjects, attributeList, lblResultMessage))
            {
                if (dtProjects.Rows.Count != 0)
                {
                    ddlProjects.DataSource = dtProjects;
                    ddlProjects.DataValueField = "fypId";
                    ddlProjects.DataTextField = "title";
                    ddlProjects.DataBind();
                    ddlProjects.Items.Insert(0, "-- Select Project --");
                }
                else // Nothing to display.
                {
                    myHelpers.ShowMessage(lblResultMessage, "You have not posted any projects.");
                    pnlSelectProject.Visible = false;
                }
            }
        }

        private bool ProjectIsOpen(string fypId)
        {
            bool projectIsOpen = false;

            //***************
            // Uses TODO 14 *
            //***************
            DataTable dtStatus = myFYPMSDB.GetProjectAvailability(fypId);

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "ISAVAILABLE" };

            if (myHelpers.IsQueryResultValid("14", dtStatus, attributeList, lblResultMessage))
            {
                if (dtStatus.Rows.Count != 0)
                {
                    if (dtStatus.Rows[0]["ISAVAILABLE"].ToString() == "Y")
                    {
                        projectIsOpen = true;
                    }
                }
                else // Nothing to display
                {
                    myHelpers.ShowMessage(lblResultMessage, "*** There is no project with the specified id.");
                }
            }
            return projectIsOpen;
        }


        /***** Protected Methods *****/

        protected void BtnAssignGroups_Click(object sender, EventArgs e)
        {
            DataTable dtAvailableForAssignment = ViewState["dtAvailableForAssignment"] as DataTable;
            string fypId = ddlProjects.SelectedValue;
            string groupId = "";

            // Determine if any groups were selected for assignment to projects.
            foreach (GridViewRow row in gvAvailableForAssignment.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkSelected") as CheckBox);
                    if (chkRow != null && chkRow.Checked)
                    {
                        // Get the group id.
                        groupId = row.Cells[1].Text;
                        // Construct the group code.
                        string groupCode = CreateGroupCode(fypId);
                        if (groupCode == "") { return; }

                        // Get the students in the group.
                        DataTable dtGroupMembers = dtAvailableForAssignment.Select("GROUPID=" + groupId).CopyToDataTable<DataRow>();

                        //***************
                        // Uses TODO 15 *
                        //***************
                        if (myFYPMSDB.AssignGroupToProject(groupCode, fypId, groupId))
                        {
                            // Uses TODO 33 in SharedAccess.cs.
                            if (!mySharedAccess.CreateRequirementRecord(loggedinUsername, dtGroupMembers, lblResultMessage))
                            {
                                // Uses TODO 45. Undo the assignment of the group to the project if creating the Requirement record failed.
                                if (!myFYPMSDB.RemoveGroupFromProject(groupId))
                                {
                                    myHelpers.ShowMessage(lblResultMessage, "*** Error removing group from project. Please contact 3311 Rep.");
                                }
                                groupId = "SQL_ERROR";
                            }
                        }
                        else
                        {
                            myHelpers.ShowMessage(lblResultMessage, "*** SQL error in TODO 15: " + Global.sqlError + ".");
                            groupId = "SQL_ERROR";
                        }
                    }
                }
            }

            // Show result message and refresh the web form if no error occurred.
            if (groupId != "SQL_ERROR")
            {
                if (groupId != "")
                {
                    PopulateGroupsCurrentlyAssigned(fypId);
                    PopulateGroupsAvailableForAssignment(fypId);
                }
                else
                {
                    myHelpers.ShowMessage(lblResultMessage, "No group was selected for assignment.");
                }
            }
        }

        protected void DdlProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProjects.SelectedIndex != 0)
            {
                if (PopulateGroupsCurrentlyAssigned(ddlProjects.SelectedValue))
                {
                    PopulateGroupsAvailableForAssignment(ddlProjects.SelectedValue);
                }
            }
            else
            {
                pnlGroupsCurrentlyAssigned.Visible = false;
                pnlGroupsAvailableForAssignment.Visible = false;

            }
        }

        protected void GvAvailableForAssignment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Controls.Count == 4)
            {
                e.Row.Cells[1].Visible = false;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }

        protected void GvCurrentlyAssigned_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Controls.Count == 3)
            {
                e.Row.Cells[0].Visible = false;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // Get the number of groups that the faculty is currently supervising.
                //***************
                // Uses TODO 16 *
                //***************
                decimal numGroups = myFYPMSDB.GetNumberOfGroupsSupervised(loggedinUsername);
                ViewState["numGroups"] = numGroups;

                if (numGroups != -1)
                {
                    if (!IsMaxGroups(numGroups))
                    {
                        if (numGroups != 1)
                        {
                            myHelpers.ShowMessage(lblResultMessage, "You are currently supervising " + numGroups + " groups.");
                        }
                        else
                        {
                            myHelpers.ShowMessage(lblResultMessage, "You are currently supervising " + numGroups + " group.");
                        }
                    }
                    PopulateProjectsDropDownList();
                }
                else
                {
                    myHelpers.ShowMessage(lblResultMessage, "*** SQL error in TODO 16: " + Global.sqlError + ".");
                }
            }
        }
    }
}