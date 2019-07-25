using FYPMSWebsite.App_Code;
using System;
using System.Data;

namespace FYPMSWebsite
{
    public partial class DisplayProjectDetails : System.Web.UI.Page
    {
        //*********************************
        // Uses TODO in Helpers.cs 38, 39 *
        //*********************************

        private FYPMSDB myFYPMSDB = new FYPMSDB();
        private Helpers myHelpers = new Helpers();
        private string returnUrl;


        /***** Private Methods *****/

        private void PopulateProjectDetails()
        {
            string fypId = Request["fypId"];
            returnUrl = Request["returnUrl"];

            if (fypId != "")
            {
                // Uses TODO 38 in Helpers.cs.
                DataTable dtProject = myHelpers.GetProjectDetails(fypId, lblResultMessage);

                // Get the project information and display it if it is valid.
                if (dtProject != null)
                {
                    if (dtProject.Rows.Count != 0)
                    {
                        txtTitle.Text = dtProject.Rows[0]["TITLE"].ToString().Trim();
                        txtDescription.Text = dtProject.Rows[0]["FYPDESCRIPTION"].ToString().Trim();
                        txtCategory.Text = dtProject.Rows[0]["FYPCATEGORY"].ToString().Trim();
                        txtType.Text = dtProject.Rows[0]["FYPTYPE"].ToString().Trim();
                        txtRequirement.Text = dtProject.Rows[0]["REQUIREMENT"].ToString().Trim();
                        txtMinStudents.Text = dtProject.Rows[0]["MINSTUDENTS"].ToString().Trim();
                        txtMaxStudents.Text = dtProject.Rows[0]["MAXSTUDENTS"].ToString().Trim();

                        // Uses TODO 39 in Helpers.cs.
                        DataTable dtSupervisors = myHelpers.GetProjectSupervisors(fypId, lblResultMessage);
                        
                        if (dtSupervisors != null)
                        {
                            if (dtSupervisors.Rows.Count != 0)
                            {
                                txtSupervisor.Text = myHelpers.SupervisorsToString(dtSupervisors);
                            }
                            else // Nothing to display.
                            {
                                myHelpers.ShowMessage(lblResultMessage, "There are no supervisors for this project.");
                            }
                        }
                    }
                    else // Nothing to display.
                    {
                        myHelpers.ShowMessage(lblResultMessage, "There are no projects.");
                    }
                }
            }
        }


        /***** Protected Methods *****/

        protected void BtnReturn_Click(object sender, EventArgs e)
        {
            Response.Redirect(returnUrl);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PopulateProjectDetails();
        }
    }
}