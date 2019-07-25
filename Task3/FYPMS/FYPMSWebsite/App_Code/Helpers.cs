using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace FYPMSWebsite.App_Code
{
    /// <summary>
    /// Helpers for the Fan Club Website
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
            if (nextId == -1)
            {
                ShowMessage(labelControl, "*** Error getting the next " + idName + " for table " + tableName + ". Please contact 3311rep.");
            }
            else
            {
                id = (nextId + 1).ToString();
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

        // Project specific methods.

        public bool CreateRequirementRecord(string facultyUsername, DataTable dtGroupMembers, System.Web.UI.WebControls.Label labelControl)
        {
            bool result = true;
            if (dtGroupMembers.Rows.Count != 0)
            {
                // Create the a Requirement record for each student in the group.
                foreach (DataRow row in dtGroupMembers.Rows)
                {
                    //***************
                    // Uses TODO 33 *
                    //***************
                    if (!myFYPMSDB.CreateRequirement(facultyUsername, row["USERNAME"].ToString(), "null", "null", "null", "null"))
                    {
                        ShowMessage(labelControl, "*** SQL error in TODO 33: " + Global.sqlError + ".");
                        result = false;
                    }
                }
            }
            else// Nothing was retrieved.
            {
                ShowMessage(labelControl, "*** The project group has no members. Please check your database.");
                result = false;
            }
            return result;
        }

        public DataTable GetFaculty(System.Web.UI.WebControls.Label labelControl)
        {
            //***************
            // Uses TODO 35 *
            //***************
            DataTable dtFaculty = myFYPMSDB.GetFaculty();

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "USERNAME", "FACULTYNAME" };

            // Display the query result if it is valid.
            if (IsQueryResultValid("35", dtFaculty, attributeList, labelControl))
            {
                return dtFaculty;
            }
            else
            {
                return null;
            }
        }

        public DataTable GetGroupAvailableProjectDigests(string groupId, System.Web.UI.WebControls.Label labelControl)
        {
            //***************
            // Uses TODO 36 *
            //***************
            DataTable dtProjects = myFYPMSDB.GetGroupAvailableProjectDigests(groupId);

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "FYPID", "TITLE", "FYPCATEGORY", "FYPTYPE", "MINSTUDENTS", "MAXSTUDENTS" };

            // Display the query result if it is valid.
            if (IsQueryResultValid("36", dtProjects, attributeList, labelControl))
            {
                return dtProjects;
            }
            else
            {
                return null;
            }
        }

        public DataTable GetProjectSupervisors(string fypId, System.Web.UI.WebControls.Label labelControl)
        {
            // Get all the supervisors of a project.
            //***************
            // Uses TODO 39 *
            //***************
            DataTable dtSupervisors = myFYPMSDB.GetSupervisors(fypId);

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "USERNAME", "FACULTYNAME" };

            if (IsQueryResultValid("39", dtSupervisors, attributeList, labelControl))
            {
                return dtSupervisors;
            }
            else
            {
                return null;
            }
        }

        public DataTable GetProjectCategories(System.Web.UI.WebControls.Label labelControl)
        {
            DataTable dtCategory = myFYPMSDB.GetProjectCategories();

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "FYPCATEGORY" };

            if (IsQueryResultValid("GetProjectCategories", dtCategory, attributeList, labelControl))
            {
                return dtCategory;
            }
            else
            {
                return null;
            }
        }

        public DataTable GetProjectDetails(string fypId, System.Web.UI.WebControls.Label labelControl)
        {
            DataTable dtProject = myFYPMSDB.GetProjectDetails(fypId);

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "FYPID", "TITLE", "FYPDESCRIPTION", "FYPCATEGORY", "FYPTYPE", "REQUIREMENT", "MINSTUDENTS", "MAXSTUDENTS", "ISAVAILABLE" };

            // Get the project information; save the result in ViewState and display it if it is not null.
            if (IsQueryResultValid("Get Project Details", dtProject, attributeList, labelControl))
            {
                return dtProject;
            }
            else
            {
                return null;
            }
        }

        public DataTable GetProjectDigests(System.Web.UI.WebControls.Label labelControl)
        {
            DataTable dtProjects = myFYPMSDB.GetProjectDigests();

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "FYPID", "TITLE", "FYPCATEGORY", "FYPTYPE", "MINSTUDENTS", "MAXSTUDENTS", "ISAVAILABLE" };

            // Display the query result if it is valid.
            if (IsQueryResultValid("Get Project Digests", dtProjects, attributeList, labelControl))
            {
                return dtProjects;
            }
            else
            {
                return null;
            }
        }

        public DataTable GetProjectsGroupInterestedIn(string groupId, System.Web.UI.WebControls.Label labelControl)
        {
            //***************
            // Uses TODO 40 *
            //***************
            DataTable dtProjectsGroupInterestedIn = myFYPMSDB.GetProjectsGroupInterestedIn(groupId);

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "FYPID", "TITLE", "FYPCATEGORY", "FYPTYPE", "FYPPRIORITY" };

            // Display the query result if it is valid.
            if (IsQueryResultValid("40", dtProjectsGroupInterestedIn, attributeList, labelControl))
            {
                return dtProjectsGroupInterestedIn;
            }
            else
            {
                return null;
            }
        }

        public DataTable GetProjectGroupMembers(string groupId, System.Web.UI.WebControls.Label labelControl)
        {
            //***************
            // Uses TODO 37 *
            //***************
            DataTable dtGroupMembers = myFYPMSDB.GetProjectGroupMembers(groupId);

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "USERNAME", "STUDENTNAME", "GROUPID" };

            if (IsQueryResultValid("37", dtGroupMembers, attributeList, labelControl))
            {
                return dtGroupMembers;
            }
            else
            {
                return null;
            }
        }

        public string GetStudentGroupId(string username, System.Web.UI.WebControls.Label labelControl)
        {
            string groupId = "SQL_ERROR";

            if (username != "")
            {
                //***************
                // Uses TODO 38 *
                //***************
                DataTable dtGroup = myFYPMSDB.GetStudentGroupId(username);

                // Attributes expected to be returned by the query result.
                var attributeList = new List<string> { "GROUPID" };

                // Display the query result if it is valid.
                if (IsQueryResultValid("38", dtGroup, attributeList, labelControl))
                {
                    if (dtGroup.Rows.Count != 0) // The student is a member of a group.
                    {
                        groupId = dtGroup.Rows[0]["GROUPID"].ToString();
                    }
                    else // The student is not a member of a group. 
                    {
                        groupId = "";
                    }
                }
            }
            else // There is no username; should not happen!
            {
                ShowMessage(labelControl, "*** Cannot get the username. Please check your database.");
            }
            return groupId;
        }

        public string IsGroupAssigned(string groupId, System.Web.UI.WebControls.Label labelControl)
        {
            string isAssigned = "false";

            if (groupId != "")
            {
                //***************
                // Uses TODO 34 *
                //***************
                DataTable dtIsAssigned = myFYPMSDB.GetAssignedFypId(groupId);

                // Attributes expected to be returned by the query result.
                var attributeList = new List<string> { "FYPASSIGNED" };

                // Display the query result if it is valid.
                if (IsQueryResultValid("34", dtIsAssigned, attributeList, labelControl))
                {
                    if (dtIsAssigned.Rows.Count != 0)
                    {
                        if (dtIsAssigned.Rows[0]["FYPASSIGNED"].ToString() != "")
                        {
                            isAssigned = "true";
                        }
                    }
                    else // Nothing returned; should not happen!
                    {
                        ShowMessage(labelControl, "There is no group with group id " + groupId + ". Please check your database.");
                        isAssigned = "SQL_ERROR";
                    }
                }
                else // An SQL error occurred.
                {
                    isAssigned = "SQL_ERROR";
                }
            }
            return isAssigned;
        }

        public string SupervisorsToString(DataTable dtSupervisors)
        {
            string result = dtSupervisors.Rows[0]["FACULTYNAME"].ToString().Trim();
            if (dtSupervisors.Rows.Count == 2)
            {
                result = result + ", " + dtSupervisors.Rows[1]["FACULTYNAME"].ToString().Trim();
            }
            return result;
        }

        public DataTable RemoveSupervisor(DataTable dtFaculty, string username)
        {
            // Remove the existing supervisor from the list of potential cosupervisors.
            foreach (DataRow rowFaculty in dtFaculty.Rows)
            {
                if (rowFaculty["USERNAME"].ToString().Equals(username))
                {
                    dtFaculty.Rows.Remove(rowFaculty);
                    return dtFaculty;
                }
            }
            return dtFaculty;
        }
    }
}
