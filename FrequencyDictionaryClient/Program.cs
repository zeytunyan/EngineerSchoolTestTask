using EngineerSchoolTestTask;
using System.Net.Http;
using System;
using System.Net.Http.Json;

namespace FrequencyDictionaryClient
{
    internal static class Program
    {
        private const string _httpPath = "https://localhost:7048/frequencies/create";
        private static readonly HttpClient _httpClient = new();

        private static async Task Main(string[] args)
        {
            var interactor = new ConsoleInteractor
            {
                Greetings = "Hello, this is a program for creating a frequency dictionary",
                AskEnterValueMessage = "Please, enter the file path:",
                FailMessage = "Failed to create a frequency dictionary",
                ActionDoneMessage = "The frequency dictionary was created"
            };

            await interactor.ContinuousInteraction(async (path) =>
            {
                var fileText = File.ReadAllText(path ?? "");

                using var response = await _httpClient.PostAsJsonAsync(_httpPath, fileText);
                var frequencyDictionary = await response.Content.ReadFromJsonAsync<Dictionary<string, int>>() ?? new();

                Console.WriteLine();
                interactor.WriteDictionary(frequencyDictionary);

                if (!interactor.AskYesOrNo("Save it to a file?"))
                {
                    return;
                }

                var filePath = interactor.AskEnterValue() ?? "";

                using var writer = new StreamWriter(filePath, false);

                if (!frequencyDictionary.Any())
                {
                    writer.WriteLine("*No words found*");
                    return;
                }

                foreach (var wordFrequency in frequencyDictionary)
                {
                    writer.WriteLine($"{wordFrequency.Key}: {wordFrequency.Value}");
                }
            });
        }
    }
}