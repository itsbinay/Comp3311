using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using FYPMSWebsite.App_Code;

namespace FYPMSWebsite.Faculty
{
    public partial class EditProject : System.Web.UI.Page
    {
        //*************************************************
        // Uses TODO 18, 20, 21, 22, 23; in Helpers.cs 35 *
        //*************************************************
        private FYPMSDB myFYPMSDB = new FYPMSDB();
        private Helpers myHelpers = new Helpers();
        private readonly string loggedinUsername = HttpContext.Current.User.Identity.Name;


        /***** Private Methods *****/

        private bool IsInterestIndicated(string fypId)
        {
            bool result = false;
            //***************
            // Uses TODO 20 *
            //***************
            DataTable dtInterestedIn = myFYPMSDB.GetInterestedInProject(fypId);

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "FYPID", "GROUPID", "FYPPRIORITY" };

            // Determine whether the result is valid.
            if (myHelpers.IsQueryResultValid("20", dtInterestedIn, attributeList, lblResultMessage))
            {
                // If a tuple exists for the project, return true.
                if (dtInterestedIn.Rows.Count != 0)
                { result = true; }
            }
            else
            {
                hfSQLError.Value="true";
            }
            return result;
        }

        private void PopulateCosupervisor()
        {
            // Uses TODO 35 in Helpers.cs.
            DataTable dtPossibleCosupervisors = myHelpers.GetFaculty(lblResultMessage);

            // Display the query result if it is valid.
            if (dtPossibleCosupervisors != null)
            {
                // Populate the cosupervisor dropdown list with the faculty that could be cosupervisors if there is no error in the query.
                if (dtPossibleCosupervisors != null)
                {
                    // Remove the existing supervisor from the list of potential cosupervisors.
                    dtPossibleCosupervisors = myHelpers.RemoveSupervisor(dtPossibleCosupervisors, loggedinUsername);
                    ddlCosupervisor.DataSource = dtPossibleCosupervisors;
                    ddlCosupervisor.DataValueField = "USERNAME";
                    ddlCosupervisor.DataTextField = "FACULTYNAME";
                    ddlCosupervisor.DataBind();
                    ddlCosupervisor.Items.Insert(0, "     -- None --");
                    ddlCosupervisor.Items.FindByText("     -- None --").Value = "";
                }
            }
            else // An SQL error occurred.
            {
                hfSQLError.Value = "true";
            }
        }

        private void PopulateProjectCategory()
        {
            DataTable dtCategory = myHelpers.GetProjectCategories(lblResultMessage);

            // Populate the category dropdown list with the FYP categories if there is no error in the query.
            if (dtCategory != null)
            {
                ddlCategory.DataSource = dtCategory;
                ddlCategory.DataValueField = "FYPCATEGORY";
                ddlCategory.DataTextField = "FYPCATEGORY";
                ddlCategory.DataBind();
            }
            else // An SQL error occurred.
            {
                hfSQLError.Value = "true";
            }
        }

        private void PopulateProjectInformation()
        {
            string fypId = Request["fypId"];

            if (hfSQLError.Value == "false")
            {
                if (!IsInterestIndicated(fypId))
                {
                    {
                        PopulateCosupervisor();
                        PopulateProjectCategory();

                        DataTable dtProject = myHelpers.GetProjectDetails(fypId, lblResultMessage);

                        // Get the project information; save the result in ViewState and display it if it is not null.
                        if (dtProject != null)
                        {
                            if (dtProject.Rows.Count != 0)
                            {
                                ViewState["fypId"] = dtProject.Rows[0]["FYPID"].ToString().Trim();
                                ViewState["oldTitle"] = txtTitle.Text = dtProject.Rows[0]["TITLE"].ToString().Trim();
                                ViewState["oldDescription"] = txtDescription.Text = dtProject.Rows[0]["FYPDESCRIPTION"].ToString().Trim();
                                ViewState["oldCategory"] = ddlCategory.SelectedValue = dtProject.Rows[0]["FYPCATEGORY"].ToString().Trim();
                                ViewState["oldType"] = rblType.SelectedValue = dtProject.Rows[0]["FYPTYPE"].ToString().Trim();
                                ViewState["oldRequirement"] = txtRequirement.Text = dtProject.Rows[0]["REQUIREMENT"].ToString().Trim();
                                ViewState["oldMinStudents"] = rblMinStudents.SelectedValue = dtProject.Rows[0]["MINSTUDENTS"].ToString().Trim();
                                ViewState["oldMaxStudents"] = rblMaxStudents.SelectedValue = dtProject.Rows[0]["MAXSTUDENTS"].ToString().Trim();
                                ViewState["oldIsAvailable"] = rblIsAvailable.SelectedValue = dtProject.Rows[0]["ISAVAILABLE"].ToString().Trim();

                                //***************
                                // Uses TODO 21 *
                                //***************
                                DataTable dtCosupervisor = myFYPMSDB.GetCosupervisorInfoForEdit(fypId, loggedinUsername);

                                // Attributes expected to be returned by the query result.
                                var attributeList = new List<string> { "USERNAME" };

                                // Get the cosupervisor information; save the result in ViewState and display it if it is valid.
                                if (myHelpers.IsQueryResultValid("21", dtCosupervisor, attributeList, lblResultMessage))
                                {
                                    if (dtCosupervisor.Rows.Count != 0)
                                    {
                                        ViewState["oldCosupervisor"] = ddlCosupervisor.SelectedValue = dtCosupervisor.Rows[0]["USERNAME"].ToString().Trim();
                                    }
                                    else
                                    {
                                        ViewState["oldCosupervisor"] = "";
                                    }
                                }
                                else // An SQL error occurred.
                                {
                                    hfSQLError.Value = "true";
                                }
                            }
                            else // Nothing to display.
                            {
                                myHelpers.ShowMessage(lblResultMessage, "There are no projects.");
                            }
                        }
                    }
                }
                else
                {
                    pnlProjectInfo.Visible = false;
                    myHelpers.ShowMessage(lblResultMessage, Request["title"] + " cannot be edited as one or more groups have indicated an interest in this project.");
                }
            }
        }

