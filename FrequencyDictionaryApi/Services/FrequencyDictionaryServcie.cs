using FrequencyDictionaryLib;

namespace FrequencyDictionaryApi.Services
{
    public class FrequencyDictionaryServcie
    {
        private readonly FrequencyDictionary _frequencyDictionary = new();

        public FrequencyDictionaryServcie() { }

        public Dictionary<string, int> CreateFrequencyDictionary(string text)
        {
            return _frequencyDictionary.MakeWordFrequencyDictionaryFromTextParallelLinq(text);
        }
    }
}
