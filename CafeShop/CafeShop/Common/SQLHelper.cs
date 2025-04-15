using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.Common;
using System.Reflection;
using System.Data;
using Microsoft.Data.SqlClient;
using CafeShop.Common;

namespace ManagementCourse.Common
{
	public static class SQLHelper<T> where T : class, new()
    {
        static string connectionString = Config.Connection();
        public static T ProcedureToModel(string procedureName, string[] paramName, object[] paramValue)
        {
            T model = new T();
            SqlConnection mySqlConnection = new SqlConnection(connectionString);
            SqlParameter sqlParam;
            mySqlConnection.Open();

            try
            {
                SqlCommand mySqlCommand = new SqlCommand(procedureName, mySqlConnection);
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                if (paramName != null)
                {
                    for (int i = 0; i < paramName.Length; i++)
                    {
                        sqlParam = new SqlParameter(paramName[i], paramValue[i]);
                        mySqlCommand.Parameters.Add(sqlParam);
                    }
                }
                SqlDataReader reader = mySqlCommand.ExecuteReader();
                model = reader.MapToSingle<T>();
            }
            catch (SqlException e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                mySqlConnection.Close();
            }

            return model;
        }
        public static List<T> ProcedureToList( string procedureName, string[] paramName, object[] paramValue)
        {
            List<T> lst = new List<T>();
            SqlConnection mySqlConnection = new SqlConnection(connectionString);
            SqlParameter sqlParam;
            mySqlConnection.Open();

            try
            {
                SqlCommand mySqlCommand = new SqlCommand(procedureName, mySqlConnection);
                mySqlCommand.CommandType = CommandType.StoredProcedure;
                if (paramName != null)
                {
                    for (int i = 0; i < paramName.Length; i++)
                    {
                        sqlParam = new SqlParameter(paramName[i], paramValue[i]);
                        mySqlCommand.Parameters.Add(sqlParam);
                    }
                }
                SqlDataReader reader = mySqlCommand.ExecuteReader();
                lst = reader.MapToList<T>();
            }
            catch (SqlException e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                mySqlConnection.Close();
            }

            return lst;
        }
        public static T SqlToModel(string sql)
        {
            T model = new T();
            SqlConnection mySqlConnection = new SqlConnection(connectionString);
            mySqlConnection.Open();
            try
            {
                SqlCommand mySqlCommand = new SqlCommand(sql, mySqlConnection);
                mySqlCommand.CommandType = CommandType.Text;
                SqlDataReader reader = mySqlCommand.ExecuteReader();
                model = reader.MapToSingle<T>();
            }
            catch (SqlException e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                mySqlConnection.Close();
            }

            return model;
        }
        public static List<T> SqlToList(string sql)
        {
            List<T> lst = new List<T>();
            SqlConnection mySqlConnection = new SqlConnection(connectionString);
            mySqlConnection.Open();
            try
            {
                SqlCommand mySqlCommand = new SqlCommand(sql, mySqlConnection);
                mySqlCommand.CommandType = CommandType.Text;
                SqlDataReader reader = mySqlCommand.ExecuteReader();
                lst = reader.MapToList<T>();
            }
            catch (SqlException e)
            {
                throw new Exception(e.ToString());
            }
            finally
            {
                mySqlConnection.Close();
            }

            return lst;
        }

        public static DataTable GetDataTableSP(string commandText, string[] param, object[] valueParam)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                var dt = new DataTable();
                SqlParameter sqlParam;
                SqlCommand cmd = new SqlCommand(commandText, conn);
                if (param != null)
                {
                    for (int i = 0; i < param.Length; i++)
                    {
                        sqlParam = new SqlParameter(param[i], valueParam[i]);
                        cmd.Parameters.Add(sqlParam);
                    }
                }
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                conn.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
        public static DataSet GetDataSetSP(string commandText, string[] param, object[] valueParam)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                DataSet dataSet = new DataSet();
                SqlParameter sqlParam;
                SqlCommand cmd = new SqlCommand(commandText, conn);
                if (param != null)
                {
                    for (int i = 0; i < param.Length; i++)
                    {
                        sqlParam = new SqlParameter(param[i], valueParam[i]);
                        cmd.Parameters.Add(sqlParam);
                    }
                }

                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dataSet);
                conn.Close();
                return dataSet;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
