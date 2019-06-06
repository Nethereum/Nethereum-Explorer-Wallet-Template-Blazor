using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using ReactiveUI;

namespace NethereumBlazor.Services
{
    public class AccountsTransactionMonitoringService
    {
        private readonly IAccountsService _accountsService;
        private readonly IWeb3ProviderService _web3ProviderService;
        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(1000);
        private IDisposable _timer;

        public AccountsTransactionMonitoringService(IAccountsService accountsService, IWeb3ProviderService web3ProviderService)
        {
            _accountsService = accountsService;
            _web3ProviderService = web3ProviderService;
            _timer = Observable.Timer(TimeSpan.FromMilliseconds(500), _updateInterval, RxApp.MainThreadScheduler)
                .Select(_ => ProcessCompletedTransactions().ToObservable()).Concat().Subscribe();
        }

        public async Task ProcessCompletedTransactions()
        {
            var pendingTransactions = _accountsService.GetCurrentChainPendingTransactions();
            var web3 = _web3ProviderService.GetWeb3();
            foreach (var pendingTransaction in pendingTransactions)
            {
                var transactionReceipt = await
                    web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(pendingTransaction.TransactionHash);

                if (transactionReceipt != null)
                {
                    pendingTransaction.TransactionReceipt = transactionReceipt;
                    pendingTransaction.Pending = false;
                    _accountsService.UpdateTransactionInfo(pendingTransaction);
                }
            }
        }
    }
}