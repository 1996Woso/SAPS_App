using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SAPS_App.Models
{
    public class SuspectDetailsViewModel
    {
        public Suspect Suspect { get; set; }
        public List<CriminalRecord> CriminalRecords { get; set; }
  
    }
}
