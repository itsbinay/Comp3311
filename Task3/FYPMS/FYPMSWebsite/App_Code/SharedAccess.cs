using System.Data;
using System.Collections.Generic;


namespace FYPMSWebsite.App_Code
{
    /// <summary>
    /// Project specific methods.
    /// </summary>

    public class SharedAccess
    {
        private FYPMSDB myFYPMSDB = new FYPMSDB();
        private Helpers myHelpers = new Helpers();

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
                    // Uses TODO 33 *
                    //***************
                    if (!myFYPMSDB.CreateRequirement(facultyUsername, row["USERNAME"].ToString(), "null", "null", "null", "null"))
                    {
                        myHelpers.ShowMessage(labelControl, "*** SQL error in TODO 33: " + Global.sqlError + ".");
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
            // Uses TODO 35 *
            //***************
            DataTable dtFaculty = myFYPMSDB.GetFaculty();

            // Attributes expected to be returned by the query result.
            var attributeList = new List<string> { "USERNAME", "FACULTYNAME" };

            // Display the query result if it is valid.
            if (myHelpers.IsQueryResultValid("35", dtFaculty, attributeList, labelControl))
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
            if (myHelpers.IsQueryResultValid("36", dtProjects, attributeList, labelControl))
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

            if (myHelpers.IsQueryResultValid("39", dtSupervisors, attributeList, labelControl))
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

            if (myHelpers.IsQueryResultValid("GetProjectCategories", dtCategory, attributeList, labelControl))
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
            if (myHelpers.IsQueryResultValid("Get Project Details", dtProject, attributeList, labelControl))
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
            if (myHelpers.IsQueryResultValid("Get Project Digests", dtProjects, attributeList, labelControl))
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
            if (myHelpers.IsQueryResultValid("40", dtProjectsGroupInterestedIn, attributeList, labelControl))
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

            if (myHelpers.IsQueryResultValid("37", dtGroupMembers, attributeList, labelControl))
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
                if (myHelpers.IsQueryResultValid("38", dtGroup, attributeList, labelControl))
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
                myHelpers.ShowMessage(labelControl, "*** Cannot get the username. Please check your database.");
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
                if (myHelpers.IsQueryResultValid("34", dtIsAssigned, attributeList, labelControl))
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
                        myHelpers.ShowMessage(labelControl, "There is no group with group id " + groupId + ". Please check your database.");
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
