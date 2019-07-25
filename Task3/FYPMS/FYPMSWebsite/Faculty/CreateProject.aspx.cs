using System;
using System.Data;
using System.Web;
using System.Web.UI;
using FYPMSWebsite.App_Code;

namespace FYPMSWebsite.Faculty
{
    public partial class CreateProject : System.Web.UI.Page
    {
        //**************************************
        // USES TODO: 17, 18; in Helpers.cs 35 *
        //**************************************
        private FYPMSDB myFYPMSDB = new FYPMSDB();
        private Helpers myHelpers = new Helpers();
        private readonly string loggedinUsername = HttpContext.Current.User.Identity.Name;


        /***** Private Methods *****/

        private void PopulateCosupervisor()
        {
            // Uses TODO 35 in Helpers.cs.
            DataTable dtPossibleCosupervisors = myHelpers.GetFaculty(lblResultMessage);

            // Populate the cosupervisor dropdown list.
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
            else
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
            else
            {
                hfSQLError.Value = "true";
            }
        }


        /***** Protected Methods *****/

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/");
        }

        protected void BtnCreate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid && hfSQLError.Value == "false")
            {
                lblResultMessage.Visible = false;
                // Collect the project information.
                string title = myHelpers.CleanInput(txtTitle.Text);
                string description = myHelpers.CleanInput(txtDescription.Text);
                string cosupervisor = ddlCosupervisor.SelectedValue;
                string category = ddlCategory.SelectedValue;
                string type = rblType.SelectedValue;
                string requirement = myHelpers.CleanInput(txtRequirement.Text);
                string minStudents = rblMinStudents.SelectedValue;
                string maxStudents = rblMaxStudents.SelectedValue;
                string isAvailable = rblIsAvailable.SelectedValue;
                string fypId = myHelpers.GetNextTableId("FYProject", "fypId", lblResultMessage);

                if (fypId != "")
                {
                    //Create the FYProject and Supervises records.
                    //*******************
                    // Uses TODO 17, 18 *
                    //*******************
                    if (myFYPMSDB.CreateFYProject(fypId, title, description, category, type,
                    requirement, minStudents, maxStudents, isAvailable, loggedinUsername, cosupervisor))
                    {
                        pnlProjectInfo.Visible = false;
                        myHelpers.ShowMessage(lblResultMessage, "The project - " + title + " - has been created.");
                    }
                    else // An SQL error occurrd in CreateFYProject or CreateSupervises in FYPMSDB.cs.
                    {
                        myHelpers.ShowMessage(lblResultMessage, "*** SQL error in TODO 17 or 18: " + Global.sqlError + ".");
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateCosupervisor();
                PopulateProjectCategory();
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