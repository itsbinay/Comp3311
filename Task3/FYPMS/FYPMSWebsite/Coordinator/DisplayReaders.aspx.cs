using FYPMSWebsite.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace FYPMSWebsite.Coordinator
{
    public partial class DisplayReaders : System.Web.UI.Page
    {
        //***************
        // Uses TODO 04 *
        //***************
        private FYPMSDB myFYPMSDB = new FYPMSDB();
        private Helpers myHelpers = new Helpers();

        protected void GvAssignedReaders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.Controls.Count == 3)
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    myHelpers.RenameGridViewColumn(e, "FACULTYNAME", "READER");
                    myHelpers.RenameGridViewColumn(e, "GROUPCODE", "GROUP CODE");
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //***************
            // Uses TODO 04 *
            //***************
            DataTable dtReaders = myFYPMSDB.GetAssignedReaders();

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "FACULTYNAME", "GROUPCODE", "TITLE" };

            if (myHelpers.IsQueryResultValid("04", dtReaders, attributeList, lblResultMessage))
            {
                if (dtReaders.Rows.Count != 0)
                {
                    gvAssignedReaders.DataSource = dtReaders;
                    gvAssignedReaders.DataBind();
                    pnlAssignedReaders.Visible = true;
                }
                else // Nothing to display
                {
                    myHelpers.ShowMessage(lblResultMessage, "There are no projects with assigned readers.");
                    pnlAssignedReaders.Visible = false;
                }
            }
        }
    }
}