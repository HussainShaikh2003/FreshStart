
using FreshStart.Models.Application;

namespace FreshStart.Models.ViewModels
{
    public class ApplicantMatchViewModel
    {
        public JobApplication Application { get; set; }

        public double MatchPercentage { get; set; }

        public List<string> MatchedTerms { get; set; }
            = new List<string>();

        public List<string> MissingTerms { get; set; }
            = new List<string>();
    }
}
