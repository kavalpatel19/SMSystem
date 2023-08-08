using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SMSystem.Helpers.DropdownHelper
{
    public class EnumHelper
    {
        // This method is used to return enum list to view.
        //
        //
        public static IEnumerable<SelectListItem> GetEnumSelectList<T>()
        {
            var enumList = Enum.GetValues(typeof(T)).Cast<T>().Select(p => new SelectListItem() { Text = p.ToString(), Value = p.ToString() });

            return enumList;
        }
    }
}
