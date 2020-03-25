using System;
using System.Collections.Generic;

namespace BFPoc.Extensibility
{

    /// <summary>
    /// TODO - trite example of producing word variations for checking for suggestions
    /// Sourced from: https://codereview.stackexchange.com/questions/28248/implement-a-function-that-prints-all-possible-combinations-of-the-characters-in
    /// </summary>
    class AlternativeWordGenerator
    {

        public string[] Combination2(string str)
        {
            List<string> output = new List<string>();
            // Working buffer to build new sub-strings
            char[] buffer = new char[str.Length];

            Combination2Recurse(str.ToCharArray(), 0, buffer, 0, output);

            return output.ToArray();
        }

        public void Combination2Recurse(char[] input, int inputPos, char[] buffer, int bufferPos, List<string> output)
        {
            if (inputPos >= input.Length)
            {
                // Add only non-empty strings
                if (bufferPos > 0)
                    output.Add(new string(buffer, 0, bufferPos));

                return;
            }

            // Recurse 2 times - one time without adding current input char, one time with.
            Combination2Recurse(input, inputPos + 1, buffer, bufferPos, output);

            buffer[bufferPos] = input[inputPos];
            Combination2Recurse(input, inputPos + 1, buffer, bufferPos + 1, output);
        }
    }
}
