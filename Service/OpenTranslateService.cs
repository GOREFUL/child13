using child13.Models;
using Newtonsoft.Json;
using OpenAI.Chat;

namespace child13.Service
{
    public static class OpenTranslateService
    {
        public static string TranslationWithOpenAI(string originalText, string language)
        {
            ChatClient client = new ChatClient(model: "gpt-4o-mini",
                apiKey: api_Trans._api);
            if (language == "ua")
            {
                var request = new
                {
                    prompt = "Just translate the text to Ukrainian",
                    text = originalText
                };
                string jsonRequest = JsonConvert.SerializeObject(request);
                ChatCompletion chat = client.CompleteChat(jsonRequest);
                return chat.Content[0].Text;
            }
            else if (language == "ru")
            {
                var request = new
                {
                    prompt = "Just translate the text to Russian",
                    text = originalText
                };
                string jsonRequest = JsonConvert.SerializeObject(request);
                ChatCompletion chat = client.CompleteChat(jsonRequest);
                return chat.Content[0].Text;
            }
            else if (language == "pl")
            {
                var request = new
                {
                    prompt = "Just translate the text to Polish",
                    text = originalText
                };
                string jsonRequest = JsonConvert.SerializeObject(request);
                ChatCompletion chat = client.CompleteChat(jsonRequest);
                return chat.Content[0].Text;
            }
            else
            {
                return "Language not supported";
            }
        }
    }
}
