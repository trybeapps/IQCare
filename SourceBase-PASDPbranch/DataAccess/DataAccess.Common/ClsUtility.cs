using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.Common
{
    public class ClsUtility
    {
        private static int Pkey;
        public static Hashtable theParams = new Hashtable();

        public enum ObjectEnum
        {
            DataSet,DataTable,DataRow,ExecuteNonQuery
        }  

        public static void Init_Hashtable()
        {
            //theParams.Clear();
            theParams = new Hashtable();
            Pkey = 0;
        }
        public static void AddParameters(string FieldName, SqlDbType FieldType, string FieldValue)
        {
            Pkey = Pkey + 1;
            theParams.Add(Pkey, FieldName);
            Pkey = Pkey + 1;
            theParams.Add(Pkey, FieldType);
            Pkey = Pkey + 1;
            theParams.Add(Pkey, FieldValue);
        }
        /// <summary>
        /// Update: C.Low 27-Jan-2014 : Overload not available for some reason, so adding added additional method to add Paramater Direction
        /// </summary>
        /// <param name="FieldName"></param>
        /// <param name="FieldType"></param>
        /// <param name="FieldValue"></param>
        /// <param name="ParamDirection">nullable value determining direction of output or input</param>
        public static void AddDirectionParameter(string FieldName, SqlDbType FieldType, ParameterDirection? ParamDirection)
        {

            Pkey += 1;
            theParams.Add(Pkey, FieldName);
            Pkey += 1;
            theParams.Add(Pkey, FieldType);
            Pkey += 1;
            theParams.Add(Pkey, ParamDirection);

        }
        
        
    }
}
