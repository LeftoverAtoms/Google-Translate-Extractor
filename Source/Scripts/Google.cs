namespace GTE
{
	public static class Google
	{
		public static int Downloads { get; private set; }
		public static long WaitTime { get; private set; }

        static readonly HttpClient Client = new HttpClient();
        static readonly Stopwatch Stopwatch = new Stopwatch();

        public static void Download(Sequence sequence, Locale locale)
		{
			Google.Downloads += 1;

			string path = $"Sounds/{locale.Language.Name}/{sequence.Name}/";

			foreach (Dialogue dialogue in sequence.Dialogue)
			{
				string? url = URL.Format(dialogue.Subtitle, locale.Language.Name);
				if (url == null)
				{
					return;
				}

				// Request to download dialogue audio.
				Task<Stream> task = Task.Run(async () => await Google.Request(url));

				string meta = Sound.Format(dialogue.Guid);

				// Write audio files to disk.
				Program.Write(task.Result, path, $"{dialogue.Name}.mp3");
				Program.Write(meta, path, $"{dialogue.Name}.meta");
			}

			// Write asset file to disk.
			string asset = Sequence.Format(sequence, locale.Language.GetID());
			Program.Write(asset, path, $"{sequence.Name}.asset");

			Google.Downloads -= 1;
		}

        /// <summary>
        /// Send a file request to the specified url.
        /// </summary>
        static async Task<Stream> Request(string url)
        {
            Stopwatch.Start();

            HttpResponseMessage response = await Client.GetAsync(url);
            Stream stream = await response.Content.ReadAsStreamAsync();

            WaitTime += Stopwatch.ElapsedMilliseconds;
            Stopwatch.Reset();

            return stream;
        }
    }
}