using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using FYPMSWebsite.App_Code;

namespace FYPMSWebsite.Faculty
{
    public partial class DisplayProjects : System.Web.UI.Page
    {
        //****************
        // Uses TODOS 19 *
        //****************
        private FYPMSDB myFYPMSDB = new FYPMSDB();
        private Helpers myHelpers = new Helpers();
        readonly string loggedinUsername = HttpContext.Current.User.Identity.Name;

        /***** Protected Methods *****/

        protected void GvProjects_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.Controls.Count == 7)
            {
                e.Row.Cells[1].Visible = false;
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    myHelpers.RenameGridViewColumn(e, "FYPCATEGORY", "CATEGORY");
                    myHelpers.RenameGridViewColumn(e, "FYPTYPE", "TYPE");
                    myHelpers.RenameGridViewColumn(e, "MINSTUDENTS", "MIN");
                    myHelpers.RenameGridViewColumn(e, "MAXSTUDENTS", "MAX");
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblResultMessage.Visible = false;
            pnlProjectInfo.Visible = false;

            //***************
            // Uses TODO 19 *
            //***************
            DataTable dtProjects = myFYPMSDB.GetSupervisorProjectDigest(loggedinUsername);

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "FYPID", "TITLE", "FYPCATEGORY", "FYPTYPE", "MINSTUDENTS", "MAXSTUDENTS" };

            // Display the query result if it is valid.
            if (myHelpers.IsQueryResultValid("19", dtProjects, attributeList, lblResultMessage))
            {
                if (dtProjects.Rows.Count != 0)
                {
                    gvProjects.DataSource = dtProjects;
                    gvProjects.DataBind();
                    pnlProjectInfo.Visible = true;
                }
                else // Nothing to display
                {
                    myHelpers.ShowMessage(lblResultMessage, "There are no projects.");
                }
            }
        }
    }
}