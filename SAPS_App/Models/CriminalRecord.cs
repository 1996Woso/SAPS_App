using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace SAPS_App.Models
{
	public class CriminalRecord
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[DisplayName("Offence Commited")]
		public string OffenceCommited { get; set; }
		[Required]
		[StringLength(3, MinimumLength = 1, ErrorMessage = "The Sentence must not have more than 3 digits")]
		public string Sentence { get; set; }
		//[Required]
		//[StringLength(20, MinimumLength = 3, ErrorMessage = "Issued At must have have 3 to 20 characters .")]
		[DisplayName("Issued At")]
		public string IssuedAt { get; set; }
		[StringLength(20, MinimumLength = 3, ErrorMessage = "Issued By must have have 3 to 20 characters .")]
		[DisplayName("Issued By")]
		public string IssuedBy { get; set; }	
		[DisplayName("Issue Date")]
		//[Required]
		public DateTime IssueDate { get; set; }
		//[Required]
		public int SuspectNumber { get; set; }
		public Suspect Suspect { get; set; }

        public int CaseManagerNo { get; set; }
		public string CaseManagerId { get; set; }
		public string CaseManagerName { get; set; }	
        public CaseManager CaseManager { get; set; }
    }
}
