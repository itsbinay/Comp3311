using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace FYPMSWebsite.App_Code
{
    /// <summary>
    /// Helpers for the website project.
    /// </summary>

    public class Helpers
    {
        private OracleDBAccess myOracleDBAccess = new OracleDBAccess();
        private FYPMSDB myFYPMSDB = new FYPMSDB();
        private string sql;

        public Helpers()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public string CleanInput(string input)
        {
            // Replace single quote by two quotes and remove leading and trailing spaces.
            return input.Replace("'", "''").Trim();
        }

        public bool IsQueryResultValid(string TODO, DataTable datatableToCheck, List<string> columnsNames, System.Web.UI.WebControls.Label labelControl)
        {
            bool isQueryResultValid = true;
            if (datatableToCheck != null)
            {
                if (datatableToCheck.Columns != null && datatableToCheck.Columns.Count == columnsNames.Count)
                {
                    foreach (string columnName in columnsNames)
                    {
                        if (!datatableToCheck.Columns.Contains(columnName))
                        {
                            ShowMessage(labelControl, "*** The SELECT statement of TODO " + TODO + " does not retrieve the attribute " + columnName + ".");
                            isQueryResultValid = false;
                            break;
                        }
                    }
                }
                else
                {
                    ShowMessage(labelControl, "*** The SELECT statement of TODO " + TODO + " retrieves " + datatableToCheck.Columns.Count + " attributes while the required number is " + columnsNames.Count + ".");
                    isQueryResultValid = false;
                }
            }
            else // An SQL error occurred.
            {
                ShowMessage(labelControl, "*** SQL error in TODO " + TODO + ": " + Global.sqlError + ".");
                isQueryResultValid = false;
            }
            return isQueryResultValid;
        }

        public bool IsInteger(string number)
        {
            int n;
            return int.TryParse(number, out n);
        }

        public bool IsValidAndInRange(string number, decimal min, decimal max)
        {
            decimal n;
            if (decimal.TryParse(number, out n))
            {
                if (min <= n && n <= max)
                { return true; }
            }
            return false;
        }

        public int GetColumnIndexByName(GridViewRowEventArgs e, string columnName)
        {
            for (int i = 0; i < e.Row.Controls.Count; i++)
                if (e.Row.Cells[i].Text.ToLower().Trim() == columnName.ToLower().Trim())
                {
                    return i;
                }
            return -1;
        }

        public string GetNextTableId(string tableName, string idName, System.Web.UI.WebControls.Label labelControl)
        {
            string id = "";
            sql = "select max(" + idName + ") from " + tableName;
            decimal nextId = myOracleDBAccess.GetAggregateValue(sql);
            if (nextId != -1)
            {
                id = (nextId + 1).ToString();
            }
            else //An SQL error occurred.
            {
                ShowMessage(labelControl, "*** Error getting the next " + idName + " for table " + tableName + ". Please contact 3311rep.");
            }
            return id;
        }

        public void RenameGridViewColumn(GridViewRowEventArgs e, string fromName, string toName)
        {
            int col = GetColumnIndexByName(e, fromName);
            // If the column is not found, ignore renaming.
            if (col != -1)
            {
                e.Row.Cells[col].Text = toName;
            }
        }

        public void ShowMessage(System.Web.UI.WebControls.Label labelControl, string message)
        {
            if (message.Substring(0, 3) == "***" || message.Substring(0, 6) == "Please") // Error message.
            {
                labelControl.ForeColor = System.Drawing.Color.Red;
            }
            else // Information message.
            {
                labelControl.ForeColor = System.Drawing.Color.Blue; // "#FF0000FF"
            }
            labelControl.Text = message;
            labelControl.Visible = true;
        }
    }
}
