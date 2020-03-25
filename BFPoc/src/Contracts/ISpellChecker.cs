

using BFPoc.Models;

namespace BFPoc.Contracts
{
    public interface ISpellChecker
    {

        /// <summary>
        /// Main Check API
        /// </summary>
        /// <param name="toCheck"></param>
        /// <returns></returns>
        SpellCheckResult CheckSpelling(string toCheck);
    }
}