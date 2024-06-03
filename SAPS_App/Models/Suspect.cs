using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace SAPS_App.Models
{
	public class Suspect
	{
        [Key]
        [DisplayName("Suspect Number")]
        [Display(Order =1)]
        public int SuspectNumber { get; set; }
        [Required]
        [Display(Name = "Suspect ID",Order =2)]
        [RegularExpression(@"(((\d{2}((0[013578]|1[02])(0[1-9]|[12]\d|3[01])|(0[13456789]|1[012])(0[1-9]|[12]\d|30)|02(0[1-9]|1\d|2[0-8])))|([02468][048]|[13579][26])0229))(( |-)(\d{4})( |-)([01]8((( |-)\d{1})|\d{1}))|(\d{4}[01]8\d{1}))", ErrorMessage = "Invalid ID Number")]
        [StringLength(13)]
        public string SuspectId { get; set; }
		[Required(ErrorMessage = "First name is required.")]
		[Display(Name ="First Name",Order =3)]
		[StringLength(20, MinimumLength = 2, ErrorMessage = "The First Name must be between 2 and 20 characters.")]
		public string FirstName { get; set; }
		[Display(Name="Last Name",Order =4)]
		[Required(ErrorMessage = "Last name name is requires.")]
		[StringLength(20, MinimumLength = 2, ErrorMessage = "The Last Name must be between 2 and 20 characters.")]
		public string LastName { get; set; }
        [Display(Name="Total Criminal Records",Order =5)]
        public int TotalCriminalRecords
        {
            get
            {
                // If CriminalRecords is null, return 0; otherwise, return the count.
                return CriminalRecords?.Count ?? 0;

                //Or use this code

                //if (CriminalRecords != null)
                //{
                //    return CriminalRecords.Count;
                //}
                //else
                //{
                //    return 0;
                //}
            }
        }
        public List<CriminalRecord> CriminalRecords { get; set; }
        //public string ManagerId { get; set; }
        //public ApplicationUser Manager { get; set; }
    }
}
