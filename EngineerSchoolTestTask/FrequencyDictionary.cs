using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace EngineerSchoolTestTask
{
    public class FrequencyDictionary : IEnumerable<WordFrequency>
    {
        public Dictionary<string, int> WordFrequenciesDictionary { get; private set; } = new();
        public WordFrequency[] WordFrequenciesArray { get; private set; } = { };

        public FrequencyDictionary() { }

        public FrequencyDictionary(string filePath)
        {
            AddDataFromFile(filePath);
        }

        public void AddDataFromFile(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                foreach (var fileWord in FileWords(reader))
                {
                    if (String.IsNullOrEmpty(fileWord)) continue;

                    WordFrequenciesDictionary[fileWord] = this[fileWord] + 1;
                }
            }

            WordFrequenciesArray = WordFrequenciesDictionary
                .OrderByDescending(p => p.Value).ThenBy(p => p.Key)
                .Select(p => new WordFrequency(p.Key, p.Value))
                .ToArray();
        }

        private IEnumerable<string> FileWords(StreamReader reader)
        {
            var lettersRegex = new Regex(@"\p{L}{1}");
            var inWordSymbolsRegex = new Regex(@"[-‐‑‒–−—'’]{1}");

            string symbol, inWordSymbol = "";
            var word = new StringBuilder();

            while(true)
            {
                var symbolAsInt = reader.Read();

                if (symbolAsInt == -1)
                {
                    yield return word.ToString();
                    break;
                }

                symbol = ((char)symbolAsInt).ToString();
                var isLetter = lettersRegex.Match(symbol).Success;
                var isInWordSymbol = inWordSymbolsRegex.Match(symbol).Success;

                if (isLetter)
                {
                    if (inWordSymbol != "")
                    {
                        word.Append(inWordSymbol);
                        inWordSymbol = "";
                    }

                    word.Append(symbol);
                    continue;
                }

                if (isInWordSymbol && word.Length > 0 && inWordSymbol == "")
                {
                    inWordSymbol = symbol;
                    continue;
                }

                inWordSymbol = "";

                if (word.Length == 0) continue;

                string madeWord = word.ToString();
                word.Clear();
                
                yield return madeWord.ToLower();
            }
        }

        public void SaveToFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                if (Count == 0)
                {
                    writer.WriteLine("*No words found*");
                    return;
                }

                foreach (var wordFrequency in this)
                {
                    writer.WriteLine($"{wordFrequency.Word}: {wordFrequency.Frequency}");
                }
            }
        }

        public int Count => WordFrequenciesArray.Length;

        public int this[string word] 
            => WordFrequenciesDictionary.ContainsKey(word) ? WordFrequenciesDictionary[word] : 0;
        
        public WordFrequency this[int position] => WordFrequenciesArray[position];

        public IEnumerator<WordFrequency> GetEnumerator() => WordFrequenciesArray.AsEnumerable().GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public record WordFrequency(string Word, int Frequency);
}
