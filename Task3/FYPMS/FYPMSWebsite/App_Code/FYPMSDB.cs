﻿using System.Data;
using Oracle.DataAccess.Client;


namespace FYPMSWebsite.App_Code
{
    /// <summary>
    /// Student name: GURUNG Binay
    /// Student number: 20508606
    /// 
    /// NOTE: This is an individual task. By submitting this file you certify that this
    /// code is the result of your individul effort and that it has not been developed
    /// in collaoration with or copied from any other person. If this is not the case,
    /// then you must identify by name all the persons with whom you collaborated or
    /// from whom you copied code below.
    /// 
    /// Collaborators: 
    /// </summary>

    public class FYPMSDB
    {
        //******************************** IMPORTANT NOTE **********************************
        // For the web pages to display a query result correctly, the attribute names in   *
        // the query result table must be EXACTLY the same as that in the database tables. *
        //         Report problems with the website code to 3311rep@cse.ust.hk.            *
        //**********************************************************************************

        private OracleDBAccess myOracleDBAccess = new OracleDBAccess();
        private string sql;

        public DataTable GetProjectsWithoutReaders()
        {
            //**************************************************************************
            // TODO 01: Used in Coordinator/AssignReader.aspx.cs                       *
            // Construct the SQL SELECT statement to retrieve the group id, group code *
            // assigned fyp id and project title, category and type for the project groups that DO NOT *
            // have an assigned reader. Order the result by group code ascending.      *
            //**************************************************************************
            sql = "select p.groupid,p.groupCode,p.fypAssigned,f.title,f.fypCategory,f.fypType from ProjectGroup p,FYProject f " +
                "where p.fypAssigned=f.fypId and reader is NULL order by p.groupCode asc";
            return myOracleDBAccess.GetData(sql);
        }

        public decimal NumberProjectsAssignedToReader(string username)
        {
            //*****************************************************************************
            // TODO 02: Used in Coordinator/AssignReader.aspx.cs                          *
            // Construct the SQL SELECT statement to retrieve the the number of projects  *
            // to which a faculty, identified by his/her username, is assigned as reader. *
            //*****************************************************************************
            sql = "select count(*) from ProjectGroup where reader='"+username+"'";
            return myOracleDBAccess.GetAggregateValue(sql);
        }

        public bool AssignReaderToProject(string groupId, string username)
        {
            //**********************************************************************
            // TODO 03: Used in AssignReader.aspx.cs                               *
            // Construct the SQL UPDATE statement to assign a reader, identified   * 
            // by his/her username, to a project group, identified by its groupId. *
            //**********************************************************************
            sql = "Update ProjectGroup set reader='"+username+"' where groupId="+groupId;
            return SetData(sql);
        }

        public DataTable GetAssignedReaders()
        {
            //************************************************************************
            // TODO 04: Used in Coordinator/DisplayProjectReaders.aspx.cs            *
            // Construct the SQL SELECT statement to retrieve the reader name, group * 
            // code and project title for the project groups with assigned readers.  *
            // Order the result by group code ascending.                             *
            //************************************************************************
            sql = "select f.facultyName,p.groupCode,fyp.title from Faculty f, FYProject fyp," +
                "ProjectGroup p where p.fypAssigned=fyp.fypId and p.reader=f.username and p.reader is NOT NULL " +
                "order by p.groupCode asc";
            return myOracleDBAccess.GetData(sql);
        }

        public DataTable GetFacultyCode(string username)
        {
            //*************************************************************
            // TODO 05: Used in Faculty/AssignGrades.aspx.cs              *
            // Construct the SQL SELECT statement to retrieve the faculty *
            // code of a faculty identified by his/her username.          *
            //*************************************************************
            sql = "select facultyCode from Faculty where username='"+username+"'";
            return myOracleDBAccess.GetData(sql);
        }

