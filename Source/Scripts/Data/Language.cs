namespace GTE
{
	public struct Language(string name)
	{
		static string[] ValidCodes => ["en", "fr", "es"];

		public string Name { get; set; } = name;

		public readonly int GetID()
		{
			string name = Name; // Lambda expressions cannot access instance members. 
			int id = Array.FindIndex(ValidCodes, x => (x == name));
			return id;
		}
	}
}