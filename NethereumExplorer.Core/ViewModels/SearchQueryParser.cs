using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NethereumExplorer.ViewModels
{
    public static class SearchQueryParser
    {
        public static readonly Regex BlockNumber = new Regex(@"^\d+$");
        public static readonly Regex TransactionHash = new Regex("^0(?i)x([A-Fa-f0-9]{64})$");
        public static readonly Regex Address = new Regex("^0(?i)x([A-Fa-f0-9]{40})$");

        public static readonly Regex[] All = { BlockNumber, TransactionHash, Address };

        public static readonly Dictionary<Regex, SearchType> RegexToSearchTypeDictionary = new Dictionary<Regex, SearchType>
        {
            {BlockNumber, SearchType.Block},
            {Address, SearchType.Address},
            {TransactionHash, SearchType.Transaction}
        };

        public static SearchType InferSearchType(string query)
        {
            var matchingRegex = All.FirstOrDefault(regex => regex.IsMatch(query));
            return matchingRegex == null ? SearchType.Unknown : RegexToSearchTypeDictionary[matchingRegex];
        }
    }
}
