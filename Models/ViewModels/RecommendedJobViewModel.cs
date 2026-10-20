
using System.Collections.Generic;
using JobModel = FreshStart.Models.Job.Job;

namespace FreshStart.Models.ViewModels
{
    public class RecommendedJobViewModel
    {
        public JobModel Job { get; set; } = null!;
        public double MatchPercentage { get; set; }

        public List<string> MatchedTerms { get; set; } = new List<string>();

        public List<string> MissingTerms { get; set; } = new List<string>();
    }
}