        public DataTable GetFacultyGroups(string facultyCode)
        {
            //****************************************************************************************
            // TODO 06: Used in Faculty/AssignGrades.aspx.cs                                         *
            // Construct the SQL SELECT statement to retrieve the group id, group code and FYP id of *
            // the project to which the group is assigned for the groups supervised by the faculty   *
            // identified by his/her faculty code. Order the result by group id ascending.           *
            //****************************************************************************************
            sql = "select distinct p.groupId,p.groupCode,p.fypAssigned from ProjectGroup p,Faculty f,Supervises s "
                + "where p.fypAssigned=s.fypId and s.username=f.username and f.facultyCode='" + facultyCode + "' " +
                "order by p.groupId asc";
                /*"select p.groupId,p.groupCode,p.fypAssigned from ProjectGroup p,Faculty f where " +
                "p.reader=f.username and f.facultyCode='"+facultyCode+"' order by p.groupId asc";*/
            return myOracleDBAccess.GetData(sql);
        }

        public DataTable GetStudentRequirements(string groupId, string fypId)
        {
            //****************************************************************************************
            // TODO 07: Used in Faculty/AssignGrades.aspx.cs                                         *
            // Construct the SQL SELECT statement to retrieve students' username, name and all the   *
            // requirement grades given by any of the faculty that are the supervisors of a project  *
            // identified by its fyp id, for all the students in a group identified by its group id. *
            //****************************************************************************************
            sql = "select s.Username,s.studentname,r.proposalGrade," +
                "r.progressGrade,r.finalGrade,r.presentationGrade from Requirement r, Students s,ProjectGroup p,Faculty f " +
                "where r.studentUsername=s.username and p.groupId=s.groupId and r.facultyusername=f.username and p.groupId='" + groupId+"' and p.fypAssigned="+fypId +
                " and r.facultyUsername in (select username from Supervises where fypId="+fypId+" )";
            return myOracleDBAccess.GetData(sql);
        }

        public bool UpdateGrades(string fypId, string studentUsername, string proposalGrade,
            string progressGrade, string finalGrade, string presentationGrade)
        {
            //***************************************************************************************
            // TODO 08: Used in Faculty/AssignGrades.aspx.cs                                        *
            // Construct the SQL UPDATE statement to update ALL grade values in a requirement       *
            // record, identified by a faculty username and a student username. NOTE: While a grade *
            // can be updated by ANY of the supervisors of a project, only the username of the      *
            // faculty who assigned a group to a project appears in the Requirement table record.   *                                                  *
            //***************************************************************************************
            sql = "update Requirement set progressGrade =" + progressGrade + ", finalGrade=" + finalGrade +
                ",presentationGrade=" + presentationGrade + ",proposalGrade=" + proposalGrade +
                " where studentUsername='" + studentUsername + "' and facultyUsername in " +
                "(select username from Supervises where fypId=" + fypId+")";

            return SetData(sql);
        }

        public DataTable GetProjectFacultyCodes(string fypId)
        {
            //******************************************************************
            // TODO 09: Used in Faculty/AssignGroupToProject.aspx.cs           *
            // Construct the SQL SELECT statement to retrieve the faculty      *
            // codes of the supervisors of a project identified by its FYP id. *
            // Order the result by faculty code ascending.                     *
            //******************************************************************
            sql = "select f.facultyCode from Faculty f, Supervises s where " +
                "s.username=f.username and s.fypId="+fypId;
            return myOracleDBAccess.GetData(sql);
        }

        public decimal GetFacultyCodeSequenceNumber(string groupCodePrefix)
        {
            //************************************************************
            // TODO 10: Used in Faculty/AssignGroupToProject.aspx.cs     *
            // Construct the SQL SELECT statement to retrieve the number *
            // of times a given group code prefix has been used.         *
            // A group code prefix is the group code minus its trailing  *
            // integer (e.g., for group code "FL1" the prefix is "FL").  *
            //************************************************************
            sql = "select count(*) from ProjectGroup where substr(groupCode,0,(Length(groupCode)-1))='"+groupCodePrefix+"'";
            return myOracleDBAccess.GetAggregateValue(sql);
        }

