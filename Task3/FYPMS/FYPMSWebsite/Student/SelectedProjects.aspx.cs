using FYPMSWebsite.App_Code;
using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace FYPMSWebsite.Student
{
    public partial class SelectedProjects : System.Web.UI.Page
    {
        //**************************************
        // Uses TODO in SharedAccess.cs 38, 40 *
        //**************************************
        private FYPMSDB myFYPMSDB = new FYPMSDB();
        private Helpers myHelpers = new Helpers();
        SharedAccess mySharedAccess = new SharedAccess();
        readonly string loggedinUsername = HttpContext.Current.User.Identity.Name;


        /***** Private Methods *****/

        private void GetSelectedProjects(string groupId)
        {
            lblResultMessage.Visible = false;

            // If the group id is empty, the student is not yet a member of any group.
            if (groupId != "")
            {
                // Uses TODO 40 in SharedAccess.cs.
                DataTable dtProjects = mySharedAccess.GetProjectsGroupInterestedIn(groupId, lblResultMessage);

                if (dtProjects != null)
                {
                    if (dtProjects.Rows.Count != 0)
                    {
                        gvSelectedProjects.DataSource = dtProjects;
                        gvSelectedProjects.DataBind();
                        pnlSelectedProjects.Visible = true;
                        pnlSelectProjects.Visible = true;
                    }
                    else
                    {
                        myHelpers.ShowMessage(lblResultMessage, "Your group has not indicated an interest in any projects.");
                        pnlSelectProjects.Visible = true;
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

        protected void GvSelectedProjects_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Controls.Count == 6)
            {
                e.Row.Cells[1].Visible = false;
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    myHelpers.RenameGridViewColumn(e, "FYPCATEGORY", "CATEGORY");
                    myHelpers.RenameGridViewColumn(e, "FYPTYPE", "TYPE");
                    myHelpers.RenameGridViewColumn(e, "FYPPRIORITY", "PRIORITY");
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Uses TODO 38 in SharedAccess.cs.
            string groupId = mySharedAccess.GetStudentGroupId(loggedinUsername, lblResultMessage);
            if (groupId != "SQL_ERROR")
            {
                if (!IsPostBack)
                {
                    GetSelectedProjects(groupId);
                }
            }
        }
    }
}