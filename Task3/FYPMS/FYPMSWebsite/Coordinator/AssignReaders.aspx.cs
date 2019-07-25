using FYPMSWebsite.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace FYPMSWebsite.Coordinator
{
    public partial class AssignReaders : System.Web.UI.Page
    {
        //******************************************************
        // Uses TODO 01, 02, 03; in SharedAccess.cs 33, 35, 39 *
        //******************************************************
        private FYPMSDB myFYPMSDB = new FYPMSDB();
        private Helpers myHelpers = new Helpers();
        private SharedAccess mySharedAccess = new SharedAccess();


        /***** Private Methods *****/

        private void PopulateAvailableReaders(string fypId)
        {
            // Uses TODO 35 in SharedAccess.cs.
            DataTable dtAvailableReaders = mySharedAccess.GetFaculty(lblResultMessage);

            // Display the query result if it is valid.
            if (dtAvailableReaders != null)
            {
                // Uses TODO 39 in SharedAccess.cs.
                DataTable dtSupervisors = mySharedAccess.GetProjectSupervisors(fypId, lblResultMessage);
                if (dtSupervisors != null)
                {
                    if (dtSupervisors.Rows.Count != 0)
                    {
                        foreach (DataRow row in dtSupervisors.Rows)
                        {
                            // Exclude the faculty who are the supervisors of the project.
                            dtAvailableReaders = mySharedAccess.RemoveSupervisor(dtAvailableReaders, row["USERNAME"].ToString());
                        }
                    }
                    gvAvailableReaders.DataSource = dtAvailableReaders;
                    gvAvailableReaders.DataBind();
                    gvProjectsWithoutReaders.DataSource = ViewState["dtProjectsWithoutReaders"] as DataTable;
                    gvProjectsWithoutReaders.DataBind();
                    pnlAssignReader.Visible = true;
                }
            }
        }

        private bool PopulateProjectsWithoutReaders()
        {
            bool result = false;
            //***************
            // Uses TODO 01 *
            //***************
            DataTable dtProjectsWithoutReaders = myFYPMSDB.GetProjectsWithoutReaders();

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "GROUPID", "GROUPCODE", "FYPASSIGNED", "TITLE", "FYPCATEGORY", "FYPTYPE" };

            // Display the query result if it is valid.
            if (myHelpers.IsQueryResultValid("01", dtProjectsWithoutReaders, attributeList, lblResultMessage))
            {
                if (dtProjectsWithoutReaders.Rows.Count != 0)
                {
                    ViewState["dtProjectsWithoutReaders"] = dtProjectsWithoutReaders;
                    gvProjectsWithoutReaders.DataSource = dtProjectsWithoutReaders;
                    gvProjectsWithoutReaders.DataBind();
                    pnlDisplayProjectsWithoutReaders.Visible = true;
                    result = true;
                }
                else // No projects without readers.
                {
                    pnlDisplayProjectsWithoutReaders.Visible = false;
                }
            }
            return result;
        }


        /***** Protected Methods *****/

        protected void GvAvailableReaders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Controls.Count == 3)
            {
                e.Row.Cells[1].Visible = false;
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    myHelpers.RenameGridViewColumn(e, "FACULTYNAME", "NAME");
                    TableHeaderCell headerCell = new TableHeaderCell();
                    headerCell.Text = "PROJECTS ASSIGNED";
                    e.Row.Cells.Add(headerCell);
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    TableCell dataCell = new TableCell();

                    //***************
                    // Uses TODO 02 *
                    //***************
                    decimal numberAssigned = myFYPMSDB.NumberProjectsAssignedToReader(e.Row.Cells[1].Text);

                    if (numberAssigned != -1)
                    {
                        dataCell.Text = numberAssigned.ToString();
                        dataCell.HorizontalAlign = HorizontalAlign.Center;
                    }
                    else
                    {
                        dataCell.Text = "";
                        myHelpers.ShowMessage(lblResultMessage, "*** SQL error in TODO 02: " + Global.sqlError + ".");
                    }
                    e.Row.Cells.Add(dataCell);
                }
            }
        }

        protected void GvAvailableReaders_SelectedIndexChanged(object sender, EventArgs e)
        {
            string facultyUsername = gvAvailableReaders.SelectedRow.Cells[1].Text;
            string facultyName = gvAvailableReaders.SelectedRow.Cells[2].Text;
            string groupId = gvProjectsWithoutReaders.SelectedRow.Cells[1].Text;
            string groupCode = gvProjectsWithoutReaders.SelectedRow.Cells[2].Text;

            //***************
            // Uses TODO 03 *
            //***************
            if (myFYPMSDB.AssignReaderToProject(groupId, facultyUsername))
            {
                DataTable dtGroupMembers = mySharedAccess.GetProjectGroupMembers(groupId, lblResultMessage);

                if (dtGroupMembers != null)
                {
                    // Uses TODO 33 in SharedAccess.cs
                    if (!mySharedAccess.CreateRequirementRecord(facultyUsername, dtGroupMembers, lblResultMessage))
                    {
                        // Undo the assignment of the reader to the project if creating the Requirement record failed.
                        if (!myFYPMSDB.RemoveReader(groupId))
                        {
                            myHelpers.ShowMessage(lblResultMessage, "*** Error removing reader from project. Please contct 3311 Rep.");
                        }
                        PopulateAvailableReaders(ViewState["fypId"].ToString());
                    }
                    else
                    {
                        myHelpers.ShowMessage(lblResultMessage, facultyName + " has been assigned as the reader for group " + groupCode + ".");
                        pnlAssignReader.Visible = false;
                    }
                    PopulateProjectsWithoutReaders();
                }
            }
            else // An SQL error occurred.
            {
                myHelpers.ShowMessage(lblResultMessage, "*** SQL error in TODO 03: " + Global.sqlError + ".");
            }
        }

        protected void GvProjectsWithoutReaders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Controls.Count == 7)
            {
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[3].Visible = false;
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    myHelpers.RenameGridViewColumn(e, "GROUPCODE", "GROUP CODE");
                    myHelpers.RenameGridViewColumn(e, "FYPCATEGORY", "CATEGORY");
                    myHelpers.RenameGridViewColumn(e, "FYPTYPE", "TYPE");
                    TableHeaderCell headerCell = new TableHeaderCell();
                    headerCell.Text = "SUPERVISOR(S)";
                    e.Row.Cells.Add(headerCell);
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                    TableCell dataCell = new TableCell();
                    dataCell.Text = "";
                    string fypId = e.Row.Cells[3].Text;
                    // Uses TODO 39 in SharedAccess.cs.
                    DataTable dtSupervisors = mySharedAccess.GetProjectSupervisors(fypId, lblResultMessage);
                    if (dtSupervisors != null)
                    {
                        dataCell.Text = mySharedAccess.SupervisorsToString(dtSupervisors);
                    }
                    e.Row.Cells.Add(dataCell);
                }
            }
        }

        protected void GvProjectsWithoutReaders_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblResultMessage.Visible = false;
            string fypId = gvProjectsWithoutReaders.SelectedRow.Cells[3].Text;
            txtTitle.Text = gvProjectsWithoutReaders.SelectedRow.Cells[3].Text;
            ViewState["fypId"] = fypId;
            PopulateAvailableReaders(fypId);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!PopulateProjectsWithoutReaders())
                {
                    myHelpers.ShowMessage(lblResultMessage, "There are no projects that require a reader.");
                }
            }
        }
    }
}