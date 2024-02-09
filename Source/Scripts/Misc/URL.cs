namespace GTE
{
	public static class URL
	{
		/// <summary>
		/// ...
		/// </summary>
		public static string? Format(string prompt, string language)
		{
			if (prompt != null && language != null)
			{
				prompt = prompt.Replace(" ", "%20");
				return string.Format(Text.URL, prompt, language);
			}
			else
			{
				return null;
			}
		}
	}
}