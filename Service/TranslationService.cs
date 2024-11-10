using child13.Models;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Text;
using Tesseract;

namespace child13.Service
{
    public static class TranslationService
    {
        private static readonly HttpClient _client = new HttpClient();

        public static string TranslateText()
        {
            using (var img = Pix.LoadFromFile($"Resources/img/image.png"))
            {
                using (var engine = new TesseractEngine(@"Resources/tessdata_fast", "eng", EngineMode.Default))
                {
                    var customConfig = @"--psm 7 --oem 3";
                    using (var page = engine.Process(img, customConfig))
                    {
                        var text = page.GetText(); File.WriteAllText(@"Resources/text/output.txt", text);
                        string _text = File.ReadAllText(@"Resources/text/output.txt");

                        return _text;
                    }
                }
            }
        }

        public static async Task<string> GetTranslationAsync(string _connection, string _message,
            string _sourceName, string _originalText, string _language)
        {
            try
            {
                var request = new TranslationResponse
                {
                    sourceName = _sourceName,
                    originalText = _originalText,
                    language = _language
                };
                var json = JsonConvert.SerializeObject(request);
                var context = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(_connection + "/getTranslations", context);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    if (_message != null)
                    {
                        string translatedText = OpenTranslateService.TranslationWithOpenAI(_originalText, _language);

                        DateTime dateTime = DateTime.Now;
                        var translation = new TranslationRequest
                        {
                            sourceName = _sourceName,
                            originalText = _originalText,
                            translatedText = translatedText,
                            language = _language,
                            createdAT = dateTime.ToString("yyyy-MM-dd HH:mm")
                        };
                        await SaveTranslationAsync(translation, _connection, _message);
                        return translatedText;
                    }
                    else
                    {
                        return "API key not found";
                    }
                }
                else
                {
                    return "Failed to get translation";
                }
            }
            catch (Exception ex)
            {
                return $"An error occurred: {ex.Message}";
                throw;
            }
        }

        private static async Task SaveTranslationAsync(TranslationRequest translation,
            string _connection, string _message)
        {
            var jsonContent = JsonConvert.SerializeObject(translation);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(_connection + $"/translations?access_key={_message}", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed to save translation: " + response.ReasonPhrase);
            }
        }
    }

}
