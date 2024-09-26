using Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace GTE;

public static class JSON
{
    public static ConcurrentDictionary<string, List<Sequence>> Data { get; }

    public static bool IsProcessing { get => s_processes != 0; }

    private static int s_processes;

    static JSON()
    {
        Data = new ConcurrentDictionary<string, List<Sequence>>();
    }

    public static async void Deserialize(string filePath)
    {
        s_processes++;

        // Use file name to categorize sequences into groups.
        string type = Path.GetFileNameWithoutExtension(filePath);

        // Load JSON document.
        string document = await File.ReadAllTextAsync(filePath);
        ConsoleColor.DarkGreen.WriteLine($"Loaded '{type}'");

        // Deserialize JSON to structure.
        var group = JsonConvert.DeserializeObject<List<Sequence>>(document);
        if (group != null)
        {
            Data.TryAdd(type, group);
            ConsoleColor.DarkGreen.WriteLine($"Deserialized '{type}'");
        }

        s_processes--;
    }
}
