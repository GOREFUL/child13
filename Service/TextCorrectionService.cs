using System.IO;
using System.Windows.Controls;

namespace child13.Service
{
    public static class TextCorrectionService
    {
        private static readonly string correctionsFilePath = "Resources/text/correctText.txt";

        public static Task<string> CorrectText(string inputText)
        {
            Dictionary<string, string> corrections = LoadCorrections();

            inputText = RemoveTrailingSpaces(inputText);
            inputText = RemoveLeadingSpaces(inputText);

            foreach (var correction in corrections)
            {
                inputText = inputText.Replace(correction.Key, correction.Value);
            }

            return Task.FromResult(inputText);
        }

        private static string RemoveTrailingSpaces(string inputText)
        {
            var lines = inputText.Split(new[] { "\n" }, StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].TrimEnd();
            }
            return string.Join("\n", lines);
        }

        private static string RemoveLeadingSpaces(string inputText)
        {
            var lines = inputText.Split(new[] { "\n" }, StringSplitOptions.None);
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = lines[i].TrimStart();
            }
            return string.Join("\n", lines);
        }

        private static Dictionary<string, string> LoadCorrections()
        {
            var replacements = new Dictionary<string, string>();

            foreach (var line in File.ReadLines(correctionsFilePath))
            {
                var parts = line.Split(new[] { " - " }, StringSplitOptions.None);
                if (parts.Length == 2)
                {
                    string key = parts[0].Trim();
                    string value = parts[1].Trim();

                    if (key == "\\n") key = "\n";
                    if (key == "\\r") key = "\r";
                    if (string.IsNullOrEmpty(value)) value = " ";

                    replacements[key] = value;
                }
            }
            return replacements;
        }
        private static void TextToTextBox(string filePath, TextBlock textBlock)
        {
            textBlock.Text = File.ReadAllText(filePath);
        }
        private static void WriteTextToFile(string filePath, string text)
        {
            File.WriteAllText(filePath, text);
        }

        public static async Task ProcessTextAsync(TextBlock textBlock)
        {
            string _text = File.ReadAllText(@"./Resources/text/output.txt");
            string correctedText = await CorrectText(_text);
            WriteTextToFile(@"./Resources/text/output.txt", correctedText);
            TextToTextBox(@"./Resources/text/output.txt", textBlock);
        }

        public static async Task ProcessOutputTextAsync(TextBlock textBlock, string outputText)
        {
            string correctedText = await CorrectText(outputText);
            textBlock.Text = correctedText;
        }
    }
}
