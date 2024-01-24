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

		public static void Serialize(Locale[] value)
		{
			using (FileStream stream = new FileStream(Json, FileMode.OpenOrCreate, FileAccess.ReadWrite))
			{
				JsonSerializer.Serialize(stream, value, JsonOptions);
			}
		}
		public static void Parse(out Locale[] data)
		{
			// Missing file.
			if (!Path.Exists(Json))
			{
				Log.Error("Configuration is missing!");
				data = WriteDefaults();
			}
			else
			{
				// Compiler was complaining.
				data = [];

				// Has a chance to throw an exception.
				try
				{
					using (FileStream stream = new FileStream(Json, FileMode.Open, FileAccess.Read))
					{
						data = JsonSerializer.Deserialize<Locale[]>(stream, JsonOptions);
					}
				}
				catch (Exception error)
				{
					Log.Info("--- PLEASE POST AN ISSUE WITH THIS ERROR ON GITHUB ---");
					Log.Info(error, ConsoleColor.Red);
					Log.Pause();
				}
			}

			// Compiler was complaining.
			if (data == null)
			{
				data = WriteDefaults();
			}

			ValidateGuid(data);
		}

		public static Locale[] WriteDefaults()
		{
			Log.Info("Writing defaults to working directory...\n");

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
								new Dialogue("en_test_01", "the cake. is a lie")
							]
						}
					]
				}
			];

			Serialize(data);

			return data;
		}
		public static void ValidateGuid(Locale[] data)
		{
			bool changed = false;

			foreach (Locale locale in data)
			{
				if (locale.Sequence == null)
				{
					Log.Error("Sequence is null!");
					return;
				}

				foreach (Sequence sequence in locale.Sequence)
				{
					if (sequence.Dialogue == null)
					{
						Log.Error("Dialogue is null!");
						Log.Pause();
						return;
					}

					for (int i = 0; i < sequence.Dialogue.Length; i++)
					{
						// Validate each dialogue guid.
						if (!Guid.TryParse(sequence.Dialogue[i].Guid, out _))
						{
							sequence.Dialogue[i].Guid = GitGuid.Generate();
							changed = true;
						}
					}
				}
			}

			// Changed some data so reserialize the config.
			if (changed)
			{
				Log.Warning("Reserializing config with valid guids!\n");
				Serialize(data);
			}
		}
	}
}