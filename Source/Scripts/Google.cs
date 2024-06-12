using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace GTE
{
    public static class Google
    {
        private static readonly HttpClient m_client = new HttpClient();

        public static async Task<Stream> Request(string language, string content)
        {
            Language.Table.TryGetKey(language, out var code);
            var url = string.Format(@"https://translate.google.com/translate_tts?tl={0}&q={1}&client=tw-ob", code, content);

            var response = await m_client.GetAsync(url);
            var stream = await response.Content.ReadAsStreamAsync();
            return stream;
        }
    }
}