namespace GTE
{
	// Interface
	public partial struct Language
	{
		public string Name { get; set; }

		public int GetID()
		{
			string name = Name; // Lambda expressions cannot access instance members. 
			int id = System.Array.FindIndex(validCodes, x => (x == name));
			return id;
		}
	}

	// Internal
	public partial struct Language
	{
		static readonly string[] validCodes = ["en", "fr", "es"];
	}
}