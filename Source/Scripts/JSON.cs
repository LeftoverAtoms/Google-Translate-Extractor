using Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace GTE
{
    public static class JSON
    {
        // Sequence Type -> Sequence Group -> Sequence
        public static ConcurrentDictionary<string, Dictionary<string, Sequence>> Data { get; }

        public static bool HasProcesses => m_processes != 0;

        private static int m_processes;

        static JSON()
        {
            Data = new ConcurrentDictionary<string, Dictionary<string, Sequence>>();
        }

        public class Sequence
        {
            // Language -> Subtitles
            public Dictionary<string, string[]>? Variants { get; init; }

            public bool TryGetVariant(string language, out string[] subtitles)
            {
                if (Variants != null && Variants.TryGetValue(language, out var sub))
                {
                    subtitles = sub;
                    return true;
                }
                ConsoleColor.Red.WriteLine($"Sequence Variant: '{language}' is undefined or invalid!");
                subtitles = [];
                return false;
            }
        }

        public static async void Deserialize(string filePath)
        {
            m_processes++;

            // Use file name to categorize sequences into groups.
            string sequenceType = Path.GetFileNameWithoutExtension(filePath);

            // Load JSON document.
            string document = await File.ReadAllTextAsync(filePath);
            ConsoleColor.Green.WriteLine($"Loaded '{sequenceType}'");

            // Deserialize JSON to structure.
            var sequenceGroup = JsonConvert.DeserializeObject<Dictionary<string, Sequence>>(document);
            if (sequenceGroup != null)
            {
                Data.TryAdd(sequenceType, sequenceGroup);
                ConsoleColor.Green.WriteLine($"Deserialized '{sequenceType}'");
            }

            m_processes--;
        }

        public static bool TryGetSequenceGroup(string type, out Dictionary<string, Sequence> sequenceGroup)
        {
            if (Data != null && Data.TryGetValue(type, out var group))
            {
                sequenceGroup = group;
                return true;
            }
            ConsoleColor.Red.WriteLine($"Sequence Group: '{type}' is undefined!");
            sequenceGroup = [];
            return false;
        }
        public static bool TryGetSequence(string type, string name, out Sequence sequence)
        {
            if (TryGetSequenceGroup(type, out var sequenceGroup) && sequenceGroup.TryGetValue(name, out var seq))
            {
                sequence = seq;
                return true;
            }
            ConsoleColor.Red.WriteLine($"Sequence: '{name}' is undefined!");
            sequence = new Sequence();
            return false;
        }
    }
}