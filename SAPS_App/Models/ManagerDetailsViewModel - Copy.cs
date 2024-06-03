using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SAPS_App.Models
{
    public class CaseManagerDetailsViewModel
    {
        public CaseManager CaseManager { get; set; }
        public List<Suspect> Suspects { get; set; } 
        public List<CriminalRecord> CriminalRecords { get; set; }
  
    }
}