        public DataTable GetGroupsAvailableToAssign(string fypId)
        {
            //********************************************************************************
            // TODO 11: Used in Faculty/AssignGroupToProject.aspx.cs                         *
            // Construct the SQL SELECT statement to retrieve the group id and priority, as  *
            // well as the name and username of the students in the group, for those groups  *
            // that are available for assignment and that have have indicated an interest in *
            // a project identified by its fyp id where the project is available. Order the  *
            // result first by group id ascending and then by student name ascending.        *
            //********************************************************************************
            sql = "select i.groupid,i.fypPriority,s.studentName,s.username from "+
            "Students s, InterestedIn i, ProjectGroup p,FYProject f where "+
            "s.groupId=p.groupId and i.groupId=p.groupId and i.fypId=f.fypId and i.fypId="+fypId+
            " and p.fypAssigned is null and (select isAvailable from FYProject "+
            "where fypId="+fypId+")='Y' order by i.groupId asc,s.studentName asc";
            return myOracleDBAccess.GetData(sql);
        }

        public DataTable GetGroupsCurrentlyAssigned(string fypId)
        {
            //**********************************************************************************
            // TODO 12: Used in Faculty/AssignGroupToProject.aspx.cs                           *
            // Construct the SQL SELECT statement to retrieve the group id and group code, as  *
            // well as the name of the students in the group, for those groups that have been  *
            // assigned to a project identified by its fyp id. Order the result first by group *
            // id ascending and then by student name ascending.                                *
            //**********************************************************************************
            sql = "select p.groupId,p.groupCode,s.StudentName from Students s,ProjectGroup p"+
            " where s.groupId=p.groupId and p.fypAssigned="+fypId+" order by p.groupId asc,s.studentName asc";
            return myOracleDBAccess.GetData(sql);
        }

        public DataTable GetFacultyProjects(string username)
        {
            //************************************************************
            // TODO 13: Used in Faculty/AssignGroupToProject.aspx.cs     *
            // Construct the SQL SELECT statement to retrieve the id and *
            // title of the projects supervised by a faculty identified  *
            // by his/her username. Order the result by title ascending. *
            //************************************************************
            sql = "select f.fypId,f.title from FYProject f,Supervises s where "+
            "s.fypId=f.fypId and s.username='"+username+"' order by f.title asc";
            return myOracleDBAccess.GetData(sql);
        }

        public DataTable GetProjectAvailability(string fypId)
        {
            //********************************************************
            // TODO 14: Used in Faculty/AssignGroupToProject.aspx.cs *
            // Construct the SQL SELECT statement to retrieve the    *
            // availability of a project identified by its fyp id.   *
            //********************************************************
            sql = "select isAvailable from FYProject where fypId="+fypId;
            return myOracleDBAccess.GetData(sql);
        }

        public bool AssignGroupToProject(string groupCode, string fypId, string groupId)
        {
            //*****************************************************************************
            // TODO 15: Used in Faculty/AssignGroupToProject.aspx.cs                      *
            // Construct the SQL UPDATE statement to assign a project group to a project. *
            //*****************************************************************************
            sql = "update ProjectGroup set groupCode='"+groupCode+"',fypAssigned="+fypId+
            " where groupId="+groupId;
            return SetData(sql);
        }

        public decimal GetNumberOfGroupsSupervised(string username)
        {
            //***********************************************************************************
            // TODO 16: Used in Faculty/AssignGroupToProject.aspx.cs                            *
            // Construct the SQL SELECT statement to retrieve the total number of groups that a * 
            // faculty, identified by his/her username, has assigned to all of his/her projects.*
            //***********************************************************************************
            sql = "select count(p.groupId) from ProjectGroup p,Supervises s "+
            "where p.fypAssigned=s.fypId and s.username='"+username+"'";
            return myOracleDBAccess.GetAggregateValue(sql);
        }

