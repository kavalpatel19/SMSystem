using System.Data;
using System.Reflection;

namespace SMSystem.Helpers
{
    public class ConvertDataTable
    {
        // This method is used to convert LIST to DATATABLE for EXPORT the data list.
        //
        //
        public static DataTable Convert<T>(List<T> data)
        {
            var dt = new DataTable(typeof(T).Name);
            PropertyInfo[] propinfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in propinfo)
            {
                dt.Columns.Add(prop.Name);
            }
            foreach (T item in data)
            {
                var values = new Object[propinfo.Length];
                for (int i = 0; i < propinfo.Length; i++)
                {
                    values[i] = propinfo[i].GetValue(item, null);
                }
                dt.Rows.Add(values);
            }
            return dt;
        }
    }
}
