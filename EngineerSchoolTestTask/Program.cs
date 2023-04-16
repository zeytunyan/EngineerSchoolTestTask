
using EngineerSchoolTestTask;

var interactor = new ConsoleInteractor();

interactor.Greetings = "Hello, this is a program for creating a frequency dictionary";
interactor.AskEnterValueMessage = "Please, enter the file path:";
interactor.FailMessage = "Failed to create a frequency dictionary";
interactor.ActionDoneMessage = "The frequency dictionary was created";

interactor.ContinuousInteraction(path =>
{
    var frequencyDictionary = new FrequencyDictionary(path ?? "");
    Console.WriteLine();
    interactor.WriteEnumerable(frequencyDictionary);
    
    if (interactor.AskYesOrNo("Save it to a file?"))
    {
        var filePath = interactor.AskEnterValue() ?? "";
        frequencyDictionary.SaveToFile(filePath);
    }
});