        public bool CreateFYProject(string fypId, string title, string fypDescription, string fypCategory,
            string fypType, string requirement, string minStudents, string maxStudents, string isAvailable,
            string supervisor, string cosupervisor)
        {
            // First, create an Oracle transaction.
            OracleTransaction trans = myOracleDBAccess.BeginTransaction();
            if (trans == null) { return false; }

            // Second, create the project record.
            //*******************************************************************
            // TODO 17: Used in Faculty/CreateProject.aspx.cs                   *
            // Construct the SQL INSERT statement to insert a FYProject record. *
            //*******************************************************************
            sql = "insert into FYProject values("+fypId+",'"+title+"','"+fypDescription+"','"+fypCategory+"','"
            +fypType+"','"+requirement+"',"+minStudents+","+maxStudents+",'"+isAvailable+"')";
            if (!myOracleDBAccess.SetData(sql, trans)) { myOracleDBAccess.DisposeTransaction(trans); return false; }

            // Third, create the Supervises record for the supervisor.
            //***************
            // Uses TODO 18 *
            //***************
            if (!CreateSupervises(supervisor, fypId, trans)) { myOracleDBAccess.DisposeTransaction(trans); return false; }
            {
                // Create the Supervises record for the cosupervisor, if any.
                if (cosupervisor != "")
                {
                    if (!CreateSupervises(cosupervisor, fypId, trans)) { myOracleDBAccess.DisposeTransaction(trans); return false; }
                }
            }
            myOracleDBAccess.CommitTransaction(trans);
            return true;
        }

        public bool CreateSupervises(string username, string fypId, OracleTransaction trans)
        {
            if (trans == null)
            {
                trans = myOracleDBAccess.BeginTransaction();
                if (trans == null) { return false; }
            }
            //********************************************************************
            // TODO 18: Used in App_Code/FYPMSDB.CreateFYProject                 *
            // Construct the SQL INSERT statement to insert a Supervises record. *
            //********************************************************************
            sql = "Insert into Supervises values('"+username+"',"+fypId+")";
            return myOracleDBAccess.SetData(sql, trans);
        }

        public DataTable GetSupervisorProjectDigest(string username)
        {
            //*******************************************************************************
            // TODO 19: Used in Faculty/DisplayProjects.aspx.cs                             *
            // Construct the SQL SELECT statement to retrieve the fyp id, title, category,  *
            // type, mimimum students and maximum students of the projects supervised by a  *
            // faculty identified by his/her username. Order the result by title ascending. *
            //*******************************************************************************
            sql = "select fp.fypId,fp.title,fp.fypcategory,fp.fypType,fp.minStudents,fp.maxStudents "+
            "from FYProject fp, Supervises s where s.fypId=fp.fypId "+
            "and s.username='"+username+"' order by fp.title asc";
            return myOracleDBAccess.GetData(sql);
        }

        public DataTable GetInterestedInProject(string fypId)
        {
            //**********************************************************************
            // TODO 20: Used in Faculty/EditProject.aspx.cs                        *
            // Construct the SQL SELECT statement to retrieve all the attributes   * 
            // from the InterestedIn table for a project identified by its fyp id. *
            //**********************************************************************
            sql = "select * from InterestedIn where fypId="+fypId;
            return myOracleDBAccess.GetData(sql);
        }

        public DataTable GetCosupervisorInfoForEdit(string fypId, string username)
        {
            //**************************************************************************************
            // TODO 21: Used in Faculty/EditProject.aspx.cs                                        *
            // Construct the SQL SELECT statement to retrieve the username of the cosupervisor, if *
            // any, of a project, identified by its fyp id, given the username of one supervisor.  *
            //**************************************************************************************
            sql = "select distinct s2.username from Supervises s1,Supervises s2 where s1.fypId=s2.fypId "+
            "and s1.username!=s2.username and s1.username='"+username+"' and s1.fypId="+fypId;
            return myOracleDBAccess.GetData(sql);
        }

