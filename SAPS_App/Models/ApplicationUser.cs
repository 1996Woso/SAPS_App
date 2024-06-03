using Microsoft.AspNetCore.Identity;//This allows ApplicationUser to Inherity from IdentityUser

namespace SAPS_App.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string Name { get; set; }
		public string Surname { get; set; }

        //public virtual ICollection<CriminalRecord> ManagedCriminalRecords { get; set; }
        //public int TotalCases
        //{
        //    get
        //    {
        //        return CriminalRecords?.Count ?? 0;
        //    }
        //}
        //public List<CriminalRecord> CriminalRecords { get; set; }
    }
	
}