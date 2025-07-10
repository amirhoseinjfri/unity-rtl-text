using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

// Developed by Spiritgoal Game Studio
// info@spiritgoal.com
public static class RtlTextShaper
{
    private static readonly HashSet<char> JoinsForward = new HashSet<char> {
        'ب', 'پ', 'ت', 'ث', 'ج', 'چ', 'ح', 'خ', 'س', 'ش', 'ص', 'ض', 'ط', 'ظ', 'ع', 'غ', 'ف', 'ق', 'ک', 'ك', 'گ', 'ل', 'م', 'ن', 'ه', 'ی', 'ي', 'ئ'
    };

    private static readonly Dictionary<char, int> RegularAlphabetIndex = new Dictionary<char, int>() {
        {'ا', 0}, {'آ', 1}, {'أ', 2}, {'إ', 3}, {'ب', 4}, {'پ', 5}, {'ت', 6}, {'ث', 7}, {'ج', 8}, {'چ', 9},
        {'ح', 10}, {'خ', 11}, {'د', 12}, {'ذ', 13}, {'ر', 14}, {'ز', 15}, {'ژ', 16}, {'س', 17}, {'ش', 18}, {'ص', 19},
        {'ض', 20}, {'ط', 21}, {'ظ', 22}, {'ع', 23}, {'غ', 24}, {'ف', 25}, {'ق', 26}, {'ک', 27}, {'ك', 28}, {'گ', 29},
        {'ل', 30}, {'م', 31}, {'ن', 32}, {'و', 33}, {'ؤ', 34}, {'ه', 35}, {'ی', 36}, {'ي', 37}, {'ئ', 38}, {'ء', 39},
        {'1', 100}, {'2', 101}, {'3', 102}, {'4', 103}, {'5', 104}, {'6', 105}, {'7', 106}, {'8', 107}, {'9', 108}, {'0', 109}
       };

    private static readonly List<char[]> ArabicPresentationForms = new List<char[]>() {
        /* 0*/ new char[] {'ﺍ', 'ﺍ', 'ﺎ', 'ﺎ'}, /* 1*/ new char[] {'ﺁ', 'ﺁ', 'ﺂ', 'ﺂ'}, /* 2*/ new char[] {'ﺃ', 'ﺃ', 'ﺄ', 'ﺄ'},
        /* 3*/ new char[] {'ﺇ', 'ﺇ', 'ﺈ', 'ﺈ'}, /* 4*/ new char[] {'ﺏ', 'ﺑ', 'ﺐ', 'ﺒ'}, /* 5*/ new char[] {'ﭖ', 'ﭘ', 'ﭗ', 'ﭙ'},
        /* 6*/ new char[] {'ﺕ', 'ﺗ', 'ﺖ', 'ﺘ'}, /* 7*/ new char[] {'ﺙ', 'ﺛ', 'ﺚ', 'ﺜ'}, /* 8*/ new char[] {'ﺝ', 'ﺟ', 'ﺞ', 'ﺠ'},
        /* 9*/ new char[] {'ﭺ', 'ﭼ', 'ﭻ', 'ﭽ'}, /*10*/ new char[] {'ﺡ', 'ﺣ', 'ﺢ', 'ﺤ'}, /*11*/ new char[] {'ﺥ', 'ﺧ', 'ﺦ', 'ﺨ'},
        /*12*/ new char[] {'ﺩ', 'ﺩ', 'ﺪ', 'ﺪ'}, /*13*/ new char[] {'ﺫ', 'ﺫ', 'ﺬ', 'ﺬ'}, /*14*/ new char[] {'ﺭ', 'ﺭ', 'ﺮ', 'ﺮ'},
        /*15*/ new char[] {'ﺯ', 'ﺯ', 'ﺰ', 'ﺰ'}, /*16*/ new char[] {'ﮊ', 'ﮊ', 'ﮋ', 'ﮋ'}, /*17*/ new char[] {'ﺱ', 'ﺳ', 'ﺲ', 'ﺴ'},
        /*18*/ new char[] {'ﺵ', 'ﺷ', 'ﺶ', 'ﺸ'}, /*19*/ new char[] {'ﺹ', 'ﺻ', 'ﺺ', 'ﺼ'}, /*20*/ new char[] {'ﺽ', 'ﺿ', 'ﺾ', 'ﻀ'},
        /*21*/ new char[] {'ﻁ', 'ﻃ', 'ﻂ', 'ﻄ'}, /*22*/ new char[] {'ﻅ', 'ﻇ', 'ﻆ', 'ﻈ'}, /*23*/ new char[] {'ﻉ', 'ﻋ', 'ﻊ', 'ﻌ'},
        /*24*/ new char[] {'ﻍ', 'ﻏ', 'ﻎ', 'ﻐ'}, /*25*/ new char[] {'ﻑ', 'ﻓ', 'ﻒ', 'ﻔ'}, /*26*/ new char[] {'ﻕ', 'ﻗ', 'ﻖ', 'ﻘ'},
        /*27*/ new char[] {'ک', 'ﻛ', 'ﻚ', 'ﻜ'}, /*28*/ new char[] {'ك', 'ﻛ', 'ﻚ', 'ﻜ'}, /*29*/ new char[] {'ﮒ', 'ﮔ', 'ﮓ', 'ﮕ'},
        /*30*/ new char[] {'ﻝ', 'ﻟ', 'ﻞ', 'ﻠ'}, /*31*/ new char[] {'ﻡ', 'ﻣ', 'ﻢ', 'ﻤ'}, /*32*/ new char[] {'ﻥ', 'ﻧ', 'ﻦ', 'ﻨ'},
        /*33*/ new char[] {'ﻭ', 'ﻭ', 'ﻮ', 'ﻮ'}, /*34*/ new char[] {'ﺅ', 'ﺅ', 'ﺆ', 'ﺆ'}, /*35*/ new char[] {'ﻩ', 'ﻫ', 'ﻪ', 'ﻬ'},
        /*36*/ new char[] {'ﻯ', 'ﻳ', 'ﻰ', 'ﻴ'}, /*37*/ new char[] {'ي', 'ﻳ', 'ﻲ', 'ﻴ'}, /*38*/ new char[] {'ﺉ', 'ﺋ', 'ﺊ', 'ﺌ'},
        /*39*/ new char[] {'ﺀ', 'ﺀ', 'ﺀ', 'ﺀ'}
    };