        public bool UpdateFYProject(string fypId, string title, string fypDescription, string fypCategory,
            string fypType, string requirement, string minStudents, string maxStudents, string isAvailable,
            string oldCosupervisor, string newCosupervisor)
        {
            // First, create an Oracle transaction.
            OracleTransaction trans = myOracleDBAccess.BeginTransaction();
            if (trans == null) { return false; }

            // Second, update the FYP project values.
            //***********************************************************************************
            // TODO 22: Used in Faculty/EditProject.aspx.cs                                     *
            // Construct the SQL UPDATE statement to update all the values of a FYProject table *
            // record with the corresponding parameter value for the given project fyp id.      *
            //***********************************************************************************
            sql = "update FYProject set title='"+title+"',fypDescription='"+fypDescription+"',fypCategory='"+
            fypCategory+"',fypType='"+fypType+"',requirement='"+requirement+"',minStudents="+minStudents+",maxStudents="+maxStudents+
            ",isAvailable='"+isAvailable+"' where fypId="+fypId;
            if (!myOracleDBAccess.SetData(sql, trans)) { myOracleDBAccess.DisposeTransaction(trans); return false; }

            // Finally, update the cosupervisor, if necessary.
            if (oldCosupervisor != newCosupervisor)
            {
                if (oldCosupervisor != "")
                {
                    // Delete the old cosupervsior for the project from the Supervises table.
                    //***************
                    // Uses TODO 23 *
                    //***************
                    if (!DeleteSupervises(oldCosupervisor, fypId, trans)) { myOracleDBAccess.DisposeTransaction(trans); return false; }
                }
                if (newCosupervisor != "")
                {
                    // Insert a new cosupervisor for the project into the Supervises table.
                    //***************
                    // Uses TODO 18 *
                    //***************
                    if (!CreateSupervises(newCosupervisor, fypId, trans)) { myOracleDBAccess.DisposeTransaction(trans); return false; }
                }
            }
            myOracleDBAccess.CommitTransaction(trans);
            return true;
        }

        public bool DeleteSupervises(string username, string fypId, OracleTransaction trans)
        {
            //**************************************************************
            // TODO 23: Used in App_Code/FYPMSDB.UpdateFYProject           *
            // Construct the SQL DELETE statement to delete the Supervises *
            // record identified by an fyp id and a username.              *
            //**************************************************************
            sql = "delete from Supervises where username='"+username+"' and fypId="+fypId;
            return myOracleDBAccess.SetData(sql, trans);
        }

        public bool CreateInterestedIn(string fypId, string groupId, string fypPriority)
        {
            //*************************************************************************
            // TODO 24: Used in Student/AvailableProjects.aspx.cs                     *
            // Construct the SQL INSERT statement to add a record to the InterestedIn *
            // table with the specified fyp id, group id and priority values.         *
            //*************************************************************************
            sql = "insert into InterestedIn values("+fypId+","+groupId+","+fypPriority+")";
            return SetData(sql);
        }

        public DataTable GetProjectAssignedToGroup(string groupId)
        {
            //***************************************************************
            // TODO 25: Used in Student/AvailableProjects.aspx.cs           *
            // Construct the SQL SELECT statement to retrieve the title of  *
            // the project assigned to the group identified by its groupId. *
            // Order the result by title ascending.                         * 
            //***************************************************************
            sql = "select f.title from FYProject f, ProjectGroup p where "+
            "p.fypAssigned=f.fypId and p.groupId="+groupId;
            return myOracleDBAccess.GetData(sql);
        }

        public bool AddStudentToGroup(string groupId, string username)
        {
            //***********************************************************************
            // TODO 26: Used in Student/ManageProjectGroup.aspx.cs                  *
            // Construct the SQL UPDATE statement to assign a groupId to a student. *
            //***********************************************************************
            sql = "Update Students set groupId="+groupId+" where username='"+username+"'";
            return SetData(sql);
        }

        public DataTable GetStudentRecord(string username)
        {
            //****************************************************************
            // TODO 27: Used in Student/ManageProjectGroup.aspx.cs           *
            // Construct the SQL SELECT statement to retrieve all of the     *
            // attribute values of a student identified by his/her username. *
            //****************************************************************
            sql = "select * from Students where username='"+username+"'";
            return myOracleDBAccess.GetData(sql);
        }

        public bool RemoveGroupMember(string username)
        {
            //*********************************************************
            // TODO 28: Used in Student/ManageProjectGroup.aspx.cs    *
            // Construct the SQL UPDATE statement to remove a student *
            // identified by his/her username from a project group.   *
            //*********************************************************
            sql = "update Students set groupId=NULL where username='"+username+"'";
            return SetData(sql);
        }

        public bool DeleteProjectGroup(string groupId)
        {
            //******************************************************
            // TODO 29: Used in Student/ManageProjectGroup.aspx.cs *
            // Construct the SQL DELETE statement to delete a      *
            // project group identified by its groupId.            *
            //******************************************************
            sql = "delete from ProjectGroup where groupId="+groupId;
            return SetData(sql);
        }

