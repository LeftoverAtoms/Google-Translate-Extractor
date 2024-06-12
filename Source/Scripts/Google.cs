using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace GTE
{
    public static class Google
    {
        private static readonly HttpClient m_client = new HttpClient();

        public static async Task<Stream> Request(string URL)
        {
            var response = await m_client.GetAsync(URL);
            var stream = await response.Content.ReadAsStreamAsync();
            return stream;
        }
    }
}