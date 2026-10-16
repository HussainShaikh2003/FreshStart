
using System.Text;
using UglyToad.PdfPig;

namespace FreshStart.Services
{
    public class ResumeTextExtractor
    {
        public string ExtractText(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(
                    "Resume PDF was not found.",
                    filePath);
            }

            var text = new StringBuilder();

            using (var document = PdfDocument.Open(filePath))
            {
                foreach (var page in document.GetPages())
                {
                    text.AppendLine(page.Text);
                }
            }

            return text.ToString();
        }
    }
}
