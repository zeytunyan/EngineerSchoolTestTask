using EngineerSchoolTestTask;
using FrequencyDictionaryLib;
using System.Diagnostics;
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
    var frequencyDictionaryObj = new FrequencyDictionary();
    var methodname = "MakeWordFrequencyDictionaryFromTextLinq";
    var dictionaryMethod = typeof(FrequencyDictionary).GetMethod(methodname, BindingFlags.NonPublic | BindingFlags.Instance);

    Dictionary<string, int>? invokeResult = new();
    var (serialResults, parallelResults) = (0L, 0L);
    var launchesCount = 50;

    for (int i = 0; i < launchesCount; i++)
    {
        var stopwatch = Stopwatch.StartNew();
        invokeResult = dictionaryMethod?.Invoke(frequencyDictionaryObj, new[] { fileText }) as Dictionary<string, int>;
        stopwatch.Stop();
        serialResults += stopwatch.ElapsedMilliseconds;

        stopwatch.Restart();
        invokeResult = frequencyDictionaryObj.MakeWordFrequencyDictionaryFromTextParallelLinq(fileText);
        stopwatch.Stop();
        parallelResults += stopwatch.ElapsedMilliseconds;
    }

    var frequencyDictionary = invokeResult ?? new();
    var (finalSerialResult, finalParallelResult) = ( ((float)serialResults) / 100, ((float)parallelResults) / 100 );

    Console.WriteLine();
    interactor.WriteDictionary(frequencyDictionary);

    var мeasurements = $@"
Medium elapsed time in milliseconds:
Serial: {finalSerialResult}
Parallel: {finalParallelResult}
";

    Console.WriteLine(мeasurements);

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

    writer.WriteLine(мeasurements);
});