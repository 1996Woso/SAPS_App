using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SAPS_App.Models
{
    public class CaseManager
    {
        [Key]
        [DisplayName("Manager Id")]
        public int CaseManagerNo {  get; set; }
        public string Name { get; set; }   
        public string Surname { get; set; }
        public string Email { get; set; }
        public string CaseManagerId { get; set; }
        [DisplayName("Total Cases")]
        public int TotalCases
        {
            get
            {
                return CriminalRecords?.Count ?? 0;

            }
        }
        public List<CriminalRecord> CriminalRecords { get; set; }
    }
}
