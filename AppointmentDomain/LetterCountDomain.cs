using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppointmentDomain
{
    public class LetterCountDomain
    {
        public string LetterCount(string message)
        {
            var wordList = message.Split(' ').ToList();
            int score = 0;
            string highestScoreWord = "-1";
            foreach (var iWord in wordList)
            {
                var tempScore = WordScore(iWord);
                if (tempScore > score)
                {
                    score = tempScore;
                    highestScoreWord = iWord;
                }

            }

            return highestScoreWord;
        }


        private int WordScore(string message)
        {
            //Get the first character.
            //Find the count of that character in the string
            //If it is greater than 1 then increment a counter.
            //Remove all occurrances of the character from the string and repeat until string.length == 0
            int score = 0;
            while (message.Length > 0)
            {
                var firstChar = message[0];
                var regEx = new Regex(firstChar.ToString());
                var charCount = regEx.Matches(message).Count;
                if (charCount > 1)
                {
                    score++;
                }
                message = message.Replace(message[0].ToString(), "");
            }

            return score;
        }
    }
}
