namespace GTE
{
	public struct Sequence
	{
		public string Name { get; set; }
		public Dialogue[] Dialogue { get; set; }

		/// <summary>
		/// Create a dialogue asset.
		/// </summary>
		public static string Format(Sequence sequence, int language)
		{
			string subtitles = "";

			foreach (Dialogue dialogue in sequence.Dialogue)
			{
				string subtitle = dialogue.Subtitle.Replace(".", "").Replace(",", "");

				subtitles += string.Format(Text.Subtitle, dialogue.Guid, subtitle);
			}

			return string.Format(Text.Dialogue, sequence.Name, subtitles, language);
		}
	}
}