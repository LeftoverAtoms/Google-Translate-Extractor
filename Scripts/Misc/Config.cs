using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace GTE
{
	// Interface
	public static partial class Config
	{
		public static void Serialize(Locale[] value)
		{
			using (FileStream stream = new FileStream(Json, FileMode.OpenOrCreate, FileAccess.ReadWrite))
			{
				JsonSerializer.Serialize(stream, value, options);
			}
		}
		public static void Parse(out Locale[] data)
		{
			try
			{
				using (FileStream stream = new FileStream(Json, FileMode.Open, FileAccess.Read))
				{
					data = JsonSerializer.Deserialize<Locale[]>(stream);
				}
			}
			catch
			{
				Log.Error("Configuration is missing or invalid! Writing defaults to working directory...\n");
				data = WriteDefaults();
			}

			ValidateGuid(data);
		}

		public static Locale[] WriteDefaults()
		{
			Locale[] data =
			[
				new Locale()
				{
					Language = new Language()
					{
						Name = "en"
					},
					Sequence =
					[
						new Sequence()
						{
							Name = "test",
							Dialogue =
							[
								new Dialogue()
								{
									Name = "en_test_01",
									Subtitle = "the cake. is a lie",
									Guid = null
								}
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
                        Log.Error("Dialogue is null!", true);
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

	// Internal
	public static partial class Config
	{
		const string Json = "Config.json";

		static JsonSerializerOptions options = new JsonSerializerOptions()
		{
			Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
			WriteIndented = true
		};
	}
}