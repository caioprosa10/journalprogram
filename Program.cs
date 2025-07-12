using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace JournalProgram
{
    // Abstração de uma entrada de diário
    public class Entry
    {
        public string Prompt   { get; set; }
        public string Response { get; set; }
        public DateTime Date   { get; set; }

        public Entry(string prompt, string response, DateTime date)
        {
            Prompt   = prompt;
            Response = response;
            Date     = date;
        }
    }

    // Classe que gerencia o conjunto de entradas
    public class Journal
    {
        private List<Entry> _entries = new();
        private static readonly List<string> _prompts = new()
        {
            "Who was the most interesting person I interacted with today?",
            "What was the best part of my day?",
            "How did I see the hand of the Lord in my life today?",
            "What was the strongest emotion I felt today?",
            "If I had one thing I could do over today, what would it be?"
        };
        private Random _random = new();

        public void AddEntry()
        {
            var prompt = _prompts[_random.Next(_prompts.Count)];
            Console.WriteLine($"\nPrompt: {prompt}");
            Console.Write("Your response: ");
            var response = Console.ReadLine()!;
            _entries.Add(new Entry(prompt, response, DateTime.Now));
            Console.WriteLine("✔ Entry added.");
        }

        public void DisplayEntries()
        {
            if (_entries.Count == 0)
            {
                Console.WriteLine("⚠ No entries found.");
                return;
            }
            Console.WriteLine("\nJournal Entries:");
            foreach (var e in _entries)
                Console.WriteLine($"- {e.Date:yyyy-MM-dd HH:mm:ss} | {e.Prompt} | {e.Response}");
        }

        public void SaveToFile(string filename)
        {
            using var stream = File.Create(filename);
            JsonSerializer.Serialize(stream, _entries, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine($"✔ Journal saved to '{filename}'.");
        }

        public void LoadFromFile(string filename)
        {
            if (!File.Exists(filename))
            {
                Console.WriteLine($"ℹ File '{filename}' not found. Starting with empty journal.");
                return;
            }
            using var stream = File.OpenRead(filename);
            var loaded = JsonSerializer.Deserialize<List<Entry>>(stream);
            if (loaded != null)
                _entries = loaded;
            Console.WriteLine($"✔ Journal loaded from '{filename}'.");
        }
    }

    // Classe principal com o menu
    class Program
    {
        static void Main(string[] args)
        {
            var journal = new Journal();
            const string defaultFile = "journal.json";
            journal.LoadFromFile(defaultFile);

            while (true)
            {
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1 - Write new entry");
                Console.WriteLine("2 - Display journal");
                Console.WriteLine("3 - Save journal");
                Console.WriteLine("4 - Load journal");
                Console.WriteLine("5 - Exit");
                Console.Write("Option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        journal.AddEntry();
                        break;
                    case "2":
                        journal.DisplayEntries();
                        break;
                    case "3":
                        Console.Write("Filename to save: ");
                        journal.SaveToFile(Console.ReadLine()!);
                        break;
                    case "4":
                        Console.Write("Filename to load: ");
                        journal.LoadFromFile(Console.ReadLine()!);
                        break;
                    case "5":
                        Console.WriteLine("Exiting... See you soon!");
                        return;
                    default:
                        Console.WriteLine("❌ Invalid option. Try again.");
                        break;
                }
            }
        }
    }
}
