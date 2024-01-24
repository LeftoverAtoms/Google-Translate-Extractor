namespace GTE
{
    public static class GitGuid
    {
        const string GUID = "guid";

        static readonly List<string> collection = [];
        static int Writes;

        public static bool Init(string path)
        {
            string[]? files = null;

            // Retrieve all meta files in path.
            if (Directory.Exists(path))
            {
                files = Directory.GetFiles(path, "*.meta", SearchOption.AllDirectories);
            }

            // Failed.
            if (files == null || files.Length == 0)
            {
                Console.CursorTop -= 1;
                Log.Warning($"Failed to find any meta files at '{path}'!");
                return false;
            }

            Log.Info($"Found {files.Length} meta files at '{path}'!\n");

            // Initialize a buffer to write to while reading through each file stream.
            char[] buffer = new char[48]; // Minimum requirement is 38 characters, but I recommended to add some leeway.

            foreach (string file in files)
            {
                Parse(file, buffer);
            }

            return true;
        }
        public static string Generate()
        {
            // Regenerate until the guid is unique.
            while (true)
            {
                string guid = Guid.NewGuid().ToString("N");

                // Guid is a duplicate.
                if (collection.Contains(guid))
                {
                    Console.WriteLine("Duplicate Guid, regenerating...");
                    continue;
                }

                collection.Add(guid);
                return guid;
            }
        }

        static void Parse(string file, char[] buffer)
        {
            using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read))
            using (StreamReader reader = new StreamReader(stream))
            {
                int lineCount = reader.ReadToEnd().Count(x => x == '\n');
                stream.Position = 0; // Reset after reading through the entire stream.

                for (int i = 0; i < lineCount; i++)
                {
                    string? line = reader.ReadLine();
                    if (line == null)
                    {
                        Console.WriteLine("BAD");
                        continue;
                    }

                    foreach (char character in line)
                    {
                        // Ignore special characters.
                        if (char.IsLetterOrDigit(character))
                        {
                            continue;
                        }

                        // Write character to buffer.
                        buffer[Writes++] = character;

                        // Check if variable name is a guid.
                        if (Writes == GUID.Length)
                        {
                            string str = BuildString(buffer);

                            // Not a guid, check next line.
                            if (str != GUID)
                            {
                                break;
                            }
                        }
                        // Must be the value of a guid.
                        else if (Writes == 32)
                        {
                            string str = BuildString(buffer);

                            // Ignore duplicate guids.
                            if (!collection.Contains(str))
                            {
                                collection.Add(str);
                            }

                            break;
                        }
                    }
                }
            }
        }
        static string BuildString(char[] buffer)
        {
            string str = new string(buffer, 0, Writes);
            Writes = 0;

            return str;
        }
    }
}