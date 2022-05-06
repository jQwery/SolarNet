using System.Collections.Generic;

namespace SolaraNet.DataAccessLayer.EntityFramework.Common.Validators
{
    public class StringValidator
    {
        private static readonly IList<string> _censoreWords = new List<string>();

        public static bool CheckString(string valueToCheck)
        {
            if (_censoreWords.Count==0)
            {
                SetUpBadWords();
            }
            if (!string.IsNullOrWhiteSpace(valueToCheck))
            {
                var words = valueToCheck.Split(' ', ',', '.');
                foreach (var word in words)
                {
                    if (_censoreWords.Contains(word))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void SetUpBadWords()
        {
            _censoreWords.Add("дурак");
            _censoreWords.Add("тупой");
            _censoreWords.Add("нехороший");
            _censoreWords.Add("свинья");
        }
    }
}