using FYPMSWebsite.App_Code;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace FYPMSWebsite
{
    public partial class DisplayAllProjects : System.Web.UI.Page
    {
        private FYPMSDB myFYPMSDB = new FYPMSDB();
        private Helpers myHelpers = new Helpers();


        /***** Private Methods *****/

        private void GetAllProjects()
        {
            lblResultMessage.Visible = false;

            DataTable dtProjects = myHelpers.GetProjectDigests(lblResultMessage);

            // Display the query result if it is valid.
            if (dtProjects != null)
            {
                if (dtProjects.Rows.Count != 0)
                {
                    gvProjects.DataSource = dtProjects;
                    gvProjects.DataBind();
                    pnlProjectInfo.Visible = true;
                }
                else // Nothing to display
                {
                    myHelpers.ShowMessage(lblResultMessage, "There are no available projects.");
                }
            }
        }


        /***** Protected Methods *****/

        protected void Page_Load(object sender, EventArgs e)
        {
            GetAllProjects();
        }

        protected void GvProjects_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Controls.Count == 8)
            {
                e.Row.Cells[1].Visible = false;
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    myHelpers.RenameGridViewColumn(e, "FYPCATEGORY", "CATEGORY");
                    myHelpers.RenameGridViewColumn(e, "FYPTYPE", "TYPE");
                    myHelpers.RenameGridViewColumn(e, "MINSTUDENTS", "MIN");
                    myHelpers.RenameGridViewColumn(e, "MAXSTUDENTS", "MAX");
                    myHelpers.RenameGridViewColumn(e, "ISAVAILABLE", "AVAILABLE?");
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }

        protected void gvProjects_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProjects.PageIndex = e.NewPageIndex;
            gvProjects.DataBind();
        }
    }
}