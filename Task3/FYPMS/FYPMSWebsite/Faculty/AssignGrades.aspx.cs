using System;
using System.Data;
using FYPMSWebsite.App_Code;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web;

namespace FYPMSWebsite.Faculty
{
    public partial class AssignGrades : System.Web.UI.Page
    {
        //***************************
        // Uses TODO 05, 06, 07, 08 *
        //***************************
        private FYPMSDB myFYPMSDB = new FYPMSDB();
        private Helpers myHelpers = new Helpers();
        private readonly string loggedinUsername = HttpContext.Current.User.Identity.Name;


        /***** Private Methods *****/

        private string GetFacultyCode(string username)
        {
            //***************
            // Uses TODO 05 *
            //***************
            DataTable dtFacultyCode = myFYPMSDB.GetFacultyCode(loggedinUsername);

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "FACULTYCODE" };

            // Display the query result if it is valid.
            if (myHelpers.IsQueryResultValid("05", dtFacultyCode, attributeList, lblResultMessage))
            {
                if (dtFacultyCode.Rows.Count != 0)
                {
                    return dtFacultyCode.Rows[0]["FACULTYCODE"].ToString();
                }
                else // Nothing to display.
                {
                    myHelpers.ShowMessage(lblResultMessage, "There is no faculty with the username " + loggedinUsername + ".");
                }
            }
            pnlGroupMembers.Visible = false;
            return null;
        }

        private bool GradeIsValid(string gradeName, string grade)
        {
            if (lblGradeErrorMessage.Visible == false)
            {
                if (myHelpers.IsValidAndInRange(grade, 0, 100) || grade == "")
                {
                    return true;
                }
                else
                {
                    myHelpers.ShowMessage(lblGradeErrorMessage, "Please enter a valid " + gradeName + " grade.");
                    lblGradeErrorMessage.Visible = true;
                }
            }
            return false;
        }

        private void PopulateGroupsDropDownList()
        {
            string facultyCode = GetFacultyCode(loggedinUsername);
            if (facultyCode != null)
            {
                //***************
                // Uses TODO 06 *
                //***************
                DataTable dtGroups = myFYPMSDB.GetFacultyGroups(facultyCode);

                // Attributes expected to be returned by the query result.
                var attributeList = new List<string> { "GROUPID", "GROUPCODE", "FYPASSIGNED" };

                // Display the query result if it is valid.
                if (myHelpers.IsQueryResultValid("06", dtGroups, attributeList, lblResultMessage))
                {
                    if (dtGroups.Rows.Count != 0)
                    {
                        ddlGroups.DataSource = dtGroups;
                        ddlGroups.DataValueField = "GROUPID";
                        ddlGroups.DataTextField = "GROUPCODE";
                        ddlGroups.DataBind();
                        ddlGroups.Items.Insert(0, "-- Select Group --");
                        ViewState["dtGroups"] = dtGroups;
                    }
                    else // Nothing to display.
                    {
                        myHelpers.ShowMessage(lblResultMessage, "There are no project groups.");
                        pnlSelectGroup.Visible = false;
                    }
                }
            }
        }

        private void PopulateStudentRequirements(string groupId)
        {
            string fypId = (ViewState["dtGroups"] as DataTable).Rows[Convert.ToInt16(ddlGroups.SelectedIndex) - 1]["FYPASSIGNED"].ToString();

            //***************
            // Uses TODO 07 *
            //***************
            DataTable dtRequirements = myFYPMSDB.GetStudentRequirements(groupId, fypId);

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "USERNAME", "STUDENTNAME", "PROPOSALGRADE", "PROGRESSGRADE", "FINALGRADE", "PRESENTATIONGRADE" };

