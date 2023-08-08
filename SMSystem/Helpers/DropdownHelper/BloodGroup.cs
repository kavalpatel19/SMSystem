using System.ComponentModel.DataAnnotations;

namespace SMSystem.Helpers.DropdownHelper
{
    public enum BloodGroup
    {
        [Display(Name = "O+")]
        OPositive,
        [Display(Name = "A+")]
        APositive,
        [Display(Name = "B+")]
        BPositive,
        [Display(Name = "AB+")]
        ABPositive,
        [Display(Name = "AB-")]
        ABNegative,
        [Display(Name = "A-")]
        ANegative,
        [Display(Name = "B-")]
        BNegative,
        [Display(Name = "O-")]
        ONegative
    }
}