        public bool CreateProjectGroup(string groupId)
        {
            //*********************************************************************************
            // TODO 30: Used in Student/ManageProjectGroup.aspx.cs                            *
            // Construct the SQL INSERT statement to add a project group with the specified   *
            // groupId to the ProjectGroup table. All other attributes values should be null. *
            //*********************************************************************************
            sql = "insert into ProjectGroup values("+groupId+",NULL,NULL,NULL)";
            return SetData(sql);
        }

        public DataTable GetAssignedProjectInformatione(string username)
        {
            //*****************************************************************************
            // TODO 31: Used in Student/ViewGrades.aspx.cs                                *
            // Construct the SQL SELECT statement to retrieve the fyp id and title of the *
            // project to which a student, identified by his/her username, is assigned.   *
            //*****************************************************************************
            sql = "select f.fypId,f.title from FYProject f,ProjectGroup p,Students s "+
                "where s.groupId=p.groupId and p.fypAssigned=f.fypId and s.username='"+username+"'";
            return myOracleDBAccess.GetData(sql);
        }

        public DataTable GetStudentGrades(string username)
        {
            //****************************************************************************
            // TODO 32: Used in Student/ViewGrades.aspx.cs                               *
            // Construct the SQL SELECT statement to retrieve the faculty name, as well  *
            // as the proposal, progress, final and presentation grades, given by the    *
            // supervisor and the reader for the student identified by his/her username. *
            //****************************************************************************
            sql = "select f.facultyName,r.proposalGrade,r.progressGrade,r.finalGrade,r.PresentationGrade "+
            "from Faculty f,Requirement r where f.username=r.facultyusername and "+
            "r.studentUsername='"+username+"'";
            return myOracleDBAccess.GetData(sql);
        }

        /***********************************************/
        /***** Methods used in App_Code/Helpers.cs *****/
        /***********************************************/

        public bool CreateRequirement(string facultyUsername, string studentUsername, string proposalGrade,
            string progressGrade, string finalGrade, string presentationGrade)
        {
            //******************************************************************
            // TODO 33: Used in CreateRequirementRecord in App_Code/Helpers.cs *
            // Construct the SQL INSERT statement to insert a value for each   *
            // attribute of the Requirement table whose values are specified   *
            // by the corresponding parameter of this method.                  *
            //******************************************************************
            sql = "insert into Requirement values('"+facultyUsername+"','"+studentUsername+"',"+
            proposalGrade+","+progressGrade+","+finalGrade+","+presentationGrade+")";
            return SetData(sql);
        }

        public DataTable GetAssignedFypId(string groupId)
        {
            //***************************************************************************
            // TODO 34: Used in IsGroupAssigned in App_Code/Helpers.cs                  *
            // Construct the SQL SELECT statement to retrieve the fyp id of the project *
            // to which a project group, identified by its group id, has been assigned. *
            //***************************************************************************
            sql = "select fypAssigned from ProjectGroup where groupId="+groupId;
            return myOracleDBAccess.GetData(sql);
        }

        public DataTable GetFaculty()
        {
            //*****************************************************
            // TODO 35: Used in GetFaculty in App_Code/Helpers.cs *
            // Construct the SQL SELECT statement to retrieve     *
            // the username and name of all faculty.              *
            //*****************************************************
            sql = "select username,facultyName from Faculty";
            return myOracleDBAccess.GetData(sql);
        }

        public DataTable GetGroupAvailableProjectDigests(string groupId)
        {
            //**************************************************************************************
            // TODO 36: Used in GetGroupAvailableProjectDigests in App_Code/Helpers.cs             *
            // Construct the SQL SELECT statement to retrieve the fyp id, title, category, type,   *
            // minimum students and maximum students of the projects for which a group, identified *
            // by its group id, can indicate an interest. Only those projects that are available,  *
            // meet the group's size requirements and for which a group has not alredy indicated   *
            // an interest should be retrieved. Groups that have been assigned to a project cannot *
            // indicate an interest in any project. Order the result by title ascending.           *                                       *
            //**************************************************************************************
            sql = "select distinct fp.fypId,fp.title,fp.fypCategory,fp.fypType,fp.minStudents,fp.maxStudents "+
            "from FYProject fp where fp.isAvailable='Y' and fp.fypId NOT in "+
            "(select fypId from InterestedIn where groupId="+groupId+") and "+  //hasn't indicated interest
            "(select count(*) from Students where groupId="+groupId+") between fp.minStudents and fp.maxStudents "+
            "order by fp.title asc";
            return myOracleDBAccess.GetData(sql);
        }

