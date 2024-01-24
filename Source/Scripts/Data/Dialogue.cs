namespace GTE
{
	public struct Dialogue(string name, string subtitle)
	{
		public string Name { get; set; } = name;
		public string Subtitle { get; set; } = subtitle;
		public string Guid { get; set; } = "";
	}
}