            // Display the query result if it is valid.
            if (myHelpers.IsQueryResultValid("07", dtRequirements, attributeList, lblResultMessage))
            {
                if (dtRequirements.Rows.Count != 0)
                {
                    gvStudents.DataSource = dtRequirements;
                    gvStudents.DataBind();
                    pnlGroupMembers.Visible = true;
                }
                else // No Requirement record found.
                {
                    myHelpers.ShowMessage(lblResultMessage, "There are no Requirement records. Please check your database.");
                    pnlGroupMembers.Visible = false;
                    /* Creates a Requirement record if none is found. Shoul dnot happen.
                    DataTable dtGroupMembers = myHelpers.GetProjectGroupMembers(groupId, lblResultMessage);
                    if (dtGroupMembers != null)
                    {
                        if(myHelpers.CreateRequirementRecord(loggedinUsername, dtGroupMembers, lblResultMessage))
                        {
                            PopulateStudentRequirements(groupId);
                        }
                    }
                    */
                }
            }
        }


        /***** Protected Methods *****/

        protected void DdlGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlGroups.SelectedIndex != 0)
            {
                lblResultMessage.Visible = false;
                PopulateStudentRequirements(ddlGroups.SelectedValue);
            }
            else
            {
                pnlGroupMembers.Visible = false;
            }
        }

        protected void GvStudents_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            gvStudents.EditIndex = -1;
            lblGradeErrorMessage.Visible = false;
            pnlGroupMembers.Visible = false;
            PopulateStudentRequirements(ddlGroups.SelectedValue);
        }

        protected void GvStudents_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.Controls.Count == 7)
            {
                e.Row.Cells[1].Visible = false;
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    myHelpers.RenameGridViewColumn(e, "STUDENTNAME", "NAME");
                    myHelpers.RenameGridViewColumn(e, "PROPOSALGRADE", "PROPOSAL");
                    myHelpers.RenameGridViewColumn(e, "PROGRESSGRADE", "PROGRESS");
                    myHelpers.RenameGridViewColumn(e, "FINALGRADE", "FINAL");
                    myHelpers.RenameGridViewColumn(e, "PRESENTATIONGRADE", "PRESENTATION");
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }

        protected void GvStudents_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            gvStudents.EditIndex = e.NewEditIndex;
            PopulateStudentRequirements(ddlGroups.SelectedValue);
        }

        protected void GvStudents_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            if (Page.IsValid)
            {
                lblGradeErrorMessage.Visible = false;
                string fypId = (ViewState["dtGroups"] as DataTable).Rows[Convert.ToInt16(ddlGroups.SelectedIndex) - 1]["FYPASSIGNED"].ToString();

                GridViewRow row = gvStudents.Rows[e.RowIndex];
                string studentUsername = (row.Cells[1].Controls[0] as TextBox).Text;
                string proposalGrade = (row.Cells[3].Controls[0] as TextBox).Text;
                string progressGrade = (row.Cells[4].Controls[0] as TextBox).Text;
                string finalGrade = (row.Cells[5].Controls[0] as TextBox).Text;
                string presentationGrade = (row.Cells[6].Controls[0] as TextBox).Text;

                // Check if all grades are valid.
                if (GradeIsValid("proposal", proposalGrade) && GradeIsValid("progress", progressGrade)
                    && GradeIsValid("final", finalGrade) && GradeIsValid("presentation", presentationGrade))
                {
                    if (proposalGrade == "") { proposalGrade = "null"; }
                    if (progressGrade == "") { progressGrade = "null"; }
                    if (finalGrade == "") { finalGrade = "null"; }
                    if (presentationGrade == "") { presentationGrade = "null"; }

                    //***************
                    // Uses TODO 08 *
                    //***************
                    if (myFYPMSDB.UpdateGrades(fypId, studentUsername, proposalGrade, progressGrade, finalGrade, presentationGrade))
                    {
                        gvStudents.EditIndex = -1;
                        PopulateStudentRequirements(ddlGroups.SelectedValue);
                    }
                    else
                    {
                        myHelpers.ShowMessage(lblResultMessage, "*** SQL error in TODO 08: " + Global.sqlError + ".");
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                PopulateGroupsDropDownList();
            }
        }
    }
}