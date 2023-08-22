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

            for (int i = 0 ; i < propinfo.Length - 6 ; i++)
            {
                dt.Columns.Add(propinfo[i].Name);
            }

            foreach (T item in data)
            {
                var values = new Object[propinfo.Length - 6];
                for (int i = 0; i < propinfo.Length - 6 ; i++)
                {
                    values[i] = propinfo[i].GetValue(item, null);
                }
                dt.Rows.Add(values);
            }
            return dt;
        }
    }
}
