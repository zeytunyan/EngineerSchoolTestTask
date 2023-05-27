namespace EngineerSchoolTestTask
{
    public class ConsoleInteractor
    {
        public string Greetings { get; set; } = "Hello!";
        public string Farewell { get; set; } = "Bye!";
        public string AskEnterValueMessage { get; set; } = "Please, enter a value:";
        public string ActionStartsMessage { get; set; } = "The work began...";
        public string ActionDoneMessage { get; set; } = "The work is finished!";
        public string FailMessage { get; set; } = "Fail";
        public string TryAgainMessage { get; set; } = "Try again?";
        public string HowToAgreeInstructions { get; set; }
        public string AgreementKey { get; set; } = "y";

        public ConsoleInteractor() 
        {
            HowToAgreeInstructions = $"Please, type {AgreementKey} to agree, or anything else to exit";
        }

        public async Task ContinuousInteraction(Func<string?, Task>? interactionAction)
        {
            SayHello();

            while (true)
            {
                var value = AskEnterValue();
                SayActionStarts();

                try
                {
                    await interactionAction?.Invoke(value);
                }
                catch (Exception ex)
                {
                    ReportFail(ex.Message);
                    
                    if (AskTryAgain())
                    {
                        continue;
                    }
                    
                    break;
                }
                
                Say(ActionDoneMessage);

                if (!AskTryAgain()) break;
            }

            SayGoodbye();
        }


        public void WriteDictionary<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
        {
            foreach (var item in dictionary)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
            Console.WriteLine();
        }

        public void WriteEnumerable<T>(IEnumerable<T> enumerable)
        {
            foreach (var item in enumerable)
            {
                Console.WriteLine(item?.ToString());
            }

            Console.WriteLine();
        }

        public void SayHello() => Say(Greetings);
        public void SayActionStarts() => Say(ActionStartsMessage);
        public void SayActionDone() => Say(ActionDoneMessage);
        public void ReportFail(string report) => Say($"{FailMessage}: {report}");
        
        public void SayGoodbye()
        {
            Say(Farewell);
            Console.ReadKey();
        }

        public void Say(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine();
        }

        public string? AskEnterValue() => Ask(AskEnterValueMessage);
        public bool AskTryAgain() => AskYesOrNo(TryAgainMessage);
        
        public bool AskYesOrNo(string question)
        {
            var answer = Ask(question, HowToAgreeInstructions) ?? "";

            if (String.IsNullOrWhiteSpace(answer))
            {
                answer = answer.Trim().ToLower();
            }

            return answer == $"{AgreementKey}";
        }

        public string? Ask(string question, string? instructions = "")
        {
            Console.WriteLine(question);

            if (!String.IsNullOrEmpty(instructions))
            {
                Console.WriteLine(instructions);
            }

            var value = Console.ReadLine();
            Console.WriteLine();
            return value;
        }
    }
}
