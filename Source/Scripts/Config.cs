namespace GTE
{
	public static class Config
	{
		const string Json = "Config.json";
		static readonly JsonSerializerOptions JsonOptions;

		static Config()
		{
			JsonOptions = new JsonSerializerOptions()
			{
				AllowTrailingCommas = true,                            // Ignore trailing commas.
				Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // Allow irregular characters.
				ReadCommentHandling = JsonCommentHandling.Skip,        // Ignore comments.
				WriteIndented = true                                   // Output legible text.
			};
		}

		/// <summary>
		/// ...
		/// </summary>
		public static void Serialize(Locale[] value)
		{
			using (FileStream stream = new FileStream(Json, FileMode.OpenOrCreate, FileAccess.ReadWrite))
			{
				JsonSerializer.Serialize(stream, value, JsonOptions);
			}
		}

		/// <summary>
		/// ...
		/// </summary>
		public static void Parse(out Locale[] data)
		{
			Locale[]? file = null;

			// Missing file.
			if (!Path.Exists(Json))
			{
				Log.Info("Error: Configuration is missing!", 2, ConsoleColor.Red);
				file = WriteDefaults();
			}
			else
			{
				// Has a chance to throw an exception.
				try
				{
					using (FileStream stream = new FileStream(Json, FileMode.Open, FileAccess.Read))
					{
						file = JsonSerializer.Deserialize<Locale[]>(stream, JsonOptions);
					}
				}
				catch (Exception error)
				{
					Log.Info("--- PLEASE POST AN ISSUE WITH THE ERROR BELOW ON GITHUB ---", 1, ConsoleColor.Magenta);
					Log.Info(error, 1, ConsoleColor.Red);
					Log.Info("--- PLEASE POST AN ISSUE WITH THE ERROR ABOVE ON GITHUB ---", 1, ConsoleColor.Magenta);
					Log.Pause();
				}
			}

			// Compiler was complaining.
			if (file == null)
			{
				file = WriteDefaults();
			}

			data = file;
		}

		/// <summary>
		/// ...
		/// </summary>
		public static Locale[] WriteDefaults()
		{
			Log.Info("Writing defaults to working directory...", 2, ConsoleColor.Yellow);

			Locale[] data =
			[
				new Locale()
				{
					Language = new Language("en"),
					Sequence =
					[
						new Sequence()
						{
							Name = "test",
							Dialogue =
							[
								"the cake. is a lie",
								"goodbye"
							]
						}
					]
				}
			];

			Serialize(data);

			return data;
		}
	}
}