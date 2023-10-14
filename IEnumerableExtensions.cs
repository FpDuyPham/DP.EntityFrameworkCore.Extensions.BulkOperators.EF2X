using DP.EntityFrameworkCore.Extensions.BulkOperators.EF2X.Resolvers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace DP.EntityFrameworkCore.Extensions.BulkOperators.EF2X
{
    public static class IEnumerableExtensions
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> items, bool exceptVirtualMethod = true) where T : class, new()
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props;
            if (typeof(IBulkPropertyResolver).IsAssignableFrom(typeof(T)))
            {
                var fields = (new T() as IBulkPropertyResolver).GetFields();
                Props = typeof(T).GetProperties()
                    .Where(p => fields.Contains(p.Name))
                    .OrderBy(p => Array.IndexOf(fields, p.Name))
                    .ToArray();
            }
            else
            {
                Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                if (exceptVirtualMethod)
                    Props = Props.Where(p => p.GetGetMethod() == null || p.GetGetMethod().IsVirtual != true).ToArray();
            }

            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType;
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }

            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }

                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}
