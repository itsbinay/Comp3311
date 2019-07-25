using FYPMSWebsite.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace FYPMSWebsite.Student
{
    public partial class ViewGrades : System.Web.UI.Page
    {
        //*************************************
        // Uses TODO 31, 32; in Helpers.cs 39 *
        //*************************************
        private FYPMSDB myFYPMSDB = new FYPMSDB();
        private Helpers myHelpers = new Helpers();
        private readonly string loggedinUsername = HttpContext.Current.User.Identity.Name;


        /***** Private Methods *****/

        private bool IsProjectAssigned(string username)
        {
            bool result = false;

            lblResultMessage.Visible = false;
            //***************
            // Uses TODO 31 *
            //***************
            DataTable dtProjectInfo = myFYPMSDB.GetAssignedProjectInformatione(username);

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "FYPID", "TITLE" };

            // Return the query result if it is valid.
            if (myHelpers.IsQueryResultValid("31", dtProjectInfo, attributeList, lblResultMessage))
            {
                if (dtProjectInfo.Rows.Count != 0)
                {
                    // Uses TODO 39 in Helpers.cs.
                    DataTable dtSupervisors = myHelpers.GetProjectSupervisors(dtProjectInfo.Rows[0]["FYPID"].ToString(), lblResultMessage);

                    if (dtSupervisors != null)
                    {
                        if (dtSupervisors.Rows.Count != 0)
                        {
                            txtTitle.Text = dtProjectInfo.Rows[0]["TITLE"].ToString();
                            txtSupervisor.Text = myHelpers.SupervisorsToString(dtSupervisors);
                            result = true;
                        }
                    }
                }
                else // The student is not assigned to any project.
                {
                    myHelpers.ShowMessage(lblResultMessage, "You are not assigned to a project.");
                }
            }
            return result;
        }

        private void PopulateStudentGrades(string username)
        {
            //***************
            // Uses TODO 32 *
            //***************
            DataTable dtGrades = myFYPMSDB.GetStudentGrades(username);

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "FACULTYNAME", "PROPOSALGRADE", "PROGRESSGRADE", "FINALGRADE", "PRESENTATIONGRADE" };

            // Display the query result if it is valid.
            if (myHelpers.IsQueryResultValid("32", dtGrades, attributeList, lblResultMessage))
            {
                if (dtGrades.Rows.Count != 0)
                {
                    gvGrades.DataSource = dtGrades;
                    gvGrades.DataBind();
                    pnlGrades.Visible = true;
                }
                else // Nothing to display.
                {
                    myHelpers.ShowMessage(lblResultMessage, "There are no grades for you to view as you have not yet been assigned to a project.");
                    pnlGrades.Visible = false;
                }
            }
        }


        /***** Protected Methods *****/

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsProjectAssigned(loggedinUsername))
            {
                PopulateStudentGrades(loggedinUsername);
            }
        }

        protected void GvGrades_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Controls.Count == 5)
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    myHelpers.RenameGridViewColumn(e, "FACULTYNAME", "FACULTY");
                    myHelpers.RenameGridViewColumn(e, "PROPOSALGRADE", "PROPOSAL");
                    myHelpers.RenameGridViewColumn(e, "PROGRESSGRADE", "PROGRESS");
                    myHelpers.RenameGridViewColumn(e, "FINALGRADE", "FINAL");
                    myHelpers.RenameGridViewColumn(e, "PRESENTATIONGRADE", "PRESENTATION");
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                }
            }

        }
    }
}