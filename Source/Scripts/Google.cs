using Extensions;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace GTE
{
    public static class Google
    {
        private static readonly HttpClient m_client = new HttpClient();

        public static async Task<Stream?> Request(string language, string content)
        {
            if (Language.TryGetCode(language, out var code))
            {
                var url = string.Format(@"https://translate.google.com/translate_tts?tl={0}&q={1}&client=tw-ob", code, content);

                var response = await m_client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStreamAsync();
                }
                else
                {
                    ConsoleColor.Red.WriteLine($"Google.Request({language}) -> Status: {response.StatusCode} -> Language may not support TTS");
                }
            }
            return null;
        }
    }
}