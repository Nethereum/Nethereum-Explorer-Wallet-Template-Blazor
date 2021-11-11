using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Nethereum.Contracts.QueryHandlers.MultiCall;
using Nethereum.Web3;
using Newtonsoft.Json;


namespace NethereumExplorer.Services
{

    public class BalanceResult
    {
        public string TokenImage { get; set; }
        public string TokenName { get; set; }
        public decimal Balance { get; set; }
        public decimal Price { get; set; }
        public decimal Value { get; set; }
        public string TokenAdress { get; set; }
        public GeckoToken GeckoToken { get; set; }
    }

    public class TokenBalanceService
    {
        private readonly HttpClient _httpClient;
        private readonly Web3 _web3;
        private List<Token> _tokens = new TokenListService().GetTokens();
        private List<GeckoToken> _geckoTokens = new GeckoTokenService().GetGeckoTokens();

        public int NumberOfTokens => _tokens.Count;

        public TokenBalanceService(HttpClient httpClient, Web3 web3)
        {
            _httpClient = httpClient;
            _web3 = web3;
        }

        public async Task<BalanceResult> GetEtherBalance(string account)
        {
            var etherBalance = Web3.Convert.FromWei(await _web3.Eth.GetBalance.SendRequestAsync(account));
            var etherPrice = (await GetPrices("ethereum"))["ethereum"]["usd"];

            return new BalanceResult() { Balance = etherBalance, Price = etherPrice, TokenName = "ETH", Value = etherBalance * etherPrice, TokenImage = "_content/NethereumExplorer.Core/images/0xeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee.png" };
        }

        public async Task<List<BalanceResult>> GetBalances(int page, int pageSize, string account)
        {

            var callList = new List<IMulticallInputOutput>();
            var startItem = (page * pageSize);
            var totaItemsToFetch = startItem + pageSize <= _tokens.Count ? startItem + pageSize : _tokens.Count + startItem;

            for (int i = startItem; i < totaItemsToFetch; i++)
            {
                var balanceOfMessage = new BalanceOfFunction() { Owner = account };
                var call = new MulticallInputOutput<BalanceOfFunction, BalanceOfOutputDTO>(balanceOfMessage,
                    _tokens[i].Address);
                callList.Add(call);
            }

            await _web3.Eth.GetMultiQueryHandler().MultiCallAsync(callList.ToArray()).ConfigureAwait(false);

            var currentBalances = new List<BalanceResult>();

            for (int i = startItem; i < totaItemsToFetch; i++)
            {

                var balance = ((MulticallInputOutput<BalanceOfFunction, BalanceOfOutputDTO>)callList[i - startItem]).Output.Balance;
                if (balance > 0)
                {
                    var balanceResult = new BalanceResult()
                    {
                        Balance = Web3.Convert.FromWei(balance, _tokens[i].Decimals),
                        TokenImage = _tokens[i].LogoURI,
                        TokenName = _tokens[i].Symbol,
                        TokenAdress = _tokens[i].Address,
                        GeckoToken = _geckoTokens.FirstOrDefault(x => x.Symbol.ToLower() == _tokens[i].Symbol.ToLower())
                    };

                    currentBalances.Add(balanceResult);
                }
            }

            if (currentBalances.Count > 0)
            {
                var ids = currentBalances.Select(x => x.GeckoToken?.Id).Distinct().ToArray();
                var prices = await GetPrices(ids).ConfigureAwait(false);

                foreach (var balance in currentBalances)
                {
                    if (balance.GeckoToken != null)
                    {
                        var price = prices[balance.GeckoToken.Id.ToLower()];
                        balance.Price = price["usd"];
                        balance.Value = balance.Price * balance.Balance;
                    }
                }
            }

            return currentBalances;
        }

        public Task<Dictionary<string, Dictionary<string, decimal>>> GetPrices(params string[] geckoIds)
        {
            var idsCombined = String.Join(",", geckoIds);
            return _httpClient.GetFromJsonAsync<Dictionary<string, Dictionary<string, decimal>>>("https://api.coingecko.com/api/v3/simple/price?ids=" + idsCombined + "&vs_currencies=usd");
        }
    }
}
