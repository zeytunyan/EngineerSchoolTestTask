using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace FrequencyDictionaryLib
{
    public record WordFrequency(string Word, int Frequency);

    public class FrequencyDictionary : IEnumerable<WordFrequency>
    {
        public Dictionary<string, int> WordFrequenciesDictionary { get; private set; } = new();
        public WordFrequency[] WordFrequenciesArray { get; private set; } = Array.Empty<WordFrequency>();

        public FrequencyDictionary() { }

        public FrequencyDictionary(string filePath)
        {
            AddDataFromFile(filePath);
        }

        public void AddDataFromFile(string filePath)
        {
            using (var reader = new StreamReader(filePath))
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

                // Если файл закончился, возвращаем последнее слово и завершаем цикл
                if (symbolAsInt == -1)
                {
                    yield return word.ToString();
                    break;
                }

                symbol = ((char)symbolAsInt).ToString();
                var isLetter = lettersRegex.Match(symbol).Success;
                var isInWordSymbol = inWordSymbolsRegex.Match(symbol).Success;

                // Если символ - буква
                if (isLetter)
                {
                    // Если она идет после символа, который может быть внутри слова
                    if (inWordSymbol != "")
                    {
                        // Добавляем этот символ и забываем его 
                        word.Append(inWordSymbol);
                        inWordSymbol = "";
                    }

                    // Добавляем саму букву и идем дальше
                    word.Append(symbol);
                    continue;
                }

                // Если это символ, который может быть внутри слова,
                // при этом он действительно внутри слова и перед ним нет другого такого
                if (isInWordSymbol && word.Length > 0 && inWordSymbol == "")
                {
                    // Сохраняем его, чтобы добавить, если встретим букву, и идем дальше
                    inWordSymbol = symbol;
                    continue;
                }

                // Если это не буква и не символ, который может быть внутри слова
                // или символ, который может быть внутри слова, но он не внутри
                // или он там не один, то считаем, что слово готово

                // Обнуляем предыдущий символ, который может быть внутри слова
                inWordSymbol = "";

                if (word.Length == 0) continue;

                string madeWord = word.ToString();
                word.Clear();

                yield return madeWord.ToLower();
            }
        }

        public void SaveToFile(string filePath)
        {
            using var writer = new StreamWriter(filePath, false);
            
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


        private Dictionary<string , int> MakeWordFrequencyDictionaryFromTextLinq(string text)
        {
            var wordRegex = new Regex("\\p{L}+([-‐‑‒–−—'’]?\\p{L}+)*");

            return wordRegex.Matches(text)
                .GroupBy(match => match.ToString().ToLower(), (word, matches) => new WordFrequency(word, matches.Count()))
                .OrderByDescending(wordFrequency => wordFrequency.Frequency).ThenBy(wordFrequency => wordFrequency.Word)
                .ToDictionary(wordFrequency => wordFrequency.Word, wordFrequency => wordFrequency.Frequency);
        }

        public Dictionary<string, int> MakeWordFrequencyDictionaryFromTextParallelLinq(string text)
        {
            var wordRegex = new Regex("\\p{L}+([-‐‑‒–−—'’]?\\p{L}+)*");

            return wordRegex.Matches(text).AsParallel()
                .GroupBy(match => match.ToString().ToLower(), (word, matches) => new WordFrequency(word, matches.Count()))
                .OrderByDescending(wordFrequency => wordFrequency.Frequency).ThenBy(wordFrequency => wordFrequency.Word)
                .ToDictionary(wordFrequency => wordFrequency.Word, wordFrequency => wordFrequency.Frequency);
        }

        public int Count => WordFrequenciesArray.Length;

        public int this[string word] 
            => WordFrequenciesDictionary.ContainsKey(word) ? WordFrequenciesDictionary[word] : 0;
        
        public WordFrequency this[int position] => WordFrequenciesArray[position];

        public IEnumerator<WordFrequency> GetEnumerator() => WordFrequenciesArray.AsEnumerable().GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
