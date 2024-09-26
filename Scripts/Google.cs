using Extensions;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace GTE;

public static class Google
{
    private static readonly HttpClient s_client;

    static Google()
    {
        s_client = new HttpClient();
    }

    public static async Task<Stream> Request(string language, string content)
    {
        string code = Language.GetCode(language);
        if (code == null)
        {
            return null;
        }

        string url = string.Format(@"https://translate.google.com/translate_tts?tl={0}&q={1}&client=tw-ob", code, content);

        var response = await s_client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            ConsoleColor.Red.WriteLine($"Google.Request({language}) -> Status: {response.StatusCode} -> Language may not support TTS");
            return null;
        }

        return await response.Content.ReadAsStreamAsync();
    }

    public static async Task Download(string language, string content, string filePath)
    {
        var stream = await Request(language, content);
        if (stream == null)
        {
            return;
        }

        Program.Write(stream, filePath);
    }
}
