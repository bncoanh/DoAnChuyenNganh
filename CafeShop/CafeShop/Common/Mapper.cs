using Microsoft.Data.SqlClient;
using System.Reflection;

namespace CafeShop.Common
{
    public static class Mapper
    {
        public static T MapToSingle<T>(this SqlDataReader dr) where T : new()
        {
            T RetVal = new T();
            var Entity = typeof(T);
            var PropDict = new Dictionary<string, PropertyInfo>();

            string columnName = "";
            try
            {
                if (dr != null && dr.HasRows)
                {
                    var Props = Entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    PropDict = Props.ToDictionary(p => p.Name.ToUpper(), p => p);
                    dr.Read();
                    for (int Index = 0; Index < dr.FieldCount; Index++)
                    {
                        if (PropDict.ContainsKey(dr.GetName(Index).ToUpper()))
                        {
                            columnName = dr.GetName(Index);
                            var Info = PropDict[dr.GetName(Index).ToUpper()];
                            if ((Info != null) && Info.CanWrite)
                            {
                                var Val = dr.GetValue(Index);
                                Info.SetValue(RetVal, (Val == DBNull.Value) ? null : Val, null);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(columnName + " " + ex.Message);
            }
            return RetVal;
        }

        public static List<T> MapToList<T>(this SqlDataReader dr) where T : new()
        {
            List<T> RetVal = new List<T>();
            var Entity = typeof(T);
            var PropDict = new Dictionary<string, PropertyInfo>();

            string columnName = "";
            try
            {
                if (dr != null && dr.HasRows)
                {
                    RetVal = new List<T>();
                    var Props = Entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    PropDict = Props.ToDictionary(p => p.Name.ToUpper(), p => p);
                    while (dr.Read())
                    {
                        T newObject = new T();
                        for (int Index = 0; Index < dr.FieldCount; Index++)
                        {
                            columnName = dr.GetName(Index);
                            if (PropDict.ContainsKey(dr.GetName(Index).ToUpper()))
                            {
                                var Info = PropDict[dr.GetName(Index).ToUpper()];
                                if ((Info != null) && Info.CanWrite)
                                {
                                    var Val = dr.GetValue(Index);
                                    Info.SetValue(newObject, (Val == DBNull.Value) ? null : Val, null);
                                }
                            }
                        }
                        RetVal.Add(newObject);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + "\n" + columnName);
            }
            return RetVal;
        }
    }
}
