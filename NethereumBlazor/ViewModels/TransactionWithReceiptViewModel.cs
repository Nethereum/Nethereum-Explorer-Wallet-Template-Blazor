using System.Numerics;
using System.Reactive;
using System.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;
using NethereumBlazor.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace NethereumBlazor.ViewModels
{
    public class TransactionWithReceiptViewModel : TransactionViewModel
    {
        private readonly IWeb3ProviderService _web3ProviderService;

        [Reactive]
        public bool TransactionFound { get; set; }

        [Reactive]
        public bool Loading { get; set; }

        [Reactive]
        public bool HasErrors { get; set; }

        [Reactive]
        public string Logs { get; set; }

        [Reactive]
        public BigInteger? CumulativeGasUsed { get; set; }

        [Reactive]
        public string ContractAddress { get; set; }

        public ReactiveCommand<string, Unit> LoadTransactionCommand { get; }

        public TransactionWithReceiptViewModel(IWeb3ProviderService web3ProviderService = null)
        {
            _web3ProviderService = web3ProviderService ?? Locator.Current.GetService<IWeb3ProviderService>();

            LoadTransactionCommand = ReactiveCommand.CreateFromTask<string>(LoadTransactionAsync, outputScheduler: RxApp.TaskpoolScheduler);
        }

        private async Task LoadTransactionAsync(string transactionHash)
        {
            try
            {
                Loading = true;
                var web3 = _web3ProviderService.GetWeb3();
                var transaction = await web3.Eth.Transactions.GetTransactionByHash.SendRequestAsync(transactionHash)
                    .ConfigureAwait(false);

                if (transaction != null)
                {
                    TransactionFound = true;
                    var transactionReceipt = await web3.Eth.Transactions.GetTransactionReceipt
                        .SendRequestAsync(transactionHash)
                        .ConfigureAwait(false);
                    Initialise(transaction);
                    if (transactionReceipt != null)
                    {
                        Initialise(transactionReceipt);
                    }
                }
                else
                {
                    TransactionFound = false;
                }
            }
            finally
            {
                Loading = false;
            }
        }

        private void Initialise(TransactionReceipt transactionReceipt)
        {
            CumulativeGasUsed = transactionReceipt.CumulativeGasUsed;
            ContractAddress = transactionReceipt.ContractAddress;
            Logs = transactionReceipt.Logs.ToString();
            HasErrors = transactionReceipt.HasErrors().HasValue ? transactionReceipt.HasErrors().Value : false;
        }
    }
}
