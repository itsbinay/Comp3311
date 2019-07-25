using System;
using Oracle.DataAccess.Client;
using System.Configuration;
using System.Data;

namespace FYPMSWebsite.App_Code
{
    //**********************************************************
    //* THE CODE IN THIS CLASS CANNOT BE MODIFIED OR ADDED TO. *
    //*        Report problems to 3311rep@cse.ust.hk.          *
    //**********************************************************

    public class OracleDBAccess
    {
        // Set the connection string to connect to the Oracle database.
        private OracleConnection myOracleDBConnection = new OracleConnection(ConfigurationManager.ConnectionStrings["FYPMSConnectionString"].ConnectionString);

        // Process a SQL SELECT statement.
        public DataTable GetData(string sql)
        {
            try
            {
                Global.sqlError = "";
                if (sql.Trim() == "")
                {
                    throw new ArgumentException("The SQL statement is empty");
                }

                DataTable dt = new DataTable();
                if (myOracleDBConnection.State != ConnectionState.Open)
                {
                    myOracleDBConnection.Open();
                    OracleDataAdapter da = new OracleDataAdapter(sql, myOracleDBConnection);
                    da.Fill(dt);
                    myOracleDBConnection.Close();
                }
                else
                {
                    OracleDataAdapter da = new OracleDataAdapter(sql, myOracleDBConnection);
                    da.Fill(dt);
                }
                return dt;
            }
            catch (ArgumentException ex)
            {
                Global.sqlError = ex.Message;
            }
            catch (FormatException ex)
            {
                Global.sqlError = ex.Message;
            }
            catch (OracleException ex)
            {
                Global.sqlError = ex.Message;
            }
            catch (StackOverflowException ex)
            {
                Global.sqlError = ex.Message;
            }
            return null;
        }

        // Process an SQL SELECT statement that returns only a single value.
        // Returns 0 if the table is empty or the column has no values.
        // Returns -1 if there is an error in the SELECT statement.
        public decimal GetAggregateValue(string sql)
        {
            try
            {
                Global.sqlError = "";
                if (sql.Trim() == "")
                {
                    throw new ArgumentException("The SQL statement is empty");
                }
                object aggregateValue;
                if (myOracleDBConnection.State != ConnectionState.Open)
                {
                    myOracleDBConnection.Open();
                    OracleCommand SQLCmd = new OracleCommand(sql, myOracleDBConnection);
                    SQLCmd.CommandType = CommandType.Text;
                    aggregateValue = SQLCmd.ExecuteScalar();
                    myOracleDBConnection.Close();
                }
                else
                {
                    OracleCommand SQLCmd = new OracleCommand(sql, myOracleDBConnection);
                    SQLCmd.CommandType = CommandType.Text;
                    aggregateValue = SQLCmd.ExecuteScalar();
                }
                return (DBNull.Value == aggregateValue ? 0 : Convert.ToDecimal(aggregateValue));
            }
            catch (ArgumentException ex)
            {
                Global.sqlError = ex.Message;
            }
            catch (FormatException ex)
            {
                Global.sqlError = ex.Message;
            }
            catch (OracleException ex)
            {
                Global.sqlError = ex.Message;
            }
            catch (StackOverflowException ex)
            {
                Global.sqlError = ex.Message;
            }
            return -1;
        }

        // Process SQL INSERT, UPDATE and DELETE statements.
        public bool SetData(string sql, OracleTransaction trans)
        {
            try
            {
                Global.sqlError = "";
                if (sql.Trim() == "")
                {
                    throw new ArgumentException("The SQL statement is empty");
                }

                OracleCommand SQLCmd = new OracleCommand(sql, myOracleDBConnection);
                SQLCmd.Transaction = trans;
                SQLCmd.CommandType = CommandType.Text;
                SQLCmd.ExecuteNonQuery();
                return true;
            }
            catch (ArgumentException ex)
            {
                myOracleDBConnection.Close();
                Global.sqlError = ex.Message;
                return false;
            }
            catch (FormatException ex)
            {
                myOracleDBConnection.Close();
                Global.sqlError = ex.Message;
                return false;
            }
            catch (ApplicationException ex)
            {
                myOracleDBConnection.Close();
                Global.sqlError = ex.Message;
                return false;
            }
            catch (OracleException ex)
            {
                myOracleDBConnection.Close();
                Global.sqlError = ex.Message;
                return false;
            }
            catch (InvalidOperationException ex)
            {
                myOracleDBConnection.Close();
                Global.sqlError = ex.Message;
                return false;
            }
        }

        public OracleTransaction BeginTransaction()
        {
            try
            {
                if (myOracleDBConnection.State != ConnectionState.Open)
                {
                    myOracleDBConnection.Open();
                    OracleTransaction trans = myOracleDBConnection.BeginTransaction();
                    return trans;
                }
                else
                {
                    OracleTransaction trans = myOracleDBConnection.BeginTransaction();
                    return trans;
                }
            }
            catch (InvalidOperationException ex)
            {
                Global.sqlError = ex.Message;
                return null;
            }
        }

        public void CommitTransaction(OracleTransaction trans)
        {
            try
            {
                if (myOracleDBConnection.State == ConnectionState.Open)
                {
                    trans.Commit();
                    myOracleDBConnection.Close();
                }
            }
            catch (ApplicationException ex)
            {
                Global.sqlError = ex.Message;
            }
            catch (FormatException ex)
            {
                Global.sqlError = ex.Message;
            }
            catch (OracleException ex)
            {
                Global.sqlError = ex.Message;
            }
            catch (InvalidOperationException ex)
            {
                Global.sqlError = ex.Message;
            }
        }

        public void DisposeTransaction(OracleTransaction trans)
        {
            try
            {
                if (myOracleDBConnection.State == ConnectionState.Open)
                {
                    trans.Dispose();
                    myOracleDBConnection.Close();
                }
            }
            catch (ApplicationException ex)
            {
                Global.sqlError = ex.Message;
            }
            catch (FormatException ex)
            {
                Global.sqlError = ex.Message;
            }
            catch (OracleException ex)
            {
                Global.sqlError = ex.Message;
            }
            catch (InvalidOperationException ex)
            {
                Global.sqlError = ex.Message;
            }
        }
    }
}