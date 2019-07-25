using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FYPMSWebsite.App_Code
{
    /// <summary>
    /// Helpers for the Fan Club Website
    /// </summary>

    public class SharedAccess
    {
        private OracleDBAccess myOracleDBAccess = new OracleDBAccess();
        private FYPMSDB myFYPMSDB = new FYPMSDB();
        private Helpers myHelpers = new Helpers();
        private string sql;

        public SharedAccess()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public bool CreateRequirementRecord(string facultyUsername, DataTable dtGroupMembers, System.Web.UI.WebControls.Label labelControl)
        {
            bool result = true;
            if (dtGroupMembers.Rows.Count != 0)
            {
                // Create the a Requirement record for each student in the group.
                foreach (DataRow row in dtGroupMembers.Rows)
                {
                    //***************
                    // Uses TODO XX *
                    //***************
                    if (!myFYPMSDB.CreateRequirement(facultyUsername, row["USERNAME"].ToString(), "null", "null", "null", "null"))
                    {
                        myHelpers.ShowMessage(labelControl, "*** The SQL statement of TODO XX has an error or is incorrect.");
                        result = false;
                    }
                }
            }
            else// Nothing was retrieved.
            {
                myHelpers.ShowMessage(labelControl, "*** The project group has no members. Please check your database.");
                result = false;
            }
            return result;
        }

        public DataTable GetFaculty(System.Web.UI.WebControls.Label labelControl)
        {
            //***************
            // Uses TODO XX *
            //***************
            DataTable dtFaculty = myFYPMSDB.GetFaculty();

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "USERNAME", "FACULTYNAME" };

            // Display the query result if it is valid.
            if (myHelpers.IsQueryResultValid("03", dtFaculty, attributeList, labelControl))
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
            // Uses TODO XX *
            //***************
            DataTable dtProjects = myFYPMSDB.GetGroupAvailableProjectDigests(groupId);

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "FYPID", "TITLE", "FYPCATEGORY", "FYPTYPE", "MINSTUDENTS", "MAXSTUDENTS" };

            // Display the query result if it is valid.
            if (myHelpers.IsQueryResultValid("XX", dtProjects, attributeList, labelControl))
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
            // Uses TODO XX *
            //***************
            DataTable dtSupervisors = myFYPMSDB.GetSupervisors(fypId);

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "USERNAME", "FACULTYNAME" };

            if (myHelpers.IsQueryResultValid("XX", dtSupervisors, attributeList, labelControl))
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
            //***************
            // Uses TODO XX *
            //***************
            DataTable dtCategory = myFYPMSDB.GetProjectCategories();

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "FYPCATEGORY" };

            if (myHelpers.IsQueryResultValid("XX", dtCategory, attributeList, labelControl))
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
            //***************
            // Uses TODO XX *
            //***************
            DataTable dtProject = myFYPMSDB.GetProjectDetails(fypId);

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "FYPID", "TITLE", "FYPDESCRIPTION", "FYPCATEGORY", "FYPTYPE", "REQUIREMENT", "MINSTUDENTS", "MAXSTUDENTS", "ISAVAILABLE" };

            // Get the project information; save the result in ViewState and display it if it is not null.
            if (myHelpers.IsQueryResultValid("XX", dtProject, attributeList, labelControl))
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
            //***************
            // Uses TODO XX *
            //***************
            DataTable dtProjects = myFYPMSDB.GetProjectDigests();

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "FYPID", "TITLE", "FYPCATEGORY", "FYPTYPE", "MINSTUDENTS", "MAXSTUDENTS", "ISAVAILABLE" };

            // Display the query result if it is valid.
            if (myHelpers.IsQueryResultValid("01", dtProjects, attributeList, labelControl))
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
            // Uses TODO XX *
            //***************
            DataTable dtProjectsGroupInterestedIn = myFYPMSDB.GetProjectsGroupInterestedIn(groupId);

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "FYPID", "TITLE", "FYPCATEGORY", "FYPTYPE", "FYPPRIORITY" };

            // Display the query result if it is valid.
            if (myHelpers.IsQueryResultValid("XX", dtProjectsGroupInterestedIn, attributeList, labelControl))
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
            // Uses TODO XX *
            //***************
            DataTable dtGroupMembers = myFYPMSDB.GetProjectGroupMembers(groupId);

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "USERNAME", "STUDENTNAME", "GROUPID" };

            if (myHelpers.IsQueryResultValid("XX", dtGroupMembers, attributeList, labelControl))
            {
                return dtGroupMembers;
            }
            else
            {
                return null;
            }
        }

        public string GetStudentGroupId(string username)
        {
            string groupId = "";

            if (username != "")
            {
                //***************
                // Uses TODO XX *
                //***************
                DataTable dtGroup = myFYPMSDB.GetStudentGroupId(username);

                // Display the query result if it is valid.
                if (dtGroup != null)
                {
                    if (dtGroup.Rows.Count != 0) // The user is a member of a group.
                    {
                        groupId = dtGroup.Rows[0]["GROUPID"].ToString();
                    }
                }
            }
            return groupId;
        }

        public bool IsGroupAssigned(string groupId, System.Web.UI.WebControls.Label labelControl)
        {
            bool isAssigned = false;

            if (groupId != "")
            {
                //***************
                // Uses TODO XX *
                //***************
                DataTable dtIsAssigned = myFYPMSDB.GetAssignedFypId(groupId);

                // Attributes expected to be returned by the query result.
                var attributeList = new List<string> { "FYPASSIGNED" };

                // Display the query result if it is valid.
                if (myHelpers.IsQueryResultValid("XX", dtIsAssigned, attributeList, labelControl))
                {
                    if (dtIsAssigned.Rows.Count != 0)
                    {
                        if (dtIsAssigned.Rows[0]["FYPASSIGNED"].ToString() != "")
                        {
                            isAssigned = true;
                        }
                    }
                    else // Nothing returned; should not happen!
                    {
                        myHelpers.ShowMessage(labelControl, "There is no group with group id " + groupId + ". Please check your database.");
                    }
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