    private static readonly HashSet<char> LatinNumerals = new HashSet<char> {
        '1', '2', '3', '4', '5', '6', '7', '8', '9', '0'
    };
    private static readonly Dictionary<char, char> LatinToArabicIndicMap = new Dictionary<char, char> {
        {'1', '١'}, {'2', '٢'}, {'3', '٣'}, {'4', '٤'}, {'5', '٥'},
        {'6', '٦'}, {'7', '٧'}, {'8', '٨'}, {'9', '٩'}, {'0', '٠'}
    };
    private static readonly HashSet<char> ArabicIndicNumerals = new HashSet<char>(LatinToArabicIndicMap.Values);

    private static bool IsLatinLetter(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
    }

    /// <summary>
    /// Shapes Persian/Arabic text for display engines like TextMesh Pro.
    /// Processes text in logical order, relying on the engine for final RTL display.
    /// </summary>
    public static string FixTextForTextMeshPro(string logicalInput, bool convertNumbersToArabicIndic = true)
    {
        if (string.IsNullOrEmpty(logicalInput))
        {
            return logicalInput;
        }

        var finalResultBuilder = new StringBuilder();
        logicalInput = logicalInput.Replace("لآ", "ﻵ").Replace("لأ", "ﻷ").Replace("لإ", "ﻹ");

        string[] lines = logicalInput.Split('\n');

        for (int j = 0; j < lines.Length; j++)
        {
            if (j > 0)
            {
                finalResultBuilder.Append('\n');
            }

            string line = lines[j];
            string[] words = Regex.Split(line, @"( )");
            var processedSegments = new List<string>(words.Length);

            foreach (string currentSegment in words)
            {
                if (string.IsNullOrWhiteSpace(currentSegment))
                {
                    processedSegments.Add(currentSegment);
                    continue;
                }

                string currentWord = currentSegment;
                var convertedWordBuilder = new StringBuilder(currentWord.Length);

                for (int i = 0; i < currentWord.Length; i++)
                {
                    char currentChar = currentWord[i];

                    if (IsLatinLetter(currentChar))
                    {
                        convertedWordBuilder.Append(currentChar);
                        continue;
                    }

                    char prevChar = (i > 0) ? currentWord[i - 1] : '\0';
                    char nextChar = (i < currentWord.Length - 1) ? currentWord[i + 1] : '\0';

                    if (RegularAlphabetIndex.TryGetValue(currentChar, out int charIndex) && charIndex < 100)
                    {
                        bool prevWasLatin = (i > 0) && IsLatinLetter(prevChar);
                        bool nextWasLatin = (i < currentWord.Length - 1) && IsLatinLetter(nextChar);

                        bool joinsFromPrevious = !prevWasLatin && (i > 0) && JoinsForward.Contains(prevChar);
                        bool joinsToNext = !nextWasLatin && (i < currentWord.Length - 1) && JoinsForward.Contains(currentChar)
                                          && RegularAlphabetIndex.ContainsKey(nextChar) && RegularAlphabetIndex[nextChar] < 100;

                        int formIndex = 0;
                        if (!joinsFromPrevious && joinsToNext) formIndex = 1;
                        else if (joinsFromPrevious && !joinsToNext) formIndex = 2;
                        else if (joinsFromPrevious && joinsToNext) formIndex = 3;

                        convertedWordBuilder.Append(ArabicPresentationForms[charIndex][formIndex]);
                    }
                    else
                    {
                        char charToAppend = currentChar;
                        if (convertNumbersToArabicIndic && LatinNumerals.Contains(currentChar))
                        {
                            charToAppend = LatinToArabicIndicMap[currentChar];
                        }
                        convertedWordBuilder.Append(charToAppend);
                    }
                }
                processedSegments.Add(convertedWordBuilder.ToString());
            }
            finalResultBuilder.Append(string.Concat(processedSegments));
        }
        return finalResultBuilder.ToString();
    }
}