        private bool ProjectInformationIsChanged(DataTable dtOldNewProjectValues)
        {
            bool result = false;
            foreach (DataRow row in dtOldNewProjectValues.Rows)
            {
                if (!row["oldValue"].ToString().Equals(row["newValue"].ToString()))
                {
                    result = true;
                }
            }
            return result;
        }


        /***** Protected Methods *****/

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Faculty/DisplayProjects");
        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (IsValid && hfSQLError.Value == "false")
            {
                lblResultMessage.Visible = false;
                string fypId = ViewState["fypId"].ToString();
                string oldCosupervisor = ViewState["oldCosupervisor"].ToString();
                string resultMessage = "You have not changed any information.";

                // Collect the updated project values.
                string newTitle = myHelpers.CleanInput(txtTitle.Text);
                string newDescription = myHelpers.CleanInput(txtDescription.Text);
                string newCategory = ddlCategory.SelectedValue;
                string newType = rblType.SelectedValue;
                string newRequirement = myHelpers.CleanInput(txtRequirement.Text);
                string newMinStudents = rblMinStudents.SelectedValue;
                string newMaxStudents = rblMaxStudents.SelectedValue;
                string newIsAvailable = rblIsAvailable.SelectedValue;
                string newCosupervisor = ddlCosupervisor.SelectedValue;

                DataTable dtOldNewProjectValues = new DataTable();
                dtOldNewProjectValues.Columns.Add("oldValue", typeof(string));
                dtOldNewProjectValues.Columns.Add("newValue", typeof(string));
                // Collect the old and new project values into a DataTable.
                dtOldNewProjectValues.Rows.Add(new object[] { ViewState["oldTitle"].ToString(), newTitle });
                dtOldNewProjectValues.Rows.Add(new object[] { ViewState["oldDescription"].ToString(), newDescription });
                dtOldNewProjectValues.Rows.Add(new object[] { ViewState["oldCategory"].ToString(), newCategory });
                dtOldNewProjectValues.Rows.Add(new object[] { ViewState["oldType"].ToString(), newType });
                dtOldNewProjectValues.Rows.Add(new object[] { ViewState["oldRequirement"].ToString(), newRequirement });
                dtOldNewProjectValues.Rows.Add(new object[] { ViewState["oldMinStudents"].ToString(), newMinStudents });
                dtOldNewProjectValues.Rows.Add(new object[] { ViewState["oldMaxStudents"].ToString(), newMaxStudents });
                dtOldNewProjectValues.Rows.Add(new object[] { ViewState["oldIsAvailable"].ToString(), newIsAvailable });

                // Update the changed FYP project information.
                if (ProjectInformationIsChanged(dtOldNewProjectValues) || oldCosupervisor != newCosupervisor)
                {
                    //***********************
                    // Uses TODO 18, 22, 23 *
                    //***********************
                    if (myFYPMSDB.UpdateFYProject(fypId, newTitle, newDescription, newCategory, newType, newRequirement,
                        newMinStudents, newMaxStudents, newIsAvailable, oldCosupervisor, newCosupervisor))
                    {
                        pnlProjectInfo.Visible = false; ;
                        resultMessage = "The project information for - " + newTitle + " - has been updated.";
                    }
                    else // An SQL error occurred in method CreateSupervises (TODO 18), UpdateFYProject (TODO 22) or DeleteSupervises (TODO 23) in FYPMSDB.cs.
                    {
                        resultMessage = "*** SQL error in TODO 18, 22 or 23: " + Global.sqlError + ".";
                    }
                }
                myHelpers.ShowMessage(lblResultMessage, resultMessage);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateProjectInformation();
            }
        }

        protected void RblMaxStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblType.SelectedValue == "thesis")
            {
                rblMaxStudents.SelectedValue = "1";
            }
            cvMinMaxStudents.Validate();
        }

        protected void RblMinStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblType.SelectedValue == "thesis")
            {
                rblMinStudents.SelectedValue = "1";
            }
            cvMinMaxStudents.Validate();
        }

        protected void RblType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rblType.SelectedValue == "thesis")
            {
                rblMinStudents.SelectedValue = "1";
                rblMaxStudents.SelectedValue = "1";
            }
        }
    }
}