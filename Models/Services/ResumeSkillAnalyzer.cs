
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FreshStart.Services
{
    public class ResumeSkillAnalyzer
    {
        private readonly string[] _knownSkills =
        {
            "C#",
            "ASP.NET Core",
            "ASP.NET Core MVC",
            ".NET",
            "Entity Framework Core",
            "SQL Server",
            "MySQL",
            "Python",
            "Java",
            "JavaScript",
            "HTML",
            "CSS",
            "Bootstrap",
            "jQuery",
            "PHP",
            "React",
            "Angular",
            "Node.js",
            "Git",
            "GitHub",
            "REST API",
            "Web Development",
            "Machine Learning",
            "Data Analysis",
            "Microsoft Office"
        };

        public List<string> ExtractSkills(string resumeText)
        {
            var foundSkills = new List<string>();

            if (string.IsNullOrWhiteSpace(resumeText))
            {
                return foundSkills;
            }

            foreach (var skill in _knownSkills)
            {
                var pattern = @"(?<![\w+#.])"
                    + Regex.Escape(skill)
                    + @"(?![\w+#.])";

                if (Regex.IsMatch(
                    resumeText,
                    pattern,
                    RegexOptions.IgnoreCase))
                {
                    foundSkills.Add(skill);
                }
            }

            return foundSkills;
        }
    }
}
