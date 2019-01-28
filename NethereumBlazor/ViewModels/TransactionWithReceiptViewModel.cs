using System.Numerics;
using System.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;
using NethereumBlazor.Services;
using ReactiveUI;

namespace NethereumBlazor.ViewModels
{
    public class TransactionWithReceiptViewModel : TransactionViewModel
    {
        private readonly IWeb3ProviderService _web3ProviderService;


        private bool _transactionFound;
        public bool TransactionFound
        {
            get { return _transactionFound; }
            set { this.RaiseAndSetIfChanged(ref _transactionFound, value); }
        }

        private bool _loading;
        public bool Loading
        {
            get { return _loading; }
            set { this.RaiseAndSetIfChanged(ref _loading, value); }
        }
        private bool _hasErrors;

        public bool HasErrors
        {
            get => _hasErrors;

            set => this.RaiseAndSetIfChanged(ref _hasErrors, value);
        }

        private string _logs;

        public string Logs
        {
            get => _logs;

            set => this.RaiseAndSetIfChanged(ref _logs, value);
        }

        private BigInteger? _cumulativeGasUsed;

        public BigInteger? CumulativeGasUsed
        {
            get => _cumulativeGasUsed;

            set => this.RaiseAndSetIfChanged(ref _cumulativeGasUsed, value);
        }

        private string _contractAddress;

        public string ContractAddress
        {
            get => _contractAddress;

            set => this.RaiseAndSetIfChanged(ref _contractAddress, value);
        }

        public TransactionWithReceiptViewModel(IWeb3ProviderService web3ProviderService)
        {
            _web3ProviderService = web3ProviderService;
        }

        public async Task LoadTransactionAsync(string transactionHash)
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

        public void Initialise(TransactionReceipt transactionReceipt)
        {
            CumulativeGasUsed = transactionReceipt.CumulativeGasUsed;
            ContractAddress = transactionReceipt.ContractAddress;
            Logs = transactionReceipt.Logs.ToString();
            HasErrors = transactionReceipt.HasErrors().HasValue ? transactionReceipt.HasErrors().Value : false;
        }
    }
}
