using System.Collections.Generic;
using System.Threading.Tasks;
using DynamicData;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using NethereumBlazor.Model;

namespace NethereumBlazor.Services
{
    public interface IAccountsService
    {
        IEnumerable<string> GetAccountsAddresses();
        Task<decimal> GetAccountEtherBalanceAsync(string address);
        SourceCache<AccountInfo, string> Accounts { get; set; }
        void AddAccount(AccountInfo accountInfo, string privateKey);
        Task<string> SendTransactionAsync(TransactionInput transactionInput);
        Task<string> SendTransactionAsync<TFunctionMessage>(string contractAddress, TFunctionMessage functionMessage) where TFunctionMessage : FunctionMessage, new();
        List<TransactionInfo> GetCurrentChainPendingTransactions();
        void UpdateTransactionInfo(TransactionInfo transactionInfo);
        Task<decimal> GetAccountTokenBalanceAsync(string address, string contractAddress, int numberOfDecimals = 18);
    }
}