        public DataTable GetProjectGroupMembers(string groupId)
        {
            //****************************************************************************
            // TODO 37: Used in GetProjectGroupMembers in App_Code/Helpers.cs            *
            // Construct the SQL SELECT statement to retrieve the value of all the       *
            // attributes of the students in a project group identified by its group id. *
            //****************************************************************************
            sql = "select * from Students where groupId="+groupId;
            return myOracleDBAccess.GetData(sql);
        }

        public DataTable GetStudentGroupId(string username)
        {
            //************************************************************
            // TODO 38: Used in GetStudentGroupId in App_Code/Helpers.cs *
            // Construct the SQL SELECT statement to retrieve the        *
            // group id for the student identified by his/her username.  *
            //************************************************************
            sql = "select GroupId from Students where username='"+username+"'";
            return myOracleDBAccess.GetData(sql);
        }

        public DataTable GetSupervisors(string fypId)
        {
            //**************************************************************************
            // TODO 39: Used in GetProjectSupervisors in App_Code/Helpers.cs           *
            // Construct the SQL SELECT statement to retrieve the username and faculty *
            // name of all the supervisors of a project identified by its fyp id.      *
            //**************************************************************************
            sql = "select f.username,f.facultyName from Faculty f,Supervises s "+
            "where f.username=s.username and s.fypId="+fypId;
            return myOracleDBAccess.GetData(sql);
        }

        public DataTable GetProjectsGroupInterestedIn(string groupId)
        {
            //*********************************************************************************
            // TODO 40: Used in GetProjectsGroupInterestedIn in App_Code/Helpers.cs           *
            // Construct the SQL SELECT statement to retrieve the fyp id, title, category,    *
            // type and priority of the projects for which a group, identified by its group   *
            // id, HAS ALREADY indicated an interest. Order the result by priority ascending. *
            //*********************************************************************************
            sql = "select f.fypId,f.title,f.fypcategory,f.fyptype,i.fypPriority from "+
            "InterestedIn i, FYProject f where f.fypId=i.fypId and i.groupId="+groupId+
            " order by i.fypPriority asc";
            return myOracleDBAccess.GetData(sql);
        }

        /*---------------------------------END OF TODOS---------------------------------*/

        #region *** DO NOT CHANGE THE METHODS BELOW THIS LINE. THEY ARE NOT TODOS!!! ***!

        public DataTable GetProjectCategories()
        {
            sql = "select * from ProjectCategory"; // ALL TESTED
            return myOracleDBAccess.GetData(sql);
        }

        public DataTable GetProjectDetails(string fypId)
        {
            sql = "select * from FYProject where fypId=" + fypId;
            return myOracleDBAccess.GetData(sql);
        }

        public DataTable GetProjectDigests()
        {
            sql = "select fypId, title, fypCategory, fypType, minStudents, maxStudents, isAvailable" +
                " from FYProject order by title";
            return myOracleDBAccess.GetData(sql);
        }

        public bool RemoveGroupFromProject(string groupId)
        {
            sql = "update ProjectGroup set fypAssigned=null where groupId=" + groupId;
            return SetData(sql);
        }

        public bool RemoveReader(string groupId)
        {
            sql = "update ProjectGroup set reader=null where groupId=" + groupId;
            return SetData(sql);
        }

        public bool SetData(string sql)
        {
            OracleTransaction trans = myOracleDBAccess.BeginTransaction();
            if (trans == null) { return false; }
            if (myOracleDBAccess.SetData(sql, trans))
            { myOracleDBAccess.CommitTransaction(trans); return true; } // The insert/update/delete succeeded.
            else
            { myOracleDBAccess.DisposeTransaction(trans); return false; } // The insert/update/delete failed.
        }
        #endregion
    }
}