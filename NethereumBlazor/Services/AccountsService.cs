using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using DynamicData;
using Microsoft.Extensions.Primitives;
using Microsoft.JSInterop;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using NethereumBlazor.Messages;
using NethereumBlazor.Model;
using ReactiveUI;

namespace NethereumBlazor.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly IWeb3ProviderService _web3ProviderService;
        
        public SourceCache<AccountInfo, string> Accounts { get; set; }
        private ConcurrentDictionary<string, string> _privateKeyStorage;
        

        public AccountsService(IWeb3ProviderService web3ProviderService)
        {
            _web3ProviderService = web3ProviderService;
            
            _privateKeyStorage = new ConcurrentDictionary<string, string>();
            Accounts = new SourceCache<AccountInfo, string>(x => x.Address.ToLowerInvariant());
        }

        public IEnumerable<string> GetAccountsAddresses()
        {
            return Accounts.Items.Select(x => x.Address);
        }

        //Better save to local file instead as people may use this as a real wallet.
        //public void SaveToLocalStorage(string encryptionKey)
        //{
        //    _localStorage.SetItem(LOCAL_STORAGE_PRIVATE_KEY, Json.Serialize(_privateKeyStorage));
        //}

        //public void LoadFromLocalStorage(string encryptionKey)
        //{
        //    var output = _localStorage.GetItem(LOCAL_STORAGE_PRIVATE_KEY);
        //    var values = Json.Deserialize<Dictionary<string, string>>(output);
        //    foreach (var value in values)
        //    {
        //        _privateKeyStorage.TryAdd(value.Key, value.Value);
        //        var accountInfo = new AccountInfo(){Address = value.Key};
        //        Accounts.AddOrUpdate(accountInfo);
        //    }
        //}

        public void AddAccount(AccountInfo accountInfo, string privateKey)
        {
            Accounts.AddOrUpdate(accountInfo);
            _privateKeyStorage.TryAdd(accountInfo.Address.ToLowerInvariant(), privateKey);
        }

        public async Task<decimal> GetAccountEtherBalanceAsync(string address)
        {
            var web3 = _web3ProviderService.GetWeb3();
            var balance = await web3.Eth.GetBalance.SendRequestAsync(address);
            return Web3.Convert.FromWei(balance.Value);
        }

        public async Task<decimal> GetAccountTokenBalanceAsync(string address, string contractAddress, int numberOfDecimals = 18)
        {
            var web3 = _web3ProviderService.GetWeb3();
            var balance = await web3.Eth.GetContractQueryHandler<BalanceOfFunction>()
                .QueryAsync<BigInteger>(contractAddress, new BalanceOfFunction() {Owner = address});
            return Web3.Convert.FromWei(balance, numberOfDecimals);
        }

        public List<TransactionInfo> GetCurrentChainPendingTransactions()
        {
            return Accounts.Items.SelectMany(x => x.GetPendingTransactions(_web3ProviderService.ChainId)).ToList();
        }

        public void UpdateTransactionInfo(TransactionInfo transactionInfo)
        {
            var accountInfo = Accounts.Lookup(transactionInfo.AccountAddress.ToLowerInvariant());
            if (accountInfo.HasValue)
            {
                accountInfo.Value.AddOrUpdateTransaction(transactionInfo);
                MessageBus.Current.SendMessage(new AccountTransactionCompleted(transactionInfo.TransactionHash, transactionInfo.AccountAddress, transactionInfo.TransactionReceipt.HasErrors()));
            }
        }

        public async Task<string> SendTransactionAsync(TransactionInput transactionInput)
        {
            var accountInfo = Accounts.Lookup(transactionInput.From.ToLowerInvariant());
            if (accountInfo.HasValue)
            {
                var hasPrivateKey = _privateKeyStorage.TryGetValue(transactionInput.From.ToLowerInvariant(), out var privateKey);
                if (hasPrivateKey)
                {
        
                    var web3 = _web3ProviderService.GetWeb3(new Account(privateKey));
                    var txnHash = await web3.Eth.TransactionManager.SendTransactionAsync(transactionInput).ConfigureAwait(false);
                    accountInfo.Value.AddOrUpdateTransaction(new TransactionInfo()
                    {
                        AccountAddress = accountInfo.Value.Address,
                        ChainId = _web3ProviderService.ChainId,
                        TransactionHash = txnHash,
                        Pending = true
                    });
                    return txnHash;
                }
            }

            throw new ArgumentException($@"Account address: {transactionInput.From}, not found", nameof(transactionInput));   
        }

        public async Task<string> SendTransactionAsync<TFunctionMessage>(string contractAddress, TFunctionMessage functionMessage) where TFunctionMessage : FunctionMessage, new()
        {
            var accountInfo = Accounts.Lookup(functionMessage.FromAddress.ToLowerInvariant());
            if (accountInfo.HasValue)
            {
                var hasPrivateKey = _privateKeyStorage.TryGetValue(functionMessage.FromAddress.ToLowerInvariant(), out var privateKey);
                if (hasPrivateKey)
                {

                    var web3 = _web3ProviderService.GetWeb3(new Account(privateKey));
                    var txnHash = await web3.Eth.GetContractTransactionHandler<TFunctionMessage>()
                        .SendRequestAsync(contractAddress, functionMessage).ConfigureAwait(false);
                    accountInfo.Value.AddOrUpdateTransaction(new TransactionInfo()
                    {
                        AccountAddress = accountInfo.Value.Address,
                        ChainId = _web3ProviderService.ChainId,
                        TransactionHash = txnHash,
                        Pending = true
                    });
                    return txnHash;
                }
            }
            throw new ArgumentException($@"Account address: {functionMessage.FromAddress}, not found", nameof(functionMessage));
        }
    }

    public partial class BalanceOfFunction : BalanceOfFunctionBase { }

    [Function("balanceOf", "uint256")]
    public class BalanceOfFunctionBase : FunctionMessage
    {
        [Parameter("address", "_owner", 1)]
        public virtual string Owner { get; set; }
    }


    public partial class TransferFunction : TransferFunctionBase { }

    [Function("transfer", "bool")]
    public class TransferFunctionBase : FunctionMessage
    {
        [Parameter("address", "_to", 1)]
        public virtual string To { get; set; }
        [Parameter("uint256", "_value", 2)]
        public virtual BigInteger Value { get; set; }
    }
}
