using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace NethereumExplorer.Services
{
    public interface IWeb3ProviderService
    {
        string CurrentUrl { get; set; }
        string ChainId { get; }
        Web3 GetWeb3();
        Web3 GetWeb3(Account account);
    }
}
