namespace GTE
{
	public struct Sequence
	{
		public string Name { get; set; }
		public string[] Dialogue { get; set; }

		/// <summary>
		/// Create a dialogue asset.
		/// </summary>
		public static string Format(Sequence sequence, int language)
		{
			string subtitles = "";

			foreach (string dialogue in sequence.Dialogue)
			{
				string subtitle = dialogue.Replace(".", "").Replace(",", "");

				subtitles += string.Format(Text.Subtitle, subtitle);
			}

			return string.Format(Text.Dialogue, sequence.Name, subtitles, language);
		}
	}
}