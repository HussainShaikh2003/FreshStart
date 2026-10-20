
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FreshStart.Services
{
    public class JobMatchingService
    {
        private static readonly HashSet<string> StopWords =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "the", "and", "for", "with", "from", "this", "that",
                "are", "you", "your", "our", "will", "can", "has",
                "have", "had", "was", "were", "job", "role", "work",
                "working", "team", "candidate", "candidates", "ability",
                "skills", "experience", "knowledge", "responsibilities",
                "requirements", "required", "preferred", "using", "use",
                "good", "strong", "must", "should", "into", "their",
                "they", "what", "when", "where", "how", "all", "any",
                "but", "also", "looking", "need", "needed", "years",
                "year", "plus", "about", "such", "including", "related",
                "developer", "development"
            };

        public JobMatchResult CalculateMatch(
            string resumeText,
            string profileSkills,
            string jobDescription,
            string requiredSkills)
        {
            string candidateText = Normalize(
                (resumeText ?? "") + " " + (profileSkills ?? ""));

            var descriptionTerms = ExtractTerms(jobDescription);
            var requiredTerms = ExtractTerms(requiredSkills);

            var allTerms = requiredTerms
                .Concat(descriptionTerms)
                .Distinct()
                .ToList();

            var matched = allTerms
                .Where(term => ContainsTerm(candidateText, term))
                .ToList();

            var missing = allTerms
                .Where(term => !ContainsTerm(candidateText, term))
                .ToList();

            double descriptionScore =
                CalculateScore(candidateText, descriptionTerms);

            double requiredScore =
                CalculateScore(candidateText, requiredTerms);

            double percentage;

            if (descriptionTerms.Count > 0 && requiredTerms.Count > 0)
            {
                percentage = (requiredScore * 0.60)
                           + (descriptionScore * 0.40);
            }
            else if (requiredTerms.Count > 0)
            {
                percentage = requiredScore;
            }
            else
            {
                percentage = descriptionScore;
            }

            return new JobMatchResult
            {
                MatchPercentage = Math.Round(percentage, 2),
                MatchedTerms = matched,
                MissingTerms = missing
            };
        }

        private double CalculateScore(
            string candidateText,
            List<string> terms)
        {
            if (terms.Count == 0)
                return 0;

            int matchedCount = terms.Count(
                term => ContainsTerm(candidateText, term));

            return matchedCount * 100.0 / terms.Count;
        }

        private string Normalize(string text)
        {
            text = (text ?? "").ToLowerInvariant();

            text = Regex.Replace(
                text, @"asp\s*\.?\s*net\s*\.?\s*core", " aspnetcore ");

            text = Regex.Replace(
                text, @"(?<![a-z0-9])c\s*#(?![a-z0-9])", " csharp ");

            text = Regex.Replace(
                text, @"(?<![a-z0-9])c\s+sharp(?![a-z0-9])", " csharp ");

            text = Regex.Replace(
                text, @"(?<![a-z0-9])\.net(?![a-z0-9])", " dotnet ");

            text = Regex.Replace(
                text, @"sql\s+server", " sqlserver ");

            text = Regex.Replace(text, @"[^\w]+", " ");
            return Regex.Replace(text, @"\s+", " ").Trim();
        }

        private List<string> ExtractTerms(string text)
        {
            return Normalize(text)
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Where(term =>
                    term.Length > 2 &&
                    !StopWords.Contains(term) &&
                    !Regex.IsMatch(term, @"(.)\1{2,}"))
                .Distinct()
                .ToList();
        }

        private bool ContainsTerm(string text, string term)
        {
            string normalizedText = " " + Normalize(text) + " ";
            string normalizedTerm = Normalize(term);

            if (string.IsNullOrWhiteSpace(normalizedTerm))
                return false;

            return normalizedText.Contains(
                " " + normalizedTerm + " ",
                StringComparison.OrdinalIgnoreCase);
        }
    }

    public class JobMatchResult
    {
        public double MatchPercentage { get; set; }

        public List<string> MatchedTerms { get; set; }
            = new List<string>();

        public List<string> MissingTerms { get; set; }
            = new List<string>();
    }
}
