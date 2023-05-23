using EngineerSchoolTestTask;
using FrequencyDictionaryLib;
using System.Reflection;

var interactor = new ConsoleInteractor
{
    Greetings = "Hello, this is a program for creating a frequency dictionary",
    AskEnterValueMessage = "Please, enter the file path:",
    FailMessage = "Failed to create a frequency dictionary",
    ActionDoneMessage = "The frequency dictionary was created"
};

interactor.ContinuousInteraction(path =>
{
    var fileText = File.ReadAllText(path ?? "");

    var methodname = "MakeWordFrequencyDictionaryFromText";
    var frequencyDictionaryType = typeof(FrequencyDictionary);
    var dictionaryMethod = frequencyDictionaryType.GetMethod(methodname, BindingFlags.NonPublic | BindingFlags.Instance);
    var invokeResult = dictionaryMethod?.Invoke(new FrequencyDictionary(), new[] { fileText }) as Dictionary<string, int>;
    Dictionary<string, int> frequencyDictionary = invokeResult ?? new(); 

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