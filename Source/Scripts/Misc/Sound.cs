namespace GTE
{
	public static class Sound
	{
		/// <summary>
		/// Create a meta file for sounds with a unique guid.
		/// </summary>
		public static string Format(string guid)
		{
			return string.Format(Text.Sound, guid);
		}
	}
}