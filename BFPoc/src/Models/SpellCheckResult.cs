using System;
using System.Collections.Generic;
using System.Text;

namespace BFPoc.Models
{
    /// <summary>
    /// Wrapper for Results from SpellChecker
    /// TODO - Should expose some type of probabilistic result instead of Enum 
    /// </summary>
    public struct SpellCheckResult
    {
        public SpellCheckResult(ResultStatus status, ICollection<string> matches)
        {
            Status = status;
            Matches = matches;
        }

        public ResultStatus Status { get; }
        public ICollection<string> Matches { get; }

        public override string ToString()
        {
            return $"{nameof(Status)}: {Status}, {nameof(Matches)}: {GetString(Matches)}";
        }

        private string GetString(ICollection<string> matches)
        {
            var sb = new StringBuilder();
            foreach (var match in matches)
            {
                sb.Append($"Did you mean: {match}?").Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
